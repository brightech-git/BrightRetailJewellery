<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmCategoryWeightTransfer
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
        Me.components = New System.ComponentModel.Container()
        Me.btnTransfer = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbCatNamefrom = New System.Windows.Forms.ComboBox()
        Me.cmbCategoryTo = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnnew = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtCatGrsWt_Wet = New System.Windows.Forms.TextBox()
        Me.txtCatNetWt_Wet = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtCatPcs_NUM = New System.Windows.Forms.TextBox()
        Me.cmbCostCentre_MAN = New System.Windows.Forms.ComboBox()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnTransfer
        '
        Me.btnTransfer.Location = New System.Drawing.Point(114, 173)
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.Size = New System.Drawing.Size(114, 32)
        Me.btnTransfer.TabIndex = 14
        Me.btnTransfer.Text = "&Transfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(387, 173)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(114, 32)
        Me.btnExit.TabIndex = 16
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(1, 40)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 14)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Date"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem1})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(225, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'NewToolStripMenuItem1
        '
        Me.NewToolStripMenuItem1.Name = "NewToolStripMenuItem1"
        Me.NewToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem1.Size = New System.Drawing.Size(224, 22)
        Me.NewToolStripMenuItem1.Text = "NewToolStripMenuItem1"
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(114, 36)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(118, 22)
        Me.dtpFrom.TabIndex = 3
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(1, 12)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(84, 14)
        Me.Label.TabIndex = 0
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(1, 64)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(106, 21)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Category from"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCatNamefrom
        '
        Me.cmbCatNamefrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCatNamefrom.FormattingEnabled = True
        Me.cmbCatNamefrom.Location = New System.Drawing.Point(114, 63)
        Me.cmbCatNamefrom.Name = "cmbCatNamefrom"
        Me.cmbCatNamefrom.Size = New System.Drawing.Size(386, 22)
        Me.cmbCatNamefrom.TabIndex = 5
        '
        'cmbCategoryTo
        '
        Me.cmbCategoryTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCategoryTo.FormattingEnabled = True
        Me.cmbCategoryTo.Location = New System.Drawing.Point(114, 90)
        Me.cmbCategoryTo.Name = "cmbCategoryTo"
        Me.cmbCategoryTo.Size = New System.Drawing.Size(386, 22)
        Me.cmbCategoryTo.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(1, 91)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 21)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Category to"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnnew
        '
        Me.btnnew.Location = New System.Drawing.Point(249, 174)
        Me.btnnew.Name = "btnnew"
        Me.btnnew.Size = New System.Drawing.Size(114, 32)
        Me.btnnew.TabIndex = 15
        Me.btnnew.Text = "New[F3]"
        Me.btnnew.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(1, 148)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 14)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Grs Wt"
        '
        'txtCatGrsWt_Wet
        '
        Me.txtCatGrsWt_Wet.Location = New System.Drawing.Point(114, 144)
        Me.txtCatGrsWt_Wet.Name = "txtCatGrsWt_Wet"
        Me.txtCatGrsWt_Wet.Size = New System.Drawing.Size(118, 22)
        Me.txtCatGrsWt_Wet.TabIndex = 11
        Me.txtCatGrsWt_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCatNetWt_Wet
        '
        Me.txtCatNetWt_Wet.Location = New System.Drawing.Point(293, 144)
        Me.txtCatNetWt_Wet.Name = "txtCatNetWt_Wet"
        Me.txtCatNetWt_Wet.Size = New System.Drawing.Size(118, 22)
        Me.txtCatNetWt_Wet.TabIndex = 13
        Me.txtCatNetWt_Wet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(238, 148)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 14)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Net Wt"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(1, 121)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(30, 14)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Pcs"
        '
        'txtCatPcs_NUM
        '
        Me.txtCatPcs_NUM.Location = New System.Drawing.Point(114, 117)
        Me.txtCatPcs_NUM.Name = "txtCatPcs_NUM"
        Me.txtCatPcs_NUM.Size = New System.Drawing.Size(118, 22)
        Me.txtCatPcs_NUM.TabIndex = 9
        Me.txtCatPcs_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmbCostCentre_MAN
        '
        Me.cmbCostCentre_MAN.FormattingEnabled = True
        Me.cmbCostCentre_MAN.Location = New System.Drawing.Point(114, 8)
        Me.cmbCostCentre_MAN.Name = "cmbCostCentre_MAN"
        Me.cmbCostCentre_MAN.Size = New System.Drawing.Size(387, 22)
        Me.cmbCostCentre_MAN.TabIndex = 1
        '
        'frmCategoryWeightTransfer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(532, 214)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.cmbCostCentre_MAN)
        Me.Controls.Add(Me.txtCatPcs_NUM)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtCatNetWt_Wet)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtCatGrsWt_Wet)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cmbCategoryTo)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmbCatNamefrom)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label)
        Me.Controls.Add(Me.dtpFrom)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnnew)
        Me.Controls.Add(Me.btnTransfer)
        Me.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmCategoryWeightTransfer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Category Transfer"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnTransfer As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbCatNamefrom As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCategoryTo As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnnew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label4 As Label
    Friend WithEvents txtCatGrsWt_Wet As TextBox
    Friend WithEvents txtCatNetWt_Wet As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtCatPcs_NUM As TextBox
    Friend WithEvents cmbCostCentre_MAN As ComboBox
End Class
