<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_USRATEUPDATE_BULK
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
        Me.txtNewUSRate_AMT = New System.Windows.Forms.TextBox
        Me.txtNewRupees_AMT = New System.Windows.Forms.TextBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.grpContainer = New System.Windows.Forms.GroupBox
        Me.GridView1 = New System.Windows.Forms.DataGridView
        Me.chkOldRup = New System.Windows.Forms.CheckBox
        Me.ChkOldUsRate = New System.Windows.Forms.CheckBox
        Me.txtOldUSRate_AMT = New System.Windows.Forms.TextBox
        Me.txtOldRupees_AMT = New System.Windows.Forms.TextBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.RdEffectTagStn = New System.Windows.Forms.RadioButton
        Me.RdEffectTag = New System.Windows.Forms.RadioButton
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtItemName = New System.Windows.Forms.TextBox
        Me.txtTagNo = New System.Windows.Forms.TextBox
        Me.txtItemCode_NUM = New System.Windows.Forms.TextBox
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.tabView = New System.Windows.Forms.TabPage
        Me.pnlContainer_OWN = New System.Windows.Forms.Panel
        Me.GridView_OWN = New System.Windows.Forms.DataGridView
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ChkAll_OWN = New System.Windows.Forms.CheckBox
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpContainer.SuspendLayout()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.pnlContainer_OWN.SuspendLayout()
        CType(Me.GridView_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtNewUSRate_AMT
        '
        Me.txtNewUSRate_AMT.Location = New System.Drawing.Point(332, 87)
        Me.txtNewUSRate_AMT.Name = "txtNewUSRate_AMT"
        Me.txtNewUSRate_AMT.Size = New System.Drawing.Size(102, 21)
        Me.txtNewUSRate_AMT.TabIndex = 8
        '
        'txtNewRupees_AMT
        '
        Me.txtNewRupees_AMT.Location = New System.Drawing.Point(332, 118)
        Me.txtNewRupees_AMT.Name = "txtNewRupees_AMT"
        Me.txtNewRupees_AMT.Size = New System.Drawing.Size(102, 21)
        Me.txtNewRupees_AMT.TabIndex = 12
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(117, 204)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 14
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(324, 204)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 16
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.GridView1)
        Me.grpContainer.Controls.Add(Me.chkOldRup)
        Me.grpContainer.Controls.Add(Me.ChkOldUsRate)
        Me.grpContainer.Controls.Add(Me.txtOldUSRate_AMT)
        Me.grpContainer.Controls.Add(Me.txtOldRupees_AMT)
        Me.grpContainer.Controls.Add(Me.GroupBox1)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Controls.Add(Me.Label3)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.Label5)
        Me.grpContainer.Controls.Add(Me.txtItemName)
        Me.grpContainer.Controls.Add(Me.txtTagNo)
        Me.grpContainer.Controls.Add(Me.txtItemCode_NUM)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.txtNewUSRate_AMT)
        Me.grpContainer.Controls.Add(Me.txtNewRupees_AMT)
        Me.grpContainer.Location = New System.Drawing.Point(271, 66)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(497, 507)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'GridView1
        '
        Me.GridView1.AllowUserToAddRows = False
        Me.GridView1.AllowUserToDeleteRows = False
        Me.GridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GridView1.Location = New System.Drawing.Point(3, 249)
        Me.GridView1.Name = "GridView1"
        Me.GridView1.ReadOnly = True
        Me.GridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GridView1.Size = New System.Drawing.Size(491, 255)
        Me.GridView1.TabIndex = 17
        '
        'chkOldRup
        '
        Me.chkOldRup.AutoSize = True
        Me.chkOldRup.Checked = True
        Me.chkOldRup.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOldRup.Location = New System.Drawing.Point(12, 120)
        Me.chkOldRup.Name = "chkOldRup"
        Me.chkOldRup.Size = New System.Drawing.Size(105, 17)
        Me.chkOldRup.TabIndex = 9
        Me.chkOldRup.Text = "OldIndRupess"
        Me.chkOldRup.UseVisualStyleBackColor = True
        '
        'ChkOldUsRate
        '
        Me.ChkOldUsRate.AutoSize = True
        Me.ChkOldUsRate.Checked = True
        Me.ChkOldUsRate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkOldUsRate.Location = New System.Drawing.Point(12, 89)
        Me.ChkOldUsRate.Name = "ChkOldUsRate"
        Me.ChkOldUsRate.Size = New System.Drawing.Size(93, 17)
        Me.ChkOldUsRate.TabIndex = 5
        Me.ChkOldUsRate.Text = "Old Us Rate" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.ChkOldUsRate.UseVisualStyleBackColor = True
        '
        'txtOldUSRate_AMT
        '
        Me.txtOldUSRate_AMT.Location = New System.Drawing.Point(117, 87)
        Me.txtOldUSRate_AMT.Name = "txtOldUSRate_AMT"
        Me.txtOldUSRate_AMT.Size = New System.Drawing.Size(102, 21)
        Me.txtOldUSRate_AMT.TabIndex = 6
        '
        'txtOldRupees_AMT
        '
        Me.txtOldRupees_AMT.Location = New System.Drawing.Point(117, 118)
        Me.txtOldRupees_AMT.Name = "txtOldRupees_AMT"
        Me.txtOldRupees_AMT.Size = New System.Drawing.Size(102, 21)
        Me.txtOldRupees_AMT.TabIndex = 10
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RdEffectTagStn)
        Me.GroupBox1.Controls.Add(Me.RdEffectTag)
        Me.GroupBox1.Location = New System.Drawing.Point(117, 148)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(201, 38)
        Me.GroupBox1.TabIndex = 13
        Me.GroupBox1.TabStop = False
        '
        'RdEffectTagStn
        '
        Me.RdEffectTagStn.AutoSize = True
        Me.RdEffectTagStn.Location = New System.Drawing.Point(103, 15)
        Me.RdEffectTagStn.Name = "RdEffectTagStn"
        Me.RdEffectTagStn.Size = New System.Drawing.Size(90, 17)
        Me.RdEffectTagStn.TabIndex = 1
        Me.RdEffectTagStn.TabStop = True
        Me.RdEffectTagStn.Text = "EffectStone"
        Me.RdEffectTagStn.UseVisualStyleBackColor = True
        '
        'RdEffectTag
        '
        Me.RdEffectTag.AutoSize = True
        Me.RdEffectTag.Checked = True
        Me.RdEffectTag.Location = New System.Drawing.Point(6, 15)
        Me.RdEffectTag.Name = "RdEffectTag"
        Me.RdEffectTag.Size = New System.Drawing.Size(78, 17)
        Me.RdEffectTag.TabIndex = 0
        Me.RdEffectTag.TabStop = True
        Me.RdEffectTag.Text = "EffectTag"
        Me.RdEffectTag.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(220, 204)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 15
        Me.btnNew.Text = "&New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(236, 121)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 13)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "NewInd Rupess"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(236, 93)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "NewUS Rate"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Item Code"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(9, 59)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(43, 13)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "TagNo"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtItemName
        '
        Me.txtItemName.BackColor = System.Drawing.SystemColors.Window
        Me.txtItemName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemName.Enabled = False
        Me.txtItemName.Location = New System.Drawing.Point(228, 23)
        Me.txtItemName.Name = "txtItemName"
        Me.txtItemName.Size = New System.Drawing.Size(206, 21)
        Me.txtItemName.TabIndex = 2
        '
        'txtTagNo
        '
        Me.txtTagNo.BackColor = System.Drawing.SystemColors.Window
        Me.txtTagNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTagNo.Location = New System.Drawing.Point(117, 56)
        Me.txtTagNo.MaxLength = 10
        Me.txtTagNo.Name = "txtTagNo"
        Me.txtTagNo.Size = New System.Drawing.Size(100, 21)
        Me.txtTagNo.TabIndex = 4
        '
        'txtItemCode_NUM
        '
        Me.txtItemCode_NUM.BackColor = System.Drawing.SystemColors.Window
        Me.txtItemCode_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemCode_NUM.Location = New System.Drawing.Point(117, 23)
        Me.txtItemCode_NUM.MaxLength = 8
        Me.txtItemCode_NUM.Name = "txtItemCode_NUM"
        Me.txtItemCode_NUM.Size = New System.Drawing.Size(100, 21)
        Me.txtItemCode_NUM.TabIndex = 1
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1022, 640)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grpContainer)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1014, 614)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlContainer_OWN)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1014, 614)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'pnlContainer_OWN
        '
        Me.pnlContainer_OWN.Controls.Add(Me.GridView_OWN)
        Me.pnlContainer_OWN.Controls.Add(Me.Panel1)
        Me.pnlContainer_OWN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContainer_OWN.Location = New System.Drawing.Point(3, 3)
        Me.pnlContainer_OWN.Name = "pnlContainer_OWN"
        Me.pnlContainer_OWN.Size = New System.Drawing.Size(1008, 608)
        Me.pnlContainer_OWN.TabIndex = 3
        '
        'GridView_OWN
        '
        Me.GridView_OWN.AllowUserToAddRows = False
        Me.GridView_OWN.AllowUserToDeleteRows = False
        Me.GridView_OWN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView_OWN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridView_OWN.Location = New System.Drawing.Point(0, 39)
        Me.GridView_OWN.Name = "GridView_OWN"
        Me.GridView_OWN.Size = New System.Drawing.Size(1008, 569)
        Me.GridView_OWN.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ChkAll_OWN)
        Me.Panel1.Controls.Add(Me.btnUpdate)
        Me.Panel1.Controls.Add(Me.btnBack)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1008, 39)
        Me.Panel1.TabIndex = 0
        '
        'ChkAll_OWN
        '
        Me.ChkAll_OWN.AutoSize = True
        Me.ChkAll_OWN.Location = New System.Drawing.Point(6, 11)
        Me.ChkAll_OWN.Name = "ChkAll_OWN"
        Me.ChkAll_OWN.Size = New System.Drawing.Size(75, 17)
        Me.ChkAll_OWN.TabIndex = 0
        Me.ChkAll_OWN.Text = "SelectAll"
        Me.ChkAll_OWN.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(205, 5)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(100, 30)
        Me.btnUpdate.TabIndex = 2
        Me.btnUpdate.Text = "&Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(99, 5)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 1
        Me.btnBack.Text = "&Back"
        Me.btnBack.UseVisualStyleBackColor = True
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
        'FRM_USRATEUPDATE_BULK
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FRM_USRATEUPDATE_BULK"
        Me.ShowIcon = False
        Me.Text = "Update US Rate"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.pnlContainer_OWN.ResumeLayout(False)
        CType(Me.GridView_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtNewUSRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtNewRupees_AMT As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtItemName As System.Windows.Forms.TextBox
    Friend WithEvents txtTagNo As System.Windows.Forms.TextBox
    Friend WithEvents txtItemCode_NUM As System.Windows.Forms.TextBox
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents RdEffectTagStn As System.Windows.Forms.RadioButton
    Friend WithEvents RdEffectTag As System.Windows.Forms.RadioButton
    Friend WithEvents pnlContainer_OWN As System.Windows.Forms.Panel
    Friend WithEvents GridView_OWN As System.Windows.Forms.DataGridView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents ChkAll_OWN As System.Windows.Forms.CheckBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkOldRup As System.Windows.Forms.CheckBox
    Friend WithEvents ChkOldUsRate As System.Windows.Forms.CheckBox
    Friend WithEvents txtOldUSRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtOldRupees_AMT As System.Windows.Forms.TextBox
    Friend WithEvents GridView1 As System.Windows.Forms.DataGridView
End Class
