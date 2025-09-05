<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBagNoWiseReport
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
        Me.grpContainer = New System.Windows.Forms.GroupBox()
        Me.ChkOnlyPendingBagno = New System.Windows.Forms.CheckBox()
        Me.CmbTrantype = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.Label = New System.Windows.Forms.Label()
        Me.cmbCatName = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button()
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DetailToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DupPrintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grpContainer.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.ChkOnlyPendingBagno)
        Me.grpContainer.Controls.Add(Me.CmbTrantype)
        Me.grpContainer.Controls.Add(Me.Label4)
        Me.grpContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.grpContainer.Controls.Add(Me.Label)
        Me.grpContainer.Controls.Add(Me.cmbCatName)
        Me.grpContainer.Controls.Add(Me.Label8)
        Me.grpContainer.Controls.Add(Me.dtpTo)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.dtpFrom)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.cmbMetal)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.Label3)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Location = New System.Drawing.Point(144, 132)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(505, 222)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'ChkOnlyPendingBagno
        '
        Me.ChkOnlyPendingBagno.AutoSize = True
        Me.ChkOnlyPendingBagno.Location = New System.Drawing.Point(162, 156)
        Me.ChkOnlyPendingBagno.Name = "ChkOnlyPendingBagno"
        Me.ChkOnlyPendingBagno.Size = New System.Drawing.Size(141, 17)
        Me.ChkOnlyPendingBagno.TabIndex = 12
        Me.ChkOnlyPendingBagno.Text = "Only Pending Bagno"
        Me.ChkOnlyPendingBagno.UseVisualStyleBackColor = True
        '
        'CmbTrantype
        '
        Me.CmbTrantype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbTrantype.FormattingEnabled = True
        Me.CmbTrantype.Location = New System.Drawing.Point(162, 129)
        Me.CmbTrantype.Name = "CmbTrantype"
        Me.CmbTrantype.Size = New System.Drawing.Size(231, 21)
        Me.CmbTrantype.TabIndex = 11
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(57, 133)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Trantype"
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(162, 101)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(231, 22)
        Me.chkCmbCostCentre.TabIndex = 9
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(57, 106)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 8
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCatName
        '
        Me.cmbCatName.DropDownHeight = 50
        Me.cmbCatName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCatName.FormattingEnabled = True
        Me.cmbCatName.IntegralHeight = False
        Me.cmbCatName.Location = New System.Drawing.Point(162, 74)
        Me.cmbCatName.Name = "cmbCatName"
        Me.cmbCatName.Size = New System.Drawing.Size(231, 21)
        Me.cmbCatName.TabIndex = 7
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(57, 74)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(82, 21)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Category"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(291, 20)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(102, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "06/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(304, 177)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 15
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(162, 20)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(96, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "06/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(198, 177)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 14
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(92, 177)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 13
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cmbMetal
        '
        Me.cmbMetal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(162, 47)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(231, 21)
        Me.cmbMetal.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(264, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(57, 51)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Metal"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(57, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Bill Date From"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.DetailToolStripMenuItem, Me.DupPrintToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(171, 92)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'DetailToolStripMenuItem
        '
        Me.DetailToolStripMenuItem.Name = "DetailToolStripMenuItem"
        Me.DetailToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.DetailToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.DetailToolStripMenuItem.Text = "Detail View"
        '
        'DupPrintToolStripMenuItem
        '
        Me.DupPrintToolStripMenuItem.Name = "DupPrintToolStripMenuItem"
        Me.DupPrintToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.DupPrintToolStripMenuItem.Text = "Dup Print"
        '
        'frmBagNoWiseReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(782, 522)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmBagNoWiseReport"
        Me.Text = "BagNo Wise Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents DetailToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DupPrintToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As Label
    Friend WithEvents cmbCatName As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents CmbTrantype As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents ChkOnlyPendingBagno As CheckBox
End Class
