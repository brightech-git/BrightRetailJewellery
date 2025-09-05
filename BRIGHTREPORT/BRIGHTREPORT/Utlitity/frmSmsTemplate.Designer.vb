<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSmsTemplate
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.btnExit = New System.Windows.Forms.Button
        Me.LABEL1 = New System.Windows.Forms.Label
        Me.btnSave = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.CmbTemplateName = New System.Windows.Forms.ComboBox
        Me.GrpContainer = New System.Windows.Forms.GroupBox
        Me.chkTamilFont = New System.Windows.Forms.CheckBox
        Me.CmbType = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbActive = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtTemplateDescription_OWN = New System.Windows.Forms.TextBox
        Me.ContextMenuStrip1.SuspendLayout()
        Me.GrpContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(178, 48)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.SaveToolStripMenuItem.Text = "Save                   "
        Me.SaveToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.ExitToolStripMenuItem.Text = "Exit                    "
        Me.ExitToolStripMenuItem.Visible = False
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(248, 254)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 10
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'LABEL1
        '
        Me.LABEL1.AutoSize = True
        Me.LABEL1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LABEL1.Location = New System.Drawing.Point(15, 48)
        Me.LABEL1.Name = "LABEL1"
        Me.LABEL1.Size = New System.Drawing.Size(93, 13)
        Me.LABEL1.TabIndex = 0
        Me.LABEL1.Text = "TemplateName"
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(141, 254)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 9
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(15, 78)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(124, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "TemplateDescription"
        '
        'CmbTemplateName
        '
        Me.CmbTemplateName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbTemplateName.FormattingEnabled = True
        Me.CmbTemplateName.Location = New System.Drawing.Point(143, 44)
        Me.CmbTemplateName.Name = "CmbTemplateName"
        Me.CmbTemplateName.Size = New System.Drawing.Size(327, 21)
        Me.CmbTemplateName.TabIndex = 1
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.chkTamilFont)
        Me.GrpContainer.Controls.Add(Me.CmbType)
        Me.GrpContainer.Controls.Add(Me.Label3)
        Me.GrpContainer.Controls.Add(Me.cmbActive)
        Me.GrpContainer.Controls.Add(Me.Label9)
        Me.GrpContainer.Controls.Add(Me.txtTemplateDescription_OWN)
        Me.GrpContainer.Controls.Add(Me.CmbTemplateName)
        Me.GrpContainer.Controls.Add(Me.Label2)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSave)
        Me.GrpContainer.Controls.Add(Me.LABEL1)
        Me.GrpContainer.Location = New System.Drawing.Point(38, 14)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(500, 314)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'chkTamilFont
        '
        Me.chkTamilFont.AutoSize = True
        Me.chkTamilFont.Location = New System.Drawing.Point(18, 109)
        Me.chkTamilFont.Name = "chkTamilFont"
        Me.chkTamilFont.Size = New System.Drawing.Size(85, 17)
        Me.chkTamilFont.TabIndex = 3
        Me.chkTamilFont.Text = "Tamil Font"
        Me.chkTamilFont.UseVisualStyleBackColor = True
        Me.chkTamilFont.Visible = False
        '
        'CmbType
        '
        Me.CmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbType.FormattingEnabled = True
        Me.CmbType.Items.AddRange(New Object() {"Customer", "Owner"})
        Me.CmbType.Location = New System.Drawing.Point(282, 214)
        Me.CmbType.Name = "CmbType"
        Me.CmbType.Size = New System.Drawing.Size(99, 21)
        Me.CmbType.TabIndex = 8
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(216, 218)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Sms Type"
        '
        'cmbActive
        '
        Me.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActive.FormattingEnabled = True
        Me.cmbActive.Items.AddRange(New Object() {"Yes", "No"})
        Me.cmbActive.Location = New System.Drawing.Point(143, 214)
        Me.cmbActive.Name = "cmbActive"
        Me.cmbActive.Size = New System.Drawing.Size(67, 21)
        Me.cmbActive.TabIndex = 6
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(24, 218)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(65, 13)
        Me.Label9.TabIndex = 5
        Me.Label9.Text = "Send Sms"
        '
        'txtTemplateDescription_OWN
        '
        Me.txtTemplateDescription_OWN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTemplateDescription_OWN.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTemplateDescription_OWN.Location = New System.Drawing.Point(143, 76)
        Me.txtTemplateDescription_OWN.Multiline = True
        Me.txtTemplateDescription_OWN.Name = "txtTemplateDescription_OWN"
        Me.txtTemplateDescription_OWN.Size = New System.Drawing.Size(327, 126)
        Me.txtTemplateDescription_OWN.TabIndex = 4
        '
        'frmSmsTemplate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(585, 353)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmSmsTemplate"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SmsTemplate"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents LABEL1 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents CmbTemplateName As System.Windows.Forms.ComboBox
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents txtTemplateDescription_OWN As System.Windows.Forms.TextBox
    Friend WithEvents cmbActive As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents CmbType As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkTamilFont As System.Windows.Forms.CheckBox

End Class
