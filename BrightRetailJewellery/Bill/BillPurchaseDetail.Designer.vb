<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BillPurchaseDetail
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
        Me.grpPurhcase = New CodeVendor.Controls.Grouper
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.rbtOthers = New System.Windows.Forms.RadioButton
        Me.rbtOwn = New System.Windows.Forms.RadioButton
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbtPurch = New System.Windows.Forms.RadioButton
        Me.rbtCash = New System.Windows.Forms.RadioButton
        Me.Label5 = New System.Windows.Forms.Label
        Me.cmbItemType_MAN = New System.Windows.Forms.ComboBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbSubItem_Own = New System.Windows.Forms.ComboBox
        Me.cmbItem = New System.Windows.Forms.ComboBox
        Me.cmbCategory_MAN = New System.Windows.Forms.ComboBox
        Me.grpPurhcase.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpPurhcase
        '
        Me.grpPurhcase.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpPurhcase.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpPurhcase.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpPurhcase.BorderColor = System.Drawing.Color.Transparent
        Me.grpPurhcase.BorderThickness = 1.0!
        Me.grpPurhcase.Controls.Add(Me.GroupBox2)
        Me.grpPurhcase.Controls.Add(Me.GroupBox1)
        Me.grpPurhcase.Controls.Add(Me.Label5)
        Me.grpPurhcase.Controls.Add(Me.cmbItemType_MAN)
        Me.grpPurhcase.Controls.Add(Me.txtDescription)
        Me.grpPurhcase.Controls.Add(Me.Label4)
        Me.grpPurhcase.Controls.Add(Me.Label3)
        Me.grpPurhcase.Controls.Add(Me.Label2)
        Me.grpPurhcase.Controls.Add(Me.Label1)
        Me.grpPurhcase.Controls.Add(Me.cmbSubItem_Own)
        Me.grpPurhcase.Controls.Add(Me.cmbItem)
        Me.grpPurhcase.Controls.Add(Me.cmbCategory_MAN)
        Me.grpPurhcase.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpPurhcase.GroupImage = Nothing
        Me.grpPurhcase.GroupTitle = ""
        Me.grpPurhcase.Location = New System.Drawing.Point(4, -5)
        Me.grpPurhcase.Name = "grpPurhcase"
        Me.grpPurhcase.Padding = New System.Windows.Forms.Padding(20)
        Me.grpPurhcase.PaintGroupBox = False
        Me.grpPurhcase.RoundCorners = 10
        Me.grpPurhcase.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpPurhcase.ShadowControl = False
        Me.grpPurhcase.ShadowThickness = 3
        Me.grpPurhcase.Size = New System.Drawing.Size(426, 183)
        Me.grpPurhcase.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbtOthers)
        Me.GroupBox2.Controls.Add(Me.rbtOwn)
        Me.GroupBox2.Location = New System.Drawing.Point(78, 124)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(159, 26)
        Me.GroupBox2.TabIndex = 8
        Me.GroupBox2.TabStop = False
        '
        'rbtOthers
        '
        Me.rbtOthers.AutoSize = True
        Me.rbtOthers.Location = New System.Drawing.Point(83, 9)
        Me.rbtOthers.Name = "rbtOthers"
        Me.rbtOthers.Size = New System.Drawing.Size(63, 17)
        Me.rbtOthers.TabIndex = 1
        Me.rbtOthers.Text = "Others"
        Me.rbtOthers.UseVisualStyleBackColor = True
        '
        'rbtOwn
        '
        Me.rbtOwn.AutoSize = True
        Me.rbtOwn.Checked = True
        Me.rbtOwn.Location = New System.Drawing.Point(15, 9)
        Me.rbtOwn.Name = "rbtOwn"
        Me.rbtOwn.Size = New System.Drawing.Size(50, 17)
        Me.rbtOwn.TabIndex = 0
        Me.rbtOwn.TabStop = True
        Me.rbtOwn.Text = "Own"
        Me.rbtOwn.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtPurch)
        Me.GroupBox1.Controls.Add(Me.rbtCash)
        Me.GroupBox1.Location = New System.Drawing.Point(243, 124)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(176, 26)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        '
        'rbtPurch
        '
        Me.rbtPurch.AutoSize = True
        Me.rbtPurch.Checked = True
        Me.rbtPurch.Location = New System.Drawing.Point(96, 9)
        Me.rbtPurch.Name = "rbtPurch"
        Me.rbtPurch.Size = New System.Drawing.Size(80, 17)
        Me.rbtPurch.TabIndex = 1
        Me.rbtPurch.TabStop = True
        Me.rbtPurch.Text = "Exchange"
        Me.rbtPurch.UseVisualStyleBackColor = True
        '
        'rbtCash
        '
        Me.rbtCash.AutoSize = True
        Me.rbtCash.Location = New System.Drawing.Point(6, 9)
        Me.rbtCash.Name = "rbtCash"
        Me.rbtCash.Size = New System.Drawing.Size(90, 17)
        Me.rbtCash.TabIndex = 0
        Me.rbtCash.Text = "Cash Purch"
        Me.rbtCash.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(8, 101)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(66, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Item Type"
        '
        'cmbItemType_MAN
        '
        Me.cmbItemType_MAN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbItemType_MAN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbItemType_MAN.FormattingEnabled = True
        Me.cmbItemType_MAN.Location = New System.Drawing.Point(74, 97)
        Me.cmbItemType_MAN.Name = "cmbItemType_MAN"
        Me.cmbItemType_MAN.Size = New System.Drawing.Size(211, 21)
        Me.cmbItemType_MAN.TabIndex = 7
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(74, 154)
        Me.txtDescription.MaxLength = 50
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(345, 21)
        Me.txtDescription.TabIndex = 11
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 157)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Description"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 74)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Sub Item"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Item"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Category"
        '
        'cmbSubItem_Own
        '
        Me.cmbSubItem_Own.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbSubItem_Own.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbSubItem_Own.FormattingEnabled = True
        Me.cmbSubItem_Own.Location = New System.Drawing.Point(74, 70)
        Me.cmbSubItem_Own.Name = "cmbSubItem_Own"
        Me.cmbSubItem_Own.Size = New System.Drawing.Size(211, 21)
        Me.cmbSubItem_Own.TabIndex = 5
        '
        'cmbItem
        '
        Me.cmbItem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbItem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbItem.FormattingEnabled = True
        Me.cmbItem.Location = New System.Drawing.Point(74, 43)
        Me.cmbItem.Name = "cmbItem"
        Me.cmbItem.Size = New System.Drawing.Size(211, 21)
        Me.cmbItem.TabIndex = 3
        '
        'cmbCategory_MAN
        '
        Me.cmbCategory_MAN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbCategory_MAN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbCategory_MAN.FormattingEnabled = True
        Me.cmbCategory_MAN.Location = New System.Drawing.Point(74, 16)
        Me.cmbCategory_MAN.Name = "cmbCategory_MAN"
        Me.cmbCategory_MAN.Size = New System.Drawing.Size(345, 21)
        Me.cmbCategory_MAN.TabIndex = 1
        '
        'BillPurchaseDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(435, 184)
        Me.Controls.Add(Me.grpPurhcase)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "BillPurchaseDetail"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BillPurchaseDetail"
        Me.grpPurhcase.ResumeLayout(False)
        Me.grpPurhcase.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpPurhcase As CodeVendor.Controls.Grouper
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbSubItem_Own As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItem As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCategory_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbItemType_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtPurch As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCash As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtOthers As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOwn As System.Windows.Forms.RadioButton
End Class
