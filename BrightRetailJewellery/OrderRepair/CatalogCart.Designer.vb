<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CatalogCart
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
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.grpAddress = New CodeVendor.Controls.Grouper
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpAddress.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(9, 19)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(849, 367)
        Me.DataGridView1.TabIndex = 0
        '
        'grpAddress
        '
        Me.grpAddress.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpAddress.BackgroundGradientColor = System.Drawing.Color.White
        Me.grpAddress.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpAddress.BorderColor = System.Drawing.Color.Transparent
        Me.grpAddress.BorderThickness = 1.0!
        Me.grpAddress.Controls.Add(Me.Label3)
        Me.grpAddress.Controls.Add(Me.Label2)
        Me.grpAddress.Controls.Add(Me.Label1)
        Me.grpAddress.Controls.Add(Me.DataGridView1)
        Me.grpAddress.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpAddress.GroupImage = Nothing
        Me.grpAddress.GroupTitle = ""
        Me.grpAddress.Location = New System.Drawing.Point(4, -5)
        Me.grpAddress.Name = "grpAddress"
        Me.grpAddress.Padding = New System.Windows.Forms.Padding(20)
        Me.grpAddress.PaintGroupBox = False
        Me.grpAddress.RoundCorners = 10
        Me.grpAddress.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpAddress.ShadowControl = False
        Me.grpAddress.ShadowThickness = 3
        Me.grpAddress.Size = New System.Drawing.Size(868, 417)
        Me.grpAddress.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(10, 396)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(192, 14)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Press [Enter] to Select Item"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(324, 396)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(203, 14)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Press [Del] to Remove a Item"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(622, 396)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(234, 14)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Press [Ctrl+D] to Remove All Item"
        '
        'CatalogCart
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(879, 418)
        Me.Controls.Add(Me.grpAddress)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "CatalogCart"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CatalogCart"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpAddress.ResumeLayout(False)
        Me.grpAddress.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents grpAddress As CodeVendor.Controls.Grouper
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
