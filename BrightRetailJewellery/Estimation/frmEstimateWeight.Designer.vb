<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEstimateWeight
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
        Me.components = New System.ComponentModel.Container()
        Me.grpEstimateWeight = New CodeVendor.Controls.Grouper()
        Me.txtDescription_OWN = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtEstPcs_NUM = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpWtMachingDet = New System.Windows.Forms.GroupBox()
        Me.Label78 = New System.Windows.Forms.Label()
        Me.Label77 = New System.Windows.Forms.Label()
        Me.ReadData = New System.Windows.Forms.Label()
        Me.Label76 = New System.Windows.Forms.Label()
        Me.ModifyData = New System.Windows.Forms.Label()
        Me.SplitData = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.txtEstWt_WET = New System.Windows.Forms.TextBox()
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WeighingScalePropertyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grpEstimateWeight.SuspendLayout()
        Me.grpWtMachingDet.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpEstimateWeight
        '
        Me.grpEstimateWeight.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpEstimateWeight.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpEstimateWeight.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpEstimateWeight.BorderColor = System.Drawing.Color.Transparent
        Me.grpEstimateWeight.BorderThickness = 1.0!
        Me.grpEstimateWeight.Controls.Add(Me.txtDescription_OWN)
        Me.grpEstimateWeight.Controls.Add(Me.Label4)
        Me.grpEstimateWeight.Controls.Add(Me.txtEstPcs_NUM)
        Me.grpEstimateWeight.Controls.Add(Me.Label3)
        Me.grpEstimateWeight.Controls.Add(Me.Label2)
        Me.grpEstimateWeight.Controls.Add(Me.cmbMetal)
        Me.grpEstimateWeight.Controls.Add(Me.Label1)
        Me.grpEstimateWeight.Controls.Add(Me.grpWtMachingDet)
        Me.grpEstimateWeight.Controls.Add(Me.btnExit)
        Me.grpEstimateWeight.Controls.Add(Me.btnSave)
        Me.grpEstimateWeight.Controls.Add(Me.btnNew)
        Me.grpEstimateWeight.Controls.Add(Me.Label49)
        Me.grpEstimateWeight.Controls.Add(Me.txtEstWt_WET)
        Me.grpEstimateWeight.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpEstimateWeight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpEstimateWeight.GroupImage = Nothing
        Me.grpEstimateWeight.GroupTitle = ""
        Me.grpEstimateWeight.Location = New System.Drawing.Point(0, 0)
        Me.grpEstimateWeight.Name = "grpEstimateWeight"
        Me.grpEstimateWeight.Padding = New System.Windows.Forms.Padding(20)
        Me.grpEstimateWeight.PaintGroupBox = False
        Me.grpEstimateWeight.RoundCorners = 10
        Me.grpEstimateWeight.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpEstimateWeight.ShadowControl = False
        Me.grpEstimateWeight.ShadowThickness = 3
        Me.grpEstimateWeight.Size = New System.Drawing.Size(345, 227)
        Me.grpEstimateWeight.TabIndex = 0
        '
        'txtDescription_OWN
        '
        Me.txtDescription_OWN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription_OWN.Location = New System.Drawing.Point(118, 58)
        Me.txtDescription_OWN.MaxLength = 35
        Me.txtDescription_OWN.Name = "txtDescription_OWN"
        Me.txtDescription_OWN.Size = New System.Drawing.Size(206, 22)
        Me.txtDescription_OWN.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(18, 62)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 14)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Description"
        '
        'txtEstPcs_NUM
        '
        Me.txtEstPcs_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEstPcs_NUM.Location = New System.Drawing.Point(118, 92)
        Me.txtEstPcs_NUM.MaxLength = 50
        Me.txtEstPcs_NUM.Name = "txtEstPcs_NUM"
        Me.txtEstPcs_NUM.Size = New System.Drawing.Size(206, 22)
        Me.txtEstPcs_NUM.TabIndex = 5
        Me.txtEstPcs_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(18, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(30, 14)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Pcs"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(120, 205)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(204, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "* Press Space to Refresh Weight *"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbMetal
        '
        Me.cmbMetal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(118, 27)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(206, 21)
        Me.cmbMetal.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(18, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 14)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Metal"
        '
        'grpWtMachingDet
        '
        Me.grpWtMachingDet.Controls.Add(Me.Label78)
        Me.grpWtMachingDet.Controls.Add(Me.Label77)
        Me.grpWtMachingDet.Controls.Add(Me.ReadData)
        Me.grpWtMachingDet.Controls.Add(Me.Label76)
        Me.grpWtMachingDet.Controls.Add(Me.ModifyData)
        Me.grpWtMachingDet.Controls.Add(Me.SplitData)
        Me.grpWtMachingDet.Location = New System.Drawing.Point(419, 34)
        Me.grpWtMachingDet.Name = "grpWtMachingDet"
        Me.grpWtMachingDet.Size = New System.Drawing.Size(142, 88)
        Me.grpWtMachingDet.TabIndex = 12
        Me.grpWtMachingDet.TabStop = False
        Me.grpWtMachingDet.Text = "Weighing Scale Detail"
        '
        'Label78
        '
        Me.Label78.AutoSize = True
        Me.Label78.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label78.Location = New System.Drawing.Point(3, 13)
        Me.Label78.Name = "Label78"
        Me.Label78.Size = New System.Drawing.Size(54, 12)
        Me.Label78.TabIndex = 0
        Me.Label78.Text = "ReadData"
        '
        'Label77
        '
        Me.Label77.AutoSize = True
        Me.Label77.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label77.Location = New System.Drawing.Point(3, 61)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(62, 12)
        Me.Label77.TabIndex = 4
        Me.Label77.Text = "ModifyData"
        '
        'ReadData
        '
        Me.ReadData.AutoSize = True
        Me.ReadData.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ReadData.Location = New System.Drawing.Point(3, 25)
        Me.ReadData.Name = "ReadData"
        Me.ReadData.Size = New System.Drawing.Size(56, 12)
        Me.ReadData.TabIndex = 1
        Me.ReadData.Text = "ReadDAta"
        '
        'Label76
        '
        Me.Label76.AutoSize = True
        Me.Label76.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label76.Location = New System.Drawing.Point(3, 38)
        Me.Label76.Name = "Label76"
        Me.Label76.Size = New System.Drawing.Size(52, 12)
        Me.Label76.TabIndex = 2
        Me.Label76.Text = "SplitData"
        '
        'ModifyData
        '
        Me.ModifyData.AutoSize = True
        Me.ModifyData.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ModifyData.Location = New System.Drawing.Point(3, 73)
        Me.ModifyData.Name = "ModifyData"
        Me.ModifyData.Size = New System.Drawing.Size(65, 12)
        Me.ModifyData.TabIndex = 5
        Me.ModifyData.Text = "MOdifyData"
        '
        'SplitData
        '
        Me.SplitData.AutoSize = True
        Me.SplitData.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SplitData.Location = New System.Drawing.Point(3, 49)
        Me.SplitData.Name = "SplitData"
        Me.SplitData.Size = New System.Drawing.Size(52, 12)
        Me.SplitData.TabIndex = 3
        Me.SplitData.Text = "SplitData"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(224, 164)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 10
        Me.btnExit.Text = "&Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(12, 164)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "&Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(118, 164)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 9
        Me.btnNew.Text = "&New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(18, 128)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(54, 14)
        Me.Label49.TabIndex = 6
        Me.Label49.Text = "Weight"
        '
        'txtEstWt_WET
        '
        Me.txtEstWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEstWt_WET.Location = New System.Drawing.Point(118, 124)
        Me.txtEstWt_WET.MaxLength = 50
        Me.txtEstWt_WET.Name = "txtEstWt_WET"
        Me.txtEstWt_WET.Size = New System.Drawing.Size(206, 22)
        Me.txtEstWt_WET.TabIndex = 7
        Me.txtEstWt_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.WeighingScalePropertyToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(204, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'WeighingScalePropertyToolStripMenuItem
        '
        Me.WeighingScalePropertyToolStripMenuItem.Name = "WeighingScalePropertyToolStripMenuItem"
        Me.WeighingScalePropertyToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.WeighingScalePropertyToolStripMenuItem.Text = "Weighing Scale Property"
        '
        'frmEstimateWeight
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(345, 227)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpEstimateWeight)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmEstimateWeight"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Estimate Weight"
        Me.grpEstimateWeight.ResumeLayout(False)
        Me.grpEstimateWeight.PerformLayout()
        Me.grpWtMachingDet.ResumeLayout(False)
        Me.grpWtMachingDet.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpEstimateWeight As CodeVendor.Controls.Grouper
    Friend WithEvents Label49 As Label
    Friend WithEvents txtEstWt_WET As TextBox
    Friend WithEvents btnSave As Button
    Friend WithEvents btnNew As Button
    Friend WithEvents btnExit As Button
    Friend WithEvents SerialPort1 As IO.Ports.SerialPort
    Friend WithEvents grpWtMachingDet As GroupBox
    Friend WithEvents Label78 As Label
    Friend WithEvents Label77 As Label
    Friend WithEvents ReadData As Label
    Friend WithEvents Label76 As Label
    Friend WithEvents ModifyData As Label
    Friend WithEvents SplitData As Label
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents WeighingScalePropertyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbMetal As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtEstPcs_NUM As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtDescription_OWN As TextBox
    Friend WithEvents Label4 As Label
End Class
