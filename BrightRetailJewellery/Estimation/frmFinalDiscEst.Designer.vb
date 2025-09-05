<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFinalDiscEst
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
        Me.fraVbcTax = New System.Windows.Forms.GroupBox
        Me.rbtVbcYes = New System.Windows.Forms.RadioButton
        Me.rbtVbcNo = New System.Windows.Forms.RadioButton
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtNet_AMT = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtExcise_AMT = New System.Windows.Forms.TextBox
        Me.Grsamt = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtGrs_Amt = New System.Windows.Forms.TextBox
        Me.LBLAmountLabel = New System.Windows.Forms.Label
        Me.txtVat_AMT = New System.Windows.Forms.TextBox
        Me.grpDiscount.SuspendLayout()
        Me.fraVbcTax.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpDiscount
        '
        Me.grpDiscount.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpDiscount.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpDiscount.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpDiscount.BorderColor = System.Drawing.Color.Transparent
        Me.grpDiscount.BorderThickness = 1.0!
        Me.grpDiscount.Controls.Add(Me.fraVbcTax)
        Me.grpDiscount.Controls.Add(Me.Label3)
        Me.grpDiscount.Controls.Add(Me.txtNet_AMT)
        Me.grpDiscount.Controls.Add(Me.Label2)
        Me.grpDiscount.Controls.Add(Me.txtExcise_AMT)
        Me.grpDiscount.Controls.Add(Me.Grsamt)
        Me.grpDiscount.Controls.Add(Me.Label1)
        Me.grpDiscount.Controls.Add(Me.txtGrs_Amt)
        Me.grpDiscount.Controls.Add(Me.LBLAmountLabel)
        Me.grpDiscount.Controls.Add(Me.txtVat_AMT)
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
        Me.grpDiscount.Size = New System.Drawing.Size(322, 149)
        Me.grpDiscount.TabIndex = 0
        '
        'fraVbcTax
        '
        Me.fraVbcTax.Controls.Add(Me.rbtVbcYes)
        Me.fraVbcTax.Controls.Add(Me.rbtVbcNo)
        Me.fraVbcTax.Controls.Add(Me.Label4)
        Me.fraVbcTax.Location = New System.Drawing.Point(16, 5)
        Me.fraVbcTax.Name = "fraVbcTax"
        Me.fraVbcTax.Size = New System.Drawing.Size(276, 29)
        Me.fraVbcTax.TabIndex = 0
        Me.fraVbcTax.TabStop = False
        Me.fraVbcTax.Visible = False
        '
        'rbtVbcYes
        '
        Me.rbtVbcYes.AutoSize = True
        Me.rbtVbcYes.Location = New System.Drawing.Point(230, 9)
        Me.rbtVbcYes.Name = "rbtVbcYes"
        Me.rbtVbcYes.Size = New System.Drawing.Size(45, 17)
        Me.rbtVbcYes.TabIndex = 2
        Me.rbtVbcYes.Text = "Yes"
        Me.rbtVbcYes.UseVisualStyleBackColor = True
        '
        'rbtVbcNo
        '
        Me.rbtVbcNo.AutoSize = True
        Me.rbtVbcNo.Checked = True
        Me.rbtVbcNo.Location = New System.Drawing.Point(164, 9)
        Me.rbtVbcNo.Name = "rbtVbcNo"
        Me.rbtVbcNo.Size = New System.Drawing.Size(40, 17)
        Me.rbtVbcNo.TabIndex = 1
        Me.rbtVbcNo.TabStop = True
        Me.rbtVbcNo.Text = "No"
        Me.rbtVbcNo.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(1, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(157, 14)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Vat Borne by Company"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(11, 125)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 14)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Net Amount"
        '
        'txtNet_AMT
        '
        Me.txtNet_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNet_AMT.Location = New System.Drawing.Point(178, 121)
        Me.txtNet_AMT.Name = "txtNet_AMT"
        Me.txtNet_AMT.Size = New System.Drawing.Size(112, 22)
        Me.txtNet_AMT.TabIndex = 8
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(11, 97)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(103, 14)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Excise Amount"
        '
        'txtExcise_AMT
        '
        Me.txtExcise_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExcise_AMT.Location = New System.Drawing.Point(178, 93)
        Me.txtExcise_AMT.Name = "txtExcise_AMT"
        Me.txtExcise_AMT.Size = New System.Drawing.Size(112, 22)
        Me.txtExcise_AMT.TabIndex = 6
        '
        'Grsamt
        '
        Me.Grsamt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Grsamt.Location = New System.Drawing.Point(0, -11)
        Me.Grsamt.Name = "Grsamt"
        Me.Grsamt.Size = New System.Drawing.Size(37, 22)
        Me.Grsamt.TabIndex = 0
        Me.Grsamt.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(11, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(99, 14)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Gross Amount"
        '
        'txtGrs_Amt
        '
        Me.txtGrs_Amt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGrs_Amt.Location = New System.Drawing.Point(178, 37)
        Me.txtGrs_Amt.Name = "txtGrs_Amt"
        Me.txtGrs_Amt.Size = New System.Drawing.Size(112, 22)
        Me.txtGrs_Amt.TabIndex = 2
        '
        'LBLAmountLabel
        '
        Me.LBLAmountLabel.AutoSize = True
        Me.LBLAmountLabel.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LBLAmountLabel.Location = New System.Drawing.Point(11, 69)
        Me.LBLAmountLabel.Name = "LBLAmountLabel"
        Me.LBLAmountLabel.Size = New System.Drawing.Size(33, 14)
        Me.LBLAmountLabel.TabIndex = 3
        Me.LBLAmountLabel.Text = "Vat "
        '
        'txtVat_AMT
        '
        Me.txtVat_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVat_AMT.Location = New System.Drawing.Point(178, 65)
        Me.txtVat_AMT.Name = "txtVat_AMT"
        Me.txtVat_AMT.Size = New System.Drawing.Size(112, 22)
        Me.txtVat_AMT.TabIndex = 4
        '
        'frmFinalDiscEst
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(338, 143)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpDiscount)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmFinalDiscEst"
        Me.Text = "Final Amount "
        Me.grpDiscount.ResumeLayout(False)
        Me.grpDiscount.PerformLayout()
        Me.fraVbcTax.ResumeLayout(False)
        Me.fraVbcTax.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpDiscount As CodeVendor.Controls.Grouper
    Friend WithEvents LBLAmountLabel As System.Windows.Forms.Label
    Friend WithEvents txtVat_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtGrs_Amt As System.Windows.Forms.TextBox
    Friend WithEvents Grsamt As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNet_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtExcise_AMT As System.Windows.Forms.TextBox
    Friend WithEvents fraVbcTax As System.Windows.Forms.GroupBox
    Friend WithEvents rbtVbcYes As System.Windows.Forms.RadioButton
    Friend WithEvents rbtVbcNo As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
