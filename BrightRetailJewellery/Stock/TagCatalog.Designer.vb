<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TagCatalog
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
        Me.picTagImage = New System.Windows.Forms.PictureBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlSearch = New System.Windows.Forms.Panel()
        Me.cmbSearch_OWN = New System.Windows.Forms.ComboBox()
        Me.picCapture = New System.Windows.Forms.PictureBox()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtStyleNo_NUM = New System.Windows.Forms.TextBox()
        Me.cmbItem_MAN = New System.Windows.Forms.ComboBox()
        Me.cmbSubitem_MAN = New System.Windows.Forms.ComboBox()
        Me.cmbItemType_MAN = New System.Windows.Forms.ComboBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.cmbSize_OWN = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbGrsNet = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtPcs_NUM_MAN = New System.Windows.Forms.TextBox()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.txtGrsWt_WET_MAN = New System.Windows.Forms.TextBox()
        Me.txtLessWt_WET = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtNetWt_WET = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtNarration = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.pnlDesiner = New System.Windows.Forms.Panel()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtDesiner = New System.Windows.Forms.TextBox()
        CType(Me.picTagImage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlSearch.SuspendLayout()
        CType(Me.picCapture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlMain.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlDesiner.SuspendLayout()
        Me.SuspendLayout()
        '
        'picTagImage
        '
        Me.picTagImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picTagImage.Image = Global.BrighttechRetailJewellery.My.Resources.Resources.no_photo
        Me.picTagImage.Location = New System.Drawing.Point(397, 9)
        Me.picTagImage.Name = "picTagImage"
        Me.picTagImage.Size = New System.Drawing.Size(300, 312)
        Me.picTagImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picTagImage.TabIndex = 41
        Me.picTagImage.TabStop = False
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(144, 383)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 2
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(249, 383)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 3
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(355, 383)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 4
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(10, 419)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(900, 139)
        Me.gridView.TabIndex = 27
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 70)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'pnlSearch
        '
        Me.pnlSearch.Controls.Add(Me.cmbSearch_OWN)
        Me.pnlSearch.Location = New System.Drawing.Point(702, 8)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(209, 327)
        Me.pnlSearch.TabIndex = 42
        Me.pnlSearch.Visible = False
        '
        'cmbSearch_OWN
        '
        Me.cmbSearch_OWN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.cmbSearch_OWN.FormattingEnabled = True
        Me.cmbSearch_OWN.Location = New System.Drawing.Point(1, -21)
        Me.cmbSearch_OWN.Name = "cmbSearch_OWN"
        Me.cmbSearch_OWN.Size = New System.Drawing.Size(206, 345)
        Me.cmbSearch_OWN.TabIndex = 0
        '
        'picCapture
        '
        Me.picCapture.Image = Global.BrighttechRetailJewellery.My.Resources.Resources.no_photo
        Me.picCapture.Location = New System.Drawing.Point(396, 8)
        Me.picCapture.Name = "picCapture"
        Me.picCapture.Size = New System.Drawing.Size(300, 320)
        Me.picCapture.TabIndex = 68
        Me.picCapture.TabStop = False
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.PictureBox1)
        Me.pnlMain.Controls.Add(Me.Label1)
        Me.pnlMain.Controls.Add(Me.txtStyleNo_NUM)
        Me.pnlMain.Controls.Add(Me.cmbItem_MAN)
        Me.pnlMain.Controls.Add(Me.cmbSubitem_MAN)
        Me.pnlMain.Controls.Add(Me.cmbItemType_MAN)
        Me.pnlMain.Controls.Add(Me.btnBrowse)
        Me.pnlMain.Controls.Add(Me.cmbSize_OWN)
        Me.pnlMain.Controls.Add(Me.Label10)
        Me.pnlMain.Controls.Add(Me.cmbGrsNet)
        Me.pnlMain.Controls.Add(Me.Label9)
        Me.pnlMain.Controls.Add(Me.txtPcs_NUM_MAN)
        Me.pnlMain.Controls.Add(Me.Label52)
        Me.pnlMain.Controls.Add(Me.txtGrsWt_WET_MAN)
        Me.pnlMain.Controls.Add(Me.txtLessWt_WET)
        Me.pnlMain.Controls.Add(Me.Label8)
        Me.pnlMain.Controls.Add(Me.txtNetWt_WET)
        Me.pnlMain.Controls.Add(Me.Label7)
        Me.pnlMain.Controls.Add(Me.Label11)
        Me.pnlMain.Controls.Add(Me.txtNarration)
        Me.pnlMain.Controls.Add(Me.Label6)
        Me.pnlMain.Controls.Add(Me.Label2)
        Me.pnlMain.Controls.Add(Me.Label5)
        Me.pnlMain.Controls.Add(Me.Label3)
        Me.pnlMain.Controls.Add(Me.Label4)
        Me.pnlMain.Location = New System.Drawing.Point(10, 38)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(380, 340)
        Me.pnlMain.TabIndex = 1
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.Location = New System.Drawing.Point(338, 314)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(17, 22)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 94
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Item"
        '
        'txtStyleNo_NUM
        '
        Me.txtStyleNo_NUM.Location = New System.Drawing.Point(127, 116)
        Me.txtStyleNo_NUM.MaxLength = 12
        Me.txtStyleNo_NUM.Name = "txtStyleNo_NUM"
        Me.txtStyleNo_NUM.Size = New System.Drawing.Size(113, 21)
        Me.txtStyleNo_NUM.TabIndex = 9
        Me.txtStyleNo_NUM.Text = "999999999999"
        '
        'cmbItem_MAN
        '
        Me.cmbItem_MAN.FormattingEnabled = True
        Me.cmbItem_MAN.Location = New System.Drawing.Point(127, 4)
        Me.cmbItem_MAN.Name = "cmbItem_MAN"
        Me.cmbItem_MAN.Size = New System.Drawing.Size(246, 21)
        Me.cmbItem_MAN.TabIndex = 1
        '
        'cmbSubitem_MAN
        '
        Me.cmbSubitem_MAN.FormattingEnabled = True
        Me.cmbSubitem_MAN.Location = New System.Drawing.Point(127, 32)
        Me.cmbSubitem_MAN.Name = "cmbSubitem_MAN"
        Me.cmbSubitem_MAN.Size = New System.Drawing.Size(246, 21)
        Me.cmbSubitem_MAN.TabIndex = 3
        '
        'cmbItemType_MAN
        '
        Me.cmbItemType_MAN.FormattingEnabled = True
        Me.cmbItemType_MAN.Location = New System.Drawing.Point(127, 60)
        Me.cmbItemType_MAN.Name = "cmbItemType_MAN"
        Me.cmbItemType_MAN.Size = New System.Drawing.Size(246, 21)
        Me.cmbItemType_MAN.TabIndex = 5
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(127, 314)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(83, 21)
        Me.btnBrowse.TabIndex = 23
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'cmbSize_OWN
        '
        Me.cmbSize_OWN.FormattingEnabled = True
        Me.cmbSize_OWN.Location = New System.Drawing.Point(127, 88)
        Me.cmbSize_OWN.MaxLength = 50
        Me.cmbSize_OWN.Name = "cmbSize_OWN"
        Me.cmbSize_OWN.Size = New System.Drawing.Size(246, 21)
        Me.cmbSize_OWN.TabIndex = 7
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(8, 317)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(44, 13)
        Me.Label10.TabIndex = 22
        Me.Label10.Text = "Image"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbGrsNet
        '
        Me.cmbGrsNet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbGrsNet.FormattingEnabled = True
        Me.cmbGrsNet.Location = New System.Drawing.Point(127, 258)
        Me.cmbGrsNet.Name = "cmbGrsNet"
        Me.cmbGrsNet.Size = New System.Drawing.Size(113, 21)
        Me.cmbGrsNet.TabIndex = 19
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(8, 289)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(60, 13)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Narration"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPcs_NUM_MAN
        '
        Me.txtPcs_NUM_MAN.Location = New System.Drawing.Point(127, 144)
        Me.txtPcs_NUM_MAN.Name = "txtPcs_NUM_MAN"
        Me.txtPcs_NUM_MAN.Size = New System.Drawing.Size(113, 21)
        Me.txtPcs_NUM_MAN.TabIndex = 11
        '
        'Label52
        '
        Me.Label52.AutoSize = True
        Me.Label52.Location = New System.Drawing.Point(8, 261)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(66, 13)
        Me.Label52.TabIndex = 18
        Me.Label52.Text = "Calc Mode"
        Me.Label52.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtGrsWt_WET_MAN
        '
        Me.txtGrsWt_WET_MAN.Location = New System.Drawing.Point(127, 172)
        Me.txtGrsWt_WET_MAN.Name = "txtGrsWt_WET_MAN"
        Me.txtGrsWt_WET_MAN.Size = New System.Drawing.Size(113, 21)
        Me.txtGrsWt_WET_MAN.TabIndex = 13
        '
        'txtLessWt_WET
        '
        Me.txtLessWt_WET.Location = New System.Drawing.Point(127, 201)
        Me.txtLessWt_WET.Name = "txtLessWt_WET"
        Me.txtLessWt_WET.Size = New System.Drawing.Size(113, 21)
        Me.txtLessWt_WET.TabIndex = 15
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(8, 233)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(45, 13)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Net Wt"
        '
        'txtNetWt_WET
        '
        Me.txtNetWt_WET.Location = New System.Drawing.Point(127, 230)
        Me.txtNetWt_WET.Name = "txtNetWt_WET"
        Me.txtNetWt_WET.Size = New System.Drawing.Size(113, 21)
        Me.txtNetWt_WET.TabIndex = 17
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 175)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(46, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Grs Wt"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(7, 204)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(51, 13)
        Me.Label11.TabIndex = 14
        Me.Label11.Text = "Less Wt"
        '
        'txtNarration
        '
        Me.txtNarration.Location = New System.Drawing.Point(127, 286)
        Me.txtNarration.MaxLength = 50
        Me.txtNarration.Name = "txtNarration"
        Me.txtNarration.Size = New System.Drawing.Size(247, 21)
        Me.txtNarration.TabIndex = 21
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 147)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(26, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Pcs"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Sub Item"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(8, 119)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(55, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Style No"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 63)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Item Type"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 91)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Size"
        '
        'pnlDesiner
        '
        Me.pnlDesiner.Controls.Add(Me.Label12)
        Me.pnlDesiner.Controls.Add(Me.txtDesiner)
        Me.pnlDesiner.Location = New System.Drawing.Point(10, 8)
        Me.pnlDesiner.Name = "pnlDesiner"
        Me.pnlDesiner.Size = New System.Drawing.Size(380, 27)
        Me.pnlDesiner.TabIndex = 0
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(7, 6)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(58, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Designer"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDesiner
        '
        Me.txtDesiner.Location = New System.Drawing.Point(126, 3)
        Me.txtDesiner.MaxLength = 50
        Me.txtDesiner.Name = "txtDesiner"
        Me.txtDesiner.Size = New System.Drawing.Size(247, 21)
        Me.txtDesiner.TabIndex = 1
        '
        'TagCatalog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(921, 570)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlDesiner)
        Me.Controls.Add(Me.pnlMain)
        Me.Controls.Add(Me.pnlSearch)
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.picCapture)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.picTagImage)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "TagCatalog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TagCatalog"
        CType(Me.picTagImage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlSearch.ResumeLayout(False)
        CType(Me.picCapture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlMain.ResumeLayout(False)
        Me.pnlMain.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlDesiner.ResumeLayout(False)
        Me.pnlDesiner.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents picTagImage As System.Windows.Forms.PictureBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlSearch As System.Windows.Forms.Panel
    Friend WithEvents cmbSearch_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents picCapture As System.Windows.Forms.PictureBox
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtStyleNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents cmbItem_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubitem_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItemType_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents cmbSize_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cmbGrsNet As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtPcs_NUM_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents txtGrsWt_WET_MAN As System.Windows.Forms.TextBox
    Friend WithEvents txtLessWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtNarration As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents pnlDesiner As System.Windows.Forms.Panel
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtDesiner As System.Windows.Forms.TextBox
End Class
