<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OrderStatusUpdate
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
        Me.rbtIssued = New System.Windows.Forms.RadioButton
        Me.rbtReceived = New System.Windows.Forms.RadioButton
        Me.rbtDelivered = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.grpIssuedDet = New System.Windows.Forms.GroupBox
        Me.dtpBillDate = New BrighttechPack.DatePicker(Me.components)
        Me.Label13 = New System.Windows.Forms.Label
        Me.txtIssTranNo_NUM = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.cmbIssDesigner_MAN = New System.Windows.Forms.ComboBox
        Me.txtIssNetWt_WET = New System.Windows.Forms.TextBox
        Me.txtIssGrsWt_WET = New System.Windows.Forms.TextBox
        Me.txtIssPcs_NUM = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.grpReceived = New System.Windows.Forms.GroupBox
        Me.cmbRecDesigner_MAN = New System.Windows.Forms.ComboBox
        Me.txtRecMc_AMT = New System.Windows.Forms.TextBox
        Me.txtRecNetWt_WET = New System.Windows.Forms.TextBox
        Me.txtRecWastage_WET = New System.Windows.Forms.TextBox
        Me.txtRecGrsWt_WET = New System.Windows.Forms.TextBox
        Me.txtRecPcs_NUM = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.dtpRecBillDate = New BrighttechPack.DatePicker(Me.components)
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtRecTranNo_NUM = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.grpIssuedDet.SuspendLayout()
        Me.grpReceived.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'rbtIssued
        '
        Me.rbtIssued.AutoSize = True
        Me.rbtIssued.Location = New System.Drawing.Point(164, 11)
        Me.rbtIssued.Name = "rbtIssued"
        Me.rbtIssued.Size = New System.Drawing.Size(56, 17)
        Me.rbtIssued.TabIndex = 2
        Me.rbtIssued.TabStop = True
        Me.rbtIssued.Text = "Issue"
        Me.rbtIssued.UseVisualStyleBackColor = True
        '
        'rbtReceived
        '
        Me.rbtReceived.AutoSize = True
        Me.rbtReceived.Location = New System.Drawing.Point(233, 11)
        Me.rbtReceived.Name = "rbtReceived"
        Me.rbtReceived.Size = New System.Drawing.Size(67, 17)
        Me.rbtReceived.TabIndex = 3
        Me.rbtReceived.TabStop = True
        Me.rbtReceived.Text = "Receipt"
        Me.rbtReceived.UseVisualStyleBackColor = True
        '
        'rbtDelivered
        '
        Me.rbtDelivered.AutoSize = True
        Me.rbtDelivered.Location = New System.Drawing.Point(83, 11)
        Me.rbtDelivered.Name = "rbtDelivered"
        Me.rbtDelivered.Size = New System.Drawing.Size(80, 17)
        Me.rbtDelivered.TabIndex = 1
        Me.rbtDelivered.TabStop = True
        Me.rbtDelivered.Text = "Delivered"
        Me.rbtDelivered.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Update To"
        '
        'grpIssuedDet
        '
        Me.grpIssuedDet.Controls.Add(Me.dtpBillDate)
        Me.grpIssuedDet.Controls.Add(Me.Label13)
        Me.grpIssuedDet.Controls.Add(Me.txtIssTranNo_NUM)
        Me.grpIssuedDet.Controls.Add(Me.Label12)
        Me.grpIssuedDet.Controls.Add(Me.cmbIssDesigner_MAN)
        Me.grpIssuedDet.Controls.Add(Me.txtIssNetWt_WET)
        Me.grpIssuedDet.Controls.Add(Me.txtIssGrsWt_WET)
        Me.grpIssuedDet.Controls.Add(Me.txtIssPcs_NUM)
        Me.grpIssuedDet.Controls.Add(Me.Label4)
        Me.grpIssuedDet.Controls.Add(Me.Label5)
        Me.grpIssuedDet.Controls.Add(Me.Label3)
        Me.grpIssuedDet.Controls.Add(Me.Label2)
        Me.grpIssuedDet.Enabled = False
        Me.grpIssuedDet.Location = New System.Drawing.Point(6, 35)
        Me.grpIssuedDet.Name = "grpIssuedDet"
        Me.grpIssuedDet.Size = New System.Drawing.Size(308, 123)
        Me.grpIssuedDet.TabIndex = 4
        Me.grpIssuedDet.TabStop = False
        Me.grpIssuedDet.Text = "Issue Detail"
        '
        'dtpBillDate
        '
        Me.dtpBillDate.Location = New System.Drawing.Point(215, 18)
        Me.dtpBillDate.Mask = "##/##/####"
        Me.dtpBillDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpBillDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpBillDate.Name = "dtpBillDate"
        Me.dtpBillDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpBillDate.Size = New System.Drawing.Size(83, 21)
        Me.dtpBillDate.TabIndex = 3
        Me.dtpBillDate.Text = "06/03/9998"
        Me.dtpBillDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(155, 21)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(34, 13)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "Date"
        '
        'txtIssTranNo_NUM
        '
        Me.txtIssTranNo_NUM.Location = New System.Drawing.Point(66, 18)
        Me.txtIssTranNo_NUM.Name = "txtIssTranNo_NUM"
        Me.txtIssTranNo_NUM.Size = New System.Drawing.Size(83, 21)
        Me.txtIssTranNo_NUM.TabIndex = 1
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(2, 21)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(57, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Issue No"
        '
        'cmbIssDesigner_MAN
        '
        Me.cmbIssDesigner_MAN.FormattingEnabled = True
        Me.cmbIssDesigner_MAN.Location = New System.Drawing.Point(66, 94)
        Me.cmbIssDesigner_MAN.Name = "cmbIssDesigner_MAN"
        Me.cmbIssDesigner_MAN.Size = New System.Drawing.Size(232, 21)
        Me.cmbIssDesigner_MAN.TabIndex = 11
        '
        'txtIssNetWt_WET
        '
        Me.txtIssNetWt_WET.Location = New System.Drawing.Point(215, 68)
        Me.txtIssNetWt_WET.Name = "txtIssNetWt_WET"
        Me.txtIssNetWt_WET.Size = New System.Drawing.Size(83, 21)
        Me.txtIssNetWt_WET.TabIndex = 9
        '
        'txtIssGrsWt_WET
        '
        Me.txtIssGrsWt_WET.Location = New System.Drawing.Point(66, 68)
        Me.txtIssGrsWt_WET.Name = "txtIssGrsWt_WET"
        Me.txtIssGrsWt_WET.Size = New System.Drawing.Size(83, 21)
        Me.txtIssGrsWt_WET.TabIndex = 7
        '
        'txtIssPcs_NUM
        '
        Me.txtIssPcs_NUM.Location = New System.Drawing.Point(66, 42)
        Me.txtIssPcs_NUM.Name = "txtIssPcs_NUM"
        Me.txtIssPcs_NUM.Size = New System.Drawing.Size(83, 21)
        Me.txtIssPcs_NUM.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(2, 99)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(58, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Designer"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(155, 71)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Net Wt"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(2, 71)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(42, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "GrsWt"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(2, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(26, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Pcs"
        '
        'grpReceived
        '
        Me.grpReceived.Controls.Add(Me.dtpRecBillDate)
        Me.grpReceived.Controls.Add(Me.Label14)
        Me.grpReceived.Controls.Add(Me.txtRecTranNo_NUM)
        Me.grpReceived.Controls.Add(Me.Label15)
        Me.grpReceived.Controls.Add(Me.cmbRecDesigner_MAN)
        Me.grpReceived.Controls.Add(Me.txtRecMc_AMT)
        Me.grpReceived.Controls.Add(Me.txtRecNetWt_WET)
        Me.grpReceived.Controls.Add(Me.txtRecWastage_WET)
        Me.grpReceived.Controls.Add(Me.txtRecGrsWt_WET)
        Me.grpReceived.Controls.Add(Me.txtRecPcs_NUM)
        Me.grpReceived.Controls.Add(Me.Label6)
        Me.grpReceived.Controls.Add(Me.Label11)
        Me.grpReceived.Controls.Add(Me.Label7)
        Me.grpReceived.Controls.Add(Me.Label10)
        Me.grpReceived.Controls.Add(Me.Label8)
        Me.grpReceived.Controls.Add(Me.Label9)
        Me.grpReceived.Enabled = False
        Me.grpReceived.Location = New System.Drawing.Point(6, 164)
        Me.grpReceived.Name = "grpReceived"
        Me.grpReceived.Size = New System.Drawing.Size(308, 165)
        Me.grpReceived.TabIndex = 5
        Me.grpReceived.TabStop = False
        Me.grpReceived.Text = "Receipt Details"
        '
        'cmbRecDesigner_MAN
        '
        Me.cmbRecDesigner_MAN.FormattingEnabled = True
        Me.cmbRecDesigner_MAN.Location = New System.Drawing.Point(66, 128)
        Me.cmbRecDesigner_MAN.Name = "cmbRecDesigner_MAN"
        Me.cmbRecDesigner_MAN.Size = New System.Drawing.Size(232, 21)
        Me.cmbRecDesigner_MAN.TabIndex = 15
        '
        'txtRecMc_AMT
        '
        Me.txtRecMc_AMT.Location = New System.Drawing.Point(215, 102)
        Me.txtRecMc_AMT.Name = "txtRecMc_AMT"
        Me.txtRecMc_AMT.Size = New System.Drawing.Size(83, 21)
        Me.txtRecMc_AMT.TabIndex = 13
        '
        'txtRecNetWt_WET
        '
        Me.txtRecNetWt_WET.Location = New System.Drawing.Point(215, 76)
        Me.txtRecNetWt_WET.Name = "txtRecNetWt_WET"
        Me.txtRecNetWt_WET.Size = New System.Drawing.Size(83, 21)
        Me.txtRecNetWt_WET.TabIndex = 9
        '
        'txtRecWastage_WET
        '
        Me.txtRecWastage_WET.Location = New System.Drawing.Point(66, 102)
        Me.txtRecWastage_WET.Name = "txtRecWastage_WET"
        Me.txtRecWastage_WET.Size = New System.Drawing.Size(83, 21)
        Me.txtRecWastage_WET.TabIndex = 11
        '
        'txtRecGrsWt_WET
        '
        Me.txtRecGrsWt_WET.Location = New System.Drawing.Point(66, 76)
        Me.txtRecGrsWt_WET.Name = "txtRecGrsWt_WET"
        Me.txtRecGrsWt_WET.Size = New System.Drawing.Size(83, 21)
        Me.txtRecGrsWt_WET.TabIndex = 7
        '
        'txtRecPcs_NUM
        '
        Me.txtRecPcs_NUM.Location = New System.Drawing.Point(66, 50)
        Me.txtRecPcs_NUM.Name = "txtRecPcs_NUM"
        Me.txtRecPcs_NUM.Size = New System.Drawing.Size(83, 21)
        Me.txtRecPcs_NUM.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(2, 131)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(58, 13)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Designer"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(155, 105)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(22, 13)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "Mc"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(155, 79)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(45, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Net Wt"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(2, 105)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(56, 13)
        Me.Label10.TabIndex = 10
        Me.Label10.Text = "Wastage"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(2, 79)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(42, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "GrsWt"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(2, 53)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(26, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Pcs"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(102, 335)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Update"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(208, 335)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 1
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
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
        '
        'dtpRecBillDate
        '
        Me.dtpRecBillDate.Location = New System.Drawing.Point(215, 23)
        Me.dtpRecBillDate.Mask = "##/##/####"
        Me.dtpRecBillDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpRecBillDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpRecBillDate.Name = "dtpRecBillDate"
        Me.dtpRecBillDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpRecBillDate.Size = New System.Drawing.Size(83, 21)
        Me.dtpRecBillDate.TabIndex = 3
        Me.dtpRecBillDate.Text = "06/03/9998"
        Me.dtpRecBillDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(155, 26)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(34, 13)
        Me.Label14.TabIndex = 2
        Me.Label14.Text = "Date"
        '
        'txtRecTranNo_NUM
        '
        Me.txtRecTranNo_NUM.Location = New System.Drawing.Point(66, 23)
        Me.txtRecTranNo_NUM.Name = "txtRecTranNo_NUM"
        Me.txtRecTranNo_NUM.Size = New System.Drawing.Size(83, 21)
        Me.txtRecTranNo_NUM.TabIndex = 1
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(2, 26)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(68, 13)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "Receipt No"
        '
        'OrderStatusUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(320, 377)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpReceived)
        Me.Controls.Add(Me.grpIssuedDet)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.rbtDelivered)
        Me.Controls.Add(Me.rbtReceived)
        Me.Controls.Add(Me.rbtIssued)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "OrderStatusUpdate"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Order Status Update"
        Me.grpIssuedDet.ResumeLayout(False)
        Me.grpIssuedDet.PerformLayout()
        Me.grpReceived.ResumeLayout(False)
        Me.grpReceived.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents rbtIssued As System.Windows.Forms.RadioButton
    Friend WithEvents rbtReceived As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDelivered As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grpIssuedDet As System.Windows.Forms.GroupBox
    Friend WithEvents cmbIssDesigner_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents txtIssGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtIssPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtIssNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents grpReceived As System.Windows.Forms.GroupBox
    Friend WithEvents cmbRecDesigner_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents txtRecMc_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtRecNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtRecWastage_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtRecGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtRecPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtIssTranNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents dtpBillDate As BrighttechPack.DatePicker
    Friend WithEvents dtpRecBillDate As BrighttechPack.DatePicker
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtRecTranNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
End Class
