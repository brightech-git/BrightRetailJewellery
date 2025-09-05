<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOrderAdvance
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
        Me.grpPur = New CodeVendor.Controls.Grouper
        Me.txtPuWastage_PER = New System.Windows.Forms.TextBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.txtPurTotalAmount = New System.Windows.Forms.TextBox
        Me.txtPurTotalWeight = New System.Windows.Forms.TextBox
        Me.txtPUCategory = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.txtPUAmount_AMT = New System.Windows.Forms.TextBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.txtPUPcs_NUM = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.txtPUGrsWt_WET = New System.Windows.Forms.TextBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.txtPUVat_AMT = New System.Windows.Forms.TextBox
        Me.Label24 = New System.Windows.Forms.Label
        Me.txtPURate_AMT = New System.Windows.Forms.TextBox
        Me.txtPUDustWt_WET = New System.Windows.Forms.TextBox
        Me.Label25 = New System.Windows.Forms.Label
        Me.Label33 = New System.Windows.Forms.Label
        Me.txtPUNetWt_WET = New System.Windows.Forms.TextBox
        Me.txtPUWastage_WET = New System.Windows.Forms.TextBox
        Me.Label34 = New System.Windows.Forms.Label
        Me.Label35 = New System.Windows.Forms.Label
        Me.txtPUStoneWt_WET = New System.Windows.Forms.TextBox
        Me.txtPURowIndex = New System.Windows.Forms.TextBox
        Me.cmbPurMode = New System.Windows.Forms.ComboBox
        Me.gridPur = New System.Windows.Forms.DataGridView
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label23 = New System.Windows.Forms.Label
        Me.gridPurTotal = New System.Windows.Forms.DataGridView
        Me.txtPuPurity_PER = New System.Windows.Forms.TextBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.StuddedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpPur.SuspendLayout()
        CType(Me.gridPur, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridPurTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpPur
        '
        Me.grpPur.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpPur.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpPur.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpPur.BorderColor = System.Drawing.Color.Transparent
        Me.grpPur.BorderThickness = 1.0!
        Me.grpPur.Controls.Add(Me.txtPuWastage_PER)
        Me.grpPur.Controls.Add(Me.Label19)
        Me.grpPur.Controls.Add(Me.txtPurTotalAmount)
        Me.grpPur.Controls.Add(Me.txtPurTotalWeight)
        Me.grpPur.Controls.Add(Me.txtPUCategory)
        Me.grpPur.Controls.Add(Me.Label16)
        Me.grpPur.Controls.Add(Me.txtPUAmount_AMT)
        Me.grpPur.Controls.Add(Me.Label21)
        Me.grpPur.Controls.Add(Me.Label18)
        Me.grpPur.Controls.Add(Me.txtPUPcs_NUM)
        Me.grpPur.Controls.Add(Me.Label20)
        Me.grpPur.Controls.Add(Me.txtPUGrsWt_WET)
        Me.grpPur.Controls.Add(Me.Label22)
        Me.grpPur.Controls.Add(Me.txtPUVat_AMT)
        Me.grpPur.Controls.Add(Me.Label24)
        Me.grpPur.Controls.Add(Me.txtPURate_AMT)
        Me.grpPur.Controls.Add(Me.txtPUDustWt_WET)
        Me.grpPur.Controls.Add(Me.Label25)
        Me.grpPur.Controls.Add(Me.Label33)
        Me.grpPur.Controls.Add(Me.txtPUNetWt_WET)
        Me.grpPur.Controls.Add(Me.txtPUWastage_WET)
        Me.grpPur.Controls.Add(Me.Label34)
        Me.grpPur.Controls.Add(Me.Label35)
        Me.grpPur.Controls.Add(Me.txtPUStoneWt_WET)
        Me.grpPur.Controls.Add(Me.txtPURowIndex)
        Me.grpPur.Controls.Add(Me.cmbPurMode)
        Me.grpPur.Controls.Add(Me.gridPur)
        Me.grpPur.Controls.Add(Me.Label17)
        Me.grpPur.Controls.Add(Me.Label23)
        Me.grpPur.Controls.Add(Me.gridPurTotal)
        Me.grpPur.Controls.Add(Me.txtPuPurity_PER)
        Me.grpPur.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpPur.GroupImage = Nothing
        Me.grpPur.GroupTitle = ""
        Me.grpPur.Location = New System.Drawing.Point(2, -6)
        Me.grpPur.Name = "grpPur"
        Me.grpPur.Padding = New System.Windows.Forms.Padding(20)
        Me.grpPur.PaintGroupBox = False
        Me.grpPur.RoundCorners = 10
        Me.grpPur.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpPur.ShadowControl = False
        Me.grpPur.ShadowThickness = 3
        Me.grpPur.Size = New System.Drawing.Size(1006, 193)
        Me.grpPur.TabIndex = 1
        '
        'txtPuWastage_PER
        '
        Me.txtPuWastage_PER.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPuWastage_PER.Location = New System.Drawing.Point(567, 36)
        Me.txtPuWastage_PER.Name = "txtPuWastage_PER"
        Me.txtPuWastage_PER.Size = New System.Drawing.Size(48, 21)
        Me.txtPuWastage_PER.TabIndex = 13
        Me.txtPuWastage_PER.Text = "99.99"
        '
        'Label19
        '
        Me.Label19.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(567, 21)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(48, 13)
        Me.Label19.TabIndex = 12
        Me.Label19.Text = "W%"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPurTotalAmount
        '
        Me.txtPurTotalAmount.Location = New System.Drawing.Point(526, 136)
        Me.txtPurTotalAmount.Name = "txtPurTotalAmount"
        Me.txtPurTotalAmount.Size = New System.Drawing.Size(143, 21)
        Me.txtPurTotalAmount.TabIndex = 30
        Me.txtPurTotalAmount.Visible = False
        '
        'txtPurTotalWeight
        '
        Me.txtPurTotalWeight.Location = New System.Drawing.Point(363, 136)
        Me.txtPurTotalWeight.Name = "txtPurTotalWeight"
        Me.txtPurTotalWeight.Size = New System.Drawing.Size(143, 21)
        Me.txtPurTotalWeight.TabIndex = 29
        Me.txtPurTotalWeight.Visible = False
        '
        'txtPUCategory
        '
        Me.txtPUCategory.Location = New System.Drawing.Point(159, 36)
        Me.txtPUCategory.Name = "txtPUCategory"
        Me.txtPUCategory.Size = New System.Drawing.Size(188, 21)
        Me.txtPUCategory.TabIndex = 3
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(159, 21)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(188, 13)
        Me.Label16.TabIndex = 2
        Me.Label16.Text = "Category"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUAmount_AMT
        '
        Me.txtPUAmount_AMT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUAmount_AMT.Location = New System.Drawing.Point(886, 36)
        Me.txtPUAmount_AMT.MaxLength = 12
        Me.txtPUAmount_AMT.Name = "txtPUAmount_AMT"
        Me.txtPUAmount_AMT.Size = New System.Drawing.Size(84, 21)
        Me.txtPUAmount_AMT.TabIndex = 23
        Me.txtPUAmount_AMT.Text = "`"
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(750, 21)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(70, 13)
        Me.Label21.TabIndex = 18
        Me.Label21.Text = "Rate"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(821, 21)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(64, 13)
        Me.Label18.TabIndex = 20
        Me.Label18.Text = "Vat"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUPcs_NUM
        '
        Me.txtPUPcs_NUM.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUPcs_NUM.Location = New System.Drawing.Point(407, 36)
        Me.txtPUPcs_NUM.MaxLength = 5
        Me.txtPUPcs_NUM.Name = "txtPUPcs_NUM"
        Me.txtPUPcs_NUM.Size = New System.Drawing.Size(39, 21)
        Me.txtPUPcs_NUM.TabIndex = 7
        Me.txtPUPcs_NUM.Text = "9999"
        '
        'Label20
        '
        Me.Label20.Location = New System.Drawing.Point(683, 21)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(66, 13)
        Me.Label20.TabIndex = 16
        Me.Label20.Text = "Net WT"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUGrsWt_WET
        '
        Me.txtPUGrsWt_WET.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUGrsWt_WET.Location = New System.Drawing.Point(447, 36)
        Me.txtPUGrsWt_WET.MaxLength = 10
        Me.txtPUGrsWt_WET.Name = "txtPUGrsWt_WET"
        Me.txtPUGrsWt_WET.Size = New System.Drawing.Size(66, 21)
        Me.txtPUGrsWt_WET.TabIndex = 9
        Me.txtPUGrsWt_WET.Text = "999.999"
        '
        'Label22
        '
        Me.Label22.Location = New System.Drawing.Point(886, 21)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(78, 13)
        Me.Label22.TabIndex = 22
        Me.Label22.Text = "Amount"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUVat_AMT
        '
        Me.txtPUVat_AMT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUVat_AMT.Location = New System.Drawing.Point(821, 36)
        Me.txtPUVat_AMT.MaxLength = 12
        Me.txtPUVat_AMT.Name = "txtPUVat_AMT"
        Me.txtPUVat_AMT.Size = New System.Drawing.Size(64, 21)
        Me.txtPUVat_AMT.TabIndex = 21
        Me.txtPUVat_AMT.Text = "99999.99"
        '
        'Label24
        '
        Me.Label24.Location = New System.Drawing.Point(407, 21)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(39, 13)
        Me.Label24.TabIndex = 6
        Me.Label24.Text = "Pcs"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPURate_AMT
        '
        Me.txtPURate_AMT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPURate_AMT.Location = New System.Drawing.Point(750, 36)
        Me.txtPURate_AMT.MaxLength = 10
        Me.txtPURate_AMT.Name = "txtPURate_AMT"
        Me.txtPURate_AMT.Size = New System.Drawing.Size(70, 21)
        Me.txtPURate_AMT.TabIndex = 19
        Me.txtPURate_AMT.Text = "999.999"
        '
        'txtPUDustWt_WET
        '
        Me.txtPUDustWt_WET.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUDustWt_WET.Location = New System.Drawing.Point(514, 36)
        Me.txtPUDustWt_WET.MaxLength = 10
        Me.txtPUDustWt_WET.Name = "txtPUDustWt_WET"
        Me.txtPUDustWt_WET.Size = New System.Drawing.Size(52, 21)
        Me.txtPUDustWt_WET.TabIndex = 11
        Me.txtPUDustWt_WET.Text = "999.999"
        '
        'Label25
        '
        Me.Label25.Location = New System.Drawing.Point(698, 91)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(66, 13)
        Me.Label25.TabIndex = 24
        Me.Label25.Text = "Stone Wt"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label25.Visible = False
        '
        'Label33
        '
        Me.Label33.Location = New System.Drawing.Point(447, 21)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(66, 13)
        Me.Label33.TabIndex = 8
        Me.Label33.Text = "Grswt"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUNetWt_WET
        '
        Me.txtPUNetWt_WET.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUNetWt_WET.Location = New System.Drawing.Point(683, 36)
        Me.txtPUNetWt_WET.MaxLength = 10
        Me.txtPUNetWt_WET.Name = "txtPUNetWt_WET"
        Me.txtPUNetWt_WET.Size = New System.Drawing.Size(66, 21)
        Me.txtPUNetWt_WET.TabIndex = 17
        Me.txtPUNetWt_WET.Text = "999.999"
        '
        'txtPUWastage_WET
        '
        Me.txtPUWastage_WET.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUWastage_WET.Location = New System.Drawing.Point(616, 36)
        Me.txtPUWastage_WET.MaxLength = 10
        Me.txtPUWastage_WET.Name = "txtPUWastage_WET"
        Me.txtPUWastage_WET.Size = New System.Drawing.Size(66, 21)
        Me.txtPUWastage_WET.TabIndex = 15
        Me.txtPUWastage_WET.Text = "999.999"
        '
        'Label34
        '
        Me.Label34.Location = New System.Drawing.Point(616, 21)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(66, 13)
        Me.Label34.TabIndex = 14
        Me.Label34.Text = "Wastage"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label35
        '
        Me.Label35.Location = New System.Drawing.Point(514, 21)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(52, 13)
        Me.Label35.TabIndex = 10
        Me.Label35.Text = "DustWt"
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUStoneWt_WET
        '
        Me.txtPUStoneWt_WET.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUStoneWt_WET.Location = New System.Drawing.Point(698, 106)
        Me.txtPUStoneWt_WET.MaxLength = 10
        Me.txtPUStoneWt_WET.Name = "txtPUStoneWt_WET"
        Me.txtPUStoneWt_WET.Size = New System.Drawing.Size(66, 21)
        Me.txtPUStoneWt_WET.TabIndex = 25
        Me.txtPUStoneWt_WET.Text = "999.999"
        Me.txtPUStoneWt_WET.Visible = False
        '
        'txtPURowIndex
        '
        Me.txtPURowIndex.Location = New System.Drawing.Point(872, 135)
        Me.txtPURowIndex.Name = "txtPURowIndex"
        Me.txtPURowIndex.Size = New System.Drawing.Size(8, 21)
        Me.txtPURowIndex.TabIndex = 28
        Me.txtPURowIndex.Visible = False
        '
        'cmbPurMode
        '
        Me.cmbPurMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPurMode.FormattingEnabled = True
        Me.cmbPurMode.Location = New System.Drawing.Point(7, 36)
        Me.cmbPurMode.Name = "cmbPurMode"
        Me.cmbPurMode.Size = New System.Drawing.Size(151, 21)
        Me.cmbPurMode.TabIndex = 1
        '
        'gridPur
        '
        Me.gridPur.AllowUserToAddRows = False
        Me.gridPur.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridPur.Location = New System.Drawing.Point(7, 58)
        Me.gridPur.Name = "gridPur"
        Me.gridPur.ReadOnly = True
        Me.gridPur.Size = New System.Drawing.Size(991, 110)
        Me.gridPur.TabIndex = 26
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(7, 21)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(151, 13)
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "Mode"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label23
        '
        Me.Label23.Location = New System.Drawing.Point(348, 21)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(58, 13)
        Me.Label23.TabIndex = 4
        Me.Label23.Text = "Purity"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gridPurTotal
        '
        Me.gridPurTotal.AllowUserToAddRows = False
        Me.gridPurTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridPurTotal.Enabled = False
        Me.gridPurTotal.Location = New System.Drawing.Point(7, 168)
        Me.gridPurTotal.Name = "gridPurTotal"
        Me.gridPurTotal.ReadOnly = True
        Me.gridPurTotal.Size = New System.Drawing.Size(991, 19)
        Me.gridPurTotal.TabIndex = 27
        '
        'txtPuPurity_PER
        '
        Me.txtPuPurity_PER.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPuPurity_PER.Location = New System.Drawing.Point(348, 36)
        Me.txtPuPurity_PER.MaxLength = 10
        Me.txtPuPurity_PER.Name = "txtPuPurity_PER"
        Me.txtPuPurity_PER.Size = New System.Drawing.Size(58, 21)
        Me.txtPuPurity_PER.TabIndex = 5
        Me.txtPuPurity_PER.Text = "999.999"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StuddedToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(159, 48)
        '
        'StuddedToolStripMenuItem
        '
        Me.StuddedToolStripMenuItem.Name = "StuddedToolStripMenuItem"
        Me.StuddedToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.StuddedToolStripMenuItem.Size = New System.Drawing.Size(158, 22)
        Me.StuddedToolStripMenuItem.Text = "Studded"
        Me.StuddedToolStripMenuItem.Visible = False
        '
        'frmOrderAdvance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(1011, 190)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpPur)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmOrderAdvance"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Order Advance"
        Me.grpPur.ResumeLayout(False)
        Me.grpPur.PerformLayout()
        CType(Me.gridPur, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridPurTotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpPur As CodeVendor.Controls.Grouper
    Friend WithEvents txtPuWastage_PER As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtPurTotalAmount As System.Windows.Forms.TextBox
    Friend WithEvents txtPurTotalWeight As System.Windows.Forms.TextBox
    Friend WithEvents txtPUCategory As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtPUAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtPUPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtPUGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents txtPUVat_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents txtPURate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtPUDustWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents txtPUNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtPUWastage_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents txtPUStoneWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtPURowIndex As System.Windows.Forms.TextBox
    Friend WithEvents cmbPurMode As System.Windows.Forms.ComboBox
    Friend WithEvents gridPur As System.Windows.Forms.DataGridView
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents gridPurTotal As System.Windows.Forms.DataGridView
    Friend WithEvents txtPuPurity_PER As System.Windows.Forms.TextBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents StuddedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
