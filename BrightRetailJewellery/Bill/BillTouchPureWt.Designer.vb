<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BillTouchPureWt
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
        Me.grpTouchPureWt = New CodeVendor.Controls.Grouper
        Me.lblTouch = New System.Windows.Forms.Label
        Me.Label49 = New System.Windows.Forms.Label
        Me.txtSaPureWt_WET = New System.Windows.Forms.TextBox
        Me.txtSaTouch_AMT = New System.Windows.Forms.TextBox
        Me.grpTouchPureWt.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpTouchPureWt
        '
        Me.grpTouchPureWt.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpTouchPureWt.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpTouchPureWt.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpTouchPureWt.BorderColor = System.Drawing.Color.Transparent
        Me.grpTouchPureWt.BorderThickness = 1.0!
        Me.grpTouchPureWt.Controls.Add(Me.lblTouch)
        Me.grpTouchPureWt.Controls.Add(Me.Label49)
        Me.grpTouchPureWt.Controls.Add(Me.txtSaPureWt_WET)
        Me.grpTouchPureWt.Controls.Add(Me.txtSaTouch_AMT)
        Me.grpTouchPureWt.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpTouchPureWt.GroupImage = Nothing
        Me.grpTouchPureWt.GroupTitle = ""
        Me.grpTouchPureWt.Location = New System.Drawing.Point(6, -2)
        Me.grpTouchPureWt.Name = "grpTouchPureWt"
        Me.grpTouchPureWt.Padding = New System.Windows.Forms.Padding(20)
        Me.grpTouchPureWt.PaintGroupBox = False
        Me.grpTouchPureWt.RoundCorners = 10
        Me.grpTouchPureWt.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpTouchPureWt.ShadowControl = False
        Me.grpTouchPureWt.ShadowThickness = 3
        Me.grpTouchPureWt.Size = New System.Drawing.Size(331, 59)
        Me.grpTouchPureWt.TabIndex = 1
        '
        'lblTouch
        '
        Me.lblTouch.AutoSize = True
        Me.lblTouch.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTouch.Location = New System.Drawing.Point(14, 30)
        Me.lblTouch.Name = "lblTouch"
        Me.lblTouch.Size = New System.Drawing.Size(46, 14)
        Me.lblTouch.TabIndex = 0
        Me.lblTouch.Text = "Touch"
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(164, 30)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(61, 14)
        Me.Label49.TabIndex = 2
        Me.Label49.Text = "Pure Wt"
        '
        'txtSaPureWt_WET
        '
        Me.txtSaPureWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSaPureWt_WET.Location = New System.Drawing.Point(231, 27)
        Me.txtSaPureWt_WET.Name = "txtSaPureWt_WET"
        Me.txtSaPureWt_WET.Size = New System.Drawing.Size(90, 22)
        Me.txtSaPureWt_WET.TabIndex = 3
        '
        'txtSaTouch_AMT
        '
        Me.txtSaTouch_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSaTouch_AMT.Location = New System.Drawing.Point(66, 26)
        Me.txtSaTouch_AMT.Name = "txtSaTouch_AMT"
        Me.txtSaTouch_AMT.Size = New System.Drawing.Size(92, 22)
        Me.txtSaTouch_AMT.TabIndex = 1
        '
        'BillTouchPureWt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(344, 64)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpTouchPureWt)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "BillTouchPureWt"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Touch & PureWt"
        Me.grpTouchPureWt.ResumeLayout(False)
        Me.grpTouchPureWt.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpTouchPureWt As CodeVendor.Controls.Grouper
    Friend WithEvents lblTouch As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents txtSaPureWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtSaTouch_AMT As System.Windows.Forms.TextBox
End Class
