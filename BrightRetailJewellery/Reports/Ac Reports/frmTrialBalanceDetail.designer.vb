<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTrialBalanceDetail
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
        Me.grpDetailView = New System.Windows.Forms.GroupBox
        Me.gridDetailView = New System.Windows.Forms.DataGridView
        Me.gridDetailViewHead = New System.Windows.Forms.DataGridView
        Me.pnlDetailHeading = New System.Windows.Forms.Panel
        Me.lbltitledet = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.grpDetailView.SuspendLayout()
        CType(Me.gridDetailView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridDetailViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlDetailHeading.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpDetailView
        '
        Me.grpDetailView.Controls.Add(Me.gridDetailView)
        Me.grpDetailView.Controls.Add(Me.gridDetailViewHead)
        Me.grpDetailView.Controls.Add(Me.pnlDetailHeading)
        Me.grpDetailView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpDetailView.Location = New System.Drawing.Point(0, 0)
        Me.grpDetailView.Name = "grpDetailView"
        Me.grpDetailView.Size = New System.Drawing.Size(878, 313)
        Me.grpDetailView.TabIndex = 2
        Me.grpDetailView.TabStop = False
        '
        'gridDetailView
        '
        Me.gridDetailView.AllowUserToAddRows = False
        Me.gridDetailView.AllowUserToDeleteRows = False
        Me.gridDetailView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridDetailView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridDetailView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridDetailView.Location = New System.Drawing.Point(3, 57)
        Me.gridDetailView.Name = "gridDetailView"
        Me.gridDetailView.ReadOnly = True
        Me.gridDetailView.RowHeadersVisible = False
        Me.gridDetailView.RowTemplate.Height = 18
        Me.gridDetailView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridDetailView.Size = New System.Drawing.Size(872, 253)
        Me.gridDetailView.TabIndex = 0
        '
        'gridDetailViewHead
        '
        Me.gridDetailViewHead.AllowUserToAddRows = False
        Me.gridDetailViewHead.AllowUserToDeleteRows = False
        Me.gridDetailViewHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridDetailViewHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridDetailViewHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridDetailViewHead.Location = New System.Drawing.Point(3, 37)
        Me.gridDetailViewHead.Name = "gridDetailViewHead"
        Me.gridDetailViewHead.ReadOnly = True
        Me.gridDetailViewHead.RowHeadersVisible = False
        Me.gridDetailViewHead.Size = New System.Drawing.Size(872, 20)
        Me.gridDetailViewHead.TabIndex = 2
        '
        'pnlDetailHeading
        '
        Me.pnlDetailHeading.Controls.Add(Me.lbltitledet)
        Me.pnlDetailHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlDetailHeading.Location = New System.Drawing.Point(3, 17)
        Me.pnlDetailHeading.Name = "pnlDetailHeading"
        Me.pnlDetailHeading.Size = New System.Drawing.Size(872, 20)
        Me.pnlDetailHeading.TabIndex = 1
        '
        'lbltitledet
        '
        Me.lbltitledet.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbltitledet.Location = New System.Drawing.Point(0, 0)
        Me.lbltitledet.Name = "lbltitledet"
        Me.lbltitledet.Size = New System.Drawing.Size(872, 20)
        Me.lbltitledet.TabIndex = 2
        Me.lbltitledet.Text = "DETAILTITLE"
        Me.lbltitledet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbltitledet.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 282)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(878, 31)
        Me.Panel1.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(235, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "PRESS 'X' - EXPORT   'P' - PRINTING"
        '
        'frmTrialBalanceDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(878, 313)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.grpDetailView)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmTrialBalanceDetail"
        Me.Text = "frmTrialBalanceDetail"
        Me.grpDetailView.ResumeLayout(False)
        CType(Me.gridDetailView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridDetailViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlDetailHeading.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpDetailView As System.Windows.Forms.GroupBox
    Friend WithEvents gridDetailViewHead As System.Windows.Forms.DataGridView
    Friend WithEvents gridDetailView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlDetailHeading As System.Windows.Forms.Panel
    Friend WithEvents lbltitledet As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
