<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOpenBrs
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
        Me.cmbCostCentre_MAN = New System.Windows.Forms.ComboBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.grpDetails = New System.Windows.Forms.GroupBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbMode = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtAmount_AMT = New System.Windows.Forms.TextBox
        Me.lblrealisedate = New System.Windows.Forms.Label
        Me.txtChequeNO_NUM = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtRemark = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtTranNo_NUM = New System.Windows.Forms.TextBox
        Me.cmbBRSFLAG = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.dtpTrandate = New BrighttechPack.DatePicker(Me.components)
        Me.Label3 = New System.Windows.Forms.Label
        Me.dtprealisedate = New BrighttechPack.DatePicker(Me.components)
        Me.dtpChequeDate = New BrighttechPack.DatePicker(Me.components)
        Me.btnOpen = New System.Windows.Forms.Button
        Me.Label11 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.CmbAccName = New System.Windows.Forms.ComboBox
        Me.tabView = New System.Windows.Forms.TabPage
        Me.lblDelete = New System.Windows.Forms.Label
        Me.cmbAcNameForVw = New System.Windows.Forms.ComboBox
        Me.dtpOpenTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpOpenFrom = New BrighttechPack.DatePicker(Me.components)
        Me.lblEdit = New System.Windows.Forms.Label
        Me.btnOSearch_View = New System.Windows.Forms.Button
        Me.Label23 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.chkOpenDate = New System.Windows.Forms.CheckBox
        Me.gridOpenView = New System.Windows.Forms.DataGridView
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDetails.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridOpenView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbCostCentre_MAN
        '
        Me.cmbCostCentre_MAN.FormattingEnabled = True
        Me.cmbCostCentre_MAN.Location = New System.Drawing.Point(118, 47)
        Me.cmbCostCentre_MAN.Name = "cmbCostCentre_MAN"
        Me.cmbCostCentre_MAN.Size = New System.Drawing.Size(391, 21)
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
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(3, 69)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(956, 307)
        Me.gridView.TabIndex = 1
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(267, 396)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 2
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(475, 396)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 4
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(579, 396)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 5
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'grpDetails
        '
        Me.grpDetails.Controls.Add(Me.Panel1)
        Me.grpDetails.Controls.Add(Me.gridView)
        Me.grpDetails.Controls.Add(Me.btnOpen)
        Me.grpDetails.Controls.Add(Me.btnExit)
        Me.grpDetails.Controls.Add(Me.btnNew)
        Me.grpDetails.Controls.Add(Me.btnSave)
        Me.grpDetails.Location = New System.Drawing.Point(23, 146)
        Me.grpDetails.Name = "grpDetails"
        Me.grpDetails.Size = New System.Drawing.Size(971, 434)
        Me.grpDetails.TabIndex = 4
        Me.grpDetails.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.cmbMode)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.txtAmount_AMT)
        Me.Panel1.Controls.Add(Me.lblrealisedate)
        Me.Panel1.Controls.Add(Me.txtChequeNO_NUM)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.txtRemark)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.txtTranNo_NUM)
        Me.Panel1.Controls.Add(Me.cmbBRSFLAG)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.dtpTrandate)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.dtprealisedate)
        Me.Panel1.Controls.Add(Me.dtpChequeDate)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 17)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(965, 50)
        Me.Panel1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(503, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 17)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "ChequeDate"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(300, 3)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(98, 17)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Amount"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(602, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(98, 18)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "BRS Flag"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbMode
        '
        Me.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMode.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMode.FormattingEnabled = True
        Me.cmbMode.Location = New System.Drawing.Point(201, 25)
        Me.cmbMode.Name = "cmbMode"
        Me.cmbMode.Size = New System.Drawing.Size(99, 22)
        Me.cmbMode.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(800, 3)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(156, 17)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Remark"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtAmount_AMT
        '
        Me.txtAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount_AMT.Location = New System.Drawing.Point(301, 25)
        Me.txtAmount_AMT.MaxLength = 9
        Me.txtAmount_AMT.Name = "txtAmount_AMT"
        Me.txtAmount_AMT.Size = New System.Drawing.Size(99, 22)
        Me.txtAmount_AMT.TabIndex = 7
        Me.txtAmount_AMT.Text = "123456789.00"
        '
        'lblrealisedate
        '
        Me.lblrealisedate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblrealisedate.Location = New System.Drawing.Point(700, 2)
        Me.lblrealisedate.Name = "lblrealisedate"
        Me.lblrealisedate.Size = New System.Drawing.Size(100, 18)
        Me.lblrealisedate.TabIndex = 14
        Me.lblrealisedate.Text = "ReliaseDate"
        Me.lblrealisedate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtChequeNO_NUM
        '
        Me.txtChequeNO_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChequeNO_NUM.Location = New System.Drawing.Point(401, 25)
        Me.txtChequeNO_NUM.MaxLength = 10
        Me.txtChequeNO_NUM.Name = "txtChequeNO_NUM"
        Me.txtChequeNO_NUM.Size = New System.Drawing.Size(98, 22)
        Me.txtChequeNO_NUM.TabIndex = 9
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(401, 3)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(97, 17)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "ChequeNO"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtRemark
        '
        Me.txtRemark.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRemark.Location = New System.Drawing.Point(804, 25)
        Me.txtRemark.MaxLength = 50
        Me.txtRemark.Name = "txtRemark"
        Me.txtRemark.Size = New System.Drawing.Size(150, 22)
        Me.txtRemark.TabIndex = 17
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(1, 1)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 22)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Tran No"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTranNo_NUM
        '
        Me.txtTranNo_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTranNo_NUM.Location = New System.Drawing.Point(1, 25)
        Me.txtTranNo_NUM.Name = "txtTranNo_NUM"
        Me.txtTranNo_NUM.Size = New System.Drawing.Size(99, 22)
        Me.txtTranNo_NUM.TabIndex = 1
        '
        'cmbBRSFLAG
        '
        Me.cmbBRSFLAG.FormattingEnabled = True
        Me.cmbBRSFLAG.Location = New System.Drawing.Point(600, 25)
        Me.cmbBRSFLAG.Name = "cmbBRSFLAG"
        Me.cmbBRSFLAG.Size = New System.Drawing.Size(100, 21)
        Me.cmbBRSFLAG.TabIndex = 13
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(200, 4)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(98, 17)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Mode"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtpTrandate
        '
        Me.dtpTrandate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpTrandate.Location = New System.Drawing.Point(101, 25)
        Me.dtpTrandate.Mask = "##/##/####"
        Me.dtpTrandate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTrandate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTrandate.Name = "dtpTrandate"
        Me.dtpTrandate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTrandate.Size = New System.Drawing.Size(99, 22)
        Me.dtpTrandate.TabIndex = 3
        Me.dtpTrandate.Text = "06/03/9998"
        Me.dtpTrandate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(99, 4)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Tran Date"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtprealisedate
        '
        Me.dtprealisedate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtprealisedate.Location = New System.Drawing.Point(701, 25)
        Me.dtprealisedate.Mask = "##/##/####"
        Me.dtprealisedate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtprealisedate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtprealisedate.Name = "dtprealisedate"
        Me.dtprealisedate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtprealisedate.Size = New System.Drawing.Size(102, 22)
        Me.dtprealisedate.TabIndex = 15
        Me.dtprealisedate.Text = "06/03/9998"
        Me.dtprealisedate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpChequeDate
        '
        Me.dtpChequeDate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpChequeDate.Location = New System.Drawing.Point(500, 25)
        Me.dtpChequeDate.Mask = "##/##/####"
        Me.dtpChequeDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpChequeDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpChequeDate.Name = "dtpChequeDate"
        Me.dtpChequeDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpChequeDate.Size = New System.Drawing.Size(99, 22)
        Me.dtpChequeDate.TabIndex = 11
        Me.dtpChequeDate.Text = "06/03/9998"
        Me.dtpChequeDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(371, 396)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 3
        Me.btnOpen.Text = "Open[F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(36, 83)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(64, 13)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Acc Name"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.OpenToolStripMenuItem})
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
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
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
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.Label22)
        Me.tabGeneral.Controls.Add(Me.grpDetails)
        Me.tabGeneral.Controls.Add(Me.CmbAccName)
        Me.tabGeneral.Controls.Add(Me.cmbCostCentre_MAN)
        Me.tabGeneral.Controls.Add(Me.Label11)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 25)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1012, 601)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'CmbAccName
        '
        Me.CmbAccName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CmbAccName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CmbAccName.FormattingEnabled = True
        Me.CmbAccName.Location = New System.Drawing.Point(118, 83)
        Me.CmbAccName.Name = "CmbAccName"
        Me.CmbAccName.Size = New System.Drawing.Size(391, 21)
        Me.CmbAccName.TabIndex = 3
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.lblDelete)
        Me.tabView.Controls.Add(Me.cmbAcNameForVw)
        Me.tabView.Controls.Add(Me.dtpOpenTo)
        Me.tabView.Controls.Add(Me.dtpOpenFrom)
        Me.tabView.Controls.Add(Me.lblEdit)
        Me.tabView.Controls.Add(Me.btnOSearch_View)
        Me.tabView.Controls.Add(Me.Label23)
        Me.tabView.Controls.Add(Me.Label13)
        Me.tabView.Controls.Add(Me.chkOpenDate)
        Me.tabView.Controls.Add(Me.gridOpenView)
        Me.tabView.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabView.Location = New System.Drawing.Point(4, 25)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1012, 601)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'lblDelete
        '
        Me.lblDelete.AutoSize = True
        Me.lblDelete.ForeColor = System.Drawing.Color.Red
        Me.lblDelete.Location = New System.Drawing.Point(839, 548)
        Me.lblDelete.Name = "lblDelete"
        Me.lblDelete.Size = New System.Drawing.Size(162, 14)
        Me.lblDelete.TabIndex = 9
        Me.lblDelete.Text = "Press Del to Delete Rec"
        Me.lblDelete.Visible = False
        '
        'cmbAcNameForVw
        '
        Me.cmbAcNameForVw.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAcNameForVw.FormattingEnabled = True
        Me.cmbAcNameForVw.Location = New System.Drawing.Point(112, 42)
        Me.cmbAcNameForVw.Name = "cmbAcNameForVw"
        Me.cmbAcNameForVw.Size = New System.Drawing.Size(345, 21)
        Me.cmbAcNameForVw.TabIndex = 5
        '
        'dtpOpenTo
        '
        Me.dtpOpenTo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpOpenTo.Location = New System.Drawing.Point(238, 13)
        Me.dtpOpenTo.Mask = "##/##/####"
        Me.dtpOpenTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpOpenTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpOpenTo.Name = "dtpOpenTo"
        Me.dtpOpenTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpOpenTo.Size = New System.Drawing.Size(93, 22)
        Me.dtpOpenTo.TabIndex = 3
        Me.dtpOpenTo.Text = "06/03/9998"
        Me.dtpOpenTo.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpOpenFrom
        '
        Me.dtpOpenFrom.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpOpenFrom.Location = New System.Drawing.Point(112, 13)
        Me.dtpOpenFrom.Mask = "##/##/####"
        Me.dtpOpenFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpOpenFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpOpenFrom.Name = "dtpOpenFrom"
        Me.dtpOpenFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpOpenFrom.Size = New System.Drawing.Size(93, 22)
        Me.dtpOpenFrom.TabIndex = 1
        Me.dtpOpenFrom.Text = "06/03/9998"
        Me.dtpOpenFrom.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'lblEdit
        '
        Me.lblEdit.AutoSize = True
        Me.lblEdit.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEdit.ForeColor = System.Drawing.Color.Red
        Me.lblEdit.Location = New System.Drawing.Point(46, 550)
        Me.lblEdit.Name = "lblEdit"
        Me.lblEdit.Size = New System.Drawing.Size(142, 16)
        Me.lblEdit.TabIndex = 8
        Me.lblEdit.Text = "Press Enter to Edit"
        Me.lblEdit.Visible = False
        '
        'btnOSearch_View
        '
        Me.btnOSearch_View.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOSearch_View.Location = New System.Drawing.Point(493, 36)
        Me.btnOSearch_View.Name = "btnOSearch_View"
        Me.btnOSearch_View.Size = New System.Drawing.Size(100, 30)
        Me.btnOSearch_View.TabIndex = 6
        Me.btnOSearch_View.Text = "&Search"
        Me.btnOSearch_View.UseVisualStyleBackColor = True
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(38, 45)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(67, 14)
        Me.Label23.TabIndex = 4
        Me.Label23.Text = "Acc Name"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(211, 18)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(22, 14)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "To"
        '
        'chkOpenDate
        '
        Me.chkOpenDate.AutoSize = True
        Me.chkOpenDate.Checked = True
        Me.chkOpenDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOpenDate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOpenDate.Location = New System.Drawing.Point(20, 17)
        Me.chkOpenDate.Name = "chkOpenDate"
        Me.chkOpenDate.Size = New System.Drawing.Size(91, 18)
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
        Me.gridOpenView.TabIndex = 7
        '
        'frmOpenBrs
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
        Me.Name = "frmOpenBrs"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opening BRS"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDetails.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.tabView.PerformLayout()
        CType(Me.gridOpenView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmbCostCentre_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents grpDetails As System.Windows.Forms.GroupBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CmbAccName As System.Windows.Forms.ComboBox
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents dtpOpenTo As BrighttechPack.DatePicker
    Friend WithEvents dtpOpenFrom As BrighttechPack.DatePicker
    Friend WithEvents lblEdit As System.Windows.Forms.Label
    Friend WithEvents btnOSearch_View As System.Windows.Forms.Button
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents chkOpenDate As System.Windows.Forms.CheckBox
    Friend WithEvents gridOpenView As System.Windows.Forms.DataGridView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbMode As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents lblrealisedate As System.Windows.Forms.Label
    Friend WithEvents txtChequeNO_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtRemark As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtTranNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents cmbBRSFLAG As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents dtpTrandate As BrighttechPack.DatePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtprealisedate As BrighttechPack.DatePicker
    Friend WithEvents dtpChequeDate As BrighttechPack.DatePicker
    Friend WithEvents lblDelete As System.Windows.Forms.Label
    Friend WithEvents cmbAcNameForVw As System.Windows.Forms.ComboBox
End Class
