<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTagtoNontag
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
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtTagNo = New System.Windows.Forms.TextBox()
        Me.chkcmbitemname = New BrighttechPack.CheckedComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnTransfer = New System.Windows.Forms.Button()
        Me.CounterName = New System.Windows.Forms.Label()
        Me.cmbcountername_OWN = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbnonsubitem_OWN = New System.Windows.Forms.ComboBox()
        Me.cmbNontag_OWN = New System.Windows.Forms.ComboBox()
        Me.lblnontag = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.transferToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.exitToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.newToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.focusToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label6 = New System.Windows.Forms.Label()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(3, 17)
        Me.gridView.Name = "gridView"
        Me.gridView.Size = New System.Drawing.Size(653, 278)
        Me.gridView.TabIndex = 0
        '
        'chkSelectAll
        '
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Checked = True
        Me.chkSelectAll.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSelectAll.Location = New System.Drawing.Point(577, 86)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkSelectAll.TabIndex = 4
        Me.chkSelectAll.Text = "Select All"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(136, 66)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(117, 30)
        Me.btnSearch.TabIndex = 5
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(60, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Tag No"
        '
        'txtTagNo
        '
        Me.txtTagNo.Location = New System.Drawing.Point(138, 42)
        Me.txtTagNo.MaxLength = 1500
        Me.txtTagNo.Name = "txtTagNo"
        Me.txtTagNo.Size = New System.Drawing.Size(585, 21)
        Me.txtTagNo.TabIndex = 3
        '
        'chkcmbitemname
        '
        Me.chkcmbitemname.CheckOnClick = True
        Me.chkcmbitemname.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbitemname.DropDownHeight = 1
        Me.chkcmbitemname.FormattingEnabled = True
        Me.chkcmbitemname.IntegralHeight = False
        Me.chkcmbitemname.Location = New System.Drawing.Point(136, 12)
        Me.chkcmbitemname.Name = "chkcmbitemname"
        Me.chkcmbitemname.Size = New System.Drawing.Size(255, 22)
        Me.chkcmbitemname.TabIndex = 1
        Me.chkcmbitemname.ValueSeparator = ", "
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(60, 11)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 21)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Item Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1043, 103)
        Me.Panel1.TabIndex = 24
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.chkcmbitemname)
        Me.GroupBox1.Controls.Add(Me.txtTagNo)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.btnSearch)
        Me.GroupBox1.Controls.Add(Me.chkSelectAll)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1043, 103)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filteration"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.GroupBox4)
        Me.Panel2.Controls.Add(Me.GroupBox2)
        Me.Panel2.Location = New System.Drawing.Point(0, 109)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1043, 298)
        Me.Panel2.TabIndex = 25
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Controls.Add(Me.btnNew)
        Me.GroupBox4.Controls.Add(Me.btnExit)
        Me.GroupBox4.Controls.Add(Me.btnTransfer)
        Me.GroupBox4.Controls.Add(Me.CounterName)
        Me.GroupBox4.Controls.Add(Me.cmbcountername_OWN)
        Me.GroupBox4.Controls.Add(Me.Label1)
        Me.GroupBox4.Controls.Add(Me.cmbnonsubitem_OWN)
        Me.GroupBox4.Controls.Add(Me.cmbNontag_OWN)
        Me.GroupBox4.Controls.Add(Me.lblnontag)
        Me.GroupBox4.Location = New System.Drawing.Point(677, 19)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(363, 273)
        Me.GroupBox4.TabIndex = 11
        Me.GroupBox4.TabStop = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(-2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(97, 13)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Transfer Details"
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(119, 237)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(117, 30)
        Me.btnNew.TabIndex = 14
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(236, 237)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(117, 30)
        Me.btnExit.TabIndex = 15
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnTransfer
        '
        Me.btnTransfer.Location = New System.Drawing.Point(1, 237)
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.Size = New System.Drawing.Size(117, 30)
        Me.btnTransfer.TabIndex = 13
        Me.btnTransfer.Text = "&Transfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'CounterName
        '
        Me.CounterName.AutoSize = True
        Me.CounterName.Location = New System.Drawing.Point(9, 194)
        Me.CounterName.Name = "CounterName"
        Me.CounterName.Size = New System.Drawing.Size(70, 13)
        Me.CounterName.TabIndex = 11
        Me.CounterName.Text = "To Counter"
        '
        'cmbcountername_OWN
        '
        Me.cmbcountername_OWN.FormattingEnabled = True
        Me.cmbcountername_OWN.Location = New System.Drawing.Point(119, 186)
        Me.cmbcountername_OWN.Name = "cmbcountername_OWN"
        Me.cmbcountername_OWN.Size = New System.Drawing.Size(226, 21)
        Me.cmbcountername_OWN.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 149)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "To  SubItem"
        '
        'cmbnonsubitem_OWN
        '
        Me.cmbnonsubitem_OWN.FormattingEnabled = True
        Me.cmbnonsubitem_OWN.Location = New System.Drawing.Point(119, 149)
        Me.cmbnonsubitem_OWN.Name = "cmbnonsubitem_OWN"
        Me.cmbnonsubitem_OWN.Size = New System.Drawing.Size(226, 21)
        Me.cmbnonsubitem_OWN.TabIndex = 10
        '
        'cmbNontag_OWN
        '
        Me.cmbNontag_OWN.FormattingEnabled = True
        Me.cmbNontag_OWN.Location = New System.Drawing.Point(119, 112)
        Me.cmbNontag_OWN.Name = "cmbNontag_OWN"
        Me.cmbNontag_OWN.Size = New System.Drawing.Size(226, 21)
        Me.cmbNontag_OWN.TabIndex = 8
        '
        'lblnontag
        '
        Me.lblnontag.AutoSize = True
        Me.lblnontag.Location = New System.Drawing.Point(9, 112)
        Me.lblnontag.Name = "lblnontag"
        Me.lblnontag.Size = New System.Drawing.Size(51, 13)
        Me.lblnontag.TabIndex = 7
        Me.lblnontag.Text = "To Item"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.gridView)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(659, 298)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = " "
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 407)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1043, 65)
        Me.Panel3.TabIndex = 26
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(14, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(212, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Press Escape for select NonTagItem"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.transferToolStripMenuItem1, Me.exitToolStripMenuItem2, Me.newToolStripMenuItem3, Me.focusToolStripMenuItem1})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(242, 92)
        '
        'transferToolStripMenuItem1
        '
        Me.transferToolStripMenuItem1.Name = "transferToolStripMenuItem1"
        Me.transferToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.transferToolStripMenuItem1.Size = New System.Drawing.Size(241, 22)
        Me.transferToolStripMenuItem1.Text = "transferToolStripMenuItem1"
        '
        'exitToolStripMenuItem2
        '
        Me.exitToolStripMenuItem2.Name = "exitToolStripMenuItem2"
        Me.exitToolStripMenuItem2.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.exitToolStripMenuItem2.Size = New System.Drawing.Size(241, 22)
        Me.exitToolStripMenuItem2.Text = "exitToolStripMenuItem2"
        '
        'newToolStripMenuItem3
        '
        Me.newToolStripMenuItem3.Name = "newToolStripMenuItem3"
        Me.newToolStripMenuItem3.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.newToolStripMenuItem3.Size = New System.Drawing.Size(241, 22)
        Me.newToolStripMenuItem3.Text = "newToolStripMenuItem3"
        '
        'focusToolStripMenuItem1
        '
        Me.focusToolStripMenuItem1.Name = "focusToolStripMenuItem1"
        Me.focusToolStripMenuItem1.Size = New System.Drawing.Size(241, 22)
        Me.focusToolStripMenuItem1.Text = "focusToolStripMenuItem1"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Coral
        Me.Label6.Location = New System.Drawing.Point(721, 66)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(310, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "*Hint Search Tagno Bulk E.g : ABC,DEF,GHI,JKL,MNO"
        '
        'frmTagtoNontag
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1043, 472)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmTagtoNontag"
        Me.Text = "Tag To Nontag Transfer"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtTagNo As System.Windows.Forms.TextBox
    Friend WithEvents chkcmbitemname As BrighttechPack.CheckedComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents transferToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents exitToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents newToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents focusToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents CounterName As System.Windows.Forms.Label
    Friend WithEvents cmbcountername_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbnonsubitem_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbNontag_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents lblnontag As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnTransfer As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As Label
End Class
