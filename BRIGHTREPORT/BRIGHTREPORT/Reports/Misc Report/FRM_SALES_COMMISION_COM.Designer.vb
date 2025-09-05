<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_SALES_COMMISION_COM
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
        Me.GrpContainer = New System.Windows.Forms.GroupBox
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.CmbSearch = New System.Windows.Forms.ComboBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.DgvCategory = New System.Windows.Forms.DataGridView
        Me.DgvMetal = New System.Windows.Forms.DataGridView
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbtMetalWise = New System.Windows.Forms.RadioButton
        Me.rbtCategorywise = New System.Windows.Forms.RadioButton
        Me.ChkcmbCompany = New BrighttechPack.CheckedComboBox
        Me.chkOrderbyEmpId = New System.Windows.Forms.CheckBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.rbtSummary = New System.Windows.Forms.RadioButton
        Me.rbtDetailed = New System.Windows.Forms.RadioButton
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.GrpContainer.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.DgvCategory, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgvMetal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.txtSearch)
        Me.GrpContainer.Controls.Add(Me.CmbSearch)
        Me.GrpContainer.Controls.Add(Me.Panel2)
        Me.GrpContainer.Controls.Add(Me.Panel1)
        Me.GrpContainer.Controls.Add(Me.ChkcmbCompany)
        Me.GrpContainer.Controls.Add(Me.chkOrderbyEmpId)
        Me.GrpContainer.Controls.Add(Me.Label3)
        Me.GrpContainer.Controls.Add(Me.rbtSummary)
        Me.GrpContainer.Controls.Add(Me.rbtDetailed)
        Me.GrpContainer.Controls.Add(Me.Label2)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.dtpTo)
        Me.GrpContainer.Controls.Add(Me.dtpFrom)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Location = New System.Drawing.Point(112, 47)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(546, 439)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(376, 390)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(100, 21)
        Me.txtSearch.TabIndex = 14
        Me.txtSearch.Visible = False
        '
        'CmbSearch
        '
        Me.CmbSearch.FormattingEnabled = True
        Me.CmbSearch.Location = New System.Drawing.Point(376, 362)
        Me.CmbSearch.Name = "CmbSearch"
        Me.CmbSearch.Size = New System.Drawing.Size(121, 21)
        Me.CmbSearch.TabIndex = 13
        Me.CmbSearch.Visible = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.DgvCategory)
        Me.Panel2.Controls.Add(Me.DgvMetal)
        Me.Panel2.Location = New System.Drawing.Point(29, 160)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(511, 169)
        Me.Panel2.TabIndex = 16
        '
        'DgvCategory
        '
        Me.DgvCategory.AllowUserToAddRows = False
        Me.DgvCategory.AllowUserToDeleteRows = False
        Me.DgvCategory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgvCategory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DgvCategory.Location = New System.Drawing.Point(0, 0)
        Me.DgvCategory.Name = "DgvCategory"
        Me.DgvCategory.Size = New System.Drawing.Size(511, 169)
        Me.DgvCategory.TabIndex = 0
        '
        'DgvMetal
        '
        Me.DgvMetal.AllowUserToAddRows = False
        Me.DgvMetal.AllowUserToDeleteRows = False
        Me.DgvMetal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgvMetal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DgvMetal.Location = New System.Drawing.Point(0, 0)
        Me.DgvMetal.Name = "DgvMetal"
        Me.DgvMetal.Size = New System.Drawing.Size(511, 169)
        Me.DgvMetal.TabIndex = 9
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtMetalWise)
        Me.Panel1.Controls.Add(Me.rbtCategorywise)
        Me.Panel1.Location = New System.Drawing.Point(120, 118)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(260, 28)
        Me.Panel1.TabIndex = 9
        '
        'rbtMetalWise
        '
        Me.rbtMetalWise.AutoSize = True
        Me.rbtMetalWise.Checked = True
        Me.rbtMetalWise.Location = New System.Drawing.Point(10, 4)
        Me.rbtMetalWise.Name = "rbtMetalWise"
        Me.rbtMetalWise.Size = New System.Drawing.Size(84, 17)
        Me.rbtMetalWise.TabIndex = 0
        Me.rbtMetalWise.TabStop = True
        Me.rbtMetalWise.Text = "Metal wise"
        Me.rbtMetalWise.UseVisualStyleBackColor = True
        '
        'rbtCategorywise
        '
        Me.rbtCategorywise.AutoSize = True
        Me.rbtCategorywise.Location = New System.Drawing.Point(103, 4)
        Me.rbtCategorywise.Name = "rbtCategorywise"
        Me.rbtCategorywise.Size = New System.Drawing.Size(107, 17)
        Me.rbtCategorywise.TabIndex = 1
        Me.rbtCategorywise.Text = "Category wise"
        Me.rbtCategorywise.UseVisualStyleBackColor = True
        '
        'ChkcmbCompany
        '
        Me.ChkcmbCompany.CheckOnClick = True
        Me.ChkcmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkcmbCompany.DropDownHeight = 1
        Me.ChkcmbCompany.FormattingEnabled = True
        Me.ChkcmbCompany.IntegralHeight = False
        Me.ChkcmbCompany.Location = New System.Drawing.Point(129, 64)
        Me.ChkcmbCompany.Name = "ChkcmbCompany"
        Me.ChkcmbCompany.Size = New System.Drawing.Size(319, 22)
        Me.ChkcmbCompany.TabIndex = 6
        Me.ChkcmbCompany.ValueSeparator = ", "
        '
        'chkOrderbyEmpId
        '
        Me.chkOrderbyEmpId.AutoSize = True
        Me.chkOrderbyEmpId.Location = New System.Drawing.Point(334, 31)
        Me.chkOrderbyEmpId.Name = "chkOrderbyEmpId"
        Me.chkOrderbyEmpId.Size = New System.Drawing.Size(118, 17)
        Me.chkOrderbyEmpId.TabIndex = 4
        Me.chkOrderbyEmpId.Text = "Order by EmpId"
        Me.chkOrderbyEmpId.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(56, 69)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Company"
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.Checked = True
        Me.rbtSummary.Location = New System.Drawing.Point(208, 95)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 8
        Me.rbtSummary.TabStop = True
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'rbtDetailed
        '
        Me.rbtDetailed.AutoSize = True
        Me.rbtDetailed.Location = New System.Drawing.Point(130, 95)
        Me.rbtDetailed.Name = "rbtDetailed"
        Me.rbtDetailed.Size = New System.Drawing.Size(72, 17)
        Me.rbtDetailed.TabIndex = 7
        Me.rbtDetailed.Text = "Detailed"
        Me.rbtDetailed.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(51, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Date From"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(219, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(21, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "To"
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(246, 29)
        Me.dtpTo.Mask = "##-##-####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(82, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "29/09/2010"
        Me.dtpTo.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(130, 29)
        Me.dtpFrom.Mask = "##-##-####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(82, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "29/09/2010"
        Me.dtpFrom.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(241, 362)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 12
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(29, 362)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 10
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(135, 362)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 11
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'FRM_SALES_COMMISION_COM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(755, 527)
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FRM_SALES_COMMISION_COM"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FRM_SALES_COMMISION_COM"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.DgvCategory, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgvMetal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents DgvMetal As System.Windows.Forms.DataGridView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents chkOrderbyEmpId As System.Windows.Forms.CheckBox
    Friend WithEvents ChkcmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtMetalWise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCategorywise As System.Windows.Forms.RadioButton
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents DgvCategory As System.Windows.Forms.DataGridView
    Friend WithEvents CmbSearch As System.Windows.Forms.ComboBox
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
End Class
