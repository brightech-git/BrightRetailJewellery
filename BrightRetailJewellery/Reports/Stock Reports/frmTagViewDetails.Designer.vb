<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTagViewDetails
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
        Me.pnlTagView = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnOk = New System.Windows.Forms.Button
        Me.gridviewTag = New System.Windows.Forms.DataGridView
        Me.pnlTagView.SuspendLayout()
        CType(Me.gridviewTag, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlTagView
        '
        Me.pnlTagView.Controls.Add(Me.Label1)
        Me.pnlTagView.Controls.Add(Me.btnOk)
        Me.pnlTagView.Controls.Add(Me.gridviewTag)
        Me.pnlTagView.Location = New System.Drawing.Point(4, 5)
        Me.pnlTagView.Name = "pnlTagView"
        Me.pnlTagView.Size = New System.Drawing.Size(573, 382)
        Me.pnlTagView.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(246, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Tag Details"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(487, 343)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 31)
        Me.btnOk.TabIndex = 1
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'gridviewTag
        '
        Me.gridviewTag.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridviewTag.Location = New System.Drawing.Point(3, 28)
        Me.gridviewTag.Name = "gridviewTag"
        Me.gridviewTag.ReadOnly = True
        Me.gridviewTag.RowHeadersVisible = False
        Me.gridviewTag.Size = New System.Drawing.Size(567, 308)
        Me.gridviewTag.TabIndex = 0
        '
        'frmTagViewDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(582, 395)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlTagView)
        Me.KeyPreview = True
        Me.Name = "frmTagViewDetails"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TagView Details"
        Me.pnlTagView.ResumeLayout(False)
        Me.pnlTagView.PerformLayout()
        CType(Me.gridviewTag, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlTagView As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents gridviewTag As System.Windows.Forms.DataGridView
End Class
