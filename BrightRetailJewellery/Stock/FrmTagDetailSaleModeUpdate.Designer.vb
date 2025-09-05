<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmTagDetailSaleModeUpdate
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.grpContrainer = New System.Windows.Forms.GroupBox
        Me.CmbNewSaleMode = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label26 = New System.Windows.Forms.Label
        Me.chkcmbcounter = New BrighttechPack.CheckedComboBox
        Me.CmbOldSaleMode = New System.Windows.Forms.ComboBox
        Me.cmbSubItem_MAN = New System.Windows.Forms.ComboBox
        Me.cmbItem = New System.Windows.Forms.ComboBox
        Me.chkDesigner = New System.Windows.Forms.CheckBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.chkLstDesigner = New System.Windows.Forms.CheckedListBox
        Me.tabView = New System.Windows.Forms.TabPage
        Me.pnlContainer_OWN = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.pnlFooter = New System.Windows.Forms.Panel
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.grpContrainer.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.pnlContainer_OWN.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlFooter.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
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
        Me.tabMain.Size = New System.Drawing.Size(968, 720)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grpContrainer)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(960, 694)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "TabPage1"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'grpContrainer
        '
        Me.grpContrainer.Controls.Add(Me.CmbNewSaleMode)
        Me.grpContrainer.Controls.Add(Me.Label4)
        Me.grpContrainer.Controls.Add(Me.Label26)
        Me.grpContrainer.Controls.Add(Me.chkcmbcounter)
        Me.grpContrainer.Controls.Add(Me.CmbOldSaleMode)
        Me.grpContrainer.Controls.Add(Me.cmbSubItem_MAN)
        Me.grpContrainer.Controls.Add(Me.cmbItem)
        Me.grpContrainer.Controls.Add(Me.chkDesigner)
        Me.grpContrainer.Controls.Add(Me.btnExit)
        Me.grpContrainer.Controls.Add(Me.btnNew)
        Me.grpContrainer.Controls.Add(Me.btnSearch)
        Me.grpContrainer.Controls.Add(Me.Label3)
        Me.grpContrainer.Controls.Add(Me.Label1)
        Me.grpContrainer.Controls.Add(Me.Label2)
        Me.grpContrainer.Controls.Add(Me.chkLstDesigner)
        Me.grpContrainer.Location = New System.Drawing.Point(188, 86)
        Me.grpContrainer.Name = "grpContrainer"
        Me.grpContrainer.Size = New System.Drawing.Size(417, 410)
        Me.grpContrainer.TabIndex = 0
        Me.grpContrainer.TabStop = False
        '
        'CmbNewSaleMode
        '
        Me.CmbNewSaleMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbNewSaleMode.FormattingEnabled = True
        Me.CmbNewSaleMode.Items.AddRange(New Object() {"WEIGHT", "RATE", "FIXED", "METAL RATE"})
        Me.CmbNewSaleMode.Location = New System.Drawing.Point(101, 264)
        Me.CmbNewSaleMode.Name = "CmbNewSaleMode"
        Me.CmbNewSaleMode.Size = New System.Drawing.Size(153, 21)
        Me.CmbNewSaleMode.TabIndex = 11
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 267)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(83, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "New Sale Mode"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(7, 82)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(44, 13)
        Me.Label26.TabIndex = 4
        Me.Label26.Text = "Counter"
        '
        'chkcmbcounter
        '
        Me.chkcmbcounter.CheckOnClick = True
        Me.chkcmbcounter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbcounter.DropDownHeight = 1
        Me.chkcmbcounter.FormattingEnabled = True
        Me.chkcmbcounter.IntegralHeight = False
        Me.chkcmbcounter.Location = New System.Drawing.Point(101, 81)
        Me.chkcmbcounter.Name = "chkcmbcounter"
        Me.chkcmbcounter.Size = New System.Drawing.Size(261, 21)
        Me.chkcmbcounter.TabIndex = 5
        Me.chkcmbcounter.ValueSeparator = ", "
        '
        'CmbOldSaleMode
        '
        Me.CmbOldSaleMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbOldSaleMode.FormattingEnabled = True
        Me.CmbOldSaleMode.Items.AddRange(New Object() {"WEIGHT", "RATE", "FIXED", "METAL RATE"})
        Me.CmbOldSaleMode.Location = New System.Drawing.Point(102, 229)
        Me.CmbOldSaleMode.Name = "CmbOldSaleMode"
        Me.CmbOldSaleMode.Size = New System.Drawing.Size(152, 21)
        Me.CmbOldSaleMode.TabIndex = 9
        '
        'cmbSubItem_MAN
        '
        Me.cmbSubItem_MAN.FormattingEnabled = True
        Me.cmbSubItem_MAN.Location = New System.Drawing.Point(101, 50)
        Me.cmbSubItem_MAN.Name = "cmbSubItem_MAN"
        Me.cmbSubItem_MAN.Size = New System.Drawing.Size(261, 21)
        Me.cmbSubItem_MAN.TabIndex = 3
        '
        'cmbItem
        '
        Me.cmbItem.FormattingEnabled = True
        Me.cmbItem.Location = New System.Drawing.Point(101, 19)
        Me.cmbItem.Name = "cmbItem"
        Me.cmbItem.Size = New System.Drawing.Size(261, 21)
        Me.cmbItem.TabIndex = 1
        '
        'chkDesigner
        '
        Me.chkDesigner.AutoSize = True
        Me.chkDesigner.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDesigner.Location = New System.Drawing.Point(6, 112)
        Me.chkDesigner.Name = "chkDesigner"
        Me.chkDesigner.Size = New System.Drawing.Size(68, 17)
        Me.chkDesigner.TabIndex = 6
        Me.chkDesigner.Text = "Designer"
        Me.chkDesigner.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(232, 306)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 14
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(126, 306)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 13
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(20, 306)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 12
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 53)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Sub Item"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 232)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Old Sale Mode"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(27, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Item"
        '
        'chkLstDesigner
        '
        Me.chkLstDesigner.FormattingEnabled = True
        Me.chkLstDesigner.Location = New System.Drawing.Point(102, 110)
        Me.chkLstDesigner.Name = "chkLstDesigner"
        Me.chkLstDesigner.Size = New System.Drawing.Size(262, 109)
        Me.chkLstDesigner.TabIndex = 7
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlContainer_OWN)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(960, 694)
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
        Me.pnlContainer_OWN.Size = New System.Drawing.Size(954, 688)
        Me.pnlContainer_OWN.TabIndex = 0
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.DefaultCellStyle = DataGridViewCellStyle2
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 39)
        Me.gridView.Name = "gridView"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.gridView.Size = New System.Drawing.Size(954, 649)
        Me.gridView.TabIndex = 0
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.btnUpdate)
        Me.pnlFooter.Controls.Add(Me.btnBack)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlFooter.Location = New System.Drawing.Point(0, 0)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(954, 39)
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
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'FrmTagDetailSaleModeUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(968, 720)
        Me.Controls.Add(Me.tabMain)
        Me.Name = "FrmTagDetailSaleModeUpdate"
        Me.Text = "FrmTagDetailSaleModeUpdate"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.grpContrainer.ResumeLayout(False)
        Me.grpContrainer.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.pnlContainer_OWN.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlFooter.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents grpContrainer As System.Windows.Forms.GroupBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents chkcmbcounter As BrighttechPack.CheckedComboBox
    Friend WithEvents CmbOldSaleMode As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubItem_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItem As System.Windows.Forms.ComboBox
    Friend WithEvents chkDesigner As System.Windows.Forms.CheckBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkLstDesigner As System.Windows.Forms.CheckedListBox
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents pnlContainer_OWN As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CmbNewSaleMode As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
