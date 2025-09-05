<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStaxTdsReport
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
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.btnEd = New System.Windows.Forms.Button()
        Me.btngenerate = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.rbttds = New System.Windows.Forms.RadioButton()
        Me.rbtstax = New System.Windows.Forms.RadioButton()
        Me.btnVerify = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.chktotal = New System.Windows.Forms.CheckBox()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.chkcmbacname = New BrighttechPack.CheckedComboBox()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.chkAsonDate = New System.Windows.Forms.CheckBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.lblTo = New System.Windows.Forms.Label()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.PnlRange = New System.Windows.Forms.Panel()
        Me.GrpRange = New CodeVendor.Controls.Grouper()
        Me.GridViewHelp = New System.Windows.Forms.DataGridView()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlGridTot = New System.Windows.Forms.Panel()
        Me.gridTot = New System.Windows.Forms.DataGridView()
        Me.pnlGridHeading = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlHeader.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        Me.PnlRange.SuspendLayout()
        Me.GrpRange.SuspendLayout()
        CType(Me.GridViewHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        Me.pnlGridTot.SuspendLayout()
        CType(Me.gridTot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGridHeading.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.btnEd)
        Me.pnlHeader.Controls.Add(Me.btngenerate)
        Me.pnlHeader.Controls.Add(Me.Panel3)
        Me.pnlHeader.Controls.Add(Me.btnVerify)
        Me.pnlHeader.Controls.Add(Me.btnPrint)
        Me.pnlHeader.Controls.Add(Me.chktotal)
        Me.pnlHeader.Controls.Add(Me.btnView_Search)
        Me.pnlHeader.Controls.Add(Me.chkcmbacname)
        Me.pnlHeader.Controls.Add(Me.btnNew)
        Me.pnlHeader.Controls.Add(Me.btnExit)
        Me.pnlHeader.Controls.Add(Me.Label1)
        Me.pnlHeader.Controls.Add(Me.btnExport)
        Me.pnlHeader.Controls.Add(Me.chkAsonDate)
        Me.pnlHeader.Controls.Add(Me.dtpTo)
        Me.pnlHeader.Controls.Add(Me.dtpFrom)
        Me.pnlHeader.Controls.Add(Me.lblTo)
        Me.pnlHeader.Controls.Add(Me.chkCmbCostCentre)
        Me.pnlHeader.Controls.Add(Me.Label3)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(1020, 124)
        Me.pnlHeader.TabIndex = 1
        '
        'btnEd
        '
        Me.btnEd.Location = New System.Drawing.Point(809, 88)
        Me.btnEd.Name = "btnEd"
        Me.btnEd.Size = New System.Drawing.Size(100, 30)
        Me.btnEd.TabIndex = 17
        Me.btnEd.Text = "Generate ED"
        Me.btnEd.UseVisualStyleBackColor = True
        Me.btnEd.Visible = False
        '
        'btngenerate
        '
        Me.btngenerate.Location = New System.Drawing.Point(709, 88)
        Me.btngenerate.Name = "btngenerate"
        Me.btngenerate.Size = New System.Drawing.Size(100, 30)
        Me.btngenerate.TabIndex = 16
        Me.btngenerate.Text = "Generate"
        Me.btngenerate.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rbttds)
        Me.Panel3.Controls.Add(Me.rbtstax)
        Me.Panel3.Location = New System.Drawing.Point(332, 57)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(279, 22)
        Me.Panel3.TabIndex = 9
        '
        'rbttds
        '
        Me.rbttds.AutoSize = True
        Me.rbttds.Checked = True
        Me.rbttds.Location = New System.Drawing.Point(11, 1)
        Me.rbttds.Name = "rbttds"
        Me.rbttds.Size = New System.Drawing.Size(87, 17)
        Me.rbttds.TabIndex = 0
        Me.rbttds.TabStop = True
        Me.rbttds.Text = "Tds Report"
        Me.rbttds.UseVisualStyleBackColor = True
        '
        'rbtstax
        '
        Me.rbtstax.AutoSize = True
        Me.rbtstax.Location = New System.Drawing.Point(139, 3)
        Me.rbtstax.Name = "rbtstax"
        Me.rbtstax.Size = New System.Drawing.Size(129, 17)
        Me.rbtstax.TabIndex = 1
        Me.rbtstax.Text = "ServiceTax Report"
        Me.rbtstax.UseVisualStyleBackColor = True
        '
        'btnVerify
        '
        Me.btnVerify.Location = New System.Drawing.Point(609, 88)
        Me.btnVerify.Name = "btnVerify"
        Me.btnVerify.Size = New System.Drawing.Size(100, 30)
        Me.btnVerify.TabIndex = 15
        Me.btnVerify.Text = "Verify"
        Me.btnVerify.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(509, 88)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 14
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'chktotal
        '
        Me.chktotal.AutoSize = True
        Me.chktotal.Location = New System.Drawing.Point(692, 58)
        Me.chktotal.Name = "chktotal"
        Me.chktotal.Size = New System.Drawing.Size(53, 17)
        Me.chktotal.TabIndex = 4
        Me.chktotal.Text = "Total"
        Me.chktotal.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(109, 88)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 10
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'chkcmbacname
        '
        Me.chkcmbacname.CheckOnClick = True
        Me.chkcmbacname.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbacname.DropDownHeight = 1
        Me.chkcmbacname.FormattingEnabled = True
        Me.chkcmbacname.IntegralHeight = False
        Me.chkcmbacname.Location = New System.Drawing.Point(110, 58)
        Me.chkcmbacname.Name = "chkcmbacname"
        Me.chkcmbacname.Size = New System.Drawing.Size(216, 22)
        Me.chkcmbacname.TabIndex = 8
        Me.chkcmbacname.ValueSeparator = ", "
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(209, 88)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 11
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(409, 88)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 13
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(26, 58)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 21)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "&Ac Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(309, 88)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 12
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'chkAsonDate
        '
        Me.chkAsonDate.Location = New System.Drawing.Point(16, 12)
        Me.chkAsonDate.Name = "chkAsonDate"
        Me.chkAsonDate.Size = New System.Drawing.Size(87, 17)
        Me.chkAsonDate.TabIndex = 0
        Me.chkAsonDate.Text = "As OnDate"
        Me.chkAsonDate.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(233, 9)
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
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(111, 9)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblTo
        '
        Me.lblTo.Location = New System.Drawing.Point(209, 8)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(24, 21)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(110, 33)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(216, 22)
        Me.chkCmbCostCentre.TabIndex = 6
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(25, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 21)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "&Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.pnlGrid)
        Me.Panel2.Controls.Add(Me.pnlGridTot)
        Me.Panel2.Controls.Add(Me.pnlGridHeading)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 124)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1020, 492)
        Me.Panel2.TabIndex = 3
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.PnlRange)
        Me.pnlGrid.Controls.Add(Me.gridView)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(0, 25)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1020, 431)
        Me.pnlGrid.TabIndex = 3
        '
        'PnlRange
        '
        Me.PnlRange.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.PnlRange.Controls.Add(Me.GrpRange)
        Me.PnlRange.Location = New System.Drawing.Point(201, 21)
        Me.PnlRange.Name = "PnlRange"
        Me.PnlRange.Size = New System.Drawing.Size(608, 285)
        Me.PnlRange.TabIndex = 6
        Me.PnlRange.Visible = False
        '
        'GrpRange
        '
        Me.GrpRange.BackgroundColor = System.Drawing.Color.Lavender
        Me.GrpRange.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.GrpRange.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.GrpRange.BorderColor = System.Drawing.Color.Transparent
        Me.GrpRange.BorderThickness = 1.0!
        Me.GrpRange.Controls.Add(Me.GridViewHelp)
        Me.GrpRange.CustomGroupBoxColor = System.Drawing.Color.White
        Me.GrpRange.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GrpRange.GroupImage = Nothing
        Me.GrpRange.GroupTitle = ""
        Me.GrpRange.Location = New System.Drawing.Point(0, 0)
        Me.GrpRange.Name = "GrpRange"
        Me.GrpRange.Padding = New System.Windows.Forms.Padding(20)
        Me.GrpRange.PaintGroupBox = False
        Me.GrpRange.RoundCorners = 10
        Me.GrpRange.ShadowColor = System.Drawing.Color.DarkGray
        Me.GrpRange.ShadowControl = False
        Me.GrpRange.ShadowThickness = 3
        Me.GrpRange.Size = New System.Drawing.Size(608, 285)
        Me.GrpRange.TabIndex = 4
        '
        'GridViewHelp
        '
        Me.GridViewHelp.AllowUserToAddRows = False
        Me.GridViewHelp.AllowUserToDeleteRows = False
        Me.GridViewHelp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridViewHelp.Location = New System.Drawing.Point(6, 19)
        Me.GridViewHelp.Name = "GridViewHelp"
        Me.GridViewHelp.ReadOnly = True
        Me.GridViewHelp.RowHeadersVisible = False
        Me.GridViewHelp.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GridViewHelp.RowTemplate.Height = 18
        Me.GridViewHelp.Size = New System.Drawing.Size(597, 258)
        Me.GridViewHelp.TabIndex = 1
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1020, 431)
        Me.gridView.TabIndex = 0
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(136, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'pnlGridTot
        '
        Me.pnlGridTot.Controls.Add(Me.gridTot)
        Me.pnlGridTot.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlGridTot.Location = New System.Drawing.Point(0, 456)
        Me.pnlGridTot.Name = "pnlGridTot"
        Me.pnlGridTot.Size = New System.Drawing.Size(1020, 36)
        Me.pnlGridTot.TabIndex = 2
        '
        'gridTot
        '
        Me.gridTot.AllowUserToAddRows = False
        Me.gridTot.AllowUserToDeleteRows = False
        Me.gridTot.BackgroundColor = System.Drawing.SystemColors.ControlDark
        Me.gridTot.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridTot.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridTot.ColumnHeadersVisible = False
        Me.gridTot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridTot.Location = New System.Drawing.Point(0, 0)
        Me.gridTot.Name = "gridTot"
        Me.gridTot.ReadOnly = True
        Me.gridTot.RowHeadersVisible = False
        Me.gridTot.RowTemplate.Height = 18
        Me.gridTot.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridTot.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridTot.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridTot.Size = New System.Drawing.Size(1020, 36)
        Me.gridTot.TabIndex = 0
        '
        'pnlGridHeading
        '
        Me.pnlGridHeading.Controls.Add(Me.lblTitle)
        Me.pnlGridHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlGridHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlGridHeading.Name = "pnlGridHeading"
        Me.pnlGridHeading.Size = New System.Drawing.Size(1020, 25)
        Me.pnlGridHeading.TabIndex = 1
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1020, 25)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'frmStaxTdsReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1020, 616)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.pnlHeader)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmStaxTdsReport"
        Me.Text = "ServiceTax & Tds Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.pnlGrid.ResumeLayout(False)
        Me.PnlRange.ResumeLayout(False)
        Me.GrpRange.ResumeLayout(False)
        CType(Me.GridViewHelp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.pnlGridTot.ResumeLayout(False)
        CType(Me.gridTot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGridHeading.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents chkAsonDate As System.Windows.Forms.CheckBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents pnlGridTot As System.Windows.Forms.Panel
    Friend WithEvents gridTot As System.Windows.Forms.DataGridView
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkcmbacname As BrighttechPack.CheckedComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chktotal As System.Windows.Forms.CheckBox
    Friend WithEvents pnlGridHeading As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents btnVerify As System.Windows.Forms.Button
    Friend WithEvents GridViewHelp As System.Windows.Forms.DataGridView
    Friend WithEvents PnlRange As System.Windows.Forms.Panel
    Friend WithEvents GrpRange As CodeVendor.Controls.Grouper
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents rbttds As System.Windows.Forms.RadioButton
    Friend WithEvents rbtstax As System.Windows.Forms.RadioButton
    Friend WithEvents btngenerate As System.Windows.Forms.Button
    Friend WithEvents btnEd As System.Windows.Forms.Button
End Class
