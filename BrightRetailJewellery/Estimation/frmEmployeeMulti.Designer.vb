<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmEmployeeMulti
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkCmbEmployee = New BrighttechPack.CheckedComboBox()
        Me.txtEmpId = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(21, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(126, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Employee Name"
        '
        'chkCmbEmployee
        '
        Me.chkCmbEmployee.CheckOnClick = True
        Me.chkCmbEmployee.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbEmployee.DropDownHeight = 1
        Me.chkCmbEmployee.FormattingEnabled = True
        Me.chkCmbEmployee.IntegralHeight = False
        Me.chkCmbEmployee.Location = New System.Drawing.Point(153, 9)
        Me.chkCmbEmployee.Name = "chkCmbEmployee"
        Me.chkCmbEmployee.Size = New System.Drawing.Size(248, 22)
        Me.chkCmbEmployee.TabIndex = 1
        Me.chkCmbEmployee.ValueSeparator = ", "
        '
        'txtEmpId
        '
        Me.txtEmpId.Location = New System.Drawing.Point(153, 38)
        Me.txtEmpId.Name = "txtEmpId"
        Me.txtEmpId.ReadOnly = True
        Me.txtEmpId.Size = New System.Drawing.Size(248, 21)
        Me.txtEmpId.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(21, 39)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Emp Id"
        '
        'frmEmployeeMulti
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(402, 63)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtEmpId)
        Me.Controls.Add(Me.chkCmbEmployee)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmEmployeeMulti"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Employee Multi"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents chkCmbEmployee As BrighttechPack.CheckedComboBox
    Friend WithEvents txtEmpId As TextBox
    Friend WithEvents Label2 As Label
End Class
