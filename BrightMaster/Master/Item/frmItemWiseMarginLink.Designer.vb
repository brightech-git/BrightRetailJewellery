<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmItemWiseMarginLink
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
        Me.pnlSubItem = New System.Windows.Forms.Panel
        Me.chkListMargin = New System.Windows.Forms.CheckedListBox
        Me.pnlItem = New System.Windows.Forms.Panel
        Me.chkListItem = New System.Windows.Forms.CheckedListBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.chkListMetal = New System.Windows.Forms.CheckedListBox
        Me.bnSave = New System.Windows.Forms.Button
        Me.bnNew = New System.Windows.Forms.Button
        Me.bnExit = New System.Windows.Forms.Button
        Me.gridShow = New System.Windows.Forms.DataGridView
        Me.chkMetalAll = New System.Windows.Forms.CheckBox
        Me.chkItemAll = New System.Windows.Forms.CheckBox
        Me.chkMarginAll = New System.Windows.Forms.CheckBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.chkListCostCentre = New System.Windows.Forms.CheckedListBox
        Me.chkCostCentreAll = New System.Windows.Forms.CheckBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlSubItem.SuspendLayout()
        Me.pnlItem.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.gridShow, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlSubItem
        '
        Me.pnlSubItem.Controls.Add(Me.chkListMargin)
        Me.pnlSubItem.Location = New System.Drawing.Point(587, 27)
        Me.pnlSubItem.Name = "pnlSubItem"
        Me.pnlSubItem.Size = New System.Drawing.Size(190, 403)
        Me.pnlSubItem.TabIndex = 7
        '
        'chkListMargin
        '
        Me.chkListMargin.CheckOnClick = True
        Me.chkListMargin.FormattingEnabled = True
        Me.chkListMargin.HorizontalScrollbar = True
        Me.chkListMargin.Location = New System.Drawing.Point(4, 3)
        Me.chkListMargin.Name = "chkListMargin"
        Me.chkListMargin.ScrollAlwaysVisible = True
        Me.chkListMargin.Size = New System.Drawing.Size(185, 394)
        Me.chkListMargin.TabIndex = 0
        '
        'pnlItem
        '
        Me.pnlItem.Controls.Add(Me.chkListItem)
        Me.pnlItem.Location = New System.Drawing.Point(392, 26)
        Me.pnlItem.Name = "pnlItem"
        Me.pnlItem.Size = New System.Drawing.Size(191, 404)
        Me.pnlItem.TabIndex = 5
        '
        'chkListItem
        '
        Me.chkListItem.CheckOnClick = True
        Me.chkListItem.FormattingEnabled = True
        Me.chkListItem.HorizontalScrollbar = True
        Me.chkListItem.Location = New System.Drawing.Point(3, 4)
        Me.chkListItem.Name = "chkListItem"
        Me.chkListItem.ScrollAlwaysVisible = True
        Me.chkListItem.Size = New System.Drawing.Size(185, 394)
        Me.chkListItem.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkListMetal)
        Me.Panel1.Location = New System.Drawing.Point(1, 26)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(191, 404)
        Me.Panel1.TabIndex = 1
        '
        'chkListMetal
        '
        Me.chkListMetal.CheckOnClick = True
        Me.chkListMetal.FormattingEnabled = True
        Me.chkListMetal.HorizontalScrollbar = True
        Me.chkListMetal.Location = New System.Drawing.Point(3, 4)
        Me.chkListMetal.Name = "chkListMetal"
        Me.chkListMetal.ScrollAlwaysVisible = True
        Me.chkListMetal.Size = New System.Drawing.Size(185, 394)
        Me.chkListMetal.TabIndex = 0
        '
        'bnSave
        '
        Me.bnSave.Location = New System.Drawing.Point(271, 455)
        Me.bnSave.Name = "bnSave"
        Me.bnSave.Size = New System.Drawing.Size(100, 35)
        Me.bnSave.TabIndex = 8
        Me.bnSave.Text = "Save [F1]"
        Me.bnSave.UseVisualStyleBackColor = True
        '
        'bnNew
        '
        Me.bnNew.Location = New System.Drawing.Point(377, 455)
        Me.bnNew.Name = "bnNew"
        Me.bnNew.Size = New System.Drawing.Size(100, 35)
        Me.bnNew.TabIndex = 9
        Me.bnNew.Text = "New [F3]"
        Me.bnNew.UseVisualStyleBackColor = True
        '
        'bnExit
        '
        Me.bnExit.Location = New System.Drawing.Point(483, 455)
        Me.bnExit.Name = "bnExit"
        Me.bnExit.Size = New System.Drawing.Size(100, 35)
        Me.bnExit.TabIndex = 10
        Me.bnExit.Text = "Exit [F12]"
        Me.bnExit.UseVisualStyleBackColor = True
        '
        'gridShow
        '
        Me.gridShow.AllowUserToAddRows = False
        Me.gridShow.AllowUserToDeleteRows = False
        Me.gridShow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridShow.Location = New System.Drawing.Point(4, 436)
        Me.gridShow.Name = "gridShow"
        Me.gridShow.ReadOnly = True
        Me.gridShow.RowHeadersVisible = False
        Me.gridShow.RowTemplate.Height = 20
        Me.gridShow.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridShow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridShow.Size = New System.Drawing.Size(165, 58)
        Me.gridShow.TabIndex = 11
        Me.gridShow.Visible = False
        '
        'chkMetalAll
        '
        Me.chkMetalAll.AutoSize = True
        Me.chkMetalAll.Location = New System.Drawing.Point(7, 7)
        Me.chkMetalAll.Name = "chkMetalAll"
        Me.chkMetalAll.Size = New System.Drawing.Size(52, 17)
        Me.chkMetalAll.TabIndex = 0
        Me.chkMetalAll.Text = "Metal"
        Me.chkMetalAll.UseVisualStyleBackColor = True
        '
        'chkItemAll
        '
        Me.chkItemAll.AutoSize = True
        Me.chkItemAll.Location = New System.Drawing.Point(397, 7)
        Me.chkItemAll.Name = "chkItemAll"
        Me.chkItemAll.Size = New System.Drawing.Size(46, 17)
        Me.chkItemAll.TabIndex = 4
        Me.chkItemAll.Text = "Item"
        Me.chkItemAll.UseVisualStyleBackColor = True
        '
        'chkMarginAll
        '
        Me.chkMarginAll.AutoSize = True
        Me.chkMarginAll.Location = New System.Drawing.Point(594, 7)
        Me.chkMarginAll.Name = "chkMarginAll"
        Me.chkMarginAll.Size = New System.Drawing.Size(58, 17)
        Me.chkMarginAll.TabIndex = 6
        Me.chkMarginAll.Text = "Margin"
        Me.chkMarginAll.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.chkListCostCentre)
        Me.Panel2.Location = New System.Drawing.Point(197, 27)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(191, 404)
        Me.Panel2.TabIndex = 3
        '
        'chkListCostCentre
        '
        Me.chkListCostCentre.CheckOnClick = True
        Me.chkListCostCentre.FormattingEnabled = True
        Me.chkListCostCentre.HorizontalScrollbar = True
        Me.chkListCostCentre.Location = New System.Drawing.Point(3, 4)
        Me.chkListCostCentre.Name = "chkListCostCentre"
        Me.chkListCostCentre.ScrollAlwaysVisible = True
        Me.chkListCostCentre.Size = New System.Drawing.Size(185, 394)
        Me.chkListCostCentre.TabIndex = 0
        '
        'chkCostCentreAll
        '
        Me.chkCostCentreAll.AutoSize = True
        Me.chkCostCentreAll.Location = New System.Drawing.Point(203, 7)
        Me.chkCostCentreAll.Name = "chkCostCentreAll"
        Me.chkCostCentreAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCostCentreAll.TabIndex = 2
        Me.chkCostCentreAll.Text = "Cost Centre"
        Me.chkCostCentreAll.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(131, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'frmItemWiseMarginLink
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(783, 497)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.chkCostCentreAll)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.chkMarginAll)
        Me.Controls.Add(Me.chkMetalAll)
        Me.Controls.Add(Me.chkItemAll)
        Me.Controls.Add(Me.gridShow)
        Me.Controls.Add(Me.bnExit)
        Me.Controls.Add(Me.bnNew)
        Me.Controls.Add(Me.bnSave)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlSubItem)
        Me.Controls.Add(Me.pnlItem)
        Me.KeyPreview = True
        Me.Name = "frmItemWiseMarginLink"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Item Wise Margin Link"
        Me.pnlSubItem.ResumeLayout(False)
        Me.pnlItem.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        CType(Me.gridShow, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlSubItem As System.Windows.Forms.Panel
    Friend WithEvents chkListMargin As System.Windows.Forms.CheckedListBox
    Friend WithEvents pnlItem As System.Windows.Forms.Panel
    Friend WithEvents chkListItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkListMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents bnSave As System.Windows.Forms.Button
    Friend WithEvents bnNew As System.Windows.Forms.Button
    Friend WithEvents bnExit As System.Windows.Forms.Button
    Friend WithEvents gridShow As System.Windows.Forms.DataGridView
    Friend WithEvents chkMetalAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkItemAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkMarginAll As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents chkListCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCostCentreAll As System.Windows.Forms.CheckBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
