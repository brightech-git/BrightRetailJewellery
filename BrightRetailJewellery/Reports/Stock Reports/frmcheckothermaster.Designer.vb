<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmcheckothermaster
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
        Me.chkothermisc = New BrighttechPack.CheckedComboBox
        Me.chkmiscname = New BrighttechPack.CheckedComboBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkothermisc
        '
        Me.chkothermisc.CheckOnClick = True
        Me.chkothermisc.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkothermisc.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkothermisc.DropDownHeight = 1
        Me.chkothermisc.FormattingEnabled = True
        Me.chkothermisc.IntegralHeight = False
        Me.chkothermisc.Location = New System.Drawing.Point(15, 55)
        Me.chkothermisc.Name = "chkothermisc"
        Me.chkothermisc.Size = New System.Drawing.Size(165, 22)
        Me.chkothermisc.TabIndex = 1
        Me.chkothermisc.ValueSeparator = ", "
        '
        'chkmiscname
        '
        Me.chkmiscname.CheckOnClick = True
        Me.chkmiscname.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkmiscname.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkmiscname.DropDownHeight = 1
        Me.chkmiscname.FormattingEnabled = True
        Me.chkmiscname.IntegralHeight = False
        Me.chkmiscname.Location = New System.Drawing.Point(15, 15)
        Me.chkmiscname.Name = "chkmiscname"
        Me.chkmiscname.Size = New System.Drawing.Size(165, 22)
        Me.chkmiscname.TabIndex = 0
        Me.chkmiscname.ValueSeparator = ", "
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkmiscname)
        Me.Panel1.Controls.Add(Me.chkothermisc)
        Me.Panel1.Location = New System.Drawing.Point(100, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(200, 100)
        Me.Panel1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Misc Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Others Name"
        '
        'frmcheckothermaster
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(327, 132)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmcheckothermaster"
        Me.ShowInTaskbar = False
        Me.Text = "Taged Other Details"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkothermisc As BrighttechPack.CheckedComboBox
    Friend WithEvents chkmiscname As BrighttechPack.CheckedComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
