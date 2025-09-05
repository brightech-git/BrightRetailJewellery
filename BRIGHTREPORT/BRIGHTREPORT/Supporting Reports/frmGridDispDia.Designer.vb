<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGridDispDia
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGridDispDia))
        Me.pnlHeader = New System.Windows.Forms.Panel
        Me.lblTitle = New System.Windows.Forms.Label
        Me.pnlBody = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.gridViewHeader = New System.Windows.Forms.DataGridView
        Me.pnlFooter = New System.Windows.Forms.Panel
        Me.lblApproval = New System.Windows.Forms.Label
        Me.lblAppIss = New System.Windows.Forms.Label
        Me.lblReadyforDel = New System.Windows.Forms.Label
        Me.lblReady = New System.Windows.Forms.Label
        Me.lblCancelled = New System.Windows.Forms.Label
        Me.lblCancel = New System.Windows.Forms.Label
        Me.lblRecSmith = New System.Windows.Forms.Label
        Me.lblRec = New System.Windows.Forms.Label
        Me.lblPendingWithUs = New System.Windows.Forms.Label
        Me.lblPending = New System.Windows.Forms.Label
        Me.lblDeliver = New System.Windows.Forms.Label
        Me.lblIsstoSmith = New System.Windows.Forms.Label
        Me.lblIss = New System.Windows.Forms.Label
        Me.lblDelivered = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tStripExcel = New System.Windows.Forms.ToolStripButton
        Me.tStripPrint = New System.Windows.Forms.ToolStripButton
        Me.tStripExit = New System.Windows.Forms.ToolStripButton
        Me.TStripDetPrint = New System.Windows.Forms.ToolStripButton
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblRemark1 = New System.Windows.Forms.Label
        Me.lblRemark2 = New System.Windows.Forms.Label
        Me.txtRemark1 = New System.Windows.Forms.TextBox
        Me.txtRemark2 = New System.Windows.Forms.TextBox
        Me.pnlHeader.SuspendLayout()
        Me.pnlBody.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        CType(Me.gridViewHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlFooter.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.lblTitle)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 25)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(943, 39)
        Me.pnlHeader.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(943, 39)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlBody
        '
        Me.pnlBody.Controls.Add(Me.gridView)
        Me.pnlBody.Controls.Add(Me.gridViewHeader)
        Me.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlBody.Location = New System.Drawing.Point(0, 64)
        Me.pnlBody.Name = "pnlBody"
        Me.pnlBody.Size = New System.Drawing.Size(943, 407)
        Me.pnlBody.TabIndex = 0
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 19)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(943, 388)
        Me.gridView.TabIndex = 0
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(143, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'gridViewHeader
        '
        Me.gridViewHeader.AllowUserToAddRows = False
        Me.gridViewHeader.AllowUserToDeleteRows = False
        Me.gridViewHeader.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHeader.Enabled = False
        Me.gridViewHeader.Location = New System.Drawing.Point(0, 0)
        Me.gridViewHeader.Name = "gridViewHeader"
        Me.gridViewHeader.ReadOnly = True
        Me.gridViewHeader.RowHeadersVisible = False
        Me.gridViewHeader.Size = New System.Drawing.Size(943, 19)
        Me.gridViewHeader.TabIndex = 1
        Me.gridViewHeader.Visible = False
        '
        'pnlFooter
        '
        Me.pnlFooter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlFooter.Controls.Add(Me.txtRemark2)
        Me.pnlFooter.Controls.Add(Me.lblApproval)
        Me.pnlFooter.Controls.Add(Me.txtRemark1)
        Me.pnlFooter.Controls.Add(Me.lblAppIss)
        Me.pnlFooter.Controls.Add(Me.lblRemark2)
        Me.pnlFooter.Controls.Add(Me.lblReadyforDel)
        Me.pnlFooter.Controls.Add(Me.lblRemark1)
        Me.pnlFooter.Controls.Add(Me.lblReady)
        Me.pnlFooter.Controls.Add(Me.lblCancelled)
        Me.pnlFooter.Controls.Add(Me.lblCancel)
        Me.pnlFooter.Controls.Add(Me.lblRecSmith)
        Me.pnlFooter.Controls.Add(Me.lblRec)
        Me.pnlFooter.Controls.Add(Me.lblPendingWithUs)
        Me.pnlFooter.Controls.Add(Me.lblPending)
        Me.pnlFooter.Controls.Add(Me.lblDeliver)
        Me.pnlFooter.Controls.Add(Me.lblIsstoSmith)
        Me.pnlFooter.Controls.Add(Me.lblIss)
        Me.pnlFooter.Controls.Add(Me.lblDelivered)
        Me.pnlFooter.Controls.Add(Me.lblStatus)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 471)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(943, 31)
        Me.pnlFooter.TabIndex = 0
        Me.pnlFooter.Visible = False
        '
        'lblApproval
        '
        Me.lblApproval.AutoSize = True
        Me.lblApproval.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblApproval.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblApproval.ForeColor = System.Drawing.Color.Blue
        Me.lblApproval.Location = New System.Drawing.Point(852, 1)
        Me.lblApproval.Name = "lblApproval"
        Me.lblApproval.Size = New System.Drawing.Size(116, 13)
        Me.lblApproval.TabIndex = 42
        Me.lblApproval.Text = "APPROVAL ISSUE"
        Me.lblApproval.Visible = False
        '
        'lblAppIss
        '
        Me.lblAppIss.BackColor = System.Drawing.Color.LightSkyBlue
        Me.lblAppIss.Location = New System.Drawing.Point(829, 0)
        Me.lblAppIss.Name = "lblAppIss"
        Me.lblAppIss.Size = New System.Drawing.Size(18, 17)
        Me.lblAppIss.TabIndex = 41
        Me.lblAppIss.Visible = False
        '
        'lblReadyforDel
        '
        Me.lblReadyforDel.AutoSize = True
        Me.lblReadyforDel.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblReadyforDel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadyforDel.ForeColor = System.Drawing.Color.Blue
        Me.lblReadyforDel.Location = New System.Drawing.Point(679, 1)
        Me.lblReadyforDel.Name = "lblReadyforDel"
        Me.lblReadyforDel.Size = New System.Drawing.Size(144, 13)
        Me.lblReadyforDel.TabIndex = 40
        Me.lblReadyforDel.Text = "READY FOR DELIVERY"
        Me.lblReadyforDel.Visible = False
        '
        'lblReady
        '
        Me.lblReady.BackColor = System.Drawing.Color.Orange
        Me.lblReady.Location = New System.Drawing.Point(656, 1)
        Me.lblReady.Name = "lblReady"
        Me.lblReady.Size = New System.Drawing.Size(18, 17)
        Me.lblReady.TabIndex = 39
        Me.lblReady.Visible = False
        '
        'lblCancelled
        '
        Me.lblCancelled.AutoSize = True
        Me.lblCancelled.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblCancelled.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCancelled.ForeColor = System.Drawing.Color.Blue
        Me.lblCancelled.Location = New System.Drawing.Point(570, 3)
        Me.lblCancelled.Name = "lblCancelled"
        Me.lblCancelled.Size = New System.Drawing.Size(80, 13)
        Me.lblCancelled.TabIndex = 38
        Me.lblCancelled.Text = "CANCELLED"
        Me.lblCancelled.Visible = False
        '
        'lblCancel
        '
        Me.lblCancel.BackColor = System.Drawing.Color.Red
        Me.lblCancel.Location = New System.Drawing.Point(547, 2)
        Me.lblCancel.Name = "lblCancel"
        Me.lblCancel.Size = New System.Drawing.Size(18, 17)
        Me.lblCancel.TabIndex = 37
        Me.lblCancel.Visible = False
        '
        'lblRecSmith
        '
        Me.lblRecSmith.AutoSize = True
        Me.lblRecSmith.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblRecSmith.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecSmith.ForeColor = System.Drawing.Color.Blue
        Me.lblRecSmith.Location = New System.Drawing.Point(427, 3)
        Me.lblRecSmith.Name = "lblRecSmith"
        Me.lblRecSmith.Size = New System.Drawing.Size(115, 13)
        Me.lblRecSmith.TabIndex = 36
        Me.lblRecSmith.Text = "REC FROM SMITH"
        Me.lblRecSmith.Visible = False
        '
        'lblRec
        '
        Me.lblRec.BackColor = System.Drawing.Color.RosyBrown
        Me.lblRec.Location = New System.Drawing.Point(404, 2)
        Me.lblRec.Name = "lblRec"
        Me.lblRec.Size = New System.Drawing.Size(18, 17)
        Me.lblRec.TabIndex = 35
        Me.lblRec.Visible = False
        '
        'lblPendingWithUs
        '
        Me.lblPendingWithUs.AutoSize = True
        Me.lblPendingWithUs.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblPendingWithUs.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPendingWithUs.ForeColor = System.Drawing.Color.Blue
        Me.lblPendingWithUs.Location = New System.Drawing.Point(274, 3)
        Me.lblPendingWithUs.Name = "lblPendingWithUs"
        Me.lblPendingWithUs.Size = New System.Drawing.Size(125, 13)
        Me.lblPendingWithUs.TabIndex = 34
        Me.lblPendingWithUs.Text = "PENDING WITH US"
        Me.lblPendingWithUs.Visible = False
        '
        'lblPending
        '
        Me.lblPending.BackColor = System.Drawing.Color.Wheat
        Me.lblPending.Location = New System.Drawing.Point(251, 2)
        Me.lblPending.Name = "lblPending"
        Me.lblPending.Size = New System.Drawing.Size(18, 17)
        Me.lblPending.TabIndex = 33
        Me.lblPending.Visible = False
        '
        'lblDeliver
        '
        Me.lblDeliver.BackColor = System.Drawing.Color.LightGreen
        Me.lblDeliver.Location = New System.Drawing.Point(3, 2)
        Me.lblDeliver.Name = "lblDeliver"
        Me.lblDeliver.Size = New System.Drawing.Size(18, 17)
        Me.lblDeliver.TabIndex = 32
        Me.lblDeliver.Visible = False
        '
        'lblIsstoSmith
        '
        Me.lblIsstoSmith.AutoSize = True
        Me.lblIsstoSmith.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblIsstoSmith.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsstoSmith.ForeColor = System.Drawing.Color.Blue
        Me.lblIsstoSmith.Location = New System.Drawing.Point(135, 3)
        Me.lblIsstoSmith.Name = "lblIsstoSmith"
        Me.lblIsstoSmith.Size = New System.Drawing.Size(112, 13)
        Me.lblIsstoSmith.TabIndex = 31
        Me.lblIsstoSmith.Text = "ISSUE TO SMITH"
        Me.lblIsstoSmith.Visible = False
        '
        'lblIss
        '
        Me.lblIss.BackColor = System.Drawing.Color.LightPink
        Me.lblIss.Location = New System.Drawing.Point(112, 2)
        Me.lblIss.Name = "lblIss"
        Me.lblIss.Size = New System.Drawing.Size(18, 17)
        Me.lblIss.TabIndex = 30
        Me.lblIss.Visible = False
        '
        'lblDelivered
        '
        Me.lblDelivered.AutoSize = True
        Me.lblDelivered.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblDelivered.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDelivered.ForeColor = System.Drawing.Color.Blue
        Me.lblDelivered.Location = New System.Drawing.Point(24, 3)
        Me.lblDelivered.Name = "lblDelivered"
        Me.lblDelivered.Size = New System.Drawing.Size(78, 13)
        Me.lblDelivered.TabIndex = 29
        Me.lblDelivered.Text = "DELIVERED"
        Me.lblDelivered.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(0, 0)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(939, 27)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Label1"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tStripExcel, Me.tStripPrint, Me.tStripExit, Me.TStripDetPrint})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(943, 25)
        Me.ToolStrip1.TabIndex = 5
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
        Me.tStripExit.Image = Global.BrighttechREPORT.My.Resources.Resources.exit_22
        Me.tStripExit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tStripExit.Name = "tStripExit"
        Me.tStripExit.Size = New System.Drawing.Size(23, 22)
        Me.tStripExit.Text = "Exit"
        '
        'TStripDetPrint
        '
        Me.TStripDetPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TStripDetPrint.Image = Global.BrighttechREPORT.My.Resources.Resources.notes_22
        Me.TStripDetPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TStripDetPrint.Name = "TStripDetPrint"
        Me.TStripDetPrint.Size = New System.Drawing.Size(23, 22)
        Me.TStripDetPrint.Text = "Detail Print"
        Me.TStripDetPrint.Visible = False
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
        'lblRemark1
        '
        Me.lblRemark1.AutoSize = True
        Me.lblRemark1.Location = New System.Drawing.Point(2, 6)
        Me.lblRemark1.Name = "lblRemark1"
        Me.lblRemark1.Size = New System.Drawing.Size(59, 13)
        Me.lblRemark1.TabIndex = 2
        Me.lblRemark1.Text = "Remark1"
        Me.lblRemark1.Visible = False
        '
        'lblRemark2
        '
        Me.lblRemark2.AutoSize = True
        Me.lblRemark2.Location = New System.Drawing.Point(326, 7)
        Me.lblRemark2.Name = "lblRemark2"
        Me.lblRemark2.Size = New System.Drawing.Size(59, 13)
        Me.lblRemark2.TabIndex = 3
        Me.lblRemark2.Text = "Remark2"
        Me.lblRemark2.Visible = False
        '
        'txtRemark1
        '
        Me.txtRemark1.Location = New System.Drawing.Point(67, 3)
        Me.txtRemark1.Name = "txtRemark1"
        Me.txtRemark1.Size = New System.Drawing.Size(250, 21)
        Me.txtRemark1.TabIndex = 4
        Me.txtRemark1.Visible = False
        '
        'txtRemark2
        '
        Me.txtRemark2.Location = New System.Drawing.Point(391, 4)
        Me.txtRemark2.Name = "txtRemark2"
        Me.txtRemark2.Size = New System.Drawing.Size(250, 21)
        Me.txtRemark2.TabIndex = 5
        Me.txtRemark2.Visible = False
        '
        'frmGridDispDia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(943, 502)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.pnlBody)
        Me.Controls.Add(Me.pnlHeader)
        Me.Controls.Add(Me.pnlFooter)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmGridDispDia"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Summary View"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlBody.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        CType(Me.gridViewHeader, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlFooter.ResumeLayout(False)
        Me.pnlFooter.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents pnlBody As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tStripExcel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents tStripExit As System.Windows.Forms.ToolStripButton
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents gridViewHeader As System.Windows.Forms.DataGridView
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TStripDetPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblCancelled As System.Windows.Forms.Label
    Friend WithEvents lblCancel As System.Windows.Forms.Label
    Friend WithEvents lblRecSmith As System.Windows.Forms.Label
    Friend WithEvents lblRec As System.Windows.Forms.Label
    Friend WithEvents lblPendingWithUs As System.Windows.Forms.Label
    Friend WithEvents lblPending As System.Windows.Forms.Label
    Friend WithEvents lblDeliver As System.Windows.Forms.Label
    Friend WithEvents lblIsstoSmith As System.Windows.Forms.Label
    Friend WithEvents lblIss As System.Windows.Forms.Label
    Friend WithEvents lblDelivered As System.Windows.Forms.Label
    Friend WithEvents lblReadyforDel As System.Windows.Forms.Label
    Friend WithEvents lblReady As System.Windows.Forms.Label
    Friend WithEvents lblApproval As System.Windows.Forms.Label
    Friend WithEvents lblAppIss As System.Windows.Forms.Label
    Friend WithEvents txtRemark2 As System.Windows.Forms.TextBox
    Friend WithEvents txtRemark1 As System.Windows.Forms.TextBox
    Friend WithEvents lblRemark2 As System.Windows.Forms.Label
    Friend WithEvents lblRemark1 As System.Windows.Forms.Label
End Class
