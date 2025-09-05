<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Status
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Status))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.lblConnectionType = New System.Windows.Forms.Label
        Me.lblConnectQuality = New System.Windows.Forms.Label
        Me.Dgv = New System.Windows.Forms.DataGridView
        Me.btnHide = New System.Windows.Forms.Button
        Me.btnStatus = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        CType(Me.Dgv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "ball7.ico")
        Me.ImageList1.Images.SetKeyName(1, "Ball8.ico")
        Me.ImageList1.Images.SetKeyName(2, "Ball9.ico")
        Me.ImageList1.Images.SetKeyName(3, "Ball15.ico")
        '
        'lblConnectionType
        '
        Me.lblConnectionType.AutoSize = True
        Me.lblConnectionType.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblConnectionType.ImageIndex = 1
        Me.lblConnectionType.ImageList = Me.ImageList1
        Me.lblConnectionType.Location = New System.Drawing.Point(12, 40)
        Me.lblConnectionType.Name = "lblConnectionType"
        Me.lblConnectionType.Size = New System.Drawing.Size(148, 13)
        Me.lblConnectionType.TabIndex = 2
        Me.lblConnectionType.Text = "          Connection Type:"
        Me.lblConnectionType.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'lblConnectQuality
        '
        Me.lblConnectQuality.AutoSize = True
        Me.lblConnectQuality.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConnectQuality.ForeColor = System.Drawing.Color.Red
        Me.lblConnectQuality.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblConnectQuality.ImageIndex = 3
        Me.lblConnectQuality.Location = New System.Drawing.Point(12, 15)
        Me.lblConnectQuality.Name = "lblConnectQuality"
        Me.lblConnectQuality.Size = New System.Drawing.Size(135, 14)
        Me.lblConnectQuality.TabIndex = 1
        Me.lblConnectQuality.Text = "Connection Quality:  Off"
        Me.lblConnectQuality.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Dgv
        '
        Me.Dgv.AllowUserToAddRows = False
        Me.Dgv.AllowUserToDeleteRows = False
        Me.Dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Dgv.Location = New System.Drawing.Point(12, 99)
        Me.Dgv.Name = "Dgv"
        Me.Dgv.RowHeadersVisible = False
        Me.Dgv.RowTemplate.Height = 18
        Me.Dgv.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Dgv.Size = New System.Drawing.Size(314, 159)
        Me.Dgv.TabIndex = 3
        '
        'btnHide
        '
        Me.btnHide.Location = New System.Drawing.Point(285, 6)
        Me.btnHide.Name = "btnHide"
        Me.btnHide.Size = New System.Drawing.Size(41, 30)
        Me.btnHide.TabIndex = 4
        Me.btnHide.Text = "Hide"
        Me.btnHide.UseVisualStyleBackColor = True
        '
        'btnStatus
        '
        Me.btnStatus.Location = New System.Drawing.Point(12, 67)
        Me.btnStatus.Name = "btnStatus"
        Me.btnStatus.Size = New System.Drawing.Size(135, 26)
        Me.btnStatus.TabIndex = 5
        Me.btnStatus.Text = "<< Hide Status"
        Me.btnStatus.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(311, 79)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox1.TabIndex = 6
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Status
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(337, 99)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.btnStatus)
        Me.Controls.Add(Me.btnHide)
        Me.Controls.Add(Me.Dgv)
        Me.Controls.Add(Me.lblConnectionType)
        Me.Controls.Add(Me.lblConnectQuality)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximizeBox = False
        Me.Name = "Status"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Status"
        CType(Me.Dgv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents lblConnectionType As System.Windows.Forms.Label
    Friend WithEvents lblConnectQuality As System.Windows.Forms.Label
    Friend WithEvents Dgv As System.Windows.Forms.DataGridView
    Friend WithEvents btnHide As System.Windows.Forms.Button
    Friend WithEvents btnStatus As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
End Class
