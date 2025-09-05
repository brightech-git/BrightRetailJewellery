<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTagStuddedStoneDetails
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.grpStudDetails = New System.Windows.Forms.GroupBox()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.gridStudDetails = New System.Windows.Forms.DataGridView()
        Me.gridStudDetailsHead = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.grpStudDetails.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        CType(Me.gridStudDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridStudDetailsHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpStudDetails
        '
        Me.grpStudDetails.Controls.Add(Me.pnlGrid)
        Me.grpStudDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpStudDetails.Location = New System.Drawing.Point(0, 78)
        Me.grpStudDetails.Name = "grpStudDetails"
        Me.grpStudDetails.Size = New System.Drawing.Size(820, 263)
        Me.grpStudDetails.TabIndex = 9
        Me.grpStudDetails.TabStop = False
        Me.grpStudDetails.Text = "STUD DETAILS"
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.gridStudDetails)
        Me.pnlGrid.Controls.Add(Me.gridStudDetailsHead)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(3, 17)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(814, 243)
        Me.pnlGrid.TabIndex = 4
        '
        'gridStudDetails
        '
        Me.gridStudDetails.AllowUserToAddRows = False
        Me.gridStudDetails.AllowUserToDeleteRows = False
        Me.gridStudDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridStudDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridStudDetails.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.gridStudDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridStudDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridStudDetails.Location = New System.Drawing.Point(0, 17)
        Me.gridStudDetails.MultiSelect = False
        Me.gridStudDetails.Name = "gridStudDetails"
        Me.gridStudDetails.ReadOnly = True
        Me.gridStudDetails.RowHeadersVisible = False
        Me.gridStudDetails.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridStudDetails.RowTemplate.Height = 20
        Me.gridStudDetails.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridStudDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridStudDetails.Size = New System.Drawing.Size(814, 226)
        Me.gridStudDetails.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.gridStudDetails, "<ESC> EXIT")
        '
        'gridStudDetailsHead
        '
        Me.gridStudDetailsHead.AllowUserToAddRows = False
        Me.gridStudDetailsHead.AllowUserToDeleteRows = False
        Me.gridStudDetailsHead.AllowUserToResizeColumns = False
        Me.gridStudDetailsHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridStudDetailsHead.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridStudDetailsHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridStudDetailsHead.DefaultCellStyle = DataGridViewCellStyle2
        Me.gridStudDetailsHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridStudDetailsHead.Location = New System.Drawing.Point(0, 0)
        Me.gridStudDetailsHead.Name = "gridStudDetailsHead"
        Me.gridStudDetailsHead.ReadOnly = True
        Me.gridStudDetailsHead.Size = New System.Drawing.Size(814, 17)
        Me.gridStudDetailsHead.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(100, 9)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(567, 24)
        Me.lblTitle.TabIndex = 9
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(699, 38)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(99, 27)
        Me.btnPrint.TabIndex = 12
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(594, 38)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(99, 27)
        Me.btnExport.TabIndex = 11
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoResizeToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(136, 26)
        '
        'AutoResizeToolStripMenuItem
        '
        Me.AutoResizeToolStripMenuItem.CheckOnClick = True
        Me.AutoResizeToolStripMenuItem.Name = "AutoResizeToolStripMenuItem"
        Me.AutoResizeToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.AutoResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblTitle)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(820, 78)
        Me.Panel1.TabIndex = 14
        '
        'frmTagStuddedStoneDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(820, 341)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.grpStudDetails)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmTagStuddedStoneDetails"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.grpStudDetails.ResumeLayout(False)
        Me.pnlGrid.ResumeLayout(False)
        CType(Me.gridStudDetails, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridStudDetailsHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpStudDetails As System.Windows.Forms.GroupBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents gridStudDetails As System.Windows.Forms.DataGridView
    Friend WithEvents gridStudDetailsHead As System.Windows.Forms.DataGridView
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
End Class
