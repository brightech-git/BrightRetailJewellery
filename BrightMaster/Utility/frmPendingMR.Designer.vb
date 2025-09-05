<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPendingMR
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
        Me.Dgv = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.dtpfromdate = New BrighttechPack.DatePicker(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpTodate = New BrighttechPack.DatePicker(Me.components)
        Me.chkWithFromDate = New System.Windows.Forms.CheckBox()
        Me.btnManufacturing = New System.Windows.Forms.Button()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.ChkCmbCompany = New BrighttechPack.CheckedComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblHelp = New System.Windows.Forms.Label()
        Me.txtTranNo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblTranDate = New System.Windows.Forms.Label()
        Me.dtpTransfer = New BrighttechPack.DatePicker(Me.components)
        Me.btnTransfer = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGen = New System.Windows.Forms.TabPage()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.gridview = New System.Windows.Forms.DataGridView()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.gridViewHead = New System.Windows.Forms.DataGridView()
        Me.pnlBottom = New System.Windows.Forms.Panel()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.dtpTo2 = New BrighttechPack.DatePicker(Me.components)
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.dtpFrom2 = New BrighttechPack.DatePicker(Me.components)
        Me.btnView = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoSizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.Dgv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlBottom.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Dgv
        '
        Me.Dgv.AllowUserToAddRows = False
        Me.Dgv.AllowUserToDeleteRows = False
        Me.Dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Dgv.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Dgv.Location = New System.Drawing.Point(3, 118)
        Me.Dgv.Name = "Dgv"
        Me.Dgv.Size = New System.Drawing.Size(1002, 469)
        Me.Dgv.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.dtpfromdate)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.dtpTodate)
        Me.Panel1.Controls.Add(Me.chkWithFromDate)
        Me.Panel1.Controls.Add(Me.btnManufacturing)
        Me.Panel1.Controls.Add(Me.btnOpen)
        Me.Panel1.Controls.Add(Me.ChkCmbCompany)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.lblHelp)
        Me.Panel1.Controls.Add(Me.txtTranNo)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.lblTranDate)
        Me.Panel1.Controls.Add(Me.dtpTransfer)
        Me.Panel1.Controls.Add(Me.btnTransfer)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1002, 115)
        Me.Panel1.TabIndex = 0
        '
        'dtpfromdate
        '
        Me.dtpfromdate.Location = New System.Drawing.Point(275, 11)
        Me.dtpfromdate.Mask = "##/##/####"
        Me.dtpfromdate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpfromdate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpfromdate.Name = "dtpfromdate"
        Me.dtpfromdate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpfromdate.Size = New System.Drawing.Size(81, 21)
        Me.dtpfromdate.TabIndex = 3
        Me.dtpfromdate.Text = "07/03/9998"
        Me.dtpfromdate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        Me.dtpfromdate.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Enabled = False
        Me.Label2.Location = New System.Drawing.Point(363, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "To"
        '
        'dtpTodate
        '
        Me.dtpTodate.Location = New System.Drawing.Point(392, 11)
        Me.dtpTodate.Mask = "##/##/####"
        Me.dtpTodate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTodate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTodate.Name = "dtpTodate"
        Me.dtpTodate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTodate.Size = New System.Drawing.Size(81, 21)
        Me.dtpTodate.TabIndex = 5
        Me.dtpTodate.Text = "07/03/9998"
        Me.dtpTodate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        Me.dtpTodate.Visible = False
        '
        'chkWithFromDate
        '
        Me.chkWithFromDate.Location = New System.Drawing.Point(192, 13)
        Me.chkWithFromDate.Name = "chkWithFromDate"
        Me.chkWithFromDate.Size = New System.Drawing.Size(94, 17)
        Me.chkWithFromDate.TabIndex = 2
        Me.chkWithFromDate.Text = "ChkDate"
        Me.chkWithFromDate.UseVisualStyleBackColor = True
        '
        'btnManufacturing
        '
        Me.btnManufacturing.Location = New System.Drawing.Point(631, 72)
        Me.btnManufacturing.Name = "btnManufacturing"
        Me.btnManufacturing.Size = New System.Drawing.Size(100, 30)
        Me.btnManufacturing.TabIndex = 0
        Me.btnManufacturing.Text = "Manufacturing"
        Me.btnManufacturing.UseVisualStyleBackColor = True
        Me.btnManufacturing.Visible = False
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(418, 72)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 14
        Me.btnOpen.Text = "&Open"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'ChkCmbCompany
        '
        Me.ChkCmbCompany.CheckOnClick = True
        Me.ChkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkCmbCompany.DropDownHeight = 3
        Me.ChkCmbCompany.FormattingEnabled = True
        Me.ChkCmbCompany.IntegralHeight = False
        Me.ChkCmbCompany.Location = New System.Drawing.Point(275, 40)
        Me.ChkCmbCompany.Name = "ChkCmbCompany"
        Me.ChkCmbCompany.Size = New System.Drawing.Size(314, 22)
        Me.ChkCmbCompany.TabIndex = 9
        Me.ChkCmbCompany.ValueSeparator = ", "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(192, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Company"
        '
        'lblHelp
        '
        Me.lblHelp.AutoSize = True
        Me.lblHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHelp.ForeColor = System.Drawing.Color.Red
        Me.lblHelp.Location = New System.Drawing.Point(759, 10)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(80, 13)
        Me.lblHelp.TabIndex = 1
        Me.lblHelp.Text = "SoftControl"
        '
        'txtTranNo
        '
        Me.txtTranNo.Location = New System.Drawing.Point(97, 41)
        Me.txtTranNo.Name = "txtTranNo"
        Me.txtTranNo.Size = New System.Drawing.Size(85, 21)
        Me.txtTranNo.TabIndex = 7
        Me.txtTranNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(14, 47)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Transfer No"
        '
        'lblTranDate
        '
        Me.lblTranDate.AutoSize = True
        Me.lblTranDate.Enabled = False
        Me.lblTranDate.Location = New System.Drawing.Point(14, 15)
        Me.lblTranDate.Name = "lblTranDate"
        Me.lblTranDate.Size = New System.Drawing.Size(81, 13)
        Me.lblTranDate.TabIndex = 0
        Me.lblTranDate.Text = "TransferDate"
        '
        'dtpTransfer
        '
        Me.dtpTransfer.Enabled = False
        Me.dtpTransfer.Location = New System.Drawing.Point(97, 11)
        Me.dtpTransfer.Mask = "##/##/####"
        Me.dtpTransfer.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTransfer.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTransfer.Name = "dtpTransfer"
        Me.dtpTransfer.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTransfer.Size = New System.Drawing.Size(81, 21)
        Me.dtpTransfer.TabIndex = 1
        Me.dtpTransfer.Text = "07/03/9998"
        Me.dtpTransfer.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnTransfer
        '
        Me.btnTransfer.Location = New System.Drawing.Point(311, 72)
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.Size = New System.Drawing.Size(100, 30)
        Me.btnTransfer.TabIndex = 13
        Me.btnTransfer.Text = "&Transfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(525, 72)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 16
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(204, 72)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 11
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(97, 72)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 10
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1016, 616)
        Me.tabMain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.Dgv)
        Me.tabGen.Controls.Add(Me.Panel1)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1008, 590)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.Panel2)
        Me.tabView.Controls.Add(Me.pnlBottom)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1008, 590)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridview)
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 53)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1002, 534)
        Me.Panel2.TabIndex = 5
        '
        'gridview
        '
        Me.gridview.AllowUserToAddRows = False
        Me.gridview.AllowUserToDeleteRows = False
        Me.gridview.AllowUserToResizeColumns = False
        Me.gridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridview.Location = New System.Drawing.Point(0, 16)
        Me.gridview.Name = "gridview"
        Me.gridview.ReadOnly = True
        Me.gridview.RowHeadersVisible = False
        Me.gridview.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridview.Size = New System.Drawing.Size(1002, 518)
        Me.gridview.TabIndex = 0
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.gridViewHead)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1002, 16)
        Me.Panel3.TabIndex = 6
        '
        'gridViewHead
        '
        Me.gridViewHead.AllowUserToAddRows = False
        Me.gridViewHead.AllowUserToDeleteRows = False
        Me.gridViewHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHead.Location = New System.Drawing.Point(0, 0)
        Me.gridViewHead.Name = "gridViewHead"
        Me.gridViewHead.ReadOnly = True
        Me.gridViewHead.RowHeadersVisible = False
        Me.gridViewHead.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridViewHead.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridViewHead.Size = New System.Drawing.Size(1002, 16)
        Me.gridViewHead.TabIndex = 0
        Me.gridViewHead.Visible = False
        '
        'pnlBottom
        '
        Me.pnlBottom.Controls.Add(Me.lblTo)
        Me.pnlBottom.Controls.Add(Me.dtpTo2)
        Me.pnlBottom.Controls.Add(Me.lblFrom)
        Me.pnlBottom.Controls.Add(Me.dtpFrom2)
        Me.pnlBottom.Controls.Add(Me.btnView)
        Me.pnlBottom.Controls.Add(Me.btnExport)
        Me.pnlBottom.Controls.Add(Me.btnPrint)
        Me.pnlBottom.Controls.Add(Me.Button3)
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlBottom.Location = New System.Drawing.Point(3, 3)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(1002, 50)
        Me.pnlBottom.TabIndex = 0
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(156, 18)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(20, 13)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        '
        'dtpTo2
        '
        Me.dtpTo2.Location = New System.Drawing.Point(182, 15)
        Me.dtpTo2.Mask = "##/##/####"
        Me.dtpTo2.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo2.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo2.Name = "dtpTo2"
        Me.dtpTo2.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo2.Size = New System.Drawing.Size(85, 21)
        Me.dtpTo2.TabIndex = 3
        Me.dtpTo2.Text = "07/03/9998"
        Me.dtpTo2.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(25, 18)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(36, 13)
        Me.lblFrom.TabIndex = 0
        Me.lblFrom.Text = "From"
        '
        'dtpFrom2
        '
        Me.dtpFrom2.Location = New System.Drawing.Point(66, 14)
        Me.dtpFrom2.Mask = "##/##/####"
        Me.dtpFrom2.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom2.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom2.Name = "dtpFrom2"
        Me.dtpFrom2.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom2.Size = New System.Drawing.Size(85, 21)
        Me.dtpFrom2.TabIndex = 1
        Me.dtpFrom2.Text = "07/03/9998"
        Me.dtpFrom2.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnView
        '
        Me.btnView.Location = New System.Drawing.Point(272, 9)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(100, 30)
        Me.btnView.TabIndex = 4
        Me.btnView.Text = "&View"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(377, 9)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 5
        Me.btnExport.Text = "&Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(482, 9)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 6
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(587, 9)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(100, 30)
        Me.Button3.TabIndex = 7
        Me.Button3.Text = "Back[ESC]"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoSizeToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(121, 26)
        '
        'AutoSizeToolStripMenuItem
        '
        Me.AutoSizeToolStripMenuItem.Checked = True
        Me.AutoSizeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AutoSizeToolStripMenuItem.Name = "AutoSizeToolStripMenuItem"
        Me.AutoSizeToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.AutoSizeToolStripMenuItem.Text = "AutoSize"
        '
        'frmPendingMR
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1016, 616)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmPendingMR"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AccApproval"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.Dgv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlBottom.ResumeLayout(False)
        Me.pnlBottom.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Dgv As System.Windows.Forms.DataGridView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnTransfer As System.Windows.Forms.Button
    Friend WithEvents lblTranDate As System.Windows.Forms.Label
    Friend WithEvents dtpTransfer As BrighttechPack.DatePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtTranNo As System.Windows.Forms.TextBox
    Friend WithEvents lblHelp As System.Windows.Forms.Label
    Friend WithEvents ChkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents pnlBottom As System.Windows.Forms.Panel
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents gridview As System.Windows.Forms.DataGridView
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents gridViewHead As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoSizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents dtpTo2 As BrighttechPack.DatePicker
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents dtpFrom2 As BrighttechPack.DatePicker
    Friend WithEvents btnManufacturing As Button
    Friend WithEvents chkWithFromDate As CheckBox
    Friend WithEvents dtpTodate As BrighttechPack.DatePicker
    Friend WithEvents dtpfromdate As BrighttechPack.DatePicker
    Friend WithEvents Label2 As Label
End Class
