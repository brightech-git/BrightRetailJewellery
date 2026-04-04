<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDiscountRateMaster
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblBullPlat = New System.Windows.Forms.Label()
        Me.txtBullPRate_AMT = New System.Windows.Forms.TextBox()
        Me.lblBullSil = New System.Windows.Forms.Label()
        Me.txtBullSRate_AMT = New System.Windows.Forms.TextBox()
        Me.lblBullGold = New System.Windows.Forms.Label()
        Me.txtBullGRate_AMT = New System.Windows.Forms.TextBox()
        Me.dtpDate = New BrighttechPack.DatePicker(Me.components)
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.txtDisrate = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbMetalName_Man = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(220, 3)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(100, 30)
        Me.btnUpdate.TabIndex = 0
        Me.btnUpdate.Text = "Save [F1]"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(353, 3)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 2
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(6, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "&Date"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.ExitToolStripMenuItem, Me.OpenToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 70)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.Controls.Add(Me.lblBullPlat)
        Me.Panel1.Controls.Add(Me.txtBullPRate_AMT)
        Me.Panel1.Controls.Add(Me.lblBullSil)
        Me.Panel1.Controls.Add(Me.txtBullSRate_AMT)
        Me.Panel1.Controls.Add(Me.lblBullGold)
        Me.Panel1.Controls.Add(Me.txtBullGRate_AMT)
        Me.Panel1.Controls.Add(Me.dtpDate)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(670, 30)
        Me.Panel1.TabIndex = 0
        '
        'lblBullPlat
        '
        Me.lblBullPlat.Location = New System.Drawing.Point(504, 4)
        Me.lblBullPlat.Name = "lblBullPlat"
        Me.lblBullPlat.Size = New System.Drawing.Size(62, 21)
        Me.lblBullPlat.TabIndex = 6
        Me.lblBullPlat.Text = "&Platinum"
        Me.lblBullPlat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblBullPlat.Visible = False
        '
        'txtBullPRate_AMT
        '
        Me.txtBullPRate_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBullPRate_AMT.Location = New System.Drawing.Point(575, 3)
        Me.txtBullPRate_AMT.MaxLength = 12
        Me.txtBullPRate_AMT.Name = "txtBullPRate_AMT"
        Me.txtBullPRate_AMT.Size = New System.Drawing.Size(89, 22)
        Me.txtBullPRate_AMT.TabIndex = 7
        Me.txtBullPRate_AMT.Visible = False
        '
        'lblBullSil
        '
        Me.lblBullSil.Location = New System.Drawing.Point(363, 4)
        Me.lblBullSil.Name = "lblBullSil"
        Me.lblBullSil.Size = New System.Drawing.Size(48, 21)
        Me.lblBullSil.TabIndex = 4
        Me.lblBullSil.Text = "&Silver"
        Me.lblBullSil.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblBullSil.Visible = False
        '
        'txtBullSRate_AMT
        '
        Me.txtBullSRate_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBullSRate_AMT.Location = New System.Drawing.Point(420, 3)
        Me.txtBullSRate_AMT.MaxLength = 12
        Me.txtBullSRate_AMT.Name = "txtBullSRate_AMT"
        Me.txtBullSRate_AMT.Size = New System.Drawing.Size(75, 22)
        Me.txtBullSRate_AMT.TabIndex = 5
        Me.txtBullSRate_AMT.Visible = False
        '
        'lblBullGold
        '
        Me.lblBullGold.Location = New System.Drawing.Point(156, 4)
        Me.lblBullGold.Name = "lblBullGold"
        Me.lblBullGold.Size = New System.Drawing.Size(111, 21)
        Me.lblBullGold.TabIndex = 2
        Me.lblBullGold.Text = "&Bullion Rate Gold"
        Me.lblBullGold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblBullGold.Visible = False
        '
        'txtBullGRate_AMT
        '
        Me.txtBullGRate_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBullGRate_AMT.Location = New System.Drawing.Point(276, 3)
        Me.txtBullGRate_AMT.MaxLength = 12
        Me.txtBullGRate_AMT.Name = "txtBullGRate_AMT"
        Me.txtBullGRate_AMT.Size = New System.Drawing.Size(78, 22)
        Me.txtBullGRate_AMT.TabIndex = 3
        Me.txtBullGRate_AMT.Visible = False
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(54, 4)
        Me.dtpDate.Mask = "##/##/####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDate.Size = New System.Drawing.Size(93, 21)
        Me.dtpDate.TabIndex = 1
        Me.dtpDate.Text = "07/03/9998"
        Me.dtpDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.Transparent
        Me.Panel2.Controls.Add(Me.btnUpdate)
        Me.Panel2.Controls.Add(Me.btnExit)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 334)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(670, 37)
        Me.Panel2.TabIndex = 2
        '
        'txtDisrate
        '
        Me.txtDisrate.Location = New System.Drawing.Point(327, 155)
        Me.txtDisrate.Name = "txtDisrate"
        Me.txtDisrate.Size = New System.Drawing.Size(115, 21)
        Me.txtDisrate.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(162, 158)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Enter Discount Rate"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMetalName_Man
        '
        Me.cmbMetalName_Man.FormattingEnabled = True
        Me.cmbMetalName_Man.Location = New System.Drawing.Point(327, 115)
        Me.cmbMetalName_Man.Name = "cmbMetalName_Man"
        Me.cmbMetalName_Man.Size = New System.Drawing.Size(113, 21)
        Me.cmbMetalName_Man.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(162, 115)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(74, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Metal Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmDiscountRateMaster
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CausesValidation = False
        Me.ClientSize = New System.Drawing.Size(670, 371)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmbMetalName_Man)
        Me.Controls.Add(Me.txtDisrate)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmDiscountRateMaster"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "RateMaster"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents dtpDate As BrighttechPack.DatePicker
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblBullGold As System.Windows.Forms.Label
    Friend WithEvents txtBullGRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents lblBullSil As System.Windows.Forms.Label
    Friend WithEvents txtBullSRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents lblBullPlat As Label
    Friend WithEvents txtBullPRate_AMT As TextBox
    Friend WithEvents txtDisrate As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbMetalName_Man As ComboBox
    Friend WithEvents Label3 As Label
End Class
