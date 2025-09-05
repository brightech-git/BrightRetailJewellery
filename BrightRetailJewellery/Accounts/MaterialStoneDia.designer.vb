<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MaterialStoneDia
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
        Me.Label57 = New System.Windows.Forms.Label
        Me.cmbStCalc = New System.Windows.Forms.ComboBox
        Me.cmbStUnit = New System.Windows.Forms.ComboBox
        Me.Label58 = New System.Windows.Forms.Label
        Me.Label59 = New System.Windows.Forms.Label
        Me.Label47 = New System.Windows.Forms.Label
        Me.Label46 = New System.Windows.Forms.Label
        Me.Label26 = New System.Windows.Forms.Label
        Me.grpStone = New CodeVendor.Controls.Grouper
        Me.txtStMetalCode = New System.Windows.Forms.TextBox
        Me.Label61 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label60 = New System.Windows.Forms.Label
        Me.txtStAmount_AMT = New System.Windows.Forms.TextBox
        Me.txtSize = New System.Windows.Forms.TextBox
        Me.txtStRate_AMT = New System.Windows.Forms.TextBox
        Me.txtStRowIndex = New System.Windows.Forms.TextBox
        Me.txtStSubItem = New System.Windows.Forms.TextBox
        Me.txtStItem = New System.Windows.Forms.TextBox
        Me.gridStoneTotal = New System.Windows.Forms.DataGridView
        Me.gridStone = New System.Windows.Forms.DataGridView
        Me.txtStPcs_NUM = New System.Windows.Forms.TextBox
        Me.txtStWeight = New System.Windows.Forms.TextBox
        Me.grpStone.SuspendLayout()
        CType(Me.gridStoneTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridStone, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label57
        '
        Me.Label57.BackColor = System.Drawing.Color.Transparent
        Me.Label57.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label57.Location = New System.Drawing.Point(660, 15)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(39, 15)
        Me.Label57.TabIndex = 9
        Me.Label57.Text = "Unit"
        Me.Label57.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbStCalc
        '
        Me.cmbStCalc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStCalc.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStCalc.FormattingEnabled = True
        Me.cmbStCalc.Location = New System.Drawing.Point(700, 36)
        Me.cmbStCalc.Name = "cmbStCalc"
        Me.cmbStCalc.Size = New System.Drawing.Size(39, 22)
        Me.cmbStCalc.TabIndex = 12
        '
        'cmbStUnit
        '
        Me.cmbStUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStUnit.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStUnit.FormattingEnabled = True
        Me.cmbStUnit.Location = New System.Drawing.Point(660, 36)
        Me.cmbStUnit.Name = "cmbStUnit"
        Me.cmbStUnit.Size = New System.Drawing.Size(39, 22)
        Me.cmbStUnit.TabIndex = 10
        '
        'Label58
        '
        Me.Label58.BackColor = System.Drawing.Color.Transparent
        Me.Label58.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label58.Location = New System.Drawing.Point(700, 15)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(39, 15)
        Me.Label58.TabIndex = 11
        Me.Label58.Text = "Cal"
        Me.Label58.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label59
        '
        Me.Label59.BackColor = System.Drawing.Color.Transparent
        Me.Label59.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label59.Location = New System.Drawing.Point(419, 15)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(84, 15)
        Me.Label59.TabIndex = 6
        Me.Label59.Text = "Weight"
        Me.Label59.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label47
        '
        Me.Label47.BackColor = System.Drawing.Color.Transparent
        Me.Label47.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.Location = New System.Drawing.Point(8, 15)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(201, 15)
        Me.Label47.TabIndex = 0
        Me.Label47.Text = "Item"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label46
        '
        Me.Label46.BackColor = System.Drawing.Color.Transparent
        Me.Label46.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label46.Location = New System.Drawing.Point(210, 15)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(168, 15)
        Me.Label46.TabIndex = 2
        Me.Label46.Text = "Sub Item"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label26
        '
        Me.Label26.BackColor = System.Drawing.Color.Transparent
        Me.Label26.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(379, 15)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(39, 15)
        Me.Label26.TabIndex = 4
        Me.Label26.Text = "Pcs"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpStone
        '
        Me.grpStone.BackgroundColor = System.Drawing.SystemColors.Control
        Me.grpStone.BackgroundGradientColor = System.Drawing.SystemColors.Control
        Me.grpStone.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpStone.BorderColor = System.Drawing.Color.Transparent
        Me.grpStone.BorderThickness = 1.0!
        Me.grpStone.Controls.Add(Me.txtStMetalCode)
        Me.grpStone.Controls.Add(Me.Label61)
        Me.grpStone.Controls.Add(Me.Label1)
        Me.grpStone.Controls.Add(Me.Label60)
        Me.grpStone.Controls.Add(Me.txtStAmount_AMT)
        Me.grpStone.Controls.Add(Me.txtSize)
        Me.grpStone.Controls.Add(Me.txtStRate_AMT)
        Me.grpStone.Controls.Add(Me.txtStRowIndex)
        Me.grpStone.Controls.Add(Me.txtStSubItem)
        Me.grpStone.Controls.Add(Me.txtStItem)
        Me.grpStone.Controls.Add(Me.gridStoneTotal)
        Me.grpStone.Controls.Add(Me.gridStone)
        Me.grpStone.Controls.Add(Me.Label57)
        Me.grpStone.Controls.Add(Me.cmbStCalc)
        Me.grpStone.Controls.Add(Me.cmbStUnit)
        Me.grpStone.Controls.Add(Me.Label58)
        Me.grpStone.Controls.Add(Me.Label59)
        Me.grpStone.Controls.Add(Me.Label47)
        Me.grpStone.Controls.Add(Me.Label46)
        Me.grpStone.Controls.Add(Me.Label26)
        Me.grpStone.Controls.Add(Me.txtStPcs_NUM)
        Me.grpStone.Controls.Add(Me.txtStWeight)
        Me.grpStone.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpStone.GroupImage = Nothing
        Me.grpStone.GroupTitle = ""
        Me.grpStone.Location = New System.Drawing.Point(-1, 2)
        Me.grpStone.Name = "grpStone"
        Me.grpStone.Padding = New System.Windows.Forms.Padding(23, 20, 23, 20)
        Me.grpStone.PaintGroupBox = False
        Me.grpStone.RoundCorners = 10
        Me.grpStone.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpStone.ShadowControl = False
        Me.grpStone.ShadowThickness = 3
        Me.grpStone.Size = New System.Drawing.Size(993, 207)
        Me.grpStone.TabIndex = 1
        '
        'txtStMetalCode
        '
        Me.txtStMetalCode.Enabled = False
        Me.txtStMetalCode.Location = New System.Drawing.Point(492, 92)
        Me.txtStMetalCode.Name = "txtStMetalCode"
        Me.txtStMetalCode.Size = New System.Drawing.Size(8, 22)
        Me.txtStMetalCode.TabIndex = 36
        Me.txtStMetalCode.Visible = False
        '
        'Label61
        '
        Me.Label61.BackColor = System.Drawing.Color.Transparent
        Me.Label61.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label61.Location = New System.Drawing.Point(848, 15)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(111, 15)
        Me.Label61.TabIndex = 15
        Me.Label61.Text = "Amount"
        Me.Label61.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(504, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(155, 15)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Seive Size"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label60
        '
        Me.Label60.BackColor = System.Drawing.Color.Transparent
        Me.Label60.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label60.Location = New System.Drawing.Point(740, 15)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(107, 15)
        Me.Label60.TabIndex = 13
        Me.Label60.Text = "Rate"
        Me.Label60.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtStAmount_AMT
        '
        Me.txtStAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStAmount_AMT.Location = New System.Drawing.Point(848, 36)
        Me.txtStAmount_AMT.MaxLength = 12
        Me.txtStAmount_AMT.Name = "txtStAmount_AMT"
        Me.txtStAmount_AMT.Size = New System.Drawing.Size(111, 22)
        Me.txtStAmount_AMT.TabIndex = 16
        Me.txtStAmount_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSize
        '
        Me.txtSize.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSize.Location = New System.Drawing.Point(504, 36)
        Me.txtSize.Name = "txtSize"
        Me.txtSize.Size = New System.Drawing.Size(155, 22)
        Me.txtSize.TabIndex = 8
        '
        'txtStRate_AMT
        '
        Me.txtStRate_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStRate_AMT.Location = New System.Drawing.Point(740, 36)
        Me.txtStRate_AMT.MaxLength = 10
        Me.txtStRate_AMT.Name = "txtStRate_AMT"
        Me.txtStRate_AMT.Size = New System.Drawing.Size(107, 22)
        Me.txtStRate_AMT.TabIndex = 14
        Me.txtStRate_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtStRowIndex
        '
        Me.txtStRowIndex.Location = New System.Drawing.Point(642, 0)
        Me.txtStRowIndex.Name = "txtStRowIndex"
        Me.txtStRowIndex.Size = New System.Drawing.Size(12, 22)
        Me.txtStRowIndex.TabIndex = 13
        Me.txtStRowIndex.Visible = False
        '
        'txtStSubItem
        '
        Me.txtStSubItem.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStSubItem.Location = New System.Drawing.Point(210, 36)
        Me.txtStSubItem.Name = "txtStSubItem"
        Me.txtStSubItem.Size = New System.Drawing.Size(168, 22)
        Me.txtStSubItem.TabIndex = 3
        '
        'txtStItem
        '
        Me.txtStItem.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStItem.Location = New System.Drawing.Point(8, 36)
        Me.txtStItem.Name = "txtStItem"
        Me.txtStItem.Size = New System.Drawing.Size(201, 22)
        Me.txtStItem.TabIndex = 1
        '
        'gridStoneTotal
        '
        Me.gridStoneTotal.AllowUserToAddRows = False
        Me.gridStoneTotal.AllowUserToDeleteRows = False
        Me.gridStoneTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridStoneTotal.ColumnHeadersVisible = False
        Me.gridStoneTotal.Enabled = False
        Me.gridStoneTotal.Location = New System.Drawing.Point(8, 181)
        Me.gridStoneTotal.Name = "gridStoneTotal"
        Me.gridStoneTotal.ReadOnly = True
        Me.gridStoneTotal.RowHeadersVisible = False
        Me.gridStoneTotal.Size = New System.Drawing.Size(977, 19)
        Me.gridStoneTotal.TabIndex = 18
        '
        'gridStone
        '
        Me.gridStone.AllowUserToAddRows = False
        Me.gridStone.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridStone.ColumnHeadersVisible = False
        Me.gridStone.Location = New System.Drawing.Point(8, 59)
        Me.gridStone.MultiSelect = False
        Me.gridStone.Name = "gridStone"
        Me.gridStone.ReadOnly = True
        Me.gridStone.RowHeadersVisible = False
        Me.gridStone.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridStone.RowTemplate.Height = 20
        Me.gridStone.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridStone.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.gridStone.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridStone.Size = New System.Drawing.Size(977, 122)
        Me.gridStone.TabIndex = 17
        '
        'txtStPcs_NUM
        '
        Me.txtStPcs_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStPcs_NUM.Location = New System.Drawing.Point(379, 36)
        Me.txtStPcs_NUM.MaxLength = 8
        Me.txtStPcs_NUM.Name = "txtStPcs_NUM"
        Me.txtStPcs_NUM.Size = New System.Drawing.Size(39, 22)
        Me.txtStPcs_NUM.TabIndex = 5
        Me.txtStPcs_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtStWeight
        '
        Me.txtStWeight.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStWeight.Location = New System.Drawing.Point(419, 36)
        Me.txtStWeight.MaxLength = 10
        Me.txtStWeight.Name = "txtStWeight"
        Me.txtStWeight.Size = New System.Drawing.Size(84, 22)
        Me.txtStWeight.TabIndex = 7
        Me.txtStWeight.Text = "9999.999"
        Me.txtStWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'MaterialStoneDia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(996, 210)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpStone)
        Me.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "MaterialStoneDia"
        Me.ShowInTaskbar = False
        Me.Text = "Stone Details"
        Me.grpStone.ResumeLayout(False)
        Me.grpStone.PerformLayout()
        CType(Me.gridStoneTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridStone, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents cmbStCalc As System.Windows.Forms.ComboBox
    Friend WithEvents cmbStUnit As System.Windows.Forms.ComboBox
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents grpStone As CodeVendor.Controls.Grouper
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtSize As System.Windows.Forms.TextBox
    Friend WithEvents txtStRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents txtStSubItem As System.Windows.Forms.TextBox
    Friend WithEvents txtStItem As System.Windows.Forms.TextBox
    Friend WithEvents gridStoneTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridStone As System.Windows.Forms.DataGridView
    Friend WithEvents txtStPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtStWeight As System.Windows.Forms.TextBox
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents txtStAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtStRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtStMetalCode As System.Windows.Forms.TextBox
End Class
