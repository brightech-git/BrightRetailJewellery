<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DB_Bakup
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
        Me.chkLstDataBases = New System.Windows.Forms.CheckedListBox
        Me.btnLoad = New System.Windows.Forms.Button
        Me.btnBackUp = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.txtBackUpPath = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnBrowsePath = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.pBar = New System.Windows.Forms.ProgressBar
        Me.gridStatus = New System.Windows.Forms.DataGridView
        Me.rbtDayWise = New System.Windows.Forms.RadioButton
        Me.rbtTimeWise = New System.Windows.Forms.RadioButton
        CType(Me.gridStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chkLstDataBases
        '
        Me.chkLstDataBases.FormattingEnabled = True
        Me.chkLstDataBases.Location = New System.Drawing.Point(27, 71)
        Me.chkLstDataBases.Name = "chkLstDataBases"
        Me.chkLstDataBases.Size = New System.Drawing.Size(367, 196)
        Me.chkLstDataBases.TabIndex = 4
        '
        'btnLoad
        '
        Me.btnLoad.Location = New System.Drawing.Point(27, 22)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(100, 30)
        Me.btnLoad.TabIndex = 0
        Me.btnLoad.Text = "&Load"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'btnBackUp
        '
        Me.btnBackUp.Location = New System.Drawing.Point(27, 401)
        Me.btnBackUp.Name = "btnBackUp"
        Me.btnBackUp.Size = New System.Drawing.Size(100, 30)
        Me.btnBackUp.TabIndex = 10
        Me.btnBackUp.Text = "&BackUp"
        Me.btnBackUp.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(133, 401)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 11
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'txtBackUpPath
        '
        Me.txtBackUpPath.Location = New System.Drawing.Point(27, 286)
        Me.txtBackUpPath.Name = "txtBackUpPath"
        Me.txtBackUpPath.Size = New System.Drawing.Size(336, 21)
        Me.txtBackUpPath.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(27, 270)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "BackUp Path :"
        '
        'btnBrowsePath
        '
        Me.btnBrowsePath.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowsePath.ForeColor = System.Drawing.Color.Red
        Me.btnBrowsePath.Location = New System.Drawing.Point(366, 285)
        Me.btnBrowsePath.Name = "btnBrowsePath"
        Me.btnBrowsePath.Size = New System.Drawing.Size(28, 23)
        Me.btnBrowsePath.TabIndex = 7
        Me.btnBrowsePath.Text = ".."
        Me.btnBrowsePath.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(27, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Databases :"
        '
        'pBar
        '
        Me.pBar.Location = New System.Drawing.Point(27, 382)
        Me.pBar.Name = "pBar"
        Me.pBar.Size = New System.Drawing.Size(367, 13)
        Me.pBar.TabIndex = 9
        '
        'gridStatus
        '
        Me.gridStatus.AllowUserToAddRows = False
        Me.gridStatus.AllowUserToDeleteRows = False
        Me.gridStatus.BackgroundColor = System.Drawing.SystemColors.Window
        Me.gridStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridStatus.DefaultCellStyle = DataGridViewCellStyle1
        Me.gridStatus.GridColor = System.Drawing.Color.Gainsboro
        Me.gridStatus.Location = New System.Drawing.Point(27, 313)
        Me.gridStatus.Name = "gridStatus"
        Me.gridStatus.ReadOnly = True
        Me.gridStatus.RowHeadersVisible = False
        Me.gridStatus.RowTemplate.Height = 16
        Me.gridStatus.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridStatus.Size = New System.Drawing.Size(367, 63)
        Me.gridStatus.TabIndex = 8
        '
        'rbtDayWise
        '
        Me.rbtDayWise.AutoSize = True
        Me.rbtDayWise.Location = New System.Drawing.Point(237, 29)
        Me.rbtDayWise.Name = "rbtDayWise"
        Me.rbtDayWise.Size = New System.Drawing.Size(75, 17)
        Me.rbtDayWise.TabIndex = 2
        Me.rbtDayWise.TabStop = True
        Me.rbtDayWise.Text = "DayWise"
        Me.rbtDayWise.UseVisualStyleBackColor = True
        '
        'rbtTimeWise
        '
        Me.rbtTimeWise.AutoSize = True
        Me.rbtTimeWise.Location = New System.Drawing.Point(149, 29)
        Me.rbtTimeWise.Name = "rbtTimeWise"
        Me.rbtTimeWise.Size = New System.Drawing.Size(84, 17)
        Me.rbtTimeWise.TabIndex = 1
        Me.rbtTimeWise.TabStop = True
        Me.rbtTimeWise.Text = "Time Wise"
        Me.rbtTimeWise.UseVisualStyleBackColor = True
        '
        'Bakup_DB
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(416, 450)
        Me.Controls.Add(Me.rbtTimeWise)
        Me.Controls.Add(Me.rbtDayWise)
        Me.Controls.Add(Me.gridStatus)
        Me.Controls.Add(Me.pBar)
        Me.Controls.Add(Me.btnBrowsePath)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtBackUpPath)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnBackUp)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.chkLstDataBases)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Bakup_DB"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BackUp"
        CType(Me.gridStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkLstDataBases As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents btnBackUp As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnBrowsePath As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents pBar As System.Windows.Forms.ProgressBar
    Friend WithEvents gridStatus As System.Windows.Forms.DataGridView
    Friend WithEvents rbtDayWise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTimeWise As System.Windows.Forms.RadioButton
    Public WithEvents txtBackUpPath As System.Windows.Forms.TextBox

End Class
