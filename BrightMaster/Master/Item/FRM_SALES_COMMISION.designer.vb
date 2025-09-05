<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_SALES_COMMISION
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.grpContainer = New System.Windows.Forms.GroupBox()
        Me.rbtAge = New System.Windows.Forms.RadioButton()
        Me.dtpToDate_OWN = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFromDate_OWN = New BrighttechPack.DatePicker(Me.components)
        Me.lblToDate = New System.Windows.Forms.Label()
        Me.lblFromdate = New System.Windows.Forms.Label()
        Me.rbtRecdate = New System.Windows.Forms.RadioButton()
        Me.cmbCostcentre = New System.Windows.Forms.ComboBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtCommPerGrm_AMT = New System.Windows.Forms.TextBox()
        Me.txtTagno = New System.Windows.Forms.TextBox()
        Me.lblTagNo = New System.Windows.Forms.Label()
        Me.rbtTag = New System.Windows.Forms.RadioButton()
        Me.rbtPcs = New System.Windows.Forms.RadioButton()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtsubitemid = New System.Windows.Forms.TextBox()
        Me.txtItemCode_NUM = New System.Windows.Forms.TextBox()
        Me.cmbItem_MAN = New System.Windows.Forms.ComboBox()
        Me.chkcmbsubitem = New BrighttechPack.CheckedComboBox()
        Me.cmbCounter_MAN = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.rbtValue = New System.Windows.Forms.RadioButton()
        Me.rbtWeight = New System.Windows.Forms.RadioButton()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.lblFromValue = New System.Windows.Forms.Label()
        Me.txtTo_WET = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtCommPercentage_AMT = New System.Windows.Forms.TextBox()
        Me.txtCommFlat_AMT = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtFrom_WET = New System.Windows.Forms.TextBox()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoResizrToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmbActiveitem = New System.Windows.Forms.ComboBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.CmbBasedOn = New System.Windows.Forms.ComboBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.cmbMetalType = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.btnopenNew = New System.Windows.Forms.Button()
        Me.cmbOpenCounter = New System.Windows.Forms.ComboBox()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.cmbOpenSubItem = New System.Windows.Forms.ComboBox()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.cmbOpenItem = New System.Windows.Forms.ComboBox()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.cmbvcostcenter = New System.Windows.Forms.ComboBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.grpContainer.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.rbtAge)
        Me.grpContainer.Controls.Add(Me.dtpToDate_OWN)
        Me.grpContainer.Controls.Add(Me.dtpFromDate_OWN)
        Me.grpContainer.Controls.Add(Me.lblToDate)
        Me.grpContainer.Controls.Add(Me.lblFromdate)
        Me.grpContainer.Controls.Add(Me.rbtRecdate)
        Me.grpContainer.Controls.Add(Me.cmbCostcentre)
        Me.grpContainer.Controls.Add(Me.Label17)
        Me.grpContainer.Controls.Add(Me.txtCommPerGrm_AMT)
        Me.grpContainer.Controls.Add(Me.txtTagno)
        Me.grpContainer.Controls.Add(Me.lblTagNo)
        Me.grpContainer.Controls.Add(Me.rbtTag)
        Me.grpContainer.Controls.Add(Me.rbtPcs)
        Me.grpContainer.Controls.Add(Me.Label13)
        Me.grpContainer.Controls.Add(Me.Label12)
        Me.grpContainer.Controls.Add(Me.txtsubitemid)
        Me.grpContainer.Controls.Add(Me.txtItemCode_NUM)
        Me.grpContainer.Controls.Add(Me.cmbItem_MAN)
        Me.grpContainer.Controls.Add(Me.chkcmbsubitem)
        Me.grpContainer.Controls.Add(Me.cmbCounter_MAN)
        Me.grpContainer.Controls.Add(Me.Label3)
        Me.grpContainer.Controls.Add(Me.rbtValue)
        Me.grpContainer.Controls.Add(Me.rbtWeight)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.btnOpen)
        Me.grpContainer.Controls.Add(Me.Label6)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.btnSave)
        Me.grpContainer.Controls.Add(Me.lblFromValue)
        Me.grpContainer.Controls.Add(Me.txtTo_WET)
        Me.grpContainer.Controls.Add(Me.Label10)
        Me.grpContainer.Controls.Add(Me.Label9)
        Me.grpContainer.Controls.Add(Me.Label5)
        Me.grpContainer.Controls.Add(Me.txtCommPercentage_AMT)
        Me.grpContainer.Controls.Add(Me.txtCommFlat_AMT)
        Me.grpContainer.Controls.Add(Me.Label4)
        Me.grpContainer.Controls.Add(Me.txtFrom_WET)
        Me.grpContainer.Location = New System.Drawing.Point(187, 14)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(547, 455)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'rbtAge
        '
        Me.rbtAge.AutoSize = True
        Me.rbtAge.Location = New System.Drawing.Point(462, 54)
        Me.rbtAge.Name = "rbtAge"
        Me.rbtAge.Size = New System.Drawing.Size(47, 17)
        Me.rbtAge.TabIndex = 6
        Me.rbtAge.TabStop = True
        Me.rbtAge.Text = "Age"
        Me.rbtAge.UseVisualStyleBackColor = True
        '
        'dtpToDate_OWN
        '
        Me.dtpToDate_OWN.Location = New System.Drawing.Point(263, 225)
        Me.dtpToDate_OWN.Mask = "##-##-####"
        Me.dtpToDate_OWN.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpToDate_OWN.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpToDate_OWN.Name = "dtpToDate_OWN"
        Me.dtpToDate_OWN.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpToDate_OWN.Size = New System.Drawing.Size(85, 21)
        Me.dtpToDate_OWN.TabIndex = 28
        Me.dtpToDate_OWN.Text = "27-10-9998"
        Me.dtpToDate_OWN.Value = New Date(9998, 10, 27, 0, 0, 0, 0)
        '
        'dtpFromDate_OWN
        '
        Me.dtpFromDate_OWN.Location = New System.Drawing.Point(145, 225)
        Me.dtpFromDate_OWN.Mask = "##-##-####"
        Me.dtpFromDate_OWN.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFromDate_OWN.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFromDate_OWN.Name = "dtpFromDate_OWN"
        Me.dtpFromDate_OWN.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpFromDate_OWN.Size = New System.Drawing.Size(85, 21)
        Me.dtpFromDate_OWN.TabIndex = 26
        Me.dtpFromDate_OWN.Text = "27-10-9998"
        Me.dtpFromDate_OWN.Value = New Date(9998, 10, 27, 0, 0, 0, 0)
        '
        'lblToDate
        '
        Me.lblToDate.AutoSize = True
        Me.lblToDate.Location = New System.Drawing.Point(236, 229)
        Me.lblToDate.Name = "lblToDate"
        Me.lblToDate.Size = New System.Drawing.Size(20, 13)
        Me.lblToDate.TabIndex = 27
        Me.lblToDate.Text = "To"
        '
        'lblFromdate
        '
        Me.lblFromdate.AutoSize = True
        Me.lblFromdate.Location = New System.Drawing.Point(30, 229)
        Me.lblFromdate.Name = "lblFromdate"
        Me.lblFromdate.Size = New System.Drawing.Size(67, 13)
        Me.lblFromdate.TabIndex = 25
        Me.lblFromdate.Text = "Date From"
        '
        'rbtRecdate
        '
        Me.rbtRecdate.AutoSize = True
        Me.rbtRecdate.Location = New System.Drawing.Point(379, 55)
        Me.rbtRecdate.Name = "rbtRecdate"
        Me.rbtRecdate.Size = New System.Drawing.Size(77, 17)
        Me.rbtRecdate.TabIndex = 5
        Me.rbtRecdate.TabStop = True
        Me.rbtRecdate.Text = "Rec Date"
        Me.rbtRecdate.UseVisualStyleBackColor = True
        '
        'cmbCostcentre
        '
        Me.cmbCostcentre.FormattingEnabled = True
        Me.cmbCostcentre.Location = New System.Drawing.Point(145, 78)
        Me.cmbCostcentre.Name = "cmbCostcentre"
        Me.cmbCostcentre.Size = New System.Drawing.Size(273, 21)
        Me.cmbCostcentre.TabIndex = 8
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(30, 82)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(76, 13)
        Me.Label17.TabIndex = 7
        Me.Label17.Text = "Cost Center"
        '
        'txtCommPerGrm_AMT
        '
        Me.txtCommPerGrm_AMT.Location = New System.Drawing.Point(145, 312)
        Me.txtCommPerGrm_AMT.Name = "txtCommPerGrm_AMT"
        Me.txtCommPerGrm_AMT.Size = New System.Drawing.Size(85, 21)
        Me.txtCommPerGrm_AMT.TabIndex = 36
        '
        'txtTagno
        '
        Me.txtTagno.Location = New System.Drawing.Point(145, 194)
        Me.txtTagno.Name = "txtTagno"
        Me.txtTagno.Size = New System.Drawing.Size(85, 21)
        Me.txtTagno.TabIndex = 20
        '
        'lblTagNo
        '
        Me.lblTagNo.AutoSize = True
        Me.lblTagNo.Location = New System.Drawing.Point(30, 198)
        Me.lblTagNo.Name = "lblTagNo"
        Me.lblTagNo.Size = New System.Drawing.Size(42, 13)
        Me.lblTagNo.TabIndex = 19
        Me.lblTagNo.Text = "TagNo"
        '
        'rbtTag
        '
        Me.rbtTag.AutoSize = True
        Me.rbtTag.Location = New System.Drawing.Point(328, 55)
        Me.rbtTag.Name = "rbtTag"
        Me.rbtTag.Size = New System.Drawing.Size(45, 17)
        Me.rbtTag.TabIndex = 4
        Me.rbtTag.TabStop = True
        Me.rbtTag.Text = "Tag"
        Me.rbtTag.UseVisualStyleBackColor = True
        '
        'rbtPcs
        '
        Me.rbtPcs.AutoSize = True
        Me.rbtPcs.Location = New System.Drawing.Point(278, 55)
        Me.rbtPcs.Name = "rbtPcs"
        Me.rbtPcs.Size = New System.Drawing.Size(44, 17)
        Me.rbtPcs.TabIndex = 3
        Me.rbtPcs.Text = "Pcs"
        Me.rbtPcs.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.Color.Red
        Me.Label13.Location = New System.Drawing.Point(423, 169)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(94, 13)
        Me.Label13.TabIndex = 18
        Me.Label13.Text = "Sub Item Mode"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.Color.Red
        Me.Label12.Location = New System.Drawing.Point(425, 140)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(68, 13)
        Me.Label12.TabIndex = 14
        Me.Label12.Text = "Item Mode"
        '
        'txtsubitemid
        '
        Me.txtsubitemid.BackColor = System.Drawing.SystemColors.Window
        Me.txtsubitemid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtsubitemid.Location = New System.Drawing.Point(145, 165)
        Me.txtsubitemid.MaxLength = 8
        Me.txtsubitemid.Name = "txtsubitemid"
        Me.txtsubitemid.Size = New System.Drawing.Size(64, 21)
        Me.txtsubitemid.TabIndex = 16
        Me.txtsubitemid.Text = "SUBITEMID"
        '
        'txtItemCode_NUM
        '
        Me.txtItemCode_NUM.BackColor = System.Drawing.SystemColors.Window
        Me.txtItemCode_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemCode_NUM.Location = New System.Drawing.Point(145, 136)
        Me.txtItemCode_NUM.MaxLength = 8
        Me.txtItemCode_NUM.Name = "txtItemCode_NUM"
        Me.txtItemCode_NUM.Size = New System.Drawing.Size(64, 21)
        Me.txtItemCode_NUM.TabIndex = 12
        Me.txtItemCode_NUM.Text = "ITEMID"
        '
        'cmbItem_MAN
        '
        Me.cmbItem_MAN.FormattingEnabled = True
        Me.cmbItem_MAN.Location = New System.Drawing.Point(215, 136)
        Me.cmbItem_MAN.Name = "cmbItem_MAN"
        Me.cmbItem_MAN.Size = New System.Drawing.Size(203, 21)
        Me.cmbItem_MAN.TabIndex = 13
        '
        'chkcmbsubitem
        '
        Me.chkcmbsubitem.CheckOnClick = True
        Me.chkcmbsubitem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbsubitem.DropDownHeight = 1
        Me.chkcmbsubitem.FormattingEnabled = True
        Me.chkcmbsubitem.IntegralHeight = False
        Me.chkcmbsubitem.Location = New System.Drawing.Point(215, 164)
        Me.chkcmbsubitem.Name = "chkcmbsubitem"
        Me.chkcmbsubitem.Size = New System.Drawing.Size(203, 22)
        Me.chkcmbsubitem.TabIndex = 17
        Me.chkcmbsubitem.ValueSeparator = ", "
        '
        'cmbCounter_MAN
        '
        Me.cmbCounter_MAN.FormattingEnabled = True
        Me.cmbCounter_MAN.Location = New System.Drawing.Point(145, 107)
        Me.cmbCounter_MAN.Name = "cmbCounter_MAN"
        Me.cmbCounter_MAN.Size = New System.Drawing.Size(273, 21)
        Me.cmbCounter_MAN.TabIndex = 10
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(30, 111)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Counter"
        '
        'rbtValue
        '
        Me.rbtValue.AutoSize = True
        Me.rbtValue.Location = New System.Drawing.Point(215, 55)
        Me.rbtValue.Name = "rbtValue"
        Me.rbtValue.Size = New System.Drawing.Size(56, 17)
        Me.rbtValue.TabIndex = 2
        Me.rbtValue.Text = "Value"
        Me.rbtValue.UseVisualStyleBackColor = True
        '
        'rbtWeight
        '
        Me.rbtWeight.AutoSize = True
        Me.rbtWeight.Checked = True
        Me.rbtWeight.Location = New System.Drawing.Point(145, 55)
        Me.rbtWeight.Name = "rbtWeight"
        Me.rbtWeight.Size = New System.Drawing.Size(63, 17)
        Me.rbtWeight.TabIndex = 1
        Me.rbtWeight.TabStop = True
        Me.rbtWeight.Text = "Weight"
        Me.rbtWeight.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(357, 370)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 42
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(249, 370)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 41
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(30, 140)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Item Code"
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(141, 370)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 40
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(30, 57)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(104, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Comm Based On"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(30, 169)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Sub Item"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(33, 370)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 39
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'lblFromValue
        '
        Me.lblFromValue.AutoSize = True
        Me.lblFromValue.Location = New System.Drawing.Point(30, 258)
        Me.lblFromValue.Name = "lblFromValue"
        Me.lblFromValue.Size = New System.Drawing.Size(36, 13)
        Me.lblFromValue.TabIndex = 29
        Me.lblFromValue.Text = "From"
        '
        'txtTo_WET
        '
        Me.txtTo_WET.Location = New System.Drawing.Point(263, 254)
        Me.txtTo_WET.Name = "txtTo_WET"
        Me.txtTo_WET.Size = New System.Drawing.Size(85, 21)
        Me.txtTo_WET.TabIndex = 32
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(30, 345)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(61, 13)
        Me.Label10.TabIndex = 37
        Me.Label10.Text = "Comm %"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(30, 316)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(97, 13)
        Me.Label9.TabIndex = 35
        Me.Label9.Text = "Comm Per Grm"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(30, 287)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(69, 13)
        Me.Label5.TabIndex = 33
        Me.Label5.Text = "Comm Flat"
        '
        'txtCommPercentage_AMT
        '
        Me.txtCommPercentage_AMT.Location = New System.Drawing.Point(145, 341)
        Me.txtCommPercentage_AMT.Name = "txtCommPercentage_AMT"
        Me.txtCommPercentage_AMT.Size = New System.Drawing.Size(85, 21)
        Me.txtCommPercentage_AMT.TabIndex = 38
        '
        'txtCommFlat_AMT
        '
        Me.txtCommFlat_AMT.Location = New System.Drawing.Point(145, 283)
        Me.txtCommFlat_AMT.Name = "txtCommFlat_AMT"
        Me.txtCommFlat_AMT.Size = New System.Drawing.Size(85, 21)
        Me.txtCommFlat_AMT.TabIndex = 34
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(236, 258)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(20, 13)
        Me.Label4.TabIndex = 31
        Me.Label4.Text = "To"
        '
        'txtFrom_WET
        '
        Me.txtFrom_WET.Location = New System.Drawing.Point(145, 254)
        Me.txtFrom_WET.Name = "txtFrom_WET"
        Me.txtFrom_WET.Size = New System.Drawing.Size(85, 21)
        Me.txtFrom_WET.TabIndex = 30
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(928, 509)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grpContainer)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(920, 483)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.ContextMenuStrip = Me.ContextMenuStrip2
        Me.tabView.Controls.Add(Me.GroupBox2)
        Me.tabView.Controls.Add(Me.Panel1)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(920, 483)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(122, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.ExitToolStripMenuItem.Text = "Exit "
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.gridView)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(3, 115)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(914, 365)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.ContextMenuStrip1
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(3, 17)
        Me.gridView.Name = "gridView"
        Me.gridView.RowHeadersVisible = False
        Me.gridView.Size = New System.Drawing.Size(908, 345)
        Me.gridView.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoResizrToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(136, 26)
        '
        'AutoResizrToolStripMenuItem
        '
        Me.AutoResizrToolStripMenuItem.Name = "AutoResizrToolStripMenuItem"
        Me.AutoResizrToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.AutoResizrToolStripMenuItem.Text = "Auto Resize"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(914, 112)
        Me.Panel1.TabIndex = 9
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbvcostcenter)
        Me.GroupBox1.Controls.Add(Me.Label18)
        Me.GroupBox1.Controls.Add(Me.cmbActiveitem)
        Me.GroupBox1.Controls.Add(Me.Label16)
        Me.GroupBox1.Controls.Add(Me.CmbBasedOn)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.cmbMetalType)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.btnopenNew)
        Me.GroupBox1.Controls.Add(Me.cmbOpenCounter)
        Me.GroupBox1.Controls.Add(Me.btnView_Search)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.cmbOpenSubItem)
        Me.GroupBox1.Controls.Add(Me.btnEdit)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.btnBack)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.btnExport)
        Me.GroupBox1.Controls.Add(Me.cmbOpenItem)
        Me.GroupBox1.Controls.Add(Me.btnDelete)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(914, 112)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'cmbActiveitem
        '
        Me.cmbActiveitem.FormattingEnabled = True
        Me.cmbActiveitem.Items.AddRange(New Object() {"ALL", "YES", "NO"})
        Me.cmbActiveitem.Location = New System.Drawing.Point(781, 39)
        Me.cmbActiveitem.Name = "cmbActiveitem"
        Me.cmbActiveitem.Size = New System.Drawing.Size(131, 21)
        Me.cmbActiveitem.TabIndex = 13
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(676, 42)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(73, 13)
        Me.Label16.TabIndex = 12
        Me.Label16.Text = "Active Item"
        '
        'CmbBasedOn
        '
        Me.CmbBasedOn.FormattingEnabled = True
        Me.CmbBasedOn.Location = New System.Drawing.Point(781, 11)
        Me.CmbBasedOn.Name = "CmbBasedOn"
        Me.CmbBasedOn.Size = New System.Drawing.Size(131, 21)
        Me.CmbBasedOn.TabIndex = 11
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(676, 14)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(104, 13)
        Me.Label15.TabIndex = 10
        Me.Label15.Text = "Comm Based On"
        '
        'cmbMetalType
        '
        Me.cmbMetalType.FormattingEnabled = True
        Me.cmbMetalType.Location = New System.Drawing.Point(83, 11)
        Me.cmbMetalType.Name = "cmbMetalType"
        Me.cmbMetalType.Size = New System.Drawing.Size(226, 21)
        Me.cmbMetalType.TabIndex = 1
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(11, 15)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(41, 13)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Metal "
        '
        'btnopenNew
        '
        Me.btnopenNew.Location = New System.Drawing.Point(413, 69)
        Me.btnopenNew.Name = "btnopenNew"
        Me.btnopenNew.Size = New System.Drawing.Size(100, 30)
        Me.btnopenNew.TabIndex = 15
        Me.btnopenNew.Text = "&New[F3]"
        Me.btnopenNew.UseVisualStyleBackColor = True
        '
        'cmbOpenCounter
        '
        Me.cmbOpenCounter.FormattingEnabled = True
        Me.cmbOpenCounter.Location = New System.Drawing.Point(402, 39)
        Me.cmbOpenCounter.Name = "cmbOpenCounter"
        Me.cmbOpenCounter.Size = New System.Drawing.Size(226, 21)
        Me.cmbOpenCounter.TabIndex = 9
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(313, 69)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 14
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(334, 43)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(53, 13)
        Me.Label11.TabIndex = 8
        Me.Label11.Text = "Counter"
        '
        'cmbOpenSubItem
        '
        Me.cmbOpenSubItem.FormattingEnabled = True
        Me.cmbOpenSubItem.Location = New System.Drawing.Point(402, 11)
        Me.cmbOpenSubItem.Name = "cmbOpenSubItem"
        Me.cmbOpenSubItem.Size = New System.Drawing.Size(226, 21)
        Me.cmbOpenSubItem.TabIndex = 7
        '
        'btnEdit
        '
        Me.btnEdit.Location = New System.Drawing.Point(613, 69)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(100, 30)
        Me.btnEdit.TabIndex = 17
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(11, 43)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(34, 13)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "Item"
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(513, 69)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 16
        Me.btnBack.Text = "&Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(334, 15)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(60, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Sub Item"
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(813, 69)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 19
        Me.btnExport.Text = "&Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'cmbOpenItem
        '
        Me.cmbOpenItem.FormattingEnabled = True
        Me.cmbOpenItem.Location = New System.Drawing.Point(83, 38)
        Me.cmbOpenItem.Name = "cmbOpenItem"
        Me.cmbOpenItem.Size = New System.Drawing.Size(226, 21)
        Me.cmbOpenItem.TabIndex = 3
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(713, 69)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 18
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'cmbvcostcenter
        '
        Me.cmbvcostcenter.FormattingEnabled = True
        Me.cmbvcostcenter.Location = New System.Drawing.Point(83, 65)
        Me.cmbvcostcenter.Name = "cmbvcostcenter"
        Me.cmbvcostcenter.Size = New System.Drawing.Size(226, 21)
        Me.cmbvcostcenter.TabIndex = 5
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(6, 70)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(76, 13)
        Me.Label18.TabIndex = 4
        Me.Label18.Text = "Cost Center"
        '
        'FRM_SALES_COMMISION
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(928, 509)
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "FRM_SALES_COMMISION"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sales Commision"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents txtTo_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtFrom_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblFromValue As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCommFlat_AMT As System.Windows.Forms.TextBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents rbtValue As System.Windows.Forms.RadioButton
    Friend WithEvents rbtWeight As System.Windows.Forms.RadioButton
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbOpenItem As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents cmbOpenSubItem As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbCounter_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtCommPercentage_AMT As System.Windows.Forms.TextBox
    Friend WithEvents cmbOpenCounter As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents chkcmbsubitem As BrighttechPack.CheckedComboBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents txtItemCode_NUM As System.Windows.Forms.TextBox
    Friend WithEvents cmbItem_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents txtsubitemid As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents btnopenNew As System.Windows.Forms.Button
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents cmbMetalType As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents CmbBasedOn As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents cmbActiveitem As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoResizrToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rbtPcs As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTag As RadioButton
    Friend WithEvents txtTagno As TextBox
    Friend WithEvents lblTagNo As Label
    Friend WithEvents txtCommPerGrm_AMT As TextBox
    Friend WithEvents cmbCostcentre As ComboBox
    Friend WithEvents Label17 As Label
    Friend WithEvents rbtRecdate As RadioButton
    Friend WithEvents lblFromdate As Label
    Friend WithEvents lblToDate As Label
    Friend WithEvents dtpToDate_OWN As BrighttechPack.DatePicker
    Friend WithEvents dtpFromDate_OWN As BrighttechPack.DatePicker
    Friend WithEvents rbtAge As RadioButton
    Friend WithEvents cmbvcostcenter As ComboBox
    Friend WithEvents Label18 As Label
End Class
