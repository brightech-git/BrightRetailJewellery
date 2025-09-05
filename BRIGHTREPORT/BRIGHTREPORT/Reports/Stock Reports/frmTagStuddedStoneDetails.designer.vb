<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTagStuddedStoneDetails
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
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.grpStudDetails = New System.Windows.Forms.GroupBox
        Me.pnlGrid = New System.Windows.Forms.Panel
        Me.gridStudDetails = New System.Windows.Forms.DataGridView
        Me.gridStudDetailsHead = New System.Windows.Forms.DataGridView
        Me.lblTitle = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.grpStudDetails.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        CType(Me.gridStudDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridStudDetailsHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpStudDetails
        '
        Me.grpStudDetails.Controls.Add(Me.pnlGrid)
        Me.grpStudDetails.Location = New System.Drawing.Point(3, 86)
        Me.grpStudDetails.Name = "grpStudDetails"
        Me.grpStudDetails.Size = New System.Drawing.Size(813, 250)
        Me.grpStudDetails.TabIndex = 9
        Me.grpStudDetails.TabStop = False
        Me.grpStudDetails.Text = "STUD DETAILS"
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.gridStudDetails)
        Me.pnlGrid.Controls.Add(Me.gridStudDetailsHead)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(3, 17)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(807, 230)
        Me.pnlGrid.TabIndex = 4
        '
        'gridStudDetails
        '
        Me.gridStudDetails.AllowUserToAddRows = False
        Me.gridStudDetails.AllowUserToDeleteRows = False
        Me.gridStudDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridStudDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridStudDetails.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.gridStudDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridStudDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridStudDetails.Location = New System.Drawing.Point(0, 20)
        Me.gridStudDetails.MultiSelect = False
        Me.gridStudDetails.Name = "gridStudDetails"
        Me.gridStudDetails.ReadOnly = True
        Me.gridStudDetails.RowHeadersVisible = False
        Me.gridStudDetails.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridStudDetails.RowTemplate.Height = 20
        Me.gridStudDetails.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridStudDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridStudDetails.Size = New System.Drawing.Size(807, 210)
        Me.gridStudDetails.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.gridStudDetails, "<ESC> EXIT")
        '
        'gridStudDetailsHead
        '
        Me.gridStudDetailsHead.AllowUserToAddRows = False
        Me.gridStudDetailsHead.AllowUserToDeleteRows = False
        Me.gridStudDetailsHead.AllowUserToResizeColumns = False
        Me.gridStudDetailsHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridStudDetailsHead.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridStudDetailsHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridStudDetailsHead.DefaultCellStyle = DataGridViewCellStyle6
        Me.gridStudDetailsHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridStudDetailsHead.Location = New System.Drawing.Point(0, 0)
        Me.gridStudDetailsHead.Name = "gridStudDetailsHead"
        Me.gridStudDetailsHead.ReadOnly = True
        Me.gridStudDetailsHead.Size = New System.Drawing.Size(807, 20)
        Me.gridStudDetailsHead.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Location = New System.Drawing.Point(93, 9)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(567, 24)
        Me.lblTitle.TabIndex = 10
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(608, 51)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(99, 29)
        Me.btnPrint.TabIndex = 12
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(503, 51)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(99, 29)
        Me.btnExport.TabIndex = 11
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'frmTagStuddedStoneDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(820, 341)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.grpStudDetails)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmTagStuddedStoneDetails"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.grpStudDetails.ResumeLayout(False)
        Me.pnlGrid.ResumeLayout(False)
        CType(Me.gridStudDetails, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridStudDetailsHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpStudDetails As System.Windows.Forms.GroupBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents gridStudDetails As System.Windows.Forms.DataGridView
    Friend WithEvents gridStudDetailsHead As System.Windows.Forms.DataGridView
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
End Class
