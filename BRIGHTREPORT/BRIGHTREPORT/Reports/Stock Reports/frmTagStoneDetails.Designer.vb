<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTagStoneDetails
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
        Me.grpStoneDetails = New System.Windows.Forms.GroupBox
        Me.gridStoneDetails = New System.Windows.Forms.DataGridView
        Me.lblTitle = New System.Windows.Forms.Label
        Me.grpStoneDetails.SuspendLayout()
        CType(Me.gridStoneDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpStoneDetails
        '
        Me.grpStoneDetails.Controls.Add(Me.gridStoneDetails)
        Me.grpStoneDetails.Location = New System.Drawing.Point(2, 26)
        Me.grpStoneDetails.Name = "grpStoneDetails"
        Me.grpStoneDetails.Size = New System.Drawing.Size(743, 185)
        Me.grpStoneDetails.TabIndex = 4
        Me.grpStoneDetails.TabStop = False
        '
        'gridStoneDetails
        '
        Me.gridStoneDetails.AllowUserToAddRows = False
        Me.gridStoneDetails.AllowUserToDeleteRows = False
        Me.gridStoneDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridStoneDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridStoneDetails.Location = New System.Drawing.Point(11, 14)
        Me.gridStoneDetails.MultiSelect = False
        Me.gridStoneDetails.Name = "gridStoneDetails"
        Me.gridStoneDetails.ReadOnly = True
        Me.gridStoneDetails.RowHeadersVisible = False
        Me.gridStoneDetails.RowTemplate.Height = 20
        Me.gridStoneDetails.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridStoneDetails.Size = New System.Drawing.Size(719, 165)
        Me.gridStoneDetails.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Location = New System.Drawing.Point(34, 10)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(683, 13)
        Me.lblTitle.TabIndex = 5
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmTagStoneDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(746, 215)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.grpStoneDetails)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmTagStoneDetails"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmTagStoneDetails"
        Me.grpStoneDetails.ResumeLayout(False)
        CType(Me.gridStoneDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpStoneDetails As System.Windows.Forms.GroupBox
    Friend WithEvents gridStoneDetails As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
End Class
