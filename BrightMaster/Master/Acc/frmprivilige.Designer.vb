<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmprivilige
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
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.pnlControls = New System.Windows.Forms.Panel
        Me.ChkcmbSubItem = New BrighttechPack.CheckedComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.ChkPointMultiple = New System.Windows.Forms.CheckBox
        Me.CmbValType_MAN = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtCusttype = New System.Windows.Forms.ComboBox
        Me.chkfrange = New System.Windows.Forms.CheckBox
        Me.chkCmbdtScheme = New BrighttechPack.CheckedComboBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtvalue_AMT = New System.Windows.Forms.TextBox
        Me.rbtValue = New System.Windows.Forms.RadioButton
        Me.rbtWeight = New System.Windows.Forms.RadioButton
        Me.Label6 = New System.Windows.Forms.Label
        Me.lblFromValue = New System.Windows.Forms.Label
        Me.txtTo = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtpoints = New System.Windows.Forms.TextBox
        Me.txtFrom = New System.Windows.Forms.TextBox
        Me.cmbActive = New System.Windows.Forms.ComboBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.chkCmbItem = New BrighttechPack.CheckedComboBox
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.chkCmbMetal = New BrighttechPack.CheckedComboBox
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtFrom1_AMT = New System.Windows.Forms.TextBox
        Me.txtTo1_AMT = New System.Windows.Forms.TextBox
        Me.tabView = New System.Windows.Forms.TabPage
        Me.btnBack = New System.Windows.Forms.Button
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.lblStatus = New System.Windows.Forms.Label
        Me.btnDelete = New System.Windows.Forms.Button
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.pnlControls.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(131, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1022, 640)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.pnlControls)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1014, 614)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'pnlControls
        '
        Me.pnlControls.BackColor = System.Drawing.Color.Transparent
        Me.pnlControls.Controls.Add(Me.ChkcmbSubItem)
        Me.pnlControls.Controls.Add(Me.Label4)
        Me.pnlControls.Controls.Add(Me.ChkPointMultiple)
        Me.pnlControls.Controls.Add(Me.CmbValType_MAN)
        Me.pnlControls.Controls.Add(Me.Label2)
        Me.pnlControls.Controls.Add(Me.Label1)
        Me.pnlControls.Controls.Add(Me.txtCusttype)
        Me.pnlControls.Controls.Add(Me.chkfrange)
        Me.pnlControls.Controls.Add(Me.chkCmbdtScheme)
        Me.pnlControls.Controls.Add(Me.btnExit)
        Me.pnlControls.Controls.Add(Me.btnNew)
        Me.pnlControls.Controls.Add(Me.btnOpen)
        Me.pnlControls.Controls.Add(Me.btnSave)
        Me.pnlControls.Controls.Add(Me.Label7)
        Me.pnlControls.Controls.Add(Me.Label3)
        Me.pnlControls.Controls.Add(Me.txtvalue_AMT)
        Me.pnlControls.Controls.Add(Me.rbtValue)
        Me.pnlControls.Controls.Add(Me.rbtWeight)
        Me.pnlControls.Controls.Add(Me.Label6)
        Me.pnlControls.Controls.Add(Me.lblFromValue)
        Me.pnlControls.Controls.Add(Me.txtTo)
        Me.pnlControls.Controls.Add(Me.Label5)
        Me.pnlControls.Controls.Add(Me.txtpoints)
        Me.pnlControls.Controls.Add(Me.txtFrom)
        Me.pnlControls.Controls.Add(Me.cmbActive)
        Me.pnlControls.Controls.Add(Me.Label14)
        Me.pnlControls.Controls.Add(Me.Label19)
        Me.pnlControls.Controls.Add(Me.cmbType)
        Me.pnlControls.Controls.Add(Me.Label8)
        Me.pnlControls.Controls.Add(Me.chkCmbItem)
        Me.pnlControls.Controls.Add(Me.chkCmbCostCentre)
        Me.pnlControls.Controls.Add(Me.chkCmbMetal)
        Me.pnlControls.Controls.Add(Me.chkCmbCompany)
        Me.pnlControls.Controls.Add(Me.label10)
        Me.pnlControls.Controls.Add(Me.Label9)
        Me.pnlControls.Controls.Add(Me.Label)
        Me.pnlControls.Controls.Add(Me.Label11)
        Me.pnlControls.Controls.Add(Me.txtFrom1_AMT)
        Me.pnlControls.Controls.Add(Me.txtTo1_AMT)
        Me.pnlControls.Location = New System.Drawing.Point(328, 26)
        Me.pnlControls.Name = "pnlControls"
        Me.pnlControls.Size = New System.Drawing.Size(465, 580)
        Me.pnlControls.TabIndex = 0
        '
        'ChkcmbSubItem
        '
        Me.ChkcmbSubItem.CheckOnClick = True
        Me.ChkcmbSubItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkcmbSubItem.DropDownHeight = 1
        Me.ChkcmbSubItem.FormattingEnabled = True
        Me.ChkcmbSubItem.IntegralHeight = False
        Me.ChkcmbSubItem.Location = New System.Drawing.Point(122, 206)
        Me.ChkcmbSubItem.Name = "ChkcmbSubItem"
        Me.ChkcmbSubItem.Size = New System.Drawing.Size(320, 22)
        Me.ChkcmbSubItem.TabIndex = 13
        Me.ChkcmbSubItem.ValueSeparator = ", "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(24, 215)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(89, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "SubItemName"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ChkPointMultiple
        '
        Me.ChkPointMultiple.AutoSize = True
        Me.ChkPointMultiple.Location = New System.Drawing.Point(224, 353)
        Me.ChkPointMultiple.Name = "ChkPointMultiple"
        Me.ChkPointMultiple.Size = New System.Drawing.Size(107, 17)
        Me.ChkPointMultiple.TabIndex = 28
        Me.ChkPointMultiple.Text = "Points Multiple"
        Me.ChkPointMultiple.UseVisualStyleBackColor = True
        '
        'CmbValType_MAN
        '
        Me.CmbValType_MAN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbValType_MAN.FormattingEnabled = True
        Me.CmbValType_MAN.Location = New System.Drawing.Point(273, 381)
        Me.CmbValType_MAN.Name = "CmbValType_MAN"
        Me.CmbValType_MAN.Size = New System.Drawing.Size(85, 21)
        Me.CmbValType_MAN.TabIndex = 32
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(214, 384)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 13)
        Me.Label2.TabIndex = 31
        Me.Label2.Text = "Val Type"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(225, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Customer Type"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCusttype
        '
        Me.txtCusttype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.txtCusttype.FormattingEnabled = True
        Me.txtCusttype.Location = New System.Drawing.Point(326, 64)
        Me.txtCusttype.Name = "txtCusttype"
        Me.txtCusttype.Size = New System.Drawing.Size(113, 21)
        Me.txtCusttype.TabIndex = 3
        '
        'chkfrange
        '
        Me.chkfrange.AutoSize = True
        Me.chkfrange.Checked = True
        Me.chkfrange.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkfrange.Location = New System.Drawing.Point(308, 273)
        Me.chkfrange.Name = "chkfrange"
        Me.chkfrange.Size = New System.Drawing.Size(96, 17)
        Me.chkfrange.TabIndex = 19
        Me.chkfrange.Text = "Fixed Range"
        Me.chkfrange.UseVisualStyleBackColor = True
        '
        'chkCmbdtScheme
        '
        Me.chkCmbdtScheme.CheckOnClick = True
        Me.chkCmbdtScheme.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbdtScheme.DropDownHeight = 1
        Me.chkCmbdtScheme.FormattingEnabled = True
        Me.chkCmbdtScheme.IntegralHeight = False
        Me.chkCmbdtScheme.Location = New System.Drawing.Point(122, 237)
        Me.chkCmbdtScheme.Name = "chkCmbdtScheme"
        Me.chkCmbdtScheme.Size = New System.Drawing.Size(320, 22)
        Me.chkCmbdtScheme.TabIndex = 15
        Me.chkCmbdtScheme.ValueSeparator = ", "
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(342, 487)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 38
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(234, 487)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 37
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(126, 487)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 36
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(18, 487)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 35
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(221, 326)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(21, 13)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(24, 384)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "Value"
        '
        'txtvalue_AMT
        '
        Me.txtvalue_AMT.Location = New System.Drawing.Point(123, 381)
        Me.txtvalue_AMT.Name = "txtvalue_AMT"
        Me.txtvalue_AMT.Size = New System.Drawing.Size(85, 21)
        Me.txtvalue_AMT.TabIndex = 30
        '
        'rbtValue
        '
        Me.rbtValue.AutoSize = True
        Me.rbtValue.Location = New System.Drawing.Point(224, 272)
        Me.rbtValue.Name = "rbtValue"
        Me.rbtValue.Size = New System.Drawing.Size(57, 17)
        Me.rbtValue.TabIndex = 18
        Me.rbtValue.Text = "Value"
        Me.rbtValue.UseVisualStyleBackColor = True
        '
        'rbtWeight
        '
        Me.rbtWeight.AutoSize = True
        Me.rbtWeight.Checked = True
        Me.rbtWeight.Location = New System.Drawing.Point(123, 272)
        Me.rbtWeight.Name = "rbtWeight"
        Me.rbtWeight.Size = New System.Drawing.Size(64, 17)
        Me.rbtWeight.TabIndex = 17
        Me.rbtWeight.TabStop = True
        Me.rbtWeight.Text = "Weight"
        Me.rbtWeight.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(24, 278)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(62, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Based On"
        '
        'lblFromValue
        '
        Me.lblFromValue.AutoSize = True
        Me.lblFromValue.Location = New System.Drawing.Point(24, 327)
        Me.lblFromValue.Name = "lblFromValue"
        Me.lblFromValue.Size = New System.Drawing.Size(36, 13)
        Me.lblFromValue.TabIndex = 20
        Me.lblFromValue.Text = "From"
        '
        'txtTo
        '
        Me.txtTo.Location = New System.Drawing.Point(272, 323)
        Me.txtTo.Name = "txtTo"
        Me.txtTo.Size = New System.Drawing.Size(85, 21)
        Me.txtTo.TabIndex = 25
        Me.txtTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(24, 353)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(41, 13)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "Points"
        '
        'txtpoints
        '
        Me.txtpoints.Location = New System.Drawing.Point(123, 350)
        Me.txtpoints.MaxLength = 15
        Me.txtpoints.Name = "txtpoints"
        Me.txtpoints.Size = New System.Drawing.Size(85, 21)
        Me.txtpoints.TabIndex = 27
        Me.txtpoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFrom
        '
        Me.txtFrom.Location = New System.Drawing.Point(122, 323)
        Me.txtFrom.Name = "txtFrom"
        Me.txtFrom.Size = New System.Drawing.Size(85, 21)
        Me.txtFrom.TabIndex = 22
        Me.txtFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmbActive
        '
        Me.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActive.FormattingEnabled = True
        Me.cmbActive.Location = New System.Drawing.Point(123, 412)
        Me.cmbActive.Name = "cmbActive"
        Me.cmbActive.Size = New System.Drawing.Size(85, 21)
        Me.cmbActive.TabIndex = 34
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(24, 415)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(42, 13)
        Me.Label14.TabIndex = 33
        Me.Label14.Text = "Active"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(24, 67)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(35, 13)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "Type"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbType
        '
        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbType.FormattingEnabled = True
        Me.cmbType.Location = New System.Drawing.Point(123, 64)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(96, 21)
        Me.cmbType.TabIndex = 1
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(24, 246)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(53, 13)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Scheme"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbItem
        '
        Me.chkCmbItem.CheckOnClick = True
        Me.chkCmbItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItem.DropDownHeight = 1
        Me.chkCmbItem.FormattingEnabled = True
        Me.chkCmbItem.IntegralHeight = False
        Me.chkCmbItem.Location = New System.Drawing.Point(122, 176)
        Me.chkCmbItem.Name = "chkCmbItem"
        Me.chkCmbItem.Size = New System.Drawing.Size(320, 22)
        Me.chkCmbItem.TabIndex = 11
        Me.chkCmbItem.ValueSeparator = ", "
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(122, 120)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(320, 22)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'chkCmbMetal
        '
        Me.chkCmbMetal.CheckOnClick = True
        Me.chkCmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbMetal.DropDownHeight = 1
        Me.chkCmbMetal.FormattingEnabled = True
        Me.chkCmbMetal.IntegralHeight = False
        Me.chkCmbMetal.Location = New System.Drawing.Point(122, 148)
        Me.chkCmbMetal.Name = "chkCmbMetal"
        Me.chkCmbMetal.Size = New System.Drawing.Size(320, 22)
        Me.chkCmbMetal.TabIndex = 9
        Me.chkCmbMetal.ValueSeparator = ", "
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(122, 92)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(320, 22)
        Me.chkCmbCompany.TabIndex = 5
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'label10
        '
        Me.label10.AutoSize = True
        Me.label10.Location = New System.Drawing.Point(24, 157)
        Me.label10.Name = "label10"
        Me.label10.Size = New System.Drawing.Size(37, 13)
        Me.label10.TabIndex = 8
        Me.label10.Text = "Metal"
        Me.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(24, 185)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(67, 13)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "ItemName"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(24, 129)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 6
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(24, 101)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(62, 13)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Company"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFrom1_AMT
        '
        Me.txtFrom1_AMT.Location = New System.Drawing.Point(123, 323)
        Me.txtFrom1_AMT.Name = "txtFrom1_AMT"
        Me.txtFrom1_AMT.Size = New System.Drawing.Size(85, 21)
        Me.txtFrom1_AMT.TabIndex = 21
        '
        'txtTo1_AMT
        '
        Me.txtTo1_AMT.Location = New System.Drawing.Point(272, 323)
        Me.txtTo1_AMT.Name = "txtTo1_AMT"
        Me.txtTo1_AMT.Size = New System.Drawing.Size(85, 21)
        Me.txtTo1_AMT.TabIndex = 24
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.btnBack)
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.lblStatus)
        Me.tabView.Controls.Add(Me.btnDelete)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1014, 614)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(113, 8)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 34
        Me.btnBack.Text = "Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(5, 44)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1007, 512)
        Me.gridView.TabIndex = 2
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(6, 559)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 25
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(7, 8)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 33
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'frmprivilige
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmprivilige"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Privilige"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.pnlControls.ResumeLayout(False)
        Me.pnlControls.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.tabView.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents pnlControls As System.Windows.Forms.Panel
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtvalue_AMT As System.Windows.Forms.TextBox
    Friend WithEvents rbtValue As System.Windows.Forms.RadioButton
    Friend WithEvents rbtWeight As System.Windows.Forms.RadioButton
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblFromValue As System.Windows.Forms.Label
    Friend WithEvents txtTo As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtpoints As System.Windows.Forms.TextBox
    Friend WithEvents txtFrom As System.Windows.Forms.TextBox
    Friend WithEvents cmbActive As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents chkCmbItem As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents chkCmbdtScheme As BrighttechPack.CheckedComboBox
    Friend WithEvents txtTo1_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtFrom1_AMT As System.Windows.Forms.TextBox
    Friend WithEvents chkfrange As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCusttype As System.Windows.Forms.ComboBox
    Friend WithEvents CmbValType_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ChkPointMultiple As System.Windows.Forms.CheckBox
    Friend WithEvents ChkcmbSubItem As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
