<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFundTransfer
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtAcNo_MAN = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtAcName_MAN = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtAcAddress_MAN = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtIFSCCode_MAN = New System.Windows.Forms.TextBox
        Me.txtBankName_MAN = New System.Windows.Forms.TextBox
        Me.txtBankAddress = New System.Windows.Forms.TextBox
        Me.txtBankBranch_MAN = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.rbtRTGS = New System.Windows.Forms.RadioButton
        Me.rbtCheque = New System.Windows.Forms.RadioButton
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.grpFundTransfer = New CodeVendor.Controls.Grouper
        Me.grpBank = New System.Windows.Forms.GroupBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.grpFundTransfer.SuspendLayout()
        Me.grpBank.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "A/c No"
        '
        'txtAcNo_MAN
        '
        Me.txtAcNo_MAN.Location = New System.Drawing.Point(6, 31)
        Me.txtAcNo_MAN.MaxLength = 30
        Me.txtAcNo_MAN.Name = "txtAcNo_MAN"
        Me.txtAcNo_MAN.Size = New System.Drawing.Size(252, 21)
        Me.txtAcNo_MAN.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "A/c Name"
        '
        'txtAcName_MAN
        '
        Me.txtAcName_MAN.Location = New System.Drawing.Point(6, 71)
        Me.txtAcName_MAN.MaxLength = 55
        Me.txtAcName_MAN.Name = "txtAcName_MAN"
        Me.txtAcName_MAN.Size = New System.Drawing.Size(252, 21)
        Me.txtAcName_MAN.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 95)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Address"
        '
        'txtAcAddress_MAN
        '
        Me.txtAcAddress_MAN.Location = New System.Drawing.Point(6, 111)
        Me.txtAcAddress_MAN.MaxLength = 300
        Me.txtAcAddress_MAN.Multiline = True
        Me.txtAcAddress_MAN.Name = "txtAcAddress_MAN"
        Me.txtAcAddress_MAN.Size = New System.Drawing.Size(252, 123)
        Me.txtAcAddress_MAN.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "IFSC Code"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 56)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(73, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Bank Name"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(7, 96)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(80, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Bank Branch"
        '
        'txtIFSCCode_MAN
        '
        Me.txtIFSCCode_MAN.Location = New System.Drawing.Point(7, 32)
        Me.txtIFSCCode_MAN.MaxLength = 20
        Me.txtIFSCCode_MAN.Name = "txtIFSCCode_MAN"
        Me.txtIFSCCode_MAN.Size = New System.Drawing.Size(252, 21)
        Me.txtIFSCCode_MAN.TabIndex = 1
        '
        'txtBankName_MAN
        '
        Me.txtBankName_MAN.Location = New System.Drawing.Point(7, 72)
        Me.txtBankName_MAN.MaxLength = 50
        Me.txtBankName_MAN.Name = "txtBankName_MAN"
        Me.txtBankName_MAN.Size = New System.Drawing.Size(252, 21)
        Me.txtBankName_MAN.TabIndex = 3
        '
        'txtBankAddress
        '
        Me.txtBankAddress.Location = New System.Drawing.Point(5, 152)
        Me.txtBankAddress.MaxLength = 300
        Me.txtBankAddress.Multiline = True
        Me.txtBankAddress.Name = "txtBankAddress"
        Me.txtBankAddress.Size = New System.Drawing.Size(252, 83)
        Me.txtBankAddress.TabIndex = 7
        '
        'txtBankBranch_MAN
        '
        Me.txtBankBranch_MAN.Location = New System.Drawing.Point(7, 112)
        Me.txtBankBranch_MAN.MaxLength = 50
        Me.txtBankBranch_MAN.Name = "txtBankBranch_MAN"
        Me.txtBankBranch_MAN.Size = New System.Drawing.Size(252, 21)
        Me.txtBankBranch_MAN.TabIndex = 5
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(5, 136)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(86, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Bank Address"
        '
        'rbtRTGS
        '
        Me.rbtRTGS.AutoSize = True
        Me.rbtRTGS.Location = New System.Drawing.Point(79, 40)
        Me.rbtRTGS.Name = "rbtRTGS"
        Me.rbtRTGS.Size = New System.Drawing.Size(57, 17)
        Me.rbtRTGS.TabIndex = 0
        Me.rbtRTGS.TabStop = True
        Me.rbtRTGS.Text = "RTGS"
        Me.rbtRTGS.UseVisualStyleBackColor = True
        '
        'rbtCheque
        '
        Me.rbtCheque.AutoSize = True
        Me.rbtCheque.Location = New System.Drawing.Point(153, 40)
        Me.rbtCheque.Name = "rbtCheque"
        Me.rbtCheque.Size = New System.Drawing.Size(69, 17)
        Me.rbtCheque.TabIndex = 1
        Me.rbtCheque.TabStop = True
        Me.rbtCheque.Text = "Cheque"
        Me.rbtCheque.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(341, 309)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 4
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(447, 309)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'grpFundTransfer
        '
        Me.grpFundTransfer.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpFundTransfer.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpFundTransfer.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpFundTransfer.BorderColor = System.Drawing.Color.Transparent
        Me.grpFundTransfer.BorderThickness = 1.0!
        Me.grpFundTransfer.Controls.Add(Me.grpBank)
        Me.grpFundTransfer.Controls.Add(Me.btnCancel)
        Me.grpFundTransfer.Controls.Add(Me.GroupBox1)
        Me.grpFundTransfer.Controls.Add(Me.btnOk)
        Me.grpFundTransfer.Controls.Add(Me.rbtCheque)
        Me.grpFundTransfer.Controls.Add(Me.rbtRTGS)
        Me.grpFundTransfer.CustomGroupBoxColor = System.Drawing.Color.Lavender
        Me.grpFundTransfer.GroupImage = Nothing
        Me.grpFundTransfer.GroupTitle = ""
        Me.grpFundTransfer.Location = New System.Drawing.Point(4, -5)
        Me.grpFundTransfer.Name = "grpFundTransfer"
        Me.grpFundTransfer.Padding = New System.Windows.Forms.Padding(20)
        Me.grpFundTransfer.PaintGroupBox = False
        Me.grpFundTransfer.RoundCorners = 10
        Me.grpFundTransfer.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpFundTransfer.ShadowControl = False
        Me.grpFundTransfer.ShadowThickness = 3
        Me.grpFundTransfer.Size = New System.Drawing.Size(553, 344)
        Me.grpFundTransfer.TabIndex = 0
        '
        'grpBank
        '
        Me.grpBank.Controls.Add(Me.Label4)
        Me.grpBank.Controls.Add(Me.txtIFSCCode_MAN)
        Me.grpBank.Controls.Add(Me.txtBankName_MAN)
        Me.grpBank.Controls.Add(Me.Label6)
        Me.grpBank.Controls.Add(Me.txtBankBranch_MAN)
        Me.grpBank.Controls.Add(Me.Label7)
        Me.grpBank.Controls.Add(Me.txtBankAddress)
        Me.grpBank.Controls.Add(Me.Label5)
        Me.grpBank.Location = New System.Drawing.Point(281, 63)
        Me.grpBank.Name = "grpBank"
        Me.grpBank.Size = New System.Drawing.Size(265, 240)
        Me.grpBank.TabIndex = 3
        Me.grpBank.TabStop = False
        Me.grpBank.Text = "Bank"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtAcName_MAN)
        Me.GroupBox1.Controls.Add(Me.txtAcNo_MAN)
        Me.GroupBox1.Controls.Add(Me.txtAcAddress_MAN)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 63)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(264, 240)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Customer"
        '
        'frmFundTransfer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(562, 343)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpFundTransfer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmFundTransfer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Fund Transfer"
        Me.grpFundTransfer.ResumeLayout(False)
        Me.grpFundTransfer.PerformLayout()
        Me.grpBank.ResumeLayout(False)
        Me.grpBank.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtAcNo_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtAcName_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtAcAddress_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtIFSCCode_MAN As System.Windows.Forms.TextBox
    Friend WithEvents txtBankName_MAN As System.Windows.Forms.TextBox
    Friend WithEvents txtBankAddress As System.Windows.Forms.TextBox
    Friend WithEvents txtBankBranch_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents rbtRTGS As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCheque As System.Windows.Forms.RadioButton
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents grpFundTransfer As CodeVendor.Controls.Grouper
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents grpBank As System.Windows.Forms.GroupBox
End Class
