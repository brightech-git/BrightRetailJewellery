<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHomeSalesReport
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
        Me.lblFromDate = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.grpControls = New System.Windows.Forms.GroupBox()
        Me.chkWithApproval = New System.Windows.Forms.CheckBox()
        Me.chkcmbNodeId = New BrighttechPack.CheckedComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.chkcmbCounterName = New BrighttechPack.CheckedComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ChkOrderDelivery = New System.Windows.Forms.CheckBox()
        Me.chkcmbitemname = New BrighttechPack.CheckedComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkGroupItem = New System.Windows.Forms.CheckBox()
        Me.rbtCounterSales = New System.Windows.Forms.RadioButton()
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox()
        Me.rbtBackOffice = New System.Windows.Forms.RadioButton()
        Me.rbtBoth = New System.Windows.Forms.RadioButton()
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnUpdateTagNo = New System.Windows.Forms.Button()
        Me.cmbMetalName = New System.Windows.Forms.ComboBox()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.lblMetalName = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.gridFlag = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AutoSizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.grpControls.SuspendLayout()
        CType(Me.gridFlag, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblFromDate
        '
        Me.lblFromDate.AutoSize = True
        Me.lblFromDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFromDate.Location = New System.Drawing.Point(7, 16)
        Me.lblFromDate.Name = "lblFromDate"
        Me.lblFromDate.Size = New System.Drawing.Size(67, 13)
        Me.lblFromDate.TabIndex = 0
        Me.lblFromDate.Text = "Date From"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(175, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'btnView_Search
        '
        Me.btnView_Search.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView_Search.Location = New System.Drawing.Point(305, 96)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 22
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'grpControls
        '
        Me.grpControls.Controls.Add(Me.chkWithApproval)
        Me.grpControls.Controls.Add(Me.chkcmbNodeId)
        Me.grpControls.Controls.Add(Me.Label5)
        Me.grpControls.Controls.Add(Me.chkcmbCounterName)
        Me.grpControls.Controls.Add(Me.Label4)
        Me.grpControls.Controls.Add(Me.ChkOrderDelivery)
        Me.grpControls.Controls.Add(Me.chkcmbitemname)
        Me.grpControls.Controls.Add(Me.Label3)
        Me.grpControls.Controls.Add(Me.chkCmbCostCentre)
        Me.grpControls.Controls.Add(Me.Label1)
        Me.grpControls.Controls.Add(Me.chkGroupItem)
        Me.grpControls.Controls.Add(Me.rbtCounterSales)
        Me.grpControls.Controls.Add(Me.chkCompanySelectAll)
        Me.grpControls.Controls.Add(Me.rbtBackOffice)
        Me.grpControls.Controls.Add(Me.rbtBoth)
        Me.grpControls.Controls.Add(Me.chkLstCompany)
        Me.grpControls.Controls.Add(Me.dtpTo)
        Me.grpControls.Controls.Add(Me.dtpFrom)
        Me.grpControls.Controls.Add(Me.btnUpdateTagNo)
        Me.grpControls.Controls.Add(Me.cmbMetalName)
        Me.grpControls.Controls.Add(Me.lblFromDate)
        Me.grpControls.Controls.Add(Me.btnView_Search)
        Me.grpControls.Controls.Add(Me.Label2)
        Me.grpControls.Controls.Add(Me.btnNew)
        Me.grpControls.Controls.Add(Me.lblMetalName)
        Me.grpControls.Controls.Add(Me.btnExit)
        Me.grpControls.Controls.Add(Me.btnExport)
        Me.grpControls.Controls.Add(Me.btnPrint)
        Me.grpControls.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpControls.Location = New System.Drawing.Point(0, 0)
        Me.grpControls.Name = "grpControls"
        Me.grpControls.Size = New System.Drawing.Size(1253, 133)
        Me.grpControls.TabIndex = 0
        Me.grpControls.TabStop = False
        '
        'chkWithApproval
        '
        Me.chkWithApproval.AutoSize = True
        Me.chkWithApproval.Location = New System.Drawing.Point(868, 14)
        Me.chkWithApproval.Name = "chkWithApproval"
        Me.chkWithApproval.Size = New System.Drawing.Size(106, 17)
        Me.chkWithApproval.TabIndex = 14
        Me.chkWithApproval.Text = "With Approval"
        Me.chkWithApproval.UseVisualStyleBackColor = True
        Me.chkWithApproval.Visible = False
        '
        'chkcmbNodeId
        '
        Me.chkcmbNodeId.CheckOnClick = True
        Me.chkcmbNodeId.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbNodeId.DropDownHeight = 1
        Me.chkcmbNodeId.FormattingEnabled = True
        Me.chkcmbNodeId.IntegralHeight = False
        Me.chkcmbNodeId.Location = New System.Drawing.Point(977, 61)
        Me.chkcmbNodeId.Name = "chkcmbNodeId"
        Me.chkcmbNodeId.Size = New System.Drawing.Size(219, 22)
        Me.chkcmbNodeId.TabIndex = 18
        Me.chkcmbNodeId.ValueSeparator = ", "
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(977, 35)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(219, 21)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "NODE ID"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkcmbCounterName
        '
        Me.chkcmbCounterName.CheckOnClick = True
        Me.chkcmbCounterName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbCounterName.DropDownHeight = 1
        Me.chkcmbCounterName.FormattingEnabled = True
        Me.chkcmbCounterName.IntegralHeight = False
        Me.chkcmbCounterName.Location = New System.Drawing.Point(755, 61)
        Me.chkcmbCounterName.Name = "chkcmbCounterName"
        Me.chkcmbCounterName.Size = New System.Drawing.Size(219, 22)
        Me.chkcmbCounterName.TabIndex = 16
        Me.chkcmbCounterName.ValueSeparator = ", "
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(755, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(219, 21)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Counter Name"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ChkOrderDelivery
        '
        Me.ChkOrderDelivery.AutoSize = True
        Me.ChkOrderDelivery.Location = New System.Drawing.Point(722, 14)
        Me.ChkOrderDelivery.Name = "ChkOrderDelivery"
        Me.ChkOrderDelivery.Size = New System.Drawing.Size(137, 17)
        Me.ChkOrderDelivery.TabIndex = 13
        Me.ChkOrderDelivery.Text = "Sep Order Delivery"
        Me.ChkOrderDelivery.UseVisualStyleBackColor = True
        '
        'chkcmbitemname
        '
        Me.chkcmbitemname.CheckOnClick = True
        Me.chkcmbitemname.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbitemname.DropDownHeight = 1
        Me.chkcmbitemname.FormattingEnabled = True
        Me.chkcmbitemname.IntegralHeight = False
        Me.chkcmbitemname.Location = New System.Drawing.Point(530, 61)
        Me.chkcmbitemname.Name = "chkcmbitemname"
        Me.chkcmbitemname.Size = New System.Drawing.Size(219, 22)
        Me.chkcmbitemname.TabIndex = 11
        Me.chkcmbitemname.ValueSeparator = ", "
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(530, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(219, 21)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Item Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(311, 61)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(219, 22)
        Me.chkCmbCostCentre.TabIndex = 9
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(311, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(219, 21)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Cost Centre"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkGroupItem
        '
        Me.chkGroupItem.AutoSize = True
        Me.chkGroupItem.Location = New System.Drawing.Point(605, 14)
        Me.chkGroupItem.Name = "chkGroupItem"
        Me.chkGroupItem.Size = New System.Drawing.Size(111, 17)
        Me.chkGroupItem.TabIndex = 12
        Me.chkGroupItem.Text = "Group By Item"
        Me.chkGroupItem.UseVisualStyleBackColor = True
        '
        'rbtCounterSales
        '
        Me.rbtCounterSales.AutoSize = True
        Me.rbtCounterSales.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtCounterSales.Location = New System.Drawing.Point(105, 101)
        Me.rbtCounterSales.Name = "rbtCounterSales"
        Me.rbtCounterSales.Size = New System.Drawing.Size(102, 17)
        Me.rbtCounterSales.TabIndex = 20
        Me.rbtCounterSales.TabStop = True
        Me.rbtCounterSales.Text = "CounterSales"
        Me.rbtCounterSales.UseVisualStyleBackColor = True
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(13, 35)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 4
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'rbtBackOffice
        '
        Me.rbtBackOffice.AutoSize = True
        Me.rbtBackOffice.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtBackOffice.Location = New System.Drawing.Point(13, 101)
        Me.rbtBackOffice.Name = "rbtBackOffice"
        Me.rbtBackOffice.Size = New System.Drawing.Size(86, 17)
        Me.rbtBackOffice.TabIndex = 19
        Me.rbtBackOffice.TabStop = True
        Me.rbtBackOffice.Text = "BackOffice"
        Me.rbtBackOffice.UseVisualStyleBackColor = True
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtBoth.Location = New System.Drawing.Point(210, 101)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 21
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(13, 54)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(289, 36)
        Me.chkLstCompany.TabIndex = 5
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(201, 12)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(89, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(80, 12)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(89, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnUpdateTagNo
        '
        Me.btnUpdateTagNo.Location = New System.Drawing.Point(840, 95)
        Me.btnUpdateTagNo.Name = "btnUpdateTagNo"
        Me.btnUpdateTagNo.Size = New System.Drawing.Size(108, 30)
        Me.btnUpdateTagNo.TabIndex = 27
        Me.btnUpdateTagNo.Text = "Update TagNo"
        Me.btnUpdateTagNo.UseVisualStyleBackColor = True
        '
        'cmbMetalName
        '
        Me.cmbMetalName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMetalName.FormattingEnabled = True
        Me.cmbMetalName.Location = New System.Drawing.Point(380, 12)
        Me.cmbMetalName.Name = "cmbMetalName"
        Me.cmbMetalName.Size = New System.Drawing.Size(219, 21)
        Me.cmbMetalName.TabIndex = 7
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.Location = New System.Drawing.Point(412, 96)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 23
        Me.btnNew.Text = "&New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'lblMetalName
        '
        Me.lblMetalName.AutoSize = True
        Me.lblMetalName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMetalName.Location = New System.Drawing.Point(296, 16)
        Me.lblMetalName.Name = "lblMetalName"
        Me.lblMetalName.Size = New System.Drawing.Size(78, 13)
        Me.lblMetalName.TabIndex = 6
        Me.lblMetalName.Text = "Metal Name "
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(733, 96)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 26
        Me.btnExit.Text = "&Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExport.Location = New System.Drawing.Point(519, 96)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 24
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(626, 96)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 25
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'gridFlag
        '
        Me.gridFlag.AllowUserToAddRows = False
        Me.gridFlag.AllowUserToDeleteRows = False
        Me.gridFlag.AllowUserToResizeRows = False
        Me.gridFlag.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridFlag.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridFlag.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridFlag.Location = New System.Drawing.Point(0, 20)
        Me.gridFlag.MultiSelect = False
        Me.gridFlag.Name = "gridFlag"
        Me.gridFlag.ReadOnly = True
        Me.gridFlag.RowHeadersVisible = False
        Me.gridFlag.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridFlag.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridFlag.Size = New System.Drawing.Size(1253, 479)
        Me.gridFlag.StandardTab = True
        Me.gridFlag.TabIndex = 1
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.AutoSizeToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(136, 70)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.NewToolStripMenuItem.Text = "&New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'AutoSizeToolStripMenuItem
        '
        Me.AutoSizeToolStripMenuItem.CheckOnClick = True
        Me.AutoSizeToolStripMenuItem.Name = "AutoSizeToolStripMenuItem"
        Me.AutoSizeToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.AutoSizeToolStripMenuItem.Text = "Auto Resize"
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.gridFlag)
        Me.pnlGrid.Controls.Add(Me.lblTitle)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(0, 133)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1253, 499)
        Me.pnlGrid.TabIndex = 2
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1253, 20)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmHomeSalesReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(1253, 632)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlGrid)
        Me.Controls.Add(Me.grpControls)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmHomeSalesReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "CounterSalesReport"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpControls.ResumeLayout(False)
        Me.grpControls.PerformLayout()
        CType(Me.gridFlag, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlGrid.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblFromDate As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents grpControls As System.Windows.Forms.GroupBox
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents gridFlag As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCounterSales As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBackOffice As System.Windows.Forms.RadioButton
    Friend WithEvents cmbMetalName As System.Windows.Forms.ComboBox
    Friend WithEvents lblMetalName As System.Windows.Forms.Label
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents btnUpdateTagNo As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkGroupItem As System.Windows.Forms.CheckBox
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkcmbitemname As BrighttechPack.CheckedComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ChkOrderDelivery As System.Windows.Forms.CheckBox
    Friend WithEvents AutoSizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkcmbNodeId As BrighttechPack.CheckedComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents chkcmbCounterName As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents chkWithApproval As CheckBox
End Class
