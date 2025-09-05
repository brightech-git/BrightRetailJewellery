<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WTagedView
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
        Me.pnlMain = New System.Windows.Forms.Panel
        Me.grpFiltration = New System.Windows.Forms.GroupBox
        Me.txtSearch_txt = New System.Windows.Forms.TextBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.cmbSearchKey = New System.Windows.Forms.ComboBox
        Me.chkStudColumn = New System.Windows.Forms.CheckBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.cmbGroup = New System.Windows.Forms.ComboBox
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.cmbCostCenter = New System.Windows.Forms.ComboBox
        Me.cmbItemType = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.txtToRate_WET = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtToDiaWt_WET = New System.Windows.Forms.TextBox
        Me.txtFromRate_WET = New System.Windows.Forms.TextBox
        Me.txtToWt_WET = New System.Windows.Forms.TextBox
        Me.txtFromDiaWt_WET = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtFromWt_WET = New System.Windows.Forms.TextBox
        Me.txtItemName = New System.Windows.Forms.TextBox
        Me.AsOnDate = New System.Windows.Forms.Label
        Me.txtTagNo = New System.Windows.Forms.TextBox
        Me.txtItemCode_NUM = New System.Windows.Forms.TextBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlMain.SuspendLayout()
        Me.grpFiltration.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.grpFiltration)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(1022, 640)
        Me.pnlMain.TabIndex = 0
        '
        'grpFiltration
        '
        Me.grpFiltration.Controls.Add(Me.txtSearch_txt)
        Me.grpFiltration.Controls.Add(Me.Label19)
        Me.grpFiltration.Controls.Add(Me.cmbSearchKey)
        Me.grpFiltration.Controls.Add(Me.chkStudColumn)
        Me.grpFiltration.Controls.Add(Me.Label20)
        Me.grpFiltration.Controls.Add(Me.cmbGroup)
        Me.grpFiltration.Controls.Add(Me.dtpAsOnDate)
        Me.grpFiltration.Controls.Add(Me.btnNew)
        Me.grpFiltration.Controls.Add(Me.btnExit)
        Me.grpFiltration.Controls.Add(Me.btnView_Search)
        Me.grpFiltration.Controls.Add(Me.cmbCostCenter)
        Me.grpFiltration.Controls.Add(Me.cmbItemType)
        Me.grpFiltration.Controls.Add(Me.Label9)
        Me.grpFiltration.Controls.Add(Me.Label8)
        Me.grpFiltration.Controls.Add(Me.Label1)
        Me.grpFiltration.Controls.Add(Me.Label4)
        Me.grpFiltration.Controls.Add(Me.Label16)
        Me.grpFiltration.Controls.Add(Me.Label14)
        Me.grpFiltration.Controls.Add(Me.Label12)
        Me.grpFiltration.Controls.Add(Me.Label18)
        Me.grpFiltration.Controls.Add(Me.Label15)
        Me.grpFiltration.Controls.Add(Me.Label13)
        Me.grpFiltration.Controls.Add(Me.txtToRate_WET)
        Me.grpFiltration.Controls.Add(Me.Label11)
        Me.grpFiltration.Controls.Add(Me.txtToDiaWt_WET)
        Me.grpFiltration.Controls.Add(Me.txtFromRate_WET)
        Me.grpFiltration.Controls.Add(Me.txtToWt_WET)
        Me.grpFiltration.Controls.Add(Me.txtFromDiaWt_WET)
        Me.grpFiltration.Controls.Add(Me.Label2)
        Me.grpFiltration.Controls.Add(Me.txtFromWt_WET)
        Me.grpFiltration.Controls.Add(Me.txtItemName)
        Me.grpFiltration.Controls.Add(Me.AsOnDate)
        Me.grpFiltration.Controls.Add(Me.txtTagNo)
        Me.grpFiltration.Controls.Add(Me.txtItemCode_NUM)
        Me.grpFiltration.Location = New System.Drawing.Point(354, 60)
        Me.grpFiltration.Name = "grpFiltration"
        Me.grpFiltration.Size = New System.Drawing.Size(345, 403)
        Me.grpFiltration.TabIndex = 0
        Me.grpFiltration.TabStop = False
        Me.grpFiltration.Text = "STOCK VIEW"
        '
        'txtSearch_txt
        '
        Me.txtSearch_txt.Location = New System.Drawing.Point(111, 276)
        Me.txtSearch_txt.Name = "txtSearch_txt"
        Me.txtSearch_txt.Size = New System.Drawing.Size(226, 21)
        Me.txtSearch_txt.TabIndex = 39
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(6, 276)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(76, 13)
        Me.Label19.TabIndex = 38
        Me.Label19.Text = "Search Text"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSearchKey
        '
        Me.cmbSearchKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSearchKey.FormattingEnabled = True
        Me.cmbSearchKey.Items.AddRange(New Object() {"PNAME", "ADDRESS1", "AREA", "CITY", "STATE", "MOBILE"})
        Me.cmbSearchKey.Location = New System.Drawing.Point(111, 251)
        Me.cmbSearchKey.Name = "cmbSearchKey"
        Me.cmbSearchKey.Size = New System.Drawing.Size(226, 21)
        Me.cmbSearchKey.TabIndex = 37
        '
        'chkStudColumn
        '
        Me.chkStudColumn.AutoSize = True
        Me.chkStudColumn.Checked = True
        Me.chkStudColumn.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkStudColumn.Location = New System.Drawing.Point(111, 332)
        Me.chkStudColumn.Name = "chkStudColumn"
        Me.chkStudColumn.Size = New System.Drawing.Size(162, 17)
        Me.chkStudColumn.TabIndex = 46
        Me.chkStudColumn.Text = "Stud Details In columns"
        Me.chkStudColumn.UseVisualStyleBackColor = True
        Me.chkStudColumn.Visible = False
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(6, 253)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(73, 13)
        Me.Label20.TabIndex = 36
        Me.Label20.Text = "Search Key"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbGroup
        '
        Me.cmbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbGroup.FormattingEnabled = True
        Me.cmbGroup.Location = New System.Drawing.Point(111, 305)
        Me.cmbGroup.Name = "cmbGroup"
        Me.cmbGroup.Size = New System.Drawing.Size(224, 21)
        Me.cmbGroup.TabIndex = 41
        Me.cmbGroup.Visible = False
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(111, 92)
        Me.dtpAsOnDate.Mask = "##/##/####"
        Me.dtpAsOnDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate.Name = "dtpAsOnDate"
        Me.dtpAsOnDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate.Size = New System.Drawing.Size(93, 21)
        Me.dtpAsOnDate.TabIndex = 9
        Me.dtpAsOnDate.Text = "07/03/9998"
        Me.dtpAsOnDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(130, 355)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 48
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(243, 355)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 49
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(17, 355)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 47
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'cmbCostCenter
        '
        Me.cmbCostCenter.FormattingEnabled = True
        Me.cmbCostCenter.Location = New System.Drawing.Point(111, 146)
        Me.cmbCostCenter.Name = "cmbCostCenter"
        Me.cmbCostCenter.Size = New System.Drawing.Size(226, 21)
        Me.cmbCostCenter.TabIndex = 19
        '
        'cmbItemType
        '
        Me.cmbItemType.FormattingEnabled = True
        Me.cmbItemType.Location = New System.Drawing.Point(111, 119)
        Me.cmbItemType.Name = "cmbItemType"
        Me.cmbItemType.Size = New System.Drawing.Size(226, 21)
        Me.cmbItemType.TabIndex = 15
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(4, 150)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(76, 13)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "Cost Centre"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(4, 123)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(66, 13)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Item Type"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Item Code"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 43)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Item Name"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(209, 231)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(21, 13)
        Me.Label16.TabIndex = 34
        Me.Label16.Text = "To"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(209, 205)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(21, 13)
        Me.Label14.TabIndex = 30
        Me.Label14.Text = "To"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(209, 179)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(21, 13)
        Me.Label12.TabIndex = 26
        Me.Label12.Text = "To"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(6, 311)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(61, 13)
        Me.Label18.TabIndex = 40
        Me.Label18.Text = "Group By"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label18.Visible = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(6, 231)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(66, 13)
        Me.Label15.TabIndex = 32
        Me.Label15.Text = "Rate From"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 205)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(98, 13)
        Me.Label13.TabIndex = 28
        Me.Label13.Text = "DiaWeight From"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtToRate_WET
        '
        Me.txtToRate_WET.BackColor = System.Drawing.SystemColors.Window
        Me.txtToRate_WET.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtToRate_WET.Location = New System.Drawing.Point(241, 227)
        Me.txtToRate_WET.MaxLength = 8
        Me.txtToRate_WET.Name = "txtToRate_WET"
        Me.txtToRate_WET.Size = New System.Drawing.Size(96, 21)
        Me.txtToRate_WET.TabIndex = 35
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 179)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(79, 13)
        Me.Label11.TabIndex = 24
        Me.Label11.Text = "Weight From"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtToDiaWt_WET
        '
        Me.txtToDiaWt_WET.BackColor = System.Drawing.SystemColors.Window
        Me.txtToDiaWt_WET.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtToDiaWt_WET.Location = New System.Drawing.Point(241, 201)
        Me.txtToDiaWt_WET.MaxLength = 8
        Me.txtToDiaWt_WET.Name = "txtToDiaWt_WET"
        Me.txtToDiaWt_WET.Size = New System.Drawing.Size(96, 21)
        Me.txtToDiaWt_WET.TabIndex = 31
        '
        'txtFromRate_WET
        '
        Me.txtFromRate_WET.BackColor = System.Drawing.SystemColors.Window
        Me.txtFromRate_WET.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromRate_WET.Location = New System.Drawing.Point(111, 227)
        Me.txtFromRate_WET.MaxLength = 8
        Me.txtFromRate_WET.Name = "txtFromRate_WET"
        Me.txtFromRate_WET.Size = New System.Drawing.Size(96, 21)
        Me.txtFromRate_WET.TabIndex = 33
        '
        'txtToWt_WET
        '
        Me.txtToWt_WET.BackColor = System.Drawing.SystemColors.Window
        Me.txtToWt_WET.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtToWt_WET.Location = New System.Drawing.Point(241, 175)
        Me.txtToWt_WET.MaxLength = 8
        Me.txtToWt_WET.Name = "txtToWt_WET"
        Me.txtToWt_WET.Size = New System.Drawing.Size(96, 21)
        Me.txtToWt_WET.TabIndex = 27
        '
        'txtFromDiaWt_WET
        '
        Me.txtFromDiaWt_WET.BackColor = System.Drawing.SystemColors.Window
        Me.txtFromDiaWt_WET.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDiaWt_WET.Location = New System.Drawing.Point(111, 201)
        Me.txtFromDiaWt_WET.MaxLength = 8
        Me.txtFromDiaWt_WET.Name = "txtFromDiaWt_WET"
        Me.txtFromDiaWt_WET.Size = New System.Drawing.Size(96, 21)
        Me.txtFromDiaWt_WET.TabIndex = 29
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "TagNo"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFromWt_WET
        '
        Me.txtFromWt_WET.BackColor = System.Drawing.SystemColors.Window
        Me.txtFromWt_WET.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromWt_WET.Location = New System.Drawing.Point(111, 175)
        Me.txtFromWt_WET.MaxLength = 8
        Me.txtFromWt_WET.Name = "txtFromWt_WET"
        Me.txtFromWt_WET.Size = New System.Drawing.Size(96, 21)
        Me.txtFromWt_WET.TabIndex = 25
        '
        'txtItemName
        '
        Me.txtItemName.BackColor = System.Drawing.SystemColors.Window
        Me.txtItemName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemName.Enabled = False
        Me.txtItemName.Location = New System.Drawing.Point(111, 39)
        Me.txtItemName.Name = "txtItemName"
        Me.txtItemName.Size = New System.Drawing.Size(226, 21)
        Me.txtItemName.TabIndex = 3
        Me.txtItemName.Text = "DFSSD"
        '
        'AsOnDate
        '
        Me.AsOnDate.AutoSize = True
        Me.AsOnDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AsOnDate.Location = New System.Drawing.Point(6, 96)
        Me.AsOnDate.Name = "AsOnDate"
        Me.AsOnDate.Size = New System.Drawing.Size(64, 13)
        Me.AsOnDate.TabIndex = 8
        Me.AsOnDate.Text = "AsOnDate"
        Me.AsOnDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtTagNo
        '
        Me.txtTagNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtTagNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTagNo.Location = New System.Drawing.Point(111, 66)
        Me.txtTagNo.MaxLength = 15
        Me.txtTagNo.Name = "txtTagNo"
        Me.txtTagNo.Size = New System.Drawing.Size(119, 21)
        Me.txtTagNo.TabIndex = 7
        Me.txtTagNo.Text = "FSDSDFSDF"
        '
        'txtItemCode_NUM
        '
        Me.txtItemCode_NUM.BackColor = System.Drawing.SystemColors.Window
        Me.txtItemCode_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemCode_NUM.Location = New System.Drawing.Point(111, 13)
        Me.txtItemCode_NUM.MaxLength = 8
        Me.txtItemCode_NUM.Name = "txtItemCode_NUM"
        Me.txtItemCode_NUM.Size = New System.Drawing.Size(119, 21)
        Me.txtItemCode_NUM.TabIndex = 1
        Me.txtItemCode_NUM.Text = "DAFDS"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 1000
        Me.ToolTip1.IsBalloon = True
        Me.ToolTip1.ShowAlways = True
        '
        'WTagedView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "WTagedView"
        Me.Text = "Item Stock View Detailed"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlMain.ResumeLayout(False)
        Me.grpFiltration.ResumeLayout(False)
        Me.grpFiltration.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtItemName As System.Windows.Forms.TextBox
    Friend WithEvents txtItemCode_NUM As System.Windows.Forms.TextBox
    Friend WithEvents AsOnDate As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtTagNo As System.Windows.Forms.TextBox
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents grpFiltration As System.Windows.Forms.GroupBox
    Friend WithEvents cmbCostCenter As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItemType As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtToRate_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtToDiaWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtFromRate_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtToWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDiaWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtFromWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents cmbGroup As System.Windows.Forms.ComboBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents chkStudColumn As System.Windows.Forms.CheckBox
    Friend WithEvents txtSearch_txt As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents cmbSearchKey As System.Windows.Forms.ComboBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
End Class
