<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEstPay
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
        Me.grpAdj = New CodeVendor.Controls.Grouper
        Me.txtAdjIndDiscount = New System.Windows.Forms.TextBox
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.txtAdjGiftVoucher_AMT = New System.Windows.Forms.TextBox
        Me.Label28 = New System.Windows.Forms.Label
        Me.Label43 = New System.Windows.Forms.Label
        Me.Label41 = New System.Windows.Forms.Label
        Me.Label44 = New System.Windows.Forms.Label
        Me.Label38 = New System.Windows.Forms.Label
        Me.Label59 = New System.Windows.Forms.Label
        Me.txtAdjCredit_AMT = New System.Windows.Forms.TextBox
        Me.Label40 = New System.Windows.Forms.Label
        Me.Label39 = New System.Windows.Forms.Label
        Me.txtAdjDiscount_AMT = New System.Windows.Forms.TextBox
        Me.txtAdjAdvance_AMT = New System.Windows.Forms.TextBox
        Me.Label57 = New System.Windows.Forms.Label
        Me.Label42 = New System.Windows.Forms.Label
        Me.Label27 = New System.Windows.Forms.Label
        Me.Label45 = New System.Windows.Forms.Label
        Me.txtAdjChitCard_AMT = New System.Windows.Forms.TextBox
        Me.Label54 = New System.Windows.Forms.Label
        Me.txtAdjHandlingCharge_AMT = New System.Windows.Forms.TextBox
        Me.txtAdjRoundoff_AMT = New System.Windows.Forms.TextBox
        Me.Label46 = New System.Windows.Forms.Label
        Me.Label148 = New System.Windows.Forms.Label
        Me.txtAdjCash_AMT = New System.Windows.Forms.TextBox
        Me.Label26 = New System.Windows.Forms.Label
        Me.txtAdjReceive_AMT = New System.Windows.Forms.TextBox
        Me.Label47 = New System.Windows.Forms.Label
        Me.txtAdjCheque_AMT = New System.Windows.Forms.TextBox
        Me.txtAdjCreditCard_AMT = New System.Windows.Forms.TextBox
        Me.cmbRecPay_MAN = New System.Windows.Forms.ComboBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AdvanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ChequeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CreditCardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HandlingChargeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DiscountToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CashToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tStripGiftVouhcer = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SchemeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CreditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpAdj.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpAdj
        '
        Me.grpAdj.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpAdj.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpAdj.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpAdj.BorderColor = System.Drawing.Color.Transparent
        Me.grpAdj.BorderThickness = 1.0!
        Me.grpAdj.Controls.Add(Me.txtAdjIndDiscount)
        Me.grpAdj.Controls.Add(Me.btnSave)
        Me.grpAdj.Controls.Add(Me.btnExit)
        Me.grpAdj.Controls.Add(Me.txtAdjGiftVoucher_AMT)
        Me.grpAdj.Controls.Add(Me.Label28)
        Me.grpAdj.Controls.Add(Me.Label43)
        Me.grpAdj.Controls.Add(Me.Label41)
        Me.grpAdj.Controls.Add(Me.Label44)
        Me.grpAdj.Controls.Add(Me.Label38)
        Me.grpAdj.Controls.Add(Me.Label59)
        Me.grpAdj.Controls.Add(Me.txtAdjCredit_AMT)
        Me.grpAdj.Controls.Add(Me.Label40)
        Me.grpAdj.Controls.Add(Me.Label39)
        Me.grpAdj.Controls.Add(Me.txtAdjDiscount_AMT)
        Me.grpAdj.Controls.Add(Me.txtAdjAdvance_AMT)
        Me.grpAdj.Controls.Add(Me.Label57)
        Me.grpAdj.Controls.Add(Me.Label42)
        Me.grpAdj.Controls.Add(Me.Label27)
        Me.grpAdj.Controls.Add(Me.Label45)
        Me.grpAdj.Controls.Add(Me.txtAdjChitCard_AMT)
        Me.grpAdj.Controls.Add(Me.Label54)
        Me.grpAdj.Controls.Add(Me.txtAdjHandlingCharge_AMT)
        Me.grpAdj.Controls.Add(Me.txtAdjRoundoff_AMT)
        Me.grpAdj.Controls.Add(Me.Label46)
        Me.grpAdj.Controls.Add(Me.Label148)
        Me.grpAdj.Controls.Add(Me.txtAdjCash_AMT)
        Me.grpAdj.Controls.Add(Me.Label26)
        Me.grpAdj.Controls.Add(Me.txtAdjReceive_AMT)
        Me.grpAdj.Controls.Add(Me.Label47)
        Me.grpAdj.Controls.Add(Me.txtAdjCheque_AMT)
        Me.grpAdj.Controls.Add(Me.txtAdjCreditCard_AMT)
        Me.grpAdj.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpAdj.GroupImage = Nothing
        Me.grpAdj.GroupTitle = ""
        Me.grpAdj.Location = New System.Drawing.Point(171, 1)
        Me.grpAdj.Name = "grpAdj"
        Me.grpAdj.Padding = New System.Windows.Forms.Padding(20)
        Me.grpAdj.PaintGroupBox = False
        Me.grpAdj.RoundCorners = 10
        Me.grpAdj.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpAdj.ShadowControl = False
        Me.grpAdj.ShadowThickness = 3
        Me.grpAdj.Size = New System.Drawing.Size(262, 323)
        Me.grpAdj.TabIndex = 0
        '
        'txtAdjIndDiscount
        '
        Me.txtAdjIndDiscount.Location = New System.Drawing.Point(114, 234)
        Me.txtAdjIndDiscount.Name = "txtAdjIndDiscount"
        Me.txtAdjIndDiscount.Size = New System.Drawing.Size(12, 21)
        Me.txtAdjIndDiscount.TabIndex = 30
        Me.txtAdjIndDiscount.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(20, 286)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 31
        Me.btnSave.Text = "&Ok [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(126, 286)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 32
        Me.btnExit.Text = "&Cancel"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'txtAdjGiftVoucher_AMT
        '
        Me.txtAdjGiftVoucher_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjGiftVoucher_AMT.Location = New System.Drawing.Point(127, 112)
        Me.txtAdjGiftVoucher_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjGiftVoucher_AMT.MaxLength = 12
        Me.txtAdjGiftVoucher_AMT.Name = "txtAdjGiftVoucher_AMT"
        Me.txtAdjGiftVoucher_AMT.Size = New System.Drawing.Size(111, 22)
        Me.txtAdjGiftVoucher_AMT.TabIndex = 11
        Me.txtAdjGiftVoucher_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.BackColor = System.Drawing.Color.Transparent
        Me.Label28.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(3, 140)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(96, 14)
        Me.Label28.TabIndex = 13
        Me.Label28.Text = "[F9]  Scheme"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.BackColor = System.Drawing.Color.Transparent
        Me.Label43.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.Location = New System.Drawing.Point(45, 20)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(59, 14)
        Me.Label43.TabIndex = 0
        Me.Label43.Text = "Receive"
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.BackColor = System.Drawing.Color.Transparent
        Me.Label41.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.Location = New System.Drawing.Point(43, 236)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(64, 14)
        Me.Label41.TabIndex = 25
        Me.Label41.Text = "Discount"
        Me.Label41.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.BackColor = System.Drawing.Color.Transparent
        Me.Label44.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.Location = New System.Drawing.Point(43, 116)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(85, 14)
        Me.Label44.TabIndex = 10
        Me.Label44.Text = "Gift&Voucher"
        Me.Label44.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.BackColor = System.Drawing.Color.Transparent
        Me.Label38.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.Location = New System.Drawing.Point(43, 164)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(56, 14)
        Me.Label38.TabIndex = 16
        Me.Label38.Text = "Cheque"
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label59
        '
        Me.Label59.AutoSize = True
        Me.Label59.BackColor = System.Drawing.Color.Transparent
        Me.Label59.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label59.Location = New System.Drawing.Point(3, 236)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(36, 14)
        Me.Label59.TabIndex = 24
        Me.Label59.Text = "[F5]"
        Me.Label59.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjCredit_AMT
        '
        Me.txtAdjCredit_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCredit_AMT.Location = New System.Drawing.Point(127, 88)
        Me.txtAdjCredit_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCredit_AMT.MaxLength = 12
        Me.txtAdjCredit_AMT.Name = "txtAdjCredit_AMT"
        Me.txtAdjCredit_AMT.Size = New System.Drawing.Size(111, 22)
        Me.txtAdjCredit_AMT.TabIndex = 8
        Me.txtAdjCredit_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.BackColor = System.Drawing.Color.Transparent
        Me.Label40.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label40.Location = New System.Drawing.Point(43, 188)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(82, 14)
        Me.Label40.TabIndex = 19
        Me.Label40.Text = "Credit Card"
        Me.Label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.BackColor = System.Drawing.Color.Transparent
        Me.Label39.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.Location = New System.Drawing.Point(3, 92)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(89, 14)
        Me.Label39.TabIndex = 7
        Me.Label39.Text = "[F11] Credit"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjDiscount_AMT
        '
        Me.txtAdjDiscount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjDiscount_AMT.Location = New System.Drawing.Point(127, 232)
        Me.txtAdjDiscount_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjDiscount_AMT.MaxLength = 12
        Me.txtAdjDiscount_AMT.Name = "txtAdjDiscount_AMT"
        Me.txtAdjDiscount_AMT.Size = New System.Drawing.Size(111, 22)
        Me.txtAdjDiscount_AMT.TabIndex = 26
        Me.txtAdjDiscount_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjAdvance_AMT
        '
        Me.txtAdjAdvance_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjAdvance_AMT.Location = New System.Drawing.Point(127, 64)
        Me.txtAdjAdvance_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjAdvance_AMT.MaxLength = 12
        Me.txtAdjAdvance_AMT.Name = "txtAdjAdvance_AMT"
        Me.txtAdjAdvance_AMT.Size = New System.Drawing.Size(111, 22)
        Me.txtAdjAdvance_AMT.TabIndex = 5
        Me.txtAdjAdvance_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.BackColor = System.Drawing.Color.Transparent
        Me.Label57.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label57.Location = New System.Drawing.Point(3, 164)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(36, 14)
        Me.Label57.TabIndex = 15
        Me.Label57.Text = "[F8]"
        Me.Label57.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.BackColor = System.Drawing.Color.Transparent
        Me.Label42.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.Location = New System.Drawing.Point(43, 260)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(39, 14)
        Me.Label42.TabIndex = 28
        Me.Label42.Text = "Cash"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.BackColor = System.Drawing.Color.Transparent
        Me.Label27.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label27.Location = New System.Drawing.Point(43, 68)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(63, 14)
        Me.Label27.TabIndex = 4
        Me.Label27.Text = "Advance"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.BackColor = System.Drawing.Color.Transparent
        Me.Label45.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.Location = New System.Drawing.Point(43, 212)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(84, 14)
        Me.Label45.TabIndex = 22
        Me.Label45.Text = "Hand Charg"
        Me.Label45.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjChitCard_AMT
        '
        Me.txtAdjChitCard_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjChitCard_AMT.Location = New System.Drawing.Point(127, 136)
        Me.txtAdjChitCard_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjChitCard_AMT.MaxLength = 12
        Me.txtAdjChitCard_AMT.Name = "txtAdjChitCard_AMT"
        Me.txtAdjChitCard_AMT.Size = New System.Drawing.Size(111, 22)
        Me.txtAdjChitCard_AMT.TabIndex = 14
        Me.txtAdjChitCard_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.BackColor = System.Drawing.Color.Transparent
        Me.Label54.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label54.Location = New System.Drawing.Point(3, 188)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(36, 14)
        Me.Label54.TabIndex = 18
        Me.Label54.Text = "[F7]"
        Me.Label54.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjHandlingCharge_AMT
        '
        Me.txtAdjHandlingCharge_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjHandlingCharge_AMT.Location = New System.Drawing.Point(127, 208)
        Me.txtAdjHandlingCharge_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjHandlingCharge_AMT.MaxLength = 12
        Me.txtAdjHandlingCharge_AMT.Name = "txtAdjHandlingCharge_AMT"
        Me.txtAdjHandlingCharge_AMT.Size = New System.Drawing.Size(111, 22)
        Me.txtAdjHandlingCharge_AMT.TabIndex = 23
        Me.txtAdjHandlingCharge_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjRoundoff_AMT
        '
        Me.txtAdjRoundoff_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjRoundoff_AMT.Location = New System.Drawing.Point(127, 40)
        Me.txtAdjRoundoff_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjRoundoff_AMT.MaxLength = 12
        Me.txtAdjRoundoff_AMT.Name = "txtAdjRoundoff_AMT"
        Me.txtAdjRoundoff_AMT.Size = New System.Drawing.Size(111, 22)
        Me.txtAdjRoundoff_AMT.TabIndex = 2
        Me.txtAdjRoundoff_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label46
        '
        Me.Label46.AutoSize = True
        Me.Label46.BackColor = System.Drawing.Color.Transparent
        Me.Label46.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label46.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label46.Location = New System.Drawing.Point(3, 68)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(45, 14)
        Me.Label46.TabIndex = 3
        Me.Label46.Text = "[F12]"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label148
        '
        Me.Label148.AutoSize = True
        Me.Label148.BackColor = System.Drawing.Color.Transparent
        Me.Label148.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label148.Location = New System.Drawing.Point(3, 116)
        Me.Label148.Name = "Label148"
        Me.Label148.Size = New System.Drawing.Size(45, 14)
        Me.Label148.TabIndex = 9
        Me.Label148.Text = "[F10]"
        Me.Label148.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjCash_AMT
        '
        Me.txtAdjCash_AMT.BackColor = System.Drawing.SystemColors.Window
        Me.txtAdjCash_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCash_AMT.Location = New System.Drawing.Point(127, 256)
        Me.txtAdjCash_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCash_AMT.MaxLength = 12
        Me.txtAdjCash_AMT.Name = "txtAdjCash_AMT"
        Me.txtAdjCash_AMT.ReadOnly = True
        Me.txtAdjCash_AMT.Size = New System.Drawing.Size(111, 22)
        Me.txtAdjCash_AMT.TabIndex = 29
        Me.txtAdjCash_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.BackColor = System.Drawing.Color.Transparent
        Me.Label26.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(3, 212)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(36, 14)
        Me.Label26.TabIndex = 21
        Me.Label26.Text = "[F6]"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjReceive_AMT
        '
        Me.txtAdjReceive_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjReceive_AMT.Location = New System.Drawing.Point(127, 16)
        Me.txtAdjReceive_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjReceive_AMT.MaxLength = 12
        Me.txtAdjReceive_AMT.Name = "txtAdjReceive_AMT"
        Me.txtAdjReceive_AMT.Size = New System.Drawing.Size(111, 22)
        Me.txtAdjReceive_AMT.TabIndex = 1
        Me.txtAdjReceive_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.BackColor = System.Drawing.Color.Transparent
        Me.Label47.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.Location = New System.Drawing.Point(3, 260)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(36, 14)
        Me.Label47.TabIndex = 27
        Me.Label47.Text = "[F4]"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjCheque_AMT
        '
        Me.txtAdjCheque_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCheque_AMT.Location = New System.Drawing.Point(127, 160)
        Me.txtAdjCheque_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCheque_AMT.MaxLength = 12
        Me.txtAdjCheque_AMT.Name = "txtAdjCheque_AMT"
        Me.txtAdjCheque_AMT.Size = New System.Drawing.Size(111, 22)
        Me.txtAdjCheque_AMT.TabIndex = 17
        Me.txtAdjCheque_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjCreditCard_AMT
        '
        Me.txtAdjCreditCard_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCreditCard_AMT.Location = New System.Drawing.Point(127, 184)
        Me.txtAdjCreditCard_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCreditCard_AMT.MaxLength = 12
        Me.txtAdjCreditCard_AMT.Name = "txtAdjCreditCard_AMT"
        Me.txtAdjCreditCard_AMT.Size = New System.Drawing.Size(111, 22)
        Me.txtAdjCreditCard_AMT.TabIndex = 20
        Me.txtAdjCreditCard_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmbRecPay_MAN
        '
        Me.cmbRecPay_MAN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbRecPay_MAN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbRecPay_MAN.FormattingEnabled = True
        Me.cmbRecPay_MAN.Location = New System.Drawing.Point(-4, -8)
        Me.cmbRecPay_MAN.Name = "cmbRecPay_MAN"
        Me.cmbRecPay_MAN.Size = New System.Drawing.Size(214, 21)
        Me.cmbRecPay_MAN.TabIndex = 3
        Me.cmbRecPay_MAN.Visible = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AdvanceToolStripMenuItem, Me.ChequeToolStripMenuItem, Me.CreditCardToolStripMenuItem, Me.HandlingChargeToolStripMenuItem, Me.DiscountToolStripMenuItem, Me.CashToolStripMenuItem, Me.tStripGiftVouhcer, Me.NewToolStripMenuItem, Me.SchemeToolStripMenuItem, Me.CreditToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(184, 224)
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
        'SchemeToolStripMenuItem
        '
        Me.SchemeToolStripMenuItem.Name = "SchemeToolStripMenuItem"
        Me.SchemeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9
        Me.SchemeToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.SchemeToolStripMenuItem.Text = "Scheme"
        '
        'CreditToolStripMenuItem
        '
        Me.CreditToolStripMenuItem.Name = "CreditToolStripMenuItem"
        Me.CreditToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11
        Me.CreditToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.CreditToolStripMenuItem.Text = "Credit"
        '
        'frmEstPay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(589, 325)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAdj)
        Me.Controls.Add(Me.cmbRecPay_MAN)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmEstPay"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cash Point"
        Me.grpAdj.ResumeLayout(False)
        Me.grpAdj.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpAdj As CodeVendor.Controls.Grouper
    Friend WithEvents txtAdjGiftVoucher_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents txtAdjCredit_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents txtAdjDiscount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjAdvance_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents txtAdjChitCard_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents txtAdjHandlingCharge_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjRoundoff_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents Label148 As System.Windows.Forms.Label
    Friend WithEvents txtAdjCash_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents txtAdjReceive_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents txtAdjCheque_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjCreditCard_AMT As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents txtAdjIndDiscount As System.Windows.Forms.TextBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AdvanceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChequeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CreditCardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HandlingChargeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DiscountToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CashToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripGiftVouhcer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SchemeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CreditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbRecPay_MAN As System.Windows.Forms.ComboBox
End Class
