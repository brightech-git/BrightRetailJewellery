<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRequestionSlip
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
        Me.pnltop = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtRange_num = New System.Windows.Forms.TextBox
        Me.txtTo_num = New System.Windows.Forms.TextBox
        Me.txtFrom_NUM = New System.Windows.Forms.TextBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.cmbCountername = New System.Windows.Forms.ComboBox
        Me.chkItemName = New BrighttechPack.CheckedComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.dGrid = New System.Windows.Forms.DataGridView
        Me.pnltop.SuspendLayout()
        CType(Me.dGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnltop
        '
        Me.pnltop.Controls.Add(Me.Label5)
        Me.pnltop.Controls.Add(Me.Label4)
        Me.pnltop.Controls.Add(Me.Label3)
        Me.pnltop.Controls.Add(Me.txtRange_num)
        Me.pnltop.Controls.Add(Me.txtTo_num)
        Me.pnltop.Controls.Add(Me.txtFrom_NUM)
        Me.pnltop.Controls.Add(Me.btnExit)
        Me.pnltop.Controls.Add(Me.btnSearch)
        Me.pnltop.Controls.Add(Me.btnNew)
        Me.pnltop.Controls.Add(Me.cmbCountername)
        Me.pnltop.Controls.Add(Me.chkItemName)
        Me.pnltop.Controls.Add(Me.Label2)
        Me.pnltop.Controls.Add(Me.Label1)
        Me.pnltop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnltop.Location = New System.Drawing.Point(0, 0)
        Me.pnltop.Name = "pnltop"
        Me.pnltop.Size = New System.Drawing.Size(898, 120)
        Me.pnltop.TabIndex = 0
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(226, 46)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(39, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Range"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(150, 46)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(20, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 46)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Weight From"
        '
        'txtRange_num
        '
        Me.txtRange_num.Location = New System.Drawing.Point(290, 43)
        Me.txtRange_num.Name = "txtRange_num"
        Me.txtRange_num.Size = New System.Drawing.Size(56, 20)
        Me.txtRange_num.TabIndex = 9
        '
        'txtTo_num
        '
        Me.txtTo_num.Location = New System.Drawing.Point(171, 42)
        Me.txtTo_num.Name = "txtTo_num"
        Me.txtTo_num.Size = New System.Drawing.Size(49, 20)
        Me.txtTo_num.TabIndex = 7
        '
        'txtFrom_NUM
        '
        Me.txtFrom_NUM.Location = New System.Drawing.Point(99, 42)
        Me.txtFrom_NUM.Name = "txtFrom_NUM"
        Me.txtFrom_NUM.Size = New System.Drawing.Size(49, 20)
        Me.txtFrom_NUM.TabIndex = 5
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(307, 79)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 12
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(95, 79)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 10
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(201, 79)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 11
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'cmbCountername
        '
        Me.cmbCountername.FormattingEnabled = True
        Me.cmbCountername.Location = New System.Drawing.Point(99, 10)
        Me.cmbCountername.Name = "cmbCountername"
        Me.cmbCountername.Size = New System.Drawing.Size(121, 21)
        Me.cmbCountername.TabIndex = 1
        '
        'chkItemName
        '
        Me.chkItemName.CheckOnClick = True
        Me.chkItemName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkItemName.DropDownHeight = 1
        Me.chkItemName.FormattingEnabled = True
        Me.chkItemName.IntegralHeight = False
        Me.chkItemName.Location = New System.Drawing.Point(290, 10)
        Me.chkItemName.Name = "chkItemName"
        Me.chkItemName.Size = New System.Drawing.Size(157, 21)
        Me.chkItemName.TabIndex = 3
        Me.chkItemName.ValueSeparator = ", "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(226, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Item Name"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Counter Name"
        '
        'dGrid
        '
        Me.dGrid.AllowUserToAddRows = False
        Me.dGrid.AllowUserToDeleteRows = False
        Me.dGrid.AllowUserToResizeColumns = False
        Me.dGrid.AllowUserToResizeRows = False
        Me.dGrid.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dGrid.Location = New System.Drawing.Point(0, 120)
        Me.dGrid.Name = "dGrid"
        Me.dGrid.RowHeadersVisible = False
        Me.dGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dGrid.Size = New System.Drawing.Size(898, 406)
        Me.dGrid.TabIndex = 1
        '
        'frmRequestionSlip
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(898, 526)
        Me.Controls.Add(Me.dGrid)
        Me.Controls.Add(Me.pnltop)
        Me.Name = "frmRequestionSlip"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Requestion Slip"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnltop.ResumeLayout(False)
        Me.pnltop.PerformLayout()
        CType(Me.dGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnltop As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbCountername As System.Windows.Forms.ComboBox
    Friend WithEvents chkItemName As BrighttechPack.CheckedComboBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents dGrid As System.Windows.Forms.DataGridView
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtRange_num As System.Windows.Forms.TextBox
    Friend WithEvents txtTo_num As System.Windows.Forms.TextBox
    Friend WithEvents txtFrom_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
