<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOpeningTrailBalOutStDt
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
        Me.txtTranNo_NUM_MAN = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtRefNo_NUM_MAN = New System.Windows.Forms.TextBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbMode = New System.Windows.Forms.ComboBox
        Me.txtRunno = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtRemark = New System.Windows.Forms.TextBox
        Me.txtAmount_AMT_MAN = New System.Windows.Forms.TextBox
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.Label5 = New System.Windows.Forms.Label
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblBalanceAmt = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.dtpTranDate = New BrighttechPack.DatePicker(Me.components)
        Me.GroupBox1.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Tran No"
        '
        'txtTranNo_NUM_MAN
        '
        Me.txtTranNo_NUM_MAN.Location = New System.Drawing.Point(3, 28)
        Me.txtTranNo_NUM_MAN.Name = "txtTranNo_NUM_MAN"
        Me.txtTranNo_NUM_MAN.Size = New System.Drawing.Size(94, 21)
        Me.txtTranNo_NUM_MAN.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(113, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Tran Date"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(307, 12)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "RefNo"
        '
        'txtRefNo_NUM_MAN
        '
        Me.txtRefNo_NUM_MAN.Location = New System.Drawing.Point(288, 28)
        Me.txtRefNo_NUM_MAN.Name = "txtRefNo_NUM_MAN"
        Me.txtRefNo_NUM_MAN.Size = New System.Drawing.Size(78, 21)
        Me.txtRefNo_NUM_MAN.TabIndex = 7
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.dtpTranDate)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.txtRefNo_NUM_MAN)
        Me.GroupBox1.Controls.Add(Me.cmbMode)
        Me.GroupBox1.Controls.Add(Me.txtRunno)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.txtRemark)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtAmount_AMT_MAN)
        Me.GroupBox1.Controls.Add(Me.gridView)
        Me.GroupBox1.Controls.Add(Me.txtTranNo_NUM_MAN)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(7, -3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(769, 185)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(495, 12)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(51, 13)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "Amount"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(639, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Remark"
        '
        'cmbMode
        '
        Me.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMode.FormattingEnabled = True
        Me.cmbMode.Location = New System.Drawing.Point(193, 28)
        Me.cmbMode.Name = "cmbMode"
        Me.cmbMode.Size = New System.Drawing.Size(94, 21)
        Me.cmbMode.TabIndex = 5
        '
        'txtRunno
        '
        Me.txtRunno.Location = New System.Drawing.Point(367, 28)
        Me.txtRunno.Name = "txtRunno"
        Me.txtRunno.Size = New System.Drawing.Size(90, 21)
        Me.txtRunno.TabIndex = 9
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(390, 12)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(44, 13)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "RunNo"
        '
        'txtRemark
        '
        Me.txtRemark.Location = New System.Drawing.Point(584, 28)
        Me.txtRemark.MaxLength = 50
        Me.txtRemark.Name = "txtRemark"
        Me.txtRemark.Size = New System.Drawing.Size(162, 21)
        Me.txtRemark.TabIndex = 13
        '
        'txtAmount_AMT_MAN
        '
        Me.txtAmount_AMT_MAN.Location = New System.Drawing.Point(458, 28)
        Me.txtAmount_AMT_MAN.MaxLength = 50
        Me.txtAmount_AMT_MAN.Name = "txtAmount_AMT_MAN"
        Me.txtAmount_AMT_MAN.Size = New System.Drawing.Size(125, 21)
        Me.txtAmount_AMT_MAN.TabIndex = 11
        Me.txtAmount_AMT_MAN.Text = "123456789.00"
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(3, 50)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(761, 130)
        Me.gridView.TabIndex = 14
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(211, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Ref Type"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(591, 186)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(87, 27)
        Me.btnOk.TabIndex = 1
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(684, 186)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(87, 27)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "E&xit"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblBalanceAmt
        '
        Me.lblBalanceAmt.AutoSize = True
        Me.lblBalanceAmt.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBalanceAmt.ForeColor = System.Drawing.Color.Red
        Me.lblBalanceAmt.Location = New System.Drawing.Point(86, 193)
        Me.lblBalanceAmt.Name = "lblBalanceAmt"
        Me.lblBalanceAmt.Size = New System.Drawing.Size(56, 16)
        Me.lblBalanceAmt.TabIndex = 2
        Me.lblBalanceAmt.Text = "Label8"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Red
        Me.Label8.Location = New System.Drawing.Point(12, 193)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(75, 16)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Balance :"
        '
        'dtpTranDate
        '
        Me.dtpTranDate.Location = New System.Drawing.Point(98, 28)
        Me.dtpTranDate.Mask = "##/##/####"
        Me.dtpTranDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTranDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTranDate.Name = "dtpTranDate"
        Me.dtpTranDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTranDate.Size = New System.Drawing.Size(94, 21)
        Me.dtpTranDate.TabIndex = 3
        Me.dtpTranDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'frmOpeningTrailBalOutStDt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(782, 218)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.lblBalanceAmt)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmOpeningTrailBalOutStDt"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Outstanding Detail"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtTranNo_NUM_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtRefNo_NUM_MAN As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtRemark As System.Windows.Forms.TextBox
    Friend WithEvents cmbMode As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtRunno As System.Windows.Forms.TextBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents txtAmount_AMT_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblBalanceAmt As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents dtpTranDate As BrighttechPack.DatePicker
End Class
