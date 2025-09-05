<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BillCollection
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
        Me.grpCollection = New CodeVendor.Controls.Grouper
        Me.Label10 = New System.Windows.Forms.Label
        Me.chkDiscount = New System.Windows.Forms.CheckBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.chkConvertAmount = New System.Windows.Forms.CheckBox
        Me.btnSave = New System.Windows.Forms.Button
        Me.txtCutRate_AMT = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtTotWeight_WET = New System.Windows.Forms.TextBox
        Me.txtBalAmount_AMT = New System.Windows.Forms.TextBox
        Me.txtDiscPer_PER = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtDiscWeight_WET = New System.Windows.Forms.TextBox
        Me.txtBalWeight_WET = New System.Windows.Forms.TextBox
        Me.txtDueWeight_WET = New System.Windows.Forms.TextBox
        Me.txtEmpId_MAN = New System.Windows.Forms.TextBox
        Me.txtCutAmount_AMT = New System.Windows.Forms.TextBox
        Me.txtEmpName = New System.Windows.Forms.TextBox
        Me.txtCutWeight_WET = New System.Windows.Forms.TextBox
        Me.txtRemark = New System.Windows.Forms.TextBox
        Me.txtDueAmount_AMT = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtDiscAmount_AMT = New System.Windows.Forms.TextBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlLeft = New System.Windows.Forms.Panel
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.txtAdjCreditCard_AMT = New System.Windows.Forms.TextBox
        Me.txtAdjCheque_AMT = New System.Windows.Forms.TextBox
        Me.Label47 = New System.Windows.Forms.Label
        Me.Label26 = New System.Windows.Forms.Label
        Me.txtAdjHandlingCharge_AMT = New System.Windows.Forms.TextBox
        Me.Label54 = New System.Windows.Forms.Label
        Me.txtAdjChitCard_AMT = New System.Windows.Forms.TextBox
        Me.Label45 = New System.Windows.Forms.Label
        Me.Label42 = New System.Windows.Forms.Label
        Me.Label57 = New System.Windows.Forms.Label
        Me.Label40 = New System.Windows.Forms.Label
        Me.Label38 = New System.Windows.Forms.Label
        Me.Label60 = New System.Windows.Forms.Label
        Me.Label28 = New System.Windows.Forms.Label
        Me.txtCash_AMT = New System.Windows.Forms.TextBox
        Me.grpAdj = New CodeVendor.Controls.Grouper
        Me.lblDiscount = New System.Windows.Forms.Label
        Me.Label59 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbCategory = New System.Windows.Forms.ComboBox
        Me.grpCollection.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.grpAdj.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpCollection
        '
        Me.grpCollection.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpCollection.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpCollection.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpCollection.BorderColor = System.Drawing.Color.Empty
        Me.grpCollection.BorderThickness = 1.0!
        Me.grpCollection.Controls.Add(Me.cmbCategory)
        Me.grpCollection.Controls.Add(Me.Label3)
        Me.grpCollection.Controls.Add(Me.Label10)
        Me.grpCollection.Controls.Add(Me.chkDiscount)
        Me.grpCollection.Controls.Add(Me.Label9)
        Me.grpCollection.Controls.Add(Me.chkConvertAmount)
        Me.grpCollection.Controls.Add(Me.btnSave)
        Me.grpCollection.Controls.Add(Me.txtCutRate_AMT)
        Me.grpCollection.Controls.Add(Me.Label11)
        Me.grpCollection.Controls.Add(Me.txtTotWeight_WET)
        Me.grpCollection.Controls.Add(Me.txtBalAmount_AMT)
        Me.grpCollection.Controls.Add(Me.txtDiscPer_PER)
        Me.grpCollection.Controls.Add(Me.Label12)
        Me.grpCollection.Controls.Add(Me.txtDiscWeight_WET)
        Me.grpCollection.Controls.Add(Me.txtBalWeight_WET)
        Me.grpCollection.Controls.Add(Me.txtDueWeight_WET)
        Me.grpCollection.Controls.Add(Me.txtEmpId_MAN)
        Me.grpCollection.Controls.Add(Me.txtCutAmount_AMT)
        Me.grpCollection.Controls.Add(Me.txtEmpName)
        Me.grpCollection.Controls.Add(Me.txtCutWeight_WET)
        Me.grpCollection.Controls.Add(Me.txtRemark)
        Me.grpCollection.Controls.Add(Me.txtDueAmount_AMT)
        Me.grpCollection.Controls.Add(Me.Label7)
        Me.grpCollection.Controls.Add(Me.Label6)
        Me.grpCollection.Controls.Add(Me.Label5)
        Me.grpCollection.Controls.Add(Me.Label4)
        Me.grpCollection.Controls.Add(Me.Label2)
        Me.grpCollection.Controls.Add(Me.Label13)
        Me.grpCollection.Controls.Add(Me.Label1)
        Me.grpCollection.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpCollection.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpCollection.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpCollection.GroupImage = Nothing
        Me.grpCollection.GroupTitle = ""
        Me.grpCollection.Location = New System.Drawing.Point(0, 0)
        Me.grpCollection.Name = "grpCollection"
        Me.grpCollection.Padding = New System.Windows.Forms.Padding(20)
        Me.grpCollection.PaintGroupBox = False
        Me.grpCollection.RoundCorners = 10
        Me.grpCollection.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpCollection.ShadowControl = False
        Me.grpCollection.ShadowThickness = 3
        Me.grpCollection.Size = New System.Drawing.Size(396, 270)
        Me.grpCollection.TabIndex = 0
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(4, 162)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(79, 14)
        Me.Label10.TabIndex = 19
        Me.Label10.Text = "Bal Weight"
        '
        'chkDiscount
        '
        Me.chkDiscount.AutoSize = True
        Me.chkDiscount.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDiscount.Checked = True
        Me.chkDiscount.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDiscount.Location = New System.Drawing.Point(198, 45)
        Me.chkDiscount.Name = "chkDiscount"
        Me.chkDiscount.Size = New System.Drawing.Size(77, 18)
        Me.chkDiscount.TabIndex = 4
        Me.chkDiscount.Text = "Disc Wt"
        Me.chkDiscount.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(199, 162)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(82, 14)
        Me.Label9.TabIndex = 21
        Me.Label9.Text = "Bal Amount"
        '
        'chkConvertAmount
        '
        Me.chkConvertAmount.AutoSize = True
        Me.chkConvertAmount.Location = New System.Drawing.Point(8, 68)
        Me.chkConvertAmount.Name = "chkConvertAmount"
        Me.chkConvertAmount.Size = New System.Drawing.Size(132, 18)
        Me.chkConvertAmount.TabIndex = 8
        Me.chkConvertAmount.Text = "Convert Amount"
        Me.chkConvertAmount.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(284, 232)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(105, 30)
        Me.btnSave.TabIndex = 28
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'txtCutRate_AMT
        '
        Me.txtCutRate_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCutRate_AMT.Location = New System.Drawing.Point(97, 112)
        Me.txtCutRate_AMT.Name = "txtCutRate_AMT"
        Me.txtCutRate_AMT.Size = New System.Drawing.Size(103, 22)
        Me.txtCutRate_AMT.TabIndex = 14
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(4, 185)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(71, 14)
        Me.Label11.TabIndex = 23
        Me.Label11.Text = "Employee"
        '
        'txtTotWeight_WET
        '
        Me.txtTotWeight_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotWeight_WET.Location = New System.Drawing.Point(286, 66)
        Me.txtTotWeight_WET.Name = "txtTotWeight_WET"
        Me.txtTotWeight_WET.Size = New System.Drawing.Size(103, 22)
        Me.txtTotWeight_WET.TabIndex = 10
        '
        'txtBalAmount_AMT
        '
        Me.txtBalAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBalAmount_AMT.Location = New System.Drawing.Point(286, 158)
        Me.txtBalAmount_AMT.Name = "txtBalAmount_AMT"
        Me.txtBalAmount_AMT.Size = New System.Drawing.Size(103, 22)
        Me.txtBalAmount_AMT.TabIndex = 22
        '
        'txtDiscPer_PER
        '
        Me.txtDiscPer_PER.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscPer_PER.Location = New System.Drawing.Point(287, 43)
        Me.txtDiscPer_PER.Name = "txtDiscPer_PER"
        Me.txtDiscPer_PER.Size = New System.Drawing.Size(24, 22)
        Me.txtDiscPer_PER.TabIndex = 5
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(4, 208)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(58, 14)
        Me.Label12.TabIndex = 26
        Me.Label12.Text = "Remark"
        '
        'txtDiscWeight_WET
        '
        Me.txtDiscWeight_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscWeight_WET.Location = New System.Drawing.Point(327, 43)
        Me.txtDiscWeight_WET.Name = "txtDiscWeight_WET"
        Me.txtDiscWeight_WET.Size = New System.Drawing.Size(62, 22)
        Me.txtDiscWeight_WET.TabIndex = 7
        '
        'txtBalWeight_WET
        '
        Me.txtBalWeight_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBalWeight_WET.Location = New System.Drawing.Point(97, 158)
        Me.txtBalWeight_WET.Name = "txtBalWeight_WET"
        Me.txtBalWeight_WET.Size = New System.Drawing.Size(102, 22)
        Me.txtBalWeight_WET.TabIndex = 20
        '
        'txtDueWeight_WET
        '
        Me.txtDueWeight_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDueWeight_WET.Location = New System.Drawing.Point(286, 20)
        Me.txtDueWeight_WET.Name = "txtDueWeight_WET"
        Me.txtDueWeight_WET.Size = New System.Drawing.Size(103, 22)
        Me.txtDueWeight_WET.TabIndex = 3
        '
        'txtEmpId_MAN
        '
        Me.txtEmpId_MAN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmpId_MAN.Location = New System.Drawing.Point(97, 181)
        Me.txtEmpId_MAN.Name = "txtEmpId_MAN"
        Me.txtEmpId_MAN.Size = New System.Drawing.Size(62, 22)
        Me.txtEmpId_MAN.TabIndex = 24
        '
        'txtCutAmount_AMT
        '
        Me.txtCutAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCutAmount_AMT.Location = New System.Drawing.Point(286, 135)
        Me.txtCutAmount_AMT.Name = "txtCutAmount_AMT"
        Me.txtCutAmount_AMT.Size = New System.Drawing.Size(103, 22)
        Me.txtCutAmount_AMT.TabIndex = 18
        '
        'txtEmpName
        '
        Me.txtEmpName.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmpName.Location = New System.Drawing.Point(158, 181)
        Me.txtEmpName.Name = "txtEmpName"
        Me.txtEmpName.Size = New System.Drawing.Size(231, 22)
        Me.txtEmpName.TabIndex = 25
        '
        'txtCutWeight_WET
        '
        Me.txtCutWeight_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCutWeight_WET.Location = New System.Drawing.Point(286, 112)
        Me.txtCutWeight_WET.Name = "txtCutWeight_WET"
        Me.txtCutWeight_WET.Size = New System.Drawing.Size(103, 22)
        Me.txtCutWeight_WET.TabIndex = 16
        '
        'txtRemark
        '
        Me.txtRemark.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRemark.Location = New System.Drawing.Point(97, 204)
        Me.txtRemark.Name = "txtRemark"
        Me.txtRemark.Size = New System.Drawing.Size(292, 22)
        Me.txtRemark.TabIndex = 27
        '
        'txtDueAmount_AMT
        '
        Me.txtDueAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDueAmount_AMT.Location = New System.Drawing.Point(97, 20)
        Me.txtDueAmount_AMT.Name = "txtDueAmount_AMT"
        Me.txtDueAmount_AMT.Size = New System.Drawing.Size(102, 22)
        Me.txtDueAmount_AMT.TabIndex = 1
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(199, 139)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(83, 14)
        Me.Label7.TabIndex = 17
        Me.Label7.Text = "Cut Amount"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(199, 116)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(80, 14)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "Cut Weight"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(4, 116)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 14)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "Cut Rate"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(199, 70)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 14)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Tot Weight"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(4, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 14)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Due Amount"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(309, 47)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(22, 14)
        Me.Label13.TabIndex = 6
        Me.Label13.Text = "%"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(198, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 14)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Due Weight"
        '
        'txtDiscAmount_AMT
        '
        Me.txtDiscAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiscAmount_AMT.Location = New System.Drawing.Point(132, 112)
        Me.txtDiscAmount_AMT.Name = "txtDiscAmount_AMT"
        Me.txtDiscAmount_AMT.Size = New System.Drawing.Size(108, 22)
        Me.txtDiscAmount_AMT.TabIndex = 14
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 26)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'pnlLeft
        '
        Me.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlLeft.Location = New System.Drawing.Point(0, 0)
        Me.pnlLeft.Name = "pnlLeft"
        Me.pnlLeft.Size = New System.Drawing.Size(10, 270)
        Me.pnlLeft.TabIndex = 17
        '
        'Panel1
        '
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel1.Location = New System.Drawing.Point(661, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(10, 270)
        Me.Panel1.TabIndex = 18
        '
        'Panel2
        '
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 270)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(671, 10)
        Me.Panel2.TabIndex = 19
        '
        'txtAdjCreditCard_AMT
        '
        Me.txtAdjCreditCard_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCreditCard_AMT.Location = New System.Drawing.Point(132, 66)
        Me.txtAdjCreditCard_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCreditCard_AMT.MaxLength = 12
        Me.txtAdjCreditCard_AMT.Name = "txtAdjCreditCard_AMT"
        Me.txtAdjCreditCard_AMT.Size = New System.Drawing.Size(108, 22)
        Me.txtAdjCreditCard_AMT.TabIndex = 8
        Me.txtAdjCreditCard_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjCheque_AMT
        '
        Me.txtAdjCheque_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCheque_AMT.Location = New System.Drawing.Point(132, 43)
        Me.txtAdjCheque_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCheque_AMT.MaxLength = 12
        Me.txtAdjCheque_AMT.Name = "txtAdjCheque_AMT"
        Me.txtAdjCheque_AMT.Size = New System.Drawing.Size(108, 22)
        Me.txtAdjCheque_AMT.TabIndex = 5
        Me.txtAdjCheque_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.BackColor = System.Drawing.Color.Transparent
        Me.Label47.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.Location = New System.Drawing.Point(4, 139)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(36, 14)
        Me.Label47.TabIndex = 15
        Me.Label47.Text = "[F4]"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.BackColor = System.Drawing.Color.Transparent
        Me.Label26.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(4, 93)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(36, 14)
        Me.Label26.TabIndex = 9
        Me.Label26.Text = "[F6]"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjHandlingCharge_AMT
        '
        Me.txtAdjHandlingCharge_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjHandlingCharge_AMT.Location = New System.Drawing.Point(132, 89)
        Me.txtAdjHandlingCharge_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjHandlingCharge_AMT.MaxLength = 12
        Me.txtAdjHandlingCharge_AMT.Name = "txtAdjHandlingCharge_AMT"
        Me.txtAdjHandlingCharge_AMT.Size = New System.Drawing.Size(108, 22)
        Me.txtAdjHandlingCharge_AMT.TabIndex = 11
        Me.txtAdjHandlingCharge_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.BackColor = System.Drawing.Color.Transparent
        Me.Label54.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label54.Location = New System.Drawing.Point(4, 70)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(36, 14)
        Me.Label54.TabIndex = 6
        Me.Label54.Text = "[F7]"
        Me.Label54.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjChitCard_AMT
        '
        Me.txtAdjChitCard_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjChitCard_AMT.Location = New System.Drawing.Point(132, 20)
        Me.txtAdjChitCard_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjChitCard_AMT.MaxLength = 12
        Me.txtAdjChitCard_AMT.Name = "txtAdjChitCard_AMT"
        Me.txtAdjChitCard_AMT.Size = New System.Drawing.Size(108, 22)
        Me.txtAdjChitCard_AMT.TabIndex = 2
        Me.txtAdjChitCard_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.BackColor = System.Drawing.Color.Transparent
        Me.Label45.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.Location = New System.Drawing.Point(39, 93)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(84, 14)
        Me.Label45.TabIndex = 10
        Me.Label45.Text = "Hand Charg"
        Me.Label45.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.BackColor = System.Drawing.Color.Transparent
        Me.Label42.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.Location = New System.Drawing.Point(39, 139)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(39, 14)
        Me.Label42.TabIndex = 16
        Me.Label42.Text = "Cash"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.BackColor = System.Drawing.Color.Transparent
        Me.Label57.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label57.Location = New System.Drawing.Point(4, 47)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(36, 14)
        Me.Label57.TabIndex = 3
        Me.Label57.Text = "[F8]"
        Me.Label57.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.BackColor = System.Drawing.Color.Transparent
        Me.Label40.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label40.Location = New System.Drawing.Point(39, 70)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(82, 14)
        Me.Label40.TabIndex = 7
        Me.Label40.Text = "Credit Card"
        Me.Label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.BackColor = System.Drawing.Color.Transparent
        Me.Label38.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.Location = New System.Drawing.Point(39, 47)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(56, 14)
        Me.Label38.TabIndex = 4
        Me.Label38.Text = "Cheque"
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label60
        '
        Me.Label60.AutoSize = True
        Me.Label60.BackColor = System.Drawing.Color.Transparent
        Me.Label60.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label60.Location = New System.Drawing.Point(4, 24)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(36, 14)
        Me.Label60.TabIndex = 0
        Me.Label60.Text = "[F9]"
        Me.Label60.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.BackColor = System.Drawing.Color.Transparent
        Me.Label28.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(39, 24)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(59, 14)
        Me.Label28.TabIndex = 1
        Me.Label28.Text = "Scheme"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCash_AMT
        '
        Me.txtCash_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCash_AMT.Location = New System.Drawing.Point(132, 135)
        Me.txtCash_AMT.Name = "txtCash_AMT"
        Me.txtCash_AMT.Size = New System.Drawing.Size(108, 22)
        Me.txtCash_AMT.TabIndex = 17
        '
        'grpAdj
        '
        Me.grpAdj.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpAdj.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpAdj.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpAdj.BorderColor = System.Drawing.Color.Transparent
        Me.grpAdj.BorderThickness = 1.0!
        Me.grpAdj.Controls.Add(Me.txtCash_AMT)
        Me.grpAdj.Controls.Add(Me.Label28)
        Me.grpAdj.Controls.Add(Me.lblDiscount)
        Me.grpAdj.Controls.Add(Me.Label60)
        Me.grpAdj.Controls.Add(Me.txtDiscAmount_AMT)
        Me.grpAdj.Controls.Add(Me.Label38)
        Me.grpAdj.Controls.Add(Me.Label59)
        Me.grpAdj.Controls.Add(Me.Label40)
        Me.grpAdj.Controls.Add(Me.Label57)
        Me.grpAdj.Controls.Add(Me.Label42)
        Me.grpAdj.Controls.Add(Me.Label45)
        Me.grpAdj.Controls.Add(Me.txtAdjChitCard_AMT)
        Me.grpAdj.Controls.Add(Me.Label54)
        Me.grpAdj.Controls.Add(Me.txtAdjHandlingCharge_AMT)
        Me.grpAdj.Controls.Add(Me.Label26)
        Me.grpAdj.Controls.Add(Me.Label47)
        Me.grpAdj.Controls.Add(Me.txtAdjCheque_AMT)
        Me.grpAdj.Controls.Add(Me.txtAdjCreditCard_AMT)
        Me.grpAdj.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpAdj.Dock = System.Windows.Forms.DockStyle.Right
        Me.grpAdj.GroupImage = Nothing
        Me.grpAdj.GroupTitle = ""
        Me.grpAdj.Location = New System.Drawing.Point(406, 0)
        Me.grpAdj.Name = "grpAdj"
        Me.grpAdj.Padding = New System.Windows.Forms.Padding(20)
        Me.grpAdj.PaintGroupBox = False
        Me.grpAdj.RoundCorners = 10
        Me.grpAdj.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpAdj.ShadowControl = False
        Me.grpAdj.ShadowThickness = 3
        Me.grpAdj.Size = New System.Drawing.Size(245, 270)
        Me.grpAdj.TabIndex = 0
        '
        'lblDiscount
        '
        Me.lblDiscount.AutoSize = True
        Me.lblDiscount.BackColor = System.Drawing.Color.Transparent
        Me.lblDiscount.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDiscount.Location = New System.Drawing.Point(39, 116)
        Me.lblDiscount.Name = "lblDiscount"
        Me.lblDiscount.Size = New System.Drawing.Size(64, 14)
        Me.lblDiscount.TabIndex = 13
        Me.lblDiscount.Text = "Discount"
        Me.lblDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label59
        '
        Me.Label59.AutoSize = True
        Me.Label59.BackColor = System.Drawing.Color.Transparent
        Me.Label59.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label59.Location = New System.Drawing.Point(4, 116)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(36, 14)
        Me.Label59.TabIndex = 12
        Me.Label59.Text = "[F5]"
        Me.Label59.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.grpCollection)
        Me.Panel3.Controls.Add(Me.Panel4)
        Me.Panel3.Controls.Add(Me.grpAdj)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(10, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(651, 270)
        Me.Panel3.TabIndex = 20
        '
        'Panel4
        '
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel4.Location = New System.Drawing.Point(396, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(10, 270)
        Me.Panel4.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 93)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 14)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Category"
        '
        'cmbCategory
        '
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Location = New System.Drawing.Point(97, 89)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(292, 22)
        Me.cmbCategory.TabIndex = 12
        '
        'BillCollection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(671, 280)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.pnlLeft)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "BillCollection"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Bill Collection"
        Me.grpCollection.ResumeLayout(False)
        Me.grpCollection.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.grpAdj.ResumeLayout(False)
        Me.grpAdj.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpCollection As CodeVendor.Controls.Grouper
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtDueAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtDueWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtDiscWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtDiscPer_PER As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtTotWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtCutWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtCutRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtBalAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtDiscAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtEmpId_MAN As System.Windows.Forms.TextBox
    Friend WithEvents txtBalWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtCutAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents txtRemark As System.Windows.Forms.TextBox
    Friend WithEvents txtEmpName As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents chkConvertAmount As System.Windows.Forms.CheckBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents chkDiscount As System.Windows.Forms.CheckBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlLeft As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents txtAdjCreditCard_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjCheque_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents txtAdjHandlingCharge_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents txtAdjChitCard_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents txtCash_AMT As System.Windows.Forms.TextBox
    Friend WithEvents grpAdj As CodeVendor.Controls.Grouper
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents lblDiscount As System.Windows.Forms.Label
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
End Class
