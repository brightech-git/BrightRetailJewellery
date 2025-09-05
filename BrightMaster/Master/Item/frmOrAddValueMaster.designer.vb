<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOrAddValueMaster
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
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.txtValueName__Man = New System.Windows.Forms.TextBox()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.txtDisplayOrder_Num = New System.Windows.Forms.TextBox()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.txtValueId_Num_Man = New System.Windows.Forms.TextBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblActive = New System.Windows.Forms.Label()
        Me.cmbActive = New System.Windows.Forms.ComboBox()
        Me.cmbTypename = New System.Windows.Forms.ComboBox()
        Me.lblValue = New System.Windows.Forms.Label()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(442, 119)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 12
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'txtValueName__Man
        '
        Me.txtValueName__Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtValueName__Man.Location = New System.Drawing.Point(118, 63)
        Me.txtValueName__Man.MaxLength = 15
        Me.txtValueName__Man.Name = "txtValueName__Man"
        Me.txtValueName__Man.Size = New System.Drawing.Size(211, 21)
        Me.txtValueName__Man.TabIndex = 3
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(15, 157)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(628, 315)
        Me.gridView.TabIndex = 13
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(15, 119)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(122, 119)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 9
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'txtDisplayOrder_Num
        '
        Me.txtDisplayOrder_Num.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDisplayOrder_Num.Location = New System.Drawing.Point(118, 90)
        Me.txtDisplayOrder_Num.MaxLength = 7
        Me.txtDisplayOrder_Num.Name = "txtDisplayOrder_Num"
        Me.txtDisplayOrder_Num.Size = New System.Drawing.Size(59, 21)
        Me.txtDisplayOrder_Num.TabIndex = 5
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(229, 119)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 10
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'txtValueId_Num_Man
        '
        Me.txtValueId_Num_Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtValueId_Num_Man.Location = New System.Drawing.Point(118, 36)
        Me.txtValueId_Num_Man.MaxLength = 7
        Me.txtValueId_Num_Man.Name = "txtValueId_Num_Man"
        Me.txtValueId_Num_Man.Size = New System.Drawing.Size(97, 21)
        Me.txtValueId_Num_Man.TabIndex = 1
        '
        'btnExit
        '
        Me.btnExit.CausesValidation = False
        Me.btnExit.Location = New System.Drawing.Point(336, 119)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 11
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "ValueId"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Type Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 94)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(86, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Display Order"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(12, 476)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 17
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'lblActive
        '
        Me.lblActive.AutoSize = True
        Me.lblActive.Location = New System.Drawing.Point(183, 94)
        Me.lblActive.Name = "lblActive"
        Me.lblActive.Size = New System.Drawing.Size(42, 13)
        Me.lblActive.TabIndex = 6
        Me.lblActive.Text = "Active"
        Me.lblActive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbActive
        '
        Me.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActive.FormattingEnabled = True
        Me.cmbActive.Location = New System.Drawing.Point(233, 90)
        Me.cmbActive.Name = "cmbActive"
        Me.cmbActive.Size = New System.Drawing.Size(96, 21)
        Me.cmbActive.TabIndex = 7
        '
        'cmbTypename
        '
        Me.cmbTypename.FormattingEnabled = True
        Me.cmbTypename.Location = New System.Drawing.Point(118, 9)
        Me.cmbTypename.Name = "cmbTypename"
        Me.cmbTypename.Size = New System.Drawing.Size(211, 21)
        Me.cmbTypename.TabIndex = 18
        '
        'lblValue
        '
        Me.lblValue.AutoSize = True
        Me.lblValue.Location = New System.Drawing.Point(13, 70)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.Size = New System.Drawing.Size(75, 13)
        Me.lblValue.TabIndex = 19
        Me.lblValue.Text = "Value Name"
        '
        'frmOrAddValueMaster
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(653, 498)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.lblValue)
        Me.Controls.Add(Me.cmbTypename)
        Me.Controls.Add(Me.cmbActive)
        Me.Controls.Add(Me.lblActive)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.txtValueName__Man)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.txtDisplayOrder_Num)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.txtValueId_Num_Man)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmOrAddValueMaster"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Addtional Details"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtValueName__Man As System.Windows.Forms.TextBox
    Friend WithEvents txtDisplayOrder_Num As System.Windows.Forms.TextBox
    Friend WithEvents txtValueId_Num_Man As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents lblActive As Label
    Friend WithEvents cmbActive As ComboBox
    Friend WithEvents cmbTypename As ComboBox
    Friend WithEvents lblValue As Label
End Class
