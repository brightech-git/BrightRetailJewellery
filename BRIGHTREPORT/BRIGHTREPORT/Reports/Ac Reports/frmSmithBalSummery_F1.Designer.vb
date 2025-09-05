<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSmithBalSummery_F1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSmithBalSummery_F1))
        Me.pnlHeader = New System.Windows.Forms.Panel
        Me.lblTitle = New System.Windows.Forms.Label
        Me.pnlBody = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.gridViewHeader = New System.Windows.Forms.DataGridView
        Me.pnlFooter = New System.Windows.Forms.Panel
        Me.pnldetail = New System.Windows.Forms.Panel
        Me.Label9 = New System.Windows.Forms.Label
        Me.lblTranno = New System.Windows.Forms.Label
        Me.lblGrswt = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.lblBillno = New System.Windows.Forms.Label
        Me.lblTotAmt = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.lblPurewt = New System.Windows.Forms.Label
        Me.lblTouch = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.lblAlloy = New System.Windows.Forms.Label
        Me.lblStnamt = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblNetwt = New System.Windows.Forms.Label
        Me.lblRemark = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.lblTrantype = New System.Windows.Forms.Label
        Me.lblOthCharge = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblStnwt = New System.Windows.Forms.Label
        Me.lblItem = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.lblOCatName = New System.Windows.Forms.Label
        Me.lblTax = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.lblWtMode = New System.Windows.Forms.Label
        Me.lblCatName = New System.Windows.Forms.Label
        Me.Label27 = New System.Windows.Forms.Label
        Me.CatName = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.lblMc = New System.Windows.Forms.Label
        Me.lblPacketno = New System.Windows.Forms.Label
        Me.Label29 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.lblTranmode = New System.Windows.Forms.Label
        Me.lblWastage = New System.Windows.Forms.Label
        Me.Label31 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.lblStonemode = New System.Windows.Forms.Label
        Me.lblPcs = New System.Windows.Forms.Label
        Me.Label33 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.lblMetalamt = New System.Windows.Forms.Label
        Me.lblsalesman = New System.Windows.Forms.Label
        Me.Label35 = New System.Windows.Forms.Label
        Me.Label37 = New System.Windows.Forms.Label
        Me.lblApproval = New System.Windows.Forms.Label
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tStripExcel = New System.Windows.Forms.ToolStripButton
        Me.tStripPrint = New System.Windows.Forms.ToolStripButton
        Me.tStripExit = New System.Windows.Forms.ToolStripButton
        Me.TStripDetPrint = New System.Windows.Forms.ToolStripButton
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlHeader.SuspendLayout()
        Me.pnlBody.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        CType(Me.gridViewHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlFooter.SuspendLayout()
        Me.pnldetail.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.lblTitle)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 25)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(943, 39)
        Me.pnlHeader.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(943, 39)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlBody
        '
        Me.pnlBody.Controls.Add(Me.gridView)
        Me.pnlBody.Controls.Add(Me.gridViewHeader)
        Me.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlBody.Location = New System.Drawing.Point(0, 64)
        Me.pnlBody.Name = "pnlBody"
        Me.pnlBody.Size = New System.Drawing.Size(943, 272)
        Me.pnlBody.TabIndex = 0
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 19)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(943, 253)
        Me.gridView.TabIndex = 0
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(143, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'gridViewHeader
        '
        Me.gridViewHeader.AllowUserToAddRows = False
        Me.gridViewHeader.AllowUserToDeleteRows = False
        Me.gridViewHeader.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHeader.Enabled = False
        Me.gridViewHeader.Location = New System.Drawing.Point(0, 0)
        Me.gridViewHeader.Name = "gridViewHeader"
        Me.gridViewHeader.ReadOnly = True
        Me.gridViewHeader.RowHeadersVisible = False
        Me.gridViewHeader.Size = New System.Drawing.Size(943, 19)
        Me.gridViewHeader.TabIndex = 1
        '
        'pnlFooter
        '
        Me.pnlFooter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlFooter.Controls.Add(Me.pnldetail)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 336)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(943, 174)
        Me.pnlFooter.TabIndex = 0
        Me.pnlFooter.Visible = False
        '
        'pnldetail
        '
        Me.pnldetail.Controls.Add(Me.Label9)
        Me.pnldetail.Controls.Add(Me.lblTranno)
        Me.pnldetail.Controls.Add(Me.lblGrswt)
        Me.pnldetail.Controls.Add(Me.Label19)
        Me.pnldetail.Controls.Add(Me.Label15)
        Me.pnldetail.Controls.Add(Me.lblBillno)
        Me.pnldetail.Controls.Add(Me.lblTotAmt)
        Me.pnldetail.Controls.Add(Me.Label8)
        Me.pnldetail.Controls.Add(Me.lblPurewt)
        Me.pnldetail.Controls.Add(Me.lblTouch)
        Me.pnldetail.Controls.Add(Me.Label13)
        Me.pnldetail.Controls.Add(Me.Label5)
        Me.pnldetail.Controls.Add(Me.Label17)
        Me.pnldetail.Controls.Add(Me.lblAlloy)
        Me.pnldetail.Controls.Add(Me.lblStnamt)
        Me.pnldetail.Controls.Add(Me.Label4)
        Me.pnldetail.Controls.Add(Me.lblNetwt)
        Me.pnldetail.Controls.Add(Me.lblRemark)
        Me.pnldetail.Controls.Add(Me.Label21)
        Me.pnldetail.Controls.Add(Me.Label3)
        Me.pnldetail.Controls.Add(Me.Label10)
        Me.pnldetail.Controls.Add(Me.lblTrantype)
        Me.pnldetail.Controls.Add(Me.lblOthCharge)
        Me.pnldetail.Controls.Add(Me.Label2)
        Me.pnldetail.Controls.Add(Me.lblStnwt)
        Me.pnldetail.Controls.Add(Me.lblItem)
        Me.pnldetail.Controls.Add(Me.Label25)
        Me.pnldetail.Controls.Add(Me.Label7)
        Me.pnldetail.Controls.Add(Me.Label12)
        Me.pnldetail.Controls.Add(Me.lblOCatName)
        Me.pnldetail.Controls.Add(Me.lblTax)
        Me.pnldetail.Controls.Add(Me.Label6)
        Me.pnldetail.Controls.Add(Me.lblWtMode)
        Me.pnldetail.Controls.Add(Me.lblCatName)
        Me.pnldetail.Controls.Add(Me.Label27)
        Me.pnldetail.Controls.Add(Me.CatName)
        Me.pnldetail.Controls.Add(Me.Label24)
        Me.pnldetail.Controls.Add(Me.lblMc)
        Me.pnldetail.Controls.Add(Me.lblPacketno)
        Me.pnldetail.Controls.Add(Me.Label29)
        Me.pnldetail.Controls.Add(Me.Label22)
        Me.pnldetail.Controls.Add(Me.lblTranmode)
        Me.pnldetail.Controls.Add(Me.lblWastage)
        Me.pnldetail.Controls.Add(Me.Label31)
        Me.pnldetail.Controls.Add(Me.Label20)
        Me.pnldetail.Controls.Add(Me.lblStonemode)
        Me.pnldetail.Controls.Add(Me.lblPcs)
        Me.pnldetail.Controls.Add(Me.Label33)
        Me.pnldetail.Controls.Add(Me.Label18)
        Me.pnldetail.Controls.Add(Me.lblMetalamt)
        Me.pnldetail.Controls.Add(Me.lblsalesman)
        Me.pnldetail.Controls.Add(Me.Label35)
        Me.pnldetail.Controls.Add(Me.Label37)
        Me.pnldetail.Controls.Add(Me.lblApproval)
        Me.pnldetail.Location = New System.Drawing.Point(1, 4)
        Me.pnldetail.Name = "pnldetail"
        Me.pnldetail.Size = New System.Drawing.Size(940, 169)
        Me.pnldetail.TabIndex = 99
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(416, 5)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(50, 13)
        Me.Label9.TabIndex = 45
        Me.Label9.Text = "Grs Wt"
        '
        'lblTranno
        '
        Me.lblTranno.AutoSize = True
        Me.lblTranno.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTranno.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblTranno.Location = New System.Drawing.Point(98, 153)
        Me.lblTranno.Name = "lblTranno"
        Me.lblTranno.Size = New System.Drawing.Size(167, 13)
        Me.lblTranno.TabIndex = 98
        Me.lblTranno.Text = "XXXXXXXXXXXXXXXXXXXX"
        Me.lblTranno.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblGrswt
        '
        Me.lblGrswt.AutoSize = True
        Me.lblGrswt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrswt.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblGrswt.Location = New System.Drawing.Point(494, 5)
        Me.lblGrswt.Name = "lblGrswt"
        Me.lblGrswt.Size = New System.Drawing.Size(58, 13)
        Me.lblGrswt.TabIndex = 46
        Me.lblGrswt.Text = "Label11"
        Me.lblGrswt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(12, 153)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(53, 13)
        Me.Label19.TabIndex = 97
        Me.Label19.Text = "Tranno"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(416, 62)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(58, 13)
        Me.Label15.TabIndex = 47
        Me.Label15.Text = "Pure Wt"
        '
        'lblBillno
        '
        Me.lblBillno.AutoSize = True
        Me.lblBillno.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillno.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblBillno.Location = New System.Drawing.Point(98, 136)
        Me.lblBillno.Name = "lblBillno"
        Me.lblBillno.Size = New System.Drawing.Size(167, 13)
        Me.lblBillno.TabIndex = 66
        Me.lblBillno.Text = "XXXXXXXXXXXXXXXXXXXX"
        Me.lblBillno.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTotAmt
        '
        Me.lblTotAmt.AutoSize = True
        Me.lblTotAmt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotAmt.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblTotAmt.Location = New System.Drawing.Point(760, 152)
        Me.lblTotAmt.Name = "lblTotAmt"
        Me.lblTotAmt.Size = New System.Drawing.Size(58, 13)
        Me.lblTotAmt.TabIndex = 96
        Me.lblTotAmt.Text = "Label13"
        Me.lblTotAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(12, 136)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(43, 13)
        Me.Label8.TabIndex = 65
        Me.Label8.Text = "Billno"
        '
        'lblPurewt
        '
        Me.lblPurewt.AutoSize = True
        Me.lblPurewt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPurewt.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblPurewt.Location = New System.Drawing.Point(494, 62)
        Me.lblPurewt.Name = "lblPurewt"
        Me.lblPurewt.Size = New System.Drawing.Size(58, 13)
        Me.lblPurewt.TabIndex = 48
        Me.lblPurewt.Text = "Label14"
        Me.lblPurewt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTouch
        '
        Me.lblTouch.AutoSize = True
        Me.lblTouch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTouch.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblTouch.Location = New System.Drawing.Point(98, 118)
        Me.lblTouch.Name = "lblTouch"
        Me.lblTouch.Size = New System.Drawing.Size(167, 13)
        Me.lblTouch.TabIndex = 64
        Me.lblTouch.Text = "XXXXXXXXXXXXXXXXXXXX"
        Me.lblTouch.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(682, 152)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(40, 13)
        Me.Label13.TabIndex = 95
        Me.Label13.Text = "Total"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 118)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(46, 13)
        Me.Label5.TabIndex = 63
        Me.Label5.Text = "Touch"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(416, 43)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(50, 13)
        Me.Label17.TabIndex = 49
        Me.Label17.Text = "Net Wt"
        '
        'lblAlloy
        '
        Me.lblAlloy.AutoSize = True
        Me.lblAlloy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlloy.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblAlloy.Location = New System.Drawing.Point(98, 100)
        Me.lblAlloy.Name = "lblAlloy"
        Me.lblAlloy.Size = New System.Drawing.Size(167, 13)
        Me.lblAlloy.TabIndex = 62
        Me.lblAlloy.Text = "XXXXXXXXXXXXXXXXXXXX"
        Me.lblAlloy.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblStnamt
        '
        Me.lblStnamt.AutoSize = True
        Me.lblStnamt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStnamt.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblStnamt.Location = New System.Drawing.Point(760, 95)
        Me.lblStnamt.Name = "lblStnamt"
        Me.lblStnamt.Size = New System.Drawing.Size(58, 13)
        Me.lblStnamt.TabIndex = 94
        Me.lblStnamt.Text = "Label14"
        Me.lblStnamt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(12, 100)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 61
        Me.Label4.Text = "Alloy"
        '
        'lblNetwt
        '
        Me.lblNetwt.AutoSize = True
        Me.lblNetwt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNetwt.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblNetwt.Location = New System.Drawing.Point(494, 43)
        Me.lblNetwt.Name = "lblNetwt"
        Me.lblNetwt.Size = New System.Drawing.Size(58, 13)
        Me.lblNetwt.TabIndex = 50
        Me.lblNetwt.Text = "Label16"
        Me.lblNetwt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblRemark
        '
        Me.lblRemark.AutoSize = True
        Me.lblRemark.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRemark.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblRemark.Location = New System.Drawing.Point(98, 81)
        Me.lblRemark.Name = "lblRemark"
        Me.lblRemark.Size = New System.Drawing.Size(167, 13)
        Me.lblRemark.TabIndex = 60
        Me.lblRemark.Text = "XXXXXXXXXXXXXXXXXXXX"
        Me.lblRemark.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(682, 95)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(74, 13)
        Me.Label21.TabIndex = 93
        Me.Label21.Text = "Stone Amt"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 81)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 13)
        Me.Label3.TabIndex = 59
        Me.Label3.Text = "Remark"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(416, 24)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(65, 13)
        Me.Label10.TabIndex = 67
        Me.Label10.Text = "Stone Wt"
        '
        'lblTrantype
        '
        Me.lblTrantype.AutoSize = True
        Me.lblTrantype.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTrantype.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblTrantype.Location = New System.Drawing.Point(98, 63)
        Me.lblTrantype.Name = "lblTrantype"
        Me.lblTrantype.Size = New System.Drawing.Size(167, 13)
        Me.lblTrantype.TabIndex = 58
        Me.lblTrantype.Text = "XXXXXXXXXXXXXXXXXXXX"
        Me.lblTrantype.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblOthCharge
        '
        Me.lblOthCharge.AutoSize = True
        Me.lblOthCharge.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOthCharge.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblOthCharge.Location = New System.Drawing.Point(760, 114)
        Me.lblOthCharge.Name = "lblOthCharge"
        Me.lblOthCharge.Size = New System.Drawing.Size(58, 13)
        Me.lblOthCharge.TabIndex = 92
        Me.lblOthCharge.Text = "Label21"
        Me.lblOthCharge.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 13)
        Me.Label2.TabIndex = 57
        Me.Label2.Text = "Trantype"
        '
        'lblStnwt
        '
        Me.lblStnwt.AutoSize = True
        Me.lblStnwt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStnwt.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblStnwt.Location = New System.Drawing.Point(494, 24)
        Me.lblStnwt.Name = "lblStnwt"
        Me.lblStnwt.Size = New System.Drawing.Size(58, 13)
        Me.lblStnwt.TabIndex = 68
        Me.lblStnwt.Text = "Label14"
        Me.lblStnwt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblItem
        '
        Me.lblItem.AutoSize = True
        Me.lblItem.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItem.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblItem.Location = New System.Drawing.Point(98, 44)
        Me.lblItem.Name = "lblItem"
        Me.lblItem.Size = New System.Drawing.Size(167, 13)
        Me.lblItem.TabIndex = 56
        Me.lblItem.Text = "XXXXXXXXXXXXXXXXXXXX"
        Me.lblItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(682, 114)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(79, 13)
        Me.Label25.TabIndex = 91
        Me.Label25.Text = "Oth Charge"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(12, 44)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(79, 13)
        Me.Label7.TabIndex = 55
        Me.Label7.Text = "Item Name"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(416, 81)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(62, 13)
        Me.Label12.TabIndex = 69
        Me.Label12.Text = "Wt Mode"
        '
        'lblOCatName
        '
        Me.lblOCatName.AutoSize = True
        Me.lblOCatName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOCatName.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblOCatName.Location = New System.Drawing.Point(98, 25)
        Me.lblOCatName.Name = "lblOCatName"
        Me.lblOCatName.Size = New System.Drawing.Size(167, 13)
        Me.lblOCatName.TabIndex = 54
        Me.lblOCatName.Text = "XXXXXXXXXXXXXXXXXXXX"
        Me.lblOCatName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTax
        '
        Me.lblTax.AutoSize = True
        Me.lblTax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTax.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblTax.Location = New System.Drawing.Point(760, 133)
        Me.lblTax.Name = "lblTax"
        Me.lblTax.Size = New System.Drawing.Size(58, 13)
        Me.lblTax.TabIndex = 90
        Me.lblTax.Text = "Label23"
        Me.lblTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(12, 25)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(78, 13)
        Me.Label6.TabIndex = 53
        Me.Label6.Text = "O CatName"
        '
        'lblWtMode
        '
        Me.lblWtMode.AutoSize = True
        Me.lblWtMode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWtMode.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblWtMode.Location = New System.Drawing.Point(494, 81)
        Me.lblWtMode.Name = "lblWtMode"
        Me.lblWtMode.Size = New System.Drawing.Size(50, 13)
        Me.lblWtMode.TabIndex = 70
        Me.lblWtMode.Text = "Label1"
        Me.lblWtMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCatName
        '
        Me.lblCatName.AutoSize = True
        Me.lblCatName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCatName.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblCatName.Location = New System.Drawing.Point(98, 6)
        Me.lblCatName.Name = "lblCatName"
        Me.lblCatName.Size = New System.Drawing.Size(167, 13)
        Me.lblCatName.TabIndex = 52
        Me.lblCatName.Text = "XXXXXXXXXXXXXXXXXXXX"
        Me.lblCatName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.Location = New System.Drawing.Point(682, 133)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(31, 13)
        Me.Label27.TabIndex = 89
        Me.Label27.Text = "Tax"
        '
        'CatName
        '
        Me.CatName.AutoSize = True
        Me.CatName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CatName.Location = New System.Drawing.Point(12, 6)
        Me.CatName.Name = "CatName"
        Me.CatName.Size = New System.Drawing.Size(65, 13)
        Me.CatName.TabIndex = 51
        Me.CatName.Text = "CatName"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(416, 134)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(68, 13)
        Me.Label24.TabIndex = 71
        Me.Label24.Text = "PacketNo"
        '
        'lblMc
        '
        Me.lblMc.AutoSize = True
        Me.lblMc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMc.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblMc.Location = New System.Drawing.Point(760, 80)
        Me.lblMc.Name = "lblMc"
        Me.lblMc.Size = New System.Drawing.Size(50, 13)
        Me.lblMc.TabIndex = 88
        Me.lblMc.Text = "Label1"
        Me.lblMc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPacketno
        '
        Me.lblPacketno.AutoSize = True
        Me.lblPacketno.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPacketno.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblPacketno.Location = New System.Drawing.Point(494, 134)
        Me.lblPacketno.Name = "lblPacketno"
        Me.lblPacketno.Size = New System.Drawing.Size(58, 13)
        Me.lblPacketno.TabIndex = 72
        Me.lblPacketno.Text = "Label23"
        Me.lblPacketno.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(682, 80)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(25, 13)
        Me.Label29.TabIndex = 87
        Me.Label29.Text = "MC"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(416, 115)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(63, 13)
        Me.Label22.TabIndex = 73
        Me.Label22.Text = "Wastage"
        '
        'lblTranmode
        '
        Me.lblTranmode.AutoSize = True
        Me.lblTranmode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTranmode.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblTranmode.Location = New System.Drawing.Point(760, 23)
        Me.lblTranmode.Name = "lblTranmode"
        Me.lblTranmode.Size = New System.Drawing.Size(58, 13)
        Me.lblTranmode.TabIndex = 86
        Me.lblTranmode.Text = "Label14"
        Me.lblTranmode.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblWastage
        '
        Me.lblWastage.AutoSize = True
        Me.lblWastage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWastage.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblWastage.Location = New System.Drawing.Point(494, 115)
        Me.lblWastage.Name = "lblWastage"
        Me.lblWastage.Size = New System.Drawing.Size(58, 13)
        Me.lblWastage.TabIndex = 74
        Me.lblWastage.Text = "Label21"
        Me.lblWastage.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(682, 23)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(73, 13)
        Me.Label31.TabIndex = 85
        Me.Label31.Text = "Tranmode"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(416, 96)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(29, 13)
        Me.Label20.TabIndex = 75
        Me.Label20.Text = "Pcs"
        '
        'lblStonemode
        '
        Me.lblStonemode.AutoSize = True
        Me.lblStonemode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStonemode.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblStonemode.Location = New System.Drawing.Point(760, 42)
        Me.lblStonemode.Name = "lblStonemode"
        Me.lblStonemode.Size = New System.Drawing.Size(58, 13)
        Me.lblStonemode.TabIndex = 84
        Me.lblStonemode.Text = "Label32"
        Me.lblStonemode.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPcs
        '
        Me.lblPcs.AutoSize = True
        Me.lblPcs.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPcs.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblPcs.Location = New System.Drawing.Point(494, 96)
        Me.lblPcs.Name = "lblPcs"
        Me.lblPcs.Size = New System.Drawing.Size(58, 13)
        Me.lblPcs.TabIndex = 76
        Me.lblPcs.Text = "Label14"
        Me.lblPcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.Location = New System.Drawing.Point(682, 42)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(78, 13)
        Me.Label33.TabIndex = 83
        Me.Label33.Text = "StoneMode"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(416, 153)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(68, 13)
        Me.Label18.TabIndex = 77
        Me.Label18.Text = "SalesMan"
        '
        'lblMetalamt
        '
        Me.lblMetalamt.AutoSize = True
        Me.lblMetalamt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMetalamt.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblMetalamt.Location = New System.Drawing.Point(760, 61)
        Me.lblMetalamt.Name = "lblMetalamt"
        Me.lblMetalamt.Size = New System.Drawing.Size(58, 13)
        Me.lblMetalamt.TabIndex = 82
        Me.lblMetalamt.Text = "Label34"
        Me.lblMetalamt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblsalesman
        '
        Me.lblsalesman.AutoSize = True
        Me.lblsalesman.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblsalesman.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblsalesman.Location = New System.Drawing.Point(494, 153)
        Me.lblsalesman.Name = "lblsalesman"
        Me.lblsalesman.Size = New System.Drawing.Size(58, 13)
        Me.lblsalesman.TabIndex = 78
        Me.lblsalesman.Text = "Label13"
        Me.lblsalesman.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.Location = New System.Drawing.Point(682, 61)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(72, 13)
        Me.Label35.TabIndex = 81
        Me.Label35.Text = "Metal Amt"
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.Location = New System.Drawing.Point(682, 4)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(66, 13)
        Me.Label37.TabIndex = 79
        Me.Label37.Text = "Approval"
        '
        'lblApproval
        '
        Me.lblApproval.AutoSize = True
        Me.lblApproval.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblApproval.ForeColor = System.Drawing.SystemColors.Desktop
        Me.lblApproval.Location = New System.Drawing.Point(760, 4)
        Me.lblApproval.Name = "lblApproval"
        Me.lblApproval.Size = New System.Drawing.Size(58, 13)
        Me.lblApproval.TabIndex = 80
        Me.lblApproval.Text = "Label36"
        Me.lblApproval.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tStripExcel, Me.tStripPrint, Me.tStripExit, Me.TStripDetPrint})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(943, 25)
        Me.ToolStrip1.TabIndex = 5
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tStripExcel
        '
        Me.tStripExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripExcel.Image = CType(resources.GetObject("tStripExcel.Image"), System.Drawing.Image)
        Me.tStripExcel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripExcel.Name = "tStripExcel"
        Me.tStripExcel.Size = New System.Drawing.Size(23, 22)
        Me.tStripExcel.Text = "Export"
        Me.tStripExcel.ToolTipText = "Exel"
        '
        'tStripPrint
        '
        Me.tStripPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripPrint.Image = CType(resources.GetObject("tStripPrint.Image"), System.Drawing.Image)
        Me.tStripPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripPrint.Name = "tStripPrint"
        Me.tStripPrint.Size = New System.Drawing.Size(23, 22)
        Me.tStripPrint.Text = "Print"
        '
        'tStripExit
        '
        Me.tStripExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripExit.Image = Global.BrighttechREPORT.My.Resources.Resources.exit_22
        Me.tStripExit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripExit.Name = "tStripExit"
        Me.tStripExit.Size = New System.Drawing.Size(23, 22)
        Me.tStripExit.Text = "Exit"
        '
        'TStripDetPrint
        '
        Me.TStripDetPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TStripDetPrint.Image = Global.BrighttechREPORT.My.Resources.Resources.notes_22
        Me.TStripDetPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TStripDetPrint.Name = "TStripDetPrint"
        Me.TStripDetPrint.Size = New System.Drawing.Size(23, 22)
        Me.TStripDetPrint.Text = "Detail Print"
        Me.TStripDetPrint.Visible = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 26)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'frmSmithBalSummery_F1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(943, 510)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.pnlBody)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.pnlFooter)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmSmithBalSummery_F1"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Detail View"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlBody.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        CType(Me.gridViewHeader, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlFooter.ResumeLayout(False)
        Me.pnldetail.ResumeLayout(False)
        Me.pnldetail.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents pnlBody As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tStripExcel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripExit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridViewHeader As System.Windows.Forms.DataGridView
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TStripDetPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblRemark As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblTrantype As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblItem As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblOCatName As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblCatName As System.Windows.Forms.Label
    Friend WithEvents CatName As System.Windows.Forms.Label
    Friend WithEvents lblBillno As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblTouch As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblAlloy As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblTotAmt As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents lblStnamt As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents lblOthCharge As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents lblTax As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents lblMc As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents lblTranmode As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents lblStonemode As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents lblMetalamt As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents lblApproval As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents lblsalesman As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents lblPcs As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents lblWastage As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents lblPacketno As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents lblWtMode As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lblStnwt As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblNetwt As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents lblPurewt As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents lblGrswt As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lblTranno As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents pnldetail As System.Windows.Forms.Panel
End Class
