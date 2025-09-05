<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BillKatchaFtDetail
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
        Me.lblWeight = New System.Windows.Forms.Label
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
        Me.grpTouchPureWt.Controls.Add(Me.lblWeight)
        Me.grpTouchPureWt.Controls.Add(Me.txtSaPureWt_WET)
        Me.grpTouchPureWt.Controls.Add(Me.txtSaTouch_AMT)
        Me.grpTouchPureWt.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpTouchPureWt.GroupImage = Nothing
        Me.grpTouchPureWt.GroupTitle = ""
        Me.grpTouchPureWt.Location = New System.Drawing.Point(5, -2)
        Me.grpTouchPureWt.Name = "grpTouchPureWt"
        Me.grpTouchPureWt.Padding = New System.Windows.Forms.Padding(20)
        Me.grpTouchPureWt.PaintGroupBox = False
        Me.grpTouchPureWt.RoundCorners = 10
        Me.grpTouchPureWt.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpTouchPureWt.ShadowControl = False
        Me.grpTouchPureWt.ShadowThickness = 3
        Me.grpTouchPureWt.Size = New System.Drawing.Size(331, 59)
        Me.grpTouchPureWt.TabIndex = 0
        '
        'lblTouch
        '
        Me.lblTouch.AutoSize = True
        Me.lblTouch.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTouch.Location = New System.Drawing.Point(180, 29)
        Me.lblTouch.Name = "lblTouch"
        Me.lblTouch.Size = New System.Drawing.Size(46, 14)
        Me.lblTouch.TabIndex = 2
        Me.lblTouch.Text = "Touch"
        '
        'lblWeight
        '
        Me.lblWeight.AutoSize = True
        Me.lblWeight.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWeight.Location = New System.Drawing.Point(16, 28)
        Me.lblWeight.Name = "lblWeight"
        Me.lblWeight.Size = New System.Drawing.Size(54, 14)
        Me.lblWeight.TabIndex = 0
        Me.lblWeight.Text = "Weight"
        '
        'txtSaPureWt_WET
        '
        Me.txtSaPureWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSaPureWt_WET.Location = New System.Drawing.Point(83, 25)
        Me.txtSaPureWt_WET.Name = "txtSaPureWt_WET"
        Me.txtSaPureWt_WET.Size = New System.Drawing.Size(90, 22)
        Me.txtSaPureWt_WET.TabIndex = 1
        '
        'txtSaTouch_AMT
        '
        Me.txtSaTouch_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSaTouch_AMT.Location = New System.Drawing.Point(232, 25)
        Me.txtSaTouch_AMT.Name = "txtSaTouch_AMT"
        Me.txtSaTouch_AMT.Size = New System.Drawing.Size(92, 22)
        Me.txtSaTouch_AMT.TabIndex = 3
        '
        'BillKatchaFtDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(341, 65)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpTouchPureWt)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "BillKatchaFtDetail"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "KatchaFtDetail"
        Me.grpTouchPureWt.ResumeLayout(False)
        Me.grpTouchPureWt.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpTouchPureWt As CodeVendor.Controls.Grouper
    Friend WithEvents lblTouch As System.Windows.Forms.Label
    Friend WithEvents lblWeight As System.Windows.Forms.Label
    Friend WithEvents txtSaPureWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtSaTouch_AMT As System.Windows.Forms.TextBox
End Class
