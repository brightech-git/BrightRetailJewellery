<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAccBankDetails
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
        Me.txtChqNo = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpCheque = New CodeVendor.Controls.Grouper()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.CmbChqDetail_OWN = New System.Windows.Forms.ComboBox()
        Me.dtpBankDate = New BrighttechPack.DatePicker(Me.components)
        Me.grpCheque.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtChqNo
        '
        Me.txtChqNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtChqNo.Location = New System.Drawing.Point(85, 28)
        Me.txtChqNo.MaxLength = 30
        Me.txtChqNo.Name = "txtChqNo"
        Me.txtChqNo.Size = New System.Drawing.Size(298, 21)
        Me.txtChqNo.TabIndex = 1
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(12, 58)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(34, 13)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "Date"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(12, 31)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(70, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Cheque No"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 83)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Favour of"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpCheque
        '
        Me.grpCheque.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpCheque.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpCheque.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpCheque.BorderColor = System.Drawing.Color.Transparent
        Me.grpCheque.BorderThickness = 1.0!
        Me.grpCheque.Controls.Add(Me.btnRefresh)
        Me.grpCheque.Controls.Add(Me.CmbChqDetail_OWN)
        Me.grpCheque.Controls.Add(Me.dtpBankDate)
        Me.grpCheque.Controls.Add(Me.Label11)
        Me.grpCheque.Controls.Add(Me.Label1)
        Me.grpCheque.Controls.Add(Me.Label13)
        Me.grpCheque.Controls.Add(Me.txtChqNo)
        Me.grpCheque.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpCheque.GroupImage = Nothing
        Me.grpCheque.GroupTitle = ""
        Me.grpCheque.Location = New System.Drawing.Point(4, -4)
        Me.grpCheque.Name = "grpCheque"
        Me.grpCheque.Padding = New System.Windows.Forms.Padding(20)
        Me.grpCheque.PaintGroupBox = False
        Me.grpCheque.RoundCorners = 10
        Me.grpCheque.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpCheque.ShadowControl = False
        Me.grpCheque.ShadowThickness = 3
        Me.grpCheque.Size = New System.Drawing.Size(391, 116)
        Me.grpCheque.TabIndex = 6
        '
        'btnRefresh
        '
        Me.btnRefresh.Image = Global.BrighttechRetailJewellery.My.Resources.Resources.images_22
        Me.btnRefresh.Location = New System.Drawing.Point(366, 78)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(17, 23)
        Me.btnRefresh.TabIndex = 7
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'CmbChqDetail_OWN
        '
        Me.CmbChqDetail_OWN.FormattingEnabled = True
        Me.CmbChqDetail_OWN.Location = New System.Drawing.Point(85, 80)
        Me.CmbChqDetail_OWN.Name = "CmbChqDetail_OWN"
        Me.CmbChqDetail_OWN.Size = New System.Drawing.Size(283, 21)
        Me.CmbChqDetail_OWN.TabIndex = 6
        '
        'dtpBankDate
        '
        Me.dtpBankDate.Location = New System.Drawing.Point(85, 54)
        Me.dtpBankDate.Mask = "##/##/####"
        Me.dtpBankDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpBankDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpBankDate.Name = "dtpBankDate"
        Me.dtpBankDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpBankDate.Size = New System.Drawing.Size(75, 21)
        Me.dtpBankDate.TabIndex = 3
        Me.dtpBankDate.Text = "06/03/9998"
        Me.dtpBankDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'frmAccBankDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(396, 117)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpCheque)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmAccBankDetails"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cheque Detail"
        Me.grpCheque.ResumeLayout(False)
        Me.grpCheque.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtChqNo As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grpCheque As CodeVendor.Controls.Grouper
    Friend WithEvents dtpBankDate As BrighttechPack.DatePicker
    Friend WithEvents CmbChqDetail_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
End Class
