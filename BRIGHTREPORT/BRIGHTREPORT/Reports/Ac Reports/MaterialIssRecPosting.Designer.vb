<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MaterialIssRecPosting
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
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.cmbAcName = New System.Windows.Forms.ComboBox
        Me.AcName = New System.Windows.Forms.Label
        Me.CheckedComboBox2 = New BrighttechPack.CheckedComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Cmbcompany = New System.Windows.Forms.ComboBox
        Me.DtPicker = New GiriDatePicker.DatePicker(Me.components)
        Me.btnView = New System.Windows.Forms.Button
        Me.btnposting = New System.Windows.Forms.Button
        Me.Button8 = New System.Windows.Forms.Button
        Me.Button9 = New System.Windows.Forms.Button
        Me.Button10 = New System.Windows.Forms.Button
        Me.Label9 = New System.Windows.Forms.Label
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Panel3.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.cmbAcName)
        Me.Panel3.Controls.Add(Me.AcName)
        Me.Panel3.Controls.Add(Me.CheckedComboBox2)
        Me.Panel3.Controls.Add(Me.Label7)
        Me.Panel3.Controls.Add(Me.Label8)
        Me.Panel3.Controls.Add(Me.Cmbcompany)
        Me.Panel3.Controls.Add(Me.DtPicker)
        Me.Panel3.Controls.Add(Me.btnView)
        Me.Panel3.Controls.Add(Me.btnposting)
        Me.Panel3.Controls.Add(Me.Button8)
        Me.Panel3.Controls.Add(Me.Button9)
        Me.Panel3.Controls.Add(Me.Button10)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(907, 128)
        Me.Panel3.TabIndex = 0
        '
        'cmbAcName
        '
        Me.cmbAcName.FormattingEnabled = True
        Me.cmbAcName.Location = New System.Drawing.Point(95, 52)
        Me.cmbAcName.Name = "cmbAcName"
        Me.cmbAcName.Size = New System.Drawing.Size(260, 21)
        Me.cmbAcName.TabIndex = 3
        '
        'AcName
        '
        Me.AcName.AutoSize = True
        Me.AcName.Location = New System.Drawing.Point(32, 55)
        Me.AcName.Name = "AcName"
        Me.AcName.Size = New System.Drawing.Size(48, 13)
        Me.AcName.TabIndex = 2
        Me.AcName.Text = "AcName"
        '
        'CheckedComboBox2
        '
        Me.CheckedComboBox2.CheckOnClick = True
        Me.CheckedComboBox2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.CheckedComboBox2.DropDownHeight = 1
        Me.CheckedComboBox2.FormattingEnabled = True
        Me.CheckedComboBox2.IntegralHeight = False
        Me.CheckedComboBox2.Location = New System.Drawing.Point(464, 52)
        Me.CheckedComboBox2.Name = "CheckedComboBox2"
        Me.CheckedComboBox2.Size = New System.Drawing.Size(276, 21)
        Me.CheckedComboBox2.TabIndex = 7
        Me.CheckedComboBox2.ValueSeparator = ", "
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(385, 55)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(62, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Cost Centre"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(396, 19)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(51, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Company"
        '
        'Cmbcompany
        '
        Me.Cmbcompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cmbcompany.FormattingEnabled = True
        Me.Cmbcompany.Location = New System.Drawing.Point(464, 16)
        Me.Cmbcompany.Name = "Cmbcompany"
        Me.Cmbcompany.Size = New System.Drawing.Size(276, 21)
        Me.Cmbcompany.TabIndex = 1
        '
        'DtPicker
        '
        Me.DtPicker.Location = New System.Drawing.Point(96, 14)
        Me.DtPicker.Mask = "##-##-####"
        Me.DtPicker.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.DtPicker.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.DtPicker.Name = "DtPicker"
        Me.DtPicker.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.DtPicker.Size = New System.Drawing.Size(88, 20)
        Me.DtPicker.TabIndex = 5
        Me.DtPicker.Text = "01-03-9998"
        Me.DtPicker.Value = New Date(9998, 3, 1, 0, 0, 0, 0)
        '
        'btnView
        '
        Me.btnView.Location = New System.Drawing.Point(372, 88)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(96, 31)
        Me.btnView.TabIndex = 8
        Me.btnView.Text = "View [F2]"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'btnposting
        '
        Me.btnposting.Location = New System.Drawing.Point(678, 90)
        Me.btnposting.Name = "btnposting"
        Me.btnposting.Size = New System.Drawing.Size(96, 31)
        Me.btnposting.TabIndex = 10
        Me.btnposting.Text = "Posting"
        Me.btnposting.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(576, 90)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(96, 31)
        Me.Button8.TabIndex = 12
        Me.Button8.Text = "Exit [F12]"
        Me.Button8.UseVisualStyleBackColor = True
        Me.Button8.Visible = False
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(780, 88)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(96, 31)
        Me.Button9.TabIndex = 11
        Me.Button9.Text = "Print"
        Me.Button9.UseVisualStyleBackColor = True
        Me.Button9.Visible = False
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(474, 89)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(96, 31)
        Me.Button10.TabIndex = 9
        Me.Button10.Text = "New [F3]"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(32, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(30, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Date"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.DataGridView1.Location = New System.Drawing.Point(0, 127)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(907, 343)
        Me.DataGridView1.TabIndex = 1
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'MaterialIssRecPosting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(907, 470)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.DataGridView1)
        Me.Name = "MaterialIssRecPosting"
        Me.Text = "MaterialIssRecPosting"
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents CheckedComboBox2 As BrighttechPack.CheckedComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Cmbcompany As System.Windows.Forms.ComboBox
    Friend WithEvents DtPicker As GiriDatePicker.DatePicker
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents btnposting As System.Windows.Forms.Button
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cmbAcName As System.Windows.Forms.ComboBox
    Friend WithEvents AcName As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
End Class
