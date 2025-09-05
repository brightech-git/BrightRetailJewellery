<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmbillgift
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
        Me.gridDiscTotal = New System.Windows.Forms.DataGridView
        Me.gridToBe1 = New System.Windows.Forms.DataGridView
        Me.grpMultiDiscount = New CodeVendor.Controls.Grouper
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtidesc = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtmemo = New System.Windows.Forms.TextBox
        Me.txtipce_NUM = New System.Windows.Forms.TextBox
        Me.txtiname = New System.Windows.Forms.TextBox
        CType(Me.gridDiscTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridToBe1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpMultiDiscount.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridDiscTotal
        '
        Me.gridDiscTotal.AllowUserToAddRows = False
        Me.gridDiscTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridDiscTotal.Enabled = False
        Me.gridDiscTotal.Location = New System.Drawing.Point(6, 141)
        Me.gridDiscTotal.Name = "gridDiscTotal"
        Me.gridDiscTotal.ReadOnly = True
        Me.gridDiscTotal.Size = New System.Drawing.Size(578, 19)
        Me.gridDiscTotal.TabIndex = 14
        '
        'gridToBe1
        '
        Me.gridToBe1.AllowUserToAddRows = False
        Me.gridToBe1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridToBe1.ColumnHeadersVisible = False
        Me.gridToBe1.Location = New System.Drawing.Point(6, 52)
        Me.gridToBe1.Name = "gridToBe1"
        Me.gridToBe1.ReadOnly = True
        Me.gridToBe1.RowHeadersVisible = False
        Me.gridToBe1.RowTemplate.Height = 20
        Me.gridToBe1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridToBe1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridToBe1.Size = New System.Drawing.Size(578, 89)
        Me.gridToBe1.TabIndex = 5
        '
        'grpMultiDiscount
        '
        Me.grpMultiDiscount.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpMultiDiscount.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpMultiDiscount.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpMultiDiscount.BorderColor = System.Drawing.Color.Transparent
        Me.grpMultiDiscount.BorderThickness = 1.0!
        Me.grpMultiDiscount.Controls.Add(Me.Label4)
        Me.grpMultiDiscount.Controls.Add(Me.txtidesc)
        Me.grpMultiDiscount.Controls.Add(Me.Label3)
        Me.grpMultiDiscount.Controls.Add(Me.Label2)
        Me.grpMultiDiscount.Controls.Add(Me.Label1)
        Me.grpMultiDiscount.Controls.Add(Me.txtmemo)
        Me.grpMultiDiscount.Controls.Add(Me.txtipce_NUM)
        Me.grpMultiDiscount.Controls.Add(Me.txtiname)
        Me.grpMultiDiscount.Controls.Add(Me.gridDiscTotal)
        Me.grpMultiDiscount.Controls.Add(Me.gridToBe1)
        Me.grpMultiDiscount.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpMultiDiscount.GroupImage = Nothing
        Me.grpMultiDiscount.GroupTitle = ""
        Me.grpMultiDiscount.Location = New System.Drawing.Point(3, -6)
        Me.grpMultiDiscount.Name = "grpMultiDiscount"
        Me.grpMultiDiscount.Padding = New System.Windows.Forms.Padding(20)
        Me.grpMultiDiscount.PaintGroupBox = False
        Me.grpMultiDiscount.RoundCorners = 10
        Me.grpMultiDiscount.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpMultiDiscount.ShadowControl = False
        Me.grpMultiDiscount.ShadowThickness = 3
        Me.grpMultiDiscount.Size = New System.Drawing.Size(589, 166)
        Me.grpMultiDiscount.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(137, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 13)
        Me.Label4.TabIndex = 19
        Me.Label4.Text = "Description"
        '
        'txtidesc
        '
        Me.txtidesc.Enabled = False
        Me.txtidesc.Location = New System.Drawing.Point(82, 31)
        Me.txtidesc.Name = "txtidesc"
        Me.txtidesc.Size = New System.Drawing.Size(227, 20)
        Me.txtidesc.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(397, 15)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "Remarks"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(319, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(26, 13)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "Pcs"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(23, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Item"
        '
        'txtmemo
        '
        Me.txtmemo.Location = New System.Drawing.Point(351, 31)
        Me.txtmemo.Name = "txtmemo"
        Me.txtmemo.Size = New System.Drawing.Size(228, 20)
        Me.txtmemo.TabIndex = 4
        '
        'txtipce_NUM
        '
        Me.txtipce_NUM.Location = New System.Drawing.Point(310, 31)
        Me.txtipce_NUM.Name = "txtipce_NUM"
        Me.txtipce_NUM.Size = New System.Drawing.Size(40, 20)
        Me.txtipce_NUM.TabIndex = 3
        '
        'txtiname
        '
        Me.txtiname.BackColor = System.Drawing.Color.White
        Me.txtiname.Location = New System.Drawing.Point(6, 31)
        Me.txtiname.Name = "txtiname"
        Me.txtiname.Size = New System.Drawing.Size(75, 20)
        Me.txtiname.TabIndex = 1
        '
        'frmbillgift
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(594, 155)
        Me.Controls.Add(Me.grpMultiDiscount)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmbillgift"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gift Entry"
        CType(Me.gridDiscTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridToBe1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpMultiDiscount.ResumeLayout(False)
        Me.grpMultiDiscount.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gridDiscTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridToBe1 As System.Windows.Forms.DataGridView
    Friend WithEvents grpMultiDiscount As CodeVendor.Controls.Grouper
    Friend WithEvents txtmemo As System.Windows.Forms.TextBox
    Friend WithEvents txtipce_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtiname As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtidesc As System.Windows.Forms.TextBox
End Class
