<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInternalTransferRpt
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
        Me.pnltop = New System.Windows.Forms.Panel()
        Me.chkWithNoofTags = New System.Windows.Forms.CheckBox()
        Me.chkWithPurchaseValue = New System.Windows.Forms.CheckBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.BtnSplit = New System.Windows.Forms.Button()
        Me.lblHeading = New System.Windows.Forms.Label()
        Me.txtRefNo = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.chkCostCentreTo = New BrighttechPack.CheckedComboBox()
        Me.chkCmbCostCentrefrom = New BrighttechPack.CheckedComboBox()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnexit = New System.Windows.Forms.Button()
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.btnview = New System.Windows.Forms.Button()
        Me.btnnew = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dtpToDate = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrmDate = New BrighttechPack.DatePicker(Me.components)
        Me.lblEditDate = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.dGrid = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.gridHead = New System.Windows.Forms.DataGridView()
        Me.pnltop.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.dGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.gridHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnltop
        '
        Me.pnltop.Controls.Add(Me.chkWithNoofTags)
        Me.pnltop.Controls.Add(Me.chkWithPurchaseValue)
        Me.pnltop.Controls.Add(Me.lblStatus)
        Me.pnltop.Controls.Add(Me.BtnSplit)
        Me.pnltop.Controls.Add(Me.lblHeading)
        Me.pnltop.Controls.Add(Me.txtRefNo)
        Me.pnltop.Controls.Add(Me.Label5)
        Me.pnltop.Controls.Add(Me.cmbMetal)
        Me.pnltop.Controls.Add(Me.chkCostCentreTo)
        Me.pnltop.Controls.Add(Me.chkCmbCostCentrefrom)
        Me.pnltop.Controls.Add(Me.btnPrint)
        Me.pnltop.Controls.Add(Me.btnexit)
        Me.pnltop.Controls.Add(Me.btnExcel)
        Me.pnltop.Controls.Add(Me.btnview)
        Me.pnltop.Controls.Add(Me.btnnew)
        Me.pnltop.Controls.Add(Me.Label4)
        Me.pnltop.Controls.Add(Me.dtpToDate)
        Me.pnltop.Controls.Add(Me.dtpFrmDate)
        Me.pnltop.Controls.Add(Me.lblEditDate)
        Me.pnltop.Controls.Add(Me.Label3)
        Me.pnltop.Controls.Add(Me.Label2)
        Me.pnltop.Controls.Add(Me.Label1)
        Me.pnltop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnltop.Location = New System.Drawing.Point(0, 0)
        Me.pnltop.Name = "pnltop"
        Me.pnltop.Size = New System.Drawing.Size(938, 134)
        Me.pnltop.TabIndex = 0
        '
        'chkWithNoofTags
        '
        Me.chkWithNoofTags.AutoSize = True
        Me.chkWithNoofTags.Location = New System.Drawing.Point(651, 47)
        Me.chkWithNoofTags.Name = "chkWithNoofTags"
        Me.chkWithNoofTags.Size = New System.Drawing.Size(117, 17)
        Me.chkWithNoofTags.TabIndex = 13
        Me.chkWithNoofTags.Text = "With No Of Tags"
        Me.chkWithNoofTags.UseVisualStyleBackColor = True
        '
        'chkWithPurchaseValue
        '
        Me.chkWithPurchaseValue.AutoSize = True
        Me.chkWithPurchaseValue.Location = New System.Drawing.Point(503, 46)
        Me.chkWithPurchaseValue.Name = "chkWithPurchaseValue"
        Me.chkWithPurchaseValue.Size = New System.Drawing.Size(142, 17)
        Me.chkWithPurchaseValue.TabIndex = 12
        Me.chkWithPurchaseValue.Text = "With Purchase Value"
        Me.chkWithPurchaseValue.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(752, 83)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(0, 13)
        Me.lblStatus.TabIndex = 20
        '
        'BtnSplit
        '
        Me.BtnSplit.Location = New System.Drawing.Point(648, 74)
        Me.BtnSplit.Name = "BtnSplit"
        Me.BtnSplit.Size = New System.Drawing.Size(98, 30)
        Me.BtnSplit.TabIndex = 19
        Me.BtnSplit.Text = "Split by Type"
        Me.BtnSplit.UseVisualStyleBackColor = True
        '
        'lblHeading
        '
        Me.lblHeading.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblHeading.Location = New System.Drawing.Point(0, 107)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(938, 27)
        Me.lblHeading.TabIndex = 1
        Me.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtRefNo
        '
        Me.txtRefNo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.txtRefNo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.txtRefNo.Location = New System.Drawing.Point(381, 45)
        Me.txtRefNo.Name = "txtRefNo"
        Me.txtRefNo.Size = New System.Drawing.Size(116, 21)
        Me.txtRefNo.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(329, 48)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "RefNo."
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(640, 10)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(188, 21)
        Me.cmbMetal.TabIndex = 5
        '
        'chkCostCentreTo
        '
        Me.chkCostCentreTo.CheckOnClick = True
        Me.chkCostCentreTo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCostCentreTo.DropDownHeight = 1
        Me.chkCostCentreTo.FormattingEnabled = True
        Me.chkCostCentreTo.IntegralHeight = False
        Me.chkCostCentreTo.Location = New System.Drawing.Point(381, 10)
        Me.chkCostCentreTo.Name = "chkCostCentreTo"
        Me.chkCostCentreTo.Size = New System.Drawing.Size(188, 22)
        Me.chkCostCentreTo.TabIndex = 3
        Me.chkCostCentreTo.ValueSeparator = ", "
        '
        'chkCmbCostCentrefrom
        '
        Me.chkCmbCostCentrefrom.CheckOnClick = True
        Me.chkCmbCostCentrefrom.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentrefrom.DropDownHeight = 1
        Me.chkCmbCostCentrefrom.FormattingEnabled = True
        Me.chkCmbCostCentrefrom.IntegralHeight = False
        Me.chkCmbCostCentrefrom.Location = New System.Drawing.Point(131, 10)
        Me.chkCmbCostCentrefrom.Name = "chkCmbCostCentrefrom"
        Me.chkCmbCostCentrefrom.Size = New System.Drawing.Size(194, 22)
        Me.chkCmbCostCentrefrom.TabIndex = 1
        Me.chkCmbCostCentrefrom.ValueSeparator = ", "
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(439, 73)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(98, 30)
        Me.btnPrint.TabIndex = 17
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnexit
        '
        Me.btnexit.Location = New System.Drawing.Point(544, 74)
        Me.btnexit.Name = "btnexit"
        Me.btnexit.Size = New System.Drawing.Size(98, 30)
        Me.btnexit.TabIndex = 18
        Me.btnexit.Text = "Exit[F12]"
        Me.btnexit.UseVisualStyleBackColor = True
        '
        'btnExcel
        '
        Me.btnExcel.Location = New System.Drawing.Point(336, 73)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(98, 30)
        Me.btnExcel.TabIndex = 16
        Me.btnExcel.Text = "Export[X]"
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'btnview
        '
        Me.btnview.Location = New System.Drawing.Point(131, 74)
        Me.btnview.Name = "btnview"
        Me.btnview.Size = New System.Drawing.Size(98, 30)
        Me.btnview.TabIndex = 14
        Me.btnview.Text = "&View"
        Me.btnview.UseVisualStyleBackColor = True
        '
        'btnnew
        '
        Me.btnnew.Location = New System.Drawing.Point(233, 74)
        Me.btnnew.Name = "btnnew"
        Me.btnnew.Size = New System.Drawing.Size(98, 30)
        Me.btnnew.TabIndex = 15
        Me.btnnew.Text = "New[F3]"
        Me.btnnew.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(19, 48)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(67, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Date From"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpToDate
        '
        Me.dtpToDate.Location = New System.Drawing.Point(240, 45)
        Me.dtpToDate.Mask = "##/##/####"
        Me.dtpToDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpToDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpToDate.Size = New System.Drawing.Size(81, 21)
        Me.dtpToDate.TabIndex = 9
        Me.dtpToDate.Text = "07/03/9998"
        Me.dtpToDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrmDate
        '
        Me.dtpFrmDate.Location = New System.Drawing.Point(131, 45)
        Me.dtpFrmDate.Mask = "##/##/####"
        Me.dtpFrmDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrmDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrmDate.Name = "dtpFrmDate"
        Me.dtpFrmDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrmDate.Size = New System.Drawing.Size(81, 21)
        Me.dtpFrmDate.TabIndex = 7
        Me.dtpFrmDate.Text = "07/03/9998"
        Me.dtpFrmDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblEditDate
        '
        Me.lblEditDate.AutoSize = True
        Me.lblEditDate.Location = New System.Drawing.Point(215, 47)
        Me.lblEditDate.Name = "lblEditDate"
        Me.lblEditDate.Size = New System.Drawing.Size(20, 13)
        Me.lblEditDate.TabIndex = 8
        Me.lblEditDate.Text = "To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(577, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Metal"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(331, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(113, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Cost Centre From "
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.dGrid)
        Me.Panel2.Controls.Add(Me.gridHead)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 134)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(938, 435)
        Me.Panel2.TabIndex = 4
        '
        'dGrid
        '
        Me.dGrid.AllowUserToAddRows = False
        Me.dGrid.AllowUserToDeleteRows = False
        Me.dGrid.AllowUserToResizeColumns = False
        Me.dGrid.AllowUserToResizeRows = False
        Me.dGrid.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dGrid.ContextMenuStrip = Me.ContextMenuStrip1
        Me.dGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dGrid.Location = New System.Drawing.Point(0, 18)
        Me.dGrid.Name = "dGrid"
        Me.dGrid.RowHeadersVisible = False
        Me.dGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dGrid.Size = New System.Drawing.Size(938, 417)
        Me.dGrid.StandardTab = True
        Me.dGrid.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(107, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.ResizeToolStripMenuItem.Text = "Resize"
        '
        'gridHead
        '
        Me.gridHead.AllowUserToAddRows = False
        Me.gridHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridHead.Location = New System.Drawing.Point(0, 0)
        Me.gridHead.Name = "gridHead"
        Me.gridHead.RowHeadersVisible = False
        Me.gridHead.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridHead.Size = New System.Drawing.Size(938, 18)
        Me.gridHead.TabIndex = 0
        '
        'frmInternalTransferRpt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(938, 569)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.pnltop)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmInternalTransferRpt"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Internal Transfer Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnltop.ResumeLayout(False)
        Me.pnltop.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.dGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.gridHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnltop As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents chkCostCentreTo As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCostCentrefrom As BrighttechPack.CheckedComboBox
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnexit As System.Windows.Forms.Button
    Friend WithEvents btnExcel As System.Windows.Forms.Button
    Friend WithEvents btnview As System.Windows.Forms.Button
    Friend WithEvents btnnew As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents dtpToDate As BrighttechPack.DatePicker
    Friend WithEvents dtpFrmDate As BrighttechPack.DatePicker
    Friend WithEvents lblEditDate As System.Windows.Forms.Label
    Friend WithEvents lblHeading As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents dGrid As System.Windows.Forms.DataGridView
    Friend WithEvents gridHead As System.Windows.Forms.DataGridView
    Friend WithEvents txtRefNo As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BtnSplit As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents chkWithNoofTags As CheckBox
    Friend WithEvents chkWithPurchaseValue As CheckBox
End Class
