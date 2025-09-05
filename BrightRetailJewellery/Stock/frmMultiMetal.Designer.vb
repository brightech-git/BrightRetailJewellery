<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMultiMetal
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
        Me.gridMultiMetal = New System.Windows.Forms.DataGridView
        Me.txtGrid = New System.Windows.Forms.TextBox
        CType(Me.gridMultiMetal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gridMultiMetal
        '
        Me.gridMultiMetal.AllowUserToAddRows = False
        Me.gridMultiMetal.AllowUserToDeleteRows = False
        Me.gridMultiMetal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridMultiMetal.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridMultiMetal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridMultiMetal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridMultiMetal.Location = New System.Drawing.Point(0, 0)
        Me.gridMultiMetal.Name = "gridMultiMetal"
        Me.gridMultiMetal.ReadOnly = True
        Me.gridMultiMetal.RowHeadersVisible = False
        Me.gridMultiMetal.Size = New System.Drawing.Size(750, 203)
        Me.gridMultiMetal.TabIndex = 0
        '
        'txtGrid
        '
        Me.txtGrid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGrid.Location = New System.Drawing.Point(249, 136)
        Me.txtGrid.Name = "txtGrid"
        Me.txtGrid.Size = New System.Drawing.Size(179, 21)
        Me.txtGrid.TabIndex = 1
        '
        'frmMultiMetal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(750, 203)
        Me.Controls.Add(Me.txtGrid)
        Me.Controls.Add(Me.gridMultiMetal)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmMultiMetal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MultiMetal"
        CType(Me.gridMultiMetal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gridMultiMetal As System.Windows.Forms.DataGridView
    Friend WithEvents txtGrid As System.Windows.Forms.TextBox
End Class
