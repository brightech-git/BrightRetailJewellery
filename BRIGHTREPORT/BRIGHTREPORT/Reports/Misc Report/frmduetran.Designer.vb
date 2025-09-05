<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmduetran
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
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.newToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblTitle = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.contextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.viewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.exitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.PnlValuation = New System.Windows.Forms.Panel
        Me.rbtFifo = New System.Windows.Forms.RadioButton
        Me.rbtNormal = New System.Windows.Forms.RadioButton
        Me.chkRange = New System.Windows.Forms.CheckBox
        Me.chkBasedOnMasterDays = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.cmbAcName_MAN = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.cmbCompany = New System.Windows.Forms.ComboBox
        Me.btnView = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rdbTranDateWise = New System.Windows.Forms.RadioButton
        Me.rdbBillDateWise = New System.Windows.Forms.RadioButton
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.txtToDay4 = New System.Windows.Forms.TextBox
        Me.txtToDay6 = New System.Windows.Forms.TextBox
        Me.txtFromDay5 = New System.Windows.Forms.TextBox
        Me.txtFromDay6 = New System.Windows.Forms.TextBox
        Me.txtFromDay4 = New System.Windows.Forms.TextBox
        Me.txtToDay5 = New System.Windows.Forms.TextBox
        Me.txtToDay1 = New System.Windows.Forms.TextBox
        Me.txtToDay3 = New System.Windows.Forms.TextBox
        Me.txtFromDay2 = New System.Windows.Forms.TextBox
        Me.txtFromDay3 = New System.Windows.Forms.TextBox
        Me.txtFromDay1 = New System.Windows.Forms.TextBox
        Me.txtToDay2 = New System.Windows.Forms.TextBox
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoReziseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GrpRange = New CodeVendor.Controls.Grouper
        Me.PnlRange = New System.Windows.Forms.Panel
        Me.contextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.PnlValuation.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.GrpRange.SuspendLayout()
        Me.PnlRange.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(80, 56)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(340, 22)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'newToolStripMenuItem
        '
        Me.newToolStripMenuItem.Name = "newToolStripMenuItem"
        Me.newToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.newToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.newToolStripMenuItem.Text = "New"
        Me.newToolStripMenuItem.Visible = False
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 113)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1028, 28)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Label1"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(5, 59)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'contextMenuStrip1
        '
        Me.contextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.viewToolStripMenuItem, Me.newToolStripMenuItem, Me.exitToolStripMenuItem})
        Me.contextMenuStrip1.Name = "contextMenuStrip1"
        Me.contextMenuStrip1.Size = New System.Drawing.Size(129, 70)
        '
        'viewToolStripMenuItem
        '
        Me.viewToolStripMenuItem.Name = "viewToolStripMenuItem"
        Me.viewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.viewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.viewToolStripMenuItem.Text = "View"
        Me.viewToolStripMenuItem.Visible = False
        '
        'exitToolStripMenuItem
        '
        Me.exitToolStripMenuItem.Name = "exitToolStripMenuItem"
        Me.exitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10
        Me.exitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.exitToolStripMenuItem.Text = "Exit"
        Me.exitToolStripMenuItem.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(160, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Company"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.PnlValuation)
        Me.Panel1.Controls.Add(Me.chkRange)
        Me.Panel1.Controls.Add(Me.chkBasedOnMasterDays)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.cmbAcName_MAN)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cmbCompany)
        Me.Panel1.Controls.Add(Me.btnView)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1028, 113)
        Me.Panel1.TabIndex = 0
        '
        'PnlValuation
        '
        Me.PnlValuation.Controls.Add(Me.rbtFifo)
        Me.PnlValuation.Controls.Add(Me.rbtNormal)
        Me.PnlValuation.Location = New System.Drawing.Point(424, 38)
        Me.PnlValuation.Name = "PnlValuation"
        Me.PnlValuation.Size = New System.Drawing.Size(224, 27)
        Me.PnlValuation.TabIndex = 11
        '
        'rbtFifo
        '
        Me.rbtFifo.AutoSize = True
        Me.rbtFifo.Location = New System.Drawing.Point(109, 5)
        Me.rbtFifo.Name = "rbtFifo"
        Me.rbtFifo.Size = New System.Drawing.Size(51, 17)
        Me.rbtFifo.TabIndex = 1
        Me.rbtFifo.Text = "FIFO"
        Me.rbtFifo.UseVisualStyleBackColor = True
        '
        'rbtNormal
        '
        Me.rbtNormal.AutoSize = True
        Me.rbtNormal.Checked = True
        Me.rbtNormal.Location = New System.Drawing.Point(13, 5)
        Me.rbtNormal.Name = "rbtNormal"
        Me.rbtNormal.Size = New System.Drawing.Size(60, 17)
        Me.rbtNormal.TabIndex = 0
        Me.rbtNormal.TabStop = True
        Me.rbtNormal.Text = "Actual"
        Me.rbtNormal.UseVisualStyleBackColor = True
        '
        'chkRange
        '
        Me.chkRange.AutoSize = True
        Me.chkRange.Location = New System.Drawing.Point(227, 84)
        Me.chkRange.Name = "chkRange"
        Me.chkRange.Size = New System.Drawing.Size(62, 17)
        Me.chkRange.TabIndex = 9
        Me.chkRange.Text = "Range"
        Me.chkRange.UseVisualStyleBackColor = True
        '
        'chkBasedOnMasterDays
        '
        Me.chkBasedOnMasterDays.AutoSize = True
        Me.chkBasedOnMasterDays.Location = New System.Drawing.Point(8, 84)
        Me.chkBasedOnMasterDays.Name = "chkBasedOnMasterDays"
        Me.chkBasedOnMasterDays.Size = New System.Drawing.Size(193, 17)
        Me.chkBasedOnMasterDays.TabIndex = 8
        Me.chkBasedOnMasterDays.Text = "Credit Days Based on Master"
        Me.chkBasedOnMasterDays.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "As On Date"
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(80, 10)
        Me.dtpFrom.Mask = "##-##-####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(77, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "29/09/2010"
        Me.dtpFrom.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'cmbAcName_MAN
        '
        Me.cmbAcName_MAN.FormattingEnabled = True
        Me.cmbAcName_MAN.Location = New System.Drawing.Point(80, 33)
        Me.cmbAcName_MAN.Name = "cmbAcName_MAN"
        Me.cmbAcName_MAN.Size = New System.Drawing.Size(340, 21)
        Me.cmbAcName_MAN.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(5, 37)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Ac Name"
        '
        'cmbCompany
        '
        Me.cmbCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCompany.FormattingEnabled = True
        Me.cmbCompany.Location = New System.Drawing.Point(227, 10)
        Me.cmbCompany.Name = "cmbCompany"
        Me.cmbCompany.Size = New System.Drawing.Size(192, 21)
        Me.cmbCompany.TabIndex = 3
        '
        'btnView
        '
        Me.btnView.Location = New System.Drawing.Point(437, 77)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(112, 31)
        Me.btnView.TabIndex = 12
        Me.btnView.Text = "View [F2]"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(661, 77)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(112, 31)
        Me.btnExport.TabIndex = 14
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(773, 77)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(112, 31)
        Me.btnExit.TabIndex = 15
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(885, 77)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(112, 31)
        Me.btnPrint.TabIndex = 16
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(549, 77)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(112, 31)
        Me.btnNew.TabIndex = 13
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rdbTranDateWise)
        Me.Panel2.Controls.Add(Me.rdbBillDateWise)
        Me.Panel2.Location = New System.Drawing.Point(424, 11)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(224, 27)
        Me.Panel2.TabIndex = 10
        '
        'rdbTranDateWise
        '
        Me.rdbTranDateWise.AutoSize = True
        Me.rdbTranDateWise.Location = New System.Drawing.Point(109, 3)
        Me.rdbTranDateWise.Name = "rdbTranDateWise"
        Me.rdbTranDateWise.Size = New System.Drawing.Size(109, 17)
        Me.rdbTranDateWise.TabIndex = 1
        Me.rdbTranDateWise.Text = "TranDate Wise"
        Me.rdbTranDateWise.UseVisualStyleBackColor = True
        '
        'rdbBillDateWise
        '
        Me.rdbBillDateWise.AutoSize = True
        Me.rdbBillDateWise.Checked = True
        Me.rdbBillDateWise.Location = New System.Drawing.Point(11, 3)
        Me.rdbBillDateWise.Name = "rdbBillDateWise"
        Me.rdbBillDateWise.Size = New System.Drawing.Size(100, 17)
        Me.rdbBillDateWise.TabIndex = 0
        Me.rdbBillDateWise.TabStop = True
        Me.rdbBillDateWise.Text = "BillDate Wise"
        Me.rdbBillDateWise.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(14, 137)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(50, 13)
        Me.Label10.TabIndex = 15
        Me.Label10.Text = "Range6"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(14, 115)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(50, 13)
        Me.Label9.TabIndex = 12
        Me.Label9.Text = "Range5"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(14, 93)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(50, 13)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "Range4"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(14, 71)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(50, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Range3"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(14, 48)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(50, 13)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Range2"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(13, 26)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Range1"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(66, 160)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(73, 27)
        Me.btnOk.TabIndex = 18
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(141, 160)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(73, 27)
        Me.btnCancel.TabIndex = 19
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'txtToDay4
        '
        Me.txtToDay4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtToDay4.Location = New System.Drawing.Point(141, 89)
        Me.txtToDay4.MaxLength = 3
        Me.txtToDay4.Name = "txtToDay4"
        Me.txtToDay4.Size = New System.Drawing.Size(71, 21)
        Me.txtToDay4.TabIndex = 11
        Me.txtToDay4.Text = "120"
        Me.txtToDay4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtToDay6
        '
        Me.txtToDay6.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtToDay6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtToDay6.Location = New System.Drawing.Point(141, 133)
        Me.txtToDay6.MaxLength = 3
        Me.txtToDay6.Name = "txtToDay6"
        Me.txtToDay6.Size = New System.Drawing.Size(71, 21)
        Me.txtToDay6.TabIndex = 17
        Me.txtToDay6.Text = "180"
        Me.txtToDay6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFromDay5
        '
        Me.txtFromDay5.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDay5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromDay5.Location = New System.Drawing.Point(66, 111)
        Me.txtFromDay5.MaxLength = 3
        Me.txtFromDay5.Name = "txtFromDay5"
        Me.txtFromDay5.Size = New System.Drawing.Size(71, 21)
        Me.txtFromDay5.TabIndex = 13
        Me.txtFromDay5.Text = "121"
        Me.txtFromDay5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFromDay6
        '
        Me.txtFromDay6.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDay6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromDay6.Location = New System.Drawing.Point(66, 133)
        Me.txtFromDay6.MaxLength = 3
        Me.txtFromDay6.Name = "txtFromDay6"
        Me.txtFromDay6.Size = New System.Drawing.Size(71, 21)
        Me.txtFromDay6.TabIndex = 16
        Me.txtFromDay6.Text = "151"
        Me.txtFromDay6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFromDay4
        '
        Me.txtFromDay4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDay4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromDay4.Location = New System.Drawing.Point(66, 89)
        Me.txtFromDay4.MaxLength = 3
        Me.txtFromDay4.Name = "txtFromDay4"
        Me.txtFromDay4.Size = New System.Drawing.Size(71, 21)
        Me.txtFromDay4.TabIndex = 10
        Me.txtFromDay4.Text = "91"
        Me.txtFromDay4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtToDay5
        '
        Me.txtToDay5.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtToDay5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtToDay5.Location = New System.Drawing.Point(141, 111)
        Me.txtToDay5.MaxLength = 3
        Me.txtToDay5.Name = "txtToDay5"
        Me.txtToDay5.Size = New System.Drawing.Size(71, 21)
        Me.txtToDay5.TabIndex = 14
        Me.txtToDay5.Text = "150"
        Me.txtToDay5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtToDay1
        '
        Me.txtToDay1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtToDay1.Location = New System.Drawing.Point(141, 23)
        Me.txtToDay1.MaxLength = 3
        Me.txtToDay1.Name = "txtToDay1"
        Me.txtToDay1.Size = New System.Drawing.Size(71, 21)
        Me.txtToDay1.TabIndex = 2
        Me.txtToDay1.Text = "30"
        Me.txtToDay1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtToDay3
        '
        Me.txtToDay3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtToDay3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtToDay3.Location = New System.Drawing.Point(141, 67)
        Me.txtToDay3.MaxLength = 3
        Me.txtToDay3.Name = "txtToDay3"
        Me.txtToDay3.Size = New System.Drawing.Size(71, 21)
        Me.txtToDay3.TabIndex = 8
        Me.txtToDay3.Text = "90"
        Me.txtToDay3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFromDay2
        '
        Me.txtFromDay2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDay2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromDay2.Location = New System.Drawing.Point(66, 45)
        Me.txtFromDay2.MaxLength = 3
        Me.txtFromDay2.Name = "txtFromDay2"
        Me.txtFromDay2.Size = New System.Drawing.Size(71, 21)
        Me.txtFromDay2.TabIndex = 4
        Me.txtFromDay2.Text = "31"
        Me.txtFromDay2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFromDay3
        '
        Me.txtFromDay3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDay3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromDay3.Location = New System.Drawing.Point(66, 67)
        Me.txtFromDay3.MaxLength = 3
        Me.txtFromDay3.Name = "txtFromDay3"
        Me.txtFromDay3.Size = New System.Drawing.Size(71, 21)
        Me.txtFromDay3.TabIndex = 7
        Me.txtFromDay3.Text = "61"
        Me.txtFromDay3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFromDay1
        '
        Me.txtFromDay1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDay1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromDay1.Location = New System.Drawing.Point(66, 23)
        Me.txtFromDay1.MaxLength = 3
        Me.txtFromDay1.Name = "txtFromDay1"
        Me.txtFromDay1.Size = New System.Drawing.Size(71, 21)
        Me.txtFromDay1.TabIndex = 1
        Me.txtFromDay1.Text = "0"
        Me.txtFromDay1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtToDay2
        '
        Me.txtToDay2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtToDay2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtToDay2.Location = New System.Drawing.Point(141, 45)
        Me.txtToDay2.MaxLength = 3
        Me.txtToDay2.Name = "txtToDay2"
        Me.txtToDay2.Size = New System.Drawing.Size(71, 21)
        Me.txtToDay2.TabIndex = 5
        Me.txtToDay2.Text = "60"
        Me.txtToDay2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.ContextMenuStrip2
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 141)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 21
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.Size = New System.Drawing.Size(1028, 339)
        Me.gridView.TabIndex = 2
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoReziseToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(140, 26)
        '
        'AutoReziseToolStripMenuItem
        '
        Me.AutoReziseToolStripMenuItem.Name = "AutoReziseToolStripMenuItem"
        Me.AutoReziseToolStripMenuItem.Size = New System.Drawing.Size(139, 22)
        Me.AutoReziseToolStripMenuItem.Text = "AutoRezise"
        Me.AutoReziseToolStripMenuItem.Visible = False
        '
        'GrpRange
        '
        Me.GrpRange.BackgroundColor = System.Drawing.Color.Lavender
        Me.GrpRange.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.GrpRange.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.GrpRange.BorderColor = System.Drawing.Color.Transparent
        Me.GrpRange.BorderThickness = 1.0!
        Me.GrpRange.Controls.Add(Me.Label10)
        Me.GrpRange.Controls.Add(Me.txtFromDay1)
        Me.GrpRange.Controls.Add(Me.Label9)
        Me.GrpRange.Controls.Add(Me.txtToDay2)
        Me.GrpRange.Controls.Add(Me.Label8)
        Me.GrpRange.Controls.Add(Me.txtFromDay3)
        Me.GrpRange.Controls.Add(Me.Label7)
        Me.GrpRange.Controls.Add(Me.txtFromDay2)
        Me.GrpRange.Controls.Add(Me.Label6)
        Me.GrpRange.Controls.Add(Me.txtToDay3)
        Me.GrpRange.Controls.Add(Me.Label4)
        Me.GrpRange.Controls.Add(Me.txtToDay1)
        Me.GrpRange.Controls.Add(Me.btnOk)
        Me.GrpRange.Controls.Add(Me.txtToDay5)
        Me.GrpRange.Controls.Add(Me.btnCancel)
        Me.GrpRange.Controls.Add(Me.txtFromDay4)
        Me.GrpRange.Controls.Add(Me.txtToDay4)
        Me.GrpRange.Controls.Add(Me.txtFromDay6)
        Me.GrpRange.Controls.Add(Me.txtToDay6)
        Me.GrpRange.Controls.Add(Me.txtFromDay5)
        Me.GrpRange.CustomGroupBoxColor = System.Drawing.Color.White
        Me.GrpRange.GroupImage = Nothing
        Me.GrpRange.GroupTitle = ""
        Me.GrpRange.Location = New System.Drawing.Point(2, -1)
        Me.GrpRange.Name = "GrpRange"
        Me.GrpRange.Padding = New System.Windows.Forms.Padding(20)
        Me.GrpRange.PaintGroupBox = False
        Me.GrpRange.RoundCorners = 10
        Me.GrpRange.ShadowColor = System.Drawing.Color.DarkGray
        Me.GrpRange.ShadowControl = False
        Me.GrpRange.ShadowThickness = 3
        Me.GrpRange.Size = New System.Drawing.Size(222, 196)
        Me.GrpRange.TabIndex = 4
        '
        'PnlRange
        '
        Me.PnlRange.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.PnlRange.Controls.Add(Me.GrpRange)
        Me.PnlRange.Location = New System.Drawing.Point(358, 145)
        Me.PnlRange.Name = "PnlRange"
        Me.PnlRange.Size = New System.Drawing.Size(226, 197)
        Me.PnlRange.TabIndex = 5
        '
        'frmduetran
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 480)
        Me.ContextMenuStrip = Me.contextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.PnlRange)
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmduetran"
        Me.Text = "Outstanding"
        Me.contextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.PnlValuation.ResumeLayout(False)
        Me.PnlValuation.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.GrpRange.ResumeLayout(False)
        Me.GrpRange.PerformLayout()
        Me.PnlRange.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Private WithEvents newToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Private WithEvents contextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Private WithEvents viewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents exitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbCompany As System.Windows.Forms.ComboBox
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents cmbAcName_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents rdbTranDateWise As System.Windows.Forms.RadioButton
    Friend WithEvents rdbBillDateWise As System.Windows.Forms.RadioButton
    Friend WithEvents chkBasedOnMasterDays As System.Windows.Forms.CheckBox
    Friend WithEvents chkRange As System.Windows.Forms.CheckBox
    Friend WithEvents txtToDay1 As System.Windows.Forms.TextBox
    Friend WithEvents txtToDay3 As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDay2 As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDay3 As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDay1 As System.Windows.Forms.TextBox
    Friend WithEvents txtToDay2 As System.Windows.Forms.TextBox
    Friend WithEvents PnlValuation As System.Windows.Forms.Panel
    Friend WithEvents rbtFifo As System.Windows.Forms.RadioButton
    Friend WithEvents rbtNormal As System.Windows.Forms.RadioButton
    Friend WithEvents txtToDay4 As System.Windows.Forms.TextBox
    Friend WithEvents txtToDay6 As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDay5 As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDay6 As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDay4 As System.Windows.Forms.TextBox
    Friend WithEvents txtToDay5 As System.Windows.Forms.TextBox
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoReziseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents GrpRange As CodeVendor.Controls.Grouper
    Friend WithEvents PnlRange As System.Windows.Forms.Panel
End Class
