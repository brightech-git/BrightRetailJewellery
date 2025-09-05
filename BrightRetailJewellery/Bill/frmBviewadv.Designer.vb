<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBviewadv
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
        Me.grpWastageMc = New CodeVendor.Controls.Grouper
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtSearchString = New System.Windows.Forms.TextBox
        Me.rbtLike = New System.Windows.Forms.RadioButton
        Me.rbtExact = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbSearchKey = New System.Windows.Forms.ComboBox
        Me.cmbOGrsNet = New System.Windows.Forms.ComboBox
        Me.dtpBillDatet = New BrighttechPack.DatePicker(Me.components)
        Me.dtpBillDatef = New BrighttechPack.DatePicker(Me.components)
        Me.txtWt_To = New System.Windows.Forms.TextBox
        Me.Label49 = New System.Windows.Forms.Label
        Me.txtWt_From = New System.Windows.Forms.TextBox
        Me.chkDate = New System.Windows.Forms.CheckBox
        Me.grpWastageMc.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpWastageMc
        '
        Me.grpWastageMc.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpWastageMc.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpWastageMc.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpWastageMc.BorderColor = System.Drawing.Color.Transparent
        Me.grpWastageMc.BorderThickness = 1.0!
        Me.grpWastageMc.Controls.Add(Me.chkDate)
        Me.grpWastageMc.Controls.Add(Me.Label3)
        Me.grpWastageMc.Controls.Add(Me.txtSearchString)
        Me.grpWastageMc.Controls.Add(Me.rbtLike)
        Me.grpWastageMc.Controls.Add(Me.rbtExact)
        Me.grpWastageMc.Controls.Add(Me.Label1)
        Me.grpWastageMc.Controls.Add(Me.cmbSearchKey)
        Me.grpWastageMc.Controls.Add(Me.cmbOGrsNet)
        Me.grpWastageMc.Controls.Add(Me.dtpBillDatet)
        Me.grpWastageMc.Controls.Add(Me.dtpBillDatef)
        Me.grpWastageMc.Controls.Add(Me.txtWt_To)
        Me.grpWastageMc.Controls.Add(Me.Label49)
        Me.grpWastageMc.Controls.Add(Me.txtWt_From)
        Me.grpWastageMc.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpWastageMc.GroupImage = Nothing
        Me.grpWastageMc.GroupTitle = ""
        Me.grpWastageMc.Location = New System.Drawing.Point(4, -5)
        Me.grpWastageMc.Name = "grpWastageMc"
        Me.grpWastageMc.Padding = New System.Windows.Forms.Padding(20)
        Me.grpWastageMc.PaintGroupBox = False
        Me.grpWastageMc.RoundCorners = 10
        Me.grpWastageMc.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpWastageMc.ShadowControl = False
        Me.grpWastageMc.ShadowThickness = 3
        Me.grpWastageMc.Size = New System.Drawing.Size(361, 166)
        Me.grpWastageMc.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 135)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(97, 14)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Search String"
        '
        'txtSearchString
        '
        Me.txtSearchString.Location = New System.Drawing.Point(132, 133)
        Me.txtSearchString.Name = "txtSearchString"
        Me.txtSearchString.Size = New System.Drawing.Size(221, 21)
        Me.txtSearchString.TabIndex = 12
        '
        'rbtLike
        '
        Me.rbtLike.AutoSize = True
        Me.rbtLike.Checked = True
        Me.rbtLike.Location = New System.Drawing.Point(134, 110)
        Me.rbtLike.Name = "rbtLike"
        Me.rbtLike.Size = New System.Drawing.Size(48, 17)
        Me.rbtLike.TabIndex = 9
        Me.rbtLike.TabStop = True
        Me.rbtLike.Text = "Like"
        Me.rbtLike.UseVisualStyleBackColor = True
        '
        'rbtExact
        '
        Me.rbtExact.AutoSize = True
        Me.rbtExact.Location = New System.Drawing.Point(188, 110)
        Me.rbtExact.Name = "rbtExact"
        Me.rbtExact.Size = New System.Drawing.Size(56, 17)
        Me.rbtExact.TabIndex = 10
        Me.rbtExact.Text = "Exact"
        Me.rbtExact.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 90)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 14)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Search Key"
        '
        'cmbSearchKey
        '
        Me.cmbSearchKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSearchKey.FormattingEnabled = True
        Me.cmbSearchKey.Location = New System.Drawing.Point(132, 83)
        Me.cmbSearchKey.Name = "cmbSearchKey"
        Me.cmbSearchKey.Size = New System.Drawing.Size(221, 21)
        Me.cmbSearchKey.TabIndex = 8
        '
        'cmbOGrsNet
        '
        Me.cmbOGrsNet.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbOGrsNet.FormattingEnabled = True
        Me.cmbOGrsNet.Location = New System.Drawing.Point(60, 50)
        Me.cmbOGrsNet.Name = "cmbOGrsNet"
        Me.cmbOGrsNet.Size = New System.Drawing.Size(66, 22)
        Me.cmbOGrsNet.TabIndex = 4
        '
        'dtpBillDatet
        '
        Me.dtpBillDatet.Location = New System.Drawing.Point(250, 21)
        Me.dtpBillDatet.Mask = "##/##/####"
        Me.dtpBillDatet.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpBillDatet.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpBillDatet.Name = "dtpBillDatet"
        Me.dtpBillDatet.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpBillDatet.Size = New System.Drawing.Size(103, 21)
        Me.dtpBillDatet.TabIndex = 2
        Me.dtpBillDatet.Text = "06/03/9998"
        Me.dtpBillDatet.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpBillDatef
        '
        Me.dtpBillDatef.Location = New System.Drawing.Point(132, 21)
        Me.dtpBillDatef.Mask = "##/##/####"
        Me.dtpBillDatef.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpBillDatef.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpBillDatef.Name = "dtpBillDatef"
        Me.dtpBillDatef.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpBillDatef.Size = New System.Drawing.Size(103, 21)
        Me.dtpBillDatef.TabIndex = 1
        Me.dtpBillDatef.Text = "06/03/9998"
        Me.dtpBillDatef.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'txtWt_To
        '
        Me.txtWt_To.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWt_To.Location = New System.Drawing.Point(250, 50)
        Me.txtWt_To.Name = "txtWt_To"
        Me.txtWt_To.Size = New System.Drawing.Size(103, 22)
        Me.txtWt_To.TabIndex = 6
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(8, 53)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(54, 14)
        Me.Label49.TabIndex = 3
        Me.Label49.Text = "Weight"
        '
        'txtWt_From
        '
        Me.txtWt_From.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWt_From.Location = New System.Drawing.Point(132, 50)
        Me.txtWt_From.Name = "txtWt_From"
        Me.txtWt_From.Size = New System.Drawing.Size(103, 22)
        Me.txtWt_From.TabIndex = 5
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Checked = True
        Me.chkDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDate.Location = New System.Drawing.Point(11, 23)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(82, 18)
        Me.chkDate.TabIndex = 13
        Me.chkDate.Text = "Bill Date"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'frmBviewadv
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(369, 173)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpWastageMc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmBviewadv"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Advanced Search"
        Me.grpWastageMc.ResumeLayout(False)
        Me.grpWastageMc.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpWastageMc As CodeVendor.Controls.Grouper
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents txtWt_From As System.Windows.Forms.TextBox
    Friend WithEvents txtWt_To As System.Windows.Forms.TextBox
    Friend WithEvents dtpBillDatet As BrighttechPack.DatePicker
    Friend WithEvents dtpBillDatef As BrighttechPack.DatePicker
    Friend WithEvents cmbOGrsNet As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbSearchKey As System.Windows.Forms.ComboBox
    Friend WithEvents rbtLike As System.Windows.Forms.RadioButton
    Friend WithEvents rbtExact As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtSearchString As System.Windows.Forms.TextBox
    Friend WithEvents chkDate As System.Windows.Forms.CheckBox
End Class
