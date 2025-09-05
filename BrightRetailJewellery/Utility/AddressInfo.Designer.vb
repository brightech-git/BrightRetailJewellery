<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddressInfo
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
        Me.chkDate = New System.Windows.Forms.CheckBox
        Me.lblTo = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblReligion = New System.Windows.Forms.Label
        Me.chkHasDealer = New System.Windows.Forms.CheckBox
        Me.chkHasSmith = New System.Windows.Forms.CheckBox
        Me.chkHasCustomer = New System.Windows.Forms.CheckBox
        Me.chkRegularCustomer = New System.Windows.Forms.CheckBox
        Me.chkHeadInfo = New System.Windows.Forms.CheckBox
        Me.Label = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.chkCmbArea = New BrighttechPack.CheckedComboBox
        Me.chkCmbReligion = New BrighttechPack.CheckedComboBox
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.GrpContainer.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.chkDate)
        Me.GrpContainer.Controls.Add(Me.dtpTo)
        Me.GrpContainer.Controls.Add(Me.dtpFrom)
        Me.GrpContainer.Controls.Add(Me.lblTo)
        Me.GrpContainer.Controls.Add(Me.chkCmbArea)
        Me.GrpContainer.Controls.Add(Me.Label2)
        Me.GrpContainer.Controls.Add(Me.chkCmbReligion)
        Me.GrpContainer.Controls.Add(Me.lblReligion)
        Me.GrpContainer.Controls.Add(Me.chkHasDealer)
        Me.GrpContainer.Controls.Add(Me.chkHasSmith)
        Me.GrpContainer.Controls.Add(Me.chkHasCustomer)
        Me.GrpContainer.Controls.Add(Me.chkRegularCustomer)
        Me.GrpContainer.Controls.Add(Me.chkHeadInfo)
        Me.GrpContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.GrpContainer.Controls.Add(Me.Label)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Location = New System.Drawing.Point(186, 99)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(573, 298)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Location = New System.Drawing.Point(126, 104)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(86, 17)
        Me.chkDate.TabIndex = 6
        Me.chkDate.Text = "Date From"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Enabled = False
        Me.lblTo.Location = New System.Drawing.Point(328, 105)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(21, 13)
        Me.lblTo.TabIndex = 8
        Me.lblTo.Text = "To"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(123, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Area"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblReligion
        '
        Me.lblReligion.AutoSize = True
        Me.lblReligion.Location = New System.Drawing.Point(123, 78)
        Me.lblReligion.Name = "lblReligion"
        Me.lblReligion.Size = New System.Drawing.Size(52, 13)
        Me.lblReligion.TabIndex = 4
        Me.lblReligion.Text = "Religion"
        Me.lblReligion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkHasDealer
        '
        Me.chkHasDealer.AutoSize = True
        Me.chkHasDealer.Location = New System.Drawing.Point(258, 197)
        Me.chkHasDealer.Name = "chkHasDealer"
        Me.chkHasDealer.Size = New System.Drawing.Size(64, 17)
        Me.chkHasDealer.TabIndex = 13
        Me.chkHasDealer.Text = "Dealer"
        Me.chkHasDealer.UseVisualStyleBackColor = True
        '
        'chkHasSmith
        '
        Me.chkHasSmith.AutoSize = True
        Me.chkHasSmith.Location = New System.Drawing.Point(258, 174)
        Me.chkHasSmith.Name = "chkHasSmith"
        Me.chkHasSmith.Size = New System.Drawing.Size(59, 17)
        Me.chkHasSmith.TabIndex = 12
        Me.chkHasSmith.Text = "Smith"
        Me.chkHasSmith.UseVisualStyleBackColor = True
        '
        'chkHasCustomer
        '
        Me.chkHasCustomer.AutoSize = True
        Me.chkHasCustomer.Location = New System.Drawing.Point(258, 151)
        Me.chkHasCustomer.Name = "chkHasCustomer"
        Me.chkHasCustomer.Size = New System.Drawing.Size(82, 17)
        Me.chkHasCustomer.TabIndex = 11
        Me.chkHasCustomer.Text = "Customer"
        Me.chkHasCustomer.UseVisualStyleBackColor = True
        '
        'chkRegularCustomer
        '
        Me.chkRegularCustomer.AutoSize = True
        Me.chkRegularCustomer.Location = New System.Drawing.Point(218, 220)
        Me.chkRegularCustomer.Name = "chkRegularCustomer"
        Me.chkRegularCustomer.Size = New System.Drawing.Size(157, 17)
        Me.chkRegularCustomer.TabIndex = 14
        Me.chkRegularCustomer.Text = "Regular Customer Info"
        Me.chkRegularCustomer.UseVisualStyleBackColor = True
        '
        'chkHeadInfo
        '
        Me.chkHeadInfo.AutoSize = True
        Me.chkHeadInfo.Location = New System.Drawing.Point(218, 128)
        Me.chkHeadInfo.Name = "chkHeadInfo"
        Me.chkHeadInfo.Size = New System.Drawing.Size(131, 17)
        Me.chkHeadInfo.TabIndex = 10
        Me.chkHeadInfo.Text = "Account Head Info"
        Me.chkHeadInfo.UseVisualStyleBackColor = True
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(123, 22)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 0
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(362, 243)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 17
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(150, 243)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 15
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(256, 243)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 16
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
        'dtpTo
        '
        Me.dtpTo.Enabled = False
        Me.dtpTo.Location = New System.Drawing.Point(369, 101)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 9
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Enabled = False
        Me.dtpFrom.Location = New System.Drawing.Point(218, 101)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 7
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkCmbArea
        '
        Me.chkCmbArea.CheckOnClick = True
        Me.chkCmbArea.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbArea.DropDownHeight = 1
        Me.chkCmbArea.FormattingEnabled = True
        Me.chkCmbArea.IntegralHeight = False
        Me.chkCmbArea.Location = New System.Drawing.Point(218, 45)
        Me.chkCmbArea.Name = "chkCmbArea"
        Me.chkCmbArea.Size = New System.Drawing.Size(244, 22)
        Me.chkCmbArea.TabIndex = 3
        Me.chkCmbArea.ValueSeparator = ", "
        '
        'chkCmbReligion
        '
        Me.chkCmbReligion.CheckOnClick = True
        Me.chkCmbReligion.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbReligion.DropDownHeight = 1
        Me.chkCmbReligion.FormattingEnabled = True
        Me.chkCmbReligion.IntegralHeight = False
        Me.chkCmbReligion.Location = New System.Drawing.Point(218, 73)
        Me.chkCmbReligion.Name = "chkCmbReligion"
        Me.chkCmbReligion.Size = New System.Drawing.Size(244, 22)
        Me.chkCmbReligion.TabIndex = 5
        Me.chkCmbReligion.ValueSeparator = ", "
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(218, 17)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(244, 22)
        Me.chkCmbCostCentre.TabIndex = 1
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'AddressInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(945, 462)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AddressInfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AddressInfo"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents chkHasDealer As System.Windows.Forms.CheckBox
    Friend WithEvents chkHasSmith As System.Windows.Forms.CheckBox
    Friend WithEvents chkHasCustomer As System.Windows.Forms.CheckBox
    Friend WithEvents chkRegularCustomer As System.Windows.Forms.CheckBox
    Friend WithEvents chkHeadInfo As System.Windows.Forms.CheckBox
    Friend WithEvents chkCmbArea As BrighttechPack.CheckedComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkCmbReligion As BrighttechPack.CheckedComboBox
    Friend WithEvents lblReligion As System.Windows.Forms.Label
    Friend WithEvents chkDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents lblTo As System.Windows.Forms.Label
End Class
