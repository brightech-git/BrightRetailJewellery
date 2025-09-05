<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTrialBal_Summary
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
        Me.pnlHeader = New System.Windows.Forms.Panel
        Me.pnlContainer = New System.Windows.Forms.Panel
        Me.pnlCostCentre = New System.Windows.Forms.Panel
        Me.chkCmbCostCentre = New GiritechPack.CheckedComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.pnlDate = New System.Windows.Forms.Panel
        Me.dtpTo = New GiritechPack.DatePicker(Me.components)
        Me.lblTo = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.chkAsonDate = New System.Windows.Forms.CheckBox
        Me.dtpFrom = New GiritechPack.DatePicker(Me.components)
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.chkTransaction = New System.Windows.Forms.CheckBox
        Me.chkOpening = New System.Windows.Forms.CheckBox
        Me.btnExport = New System.Windows.Forms.Button
        Me.cmbOrderBy = New System.Windows.Forms.ComboBox
        Me.cmbRequired = New System.Windows.Forms.ComboBox
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.pnlGrid = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlGridTot = New System.Windows.Forms.Panel
        Me.gridTot = New System.Windows.Forms.DataGridView
        Me.pnlGridHeading = New System.Windows.Forms.Panel
        Me.gridHead = New System.Windows.Forms.DataGridView
        Me.lblTitle = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.chkoldformat = New System.Windows.Forms.CheckBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.chkMore = New System.Windows.Forms.CheckBox
        Me.pnlHeader.SuspendLayout()
        Me.pnlContainer.SuspendLayout()
        Me.pnlCostCentre.SuspendLayout()
        Me.pnlDate.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        Me.pnlGridTot.SuspendLayout()
        CType(Me.gridTot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGridHeading.SuspendLayout()
        CType(Me.gridHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.pnlContainer)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(1028, 42)
        Me.pnlHeader.TabIndex = 0
        '
        'pnlContainer
        '
        Me.pnlContainer.Controls.Add(Me.pnlCostCentre)
        Me.pnlContainer.Controls.Add(Me.pnlDate)
        Me.pnlContainer.Controls.Add(Me.Panel3)
        Me.pnlContainer.Location = New System.Drawing.Point(12, 10)
        Me.pnlContainer.Name = "pnlContainer"
        Me.pnlContainer.Size = New System.Drawing.Size(655, 27)
        Me.pnlContainer.TabIndex = 0
        '
        'pnlCostCentre
        '
        Me.pnlCostCentre.Controls.Add(Me.chkCmbCostCentre)
        Me.pnlCostCentre.Controls.Add(Me.Label3)
        Me.pnlCostCentre.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlCostCentre.Location = New System.Drawing.Point(323, 0)
        Me.pnlCostCentre.Name = "pnlCostCentre"
        Me.pnlCostCentre.Size = New System.Drawing.Size(332, 27)
        Me.pnlCostCentre.TabIndex = 2
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(87, 2)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(224, 22)
        Me.chkCmbCostCentre.TabIndex = 1
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(3, 3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 21)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "&Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlDate
        '
        Me.pnlDate.Controls.Add(Me.dtpTo)
        Me.pnlDate.Controls.Add(Me.lblTo)
        Me.pnlDate.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlDate.Location = New System.Drawing.Point(197, 0)
        Me.pnlDate.Name = "pnlDate"
        Me.pnlDate.Size = New System.Drawing.Size(126, 27)
        Me.pnlDate.TabIndex = 1
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(31, 2)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 1
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblTo
        '
        Me.lblTo.Location = New System.Drawing.Point(3, 3)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(24, 21)
        Me.lblTo.TabIndex = 0
        Me.lblTo.Text = "To"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.chkAsonDate)
        Me.Panel3.Controls.Add(Me.dtpFrom)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(197, 27)
        Me.Panel3.TabIndex = 0
        '
        'chkAsonDate
        '
        Me.chkAsonDate.Location = New System.Drawing.Point(3, 5)
        Me.chkAsonDate.Name = "chkAsonDate"
        Me.chkAsonDate.Size = New System.Drawing.Size(87, 17)
        Me.chkAsonDate.TabIndex = 0
        Me.chkAsonDate.Text = "As OnDate"
        Me.chkAsonDate.UseVisualStyleBackColor = True
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(98, 2)
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
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(546, 4)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 4
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(219, 4)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 1
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'chkTransaction
        '
        Me.chkTransaction.AutoSize = True
        Me.chkTransaction.Location = New System.Drawing.Point(187, 56)
        Me.chkTransaction.Name = "chkTransaction"
        Me.chkTransaction.Size = New System.Drawing.Size(92, 17)
        Me.chkTransaction.TabIndex = 6
        Me.chkTransaction.Text = "Transaction"
        Me.chkTransaction.UseVisualStyleBackColor = True
        '
        'chkOpening
        '
        Me.chkOpening.AutoSize = True
        Me.chkOpening.Location = New System.Drawing.Point(111, 56)
        Me.chkOpening.Name = "chkOpening"
        Me.chkOpening.Size = New System.Drawing.Size(73, 17)
        Me.chkOpening.TabIndex = 5
        Me.chkOpening.Text = "Opening"
        Me.chkOpening.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(328, 4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 2
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'cmbOrderBy
        '
        Me.cmbOrderBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOrderBy.FormattingEnabled = True
        Me.cmbOrderBy.Location = New System.Drawing.Point(111, 3)
        Me.cmbOrderBy.Name = "cmbOrderBy"
        Me.cmbOrderBy.Size = New System.Drawing.Size(254, 21)
        Me.cmbOrderBy.TabIndex = 1
        '
        'cmbRequired
        '
        Me.cmbRequired.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRequired.FormattingEnabled = True
        Me.cmbRequired.Location = New System.Drawing.Point(111, 29)
        Me.cmbRequired.Name = "cmbRequired"
        Me.cmbRequired.Size = New System.Drawing.Size(121, 21)
        Me.cmbRequired.TabIndex = 3
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(110, 4)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 0
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(437, 4)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 3
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Re&quired"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(13, 58)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(78, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Closing With"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "&Order By"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.pnlGrid)
        Me.Panel2.Controls.Add(Me.pnlGridTot)
        Me.Panel2.Controls.Add(Me.pnlGridHeading)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 159)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1028, 457)
        Me.Panel2.TabIndex = 3
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.gridView)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(0, 44)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1028, 377)
        Me.pnlGrid.TabIndex = 3
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
        Me.gridView.Size = New System.Drawing.Size(1028, 377)
        Me.gridView.TabIndex = 0
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(143, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'pnlGridTot
        '
        Me.pnlGridTot.Controls.Add(Me.gridTot)
        Me.pnlGridTot.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlGridTot.Location = New System.Drawing.Point(0, 421)
        Me.pnlGridTot.Name = "pnlGridTot"
        Me.pnlGridTot.Size = New System.Drawing.Size(1028, 36)
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
        Me.gridTot.Size = New System.Drawing.Size(1028, 36)
        Me.gridTot.TabIndex = 0
        '
        'pnlGridHeading
        '
        Me.pnlGridHeading.Controls.Add(Me.gridHead)
        Me.pnlGridHeading.Controls.Add(Me.lblTitle)
        Me.pnlGridHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlGridHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlGridHeading.Name = "pnlGridHeading"
        Me.pnlGridHeading.Size = New System.Drawing.Size(1028, 44)
        Me.pnlGridHeading.TabIndex = 1
        '
        'gridHead
        '
        Me.gridHead.AllowUserToAddRows = False
        Me.gridHead.AllowUserToDeleteRows = False
        Me.gridHead.BackgroundColor = System.Drawing.SystemColors.ButtonShadow
        Me.gridHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridHead.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridHead.Enabled = False
        Me.gridHead.Location = New System.Drawing.Point(0, 24)
        Me.gridHead.Name = "gridHead"
        Me.gridHead.ReadOnly = True
        Me.gridHead.RowHeadersVisible = False
        Me.gridHead.Size = New System.Drawing.Size(1028, 20)
        Me.gridHead.TabIndex = 1
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1028, 24)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkoldformat)
        Me.Panel1.Controls.Add(Me.chkTransaction)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.chkOpening)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.cmbOrderBy)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cmbRequired)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 42)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1028, 78)
        Me.Panel1.TabIndex = 1
        Me.Panel1.Visible = False
        '
        'chkoldformat
        '
        Me.chkoldformat.AutoSize = True
        Me.chkoldformat.Location = New System.Drawing.Point(495, 54)
        Me.chkoldformat.Name = "chkoldformat"
        Me.chkoldformat.Size = New System.Drawing.Size(87, 17)
        Me.chkoldformat.TabIndex = 7
        Me.chkoldformat.Text = "Old format"
        Me.chkoldformat.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.chkMore)
        Me.Panel4.Controls.Add(Me.btnPrint)
        Me.Panel4.Controls.Add(Me.btnView_Search)
        Me.Panel4.Controls.Add(Me.btnNew)
        Me.Panel4.Controls.Add(Me.btnExit)
        Me.Panel4.Controls.Add(Me.btnExport)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 120)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1028, 39)
        Me.Panel4.TabIndex = 2
        '
        'chkMore
        '
        Me.chkMore.AutoSize = True
        Me.chkMore.Location = New System.Drawing.Point(656, 10)
        Me.chkMore.Name = "chkMore"
        Me.chkMore.Size = New System.Drawing.Size(54, 17)
        Me.chkMore.TabIndex = 5
        Me.chkMore.Text = "More"
        Me.chkMore.UseVisualStyleBackColor = True
        '
        'frmTrailBal_Summary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 616)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlHeader)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmTrailBal_Summary"
        Me.Text = "Trial Balance"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlContainer.ResumeLayout(False)
        Me.pnlCostCentre.ResumeLayout(False)
        Me.pnlDate.ResumeLayout(False)
        Me.pnlDate.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.pnlGrid.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.pnlGridTot.ResumeLayout(False)
        CType(Me.gridTot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGridHeading.ResumeLayout(False)
        CType(Me.gridHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
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
    Friend WithEvents cmbRequired As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbOrderBy As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents pnlGridHeading As System.Windows.Forms.Panel
    Friend WithEvents pnlGridTot As System.Windows.Forms.Panel
    Friend WithEvents gridTot As System.Windows.Forms.DataGridView
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents pnlDate As System.Windows.Forms.Panel
    Friend WithEvents pnlCostCentre As System.Windows.Forms.Panel
    Friend WithEvents pnlContainer As System.Windows.Forms.Panel
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents chkTransaction As System.Windows.Forms.CheckBox
    Friend WithEvents chkOpening As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridHead As System.Windows.Forms.DataGridView
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents chkMore As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As GiritechPack.DatePicker
    Friend WithEvents dtpFrom As GiritechPack.DatePicker
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkCmbCostCentre As GiritechPack.CheckedComboBox
    Friend WithEvents chkoldformat As System.Windows.Forms.CheckBox
End Class
