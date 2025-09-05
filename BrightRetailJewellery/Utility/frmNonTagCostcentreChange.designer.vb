<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNonTagCostcentreChange
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
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtPcs_NUM = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtGrsWt_WET = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtNetWt_WET = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.grpOld = New System.Windows.Forms.GroupBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtOldPcs = New System.Windows.Forms.TextBox
        Me.txtOldGrsWt = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtOldNetWt = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.grpNew = New System.Windows.Forms.GroupBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtNewPcs = New System.Windows.Forms.TextBox
        Me.txtNewGrsWt = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.txtNewNetWt = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.txtItemId_NUM = New System.Windows.Forms.TextBox
        Me.txtItemName = New System.Windows.Forms.TextBox
        Me.txtSubItemName = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbDesigner_MAN = New System.Windows.Forms.ComboBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.cmbItemType_MAN = New System.Windows.Forms.ComboBox
        Me.cmbOldCostCentre_MAN = New System.Windows.Forms.ComboBox
        Me.dtpTranDate = New BrighttechPack.DatePicker(Me.components)
        Me.cmbNewCostCenter_MAN = New System.Windows.Forms.ComboBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.cmbItemCounter_MAN = New System.Windows.Forms.ComboBox
        Me.txtPktno = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.grpOld.SuspendLayout()
        Me.grpNew.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Tran Date"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(9, 125)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(92, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Old Costcentre"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 152)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(97, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "New Costcentre"
        '
        'txtPcs_NUM
        '
        Me.txtPcs_NUM.Location = New System.Drawing.Point(109, 175)
        Me.txtPcs_NUM.Name = "txtPcs_NUM"
        Me.txtPcs_NUM.Size = New System.Drawing.Size(93, 21)
        Me.txtPcs_NUM.TabIndex = 14
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(9, 179)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(26, 13)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Pcs"
        '
        'txtGrsWt_WET
        '
        Me.txtGrsWt_WET.Location = New System.Drawing.Point(109, 202)
        Me.txtGrsWt_WET.Name = "txtGrsWt_WET"
        Me.txtGrsWt_WET.Size = New System.Drawing.Size(93, 21)
        Me.txtGrsWt_WET.TabIndex = 16
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(9, 206)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(70, 13)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Grs Weight"
        '
        'txtNetWt_WET
        '
        Me.txtNetWt_WET.Location = New System.Drawing.Point(109, 229)
        Me.txtNetWt_WET.Name = "txtNetWt_WET"
        Me.txtNetWt_WET.Size = New System.Drawing.Size(93, 21)
        Me.txtNetWt_WET.TabIndex = 18
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(9, 233)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(69, 13)
        Me.Label8.TabIndex = 17
        Me.Label8.Text = "Net Weight"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(112, 312)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 23
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(218, 312)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 24
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(324, 312)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 25
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'grpOld
        '
        Me.grpOld.Controls.Add(Me.Label9)
        Me.grpOld.Controls.Add(Me.txtOldPcs)
        Me.grpOld.Controls.Add(Me.txtOldGrsWt)
        Me.grpOld.Controls.Add(Me.Label10)
        Me.grpOld.Controls.Add(Me.txtOldNetWt)
        Me.grpOld.Controls.Add(Me.Label11)
        Me.grpOld.Location = New System.Drawing.Point(349, 17)
        Me.grpOld.Name = "grpOld"
        Me.grpOld.Size = New System.Drawing.Size(188, 100)
        Me.grpOld.TabIndex = 28
        Me.grpOld.TabStop = False
        Me.grpOld.Text = "Old CostCentre Stock"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 18)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(26, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Pcs"
        '
        'txtOldPcs
        '
        Me.txtOldPcs.Location = New System.Drawing.Point(88, 14)
        Me.txtOldPcs.Name = "txtOldPcs"
        Me.txtOldPcs.Size = New System.Drawing.Size(93, 21)
        Me.txtOldPcs.TabIndex = 1
        Me.txtOldPcs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtOldGrsWt
        '
        Me.txtOldGrsWt.Location = New System.Drawing.Point(88, 43)
        Me.txtOldGrsWt.Name = "txtOldGrsWt"
        Me.txtOldGrsWt.Size = New System.Drawing.Size(93, 21)
        Me.txtOldGrsWt.TabIndex = 3
        Me.txtOldGrsWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 47)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(70, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "Grs Weight"
        '
        'txtOldNetWt
        '
        Me.txtOldNetWt.Location = New System.Drawing.Point(88, 72)
        Me.txtOldNetWt.Name = "txtOldNetWt"
        Me.txtOldNetWt.Size = New System.Drawing.Size(93, 21)
        Me.txtOldNetWt.TabIndex = 5
        Me.txtOldNetWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 76)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(69, 13)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Net Weight"
        '
        'grpNew
        '
        Me.grpNew.Controls.Add(Me.Label12)
        Me.grpNew.Controls.Add(Me.txtNewPcs)
        Me.grpNew.Controls.Add(Me.txtNewGrsWt)
        Me.grpNew.Controls.Add(Me.Label13)
        Me.grpNew.Controls.Add(Me.txtNewNetWt)
        Me.grpNew.Controls.Add(Me.Label14)
        Me.grpNew.Location = New System.Drawing.Point(349, 123)
        Me.grpNew.Name = "grpNew"
        Me.grpNew.Size = New System.Drawing.Size(188, 100)
        Me.grpNew.TabIndex = 29
        Me.grpNew.TabStop = False
        Me.grpNew.Text = "New CostCenter Stock"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(6, 19)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(26, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Pcs"
        '
        'txtNewPcs
        '
        Me.txtNewPcs.Location = New System.Drawing.Point(88, 15)
        Me.txtNewPcs.Name = "txtNewPcs"
        Me.txtNewPcs.Size = New System.Drawing.Size(93, 21)
        Me.txtNewPcs.TabIndex = 1
        Me.txtNewPcs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNewGrsWt
        '
        Me.txtNewGrsWt.Location = New System.Drawing.Point(88, 44)
        Me.txtNewGrsWt.Name = "txtNewGrsWt"
        Me.txtNewGrsWt.Size = New System.Drawing.Size(93, 21)
        Me.txtNewGrsWt.TabIndex = 3
        Me.txtNewGrsWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 48)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(70, 13)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "Grs Weight"
        '
        'txtNewNetWt
        '
        Me.txtNewNetWt.Location = New System.Drawing.Point(88, 73)
        Me.txtNewNetWt.Name = "txtNewNetWt"
        Me.txtNewNetWt.Size = New System.Drawing.Size(93, 21)
        Me.txtNewNetWt.TabIndex = 5
        Me.txtNewNetWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(6, 77)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(69, 13)
        Me.Label14.TabIndex = 4
        Me.Label14.Text = "Net Weight"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.SaveToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 70)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'txtItemId_NUM
        '
        Me.txtItemId_NUM.Location = New System.Drawing.Point(109, 65)
        Me.txtItemId_NUM.Name = "txtItemId_NUM"
        Me.txtItemId_NUM.Size = New System.Drawing.Size(37, 21)
        Me.txtItemId_NUM.TabIndex = 5
        '
        'txtItemName
        '
        Me.txtItemName.Location = New System.Drawing.Point(149, 65)
        Me.txtItemName.Name = "txtItemName"
        Me.txtItemName.Size = New System.Drawing.Size(194, 21)
        Me.txtItemName.TabIndex = 6
        '
        'txtSubItemName
        '
        Me.txtSubItemName.Location = New System.Drawing.Point(109, 92)
        Me.txtSubItemName.Name = "txtSubItemName"
        Me.txtSubItemName.Size = New System.Drawing.Size(234, 21)
        Me.txtSubItemName.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 69)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Item"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Sub Item"
        '
        'cmbDesigner_MAN
        '
        Me.cmbDesigner_MAN.FormattingEnabled = True
        Me.cmbDesigner_MAN.Location = New System.Drawing.Point(109, 257)
        Me.cmbDesigner_MAN.Name = "cmbDesigner_MAN"
        Me.cmbDesigner_MAN.Size = New System.Drawing.Size(234, 21)
        Me.cmbDesigner_MAN.TabIndex = 20
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(9, 260)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(58, 13)
        Me.Label15.TabIndex = 19
        Me.Label15.Text = "Designer"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(9, 288)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(63, 13)
        Me.Label16.TabIndex = 21
        Me.Label16.Text = "Item type"
        '
        'cmbItemType_MAN
        '
        Me.cmbItemType_MAN.FormattingEnabled = True
        Me.cmbItemType_MAN.Location = New System.Drawing.Point(109, 285)
        Me.cmbItemType_MAN.Name = "cmbItemType_MAN"
        Me.cmbItemType_MAN.Size = New System.Drawing.Size(234, 21)
        Me.cmbItemType_MAN.TabIndex = 22
        '
        'cmbOldCostCentre_MAN
        '
        Me.cmbOldCostCentre_MAN.FormattingEnabled = True
        Me.cmbOldCostCentre_MAN.Location = New System.Drawing.Point(109, 121)
        Me.cmbOldCostCentre_MAN.Name = "cmbOldCostCentre_MAN"
        Me.cmbOldCostCentre_MAN.Size = New System.Drawing.Size(234, 21)
        Me.cmbOldCostCentre_MAN.TabIndex = 10
        '
        'dtpTranDate
        '
        Me.dtpTranDate.Location = New System.Drawing.Point(109, 11)
        Me.dtpTranDate.Mask = "##/##/####"
        Me.dtpTranDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTranDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTranDate.Name = "dtpTranDate"
        Me.dtpTranDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTranDate.Size = New System.Drawing.Size(78, 21)
        Me.dtpTranDate.TabIndex = 1
        Me.dtpTranDate.Text = "07/03/9998"
        Me.dtpTranDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'cmbNewCostCenter_MAN
        '
        Me.cmbNewCostCenter_MAN.FormattingEnabled = True
        Me.cmbNewCostCenter_MAN.Location = New System.Drawing.Point(109, 148)
        Me.cmbNewCostCenter_MAN.Name = "cmbNewCostCenter_MAN"
        Me.cmbNewCostCenter_MAN.Size = New System.Drawing.Size(234, 21)
        Me.cmbNewCostCenter_MAN.TabIndex = 12
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(9, 41)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(53, 13)
        Me.Label17.TabIndex = 2
        Me.Label17.Text = "Counter"
        '
        'cmbItemCounter_MAN
        '
        Me.cmbItemCounter_MAN.FormattingEnabled = True
        Me.cmbItemCounter_MAN.Location = New System.Drawing.Point(109, 38)
        Me.cmbItemCounter_MAN.Name = "cmbItemCounter_MAN"
        Me.cmbItemCounter_MAN.Size = New System.Drawing.Size(234, 21)
        Me.cmbItemCounter_MAN.TabIndex = 3
        '
        'txtPktno
        '
        Me.txtPktno.Location = New System.Drawing.Point(250, 11)
        Me.txtPktno.Name = "txtPktno"
        Me.txtPktno.Size = New System.Drawing.Size(93, 21)
        Me.txtPktno.TabIndex = 30
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(193, 16)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(52, 13)
        Me.Label18.TabIndex = 31
        Me.Label18.Text = "Pkt. No."
        '
        'frmNonTagCostcentreChange
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(558, 348)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.txtPktno)
        Me.Controls.Add(Me.dtpTranDate)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.grpNew)
        Me.Controls.Add(Me.cmbItemCounter_MAN)
        Me.Controls.Add(Me.txtSubItemName)
        Me.Controls.Add(Me.cmbItemType_MAN)
        Me.Controls.Add(Me.cmbDesigner_MAN)
        Me.Controls.Add(Me.txtItemName)
        Me.Controls.Add(Me.cmbOldCostCentre_MAN)
        Me.Controls.Add(Me.grpOld)
        Me.Controls.Add(Me.cmbNewCostCenter_MAN)
        Me.Controls.Add(Me.txtItemId_NUM)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtPcs_NUM)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtGrsWt_WET)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.txtNetWt_WET)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label8)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "frmNonTagCostcentreChange"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "NonTag Costcentre Change"
        Me.grpOld.ResumeLayout(False)
        Me.grpOld.PerformLayout()
        Me.grpNew.ResumeLayout(False)
        Me.grpNew.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents grpOld As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtOldPcs As System.Windows.Forms.TextBox
    Friend WithEvents txtOldGrsWt As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtOldNetWt As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents grpNew As System.Windows.Forms.GroupBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtNewPcs As System.Windows.Forms.TextBox
    Friend WithEvents txtNewGrsWt As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtNewNetWt As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtItemId_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtItemName As System.Windows.Forms.TextBox
    Friend WithEvents txtSubItemName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbDesigner_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents cmbItemType_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOldCostCentre_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents dtpTranDate As BrighttechPack.DatePicker
    Friend WithEvents cmbNewCostCenter_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents cmbItemCounter_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents txtPktno As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
End Class
