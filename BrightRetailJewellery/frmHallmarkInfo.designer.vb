<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmHallmarkInfo
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.grpHallmarkDetails = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txthallmarkRowIndex = New System.Windows.Forms.TextBox()
        Me.txtTagWt_WET = New System.Windows.Forms.TextBox()
        Me.lblTagWt = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.gridHallmarkDetails = New System.Windows.Forms.DataGridView()
        Me.txtHallmarkNo = New System.Windows.Forms.TextBox()
        Me.grpHallmarkDetails.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.gridHallmarkDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpHallmarkDetails
        '
        Me.grpHallmarkDetails.Controls.Add(Me.Panel1)
        Me.grpHallmarkDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpHallmarkDetails.Location = New System.Drawing.Point(0, 0)
        Me.grpHallmarkDetails.Name = "grpHallmarkDetails"
        Me.grpHallmarkDetails.Size = New System.Drawing.Size(425, 174)
        Me.grpHallmarkDetails.TabIndex = 8
        Me.grpHallmarkDetails.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txthallmarkRowIndex)
        Me.Panel1.Controls.Add(Me.txtTagWt_WET)
        Me.Panel1.Controls.Add(Me.lblTagWt)
        Me.Panel1.Controls.Add(Me.Label15)
        Me.Panel1.Controls.Add(Me.gridHallmarkDetails)
        Me.Panel1.Controls.Add(Me.txtHallmarkNo)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 17)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(419, 154)
        Me.Panel1.TabIndex = 0
        '
        'txthallmarkRowIndex
        '
        Me.txthallmarkRowIndex.Location = New System.Drawing.Point(474, 17)
        Me.txthallmarkRowIndex.Name = "txthallmarkRowIndex"
        Me.txthallmarkRowIndex.Size = New System.Drawing.Size(43, 21)
        Me.txthallmarkRowIndex.TabIndex = 34
        Me.txthallmarkRowIndex.Visible = False
        '
        'txtTagWt_WET
        '
        Me.txtTagWt_WET.Location = New System.Drawing.Point(0, 22)
        Me.txtTagWt_WET.Name = "txtTagWt_WET"
        Me.txtTagWt_WET.Size = New System.Drawing.Size(218, 21)
        Me.txtTagWt_WET.TabIndex = 1
        Me.txtTagWt_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblTagWt
        '
        Me.lblTagWt.BackColor = System.Drawing.SystemColors.Control
        Me.lblTagWt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTagWt.Location = New System.Drawing.Point(0, 1)
        Me.lblTagWt.Name = "lblTagWt"
        Me.lblTagWt.Size = New System.Drawing.Size(218, 20)
        Me.lblTagWt.TabIndex = 0
        Me.lblTagWt.Text = "Tag Wt"
        Me.lblTagWt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.SystemColors.Control
        Me.Label15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label15.Location = New System.Drawing.Point(219, 1)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(199, 20)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "HallmarkNo"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gridHallmarkDetails
        '
        Me.gridHallmarkDetails.AllowUserToAddRows = False
        Me.gridHallmarkDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridHallmarkDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridHallmarkDetails.ColumnHeadersVisible = False
        Me.gridHallmarkDetails.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.gridHallmarkDetails.Location = New System.Drawing.Point(0, 44)
        Me.gridHallmarkDetails.Name = "gridHallmarkDetails"
        Me.gridHallmarkDetails.ReadOnly = True
        Me.gridHallmarkDetails.RowHeadersVisible = False
        Me.gridHallmarkDetails.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridHallmarkDetails.RowTemplate.Height = 20
        Me.gridHallmarkDetails.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridHallmarkDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridHallmarkDetails.Size = New System.Drawing.Size(419, 110)
        Me.gridHallmarkDetails.TabIndex = 4
        '
        'txtHallmarkNo
        '
        Me.txtHallmarkNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtHallmarkNo.Location = New System.Drawing.Point(219, 22)
        Me.txtHallmarkNo.Name = "txtHallmarkNo"
        Me.txtHallmarkNo.Size = New System.Drawing.Size(199, 21)
        Me.txtHallmarkNo.TabIndex = 3
        '
        'frmHallmarkInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(425, 174)
        Me.Controls.Add(Me.grpHallmarkDetails)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmHallmarkInfo"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Hallmark Info"
        Me.grpHallmarkDetails.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.gridHallmarkDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpHallmarkDetails As GroupBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents txtTagWt_WET As TextBox
    Friend WithEvents lblTagWt As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents gridHallmarkDetails As DataGridView
    Friend WithEvents txtHallmarkNo As TextBox
    Friend WithEvents txthallmarkRowIndex As TextBox
End Class
