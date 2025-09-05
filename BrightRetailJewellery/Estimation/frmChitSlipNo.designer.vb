<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmchitslipNo
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
        Me.grpCHIT = New CodeVendor.Controls.Grouper
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtCHITCardRowIndex = New System.Windows.Forms.TextBox
        Me.gridCHITSLIP = New System.Windows.Forms.DataGridView
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.grpCHIT.SuspendLayout()
        CType(Me.gridCHITSLIP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpCHIT
        '
        Me.grpCHIT.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpCHIT.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpCHIT.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpCHIT.BorderColor = System.Drawing.Color.Transparent
        Me.grpCHIT.BorderThickness = 1.0!
        Me.grpCHIT.Controls.Add(Me.Label1)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardRowIndex)
        Me.grpCHIT.Controls.Add(Me.gridCHITSLIP)
        Me.grpCHIT.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpCHIT.GroupImage = Nothing
        Me.grpCHIT.GroupTitle = ""
        Me.grpCHIT.Location = New System.Drawing.Point(4, -5)
        Me.grpCHIT.Name = "grpCHIT"
        Me.grpCHIT.Padding = New System.Windows.Forms.Padding(20)
        Me.grpCHIT.PaintGroupBox = False
        Me.grpCHIT.RoundCorners = 10
        Me.grpCHIT.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpCHIT.ShadowControl = False
        Me.grpCHIT.ShadowThickness = 3
        Me.grpCHIT.Size = New System.Drawing.Size(363, 234)
        Me.grpCHIT.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(78, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 17)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Grp Code"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtCHITCardRowIndex
        '
        Me.txtCHITCardRowIndex.Location = New System.Drawing.Point(592, 41)
        Me.txtCHITCardRowIndex.Name = "txtCHITCardRowIndex"
        Me.txtCHITCardRowIndex.Size = New System.Drawing.Size(10, 21)
        Me.txtCHITCardRowIndex.TabIndex = 18
        Me.txtCHITCardRowIndex.Visible = False
        '
        'gridCHITSLIP
        '
        Me.gridCHITSLIP.AllowUserToAddRows = False
        Me.gridCHITSLIP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridCHITSLIP.ColumnHeadersVisible = False
        Me.gridCHITSLIP.Location = New System.Drawing.Point(6, 68)
        Me.gridCHITSLIP.MultiSelect = False
        Me.gridCHITSLIP.Name = "gridCHITSLIP"
        Me.gridCHITSLIP.ReadOnly = True
        Me.gridCHITSLIP.RowHeadersVisible = False
        Me.gridCHITSLIP.RowTemplate.Height = 20
        Me.gridCHITSLIP.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridCHITSLIP.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridCHITSLIP.Size = New System.Drawing.Size(343, 118)
        Me.gridCHITSLIP.TabIndex = 20
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(181, 232)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 7
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(84, 232)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 6
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'frmchitslipNo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(371, 265)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grpCHIT)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmchitslipNo"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Chit Slip Number"
        Me.grpCHIT.ResumeLayout(False)
        Me.grpCHIT.PerformLayout()
        CType(Me.gridCHITSLIP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpCHIT As CodeVendor.Controls.Grouper
    Friend WithEvents txtCHITCardRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents gridCHITSLIP As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
End Class
