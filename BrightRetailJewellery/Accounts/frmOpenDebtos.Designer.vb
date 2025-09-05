<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOpenDebtos
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
        Me.txtAccCode = New System.Windows.Forms.TextBox
        Me.cmbCostCentre_MAN = New System.Windows.Forms.ComboBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtTranNo_NUM_MAN = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbMode = New System.Windows.Forms.ComboBox
        Me.txtRefNo_NUM_MAN = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtAmount_AMT = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtWeight_WET = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtRemark = New System.Windows.Forms.TextBox
        Me.txtEmpId_NUM = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.grpDetails = New System.Windows.Forms.GroupBox
        Me.dtpTrandate = New BrighttechPack.DatePicker(Me.components)
        Me.btnOpen = New System.Windows.Forms.Button
        Me.lblTotal = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtAccName_MAN = New System.Windows.Forms.TextBox
        Me.txtAddress1 = New System.Windows.Forms.TextBox
        Me.txtAddress2 = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.tabView = New System.Windows.Forms.TabPage
        Me.dtpOpenTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpOpenFrom = New BrighttechPack.DatePicker(Me.components)
        Me.txtOpenAccName = New System.Windows.Forms.TextBox
        Me.lblEdit = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label33 = New System.Windows.Forms.Label
        Me.Label31 = New System.Windows.Forms.Label
        Me.Label29 = New System.Windows.Forms.Label
        Me.lblRate = New System.Windows.Forms.Label
        Me.lblAddress2 = New System.Windows.Forms.Label
        Me.lblEmpname = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.Label30 = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.Label28 = New System.Windows.Forms.Label
        Me.lblCostcentre = New System.Windows.Forms.Label
        Me.lblPurity = New System.Windows.Forms.Label
        Me.lblAddress1 = New System.Windows.Forms.Label
        Me.lblType = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.cmbOpenType = New System.Windows.Forms.ComboBox
        Me.btnOSearch_View = New System.Windows.Forms.Button
        Me.Label23 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.chkOpenDate = New System.Windows.Forms.CheckBox
        Me.gridOpenView = New System.Windows.Forms.DataGridView
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDetails.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridOpenView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtAccCode
        '
        Me.txtAccCode.Location = New System.Drawing.Point(387, 47)
        Me.txtAccCode.Name = "txtAccCode"
        Me.txtAccCode.Size = New System.Drawing.Size(115, 21)
        Me.txtAccCode.TabIndex = 3
        '
        'cmbCostCentre_MAN
        '
        Me.cmbCostCentre_MAN.FormattingEnabled = True
        Me.cmbCostCentre_MAN.Location = New System.Drawing.Point(118, 47)
        Me.cmbCostCentre_MAN.Name = "cmbCostCentre_MAN"
        Me.cmbCostCentre_MAN.Size = New System.Drawing.Size(197, 21)
        Me.cmbCostCentre_MAN.TabIndex = 1
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(36, 50)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(76, 13)
        Me.Label22.TabIndex = 0
        Me.Label22.Text = "Cost Centre"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(321, 50)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Acc Code"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(25, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Tran No"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtTranNo_NUM_MAN
        '
        Me.txtTranNo_NUM_MAN.Location = New System.Drawing.Point(6, 42)
        Me.txtTranNo_NUM_MAN.Name = "txtTranNo_NUM_MAN"
        Me.txtTranNo_NUM_MAN.Size = New System.Drawing.Size(91, 21)
        Me.txtTranNo_NUM_MAN.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(110, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Tran Date"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMode
        '
        Me.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMode.FormattingEnabled = True
        Me.cmbMode.Location = New System.Drawing.Point(267, 42)
        Me.cmbMode.Name = "cmbMode"
        Me.cmbMode.Size = New System.Drawing.Size(81, 21)
        Me.cmbMode.TabIndex = 7
        '
        'txtRefNo_NUM_MAN
        '
        Me.txtRefNo_NUM_MAN.Location = New System.Drawing.Point(349, 42)
        Me.txtRefNo_NUM_MAN.MaxLength = 9
        Me.txtRefNo_NUM_MAN.Name = "txtRefNo_NUM_MAN"
        Me.txtRefNo_NUM_MAN.Size = New System.Drawing.Size(89, 21)
        Me.txtRefNo_NUM_MAN.TabIndex = 9
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(373, 26)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(41, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "RefNo"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAmount_AMT
        '
        Me.txtAmount_AMT.Location = New System.Drawing.Point(439, 42)
        Me.txtAmount_AMT.MaxLength = 9
        Me.txtAmount_AMT.Name = "txtAmount_AMT"
        Me.txtAmount_AMT.Size = New System.Drawing.Size(98, 21)
        Me.txtAmount_AMT.TabIndex = 11
        Me.txtAmount_AMT.Text = "123456789.00"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(463, 26)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Amount"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtWeight_WET
        '
        Me.txtWeight_WET.Location = New System.Drawing.Point(538, 42)
        Me.txtWeight_WET.MaxLength = 10
        Me.txtWeight_WET.Name = "txtWeight_WET"
        Me.txtWeight_WET.Size = New System.Drawing.Size(89, 21)
        Me.txtWeight_WET.TabIndex = 13
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(559, 26)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(46, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Weight"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRemark
        '
        Me.txtRemark.Location = New System.Drawing.Point(628, 42)
        Me.txtRemark.MaxLength = 75
        Me.txtRemark.Name = "txtRemark"
        Me.txtRemark.Size = New System.Drawing.Size(252, 21)
        Me.txtRemark.TabIndex = 15
        '
        'txtEmpId_NUM
        '
        Me.txtEmpId_NUM.Location = New System.Drawing.Point(881, 42)
        Me.txtEmpId_NUM.MaxLength = 10
        Me.txtEmpId_NUM.Name = "txtEmpId_NUM"
        Me.txtEmpId_NUM.Size = New System.Drawing.Size(48, 21)
        Me.txtEmpId_NUM.TabIndex = 17
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(728, 26)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(52, 13)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Remark"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(881, 26)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(48, 13)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Emp Id"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(285, 26)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(37, 13)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "Mode"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbType
        '
        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbType.FormattingEnabled = True
        Me.cmbType.Location = New System.Drawing.Point(188, 42)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(78, 21)
        Me.cmbType.TabIndex = 5
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(210, 26)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(35, 13)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "Type"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(6, 64)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(943, 324)
        Me.gridView.TabIndex = 18
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(518, 394)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 19
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(726, 394)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 20
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(830, 394)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 21
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'grpDetails
        '
        Me.grpDetails.Controls.Add(Me.dtpTrandate)
        Me.grpDetails.Controls.Add(Me.btnOpen)
        Me.grpDetails.Controls.Add(Me.lblTotal)
        Me.grpDetails.Controls.Add(Me.Label2)
        Me.grpDetails.Controls.Add(Me.txtEmpId_NUM)
        Me.grpDetails.Controls.Add(Me.btnExit)
        Me.grpDetails.Controls.Add(Me.txtTranNo_NUM_MAN)
        Me.grpDetails.Controls.Add(Me.Label8)
        Me.grpDetails.Controls.Add(Me.btnNew)
        Me.grpDetails.Controls.Add(Me.Label3)
        Me.grpDetails.Controls.Add(Me.btnSave)
        Me.grpDetails.Controls.Add(Me.Label4)
        Me.grpDetails.Controls.Add(Me.gridView)
        Me.grpDetails.Controls.Add(Me.txtRemark)
        Me.grpDetails.Controls.Add(Me.Label10)
        Me.grpDetails.Controls.Add(Me.cmbType)
        Me.grpDetails.Controls.Add(Me.Label9)
        Me.grpDetails.Controls.Add(Me.Label5)
        Me.grpDetails.Controls.Add(Me.txtWeight_WET)
        Me.grpDetails.Controls.Add(Me.Label7)
        Me.grpDetails.Controls.Add(Me.txtAmount_AMT)
        Me.grpDetails.Controls.Add(Me.Label6)
        Me.grpDetails.Controls.Add(Me.txtRefNo_NUM_MAN)
        Me.grpDetails.Controls.Add(Me.cmbMode)
        Me.grpDetails.Location = New System.Drawing.Point(31, 144)
        Me.grpDetails.Name = "grpDetails"
        Me.grpDetails.Size = New System.Drawing.Size(955, 434)
        Me.grpDetails.TabIndex = 9
        Me.grpDetails.TabStop = False
        '
        'dtpTrandate
        '
        Me.dtpTrandate.Location = New System.Drawing.Point(98, 42)
        Me.dtpTrandate.Mask = "##/##/####"
        Me.dtpTrandate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTrandate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTrandate.Name = "dtpTrandate"
        Me.dtpTrandate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTrandate.Size = New System.Drawing.Size(89, 21)
        Me.dtpTrandate.TabIndex = 3
        Me.dtpTrandate.Text = "06/03/9998"
        Me.dtpTrandate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(622, 394)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 23
        Me.btnOpen.Text = "Open"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.ForeColor = System.Drawing.Color.Red
        Me.lblTotal.Location = New System.Drawing.Point(353, 401)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(44, 16)
        Me.lblTotal.TabIndex = 22
        Me.lblTotal.Text = "Total"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(36, 83)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(64, 13)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Acc Name"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAccName_MAN
        '
        Me.txtAccName_MAN.Location = New System.Drawing.Point(118, 80)
        Me.txtAccName_MAN.Name = "txtAccName_MAN"
        Me.txtAccName_MAN.Size = New System.Drawing.Size(384, 21)
        Me.txtAccName_MAN.TabIndex = 5
        '
        'txtAddress1
        '
        Me.txtAddress1.Location = New System.Drawing.Point(118, 100)
        Me.txtAddress1.Name = "txtAddress1"
        Me.txtAddress1.Size = New System.Drawing.Size(384, 21)
        Me.txtAddress1.TabIndex = 7
        '
        'txtAddress2
        '
        Me.txtAddress2.Location = New System.Drawing.Point(118, 117)
        Me.txtAddress2.Name = "txtAddress2"
        Me.txtAddress2.Size = New System.Drawing.Size(384, 21)
        Me.txtAddress2.TabIndex = 8
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(36, 104)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(53, 13)
        Me.Label12.TabIndex = 6
        Me.Label12.Text = "Address"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.OpenToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(120, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'tabMain
        '
        Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1020, 630)
        Me.tabMain.TabIndex = 10
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.Label22)
        Me.tabGeneral.Controls.Add(Me.txtAccCode)
        Me.tabGeneral.Controls.Add(Me.grpDetails)
        Me.tabGeneral.Controls.Add(Me.txtAccName_MAN)
        Me.tabGeneral.Controls.Add(Me.cmbCostCentre_MAN)
        Me.tabGeneral.Controls.Add(Me.txtAddress1)
        Me.tabGeneral.Controls.Add(Me.Label1)
        Me.tabGeneral.Controls.Add(Me.txtAddress2)
        Me.tabGeneral.Controls.Add(Me.Label12)
        Me.tabGeneral.Controls.Add(Me.Label11)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 25)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1012, 601)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.dtpOpenTo)
        Me.tabView.Controls.Add(Me.dtpOpenFrom)
        Me.tabView.Controls.Add(Me.txtOpenAccName)
        Me.tabView.Controls.Add(Me.lblEdit)
        Me.tabView.Controls.Add(Me.Label16)
        Me.tabView.Controls.Add(Me.Label33)
        Me.tabView.Controls.Add(Me.Label31)
        Me.tabView.Controls.Add(Me.Label29)
        Me.tabView.Controls.Add(Me.lblRate)
        Me.tabView.Controls.Add(Me.lblAddress2)
        Me.tabView.Controls.Add(Me.lblEmpname)
        Me.tabView.Controls.Add(Me.Label20)
        Me.tabView.Controls.Add(Me.Label32)
        Me.tabView.Controls.Add(Me.Label30)
        Me.tabView.Controls.Add(Me.Label24)
        Me.tabView.Controls.Add(Me.Label28)
        Me.tabView.Controls.Add(Me.lblCostcentre)
        Me.tabView.Controls.Add(Me.lblPurity)
        Me.tabView.Controls.Add(Me.lblAddress1)
        Me.tabView.Controls.Add(Me.lblType)
        Me.tabView.Controls.Add(Me.Label19)
        Me.tabView.Controls.Add(Me.Label18)
        Me.tabView.Controls.Add(Me.Label21)
        Me.tabView.Controls.Add(Me.Label17)
        Me.tabView.Controls.Add(Me.Label15)
        Me.tabView.Controls.Add(Me.cmbOpenType)
        Me.tabView.Controls.Add(Me.btnOSearch_View)
        Me.tabView.Controls.Add(Me.Label23)
        Me.tabView.Controls.Add(Me.Label14)
        Me.tabView.Controls.Add(Me.Label13)
        Me.tabView.Controls.Add(Me.chkOpenDate)
        Me.tabView.Controls.Add(Me.gridOpenView)
        Me.tabView.Location = New System.Drawing.Point(4, 25)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1012, 601)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'dtpOpenTo
        '
        Me.dtpOpenTo.Location = New System.Drawing.Point(238, 13)
        Me.dtpOpenTo.Mask = "##/##/####"
        Me.dtpOpenTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpOpenTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpOpenTo.Name = "dtpOpenTo"
        Me.dtpOpenTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpOpenTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpOpenTo.TabIndex = 3
        Me.dtpOpenTo.Text = "06/03/9998"
        Me.dtpOpenTo.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpOpenFrom
        '
        Me.dtpOpenFrom.Location = New System.Drawing.Point(112, 13)
        Me.dtpOpenFrom.Mask = "##/##/####"
        Me.dtpOpenFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpOpenFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpOpenFrom.Name = "dtpOpenFrom"
        Me.dtpOpenFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpOpenFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpOpenFrom.TabIndex = 1
        Me.dtpOpenFrom.Text = "06/03/9998"
        Me.dtpOpenFrom.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'txtOpenAccName
        '
        Me.txtOpenAccName.Location = New System.Drawing.Point(112, 42)
        Me.txtOpenAccName.Name = "txtOpenAccName"
        Me.txtOpenAccName.Size = New System.Drawing.Size(365, 21)
        Me.txtOpenAccName.TabIndex = 7
        '
        'lblEdit
        '
        Me.lblEdit.AutoSize = True
        Me.lblEdit.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEdit.ForeColor = System.Drawing.Color.Red
        Me.lblEdit.Location = New System.Drawing.Point(46, 583)
        Me.lblEdit.Name = "lblEdit"
        Me.lblEdit.Size = New System.Drawing.Size(142, 16)
        Me.lblEdit.TabIndex = 29
        Me.lblEdit.Text = "Press Enter to Edit"
        Me.lblEdit.Visible = False
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(337, 558)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(60, 13)
        Me.Label16.TabIndex = 16
        Me.Label16.Text = "Address2"
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(118, 558)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(12, 13)
        Me.Label33.TabIndex = 13
        Me.Label33.Text = ":"
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(403, 558)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(12, 13)
        Me.Label31.TabIndex = 17
        Me.Label31.Text = ":"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(796, 558)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(12, 13)
        Me.Label29.TabIndex = 24
        Me.Label29.Text = ":"
        '
        'lblRate
        '
        Me.lblRate.AutoSize = True
        Me.lblRate.Location = New System.Drawing.Point(826, 558)
        Me.lblRate.Name = "lblRate"
        Me.lblRate.Size = New System.Drawing.Size(69, 13)
        Me.lblRate.TabIndex = 27
        Me.lblRate.Text = "Emp Name"
        '
        'lblAddress2
        '
        Me.lblAddress2.AutoSize = True
        Me.lblAddress2.Location = New System.Drawing.Point(424, 558)
        Me.lblAddress2.Name = "lblAddress2"
        Me.lblAddress2.Size = New System.Drawing.Size(69, 13)
        Me.lblAddress2.TabIndex = 18
        Me.lblAddress2.Text = "Emp Name"
        '
        'lblEmpname
        '
        Me.lblEmpname.AutoSize = True
        Me.lblEmpname.Location = New System.Drawing.Point(127, 558)
        Me.lblEmpname.Name = "lblEmpname"
        Me.lblEmpname.Size = New System.Drawing.Size(69, 13)
        Me.lblEmpname.TabIndex = 9
        Me.lblEmpname.Text = "Emp Name"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(46, 558)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(69, 13)
        Me.Label20.TabIndex = 12
        Me.Label20.Text = "Emp Name"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(118, 532)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(12, 13)
        Me.Label32.TabIndex = 11
        Me.Label32.Text = ":"
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(403, 532)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(12, 13)
        Me.Label30.TabIndex = 15
        Me.Label30.Text = ":"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(796, 583)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(12, 13)
        Me.Label24.TabIndex = 25
        Me.Label24.Text = ":"
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(796, 532)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(12, 13)
        Me.Label28.TabIndex = 23
        Me.Label28.Text = ":"
        '
        'lblCostcentre
        '
        Me.lblCostcentre.AutoSize = True
        Me.lblCostcentre.Location = New System.Drawing.Point(826, 583)
        Me.lblCostcentre.Name = "lblCostcentre"
        Me.lblCostcentre.Size = New System.Drawing.Size(35, 13)
        Me.lblCostcentre.TabIndex = 28
        Me.lblCostcentre.Text = "Type"
        '
        'lblPurity
        '
        Me.lblPurity.AutoSize = True
        Me.lblPurity.Location = New System.Drawing.Point(826, 532)
        Me.lblPurity.Name = "lblPurity"
        Me.lblPurity.Size = New System.Drawing.Size(35, 13)
        Me.lblPurity.TabIndex = 26
        Me.lblPurity.Text = "Type"
        '
        'lblAddress1
        '
        Me.lblAddress1.AutoSize = True
        Me.lblAddress1.Location = New System.Drawing.Point(424, 532)
        Me.lblAddress1.Name = "lblAddress1"
        Me.lblAddress1.Size = New System.Drawing.Size(35, 13)
        Me.lblAddress1.TabIndex = 19
        Me.lblAddress1.Text = "Type"
        '
        'lblType
        '
        Me.lblType.AutoSize = True
        Me.lblType.Location = New System.Drawing.Point(127, 532)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(35, 13)
        Me.lblType.TabIndex = 9
        Me.lblType.Text = "Type"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(46, 532)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(35, 13)
        Me.Label19.TabIndex = 10
        Me.Label19.Text = "Type"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(723, 558)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(33, 13)
        Me.Label18.TabIndex = 21
        Me.Label18.Text = "Rate"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(723, 583)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(76, 13)
        Me.Label21.TabIndex = 22
        Me.Label21.Text = "Cost Centre"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(723, 532)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(40, 13)
        Me.Label17.TabIndex = 20
        Me.Label17.Text = "Purity"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(337, 532)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(60, 13)
        Me.Label15.TabIndex = 14
        Me.Label15.Text = "Address1"
        '
        'cmbOpenType
        '
        Me.cmbOpenType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOpenType.FormattingEnabled = True
        Me.cmbOpenType.Location = New System.Drawing.Point(378, 13)
        Me.cmbOpenType.Name = "cmbOpenType"
        Me.cmbOpenType.Size = New System.Drawing.Size(99, 21)
        Me.cmbOpenType.TabIndex = 5
        '
        'btnOSearch_View
        '
        Me.btnOSearch_View.Location = New System.Drawing.Point(493, 36)
        Me.btnOSearch_View.Name = "btnOSearch_View"
        Me.btnOSearch_View.Size = New System.Drawing.Size(100, 30)
        Me.btnOSearch_View.TabIndex = 8
        Me.btnOSearch_View.Text = "&Search"
        Me.btnOSearch_View.UseVisualStyleBackColor = True
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(46, 45)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(64, 13)
        Me.Label23.TabIndex = 6
        Me.Label23.Text = "Acc Name"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(337, 18)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(35, 13)
        Me.Label14.TabIndex = 4
        Me.Label14.Text = "Type"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(211, 18)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(21, 13)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "To"
        '
        'chkOpenDate
        '
        Me.chkOpenDate.AutoSize = True
        Me.chkOpenDate.Checked = True
        Me.chkOpenDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOpenDate.Location = New System.Drawing.Point(20, 17)
        Me.chkOpenDate.Name = "chkOpenDate"
        Me.chkOpenDate.Size = New System.Drawing.Size(86, 17)
        Me.chkOpenDate.TabIndex = 0
        Me.chkOpenDate.Text = "Date From"
        Me.chkOpenDate.UseVisualStyleBackColor = True
        '
        'gridOpenView
        '
        Me.gridOpenView.AllowUserToAddRows = False
        Me.gridOpenView.AllowUserToDeleteRows = False
        Me.gridOpenView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridOpenView.Location = New System.Drawing.Point(6, 82)
        Me.gridOpenView.Name = "gridOpenView"
        Me.gridOpenView.ReadOnly = True
        Me.gridOpenView.Size = New System.Drawing.Size(996, 425)
        Me.gridOpenView.TabIndex = 9
        '
        'frmOpenDebtos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1020, 630)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.Name = "frmOpenDebtos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opening Debitos"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDetails.ResumeLayout(False)
        Me.grpDetails.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.tabView.PerformLayout()
        CType(Me.gridOpenView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtAccCode As System.Windows.Forms.TextBox
    Friend WithEvents cmbCostCentre_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtTranNo_NUM_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbMode As System.Windows.Forms.ComboBox
    Friend WithEvents txtRefNo_NUM_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtRemark As System.Windows.Forms.TextBox
    Friend WithEvents txtEmpId_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents grpDetails As System.Windows.Forms.GroupBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtAccName_MAN As System.Windows.Forms.TextBox
    Friend WithEvents txtAddress1 As System.Windows.Forms.TextBox
    Friend WithEvents txtAddress2 As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents chkOpenDate As System.Windows.Forms.CheckBox
    Friend WithEvents gridOpenView As System.Windows.Forms.DataGridView
    Friend WithEvents btnOSearch_View As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbOpenType As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents lblRate As System.Windows.Forms.Label
    Friend WithEvents lblAddress2 As System.Windows.Forms.Label
    Friend WithEvents lblEmpname As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents lblPurity As System.Windows.Forms.Label
    Friend WithEvents lblAddress1 As System.Windows.Forms.Label
    Friend WithEvents lblType As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents lblCostcentre As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents lblEdit As System.Windows.Forms.Label
    Friend WithEvents txtOpenAccName As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents dtpTrandate As BrighttechPack.DatePicker
    Friend WithEvents dtpOpenTo As BrighttechPack.DatePicker
    Friend WithEvents dtpOpenFrom As BrighttechPack.DatePicker
End Class
