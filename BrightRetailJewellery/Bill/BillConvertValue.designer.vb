<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BillConvertValue
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
        Me.grpConvert = New CodeVendor.Controls.Grouper
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtConvertValue_AMT = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtConvertRate_AMT = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtConvertWeight_WET = New System.Windows.Forms.TextBox
        Me.grpConvert.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpConvert
        '
        Me.grpConvert.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpConvert.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpConvert.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpConvert.BorderColor = System.Drawing.Color.Empty
        Me.grpConvert.BorderThickness = 1.0!
        Me.grpConvert.Controls.Add(Me.Label3)
        Me.grpConvert.Controls.Add(Me.Label1)
        Me.grpConvert.Controls.Add(Me.txtConvertWeight_WET)
        Me.grpConvert.Controls.Add(Me.Label2)
        Me.grpConvert.Controls.Add(Me.txtConvertRate_AMT)
        Me.grpConvert.Controls.Add(Me.txtConvertValue_AMT)
        Me.grpConvert.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpConvert.GroupImage = Nothing
        Me.grpConvert.GroupTitle = ""
        Me.grpConvert.Location = New System.Drawing.Point(10, -1)
        Me.grpConvert.Name = "grpConvert"
        Me.grpConvert.Padding = New System.Windows.Forms.Padding(23, 22, 23, 22)
        Me.grpConvert.PaintGroupBox = False
        Me.grpConvert.RoundCorners = 10
        Me.grpConvert.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpConvert.ShadowControl = False
        Me.grpConvert.ShadowThickness = 3
        Me.grpConvert.Size = New System.Drawing.Size(160, 154)
        Me.grpConvert.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 14)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Convert Value"
        '
        'txtConvertValue_AMT
        '
        Me.txtConvertValue_AMT.Location = New System.Drawing.Point(7, 39)
        Me.txtConvertValue_AMT.Name = "txtConvertValue_AMT"
        Me.txtConvertValue_AMT.Size = New System.Drawing.Size(145, 22)
        Me.txtConvertValue_AMT.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 14)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Convert Rate"
        '
        'txtConvertRate_AMT
        '
        Me.txtConvertRate_AMT.Location = New System.Drawing.Point(7, 81)
        Me.txtConvertRate_AMT.Name = "txtConvertRate_AMT"
        Me.txtConvertRate_AMT.Size = New System.Drawing.Size(145, 22)
        Me.txtConvertRate_AMT.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 106)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 14)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Convert Weight"
        '
        'txtConvertWeight_WET
        '
        Me.txtConvertWeight_WET.Location = New System.Drawing.Point(7, 123)
        Me.txtConvertWeight_WET.Name = "txtConvertWeight_WET"
        Me.txtConvertWeight_WET.Size = New System.Drawing.Size(145, 22)
        Me.txtConvertWeight_WET.TabIndex = 5
        '
        'BillConvertValue
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(179, 162)
        Me.Controls.Add(Me.grpConvert)
        Me.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "BillConvertValue"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Convert Value"
        Me.grpConvert.ResumeLayout(False)
        Me.grpConvert.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpConvert As CodeVendor.Controls.Grouper
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtConvertWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtConvertRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtConvertValue_AMT As System.Windows.Forms.TextBox
End Class
