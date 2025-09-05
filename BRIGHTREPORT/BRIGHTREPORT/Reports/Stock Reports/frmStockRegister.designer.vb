<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStockRegister
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
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpMain = New System.Windows.Forms.GroupBox
        Me.ChkRate = New System.Windows.Forms.CheckBox
        Me.ChkSplitPur = New System.Windows.Forms.CheckBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rbtGs11Netwt = New System.Windows.Forms.RadioButton
        Me.rbtGs11Grswt = New System.Windows.Forms.RadioButton
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbtGs12Netwt = New System.Windows.Forms.RadioButton
        Me.rbtGs12Grswt = New System.Windows.Forms.RadioButton
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.lblTitle = New System.Windows.Forms.Label
        Me.Label = New System.Windows.Forms.Label
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.cmbMetal = New System.Windows.Forms.ComboBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.pnlView = New System.Windows.Forms.Panel
        Me.PnlRange = New System.Windows.Forms.Panel
        Me.GrpRange = New CodeVendor.Controls.Grouper
        Me.Label8 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.txtAmt = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtNetWt = New System.Windows.Forms.TextBox
        Me.txtGrsWt = New System.Windows.Forms.TextBox
        Me.GridViewDetail = New System.Windows.Forms.DataGridView
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ChkSRate = New System.Windows.Forms.CheckBox
        Me.ContextMenuStrip1.SuspendLayout()
        Me.grpMain.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlView.SuspendLayout()
        Me.PnlRange.SuspendLayout()
        Me.GrpRange.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.GridViewDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'grpMain
        '
        Me.grpMain.Controls.Add(Me.ChkSRate)
        Me.grpMain.Controls.Add(Me.ChkRate)
        Me.grpMain.Controls.Add(Me.ChkSplitPur)
        Me.grpMain.Controls.Add(Me.Label11)
        Me.grpMain.Controls.Add(Me.Label10)
        Me.grpMain.Controls.Add(Me.Label3)
        Me.grpMain.Controls.Add(Me.Label1)
        Me.grpMain.Controls.Add(Me.Panel2)
        Me.grpMain.Controls.Add(Me.Panel1)
        Me.grpMain.Controls.Add(Me.chkCmbCostCentre)
        Me.grpMain.Controls.Add(Me.lblTitle)
        Me.grpMain.Controls.Add(Me.Label)
        Me.grpMain.Controls.Add(Me.chkCmbCompany)
        Me.grpMain.Controls.Add(Me.Label9)
        Me.grpMain.Controls.Add(Me.dtpTo)
        Me.grpMain.Controls.Add(Me.dtpFrom)
        Me.grpMain.Controls.Add(Me.btnPrint)
        Me.grpMain.Controls.Add(Me.btnExport)
        Me.grpMain.Controls.Add(Me.cmbMetal)
        Me.grpMain.Controls.Add(Me.btnExit)
        Me.grpMain.Controls.Add(Me.btnNew)
        Me.grpMain.Controls.Add(Me.Label6)
        Me.grpMain.Controls.Add(Me.btnView_Search)
        Me.grpMain.Controls.Add(Me.Label2)
        Me.grpMain.Controls.Add(Me.Label5)
        Me.grpMain.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpMain.Location = New System.Drawing.Point(0, 0)
        Me.grpMain.Name = "grpMain"
        Me.grpMain.Size = New System.Drawing.Size(1010, 143)
        Me.grpMain.TabIndex = 0
        Me.grpMain.TabStop = False
        '
        'ChkRate
        '
        Me.ChkRate.AutoSize = True
        Me.ChkRate.Location = New System.Drawing.Point(586, 40)
        Me.ChkRate.Name = "ChkRate"
        Me.ChkRate.Size = New System.Drawing.Size(264, 17)
        Me.ChkRate.TabIndex = 15
        Me.ChkRate.Text = "Issue to GS12 Rate from Purchase [Gold]"
        Me.ChkRate.UseVisualStyleBackColor = True
        '
        'ChkSplitPur
        '
        Me.ChkSplitPur.AutoSize = True
        Me.ChkSplitPur.Location = New System.Drawing.Point(586, 15)
        Me.ChkSplitPur.Name = "ChkSplitPur"
        Me.ChkSplitPur.Size = New System.Drawing.Size(114, 17)
        Me.ChkSplitPur.TabIndex = 14
        Me.ChkSplitPur.Text = "Split Purchased"
        Me.ChkSplitPur.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.DodgerBlue
        Me.Label11.Location = New System.Drawing.Point(825, 25)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(181, 13)
        Me.Label11.TabIndex = 22
        Me.Label11.Text = "[Press-S]   SUMMARY VIEW"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.DodgerBlue
        Me.Label10.Location = New System.Drawing.Point(825, 10)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(180, 13)
        Me.Label10.TabIndex = 21
        Me.Label10.Text = "[Press-D]  DETAILED VIEW"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(359, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 13)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "GS12"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(359, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "GS11"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtGs11Netwt)
        Me.Panel2.Controls.Add(Me.rbtGs11Grswt)
        Me.Panel2.Location = New System.Drawing.Point(408, 12)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(172, 22)
        Me.Panel2.TabIndex = 11
        '
        'rbtGs11Netwt
        '
        Me.rbtGs11Netwt.AutoSize = True
        Me.rbtGs11Netwt.Location = New System.Drawing.Point(97, 3)
        Me.rbtGs11Netwt.Name = "rbtGs11Netwt"
        Me.rbtGs11Netwt.Size = New System.Drawing.Size(63, 17)
        Me.rbtGs11Netwt.TabIndex = 0
        Me.rbtGs11Netwt.Text = "Net Wt"
        Me.rbtGs11Netwt.UseVisualStyleBackColor = True
        '
        'rbtGs11Grswt
        '
        Me.rbtGs11Grswt.AutoSize = True
        Me.rbtGs11Grswt.Checked = True
        Me.rbtGs11Grswt.Location = New System.Drawing.Point(5, 3)
        Me.rbtGs11Grswt.Name = "rbtGs11Grswt"
        Me.rbtGs11Grswt.Size = New System.Drawing.Size(64, 17)
        Me.rbtGs11Grswt.TabIndex = 1
        Me.rbtGs11Grswt.TabStop = True
        Me.rbtGs11Grswt.Text = "Grs Wt"
        Me.rbtGs11Grswt.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtGs12Netwt)
        Me.Panel1.Controls.Add(Me.rbtGs12Grswt)
        Me.Panel1.Location = New System.Drawing.Point(408, 44)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(172, 22)
        Me.Panel1.TabIndex = 13
        '
        'rbtGs12Netwt
        '
        Me.rbtGs12Netwt.AutoSize = True
        Me.rbtGs12Netwt.Location = New System.Drawing.Point(97, 3)
        Me.rbtGs12Netwt.Name = "rbtGs12Netwt"
        Me.rbtGs12Netwt.Size = New System.Drawing.Size(63, 17)
        Me.rbtGs12Netwt.TabIndex = 0
        Me.rbtGs12Netwt.Text = "Net Wt"
        Me.rbtGs12Netwt.UseVisualStyleBackColor = True
        '
        'rbtGs12Grswt
        '
        Me.rbtGs12Grswt.AutoSize = True
        Me.rbtGs12Grswt.Checked = True
        Me.rbtGs12Grswt.Location = New System.Drawing.Point(5, 3)
        Me.rbtGs12Grswt.Name = "rbtGs12Grswt"
        Me.rbtGs12Grswt.Size = New System.Drawing.Size(64, 17)
        Me.rbtGs12Grswt.TabIndex = 1
        Me.rbtGs12Grswt.TabStop = True
        Me.rbtGs12Grswt.Text = "Grs Wt"
        Me.rbtGs12Grswt.UseVisualStyleBackColor = True
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(127, 57)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(222, 22)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'lblTitle
        '
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(2, 120)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(899, 21)
        Me.lblTitle.TabIndex = 19
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(6, 60)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 6
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(127, 33)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(222, 22)
        Me.chkCmbCompany.TabIndex = 5
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 37)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(62, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Company"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(261, 10)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(88, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(127, 10)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(88, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(783, 88)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 20
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(571, 88)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 18
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'cmbMetal
        '
        Me.cmbMetal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(127, 81)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(222, 21)
        Me.cmbMetal.TabIndex = 9
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(677, 88)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 19
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(465, 88)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 17
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(224, 9)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(25, 21)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "To"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(359, 88)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 16
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(6, 79)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(115, 21)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Metal Combination"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(6, 9)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(115, 21)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Date From"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlView
        '
        Me.pnlView.Controls.Add(Me.PnlRange)
        Me.pnlView.Controls.Add(Me.gridView)
        Me.pnlView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlView.Location = New System.Drawing.Point(0, 143)
        Me.pnlView.Name = "pnlView"
        Me.pnlView.Size = New System.Drawing.Size(1010, 453)
        Me.pnlView.TabIndex = 3
        '
        'PnlRange
        '
        Me.PnlRange.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.PnlRange.Controls.Add(Me.GrpRange)
        Me.PnlRange.Enabled = False
        Me.PnlRange.Location = New System.Drawing.Point(203, 34)
        Me.PnlRange.Name = "PnlRange"
        Me.PnlRange.Size = New System.Drawing.Size(615, 376)
        Me.PnlRange.TabIndex = 7
        Me.PnlRange.Visible = False
        '
        'GrpRange
        '
        Me.GrpRange.BackgroundColor = System.Drawing.Color.Lavender
        Me.GrpRange.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.GrpRange.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.GrpRange.BorderColor = System.Drawing.Color.Transparent
        Me.GrpRange.BorderThickness = 1.0!
        Me.GrpRange.Controls.Add(Me.Label8)
        Me.GrpRange.Controls.Add(Me.Panel3)
        Me.GrpRange.Controls.Add(Me.GridViewDetail)
        Me.GrpRange.CustomGroupBoxColor = System.Drawing.Color.White
        Me.GrpRange.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GrpRange.Enabled = False
        Me.GrpRange.GroupImage = Nothing
        Me.GrpRange.GroupTitle = ""
        Me.GrpRange.Location = New System.Drawing.Point(0, 0)
        Me.GrpRange.Name = "GrpRange"
        Me.GrpRange.Padding = New System.Windows.Forms.Padding(30)
        Me.GrpRange.PaintGroupBox = False
        Me.GrpRange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GrpRange.RoundCorners = 15
        Me.GrpRange.ShadowColor = System.Drawing.Color.DarkGray
        Me.GrpRange.ShadowControl = False
        Me.GrpRange.ShadowThickness = 3
        Me.GrpRange.Size = New System.Drawing.Size(615, 376)
        Me.GrpRange.TabIndex = 4
        Me.GrpRange.Visible = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(276, 19)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(50, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Label8"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.txtAmt)
        Me.Panel3.Controls.Add(Me.Label7)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.txtNetWt)
        Me.Panel3.Controls.Add(Me.txtGrsWt)
        Me.Panel3.Location = New System.Drawing.Point(9, 330)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(597, 26)
        Me.Panel3.TabIndex = 3
        '
        'txtAmt
        '
        Me.txtAmt.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmt.Location = New System.Drawing.Point(493, 3)
        Me.txtAmt.Name = "txtAmt"
        Me.txtAmt.ReadOnly = True
        Me.txtAmt.Size = New System.Drawing.Size(100, 21)
        Me.txtAmt.TabIndex = 4
        Me.txtAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(17, 6)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(103, 13)
        Me.Label7.TabIndex = 3
        Me.Label7.Text = "*Hit Enter to Edit"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(245, 7)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Total"
        '
        'txtNetWt
        '
        Me.txtNetWt.BackColor = System.Drawing.SystemColors.Window
        Me.txtNetWt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNetWt.Location = New System.Drawing.Point(391, 3)
        Me.txtNetWt.Name = "txtNetWt"
        Me.txtNetWt.ReadOnly = True
        Me.txtNetWt.Size = New System.Drawing.Size(100, 21)
        Me.txtNetWt.TabIndex = 1
        Me.txtNetWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtGrsWt
        '
        Me.txtGrsWt.BackColor = System.Drawing.SystemColors.Window
        Me.txtGrsWt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGrsWt.Location = New System.Drawing.Point(289, 3)
        Me.txtGrsWt.Name = "txtGrsWt"
        Me.txtGrsWt.ReadOnly = True
        Me.txtGrsWt.Size = New System.Drawing.Size(100, 21)
        Me.txtGrsWt.TabIndex = 0
        Me.txtGrsWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GridViewDetail
        '
        Me.GridViewDetail.AllowUserToAddRows = False
        Me.GridViewDetail.AllowUserToDeleteRows = False
        Me.GridViewDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridViewDetail.Location = New System.Drawing.Point(9, 42)
        Me.GridViewDetail.Name = "GridViewDetail"
        Me.GridViewDetail.ReadOnly = True
        Me.GridViewDetail.RowHeadersVisible = False
        Me.GridViewDetail.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GridViewDetail.RowTemplate.Height = 18
        Me.GridViewDetail.Size = New System.Drawing.Size(597, 282)
        Me.GridViewDetail.TabIndex = 1
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.ContextMenuStrip2
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.Name = "gridView"
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.gridView.Size = New System.Drawing.Size(1010, 453)
        Me.gridView.TabIndex = 0
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoResizeToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(133, 26)
        '
        'AutoResizeToolStripMenuItem
        '
        Me.AutoResizeToolStripMenuItem.CheckOnClick = True
        Me.AutoResizeToolStripMenuItem.Name = "AutoResizeToolStripMenuItem"
        Me.AutoResizeToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.AutoResizeToolStripMenuItem.Text = "AutoResize"
        '
        'ChkSRate
        '
        Me.ChkSRate.AutoSize = True
        Me.ChkSRate.Location = New System.Drawing.Point(586, 65)
        Me.ChkSRate.Name = "ChkSRate"
        Me.ChkSRate.Size = New System.Drawing.Size(271, 17)
        Me.ChkSRate.TabIndex = 23
        Me.ChkSRate.Text = "Issue to GS12 Rate from Purchase [Silver]"
        Me.ChkSRate.UseVisualStyleBackColor = True
        '
        'frmStockRegister
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1010, 596)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlView)
        Me.Controls.Add(Me.grpMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmStockRegister"
        Me.Text = "Stock Registers"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.grpMain.ResumeLayout(False)
        Me.grpMain.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlView.ResumeLayout(False)
        Me.PnlRange.ResumeLayout(False)
        Me.GrpRange.ResumeLayout(False)
        Me.GrpRange.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.GridViewDetail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpMain As System.Windows.Forms.GroupBox
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents pnlView As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtGs12Netwt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGs12Grswt As System.Windows.Forms.RadioButton
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtGs11Netwt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGs11Grswt As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PnlRange As System.Windows.Forms.Panel
    Friend WithEvents GrpRange As CodeVendor.Controls.Grouper
    Friend WithEvents GridViewDetail As System.Windows.Forms.DataGridView
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtNetWt As System.Windows.Forms.TextBox
    Friend WithEvents txtGrsWt As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtAmt As System.Windows.Forms.TextBox
    Friend WithEvents ChkSplitPur As System.Windows.Forms.CheckBox
    Friend WithEvents ChkRate As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSRate As System.Windows.Forms.CheckBox
End Class
