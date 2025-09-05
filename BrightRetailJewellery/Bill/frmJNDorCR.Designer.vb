<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmJNDorCR
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
        Me.rbtBoth = New System.Windows.Forms.RadioButton
        Me.rbtCredit = New System.Windows.Forms.RadioButton
        Me.rbtJnd = New System.Windows.Forms.RadioButton
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
        Me.grpDiscount.Controls.Add(Me.rbtJnd)
        Me.grpDiscount.Controls.Add(Me.rbtCredit)
        Me.grpDiscount.Controls.Add(Me.rbtBoth)
        Me.grpDiscount.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpDiscount.GroupImage = Nothing
        Me.grpDiscount.GroupTitle = ""
        Me.grpDiscount.Location = New System.Drawing.Point(6, -5)
        Me.grpDiscount.Name = "grpDiscount"
        Me.grpDiscount.Padding = New System.Windows.Forms.Padding(23, 20, 23, 20)
        Me.grpDiscount.PaintGroupBox = False
        Me.grpDiscount.RoundCorners = 10
        Me.grpDiscount.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpDiscount.ShadowControl = False
        Me.grpDiscount.ShadowThickness = 3
        Me.grpDiscount.Size = New System.Drawing.Size(386, 54)
        Me.grpDiscount.TabIndex = 1
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Checked = True
        Me.rbtBoth.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtBoth.Location = New System.Drawing.Point(300, 23)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(60, 20)
        Me.rbtBoth.TabIndex = 1
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'rbtCredit
        '
        Me.rbtCredit.AutoSize = True
        Me.rbtCredit.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtCredit.Location = New System.Drawing.Point(26, 23)
        Me.rbtCredit.Name = "rbtCredit"
        Me.rbtCredit.Size = New System.Drawing.Size(133, 20)
        Me.rbtCredit.TabIndex = 2
        Me.rbtCredit.Text = "Credit Bill Only"
        Me.rbtCredit.UseVisualStyleBackColor = True
        '
        'rbtJnd
        '
        Me.rbtJnd.AutoSize = True
        Me.rbtJnd.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtJnd.Location = New System.Drawing.Point(181, 23)
        Me.rbtJnd.Name = "rbtJnd"
        Me.rbtJnd.Size = New System.Drawing.Size(90, 20)
        Me.rbtJnd.TabIndex = 3
        Me.rbtJnd.Text = "JND Only"
        Me.rbtJnd.UseVisualStyleBackColor = True
        '
        'frmJNDorCR
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(394, 54)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpDiscount)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "frmJNDorCR"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Adjustment Bill for"
        Me.grpDiscount.ResumeLayout(False)
        Me.grpDiscount.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpDiscount As CodeVendor.Controls.Grouper
    Friend WithEvents rbtJnd As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCredit As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
End Class
