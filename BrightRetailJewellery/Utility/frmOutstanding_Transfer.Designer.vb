<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOutstanding_Transfer
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmbFromcostCentre = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpSRBillDate = New BrighttechPack.DatePicker(Me.components)
        Me.rbtSR = New System.Windows.Forms.RadioButton()
        Me.rbtAdvance = New System.Windows.Forms.RadioButton()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnTransfer = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.cmbTocostCentre = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtRunno_MAN = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.GridView = New System.Windows.Forms.DataGridView()
        Me.rbtGiftVoucher = New System.Windows.Forms.RadioButton()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.Panel1.Controls.Add(Me.rbtGiftVoucher)
        Me.Panel1.Controls.Add(Me.cmbFromcostCentre)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.dtpSRBillDate)
        Me.Panel1.Controls.Add(Me.rbtSR)
        Me.Panel1.Controls.Add(Me.rbtAdvance)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnTransfer)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.cmbTocostCentre)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.txtRunno_MAN)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(768, 133)
        Me.Panel1.TabIndex = 0
        '
        'cmbFromcostCentre
        '
        Me.cmbFromcostCentre.FormattingEnabled = True
        Me.cmbFromcostCentre.Location = New System.Drawing.Point(142, 48)
        Me.cmbFromcostCentre.Name = "cmbFromcostCentre"
        Me.cmbFromcostCentre.Size = New System.Drawing.Size(209, 21)
        Me.cmbFromcostCentre.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 52)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(109, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "From Cost Centre"
        '
        'dtpSRBillDate
        '
        Me.dtpSRBillDate.Location = New System.Drawing.Point(142, 102)
        Me.dtpSRBillDate.Mask = "##/##/####"
        Me.dtpSRBillDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpSRBillDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpSRBillDate.Name = "dtpSRBillDate"
        Me.dtpSRBillDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpSRBillDate.Size = New System.Drawing.Size(93, 21)
        Me.dtpSRBillDate.TabIndex = 8
        Me.dtpSRBillDate.Text = "07/03/9998"
        Me.dtpSRBillDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'rbtSR
        '
        Me.rbtSR.AutoSize = True
        Me.rbtSR.Location = New System.Drawing.Point(253, 21)
        Me.rbtSR.Name = "rbtSR"
        Me.rbtSR.Size = New System.Drawing.Size(98, 17)
        Me.rbtSR.TabIndex = 2
        Me.rbtSR.Text = "Sales Return"
        Me.rbtSR.UseVisualStyleBackColor = True
        '
        'rbtAdvance
        '
        Me.rbtAdvance.AutoSize = True
        Me.rbtAdvance.Checked = True
        Me.rbtAdvance.Location = New System.Drawing.Point(20, 21)
        Me.rbtAdvance.Name = "rbtAdvance"
        Me.rbtAdvance.Size = New System.Drawing.Size(138, 17)
        Me.rbtAdvance.TabIndex = 0
        Me.rbtAdvance.Text = "Advance and Credit"
        Me.rbtAdvance.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(356, 97)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(90, 30)
        Me.btnSearch.TabIndex = 10
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(538, 97)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(90, 30)
        Me.btnNew.TabIndex = 12
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnTransfer
        '
        Me.btnTransfer.Enabled = False
        Me.btnTransfer.Location = New System.Drawing.Point(447, 97)
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.Size = New System.Drawing.Size(90, 30)
        Me.btnTransfer.TabIndex = 11
        Me.btnTransfer.Text = "Transfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(629, 97)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(90, 30)
        Me.btnExit.TabIndex = 13
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'cmbTocostCentre
        '
        Me.cmbTocostCentre.FormattingEnabled = True
        Me.cmbTocostCentre.Location = New System.Drawing.Point(142, 75)
        Me.cmbTocostCentre.Name = "cmbTocostCentre"
        Me.cmbTocostCentre.Size = New System.Drawing.Size(209, 21)
        Me.cmbTocostCentre.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(20, 79)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "To Cost Centre"
        '
        'txtRunno_MAN
        '
        Me.txtRunno_MAN.Location = New System.Drawing.Point(241, 102)
        Me.txtRunno_MAN.Name = "txtRunno_MAN"
        Me.txtRunno_MAN.Size = New System.Drawing.Size(110, 21)
        Me.txtRunno_MAN.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 106)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Ref No"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.GridView)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 133)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(768, 314)
        Me.Panel2.TabIndex = 1
        '
        'GridView
        '
        Me.GridView.AllowUserToAddRows = False
        Me.GridView.AllowUserToDeleteRows = False
        Me.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridView.Location = New System.Drawing.Point(0, 0)
        Me.GridView.Name = "GridView"
        Me.GridView.ReadOnly = True
        Me.GridView.Size = New System.Drawing.Size(768, 314)
        Me.GridView.TabIndex = 0
        '
        'rbtGiftVoucher
        '
        Me.rbtGiftVoucher.AutoSize = True
        Me.rbtGiftVoucher.Location = New System.Drawing.Point(158, 21)
        Me.rbtGiftVoucher.Name = "rbtGiftVoucher"
        Me.rbtGiftVoucher.Size = New System.Drawing.Size(95, 17)
        Me.rbtGiftVoucher.TabIndex = 1
        Me.rbtGiftVoucher.Text = "Gift Voucher"
        Me.rbtGiftVoucher.UseVisualStyleBackColor = True
        '
        'frmOutstanding_Transfer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Silver
        Me.ClientSize = New System.Drawing.Size(768, 447)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmOutstanding_Transfer"
        Me.Text = "Outstanding Transfer"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtRunno_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents GridView As System.Windows.Forms.DataGridView
    Friend WithEvents cmbTocostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnTransfer As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents rbtSR As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAdvance As System.Windows.Forms.RadioButton
    Friend WithEvents dtpSRBillDate As BrighttechPack.DatePicker
    Friend WithEvents cmbFromcostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents rbtGiftVoucher As RadioButton
End Class
