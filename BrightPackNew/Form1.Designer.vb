<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.CheckedComboBox1 = New BrightPackNew.CheckedComboBoxNew()
        Me.SuspendLayout()
        '
        'CheckedComboBox1
        '
        Me.CheckedComboBox1.CheckOnClick = True
        Me.CheckedComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.CheckedComboBox1.DropDownHeight = 1
        Me.CheckedComboBox1.FormattingEnabled = True
        Me.CheckedComboBox1.IntegralHeight = False
        Me.CheckedComboBox1.Location = New System.Drawing.Point(12, 33)
        Me.CheckedComboBox1.Name = "CheckedComboBox1"
        Me.CheckedComboBox1.Size = New System.Drawing.Size(356, 21)
        Me.CheckedComboBox1.TabIndex = 0
        Me.CheckedComboBox1.ValueSeparator = ", "
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.CheckedComboBox1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents CheckedComboBox1 As CheckedComboBoxNew
End Class
