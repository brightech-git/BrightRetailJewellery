<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_ESTIMATEISSNO
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
        Me.grpOrderSearch = New CodeVendor.Controls.Grouper
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtEstNo_NUM = New System.Windows.Forms.TextBox
        Me.grpOrderSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpOrderSearch
        '
        Me.grpOrderSearch.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpOrderSearch.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpOrderSearch.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpOrderSearch.BorderColor = System.Drawing.Color.Empty
        Me.grpOrderSearch.BorderThickness = 1.0!
        Me.grpOrderSearch.Controls.Add(Me.Label1)
        Me.grpOrderSearch.Controls.Add(Me.txtEstNo_NUM)
        Me.grpOrderSearch.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpOrderSearch.GroupImage = Nothing
        Me.grpOrderSearch.GroupTitle = ""
        Me.grpOrderSearch.Location = New System.Drawing.Point(4, -5)
        Me.grpOrderSearch.Name = "grpOrderSearch"
        Me.grpOrderSearch.Padding = New System.Windows.Forms.Padding(20)
        Me.grpOrderSearch.PaintGroupBox = False
        Me.grpOrderSearch.RoundCorners = 10
        Me.grpOrderSearch.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpOrderSearch.ShadowControl = False
        Me.grpOrderSearch.ShadowThickness = 3
        Me.grpOrderSearch.Size = New System.Drawing.Size(193, 54)
        Me.grpOrderSearch.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(11, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 14)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Est No"
        '
        'txtEstNo_NUM
        '
        Me.txtEstNo_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEstNo_NUM.Location = New System.Drawing.Point(86, 23)
        Me.txtEstNo_NUM.Name = "txtEstNo_NUM"
        Me.txtEstNo_NUM.Size = New System.Drawing.Size(99, 22)
        Me.txtEstNo_NUM.TabIndex = 0
        Me.txtEstNo_NUM.Text = "O11128"
        '
        'FRM_ESTIMATEISSNO
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(202, 55)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpOrderSearch)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FRM_ESTIMATEISSNO"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Estimate No"
        Me.grpOrderSearch.ResumeLayout(False)
        Me.grpOrderSearch.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpOrderSearch As CodeVendor.Controls.Grouper
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtEstNo_NUM As System.Windows.Forms.TextBox
End Class
