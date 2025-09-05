<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMultiMetalDia
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
        Me.grpMultiMetal = New CodeVendor.Controls.Grouper()
        Me.lblMMNetWt = New System.Windows.Forms.Label()
        Me.txtMMNetWeight_Wet = New System.Windows.Forms.TextBox()
        Me.txtMMNet_AMT = New System.Windows.Forms.TextBox()
        Me.lblMMNetAmt = New System.Windows.Forms.Label()
        Me.lblMMTax = New System.Windows.Forms.Label()
        Me.txtMMVAT_AMT = New System.Windows.Forms.TextBox()
        Me.lblMMRate = New System.Windows.Forms.Label()
        Me.txtMMRate = New System.Windows.Forms.TextBox()
        Me.txtMMRowIndex = New System.Windows.Forms.TextBox()
        Me.txtMMAmount_AMT = New System.Windows.Forms.TextBox()
        Me.txtMMMc_AMT = New System.Windows.Forms.TextBox()
        Me.txtMMWastage_WET = New System.Windows.Forms.TextBox()
        Me.txtMMMcPerGRm_AMT = New System.Windows.Forms.TextBox()
        Me.txtMMWastagePer_PER = New System.Windows.Forms.TextBox()
        Me.txtMMCategory = New System.Windows.Forms.TextBox()
        Me.gridMultimetal = New System.Windows.Forms.DataGridView()
        Me.gridMultiMetalTotal = New System.Windows.Forms.DataGridView()
        Me.Label108 = New System.Windows.Forms.Label()
        Me.lblMMGrossAmt = New System.Windows.Forms.Label()
        Me.lblMMMc = New System.Windows.Forms.Label()
        Me.lblMMwastage = New System.Windows.Forms.Label()
        Me.lblMMMcGrm = New System.Windows.Forms.Label()
        Me.lblMMWastPer = New System.Windows.Forms.Label()
        Me.lblMMWeight = New System.Windows.Forms.Label()
        Me.txtMMWeight_Wet = New System.Windows.Forms.TextBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RateChangeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grpMultiMetal.SuspendLayout()
        CType(Me.gridMultimetal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridMultiMetalTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpMultiMetal
        '
        Me.grpMultiMetal.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpMultiMetal.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpMultiMetal.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpMultiMetal.BorderColor = System.Drawing.Color.Transparent
        Me.grpMultiMetal.BorderThickness = 1.0!
        Me.grpMultiMetal.Controls.Add(Me.lblMMNetWt)
        Me.grpMultiMetal.Controls.Add(Me.txtMMNetWeight_Wet)
        Me.grpMultiMetal.Controls.Add(Me.txtMMNet_AMT)
        Me.grpMultiMetal.Controls.Add(Me.lblMMNetAmt)
        Me.grpMultiMetal.Controls.Add(Me.lblMMTax)
        Me.grpMultiMetal.Controls.Add(Me.txtMMVAT_AMT)
        Me.grpMultiMetal.Controls.Add(Me.lblMMRate)
        Me.grpMultiMetal.Controls.Add(Me.txtMMRate)
        Me.grpMultiMetal.Controls.Add(Me.txtMMRowIndex)
        Me.grpMultiMetal.Controls.Add(Me.txtMMAmount_AMT)
        Me.grpMultiMetal.Controls.Add(Me.txtMMMc_AMT)
        Me.grpMultiMetal.Controls.Add(Me.txtMMWastage_WET)
        Me.grpMultiMetal.Controls.Add(Me.txtMMMcPerGRm_AMT)
        Me.grpMultiMetal.Controls.Add(Me.txtMMWastagePer_PER)
        Me.grpMultiMetal.Controls.Add(Me.txtMMCategory)
        Me.grpMultiMetal.Controls.Add(Me.gridMultimetal)
        Me.grpMultiMetal.Controls.Add(Me.gridMultiMetalTotal)
        Me.grpMultiMetal.Controls.Add(Me.Label108)
        Me.grpMultiMetal.Controls.Add(Me.lblMMGrossAmt)
        Me.grpMultiMetal.Controls.Add(Me.lblMMMc)
        Me.grpMultiMetal.Controls.Add(Me.lblMMwastage)
        Me.grpMultiMetal.Controls.Add(Me.lblMMMcGrm)
        Me.grpMultiMetal.Controls.Add(Me.lblMMWastPer)
        Me.grpMultiMetal.Controls.Add(Me.lblMMWeight)
        Me.grpMultiMetal.Controls.Add(Me.txtMMWeight_Wet)
        Me.grpMultiMetal.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpMultiMetal.GroupImage = Nothing
        Me.grpMultiMetal.GroupTitle = ""
        Me.grpMultiMetal.Location = New System.Drawing.Point(6, -5)
        Me.grpMultiMetal.Name = "grpMultiMetal"
        Me.grpMultiMetal.Padding = New System.Windows.Forms.Padding(20)
        Me.grpMultiMetal.PaintGroupBox = False
        Me.grpMultiMetal.RoundCorners = 10
        Me.grpMultiMetal.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpMultiMetal.ShadowControl = False
        Me.grpMultiMetal.ShadowThickness = 3
        Me.grpMultiMetal.Size = New System.Drawing.Size(974, 200)
        Me.grpMultiMetal.TabIndex = 0
        '
        'lblMMNetWt
        '
        Me.lblMMNetWt.BackColor = System.Drawing.Color.Transparent
        Me.lblMMNetWt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMMNetWt.Location = New System.Drawing.Point(292, 17)
        Me.lblMMNetWt.Name = "lblMMNetWt"
        Me.lblMMNetWt.Size = New System.Drawing.Size(81, 21)
        Me.lblMMNetWt.TabIndex = 4
        Me.lblMMNetWt.Text = "NetWt"
        Me.lblMMNetWt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtMMNetWeight_Wet
        '
        Me.txtMMNetWeight_Wet.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMMNetWeight_Wet.Location = New System.Drawing.Point(292, 38)
        Me.txtMMNetWeight_Wet.Name = "txtMMNetWeight_Wet"
        Me.txtMMNetWeight_Wet.Size = New System.Drawing.Size(81, 22)
        Me.txtMMNetWeight_Wet.TabIndex = 5
        Me.txtMMNetWeight_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMMNet_AMT
        '
        Me.txtMMNet_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMMNet_AMT.Location = New System.Drawing.Point(873, 38)
        Me.txtMMNet_AMT.MaxLength = 20
        Me.txtMMNet_AMT.Name = "txtMMNet_AMT"
        Me.txtMMNet_AMT.Size = New System.Drawing.Size(93, 22)
        Me.txtMMNet_AMT.TabIndex = 21
        '
        'lblMMNetAmt
        '
        Me.lblMMNetAmt.BackColor = System.Drawing.Color.Transparent
        Me.lblMMNetAmt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMMNetAmt.Location = New System.Drawing.Point(868, 17)
        Me.lblMMNetAmt.Name = "lblMMNetAmt"
        Me.lblMMNetAmt.Size = New System.Drawing.Size(93, 21)
        Me.lblMMNetAmt.TabIndex = 20
        Me.lblMMNetAmt.Text = "Net Value"
        Me.lblMMNetAmt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMMTax
        '
        Me.lblMMTax.BackColor = System.Drawing.Color.Transparent
        Me.lblMMTax.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMMTax.Location = New System.Drawing.Point(799, 17)
        Me.lblMMTax.Name = "lblMMTax"
        Me.lblMMTax.Size = New System.Drawing.Size(73, 21)
        Me.lblMMTax.TabIndex = 18
        Me.lblMMTax.Text = "Vat"
        Me.lblMMTax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtMMVAT_AMT
        '
        Me.txtMMVAT_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMMVAT_AMT.Location = New System.Drawing.Point(799, 38)
        Me.txtMMVAT_AMT.MaxLength = 20
        Me.txtMMVAT_AMT.Name = "txtMMVAT_AMT"
        Me.txtMMVAT_AMT.Size = New System.Drawing.Size(73, 22)
        Me.txtMMVAT_AMT.TabIndex = 19
        '
        'lblMMRate
        '
        Me.lblMMRate.BackColor = System.Drawing.Color.Transparent
        Me.lblMMRate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMMRate.Location = New System.Drawing.Point(376, 17)
        Me.lblMMRate.Name = "lblMMRate"
        Me.lblMMRate.Size = New System.Drawing.Size(70, 21)
        Me.lblMMRate.TabIndex = 6
        Me.lblMMRate.Text = "Rate"
        Me.lblMMRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtMMRate
        '
        Me.txtMMRate.Location = New System.Drawing.Point(374, 39)
        Me.txtMMRate.MaxLength = 20
        Me.txtMMRate.Name = "txtMMRate"
        Me.txtMMRate.Size = New System.Drawing.Size(67, 21)
        Me.txtMMRate.TabIndex = 7
        '
        'txtMMRowIndex
        '
        Me.txtMMRowIndex.Location = New System.Drawing.Point(291, 69)
        Me.txtMMRowIndex.Name = "txtMMRowIndex"
        Me.txtMMRowIndex.Size = New System.Drawing.Size(43, 21)
        Me.txtMMRowIndex.TabIndex = 22
        Me.txtMMRowIndex.Visible = False
        '
        'txtMMAmount_AMT
        '
        Me.txtMMAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMMAmount_AMT.Location = New System.Drawing.Point(705, 38)
        Me.txtMMAmount_AMT.MaxLength = 20
        Me.txtMMAmount_AMT.Name = "txtMMAmount_AMT"
        Me.txtMMAmount_AMT.Size = New System.Drawing.Size(93, 22)
        Me.txtMMAmount_AMT.TabIndex = 17
        '
        'txtMMMc_AMT
        '
        Me.txtMMMc_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMMMc_AMT.Location = New System.Drawing.Point(628, 38)
        Me.txtMMMc_AMT.MaxLength = 20
        Me.txtMMMc_AMT.Name = "txtMMMc_AMT"
        Me.txtMMMc_AMT.Size = New System.Drawing.Size(76, 22)
        Me.txtMMMc_AMT.TabIndex = 15
        '
        'txtMMWastage_WET
        '
        Me.txtMMWastage_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMMWastage_WET.Location = New System.Drawing.Point(494, 38)
        Me.txtMMWastage_WET.MaxLength = 20
        Me.txtMMWastage_WET.Name = "txtMMWastage_WET"
        Me.txtMMWastage_WET.Size = New System.Drawing.Size(68, 22)
        Me.txtMMWastage_WET.TabIndex = 11
        '
        'txtMMMcPerGRm_AMT
        '
        Me.txtMMMcPerGRm_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMMMcPerGRm_AMT.Location = New System.Drawing.Point(563, 38)
        Me.txtMMMcPerGRm_AMT.MaxLength = 20
        Me.txtMMMcPerGRm_AMT.Name = "txtMMMcPerGRm_AMT"
        Me.txtMMMcPerGRm_AMT.Size = New System.Drawing.Size(64, 22)
        Me.txtMMMcPerGRm_AMT.TabIndex = 13
        '
        'txtMMWastagePer_PER
        '
        Me.txtMMWastagePer_PER.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMMWastagePer_PER.Location = New System.Drawing.Point(442, 38)
        Me.txtMMWastagePer_PER.MaxLength = 5
        Me.txtMMWastagePer_PER.Name = "txtMMWastagePer_PER"
        Me.txtMMWastagePer_PER.Size = New System.Drawing.Size(51, 22)
        Me.txtMMWastagePer_PER.TabIndex = 9
        '
        'txtMMCategory
        '
        Me.txtMMCategory.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMMCategory.Location = New System.Drawing.Point(7, 38)
        Me.txtMMCategory.Name = "txtMMCategory"
        Me.txtMMCategory.Size = New System.Drawing.Size(202, 22)
        Me.txtMMCategory.TabIndex = 1
        '
        'gridMultimetal
        '
        Me.gridMultimetal.AllowUserToAddRows = False
        Me.gridMultimetal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridMultimetal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridMultimetal.ColumnHeadersVisible = False
        Me.gridMultimetal.Location = New System.Drawing.Point(7, 61)
        Me.gridMultimetal.Name = "gridMultimetal"
        Me.gridMultimetal.ReadOnly = True
        Me.gridMultimetal.RowHeadersVisible = False
        Me.gridMultimetal.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridMultimetal.RowTemplate.Height = 20
        Me.gridMultimetal.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridMultimetal.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridMultimetal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridMultimetal.Size = New System.Drawing.Size(959, 113)
        Me.gridMultimetal.TabIndex = 23
        '
        'gridMultiMetalTotal
        '
        Me.gridMultiMetalTotal.AllowUserToAddRows = False
        Me.gridMultiMetalTotal.AllowUserToDeleteRows = False
        Me.gridMultiMetalTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridMultiMetalTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridMultiMetalTotal.ColumnHeadersVisible = False
        Me.gridMultiMetalTotal.Enabled = False
        Me.gridMultiMetalTotal.Location = New System.Drawing.Point(7, 174)
        Me.gridMultiMetalTotal.Name = "gridMultiMetalTotal"
        Me.gridMultiMetalTotal.ReadOnly = True
        Me.gridMultiMetalTotal.RowHeadersVisible = False
        Me.gridMultiMetalTotal.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridMultiMetalTotal.Size = New System.Drawing.Size(959, 19)
        Me.gridMultiMetalTotal.TabIndex = 24
        '
        'Label108
        '
        Me.Label108.BackColor = System.Drawing.Color.Transparent
        Me.Label108.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label108.Location = New System.Drawing.Point(7, 17)
        Me.Label108.Name = "Label108"
        Me.Label108.Size = New System.Drawing.Size(202, 21)
        Me.Label108.TabIndex = 0
        Me.Label108.Text = "Category Name"
        Me.Label108.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMMGrossAmt
        '
        Me.lblMMGrossAmt.BackColor = System.Drawing.Color.Transparent
        Me.lblMMGrossAmt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMMGrossAmt.Location = New System.Drawing.Point(704, 17)
        Me.lblMMGrossAmt.Name = "lblMMGrossAmt"
        Me.lblMMGrossAmt.Size = New System.Drawing.Size(93, 21)
        Me.lblMMGrossAmt.TabIndex = 16
        Me.lblMMGrossAmt.Text = "Gross Value"
        Me.lblMMGrossAmt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMMMc
        '
        Me.lblMMMc.BackColor = System.Drawing.Color.Transparent
        Me.lblMMMc.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMMMc.Location = New System.Drawing.Point(630, 17)
        Me.lblMMMc.Name = "lblMMMc"
        Me.lblMMMc.Size = New System.Drawing.Size(76, 21)
        Me.lblMMMc.TabIndex = 14
        Me.lblMMMc.Text = "Mc"
        Me.lblMMMc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMMwastage
        '
        Me.lblMMwastage.BackColor = System.Drawing.Color.Transparent
        Me.lblMMwastage.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMMwastage.Location = New System.Drawing.Point(497, 17)
        Me.lblMMwastage.Name = "lblMMwastage"
        Me.lblMMwastage.Size = New System.Drawing.Size(68, 21)
        Me.lblMMwastage.TabIndex = 10
        Me.lblMMwastage.Text = "Wastage"
        Me.lblMMwastage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMMMcGrm
        '
        Me.lblMMMcGrm.BackColor = System.Drawing.Color.Transparent
        Me.lblMMMcGrm.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMMMcGrm.Location = New System.Drawing.Point(568, 17)
        Me.lblMMMcGrm.Name = "lblMMMcGrm"
        Me.lblMMMcGrm.Size = New System.Drawing.Size(64, 21)
        Me.lblMMMcGrm.TabIndex = 12
        Me.lblMMMcGrm.Text = "Mc/Grm"
        Me.lblMMMcGrm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMMWastPer
        '
        Me.lblMMWastPer.BackColor = System.Drawing.Color.Transparent
        Me.lblMMWastPer.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMMWastPer.Location = New System.Drawing.Point(439, 17)
        Me.lblMMWastPer.Name = "lblMMWastPer"
        Me.lblMMWastPer.Size = New System.Drawing.Size(64, 21)
        Me.lblMMWastPer.TabIndex = 8
        Me.lblMMWastPer.Text = "Wast%"
        Me.lblMMWastPer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMMWeight
        '
        Me.lblMMWeight.BackColor = System.Drawing.Color.Transparent
        Me.lblMMWeight.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMMWeight.Location = New System.Drawing.Point(210, 17)
        Me.lblMMWeight.Name = "lblMMWeight"
        Me.lblMMWeight.Size = New System.Drawing.Size(81, 21)
        Me.lblMMWeight.TabIndex = 2
        Me.lblMMWeight.Text = "Weight"
        Me.lblMMWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtMMWeight_Wet
        '
        Me.txtMMWeight_Wet.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMMWeight_Wet.Location = New System.Drawing.Point(210, 38)
        Me.txtMMWeight_Wet.Name = "txtMMWeight_Wet"
        Me.txtMMWeight_Wet.Size = New System.Drawing.Size(81, 22)
        Me.txtMMWeight_Wet.TabIndex = 3
        Me.txtMMWeight_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RateChangeToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(180, 26)
        '
        'RateChangeToolStripMenuItem
        '
        Me.RateChangeToolStripMenuItem.Name = "RateChangeToolStripMenuItem"
        Me.RateChangeToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.RateChangeToolStripMenuItem.Size = New System.Drawing.Size(179, 22)
        Me.RateChangeToolStripMenuItem.Text = "RateChange"
        Me.RateChangeToolStripMenuItem.Visible = False
        '
        'frmMultiMetalDia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(973, 200)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpMultiMetal)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmMultiMetalDia"
        Me.Text = "Multimetal Detail"
        Me.grpMultiMetal.ResumeLayout(False)
        Me.grpMultiMetal.PerformLayout()
        CType(Me.gridMultimetal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridMultiMetalTotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpMultiMetal As CodeVendor.Controls.Grouper
    Friend WithEvents txtMMRate As System.Windows.Forms.TextBox
    Friend WithEvents txtMMRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents txtMMAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtMMMc_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtMMWastage_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtMMMcPerGRm_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtMMWastagePer_PER As System.Windows.Forms.TextBox
    Friend WithEvents txtMMCategory As System.Windows.Forms.TextBox
    Friend WithEvents gridMultimetal As System.Windows.Forms.DataGridView
    Friend WithEvents gridMultiMetalTotal As System.Windows.Forms.DataGridView
    Friend WithEvents Label108 As System.Windows.Forms.Label
    Friend WithEvents lblMMGrossAmt As System.Windows.Forms.Label
    Friend WithEvents lblMMMc As System.Windows.Forms.Label
    Friend WithEvents lblMMwastage As System.Windows.Forms.Label
    Friend WithEvents lblMMMcGrm As System.Windows.Forms.Label
    Friend WithEvents lblMMWastPer As System.Windows.Forms.Label
    Friend WithEvents lblMMWeight As System.Windows.Forms.Label
    Friend WithEvents txtMMWeight_Wet As System.Windows.Forms.TextBox
    Friend WithEvents lblMMRate As System.Windows.Forms.Label
    Friend WithEvents txtMMVAT_AMT As System.Windows.Forms.TextBox
    Friend WithEvents lblMMTax As System.Windows.Forms.Label
    Friend WithEvents txtMMNet_AMT As System.Windows.Forms.TextBox
    Friend WithEvents lblMMNetAmt As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RateChangeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblMMNetWt As Label
    Friend WithEvents txtMMNetWeight_Wet As TextBox
End Class
