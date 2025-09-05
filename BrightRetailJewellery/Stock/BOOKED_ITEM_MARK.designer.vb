<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BOOKED_ITEM_MARK
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
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cmbCounter_MAN = New System.Windows.Forms.ComboBox
        Me.lblCounterId = New System.Windows.Forms.Label
        Me.lblRunNoCustInfo = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtRunNo = New System.Windows.Forms.TextBox
        Me.gridViewFooter = New System.Windows.Forms.DataGridView
        Me.lblRowDet1 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnMark = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtItemId_NUM = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtTagNo = New System.Windows.Forms.TextBox
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.gridViewFooter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(6, 96)
        Me.gridView.Name = "gridView"
        Me.gridView.Size = New System.Drawing.Size(791, 243)
        Me.gridView.TabIndex = 13
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbCounter_MAN)
        Me.GroupBox1.Controls.Add(Me.lblCounterId)
        Me.GroupBox1.Controls.Add(Me.lblRunNoCustInfo)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtRunNo)
        Me.GroupBox1.Controls.Add(Me.gridView)
        Me.GroupBox1.Controls.Add(Me.gridViewFooter)
        Me.GroupBox1.Controls.Add(Me.lblRowDet1)
        Me.GroupBox1.Controls.Add(Me.btnExit)
        Me.GroupBox1.Controls.Add(Me.btnMark)
        Me.GroupBox1.Controls.Add(Me.btnNew)
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtItemId_NUM)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.txtTagNo)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(810, 408)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'cmbCounter_MAN
        '
        Me.cmbCounter_MAN.FormattingEnabled = True
        Me.cmbCounter_MAN.Location = New System.Drawing.Point(92, 69)
        Me.cmbCounter_MAN.Name = "cmbCounter_MAN"
        Me.cmbCounter_MAN.Size = New System.Drawing.Size(222, 21)
        Me.cmbCounter_MAN.TabIndex = 8
        '
        'lblCounterId
        '
        Me.lblCounterId.AutoSize = True
        Me.lblCounterId.Location = New System.Drawing.Point(9, 72)
        Me.lblCounterId.Name = "lblCounterId"
        Me.lblCounterId.Size = New System.Drawing.Size(84, 13)
        Me.lblCounterId.TabIndex = 7
        Me.lblCounterId.Text = "Item Counter"
        '
        'lblRunNoCustInfo
        '
        Me.lblRunNoCustInfo.AutoSize = True
        Me.lblRunNoCustInfo.BackColor = System.Drawing.Color.Aqua
        Me.lblRunNoCustInfo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRunNoCustInfo.ForeColor = System.Drawing.Color.Red
        Me.lblRunNoCustInfo.Location = New System.Drawing.Point(171, 18)
        Me.lblRunNoCustInfo.Name = "lblRunNoCustInfo"
        Me.lblRunNoCustInfo.Size = New System.Drawing.Size(70, 13)
        Me.lblRunNoCustInfo.TabIndex = 2
        Me.lblRunNoCustInfo.Text = "Cust Info."
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Run No"
        '
        'txtRunNo
        '
        Me.txtRunNo.Location = New System.Drawing.Point(92, 15)
        Me.txtRunNo.Name = "txtRunNo"
        Me.txtRunNo.ReadOnly = True
        Me.txtRunNo.Size = New System.Drawing.Size(73, 21)
        Me.txtRunNo.TabIndex = 1
        '
        'gridViewFooter
        '
        Me.gridViewFooter.AllowUserToAddRows = False
        Me.gridViewFooter.AllowUserToDeleteRows = False
        Me.gridViewFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewFooter.ColumnHeadersVisible = False
        Me.gridViewFooter.Enabled = False
        Me.gridViewFooter.Location = New System.Drawing.Point(6, 339)
        Me.gridViewFooter.Name = "gridViewFooter"
        Me.gridViewFooter.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridViewFooter.Size = New System.Drawing.Size(791, 39)
        Me.gridViewFooter.TabIndex = 14
        '
        'lblRowDet1
        '
        Me.lblRowDet1.AutoSize = True
        Me.lblRowDet1.BackColor = System.Drawing.Color.Aqua
        Me.lblRowDet1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRowDet1.ForeColor = System.Drawing.Color.Red
        Me.lblRowDet1.Location = New System.Drawing.Point(6, 381)
        Me.lblRowDet1.Name = "lblRowDet1"
        Me.lblRowDet1.Size = New System.Drawing.Size(70, 13)
        Me.lblRowDet1.TabIndex = 15
        Me.lblRowDet1.Text = "Cust Info."
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(660, 63)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(110, 30)
        Me.btnExit.TabIndex = 12
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnMark
        '
        Me.btnMark.Location = New System.Drawing.Point(544, 63)
        Me.btnMark.Name = "btnMark"
        Me.btnMark.Size = New System.Drawing.Size(110, 30)
        Me.btnMark.TabIndex = 11
        Me.btnMark.Text = "&Mark"
        Me.btnMark.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(438, 63)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 10
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(332, 63)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 9
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Item Id"
        '
        'txtItemId_NUM
        '
        Me.txtItemId_NUM.Location = New System.Drawing.Point(92, 42)
        Me.txtItemId_NUM.Name = "txtItemId_NUM"
        Me.txtItemId_NUM.Size = New System.Drawing.Size(73, 21)
        Me.txtItemId_NUM.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(171, 45)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "TagNo"
        '
        'txtTagNo
        '
        Me.txtTagNo.Location = New System.Drawing.Point(228, 42)
        Me.txtTagNo.Name = "txtTagNo"
        Me.txtTagNo.Size = New System.Drawing.Size(86, 21)
        Me.txtTagNo.TabIndex = 6
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'BOOKED_ITEM_MARK
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(834, 426)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "BOOKED_ITEM_MARK"
        Me.Text = "BOOKED_ITEM_MARK"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.gridViewFooter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents gridViewFooter As System.Windows.Forms.DataGridView
    Friend WithEvents lblRowDet1 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnMark As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtItemId_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtTagNo As System.Windows.Forms.TextBox
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtRunNo As System.Windows.Forms.TextBox
    Friend WithEvents lblRunNoCustInfo As System.Windows.Forms.Label
    Friend WithEvents lblCounterId As System.Windows.Forms.Label
    Friend WithEvents cmbCounter_MAN As System.Windows.Forms.ComboBox
End Class
