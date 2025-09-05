<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOtherMast
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.CmbOtherName = New System.Windows.Forms.ComboBox
        Me.CmbOtherVal = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Misc Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Others Name"
        '
        'CmbOtherName
        '
        Me.CmbOtherName.FormattingEnabled = True
        Me.CmbOtherName.Location = New System.Drawing.Point(94, 31)
        Me.CmbOtherName.Name = "CmbOtherName"
        Me.CmbOtherName.Size = New System.Drawing.Size(223, 21)
        Me.CmbOtherName.TabIndex = 1
        '
        'CmbOtherVal
        '
        Me.CmbOtherVal.FormattingEnabled = True
        Me.CmbOtherVal.Location = New System.Drawing.Point(94, 61)
        Me.CmbOtherVal.Name = "CmbOtherVal"
        Me.CmbOtherVal.Size = New System.Drawing.Size(223, 21)
        Me.CmbOtherVal.TabIndex = 3
        '
        'frmOtherMast
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(327, 117)
        Me.ControlBox = False
        Me.Controls.Add(Me.CmbOtherVal)
        Me.Controls.Add(Me.CmbOtherName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmOtherMast"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Tage Other Info Details"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents CmbOtherName As System.Windows.Forms.ComboBox
    Friend WithEvents CmbOtherVal As System.Windows.Forms.ComboBox
End Class
