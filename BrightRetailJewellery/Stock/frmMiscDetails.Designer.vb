<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMiscDetails
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.grpOtherCharges = New System.Windows.Forms.GroupBox()
        Me.pnlMisc = New System.Windows.Forms.Panel()
        Me.cmbMiscDetails = New System.Windows.Forms.ComboBox()
        Me.txtMiscRowIndex = New System.Windows.Forms.TextBox()
        Me.txtMiscTotAmt = New System.Windows.Forms.TextBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label71 = New System.Windows.Forms.Label()
        Me.Label72 = New System.Windows.Forms.Label()
        Me.gridMisc = New System.Windows.Forms.DataGridView()
        Me.gridMiscFooter = New System.Windows.Forms.DataGridView()
        Me.txtMiscAmount_AMT = New System.Windows.Forms.TextBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ClearToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grpOtherCharges.SuspendLayout()
        Me.pnlMisc.SuspendLayout()
        CType(Me.gridMisc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridMiscFooter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpOtherCharges
        '
        Me.grpOtherCharges.Controls.Add(Me.pnlMisc)
        Me.grpOtherCharges.Location = New System.Drawing.Point(10, 4)
        Me.grpOtherCharges.Name = "grpOtherCharges"
        Me.grpOtherCharges.Size = New System.Drawing.Size(431, 156)
        Me.grpOtherCharges.TabIndex = 14
        Me.grpOtherCharges.TabStop = False
        '
        'pnlMisc
        '
        Me.pnlMisc.Controls.Add(Me.cmbMiscDetails)
        Me.pnlMisc.Controls.Add(Me.txtMiscRowIndex)
        Me.pnlMisc.Controls.Add(Me.txtMiscTotAmt)
        Me.pnlMisc.Controls.Add(Me.Label31)
        Me.pnlMisc.Controls.Add(Me.Label71)
        Me.pnlMisc.Controls.Add(Me.Label72)
        Me.pnlMisc.Controls.Add(Me.gridMisc)
        Me.pnlMisc.Controls.Add(Me.gridMiscFooter)
        Me.pnlMisc.Controls.Add(Me.txtMiscAmount_AMT)
        Me.pnlMisc.Location = New System.Drawing.Point(6, 15)
        Me.pnlMisc.Name = "pnlMisc"
        Me.pnlMisc.Size = New System.Drawing.Size(418, 135)
        Me.pnlMisc.TabIndex = 0
        '
        'cmbMiscDetails
        '
        Me.cmbMiscDetails.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMiscDetails.FormattingEnabled = True
        Me.cmbMiscDetails.Location = New System.Drawing.Point(0, 22)
        Me.cmbMiscDetails.Name = "cmbMiscDetails"
        Me.cmbMiscDetails.Size = New System.Drawing.Size(298, 21)
        Me.cmbMiscDetails.TabIndex = 39
        '
        'txtMiscRowIndex
        '
        Me.txtMiscRowIndex.Location = New System.Drawing.Point(87, 71)
        Me.txtMiscRowIndex.Name = "txtMiscRowIndex"
        Me.txtMiscRowIndex.Size = New System.Drawing.Size(43, 21)
        Me.txtMiscRowIndex.TabIndex = 38
        Me.txtMiscRowIndex.Visible = False
        '
        'txtMiscTotAmt
        '
        Me.txtMiscTotAmt.Enabled = False
        Me.txtMiscTotAmt.Location = New System.Drawing.Point(197, 71)
        Me.txtMiscTotAmt.Name = "txtMiscTotAmt"
        Me.txtMiscTotAmt.Size = New System.Drawing.Size(76, 21)
        Me.txtMiscTotAmt.TabIndex = 37
        Me.txtMiscTotAmt.Visible = False
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Enabled = False
        Me.Label31.Location = New System.Drawing.Point(194, 55)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(82, 13)
        Me.Label31.TabIndex = 36
        Me.Label31.Text = "Total Amount"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label31.Visible = False
        '
        'Label71
        '
        Me.Label71.BackColor = System.Drawing.SystemColors.Control
        Me.Label71.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label71.Location = New System.Drawing.Point(0, 1)
        Me.Label71.Name = "Label71"
        Me.Label71.Size = New System.Drawing.Size(298, 20)
        Me.Label71.TabIndex = 0
        Me.Label71.Text = "Miscellaneous"
        Me.Label71.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label72
        '
        Me.Label72.BackColor = System.Drawing.SystemColors.Control
        Me.Label72.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label72.Location = New System.Drawing.Point(299, 1)
        Me.Label72.Name = "Label72"
        Me.Label72.Size = New System.Drawing.Size(99, 20)
        Me.Label72.TabIndex = 2
        Me.Label72.Text = "Amount"
        Me.Label72.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gridMisc
        '
        Me.gridMisc.AllowUserToAddRows = False
        Me.gridMisc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridMisc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridMisc.ColumnHeadersVisible = False
        Me.gridMisc.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.gridMisc.Location = New System.Drawing.Point(0, 45)
        Me.gridMisc.Name = "gridMisc"
        Me.gridMisc.ReadOnly = True
        Me.gridMisc.RowHeadersVisible = False
        Me.gridMisc.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridMisc.RowTemplate.Height = 20
        Me.gridMisc.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridMisc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridMisc.Size = New System.Drawing.Size(418, 71)
        Me.gridMisc.TabIndex = 4
        '
        'gridMiscFooter
        '
        Me.gridMiscFooter.AllowUserToAddRows = False
        Me.gridMiscFooter.AllowUserToDeleteRows = False
        Me.gridMiscFooter.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridMiscFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridMiscFooter.ColumnHeadersVisible = False
        Me.gridMiscFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.gridMiscFooter.Enabled = False
        Me.gridMiscFooter.Location = New System.Drawing.Point(0, 116)
        Me.gridMiscFooter.Name = "gridMiscFooter"
        Me.gridMiscFooter.ReadOnly = True
        Me.gridMiscFooter.RowHeadersVisible = False
        Me.gridMiscFooter.Size = New System.Drawing.Size(418, 19)
        Me.gridMiscFooter.TabIndex = 5
        '
        'txtMiscAmount_AMT
        '
        Me.txtMiscAmount_AMT.Location = New System.Drawing.Point(299, 22)
        Me.txtMiscAmount_AMT.Name = "txtMiscAmount_AMT"
        Me.txtMiscAmount_AMT.Size = New System.Drawing.Size(99, 21)
        Me.txtMiscAmount_AMT.TabIndex = 3
        Me.txtMiscAmount_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClearToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(121, 26)
        '
        'ClearToolStripMenuItem
        '
        Me.ClearToolStripMenuItem.Name = "ClearToolStripMenuItem"
        Me.ClearToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.ClearToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ClearToolStripMenuItem.Text = "Clear"
        '
        'frmMiscDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(451, 170)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.grpOtherCharges)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmMiscDetails"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Misc Details"
        Me.grpOtherCharges.ResumeLayout(False)
        Me.pnlMisc.ResumeLayout(False)
        Me.pnlMisc.PerformLayout()
        CType(Me.gridMisc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridMiscFooter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpOtherCharges As GroupBox
    Friend WithEvents pnlMisc As Panel
    Friend WithEvents Label71 As Label
    Friend WithEvents Label72 As Label
    Friend WithEvents gridMisc As DataGridView
    Friend WithEvents gridMiscFooter As DataGridView
    Friend WithEvents txtMiscAmount_AMT As TextBox
    Friend WithEvents txtMiscRowIndex As TextBox
    Friend WithEvents txtMiscTotAmt As TextBox
    Friend WithEvents Label31 As Label
    Friend WithEvents cmbMiscDetails As ComboBox
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents ClearToolStripMenuItem As ToolStripMenuItem
End Class
