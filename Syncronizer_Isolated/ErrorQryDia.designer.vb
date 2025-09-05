<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ErrorQryDia
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
        Me.txtErr = New System.Windows.Forms.TextBox
        Me.txtOrginal = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'txtErr
        '
        Me.txtErr.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtErr.Location = New System.Drawing.Point(0, 136)
        Me.txtErr.Multiline = True
        Me.txtErr.Name = "txtErr"
        Me.txtErr.Size = New System.Drawing.Size(920, 121)
        Me.txtErr.TabIndex = 0
        Me.txtErr.Text = "Replaced"
        '
        'txtOrginal
        '
        Me.txtOrginal.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtOrginal.Location = New System.Drawing.Point(0, 13)
        Me.txtOrginal.Multiline = True
        Me.txtOrginal.Name = "txtOrginal"
        Me.txtOrginal.Size = New System.Drawing.Size(920, 110)
        Me.txtOrginal.TabIndex = 1
        Me.txtOrginal.Text = "Original"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Original"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Location = New System.Drawing.Point(0, 123)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Replaced"
        '
        'ErrorQryDia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(920, 257)
        Me.Controls.Add(Me.txtErr)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtOrginal)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "ErrorQryDia"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sync Error"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtErr As System.Windows.Forms.TextBox
    Friend WithEvents txtOrginal As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
