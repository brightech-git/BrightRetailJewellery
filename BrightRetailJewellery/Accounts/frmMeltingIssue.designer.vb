<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMeltingIssue
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbSmith_OWN = New System.Windows.Forms.ComboBox()
        Me.txtBagNo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtPurity_PER = New System.Windows.Forms.TextBox()
        Me.txtPcs_NUM = New System.Windows.Forms.TextBox()
        Me.txtWeight_WET = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtRemark1 = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtRemark2 = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtRate_AMT = New System.Windows.Forms.TextBox()
        Me.GridView = New System.Windows.Forms.DataGridView()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.gridViewPendingBagNo = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.CmbTrantype = New System.Windows.Forms.ComboBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.dtpBillDate = New BrighttechPack.DatePicker(Me.components)
        Me.chkPurchase = New System.Windows.Forms.CheckBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtIssueLessStnWt_WET = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtIssueNetWt_WET = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtIssueWeight_WET = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtDiffWeight_WET = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.lblTranNo = New System.Windows.Forms.Label()
        Me.CmbCompany_MAN = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.CmbCategory_OWN = New System.Windows.Forms.ComboBox()
        Me.txtGrpbag = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtLessStnWt_WET = New System.Windows.Forms.TextBox()
        Me.lblhelp = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtNetWt_WET = New System.Windows.Forms.TextBox()
        Me.dtpDate = New BrighttechPack.DatePicker(Me.components)
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.btnBAck = New System.Windows.Forms.Button()
        Me.gridViewOpen = New System.Windows.Forms.DataGridView()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridViewPendingBagNo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridViewOpen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 87)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Smith Name"
        '
        'cmbSmith_OWN
        '
        Me.cmbSmith_OWN.FormattingEnabled = True
        Me.cmbSmith_OWN.Location = New System.Drawing.Point(86, 83)
        Me.cmbSmith_OWN.Name = "cmbSmith_OWN"
        Me.cmbSmith_OWN.Size = New System.Drawing.Size(345, 21)
        Me.cmbSmith_OWN.TabIndex = 9
        '
        'txtBagNo
        '
        Me.txtBagNo.Enabled = False
        Me.txtBagNo.Location = New System.Drawing.Point(86, 107)
        Me.txtBagNo.Name = "txtBagNo"
        Me.txtBagNo.Size = New System.Drawing.Size(74, 21)
        Me.txtBagNo.TabIndex = 11
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 111)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Bag No"
        '
        'txtPurity_PER
        '
        Me.txtPurity_PER.Location = New System.Drawing.Point(86, 155)
        Me.txtPurity_PER.Name = "txtPurity_PER"
        Me.txtPurity_PER.Size = New System.Drawing.Size(74, 21)
        Me.txtPurity_PER.TabIndex = 16
        '
        'txtPcs_NUM
        '
        Me.txtPcs_NUM.Location = New System.Drawing.Point(358, 156)
        Me.txtPcs_NUM.Name = "txtPcs_NUM"
        Me.txtPcs_NUM.Size = New System.Drawing.Size(74, 21)
        Me.txtPcs_NUM.TabIndex = 20
        '
        'txtWeight_WET
        '
        Me.txtWeight_WET.Location = New System.Drawing.Point(86, 204)
        Me.txtWeight_WET.Name = "txtWeight_WET"
        Me.txtWeight_WET.Size = New System.Drawing.Size(74, 21)
        Me.txtWeight_WET.TabIndex = 23
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 12)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(63, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Tran Date"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 135)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Category"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 159)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Purity"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(326, 158)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(26, 13)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "Pcs"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 208)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(45, 13)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "Weight"
        '
        'txtRemark1
        '
        Me.txtRemark1.Location = New System.Drawing.Point(86, 304)
        Me.txtRemark1.MaxLength = 50
        Me.txtRemark1.Name = "txtRemark1"
        Me.txtRemark1.Size = New System.Drawing.Size(345, 21)
        Me.txtRemark1.TabIndex = 38
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 308)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(52, 13)
        Me.Label7.TabIndex = 37
        Me.Label7.Text = "Remark"
        '
        'txtRemark2
        '
        Me.txtRemark2.Location = New System.Drawing.Point(86, 326)
        Me.txtRemark2.MaxLength = 50
        Me.txtRemark2.Name = "txtRemark2"
        Me.txtRemark2.Size = New System.Drawing.Size(345, 21)
        Me.txtRemark2.TabIndex = 39
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(165, 159)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(75, 13)
        Me.Label8.TabIndex = 17
        Me.Label8.Text = "Bullion Rate"
        '
        'txtRate_AMT
        '
        Me.txtRate_AMT.Location = New System.Drawing.Point(241, 155)
        Me.txtRate_AMT.Name = "txtRate_AMT"
        Me.txtRate_AMT.Size = New System.Drawing.Size(82, 21)
        Me.txtRate_AMT.TabIndex = 18
        '
        'GridView
        '
        Me.GridView.AllowUserToAddRows = False
        Me.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView.Location = New System.Drawing.Point(5, 389)
        Me.GridView.Name = "GridView"
        Me.GridView.ReadOnly = True
        Me.GridView.Size = New System.Drawing.Size(871, 192)
        Me.GridView.TabIndex = 48
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(86, 353)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(100, 30)
        Me.btnAdd.TabIndex = 40
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(192, 353)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 41
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(403, 353)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 43
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(508, 353)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 44
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'gridViewPendingBagNo
        '
        Me.gridViewPendingBagNo.AllowUserToAddRows = False
        Me.gridViewPendingBagNo.AllowUserToDeleteRows = False
        Me.gridViewPendingBagNo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewPendingBagNo.Location = New System.Drawing.Point(449, 12)
        Me.gridViewPendingBagNo.Name = "gridViewPendingBagNo"
        Me.gridViewPendingBagNo.ReadOnly = True
        Me.gridViewPendingBagNo.RowHeadersVisible = False
        Me.gridViewPendingBagNo.Size = New System.Drawing.Size(428, 239)
        Me.gridViewPendingBagNo.TabIndex = 46
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.OpenToolStripMenuItem})
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
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(297, 353)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 42
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(896, 615)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.CmbTrantype)
        Me.tabGeneral.Controls.Add(Me.Label21)
        Me.tabGeneral.Controls.Add(Me.dtpBillDate)
        Me.tabGeneral.Controls.Add(Me.chkPurchase)
        Me.tabGeneral.Controls.Add(Me.Label18)
        Me.tabGeneral.Controls.Add(Me.txtIssueLessStnWt_WET)
        Me.tabGeneral.Controls.Add(Me.Label19)
        Me.tabGeneral.Controls.Add(Me.txtIssueNetWt_WET)
        Me.tabGeneral.Controls.Add(Me.Label20)
        Me.tabGeneral.Controls.Add(Me.txtIssueWeight_WET)
        Me.tabGeneral.Controls.Add(Me.Label17)
        Me.tabGeneral.Controls.Add(Me.Label16)
        Me.tabGeneral.Controls.Add(Me.txtDiffWeight_WET)
        Me.tabGeneral.Controls.Add(Me.Label15)
        Me.tabGeneral.Controls.Add(Me.lblTranNo)
        Me.tabGeneral.Controls.Add(Me.CmbCompany_MAN)
        Me.tabGeneral.Controls.Add(Me.Label13)
        Me.tabGeneral.Controls.Add(Me.CmbCategory_OWN)
        Me.tabGeneral.Controls.Add(Me.txtGrpbag)
        Me.tabGeneral.Controls.Add(Me.Label12)
        Me.tabGeneral.Controls.Add(Me.txtLessStnWt_WET)
        Me.tabGeneral.Controls.Add(Me.lblhelp)
        Me.tabGeneral.Controls.Add(Me.Label11)
        Me.tabGeneral.Controls.Add(Me.txtNetWt_WET)
        Me.tabGeneral.Controls.Add(Me.dtpDate)
        Me.tabGeneral.Controls.Add(Me.Label9)
        Me.tabGeneral.Controls.Add(Me.GridView)
        Me.tabGeneral.Controls.Add(Me.cmbSmith_OWN)
        Me.tabGeneral.Controls.Add(Me.btnOpen)
        Me.tabGeneral.Controls.Add(Me.Label2)
        Me.tabGeneral.Controls.Add(Me.gridViewPendingBagNo)
        Me.tabGeneral.Controls.Add(Me.Label3)
        Me.tabGeneral.Controls.Add(Me.btnSave)
        Me.tabGeneral.Controls.Add(Me.Label4)
        Me.tabGeneral.Controls.Add(Me.btnExit)
        Me.tabGeneral.Controls.Add(Me.Label5)
        Me.tabGeneral.Controls.Add(Me.btnNew)
        Me.tabGeneral.Controls.Add(Me.Label8)
        Me.tabGeneral.Controls.Add(Me.btnAdd)
        Me.tabGeneral.Controls.Add(Me.Label6)
        Me.tabGeneral.Controls.Add(Me.Label7)
        Me.tabGeneral.Controls.Add(Me.txtWeight_WET)
        Me.tabGeneral.Controls.Add(Me.txtBagNo)
        Me.tabGeneral.Controls.Add(Me.txtRemark2)
        Me.tabGeneral.Controls.Add(Me.Label1)
        Me.tabGeneral.Controls.Add(Me.txtPurity_PER)
        Me.tabGeneral.Controls.Add(Me.txtRemark1)
        Me.tabGeneral.Controls.Add(Me.txtRate_AMT)
        Me.tabGeneral.Controls.Add(Me.txtPcs_NUM)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(888, 589)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'CmbTrantype
        '
        Me.CmbTrantype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbTrantype.FormattingEnabled = True
        Me.CmbTrantype.Location = New System.Drawing.Point(259, 34)
        Me.CmbTrantype.Name = "CmbTrantype"
        Me.CmbTrantype.Size = New System.Drawing.Size(171, 21)
        Me.CmbTrantype.TabIndex = 5
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(194, 37)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(59, 13)
        Me.Label21.TabIndex = 4
        Me.Label21.Text = "TranType"
        '
        'dtpBillDate
        '
        Me.dtpBillDate.Location = New System.Drawing.Point(86, 33)
        Me.dtpBillDate.Mask = "##/##/####"
        Me.dtpBillDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpBillDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpBillDate.Name = "dtpBillDate"
        Me.dtpBillDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpBillDate.Size = New System.Drawing.Size(100, 21)
        Me.dtpBillDate.TabIndex = 3
        Me.dtpBillDate.Text = "06/03/9998"
        Me.dtpBillDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'chkPurchase
        '
        Me.chkPurchase.AutoSize = True
        Me.chkPurchase.Location = New System.Drawing.Point(9, 35)
        Me.chkPurchase.Name = "chkPurchase"
        Me.chkPurchase.Size = New System.Drawing.Size(74, 17)
        Me.chkPurchase.TabIndex = 2
        Me.chkPurchase.Text = "Bill Date"
        Me.chkPurchase.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(165, 283)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(75, 13)
        Me.Label18.TabIndex = 33
        Me.Label18.Text = "Less/Stn Wt"
        '
        'txtIssueLessStnWt_WET
        '
        Me.txtIssueLessStnWt_WET.Location = New System.Drawing.Point(241, 279)
        Me.txtIssueLessStnWt_WET.Name = "txtIssueLessStnWt_WET"
        Me.txtIssueLessStnWt_WET.ReadOnly = True
        Me.txtIssueLessStnWt_WET.Size = New System.Drawing.Size(82, 21)
        Me.txtIssueLessStnWt_WET.TabIndex = 34
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(326, 283)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(26, 13)
        Me.Label19.TabIndex = 35
        Me.Label19.Text = "Net"
        '
        'txtIssueNetWt_WET
        '
        Me.txtIssueNetWt_WET.Location = New System.Drawing.Point(358, 280)
        Me.txtIssueNetWt_WET.Name = "txtIssueNetWt_WET"
        Me.txtIssueNetWt_WET.ReadOnly = True
        Me.txtIssueNetWt_WET.Size = New System.Drawing.Size(75, 21)
        Me.txtIssueNetWt_WET.TabIndex = 36
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(8, 283)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(45, 13)
        Me.Label20.TabIndex = 31
        Me.Label20.Text = "Weight"
        '
        'txtIssueWeight_WET
        '
        Me.txtIssueWeight_WET.Location = New System.Drawing.Point(86, 279)
        Me.txtIssueWeight_WET.Name = "txtIssueWeight_WET"
        Me.txtIssueWeight_WET.ReadOnly = True
        Me.txtIssueWeight_WET.Size = New System.Drawing.Size(74, 21)
        Me.txtIssueWeight_WET.TabIndex = 32
        '
        'Label17
        '
        Me.Label17.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(10, 260)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(421, 13)
        Me.Label17.TabIndex = 30
        Me.Label17.Text = "ISSUED"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(6, 235)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(69, 13)
        Me.Label16.TabIndex = 28
        Me.Label16.Text = "Diff Weight"
        '
        'txtDiffWeight_WET
        '
        Me.txtDiffWeight_WET.Location = New System.Drawing.Point(86, 227)
        Me.txtDiffWeight_WET.Name = "txtDiffWeight_WET"
        Me.txtDiffWeight_WET.Size = New System.Drawing.Size(74, 21)
        Me.txtDiffWeight_WET.TabIndex = 29
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(9, 185)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(421, 13)
        Me.Label15.TabIndex = 21
        Me.Label15.Text = "BAG"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTranNo
        '
        Me.lblTranNo.AutoSize = True
        Me.lblTranNo.Location = New System.Drawing.Point(238, 12)
        Me.lblTranNo.Name = "lblTranNo"
        Me.lblTranNo.Size = New System.Drawing.Size(19, 13)
        Me.lblTranNo.TabIndex = 45
        Me.lblTranNo.Text = "..."
        '
        'CmbCompany_MAN
        '
        Me.CmbCompany_MAN.FormattingEnabled = True
        Me.CmbCompany_MAN.Location = New System.Drawing.Point(86, 58)
        Me.CmbCompany_MAN.Name = "CmbCompany_MAN"
        Me.CmbCompany_MAN.Size = New System.Drawing.Size(344, 21)
        Me.CmbCompany_MAN.TabIndex = 7
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 62)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(62, 13)
        Me.Label13.TabIndex = 6
        Me.Label13.Text = "Company"
        '
        'CmbCategory_OWN
        '
        Me.CmbCategory_OWN.FormattingEnabled = True
        Me.CmbCategory_OWN.Location = New System.Drawing.Point(86, 131)
        Me.CmbCategory_OWN.Name = "CmbCategory_OWN"
        Me.CmbCategory_OWN.Size = New System.Drawing.Size(345, 21)
        Me.CmbCategory_OWN.TabIndex = 14
        '
        'txtGrpbag
        '
        Me.txtGrpbag.Enabled = False
        Me.txtGrpbag.Location = New System.Drawing.Point(297, 107)
        Me.txtGrpbag.Name = "txtGrpbag"
        Me.txtGrpbag.Size = New System.Drawing.Size(74, 21)
        Me.txtGrpbag.TabIndex = 12
        Me.txtGrpbag.Visible = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(165, 208)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(75, 13)
        Me.Label12.TabIndex = 24
        Me.Label12.Text = "Less/Stn Wt"
        '
        'txtLessStnWt_WET
        '
        Me.txtLessStnWt_WET.Location = New System.Drawing.Point(241, 204)
        Me.txtLessStnWt_WET.Name = "txtLessStnWt_WET"
        Me.txtLessStnWt_WET.Size = New System.Drawing.Size(82, 21)
        Me.txtLessStnWt_WET.TabIndex = 25
        '
        'lblhelp
        '
        Me.lblhelp.AutoSize = True
        Me.lblhelp.ForeColor = System.Drawing.Color.Red
        Me.lblhelp.Location = New System.Drawing.Point(623, 238)
        Me.lblhelp.Name = "lblhelp"
        Me.lblhelp.Size = New System.Drawing.Size(0, 13)
        Me.lblhelp.TabIndex = 47
        Me.lblhelp.Visible = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(326, 208)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(26, 13)
        Me.Label11.TabIndex = 26
        Me.Label11.Text = "Net"
        '
        'txtNetWt_WET
        '
        Me.txtNetWt_WET.Location = New System.Drawing.Point(358, 205)
        Me.txtNetWt_WET.Name = "txtNetWt_WET"
        Me.txtNetWt_WET.Size = New System.Drawing.Size(75, 21)
        Me.txtNetWt_WET.TabIndex = 27
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(86, 9)
        Me.dtpDate.Mask = "##/##/####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDate.Size = New System.Drawing.Size(100, 21)
        Me.dtpDate.TabIndex = 1
        Me.dtpDate.Text = "06/03/9998"
        Me.dtpDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.btnPrint)
        Me.tabView.Controls.Add(Me.btnExport)
        Me.tabView.Controls.Add(Me.Label14)
        Me.tabView.Controls.Add(Me.Label10)
        Me.tabView.Controls.Add(Me.btnBAck)
        Me.tabView.Controls.Add(Me.gridViewOpen)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(888, 589)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(216, 425)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 6
        Me.btnPrint.TabStop = False
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(113, 425)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 5
        Me.btnExport.TabStop = False
        Me.btnExport.Text = "Export[X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.Red
        Me.Label14.Location = New System.Drawing.Point(88, 458)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(83, 13)
        Me.Label14.TabIndex = 3
        Me.Label14.Text = "[Enter] Edit"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Red
        Me.Label10.Location = New System.Drawing.Point(8, 458)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(74, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "[C] Cancel"
        '
        'btnBAck
        '
        Me.btnBAck.Location = New System.Drawing.Point(11, 425)
        Me.btnBAck.Name = "btnBAck"
        Me.btnBAck.Size = New System.Drawing.Size(100, 30)
        Me.btnBAck.TabIndex = 1
        Me.btnBAck.TabStop = False
        Me.btnBAck.Text = "&Back"
        Me.btnBAck.UseVisualStyleBackColor = True
        '
        'gridViewOpen
        '
        Me.gridViewOpen.AllowUserToAddRows = False
        Me.gridViewOpen.AllowUserToDeleteRows = False
        Me.gridViewOpen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewOpen.Location = New System.Drawing.Point(8, 6)
        Me.gridViewOpen.Name = "gridViewOpen"
        Me.gridViewOpen.ReadOnly = True
        Me.gridViewOpen.Size = New System.Drawing.Size(871, 413)
        Me.gridViewOpen.TabIndex = 0
        '
        'frmMeltingIssue
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(896, 615)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmMeltingIssue"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Melting Issue"
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridViewPendingBagNo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.tabView.PerformLayout()
        CType(Me.gridViewOpen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtBagNo As System.Windows.Forms.TextBox
    Friend WithEvents cmbSmith_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPurity_PER As System.Windows.Forms.TextBox
    Friend WithEvents txtPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtRemark1 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtRemark2 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents GridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents gridViewPendingBagNo As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents btnBAck As System.Windows.Forms.Button
    Friend WithEvents gridViewOpen As System.Windows.Forms.DataGridView
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents dtpDate As BrighttechPack.DatePicker
    Friend WithEvents txtNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblhelp As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtLessStnWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtGrpbag As System.Windows.Forms.TextBox
    Friend WithEvents CmbCategory_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents CmbCompany_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents lblTranNo As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents btnPrint As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents Label16 As Label
    Friend WithEvents txtDiffWeight_WET As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents txtIssueLessStnWt_WET As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents txtIssueNetWt_WET As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents txtIssueWeight_WET As TextBox
    Friend WithEvents Label21 As Label
    Friend WithEvents dtpBillDate As BrighttechPack.DatePicker
    Friend WithEvents chkPurchase As CheckBox
    Friend WithEvents CmbTrantype As ComboBox
End Class
