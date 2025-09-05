<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LotBulkIssueDia
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
        Me.cmbDesigner_MAN = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbCostCentre_Man = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label30 = New System.Windows.Forms.Label
        Me.cmbItemCounter_MAN = New System.Windows.Forms.ComboBox
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbEntryType = New System.Windows.Forms.ComboBox
        Me.cmbSubItemName_Man = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtItemName = New System.Windows.Forms.TextBox
        Me.txtItemCode_Num_Man = New System.Windows.Forms.TextBox
        Me.chkIssToAssort = New System.Windows.Forms.CheckBox
        Me.pnlItemCode = New System.Windows.Forms.Panel
        Me.pnlItemCode.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmbDesigner_MAN
        '
        Me.cmbDesigner_MAN.FormattingEnabled = True
        Me.cmbDesigner_MAN.Location = New System.Drawing.Point(100, 34)
        Me.cmbDesigner_MAN.Name = "cmbDesigner_MAN"
        Me.cmbDesigner_MAN.Size = New System.Drawing.Size(306, 21)
        Me.cmbDesigner_MAN.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Designer"
        '
        'cmbCostCentre_Man
        '
        Me.cmbCostCentre_Man.FormattingEnabled = True
        Me.cmbCostCentre_Man.Location = New System.Drawing.Point(100, 115)
        Me.cmbCostCentre_Man.Name = "cmbCostCentre_Man"
        Me.cmbCostCentre_Man.Size = New System.Drawing.Size(306, 21)
        Me.cmbCostCentre_Man.TabIndex = 7
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(10, 119)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(76, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Cost Centre"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(10, 147)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(84, 13)
        Me.Label30.TabIndex = 8
        Me.Label30.Text = "Item Counter"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItemCounter_MAN
        '
        Me.cmbItemCounter_MAN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbItemCounter_MAN.FormattingEnabled = True
        Me.cmbItemCounter_MAN.Location = New System.Drawing.Point(100, 143)
        Me.cmbItemCounter_MAN.Name = "cmbItemCounter_MAN"
        Me.cmbItemCounter_MAN.Size = New System.Drawing.Size(306, 21)
        Me.cmbItemCounter_MAN.TabIndex = 9
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(200, 170)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 10
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(306, 170)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Entry Type"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbEntryType
        '
        Me.cmbEntryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEntryType.FormattingEnabled = True
        Me.cmbEntryType.Items.AddRange(New Object() {"REGULAR", "ORDER", "REPAIR"})
        Me.cmbEntryType.Location = New System.Drawing.Point(100, 6)
        Me.cmbEntryType.Name = "cmbEntryType"
        Me.cmbEntryType.Size = New System.Drawing.Size(138, 21)
        Me.cmbEntryType.TabIndex = 1
        '
        'cmbSubItemName_Man
        '
        Me.cmbSubItemName_Man.FormattingEnabled = True
        Me.cmbSubItemName_Man.Location = New System.Drawing.Point(95, 30)
        Me.cmbSubItemName_Man.Name = "cmbSubItemName_Man"
        Me.cmbSubItemName_Man.Size = New System.Drawing.Size(306, 21)
        Me.cmbSubItemName_Man.TabIndex = 4
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(5, 6)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Item Code"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(5, 33)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(60, 13)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "Sub Item"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtItemName
        '
        Me.txtItemName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemName.Enabled = False
        Me.txtItemName.Location = New System.Drawing.Point(154, 3)
        Me.txtItemName.MaxLength = 50
        Me.txtItemName.Name = "txtItemName"
        Me.txtItemName.Size = New System.Drawing.Size(247, 21)
        Me.txtItemName.TabIndex = 2
        '
        'txtItemCode_Num_Man
        '
        Me.txtItemCode_Num_Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemCode_Num_Man.Location = New System.Drawing.Point(95, 3)
        Me.txtItemCode_Num_Man.MaxLength = 8
        Me.txtItemCode_Num_Man.Name = "txtItemCode_Num_Man"
        Me.txtItemCode_Num_Man.Size = New System.Drawing.Size(53, 21)
        Me.txtItemCode_Num_Man.TabIndex = 1
        '
        'chkIssToAssort
        '
        Me.chkIssToAssort.AutoSize = True
        Me.chkIssToAssort.Location = New System.Drawing.Point(244, 9)
        Me.chkIssToAssort.Name = "chkIssToAssort"
        Me.chkIssToAssort.Size = New System.Drawing.Size(166, 17)
        Me.chkIssToAssort.TabIndex = 2
        Me.chkIssToAssort.Text = "Issue To Assorted Items"
        Me.chkIssToAssort.UseVisualStyleBackColor = True
        '
        'pnlItemCode
        '
        Me.pnlItemCode.Controls.Add(Me.txtItemCode_Num_Man)
        Me.pnlItemCode.Controls.Add(Me.txtItemName)
        Me.pnlItemCode.Controls.Add(Me.cmbSubItemName_Man)
        Me.pnlItemCode.Controls.Add(Me.Label9)
        Me.pnlItemCode.Controls.Add(Me.Label8)
        Me.pnlItemCode.Location = New System.Drawing.Point(5, 57)
        Me.pnlItemCode.Name = "pnlItemCode"
        Me.pnlItemCode.Size = New System.Drawing.Size(405, 56)
        Me.pnlItemCode.TabIndex = 5
        '
        'LotBulkIssueDia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(419, 207)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlItemCode)
        Me.Controls.Add(Me.chkIssToAssort)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbEntryType)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.Label30)
        Me.Controls.Add(Me.cmbItemCounter_MAN)
        Me.Controls.Add(Me.cmbCostCentre_Man)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.cmbDesigner_MAN)
        Me.Controls.Add(Me.Label2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "LotBulkIssueDia"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LotBulkIssueDia"
        Me.pnlItemCode.ResumeLayout(False)
        Me.pnlItemCode.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbDesigner_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents cmbItemCounter_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbEntryType As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubItemName_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtItemName As System.Windows.Forms.TextBox
    Friend WithEvents txtItemCode_Num_Man As System.Windows.Forms.TextBox
    Friend WithEvents chkIssToAssort As System.Windows.Forms.CheckBox
    Friend WithEvents pnlItemCode As System.Windows.Forms.Panel
End Class
