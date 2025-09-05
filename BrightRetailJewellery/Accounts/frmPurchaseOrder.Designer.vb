<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPurchaseOrder
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
        Me.components = New System.ComponentModel.Container
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.cmbOItem_OWN = New System.Windows.Forms.ComboBox
        Me.cmbOSubItem_OWN = New System.Windows.Forms.ComboBox
        Me.txtOPcs_NUM = New System.Windows.Forms.TextBox
        Me.txtOGrsWt_WET = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtONetWt_WET = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtOLessWt_WET = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtORemark2 = New System.Windows.Forms.TextBox
        Me.txtORemark1 = New System.Windows.Forms.TextBox
        Me.Label61 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.dtptrandate = New BrighttechPack.DatePicker(Me.components)
        Me.Label2 = New System.Windows.Forms.Label
        Me.Grouper1 = New CodeVendor.Controls.Grouper
        Me.cmbAcName = New System.Windows.Forms.ComboBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.Grouper1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(16, 80)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Item"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(16, 107)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(54, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Subitem"
        '
        'cmbOItem_OWN
        '
        Me.cmbOItem_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbOItem_OWN.FormattingEnabled = True
        Me.cmbOItem_OWN.Location = New System.Drawing.Point(128, 76)
        Me.cmbOItem_OWN.Name = "cmbOItem_OWN"
        Me.cmbOItem_OWN.Size = New System.Drawing.Size(222, 21)
        Me.cmbOItem_OWN.TabIndex = 5
        '
        'cmbOSubItem_OWN
        '
        Me.cmbOSubItem_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbOSubItem_OWN.FormattingEnabled = True
        Me.cmbOSubItem_OWN.Location = New System.Drawing.Point(128, 103)
        Me.cmbOSubItem_OWN.Name = "cmbOSubItem_OWN"
        Me.cmbOSubItem_OWN.Size = New System.Drawing.Size(222, 21)
        Me.cmbOSubItem_OWN.TabIndex = 7
        '
        'txtOPcs_NUM
        '
        Me.txtOPcs_NUM.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOPcs_NUM.Location = New System.Drawing.Point(128, 128)
        Me.txtOPcs_NUM.Name = "txtOPcs_NUM"
        Me.txtOPcs_NUM.Size = New System.Drawing.Size(80, 21)
        Me.txtOPcs_NUM.TabIndex = 9
        '
        'txtOGrsWt_WET
        '
        Me.txtOGrsWt_WET.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOGrsWt_WET.Location = New System.Drawing.Point(128, 155)
        Me.txtOGrsWt_WET.Name = "txtOGrsWt_WET"
        Me.txtOGrsWt_WET.Size = New System.Drawing.Size(80, 21)
        Me.txtOGrsWt_WET.TabIndex = 11
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(16, 133)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(26, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Pcs"
        '
        'txtONetWt_WET
        '
        Me.txtONetWt_WET.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtONetWt_WET.Location = New System.Drawing.Point(128, 182)
        Me.txtONetWt_WET.Name = "txtONetWt_WET"
        Me.txtONetWt_WET.ReadOnly = True
        Me.txtONetWt_WET.Size = New System.Drawing.Size(80, 21)
        Me.txtONetWt_WET.TabIndex = 15
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(16, 160)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(83, 13)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Gross Weight"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(213, 160)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(51, 13)
        Me.Label9.TabIndex = 12
        Me.Label9.Text = "Less Wt"
        '
        'txtOLessWt_WET
        '
        Me.txtOLessWt_WET.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOLessWt_WET.Location = New System.Drawing.Point(272, 155)
        Me.txtOLessWt_WET.Name = "txtOLessWt_WET"
        Me.txtOLessWt_WET.ReadOnly = True
        Me.txtOLessWt_WET.Size = New System.Drawing.Size(80, 21)
        Me.txtOLessWt_WET.TabIndex = 13
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(16, 187)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(69, 13)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "Net Weight"
        '
        'txtORemark2
        '
        Me.txtORemark2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtORemark2.Location = New System.Drawing.Point(128, 231)
        Me.txtORemark2.MaxLength = 50
        Me.txtORemark2.Name = "txtORemark2"
        Me.txtORemark2.Size = New System.Drawing.Size(307, 21)
        Me.txtORemark2.TabIndex = 18
        '
        'txtORemark1
        '
        Me.txtORemark1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtORemark1.Location = New System.Drawing.Point(128, 207)
        Me.txtORemark1.MaxLength = 50
        Me.txtORemark1.Name = "txtORemark1"
        Me.txtORemark1.Size = New System.Drawing.Size(307, 21)
        Me.txtORemark1.TabIndex = 17
        '
        'Label61
        '
        Me.Label61.AutoSize = True
        Me.Label61.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label61.Location = New System.Drawing.Point(17, 213)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(52, 13)
        Me.Label61.TabIndex = 16
        Me.Label61.Text = "Remark"
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(229, 268)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 20
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Location = New System.Drawing.Point(124, 268)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 19
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'dtptrandate
        '
        Me.dtptrandate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtptrandate.Location = New System.Drawing.Point(128, 23)
        Me.dtptrandate.Mask = "##/##/####"
        Me.dtptrandate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtptrandate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtptrandate.Name = "dtptrandate"
        Me.dtptrandate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtptrandate.Size = New System.Drawing.Size(94, 21)
        Me.dtptrandate.TabIndex = 1
        Me.dtptrandate.Text = "06/03/9998"
        Me.dtptrandate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(16, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Tran Date"
        '
        'Grouper1
        '
        Me.Grouper1.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper1.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper1.BorderThickness = 1.0!
        Me.Grouper1.Controls.Add(Me.cmbAcName)
        Me.Grouper1.Controls.Add(Me.Label17)
        Me.Grouper1.Controls.Add(Me.cmbOItem_OWN)
        Me.Grouper1.Controls.Add(Me.cmbOSubItem_OWN)
        Me.Grouper1.Controls.Add(Me.dtptrandate)
        Me.Grouper1.Controls.Add(Me.Label7)
        Me.Grouper1.Controls.Add(Me.txtOGrsWt_WET)
        Me.Grouper1.Controls.Add(Me.Label2)
        Me.Grouper1.Controls.Add(Me.txtONetWt_WET)
        Me.Grouper1.Controls.Add(Me.Label5)
        Me.Grouper1.Controls.Add(Me.txtOPcs_NUM)
        Me.Grouper1.Controls.Add(Me.btnCancel)
        Me.Grouper1.Controls.Add(Me.Label8)
        Me.Grouper1.Controls.Add(Me.Label4)
        Me.Grouper1.Controls.Add(Me.Label61)
        Me.Grouper1.Controls.Add(Me.btnOk)
        Me.Grouper1.Controls.Add(Me.Label9)
        Me.Grouper1.Controls.Add(Me.Label10)
        Me.Grouper1.Controls.Add(Me.txtORemark1)
        Me.Grouper1.Controls.Add(Me.txtORemark2)
        Me.Grouper1.Controls.Add(Me.txtOLessWt_WET)
        Me.Grouper1.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Grouper1.GroupImage = Nothing
        Me.Grouper1.GroupTitle = ""
        Me.Grouper1.Location = New System.Drawing.Point(0, 0)
        Me.Grouper1.Name = "Grouper1"
        Me.Grouper1.Padding = New System.Windows.Forms.Padding(23, 20, 23, 20)
        Me.Grouper1.PaintGroupBox = False
        Me.Grouper1.RoundCorners = 10
        Me.Grouper1.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper1.ShadowControl = False
        Me.Grouper1.ShadowThickness = 3
        Me.Grouper1.Size = New System.Drawing.Size(457, 309)
        Me.Grouper1.TabIndex = 0
        '
        'cmbAcName
        '
        Me.cmbAcName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAcName.FormattingEnabled = True
        Me.cmbAcName.Location = New System.Drawing.Point(128, 50)
        Me.cmbAcName.Name = "cmbAcName"
        Me.cmbAcName.Size = New System.Drawing.Size(222, 21)
        Me.cmbAcName.TabIndex = 3
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(16, 53)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(58, 13)
        Me.Label17.TabIndex = 2
        Me.Label17.Text = "Ac Name"
        '
        'frmPurchaseOrder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(457, 309)
        Me.Controls.Add(Me.Grouper1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmPurchaseOrder"
        Me.Text = "Purchase Order Entry"
        Me.Grouper1.ResumeLayout(False)
        Me.Grouper1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbOItem_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOSubItem_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents txtOPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtOGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtONetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtOLessWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtORemark2 As System.Windows.Forms.TextBox
    Friend WithEvents txtORemark1 As System.Windows.Forms.TextBox
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents dtptrandate As BrighttechPack.DatePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Grouper1 As CodeVendor.Controls.Grouper
    Friend WithEvents cmbAcName As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
End Class
