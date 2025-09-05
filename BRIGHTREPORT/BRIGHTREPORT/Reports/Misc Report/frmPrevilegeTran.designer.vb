<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrevilegetran
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
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.gridHeaderview = New System.Windows.Forms.DataGridView()
        Me.pnlHeading = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExtToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.chkPcs = New System.Windows.Forms.CheckBox()
        Me.chkGrsWt = New System.Windows.Forms.CheckBox()
        Me.chkNetWt = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TXTPREVILEGEID = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblcustname = New System.Windows.Forms.Label()
        Me.chkwithpoints = New System.Windows.Forms.CheckBox()
        Me.chkchit = New System.Windows.Forms.CheckBox()
        Me.chkchitOnly = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkSpecific = New System.Windows.Forms.CheckBox()
        Me.chkOnlyPrevilege = New System.Windows.Forms.CheckBox()
        Me.chkSummary = New System.Windows.Forms.CheckBox()
        Me.GrpDetailed = New System.Windows.Forms.GroupBox()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridHeaderview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHeading.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GrpDetailed.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridView)
        Me.Panel2.Controls.Add(Me.gridHeaderview)
        Me.Panel2.Controls.Add(Me.pnlHeading)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 122)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1105, 475)
        Me.Panel2.TabIndex = 1
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 45)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 21
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.Size = New System.Drawing.Size(1105, 430)
        Me.gridView.TabIndex = 1
        '
        'gridHeaderview
        '
        Me.gridHeaderview.AllowUserToDeleteRows = False
        Me.gridHeaderview.AllowUserToResizeColumns = False
        Me.gridHeaderview.AllowUserToResizeRows = False
        Me.gridHeaderview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridHeaderview.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridHeaderview.Location = New System.Drawing.Point(0, 25)
        Me.gridHeaderview.Name = "gridHeaderview"
        Me.gridHeaderview.Size = New System.Drawing.Size(1105, 20)
        Me.gridHeaderview.TabIndex = 0
        '
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.lblTitle)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1105, 25)
        Me.pnlHeading.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1105, 25)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExtToolStripMenuItem})
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
        'ExtToolStripMenuItem
        '
        Me.ExtToolStripMenuItem.Name = "ExtToolStripMenuItem"
        Me.ExtToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExtToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExtToolStripMenuItem.Text = "Exit"
        Me.ExtToolStripMenuItem.Visible = False
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(17, 86)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 10
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(317, 86)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 13
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(117, 86)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 11
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(14, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(92, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(14, 39)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(92, 21)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(232, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(27, 21)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(119, 47)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(176, 21)
        Me.cmbMetal.TabIndex = 3
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(217, 86)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 12
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(417, 86)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 14
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'chkPcs
        '
        Me.chkPcs.AutoSize = True
        Me.chkPcs.Location = New System.Drawing.Point(411, 22)
        Me.chkPcs.Name = "chkPcs"
        Me.chkPcs.Size = New System.Drawing.Size(62, 17)
        Me.chkPcs.TabIndex = 6
        Me.chkPcs.Text = "Pieces"
        Me.chkPcs.UseVisualStyleBackColor = True
        '
        'chkGrsWt
        '
        Me.chkGrsWt.AutoSize = True
        Me.chkGrsWt.Location = New System.Drawing.Point(411, 48)
        Me.chkGrsWt.Name = "chkGrsWt"
        Me.chkGrsWt.Size = New System.Drawing.Size(74, 17)
        Me.chkGrsWt.TabIndex = 7
        Me.chkGrsWt.Text = "GrossWt"
        Me.chkGrsWt.UseVisualStyleBackColor = True
        '
        'chkNetWt
        '
        Me.chkNetWt.AutoSize = True
        Me.chkNetWt.Location = New System.Drawing.Point(491, 47)
        Me.chkNetWt.Name = "chkNetWt"
        Me.chkNetWt.Size = New System.Drawing.Size(60, 17)
        Me.chkNetWt.TabIndex = 9
        Me.chkNetWt.Text = "NetWt"
        Me.chkNetWt.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(13, 15)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(92, 21)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Previlege Id"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TXTPREVILEGEID
        '
        Me.TXTPREVILEGEID.Location = New System.Drawing.Point(119, 18)
        Me.TXTPREVILEGEID.Name = "TXTPREVILEGEID"
        Me.TXTPREVILEGEID.Size = New System.Drawing.Size(176, 21)
        Me.TXTPREVILEGEID.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(13, 48)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(92, 21)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Metal"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblcustname
        '
        Me.lblcustname.AutoSize = True
        Me.lblcustname.ForeColor = System.Drawing.Color.Red
        Me.lblcustname.Location = New System.Drawing.Point(1034, 10)
        Me.lblcustname.Name = "lblcustname"
        Me.lblcustname.Size = New System.Drawing.Size(0, 13)
        Me.lblcustname.TabIndex = 15
        Me.lblcustname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkwithpoints
        '
        Me.chkwithpoints.AutoSize = True
        Me.chkwithpoints.Location = New System.Drawing.Point(491, 20)
        Me.chkwithpoints.Name = "chkwithpoints"
        Me.chkwithpoints.Size = New System.Drawing.Size(89, 17)
        Me.chkwithpoints.TabIndex = 8
        Me.chkwithpoints.Text = "With Points"
        Me.chkwithpoints.UseVisualStyleBackColor = True
        '
        'chkchit
        '
        Me.chkchit.AutoSize = True
        Me.chkchit.Location = New System.Drawing.Point(301, 20)
        Me.chkchit.Name = "chkchit"
        Me.chkchit.Size = New System.Drawing.Size(103, 17)
        Me.chkchit.TabIndex = 4
        Me.chkchit.Text = "With SCHEME"
        Me.chkchit.UseVisualStyleBackColor = True
        '
        'chkchitOnly
        '
        Me.chkchitOnly.AutoSize = True
        Me.chkchitOnly.Location = New System.Drawing.Point(301, 50)
        Me.chkchitOnly.Name = "chkchitOnly"
        Me.chkchitOnly.Size = New System.Drawing.Size(104, 17)
        Me.chkchitOnly.TabIndex = 5
        Me.chkchitOnly.Text = "Only SCHEME"
        Me.chkchitOnly.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkSpecific)
        Me.Panel1.Controls.Add(Me.chkOnlyPrevilege)
        Me.Panel1.Controls.Add(Me.chkSummary)
        Me.Panel1.Controls.Add(Me.GrpDetailed)
        Me.Panel1.Controls.Add(Me.lblcustname)
        Me.Panel1.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnView_Search)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1105, 122)
        Me.Panel1.TabIndex = 0
        '
        'chkSpecific
        '
        Me.chkSpecific.AutoSize = True
        Me.chkSpecific.Location = New System.Drawing.Point(111, 66)
        Me.chkSpecific.Name = "chkSpecific"
        Me.chkSpecific.Size = New System.Drawing.Size(114, 17)
        Me.chkSpecific.TabIndex = 6
        Me.chkSpecific.Text = "Specific Format"
        Me.chkSpecific.UseVisualStyleBackColor = True
        '
        'chkOnlyPrevilege
        '
        Me.chkOnlyPrevilege.AutoSize = True
        Me.chkOnlyPrevilege.Location = New System.Drawing.Point(307, 67)
        Me.chkOnlyPrevilege.Name = "chkOnlyPrevilege"
        Me.chkOnlyPrevilege.Size = New System.Drawing.Size(109, 17)
        Me.chkOnlyPrevilege.TabIndex = 8
        Me.chkOnlyPrevilege.Text = "Only Previlege"
        Me.chkOnlyPrevilege.UseVisualStyleBackColor = True
        '
        'chkSummary
        '
        Me.chkSummary.AutoSize = True
        Me.chkSummary.Location = New System.Drawing.Point(225, 67)
        Me.chkSummary.Name = "chkSummary"
        Me.chkSummary.Size = New System.Drawing.Size(82, 17)
        Me.chkSummary.TabIndex = 7
        Me.chkSummary.Text = "Summary"
        Me.chkSummary.UseVisualStyleBackColor = True
        '
        'GrpDetailed
        '
        Me.GrpDetailed.Controls.Add(Me.cmbMetal)
        Me.GrpDetailed.Controls.Add(Me.chkwithpoints)
        Me.GrpDetailed.Controls.Add(Me.chkchitOnly)
        Me.GrpDetailed.Controls.Add(Me.Label5)
        Me.GrpDetailed.Controls.Add(Me.chkNetWt)
        Me.GrpDetailed.Controls.Add(Me.chkGrsWt)
        Me.GrpDetailed.Controls.Add(Me.chkchit)
        Me.GrpDetailed.Controls.Add(Me.chkPcs)
        Me.GrpDetailed.Controls.Add(Me.TXTPREVILEGEID)
        Me.GrpDetailed.Controls.Add(Me.Label6)
        Me.GrpDetailed.Location = New System.Drawing.Point(417, 4)
        Me.GrpDetailed.Name = "GrpDetailed"
        Me.GrpDetailed.Size = New System.Drawing.Size(587, 80)
        Me.GrpDetailed.TabIndex = 9
        Me.GrpDetailed.TabStop = False
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(111, 39)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(268, 22)
        Me.chkCmbCostCentre.TabIndex = 5
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(271, 8)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(108, 21)
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
        Me.dtpFrom.Size = New System.Drawing.Size(108, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'frmPrevilegetran
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1105, 597)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmPrevilegetran"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Previlege Transaction Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridHeaderview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHeading.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GrpDetailed.ResumeLayout(False)
        Me.GrpDetailed.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExtToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnView_Search As Button
    Friend WithEvents btnExit As Button
    Friend WithEvents btnNew As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbMetal As ComboBox
    Friend WithEvents btnExport As Button
    Friend WithEvents btnPrint As Button
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkPcs As CheckBox
    Friend WithEvents chkGrsWt As CheckBox
    Friend WithEvents chkNetWt As CheckBox
    Friend WithEvents Label5 As Label
    Friend WithEvents TXTPREVILEGEID As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents lblcustname As Label
    Friend WithEvents chkwithpoints As CheckBox
    Friend WithEvents chkchit As CheckBox
    Friend WithEvents chkchitOnly As CheckBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents chkSummary As CheckBox
    Friend WithEvents GrpDetailed As GroupBox
    Friend WithEvents gridHeaderview As DataGridView
    Friend WithEvents chkOnlyPrevilege As CheckBox
    Friend WithEvents chkSpecific As CheckBox
End Class
