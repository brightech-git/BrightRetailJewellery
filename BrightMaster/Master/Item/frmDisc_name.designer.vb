<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDisc_Name
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
        Me.grpDisc = New CodeVendor.Controls.Grouper
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtDiscName = New System.Windows.Forms.TextBox
        Me.grpDisc.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpDisc
        '
        Me.grpDisc.BackgroundColor = System.Drawing.SystemColors.Control
        Me.grpDisc.BackgroundGradientColor = System.Drawing.SystemColors.Control
        Me.grpDisc.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpDisc.BorderColor = System.Drawing.Color.Transparent
        Me.grpDisc.BorderThickness = 1.0!
        Me.grpDisc.Controls.Add(Me.btnCancel)
        Me.grpDisc.Controls.Add(Me.btnOk)
        Me.grpDisc.Controls.Add(Me.Label2)
        Me.grpDisc.Controls.Add(Me.Label1)
        Me.grpDisc.Controls.Add(Me.dtpTo)
        Me.grpDisc.Controls.Add(Me.dtpFrom)
        Me.grpDisc.Controls.Add(Me.Label6)
        Me.grpDisc.Controls.Add(Me.txtDiscName)
        Me.grpDisc.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpDisc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpDisc.GroupImage = Nothing
        Me.grpDisc.GroupTitle = ""
        Me.grpDisc.Location = New System.Drawing.Point(0, 0)
        Me.grpDisc.Name = "grpDisc"
        Me.grpDisc.Padding = New System.Windows.Forms.Padding(20)
        Me.grpDisc.PaintGroupBox = False
        Me.grpDisc.RoundCorners = 10
        Me.grpDisc.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpDisc.ShadowControl = False
        Me.grpDisc.ShadowThickness = 3
        Me.grpDisc.Size = New System.Drawing.Size(400, 202)
        Me.grpDisc.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(153, 149)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(26, 149)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 8
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(23, 73)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Date From"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 106)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Date To"
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(137, 98)
        Me.dtpTo.Mask = "##-##-####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(82, 21)
        Me.dtpTo.TabIndex = 5
        Me.dtpTo.Text = "29/09/2010"
        Me.dtpTo.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(137, 65)
        Me.dtpFrom.Mask = "##-##-####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(82, 21)
        Me.dtpFrom.TabIndex = 3
        Me.dtpFrom.Text = "29/09/2010"
        Me.dtpFrom.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(23, 33)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(108, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Description Name"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDiscName
        '
        Me.txtDiscName.Location = New System.Drawing.Point(137, 30)
        Me.txtDiscName.MaxLength = 20
        Me.txtDiscName.Name = "txtDiscName"
        Me.txtDiscName.Size = New System.Drawing.Size(257, 21)
        Me.txtDiscName.TabIndex = 1
        '
        'frmDisc_Name
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(400, 202)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpDisc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDisc_Name"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DiscInfo"
        Me.grpDisc.ResumeLayout(False)
        Me.grpDisc.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpDisc As CodeVendor.Controls.Grouper
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtDiscName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
End Class
