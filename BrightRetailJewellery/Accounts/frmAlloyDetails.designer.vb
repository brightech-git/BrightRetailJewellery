<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAlloyDetails
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
        Me.txtAlloyWt_WET = New System.Windows.Forms.TextBox
        Me.gridAlloyView = New System.Windows.Forms.DataGridView
        Me.txtAlloy_MAN = New System.Windows.Forms.TextBox
        Me.Label71 = New System.Windows.Forms.Label
        Me.Label72 = New System.Windows.Forms.Label
        Me.grpAlloy = New CodeVendor.Controls.Grouper
        Me.gridAlloyTotal = New System.Windows.Forms.DataGridView
        Me.txtAlloyRowIndex = New System.Windows.Forms.TextBox
        CType(Me.gridAlloyView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpAlloy.SuspendLayout()
        CType(Me.gridAlloyTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtAlloyWt_WET
        '
        Me.txtAlloyWt_WET.Location = New System.Drawing.Point(220, 36)
        Me.txtAlloyWt_WET.Name = "txtAlloyWt_WET"
        Me.txtAlloyWt_WET.Size = New System.Drawing.Size(100, 21)
        Me.txtAlloyWt_WET.TabIndex = 7
        Me.txtAlloyWt_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'gridAlloyView
        '
        Me.gridAlloyView.AllowUserToAddRows = False
        Me.gridAlloyView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridAlloyView.Location = New System.Drawing.Point(9, 59)
        Me.gridAlloyView.Name = "gridAlloyView"
        Me.gridAlloyView.ReadOnly = True
        Me.gridAlloyView.Size = New System.Drawing.Size(322, 74)
        Me.gridAlloyView.TabIndex = 8
        '
        'txtAlloy_MAN
        '
        Me.txtAlloy_MAN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlloy_MAN.Location = New System.Drawing.Point(9, 36)
        Me.txtAlloy_MAN.Name = "txtAlloy_MAN"
        Me.txtAlloy_MAN.Size = New System.Drawing.Size(210, 21)
        Me.txtAlloy_MAN.TabIndex = 19
        '
        'Label71
        '
        Me.Label71.BackColor = System.Drawing.Color.Transparent
        Me.Label71.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label71.Location = New System.Drawing.Point(6, 18)
        Me.Label71.Name = "Label71"
        Me.Label71.Size = New System.Drawing.Size(213, 18)
        Me.Label71.TabIndex = 0
        Me.Label71.Text = "Alloy"
        Me.Label71.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label72
        '
        Me.Label72.BackColor = System.Drawing.Color.Transparent
        Me.Label72.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label72.Location = New System.Drawing.Point(221, 20)
        Me.Label72.Name = "Label72"
        Me.Label72.Size = New System.Drawing.Size(99, 15)
        Me.Label72.TabIndex = 2
        Me.Label72.Text = "Weight"
        Me.Label72.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpAlloy
        '
        Me.grpAlloy.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpAlloy.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpAlloy.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpAlloy.BorderColor = System.Drawing.Color.Transparent
        Me.grpAlloy.BorderThickness = 1.0!
        Me.grpAlloy.Controls.Add(Me.txtAlloyRowIndex)
        Me.grpAlloy.Controls.Add(Me.gridAlloyView)
        Me.grpAlloy.Controls.Add(Me.Label71)
        Me.grpAlloy.Controls.Add(Me.txtAlloy_MAN)
        Me.grpAlloy.Controls.Add(Me.Label72)
        Me.grpAlloy.Controls.Add(Me.txtAlloyWt_WET)
        Me.grpAlloy.Controls.Add(Me.gridAlloyTotal)
        Me.grpAlloy.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpAlloy.GroupImage = Nothing
        Me.grpAlloy.GroupTitle = ""
        Me.grpAlloy.Location = New System.Drawing.Point(2, -4)
        Me.grpAlloy.Name = "grpAlloy"
        Me.grpAlloy.Padding = New System.Windows.Forms.Padding(20)
        Me.grpAlloy.PaintGroupBox = False
        Me.grpAlloy.RoundCorners = 10
        Me.grpAlloy.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpAlloy.ShadowControl = False
        Me.grpAlloy.ShadowThickness = 3
        Me.grpAlloy.Size = New System.Drawing.Size(337, 158)
        Me.grpAlloy.TabIndex = 20
        '
        'gridAlloyTotal
        '
        Me.gridAlloyTotal.AllowUserToAddRows = False
        Me.gridAlloyTotal.AllowUserToDeleteRows = False
        Me.gridAlloyTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridAlloyTotal.ColumnHeadersVisible = False
        Me.gridAlloyTotal.Enabled = False
        Me.gridAlloyTotal.Location = New System.Drawing.Point(9, 133)
        Me.gridAlloyTotal.Name = "gridAlloyTotal"
        Me.gridAlloyTotal.ReadOnly = True
        Me.gridAlloyTotal.RowHeadersVisible = False
        Me.gridAlloyTotal.Size = New System.Drawing.Size(322, 19)
        Me.gridAlloyTotal.TabIndex = 5
        '
        'txtAlloyRowIndex
        '
        Me.txtAlloyRowIndex.Location = New System.Drawing.Point(147, 69)
        Me.txtAlloyRowIndex.Name = "txtAlloyRowIndex"
        Me.txtAlloyRowIndex.Size = New System.Drawing.Size(43, 21)
        Me.txtAlloyRowIndex.TabIndex = 34
        Me.txtAlloyRowIndex.Visible = False
        '
        'frmAlloyDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(341, 158)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAlloy)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmAlloyDetails"
        Me.Text = "Alloy Details"
        CType(Me.gridAlloyView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpAlloy.ResumeLayout(False)
        Me.grpAlloy.PerformLayout()
        CType(Me.gridAlloyTotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtAlloyWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents gridAlloyView As System.Windows.Forms.DataGridView
    Friend WithEvents txtAlloy_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label71 As System.Windows.Forms.Label
    Friend WithEvents Label72 As System.Windows.Forms.Label
    Friend WithEvents grpAlloy As CodeVendor.Controls.Grouper
    Friend WithEvents gridAlloyTotal As System.Windows.Forms.DataGridView
    Friend WithEvents txtAlloyRowIndex As System.Windows.Forms.TextBox
End Class
