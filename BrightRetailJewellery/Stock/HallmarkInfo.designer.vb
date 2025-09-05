<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HallmarkInfo
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
        Me.grpContainer = New System.Windows.Forms.GroupBox()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmbCostCentre_Man = New System.Windows.Forms.ComboBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblDelete = New System.Windows.Forms.Label()
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.txtHmBillCenter = New System.Windows.Forms.TextBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.txtHmBillNo = New System.Windows.Forms.TextBox()
        Me.txtLotNo_NUM = New System.Windows.Forms.TextBox()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.txtTagNo = New System.Windows.Forms.TextBox()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbDesigner = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtFindTag = New System.Windows.Forms.TextBox()
        Me.cmbNewCounter = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.txtItemId_NUM = New System.Windows.Forms.TextBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grpContainer.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.gridView)
        Me.grpContainer.Controls.Add(Me.Panel1)
        Me.grpContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpContainer.Location = New System.Drawing.Point(0, 0)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(952, 523)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(3, 161)
        Me.gridView.Name = "gridView"
        Me.gridView.Size = New System.Drawing.Size(946, 359)
        Me.gridView.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cmbCostCentre_Man)
        Me.Panel1.Controls.Add(Me.Label32)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.lblDelete)
        Me.Panel1.Controls.Add(Me.chkDate)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.txtHmBillCenter)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.txtHmBillNo)
        Me.Panel1.Controls.Add(Me.txtLotNo_NUM)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.txtTagNo)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cmbDesigner)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.txtFindTag)
        Me.Panel1.Controls.Add(Me.cmbNewCounter)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.txtItemId_NUM)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 17)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(946, 144)
        Me.Panel1.TabIndex = 0
        '
        'cmbCostCentre_Man
        '
        Me.cmbCostCentre_Man.FormattingEnabled = True
        Me.cmbCostCentre_Man.Location = New System.Drawing.Point(92, 81)
        Me.cmbCostCentre_Man.Name = "cmbCostCentre_Man"
        Me.cmbCostCentre_Man.Size = New System.Drawing.Size(222, 21)
        Me.cmbCostCentre_Man.TabIndex = 28
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(8, 85)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(76, 13)
        Me.Label32.TabIndex = 27
        Me.Label32.Text = "Cost Centre"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Red
        Me.Label10.Location = New System.Drawing.Point(685, 84)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(166, 14)
        Me.Label10.TabIndex = 26
        Me.Label10.Text = "[H] for Hallmark Details"
        '
        'lblDelete
        '
        Me.lblDelete.AutoSize = True
        Me.lblDelete.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDelete.ForeColor = System.Drawing.Color.Red
        Me.lblDelete.Location = New System.Drawing.Point(535, 84)
        Me.lblDelete.Name = "lblDelete"
        Me.lblDelete.Size = New System.Drawing.Size(144, 14)
        Me.lblDelete.TabIndex = 18
        Me.lblDelete.Text = "[M] for Misc Charges"
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDate.Location = New System.Drawing.Point(3, 11)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(86, 17)
        Me.chkDate.TabIndex = 0
        Me.chkDate.Text = "Date From"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(8, 61)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(69, 13)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "Hm Center"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(92, 105)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 19
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Enabled = False
        Me.Label8.Location = New System.Drawing.Point(320, 61)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(66, 13)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Hm Bill No"
        Me.Label8.Visible = False
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(198, 105)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 20
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'txtHmBillCenter
        '
        Me.txtHmBillCenter.Location = New System.Drawing.Point(92, 57)
        Me.txtHmBillCenter.Name = "txtHmBillCenter"
        Me.txtHmBillCenter.Size = New System.Drawing.Size(222, 21)
        Me.txtHmBillCenter.TabIndex = 15
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(304, 105)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(110, 30)
        Me.btnExit.TabIndex = 21
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'txtHmBillNo
        '
        Me.txtHmBillNo.Enabled = False
        Me.txtHmBillNo.Location = New System.Drawing.Point(392, 57)
        Me.txtHmBillNo.Name = "txtHmBillNo"
        Me.txtHmBillNo.Size = New System.Drawing.Size(134, 21)
        Me.txtHmBillNo.TabIndex = 17
        Me.txtHmBillNo.Visible = False
        '
        'txtLotNo_NUM
        '
        Me.txtLotNo_NUM.Location = New System.Drawing.Point(392, 9)
        Me.txtLotNo_NUM.Name = "txtLotNo_NUM"
        Me.txtLotNo_NUM.Size = New System.Drawing.Size(78, 21)
        Me.txtLotNo_NUM.TabIndex = 5
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(525, 105)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 23
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'txtTagNo
        '
        Me.txtTagNo.Location = New System.Drawing.Point(662, 9)
        Me.txtTagNo.Name = "txtTagNo"
        Me.txtTagNo.Size = New System.Drawing.Size(93, 21)
        Me.txtTagNo.TabIndex = 9
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(419, 105)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 22
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(314, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Lot No"
        '
        'cmbDesigner
        '
        Me.cmbDesigner.FormattingEnabled = True
        Me.cmbDesigner.Location = New System.Drawing.Point(392, 33)
        Me.cmbDesigner.Name = "cmbDesigner"
        Me.cmbDesigner.Size = New System.Drawing.Size(219, 21)
        Me.cmbDesigner.TabIndex = 13
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 37)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(53, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Counter"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(320, 37)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Designer"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(617, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(42, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "TagNo"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(631, 114)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(54, 13)
        Me.Label7.TabIndex = 24
        Me.Label7.Text = "Find Tag"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(191, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(20, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "To"
        '
        'txtFindTag
        '
        Me.txtFindTag.Location = New System.Drawing.Point(693, 110)
        Me.txtFindTag.Name = "txtFindTag"
        Me.txtFindTag.Size = New System.Drawing.Size(93, 21)
        Me.txtFindTag.TabIndex = 25
        '
        'cmbNewCounter
        '
        Me.cmbNewCounter.FormattingEnabled = True
        Me.cmbNewCounter.Location = New System.Drawing.Point(92, 33)
        Me.cmbNewCounter.Name = "cmbNewCounter"
        Me.cmbNewCounter.Size = New System.Drawing.Size(222, 21)
        Me.cmbNewCounter.TabIndex = 11
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(476, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Item Id"
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(92, 9)
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
        'txtItemId_NUM
        '
        Me.txtItemId_NUM.Location = New System.Drawing.Point(538, 9)
        Me.txtItemId_NUM.Name = "txtItemId_NUM"
        Me.txtItemId_NUM.Size = New System.Drawing.Size(73, 21)
        Me.txtItemId_NUM.TabIndex = 7
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(220, 9)
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
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'HallmarkInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(952, 523)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "HallmarkInfo"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Hall Mark Info"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtFindTag As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtItemId_NUM As System.Windows.Forms.TextBox
    Friend WithEvents chkDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents cmbNewCounter As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtTagNo As System.Windows.Forms.TextBox
    Friend WithEvents txtLotNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents cmbDesigner As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtHmBillCenter As System.Windows.Forms.TextBox
    Friend WithEvents txtHmBillNo As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblDelete As System.Windows.Forms.Label
    Friend WithEvents Label10 As Label
    Friend WithEvents cmbCostCentre_Man As ComboBox
    Friend WithEvents Label32 As Label
End Class
