<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStickerPrint
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
        Me.txtMaxMkCharge_Amt = New System.Windows.Forms.TextBox()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.txtMaxWastage_Per = New System.Windows.Forms.TextBox()
        Me.txtMaxWastage_Wet = New System.Windows.Forms.TextBox()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.txtMaxMcPerGrm_Amt = New System.Windows.Forms.TextBox()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.cmbItem_MAN = New System.Windows.Forms.ComboBox()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.txtNarration = New System.Windows.Forms.TextBox()
        Me.cmbItemSize = New System.Windows.Forms.ComboBox()
        Me.cmbSubItem_Man = New System.Windows.Forms.ComboBox()
        Me.cmbDesigner_MAN = New System.Windows.Forms.ComboBox()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.txtNetWt_Wet = New System.Windows.Forms.TextBox()
        Me.txtGrossWt_Wet_MAN = New System.Windows.Forms.TextBox()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.txtLessWt_Wet = New System.Windows.Forms.TextBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.cmbCounter_MAN = New System.Windows.Forms.ComboBox()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtPieces_Num_MAN = New System.Windows.Forms.TextBox()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtMaxMkCharge_Amt
        '
        Me.txtMaxMkCharge_Amt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxMkCharge_Amt.Location = New System.Drawing.Point(351, 273)
        Me.txtMaxMkCharge_Amt.MaxLength = 10
        Me.txtMaxMkCharge_Amt.Name = "txtMaxMkCharge_Amt"
        Me.txtMaxMkCharge_Amt.Size = New System.Drawing.Size(87, 21)
        Me.txtMaxMkCharge_Amt.TabIndex = 25
        Me.txtMaxMkCharge_Amt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.Location = New System.Drawing.Point(27, 249)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(71, 13)
        Me.Label45.TabIndex = 18
        Me.Label45.Text = "Wastage %"
        Me.Label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMaxWastage_Per
        '
        Me.txtMaxWastage_Per.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxWastage_Per.Location = New System.Drawing.Point(129, 245)
        Me.txtMaxWastage_Per.MaxLength = 10
        Me.txtMaxWastage_Per.Name = "txtMaxWastage_Per"
        Me.txtMaxWastage_Per.Size = New System.Drawing.Size(104, 21)
        Me.txtMaxWastage_Per.TabIndex = 19
        Me.txtMaxWastage_Per.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMaxWastage_Wet
        '
        Me.txtMaxWastage_Wet.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxWastage_Wet.Location = New System.Drawing.Point(129, 273)
        Me.txtMaxWastage_Wet.MaxLength = 10
        Me.txtMaxWastage_Wet.Name = "txtMaxWastage_Wet"
        Me.txtMaxWastage_Wet.Size = New System.Drawing.Size(104, 21)
        Me.txtMaxWastage_Wet.TabIndex = 23
        Me.txtMaxWastage_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label46
        '
        Me.Label46.AutoSize = True
        Me.Label46.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label46.Location = New System.Drawing.Point(27, 277)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(55, 13)
        Me.Label46.TabIndex = 22
        Me.Label46.Text = "Wastage"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.Location = New System.Drawing.Point(242, 277)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(25, 13)
        Me.Label47.TabIndex = 24
        Me.Label47.Text = "MC"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMaxMcPerGrm_Amt
        '
        Me.txtMaxMcPerGrm_Amt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxMcPerGrm_Amt.Location = New System.Drawing.Point(351, 245)
        Me.txtMaxMcPerGrm_Amt.MaxLength = 10
        Me.txtMaxMcPerGrm_Amt.Name = "txtMaxMcPerGrm_Amt"
        Me.txtMaxMcPerGrm_Amt.Size = New System.Drawing.Size(87, 21)
        Me.txtMaxMcPerGrm_Amt.TabIndex = 21
        Me.txtMaxMcPerGrm_Amt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.Location = New System.Drawing.Point(242, 249)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(74, 13)
        Me.Label44.TabIndex = 20
        Me.Label44.Text = "Mc Per Grm"
        Me.Label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItem_MAN
        '
        Me.cmbItem_MAN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbItem_MAN.FormattingEnabled = True
        Me.cmbItem_MAN.Location = New System.Drawing.Point(129, 49)
        Me.cmbItem_MAN.Name = "cmbItem_MAN"
        Me.cmbItem_MAN.Size = New System.Drawing.Size(309, 21)
        Me.cmbItem_MAN.TabIndex = 3
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label55.Location = New System.Drawing.Point(27, 305)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(60, 13)
        Me.Label55.TabIndex = 26
        Me.Label55.Text = "Narration"
        Me.Label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtNarration
        '
        Me.txtNarration.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNarration.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNarration.Location = New System.Drawing.Point(129, 301)
        Me.txtNarration.MaxLength = 50
        Me.txtNarration.Name = "txtNarration"
        Me.txtNarration.Size = New System.Drawing.Size(309, 21)
        Me.txtNarration.TabIndex = 27
        '
        'cmbItemSize
        '
        Me.cmbItemSize.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbItemSize.FormattingEnabled = True
        Me.cmbItemSize.Location = New System.Drawing.Point(129, 133)
        Me.cmbItemSize.Name = "cmbItemSize"
        Me.cmbItemSize.Size = New System.Drawing.Size(309, 21)
        Me.cmbItemSize.TabIndex = 9
        '
        'cmbSubItem_Man
        '
        Me.cmbSubItem_Man.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSubItem_Man.FormattingEnabled = True
        Me.cmbSubItem_Man.Location = New System.Drawing.Point(129, 77)
        Me.cmbSubItem_Man.Name = "cmbSubItem_Man"
        Me.cmbSubItem_Man.Size = New System.Drawing.Size(309, 21)
        Me.cmbSubItem_Man.TabIndex = 5
        '
        'cmbDesigner_MAN
        '
        Me.cmbDesigner_MAN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbDesigner_MAN.FormattingEnabled = True
        Me.cmbDesigner_MAN.Location = New System.Drawing.Point(129, 21)
        Me.cmbDesigner_MAN.Name = "cmbDesigner_MAN"
        Me.cmbDesigner_MAN.Size = New System.Drawing.Size(309, 21)
        Me.cmbDesigner_MAN.TabIndex = 1
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.Location = New System.Drawing.Point(27, 53)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(71, 13)
        Me.Label34.TabIndex = 2
        Me.Label34.Text = "Item Name"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(27, 137)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Item Size"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label56.Location = New System.Drawing.Point(27, 81)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(56, 13)
        Me.Label56.TabIndex = 4
        Me.Label56.Text = "SubItem"
        Me.Label56.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label54.Location = New System.Drawing.Point(27, 25)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(62, 13)
        Me.Label54.TabIndex = 0
        Me.Label54.Text = "&Designer "
        Me.Label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.Location = New System.Drawing.Point(258, 221)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(68, 13)
        Me.Label43.TabIndex = 16
        Me.Label43.Text = "Net Weight"
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.Location = New System.Drawing.Point(27, 221)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(74, 13)
        Me.Label39.TabIndex = 14
        Me.Label39.Text = "Less Weight"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtNetWt_Wet
        '
        Me.txtNetWt_Wet.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNetWt_Wet.Location = New System.Drawing.Point(351, 217)
        Me.txtNetWt_Wet.MaxLength = 10
        Me.txtNetWt_Wet.Name = "txtNetWt_Wet"
        Me.txtNetWt_Wet.Size = New System.Drawing.Size(104, 21)
        Me.txtNetWt_Wet.TabIndex = 17
        Me.txtNetWt_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtGrossWt_Wet_MAN
        '
        Me.txtGrossWt_Wet_MAN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGrossWt_Wet_MAN.Location = New System.Drawing.Point(129, 189)
        Me.txtGrossWt_Wet_MAN.MaxLength = 10
        Me.txtGrossWt_Wet_MAN.Name = "txtGrossWt_Wet_MAN"
        Me.txtGrossWt_Wet_MAN.Size = New System.Drawing.Size(104, 21)
        Me.txtGrossWt_Wet_MAN.TabIndex = 13
        Me.txtGrossWt_Wet_MAN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.Location = New System.Drawing.Point(27, 193)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(82, 13)
        Me.Label37.TabIndex = 12
        Me.Label37.Text = "Gross Weight"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtLessWt_Wet
        '
        Me.txtLessWt_Wet.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLessWt_Wet.Location = New System.Drawing.Point(129, 217)
        Me.txtLessWt_Wet.MaxLength = 10
        Me.txtLessWt_Wet.Name = "txtLessWt_Wet"
        Me.txtLessWt_Wet.Size = New System.Drawing.Size(104, 21)
        Me.txtLessWt_Wet.TabIndex = 15
        Me.txtLessWt_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(298, 346)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 30
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.Location = New System.Drawing.Point(190, 346)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 29
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(82, 346)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 28
        Me.btnSave.Text = "Print [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'cmbCounter_MAN
        '
        Me.cmbCounter_MAN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCounter_MAN.FormattingEnabled = True
        Me.cmbCounter_MAN.Location = New System.Drawing.Point(129, 105)
        Me.cmbCounter_MAN.Name = "cmbCounter_MAN"
        Me.cmbCounter_MAN.Size = New System.Drawing.Size(309, 21)
        Me.cmbCounter_MAN.TabIndex = 7
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.Location = New System.Drawing.Point(27, 109)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(53, 13)
        Me.Label38.TabIndex = 6
        Me.Label38.Text = "&Counter"
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 70)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(27, 165)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(53, 13)
        Me.Label10.TabIndex = 10
        Me.Label10.Text = "Piece(s)"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPieces_Num_MAN
        '
        Me.txtPieces_Num_MAN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPieces_Num_MAN.Location = New System.Drawing.Point(129, 161)
        Me.txtPieces_Num_MAN.Name = "txtPieces_Num_MAN"
        Me.txtPieces_Num_MAN.Size = New System.Drawing.Size(104, 21)
        Me.txtPieces_Num_MAN.TabIndex = 11
        Me.txtPieces_Num_MAN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmStickerPrint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(480, 396)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtPieces_Num_MAN)
        Me.Controls.Add(Me.cmbCounter_MAN)
        Me.Controls.Add(Me.Label38)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtMaxMkCharge_Amt)
        Me.Controls.Add(Me.cmbItem_MAN)
        Me.Controls.Add(Me.Label45)
        Me.Controls.Add(Me.Label55)
        Me.Controls.Add(Me.txtMaxWastage_Per)
        Me.Controls.Add(Me.txtNarration)
        Me.Controls.Add(Me.txtMaxWastage_Wet)
        Me.Controls.Add(Me.Label46)
        Me.Controls.Add(Me.cmbItemSize)
        Me.Controls.Add(Me.Label47)
        Me.Controls.Add(Me.cmbSubItem_Man)
        Me.Controls.Add(Me.txtMaxMcPerGrm_Amt)
        Me.Controls.Add(Me.cmbDesigner_MAN)
        Me.Controls.Add(Me.Label44)
        Me.Controls.Add(Me.Label34)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label56)
        Me.Controls.Add(Me.Label54)
        Me.Controls.Add(Me.Label43)
        Me.Controls.Add(Me.Label39)
        Me.Controls.Add(Me.txtNetWt_Wet)
        Me.Controls.Add(Me.txtGrossWt_Wet_MAN)
        Me.Controls.Add(Me.Label37)
        Me.Controls.Add(Me.txtLessWt_Wet)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmStickerPrint"
        Me.Text = "Sticker Print"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtMaxMkCharge_Amt As TextBox
    Friend WithEvents Label45 As Label
    Friend WithEvents txtMaxWastage_Per As TextBox
    Friend WithEvents txtMaxWastage_Wet As TextBox
    Friend WithEvents Label46 As Label
    Friend WithEvents Label47 As Label
    Friend WithEvents txtMaxMcPerGrm_Amt As TextBox
    Friend WithEvents Label44 As Label
    Friend WithEvents cmbItem_MAN As ComboBox
    Friend WithEvents Label55 As Label
    Friend WithEvents txtNarration As TextBox
    Friend WithEvents cmbItemSize As ComboBox
    Friend WithEvents cmbSubItem_Man As ComboBox
    Friend WithEvents cmbDesigner_MAN As ComboBox
    Friend WithEvents Label34 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label56 As Label
    Friend WithEvents Label54 As Label
    Friend WithEvents Label43 As Label
    Friend WithEvents Label39 As Label
    Friend WithEvents txtNetWt_Wet As TextBox
    Friend WithEvents txtGrossWt_Wet_MAN As TextBox
    Friend WithEvents Label37 As Label
    Friend WithEvents txtLessWt_Wet As TextBox
    Friend WithEvents btnExit As Button
    Friend WithEvents btnNew As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents cmbCounter_MAN As ComboBox
    Friend WithEvents Label38 As Label
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Label10 As Label
    Friend WithEvents txtPieces_Num_MAN As TextBox
End Class
