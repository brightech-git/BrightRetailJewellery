<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSrAr_Dia
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
        Me.grpSrAr = New CodeVendor.Controls.Grouper
        Me.lblCostId = New System.Windows.Forms.Label
        Me.txtCostId = New System.Windows.Forms.TextBox
        Me.chkDate = New System.Windows.Forms.CheckBox
        Me.dtpSRBillDate = New BrighttechPack.DatePicker(Me.components)
        Me.txtSRBatchno = New System.Windows.Forms.TextBox
        Me.lblSRTitle = New System.Windows.Forms.Label
        Me.lblSRDate = New System.Windows.Forms.Label
        Me.lblSRNo = New System.Windows.Forms.Label
        Me.txtSRBillNo = New System.Windows.Forms.TextBox
        Me.grpSrAr.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpSrAr
        '
        Me.grpSrAr.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpSrAr.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpSrAr.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpSrAr.BorderColor = System.Drawing.Color.Transparent
        Me.grpSrAr.BorderThickness = 1.0!
        Me.grpSrAr.Controls.Add(Me.lblCostId)
        Me.grpSrAr.Controls.Add(Me.txtCostId)
        Me.grpSrAr.Controls.Add(Me.chkDate)
        Me.grpSrAr.Controls.Add(Me.dtpSRBillDate)
        Me.grpSrAr.Controls.Add(Me.txtSRBatchno)
        Me.grpSrAr.Controls.Add(Me.lblSRTitle)
        Me.grpSrAr.Controls.Add(Me.lblSRDate)
        Me.grpSrAr.Controls.Add(Me.lblSRNo)
        Me.grpSrAr.Controls.Add(Me.txtSRBillNo)
        Me.grpSrAr.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpSrAr.GroupImage = Nothing
        Me.grpSrAr.GroupTitle = ""
        Me.grpSrAr.Location = New System.Drawing.Point(6, -4)
        Me.grpSrAr.Name = "grpSrAr"
        Me.grpSrAr.Padding = New System.Windows.Forms.Padding(20)
        Me.grpSrAr.PaintGroupBox = False
        Me.grpSrAr.RoundCorners = 10
        Me.grpSrAr.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpSrAr.ShadowControl = False
        Me.grpSrAr.ShadowThickness = 3
        Me.grpSrAr.Size = New System.Drawing.Size(238, 148)
        Me.grpSrAr.TabIndex = 0
        '
        'lblCostId
        '
        Me.lblCostId.AutoSize = True
        Me.lblCostId.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCostId.Location = New System.Drawing.Point(49, 99)
        Me.lblCostId.Name = "lblCostId"
        Me.lblCostId.Size = New System.Drawing.Size(50, 14)
        Me.lblCostId.TabIndex = 4
        Me.lblCostId.Text = "CostId"
        Me.lblCostId.Visible = False
        '
        'txtCostId
        '
        Me.txtCostId.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCostId.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCostId.Location = New System.Drawing.Point(52, 116)
        Me.txtCostId.Name = "txtCostId"
        Me.txtCostId.Size = New System.Drawing.Size(43, 22)
        Me.txtCostId.TabIndex = 6
        Me.txtCostId.Visible = False
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Checked = True
        Me.chkDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDate.Location = New System.Drawing.Point(34, 77)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(15, 14)
        Me.chkDate.TabIndex = 2
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'dtpSRBillDate
        '
        Me.dtpSRBillDate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.dtpSRBillDate.Location = New System.Drawing.Point(52, 74)
        Me.dtpSRBillDate.Mask = "##/##/####"
        Me.dtpSRBillDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpSRBillDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpSRBillDate.Name = "dtpSRBillDate"
        Me.dtpSRBillDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpSRBillDate.Size = New System.Drawing.Size(114, 22)
        Me.dtpSRBillDate.TabIndex = 3
        Me.dtpSRBillDate.Text = "06/03/9998"
        Me.dtpSRBillDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'txtSRBatchno
        '
        Me.txtSRBatchno.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSRBatchno.Location = New System.Drawing.Point(176, 116)
        Me.txtSRBatchno.Name = "txtSRBatchno"
        Me.txtSRBatchno.Size = New System.Drawing.Size(43, 22)
        Me.txtSRBatchno.TabIndex = 8
        Me.txtSRBatchno.Visible = False
        '
        'lblSRTitle
        '
        Me.lblSRTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblSRTitle.Font = New System.Drawing.Font("Verdana", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSRTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSRTitle.Location = New System.Drawing.Point(6, 20)
        Me.lblSRTitle.Name = "lblSRTitle"
        Me.lblSRTitle.Size = New System.Drawing.Size(229, 32)
        Me.lblSRTitle.TabIndex = 0
        Me.lblSRTitle.Text = "APPROVAL RECEIPT"
        Me.lblSRTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSRDate
        '
        Me.lblSRDate.AutoSize = True
        Me.lblSRDate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSRDate.Location = New System.Drawing.Point(49, 57)
        Me.lblSRDate.Name = "lblSRDate"
        Me.lblSRDate.Size = New System.Drawing.Size(63, 14)
        Me.lblSRDate.TabIndex = 1
        Me.lblSRDate.Text = "Bill Date"
        '
        'lblSRNo
        '
        Me.lblSRNo.AutoSize = True
        Me.lblSRNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSRNo.Location = New System.Drawing.Point(96, 99)
        Me.lblSRNo.Name = "lblSRNo"
        Me.lblSRNo.Size = New System.Drawing.Size(50, 14)
        Me.lblSRNo.TabIndex = 5
        Me.lblSRNo.Text = "Bill No"
        '
        'txtSRBillNo
        '
        Me.txtSRBillNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSRBillNo.Location = New System.Drawing.Point(52, 116)
        Me.txtSRBillNo.Name = "txtSRBillNo"
        Me.txtSRBillNo.Size = New System.Drawing.Size(114, 22)
        Me.txtSRBillNo.TabIndex = 7
        '
        'frmSrAr_Dia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(249, 149)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpSrAr)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmSrAr_Dia"
        Me.Text = "frmSrAr_Dia"
        Me.grpSrAr.ResumeLayout(False)
        Me.grpSrAr.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpSrAr As CodeVendor.Controls.Grouper
    Friend WithEvents txtSRBatchno As System.Windows.Forms.TextBox
    Friend WithEvents lblSRTitle As System.Windows.Forms.Label
    Friend WithEvents lblSRDate As System.Windows.Forms.Label
    Friend WithEvents lblSRNo As System.Windows.Forms.Label
    Friend WithEvents txtSRBillNo As System.Windows.Forms.TextBox
    Friend WithEvents dtpSRBillDate As BrighttechPack.DatePicker
    Friend WithEvents chkDate As System.Windows.Forms.CheckBox
    Friend WithEvents txtCostId As System.Windows.Forms.TextBox
    Friend WithEvents lblCostId As System.Windows.Forms.Label
End Class
