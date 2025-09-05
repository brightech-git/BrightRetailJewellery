<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGiftGenerate
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
        Me.BtnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.gridViewTotal = New System.Windows.Forms.DataGridView
        Me.Label1 = New System.Windows.Forms.Label
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.txtBillNo_NUM = New System.Windows.Forms.TextBox
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Grouper1.SuspendLayout()
        CType(Me.gridViewTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.Grouper1.Controls.Add(Me.BtnNew)
        Me.Grouper1.Controls.Add(Me.btnExit)
        Me.Grouper1.Controls.Add(Me.gridViewTotal)
        Me.Grouper1.Controls.Add(Me.Label1)
        Me.Grouper1.Controls.Add(Me.gridView)
        Me.Grouper1.Controls.Add(Me.txtBillNo_NUM)
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
        Me.Grouper1.Size = New System.Drawing.Size(930, 516)
        Me.Grouper1.TabIndex = 0
        '
        'BtnNew
        '
        Me.BtnNew.Location = New System.Drawing.Point(703, 463)
        Me.BtnNew.Name = "BtnNew"
        Me.BtnNew.Size = New System.Drawing.Size(100, 30)
        Me.BtnNew.TabIndex = 7
        Me.BtnNew.Text = "New"
        Me.BtnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(809, 463)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 8
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'gridViewTotal
        '
        Me.gridViewTotal.AllowUserToAddRows = False
        Me.gridViewTotal.AllowUserToDeleteRows = False
        Me.gridViewTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewTotal.Enabled = False
        Me.gridViewTotal.Location = New System.Drawing.Point(8, 415)
        Me.gridViewTotal.Name = "gridViewTotal"
        Me.gridViewTotal.ReadOnly = True
        Me.gridViewTotal.Size = New System.Drawing.Size(900, 42)
        Me.gridViewTotal.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(7, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 14)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "TranNo"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Enabled = False
        Me.gridView.Location = New System.Drawing.Point(8, 55)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(900, 358)
        Me.gridView.TabIndex = 4
        '
        'txtBillNo_NUM
        '
        Me.txtBillNo_NUM.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBillNo_NUM.Location = New System.Drawing.Point(68, 14)
        Me.txtBillNo_NUM.Name = "txtBillNo_NUM"
        Me.txtBillNo_NUM.Size = New System.Drawing.Size(163, 31)
        Me.txtBillNo_NUM.TabIndex = 1
        '
        'frmGiftGenerate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(934, 515)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Grouper1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmGiftGenerate"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gift Generate"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Grouper1.ResumeLayout(False)
        Me.Grouper1.PerformLayout()
        CType(Me.gridViewTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents txtBillNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridViewTotal As System.Windows.Forms.DataGridView
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents BtnNew As System.Windows.Forms.Button
End Class
