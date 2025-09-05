<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmwGridDispDia
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmwGridDispDia))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tStripExcel = New System.Windows.Forms.ToolStripButton
        Me.tStripPrint = New System.Windows.Forms.ToolStripButton
        Me.tStripExit = New System.Windows.Forms.ToolStripButton
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.lblStatus = New System.Windows.Forms.Label
        Me.pnlBody = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.gridViewSubHeader = New System.Windows.Forms.DataGridView
        Me.gridViewHeader = New System.Windows.Forms.DataGridView
        Me.lblTitle = New System.Windows.Forms.Label
        Me.pnlHeader = New System.Windows.Forms.Panel
        Me.pnlFooter = New System.Windows.Forms.Panel
        Me.ToolStrip1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlBody.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridViewSubHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridViewHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHeader.SuspendLayout()
        Me.pnlFooter.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tStripExcel, Me.tStripPrint, Me.tStripExit})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1028, 25)
        Me.ToolStrip1.TabIndex = 9
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tStripExcel
        '
        Me.tStripExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripExcel.Image = CType(resources.GetObject("tStripExcel.Image"), System.Drawing.Image)
        Me.tStripExcel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripExcel.Name = "tStripExcel"
        Me.tStripExcel.Size = New System.Drawing.Size(23, 22)
        Me.tStripExcel.Text = "Export"
        Me.tStripExcel.ToolTipText = "Exel"
        '
        'tStripPrint
        '
        Me.tStripPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripPrint.Image = CType(resources.GetObject("tStripPrint.Image"), System.Drawing.Image)
        Me.tStripPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripPrint.Name = "tStripPrint"
        Me.tStripPrint.Size = New System.Drawing.Size(23, 22)
        Me.tStripPrint.Text = "Print"
        '
        'tStripExit
        '
        Me.tStripExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tStripExit.Image = Global.BrighttechRetailJewellery.My.Resources.Resources.exit_22
        Me.tStripExit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripExit.Name = "tStripExit"
        Me.tStripExit.Size = New System.Drawing.Size(23, 22)
        Me.tStripExit.Text = "Exit"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 26)
        '
        'lblStatus
        '
        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(0, 0)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(1024, 16)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Label1"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlBody
        '
        Me.pnlBody.Controls.Add(Me.gridView)
        Me.pnlBody.Controls.Add(Me.gridViewSubHeader)
        Me.pnlBody.Controls.Add(Me.gridViewHeader)
        Me.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlBody.Location = New System.Drawing.Point(0, 52)
        Me.pnlBody.Name = "pnlBody"
        Me.pnlBody.Size = New System.Drawing.Size(1028, 421)
        Me.pnlBody.TabIndex = 8
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 78)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(1028, 343)
        Me.gridView.TabIndex = 0
        '
        'gridViewSubHeader
        '
        Me.gridViewSubHeader.AllowUserToAddRows = False
        Me.gridViewSubHeader.AllowUserToDeleteRows = False
        Me.gridViewSubHeader.BackgroundColor = System.Drawing.Color.Red
        Me.gridViewSubHeader.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewSubHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewSubHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewSubHeader.Enabled = False
        Me.gridViewSubHeader.Location = New System.Drawing.Point(0, 38)
        Me.gridViewSubHeader.Name = "gridViewSubHeader"
        Me.gridViewSubHeader.ReadOnly = True
        Me.gridViewSubHeader.RowHeadersVisible = False
        Me.gridViewSubHeader.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridViewSubHeader.Size = New System.Drawing.Size(1028, 40)
        Me.gridViewSubHeader.TabIndex = 2
        Me.gridViewSubHeader.Visible = False
        '
        'gridViewHeader
        '
        Me.gridViewHeader.AllowUserToAddRows = False
        Me.gridViewHeader.AllowUserToDeleteRows = False
        Me.gridViewHeader.BackgroundColor = System.Drawing.SystemColors.ActiveCaption
        Me.gridViewHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHeader.Enabled = False
        Me.gridViewHeader.Location = New System.Drawing.Point(0, 0)
        Me.gridViewHeader.Name = "gridViewHeader"
        Me.gridViewHeader.ReadOnly = True
        Me.gridViewHeader.RowHeadersVisible = False
        Me.gridViewHeader.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridViewHeader.Size = New System.Drawing.Size(1028, 38)
        Me.gridViewHeader.TabIndex = 1
        Me.gridViewHeader.Visible = False
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1028, 27)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.lblTitle)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 25)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(1028, 27)
        Me.pnlHeader.TabIndex = 6
        '
        'pnlFooter
        '
        Me.pnlFooter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlFooter.Controls.Add(Me.lblStatus)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 473)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(1028, 20)
        Me.pnlFooter.TabIndex = 7
        Me.pnlFooter.Visible = False
        '
        'frmwGridDispDia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 493)
        Me.Controls.Add(Me.pnlBody)
        Me.Controls.Add(Me.pnlFooter)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "frmwGridDispDia"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlBody.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridViewSubHeader, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridViewHeader, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlFooter.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tStripPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripExcel As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tStripExit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents pnlBody As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents gridViewHeader As System.Windows.Forms.DataGridView
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents gridViewSubHeader As System.Windows.Forms.DataGridView
End Class
