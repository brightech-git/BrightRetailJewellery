<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSyncStatusChecking
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
        Me.tabmain = New System.Windows.Forms.TabControl
        Me.GeneralTabPage = New System.Windows.Forms.TabPage
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.GridaccView = New System.Windows.Forms.DataGridView
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.dtpChkdate = New BrighttechPack.DatePicker(Me.components)
        Me.clrtable = New System.Windows.Forms.Button
        Me.btnSend = New System.Windows.Forms.Button
        Me.btntriggercreate = New System.Windows.Forms.Button
        Me.chkonlydiff = New System.Windows.Forms.CheckBox
        Me.btnupdate = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbCostname = New System.Windows.Forms.ComboBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnView = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.DetailTabPage = New System.Windows.Forms.TabPage
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.Gridcorp = New System.Windows.Forms.DataGridView
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.lblcorp = New System.Windows.Forms.Label
        Me.GridBranch = New System.Windows.Forms.DataGridView
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.lblbranch = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.btnback = New System.Windows.Forms.Button
        Me.btnfinupdate = New System.Windows.Forms.Button
        Me.dtpChkdate1 = New BrighttechPack.DatePicker(Me.components)
        Me.Label3 = New System.Windows.Forms.Label
        Me.tabmain.SuspendLayout()
        Me.GeneralTabPage.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.GridaccView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.DetailTabPage.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.Gridcorp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel5.SuspendLayout()
        CType(Me.GridBranch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel6.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabmain
        '
        Me.tabmain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabmain.Controls.Add(Me.GeneralTabPage)
        Me.tabmain.Controls.Add(Me.DetailTabPage)
        Me.tabmain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabmain.Location = New System.Drawing.Point(0, 0)
        Me.tabmain.Name = "tabmain"
        Me.tabmain.SelectedIndex = 0
        Me.tabmain.Size = New System.Drawing.Size(1028, 539)
        Me.tabmain.TabIndex = 0
        '
        'GeneralTabPage
        '
        Me.GeneralTabPage.Controls.Add(Me.Panel4)
        Me.GeneralTabPage.Controls.Add(Me.GroupBox3)
        Me.GeneralTabPage.Location = New System.Drawing.Point(4, 25)
        Me.GeneralTabPage.Name = "GeneralTabPage"
        Me.GeneralTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.GeneralTabPage.Size = New System.Drawing.Size(1020, 510)
        Me.GeneralTabPage.TabIndex = 0
        Me.GeneralTabPage.Text = "GeneralTabPage"
        Me.GeneralTabPage.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.Panel1)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(3, 77)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1014, 430)
        Me.Panel4.TabIndex = 6
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Panel1.Controls.Add(Me.GroupBox2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1014, 430)
        Me.Panel1.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox2.Controls.Add(Me.GridaccView)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1014, 430)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        '
        'GridaccView
        '
        Me.GridaccView.AllowUserToAddRows = False
        Me.GridaccView.AllowUserToDeleteRows = False
        Me.GridaccView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridaccView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridaccView.Location = New System.Drawing.Point(3, 16)
        Me.GridaccView.Name = "GridaccView"
        Me.GridaccView.ReadOnly = True
        Me.GridaccView.RowHeadersVisible = False
        Me.GridaccView.Size = New System.Drawing.Size(1008, 411)
        Me.GridaccView.TabIndex = 0
        '
        'GroupBox3
        '
        Me.GroupBox3.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.dtpChkdate1)
        Me.GroupBox3.Controls.Add(Me.dtpChkdate)
        Me.GroupBox3.Controls.Add(Me.clrtable)
        Me.GroupBox3.Controls.Add(Me.btnSend)
        Me.GroupBox3.Controls.Add(Me.btntriggercreate)
        Me.GroupBox3.Controls.Add(Me.chkonlydiff)
        Me.GroupBox3.Controls.Add(Me.btnupdate)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.cmbCostname)
        Me.GroupBox3.Controls.Add(Me.btnExit)
        Me.GroupBox3.Controls.Add(Me.btnView)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox3.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(1014, 74)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        '
        'dtpChkdate
        '
        Me.dtpChkdate.Location = New System.Drawing.Point(82, 13)
        Me.dtpChkdate.Mask = "##/##/####"
        Me.dtpChkdate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpChkdate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpChkdate.Name = "dtpChkdate"
        Me.dtpChkdate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpChkdate.Size = New System.Drawing.Size(93, 21)
        Me.dtpChkdate.TabIndex = 1
        Me.dtpChkdate.Text = "07/03/9998"
        Me.dtpChkdate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'clrtable
        '
        Me.clrtable.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clrtable.Location = New System.Drawing.Point(706, 30)
        Me.clrtable.Name = "clrtable"
        Me.clrtable.Size = New System.Drawing.Size(100, 30)
        Me.clrtable.TabIndex = 11
        Me.clrtable.Text = "Delete"
        Me.clrtable.UseVisualStyleBackColor = True
        '
        'btnSend
        '
        Me.btnSend.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSend.Location = New System.Drawing.Point(406, 30)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(100, 30)
        Me.btnSend.TabIndex = 8
        Me.btnSend.Text = "Send"
        Me.btnSend.UseVisualStyleBackColor = True
        '
        'btntriggercreate
        '
        Me.btntriggercreate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btntriggercreate.Location = New System.Drawing.Point(606, 30)
        Me.btntriggercreate.Name = "btntriggercreate"
        Me.btntriggercreate.Size = New System.Drawing.Size(100, 30)
        Me.btntriggercreate.TabIndex = 10
        Me.btntriggercreate.Text = "Generate"
        Me.btntriggercreate.UseVisualStyleBackColor = True
        '
        'chkonlydiff
        '
        Me.chkonlydiff.AutoSize = True
        Me.chkonlydiff.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkonlydiff.Location = New System.Drawing.Point(309, 14)
        Me.chkonlydiff.Name = "chkonlydiff"
        Me.chkonlydiff.Size = New System.Drawing.Size(115, 17)
        Me.chkonlydiff.TabIndex = 6
        Me.chkonlydiff.Text = "Only Difference"
        Me.chkonlydiff.UseVisualStyleBackColor = True
        '
        'btnupdate
        '
        Me.btnupdate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnupdate.Location = New System.Drawing.Point(506, 30)
        Me.btnupdate.Name = "btnupdate"
        Me.btnupdate.Size = New System.Drawing.Size(100, 30)
        Me.btnupdate.TabIndex = 9
        Me.btnupdate.Text = "Receive"
        Me.btnupdate.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(7, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "CostName"
        '
        'cmbCostname
        '
        Me.cmbCostname.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCostname.FormattingEnabled = True
        Me.cmbCostname.Location = New System.Drawing.Point(82, 38)
        Me.cmbCostname.Name = "cmbCostname"
        Me.cmbCostname.Size = New System.Drawing.Size(211, 21)
        Me.cmbCostname.TabIndex = 5
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(806, 30)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 12
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnView
        '
        Me.btnView.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView.Location = New System.Drawing.Point(306, 30)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(100, 30)
        Me.btnView.TabIndex = 7
        Me.btnView.Text = "View"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(7, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Check Date"
        '
        'DetailTabPage
        '
        Me.DetailTabPage.Controls.Add(Me.Panel3)
        Me.DetailTabPage.Controls.Add(Me.Panel2)
        Me.DetailTabPage.Location = New System.Drawing.Point(4, 25)
        Me.DetailTabPage.Name = "DetailTabPage"
        Me.DetailTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.DetailTabPage.Size = New System.Drawing.Size(1020, 510)
        Me.DetailTabPage.TabIndex = 1
        Me.DetailTabPage.Text = "DetailTabPage"
        Me.DetailTabPage.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.SplitContainer1)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(3, 3)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1014, 450)
        Me.Panel3.TabIndex = 1
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.SplitContainer1.Panel1.Controls.Add(Me.Gridcorp)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel5)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.SplitContainer1.Panel2.Controls.Add(Me.GridBranch)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel6)
        Me.SplitContainer1.Size = New System.Drawing.Size(1014, 450)
        Me.SplitContainer1.SplitterDistance = 505
        Me.SplitContainer1.SplitterWidth = 5
        Me.SplitContainer1.TabIndex = 0
        '
        'Gridcorp
        '
        Me.Gridcorp.AllowUserToAddRows = False
        Me.Gridcorp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Gridcorp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Gridcorp.Location = New System.Drawing.Point(0, 20)
        Me.Gridcorp.Name = "Gridcorp"
        Me.Gridcorp.RowHeadersVisible = False
        Me.Gridcorp.Size = New System.Drawing.Size(505, 430)
        Me.Gridcorp.TabIndex = 2
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.SeaGreen
        Me.Panel5.Controls.Add(Me.lblcorp)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(505, 20)
        Me.Panel5.TabIndex = 0
        '
        'lblcorp
        '
        Me.lblcorp.AutoSize = True
        Me.lblcorp.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcorp.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblcorp.Location = New System.Drawing.Point(188, 3)
        Me.lblcorp.Name = "lblcorp"
        Me.lblcorp.Size = New System.Drawing.Size(55, 16)
        Me.lblcorp.TabIndex = 0
        Me.lblcorp.Text = "Label3"
        Me.lblcorp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GridBranch
        '
        Me.GridBranch.AllowUserToAddRows = False
        Me.GridBranch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridBranch.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridBranch.Location = New System.Drawing.Point(0, 20)
        Me.GridBranch.Name = "GridBranch"
        Me.GridBranch.RowHeadersVisible = False
        Me.GridBranch.Size = New System.Drawing.Size(504, 430)
        Me.GridBranch.TabIndex = 2
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.PaleVioletRed
        Me.Panel6.Controls.Add(Me.lblbranch)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel6.Location = New System.Drawing.Point(0, 0)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(504, 20)
        Me.Panel6.TabIndex = 1
        '
        'lblbranch
        '
        Me.lblbranch.AutoSize = True
        Me.lblbranch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblbranch.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblbranch.Location = New System.Drawing.Point(244, 1)
        Me.lblbranch.Name = "lblbranch"
        Me.lblbranch.Size = New System.Drawing.Size(55, 16)
        Me.lblbranch.TabIndex = 1
        Me.lblbranch.Text = "Label4"
        Me.lblbranch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Panel2.Controls.Add(Me.btnback)
        Me.Panel2.Controls.Add(Me.btnfinupdate)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(3, 453)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1014, 54)
        Me.Panel2.TabIndex = 0
        '
        'btnback
        '
        Me.btnback.Location = New System.Drawing.Point(862, 9)
        Me.btnback.Name = "btnback"
        Me.btnback.Size = New System.Drawing.Size(100, 30)
        Me.btnback.TabIndex = 1
        Me.btnback.Text = "Back"
        Me.btnback.UseVisualStyleBackColor = True
        '
        'btnfinupdate
        '
        Me.btnfinupdate.Location = New System.Drawing.Point(759, 9)
        Me.btnfinupdate.Name = "btnfinupdate"
        Me.btnfinupdate.Size = New System.Drawing.Size(100, 30)
        Me.btnfinupdate.TabIndex = 0
        Me.btnfinupdate.Text = "Update"
        Me.btnfinupdate.UseVisualStyleBackColor = True
        '
        'dtpChkdate1
        '
        Me.dtpChkdate1.Location = New System.Drawing.Point(200, 13)
        Me.dtpChkdate1.Mask = "##/##/####"
        Me.dtpChkdate1.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpChkdate1.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpChkdate1.Name = "dtpChkdate1"
        Me.dtpChkdate1.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpChkdate1.Size = New System.Drawing.Size(93, 21)
        Me.dtpChkdate1.TabIndex = 3
        Me.dtpChkdate1.Text = "07/03/9998"
        Me.dtpChkdate1.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(179, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(21, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "To"
        '
        'frmSyncStatusChecking
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 539)
        Me.Controls.Add(Me.tabmain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmSyncStatusChecking"
        Me.Text = "Corporate Vs Branches"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabmain.ResumeLayout(False)
        Me.GeneralTabPage.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.GridaccView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.DetailTabPage.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.Gridcorp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        CType(Me.GridBranch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabmain As System.Windows.Forms.TabControl
    Friend WithEvents GeneralTabPage As System.Windows.Forms.TabPage
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GridaccView As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents chkonlydiff As System.Windows.Forms.CheckBox
    Friend WithEvents btnupdate As System.Windows.Forms.Button
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbCostname As System.Windows.Forms.ComboBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DetailTabPage As System.Windows.Forms.TabPage
    Friend WithEvents btntriggercreate As System.Windows.Forms.Button
    Friend WithEvents clrtable As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents lblcorp As System.Windows.Forms.Label
    Friend WithEvents lblbranch As System.Windows.Forms.Label
    Friend WithEvents Gridcorp As System.Windows.Forms.DataGridView
    Friend WithEvents GridBranch As System.Windows.Forms.DataGridView
    Friend WithEvents btnfinupdate As System.Windows.Forms.Button
    Friend WithEvents btnback As System.Windows.Forms.Button
    Friend WithEvents dtpChkdate As BrighttechPack.DatePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtpChkdate1 As BrighttechPack.DatePicker

End Class
