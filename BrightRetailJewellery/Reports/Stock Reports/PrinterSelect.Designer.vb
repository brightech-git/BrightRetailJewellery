<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPrinterSelect
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
        Me.cmbPrinterName = New System.Windows.Forms.ComboBox
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.rdbInkJet = New System.Windows.Forms.RadioButton
        Me.rdbDotMatrix = New System.Windows.Forms.RadioButton
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Printer Name"
        '
        'cmbPrinterName
        '
        Me.cmbPrinterName.FormattingEnabled = True
        Me.cmbPrinterName.Location = New System.Drawing.Point(108, 9)
        Me.cmbPrinterName.Name = "cmbPrinterName"
        Me.cmbPrinterName.Size = New System.Drawing.Size(210, 21)
        Me.cmbPrinterName.TabIndex = 1
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(81, 80)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(75, 25)
        Me.btnPrint.TabIndex = 3
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(174, 80)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 25)
        Me.btnExit.TabIndex = 4
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rdbInkJet)
        Me.GroupBox2.Controls.Add(Me.rdbDotMatrix)
        Me.GroupBox2.Location = New System.Drawing.Point(108, 36)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(162, 32)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        '
        'rdbInkJet
        '
        Me.rdbInkJet.AutoSize = True
        Me.rdbInkJet.Location = New System.Drawing.Point(93, 11)
        Me.rdbInkJet.Name = "rdbInkJet"
        Me.rdbInkJet.Size = New System.Drawing.Size(64, 17)
        Me.rdbInkJet.TabIndex = 1
        Me.rdbInkJet.TabStop = True
        Me.rdbInkJet.Text = "Ink Jet"
        Me.rdbInkJet.UseVisualStyleBackColor = True
        '
        'rdbDotMatrix
        '
        Me.rdbDotMatrix.AutoSize = True
        Me.rdbDotMatrix.Location = New System.Drawing.Point(7, 11)
        Me.rdbDotMatrix.Name = "rdbDotMatrix"
        Me.rdbDotMatrix.Size = New System.Drawing.Size(84, 17)
        Me.rdbDotMatrix.TabIndex = 0
        Me.rdbDotMatrix.TabStop = True
        Me.rdbDotMatrix.Text = "Dot Matrix"
        Me.rdbDotMatrix.UseVisualStyleBackColor = True
        '
        'frmPrinterSelect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(341, 111)
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.cmbPrinterName)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmPrinterSelect"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Select Printer"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbPrinterName As System.Windows.Forms.ComboBox
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rdbInkJet As System.Windows.Forms.RadioButton
    Friend WithEvents rdbDotMatrix As System.Windows.Forms.RadioButton
End Class
