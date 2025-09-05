<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReceiptTagPosting
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.grpRecTagDetail = New CodeVendor.Controls.Grouper
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtStnAmt_amt = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtstnWt_Wet = New System.Windows.Forms.TextBox
        Me.GrpDetail = New System.Windows.Forms.GroupBox
        Me.txtTablecode = New System.Windows.Forms.TextBox
        Me.txt_Purity = New System.Windows.Forms.TextBox
        Me.txt_DesignerName = New System.Windows.Forms.TextBox
        Me.txt_SubItemname = New System.Windows.Forms.TextBox
        Me.txt_Itemname = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.txtMcGrm = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtWastPer = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtSalvalue_Amt = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtMcharge = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtWastage = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtLessWt_Wet = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtNetwt_Wet = New System.Windows.Forms.TextBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.txtPcs = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.txtTAGRowIndex = New System.Windows.Forms.TextBox
        Me.gridViewTotal = New System.Windows.Forms.DataGridView
        Me.gridview = New System.Windows.Forms.DataGridView
        Me.txtItemId = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtGrsWt_WET = New System.Windows.Forms.TextBox
        Me.txtTranno = New System.Windows.Forms.TextBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StoneDetailsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpRecTagDetail.SuspendLayout()
        Me.GrpDetail.SuspendLayout()
        CType(Me.gridViewTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpRecTagDetail
        '
        Me.grpRecTagDetail.AutoSize = True
        Me.grpRecTagDetail.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpRecTagDetail.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpRecTagDetail.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpRecTagDetail.BorderColor = System.Drawing.Color.Transparent
        Me.grpRecTagDetail.BorderThickness = 1.0!
        Me.grpRecTagDetail.Controls.Add(Me.Label7)
        Me.grpRecTagDetail.Controls.Add(Me.txtStnAmt_amt)
        Me.grpRecTagDetail.Controls.Add(Me.Label4)
        Me.grpRecTagDetail.Controls.Add(Me.txtstnWt_Wet)
        Me.grpRecTagDetail.Controls.Add(Me.GrpDetail)
        Me.grpRecTagDetail.Controls.Add(Me.Label13)
        Me.grpRecTagDetail.Controls.Add(Me.txtMcGrm)
        Me.grpRecTagDetail.Controls.Add(Me.Label12)
        Me.grpRecTagDetail.Controls.Add(Me.txtWastPer)
        Me.grpRecTagDetail.Controls.Add(Me.Label11)
        Me.grpRecTagDetail.Controls.Add(Me.txtSalvalue_Amt)
        Me.grpRecTagDetail.Controls.Add(Me.Label5)
        Me.grpRecTagDetail.Controls.Add(Me.txtMcharge)
        Me.grpRecTagDetail.Controls.Add(Me.Label10)
        Me.grpRecTagDetail.Controls.Add(Me.txtWastage)
        Me.grpRecTagDetail.Controls.Add(Me.Label1)
        Me.grpRecTagDetail.Controls.Add(Me.txtLessWt_Wet)
        Me.grpRecTagDetail.Controls.Add(Me.Label9)
        Me.grpRecTagDetail.Controls.Add(Me.txtNetwt_Wet)
        Me.grpRecTagDetail.Controls.Add(Me.btnExit)
        Me.grpRecTagDetail.Controls.Add(Me.btnSave)
        Me.grpRecTagDetail.Controls.Add(Me.btnNew)
        Me.grpRecTagDetail.Controls.Add(Me.txtPcs)
        Me.grpRecTagDetail.Controls.Add(Me.Label18)
        Me.grpRecTagDetail.Controls.Add(Me.txtTAGRowIndex)
        Me.grpRecTagDetail.Controls.Add(Me.gridViewTotal)
        Me.grpRecTagDetail.Controls.Add(Me.gridview)
        Me.grpRecTagDetail.Controls.Add(Me.txtItemId)
        Me.grpRecTagDetail.Controls.Add(Me.Label3)
        Me.grpRecTagDetail.Controls.Add(Me.Label6)
        Me.grpRecTagDetail.Controls.Add(Me.Label2)
        Me.grpRecTagDetail.Controls.Add(Me.txtGrsWt_WET)
        Me.grpRecTagDetail.Controls.Add(Me.txtTranno)
        Me.grpRecTagDetail.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpRecTagDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpRecTagDetail.GroupImage = Nothing
        Me.grpRecTagDetail.GroupTitle = ""
        Me.grpRecTagDetail.Location = New System.Drawing.Point(0, 0)
        Me.grpRecTagDetail.Name = "grpRecTagDetail"
        Me.grpRecTagDetail.Padding = New System.Windows.Forms.Padding(20)
        Me.grpRecTagDetail.PaintGroupBox = False
        Me.grpRecTagDetail.RoundCorners = 10
        Me.grpRecTagDetail.ShadowColor = System.Drawing.SystemColors.InactiveCaption
        Me.grpRecTagDetail.ShadowControl = False
        Me.grpRecTagDetail.ShadowThickness = 3
        Me.grpRecTagDetail.Size = New System.Drawing.Size(1028, 538)
        Me.grpRecTagDetail.TabIndex = 0
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(838, 14)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(80, 15)
        Me.Label7.TabIndex = 22
        Me.Label7.Text = "StnAmt"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtStnAmt_amt
        '
        Me.txtStnAmt_amt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStnAmt_amt.Location = New System.Drawing.Point(841, 31)
        Me.txtStnAmt_amt.MaxLength = 10
        Me.txtStnAmt_amt.Name = "txtStnAmt_amt"
        Me.txtStnAmt_amt.ReadOnly = True
        Me.txtStnAmt_amt.ShortcutsEnabled = False
        Me.txtStnAmt_amt.Size = New System.Drawing.Size(80, 22)
        Me.txtStnAmt_amt.TabIndex = 23
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(758, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 15)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "StnWt"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtstnWt_Wet
        '
        Me.txtstnWt_Wet.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtstnWt_Wet.Location = New System.Drawing.Point(761, 31)
        Me.txtstnWt_Wet.MaxLength = 10
        Me.txtstnWt_Wet.Name = "txtstnWt_Wet"
        Me.txtstnWt_Wet.ReadOnly = True
        Me.txtstnWt_Wet.ShortcutsEnabled = False
        Me.txtstnWt_Wet.Size = New System.Drawing.Size(80, 22)
        Me.txtstnWt_Wet.TabIndex = 21
        '
        'GrpDetail
        '
        Me.GrpDetail.BackColor = System.Drawing.Color.Lavender
        Me.GrpDetail.Controls.Add(Me.txtTablecode)
        Me.GrpDetail.Controls.Add(Me.txt_Purity)
        Me.GrpDetail.Controls.Add(Me.txt_DesignerName)
        Me.GrpDetail.Controls.Add(Me.txt_SubItemname)
        Me.GrpDetail.Controls.Add(Me.txt_Itemname)
        Me.GrpDetail.Controls.Add(Me.Label17)
        Me.GrpDetail.Controls.Add(Me.Label8)
        Me.GrpDetail.Controls.Add(Me.Label16)
        Me.GrpDetail.Controls.Add(Me.Label15)
        Me.GrpDetail.Controls.Add(Me.Label14)
        Me.GrpDetail.Location = New System.Drawing.Point(12, 372)
        Me.GrpDetail.Name = "GrpDetail"
        Me.GrpDetail.Size = New System.Drawing.Size(567, 125)
        Me.GrpDetail.TabIndex = 32
        Me.GrpDetail.TabStop = False
        Me.GrpDetail.Visible = False
        '
        'txtTablecode
        '
        Me.txtTablecode.Location = New System.Drawing.Point(407, 51)
        Me.txtTablecode.Name = "txtTablecode"
        Me.txtTablecode.Size = New System.Drawing.Size(111, 21)
        Me.txtTablecode.TabIndex = 9
        '
        'txt_Purity
        '
        Me.txt_Purity.Location = New System.Drawing.Point(407, 14)
        Me.txt_Purity.Name = "txt_Purity"
        Me.txt_Purity.Size = New System.Drawing.Size(111, 21)
        Me.txt_Purity.TabIndex = 7
        '
        'txt_DesignerName
        '
        Me.txt_DesignerName.Location = New System.Drawing.Point(112, 84)
        Me.txt_DesignerName.Name = "txt_DesignerName"
        Me.txt_DesignerName.Size = New System.Drawing.Size(173, 21)
        Me.txt_DesignerName.TabIndex = 5
        '
        'txt_SubItemname
        '
        Me.txt_SubItemname.Location = New System.Drawing.Point(114, 51)
        Me.txt_SubItemname.Name = "txt_SubItemname"
        Me.txt_SubItemname.Size = New System.Drawing.Size(171, 21)
        Me.txt_SubItemname.TabIndex = 3
        '
        'txt_Itemname
        '
        Me.txt_Itemname.Location = New System.Drawing.Point(114, 14)
        Me.txt_Itemname.Name = "txt_Itemname"
        Me.txt_Itemname.Size = New System.Drawing.Size(171, 21)
        Me.txt_Itemname.TabIndex = 1
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(11, 51)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(89, 13)
        Me.Label17.TabIndex = 2
        Me.Label17.Text = "SubItemName"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(11, 17)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(67, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "ItemName"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(326, 17)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(40, 13)
        Me.Label16.TabIndex = 6
        Me.Label16.Text = "Purity"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(326, 51)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(68, 13)
        Me.Label15.TabIndex = 8
        Me.Label15.Text = "TableCode"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(11, 84)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(94, 13)
        Me.Label14.TabIndex = 4
        Me.Label14.Text = "Designer name"
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(601, 14)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(80, 15)
        Me.Label13.TabIndex = 16
        Me.Label13.Text = "MaxMcGrm"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtMcGrm
        '
        Me.txtMcGrm.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMcGrm.Location = New System.Drawing.Point(601, 31)
        Me.txtMcGrm.MaxLength = 10
        Me.txtMcGrm.Name = "txtMcGrm"
        Me.txtMcGrm.ShortcutsEnabled = False
        Me.txtMcGrm.Size = New System.Drawing.Size(80, 22)
        Me.txtMcGrm.TabIndex = 17
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(440, 14)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(80, 15)
        Me.Label12.TabIndex = 12
        Me.Label12.Text = "MaxWast%"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtWastPer
        '
        Me.txtWastPer.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWastPer.Location = New System.Drawing.Point(441, 31)
        Me.txtWastPer.MaxLength = 10
        Me.txtWastPer.Name = "txtWastPer"
        Me.txtWastPer.ShortcutsEnabled = False
        Me.txtWastPer.Size = New System.Drawing.Size(80, 22)
        Me.txtWastPer.TabIndex = 13
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(919, 14)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(95, 15)
        Me.Label11.TabIndex = 24
        Me.Label11.Text = "SalValue"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSalvalue_Amt
        '
        Me.txtSalvalue_Amt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSalvalue_Amt.Location = New System.Drawing.Point(921, 31)
        Me.txtSalvalue_Amt.MaxLength = 10
        Me.txtSalvalue_Amt.Name = "txtSalvalue_Amt"
        Me.txtSalvalue_Amt.ShortcutsEnabled = False
        Me.txtSalvalue_Amt.Size = New System.Drawing.Size(95, 22)
        Me.txtSalvalue_Amt.TabIndex = 25
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(678, 14)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(80, 15)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "MaxMC"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtMcharge
        '
        Me.txtMcharge.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMcharge.Location = New System.Drawing.Point(681, 31)
        Me.txtMcharge.MaxLength = 10
        Me.txtMcharge.Name = "txtMcharge"
        Me.txtMcharge.ShortcutsEnabled = False
        Me.txtMcharge.Size = New System.Drawing.Size(80, 22)
        Me.txtMcharge.TabIndex = 19
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(521, 14)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(80, 15)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "MaxWast"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtWastage
        '
        Me.txtWastage.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWastage.Location = New System.Drawing.Point(521, 31)
        Me.txtWastage.MaxLength = 10
        Me.txtWastage.Name = "txtWastage"
        Me.txtWastage.ShortcutsEnabled = False
        Me.txtWastage.Size = New System.Drawing.Size(80, 22)
        Me.txtWastage.TabIndex = 15
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(355, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 15)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "LessWt"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtLessWt_Wet
        '
        Me.txtLessWt_Wet.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLessWt_Wet.Location = New System.Drawing.Point(356, 31)
        Me.txtLessWt_Wet.MaxLength = 10
        Me.txtLessWt_Wet.Name = "txtLessWt_Wet"
        Me.txtLessWt_Wet.ShortcutsEnabled = False
        Me.txtLessWt_Wet.Size = New System.Drawing.Size(85, 22)
        Me.txtLessWt_Wet.TabIndex = 11
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(268, 14)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(85, 15)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "NetWt"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtNetwt_Wet
        '
        Me.txtNetwt_Wet.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNetwt_Wet.Location = New System.Drawing.Point(271, 31)
        Me.txtNetwt_Wet.MaxLength = 10
        Me.txtNetwt_Wet.Name = "txtNetwt_Wet"
        Me.txtNetwt_Wet.ShortcutsEnabled = False
        Me.txtNetwt_Wet.Size = New System.Drawing.Size(85, 22)
        Me.txtNetwt_Wet.TabIndex = 9
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(841, 377)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 31
        Me.btnExit.Text = "E&xit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(634, 377)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 29
        Me.btnSave.Text = "&Save[F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.Location = New System.Drawing.Point(738, 377)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 30
        Me.btnNew.Text = "&New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'txtPcs
        '
        Me.txtPcs.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPcs.Location = New System.Drawing.Point(126, 31)
        Me.txtPcs.MaxLength = 10
        Me.txtPcs.Name = "txtPcs"
        Me.txtPcs.ShortcutsEnabled = False
        Me.txtPcs.Size = New System.Drawing.Size(60, 22)
        Me.txtPcs.TabIndex = 5
        '
        'Label18
        '
        Me.Label18.BackColor = System.Drawing.Color.Transparent
        Me.Label18.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(183, 14)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(85, 15)
        Me.Label18.TabIndex = 6
        Me.Label18.Text = "GrsWt"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTAGRowIndex
        '
        Me.txtTAGRowIndex.Location = New System.Drawing.Point(905, 86)
        Me.txtTAGRowIndex.Name = "txtTAGRowIndex"
        Me.txtTAGRowIndex.Size = New System.Drawing.Size(10, 21)
        Me.txtTAGRowIndex.TabIndex = 28
        Me.txtTAGRowIndex.Visible = False
        '
        'gridViewTotal
        '
        Me.gridViewTotal.AllowUserToAddRows = False
        Me.gridViewTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewTotal.Location = New System.Drawing.Point(3, 342)
        Me.gridViewTotal.Name = "gridViewTotal"
        Me.gridViewTotal.Size = New System.Drawing.Size(1013, 20)
        Me.gridViewTotal.TabIndex = 27
        '
        'gridview
        '
        Me.gridview.AllowUserToAddRows = False
        Me.gridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridview.Location = New System.Drawing.Point(3, 56)
        Me.gridview.Name = "gridview"
        Me.gridview.ReadOnly = True
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.AliceBlue
        Me.gridview.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.gridview.Size = New System.Drawing.Size(1013, 286)
        Me.gridview.TabIndex = 26
        '
        'txtItemId
        '
        Me.txtItemId.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemId.Location = New System.Drawing.Point(64, 31)
        Me.txtItemId.MaxLength = 10
        Me.txtItemId.Name = "txtItemId"
        Me.txtItemId.ReadOnly = True
        Me.txtItemId.ShortcutsEnabled = False
        Me.txtItemId.Size = New System.Drawing.Size(62, 22)
        Me.txtItemId.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(63, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "ItemId"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(125, 14)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(60, 15)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Pcs"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(4, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 15)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Tranno"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtGrsWt_WET
        '
        Me.txtGrsWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGrsWt_WET.Location = New System.Drawing.Point(186, 31)
        Me.txtGrsWt_WET.MaxLength = 10
        Me.txtGrsWt_WET.Name = "txtGrsWt_WET"
        Me.txtGrsWt_WET.ShortcutsEnabled = False
        Me.txtGrsWt_WET.Size = New System.Drawing.Size(85, 22)
        Me.txtGrsWt_WET.TabIndex = 7
        '
        'txtTranno
        '
        Me.txtTranno.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTranno.Location = New System.Drawing.Point(4, 31)
        Me.txtTranno.MaxLength = 15
        Me.txtTranno.Name = "txtTranno"
        Me.txtTranno.ShortcutsEnabled = False
        Me.txtTranno.Size = New System.Drawing.Size(60, 22)
        Me.txtTranno.TabIndex = 1
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 2000
        Me.ToolTip1.ReshowDelay = 100
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.StoneDetailsToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(187, 114)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'StoneDetailsToolStripMenuItem
        '
        Me.StoneDetailsToolStripMenuItem.Name = "StoneDetailsToolStripMenuItem"
        Me.StoneDetailsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.StoneDetailsToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.StoneDetailsToolStripMenuItem.Text = "Stone Details"
        '
        'ReceiptTagPosting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(1028, 538)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpRecTagDetail)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(1078, 745)
        Me.Name = "ReceiptTagPosting"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpRecTagDetail.ResumeLayout(False)
        Me.grpRecTagDetail.PerformLayout()
        Me.GrpDetail.ResumeLayout(False)
        Me.GrpDetail.PerformLayout()
        CType(Me.gridViewTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpRecTagDetail As CodeVendor.Controls.Grouper
    Friend WithEvents txtTAGRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents gridViewTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridview As System.Windows.Forms.DataGridView
    Friend WithEvents txtItemId As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtTranno As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents txtPcs As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtNetwt_Wet As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtLessWt_Wet As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtWastage As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtMcharge As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtSalvalue_Amt As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtWastPer As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtMcGrm As System.Windows.Forms.TextBox
    Friend WithEvents StoneDetailsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GrpDetail As System.Windows.Forms.GroupBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtStnAmt_amt As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtstnWt_Wet As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtTablecode As System.Windows.Forms.TextBox
    Friend WithEvents txt_Purity As System.Windows.Forms.TextBox
    Friend WithEvents txt_DesignerName As System.Windows.Forms.TextBox
    Friend WithEvents txt_SubItemname As System.Windows.Forms.TextBox
    Friend WithEvents txt_Itemname As System.Windows.Forms.TextBox
End Class
