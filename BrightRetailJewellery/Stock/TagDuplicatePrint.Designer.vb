<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TagDuplicatePrint
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
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.dtpTagRecDate = New BrighttechPack.DatePicker(Me.components)
        Me.txtLotNo_NUM = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtItemId_NUM = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtTagNoFrom = New System.Windows.Forms.TextBox()
        Me.txtTagNoTo = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txttray = New System.Windows.Forms.TextBox()
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbCounter = New System.Windows.Forms.ComboBox()
        Me.txtEstNo = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbsubitem = New System.Windows.Forms.ComboBox()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGen = New System.Windows.Forms.TabPage()
        Me.cmbCalType = New System.Windows.Forms.ComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.dgvView = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnViewSearch = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.dgvView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDate.Checked = True
        Me.chkDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDate.Location = New System.Drawing.Point(10, 8)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(123, 17)
        Me.chkDate.TabIndex = 0
        Me.chkDate.Text = "Tag Receipt Date"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'dtpTagRecDate
        '
        Me.dtpTagRecDate.Location = New System.Drawing.Point(137, 6)
        Me.dtpTagRecDate.Mask = "##-##-####"
        Me.dtpTagRecDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTagRecDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTagRecDate.Name = "dtpTagRecDate"
        Me.dtpTagRecDate.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpTagRecDate.Size = New System.Drawing.Size(100, 21)
        Me.dtpTagRecDate.TabIndex = 1
        Me.dtpTagRecDate.Text = "18-03-9998"
        Me.dtpTagRecDate.Value = New Date(9998, 3, 18, 0, 0, 0, 0)
        '
        'txtLotNo_NUM
        '
        Me.txtLotNo_NUM.Location = New System.Drawing.Point(137, 38)
        Me.txtLotNo_NUM.Name = "txtLotNo_NUM"
        Me.txtLotNo_NUM.Size = New System.Drawing.Size(100, 21)
        Me.txtLotNo_NUM.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 42)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Lot No"
        '
        'txtItemId_NUM
        '
        Me.txtItemId_NUM.Location = New System.Drawing.Point(322, 38)
        Me.txtItemId_NUM.Name = "txtItemId_NUM"
        Me.txtItemId_NUM.Size = New System.Drawing.Size(102, 21)
        Me.txtItemId_NUM.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(243, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Item Id"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(10, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Tag No From"
        '
        'txtTagNoFrom
        '
        Me.txtTagNoFrom.Location = New System.Drawing.Point(137, 68)
        Me.txtTagNoFrom.Name = "txtTagNoFrom"
        Me.txtTagNoFrom.Size = New System.Drawing.Size(100, 21)
        Me.txtTagNoFrom.TabIndex = 11
        '
        'txtTagNoTo
        '
        Me.txtTagNoTo.Location = New System.Drawing.Point(322, 68)
        Me.txtTagNoTo.Name = "txtTagNoTo"
        Me.txtTagNoTo.Size = New System.Drawing.Size(102, 21)
        Me.txtTagNoTo.TabIndex = 13
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(243, 72)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(20, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "To"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(134, 120)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 23
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(116, 447)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 25
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(328, 447)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 27
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(11, 157)
        Me.gridView.Name = "gridView"
        Me.gridView.Size = New System.Drawing.Size(677, 284)
        Me.gridView.TabIndex = 28
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(10, 447)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(100, 30)
        Me.btnGenerate.TabIndex = 24
        Me.btnGenerate.Text = "&Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'chkSelectAll
        '
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Location = New System.Drawing.Point(11, 130)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkSelectAll.TabIndex = 22
        Me.chkSelectAll.Text = "Select All"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.OpenToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 70)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(427, 72)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(32, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Tray"
        '
        'txttray
        '
        Me.txttray.Location = New System.Drawing.Point(488, 68)
        Me.txttray.Name = "txttray"
        Me.txttray.Size = New System.Drawing.Size(200, 21)
        Me.txttray.TabIndex = 15
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(322, 6)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(366, 21)
        Me.cmbCostCentre.TabIndex = 3
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(243, 4)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(81, 21)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "Cost Centre"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(10, 99)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(53, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Counter"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCounter
        '
        Me.cmbCounter.FormattingEnabled = True
        Me.cmbCounter.Location = New System.Drawing.Point(137, 95)
        Me.cmbCounter.Name = "cmbCounter"
        Me.cmbCounter.Size = New System.Drawing.Size(287, 21)
        Me.cmbCounter.TabIndex = 17
        '
        'txtEstNo
        '
        Me.txtEstNo.Location = New System.Drawing.Point(488, 95)
        Me.txtEstNo.Name = "txtEstNo"
        Me.txtEstNo.Size = New System.Drawing.Size(200, 21)
        Me.txtEstNo.TabIndex = 19
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(427, 99)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(43, 13)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Est No"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(427, 42)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(56, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "SubItem"
        '
        'cmbsubitem
        '
        Me.cmbsubitem.FormattingEnabled = True
        Me.cmbsubitem.Location = New System.Drawing.Point(488, 38)
        Me.cmbsubitem.Name = "cmbsubitem"
        Me.cmbsubitem.Size = New System.Drawing.Size(200, 21)
        Me.cmbsubitem.TabIndex = 9
        '
        'tabMain
        '
        Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(704, 512)
        Me.tabMain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.cmbCalType)
        Me.tabGen.Controls.Add(Me.Label12)
        Me.tabGen.Controls.Add(Me.btnOpen)
        Me.tabGen.Controls.Add(Me.btnSearch)
        Me.tabGen.Controls.Add(Me.gridView)
        Me.tabGen.Controls.Add(Me.btnExit)
        Me.tabGen.Controls.Add(Me.chkDate)
        Me.tabGen.Controls.Add(Me.btnNew)
        Me.tabGen.Controls.Add(Me.btnGenerate)
        Me.tabGen.Controls.Add(Me.cmbsubitem)
        Me.tabGen.Controls.Add(Me.dtpTagRecDate)
        Me.tabGen.Controls.Add(Me.Label9)
        Me.tabGen.Controls.Add(Me.txtLotNo_NUM)
        Me.tabGen.Controls.Add(Me.txtEstNo)
        Me.tabGen.Controls.Add(Me.Label1)
        Me.tabGen.Controls.Add(Me.Label8)
        Me.tabGen.Controls.Add(Me.txtItemId_NUM)
        Me.tabGen.Controls.Add(Me.cmbCounter)
        Me.tabGen.Controls.Add(Me.txtTagNoFrom)
        Me.tabGen.Controls.Add(Me.Label6)
        Me.tabGen.Controls.Add(Me.txtTagNoTo)
        Me.tabGen.Controls.Add(Me.cmbCostCentre)
        Me.tabGen.Controls.Add(Me.Label2)
        Me.tabGen.Controls.Add(Me.Label7)
        Me.tabGen.Controls.Add(Me.Label3)
        Me.tabGen.Controls.Add(Me.Label5)
        Me.tabGen.Controls.Add(Me.Label4)
        Me.tabGen.Controls.Add(Me.chkSelectAll)
        Me.tabGen.Controls.Add(Me.txttray)
        Me.tabGen.Location = New System.Drawing.Point(4, 25)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(696, 483)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'cmbCalType
        '
        Me.cmbCalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCalType.FormattingEnabled = True
        Me.cmbCalType.Location = New System.Drawing.Point(488, 123)
        Me.cmbCalType.Name = "cmbCalType"
        Me.cmbCalType.Size = New System.Drawing.Size(200, 21)
        Me.cmbCalType.TabIndex = 21
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(427, 126)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(57, 13)
        Me.Label12.TabIndex = 20
        Me.Label12.Text = "Cal Type"
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(222, 447)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 26
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.dgvView)
        Me.tabView.Controls.Add(Me.Panel1)
        Me.tabView.Location = New System.Drawing.Point(4, 25)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(696, 483)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'dgvView
        '
        Me.dgvView.AllowUserToAddRows = False
        Me.dgvView.AllowUserToDeleteRows = False
        Me.dgvView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvView.Location = New System.Drawing.Point(3, 78)
        Me.dgvView.Name = "dgvView"
        Me.dgvView.Size = New System.Drawing.Size(690, 402)
        Me.dgvView.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.btnViewSearch)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(690, 75)
        Me.Panel1.TabIndex = 0
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(175, 14)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(51, 13)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Date To"
        '
        'btnViewSearch
        '
        Me.btnViewSearch.Location = New System.Drawing.Point(73, 38)
        Me.btnViewSearch.Name = "btnViewSearch"
        Me.btnViewSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnViewSearch.TabIndex = 4
        Me.btnViewSearch.Text = "&Search"
        Me.btnViewSearch.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(5, 13)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(67, 13)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Date From"
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(73, 11)
        Me.dtpFrom.Mask = "##-##-####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpFrom.Size = New System.Drawing.Size(100, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "18-03-9998"
        Me.dtpFrom.Value = New Date(9998, 3, 18, 0, 0, 0, 0)
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(229, 11)
        Me.dtpTo.Mask = "##-##-####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpTo.Size = New System.Drawing.Size(100, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "18-03-9998"
        Me.dtpTo.Value = New Date(9998, 3, 18, 0, 0, 0, 0)
        '
        'TagDuplicatePrint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(704, 512)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "TagDuplicatePrint"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tag Duplicate Print"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.tabGen.PerformLayout()
        Me.tabView.ResumeLayout(False)
        CType(Me.dgvView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTagRecDate As BrighttechPack.DatePicker
    Friend WithEvents txtLotNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtItemId_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtTagNoFrom As System.Windows.Forms.TextBox
    Friend WithEvents txtTagNoTo As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txttray As System.Windows.Forms.TextBox
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbCounter As System.Windows.Forms.ComboBox
    Friend WithEvents txtEstNo As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cmbsubitem As System.Windows.Forms.ComboBox
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnViewSearch As System.Windows.Forms.Button
    Friend WithEvents dgvView As System.Windows.Forms.DataGridView
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbCalType As ComboBox
    Friend WithEvents Label12 As Label
End Class
