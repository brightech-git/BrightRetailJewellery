<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBookedItem
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtRunNO = New System.Windows.Forms.TextBox
        Me.txtBalance_AMT = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtRateFix = New System.Windows.Forms.TextBox
        Me.gridRes = New System.Windows.Forms.DataGridView
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.Grouper1 = New CodeVendor.Controls.Grouper
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtAdvanceGST_AMT = New System.Windows.Forms.TextBox
        Me.txtAdvMode = New System.Windows.Forms.TextBox
        Me.txtCompanyid = New System.Windows.Forms.TextBox
        Me.gridBookedAdvance = New System.Windows.Forms.DataGridView
        Me.txtDoorNo = New System.Windows.Forms.TextBox
        Me.txtName = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtAdjusted_AMT = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtReceived_AMT = New System.Windows.Forms.TextBox
        Me.Grouper2 = New CodeVendor.Controls.Grouper
        Me.txtSalesManName = New System.Windows.Forms.TextBox
        Me.txtSalesMan_NUM = New System.Windows.Forms.TextBox
        Me.Label39 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.txtAdvanceAmt_AMT = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        CType(Me.gridRes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Grouper1.SuspendLayout()
        CType(Me.gridBookedAdvance, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Grouper2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Booked No"
        '
        'txtRunNO
        '
        Me.txtRunNO.Location = New System.Drawing.Point(9, 32)
        Me.txtRunNO.Name = "txtRunNO"
        Me.txtRunNO.Size = New System.Drawing.Size(85, 21)
        Me.txtRunNO.TabIndex = 1
        '
        'txtBalance_AMT
        '
        Me.txtBalance_AMT.Location = New System.Drawing.Point(267, 32)
        Me.txtBalance_AMT.Name = "txtBalance_AMT"
        Me.txtBalance_AMT.Size = New System.Drawing.Size(85, 21)
        Me.txtBalance_AMT.TabIndex = 7
        Me.txtBalance_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(284, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Balance"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(362, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Fixed Rate"
        '
        'txtRateFix
        '
        Me.txtRateFix.Location = New System.Drawing.Point(353, 32)
        Me.txtRateFix.Name = "txtRateFix"
        Me.txtRateFix.Size = New System.Drawing.Size(85, 21)
        Me.txtRateFix.TabIndex = 9
        Me.txtRateFix.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'gridRes
        '
        Me.gridRes.AllowUserToAddRows = False
        Me.gridRes.AllowUserToDeleteRows = False
        Me.gridRes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridRes.Location = New System.Drawing.Point(9, 16)
        Me.gridRes.Name = "gridRes"
        Me.gridRes.ReadOnly = True
        Me.gridRes.Size = New System.Drawing.Size(887, 112)
        Me.gridRes.TabIndex = 0
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(691, 133)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 4
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(797, 133)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 5
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Grouper1
        '
        Me.Grouper1.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper1.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper1.BorderThickness = 1.0!
        Me.Grouper1.Controls.Add(Me.Label9)
        Me.Grouper1.Controls.Add(Me.txtAdvanceAmt_AMT)
        Me.Grouper1.Controls.Add(Me.Label7)
        Me.Grouper1.Controls.Add(Me.txtAdvanceGST_AMT)
        Me.Grouper1.Controls.Add(Me.txtAdvMode)
        Me.Grouper1.Controls.Add(Me.txtCompanyid)
        Me.Grouper1.Controls.Add(Me.gridBookedAdvance)
        Me.Grouper1.Controls.Add(Me.txtDoorNo)
        Me.Grouper1.Controls.Add(Me.txtName)
        Me.Grouper1.Controls.Add(Me.Label8)
        Me.Grouper1.Controls.Add(Me.Label4)
        Me.Grouper1.Controls.Add(Me.Label1)
        Me.Grouper1.Controls.Add(Me.txtRunNO)
        Me.Grouper1.Controls.Add(Me.txtRateFix)
        Me.Grouper1.Controls.Add(Me.Label3)
        Me.Grouper1.Controls.Add(Me.Label6)
        Me.Grouper1.Controls.Add(Me.txtAdjusted_AMT)
        Me.Grouper1.Controls.Add(Me.Label5)
        Me.Grouper1.Controls.Add(Me.txtReceived_AMT)
        Me.Grouper1.Controls.Add(Me.Label2)
        Me.Grouper1.Controls.Add(Me.txtBalance_AMT)
        Me.Grouper1.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper1.GroupImage = Nothing
        Me.Grouper1.GroupTitle = ""
        Me.Grouper1.Location = New System.Drawing.Point(7, -3)
        Me.Grouper1.Name = "Grouper1"
        Me.Grouper1.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper1.PaintGroupBox = False
        Me.Grouper1.RoundCorners = 10
        Me.Grouper1.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper1.ShadowControl = False
        Me.Grouper1.ShadowThickness = 3
        Me.Grouper1.Size = New System.Drawing.Size(905, 175)
        Me.Grouper1.TabIndex = 0
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(543, 16)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(31, 13)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "GST"
        '
        'txtAdvanceGST_AMT
        '
        Me.txtAdvanceGST_AMT.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAdvanceGST_AMT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdvanceGST_AMT.Location = New System.Drawing.Point(528, 32)
        Me.txtAdvanceGST_AMT.MaxLength = 12
        Me.txtAdvanceGST_AMT.Name = "txtAdvanceGST_AMT"
        Me.txtAdvanceGST_AMT.ReadOnly = True
        Me.txtAdvanceGST_AMT.Size = New System.Drawing.Size(66, 21)
        Me.txtAdvanceGST_AMT.TabIndex = 11
        '
        'txtAdvMode
        '
        Me.txtAdvMode.Location = New System.Drawing.Point(863, 8)
        Me.txtAdvMode.Name = "txtAdvMode"
        Me.txtAdvMode.Size = New System.Drawing.Size(34, 21)
        Me.txtAdvMode.TabIndex = 12
        Me.txtAdvMode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtAdvMode.Visible = False
        '
        'txtCompanyid
        '
        Me.txtCompanyid.Location = New System.Drawing.Point(879, 32)
        Me.txtCompanyid.Name = "txtCompanyid"
        Me.txtCompanyid.Size = New System.Drawing.Size(22, 21)
        Me.txtCompanyid.TabIndex = 11
        Me.txtCompanyid.Visible = False
        '
        'gridBookedAdvance
        '
        Me.gridBookedAdvance.AllowUserToAddRows = False
        Me.gridBookedAdvance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridBookedAdvance.Location = New System.Drawing.Point(9, 54)
        Me.gridBookedAdvance.Name = "gridBookedAdvance"
        Me.gridBookedAdvance.ReadOnly = True
        Me.gridBookedAdvance.Size = New System.Drawing.Size(887, 114)
        Me.gridBookedAdvance.TabIndex = 16
        '
        'txtDoorNo
        '
        Me.txtDoorNo.Location = New System.Drawing.Point(737, 32)
        Me.txtDoorNo.Name = "txtDoorNo"
        Me.txtDoorNo.Size = New System.Drawing.Size(140, 21)
        Me.txtDoorNo.TabIndex = 15
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(595, 32)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(141, 21)
        Me.txtName.TabIndex = 13
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(783, 16)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(53, 13)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Address"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(649, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Name"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(198, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(57, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Adjusted"
        '
        'txtAdjusted_AMT
        '
        Me.txtAdjusted_AMT.Location = New System.Drawing.Point(181, 32)
        Me.txtAdjusted_AMT.Name = "txtAdjusted_AMT"
        Me.txtAdjusted_AMT.Size = New System.Drawing.Size(85, 21)
        Me.txtAdjusted_AMT.TabIndex = 5
        Me.txtAdjusted_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(112, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Received"
        '
        'txtReceived_AMT
        '
        Me.txtReceived_AMT.Location = New System.Drawing.Point(95, 32)
        Me.txtReceived_AMT.Name = "txtReceived_AMT"
        Me.txtReceived_AMT.Size = New System.Drawing.Size(85, 21)
        Me.txtReceived_AMT.TabIndex = 3
        Me.txtReceived_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Grouper2
        '
        Me.Grouper2.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper2.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Grouper2.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper2.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper2.BorderThickness = 1.0!
        Me.Grouper2.Controls.Add(Me.txtSalesManName)
        Me.Grouper2.Controls.Add(Me.txtSalesMan_NUM)
        Me.Grouper2.Controls.Add(Me.Label39)
        Me.Grouper2.Controls.Add(Me.gridRes)
        Me.Grouper2.Controls.Add(Me.btnExit)
        Me.Grouper2.Controls.Add(Me.btnOk)
        Me.Grouper2.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper2.GroupImage = Nothing
        Me.Grouper2.GroupTitle = ""
        Me.Grouper2.Location = New System.Drawing.Point(7, 167)
        Me.Grouper2.Name = "Grouper2"
        Me.Grouper2.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper2.PaintGroupBox = False
        Me.Grouper2.RoundCorners = 10
        Me.Grouper2.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper2.ShadowControl = False
        Me.Grouper2.ShadowThickness = 3
        Me.Grouper2.Size = New System.Drawing.Size(905, 168)
        Me.Grouper2.TabIndex = 1
        '
        'txtSalesManName
        '
        Me.txtSalesManName.Location = New System.Drawing.Point(515, 139)
        Me.txtSalesManName.Name = "txtSalesManName"
        Me.txtSalesManName.Size = New System.Drawing.Size(170, 21)
        Me.txtSalesManName.TabIndex = 3
        '
        'txtSalesMan_NUM
        '
        Me.txtSalesMan_NUM.Location = New System.Drawing.Point(467, 139)
        Me.txtSalesMan_NUM.Name = "txtSalesMan_NUM"
        Me.txtSalesMan_NUM.Size = New System.Drawing.Size(46, 21)
        Me.txtSalesMan_NUM.TabIndex = 2
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Location = New System.Drawing.Point(396, 143)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(63, 13)
        Me.Label39.TabIndex = 1
        Me.Label39.Text = "Employee"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 26)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'txtAdvanceAmt_AMT
        '
        Me.txtAdvanceAmt_AMT.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAdvanceAmt_AMT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdvanceAmt_AMT.Location = New System.Drawing.Point(439, 32)
        Me.txtAdvanceAmt_AMT.MaxLength = 12
        Me.txtAdvanceAmt_AMT.Name = "txtAdvanceAmt_AMT"
        Me.txtAdvanceAmt_AMT.ReadOnly = True
        Me.txtAdvanceAmt_AMT.Size = New System.Drawing.Size(88, 21)
        Me.txtAdvanceAmt_AMT.TabIndex = 17
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(457, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(51, 13)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "Amount"
        '
        'frmBookedItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(920, 340)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Grouper1)
        Me.Controls.Add(Me.Grouper2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmBookedItem"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Booked Item"
        CType(Me.gridRes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Grouper1.ResumeLayout(False)
        Me.Grouper1.PerformLayout()
        CType(Me.gridBookedAdvance, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Grouper2.ResumeLayout(False)
        Me.Grouper2.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents gridRes As System.Windows.Forms.DataGridView
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents Grouper1 As CodeVendor.Controls.Grouper
    Friend WithEvents Grouper2 As CodeVendor.Controls.Grouper
    Public WithEvents txtRunNO As System.Windows.Forms.TextBox
    Public WithEvents txtBalance_AMT As System.Windows.Forms.TextBox
    Public WithEvents txtRateFix As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtDoorNo As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridBookedAdvance As System.Windows.Forms.DataGridView
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents txtAdjusted_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents txtReceived_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtSalesManName As System.Windows.Forms.TextBox
    Friend WithEvents txtSalesMan_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents txtCompanyid As System.Windows.Forms.TextBox
    Public WithEvents txtAdvMode As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtAdvanceGST_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtAdvanceAmt_AMT As System.Windows.Forms.TextBox
End Class
