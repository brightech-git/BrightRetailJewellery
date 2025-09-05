<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCounterwiseSales
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.grpSumDet = New System.Windows.Forms.GroupBox()
        Me.rbtdetailed = New System.Windows.Forms.RadioButton()
        Me.rbtSummary = New System.Windows.Forms.RadioButton()
        Me.chkLstItemCounter = New System.Windows.Forms.CheckedListBox()
        Me.chkLstNodeId = New System.Windows.Forms.CheckedListBox()
        Me.chkLstCashCounter = New System.Windows.Forms.CheckedListBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.grpinsrep = New System.Windows.Forms.GroupBox()
        Me.ChkSubItem = New System.Windows.Forms.CheckBox()
        Me.chkWithApp = New System.Windows.Forms.CheckBox()
        Me.grbwt = New System.Windows.Forms.GroupBox()
        Me.rbtNetwt = New System.Windows.Forms.RadioButton()
        Me.rbtGrswt = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.lbltitle = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabmain = New System.Windows.Forms.TabControl()
        Me.tabGen = New System.Windows.Forms.TabPage()
        Me.grbInput = New System.Windows.Forms.GroupBox()
        Me.cmbHallmarkFilter = New System.Windows.Forms.ComboBox()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.Panel1.SuspendLayout()
        Me.grpSumDet.SuspendLayout()
        Me.grpinsrep.SuspendLayout()
        Me.grbwt.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabmain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.grbInput.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnBack)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(3, 454)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1014, 40)
        Me.Panel1.TabIndex = 2
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(577, 3)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 20
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(683, 3)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 18
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(789, 3)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 19
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'grpSumDet
        '
        Me.grpSumDet.Controls.Add(Me.rbtdetailed)
        Me.grpSumDet.Controls.Add(Me.rbtSummary)
        Me.grpSumDet.Location = New System.Drawing.Point(18, 289)
        Me.grpSumDet.Name = "grpSumDet"
        Me.grpSumDet.Size = New System.Drawing.Size(172, 35)
        Me.grpSumDet.TabIndex = 14
        Me.grpSumDet.TabStop = False
        '
        'rbtdetailed
        '
        Me.rbtdetailed.AutoSize = True
        Me.rbtdetailed.Location = New System.Drawing.Point(92, 13)
        Me.rbtdetailed.Name = "rbtdetailed"
        Me.rbtdetailed.Size = New System.Drawing.Size(72, 17)
        Me.rbtdetailed.TabIndex = 1
        Me.rbtdetailed.Text = "Detailed"
        Me.rbtdetailed.UseVisualStyleBackColor = True
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.Checked = True
        Me.rbtSummary.Location = New System.Drawing.Point(8, 13)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 0
        Me.rbtSummary.TabStop = True
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'chkLstItemCounter
        '
        Me.chkLstItemCounter.FormattingEnabled = True
        Me.chkLstItemCounter.Location = New System.Drawing.Point(341, 169)
        Me.chkLstItemCounter.Name = "chkLstItemCounter"
        Me.chkLstItemCounter.Size = New System.Drawing.Size(130, 116)
        Me.chkLstItemCounter.TabIndex = 13
        '
        'chkLstNodeId
        '
        Me.chkLstNodeId.FormattingEnabled = True
        Me.chkLstNodeId.Location = New System.Drawing.Point(341, 73)
        Me.chkLstNodeId.Name = "chkLstNodeId"
        Me.chkLstNodeId.Size = New System.Drawing.Size(130, 68)
        Me.chkLstNodeId.TabIndex = 7
        '
        'chkLstCashCounter
        '
        Me.chkLstCashCounter.FormattingEnabled = True
        Me.chkLstCashCounter.Location = New System.Drawing.Point(152, 169)
        Me.chkLstCashCounter.Name = "chkLstCashCounter"
        Me.chkLstCashCounter.Size = New System.Drawing.Size(183, 116)
        Me.chkLstCashCounter.TabIndex = 11
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(16, 169)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(130, 116)
        Me.chkLstCostCentre.TabIndex = 9
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(198, 374)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(90, 30)
        Me.btnExit.TabIndex = 21
        Me.btnExit.Text = "Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(107, 374)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(90, 30)
        Me.btnNew.TabIndex = 20
        Me.btnNew.Text = "New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(16, 374)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(90, 30)
        Me.btnView_Search.TabIndex = 19
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'grpinsrep
        '
        Me.grpinsrep.Controls.Add(Me.ChkSubItem)
        Me.grpinsrep.Controls.Add(Me.chkWithApp)
        Me.grpinsrep.Location = New System.Drawing.Point(19, 333)
        Me.grpinsrep.Name = "grpinsrep"
        Me.grpinsrep.Size = New System.Drawing.Size(249, 35)
        Me.grpinsrep.TabIndex = 16
        Me.grpinsrep.TabStop = False
        '
        'ChkSubItem
        '
        Me.ChkSubItem.AutoSize = True
        Me.ChkSubItem.Location = New System.Drawing.Point(133, 12)
        Me.ChkSubItem.Name = "ChkSubItem"
        Me.ChkSubItem.Size = New System.Drawing.Size(102, 17)
        Me.ChkSubItem.TabIndex = 1
        Me.ChkSubItem.Text = "With Subitem"
        Me.ChkSubItem.UseVisualStyleBackColor = True
        '
        'chkWithApp
        '
        Me.chkWithApp.AutoSize = True
        Me.chkWithApp.Location = New System.Drawing.Point(5, 13)
        Me.chkWithApp.Name = "chkWithApp"
        Me.chkWithApp.Size = New System.Drawing.Size(106, 17)
        Me.chkWithApp.TabIndex = 0
        Me.chkWithApp.Text = "With Approval"
        Me.chkWithApp.UseVisualStyleBackColor = True
        '
        'grbwt
        '
        Me.grbwt.Controls.Add(Me.rbtNetwt)
        Me.grbwt.Controls.Add(Me.rbtGrswt)
        Me.grbwt.Location = New System.Drawing.Point(195, 288)
        Me.grbwt.Name = "grbwt"
        Me.grbwt.Size = New System.Drawing.Size(144, 35)
        Me.grbwt.TabIndex = 15
        Me.grbwt.TabStop = False
        '
        'rbtNetwt
        '
        Me.rbtNetwt.AutoSize = True
        Me.rbtNetwt.Location = New System.Drawing.Point(79, 13)
        Me.rbtNetwt.Name = "rbtNetwt"
        Me.rbtNetwt.Size = New System.Drawing.Size(63, 17)
        Me.rbtNetwt.TabIndex = 1
        Me.rbtNetwt.Text = "Net Wt"
        Me.rbtNetwt.UseVisualStyleBackColor = True
        '
        'rbtGrswt
        '
        Me.rbtGrswt.AutoSize = True
        Me.rbtGrswt.Checked = True
        Me.rbtGrswt.Location = New System.Drawing.Point(9, 13)
        Me.rbtGrswt.Name = "rbtGrswt"
        Me.rbtGrswt.Size = New System.Drawing.Size(64, 17)
        Me.rbtGrswt.TabIndex = 0
        Me.rbtGrswt.TabStop = True
        Me.rbtGrswt.Text = "Grs Wt"
        Me.rbtGrswt.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(210, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(341, 153)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(84, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Item Counter"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(341, 57)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(52, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Node Id"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(152, 153)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(86, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Cash Counter"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(16, 153)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Cost Centre"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(3, 29)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(1014, 425)
        Me.gridView.TabIndex = 8
        '
        'lbltitle
        '
        Me.lbltitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbltitle.Location = New System.Drawing.Point(3, 3)
        Me.lbltitle.Name = "lbltitle"
        Me.lbltitle.Size = New System.Drawing.Size(1014, 26)
        Me.lbltitle.TabIndex = 9
        Me.lbltitle.Text = "Title"
        Me.lbltitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
        'tabmain
        '
        Me.tabmain.Controls.Add(Me.tabGen)
        Me.tabmain.Controls.Add(Me.tabView)
        Me.tabmain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabmain.Location = New System.Drawing.Point(0, 0)
        Me.tabmain.Name = "tabmain"
        Me.tabmain.SelectedIndex = 0
        Me.tabmain.Size = New System.Drawing.Size(1028, 523)
        Me.tabmain.TabIndex = 10
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.grbInput)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1020, 497)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "General"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'grbInput
        '
        Me.grbInput.Controls.Add(Me.cmbHallmarkFilter)
        Me.grbInput.Controls.Add(Me.Label36)
        Me.grbInput.Controls.Add(Me.chkCompanySelectAll)
        Me.grbInput.Controls.Add(Me.chkLstCompany)
        Me.grbInput.Controls.Add(Me.dtpTo)
        Me.grbInput.Controls.Add(Me.dtpFrom)
        Me.grbInput.Controls.Add(Me.chkLstNodeId)
        Me.grbInput.Controls.Add(Me.btnExit)
        Me.grbInput.Controls.Add(Me.grpSumDet)
        Me.grbInput.Controls.Add(Me.Label1)
        Me.grbInput.Controls.Add(Me.btnNew)
        Me.grbInput.Controls.Add(Me.chkLstItemCounter)
        Me.grbInput.Controls.Add(Me.btnView_Search)
        Me.grbInput.Controls.Add(Me.Label3)
        Me.grbInput.Controls.Add(Me.Label4)
        Me.grbInput.Controls.Add(Me.chkLstCashCounter)
        Me.grbInput.Controls.Add(Me.Label5)
        Me.grbInput.Controls.Add(Me.chkLstCostCentre)
        Me.grbInput.Controls.Add(Me.Label6)
        Me.grbInput.Controls.Add(Me.Label2)
        Me.grbInput.Controls.Add(Me.grbwt)
        Me.grbInput.Controls.Add(Me.grpinsrep)
        Me.grbInput.Location = New System.Drawing.Point(199, 30)
        Me.grbInput.Name = "grbInput"
        Me.grbInput.Size = New System.Drawing.Size(485, 420)
        Me.grbInput.TabIndex = 0
        Me.grbInput.TabStop = False
        '
        'cmbHallmarkFilter
        '
        Me.cmbHallmarkFilter.FormattingEnabled = True
        Me.cmbHallmarkFilter.Items.AddRange(New Object() {"PNAME", "ADDRESS1", "AREA", "CITY", "STATE", "MOBILE"})
        Me.cmbHallmarkFilter.Location = New System.Drawing.Point(335, 341)
        Me.cmbHallmarkFilter.Name = "cmbHallmarkFilter"
        Me.cmbHallmarkFilter.Size = New System.Drawing.Size(136, 21)
        Me.cmbHallmarkFilter.TabIndex = 18
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(271, 345)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(58, 13)
        Me.Label36.TabIndex = 17
        Me.Label36.Text = "Hallmark"
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(19, 50)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 4
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(19, 73)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(315, 68)
        Me.chkLstCompany.TabIndex = 5
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(238, 20)
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
        Me.dtpFrom.Location = New System.Drawing.Point(104, 20)
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
        'tabView
        '
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.Panel1)
        Me.tabView.Controls.Add(Me.lbltitle)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1020, 497)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'frmCounterwiseSales
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 523)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabmain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmCounterwiseSales"
        Me.Text = "frmCounterwiseSales"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.grpSumDet.ResumeLayout(False)
        Me.grpSumDet.PerformLayout()
        Me.grpinsrep.ResumeLayout(False)
        Me.grpinsrep.PerformLayout()
        Me.grbwt.ResumeLayout(False)
        Me.grbwt.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabmain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.grbInput.ResumeLayout(False)
        Me.grbInput.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grbwt As System.Windows.Forms.GroupBox
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lbltitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rbtNetwt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGrswt As System.Windows.Forms.RadioButton
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstNodeId As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstCashCounter As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents grpinsrep As System.Windows.Forms.GroupBox
    Friend WithEvents chkWithApp As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstItemCounter As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents grpSumDet As System.Windows.Forms.GroupBox
    Friend WithEvents rbtdetailed As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents tabmain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents grbInput As System.Windows.Forms.GroupBox
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents ChkSubItem As System.Windows.Forms.CheckBox
    Friend WithEvents cmbHallmarkFilter As ComboBox
    Friend WithEvents Label36 As Label
End Class
