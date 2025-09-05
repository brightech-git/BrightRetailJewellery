<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmItemHallmarkDetalisUpdate
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.grpHallmarkDetails = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txthallmarkRowIndex = New System.Windows.Forms.TextBox()
        Me.txtTagWt_WET = New System.Windows.Forms.TextBox()
        Me.lblTagWt = New System.Windows.Forms.Label()
        Me.txtHallmarkNo = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.gridHallmarkDetails = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.grpHallmarkDetails.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.gridHallmarkDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NewToolStripMenuItem.Text = "Save"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(219, 179)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 2
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(118, 179)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'grpHallmarkDetails
        '
        Me.grpHallmarkDetails.Controls.Add(Me.Panel1)
        Me.grpHallmarkDetails.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpHallmarkDetails.Location = New System.Drawing.Point(0, 0)
        Me.grpHallmarkDetails.Name = "grpHallmarkDetails"
        Me.grpHallmarkDetails.Size = New System.Drawing.Size(424, 174)
        Me.grpHallmarkDetails.TabIndex = 0
        Me.grpHallmarkDetails.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txthallmarkRowIndex)
        Me.Panel1.Controls.Add(Me.txtTagWt_WET)
        Me.Panel1.Controls.Add(Me.lblTagWt)
        Me.Panel1.Controls.Add(Me.txtHallmarkNo)
        Me.Panel1.Controls.Add(Me.Label15)
        Me.Panel1.Controls.Add(Me.gridHallmarkDetails)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 17)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(418, 154)
        Me.Panel1.TabIndex = 0
        '
        'txthallmarkRowIndex
        '
        Me.txthallmarkRowIndex.Location = New System.Drawing.Point(482, 17)
        Me.txthallmarkRowIndex.Name = "txthallmarkRowIndex"
        Me.txthallmarkRowIndex.Size = New System.Drawing.Size(43, 21)
        Me.txthallmarkRowIndex.TabIndex = 4
        Me.txthallmarkRowIndex.Visible = False
        '
        'txtTagWt_WET
        '
        Me.txtTagWt_WET.Location = New System.Drawing.Point(0, 26)
        Me.txtTagWt_WET.MaxLength = 20
        Me.txtTagWt_WET.Name = "txtTagWt_WET"
        Me.txtTagWt_WET.Size = New System.Drawing.Size(218, 21)
        Me.txtTagWt_WET.TabIndex = 2
        Me.txtTagWt_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblTagWt
        '
        Me.lblTagWt.BackColor = System.Drawing.SystemColors.Control
        Me.lblTagWt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTagWt.Enabled = False
        Me.lblTagWt.Location = New System.Drawing.Point(0, 1)
        Me.lblTagWt.Name = "lblTagWt"
        Me.lblTagWt.Size = New System.Drawing.Size(218, 20)
        Me.lblTagWt.TabIndex = 0
        Me.lblTagWt.Text = "Tag Wt"
        Me.lblTagWt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtHallmarkNo
        '
        Me.txtHallmarkNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtHallmarkNo.Location = New System.Drawing.Point(219, 26)
        Me.txtHallmarkNo.MaxLength = 20
        Me.txtHallmarkNo.Name = "txtHallmarkNo"
        Me.txtHallmarkNo.Size = New System.Drawing.Size(199, 21)
        Me.txtHallmarkNo.TabIndex = 3
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.SystemColors.Control
        Me.Label15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label15.Enabled = False
        Me.Label15.Location = New System.Drawing.Point(219, 1)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(199, 20)
        Me.Label15.TabIndex = 1
        Me.Label15.Text = "HallmarkNo"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gridHallmarkDetails
        '
        Me.gridHallmarkDetails.AllowUserToAddRows = False
        Me.gridHallmarkDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridHallmarkDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridHallmarkDetails.ColumnHeadersVisible = False
        Me.gridHallmarkDetails.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.gridHallmarkDetails.Location = New System.Drawing.Point(0, 49)
        Me.gridHallmarkDetails.Name = "gridHallmarkDetails"
        Me.gridHallmarkDetails.ReadOnly = True
        Me.gridHallmarkDetails.RowHeadersVisible = False
        Me.gridHallmarkDetails.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridHallmarkDetails.RowTemplate.Height = 20
        Me.gridHallmarkDetails.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridHallmarkDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridHallmarkDetails.Size = New System.Drawing.Size(418, 105)
        Me.gridHallmarkDetails.TabIndex = 5
        '
        'frmItemHallmarkDetalisUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(424, 212)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.grpHallmarkDetails)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnSave)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmItemHallmarkDetalisUpdate"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Item Tag Misc Charge"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.grpHallmarkDetails.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.gridHallmarkDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents grpHallmarkDetails As GroupBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents txthallmarkRowIndex As TextBox
    Friend WithEvents txtTagWt_WET As TextBox
    Friend WithEvents lblTagWt As Label
    Friend WithEvents txtHallmarkNo As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents gridHallmarkDetails As DataGridView
End Class
