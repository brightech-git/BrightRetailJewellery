<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_RedeemPriviledge
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
        Me.txtPAmount_NUM = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblValType = New System.Windows.Forms.Label
        Me.txtPValue_NUM = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.txtRPoints_AMT = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtValue_NUM = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtPoints_NUM = New System.Windows.Forms.TextBox
        Me.lblPoints = New System.Windows.Forms.Label
        Me.lblCustomername = New System.Windows.Forms.Label
        Me.txtPriviledge = New System.Windows.Forms.TextBox
        Me.grpPriviledge.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblPriviledge
        '
        Me.lblPriviledge.AutoSize = True
        Me.lblPriviledge.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPriviledge.Location = New System.Drawing.Point(10, 35)
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
        Me.grpPriviledge.Controls.Add(Me.txtPAmount_NUM)
        Me.grpPriviledge.Controls.Add(Me.Label2)
        Me.grpPriviledge.Controls.Add(Me.lblValType)
        Me.grpPriviledge.Controls.Add(Me.txtPValue_NUM)
        Me.grpPriviledge.Controls.Add(Me.Label1)
        Me.grpPriviledge.Controls.Add(Me.btnCancel)
        Me.grpPriviledge.Controls.Add(Me.btnOk)
        Me.grpPriviledge.Controls.Add(Me.txtRPoints_AMT)
        Me.grpPriviledge.Controls.Add(Me.Label5)
        Me.grpPriviledge.Controls.Add(Me.txtValue_NUM)
        Me.grpPriviledge.Controls.Add(Me.Label4)
        Me.grpPriviledge.Controls.Add(Me.txtPoints_NUM)
        Me.grpPriviledge.Controls.Add(Me.lblPoints)
        Me.grpPriviledge.Controls.Add(Me.lblCustomername)
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
        Me.grpPriviledge.Size = New System.Drawing.Size(342, 291)
        Me.grpPriviledge.TabIndex = 0
        '
        'txtPAmount_NUM
        '
        Me.txtPAmount_NUM.Enabled = False
        Me.txtPAmount_NUM.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPAmount_NUM.Location = New System.Drawing.Point(158, 187)
        Me.txtPAmount_NUM.Name = "txtPAmount_NUM"
        Me.txtPAmount_NUM.Size = New System.Drawing.Size(86, 23)
        Me.txtPAmount_NUM.TabIndex = 16
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(8, 220)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(147, 16)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Redeem In Amount"
        '
        'lblValType
        '
        Me.lblValType.AutoSize = True
        Me.lblValType.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValType.Location = New System.Drawing.Point(256, 128)
        Me.lblValType.Name = "lblValType"
        Me.lblValType.Size = New System.Drawing.Size(0, 16)
        Me.lblValType.TabIndex = 14
        '
        'txtPValue_NUM
        '
        Me.txtPValue_NUM.Enabled = False
        Me.txtPValue_NUM.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPValue_NUM.Location = New System.Drawing.Point(158, 216)
        Me.txtPValue_NUM.Name = "txtPValue_NUM"
        Me.txtPValue_NUM.Size = New System.Drawing.Size(86, 23)
        Me.txtPValue_NUM.TabIndex = 13
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 190)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(112, 16)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Redeem Value"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(131, 251)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(25, 251)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 10
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'txtRPoints_AMT
        '
        Me.txtRPoints_AMT.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRPoints_AMT.Location = New System.Drawing.Point(158, 156)
        Me.txtRPoints_AMT.Name = "txtRPoints_AMT"
        Me.txtRPoints_AMT.Size = New System.Drawing.Size(86, 23)
        Me.txtRPoints_AMT.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(10, 161)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(116, 16)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Redeem Points"
        '
        'txtValue_NUM
        '
        Me.txtValue_NUM.Enabled = False
        Me.txtValue_NUM.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtValue_NUM.Location = New System.Drawing.Point(158, 125)
        Me.txtValue_NUM.Name = "txtValue_NUM"
        Me.txtValue_NUM.Size = New System.Drawing.Size(86, 23)
        Me.txtValue_NUM.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(10, 131)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(49, 16)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Value"
        '
        'txtPoints_NUM
        '
        Me.txtPoints_NUM.Enabled = False
        Me.txtPoints_NUM.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPoints_NUM.Location = New System.Drawing.Point(158, 94)
        Me.txtPoints_NUM.Name = "txtPoints_NUM"
        Me.txtPoints_NUM.Size = New System.Drawing.Size(86, 23)
        Me.txtPoints_NUM.TabIndex = 5
        '
        'lblPoints
        '
        Me.lblPoints.AutoSize = True
        Me.lblPoints.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPoints.Location = New System.Drawing.Point(10, 100)
        Me.lblPoints.Name = "lblPoints"
        Me.lblPoints.Size = New System.Drawing.Size(53, 16)
        Me.lblPoints.TabIndex = 4
        Me.lblPoints.Text = "Points"
        '
        'lblCustomername
        '
        Me.lblCustomername.AutoSize = True
        Me.lblCustomername.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCustomername.ForeColor = System.Drawing.Color.OrangeRed
        Me.lblCustomername.Location = New System.Drawing.Point(10, 68)
        Me.lblCustomername.Name = "lblCustomername"
        Me.lblCustomername.Size = New System.Drawing.Size(56, 16)
        Me.lblCustomername.TabIndex = 2
        Me.lblCustomername.Text = "Return"
        '
        'txtPriviledge
        '
        Me.txtPriviledge.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPriviledge.Location = New System.Drawing.Point(142, 32)
        Me.txtPriviledge.Name = "txtPriviledge"
        Me.txtPriviledge.Size = New System.Drawing.Size(102, 23)
        Me.txtPriviledge.TabIndex = 1
        '
        'FRM_RedeemPriviledge
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(352, 289)
        Me.Controls.Add(Me.grpPriviledge)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FRM_RedeemPriviledge"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Redeem Priviledge"
        Me.grpPriviledge.ResumeLayout(False)
        Me.grpPriviledge.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblPriviledge As System.Windows.Forms.Label
    Friend WithEvents grpPriviledge As CodeVendor.Controls.Grouper
    Friend WithEvents txtPoints_NUM As System.Windows.Forms.TextBox
    Friend WithEvents lblPoints As System.Windows.Forms.Label
    Friend WithEvents lblCustomername As System.Windows.Forms.Label
    Friend WithEvents txtRPoints_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtValue_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents txtPValue_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPriviledge As System.Windows.Forms.TextBox
    Friend WithEvents lblValType As System.Windows.Forms.Label
    Friend WithEvents txtPAmount_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
