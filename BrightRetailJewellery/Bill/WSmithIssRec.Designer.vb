<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WSmithIssRec
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
        Me.grpHeader = New CodeVendor.Controls.Grouper
        Me.dtpTranDate = New BrighttechPack.DatePicker(Me.components)
        Me.lblBalance = New System.Windows.Forms.Label
        Me.Label26 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.cmbTranType = New System.Windows.Forms.ComboBox
        Me.cmbParty_MAN = New System.Windows.Forms.ComboBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Grouper1 = New CodeVendor.Controls.Grouper
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.txtPUNetWt_WET = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtPUGrsWt_WET = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.txtPUPcs_NUM = New System.Windows.Forms.TextBox
        Me.gridPurTotal = New System.Windows.Forms.DataGridView
        Me.Label11 = New System.Windows.Forms.Label
        Me.gridPur = New System.Windows.Forms.DataGridView
        Me.txtPUCategory = New System.Windows.Forms.TextBox
        Me.txtPURowIndex = New System.Windows.Forms.TextBox
        Me.Label41 = New System.Windows.Forms.Label
        Me.txtPuTotalAmt_AMT = New System.Windows.Forms.TextBox
        Me.Label43 = New System.Windows.Forms.Label
        Me.txtPuPureWt_WET = New System.Windows.Forms.TextBox
        Me.Label42 = New System.Windows.Forms.Label
        Me.txtPuTouch_AMT = New System.Windows.Forms.TextBox
        Me.txtPuMc_AMT = New System.Windows.Forms.TextBox
        Me.Label47 = New System.Windows.Forms.Label
        Me.txtPUStoneWt_WET = New System.Windows.Forms.TextBox
        Me.grpPu = New CodeVendor.Controls.Grouper
        Me.Label46 = New System.Windows.Forms.Label
        Me.txtPurKaTouch_AMT = New System.Windows.Forms.TextBox
        Me.Label45 = New System.Windows.Forms.Label
        Me.txtPuKatchaWt_WET = New System.Windows.Forms.TextBox
        Me.Label44 = New System.Windows.Forms.Label
        Me.grpHeader.SuspendLayout()
        Me.Grouper1.SuspendLayout()
        CType(Me.gridPurTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridPur, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpPu.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpHeader
        '
        Me.grpHeader.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpHeader.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpHeader.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpHeader.BorderColor = System.Drawing.Color.Transparent
        Me.grpHeader.BorderThickness = 1.0!
        Me.grpHeader.Controls.Add(Me.dtpTranDate)
        Me.grpHeader.Controls.Add(Me.lblBalance)
        Me.grpHeader.Controls.Add(Me.Label26)
        Me.grpHeader.Controls.Add(Me.Label1)
        Me.grpHeader.Controls.Add(Me.Label25)
        Me.grpHeader.Controls.Add(Me.cmbTranType)
        Me.grpHeader.Controls.Add(Me.cmbParty_MAN)
        Me.grpHeader.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpHeader.GroupImage = Nothing
        Me.grpHeader.GroupTitle = ""
        Me.grpHeader.Location = New System.Drawing.Point(10, 0)
        Me.grpHeader.Name = "grpHeader"
        Me.grpHeader.Padding = New System.Windows.Forms.Padding(20)
        Me.grpHeader.PaintGroupBox = False
        Me.grpHeader.RoundCorners = 10
        Me.grpHeader.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpHeader.ShadowControl = False
        Me.grpHeader.ShadowThickness = 3
        Me.grpHeader.Size = New System.Drawing.Size(944, 103)
        Me.grpHeader.TabIndex = 0
        '
        'dtpTranDate
        '
        Me.dtpTranDate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.dtpTranDate.Location = New System.Drawing.Point(98, 27)
        Me.dtpTranDate.Mask = "##/##/####"
        Me.dtpTranDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTranDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTranDate.Name = "dtpTranDate"
        Me.dtpTranDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTranDate.Size = New System.Drawing.Size(96, 22)
        Me.dtpTranDate.TabIndex = 1
        Me.dtpTranDate.Text = "07/03/9998"
        Me.dtpTranDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblBalance
        '
        Me.lblBalance.AutoSize = True
        Me.lblBalance.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBalance.ForeColor = System.Drawing.Color.Red
        Me.lblBalance.Location = New System.Drawing.Point(95, 51)
        Me.lblBalance.Name = "lblBalance"
        Me.lblBalance.Size = New System.Drawing.Size(61, 14)
        Me.lblBalance.TabIndex = 2
        Me.lblBalance.Text = "Label48"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(456, 75)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(101, 14)
        Me.Label26.TabIndex = 5
        Me.Label26.Text = "Mode Of Entry"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(27, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 14)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Tran Date"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(27, 75)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(45, 14)
        Me.Label25.TabIndex = 3
        Me.Label25.Text = "Name"
        '
        'cmbTranType
        '
        Me.cmbTranType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTranType.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTranType.FormattingEnabled = True
        Me.cmbTranType.Location = New System.Drawing.Point(569, 72)
        Me.cmbTranType.Name = "cmbTranType"
        Me.cmbTranType.Size = New System.Drawing.Size(148, 22)
        Me.cmbTranType.TabIndex = 6
        '
        'cmbParty_MAN
        '
        Me.cmbParty_MAN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbParty_MAN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbParty_MAN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbParty_MAN.FormattingEnabled = True
        Me.cmbParty_MAN.Location = New System.Drawing.Point(98, 72)
        Me.cmbParty_MAN.Name = "cmbParty_MAN"
        Me.cmbParty_MAN.Size = New System.Drawing.Size(352, 22)
        Me.cmbParty_MAN.TabIndex = 4
        '
        'Panel1
        '
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(10, 392)
        Me.Panel1.TabIndex = 2
        '
        'Panel2
        '
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel2.Location = New System.Drawing.Point(954, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(10, 392)
        Me.Panel2.TabIndex = 3
        '
        'Panel3
        '
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(10, 382)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(944, 10)
        Me.Panel3.TabIndex = 3
        '
        'Grouper1
        '
        Me.Grouper1.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper1.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper1.BorderThickness = 1.0!
        Me.Grouper1.Controls.Add(Me.btnExit)
        Me.Grouper1.Controls.Add(Me.btnNew)
        Me.Grouper1.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Grouper1.GroupImage = Nothing
        Me.Grouper1.GroupTitle = ""
        Me.Grouper1.Location = New System.Drawing.Point(10, 308)
        Me.Grouper1.Name = "Grouper1"
        Me.Grouper1.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper1.PaintGroupBox = False
        Me.Grouper1.RoundCorners = 10
        Me.Grouper1.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper1.ShadowControl = False
        Me.Grouper1.ShadowThickness = 3
        Me.Grouper1.Size = New System.Drawing.Size(944, 74)
        Me.Grouper1.TabIndex = 2
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(820, 21)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(105, 30)
        Me.btnExit.TabIndex = 1
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.Location = New System.Drawing.Point(709, 21)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(105, 30)
        Me.btnNew.TabIndex = 0
        Me.btnNew.Text = "&New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'txtPUNetWt_WET
        '
        Me.txtPUNetWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUNetWt_WET.Location = New System.Drawing.Point(516, 31)
        Me.txtPUNetWt_WET.MaxLength = 10
        Me.txtPUNetWt_WET.Name = "txtPUNetWt_WET"
        Me.txtPUNetWt_WET.Size = New System.Drawing.Size(73, 22)
        Me.txtPUNetWt_WET.TabIndex = 13
        Me.txtPUNetWt_WET.Text = "999.999"
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(377, 12)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(69, 17)
        Me.Label16.TabIndex = 8
        Me.Label16.Text = "Grswt"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(343, 12)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(33, 17)
        Me.Label14.TabIndex = 6
        Me.Label14.Text = "Pcs"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUGrsWt_WET
        '
        Me.txtPUGrsWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUGrsWt_WET.Location = New System.Drawing.Point(377, 31)
        Me.txtPUGrsWt_WET.MaxLength = 10
        Me.txtPUGrsWt_WET.Name = "txtPUGrsWt_WET"
        Me.txtPUGrsWt_WET.Size = New System.Drawing.Size(69, 22)
        Me.txtPUGrsWt_WET.TabIndex = 9
        Me.txtPUGrsWt_WET.Text = "999.999"
        '
        'Label20
        '
        Me.Label20.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(516, 12)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(73, 17)
        Me.Label20.TabIndex = 12
        Me.Label20.Text = "Net WT"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUPcs_NUM
        '
        Me.txtPUPcs_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUPcs_NUM.Location = New System.Drawing.Point(343, 31)
        Me.txtPUPcs_NUM.MaxLength = 5
        Me.txtPUPcs_NUM.Name = "txtPUPcs_NUM"
        Me.txtPUPcs_NUM.Size = New System.Drawing.Size(33, 22)
        Me.txtPUPcs_NUM.TabIndex = 7
        Me.txtPUPcs_NUM.Text = "9999"
        '
        'gridPurTotal
        '
        Me.gridPurTotal.AllowUserToAddRows = False
        Me.gridPurTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridPurTotal.Enabled = False
        Me.gridPurTotal.Location = New System.Drawing.Point(8, 178)
        Me.gridPurTotal.Name = "gridPurTotal"
        Me.gridPurTotal.ReadOnly = True
        Me.gridPurTotal.Size = New System.Drawing.Size(933, 19)
        Me.gridPurTotal.TabIndex = 24
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(9, 12)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(199, 17)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Category"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gridPur
        '
        Me.gridPur.AllowUserToAddRows = False
        Me.gridPur.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridPur.Location = New System.Drawing.Point(8, 54)
        Me.gridPur.Name = "gridPur"
        Me.gridPur.ReadOnly = True
        Me.gridPur.Size = New System.Drawing.Size(933, 124)
        Me.gridPur.TabIndex = 22
        '
        'txtPUCategory
        '
        Me.txtPUCategory.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUCategory.Location = New System.Drawing.Point(9, 31)
        Me.txtPUCategory.Name = "txtPUCategory"
        Me.txtPUCategory.Size = New System.Drawing.Size(199, 22)
        Me.txtPUCategory.TabIndex = 1
        '
        'txtPURowIndex
        '
        Me.txtPURowIndex.Location = New System.Drawing.Point(754, 80)
        Me.txtPURowIndex.Name = "txtPURowIndex"
        Me.txtPURowIndex.Size = New System.Drawing.Size(8, 21)
        Me.txtPURowIndex.TabIndex = 23
        Me.txtPURowIndex.Visible = False
        '
        'Label41
        '
        Me.Label41.BackColor = System.Drawing.Color.Transparent
        Me.Label41.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.Location = New System.Drawing.Point(827, 14)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(97, 15)
        Me.Label41.TabIndex = 20
        Me.Label41.Text = "Total Amt"
        Me.Label41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPuTotalAmt_AMT
        '
        Me.txtPuTotalAmt_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPuTotalAmt_AMT.Location = New System.Drawing.Point(827, 31)
        Me.txtPuTotalAmt_AMT.MaxLength = 12
        Me.txtPuTotalAmt_AMT.Name = "txtPuTotalAmt_AMT"
        Me.txtPuTotalAmt_AMT.Size = New System.Drawing.Size(97, 22)
        Me.txtPuTotalAmt_AMT.TabIndex = 21
        Me.txtPuTotalAmt_AMT.Text = "9999999.99"
        '
        'Label43
        '
        Me.Label43.BackColor = System.Drawing.Color.Transparent
        Me.Label43.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.Location = New System.Drawing.Point(659, 14)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(81, 15)
        Me.Label43.TabIndex = 16
        Me.Label43.Text = "PureWt"
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPuPureWt_WET
        '
        Me.txtPuPureWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPuPureWt_WET.Location = New System.Drawing.Point(659, 31)
        Me.txtPuPureWt_WET.MaxLength = 10
        Me.txtPuPureWt_WET.Name = "txtPuPureWt_WET"
        Me.txtPuPureWt_WET.Size = New System.Drawing.Size(81, 22)
        Me.txtPuPureWt_WET.TabIndex = 17
        Me.txtPuPureWt_WET.Text = "999.999"
        '
        'Label42
        '
        Me.Label42.BackColor = System.Drawing.Color.Transparent
        Me.Label42.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.Location = New System.Drawing.Point(590, 14)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(68, 15)
        Me.Label42.TabIndex = 14
        Me.Label42.Text = "Touch"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPuTouch_AMT
        '
        Me.txtPuTouch_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPuTouch_AMT.Location = New System.Drawing.Point(590, 31)
        Me.txtPuTouch_AMT.MaxLength = 10
        Me.txtPuTouch_AMT.Name = "txtPuTouch_AMT"
        Me.txtPuTouch_AMT.Size = New System.Drawing.Size(68, 22)
        Me.txtPuTouch_AMT.TabIndex = 15
        Me.txtPuTouch_AMT.Text = "999.999"
        '
        'txtPuMc_AMT
        '
        Me.txtPuMc_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPuMc_AMT.Location = New System.Drawing.Point(741, 31)
        Me.txtPuMc_AMT.Name = "txtPuMc_AMT"
        Me.txtPuMc_AMT.Size = New System.Drawing.Size(84, 22)
        Me.txtPuMc_AMT.TabIndex = 19
        '
        'Label47
        '
        Me.Label47.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.Location = New System.Drawing.Point(447, 12)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(68, 17)
        Me.Label47.TabIndex = 10
        Me.Label47.Text = "Stn Wt"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUStoneWt_WET
        '
        Me.txtPUStoneWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUStoneWt_WET.Location = New System.Drawing.Point(447, 31)
        Me.txtPUStoneWt_WET.MaxLength = 10
        Me.txtPUStoneWt_WET.Name = "txtPUStoneWt_WET"
        Me.txtPUStoneWt_WET.Size = New System.Drawing.Size(68, 22)
        Me.txtPUStoneWt_WET.TabIndex = 11
        Me.txtPUStoneWt_WET.Text = "999.999"
        '
        'grpPu
        '
        Me.grpPu.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpPu.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpPu.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpPu.BorderColor = System.Drawing.Color.Transparent
        Me.grpPu.BorderThickness = 1.0!
        Me.grpPu.Controls.Add(Me.txtPUStoneWt_WET)
        Me.grpPu.Controls.Add(Me.Label47)
        Me.grpPu.Controls.Add(Me.txtPuMc_AMT)
        Me.grpPu.Controls.Add(Me.Label46)
        Me.grpPu.Controls.Add(Me.txtPurKaTouch_AMT)
        Me.grpPu.Controls.Add(Me.Label45)
        Me.grpPu.Controls.Add(Me.txtPuKatchaWt_WET)
        Me.grpPu.Controls.Add(Me.Label44)
        Me.grpPu.Controls.Add(Me.txtPuTouch_AMT)
        Me.grpPu.Controls.Add(Me.Label42)
        Me.grpPu.Controls.Add(Me.txtPuPureWt_WET)
        Me.grpPu.Controls.Add(Me.Label43)
        Me.grpPu.Controls.Add(Me.txtPuTotalAmt_AMT)
        Me.grpPu.Controls.Add(Me.Label41)
        Me.grpPu.Controls.Add(Me.txtPURowIndex)
        Me.grpPu.Controls.Add(Me.txtPUCategory)
        Me.grpPu.Controls.Add(Me.gridPur)
        Me.grpPu.Controls.Add(Me.Label11)
        Me.grpPu.Controls.Add(Me.gridPurTotal)
        Me.grpPu.Controls.Add(Me.txtPUPcs_NUM)
        Me.grpPu.Controls.Add(Me.Label20)
        Me.grpPu.Controls.Add(Me.txtPUGrsWt_WET)
        Me.grpPu.Controls.Add(Me.Label14)
        Me.grpPu.Controls.Add(Me.Label16)
        Me.grpPu.Controls.Add(Me.txtPUNetWt_WET)
        Me.grpPu.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpPu.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpPu.GroupImage = Nothing
        Me.grpPu.GroupTitle = ""
        Me.grpPu.Location = New System.Drawing.Point(10, 103)
        Me.grpPu.Name = "grpPu"
        Me.grpPu.Padding = New System.Windows.Forms.Padding(20)
        Me.grpPu.PaintGroupBox = False
        Me.grpPu.RoundCorners = 10
        Me.grpPu.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpPu.ShadowControl = False
        Me.grpPu.ShadowThickness = 3
        Me.grpPu.Size = New System.Drawing.Size(944, 205)
        Me.grpPu.TabIndex = 1
        '
        'Label46
        '
        Me.Label46.BackColor = System.Drawing.Color.Transparent
        Me.Label46.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label46.Location = New System.Drawing.Point(741, 14)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(84, 15)
        Me.Label46.TabIndex = 18
        Me.Label46.Text = "Mc"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPurKaTouch_AMT
        '
        Me.txtPurKaTouch_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPurKaTouch_AMT.Location = New System.Drawing.Point(278, 31)
        Me.txtPurKaTouch_AMT.MaxLength = 10
        Me.txtPurKaTouch_AMT.Name = "txtPurKaTouch_AMT"
        Me.txtPurKaTouch_AMT.Size = New System.Drawing.Size(64, 22)
        Me.txtPurKaTouch_AMT.TabIndex = 5
        Me.txtPurKaTouch_AMT.Text = "999.999"
        '
        'Label45
        '
        Me.Label45.BackColor = System.Drawing.Color.Transparent
        Me.Label45.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.Location = New System.Drawing.Point(278, 14)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(64, 15)
        Me.Label45.TabIndex = 4
        Me.Label45.Text = "KaTouch"
        Me.Label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPuKatchaWt_WET
        '
        Me.txtPuKatchaWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPuKatchaWt_WET.Location = New System.Drawing.Point(209, 31)
        Me.txtPuKatchaWt_WET.MaxLength = 10
        Me.txtPuKatchaWt_WET.Name = "txtPuKatchaWt_WET"
        Me.txtPuKatchaWt_WET.Size = New System.Drawing.Size(68, 22)
        Me.txtPuKatchaWt_WET.TabIndex = 3
        Me.txtPuKatchaWt_WET.Text = "999.999"
        '
        'Label44
        '
        Me.Label44.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.Location = New System.Drawing.Point(209, 12)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(68, 17)
        Me.Label44.TabIndex = 2
        Me.Label44.Text = "Ka Wt"
        Me.Label44.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'WSmithIssRec
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(964, 392)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpPu)
        Me.Controls.Add(Me.grpHeader)
        Me.Controls.Add(Me.Grouper1)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.KeyPreview = True
        Me.Name = "WSmithIssRec"
        Me.ShowIcon = False
        Me.Text = "Smith & Dealer Issue Receipt"
        Me.grpHeader.ResumeLayout(False)
        Me.grpHeader.PerformLayout()
        Me.Grouper1.ResumeLayout(False)
        CType(Me.gridPurTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridPur, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpPu.ResumeLayout(False)
        Me.grpPu.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpHeader As CodeVendor.Controls.Grouper
    Friend WithEvents lblBalance As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents cmbTranType As System.Windows.Forms.ComboBox
    Friend WithEvents cmbParty_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Grouper1 As CodeVendor.Controls.Grouper
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents dtpTranDate As BrighttechPack.DatePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPUNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtPUGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtPUPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents gridPurTotal As System.Windows.Forms.DataGridView
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents gridPur As System.Windows.Forms.DataGridView
    Friend WithEvents txtPUCategory As System.Windows.Forms.TextBox
    Friend WithEvents txtPURowIndex As System.Windows.Forms.TextBox
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents txtPuTotalAmt_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents txtPuPureWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents txtPuTouch_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtPuMc_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents txtPUStoneWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents grpPu As CodeVendor.Controls.Grouper
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents txtPurKaTouch_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents txtPuKatchaWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label44 As System.Windows.Forms.Label
End Class
