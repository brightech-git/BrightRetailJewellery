<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm4C
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
        Me.lblcut = New System.Windows.Forms.Label()
        Me.CmbCut = New System.Windows.Forms.ComboBox()
        Me.lblSetType = New System.Windows.Forms.Label()
        Me.cmbSetType = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtHeight_WET = New System.Windows.Forms.TextBox()
        Me.txtWidth_WET = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.lblShape = New System.Windows.Forms.Label()
        Me.cmbShape = New System.Windows.Forms.ComboBox()
        Me.lblClarity = New System.Windows.Forms.Label()
        Me.lblColor = New System.Windows.Forms.Label()
        Me.CmbClarity = New System.Windows.Forms.ComboBox()
        Me.CmbColor = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'lblcut
        '
        Me.lblcut.AutoSize = True
        Me.lblcut.Location = New System.Drawing.Point(4, 8)
        Me.lblcut.Name = "lblcut"
        Me.lblcut.Size = New System.Drawing.Size(27, 13)
        Me.lblcut.TabIndex = 0
        Me.lblcut.Text = "Cut"
        Me.lblcut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CmbCut
        '
        Me.CmbCut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbCut.FormattingEnabled = True
        Me.CmbCut.Location = New System.Drawing.Point(81, 4)
        Me.CmbCut.Name = "CmbCut"
        Me.CmbCut.Size = New System.Drawing.Size(170, 21)
        Me.CmbCut.TabIndex = 1
        '
        'lblSetType
        '
        Me.lblSetType.AutoSize = True
        Me.lblSetType.Location = New System.Drawing.Point(4, 106)
        Me.lblSetType.Name = "lblSetType"
        Me.lblSetType.Size = New System.Drawing.Size(74, 13)
        Me.lblSetType.TabIndex = 8
        Me.lblSetType.Text = "SettingType"
        Me.lblSetType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSetType
        '
        Me.cmbSetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSetType.FormattingEnabled = True
        Me.cmbSetType.Location = New System.Drawing.Point(81, 95)
        Me.cmbSetType.Name = "cmbSetType"
        Me.cmbSetType.Size = New System.Drawing.Size(170, 21)
        Me.cmbSetType.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 127)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(43, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Height"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtHeight_WET
        '
        Me.txtHeight_WET.Location = New System.Drawing.Point(81, 118)
        Me.txtHeight_WET.Name = "txtHeight_WET"
        Me.txtHeight_WET.Size = New System.Drawing.Size(62, 21)
        Me.txtHeight_WET.TabIndex = 11
        '
        'txtWidth_WET
        '
        Me.txtWidth_WET.Location = New System.Drawing.Point(187, 119)
        Me.txtWidth_WET.Name = "txtWidth_WET"
        Me.txtWidth_WET.Size = New System.Drawing.Size(62, 21)
        Me.txtWidth_WET.TabIndex = 13
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(146, 122)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Width"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(81, 142)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(62, 21)
        Me.btnOk.TabIndex = 14
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'lblShape
        '
        Me.lblShape.AutoSize = True
        Me.lblShape.Location = New System.Drawing.Point(4, 79)
        Me.lblShape.Name = "lblShape"
        Me.lblShape.Size = New System.Drawing.Size(43, 13)
        Me.lblShape.TabIndex = 6
        Me.lblShape.Text = "Shape"
        Me.lblShape.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbShape
        '
        Me.cmbShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbShape.FormattingEnabled = True
        Me.cmbShape.Location = New System.Drawing.Point(81, 72)
        Me.cmbShape.Name = "cmbShape"
        Me.cmbShape.Size = New System.Drawing.Size(170, 21)
        Me.cmbShape.TabIndex = 7
        '
        'lblClarity
        '
        Me.lblClarity.AutoSize = True
        Me.lblClarity.Location = New System.Drawing.Point(4, 55)
        Me.lblClarity.Name = "lblClarity"
        Me.lblClarity.Size = New System.Drawing.Size(45, 13)
        Me.lblClarity.TabIndex = 4
        Me.lblClarity.Text = "Clarity"
        Me.lblClarity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblColor
        '
        Me.lblColor.AutoSize = True
        Me.lblColor.Location = New System.Drawing.Point(4, 31)
        Me.lblColor.Name = "lblColor"
        Me.lblColor.Size = New System.Drawing.Size(38, 13)
        Me.lblColor.TabIndex = 2
        Me.lblColor.Text = "Color"
        Me.lblColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CmbClarity
        '
        Me.CmbClarity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbClarity.FormattingEnabled = True
        Me.CmbClarity.Location = New System.Drawing.Point(81, 49)
        Me.CmbClarity.Name = "CmbClarity"
        Me.CmbClarity.Size = New System.Drawing.Size(170, 21)
        Me.CmbClarity.TabIndex = 5
        '
        'CmbColor
        '
        Me.CmbColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbColor.FormattingEnabled = True
        Me.CmbColor.Location = New System.Drawing.Point(81, 26)
        Me.CmbColor.Name = "CmbColor"
        Me.CmbColor.Size = New System.Drawing.Size(170, 21)
        Me.CmbColor.TabIndex = 3
        '
        'frm4C
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(256, 166)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblShape)
        Me.Controls.Add(Me.cmbShape)
        Me.Controls.Add(Me.lblClarity)
        Me.Controls.Add(Me.lblColor)
        Me.Controls.Add(Me.CmbClarity)
        Me.Controls.Add(Me.CmbColor)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtWidth_WET)
        Me.Controls.Add(Me.txtHeight_WET)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblSetType)
        Me.Controls.Add(Me.cmbSetType)
        Me.Controls.Add(Me.lblcut)
        Me.Controls.Add(Me.CmbCut)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(481, 500)
        Me.MaximizeBox = False
        Me.Name = "frm4C"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Diamond Details"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblcut As System.Windows.Forms.Label
    Friend WithEvents CmbCut As System.Windows.Forms.ComboBox
    Friend WithEvents lblSetType As System.Windows.Forms.Label
    Friend WithEvents cmbSetType As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtHeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtWidth_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents lblShape As Label
    Friend WithEvents cmbShape As ComboBox
    Friend WithEvents lblClarity As Label
    Friend WithEvents lblColor As Label
    Friend WithEvents CmbClarity As ComboBox
    Friend WithEvents CmbColor As ComboBox
End Class
