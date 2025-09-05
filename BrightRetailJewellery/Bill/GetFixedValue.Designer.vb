<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GetFixedValue
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
        Me.txtFixedValue = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'txtFixedValue
        '
        Me.txtFixedValue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtFixedValue.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFixedValue.Location = New System.Drawing.Point(0, 0)
        Me.txtFixedValue.Name = "txtFixedValue"
        Me.txtFixedValue.Size = New System.Drawing.Size(162, 26)
        Me.txtFixedValue.TabIndex = 0
        Me.txtFixedValue.Text = "59999999.99"
        '
        'GetFixedValue
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.ClientSize = New System.Drawing.Size(162, 24)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtFixedValue)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "GetFixedValue"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MRP Value"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtFixedValue As System.Windows.Forms.TextBox
End Class
