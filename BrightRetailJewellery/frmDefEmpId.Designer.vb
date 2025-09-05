<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDefEmpId
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
        Me.txtDefEmpId_NUM = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'txtDefEmpId_NUM
        '
        Me.txtDefEmpId_NUM.BackColor = System.Drawing.Color.LightGreen
        Me.txtDefEmpId_NUM.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDefEmpId_NUM.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDefEmpId_NUM.Location = New System.Drawing.Point(0, 0)
        Me.txtDefEmpId_NUM.Multiline = True
        Me.txtDefEmpId_NUM.Name = "txtDefEmpId_NUM"
        Me.txtDefEmpId_NUM.Size = New System.Drawing.Size(116, 32)
        Me.txtDefEmpId_NUM.TabIndex = 0
        Me.txtDefEmpId_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'frmDefEmpId
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(116, 32)
        Me.Controls.Add(Me.txtDefEmpId_NUM)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "frmDefEmpId"
        Me.Text = "Employee Id"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtDefEmpId_NUM As System.Windows.Forms.TextBox
End Class
