<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDailyAuditReport
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
        Me.components = New System.ComponentModel.Container()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.chkwithvalue = New System.Windows.Forms.CheckBox()
        Me.chkCmbUserName_own = New BrighttechPack.CheckedComboBox()
        Me.btnview = New System.Windows.Forms.Button()
        Me.btnexit = New System.Windows.Forms.Button()
        Me.btnsendmail = New System.Windows.Forms.Button()
        Me.btnnew = New System.Windows.Forms.Button()
        Me.dtpEditDate = New BrighttechPack.DatePicker(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1006, 66)
        Me.Panel1.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnExcel)
        Me.GroupBox1.Controls.Add(Me.chkwithvalue)
        Me.GroupBox1.Controls.Add(Me.chkCmbUserName_own)
        Me.GroupBox1.Controls.Add(Me.btnview)
        Me.GroupBox1.Controls.Add(Me.btnexit)
        Me.GroupBox1.Controls.Add(Me.btnsendmail)
        Me.GroupBox1.Controls.Add(Me.btnnew)
        Me.GroupBox1.Controls.Add(Me.dtpEditDate)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1006, 66)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'btnExcel
        '
        Me.btnExcel.Location = New System.Drawing.Point(642, 29)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(117, 30)
        Me.btnExcel.TabIndex = 7
        Me.btnExcel.Text = "Excel[x]"
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'chkwithvalue
        '
        Me.chkwithvalue.AutoSize = True
        Me.chkwithvalue.Location = New System.Drawing.Point(199, 15)
        Me.chkwithvalue.Name = "chkwithvalue"
        Me.chkwithvalue.Size = New System.Drawing.Size(82, 17)
        Me.chkwithvalue.TabIndex = 2
        Me.chkwithvalue.Text = "WithValue"
        Me.chkwithvalue.UseVisualStyleBackColor = True
        '
        'chkCmbUserName_own
        '
        Me.chkCmbUserName_own.CheckOnClick = True
        Me.chkCmbUserName_own.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbUserName_own.DropDownHeight = 1
        Me.chkCmbUserName_own.FormattingEnabled = True
        Me.chkCmbUserName_own.IntegralHeight = False
        Me.chkCmbUserName_own.Location = New System.Drawing.Point(87, 36)
        Me.chkCmbUserName_own.Name = "chkCmbUserName_own"
        Me.chkCmbUserName_own.Size = New System.Drawing.Size(319, 22)
        Me.chkCmbUserName_own.TabIndex = 4
        Me.chkCmbUserName_own.ValueSeparator = ", "
        '
        'btnview
        '
        Me.btnview.Location = New System.Drawing.Point(408, 29)
        Me.btnview.Name = "btnview"
        Me.btnview.Size = New System.Drawing.Size(117, 30)
        Me.btnview.TabIndex = 5
        Me.btnview.Text = "&View"
        Me.btnview.UseVisualStyleBackColor = True
        '
        'btnexit
        '
        Me.btnexit.Location = New System.Drawing.Point(876, 29)
        Me.btnexit.Name = "btnexit"
        Me.btnexit.Size = New System.Drawing.Size(117, 30)
        Me.btnexit.TabIndex = 9
        Me.btnexit.Text = "Exit[F12]"
        Me.btnexit.UseVisualStyleBackColor = True
        '
        'btnsendmail
        '
        Me.btnsendmail.Location = New System.Drawing.Point(759, 29)
        Me.btnsendmail.Name = "btnsendmail"
        Me.btnsendmail.Size = New System.Drawing.Size(117, 30)
        Me.btnsendmail.TabIndex = 8
        Me.btnsendmail.Text = "&Mail"
        Me.btnsendmail.UseVisualStyleBackColor = True
        '
        'btnnew
        '
        Me.btnnew.Location = New System.Drawing.Point(525, 29)
        Me.btnnew.Name = "btnnew"
        Me.btnnew.Size = New System.Drawing.Size(117, 30)
        Me.btnnew.TabIndex = 6
        Me.btnnew.Text = "New[F3]"
        Me.btnnew.UseVisualStyleBackColor = True
        '
        'dtpEditDate
        '
        Me.dtpEditDate.Location = New System.Drawing.Point(87, 13)
        Me.dtpEditDate.Mask = "##/##/####"
        Me.dtpEditDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpEditDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpEditDate.Name = "dtpEditDate"
        Me.dtpEditDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpEditDate.Size = New System.Drawing.Size(107, 21)
        Me.dtpEditDate.TabIndex = 1
        Me.dtpEditDate.Text = "07/03/9998"
        Me.dtpEditDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Edit Date"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "User Name"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.GroupBox2)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 66)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1006, 371)
        Me.Panel2.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.GridView)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1006, 371)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        '
        'GridView
        '
        Me.GridView.AllowUserToAddRows = False
        Me.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridView.Location = New System.Drawing.Point(3, 17)
        Me.GridView.Name = "GridView"
        Me.GridView.ReadOnly = True
        Me.GridView.RowHeadersVisible = False
        Me.GridView.Size = New System.Drawing.Size(1000, 351)
        Me.GridView.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem1, Me.ExitToolStripMenuItem2})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(226, 48)
        '
        'NewToolStripMenuItem1
        '
        Me.NewToolStripMenuItem1.Name = "NewToolStripMenuItem1"
        Me.NewToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem1.Size = New System.Drawing.Size(225, 22)
        Me.NewToolStripMenuItem1.Text = "newToolStripMenuItem1"
        '
        'ExitToolStripMenuItem2
        '
        Me.ExitToolStripMenuItem2.Name = "ExitToolStripMenuItem2"
        Me.ExitToolStripMenuItem2.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem2.Size = New System.Drawing.Size(225, 22)
        Me.ExitToolStripMenuItem2.Text = "ExitToolStripMenuItem2"
        '
        'FrmDailyAuditReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1006, 437)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "FrmDailyAuditReport"
        Me.Text = "Daily Audit Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GridView As System.Windows.Forms.DataGridView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpEditDate As BrighttechPack.DatePicker
    Friend WithEvents btnexit As System.Windows.Forms.Button
    Friend WithEvents btnsendmail As System.Windows.Forms.Button
    Friend WithEvents btnnew As System.Windows.Forms.Button
    Friend WithEvents btnview As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkCmbUserName_own As BrighttechPack.CheckedComboBox
    Friend WithEvents chkwithvalue As System.Windows.Forms.CheckBox
    Friend WithEvents btnExcel As System.Windows.Forms.Button
End Class
