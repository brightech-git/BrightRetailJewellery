<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DB_Detach
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
        Me.cmbDbName = New System.Windows.Forms.ComboBox
        Me.txtDestPath = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnBrowseDest = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnDetach = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'cmbDbName
        '
        Me.cmbDbName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDbName.FormattingEnabled = True
        Me.cmbDbName.Location = New System.Drawing.Point(25, 40)
        Me.cmbDbName.Name = "cmbDbName"
        Me.cmbDbName.Size = New System.Drawing.Size(228, 21)
        Me.cmbDbName.TabIndex = 0
        '
        'txtDestPath
        '
        Me.txtDestPath.Location = New System.Drawing.Point(25, 83)
        Me.txtDestPath.Name = "txtDestPath"
        Me.txtDestPath.Size = New System.Drawing.Size(320, 21)
        Me.txtDestPath.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Database Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(22, 67)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Destination Path"
        '
        'btnBrowseDest
        '
        Me.btnBrowseDest.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseDest.ForeColor = System.Drawing.Color.Red
        Me.btnBrowseDest.Location = New System.Drawing.Point(351, 83)
        Me.btnBrowseDest.Name = "btnBrowseDest"
        Me.btnBrowseDest.Size = New System.Drawing.Size(28, 23)
        Me.btnBrowseDest.TabIndex = 7
        Me.btnBrowseDest.Text = ".."
        Me.btnBrowseDest.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(131, 113)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 10
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnDetach
        '
        Me.btnDetach.Location = New System.Drawing.Point(25, 113)
        Me.btnDetach.Name = "btnDetach"
        Me.btnDetach.Size = New System.Drawing.Size(100, 30)
        Me.btnDetach.TabIndex = 9
        Me.btnDetach.Text = "&Detach"
        Me.btnDetach.UseVisualStyleBackColor = True
        '
        'Detach_DB
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(399, 161)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnDetach)
        Me.Controls.Add(Me.btnBrowseDest)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtDestPath)
        Me.Controls.Add(Me.cmbDbName)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Detach_DB"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Detach_DB"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbDbName As System.Windows.Forms.ComboBox
    Friend WithEvents txtDestPath As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnBrowseDest As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnDetach As System.Windows.Forms.Button
End Class
