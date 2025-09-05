<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DB_Attach
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.btnBrowseSource = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtSourcePath = New System.Windows.Forms.TextBox
        Me.btnAttach = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.txtDestinationPath = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnBrowseDest = New System.Windows.Forms.Button
        Me.gridFileView = New System.Windows.Forms.DataGridView
        CType(Me.gridFileView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnBrowseSource
        '
        Me.btnBrowseSource.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseSource.ForeColor = System.Drawing.Color.Red
        Me.btnBrowseSource.Location = New System.Drawing.Point(421, 29)
        Me.btnBrowseSource.Name = "btnBrowseSource"
        Me.btnBrowseSource.Size = New System.Drawing.Size(28, 23)
        Me.btnBrowseSource.TabIndex = 2
        Me.btnBrowseSource.Text = ".."
        Me.btnBrowseSource.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(108, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Source File Path :"
        '
        'txtSourcePath
        '
        Me.txtSourcePath.Location = New System.Drawing.Point(22, 29)
        Me.txtSourcePath.Name = "txtSourcePath"
        Me.txtSourcePath.Size = New System.Drawing.Size(395, 21)
        Me.txtSourcePath.TabIndex = 1
        '
        'btnAttach
        '
        Me.btnAttach.Location = New System.Drawing.Point(151, 160)
        Me.btnAttach.Name = "btnAttach"
        Me.btnAttach.Size = New System.Drawing.Size(100, 30)
        Me.btnAttach.TabIndex = 7
        Me.btnAttach.Text = "&Attach"
        Me.btnAttach.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(257, 160)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 8
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'txtDestinationPath
        '
        Me.txtDestinationPath.Location = New System.Drawing.Point(22, 133)
        Me.txtDestinationPath.Name = "txtDestinationPath"
        Me.txtDestinationPath.Size = New System.Drawing.Size(395, 21)
        Me.txtDestinationPath.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(22, 117)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(148, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Destination Folder Path :"
        '
        'btnBrowseDest
        '
        Me.btnBrowseDest.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowseDest.ForeColor = System.Drawing.Color.Red
        Me.btnBrowseDest.Location = New System.Drawing.Point(421, 133)
        Me.btnBrowseDest.Name = "btnBrowseDest"
        Me.btnBrowseDest.Size = New System.Drawing.Size(28, 23)
        Me.btnBrowseDest.TabIndex = 6
        Me.btnBrowseDest.Text = ".."
        Me.btnBrowseDest.UseVisualStyleBackColor = True
        '
        'gridFileView
        '
        Me.gridFileView.AllowUserToAddRows = False
        Me.gridFileView.AllowUserToDeleteRows = False
        Me.gridFileView.BackgroundColor = System.Drawing.SystemColors.Window
        Me.gridFileView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridFileView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.gridFileView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridFileView.DefaultCellStyle = DataGridViewCellStyle1
        Me.gridFileView.Location = New System.Drawing.Point(22, 56)
        Me.gridFileView.Name = "gridFileView"
        Me.gridFileView.ReadOnly = True
        Me.gridFileView.RowHeadersVisible = False
        Me.gridFileView.RowTemplate.Height = 18
        Me.gridFileView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridFileView.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.gridFileView.Size = New System.Drawing.Size(395, 57)
        Me.gridFileView.TabIndex = 3
        '
        'Attach_DB
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(461, 200)
        Me.Controls.Add(Me.gridFileView)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnAttach)
        Me.Controls.Add(Me.btnBrowseDest)
        Me.Controls.Add(Me.btnBrowseSource)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtDestinationPath)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtSourcePath)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Attach_DB"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Attach"
        CType(Me.gridFileView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnBrowseSource As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtSourcePath As System.Windows.Forms.TextBox
    Friend WithEvents btnAttach As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents txtDestinationPath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnBrowseDest As System.Windows.Forms.Button
    Friend WithEvents gridFileView As System.Windows.Forms.DataGridView
End Class
