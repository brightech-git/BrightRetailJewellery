<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDupBill
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
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.cmbEntryType = New System.Windows.Forms.ComboBox
        Me.txtBillNo = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.dtpBillDate = New BrighttechPack.DatePicker(Me.components)
        Me.SuspendLayout()
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(134, 133)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 7
        Me.btnExit.Text = "Cancel"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(15, 133)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 6
        Me.btnPrint.Text = "Print "
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'cmbEntryType
        '
        Me.cmbEntryType.FormattingEnabled = True
        Me.cmbEntryType.Items.AddRange(New Object() {"SALES", "PURCHASE", "SALES & PURCHASE", "PAYMENTS", "RECEIPTS", "MISC ISSUE", "SALES RETURN", "APPROVAL ISSUE", "APPROVAL RECEIPT", "ORDER DELIVERY", "ORDER BOOKING", "REPAIR DELIVERY", "REPAIR BOOKING", "GIFT VOUCHER"})
        Me.cmbEntryType.Location = New System.Drawing.Point(102, 50)
        Me.cmbEntryType.Name = "cmbEntryType"
        Me.cmbEntryType.Size = New System.Drawing.Size(144, 21)
        Me.cmbEntryType.TabIndex = 3
        '
        'txtBillNo
        '
        Me.txtBillNo.Location = New System.Drawing.Point(102, 82)
        Me.txtBillNo.Name = "txtBillNo"
        Me.txtBillNo.Size = New System.Drawing.Size(103, 20)
        Me.txtBillNo.TabIndex = 5
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(12, 58)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(58, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "Entry Type"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 89)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Bill No"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 23)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(46, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Bill Date"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpBillDate
        '
        Me.dtpBillDate.Location = New System.Drawing.Point(102, 20)
        Me.dtpBillDate.Mask = "##/##/####"
        Me.dtpBillDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpBillDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpBillDate.Name = "dtpBillDate"
        Me.dtpBillDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpBillDate.Size = New System.Drawing.Size(103, 20)
        Me.dtpBillDate.TabIndex = 1
        Me.dtpBillDate.Text = "06/03/9998"
        Me.dtpBillDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'frmDupBill
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(246, 165)
        Me.ControlBox = False
        Me.Controls.Add(Me.dtpBillDate)
        Me.Controls.Add(Me.cmbEntryType)
        Me.Controls.Add(Me.txtBillNo)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnExit)
        Me.KeyPreview = True
        Me.Name = "frmDupBill"
        Me.Text = "Duplicate Bill"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents dtpBillDate As BrighttechPack.DatePicker
    Friend WithEvents cmbEntryType As System.Windows.Forms.ComboBox
    Friend WithEvents txtBillNo As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
End Class
