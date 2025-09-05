<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHallmarkIssueReceiptDia
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
        Me.pnlItemCode = New System.Windows.Forms.Panel
        Me.txtItemCode_Num_Man = New System.Windows.Forms.TextBox
        Me.txtItemName = New System.Windows.Forms.TextBox
        Me.cmbSubItemName_Man = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.cmbDesigner_MAN = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label43 = New System.Windows.Forms.Label
        Me.txtNetWt_Wet = New System.Windows.Forms.TextBox
        Me.txtGrossWt_Wet = New System.Windows.Forms.TextBox
        Me.Label35 = New System.Windows.Forms.Label
        Me.Label37 = New System.Windows.Forms.Label
        Me.txtPieces_Num_Man = New System.Windows.Forms.TextBox
        Me.pnlItemCode.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlItemCode
        '
        Me.pnlItemCode.Controls.Add(Me.txtItemCode_Num_Man)
        Me.pnlItemCode.Controls.Add(Me.txtItemName)
        Me.pnlItemCode.Controls.Add(Me.cmbSubItemName_Man)
        Me.pnlItemCode.Controls.Add(Me.Label9)
        Me.pnlItemCode.Controls.Add(Me.Label8)
        Me.pnlItemCode.Location = New System.Drawing.Point(7, 31)
        Me.pnlItemCode.Name = "pnlItemCode"
        Me.pnlItemCode.Size = New System.Drawing.Size(440, 56)
        Me.pnlItemCode.TabIndex = 2
        '
        'txtItemCode_Num_Man
        '
        Me.txtItemCode_Num_Man.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.txtItemCode_Num_Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemCode_Num_Man.Location = New System.Drawing.Point(82, 3)
        Me.txtItemCode_Num_Man.MaxLength = 8
        Me.txtItemCode_Num_Man.Name = "txtItemCode_Num_Man"
        Me.txtItemCode_Num_Man.ReadOnly = True
        Me.txtItemCode_Num_Man.Size = New System.Drawing.Size(61, 21)
        Me.txtItemCode_Num_Man.TabIndex = 1
        '
        'txtItemName
        '
        Me.txtItemName.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.txtItemName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemName.Location = New System.Drawing.Point(151, 3)
        Me.txtItemName.MaxLength = 50
        Me.txtItemName.Name = "txtItemName"
        Me.txtItemName.ReadOnly = True
        Me.txtItemName.Size = New System.Drawing.Size(287, 21)
        Me.txtItemName.TabIndex = 2
        '
        'cmbSubItemName_Man
        '
        Me.cmbSubItemName_Man.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubItemName_Man.FormattingEnabled = True
        Me.cmbSubItemName_Man.Location = New System.Drawing.Point(82, 30)
        Me.cmbSubItemName_Man.Name = "cmbSubItemName_Man"
        Me.cmbSubItemName_Man.Size = New System.Drawing.Size(356, 21)
        Me.cmbSubItemName_Man.TabIndex = 4
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 33)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(60, 13)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "Sub Item"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 6)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Item Code"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(325, 144)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(117, 30)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(201, 144)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(117, 30)
        Me.btnOk.TabIndex = 9
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'cmbDesigner_MAN
        '
        Me.cmbDesigner_MAN.FormattingEnabled = True
        Me.cmbDesigner_MAN.Location = New System.Drawing.Point(89, 4)
        Me.cmbDesigner_MAN.Name = "cmbDesigner_MAN"
        Me.cmbDesigner_MAN.Size = New System.Drawing.Size(356, 21)
        Me.cmbDesigner_MAN.TabIndex = 1
        Me.cmbDesigner_MAN.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Dealer"
        Me.Label2.Visible = False
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Location = New System.Drawing.Point(13, 122)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(69, 13)
        Me.Label43.TabIndex = 7
        Me.Label43.Text = "Net Weight"
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtNetWt_Wet
        '
        Me.txtNetWt_Wet.BackColor = System.Drawing.SystemColors.Window
        Me.txtNetWt_Wet.Location = New System.Drawing.Point(89, 119)
        Me.txtNetWt_Wet.MaxLength = 10
        Me.txtNetWt_Wet.Name = "txtNetWt_Wet"
        Me.txtNetWt_Wet.Size = New System.Drawing.Size(132, 21)
        Me.txtNetWt_Wet.TabIndex = 8
        Me.txtNetWt_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtGrossWt_Wet
        '
        Me.txtGrossWt_Wet.BackColor = System.Drawing.SystemColors.Window
        Me.txtGrossWt_Wet.Location = New System.Drawing.Point(319, 93)
        Me.txtGrossWt_Wet.MaxLength = 10
        Me.txtGrossWt_Wet.Name = "txtGrossWt_Wet"
        Me.txtGrossWt_Wet.Size = New System.Drawing.Size(124, 21)
        Me.txtGrossWt_Wet.TabIndex = 6
        Me.txtGrossWt_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(14, 96)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(43, 13)
        Me.Label35.TabIndex = 3
        Me.Label35.Text = "Pieces"
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(229, 96)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(83, 13)
        Me.Label37.TabIndex = 5
        Me.Label37.Text = "Gross Weight"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPieces_Num_Man
        '
        Me.txtPieces_Num_Man.BackColor = System.Drawing.SystemColors.Window
        Me.txtPieces_Num_Man.Location = New System.Drawing.Point(89, 93)
        Me.txtPieces_Num_Man.MaxLength = 8
        Me.txtPieces_Num_Man.Name = "txtPieces_Num_Man"
        Me.txtPieces_Num_Man.Size = New System.Drawing.Size(132, 21)
        Me.txtPieces_Num_Man.TabIndex = 4
        Me.txtPieces_Num_Man.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmHallmarkIssueReceiptDia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(455, 179)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label43)
        Me.Controls.Add(Me.txtNetWt_Wet)
        Me.Controls.Add(Me.txtGrossWt_Wet)
        Me.Controls.Add(Me.Label35)
        Me.Controls.Add(Me.Label37)
        Me.Controls.Add(Me.txtPieces_Num_Man)
        Me.Controls.Add(Me.pnlItemCode)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.cmbDesigner_MAN)
        Me.Controls.Add(Me.Label2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmHallmarkIssueReceiptDia"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Hallmark Issue"
        Me.pnlItemCode.ResumeLayout(False)
        Me.pnlItemCode.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlItemCode As System.Windows.Forms.Panel
    Friend WithEvents txtItemCode_Num_Man As System.Windows.Forms.TextBox
    Friend WithEvents txtItemName As System.Windows.Forms.TextBox
    Friend WithEvents cmbSubItemName_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents cmbDesigner_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents txtNetWt_Wet As System.Windows.Forms.TextBox
    Friend WithEvents txtGrossWt_Wet As System.Windows.Forms.TextBox
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents txtPieces_Num_Man As System.Windows.Forms.TextBox
End Class
