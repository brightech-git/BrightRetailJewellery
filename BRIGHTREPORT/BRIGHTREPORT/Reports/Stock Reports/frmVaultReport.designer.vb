<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVaultReport
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
        Me.pnlGridTot = New System.Windows.Forms.Panel()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.chkSummary = New System.Windows.Forms.CheckBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.pnlGridHeading = New System.Windows.Forms.Panel()
        Me.btnSendSMS = New System.Windows.Forms.Button()
        Me.ChkCmbMetal = New BrighttechPack.CheckedComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.LblCostcentre = New System.Windows.Forms.Label()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.gridHead = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.pnlHeader.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        CType(Me.gridHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.pnlGridTot)
        Me.pnlHeader.Controls.Add(Me.pnlGrid)
        Me.pnlHeader.Controls.Add(Me.chkSummary)
        Me.pnlHeader.Controls.Add(Me.Panel2)
        Me.pnlHeader.Controls.Add(Me.btnSendSMS)
        Me.pnlHeader.Controls.Add(Me.ChkCmbMetal)
        Me.pnlHeader.Controls.Add(Me.Label2)
        Me.pnlHeader.Controls.Add(Me.lblTitle)
        Me.pnlHeader.Controls.Add(Me.btnPrint)
        Me.pnlHeader.Controls.Add(Me.Label1)
        Me.pnlHeader.Controls.Add(Me.dtpFrom)
        Me.pnlHeader.Controls.Add(Me.chkCmbCostCentre)
        Me.pnlHeader.Controls.Add(Me.btnView_Search)
        Me.pnlHeader.Controls.Add(Me.LblCostcentre)
        Me.pnlHeader.Controls.Add(Me.btnNew)
        Me.pnlHeader.Controls.Add(Me.btnExit)
        Me.pnlHeader.Controls.Add(Me.btnExport)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(1028, 138)
        Me.pnlHeader.TabIndex = 0
        '
        'pnlGridTot
        '
        Me.pnlGridTot.Location = New System.Drawing.Point(806, 14)
        Me.pnlGridTot.Name = "pnlGridTot"
        Me.pnlGridTot.Size = New System.Drawing.Size(35, 41)
        Me.pnlGridTot.TabIndex = 0
        Me.pnlGridTot.Visible = False
        '
        'pnlGrid
        '
        Me.pnlGrid.Location = New System.Drawing.Point(847, 14)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(35, 41)
        Me.pnlGrid.TabIndex = 1
        Me.pnlGrid.Visible = False
        '
        'chkSummary
        '
        Me.chkSummary.AutoSize = True
        Me.chkSummary.Location = New System.Drawing.Point(320, 62)
        Me.chkSummary.Name = "chkSummary"
        Me.chkSummary.Size = New System.Drawing.Size(82, 17)
        Me.chkSummary.TabIndex = 6
        Me.chkSummary.Text = "Summary"
        Me.chkSummary.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.pnlGridHeading)
        Me.Panel2.Location = New System.Drawing.Point(806, 56)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(76, 30)
        Me.Panel2.TabIndex = 3
        Me.Panel2.Visible = False
        '
        'pnlGridHeading
        '
        Me.pnlGridHeading.Location = New System.Drawing.Point(803, 0)
        Me.pnlGridHeading.Name = "pnlGridHeading"
        Me.pnlGridHeading.Size = New System.Drawing.Size(225, 75)
        Me.pnlGridHeading.TabIndex = 1
        '
        'btnSendSMS
        '
        Me.btnSendSMS.Location = New System.Drawing.Point(591, 83)
        Me.btnSendSMS.Name = "btnSendSMS"
        Me.btnSendSMS.Size = New System.Drawing.Size(100, 30)
        Me.btnSendSMS.TabIndex = 12
        Me.btnSendSMS.Text = "Send SMS"
        Me.btnSendSMS.UseVisualStyleBackColor = True
        '
        'ChkCmbMetal
        '
        Me.ChkCmbMetal.CheckOnClick = True
        Me.ChkCmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkCmbMetal.DropDownHeight = 1
        Me.ChkCmbMetal.FormattingEnabled = True
        Me.ChkCmbMetal.IntegralHeight = False
        Me.ChkCmbMetal.Location = New System.Drawing.Point(92, 58)
        Me.ChkCmbMetal.Name = "ChkCmbMetal"
        Me.ChkCmbMetal.Size = New System.Drawing.Size(224, 22)
        Me.ChkCmbMetal.TabIndex = 5
        Me.ChkCmbMetal.ValueSeparator = ", "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 62)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Metal"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblTitle.Location = New System.Drawing.Point(0, 114)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1028, 24)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(391, 83)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 10
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "From Date"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(92, 9)
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
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(92, 33)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(224, 22)
        Me.chkCmbCostCentre.TabIndex = 3
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(91, 83)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 7
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'LblCostcentre
        '
        Me.LblCostcentre.AutoSize = True
        Me.LblCostcentre.Location = New System.Drawing.Point(13, 37)
        Me.LblCostcentre.Name = "LblCostcentre"
        Me.LblCostcentre.Size = New System.Drawing.Size(76, 13)
        Me.LblCostcentre.TabIndex = 2
        Me.LblCostcentre.Text = "&Cost Centre"
        Me.LblCostcentre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(191, 83)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 8
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(491, 83)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 11
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(291, 83)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 9
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
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
        Me.gridView.Size = New System.Drawing.Size(1028, 455)
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
        'gridHead
        '
        Me.gridHead.AllowUserToAddRows = False
        Me.gridHead.AllowUserToDeleteRows = False
        Me.gridHead.BackgroundColor = System.Drawing.SystemColors.ButtonShadow
        Me.gridHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridHead.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridHead.Enabled = False
        Me.gridHead.Location = New System.Drawing.Point(0, 0)
        Me.gridHead.Name = "gridHead"
        Me.gridHead.ReadOnly = True
        Me.gridHead.RowHeadersVisible = False
        Me.gridHead.Size = New System.Drawing.Size(1028, 20)
        Me.gridHead.TabIndex = 1
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
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1028, 616)
        Me.Panel1.TabIndex = 2
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.Panel6)
        Me.Panel4.Controls.Add(Me.Panel5)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 141)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1028, 475)
        Me.Panel4.TabIndex = 2
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.gridView)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel6.Location = New System.Drawing.Point(0, 20)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(1028, 455)
        Me.Panel6.TabIndex = 1
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.gridHead)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(1028, 20)
        Me.Panel5.TabIndex = 0
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.pnlHeader)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1028, 141)
        Me.Panel3.TabIndex = 1
        '
        'frmVaultReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 616)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmVaultReport"
        Me.Text = "Vault Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        CType(Me.gridHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LblCostcentre As System.Windows.Forms.Label
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridHead As System.Windows.Forms.DataGridView
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ChkCmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents btnSendSMS As System.Windows.Forms.Button
    Friend WithEvents chkSummary As System.Windows.Forms.CheckBox
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents pnlGridTot As System.Windows.Forms.Panel
    Friend WithEvents pnlGridHeading As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
End Class
