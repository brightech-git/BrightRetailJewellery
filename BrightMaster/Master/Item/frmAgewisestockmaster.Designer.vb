<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAgewisestockmaster
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
        Me.GRPfIELDS = New System.Windows.Forms.GroupBox
        Me.grpDesignOrder = New System.Windows.Forms.GroupBox
        Me.txtMinPcs = New System.Windows.Forms.TextBox
        Me.lblReOrderPce = New System.Windows.Forms.Label
        Me.txtMaxPiece = New System.Windows.Forms.TextBox
        Me.minLbl = New System.Windows.Forms.Label
        Me.btnDelete = New System.Windows.Forms.Button
        Me.pnlControls = New System.Windows.Forms.Panel
        Me.cmbDesignerName = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.cmbSize = New System.Windows.Forms.ComboBox
        Me.lblSize = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbSubItem_Man = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbItem_Man = New System.Windows.Forms.ComboBox
        Me.cmbMetal_Man = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cmbSubItemSearch_OWN = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.cmbItemSearch_OWN = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.cmbCostSearch_OWN = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NEwToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblStatus = New System.Windows.Forms.Label
        Me.GRPfIELDS.SuspendLayout()
        Me.grpDesignOrder.SuspendLayout()
        Me.pnlControls.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GRPfIELDS
        '
        Me.GRPfIELDS.BackColor = System.Drawing.Color.Transparent
        Me.GRPfIELDS.Controls.Add(Me.grpDesignOrder)
        Me.GRPfIELDS.Controls.Add(Me.btnDelete)
        Me.GRPfIELDS.Controls.Add(Me.pnlControls)
        Me.GRPfIELDS.Controls.Add(Me.btnExit)
        Me.GRPfIELDS.Controls.Add(Me.btnNew)
        Me.GRPfIELDS.Controls.Add(Me.btnOpen)
        Me.GRPfIELDS.Controls.Add(Me.btnSave)
        Me.GRPfIELDS.Location = New System.Drawing.Point(6, 8)
        Me.GRPfIELDS.Name = "GRPfIELDS"
        Me.GRPfIELDS.Size = New System.Drawing.Size(810, 182)
        Me.GRPfIELDS.TabIndex = 0
        Me.GRPfIELDS.TabStop = False
        '
        'grpDesignOrder
        '
        Me.grpDesignOrder.Controls.Add(Me.txtMinPcs)
        Me.grpDesignOrder.Controls.Add(Me.lblReOrderPce)
        Me.grpDesignOrder.Controls.Add(Me.txtMaxPiece)
        Me.grpDesignOrder.Controls.Add(Me.minLbl)
        Me.grpDesignOrder.Location = New System.Drawing.Point(5, 93)
        Me.grpDesignOrder.Name = "grpDesignOrder"
        Me.grpDesignOrder.Size = New System.Drawing.Size(398, 44)
        Me.grpDesignOrder.TabIndex = 1
        Me.grpDesignOrder.TabStop = False
        '
        'txtMinPcs
        '
        Me.txtMinPcs.Location = New System.Drawing.Point(107, 12)
        Me.txtMinPcs.MaxLength = 10
        Me.txtMinPcs.Name = "txtMinPcs"
        Me.txtMinPcs.Size = New System.Drawing.Size(99, 20)
        Me.txtMinPcs.TabIndex = 1
        '
        'lblReOrderPce
        '
        Me.lblReOrderPce.AutoSize = True
        Me.lblReOrderPce.Location = New System.Drawing.Point(6, 21)
        Me.lblReOrderPce.Name = "lblReOrderPce"
        Me.lblReOrderPce.Size = New System.Drawing.Size(52, 13)
        Me.lblReOrderPce.TabIndex = 0
        Me.lblReOrderPce.Text = "Day From"
        Me.lblReOrderPce.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMaxPiece
        '
        Me.txtMaxPiece.Location = New System.Drawing.Point(268, 12)
        Me.txtMaxPiece.MaxLength = 10
        Me.txtMaxPiece.Name = "txtMaxPiece"
        Me.txtMaxPiece.Size = New System.Drawing.Size(110, 20)
        Me.txtMaxPiece.TabIndex = 3
        '
        'minLbl
        '
        Me.minLbl.AutoSize = True
        Me.minLbl.Location = New System.Drawing.Point(225, 17)
        Me.minLbl.Name = "minLbl"
        Me.minLbl.Size = New System.Drawing.Size(20, 13)
        Me.minLbl.TabIndex = 2
        Me.minLbl.Text = "To"
        Me.minLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(544, 146)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 6
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'pnlControls
        '
        Me.pnlControls.Controls.Add(Me.cmbDesignerName)
        Me.pnlControls.Controls.Add(Me.Label9)
        Me.pnlControls.Controls.Add(Me.cmbSize)
        Me.pnlControls.Controls.Add(Me.lblSize)
        Me.pnlControls.Controls.Add(Me.Label1)
        Me.pnlControls.Controls.Add(Me.cmbSubItem_Man)
        Me.pnlControls.Controls.Add(Me.Label2)
        Me.pnlControls.Controls.Add(Me.cmbCostCentre)
        Me.pnlControls.Controls.Add(Me.Label4)
        Me.pnlControls.Controls.Add(Me.cmbItem_Man)
        Me.pnlControls.Controls.Add(Me.cmbMetal_Man)
        Me.pnlControls.Controls.Add(Me.Label3)
        Me.pnlControls.Location = New System.Drawing.Point(5, 11)
        Me.pnlControls.Name = "pnlControls"
        Me.pnlControls.Size = New System.Drawing.Size(799, 82)
        Me.pnlControls.TabIndex = 0
        '
        'cmbDesignerName
        '
        Me.cmbDesignerName.FormattingEnabled = True
        Me.cmbDesignerName.Location = New System.Drawing.Point(510, 55)
        Me.cmbDesignerName.Name = "cmbDesignerName"
        Me.cmbDesignerName.Size = New System.Drawing.Size(252, 21)
        Me.cmbDesignerName.TabIndex = 11
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(409, 58)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(80, 13)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "Designer Name"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSize
        '
        Me.cmbSize.FormattingEnabled = True
        Me.cmbSize.Location = New System.Drawing.Point(510, 30)
        Me.cmbSize.Name = "cmbSize"
        Me.cmbSize.Size = New System.Drawing.Size(252, 21)
        Me.cmbSize.TabIndex = 7
        '
        'lblSize
        '
        Me.lblSize.AutoSize = True
        Me.lblSize.Location = New System.Drawing.Point(409, 33)
        Me.lblSize.Name = "lblSize"
        Me.lblSize.Size = New System.Drawing.Size(27, 13)
        Me.lblSize.TabIndex = 6
        Me.lblSize.Text = "Size"
        Me.lblSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(409, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Metal"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSubItem_Man
        '
        Me.cmbSubItem_Man.FormattingEnabled = True
        Me.cmbSubItem_Man.Location = New System.Drawing.Point(107, 55)
        Me.cmbSubItem_Man.Name = "cmbSubItem_Man"
        Me.cmbSubItem_Man.Size = New System.Drawing.Size(271, 21)
        Me.cmbSubItem_Man.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(30, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Item "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(107, 5)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(271, 21)
        Me.cmbCostCentre.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Cost Centre"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItem_Man
        '
        Me.cmbItem_Man.FormattingEnabled = True
        Me.cmbItem_Man.Location = New System.Drawing.Point(107, 30)
        Me.cmbItem_Man.Name = "cmbItem_Man"
        Me.cmbItem_Man.Size = New System.Drawing.Size(271, 21)
        Me.cmbItem_Man.TabIndex = 5
        '
        'cmbMetal_Man
        '
        Me.cmbMetal_Man.FormattingEnabled = True
        Me.cmbMetal_Man.Location = New System.Drawing.Point(510, 4)
        Me.cmbMetal_Man.Name = "cmbMetal_Man"
        Me.cmbMetal_Man.Size = New System.Drawing.Size(252, 21)
        Me.cmbMetal_Man.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Sub Item"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(436, 146)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 5
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(328, 146)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 4
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(220, 146)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 3
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(112, 146)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 2
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbSubItemSearch_OWN)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.cmbItemSearch_OWN)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.cmbCostSearch_OWN)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 195)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(810, 69)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Search"
        '
        'cmbSubItemSearch_OWN
        '
        Me.cmbSubItemSearch_OWN.FormattingEnabled = True
        Me.cmbSubItemSearch_OWN.Items.AddRange(New Object() {"ALL"})
        Me.cmbSubItemSearch_OWN.Location = New System.Drawing.Point(390, 42)
        Me.cmbSubItemSearch_OWN.Name = "cmbSubItemSearch_OWN"
        Me.cmbSubItemSearch_OWN.Size = New System.Drawing.Size(272, 21)
        Me.cmbSubItemSearch_OWN.TabIndex = 5
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(475, 21)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "SubItem"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItemSearch_OWN
        '
        Me.cmbItemSearch_OWN.FormattingEnabled = True
        Me.cmbItemSearch_OWN.Location = New System.Drawing.Point(178, 42)
        Me.cmbItemSearch_OWN.Name = "cmbItemSearch_OWN"
        Me.cmbItemSearch_OWN.Size = New System.Drawing.Size(207, 21)
        Me.cmbItemSearch_OWN.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(240, 21)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(27, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Item"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCostSearch_OWN
        '
        Me.cmbCostSearch_OWN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCostSearch_OWN.FormattingEnabled = True
        Me.cmbCostSearch_OWN.Location = New System.Drawing.Point(14, 42)
        Me.cmbCostSearch_OWN.Name = "cmbCostSearch_OWN"
        Me.cmbCostSearch_OWN.Size = New System.Drawing.Size(159, 21)
        Me.cmbCostSearch_OWN.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(40, 21)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(62, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Cost Centre"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(667, 36)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 6
        Me.btnSearch.Text = "  Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(6, 269)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(810, 272)
        Me.gridView.TabIndex = 2
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NEwToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(131, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'NEwToolStripMenuItem
        '
        Me.NEwToolStripMenuItem.Name = "NEwToolStripMenuItem"
        Me.NEwToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NEwToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.NEwToolStripMenuItem.Text = "New"
        Me.NEwToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(8, 544)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(85, 13)
        Me.lblStatus.TabIndex = 29
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'frmAgewisestockmaster
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(831, 557)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.GRPfIELDS)
        Me.KeyPreview = True
        Me.Name = "frmAgewisestockmaster"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Piece Wise Stock ReOrder"
        Me.GRPfIELDS.ResumeLayout(False)
        Me.grpDesignOrder.ResumeLayout(False)
        Me.grpDesignOrder.PerformLayout()
        Me.pnlControls.ResumeLayout(False)
        Me.pnlControls.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GRPfIELDS As System.Windows.Forms.GroupBox
    Friend WithEvents grpDesignOrder As System.Windows.Forms.GroupBox
    Friend WithEvents cmbDesignerName As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtMinPcs As System.Windows.Forms.TextBox
    Friend WithEvents lblReOrderPce As System.Windows.Forms.Label
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents pnlControls As System.Windows.Forms.Panel
    Friend WithEvents cmbSize As System.Windows.Forms.ComboBox
    Friend WithEvents lblSize As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbSubItem_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbItem_Man As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMetal_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents txtMaxPiece As System.Windows.Forms.TextBox
    Friend WithEvents minLbl As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbSubItemSearch_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbItemSearch_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbCostSearch_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NEwToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblStatus As System.Windows.Forms.Label
End Class
