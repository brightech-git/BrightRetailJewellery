<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGiftDetail
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
        Me.AdvanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ChequeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CreditCardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HandlingChargeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DiscountToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CashToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tStripGiftVouhcer = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Grouper1 = New CodeVendor.Controls.Grouper
        Me.BtnCancel = New System.Windows.Forms.Button
        Me.BtnOk = New System.Windows.Forms.Button
        Me.cmbGvName = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtGiftAmt = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtGiftValPer = New System.Windows.Forms.TextBox
        Me.txtPayableAmt = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Grouper1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AdvanceToolStripMenuItem, Me.ChequeToolStripMenuItem, Me.CreditCardToolStripMenuItem, Me.HandlingChargeToolStripMenuItem, Me.DiscountToolStripMenuItem, Me.CashToolStripMenuItem, Me.tStripGiftVouhcer, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(184, 180)
        '
        'AdvanceToolStripMenuItem
        '
        Me.AdvanceToolStripMenuItem.Name = "AdvanceToolStripMenuItem"
        Me.AdvanceToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.AdvanceToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.AdvanceToolStripMenuItem.Text = "Advance"
        Me.AdvanceToolStripMenuItem.Visible = False
        '
        'ChequeToolStripMenuItem
        '
        Me.ChequeToolStripMenuItem.Name = "ChequeToolStripMenuItem"
        Me.ChequeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8
        Me.ChequeToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.ChequeToolStripMenuItem.Text = "Cheque"
        Me.ChequeToolStripMenuItem.Visible = False
        '
        'CreditCardToolStripMenuItem
        '
        Me.CreditCardToolStripMenuItem.Name = "CreditCardToolStripMenuItem"
        Me.CreditCardToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7
        Me.CreditCardToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.CreditCardToolStripMenuItem.Text = "Credit Card"
        Me.CreditCardToolStripMenuItem.Visible = False
        '
        'HandlingChargeToolStripMenuItem
        '
        Me.HandlingChargeToolStripMenuItem.Name = "HandlingChargeToolStripMenuItem"
        Me.HandlingChargeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6
        Me.HandlingChargeToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.HandlingChargeToolStripMenuItem.Text = "Handling Charge"
        Me.HandlingChargeToolStripMenuItem.Visible = False
        '
        'DiscountToolStripMenuItem
        '
        Me.DiscountToolStripMenuItem.Name = "DiscountToolStripMenuItem"
        Me.DiscountToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.DiscountToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.DiscountToolStripMenuItem.Text = "Discount"
        Me.DiscountToolStripMenuItem.Visible = False
        '
        'CashToolStripMenuItem
        '
        Me.CashToolStripMenuItem.Name = "CashToolStripMenuItem"
        Me.CashToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4
        Me.CashToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.CashToolStripMenuItem.Text = "Cash"
        Me.CashToolStripMenuItem.Visible = False
        '
        'tStripGiftVouhcer
        '
        Me.tStripGiftVouhcer.Name = "tStripGiftVouhcer"
        Me.tStripGiftVouhcer.ShortcutKeys = System.Windows.Forms.Keys.F10
        Me.tStripGiftVouhcer.Size = New System.Drawing.Size(183, 22)
        Me.tStripGiftVouhcer.Text = "Gift Voucher"
        Me.tStripGiftVouhcer.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'Grouper1
        '
        Me.Grouper1.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper1.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper1.BorderThickness = 1.0!
        Me.Grouper1.Controls.Add(Me.BtnCancel)
        Me.Grouper1.Controls.Add(Me.BtnOk)
        Me.Grouper1.Controls.Add(Me.cmbGvName)
        Me.Grouper1.Controls.Add(Me.Label4)
        Me.Grouper1.Controls.Add(Me.txtGiftAmt)
        Me.Grouper1.Controls.Add(Me.Label3)
        Me.Grouper1.Controls.Add(Me.txtGiftValPer)
        Me.Grouper1.Controls.Add(Me.txtPayableAmt)
        Me.Grouper1.Controls.Add(Me.Label2)
        Me.Grouper1.Controls.Add(Me.Label1)
        Me.Grouper1.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper1.GroupImage = Nothing
        Me.Grouper1.GroupTitle = ""
        Me.Grouper1.Location = New System.Drawing.Point(2, -6)
        Me.Grouper1.Name = "Grouper1"
        Me.Grouper1.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper1.PaintGroupBox = False
        Me.Grouper1.RoundCorners = 10
        Me.Grouper1.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper1.ShadowControl = False
        Me.Grouper1.ShadowThickness = 3
        Me.Grouper1.Size = New System.Drawing.Size(409, 228)
        Me.Grouper1.TabIndex = 0
        '
        'BtnCancel
        '
        Me.BtnCancel.Location = New System.Drawing.Point(177, 163)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(98, 27)
        Me.BtnCancel.TabIndex = 3
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnOk
        '
        Me.BtnOk.Location = New System.Drawing.Point(61, 163)
        Me.BtnOk.Name = "BtnOk"
        Me.BtnOk.Size = New System.Drawing.Size(98, 27)
        Me.BtnOk.TabIndex = 2
        Me.BtnOk.Text = "Generate"
        Me.BtnOk.UseVisualStyleBackColor = True
        '
        'cmbGvName
        '
        Me.cmbGvName.FormattingEnabled = True
        Me.cmbGvName.Location = New System.Drawing.Point(145, 106)
        Me.cmbGvName.Name = "cmbGvName"
        Me.cmbGvName.Size = New System.Drawing.Size(216, 21)
        Me.cmbGvName.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(23, 109)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(115, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Gift Voucher Name"
        '
        'txtGiftAmt
        '
        Me.txtGiftAmt.Location = New System.Drawing.Point(261, 58)
        Me.txtGiftAmt.Name = "txtGiftAmt"
        Me.txtGiftAmt.ReadOnly = True
        Me.txtGiftAmt.Size = New System.Drawing.Size(100, 21)
        Me.txtGiftAmt.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(263, 41)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "GiftValue Amt"
        '
        'txtGiftValPer
        '
        Me.txtGiftValPer.Location = New System.Drawing.Point(148, 58)
        Me.txtGiftValPer.Name = "txtGiftValPer"
        Me.txtGiftValPer.ReadOnly = True
        Me.txtGiftValPer.Size = New System.Drawing.Size(56, 21)
        Me.txtGiftValPer.TabIndex = 7
        '
        'txtPayableAmt
        '
        Me.txtPayableAmt.Location = New System.Drawing.Point(26, 59)
        Me.txtPayableAmt.Name = "txtPayableAmt"
        Me.txtPayableAmt.ReadOnly = True
        Me.txtPayableAmt.Size = New System.Drawing.Size(100, 21)
        Me.txtPayableAmt.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(145, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "GiftValue %"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 43)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Payable Amt"
        '
        'frmGiftDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(413, 227)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Grouper1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmGiftDetail"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gift Detail"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Grouper1.ResumeLayout(False)
        Me.Grouper1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AdvanceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChequeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CreditCardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HandlingChargeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DiscountToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CashToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripGiftVouhcer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Grouper1 As CodeVendor.Controls.Grouper
    Friend WithEvents txtGiftValPer As System.Windows.Forms.TextBox
    Friend WithEvents txtPayableAmt As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbGvName As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtGiftAmt As System.Windows.Forms.TextBox
    Friend WithEvents BtnOk As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
End Class
