<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWeightPieceWiseStockReOrder
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
        Me.GRPfIELDS = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtRangeCaption = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtMaxWeight = New System.Windows.Forms.TextBox()
        Me.txtMinWeight = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtMaxPcs = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblWtFrom = New System.Windows.Forms.Label()
        Me.txtMinPcs = New System.Windows.Forms.TextBox()
        Me.lblReOrderPce = New System.Windows.Forms.Label()
        Me.txtWtFrom_Own = New System.Windows.Forms.TextBox()
        Me.lblWtTo = New System.Windows.Forms.Label()
        Me.txtWtTo = New System.Windows.Forms.TextBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.pnlControls = New System.Windows.Forms.Panel()
        Me.cmbDesignerName = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbSize = New System.Windows.Forms.ComboBox()
        Me.lblSize = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbSubItem_Man = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbItem_Man = New System.Windows.Forms.ComboBox()
        Me.cmbMetal_Man = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmbSubItemSearch_OWN = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbItemSearch_OWN = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbCostSearch_OWN = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NEwToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GRPfIELDS.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlControls.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GRPfIELDS
        '
        Me.GRPfIELDS.BackColor = System.Drawing.Color.Transparent
        Me.GRPfIELDS.Controls.Add(Me.Panel1)
        Me.GRPfIELDS.Controls.Add(Me.lblStatus)
        Me.GRPfIELDS.Controls.Add(Me.btnDelete)
        Me.GRPfIELDS.Controls.Add(Me.pnlControls)
        Me.GRPfIELDS.Controls.Add(Me.btnExit)
        Me.GRPfIELDS.Controls.Add(Me.btnNew)
        Me.GRPfIELDS.Controls.Add(Me.btnOpen)
        Me.GRPfIELDS.Controls.Add(Me.btnSave)
        Me.GRPfIELDS.Dock = System.Windows.Forms.DockStyle.Top
        Me.GRPfIELDS.Location = New System.Drawing.Point(0, 0)
        Me.GRPfIELDS.Name = "GRPfIELDS"
        Me.GRPfIELDS.Size = New System.Drawing.Size(969, 233)
        Me.GRPfIELDS.TabIndex = 1
        Me.GRPfIELDS.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.txtRangeCaption)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.txtMaxWeight)
        Me.Panel1.Controls.Add(Me.txtMinWeight)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.txtMaxPcs)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.lblWtFrom)
        Me.Panel1.Controls.Add(Me.txtMinPcs)
        Me.Panel1.Controls.Add(Me.lblReOrderPce)
        Me.Panel1.Controls.Add(Me.txtWtFrom_Own)
        Me.Panel1.Controls.Add(Me.lblWtTo)
        Me.Panel1.Controls.Add(Me.txtWtTo)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 98)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(963, 87)
        Me.Panel1.TabIndex = 1
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(7, 62)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(91, 13)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "Range Caption"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRangeCaption
        '
        Me.txtRangeCaption.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtRangeCaption.Location = New System.Drawing.Point(125, 62)
        Me.txtRangeCaption.MaxLength = 10
        Me.txtRangeCaption.Name = "txtRangeCaption"
        Me.txtRangeCaption.Size = New System.Drawing.Size(121, 21)
        Me.txtRangeCaption.TabIndex = 5
        Me.txtRangeCaption.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(710, 38)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(72, 13)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "Max Weight"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMaxWeight
        '
        Me.txtMaxWeight.Location = New System.Drawing.Point(781, 38)
        Me.txtMaxWeight.MaxLength = 10
        Me.txtMaxWeight.Name = "txtMaxWeight"
        Me.txtMaxWeight.Size = New System.Drawing.Size(110, 21)
        Me.txtMaxWeight.TabIndex = 13
        Me.txtMaxWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMinWeight
        '
        Me.txtMinWeight.Location = New System.Drawing.Point(781, 12)
        Me.txtMinWeight.MaxLength = 10
        Me.txtMinWeight.Name = "txtMinWeight"
        Me.txtMinWeight.Size = New System.Drawing.Size(110, 21)
        Me.txtMinWeight.TabIndex = 11
        Me.txtMinWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(710, 16)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(68, 13)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "Min Weight"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMaxPcs
        '
        Me.txtMaxPcs.Location = New System.Drawing.Point(595, 38)
        Me.txtMaxPcs.MaxLength = 10
        Me.txtMaxPcs.Name = "txtMaxPcs"
        Me.txtMaxPcs.Size = New System.Drawing.Size(111, 21)
        Me.txtMaxPcs.TabIndex = 9
        Me.txtMaxPcs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(477, 38)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(53, 13)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Max Pcs"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblWtFrom
        '
        Me.lblWtFrom.AutoSize = True
        Me.lblWtFrom.Location = New System.Drawing.Point(7, 16)
        Me.lblWtFrom.Name = "lblWtFrom"
        Me.lblWtFrom.Size = New System.Drawing.Size(78, 13)
        Me.lblWtFrom.TabIndex = 0
        Me.lblWtFrom.Text = "From Weight"
        Me.lblWtFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMinPcs
        '
        Me.txtMinPcs.Location = New System.Drawing.Point(595, 14)
        Me.txtMinPcs.MaxLength = 10
        Me.txtMinPcs.Name = "txtMinPcs"
        Me.txtMinPcs.Size = New System.Drawing.Size(110, 21)
        Me.txtMinPcs.TabIndex = 7
        Me.txtMinPcs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblReOrderPce
        '
        Me.lblReOrderPce.AutoSize = True
        Me.lblReOrderPce.Location = New System.Drawing.Point(477, 15)
        Me.lblReOrderPce.Name = "lblReOrderPce"
        Me.lblReOrderPce.Size = New System.Drawing.Size(49, 13)
        Me.lblReOrderPce.TabIndex = 6
        Me.lblReOrderPce.Text = "Min Pcs"
        Me.lblReOrderPce.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtWtFrom_Own
        '
        Me.txtWtFrom_Own.Location = New System.Drawing.Point(125, 12)
        Me.txtWtFrom_Own.MaxLength = 10
        Me.txtWtFrom_Own.Name = "txtWtFrom_Own"
        Me.txtWtFrom_Own.Size = New System.Drawing.Size(120, 21)
        Me.txtWtFrom_Own.TabIndex = 1
        Me.txtWtFrom_Own.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblWtTo
        '
        Me.lblWtTo.AutoSize = True
        Me.lblWtTo.Location = New System.Drawing.Point(7, 38)
        Me.lblWtTo.Name = "lblWtTo"
        Me.lblWtTo.Size = New System.Drawing.Size(62, 13)
        Me.lblWtTo.TabIndex = 2
        Me.lblWtTo.Text = "To Weight"
        Me.lblWtTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtWtTo
        '
        Me.txtWtTo.Location = New System.Drawing.Point(125, 38)
        Me.txtWtTo.MaxLength = 10
        Me.txtWtTo.Name = "txtWtTo"
        Me.txtWtTo.Size = New System.Drawing.Size(121, 21)
        Me.txtWtTo.TabIndex = 3
        Me.txtWtTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(854, 197)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 7
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(655, 193)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(117, 30)
        Me.btnDelete.TabIndex = 6
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'pnlControls
        '
        Me.pnlControls.Controls.Add(Me.cmbDesignerName)
        Me.pnlControls.Controls.Add(Me.Label9)
        Me.pnlControls.Controls.Add(Me.cmbSize)
        Me.pnlControls.Controls.Add(Me.lblSize)
        Me.pnlControls.Controls.Add(Me.Label1)
        Me.pnlControls.Controls.Add(Me.cmbSubItem_Man)
        Me.pnlControls.Controls.Add(Me.Label2)
        Me.pnlControls.Controls.Add(Me.cmbCostCentre)
        Me.pnlControls.Controls.Add(Me.Label4)
        Me.pnlControls.Controls.Add(Me.cmbItem_Man)
        Me.pnlControls.Controls.Add(Me.cmbMetal_Man)
        Me.pnlControls.Controls.Add(Me.Label3)
        Me.pnlControls.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlControls.Location = New System.Drawing.Point(3, 17)
        Me.pnlControls.Name = "pnlControls"
        Me.pnlControls.Size = New System.Drawing.Size(963, 81)
        Me.pnlControls.TabIndex = 0
        '
        'cmbDesignerName
        '
        Me.cmbDesignerName.FormattingEnabled = True
        Me.cmbDesignerName.Location = New System.Drawing.Point(595, 55)
        Me.cmbDesignerName.Name = "cmbDesignerName"
        Me.cmbDesignerName.Size = New System.Drawing.Size(293, 21)
        Me.cmbDesignerName.TabIndex = 11
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(477, 58)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(95, 13)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "Designer Name"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSize
        '
        Me.cmbSize.FormattingEnabled = True
        Me.cmbSize.Location = New System.Drawing.Point(595, 30)
        Me.cmbSize.Name = "cmbSize"
        Me.cmbSize.Size = New System.Drawing.Size(293, 21)
        Me.cmbSize.TabIndex = 9
        '
        'lblSize
        '
        Me.lblSize.AutoSize = True
        Me.lblSize.Location = New System.Drawing.Point(477, 33)
        Me.lblSize.Name = "lblSize"
        Me.lblSize.Size = New System.Drawing.Size(31, 13)
        Me.lblSize.TabIndex = 8
        Me.lblSize.Text = "Size"
        Me.lblSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(477, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Metal"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSubItem_Man
        '
        Me.cmbSubItem_Man.FormattingEnabled = True
        Me.cmbSubItem_Man.Location = New System.Drawing.Point(125, 55)
        Me.cmbSubItem_Man.Name = "cmbSubItem_Man"
        Me.cmbSubItem_Man.Size = New System.Drawing.Size(315, 21)
        Me.cmbSubItem_Man.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Item "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(125, 5)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(315, 21)
        Me.cmbCostCentre.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Cost Centre"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItem_Man
        '
        Me.cmbItem_Man.FormattingEnabled = True
        Me.cmbItem_Man.Location = New System.Drawing.Point(125, 30)
        Me.cmbItem_Man.Name = "cmbItem_Man"
        Me.cmbItem_Man.Size = New System.Drawing.Size(315, 21)
        Me.cmbItem_Man.TabIndex = 5
        '
        'cmbMetal_Man
        '
        Me.cmbMetal_Man.FormattingEnabled = True
        Me.cmbMetal_Man.Location = New System.Drawing.Point(595, 4)
        Me.cmbMetal_Man.Name = "cmbMetal_Man"
        Me.cmbMetal_Man.Size = New System.Drawing.Size(293, 21)
        Me.cmbMetal_Man.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Sub Item"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(532, 193)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(117, 30)
        Me.btnExit.TabIndex = 5
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(409, 193)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(117, 30)
        Me.btnNew.TabIndex = 4
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(286, 193)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(117, 30)
        Me.btnOpen.TabIndex = 3
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(163, 193)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(117, 30)
        Me.btnSave.TabIndex = 2
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
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
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 233)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(969, 69)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Search"
        '
        'cmbSubItemSearch_OWN
        '
        Me.cmbSubItemSearch_OWN.FormattingEnabled = True
        Me.cmbSubItemSearch_OWN.Items.AddRange(New Object() {"ALL"})
        Me.cmbSubItemSearch_OWN.Location = New System.Drawing.Point(455, 42)
        Me.cmbSubItemSearch_OWN.Name = "cmbSubItemSearch_OWN"
        Me.cmbSubItemSearch_OWN.Size = New System.Drawing.Size(317, 21)
        Me.cmbSubItemSearch_OWN.TabIndex = 5
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(554, 21)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "SubItem"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItemSearch_OWN
        '
        Me.cmbItemSearch_OWN.FormattingEnabled = True
        Me.cmbItemSearch_OWN.Location = New System.Drawing.Point(208, 42)
        Me.cmbItemSearch_OWN.Name = "cmbItemSearch_OWN"
        Me.cmbItemSearch_OWN.Size = New System.Drawing.Size(241, 21)
        Me.cmbItemSearch_OWN.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(280, 21)
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
        Me.cmbCostSearch_OWN.Location = New System.Drawing.Point(16, 42)
        Me.cmbCostSearch_OWN.Name = "cmbCostSearch_OWN"
        Me.cmbCostSearch_OWN.Size = New System.Drawing.Size(185, 21)
        Me.cmbCostSearch_OWN.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(47, 21)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(76, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Cost Centre"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(778, 36)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(117, 30)
        Me.btnSearch.TabIndex = 6
        Me.btnSearch.Text = "  Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 302)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(969, 255)
        Me.gridView.TabIndex = 0
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
        'frmWeightPieceWiseStockReOrder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(969, 557)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GRPfIELDS)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmWeightPieceWiseStockReOrder"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Weight And Piece Wise Stock ReOrder"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GRPfIELDS.ResumeLayout(False)
        Me.GRPfIELDS.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlControls.ResumeLayout(False)
        Me.pnlControls.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GRPfIELDS As System.Windows.Forms.GroupBox
    Friend WithEvents cmbDesignerName As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtMinPcs As System.Windows.Forms.TextBox
    Friend WithEvents lblReOrderPce As System.Windows.Forms.Label
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents pnlControls As System.Windows.Forms.Panel
    Friend WithEvents cmbSize As System.Windows.Forms.ComboBox
    Friend WithEvents lblSize As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbSubItem_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbItem_Man As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMetal_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbSubItemSearch_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbItemSearch_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbCostSearch_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NEwToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents lblWtFrom As System.Windows.Forms.Label
    Friend WithEvents lblWtTo As System.Windows.Forms.Label
    Friend WithEvents txtWtTo As System.Windows.Forms.TextBox
    Friend WithEvents txtWtFrom_Own As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtMaxPcs As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtMaxWeight As System.Windows.Forms.TextBox
    Friend WithEvents txtMinWeight As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label12 As Label
    Friend WithEvents txtRangeCaption As TextBox
End Class
