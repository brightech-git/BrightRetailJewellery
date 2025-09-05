<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReSend_Master
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
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSend = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbCostId = New System.Windows.Forms.ComboBox
        Me.gridView_OWN = New System.Windows.Forms.DataGridView
        Me.ChkAll = New System.Windows.Forms.CheckBox
        Me.ChkSelcostid = New System.Windows.Forms.CheckBox
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 26)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(266, 91)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 6
        Me.btnExit.Text = "Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSend
        '
        Me.btnSend.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSend.Location = New System.Drawing.Point(137, 91)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(100, 30)
        Me.btnSend.TabIndex = 5
        Me.btnSend.Text = "Generate"
        Me.btnSend.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(41, 29)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(68, 14)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "To CostId"
        '
        'cmbCostId
        '
        Me.cmbCostId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCostId.FormattingEnabled = True
        Me.cmbCostId.Location = New System.Drawing.Point(124, 26)
        Me.cmbCostId.Name = "cmbCostId"
        Me.cmbCostId.Size = New System.Drawing.Size(256, 21)
        Me.cmbCostId.TabIndex = 1
        '
        'gridView_OWN
        '
        Me.gridView_OWN.AllowUserToAddRows = False
        Me.gridView_OWN.AllowUserToDeleteRows = False
        Me.gridView_OWN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView_OWN.Location = New System.Drawing.Point(5, 127)
        Me.gridView_OWN.Name = "gridView_OWN"
        Me.gridView_OWN.ReadOnly = True
        Me.gridView_OWN.RowHeadersVisible = False
        Me.gridView_OWN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.gridView_OWN.Size = New System.Drawing.Size(416, 312)
        Me.gridView_OWN.TabIndex = 4
        '
        'ChkAll
        '
        Me.ChkAll.AutoSize = True
        Me.ChkAll.Location = New System.Drawing.Point(47, 64)
        Me.ChkAll.Name = "ChkAll"
        Me.ChkAll.Size = New System.Drawing.Size(80, 17)
        Me.ChkAll.TabIndex = 2
        Me.ChkAll.Text = "Check All"
        Me.ChkAll.UseVisualStyleBackColor = True
        '
        'ChkSelcostid
        '
        Me.ChkSelcostid.AutoSize = True
        Me.ChkSelcostid.Location = New System.Drawing.Point(133, 64)
        Me.ChkSelcostid.Name = "ChkSelcostid"
        Me.ChkSelcostid.Size = New System.Drawing.Size(187, 17)
        Me.ChkSelcostid.TabIndex = 3
        Me.ChkSelcostid.Text = "Selection Costid record only"
        Me.ChkSelcostid.UseVisualStyleBackColor = True
        '
        'ReSend_Master
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(421, 441)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.ChkSelcostid)
        Me.Controls.Add(Me.ChkAll)
        Me.Controls.Add(Me.gridView_OWN)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmbCostId)
        Me.Controls.Add(Me.btnSend)
        Me.Controls.Add(Me.btnExit)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "ReSend_Master"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Data Generate To Re Send "
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbCostId As System.Windows.Forms.ComboBox
    Friend WithEvents gridView_OWN As System.Windows.Forms.DataGridView
    Friend WithEvents ChkAll As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSelcostid As System.Windows.Forms.CheckBox
End Class
