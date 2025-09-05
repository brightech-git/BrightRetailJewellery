<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTargetCounterReport
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
        Me.grpContainer = New System.Windows.Forms.GroupBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rdbGram = New System.Windows.Forms.RadioButton
        Me.rdbCarot = New System.Windows.Forms.RadioButton
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbCounterGroup = New System.Windows.Forms.ComboBox
        Me.dtpToDate = New BrighttechPack.DatePicker(Me.components)
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.rbtDetailed = New System.Windows.Forms.RadioButton
        Me.rbtSummary = New System.Windows.Forms.RadioButton
        Me.dtpFromDate = New BrighttechPack.DatePicker(Me.components)
        Me.chkLstCounter = New System.Windows.Forms.CheckedListBox
        Me.chkCounterNameAll = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.grpContainer.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.Label4)
        Me.grpContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.grpContainer.Controls.Add(Me.GroupBox1)
        Me.grpContainer.Controls.Add(Me.Label3)
        Me.grpContainer.Controls.Add(Me.cmbCounterGroup)
        Me.grpContainer.Controls.Add(Me.dtpToDate)
        Me.grpContainer.Controls.Add(Me.GroupBox2)
        Me.grpContainer.Controls.Add(Me.dtpFromDate)
        Me.grpContainer.Controls.Add(Me.chkLstCounter)
        Me.grpContainer.Controls.Add(Me.chkCounterNameAll)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Location = New System.Drawing.Point(174, 15)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(430, 409)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(9, 49)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 21)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Cost Centre"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(88, 49)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(231, 21)
        Me.chkCmbCostCentre.TabIndex = 5
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rdbGram)
        Me.GroupBox1.Controls.Add(Me.rdbCarot)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 324)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(161, 34)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        '
        'rdbGram
        '
        Me.rdbGram.Checked = True
        Me.rdbGram.Location = New System.Drawing.Point(5, 12)
        Me.rdbGram.Name = "rdbGram"
        Me.rdbGram.Size = New System.Drawing.Size(68, 18)
        Me.rdbGram.TabIndex = 0
        Me.rdbGram.TabStop = True
        Me.rdbGram.Text = "Gram"
        '
        'rdbCarot
        '
        Me.rdbCarot.Location = New System.Drawing.Point(81, 12)
        Me.rdbCarot.Name = "rdbCarot"
        Me.rdbCarot.Size = New System.Drawing.Size(74, 18)
        Me.rdbCarot.TabIndex = 1
        Me.rdbCarot.Text = "Carat"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Counter Group"
        '
        'cmbCounterGroup
        '
        Me.cmbCounterGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCounterGroup.FormattingEnabled = True
        Me.cmbCounterGroup.Location = New System.Drawing.Point(89, 84)
        Me.cmbCounterGroup.Name = "cmbCounterGroup"
        Me.cmbCounterGroup.Size = New System.Drawing.Size(121, 21)
        Me.cmbCounterGroup.TabIndex = 7
        '
        'dtpToDate
        '
        Me.dtpToDate.Location = New System.Drawing.Point(226, 14)
        Me.dtpToDate.Mask = "##/##/####"
        Me.dtpToDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpToDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpToDate.Size = New System.Drawing.Size(93, 20)
        Me.dtpToDate.TabIndex = 3
        Me.dtpToDate.Text = "07/03/9998"
        Me.dtpToDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbtDetailed)
        Me.GroupBox2.Controls.Add(Me.rbtSummary)
        Me.GroupBox2.Location = New System.Drawing.Point(206, 324)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(161, 34)
        Me.GroupBox2.TabIndex = 11
        Me.GroupBox2.TabStop = False
        '
        'rbtDetailed
        '
        Me.rbtDetailed.Checked = True
        Me.rbtDetailed.Location = New System.Drawing.Point(5, 12)
        Me.rbtDetailed.Name = "rbtDetailed"
        Me.rbtDetailed.Size = New System.Drawing.Size(68, 18)
        Me.rbtDetailed.TabIndex = 0
        Me.rbtDetailed.TabStop = True
        Me.rbtDetailed.Text = "Detail"
        '
        'rbtSummary
        '
        Me.rbtSummary.Location = New System.Drawing.Point(81, 12)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(74, 18)
        Me.rbtSummary.TabIndex = 1
        Me.rbtSummary.Text = "Summary "
        '
        'dtpFromDate
        '
        Me.dtpFromDate.Location = New System.Drawing.Point(89, 14)
        Me.dtpFromDate.Mask = "##/##/####"
        Me.dtpFromDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFromDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFromDate.Size = New System.Drawing.Size(79, 20)
        Me.dtpFromDate.TabIndex = 1
        Me.dtpFromDate.Text = "07/03/9998"
        Me.dtpFromDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkLstCounter
        '
        Me.chkLstCounter.CheckOnClick = True
        Me.chkLstCounter.FormattingEnabled = True
        Me.chkLstCounter.Location = New System.Drawing.Point(9, 134)
        Me.chkLstCounter.Name = "chkLstCounter"
        Me.chkLstCounter.Size = New System.Drawing.Size(410, 184)
        Me.chkLstCounter.TabIndex = 9
        '
        'chkCounterNameAll
        '
        Me.chkCounterNameAll.AutoSize = True
        Me.chkCounterNameAll.Location = New System.Drawing.Point(12, 111)
        Me.chkCounterNameAll.Name = "chkCounterNameAll"
        Me.chkCounterNameAll.Size = New System.Drawing.Size(94, 17)
        Me.chkCounterNameAll.TabIndex = 8
        Me.chkCounterNameAll.Text = "Counter Name"
        Me.chkCounterNameAll.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(174, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To Date"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "From Date"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(12, 368)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 12
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(224, 368)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 14
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(118, 368)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 13
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'frmTargetCounterReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(778, 446)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpContainer)
        Me.KeyPreview = True
        Me.Name = "frmTargetCounterReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmTargetCounterSalesReport"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents dtpToDate As BrighttechPack.DatePicker
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents dtpFromDate As BrighttechPack.DatePicker
    Friend WithEvents chkLstCounter As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCounterNameAll As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbCounterGroup As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rdbGram As System.Windows.Forms.RadioButton
    Friend WithEvents rdbCarot As System.Windows.Forms.RadioButton
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
