<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ManuFacturingIssRec_ORDERINFO
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
        Me.grpOrderInfo = New CodeVendor.Controls.Grouper
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.DgvOrder = New System.Windows.Forms.DataGridView
        Me.grpOrderInfo.SuspendLayout()
        CType(Me.DgvOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpOrderInfo
        '
        Me.grpOrderInfo.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpOrderInfo.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpOrderInfo.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpOrderInfo.BorderColor = System.Drawing.Color.Transparent
        Me.grpOrderInfo.BorderThickness = 1.0!
        Me.grpOrderInfo.Controls.Add(Me.Label1)
        Me.grpOrderInfo.Controls.Add(Me.btnCancel)
        Me.grpOrderInfo.Controls.Add(Me.btnOk)
        Me.grpOrderInfo.Controls.Add(Me.DgvOrder)
        Me.grpOrderInfo.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpOrderInfo.GroupImage = Nothing
        Me.grpOrderInfo.GroupTitle = ""
        Me.grpOrderInfo.Location = New System.Drawing.Point(4, -4)
        Me.grpOrderInfo.Name = "grpOrderInfo"
        Me.grpOrderInfo.Padding = New System.Windows.Forms.Padding(20)
        Me.grpOrderInfo.PaintGroupBox = False
        Me.grpOrderInfo.RoundCorners = 10
        Me.grpOrderInfo.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpOrderInfo.ShadowControl = False
        Me.grpOrderInfo.ShadowThickness = 3
        Me.grpOrderInfo.Size = New System.Drawing.Size(731, 297)
        Me.grpOrderInfo.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(8, 264)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(143, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Select All - [Ctrl + A]"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(552, 260)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(446, 260)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 1
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'DgvOrder
        '
        Me.DgvOrder.AllowUserToAddRows = False
        Me.DgvOrder.AllowUserToDeleteRows = False
        Me.DgvOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgvOrder.Location = New System.Drawing.Point(7, 15)
        Me.DgvOrder.Name = "DgvOrder"
        Me.DgvOrder.Size = New System.Drawing.Size(716, 239)
        Me.DgvOrder.TabIndex = 0
        '
        'ManuFacturingIssRec_ORDERINFO
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(740, 298)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpOrderInfo)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "ManuFacturingIssRec_ORDERINFO"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Select OrderInfo"
        Me.grpOrderInfo.ResumeLayout(False)
        Me.grpOrderInfo.PerformLayout()
        CType(Me.DgvOrder, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpOrderInfo As CodeVendor.Controls.Grouper
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents DgvOrder As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
