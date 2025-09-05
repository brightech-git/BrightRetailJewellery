<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmStoneGroupReport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.chkCmbSubItem = New BrighttechPack.CheckedComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.chkCmbItem = New BrighttechPack.CheckedComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.chkCmbMetel = New BrighttechPack.CheckedComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkCmbStoneGroup = New BrighttechPack.CheckedComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkCmbCategory = New BrighttechPack.CheckedComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkCmbGropBy = New BrighttechPack.CheckedComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.Label = New System.Windows.Forms.Label()
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.Label6 = New System.Windows.Forms.Label()
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.pnlMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.btnNew)
        Me.pnlMain.Controls.Add(Me.btnView_Search)
        Me.pnlMain.Controls.Add(Me.btnExit)
        Me.pnlMain.Controls.Add(Me.chkCmbSubItem)
        Me.pnlMain.Controls.Add(Me.Label8)
        Me.pnlMain.Controls.Add(Me.chkCmbItem)
        Me.pnlMain.Controls.Add(Me.Label7)
        Me.pnlMain.Controls.Add(Me.chkCmbMetel)
        Me.pnlMain.Controls.Add(Me.Label2)
        Me.pnlMain.Controls.Add(Me.chkCmbStoneGroup)
        Me.pnlMain.Controls.Add(Me.Label3)
        Me.pnlMain.Controls.Add(Me.chkCmbCategory)
        Me.pnlMain.Controls.Add(Me.Label4)
        Me.pnlMain.Controls.Add(Me.chkCmbGropBy)
        Me.pnlMain.Controls.Add(Me.Label1)
        Me.pnlMain.Controls.Add(Me.chkCmbCostCentre)
        Me.pnlMain.Controls.Add(Me.Label)
        Me.pnlMain.Controls.Add(Me.chkCmbCompany)
        Me.pnlMain.Controls.Add(Me.Label5)
        Me.pnlMain.Controls.Add(Me.Label9)
        Me.pnlMain.Controls.Add(Me.dtpTo)
        Me.pnlMain.Controls.Add(Me.Label6)
        Me.pnlMain.Controls.Add(Me.dtpFrom)
        Me.pnlMain.Location = New System.Drawing.Point(289, 73)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(354, 305)
        Me.pnlMain.TabIndex = 0
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(125, 262)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 21
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(9, 262)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 20
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(241, 262)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 22
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'chkCmbSubItem
        '
        Me.chkCmbSubItem.CheckOnClick = True
        Me.chkCmbSubItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbSubItem.DropDownHeight = 1
        Me.chkCmbSubItem.FormattingEnabled = True
        Me.chkCmbSubItem.IntegralHeight = False
        Me.chkCmbSubItem.Location = New System.Drawing.Point(90, 234)
        Me.chkCmbSubItem.Name = "chkCmbSubItem"
        Me.chkCmbSubItem.Size = New System.Drawing.Size(251, 22)
        Me.chkCmbSubItem.TabIndex = 19
        Me.chkCmbSubItem.ValueSeparator = ", "
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 239)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(60, 13)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Sub Item"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbItem
        '
        Me.chkCmbItem.CheckOnClick = True
        Me.chkCmbItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItem.DropDownHeight = 1
        Me.chkCmbItem.FormattingEnabled = True
        Me.chkCmbItem.IntegralHeight = False
        Me.chkCmbItem.Location = New System.Drawing.Point(90, 206)
        Me.chkCmbItem.Name = "chkCmbItem"
        Me.chkCmbItem.Size = New System.Drawing.Size(251, 22)
        Me.chkCmbItem.TabIndex = 17
        Me.chkCmbItem.ValueSeparator = ", "
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 211)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(34, 13)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Item"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbMetel
        '
        Me.chkCmbMetel.CheckOnClick = True
        Me.chkCmbMetel.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbMetel.DropDownHeight = 1
        Me.chkCmbMetel.FormattingEnabled = True
        Me.chkCmbMetel.IntegralHeight = False
        Me.chkCmbMetel.Location = New System.Drawing.Point(90, 122)
        Me.chkCmbMetel.Name = "chkCmbMetel"
        Me.chkCmbMetel.Size = New System.Drawing.Size(251, 22)
        Me.chkCmbMetel.TabIndex = 11
        Me.chkCmbMetel.ValueSeparator = ", "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 183)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Stone Group"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbStoneGroup
        '
        Me.chkCmbStoneGroup.CheckOnClick = True
        Me.chkCmbStoneGroup.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbStoneGroup.DropDownHeight = 1
        Me.chkCmbStoneGroup.FormattingEnabled = True
        Me.chkCmbStoneGroup.IntegralHeight = False
        Me.chkCmbStoneGroup.Location = New System.Drawing.Point(90, 178)
        Me.chkCmbStoneGroup.Name = "chkCmbStoneGroup"
        Me.chkCmbStoneGroup.Size = New System.Drawing.Size(251, 22)
        Me.chkCmbStoneGroup.TabIndex = 15
        Me.chkCmbStoneGroup.ValueSeparator = ", "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 127)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Metel"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCategory
        '
        Me.chkCmbCategory.CheckOnClick = True
        Me.chkCmbCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCategory.DropDownHeight = 1
        Me.chkCmbCategory.FormattingEnabled = True
        Me.chkCmbCategory.IntegralHeight = False
        Me.chkCmbCategory.Location = New System.Drawing.Point(90, 150)
        Me.chkCmbCategory.Name = "chkCmbCategory"
        Me.chkCmbCategory.Size = New System.Drawing.Size(251, 22)
        Me.chkCmbCategory.TabIndex = 13
        Me.chkCmbCategory.ValueSeparator = ", "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 155)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Category"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbGropBy
        '
        Me.chkCmbGropBy.CheckOnClick = True
        Me.chkCmbGropBy.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbGropBy.DropDownHeight = 1
        Me.chkCmbGropBy.FormattingEnabled = True
        Me.chkCmbGropBy.IntegralHeight = False
        Me.chkCmbGropBy.Location = New System.Drawing.Point(90, 38)
        Me.chkCmbGropBy.Name = "chkCmbGropBy"
        Me.chkCmbGropBy.Size = New System.Drawing.Size(251, 22)
        Me.chkCmbGropBy.TabIndex = 5
        Me.chkCmbGropBy.ValueSeparator = ", "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 43)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "GroupBy"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(90, 94)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(251, 22)
        Me.chkCmbCostCentre.TabIndex = 9
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(6, 99)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 8
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(90, 66)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(251, 22)
        Me.chkCmbCompany.TabIndex = 7
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 15)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(67, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Date From"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 71)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(62, 13)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "Company"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(208, 11)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpTo.Size = New System.Drawing.Size(86, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "27-12-2018"
        Me.dtpTo.Value = New Date(2018, 12, 27, 0, 0, 0, 0)
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(182, 15)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(20, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "To"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(90, 11)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpFrom.Size = New System.Drawing.Size(86, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "27-12-2018"
        Me.dtpFrom.Value = New Date(2018, 12, 27, 0, 0, 0, 0)
        '
        'frmStoneGroupReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(933, 450)
        Me.Controls.Add(Me.pnlMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmStoneGroupReport"
        Me.Text = "frmStoneGroupReport"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlMain.ResumeLayout(False)
        Me.pnlMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlMain As Panel
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As Label
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents Label6 As Label
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCmbGropBy As BrighttechPack.CheckedComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents chkCmbMetel As BrighttechPack.CheckedComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents chkCmbStoneGroup As BrighttechPack.CheckedComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents chkCmbCategory As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents chkCmbItem As BrighttechPack.CheckedComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents chkCmbSubItem As BrighttechPack.CheckedComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents btnNew As Button
    Friend WithEvents btnView_Search As Button
    Friend WithEvents btnExit As Button
End Class
