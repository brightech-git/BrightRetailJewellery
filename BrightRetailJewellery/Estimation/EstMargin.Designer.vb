<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EstMargin
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
        Me.grpEstMargin = New CodeVendor.Controls.Grouper
        Me.cmbVaMargin_OWN = New System.Windows.Forms.ComboBox
        Me.grpEstMargin.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpEstMargin
        '
        Me.grpEstMargin.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpEstMargin.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpEstMargin.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpEstMargin.BorderColor = System.Drawing.Color.Empty
        Me.grpEstMargin.BorderThickness = 1.0!
        Me.grpEstMargin.Controls.Add(Me.cmbVaMargin_OWN)
        Me.grpEstMargin.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpEstMargin.GroupImage = Nothing
        Me.grpEstMargin.GroupTitle = ""
        Me.grpEstMargin.Location = New System.Drawing.Point(7, -3)
        Me.grpEstMargin.Name = "grpEstMargin"
        Me.grpEstMargin.Padding = New System.Windows.Forms.Padding(20)
        Me.grpEstMargin.PaintGroupBox = False
        Me.grpEstMargin.RoundCorners = 10
        Me.grpEstMargin.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpEstMargin.ShadowControl = False
        Me.grpEstMargin.ShadowThickness = 3
        Me.grpEstMargin.Size = New System.Drawing.Size(170, 172)
        Me.grpEstMargin.TabIndex = 1
        '
        'cmbVaMargin_OWN
        '
        Me.cmbVaMargin_OWN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cmbVaMargin_OWN.FormattingEnabled = True
        Me.cmbVaMargin_OWN.Location = New System.Drawing.Point(5, 19)
        Me.cmbVaMargin_OWN.Name = "cmbVaMargin_OWN"
        Me.cmbVaMargin_OWN.Size = New System.Drawing.Size(159, 146)
        Me.cmbVaMargin_OWN.TabIndex = 0
        '
        'EstMargin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(183, 176)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpEstMargin)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "EstMargin"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Margin Level"
        Me.grpEstMargin.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpEstMargin As CodeVendor.Controls.Grouper
    Friend WithEvents cmbVaMargin_OWN As System.Windows.Forms.ComboBox
End Class
