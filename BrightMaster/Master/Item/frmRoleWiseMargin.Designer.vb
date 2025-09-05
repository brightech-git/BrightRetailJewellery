<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRoleWiseMargin
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
        Me.bnExit = New System.Windows.Forms.Button
        Me.bnNew = New System.Windows.Forms.Button
        Me.bnSave = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.chkListMetal = New System.Windows.Forms.CheckedListBox
        Me.pnlSubItem = New System.Windows.Forms.Panel
        Me.chkListMargin = New System.Windows.Forms.CheckedListBox
        Me.pnlItem = New System.Windows.Forms.Panel
        Me.chkListRole = New System.Windows.Forms.CheckedListBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.FindToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel1.SuspendLayout()
        Me.pnlSubItem.SuspendLayout()
        Me.pnlItem.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnExit
        '
        Me.bnExit.Location = New System.Drawing.Point(487, 454)
        Me.bnExit.Name = "bnExit"
        Me.bnExit.Size = New System.Drawing.Size(100, 35)
        Me.bnExit.TabIndex = 11
        Me.bnExit.Text = "Exit [F12]"
        Me.bnExit.UseVisualStyleBackColor = True
        '
        'bnNew
        '
        Me.bnNew.Location = New System.Drawing.Point(381, 454)
        Me.bnNew.Name = "bnNew"
        Me.bnNew.Size = New System.Drawing.Size(100, 35)
        Me.bnNew.TabIndex = 10
        Me.bnNew.Text = "New [F3]"
        Me.bnNew.UseVisualStyleBackColor = True
        '
        'bnSave
        '
        Me.bnSave.Location = New System.Drawing.Point(275, 454)
        Me.bnSave.Name = "bnSave"
        Me.bnSave.Size = New System.Drawing.Size(100, 35)
        Me.bnSave.TabIndex = 9
        Me.bnSave.Text = "Save [F1]"
        Me.bnSave.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkListMetal)
        Me.Panel1.Location = New System.Drawing.Point(5, 8)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(191, 404)
        Me.Panel1.TabIndex = 6
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
        'pnlSubItem
        '
        Me.pnlSubItem.Controls.Add(Me.chkListMargin)
        Me.pnlSubItem.Location = New System.Drawing.Point(399, 9)
        Me.pnlSubItem.Name = "pnlSubItem"
        Me.pnlSubItem.Size = New System.Drawing.Size(190, 403)
        Me.pnlSubItem.TabIndex = 8
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
        Me.pnlItem.Controls.Add(Me.chkListRole)
        Me.pnlItem.Location = New System.Drawing.Point(202, 8)
        Me.pnlItem.Name = "pnlItem"
        Me.pnlItem.Size = New System.Drawing.Size(191, 404)
        Me.pnlItem.TabIndex = 7
        '
        'chkListRole
        '
        Me.chkListRole.CheckOnClick = True
        Me.chkListRole.FormattingEnabled = True
        Me.chkListRole.HorizontalScrollbar = True
        Me.chkListRole.Location = New System.Drawing.Point(3, 4)
        Me.chkListRole.Name = "chkListRole"
        Me.chkListRole.ScrollAlwaysVisible = True
        Me.chkListRole.Size = New System.Drawing.Size(185, 394)
        Me.chkListRole.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FindToolStripMenuItem, Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 70)
        '
        'FindToolStripMenuItem
        '
        Me.FindToolStripMenuItem.Name = "FindToolStripMenuItem"
        Me.FindToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.FindToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.FindToolStripMenuItem.Text = "Save"
        Me.FindToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'frmRoleWiseMargin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(594, 497)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.bnExit)
        Me.Controls.Add(Me.bnNew)
        Me.Controls.Add(Me.bnSave)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlSubItem)
        Me.Controls.Add(Me.pnlItem)
        Me.KeyPreview = True
        Me.Name = "frmRoleWiseMargin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Role Margin"
        Me.Panel1.ResumeLayout(False)
        Me.pnlSubItem.ResumeLayout(False)
        Me.pnlItem.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bnExit As System.Windows.Forms.Button
    Friend WithEvents bnNew As System.Windows.Forms.Button
    Friend WithEvents bnSave As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkListMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents pnlSubItem As System.Windows.Forms.Panel
    Friend WithEvents chkListMargin As System.Windows.Forms.CheckedListBox
    Friend WithEvents pnlItem As System.Windows.Forms.Panel
    Friend WithEvents chkListRole As System.Windows.Forms.CheckedListBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents FindToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
