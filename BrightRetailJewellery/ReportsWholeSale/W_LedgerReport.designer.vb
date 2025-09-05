<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WS_DealerSmithLedger
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
        Me.rbtGoldSmith = New System.Windows.Forms.RadioButton
        Me.chkLstName = New System.Windows.Forms.CheckedListBox
        Me.chkSelectAll = New System.Windows.Forms.CheckBox
        Me.grpContainer = New System.Windows.Forms.GroupBox
        Me.rbtDealer = New System.Windows.Forms.RadioButton
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OptGrwt = New System.Windows.Forms.RadioButton
        Me.OptNetwt = New System.Windows.Forms.RadioButton
        Me.OptPurewt = New System.Windows.Forms.RadioButton
        Me.chkRunBal = New System.Windows.Forms.CheckBox
        Me.grpContainer.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'rbtGoldSmith
        '
        Me.rbtGoldSmith.AutoSize = True
        Me.rbtGoldSmith.Checked = True
        Me.rbtGoldSmith.Location = New System.Drawing.Point(91, 71)
        Me.rbtGoldSmith.Name = "rbtGoldSmith"
        Me.rbtGoldSmith.Size = New System.Drawing.Size(88, 17)
        Me.rbtGoldSmith.TabIndex = 5
        Me.rbtGoldSmith.TabStop = True
        Me.rbtGoldSmith.Text = "Gold Smith"
        Me.rbtGoldSmith.UseVisualStyleBackColor = True
        '
        'chkLstName
        '
        Me.chkLstName.FormattingEnabled = True
        Me.chkLstName.Location = New System.Drawing.Point(88, 120)
        Me.chkLstName.Name = "chkLstName"
        Me.chkLstName.Size = New System.Drawing.Size(368, 180)
        Me.chkLstName.TabIndex = 7
        '
        'chkSelectAll
        '
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Location = New System.Drawing.Point(91, 100)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkSelectAll.TabIndex = 6
        Me.chkSelectAll.Text = "Select All"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.rbtDealer)
        Me.grpContainer.Controls.Add(Me.Panel1)
        Me.grpContainer.Controls.Add(Me.btnView_Search)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Controls.Add(Me.dtpTo)
        Me.grpContainer.Controls.Add(Me.dtpFrom)
        Me.grpContainer.Controls.Add(Me.Label4)
        Me.grpContainer.Controls.Add(Me.Label5)
        Me.grpContainer.Controls.Add(Me.chkSelectAll)
        Me.grpContainer.Controls.Add(Me.rbtGoldSmith)
        Me.grpContainer.Controls.Add(Me.chkLstName)
        Me.grpContainer.Location = New System.Drawing.Point(180, 30)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(498, 407)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'rbtDealer
        '
        Me.rbtDealer.AutoSize = True
        Me.rbtDealer.Location = New System.Drawing.Point(239, 71)
        Me.rbtDealer.Name = "rbtDealer"
        Me.rbtDealer.Size = New System.Drawing.Size(67, 17)
        Me.rbtDealer.TabIndex = 13
        Me.rbtDealer.TabStop = True
        Me.rbtDealer.Text = " Dealer"
        Me.rbtDealer.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkRunBal)
        Me.Panel1.Controls.Add(Me.OptPurewt)
        Me.Panel1.Controls.Add(Me.OptNetwt)
        Me.Panel1.Controls.Add(Me.OptGrwt)
        Me.Panel1.Location = New System.Drawing.Point(88, 319)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(365, 28)
        Me.Panel1.TabIndex = 11
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(86, 371)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 8
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(312, 371)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 10
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(199, 371)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 9
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(213, 35)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(93, 35)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 39)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(67, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Date From"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(192, 39)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(21, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "To"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'OptGrwt
        '
        Me.OptGrwt.AutoSize = True
        Me.OptGrwt.Enabled = False
        Me.OptGrwt.Location = New System.Drawing.Point(150, 3)
        Me.OptGrwt.Name = "OptGrwt"
        Me.OptGrwt.Size = New System.Drawing.Size(68, 17)
        Me.OptGrwt.TabIndex = 0
        Me.OptGrwt.TabStop = True
        Me.OptGrwt.Text = "Grs. Wt"
        Me.OptGrwt.UseVisualStyleBackColor = True
        '
        'OptNetwt
        '
        Me.OptNetwt.AutoSize = True
        Me.OptNetwt.Enabled = False
        Me.OptNetwt.Location = New System.Drawing.Point(223, 3)
        Me.OptNetwt.Name = "OptNetwt"
        Me.OptNetwt.Size = New System.Drawing.Size(63, 17)
        Me.OptNetwt.TabIndex = 1
        Me.OptNetwt.TabStop = True
        Me.OptNetwt.Text = "Net Wt"
        Me.OptNetwt.UseVisualStyleBackColor = True
        '
        'OptPurewt
        '
        Me.OptPurewt.AutoSize = True
        Me.OptPurewt.Checked = True
        Me.OptPurewt.Enabled = False
        Me.OptPurewt.Location = New System.Drawing.Point(292, 3)
        Me.OptPurewt.Name = "OptPurewt"
        Me.OptPurewt.Size = New System.Drawing.Size(70, 17)
        Me.OptPurewt.TabIndex = 2
        Me.OptPurewt.TabStop = True
        Me.OptPurewt.Text = "Pure Wt"
        Me.OptPurewt.UseVisualStyleBackColor = True
        '
        'chkRunBal
        '
        Me.chkRunBal.AutoSize = True
        Me.chkRunBal.Location = New System.Drawing.Point(7, 4)
        Me.chkRunBal.Name = "chkRunBal"
        Me.chkRunBal.Size = New System.Drawing.Size(141, 17)
        Me.chkRunBal.TabIndex = 3
        Me.chkRunBal.Text = "Running Balance On"
        Me.chkRunBal.UseVisualStyleBackColor = True
        '
        'WS_DealerSmithLedger
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(862, 529)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "WS_DealerSmithLedger"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Dealer/Smith Ledger Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents rbtGoldSmith As System.Windows.Forms.RadioButton
    Friend WithEvents chkLstName As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtDealer As System.Windows.Forms.RadioButton
    Friend WithEvents OptGrwt As System.Windows.Forms.RadioButton
    Friend WithEvents chkRunBal As System.Windows.Forms.CheckBox
    Friend WithEvents OptPurewt As System.Windows.Forms.RadioButton
    Friend WithEvents OptNetwt As System.Windows.Forms.RadioButton
End Class
