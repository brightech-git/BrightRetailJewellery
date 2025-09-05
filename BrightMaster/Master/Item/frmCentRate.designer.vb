<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCentRate
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
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.pnlControls = New System.Windows.Forms.Panel()
        Me.lblParty = New System.Windows.Forms.Label()
        Me.cmbParty = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbItemName_Man = New System.Windows.Forms.ComboBox()
        Me.cmbSubItemName_Man = New System.Windows.Forms.ComboBox()
        Me.lbldesigner = New System.Windows.Forms.Label()
        Me.cmbDesigner_MAN = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtMinRate_Amt = New System.Windows.Forms.TextBox()
        Me.txtCentFrom = New System.Windows.Forms.TextBox()
        Me.txtCentTo = New System.Windows.Forms.TextBox()
        Me.txtMaxRate_Amt = New System.Windows.Forms.TextBox()
        Me.txtPurchaseRate_Amt = New System.Windows.Forms.TextBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.CmbColor = New System.Windows.Forms.ComboBox()
        Me.lblCut = New System.Windows.Forms.Label()
        Me.CmbCut = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.CmbClarity = New System.Windows.Forms.ComboBox()
        Me.pnl4c = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cmbShape = New System.Windows.Forms.ComboBox()
        Me.cmbStnGroup_MAN = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.chkLstCostcentre = New System.Windows.Forms.CheckedListBox()
        Me.txtsaleper_PER = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.BtnTemplate = New System.Windows.Forms.Button()
        Me.BtnUpdate = New System.Windows.Forms.Button()
        Me.BtnImport = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlControls.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnl4c.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(12, 242)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(867, 307)
        Me.gridView.TabIndex = 23
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(20, 130)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "From Cent"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(210, 130)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(20, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "To"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(20, 155)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Max Rate"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(21, 183)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(89, 13)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "Purchase Rate"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlControls
        '
        Me.pnlControls.BackColor = System.Drawing.Color.Transparent
        Me.pnlControls.Controls.Add(Me.lblParty)
        Me.pnlControls.Controls.Add(Me.cmbParty)
        Me.pnlControls.Controls.Add(Me.Label1)
        Me.pnlControls.Controls.Add(Me.Label2)
        Me.pnlControls.Controls.Add(Me.cmbItemName_Man)
        Me.pnlControls.Controls.Add(Me.cmbSubItemName_Man)
        Me.pnlControls.Location = New System.Drawing.Point(15, 32)
        Me.pnlControls.Name = "pnlControls"
        Me.pnlControls.Size = New System.Drawing.Size(373, 84)
        Me.pnlControls.TabIndex = 2
        '
        'lblParty
        '
        Me.lblParty.AutoSize = True
        Me.lblParty.Location = New System.Drawing.Point(3, 6)
        Me.lblParty.Name = "lblParty"
        Me.lblParty.Size = New System.Drawing.Size(40, 13)
        Me.lblParty.TabIndex = 0
        Me.lblParty.Text = "Name"
        Me.lblParty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbParty
        '
        Me.cmbParty.FormattingEnabled = True
        Me.cmbParty.Location = New System.Drawing.Point(103, 3)
        Me.cmbParty.Name = "cmbParty"
        Me.cmbParty.Size = New System.Drawing.Size(266, 21)
        Me.cmbParty.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Item Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Sub Item Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItemName_Man
        '
        Me.cmbItemName_Man.FormattingEnabled = True
        Me.cmbItemName_Man.Location = New System.Drawing.Point(103, 31)
        Me.cmbItemName_Man.Name = "cmbItemName_Man"
        Me.cmbItemName_Man.Size = New System.Drawing.Size(266, 21)
        Me.cmbItemName_Man.TabIndex = 3
        '
        'cmbSubItemName_Man
        '
        Me.cmbSubItemName_Man.FormattingEnabled = True
        Me.cmbSubItemName_Man.Location = New System.Drawing.Point(103, 58)
        Me.cmbSubItemName_Man.Name = "cmbSubItemName_Man"
        Me.cmbSubItemName_Man.Size = New System.Drawing.Size(266, 21)
        Me.cmbSubItemName_Man.TabIndex = 5
        '
        'lbldesigner
        '
        Me.lbldesigner.AutoSize = True
        Me.lbldesigner.Location = New System.Drawing.Point(22, 9)
        Me.lbldesigner.Name = "lbldesigner"
        Me.lbldesigner.Size = New System.Drawing.Size(58, 13)
        Me.lbldesigner.TabIndex = 0
        Me.lbldesigner.Text = "Designer"
        Me.lbldesigner.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbDesigner_MAN
        '
        Me.cmbDesigner_MAN.FormattingEnabled = True
        Me.cmbDesigner_MAN.Location = New System.Drawing.Point(118, 6)
        Me.cmbDesigner_MAN.Name = "cmbDesigner_MAN"
        Me.cmbDesigner_MAN.Size = New System.Drawing.Size(266, 21)
        Me.cmbDesigner_MAN.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(210, 156)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Min Rate"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMinRate_Amt
        '
        Me.txtMinRate_Amt.Location = New System.Drawing.Point(290, 152)
        Me.txtMinRate_Amt.MaxLength = 14
        Me.txtMinRate_Amt.Name = "txtMinRate_Amt"
        Me.txtMinRate_Amt.Size = New System.Drawing.Size(94, 21)
        Me.txtMinRate_Amt.TabIndex = 11
        '
        'txtCentFrom
        '
        Me.txtCentFrom.Location = New System.Drawing.Point(118, 126)
        Me.txtCentFrom.MaxLength = 9
        Me.txtCentFrom.Name = "txtCentFrom"
        Me.txtCentFrom.Size = New System.Drawing.Size(86, 21)
        Me.txtCentFrom.TabIndex = 5
        Me.txtCentFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCentTo
        '
        Me.txtCentTo.Location = New System.Drawing.Point(290, 126)
        Me.txtCentTo.MaxLength = 9
        Me.txtCentTo.Name = "txtCentTo"
        Me.txtCentTo.Size = New System.Drawing.Size(94, 21)
        Me.txtCentTo.TabIndex = 7
        Me.txtCentTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMaxRate_Amt
        '
        Me.txtMaxRate_Amt.Location = New System.Drawing.Point(118, 151)
        Me.txtMaxRate_Amt.MaxLength = 14
        Me.txtMaxRate_Amt.Name = "txtMaxRate_Amt"
        Me.txtMaxRate_Amt.Size = New System.Drawing.Size(86, 21)
        Me.txtMaxRate_Amt.TabIndex = 9
        '
        'txtPurchaseRate_Amt
        '
        Me.txtPurchaseRate_Amt.Location = New System.Drawing.Point(119, 179)
        Me.txtPurchaseRate_Amt.MaxLength = 14
        Me.txtPurchaseRate_Amt.Name = "txtPurchaseRate_Amt"
        Me.txtPurchaseRate_Amt.Size = New System.Drawing.Size(86, 21)
        Me.txtPurchaseRate_Amt.TabIndex = 13
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
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
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(12, 552)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 22
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(231, 207)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 20
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(21, 207)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 18
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(336, 207)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 21
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(126, 207)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 19
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(441, 207)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 22
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(5, 63)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(45, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Colour"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CmbColor
        '
        Me.CmbColor.FormattingEnabled = True
        Me.CmbColor.Location = New System.Drawing.Point(63, 59)
        Me.CmbColor.Name = "CmbColor"
        Me.CmbColor.Size = New System.Drawing.Size(161, 21)
        Me.CmbColor.TabIndex = 5
        '
        'lblCut
        '
        Me.lblCut.AutoSize = True
        Me.lblCut.Location = New System.Drawing.Point(5, 38)
        Me.lblCut.Name = "lblCut"
        Me.lblCut.Size = New System.Drawing.Size(27, 13)
        Me.lblCut.TabIndex = 2
        Me.lblCut.Text = "Cut"
        Me.lblCut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CmbCut
        '
        Me.CmbCut.FormattingEnabled = True
        Me.CmbCut.Location = New System.Drawing.Point(63, 34)
        Me.CmbCut.Name = "CmbCut"
        Me.CmbCut.Size = New System.Drawing.Size(161, 21)
        Me.CmbCut.TabIndex = 3
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(5, 88)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(45, 13)
        Me.Label10.TabIndex = 6
        Me.Label10.Text = "Clarity"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CmbClarity
        '
        Me.CmbClarity.FormattingEnabled = True
        Me.CmbClarity.Location = New System.Drawing.Point(63, 84)
        Me.CmbClarity.Name = "CmbClarity"
        Me.CmbClarity.Size = New System.Drawing.Size(161, 21)
        Me.CmbClarity.TabIndex = 7
        '
        'pnl4c
        '
        Me.pnl4c.Controls.Add(Me.Label14)
        Me.pnl4c.Controls.Add(Me.cmbShape)
        Me.pnl4c.Controls.Add(Me.cmbStnGroup_MAN)
        Me.pnl4c.Controls.Add(Me.Label9)
        Me.pnl4c.Controls.Add(Me.CmbClarity)
        Me.pnl4c.Controls.Add(Me.Label10)
        Me.pnl4c.Controls.Add(Me.CmbColor)
        Me.pnl4c.Controls.Add(Me.Label7)
        Me.pnl4c.Controls.Add(Me.lblCut)
        Me.pnl4c.Controls.Add(Me.CmbCut)
        Me.pnl4c.Location = New System.Drawing.Point(394, 30)
        Me.pnl4c.Name = "pnl4c"
        Me.pnl4c.Size = New System.Drawing.Size(230, 138)
        Me.pnl4c.TabIndex = 3
        Me.pnl4c.Visible = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(6, 13)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(51, 13)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Stn Grp"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbShape
        '
        Me.cmbShape.FormattingEnabled = True
        Me.cmbShape.Location = New System.Drawing.Point(63, 109)
        Me.cmbShape.Name = "cmbShape"
        Me.cmbShape.Size = New System.Drawing.Size(161, 21)
        Me.cmbShape.TabIndex = 9
        '
        'cmbStnGroup_MAN
        '
        Me.cmbStnGroup_MAN.FormattingEnabled = True
        Me.cmbStnGroup_MAN.Location = New System.Drawing.Point(63, 9)
        Me.cmbStnGroup_MAN.Name = "cmbStnGroup_MAN"
        Me.cmbStnGroup_MAN.Size = New System.Drawing.Size(161, 21)
        Me.cmbStnGroup_MAN.TabIndex = 1
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(5, 113)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(43, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Shape"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(693, 61)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(92, 13)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "Cost Centre(s)"
        '
        'chkLstCostcentre
        '
        Me.chkLstCostcentre.FormattingEnabled = True
        Me.chkLstCostcentre.Location = New System.Drawing.Point(630, 77)
        Me.chkLstCostcentre.Name = "chkLstCostcentre"
        Me.chkLstCostcentre.Size = New System.Drawing.Size(249, 84)
        Me.chkLstCostcentre.TabIndex = 17
        '
        'txtsaleper_PER
        '
        Me.txtsaleper_PER.Location = New System.Drawing.Point(290, 180)
        Me.txtsaleper_PER.MaxLength = 14
        Me.txtsaleper_PER.Name = "txtsaleper_PER"
        Me.txtsaleper_PER.Size = New System.Drawing.Size(94, 21)
        Me.txtsaleper_PER.TabIndex = 15
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Location = New System.Drawing.Point(210, 184)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(78, 13)
        Me.Label12.TabIndex = 14
        Me.Label12.Text = "Sale Rate %"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.Red
        Me.Label13.Location = New System.Drawing.Point(389, 184)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(407, 13)
        Me.Label13.TabIndex = 8
        Me.Label13.Text = "For Piece Wise Calculation FROM CENT Should Start with ZERO"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'BtnTemplate
        '
        Me.BtnTemplate.Location = New System.Drawing.Point(771, 21)
        Me.BtnTemplate.Name = "BtnTemplate"
        Me.BtnTemplate.Size = New System.Drawing.Size(86, 30)
        Me.BtnTemplate.TabIndex = 27
        Me.BtnTemplate.Text = "&Template"
        Me.BtnTemplate.UseVisualStyleBackColor = True
        '
        'BtnUpdate
        '
        Me.BtnUpdate.Enabled = False
        Me.BtnUpdate.Location = New System.Drawing.Point(742, 207)
        Me.BtnUpdate.Name = "BtnUpdate"
        Me.BtnUpdate.Size = New System.Drawing.Size(86, 30)
        Me.BtnUpdate.TabIndex = 26
        Me.BtnUpdate.Text = "&Update"
        Me.BtnUpdate.UseVisualStyleBackColor = True
        '
        'BtnImport
        '
        Me.BtnImport.Location = New System.Drawing.Point(651, 207)
        Me.BtnImport.Name = "BtnImport"
        Me.BtnImport.Size = New System.Drawing.Size(86, 30)
        Me.BtnImport.TabIndex = 25
        Me.BtnImport.Text = "&Import"
        Me.BtnImport.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(547, 207)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 32
        Me.btnExport.Text = "&Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'frmCentRate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(891, 573)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.BtnTemplate)
        Me.Controls.Add(Me.BtnUpdate)
        Me.Controls.Add(Me.BtnImport)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.txtsaleper_PER)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.chkLstCostcentre)
        Me.Controls.Add(Me.lbldesigner)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.cmbDesigner_MAN)
        Me.Controls.Add(Me.pnl4c)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.pnlControls)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.txtMinRate_Amt)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtPurchaseRate_Amt)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtMaxRate_Amt)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtCentTo)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtCentFrom)
        Me.Controls.Add(Me.btnNew)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmCentRate"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CentRate"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlControls.ResumeLayout(False)
        Me.pnlControls.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnl4c.ResumeLayout(False)
        Me.pnl4c.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtMinRate_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtMaxRate_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtPurchaseRate_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtCentTo As System.Windows.Forms.TextBox
    Friend WithEvents txtCentFrom As System.Windows.Forms.TextBox
    Friend WithEvents cmbSubItemName_Man As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItemName_Man As System.Windows.Forms.ComboBox
    Friend WithEvents pnlControls As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents cmbParty As System.Windows.Forms.ComboBox
    Friend WithEvents lblParty As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents CmbColor As System.Windows.Forms.ComboBox
    Friend WithEvents lblCut As System.Windows.Forms.Label
    Friend WithEvents CmbCut As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents CmbClarity As System.Windows.Forms.ComboBox
    Friend WithEvents pnl4c As System.Windows.Forms.Panel
    Friend WithEvents cmbShape As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lbldesigner As System.Windows.Forms.Label
    Friend WithEvents cmbDesigner_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents chkLstCostcentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents txtsaleper_PER As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As Label
    Friend WithEvents cmbStnGroup_MAN As ComboBox
    Friend WithEvents BtnTemplate As Button
    Friend WithEvents BtnUpdate As Button
    Friend WithEvents BtnImport As Button
    Friend WithEvents btnExport As Button
End Class
