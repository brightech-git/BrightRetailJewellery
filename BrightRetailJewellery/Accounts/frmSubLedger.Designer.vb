<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSubLedger
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.grpSubledger = New CodeVendor.Controls.Grouper
        Me.GridSubledgerTotal = New System.Windows.Forms.DataGridView
        Me.gridView_OWN = New System.Windows.Forms.DataGridView
        Me.cmbAcname = New System.Windows.Forms.ComboBox
        Me.txtRowIndex = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtAmount_AMT = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label55 = New System.Windows.Forms.Label
        Me.grpSubledger.SuspendLayout()
        CType(Me.GridSubledgerTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpSubledger
        '
        Me.grpSubledger.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpSubledger.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpSubledger.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpSubledger.BorderColor = System.Drawing.Color.Transparent
        Me.grpSubledger.BorderThickness = 1.0!
        Me.grpSubledger.Controls.Add(Me.GridSubledgerTotal)
        Me.grpSubledger.Controls.Add(Me.gridView_OWN)
        Me.grpSubledger.Controls.Add(Me.cmbAcname)
        Me.grpSubledger.Controls.Add(Me.txtRowIndex)
        Me.grpSubledger.Controls.Add(Me.Label2)
        Me.grpSubledger.Controls.Add(Me.txtAmount_AMT)
        Me.grpSubledger.Controls.Add(Me.Label1)
        Me.grpSubledger.Controls.Add(Me.Label55)
        Me.grpSubledger.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpSubledger.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpSubledger.GroupImage = Nothing
        Me.grpSubledger.GroupTitle = ""
        Me.grpSubledger.Location = New System.Drawing.Point(0, 0)
        Me.grpSubledger.Name = "grpSubledger"
        Me.grpSubledger.Padding = New System.Windows.Forms.Padding(20)
        Me.grpSubledger.PaintGroupBox = False
        Me.grpSubledger.RoundCorners = 10
        Me.grpSubledger.ShadowColor = System.Drawing.SystemColors.InactiveCaption
        Me.grpSubledger.ShadowControl = False
        Me.grpSubledger.ShadowThickness = 3
        Me.grpSubledger.Size = New System.Drawing.Size(531, 222)
        Me.grpSubledger.TabIndex = 0
        '
        'GridSubledgerTotal
        '
        Me.GridSubledgerTotal.AllowUserToAddRows = False
        Me.GridSubledgerTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.GridSubledgerTotal.DefaultCellStyle = DataGridViewCellStyle1
        Me.GridSubledgerTotal.Enabled = False
        Me.GridSubledgerTotal.Location = New System.Drawing.Point(1, 193)
        Me.GridSubledgerTotal.Name = "GridSubledgerTotal"
        Me.GridSubledgerTotal.ReadOnly = True
        Me.GridSubledgerTotal.RowHeadersVisible = False
        Me.GridSubledgerTotal.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Window
        Me.GridSubledgerTotal.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.WindowText
        Me.GridSubledgerTotal.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.GridSubledgerTotal.Size = New System.Drawing.Size(528, 30)
        Me.GridSubledgerTotal.TabIndex = 5
        '
        'gridView_OWN
        '
        Me.gridView_OWN.AllowUserToAddRows = False
        Me.gridView_OWN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.InactiveCaptionText
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView_OWN.DefaultCellStyle = DataGridViewCellStyle2
        Me.gridView_OWN.Location = New System.Drawing.Point(1, 54)
        Me.gridView_OWN.Name = "gridView_OWN"
        Me.gridView_OWN.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.InactiveCaptionText
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridView_OWN.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.gridView_OWN.RowHeadersVisible = False
        Me.gridView_OWN.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.InactiveCaptionText
        Me.gridView_OWN.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.WindowText
        Me.gridView_OWN.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridView_OWN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView_OWN.Size = New System.Drawing.Size(528, 138)
        Me.gridView_OWN.TabIndex = 4
        '
        'cmbAcname
        '
        Me.cmbAcname.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAcname.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbAcname.FormattingEnabled = True
        Me.cmbAcname.Location = New System.Drawing.Point(0, 31)
        Me.cmbAcname.Name = "cmbAcname"
        Me.cmbAcname.Size = New System.Drawing.Size(342, 22)
        Me.cmbAcname.TabIndex = 1
        '
        'txtRowIndex
        '
        Me.txtRowIndex.Location = New System.Drawing.Point(519, 6)
        Me.txtRowIndex.Name = "txtRowIndex"
        Me.txtRowIndex.Size = New System.Drawing.Size(10, 21)
        Me.txtRowIndex.TabIndex = 6
        Me.txtRowIndex.Visible = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(-3, 3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(345, 24)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "PARTICULAR"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtAmount_AMT
        '
        Me.txtAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount_AMT.Location = New System.Drawing.Point(343, 31)
        Me.txtAmount_AMT.MaxLength = 12
        Me.txtAmount_AMT.Name = "txtAmount_AMT"
        Me.txtAmount_AMT.ShortcutsEnabled = False
        Me.txtAmount_AMT.Size = New System.Drawing.Size(186, 22)
        Me.txtAmount_AMT.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(340, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(190, 24)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "AMOUNT"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label55
        '
        Me.Label55.BackColor = System.Drawing.Color.Transparent
        Me.Label55.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label55.Location = New System.Drawing.Point(982, 14)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(80, 15)
        Me.Label55.TabIndex = 20
        Me.Label55.Text = "AMOUNT"
        Me.Label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmSubLedger
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(531, 222)
        Me.Controls.Add(Me.grpSubledger)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.Name = "frmSubLedger"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sub Ledger Entries"
        Me.grpSubledger.ResumeLayout(False)
        Me.grpSubledger.PerformLayout()
        CType(Me.GridSubledgerTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpSubledger As CodeVendor.Controls.Grouper
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents cmbAcname As System.Windows.Forms.ComboBox
    Friend WithEvents gridView_OWN As System.Windows.Forms.DataGridView
    Friend WithEvents GridSubledgerTotal As System.Windows.Forms.DataGridView
End Class
