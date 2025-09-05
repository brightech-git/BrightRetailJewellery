<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Wishes
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
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.chkSavings = New System.Windows.Forms.CheckBox()
        Me.btnSend = New System.Windows.Forms.Button()
        Me.chkAll = New System.Windows.Forms.CheckBox()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnView = New System.Windows.Forms.Button()
        Me.cmbtype = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dtpTo_OWN = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom_OWN = New BrighttechPack.DatePicker(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.pnlTop.SuspendLayout()
        Me.pnlMain.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.Controls.Add(Me.btnExport)
        Me.pnlTop.Controls.Add(Me.chkSavings)
        Me.pnlTop.Controls.Add(Me.btnSend)
        Me.pnlTop.Controls.Add(Me.chkAll)
        Me.pnlTop.Controls.Add(Me.btnNew)
        Me.pnlTop.Controls.Add(Me.btnExit)
        Me.pnlTop.Controls.Add(Me.btnView)
        Me.pnlTop.Controls.Add(Me.cmbtype)
        Me.pnlTop.Controls.Add(Me.Label4)
        Me.pnlTop.Controls.Add(Me.dtpTo_OWN)
        Me.pnlTop.Controls.Add(Me.dtpFrom_OWN)
        Me.pnlTop.Controls.Add(Me.Label2)
        Me.pnlTop.Controls.Add(Me.Label1)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(800, 100)
        Me.pnlTop.TabIndex = 0
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(504, 61)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 28
        Me.btnExport.Text = "Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'chkSavings
        '
        Me.chkSavings.AutoSize = True
        Me.chkSavings.Location = New System.Drawing.Point(320, 38)
        Me.chkSavings.Name = "chkSavings"
        Me.chkSavings.Size = New System.Drawing.Size(64, 17)
        Me.chkSavings.TabIndex = 27
        Me.chkSavings.Text = "Savings"
        Me.chkSavings.UseVisualStyleBackColor = True
        '
        'btnSend
        '
        Me.btnSend.Location = New System.Drawing.Point(398, 61)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(100, 30)
        Me.btnSend.TabIndex = 26
        Me.btnSend.Text = "Send SMS"
        Me.btnSend.UseVisualStyleBackColor = True
        '
        'chkAll
        '
        Me.chkAll.AutoSize = True
        Me.chkAll.Location = New System.Drawing.Point(15, 77)
        Me.chkAll.Name = "chkAll"
        Me.chkAll.Size = New System.Drawing.Size(37, 17)
        Me.chkAll.TabIndex = 25
        Me.chkAll.Text = "All"
        Me.chkAll.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(194, 61)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 23
        Me.btnNew.Text = "&New "
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(296, 61)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 24
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnView
        '
        Me.btnView.Location = New System.Drawing.Point(92, 61)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(100, 30)
        Me.btnView.TabIndex = 22
        Me.btnView.Text = "&View"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'cmbtype
        '
        Me.cmbtype.FormattingEnabled = True
        Me.cmbtype.Location = New System.Drawing.Point(92, 34)
        Me.cmbtype.Name = "cmbtype"
        Me.cmbtype.Size = New System.Drawing.Size(222, 21)
        Me.cmbtype.TabIndex = 9
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Type"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpTo_OWN
        '
        Me.dtpTo_OWN.Location = New System.Drawing.Point(221, 8)
        Me.dtpTo_OWN.Mask = "##/##/####"
        Me.dtpTo_OWN.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo_OWN.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo_OWN.Name = "dtpTo_OWN"
        Me.dtpTo_OWN.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo_OWN.Size = New System.Drawing.Size(93, 20)
        Me.dtpTo_OWN.TabIndex = 7
        Me.dtpTo_OWN.Text = "07/03/9998"
        Me.dtpTo_OWN.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom_OWN
        '
        Me.dtpFrom_OWN.Location = New System.Drawing.Point(92, 8)
        Me.dtpFrom_OWN.Mask = "##/##/####"
        Me.dtpFrom_OWN.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom_OWN.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom_OWN.Name = "dtpFrom_OWN"
        Me.dtpFrom_OWN.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom_OWN.Size = New System.Drawing.Size(93, 20)
        Me.dtpFrom_OWN.TabIndex = 5
        Me.dtpFrom_OWN.Text = "07/03/9998"
        Me.dtpFrom_OWN.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(191, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(23, 21)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "To"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Date From"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.gridView)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 100)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(800, 350)
        Me.pnlMain.TabIndex = 1
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.Name = "gridView"
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 21
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.Size = New System.Drawing.Size(800, 350)
        Me.gridView.TabIndex = 1
        '
        'Wishes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlMain)
        Me.Controls.Add(Me.pnlTop)
        Me.KeyPreview = True
        Me.Name = "Wishes"
        Me.Text = "Wishes"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.pnlMain.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlTop As Panel
    Friend WithEvents pnlMain As Panel
    Friend WithEvents dtpTo_OWN As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom_OWN As BrighttechPack.DatePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbtype As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents btnNew As Button
    Friend WithEvents btnExit As Button
    Friend WithEvents btnView As Button
    Friend WithEvents gridView As DataGridView
    Friend WithEvents chkAll As CheckBox
    Friend WithEvents btnSend As Button
    Friend WithEvents chkSavings As CheckBox
    Friend WithEvents btnExport As Button
End Class
