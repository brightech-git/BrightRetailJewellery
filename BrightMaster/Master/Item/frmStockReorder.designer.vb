<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStockReorder
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
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.GRPfIELDS = New System.Windows.Forms.GroupBox()
        Me.txtStkReordCaption = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.grpDesignOrder = New System.Windows.Forms.GroupBox()
        Me.txtLeadTime = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbDesignerName = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtReOrder = New System.Windows.Forms.TextBox()
        Me.lblReOrderPce = New System.Windows.Forms.Label()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.pnlControls = New System.Windows.Forms.Panel()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.cmbItemCounter = New System.Windows.Forms.ComboBox()
        Me.cmbRangeMode = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbSize = New System.Windows.Forms.ComboBox()
        Me.lblSize = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbSubItem_Man = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbCostCentre_own = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbItem_Man = New System.Windows.Forms.ComboBox()
        Me.cmbMetal_Man = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.lblWtFrom = New System.Windows.Forms.Label()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.lblWtTo = New System.Windows.Forms.Label()
        Me.txtWeight = New System.Windows.Forms.TextBox()
        Me.txtPiece = New System.Windows.Forms.TextBox()
        Me.minLbl = New System.Windows.Forms.Label()
        Me.txtWtTo = New System.Windows.Forms.TextBox()
        Me.lblWt = New System.Windows.Forms.Label()
        Me.txtWtFrom_Own = New System.Windows.Forms.TextBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NEwToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmbSubItemSearch_OWN = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbItemSearch_OWN = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbCostSearch_OWN = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GRPfIELDS.SuspendLayout()
        Me.grpDesignOrder.SuspendLayout()
        Me.pnlControls.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(12, 296)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(810, 257)
        Me.gridView.TabIndex = 2
        '
        'GRPfIELDS
        '
        Me.GRPfIELDS.BackColor = System.Drawing.Color.Transparent
        Me.GRPfIELDS.Controls.Add(Me.txtStkReordCaption)
        Me.GRPfIELDS.Controls.Add(Me.Label12)
        Me.GRPfIELDS.Controls.Add(Me.grpDesignOrder)
        Me.GRPfIELDS.Controls.Add(Me.btnDelete)
        Me.GRPfIELDS.Controls.Add(Me.pnlControls)
        Me.GRPfIELDS.Controls.Add(Me.btnExit)
        Me.GRPfIELDS.Controls.Add(Me.btnNew)
        Me.GRPfIELDS.Controls.Add(Me.lblWtFrom)
        Me.GRPfIELDS.Controls.Add(Me.btnOpen)
        Me.GRPfIELDS.Controls.Add(Me.btnSave)
        Me.GRPfIELDS.Controls.Add(Me.lblWtTo)
        Me.GRPfIELDS.Controls.Add(Me.txtWeight)
        Me.GRPfIELDS.Controls.Add(Me.txtPiece)
        Me.GRPfIELDS.Controls.Add(Me.minLbl)
        Me.GRPfIELDS.Controls.Add(Me.txtWtTo)
        Me.GRPfIELDS.Controls.Add(Me.lblWt)
        Me.GRPfIELDS.Controls.Add(Me.txtWtFrom_Own)
        Me.GRPfIELDS.Location = New System.Drawing.Point(12, 3)
        Me.GRPfIELDS.Name = "GRPfIELDS"
        Me.GRPfIELDS.Size = New System.Drawing.Size(810, 219)
        Me.GRPfIELDS.TabIndex = 0
        Me.GRPfIELDS.TabStop = False
        '
        'txtStkReordCaption
        '
        Me.txtStkReordCaption.Location = New System.Drawing.Point(112, 162)
        Me.txtStkReordCaption.MaxLength = 10
        Me.txtStkReordCaption.Name = "txtStkReordCaption"
        Me.txtStkReordCaption.Size = New System.Drawing.Size(110, 21)
        Me.txtStkReordCaption.TabIndex = 10
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(11, 166)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(51, 13)
        Me.Label12.TabIndex = 9
        Me.Label12.Text = "Caption"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpDesignOrder
        '
        Me.grpDesignOrder.Controls.Add(Me.txtLeadTime)
        Me.grpDesignOrder.Controls.Add(Me.Label10)
        Me.grpDesignOrder.Controls.Add(Me.cmbDesignerName)
        Me.grpDesignOrder.Controls.Add(Me.Label9)
        Me.grpDesignOrder.Controls.Add(Me.txtReOrder)
        Me.grpDesignOrder.Controls.Add(Me.lblReOrderPce)
        Me.grpDesignOrder.Location = New System.Drawing.Point(408, 116)
        Me.grpDesignOrder.Name = "grpDesignOrder"
        Me.grpDesignOrder.Size = New System.Drawing.Size(396, 66)
        Me.grpDesignOrder.TabIndex = 11
        Me.grpDesignOrder.TabStop = False
        Me.grpDesignOrder.Visible = False
        '
        'txtLeadTime
        '
        Me.txtLeadTime.Location = New System.Drawing.Point(280, 36)
        Me.txtLeadTime.MaxLength = 10
        Me.txtLeadTime.Name = "txtLeadTime"
        Me.txtLeadTime.Size = New System.Drawing.Size(79, 21)
        Me.txtLeadTime.TabIndex = 5
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(212, 39)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(66, 13)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "Lead Time"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbDesignerName
        '
        Me.cmbDesignerName.FormattingEnabled = True
        Me.cmbDesignerName.Location = New System.Drawing.Point(107, 9)
        Me.cmbDesignerName.Name = "cmbDesignerName"
        Me.cmbDesignerName.Size = New System.Drawing.Size(252, 21)
        Me.cmbDesignerName.TabIndex = 1
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 17)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(95, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Designer Name"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtReOrder
        '
        Me.txtReOrder.Location = New System.Drawing.Point(107, 36)
        Me.txtReOrder.MaxLength = 10
        Me.txtReOrder.Name = "txtReOrder"
        Me.txtReOrder.Size = New System.Drawing.Size(99, 21)
        Me.txtReOrder.TabIndex = 3
        '
        'lblReOrderPce
        '
        Me.lblReOrderPce.AutoSize = True
        Me.lblReOrderPce.Location = New System.Drawing.Point(6, 39)
        Me.lblReOrderPce.Name = "lblReOrderPce"
        Me.lblReOrderPce.Size = New System.Drawing.Size(99, 13)
        Me.lblReOrderPce.TabIndex = 2
        Me.lblReOrderPce.Text = "Re Order Pieces"
        Me.lblReOrderPce.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(544, 184)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 16
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'pnlControls
        '
        Me.pnlControls.Controls.Add(Me.Label11)
        Me.pnlControls.Controls.Add(Me.cmbItemCounter)
        Me.pnlControls.Controls.Add(Me.cmbRangeMode)
        Me.pnlControls.Controls.Add(Me.Label7)
        Me.pnlControls.Controls.Add(Me.cmbSize)
        Me.pnlControls.Controls.Add(Me.lblSize)
        Me.pnlControls.Controls.Add(Me.Label1)
        Me.pnlControls.Controls.Add(Me.cmbSubItem_Man)
        Me.pnlControls.Controls.Add(Me.Label2)
        Me.pnlControls.Controls.Add(Me.cmbCostCentre_own)
        Me.pnlControls.Controls.Add(Me.Label4)
        Me.pnlControls.Controls.Add(Me.cmbItem_Man)
        Me.pnlControls.Controls.Add(Me.cmbMetal_Man)
        Me.pnlControls.Controls.Add(Me.Label3)
        Me.pnlControls.Location = New System.Drawing.Point(5, 11)
        Me.pnlControls.Name = "pnlControls"
        Me.pnlControls.Size = New System.Drawing.Size(799, 105)
        Me.pnlControls.TabIndex = 0
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 85)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(80, 13)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "ItemCounter"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItemCounter
        '
        Me.cmbItemCounter.FormattingEnabled = True
        Me.cmbItemCounter.Location = New System.Drawing.Point(107, 79)
        Me.cmbItemCounter.Name = "cmbItemCounter"
        Me.cmbItemCounter.Size = New System.Drawing.Size(252, 21)
        Me.cmbItemCounter.TabIndex = 13
        '
        'cmbRangeMode
        '
        Me.cmbRangeMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRangeMode.FormattingEnabled = True
        Me.cmbRangeMode.Items.AddRange(New Object() {"Weight Range", "Rate Range"})
        Me.cmbRangeMode.Location = New System.Drawing.Point(510, 30)
        Me.cmbRangeMode.Name = "cmbRangeMode"
        Me.cmbRangeMode.Size = New System.Drawing.Size(252, 21)
        Me.cmbRangeMode.TabIndex = 7
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(409, 33)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(77, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Range Mode"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSize
        '
        Me.cmbSize.FormattingEnabled = True
        Me.cmbSize.Location = New System.Drawing.Point(510, 55)
        Me.cmbSize.Name = "cmbSize"
        Me.cmbSize.Size = New System.Drawing.Size(252, 21)
        Me.cmbSize.TabIndex = 11
        '
        'lblSize
        '
        Me.lblSize.AutoSize = True
        Me.lblSize.Location = New System.Drawing.Point(409, 58)
        Me.lblSize.Name = "lblSize"
        Me.lblSize.Size = New System.Drawing.Size(31, 13)
        Me.lblSize.TabIndex = 10
        Me.lblSize.Text = "Size"
        Me.lblSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(409, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Metal"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSubItem_Man
        '
        Me.cmbSubItem_Man.FormattingEnabled = True
        Me.cmbSubItem_Man.Location = New System.Drawing.Point(107, 55)
        Me.cmbSubItem_Man.Name = "cmbSubItem_Man"
        Me.cmbSubItem_Man.Size = New System.Drawing.Size(252, 21)
        Me.cmbSubItem_Man.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Item "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCostCentre_own
        '
        Me.cmbCostCentre_own.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCostCentre_own.FormattingEnabled = True
        Me.cmbCostCentre_own.Location = New System.Drawing.Point(107, 5)
        Me.cmbCostCentre_own.Name = "cmbCostCentre_own"
        Me.cmbCostCentre_own.Size = New System.Drawing.Size(252, 21)
        Me.cmbCostCentre_own.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Cost Centre"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItem_Man
        '
        Me.cmbItem_Man.FormattingEnabled = True
        Me.cmbItem_Man.Location = New System.Drawing.Point(107, 30)
        Me.cmbItem_Man.Name = "cmbItem_Man"
        Me.cmbItem_Man.Size = New System.Drawing.Size(252, 21)
        Me.cmbItem_Man.TabIndex = 5
        '
        'cmbMetal_Man
        '
        Me.cmbMetal_Man.FormattingEnabled = True
        Me.cmbMetal_Man.Location = New System.Drawing.Point(510, 4)
        Me.cmbMetal_Man.Name = "cmbMetal_Man"
        Me.cmbMetal_Man.Size = New System.Drawing.Size(252, 21)
        Me.cmbMetal_Man.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Sub Item"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(436, 184)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 15
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(328, 184)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 14
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'lblWtFrom
        '
        Me.lblWtFrom.AutoSize = True
        Me.lblWtFrom.Location = New System.Drawing.Point(11, 122)
        Me.lblWtFrom.Name = "lblWtFrom"
        Me.lblWtFrom.Size = New System.Drawing.Size(78, 13)
        Me.lblWtFrom.TabIndex = 1
        Me.lblWtFrom.Text = "From Weight"
        Me.lblWtFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(220, 184)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 13
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(112, 184)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 12
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'lblWtTo
        '
        Me.lblWtTo.AutoSize = True
        Me.lblWtTo.Location = New System.Drawing.Point(226, 122)
        Me.lblWtTo.Name = "lblWtTo"
        Me.lblWtTo.Size = New System.Drawing.Size(20, 13)
        Me.lblWtTo.TabIndex = 3
        Me.lblWtTo.Text = "To"
        Me.lblWtTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtWeight
        '
        Me.txtWeight.Location = New System.Drawing.Point(274, 140)
        Me.txtWeight.MaxLength = 10
        Me.txtWeight.Name = "txtWeight"
        Me.txtWeight.Size = New System.Drawing.Size(110, 21)
        Me.txtWeight.TabIndex = 8
        '
        'txtPiece
        '
        Me.txtPiece.Location = New System.Drawing.Point(112, 140)
        Me.txtPiece.MaxLength = 10
        Me.txtPiece.Name = "txtPiece"
        Me.txtPiece.Size = New System.Drawing.Size(110, 21)
        Me.txtPiece.TabIndex = 6
        '
        'minLbl
        '
        Me.minLbl.AutoSize = True
        Me.minLbl.Location = New System.Drawing.Point(11, 144)
        Me.minLbl.Name = "minLbl"
        Me.minLbl.Size = New System.Drawing.Size(37, 13)
        Me.minLbl.TabIndex = 5
        Me.minLbl.Text = "Piece"
        Me.minLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtWtTo
        '
        Me.txtWtTo.Location = New System.Drawing.Point(274, 118)
        Me.txtWtTo.MaxLength = 10
        Me.txtWtTo.Name = "txtWtTo"
        Me.txtWtTo.Size = New System.Drawing.Size(111, 21)
        Me.txtWtTo.TabIndex = 4
        '
        'lblWt
        '
        Me.lblWt.AutoSize = True
        Me.lblWt.Location = New System.Drawing.Point(225, 144)
        Me.lblWt.Name = "lblWt"
        Me.lblWt.Size = New System.Drawing.Size(45, 13)
        Me.lblWt.TabIndex = 7
        Me.lblWt.Text = "Weight"
        Me.lblWt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtWtFrom_Own
        '
        Me.txtWtFrom_Own.Location = New System.Drawing.Point(112, 118)
        Me.txtWtFrom_Own.MaxLength = 10
        Me.txtWtFrom_Own.Name = "txtWtFrom_Own"
        Me.txtWtFrom_Own.Size = New System.Drawing.Size(110, 21)
        Me.txtWtFrom_Own.TabIndex = 2
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NEwToolStripMenuItem, Me.ExitToolStripMenuItem})
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
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'NEwToolStripMenuItem
        '
        Me.NEwToolStripMenuItem.Name = "NEwToolStripMenuItem"
        Me.NEwToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NEwToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.NEwToolStripMenuItem.Text = "New"
        Me.NEwToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(12, 560)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 3
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbSubItemSearch_OWN)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.cmbItemSearch_OWN)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.cmbCostSearch_OWN)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 222)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(810, 69)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Search"
        '
        'cmbSubItemSearch_OWN
        '
        Me.cmbSubItemSearch_OWN.FormattingEnabled = True
        Me.cmbSubItemSearch_OWN.Items.AddRange(New Object() {"ALL"})
        Me.cmbSubItemSearch_OWN.Location = New System.Drawing.Point(390, 42)
        Me.cmbSubItemSearch_OWN.Name = "cmbSubItemSearch_OWN"
        Me.cmbSubItemSearch_OWN.Size = New System.Drawing.Size(272, 21)
        Me.cmbSubItemSearch_OWN.TabIndex = 5
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(475, 21)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "SubItem"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItemSearch_OWN
        '
        Me.cmbItemSearch_OWN.FormattingEnabled = True
        Me.cmbItemSearch_OWN.Location = New System.Drawing.Point(178, 42)
        Me.cmbItemSearch_OWN.Name = "cmbItemSearch_OWN"
        Me.cmbItemSearch_OWN.Size = New System.Drawing.Size(207, 21)
        Me.cmbItemSearch_OWN.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(240, 21)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(34, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Item"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCostSearch_OWN
        '
        Me.cmbCostSearch_OWN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCostSearch_OWN.FormattingEnabled = True
        Me.cmbCostSearch_OWN.Location = New System.Drawing.Point(14, 42)
        Me.cmbCostSearch_OWN.Name = "cmbCostSearch_OWN"
        Me.cmbCostSearch_OWN.Size = New System.Drawing.Size(159, 21)
        Me.cmbCostSearch_OWN.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(40, 21)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(76, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Cost Centre"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(667, 36)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 6
        Me.btnSearch.Text = "  Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'frmStockReorder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(833, 581)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GRPfIELDS)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.gridView)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmStockReorder"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Stock Reorder"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GRPfIELDS.ResumeLayout(False)
        Me.GRPfIELDS.PerformLayout()
        Me.grpDesignOrder.ResumeLayout(False)
        Me.grpDesignOrder.PerformLayout()
        Me.pnlControls.ResumeLayout(False)
        Me.pnlControls.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtWtFrom_Own As System.Windows.Forms.TextBox
    Friend WithEvents cmbCostCentre_own As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubItem_Man As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItem_Man As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMetal_Man As System.Windows.Forms.ComboBox
    Friend WithEvents lblWt As System.Windows.Forms.Label
    Friend WithEvents minLbl As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblWtTo As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblWtFrom As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GRPfIELDS As System.Windows.Forms.GroupBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents txtWeight As System.Windows.Forms.TextBox
    Friend WithEvents txtPiece As System.Windows.Forms.TextBox
    Friend WithEvents txtWtTo As System.Windows.Forms.TextBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NEwToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlControls As System.Windows.Forms.Panel
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents cmbSize As System.Windows.Forms.ComboBox
    Friend WithEvents lblSize As System.Windows.Forms.Label
    Friend WithEvents grpDesignOrder As System.Windows.Forms.GroupBox
    Friend WithEvents txtReOrder As System.Windows.Forms.TextBox
    Friend WithEvents lblReOrderPce As System.Windows.Forms.Label
    Friend WithEvents cmbDesignerName As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbItemSearch_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbCostSearch_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubItemSearch_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbRangeMode As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtLeadTime As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents cmbItemCounter As System.Windows.Forms.ComboBox
    Friend WithEvents txtStkReordCaption As TextBox
    Friend WithEvents Label12 As Label
End Class
