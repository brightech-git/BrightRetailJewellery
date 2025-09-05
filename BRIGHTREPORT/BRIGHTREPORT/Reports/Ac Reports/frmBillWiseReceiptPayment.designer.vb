<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBillWiseReceiptPayment
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
        Me.grpInput = New System.Windows.Forms.GroupBox()
        Me.dtpToDate = New BrighttechPack.DatePicker(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkdatewise = New System.Windows.Forms.CheckBox()
        Me.dtpDate = New BrighttechPack.DatePicker(Me.components)
        Me.lblNodeId = New System.Windows.Forms.Label()
        Me.lblCashCounter = New System.Windows.Forms.Label()
        Me.lblCostCentre = New System.Windows.Forms.Label()
        Me.chkLstNodeId = New System.Windows.Forms.CheckedListBox()
        Me.chkLstCashCounter = New System.Windows.Forms.CheckedListBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.gridHead1 = New System.Windows.Forms.DataGridView()
        Me.pnlHeading = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGen = New System.Windows.Forms.TabPage()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.Panel1.SuspendLayout()
        Me.grpInput.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel5.SuspendLayout()
        CType(Me.gridHead1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHeading.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.grpInput)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1008, 598)
        Me.Panel1.TabIndex = 0
        '
        'grpInput
        '
        Me.grpInput.Controls.Add(Me.dtpToDate)
        Me.grpInput.Controls.Add(Me.Label2)
        Me.grpInput.Controls.Add(Me.chkdatewise)
        Me.grpInput.Controls.Add(Me.dtpDate)
        Me.grpInput.Controls.Add(Me.lblNodeId)
        Me.grpInput.Controls.Add(Me.lblCashCounter)
        Me.grpInput.Controls.Add(Me.lblCostCentre)
        Me.grpInput.Controls.Add(Me.chkLstNodeId)
        Me.grpInput.Controls.Add(Me.chkLstCashCounter)
        Me.grpInput.Controls.Add(Me.chkLstCostCentre)
        Me.grpInput.Controls.Add(Me.Label1)
        Me.grpInput.Controls.Add(Me.btnNew)
        Me.grpInput.Controls.Add(Me.btnExit)
        Me.grpInput.Controls.Add(Me.btnView_Search)
        Me.grpInput.Location = New System.Drawing.Point(267, 41)
        Me.grpInput.Name = "grpInput"
        Me.grpInput.Size = New System.Drawing.Size(431, 384)
        Me.grpInput.TabIndex = 0
        Me.grpInput.TabStop = False
        '
        'dtpToDate
        '
        Me.dtpToDate.Location = New System.Drawing.Point(255, 64)
        Me.dtpToDate.Mask = "##/##/####"
        Me.dtpToDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpToDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpToDate.Size = New System.Drawing.Size(84, 21)
        Me.dtpToDate.TabIndex = 3
        Me.dtpToDate.Text = "07/03/9998"
        Me.dtpToDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(232, 67)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkdatewise
        '
        Me.chkdatewise.AutoSize = True
        Me.chkdatewise.Location = New System.Drawing.Point(31, 270)
        Me.chkdatewise.Name = "chkdatewise"
        Me.chkdatewise.Size = New System.Drawing.Size(151, 17)
        Me.chkdatewise.TabIndex = 10
        Me.chkdatewise.Text = "Transaction Summary"
        Me.chkdatewise.UseVisualStyleBackColor = True
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(143, 64)
        Me.dtpDate.Mask = "##/##/####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDate.Size = New System.Drawing.Size(84, 21)
        Me.dtpDate.TabIndex = 1
        Me.dtpDate.Text = "07/03/9998"
        Me.dtpDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblNodeId
        '
        Me.lblNodeId.AutoSize = True
        Me.lblNodeId.Location = New System.Drawing.Point(287, 113)
        Me.lblNodeId.Name = "lblNodeId"
        Me.lblNodeId.Size = New System.Drawing.Size(48, 13)
        Me.lblNodeId.TabIndex = 8
        Me.lblNodeId.Text = "NodeId"
        '
        'lblCashCounter
        '
        Me.lblCashCounter.AutoSize = True
        Me.lblCashCounter.Location = New System.Drawing.Point(159, 113)
        Me.lblCashCounter.Name = "lblCashCounter"
        Me.lblCashCounter.Size = New System.Drawing.Size(82, 13)
        Me.lblCashCounter.TabIndex = 6
        Me.lblCashCounter.Text = "CashCounter"
        '
        'lblCostCentre
        '
        Me.lblCostCentre.AutoSize = True
        Me.lblCostCentre.Location = New System.Drawing.Point(31, 113)
        Me.lblCostCentre.Name = "lblCostCentre"
        Me.lblCostCentre.Size = New System.Drawing.Size(72, 13)
        Me.lblCostCentre.TabIndex = 4
        Me.lblCostCentre.Text = "CostCentre"
        '
        'chkLstNodeId
        '
        Me.chkLstNodeId.FormattingEnabled = True
        Me.chkLstNodeId.Location = New System.Drawing.Point(287, 129)
        Me.chkLstNodeId.Name = "chkLstNodeId"
        Me.chkLstNodeId.Size = New System.Drawing.Size(116, 132)
        Me.chkLstNodeId.TabIndex = 9
        '
        'chkLstCashCounter
        '
        Me.chkLstCashCounter.FormattingEnabled = True
        Me.chkLstCashCounter.Location = New System.Drawing.Point(159, 129)
        Me.chkLstCashCounter.Name = "chkLstCashCounter"
        Me.chkLstCashCounter.Size = New System.Drawing.Size(116, 132)
        Me.chkLstCashCounter.TabIndex = 7
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(31, 129)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(116, 132)
        Me.chkLstCostCentre.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(106, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(172, 302)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 12
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(278, 302)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 13
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(66, 302)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 11
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(857, 7)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 8
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(757, 7)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 6
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridView)
        Me.Panel2.Controls.Add(Me.Panel5)
        Me.Panel2.Controls.Add(Me.gridHead1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 30)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1008, 571)
        Me.Panel2.TabIndex = 0
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridView.ColumnHeadersHeight = 40
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 23)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1008, 508)
        Me.gridView.TabIndex = 0
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.btnBack)
        Me.Panel5.Controls.Add(Me.btnExport)
        Me.Panel5.Controls.Add(Me.btnPrint)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel5.Location = New System.Drawing.Point(0, 531)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(1008, 40)
        Me.Panel5.TabIndex = 1
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(657, 7)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 9
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'gridHead1
        '
        Me.gridHead1.AllowUserToAddRows = False
        Me.gridHead1.AllowUserToDeleteRows = False
        Me.gridHead1.BackgroundColor = System.Drawing.SystemColors.ActiveBorder
        Me.gridHead1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridHead1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridHead1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridHead1.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridHead1.Enabled = False
        Me.gridHead1.Location = New System.Drawing.Point(0, 0)
        Me.gridHead1.Name = "gridHead1"
        Me.gridHead1.ReadOnly = True
        Me.gridHead1.RowHeadersVisible = False
        Me.gridHead1.Size = New System.Drawing.Size(1008, 23)
        Me.gridHead1.TabIndex = 1
        '
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.Panel4)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(3, 3)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1008, 27)
        Me.pnlHeading.TabIndex = 0
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.lblTitle)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1008, 22)
        Me.Panel4.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1008, 22)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel3
        '
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 630)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1022, 10)
        Me.Panel3.TabIndex = 0
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
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1022, 630)
        Me.tabMain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.Panel1)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1014, 604)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.Panel2)
        Me.tabView.Controls.Add(Me.pnlHeading)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1014, 604)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'frmBillWiseReceiptPayment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.Panel3)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmBillWiseReceiptPayment"
        Me.Text = "Bill Wise Transaction"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.grpInput.ResumeLayout(False)
        Me.grpInput.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel5.ResumeLayout(False)
        CType(Me.gridHead1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHeading.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents gridHead1 As System.Windows.Forms.DataGridView
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents grpInput As System.Windows.Forms.GroupBox
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents lblCostCentre As System.Windows.Forms.Label
    Friend WithEvents chkLstNodeId As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstCashCounter As System.Windows.Forms.CheckedListBox
    Friend WithEvents lblNodeId As System.Windows.Forms.Label
    Friend WithEvents lblCashCounter As System.Windows.Forms.Label
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dtpDate As BrighttechPack.DatePicker
    Friend WithEvents chkdatewise As System.Windows.Forms.CheckBox
    Friend WithEvents dtpToDate As BrighttechPack.DatePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
