<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSubItemDesignerLink
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
        Me.tbMain = New System.Windows.Forms.TabControl
        Me.tbGen = New System.Windows.Forms.TabPage
        Me.GRPfIELDS = New System.Windows.Forms.GroupBox
        Me.cmbStuddedSubitem = New System.Windows.Forms.ComboBox
        Me.cmb_StuddedItemName = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.cmb_Designer = New System.Windows.Forms.ComboBox
        Me.cmbSubitemName = New System.Windows.Forms.ComboBox
        Me.cmb_itemName = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtStnPer = New System.Windows.Forms.TextBox
        Me.txtSTNRATE = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.cmbActive = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.bnExit = New System.Windows.Forms.Button
        Me.bnNew = New System.Windows.Forms.Button
        Me.bnOpen = New System.Windows.Forms.Button
        Me.bnSave = New System.Windows.Forms.Button
        Me.tbView = New System.Windows.Forms.TabPage
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cmbDesigner_OWN = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbSubItem_OWN = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbItem_OWN = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnSearch_Own = New System.Windows.Forms.Button
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Label4 = New System.Windows.Forms.Label
        Me.CmbUnit = New System.Windows.Forms.ComboBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.CmbCalcType = New System.Windows.Forms.ComboBox
        Me.tbMain.SuspendLayout()
        Me.tbGen.SuspendLayout()
        Me.GRPfIELDS.SuspendLayout()
        Me.tbView.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbMain
        '
        Me.tbMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tbMain.Controls.Add(Me.tbGen)
        Me.tbMain.Controls.Add(Me.tbView)
        Me.tbMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbMain.Location = New System.Drawing.Point(0, 0)
        Me.tbMain.Name = "tbMain"
        Me.tbMain.SelectedIndex = 0
        Me.tbMain.Size = New System.Drawing.Size(851, 465)
        Me.tbMain.TabIndex = 0
        '
        'tbGen
        '
        Me.tbGen.Controls.Add(Me.GRPfIELDS)
        Me.tbGen.Location = New System.Drawing.Point(4, 25)
        Me.tbGen.Name = "tbGen"
        Me.tbGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tbGen.Size = New System.Drawing.Size(843, 436)
        Me.tbGen.TabIndex = 0
        Me.tbGen.Text = "Gen"
        Me.tbGen.UseVisualStyleBackColor = True
        '
        'GRPfIELDS
        '
        Me.GRPfIELDS.BackColor = System.Drawing.Color.Transparent
        Me.GRPfIELDS.Controls.Add(Me.Label12)
        Me.GRPfIELDS.Controls.Add(Me.CmbCalcType)
        Me.GRPfIELDS.Controls.Add(Me.Label4)
        Me.GRPfIELDS.Controls.Add(Me.CmbUnit)
        Me.GRPfIELDS.Controls.Add(Me.cmbStuddedSubitem)
        Me.GRPfIELDS.Controls.Add(Me.cmb_StuddedItemName)
        Me.GRPfIELDS.Controls.Add(Me.Label10)
        Me.GRPfIELDS.Controls.Add(Me.Label11)
        Me.GRPfIELDS.Controls.Add(Me.cmb_Designer)
        Me.GRPfIELDS.Controls.Add(Me.cmbSubitemName)
        Me.GRPfIELDS.Controls.Add(Me.cmb_itemName)
        Me.GRPfIELDS.Controls.Add(Me.Label9)
        Me.GRPfIELDS.Controls.Add(Me.Label8)
        Me.GRPfIELDS.Controls.Add(Me.Label7)
        Me.GRPfIELDS.Controls.Add(Me.txtStnPer)
        Me.GRPfIELDS.Controls.Add(Me.txtSTNRATE)
        Me.GRPfIELDS.Controls.Add(Me.Label6)
        Me.GRPfIELDS.Controls.Add(Me.Label24)
        Me.GRPfIELDS.Controls.Add(Me.cmbActive)
        Me.GRPfIELDS.Controls.Add(Me.Label5)
        Me.GRPfIELDS.Controls.Add(Me.bnExit)
        Me.GRPfIELDS.Controls.Add(Me.bnNew)
        Me.GRPfIELDS.Controls.Add(Me.bnOpen)
        Me.GRPfIELDS.Controls.Add(Me.bnSave)
        Me.GRPfIELDS.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRPfIELDS.Location = New System.Drawing.Point(3, 3)
        Me.GRPfIELDS.Name = "GRPfIELDS"
        Me.GRPfIELDS.Size = New System.Drawing.Size(837, 430)
        Me.GRPfIELDS.TabIndex = 0
        Me.GRPfIELDS.TabStop = False
        '
        'cmbStuddedSubitem
        '
        Me.cmbStuddedSubitem.FormattingEnabled = True
        Me.cmbStuddedSubitem.Location = New System.Drawing.Point(228, 170)
        Me.cmbStuddedSubitem.Name = "cmbStuddedSubitem"
        Me.cmbStuddedSubitem.Size = New System.Drawing.Size(252, 21)
        Me.cmbStuddedSubitem.TabIndex = 9
        '
        'cmb_StuddedItemName
        '
        Me.cmb_StuddedItemName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmb_StuddedItemName.FormattingEnabled = True
        Me.cmb_StuddedItemName.Location = New System.Drawing.Point(228, 137)
        Me.cmb_StuddedItemName.Name = "cmb_StuddedItemName"
        Me.cmb_StuddedItemName.Size = New System.Drawing.Size(252, 21)
        Me.cmb_StuddedItemName.TabIndex = 7
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(77, 173)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(144, 13)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Studded SubItem Name"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(77, 137)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(122, 13)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "Studded Item Name"
        '
        'cmb_Designer
        '
        Me.cmb_Designer.FormattingEnabled = True
        Me.cmb_Designer.Items.AddRange(New Object() {"ALL"})
        Me.cmb_Designer.Location = New System.Drawing.Point(228, 29)
        Me.cmb_Designer.Name = "cmb_Designer"
        Me.cmb_Designer.Size = New System.Drawing.Size(252, 21)
        Me.cmb_Designer.TabIndex = 1
        '
        'cmbSubitemName
        '
        Me.cmbSubitemName.FormattingEnabled = True
        Me.cmbSubitemName.Location = New System.Drawing.Point(228, 102)
        Me.cmbSubitemName.Name = "cmbSubitemName"
        Me.cmbSubitemName.Size = New System.Drawing.Size(252, 21)
        Me.cmbSubitemName.TabIndex = 5
        '
        'cmb_itemName
        '
        Me.cmb_itemName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmb_itemName.FormattingEnabled = True
        Me.cmb_itemName.Location = New System.Drawing.Point(228, 65)
        Me.cmb_itemName.Name = "cmb_itemName"
        Me.cmb_itemName.Size = New System.Drawing.Size(252, 21)
        Me.cmb_itemName.TabIndex = 3
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(77, 105)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(93, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "SubItem Name"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(77, 65)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(71, 13)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Item Name"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(77, 29)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(58, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Designer"
        '
        'txtStnPer
        '
        Me.txtStnPer.Location = New System.Drawing.Point(228, 207)
        Me.txtStnPer.Name = "txtStnPer"
        Me.txtStnPer.Size = New System.Drawing.Size(149, 21)
        Me.txtStnPer.TabIndex = 11
        '
        'txtSTNRATE
        '
        Me.txtSTNRATE.Location = New System.Drawing.Point(228, 248)
        Me.txtSTNRATE.Name = "txtSTNRATE"
        Me.txtSTNRATE.Size = New System.Drawing.Size(149, 21)
        Me.txtSTNRATE.TabIndex = 13
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(77, 251)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(70, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Stone Rate"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(77, 314)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(42, 13)
        Me.Label24.TabIndex = 18
        Me.Label24.Text = "Active"
        '
        'cmbActive
        '
        Me.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActive.FormattingEnabled = True
        Me.cmbActive.Items.AddRange(New Object() {"YES", "NO"})
        Me.cmbActive.Location = New System.Drawing.Point(228, 314)
        Me.cmbActive.Name = "cmbActive"
        Me.cmbActive.Size = New System.Drawing.Size(88, 21)
        Me.cmbActive.TabIndex = 19
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(77, 210)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(104, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Stone Deduct  %"
        '
        'bnExit
        '
        Me.bnExit.Location = New System.Drawing.Point(461, 344)
        Me.bnExit.Name = "bnExit"
        Me.bnExit.Size = New System.Drawing.Size(117, 35)
        Me.bnExit.TabIndex = 23
        Me.bnExit.Text = "Exit [F12]"
        Me.bnExit.UseVisualStyleBackColor = True
        '
        'bnNew
        '
        Me.bnNew.Location = New System.Drawing.Point(337, 344)
        Me.bnNew.Name = "bnNew"
        Me.bnNew.Size = New System.Drawing.Size(117, 35)
        Me.bnNew.TabIndex = 22
        Me.bnNew.Text = "New [F3]"
        Me.bnNew.UseVisualStyleBackColor = True
        '
        'bnOpen
        '
        Me.bnOpen.Location = New System.Drawing.Point(219, 344)
        Me.bnOpen.Name = "bnOpen"
        Me.bnOpen.Size = New System.Drawing.Size(117, 35)
        Me.bnOpen.TabIndex = 21
        Me.bnOpen.Text = "Open [F2]"
        Me.bnOpen.UseVisualStyleBackColor = True
        '
        'bnSave
        '
        Me.bnSave.Location = New System.Drawing.Point(100, 344)
        Me.bnSave.Name = "bnSave"
        Me.bnSave.Size = New System.Drawing.Size(117, 35)
        Me.bnSave.TabIndex = 20
        Me.bnSave.Text = "Save [F1]"
        Me.bnSave.UseVisualStyleBackColor = True
        '
        'tbView
        '
        Me.tbView.Controls.Add(Me.GroupBox1)
        Me.tbView.Controls.Add(Me.gridView)
        Me.tbView.Location = New System.Drawing.Point(4, 25)
        Me.tbView.Name = "tbView"
        Me.tbView.Padding = New System.Windows.Forms.Padding(3)
        Me.tbView.Size = New System.Drawing.Size(843, 436)
        Me.tbView.TabIndex = 1
        Me.tbView.Text = "View"
        Me.tbView.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbDesigner_OWN)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.cmbSubItem_OWN)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.cmbItem_OWN)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.btnSearch_Own)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(825, 69)
        Me.GroupBox1.TabIndex = 34
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Search"
        '
        'cmbDesigner_OWN
        '
        Me.cmbDesigner_OWN.FormattingEnabled = True
        Me.cmbDesigner_OWN.Items.AddRange(New Object() {"ALL"})
        Me.cmbDesigner_OWN.Location = New System.Drawing.Point(450, 42)
        Me.cmbDesigner_OWN.Name = "cmbDesigner_OWN"
        Me.cmbDesigner_OWN.Size = New System.Drawing.Size(269, 21)
        Me.cmbDesigner_OWN.TabIndex = 14
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(448, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Designer"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSubItem_OWN
        '
        Me.cmbSubItem_OWN.FormattingEnabled = True
        Me.cmbSubItem_OWN.Location = New System.Drawing.Point(201, 42)
        Me.cmbSubItem_OWN.Name = "cmbSubItem_OWN"
        Me.cmbSubItem_OWN.Size = New System.Drawing.Size(241, 21)
        Me.cmbSubItem_OWN.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(199, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "SubItem"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItem_OWN
        '
        Me.cmbItem_OWN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbItem_OWN.FormattingEnabled = True
        Me.cmbItem_OWN.Location = New System.Drawing.Point(7, 42)
        Me.cmbItem_OWN.Name = "cmbItem_OWN"
        Me.cmbItem_OWN.Size = New System.Drawing.Size(185, 21)
        Me.cmbItem_OWN.TabIndex = 12
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Item"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSearch_Own
        '
        Me.btnSearch_Own.Location = New System.Drawing.Point(726, 36)
        Me.btnSearch_Own.Name = "btnSearch_Own"
        Me.btnSearch_Own.Size = New System.Drawing.Size(92, 30)
        Me.btnSearch_Own.TabIndex = 15
        Me.btnSearch_Own.Text = "  Search"
        Me.btnSearch_Own.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(7, 82)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(826, 332)
        Me.gridView.TabIndex = 33
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(77, 286)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(66, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Stone Unit"
        '
        'CmbUnit
        '
        Me.CmbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbUnit.FormattingEnabled = True
        Me.CmbUnit.Items.AddRange(New Object() {"CARAT", "GRAM"})
        Me.CmbUnit.Location = New System.Drawing.Point(228, 282)
        Me.CmbUnit.Name = "CmbUnit"
        Me.CmbUnit.Size = New System.Drawing.Size(88, 21)
        Me.CmbUnit.TabIndex = 15
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(324, 286)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(64, 13)
        Me.Label12.TabIndex = 16
        Me.Label12.Text = "Calc Type"
        '
        'CmbCalcType
        '
        Me.CmbCalcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbCalcType.FormattingEnabled = True
        Me.CmbCalcType.Items.AddRange(New Object() {"PCS", "WEIGHT"})
        Me.CmbCalcType.Location = New System.Drawing.Point(392, 282)
        Me.CmbCalcType.Name = "CmbCalcType"
        Me.CmbCalcType.Size = New System.Drawing.Size(88, 21)
        Me.CmbCalcType.TabIndex = 17
        '
        'frmSubItemDesignerLink
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(851, 465)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tbMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmSubItemDesignerLink"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SubItem Designer Link"
        Me.tbMain.ResumeLayout(False)
        Me.tbGen.ResumeLayout(False)
        Me.GRPfIELDS.ResumeLayout(False)
        Me.GRPfIELDS.PerformLayout()
        Me.tbView.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tbMain As System.Windows.Forms.TabControl
    Friend WithEvents tbGen As System.Windows.Forms.TabPage
    Friend WithEvents GRPfIELDS As System.Windows.Forms.GroupBox
    Friend WithEvents bnExit As System.Windows.Forms.Button
    Friend WithEvents bnNew As System.Windows.Forms.Button
    Friend WithEvents bnOpen As System.Windows.Forms.Button
    Friend WithEvents bnSave As System.Windows.Forms.Button
    Friend WithEvents tbView As System.Windows.Forms.TabPage
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbDesigner_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbSubItem_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbItem_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnSearch_Own As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents cmbActive As System.Windows.Forms.ComboBox
    Friend WithEvents txtSTNRATE As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtStnPer As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmb_Designer As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubitemName As System.Windows.Forms.ComboBox
    Friend WithEvents cmb_itemName As System.Windows.Forms.ComboBox
    Friend WithEvents cmbStuddedSubitem As System.Windows.Forms.ComboBox
    Friend WithEvents cmb_StuddedItemName As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents CmbCalcType As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents CmbUnit As System.Windows.Forms.ComboBox
End Class
