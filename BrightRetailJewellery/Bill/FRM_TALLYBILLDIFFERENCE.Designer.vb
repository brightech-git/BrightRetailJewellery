<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_TALLYBILLDIFFERENCE
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
        Me.txttranno = New System.Windows.Forms.TextBox
        Me.grpDiscount = New CodeVendor.Controls.Grouper
        Me.txtSno = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbpaymode = New System.Windows.Forms.ComboBox
        Me.cmbmode = New System.Windows.Forms.ComboBox
        Me.cmbacname = New System.Windows.Forms.ComboBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtamt_NUM = New System.Windows.Forms.TextBox
        Me.grpDiscount.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblSale
        '
        Me.lblSale.AutoSize = True
        Me.lblSale.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSale.Location = New System.Drawing.Point(39, 59)
        Me.lblSale.Name = "lblSale"
        Me.lblSale.Size = New System.Drawing.Size(59, 16)
        Me.lblSale.TabIndex = 2
        Me.lblSale.Text = "TranNo"
        '
        'txttranno
        '
        Me.txttranno.Enabled = False
        Me.txttranno.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txttranno.Location = New System.Drawing.Point(132, 57)
        Me.txttranno.Name = "txttranno"
        Me.txttranno.Size = New System.Drawing.Size(121, 23)
        Me.txttranno.TabIndex = 3
        '
        'grpDiscount
        '
        Me.grpDiscount.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpDiscount.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpDiscount.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpDiscount.BorderColor = System.Drawing.Color.Transparent
        Me.grpDiscount.BorderThickness = 1.0!
        Me.grpDiscount.Controls.Add(Me.txtSno)
        Me.grpDiscount.Controls.Add(Me.Label1)
        Me.grpDiscount.Controls.Add(Me.cmbpaymode)
        Me.grpDiscount.Controls.Add(Me.cmbmode)
        Me.grpDiscount.Controls.Add(Me.cmbacname)
        Me.grpDiscount.Controls.Add(Me.btnCancel)
        Me.grpDiscount.Controls.Add(Me.btnOk)
        Me.grpDiscount.Controls.Add(Me.Label5)
        Me.grpDiscount.Controls.Add(Me.Label4)
        Me.grpDiscount.Controls.Add(Me.Label3)
        Me.grpDiscount.Controls.Add(Me.Label2)
        Me.grpDiscount.Controls.Add(Me.txtamt_NUM)
        Me.grpDiscount.Controls.Add(Me.txttranno)
        Me.grpDiscount.Controls.Add(Me.lblSale)
        Me.grpDiscount.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpDiscount.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpDiscount.GroupImage = Nothing
        Me.grpDiscount.GroupTitle = ""
        Me.grpDiscount.Location = New System.Drawing.Point(0, 0)
        Me.grpDiscount.Name = "grpDiscount"
        Me.grpDiscount.Padding = New System.Windows.Forms.Padding(20)
        Me.grpDiscount.PaintGroupBox = False
        Me.grpDiscount.RoundCorners = 10
        Me.grpDiscount.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpDiscount.ShadowControl = False
        Me.grpDiscount.ShadowThickness = 3
        Me.grpDiscount.Size = New System.Drawing.Size(568, 274)
        Me.grpDiscount.TabIndex = 0
        '
        'txtSno
        '
        Me.txtSno.Enabled = False
        Me.txtSno.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSno.Location = New System.Drawing.Point(132, 28)
        Me.txtSno.Name = "txtSno"
        Me.txtSno.Size = New System.Drawing.Size(121, 23)
        Me.txtSno.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(39, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Sno"
        '
        'cmbpaymode
        '
        Me.cmbpaymode.FormattingEnabled = True
        Me.cmbpaymode.Location = New System.Drawing.Point(132, 179)
        Me.cmbpaymode.Name = "cmbpaymode"
        Me.cmbpaymode.Size = New System.Drawing.Size(238, 21)
        Me.cmbpaymode.TabIndex = 11
        '
        'cmbmode
        '
        Me.cmbmode.FormattingEnabled = True
        Me.cmbmode.Items.AddRange(New Object() {"Credit", "Debit"})
        Me.cmbmode.Location = New System.Drawing.Point(132, 146)
        Me.cmbmode.Name = "cmbmode"
        Me.cmbmode.Size = New System.Drawing.Size(121, 21)
        Me.cmbmode.TabIndex = 9
        '
        'cmbacname
        '
        Me.cmbacname.FormattingEnabled = True
        Me.cmbacname.Location = New System.Drawing.Point(132, 87)
        Me.cmbacname.Name = "cmbacname"
        Me.cmbacname.Size = New System.Drawing.Size(413, 21)
        Me.cmbacname.TabIndex = 5
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(238, 215)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 13
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(132, 214)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 12
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(39, 175)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(74, 16)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "PayMode"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(39, 146)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 16)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Mode"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(39, 118)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 16)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Amount"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(39, 85)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "AcName"
        '
        'txtamt_NUM
        '
        Me.txtamt_NUM.Enabled = False
        Me.txtamt_NUM.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtamt_NUM.Location = New System.Drawing.Point(132, 115)
        Me.txtamt_NUM.Name = "txtamt_NUM"
        Me.txtamt_NUM.Size = New System.Drawing.Size(121, 23)
        Me.txtamt_NUM.TabIndex = 7
        '
        'FRM_TALLYBILLDIFFERENCE
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(568, 274)
        Me.Controls.Add(Me.grpDiscount)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FRM_TALLYBILLDIFFERENCE"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Account Change"
        Me.grpDiscount.ResumeLayout(False)
        Me.grpDiscount.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblSale As System.Windows.Forms.Label
    Friend WithEvents txttranno As System.Windows.Forms.TextBox
    Friend WithEvents grpDiscount As CodeVendor.Controls.Grouper
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents cmbmode As System.Windows.Forms.ComboBox
    Friend WithEvents cmbacname As System.Windows.Forms.ComboBox
    Friend WithEvents txtamt_NUM As System.Windows.Forms.TextBox
    Friend WithEvents cmbpaymode As System.Windows.Forms.ComboBox
    Friend WithEvents txtSno As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
