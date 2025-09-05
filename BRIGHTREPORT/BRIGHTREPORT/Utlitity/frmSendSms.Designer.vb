<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSendSms
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
        Me.components = New System.ComponentModel.Container
        Me.Label1 = New System.Windows.Forms.Label
        Me.lstToMobileNo_OWN = New System.Windows.Forms.ListBox
        Me.lblMobileCnt = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtSenderId_OWN = New System.Windows.Forms.TextBox
        Me.LinkClear = New System.Windows.Forms.LinkLabel
        Me.LinkImport = New System.Windows.Forms.LinkLabel
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtMsg_OWN = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblSmsCount = New System.Windows.Forms.Label
        Me.btnSend = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LinkImportName = New System.Windows.Forms.LinkLabel
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbtSmsTable = New System.Windows.Forms.RadioButton
        Me.rbtWebBased = New System.Windows.Forms.RadioButton
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rbtPromo = New System.Windows.Forms.RadioButton
        Me.rbtGeneral = New System.Windows.Forms.RadioButton
        Me.LinkCustInfo = New System.Windows.Forms.LinkLabel
        Me.Label7 = New System.Windows.Forms.Label
        Me.CmbTemplate = New System.Windows.Forms.ComboBox
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(23, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Mobile No:"
        '
        'lstToMobileNo_OWN
        '
        Me.lstToMobileNo_OWN.FormattingEnabled = True
        Me.lstToMobileNo_OWN.Location = New System.Drawing.Point(107, 16)
        Me.lstToMobileNo_OWN.MultiColumn = True
        Me.lstToMobileNo_OWN.Name = "lstToMobileNo_OWN"
        Me.lstToMobileNo_OWN.Size = New System.Drawing.Size(272, 95)
        Me.lstToMobileNo_OWN.TabIndex = 1
        '
        'lblMobileCnt
        '
        Me.lblMobileCnt.AutoSize = True
        Me.lblMobileCnt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMobileCnt.Location = New System.Drawing.Point(107, 114)
        Me.lblMobileCnt.Name = "lblMobileCnt"
        Me.lblMobileCnt.Size = New System.Drawing.Size(84, 13)
        Me.lblMobileCnt.TabIndex = 2
        Me.lblMobileCnt.Text = "Total No's:0"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(23, 184)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(76, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Sender ID:"
        '
        'txtSenderId_OWN
        '
        Me.txtSenderId_OWN.Location = New System.Drawing.Point(107, 180)
        Me.txtSenderId_OWN.Name = "txtSenderId_OWN"
        Me.txtSenderId_OWN.Size = New System.Drawing.Size(272, 21)
        Me.txtSenderId_OWN.TabIndex = 10
        '
        'LinkClear
        '
        Me.LinkClear.AutoSize = True
        Me.LinkClear.Image = Global.BrighttechREPORT.My.Resources.Resources.Snowflake_222
        Me.LinkClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LinkClear.Location = New System.Drawing.Point(107, 131)
        Me.LinkClear.Name = "LinkClear"
        Me.LinkClear.Size = New System.Drawing.Size(77, 13)
        Me.LinkClear.TabIndex = 3
        Me.LinkClear.TabStop = True
        Me.LinkClear.Text = "    Clear List"
        '
        'LinkImport
        '
        Me.LinkImport.AutoSize = True
        Me.LinkImport.Image = Global.BrighttechREPORT.My.Resources.Resources.Excel
        Me.LinkImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LinkImport.Location = New System.Drawing.Point(188, 131)
        Me.LinkImport.Name = "LinkImport"
        Me.LinkImport.Size = New System.Drawing.Size(66, 13)
        Me.LinkImport.TabIndex = 4
        Me.LinkImport.TabStop = True
        Me.LinkImport.Text = "     Import"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(23, 209)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Message:"
        '
        'txtMsg_OWN
        '
        Me.txtMsg_OWN.Location = New System.Drawing.Point(107, 206)
        Me.txtMsg_OWN.MaxLength = 1000
        Me.txtMsg_OWN.Multiline = True
        Me.txtMsg_OWN.Name = "txtMsg_OWN"
        Me.txtMsg_OWN.Size = New System.Drawing.Size(271, 158)
        Me.txtMsg_OWN.TabIndex = 12
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(265, 370)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(114, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Max: 1000 Chars"
        '
        'lblSmsCount
        '
        Me.lblSmsCount.AutoSize = True
        Me.lblSmsCount.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSmsCount.Location = New System.Drawing.Point(107, 370)
        Me.lblSmsCount.Name = "lblSmsCount"
        Me.lblSmsCount.Size = New System.Drawing.Size(131, 13)
        Me.lblSmsCount.TabIndex = 13
        Me.lblSmsCount.Text = "0 Character(s),1 SMS"
        '
        'btnSend
        '
        Me.btnSend.Location = New System.Drawing.Point(107, 445)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(100, 30)
        Me.btnSend.TabIndex = 19
        Me.btnSend.Text = "Send"
        Me.btnSend.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(213, 445)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 20
        Me.btnExit.Text = "Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 26)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'LinkImportName
        '
        Me.LinkImportName.AutoSize = True
        Me.LinkImportName.Image = Global.BrighttechREPORT.My.Resources.Resources.Excel
        Me.LinkImportName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LinkImportName.Location = New System.Drawing.Point(251, 131)
        Me.LinkImportName.Name = "LinkImportName"
        Me.LinkImportName.Size = New System.Drawing.Size(128, 13)
        Me.LinkImportName.TabIndex = 5
        Me.LinkImportName.TabStop = True
        Me.LinkImportName.Text = "    Import With Name"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtSmsTable)
        Me.Panel1.Controls.Add(Me.rbtWebBased)
        Me.Panel1.Location = New System.Drawing.Point(107, 417)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(271, 25)
        Me.Panel1.TabIndex = 18
        '
        'rbtSmsTable
        '
        Me.rbtSmsTable.AutoSize = True
        Me.rbtSmsTable.Location = New System.Drawing.Point(117, 5)
        Me.rbtSmsTable.Name = "rbtSmsTable"
        Me.rbtSmsTable.Size = New System.Drawing.Size(133, 17)
        Me.rbtSmsTable.TabIndex = 1
        Me.rbtSmsTable.Text = "DataBase Services"
        Me.rbtSmsTable.UseVisualStyleBackColor = True
        '
        'rbtWebBased
        '
        Me.rbtWebBased.AutoSize = True
        Me.rbtWebBased.Checked = True
        Me.rbtWebBased.Location = New System.Drawing.Point(3, 5)
        Me.rbtWebBased.Name = "rbtWebBased"
        Me.rbtWebBased.Size = New System.Drawing.Size(103, 17)
        Me.rbtWebBased.TabIndex = 0
        Me.rbtWebBased.TabStop = True
        Me.rbtWebBased.Text = "Web Services"
        Me.rbtWebBased.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(23, 423)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Sms Via:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(23, 398)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(74, 13)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "Sms Type:"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtPromo)
        Me.Panel2.Controls.Add(Me.rbtGeneral)
        Me.Panel2.Location = New System.Drawing.Point(107, 391)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(271, 25)
        Me.Panel2.TabIndex = 16
        '
        'rbtPromo
        '
        Me.rbtPromo.AutoSize = True
        Me.rbtPromo.Location = New System.Drawing.Point(117, 5)
        Me.rbtPromo.Name = "rbtPromo"
        Me.rbtPromo.Size = New System.Drawing.Size(93, 17)
        Me.rbtPromo.TabIndex = 1
        Me.rbtPromo.Text = "Promotional"
        Me.rbtPromo.UseVisualStyleBackColor = True
        '
        'rbtGeneral
        '
        Me.rbtGeneral.AutoSize = True
        Me.rbtGeneral.Checked = True
        Me.rbtGeneral.Location = New System.Drawing.Point(3, 5)
        Me.rbtGeneral.Name = "rbtGeneral"
        Me.rbtGeneral.Size = New System.Drawing.Size(91, 17)
        Me.rbtGeneral.TabIndex = 0
        Me.rbtGeneral.TabStop = True
        Me.rbtGeneral.Text = "Transaction"
        Me.rbtGeneral.UseVisualStyleBackColor = True
        '
        'LinkCustInfo
        '
        Me.LinkCustInfo.AutoSize = True
        Me.LinkCustInfo.Image = Global.BrighttechREPORT.My.Resources.Resources.Excel
        Me.LinkCustInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LinkCustInfo.Location = New System.Drawing.Point(277, 157)
        Me.LinkCustInfo.Name = "LinkCustInfo"
        Me.LinkCustInfo.Size = New System.Drawing.Size(102, 13)
        Me.LinkCustInfo.TabIndex = 8
        Me.LinkCustInfo.TabStop = True
        Me.LinkCustInfo.Text = "    CustomerInfo"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(23, 158)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(72, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Template:"
        '
        'CmbTemplate
        '
        Me.CmbTemplate.FormattingEnabled = True
        Me.CmbTemplate.Items.AddRange(New Object() {"", "SEND RATE TO CUSTOMERS"})
        Me.CmbTemplate.Location = New System.Drawing.Point(107, 154)
        Me.CmbTemplate.Name = "CmbTemplate"
        Me.CmbTemplate.Size = New System.Drawing.Size(165, 21)
        Me.CmbTemplate.TabIndex = 7
        '
        'frmSendSms
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(390, 488)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.CmbTemplate)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.LinkCustInfo)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.LinkImportName)
        Me.Controls.Add(Me.btnSend)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.lblSmsCount)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtMsg_OWN)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.LinkImport)
        Me.Controls.Add(Me.LinkClear)
        Me.Controls.Add(Me.txtSenderId_OWN)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblMobileCnt)
        Me.Controls.Add(Me.lstToMobileNo_OWN)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmSendSms"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SendSms"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lstToMobileNo_OWN As System.Windows.Forms.ListBox
    Friend WithEvents lblMobileCnt As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtSenderId_OWN As System.Windows.Forms.TextBox
    Friend WithEvents LinkClear As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkImport As System.Windows.Forms.LinkLabel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtMsg_OWN As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblSmsCount As System.Windows.Forms.Label
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LinkImportName As System.Windows.Forms.LinkLabel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtSmsTable As System.Windows.Forms.RadioButton
    Friend WithEvents rbtWebBased As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtPromo As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGeneral As System.Windows.Forms.RadioButton
    Friend WithEvents LinkCustInfo As System.Windows.Forms.LinkLabel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents CmbTemplate As System.Windows.Forms.ComboBox
End Class
