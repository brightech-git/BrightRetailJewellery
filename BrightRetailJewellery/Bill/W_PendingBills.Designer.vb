<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class W_PendingBills
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
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtDetAmount = New System.Windows.Forms.TextBox
        Me.txtDetWeight = New System.Windows.Forms.TextBox
        Me.txtEditTotAMT_AMT = New System.Windows.Forms.TextBox
        Me.txtEditTotWt_WET = New System.Windows.Forms.TextBox
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.Grouper1 = New CodeVendor.Controls.Grouper
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Grouper1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(178, 315)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 14)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Amount"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 315)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(54, 14)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Weight"
        '
        'txtDetAmount
        '
        Me.txtDetAmount.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetAmount.Location = New System.Drawing.Point(241, 312)
        Me.txtDetAmount.Name = "txtDetAmount"
        Me.txtDetAmount.Size = New System.Drawing.Size(100, 22)
        Me.txtDetAmount.TabIndex = 4
        Me.txtDetAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDetWeight
        '
        Me.txtDetWeight.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetWeight.Location = New System.Drawing.Point(72, 312)
        Me.txtDetWeight.Name = "txtDetWeight"
        Me.txtDetWeight.Size = New System.Drawing.Size(100, 22)
        Me.txtDetWeight.TabIndex = 2
        Me.txtDetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtEditTotAMT_AMT
        '
        Me.txtEditTotAMT_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEditTotAMT_AMT.Location = New System.Drawing.Point(241, 344)
        Me.txtEditTotAMT_AMT.Name = "txtEditTotAMT_AMT"
        Me.txtEditTotAMT_AMT.Size = New System.Drawing.Size(100, 22)
        Me.txtEditTotAMT_AMT.TabIndex = 6
        Me.txtEditTotAMT_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtEditTotWt_WET
        '
        Me.txtEditTotWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEditTotWt_WET.Location = New System.Drawing.Point(72, 344)
        Me.txtEditTotWt_WET.Name = "txtEditTotWt_WET"
        Me.txtEditTotWt_WET.Size = New System.Drawing.Size(100, 22)
        Me.txtEditTotWt_WET.TabIndex = 5
        Me.txtEditTotWt_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BackgroundColor = System.Drawing.Color.Gainsboro
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(9, 19)
        Me.gridView.Name = "gridView"
        Me.gridView.Size = New System.Drawing.Size(332, 280)
        Me.gridView.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(260, 379)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(92, 27)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Location = New System.Drawing.Point(162, 379)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(92, 27)
        Me.btnOk.TabIndex = 1
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'Grouper1
        '
        Me.Grouper1.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper1.BorderColor = System.Drawing.Color.Empty
        Me.Grouper1.BorderThickness = 1.0!
        Me.Grouper1.Controls.Add(Me.Label4)
        Me.Grouper1.Controls.Add(Me.gridView)
        Me.Grouper1.Controls.Add(Me.Label3)
        Me.Grouper1.Controls.Add(Me.txtEditTotWt_WET)
        Me.Grouper1.Controls.Add(Me.txtDetAmount)
        Me.Grouper1.Controls.Add(Me.txtEditTotAMT_AMT)
        Me.Grouper1.Controls.Add(Me.txtDetWeight)
        Me.Grouper1.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper1.GroupImage = Nothing
        Me.Grouper1.GroupTitle = ""
        Me.Grouper1.Location = New System.Drawing.Point(6, -2)
        Me.Grouper1.Name = "Grouper1"
        Me.Grouper1.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper1.PaintGroupBox = False
        Me.Grouper1.RoundCorners = 10
        Me.Grouper1.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper1.ShadowControl = False
        Me.Grouper1.ShadowThickness = 3
        Me.Grouper1.Size = New System.Drawing.Size(350, 375)
        Me.Grouper1.TabIndex = 0
        '
        'W_PendingBills
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(364, 412)
        Me.ControlBox = False
        Me.Controls.Add(Me.Grouper1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "W_PendingBills"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "W_PendingBills"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Grouper1.ResumeLayout(False)
        Me.Grouper1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents txtEditTotAMT_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtEditTotWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtDetAmount As System.Windows.Forms.TextBox
    Friend WithEvents txtDetWeight As System.Windows.Forms.TextBox
    Friend WithEvents Grouper1 As CodeVendor.Controls.Grouper
End Class
