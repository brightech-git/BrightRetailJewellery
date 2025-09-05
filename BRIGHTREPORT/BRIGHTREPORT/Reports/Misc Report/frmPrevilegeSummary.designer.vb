<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPrevilegeSummary
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
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.gridViewHead = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExtToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.lblcustname = New System.Windows.Forms.Label()
        Me.chkchit = New System.Windows.Forms.CheckBox()
        Me.chkfromtorange = New System.Windows.Forms.CheckBox()
        Me.txtFrom_Point_NUM = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtTo_Point_NUM = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtMsg_OWN = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkWithAddress = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkReceiptSummary = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtPrevilegeId = New System.Windows.Forms.TextBox()
        Me.grpRange = New System.Windows.Forms.GroupBox()
        Me.grpASD = New System.Windows.Forms.GroupBox()
        Me.dtpFrom_OWN = New BrighttechPack.DatePicker(Me.components)
        Me.dtpTo_OWN = New BrighttechPack.DatePicker(Me.components)
        Me.chkASD = New System.Windows.Forms.CheckBox()
        Me.btnSend = New System.Windows.Forms.Button()
        Me.chksms = New System.Windows.Forms.CheckBox()
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.grpRange.SuspendLayout()
        Me.grpASD.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridView)
        Me.Panel2.Controls.Add(Me.gridViewHead)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel2.Location = New System.Drawing.Point(0, 126)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1024, 471)
        Me.Panel2.TabIndex = 1
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 19)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 21
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.Size = New System.Drawing.Size(1024, 452)
        Me.gridView.TabIndex = 1
        '
        'gridViewHead
        '
        Me.gridViewHead.AllowUserToAddRows = False
        Me.gridViewHead.AllowUserToDeleteRows = False
        Me.gridViewHead.BackgroundColor = System.Drawing.SystemColors.ControlDark
        Me.gridViewHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridViewHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHead.Enabled = False
        Me.gridViewHead.Location = New System.Drawing.Point(0, 0)
        Me.gridViewHead.Name = "gridViewHead"
        Me.gridViewHead.ReadOnly = True
        Me.gridViewHead.RowHeadersVisible = False
        Me.gridViewHead.Size = New System.Drawing.Size(1024, 19)
        Me.gridViewHead.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 101)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1024, 25)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExtToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExtToolStripMenuItem
        '
        Me.ExtToolStripMenuItem.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ExtToolStripMenuItem.Name = "ExtToolStripMenuItem"
        Me.ExtToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExtToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExtToolStripMenuItem.Text = "Exit"
        Me.ExtToolStripMenuItem.Visible = False
        '
        'btnView_Search
        '
        Me.btnView_Search.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView_Search.Location = New System.Drawing.Point(13, 67)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 7
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(328, 67)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 11
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.Location = New System.Drawing.Point(117, 67)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 8
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'lblTo
        '
        Me.lblTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTo.Location = New System.Drawing.Point(102, 14)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(23, 21)
        Me.lblTo.TabIndex = 1
        Me.lblTo.Text = "To"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExport
        '
        Me.btnExport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExport.Location = New System.Drawing.Point(222, 67)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 10
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(434, 67)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 12
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'lblcustname
        '
        Me.lblcustname.AutoSize = True
        Me.lblcustname.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcustname.ForeColor = System.Drawing.Color.Red
        Me.lblcustname.Location = New System.Drawing.Point(189, 52)
        Me.lblcustname.Name = "lblcustname"
        Me.lblcustname.Size = New System.Drawing.Size(0, 13)
        Me.lblcustname.TabIndex = 9
        Me.lblcustname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkchit
        '
        Me.chkchit.AutoSize = True
        Me.chkchit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkchit.Location = New System.Drawing.Point(6, 15)
        Me.chkchit.Name = "chkchit"
        Me.chkchit.Size = New System.Drawing.Size(101, 17)
        Me.chkchit.TabIndex = 0
        Me.chkchit.Text = "With Scheme"
        Me.chkchit.UseVisualStyleBackColor = True
        '
        'chkfromtorange
        '
        Me.chkfromtorange.AutoSize = True
        Me.chkfromtorange.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkfromtorange.Location = New System.Drawing.Point(652, 19)
        Me.chkfromtorange.Name = "chkfromtorange"
        Me.chkfromtorange.Size = New System.Drawing.Size(93, 17)
        Me.chkfromtorange.TabIndex = 5
        Me.chkfromtorange.Text = "Range Wise"
        Me.chkfromtorange.UseVisualStyleBackColor = True
        '
        'txtFrom_Point_NUM
        '
        Me.txtFrom_Point_NUM.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFrom_Point_NUM.Location = New System.Drawing.Point(55, 12)
        Me.txtFrom_Point_NUM.MaxLength = 8
        Me.txtFrom_Point_NUM.Name = "txtFrom_Point_NUM"
        Me.txtFrom_Point_NUM.Size = New System.Drawing.Size(77, 21)
        Me.txtFrom_Point_NUM.TabIndex = 1
        Me.txtFrom_Point_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(139, 16)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(20, 13)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "To"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtTo_Point_NUM
        '
        Me.txtTo_Point_NUM.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTo_Point_NUM.Location = New System.Drawing.Point(169, 12)
        Me.txtTo_Point_NUM.MaxLength = 8
        Me.txtTo_Point_NUM.Name = "txtTo_Point_NUM"
        Me.txtTo_Point_NUM.Size = New System.Drawing.Size(77, 21)
        Me.txtTo_Point_NUM.TabIndex = 3
        Me.txtTo_Point_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(11, 16)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(36, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "From"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMsg_OWN
        '
        Me.txtMsg_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMsg_OWN.Location = New System.Drawing.Point(744, 48)
        Me.txtMsg_OWN.MaxLength = 3000
        Me.txtMsg_OWN.Multiline = True
        Me.txtMsg_OWN.Name = "txtMsg_OWN"
        Me.txtMsg_OWN.Size = New System.Drawing.Size(268, 49)
        Me.txtMsg_OWN.TabIndex = 15
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txtMsg_OWN)
        Me.Panel1.Controls.Add(Me.chkWithAddress)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.lblTitle)
        Me.Panel1.Controls.Add(Me.grpRange)
        Me.Panel1.Controls.Add(Me.grpASD)
        Me.Panel1.Controls.Add(Me.chkASD)
        Me.Panel1.Controls.Add(Me.btnSend)
        Me.Panel1.Controls.Add(Me.chksms)
        Me.Panel1.Controls.Add(Me.chkfromtorange)
        Me.Panel1.Controls.Add(Me.lblcustname)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnView_Search)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1024, 126)
        Me.Panel1.TabIndex = 0
        '
        'chkWithAddress
        '
        Me.chkWithAddress.AutoSize = True
        Me.chkWithAddress.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkWithAddress.Location = New System.Drawing.Point(652, 75)
        Me.chkWithAddress.Name = "chkWithAddress"
        Me.chkWithAddress.Size = New System.Drawing.Size(97, 17)
        Me.chkWithAddress.TabIndex = 16
        Me.chkWithAddress.Text = "Incl Address"
        Me.chkWithAddress.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.chkReceiptSummary)
        Me.GroupBox1.Controls.Add(Me.chkchit)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtPrevilegeId)
        Me.GroupBox1.Location = New System.Drawing.Point(328, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(312, 58)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(167, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(139, 14)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Press [D] For Detail"
        '
        'chkReceiptSummary
        '
        Me.chkReceiptSummary.AutoSize = True
        Me.chkReceiptSummary.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReceiptSummary.Location = New System.Drawing.Point(6, 38)
        Me.chkReceiptSummary.Name = "chkReceiptSummary"
        Me.chkReceiptSummary.Size = New System.Drawing.Size(128, 17)
        Me.chkReceiptSummary.TabIndex = 3
        Me.chkReceiptSummary.Text = "Receipt Summary"
        Me.chkReceiptSummary.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(122, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Previlege ID"
        '
        'txtPrevilegeId
        '
        Me.txtPrevilegeId.Location = New System.Drawing.Point(206, 13)
        Me.txtPrevilegeId.Name = "txtPrevilegeId"
        Me.txtPrevilegeId.Size = New System.Drawing.Size(100, 21)
        Me.txtPrevilegeId.TabIndex = 2
        '
        'grpRange
        '
        Me.grpRange.Controls.Add(Me.txtTo_Point_NUM)
        Me.grpRange.Controls.Add(Me.txtFrom_Point_NUM)
        Me.grpRange.Controls.Add(Me.Label8)
        Me.grpRange.Controls.Add(Me.Label7)
        Me.grpRange.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpRange.Location = New System.Drawing.Point(744, 4)
        Me.grpRange.Name = "grpRange"
        Me.grpRange.Size = New System.Drawing.Size(268, 38)
        Me.grpRange.TabIndex = 6
        Me.grpRange.TabStop = False
        '
        'grpASD
        '
        Me.grpASD.Controls.Add(Me.dtpFrom_OWN)
        Me.grpASD.Controls.Add(Me.lblTo)
        Me.grpASD.Controls.Add(Me.dtpTo_OWN)
        Me.grpASD.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpASD.Location = New System.Drawing.Point(98, 3)
        Me.grpASD.Name = "grpASD"
        Me.grpASD.Size = New System.Drawing.Size(226, 39)
        Me.grpASD.TabIndex = 1
        Me.grpASD.TabStop = False
        '
        'dtpFrom_OWN
        '
        Me.dtpFrom_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpFrom_OWN.Location = New System.Drawing.Point(7, 12)
        Me.dtpFrom_OWN.Mask = "##/##/####"
        Me.dtpFrom_OWN.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom_OWN.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom_OWN.Name = "dtpFrom_OWN"
        Me.dtpFrom_OWN.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom_OWN.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom_OWN.TabIndex = 0
        Me.dtpFrom_OWN.Text = "07/03/9998"
        Me.dtpFrom_OWN.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpTo_OWN
        '
        Me.dtpTo_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpTo_OWN.Location = New System.Drawing.Point(127, 13)
        Me.dtpTo_OWN.Mask = "##/##/####"
        Me.dtpTo_OWN.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo_OWN.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo_OWN.Name = "dtpTo_OWN"
        Me.dtpTo_OWN.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo_OWN.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo_OWN.TabIndex = 2
        Me.dtpTo_OWN.Text = "07/03/9998"
        Me.dtpTo_OWN.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkASD
        '
        Me.chkASD.AutoSize = True
        Me.chkASD.Checked = True
        Me.chkASD.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkASD.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkASD.Location = New System.Drawing.Point(13, 17)
        Me.chkASD.Name = "chkASD"
        Me.chkASD.Size = New System.Drawing.Size(83, 17)
        Me.chkASD.TabIndex = 0
        Me.chkASD.Text = "AsOnDate"
        Me.chkASD.UseVisualStyleBackColor = True
        '
        'btnSend
        '
        Me.btnSend.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSend.Location = New System.Drawing.Point(540, 67)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(100, 30)
        Me.btnSend.TabIndex = 13
        Me.btnSend.Text = "Send"
        Me.btnSend.UseVisualStyleBackColor = True
        '
        'chksms
        '
        Me.chksms.AutoSize = True
        Me.chksms.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chksms.Location = New System.Drawing.Point(652, 51)
        Me.chksms.Name = "chksms"
        Me.chksms.Size = New System.Drawing.Size(84, 17)
        Me.chksms.TabIndex = 14
        Me.chksms.Text = "Send Sms"
        Me.chksms.UseVisualStyleBackColor = True
        '
        'frmPrevilegeSummary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1024, 597)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.ForeColor = System.Drawing.Color.Black
        Me.Name = "frmPrevilegeSummary"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Previlege Summary Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.grpRange.ResumeLayout(False)
        Me.grpRange.PerformLayout()
        Me.grpASD.ResumeLayout(False)
        Me.grpASD.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExtToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridViewHead As System.Windows.Forms.DataGridView
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents dtpFrom_OWN As BrighttechPack.DatePicker
    Friend WithEvents dtpTo_OWN As BrighttechPack.DatePicker
    Friend WithEvents lblcustname As System.Windows.Forms.Label
    Friend WithEvents chkchit As System.Windows.Forms.CheckBox
    Friend WithEvents chkfromtorange As System.Windows.Forms.CheckBox
    Friend WithEvents txtFrom_Point_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtTo_Point_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtMsg_OWN As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chksms As System.Windows.Forms.CheckBox
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents chkASD As System.Windows.Forms.CheckBox
    Friend WithEvents grpASD As System.Windows.Forms.GroupBox
    Friend WithEvents grpRange As System.Windows.Forms.GroupBox
    Friend WithEvents txtPrevilegeId As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents chkReceiptSummary As CheckBox
    Friend WithEvents Label2 As Label
    Friend WithEvents chkWithAddress As CheckBox
End Class
