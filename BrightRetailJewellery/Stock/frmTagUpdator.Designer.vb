<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTagUpdator
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
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.grpContrainer = New System.Windows.Forms.GroupBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.rbtWTag = New System.Windows.Forms.RadioButton
        Me.rbtTag = New System.Windows.Forms.RadioButton
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.tabMode = New System.Windows.Forms.TabControl
        Me.tabitem = New System.Windows.Forms.TabPage
        Me.cmbtagtype = New BrighttechPack.CheckedComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.ChkCmbSubItem = New BrighttechPack.CheckedComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.ChkCmbItem = New BrighttechPack.CheckedComboBox
        Me.tabTagType = New System.Windows.Forms.TabPage
        Me.ChkCmbTagType = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.tabDesigner = New System.Windows.Forms.TabPage
        Me.Label8 = New System.Windows.Forms.Label
        Me.ChkCmbDesigner = New BrighttechPack.CheckedComboBox
        Me.ChkCmbSubItemDes = New BrighttechPack.CheckedComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.ChkCmbItemDes = New BrighttechPack.CheckedComboBox
        Me.tabTable = New System.Windows.Forms.TabPage
        Me.CmbTable = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbtNetWt = New System.Windows.Forms.RadioButton
        Me.rbtGrsWt = New System.Windows.Forms.RadioButton
        Me.tabView = New System.Windows.Forms.TabPage
        Me.pnlContainer_OWN = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.pnlFooter = New System.Windows.Forms.Panel
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlTag = New System.Windows.Forms.Panel
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.grpContrainer.SuspendLayout()
        Me.tabMode.SuspendLayout()
        Me.tabitem.SuspendLayout()
        Me.tabTagType.SuspendLayout()
        Me.tabDesigner.SuspendLayout()
        Me.tabTable.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.pnlContainer_OWN.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlFooter.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlTag.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1022, 640)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grpContrainer)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1014, 614)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "TabPage1"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'grpContrainer
        '
        Me.grpContrainer.Controls.Add(Me.Label10)
        Me.grpContrainer.Controls.Add(Me.cmbType)
        Me.grpContrainer.Controls.Add(Me.tabMode)
        Me.grpContrainer.Controls.Add(Me.btnExit)
        Me.grpContrainer.Controls.Add(Me.btnNew)
        Me.grpContrainer.Controls.Add(Me.btnSearch)
        Me.grpContrainer.Controls.Add(Me.Label2)
        Me.grpContrainer.Controls.Add(Me.Panel1)
        Me.grpContrainer.Controls.Add(Me.pnlTag)
        Me.grpContrainer.Location = New System.Drawing.Point(279, 168)
        Me.grpContrainer.Name = "grpContrainer"
        Me.grpContrainer.Size = New System.Drawing.Size(401, 282)
        Me.grpContrainer.TabIndex = 0
        Me.grpContrainer.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(26, 192)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(62, 13)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "Based On"
        '
        'rbtWTag
        '
        Me.rbtWTag.AutoSize = True
        Me.rbtWTag.Location = New System.Drawing.Point(55, 4)
        Me.rbtWTag.Name = "rbtWTag"
        Me.rbtWTag.Size = New System.Drawing.Size(71, 17)
        Me.rbtWTag.TabIndex = 1
        Me.rbtWTag.Text = "WebTag"
        Me.rbtWTag.UseVisualStyleBackColor = True
        '
        'rbtTag
        '
        Me.rbtTag.AutoSize = True
        Me.rbtTag.Checked = True
        Me.rbtTag.Location = New System.Drawing.Point(2, 4)
        Me.rbtTag.Name = "rbtTag"
        Me.rbtTag.Size = New System.Drawing.Size(46, 17)
        Me.rbtTag.TabIndex = 0
        Me.rbtTag.TabStop = True
        Me.rbtTag.Text = "Tag"
        Me.rbtTag.UseVisualStyleBackColor = True
        '
        'cmbType
        '
        Me.cmbType.FormattingEnabled = True
        Me.cmbType.Location = New System.Drawing.Point(99, 41)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(261, 21)
        Me.cmbType.TabIndex = 2
        '
        'tabMode
        '
        Me.tabMode.Controls.Add(Me.tabitem)
        Me.tabMode.Controls.Add(Me.tabTagType)
        Me.tabMode.Controls.Add(Me.tabDesigner)
        Me.tabMode.Controls.Add(Me.tabTable)
        Me.tabMode.ItemSize = New System.Drawing.Size(1, 15)
        Me.tabMode.Location = New System.Drawing.Point(6, 60)
        Me.tabMode.Name = "tabMode"
        Me.tabMode.SelectedIndex = 0
        Me.tabMode.Size = New System.Drawing.Size(378, 124)
        Me.tabMode.TabIndex = 3
        '
        'tabitem
        '
        Me.tabitem.Controls.Add(Me.cmbtagtype)
        Me.tabitem.Controls.Add(Me.Label7)
        Me.tabitem.Controls.Add(Me.Label5)
        Me.tabitem.Controls.Add(Me.ChkCmbSubItem)
        Me.tabitem.Controls.Add(Me.Label4)
        Me.tabitem.Controls.Add(Me.ChkCmbItem)
        Me.tabitem.Location = New System.Drawing.Point(4, 19)
        Me.tabitem.Name = "tabitem"
        Me.tabitem.Size = New System.Drawing.Size(370, 101)
        Me.tabitem.TabIndex = 2
        Me.tabitem.Text = "Item"
        Me.tabitem.UseVisualStyleBackColor = True
        '
        'cmbtagtype
        '
        Me.cmbtagtype.CheckOnClick = True
        Me.cmbtagtype.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbtagtype.DropDownHeight = 1
        Me.cmbtagtype.FormattingEnabled = True
        Me.cmbtagtype.IntegralHeight = False
        Me.cmbtagtype.Location = New System.Drawing.Point(90, 74)
        Me.cmbtagtype.Name = "cmbtagtype"
        Me.cmbtagtype.Size = New System.Drawing.Size(261, 22)
        Me.cmbtagtype.TabIndex = 9
        Me.cmbtagtype.ValueSeparator = ", "
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(19, 77)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(60, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Tag Type"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(19, 45)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 13)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "SubItem"
        '
        'ChkCmbSubItem
        '
        Me.ChkCmbSubItem.CheckOnClick = True
        Me.ChkCmbSubItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkCmbSubItem.DropDownHeight = 1
        Me.ChkCmbSubItem.FormattingEnabled = True
        Me.ChkCmbSubItem.IntegralHeight = False
        Me.ChkCmbSubItem.Location = New System.Drawing.Point(89, 42)
        Me.ChkCmbSubItem.Name = "ChkCmbSubItem"
        Me.ChkCmbSubItem.Size = New System.Drawing.Size(261, 22)
        Me.ChkCmbSubItem.TabIndex = 6
        Me.ChkCmbSubItem.ValueSeparator = ", "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(19, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Item"
        '
        'ChkCmbItem
        '
        Me.ChkCmbItem.CheckOnClick = True
        Me.ChkCmbItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkCmbItem.DropDownHeight = 1
        Me.ChkCmbItem.FormattingEnabled = True
        Me.ChkCmbItem.IntegralHeight = False
        Me.ChkCmbItem.Location = New System.Drawing.Point(89, 11)
        Me.ChkCmbItem.Name = "ChkCmbItem"
        Me.ChkCmbItem.Size = New System.Drawing.Size(261, 22)
        Me.ChkCmbItem.TabIndex = 4
        Me.ChkCmbItem.ValueSeparator = ", "
        '
        'tabTagType
        '
        Me.tabTagType.Controls.Add(Me.ChkCmbTagType)
        Me.tabTagType.Controls.Add(Me.Label3)
        Me.tabTagType.Location = New System.Drawing.Point(4, 19)
        Me.tabTagType.Name = "tabTagType"
        Me.tabTagType.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTagType.Size = New System.Drawing.Size(370, 101)
        Me.tabTagType.TabIndex = 3
        Me.tabTagType.Text = "TagType"
        Me.tabTagType.UseVisualStyleBackColor = True
        '
        'ChkCmbTagType
        '
        Me.ChkCmbTagType.FormattingEnabled = True
        Me.ChkCmbTagType.Location = New System.Drawing.Point(90, 16)
        Me.ChkCmbTagType.Name = "ChkCmbTagType"
        Me.ChkCmbTagType.Size = New System.Drawing.Size(261, 21)
        Me.ChkCmbTagType.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "TagType"
        '
        'tabDesigner
        '
        Me.tabDesigner.Controls.Add(Me.Label8)
        Me.tabDesigner.Controls.Add(Me.ChkCmbDesigner)
        Me.tabDesigner.Controls.Add(Me.ChkCmbSubItemDes)
        Me.tabDesigner.Controls.Add(Me.Label9)
        Me.tabDesigner.Controls.Add(Me.Label1)
        Me.tabDesigner.Controls.Add(Me.ChkCmbItemDes)
        Me.tabDesigner.Location = New System.Drawing.Point(4, 19)
        Me.tabDesigner.Name = "tabDesigner"
        Me.tabDesigner.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDesigner.Size = New System.Drawing.Size(370, 101)
        Me.tabDesigner.TabIndex = 4
        Me.tabDesigner.Text = "Designer"
        Me.tabDesigner.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(19, 77)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 13)
        Me.Label8.TabIndex = 11
        Me.Label8.Text = "SubItem"
        '
        'ChkCmbDesigner
        '
        Me.ChkCmbDesigner.CheckOnClick = True
        Me.ChkCmbDesigner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkCmbDesigner.DropDownHeight = 1
        Me.ChkCmbDesigner.FormattingEnabled = True
        Me.ChkCmbDesigner.IntegralHeight = False
        Me.ChkCmbDesigner.Location = New System.Drawing.Point(89, 14)
        Me.ChkCmbDesigner.Name = "ChkCmbDesigner"
        Me.ChkCmbDesigner.Size = New System.Drawing.Size(261, 22)
        Me.ChkCmbDesigner.TabIndex = 6
        Me.ChkCmbDesigner.ValueSeparator = ", "
        '
        'ChkCmbSubItemDes
        '
        Me.ChkCmbSubItemDes.CheckOnClick = True
        Me.ChkCmbSubItemDes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkCmbSubItemDes.DropDownHeight = 1
        Me.ChkCmbSubItemDes.FormattingEnabled = True
        Me.ChkCmbSubItemDes.IntegralHeight = False
        Me.ChkCmbSubItemDes.Location = New System.Drawing.Point(89, 72)
        Me.ChkCmbSubItemDes.Name = "ChkCmbSubItemDes"
        Me.ChkCmbSubItemDes.Size = New System.Drawing.Size(261, 22)
        Me.ChkCmbSubItemDes.TabIndex = 10
        Me.ChkCmbSubItemDes.ValueSeparator = ", "
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(19, 48)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(34, 13)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "Item"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Designer"
        '
        'ChkCmbItemDes
        '
        Me.ChkCmbItemDes.CheckOnClick = True
        Me.ChkCmbItemDes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkCmbItemDes.DropDownHeight = 1
        Me.ChkCmbItemDes.FormattingEnabled = True
        Me.ChkCmbItemDes.IntegralHeight = False
        Me.ChkCmbItemDes.Location = New System.Drawing.Point(89, 43)
        Me.ChkCmbItemDes.Name = "ChkCmbItemDes"
        Me.ChkCmbItemDes.Size = New System.Drawing.Size(261, 22)
        Me.ChkCmbItemDes.TabIndex = 8
        Me.ChkCmbItemDes.ValueSeparator = ", "
        '
        'tabTable
        '
        Me.tabTable.Controls.Add(Me.CmbTable)
        Me.tabTable.Controls.Add(Me.Label6)
        Me.tabTable.Location = New System.Drawing.Point(4, 19)
        Me.tabTable.Name = "tabTable"
        Me.tabTable.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTable.Size = New System.Drawing.Size(370, 101)
        Me.tabTable.TabIndex = 6
        Me.tabTable.Text = "Table"
        Me.tabTable.UseVisualStyleBackColor = True
        '
        'CmbTable
        '
        Me.CmbTable.FormattingEnabled = True
        Me.CmbTable.Location = New System.Drawing.Point(89, 15)
        Me.CmbTable.Name = "CmbTable"
        Me.CmbTable.Size = New System.Drawing.Size(261, 21)
        Me.CmbTable.TabIndex = 8
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(16, 18)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(38, 13)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "Table" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(261, 227)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 8
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(155, 227)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 7
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(49, 227)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 6
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(26, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Type"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtNetWt)
        Me.Panel1.Controls.Add(Me.rbtGrsWt)
        Me.Panel1.Location = New System.Drawing.Point(99, 186)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(167, 26)
        Me.Panel1.TabIndex = 4
        '
        'rbtNetWt
        '
        Me.rbtNetWt.AutoSize = True
        Me.rbtNetWt.Location = New System.Drawing.Point(69, 4)
        Me.rbtNetWt.Name = "rbtNetWt"
        Me.rbtNetWt.Size = New System.Drawing.Size(59, 17)
        Me.rbtNetWt.TabIndex = 7
        Me.rbtNetWt.Text = "NetWt"
        Me.rbtNetWt.UseVisualStyleBackColor = True
        '
        'rbtGrsWt
        '
        Me.rbtGrsWt.AutoSize = True
        Me.rbtGrsWt.Checked = True
        Me.rbtGrsWt.Location = New System.Drawing.Point(3, 4)
        Me.rbtGrsWt.Name = "rbtGrsWt"
        Me.rbtGrsWt.Size = New System.Drawing.Size(60, 17)
        Me.rbtGrsWt.TabIndex = 6
        Me.rbtGrsWt.TabStop = True
        Me.rbtGrsWt.Text = "GrsWt"
        Me.rbtGrsWt.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlContainer_OWN)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1014, 614)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "TabPage2"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'pnlContainer_OWN
        '
        Me.pnlContainer_OWN.Controls.Add(Me.gridView)
        Me.pnlContainer_OWN.Controls.Add(Me.pnlFooter)
        Me.pnlContainer_OWN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContainer_OWN.Location = New System.Drawing.Point(3, 3)
        Me.pnlContainer_OWN.Name = "pnlContainer_OWN"
        Me.pnlContainer_OWN.Size = New System.Drawing.Size(1008, 608)
        Me.pnlContainer_OWN.TabIndex = 0
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 39)
        Me.gridView.Name = "gridView"
        Me.gridView.Size = New System.Drawing.Size(1008, 569)
        Me.gridView.TabIndex = 0
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.btnUpdate)
        Me.pnlFooter.Controls.Add(Me.btnBack)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlFooter.Location = New System.Drawing.Point(0, 0)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(1008, 39)
        Me.pnlFooter.TabIndex = 1
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(111, 5)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(100, 30)
        Me.btnUpdate.TabIndex = 1
        Me.btnUpdate.Text = "&Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(5, 5)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 0
        Me.btnBack.Text = "&Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'pnlTag
        '
        Me.pnlTag.Controls.Add(Me.rbtWTag)
        Me.pnlTag.Controls.Add(Me.rbtTag)
        Me.pnlTag.Location = New System.Drawing.Point(99, 14)
        Me.pnlTag.Name = "pnlTag"
        Me.pnlTag.Size = New System.Drawing.Size(169, 25)
        Me.pnlTag.TabIndex = 0
        '
        'frmTagUpdator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmTagUpdator"
        Me.Text = "TagUpdator"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.grpContrainer.ResumeLayout(False)
        Me.grpContrainer.PerformLayout()
        Me.tabMode.ResumeLayout(False)
        Me.tabitem.ResumeLayout(False)
        Me.tabitem.PerformLayout()
        Me.tabTagType.ResumeLayout(False)
        Me.tabTagType.PerformLayout()
        Me.tabDesigner.ResumeLayout(False)
        Me.tabDesigner.PerformLayout()
        Me.tabTable.ResumeLayout(False)
        Me.tabTable.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.pnlContainer_OWN.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlFooter.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlTag.ResumeLayout(False)
        Me.pnlTag.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents grpContrainer As System.Windows.Forms.GroupBox
    Friend WithEvents tabMode As System.Windows.Forms.TabControl
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlContainer_OWN As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tabitem As System.Windows.Forms.TabPage
    Friend WithEvents tabTagType As System.Windows.Forms.TabPage
    Friend WithEvents tabDesigner As System.Windows.Forms.TabPage
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents tabTable As System.Windows.Forms.TabPage
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ChkCmbSubItem As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ChkCmbItem As BrighttechPack.CheckedComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ChkCmbDesigner As BrighttechPack.CheckedComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents CmbTable As System.Windows.Forms.ComboBox
    Friend WithEvents ChkCmbTagType As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmbtagtype As BrighttechPack.CheckedComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ChkCmbSubItemDes As BrighttechPack.CheckedComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ChkCmbItemDes As BrighttechPack.CheckedComboBox
    Friend WithEvents rbtWTag As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTag As System.Windows.Forms.RadioButton
    Friend WithEvents rbtNetWt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGrsWt As System.Windows.Forms.RadioButton
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents pnlTag As System.Windows.Forms.Panel
End Class
