<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MaterialIssRecRateChangeBox
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtNewRate_RATE = New System.Windows.Forms.TextBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.btnSend = New System.Windows.Forms.Button()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblResend = New System.Windows.Forms.LinkLabel()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.txtNewRateInclusive_RATE = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbtUnFixed = New System.Windows.Forms.RadioButton()
        Me.rbtFixed = New System.Windows.Forms.RadioButton()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtNewRate_RATE
        '
        Me.txtNewRate_RATE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNewRate_RATE.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewRate_RATE.Location = New System.Drawing.Point(196, 76)
        Me.txtNewRate_RATE.MaxLength = 20
        Me.txtNewRate_RATE.Name = "txtNewRate_RATE"
        Me.txtNewRate_RATE.Size = New System.Drawing.Size(228, 26)
        Me.txtNewRate_RATE.TabIndex = 5
        '
        'txtPassword
        '
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.Location = New System.Drawing.Point(196, 144)
        Me.txtPassword.MaxLength = 20
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.ReadOnly = True
        Me.txtPassword.Size = New System.Drawing.Size(176, 26)
        Me.txtPassword.TabIndex = 8
        Me.txtPassword.Visible = False
        '
        'btnSend
        '
        Me.btnSend.Location = New System.Drawing.Point(197, 108)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(136, 30)
        Me.btnSend.TabIndex = 6
        Me.btnSend.Text = "OK"
        Me.btnSend.UseVisualStyleBackColor = True
        '
        'Label34
        '
        Me.Label34.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.Location = New System.Drawing.Point(39, 81)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(151, 14)
        Me.Label34.TabIndex = 4
        Me.Label34.Text = "Rate"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(436, 140)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(78, 30)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        Me.btnCancel.Visible = False
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(75, 149)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 14)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "OTP"
        Me.Label1.Visible = False
        '
        'lblResend
        '
        Me.lblResend.AutoSize = True
        Me.lblResend.Location = New System.Drawing.Point(296, 173)
        Me.lblResend.Name = "lblResend"
        Me.lblResend.Size = New System.Drawing.Size(76, 13)
        Me.lblResend.TabIndex = 11
        Me.lblResend.TabStop = True
        Me.lblResend.Text = "Resend OTP"
        Me.lblResend.Visible = False
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(376, 140)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(54, 30)
        Me.btnOk.TabIndex = 9
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        Me.btnOk.Visible = False
        '
        'txtNewRateInclusive_RATE
        '
        Me.txtNewRateInclusive_RATE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNewRateInclusive_RATE.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewRateInclusive_RATE.Location = New System.Drawing.Point(196, 45)
        Me.txtNewRateInclusive_RATE.MaxLength = 20
        Me.txtNewRateInclusive_RATE.Name = "txtNewRateInclusive_RATE"
        Me.txtNewRateInclusive_RATE.Size = New System.Drawing.Size(228, 26)
        Me.txtNewRateInclusive_RATE.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(39, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(151, 21)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Rate (GST Inclusive)"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtUnFixed)
        Me.GroupBox1.Controls.Add(Me.rbtFixed)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtNewRate_RATE)
        Me.GroupBox1.Controls.Add(Me.txtNewRateInclusive_RATE)
        Me.GroupBox1.Controls.Add(Me.txtPassword)
        Me.GroupBox1.Controls.Add(Me.btnOk)
        Me.GroupBox1.Controls.Add(Me.btnSend)
        Me.GroupBox1.Controls.Add(Me.lblResend)
        Me.GroupBox1.Controls.Add(Me.Label34)
        Me.GroupBox1.Controls.Add(Me.btnCancel)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(434, 139)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "MI/MR Rate"
        '
        'rbtUnFixed
        '
        Me.rbtUnFixed.AutoSize = True
        Me.rbtUnFixed.Location = New System.Drawing.Point(258, 21)
        Me.rbtUnFixed.Name = "rbtUnFixed"
        Me.rbtUnFixed.Size = New System.Drawing.Size(70, 17)
        Me.rbtUnFixed.TabIndex = 1
        Me.rbtUnFixed.Text = "UnFixed"
        Me.rbtUnFixed.UseVisualStyleBackColor = True
        Me.rbtUnFixed.Visible = False
        '
        'rbtFixed
        '
        Me.rbtFixed.AutoSize = True
        Me.rbtFixed.Checked = True
        Me.rbtFixed.Location = New System.Drawing.Point(197, 21)
        Me.rbtFixed.Name = "rbtFixed"
        Me.rbtFixed.Size = New System.Drawing.Size(55, 17)
        Me.rbtFixed.TabIndex = 0
        Me.rbtFixed.TabStop = True
        Me.rbtFixed.Text = "Fixed"
        Me.rbtFixed.UseVisualStyleBackColor = True
        Me.rbtFixed.Visible = False
        '
        'MaterialIssRecRateChangeBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Lavender
        Me.ClientSize = New System.Drawing.Size(434, 139)
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Name = "MaterialIssRecRateChangeBox"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MI/MR FIXED RATE "
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtNewRate_RATE As System.Windows.Forms.TextBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblResend As System.Windows.Forms.LinkLabel
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents txtNewRateInclusive_RATE As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents rbtUnFixed As RadioButton
    Friend WithEvents rbtFixed As RadioButton
End Class
