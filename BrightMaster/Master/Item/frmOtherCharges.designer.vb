<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOtherCharges
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
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.grpField = New System.Windows.Forms.GroupBox
        Me.btnDelete = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbCategoryName_Man = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbMetalName_Man = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbAcctName_Man = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.cmbActive = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtDefaultValue_Amt = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtMiscName__Man = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtMiscId = New System.Windows.Forms.TextBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblStatus = New System.Windows.Forms.Label
        Me.txtComm_Per = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpField.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(9, 157)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(834, 371)
        Me.gridView.TabIndex = 0
        '
        'grpField
        '
        Me.grpField.BackColor = System.Drawing.Color.Transparent
        Me.grpField.Controls.Add(Me.txtComm_Per)
        Me.grpField.Controls.Add(Me.Label8)
        Me.grpField.Controls.Add(Me.btnDelete)
        Me.grpField.Controls.Add(Me.btnExit)
        Me.grpField.Controls.Add(Me.btnNew)
        Me.grpField.Controls.Add(Me.btnOpen)
        Me.grpField.Controls.Add(Me.btnSave)
        Me.grpField.Controls.Add(Me.Label1)
        Me.grpField.Controls.Add(Me.cmbCategoryName_Man)
        Me.grpField.Controls.Add(Me.Label2)
        Me.grpField.Controls.Add(Me.cmbMetalName_Man)
        Me.grpField.Controls.Add(Me.Label3)
        Me.grpField.Controls.Add(Me.cmbAcctName_Man)
        Me.grpField.Controls.Add(Me.Label5)
        Me.grpField.Controls.Add(Me.cmbActive)
        Me.grpField.Controls.Add(Me.Label4)
        Me.grpField.Controls.Add(Me.txtDefaultValue_Amt)
        Me.grpField.Controls.Add(Me.Label6)
        Me.grpField.Controls.Add(Me.txtMiscName__Man)
        Me.grpField.Controls.Add(Me.Label7)
        Me.grpField.Controls.Add(Me.txtMiscId)
        Me.grpField.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpField.Location = New System.Drawing.Point(0, 0)
        Me.grpField.Name = "grpField"
        Me.grpField.Size = New System.Drawing.Size(855, 149)
        Me.grpField.TabIndex = 0
        Me.grpField.TabStop = False
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(628, 110)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 20
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(520, 110)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 19
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(412, 110)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 18
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(304, 110)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 17
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(196, 110)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 16
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(790, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Misc Id"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label1.Visible = False
        '
        'cmbCategoryName_Man
        '
        Me.cmbCategoryName_Man.FormattingEnabled = True
        Me.cmbCategoryName_Man.Location = New System.Drawing.Point(459, 48)
        Me.cmbCategoryName_Man.Name = "cmbCategoryName_Man"
        Me.cmbCategoryName_Man.Size = New System.Drawing.Size(323, 21)
        Me.cmbCategoryName_Man.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(6, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 21)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Misc Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMetalName_Man
        '
        Me.cmbMetalName_Man.FormattingEnabled = True
        Me.cmbMetalName_Man.Location = New System.Drawing.Point(112, 49)
        Me.cmbMetalName_Man.Name = "cmbMetalName_Man"
        Me.cmbMetalName_Man.Size = New System.Drawing.Size(257, 21)
        Me.cmbMetalName_Man.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(6, 49)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 21)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Metal Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbAcctName_Man
        '
        Me.cmbAcctName_Man.FormattingEnabled = True
        Me.cmbAcctName_Man.Location = New System.Drawing.Point(112, 80)
        Me.cmbAcctName_Man.Name = "cmbAcctName_Man"
        Me.cmbAcctName_Man.Size = New System.Drawing.Size(257, 21)
        Me.cmbAcctName_Man.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(6, 80)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(100, 21)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "AcctName"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbActive
        '
        Me.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActive.FormattingEnabled = True
        Me.cmbActive.Location = New System.Drawing.Point(112, 114)
        Me.cmbActive.Name = "cmbActive"
        Me.cmbActive.Size = New System.Drawing.Size(75, 21)
        Me.cmbActive.TabIndex = 15
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(375, 48)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(84, 21)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Category"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDefaultValue_Amt
        '
        Me.txtDefaultValue_Amt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDefaultValue_Amt.Location = New System.Drawing.Point(459, 80)
        Me.txtDefaultValue_Amt.MaxLength = 9
        Me.txtDefaultValue_Amt.Name = "txtDefaultValue_Amt"
        Me.txtDefaultValue_Amt.Size = New System.Drawing.Size(113, 21)
        Me.txtDefaultValue_Amt.TabIndex = 11
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(375, 80)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(84, 21)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Default Value"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMiscName__Man
        '
        Me.txtMiscName__Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMiscName__Man.Location = New System.Drawing.Point(112, 18)
        Me.txtMiscName__Man.MaxLength = 35
        Me.txtMiscName__Man.Name = "txtMiscName__Man"
        Me.txtMiscName__Man.Size = New System.Drawing.Size(257, 21)
        Me.txtMiscName__Man.TabIndex = 3
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(6, 114)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(100, 21)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Active"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMiscId
        '
        Me.txtMiscId.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMiscId.Location = New System.Drawing.Point(849, 17)
        Me.txtMiscId.MaxLength = 8
        Me.txtMiscId.Name = "txtMiscId"
        Me.txtMiscId.Size = New System.Drawing.Size(90, 21)
        Me.txtMiscId.TabIndex = 1
        Me.txtMiscId.Visible = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(131, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(6, 536)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 9
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'txtComm_Per
        '
        Me.txtComm_Per.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtComm_Per.Location = New System.Drawing.Point(669, 80)
        Me.txtComm_Per.MaxLength = 9
        Me.txtComm_Per.Name = "txtComm_Per"
        Me.txtComm_Per.Size = New System.Drawing.Size(113, 21)
        Me.txtComm_Per.TabIndex = 13
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(577, 80)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(84, 21)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "Commission"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmOtherCharges
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(855, 553)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.grpField)
        Me.Controls.Add(Me.gridView)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmOtherCharges"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "OtherCharges"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpField.ResumeLayout(False)
        Me.grpField.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grpField As System.Windows.Forms.GroupBox
    Friend WithEvents cmbCategoryName_Man As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMetalName_Man As System.Windows.Forms.ComboBox
    Friend WithEvents cmbAcctName_Man As System.Windows.Forms.ComboBox
    Friend WithEvents cmbActive As System.Windows.Forms.ComboBox
    Friend WithEvents txtDefaultValue_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtMiscName__Man As System.Windows.Forms.TextBox
    Friend WithEvents txtMiscId As System.Windows.Forms.TextBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents txtComm_Per As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
End Class
