<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDataView
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
        Me.dgv = New System.Windows.Forms.DataGridView
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tStripExcel = New System.Windows.Forms.ToolStripButton
        Me.tStripPrint = New System.Windows.Forms.ToolStripButton
        Me.tStripExit = New System.Windows.Forms.ToolStripButton
        CType(Me.dgv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgv
        '
        Me.dgv.AllowUserToAddRows = False
        Me.dgv.AllowUserToDeleteRows = False
        Me.dgv.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgv.Location = New System.Drawing.Point(0, 29)
        Me.dgv.Name = "dgv"
        Me.dgv.ReadOnly = True
        Me.dgv.Size = New System.Drawing.Size(938, 513)
        Me.dgv.TabIndex = 0
        '
        'ToolStrip1
        '
        Me.ToolStrip1.AutoSize = False
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tStripExcel, Me.tStripPrint, Me.tStripExit})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(938, 29)
        Me.ToolStrip1.TabIndex = 4
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tStripExcel
        '
        Me.tStripExcel.Image = Global.BrighttechPack.My.Resources.Resources.Excel_icon_22
        Me.tStripExcel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripExcel.Name = "tStripExcel"
        Me.tStripExcel.Size = New System.Drawing.Size(52, 37)
        Me.tStripExcel.Text = "&Excel"
        '
        'tStripPrint
        '
        Me.tStripPrint.Image = Global.BrighttechPack.My.Resources.Resources.Printer_22
        Me.tStripPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripPrint.Name = "tStripPrint"
        Me.tStripPrint.Size = New System.Drawing.Size(49, 37)
        Me.tStripPrint.Text = "&Print"
        '
        'tStripExit
        '
        Me.tStripExit.Image = Global.BrighttechPack.My.Resources.Resources.exit_22
        Me.tStripExit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripExit.Name = "tStripExit"
        Me.tStripExit.Size = New System.Drawing.Size(45, 37)
        Me.tStripExit.Text = "E&xit"
        '
        'frmDataView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(938, 542)
        Me.Controls.Add(Me.dgv)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmDataView"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DataView"
        CType(Me.dgv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tStripExcel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripExit As System.Windows.Forms.ToolStripButton
    Public WithEvents dgv As System.Windows.Forms.DataGridView
End Class
