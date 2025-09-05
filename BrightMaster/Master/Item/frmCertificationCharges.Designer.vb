<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCertificationCharges
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
        Me.btnNew = New System.Windows.Forms.Button
        Me.lblStatus = New System.Windows.Forms.Label
        Me.btnSave = New System.Windows.Forms.Button
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.btnExit = New System.Windows.Forms.Button
        Me.txtflatCharge_AMT = New System.Windows.Forms.TextBox
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblPercarat = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtPercarat_Amt = New System.Windows.Forms.TextBox
        Me.txtCentTo = New System.Windows.Forms.TextBox
        Me.txtCentFrom = New System.Windows.Forms.TextBox
        Me.txtMiscId = New System.Windows.Forms.TextBox
        Me.pnlDia = New System.Windows.Forms.Panel
        Me.pnlGeneral = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtFromWt_WET = New System.Windows.Forms.TextBox
        Me.txtToWt_WET = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.cmbMetalName_Man = New System.Windows.Forms.ComboBox
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlDia.SuspendLayout()
        Me.pnlGeneral.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(7, 161)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(668, 382)
        Me.gridView.TabIndex = 13
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(329, 107)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 10
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(7, 546)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(85, 13)
        Me.lblStatus.TabIndex = 14
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(113, 107)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
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
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(437, 107)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 11
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'txtflatCharge_AMT
        '
        Me.txtflatCharge_AMT.Location = New System.Drawing.Point(285, 75)
        Me.txtflatCharge_AMT.MaxLength = 14
        Me.txtflatCharge_AMT.Name = "txtflatCharge_AMT"
        Me.txtflatCharge_AMT.Size = New System.Drawing.Size(94, 20)
        Me.txtflatCharge_AMT.TabIndex = 7
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(221, 107)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 9
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(545, 107)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 12
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(131, 92)
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(3, 4)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "From Cent"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPercarat
        '
        Me.lblPercarat.AutoSize = True
        Me.lblPercarat.BackColor = System.Drawing.Color.Transparent
        Me.lblPercarat.Location = New System.Drawing.Point(15, 78)
        Me.lblPercarat.Name = "lblPercarat"
        Me.lblPercarat.Size = New System.Drawing.Size(51, 13)
        Me.lblPercarat.TabIndex = 4
        Me.lblPercarat.Text = "Per Carat"
        Me.lblPercarat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(193, 4)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(20, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "To"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Location = New System.Drawing.Point(205, 78)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(66, 13)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "Flat Charges"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPercarat_Amt
        '
        Me.txtPercarat_Amt.Location = New System.Drawing.Point(113, 74)
        Me.txtPercarat_Amt.MaxLength = 14
        Me.txtPercarat_Amt.Name = "txtPercarat_Amt"
        Me.txtPercarat_Amt.Size = New System.Drawing.Size(86, 20)
        Me.txtPercarat_Amt.TabIndex = 5
        '
        'txtCentTo
        '
        Me.txtCentTo.Location = New System.Drawing.Point(273, 0)
        Me.txtCentTo.MaxLength = 9
        Me.txtCentTo.Name = "txtCentTo"
        Me.txtCentTo.Size = New System.Drawing.Size(94, 20)
        Me.txtCentTo.TabIndex = 3
        Me.txtCentTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCentFrom
        '
        Me.txtCentFrom.Location = New System.Drawing.Point(101, 0)
        Me.txtCentFrom.MaxLength = 9
        Me.txtCentFrom.Name = "txtCentFrom"
        Me.txtCentFrom.Size = New System.Drawing.Size(86, 20)
        Me.txtCentFrom.TabIndex = 1
        Me.txtCentFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMiscId
        '
        Me.txtMiscId.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMiscId.Location = New System.Drawing.Point(555, 14)
        Me.txtMiscId.MaxLength = 8
        Me.txtMiscId.Name = "txtMiscId"
        Me.txtMiscId.Size = New System.Drawing.Size(90, 20)
        Me.txtMiscId.TabIndex = 14
        Me.txtMiscId.Visible = False
        '
        'pnlDia
        '
        Me.pnlDia.Controls.Add(Me.Label3)
        Me.pnlDia.Controls.Add(Me.txtCentFrom)
        Me.pnlDia.Controls.Add(Me.txtCentTo)
        Me.pnlDia.Controls.Add(Me.Label4)
        Me.pnlDia.Location = New System.Drawing.Point(12, 44)
        Me.pnlDia.Name = "pnlDia"
        Me.pnlDia.Size = New System.Drawing.Size(400, 23)
        Me.pnlDia.TabIndex = 2
        '
        'pnlGeneral
        '
        Me.pnlGeneral.Controls.Add(Me.Label1)
        Me.pnlGeneral.Controls.Add(Me.txtFromWt_WET)
        Me.pnlGeneral.Controls.Add(Me.txtToWt_WET)
        Me.pnlGeneral.Controls.Add(Me.Label2)
        Me.pnlGeneral.Location = New System.Drawing.Point(13, 44)
        Me.pnlGeneral.Name = "pnlGeneral"
        Me.pnlGeneral.Size = New System.Drawing.Size(400, 23)
        Me.pnlGeneral.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(3, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "From Wt"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFromWt_WET
        '
        Me.txtFromWt_WET.Location = New System.Drawing.Point(101, 0)
        Me.txtFromWt_WET.MaxLength = 9
        Me.txtFromWt_WET.Name = "txtFromWt_WET"
        Me.txtFromWt_WET.Size = New System.Drawing.Size(86, 20)
        Me.txtFromWt_WET.TabIndex = 1
        Me.txtFromWt_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtToWt_WET
        '
        Me.txtToWt_WET.Location = New System.Drawing.Point(273, 0)
        Me.txtToWt_WET.MaxLength = 9
        Me.txtToWt_WET.Name = "txtToWt_WET"
        Me.txtToWt_WET.Size = New System.Drawing.Size(94, 20)
        Me.txtToWt_WET.TabIndex = 3
        Me.txtToWt_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(193, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(18, 17)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(33, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Metal"
        '
        'cmbMetalName_Man
        '
        Me.cmbMetalName_Man.FormattingEnabled = True
        Me.cmbMetalName_Man.Location = New System.Drawing.Point(112, 13)
        Me.cmbMetalName_Man.Name = "cmbMetalName_Man"
        Me.cmbMetalName_Man.Size = New System.Drawing.Size(208, 21)
        Me.cmbMetalName_Man.TabIndex = 1
        '
        'frmCertificationCharges
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(683, 559)
        Me.Controls.Add(Me.cmbMetalName_Man)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.pnlDia)
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.txtMiscId)
        Me.Controls.Add(Me.pnlGeneral)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.txtflatCharge_AMT)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.lblPercarat)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtPercarat_Amt)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmCertificationCharges"
        Me.Text = "Certification Charges"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlDia.ResumeLayout(False)
        Me.pnlDia.PerformLayout()
        Me.pnlGeneral.ResumeLayout(False)
        Me.pnlGeneral.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents txtflatCharge_AMT As System.Windows.Forms.TextBox
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblPercarat As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtPercarat_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtCentTo As System.Windows.Forms.TextBox
    Friend WithEvents txtCentFrom As System.Windows.Forms.TextBox
    Friend WithEvents txtMiscId As System.Windows.Forms.TextBox
    Friend WithEvents pnlDia As System.Windows.Forms.Panel
    Friend WithEvents pnlGeneral As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtFromWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtToWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbMetalName_Man As System.Windows.Forms.ComboBox
End Class
