<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMc
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnSave = New System.Windows.Forms.Button
        Me.txtS_IGSTTax_Per = New System.Windows.Forms.TextBox
        Me.Label56 = New System.Windows.Forms.Label
        Me.txtS_CGSTTax_Per = New System.Windows.Forms.TextBox
        Me.cmbS_IGST_OWN = New System.Windows.Forms.ComboBox
        Me.txtS_SGSTTax_Per = New System.Windows.Forms.TextBox
        Me.Label57 = New System.Windows.Forms.Label
        Me.cmbS_CGST_OWN = New System.Windows.Forms.ComboBox
        Me.Label58 = New System.Windows.Forms.Label
        Me.cmbS_SGST_OWN = New System.Windows.Forms.ComboBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnSave)
        Me.GroupBox1.Controls.Add(Me.txtS_IGSTTax_Per)
        Me.GroupBox1.Controls.Add(Me.Label56)
        Me.GroupBox1.Controls.Add(Me.txtS_CGSTTax_Per)
        Me.GroupBox1.Controls.Add(Me.cmbS_IGST_OWN)
        Me.GroupBox1.Controls.Add(Me.txtS_SGSTTax_Per)
        Me.GroupBox1.Controls.Add(Me.Label57)
        Me.GroupBox1.Controls.Add(Me.cmbS_CGST_OWN)
        Me.GroupBox1.Controls.Add(Me.Label58)
        Me.GroupBox1.Controls.Add(Me.cmbS_SGST_OWN)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(456, 134)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(174, 98)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 44
        Me.btnSave.Text = "Ok"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'txtS_IGSTTax_Per
        '
        Me.txtS_IGSTTax_Per.Location = New System.Drawing.Point(104, 78)
        Me.txtS_IGSTTax_Per.MaxLength = 7
        Me.txtS_IGSTTax_Per.Name = "txtS_IGSTTax_Per"
        Me.txtS_IGSTTax_Per.Size = New System.Drawing.Size(65, 21)
        Me.txtS_IGSTTax_Per.TabIndex = 18
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Location = New System.Drawing.Point(10, 82)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(91, 13)
        Me.Label56.TabIndex = 17
        Me.Label56.Text = "Interstate GST"
        Me.Label56.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtS_CGSTTax_Per
        '
        Me.txtS_CGSTTax_Per.Location = New System.Drawing.Point(104, 50)
        Me.txtS_CGSTTax_Per.MaxLength = 7
        Me.txtS_CGSTTax_Per.Name = "txtS_CGSTTax_Per"
        Me.txtS_CGSTTax_Per.Size = New System.Drawing.Size(65, 21)
        Me.txtS_CGSTTax_Per.TabIndex = 15
        '
        'cmbS_IGST_OWN
        '
        Me.cmbS_IGST_OWN.FormattingEnabled = True
        Me.cmbS_IGST_OWN.Location = New System.Drawing.Point(174, 78)
        Me.cmbS_IGST_OWN.Name = "cmbS_IGST_OWN"
        Me.cmbS_IGST_OWN.Size = New System.Drawing.Size(265, 21)
        Me.cmbS_IGST_OWN.Sorted = True
        Me.cmbS_IGST_OWN.TabIndex = 19
        '
        'txtS_SGSTTax_Per
        '
        Me.txtS_SGSTTax_Per.Location = New System.Drawing.Point(104, 23)
        Me.txtS_SGSTTax_Per.MaxLength = 7
        Me.txtS_SGSTTax_Per.Name = "txtS_SGSTTax_Per"
        Me.txtS_SGSTTax_Per.Size = New System.Drawing.Size(65, 21)
        Me.txtS_SGSTTax_Per.TabIndex = 12
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.Location = New System.Drawing.Point(10, 55)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(77, 13)
        Me.Label57.TabIndex = 14
        Me.Label57.Text = "Central GST"
        Me.Label57.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbS_CGST_OWN
        '
        Me.cmbS_CGST_OWN.FormattingEnabled = True
        Me.cmbS_CGST_OWN.Location = New System.Drawing.Point(174, 50)
        Me.cmbS_CGST_OWN.Name = "cmbS_CGST_OWN"
        Me.cmbS_CGST_OWN.Size = New System.Drawing.Size(265, 21)
        Me.cmbS_CGST_OWN.Sorted = True
        Me.cmbS_CGST_OWN.TabIndex = 16
        '
        'Label58
        '
        Me.Label58.AutoSize = True
        Me.Label58.Location = New System.Drawing.Point(10, 27)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(65, 13)
        Me.Label58.TabIndex = 11
        Me.Label58.Text = "State GST"
        Me.Label58.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbS_SGST_OWN
        '
        Me.cmbS_SGST_OWN.FormattingEnabled = True
        Me.cmbS_SGST_OWN.Location = New System.Drawing.Point(174, 23)
        Me.cmbS_SGST_OWN.Name = "cmbS_SGST_OWN"
        Me.cmbS_SGST_OWN.Size = New System.Drawing.Size(265, 21)
        Me.cmbS_SGST_OWN.Sorted = True
        Me.cmbS_SGST_OWN.TabIndex = 13
        '
        'frmMc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(456, 134)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Viner Hand ITC", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmMc"
        Me.Text = "Making Charge Account"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtS_IGSTTax_Per As System.Windows.Forms.TextBox
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents txtS_CGSTTax_Per As System.Windows.Forms.TextBox
    Friend WithEvents cmbS_IGST_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents txtS_SGSTTax_Per As System.Windows.Forms.TextBox
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents cmbS_CGST_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents cmbS_SGST_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
End Class
