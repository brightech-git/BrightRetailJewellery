<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPendingLotView
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
        Me.grpContainer = New System.Windows.Forms.GroupBox()
        Me.ChkMultiUpdate = New System.Windows.Forms.CheckBox()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.chkAsonDate = New System.Windows.Forms.CheckBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.chkwithMinbal = New System.Windows.Forms.CheckBox()
        Me.chkOrderByItemId = New System.Windows.Forms.CheckBox()
        Me.chkDesignerSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstDesigner = New System.Windows.Forms.CheckedListBox()
        Me.chkWithPendingCompletedLot = New System.Windows.Forms.CheckBox()
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.chkItemCounterSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstMetal = New System.Windows.Forms.CheckedListBox()
        Me.chkLstItemCounter = New System.Windows.Forms.CheckedListBox()
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.ChkAll = New System.Windows.Forms.CheckBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabMain = New System.Windows.Forms.TabControl()
        Me.TabGen = New System.Windows.Forms.TabPage()
        Me.TabView = New System.Windows.Forms.TabPage()
        Me.PnlCenter = New System.Windows.Forms.Panel()
        Me.DgvView = New System.Windows.Forms.DataGridView()
        Me.PnlTop = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.PnlBottom = New System.Windows.Forms.Panel()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.grpContainer.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.TabMain.SuspendLayout()
        Me.TabGen.SuspendLayout()
        Me.TabView.SuspendLayout()
        Me.PnlCenter.SuspendLayout()
        CType(Me.DgvView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlTop.SuspendLayout()
        Me.PnlBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.ChkMultiUpdate)
        Me.grpContainer.Controls.Add(Me.lblTo)
        Me.grpContainer.Controls.Add(Me.chkAsonDate)
        Me.grpContainer.Controls.Add(Me.dtpTo)
        Me.grpContainer.Controls.Add(Me.chkwithMinbal)
        Me.grpContainer.Controls.Add(Me.chkOrderByItemId)
        Me.grpContainer.Controls.Add(Me.chkDesignerSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstDesigner)
        Me.grpContainer.Controls.Add(Me.chkWithPendingCompletedLot)
        Me.grpContainer.Controls.Add(Me.dtpAsOnDate)
        Me.grpContainer.Controls.Add(Me.chkItemCounterSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstCostCentre)
        Me.grpContainer.Controls.Add(Me.chkMetalSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstMetal)
        Me.grpContainer.Controls.Add(Me.chkLstItemCounter)
        Me.grpContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Location = New System.Drawing.Point(244, 16)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(424, 482)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'ChkMultiUpdate
        '
        Me.ChkMultiUpdate.AutoSize = True
        Me.ChkMultiUpdate.Location = New System.Drawing.Point(303, 418)
        Me.ChkMultiUpdate.Name = "ChkMultiUpdate"
        Me.ChkMultiUpdate.Size = New System.Drawing.Size(96, 17)
        Me.ChkMultiUpdate.TabIndex = 15
        Me.ChkMultiUpdate.Text = "Multi Update"
        Me.ChkMultiUpdate.UseVisualStyleBackColor = True
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(219, 18)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(20, 13)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        Me.lblTo.Visible = False
        '
        'chkAsonDate
        '
        Me.chkAsonDate.AutoSize = True
        Me.chkAsonDate.Checked = True
        Me.chkAsonDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAsonDate.Location = New System.Drawing.Point(16, 17)
        Me.chkAsonDate.Name = "chkAsonDate"
        Me.chkAsonDate.Size = New System.Drawing.Size(91, 17)
        Me.chkAsonDate.TabIndex = 0
        Me.chkAsonDate.Text = "As On Date"
        Me.chkAsonDate.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(251, 13)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(95, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        Me.dtpTo.Visible = False
        '
        'chkwithMinbal
        '
        Me.chkwithMinbal.AutoSize = True
        Me.chkwithMinbal.Location = New System.Drawing.Point(141, 418)
        Me.chkwithMinbal.Name = "chkwithMinbal"
        Me.chkwithMinbal.Size = New System.Drawing.Size(156, 17)
        Me.chkwithMinbal.TabIndex = 14
        Me.chkwithMinbal.Text = "With minimum balance"
        Me.chkwithMinbal.UseVisualStyleBackColor = True
        '
        'chkOrderByItemId
        '
        Me.chkOrderByItemId.AutoSize = True
        Me.chkOrderByItemId.Location = New System.Drawing.Point(16, 418)
        Me.chkOrderByItemId.Name = "chkOrderByItemId"
        Me.chkOrderByItemId.Size = New System.Drawing.Size(119, 17)
        Me.chkOrderByItemId.TabIndex = 13
        Me.chkOrderByItemId.Text = "Order By Itemid"
        Me.chkOrderByItemId.UseVisualStyleBackColor = True
        '
        'chkDesignerSelectAll
        '
        Me.chkDesignerSelectAll.AutoSize = True
        Me.chkDesignerSelectAll.Location = New System.Drawing.Point(224, 230)
        Me.chkDesignerSelectAll.Name = "chkDesignerSelectAll"
        Me.chkDesignerSelectAll.Size = New System.Drawing.Size(77, 17)
        Me.chkDesignerSelectAll.TabIndex = 11
        Me.chkDesignerSelectAll.Text = "Designer"
        Me.chkDesignerSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstDesigner
        '
        Me.chkLstDesigner.FormattingEnabled = True
        Me.chkLstDesigner.Location = New System.Drawing.Point(221, 248)
        Me.chkLstDesigner.Name = "chkLstDesigner"
        Me.chkLstDesigner.Size = New System.Drawing.Size(199, 164)
        Me.chkLstDesigner.TabIndex = 12
        '
        'chkWithPendingCompletedLot
        '
        Me.chkWithPendingCompletedLot.AutoSize = True
        Me.chkWithPendingCompletedLot.Location = New System.Drawing.Point(16, 45)
        Me.chkWithPendingCompletedLot.Name = "chkWithPendingCompletedLot"
        Me.chkWithPendingCompletedLot.Size = New System.Drawing.Size(187, 17)
        Me.chkWithPendingCompletedLot.TabIndex = 4
        Me.chkWithPendingCompletedLot.Text = "With Pending Completed Lot"
        Me.chkWithPendingCompletedLot.UseVisualStyleBackColor = True
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(119, 13)
        Me.dtpAsOnDate.Mask = "##/##/####"
        Me.dtpAsOnDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate.Name = "dtpAsOnDate"
        Me.dtpAsOnDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate.Size = New System.Drawing.Size(95, 21)
        Me.dtpAsOnDate.TabIndex = 1
        Me.dtpAsOnDate.Text = "07/03/9998"
        Me.dtpAsOnDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkItemCounterSelectAll
        '
        Me.chkItemCounterSelectAll.AutoSize = True
        Me.chkItemCounterSelectAll.Location = New System.Drawing.Point(16, 230)
        Me.chkItemCounterSelectAll.Name = "chkItemCounterSelectAll"
        Me.chkItemCounterSelectAll.Size = New System.Drawing.Size(103, 17)
        Me.chkItemCounterSelectAll.TabIndex = 9
        Me.chkItemCounterSelectAll.Text = "Item Counter"
        Me.chkItemCounterSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(13, 92)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 132)
        Me.chkLstCostCentre.TabIndex = 6
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(224, 73)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 7
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(221, 92)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(199, 132)
        Me.chkLstMetal.TabIndex = 8
        '
        'chkLstItemCounter
        '
        Me.chkLstItemCounter.FormattingEnabled = True
        Me.chkLstItemCounter.Location = New System.Drawing.Point(13, 248)
        Me.chkLstItemCounter.Name = "chkLstItemCounter"
        Me.chkLstItemCounter.Size = New System.Drawing.Size(201, 164)
        Me.chkLstItemCounter.TabIndex = 10
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(16, 73)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 5
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(62, 441)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 16
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(274, 441)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 18
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(168, 441)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 17
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'ChkAll
        '
        Me.ChkAll.AutoSize = True
        Me.ChkAll.Location = New System.Drawing.Point(14, 13)
        Me.ChkAll.Name = "ChkAll"
        Me.ChkAll.Size = New System.Drawing.Size(80, 17)
        Me.ChkAll.TabIndex = 15
        Me.ChkAll.Text = "Check All"
        Me.ChkAll.UseVisualStyleBackColor = True
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
        'TabMain
        '
        Me.TabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.TabMain.Controls.Add(Me.TabGen)
        Me.TabMain.Controls.Add(Me.TabView)
        Me.TabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabMain.Location = New System.Drawing.Point(0, 0)
        Me.TabMain.Name = "TabMain"
        Me.TabMain.SelectedIndex = 0
        Me.TabMain.Size = New System.Drawing.Size(863, 558)
        Me.TabMain.TabIndex = 0
        '
        'TabGen
        '
        Me.TabGen.Controls.Add(Me.grpContainer)
        Me.TabGen.Location = New System.Drawing.Point(4, 25)
        Me.TabGen.Name = "TabGen"
        Me.TabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.TabGen.Size = New System.Drawing.Size(855, 529)
        Me.TabGen.TabIndex = 0
        Me.TabGen.Text = "General"
        Me.TabGen.UseVisualStyleBackColor = True
        '
        'TabView
        '
        Me.TabView.Controls.Add(Me.PnlCenter)
        Me.TabView.Controls.Add(Me.PnlTop)
        Me.TabView.Controls.Add(Me.PnlBottom)
        Me.TabView.Location = New System.Drawing.Point(4, 25)
        Me.TabView.Name = "TabView"
        Me.TabView.Padding = New System.Windows.Forms.Padding(3)
        Me.TabView.Size = New System.Drawing.Size(855, 529)
        Me.TabView.TabIndex = 1
        Me.TabView.Text = "View"
        Me.TabView.UseVisualStyleBackColor = True
        '
        'PnlCenter
        '
        Me.PnlCenter.Controls.Add(Me.DgvView)
        Me.PnlCenter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PnlCenter.Location = New System.Drawing.Point(3, 44)
        Me.PnlCenter.Name = "PnlCenter"
        Me.PnlCenter.Size = New System.Drawing.Size(849, 444)
        Me.PnlCenter.TabIndex = 1
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
        Me.DgvView.Size = New System.Drawing.Size(849, 444)
        Me.DgvView.TabIndex = 0
        '
        'PnlTop
        '
        Me.PnlTop.Controls.Add(Me.lblTitle)
        Me.PnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.PnlTop.Location = New System.Drawing.Point(3, 3)
        Me.PnlTop.Name = "PnlTop"
        Me.PnlTop.Size = New System.Drawing.Size(849, 41)
        Me.PnlTop.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(849, 41)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Label1"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PnlBottom
        '
        Me.PnlBottom.Controls.Add(Me.btnPrint)
        Me.PnlBottom.Controls.Add(Me.btnExport)
        Me.PnlBottom.Controls.Add(Me.ChkAll)
        Me.PnlBottom.Controls.Add(Me.btnBack)
        Me.PnlBottom.Controls.Add(Me.btnUpdate)
        Me.PnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PnlBottom.Location = New System.Drawing.Point(3, 488)
        Me.PnlBottom.Name = "PnlBottom"
        Me.PnlBottom.Size = New System.Drawing.Size(849, 38)
        Me.PnlBottom.TabIndex = 2
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(498, 4)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 17
        Me.btnPrint.Text = "&Print [P]"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(384, 4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 16
        Me.btnExport.Text = "E&xport [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(609, 5)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 0
        Me.btnBack.Text = "Back[Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(723, 5)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(100, 30)
        Me.btnUpdate.TabIndex = 1
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'frmPendingLotView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(863, 558)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.TabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmPendingLotView"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pending LotView"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.TabMain.ResumeLayout(False)
        Me.TabGen.ResumeLayout(False)
        Me.TabView.ResumeLayout(False)
        Me.PnlCenter.ResumeLayout(False)
        CType(Me.DgvView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlTop.ResumeLayout(False)
        Me.PnlBottom.ResumeLayout(False)
        Me.PnlBottom.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents chkItemCounterSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstItemCounter As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents chkWithPendingCompletedLot As System.Windows.Forms.CheckBox
    Friend WithEvents chkDesignerSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstDesigner As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkOrderByItemId As System.Windows.Forms.CheckBox
    Friend WithEvents chkwithMinbal As System.Windows.Forms.CheckBox
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents chkAsonDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents ChkAll As System.Windows.Forms.CheckBox
    Friend WithEvents TabMain As System.Windows.Forms.TabControl
    Friend WithEvents TabGen As System.Windows.Forms.TabPage
    Friend WithEvents TabView As System.Windows.Forms.TabPage
    Friend WithEvents PnlCenter As System.Windows.Forms.Panel
    Friend WithEvents PnlTop As System.Windows.Forms.Panel
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents DgvView As System.Windows.Forms.DataGridView
    Friend WithEvents PnlBottom As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ChkMultiUpdate As System.Windows.Forms.CheckBox
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
End Class
