<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGSTTax_New
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.grpFinalDisc = New CodeVendor.Controls.Grouper
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtNetAmount_AMT = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbTdsCategory_OWN = New System.Windows.Forms.ComboBox
        Me.Label54 = New System.Windows.Forms.Label
        Me.Label49 = New System.Windows.Forms.Label
        Me.txtTdsAmt_AMT = New System.Windows.Forms.TextBox
        Me.txtTdsPer_PER = New System.Windows.Forms.TextBox
        Me.btnOk = New System.Windows.Forms.Button
        Me.txtCgst_per_AMT = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtSgst_per_AMT = New System.Windows.Forms.TextBox
        Me.txtIgst_per_AMT = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.lblFinalAmount = New System.Windows.Forms.Label
        Me.grpFinalDisc.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpFinalDisc
        '
        Me.grpFinalDisc.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpFinalDisc.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpFinalDisc.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpFinalDisc.BorderColor = System.Drawing.Color.Transparent
        Me.grpFinalDisc.BorderThickness = 1.0!
        Me.grpFinalDisc.Controls.Add(Me.Label3)
        Me.grpFinalDisc.Controls.Add(Me.txtNetAmount_AMT)
        Me.grpFinalDisc.Controls.Add(Me.Label1)
        Me.grpFinalDisc.Controls.Add(Me.cmbTdsCategory_OWN)
        Me.grpFinalDisc.Controls.Add(Me.Label54)
        Me.grpFinalDisc.Controls.Add(Me.Label49)
        Me.grpFinalDisc.Controls.Add(Me.txtTdsAmt_AMT)
        Me.grpFinalDisc.Controls.Add(Me.txtTdsPer_PER)
        Me.grpFinalDisc.Controls.Add(Me.btnOk)
        Me.grpFinalDisc.Controls.Add(Me.txtCgst_per_AMT)
        Me.grpFinalDisc.Controls.Add(Me.Label15)
        Me.grpFinalDisc.Controls.Add(Me.txtSgst_per_AMT)
        Me.grpFinalDisc.Controls.Add(Me.txtIgst_per_AMT)
        Me.grpFinalDisc.Controls.Add(Me.Label5)
        Me.grpFinalDisc.Controls.Add(Me.Label7)
        Me.grpFinalDisc.Controls.Add(Me.lblFinalAmount)
        Me.grpFinalDisc.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpFinalDisc.GroupImage = Nothing
        Me.grpFinalDisc.GroupTitle = ""
        Me.grpFinalDisc.Location = New System.Drawing.Point(6, -5)
        Me.grpFinalDisc.Name = "grpFinalDisc"
        Me.grpFinalDisc.Padding = New System.Windows.Forms.Padding(20)
        Me.grpFinalDisc.PaintGroupBox = False
        Me.grpFinalDisc.RoundCorners = 10
        Me.grpFinalDisc.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpFinalDisc.ShadowControl = False
        Me.grpFinalDisc.ShadowThickness = 3
        Me.grpFinalDisc.Size = New System.Drawing.Size(424, 217)
        Me.grpFinalDisc.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(21, 223)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 14)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Net Amount"
        Me.Label3.Visible = False
        '
        'txtNetAmount_AMT
        '
        Me.txtNetAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNetAmount_AMT.Location = New System.Drawing.Point(166, 216)
        Me.txtNetAmount_AMT.Name = "txtNetAmount_AMT"
        Me.txtNetAmount_AMT.Size = New System.Drawing.Size(136, 22)
        Me.txtNetAmount_AMT.TabIndex = 14
        Me.txtNetAmount_AMT.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(21, 142)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 14)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Tds Category"
        '
        'cmbTdsCategory_OWN
        '
        Me.cmbTdsCategory_OWN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTdsCategory_OWN.FormattingEnabled = True
        Me.cmbTdsCategory_OWN.Location = New System.Drawing.Point(167, 139)
        Me.cmbTdsCategory_OWN.Name = "cmbTdsCategory_OWN"
        Me.cmbTdsCategory_OWN.Size = New System.Drawing.Size(248, 22)
        Me.cmbTdsCategory_OWN.TabIndex = 8
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label54.Location = New System.Drawing.Point(21, 169)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(49, 14)
        Me.Label54.TabIndex = 9
        Me.Label54.Text = "Tds %"
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(21, 196)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(84, 14)
        Me.Label49.TabIndex = 11
        Me.Label49.Text = "Tds Amount"
        Me.Label49.Visible = False
        '
        'txtTdsAmt_AMT
        '
        Me.txtTdsAmt_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTdsAmt_AMT.Location = New System.Drawing.Point(167, 190)
        Me.txtTdsAmt_AMT.Name = "txtTdsAmt_AMT"
        Me.txtTdsAmt_AMT.Size = New System.Drawing.Size(136, 22)
        Me.txtTdsAmt_AMT.TabIndex = 12
        Me.txtTdsAmt_AMT.Visible = False
        '
        'txtTdsPer_PER
        '
        Me.txtTdsPer_PER.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTdsPer_PER.Location = New System.Drawing.Point(166, 165)
        Me.txtTdsPer_PER.Name = "txtTdsPer_PER"
        Me.txtTdsPer_PER.Size = New System.Drawing.Size(136, 22)
        Me.txtTdsPer_PER.TabIndex = 10
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(240, 304)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 15
        Me.btnOk.Text = "ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'txtCgst_per_AMT
        '
        Me.txtCgst_per_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCgst_per_AMT.Location = New System.Drawing.Point(168, 74)
        Me.txtCgst_per_AMT.Multiline = True
        Me.txtCgst_per_AMT.Name = "txtCgst_per_AMT"
        Me.txtCgst_per_AMT.Size = New System.Drawing.Size(89, 31)
        Me.txtCgst_per_AMT.TabIndex = 4
        Me.txtCgst_per_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label15
        '
        Me.Label15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label15.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(19, 43)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(146, 31)
        Me.Label15.TabIndex = 1
        Me.Label15.Text = "State GST %"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSgst_per_AMT
        '
        Me.txtSgst_per_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSgst_per_AMT.Location = New System.Drawing.Point(168, 43)
        Me.txtSgst_per_AMT.Multiline = True
        Me.txtSgst_per_AMT.Name = "txtSgst_per_AMT"
        Me.txtSgst_per_AMT.Size = New System.Drawing.Size(89, 31)
        Me.txtSgst_per_AMT.TabIndex = 2
        Me.txtSgst_per_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtIgst_per_AMT
        '
        Me.txtIgst_per_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIgst_per_AMT.Location = New System.Drawing.Point(168, 105)
        Me.txtIgst_per_AMT.Multiline = True
        Me.txtIgst_per_AMT.Name = "txtIgst_per_AMT"
        Me.txtIgst_per_AMT.Size = New System.Drawing.Size(89, 31)
        Me.txtIgst_per_AMT.TabIndex = 6
        Me.txtIgst_per_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(19, 105)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(146, 31)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Inter-State GST %"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label7.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(19, 74)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(146, 31)
        Me.Label7.TabIndex = 3
        Me.Label7.Text = "Central GST %"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblFinalAmount
        '
        Me.lblFinalAmount.BackColor = System.Drawing.Color.Transparent
        Me.lblFinalAmount.Font = New System.Drawing.Font("Verdana", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinalAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFinalAmount.Location = New System.Drawing.Point(5, 8)
        Me.lblFinalAmount.Name = "lblFinalAmount"
        Me.lblFinalAmount.Size = New System.Drawing.Size(266, 32)
        Me.lblFinalAmount.TabIndex = 0
        Me.lblFinalAmount.Text = "GST TAX"
        Me.lblFinalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmGSTTax
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(434, 216)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpFinalDisc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmGSTTax"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "GST"
        Me.grpFinalDisc.ResumeLayout(False)
        Me.grpFinalDisc.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpFinalDisc As CodeVendor.Controls.Grouper
    Friend WithEvents lblFinalAmount As System.Windows.Forms.Label
    Friend WithEvents txtCgst_per_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtSgst_per_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtIgst_per_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNetAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbTdsCategory_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents txtTdsAmt_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtTdsPer_PER As System.Windows.Forms.TextBox
End Class
