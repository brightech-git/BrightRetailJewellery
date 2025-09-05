<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_ORDERREPAIRSTATUS
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
        Me.txtOrdRepNo = New System.Windows.Forms.TextBox
        Me.cmbCostcentre = New System.Windows.Forms.ComboBox
        Me.grpContainer = New System.Windows.Forms.GroupBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbtRepair = New System.Windows.Forms.RadioButton
        Me.rbtorder = New System.Windows.Forms.RadioButton
        Me.chkImage = New System.Windows.Forms.CheckBox
        Me.rbtReqdetail = New System.Windows.Forms.RadioButton
        Me.rbtStatus = New System.Windows.Forms.RadioButton
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.rbtnbooked = New System.Windows.Forms.RadioButton
        Me.grpContainer.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtOrdRepNo
        '
        Me.txtOrdRepNo.Location = New System.Drawing.Point(108, 75)
        Me.txtOrdRepNo.Name = "txtOrdRepNo"
        Me.txtOrdRepNo.Size = New System.Drawing.Size(103, 21)
        Me.txtOrdRepNo.TabIndex = 4
        '
        'cmbCostcentre
        '
        Me.cmbCostcentre.FormattingEnabled = True
        Me.cmbCostcentre.Location = New System.Drawing.Point(108, 48)
        Me.cmbCostcentre.Name = "cmbCostcentre"
        Me.cmbCostcentre.Size = New System.Drawing.Size(206, 21)
        Me.cmbCostcentre.TabIndex = 2
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.Panel1)
        Me.grpContainer.Controls.Add(Me.chkImage)
        Me.grpContainer.Controls.Add(Me.rbtReqdetail)
        Me.grpContainer.Controls.Add(Me.rbtStatus)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.cmbCostcentre)
        Me.grpContainer.Controls.Add(Me.txtOrdRepNo)
        Me.grpContainer.Location = New System.Drawing.Point(126, 62)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(360, 224)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtnbooked)
        Me.Panel1.Controls.Add(Me.rbtRepair)
        Me.Panel1.Controls.Add(Me.rbtorder)
        Me.Panel1.Location = New System.Drawing.Point(108, 20)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(218, 21)
        Me.Panel1.TabIndex = 0
        '
        'rbtRepair
        '
        Me.rbtRepair.AutoSize = True
        Me.rbtRepair.Location = New System.Drawing.Point(71, 1)
        Me.rbtRepair.Name = "rbtRepair"
        Me.rbtRepair.Size = New System.Drawing.Size(62, 17)
        Me.rbtRepair.TabIndex = 1
        Me.rbtRepair.Text = "Repair"
        Me.rbtRepair.UseVisualStyleBackColor = True
        '
        'rbtorder
        '
        Me.rbtorder.AutoSize = True
        Me.rbtorder.Checked = True
        Me.rbtorder.Location = New System.Drawing.Point(7, 1)
        Me.rbtorder.Name = "rbtorder"
        Me.rbtorder.Size = New System.Drawing.Size(58, 17)
        Me.rbtorder.TabIndex = 0
        Me.rbtorder.TabStop = True
        Me.rbtorder.Text = "Order"
        Me.rbtorder.UseVisualStyleBackColor = True
        '
        'chkImage
        '
        Me.chkImage.AutoSize = True
        Me.chkImage.Location = New System.Drawing.Point(108, 131)
        Me.chkImage.Name = "chkImage"
        Me.chkImage.Size = New System.Drawing.Size(63, 17)
        Me.chkImage.TabIndex = 7
        Me.chkImage.Text = "Image"
        Me.chkImage.UseVisualStyleBackColor = True
        '
        'rbtReqdetail
        '
        Me.rbtReqdetail.AutoSize = True
        Me.rbtReqdetail.Location = New System.Drawing.Point(208, 104)
        Me.rbtReqdetail.Name = "rbtReqdetail"
        Me.rbtReqdetail.Size = New System.Drawing.Size(135, 17)
        Me.rbtReqdetail.TabIndex = 6
        Me.rbtReqdetail.Text = "Requirement Detail"
        Me.rbtReqdetail.UseVisualStyleBackColor = True
        '
        'rbtStatus
        '
        Me.rbtStatus.AutoSize = True
        Me.rbtStatus.Checked = True
        Me.rbtStatus.Location = New System.Drawing.Point(108, 104)
        Me.rbtStatus.Name = "rbtStatus"
        Me.rbtStatus.Size = New System.Drawing.Size(61, 17)
        Me.rbtStatus.TabIndex = 5
        Me.rbtStatus.TabStop = True
        Me.rbtStatus.Text = "Status"
        Me.rbtStatus.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(214, 163)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 9
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(108, 163)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 8
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Order No"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Cost Centre"
        '
        'rbtnbooked
        '
        Me.rbtnbooked.AutoSize = True
        Me.rbtnbooked.Location = New System.Drawing.Point(138, 1)
        Me.rbtnbooked.Name = "rbtnbooked"
        Me.rbtnbooked.Size = New System.Drawing.Size(68, 17)
        Me.rbtnbooked.TabIndex = 2
        Me.rbtnbooked.Text = "Booked"
        Me.rbtnbooked.UseVisualStyleBackColor = True
        '
        'FRM_ORDERREPAIRSTATUS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(613, 349)
        Me.Controls.Add(Me.grpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FRM_ORDERREPAIRSTATUS"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ORDER/REPAIR STATUS"
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtOrdRepNo As System.Windows.Forms.TextBox
    Friend WithEvents cmbCostcentre As System.Windows.Forms.ComboBox
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents rbtReqdetail As System.Windows.Forms.RadioButton
    Friend WithEvents rbtStatus As System.Windows.Forms.RadioButton
    Friend WithEvents chkImage As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtRepair As System.Windows.Forms.RadioButton
    Friend WithEvents rbtorder As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnbooked As System.Windows.Forms.RadioButton
End Class
