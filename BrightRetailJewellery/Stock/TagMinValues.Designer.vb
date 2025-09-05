<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TagMinValues
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
        Me.Label27 = New System.Windows.Forms.Label
        Me.txtMinMkCharge_Amt = New System.Windows.Forms.TextBox
        Me.Label26 = New System.Windows.Forms.Label
        Me.txtMinWastage_Wet = New System.Windows.Forms.TextBox
        Me.Label49 = New System.Windows.Forms.Label
        Me.Label48 = New System.Windows.Forms.Label
        Me.txtMinMcPerGram_Amt = New System.Windows.Forms.TextBox
        Me.txtMinWastage_Per = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(10, 81)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(99, 13)
        Me.Label27.TabIndex = 6
        Me.Label27.Text = "Min Mak Charge"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMinMkCharge_Amt
        '
        Me.txtMinMkCharge_Amt.Location = New System.Drawing.Point(135, 77)
        Me.txtMinMkCharge_Amt.Name = "txtMinMkCharge_Amt"
        Me.txtMinMkCharge_Amt.Size = New System.Drawing.Size(81, 21)
        Me.txtMinMkCharge_Amt.TabIndex = 7
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(10, 9)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(95, 13)
        Me.Label26.TabIndex = 0
        Me.Label26.Text = "Min Wastage %"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMinWastage_Wet
        '
        Me.txtMinWastage_Wet.Location = New System.Drawing.Point(135, 53)
        Me.txtMinWastage_Wet.Name = "txtMinWastage_Wet"
        Me.txtMinWastage_Wet.Size = New System.Drawing.Size(81, 21)
        Me.txtMinWastage_Wet.TabIndex = 5
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Location = New System.Drawing.Point(10, 33)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(104, 13)
        Me.Label49.TabIndex = 2
        Me.Label49.Text = "Min Mc Per Gram"
        Me.Label49.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label48
        '
        Me.Label48.AutoSize = True
        Me.Label48.Location = New System.Drawing.Point(10, 57)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(79, 13)
        Me.Label48.TabIndex = 4
        Me.Label48.Text = "Min Wastage"
        Me.Label48.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMinMcPerGram_Amt
        '
        Me.txtMinMcPerGram_Amt.Location = New System.Drawing.Point(135, 29)
        Me.txtMinMcPerGram_Amt.Name = "txtMinMcPerGram_Amt"
        Me.txtMinMcPerGram_Amt.Size = New System.Drawing.Size(81, 21)
        Me.txtMinMcPerGram_Amt.TabIndex = 3
        '
        'txtMinWastage_Per
        '
        Me.txtMinWastage_Per.Location = New System.Drawing.Point(135, 5)
        Me.txtMinWastage_Per.Name = "txtMinWastage_Per"
        Me.txtMinWastage_Per.Size = New System.Drawing.Size(81, 21)
        Me.txtMinWastage_Per.TabIndex = 1
        '
        'TagMinValues
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(227, 103)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label27)
        Me.Controls.Add(Me.txtMinMkCharge_Amt)
        Me.Controls.Add(Me.Label26)
        Me.Controls.Add(Me.txtMinWastage_Per)
        Me.Controls.Add(Me.txtMinWastage_Wet)
        Me.Controls.Add(Me.txtMinMcPerGram_Amt)
        Me.Controls.Add(Me.Label49)
        Me.Controls.Add(Me.Label48)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "TagMinValues"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TagMinValues"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents txtMinMkCharge_Amt As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents txtMinWastage_Wet As System.Windows.Forms.TextBox
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents txtMinMcPerGram_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtMinWastage_Per As System.Windows.Forms.TextBox
End Class
