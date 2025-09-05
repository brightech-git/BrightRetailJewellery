<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Denomination
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
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.grpDenomination = New CodeVendor.Controls.Grouper
        Me.lblSugAmt = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.gridTotal = New System.Windows.Forms.DataGridView
        Me.gridViewHeader = New System.Windows.Forms.DataGridView
        Me.txt = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtBillAmount = New System.Windows.Forms.TextBox
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDenomination.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.gridTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridViewHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BackgroundColor = System.Drawing.Color.DarkGray
        Me.gridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 18)
        Me.gridView.Name = "gridView"
        Me.gridView.Size = New System.Drawing.Size(343, 268)
        Me.gridView.TabIndex = 0
        '
        'grpDenomination
        '
        Me.grpDenomination.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpDenomination.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpDenomination.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpDenomination.BorderColor = System.Drawing.Color.Empty
        Me.grpDenomination.BorderThickness = 1.0!
        Me.grpDenomination.Controls.Add(Me.lblSugAmt)
        Me.grpDenomination.Controls.Add(Me.Panel1)
        Me.grpDenomination.Controls.Add(Me.Label1)
        Me.grpDenomination.Controls.Add(Me.txtBillAmount)
        Me.grpDenomination.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpDenomination.GroupImage = Nothing
        Me.grpDenomination.GroupTitle = ""
        Me.grpDenomination.Location = New System.Drawing.Point(7, -3)
        Me.grpDenomination.Name = "grpDenomination"
        Me.grpDenomination.Padding = New System.Windows.Forms.Padding(26, 24, 26, 24)
        Me.grpDenomination.PaintGroupBox = False
        Me.grpDenomination.RoundCorners = 10
        Me.grpDenomination.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpDenomination.ShadowControl = False
        Me.grpDenomination.ShadowThickness = 3
        Me.grpDenomination.Size = New System.Drawing.Size(354, 368)
        Me.grpDenomination.TabIndex = 1
        '
        'lblSugAmt
        '
        Me.lblSugAmt.AutoSize = True
        Me.lblSugAmt.Location = New System.Drawing.Point(18, 22)
        Me.lblSugAmt.Name = "lblSugAmt"
        Me.lblSugAmt.Size = New System.Drawing.Size(52, 14)
        Me.lblSugAmt.TabIndex = 7
        Me.lblSugAmt.Text = "Label2"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.gridView)
        Me.Panel1.Controls.Add(Me.gridTotal)
        Me.Panel1.Controls.Add(Me.gridViewHeader)
        Me.Panel1.Controls.Add(Me.txt)
        Me.Panel1.Location = New System.Drawing.Point(5, 50)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(343, 308)
        Me.Panel1.TabIndex = 5
        '
        'gridTotal
        '
        Me.gridTotal.AllowUserToAddRows = False
        Me.gridTotal.AllowUserToDeleteRows = False
        Me.gridTotal.AllowUserToResizeColumns = False
        Me.gridTotal.AllowUserToResizeRows = False
        Me.gridTotal.BackgroundColor = System.Drawing.Color.DarkGray
        Me.gridTotal.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridTotal.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.gridTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridTotal.ColumnHeadersVisible = False
        Me.gridTotal.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.gridTotal.Location = New System.Drawing.Point(0, 286)
        Me.gridTotal.Name = "gridTotal"
        Me.gridTotal.ReadOnly = True
        Me.gridTotal.RowHeadersVisible = False
        Me.gridTotal.Size = New System.Drawing.Size(343, 22)
        Me.gridTotal.TabIndex = 7
        '
        'gridViewHeader
        '
        Me.gridViewHeader.AllowUserToAddRows = False
        Me.gridViewHeader.AllowUserToDeleteRows = False
        Me.gridViewHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHeader.Location = New System.Drawing.Point(0, 0)
        Me.gridViewHeader.Name = "gridViewHeader"
        Me.gridViewHeader.ReadOnly = True
        Me.gridViewHeader.Size = New System.Drawing.Size(343, 18)
        Me.gridViewHeader.TabIndex = 6
        Me.gridViewHeader.Visible = False
        '
        'txt
        '
        Me.txt.Location = New System.Drawing.Point(147, 101)
        Me.txt.Name = "txt"
        Me.txt.Size = New System.Drawing.Size(89, 22)
        Me.txt.TabIndex = 1
        Me.txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(181, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 14)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Cash"
        '
        'txtBillAmount
        '
        Me.txtBillAmount.Location = New System.Drawing.Point(226, 22)
        Me.txtBillAmount.Name = "txtBillAmount"
        Me.txtBillAmount.Size = New System.Drawing.Size(119, 22)
        Me.txtBillAmount.TabIndex = 2
        Me.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Denomination
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(366, 369)
        Me.Controls.Add(Me.grpDenomination)
        Me.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Denomination"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Denomination"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDenomination.ResumeLayout(False)
        Me.grpDenomination.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.gridTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridViewHeader, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents grpDenomination As CodeVendor.Controls.Grouper
    Friend WithEvents txt As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtBillAmount As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents gridViewHeader As System.Windows.Forms.DataGridView
    Friend WithEvents lblSugAmt As System.Windows.Forms.Label
    Friend WithEvents gridTotal As System.Windows.Forms.DataGridView
End Class
