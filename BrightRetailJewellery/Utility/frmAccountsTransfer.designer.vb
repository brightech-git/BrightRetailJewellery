<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAccountsTransfer
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
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.Label = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtFromAcName = New System.Windows.Forms.TextBox
        Me.txtToAcName = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.chkAccodeDelete = New System.Windows.Forms.CheckBox
        Me.btnTransfer = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.gridDisplay = New System.Windows.Forms.DataGridView
        CType(Me.gridDisplay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(115, 60)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(422, 22)
        Me.chkCmbCostCentre.TabIndex = 3
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(115, 32)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(422, 22)
        Me.chkCmbCompany.TabIndex = 1
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(22, 63)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 2
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(22, 35)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(62, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Company"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFromAcName
        '
        Me.txtFromAcName.Location = New System.Drawing.Point(115, 88)
        Me.txtFromAcName.Name = "txtFromAcName"
        Me.txtFromAcName.Size = New System.Drawing.Size(767, 21)
        Me.txtFromAcName.TabIndex = 5
        '
        'txtToAcName
        '
        Me.txtToAcName.Location = New System.Drawing.Point(115, 115)
        Me.txtToAcName.Name = "txtToAcName"
        Me.txtToAcName.Size = New System.Drawing.Size(767, 21)
        Me.txtToAcName.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 91)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "From AcName"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(22, 118)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "To AcName"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkAccodeDelete
        '
        Me.chkAccodeDelete.AutoSize = True
        Me.chkAccodeDelete.Location = New System.Drawing.Point(115, 142)
        Me.chkAccodeDelete.Name = "chkAccodeDelete"
        Me.chkAccodeDelete.Size = New System.Drawing.Size(147, 17)
        Me.chkAccodeDelete.TabIndex = 8
        Me.chkAccodeDelete.Text = "Delete From AcName"
        Me.chkAccodeDelete.UseVisualStyleBackColor = True
        '
        'btnTransfer
        '
        Me.btnTransfer.Location = New System.Drawing.Point(115, 165)
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.Size = New System.Drawing.Size(100, 30)
        Me.btnTransfer.TabIndex = 9
        Me.btnTransfer.Text = "&Transfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(221, 165)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 10
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(327, 165)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 11
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'gridDisplay
        '
        Me.gridDisplay.AllowUserToAddRows = False
        Me.gridDisplay.AllowUserToDeleteRows = False
        Me.gridDisplay.AllowUserToResizeColumns = False
        Me.gridDisplay.AllowUserToResizeRows = False
        Me.gridDisplay.BackgroundColor = System.Drawing.Color.White
        Me.gridDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridDisplay.ColumnHeadersVisible = False
        Me.gridDisplay.Location = New System.Drawing.Point(331, -10)
        Me.gridDisplay.Name = "gridDisplay"
        Me.gridDisplay.RowHeadersVisible = False
        Me.gridDisplay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridDisplay.Size = New System.Drawing.Size(756, 92)
        Me.gridDisplay.TabIndex = 12
        '
        'frmAccountsTransfer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(894, 221)
        Me.Controls.Add(Me.gridDisplay)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.btnTransfer)
        Me.Controls.Add(Me.chkAccodeDelete)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtToAcName)
        Me.Controls.Add(Me.txtFromAcName)
        Me.Controls.Add(Me.chkCmbCostCentre)
        Me.Controls.Add(Me.chkCmbCompany)
        Me.Controls.Add(Me.Label)
        Me.Controls.Add(Me.Label9)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "frmAccountsTransfer"
        Me.Text = "Accounts Transfer"
        CType(Me.gridDisplay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtFromAcName As System.Windows.Forms.TextBox
    Friend WithEvents txtToAcName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkAccodeDelete As System.Windows.Forms.CheckBox
    Friend WithEvents btnTransfer As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents gridDisplay As System.Windows.Forms.DataGridView
End Class
