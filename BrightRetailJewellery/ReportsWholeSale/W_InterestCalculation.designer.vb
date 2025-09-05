<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class W_InterestCalculation
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
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.gridViewHeader = New System.Windows.Forms.DataGridView
        Me.lblTitle = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label8 = New System.Windows.Forms.Label
        Me.cmbPartyName = New System.Windows.Forms.ComboBox
        Me.chkIssue = New System.Windows.Forms.CheckBox
        Me.chkReceipt = New System.Windows.Forms.CheckBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtTotDays = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtGraceDays = New System.Windows.Forms.TextBox
        Me.txtIntWt = New System.Windows.Forms.TextBox
        Me.txtIntRs = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.dtpCalcDate = New BrighttechPack.DatePicker(Me.components)
        Me.optAmt = New System.Windows.Forms.RadioButton
        Me.optPer = New System.Windows.Forms.RadioButton
        Me.btnExit = New System.Windows.Forms.Button
        Me.gridView = New System.Windows.Forms.DataGridView
        CType(Me.gridViewHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnExport
        '
        Me.btnExport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExport.Location = New System.Drawing.Point(627, 58)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(117, 30)
        Me.btnExport.TabIndex = 13
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(753, 58)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(117, 30)
        Me.btnPrint.TabIndex = 14
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(520, 58)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(105, 30)
        Me.btnView_Search.TabIndex = 12
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(155, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "To"
        '
        'gridViewHeader
        '
        Me.gridViewHeader.AllowUserToAddRows = False
        Me.gridViewHeader.AllowUserToDeleteRows = False
        Me.gridViewHeader.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridViewHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHeader.Enabled = False
        Me.gridViewHeader.Location = New System.Drawing.Point(0, 134)
        Me.gridViewHeader.Name = "gridViewHeader"
        Me.gridViewHeader.ReadOnly = True
        Me.gridViewHeader.RowHeadersVisible = False
        Me.gridViewHeader.Size = New System.Drawing.Size(1028, 19)
        Me.gridViewHeader.TabIndex = 19
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 99)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1028, 35)
        Me.lblTitle.TabIndex = 18
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Date From"
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(75, 27)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(68, 20)
        Me.dtpFrom.TabIndex = 4
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(181, 27)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(66, 20)
        Me.dtpTo.TabIndex = 5
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.cmbPartyName)
        Me.Panel1.Controls.Add(Me.chkIssue)
        Me.Panel1.Controls.Add(Me.chkReceipt)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.txtTotDays)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.txtGraceDays)
        Me.Panel1.Controls.Add(Me.txtIntWt)
        Me.Panel1.Controls.Add(Me.txtIntRs)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.dtpCalcDate)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.optAmt)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.optPer)
        Me.Panel1.Controls.Add(Me.btnView_Search)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1028, 99)
        Me.Panel1.TabIndex = 0
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(387, 30)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(35, 13)
        Me.Label8.TabIndex = 26
        Me.Label8.Text = "Name"
        '
        'cmbPartyName
        '
        Me.cmbPartyName.FormattingEnabled = True
        Me.cmbPartyName.Location = New System.Drawing.Point(433, 27)
        Me.cmbPartyName.Name = "cmbPartyName"
        Me.cmbPartyName.Size = New System.Drawing.Size(287, 21)
        Me.cmbPartyName.TabIndex = 7
        '
        'chkIssue
        '
        Me.chkIssue.AutoSize = True
        Me.chkIssue.Checked = True
        Me.chkIssue.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIssue.Location = New System.Drawing.Point(315, 8)
        Me.chkIssue.Name = "chkIssue"
        Me.chkIssue.Size = New System.Drawing.Size(51, 17)
        Me.chkIssue.TabIndex = 3
        Me.chkIssue.Text = "Issue"
        Me.chkIssue.UseVisualStyleBackColor = True
        '
        'chkReceipt
        '
        Me.chkReceipt.AutoSize = True
        Me.chkReceipt.Checked = True
        Me.chkReceipt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkReceipt.Location = New System.Drawing.Point(183, 8)
        Me.chkReceipt.Name = "chkReceipt"
        Me.chkReceipt.Size = New System.Drawing.Size(63, 17)
        Me.chkReceipt.TabIndex = 2
        Me.chkReceipt.Text = "Receipt"
        Me.chkReceipt.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(380, 60)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(48, 13)
        Me.Label7.TabIndex = 22
        Me.Label7.Text = "360/365"
        '
        'txtTotDays
        '
        Me.txtTotDays.Location = New System.Drawing.Point(433, 57)
        Me.txtTotDays.Name = "txtTotDays"
        Me.txtTotDays.Size = New System.Drawing.Size(64, 20)
        Me.txtTotDays.TabIndex = 11
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(256, 60)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(45, 13)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "Gr.Days"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(144, 60)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 13)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "Int Wt"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(34, 60)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(35, 13)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "Int Rs"
        '
        'txtGraceDays
        '
        Me.txtGraceDays.Location = New System.Drawing.Point(314, 57)
        Me.txtGraceDays.Name = "txtGraceDays"
        Me.txtGraceDays.Size = New System.Drawing.Size(64, 20)
        Me.txtGraceDays.TabIndex = 10
        '
        'txtIntWt
        '
        Me.txtIntWt.Location = New System.Drawing.Point(183, 57)
        Me.txtIntWt.Name = "txtIntWt"
        Me.txtIntWt.Size = New System.Drawing.Size(64, 20)
        Me.txtIntWt.TabIndex = 9
        '
        'txtIntRs
        '
        Me.txtIntRs.Location = New System.Drawing.Point(75, 57)
        Me.txtIntRs.Name = "txtIntRs"
        Me.txtIntRs.Size = New System.Drawing.Size(66, 20)
        Me.txtIntRs.TabIndex = 8
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(249, 30)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(54, 13)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Calc Date"
        '
        'dtpCalcDate
        '
        Me.dtpCalcDate.Location = New System.Drawing.Point(312, 27)
        Me.dtpCalcDate.Mask = "##/##/####"
        Me.dtpCalcDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpCalcDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpCalcDate.Name = "dtpCalcDate"
        Me.dtpCalcDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpCalcDate.Size = New System.Drawing.Size(65, 20)
        Me.dtpCalcDate.TabIndex = 6
        Me.dtpCalcDate.Text = "07/03/9998"
        Me.dtpCalcDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'optAmt
        '
        Me.optAmt.AutoSize = True
        Me.optAmt.Location = New System.Drawing.Point(77, 9)
        Me.optAmt.Name = "optAmt"
        Me.optAmt.Size = New System.Drawing.Size(43, 17)
        Me.optAmt.TabIndex = 1
        Me.optAmt.Text = "Amt"
        Me.optAmt.UseVisualStyleBackColor = True
        '
        'optPer
        '
        Me.optPer.AutoSize = True
        Me.optPer.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.optPer.Checked = True
        Me.optPer.Cursor = System.Windows.Forms.Cursors.Default
        Me.optPer.Location = New System.Drawing.Point(20, 8)
        Me.optPer.Name = "optPer"
        Me.optPer.Size = New System.Drawing.Size(41, 17)
        Me.optPer.TabIndex = 0
        Me.optPer.TabStop = True
        Me.optPer.Text = "Per"
        Me.optPer.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(872, 58)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(105, 30)
        Me.btnExit.TabIndex = 15
        Me.btnExit.Text = "Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 153)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(1028, 351)
        Me.gridView.TabIndex = 16
        '
        'W_InterestCalculation
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1028, 504)
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.gridViewHeader)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "W_InterestCalculation"
        Me.Text = "W_Interest Calculation"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridViewHeader, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents gridViewHeader As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents optAmt As System.Windows.Forms.RadioButton
    Friend WithEvents optPer As System.Windows.Forms.RadioButton
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtGraceDays As System.Windows.Forms.TextBox
    Friend WithEvents txtIntWt As System.Windows.Forms.TextBox
    Friend WithEvents txtIntRs As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtpCalcDate As BrighttechPack.DatePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtTotDays As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkIssue As System.Windows.Forms.CheckBox
    Friend WithEvents chkReceipt As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbPartyName As System.Windows.Forms.ComboBox
End Class
