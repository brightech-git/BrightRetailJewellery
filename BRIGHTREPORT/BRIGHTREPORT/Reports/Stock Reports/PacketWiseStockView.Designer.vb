<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PacketWiseStockView
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
        Me.GrpContainer = New System.Windows.Forms.GroupBox
        Me.chkCmbItemName = New BrighttechPack.CheckedComboBox
        Me.chkCmbDesigner = New BrighttechPack.CheckedComboBox
        Me.chkGroupByDesigner = New System.Windows.Forms.CheckBox
        Me.Label = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.chkWithValue = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.chkAsOnDate = New System.Windows.Forms.CheckBox
        Me.dtpTo_OWN = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom_OWN = New BrighttechPack.DatePicker(Me.components)
        Me.chkWithSubitem = New System.Windows.Forms.CheckBox
        Me.chkGroupbyItem = New System.Windows.Forms.CheckBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GrpContainer.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.chkCmbItemName)
        Me.GrpContainer.Controls.Add(Me.chkCmbDesigner)
        Me.GrpContainer.Controls.Add(Me.chkGroupByDesigner)
        Me.GrpContainer.Controls.Add(Me.Label)
        Me.GrpContainer.Controls.Add(Me.Label3)
        Me.GrpContainer.Controls.Add(Me.Label6)
        Me.GrpContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.GrpContainer.Controls.Add(Me.chkWithValue)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.chkAsOnDate)
        Me.GrpContainer.Controls.Add(Me.dtpTo_OWN)
        Me.GrpContainer.Controls.Add(Me.dtpFrom_OWN)
        Me.GrpContainer.Controls.Add(Me.chkWithSubitem)
        Me.GrpContainer.Controls.Add(Me.chkGroupbyItem)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Location = New System.Drawing.Point(242, 89)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(386, 278)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'chkCmbItemName
        '
        Me.chkCmbItemName.CheckOnClick = True
        Me.chkCmbItemName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItemName.DropDownHeight = 1
        Me.chkCmbItemName.FormattingEnabled = True
        Me.chkCmbItemName.IntegralHeight = False
        Me.chkCmbItemName.Location = New System.Drawing.Point(130, 59)
        Me.chkCmbItemName.Name = "chkCmbItemName"
        Me.chkCmbItemName.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbItemName.TabIndex = 5
        Me.chkCmbItemName.ValueSeparator = ", "
        '
        'chkCmbDesigner
        '
        Me.chkCmbDesigner.CheckOnClick = True
        Me.chkCmbDesigner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbDesigner.DropDownHeight = 1
        Me.chkCmbDesigner.FormattingEnabled = True
        Me.chkCmbDesigner.IntegralHeight = False
        Me.chkCmbDesigner.Location = New System.Drawing.Point(130, 90)
        Me.chkCmbDesigner.Name = "chkCmbDesigner"
        Me.chkCmbDesigner.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbDesigner.TabIndex = 7
        Me.chkCmbDesigner.ValueSeparator = ", "
        '
        'chkGroupByDesigner
        '
        Me.chkGroupByDesigner.AutoSize = True
        Me.chkGroupByDesigner.Location = New System.Drawing.Point(130, 155)
        Me.chkGroupByDesigner.Name = "chkGroupByDesigner"
        Me.chkGroupByDesigner.Size = New System.Drawing.Size(134, 17)
        Me.chkGroupByDesigner.TabIndex = 10
        Me.chkGroupByDesigner.Text = "Group by Designer"
        Me.chkGroupByDesigner.UseVisualStyleBackColor = True
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(26, 126)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 8
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(26, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "ItemName"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(26, 95)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(58, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Designer"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(130, 121)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbCostCentre.TabIndex = 9
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'chkWithValue
        '
        Me.chkWithValue.AutoSize = True
        Me.chkWithValue.Location = New System.Drawing.Point(240, 204)
        Me.chkWithValue.Name = "chkWithValue"
        Me.chkWithValue.Size = New System.Drawing.Size(87, 17)
        Me.chkWithValue.TabIndex = 13
        Me.chkWithValue.Text = "With Value"
        Me.chkWithValue.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(219, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(21, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "To"
        '
        'chkAsOnDate
        '
        Me.chkAsOnDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAsOnDate.Location = New System.Drawing.Point(26, 31)
        Me.chkAsOnDate.Name = "chkAsOnDate"
        Me.chkAsOnDate.Size = New System.Drawing.Size(95, 17)
        Me.chkAsOnDate.TabIndex = 0
        Me.chkAsOnDate.Text = "Date From"
        Me.chkAsOnDate.UseVisualStyleBackColor = True
        '
        'dtpTo_OWN
        '
        Me.dtpTo_OWN.Location = New System.Drawing.Point(246, 29)
        Me.dtpTo_OWN.Mask = "##-##-####"
        Me.dtpTo_OWN.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo_OWN.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo_OWN.Name = "dtpTo_OWN"
        Me.dtpTo_OWN.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo_OWN.Size = New System.Drawing.Size(82, 21)
        Me.dtpTo_OWN.TabIndex = 3
        Me.dtpTo_OWN.Text = "29/09/2010"
        Me.dtpTo_OWN.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'dtpFrom_OWN
        '
        Me.dtpFrom_OWN.Location = New System.Drawing.Point(130, 29)
        Me.dtpFrom_OWN.Mask = "##-##-####"
        Me.dtpFrom_OWN.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom_OWN.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom_OWN.Name = "dtpFrom_OWN"
        Me.dtpFrom_OWN.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom_OWN.Size = New System.Drawing.Size(82, 21)
        Me.dtpFrom_OWN.TabIndex = 1
        Me.dtpFrom_OWN.Text = "29/09/2010"
        Me.dtpFrom_OWN.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'chkWithSubitem
        '
        Me.chkWithSubitem.AutoSize = True
        Me.chkWithSubitem.Location = New System.Drawing.Point(130, 204)
        Me.chkWithSubitem.Name = "chkWithSubitem"
        Me.chkWithSubitem.Size = New System.Drawing.Size(104, 17)
        Me.chkWithSubitem.TabIndex = 12
        Me.chkWithSubitem.Text = "With SubItem"
        Me.chkWithSubitem.UseVisualStyleBackColor = True
        '
        'chkGroupbyItem
        '
        Me.chkGroupbyItem.AutoSize = True
        Me.chkGroupbyItem.Location = New System.Drawing.Point(130, 178)
        Me.chkGroupbyItem.Name = "chkGroupbyItem"
        Me.chkGroupbyItem.Size = New System.Drawing.Size(110, 17)
        Me.chkGroupbyItem.TabIndex = 11
        Me.chkGroupbyItem.Text = "Group by Item"
        Me.chkGroupbyItem.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(233, 230)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 16
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(21, 230)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 14
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(127, 230)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 15
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'PacketWiseStockView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(849, 500)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "PacketWiseStockView"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PacketWiseStockView"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkAsOnDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo_OWN As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom_OWN As BrighttechPack.DatePicker
    Friend WithEvents chkWithSubitem As System.Windows.Forms.CheckBox
    Friend WithEvents chkGroupbyItem As System.Windows.Forms.CheckBox
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkWithValue As System.Windows.Forms.CheckBox
    Friend WithEvents chkGroupByDesigner As System.Windows.Forms.CheckBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbItemName As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbDesigner As BrighttechPack.CheckedComboBox
End Class
