<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGridSearch
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
        Me.cmbSearchKey = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtSearchText = New System.Windows.Forms.TextBox
        Me.btnFindNext = New System.Windows.Forms.Button
        Me.chkMatchWholeWord = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Search Key"
        '
        'cmbSearchKey
        '
        Me.cmbSearchKey.FormattingEnabled = True
        Me.cmbSearchKey.Location = New System.Drawing.Point(100, 17)
        Me.cmbSearchKey.Name = "cmbSearchKey"
        Me.cmbSearchKey.Size = New System.Drawing.Size(200, 21)
        Me.cmbSearchKey.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(21, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Search Text"
        '
        'txtSearchText
        '
        Me.txtSearchText.Location = New System.Drawing.Point(100, 53)
        Me.txtSearchText.Name = "txtSearchText"
        Me.txtSearchText.Size = New System.Drawing.Size(200, 21)
        Me.txtSearchText.TabIndex = 3
        '
        'btnFindNext
        '
        Me.btnFindNext.Location = New System.Drawing.Point(224, 113)
        Me.btnFindNext.Name = "btnFindNext"
        Me.btnFindNext.Size = New System.Drawing.Size(76, 26)
        Me.btnFindNext.TabIndex = 5
        Me.btnFindNext.Text = "Find Next"
        Me.btnFindNext.UseVisualStyleBackColor = True
        '
        'chkMatchWholeWord
        '
        Me.chkMatchWholeWord.AutoSize = True
        Me.chkMatchWholeWord.Location = New System.Drawing.Point(100, 90)
        Me.chkMatchWholeWord.Name = "chkMatchWholeWord"
        Me.chkMatchWholeWord.Size = New System.Drawing.Size(128, 17)
        Me.chkMatchWholeWord.TabIndex = 4
        Me.chkMatchWholeWord.Text = "Match whole word"
        Me.chkMatchWholeWord.UseVisualStyleBackColor = True
        '
        'frmGridSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(316, 151)
        Me.Controls.Add(Me.chkMatchWholeWord)
        Me.Controls.Add(Me.btnFindNext)
        Me.Controls.Add(Me.txtSearchText)
        Me.Controls.Add(Me.cmbSearchKey)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmGridSearch"
        Me.Text = "Search"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbSearchKey As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtSearchText As System.Windows.Forms.TextBox
    Friend WithEvents btnFindNext As System.Windows.Forms.Button
    Friend WithEvents chkMatchWholeWord As System.Windows.Forms.CheckBox
End Class
