<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TagMerge
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
        Me.btnExit = New System.Windows.Forms.Button()
        Me.PnlMark = New System.Windows.Forms.Panel()
        Me.rbtGenerateNewTag = New System.Windows.Forms.RadioButton()
        Me.rbtInterchange = New System.Windows.Forms.RadioButton()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.grpTagDetail = New CodeVendor.Controls.Grouper()
        Me.txtTagOldNetWt_WET = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtTagOldAmount_AMT = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtTagOldGrsWt_WET = New System.Windows.Forms.TextBox()
        Me.txttAGDiaWt_WET = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txttAGDiaAmount_AMT = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txttAGStoneWt_WET = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtTagRate_AMT = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtTAGRowIndex = New System.Windows.Forms.TextBox()
        Me.txtTagAmount_AMT = New System.Windows.Forms.TextBox()
        Me.gridTAGTotal = New System.Windows.Forms.DataGridView()
        Me.gridTAG = New System.Windows.Forms.DataGridView()
        Me.txttAGStoneAmount_AMT = New System.Windows.Forms.TextBox()
        Me.txtTagTagNo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtTagPcs_NUM = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtTagGrsWt_WET = New System.Windows.Forms.TextBox()
        Me.txtTagNetWt_WET = New System.Windows.Forms.TextBox()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtTagItemId = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblHelpText = New System.Windows.Forms.Label()
        Me.TabControlstone = New System.Windows.Forms.TabControl()
        Me.tabStone = New System.Windows.Forms.TabPage()
        Me.grpStoneDetails = New System.Windows.Forms.GroupBox()
        Me.pnlStoneGrid = New System.Windows.Forms.Panel()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtStOldAmount_Amt = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtStOldWeight_WET = New System.Windows.Forms.TextBox()
        Me.txtStRowIndex = New System.Windows.Forms.TextBox()
        Me.gridTagStonetotal = New System.Windows.Forms.DataGridView()
        Me.txtStTagno = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label61 = New System.Windows.Forms.Label()
        Me.txtStItem = New System.Windows.Forms.TextBox()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.Label59 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.gridTagStone = New System.Windows.Forms.DataGridView()
        Me.txtStPcs_NUM = New System.Windows.Forms.TextBox()
        Me.txtStWeight_WET = New System.Windows.Forms.TextBox()
        Me.txtStRate_Amt = New System.Windows.Forms.TextBox()
        Me.txtStAmount_Amt = New System.Windows.Forms.TextBox()
        Me.txtStMetalCode = New System.Windows.Forms.TextBox()
        Me.chkBarcode = New System.Windows.Forms.CheckBox()
        Me.PnlMark.SuspendLayout()
        Me.grpTagDetail.SuspendLayout()
        CType(Me.gridTAGTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridTAG, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControlstone.SuspendLayout()
        Me.tabStone.SuspendLayout()
        Me.grpStoneDetails.SuspendLayout()
        Me.pnlStoneGrid.SuspendLayout()
        CType(Me.gridTagStonetotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridTagStone, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(922, 63)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(82, 30)
        Me.btnExit.TabIndex = 5
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'PnlMark
        '
        Me.PnlMark.BackColor = System.Drawing.Color.Lavender
        Me.PnlMark.Controls.Add(Me.rbtGenerateNewTag)
        Me.PnlMark.Controls.Add(Me.rbtInterchange)
        Me.PnlMark.Location = New System.Drawing.Point(729, 23)
        Me.PnlMark.Name = "PnlMark"
        Me.PnlMark.Size = New System.Drawing.Size(274, 29)
        Me.PnlMark.TabIndex = 1
        '
        'rbtGenerateNewTag
        '
        Me.rbtGenerateNewTag.AutoSize = True
        Me.rbtGenerateNewTag.Checked = True
        Me.rbtGenerateNewTag.Location = New System.Drawing.Point(11, 3)
        Me.rbtGenerateNewTag.Name = "rbtGenerateNewTag"
        Me.rbtGenerateNewTag.Size = New System.Drawing.Size(130, 17)
        Me.rbtGenerateNewTag.TabIndex = 0
        Me.rbtGenerateNewTag.TabStop = True
        Me.rbtGenerateNewTag.Text = "Generate New Tag"
        Me.rbtGenerateNewTag.UseVisualStyleBackColor = True
        '
        'rbtInterchange
        '
        Me.rbtInterchange.AutoSize = True
        Me.rbtInterchange.Location = New System.Drawing.Point(155, 3)
        Me.rbtInterchange.Name = "rbtInterchange"
        Me.rbtInterchange.Size = New System.Drawing.Size(94, 17)
        Me.rbtInterchange.TabIndex = 1
        Me.rbtInterchange.Text = "Interchange"
        Me.rbtInterchange.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(729, 63)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 4
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.Location = New System.Drawing.Point(825, 63)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(90, 30)
        Me.btnNew.TabIndex = 3
        Me.btnNew.Text = "&New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'grpTagDetail
        '
        Me.grpTagDetail.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpTagDetail.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpTagDetail.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpTagDetail.BorderColor = System.Drawing.Color.Transparent
        Me.grpTagDetail.BorderThickness = 1.0!
        Me.grpTagDetail.Controls.Add(Me.txtTagOldNetWt_WET)
        Me.grpTagDetail.Controls.Add(Me.Label18)
        Me.grpTagDetail.Controls.Add(Me.txtTagOldAmount_AMT)
        Me.grpTagDetail.Controls.Add(Me.Label16)
        Me.grpTagDetail.Controls.Add(Me.Label15)
        Me.grpTagDetail.Controls.Add(Me.txtTagOldGrsWt_WET)
        Me.grpTagDetail.Controls.Add(Me.txttAGDiaWt_WET)
        Me.grpTagDetail.Controls.Add(Me.Label9)
        Me.grpTagDetail.Controls.Add(Me.txttAGDiaAmount_AMT)
        Me.grpTagDetail.Controls.Add(Me.Label13)
        Me.grpTagDetail.Controls.Add(Me.txttAGStoneWt_WET)
        Me.grpTagDetail.Controls.Add(Me.Label8)
        Me.grpTagDetail.Controls.Add(Me.txtTagRate_AMT)
        Me.grpTagDetail.Controls.Add(Me.Label7)
        Me.grpTagDetail.Controls.Add(Me.txtTAGRowIndex)
        Me.grpTagDetail.Controls.Add(Me.txtTagAmount_AMT)
        Me.grpTagDetail.Controls.Add(Me.gridTAGTotal)
        Me.grpTagDetail.Controls.Add(Me.gridTAG)
        Me.grpTagDetail.Controls.Add(Me.txttAGStoneAmount_AMT)
        Me.grpTagDetail.Controls.Add(Me.txtTagTagNo)
        Me.grpTagDetail.Controls.Add(Me.Label3)
        Me.grpTagDetail.Controls.Add(Me.Label6)
        Me.grpTagDetail.Controls.Add(Me.txtTagPcs_NUM)
        Me.grpTagDetail.Controls.Add(Me.Label2)
        Me.grpTagDetail.Controls.Add(Me.Label4)
        Me.grpTagDetail.Controls.Add(Me.txtTagGrsWt_WET)
        Me.grpTagDetail.Controls.Add(Me.txtTagNetWt_WET)
        Me.grpTagDetail.Controls.Add(Me.Label55)
        Me.grpTagDetail.Controls.Add(Me.Label10)
        Me.grpTagDetail.Controls.Add(Me.Label5)
        Me.grpTagDetail.Controls.Add(Me.txtTagItemId)
        Me.grpTagDetail.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpTagDetail.Dock = System.Windows.Forms.DockStyle.Top
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
        Me.grpTagDetail.Size = New System.Drawing.Size(1020, 397)
        Me.grpTagDetail.TabIndex = 0
        '
        'txtTagOldNetWt_WET
        '
        Me.txtTagOldNetWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTagOldNetWt_WET.Location = New System.Drawing.Point(339, 31)
        Me.txtTagOldNetWt_WET.MaxLength = 10
        Me.txtTagOldNetWt_WET.Name = "txtTagOldNetWt_WET"
        Me.txtTagOldNetWt_WET.ReadOnly = True
        Me.txtTagOldNetWt_WET.ShortcutsEnabled = False
        Me.txtTagOldNetWt_WET.Size = New System.Drawing.Size(75, 22)
        Me.txtTagOldNetWt_WET.TabIndex = 31
        '
        'Label18
        '
        Me.Label18.BackColor = System.Drawing.Color.Transparent
        Me.Label18.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(336, 14)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(85, 15)
        Me.Label18.TabIndex = 30
        Me.Label18.Text = "OLD NETWT"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTagOldAmount_AMT
        '
        Me.txtTagOldAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTagOldAmount_AMT.Location = New System.Drawing.Point(851, 31)
        Me.txtTagOldAmount_AMT.MaxLength = 12
        Me.txtTagOldAmount_AMT.Name = "txtTagOldAmount_AMT"
        Me.txtTagOldAmount_AMT.ReadOnly = True
        Me.txtTagOldAmount_AMT.ShortcutsEnabled = False
        Me.txtTagOldAmount_AMT.Size = New System.Drawing.Size(80, 22)
        Me.txtTagOldAmount_AMT.TabIndex = 29
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.Transparent
        Me.Label16.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(851, 14)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(80, 15)
        Me.Label16.TabIndex = 28
        Me.Label16.Text = "OLD AMT"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.Color.Transparent
        Me.Label15.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(180, 14)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(85, 15)
        Me.Label15.TabIndex = 26
        Me.Label15.Text = "OLD GRSWT"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTagOldGrsWt_WET
        '
        Me.txtTagOldGrsWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTagOldGrsWt_WET.Location = New System.Drawing.Point(182, 31)
        Me.txtTagOldGrsWt_WET.MaxLength = 10
        Me.txtTagOldGrsWt_WET.Name = "txtTagOldGrsWt_WET"
        Me.txtTagOldGrsWt_WET.ReadOnly = True
        Me.txtTagOldGrsWt_WET.ShortcutsEnabled = False
        Me.txtTagOldGrsWt_WET.Size = New System.Drawing.Size(75, 22)
        Me.txtTagOldGrsWt_WET.TabIndex = 27
        '
        'txttAGDiaWt_WET
        '
        Me.txttAGDiaWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txttAGDiaWt_WET.Location = New System.Drawing.Point(714, 31)
        Me.txttAGDiaWt_WET.Name = "txttAGDiaWt_WET"
        Me.txttAGDiaWt_WET.ShortcutsEnabled = False
        Me.txttAGDiaWt_WET.Size = New System.Drawing.Size(65, 22)
        Me.txttAGDiaWt_WET.TabIndex = 17
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(714, 14)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(65, 15)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "DIA WT"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txttAGDiaAmount_AMT
        '
        Me.txttAGDiaAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txttAGDiaAmount_AMT.Location = New System.Drawing.Point(780, 31)
        Me.txttAGDiaAmount_AMT.Name = "txttAGDiaAmount_AMT"
        Me.txttAGDiaAmount_AMT.ShortcutsEnabled = False
        Me.txttAGDiaAmount_AMT.Size = New System.Drawing.Size(70, 22)
        Me.txttAGDiaAmount_AMT.TabIndex = 19
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(779, 14)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(70, 15)
        Me.Label13.TabIndex = 18
        Me.Label13.Text = "DIA AMT"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txttAGStoneWt_WET
        '
        Me.txttAGStoneWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txttAGStoneWt_WET.Location = New System.Drawing.Point(577, 31)
        Me.txttAGStoneWt_WET.Name = "txttAGStoneWt_WET"
        Me.txttAGStoneWt_WET.ShortcutsEnabled = False
        Me.txttAGStoneWt_WET.Size = New System.Drawing.Size(65, 22)
        Me.txttAGStoneWt_WET.TabIndex = 13
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(578, 14)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(65, 15)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "STN WT"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTagRate_AMT
        '
        Me.txtTagRate_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTagRate_AMT.Location = New System.Drawing.Point(496, 31)
        Me.txtTagRate_AMT.MaxLength = 10
        Me.txtTagRate_AMT.Name = "txtTagRate_AMT"
        Me.txtTagRate_AMT.ShortcutsEnabled = False
        Me.txtTagRate_AMT.Size = New System.Drawing.Size(80, 22)
        Me.txtTagRate_AMT.TabIndex = 11
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(495, 14)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(80, 15)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "RATE"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTAGRowIndex
        '
        Me.txtTAGRowIndex.Location = New System.Drawing.Point(1006, 12)
        Me.txtTAGRowIndex.Name = "txtTAGRowIndex"
        Me.txtTAGRowIndex.Size = New System.Drawing.Size(10, 21)
        Me.txtTAGRowIndex.TabIndex = 22
        Me.txtTAGRowIndex.Visible = False
        '
        'txtTagAmount_AMT
        '
        Me.txtTagAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTagAmount_AMT.Location = New System.Drawing.Point(933, 31)
        Me.txtTagAmount_AMT.MaxLength = 12
        Me.txtTagAmount_AMT.Name = "txtTagAmount_AMT"
        Me.txtTagAmount_AMT.ShortcutsEnabled = False
        Me.txtTagAmount_AMT.Size = New System.Drawing.Size(80, 22)
        Me.txtTagAmount_AMT.TabIndex = 21
        '
        'gridTAGTotal
        '
        Me.gridTAGTotal.AllowUserToAddRows = False
        Me.gridTAGTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridTAGTotal.Enabled = False
        Me.gridTAGTotal.Location = New System.Drawing.Point(2, 363)
        Me.gridTAGTotal.Name = "gridTAGTotal"
        Me.gridTAGTotal.ReadOnly = True
        Me.gridTAGTotal.Size = New System.Drawing.Size(1014, 30)
        Me.gridTAGTotal.TabIndex = 25
        '
        'gridTAG
        '
        Me.gridTAG.AllowUserToAddRows = False
        Me.gridTAG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridTAG.Location = New System.Drawing.Point(3, 59)
        Me.gridTAG.Name = "gridTAG"
        Me.gridTAG.ReadOnly = True
        Me.gridTAG.Size = New System.Drawing.Size(1013, 302)
        Me.gridTAG.TabIndex = 24
        '
        'txttAGStoneAmount_AMT
        '
        Me.txttAGStoneAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txttAGStoneAmount_AMT.Location = New System.Drawing.Point(643, 31)
        Me.txttAGStoneAmount_AMT.Name = "txttAGStoneAmount_AMT"
        Me.txttAGStoneAmount_AMT.ShortcutsEnabled = False
        Me.txttAGStoneAmount_AMT.Size = New System.Drawing.Size(70, 22)
        Me.txttAGStoneAmount_AMT.TabIndex = 15
        '
        'txtTagTagNo
        '
        Me.txtTagTagNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTagTagNo.Location = New System.Drawing.Point(60, 31)
        Me.txtTagTagNo.MaxLength = 10
        Me.txtTagTagNo.Name = "txtTagTagNo"
        Me.txtTagTagNo.ShortcutsEnabled = False
        Me.txtTagTagNo.Size = New System.Drawing.Size(75, 22)
        Me.txtTagTagNo.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(60, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "TAGNO"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(263, 14)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(70, 15)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "GRSWT"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTagPcs_NUM
        '
        Me.txtTagPcs_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTagPcs_NUM.Location = New System.Drawing.Point(136, 31)
        Me.txtTagPcs_NUM.MaxLength = 5
        Me.txtTagPcs_NUM.Name = "txtTagPcs_NUM"
        Me.txtTagPcs_NUM.ShortcutsEnabled = False
        Me.txtTagPcs_NUM.Size = New System.Drawing.Size(45, 22)
        Me.txtTagPcs_NUM.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(4, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 15)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "ITEMID"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(136, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(45, 15)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "PCS"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTagGrsWt_WET
        '
        Me.txtTagGrsWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTagGrsWt_WET.Location = New System.Drawing.Point(258, 31)
        Me.txtTagGrsWt_WET.MaxLength = 10
        Me.txtTagGrsWt_WET.Name = "txtTagGrsWt_WET"
        Me.txtTagGrsWt_WET.ShortcutsEnabled = False
        Me.txtTagGrsWt_WET.Size = New System.Drawing.Size(80, 22)
        Me.txtTagGrsWt_WET.TabIndex = 7
        '
        'txtTagNetWt_WET
        '
        Me.txtTagNetWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTagNetWt_WET.Location = New System.Drawing.Point(415, 31)
        Me.txtTagNetWt_WET.MaxLength = 10
        Me.txtTagNetWt_WET.Name = "txtTagNetWt_WET"
        Me.txtTagNetWt_WET.ShortcutsEnabled = False
        Me.txtTagNetWt_WET.Size = New System.Drawing.Size(80, 22)
        Me.txtTagNetWt_WET.TabIndex = 9
        '
        'Label55
        '
        Me.Label55.BackColor = System.Drawing.Color.Transparent
        Me.Label55.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label55.Location = New System.Drawing.Point(932, 14)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(80, 15)
        Me.Label55.TabIndex = 20
        Me.Label55.Text = "AMOUNT"
        Me.Label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(642, 14)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(70, 15)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "STN AMT"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(417, 14)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(75, 15)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "NETWT"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTagItemId
        '
        Me.txtTagItemId.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTagItemId.Location = New System.Drawing.Point(4, 31)
        Me.txtTagItemId.MaxLength = 15
        Me.txtTagItemId.Name = "txtTagItemId"
        Me.txtTagItemId.ShortcutsEnabled = False
        Me.txtTagItemId.Size = New System.Drawing.Size(55, 22)
        Me.txtTagItemId.TabIndex = 1
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 2000
        Me.ToolTip1.ReshowDelay = 100
        '
        'lblHelpText
        '
        Me.lblHelpText.BackColor = System.Drawing.SystemColors.Control
        Me.lblHelpText.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHelpText.ForeColor = System.Drawing.Color.Red
        Me.lblHelpText.Location = New System.Drawing.Point(538, 515)
        Me.lblHelpText.Name = "lblHelpText"
        Me.lblHelpText.Size = New System.Drawing.Size(524, 13)
        Me.lblHelpText.TabIndex = 3
        Me.lblHelpText.Text = "Help"
        Me.lblHelpText.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TabControlstone
        '
        Me.TabControlstone.Controls.Add(Me.tabStone)
        Me.TabControlstone.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TabControlstone.ItemSize = New System.Drawing.Size(156, 18)
        Me.TabControlstone.Location = New System.Drawing.Point(0, 403)
        Me.TabControlstone.Multiline = True
        Me.TabControlstone.Name = "TabControlstone"
        Me.TabControlstone.SelectedIndex = 0
        Me.TabControlstone.Size = New System.Drawing.Size(1020, 227)
        Me.TabControlstone.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.TabControlstone.TabIndex = 1
        '
        'tabStone
        '
        Me.tabStone.BackColor = System.Drawing.Color.Lavender
        Me.tabStone.Controls.Add(Me.chkBarcode)
        Me.tabStone.Controls.Add(Me.btnExit)
        Me.tabStone.Controls.Add(Me.grpStoneDetails)
        Me.tabStone.Controls.Add(Me.btnSave)
        Me.tabStone.Controls.Add(Me.PnlMark)
        Me.tabStone.Controls.Add(Me.btnNew)
        Me.tabStone.Location = New System.Drawing.Point(4, 22)
        Me.tabStone.Name = "tabStone"
        Me.tabStone.Padding = New System.Windows.Forms.Padding(3)
        Me.tabStone.Size = New System.Drawing.Size(1012, 201)
        Me.tabStone.TabIndex = 1
        Me.tabStone.Text = "Stone"
        '
        'grpStoneDetails
        '
        Me.grpStoneDetails.Controls.Add(Me.pnlStoneGrid)
        Me.grpStoneDetails.Location = New System.Drawing.Point(4, -1)
        Me.grpStoneDetails.Name = "grpStoneDetails"
        Me.grpStoneDetails.Size = New System.Drawing.Size(719, 198)
        Me.grpStoneDetails.TabIndex = 0
        Me.grpStoneDetails.TabStop = False
        '
        'pnlStoneGrid
        '
        Me.pnlStoneGrid.Controls.Add(Me.Label21)
        Me.pnlStoneGrid.Controls.Add(Me.Label20)
        Me.pnlStoneGrid.Controls.Add(Me.txtStOldAmount_Amt)
        Me.pnlStoneGrid.Controls.Add(Me.Label17)
        Me.pnlStoneGrid.Controls.Add(Me.txtStOldWeight_WET)
        Me.pnlStoneGrid.Controls.Add(Me.txtStRowIndex)
        Me.pnlStoneGrid.Controls.Add(Me.gridTagStonetotal)
        Me.pnlStoneGrid.Controls.Add(Me.txtStTagno)
        Me.pnlStoneGrid.Controls.Add(Me.Label12)
        Me.pnlStoneGrid.Controls.Add(Me.Label61)
        Me.pnlStoneGrid.Controls.Add(Me.txtStItem)
        Me.pnlStoneGrid.Controls.Add(Me.Label60)
        Me.pnlStoneGrid.Controls.Add(Me.Label59)
        Me.pnlStoneGrid.Controls.Add(Me.Label19)
        Me.pnlStoneGrid.Controls.Add(Me.Label14)
        Me.pnlStoneGrid.Controls.Add(Me.gridTagStone)
        Me.pnlStoneGrid.Controls.Add(Me.txtStPcs_NUM)
        Me.pnlStoneGrid.Controls.Add(Me.txtStWeight_WET)
        Me.pnlStoneGrid.Controls.Add(Me.txtStRate_Amt)
        Me.pnlStoneGrid.Controls.Add(Me.txtStAmount_Amt)
        Me.pnlStoneGrid.Controls.Add(Me.txtStMetalCode)
        Me.pnlStoneGrid.Location = New System.Drawing.Point(5, 12)
        Me.pnlStoneGrid.Name = "pnlStoneGrid"
        Me.pnlStoneGrid.Size = New System.Drawing.Size(707, 180)
        Me.pnlStoneGrid.TabIndex = 26
        '
        'Label21
        '
        Me.Label21.BackColor = System.Drawing.Color.Transparent
        Me.Label21.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(304, 1)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(85, 21)
        Me.Label21.TabIndex = 19
        Me.Label21.Text = "WEIGHT"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label20
        '
        Me.Label20.BackColor = System.Drawing.Color.Transparent
        Me.Label20.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(481, 1)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(100, 21)
        Me.Label20.TabIndex = 17
        Me.Label20.Text = "OLD AMT"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtStOldAmount_Amt
        '
        Me.txtStOldAmount_Amt.Location = New System.Drawing.Point(481, 22)
        Me.txtStOldAmount_Amt.Name = "txtStOldAmount_Amt"
        Me.txtStOldAmount_Amt.ReadOnly = True
        Me.txtStOldAmount_Amt.Size = New System.Drawing.Size(100, 21)
        Me.txtStOldAmount_Amt.TabIndex = 18
        '
        'Label17
        '
        Me.Label17.BackColor = System.Drawing.Color.Transparent
        Me.Label17.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(213, 1)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(90, 21)
        Me.Label17.TabIndex = 15
        Me.Label17.Text = "OLD WEIGHT"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtStOldWeight_WET
        '
        Me.txtStOldWeight_WET.Location = New System.Drawing.Point(213, 22)
        Me.txtStOldWeight_WET.Name = "txtStOldWeight_WET"
        Me.txtStOldWeight_WET.ReadOnly = True
        Me.txtStOldWeight_WET.Size = New System.Drawing.Size(90, 21)
        Me.txtStOldWeight_WET.TabIndex = 16
        Me.txtStOldWeight_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtStRowIndex
        '
        Me.txtStRowIndex.Location = New System.Drawing.Point(688, 2)
        Me.txtStRowIndex.Name = "txtStRowIndex"
        Me.txtStRowIndex.Size = New System.Drawing.Size(14, 21)
        Me.txtStRowIndex.TabIndex = 0
        Me.txtStRowIndex.Visible = False
        '
        'gridTagStonetotal
        '
        Me.gridTagStonetotal.AllowUserToAddRows = False
        Me.gridTagStonetotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridTagStonetotal.Enabled = False
        Me.gridTagStonetotal.Location = New System.Drawing.Point(3, 159)
        Me.gridTagStonetotal.Name = "gridTagStonetotal"
        Me.gridTagStonetotal.ReadOnly = True
        Me.gridTagStonetotal.Size = New System.Drawing.Size(693, 18)
        Me.gridTagStonetotal.TabIndex = 13
        '
        'txtStTagno
        '
        Me.txtStTagno.Location = New System.Drawing.Point(71, 22)
        Me.txtStTagno.Name = "txtStTagno"
        Me.txtStTagno.Size = New System.Drawing.Size(85, 21)
        Me.txtStTagno.TabIndex = 3
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(71, 1)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(85, 21)
        Me.Label12.TabIndex = 2
        Me.Label12.Text = "TAGNO"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label61
        '
        Me.Label61.BackColor = System.Drawing.Color.Transparent
        Me.Label61.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label61.Location = New System.Drawing.Point(582, 1)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(100, 21)
        Me.Label61.TabIndex = 10
        Me.Label61.Text = "AMOUNT"
        Me.Label61.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtStItem
        '
        Me.txtStItem.Location = New System.Drawing.Point(0, 22)
        Me.txtStItem.Name = "txtStItem"
        Me.txtStItem.Size = New System.Drawing.Size(70, 21)
        Me.txtStItem.TabIndex = 1
        '
        'Label60
        '
        Me.Label60.BackColor = System.Drawing.Color.Transparent
        Me.Label60.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label60.Location = New System.Drawing.Point(390, 1)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(90, 21)
        Me.Label60.TabIndex = 8
        Me.Label60.Text = "RATE"
        Me.Label60.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label59
        '
        Me.Label59.BackColor = System.Drawing.Color.Transparent
        Me.Label59.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label59.Location = New System.Drawing.Point(300, 1)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(3, 21)
        Me.Label59.TabIndex = 6
        Me.Label59.Text = "WEIGHT"
        Me.Label59.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label19
        '
        Me.Label19.BackColor = System.Drawing.Color.Transparent
        Me.Label19.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(157, 1)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(55, 21)
        Me.Label19.TabIndex = 4
        Me.Label19.Text = "PCS"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(0, 1)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(70, 21)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "ITEMID"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gridTagStone
        '
        Me.gridTagStone.AllowUserToAddRows = False
        Me.gridTagStone.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridTagStone.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridTagStone.ColumnHeadersVisible = False
        Me.gridTagStone.Location = New System.Drawing.Point(0, 44)
        Me.gridTagStone.MultiSelect = False
        Me.gridTagStone.Name = "gridTagStone"
        Me.gridTagStone.ReadOnly = True
        Me.gridTagStone.RowHeadersVisible = False
        Me.gridTagStone.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridTagStone.RowTemplate.Height = 20
        Me.gridTagStone.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridTagStone.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.gridTagStone.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridTagStone.Size = New System.Drawing.Size(696, 117)
        Me.gridTagStone.TabIndex = 12
        '
        'txtStPcs_NUM
        '
        Me.txtStPcs_NUM.Location = New System.Drawing.Point(157, 22)
        Me.txtStPcs_NUM.Name = "txtStPcs_NUM"
        Me.txtStPcs_NUM.Size = New System.Drawing.Size(55, 21)
        Me.txtStPcs_NUM.TabIndex = 5
        '
        'txtStWeight_WET
        '
        Me.txtStWeight_WET.Location = New System.Drawing.Point(304, 22)
        Me.txtStWeight_WET.Name = "txtStWeight_WET"
        Me.txtStWeight_WET.Size = New System.Drawing.Size(85, 21)
        Me.txtStWeight_WET.TabIndex = 7
        Me.txtStWeight_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtStRate_Amt
        '
        Me.txtStRate_Amt.Location = New System.Drawing.Point(390, 22)
        Me.txtStRate_Amt.Name = "txtStRate_Amt"
        Me.txtStRate_Amt.Size = New System.Drawing.Size(90, 21)
        Me.txtStRate_Amt.TabIndex = 9
        '
        'txtStAmount_Amt
        '
        Me.txtStAmount_Amt.Location = New System.Drawing.Point(582, 22)
        Me.txtStAmount_Amt.Name = "txtStAmount_Amt"
        Me.txtStAmount_Amt.Size = New System.Drawing.Size(100, 21)
        Me.txtStAmount_Amt.TabIndex = 11
        '
        'txtStMetalCode
        '
        Me.txtStMetalCode.Enabled = False
        Me.txtStMetalCode.Location = New System.Drawing.Point(771, 18)
        Me.txtStMetalCode.Name = "txtStMetalCode"
        Me.txtStMetalCode.Size = New System.Drawing.Size(19, 21)
        Me.txtStMetalCode.TabIndex = 14
        Me.txtStMetalCode.Visible = False
        '
        'chkBarcode
        '
        Me.chkBarcode.AutoSize = True
        Me.chkBarcode.Location = New System.Drawing.Point(729, 99)
        Me.chkBarcode.Name = "chkBarcode"
        Me.chkBarcode.Size = New System.Drawing.Size(73, 17)
        Me.chkBarcode.TabIndex = 7
        Me.chkBarcode.Text = "Barcode"
        Me.chkBarcode.UseVisualStyleBackColor = True
        '
        'TagMerge
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(1020, 630)
        Me.Controls.Add(Me.TabControlstone)
        Me.Controls.Add(Me.lblHelpText)
        Me.Controls.Add(Me.grpTagDetail)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(1078, 745)
        Me.Name = "TagMerge"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TagMerge"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.PnlMark.ResumeLayout(False)
        Me.PnlMark.PerformLayout()
        Me.grpTagDetail.ResumeLayout(False)
        Me.grpTagDetail.PerformLayout()
        CType(Me.gridTAGTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridTAG, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControlstone.ResumeLayout(False)
        Me.tabStone.ResumeLayout(False)
        Me.tabStone.PerformLayout()
        Me.grpStoneDetails.ResumeLayout(False)
        Me.pnlStoneGrid.ResumeLayout(False)
        Me.pnlStoneGrid.PerformLayout()
        CType(Me.gridTagStonetotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridTagStone, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpTagDetail As CodeVendor.Controls.Grouper
    Friend WithEvents txtTAGRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents txtTagAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents gridTAGTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridTAG As System.Windows.Forms.DataGridView
    Friend WithEvents txttAGStoneAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtTagTagNo As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtTagPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtTagGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtTagNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtTagItemId As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents lblHelpText As System.Windows.Forms.Label
    Friend WithEvents TabControlstone As System.Windows.Forms.TabControl
    Friend WithEvents tabStone As System.Windows.Forms.TabPage
    Friend WithEvents txtStRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents grpStoneDetails As System.Windows.Forms.GroupBox
    Friend WithEvents pnlStoneGrid As System.Windows.Forms.Panel
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents txtStItem As System.Windows.Forms.TextBox
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents gridTagStone As System.Windows.Forms.DataGridView
    Friend WithEvents txtStPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtStWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtStRate_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtStAmount_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtStMetalCode As System.Windows.Forms.TextBox
    Friend WithEvents rbtInterchange As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGenerateNewTag As System.Windows.Forms.RadioButton
    Friend WithEvents PnlMark As System.Windows.Forms.Panel
    Friend WithEvents txtTagRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents txttAGStoneWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txttAGDiaWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txttAGDiaAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtStTagno As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents gridTagStonetotal As System.Windows.Forms.DataGridView
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtTagOldGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtTagOldNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtTagOldAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txtStOldWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtStOldAmount_Amt As System.Windows.Forms.TextBox
    Friend WithEvents chkBarcode As CheckBox
End Class
