<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WRateCut
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbPartyName_MAN = New System.Windows.Forms.ComboBox()
        Me.pnlBalance = New System.Windows.Forms.Panel()
        Me.txtAmtBal = New System.Windows.Forms.TextBox()
        Me.txtWtBal = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.dtpBilldate = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.rbtAmt2Wt = New System.Windows.Forms.RadioButton()
        Me.rbtWt2Amt = New System.Windows.Forms.RadioButton()
        Me.lblConvertVal = New System.Windows.Forms.Label()
        Me.txtConvertVal = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtRate = New System.Windows.Forms.TextBox()
        Me.lblValue = New System.Windows.Forms.Label()
        Me.txtValue = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtRemark = New System.Windows.Forms.TextBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtTouch = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbtPayment = New System.Windows.Forms.RadioButton()
        Me.rbtReceipt = New System.Windows.Forms.RadioButton()
        Me.cmbCatName_MAN = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.chkcmbCostcentre = New BrighttechPack.CheckedComboBox()
        Me.pnlBalance.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(72, 110)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Party Name"
        '
        'cmbPartyName_MAN
        '
        Me.cmbPartyName_MAN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbPartyName_MAN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbPartyName_MAN.FormattingEnabled = True
        Me.cmbPartyName_MAN.Location = New System.Drawing.Point(152, 106)
        Me.cmbPartyName_MAN.Name = "cmbPartyName_MAN"
        Me.cmbPartyName_MAN.Size = New System.Drawing.Size(319, 21)
        Me.cmbPartyName_MAN.TabIndex = 6
        '
        'pnlBalance
        '
        Me.pnlBalance.Controls.Add(Me.txtAmtBal)
        Me.pnlBalance.Controls.Add(Me.txtWtBal)
        Me.pnlBalance.Controls.Add(Me.Label26)
        Me.pnlBalance.Controls.Add(Me.Label20)
        Me.pnlBalance.Location = New System.Drawing.Point(474, 83)
        Me.pnlBalance.Name = "pnlBalance"
        Me.pnlBalance.Size = New System.Drawing.Size(203, 46)
        Me.pnlBalance.TabIndex = 6
        '
        'txtAmtBal
        '
        Me.txtAmtBal.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.txtAmtBal.Location = New System.Drawing.Point(101, 23)
        Me.txtAmtBal.Name = "txtAmtBal"
        Me.txtAmtBal.ReadOnly = True
        Me.txtAmtBal.Size = New System.Drawing.Size(100, 21)
        Me.txtAmtBal.TabIndex = 3
        Me.txtAmtBal.Text = "99999.999 Dr"
        Me.txtAmtBal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtWtBal
        '
        Me.txtWtBal.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.txtWtBal.Location = New System.Drawing.Point(2, 23)
        Me.txtWtBal.Name = "txtWtBal"
        Me.txtWtBal.ReadOnly = True
        Me.txtWtBal.Size = New System.Drawing.Size(95, 21)
        Me.txtWtBal.TabIndex = 1
        Me.txtWtBal.Text = "99999.999 Dr"
        Me.txtWtBal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(13, 7)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(67, 13)
        Me.Label26.TabIndex = 0
        Me.Label26.Text = "Weight Bal"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(113, 7)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(73, 13)
        Me.Label20.TabIndex = 2
        Me.Label20.Text = "Amount Bal"
        '
        'dtpBilldate
        '
        Me.dtpBilldate.CustomFormat = "dd/MM/yyyy"
        Me.dtpBilldate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpBilldate.Location = New System.Drawing.Point(152, 78)
        Me.dtpBilldate.Name = "dtpBilldate"
        Me.dtpBilldate.Size = New System.Drawing.Size(104, 21)
        Me.dtpBilldate.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(72, 82)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Date"
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(258, 296)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 20
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'rbtAmt2Wt
        '
        Me.rbtAmt2Wt.AutoSize = True
        Me.rbtAmt2Wt.Location = New System.Drawing.Point(152, 134)
        Me.rbtAmt2Wt.Name = "rbtAmt2Wt"
        Me.rbtAmt2Wt.Size = New System.Drawing.Size(126, 17)
        Me.rbtAmt2Wt.TabIndex = 7
        Me.rbtAmt2Wt.TabStop = True
        Me.rbtAmt2Wt.Text = "Amount to Weight"
        Me.rbtAmt2Wt.UseVisualStyleBackColor = True
        '
        'rbtWt2Amt
        '
        Me.rbtWt2Amt.AutoSize = True
        Me.rbtWt2Amt.Location = New System.Drawing.Point(298, 134)
        Me.rbtWt2Amt.Name = "rbtWt2Amt"
        Me.rbtWt2Amt.Size = New System.Drawing.Size(126, 17)
        Me.rbtWt2Amt.TabIndex = 8
        Me.rbtWt2Amt.TabStop = True
        Me.rbtWt2Amt.Text = "Weight to Amount"
        Me.rbtWt2Amt.UseVisualStyleBackColor = True
        '
        'lblConvertVal
        '
        Me.lblConvertVal.AutoSize = True
        Me.lblConvertVal.Location = New System.Drawing.Point(72, 186)
        Me.lblConvertVal.Name = "lblConvertVal"
        Me.lblConvertVal.Size = New System.Drawing.Size(51, 13)
        Me.lblConvertVal.TabIndex = 11
        Me.lblConvertVal.Text = "Amount"
        '
        'txtConvertVal
        '
        Me.txtConvertVal.Location = New System.Drawing.Point(152, 182)
        Me.txtConvertVal.MaxLength = 15
        Me.txtConvertVal.Name = "txtConvertVal"
        Me.txtConvertVal.Size = New System.Drawing.Size(104, 21)
        Me.txtConvertVal.TabIndex = 12
        Me.txtConvertVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(72, 214)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(33, 13)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Rate"
        '
        'txtRate
        '
        Me.txtRate.Location = New System.Drawing.Point(152, 210)
        Me.txtRate.MaxLength = 15
        Me.txtRate.Name = "txtRate"
        Me.txtRate.Size = New System.Drawing.Size(104, 21)
        Me.txtRate.TabIndex = 14
        Me.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblValue
        '
        Me.lblValue.AutoSize = True
        Me.lblValue.Location = New System.Drawing.Point(72, 242)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.Size = New System.Drawing.Size(38, 13)
        Me.lblValue.TabIndex = 15
        Me.lblValue.Text = "Value"
        '
        'txtValue
        '
        Me.txtValue.Location = New System.Drawing.Point(152, 238)
        Me.txtValue.MaxLength = 15
        Me.txtValue.Name = "txtValue"
        Me.txtValue.Size = New System.Drawing.Size(104, 21)
        Me.txtValue.TabIndex = 16
        Me.txtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(72, 272)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(52, 13)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "Remark"
        '
        'txtRemark
        '
        Me.txtRemark.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtRemark.Location = New System.Drawing.Point(152, 268)
        Me.txtRemark.MaxLength = 50
        Me.txtRemark.Name = "txtRemark"
        Me.txtRemark.Size = New System.Drawing.Size(319, 21)
        Me.txtRemark.TabIndex = 18
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(152, 296)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 19
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(363, 296)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 21
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 70)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'txtTouch
        '
        Me.txtTouch.Location = New System.Drawing.Point(583, 318)
        Me.txtTouch.Name = "txtTouch"
        Me.txtTouch.Size = New System.Drawing.Size(104, 21)
        Me.txtTouch.TabIndex = 23
        Me.txtTouch.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(503, 321)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 13)
        Me.Label3.TabIndex = 22
        Me.Label3.Text = "Touch"
        Me.Label3.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtPayment)
        Me.GroupBox1.Controls.Add(Me.rbtReceipt)
        Me.GroupBox1.Location = New System.Drawing.Point(152, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(523, 34)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'rbtPayment
        '
        Me.rbtPayment.AutoSize = True
        Me.rbtPayment.Checked = True
        Me.rbtPayment.Location = New System.Drawing.Point(12, 14)
        Me.rbtPayment.Name = "rbtPayment"
        Me.rbtPayment.Size = New System.Drawing.Size(218, 17)
        Me.rbtPayment.TabIndex = 0
        Me.rbtPayment.TabStop = True
        Me.rbtPayment.Text = "Receipt(To be Adjust DR Balance)"
        Me.rbtPayment.UseVisualStyleBackColor = True
        '
        'rbtReceipt
        '
        Me.rbtReceipt.AutoSize = True
        Me.rbtReceipt.Location = New System.Drawing.Point(288, 14)
        Me.rbtReceipt.Name = "rbtReceipt"
        Me.rbtReceipt.Size = New System.Drawing.Size(223, 17)
        Me.rbtReceipt.TabIndex = 1
        Me.rbtReceipt.Text = "Payment(To be Settle CR Balance)"
        Me.rbtReceipt.UseVisualStyleBackColor = True
        '
        'cmbCatName_MAN
        '
        Me.cmbCatName_MAN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbCatName_MAN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbCatName_MAN.FormattingEnabled = True
        Me.cmbCatName_MAN.Location = New System.Drawing.Point(152, 157)
        Me.cmbCatName_MAN.Name = "cmbCatName_MAN"
        Me.cmbCatName_MAN.Size = New System.Drawing.Size(319, 21)
        Me.cmbCatName_MAN.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(72, 160)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Category"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(74, 56)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(72, 13)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "CostCentre"
        '
        'chkcmbCostcentre
        '
        Me.chkcmbCostcentre.CheckOnClick = True
        Me.chkcmbCostcentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbCostcentre.DropDownHeight = 1
        Me.chkcmbCostcentre.FormattingEnabled = True
        Me.chkcmbCostcentre.IntegralHeight = False
        Me.chkcmbCostcentre.Location = New System.Drawing.Point(152, 50)
        Me.chkcmbCostcentre.Name = "chkcmbCostcentre"
        Me.chkcmbCostcentre.Size = New System.Drawing.Size(229, 22)
        Me.chkcmbCostcentre.TabIndex = 2
        Me.chkcmbCostcentre.ValueSeparator = ", "
        '
        'WRateCut
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(720, 343)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.chkcmbCostcentre)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbCatName_MAN)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtValue)
        Me.Controls.Add(Me.txtRate)
        Me.Controls.Add(Me.txtTouch)
        Me.Controls.Add(Me.txtConvertVal)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtRemark)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.lblValue)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblConvertVal)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.rbtWt2Amt)
        Me.Controls.Add(Me.rbtAmt2Wt)
        Me.Controls.Add(Me.dtpBilldate)
        Me.Controls.Add(Me.pnlBalance)
        Me.Controls.Add(Me.cmbPartyName_MAN)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximumSize = New System.Drawing.Size(736, 451)
        Me.Name = "WRateCut"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Rate Cut"
        Me.pnlBalance.ResumeLayout(False)
        Me.pnlBalance.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbPartyName_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents pnlBalance As System.Windows.Forms.Panel
    Friend WithEvents txtAmtBal As System.Windows.Forms.TextBox
    Friend WithEvents txtWtBal As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents dtpBilldate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents rbtAmt2Wt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtWt2Amt As System.Windows.Forms.RadioButton
    Friend WithEvents lblConvertVal As System.Windows.Forms.Label
    Friend WithEvents txtConvertVal As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtRate As System.Windows.Forms.TextBox
    Friend WithEvents lblValue As System.Windows.Forms.Label
    Friend WithEvents txtValue As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtRemark As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtTouch As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtPayment As System.Windows.Forms.RadioButton
    Friend WithEvents rbtReceipt As System.Windows.Forms.RadioButton
    Friend WithEvents cmbCatName_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As Label
    Friend WithEvents chkcmbCostcentre As BrighttechPack.CheckedComboBox
End Class
