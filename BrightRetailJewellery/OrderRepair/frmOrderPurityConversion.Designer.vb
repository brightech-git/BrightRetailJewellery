<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmorderpurityconversion
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtConvRate_AMT = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtconvAmt_AMT = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.rbtAmount = New System.Windows.Forms.RadioButton
        Me.rbtPurity = New System.Windows.Forms.RadioButton
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtconvtwt_WET = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnSave = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.btnNew = New System.Windows.Forms.Button
        Me.cmbPurity_OWN = New System.Windows.Forms.ComboBox
        Me.txtadvancepurity_WET = New System.Windows.Forms.TextBox
        Me.txtorderNo = New System.Windows.Forms.TextBox
        Me.txtadvancewt_WET = New System.Windows.Forms.TextBox
        Me.txtcategory = New System.Windows.Forms.TextBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.exitToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.newToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.salesToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupBox1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtConvRate_AMT)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.txtconvAmt_AMT)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.rbtAmount)
        Me.GroupBox1.Controls.Add(Me.rbtPurity)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.txtconvtwt_WET)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.btnExit)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.btnSave)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.btnNew)
        Me.GroupBox1.Controls.Add(Me.cmbPurity_OWN)
        Me.GroupBox1.Controls.Add(Me.txtadvancepurity_WET)
        Me.GroupBox1.Controls.Add(Me.txtorderNo)
        Me.GroupBox1.Controls.Add(Me.txtadvancewt_WET)
        Me.GroupBox1.Controls.Add(Me.txtcategory)
        Me.GroupBox1.Location = New System.Drawing.Point(37, 26)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(468, 295)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'txtConvRate_AMT
        '
        Me.txtConvRate_AMT.Location = New System.Drawing.Point(152, 204)
        Me.txtConvRate_AMT.MaxLength = 20
        Me.txtConvRate_AMT.Name = "txtConvRate_AMT"
        Me.txtConvRate_AMT.Size = New System.Drawing.Size(75, 20)
        Me.txtConvRate_AMT.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(239, 209)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(86, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Convert  Amount"
        '
        'txtconvAmt_AMT
        '
        Me.txtconvAmt_AMT.Location = New System.Drawing.Point(352, 204)
        Me.txtconvAmt_AMT.MaxLength = 20
        Me.txtconvAmt_AMT.Name = "txtconvAmt_AMT"
        Me.txtconvAmt_AMT.Size = New System.Drawing.Size(82, 20)
        Me.txtconvAmt_AMT.TabIndex = 17
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(47, 209)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(70, 13)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Convert Rate"
        '
        'rbtAmount
        '
        Me.rbtAmount.AutoSize = True
        Me.rbtAmount.Location = New System.Drawing.Point(318, 140)
        Me.rbtAmount.Name = "rbtAmount"
        Me.rbtAmount.Size = New System.Drawing.Size(117, 17)
        Me.rbtAmount.TabIndex = 9
        Me.rbtAmount.Text = "Amount Conversion"
        Me.rbtAmount.UseVisualStyleBackColor = True
        '
        'rbtPurity
        '
        Me.rbtPurity.AutoSize = True
        Me.rbtPurity.Checked = True
        Me.rbtPurity.Location = New System.Drawing.Point(152, 140)
        Me.rbtPurity.Name = "rbtPurity"
        Me.rbtPurity.Size = New System.Drawing.Size(107, 17)
        Me.rbtPurity.TabIndex = 8
        Me.rbtPurity.TabStop = True
        Me.rbtPurity.Text = "Purity Conversion"
        Me.rbtPurity.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(239, 175)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(84, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Convert  Weight"
        '
        'txtconvtwt_WET
        '
        Me.txtconvtwt_WET.Location = New System.Drawing.Point(352, 170)
        Me.txtconvtwt_WET.MaxLength = 20
        Me.txtconvtwt_WET.Name = "txtconvtwt_WET"
        Me.txtconvtwt_WET.Size = New System.Drawing.Size(82, 20)
        Me.txtconvtwt_WET.TabIndex = 13
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(47, 175)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Convert  Purity"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(70, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Order No"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(285, 259)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 20
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(71, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Category"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(236, 110)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(87, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Advance Weight"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(67, 259)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 18
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(41, 110)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Advance Purity"
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(174, 259)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 19
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'cmbPurity_OWN
        '
        Me.cmbPurity_OWN.FormattingEnabled = True
        Me.cmbPurity_OWN.Location = New System.Drawing.Point(152, 170)
        Me.cmbPurity_OWN.Name = "cmbPurity_OWN"
        Me.cmbPurity_OWN.Size = New System.Drawing.Size(75, 21)
        Me.cmbPurity_OWN.TabIndex = 11
        '
        'txtadvancepurity_WET
        '
        Me.txtadvancepurity_WET.Location = New System.Drawing.Point(152, 102)
        Me.txtadvancepurity_WET.MaxLength = 20
        Me.txtadvancepurity_WET.Name = "txtadvancepurity_WET"
        Me.txtadvancepurity_WET.Size = New System.Drawing.Size(75, 20)
        Me.txtadvancepurity_WET.TabIndex = 5
        '
        'txtorderNo
        '
        Me.txtorderNo.Location = New System.Drawing.Point(152, 42)
        Me.txtorderNo.Name = "txtorderNo"
        Me.txtorderNo.Size = New System.Drawing.Size(91, 20)
        Me.txtorderNo.TabIndex = 1
        '
        'txtadvancewt_WET
        '
        Me.txtadvancewt_WET.Location = New System.Drawing.Point(352, 103)
        Me.txtadvancewt_WET.MaxLength = 20
        Me.txtadvancewt_WET.Name = "txtadvancewt_WET"
        Me.txtadvancewt_WET.Size = New System.Drawing.Size(83, 20)
        Me.txtadvancewt_WET.TabIndex = 7
        '
        'txtcategory
        '
        Me.txtcategory.Enabled = False
        Me.txtcategory.Location = New System.Drawing.Point(152, 70)
        Me.txtcategory.MaxLength = 50
        Me.txtcategory.Name = "txtcategory"
        Me.txtcategory.Size = New System.Drawing.Size(283, 20)
        Me.txtcategory.TabIndex = 3
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.exitToolStripMenuItem1, Me.newToolStripMenuItem2, Me.salesToolStripMenuItem3})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(225, 70)
        '
        'exitToolStripMenuItem1
        '
        Me.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1"
        Me.exitToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.exitToolStripMenuItem1.Size = New System.Drawing.Size(224, 22)
        Me.exitToolStripMenuItem1.Text = "exitToolStripMenuItem1"
        '
        'newToolStripMenuItem2
        '
        Me.newToolStripMenuItem2.Name = "newToolStripMenuItem2"
        Me.newToolStripMenuItem2.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.newToolStripMenuItem2.Size = New System.Drawing.Size(224, 22)
        Me.newToolStripMenuItem2.Text = "newToolStripMenuItem2"
        '
        'salesToolStripMenuItem3
        '
        Me.salesToolStripMenuItem3.Name = "salesToolStripMenuItem3"
        Me.salesToolStripMenuItem3.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.salesToolStripMenuItem3.Size = New System.Drawing.Size(224, 22)
        Me.salesToolStripMenuItem3.Text = "ToolStripMenuItem3"
        '
        'frmorderpurityconversion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(542, 346)
        Me.Controls.Add(Me.GroupBox1)
        Me.KeyPreview = True
        Me.Name = "frmorderpurityconversion"
        Me.Text = "Order Adv. Weight Purity Conversion"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtconvtwt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents cmbPurity_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents txtadvancepurity_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtorderNo As System.Windows.Forms.TextBox
    Friend WithEvents txtadvancewt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtcategory As System.Windows.Forms.TextBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents exitToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents newToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents salesToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rbtPurity As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAmount As System.Windows.Forms.RadioButton
    Friend WithEvents txtConvRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtconvAmt_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
End Class
