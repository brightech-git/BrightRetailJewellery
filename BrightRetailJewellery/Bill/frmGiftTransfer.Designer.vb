<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGiftTransfer
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
        Me.txtTagNo = New System.Windows.Forms.TextBox
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FileSystemWatcher1 = New System.IO.FileSystemWatcher
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.chkRecDate = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbCostCentre_MAN = New System.Windows.Forms.ComboBox
        Me.btnTransfer = New System.Windows.Forms.Button
        Me.dtpDate = New BrighttechPack.DatePicker(Me.components)
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.chkCheckByScan = New System.Windows.Forms.CheckBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.txtLotNo = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnMiscIssue = New System.Windows.Forms.Button
        Me.txtEstNo = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.cmbCounter_OWN = New BrighttechPack.CheckedComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.cmbGiftsCostName_MAN = New System.Windows.Forms.ComboBox
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtTagNo
        '
        Me.txtTagNo.Location = New System.Drawing.Point(432, 70)
        Me.txtTagNo.Name = "txtTagNo"
        Me.txtTagNo.Size = New System.Drawing.Size(94, 21)
        Me.txtTagNo.TabIndex = 19
        Me.txtTagNo.Visible = False
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 136)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(934, 384)
        Me.gridView.TabIndex = 1
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(136, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'FileSystemWatcher1
        '
        Me.FileSystemWatcher1.EnableRaisingEvents = True
        Me.FileSystemWatcher1.SynchronizingObject = Me
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(375, 74)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Tag No"
        Me.Label2.Visible = False
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(431, 95)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 10
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(746, 95)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 13
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'chkRecDate
        '
        Me.chkRecDate.AutoSize = True
        Me.chkRecDate.Location = New System.Drawing.Point(17, 70)
        Me.chkRecDate.Name = "chkRecDate"
        Me.chkRecDate.Size = New System.Drawing.Size(78, 17)
        Me.chkRecDate.TabIndex = 4
        Me.chkRecDate.Text = "Rec Date"
        Me.chkRecDate.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Transfer To"
        '
        'cmbCostCentre_MAN
        '
        Me.cmbCostCentre_MAN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCostCentre_MAN.FormattingEnabled = True
        Me.cmbCostCentre_MAN.Location = New System.Drawing.Point(116, 12)
        Me.cmbCostCentre_MAN.Name = "cmbCostCentre_MAN"
        Me.cmbCostCentre_MAN.Size = New System.Drawing.Size(253, 21)
        Me.cmbCostCentre_MAN.TabIndex = 1
        '
        'btnTransfer
        '
        Me.btnTransfer.Location = New System.Drawing.Point(221, 95)
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.Size = New System.Drawing.Size(100, 30)
        Me.btnTransfer.TabIndex = 9
        Me.btnTransfer.Text = "&Transfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(116, 68)
        Me.dtpDate.Mask = "##/##/####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDate.Size = New System.Drawing.Size(94, 21)
        Me.dtpDate.TabIndex = 5
        Me.dtpDate.Text = "07/03/9998"
        Me.dtpDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(641, 95)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 12
        Me.btnPrint.Text = "Print [P]"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(536, 95)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 11
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(372, 44)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(90, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Counter Name"
        Me.Label4.Visible = False
        '
        'chkCheckByScan
        '
        Me.chkCheckByScan.AutoSize = True
        Me.chkCheckByScan.Location = New System.Drawing.Point(375, 14)
        Me.chkCheckByScan.Name = "chkCheckByScan"
        Me.chkCheckByScan.Size = New System.Drawing.Size(112, 17)
        Me.chkCheckByScan.TabIndex = 14
        Me.chkCheckByScan.Text = "Check by Scan"
        Me.chkCheckByScan.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(116, 95)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 8
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'txtLotNo
        '
        Me.txtLotNo.Location = New System.Drawing.Point(275, 68)
        Me.txtLotNo.Name = "txtLotNo"
        Me.txtLotNo.Size = New System.Drawing.Size(94, 21)
        Me.txtLotNo.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(218, 72)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(44, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "RunNo"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnMiscIssue)
        Me.Panel1.Controls.Add(Me.txtEstNo)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.cmbCounter_OWN)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.cmbGiftsCostName_MAN)
        Me.Panel1.Controls.Add(Me.txtTagNo)
        Me.Panel1.Controls.Add(Me.chkCheckByScan)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.chkRecDate)
        Me.Panel1.Controls.Add(Me.dtpDate)
        Me.Panel1.Controls.Add(Me.btnTransfer)
        Me.Panel1.Controls.Add(Me.cmbCostCentre_MAN)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.txtLotNo)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(934, 136)
        Me.Panel1.TabIndex = 0
        '
        'btnMiscIssue
        '
        Me.btnMiscIssue.Location = New System.Drawing.Point(326, 95)
        Me.btnMiscIssue.Name = "btnMiscIssue"
        Me.btnMiscIssue.Size = New System.Drawing.Size(100, 30)
        Me.btnMiscIssue.TabIndex = 21
        Me.btnMiscIssue.Text = "&MiscIssue"
        Me.btnMiscIssue.UseVisualStyleBackColor = True
        '
        'txtEstNo
        '
        Me.txtEstNo.Location = New System.Drawing.Point(619, 72)
        Me.txtEstNo.Name = "txtEstNo"
        Me.txtEstNo.Size = New System.Drawing.Size(94, 21)
        Me.txtEstNo.TabIndex = 0
        Me.txtEstNo.Visible = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(529, 74)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(85, 13)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "Estimation No"
        Me.Label7.Visible = False
        '
        'cmbCounter_OWN
        '
        Me.cmbCounter_OWN.CheckOnClick = True
        Me.cmbCounter_OWN.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbCounter_OWN.DropDownHeight = 1
        Me.cmbCounter_OWN.FormattingEnabled = True
        Me.cmbCounter_OWN.IntegralHeight = False
        Me.cmbCounter_OWN.Location = New System.Drawing.Point(462, 40)
        Me.cmbCounter_OWN.Name = "cmbCounter_OWN"
        Me.cmbCounter_OWN.Size = New System.Drawing.Size(204, 22)
        Me.cmbCounter_OWN.TabIndex = 18
        Me.cmbCounter_OWN.ValueSeparator = ", "
        Me.cmbCounter_OWN.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(14, 43)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(99, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Gift's CostName"
        '
        'cmbGiftsCostName_MAN
        '
        Me.cmbGiftsCostName_MAN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbGiftsCostName_MAN.FormattingEnabled = True
        Me.cmbGiftsCostName_MAN.Location = New System.Drawing.Point(116, 39)
        Me.cmbGiftsCostName_MAN.Name = "cmbGiftsCostName_MAN"
        Me.cmbGiftsCostName_MAN.Size = New System.Drawing.Size(253, 21)
        Me.cmbGiftsCostName_MAN.TabIndex = 3
        '
        'frmGiftTransfer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(934, 520)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmGiftTransfer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gift Transfer"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtTagNo As System.Windows.Forms.TextBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents FileSystemWatcher1 As System.IO.FileSystemWatcher
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkRecDate As System.Windows.Forms.CheckBox
    Friend WithEvents cmbCostCentre_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnTransfer As System.Windows.Forms.Button
    Friend WithEvents dtpDate As BrighttechPack.DatePicker
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkCheckByScan As System.Windows.Forms.CheckBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents txtLotNo As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbGiftsCostName_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCounter_OWN As BrighttechPack.CheckedComboBox
    Friend WithEvents txtEstNo As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnMiscIssue As System.Windows.Forms.Button
End Class
