<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddlChargesDia
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
        Me.grpAddlCharge = New CodeVendor.Controls.Grouper
        Me.cmbChargeName = New System.Windows.Forms.ComboBox
        Me.txtChequeRowIndex = New System.Windows.Forms.TextBox
        Me.gridAddChargeTotal = New System.Windows.Forms.DataGridView
        Me.Label137 = New System.Windows.Forms.Label
        Me.Label139 = New System.Windows.Forms.Label
        Me.txtChargeAmount_AMT = New System.Windows.Forms.TextBox
        Me.gridAddCharge = New System.Windows.Forms.DataGridView
        Me.grpAddlCharge.SuspendLayout()
        CType(Me.gridAddChargeTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridAddCharge, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpAddlCharge
        '
        Me.grpAddlCharge.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpAddlCharge.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpAddlCharge.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpAddlCharge.BorderColor = System.Drawing.Color.Transparent
        Me.grpAddlCharge.BorderThickness = 1.0!
        Me.grpAddlCharge.Controls.Add(Me.cmbChargeName)
        Me.grpAddlCharge.Controls.Add(Me.txtChequeRowIndex)
        Me.grpAddlCharge.Controls.Add(Me.gridAddChargeTotal)
        Me.grpAddlCharge.Controls.Add(Me.Label137)
        Me.grpAddlCharge.Controls.Add(Me.Label139)
        Me.grpAddlCharge.Controls.Add(Me.txtChargeAmount_AMT)
        Me.grpAddlCharge.Controls.Add(Me.gridAddCharge)
        Me.grpAddlCharge.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpAddlCharge.GroupImage = Nothing
        Me.grpAddlCharge.GroupTitle = ""
        Me.grpAddlCharge.Location = New System.Drawing.Point(5, -3)
        Me.grpAddlCharge.Name = "grpAddlCharge"
        Me.grpAddlCharge.Padding = New System.Windows.Forms.Padding(23, 20, 23, 20)
        Me.grpAddlCharge.PaintGroupBox = False
        Me.grpAddlCharge.RoundCorners = 10
        Me.grpAddlCharge.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpAddlCharge.ShadowControl = False
        Me.grpAddlCharge.ShadowThickness = 3
        Me.grpAddlCharge.Size = New System.Drawing.Size(435, 170)
        Me.grpAddlCharge.TabIndex = 2
        '
        'cmbChargeName
        '
        Me.cmbChargeName.FormattingEnabled = True
        Me.cmbChargeName.Location = New System.Drawing.Point(8, 33)
        Me.cmbChargeName.Name = "cmbChargeName"
        Me.cmbChargeName.Size = New System.Drawing.Size(285, 21)
        Me.cmbChargeName.TabIndex = 13
        '
        'txtChequeRowIndex
        '
        Me.txtChequeRowIndex.Location = New System.Drawing.Point(407, 33)
        Me.txtChequeRowIndex.Name = "txtChequeRowIndex"
        Me.txtChequeRowIndex.Size = New System.Drawing.Size(26, 21)
        Me.txtChequeRowIndex.TabIndex = 12
        Me.txtChequeRowIndex.Visible = False
        '
        'gridAddChargeTotal
        '
        Me.gridAddChargeTotal.AllowUserToAddRows = False
        Me.gridAddChargeTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridAddChargeTotal.Enabled = False
        Me.gridAddChargeTotal.Location = New System.Drawing.Point(8, 144)
        Me.gridAddChargeTotal.Name = "gridAddChargeTotal"
        Me.gridAddChargeTotal.ReadOnly = True
        Me.gridAddChargeTotal.Size = New System.Drawing.Size(419, 19)
        Me.gridAddChargeTotal.TabIndex = 11
        '
        'Label137
        '
        Me.Label137.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label137.Location = New System.Drawing.Point(294, 15)
        Me.Label137.Name = "Label137"
        Me.Label137.Size = New System.Drawing.Size(113, 14)
        Me.Label137.TabIndex = 8
        Me.Label137.Text = "AMOUNT"
        Me.Label137.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label139
        '
        Me.Label139.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label139.Location = New System.Drawing.Point(8, 16)
        Me.Label139.Name = "Label139"
        Me.Label139.Size = New System.Drawing.Size(239, 14)
        Me.Label139.TabIndex = 2
        Me.Label139.Text = "CHARGE NAME"
        Me.Label139.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtChargeAmount_AMT
        '
        Me.txtChargeAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChargeAmount_AMT.Location = New System.Drawing.Point(294, 33)
        Me.txtChargeAmount_AMT.MaxLength = 12
        Me.txtChargeAmount_AMT.Name = "txtChargeAmount_AMT"
        Me.txtChargeAmount_AMT.Size = New System.Drawing.Size(112, 22)
        Me.txtChargeAmount_AMT.TabIndex = 9
        '
        'gridAddCharge
        '
        Me.gridAddCharge.AllowUserToAddRows = False
        Me.gridAddCharge.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridAddCharge.ColumnHeadersVisible = False
        Me.gridAddCharge.Location = New System.Drawing.Point(8, 55)
        Me.gridAddCharge.Name = "gridAddCharge"
        Me.gridAddCharge.ReadOnly = True
        Me.gridAddCharge.RowHeadersVisible = False
        Me.gridAddCharge.RowTemplate.Height = 20
        Me.gridAddCharge.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridAddCharge.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridAddCharge.Size = New System.Drawing.Size(419, 89)
        Me.gridAddCharge.TabIndex = 10
        '
        'AddlChargesDia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(445, 171)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAddlCharge)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "AddlChargesDia"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AddlChargesDia"
        Me.grpAddlCharge.ResumeLayout(False)
        Me.grpAddlCharge.PerformLayout()
        CType(Me.gridAddChargeTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridAddCharge, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpAddlCharge As CodeVendor.Controls.Grouper
    Friend WithEvents txtChequeRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents gridAddChargeTotal As System.Windows.Forms.DataGridView
    Friend WithEvents Label137 As System.Windows.Forms.Label
    Friend WithEvents Label139 As System.Windows.Forms.Label
    Friend WithEvents txtChargeAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents gridAddCharge As System.Windows.Forms.DataGridView
    Friend WithEvents cmbChargeName As System.Windows.Forms.ComboBox
End Class
