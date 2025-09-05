<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBillInitialize
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbCashCounter = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Grouper2 = New CodeVendor.Controls.Grouper
        Me.chkTouchBased = New System.Windows.Forms.CheckBox
        Me.lblBalanceIn = New System.Windows.Forms.Label
        Me.rbtWeight = New System.Windows.Forms.RadioButton
        Me.rbtAmount = New System.Windows.Forms.RadioButton
        Me.dtpBillDate = New BrighttechPack.DatePicker(Me.components)
        Me.lblChangePassword = New System.Windows.Forms.Label
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Grouper2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(41, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(138, 89)
        Me.txtPassword.MaxLength = 6
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(98, 21)
        Me.txtPassword.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(41, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Cash Counter"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCashCounter
        '
        Me.cmbCashCounter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCashCounter.FormattingEnabled = True
        Me.cmbCashCounter.Location = New System.Drawing.Point(138, 57)
        Me.cmbCashCounter.Name = "cmbCashCounter"
        Me.cmbCashCounter.Size = New System.Drawing.Size(260, 21)
        Me.cmbCashCounter.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(41, 93)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "PassWord"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(136, 134)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 10
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(242, 134)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 11
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 26)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'Grouper2
        '
        Me.Grouper2.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper2.BackgroundGradientColor = System.Drawing.Color.White
        Me.Grouper2.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper2.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper2.BorderThickness = 1.0!
        Me.Grouper2.Controls.Add(Me.lblChangePassword)
        Me.Grouper2.Controls.Add(Me.chkTouchBased)
        Me.Grouper2.Controls.Add(Me.lblBalanceIn)
        Me.Grouper2.Controls.Add(Me.rbtWeight)
        Me.Grouper2.Controls.Add(Me.rbtAmount)
        Me.Grouper2.Controls.Add(Me.dtpBillDate)
        Me.Grouper2.Controls.Add(Me.Label1)
        Me.Grouper2.Controls.Add(Me.btnOk)
        Me.Grouper2.Controls.Add(Me.Label2)
        Me.Grouper2.Controls.Add(Me.btnExit)
        Me.Grouper2.Controls.Add(Me.txtPassword)
        Me.Grouper2.Controls.Add(Me.Label3)
        Me.Grouper2.Controls.Add(Me.cmbCashCounter)
        Me.Grouper2.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper2.GroupImage = Nothing
        Me.Grouper2.GroupTitle = ""
        Me.Grouper2.Location = New System.Drawing.Point(6, -2)
        Me.Grouper2.Name = "Grouper2"
        Me.Grouper2.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper2.PaintGroupBox = False
        Me.Grouper2.RoundCorners = 10
        Me.Grouper2.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper2.ShadowControl = False
        Me.Grouper2.ShadowThickness = 3
        Me.Grouper2.Size = New System.Drawing.Size(433, 170)
        Me.Grouper2.TabIndex = 0
        '
        'chkTouchBased
        '
        Me.chkTouchBased.AutoSize = True
        Me.chkTouchBased.Location = New System.Drawing.Point(283, 116)
        Me.chkTouchBased.Name = "chkTouchBased"
        Me.chkTouchBased.Size = New System.Drawing.Size(99, 17)
        Me.chkTouchBased.TabIndex = 9
        Me.chkTouchBased.Text = "Touch Based"
        Me.chkTouchBased.UseVisualStyleBackColor = True
        Me.chkTouchBased.Visible = False
        '
        'lblBalanceIn
        '
        Me.lblBalanceIn.AutoSize = True
        Me.lblBalanceIn.Location = New System.Drawing.Point(41, 118)
        Me.lblBalanceIn.Name = "lblBalanceIn"
        Me.lblBalanceIn.Size = New System.Drawing.Size(68, 13)
        Me.lblBalanceIn.TabIndex = 6
        Me.lblBalanceIn.Text = "Balance In"
        '
        'rbtWeight
        '
        Me.rbtWeight.AutoSize = True
        Me.rbtWeight.Location = New System.Drawing.Point(213, 116)
        Me.rbtWeight.Name = "rbtWeight"
        Me.rbtWeight.Size = New System.Drawing.Size(64, 17)
        Me.rbtWeight.TabIndex = 8
        Me.rbtWeight.Text = "Weight"
        Me.rbtWeight.UseVisualStyleBackColor = True
        '
        'rbtAmount
        '
        Me.rbtAmount.AutoSize = True
        Me.rbtAmount.Checked = True
        Me.rbtAmount.Location = New System.Drawing.Point(138, 116)
        Me.rbtAmount.Name = "rbtAmount"
        Me.rbtAmount.Size = New System.Drawing.Size(69, 17)
        Me.rbtAmount.TabIndex = 7
        Me.rbtAmount.TabStop = True
        Me.rbtAmount.Text = "Amount"
        Me.rbtAmount.UseVisualStyleBackColor = True
        '
        'dtpBillDate
        '
        Me.dtpBillDate.Location = New System.Drawing.Point(138, 25)
        Me.dtpBillDate.Mask = "##/##/####"
        Me.dtpBillDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpBillDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpBillDate.Name = "dtpBillDate"
        Me.dtpBillDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpBillDate.Size = New System.Drawing.Size(100, 21)
        Me.dtpBillDate.TabIndex = 1
        Me.dtpBillDate.Text = "06/03/9998"
        Me.dtpBillDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'lblChangePassword
        '
        Me.lblChangePassword.AutoSize = True
        Me.lblChangePassword.BackColor = System.Drawing.Color.Transparent
        Me.lblChangePassword.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblChangePassword.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblChangePassword.Location = New System.Drawing.Point(289, 92)
        Me.lblChangePassword.Name = "lblChangePassword"
        Me.lblChangePassword.Size = New System.Drawing.Size(109, 13)
        Me.lblChangePassword.TabIndex = 12
        Me.lblChangePassword.Text = "Change Password"
        '
        'frmBillInitialize
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(445, 174)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Grouper2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmBillInitialize"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Point of Sale Information"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Grouper2.ResumeLayout(False)
        Me.Grouper2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbCashCounter As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Grouper2 As CodeVendor.Controls.Grouper
    Friend WithEvents dtpBillDate As BrighttechPack.DatePicker
    Friend WithEvents lblBalanceIn As System.Windows.Forms.Label
    Friend WithEvents rbtWeight As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAmount As System.Windows.Forms.RadioButton
    Friend WithEvents chkTouchBased As System.Windows.Forms.CheckBox
    Friend WithEvents lblChangePassword As System.Windows.Forms.Label
End Class
