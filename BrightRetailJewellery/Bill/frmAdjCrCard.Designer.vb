<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdjCrCard
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
        Me.btnExit = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.btnOk = New System.Windows.Forms.Button
        Me.pnlBack = New System.Windows.Forms.Panel
        Me.txtGrid = New System.Windows.Forms.TextBox
        Me.gridViewTotal = New System.Windows.Forms.DataGridView
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.pnlBottom = New System.Windows.Forms.Panel
        Me.pnlLeft = New System.Windows.Forms.Panel
        Me.pnlRight = New System.Windows.Forms.Panel
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlBack.SuspendLayout()
        CType(Me.gridViewTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(6, 10)
        Me.gridView.Name = "gridView"
        Me.gridView.RowHeadersVisible = False
        Me.gridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.gridView.Size = New System.Drawing.Size(470, 158)
        Me.gridView.TabIndex = 0
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(367, 2)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 1
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
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
        Me.ExitToolStripMenuItem.Visible = False
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(261, 2)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 2
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'pnlBack
        '
        Me.pnlBack.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.pnlBack.Controls.Add(Me.txtGrid)
        Me.pnlBack.Controls.Add(Me.gridView)
        Me.pnlBack.Controls.Add(Me.gridViewTotal)
        Me.pnlBack.Controls.Add(Me.pnlTop)
        Me.pnlBack.Controls.Add(Me.pnlBottom)
        Me.pnlBack.Controls.Add(Me.pnlLeft)
        Me.pnlBack.Controls.Add(Me.pnlRight)
        Me.pnlBack.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlBack.Location = New System.Drawing.Point(0, 0)
        Me.pnlBack.Name = "pnlBack"
        Me.pnlBack.Size = New System.Drawing.Size(482, 220)
        Me.pnlBack.TabIndex = 3
        '
        'txtGrid
        '
        Me.txtGrid.Location = New System.Drawing.Point(191, 100)
        Me.txtGrid.Name = "txtGrid"
        Me.txtGrid.Size = New System.Drawing.Size(100, 21)
        Me.txtGrid.TabIndex = 30
        Me.txtGrid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtGrid.Visible = False
        '
        'gridViewTotal
        '
        Me.gridViewTotal.AllowUserToAddRows = False
        Me.gridViewTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewTotal.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.gridViewTotal.Enabled = False
        Me.gridViewTotal.Location = New System.Drawing.Point(6, 168)
        Me.gridViewTotal.Name = "gridViewTotal"
        Me.gridViewTotal.ReadOnly = True
        Me.gridViewTotal.Size = New System.Drawing.Size(470, 19)
        Me.gridViewTotal.TabIndex = 29
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(6, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(470, 10)
        Me.pnlTop.TabIndex = 6
        '
        'pnlBottom
        '
        Me.pnlBottom.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.pnlBottom.Controls.Add(Me.btnExit)
        Me.pnlBottom.Controls.Add(Me.btnOk)
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(6, 187)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(470, 33)
        Me.pnlBottom.TabIndex = 5
        '
        'pnlLeft
        '
        Me.pnlLeft.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlLeft.Location = New System.Drawing.Point(0, 0)
        Me.pnlLeft.Name = "pnlLeft"
        Me.pnlLeft.Size = New System.Drawing.Size(6, 220)
        Me.pnlLeft.TabIndex = 4
        '
        'pnlRight
        '
        Me.pnlRight.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.pnlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlRight.Location = New System.Drawing.Point(476, 0)
        Me.pnlRight.Name = "pnlRight"
        Me.pnlRight.Size = New System.Drawing.Size(6, 220)
        Me.pnlRight.TabIndex = 3
        '
        'frmAdjCrCard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(482, 220)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlBack)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "frmAdjCrCard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Credit Card Adjustment"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlBack.ResumeLayout(False)
        Me.pnlBack.PerformLayout()
        CType(Me.gridViewTotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlBottom.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents pnlBack As System.Windows.Forms.Panel
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents pnlBottom As System.Windows.Forms.Panel
    Friend WithEvents pnlLeft As System.Windows.Forms.Panel
    Friend WithEvents pnlRight As System.Windows.Forms.Panel
    Friend WithEvents gridViewTotal As System.Windows.Forms.DataGridView
    Friend WithEvents txtGrid As System.Windows.Forms.TextBox
End Class
