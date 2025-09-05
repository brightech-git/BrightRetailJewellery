<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRsUs
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtUSDollar_Amt = New System.Windows.Forms.TextBox
        Me.txtIndRs_Amt = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "US Dollar"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(185, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Indian Rs."
        '
        'txtUSDollar_Amt
        '
        Me.txtUSDollar_Amt.Location = New System.Drawing.Point(79, 8)
        Me.txtUSDollar_Amt.Name = "txtUSDollar_Amt"
        Me.txtUSDollar_Amt.Size = New System.Drawing.Size(100, 21)
        Me.txtUSDollar_Amt.TabIndex = 1
        Me.txtUSDollar_Amt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtIndRs_Amt
        '
        Me.txtIndRs_Amt.Location = New System.Drawing.Point(256, 8)
        Me.txtIndRs_Amt.Name = "txtIndRs_Amt"
        Me.txtIndRs_Amt.Size = New System.Drawing.Size(100, 21)
        Me.txtIndRs_Amt.TabIndex = 3
        Me.txtIndRs_Amt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmRsUs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(374, 42)
        Me.Controls.Add(Me.txtIndRs_Amt)
        Me.Controls.Add(Me.txtUSDollar_Amt)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmRsUs"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "US Dollar To Indian Rupees"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtUSDollar_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtIndRs_Amt As System.Windows.Forms.TextBox
End Class
