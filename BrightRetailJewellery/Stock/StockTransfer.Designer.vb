<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StockTransfer
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
        Me.grpContainer = New System.Windows.Forms.GroupBox()
        Me.txtEstNo = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.ChkOnlyApproval = New System.Windows.Forms.CheckBox()
        Me.ChkWithApproval = New System.Windows.Forms.CheckBox()
        Me.lblCostCentre = New System.Windows.Forms.Label()
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox()
        Me.gridViewFooter = New System.Windows.Forms.DataGridView()
        Me.cmbNewCounter = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbOldCompany = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbCompany_MAN = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtFindTag = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtItemId_NUM = New System.Windows.Forms.TextBox()
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.lblRowDet1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtTagNo = New System.Windows.Forms.TextBox()
        Me.txtLotNo_NUM = New System.Windows.Forms.TextBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnTransfer = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.gridView_OWN = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grpContainer.SuspendLayout()
        CType(Me.gridViewFooter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.txtEstNo)
        Me.grpContainer.Controls.Add(Me.Label9)
        Me.grpContainer.Controls.Add(Me.ChkOnlyApproval)
        Me.grpContainer.Controls.Add(Me.ChkWithApproval)
        Me.grpContainer.Controls.Add(Me.lblCostCentre)
        Me.grpContainer.Controls.Add(Me.cmbCostCentre)
        Me.grpContainer.Controls.Add(Me.gridViewFooter)
        Me.grpContainer.Controls.Add(Me.cmbNewCounter)
        Me.grpContainer.Controls.Add(Me.Label8)
        Me.grpContainer.Controls.Add(Me.cmbOldCompany)
        Me.grpContainer.Controls.Add(Me.Label6)
        Me.grpContainer.Controls.Add(Me.cmbCompany_MAN)
        Me.grpContainer.Controls.Add(Me.Label7)
        Me.grpContainer.Controls.Add(Me.txtFindTag)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.txtItemId_NUM)
        Me.grpContainer.Controls.Add(Me.chkDate)
        Me.grpContainer.Controls.Add(Me.dtpTo)
        Me.grpContainer.Controls.Add(Me.dtpFrom)
        Me.grpContainer.Controls.Add(Me.chkSelectAll)
        Me.grpContainer.Controls.Add(Me.lblRowDet1)
        Me.grpContainer.Controls.Add(Me.Label3)
        Me.grpContainer.Controls.Add(Me.Label4)
        Me.grpContainer.Controls.Add(Me.Label5)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.txtTagNo)
        Me.grpContainer.Controls.Add(Me.txtLotNo_NUM)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnTransfer)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.gridView_OWN)
        Me.grpContainer.Location = New System.Drawing.Point(10, 2)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(803, 530)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'txtEstNo
        '
        Me.txtEstNo.Location = New System.Drawing.Point(569, 67)
        Me.txtEstNo.Name = "txtEstNo"
        Me.txtEstNo.Size = New System.Drawing.Size(75, 21)
        Me.txtEstNo.TabIndex = 30
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(484, 71)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(85, 13)
        Me.Label9.TabIndex = 29
        Me.Label9.Text = "Estimation No"
        '
        'ChkOnlyApproval
        '
        Me.ChkOnlyApproval.AutoSize = True
        Me.ChkOnlyApproval.Location = New System.Drawing.Point(600, 95)
        Me.ChkOnlyApproval.Name = "ChkOnlyApproval"
        Me.ChkOnlyApproval.Size = New System.Drawing.Size(107, 17)
        Me.ChkOnlyApproval.TabIndex = 28
        Me.ChkOnlyApproval.Text = "Only Approval"
        Me.ChkOnlyApproval.UseVisualStyleBackColor = True
        '
        'ChkWithApproval
        '
        Me.ChkWithApproval.AutoSize = True
        Me.ChkWithApproval.Location = New System.Drawing.Point(488, 95)
        Me.ChkWithApproval.Name = "ChkWithApproval"
        Me.ChkWithApproval.Size = New System.Drawing.Size(106, 17)
        Me.ChkWithApproval.TabIndex = 27
        Me.ChkWithApproval.Text = "With Approval"
        Me.ChkWithApproval.UseVisualStyleBackColor = True
        '
        'lblCostCentre
        '
        Me.lblCostCentre.AutoSize = True
        Me.lblCostCentre.Location = New System.Drawing.Point(323, 45)
        Me.lblCostCentre.Name = "lblCostCentre"
        Me.lblCostCentre.Size = New System.Drawing.Size(76, 13)
        Me.lblCostCentre.TabIndex = 12
        Me.lblCostCentre.Text = "Cost Centre"
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(401, 41)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(213, 21)
        Me.cmbCostCentre.TabIndex = 13
        '
        'gridViewFooter
        '
        Me.gridViewFooter.AllowUserToAddRows = False
        Me.gridViewFooter.AllowUserToDeleteRows = False
        Me.gridViewFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewFooter.ColumnHeadersVisible = False
        Me.gridViewFooter.Enabled = False
        Me.gridViewFooter.Location = New System.Drawing.Point(6, 468)
        Me.gridViewFooter.Name = "gridViewFooter"
        Me.gridViewFooter.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridViewFooter.Size = New System.Drawing.Size(791, 37)
        Me.gridViewFooter.TabIndex = 32
        '
        'cmbNewCounter
        '
        Me.cmbNewCounter.FormattingEnabled = True
        Me.cmbNewCounter.Location = New System.Drawing.Point(100, 41)
        Me.cmbNewCounter.Name = "cmbNewCounter"
        Me.cmbNewCounter.Size = New System.Drawing.Size(222, 21)
        Me.cmbNewCounter.TabIndex = 11
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(7, 45)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(53, 13)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Counter"
        '
        'cmbOldCompany
        '
        Me.cmbOldCompany.FormattingEnabled = True
        Me.cmbOldCompany.Location = New System.Drawing.Point(100, 68)
        Me.cmbOldCompany.Name = "cmbOldCompany"
        Me.cmbOldCompany.Size = New System.Drawing.Size(378, 21)
        Me.cmbOldCompany.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(7, 72)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(95, 13)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "From Company"
        '
        'cmbCompany_MAN
        '
        Me.cmbCompany_MAN.FormattingEnabled = True
        Me.cmbCompany_MAN.Location = New System.Drawing.Point(100, 95)
        Me.cmbCompany_MAN.Name = "cmbCompany_MAN"
        Me.cmbCompany_MAN.Size = New System.Drawing.Size(378, 21)
        Me.cmbCompany_MAN.TabIndex = 17
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(548, 130)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(54, 13)
        Me.Label7.TabIndex = 25
        Me.Label7.Text = "Find Tag"
        '
        'txtFindTag
        '
        Me.txtFindTag.Location = New System.Drawing.Point(610, 127)
        Me.txtFindTag.Name = "txtFindTag"
        Me.txtFindTag.Size = New System.Drawing.Size(93, 21)
        Me.txtFindTag.TabIndex = 26
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(485, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Item Id"
        '
        'txtItemId_NUM
        '
        Me.txtItemId_NUM.Location = New System.Drawing.Point(547, 14)
        Me.txtItemId_NUM.Name = "txtItemId_NUM"
        Me.txtItemId_NUM.Size = New System.Drawing.Size(73, 21)
        Me.txtItemId_NUM.TabIndex = 7
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDate.Location = New System.Drawing.Point(2, 15)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(86, 17)
        Me.chkDate.TabIndex = 0
        Me.chkDate.Text = "Date From"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(229, 13)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(101, 13)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkSelectAll
        '
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Location = New System.Drawing.Point(12, 133)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkSelectAll.TabIndex = 20
        Me.chkSelectAll.Text = "&Select All"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'lblRowDet1
        '
        Me.lblRowDet1.AutoSize = True
        Me.lblRowDet1.BackColor = System.Drawing.Color.Aqua
        Me.lblRowDet1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRowDet1.ForeColor = System.Drawing.Color.Red
        Me.lblRowDet1.Location = New System.Drawing.Point(7, 508)
        Me.lblRowDet1.Name = "lblRowDet1"
        Me.lblRowDet1.Size = New System.Drawing.Size(56, 13)
        Me.lblRowDet1.TabIndex = 33
        Me.lblRowDet1.Text = "Lot No :"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(200, 17)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(20, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "To"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(626, 18)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(42, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "TagNo"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 100)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 13)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "To Company"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(323, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Lot No"
        '
        'txtTagNo
        '
        Me.txtTagNo.Location = New System.Drawing.Point(671, 14)
        Me.txtTagNo.Name = "txtTagNo"
        Me.txtTagNo.Size = New System.Drawing.Size(93, 21)
        Me.txtTagNo.TabIndex = 9
        '
        'txtLotNo_NUM
        '
        Me.txtLotNo_NUM.Location = New System.Drawing.Point(401, 14)
        Me.txtLotNo_NUM.Name = "txtLotNo_NUM"
        Me.txtLotNo_NUM.Size = New System.Drawing.Size(78, 21)
        Me.txtLotNo_NUM.TabIndex = 5
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(433, 127)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(110, 30)
        Me.btnExit.TabIndex = 24
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnTransfer
        '
        Me.btnTransfer.Location = New System.Drawing.Point(317, 127)
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.Size = New System.Drawing.Size(110, 30)
        Me.btnTransfer.TabIndex = 23
        Me.btnTransfer.Text = "&Transfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(211, 127)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 22
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(105, 127)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 21
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'gridView_OWN
        '
        Me.gridView_OWN.AllowUserToAddRows = False
        Me.gridView_OWN.AllowUserToDeleteRows = False
        Me.gridView_OWN.AllowUserToResizeColumns = False
        Me.gridView_OWN.AllowUserToResizeRows = False
        Me.gridView_OWN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView_OWN.Location = New System.Drawing.Point(6, 166)
        Me.gridView_OWN.MultiSelect = False
        Me.gridView_OWN.Name = "gridView_OWN"
        Me.gridView_OWN.RowHeadersVisible = False
        Me.gridView_OWN.Size = New System.Drawing.Size(791, 302)
        Me.gridView_OWN.TabIndex = 31
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'StockTransfer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(822, 541)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.grpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "StockTransfer"
        Me.Text = "StockTransfer"
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        CType(Me.gridViewFooter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtFindTag As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtItemId_NUM As System.Windows.Forms.TextBox
    Friend WithEvents chkDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents lblRowDet1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtTagNo As System.Windows.Forms.TextBox
    Friend WithEvents txtLotNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnTransfer As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents gridView_OWN As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbCompany_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOldCompany As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbNewCounter As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents gridViewFooter As System.Windows.Forms.DataGridView
    Friend WithEvents lblCostCentre As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents ChkOnlyApproval As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithApproval As System.Windows.Forms.CheckBox
    Friend WithEvents txtEstNo As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
End Class
