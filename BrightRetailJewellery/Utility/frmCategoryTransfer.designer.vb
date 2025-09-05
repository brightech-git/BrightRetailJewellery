<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCategoryTransfer
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
        Me.btnTransfer = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.Label = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.cmbCatNamefrom = New System.Windows.Forms.ComboBox
        Me.cmbCategoryTo = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnnew = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtTranno = New System.Windows.Forms.TextBox
        Me.rbtAcupdate = New System.Windows.Forms.RadioButton
        Me.rbtCatUpdate = New System.Windows.Forms.RadioButton
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnTransfer
        '
        Me.btnTransfer.Location = New System.Drawing.Point(113, 212)
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.Size = New System.Drawing.Size(114, 32)
        Me.btnTransfer.TabIndex = 16
        Me.btnTransfer.Text = "&Transfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(386, 212)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(114, 32)
        Me.btnExit.TabIndex = 18
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(238, 87)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(23, 14)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(26, 89)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 14)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Date From"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem1})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(222, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'NewToolStripMenuItem1
        '
        Me.NewToolStripMenuItem1.Name = "NewToolStripMenuItem1"
        Me.NewToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem1.Size = New System.Drawing.Size(221, 22)
        Me.NewToolStripMenuItem1.Text = "NewToolStripMenuItem1"
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(113, 84)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(118, 22)
        Me.dtpFrom.TabIndex = 7
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(275, 84)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(118, 22)
        Me.dtpTo.TabIndex = 9
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.Enabled = False
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(114, 27)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(386, 23)
        Me.chkCmbCostCentre.TabIndex = 3
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(114, -1)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(386, 23)
        Me.chkCmbCompany.TabIndex = 1
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(21, 30)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(84, 14)
        Me.Label.TabIndex = 2
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(37, 2)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(68, 14)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Company"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(1, 154)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(106, 21)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "Category from"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCatNamefrom
        '
        Me.cmbCatNamefrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCatNamefrom.Enabled = False
        Me.cmbCatNamefrom.FormattingEnabled = True
        Me.cmbCatNamefrom.Location = New System.Drawing.Point(113, 156)
        Me.cmbCatNamefrom.Name = "cmbCatNamefrom"
        Me.cmbCatNamefrom.Size = New System.Drawing.Size(386, 22)
        Me.cmbCatNamefrom.TabIndex = 13
        '
        'cmbCategoryTo
        '
        Me.cmbCategoryTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCategoryTo.Enabled = False
        Me.cmbCategoryTo.FormattingEnabled = True
        Me.cmbCategoryTo.Location = New System.Drawing.Point(114, 184)
        Me.cmbCategoryTo.Name = "cmbCategoryTo"
        Me.cmbCategoryTo.Size = New System.Drawing.Size(386, 22)
        Me.cmbCategoryTo.TabIndex = 15
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(20, 183)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 21)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Category to"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnnew
        '
        Me.btnnew.Location = New System.Drawing.Point(248, 213)
        Me.btnnew.Name = "btnnew"
        Me.btnnew.Size = New System.Drawing.Size(114, 32)
        Me.btnnew.TabIndex = 17
        Me.btnnew.Text = "New[F3]"
        Me.btnnew.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(39, 59)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 14)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Tran no"
        '
        'txtTranno
        '
        Me.txtTranno.Location = New System.Drawing.Point(114, 56)
        Me.txtTranno.Name = "txtTranno"
        Me.txtTranno.Size = New System.Drawing.Size(147, 22)
        Me.txtTranno.TabIndex = 5
        '
        'rbtAcupdate
        '
        Me.rbtAcupdate.AutoSize = True
        Me.rbtAcupdate.Location = New System.Drawing.Point(113, 122)
        Me.rbtAcupdate.Name = "rbtAcupdate"
        Me.rbtAcupdate.Size = New System.Drawing.Size(163, 18)
        Me.rbtAcupdate.TabIndex = 10
        Me.rbtAcupdate.TabStop = True
        Me.rbtAcupdate.Text = "Account Update Only"
        Me.rbtAcupdate.UseVisualStyleBackColor = True
        '
        'rbtCatUpdate
        '
        Me.rbtCatUpdate.AutoSize = True
        Me.rbtCatUpdate.Location = New System.Drawing.Point(295, 122)
        Me.rbtCatUpdate.Name = "rbtCatUpdate"
        Me.rbtCatUpdate.Size = New System.Drawing.Size(200, 18)
        Me.rbtCatUpdate.TabIndex = 11
        Me.rbtCatUpdate.TabStop = True
        Me.rbtCatUpdate.Text = "Category /Account Update"
        Me.rbtCatUpdate.UseVisualStyleBackColor = True
        '
        'frmCategoryTransfer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(532, 256)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.rbtCatUpdate)
        Me.Controls.Add(Me.rbtAcupdate)
        Me.Controls.Add(Me.txtTranno)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmbCategoryTo)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmbCatNamefrom)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.chkCmbCostCentre)
        Me.Controls.Add(Me.chkCmbCompany)
        Me.Controls.Add(Me.Label)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.dtpTo)
        Me.Controls.Add(Me.dtpFrom)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnnew)
        Me.Controls.Add(Me.btnTransfer)
        Me.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmCategoryTransfer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Category Transfer"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnTransfer As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbCatNamefrom As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCategoryTo As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnnew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtTranno As System.Windows.Forms.TextBox
    Friend WithEvents rbtAcupdate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCatUpdate As System.Windows.Forms.RadioButton
End Class
