<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPurchRateDisc
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
        Me.grpFinalDisc = New CodeVendor.Controls.Grouper
        Me.txtRateDisc_AMT = New System.Windows.Forms.TextBox
        Me.lblFinalAmount = New System.Windows.Forms.Label
        Me.grpFinalDisc.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpFinalDisc
        '
        Me.grpFinalDisc.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpFinalDisc.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpFinalDisc.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpFinalDisc.BorderColor = System.Drawing.Color.Transparent
        Me.grpFinalDisc.BorderThickness = 1.0!
        Me.grpFinalDisc.Controls.Add(Me.txtRateDisc_AMT)
        Me.grpFinalDisc.Controls.Add(Me.lblFinalAmount)
        Me.grpFinalDisc.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpFinalDisc.GroupImage = Nothing
        Me.grpFinalDisc.GroupTitle = ""
        Me.grpFinalDisc.Location = New System.Drawing.Point(4, -5)
        Me.grpFinalDisc.Name = "grpFinalDisc"
        Me.grpFinalDisc.Padding = New System.Windows.Forms.Padding(20)
        Me.grpFinalDisc.PaintGroupBox = False
        Me.grpFinalDisc.RoundCorners = 10
        Me.grpFinalDisc.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpFinalDisc.ShadowControl = False
        Me.grpFinalDisc.ShadowThickness = 3
        Me.grpFinalDisc.Size = New System.Drawing.Size(241, 79)
        Me.grpFinalDisc.TabIndex = 0
        '
        'txtRateDisc_AMT
        '
        Me.txtRateDisc_AMT.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRateDisc_AMT.Location = New System.Drawing.Point(6, 43)
        Me.txtRateDisc_AMT.Name = "txtRateDisc_AMT"
        Me.txtRateDisc_AMT.Size = New System.Drawing.Size(229, 31)
        Me.txtRateDisc_AMT.TabIndex = 4
        Me.txtRateDisc_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblFinalAmount
        '
        Me.lblFinalAmount.BackColor = System.Drawing.Color.Transparent
        Me.lblFinalAmount.Font = New System.Drawing.Font("Verdana", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinalAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFinalAmount.Location = New System.Drawing.Point(6, 8)
        Me.lblFinalAmount.Name = "lblFinalAmount"
        Me.lblFinalAmount.Size = New System.Drawing.Size(229, 32)
        Me.lblFinalAmount.TabIndex = 0
        Me.lblFinalAmount.Text = "Discount %"
        Me.lblFinalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmPurchRateDisc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(249, 75)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpFinalDisc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmPurchRateDisc"
        Me.Text = "Purchase Rate Discount"
        Me.grpFinalDisc.ResumeLayout(False)
        Me.grpFinalDisc.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpFinalDisc As CodeVendor.Controls.Grouper
    Friend WithEvents txtRateDisc_AMT As System.Windows.Forms.TextBox
    Friend WithEvents lblFinalAmount As System.Windows.Forms.Label
End Class
