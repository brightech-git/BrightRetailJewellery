<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmItemCounter
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbActive = New System.Windows.Forms.ComboBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.txtTarget_Amt = New System.Windows.Forms.TextBox()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.txtCounterGroup = New System.Windows.Forms.TextBox()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.txtCounterName__Man = New System.Windows.Forms.TextBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.txtCounterId_Num_Man = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtDisplayOrder_NUM = New System.Windows.Forms.TextBox()
        Me.txtWeight_WET = New System.Windows.Forms.TextBox()
        Me.txtPiece_NUM = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblctr = New System.Windows.Forms.Label()
        Me.txt_POS_ITEMID = New System.Windows.Forms.TextBox()
        Me.lblctr1 = New System.Windows.Forms.Label()
        Me.CmbCounterGrp = New System.Windows.Forms.ComboBox()
        Me.txtTagWt_WET = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtCoverWt_WET = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.pnlExtra = New System.Windows.Forms.Panel()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.cmbTrfCounter = New System.Windows.Forms.ComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtCounterSHName = New System.Windows.Forms.TextBox()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlExtra.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(12, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Counter Id"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbActive
        '
        Me.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActive.FormattingEnabled = True
        Me.cmbActive.Location = New System.Drawing.Point(126, 65)
        Me.cmbActive.Name = "cmbActive"
        Me.cmbActive.Size = New System.Drawing.Size(92, 21)
        Me.cmbActive.TabIndex = 7
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(126, 205)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 25
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'txtTarget_Amt
        '
        Me.txtTarget_Amt.Location = New System.Drawing.Point(126, 93)
        Me.txtTarget_Amt.MaxLength = 8
        Me.txtTarget_Amt.Name = "txtTarget_Amt"
        Me.txtTarget_Amt.Size = New System.Drawing.Size(92, 21)
        Me.txtTarget_Amt.TabIndex = 11
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(230, 205)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 26
        Me.btnOpen.Text = "Grid [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'txtCounterGroup
        '
        Me.txtCounterGroup.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCounterGroup.Location = New System.Drawing.Point(490, 65)
        Me.txtCounterGroup.MaxLength = 3
        Me.txtCounterGroup.Name = "txtCounterGroup"
        Me.txtCounterGroup.Size = New System.Drawing.Size(94, 21)
        Me.txtCounterGroup.TabIndex = 24
        Me.txtCounterGroup.Visible = False
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(334, 205)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 27
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'txtCounterName__Man
        '
        Me.txtCounterName__Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCounterName__Man.Location = New System.Drawing.Point(126, 37)
        Me.txtCounterName__Man.MaxLength = 30
        Me.txtCounterName__Man.Name = "txtCounterName__Man"
        Me.txtCounterName__Man.Size = New System.Drawing.Size(204, 21)
        Me.txtCounterName__Man.TabIndex = 3
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(438, 205)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 28
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'txtCounterId_Num_Man
        '
        Me.txtCounterId_Num_Man.Location = New System.Drawing.Point(126, 9)
        Me.txtCounterId_Num_Man.MaxLength = 7
        Me.txtCounterId_Num_Man.Name = "txtCounterId_Num_Man"
        Me.txtCounterId_Num_Man.Size = New System.Drawing.Size(92, 21)
        Me.txtCounterId_Num_Man.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(12, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Counter Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(12, 97)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(43, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Target"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(12, 69)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(42, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Active"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(224, 69)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(92, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Counter Group"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(12, 238)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(633, 263)
        Me.gridView.TabIndex = 30
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
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
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(542, 205)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 29
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(12, 503)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 31
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(224, 97)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(86, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Display Order"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDisplayOrder_NUM
        '
        Me.txtDisplayOrder_NUM.Location = New System.Drawing.Point(330, 93)
        Me.txtDisplayOrder_NUM.MaxLength = 8
        Me.txtDisplayOrder_NUM.Name = "txtDisplayOrder_NUM"
        Me.txtDisplayOrder_NUM.Size = New System.Drawing.Size(112, 21)
        Me.txtDisplayOrder_NUM.TabIndex = 13
        '
        'txtWeight_WET
        '
        Me.txtWeight_WET.Location = New System.Drawing.Point(330, 121)
        Me.txtWeight_WET.MaxLength = 10
        Me.txtWeight_WET.Name = "txtWeight_WET"
        Me.txtWeight_WET.Size = New System.Drawing.Size(112, 21)
        Me.txtWeight_WET.TabIndex = 17
        Me.txtWeight_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPiece_NUM
        '
        Me.txtPiece_NUM.Location = New System.Drawing.Point(126, 121)
        Me.txtPiece_NUM.MaxLength = 10
        Me.txtPiece_NUM.Name = "txtPiece_NUM"
        Me.txtPiece_NUM.Size = New System.Drawing.Size(92, 21)
        Me.txtPiece_NUM.TabIndex = 15
        Me.txtPiece_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 125)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(37, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Piece"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(224, 125)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(45, 13)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Weight"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblctr
        '
        Me.lblctr.Location = New System.Drawing.Point(9, 144)
        Me.lblctr.Name = "lblctr"
        Me.lblctr.Size = New System.Drawing.Size(106, 30)
        Me.lblctr.TabIndex = 18
        Me.lblctr.Text = "Item Id for Partly/Sale Ret"
        Me.lblctr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblctr.Visible = False
        '
        'txt_POS_ITEMID
        '
        Me.txt_POS_ITEMID.Location = New System.Drawing.Point(126, 149)
        Me.txt_POS_ITEMID.MaxLength = 50
        Me.txt_POS_ITEMID.Name = "txt_POS_ITEMID"
        Me.txt_POS_ITEMID.Size = New System.Drawing.Size(208, 21)
        Me.txt_POS_ITEMID.TabIndex = 19
        Me.txt_POS_ITEMID.Visible = False
        '
        'lblctr1
        '
        Me.lblctr1.Location = New System.Drawing.Point(339, 150)
        Me.lblctr1.Name = "lblctr1"
        Me.lblctr1.Size = New System.Drawing.Size(269, 22)
        Me.lblctr1.TabIndex = 20
        Me.lblctr1.Text = "Ids Seperate by comma (ex. 105,106)"
        Me.lblctr1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblctr1.Visible = False
        '
        'CmbCounterGrp
        '
        Me.CmbCounterGrp.FormattingEnabled = True
        Me.CmbCounterGrp.Location = New System.Drawing.Point(330, 65)
        Me.CmbCounterGrp.Name = "CmbCounterGrp"
        Me.CmbCounterGrp.Size = New System.Drawing.Size(112, 21)
        Me.CmbCounterGrp.TabIndex = 9
        '
        'txtTagWt_WET
        '
        Me.txtTagWt_WET.Location = New System.Drawing.Point(62, 2)
        Me.txtTagWt_WET.MaxLength = 10
        Me.txtTagWt_WET.Name = "txtTagWt_WET"
        Me.txtTagWt_WET.Size = New System.Drawing.Size(61, 21)
        Me.txtTagWt_WET.TabIndex = 1
        Me.txtTagWt_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(4, 6)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(42, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "TagWt"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCoverWt_WET
        '
        Me.txtCoverWt_WET.Location = New System.Drawing.Point(62, 29)
        Me.txtCoverWt_WET.MaxLength = 10
        Me.txtCoverWt_WET.Name = "txtCoverWt_WET"
        Me.txtCoverWt_WET.Size = New System.Drawing.Size(61, 21)
        Me.txtCoverWt_WET.TabIndex = 3
        Me.txtCoverWt_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(4, 33)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(57, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "CoverWt"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlExtra
        '
        Me.pnlExtra.Controls.Add(Me.txtCoverWt_WET)
        Me.pnlExtra.Controls.Add(Me.Label10)
        Me.pnlExtra.Controls.Add(Me.Label9)
        Me.pnlExtra.Controls.Add(Me.txtTagWt_WET)
        Me.pnlExtra.Location = New System.Drawing.Point(448, 93)
        Me.pnlExtra.Name = "pnlExtra"
        Me.pnlExtra.Size = New System.Drawing.Size(136, 53)
        Me.pnlExtra.TabIndex = 23
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Location = New System.Drawing.Point(12, 181)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(104, 13)
        Me.Label11.TabIndex = 21
        Me.Label11.Text = "Transfer Counter"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbTrfCounter
        '
        Me.cmbTrfCounter.FormattingEnabled = True
        Me.cmbTrfCounter.Location = New System.Drawing.Point(126, 178)
        Me.cmbTrfCounter.Name = "cmbTrfCounter"
        Me.cmbTrfCounter.Size = New System.Drawing.Size(316, 21)
        Me.cmbTrfCounter.TabIndex = 22
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Location = New System.Drawing.Point(339, 41)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(71, 13)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "ShortName"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCounterSHName
        '
        Me.txtCounterSHName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCounterSHName.Location = New System.Drawing.Point(416, 37)
        Me.txtCounterSHName.MaxLength = 30
        Me.txtCounterSHName.Name = "txtCounterSHName"
        Me.txtCounterSHName.Size = New System.Drawing.Size(168, 21)
        Me.txtCounterSHName.TabIndex = 5
        '
        'frmItemCounter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(657, 520)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.txtCounterSHName)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.cmbTrfCounter)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.CmbCounterGrp)
        Me.Controls.Add(Me.txt_POS_ITEMID)
        Me.Controls.Add(Me.lblctr1)
        Me.Controls.Add(Me.lblctr)
        Me.Controls.Add(Me.txtWeight_WET)
        Me.Controls.Add(Me.txtPiece_NUM)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.cmbActive)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtTarget_Amt)
        Me.Controls.Add(Me.txtDisplayOrder_NUM)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtCounterGroup)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.txtCounterName__Man)
        Me.Controls.Add(Me.txtCounterId_Num_Man)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.pnlExtra)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmItemCounter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ItemCounter"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlExtra.ResumeLayout(False)
        Me.pnlExtra.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtTarget_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtCounterGroup As System.Windows.Forms.TextBox
    Friend WithEvents txtCounterName__Man As System.Windows.Forms.TextBox
    Friend WithEvents txtCounterId_Num_Man As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents cmbActive As System.Windows.Forms.ComboBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtDisplayOrder_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtPiece_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblctr As System.Windows.Forms.Label
    Friend WithEvents txt_POS_ITEMID As System.Windows.Forms.TextBox
    Friend WithEvents lblctr1 As System.Windows.Forms.Label
    Friend WithEvents CmbCounterGrp As System.Windows.Forms.ComboBox
    Friend WithEvents txtTagWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtCoverWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents pnlExtra As System.Windows.Forms.Panel
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents cmbTrfCounter As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As Label
    Friend WithEvents txtCounterSHName As TextBox
End Class
