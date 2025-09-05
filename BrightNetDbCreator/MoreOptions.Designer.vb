<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MoreOptions
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
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.dudLimit = New System.Windows.Forms.DomainUpDown
        Me.chkLstModules = New System.Windows.Forms.CheckedListBox
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 156)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "User Limit"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Modules"
        '
        'dudLimit
        '
        Me.dudLimit.Location = New System.Drawing.Point(94, 153)
        Me.dudLimit.Name = "dudLimit"
        Me.dudLimit.Size = New System.Drawing.Size(59, 21)
        Me.dudLimit.TabIndex = 4
        Me.dudLimit.Text = "DomainUpDown1"
        '
        'chkLstModules
        '
        Me.chkLstModules.FormattingEnabled = True
        Me.chkLstModules.Items.AddRange(New Object() {"Stock", "Bill", "Estimation", "Order & Repair", "Accounts", "Stock Management", "Savings Scheme"})
        Me.chkLstModules.Location = New System.Drawing.Point(12, 31)
        Me.chkLstModules.Name = "chkLstModules"
        Me.chkLstModules.Size = New System.Drawing.Size(141, 116)
        Me.chkLstModules.TabIndex = 3
        '
        'MoreOptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(166, 187)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dudLimit)
        Me.Controls.Add(Me.chkLstModules)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "MoreOptions"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "MoreOptions"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dudLimit As System.Windows.Forms.DomainUpDown
    Friend WithEvents chkLstModules As System.Windows.Forms.CheckedListBox
End Class
