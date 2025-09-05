<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTrfNo
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtTrfNo = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtTrfNo
        '
        Me.txtTrfNo.BackColor = System.Drawing.Color.LightGreen
        Me.txtTrfNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTrfNo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtTrfNo.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTrfNo.Location = New System.Drawing.Point(0, 0)
        Me.txtTrfNo.Name = "txtTrfNo"
        Me.txtTrfNo.Size = New System.Drawing.Size(183, 26)
        Me.txtTrfNo.TabIndex = 0
        Me.txtTrfNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'frmTrfNo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(183, 32)
        Me.Controls.Add(Me.txtTrfNo)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "frmTrfNo"
        Me.Text = "Transfer No"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtTrfNo As System.Windows.Forms.TextBox
End Class
