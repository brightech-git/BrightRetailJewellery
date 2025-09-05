<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FindSubItem
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
        Me.grpAddress = New CodeVendor.Controls.Grouper
        Me.cmbSubItem_Man = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.grpAddress.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpAddress
        '
        Me.grpAddress.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpAddress.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpAddress.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpAddress.BorderColor = System.Drawing.Color.Transparent
        Me.grpAddress.BorderThickness = 1.0!
        Me.grpAddress.Controls.Add(Me.cmbSubItem_Man)
        Me.grpAddress.Controls.Add(Me.Label1)
        Me.grpAddress.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpAddress.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpAddress.GroupImage = Nothing
        Me.grpAddress.GroupTitle = ""
        Me.grpAddress.Location = New System.Drawing.Point(0, 0)
        Me.grpAddress.Name = "grpAddress"
        Me.grpAddress.Padding = New System.Windows.Forms.Padding(20)
        Me.grpAddress.PaintGroupBox = False
        Me.grpAddress.RoundCorners = 10
        Me.grpAddress.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpAddress.ShadowControl = False
        Me.grpAddress.ShadowThickness = 3
        Me.grpAddress.Size = New System.Drawing.Size(387, 107)
        Me.grpAddress.TabIndex = 0
        '
        'cmbSubItem_Man
        '
        Me.cmbSubItem_Man.FormattingEnabled = True
        Me.cmbSubItem_Man.Location = New System.Drawing.Point(126, 46)
        Me.cmbSubItem_Man.Name = "cmbSubItem_Man"
        Me.cmbSubItem_Man.Size = New System.Drawing.Size(238, 21)
        Me.cmbSubItem_Man.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(114, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "SubItem"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FindSubItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(387, 107)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAddress)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FindSubItem"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FindSubItem"
        Me.grpAddress.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpAddress As CodeVendor.Controls.Grouper
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbSubItem_Man As System.Windows.Forms.ComboBox
End Class
