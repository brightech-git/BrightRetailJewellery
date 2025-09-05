<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRegularCustomerSearchDia
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
        Me.cmbSearchKey = New System.Windows.Forms.ComboBox
        Me.txtSearchString = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.grpCustomerSearch = New CodeVendor.Controls.Grouper
        Me.rbtExact = New System.Windows.Forms.RadioButton
        Me.rbtLike = New System.Windows.Forms.RadioButton
        Me.grpCustomerSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmbSearchKey
        '
        Me.cmbSearchKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSearchKey.FormattingEnabled = True
        Me.cmbSearchKey.Location = New System.Drawing.Point(10, 36)
        Me.cmbSearchKey.Name = "cmbSearchKey"
        Me.cmbSearchKey.Size = New System.Drawing.Size(231, 21)
        Me.cmbSearchKey.TabIndex = 1
        '
        'txtSearchString
        '
        Me.txtSearchString.Location = New System.Drawing.Point(10, 101)
        Me.txtSearchString.Name = "txtSearchString"
        Me.txtSearchString.Size = New System.Drawing.Size(231, 21)
        Me.txtSearchString.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Search Key"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 85)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Search String"
        '
        'grpCustomerSearch
        '
        Me.grpCustomerSearch.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpCustomerSearch.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpCustomerSearch.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpCustomerSearch.BorderColor = System.Drawing.Color.Transparent
        Me.grpCustomerSearch.BorderThickness = 1.0!
        Me.grpCustomerSearch.Controls.Add(Me.rbtLike)
        Me.grpCustomerSearch.Controls.Add(Me.rbtExact)
        Me.grpCustomerSearch.Controls.Add(Me.Label1)
        Me.grpCustomerSearch.Controls.Add(Me.cmbSearchKey)
        Me.grpCustomerSearch.Controls.Add(Me.Label2)
        Me.grpCustomerSearch.Controls.Add(Me.txtSearchString)
        Me.grpCustomerSearch.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpCustomerSearch.GroupImage = Nothing
        Me.grpCustomerSearch.GroupTitle = ""
        Me.grpCustomerSearch.Location = New System.Drawing.Point(5, -5)
        Me.grpCustomerSearch.Name = "grpCustomerSearch"
        Me.grpCustomerSearch.Padding = New System.Windows.Forms.Padding(20)
        Me.grpCustomerSearch.PaintGroupBox = False
        Me.grpCustomerSearch.RoundCorners = 10
        Me.grpCustomerSearch.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpCustomerSearch.ShadowControl = False
        Me.grpCustomerSearch.ShadowThickness = 3
        Me.grpCustomerSearch.Size = New System.Drawing.Size(250, 131)
        Me.grpCustomerSearch.TabIndex = 4
        '
        'rbtExact
        '
        Me.rbtExact.AutoSize = True
        Me.rbtExact.Location = New System.Drawing.Point(67, 63)
        Me.rbtExact.Name = "rbtExact"
        Me.rbtExact.Size = New System.Drawing.Size(56, 17)
        Me.rbtExact.TabIndex = 3
        Me.rbtExact.Text = "Exact"
        Me.rbtExact.UseVisualStyleBackColor = True
        '
        'rbtLike
        '
        Me.rbtLike.AutoSize = True
        Me.rbtLike.Checked = True
        Me.rbtLike.Location = New System.Drawing.Point(13, 63)
        Me.rbtLike.Name = "rbtLike"
        Me.rbtLike.Size = New System.Drawing.Size(48, 17)
        Me.rbtLike.TabIndex = 2
        Me.rbtLike.TabStop = True
        Me.rbtLike.Text = "Like"
        Me.rbtLike.UseVisualStyleBackColor = True
        '
        'frmRegularCustomerSearchDia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(264, 158)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpCustomerSearch)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(266, 133)
        Me.Name = "frmRegularCustomerSearchDia"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Customer Search Dialog"
        Me.grpCustomerSearch.ResumeLayout(False)
        Me.grpCustomerSearch.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmbSearchKey As System.Windows.Forms.ComboBox
    Friend WithEvents txtSearchString As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grpCustomerSearch As CodeVendor.Controls.Grouper
    Friend WithEvents rbtLike As System.Windows.Forms.RadioButton
    Friend WithEvents rbtExact As System.Windows.Forms.RadioButton
End Class
