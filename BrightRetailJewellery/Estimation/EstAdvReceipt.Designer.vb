<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EstAdvReceipt
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
        Me.grpReceipt = New CodeVendor.Controls.Grouper
        Me.cmbReceiptAccount = New System.Windows.Forms.ComboBox
        Me.Label76 = New System.Windows.Forms.Label
        Me.txtReceiptEmpId_NUM = New System.Windows.Forms.TextBox
        Me.txtReceiptRemark = New System.Windows.Forms.TextBox
        Me.Label58 = New System.Windows.Forms.Label
        Me.txtReceiptEntRefNo = New System.Windows.Forms.TextBox
        Me.txtReceiptEntAmount = New System.Windows.Forms.TextBox
        Me.chkReceiptRateFix = New System.Windows.Forms.CheckBox
        Me.gridReceipt = New System.Windows.Forms.DataGridView
        Me.cmbReceiptTranType = New System.Windows.Forms.ComboBox
        Me.cmbReceiptReceiptType = New System.Windows.Forms.ComboBox
        Me.txtReceiptRate_AMT = New System.Windows.Forms.TextBox
        Me.txtReceiptAmount_AMT = New System.Windows.Forms.TextBox
        Me.txtReceiptRefNo = New System.Windows.Forms.TextBox
        Me.gridReceiptTotal = New System.Windows.Forms.DataGridView
        Me.Label56 = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.Label169 = New System.Windows.Forms.Label
        Me.Label168 = New System.Windows.Forms.Label
        Me.Label191 = New System.Windows.Forms.Label
        Me.Label167 = New System.Windows.Forms.Label
        Me.Label37 = New System.Windows.Forms.Label
        Me.grpRecReservedItem = New CodeVendor.Controls.Grouper
        Me.grpReceiptWeightAdvance = New CodeVendor.Controls.Grouper
        Me.Label176 = New System.Windows.Forms.Label
        Me.txtReceiptBullionRate_RATE = New System.Windows.Forms.TextBox
        Me.txtReceiptWastage_WET = New System.Windows.Forms.TextBox
        Me.txtReceiptNetWt_WET = New System.Windows.Forms.TextBox
        Me.txtReceiptValue_AMT = New System.Windows.Forms.TextBox
        Me.Label172 = New System.Windows.Forms.Label
        Me.Label171 = New System.Windows.Forms.Label
        Me.Label177 = New System.Windows.Forms.Label
        Me.Label170 = New System.Windows.Forms.Label
        Me.Label173 = New System.Windows.Forms.Label
        Me.Label174 = New System.Windows.Forms.Label
        Me.txtReceiptGrsWt_WET = New System.Windows.Forms.TextBox
        Me.txtReceiptPurity_AMT = New System.Windows.Forms.TextBox
        Me.cmbReceiptCategory = New System.Windows.Forms.ComboBox
        Me.txtReceiptValue = New System.Windows.Forms.TextBox
        Me.txtReceiptNetWT = New System.Windows.Forms.TextBox
        Me.txtReceiptPcs = New System.Windows.Forms.TextBox
        Me.txtReceiptGrsWt = New System.Windows.Forms.TextBox
        Me.Label67 = New System.Windows.Forms.Label
        Me.txtReceiptItemName = New System.Windows.Forms.TextBox
        Me.gridReceiptReserved = New System.Windows.Forms.DataGridView
        Me.txtReceiptTagNo = New System.Windows.Forms.TextBox
        Me.txtReceiptItemId_MAN = New System.Windows.Forms.TextBox
        Me.Label73 = New System.Windows.Forms.Label
        Me.Label70 = New System.Windows.Forms.Label
        Me.Label69 = New System.Windows.Forms.Label
        Me.Label68 = New System.Windows.Forms.Label
        Me.Label66 = New System.Windows.Forms.Label
        Me.Label65 = New System.Windows.Forms.Label
        Me.grpReceipt.SuspendLayout()
        CType(Me.gridReceipt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridReceiptTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpRecReservedItem.SuspendLayout()
        Me.grpReceiptWeightAdvance.SuspendLayout()
        CType(Me.gridReceiptReserved, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpReceipt
        '
        Me.grpReceipt.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpReceipt.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpReceipt.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpReceipt.BorderColor = System.Drawing.Color.Transparent
        Me.grpReceipt.BorderThickness = 1.0!
        Me.grpReceipt.Controls.Add(Me.cmbReceiptAccount)
        Me.grpReceipt.Controls.Add(Me.Label76)
        Me.grpReceipt.Controls.Add(Me.txtReceiptEmpId_NUM)
        Me.grpReceipt.Controls.Add(Me.txtReceiptRemark)
        Me.grpReceipt.Controls.Add(Me.Label58)
        Me.grpReceipt.Controls.Add(Me.txtReceiptEntRefNo)
        Me.grpReceipt.Controls.Add(Me.txtReceiptEntAmount)
        Me.grpReceipt.Controls.Add(Me.chkReceiptRateFix)
        Me.grpReceipt.Controls.Add(Me.gridReceipt)
        Me.grpReceipt.Controls.Add(Me.cmbReceiptTranType)
        Me.grpReceipt.Controls.Add(Me.cmbReceiptReceiptType)
        Me.grpReceipt.Controls.Add(Me.txtReceiptRate_AMT)
        Me.grpReceipt.Controls.Add(Me.txtReceiptAmount_AMT)
        Me.grpReceipt.Controls.Add(Me.txtReceiptRefNo)
        Me.grpReceipt.Controls.Add(Me.gridReceiptTotal)
        Me.grpReceipt.Controls.Add(Me.Label56)
        Me.grpReceipt.Controls.Add(Me.Label25)
        Me.grpReceipt.Controls.Add(Me.Label169)
        Me.grpReceipt.Controls.Add(Me.Label168)
        Me.grpReceipt.Controls.Add(Me.Label191)
        Me.grpReceipt.Controls.Add(Me.Label167)
        Me.grpReceipt.Controls.Add(Me.Label37)
        Me.grpReceipt.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpReceipt.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpReceipt.GroupImage = Nothing
        Me.grpReceipt.GroupTitle = ""
        Me.grpReceipt.Location = New System.Drawing.Point(0, 0)
        Me.grpReceipt.Name = "grpReceipt"
        Me.grpReceipt.Padding = New System.Windows.Forms.Padding(20)
        Me.grpReceipt.PaintGroupBox = False
        Me.grpReceipt.RoundCorners = 10
        Me.grpReceipt.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpReceipt.ShadowControl = False
        Me.grpReceipt.ShadowThickness = 3
        Me.grpReceipt.Size = New System.Drawing.Size(984, 254)
        Me.grpReceipt.TabIndex = 1
        '
        'cmbReceiptAccount
        '
        Me.cmbReceiptAccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbReceiptAccount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbReceiptAccount.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbReceiptAccount.FormattingEnabled = True
        Me.cmbReceiptAccount.Location = New System.Drawing.Point(298, 46)
        Me.cmbReceiptAccount.Name = "cmbReceiptAccount"
        Me.cmbReceiptAccount.Size = New System.Drawing.Size(162, 22)
        Me.cmbReceiptAccount.TabIndex = 21
        '
        'Label76
        '
        Me.Label76.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label76.Location = New System.Drawing.Point(313, 30)
        Me.Label76.Name = "Label76"
        Me.Label76.Size = New System.Drawing.Size(126, 14)
        Me.Label76.TabIndex = 18
        Me.Label76.Text = "On Account"
        Me.Label76.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtReceiptEmpId_NUM
        '
        Me.txtReceiptEmpId_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptEmpId_NUM.Location = New System.Drawing.Point(941, 46)
        Me.txtReceiptEmpId_NUM.MaxLength = 50
        Me.txtReceiptEmpId_NUM.Name = "txtReceiptEmpId_NUM"
        Me.txtReceiptEmpId_NUM.Size = New System.Drawing.Size(41, 22)
        Me.txtReceiptEmpId_NUM.TabIndex = 15
        '
        'txtReceiptRemark
        '
        Me.txtReceiptRemark.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptRemark.Location = New System.Drawing.Point(765, 46)
        Me.txtReceiptRemark.MaxLength = 50
        Me.txtReceiptRemark.Name = "txtReceiptRemark"
        Me.txtReceiptRemark.Size = New System.Drawing.Size(175, 22)
        Me.txtReceiptRemark.TabIndex = 13
        '
        'Label58
        '
        Me.Label58.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label58.Location = New System.Drawing.Point(765, 30)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(175, 16)
        Me.Label58.TabIndex = 12
        Me.Label58.Text = "Remark"
        Me.Label58.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtReceiptEntRefNo
        '
        Me.txtReceiptEntRefNo.Location = New System.Drawing.Point(503, 149)
        Me.txtReceiptEntRefNo.Name = "txtReceiptEntRefNo"
        Me.txtReceiptEntRefNo.Size = New System.Drawing.Size(31, 20)
        Me.txtReceiptEntRefNo.TabIndex = 14
        Me.txtReceiptEntRefNo.Visible = False
        '
        'txtReceiptEntAmount
        '
        Me.txtReceiptEntAmount.Location = New System.Drawing.Point(503, 176)
        Me.txtReceiptEntAmount.Name = "txtReceiptEntAmount"
        Me.txtReceiptEntAmount.Size = New System.Drawing.Size(31, 20)
        Me.txtReceiptEntAmount.TabIndex = 15
        Me.txtReceiptEntAmount.Visible = False
        '
        'chkReceiptRateFix
        '
        Me.chkReceiptRateFix.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.chkReceiptRateFix.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReceiptRateFix.Location = New System.Drawing.Point(740, 48)
        Me.chkReceiptRateFix.Name = "chkReceiptRateFix"
        Me.chkReceiptRateFix.Size = New System.Drawing.Size(22, 17)
        Me.chkReceiptRateFix.TabIndex = 11
        Me.chkReceiptRateFix.UseVisualStyleBackColor = True
        '
        'gridReceipt
        '
        Me.gridReceipt.AllowUserToAddRows = False
        Me.gridReceipt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridReceipt.Location = New System.Drawing.Point(10, 69)
        Me.gridReceipt.Name = "gridReceipt"
        Me.gridReceipt.ReadOnly = True
        Me.gridReceipt.Size = New System.Drawing.Size(991, 236)
        Me.gridReceipt.TabIndex = 16
        '
        'cmbReceiptTranType
        '
        Me.cmbReceiptTranType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbReceiptTranType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbReceiptTranType.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbReceiptTranType.FormattingEnabled = True
        Me.cmbReceiptTranType.Location = New System.Drawing.Point(168, 46)
        Me.cmbReceiptTranType.Name = "cmbReceiptTranType"
        Me.cmbReceiptTranType.Size = New System.Drawing.Size(128, 22)
        Me.cmbReceiptTranType.TabIndex = 3
        '
        'cmbReceiptReceiptType
        '
        Me.cmbReceiptReceiptType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReceiptReceiptType.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbReceiptReceiptType.FormattingEnabled = True
        Me.cmbReceiptReceiptType.Location = New System.Drawing.Point(10, 46)
        Me.cmbReceiptReceiptType.Name = "cmbReceiptReceiptType"
        Me.cmbReceiptReceiptType.Size = New System.Drawing.Size(157, 22)
        Me.cmbReceiptReceiptType.TabIndex = 1
        '
        'txtReceiptRate_AMT
        '
        Me.txtReceiptRate_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptRate_AMT.Location = New System.Drawing.Point(658, 46)
        Me.txtReceiptRate_AMT.MaxLength = 10
        Me.txtReceiptRate_AMT.Name = "txtReceiptRate_AMT"
        Me.txtReceiptRate_AMT.Size = New System.Drawing.Size(81, 22)
        Me.txtReceiptRate_AMT.TabIndex = 9
        Me.txtReceiptRate_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtReceiptAmount_AMT
        '
        Me.txtReceiptAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptAmount_AMT.Location = New System.Drawing.Point(548, 46)
        Me.txtReceiptAmount_AMT.MaxLength = 12
        Me.txtReceiptAmount_AMT.Name = "txtReceiptAmount_AMT"
        Me.txtReceiptAmount_AMT.Size = New System.Drawing.Size(109, 22)
        Me.txtReceiptAmount_AMT.TabIndex = 7
        Me.txtReceiptAmount_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtReceiptRefNo
        '
        Me.txtReceiptRefNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptRefNo.Location = New System.Drawing.Point(461, 46)
        Me.txtReceiptRefNo.MaxLength = 10
        Me.txtReceiptRefNo.Name = "txtReceiptRefNo"
        Me.txtReceiptRefNo.Size = New System.Drawing.Size(86, 22)
        Me.txtReceiptRefNo.TabIndex = 5
        Me.txtReceiptRefNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'gridReceiptTotal
        '
        Me.gridReceiptTotal.AllowUserToAddRows = False
        Me.gridReceiptTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridReceiptTotal.Enabled = False
        Me.gridReceiptTotal.Location = New System.Drawing.Point(10, 305)
        Me.gridReceiptTotal.Name = "gridReceiptTotal"
        Me.gridReceiptTotal.ReadOnly = True
        Me.gridReceiptTotal.Size = New System.Drawing.Size(991, 19)
        Me.gridReceiptTotal.TabIndex = 13
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label56.Location = New System.Drawing.Point(732, 13)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(37, 28)
        Me.Label56.TabIndex = 10
        Me.Label56.Text = "Rate" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Fix"
        Me.Label56.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label25
        '
        Me.Label25.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(941, 30)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(41, 14)
        Me.Label25.TabIndex = 14
        Me.Label25.Text = "Emp"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label169
        '
        Me.Label169.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label169.Location = New System.Drawing.Point(658, 30)
        Me.Label169.Name = "Label169"
        Me.Label169.Size = New System.Drawing.Size(81, 14)
        Me.Label169.TabIndex = 8
        Me.Label169.Text = "Rate"
        Me.Label169.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label168
        '
        Me.Label168.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label168.Location = New System.Drawing.Point(548, 30)
        Me.Label168.Name = "Label168"
        Me.Label168.Size = New System.Drawing.Size(109, 14)
        Me.Label168.TabIndex = 6
        Me.Label168.Text = "Amount"
        Me.Label168.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label191
        '
        Me.Label191.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label191.Location = New System.Drawing.Point(168, 30)
        Me.Label191.Name = "Label191"
        Me.Label191.Size = New System.Drawing.Size(127, 14)
        Me.Label191.TabIndex = 2
        Me.Label191.Text = "Tran Type"
        Me.Label191.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label167
        '
        Me.Label167.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label167.Location = New System.Drawing.Point(461, 30)
        Me.Label167.Name = "Label167"
        Me.Label167.Size = New System.Drawing.Size(86, 14)
        Me.Label167.TabIndex = 4
        Me.Label167.Text = "RefNo"
        Me.Label167.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label37
        '
        Me.Label37.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.Location = New System.Drawing.Point(8, 30)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(156, 14)
        Me.Label37.TabIndex = 0
        Me.Label37.Text = "ReceiptType"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpRecReservedItem
        '
        Me.grpRecReservedItem.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpRecReservedItem.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpRecReservedItem.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpRecReservedItem.BorderColor = System.Drawing.Color.Transparent
        Me.grpRecReservedItem.BorderThickness = 1.0!
        Me.grpRecReservedItem.Controls.Add(Me.grpReceiptWeightAdvance)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptValue)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptNetWT)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptPcs)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptGrsWt)
        Me.grpRecReservedItem.Controls.Add(Me.Label67)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptItemName)
        Me.grpRecReservedItem.Controls.Add(Me.gridReceiptReserved)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptTagNo)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptItemId_MAN)
        Me.grpRecReservedItem.Controls.Add(Me.Label73)
        Me.grpRecReservedItem.Controls.Add(Me.Label70)
        Me.grpRecReservedItem.Controls.Add(Me.Label69)
        Me.grpRecReservedItem.Controls.Add(Me.Label68)
        Me.grpRecReservedItem.Controls.Add(Me.Label66)
        Me.grpRecReservedItem.Controls.Add(Me.Label65)
        Me.grpRecReservedItem.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpRecReservedItem.GroupImage = Nothing
        Me.grpRecReservedItem.GroupTitle = ""
        Me.grpRecReservedItem.Location = New System.Drawing.Point(90, 260)
        Me.grpRecReservedItem.Name = "grpRecReservedItem"
        Me.grpRecReservedItem.Padding = New System.Windows.Forms.Padding(20)
        Me.grpRecReservedItem.PaintGroupBox = False
        Me.grpRecReservedItem.RoundCorners = 10
        Me.grpRecReservedItem.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpRecReservedItem.ShadowControl = False
        Me.grpRecReservedItem.ShadowThickness = 3
        Me.grpRecReservedItem.Size = New System.Drawing.Size(576, 285)
        Me.grpRecReservedItem.TabIndex = 30
        Me.grpRecReservedItem.Visible = False
        '
        'grpReceiptWeightAdvance
        '
        Me.grpReceiptWeightAdvance.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpReceiptWeightAdvance.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpReceiptWeightAdvance.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpReceiptWeightAdvance.BorderColor = System.Drawing.Color.Transparent
        Me.grpReceiptWeightAdvance.BorderThickness = 1.0!
        Me.grpReceiptWeightAdvance.Controls.Add(Me.Label176)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.txtReceiptBullionRate_RATE)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.txtReceiptWastage_WET)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.txtReceiptNetWt_WET)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.txtReceiptValue_AMT)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.Label172)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.Label171)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.Label177)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.Label170)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.Label173)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.Label174)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.txtReceiptGrsWt_WET)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.txtReceiptPurity_AMT)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.cmbReceiptCategory)
        Me.grpReceiptWeightAdvance.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpReceiptWeightAdvance.GroupImage = Nothing
        Me.grpReceiptWeightAdvance.GroupTitle = ""
        Me.grpReceiptWeightAdvance.Location = New System.Drawing.Point(317, 70)
        Me.grpReceiptWeightAdvance.Name = "grpReceiptWeightAdvance"
        Me.grpReceiptWeightAdvance.Padding = New System.Windows.Forms.Padding(20)
        Me.grpReceiptWeightAdvance.PaintGroupBox = False
        Me.grpReceiptWeightAdvance.RoundCorners = 10
        Me.grpReceiptWeightAdvance.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpReceiptWeightAdvance.ShadowControl = False
        Me.grpReceiptWeightAdvance.ShadowThickness = 3
        Me.grpReceiptWeightAdvance.Size = New System.Drawing.Size(393, 198)
        Me.grpReceiptWeightAdvance.TabIndex = 32
        Me.grpReceiptWeightAdvance.Visible = False
        '
        'Label176
        '
        Me.Label176.AutoSize = True
        Me.Label176.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label176.Location = New System.Drawing.Point(8, 22)
        Me.Label176.Name = "Label176"
        Me.Label176.Size = New System.Drawing.Size(67, 14)
        Me.Label176.TabIndex = 0
        Me.Label176.Text = "Category"
        Me.Label176.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtReceiptBullionRate_RATE
        '
        Me.txtReceiptBullionRate_RATE.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptBullionRate_RATE.Location = New System.Drawing.Point(250, 118)
        Me.txtReceiptBullionRate_RATE.MaxLength = 10
        Me.txtReceiptBullionRate_RATE.Name = "txtReceiptBullionRate_RATE"
        Me.txtReceiptBullionRate_RATE.Size = New System.Drawing.Size(99, 22)
        Me.txtReceiptBullionRate_RATE.TabIndex = 11
        Me.txtReceiptBullionRate_RATE.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtReceiptWastage_WET
        '
        Me.txtReceiptWastage_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptWastage_WET.Location = New System.Drawing.Point(250, 83)
        Me.txtReceiptWastage_WET.MaxLength = 9
        Me.txtReceiptWastage_WET.Name = "txtReceiptWastage_WET"
        Me.txtReceiptWastage_WET.Size = New System.Drawing.Size(99, 22)
        Me.txtReceiptWastage_WET.TabIndex = 7
        Me.txtReceiptWastage_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtReceiptNetWt_WET
        '
        Me.txtReceiptNetWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptNetWt_WET.Location = New System.Drawing.Point(86, 118)
        Me.txtReceiptNetWt_WET.MaxLength = 10
        Me.txtReceiptNetWt_WET.Name = "txtReceiptNetWt_WET"
        Me.txtReceiptNetWt_WET.Size = New System.Drawing.Size(90, 22)
        Me.txtReceiptNetWt_WET.TabIndex = 9
        Me.txtReceiptNetWt_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtReceiptValue_AMT
        '
        Me.txtReceiptValue_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptValue_AMT.Location = New System.Drawing.Point(86, 148)
        Me.txtReceiptValue_AMT.MaxLength = 12
        Me.txtReceiptValue_AMT.Name = "txtReceiptValue_AMT"
        Me.txtReceiptValue_AMT.Size = New System.Drawing.Size(90, 22)
        Me.txtReceiptValue_AMT.TabIndex = 13
        Me.txtReceiptValue_AMT.Text = "1000000.00"
        Me.txtReceiptValue_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label172
        '
        Me.Label172.AutoSize = True
        Me.Label172.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label172.Location = New System.Drawing.Point(8, 122)
        Me.Label172.Name = "Label172"
        Me.Label172.Size = New System.Drawing.Size(53, 14)
        Me.Label172.TabIndex = 8
        Me.Label172.Text = "Net Wt"
        Me.Label172.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label171
        '
        Me.Label171.AutoSize = True
        Me.Label171.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label171.Location = New System.Drawing.Point(187, 87)
        Me.Label171.Name = "Label171"
        Me.Label171.Size = New System.Drawing.Size(65, 14)
        Me.Label171.TabIndex = 6
        Me.Label171.Text = "Wastage"
        Me.Label171.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label177
        '
        Me.Label177.AutoSize = True
        Me.Label177.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label177.Location = New System.Drawing.Point(8, 53)
        Me.Label177.Name = "Label177"
        Me.Label177.Size = New System.Drawing.Size(47, 14)
        Me.Label177.TabIndex = 2
        Me.Label177.Text = "Purity"
        Me.Label177.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label170
        '
        Me.Label170.AutoSize = True
        Me.Label170.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label170.Location = New System.Drawing.Point(8, 88)
        Me.Label170.Name = "Label170"
        Me.Label170.Size = New System.Drawing.Size(53, 14)
        Me.Label170.TabIndex = 4
        Me.Label170.Text = "Grs Wt"
        Me.Label170.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label173
        '
        Me.Label173.AutoSize = True
        Me.Label173.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label173.Location = New System.Drawing.Point(187, 122)
        Me.Label173.Name = "Label173"
        Me.Label173.Size = New System.Drawing.Size(37, 14)
        Me.Label173.TabIndex = 10
        Me.Label173.Text = "Rate"
        Me.Label173.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label174
        '
        Me.Label174.AutoSize = True
        Me.Label174.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label174.Location = New System.Drawing.Point(8, 152)
        Me.Label174.Name = "Label174"
        Me.Label174.Size = New System.Drawing.Size(44, 14)
        Me.Label174.TabIndex = 12
        Me.Label174.Text = "Value"
        Me.Label174.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtReceiptGrsWt_WET
        '
        Me.txtReceiptGrsWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptGrsWt_WET.Location = New System.Drawing.Point(86, 84)
        Me.txtReceiptGrsWt_WET.MaxLength = 10
        Me.txtReceiptGrsWt_WET.Name = "txtReceiptGrsWt_WET"
        Me.txtReceiptGrsWt_WET.Size = New System.Drawing.Size(90, 22)
        Me.txtReceiptGrsWt_WET.TabIndex = 5
        Me.txtReceiptGrsWt_WET.Text = "100.000"
        Me.txtReceiptGrsWt_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtReceiptPurity_AMT
        '
        Me.txtReceiptPurity_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptPurity_AMT.Location = New System.Drawing.Point(86, 49)
        Me.txtReceiptPurity_AMT.MaxLength = 5
        Me.txtReceiptPurity_AMT.Name = "txtReceiptPurity_AMT"
        Me.txtReceiptPurity_AMT.Size = New System.Drawing.Size(90, 22)
        Me.txtReceiptPurity_AMT.TabIndex = 3
        Me.txtReceiptPurity_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmbReceiptCategory
        '
        Me.cmbReceiptCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReceiptCategory.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbReceiptCategory.FormattingEnabled = True
        Me.cmbReceiptCategory.Location = New System.Drawing.Point(86, 18)
        Me.cmbReceiptCategory.Name = "cmbReceiptCategory"
        Me.cmbReceiptCategory.Size = New System.Drawing.Size(263, 22)
        Me.cmbReceiptCategory.TabIndex = 0
        '
        'txtReceiptValue
        '
        Me.txtReceiptValue.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptValue.Location = New System.Drawing.Point(477, 38)
        Me.txtReceiptValue.Name = "txtReceiptValue"
        Me.txtReceiptValue.Size = New System.Drawing.Size(73, 22)
        Me.txtReceiptValue.TabIndex = 7
        '
        'txtReceiptNetWT
        '
        Me.txtReceiptNetWT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptNetWT.Location = New System.Drawing.Point(403, 38)
        Me.txtReceiptNetWT.Name = "txtReceiptNetWT"
        Me.txtReceiptNetWT.Size = New System.Drawing.Size(73, 22)
        Me.txtReceiptNetWT.TabIndex = 7
        '
        'txtReceiptPcs
        '
        Me.txtReceiptPcs.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptPcs.Location = New System.Drawing.Point(255, 38)
        Me.txtReceiptPcs.Name = "txtReceiptPcs"
        Me.txtReceiptPcs.Size = New System.Drawing.Size(73, 22)
        Me.txtReceiptPcs.TabIndex = 7
        '
        'txtReceiptGrsWt
        '
        Me.txtReceiptGrsWt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptGrsWt.Location = New System.Drawing.Point(329, 38)
        Me.txtReceiptGrsWt.Name = "txtReceiptGrsWt"
        Me.txtReceiptGrsWt.Size = New System.Drawing.Size(73, 22)
        Me.txtReceiptGrsWt.TabIndex = 7
        '
        'Label67
        '
        Me.Label67.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label67.Location = New System.Drawing.Point(60, 19)
        Me.Label67.Name = "Label67"
        Me.Label67.Size = New System.Drawing.Size(112, 16)
        Me.Label67.TabIndex = 2
        Me.Label67.Text = "Item Name"
        Me.Label67.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtReceiptItemName
        '
        Me.txtReceiptItemName.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptItemName.Location = New System.Drawing.Point(60, 38)
        Me.txtReceiptItemName.Name = "txtReceiptItemName"
        Me.txtReceiptItemName.Size = New System.Drawing.Size(112, 22)
        Me.txtReceiptItemName.TabIndex = 3
        '
        'gridReceiptReserved
        '
        Me.gridReceiptReserved.AllowUserToAddRows = False
        Me.gridReceiptReserved.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridReceiptReserved.Location = New System.Drawing.Point(6, 61)
        Me.gridReceiptReserved.Name = "gridReceiptReserved"
        Me.gridReceiptReserved.ReadOnly = True
        Me.gridReceiptReserved.Size = New System.Drawing.Size(563, 217)
        Me.gridReceiptReserved.TabIndex = 6
        '
        'txtReceiptTagNo
        '
        Me.txtReceiptTagNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptTagNo.Location = New System.Drawing.Point(173, 38)
        Me.txtReceiptTagNo.MaxLength = 10
        Me.txtReceiptTagNo.Name = "txtReceiptTagNo"
        Me.txtReceiptTagNo.Size = New System.Drawing.Size(81, 22)
        Me.txtReceiptTagNo.TabIndex = 5
        '
        'txtReceiptItemId_MAN
        '
        Me.txtReceiptItemId_MAN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptItemId_MAN.Location = New System.Drawing.Point(6, 38)
        Me.txtReceiptItemId_MAN.MaxLength = 15
        Me.txtReceiptItemId_MAN.Name = "txtReceiptItemId_MAN"
        Me.txtReceiptItemId_MAN.Size = New System.Drawing.Size(53, 22)
        Me.txtReceiptItemId_MAN.TabIndex = 1
        '
        'Label73
        '
        Me.Label73.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label73.Location = New System.Drawing.Point(477, 19)
        Me.Label73.Name = "Label73"
        Me.Label73.Size = New System.Drawing.Size(73, 16)
        Me.Label73.TabIndex = 4
        Me.Label73.Text = "Value"
        Me.Label73.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label70
        '
        Me.Label70.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label70.Location = New System.Drawing.Point(403, 19)
        Me.Label70.Name = "Label70"
        Me.Label70.Size = New System.Drawing.Size(73, 16)
        Me.Label70.TabIndex = 4
        Me.Label70.Text = "Net Wt"
        Me.Label70.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label69
        '
        Me.Label69.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label69.Location = New System.Drawing.Point(255, 19)
        Me.Label69.Name = "Label69"
        Me.Label69.Size = New System.Drawing.Size(73, 16)
        Me.Label69.TabIndex = 4
        Me.Label69.Text = "Pcs"
        Me.Label69.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label68
        '
        Me.Label68.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label68.Location = New System.Drawing.Point(329, 19)
        Me.Label68.Name = "Label68"
        Me.Label68.Size = New System.Drawing.Size(73, 16)
        Me.Label68.TabIndex = 4
        Me.Label68.Text = "Grs Wt"
        Me.Label68.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label66
        '
        Me.Label66.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label66.Location = New System.Drawing.Point(173, 19)
        Me.Label66.Name = "Label66"
        Me.Label66.Size = New System.Drawing.Size(81, 16)
        Me.Label66.TabIndex = 4
        Me.Label66.Text = "TagNo"
        Me.Label66.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label65
        '
        Me.Label65.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label65.Location = New System.Drawing.Point(6, 19)
        Me.Label65.Name = "Label65"
        Me.Label65.Size = New System.Drawing.Size(53, 16)
        Me.Label65.TabIndex = 0
        Me.Label65.Text = "ItemId"
        Me.Label65.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'EstAdvReceipt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 531)
        Me.Controls.Add(Me.grpRecReservedItem)
        Me.Controls.Add(Me.grpReceipt)
        Me.KeyPreview = True
        Me.Name = "EstAdvReceipt"
        Me.Text = "EstAdvReceipt"
        Me.grpReceipt.ResumeLayout(False)
        Me.grpReceipt.PerformLayout()
        CType(Me.gridReceipt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridReceiptTotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpRecReservedItem.ResumeLayout(False)
        Me.grpRecReservedItem.PerformLayout()
        Me.grpReceiptWeightAdvance.ResumeLayout(False)
        Me.grpReceiptWeightAdvance.PerformLayout()
        CType(Me.gridReceiptReserved, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpReceipt As CodeVendor.Controls.Grouper
    Friend WithEvents cmbReceiptAccount As System.Windows.Forms.ComboBox
    Friend WithEvents Label76 As System.Windows.Forms.Label
    Friend WithEvents txtReceiptEmpId_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptRemark As System.Windows.Forms.TextBox
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents txtReceiptEntRefNo As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptEntAmount As System.Windows.Forms.TextBox
    Friend WithEvents chkReceiptRateFix As System.Windows.Forms.CheckBox
    Friend WithEvents gridReceipt As System.Windows.Forms.DataGridView
    Friend WithEvents cmbReceiptTranType As System.Windows.Forms.ComboBox
    Friend WithEvents cmbReceiptReceiptType As System.Windows.Forms.ComboBox
    Friend WithEvents txtReceiptRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptRefNo As System.Windows.Forms.TextBox
    Friend WithEvents gridReceiptTotal As System.Windows.Forms.DataGridView
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label169 As System.Windows.Forms.Label
    Friend WithEvents Label168 As System.Windows.Forms.Label
    Friend WithEvents Label191 As System.Windows.Forms.Label
    Friend WithEvents Label167 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents grpRecReservedItem As CodeVendor.Controls.Grouper
    Friend WithEvents txtReceiptValue As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptNetWT As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptPcs As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptGrsWt As System.Windows.Forms.TextBox
    Friend WithEvents Label67 As System.Windows.Forms.Label
    Friend WithEvents txtReceiptItemName As System.Windows.Forms.TextBox
    Friend WithEvents gridReceiptReserved As System.Windows.Forms.DataGridView
    Friend WithEvents txtReceiptTagNo As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptItemId_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label73 As System.Windows.Forms.Label
    Friend WithEvents Label70 As System.Windows.Forms.Label
    Friend WithEvents Label69 As System.Windows.Forms.Label
    Friend WithEvents Label68 As System.Windows.Forms.Label
    Friend WithEvents Label66 As System.Windows.Forms.Label
    Friend WithEvents Label65 As System.Windows.Forms.Label
    Friend WithEvents grpReceiptWeightAdvance As CodeVendor.Controls.Grouper
    Friend WithEvents Label176 As System.Windows.Forms.Label
    Friend WithEvents txtReceiptBullionRate_RATE As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptWastage_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptValue_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label172 As System.Windows.Forms.Label
    Friend WithEvents Label171 As System.Windows.Forms.Label
    Friend WithEvents Label177 As System.Windows.Forms.Label
    Friend WithEvents Label170 As System.Windows.Forms.Label
    Friend WithEvents Label173 As System.Windows.Forms.Label
    Friend WithEvents Label174 As System.Windows.Forms.Label
    Friend WithEvents txtReceiptGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptPurity_AMT As System.Windows.Forms.TextBox
    Friend WithEvents cmbReceiptCategory As System.Windows.Forms.ComboBox
End Class
