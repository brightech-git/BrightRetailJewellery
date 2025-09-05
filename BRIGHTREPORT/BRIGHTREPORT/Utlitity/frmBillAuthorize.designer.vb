<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmbillauthorize
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.cmbtype = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.rbtAll = New System.Windows.Forms.RadioButton
        Me.rbtNotAuth = New System.Windows.Forms.RadioButton
        Me.rbtAuth = New System.Windows.Forms.RadioButton
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.gridview_OWN = New System.Windows.Forms.DataGridView
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.gridview_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnView_Search)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.cmbtype)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(761, 77)
        Me.Panel1.TabIndex = 0
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(385, 35)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 21
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(491, 35)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 22
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'cmbtype
        '
        Me.cmbtype.FormattingEnabled = True
        Me.cmbtype.Location = New System.Drawing.Point(70, 15)
        Me.cmbtype.Name = "cmbtype"
        Me.cmbtype.Size = New System.Drawing.Size(234, 21)
        Me.cmbtype.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Type"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rbtAll)
        Me.Panel3.Controls.Add(Me.rbtNotAuth)
        Me.Panel3.Controls.Add(Me.rbtAuth)
        Me.Panel3.Location = New System.Drawing.Point(70, 37)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(234, 20)
        Me.Panel3.TabIndex = 26
        '
        'rbtAll
        '
        Me.rbtAll.AutoSize = True
        Me.rbtAll.Checked = True
        Me.rbtAll.Location = New System.Drawing.Point(184, 1)
        Me.rbtAll.Name = "rbtAll"
        Me.rbtAll.Size = New System.Drawing.Size(47, 17)
        Me.rbtAll.TabIndex = 25
        Me.rbtAll.TabStop = True
        Me.rbtAll.Text = "Both"
        Me.rbtAll.UseVisualStyleBackColor = True
        '
        'rbtNotAuth
        '
        Me.rbtNotAuth.AutoSize = True
        Me.rbtNotAuth.Location = New System.Drawing.Point(83, 1)
        Me.rbtNotAuth.Name = "rbtNotAuth"
        Me.rbtNotAuth.Size = New System.Drawing.Size(95, 17)
        Me.rbtNotAuth.TabIndex = 24
        Me.rbtNotAuth.Text = "Not Authorized"
        Me.rbtNotAuth.UseVisualStyleBackColor = True
        '
        'rbtAuth
        '
        Me.rbtAuth.AutoSize = True
        Me.rbtAuth.Location = New System.Drawing.Point(2, 1)
        Me.rbtAuth.Name = "rbtAuth"
        Me.rbtAuth.Size = New System.Drawing.Size(75, 17)
        Me.rbtAuth.TabIndex = 23
        Me.rbtAuth.Text = "Authorized"
        Me.rbtAuth.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridview_OWN)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 77)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(761, 393)
        Me.Panel2.TabIndex = 1
        '
        'gridview_OWN
        '
        Me.gridview_OWN.AllowUserToAddRows = False
        Me.gridview_OWN.AllowUserToDeleteRows = False
        Me.gridview_OWN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridview_OWN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridview_OWN.Location = New System.Drawing.Point(0, 0)
        Me.gridview_OWN.Name = "gridview_OWN"
        Me.gridview_OWN.ReadOnly = True
        Me.gridview_OWN.RowHeadersVisible = False
        Me.gridview_OWN.RowTemplate.Height = 30
        Me.gridview_OWN.Size = New System.Drawing.Size(761, 393)
        Me.gridview_OWN.TabIndex = 0
        '
        'frmbillauthorize
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(761, 470)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmbillauthorize"
        Me.Text = "Bill_Authorize"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridview_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cmbtype As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents gridview_OWN As System.Windows.Forms.DataGridView
    Friend WithEvents rbtNotAuth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAuth As System.Windows.Forms.RadioButton
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents rbtAll As System.Windows.Forms.RadioButton
End Class
