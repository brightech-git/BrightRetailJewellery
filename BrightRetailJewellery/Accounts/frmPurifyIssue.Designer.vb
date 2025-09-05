<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPurifyIssue
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.lblprepurity = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtPurewt_WET = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtReceivedWt_WET = New System.Windows.Forms.TextBox()
        Me.dtpDate = New BrighttechPack.DatePicker(Me.components)
        Me.Label9 = New System.Windows.Forms.Label()
        Me.GridView = New System.Windows.Forms.DataGridView()
        Me.cmbSmith_MAN = New System.Windows.Forms.ComboBox()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.gridViewPendingBagNo = New System.Windows.Forms.DataGridView()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.lblBullionRate = New System.Windows.Forms.Label()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtWeight_WET = New System.Windows.Forms.TextBox()
        Me.txtBagNo = New System.Windows.Forms.TextBox()
        Me.txtRemark2 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCategory = New System.Windows.Forms.TextBox()
        Me.txtPurity_PER = New System.Windows.Forms.TextBox()
        Me.txtRemark1 = New System.Windows.Forms.TextBox()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.btnBAck = New System.Windows.Forms.Button()
        Me.gridViewOpen = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridViewPendingBagNo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabView.SuspendLayout()
        CType(Me.gridViewOpen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.OpenToolStripMenuItem})
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
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(899, 521)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.lblprepurity)
        Me.tabGeneral.Controls.Add(Me.Label18)
        Me.tabGeneral.Controls.Add(Me.txtPurewt_WET)
        Me.tabGeneral.Controls.Add(Me.Label15)
        Me.tabGeneral.Controls.Add(Me.txtReceivedWt_WET)
        Me.tabGeneral.Controls.Add(Me.dtpDate)
        Me.tabGeneral.Controls.Add(Me.Label9)
        Me.tabGeneral.Controls.Add(Me.GridView)
        Me.tabGeneral.Controls.Add(Me.cmbSmith_MAN)
        Me.tabGeneral.Controls.Add(Me.btnOpen)
        Me.tabGeneral.Controls.Add(Me.Label2)
        Me.tabGeneral.Controls.Add(Me.gridViewPendingBagNo)
        Me.tabGeneral.Controls.Add(Me.Label3)
        Me.tabGeneral.Controls.Add(Me.btnSave)
        Me.tabGeneral.Controls.Add(Me.Label4)
        Me.tabGeneral.Controls.Add(Me.btnExit)
        Me.tabGeneral.Controls.Add(Me.btnNew)
        Me.tabGeneral.Controls.Add(Me.lblBullionRate)
        Me.tabGeneral.Controls.Add(Me.btnAdd)
        Me.tabGeneral.Controls.Add(Me.Label6)
        Me.tabGeneral.Controls.Add(Me.Label7)
        Me.tabGeneral.Controls.Add(Me.txtWeight_WET)
        Me.tabGeneral.Controls.Add(Me.txtBagNo)
        Me.tabGeneral.Controls.Add(Me.txtRemark2)
        Me.tabGeneral.Controls.Add(Me.Label1)
        Me.tabGeneral.Controls.Add(Me.txtCategory)
        Me.tabGeneral.Controls.Add(Me.txtPurity_PER)
        Me.tabGeneral.Controls.Add(Me.txtRemark1)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(891, 495)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'lblprepurity
        '
        Me.lblprepurity.AutoSize = True
        Me.lblprepurity.Location = New System.Drawing.Point(365, 162)
        Me.lblprepurity.Name = "lblprepurity"
        Me.lblprepurity.Size = New System.Drawing.Size(40, 13)
        Me.lblprepurity.TabIndex = 46
        Me.lblprepurity.Text = "Purity"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(6, 192)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(75, 13)
        Me.Label18.TabIndex = 15
        Me.Label18.Text = "Pure Weight"
        '
        'txtPurewt_WET
        '
        Me.txtPurewt_WET.Location = New System.Drawing.Point(122, 186)
        Me.txtPurewt_WET.Name = "txtPurewt_WET"
        Me.txtPurewt_WET.Size = New System.Drawing.Size(74, 21)
        Me.txtPurewt_WET.TabIndex = 16
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(6, 162)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(115, 13)
        Me.Label15.TabIndex = 11
        Me.Label15.Text = "Issued Grs. Weight"
        '
        'txtReceivedWt_WET
        '
        Me.txtReceivedWt_WET.Location = New System.Drawing.Point(122, 154)
        Me.txtReceivedWt_WET.Name = "txtReceivedWt_WET"
        Me.txtReceivedWt_WET.Size = New System.Drawing.Size(74, 21)
        Me.txtReceivedWt_WET.TabIndex = 12
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(122, 9)
        Me.dtpDate.Mask = "##/##/####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDate.Size = New System.Drawing.Size(100, 21)
        Me.dtpDate.TabIndex = 1
        Me.dtpDate.Text = "06/03/9998"
        Me.dtpDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 12)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(34, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Date"
        '
        'GridView
        '
        Me.GridView.AllowUserToAddRows = False
        Me.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView.Location = New System.Drawing.Point(3, 333)
        Me.GridView.Name = "GridView"
        Me.GridView.ReadOnly = True
        Me.GridView.Size = New System.Drawing.Size(871, 157)
        Me.GridView.TabIndex = 25
        '
        'cmbSmith_MAN
        '
        Me.cmbSmith_MAN.FormattingEnabled = True
        Me.cmbSmith_MAN.Location = New System.Drawing.Point(122, 37)
        Me.cmbSmith_MAN.Name = "cmbSmith_MAN"
        Me.cmbSmith_MAN.Size = New System.Drawing.Size(345, 21)
        Me.cmbSmith_MAN.TabIndex = 4
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(217, 282)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 22
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Bag No"
        '
        'gridViewPendingBagNo
        '
        Me.gridViewPendingBagNo.AllowUserToAddRows = False
        Me.gridViewPendingBagNo.AllowUserToDeleteRows = False
        Me.gridViewPendingBagNo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewPendingBagNo.Location = New System.Drawing.Point(516, 12)
        Me.gridViewPendingBagNo.Name = "gridViewPendingBagNo"
        Me.gridViewPendingBagNo.ReadOnly = True
        Me.gridViewPendingBagNo.RowHeadersVisible = False
        Me.gridViewPendingBagNo.Size = New System.Drawing.Size(368, 244)
        Me.gridViewPendingBagNo.TabIndex = 26
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 99)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Category"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(112, 282)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 21
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(228, 162)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Purity"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(428, 282)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 24
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(323, 282)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 23
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'lblBullionRate
        '
        Me.lblBullionRate.AutoSize = True
        Me.lblBullionRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBullionRate.ForeColor = System.Drawing.Color.Red
        Me.lblBullionRate.Location = New System.Drawing.Point(380, 9)
        Me.lblBullionRate.Name = "lblBullionRate"
        Me.lblBullionRate.Size = New System.Drawing.Size(84, 13)
        Me.lblBullionRate.TabIndex = 2
        Me.lblBullionRate.Text = "Bullion Rate"
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(6, 282)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(100, 30)
        Me.btnAdd.TabIndex = 20
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 130)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(108, 13)
        Me.Label6.TabIndex = 9
        Me.Label6.Text = "Bag Gross Weight"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 229)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(52, 13)
        Me.Label7.TabIndex = 17
        Me.Label7.Text = "Remark"
        '
        'txtWeight_WET
        '
        Me.txtWeight_WET.Location = New System.Drawing.Point(122, 127)
        Me.txtWeight_WET.Name = "txtWeight_WET"
        Me.txtWeight_WET.Size = New System.Drawing.Size(74, 21)
        Me.txtWeight_WET.TabIndex = 10
        '
        'txtBagNo
        '
        Me.txtBagNo.Location = New System.Drawing.Point(122, 66)
        Me.txtBagNo.Name = "txtBagNo"
        Me.txtBagNo.Size = New System.Drawing.Size(74, 21)
        Me.txtBagNo.TabIndex = 6
        '
        'txtRemark2
        '
        Me.txtRemark2.Location = New System.Drawing.Point(122, 241)
        Me.txtRemark2.MaxLength = 50
        Me.txtRemark2.Name = "txtRemark2"
        Me.txtRemark2.Size = New System.Drawing.Size(345, 21)
        Me.txtRemark2.TabIndex = 19
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Smith Name"
        '
        'txtCategory
        '
        Me.txtCategory.Location = New System.Drawing.Point(122, 96)
        Me.txtCategory.MaxLength = 50
        Me.txtCategory.Name = "txtCategory"
        Me.txtCategory.Size = New System.Drawing.Size(345, 21)
        Me.txtCategory.TabIndex = 8
        '
        'txtPurity_PER
        '
        Me.txtPurity_PER.Location = New System.Drawing.Point(274, 154)
        Me.txtPurity_PER.Name = "txtPurity_PER"
        Me.txtPurity_PER.Size = New System.Drawing.Size(74, 21)
        Me.txtPurity_PER.TabIndex = 14
        '
        'txtRemark1
        '
        Me.txtRemark1.Location = New System.Drawing.Point(122, 221)
        Me.txtRemark1.MaxLength = 50
        Me.txtRemark1.Name = "txtRemark1"
        Me.txtRemark1.Size = New System.Drawing.Size(345, 21)
        Me.txtRemark1.TabIndex = 18
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.btnPrint)
        Me.tabView.Controls.Add(Me.btnExport)
        Me.tabView.Controls.Add(Me.Label10)
        Me.tabView.Controls.Add(Me.btnBAck)
        Me.tabView.Controls.Add(Me.gridViewOpen)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(887, 495)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(773, 425)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 4
        Me.btnPrint.TabStop = False
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(666, 425)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 3
        Me.btnExport.TabStop = False
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Red
        Me.Label10.Location = New System.Drawing.Point(114, 434)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(74, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "[C] Cancel"
        '
        'btnBAck
        '
        Me.btnBAck.Location = New System.Drawing.Point(8, 425)
        Me.btnBAck.Name = "btnBAck"
        Me.btnBAck.Size = New System.Drawing.Size(100, 30)
        Me.btnBAck.TabIndex = 1
        Me.btnBAck.Text = "&Back"
        Me.btnBAck.UseVisualStyleBackColor = True
        '
        'gridViewOpen
        '
        Me.gridViewOpen.AllowUserToAddRows = False
        Me.gridViewOpen.AllowUserToDeleteRows = False
        Me.gridViewOpen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewOpen.Location = New System.Drawing.Point(8, 6)
        Me.gridViewOpen.Name = "gridViewOpen"
        Me.gridViewOpen.ReadOnly = True
        Me.gridViewOpen.Size = New System.Drawing.Size(871, 413)
        Me.gridViewOpen.TabIndex = 0
        '
        'frmPurifyIssue
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(899, 521)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmPurifyIssue"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Issue to Purification"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridViewPendingBagNo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabView.ResumeLayout(False)
        Me.tabView.PerformLayout()
        CType(Me.gridViewOpen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents btnBAck As System.Windows.Forms.Button
    Friend WithEvents gridViewOpen As System.Windows.Forms.DataGridView
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtPurewt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtReceivedWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents dtpDate As BrighttechPack.DatePicker
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents GridView As System.Windows.Forms.DataGridView
    Friend WithEvents cmbSmith_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents gridViewPendingBagNo As System.Windows.Forms.DataGridView
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents lblBullionRate As System.Windows.Forms.Label
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtBagNo As System.Windows.Forms.TextBox
    Friend WithEvents txtRemark2 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCategory As System.Windows.Forms.TextBox
    Friend WithEvents txtPurity_PER As System.Windows.Forms.TextBox
    Friend WithEvents txtRemark1 As System.Windows.Forms.TextBox
    Friend WithEvents btnPrint As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents lblprepurity As Label
End Class
