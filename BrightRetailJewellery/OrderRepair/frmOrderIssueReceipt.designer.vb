<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOrderIssueReceipt
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmbProcess = New System.Windows.Forms.ComboBox()
        Me.lblProcess = New System.Windows.Forms.Label()
        Me.txtManualNo_NUM = New System.Windows.Forms.TextBox()
        Me.chkManualNo = New System.Windows.Forms.CheckBox()
        Me.btnTransfer = New System.Windows.Forms.Button()
        Me.chkAll = New System.Windows.Forms.CheckBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.rbtReceipt = New System.Windows.Forms.RadioButton()
        Me.rbtIssue = New System.Windows.Forms.RadioButton()
        Me.cmbDesigner_MAN = New System.Windows.Forms.ComboBox()
        Me.rbtRepair = New System.Windows.Forms.RadioButton()
        Me.rbtOrder = New System.Windows.Forms.RadioButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.txtOrderNo = New System.Windows.Forms.TextBox()
        Me.pnlDate = New System.Windows.Forms.Panel()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkOrderDate = New System.Windows.Forms.CheckBox()
        Me.cmbCostCentre_MAN = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.pnlDate.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cmbProcess)
        Me.Panel1.Controls.Add(Me.lblProcess)
        Me.Panel1.Controls.Add(Me.txtManualNo_NUM)
        Me.Panel1.Controls.Add(Me.chkManualNo)
        Me.Panel1.Controls.Add(Me.btnTransfer)
        Me.Panel1.Controls.Add(Me.chkAll)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.cmbDesigner_MAN)
        Me.Panel1.Controls.Add(Me.rbtRepair)
        Me.Panel1.Controls.Add(Me.rbtOrder)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.txtOrderNo)
        Me.Panel1.Controls.Add(Me.pnlDate)
        Me.Panel1.Controls.Add(Me.chkOrderDate)
        Me.Panel1.Controls.Add(Me.cmbCostCentre_MAN)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1011, 167)
        Me.Panel1.TabIndex = 0
        '
        'cmbProcess
        '
        Me.cmbProcess.FormattingEnabled = True
        Me.cmbProcess.Location = New System.Drawing.Point(432, 71)
        Me.cmbProcess.Name = "cmbProcess"
        Me.cmbProcess.Size = New System.Drawing.Size(237, 21)
        Me.cmbProcess.TabIndex = 20
        '
        'lblProcess
        '
        Me.lblProcess.AutoSize = True
        Me.lblProcess.Location = New System.Drawing.Point(347, 75)
        Me.lblProcess.Name = "lblProcess"
        Me.lblProcess.Size = New System.Drawing.Size(82, 13)
        Me.lblProcess.TabIndex = 19
        Me.lblProcess.Text = "Process Type"
        Me.lblProcess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtManualNo_NUM
        '
        Me.txtManualNo_NUM.Location = New System.Drawing.Point(569, 100)
        Me.txtManualNo_NUM.Name = "txtManualNo_NUM"
        Me.txtManualNo_NUM.Size = New System.Drawing.Size(100, 21)
        Me.txtManualNo_NUM.TabIndex = 14
        '
        'chkManualNo
        '
        Me.chkManualNo.AutoSize = True
        Me.chkManualNo.Location = New System.Drawing.Point(432, 102)
        Me.chkManualNo.Name = "chkManualNo"
        Me.chkManualNo.Size = New System.Drawing.Size(131, 17)
        Me.chkManualNo.TabIndex = 13
        Me.chkManualNo.Text = "Manual Receipt No"
        Me.chkManualNo.UseVisualStyleBackColor = True
        '
        'btnTransfer
        '
        Me.btnTransfer.Location = New System.Drawing.Point(217, 127)
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.Size = New System.Drawing.Size(100, 30)
        Me.btnTransfer.TabIndex = 16
        Me.btnTransfer.Text = "&Transfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'chkAll
        '
        Me.chkAll.AutoSize = True
        Me.chkAll.Location = New System.Drawing.Point(347, 102)
        Me.chkAll.Name = "chkAll"
        Me.chkAll.Size = New System.Drawing.Size(79, 17)
        Me.chkAll.TabIndex = 12
        Me.chkAll.Text = "Select All"
        Me.chkAll.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rbtReceipt)
        Me.Panel3.Controls.Add(Me.rbtIssue)
        Me.Panel3.Location = New System.Drawing.Point(111, 69)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(164, 25)
        Me.Panel3.TabIndex = 9
        '
        'rbtReceipt
        '
        Me.rbtReceipt.AutoSize = True
        Me.rbtReceipt.Location = New System.Drawing.Point(84, 4)
        Me.rbtReceipt.Name = "rbtReceipt"
        Me.rbtReceipt.Size = New System.Drawing.Size(67, 17)
        Me.rbtReceipt.TabIndex = 1
        Me.rbtReceipt.TabStop = True
        Me.rbtReceipt.Text = "Receipt"
        Me.rbtReceipt.UseVisualStyleBackColor = True
        '
        'rbtIssue
        '
        Me.rbtIssue.AutoSize = True
        Me.rbtIssue.Location = New System.Drawing.Point(3, 4)
        Me.rbtIssue.Name = "rbtIssue"
        Me.rbtIssue.Size = New System.Drawing.Size(56, 17)
        Me.rbtIssue.TabIndex = 0
        Me.rbtIssue.TabStop = True
        Me.rbtIssue.Text = "Issue"
        Me.rbtIssue.UseVisualStyleBackColor = True
        '
        'cmbDesigner_MAN
        '
        Me.cmbDesigner_MAN.FormattingEnabled = True
        Me.cmbDesigner_MAN.Location = New System.Drawing.Point(111, 100)
        Me.cmbDesigner_MAN.Name = "cmbDesigner_MAN"
        Me.cmbDesigner_MAN.Size = New System.Drawing.Size(227, 21)
        Me.cmbDesigner_MAN.TabIndex = 11
        '
        'rbtRepair
        '
        Me.rbtRepair.AutoSize = True
        Me.rbtRepair.Location = New System.Drawing.Point(432, 14)
        Me.rbtRepair.Name = "rbtRepair"
        Me.rbtRepair.Size = New System.Drawing.Size(62, 17)
        Me.rbtRepair.TabIndex = 3
        Me.rbtRepair.TabStop = True
        Me.rbtRepair.Text = "Repair"
        Me.rbtRepair.UseVisualStyleBackColor = True
        '
        'rbtOrder
        '
        Me.rbtOrder.AutoSize = True
        Me.rbtOrder.Location = New System.Drawing.Point(347, 14)
        Me.rbtOrder.Name = "rbtOrder"
        Me.rbtOrder.Size = New System.Drawing.Size(58, 17)
        Me.rbtOrder.TabIndex = 2
        Me.rbtOrder.TabStop = True
        Me.rbtOrder.Text = "Order"
        Me.rbtOrder.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(14, 75)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Transfer To"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(429, 127)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 18
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(14, 104)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Designer"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(323, 127)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 17
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(111, 127)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 15
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'txtOrderNo
        '
        Me.txtOrderNo.Location = New System.Drawing.Point(432, 41)
        Me.txtOrderNo.Name = "txtOrderNo"
        Me.txtOrderNo.Size = New System.Drawing.Size(97, 21)
        Me.txtOrderNo.TabIndex = 7
        '
        'pnlDate
        '
        Me.pnlDate.Controls.Add(Me.dtpTo)
        Me.pnlDate.Controls.Add(Me.dtpFrom)
        Me.pnlDate.Controls.Add(Me.Label2)
        Me.pnlDate.Location = New System.Drawing.Point(111, 39)
        Me.pnlDate.Name = "pnlDate"
        Me.pnlDate.Size = New System.Drawing.Size(232, 24)
        Me.pnlDate.TabIndex = 5
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(134, 2)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(96, 21)
        Me.dtpTo.TabIndex = 2
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(3, 2)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(96, 21)
        Me.dtpFrom.TabIndex = 0
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(105, 2)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(23, 21)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "To"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkOrderDate
        '
        Me.chkOrderDate.AutoSize = True
        Me.chkOrderDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkOrderDate.Location = New System.Drawing.Point(14, 43)
        Me.chkOrderDate.Name = "chkOrderDate"
        Me.chkOrderDate.Size = New System.Drawing.Size(53, 17)
        Me.chkOrderDate.TabIndex = 4
        Me.chkOrderDate.Text = "Date"
        Me.chkOrderDate.UseVisualStyleBackColor = True
        '
        'cmbCostCentre_MAN
        '
        Me.cmbCostCentre_MAN.FormattingEnabled = True
        Me.cmbCostCentre_MAN.Location = New System.Drawing.Point(111, 12)
        Me.cmbCostCentre_MAN.Name = "cmbCostCentre_MAN"
        Me.cmbCostCentre_MAN.Size = New System.Drawing.Size(227, 21)
        Me.cmbCostCentre_MAN.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(347, 45)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Order No"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Cost Centre"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 167)
        Me.gridView.Name = "gridView"
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.Size = New System.Drawing.Size(1011, 460)
        Me.gridView.TabIndex = 2
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
        'frmOrderIssueReceipt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 627)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximumSize = New System.Drawing.Size(1027, 666)
        Me.Name = "frmOrderIssueReceipt"
        Me.Text = "Issue Receipt Transfer"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.pnlDate.ResumeLayout(False)
        Me.pnlDate.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlDate As System.Windows.Forms.Panel
    Friend WithEvents chkOrderDate As System.Windows.Forms.CheckBox
    Friend WithEvents cmbCostCentre_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtOrderNo As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents rbtRepair As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOrder As System.Windows.Forms.RadioButton
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents rbtReceipt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtIssue As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnTransfer As System.Windows.Forms.Button
    Friend WithEvents chkAll As System.Windows.Forms.CheckBox
    Friend WithEvents cmbDesigner_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents txtManualNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents chkManualNo As System.Windows.Forms.CheckBox
    Friend WithEvents cmbProcess As ComboBox
    Friend WithEvents lblProcess As Label
End Class
