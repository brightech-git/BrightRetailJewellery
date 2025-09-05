<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCatLogItemDetail
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCatLogItemDetail))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkSelect = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbMetal = New BrighttechPack.CheckedComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkTag = New System.Windows.Forms.CheckBox()
        Me.ChkNontag = New System.Windows.Forms.CheckBox()
        Me.CmbStockType = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.CmbDesigner = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbSearchKey = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtTagNo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkRecDate = New System.Windows.Forms.CheckBox()
        Me.dtpDate = New BrighttechPack.DatePicker(Me.components)
        Me.txtItemId = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnTransfer = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.gridView_OWN = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.rbtAll = New System.Windows.Forms.RadioButton()
        Me.rbtMaster = New System.Windows.Forms.RadioButton()
        Me.rbtItemDetail = New System.Windows.Forms.RadioButton()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.cmbGridShortCut.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtItemDetail)
        Me.Panel1.Controls.Add(Me.rbtMaster)
        Me.Panel1.Controls.Add(Me.rbtAll)
        Me.Panel1.Controls.Add(Me.chkSelect)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.cmbMetal)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.chkTag)
        Me.Panel1.Controls.Add(Me.ChkNontag)
        Me.Panel1.Controls.Add(Me.CmbStockType)
        Me.Panel1.Controls.Add(Me.Label14)
        Me.Panel1.Controls.Add(Me.CmbDesigner)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.txtSearch)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.cmbSearchKey)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.txtTagNo)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.chkRecDate)
        Me.Panel1.Controls.Add(Me.dtpDate)
        Me.Panel1.Controls.Add(Me.txtItemId)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.btnTransfer)
        Me.Panel1.Controls.Add(Me.btnExit)
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.Name = "Panel1"
        '
        'chkSelect
        '
        resources.ApplyResources(Me.chkSelect, "chkSelect")
        Me.chkSelect.Name = "chkSelect"
        Me.chkSelect.UseVisualStyleBackColor = True
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'cmbMetal
        '
        Me.cmbMetal.CheckOnClick = True
        Me.cmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbMetal.DropDownHeight = 1
        Me.cmbMetal.FormattingEnabled = True
        resources.ApplyResources(Me.cmbMetal, "cmbMetal")
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.ValueSeparator = ", "
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'chkTag
        '
        resources.ApplyResources(Me.chkTag, "chkTag")
        Me.chkTag.Checked = True
        Me.chkTag.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTag.Name = "chkTag"
        Me.chkTag.UseVisualStyleBackColor = True
        '
        'ChkNontag
        '
        resources.ApplyResources(Me.ChkNontag, "ChkNontag")
        Me.ChkNontag.Name = "ChkNontag"
        Me.ChkNontag.UseVisualStyleBackColor = True
        '
        'CmbStockType
        '
        Me.CmbStockType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbStockType.FormattingEnabled = True
        Me.CmbStockType.Items.AddRange(New Object() {resources.GetString("CmbStockType.Items"), resources.GetString("CmbStockType.Items1"), resources.GetString("CmbStockType.Items2"), resources.GetString("CmbStockType.Items3")})
        resources.ApplyResources(Me.CmbStockType, "CmbStockType")
        Me.CmbStockType.Name = "CmbStockType"
        '
        'Label14
        '
        resources.ApplyResources(Me.Label14, "Label14")
        Me.Label14.Name = "Label14"
        '
        'CmbDesigner
        '
        Me.CmbDesigner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbDesigner.FormattingEnabled = True
        resources.ApplyResources(Me.CmbDesigner, "CmbDesigner")
        Me.CmbDesigner.Name = "CmbDesigner"
        '
        'Label13
        '
        resources.ApplyResources(Me.Label13, "Label13")
        Me.Label13.Name = "Label13"
        '
        'txtSearch
        '
        resources.ApplyResources(Me.txtSearch, "txtSearch")
        Me.txtSearch.Name = "txtSearch"
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        '
        'cmbSearchKey
        '
        Me.cmbSearchKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSearchKey.FormattingEnabled = True
        resources.ApplyResources(Me.cmbSearchKey, "cmbSearchKey")
        Me.cmbSearchKey.Name = "cmbSearchKey"
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'txtTagNo
        '
        resources.ApplyResources(Me.txtTagNo, "txtTagNo")
        Me.txtTagNo.Name = "txtTagNo"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'chkRecDate
        '
        resources.ApplyResources(Me.chkRecDate, "chkRecDate")
        Me.chkRecDate.Name = "chkRecDate"
        Me.chkRecDate.UseVisualStyleBackColor = True
        '
        'dtpDate
        '
        resources.ApplyResources(Me.dtpDate, "dtpDate")
        Me.dtpDate.Mask = "##/##/####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'txtItemId
        '
        resources.ApplyResources(Me.txtItemId, "txtItemId")
        Me.txtItemId.Name = "txtItemId"
        '
        'btnSearch
        '
        resources.ApplyResources(Me.btnSearch, "btnSearch")
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        resources.ApplyResources(Me.btnPrint, "btnPrint")
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        resources.ApplyResources(Me.btnNew, "btnNew")
        Me.btnNew.Name = "btnNew"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        resources.ApplyResources(Me.btnExport, "btnExport")
        Me.btnExport.Name = "btnExport"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnTransfer
        '
        resources.ApplyResources(Me.btnTransfer, "btnTransfer")
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        resources.ApplyResources(Me.btnExit, "btnExit")
        Me.btnExit.Name = "btnExit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridView_OWN)
        resources.ApplyResources(Me.Panel2, "Panel2")
        Me.Panel2.Name = "Panel2"
        '
        'gridView_OWN
        '
        Me.gridView_OWN.AllowUserToAddRows = False
        Me.gridView_OWN.AllowUserToDeleteRows = False
        Me.gridView_OWN.AllowUserToResizeColumns = False
        Me.gridView_OWN.AllowUserToResizeRows = False
        Me.gridView_OWN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        resources.ApplyResources(Me.gridView_OWN, "gridView_OWN")
        Me.gridView_OWN.MultiSelect = False
        Me.gridView_OWN.Name = "gridView_OWN"
        Me.gridView_OWN.RowHeadersVisible = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        resources.ApplyResources(Me.ContextMenuStrip1, "ContextMenuStrip1")
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        resources.ApplyResources(Me.NewToolStripMenuItem, "NewToolStripMenuItem")
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        resources.ApplyResources(Me.ExitToolStripMenuItem, "ExitToolStripMenuItem")
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        resources.ApplyResources(Me.cmbGridShortCut, "cmbGridShortCut")
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        resources.ApplyResources(Me.ResizeToolStripMenuItem, "ResizeToolStripMenuItem")
        '
        'rbtAll
        '
        resources.ApplyResources(Me.rbtAll, "rbtAll")
        Me.rbtAll.Checked = True
        Me.rbtAll.Name = "rbtAll"
        Me.rbtAll.TabStop = True
        Me.rbtAll.UseVisualStyleBackColor = True
        '
        'rbtMaster
        '
        resources.ApplyResources(Me.rbtMaster, "rbtMaster")
        Me.rbtMaster.Name = "rbtMaster"
        Me.rbtMaster.UseVisualStyleBackColor = True
        '
        'rbtItemDetail
        '
        resources.ApplyResources(Me.rbtItemDetail, "rbtItemDetail")
        Me.rbtItemDetail.Name = "rbtItemDetail"
        Me.rbtItemDetail.UseVisualStyleBackColor = True
        '
        'frmCatLogItemDetail
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmCatLogItemDetail"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents gridView_OWN As DataGridView
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnPrint As Button
    Friend WithEvents btnNew As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents btnTransfer As Button
    Friend WithEvents btnExit As Button
    Friend WithEvents CmbStockType As ComboBox
    Friend WithEvents Label14 As Label
    Friend WithEvents CmbDesigner As ComboBox
    Friend WithEvents Label13 As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents cmbSearchKey As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtTagNo As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents chkRecDate As CheckBox
    Friend WithEvents dtpDate As BrighttechPack.DatePicker
    Friend WithEvents txtItemId As TextBox
    Friend WithEvents chkTag As CheckBox
    Friend WithEvents ChkNontag As CheckBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents cmbGridShortCut As ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Label4 As Label
    Friend WithEvents cmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents chkSelect As CheckBox
    Friend WithEvents rbtItemDetail As RadioButton
    Friend WithEvents rbtMaster As RadioButton
    Friend WithEvents rbtAll As RadioButton
End Class
