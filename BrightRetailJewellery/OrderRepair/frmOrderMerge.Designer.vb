<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOrderMerge
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
        Me.grpTagDetail = New CodeVendor.Controls.Grouper
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtOWast = New System.Windows.Forms.TextBox
        Me.txtOMc = New System.Windows.Forms.TextBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtSubItemName = New System.Windows.Forms.TextBox
        Me.txtItemname = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtOnetwt = New System.Windows.Forms.TextBox
        Me.txtPcs = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtOtagno = New System.Windows.Forms.TextBox
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
        Me.txtOrate = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtOrderNo = New System.Windows.Forms.TextBox
        Me.Grouper1 = New CodeVendor.Controls.Grouper
        Me.Grouper3 = New CodeVendor.Controls.Grouper
        Me.Grouper2 = New CodeVendor.Controls.Grouper
        Me.lblOrderDate = New System.Windows.Forms.Label
        Me.lblValue = New System.Windows.Forms.Label
        Me.lblRate = New System.Windows.Forms.Label
        Me.lblGrsWt = New System.Windows.Forms.Label
        Me.lblAmount = New System.Windows.Forms.Label
        Me.lblCustomer = New System.Windows.Forms.Label
        Me.lblOrderno = New System.Windows.Forms.Label
        Me.lblNetWt = New System.Windows.Forms.Label
        Me.gridOutTotal = New System.Windows.Forms.DataGridView
        Me.gridOutstanding = New System.Windows.Forms.DataGridView
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpTagDetail.SuspendLayout()
        CType(Me.gridTAGTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridTAG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Grouper1.SuspendLayout()
        Me.Grouper3.SuspendLayout()
        Me.Grouper2.SuspendLayout()
        CType(Me.gridOutTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridOutstanding, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.grpTagDetail.Controls.Add(Me.Label10)
        Me.grpTagDetail.Controls.Add(Me.Label9)
        Me.grpTagDetail.Controls.Add(Me.txtOWast)
        Me.grpTagDetail.Controls.Add(Me.txtOMc)
        Me.grpTagDetail.Controls.Add(Me.Label8)
        Me.grpTagDetail.Controls.Add(Me.Label7)
        Me.grpTagDetail.Controls.Add(Me.txtSubItemName)
        Me.grpTagDetail.Controls.Add(Me.txtItemname)
        Me.grpTagDetail.Controls.Add(Me.Label1)
        Me.grpTagDetail.Controls.Add(Me.txtOnetwt)
        Me.grpTagDetail.Controls.Add(Me.txtPcs)
        Me.grpTagDetail.Controls.Add(Me.Label18)
        Me.grpTagDetail.Controls.Add(Me.Label15)
        Me.grpTagDetail.Controls.Add(Me.txtOtagno)
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
        Me.grpTagDetail.Controls.Add(Me.txtOrate)
        Me.grpTagDetail.Controls.Add(Me.Label5)
        Me.grpTagDetail.Controls.Add(Me.txtOrderNo)
        Me.grpTagDetail.Controls.Add(Me.Grouper1)
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
        Me.grpTagDetail.Size = New System.Drawing.Size(999, 621)
        Me.grpTagDetail.TabIndex = 0
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(923, 14)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(64, 15)
        Me.Label10.TabIndex = 41
        Me.Label10.Text = "Wast"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(849, 14)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 15)
        Me.Label9.TabIndex = 40
        Me.Label9.Text = "Mc"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtOWast
        '
        Me.txtOWast.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOWast.Location = New System.Drawing.Point(926, 31)
        Me.txtOWast.MaxLength = 10
        Me.txtOWast.Name = "txtOWast"
        Me.txtOWast.ShortcutsEnabled = False
        Me.txtOWast.Size = New System.Drawing.Size(67, 22)
        Me.txtOWast.TabIndex = 39
        '
        'txtOMc
        '
        Me.txtOMc.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOMc.Location = New System.Drawing.Point(843, 31)
        Me.txtOMc.MaxLength = 10
        Me.txtOMc.Name = "txtOMc"
        Me.txtOMc.ShortcutsEnabled = False
        Me.txtOMc.Size = New System.Drawing.Size(82, 22)
        Me.txtOMc.TabIndex = 38
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(28, 117)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 14
        Me.btnExit.Text = "E&xit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(28, 26)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 12
        Me.btnSave.Text = "&Save[F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.Location = New System.Drawing.Point(28, 71)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 13
        Me.btnNew.Text = "&New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(164, 14)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(79, 15)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "ItemName"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(348, 14)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(93, 15)
        Me.Label7.TabIndex = 36
        Me.Label7.Text = "SubItem" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSubItemName
        '
        Me.txtSubItemName.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubItemName.Location = New System.Drawing.Point(344, 31)
        Me.txtSubItemName.MaxLength = 10
        Me.txtSubItemName.Name = "txtSubItemName"
        Me.txtSubItemName.ShortcutsEnabled = False
        Me.txtSubItemName.Size = New System.Drawing.Size(106, 22)
        Me.txtSubItemName.TabIndex = 5
        '
        'txtItemname
        '
        Me.txtItemname.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemname.Location = New System.Drawing.Point(133, 31)
        Me.txtItemname.MaxLength = 10
        Me.txtItemname.Name = "txtItemname"
        Me.txtItemname.ShortcutsEnabled = False
        Me.txtItemname.Size = New System.Drawing.Size(138, 22)
        Me.txtItemname.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(668, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 15)
        Me.Label1.TabIndex = 32
        Me.Label1.Text = "NetWt"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtOnetwt
        '
        Me.txtOnetwt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOnetwt.Location = New System.Drawing.Point(670, 31)
        Me.txtOnetwt.MaxLength = 10
        Me.txtOnetwt.Name = "txtOnetwt"
        Me.txtOnetwt.ShortcutsEnabled = False
        Me.txtOnetwt.Size = New System.Drawing.Size(85, 22)
        Me.txtOnetwt.TabIndex = 9
        '
        'txtPcs
        '
        Me.txtPcs.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPcs.Location = New System.Drawing.Point(522, 31)
        Me.txtPcs.MaxLength = 10
        Me.txtPcs.Name = "txtPcs"
        Me.txtPcs.ShortcutsEnabled = False
        Me.txtPcs.Size = New System.Drawing.Size(61, 22)
        Me.txtPcs.TabIndex = 7
        '
        'Label18
        '
        Me.Label18.BackColor = System.Drawing.Color.Transparent
        Me.Label18.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(581, 14)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(85, 15)
        Me.Label18.TabIndex = 30
        Me.Label18.Text = "GrsWt"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.Color.Transparent
        Me.Label15.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(442, 14)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(85, 15)
        Me.Label15.TabIndex = 26
        Me.Label15.Text = "TagNo"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtOtagno
        '
        Me.txtOtagno.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOtagno.Location = New System.Drawing.Point(451, 31)
        Me.txtOtagno.MaxLength = 10
        Me.txtOtagno.Name = "txtOtagno"
        Me.txtOtagno.ShortcutsEnabled = False
        Me.txtOtagno.Size = New System.Drawing.Size(70, 22)
        Me.txtOtagno.TabIndex = 6
        '
        'txtTAGRowIndex
        '
        Me.txtTAGRowIndex.Location = New System.Drawing.Point(671, 127)
        Me.txtTAGRowIndex.Name = "txtTAGRowIndex"
        Me.txtTAGRowIndex.Size = New System.Drawing.Size(85, 21)
        Me.txtTAGRowIndex.TabIndex = 22
        Me.txtTAGRowIndex.Visible = False
        '
        'gridTAGTotal
        '
        Me.gridTAGTotal.AllowUserToAddRows = False
        Me.gridTAGTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridTAGTotal.Enabled = False
        Me.gridTAGTotal.Location = New System.Drawing.Point(8, 384)
        Me.gridTAGTotal.Name = "gridTAGTotal"
        Me.gridTAGTotal.ReadOnly = True
        Me.gridTAGTotal.Size = New System.Drawing.Size(985, 19)
        Me.gridTAGTotal.TabIndex = 25
        '
        'gridTAG
        '
        Me.gridTAG.AllowUserToAddRows = False
        Me.gridTAG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridTAG.Location = New System.Drawing.Point(8, 55)
        Me.gridTAG.Name = "gridTAG"
        Me.gridTAG.ReadOnly = True
        Me.gridTAG.Size = New System.Drawing.Size(985, 328)
        Me.gridTAG.TabIndex = 11
        '
        'txtItemId
        '
        Me.txtItemId.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemId.Location = New System.Drawing.Point(72, 31)
        Me.txtItemId.MaxLength = 10
        Me.txtItemId.Name = "txtItemId"
        Me.txtItemId.ShortcutsEnabled = False
        Me.txtItemId.Size = New System.Drawing.Size(60, 22)
        Me.txtItemId.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(74, 14)
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
        Me.Label6.Location = New System.Drawing.Point(516, 14)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(85, 15)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Pcs"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSubItemId
        '
        Me.txtSubItemId.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubItemId.Location = New System.Drawing.Point(272, 31)
        Me.txtSubItemId.MaxLength = 5
        Me.txtSubItemId.Name = "txtSubItemId"
        Me.txtSubItemId.ShortcutsEnabled = False
        Me.txtSubItemId.Size = New System.Drawing.Size(71, 22)
        Me.txtSubItemId.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(8, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 15)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Order No"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(270, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 15)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "SubItemId"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTagGrsWt_WET
        '
        Me.txtTagGrsWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTagGrsWt_WET.Location = New System.Drawing.Point(584, 31)
        Me.txtTagGrsWt_WET.MaxLength = 10
        Me.txtTagGrsWt_WET.Name = "txtTagGrsWt_WET"
        Me.txtTagGrsWt_WET.ShortcutsEnabled = False
        Me.txtTagGrsWt_WET.Size = New System.Drawing.Size(85, 22)
        Me.txtTagGrsWt_WET.TabIndex = 8
        '
        'txtOrate
        '
        Me.txtOrate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrate.Location = New System.Drawing.Point(756, 31)
        Me.txtOrate.MaxLength = 10
        Me.txtOrate.Name = "txtOrate"
        Me.txtOrate.ShortcutsEnabled = False
        Me.txtOrate.Size = New System.Drawing.Size(86, 22)
        Me.txtOrate.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(753, 14)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(85, 15)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Rate"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtOrderNo
        '
        Me.txtOrderNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrderNo.Location = New System.Drawing.Point(10, 31)
        Me.txtOrderNo.MaxLength = 15
        Me.txtOrderNo.Name = "txtOrderNo"
        Me.txtOrderNo.ShortcutsEnabled = False
        Me.txtOrderNo.Size = New System.Drawing.Size(61, 22)
        Me.txtOrderNo.TabIndex = 1
        '
        'Grouper1
        '
        Me.Grouper1.BackgroundColor = System.Drawing.SystemColors.InactiveCaption
        Me.Grouper1.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper1.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper1.BorderThickness = 1.0!
        Me.Grouper1.Controls.Add(Me.Grouper3)
        Me.Grouper1.Controls.Add(Me.Grouper2)
        Me.Grouper1.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper1.GroupImage = Nothing
        Me.Grouper1.GroupTitle = "Advance Details"
        Me.Grouper1.Location = New System.Drawing.Point(7, 406)
        Me.Grouper1.Name = "Grouper1"
        Me.Grouper1.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper1.PaintGroupBox = False
        Me.Grouper1.RoundCorners = 10
        Me.Grouper1.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper1.ShadowControl = False
        Me.Grouper1.ShadowThickness = 3
        Me.Grouper1.Size = New System.Drawing.Size(985, 176)
        Me.Grouper1.TabIndex = 53
        '
        'Grouper3
        '
        Me.Grouper3.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper3.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Grouper3.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper3.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper3.BorderThickness = 1.0!
        Me.Grouper3.Controls.Add(Me.btnNew)
        Me.Grouper3.Controls.Add(Me.btnSave)
        Me.Grouper3.Controls.Add(Me.btnExit)
        Me.Grouper3.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper3.GroupImage = Nothing
        Me.Grouper3.GroupTitle = ""
        Me.Grouper3.Location = New System.Drawing.Point(823, 8)
        Me.Grouper3.Name = "Grouper3"
        Me.Grouper3.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper3.PaintGroupBox = False
        Me.Grouper3.RoundCorners = 10
        Me.Grouper3.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper3.ShadowControl = False
        Me.Grouper3.ShadowThickness = 3
        Me.Grouper3.Size = New System.Drawing.Size(157, 164)
        Me.Grouper3.TabIndex = 54
        '
        'Grouper2
        '
        Me.Grouper2.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper2.BackgroundGradientColor = System.Drawing.Color.White
        Me.Grouper2.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper2.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper2.BorderThickness = 1.0!
        Me.Grouper2.Controls.Add(Me.lblOrderDate)
        Me.Grouper2.Controls.Add(Me.lblValue)
        Me.Grouper2.Controls.Add(Me.lblRate)
        Me.Grouper2.Controls.Add(Me.lblGrsWt)
        Me.Grouper2.Controls.Add(Me.lblAmount)
        Me.Grouper2.Controls.Add(Me.lblCustomer)
        Me.Grouper2.Controls.Add(Me.lblOrderno)
        Me.Grouper2.Controls.Add(Me.lblNetWt)
        Me.Grouper2.Controls.Add(Me.gridOutTotal)
        Me.Grouper2.Controls.Add(Me.gridOutstanding)
        Me.Grouper2.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper2.GroupImage = Nothing
        Me.Grouper2.GroupTitle = ""
        Me.Grouper2.Location = New System.Drawing.Point(1, 9)
        Me.Grouper2.Name = "Grouper2"
        Me.Grouper2.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper2.PaintGroupBox = False
        Me.Grouper2.RoundCorners = 10
        Me.Grouper2.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper2.ShadowControl = False
        Me.Grouper2.ShadowThickness = 3
        Me.Grouper2.Size = New System.Drawing.Size(819, 163)
        Me.Grouper2.TabIndex = 53
        '
        'lblOrderDate
        '
        Me.lblOrderDate.BackColor = System.Drawing.Color.Transparent
        Me.lblOrderDate.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrderDate.Location = New System.Drawing.Point(77, 13)
        Me.lblOrderDate.Name = "lblOrderDate"
        Me.lblOrderDate.Size = New System.Drawing.Size(105, 15)
        Me.lblOrderDate.TabIndex = 61
        Me.lblOrderDate.Text = "Order Date"
        Me.lblOrderDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblValue
        '
        Me.lblValue.BackColor = System.Drawing.Color.Transparent
        Me.lblValue.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblValue.Location = New System.Drawing.Point(702, 14)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.Size = New System.Drawing.Size(102, 15)
        Me.lblValue.TabIndex = 60
        Me.lblValue.Text = "Value"
        Me.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblRate
        '
        Me.lblRate.BackColor = System.Drawing.Color.Transparent
        Me.lblRate.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRate.Location = New System.Drawing.Point(609, 14)
        Me.lblRate.Name = "lblRate"
        Me.lblRate.Size = New System.Drawing.Size(98, 15)
        Me.lblRate.TabIndex = 59
        Me.lblRate.Text = "Rate"
        Me.lblRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblGrsWt
        '
        Me.lblGrsWt.BackColor = System.Drawing.Color.Transparent
        Me.lblGrsWt.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrsWt.Location = New System.Drawing.Point(293, 14)
        Me.lblGrsWt.Name = "lblGrsWt"
        Me.lblGrsWt.Size = New System.Drawing.Size(111, 15)
        Me.lblGrsWt.TabIndex = 58
        Me.lblGrsWt.Text = "GrossWeight"
        Me.lblGrsWt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblAmount
        '
        Me.lblAmount.BackColor = System.Drawing.Color.Transparent
        Me.lblAmount.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.Location = New System.Drawing.Point(513, 14)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.Size = New System.Drawing.Size(99, 15)
        Me.lblAmount.TabIndex = 57
        Me.lblAmount.Text = "Amount"
        Me.lblAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCustomer
        '
        Me.lblCustomer.BackColor = System.Drawing.Color.Transparent
        Me.lblCustomer.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCustomer.Location = New System.Drawing.Point(179, 13)
        Me.lblCustomer.Name = "lblCustomer"
        Me.lblCustomer.Size = New System.Drawing.Size(114, 15)
        Me.lblCustomer.TabIndex = 55
        Me.lblCustomer.Text = "Customer"
        Me.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblOrderno
        '
        Me.lblOrderno.BackColor = System.Drawing.Color.Transparent
        Me.lblOrderno.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrderno.Location = New System.Drawing.Point(2, 13)
        Me.lblOrderno.Name = "lblOrderno"
        Me.lblOrderno.Size = New System.Drawing.Size(73, 15)
        Me.lblOrderno.TabIndex = 54
        Me.lblOrderno.Text = "Order No"
        Me.lblOrderno.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblNetWt
        '
        Me.lblNetWt.BackColor = System.Drawing.Color.Transparent
        Me.lblNetWt.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNetWt.Location = New System.Drawing.Point(417, 14)
        Me.lblNetWt.Name = "lblNetWt"
        Me.lblNetWt.Size = New System.Drawing.Size(96, 15)
        Me.lblNetWt.TabIndex = 56
        Me.lblNetWt.Text = "NetWeight"
        Me.lblNetWt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gridOutTotal
        '
        Me.gridOutTotal.AllowUserToAddRows = False
        Me.gridOutTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridOutTotal.Enabled = False
        Me.gridOutTotal.Location = New System.Drawing.Point(3, 137)
        Me.gridOutTotal.Name = "gridOutTotal"
        Me.gridOutTotal.ReadOnly = True
        Me.gridOutTotal.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.gridOutTotal.Size = New System.Drawing.Size(808, 19)
        Me.gridOutTotal.TabIndex = 53
        '
        'gridOutstanding
        '
        Me.gridOutstanding.AllowUserToAddRows = False
        Me.gridOutstanding.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridOutstanding.Enabled = False
        Me.gridOutstanding.Location = New System.Drawing.Point(3, 31)
        Me.gridOutstanding.Name = "gridOutstanding"
        Me.gridOutstanding.ReadOnly = True
        Me.gridOutstanding.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.gridOutstanding.Size = New System.Drawing.Size(808, 105)
        Me.gridOutstanding.TabIndex = 52
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
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 70)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'frmOrderMerge
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(999, 621)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpTagDetail)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(1078, 745)
        Me.Name = "frmOrderMerge"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.grpTagDetail.ResumeLayout(False)
        Me.grpTagDetail.PerformLayout()
        CType(Me.gridTAGTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridTAG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Grouper1.ResumeLayout(False)
        Me.Grouper3.ResumeLayout(False)
        Me.Grouper2.ResumeLayout(False)
        CType(Me.gridOutTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridOutstanding, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents txtOrate As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtOrderNo As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtOtagno As System.Windows.Forms.TextBox
    Friend WithEvents txtPcs As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtOnetwt As System.Windows.Forms.TextBox
    Friend WithEvents txtSubItemName As System.Windows.Forms.TextBox
    Friend WithEvents txtItemname As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents txtOWast As System.Windows.Forms.TextBox
    Friend WithEvents txtOMc As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Grouper1 As CodeVendor.Controls.Grouper
    Friend WithEvents Grouper2 As CodeVendor.Controls.Grouper
    Friend WithEvents lblOrderDate As System.Windows.Forms.Label
    Friend WithEvents lblValue As System.Windows.Forms.Label
    Friend WithEvents lblRate As System.Windows.Forms.Label
    Friend WithEvents lblGrsWt As System.Windows.Forms.Label
    Friend WithEvents lblAmount As System.Windows.Forms.Label
    Friend WithEvents lblCustomer As System.Windows.Forms.Label
    Friend WithEvents lblOrderno As System.Windows.Forms.Label
    Friend WithEvents lblNetWt As System.Windows.Forms.Label
    Friend WithEvents gridOutTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridOutstanding As System.Windows.Forms.DataGridView
    Friend WithEvents Grouper3 As CodeVendor.Controls.Grouper
End Class
