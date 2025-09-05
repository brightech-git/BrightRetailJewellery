<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRedeemPriviledge
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
        Me.lblPriviledge = New System.Windows.Forms.Label
        Me.grpPriviledge = New CodeVendor.Controls.Grouper
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.txtPoints_NUM = New System.Windows.Forms.TextBox
        Me.lblPoints = New System.Windows.Forms.Label
        Me.txtPriviledge = New System.Windows.Forms.TextBox
        Me.txtPValue_NUM = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtPAmount_NUM = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblCustomername = New System.Windows.Forms.Label
        Me.grpPriviledge.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblPriviledge
        '
        Me.lblPriviledge.AutoSize = True
        Me.lblPriviledge.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPriviledge.Location = New System.Drawing.Point(39, 35)
        Me.lblPriviledge.Name = "lblPriviledge"
        Me.lblPriviledge.Size = New System.Drawing.Size(99, 16)
        Me.lblPriviledge.TabIndex = 0
        Me.lblPriviledge.Text = "Priviledge Id"
        '
        'grpPriviledge
        '
        Me.grpPriviledge.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpPriviledge.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpPriviledge.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpPriviledge.BorderColor = System.Drawing.Color.Transparent
        Me.grpPriviledge.BorderThickness = 1.0!
        Me.grpPriviledge.Controls.Add(Me.lblCustomername)
        Me.grpPriviledge.Controls.Add(Me.txtPAmount_NUM)
        Me.grpPriviledge.Controls.Add(Me.Label2)
        Me.grpPriviledge.Controls.Add(Me.txtPValue_NUM)
        Me.grpPriviledge.Controls.Add(Me.Label1)
        Me.grpPriviledge.Controls.Add(Me.btnCancel)
        Me.grpPriviledge.Controls.Add(Me.btnOk)
        Me.grpPriviledge.Controls.Add(Me.txtPoints_NUM)
        Me.grpPriviledge.Controls.Add(Me.lblPoints)
        Me.grpPriviledge.Controls.Add(Me.txtPriviledge)
        Me.grpPriviledge.Controls.Add(Me.lblPriviledge)
        Me.grpPriviledge.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpPriviledge.GroupImage = Nothing
        Me.grpPriviledge.GroupTitle = ""
        Me.grpPriviledge.Location = New System.Drawing.Point(6, -4)
        Me.grpPriviledge.Name = "grpPriviledge"
        Me.grpPriviledge.Padding = New System.Windows.Forms.Padding(20)
        Me.grpPriviledge.PaintGroupBox = False
        Me.grpPriviledge.RoundCorners = 10
        Me.grpPriviledge.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpPriviledge.ShadowControl = False
        Me.grpPriviledge.ShadowThickness = 3
        Me.grpPriviledge.Size = New System.Drawing.Size(342, 239)
        Me.grpPriviledge.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(173, 193)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(67, 193)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 8
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'txtPoints_NUM
        '
        Me.txtPoints_NUM.Enabled = False
        Me.txtPoints_NUM.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPoints_NUM.Location = New System.Drawing.Point(171, 82)
        Me.txtPoints_NUM.Name = "txtPoints_NUM"
        Me.txtPoints_NUM.Size = New System.Drawing.Size(102, 23)
        Me.txtPoints_NUM.TabIndex = 3
        '
        'lblPoints
        '
        Me.lblPoints.AutoSize = True
        Me.lblPoints.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPoints.Location = New System.Drawing.Point(39, 88)
        Me.lblPoints.Name = "lblPoints"
        Me.lblPoints.Size = New System.Drawing.Size(92, 16)
        Me.lblPoints.TabIndex = 2
        Me.lblPoints.Text = "Discount %"
        '
        'txtPriviledge
        '
        Me.txtPriviledge.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPriviledge.Location = New System.Drawing.Point(171, 32)
        Me.txtPriviledge.Name = "txtPriviledge"
        Me.txtPriviledge.Size = New System.Drawing.Size(102, 23)
        Me.txtPriviledge.TabIndex = 1
        '
        'txtPValue_NUM
        '
        Me.txtPValue_NUM.Enabled = False
        Me.txtPValue_NUM.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPValue_NUM.Location = New System.Drawing.Point(171, 148)
        Me.txtPValue_NUM.Name = "txtPValue_NUM"
        Me.txtPValue_NUM.Size = New System.Drawing.Size(102, 23)
        Me.txtPValue_NUM.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(37, 152)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(132, 16)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Discount Amount"
        '
        'txtPAmount_NUM
        '
        Me.txtPAmount_NUM.Enabled = False
        Me.txtPAmount_NUM.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPAmount_NUM.Location = New System.Drawing.Point(171, 116)
        Me.txtPAmount_NUM.Name = "txtPAmount_NUM"
        Me.txtPAmount_NUM.Size = New System.Drawing.Size(102, 23)
        Me.txtPAmount_NUM.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(38, 119)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Amount"
        '
        'lblCustomername
        '
        Me.lblCustomername.AutoSize = True
        Me.lblCustomername.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCustomername.ForeColor = System.Drawing.Color.OrangeRed
        Me.lblCustomername.Location = New System.Drawing.Point(39, 64)
        Me.lblCustomername.Name = "lblCustomername"
        Me.lblCustomername.Size = New System.Drawing.Size(56, 16)
        Me.lblCustomername.TabIndex = 10
        Me.lblCustomername.Text = "Return"
        '
        'frmRedeemPriviledge
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(352, 246)
        Me.Controls.Add(Me.grpPriviledge)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmRedeemPriviledge"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Priviledge Redeem"
        Me.grpPriviledge.ResumeLayout(False)
        Me.grpPriviledge.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblPriviledge As System.Windows.Forms.Label
    Friend WithEvents grpPriviledge As CodeVendor.Controls.Grouper
    Friend WithEvents txtPoints_NUM As System.Windows.Forms.TextBox
    Friend WithEvents lblPoints As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents txtPriviledge As System.Windows.Forms.TextBox
    Friend WithEvents txtPAmount_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtPValue_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblCustomername As System.Windows.Forms.Label
End Class
