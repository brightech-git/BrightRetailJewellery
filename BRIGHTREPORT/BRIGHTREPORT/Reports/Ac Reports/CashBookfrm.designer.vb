<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CashBookfrm
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
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox
        Me.grpContainer = New System.Windows.Forms.GroupBox
        Me.chkDetailed = New System.Windows.Forms.CheckBox
        Me.rbtTranNo = New System.Windows.Forms.RadioButton
        Me.rbtAcName = New System.Windows.Forms.RadioButton
        Me.chkOpening = New System.Windows.Forms.CheckBox
        Me.chkPageBreak = New System.Windows.Forms.CheckBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpContainer.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(225, 14)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(76, 14)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(191, 17)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(21, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(9, 54)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 4
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(6, 73)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(312, 84)
        Me.chkLstCostCentre.TabIndex = 5
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.chkDetailed)
        Me.grpContainer.Controls.Add(Me.rbtTranNo)
        Me.grpContainer.Controls.Add(Me.rbtAcName)
        Me.grpContainer.Controls.Add(Me.chkOpening)
        Me.grpContainer.Controls.Add(Me.chkPageBreak)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.dtpTo)
        Me.grpContainer.Controls.Add(Me.chkLstCostCentre)
        Me.grpContainer.Controls.Add(Me.dtpFrom)
        Me.grpContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.grpContainer.Controls.Add(Me.Label3)
        Me.grpContainer.Location = New System.Drawing.Point(301, 53)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(326, 279)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'chkDetailed
        '
        Me.chkDetailed.AutoSize = True
        Me.chkDetailed.Location = New System.Drawing.Point(194, 173)
        Me.chkDetailed.Name = "chkDetailed"
        Me.chkDetailed.Size = New System.Drawing.Size(73, 17)
        Me.chkDetailed.TabIndex = 1
        Me.chkDetailed.Text = "Detailed"
        Me.chkDetailed.UseVisualStyleBackColor = True
        '
        'rbtTranNo
        '
        Me.rbtTranNo.AutoSize = True
        Me.rbtTranNo.Location = New System.Drawing.Point(154, 207)
        Me.rbtTranNo.Name = "rbtTranNo"
        Me.rbtTranNo.Size = New System.Drawing.Size(66, 17)
        Me.rbtTranNo.TabIndex = 10
        Me.rbtTranNo.TabStop = True
        Me.rbtTranNo.Text = "TranNo"
        Me.rbtTranNo.UseVisualStyleBackColor = True
        '
        'rbtAcName
        '
        Me.rbtAcName.AutoSize = True
        Me.rbtAcName.Location = New System.Drawing.Point(76, 207)
        Me.rbtAcName.Name = "rbtAcName"
        Me.rbtAcName.Size = New System.Drawing.Size(72, 17)
        Me.rbtAcName.TabIndex = 9
        Me.rbtAcName.TabStop = True
        Me.rbtAcName.Text = "AcName"
        Me.rbtAcName.UseVisualStyleBackColor = True
        '
        'chkOpening
        '
        Me.chkOpening.AutoSize = True
        Me.chkOpening.Location = New System.Drawing.Point(9, 173)
        Me.chkOpening.Name = "chkOpening"
        Me.chkOpening.Size = New System.Drawing.Size(73, 17)
        Me.chkOpening.TabIndex = 6
        Me.chkOpening.Text = "Opening"
        Me.chkOpening.UseVisualStyleBackColor = True
        '
        'chkPageBreak
        '
        Me.chkPageBreak.AutoSize = True
        Me.chkPageBreak.Location = New System.Drawing.Point(98, 173)
        Me.chkPageBreak.Name = "chkPageBreak"
        Me.chkPageBreak.Size = New System.Drawing.Size(92, 17)
        Me.chkPageBreak.TabIndex = 7
        Me.chkPageBreak.Text = "Page Break"
        Me.chkPageBreak.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(221, 240)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 13
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(9, 240)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 11
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(115, 240)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 12
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 209)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Order By"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'CashBookfrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(928, 524)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.grpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "CashBookfrm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cash Book"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkOpening As System.Windows.Forms.CheckBox
    Friend WithEvents chkPageBreak As System.Windows.Forms.CheckBox
    Friend WithEvents rbtTranNo As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAcName As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkDetailed As System.Windows.Forms.CheckBox
End Class
