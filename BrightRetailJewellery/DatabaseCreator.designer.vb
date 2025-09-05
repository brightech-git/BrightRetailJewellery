<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DatabaseCreator
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DatabaseCreator))
        Me.pBar = New System.Windows.Forms.ProgressBar()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.cmbFromDt = New System.Windows.Forms.ComboBox()
        Me.cmbToDt = New System.Windows.Forms.ComboBox()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblCompayId = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblCompanyName = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblServer = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblPAth = New System.Windows.Forms.Label()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnInstall = New System.Windows.Forms.Button()
        Me.grpFooter = New System.Windows.Forms.GroupBox()
        Me.btnMenuModifier = New System.Windows.Forms.Button()
        Me.lbl1 = New System.Windows.Forms.Label()
        Me.pnlYEnd = New System.Windows.Forms.Panel()
        Me.chkApproval = New System.Windows.Forms.CheckBox()
        Me.chkTobe = New System.Windows.Forms.CheckBox()
        Me.chkWeightBalance = New System.Windows.Forms.CheckBox()
        Me.chkOrderRepair = New System.Windows.Forms.CheckBox()
        Me.chkTrailBalance = New System.Windows.Forms.CheckBox()
        Me.chkNonTag = New System.Windows.Forms.CheckBox()
        Me.chkAdvanceDue = New System.Windows.Forms.CheckBox()
        Me.chkDbFieldUpdation = New System.Windows.Forms.CheckBox()
        Me.chkTag = New System.Windows.Forms.CheckBox()
        Me.chkRunnoUpd = New System.Windows.Forms.CheckBox()
        Me.chkPkCustInfo = New System.Windows.Forms.CheckBox()
        Me.pnlYEnd.SuspendLayout()
        Me.SuspendLayout()
        '
        'pBar
        '
        Me.pBar.Location = New System.Drawing.Point(231, 272)
        Me.pBar.Name = "pBar"
        Me.pBar.Size = New System.Drawing.Size(370, 25)
        Me.pBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pBar.TabIndex = 15
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Location = New System.Drawing.Point(229, 303)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(44, 13)
        Me.lblStatus.TabIndex = 14
        Me.lblStatus.Text = "Label1"
        '
        'cmbFromDt
        '
        Me.cmbFromDt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFromDt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbFromDt.FormattingEnabled = True
        Me.cmbFromDt.Location = New System.Drawing.Point(346, 149)
        Me.cmbFromDt.Name = "cmbFromDt"
        Me.cmbFromDt.Size = New System.Drawing.Size(110, 21)
        Me.cmbFromDt.TabIndex = 1
        '
        'cmbToDt
        '
        Me.cmbToDt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbToDt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbToDt.FormattingEnabled = True
        Me.cmbToDt.Location = New System.Drawing.Point(493, 149)
        Me.cmbToDt.Name = "cmbToDt"
        Me.cmbToDt.Size = New System.Drawing.Size(110, 21)
        Me.cmbToDt.TabIndex = 3
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.BackColor = System.Drawing.Color.Transparent
        Me.lblFrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrom.ForeColor = System.Drawing.SystemColors.Control
        Me.lblFrom.Location = New System.Drawing.Point(230, 153)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(67, 13)
        Me.lblFrom.TabIndex = 0
        Me.lblFrom.Text = "Date From"
        Me.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.Control
        Me.Label5.Location = New System.Drawing.Point(230, 177)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(74, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "CompanyId"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCompayId
        '
        Me.lblCompayId.AutoSize = True
        Me.lblCompayId.BackColor = System.Drawing.Color.Transparent
        Me.lblCompayId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompayId.ForeColor = System.Drawing.SystemColors.Control
        Me.lblCompayId.Location = New System.Drawing.Point(343, 177)
        Me.lblCompayId.Name = "lblCompayId"
        Me.lblCompayId.Size = New System.Drawing.Size(74, 13)
        Me.lblCompayId.TabIndex = 5
        Me.lblCompayId.Text = "CompanyId"
        Me.lblCompayId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.Control
        Me.Label6.Location = New System.Drawing.Point(230, 201)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(99, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Company Name"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCompanyName
        '
        Me.lblCompanyName.AutoSize = True
        Me.lblCompanyName.BackColor = System.Drawing.Color.Transparent
        Me.lblCompanyName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompanyName.ForeColor = System.Drawing.SystemColors.Control
        Me.lblCompanyName.Location = New System.Drawing.Point(343, 201)
        Me.lblCompanyName.Name = "lblCompanyName"
        Me.lblCompanyName.Size = New System.Drawing.Size(74, 13)
        Me.lblCompanyName.TabIndex = 7
        Me.lblCompanyName.Text = "CompanyId"
        Me.lblCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.Control
        Me.Label7.Location = New System.Drawing.Point(230, 225)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(46, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Server"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblServer
        '
        Me.lblServer.AutoSize = True
        Me.lblServer.BackColor = System.Drawing.Color.Transparent
        Me.lblServer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServer.ForeColor = System.Drawing.SystemColors.Control
        Me.lblServer.Location = New System.Drawing.Point(343, 225)
        Me.lblServer.Name = "lblServer"
        Me.lblServer.Size = New System.Drawing.Size(74, 13)
        Me.lblServer.TabIndex = 9
        Me.lblServer.Text = "CompanyId"
        Me.lblServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.Control
        Me.Label8.Location = New System.Drawing.Point(230, 249)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(52, 13)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Db Path"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPAth
        '
        Me.lblPAth.AutoSize = True
        Me.lblPAth.BackColor = System.Drawing.Color.Transparent
        Me.lblPAth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPAth.ForeColor = System.Drawing.SystemColors.Control
        Me.lblPAth.Location = New System.Drawing.Point(343, 249)
        Me.lblPAth.Name = "lblPAth"
        Me.lblPAth.Size = New System.Drawing.Size(74, 13)
        Me.lblPAth.TabIndex = 11
        Me.lblPAth.Text = "CompanyId"
        Me.lblPAth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.BackColor = System.Drawing.Color.Transparent
        Me.lblTo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTo.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.lblTo.Location = New System.Drawing.Point(462, 153)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(20, 13)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExit.Location = New System.Drawing.Point(442, 322)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 13
        Me.btnExit.Text = "&Close"
        Me.btnExit.UseVisualStyleBackColor = False
        '
        'btnInstall
        '
        Me.btnInstall.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnInstall.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnInstall.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnInstall.Location = New System.Drawing.Point(302, 322)
        Me.btnInstall.Name = "btnInstall"
        Me.btnInstall.Size = New System.Drawing.Size(100, 30)
        Me.btnInstall.TabIndex = 0
        Me.btnInstall.Text = "&Proceed"
        Me.btnInstall.UseVisualStyleBackColor = False
        '
        'grpFooter
        '
        Me.grpFooter.BackColor = System.Drawing.Color.Transparent
        Me.grpFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.grpFooter.Location = New System.Drawing.Point(0, 358)
        Me.grpFooter.Name = "grpFooter"
        Me.grpFooter.Size = New System.Drawing.Size(634, 10)
        Me.grpFooter.TabIndex = 16
        Me.grpFooter.TabStop = False
        Me.grpFooter.Visible = False
        '
        'btnMenuModifier
        '
        Me.btnMenuModifier.BackColor = System.Drawing.Color.Transparent
        Me.btnMenuModifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnMenuModifier.Location = New System.Drawing.Point(524, -2)
        Me.btnMenuModifier.Name = "btnMenuModifier"
        Me.btnMenuModifier.Size = New System.Drawing.Size(110, 23)
        Me.btnMenuModifier.TabIndex = 14
        Me.btnMenuModifier.Text = "Option"
        Me.btnMenuModifier.UseVisualStyleBackColor = False
        Me.btnMenuModifier.Visible = False
        '
        'lbl1
        '
        Me.lbl1.AutoSize = True
        Me.lbl1.BackColor = System.Drawing.Color.Transparent
        Me.lbl1.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl1.ForeColor = System.Drawing.SystemColors.Control
        Me.lbl1.Location = New System.Drawing.Point(225, 9)
        Me.lbl1.Name = "lbl1"
        Me.lbl1.Size = New System.Drawing.Size(196, 24)
        Me.lbl1.TabIndex = 19
        Me.lbl1.Text = "Database Creation"
        '
        'pnlYEnd
        '
        Me.pnlYEnd.BackColor = System.Drawing.Color.Transparent
        Me.pnlYEnd.Controls.Add(Me.chkApproval)
        Me.pnlYEnd.Controls.Add(Me.chkTobe)
        Me.pnlYEnd.Controls.Add(Me.chkWeightBalance)
        Me.pnlYEnd.Controls.Add(Me.chkOrderRepair)
        Me.pnlYEnd.Controls.Add(Me.chkTrailBalance)
        Me.pnlYEnd.Controls.Add(Me.chkNonTag)
        Me.pnlYEnd.Controls.Add(Me.chkAdvanceDue)
        Me.pnlYEnd.Controls.Add(Me.chkDbFieldUpdation)
        Me.pnlYEnd.Controls.Add(Me.chkTag)
        Me.pnlYEnd.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.pnlYEnd.Location = New System.Drawing.Point(224, 75)
        Me.pnlYEnd.Name = "pnlYEnd"
        Me.pnlYEnd.Size = New System.Drawing.Size(405, 68)
        Me.pnlYEnd.TabIndex = 0
        Me.pnlYEnd.Visible = False
        '
        'chkApproval
        '
        Me.chkApproval.Checked = True
        Me.chkApproval.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkApproval.Location = New System.Drawing.Point(8, 6)
        Me.chkApproval.Name = "chkApproval"
        Me.chkApproval.Size = New System.Drawing.Size(93, 17)
        Me.chkApproval.TabIndex = 6
        Me.chkApproval.Text = "Approval"
        Me.chkApproval.UseVisualStyleBackColor = True
        '
        'chkTobe
        '
        Me.chkTobe.Checked = True
        Me.chkTobe.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTobe.Location = New System.Drawing.Point(8, 25)
        Me.chkTobe.Name = "chkTobe"
        Me.chkTobe.Size = New System.Drawing.Size(93, 17)
        Me.chkTobe.TabIndex = 7
        Me.chkTobe.Text = "JND"
        Me.chkTobe.UseVisualStyleBackColor = True
        '
        'chkWeightBalance
        '
        Me.chkWeightBalance.AutoSize = True
        Me.chkWeightBalance.Checked = True
        Me.chkWeightBalance.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWeightBalance.Location = New System.Drawing.Point(268, 7)
        Me.chkWeightBalance.Name = "chkWeightBalance"
        Me.chkWeightBalance.Size = New System.Drawing.Size(126, 17)
        Me.chkWeightBalance.TabIndex = 5
        Me.chkWeightBalance.Text = "Weight Balance"
        Me.chkWeightBalance.UseVisualStyleBackColor = True
        '
        'chkOrderRepair
        '
        Me.chkOrderRepair.AutoSize = True
        Me.chkOrderRepair.Checked = True
        Me.chkOrderRepair.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOrderRepair.Location = New System.Drawing.Point(268, 46)
        Me.chkOrderRepair.Name = "chkOrderRepair"
        Me.chkOrderRepair.Size = New System.Drawing.Size(113, 17)
        Me.chkOrderRepair.TabIndex = 2
        Me.chkOrderRepair.TabStop = False
        Me.chkOrderRepair.Text = "Order/Repair"
        Me.chkOrderRepair.UseVisualStyleBackColor = True
        Me.chkOrderRepair.Visible = False
        '
        'chkTrailBalance
        '
        Me.chkTrailBalance.AutoSize = True
        Me.chkTrailBalance.Checked = True
        Me.chkTrailBalance.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTrailBalance.Location = New System.Drawing.Point(130, 8)
        Me.chkTrailBalance.Name = "chkTrailBalance"
        Me.chkTrailBalance.Size = New System.Drawing.Size(131, 17)
        Me.chkTrailBalance.TabIndex = 4
        Me.chkTrailBalance.Text = "Amount Balance"
        Me.chkTrailBalance.UseVisualStyleBackColor = True
        '
        'chkNonTag
        '
        Me.chkNonTag.AutoSize = True
        Me.chkNonTag.Checked = True
        Me.chkNonTag.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNonTag.Location = New System.Drawing.Point(8, 46)
        Me.chkNonTag.Name = "chkNonTag"
        Me.chkNonTag.Size = New System.Drawing.Size(79, 17)
        Me.chkNonTag.TabIndex = 1
        Me.chkNonTag.TabStop = False
        Me.chkNonTag.Text = "Non Tag"
        Me.chkNonTag.UseVisualStyleBackColor = True
        Me.chkNonTag.Visible = False
        '
        'chkAdvanceDue
        '
        Me.chkAdvanceDue.AutoSize = True
        Me.chkAdvanceDue.Checked = True
        Me.chkAdvanceDue.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAdvanceDue.Location = New System.Drawing.Point(130, 48)
        Me.chkAdvanceDue.Name = "chkAdvanceDue"
        Me.chkAdvanceDue.Size = New System.Drawing.Size(115, 17)
        Me.chkAdvanceDue.TabIndex = 3
        Me.chkAdvanceDue.TabStop = False
        Me.chkAdvanceDue.Text = "Advance/Due"
        Me.chkAdvanceDue.UseVisualStyleBackColor = True
        Me.chkAdvanceDue.Visible = False
        '
        'chkDbFieldUpdation
        '
        Me.chkDbFieldUpdation.Checked = True
        Me.chkDbFieldUpdation.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDbFieldUpdation.Location = New System.Drawing.Point(130, 27)
        Me.chkDbFieldUpdation.Name = "chkDbFieldUpdation"
        Me.chkDbFieldUpdation.Size = New System.Drawing.Size(93, 17)
        Me.chkDbFieldUpdation.TabIndex = 8
        Me.chkDbFieldUpdation.Text = "Db Update"
        Me.chkDbFieldUpdation.UseVisualStyleBackColor = True
        '
        'chkTag
        '
        Me.chkTag.AutoSize = True
        Me.chkTag.Checked = True
        Me.chkTag.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTag.Location = New System.Drawing.Point(268, 27)
        Me.chkTag.Name = "chkTag"
        Me.chkTag.Size = New System.Drawing.Size(50, 17)
        Me.chkTag.TabIndex = 0
        Me.chkTag.TabStop = False
        Me.chkTag.Text = "Tag"
        Me.chkTag.UseVisualStyleBackColor = True
        Me.chkTag.Visible = False
        '
        'chkRunnoUpd
        '
        Me.chkRunnoUpd.AutoSize = True
        Me.chkRunnoUpd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRunnoUpd.Location = New System.Drawing.Point(528, 244)
        Me.chkRunnoUpd.Name = "chkRunnoUpd"
        Me.chkRunnoUpd.Size = New System.Drawing.Size(95, 17)
        Me.chkRunnoUpd.TabIndex = 21
        Me.chkRunnoUpd.Text = "Runno Upd"
        Me.chkRunnoUpd.UseVisualStyleBackColor = True
        Me.chkRunnoUpd.Visible = False
        '
        'chkPkCustInfo
        '
        Me.chkPkCustInfo.AutoSize = True
        Me.chkPkCustInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPkCustInfo.Location = New System.Drawing.Point(528, 222)
        Me.chkPkCustInfo.Name = "chkPkCustInfo"
        Me.chkPkCustInfo.Size = New System.Drawing.Size(101, 17)
        Me.chkPkCustInfo.TabIndex = 22
        Me.chkPkCustInfo.Text = "Pk CustInfo"
        Me.chkPkCustInfo.UseVisualStyleBackColor = True
        Me.chkPkCustInfo.Visible = False
        '
        'DatabaseCreator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(634, 368)
        Me.Controls.Add(Me.btnMenuModifier)
        Me.Controls.Add(Me.chkPkCustInfo)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnInstall)
        Me.Controls.Add(Me.chkRunnoUpd)
        Me.Controls.Add(Me.pnlYEnd)
        Me.Controls.Add(Me.lblTo)
        Me.Controls.Add(Me.lbl1)
        Me.Controls.Add(Me.cmbFromDt)
        Me.Controls.Add(Me.cmbToDt)
        Me.Controls.Add(Me.lblCompayId)
        Me.Controls.Add(Me.lblFrom)
        Me.Controls.Add(Me.lblCompanyName)
        Me.Controls.Add(Me.grpFooter)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblServer)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lblPAth)
        Me.Controls.Add(Me.pBar)
        Me.Controls.Add(Me.Label7)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.Control
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "DatabaseCreator"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DataBase Creation"
        Me.pnlYEnd.ResumeLayout(False)
        Me.pnlYEnd.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnInstall As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents pBar As System.Windows.Forms.ProgressBar
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents cmbFromDt As System.Windows.Forms.ComboBox
    Friend WithEvents cmbToDt As System.Windows.Forms.ComboBox
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblCompayId As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblCompanyName As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblServer As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblPAth As System.Windows.Forms.Label
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents grpFooter As System.Windows.Forms.GroupBox
    Friend WithEvents lbl1 As System.Windows.Forms.Label
    Friend WithEvents pnlYEnd As System.Windows.Forms.Panel
    Friend WithEvents chkOrderRepair As System.Windows.Forms.CheckBox
    Friend WithEvents chkNonTag As System.Windows.Forms.CheckBox
    Friend WithEvents chkTag As System.Windows.Forms.CheckBox
    Friend WithEvents chkWeightBalance As System.Windows.Forms.CheckBox
    Friend WithEvents chkTrailBalance As System.Windows.Forms.CheckBox
    Friend WithEvents chkAdvanceDue As System.Windows.Forms.CheckBox
    Friend WithEvents chkDbFieldUpdation As System.Windows.Forms.CheckBox
    Friend WithEvents chkApproval As System.Windows.Forms.CheckBox
    Friend WithEvents chkTobe As System.Windows.Forms.CheckBox
    Friend WithEvents chkRunnoUpd As System.Windows.Forms.CheckBox
    Friend WithEvents btnMenuModifier As System.Windows.Forms.Button
    Friend WithEvents chkPkCustInfo As System.Windows.Forms.CheckBox

End Class
