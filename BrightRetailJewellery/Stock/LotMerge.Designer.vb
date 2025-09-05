<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LotMerge
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.grpTagDetail = New CodeVendor.Controls.Grouper
        Me.lblHelp2 = New System.Windows.Forms.Label
        Me.lblHelp1 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtSubItemName = New System.Windows.Forms.TextBox
        Me.txtItemname = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtStonePcs = New System.Windows.Forms.TextBox
        Me.txtPcs = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtDesigner = New System.Windows.Forms.TextBox
        Me.txtTAGRowIndex = New System.Windows.Forms.TextBox
        Me.gridTAGTotal = New System.Windows.Forms.DataGridView
        Me.gridTAG = New System.Windows.Forms.DataGridView
        Me.txtItemId = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtSubItemId = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtTagGrsWt_WET = New System.Windows.Forms.TextBox
        Me.txtStoneWT = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtLotNo = New System.Windows.Forms.TextBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtDiaPcs = New System.Windows.Forms.TextBox
        Me.txtDiaWT = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.grpTagDetail.SuspendLayout()
        CType(Me.gridTAGTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridTAG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpTagDetail
        '
        Me.grpTagDetail.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpTagDetail.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpTagDetail.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpTagDetail.BorderColor = System.Drawing.Color.Transparent
        Me.grpTagDetail.BorderThickness = 1.0!
        Me.grpTagDetail.Controls.Add(Me.Label9)
        Me.grpTagDetail.Controls.Add(Me.txtDiaPcs)
        Me.grpTagDetail.Controls.Add(Me.txtDiaWT)
        Me.grpTagDetail.Controls.Add(Me.Label10)
        Me.grpTagDetail.Controls.Add(Me.lblHelp2)
        Me.grpTagDetail.Controls.Add(Me.lblHelp1)
        Me.grpTagDetail.Controls.Add(Me.btnExit)
        Me.grpTagDetail.Controls.Add(Me.btnSave)
        Me.grpTagDetail.Controls.Add(Me.btnNew)
        Me.grpTagDetail.Controls.Add(Me.Label8)
        Me.grpTagDetail.Controls.Add(Me.Label7)
        Me.grpTagDetail.Controls.Add(Me.txtSubItemName)
        Me.grpTagDetail.Controls.Add(Me.txtItemname)
        Me.grpTagDetail.Controls.Add(Me.Label1)
        Me.grpTagDetail.Controls.Add(Me.txtStonePcs)
        Me.grpTagDetail.Controls.Add(Me.txtPcs)
        Me.grpTagDetail.Controls.Add(Me.Label18)
        Me.grpTagDetail.Controls.Add(Me.Label15)
        Me.grpTagDetail.Controls.Add(Me.txtDesigner)
        Me.grpTagDetail.Controls.Add(Me.txtTAGRowIndex)
        Me.grpTagDetail.Controls.Add(Me.gridTAGTotal)
        Me.grpTagDetail.Controls.Add(Me.gridTAG)
        Me.grpTagDetail.Controls.Add(Me.txtItemId)
        Me.grpTagDetail.Controls.Add(Me.Label3)
        Me.grpTagDetail.Controls.Add(Me.Label6)
        Me.grpTagDetail.Controls.Add(Me.txtSubItemId)
        Me.grpTagDetail.Controls.Add(Me.Label2)
        Me.grpTagDetail.Controls.Add(Me.Label4)
        Me.grpTagDetail.Controls.Add(Me.txtTagGrsWt_WET)
        Me.grpTagDetail.Controls.Add(Me.txtStoneWT)
        Me.grpTagDetail.Controls.Add(Me.Label5)
        Me.grpTagDetail.Controls.Add(Me.txtLotNo)
        Me.grpTagDetail.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpTagDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpTagDetail.GroupImage = Nothing
        Me.grpTagDetail.GroupTitle = ""
        Me.grpTagDetail.Location = New System.Drawing.Point(0, 0)
        Me.grpTagDetail.Name = "grpTagDetail"
        Me.grpTagDetail.Padding = New System.Windows.Forms.Padding(20)
        Me.grpTagDetail.PaintGroupBox = False
        Me.grpTagDetail.RoundCorners = 10
        Me.grpTagDetail.ShadowColor = System.Drawing.SystemColors.InactiveCaption
        Me.grpTagDetail.ShadowControl = False
        Me.grpTagDetail.ShadowThickness = 3
        Me.grpTagDetail.Size = New System.Drawing.Size(964, 622)
        Me.grpTagDetail.TabIndex = 0
        '
        'lblHelp2
        '
        Me.lblHelp2.AutoSize = True
        Me.lblHelp2.BackColor = System.Drawing.Color.Transparent
        Me.lblHelp2.ForeColor = System.Drawing.Color.Red
        Me.lblHelp2.Location = New System.Drawing.Point(6, 593)
        Me.lblHelp2.Name = "lblHelp2"
        Me.lblHelp2.Size = New System.Drawing.Size(143, 13)
        Me.lblHelp2.TabIndex = 15
        Me.lblHelp2.Text = "Press Del Key to Delete"
        Me.lblHelp2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblHelp1
        '
        Me.lblHelp1.AutoSize = True
        Me.lblHelp1.BackColor = System.Drawing.Color.Transparent
        Me.lblHelp1.ForeColor = System.Drawing.Color.Red
        Me.lblHelp1.Location = New System.Drawing.Point(6, 577)
        Me.lblHelp1.Name = "lblHelp1"
        Me.lblHelp1.Size = New System.Drawing.Size(138, 13)
        Me.lblHelp1.TabIndex = 14
        Me.lblHelp1.Text = "Press Enter Key to Edit"
        Me.lblHelp1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(536, 577)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 18
        Me.btnExit.Text = "E&xit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(329, 577)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 16
        Me.btnSave.Text = "&Save[F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.Location = New System.Drawing.Point(433, 577)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 17
        Me.btnNew.Text = "&New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(135, 14)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(75, 13)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "ItemName"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(296, 15)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(62, 13)
        Me.Label7.TabIndex = 36
        Me.Label7.Text = "SubItem" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSubItemName
        '
        Me.txtSubItemName.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubItemName.Location = New System.Drawing.Point(281, 31)
        Me.txtSubItemName.MaxLength = 10
        Me.txtSubItemName.Name = "txtSubItemName"
        Me.txtSubItemName.ShortcutsEnabled = False
        Me.txtSubItemName.Size = New System.Drawing.Size(93, 22)
        Me.txtSubItemName.TabIndex = 5
        '
        'txtItemname
        '
        Me.txtItemname.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemname.Location = New System.Drawing.Point(126, 31)
        Me.txtItemname.MaxLength = 10
        Me.txtItemname.Name = "txtItemname"
        Me.txtItemname.ShortcutsEnabled = False
        Me.txtItemname.Size = New System.Drawing.Size(93, 22)
        Me.txtItemname.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(635, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 13)
        Me.Label1.TabIndex = 32
        Me.Label1.Text = "Stn Pcs"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtStonePcs
        '
        Me.txtStonePcs.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStonePcs.Location = New System.Drawing.Point(618, 31)
        Me.txtStonePcs.MaxLength = 10
        Me.txtStonePcs.Name = "txtStonePcs"
        Me.txtStonePcs.ShortcutsEnabled = False
        Me.txtStonePcs.Size = New System.Drawing.Size(85, 22)
        Me.txtStonePcs.TabIndex = 9
        '
        'txtPcs
        '
        Me.txtPcs.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPcs.Location = New System.Drawing.Point(446, 31)
        Me.txtPcs.MaxLength = 10
        Me.txtPcs.Name = "txtPcs"
        Me.txtPcs.ShortcutsEnabled = False
        Me.txtPcs.Size = New System.Drawing.Size(85, 22)
        Me.txtPcs.TabIndex = 7
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.BackColor = System.Drawing.Color.Transparent
        Me.Label18.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(551, 14)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(46, 13)
        Me.Label18.TabIndex = 30
        Me.Label18.Text = "GrsWt"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.BackColor = System.Drawing.Color.Transparent
        Me.Label15.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(378, 14)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(65, 13)
        Me.Label15.TabIndex = 26
        Me.Label15.Text = "Designer"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtDesigner
        '
        Me.txtDesigner.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDesigner.Location = New System.Drawing.Point(375, 31)
        Me.txtDesigner.MaxLength = 10
        Me.txtDesigner.Name = "txtDesigner"
        Me.txtDesigner.ShortcutsEnabled = False
        Me.txtDesigner.Size = New System.Drawing.Size(70, 22)
        Me.txtDesigner.TabIndex = 6
        '
        'txtTAGRowIndex
        '
        Me.txtTAGRowIndex.Location = New System.Drawing.Point(987, 31)
        Me.txtTAGRowIndex.Name = "txtTAGRowIndex"
        Me.txtTAGRowIndex.Size = New System.Drawing.Size(10, 21)
        Me.txtTAGRowIndex.TabIndex = 22
        Me.txtTAGRowIndex.Visible = False
        '
        'gridTAGTotal
        '
        Me.gridTAGTotal.AllowUserToAddRows = False
        Me.gridTAGTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridTAGTotal.Enabled = False
        Me.gridTAGTotal.Location = New System.Drawing.Point(3, 531)
        Me.gridTAGTotal.Name = "gridTAGTotal"
        Me.gridTAGTotal.ReadOnly = True
        Me.gridTAGTotal.Size = New System.Drawing.Size(959, 37)
        Me.gridTAGTotal.TabIndex = 19
        '
        'gridTAG
        '
        Me.gridTAG.AllowUserToAddRows = False
        Me.gridTAG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridTAG.Location = New System.Drawing.Point(3, 55)
        Me.gridTAG.Name = "gridTAG"
        Me.gridTAG.ReadOnly = True
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.AliceBlue
        Me.gridTAG.RowsDefaultCellStyle = DataGridViewCellStyle1
        Me.gridTAG.Size = New System.Drawing.Size(959, 475)
        Me.gridTAG.TabIndex = 13
        '
        'txtItemId
        '
        Me.txtItemId.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemId.Location = New System.Drawing.Point(65, 31)
        Me.txtItemId.MaxLength = 10
        Me.txtItemId.Name = "txtItemId"
        Me.txtItemId.ShortcutsEnabled = False
        Me.txtItemId.Size = New System.Drawing.Size(60, 22)
        Me.txtItemId.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(69, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(52, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "ItemId"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(474, 14)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(29, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Pcs"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSubItemId
        '
        Me.txtSubItemId.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubItemId.Location = New System.Drawing.Point(220, 31)
        Me.txtSubItemId.MaxLength = 5
        Me.txtSubItemId.Name = "txtSubItemId"
        Me.txtSubItemId.ShortcutsEnabled = False
        Me.txtSubItemId.Size = New System.Drawing.Size(60, 22)
        Me.txtSubItemId.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(10, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Lot No"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(212, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "SubItemId"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTagGrsWt_WET
        '
        Me.txtTagGrsWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTagGrsWt_WET.Location = New System.Drawing.Point(532, 31)
        Me.txtTagGrsWt_WET.MaxLength = 10
        Me.txtTagGrsWt_WET.Name = "txtTagGrsWt_WET"
        Me.txtTagGrsWt_WET.ShortcutsEnabled = False
        Me.txtTagGrsWt_WET.Size = New System.Drawing.Size(85, 22)
        Me.txtTagGrsWt_WET.TabIndex = 8
        '
        'txtStoneWT
        '
        Me.txtStoneWT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStoneWT.Location = New System.Drawing.Point(704, 31)
        Me.txtStoneWT.MaxLength = 10
        Me.txtStoneWT.Name = "txtStoneWT"
        Me.txtStoneWT.ShortcutsEnabled = False
        Me.txtStoneWT.Size = New System.Drawing.Size(85, 22)
        Me.txtStoneWT.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(724, 13)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Stn Wt"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtLotNo
        '
        Me.txtLotNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLotNo.Location = New System.Drawing.Point(4, 31)
        Me.txtLotNo.MaxLength = 15
        Me.txtLotNo.Name = "txtLotNo"
        Me.txtLotNo.ShortcutsEnabled = False
        Me.txtLotNo.Size = New System.Drawing.Size(60, 22)
        Me.txtLotNo.TabIndex = 1
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 2000
        Me.ToolTip1.ReshowDelay = 100
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 70)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(807, 14)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(54, 13)
        Me.Label9.TabIndex = 41
        Me.Label9.Text = "Dia Pcs"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtDiaPcs
        '
        Me.txtDiaPcs.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiaPcs.Location = New System.Drawing.Point(790, 31)
        Me.txtDiaPcs.MaxLength = 10
        Me.txtDiaPcs.Name = "txtDiaPcs"
        Me.txtDiaPcs.ShortcutsEnabled = False
        Me.txtDiaPcs.Size = New System.Drawing.Size(85, 22)
        Me.txtDiaPcs.TabIndex = 11
        '
        'txtDiaWT
        '
        Me.txtDiaWT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiaWT.Location = New System.Drawing.Point(876, 31)
        Me.txtDiaWT.MaxLength = 10
        Me.txtDiaWT.Name = "txtDiaWT"
        Me.txtDiaWT.ShortcutsEnabled = False
        Me.txtDiaWT.Size = New System.Drawing.Size(85, 22)
        Me.txtDiaWT.TabIndex = 12
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(896, 13)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(49, 13)
        Me.Label10.TabIndex = 38
        Me.Label10.Text = "Dia Wt"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LotMerge
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(964, 622)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpTagDetail)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(1078, 745)
        Me.Name = "LotMerge"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.grpTagDetail.ResumeLayout(False)
        Me.grpTagDetail.PerformLayout()
        CType(Me.gridTAGTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridTAG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpTagDetail As CodeVendor.Controls.Grouper
    Friend WithEvents txtTAGRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents gridTAGTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridTAG As System.Windows.Forms.DataGridView
    Friend WithEvents txtItemId As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtSubItemId As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtTagGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtStoneWT As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtLotNo As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtDesigner As System.Windows.Forms.TextBox
    Friend WithEvents txtPcs As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtStonePcs As System.Windows.Forms.TextBox
    Friend WithEvents txtSubItemName As System.Windows.Forms.TextBox
    Friend WithEvents txtItemname As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblHelp2 As System.Windows.Forms.Label
    Friend WithEvents lblHelp1 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtDiaPcs As System.Windows.Forms.TextBox
    Friend WithEvents txtDiaWT As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
End Class
