<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChequeAdj
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
        Me.components = New System.ComponentModel.Container()
        Me.grpCheque = New CodeVendor.Controls.Grouper()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbMode = New System.Windows.Forms.ComboBox()
        Me.dtpChequeDate = New BrighttechPack.DatePicker(Me.components)
        Me.txtChequeRowIndex = New System.Windows.Forms.TextBox()
        Me.Label145 = New System.Windows.Forms.Label()
        Me.cmbChequeDefaultName = New System.Windows.Forms.ComboBox()
        Me.gridChequeTotal = New System.Windows.Forms.DataGridView()
        Me.Label137 = New System.Windows.Forms.Label()
        Me.Label136 = New System.Windows.Forms.Label()
        Me.Label138 = New System.Windows.Forms.Label()
        Me.Label139 = New System.Windows.Forms.Label()
        Me.txtChequeAmount_AMT = New System.Windows.Forms.TextBox()
        Me.txtChequeBankName = New System.Windows.Forms.TextBox()
        Me.txtChequeNo = New System.Windows.Forms.TextBox()
        Me.gridCheque = New System.Windows.Forms.DataGridView()
        Me.grpCheque.SuspendLayout()
        CType(Me.gridChequeTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridCheque, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpCheque
        '
        Me.grpCheque.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpCheque.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpCheque.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpCheque.BorderColor = System.Drawing.Color.Transparent
        Me.grpCheque.BorderThickness = 1.0!
        Me.grpCheque.Controls.Add(Me.Label1)
        Me.grpCheque.Controls.Add(Me.cmbMode)
        Me.grpCheque.Controls.Add(Me.dtpChequeDate)
        Me.grpCheque.Controls.Add(Me.txtChequeRowIndex)
        Me.grpCheque.Controls.Add(Me.Label145)
        Me.grpCheque.Controls.Add(Me.cmbChequeDefaultName)
        Me.grpCheque.Controls.Add(Me.gridChequeTotal)
        Me.grpCheque.Controls.Add(Me.Label137)
        Me.grpCheque.Controls.Add(Me.Label136)
        Me.grpCheque.Controls.Add(Me.Label138)
        Me.grpCheque.Controls.Add(Me.Label139)
        Me.grpCheque.Controls.Add(Me.txtChequeAmount_AMT)
        Me.grpCheque.Controls.Add(Me.txtChequeBankName)
        Me.grpCheque.Controls.Add(Me.txtChequeNo)
        Me.grpCheque.Controls.Add(Me.gridCheque)
        Me.grpCheque.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpCheque.GroupImage = Nothing
        Me.grpCheque.GroupTitle = ""
        Me.grpCheque.Location = New System.Drawing.Point(5, -4)
        Me.grpCheque.Name = "grpCheque"
        Me.grpCheque.Padding = New System.Windows.Forms.Padding(20)
        Me.grpCheque.PaintGroupBox = False
        Me.grpCheque.RoundCorners = 10
        Me.grpCheque.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpCheque.ShadowControl = False
        Me.grpCheque.ShadowThickness = 3
        Me.grpCheque.Size = New System.Drawing.Size(615, 199)
        Me.grpCheque.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(429, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(42, 14)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "&Mode"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMode
        '
        Me.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMode.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMode.FormattingEnabled = True
        Me.cmbMode.Items.AddRange(New Object() {"CHEQUE", "NEFT", "RTGS", "IMPS", "FUND TRANSFER"})
        Me.cmbMode.Location = New System.Drawing.Point(489, 17)
        Me.cmbMode.MaxDropDownItems = 50
        Me.cmbMode.Name = "cmbMode"
        Me.cmbMode.Size = New System.Drawing.Size(113, 22)
        Me.cmbMode.TabIndex = 2
        '
        'dtpChequeDate
        '
        Me.dtpChequeDate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.dtpChequeDate.Location = New System.Drawing.Point(219, 58)
        Me.dtpChequeDate.Mask = "##/##/####"
        Me.dtpChequeDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpChequeDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpChequeDate.Name = "dtpChequeDate"
        Me.dtpChequeDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpChequeDate.Size = New System.Drawing.Size(112, 22)
        Me.dtpChequeDate.TabIndex = 6
        Me.dtpChequeDate.Text = "07/03/9998"
        Me.dtpChequeDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'txtChequeRowIndex
        '
        Me.txtChequeRowIndex.Location = New System.Drawing.Point(588, 43)
        Me.txtChequeRowIndex.Name = "txtChequeRowIndex"
        Me.txtChequeRowIndex.Size = New System.Drawing.Size(23, 21)
        Me.txtChequeRowIndex.TabIndex = 11
        Me.txtChequeRowIndex.Visible = False
        '
        'Label145
        '
        Me.Label145.AutoSize = True
        Me.Label145.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label145.Location = New System.Drawing.Point(13, 20)
        Me.Label145.Name = "Label145"
        Me.Label145.Size = New System.Drawing.Size(92, 14)
        Me.Label145.TabIndex = 0
        Me.Label145.Text = "&Default Bank"
        Me.Label145.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbChequeDefaultName
        '
        Me.cmbChequeDefaultName.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbChequeDefaultName.FormattingEnabled = True
        Me.cmbChequeDefaultName.Location = New System.Drawing.Point(115, 17)
        Me.cmbChequeDefaultName.MaxDropDownItems = 50
        Me.cmbChequeDefaultName.Name = "cmbChequeDefaultName"
        Me.cmbChequeDefaultName.Size = New System.Drawing.Size(296, 22)
        Me.cmbChequeDefaultName.TabIndex = 0
        '
        'gridChequeTotal
        '
        Me.gridChequeTotal.AllowUserToAddRows = False
        Me.gridChequeTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridChequeTotal.Enabled = False
        Me.gridChequeTotal.Location = New System.Drawing.Point(13, 169)
        Me.gridChequeTotal.Name = "gridChequeTotal"
        Me.gridChequeTotal.ReadOnly = True
        Me.gridChequeTotal.Size = New System.Drawing.Size(590, 19)
        Me.gridChequeTotal.TabIndex = 12
        '
        'Label137
        '
        Me.Label137.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label137.Location = New System.Drawing.Point(485, 41)
        Me.Label137.Name = "Label137"
        Me.Label137.Size = New System.Drawing.Size(97, 14)
        Me.Label137.TabIndex = 9
        Me.Label137.Text = "AMOUNT"
        Me.Label137.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label136
        '
        Me.Label136.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label136.Location = New System.Drawing.Point(332, 41)
        Me.Label136.Name = "Label136"
        Me.Label136.Size = New System.Drawing.Size(152, 14)
        Me.Label136.TabIndex = 7
        Me.Label136.Text = "CHQ NO"
        Me.Label136.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label138
        '
        Me.Label138.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label138.Location = New System.Drawing.Point(219, 41)
        Me.Label138.Name = "Label138"
        Me.Label138.Size = New System.Drawing.Size(112, 14)
        Me.Label138.TabIndex = 5
        Me.Label138.Text = "DATE"
        Me.Label138.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label139
        '
        Me.Label139.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label139.Location = New System.Drawing.Point(13, 41)
        Me.Label139.Name = "Label139"
        Me.Label139.Size = New System.Drawing.Size(205, 14)
        Me.Label139.TabIndex = 3
        Me.Label139.Text = "BANK NAME"
        Me.Label139.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtChequeAmount_AMT
        '
        Me.txtChequeAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChequeAmount_AMT.Location = New System.Drawing.Point(485, 58)
        Me.txtChequeAmount_AMT.MaxLength = 12
        Me.txtChequeAmount_AMT.Name = "txtChequeAmount_AMT"
        Me.txtChequeAmount_AMT.Size = New System.Drawing.Size(97, 22)
        Me.txtChequeAmount_AMT.TabIndex = 10
        '
        'txtChequeBankName
        '
        Me.txtChequeBankName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtChequeBankName.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChequeBankName.Location = New System.Drawing.Point(13, 58)
        Me.txtChequeBankName.MaxLength = 50
        Me.txtChequeBankName.Name = "txtChequeBankName"
        Me.txtChequeBankName.Size = New System.Drawing.Size(205, 22)
        Me.txtChequeBankName.TabIndex = 4
        '
        'txtChequeNo
        '
        Me.txtChequeNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtChequeNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChequeNo.Location = New System.Drawing.Point(332, 58)
        Me.txtChequeNo.MaxLength = 20
        Me.txtChequeNo.Name = "txtChequeNo"
        Me.txtChequeNo.Size = New System.Drawing.Size(152, 22)
        Me.txtChequeNo.TabIndex = 8
        Me.txtChequeNo.Text = "12345678901234567890"
        '
        'gridCheque
        '
        Me.gridCheque.AllowUserToAddRows = False
        Me.gridCheque.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridCheque.ColumnHeadersVisible = False
        Me.gridCheque.Location = New System.Drawing.Point(13, 80)
        Me.gridCheque.Name = "gridCheque"
        Me.gridCheque.ReadOnly = True
        Me.gridCheque.RowHeadersVisible = False
        Me.gridCheque.RowTemplate.Height = 20
        Me.gridCheque.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridCheque.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridCheque.Size = New System.Drawing.Size(590, 89)
        Me.gridCheque.TabIndex = 10
        '
        'frmChequeAdj
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(625, 200)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpCheque)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmChequeAdj"
        Me.ShowInTaskbar = False
        Me.Text = "[Adjustment] Cheque"
        Me.grpCheque.ResumeLayout(False)
        Me.grpCheque.PerformLayout()
        CType(Me.gridChequeTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridCheque, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpCheque As CodeVendor.Controls.Grouper
    Friend WithEvents txtChequeRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents Label145 As System.Windows.Forms.Label
    Friend WithEvents cmbChequeDefaultName As System.Windows.Forms.ComboBox
    Friend WithEvents gridChequeTotal As System.Windows.Forms.DataGridView
    Friend WithEvents Label137 As System.Windows.Forms.Label
    Friend WithEvents Label136 As System.Windows.Forms.Label
    Friend WithEvents Label138 As System.Windows.Forms.Label
    Friend WithEvents Label139 As System.Windows.Forms.Label
    Friend WithEvents txtChequeAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtChequeBankName As System.Windows.Forms.TextBox
    Friend WithEvents txtChequeNo As System.Windows.Forms.TextBox
    Friend WithEvents gridCheque As System.Windows.Forms.DataGridView
    Friend WithEvents dtpChequeDate As BrighttechPack.DatePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbMode As ComboBox
End Class
