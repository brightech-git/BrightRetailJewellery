<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBillPrintCustDesign
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtNo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpfromdate = New GiriDatePicker.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnView = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.tabBillLayout = New System.Windows.Forms.TabPage()
        Me.GridViewbillprint = New System.Windows.Forms.DataGridView()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.CmboBxCseries = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Lblstrtpos = New System.Windows.Forms.Label()
        Me.LblBtmPos = New System.Windows.Forms.Label()
        Me.txtbxCvalue = New System.Windows.Forms.TextBox()
        Me.lblspce = New System.Windows.Forms.Label()
        Me.CmboBxColNme = New System.Windows.Forms.ComboBox()
        Me.Txtstratpos_NUM = New System.Windows.Forms.TextBox()
        Me.TxtBtmPos_NUM = New System.Windows.Forms.TextBox()
        Me.txtlinespc_NUM = New System.Windows.Forms.TextBox()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.tabAdvance = New System.Windows.Forms.TabPage()
        Me.tabEstimation = New System.Windows.Forms.TabPage()
        Me.rbtTaxInvoiceM2_Est = New System.Windows.Forms.RadioButton()
        Me.tabTaxInvoice = New System.Windows.Forms.TabPage()
        Me.rbtTaxInvoiceM4 = New System.Windows.Forms.RadioButton()
        Me.rbtTaxInvoiceM3 = New System.Windows.Forms.RadioButton()
        Me.rbtTaxInvoiceM2 = New System.Windows.Forms.RadioButton()
        Me.rbtTaxInvoiceM1 = New System.Windows.Forms.RadioButton()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.tabBillLayout.SuspendLayout()
        CType(Me.GridViewbillprint, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.tabEstimation.SuspendLayout()
        Me.tabTaxInvoice.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(227, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(22, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "No"
        '
        'txtNo
        '
        Me.txtNo.Location = New System.Drawing.Point(255, 11)
        Me.txtNo.Name = "txtNo"
        Me.txtNo.Size = New System.Drawing.Size(105, 21)
        Me.txtNo.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(100, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date"
        '
        'dtpfromdate
        '
        Me.dtpfromdate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpfromdate.Location = New System.Drawing.Point(142, 11)
        Me.dtpfromdate.Mask = "##-##-####"
        Me.dtpfromdate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpfromdate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpfromdate.Name = "dtpfromdate"
        Me.dtpfromdate.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpfromdate.Size = New System.Drawing.Size(79, 21)
        Me.dtpfromdate.TabIndex = 1
        Me.dtpfromdate.Text = "08-11-1753"
        Me.dtpfromdate.Value = New Date(1753, 11, 8, 0, 0, 0, 0)
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(315, 2)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 2
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(209, 2)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 1
        Me.btnNew.Text = "New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView
        '
        Me.btnView.Location = New System.Drawing.Point(103, 2)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(100, 30)
        Me.btnView.TabIndex = 0
        Me.btnView.Text = "View"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnView)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 403)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(613, 35)
        Me.Panel1.TabIndex = 2
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.txtNo)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.dtpfromdate)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(613, 41)
        Me.Panel2.TabIndex = 0
        '
        'tabBillLayout
        '
        Me.tabBillLayout.Controls.Add(Me.GridViewbillprint)
        Me.tabBillLayout.Controls.Add(Me.Panel3)
        Me.tabBillLayout.Controls.Add(Me.Panel4)
        Me.tabBillLayout.Location = New System.Drawing.Point(4, 22)
        Me.tabBillLayout.Name = "tabBillLayout"
        Me.tabBillLayout.Padding = New System.Windows.Forms.Padding(3)
        Me.tabBillLayout.Size = New System.Drawing.Size(605, 336)
        Me.tabBillLayout.TabIndex = 3
        Me.tabBillLayout.Text = "BillLayout"
        Me.tabBillLayout.UseVisualStyleBackColor = True
        '
        'GridViewbillprint
        '
        Me.GridViewbillprint.AllowUserToAddRows = False
        Me.GridViewbillprint.AllowUserToDeleteRows = False
        Me.GridViewbillprint.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridViewbillprint.Location = New System.Drawing.Point(3, 81)
        Me.GridViewbillprint.Name = "GridViewbillprint"
        Me.GridViewbillprint.ReadOnly = True
        Me.GridViewbillprint.Size = New System.Drawing.Size(599, 217)
        Me.GridViewbillprint.TabIndex = 1
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Label5)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.CmboBxCseries)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.Lblstrtpos)
        Me.Panel3.Controls.Add(Me.LblBtmPos)
        Me.Panel3.Controls.Add(Me.txtbxCvalue)
        Me.Panel3.Controls.Add(Me.lblspce)
        Me.Panel3.Controls.Add(Me.CmboBxColNme)
        Me.Panel3.Controls.Add(Me.Txtstratpos_NUM)
        Me.Panel3.Controls.Add(Me.TxtBtmPos_NUM)
        Me.Panel3.Controls.Add(Me.txtlinespc_NUM)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(3, 3)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(599, 78)
        Me.Panel3.TabIndex = 0
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(225, 36)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(35, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Xpos"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(118, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(48, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Header"
        '
        'CmboBxCseries
        '
        Me.CmboBxCseries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmboBxCseries.FormattingEnabled = True
        Me.CmboBxCseries.Location = New System.Drawing.Point(8, 54)
        Me.CmboBxCseries.Name = "CmboBxCseries"
        Me.CmboBxCseries.Size = New System.Drawing.Size(105, 21)
        Me.CmboBxCseries.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 36)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Name"
        '
        'Lblstrtpos
        '
        Me.Lblstrtpos.AutoSize = True
        Me.Lblstrtpos.Location = New System.Drawing.Point(8, 11)
        Me.Lblstrtpos.Name = "Lblstrtpos"
        Me.Lblstrtpos.Size = New System.Drawing.Size(83, 13)
        Me.Lblstrtpos.TabIndex = 0
        Me.Lblstrtpos.Text = "Start Position"
        '
        'LblBtmPos
        '
        Me.LblBtmPos.AutoSize = True
        Me.LblBtmPos.Location = New System.Drawing.Point(172, 11)
        Me.LblBtmPos.Name = "LblBtmPos"
        Me.LblBtmPos.Size = New System.Drawing.Size(96, 13)
        Me.LblBtmPos.TabIndex = 2
        Me.LblBtmPos.Text = "Bottom Position"
        '
        'txtbxCvalue
        '
        Me.txtbxCvalue.Location = New System.Drawing.Point(225, 54)
        Me.txtbxCvalue.Name = "txtbxCvalue"
        Me.txtbxCvalue.Size = New System.Drawing.Size(67, 21)
        Me.txtbxCvalue.TabIndex = 13
        '
        'lblspce
        '
        Me.lblspce.AutoSize = True
        Me.lblspce.Location = New System.Drawing.Point(351, 11)
        Me.lblspce.Name = "lblspce"
        Me.lblspce.Size = New System.Drawing.Size(69, 13)
        Me.lblspce.TabIndex = 4
        Me.lblspce.Text = "Line Space"
        '
        'CmboBxColNme
        '
        Me.CmboBxColNme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmboBxColNme.FormattingEnabled = True
        Me.CmboBxColNme.Location = New System.Drawing.Point(118, 54)
        Me.CmboBxColNme.Name = "CmboBxColNme"
        Me.CmboBxColNme.Size = New System.Drawing.Size(101, 21)
        Me.CmboBxColNme.TabIndex = 11
        '
        'Txtstratpos_NUM
        '
        Me.Txtstratpos_NUM.Location = New System.Drawing.Point(95, 7)
        Me.Txtstratpos_NUM.Name = "Txtstratpos_NUM"
        Me.Txtstratpos_NUM.Size = New System.Drawing.Size(70, 21)
        Me.Txtstratpos_NUM.TabIndex = 1
        '
        'TxtBtmPos_NUM
        '
        Me.TxtBtmPos_NUM.Location = New System.Drawing.Point(276, 7)
        Me.TxtBtmPos_NUM.Name = "TxtBtmPos_NUM"
        Me.TxtBtmPos_NUM.Size = New System.Drawing.Size(70, 21)
        Me.TxtBtmPos_NUM.TabIndex = 3
        '
        'txtlinespc_NUM
        '
        Me.txtlinespc_NUM.Location = New System.Drawing.Point(423, 7)
        Me.txtlinespc_NUM.Name = "txtlinespc_NUM"
        Me.txtlinespc_NUM.Size = New System.Drawing.Size(70, 21)
        Me.txtlinespc_NUM.TabIndex = 5
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.btnSave)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel4.Location = New System.Drawing.Point(3, 298)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(599, 35)
        Me.Panel4.TabIndex = 2
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(214, 2)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'tabAdvance
        '
        Me.tabAdvance.Location = New System.Drawing.Point(4, 22)
        Me.tabAdvance.Name = "tabAdvance"
        Me.tabAdvance.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAdvance.Size = New System.Drawing.Size(605, 336)
        Me.tabAdvance.TabIndex = 1
        Me.tabAdvance.Text = "Advance/Order"
        Me.tabAdvance.UseVisualStyleBackColor = True
        '
        'tabEstimation
        '
        Me.tabEstimation.Controls.Add(Me.rbtTaxInvoiceM2_Est)
        Me.tabEstimation.Location = New System.Drawing.Point(4, 22)
        Me.tabEstimation.Name = "tabEstimation"
        Me.tabEstimation.Size = New System.Drawing.Size(605, 336)
        Me.tabEstimation.TabIndex = 2
        Me.tabEstimation.Text = "Estimation"
        Me.tabEstimation.UseVisualStyleBackColor = True
        '
        'rbtTaxInvoiceM2_Est
        '
        Me.rbtTaxInvoiceM2_Est.AutoSize = True
        Me.rbtTaxInvoiceM2_Est.Checked = True
        Me.rbtTaxInvoiceM2_Est.Location = New System.Drawing.Point(46, 35)
        Me.rbtTaxInvoiceM2_Est.Name = "rbtTaxInvoiceM2_Est"
        Me.rbtTaxInvoiceM2_Est.Size = New System.Drawing.Size(162, 17)
        Me.rbtTaxInvoiceM2_Est.TabIndex = 2
        Me.rbtTaxInvoiceM2_Est.TabStop = True
        Me.rbtTaxInvoiceM2_Est.Text = "TaxInvoice A5 Half (M2)"
        Me.rbtTaxInvoiceM2_Est.UseVisualStyleBackColor = True
        '
        'tabTaxInvoice
        '
        Me.tabTaxInvoice.Controls.Add(Me.rbtTaxInvoiceM4)
        Me.tabTaxInvoice.Controls.Add(Me.rbtTaxInvoiceM3)
        Me.tabTaxInvoice.Controls.Add(Me.rbtTaxInvoiceM2)
        Me.tabTaxInvoice.Controls.Add(Me.rbtTaxInvoiceM1)
        Me.tabTaxInvoice.Location = New System.Drawing.Point(4, 22)
        Me.tabTaxInvoice.Name = "tabTaxInvoice"
        Me.tabTaxInvoice.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTaxInvoice.Size = New System.Drawing.Size(605, 336)
        Me.tabTaxInvoice.TabIndex = 0
        Me.tabTaxInvoice.Text = "TaxInvoice"
        Me.tabTaxInvoice.UseVisualStyleBackColor = True
        '
        'rbtTaxInvoiceM4
        '
        Me.rbtTaxInvoiceM4.AutoSize = True
        Me.rbtTaxInvoiceM4.Checked = True
        Me.rbtTaxInvoiceM4.Location = New System.Drawing.Point(194, 59)
        Me.rbtTaxInvoiceM4.Name = "rbtTaxInvoiceM4"
        Me.rbtTaxInvoiceM4.Size = New System.Drawing.Size(159, 17)
        Me.rbtTaxInvoiceM4.TabIndex = 3
        Me.rbtTaxInvoiceM4.TabStop = True
        Me.rbtTaxInvoiceM4.Text = "TaxInvoice B5 Full (M4)"
        Me.rbtTaxInvoiceM4.UseVisualStyleBackColor = True
        '
        'rbtTaxInvoiceM3
        '
        Me.rbtTaxInvoiceM3.AutoSize = True
        Me.rbtTaxInvoiceM3.Checked = True
        Me.rbtTaxInvoiceM3.Location = New System.Drawing.Point(27, 59)
        Me.rbtTaxInvoiceM3.Name = "rbtTaxInvoiceM3"
        Me.rbtTaxInvoiceM3.Size = New System.Drawing.Size(162, 17)
        Me.rbtTaxInvoiceM3.TabIndex = 2
        Me.rbtTaxInvoiceM3.TabStop = True
        Me.rbtTaxInvoiceM3.Text = "TaxInvoice A5 Half (M3)"
        Me.rbtTaxInvoiceM3.UseVisualStyleBackColor = True
        '
        'rbtTaxInvoiceM2
        '
        Me.rbtTaxInvoiceM2.AutoSize = True
        Me.rbtTaxInvoiceM2.Checked = True
        Me.rbtTaxInvoiceM2.Location = New System.Drawing.Point(195, 36)
        Me.rbtTaxInvoiceM2.Name = "rbtTaxInvoiceM2"
        Me.rbtTaxInvoiceM2.Size = New System.Drawing.Size(162, 17)
        Me.rbtTaxInvoiceM2.TabIndex = 1
        Me.rbtTaxInvoiceM2.TabStop = True
        Me.rbtTaxInvoiceM2.Text = "TaxInvoice B5 Half (M2)"
        Me.rbtTaxInvoiceM2.UseVisualStyleBackColor = True
        '
        'rbtTaxInvoiceM1
        '
        Me.rbtTaxInvoiceM1.AutoSize = True
        Me.rbtTaxInvoiceM1.Location = New System.Drawing.Point(30, 36)
        Me.rbtTaxInvoiceM1.Name = "rbtTaxInvoiceM1"
        Me.rbtTaxInvoiceM1.Size = New System.Drawing.Size(159, 17)
        Me.rbtTaxInvoiceM1.TabIndex = 0
        Me.rbtTaxInvoiceM1.Text = "TaxInvoice A4 Full (M1)"
        Me.rbtTaxInvoiceM1.UseVisualStyleBackColor = True
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabTaxInvoice)
        Me.tabMain.Controls.Add(Me.tabEstimation)
        Me.tabMain.Controls.Add(Me.tabAdvance)
        Me.tabMain.Controls.Add(Me.tabBillLayout)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 41)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(613, 362)
        Me.tabMain.TabIndex = 1
        '
        'frmBillPrintCustDesign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(613, 438)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmBillPrintCustDesign"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BILL PRINT DESIGN"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.tabBillLayout.ResumeLayout(False)
        CType(Me.GridViewbillprint, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.tabEstimation.ResumeLayout(False)
        Me.tabEstimation.PerformLayout()
        Me.tabTaxInvoice.ResumeLayout(False)
        Me.tabTaxInvoice.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dtpfromdate As GiriDatePicker.DatePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents btnExit As Button
    Friend WithEvents btnNew As Button
    Friend WithEvents btnView As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents txtNo As TextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents tabBillLayout As TabPage
    Friend WithEvents txtlinespc_NUM As TextBox
    Friend WithEvents TxtBtmPos_NUM As TextBox
    Friend WithEvents Txtstratpos_NUM As TextBox
    Friend WithEvents lblspce As Label
    Friend WithEvents LblBtmPos As Label
    Friend WithEvents Lblstrtpos As Label
    Friend WithEvents tabAdvance As TabPage
    Friend WithEvents tabEstimation As TabPage
    Friend WithEvents rbtTaxInvoiceM2_Est As RadioButton
    Friend WithEvents tabTaxInvoice As TabPage
    Friend WithEvents rbtTaxInvoiceM4 As RadioButton
    Friend WithEvents rbtTaxInvoiceM3 As RadioButton
    Friend WithEvents rbtTaxInvoiceM2 As RadioButton
    Friend WithEvents rbtTaxInvoiceM1 As RadioButton
    Friend WithEvents tabMain As TabControl
    Friend WithEvents txtbxCvalue As TextBox
    Friend WithEvents CmboBxColNme As ComboBox
    Friend WithEvents GridViewbillprint As DataGridView
    Friend WithEvents btnSave As Button
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents CmboBxCseries As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
End Class
