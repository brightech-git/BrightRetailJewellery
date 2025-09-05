<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTrackingCustomer
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblcount1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbtNewCustomer = New System.Windows.Forms.RadioButton()
        Me.rbtOldCustomer = New System.Windows.Forms.RadioButton()
        Me.rbtLostCustomer = New System.Windows.Forms.RadioButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.rbtLiveCustomer = New System.Windows.Forms.RadioButton()
        Me.rbtCustomerWise = New System.Windows.Forms.RadioButton()
        Me.rbtDateWise = New System.Windows.Forms.RadioButton()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnTemplate = New System.Windows.Forms.Button()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox()
        Me.Label = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.grpDetail = New System.Windows.Forms.GroupBox()
        Me.chkPhoneRes = New System.Windows.Forms.CheckBox()
        Me.chkCountry = New System.Windows.Forms.CheckBox()
        Me.chkPincode = New System.Windows.Forms.CheckBox()
        Me.chkState = New System.Windows.Forms.CheckBox()
        Me.chkCity = New System.Windows.Forms.CheckBox()
        Me.chkArea = New System.Windows.Forms.CheckBox()
        Me.chkAddress = New System.Windows.Forms.CheckBox()
        Me.chkMore = New System.Windows.Forms.CheckBox()
        Me.txtSearchBy = New System.Windows.Forms.TextBox()
        Me.cmbSearchBy = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.gridDetail = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.grpDetail.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.gridDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(184, 37)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(89, 21)
        Me.dtpTo.TabIndex = 6
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(184, 7)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(89, 21)
        Me.dtpFrom.TabIndex = 4
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(140, 41)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(20, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(137, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(36, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "From"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblcount1)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.rbtLiveCustomer)
        Me.Panel1.Controls.Add(Me.rbtCustomerWise)
        Me.Panel1.Controls.Add(Me.rbtDateWise)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.btnTemplate)
        Me.Panel1.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel1.Controls.Add(Me.chkCmbCompany)
        Me.Panel1.Controls.Add(Me.Label)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.grpDetail)
        Me.Panel1.Controls.Add(Me.chkMore)
        Me.Panel1.Controls.Add(Me.txtSearchBy)
        Me.Panel1.Controls.Add(Me.cmbSearchBy)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnView_Search)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1279, 130)
        Me.Panel1.TabIndex = 1
        '
        'lblcount1
        '
        Me.lblcount1.AutoSize = True
        Me.lblcount1.Location = New System.Drawing.Point(922, 101)
        Me.lblcount1.Name = "lblcount1"
        Me.lblcount1.Size = New System.Drawing.Size(19, 13)
        Me.lblcount1.TabIndex = 29
        Me.lblcount1.Text = "..."
        Me.lblcount1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtNewCustomer)
        Me.GroupBox1.Controls.Add(Me.rbtOldCustomer)
        Me.GroupBox1.Controls.Add(Me.rbtLostCustomer)
        Me.GroupBox1.Location = New System.Drawing.Point(552, 65)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(161, 43)
        Me.GroupBox1.TabIndex = 22
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Customer"
        '
        'rbtNewCustomer
        '
        Me.rbtNewCustomer.AutoSize = True
        Me.rbtNewCustomer.Checked = True
        Me.rbtNewCustomer.Location = New System.Drawing.Point(6, 20)
        Me.rbtNewCustomer.Name = "rbtNewCustomer"
        Me.rbtNewCustomer.Size = New System.Drawing.Size(49, 17)
        Me.rbtNewCustomer.TabIndex = 0
        Me.rbtNewCustomer.TabStop = True
        Me.rbtNewCustomer.Text = "New"
        Me.rbtNewCustomer.UseVisualStyleBackColor = True
        '
        'rbtOldCustomer
        '
        Me.rbtOldCustomer.AutoSize = True
        Me.rbtOldCustomer.Location = New System.Drawing.Point(61, 20)
        Me.rbtOldCustomer.Name = "rbtOldCustomer"
        Me.rbtOldCustomer.Size = New System.Drawing.Size(44, 17)
        Me.rbtOldCustomer.TabIndex = 1
        Me.rbtOldCustomer.Text = "Old"
        Me.rbtOldCustomer.UseVisualStyleBackColor = True
        '
        'rbtLostCustomer
        '
        Me.rbtLostCustomer.AutoSize = True
        Me.rbtLostCustomer.Location = New System.Drawing.Point(111, 20)
        Me.rbtLostCustomer.Name = "rbtLostCustomer"
        Me.rbtLostCustomer.Size = New System.Drawing.Size(48, 17)
        Me.rbtLostCustomer.TabIndex = 2
        Me.rbtLostCustomer.Text = "Lost"
        Me.rbtLostCustomer.UseVisualStyleBackColor = True
        Me.rbtLostCustomer.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(567, 41)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(75, 13)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "Search Text"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rbtLiveCustomer
        '
        Me.rbtLiveCustomer.AutoSize = True
        Me.rbtLiveCustomer.Location = New System.Drawing.Point(18, 47)
        Me.rbtLiveCustomer.Name = "rbtLiveCustomer"
        Me.rbtLiveCustomer.Size = New System.Drawing.Size(128, 17)
        Me.rbtLiveCustomer.TabIndex = 2
        Me.rbtLiveCustomer.Text = "Live Db Customer"
        Me.rbtLiveCustomer.UseVisualStyleBackColor = True
        '
        'rbtCustomerWise
        '
        Me.rbtCustomerWise.AutoSize = True
        Me.rbtCustomerWise.Location = New System.Drawing.Point(18, 27)
        Me.rbtCustomerWise.Name = "rbtCustomerWise"
        Me.rbtCustomerWise.Size = New System.Drawing.Size(108, 17)
        Me.rbtCustomerWise.TabIndex = 1
        Me.rbtCustomerWise.Text = "CustomerWise"
        Me.rbtCustomerWise.UseVisualStyleBackColor = True
        '
        'rbtDateWise
        '
        Me.rbtDateWise.AutoSize = True
        Me.rbtDateWise.Checked = True
        Me.rbtDateWise.Location = New System.Drawing.Point(18, 8)
        Me.rbtDateWise.Name = "rbtDateWise"
        Me.rbtDateWise.Size = New System.Drawing.Size(79, 17)
        Me.rbtDateWise.TabIndex = 0
        Me.rbtDateWise.TabStop = True
        Me.rbtDateWise.Text = "DateWise"
        Me.rbtDateWise.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(815, 72)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 24)
        Me.btnSearch.TabIndex = 24
        Me.btnSearch.TabStop = False
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnTemplate
        '
        Me.btnTemplate.Location = New System.Drawing.Point(717, 72)
        Me.btnTemplate.Name = "btnTemplate"
        Me.btnTemplate.Size = New System.Drawing.Size(95, 24)
        Me.btnTemplate.TabIndex = 23
        Me.btnTemplate.TabStop = False
        Me.btnTemplate.Text = "Template"
        Me.btnTemplate.UseVisualStyleBackColor = True
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(649, 9)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(246, 22)
        Me.chkCmbCostCentre.TabIndex = 10
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(348, 9)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(213, 22)
        Me.chkCmbCompany.TabIndex = 8
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(567, 14)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 9
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(280, 14)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(62, 13)
        Me.Label9.TabIndex = 7
        Me.Label9.Text = "Company"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpDetail
        '
        Me.grpDetail.Controls.Add(Me.chkPhoneRes)
        Me.grpDetail.Controls.Add(Me.chkCountry)
        Me.grpDetail.Controls.Add(Me.chkPincode)
        Me.grpDetail.Controls.Add(Me.chkState)
        Me.grpDetail.Controls.Add(Me.chkCity)
        Me.grpDetail.Controls.Add(Me.chkArea)
        Me.grpDetail.Controls.Add(Me.chkAddress)
        Me.grpDetail.Location = New System.Drawing.Point(970, 7)
        Me.grpDetail.Name = "grpDetail"
        Me.grpDetail.Size = New System.Drawing.Size(260, 91)
        Me.grpDetail.TabIndex = 16
        Me.grpDetail.TabStop = False
        '
        'chkPhoneRes
        '
        Me.chkPhoneRes.AutoSize = True
        Me.chkPhoneRes.Checked = True
        Me.chkPhoneRes.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPhoneRes.Location = New System.Drawing.Point(168, 20)
        Me.chkPhoneRes.Name = "chkPhoneRes"
        Me.chkPhoneRes.Size = New System.Drawing.Size(86, 17)
        Me.chkPhoneRes.TabIndex = 6
        Me.chkPhoneRes.Text = "Phone Res"
        Me.chkPhoneRes.UseVisualStyleBackColor = True
        '
        'chkCountry
        '
        Me.chkCountry.AutoSize = True
        Me.chkCountry.Checked = True
        Me.chkCountry.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCountry.Location = New System.Drawing.Point(94, 66)
        Me.chkCountry.Name = "chkCountry"
        Me.chkCountry.Size = New System.Drawing.Size(72, 17)
        Me.chkCountry.TabIndex = 5
        Me.chkCountry.Text = "Country"
        Me.chkCountry.UseVisualStyleBackColor = True
        '
        'chkPincode
        '
        Me.chkPincode.AutoSize = True
        Me.chkPincode.Checked = True
        Me.chkPincode.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPincode.Location = New System.Drawing.Point(94, 43)
        Me.chkPincode.Name = "chkPincode"
        Me.chkPincode.Size = New System.Drawing.Size(70, 17)
        Me.chkPincode.TabIndex = 4
        Me.chkPincode.Text = "Pincode"
        Me.chkPincode.UseVisualStyleBackColor = True
        '
        'chkState
        '
        Me.chkState.AutoSize = True
        Me.chkState.Checked = True
        Me.chkState.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkState.Location = New System.Drawing.Point(94, 20)
        Me.chkState.Name = "chkState"
        Me.chkState.Size = New System.Drawing.Size(56, 17)
        Me.chkState.TabIndex = 3
        Me.chkState.Text = "State"
        Me.chkState.UseVisualStyleBackColor = True
        '
        'chkCity
        '
        Me.chkCity.AutoSize = True
        Me.chkCity.Checked = True
        Me.chkCity.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCity.Location = New System.Drawing.Point(16, 66)
        Me.chkCity.Name = "chkCity"
        Me.chkCity.Size = New System.Drawing.Size(49, 17)
        Me.chkCity.TabIndex = 2
        Me.chkCity.Text = "City"
        Me.chkCity.UseVisualStyleBackColor = True
        '
        'chkArea
        '
        Me.chkArea.AutoSize = True
        Me.chkArea.Checked = True
        Me.chkArea.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkArea.Location = New System.Drawing.Point(16, 43)
        Me.chkArea.Name = "chkArea"
        Me.chkArea.Size = New System.Drawing.Size(53, 17)
        Me.chkArea.TabIndex = 1
        Me.chkArea.Text = "Area"
        Me.chkArea.UseVisualStyleBackColor = True
        '
        'chkAddress
        '
        Me.chkAddress.AutoSize = True
        Me.chkAddress.Checked = True
        Me.chkAddress.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAddress.Location = New System.Drawing.Point(16, 20)
        Me.chkAddress.Name = "chkAddress"
        Me.chkAddress.Size = New System.Drawing.Size(72, 17)
        Me.chkAddress.TabIndex = 0
        Me.chkAddress.Text = "Address"
        Me.chkAddress.UseVisualStyleBackColor = True
        '
        'chkMore
        '
        Me.chkMore.AutoSize = True
        Me.chkMore.Checked = True
        Me.chkMore.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMore.Location = New System.Drawing.Point(901, 14)
        Me.chkMore.Name = "chkMore"
        Me.chkMore.Size = New System.Drawing.Size(54, 17)
        Me.chkMore.TabIndex = 15
        Me.chkMore.Text = "More"
        Me.chkMore.UseVisualStyleBackColor = True
        '
        'txtSearchBy
        '
        Me.txtSearchBy.Location = New System.Drawing.Point(649, 37)
        Me.txtSearchBy.Name = "txtSearchBy"
        Me.txtSearchBy.Size = New System.Drawing.Size(246, 21)
        Me.txtSearchBy.TabIndex = 14
        '
        'cmbSearchBy
        '
        Me.cmbSearchBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSearchBy.FormattingEnabled = True
        Me.cmbSearchBy.Location = New System.Drawing.Point(348, 37)
        Me.cmbSearchBy.Name = "cmbSearchBy"
        Me.cmbSearchBy.Size = New System.Drawing.Size(210, 21)
        Me.cmbSearchBy.TabIndex = 12
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(278, 41)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(66, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Search By"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(15, 109)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 13)
        Me.Label2.TabIndex = 28
        Me.Label2.Text = "*Press D Detail"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(445, 69)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 21
        Me.btnExit.TabStop = False
        Me.btnExit.Text = "&Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(338, 69)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 20
        Me.btnPrint.TabStop = False
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(231, 69)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 19
        Me.btnExport.TabStop = False
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(124, 69)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 18
        Me.btnNew.TabStop = False
        Me.btnNew.Text = "&New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(17, 69)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 17
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Panel4)
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 130)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1279, 478)
        Me.Panel2.TabIndex = 8
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.gridView)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1279, 378)
        Me.Panel4.TabIndex = 3
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.AllowUserToResizeColumns = False
        Me.gridView.AllowUserToResizeRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.ContextMenuStrip2
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1279, 378)
        Me.gridView.StandardTab = True
        Me.gridView.TabIndex = 0
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoResizeToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(133, 26)
        '
        'AutoResizeToolStripMenuItem
        '
        Me.AutoResizeToolStripMenuItem.CheckOnClick = True
        Me.AutoResizeToolStripMenuItem.Name = "AutoResizeToolStripMenuItem"
        Me.AutoResizeToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.AutoResizeToolStripMenuItem.Text = "AutoResize"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.gridDetail)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 378)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1279, 100)
        Me.Panel3.TabIndex = 2
        '
        'gridDetail
        '
        Me.gridDetail.AllowUserToAddRows = False
        Me.gridDetail.AllowUserToDeleteRows = False
        Me.gridDetail.AllowUserToResizeColumns = False
        Me.gridDetail.AllowUserToResizeRows = False
        Me.gridDetail.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridDetail.Location = New System.Drawing.Point(0, 0)
        Me.gridDetail.MultiSelect = False
        Me.gridDetail.Name = "gridDetail"
        Me.gridDetail.ReadOnly = True
        Me.gridDetail.RowHeadersVisible = False
        Me.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.gridDetail.Size = New System.Drawing.Size(1279, 100)
        Me.gridDetail.TabIndex = 1
        Me.gridDetail.TabStop = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "&New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'frmTrackingCustomer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1279, 608)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmTrackingCustomer"
        Me.Text = "Track Customer"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.grpDetail.ResumeLayout(False)
        Me.grpDetail.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        CType(Me.gridDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label3 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents rbtLostCustomer As RadioButton
    Friend WithEvents rbtOldCustomer As RadioButton
    Friend WithEvents rbtNewCustomer As RadioButton
    Friend WithEvents btnExit As Button
    Friend WithEvents btnPrint As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents btnNew As Button
    Friend WithEvents btnView_Search As Button
    Friend WithEvents gridView As DataGridView
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Label2 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents ContextMenuStrip2 As ContextMenuStrip
    Friend WithEvents AutoResizeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents cmbSearchBy As ComboBox
    Friend WithEvents txtSearchBy As TextBox
    Friend WithEvents grpDetail As GroupBox
    Friend WithEvents chkPhoneRes As CheckBox
    Friend WithEvents chkCountry As CheckBox
    Friend WithEvents chkPincode As CheckBox
    Friend WithEvents chkState As CheckBox
    Friend WithEvents chkCity As CheckBox
    Friend WithEvents chkArea As CheckBox
    Friend WithEvents chkAddress As CheckBox
    Friend WithEvents chkMore As CheckBox
    Friend WithEvents gridDetail As DataGridView
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnTemplate As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents rbtLiveCustomer As RadioButton
    Friend WithEvents rbtCustomerWise As RadioButton
    Friend WithEvents rbtDateWise As RadioButton
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblcount1 As Label
End Class
