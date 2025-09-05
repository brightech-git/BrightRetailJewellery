<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AgeWiseOutstanding
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
        Me.GrpContainer = New System.Windows.Forms.GroupBox()
        Me.chkWithSubTotal = New System.Windows.Forms.CheckBox()
        Me.chkOnlyGivenRange = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rbtRunno = New System.Windows.Forms.RadioButton()
        Me.rbtName = New System.Windows.Forms.RadioButton()
        Me.txtDif = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtToDay1 = New System.Windows.Forms.TextBox()
        Me.txtToDay3 = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFromDay2 = New System.Windows.Forms.TextBox()
        Me.txtFromDay3 = New System.Windows.Forms.TextBox()
        Me.txtToDay4 = New System.Windows.Forms.TextBox()
        Me.txtFromDay4 = New System.Windows.Forms.TextBox()
        Me.txtFromDay1 = New System.Windows.Forms.TextBox()
        Me.txtToDay2 = New System.Windows.Forms.TextBox()
        Me.rbtDealerSmith = New System.Windows.Forms.RadioButton()
        Me.rbtCustomer = New System.Windows.Forms.RadioButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpAsONDate = New BrighttechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GrpContainer.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.chkWithSubTotal)
        Me.GrpContainer.Controls.Add(Me.chkOnlyGivenRange)
        Me.GrpContainer.Controls.Add(Me.Panel1)
        Me.GrpContainer.Controls.Add(Me.txtDif)
        Me.GrpContainer.Controls.Add(Me.Label9)
        Me.GrpContainer.Controls.Add(Me.txtToDay1)
        Me.GrpContainer.Controls.Add(Me.txtToDay3)
        Me.GrpContainer.Controls.Add(Me.Label10)
        Me.GrpContainer.Controls.Add(Me.Label2)
        Me.GrpContainer.Controls.Add(Me.txtFromDay2)
        Me.GrpContainer.Controls.Add(Me.txtFromDay3)
        Me.GrpContainer.Controls.Add(Me.txtToDay4)
        Me.GrpContainer.Controls.Add(Me.txtFromDay4)
        Me.GrpContainer.Controls.Add(Me.txtFromDay1)
        Me.GrpContainer.Controls.Add(Me.txtToDay2)
        Me.GrpContainer.Controls.Add(Me.rbtDealerSmith)
        Me.GrpContainer.Controls.Add(Me.rbtCustomer)
        Me.GrpContainer.Controls.Add(Me.Label3)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.dtpAsONDate)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Location = New System.Drawing.Point(201, 93)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(345, 347)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'chkWithSubTotal
        '
        Me.chkWithSubTotal.AutoSize = True
        Me.chkWithSubTotal.Location = New System.Drawing.Point(181, 274)
        Me.chkWithSubTotal.Name = "chkWithSubTotal"
        Me.chkWithSubTotal.Size = New System.Drawing.Size(104, 17)
        Me.chkWithSubTotal.TabIndex = 19
        Me.chkWithSubTotal.Text = "With SubTotal"
        Me.chkWithSubTotal.UseVisualStyleBackColor = True
        '
        'chkOnlyGivenRange
        '
        Me.chkOnlyGivenRange.AutoSize = True
        Me.chkOnlyGivenRange.Location = New System.Drawing.Point(28, 274)
        Me.chkOnlyGivenRange.Name = "chkOnlyGivenRange"
        Me.chkOnlyGivenRange.Size = New System.Drawing.Size(129, 17)
        Me.chkOnlyGivenRange.TabIndex = 18
        Me.chkOnlyGivenRange.Text = "Only Given Range"
        Me.chkOnlyGivenRange.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtRunno)
        Me.Panel1.Controls.Add(Me.rbtName)
        Me.Panel1.Location = New System.Drawing.Point(94, 240)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(232, 25)
        Me.Panel1.TabIndex = 17
        '
        'rbtRunno
        '
        Me.rbtRunno.AutoSize = True
        Me.rbtRunno.Location = New System.Drawing.Point(78, 4)
        Me.rbtRunno.Name = "rbtRunno"
        Me.rbtRunno.Size = New System.Drawing.Size(62, 17)
        Me.rbtRunno.TabIndex = 1
        Me.rbtRunno.TabStop = True
        Me.rbtRunno.Text = "RunNo"
        Me.rbtRunno.UseVisualStyleBackColor = True
        '
        'rbtName
        '
        Me.rbtName.AutoSize = True
        Me.rbtName.Location = New System.Drawing.Point(19, 4)
        Me.rbtName.Name = "rbtName"
        Me.rbtName.Size = New System.Drawing.Size(58, 17)
        Me.rbtName.TabIndex = 0
        Me.rbtName.TabStop = True
        Me.rbtName.Text = "Name"
        Me.rbtName.UseVisualStyleBackColor = True
        '
        'txtDif
        '
        Me.txtDif.Location = New System.Drawing.Point(115, 86)
        Me.txtDif.Name = "txtDif"
        Me.txtDif.Size = New System.Drawing.Size(79, 21)
        Me.txtDif.TabIndex = 5
        Me.txtDif.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(123, 111)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(63, 13)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "Day From"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtToDay1
        '
        Me.txtToDay1.Location = New System.Drawing.Point(198, 128)
        Me.txtToDay1.MaxLength = 3
        Me.txtToDay1.Name = "txtToDay1"
        Me.txtToDay1.Size = New System.Drawing.Size(79, 21)
        Me.txtToDay1.TabIndex = 9
        Me.txtToDay1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtToDay3
        '
        Me.txtToDay3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtToDay3.Location = New System.Drawing.Point(198, 184)
        Me.txtToDay3.MaxLength = 3
        Me.txtToDay3.Name = "txtToDay3"
        Me.txtToDay3.Size = New System.Drawing.Size(79, 21)
        Me.txtToDay3.TabIndex = 13
        Me.txtToDay3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(212, 111)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(47, 13)
        Me.Label10.TabIndex = 7
        Me.Label10.Text = "Day To"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(28, 90)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(27, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Diff"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFromDay2
        '
        Me.txtFromDay2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDay2.Location = New System.Drawing.Point(115, 156)
        Me.txtFromDay2.MaxLength = 3
        Me.txtFromDay2.Name = "txtFromDay2"
        Me.txtFromDay2.Size = New System.Drawing.Size(79, 21)
        Me.txtFromDay2.TabIndex = 10
        Me.txtFromDay2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFromDay3
        '
        Me.txtFromDay3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDay3.Location = New System.Drawing.Point(115, 184)
        Me.txtFromDay3.MaxLength = 3
        Me.txtFromDay3.Name = "txtFromDay3"
        Me.txtFromDay3.Size = New System.Drawing.Size(79, 21)
        Me.txtFromDay3.TabIndex = 12
        Me.txtFromDay3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtToDay4
        '
        Me.txtToDay4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtToDay4.Location = New System.Drawing.Point(198, 212)
        Me.txtToDay4.MaxLength = 3
        Me.txtToDay4.Name = "txtToDay4"
        Me.txtToDay4.Size = New System.Drawing.Size(79, 21)
        Me.txtToDay4.TabIndex = 15
        Me.txtToDay4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFromDay4
        '
        Me.txtFromDay4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDay4.Location = New System.Drawing.Point(115, 212)
        Me.txtFromDay4.MaxLength = 3
        Me.txtFromDay4.Name = "txtFromDay4"
        Me.txtFromDay4.Size = New System.Drawing.Size(79, 21)
        Me.txtFromDay4.TabIndex = 14
        Me.txtFromDay4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFromDay1
        '
        Me.txtFromDay1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFromDay1.Location = New System.Drawing.Point(115, 128)
        Me.txtFromDay1.MaxLength = 3
        Me.txtFromDay1.Name = "txtFromDay1"
        Me.txtFromDay1.Size = New System.Drawing.Size(79, 21)
        Me.txtFromDay1.TabIndex = 8
        Me.txtFromDay1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtToDay2
        '
        Me.txtToDay2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtToDay2.Location = New System.Drawing.Point(198, 156)
        Me.txtToDay2.MaxLength = 3
        Me.txtToDay2.Name = "txtToDay2"
        Me.txtToDay2.Size = New System.Drawing.Size(79, 21)
        Me.txtToDay2.TabIndex = 11
        Me.txtToDay2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'rbtDealerSmith
        '
        Me.rbtDealerSmith.AutoSize = True
        Me.rbtDealerSmith.Location = New System.Drawing.Point(198, 59)
        Me.rbtDealerSmith.Name = "rbtDealerSmith"
        Me.rbtDealerSmith.Size = New System.Drawing.Size(112, 17)
        Me.rbtDealerSmith.TabIndex = 3
        Me.rbtDealerSmith.TabStop = True
        Me.rbtDealerSmith.Text = "Dealer && Smith"
        Me.rbtDealerSmith.UseVisualStyleBackColor = True
        '
        'rbtCustomer
        '
        Me.rbtCustomer.AutoSize = True
        Me.rbtCustomer.Location = New System.Drawing.Point(115, 59)
        Me.rbtCustomer.Name = "rbtCustomer"
        Me.rbtCustomer.Size = New System.Drawing.Size(81, 17)
        Me.rbtCustomer.TabIndex = 2
        Me.rbtCustomer.TabStop = True
        Me.rbtCustomer.Text = "Customer"
        Me.rbtCustomer.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(28, 246)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 13)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Order By"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(28, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "As On Date"
        '
        'dtpAsONDate
        '
        Me.dtpAsONDate.Location = New System.Drawing.Point(115, 27)
        Me.dtpAsONDate.Mask = "##-##-####"
        Me.dtpAsONDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsONDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsONDate.Name = "dtpAsONDate"
        Me.dtpAsONDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsONDate.Size = New System.Drawing.Size(82, 21)
        Me.dtpAsONDate.TabIndex = 1
        Me.dtpAsONDate.Text = "29/09/2010"
        Me.dtpAsONDate.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(227, 300)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 22
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(15, 300)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 20
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(121, 300)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 21
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'AgeWiseOutstanding
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(757, 533)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "AgeWiseOutstanding"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AgeWiseOutstanding"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents dtpAsONDate As BrighttechPack.DatePicker
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents rbtDealerSmith As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCustomer As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtDif As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtToDay1 As System.Windows.Forms.TextBox
    Friend WithEvents txtToDay3 As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtFromDay2 As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDay3 As System.Windows.Forms.TextBox
    Friend WithEvents txtToDay4 As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDay4 As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDay1 As System.Windows.Forms.TextBox
    Friend WithEvents txtToDay2 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtRunno As System.Windows.Forms.RadioButton
    Friend WithEvents rbtName As System.Windows.Forms.RadioButton
    Friend WithEvents chkOnlyGivenRange As CheckBox
    Friend WithEvents chkWithSubTotal As CheckBox
End Class
