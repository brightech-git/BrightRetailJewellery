<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSrCreditAdjustments
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
        Me.grpSrCr = New CodeVendor.Controls.Grouper
        Me.txtSrCrRowIndex = New System.Windows.Forms.TextBox
        Me.gridSrCrTotal = New System.Windows.Forms.DataGridView
        Me.Label137 = New System.Windows.Forms.Label
        Me.Label136 = New System.Windows.Forms.Label
        Me.txtAmount_AMT = New System.Windows.Forms.TextBox
        Me.txtRefNo = New System.Windows.Forms.TextBox
        Me.gridSrCr = New System.Windows.Forms.DataGridView
        Me.grpSrCr.SuspendLayout()
        CType(Me.gridSrCrTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridSrCr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpSrCr
        '
        Me.grpSrCr.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpSrCr.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpSrCr.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpSrCr.BorderColor = System.Drawing.Color.Transparent
        Me.grpSrCr.BorderThickness = 1.0!
        Me.grpSrCr.Controls.Add(Me.txtSrCrRowIndex)
        Me.grpSrCr.Controls.Add(Me.gridSrCrTotal)
        Me.grpSrCr.Controls.Add(Me.Label137)
        Me.grpSrCr.Controls.Add(Me.Label136)
        Me.grpSrCr.Controls.Add(Me.txtAmount_AMT)
        Me.grpSrCr.Controls.Add(Me.txtRefNo)
        Me.grpSrCr.Controls.Add(Me.gridSrCr)
        Me.grpSrCr.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpSrCr.GroupImage = Nothing
        Me.grpSrCr.GroupTitle = ""
        Me.grpSrCr.Location = New System.Drawing.Point(4, -4)
        Me.grpSrCr.Name = "grpSrCr"
        Me.grpSrCr.Padding = New System.Windows.Forms.Padding(20)
        Me.grpSrCr.PaintGroupBox = False
        Me.grpSrCr.RoundCorners = 10
        Me.grpSrCr.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpSrCr.ShadowControl = False
        Me.grpSrCr.ShadowThickness = 3
        Me.grpSrCr.Size = New System.Drawing.Size(283, 167)
        Me.grpSrCr.TabIndex = 2
        '
        'txtSrCrRowIndex
        '
        Me.txtSrCrRowIndex.Location = New System.Drawing.Point(259, 31)
        Me.txtSrCrRowIndex.Name = "txtSrCrRowIndex"
        Me.txtSrCrRowIndex.Size = New System.Drawing.Size(21, 21)
        Me.txtSrCrRowIndex.TabIndex = 12
        Me.txtSrCrRowIndex.Visible = False
        '
        'gridSrCrTotal
        '
        Me.gridSrCrTotal.AllowUserToAddRows = False
        Me.gridSrCrTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridSrCrTotal.Enabled = False
        Me.gridSrCrTotal.Location = New System.Drawing.Point(8, 142)
        Me.gridSrCrTotal.Name = "gridSrCrTotal"
        Me.gridSrCrTotal.ReadOnly = True
        Me.gridSrCrTotal.Size = New System.Drawing.Size(267, 19)
        Me.gridSrCrTotal.TabIndex = 11
        '
        'Label137
        '
        Me.Label137.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label137.Location = New System.Drawing.Point(161, 13)
        Me.Label137.Name = "Label137"
        Me.Label137.Size = New System.Drawing.Size(97, 14)
        Me.Label137.TabIndex = 8
        Me.Label137.Text = "AMOUNT"
        Me.Label137.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label136
        '
        Me.Label136.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label136.Location = New System.Drawing.Point(8, 13)
        Me.Label136.Name = "Label136"
        Me.Label136.Size = New System.Drawing.Size(152, 14)
        Me.Label136.TabIndex = 6
        Me.Label136.Text = "REF NO"
        Me.Label136.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtAmount_AMT
        '
        Me.txtAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount_AMT.Location = New System.Drawing.Point(161, 30)
        Me.txtAmount_AMT.MaxLength = 12
        Me.txtAmount_AMT.Name = "txtAmount_AMT"
        Me.txtAmount_AMT.Size = New System.Drawing.Size(97, 22)
        Me.txtAmount_AMT.TabIndex = 9
        '
        'txtRefNo
        '
        Me.txtRefNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtRefNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRefNo.Location = New System.Drawing.Point(8, 30)
        Me.txtRefNo.MaxLength = 20
        Me.txtRefNo.Name = "txtRefNo"
        Me.txtRefNo.Size = New System.Drawing.Size(152, 22)
        Me.txtRefNo.TabIndex = 7
        Me.txtRefNo.Text = "12345678901234567890"
        '
        'gridSrCr
        '
        Me.gridSrCr.AllowUserToAddRows = False
        Me.gridSrCr.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridSrCr.ColumnHeadersVisible = False
        Me.gridSrCr.Location = New System.Drawing.Point(8, 53)
        Me.gridSrCr.Name = "gridSrCr"
        Me.gridSrCr.ReadOnly = True
        Me.gridSrCr.RowHeadersVisible = False
        Me.gridSrCr.RowTemplate.Height = 20
        Me.gridSrCr.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridSrCr.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridSrCr.Size = New System.Drawing.Size(267, 89)
        Me.gridSrCr.TabIndex = 10
        '
        'frmSrCreditAdjustments
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(292, 167)
        Me.Controls.Add(Me.grpSrCr)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmSrCreditAdjustments"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Credit Adjustments"
        Me.grpSrCr.ResumeLayout(False)
        Me.grpSrCr.PerformLayout()
        CType(Me.gridSrCrTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridSrCr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpSrCr As CodeVendor.Controls.Grouper
    Friend WithEvents txtSrCrRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents gridSrCrTotal As System.Windows.Forms.DataGridView
    Friend WithEvents Label137 As System.Windows.Forms.Label
    Friend WithEvents Label136 As System.Windows.Forms.Label
    Friend WithEvents txtAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtRefNo As System.Windows.Forms.TextBox
    Friend WithEvents gridSrCr As System.Windows.Forms.DataGridView
End Class
