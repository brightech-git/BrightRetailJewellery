<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHallMarkMaterialIssueReceipt
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.cmbDesigner_MAN = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblHelp = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.dtpTrandate = New BrighttechPack.DatePicker(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.ChkAsOn = New System.Windows.Forms.CheckBox
        Me.lblToDate = New System.Windows.Forms.Label
        Me.dtpToDate = New BrighttechPack.DatePicker(Me.components)
        Me.btnReceipt = New System.Windows.Forms.Button
        Me.chkSelectAll = New System.Windows.Forms.CheckBox
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnIssue = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rdbReceipt = New System.Windows.Forms.RadioButton
        Me.rbtIssue = New System.Windows.Forms.RadioButton
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cmbDesigner_MAN)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.lblHelp)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.dtpTrandate)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.ChkAsOn)
        Me.Panel1.Controls.Add(Me.lblToDate)
        Me.Panel1.Controls.Add(Me.dtpToDate)
        Me.Panel1.Controls.Add(Me.btnReceipt)
        Me.Panel1.Controls.Add(Me.chkSelectAll)
        Me.Panel1.Controls.Add(Me.dtpAsOnDate)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnIssue)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(958, 138)
        Me.Panel1.TabIndex = 0
        '
        'cmbDesigner_MAN
        '
        Me.cmbDesigner_MAN.Enabled = False
        Me.cmbDesigner_MAN.FormattingEnabled = True
        Me.cmbDesigner_MAN.Location = New System.Drawing.Point(95, 80)
        Me.cmbDesigner_MAN.Name = "cmbDesigner_MAN"
        Me.cmbDesigner_MAN.Size = New System.Drawing.Size(255, 21)
        Me.cmbDesigner_MAN.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 84)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "To Dealer"
        '
        'lblHelp
        '
        Me.lblHelp.AutoSize = True
        Me.lblHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHelp.ForeColor = System.Drawing.Color.Red
        Me.lblHelp.Location = New System.Drawing.Point(684, 14)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(36, 13)
        Me.lblHelp.TabIndex = 13
        Me.lblHelp.Text = "Help"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 34)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Tran Date"
        '
        'dtpTrandate
        '
        Me.dtpTrandate.Location = New System.Drawing.Point(97, 32)
        Me.dtpTrandate.Mask = "##-##-####"
        Me.dtpTrandate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTrandate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTrandate.Name = "dtpTrandate"
        Me.dtpTrandate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTrandate.Size = New System.Drawing.Size(91, 21)
        Me.dtpTrandate.TabIndex = 5
        Me.dtpTrandate.Text = "02/07/2010"
        Me.dtpTrandate.Value = New Date(2010, 7, 2, 0, 0, 0, 0)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 60)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Hallmark "
        '
        'ChkAsOn
        '
        Me.ChkAsOn.AutoSize = True
        Me.ChkAsOn.Checked = True
        Me.ChkAsOn.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkAsOn.Location = New System.Drawing.Point(13, 10)
        Me.ChkAsOn.Name = "ChkAsOn"
        Me.ChkAsOn.Size = New System.Drawing.Size(83, 17)
        Me.ChkAsOn.TabIndex = 0
        Me.ChkAsOn.Text = "AsOnDate"
        Me.ChkAsOn.UseVisualStyleBackColor = True
        '
        'lblToDate
        '
        Me.lblToDate.AutoSize = True
        Me.lblToDate.Location = New System.Drawing.Point(191, 12)
        Me.lblToDate.Name = "lblToDate"
        Me.lblToDate.Size = New System.Drawing.Size(52, 13)
        Me.lblToDate.TabIndex = 2
        Me.lblToDate.Text = "To Date"
        Me.lblToDate.Visible = False
        '
        'dtpToDate
        '
        Me.dtpToDate.Location = New System.Drawing.Point(249, 8)
        Me.dtpToDate.Mask = "##-##-####"
        Me.dtpToDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpToDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpToDate.Size = New System.Drawing.Size(91, 21)
        Me.dtpToDate.TabIndex = 3
        Me.dtpToDate.Text = "02/07/2010"
        Me.dtpToDate.Value = New Date(2010, 7, 2, 0, 0, 0, 0)
        Me.dtpToDate.Visible = False
        '
        'btnReceipt
        '
        Me.btnReceipt.Enabled = False
        Me.btnReceipt.Location = New System.Drawing.Point(395, 104)
        Me.btnReceipt.Name = "btnReceipt"
        Me.btnReceipt.Size = New System.Drawing.Size(100, 30)
        Me.btnReceipt.TabIndex = 14
        Me.btnReceipt.Text = "&Receipt"
        Me.btnReceipt.UseVisualStyleBackColor = True
        '
        'chkSelectAll
        '
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Location = New System.Drawing.Point(356, 81)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkSelectAll.TabIndex = 10
        Me.chkSelectAll.Text = "&Select All"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(97, 8)
        Me.dtpAsOnDate.Mask = "##-##-####"
        Me.dtpAsOnDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate.Name = "dtpAsOnDate"
        Me.dtpAsOnDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate.Size = New System.Drawing.Size(91, 21)
        Me.dtpAsOnDate.TabIndex = 1
        Me.dtpAsOnDate.Text = "02/07/2010"
        Me.dtpAsOnDate.Value = New Date(2010, 7, 2, 0, 0, 0, 0)
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(495, 104)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 15
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnIssue
        '
        Me.btnIssue.Location = New System.Drawing.Point(295, 104)
        Me.btnIssue.Name = "btnIssue"
        Me.btnIssue.Size = New System.Drawing.Size(100, 30)
        Me.btnIssue.TabIndex = 13
        Me.btnIssue.Text = "&Issue"
        Me.btnIssue.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(95, 104)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 11
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(195, 104)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 12
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rdbReceipt)
        Me.Panel2.Controls.Add(Me.rbtIssue)
        Me.Panel2.Location = New System.Drawing.Point(96, 56)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(148, 21)
        Me.Panel2.TabIndex = 7
        '
        'rdbReceipt
        '
        Me.rdbReceipt.AutoSize = True
        Me.rdbReceipt.Location = New System.Drawing.Point(66, 2)
        Me.rdbReceipt.Name = "rdbReceipt"
        Me.rdbReceipt.Size = New System.Drawing.Size(67, 17)
        Me.rdbReceipt.TabIndex = 1
        Me.rdbReceipt.Text = "Receipt"
        Me.rdbReceipt.UseVisualStyleBackColor = True
        '
        'rbtIssue
        '
        Me.rbtIssue.AutoSize = True
        Me.rbtIssue.Checked = True
        Me.rbtIssue.Location = New System.Drawing.Point(4, 2)
        Me.rbtIssue.Name = "rbtIssue"
        Me.rbtIssue.Size = New System.Drawing.Size(56, 17)
        Me.rbtIssue.TabIndex = 0
        Me.rbtIssue.TabStop = True
        Me.rbtIssue.Text = "Issue"
        Me.rbtIssue.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 138)
        Me.gridView.Name = "gridView"
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(958, 384)
        Me.gridView.TabIndex = 1
        '
        'frmHallMarkMaterialIssueReceipt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(958, 522)
        Me.ControlBox = False
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmHallMarkMaterialIssueReceipt"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "HallMark Material Issue/Receipt"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnIssue As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents rdbReceipt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtIssue As System.Windows.Forms.RadioButton
    Friend WithEvents btnReceipt As System.Windows.Forms.Button
    Friend WithEvents dtpToDate As BrighttechPack.DatePicker
    Friend WithEvents lblToDate As System.Windows.Forms.Label
    Friend WithEvents ChkAsOn As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpTrandate As BrighttechPack.DatePicker
    Friend WithEvents lblHelp As System.Windows.Forms.Label
    Friend WithEvents cmbDesigner_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
