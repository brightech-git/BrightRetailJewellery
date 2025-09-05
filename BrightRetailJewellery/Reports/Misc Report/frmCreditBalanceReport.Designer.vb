<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreditBalanceReport
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
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.lblTitle = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.chkDetailed = New System.Windows.Forms.CheckBox
        Me.cmbAccountType = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.cmbAcGrp = New GiritechPack.CheckedComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.chkSummery = New System.Windows.Forms.CheckBox
        Me.rdbTranDateWise = New System.Windows.Forms.RadioButton
        Me.rdbBillDateWise = New System.Windows.Forms.RadioButton
        Me.Label2 = New System.Windows.Forms.Label
        Me.dtpFrom = New GiritechPack.DatePicker(Me.components)
        Me.cmbAcName_Own = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.chkCmbCostCentre = New GiritechPack.CheckedComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbCompany = New System.Windows.Forms.ComboBox
        Me.btnView = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 143)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 21
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.Size = New System.Drawing.Size(946, 337)
        Me.gridView.TabIndex = 2
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 107)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(946, 36)
        Me.lblTitle.TabIndex = 2
        Me.lblTitle.Text = "Label1"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkDetailed)
        Me.Panel1.Controls.Add(Me.cmbAccountType)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.cmbAcGrp)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.chkSummery)
        Me.Panel1.Controls.Add(Me.rdbTranDateWise)
        Me.Panel1.Controls.Add(Me.rdbBillDateWise)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.cmbAcName_Own)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cmbCompany)
        Me.Panel1.Controls.Add(Me.btnView)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(946, 107)
        Me.Panel1.TabIndex = 0
        '
        'chkDetailed
        '
        Me.chkDetailed.AutoSize = True
        Me.chkDetailed.Checked = True
        Me.chkDetailed.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDetailed.Location = New System.Drawing.Point(209, 83)
        Me.chkDetailed.Name = "chkDetailed"
        Me.chkDetailed.Size = New System.Drawing.Size(65, 17)
        Me.chkDetailed.TabIndex = 20
        Me.chkDetailed.Text = "Detailed"
        Me.chkDetailed.UseVisualStyleBackColor = True
        '
        'cmbAccountType
        '
        Me.cmbAccountType.FormattingEnabled = True
        Me.cmbAccountType.Location = New System.Drawing.Point(390, 41)
        Me.cmbAccountType.Name = "cmbAccountType"
        Me.cmbAccountType.Size = New System.Drawing.Size(136, 21)
        Me.cmbAccountType.TabIndex = 9
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(337, 46)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(47, 13)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Ac Type"
        '
        'cmbAcGrp
        '
        Me.cmbAcGrp.CheckOnClick = True
        Me.cmbAcGrp.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbAcGrp.DropDownHeight = 1
        Me.cmbAcGrp.FormattingEnabled = True
        Me.cmbAcGrp.IntegralHeight = False
        Me.cmbAcGrp.Location = New System.Drawing.Point(72, 41)
        Me.cmbAcGrp.Name = "cmbAcGrp"
        Me.cmbAcGrp.Size = New System.Drawing.Size(262, 21)
        Me.cmbAcGrp.TabIndex = 7
        Me.cmbAcGrp.ValueSeparator = ", "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(4, 46)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Ac Group"
        '
        'chkSummery
        '
        Me.chkSummery.AutoSize = True
        Me.chkSummery.Location = New System.Drawing.Point(282, 84)
        Me.chkSummery.Name = "chkSummery"
        Me.chkSummery.Size = New System.Drawing.Size(69, 17)
        Me.chkSummery.TabIndex = 14
        Me.chkSummery.Text = "Summary"
        Me.chkSummery.UseVisualStyleBackColor = True
        '
        'rdbTranDateWise
        '
        Me.rdbTranDateWise.AutoSize = True
        Me.rdbTranDateWise.Location = New System.Drawing.Point(105, 83)
        Me.rdbTranDateWise.Name = "rdbTranDateWise"
        Me.rdbTranDateWise.Size = New System.Drawing.Size(100, 17)
        Me.rdbTranDateWise.TabIndex = 13
        Me.rdbTranDateWise.Text = "Tran Date Wise"
        Me.rdbTranDateWise.UseVisualStyleBackColor = True
        '
        'rdbBillDateWise
        '
        Me.rdbBillDateWise.AutoSize = True
        Me.rdbBillDateWise.Checked = True
        Me.rdbBillDateWise.Location = New System.Drawing.Point(6, 83)
        Me.rdbBillDateWise.Name = "rdbBillDateWise"
        Me.rdbBillDateWise.Size = New System.Drawing.Size(91, 17)
        Me.rdbBillDateWise.TabIndex = 12
        Me.rdbBillDateWise.TabStop = True
        Me.rdbBillDateWise.Text = "Bill Date Wise"
        Me.rdbBillDateWise.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(4, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "As On Date"
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(72, 14)
        Me.dtpFrom.Mask = "##-##-####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(82, 20)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "29/09/2010"
        Me.dtpFrom.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'cmbAcName_Own
        '
        Me.cmbAcName_Own.FormattingEnabled = True
        Me.cmbAcName_Own.Location = New System.Drawing.Point(594, 38)
        Me.cmbAcName_Own.Name = "cmbAcName_Own"
        Me.cmbAcName_Own.Size = New System.Drawing.Size(276, 21)
        Me.cmbAcName_Own.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(530, 46)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Ac Name"
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(594, 11)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(276, 21)
        Me.chkCmbCostCentre.TabIndex = 5
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(529, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(157, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Company"
        '
        'cmbCompany
        '
        Me.cmbCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCompany.FormattingEnabled = True
        Me.cmbCompany.Location = New System.Drawing.Point(214, 14)
        Me.cmbCompany.Name = "cmbCompany"
        Me.cmbCompany.Size = New System.Drawing.Size(312, 21)
        Me.cmbCompany.TabIndex = 3
        '
        'btnView
        '
        Me.btnView.Location = New System.Drawing.Point(358, 71)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(96, 31)
        Me.btnView.TabIndex = 15
        Me.btnView.Text = "View [F2]"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(561, 71)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(96, 31)
        Me.btnExport.TabIndex = 17
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(662, 71)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(96, 31)
        Me.btnExit.TabIndex = 18
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(764, 71)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(96, 31)
        Me.btnPrint.TabIndex = 19
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(459, 71)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(96, 31)
        Me.btnNew.TabIndex = 16
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'frmCreditBalanceReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(946, 480)
        Me.ControlBox = False
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.Panel1)
        Me.KeyPreview = True
        Me.Name = "frmCreditBalanceReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Credit Balance Report"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpFrom As GiritechPack.DatePicker
    Friend WithEvents cmbAcName_Own As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As GiritechPack.CheckedComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbCompany As System.Windows.Forms.ComboBox
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents rdbTranDateWise As System.Windows.Forms.RadioButton
    Friend WithEvents rdbBillDateWise As System.Windows.Forms.RadioButton
    Friend WithEvents chkSummery As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbAcGrp As GiritechPack.CheckedComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbAccountType As System.Windows.Forms.ComboBox
    Friend WithEvents chkDetailed As System.Windows.Forms.CheckBox
End Class
