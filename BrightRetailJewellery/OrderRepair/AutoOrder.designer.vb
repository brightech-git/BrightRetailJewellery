<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AutoOrder
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
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.txtWeight_WET = New System.Windows.Forms.TextBox
        Me.txtPiece_NUM = New System.Windows.Forms.TextBox
        Me.lblWt = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbPartyName = New System.Windows.Forms.ComboBox
        Me.cmbSubItemName_Own = New System.Windows.Forms.ComboBox
        Me.cmbItemName_Man = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label89 = New System.Windows.Forms.Label
        Me.lblSystemId = New System.Windows.Forms.Label
        Me.lblNodeId = New System.Windows.Forms.Label
        Me.Label88 = New System.Windows.Forms.Label
        Me.Label86 = New System.Windows.Forms.Label
        Me.Label29 = New System.Windows.Forms.Label
        Me.lblBillDate = New System.Windows.Forms.Label
        Me.lblUserName = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.rdbAll = New System.Windows.Forms.RadioButton
        Me.rdbSelected = New System.Windows.Forms.RadioButton
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
        Me.Grouper1.Controls.Add(Me.rdbSelected)
        Me.Grouper1.Controls.Add(Me.rdbAll)
        Me.Grouper1.Controls.Add(Me.btnExit)
        Me.Grouper1.Controls.Add(Me.btnSave)
        Me.Grouper1.Controls.Add(Me.txtWeight_WET)
        Me.Grouper1.Controls.Add(Me.txtPiece_NUM)
        Me.Grouper1.Controls.Add(Me.lblWt)
        Me.Grouper1.Controls.Add(Me.Label7)
        Me.Grouper1.Controls.Add(Me.Label3)
        Me.Grouper1.Controls.Add(Me.cmbPartyName)
        Me.Grouper1.Controls.Add(Me.cmbSubItemName_Own)
        Me.Grouper1.Controls.Add(Me.cmbItemName_Man)
        Me.Grouper1.Controls.Add(Me.Label1)
        Me.Grouper1.Controls.Add(Me.Label2)
        Me.Grouper1.Controls.Add(Me.Label89)
        Me.Grouper1.Controls.Add(Me.lblSystemId)
        Me.Grouper1.Controls.Add(Me.lblNodeId)
        Me.Grouper1.Controls.Add(Me.Label88)
        Me.Grouper1.Controls.Add(Me.Label86)
        Me.Grouper1.Controls.Add(Me.Label29)
        Me.Grouper1.Controls.Add(Me.lblBillDate)
        Me.Grouper1.Controls.Add(Me.lblUserName)
        Me.Grouper1.Controls.Add(Me.Label24)
        Me.Grouper1.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper1.GroupImage = Nothing
        Me.Grouper1.GroupTitle = ""
        Me.Grouper1.Location = New System.Drawing.Point(5, -3)
        Me.Grouper1.Name = "Grouper1"
        Me.Grouper1.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper1.PaintGroupBox = False
        Me.Grouper1.RoundCorners = 10
        Me.Grouper1.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper1.ShadowControl = False
        Me.Grouper1.ShadowThickness = 3
        Me.Grouper1.Size = New System.Drawing.Size(419, 277)
        Me.Grouper1.TabIndex = 1
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(224, 224)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(101, 30)
        Me.btnExit.TabIndex = 41
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(117, 224)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(101, 30)
        Me.btnSave.TabIndex = 40
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'txtWeight_WET
        '
        Me.txtWeight_WET.Location = New System.Drawing.Point(295, 191)
        Me.txtWeight_WET.MaxLength = 10
        Me.txtWeight_WET.Name = "txtWeight_WET"
        Me.txtWeight_WET.Size = New System.Drawing.Size(91, 21)
        Me.txtWeight_WET.TabIndex = 37
        Me.txtWeight_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPiece_NUM
        '
        Me.txtPiece_NUM.Location = New System.Drawing.Point(117, 191)
        Me.txtPiece_NUM.MaxLength = 10
        Me.txtPiece_NUM.Name = "txtPiece_NUM"
        Me.txtPiece_NUM.Size = New System.Drawing.Size(109, 21)
        Me.txtPiece_NUM.TabIndex = 36
        Me.txtPiece_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblWt
        '
        Me.lblWt.AutoSize = True
        Me.lblWt.Location = New System.Drawing.Point(232, 194)
        Me.lblWt.Name = "lblWt"
        Me.lblWt.Size = New System.Drawing.Size(46, 13)
        Me.lblWt.TabIndex = 39
        Me.lblWt.Text = "Weight"
        Me.lblWt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(7, 194)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(37, 13)
        Me.Label7.TabIndex = 38
        Me.Label7.Text = "Piece"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(7, 167)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(74, 13)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "Party Name"
        '
        'cmbPartyName
        '
        Me.cmbPartyName.FormattingEnabled = True
        Me.cmbPartyName.Location = New System.Drawing.Point(117, 164)
        Me.cmbPartyName.Name = "cmbPartyName"
        Me.cmbPartyName.Size = New System.Drawing.Size(269, 21)
        Me.cmbPartyName.TabIndex = 30
        '
        'cmbSubItemName_Own
        '
        Me.cmbSubItemName_Own.FormattingEnabled = True
        Me.cmbSubItemName_Own.Location = New System.Drawing.Point(117, 113)
        Me.cmbSubItemName_Own.Name = "cmbSubItemName_Own"
        Me.cmbSubItemName_Own.Size = New System.Drawing.Size(269, 21)
        Me.cmbSubItemName_Own.TabIndex = 26
        '
        'cmbItemName_Man
        '
        Me.cmbItemName_Man.FormattingEnabled = True
        Me.cmbItemName_Man.Location = New System.Drawing.Point(117, 86)
        Me.cmbItemName_Man.Name = "cmbItemName_Man"
        Me.cmbItemName_Man.Size = New System.Drawing.Size(269, 21)
        Me.cmbItemName_Man.TabIndex = 25
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(7, 89)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 13)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "Item Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(7, 116)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 13)
        Me.Label2.TabIndex = 28
        Me.Label2.Text = "SubItem Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label89
        '
        Me.Label89.AutoSize = True
        Me.Label89.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label89.Location = New System.Drawing.Point(135, 51)
        Me.Label89.Name = "Label89"
        Me.Label89.Size = New System.Drawing.Size(12, 14)
        Me.Label89.TabIndex = 24
        Me.Label89.Text = ":"
        '
        'lblSystemId
        '
        Me.lblSystemId.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSystemId.Location = New System.Drawing.Point(169, 49)
        Me.lblSystemId.Name = "lblSystemId"
        Me.lblSystemId.Size = New System.Drawing.Size(85, 16)
        Me.lblSystemId.TabIndex = 21
        Me.lblSystemId.Text = "N03"
        Me.lblSystemId.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblNodeId
        '
        Me.lblNodeId.AutoSize = True
        Me.lblNodeId.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNodeId.Location = New System.Drawing.Point(24, 49)
        Me.lblNodeId.Name = "lblNodeId"
        Me.lblNodeId.Size = New System.Drawing.Size(48, 16)
        Me.lblNodeId.TabIndex = 17
        Me.lblNodeId.Text = "NODE"
        '
        'Label88
        '
        Me.Label88.AutoSize = True
        Me.Label88.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label88.Location = New System.Drawing.Point(135, 34)
        Me.Label88.Name = "Label88"
        Me.Label88.Size = New System.Drawing.Size(12, 14)
        Me.Label88.TabIndex = 13
        Me.Label88.Text = ":"
        '
        'Label86
        '
        Me.Label86.AutoSize = True
        Me.Label86.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label86.Location = New System.Drawing.Point(135, 15)
        Me.Label86.Name = "Label86"
        Me.Label86.Size = New System.Drawing.Size(12, 14)
        Me.Label86.TabIndex = 15
        Me.Label86.Text = ":"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(5, 14)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(97, 16)
        Me.Label29.TabIndex = 12
        Me.Label29.Text = "ORDER DATE"
        '
        'lblBillDate
        '
        Me.lblBillDate.AutoSize = True
        Me.lblBillDate.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillDate.Location = New System.Drawing.Point(169, 14)
        Me.lblBillDate.Name = "lblBillDate"
        Me.lblBillDate.Size = New System.Drawing.Size(98, 16)
        Me.lblBillDate.TabIndex = 11
        Me.lblBillDate.Text = "08/03/2009"
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserName.Location = New System.Drawing.Point(169, 33)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(126, 16)
        Me.lblUserName.TabIndex = 8
        Me.lblUserName.Text = "ADMINISTRATOR"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(5, 33)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(90, 16)
        Me.Label24.TabIndex = 7
        Me.Label24.Text = "USER NAME"
        '
        'rdbAll
        '
        Me.rdbAll.AutoSize = True
        Me.rdbAll.Checked = True
        Me.rdbAll.Location = New System.Drawing.Point(118, 140)
        Me.rdbAll.Name = "rdbAll"
        Me.rdbAll.Size = New System.Drawing.Size(39, 17)
        Me.rdbAll.TabIndex = 42
        Me.rdbAll.TabStop = True
        Me.rdbAll.Text = "All"
        Me.rdbAll.UseVisualStyleBackColor = True
        '
        'rdbSelected
        '
        Me.rdbSelected.AutoSize = True
        Me.rdbSelected.Location = New System.Drawing.Point(188, 140)
        Me.rdbSelected.Name = "rdbSelected"
        Me.rdbSelected.Size = New System.Drawing.Size(74, 17)
        Me.rdbSelected.TabIndex = 43
        Me.rdbSelected.Text = "Selected"
        Me.rdbSelected.UseVisualStyleBackColor = True
        '
        'AutoOrder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(427, 277)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Grouper1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "AutoOrder"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Order Booking"
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
    Friend WithEvents Grouper1 As CodeVendor.Controls.Grouper
    Friend WithEvents Label88 As System.Windows.Forms.Label
    Friend WithEvents Label86 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents lblBillDate As System.Windows.Forms.Label
    Friend WithEvents lblUserName As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label89 As System.Windows.Forms.Label
    Friend WithEvents lblSystemId As System.Windows.Forms.Label
    Friend WithEvents lblNodeId As System.Windows.Forms.Label
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbSubItemName_Own As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItemName_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbPartyName As System.Windows.Forms.ComboBox
    Friend WithEvents txtWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents lblWt As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents txtPiece_NUM As System.Windows.Forms.TextBox
    Friend WithEvents rdbSelected As System.Windows.Forms.RadioButton
    Friend WithEvents rdbAll As System.Windows.Forms.RadioButton
End Class
