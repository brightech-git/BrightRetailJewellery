<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmState
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
        Me.grpOptions = New CodeVendor.Controls.Grouper()
        Me.rbtSezSale = New System.Windows.Forms.RadioButton()
        Me.rbtExportSale = New System.Windows.Forms.RadioButton()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.rbtOther = New System.Windows.Forms.RadioButton()
        Me.rbtOwn = New System.Windows.Forms.RadioButton()
        Me.rbtNRIsale = New System.Windows.Forms.RadioButton()
        Me.grpOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpOptions
        '
        Me.grpOptions.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpOptions.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpOptions.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpOptions.BorderColor = System.Drawing.Color.Transparent
        Me.grpOptions.BorderThickness = 1.0!
        Me.grpOptions.Controls.Add(Me.rbtNRIsale)
        Me.grpOptions.Controls.Add(Me.rbtSezSale)
        Me.grpOptions.Controls.Add(Me.rbtExportSale)
        Me.grpOptions.Controls.Add(Me.btnCancel)
        Me.grpOptions.Controls.Add(Me.btnOk)
        Me.grpOptions.Controls.Add(Me.rbtOther)
        Me.grpOptions.Controls.Add(Me.rbtOwn)
        Me.grpOptions.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpOptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpOptions.GroupImage = Nothing
        Me.grpOptions.GroupTitle = ""
        Me.grpOptions.Location = New System.Drawing.Point(0, 0)
        Me.grpOptions.Name = "grpOptions"
        Me.grpOptions.Padding = New System.Windows.Forms.Padding(20)
        Me.grpOptions.PaintGroupBox = False
        Me.grpOptions.RoundCorners = 10
        Me.grpOptions.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpOptions.ShadowControl = False
        Me.grpOptions.ShadowThickness = 3
        Me.grpOptions.Size = New System.Drawing.Size(292, 147)
        Me.grpOptions.TabIndex = 0
        '
        'rbtSezSale
        '
        Me.rbtSezSale.AutoSize = True
        Me.rbtSezSale.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtSezSale.Location = New System.Drawing.Point(154, 57)
        Me.rbtSezSale.Name = "rbtSezSale"
        Me.rbtSezSale.Size = New System.Drawing.Size(101, 22)
        Me.rbtSezSale.TabIndex = 3
        Me.rbtSezSale.Text = "SEZ Sale"
        Me.rbtSezSale.UseVisualStyleBackColor = True
        '
        'rbtExportSale
        '
        Me.rbtExportSale.AutoSize = True
        Me.rbtExportSale.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtExportSale.Location = New System.Drawing.Point(13, 57)
        Me.rbtExportSale.Name = "rbtExportSale"
        Me.rbtExportSale.Size = New System.Drawing.Size(127, 22)
        Me.rbtExportSale.TabIndex = 2
        Me.rbtExportSale.Text = "Export Sale"
        Me.rbtExportSale.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(154, 107)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(117, 30)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Location = New System.Drawing.Point(23, 107)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(117, 30)
        Me.btnOk.TabIndex = 5
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'rbtOther
        '
        Me.rbtOther.AutoSize = True
        Me.rbtOther.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtOther.Location = New System.Drawing.Point(154, 32)
        Me.rbtOther.Name = "rbtOther"
        Me.rbtOther.Size = New System.Drawing.Size(129, 22)
        Me.rbtOther.TabIndex = 1
        Me.rbtOther.Text = "Other State"
        Me.rbtOther.UseVisualStyleBackColor = True
        '
        'rbtOwn
        '
        Me.rbtOwn.AutoSize = True
        Me.rbtOwn.Checked = True
        Me.rbtOwn.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtOwn.Location = New System.Drawing.Point(13, 32)
        Me.rbtOwn.Name = "rbtOwn"
        Me.rbtOwn.Size = New System.Drawing.Size(119, 22)
        Me.rbtOwn.TabIndex = 0
        Me.rbtOwn.TabStop = True
        Me.rbtOwn.Text = "Own State"
        Me.rbtOwn.UseVisualStyleBackColor = True
        '
        'rbtNRIsale
        '
        Me.rbtNRIsale.AutoSize = True
        Me.rbtNRIsale.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtNRIsale.Location = New System.Drawing.Point(13, 82)
        Me.rbtNRIsale.Name = "rbtNRIsale"
        Me.rbtNRIsale.Size = New System.Drawing.Size(102, 22)
        Me.rbtNRIsale.TabIndex = 4
        Me.rbtNRIsale.Text = "NRI Sale"
        Me.rbtNRIsale.UseVisualStyleBackColor = True
        '
        'frmState
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(292, 147)
        Me.Controls.Add(Me.grpOptions)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmState"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Own/Other State"
        Me.grpOptions.ResumeLayout(False)
        Me.grpOptions.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpOptions As CodeVendor.Controls.Grouper
    Friend WithEvents rbtOther As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOwn As System.Windows.Forms.RadioButton
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents rbtExportSale As RadioButton
    Friend WithEvents rbtSezSale As RadioButton
    Friend WithEvents rbtNRIsale As RadioButton
End Class
