<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmItemTagMiscCharge
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpOtherCharges = New System.Windows.Forms.GroupBox
        Me.pnlMisc = New System.Windows.Forms.Panel
        Me.txtMiscMisc = New System.Windows.Forms.TextBox
        Me.Label71 = New System.Windows.Forms.Label
        Me.Label72 = New System.Windows.Forms.Label
        Me.gridMisc = New System.Windows.Forms.DataGridView
        Me.gridMiscFooter = New System.Windows.Forms.DataGridView
        Me.txtMiscAmount_Amt = New System.Windows.Forms.TextBox
        Me.txtMiscTotAmt = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.txtMiscRowIndex = New System.Windows.Forms.TextBox
        Me.ContextMenuStrip1.SuspendLayout()
        Me.grpOtherCharges.SuspendLayout()
        Me.pnlMisc.SuspendLayout()
        CType(Me.gridMisc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridMiscFooter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "Save"
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
        'grpOtherCharges
        '
        Me.grpOtherCharges.Controls.Add(Me.pnlMisc)
        Me.grpOtherCharges.Location = New System.Drawing.Point(13, 12)
        Me.grpOtherCharges.Name = "grpOtherCharges"
        Me.grpOtherCharges.Size = New System.Drawing.Size(431, 156)
        Me.grpOtherCharges.TabIndex = 1
        Me.grpOtherCharges.TabStop = False
        '
        'pnlMisc
        '
        Me.pnlMisc.Controls.Add(Me.txtMiscMisc)
        Me.pnlMisc.Controls.Add(Me.Label71)
        Me.pnlMisc.Controls.Add(Me.Label72)
        Me.pnlMisc.Controls.Add(Me.gridMisc)
        Me.pnlMisc.Controls.Add(Me.gridMiscFooter)
        Me.pnlMisc.Controls.Add(Me.txtMiscAmount_Amt)
        Me.pnlMisc.Location = New System.Drawing.Point(6, 14)
        Me.pnlMisc.Name = "pnlMisc"
        Me.pnlMisc.Size = New System.Drawing.Size(418, 135)
        Me.pnlMisc.TabIndex = 0
        '
        'txtMiscMisc
        '
        Me.txtMiscMisc.Location = New System.Drawing.Point(0, 22)
        Me.txtMiscMisc.Name = "txtMiscMisc"
        Me.txtMiscMisc.Size = New System.Drawing.Size(298, 21)
        Me.txtMiscMisc.TabIndex = 1
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
        'txtMiscAmount_Amt
        '
        Me.txtMiscAmount_Amt.Location = New System.Drawing.Point(299, 22)
        Me.txtMiscAmount_Amt.Name = "txtMiscAmount_Amt"
        Me.txtMiscAmount_Amt.Size = New System.Drawing.Size(99, 21)
        Me.txtMiscAmount_Amt.TabIndex = 3
        Me.txtMiscAmount_Amt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMiscTotAmt
        '
        Me.txtMiscTotAmt.Enabled = False
        Me.txtMiscTotAmt.Location = New System.Drawing.Point(318, 174)
        Me.txtMiscTotAmt.Name = "txtMiscTotAmt"
        Me.txtMiscTotAmt.Size = New System.Drawing.Size(99, 21)
        Me.txtMiscTotAmt.TabIndex = 33
        Me.txtMiscTotAmt.Visible = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Enabled = False
        Me.Label8.Location = New System.Drawing.Point(233, 178)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(83, 13)
        Me.Label8.TabIndex = 32
        Me.Label8.Text = "Total Amount"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label8.Visible = False
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(231, 217)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 67
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(125, 217)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 66
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'txtMiscRowIndex
        '
        Me.txtMiscRowIndex.Location = New System.Drawing.Point(19, 174)
        Me.txtMiscRowIndex.Name = "txtMiscRowIndex"
        Me.txtMiscRowIndex.Size = New System.Drawing.Size(43, 21)
        Me.txtMiscRowIndex.TabIndex = 68
        Me.txtMiscRowIndex.Visible = False
        '
        'frmItemTagMiscCharge
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(457, 259)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.txtMiscRowIndex)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtMiscTotAmt)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.grpOtherCharges)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmItemTagMiscCharge"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Item Tag Misc Charge"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.grpOtherCharges.ResumeLayout(False)
        Me.pnlMisc.ResumeLayout(False)
        Me.pnlMisc.PerformLayout()
        CType(Me.gridMisc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridMiscFooter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents grpOtherCharges As System.Windows.Forms.GroupBox
    Friend WithEvents pnlMisc As System.Windows.Forms.Panel
    Friend WithEvents txtMiscMisc As System.Windows.Forms.TextBox
    Friend WithEvents Label71 As System.Windows.Forms.Label
    Friend WithEvents Label72 As System.Windows.Forms.Label
    Friend WithEvents gridMisc As System.Windows.Forms.DataGridView
    Friend WithEvents gridMiscFooter As System.Windows.Forms.DataGridView
    Friend WithEvents txtMiscAmount_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtMiscTotAmt As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents txtMiscRowIndex As System.Windows.Forms.TextBox
End Class
