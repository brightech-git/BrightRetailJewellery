<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WTrialSummary
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
        Me.gridViewFirst = New System.Windows.Forms.DataGridView
        Me.btnPrint = New System.Windows.Forms.Button
        Me.optDetailed = New System.Windows.Forms.RadioButton
        Me.optSummarry = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.dtpDateOne = New BrighttechPack.DatePicker(Me.components)
        Me.dtpDateSecond = New BrighttechPack.DatePicker(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtRateOne = New System.Windows.Forms.TextBox
        Me.txtRateSecond = New System.Windows.Forms.TextBox
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.gridViewSecond = New System.Windows.Forms.DataGridView
        CType(Me.gridViewFirst, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.gridViewSecond, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnExport
        '
        Me.btnExport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExport.Location = New System.Drawing.Point(231, 59)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(117, 30)
        Me.btnExport.TabIndex = 7
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'gridViewFirst
        '
        Me.gridViewFirst.AllowUserToAddRows = False
        Me.gridViewFirst.AllowUserToDeleteRows = False
        Me.gridViewFirst.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewFirst.Location = New System.Drawing.Point(1, 105)
        Me.gridViewFirst.Name = "gridViewFirst"
        Me.gridViewFirst.ReadOnly = True
        Me.gridViewFirst.ShowRowErrors = False
        Me.gridViewFirst.Size = New System.Drawing.Size(530, 509)
        Me.gridViewFirst.TabIndex = 10
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(353, 59)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(117, 30)
        Me.btnPrint.TabIndex = 8
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'optDetailed
        '
        Me.optDetailed.AutoSize = True
        Me.optDetailed.Checked = True
        Me.optDetailed.Location = New System.Drawing.Point(488, 8)
        Me.optDetailed.Name = "optDetailed"
        Me.optDetailed.Size = New System.Drawing.Size(64, 17)
        Me.optDetailed.TabIndex = 5
        Me.optDetailed.TabStop = True
        Me.optDetailed.Text = "Detailed"
        Me.optDetailed.UseVisualStyleBackColor = True
        Me.optDetailed.Visible = False
        '
        'optSummarry
        '
        Me.optSummarry.AutoSize = True
        Me.optSummarry.Location = New System.Drawing.Point(369, 8)
        Me.optSummarry.Name = "optSummarry"
        Me.optSummarry.Size = New System.Drawing.Size(71, 17)
        Me.optSummarry.TabIndex = 4
        Me.optSummarry.Text = "Summarry"
        Me.optSummarry.UseVisualStyleBackColor = True
        Me.optSummarry.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Comparing Dates"
        '
        'dtpDateOne
        '
        Me.dtpDateOne.Location = New System.Drawing.Point(117, 7)
        Me.dtpDateOne.Mask = "##/##/####"
        Me.dtpDateOne.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDateOne.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDateOne.Name = "dtpDateOne"
        Me.dtpDateOne.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDateOne.Size = New System.Drawing.Size(108, 20)
        Me.dtpDateOne.TabIndex = 0
        Me.dtpDateOne.Text = "07/03/9998"
        Me.dtpDateOne.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpDateSecond
        '
        Me.dtpDateSecond.Location = New System.Drawing.Point(231, 7)
        Me.dtpDateSecond.Mask = "##/##/####"
        Me.dtpDateSecond.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDateSecond.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDateSecond.Name = "dtpDateSecond"
        Me.dtpDateSecond.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDateSecond.Size = New System.Drawing.Size(108, 20)
        Me.dtpDateSecond.TabIndex = 1
        Me.dtpDateSecond.Text = "07/03/9998"
        Me.dtpDateSecond.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.txtRateOne)
        Me.Panel1.Controls.Add(Me.txtRateSecond)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.dtpDateOne)
        Me.Panel1.Controls.Add(Me.optDetailed)
        Me.Panel1.Controls.Add(Me.dtpDateSecond)
        Me.Panel1.Controls.Add(Me.optSummarry)
        Me.Panel1.Controls.Add(Me.btnView_Search)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1028, 99)
        Me.Panel1.TabIndex = 17
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(26, 36)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(88, 13)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Comparing Rates" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'txtRateOne
        '
        Me.txtRateOne.Location = New System.Drawing.Point(117, 33)
        Me.txtRateOne.Name = "txtRateOne"
        Me.txtRateOne.Size = New System.Drawing.Size(108, 20)
        Me.txtRateOne.TabIndex = 2
        '
        'txtRateSecond
        '
        Me.txtRateSecond.Location = New System.Drawing.Point(231, 33)
        Me.txtRateSecond.Name = "txtRateSecond"
        Me.txtRateSecond.Size = New System.Drawing.Size(108, 20)
        Me.txtRateSecond.TabIndex = 3
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(117, 59)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(105, 30)
        Me.btnView_Search.TabIndex = 6
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(472, 59)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(105, 30)
        Me.btnExit.TabIndex = 9
        Me.btnExit.Text = "Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'gridViewSecond
        '
        Me.gridViewSecond.AllowUserToAddRows = False
        Me.gridViewSecond.AllowUserToDeleteRows = False
        Me.gridViewSecond.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewSecond.Location = New System.Drawing.Point(537, 105)
        Me.gridViewSecond.Name = "gridViewSecond"
        Me.gridViewSecond.ReadOnly = True
        Me.gridViewSecond.Size = New System.Drawing.Size(543, 509)
        Me.gridViewSecond.TabIndex = 11
        '
        'WTrialSummary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 649)
        Me.Controls.Add(Me.gridViewSecond)
        Me.Controls.Add(Me.gridViewFirst)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "WTrialSummary"
        Me.Text = "W_TrialSummary"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridViewFirst, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.gridViewSecond, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents gridViewFirst As System.Windows.Forms.DataGridView
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents optDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents optSummarry As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpDateOne As BrighttechPack.DatePicker
    Friend WithEvents dtpDateSecond As BrighttechPack.DatePicker
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents gridViewSecond As System.Windows.Forms.DataGridView
    Friend WithEvents txtRateOne As System.Windows.Forms.TextBox
    Friend WithEvents txtRateSecond As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
