<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSchOffer
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
        Me.grpCHIT = New CodeVendor.Controls.Grouper()
        Me.txtBonustype = New System.Windows.Forms.TextBox()
        Me.txtGift = New System.Windows.Forms.TextBox()
        Me.txtPrize = New System.Windows.Forms.TextBox()
        Me.txtDed = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtOffPer = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtInsPaid = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtCHITSlipNo = New System.Windows.Forms.TextBox()
        Me.txtBon_AMT = New System.Windows.Forms.TextBox()
        Me.txtCHITCardRegNo_NUM = New System.Windows.Forms.TextBox()
        Me.txtCHITCardGrpCode = New System.Windows.Forms.TextBox()
        Me.txtCHITCardWT_WET = New System.Windows.Forms.TextBox()
        Me.txtCHITCardRate_AMT = New System.Windows.Forms.TextBox()
        Me.txtCHITCardAmount_AMT = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label126 = New System.Windows.Forms.Label()
        Me.txtCHITCardRowIndex = New System.Windows.Forms.TextBox()
        Me.gridCHITCardTotal = New System.Windows.Forms.DataGridView()
        Me.gridCHITCard = New System.Windows.Forms.DataGridView()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.lblAddress = New System.Windows.Forms.Label()
        Me.Adjgrouper = New CodeVendor.Controls.Grouper()
        Me.txtRateDiff = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.dtOffdet = New System.Windows.Forms.DataGridView()
        Me.lblGrsAmt = New System.Windows.Forms.Label()
        Me.lblNetAmt = New System.Windows.Forms.Label()
        Me.lblVat = New System.Windows.Forms.Label()
        Me.lblWastage = New System.Windows.Forms.Label()
        Me.lblVa = New System.Windows.Forms.Label()
        Me.lblParticular = New System.Windows.Forms.Label()
        Me.lblRate = New System.Windows.Forms.Label()
        Me.lblGrswt = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.dgAdjdetails = New System.Windows.Forms.DataGridView()
        Me.grpCHIT.SuspendLayout()
        CType(Me.gridCHITCardTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridCHITCard, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Adjgrouper.SuspendLayout()
        CType(Me.dtOffdet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgAdjdetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpCHIT
        '
        Me.grpCHIT.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpCHIT.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpCHIT.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpCHIT.BorderColor = System.Drawing.Color.Transparent
        Me.grpCHIT.BorderThickness = 1.0!
        Me.grpCHIT.Controls.Add(Me.txtBonustype)
        Me.grpCHIT.Controls.Add(Me.txtGift)
        Me.grpCHIT.Controls.Add(Me.txtPrize)
        Me.grpCHIT.Controls.Add(Me.txtDed)
        Me.grpCHIT.Controls.Add(Me.Label8)
        Me.grpCHIT.Controls.Add(Me.txtOffPer)
        Me.grpCHIT.Controls.Add(Me.Label6)
        Me.grpCHIT.Controls.Add(Me.txtInsPaid)
        Me.grpCHIT.Controls.Add(Me.Label5)
        Me.grpCHIT.Controls.Add(Me.txtCHITSlipNo)
        Me.grpCHIT.Controls.Add(Me.txtBon_AMT)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardRegNo_NUM)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardGrpCode)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardWT_WET)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardRate_AMT)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardAmount_AMT)
        Me.grpCHIT.Controls.Add(Me.Label2)
        Me.grpCHIT.Controls.Add(Me.Label1)
        Me.grpCHIT.Controls.Add(Me.Label4)
        Me.grpCHIT.Controls.Add(Me.Label3)
        Me.grpCHIT.Controls.Add(Me.Label126)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardRowIndex)
        Me.grpCHIT.Controls.Add(Me.gridCHITCardTotal)
        Me.grpCHIT.Controls.Add(Me.gridCHITCard)
        Me.grpCHIT.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpCHIT.GroupImage = Nothing
        Me.grpCHIT.GroupTitle = ""
        Me.grpCHIT.Location = New System.Drawing.Point(4, -5)
        Me.grpCHIT.Name = "grpCHIT"
        Me.grpCHIT.Padding = New System.Windows.Forms.Padding(20)
        Me.grpCHIT.PaintGroupBox = False
        Me.grpCHIT.RoundCorners = 10
        Me.grpCHIT.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpCHIT.ShadowControl = False
        Me.grpCHIT.ShadowThickness = 3
        Me.grpCHIT.Size = New System.Drawing.Size(598, 234)
        Me.grpCHIT.TabIndex = 0
        '
        'txtBonustype
        '
        Me.txtBonustype.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBonustype.Location = New System.Drawing.Point(256, 106)
        Me.txtBonustype.MaxLength = 12
        Me.txtBonustype.Name = "txtBonustype"
        Me.txtBonustype.Size = New System.Drawing.Size(86, 22)
        Me.txtBonustype.TabIndex = 30
        Me.txtBonustype.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtBonustype.Visible = False
        '
        'txtGift
        '
        Me.txtGift.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGift.Location = New System.Drawing.Point(263, -5)
        Me.txtGift.MaxLength = 12
        Me.txtGift.Name = "txtGift"
        Me.txtGift.Size = New System.Drawing.Size(86, 22)
        Me.txtGift.TabIndex = 29
        Me.txtGift.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtGift.Visible = False
        '
        'txtPrize
        '
        Me.txtPrize.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPrize.Location = New System.Drawing.Point(247, 0)
        Me.txtPrize.MaxLength = 12
        Me.txtPrize.Name = "txtPrize"
        Me.txtPrize.Size = New System.Drawing.Size(86, 22)
        Me.txtPrize.TabIndex = 28
        Me.txtPrize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtPrize.Visible = False
        '
        'txtDed
        '
        Me.txtDed.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDed.Location = New System.Drawing.Point(240, 0)
        Me.txtDed.MaxLength = 12
        Me.txtDed.Name = "txtDed"
        Me.txtDed.Size = New System.Drawing.Size(86, 22)
        Me.txtDed.TabIndex = 27
        Me.txtDed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDed.Visible = False
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(531, 19)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(51, 19)
        Me.Label8.TabIndex = 26
        Me.Label8.Text = "Offer"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtOffPer
        '
        Me.txtOffPer.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOffPer.Location = New System.Drawing.Point(530, 39)
        Me.txtOffPer.MaxLength = 12
        Me.txtOffPer.Name = "txtOffPer"
        Me.txtOffPer.Size = New System.Drawing.Size(50, 22)
        Me.txtOffPer.TabIndex = 25
        Me.txtOffPer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(475, 18)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(61, 19)
        Me.Label6.TabIndex = 24
        Me.Label6.Text = "PaidIns."
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtInsPaid
        '
        Me.txtInsPaid.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsPaid.Location = New System.Drawing.Point(478, 39)
        Me.txtInsPaid.MaxLength = 12
        Me.txtInsPaid.Name = "txtInsPaid"
        Me.txtInsPaid.Size = New System.Drawing.Size(50, 22)
        Me.txtInsPaid.TabIndex = 23
        Me.txtInsPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(4, 20)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(68, 17)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Slip No"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtCHITSlipNo
        '
        Me.txtCHITSlipNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITSlipNo.Location = New System.Drawing.Point(8, 39)
        Me.txtCHITSlipNo.MaxLength = 12
        Me.txtCHITSlipNo.Name = "txtCHITSlipNo"
        Me.txtCHITSlipNo.Size = New System.Drawing.Size(68, 22)
        Me.txtCHITSlipNo.TabIndex = 1
        Me.txtCHITSlipNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtBon_AMT
        '
        Me.txtBon_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBon_AMT.Location = New System.Drawing.Point(231, -5)
        Me.txtBon_AMT.MaxLength = 12
        Me.txtBon_AMT.Name = "txtBon_AMT"
        Me.txtBon_AMT.Size = New System.Drawing.Size(86, 22)
        Me.txtBon_AMT.TabIndex = 22
        Me.txtBon_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtBon_AMT.Visible = False
        '
        'txtCHITCardRegNo_NUM
        '
        Me.txtCHITCardRegNo_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITCardRegNo_NUM.Location = New System.Drawing.Point(150, 39)
        Me.txtCHITCardRegNo_NUM.MaxLength = 12
        Me.txtCHITCardRegNo_NUM.Name = "txtCHITCardRegNo_NUM"
        Me.txtCHITCardRegNo_NUM.Size = New System.Drawing.Size(84, 22)
        Me.txtCHITCardRegNo_NUM.TabIndex = 5
        Me.txtCHITCardRegNo_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCHITCardGrpCode
        '
        Me.txtCHITCardGrpCode.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITCardGrpCode.Location = New System.Drawing.Point(78, 39)
        Me.txtCHITCardGrpCode.MaxLength = 12
        Me.txtCHITCardGrpCode.Name = "txtCHITCardGrpCode"
        Me.txtCHITCardGrpCode.Size = New System.Drawing.Size(68, 22)
        Me.txtCHITCardGrpCode.TabIndex = 3
        Me.txtCHITCardGrpCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCHITCardWT_WET
        '
        Me.txtCHITCardWT_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITCardWT_WET.Location = New System.Drawing.Point(404, 39)
        Me.txtCHITCardWT_WET.MaxLength = 12
        Me.txtCHITCardWT_WET.Name = "txtCHITCardWT_WET"
        Me.txtCHITCardWT_WET.Size = New System.Drawing.Size(71, 22)
        Me.txtCHITCardWT_WET.TabIndex = 11
        Me.txtCHITCardWT_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCHITCardRate_AMT
        '
        Me.txtCHITCardRate_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITCardRate_AMT.Location = New System.Drawing.Point(322, 39)
        Me.txtCHITCardRate_AMT.MaxLength = 12
        Me.txtCHITCardRate_AMT.Name = "txtCHITCardRate_AMT"
        Me.txtCHITCardRate_AMT.Size = New System.Drawing.Size(81, 22)
        Me.txtCHITCardRate_AMT.TabIndex = 9
        Me.txtCHITCardRate_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCHITCardAmount_AMT
        '
        Me.txtCHITCardAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITCardAmount_AMT.Location = New System.Drawing.Point(235, 39)
        Me.txtCHITCardAmount_AMT.MaxLength = 12
        Me.txtCHITCardAmount_AMT.Name = "txtCHITCardAmount_AMT"
        Me.txtCHITCardAmount_AMT.Size = New System.Drawing.Size(86, 22)
        Me.txtCHITCardAmount_AMT.TabIndex = 7
        Me.txtCHITCardAmount_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(150, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Reg No"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(78, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 17)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Grp Code"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(406, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(63, 17)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Weight"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(327, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(68, 18)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Rate"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label126
        '
        Me.Label126.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label126.Location = New System.Drawing.Point(235, 20)
        Me.Label126.Name = "Label126"
        Me.Label126.Size = New System.Drawing.Size(86, 17)
        Me.Label126.TabIndex = 6
        Me.Label126.Text = "Amount"
        Me.Label126.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtCHITCardRowIndex
        '
        Me.txtCHITCardRowIndex.Location = New System.Drawing.Point(592, 41)
        Me.txtCHITCardRowIndex.Name = "txtCHITCardRowIndex"
        Me.txtCHITCardRowIndex.Size = New System.Drawing.Size(10, 21)
        Me.txtCHITCardRowIndex.TabIndex = 18
        Me.txtCHITCardRowIndex.Visible = False
        '
        'gridCHITCardTotal
        '
        Me.gridCHITCardTotal.AllowUserToAddRows = False
        Me.gridCHITCardTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridCHITCardTotal.ColumnHeadersVisible = False
        Me.gridCHITCardTotal.Location = New System.Drawing.Point(6, 208)
        Me.gridCHITCardTotal.MultiSelect = False
        Me.gridCHITCardTotal.Name = "gridCHITCardTotal"
        Me.gridCHITCardTotal.ReadOnly = True
        Me.gridCHITCardTotal.RowHeadersVisible = False
        Me.gridCHITCardTotal.RowTemplate.Height = 20
        Me.gridCHITCardTotal.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridCHITCardTotal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridCHITCardTotal.Size = New System.Drawing.Size(430, 19)
        Me.gridCHITCardTotal.TabIndex = 21
        '
        'gridCHITCard
        '
        Me.gridCHITCard.AllowUserToAddRows = False
        Me.gridCHITCard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridCHITCard.ColumnHeadersVisible = False
        Me.gridCHITCard.Location = New System.Drawing.Point(6, 68)
        Me.gridCHITCard.MultiSelect = False
        Me.gridCHITCard.Name = "gridCHITCard"
        Me.gridCHITCard.ReadOnly = True
        Me.gridCHITCard.RowHeadersVisible = False
        Me.gridCHITCard.RowTemplate.Height = 20
        Me.gridCHITCard.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridCHITCard.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridCHITCard.Size = New System.Drawing.Size(580, 118)
        Me.gridCHITCard.TabIndex = 20
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(364, 232)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 7
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(267, 232)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 6
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'lblAddress
        '
        Me.lblAddress.ForeColor = System.Drawing.Color.DarkBlue
        Me.lblAddress.Location = New System.Drawing.Point(1, 232)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.Size = New System.Drawing.Size(241, 51)
        Me.lblAddress.TabIndex = 25
        Me.lblAddress.Text = "Label8" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "4" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "5" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "6"
        '
        'Adjgrouper
        '
        Me.Adjgrouper.BackgroundColor = System.Drawing.Color.Lavender
        Me.Adjgrouper.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Adjgrouper.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Adjgrouper.BorderColor = System.Drawing.Color.Transparent
        Me.Adjgrouper.BorderThickness = 1.0!
        Me.Adjgrouper.Controls.Add(Me.txtRateDiff)
        Me.Adjgrouper.Controls.Add(Me.Label11)
        Me.Adjgrouper.Controls.Add(Me.Label7)
        Me.Adjgrouper.Controls.Add(Me.Label9)
        Me.Adjgrouper.Controls.Add(Me.Label10)
        Me.Adjgrouper.Controls.Add(Me.dtOffdet)
        Me.Adjgrouper.Controls.Add(Me.lblGrsAmt)
        Me.Adjgrouper.Controls.Add(Me.lblNetAmt)
        Me.Adjgrouper.Controls.Add(Me.lblVat)
        Me.Adjgrouper.Controls.Add(Me.lblWastage)
        Me.Adjgrouper.Controls.Add(Me.lblVa)
        Me.Adjgrouper.Controls.Add(Me.lblParticular)
        Me.Adjgrouper.Controls.Add(Me.lblRate)
        Me.Adjgrouper.Controls.Add(Me.lblGrswt)
        Me.Adjgrouper.Controls.Add(Me.DataGridView1)
        Me.Adjgrouper.Controls.Add(Me.dgAdjdetails)
        Me.Adjgrouper.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Adjgrouper.GroupImage = Nothing
        Me.Adjgrouper.GroupTitle = ""
        Me.Adjgrouper.Location = New System.Drawing.Point(4, -3)
        Me.Adjgrouper.Name = "Adjgrouper"
        Me.Adjgrouper.Padding = New System.Windows.Forms.Padding(20)
        Me.Adjgrouper.PaintGroupBox = False
        Me.Adjgrouper.RoundCorners = 10
        Me.Adjgrouper.ShadowColor = System.Drawing.Color.DarkGray
        Me.Adjgrouper.ShadowControl = False
        Me.Adjgrouper.ShadowThickness = 3
        Me.Adjgrouper.Size = New System.Drawing.Size(892, 265)
        Me.Adjgrouper.TabIndex = 28
        Me.Adjgrouper.Visible = False
        '
        'txtRateDiff
        '
        Me.txtRateDiff.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRateDiff.Location = New System.Drawing.Point(613, -17)
        Me.txtRateDiff.MaxLength = 12
        Me.txtRateDiff.Name = "txtRateDiff"
        Me.txtRateDiff.Size = New System.Drawing.Size(86, 22)
        Me.txtRateDiff.TabIndex = 30
        Me.txtRateDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtRateDiff.Visible = False
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(667, 12)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(202, 19)
        Me.Label11.TabIndex = 31
        Me.Label11.Text = "Itemwise Offer Table "
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Green
        Me.Label7.Location = New System.Drawing.Point(802, 31)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(88, 33)
        Me.Label7.TabIndex = 30
        Me.Label7.Text = "Slab (W%-MC%)"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label7.Visible = False
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Green
        Me.Label9.Location = New System.Drawing.Point(625, 40)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(112, 17)
        Me.Label9.TabIndex = 28
        Me.Label9.Text = "Item /Sub Item Id"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label9.Visible = False
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Green
        Me.Label10.Location = New System.Drawing.Point(731, 38)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(67, 18)
        Me.Label10.TabIndex = 29
        Me.Label10.Text = "Off. Wt"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label10.Visible = False
        '
        'dtOffdet
        '
        Me.dtOffdet.AllowUserToAddRows = False
        Me.dtOffdet.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dtOffdet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dtOffdet.ColumnHeadersVisible = False
        Me.dtOffdet.Location = New System.Drawing.Point(631, 37)
        Me.dtOffdet.MultiSelect = False
        Me.dtOffdet.Name = "dtOffdet"
        Me.dtOffdet.ReadOnly = True
        Me.dtOffdet.RowHeadersVisible = False
        Me.dtOffdet.RowTemplate.Height = 20
        Me.dtOffdet.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dtOffdet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dtOffdet.Size = New System.Drawing.Size(259, 195)
        Me.dtOffdet.TabIndex = 27
        '
        'lblGrsAmt
        '
        Me.lblGrsAmt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrsAmt.Location = New System.Drawing.Point(386, 20)
        Me.lblGrsAmt.Name = "lblGrsAmt"
        Me.lblGrsAmt.Size = New System.Drawing.Size(84, 17)
        Me.lblGrsAmt.TabIndex = 26
        Me.lblGrsAmt.Text = "Grs Amount"
        Me.lblGrsAmt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblNetAmt
        '
        Me.lblNetAmt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNetAmt.Location = New System.Drawing.Point(531, 21)
        Me.lblNetAmt.Name = "lblNetAmt"
        Me.lblNetAmt.Size = New System.Drawing.Size(94, 17)
        Me.lblNetAmt.TabIndex = 25
        Me.lblNetAmt.Text = "Net Amount"
        Me.lblNetAmt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblVat
        '
        Me.lblVat.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVat.Location = New System.Drawing.Point(476, 20)
        Me.lblVat.Name = "lblVat"
        Me.lblVat.Size = New System.Drawing.Size(50, 18)
        Me.lblVat.TabIndex = 24
        Me.lblVat.Text = "VAT"
        Me.lblVat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblWastage
        '
        Me.lblWastage.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWastage.Location = New System.Drawing.Point(244, 19)
        Me.lblWastage.Name = "lblWastage"
        Me.lblWastage.Size = New System.Drawing.Size(78, 19)
        Me.lblWastage.TabIndex = 23
        Me.lblWastage.Text = "Wastage"
        Me.lblWastage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblVa
        '
        Me.lblVa.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVa.Location = New System.Drawing.Point(188, 20)
        Me.lblVa.Name = "lblVa"
        Me.lblVa.Size = New System.Drawing.Size(58, 17)
        Me.lblVa.TabIndex = 22
        Me.lblVa.Text = "VA %"
        Me.lblVa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblParticular
        '
        Me.lblParticular.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParticular.Location = New System.Drawing.Point(8, 21)
        Me.lblParticular.Name = "lblParticular"
        Me.lblParticular.Size = New System.Drawing.Size(104, 17)
        Me.lblParticular.TabIndex = 2
        Me.lblParticular.Text = "Particulars"
        Me.lblParticular.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblRate
        '
        Me.lblRate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRate.Location = New System.Drawing.Point(326, 20)
        Me.lblRate.Name = "lblRate"
        Me.lblRate.Size = New System.Drawing.Size(55, 17)
        Me.lblRate.TabIndex = 8
        Me.lblRate.Text = "Rate"
        Me.lblRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblGrswt
        '
        Me.lblGrswt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrswt.Location = New System.Drawing.Point(115, 20)
        Me.lblGrswt.Name = "lblGrswt"
        Me.lblGrswt.Size = New System.Drawing.Size(67, 18)
        Me.lblGrswt.TabIndex = 6
        Me.lblGrswt.Text = "Grs Wt"
        Me.lblGrswt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.ColumnHeadersVisible = False
        Me.DataGridView1.Location = New System.Drawing.Point(6, 216)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.RowTemplate.Height = 20
        Me.DataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(619, 19)
        Me.DataGridView1.TabIndex = 21
        Me.DataGridView1.Visible = False
        '
        'dgAdjdetails
        '
        Me.dgAdjdetails.AllowUserToAddRows = False
        Me.dgAdjdetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgAdjdetails.ColumnHeadersVisible = False
        Me.dgAdjdetails.Location = New System.Drawing.Point(6, 41)
        Me.dgAdjdetails.MultiSelect = False
        Me.dgAdjdetails.Name = "dgAdjdetails"
        Me.dgAdjdetails.ReadOnly = True
        Me.dgAdjdetails.RowHeadersVisible = False
        Me.dgAdjdetails.RowTemplate.Height = 20
        Me.dgAdjdetails.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgAdjdetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgAdjdetails.Size = New System.Drawing.Size(619, 173)
        Me.dgAdjdetails.TabIndex = 20
        '
        'frmSchOffer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(899, 265)
        Me.ControlBox = False
        Me.Controls.Add(Me.Adjgrouper)
        Me.Controls.Add(Me.lblAddress)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grpCHIT)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmSchOffer"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Scheme Adjustment"
        Me.grpCHIT.ResumeLayout(False)
        Me.grpCHIT.PerformLayout()
        CType(Me.gridCHITCardTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridCHITCard, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Adjgrouper.ResumeLayout(False)
        Me.Adjgrouper.PerformLayout()
        CType(Me.dtOffdet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgAdjdetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpCHIT As CodeVendor.Controls.Grouper
    Friend WithEvents txtCHITCardRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents gridCHITCardTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridCHITCard As System.Windows.Forms.DataGridView
    Friend WithEvents txtCHITCardAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label126 As System.Windows.Forms.Label
    Friend WithEvents txtCHITCardRegNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtCHITCardGrpCode As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCHITCardWT_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtCHITCardRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents lblAddress As System.Windows.Forms.Label
    Friend WithEvents Adjgrouper As CodeVendor.Controls.Grouper
    Friend WithEvents lblGrsAmt As System.Windows.Forms.Label
    Friend WithEvents lblNetAmt As System.Windows.Forms.Label
    Friend WithEvents lblVat As System.Windows.Forms.Label
    Friend WithEvents lblWastage As System.Windows.Forms.Label
    Friend WithEvents lblVa As System.Windows.Forms.Label
    Friend WithEvents lblParticular As System.Windows.Forms.Label
    Friend WithEvents lblRate As System.Windows.Forms.Label
    Friend WithEvents lblGrswt As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents dgAdjdetails As System.Windows.Forms.DataGridView
    Friend WithEvents txtBon_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtCHITSlipNo As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtInsPaid As System.Windows.Forms.TextBox
    Friend WithEvents txtOffPer As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtGift As System.Windows.Forms.TextBox
    Friend WithEvents txtPrize As System.Windows.Forms.TextBox
    Friend WithEvents txtDed As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents dtOffdet As System.Windows.Forms.DataGridView
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtRateDiff As System.Windows.Forms.TextBox
    Friend WithEvents txtBonustype As System.Windows.Forms.TextBox
End Class
