<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreditCardAdj
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
        Me.grpCreditCard = New CodeVendor.Controls.Grouper
        Me.pnlCurrecny_OWN = New System.Windows.Forms.Panel
        Me.txtCrAmount_AMT = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtCr_AMT = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtCrCon_AMT = New System.Windows.Forms.TextBox
        Me.txtCrRemark = New System.Windows.Forms.TextBox
        Me.txtCreditCardRowIndex = New System.Windows.Forms.TextBox
        Me.txtCreditCardComm = New System.Windows.Forms.TextBox
        Me.gridCreditCardTotal = New System.Windows.Forms.DataGridView
        Me.gridCreditCard = New System.Windows.Forms.DataGridView
        Me.cmbCreditCardType = New System.Windows.Forms.ComboBox
        Me.Label124 = New System.Windows.Forms.Label
        Me.pnlCard_OWN = New System.Windows.Forms.Panel
        Me.dtpCreditCardDate = New BrighttechPack.DatePicker(Me.components)
        Me.txtCreditCardAmount_AMT = New System.Windows.Forms.TextBox
        Me.Label125 = New System.Windows.Forms.Label
        Me.Label126 = New System.Windows.Forms.Label
        Me.Label127 = New System.Windows.Forms.Label
        Me.Label128 = New System.Windows.Forms.Label
        Me.txtCreditCardNo = New System.Windows.Forms.TextBox
        Me.txtCreditCardAprovalNo = New System.Windows.Forms.TextBox
        Me.grpCreditCard.SuspendLayout()
        Me.pnlCurrecny_OWN.SuspendLayout()
        CType(Me.gridCreditCardTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridCreditCard, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlCard_OWN.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpCreditCard
        '
        Me.grpCreditCard.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpCreditCard.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpCreditCard.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpCreditCard.BorderColor = System.Drawing.Color.Transparent
        Me.grpCreditCard.BorderThickness = 1.0!
        Me.grpCreditCard.Controls.Add(Me.pnlCurrecny_OWN)
        Me.grpCreditCard.Controls.Add(Me.txtCreditCardRowIndex)
        Me.grpCreditCard.Controls.Add(Me.txtCreditCardComm)
        Me.grpCreditCard.Controls.Add(Me.gridCreditCardTotal)
        Me.grpCreditCard.Controls.Add(Me.gridCreditCard)
        Me.grpCreditCard.Controls.Add(Me.cmbCreditCardType)
        Me.grpCreditCard.Controls.Add(Me.Label124)
        Me.grpCreditCard.Controls.Add(Me.pnlCard_OWN)
        Me.grpCreditCard.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpCreditCard.GroupImage = Nothing
        Me.grpCreditCard.GroupTitle = ""
        Me.grpCreditCard.Location = New System.Drawing.Point(0, -11)
        Me.grpCreditCard.Name = "grpCreditCard"
        Me.grpCreditCard.Padding = New System.Windows.Forms.Padding(20)
        Me.grpCreditCard.PaintGroupBox = False
        Me.grpCreditCard.RoundCorners = 10
        Me.grpCreditCard.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpCreditCard.ShadowControl = False
        Me.grpCreditCard.ShadowThickness = 3
        Me.grpCreditCard.Size = New System.Drawing.Size(630, 198)
        Me.grpCreditCard.TabIndex = 0
        '
        'pnlCurrecny_OWN
        '
        Me.pnlCurrecny_OWN.Controls.Add(Me.txtCrAmount_AMT)
        Me.pnlCurrecny_OWN.Controls.Add(Me.Label1)
        Me.pnlCurrecny_OWN.Controls.Add(Me.txtCr_AMT)
        Me.pnlCurrecny_OWN.Controls.Add(Me.Label2)
        Me.pnlCurrecny_OWN.Controls.Add(Me.Label3)
        Me.pnlCurrecny_OWN.Controls.Add(Me.Label4)
        Me.pnlCurrecny_OWN.Controls.Add(Me.txtCrCon_AMT)
        Me.pnlCurrecny_OWN.Controls.Add(Me.txtCrRemark)
        Me.pnlCurrecny_OWN.Location = New System.Drawing.Point(182, 56)
        Me.pnlCurrecny_OWN.Name = "pnlCurrecny_OWN"
        Me.pnlCurrecny_OWN.Size = New System.Drawing.Size(424, 46)
        Me.pnlCurrecny_OWN.TabIndex = 3
        Me.pnlCurrecny_OWN.Visible = False
        '
        'txtCrAmount_AMT
        '
        Me.txtCrAmount_AMT.BackColor = System.Drawing.SystemColors.Window
        Me.txtCrAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCrAmount_AMT.Location = New System.Drawing.Point(171, 24)
        Me.txtCrAmount_AMT.MaxLength = 12
        Me.txtCrAmount_AMT.Name = "txtCrAmount_AMT"
        Me.txtCrAmount_AMT.ReadOnly = True
        Me.txtCrAmount_AMT.Size = New System.Drawing.Size(112, 22)
        Me.txtCrAmount_AMT.TabIndex = 5
        Me.txtCrAmount_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(171, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(112, 17)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "AMOUNT"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtCr_AMT
        '
        Me.txtCr_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCr_AMT.Location = New System.Drawing.Point(2, 24)
        Me.txtCr_AMT.MaxLength = 12
        Me.txtCr_AMT.Name = "txtCr_AMT"
        Me.txtCr_AMT.Size = New System.Drawing.Size(86, 22)
        Me.txtCr_AMT.TabIndex = 1
        Me.txtCr_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(3, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 17)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "CUR AMT"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(89, 6)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(81, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "CURRENCY"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(284, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(136, 17)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "REMARK"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtCrCon_AMT
        '
        Me.txtCrCon_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCrCon_AMT.Location = New System.Drawing.Point(89, 24)
        Me.txtCrCon_AMT.MaxLength = 20
        Me.txtCrCon_AMT.Name = "txtCrCon_AMT"
        Me.txtCrCon_AMT.Size = New System.Drawing.Size(81, 22)
        Me.txtCrCon_AMT.TabIndex = 3
        Me.txtCrCon_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCrRemark
        '
        Me.txtCrRemark.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCrRemark.Location = New System.Drawing.Point(284, 24)
        Me.txtCrRemark.MaxLength = 8
        Me.txtCrRemark.Name = "txtCrRemark"
        Me.txtCrRemark.Size = New System.Drawing.Size(136, 22)
        Me.txtCrRemark.TabIndex = 7
        Me.txtCrRemark.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCreditCardRowIndex
        '
        Me.txtCreditCardRowIndex.Location = New System.Drawing.Point(276, 113)
        Me.txtCreditCardRowIndex.Name = "txtCreditCardRowIndex"
        Me.txtCreditCardRowIndex.Size = New System.Drawing.Size(14, 21)
        Me.txtCreditCardRowIndex.TabIndex = 24
        Me.txtCreditCardRowIndex.Visible = False
        '
        'txtCreditCardComm
        '
        Me.txtCreditCardComm.Location = New System.Drawing.Point(430, 99)
        Me.txtCreditCardComm.Name = "txtCreditCardComm"
        Me.txtCreditCardComm.Size = New System.Drawing.Size(18, 21)
        Me.txtCreditCardComm.TabIndex = 23
        Me.txtCreditCardComm.Visible = False
        '
        'gridCreditCardTotal
        '
        Me.gridCreditCardTotal.AllowUserToAddRows = False
        Me.gridCreditCardTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridCreditCardTotal.ColumnHeadersVisible = False
        Me.gridCreditCardTotal.Location = New System.Drawing.Point(8, 171)
        Me.gridCreditCardTotal.MultiSelect = False
        Me.gridCreditCardTotal.Name = "gridCreditCardTotal"
        Me.gridCreditCardTotal.ReadOnly = True
        Me.gridCreditCardTotal.RowHeadersVisible = False
        Me.gridCreditCardTotal.RowTemplate.Height = 20
        Me.gridCreditCardTotal.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridCreditCardTotal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridCreditCardTotal.Size = New System.Drawing.Size(615, 19)
        Me.gridCreditCardTotal.TabIndex = 5
        '
        'gridCreditCard
        '
        Me.gridCreditCard.AllowUserToAddRows = False
        Me.gridCreditCard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridCreditCard.ColumnHeadersVisible = False
        Me.gridCreditCard.Location = New System.Drawing.Point(8, 56)
        Me.gridCreditCard.MultiSelect = False
        Me.gridCreditCard.Name = "gridCreditCard"
        Me.gridCreditCard.ReadOnly = True
        Me.gridCreditCard.RowHeadersVisible = False
        Me.gridCreditCard.RowTemplate.Height = 20
        Me.gridCreditCard.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridCreditCard.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridCreditCard.Size = New System.Drawing.Size(615, 115)
        Me.gridCreditCard.TabIndex = 4
        '
        'cmbCreditCardType
        '
        Me.cmbCreditCardType.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCreditCardType.FormattingEnabled = True
        Me.cmbCreditCardType.Location = New System.Drawing.Point(8, 33)
        Me.cmbCreditCardType.Name = "cmbCreditCardType"
        Me.cmbCreditCardType.Size = New System.Drawing.Size(174, 22)
        Me.cmbCreditCardType.TabIndex = 1
        '
        'Label124
        '
        Me.Label124.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label124.Location = New System.Drawing.Point(8, 14)
        Me.Label124.Name = "Label124"
        Me.Label124.Size = New System.Drawing.Size(174, 17)
        Me.Label124.TabIndex = 0
        Me.Label124.Text = "CARD TYPE"
        Me.Label124.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlCard_OWN
        '
        Me.pnlCard_OWN.Controls.Add(Me.dtpCreditCardDate)
        Me.pnlCard_OWN.Controls.Add(Me.txtCreditCardAmount_AMT)
        Me.pnlCard_OWN.Controls.Add(Me.Label125)
        Me.pnlCard_OWN.Controls.Add(Me.Label126)
        Me.pnlCard_OWN.Controls.Add(Me.Label127)
        Me.pnlCard_OWN.Controls.Add(Me.Label128)
        Me.pnlCard_OWN.Controls.Add(Me.txtCreditCardNo)
        Me.pnlCard_OWN.Controls.Add(Me.txtCreditCardAprovalNo)
        Me.pnlCard_OWN.Location = New System.Drawing.Point(182, 9)
        Me.pnlCard_OWN.Name = "pnlCard_OWN"
        Me.pnlCard_OWN.Size = New System.Drawing.Size(424, 46)
        Me.pnlCard_OWN.TabIndex = 2
        '
        'dtpCreditCardDate
        '
        Me.dtpCreditCardDate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.dtpCreditCardDate.Location = New System.Drawing.Point(2, 24)
        Me.dtpCreditCardDate.Mask = "##/##/####"
        Me.dtpCreditCardDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpCreditCardDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpCreditCardDate.Name = "dtpCreditCardDate"
        Me.dtpCreditCardDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpCreditCardDate.Size = New System.Drawing.Size(112, 22)
        Me.dtpCreditCardDate.TabIndex = 1
        Me.dtpCreditCardDate.Text = "07/03/9998"
        Me.dtpCreditCardDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'txtCreditCardAmount_AMT
        '
        Me.txtCreditCardAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCreditCardAmount_AMT.Location = New System.Drawing.Point(115, 24)
        Me.txtCreditCardAmount_AMT.MaxLength = 12
        Me.txtCreditCardAmount_AMT.Name = "txtCreditCardAmount_AMT"
        Me.txtCreditCardAmount_AMT.Size = New System.Drawing.Size(86, 22)
        Me.txtCreditCardAmount_AMT.TabIndex = 3
        Me.txtCreditCardAmount_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label125
        '
        Me.Label125.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label125.Location = New System.Drawing.Point(2, 5)
        Me.Label125.Name = "Label125"
        Me.Label125.Size = New System.Drawing.Size(112, 17)
        Me.Label125.TabIndex = 0
        Me.Label125.Text = "DATE"
        Me.Label125.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label126
        '
        Me.Label126.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label126.Location = New System.Drawing.Point(115, 5)
        Me.Label126.Name = "Label126"
        Me.Label126.Size = New System.Drawing.Size(86, 17)
        Me.Label126.TabIndex = 2
        Me.Label126.Text = "AMOUNT"
        Me.Label126.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label127
        '
        Me.Label127.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label127.Location = New System.Drawing.Point(202, 5)
        Me.Label127.Name = "Label127"
        Me.Label127.Size = New System.Drawing.Size(149, 17)
        Me.Label127.TabIndex = 4
        Me.Label127.Text = "CARD NO"
        Me.Label127.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label128
        '
        Me.Label128.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label128.Location = New System.Drawing.Point(352, 5)
        Me.Label128.Name = "Label128"
        Me.Label128.Size = New System.Drawing.Size(70, 17)
        Me.Label128.TabIndex = 6
        Me.Label128.Text = "APR NO"
        Me.Label128.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtCreditCardNo
        '
        Me.txtCreditCardNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCreditCardNo.Location = New System.Drawing.Point(202, 24)
        Me.txtCreditCardNo.MaxLength = 20
        Me.txtCreditCardNo.Name = "txtCreditCardNo"
        Me.txtCreditCardNo.Size = New System.Drawing.Size(149, 22)
        Me.txtCreditCardNo.TabIndex = 5
        Me.txtCreditCardNo.Text = "12345678901234567890"
        Me.txtCreditCardNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCreditCardAprovalNo
        '
        Me.txtCreditCardAprovalNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCreditCardAprovalNo.Location = New System.Drawing.Point(352, 24)
        Me.txtCreditCardAprovalNo.MaxLength = 8
        Me.txtCreditCardAprovalNo.Name = "txtCreditCardAprovalNo"
        Me.txtCreditCardAprovalNo.Size = New System.Drawing.Size(70, 22)
        Me.txtCreditCardAprovalNo.TabIndex = 7
        Me.txtCreditCardAprovalNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmCreditCardAdj
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(633, 190)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpCreditCard)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmCreditCardAdj"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "[Adjustment] Credit Card"
        Me.grpCreditCard.ResumeLayout(False)
        Me.grpCreditCard.PerformLayout()
        Me.pnlCurrecny_OWN.ResumeLayout(False)
        Me.pnlCurrecny_OWN.PerformLayout()
        CType(Me.gridCreditCardTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridCreditCard, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlCard_OWN.ResumeLayout(False)
        Me.pnlCard_OWN.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpCreditCard As CodeVendor.Controls.Grouper
    Friend WithEvents txtCreditCardRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents txtCreditCardComm As System.Windows.Forms.TextBox
    Friend WithEvents gridCreditCardTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridCreditCard As System.Windows.Forms.DataGridView
    Friend WithEvents cmbCreditCardType As System.Windows.Forms.ComboBox
    Friend WithEvents txtCreditCardAprovalNo As System.Windows.Forms.TextBox
    Friend WithEvents txtCreditCardNo As System.Windows.Forms.TextBox
    Friend WithEvents txtCreditCardAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label128 As System.Windows.Forms.Label
    Friend WithEvents Label127 As System.Windows.Forms.Label
    Friend WithEvents Label126 As System.Windows.Forms.Label
    Friend WithEvents Label125 As System.Windows.Forms.Label
    Friend WithEvents Label124 As System.Windows.Forms.Label
    Friend WithEvents dtpCreditCardDate As BrighttechPack.DatePicker
    Friend WithEvents pnlCard_OWN As System.Windows.Forms.Panel
    Friend WithEvents pnlCurrecny_OWN As System.Windows.Forms.Panel
    Friend WithEvents txtCr_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtCrCon_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtCrRemark As System.Windows.Forms.TextBox
    Friend WithEvents txtCrAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
