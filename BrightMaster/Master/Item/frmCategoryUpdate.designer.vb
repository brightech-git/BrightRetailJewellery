<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCategoryUpdate
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
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbcategory = New System.Windows.Forms.ComboBox
        Me.cmbItemName = New System.Windows.Forms.ComboBox
        Me.cmbSubName = New System.Windows.Forms.ComboBox
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.grdCategoryUpdate = New System.Windows.Forms.DataGridView
        CType(Me.grdCategoryUpdate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(331, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Item Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(35, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Sub ItemName"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(36, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 13)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Category"
        '
        'cmbcategory
        '
        Me.cmbcategory.FormattingEnabled = True
        Me.cmbcategory.Location = New System.Drawing.Point(115, 10)
        Me.cmbcategory.Name = "cmbcategory"
        Me.cmbcategory.Size = New System.Drawing.Size(193, 21)
        Me.cmbcategory.TabIndex = 12
        '
        'cmbItemName
        '
        Me.cmbItemName.FormattingEnabled = True
        Me.cmbItemName.Location = New System.Drawing.Point(395, 14)
        Me.cmbItemName.Name = "cmbItemName"
        Me.cmbItemName.Size = New System.Drawing.Size(209, 21)
        Me.cmbItemName.TabIndex = 13
        '
        'cmbSubName
        '
        Me.cmbSubName.FormattingEnabled = True
        Me.cmbSubName.Location = New System.Drawing.Point(115, 37)
        Me.cmbSubName.Name = "cmbSubName"
        Me.cmbSubName.Size = New System.Drawing.Size(189, 21)
        Me.cmbSubName.TabIndex = 14
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(395, 42)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(90, 28)
        Me.btnUpdate.TabIndex = 15
        Me.btnUpdate.Text = "Update[F1]"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(499, 41)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(105, 28)
        Me.btnExit.TabIndex = 16
        Me.btnExit.Text = "Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'grdCategoryUpdate
        '
        Me.grdCategoryUpdate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdCategoryUpdate.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.grdCategoryUpdate.Location = New System.Drawing.Point(0, 76)
        Me.grdCategoryUpdate.Name = "grdCategoryUpdate"
        Me.grdCategoryUpdate.Size = New System.Drawing.Size(641, 314)
        Me.grdCategoryUpdate.TabIndex = 17
        '
        'frmCategoryUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(641, 390)
        Me.ControlBox = False
        Me.Controls.Add(Me.grdCategoryUpdate)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.cmbSubName)
        Me.Controls.Add(Me.cmbItemName)
        Me.Controls.Add(Me.cmbcategory)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmCategoryUpdate"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmCategoryUpdate"
        CType(Me.grdCategoryUpdate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbcategory As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItemName As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubName As System.Windows.Forms.ComboBox
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents grdCategoryUpdate As System.Windows.Forms.DataGridView
End Class
