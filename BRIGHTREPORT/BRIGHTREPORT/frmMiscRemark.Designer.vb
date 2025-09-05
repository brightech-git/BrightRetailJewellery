<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMiscRemark
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpRemark = New CodeVendor.Controls.Grouper()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.cmbRemark1_OWN = New System.Windows.Forms.ComboBox()
        Me.grpRemark.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Remark"
        '
        'grpRemark
        '
        Me.grpRemark.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpRemark.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpRemark.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpRemark.BorderColor = System.Drawing.Color.Transparent
        Me.grpRemark.BorderThickness = 1.0!
        Me.grpRemark.Controls.Add(Me.Label2)
        Me.grpRemark.Controls.Add(Me.btnSave)
        Me.grpRemark.Controls.Add(Me.cmbRemark1_OWN)
        Me.grpRemark.Controls.Add(Me.Label1)
        Me.grpRemark.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpRemark.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpRemark.GroupImage = Nothing
        Me.grpRemark.GroupTitle = ""
        Me.grpRemark.Location = New System.Drawing.Point(0, 0)
        Me.grpRemark.Name = "grpRemark"
        Me.grpRemark.Padding = New System.Windows.Forms.Padding(20)
        Me.grpRemark.PaintGroupBox = False
        Me.grpRemark.RoundCorners = 10
        Me.grpRemark.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpRemark.ShadowControl = False
        Me.grpRemark.ShadowThickness = 3
        Me.grpRemark.Size = New System.Drawing.Size(480, 113)
        Me.grpRemark.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(192, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(145, 14)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "(Press Escape to Exit)"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(71, 71)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 2
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'cmbRemark1_OWN
        '
        Me.cmbRemark1_OWN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cmbRemark1_OWN.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold)
        Me.cmbRemark1_OWN.FormattingEnabled = True
        Me.cmbRemark1_OWN.Location = New System.Drawing.Point(12, 39)
        Me.cmbRemark1_OWN.MaxLength = 50
        Me.cmbRemark1_OWN.Name = "cmbRemark1_OWN"
        Me.cmbRemark1_OWN.Size = New System.Drawing.Size(456, 26)
        Me.cmbRemark1_OWN.TabIndex = 1
        '
        'frmMiscRemark
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(480, 113)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpRemark)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmMiscRemark"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Misc Remark"
        Me.grpRemark.ResumeLayout(False)
        Me.grpRemark.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents grpRemark As CodeVendor.Controls.Grouper
    Friend WithEvents cmbRemark1_OWN As ComboBox
    Friend WithEvents btnSave As Button
    Friend WithEvents Label2 As Label
End Class
