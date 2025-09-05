<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReorderStockPieceandWeightwise
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbCostCenter = New System.Windows.Forms.ComboBox()
        Me.chkItemCounterSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstItemCounter = New System.Windows.Forms.CheckedListBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.rbtItemTag = New System.Windows.Forms.RadioButton()
        Me.cmbxGrpby = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.chkSubitemAll = New System.Windows.Forms.CheckBox()
        Me.chkLstSubitem = New System.Windows.Forms.CheckedListBox()
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.chkLstItem = New System.Windows.Forms.CheckedListBox()
        Me.chkItemSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstDesigner = New System.Windows.Forms.CheckedListBox()
        Me.chkDesignerSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstMetal = New System.Windows.Forms.CheckedListBox()
        Me.lblAsonDate = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Tabmain = New System.Windows.Forms.TabControl()
        Me.tabgeneral = New System.Windows.Forms.TabPage()
        Me.tabview = New System.Windows.Forms.TabPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.GridView = New System.Windows.Forms.DataGridView()
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.gridViewHead = New System.Windows.Forms.DataGridView()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.lbltitle = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.cmbSize = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grpContainer.SuspendLayout()
        Me.Tabmain.SuspendLayout()
        Me.tabgeneral.SuspendLayout()
        Me.tabview.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel5.SuspendLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.cmbSize)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.cmbCostCenter)
        Me.grpContainer.Controls.Add(Me.chkItemCounterSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstItemCounter)
        Me.grpContainer.Controls.Add(Me.Label8)
        Me.grpContainer.Controls.Add(Me.rbtItemTag)
        Me.grpContainer.Controls.Add(Me.cmbxGrpby)
        Me.grpContainer.Controls.Add(Me.Label7)
        Me.grpContainer.Controls.Add(Me.chkSubitemAll)
        Me.grpContainer.Controls.Add(Me.chkLstSubitem)
        Me.grpContainer.Controls.Add(Me.dtpAsOnDate)
        Me.grpContainer.Controls.Add(Me.chkLstItem)
        Me.grpContainer.Controls.Add(Me.chkItemSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstDesigner)
        Me.grpContainer.Controls.Add(Me.chkDesignerSelectAll)
        Me.grpContainer.Controls.Add(Me.chkMetalSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstMetal)
        Me.grpContainer.Controls.Add(Me.lblAsonDate)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Location = New System.Drawing.Point(230, 15)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(497, 580)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(10, 455)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(99, 21)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "CostCentre"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCostCenter
        '
        Me.cmbCostCenter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCostCenter.FormattingEnabled = True
        Me.cmbCostCenter.Location = New System.Drawing.Point(120, 456)
        Me.cmbCostCenter.Name = "cmbCostCenter"
        Me.cmbCostCenter.Size = New System.Drawing.Size(369, 21)
        Me.cmbCostCenter.TabIndex = 15
        '
        'chkItemCounterSelectAll
        '
        Me.chkItemCounterSelectAll.AutoSize = True
        Me.chkItemCounterSelectAll.Location = New System.Drawing.Point(258, 308)
        Me.chkItemCounterSelectAll.Name = "chkItemCounterSelectAll"
        Me.chkItemCounterSelectAll.Size = New System.Drawing.Size(103, 17)
        Me.chkItemCounterSelectAll.TabIndex = 10
        Me.chkItemCounterSelectAll.Text = "Item Counter"
        Me.chkItemCounterSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstItemCounter
        '
        Me.chkLstItemCounter.CheckOnClick = True
        Me.chkLstItemCounter.FormattingEnabled = True
        Me.chkLstItemCounter.Location = New System.Drawing.Point(258, 326)
        Me.chkLstItemCounter.Name = "chkLstItemCounter"
        Me.chkLstItemCounter.Size = New System.Drawing.Size(231, 100)
        Me.chkLstItemCounter.TabIndex = 11
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(10, 483)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(99, 21)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Search By"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rbtItemTag
        '
        Me.rbtItemTag.AutoSize = True
        Me.rbtItemTag.Checked = True
        Me.rbtItemTag.Location = New System.Drawing.Point(120, 483)
        Me.rbtItemTag.Name = "rbtItemTag"
        Me.rbtItemTag.Size = New System.Drawing.Size(72, 17)
        Me.rbtItemTag.TabIndex = 17
        Me.rbtItemTag.TabStop = True
        Me.rbtItemTag.Text = "ItemTag"
        Me.rbtItemTag.UseVisualStyleBackColor = True
        '
        'cmbxGrpby
        '
        Me.cmbxGrpby.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxGrpby.FormattingEnabled = True
        Me.cmbxGrpby.Location = New System.Drawing.Point(119, 506)
        Me.cmbxGrpby.Name = "cmbxGrpby"
        Me.cmbxGrpby.Size = New System.Drawing.Size(369, 21)
        Me.cmbxGrpby.TabIndex = 19
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(14, 506)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(99, 21)
        Me.Label7.TabIndex = 18
        Me.Label7.Text = "Group By"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkSubitemAll
        '
        Me.chkSubitemAll.AutoSize = True
        Me.chkSubitemAll.Location = New System.Drawing.Point(10, 305)
        Me.chkSubitemAll.Name = "chkSubitemAll"
        Me.chkSubitemAll.Size = New System.Drawing.Size(79, 17)
        Me.chkSubitemAll.TabIndex = 8
        Me.chkSubitemAll.Text = "Sub Item"
        Me.chkSubitemAll.UseVisualStyleBackColor = True
        '
        'chkLstSubitem
        '
        Me.chkLstSubitem.CheckOnClick = True
        Me.chkLstSubitem.FormattingEnabled = True
        Me.chkLstSubitem.Location = New System.Drawing.Point(10, 323)
        Me.chkLstSubitem.Name = "chkLstSubitem"
        Me.chkLstSubitem.Size = New System.Drawing.Size(231, 100)
        Me.chkLstSubitem.TabIndex = 9
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(98, 13)
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
        'chkLstItem
        '
        Me.chkLstItem.CheckOnClick = True
        Me.chkLstItem.FormattingEnabled = True
        Me.chkLstItem.Location = New System.Drawing.Point(258, 185)
        Me.chkLstItem.Name = "chkLstItem"
        Me.chkLstItem.Size = New System.Drawing.Size(234, 116)
        Me.chkLstItem.TabIndex = 7
        '
        'chkItemSelectAll
        '
        Me.chkItemSelectAll.AutoSize = True
        Me.chkItemSelectAll.Location = New System.Drawing.Point(258, 168)
        Me.chkItemSelectAll.Name = "chkItemSelectAll"
        Me.chkItemSelectAll.Size = New System.Drawing.Size(53, 17)
        Me.chkItemSelectAll.TabIndex = 6
        Me.chkItemSelectAll.Text = "Item"
        Me.chkItemSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstDesigner
        '
        Me.chkLstDesigner.FormattingEnabled = True
        Me.chkLstDesigner.Location = New System.Drawing.Point(10, 63)
        Me.chkLstDesigner.MultiColumn = True
        Me.chkLstDesigner.Name = "chkLstDesigner"
        Me.chkLstDesigner.Size = New System.Drawing.Size(478, 100)
        Me.chkLstDesigner.TabIndex = 3
        '
        'chkDesignerSelectAll
        '
        Me.chkDesignerSelectAll.AutoSize = True
        Me.chkDesignerSelectAll.Location = New System.Drawing.Point(10, 42)
        Me.chkDesignerSelectAll.Name = "chkDesignerSelectAll"
        Me.chkDesignerSelectAll.Size = New System.Drawing.Size(77, 17)
        Me.chkDesignerSelectAll.TabIndex = 2
        Me.chkDesignerSelectAll.Text = "Designer"
        Me.chkDesignerSelectAll.UseVisualStyleBackColor = True
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(10, 167)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 4
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.CheckOnClick = True
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(10, 185)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(231, 116)
        Me.chkLstMetal.TabIndex = 5
        '
        'lblAsonDate
        '
        Me.lblAsonDate.AutoSize = True
        Me.lblAsonDate.Location = New System.Drawing.Point(10, 17)
        Me.lblAsonDate.Name = "lblAsonDate"
        Me.lblAsonDate.Size = New System.Drawing.Size(72, 13)
        Me.lblAsonDate.TabIndex = 0
        Me.lblAsonDate.Text = "As On Date"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(121, 533)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(117, 30)
        Me.btnSearch.TabIndex = 20
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(365, 533)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(117, 30)
        Me.btnExit.TabIndex = 22
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(242, 533)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(117, 30)
        Me.btnNew.TabIndex = 21
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Tabmain
        '
        Me.Tabmain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.Tabmain.Controls.Add(Me.tabgeneral)
        Me.Tabmain.Controls.Add(Me.tabview)
        Me.Tabmain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Tabmain.Location = New System.Drawing.Point(0, 0)
        Me.Tabmain.Name = "Tabmain"
        Me.Tabmain.SelectedIndex = 0
        Me.Tabmain.Size = New System.Drawing.Size(915, 674)
        Me.Tabmain.TabIndex = 0
        '
        'tabgeneral
        '
        Me.tabgeneral.Controls.Add(Me.grpContainer)
        Me.tabgeneral.Location = New System.Drawing.Point(4, 25)
        Me.tabgeneral.Name = "tabgeneral"
        Me.tabgeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabgeneral.Size = New System.Drawing.Size(907, 645)
        Me.tabgeneral.TabIndex = 0
        Me.tabgeneral.Text = "tabgeneral"
        Me.tabgeneral.UseVisualStyleBackColor = True
        '
        'tabview
        '
        Me.tabview.Controls.Add(Me.Panel2)
        Me.tabview.Location = New System.Drawing.Point(4, 25)
        Me.tabview.Name = "tabview"
        Me.tabview.Padding = New System.Windows.Forms.Padding(3)
        Me.tabview.Size = New System.Drawing.Size(907, 645)
        Me.tabview.TabIndex = 1
        Me.tabview.Text = "tabview"
        Me.tabview.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Controls.Add(Me.Panel1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(901, 639)
        Me.Panel2.TabIndex = 1
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Panel5)
        Me.Panel3.Controls.Add(Me.Panel4)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(901, 601)
        Me.Panel3.TabIndex = 14
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.GridView)
        Me.Panel5.Controls.Add(Me.gridViewHead)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel5.Location = New System.Drawing.Point(0, 44)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(901, 557)
        Me.Panel5.TabIndex = 13
        '
        'GridView
        '
        Me.GridView.AllowUserToAddRows = False
        Me.GridView.AllowUserToDeleteRows = False
        Me.GridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.GridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridView.Location = New System.Drawing.Point(0, 17)
        Me.GridView.Name = "GridView"
        Me.GridView.ReadOnly = True
        Me.GridView.Size = New System.Drawing.Size(901, 540)
        Me.GridView.TabIndex = 12
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
        'gridViewHead
        '
        Me.gridViewHead.AllowUserToAddRows = False
        Me.gridViewHead.AllowUserToDeleteRows = False
        Me.gridViewHead.AllowUserToResizeColumns = False
        Me.gridViewHead.AllowUserToResizeRows = False
        Me.gridViewHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHead.Location = New System.Drawing.Point(0, 0)
        Me.gridViewHead.Name = "gridViewHead"
        Me.gridViewHead.ReadOnly = True
        Me.gridViewHead.Size = New System.Drawing.Size(901, 17)
        Me.gridViewHead.TabIndex = 13
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.lbltitle)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(901, 44)
        Me.Panel4.TabIndex = 0
        '
        'lbltitle
        '
        Me.lbltitle.AutoSize = True
        Me.lbltitle.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltitle.Location = New System.Drawing.Point(386, 13)
        Me.lbltitle.Name = "lbltitle"
        Me.lbltitle.Size = New System.Drawing.Size(52, 14)
        Me.lbltitle.TabIndex = 0
        Me.lbltitle.Text = "Label2"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnBack)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 601)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(901, 38)
        Me.Panel1.TabIndex = 13
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(653, 5)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(117, 30)
        Me.btnExport.TabIndex = 6
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(779, 5)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(117, 30)
        Me.btnPrint.TabIndex = 7
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(527, 5)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(117, 30)
        Me.btnBack.TabIndex = 8
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'cmbSize
        '
        Me.cmbSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSize.FormattingEnabled = True
        Me.cmbSize.Location = New System.Drawing.Point(119, 429)
        Me.cmbSize.Name = "cmbSize"
        Me.cmbSize.Size = New System.Drawing.Size(122, 21)
        Me.cmbSize.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(10, 428)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(99, 21)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Size"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmReorderStockPieceandWeightwise
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(915, 674)
        Me.ContextMenuStrip = Me.cmbGridShortCut
        Me.ControlBox = False
        Me.Controls.Add(Me.Tabmain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmReorderStockPieceandWeightwise"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Piece and Weight Wise Reorder Stock"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.Tabmain.ResumeLayout(False)
        Me.tabgeneral.ResumeLayout(False)
        Me.tabview.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents chkLstItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkItemSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstDesigner As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkDesignerSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents lblAsonDate As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents chkSubitemAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstSubitem As System.Windows.Forms.CheckedListBox
    Friend WithEvents Tabmain As System.Windows.Forms.TabControl
    Friend WithEvents tabgeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabview As System.Windows.Forms.TabPage
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents lbltitle As System.Windows.Forms.Label
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents GridView As System.Windows.Forms.DataGridView
    Friend WithEvents cmbxGrpby As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents rbtItemTag As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents gridViewHead As System.Windows.Forms.DataGridView
    Friend WithEvents chkItemCounterSelectAll As CheckBox
    Friend WithEvents chkLstItemCounter As CheckedListBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbCostCenter As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbSize As ComboBox
End Class
