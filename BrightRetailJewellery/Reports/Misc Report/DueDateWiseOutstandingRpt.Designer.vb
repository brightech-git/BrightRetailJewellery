<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DueDateWiseOutstandingRpt
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GrpContainer = New System.Windows.Forms.GroupBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.rbtduedate = New System.Windows.Forms.RadioButton
        Me.rbtArea = New System.Windows.Forms.RadioButton
        Me.rbtOrderName = New System.Windows.Forms.RadioButton
        Me.rbtOrderRunNo = New System.Windows.Forms.RadioButton
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbtOther = New System.Windows.Forms.RadioButton
        Me.rbtDealerSmith = New System.Windows.Forms.RadioButton
        Me.rbtCustomer = New System.Windows.Forms.RadioButton
        Me.chkWithReceiptDetail = New System.Windows.Forms.CheckBox
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.chkAsOnDate = New System.Windows.Forms.CheckBox
        Me.dtpTo = New GiritechPack.DatePicker(Me.components)
        Me.dtpFrom = New GiritechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.ContextMenuStrip1.SuspendLayout()
        Me.GrpContainer.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'GrpContainer
        '
        Me.GrpContainer.ContextMenuStrip = Me.ContextMenuStrip1
        Me.GrpContainer.Controls.Add(Me.GroupBox2)
        Me.GrpContainer.Controls.Add(Me.GroupBox1)
        Me.GrpContainer.Controls.Add(Me.chkWithReceiptDetail)
        Me.GrpContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.GrpContainer.Controls.Add(Me.chkLstCostCentre)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.chkAsOnDate)
        Me.GrpContainer.Controls.Add(Me.dtpTo)
        Me.GrpContainer.Controls.Add(Me.dtpFrom)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Location = New System.Drawing.Point(218, 88)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(348, 351)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbtduedate)
        Me.GroupBox2.Controls.Add(Me.rbtArea)
        Me.GroupBox2.Controls.Add(Me.rbtOrderName)
        Me.GroupBox2.Controls.Add(Me.rbtOrderRunNo)
        Me.GroupBox2.Location = New System.Drawing.Point(27, 222)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(300, 39)
        Me.GroupBox2.TabIndex = 8
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = " "
        '
        'rbtduedate
        '
        Me.rbtduedate.AutoSize = True
        Me.rbtduedate.Location = New System.Drawing.Point(194, 14)
        Me.rbtduedate.Name = "rbtduedate"
        Me.rbtduedate.Size = New System.Drawing.Size(79, 17)
        Me.rbtduedate.TabIndex = 3
        Me.rbtduedate.Text = "Due Date"
        Me.rbtduedate.UseVisualStyleBackColor = True
        '
        'rbtArea
        '
        Me.rbtArea.AutoSize = True
        Me.rbtArea.Location = New System.Drawing.Point(133, 14)
        Me.rbtArea.Name = "rbtArea"
        Me.rbtArea.Size = New System.Drawing.Size(52, 17)
        Me.rbtArea.TabIndex = 2
        Me.rbtArea.Text = "Area"
        Me.rbtArea.UseVisualStyleBackColor = True
        '
        'rbtOrderName
        '
        Me.rbtOrderName.AutoSize = True
        Me.rbtOrderName.Checked = True
        Me.rbtOrderName.Location = New System.Drawing.Point(7, 14)
        Me.rbtOrderName.Name = "rbtOrderName"
        Me.rbtOrderName.Size = New System.Drawing.Size(58, 17)
        Me.rbtOrderName.TabIndex = 0
        Me.rbtOrderName.TabStop = True
        Me.rbtOrderName.Text = "Name"
        Me.rbtOrderName.UseVisualStyleBackColor = True
        '
        'rbtOrderRunNo
        '
        Me.rbtOrderRunNo.AutoSize = True
        Me.rbtOrderRunNo.Location = New System.Drawing.Point(65, 15)
        Me.rbtOrderRunNo.Name = "rbtOrderRunNo"
        Me.rbtOrderRunNo.Size = New System.Drawing.Size(62, 17)
        Me.rbtOrderRunNo.TabIndex = 1
        Me.rbtOrderRunNo.Text = "RunNo"
        Me.rbtOrderRunNo.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtOther)
        Me.GroupBox1.Controls.Add(Me.rbtDealerSmith)
        Me.GroupBox1.Controls.Add(Me.rbtCustomer)
        Me.GroupBox1.Location = New System.Drawing.Point(28, 175)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(300, 36)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = " "
        '
        'rbtOther
        '
        Me.rbtOther.AutoSize = True
        Me.rbtOther.Location = New System.Drawing.Point(207, 11)
        Me.rbtOther.Name = "rbtOther"
        Me.rbtOther.Size = New System.Drawing.Size(63, 17)
        Me.rbtOther.TabIndex = 2
        Me.rbtOther.Text = "Others"
        Me.rbtOther.UseVisualStyleBackColor = True
        '
        'rbtDealerSmith
        '
        Me.rbtDealerSmith.AutoSize = True
        Me.rbtDealerSmith.Location = New System.Drawing.Point(94, 10)
        Me.rbtDealerSmith.Name = "rbtDealerSmith"
        Me.rbtDealerSmith.Size = New System.Drawing.Size(112, 17)
        Me.rbtDealerSmith.TabIndex = 1
        Me.rbtDealerSmith.Text = "Dealer && Smith"
        Me.rbtDealerSmith.UseVisualStyleBackColor = True
        '
        'rbtCustomer
        '
        Me.rbtCustomer.AutoSize = True
        Me.rbtCustomer.Checked = True
        Me.rbtCustomer.Location = New System.Drawing.Point(6, 9)
        Me.rbtCustomer.Name = "rbtCustomer"
        Me.rbtCustomer.Size = New System.Drawing.Size(81, 17)
        Me.rbtCustomer.TabIndex = 0
        Me.rbtCustomer.TabStop = True
        Me.rbtCustomer.Text = "Customer"
        Me.rbtCustomer.UseVisualStyleBackColor = True
        '
        'chkWithReceiptDetail
        '
        Me.chkWithReceiptDetail.AutoSize = True
        Me.chkWithReceiptDetail.Location = New System.Drawing.Point(126, 157)
        Me.chkWithReceiptDetail.Name = "chkWithReceiptDetail"
        Me.chkWithReceiptDetail.Size = New System.Drawing.Size(140, 17)
        Me.chkWithReceiptDetail.TabIndex = 6
        Me.chkWithReceiptDetail.Text = "With Receipt Details"
        Me.chkWithReceiptDetail.UseVisualStyleBackColor = True
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(22, 83)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 4
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(123, 83)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 68)
        Me.chkLstCostCentre.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(215, 50)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(21, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "To"
        '
        'chkAsOnDate
        '
        Me.chkAsOnDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAsOnDate.Location = New System.Drawing.Point(22, 49)
        Me.chkAsOnDate.Name = "chkAsOnDate"
        Me.chkAsOnDate.Size = New System.Drawing.Size(95, 17)
        Me.chkAsOnDate.TabIndex = 0
        Me.chkAsOnDate.Text = "Date From"
        Me.chkAsOnDate.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(242, 47)
        Me.dtpTo.Mask = "##-##-####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(82, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "29/09/2010"
        Me.dtpTo.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(126, 47)
        Me.dtpFrom.Mask = "##-##-####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(82, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "29/09/2010"
        Me.dtpFrom.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(234, 263)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 11
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(22, 263)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 9
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(128, 263)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 10
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'DueDateWiseOutstandingRpt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(790, 519)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "DueDateWiseOutstandingRpt"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DueDateWiseOutstandingRpt"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkAsOnDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As GiritechPack.DatePicker
    Friend WithEvents dtpFrom As GiritechPack.DatePicker
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents chkWithReceiptDetail As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtduedate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOrderName As System.Windows.Forms.RadioButton
    Friend WithEvents rbtArea As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOrderRunNo As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtDealerSmith As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCustomer As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOther As System.Windows.Forms.RadioButton
End Class
