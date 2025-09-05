<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGSTTax
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
        Me.grpFinalDisc = New CodeVendor.Controls.Grouper()
        Me.Label106 = New System.Windows.Forms.Label()
        Me.CmbGstCategory = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblFinalAmount = New System.Windows.Forms.Label()
        Me.txtCgst_per_AMT = New System.Windows.Forms.TextBox()
        Me.txtSgst_per_AMT = New System.Windows.Forms.TextBox()
        Me.txtIgst_per_AMT = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
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
        Me.grpFinalDisc.Controls.Add(Me.Label106)
        Me.grpFinalDisc.Controls.Add(Me.CmbGstCategory)
        Me.grpFinalDisc.Controls.Add(Me.Label7)
        Me.grpFinalDisc.Controls.Add(Me.btnOk)
        Me.grpFinalDisc.Controls.Add(Me.Label5)
        Me.grpFinalDisc.Controls.Add(Me.lblFinalAmount)
        Me.grpFinalDisc.Controls.Add(Me.txtCgst_per_AMT)
        Me.grpFinalDisc.Controls.Add(Me.txtSgst_per_AMT)
        Me.grpFinalDisc.Controls.Add(Me.txtIgst_per_AMT)
        Me.grpFinalDisc.Controls.Add(Me.Label15)
        Me.grpFinalDisc.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpFinalDisc.GroupImage = Nothing
        Me.grpFinalDisc.GroupTitle = ""
        Me.grpFinalDisc.Location = New System.Drawing.Point(4, -7)
        Me.grpFinalDisc.Name = "grpFinalDisc"
        Me.grpFinalDisc.Padding = New System.Windows.Forms.Padding(20)
        Me.grpFinalDisc.PaintGroupBox = False
        Me.grpFinalDisc.RoundCorners = 10
        Me.grpFinalDisc.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpFinalDisc.ShadowControl = False
        Me.grpFinalDisc.ShadowThickness = 3
        Me.grpFinalDisc.Size = New System.Drawing.Size(327, 170)
        Me.grpFinalDisc.TabIndex = 0
        '
        'Label106
        '
        Me.Label106.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label106.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Label106.Location = New System.Drawing.Point(29, 33)
        Me.Label106.Name = "Label106"
        Me.Label106.Size = New System.Drawing.Size(128, 27)
        Me.Label106.TabIndex = 0
        Me.Label106.Text = "Gst Category"
        Me.Label106.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CmbGstCategory
        '
        Me.CmbGstCategory.FormattingEnabled = True
        Me.CmbGstCategory.Location = New System.Drawing.Point(161, 36)
        Me.CmbGstCategory.Margin = New System.Windows.Forms.Padding(7)
        Me.CmbGstCategory.Name = "CmbGstCategory"
        Me.CmbGstCategory.Size = New System.Drawing.Size(158, 21)
        Me.CmbGstCategory.TabIndex = 1
        '
        'Label7
        '
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Label7.Location = New System.Drawing.Point(29, 97)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(129, 27)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Central GST %"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(126, 183)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 10)
        Me.btnOk.TabIndex = 2
        Me.btnOk.Text = "ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Label5.Location = New System.Drawing.Point(29, 129)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(129, 27)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Inter-State GST %"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblFinalAmount
        '
        Me.lblFinalAmount.BackColor = System.Drawing.Color.Transparent
        Me.lblFinalAmount.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinalAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFinalAmount.Location = New System.Drawing.Point(5, 13)
        Me.lblFinalAmount.Name = "lblFinalAmount"
        Me.lblFinalAmount.Size = New System.Drawing.Size(314, 17)
        Me.lblFinalAmount.TabIndex = 0
        Me.lblFinalAmount.Text = "GST TAX"
        Me.lblFinalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtCgst_per_AMT
        '
        Me.txtCgst_per_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCgst_per_AMT.Location = New System.Drawing.Point(161, 96)
        Me.txtCgst_per_AMT.Multiline = True
        Me.txtCgst_per_AMT.Name = "txtCgst_per_AMT"
        Me.txtCgst_per_AMT.ReadOnly = True
        Me.txtCgst_per_AMT.Size = New System.Drawing.Size(136, 25)
        Me.txtCgst_per_AMT.TabIndex = 5
        Me.txtCgst_per_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSgst_per_AMT
        '
        Me.txtSgst_per_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSgst_per_AMT.Location = New System.Drawing.Point(161, 64)
        Me.txtSgst_per_AMT.Multiline = True
        Me.txtSgst_per_AMT.Name = "txtSgst_per_AMT"
        Me.txtSgst_per_AMT.ReadOnly = True
        Me.txtSgst_per_AMT.Size = New System.Drawing.Size(136, 24)
        Me.txtSgst_per_AMT.TabIndex = 3
        Me.txtSgst_per_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtIgst_per_AMT
        '
        Me.txtIgst_per_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIgst_per_AMT.Location = New System.Drawing.Point(161, 128)
        Me.txtIgst_per_AMT.Multiline = True
        Me.txtIgst_per_AMT.Name = "txtIgst_per_AMT"
        Me.txtIgst_per_AMT.ReadOnly = True
        Me.txtIgst_per_AMT.Size = New System.Drawing.Size(136, 25)
        Me.txtIgst_per_AMT.TabIndex = 7
        Me.txtIgst_per_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label15
        '
        Me.Label15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label15.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold)
        Me.Label15.Location = New System.Drawing.Point(29, 65)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(129, 27)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "State GST %"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmGSTTax
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(334, 167)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpFinalDisc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmGSTTax"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "GST"
        Me.grpFinalDisc.ResumeLayout(False)
        Me.grpFinalDisc.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpFinalDisc As CodeVendor.Controls.Grouper
    Friend WithEvents lblFinalAmount As System.Windows.Forms.Label
    Friend WithEvents txtCgst_per_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtSgst_per_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtIgst_per_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents Label106 As System.Windows.Forms.Label
    Friend WithEvents CmbGstCategory As System.Windows.Forms.ComboBox
End Class
