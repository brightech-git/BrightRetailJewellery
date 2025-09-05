<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WTransactionReport
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
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.optSummarry = New System.Windows.Forms.RadioButton
        Me.optDetailed = New System.Windows.Forms.RadioButton
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.chkMetalStcok = New System.Windows.Forms.CheckBox
        Me.chkAppPendings = New System.Windows.Forms.CheckBox
        Me.chkCanceledBill = New System.Windows.Forms.CheckBox
        Me.chkPartlySales = New System.Windows.Forms.CheckBox
        Me.chkTran = New System.Windows.Forms.CheckBox
        Me.lblTitle = New System.Windows.Forms.Label
        Me.gridViewHeader = New System.Windows.Forms.DataGridView
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.gridViewHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 150)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(1028, 606)
        Me.gridView.TabIndex = 9
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(274, 7)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(108, 21)
        Me.dtpTo.TabIndex = 1
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(117, 7)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(108, 21)
        Me.dtpFrom.TabIndex = 0
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Date From"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(241, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(21, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "To"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(469, 57)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(105, 30)
        Me.btnExit.TabIndex = 7
        Me.btnExit.Text = "Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(117, 57)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(105, 30)
        Me.btnView_Search.TabIndex = 4
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'optSummarry
        '
        Me.optSummarry.AutoSize = True
        Me.optSummarry.Checked = True
        Me.optSummarry.Location = New System.Drawing.Point(117, 34)
        Me.optSummarry.Name = "optSummarry"
        Me.optSummarry.Size = New System.Drawing.Size(86, 17)
        Me.optSummarry.TabIndex = 2
        Me.optSummarry.TabStop = True
        Me.optSummarry.Text = "Summarry"
        Me.optSummarry.UseVisualStyleBackColor = True
        '
        'optDetailed
        '
        Me.optDetailed.AutoSize = True
        Me.optDetailed.Location = New System.Drawing.Point(209, 34)
        Me.optDetailed.Name = "optDetailed"
        Me.optDetailed.Size = New System.Drawing.Size(72, 17)
        Me.optDetailed.TabIndex = 3
        Me.optDetailed.Text = "Detailed"
        Me.optDetailed.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExport.Location = New System.Drawing.Point(224, 57)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(117, 30)
        Me.btnExport.TabIndex = 5
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(350, 57)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(117, 30)
        Me.btnPrint.TabIndex = 6
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkMetalStcok)
        Me.Panel1.Controls.Add(Me.chkAppPendings)
        Me.Panel1.Controls.Add(Me.chkCanceledBill)
        Me.Panel1.Controls.Add(Me.chkPartlySales)
        Me.Panel1.Controls.Add(Me.chkTran)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.optDetailed)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.optSummarry)
        Me.Panel1.Controls.Add(Me.btnView_Search)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1028, 99)
        Me.Panel1.TabIndex = 13
        '
        'chkMetalStcok
        '
        Me.chkMetalStcok.AutoSize = True
        Me.chkMetalStcok.Checked = True
        Me.chkMetalStcok.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMetalStcok.Location = New System.Drawing.Point(780, 12)
        Me.chkMetalStcok.Name = "chkMetalStcok"
        Me.chkMetalStcok.Size = New System.Drawing.Size(92, 17)
        Me.chkMetalStcok.TabIndex = 17
        Me.chkMetalStcok.Text = "Metal Stock"
        Me.chkMetalStcok.UseVisualStyleBackColor = True
        '
        'chkAppPendings
        '
        Me.chkAppPendings.AutoSize = True
        Me.chkAppPendings.Checked = True
        Me.chkAppPendings.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAppPendings.Location = New System.Drawing.Point(566, 10)
        Me.chkAppPendings.Name = "chkAppPendings"
        Me.chkAppPendings.Size = New System.Drawing.Size(103, 17)
        Me.chkAppPendings.TabIndex = 16
        Me.chkAppPendings.Text = "App Pendings"
        Me.chkAppPendings.UseVisualStyleBackColor = True
        '
        'chkCanceledBill
        '
        Me.chkCanceledBill.AutoSize = True
        Me.chkCanceledBill.Checked = True
        Me.chkCanceledBill.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCanceledBill.Location = New System.Drawing.Point(674, 12)
        Me.chkCanceledBill.Name = "chkCanceledBill"
        Me.chkCanceledBill.Size = New System.Drawing.Size(100, 17)
        Me.chkCanceledBill.TabIndex = 15
        Me.chkCanceledBill.Text = "Canceled Bill"
        Me.chkCanceledBill.UseVisualStyleBackColor = True
        '
        'chkPartlySales
        '
        Me.chkPartlySales.AutoSize = True
        Me.chkPartlySales.Checked = True
        Me.chkPartlySales.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPartlySales.Location = New System.Drawing.Point(477, 11)
        Me.chkPartlySales.Name = "chkPartlySales"
        Me.chkPartlySales.Size = New System.Drawing.Size(87, 17)
        Me.chkPartlySales.TabIndex = 14
        Me.chkPartlySales.Text = "PartySales"
        Me.chkPartlySales.UseVisualStyleBackColor = True
        '
        'chkTran
        '
        Me.chkTran.AutoSize = True
        Me.chkTran.Checked = True
        Me.chkTran.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTran.Location = New System.Drawing.Point(388, 10)
        Me.chkTran.Name = "chkTran"
        Me.chkTran.Size = New System.Drawing.Size(92, 17)
        Me.chkTran.TabIndex = 13
        Me.chkTran.Text = "Transaction"
        Me.chkTran.UseVisualStyleBackColor = True
        Me.chkTran.Visible = False
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 99)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1028, 32)
        Me.lblTitle.TabIndex = 14
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gridViewHeader
        '
        Me.gridViewHeader.AllowUserToAddRows = False
        Me.gridViewHeader.AllowUserToDeleteRows = False
        Me.gridViewHeader.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridViewHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHeader.Enabled = False
        Me.gridViewHeader.Location = New System.Drawing.Point(0, 131)
        Me.gridViewHeader.Name = "gridViewHeader"
        Me.gridViewHeader.ReadOnly = True
        Me.gridViewHeader.RowHeadersVisible = False
        Me.gridViewHeader.Size = New System.Drawing.Size(1028, 19)
        Me.gridViewHeader.TabIndex = 15
        '
        'WTransactionReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 756)
        Me.ControlBox = False
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.gridViewHeader)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "WTransactionReport"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Transaction Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.gridViewHeader, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents optSummarry As System.Windows.Forms.RadioButton
    Friend WithEvents optDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents gridViewHeader As System.Windows.Forms.DataGridView
    Friend WithEvents chkAppPendings As System.Windows.Forms.CheckBox
    Friend WithEvents chkCanceledBill As System.Windows.Forms.CheckBox
    Friend WithEvents chkPartlySales As System.Windows.Forms.CheckBox
    Friend WithEvents chkTran As System.Windows.Forms.CheckBox
    Friend WithEvents chkMetalStcok As System.Windows.Forms.CheckBox
End Class
