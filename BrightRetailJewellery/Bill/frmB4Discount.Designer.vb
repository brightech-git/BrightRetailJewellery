<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmB4Discount
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
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtDiscgm = New System.Windows.Forms.TextBox
        Me.Grsamt = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtSaDiscper = New System.Windows.Forms.TextBox
        Me.LBLAmountLabel = New System.Windows.Forms.Label
        Me.txtSADiscount_AMT = New System.Windows.Forms.TextBox
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
        Me.grpDiscount.Controls.Add(Me.Label2)
        Me.grpDiscount.Controls.Add(Me.txtDiscgm)
        Me.grpDiscount.Controls.Add(Me.Grsamt)
        Me.grpDiscount.Controls.Add(Me.Label1)
        Me.grpDiscount.Controls.Add(Me.txtSaDiscper)
        Me.grpDiscount.Controls.Add(Me.LBLAmountLabel)
        Me.grpDiscount.Controls.Add(Me.txtSADiscount_AMT)
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
        Me.grpDiscount.Size = New System.Drawing.Size(201, 92)
        Me.grpDiscount.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(9, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 14)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Disc./Gm"
        '
        'txtDiscgm
        '
        Me.txtDiscgm.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscgm.Location = New System.Drawing.Point(80, 16)
        Me.txtDiscgm.Name = "txtDiscgm"
        Me.txtDiscgm.Size = New System.Drawing.Size(112, 22)
        Me.txtDiscgm.TabIndex = 1
        '
        'Grsamt
        '
        Me.Grsamt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Grsamt.Location = New System.Drawing.Point(0, -5)
        Me.Grsamt.Name = "Grsamt"
        Me.Grsamt.Size = New System.Drawing.Size(37, 22)
        Me.Grsamt.TabIndex = 4
        Me.Grsamt.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 46)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 14)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Disc. %"
        '
        'txtSaDiscper
        '
        Me.txtSaDiscper.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSaDiscper.Location = New System.Drawing.Point(80, 42)
        Me.txtSaDiscper.Name = "txtSaDiscper"
        Me.txtSaDiscper.Size = New System.Drawing.Size(112, 22)
        Me.txtSaDiscper.TabIndex = 3
        '
        'LBLAmountLabel
        '
        Me.LBLAmountLabel.AutoSize = True
        Me.LBLAmountLabel.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LBLAmountLabel.Location = New System.Drawing.Point(9, 70)
        Me.LBLAmountLabel.Name = "LBLAmountLabel"
        Me.LBLAmountLabel.Size = New System.Drawing.Size(64, 14)
        Me.LBLAmountLabel.TabIndex = 4
        Me.LBLAmountLabel.Text = "Discount"
        '
        'txtSADiscount_AMT
        '
        Me.txtSADiscount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSADiscount_AMT.Location = New System.Drawing.Point(80, 66)
        Me.txtSADiscount_AMT.Name = "txtSADiscount_AMT"
        Me.txtSADiscount_AMT.Size = New System.Drawing.Size(112, 22)
        Me.txtSADiscount_AMT.TabIndex = 5
        '
        'frmB4Discount
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Highlight
        Me.ClientSize = New System.Drawing.Size(210, 91)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpDiscount)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmB4Discount"
        Me.Text = "Discount"
        Me.grpDiscount.ResumeLayout(False)
        Me.grpDiscount.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpDiscount As CodeVendor.Controls.Grouper
    Friend WithEvents LBLAmountLabel As System.Windows.Forms.Label
    Friend WithEvents txtSADiscount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtSaDiscper As System.Windows.Forms.TextBox
    Friend WithEvents Grsamt As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtDiscgm As System.Windows.Forms.TextBox
End Class
