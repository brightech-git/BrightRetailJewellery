<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPMValidation
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
        Me.GRPfIELDS = New System.Windows.Forms.GroupBox
        Me.pnlControls = New System.Windows.Forms.Panel
        Me.Label10 = New System.Windows.Forms.Label
        Me.cmbComItem_Man = New System.Windows.Forms.ComboBox
        Me.txtToAmt = New System.Windows.Forms.TextBox
        Me.txtFromAmt = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.cmbSubItem_Man = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbCostCentre_own = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbItem_OWN = New System.Windows.Forms.ComboBox
        Me.cmbMetal_Man = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblWtFrom = New System.Windows.Forms.Label
        Me.lblWt = New System.Windows.Forms.Label
        Me.txtWtFrom = New System.Windows.Forms.TextBox
        Me.txtWtTo = New System.Windows.Forms.TextBox
        Me.lblWtTo = New System.Windows.Forms.Label
        Me.minLbl = New System.Windows.Forms.Label
        Me.txtPieceTo = New System.Windows.Forms.TextBox
        Me.txtPieceFrom = New System.Windows.Forms.TextBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NEwToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblStatus = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cmbSubItemSearch_OWN = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.cmbItemSearch_OWN = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.cmbCostSearch_OWN = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GRPfIELDS.SuspendLayout()
        Me.pnlControls.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(12, 269)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(810, 272)
        Me.gridView.TabIndex = 2
        '
        'GRPfIELDS
        '
        Me.GRPfIELDS.BackColor = System.Drawing.Color.Transparent
        Me.GRPfIELDS.Controls.Add(Me.pnlControls)
        Me.GRPfIELDS.Controls.Add(Me.btnExit)
        Me.GRPfIELDS.Controls.Add(Me.btnNew)
        Me.GRPfIELDS.Controls.Add(Me.btnOpen)
        Me.GRPfIELDS.Controls.Add(Me.btnSave)
        Me.GRPfIELDS.Controls.Add(Me.btnDelete)
        Me.GRPfIELDS.Location = New System.Drawing.Point(12, 4)
        Me.GRPfIELDS.Name = "GRPfIELDS"
        Me.GRPfIELDS.Size = New System.Drawing.Size(810, 172)
        Me.GRPfIELDS.TabIndex = 0
        Me.GRPfIELDS.TabStop = False
        '
        'pnlControls
        '
        Me.pnlControls.Controls.Add(Me.Label10)
        Me.pnlControls.Controls.Add(Me.cmbComItem_Man)
        Me.pnlControls.Controls.Add(Me.txtToAmt)
        Me.pnlControls.Controls.Add(Me.txtFromAmt)
        Me.pnlControls.Controls.Add(Me.Label1)
        Me.pnlControls.Controls.Add(Me.Label7)
        Me.pnlControls.Controls.Add(Me.cmbSubItem_Man)
        Me.pnlControls.Controls.Add(Me.Label9)
        Me.pnlControls.Controls.Add(Me.Label2)
        Me.pnlControls.Controls.Add(Me.cmbCostCentre_own)
        Me.pnlControls.Controls.Add(Me.Label4)
        Me.pnlControls.Controls.Add(Me.cmbItem_OWN)
        Me.pnlControls.Controls.Add(Me.cmbMetal_Man)
        Me.pnlControls.Controls.Add(Me.Label3)
        Me.pnlControls.Controls.Add(Me.lblWtFrom)
        Me.pnlControls.Controls.Add(Me.lblWt)
        Me.pnlControls.Controls.Add(Me.txtWtFrom)
        Me.pnlControls.Controls.Add(Me.txtWtTo)
        Me.pnlControls.Controls.Add(Me.lblWtTo)
        Me.pnlControls.Controls.Add(Me.minLbl)
        Me.pnlControls.Controls.Add(Me.txtPieceTo)
        Me.pnlControls.Controls.Add(Me.txtPieceFrom)
        Me.pnlControls.Location = New System.Drawing.Point(5, 15)
        Me.pnlControls.Name = "pnlControls"
        Me.pnlControls.Size = New System.Drawing.Size(799, 115)
        Me.pnlControls.TabIndex = 0
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(16, 90)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(107, 13)
        Me.Label10.TabIndex = 18
        Me.Label10.Text = "Compliment Item"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbComItem_Man
        '
        Me.cmbComItem_Man.FormattingEnabled = True
        Me.cmbComItem_Man.Location = New System.Drawing.Point(128, 87)
        Me.cmbComItem_Man.Name = "cmbComItem_Man"
        Me.cmbComItem_Man.Size = New System.Drawing.Size(252, 21)
        Me.cmbComItem_Man.TabIndex = 19
        '
        'txtToAmt
        '
        Me.txtToAmt.Location = New System.Drawing.Point(651, 61)
        Me.txtToAmt.MaxLength = 10
        Me.txtToAmt.Name = "txtToAmt"
        Me.txtToAmt.Size = New System.Drawing.Size(110, 21)
        Me.txtToAmt.TabIndex = 17
        '
        'txtFromAmt
        '
        Me.txtFromAmt.Location = New System.Drawing.Point(510, 62)
        Me.txtFromAmt.MaxLength = 10
        Me.txtFromAmt.Name = "txtFromAmt"
        Me.txtFromAmt.Size = New System.Drawing.Size(110, 21)
        Me.txtFromAmt.TabIndex = 15
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(50, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Metal"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(409, 67)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(84, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "From Amount"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSubItem_Man
        '
        Me.cmbSubItem_Man.FormattingEnabled = True
        Me.cmbSubItem_Man.Location = New System.Drawing.Point(510, 87)
        Me.cmbSubItem_Man.Name = "cmbSubItem_Man"
        Me.cmbSubItem_Man.Size = New System.Drawing.Size(252, 21)
        Me.cmbSubItem_Man.TabIndex = 21
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(624, 85)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(21, 13)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "To"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(24, 62)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Sale Item"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCostCentre_own
        '
        Me.cmbCostCentre_own.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCostCentre_own.FormattingEnabled = True
        Me.cmbCostCentre_own.Location = New System.Drawing.Point(128, 11)
        Me.cmbCostCentre_own.Name = "cmbCostCentre_own"
        Me.cmbCostCentre_own.Size = New System.Drawing.Size(252, 21)
        Me.cmbCostCentre_own.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 13)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "Cost Centre"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItem_OWN
        '
        Me.cmbItem_OWN.FormattingEnabled = True
        Me.cmbItem_OWN.Location = New System.Drawing.Point(128, 59)
        Me.cmbItem_OWN.Name = "cmbItem_OWN"
        Me.cmbItem_OWN.Size = New System.Drawing.Size(252, 21)
        Me.cmbItem_OWN.TabIndex = 5
        '
        'cmbMetal_Man
        '
        Me.cmbMetal_Man.FormattingEnabled = True
        Me.cmbMetal_Man.Location = New System.Drawing.Point(128, 34)
        Me.cmbMetal_Man.Name = "cmbMetal_Man"
        Me.cmbMetal_Man.Size = New System.Drawing.Size(252, 21)
        Me.cmbMetal_Man.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(382, 90)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(129, 13)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Compliment SubItem"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblWtFrom
        '
        Me.lblWtFrom.AutoSize = True
        Me.lblWtFrom.Location = New System.Drawing.Point(409, 15)
        Me.lblWtFrom.Name = "lblWtFrom"
        Me.lblWtFrom.Size = New System.Drawing.Size(79, 13)
        Me.lblWtFrom.TabIndex = 6
        Me.lblWtFrom.Text = "From Weight"
        Me.lblWtFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblWt
        '
        Me.lblWt.AutoSize = True
        Me.lblWt.Location = New System.Drawing.Point(624, 39)
        Me.lblWt.Name = "lblWt"
        Me.lblWt.Size = New System.Drawing.Size(21, 13)
        Me.lblWt.TabIndex = 12
        Me.lblWt.Text = "To"
        Me.lblWt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtWtFrom
        '
        Me.txtWtFrom.Location = New System.Drawing.Point(510, 10)
        Me.txtWtFrom.MaxLength = 10
        Me.txtWtFrom.Name = "txtWtFrom"
        Me.txtWtFrom.Size = New System.Drawing.Size(110, 21)
        Me.txtWtFrom.TabIndex = 7
        '
        'txtWtTo
        '
        Me.txtWtTo.Location = New System.Drawing.Point(651, 9)
        Me.txtWtTo.MaxLength = 10
        Me.txtWtTo.Name = "txtWtTo"
        Me.txtWtTo.Size = New System.Drawing.Size(111, 21)
        Me.txtWtTo.TabIndex = 9
        '
        'lblWtTo
        '
        Me.lblWtTo.AutoSize = True
        Me.lblWtTo.Location = New System.Drawing.Point(625, 14)
        Me.lblWtTo.Name = "lblWtTo"
        Me.lblWtTo.Size = New System.Drawing.Size(21, 13)
        Me.lblWtTo.TabIndex = 8
        Me.lblWtTo.Text = "To"
        Me.lblWtTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'minLbl
        '
        Me.minLbl.AutoSize = True
        Me.minLbl.Location = New System.Drawing.Point(409, 40)
        Me.minLbl.Name = "minLbl"
        Me.minLbl.Size = New System.Drawing.Size(70, 13)
        Me.minLbl.TabIndex = 10
        Me.minLbl.Text = "From Piece"
        Me.minLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPieceTo
        '
        Me.txtPieceTo.Location = New System.Drawing.Point(651, 34)
        Me.txtPieceTo.MaxLength = 10
        Me.txtPieceTo.Name = "txtPieceTo"
        Me.txtPieceTo.Size = New System.Drawing.Size(110, 21)
        Me.txtPieceTo.TabIndex = 13
        '
        'txtPieceFrom
        '
        Me.txtPieceFrom.Location = New System.Drawing.Point(510, 35)
        Me.txtPieceFrom.MaxLength = 10
        Me.txtPieceFrom.Name = "txtPieceFrom"
        Me.txtPieceFrom.Size = New System.Drawing.Size(110, 21)
        Me.txtPieceFrom.TabIndex = 11
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(436, 136)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 3
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(328, 136)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 2
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(220, 136)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 1
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(112, 136)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(544, 136)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 4
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NEwToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'NEwToolStripMenuItem
        '
        Me.NEwToolStripMenuItem.Name = "NEwToolStripMenuItem"
        Me.NEwToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NEwToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.NEwToolStripMenuItem.Text = "New"
        Me.NEwToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(12, 549)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 25
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
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
        Me.GroupBox1.Location = New System.Drawing.Point(12, 180)
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
        Me.Label8.Size = New System.Drawing.Size(129, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Compliment SubItem"
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
        Me.Label6.Size = New System.Drawing.Size(113, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Compliment Name"
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
        Me.Label5.Size = New System.Drawing.Size(76, 13)
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
        'frmPMValidation
        '
        Me.AccessibleDescription = "frmPMValidation"
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(833, 567)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GRPfIELDS)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.gridView)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmPMValidation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Compliment Validation "
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GRPfIELDS.ResumeLayout(False)
        Me.pnlControls.ResumeLayout(False)
        Me.pnlControls.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtWtFrom As System.Windows.Forms.TextBox
    Friend WithEvents cmbCostCentre_own As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubItem_Man As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItem_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents lblWt As System.Windows.Forms.Label
    Friend WithEvents minLbl As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblWtTo As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblWtFrom As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GRPfIELDS As System.Windows.Forms.GroupBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents txtPieceTo As System.Windows.Forms.TextBox
    Friend WithEvents txtPieceFrom As System.Windows.Forms.TextBox
    Friend WithEvents txtWtTo As System.Windows.Forms.TextBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NEwToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlControls As System.Windows.Forms.Panel
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbItemSearch_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbCostSearch_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubItemSearch_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents txtToAmt As System.Windows.Forms.TextBox
    Friend WithEvents txtFromAmt As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cmbMetal_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cmbComItem_Man As System.Windows.Forms.ComboBox
End Class
