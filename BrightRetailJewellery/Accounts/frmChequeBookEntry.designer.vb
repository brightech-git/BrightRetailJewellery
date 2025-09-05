<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChequeBookEntry
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
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpFields = New System.Windows.Forms.GroupBox
        Me.cmbCostCenter_MAN = New System.Windows.Forms.ComboBox
        Me.Label = New System.Windows.Forms.Label
        Me.txtTranLimit_AMT = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.txtchqformat_NUM = New System.Windows.Forms.TextBox
        Me.txtNoOfLeafes_NUM = New System.Windows.Forms.TextBox
        Me.txtCheqNo_NUM = New System.Windows.Forms.TextBox
        Me.cmbBankName_MAN = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.tabView = New System.Windows.Forms.TabPage
        Me.grpFiltration = New System.Windows.Forms.GroupBox
        Me.gridView_OWN = New System.Windows.Forms.DataGridView
        Me.pnlGridViewHeader = New System.Windows.Forms.Panel
        Me.lblTitle = New System.Windows.Forms.Label
        Me.pnlGridFooter = New System.Windows.Forms.Panel
        Me.lblCancel = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnBack = New System.Windows.Forms.Button
        Me.rbtAll = New System.Windows.Forms.RadioButton
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.cmbOpenBankName = New System.Windows.Forms.ComboBox
        Me.dtpOpenFrom = New System.Windows.Forms.DateTimePicker
        Me.Label5 = New System.Windows.Forms.Label
        Me.rbtUsed = New System.Windows.Forms.RadioButton
        Me.dtpOpenTo = New System.Windows.Forms.DateTimePicker
        Me.Label6 = New System.Windows.Forms.Label
        Me.rbtPending = New System.Windows.Forms.RadioButton
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.grpFields.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.grpFiltration.SuspendLayout()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGridViewHeader.SuspendLayout()
        Me.pnlGridFooter.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
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
        Me.tabMain.Size = New System.Drawing.Size(1018, 642)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.ContextMenuStrip = Me.ContextMenuStrip1
        Me.tabGeneral.Controls.Add(Me.grpFields)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 25)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1010, 613)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
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
        'grpFields
        '
        Me.grpFields.Controls.Add(Me.cmbCostCenter_MAN)
        Me.grpFields.Controls.Add(Me.Label)
        Me.grpFields.Controls.Add(Me.txtTranLimit_AMT)
        Me.grpFields.Controls.Add(Me.Label7)
        Me.grpFields.Controls.Add(Me.btnExit)
        Me.grpFields.Controls.Add(Me.btnNew)
        Me.grpFields.Controls.Add(Me.btnOpen)
        Me.grpFields.Controls.Add(Me.btnSave)
        Me.grpFields.Controls.Add(Me.txtchqformat_NUM)
        Me.grpFields.Controls.Add(Me.txtNoOfLeafes_NUM)
        Me.grpFields.Controls.Add(Me.txtCheqNo_NUM)
        Me.grpFields.Controls.Add(Me.cmbBankName_MAN)
        Me.grpFields.Controls.Add(Me.Label3)
        Me.grpFields.Controls.Add(Me.Label8)
        Me.grpFields.Controls.Add(Me.Label2)
        Me.grpFields.Controls.Add(Me.Label1)
        Me.grpFields.Location = New System.Drawing.Point(224, 142)
        Me.grpFields.Name = "grpFields"
        Me.grpFields.Size = New System.Drawing.Size(497, 229)
        Me.grpFields.TabIndex = 0
        Me.grpFields.TabStop = False
        '
        'cmbCostCenter_MAN
        '
        Me.cmbCostCenter_MAN.FormattingEnabled = True
        Me.cmbCostCenter_MAN.Location = New System.Drawing.Point(134, 18)
        Me.cmbCostCenter_MAN.Name = "cmbCostCenter_MAN"
        Me.cmbCostCenter_MAN.Size = New System.Drawing.Size(279, 21)
        Me.cmbCostCenter_MAN.TabIndex = 1
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(28, 26)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 0
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtTranLimit_AMT
        '
        Me.txtTranLimit_AMT.AcceptsTab = True
        Me.txtTranLimit_AMT.Location = New System.Drawing.Point(134, 124)
        Me.txtTranLimit_AMT.MaxLength = 25
        Me.txtTranLimit_AMT.Name = "txtTranLimit_AMT"
        Me.txtTranLimit_AMT.Size = New System.Drawing.Size(213, 21)
        Me.txtTranLimit_AMT.TabIndex = 11
        Me.txtTranLimit_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(28, 127)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(108, 21)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "Transaction Limit"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(349, 168)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 15
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(243, 168)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 14
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(137, 168)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 13
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(31, 168)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 12
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'txtchqformat_NUM
        '
        Me.txtchqformat_NUM.Location = New System.Drawing.Point(305, 98)
        Me.txtchqformat_NUM.MaxLength = 4
        Me.txtchqformat_NUM.Name = "txtchqformat_NUM"
        Me.txtchqformat_NUM.Size = New System.Drawing.Size(44, 21)
        Me.txtchqformat_NUM.TabIndex = 9
        Me.txtchqformat_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNoOfLeafes_NUM
        '
        Me.txtNoOfLeafes_NUM.Location = New System.Drawing.Point(134, 99)
        Me.txtNoOfLeafes_NUM.MaxLength = 4
        Me.txtNoOfLeafes_NUM.Name = "txtNoOfLeafes_NUM"
        Me.txtNoOfLeafes_NUM.Size = New System.Drawing.Size(67, 21)
        Me.txtNoOfLeafes_NUM.TabIndex = 7
        Me.txtNoOfLeafes_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCheqNo_NUM
        '
        Me.txtCheqNo_NUM.Location = New System.Drawing.Point(134, 72)
        Me.txtCheqNo_NUM.MaxLength = 20
        Me.txtCheqNo_NUM.Name = "txtCheqNo_NUM"
        Me.txtCheqNo_NUM.Size = New System.Drawing.Size(216, 21)
        Me.txtCheqNo_NUM.TabIndex = 5
        Me.txtCheqNo_NUM.Text = "12345678901234567890"
        '
        'cmbBankName_MAN
        '
        Me.cmbBankName_MAN.FormattingEnabled = True
        Me.cmbBankName_MAN.Location = New System.Drawing.Point(134, 45)
        Me.cmbBankName_MAN.Name = "cmbBankName_MAN"
        Me.cmbBankName_MAN.Size = New System.Drawing.Size(315, 21)
        Me.cmbBankName_MAN.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(28, 99)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 21)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "No of Leaves"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(204, 99)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(95, 21)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Cheque Format"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(28, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 21)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Cheque No"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(28, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 21)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Bank Acc Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.grpFiltration)
        Me.tabView.Location = New System.Drawing.Point(4, 25)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1010, 613)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'grpFiltration
        '
        Me.grpFiltration.Controls.Add(Me.gridView_OWN)
        Me.grpFiltration.Controls.Add(Me.pnlGridViewHeader)
        Me.grpFiltration.Controls.Add(Me.pnlGridFooter)
        Me.grpFiltration.Controls.Add(Me.Panel1)
        Me.grpFiltration.Location = New System.Drawing.Point(144, 46)
        Me.grpFiltration.Name = "grpFiltration"
        Me.grpFiltration.Size = New System.Drawing.Size(751, 539)
        Me.grpFiltration.TabIndex = 0
        Me.grpFiltration.TabStop = False
        '
        'gridView_OWN
        '
        Me.gridView_OWN.AllowUserToAddRows = False
        Me.gridView_OWN.AllowUserToDeleteRows = False
        Me.gridView_OWN.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView_OWN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView_OWN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView_OWN.Location = New System.Drawing.Point(3, 139)
        Me.gridView_OWN.Name = "gridView_OWN"
        Me.gridView_OWN.ReadOnly = True
        Me.gridView_OWN.RowHeadersVisible = False
        Me.gridView_OWN.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView_OWN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView_OWN.Size = New System.Drawing.Size(745, 367)
        Me.gridView_OWN.TabIndex = 1
        '
        'pnlGridViewHeader
        '
        Me.pnlGridViewHeader.Controls.Add(Me.lblTitle)
        Me.pnlGridViewHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlGridViewHeader.Location = New System.Drawing.Point(3, 109)
        Me.pnlGridViewHeader.Name = "pnlGridViewHeader"
        Me.pnlGridViewHeader.Size = New System.Drawing.Size(745, 30)
        Me.pnlGridViewHeader.TabIndex = 8
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(745, 30)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Label7"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlGridFooter
        '
        Me.pnlGridFooter.Controls.Add(Me.lblCancel)
        Me.pnlGridFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlGridFooter.Location = New System.Drawing.Point(3, 506)
        Me.pnlGridFooter.Name = "pnlGridFooter"
        Me.pnlGridFooter.Size = New System.Drawing.Size(745, 30)
        Me.pnlGridFooter.TabIndex = 7
        '
        'lblCancel
        '
        Me.lblCancel.Dock = System.Windows.Forms.DockStyle.Left
        Me.lblCancel.Location = New System.Drawing.Point(0, 0)
        Me.lblCancel.Name = "lblCancel"
        Me.lblCancel.Size = New System.Drawing.Size(100, 30)
        Me.lblCancel.TabIndex = 0
        Me.lblCancel.Text = "Cancel [C]"
        Me.lblCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnBack)
        Me.Panel1.Controls.Add(Me.rbtAll)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.cmbOpenBankName)
        Me.Panel1.Controls.Add(Me.dtpOpenFrom)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.rbtUsed)
        Me.Panel1.Controls.Add(Me.dtpOpenTo)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.rbtPending)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 17)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(745, 92)
        Me.Panel1.TabIndex = 0
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(427, 61)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 1
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'rbtAll
        '
        Me.rbtAll.AutoSize = True
        Me.rbtAll.Location = New System.Drawing.Point(109, 34)
        Me.rbtAll.Name = "rbtAll"
        Me.rbtAll.Size = New System.Drawing.Size(39, 17)
        Me.rbtAll.TabIndex = 6
        Me.rbtAll.TabStop = True
        Me.rbtAll.Text = "All"
        Me.rbtAll.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(3, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 21)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Date From"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(321, 61)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 11
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(215, 61)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 10
        Me.btnExport.Text = "E&xport"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(109, 61)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 9
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cmbOpenBankName
        '
        Me.cmbOpenBankName.FormattingEnabled = True
        Me.cmbOpenBankName.Location = New System.Drawing.Point(444, 3)
        Me.cmbOpenBankName.Name = "cmbOpenBankName"
        Me.cmbOpenBankName.Size = New System.Drawing.Size(285, 21)
        Me.cmbOpenBankName.TabIndex = 5
        '
        'dtpOpenFrom
        '
        Me.dtpOpenFrom.CustomFormat = "dd/MM/yyyy"
        Me.dtpOpenFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpOpenFrom.Location = New System.Drawing.Point(109, 3)
        Me.dtpOpenFrom.Name = "dtpOpenFrom"
        Me.dtpOpenFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpOpenFrom.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(208, 3)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(25, 21)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "To"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rbtUsed
        '
        Me.rbtUsed.AutoSize = True
        Me.rbtUsed.Location = New System.Drawing.Point(180, 34)
        Me.rbtUsed.Name = "rbtUsed"
        Me.rbtUsed.Size = New System.Drawing.Size(53, 17)
        Me.rbtUsed.TabIndex = 7
        Me.rbtUsed.TabStop = True
        Me.rbtUsed.Text = "Used"
        Me.rbtUsed.UseVisualStyleBackColor = True
        '
        'dtpOpenTo
        '
        Me.dtpOpenTo.CustomFormat = "dd/MM/yyyy"
        Me.dtpOpenTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpOpenTo.Location = New System.Drawing.Point(239, 3)
        Me.dtpOpenTo.Name = "dtpOpenTo"
        Me.dtpOpenTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpOpenTo.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(338, 3)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 21)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Bank Acc Name"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rbtPending
        '
        Me.rbtPending.AutoSize = True
        Me.rbtPending.Location = New System.Drawing.Point(249, 34)
        Me.rbtPending.Name = "rbtPending"
        Me.rbtPending.Size = New System.Drawing.Size(70, 17)
        Me.rbtPending.TabIndex = 8
        Me.rbtPending.TabStop = True
        Me.rbtPending.Text = "Pending"
        Me.rbtPending.UseVisualStyleBackColor = True
        '
        'frmChequeBookEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1018, 642)
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmChequeBookEntry"
        Me.Text = "Cheque Book Entry"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.grpFields.ResumeLayout(False)
        Me.grpFields.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.grpFiltration.ResumeLayout(False)
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGridViewHeader.ResumeLayout(False)
        Me.pnlGridFooter.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpOpenTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpOpenFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents gridView_OWN As System.Windows.Forms.DataGridView
    Friend WithEvents cmbOpenBankName As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents grpFiltration As System.Windows.Forms.GroupBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents rbtPending As System.Windows.Forms.RadioButton
    Friend WithEvents rbtUsed As System.Windows.Forms.RadioButton
    Friend WithEvents pnlGridViewHeader As System.Windows.Forms.Panel
    Friend WithEvents pnlGridFooter As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents lblCancel As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents rbtAll As System.Windows.Forms.RadioButton
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents grpFields As System.Windows.Forms.GroupBox
    Friend WithEvents txtTranLimit_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents txtNoOfLeafes_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtCheqNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents cmbBankName_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtchqformat_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents cmbCostCenter_MAN As System.Windows.Forms.ComboBox
End Class
