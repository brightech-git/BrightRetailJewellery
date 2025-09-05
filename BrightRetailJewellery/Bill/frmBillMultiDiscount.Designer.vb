<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBillMultiDiscount
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtAmount = New System.Windows.Forms.TextBox
        Me.grpMultiDiscount = New CodeVendor.Controls.Grouper
        Me.cmbDisc = New System.Windows.Forms.ComboBox
        Me.txtDiscRowIndex = New System.Windows.Forms.TextBox
        Me.gridDiscTotal = New System.Windows.Forms.DataGridView
        Me.gridDisc = New System.Windows.Forms.DataGridView
        Me.Label2 = New System.Windows.Forms.Label
        Me.grpMultiDiscount.SuspendLayout()
        CType(Me.gridDiscTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridDisc, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(120, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Discount"
        '
        'txtAmount
        '
        Me.txtAmount.Location = New System.Drawing.Point(245, 30)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.Size = New System.Drawing.Size(100, 21)
        Me.txtAmount.TabIndex = 1
        '
        'grpMultiDiscount
        '
        Me.grpMultiDiscount.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpMultiDiscount.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpMultiDiscount.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpMultiDiscount.BorderColor = System.Drawing.Color.Transparent
        Me.grpMultiDiscount.BorderThickness = 1.0!
        Me.grpMultiDiscount.Controls.Add(Me.cmbDisc)
        Me.grpMultiDiscount.Controls.Add(Me.txtDiscRowIndex)
        Me.grpMultiDiscount.Controls.Add(Me.gridDiscTotal)
        Me.grpMultiDiscount.Controls.Add(Me.gridDisc)
        Me.grpMultiDiscount.Controls.Add(Me.Label2)
        Me.grpMultiDiscount.Controls.Add(Me.Label1)
        Me.grpMultiDiscount.Controls.Add(Me.txtAmount)
        Me.grpMultiDiscount.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpMultiDiscount.GroupImage = Nothing
        Me.grpMultiDiscount.GroupTitle = ""
        Me.grpMultiDiscount.Location = New System.Drawing.Point(4, -5)
        Me.grpMultiDiscount.Name = "grpMultiDiscount"
        Me.grpMultiDiscount.Padding = New System.Windows.Forms.Padding(20)
        Me.grpMultiDiscount.PaintGroupBox = False
        Me.grpMultiDiscount.RoundCorners = 10
        Me.grpMultiDiscount.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpMultiDiscount.ShadowControl = False
        Me.grpMultiDiscount.ShadowThickness = 3
        Me.grpMultiDiscount.Size = New System.Drawing.Size(379, 166)
        Me.grpMultiDiscount.TabIndex = 3
        '
        'cmbDisc
        '
        Me.cmbDisc.FormattingEnabled = True
        Me.cmbDisc.Location = New System.Drawing.Point(6, 30)
        Me.cmbDisc.Name = "cmbDisc"
        Me.cmbDisc.Size = New System.Drawing.Size(238, 21)
        Me.cmbDisc.TabIndex = 16
        '
        'txtDiscRowIndex
        '
        Me.txtDiscRowIndex.Location = New System.Drawing.Point(348, 30)
        Me.txtDiscRowIndex.Name = "txtDiscRowIndex"
        Me.txtDiscRowIndex.Size = New System.Drawing.Size(23, 21)
        Me.txtDiscRowIndex.TabIndex = 15
        Me.txtDiscRowIndex.Visible = False
        '
        'gridDiscTotal
        '
        Me.gridDiscTotal.AllowUserToAddRows = False
        Me.gridDiscTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridDiscTotal.Enabled = False
        Me.gridDiscTotal.Location = New System.Drawing.Point(6, 141)
        Me.gridDiscTotal.Name = "gridDiscTotal"
        Me.gridDiscTotal.ReadOnly = True
        Me.gridDiscTotal.Size = New System.Drawing.Size(365, 19)
        Me.gridDiscTotal.TabIndex = 14
        '
        'gridDisc
        '
        Me.gridDisc.AllowUserToAddRows = False
        Me.gridDisc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridDisc.ColumnHeadersVisible = False
        Me.gridDisc.Location = New System.Drawing.Point(6, 52)
        Me.gridDisc.Name = "gridDisc"
        Me.gridDisc.ReadOnly = True
        Me.gridDisc.RowHeadersVisible = False
        Me.gridDisc.RowTemplate.Height = 20
        Me.gridDisc.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridDisc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridDisc.Size = New System.Drawing.Size(365, 89)
        Me.gridDisc.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(276, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Amount"
        '
        'frmBillMultiDiscount
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(387, 165)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpMultiDiscount)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmBillMultiDiscount"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Multi Discount"
        Me.grpMultiDiscount.ResumeLayout(False)
        Me.grpMultiDiscount.PerformLayout()
        CType(Me.gridDiscTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridDisc, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtAmount As System.Windows.Forms.TextBox
    Friend WithEvents grpMultiDiscount As CodeVendor.Controls.Grouper
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtDiscRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents gridDiscTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridDisc As System.Windows.Forms.DataGridView
    Public WithEvents cmbDisc As System.Windows.Forms.ComboBox
End Class
