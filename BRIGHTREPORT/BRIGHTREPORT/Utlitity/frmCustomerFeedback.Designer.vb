<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCustomerFeedback
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
        Me.GrpContainer = New System.Windows.Forms.GroupBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnSave = New System.Windows.Forms.Button
        Me.CmbEmployee = New System.Windows.Forms.ComboBox
        Me.txtQry = New System.Windows.Forms.TextBox
        Me.txtPvg = New System.Windows.Forms.TextBox
        Me.txtAdr3 = New System.Windows.Forms.TextBox
        Me.txtAdr2 = New System.Windows.Forms.TextBox
        Me.txtMbl = New System.Windows.Forms.TextBox
        Me.txtAdr1 = New System.Windows.Forms.TextBox
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtInkFontDetail = New System.Windows.Forms.TextBox
        Me.cmbExportTo = New System.Windows.Forms.ComboBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.cmbSearch = New System.Windows.Forms.ComboBox
        Me.chkOrder = New System.Windows.Forms.CheckBox
        Me.txtOrderNo = New System.Windows.Forms.TextBox
        Me.GrpContainer.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.txtOrderNo)
        Me.GrpContainer.Controls.Add(Me.chkOrder)
        Me.GrpContainer.Controls.Add(Me.Label3)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.btnSave)
        Me.GrpContainer.Controls.Add(Me.CmbEmployee)
        Me.GrpContainer.Controls.Add(Me.txtQry)
        Me.GrpContainer.Controls.Add(Me.txtPvg)
        Me.GrpContainer.Controls.Add(Me.txtAdr3)
        Me.GrpContainer.Controls.Add(Me.txtAdr2)
        Me.GrpContainer.Controls.Add(Me.txtMbl)
        Me.GrpContainer.Controls.Add(Me.txtAdr1)
        Me.GrpContainer.Controls.Add(Me.txtName)
        Me.GrpContainer.Controls.Add(Me.txtInkFontDetail)
        Me.GrpContainer.Controls.Add(Me.cmbExportTo)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.Label9)
        Me.GrpContainer.Controls.Add(Me.Label8)
        Me.GrpContainer.Controls.Add(Me.Label6)
        Me.GrpContainer.Controls.Add(Me.Label5)
        Me.GrpContainer.Controls.Add(Me.Label4)
        Me.GrpContainer.Controls.Add(Me.Label2)
        Me.GrpContainer.Location = New System.Drawing.Point(160, 12)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(421, 494)
        Me.GrpContainer.TabIndex = 1
        Me.GrpContainer.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(23, 182)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 13)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "Mobile         "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(23, 74)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Name          "
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(153, 401)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 20
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'CmbEmployee
        '
        Me.CmbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbEmployee.FormattingEnabled = True
        Me.CmbEmployee.Location = New System.Drawing.Point(152, 351)
        Me.CmbEmployee.Name = "CmbEmployee"
        Me.CmbEmployee.Size = New System.Drawing.Size(249, 21)
        Me.CmbEmployee.TabIndex = 19
        '
        'txtQry
        '
        Me.txtQry.Location = New System.Drawing.Point(152, 239)
        Me.txtQry.MaxLength = 500
        Me.txtQry.Multiline = True
        Me.txtQry.Name = "txtQry"
        Me.txtQry.Size = New System.Drawing.Size(249, 106)
        Me.txtQry.TabIndex = 17
        '
        'txtPvg
        '
        Me.txtPvg.Location = New System.Drawing.Point(151, 212)
        Me.txtPvg.Name = "txtPvg"
        Me.txtPvg.ReadOnly = True
        Me.txtPvg.Size = New System.Drawing.Size(154, 21)
        Me.txtPvg.TabIndex = 15
        '
        'txtAdr3
        '
        Me.txtAdr3.Location = New System.Drawing.Point(151, 158)
        Me.txtAdr3.Name = "txtAdr3"
        Me.txtAdr3.ReadOnly = True
        Me.txtAdr3.Size = New System.Drawing.Size(154, 21)
        Me.txtAdr3.TabIndex = 11
        '
        'txtAdr2
        '
        Me.txtAdr2.Location = New System.Drawing.Point(151, 131)
        Me.txtAdr2.Name = "txtAdr2"
        Me.txtAdr2.ReadOnly = True
        Me.txtAdr2.Size = New System.Drawing.Size(154, 21)
        Me.txtAdr2.TabIndex = 9
        '
        'txtMbl
        '
        Me.txtMbl.Location = New System.Drawing.Point(151, 185)
        Me.txtMbl.Name = "txtMbl"
        Me.txtMbl.ReadOnly = True
        Me.txtMbl.Size = New System.Drawing.Size(154, 21)
        Me.txtMbl.TabIndex = 13
        '
        'txtAdr1
        '
        Me.txtAdr1.Location = New System.Drawing.Point(151, 104)
        Me.txtAdr1.Name = "txtAdr1"
        Me.txtAdr1.ReadOnly = True
        Me.txtAdr1.Size = New System.Drawing.Size(154, 21)
        Me.txtAdr1.TabIndex = 6
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(151, 77)
        Me.txtName.Name = "txtName"
        Me.txtName.ReadOnly = True
        Me.txtName.Size = New System.Drawing.Size(154, 21)
        Me.txtName.TabIndex = 5
        '
        'txtInkFontDetail
        '
        Me.txtInkFontDetail.Location = New System.Drawing.Point(151, 50)
        Me.txtInkFontDetail.Name = "txtInkFontDetail"
        Me.txtInkFontDetail.Size = New System.Drawing.Size(154, 21)
        Me.txtInkFontDetail.TabIndex = 3
        '
        'cmbExportTo
        '
        Me.cmbExportTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbExportTo.FormattingEnabled = True
        Me.cmbExportTo.Location = New System.Drawing.Point(23, 50)
        Me.cmbExportTo.Name = "cmbExportTo"
        Me.cmbExportTo.Size = New System.Drawing.Size(115, 21)
        Me.cmbExportTo.TabIndex = 2
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(259, 401)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 21
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(23, 155)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(104, 13)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "Address 3          "
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(23, 128)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(104, 13)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Address 2          "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(23, 351)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(117, 13)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "Attended By          "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(23, 239)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(109, 13)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Query / Feedback"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(23, 209)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(92, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Previlege        "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(23, 101)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(104, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Address 1          "
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.SaveToolStripMenuItem.Text = "save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'cmbSearch
        '
        Me.cmbSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSearch.FormattingEnabled = True
        Me.cmbSearch.Location = New System.Drawing.Point(763, 62)
        Me.cmbSearch.Name = "cmbSearch"
        Me.cmbSearch.Size = New System.Drawing.Size(190, 21)
        Me.cmbSearch.TabIndex = 0
        Me.cmbSearch.Visible = False
        '
        'chkOrder
        '
        Me.chkOrder.AutoSize = True
        Me.chkOrder.Location = New System.Drawing.Point(23, 20)
        Me.chkOrder.Name = "chkOrder"
        Me.chkOrder.Size = New System.Drawing.Size(59, 17)
        Me.chkOrder.TabIndex = 0
        Me.chkOrder.Text = "Order"
        Me.chkOrder.UseVisualStyleBackColor = True
        '
        'txtOrderNo
        '
        Me.txtOrderNo.Location = New System.Drawing.Point(151, 24)
        Me.txtOrderNo.Name = "txtOrderNo"
        Me.txtOrderNo.Size = New System.Drawing.Size(154, 21)
        Me.txtOrderNo.TabIndex = 1
        '
        'frmCustomerFeedback
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(965, 638)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.GrpContainer)
        Me.Controls.Add(Me.cmbSearch)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCustomerFeedback"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Customer Feedback"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbExportTo As System.Windows.Forms.ComboBox
    Friend WithEvents txtMbl As System.Windows.Forms.TextBox
    Friend WithEvents txtAdr1 As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtInkFontDetail As System.Windows.Forms.TextBox
    Friend WithEvents txtAdr3 As System.Windows.Forms.TextBox
    Friend WithEvents txtAdr2 As System.Windows.Forms.TextBox
    Friend WithEvents CmbEmployee As System.Windows.Forms.ComboBox
    Friend WithEvents txtQry As System.Windows.Forms.TextBox
    Friend WithEvents txtPvg As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbSearch As System.Windows.Forms.ComboBox
    Friend WithEvents txtOrderNo As System.Windows.Forms.TextBox
    Friend WithEvents chkOrder As System.Windows.Forms.CheckBox
End Class
