<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_MANUALBILLNO
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
        Me.lblSale = New System.Windows.Forms.Label
        Me.txtSales_NUM = New System.Windows.Forms.TextBox
        Me.grpDiscount = New CodeVendor.Controls.Grouper
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtReturn_NUM = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtPurchase_NUM = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtReceipt_NUM = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtPayment_NUM = New System.Windows.Forms.TextBox
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.grpDiscount.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblSale
        '
        Me.lblSale.AutoSize = True
        Me.lblSale.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSale.Location = New System.Drawing.Point(24, 35)
        Me.lblSale.Name = "lblSale"
        Me.lblSale.Size = New System.Drawing.Size(47, 16)
        Me.lblSale.TabIndex = 0
        Me.lblSale.Text = "Sales"
        '
        'txtSales_NUM
        '
        Me.txtSales_NUM.Enabled = False
        Me.txtSales_NUM.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSales_NUM.Location = New System.Drawing.Point(145, 32)
        Me.txtSales_NUM.Name = "txtSales_NUM"
        Me.txtSales_NUM.Size = New System.Drawing.Size(86, 23)
        Me.txtSales_NUM.TabIndex = 1
        '
        'grpDiscount
        '
        Me.grpDiscount.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpDiscount.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpDiscount.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpDiscount.BorderColor = System.Drawing.Color.Transparent
        Me.grpDiscount.BorderThickness = 1.0!
        Me.grpDiscount.Controls.Add(Me.btnCancel)
        Me.grpDiscount.Controls.Add(Me.btnOk)
        Me.grpDiscount.Controls.Add(Me.txtPayment_NUM)
        Me.grpDiscount.Controls.Add(Me.Label5)
        Me.grpDiscount.Controls.Add(Me.txtReceipt_NUM)
        Me.grpDiscount.Controls.Add(Me.Label4)
        Me.grpDiscount.Controls.Add(Me.txtPurchase_NUM)
        Me.grpDiscount.Controls.Add(Me.Label3)
        Me.grpDiscount.Controls.Add(Me.txtReturn_NUM)
        Me.grpDiscount.Controls.Add(Me.Label2)
        Me.grpDiscount.Controls.Add(Me.txtSales_NUM)
        Me.grpDiscount.Controls.Add(Me.lblSale)
        Me.grpDiscount.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpDiscount.GroupImage = Nothing
        Me.grpDiscount.GroupTitle = ""
        Me.grpDiscount.Location = New System.Drawing.Point(6, -4)
        Me.grpDiscount.Name = "grpDiscount"
        Me.grpDiscount.Padding = New System.Windows.Forms.Padding(20)
        Me.grpDiscount.PaintGroupBox = False
        Me.grpDiscount.RoundCorners = 10
        Me.grpDiscount.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpDiscount.ShadowControl = False
        Me.grpDiscount.ShadowThickness = 3
        Me.grpDiscount.Size = New System.Drawing.Size(256, 236)
        Me.grpDiscount.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(24, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Return"
        '
        'txtReturn_NUM
        '
        Me.txtReturn_NUM.Enabled = False
        Me.txtReturn_NUM.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReturn_NUM.Location = New System.Drawing.Point(145, 63)
        Me.txtReturn_NUM.Name = "txtReturn_NUM"
        Me.txtReturn_NUM.Size = New System.Drawing.Size(86, 23)
        Me.txtReturn_NUM.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(24, 100)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 16)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Purchase"
        '
        'txtPurchase_NUM
        '
        Me.txtPurchase_NUM.Enabled = False
        Me.txtPurchase_NUM.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPurchase_NUM.Location = New System.Drawing.Point(145, 94)
        Me.txtPurchase_NUM.Name = "txtPurchase_NUM"
        Me.txtPurchase_NUM.Size = New System.Drawing.Size(86, 23)
        Me.txtPurchase_NUM.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(24, 131)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 16)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Receipt"
        '
        'txtReceipt_NUM
        '
        Me.txtReceipt_NUM.Enabled = False
        Me.txtReceipt_NUM.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceipt_NUM.Location = New System.Drawing.Point(145, 125)
        Me.txtReceipt_NUM.Name = "txtReceipt_NUM"
        Me.txtReceipt_NUM.Size = New System.Drawing.Size(86, 23)
        Me.txtReceipt_NUM.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(24, 161)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(73, 16)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Payment"
        '
        'txtPayment_NUM
        '
        Me.txtPayment_NUM.Enabled = False
        Me.txtPayment_NUM.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayment_NUM.Location = New System.Drawing.Point(145, 156)
        Me.txtPayment_NUM.Name = "txtPayment_NUM"
        Me.txtPayment_NUM.Size = New System.Drawing.Size(86, 23)
        Me.txtPayment_NUM.TabIndex = 9
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(27, 196)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 10
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(133, 196)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'FRM_MANUALBILLNO
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(267, 240)
        Me.Controls.Add(Me.grpDiscount)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FRM_MANUALBILLNO"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FRM_MANUALBILLNO"
        Me.grpDiscount.ResumeLayout(False)
        Me.grpDiscount.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblSale As System.Windows.Forms.Label
    Friend WithEvents txtSales_NUM As System.Windows.Forms.TextBox
    Friend WithEvents grpDiscount As CodeVendor.Controls.Grouper
    Friend WithEvents txtPurchase_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtReturn_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtPayment_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtReceipt_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
End Class
