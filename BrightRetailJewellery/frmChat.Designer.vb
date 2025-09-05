<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChat
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
        Me.txtMsg = New System.Windows.Forms.TextBox
        Me.btnSend = New System.Windows.Forms.Button
        Me.chkLstUsers = New System.Windows.Forms.CheckedListBox
        Me.SuspendLayout()
        '
        'txtMsg
        '
        Me.txtMsg.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMsg.Location = New System.Drawing.Point(12, 199)
        Me.txtMsg.Multiline = True
        Me.txtMsg.Name = "txtMsg"
        Me.txtMsg.Size = New System.Drawing.Size(367, 66)
        Me.txtMsg.TabIndex = 1
        Me.txtMsg.Text = "Title Giritech Rate Updation"
        '
        'btnSend
        '
        Me.btnSend.Location = New System.Drawing.Point(290, 271)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(89, 32)
        Me.btnSend.TabIndex = 2
        Me.btnSend.Text = "&Send"
        Me.btnSend.UseVisualStyleBackColor = True
        '
        'chkLstUsers
        '
        Me.chkLstUsers.FormattingEnabled = True
        Me.chkLstUsers.Location = New System.Drawing.Point(12, 12)
        Me.chkLstUsers.Name = "chkLstUsers"
        Me.chkLstUsers.Size = New System.Drawing.Size(365, 180)
        Me.chkLstUsers.TabIndex = 0
        '
        'frmChat
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(389, 311)
        Me.Controls.Add(Me.chkLstUsers)
        Me.Controls.Add(Me.btnSend)
        Me.Controls.Add(Me.txtMsg)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "frmChat"
        Me.Text = "Messenger"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtMsg As System.Windows.Forms.TextBox
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents chkLstUsers As System.Windows.Forms.CheckedListBox
End Class
