<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCounterChange
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
        Me.grpContainer = New System.Windows.Forms.GroupBox()
        Me.cmbOldCounter_MAN = New System.Windows.Forms.ComboBox()
        Me.chkCmbCounter_Man = New BrighttechPack.CheckedComboBox()
        Me.btnview = New System.Windows.Forms.Button()
        Me.txtPktNo = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtReason = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtEstNo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkScan = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtappno = New System.Windows.Forms.TextBox()
        Me.gridViewFooter = New System.Windows.Forms.DataGridView()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtFindTag = New System.Windows.Forms.TextBox()
        Me.lblItemid = New System.Windows.Forms.Label()
        Me.txtItemId = New System.Windows.Forms.TextBox()
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.lblRowDet1 = New System.Windows.Forms.Label()
        Me.cmbNewCounter_MAN = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblOldCounter = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtTagNo = New System.Windows.Forms.TextBox()
        Me.txtLotNo_NUM = New System.Windows.Forms.TextBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnCounterChange = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabMain = New System.Windows.Forms.TabControl()
        Me.TabGeneral = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.dgview = New System.Windows.Forms.DataGridView()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.ChkAll = New System.Windows.Forms.CheckBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnDuplicate = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.dtptabto = New BrighttechPack.DatePicker(Me.components)
        Me.dtptabfrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label10 = New System.Windows.Forms.Label()
        Me.grpContainer.SuspendLayout()
        CType(Me.gridViewFooter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.TabMain.SuspendLayout()
        Me.TabGeneral.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.dgview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.cmbOldCounter_MAN)
        Me.grpContainer.Controls.Add(Me.chkCmbCounter_Man)
        Me.grpContainer.Controls.Add(Me.btnview)
        Me.grpContainer.Controls.Add(Me.txtPktNo)
        Me.grpContainer.Controls.Add(Me.Label9)
        Me.grpContainer.Controls.Add(Me.txtReason)
        Me.grpContainer.Controls.Add(Me.Label5)
        Me.grpContainer.Controls.Add(Me.txtEstNo)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.chkScan)
        Me.grpContainer.Controls.Add(Me.Label8)
        Me.grpContainer.Controls.Add(Me.txtappno)
        Me.grpContainer.Controls.Add(Me.gridViewFooter)
        Me.grpContainer.Controls.Add(Me.Label7)
        Me.grpContainer.Controls.Add(Me.txtFindTag)
        Me.grpContainer.Controls.Add(Me.lblItemid)
        Me.grpContainer.Controls.Add(Me.txtItemId)
        Me.grpContainer.Controls.Add(Me.chkDate)
        Me.grpContainer.Controls.Add(Me.dtpTo)
        Me.grpContainer.Controls.Add(Me.dtpFrom)
        Me.grpContainer.Controls.Add(Me.chkSelectAll)
        Me.grpContainer.Controls.Add(Me.lblRowDet1)
        Me.grpContainer.Controls.Add(Me.cmbNewCounter_MAN)
        Me.grpContainer.Controls.Add(Me.Label3)
        Me.grpContainer.Controls.Add(Me.Label4)
        Me.grpContainer.Controls.Add(Me.Label6)
        Me.grpContainer.Controls.Add(Me.lblOldCounter)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.txtTagNo)
        Me.grpContainer.Controls.Add(Me.txtLotNo_NUM)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnCounterChange)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.gridView)
        Me.grpContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpContainer.Location = New System.Drawing.Point(3, 3)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(826, 540)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'cmbOldCounter_MAN
        '
        Me.cmbOldCounter_MAN.FormattingEnabled = True
        Me.cmbOldCounter_MAN.Location = New System.Drawing.Point(88, 41)
        Me.cmbOldCounter_MAN.Name = "cmbOldCounter_MAN"
        Me.cmbOldCounter_MAN.Size = New System.Drawing.Size(212, 21)
        Me.cmbOldCounter_MAN.TabIndex = 10
        '
        'chkCmbCounter_Man
        '
        Me.chkCmbCounter_Man.CheckOnClick = True
        Me.chkCmbCounter_Man.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCounter_Man.DropDownHeight = 1
        Me.chkCmbCounter_Man.FormattingEnabled = True
        Me.chkCmbCounter_Man.IntegralHeight = False
        Me.chkCmbCounter_Man.Location = New System.Drawing.Point(88, 41)
        Me.chkCmbCounter_Man.Name = "chkCmbCounter_Man"
        Me.chkCmbCounter_Man.Size = New System.Drawing.Size(213, 22)
        Me.chkCmbCounter_Man.TabIndex = 10
        Me.chkCmbCounter_Man.ValueSeparator = ", "
        '
        'btnview
        '
        Me.btnview.Location = New System.Drawing.Point(534, 123)
        Me.btnview.Name = "btnview"
        Me.btnview.Size = New System.Drawing.Size(100, 30)
        Me.btnview.TabIndex = 30
        Me.btnview.Text = "&Open[F2]"
        Me.btnview.UseVisualStyleBackColor = True
        '
        'txtPktNo
        '
        Me.txtPktNo.Location = New System.Drawing.Point(534, 67)
        Me.txtPktNo.Name = "txtPktNo"
        Me.txtPktNo.Size = New System.Drawing.Size(70, 21)
        Me.txtPktNo.TabIndex = 20
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(471, 71)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 13)
        Me.Label9.TabIndex = 19
        Me.Label9.Text = "Packet No"
        '
        'txtReason
        '
        Me.txtReason.Location = New System.Drawing.Point(91, 96)
        Me.txtReason.Name = "txtReason"
        Me.txtReason.Size = New System.Drawing.Size(212, 21)
        Me.txtReason.TabIndex = 22
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(33, 100)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 13)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "Reason"
        '
        'txtEstNo
        '
        Me.txtEstNo.Location = New System.Drawing.Point(392, 67)
        Me.txtEstNo.Name = "txtEstNo"
        Me.txtEstNo.Size = New System.Drawing.Size(75, 21)
        Me.txtEstNo.TabIndex = 18
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(307, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Estimation No"
        '
        'chkScan
        '
        Me.chkScan.AutoSize = True
        Me.chkScan.Location = New System.Drawing.Point(619, 20)
        Me.chkScan.Name = "chkScan"
        Me.chkScan.Size = New System.Drawing.Size(78, 17)
        Me.chkScan.TabIndex = 8
        Me.chkScan.Text = "S&can Tag"
        Me.chkScan.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(461, 18)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(73, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "ApprovalNo"
        Me.Label8.Visible = False
        '
        'txtappno
        '
        Me.txtappno.Location = New System.Drawing.Point(537, 14)
        Me.txtappno.Name = "txtappno"
        Me.txtappno.Size = New System.Drawing.Size(65, 21)
        Me.txtappno.TabIndex = 7
        Me.txtappno.Visible = False
        '
        'gridViewFooter
        '
        Me.gridViewFooter.AllowUserToAddRows = False
        Me.gridViewFooter.AllowUserToDeleteRows = False
        Me.gridViewFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewFooter.ColumnHeadersVisible = False
        Me.gridViewFooter.Enabled = False
        Me.gridViewFooter.Location = New System.Drawing.Point(6, 477)
        Me.gridViewFooter.Name = "gridViewFooter"
        Me.gridViewFooter.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridViewFooter.Size = New System.Drawing.Size(791, 37)
        Me.gridViewFooter.TabIndex = 34
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(557, 99)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(54, 13)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "Find Tag"
        '
        'txtFindTag
        '
        Me.txtFindTag.Location = New System.Drawing.Point(619, 96)
        Me.txtFindTag.Name = "txtFindTag"
        Me.txtFindTag.Size = New System.Drawing.Size(93, 21)
        Me.txtFindTag.TabIndex = 24
        '
        'lblItemid
        '
        Me.lblItemid.AutoSize = True
        Me.lblItemid.Location = New System.Drawing.Point(32, 72)
        Me.lblItemid.Name = "lblItemid"
        Me.lblItemid.Size = New System.Drawing.Size(50, 13)
        Me.lblItemid.TabIndex = 13
        Me.lblItemid.Text = "Item Id"
        '
        'txtItemId
        '
        Me.txtItemId.Location = New System.Drawing.Point(91, 68)
        Me.txtItemId.Name = "txtItemId"
        Me.txtItemId.Size = New System.Drawing.Size(65, 21)
        Me.txtItemId.TabIndex = 14
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDate.Location = New System.Drawing.Point(2, 15)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(86, 17)
        Me.chkDate.TabIndex = 0
        Me.chkDate.Text = "Date From"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(212, 13)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(91, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(91, 13)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(90, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkSelectAll
        '
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Location = New System.Drawing.Point(718, 98)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkSelectAll.TabIndex = 25
        Me.chkSelectAll.Text = "&Select All"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'lblRowDet1
        '
        Me.lblRowDet1.AutoSize = True
        Me.lblRowDet1.BackColor = System.Drawing.Color.Aqua
        Me.lblRowDet1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRowDet1.ForeColor = System.Drawing.Color.Red
        Me.lblRowDet1.Location = New System.Drawing.Point(10, 518)
        Me.lblRowDet1.Name = "lblRowDet1"
        Me.lblRowDet1.Size = New System.Drawing.Size(56, 13)
        Me.lblRowDet1.TabIndex = 33
        Me.lblRowDet1.Text = "Lot No :"
        '
        'cmbNewCounter_MAN
        '
        Me.cmbNewCounter_MAN.FormattingEnabled = True
        Me.cmbNewCounter_MAN.Location = New System.Drawing.Point(392, 41)
        Me.cmbNewCounter_MAN.Name = "cmbNewCounter_MAN"
        Me.cmbNewCounter_MAN.Size = New System.Drawing.Size(212, 21)
        Me.cmbNewCounter_MAN.TabIndex = 12
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(188, 17)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(20, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "To"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(166, 72)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(42, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "TagNo"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(307, 44)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(81, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "New Counter"
        '
        'lblOldCounter
        '
        Me.lblOldCounter.AutoSize = True
        Me.lblOldCounter.Location = New System.Drawing.Point(6, 44)
        Me.lblOldCounter.Name = "lblOldCounter"
        Me.lblOldCounter.Size = New System.Drawing.Size(76, 13)
        Me.lblOldCounter.TabIndex = 9
        Me.lblOldCounter.Text = "Old Counter"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(307, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Lot No"
        '
        'txtTagNo
        '
        Me.txtTagNo.Location = New System.Drawing.Point(212, 68)
        Me.txtTagNo.Name = "txtTagNo"
        Me.txtTagNo.Size = New System.Drawing.Size(91, 21)
        Me.txtTagNo.TabIndex = 16
        '
        'txtLotNo_NUM
        '
        Me.txtLotNo_NUM.Location = New System.Drawing.Point(392, 14)
        Me.txtLotNo_NUM.Name = "txtLotNo_NUM"
        Me.txtLotNo_NUM.Size = New System.Drawing.Size(65, 21)
        Me.txtLotNo_NUM.TabIndex = 5
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(418, 123)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(110, 30)
        Me.btnExit.TabIndex = 29
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnCounterChange
        '
        Me.btnCounterChange.Location = New System.Drawing.Point(302, 123)
        Me.btnCounterChange.Name = "btnCounterChange"
        Me.btnCounterChange.Size = New System.Drawing.Size(110, 30)
        Me.btnCounterChange.TabIndex = 28
        Me.btnCounterChange.Text = "&Counter Change"
        Me.btnCounterChange.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(196, 123)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 27
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(90, 123)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 26
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(6, 161)
        Me.gridView.Name = "gridView"
        Me.gridView.Size = New System.Drawing.Size(791, 316)
        Me.gridView.TabIndex = 31
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.OpenToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 70)
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
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'TabMain
        '
        Me.TabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.TabMain.Controls.Add(Me.TabGeneral)
        Me.TabMain.Controls.Add(Me.TabPage2)
        Me.TabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabMain.Location = New System.Drawing.Point(0, 0)
        Me.TabMain.Name = "TabMain"
        Me.TabMain.SelectedIndex = 0
        Me.TabMain.Size = New System.Drawing.Size(840, 575)
        Me.TabMain.TabIndex = 0
        '
        'TabGeneral
        '
        Me.TabGeneral.Controls.Add(Me.grpContainer)
        Me.TabGeneral.Location = New System.Drawing.Point(4, 25)
        Me.TabGeneral.Name = "TabGeneral"
        Me.TabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.TabGeneral.Size = New System.Drawing.Size(832, 546)
        Me.TabGeneral.TabIndex = 0
        Me.TabGeneral.Text = "General"
        Me.TabGeneral.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Panel1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(832, 546)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "View"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(826, 540)
        Me.Panel1.TabIndex = 0
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.dgview)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 84)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(826, 456)
        Me.Panel4.TabIndex = 2
        '
        'dgview
        '
        Me.dgview.AllowUserToAddRows = False
        Me.dgview.AllowUserToDeleteRows = False
        Me.dgview.AllowUserToResizeColumns = False
        Me.dgview.AllowUserToResizeRows = False
        Me.dgview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgview.Location = New System.Drawing.Point(0, 0)
        Me.dgview.Name = "dgview"
        Me.dgview.Size = New System.Drawing.Size(826, 456)
        Me.dgview.TabIndex = 0
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.ChkAll)
        Me.Panel3.Controls.Add(Me.Label11)
        Me.Panel3.Controls.Add(Me.btnDuplicate)
        Me.Panel3.Controls.Add(Me.btnBack)
        Me.Panel3.Controls.Add(Me.Button1)
        Me.Panel3.Controls.Add(Me.dtptabto)
        Me.Panel3.Controls.Add(Me.dtptabfrom)
        Me.Panel3.Controls.Add(Me.Label10)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(826, 84)
        Me.Panel3.TabIndex = 0
        '
        'ChkAll
        '
        Me.ChkAll.AutoSize = True
        Me.ChkAll.Location = New System.Drawing.Point(337, 18)
        Me.ChkAll.Name = "ChkAll"
        Me.ChkAll.Size = New System.Drawing.Size(79, 17)
        Me.ChkAll.TabIndex = 4
        Me.ChkAll.Text = "&Select All"
        Me.ChkAll.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(36, 22)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(71, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Date From "
        '
        'btnDuplicate
        '
        Me.btnDuplicate.Location = New System.Drawing.Point(334, 47)
        Me.btnDuplicate.Name = "btnDuplicate"
        Me.btnDuplicate.Size = New System.Drawing.Size(100, 30)
        Me.btnDuplicate.TabIndex = 7
        Me.btnDuplicate.Text = "&Duplicate Print"
        Me.btnDuplicate.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(229, 48)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(99, 29)
        Me.btnBack.TabIndex = 6
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(119, 47)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(100, 30)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "&Search"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'dtptabto
        '
        Me.dtptabto.Location = New System.Drawing.Point(240, 15)
        Me.dtptabto.Mask = "##/##/####"
        Me.dtptabto.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtptabto.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtptabto.Name = "dtptabto"
        Me.dtptabto.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtptabto.Size = New System.Drawing.Size(91, 21)
        Me.dtptabto.TabIndex = 3
        Me.dtptabto.Text = "07/03/9998"
        Me.dtptabto.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtptabfrom
        '
        Me.dtptabfrom.Location = New System.Drawing.Point(119, 15)
        Me.dtptabfrom.Mask = "##/##/####"
        Me.dtptabfrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtptabfrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtptabfrom.Name = "dtptabfrom"
        Me.dtptabfrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtptabfrom.Size = New System.Drawing.Size(90, 21)
        Me.dtptabfrom.TabIndex = 1
        Me.dtptabfrom.Text = "07/03/9998"
        Me.dtptabfrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(216, 19)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(20, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "To"
        '
        'frmCounterChange
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(840, 575)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.TabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmCounterChange"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Counter Change"
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        CType(Me.gridViewFooter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.TabMain.ResumeLayout(False)
        Me.TabGeneral.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        CType(Me.dgview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents btnCounterChange As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblOldCounter As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtTagNo As System.Windows.Forms.TextBox
    Friend WithEvents txtLotNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblRowDet1 As System.Windows.Forms.Label
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents cmbNewCounter_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkDate As System.Windows.Forms.CheckBox
    Friend WithEvents lblItemid As System.Windows.Forms.Label
    Friend WithEvents txtItemId As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtFindTag As System.Windows.Forms.TextBox
    Friend WithEvents gridViewFooter As System.Windows.Forms.DataGridView
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtappno As System.Windows.Forms.TextBox
    Friend WithEvents chkScan As System.Windows.Forms.CheckBox
    Friend WithEvents txtEstNo As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtReason As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtPktNo As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnview As System.Windows.Forms.Button
    Friend WithEvents TabMain As System.Windows.Forms.TabControl
    Friend WithEvents TabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dgview As System.Windows.Forms.DataGridView
    Friend WithEvents btnDuplicate As System.Windows.Forms.Button
    Friend WithEvents dtptabto As BrighttechPack.DatePicker
    Friend WithEvents dtptabfrom As BrighttechPack.DatePicker
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ChkAll As System.Windows.Forms.CheckBox
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkCmbCounter_Man As BrighttechPack.CheckedComboBox
    Friend WithEvents cmbOldCounter_MAN As System.Windows.Forms.ComboBox
End Class
