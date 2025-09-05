<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToBe
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
        Me.gridToBe = New System.Windows.Forms.DataGridView
        Me.txtPcs_NUM = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtGrsWt_WET = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtValue_AMT = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtNetWt_WET = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtRemark = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.grpTobeDet = New CodeVendor.Controls.Grouper
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtDescription = New System.Windows.Forms.TextBox
        CType(Me.gridToBe, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpTobeDet.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridToBe
        '
        Me.gridToBe.AllowUserToAddRows = False
        Me.gridToBe.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridToBe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridToBe.ColumnHeadersVisible = False
        Me.gridToBe.Location = New System.Drawing.Point(6, 62)
        Me.gridToBe.Name = "gridToBe"
        Me.gridToBe.ReadOnly = True
        Me.gridToBe.RowHeadersVisible = False
        Me.gridToBe.Size = New System.Drawing.Size(624, 138)
        Me.gridToBe.TabIndex = 12
        '
        'txtPcs_NUM
        '
        Me.txtPcs_NUM.Location = New System.Drawing.Point(153, 40)
        Me.txtPcs_NUM.Name = "txtPcs_NUM"
        Me.txtPcs_NUM.Size = New System.Drawing.Size(62, 21)
        Me.txtPcs_NUM.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(171, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(26, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Pcs"
        '
        'txtGrsWt_WET
        '
        Me.txtGrsWt_WET.Location = New System.Drawing.Point(216, 40)
        Me.txtGrsWt_WET.Name = "txtGrsWt_WET"
        Me.txtGrsWt_WET.Size = New System.Drawing.Size(75, 21)
        Me.txtGrsWt_WET.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(230, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Grs Wt"
        '
        'txtValue_AMT
        '
        Me.txtValue_AMT.Location = New System.Drawing.Point(368, 40)
        Me.txtValue_AMT.Name = "txtValue_AMT"
        Me.txtValue_AMT.Size = New System.Drawing.Size(97, 21)
        Me.txtValue_AMT.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(397, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Value"
        '
        'txtNetWt_WET
        '
        Me.txtNetWt_WET.Location = New System.Drawing.Point(292, 40)
        Me.txtNetWt_WET.Name = "txtNetWt_WET"
        Me.txtNetWt_WET.Size = New System.Drawing.Size(75, 21)
        Me.txtNetWt_WET.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(309, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(41, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "NetWt"
        '
        'txtRemark
        '
        Me.txtRemark.Location = New System.Drawing.Point(466, 40)
        Me.txtRemark.Name = "txtRemark"
        Me.txtRemark.Size = New System.Drawing.Size(146, 21)
        Me.txtRemark.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(513, 24)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(52, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Remark"
        '
        'grpTobeDet
        '
        Me.grpTobeDet.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpTobeDet.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpTobeDet.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpTobeDet.BorderColor = System.Drawing.Color.Transparent
        Me.grpTobeDet.BorderThickness = 1.0!
        Me.grpTobeDet.Controls.Add(Me.Label6)
        Me.grpTobeDet.Controls.Add(Me.Label1)
        Me.grpTobeDet.Controls.Add(Me.gridToBe)
        Me.grpTobeDet.Controls.Add(Me.Label4)
        Me.grpTobeDet.Controls.Add(Me.txtPcs_NUM)
        Me.grpTobeDet.Controls.Add(Me.txtNetWt_WET)
        Me.grpTobeDet.Controls.Add(Me.Label5)
        Me.grpTobeDet.Controls.Add(Me.Label3)
        Me.grpTobeDet.Controls.Add(Me.txtGrsWt_WET)
        Me.grpTobeDet.Controls.Add(Me.txtDescription)
        Me.grpTobeDet.Controls.Add(Me.txtRemark)
        Me.grpTobeDet.Controls.Add(Me.Label2)
        Me.grpTobeDet.Controls.Add(Me.txtValue_AMT)
        Me.grpTobeDet.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpTobeDet.GroupImage = Nothing
        Me.grpTobeDet.GroupTitle = ""
        Me.grpTobeDet.Location = New System.Drawing.Point(3, -3)
        Me.grpTobeDet.Name = "grpTobeDet"
        Me.grpTobeDet.Padding = New System.Windows.Forms.Padding(20)
        Me.grpTobeDet.PaintGroupBox = False
        Me.grpTobeDet.RoundCorners = 10
        Me.grpTobeDet.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpTobeDet.ShadowControl = False
        Me.grpTobeDet.ShadowThickness = 3
        Me.grpTobeDet.Size = New System.Drawing.Size(635, 207)
        Me.grpTobeDet.TabIndex = 0
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(44, 24)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(71, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Description"
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(6, 40)
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(146, 21)
        Me.txtDescription.TabIndex = 1
        '
        'frmToBe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(641, 208)
        Me.Controls.Add(Me.grpTobeDet)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmToBe"
        Me.ShowInTaskbar = False
        Me.Text = "Jewel not Delivered Details"
        CType(Me.gridToBe, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpTobeDet.ResumeLayout(False)
        Me.grpTobeDet.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gridToBe As System.Windows.Forms.DataGridView
    Friend WithEvents txtPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtValue_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtRemark As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents grpTobeDet As CodeVendor.Controls.Grouper
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
End Class
