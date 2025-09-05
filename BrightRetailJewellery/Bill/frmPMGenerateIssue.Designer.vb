<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPMGenerateIssue
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
        Me.pnlHeader = New System.Windows.Forms.Panel
        Me.txtBillNo = New System.Windows.Forms.TextBox
        Me.btnOk = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.pnlgrid = New System.Windows.Forms.Panel
        Me.cmbPMSubType = New System.Windows.Forms.ComboBox
        Me.txtGrid = New System.Windows.Forms.TextBox
        Me.cmbPMType = New System.Windows.Forms.ComboBox
        Me.GridView = New System.Windows.Forms.DataGridView
        Me.pnlHeader.SuspendLayout()
        Me.pnlgrid.SuspendLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.txtBillNo)
        Me.pnlHeader.Controls.Add(Me.btnOk)
        Me.pnlHeader.Controls.Add(Me.Label1)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(612, 35)
        Me.pnlHeader.TabIndex = 0
        '
        'txtBillNo
        '
        Me.txtBillNo.Location = New System.Drawing.Point(96, 10)
        Me.txtBillNo.Name = "txtBillNo"
        Me.txtBillNo.Size = New System.Drawing.Size(116, 21)
        Me.txtBillNo.TabIndex = 1
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(383, 6)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(69, 23)
        Me.btnOk.TabIndex = 2
        Me.btnOk.Text = "OK"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(31, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Bill No :"
        '
        'pnlgrid
        '
        Me.pnlgrid.Controls.Add(Me.cmbPMSubType)
        Me.pnlgrid.Controls.Add(Me.txtGrid)
        Me.pnlgrid.Controls.Add(Me.cmbPMType)
        Me.pnlgrid.Controls.Add(Me.GridView)
        Me.pnlgrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlgrid.Location = New System.Drawing.Point(0, 35)
        Me.pnlgrid.Name = "pnlgrid"
        Me.pnlgrid.Size = New System.Drawing.Size(612, 197)
        Me.pnlgrid.TabIndex = 1
        '
        'cmbPMSubType
        '
        Me.cmbPMSubType.FormattingEnabled = True
        Me.cmbPMSubType.Location = New System.Drawing.Point(310, 35)
        Me.cmbPMSubType.Name = "cmbPMSubType"
        Me.cmbPMSubType.Size = New System.Drawing.Size(140, 21)
        Me.cmbPMSubType.TabIndex = 3
        Me.cmbPMSubType.Visible = False
        '
        'txtGrid
        '
        Me.txtGrid.Location = New System.Drawing.Point(466, 35)
        Me.txtGrid.Name = "txtGrid"
        Me.txtGrid.Size = New System.Drawing.Size(116, 21)
        Me.txtGrid.TabIndex = 2
        Me.txtGrid.Visible = False
        '
        'cmbPMType
        '
        Me.cmbPMType.FormattingEnabled = True
        Me.cmbPMType.Location = New System.Drawing.Point(143, 35)
        Me.cmbPMType.Name = "cmbPMType"
        Me.cmbPMType.Size = New System.Drawing.Size(140, 21)
        Me.cmbPMType.TabIndex = 1
        Me.cmbPMType.Visible = False
        '
        'GridView
        '
        Me.GridView.AllowUserToAddRows = False
        Me.GridView.AllowUserToDeleteRows = False
        Me.GridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.GridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridView.Location = New System.Drawing.Point(0, 0)
        Me.GridView.MultiSelect = False
        Me.GridView.Name = "GridView"
        Me.GridView.RowHeadersVisible = False
        Me.GridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.GridView.Size = New System.Drawing.Size(612, 197)
        Me.GridView.TabIndex = 0
        '
        'frmPMGenerateIssue
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(612, 232)
        Me.Controls.Add(Me.pnlgrid)
        Me.Controls.Add(Me.pnlHeader)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmPMGenerateIssue"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Compement Issue"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.pnlgrid.ResumeLayout(False)
        Me.pnlgrid.PerformLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents pnlgrid As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GridView As System.Windows.Forms.DataGridView
    Friend WithEvents cmbPMType As System.Windows.Forms.ComboBox
    Friend WithEvents txtGrid As System.Windows.Forms.TextBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents txtBillNo As System.Windows.Forms.TextBox
    Friend WithEvents cmbPMSubType As System.Windows.Forms.ComboBox
End Class
