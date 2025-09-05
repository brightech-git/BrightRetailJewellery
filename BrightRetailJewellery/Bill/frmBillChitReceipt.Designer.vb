<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBillChitReceipt
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
        Me.grpChitReceipt = New CodeVendor.Controls.Grouper
        Me.txtRowIndex = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.lblRecvWt = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.lblRecvAmt = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.lblAddress = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.lblInsAmt = New System.Windows.Forms.Label
        Me.lblTotIns = New System.Windows.Forms.Label
        Me.lblPaid = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtTotAmount_AMT = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtNoOfIns_NUM = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtRegNo_NUM = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtGrpCode = New System.Windows.Forms.TextBox
        Me.grpChitReceipt.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpChitReceipt
        '
        Me.grpChitReceipt.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpChitReceipt.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpChitReceipt.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpChitReceipt.BorderColor = System.Drawing.Color.Transparent
        Me.grpChitReceipt.BorderThickness = 1.0!
        Me.grpChitReceipt.Controls.Add(Me.txtRowIndex)
        Me.grpChitReceipt.Controls.Add(Me.Label14)
        Me.grpChitReceipt.Controls.Add(Me.lblRecvWt)
        Me.grpChitReceipt.Controls.Add(Me.Label8)
        Me.grpChitReceipt.Controls.Add(Me.Label13)
        Me.grpChitReceipt.Controls.Add(Me.lblRecvAmt)
        Me.grpChitReceipt.Controls.Add(Me.Label7)
        Me.grpChitReceipt.Controls.Add(Me.Label12)
        Me.grpChitReceipt.Controls.Add(Me.lblAddress)
        Me.grpChitReceipt.Controls.Add(Me.Label9)
        Me.grpChitReceipt.Controls.Add(Me.Label11)
        Me.grpChitReceipt.Controls.Add(Me.lblName)
        Me.grpChitReceipt.Controls.Add(Me.Label4)
        Me.grpChitReceipt.Controls.Add(Me.Label22)
        Me.grpChitReceipt.Controls.Add(Me.Label17)
        Me.grpChitReceipt.Controls.Add(Me.Label10)
        Me.grpChitReceipt.Controls.Add(Me.lblInsAmt)
        Me.grpChitReceipt.Controls.Add(Me.lblTotIns)
        Me.grpChitReceipt.Controls.Add(Me.lblPaid)
        Me.grpChitReceipt.Controls.Add(Me.Label15)
        Me.grpChitReceipt.Controls.Add(Me.Label20)
        Me.grpChitReceipt.Controls.Add(Me.Label6)
        Me.grpChitReceipt.Controls.Add(Me.gridView)
        Me.grpChitReceipt.Controls.Add(Me.Label5)
        Me.grpChitReceipt.Controls.Add(Me.txtTotAmount_AMT)
        Me.grpChitReceipt.Controls.Add(Me.Label3)
        Me.grpChitReceipt.Controls.Add(Me.txtNoOfIns_NUM)
        Me.grpChitReceipt.Controls.Add(Me.Label2)
        Me.grpChitReceipt.Controls.Add(Me.txtRegNo_NUM)
        Me.grpChitReceipt.Controls.Add(Me.Label1)
        Me.grpChitReceipt.Controls.Add(Me.txtGrpCode)
        Me.grpChitReceipt.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpChitReceipt.GroupImage = Nothing
        Me.grpChitReceipt.GroupTitle = ""
        Me.grpChitReceipt.Location = New System.Drawing.Point(3, -6)
        Me.grpChitReceipt.Name = "grpChitReceipt"
        Me.grpChitReceipt.Padding = New System.Windows.Forms.Padding(20)
        Me.grpChitReceipt.PaintGroupBox = False
        Me.grpChitReceipt.RoundCorners = 10
        Me.grpChitReceipt.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpChitReceipt.ShadowControl = False
        Me.grpChitReceipt.ShadowThickness = 3
        Me.grpChitReceipt.Size = New System.Drawing.Size(591, 219)
        Me.grpChitReceipt.TabIndex = 0
        '
        'txtRowIndex
        '
        Me.txtRowIndex.Location = New System.Drawing.Point(335, 190)
        Me.txtRowIndex.Name = "txtRowIndex"
        Me.txtRowIndex.Size = New System.Drawing.Size(24, 21)
        Me.txtRowIndex.TabIndex = 9
        Me.txtRowIndex.Visible = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(398, 70)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(11, 13)
        Me.Label14.TabIndex = 4
        Me.Label14.Text = ":"
        '
        'lblRecvWt
        '
        Me.lblRecvWt.BackColor = System.Drawing.SystemColors.Window
        Me.lblRecvWt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecvWt.Location = New System.Drawing.Point(411, 70)
        Me.lblRecvWt.Name = "lblRecvWt"
        Me.lblRecvWt.Size = New System.Drawing.Size(66, 13)
        Me.lblRecvWt.TabIndex = 4
        Me.lblRecvWt.Text = "10.000"
        Me.lblRecvWt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(335, 70)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(59, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Recv Wt"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(398, 51)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(11, 13)
        Me.Label13.TabIndex = 4
        Me.Label13.Text = ":"
        '
        'lblRecvAmt
        '
        Me.lblRecvAmt.BackColor = System.Drawing.SystemColors.Window
        Me.lblRecvAmt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecvAmt.Location = New System.Drawing.Point(411, 51)
        Me.lblRecvAmt.Name = "lblRecvAmt"
        Me.lblRecvAmt.Size = New System.Drawing.Size(66, 13)
        Me.lblRecvAmt.TabIndex = 4
        Me.lblRecvAmt.Text = "1000.00"
        Me.lblRecvAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(335, 51)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(68, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Recv Amt"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(398, 108)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(11, 13)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = ":"
        '
        'lblAddress
        '
        Me.lblAddress.BackColor = System.Drawing.SystemColors.Window
        Me.lblAddress.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress.Location = New System.Drawing.Point(411, 108)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.Size = New System.Drawing.Size(173, 106)
        Me.lblAddress.TabIndex = 4
        Me.lblAddress.Text = "8A, Sarojini Street," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "T-Nagar," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Chennai - 600017" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Ph : 9894059321"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(335, 108)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(60, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Address"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(398, 89)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(11, 13)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = ":"
        '
        'lblName
        '
        Me.lblName.BackColor = System.Drawing.SystemColors.Window
        Me.lblName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.Location = New System.Drawing.Point(411, 89)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(173, 13)
        Me.lblName.TabIndex = 4
        Me.lblName.Text = "Sambath"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(335, 89)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(44, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Name"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(398, 33)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(11, 13)
        Me.Label22.TabIndex = 4
        Me.Label22.Text = ":"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(542, 32)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(11, 13)
        Me.Label17.TabIndex = 4
        Me.Label17.Text = ":"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(542, 50)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(11, 13)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = ":"
        '
        'lblInsAmt
        '
        Me.lblInsAmt.BackColor = System.Drawing.SystemColors.Window
        Me.lblInsAmt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsAmt.Location = New System.Drawing.Point(411, 33)
        Me.lblInsAmt.Name = "lblInsAmt"
        Me.lblInsAmt.Size = New System.Drawing.Size(66, 13)
        Me.lblInsAmt.TabIndex = 4
        Me.lblInsAmt.Text = "100.00"
        Me.lblInsAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTotIns
        '
        Me.lblTotIns.BackColor = System.Drawing.SystemColors.Window
        Me.lblTotIns.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotIns.Location = New System.Drawing.Point(551, 32)
        Me.lblTotIns.Name = "lblTotIns"
        Me.lblTotIns.Size = New System.Drawing.Size(34, 13)
        Me.lblTotIns.TabIndex = 4
        Me.lblTotIns.Text = "8"
        Me.lblTotIns.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPaid
        '
        Me.lblPaid.BackColor = System.Drawing.SystemColors.Window
        Me.lblPaid.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaid.Location = New System.Drawing.Point(551, 50)
        Me.lblPaid.Name = "lblPaid"
        Me.lblPaid.Size = New System.Drawing.Size(34, 13)
        Me.lblPaid.TabIndex = 4
        Me.lblPaid.Text = "8"
        Me.lblPaid.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(479, 32)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(67, 13)
        Me.Label15.TabIndex = 4
        Me.Label15.Text = "No Of Ins"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(335, 33)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(58, 13)
        Me.Label20.TabIndex = 4
        Me.Label20.Text = "Ins Amt"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(479, 50)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(35, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Paid"
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(6, 52)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(323, 163)
        Me.gridView.TabIndex = 8
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(242, 14)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(82, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Tot Amount"
        '
        'txtTotAmount_AMT
        '
        Me.txtTotAmount_AMT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotAmount_AMT.Location = New System.Drawing.Point(237, 30)
        Me.txtTotAmount_AMT.Name = "txtTotAmount_AMT"
        Me.txtTotAmount_AMT.Size = New System.Drawing.Size(92, 21)
        Me.txtTotAmount_AMT.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(172, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "No Of Ins"
        '
        'txtNoOfIns_NUM
        '
        Me.txtNoOfIns_NUM.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoOfIns_NUM.Location = New System.Drawing.Point(171, 30)
        Me.txtNoOfIns_NUM.Name = "txtNoOfIns_NUM"
        Me.txtNoOfIns_NUM.Size = New System.Drawing.Size(65, 21)
        Me.txtNoOfIns_NUM.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(102, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Reg No"
        '
        'txtRegNo_NUM
        '
        Me.txtRegNo_NUM.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRegNo_NUM.Location = New System.Drawing.Point(87, 30)
        Me.txtRegNo_NUM.Name = "txtRegNo_NUM"
        Me.txtRegNo_NUM.Size = New System.Drawing.Size(83, 21)
        Me.txtRegNo_NUM.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Group Code"
        '
        'txtGrpCode
        '
        Me.txtGrpCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGrpCode.Location = New System.Drawing.Point(6, 30)
        Me.txtGrpCode.Name = "txtGrpCode"
        Me.txtGrpCode.Size = New System.Drawing.Size(80, 21)
        Me.txtGrpCode.TabIndex = 1
        '
        'frmBillChitReceipt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(599, 217)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpChitReceipt)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmBillChitReceipt"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Scheme Receipt"
        Me.grpChitReceipt.ResumeLayout(False)
        Me.grpChitReceipt.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpChitReceipt As CodeVendor.Controls.Grouper
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNoOfIns_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtRegNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtGrpCode As System.Windows.Forms.TextBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtTotAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lblRecvWt As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents lblRecvAmt As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lblAddress As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblPaid As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents lblInsAmt As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lblTotIns As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
End Class
