<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGSTcategory
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
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.btnSave = New System.Windows.Forms.Button
        Me.txtGSTCharge = New System.Windows.Forms.TextBox
        Me.txtShortName__Man = New System.Windows.Forms.TextBox
        Me.txtCardName__Man = New System.Windows.Forms.TextBox
        Me.cmbCGSTDefaultAcCode = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.grpField = New System.Windows.Forms.GroupBox
        Me.pnlButtons = New System.Windows.Forms.Panel
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.pnl1 = New System.Windows.Forms.Panel
        Me.pnl2 = New System.Windows.Forms.Panel
        Me.cmbRSGSTDefaultAcCode = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.cmbRGSTDefaultAcCode = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.cmbRCGSTDefaultAcCode = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.cmbActive = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.cmbSGSTDefaultAcCode = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.cmbIGSTDefaultAcCode = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtorderid = New System.Windows.Forms.TextBox
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.tabView = New System.Windows.Forms.TabPage
        Me.Label20 = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpField.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
        Me.pnl1.SuspendLayout()
        Me.pnl2.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'btnSave
        '
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(3, 3)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'txtGSTCharge
        '
        Me.txtGSTCharge.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGSTCharge.Location = New System.Drawing.Point(109, 204)
        Me.txtGSTCharge.MaxLength = 3
        Me.txtGSTCharge.Name = "txtGSTCharge"
        Me.txtGSTCharge.Size = New System.Drawing.Size(59, 21)
        Me.txtGSTCharge.TabIndex = 7
        Me.txtGSTCharge.Text = "999"
        '
        'txtShortName__Man
        '
        Me.txtShortName__Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtShortName__Man.Location = New System.Drawing.Point(109, 11)
        Me.txtShortName__Man.MaxLength = 10
        Me.txtShortName__Man.Name = "txtShortName__Man"
        Me.txtShortName__Man.Size = New System.Drawing.Size(84, 21)
        Me.txtShortName__Man.TabIndex = 1
        Me.txtShortName__Man.Text = "1234567890"
        '
        'txtCardName__Man
        '
        Me.txtCardName__Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCardName__Man.Location = New System.Drawing.Point(109, 42)
        Me.txtCardName__Man.MaxLength = 30
        Me.txtCardName__Man.Name = "txtCardName__Man"
        Me.txtCardName__Man.Size = New System.Drawing.Size(289, 21)
        Me.txtCardName__Man.TabIndex = 3
        Me.txtCardName__Man.Text = "123456789012345678901234567890"
        '
        'cmbCGSTDefaultAcCode
        '
        Me.cmbCGSTDefaultAcCode.FormattingEnabled = True
        Me.cmbCGSTDefaultAcCode.Location = New System.Drawing.Point(109, 3)
        Me.cmbCGSTDefaultAcCode.Name = "cmbCGSTDefaultAcCode"
        Me.cmbCGSTDefaultAcCode.Size = New System.Drawing.Size(289, 21)
        Me.cmbCGSTDefaultAcCode.TabIndex = 1
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 207)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(47, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "GST %"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(66, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "CGST A/C"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 15)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(99, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Category Group"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Category Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(3, 23)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1445, 565)
        Me.gridView.TabIndex = 19
        '
        'grpField
        '
        Me.grpField.BackColor = System.Drawing.Color.Transparent
        Me.grpField.Controls.Add(Me.pnlButtons)
        Me.grpField.Controls.Add(Me.pnl1)
        Me.grpField.Controls.Add(Me.pnl2)
        Me.grpField.Location = New System.Drawing.Point(179, 102)
        Me.grpField.Name = "grpField"
        Me.grpField.Size = New System.Drawing.Size(436, 444)
        Me.grpField.TabIndex = 0
        Me.grpField.TabStop = False
        Me.grpField.Text = "Add Gst Category"
        '
        'pnlButtons
        '
        Me.pnlButtons.Controls.Add(Me.btnSave)
        Me.pnlButtons.Controls.Add(Me.btnOpen)
        Me.pnlButtons.Controls.Add(Me.btnNew)
        Me.pnlButtons.Controls.Add(Me.btnExit)
        Me.pnlButtons.Location = New System.Drawing.Point(6, 404)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(428, 34)
        Me.pnlButtons.TabIndex = 2
        '
        'btnOpen
        '
        Me.btnOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOpen.Location = New System.Drawing.Point(109, 3)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 1
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNew.Location = New System.Drawing.Point(215, 3)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 2
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExit.Location = New System.Drawing.Point(321, 3)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 3
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'pnl1
        '
        Me.pnl1.Controls.Add(Me.txtShortName__Man)
        Me.pnl1.Controls.Add(Me.Label3)
        Me.pnl1.Controls.Add(Me.txtCardName__Man)
        Me.pnl1.Controls.Add(Me.Label2)
        Me.pnl1.Location = New System.Drawing.Point(6, 16)
        Me.pnl1.Name = "pnl1"
        Me.pnl1.Size = New System.Drawing.Size(418, 71)
        Me.pnl1.TabIndex = 0
        '
        'pnl2
        '
        Me.pnl2.Controls.Add(Me.cmbRSGSTDefaultAcCode)
        Me.pnl2.Controls.Add(Me.Label9)
        Me.pnl2.Controls.Add(Me.cmbRGSTDefaultAcCode)
        Me.pnl2.Controls.Add(Me.Label10)
        Me.pnl2.Controls.Add(Me.cmbRCGSTDefaultAcCode)
        Me.pnl2.Controls.Add(Me.Label11)
        Me.pnl2.Controls.Add(Me.cmbActive)
        Me.pnl2.Controls.Add(Me.Label8)
        Me.pnl2.Controls.Add(Me.cmbSGSTDefaultAcCode)
        Me.pnl2.Controls.Add(Me.Label5)
        Me.pnl2.Controls.Add(Me.cmbIGSTDefaultAcCode)
        Me.pnl2.Controls.Add(Me.Label4)
        Me.pnl2.Controls.Add(Me.Label1)
        Me.pnl2.Controls.Add(Me.cmbCGSTDefaultAcCode)
        Me.pnl2.Controls.Add(Me.Label7)
        Me.pnl2.Controls.Add(Me.txtorderid)
        Me.pnl2.Controls.Add(Me.txtGSTCharge)
        Me.pnl2.Controls.Add(Me.Label6)
        Me.pnl2.Location = New System.Drawing.Point(6, 91)
        Me.pnl2.Name = "pnl2"
        Me.pnl2.Size = New System.Drawing.Size(418, 307)
        Me.pnl2.TabIndex = 1
        '
        'cmbRSGSTDefaultAcCode
        '
        Me.cmbRSGSTDefaultAcCode.FormattingEnabled = True
        Me.cmbRSGSTDefaultAcCode.Location = New System.Drawing.Point(109, 134)
        Me.cmbRSGSTDefaultAcCode.Name = "cmbRSGSTDefaultAcCode"
        Me.cmbRSGSTDefaultAcCode.Size = New System.Drawing.Size(289, 21)
        Me.cmbRSGSTDefaultAcCode.TabIndex = 15
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(8, 137)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(95, 13)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "RCM SGST A/C"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbRGSTDefaultAcCode
        '
        Me.cmbRGSTDefaultAcCode.FormattingEnabled = True
        Me.cmbRGSTDefaultAcCode.Location = New System.Drawing.Point(109, 166)
        Me.cmbRGSTDefaultAcCode.Name = "cmbRGSTDefaultAcCode"
        Me.cmbRGSTDefaultAcCode.Size = New System.Drawing.Size(289, 21)
        Me.cmbRGSTDefaultAcCode.TabIndex = 17
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(8, 169)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(92, 13)
        Me.Label10.TabIndex = 16
        Me.Label10.Text = "RCM IGST A/C"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbRCGSTDefaultAcCode
        '
        Me.cmbRCGSTDefaultAcCode.FormattingEnabled = True
        Me.cmbRCGSTDefaultAcCode.Location = New System.Drawing.Point(109, 102)
        Me.cmbRCGSTDefaultAcCode.Name = "cmbRCGSTDefaultAcCode"
        Me.cmbRCGSTDefaultAcCode.Size = New System.Drawing.Size(289, 21)
        Me.cmbRCGSTDefaultAcCode.TabIndex = 13
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(8, 105)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(96, 13)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "RCM CGST A/C"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbActive
        '
        Me.cmbActive.FormattingEnabled = True
        Me.cmbActive.Items.AddRange(New Object() {"YES", "NO"})
        Me.cmbActive.Location = New System.Drawing.Point(109, 236)
        Me.cmbActive.Name = "cmbActive"
        Me.cmbActive.Size = New System.Drawing.Size(59, 21)
        Me.cmbActive.TabIndex = 9
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(8, 239)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(52, 13)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "GST RD"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSGSTDefaultAcCode
        '
        Me.cmbSGSTDefaultAcCode.FormattingEnabled = True
        Me.cmbSGSTDefaultAcCode.Location = New System.Drawing.Point(109, 35)
        Me.cmbSGSTDefaultAcCode.Name = "cmbSGSTDefaultAcCode"
        Me.cmbSGSTDefaultAcCode.Size = New System.Drawing.Size(289, 21)
        Me.cmbSGSTDefaultAcCode.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(8, 38)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "SGST A/C"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbIGSTDefaultAcCode
        '
        Me.cmbIGSTDefaultAcCode.FormattingEnabled = True
        Me.cmbIGSTDefaultAcCode.Location = New System.Drawing.Point(109, 67)
        Me.cmbIGSTDefaultAcCode.Name = "cmbIGSTDefaultAcCode"
        Me.cmbIGSTDefaultAcCode.Size = New System.Drawing.Size(289, 21)
        Me.cmbIGSTDefaultAcCode.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 70)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "IGST A/C"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 271)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(86, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Display Order"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtorderid
        '
        Me.txtorderid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtorderid.Location = New System.Drawing.Point(109, 268)
        Me.txtorderid.MaxLength = 3
        Me.txtorderid.Name = "txtorderid"
        Me.txtorderid.Size = New System.Drawing.Size(59, 21)
        Me.txtorderid.TabIndex = 11
        Me.txtorderid.Text = "999"
        '
        'tabMain
        '
        Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.ItemSize = New System.Drawing.Size(10, 5)
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(746, 623)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grpField)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 9)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(738, 610)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.Label20)
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.lblStatus)
        Me.tabView.Location = New System.Drawing.Point(4, 9)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1020, 610)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.Red
        Me.Label20.Location = New System.Drawing.Point(117, 7)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(153, 13)
        Me.Label20.TabIndex = 23
        Me.Label20.Text = "*Press Escape to Back"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(8, 591)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 20
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'frmGSTcategory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(746, 623)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmGSTcategory"
        Me.Text = "GST CATEGORY"
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpField.ResumeLayout(False)
        Me.pnlButtons.ResumeLayout(False)
        Me.pnl1.ResumeLayout(False)
        Me.pnl1.PerformLayout()
        Me.pnl2.ResumeLayout(False)
        Me.pnl2.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.tabView.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtCardName__Man As System.Windows.Forms.TextBox
    Friend WithEvents txtGSTCharge As System.Windows.Forms.TextBox
    Friend WithEvents txtShortName__Man As System.Windows.Forms.TextBox
    Friend WithEvents cmbCGSTDefaultAcCode As System.Windows.Forms.ComboBox
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
    Friend WithEvents grpField As System.Windows.Forms.GroupBox
    Friend WithEvents pnlButtons As System.Windows.Forms.Panel
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents pnl1 As System.Windows.Forms.Panel
    Friend WithEvents pnl2 As System.Windows.Forms.Panel
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtorderid As System.Windows.Forms.TextBox
    Friend WithEvents cmbSGSTDefaultAcCode As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbIGSTDefaultAcCode As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbActive As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbRSGSTDefaultAcCode As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cmbRGSTDefaultAcCode As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cmbRCGSTDefaultAcCode As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
End Class
