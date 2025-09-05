<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOnlineImportandExport
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
        Me.chkAll = New System.Windows.Forms.CheckBox()
        Me.chkCompany = New System.Windows.Forms.CheckBox()
        Me.chkScheme = New System.Windows.Forms.CheckBox()
        Me.chkInsamount = New System.Windows.Forms.CheckBox()
        Me.chkSchemeMast = New System.Windows.Forms.CheckBox()
        Me.chkSchemeTran = New System.Windows.Forms.CheckBox()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.chkRate = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'chkAll
        '
        Me.chkAll.AutoSize = True
        Me.chkAll.Checked = True
        Me.chkAll.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAll.Location = New System.Drawing.Point(48, 33)
        Me.chkAll.Name = "chkAll"
        Me.chkAll.Size = New System.Drawing.Size(46, 17)
        Me.chkAll.TabIndex = 0
        Me.chkAll.Text = "ALL"
        Me.chkAll.UseVisualStyleBackColor = True
        '
        'chkCompany
        '
        Me.chkCompany.AutoSize = True
        Me.chkCompany.Location = New System.Drawing.Point(160, 33)
        Me.chkCompany.Name = "chkCompany"
        Me.chkCompany.Size = New System.Drawing.Size(83, 17)
        Me.chkCompany.TabIndex = 1
        Me.chkCompany.Text = "COMPANY"
        Me.chkCompany.UseVisualStyleBackColor = True
        '
        'chkScheme
        '
        Me.chkScheme.AutoSize = True
        Me.chkScheme.Location = New System.Drawing.Point(256, 33)
        Me.chkScheme.Name = "chkScheme"
        Me.chkScheme.Size = New System.Drawing.Size(74, 17)
        Me.chkScheme.TabIndex = 2
        Me.chkScheme.Text = "SCHEME"
        Me.chkScheme.UseVisualStyleBackColor = True
        '
        'chkInsamount
        '
        Me.chkInsamount.AutoSize = True
        Me.chkInsamount.Location = New System.Drawing.Point(48, 56)
        Me.chkInsamount.Name = "chkInsamount"
        Me.chkInsamount.Size = New System.Drawing.Size(96, 17)
        Me.chkInsamount.TabIndex = 3
        Me.chkInsamount.Text = "INSAMOUNT"
        Me.chkInsamount.UseVisualStyleBackColor = True
        '
        'chkSchemeMast
        '
        Me.chkSchemeMast.AutoSize = True
        Me.chkSchemeMast.Location = New System.Drawing.Point(48, 79)
        Me.chkSchemeMast.Name = "chkSchemeMast"
        Me.chkSchemeMast.Size = New System.Drawing.Size(106, 17)
        Me.chkSchemeMast.TabIndex = 5
        Me.chkSchemeMast.Text = "SCHEMEMAST"
        Me.chkSchemeMast.UseVisualStyleBackColor = True
        '
        'chkSchemeTran
        '
        Me.chkSchemeTran.AutoSize = True
        Me.chkSchemeTran.Location = New System.Drawing.Point(160, 79)
        Me.chkSchemeTran.Name = "chkSchemeTran"
        Me.chkSchemeTran.Size = New System.Drawing.Size(105, 17)
        Me.chkSchemeTran.TabIndex = 6
        Me.chkSchemeTran.Text = "SCHEMETRAN"
        Me.chkSchemeTran.UseVisualStyleBackColor = True
        '
        'btnImport
        '
        Me.btnImport.Location = New System.Drawing.Point(150, 102)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(100, 30)
        Me.btnImport.TabIndex = 8
        Me.btnImport.Text = "Import"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(44, 102)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 7
        Me.btnExport.Text = "Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(362, 102)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 10
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(256, 102)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 9
        Me.btnNew.Text = "New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'chkRate
        '
        Me.chkRate.AutoSize = True
        Me.chkRate.Location = New System.Drawing.Point(160, 56)
        Me.chkRate.Name = "chkRate"
        Me.chkRate.Size = New System.Drawing.Size(104, 17)
        Me.chkRate.TabIndex = 4
        Me.chkRate.Text = "RATE UPDATE"
        Me.chkRate.UseVisualStyleBackColor = True
        '
        'frmOnlineImportandExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(486, 167)
        Me.Controls.Add(Me.chkRate)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.chkSchemeTran)
        Me.Controls.Add(Me.chkSchemeMast)
        Me.Controls.Add(Me.chkInsamount)
        Me.Controls.Add(Me.chkScheme)
        Me.Controls.Add(Me.chkCompany)
        Me.Controls.Add(Me.chkAll)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmOnlineImportandExport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Online Import and Export"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkCompany As System.Windows.Forms.CheckBox
    Friend WithEvents chkScheme As System.Windows.Forms.CheckBox
    Friend WithEvents chkInsamount As System.Windows.Forms.CheckBox
    Friend WithEvents chkSchemeMast As System.Windows.Forms.CheckBox
    Friend WithEvents chkSchemeTran As System.Windows.Forms.CheckBox
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents chkRate As CheckBox
End Class
