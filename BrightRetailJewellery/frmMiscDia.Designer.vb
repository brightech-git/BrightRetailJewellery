<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMiscDia
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
        Me.grpMisc = New CodeVendor.Controls.Grouper
        Me.txtMiscRowIndex = New System.Windows.Forms.TextBox
        Me.txtMiscMisc = New System.Windows.Forms.TextBox
        Me.Label71 = New System.Windows.Forms.Label
        Me.Label72 = New System.Windows.Forms.Label
        Me.gridMisc = New System.Windows.Forms.DataGridView
        Me.txtMiscAmount_AMT = New System.Windows.Forms.TextBox
        Me.gridMiscTotal = New System.Windows.Forms.DataGridView
        Me.grpMisc.SuspendLayout()
        CType(Me.gridMisc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridMiscTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpMisc
        '
        Me.grpMisc.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpMisc.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpMisc.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpMisc.BorderColor = System.Drawing.Color.Transparent
        Me.grpMisc.BorderThickness = 1.0!
        Me.grpMisc.Controls.Add(Me.txtMiscRowIndex)
        Me.grpMisc.Controls.Add(Me.txtMiscMisc)
        Me.grpMisc.Controls.Add(Me.Label71)
        Me.grpMisc.Controls.Add(Me.Label72)
        Me.grpMisc.Controls.Add(Me.gridMisc)
        Me.grpMisc.Controls.Add(Me.txtMiscAmount_AMT)
        Me.grpMisc.Controls.Add(Me.gridMiscTotal)
        Me.grpMisc.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpMisc.GroupImage = Nothing
        Me.grpMisc.GroupTitle = ""
        Me.grpMisc.Location = New System.Drawing.Point(6, -4)
        Me.grpMisc.Name = "grpMisc"
        Me.grpMisc.Padding = New System.Windows.Forms.Padding(20)
        Me.grpMisc.PaintGroupBox = False
        Me.grpMisc.RoundCorners = 10
        Me.grpMisc.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpMisc.ShadowControl = False
        Me.grpMisc.ShadowThickness = 3
        Me.grpMisc.Size = New System.Drawing.Size(435, 158)
        Me.grpMisc.TabIndex = 1
        '
        'txtMiscRowIndex
        '
        Me.txtMiscRowIndex.Location = New System.Drawing.Point(286, 89)
        Me.txtMiscRowIndex.Name = "txtMiscRowIndex"
        Me.txtMiscRowIndex.Size = New System.Drawing.Size(43, 21)
        Me.txtMiscRowIndex.TabIndex = 33
        Me.txtMiscRowIndex.Visible = False
        '
        'txtMiscMisc
        '
        Me.txtMiscMisc.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMiscMisc.Location = New System.Drawing.Point(9, 39)
        Me.txtMiscMisc.Name = "txtMiscMisc"
        Me.txtMiscMisc.Size = New System.Drawing.Size(298, 22)
        Me.txtMiscMisc.TabIndex = 1
        '
        'Label71
        '
        Me.Label71.BackColor = System.Drawing.Color.Transparent
        Me.Label71.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label71.Location = New System.Drawing.Point(9, 18)
        Me.Label71.Name = "Label71"
        Me.Label71.Size = New System.Drawing.Size(298, 15)
        Me.Label71.TabIndex = 0
        Me.Label71.Text = "Miscellaneous"
        Me.Label71.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label72
        '
        Me.Label72.BackColor = System.Drawing.Color.Transparent
        Me.Label72.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label72.Location = New System.Drawing.Point(309, 21)
        Me.Label72.Name = "Label72"
        Me.Label72.Size = New System.Drawing.Size(99, 15)
        Me.Label72.TabIndex = 2
        Me.Label72.Text = "Amount"
        Me.Label72.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gridMisc
        '
        Me.gridMisc.AllowUserToAddRows = False
        Me.gridMisc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridMisc.ColumnHeadersVisible = False
        Me.gridMisc.Location = New System.Drawing.Point(9, 62)
        Me.gridMisc.Name = "gridMisc"
        Me.gridMisc.ReadOnly = True
        Me.gridMisc.RowHeadersVisible = False
        Me.gridMisc.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridMisc.RowTemplate.Height = 20
        Me.gridMisc.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridMisc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridMisc.Size = New System.Drawing.Size(418, 71)
        Me.gridMisc.TabIndex = 4
        '
        'txtMiscAmount_AMT
        '
        Me.txtMiscAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMiscAmount_AMT.Location = New System.Drawing.Point(308, 39)
        Me.txtMiscAmount_AMT.MaxLength = 12
        Me.txtMiscAmount_AMT.Name = "txtMiscAmount_AMT"
        Me.txtMiscAmount_AMT.Size = New System.Drawing.Size(99, 22)
        Me.txtMiscAmount_AMT.TabIndex = 3
        Me.txtMiscAmount_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'gridMiscTotal
        '
        Me.gridMiscTotal.AllowUserToAddRows = False
        Me.gridMiscTotal.AllowUserToDeleteRows = False
        Me.gridMiscTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridMiscTotal.ColumnHeadersVisible = False
        Me.gridMiscTotal.Enabled = False
        Me.gridMiscTotal.Location = New System.Drawing.Point(9, 133)
        Me.gridMiscTotal.Name = "gridMiscTotal"
        Me.gridMiscTotal.ReadOnly = True
        Me.gridMiscTotal.RowHeadersVisible = False
        Me.gridMiscTotal.Size = New System.Drawing.Size(418, 19)
        Me.gridMiscTotal.TabIndex = 5
        '
        'frmMiscDia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(447, 159)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpMisc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmMiscDia"
        Me.Text = "Miscellaneous Detail"
        Me.grpMisc.ResumeLayout(False)
        Me.grpMisc.PerformLayout()
        CType(Me.gridMisc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridMiscTotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpMisc As CodeVendor.Controls.Grouper
    Friend WithEvents txtMiscRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents txtMiscMisc As System.Windows.Forms.TextBox
    Friend WithEvents Label71 As System.Windows.Forms.Label
    Friend WithEvents Label72 As System.Windows.Forms.Label
    Friend WithEvents gridMisc As System.Windows.Forms.DataGridView
    Friend WithEvents txtMiscAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents gridMiscTotal As System.Windows.Forms.DataGridView
End Class
