<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdvFixWtper
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
        Me.grpDiscount = New CodeVendor.Controls.Grouper
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtFixWt = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbWtper = New System.Windows.Forms.ComboBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me.LBLAmountLabel = New System.Windows.Forms.Label
        Me.txtRate = New System.Windows.Forms.TextBox
        Me.grpDiscount.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpDiscount
        '
        Me.grpDiscount.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpDiscount.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpDiscount.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpDiscount.BorderColor = System.Drawing.Color.Transparent
        Me.grpDiscount.BorderThickness = 1.0!
        Me.grpDiscount.Controls.Add(Me.Label3)
        Me.grpDiscount.Controls.Add(Me.txtFixWt)
        Me.grpDiscount.Controls.Add(Me.Label2)
        Me.grpDiscount.Controls.Add(Me.cmbWtper)
        Me.grpDiscount.Controls.Add(Me.btnCancel)
        Me.grpDiscount.Controls.Add(Me.btnOk)
        Me.grpDiscount.Controls.Add(Me.Label1)
        Me.grpDiscount.Controls.Add(Me.txtAmount)
        Me.grpDiscount.Controls.Add(Me.LBLAmountLabel)
        Me.grpDiscount.Controls.Add(Me.txtRate)
        Me.grpDiscount.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpDiscount.GroupImage = Nothing
        Me.grpDiscount.GroupTitle = ""
        Me.grpDiscount.Location = New System.Drawing.Point(4, -5)
        Me.grpDiscount.Name = "grpDiscount"
        Me.grpDiscount.Padding = New System.Windows.Forms.Padding(20)
        Me.grpDiscount.PaintGroupBox = False
        Me.grpDiscount.RoundCorners = 10
        Me.grpDiscount.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpDiscount.ShadowControl = False
        Me.grpDiscount.ShadowThickness = 3
        Me.grpDiscount.Size = New System.Drawing.Size(229, 183)
        Me.grpDiscount.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 98)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 14)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Fix Weight"
        '
        'txtFixWt
        '
        Me.txtFixWt.Enabled = False
        Me.txtFixWt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFixWt.Location = New System.Drawing.Point(105, 95)
        Me.txtFixWt.Name = "txtFixWt"
        Me.txtFixWt.Size = New System.Drawing.Size(112, 22)
        Me.txtFixWt.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(8, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 14)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Fix Weight %"
        '
        'cmbWtper
        '
        Me.cmbWtper.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbWtper.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbWtper.FormattingEnabled = True
        Me.cmbWtper.Location = New System.Drawing.Point(105, 68)
        Me.cmbWtper.Name = "cmbWtper"
        Me.cmbWtper.Size = New System.Drawing.Size(112, 21)
        Me.cmbWtper.TabIndex = 5
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(117, 141)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "Exit [F12]"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(11, 141)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 8
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 14)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Amount"
        '
        'txtAmount
        '
        Me.txtAmount.Enabled = False
        Me.txtAmount.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.Location = New System.Drawing.Point(105, 16)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.Size = New System.Drawing.Size(112, 22)
        Me.txtAmount.TabIndex = 1
        '
        'LBLAmountLabel
        '
        Me.LBLAmountLabel.AutoSize = True
        Me.LBLAmountLabel.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LBLAmountLabel.Location = New System.Drawing.Point(8, 44)
        Me.LBLAmountLabel.Name = "LBLAmountLabel"
        Me.LBLAmountLabel.Size = New System.Drawing.Size(37, 14)
        Me.LBLAmountLabel.TabIndex = 2
        Me.LBLAmountLabel.Text = "Rate"
        '
        'txtRate
        '
        Me.txtRate.Enabled = False
        Me.txtRate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRate.Location = New System.Drawing.Point(105, 41)
        Me.txtRate.Name = "txtRate"
        Me.txtRate.Size = New System.Drawing.Size(112, 22)
        Me.txtRate.TabIndex = 3
        '
        'frmAdvFixWtper
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(232, 179)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpDiscount)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmAdvFixWtper"
        Me.Text = "Weight Fix Details"
        Me.grpDiscount.ResumeLayout(False)
        Me.grpDiscount.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpDiscount As CodeVendor.Controls.Grouper
    Friend WithEvents LBLAmountLabel As System.Windows.Forms.Label
    Friend WithEvents txtRate As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtAmount As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbWtper As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtFixWt As System.Windows.Forms.TextBox
End Class
