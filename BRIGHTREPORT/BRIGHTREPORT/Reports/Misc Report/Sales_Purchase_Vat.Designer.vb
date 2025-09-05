<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Sales_Purchase_Vat
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
        Me.rbtPurchaseVat = New System.Windows.Forms.RadioButton
        Me.Label3 = New System.Windows.Forms.Label
        Me.rbtSalesVat = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PnlMark = New System.Windows.Forms.Panel
        Me.rbtLocal = New System.Windows.Forms.RadioButton
        Me.rbtOutstation = New System.Windows.Forms.RadioButton
        Me.rbtBothMU = New System.Windows.Forms.RadioButton
        Me.GrpContainer.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.PnlMark.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.PnlMark)
        Me.GrpContainer.Controls.Add(Me.rbtPurchaseVat)
        Me.GrpContainer.Controls.Add(Me.Label3)
        Me.GrpContainer.Controls.Add(Me.rbtSalesVat)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.dtpTo)
        Me.GrpContainer.Controls.Add(Me.dtpFrom)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Location = New System.Drawing.Point(140, 111)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(344, 202)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'rbtPurchaseVat
        '
        Me.rbtPurchaseVat.AutoSize = True
        Me.rbtPurchaseVat.Enabled = False
        Me.rbtPurchaseVat.Location = New System.Drawing.Point(131, 94)
        Me.rbtPurchaseVat.Name = "rbtPurchaseVat"
        Me.rbtPurchaseVat.Size = New System.Drawing.Size(100, 17)
        Me.rbtPurchaseVat.TabIndex = 5
        Me.rbtPurchaseVat.TabStop = True
        Me.rbtPurchaseVat.Text = "Purchase Vat"
        Me.rbtPurchaseVat.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(27, 39)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Date From"
        '
        'rbtSalesVat
        '
        Me.rbtSalesVat.AutoSize = True
        Me.rbtSalesVat.Location = New System.Drawing.Point(131, 67)
        Me.rbtSalesVat.Name = "rbtSalesVat"
        Me.rbtSalesVat.Size = New System.Drawing.Size(79, 17)
        Me.rbtSalesVat.TabIndex = 4
        Me.rbtSalesVat.TabStop = True
        Me.rbtSalesVat.Text = "Sales Vat"
        Me.rbtSalesVat.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(220, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(21, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "To"
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(247, 36)
        Me.dtpTo.Mask = "##-##-####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(82, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "29/09/2010"
        Me.dtpTo.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(131, 36)
        Me.dtpFrom.Mask = "##-##-####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(82, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "29/09/2010"
        Me.dtpFrom.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(234, 151)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 8
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(22, 151)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 6
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(128, 151)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 7
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'PnlMark
        '
        Me.PnlMark.Controls.Add(Me.rbtLocal)
        Me.PnlMark.Controls.Add(Me.rbtOutstation)
        Me.PnlMark.Controls.Add(Me.rbtBothMU)
        Me.PnlMark.Location = New System.Drawing.Point(81, 115)
        Me.PnlMark.Name = "PnlMark"
        Me.PnlMark.Size = New System.Drawing.Size(249, 29)
        Me.PnlMark.TabIndex = 17
        '
        'rbtLocal
        '
        Me.rbtLocal.AutoSize = True
        Me.rbtLocal.Location = New System.Drawing.Point(72, 4)
        Me.rbtLocal.Name = "rbtLocal"
        Me.rbtLocal.Size = New System.Drawing.Size(54, 17)
        Me.rbtLocal.TabIndex = 1
        Me.rbtLocal.Text = "Local"
        Me.rbtLocal.UseVisualStyleBackColor = True
        '
        'rbtOutstation
        '
        Me.rbtOutstation.AutoSize = True
        Me.rbtOutstation.Location = New System.Drawing.Point(142, 4)
        Me.rbtOutstation.Name = "rbtOutstation"
        Me.rbtOutstation.Size = New System.Drawing.Size(83, 17)
        Me.rbtOutstation.TabIndex = 2
        Me.rbtOutstation.Text = "Outstation"
        Me.rbtOutstation.UseVisualStyleBackColor = True
        '
        'rbtBothMU
        '
        Me.rbtBothMU.AutoSize = True
        Me.rbtBothMU.Checked = True
        Me.rbtBothMU.Location = New System.Drawing.Point(5, 3)
        Me.rbtBothMU.Name = "rbtBothMU"
        Me.rbtBothMU.Size = New System.Drawing.Size(51, 17)
        Me.rbtBothMU.TabIndex = 0
        Me.rbtBothMU.TabStop = True
        Me.rbtBothMU.Text = "Both"
        Me.rbtBothMU.UseVisualStyleBackColor = True
        '
        'Sales_Purchase_Vat
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 413)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "Sales_Purchase_Vat"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sales_Purchase_Vat"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.PnlMark.ResumeLayout(False)
        Me.PnlMark.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents rbtPurchaseVat As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSalesVat As System.Windows.Forms.RadioButton
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PnlMark As System.Windows.Forms.Panel
    Friend WithEvents rbtLocal As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOutstation As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBothMU As System.Windows.Forms.RadioButton
End Class
