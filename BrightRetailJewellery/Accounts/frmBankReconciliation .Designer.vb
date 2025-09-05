<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBankReconciliation
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkrupdate = New System.Windows.Forms.CheckBox()
        Me.chkBasedOnTrandate = New System.Windows.Forms.CheckBox()
        Me.lblhint = New System.Windows.Forms.Label()
        Me.lblStatus1 = New System.Windows.Forms.Label()
        Me.lblBankName = New System.Windows.Forms.Label()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.Label = New System.Windows.Forms.Label()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rbtOthers = New System.Windows.Forms.RadioButton()
        Me.rbtBank = New System.Windows.Forms.RadioButton()
        Me.dtpGridRealise = New System.Windows.Forms.DateTimePicker()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.gridView_OWN = New System.Windows.Forms.DataGridView()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.pnlRealised = New System.Windows.Forms.Panel()
        Me.dtpRealiseDateTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpTranDateTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpRealiseDateFrom = New BrighttechPack.DatePicker(Me.components)
        Me.dtpTranDateFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.pnlUnRealised = New System.Windows.Forms.Panel()
        Me.ChkAson = New System.Windows.Forms.CheckBox()
        Me.LblUnrealizeDateTo = New System.Windows.Forms.Label()
        Me.dtpUnrealizeDateTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.rbtUnRealised = New System.Windows.Forms.RadioButton()
        Me.rbtRealised = New System.Windows.Forms.RadioButton()
        Me.cmbBank_MAN = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.dtpGridRealiseBulk = New System.Windows.Forms.DateTimePicker()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlRealised.SuspendLayout()
        Me.pnlUnRealised.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.dtpGridRealiseBulk)
        Me.GroupBox1.Controls.Add(Me.chkrupdate)
        Me.GroupBox1.Controls.Add(Me.chkBasedOnTrandate)
        Me.GroupBox1.Controls.Add(Me.lblhint)
        Me.GroupBox1.Controls.Add(Me.lblStatus1)
        Me.GroupBox1.Controls.Add(Me.lblBankName)
        Me.GroupBox1.Controls.Add(Me.chkCmbCostCentre)
        Me.GroupBox1.Controls.Add(Me.Label)
        Me.GroupBox1.Controls.Add(Me.btnPrint)
        Me.GroupBox1.Controls.Add(Me.btnExport)
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Controls.Add(Me.dtpGridRealise)
        Me.GroupBox1.Controls.Add(Me.lblStatus)
        Me.GroupBox1.Controls.Add(Me.gridView_OWN)
        Me.GroupBox1.Controls.Add(Me.btnExit)
        Me.GroupBox1.Controls.Add(Me.btnNew)
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Controls.Add(Me.pnlRealised)
        Me.GroupBox1.Controls.Add(Me.pnlUnRealised)
        Me.GroupBox1.Controls.Add(Me.rbtUnRealised)
        Me.GroupBox1.Controls.Add(Me.rbtRealised)
        Me.GroupBox1.Controls.Add(Me.cmbBank_MAN)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 11)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(998, 616)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'chkrupdate
        '
        Me.chkrupdate.AutoSize = True
        Me.chkrupdate.Location = New System.Drawing.Point(352, 97)
        Me.chkrupdate.Name = "chkrupdate"
        Me.chkrupdate.Size = New System.Drawing.Size(140, 17)
        Me.chkrupdate.TabIndex = 21
        Me.chkrupdate.Text = "Realize Bulk Update"
        Me.chkrupdate.UseVisualStyleBackColor = True
        '
        'chkBasedOnTrandate
        '
        Me.chkBasedOnTrandate.AutoSize = True
        Me.chkBasedOnTrandate.Location = New System.Drawing.Point(352, 78)
        Me.chkBasedOnTrandate.Name = "chkBasedOnTrandate"
        Me.chkBasedOnTrandate.Size = New System.Drawing.Size(135, 17)
        Me.chkBasedOnTrandate.TabIndex = 10
        Me.chkBasedOnTrandate.Text = "Based On Trandate"
        Me.chkBasedOnTrandate.UseVisualStyleBackColor = True
        '
        'lblhint
        '
        Me.lblhint.AutoSize = True
        Me.lblhint.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.lblhint.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblhint.Location = New System.Drawing.Point(565, 599)
        Me.lblhint.Name = "lblhint"
        Me.lblhint.Size = New System.Drawing.Size(0, 13)
        Me.lblhint.TabIndex = 20
        '
        'lblStatus1
        '
        Me.lblStatus1.AutoSize = True
        Me.lblStatus1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus1.ForeColor = System.Drawing.Color.Red
        Me.lblStatus1.Location = New System.Drawing.Point(501, 58)
        Me.lblStatus1.Name = "lblStatus1"
        Me.lblStatus1.Size = New System.Drawing.Size(0, 13)
        Me.lblStatus1.TabIndex = 17
        '
        'lblBankName
        '
        Me.lblBankName.AutoSize = True
        Me.lblBankName.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.lblBankName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBankName.Location = New System.Drawing.Point(11, 597)
        Me.lblBankName.Name = "lblBankName"
        Me.lblBankName.Size = New System.Drawing.Size(88, 13)
        Me.lblBankName.TabIndex = 19
        Me.lblBankName.Text = "Bank Name :"
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(135, 75)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(210, 22)
        Me.chkCmbCostCentre.TabIndex = 6
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(16, 84)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 5
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(437, 152)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 16
        Me.btnPrint.Text = "Print [P]"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(335, 152)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 15
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtOthers)
        Me.Panel1.Controls.Add(Me.rbtBank)
        Me.Panel1.Location = New System.Drawing.Point(133, 16)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(146, 27)
        Me.Panel1.TabIndex = 1
        '
        'rbtOthers
        '
        Me.rbtOthers.AutoSize = True
        Me.rbtOthers.Location = New System.Drawing.Point(63, 5)
        Me.rbtOthers.Name = "rbtOthers"
        Me.rbtOthers.Size = New System.Drawing.Size(63, 17)
        Me.rbtOthers.TabIndex = 1
        Me.rbtOthers.Text = "Others"
        Me.rbtOthers.UseVisualStyleBackColor = True
        '
        'rbtBank
        '
        Me.rbtBank.AutoSize = True
        Me.rbtBank.Checked = True
        Me.rbtBank.Location = New System.Drawing.Point(3, 5)
        Me.rbtBank.Name = "rbtBank"
        Me.rbtBank.Size = New System.Drawing.Size(54, 17)
        Me.rbtBank.TabIndex = 0
        Me.rbtBank.TabStop = True
        Me.rbtBank.Text = "Bank"
        Me.rbtBank.UseVisualStyleBackColor = True
        '
        'dtpGridRealise
        '
        Me.dtpGridRealise.CustomFormat = "dd/MM/yyyy"
        Me.dtpGridRealise.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpGridRealise.Location = New System.Drawing.Point(491, 73)
        Me.dtpGridRealise.Name = "dtpGridRealise"
        Me.dtpGridRealise.Size = New System.Drawing.Size(93, 21)
        Me.dtpGridRealise.TabIndex = 12
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(501, 43)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(50, 13)
        Me.lblStatus.TabIndex = 4
        Me.lblStatus.Text = "Label3"
        '
        'gridView_OWN
        '
        Me.gridView_OWN.AllowUserToAddRows = False
        Me.gridView_OWN.AllowUserToDeleteRows = False
        Me.gridView_OWN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView_OWN.Location = New System.Drawing.Point(6, 187)
        Me.gridView_OWN.Name = "gridView_OWN"
        Me.gridView_OWN.ReadOnly = True
        Me.gridView_OWN.RowHeadersVisible = False
        Me.gridView_OWN.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView_OWN.Size = New System.Drawing.Size(986, 405)
        Me.gridView_OWN.TabIndex = 18
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(539, 152)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 17
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(233, 152)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 14
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(131, 152)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 13
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'pnlRealised
        '
        Me.pnlRealised.Controls.Add(Me.dtpRealiseDateTo)
        Me.pnlRealised.Controls.Add(Me.dtpTranDateTo)
        Me.pnlRealised.Controls.Add(Me.dtpRealiseDateFrom)
        Me.pnlRealised.Controls.Add(Me.dtpTranDateFrom)
        Me.pnlRealised.Controls.Add(Me.Label8)
        Me.pnlRealised.Controls.Add(Me.Label5)
        Me.pnlRealised.Controls.Add(Me.Label7)
        Me.pnlRealised.Controls.Add(Me.Label6)
        Me.pnlRealised.Location = New System.Drawing.Point(14, 120)
        Me.pnlRealised.Name = "pnlRealised"
        Me.pnlRealised.Size = New System.Drawing.Size(674, 24)
        Me.pnlRealised.TabIndex = 11
        '
        'dtpRealiseDateTo
        '
        Me.dtpRealiseDateTo.Location = New System.Drawing.Point(575, 2)
        Me.dtpRealiseDateTo.Mask = "##/##/####"
        Me.dtpRealiseDateTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpRealiseDateTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpRealiseDateTo.Name = "dtpRealiseDateTo"
        Me.dtpRealiseDateTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpRealiseDateTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpRealiseDateTo.TabIndex = 7
        Me.dtpRealiseDateTo.Text = "06/03/9998"
        Me.dtpRealiseDateTo.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpTranDateTo
        '
        Me.dtpTranDateTo.Location = New System.Drawing.Point(238, 2)
        Me.dtpTranDateTo.Mask = "##/##/####"
        Me.dtpTranDateTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTranDateTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTranDateTo.Name = "dtpTranDateTo"
        Me.dtpTranDateTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTranDateTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTranDateTo.TabIndex = 3
        Me.dtpTranDateTo.Text = "06/03/9998"
        Me.dtpTranDateTo.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpRealiseDateFrom
        '
        Me.dtpRealiseDateFrom.Location = New System.Drawing.Point(458, 2)
        Me.dtpRealiseDateFrom.Mask = "##/##/####"
        Me.dtpRealiseDateFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpRealiseDateFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpRealiseDateFrom.Name = "dtpRealiseDateFrom"
        Me.dtpRealiseDateFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpRealiseDateFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpRealiseDateFrom.TabIndex = 5
        Me.dtpRealiseDateFrom.Text = "06/03/9998"
        Me.dtpRealiseDateFrom.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpTranDateFrom
        '
        Me.dtpTranDateFrom.Location = New System.Drawing.Point(120, 2)
        Me.dtpTranDateFrom.Mask = "##/##/####"
        Me.dtpTranDateFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTranDateFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTranDateFrom.Name = "dtpTranDateFrom"
        Me.dtpTranDateFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTranDateFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpTranDateFrom.TabIndex = 1
        Me.dtpTranDateFrom.Text = "06/03/9998"
        Me.dtpTranDateFrom.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(552, 6)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(20, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "To"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(215, 6)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(20, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "To"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(337, 6)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(119, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Realized Date From"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(96, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Tran Date From"
        '
        'pnlUnRealised
        '
        Me.pnlUnRealised.Controls.Add(Me.ChkAson)
        Me.pnlUnRealised.Controls.Add(Me.LblUnrealizeDateTo)
        Me.pnlUnRealised.Controls.Add(Me.dtpUnrealizeDateTo)
        Me.pnlUnRealised.Controls.Add(Me.dtpAsOnDate)
        Me.pnlUnRealised.Location = New System.Drawing.Point(14, 120)
        Me.pnlUnRealised.Name = "pnlUnRealised"
        Me.pnlUnRealised.Size = New System.Drawing.Size(421, 26)
        Me.pnlUnRealised.TabIndex = 9
        '
        'ChkAson
        '
        Me.ChkAson.AutoSize = True
        Me.ChkAson.Checked = True
        Me.ChkAson.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkAson.Location = New System.Drawing.Point(3, 4)
        Me.ChkAson.Name = "ChkAson"
        Me.ChkAson.Size = New System.Drawing.Size(56, 17)
        Me.ChkAson.TabIndex = 0
        Me.ChkAson.Text = "AsOn"
        Me.ChkAson.UseVisualStyleBackColor = True
        '
        'LblUnrealizeDateTo
        '
        Me.LblUnrealizeDateTo.AutoSize = True
        Me.LblUnrealizeDateTo.Location = New System.Drawing.Point(233, 7)
        Me.LblUnrealizeDateTo.Name = "LblUnrealizeDateTo"
        Me.LblUnrealizeDateTo.Size = New System.Drawing.Size(20, 13)
        Me.LblUnrealizeDateTo.TabIndex = 2
        Me.LblUnrealizeDateTo.Text = "To"
        Me.LblUnrealizeDateTo.Visible = False
        '
        'dtpUnrealizeDateTo
        '
        Me.dtpUnrealizeDateTo.Location = New System.Drawing.Point(321, 2)
        Me.dtpUnrealizeDateTo.Mask = "##/##/####"
        Me.dtpUnrealizeDateTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpUnrealizeDateTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpUnrealizeDateTo.Name = "dtpUnrealizeDateTo"
        Me.dtpUnrealizeDateTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpUnrealizeDateTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpUnrealizeDateTo.TabIndex = 3
        Me.dtpUnrealizeDateTo.Text = "06/03/9998"
        Me.dtpUnrealizeDateTo.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        Me.dtpUnrealizeDateTo.Visible = False
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(120, 2)
        Me.dtpAsOnDate.Mask = "##/##/####"
        Me.dtpAsOnDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate.Name = "dtpAsOnDate"
        Me.dtpAsOnDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate.Size = New System.Drawing.Size(93, 21)
        Me.dtpAsOnDate.TabIndex = 1
        Me.dtpAsOnDate.Text = "06/03/9998"
        Me.dtpAsOnDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'rbtUnRealised
        '
        Me.rbtUnRealised.AutoSize = True
        Me.rbtUnRealised.Location = New System.Drawing.Point(135, 98)
        Me.rbtUnRealised.Name = "rbtUnRealised"
        Me.rbtUnRealised.Size = New System.Drawing.Size(92, 17)
        Me.rbtUnRealised.TabIndex = 7
        Me.rbtUnRealised.TabStop = True
        Me.rbtUnRealised.Text = "Un Realized"
        Me.rbtUnRealised.UseVisualStyleBackColor = True
        '
        'rbtRealised
        '
        Me.rbtRealised.AutoSize = True
        Me.rbtRealised.Location = New System.Drawing.Point(233, 98)
        Me.rbtRealised.Name = "rbtRealised"
        Me.rbtRealised.Size = New System.Drawing.Size(73, 17)
        Me.rbtRealised.TabIndex = 8
        Me.rbtRealised.TabStop = True
        Me.rbtRealised.Text = "Realized"
        Me.rbtRealised.UseVisualStyleBackColor = True
        '
        'cmbBank_MAN
        '
        Me.cmbBank_MAN.FormattingEnabled = True
        Me.cmbBank_MAN.Location = New System.Drawing.Point(135, 48)
        Me.cmbBank_MAN.Name = "cmbBank_MAN"
        Me.cmbBank_MAN.Size = New System.Drawing.Size(359, 21)
        Me.cmbBank_MAN.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(42, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Group"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "AcName"
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
        'dtpGridRealiseBulk
        '
        Me.dtpGridRealiseBulk.CustomFormat = "dd/MM/yyyy"
        Me.dtpGridRealiseBulk.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpGridRealiseBulk.Location = New System.Drawing.Point(491, 95)
        Me.dtpGridRealiseBulk.Name = "dtpGridRealiseBulk"
        Me.dtpGridRealiseBulk.Size = New System.Drawing.Size(93, 21)
        Me.dtpGridRealiseBulk.TabIndex = 22
        Me.dtpGridRealiseBulk.Visible = False
        '
        'frmBankReconciliation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.Name = "frmBankReconciliation"
        Me.Text = "Bank Reconciliation"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlRealised.ResumeLayout(False)
        Me.pnlRealised.PerformLayout()
        Me.pnlUnRealised.ResumeLayout(False)
        Me.pnlUnRealised.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents pnlUnRealised As System.Windows.Forms.Panel
    Friend WithEvents rbtUnRealised As System.Windows.Forms.RadioButton
    Friend WithEvents rbtRealised As System.Windows.Forms.RadioButton
    Friend WithEvents cmbBank_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents pnlRealised As System.Windows.Forms.Panel
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents gridView_OWN As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents dtpTranDateTo As BrighttechPack.DatePicker
    Friend WithEvents dtpRealiseDateTo As BrighttechPack.DatePicker
    Friend WithEvents dtpRealiseDateFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpTranDateFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents dtpGridRealise As System.Windows.Forms.DateTimePicker
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtOthers As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBank As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents lblBankName As System.Windows.Forms.Label
    Friend WithEvents lblStatus1 As System.Windows.Forms.Label
    Friend WithEvents lblhint As System.Windows.Forms.Label
    Friend WithEvents chkBasedOnTrandate As CheckBox
    Friend WithEvents LblUnrealizeDateTo As Label
    Friend WithEvents dtpUnrealizeDateTo As BrighttechPack.DatePicker
    Friend WithEvents ChkAson As CheckBox
    Friend WithEvents chkrupdate As CheckBox
    Friend WithEvents dtpGridRealiseBulk As DateTimePicker
End Class
