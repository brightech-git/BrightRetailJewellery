<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LotMergeItemDet
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
        Me.grpAddress = New CodeVendor.Controls.Grouper
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtNetWt = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtPcs = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtTotWeight = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtStnPcs = New System.Windows.Forms.TextBox
        Me.txtStnWt = New System.Windows.Forms.TextBox
        Me.CmbItemCounter = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbSubItem_Man = New System.Windows.Forms.ComboBox
        Me.cmbItem_MAN = New System.Windows.Forms.ComboBox
        Me.cmbDesigner_MAN = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtDiaPcs = New System.Windows.Forms.TextBox
        Me.txtDiaWt = New System.Windows.Forms.TextBox
        Me.grpAddress.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpAddress
        '
        Me.grpAddress.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpAddress.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpAddress.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpAddress.BorderColor = System.Drawing.Color.Transparent
        Me.grpAddress.BorderThickness = 1.0!
        Me.grpAddress.Controls.Add(Me.Label10)
        Me.grpAddress.Controls.Add(Me.Label11)
        Me.grpAddress.Controls.Add(Me.txtDiaPcs)
        Me.grpAddress.Controls.Add(Me.txtDiaWt)
        Me.grpAddress.Controls.Add(Me.Label9)
        Me.grpAddress.Controls.Add(Me.txtNetWt)
        Me.grpAddress.Controls.Add(Me.Label5)
        Me.grpAddress.Controls.Add(Me.Label8)
        Me.grpAddress.Controls.Add(Me.txtPcs)
        Me.grpAddress.Controls.Add(Me.Label7)
        Me.grpAddress.Controls.Add(Me.txtTotWeight)
        Me.grpAddress.Controls.Add(Me.Label6)
        Me.grpAddress.Controls.Add(Me.txtStnPcs)
        Me.grpAddress.Controls.Add(Me.txtStnWt)
        Me.grpAddress.Controls.Add(Me.CmbItemCounter)
        Me.grpAddress.Controls.Add(Me.Label4)
        Me.grpAddress.Controls.Add(Me.cmbSubItem_Man)
        Me.grpAddress.Controls.Add(Me.cmbItem_MAN)
        Me.grpAddress.Controls.Add(Me.cmbDesigner_MAN)
        Me.grpAddress.Controls.Add(Me.Label3)
        Me.grpAddress.Controls.Add(Me.Label1)
        Me.grpAddress.Controls.Add(Me.Label2)
        Me.grpAddress.Controls.Add(Me.btnCancel)
        Me.grpAddress.Controls.Add(Me.btnOk)
        Me.grpAddress.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpAddress.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpAddress.GroupImage = Nothing
        Me.grpAddress.GroupTitle = ""
        Me.grpAddress.Location = New System.Drawing.Point(0, 0)
        Me.grpAddress.Name = "grpAddress"
        Me.grpAddress.Padding = New System.Windows.Forms.Padding(20)
        Me.grpAddress.PaintGroupBox = False
        Me.grpAddress.RoundCorners = 10
        Me.grpAddress.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpAddress.ShadowControl = False
        Me.grpAddress.ShadowThickness = 3
        Me.grpAddress.Size = New System.Drawing.Size(556, 260)
        Me.grpAddress.TabIndex = 0
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(482, 17)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(55, 14)
        Me.Label9.TabIndex = 12
        Me.Label9.Text = "NETWT"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtNetWt
        '
        Me.txtNetWt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNetWt.Location = New System.Drawing.Point(471, 35)
        Me.txtNetWt.MaxLength = 10
        Me.txtNetWt.Name = "txtNetWt"
        Me.txtNetWt.ShortcutsEnabled = False
        Me.txtNetWt.Size = New System.Drawing.Size(77, 22)
        Me.txtNetWt.TabIndex = 13
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(30, 17)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 14)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "PCS"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(248, 17)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(60, 14)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "STN WT"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPcs
        '
        Me.txtPcs.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPcs.Location = New System.Drawing.Point(9, 35)
        Me.txtPcs.MaxLength = 10
        Me.txtPcs.Name = "txtPcs"
        Me.txtPcs.ShortcutsEnabled = False
        Me.txtPcs.Size = New System.Drawing.Size(77, 22)
        Me.txtPcs.TabIndex = 1
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(169, 17)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(65, 14)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "STN PCS"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTotWeight
        '
        Me.txtTotWeight.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotWeight.Location = New System.Drawing.Point(86, 35)
        Me.txtTotWeight.MaxLength = 10
        Me.txtTotWeight.Name = "txtTotWeight"
        Me.txtTotWeight.ShortcutsEnabled = False
        Me.txtTotWeight.Size = New System.Drawing.Size(77, 22)
        Me.txtTotWeight.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(96, 17)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(57, 14)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "GRSWT"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtStnPcs
        '
        Me.txtStnPcs.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStnPcs.Location = New System.Drawing.Point(163, 35)
        Me.txtStnPcs.MaxLength = 10
        Me.txtStnPcs.Name = "txtStnPcs"
        Me.txtStnPcs.ShortcutsEnabled = False
        Me.txtStnPcs.Size = New System.Drawing.Size(77, 22)
        Me.txtStnPcs.TabIndex = 5
        '
        'txtStnWt
        '
        Me.txtStnWt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStnWt.Location = New System.Drawing.Point(240, 35)
        Me.txtStnWt.MaxLength = 10
        Me.txtStnWt.Name = "txtStnWt"
        Me.txtStnWt.ShortcutsEnabled = False
        Me.txtStnWt.Size = New System.Drawing.Size(77, 22)
        Me.txtStnWt.TabIndex = 7
        '
        'CmbItemCounter
        '
        Me.CmbItemCounter.FormattingEnabled = True
        Me.CmbItemCounter.Location = New System.Drawing.Point(289, 180)
        Me.CmbItemCounter.Name = "CmbItemCounter"
        Me.CmbItemCounter.Size = New System.Drawing.Size(198, 21)
        Me.CmbItemCounter.TabIndex = 21
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(174, 183)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(91, 15)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "ItemCounter"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbSubItem_Man
        '
        Me.cmbSubItem_Man.FormattingEnabled = True
        Me.cmbSubItem_Man.Location = New System.Drawing.Point(289, 108)
        Me.cmbSubItem_Man.Name = "cmbSubItem_Man"
        Me.cmbSubItem_Man.Size = New System.Drawing.Size(198, 21)
        Me.cmbSubItem_Man.TabIndex = 17
        '
        'cmbItem_MAN
        '
        Me.cmbItem_MAN.FormattingEnabled = True
        Me.cmbItem_MAN.Location = New System.Drawing.Point(289, 72)
        Me.cmbItem_MAN.Name = "cmbItem_MAN"
        Me.cmbItem_MAN.Size = New System.Drawing.Size(198, 21)
        Me.cmbItem_MAN.TabIndex = 15
        '
        'cmbDesigner_MAN
        '
        Me.cmbDesigner_MAN.FormattingEnabled = True
        Me.cmbDesigner_MAN.Location = New System.Drawing.Point(289, 144)
        Me.cmbDesigner_MAN.Name = "cmbDesigner_MAN"
        Me.cmbDesigner_MAN.Size = New System.Drawing.Size(198, 21)
        Me.cmbDesigner_MAN.TabIndex = 19
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(194, 147)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 15)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "Designer"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(154, 111)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(114, 15)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "SubItem Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(186, 75)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 15)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "ItemName"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(324, 213)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 23
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(216, 213)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 22
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(403, 17)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(58, 14)
        Me.Label10.TabIndex = 10
        Me.Label10.Text = "DIA WT"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(324, 17)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(63, 14)
        Me.Label11.TabIndex = 8
        Me.Label11.Text = "DIA PCS"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtDiaPcs
        '
        Me.txtDiaPcs.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiaPcs.Location = New System.Drawing.Point(317, 35)
        Me.txtDiaPcs.MaxLength = 10
        Me.txtDiaPcs.Name = "txtDiaPcs"
        Me.txtDiaPcs.ShortcutsEnabled = False
        Me.txtDiaPcs.Size = New System.Drawing.Size(77, 22)
        Me.txtDiaPcs.TabIndex = 9
        '
        'txtDiaWt
        '
        Me.txtDiaWt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiaWt.Location = New System.Drawing.Point(394, 35)
        Me.txtDiaWt.MaxLength = 10
        Me.txtDiaWt.Name = "txtDiaWt"
        Me.txtDiaWt.ShortcutsEnabled = False
        Me.txtDiaWt.Size = New System.Drawing.Size(77, 22)
        Me.txtDiaWt.TabIndex = 11
        '
        'LotMergeItemDet
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(556, 260)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAddress)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LotMergeItemDet"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LotMergeInfo"
        Me.grpAddress.ResumeLayout(False)
        Me.grpAddress.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpAddress As CodeVendor.Controls.Grouper
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbDesigner_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItem_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubItem_Man As System.Windows.Forms.ComboBox
    Friend WithEvents CmbItemCounter As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtNetWt As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtPcs As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtTotWeight As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtStnPcs As System.Windows.Forms.TextBox
    Friend WithEvents txtStnWt As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtDiaPcs As System.Windows.Forms.TextBox
    Friend WithEvents txtDiaWt As System.Windows.Forms.TextBox
End Class
