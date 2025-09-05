<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddress
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAddress))
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabState = New System.Windows.Forms.TabPage
        Me.txtCountry__Man = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.gridviewState = New System.Windows.Forms.DataGridView
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.btnSGrid = New System.Windows.Forms.Button
        Me.btnSNew = New System.Windows.Forms.Button
        Me.btnSDelete = New System.Windows.Forms.Button
        Me.btnSExit = New System.Windows.Forms.Button
        Me.btnSSave = New System.Windows.Forms.Button
        Me.txtStateId_MAN = New System.Windows.Forms.TextBox
        Me.txtState__Man = New System.Windows.Forms.TextBox
        Me.tabCity = New System.Windows.Forms.TabPage
        Me.Label13 = New System.Windows.Forms.Label
        Me.txtZonal = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtDistrict = New System.Windows.Forms.TextBox
        Me.CmbStateName = New System.Windows.Forms.ComboBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.gridviewCity = New System.Windows.Forms.DataGridView
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.btnCGrid = New System.Windows.Forms.Button
        Me.btnCNew = New System.Windows.Forms.Button
        Me.btnCDelete = New System.Windows.Forms.Button
        Me.btnCExit = New System.Windows.Forms.Button
        Me.btnCSave = New System.Windows.Forms.Button
        Me.txtCityId_MAN = New System.Windows.Forms.TextBox
        Me.txtCity__Man = New System.Windows.Forms.TextBox
        Me.tabArea = New System.Windows.Forms.TabPage
        Me.gridViewArea = New System.Windows.Forms.DataGridView
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtArea__Man = New System.Windows.Forms.TextBox
        Me.CmbCityName = New System.Windows.Forms.ComboBox
        Me.txtAreaId_MAN = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnASave = New System.Windows.Forms.Button
        Me.txtPincode__Man = New System.Windows.Forms.TextBox
        Me.btnAExit = New System.Windows.Forms.Button
        Me.lblStatus = New System.Windows.Forms.Label
        Me.btnADelete = New System.Windows.Forms.Button
        Me.btnANew = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnAGrid = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.tabId = New System.Windows.Forms.TabPage
        Me.txtName = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.txtIFormat = New System.Windows.Forms.TextBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.txtDispOrd_NUM = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.CmbActive = New System.Windows.Forms.ComboBox
        Me.lblActive = New System.Windows.Forms.Label
        Me.txtMaxlen_NUM = New System.Windows.Forms.TextBox
        Me.gridviewIdProof = New System.Windows.Forms.DataGridView
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.btnIGrid = New System.Windows.Forms.Button
        Me.btnINew = New System.Windows.Forms.Button
        Me.btnIDelete = New System.Windows.Forms.Button
        Me.btnIExit = New System.Windows.Forms.Button
        Me.btnISave = New System.Windows.Forms.Button
        Me.txtProofId = New System.Windows.Forms.TextBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GridToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.tabCountry = New System.Windows.Forms.TabPage
        Me.txtCountry_MAN = New System.Windows.Forms.TextBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.txtCountryId_MAN = New System.Windows.Forms.TextBox
        Me.btnCoGrid = New System.Windows.Forms.Button
        Me.btnCoNew = New System.Windows.Forms.Button
        Me.btnCoDelete = New System.Windows.Forms.Button
        Me.btnCoExit = New System.Windows.Forms.Button
        Me.btnCoSave = New System.Windows.Forms.Button
        Me.gridviewCountry = New System.Windows.Forms.DataGridView
        Me.Label23 = New System.Windows.Forms.Label
        Me.tabMain.SuspendLayout()
        Me.tabState.SuspendLayout()
        CType(Me.gridviewState, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabCity.SuspendLayout()
        CType(Me.gridviewCity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabArea.SuspendLayout()
        CType(Me.gridViewArea, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabId.SuspendLayout()
        CType(Me.gridviewIdProof, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.tabCountry.SuspendLayout()
        CType(Me.gridviewCountry, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabCountry)
        Me.tabMain.Controls.Add(Me.tabState)
        Me.tabMain.Controls.Add(Me.tabCity)
        Me.tabMain.Controls.Add(Me.tabArea)
        Me.tabMain.Controls.Add(Me.tabId)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(583, 495)
        Me.tabMain.TabIndex = 0
        '
        'tabState
        '
        Me.tabState.Controls.Add(Me.txtCountry__Man)
        Me.tabState.Controls.Add(Me.Label9)
        Me.tabState.Controls.Add(Me.Label10)
        Me.tabState.Controls.Add(Me.gridviewState)
        Me.tabState.Controls.Add(Me.Label11)
        Me.tabState.Controls.Add(Me.Label12)
        Me.tabState.Controls.Add(Me.btnSGrid)
        Me.tabState.Controls.Add(Me.btnSNew)
        Me.tabState.Controls.Add(Me.btnSDelete)
        Me.tabState.Controls.Add(Me.btnSExit)
        Me.tabState.Controls.Add(Me.btnSSave)
        Me.tabState.Controls.Add(Me.txtStateId_MAN)
        Me.tabState.Controls.Add(Me.txtState__Man)
        Me.tabState.Location = New System.Drawing.Point(4, 22)
        Me.tabState.Name = "tabState"
        Me.tabState.Size = New System.Drawing.Size(575, 469)
        Me.tabState.TabIndex = 3
        Me.tabState.Text = "State"
        Me.tabState.UseVisualStyleBackColor = True
        '
        'txtCountry__Man
        '
        Me.txtCountry__Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCountry__Man.Location = New System.Drawing.Point(134, 75)
        Me.txtCountry__Man.MaxLength = 50
        Me.txtCountry__Man.Name = "txtCountry__Man"
        Me.txtCountry__Man.Size = New System.Drawing.Size(318, 21)
        Me.txtCountry__Man.TabIndex = 18
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Location = New System.Drawing.Point(35, 76)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(90, 13)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "Country Name"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.Red
        Me.Label10.Location = New System.Drawing.Point(27, 448)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(103, 13)
        Me.Label10.TabIndex = 25
        Me.Label10.Text = "*Hit Enter to Edit"
        '
        'gridviewState
        '
        Me.gridviewState.AllowUserToAddRows = False
        Me.gridviewState.AllowUserToDeleteRows = False
        Me.gridviewState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridviewState.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridviewState.Location = New System.Drawing.Point(27, 142)
        Me.gridviewState.MultiSelect = False
        Me.gridviewState.Name = "gridviewState"
        Me.gridviewState.ReadOnly = True
        Me.gridviewState.RowHeadersVisible = False
        Me.gridviewState.RowTemplate.Height = 20
        Me.gridviewState.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridviewState.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridviewState.Size = New System.Drawing.Size(520, 303)
        Me.gridviewState.TabIndex = 24
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Location = New System.Drawing.Point(35, 24)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(19, 13)
        Me.Label11.TabIndex = 13
        Me.Label11.Text = "Id"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Location = New System.Drawing.Point(35, 50)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(40, 13)
        Me.Label12.TabIndex = 15
        Me.Label12.Text = "Name"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSGrid
        '
        Me.btnSGrid.Image = CType(resources.GetObject("btnSGrid.Image"), System.Drawing.Image)
        Me.btnSGrid.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSGrid.Location = New System.Drawing.Point(133, 106)
        Me.btnSGrid.Name = "btnSGrid"
        Me.btnSGrid.Size = New System.Drawing.Size(100, 30)
        Me.btnSGrid.TabIndex = 20
        Me.btnSGrid.Text = "Grid [F2]"
        Me.btnSGrid.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSGrid.UseVisualStyleBackColor = True
        '
        'btnSNew
        '
        Me.btnSNew.Image = CType(resources.GetObject("btnSNew.Image"), System.Drawing.Image)
        Me.btnSNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSNew.Location = New System.Drawing.Point(239, 106)
        Me.btnSNew.Name = "btnSNew"
        Me.btnSNew.Size = New System.Drawing.Size(100, 30)
        Me.btnSNew.TabIndex = 21
        Me.btnSNew.Text = "New [F3]"
        Me.btnSNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSNew.UseVisualStyleBackColor = True
        '
        'btnSDelete
        '
        Me.btnSDelete.Image = CType(resources.GetObject("btnSDelete.Image"), System.Drawing.Image)
        Me.btnSDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSDelete.Location = New System.Drawing.Point(449, 106)
        Me.btnSDelete.Name = "btnSDelete"
        Me.btnSDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnSDelete.TabIndex = 23
        Me.btnSDelete.Text = "&Delete"
        Me.btnSDelete.UseVisualStyleBackColor = True
        Me.btnSDelete.Visible = False
        '
        'btnSExit
        '
        Me.btnSExit.Image = CType(resources.GetObject("btnSExit.Image"), System.Drawing.Image)
        Me.btnSExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSExit.Location = New System.Drawing.Point(344, 106)
        Me.btnSExit.Name = "btnSExit"
        Me.btnSExit.Size = New System.Drawing.Size(100, 30)
        Me.btnSExit.TabIndex = 22
        Me.btnSExit.Text = "Exit [F12]"
        Me.btnSExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSExit.UseVisualStyleBackColor = True
        '
        'btnSSave
        '
        Me.btnSSave.Image = CType(resources.GetObject("btnSSave.Image"), System.Drawing.Image)
        Me.btnSSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSSave.Location = New System.Drawing.Point(27, 106)
        Me.btnSSave.Name = "btnSSave"
        Me.btnSSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSSave.TabIndex = 19
        Me.btnSSave.Text = "Save [F1]"
        Me.btnSSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSSave.UseVisualStyleBackColor = True
        '
        'txtStateId_MAN
        '
        Me.txtStateId_MAN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtStateId_MAN.Enabled = False
        Me.txtStateId_MAN.Location = New System.Drawing.Point(134, 21)
        Me.txtStateId_MAN.MaxLength = 50
        Me.txtStateId_MAN.Name = "txtStateId_MAN"
        Me.txtStateId_MAN.Size = New System.Drawing.Size(100, 21)
        Me.txtStateId_MAN.TabIndex = 14
        '
        'txtState__Man
        '
        Me.txtState__Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtState__Man.Location = New System.Drawing.Point(134, 48)
        Me.txtState__Man.MaxLength = 50
        Me.txtState__Man.Name = "txtState__Man"
        Me.txtState__Man.Size = New System.Drawing.Size(318, 21)
        Me.txtState__Man.TabIndex = 16
        '
        'tabCity
        '
        Me.tabCity.Controls.Add(Me.Label13)
        Me.tabCity.Controls.Add(Me.txtZonal)
        Me.tabCity.Controls.Add(Me.Label14)
        Me.tabCity.Controls.Add(Me.txtDistrict)
        Me.tabCity.Controls.Add(Me.CmbStateName)
        Me.tabCity.Controls.Add(Me.Label15)
        Me.tabCity.Controls.Add(Me.Label16)
        Me.tabCity.Controls.Add(Me.gridviewCity)
        Me.tabCity.Controls.Add(Me.Label17)
        Me.tabCity.Controls.Add(Me.Label18)
        Me.tabCity.Controls.Add(Me.btnCGrid)
        Me.tabCity.Controls.Add(Me.btnCNew)
        Me.tabCity.Controls.Add(Me.btnCDelete)
        Me.tabCity.Controls.Add(Me.btnCExit)
        Me.tabCity.Controls.Add(Me.btnCSave)
        Me.tabCity.Controls.Add(Me.txtCityId_MAN)
        Me.tabCity.Controls.Add(Me.txtCity__Man)
        Me.tabCity.Location = New System.Drawing.Point(4, 22)
        Me.tabCity.Name = "tabCity"
        Me.tabCity.Padding = New System.Windows.Forms.Padding(3)
        Me.tabCity.Size = New System.Drawing.Size(575, 469)
        Me.tabCity.TabIndex = 1
        Me.tabCity.Text = "City"
        Me.tabCity.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.Location = New System.Drawing.Point(35, 93)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(39, 13)
        Me.Label13.TabIndex = 23
        Me.Label13.Text = "Zonal"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtZonal
        '
        Me.txtZonal.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtZonal.Location = New System.Drawing.Point(134, 87)
        Me.txtZonal.MaxLength = 100
        Me.txtZonal.Name = "txtZonal"
        Me.txtZonal.Size = New System.Drawing.Size(318, 21)
        Me.txtZonal.TabIndex = 24
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Location = New System.Drawing.Point(35, 67)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(47, 13)
        Me.Label14.TabIndex = 21
        Me.Label14.Text = "District"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDistrict
        '
        Me.txtDistrict.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDistrict.Location = New System.Drawing.Point(134, 61)
        Me.txtDistrict.MaxLength = 100
        Me.txtDistrict.Name = "txtDistrict"
        Me.txtDistrict.Size = New System.Drawing.Size(318, 21)
        Me.txtDistrict.TabIndex = 22
        '
        'CmbStateName
        '
        Me.CmbStateName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbStateName.FormattingEnabled = True
        Me.CmbStateName.Location = New System.Drawing.Point(134, 113)
        Me.CmbStateName.Name = "CmbStateName"
        Me.CmbStateName.Size = New System.Drawing.Size(318, 21)
        Me.CmbStateName.TabIndex = 26
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.BackColor = System.Drawing.Color.Transparent
        Me.Label15.Location = New System.Drawing.Point(35, 119)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(74, 13)
        Me.Label15.TabIndex = 25
        Me.Label15.Text = "State Name"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ForeColor = System.Drawing.Color.Red
        Me.Label16.Location = New System.Drawing.Point(17, 447)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(103, 13)
        Me.Label16.TabIndex = 33
        Me.Label16.Text = "*Hit Enter to Edit"
        '
        'gridviewCity
        '
        Me.gridviewCity.AllowUserToAddRows = False
        Me.gridviewCity.AllowUserToDeleteRows = False
        Me.gridviewCity.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridviewCity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridviewCity.Location = New System.Drawing.Point(17, 173)
        Me.gridviewCity.MultiSelect = False
        Me.gridviewCity.Name = "gridviewCity"
        Me.gridviewCity.ReadOnly = True
        Me.gridviewCity.RowHeadersVisible = False
        Me.gridviewCity.RowTemplate.Height = 20
        Me.gridviewCity.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridviewCity.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridviewCity.Size = New System.Drawing.Size(520, 271)
        Me.gridviewCity.TabIndex = 32
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.BackColor = System.Drawing.Color.Transparent
        Me.Label17.Location = New System.Drawing.Point(35, 15)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(19, 13)
        Me.Label17.TabIndex = 17
        Me.Label17.Text = "Id"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.BackColor = System.Drawing.Color.Transparent
        Me.Label18.Location = New System.Drawing.Point(35, 40)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(40, 13)
        Me.Label18.TabIndex = 19
        Me.Label18.Text = "Name"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnCGrid
        '
        Me.btnCGrid.Image = CType(resources.GetObject("btnCGrid.Image"), System.Drawing.Image)
        Me.btnCGrid.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCGrid.Location = New System.Drawing.Point(123, 140)
        Me.btnCGrid.Name = "btnCGrid"
        Me.btnCGrid.Size = New System.Drawing.Size(100, 30)
        Me.btnCGrid.TabIndex = 28
        Me.btnCGrid.Text = "Grid [F2]"
        Me.btnCGrid.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCGrid.UseVisualStyleBackColor = True
        '
        'btnCNew
        '
        Me.btnCNew.Image = CType(resources.GetObject("btnCNew.Image"), System.Drawing.Image)
        Me.btnCNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCNew.Location = New System.Drawing.Point(229, 140)
        Me.btnCNew.Name = "btnCNew"
        Me.btnCNew.Size = New System.Drawing.Size(100, 30)
        Me.btnCNew.TabIndex = 29
        Me.btnCNew.Text = "New [F3]"
        Me.btnCNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCNew.UseVisualStyleBackColor = True
        '
        'btnCDelete
        '
        Me.btnCDelete.Image = CType(resources.GetObject("btnCDelete.Image"), System.Drawing.Image)
        Me.btnCDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCDelete.Location = New System.Drawing.Point(441, 140)
        Me.btnCDelete.Name = "btnCDelete"
        Me.btnCDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnCDelete.TabIndex = 31
        Me.btnCDelete.Text = "&Delete"
        Me.btnCDelete.UseVisualStyleBackColor = True
        Me.btnCDelete.Visible = False
        '
        'btnCExit
        '
        Me.btnCExit.Image = CType(resources.GetObject("btnCExit.Image"), System.Drawing.Image)
        Me.btnCExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCExit.Location = New System.Drawing.Point(335, 140)
        Me.btnCExit.Name = "btnCExit"
        Me.btnCExit.Size = New System.Drawing.Size(100, 30)
        Me.btnCExit.TabIndex = 30
        Me.btnCExit.Text = "Exit [F12]"
        Me.btnCExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCExit.UseVisualStyleBackColor = True
        '
        'btnCSave
        '
        Me.btnCSave.Image = CType(resources.GetObject("btnCSave.Image"), System.Drawing.Image)
        Me.btnCSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCSave.Location = New System.Drawing.Point(17, 140)
        Me.btnCSave.Name = "btnCSave"
        Me.btnCSave.Size = New System.Drawing.Size(100, 30)
        Me.btnCSave.TabIndex = 27
        Me.btnCSave.Text = "Save [F1]"
        Me.btnCSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCSave.UseVisualStyleBackColor = True
        '
        'txtCityId_MAN
        '
        Me.txtCityId_MAN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCityId_MAN.Enabled = False
        Me.txtCityId_MAN.Location = New System.Drawing.Point(134, 9)
        Me.txtCityId_MAN.MaxLength = 50
        Me.txtCityId_MAN.Name = "txtCityId_MAN"
        Me.txtCityId_MAN.Size = New System.Drawing.Size(100, 21)
        Me.txtCityId_MAN.TabIndex = 18
        '
        'txtCity__Man
        '
        Me.txtCity__Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCity__Man.Location = New System.Drawing.Point(134, 35)
        Me.txtCity__Man.MaxLength = 50
        Me.txtCity__Man.Name = "txtCity__Man"
        Me.txtCity__Man.Size = New System.Drawing.Size(318, 21)
        Me.txtCity__Man.TabIndex = 20
        '
        'tabArea
        '
        Me.tabArea.BackColor = System.Drawing.Color.White
        Me.tabArea.Controls.Add(Me.gridViewArea)
        Me.tabArea.Controls.Add(Me.Label3)
        Me.tabArea.Controls.Add(Me.txtArea__Man)
        Me.tabArea.Controls.Add(Me.CmbCityName)
        Me.tabArea.Controls.Add(Me.txtAreaId_MAN)
        Me.tabArea.Controls.Add(Me.Label4)
        Me.tabArea.Controls.Add(Me.btnASave)
        Me.tabArea.Controls.Add(Me.txtPincode__Man)
        Me.tabArea.Controls.Add(Me.btnAExit)
        Me.tabArea.Controls.Add(Me.lblStatus)
        Me.tabArea.Controls.Add(Me.btnADelete)
        Me.tabArea.Controls.Add(Me.btnANew)
        Me.tabArea.Controls.Add(Me.Label2)
        Me.tabArea.Controls.Add(Me.btnAGrid)
        Me.tabArea.Controls.Add(Me.Label1)
        Me.tabArea.Location = New System.Drawing.Point(4, 22)
        Me.tabArea.Name = "tabArea"
        Me.tabArea.Padding = New System.Windows.Forms.Padding(3)
        Me.tabArea.Size = New System.Drawing.Size(575, 469)
        Me.tabArea.TabIndex = 0
        Me.tabArea.Text = "Area"
        Me.tabArea.UseVisualStyleBackColor = True
        '
        'gridViewArea
        '
        Me.gridViewArea.AllowUserToAddRows = False
        Me.gridViewArea.AllowUserToDeleteRows = False
        Me.gridViewArea.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridViewArea.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewArea.Location = New System.Drawing.Point(17, 158)
        Me.gridViewArea.MultiSelect = False
        Me.gridViewArea.Name = "gridViewArea"
        Me.gridViewArea.ReadOnly = True
        Me.gridViewArea.RowHeadersVisible = False
        Me.gridViewArea.RowTemplate.Height = 20
        Me.gridViewArea.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridViewArea.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridViewArea.Size = New System.Drawing.Size(520, 285)
        Me.gridViewArea.TabIndex = 28
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(35, 71)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Pincode"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtArea__Man
        '
        Me.txtArea__Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtArea__Man.Location = New System.Drawing.Point(122, 39)
        Me.txtArea__Man.MaxLength = 50
        Me.txtArea__Man.Name = "txtArea__Man"
        Me.txtArea__Man.Size = New System.Drawing.Size(318, 21)
        Me.txtArea__Man.TabIndex = 18
        '
        'CmbCityName
        '
        Me.CmbCityName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbCityName.FormattingEnabled = True
        Me.CmbCityName.Location = New System.Drawing.Point(122, 93)
        Me.CmbCityName.Name = "CmbCityName"
        Me.CmbCityName.Size = New System.Drawing.Size(318, 21)
        Me.CmbCityName.TabIndex = 22
        '
        'txtAreaId_MAN
        '
        Me.txtAreaId_MAN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAreaId_MAN.Enabled = False
        Me.txtAreaId_MAN.Location = New System.Drawing.Point(122, 12)
        Me.txtAreaId_MAN.MaxLength = 50
        Me.txtAreaId_MAN.Name = "txtAreaId_MAN"
        Me.txtAreaId_MAN.Size = New System.Drawing.Size(100, 21)
        Me.txtAreaId_MAN.TabIndex = 16
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(35, 96)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(67, 13)
        Me.Label4.TabIndex = 21
        Me.Label4.Text = "City Name"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnASave
        '
        Me.btnASave.Image = CType(resources.GetObject("btnASave.Image"), System.Drawing.Image)
        Me.btnASave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnASave.Location = New System.Drawing.Point(17, 122)
        Me.btnASave.Name = "btnASave"
        Me.btnASave.Size = New System.Drawing.Size(100, 30)
        Me.btnASave.TabIndex = 23
        Me.btnASave.Text = "Save [F1]"
        Me.btnASave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnASave.UseVisualStyleBackColor = True
        '
        'txtPincode__Man
        '
        Me.txtPincode__Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPincode__Man.Location = New System.Drawing.Point(122, 66)
        Me.txtPincode__Man.MaxLength = 50
        Me.txtPincode__Man.Name = "txtPincode__Man"
        Me.txtPincode__Man.Size = New System.Drawing.Size(100, 21)
        Me.txtPincode__Man.TabIndex = 20
        '
        'btnAExit
        '
        Me.btnAExit.Image = CType(resources.GetObject("btnAExit.Image"), System.Drawing.Image)
        Me.btnAExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAExit.Location = New System.Drawing.Point(332, 122)
        Me.btnAExit.Name = "btnAExit"
        Me.btnAExit.Size = New System.Drawing.Size(100, 30)
        Me.btnAExit.TabIndex = 26
        Me.btnAExit.Text = "Exit [F12]"
        Me.btnAExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAExit.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(17, 446)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 29
        Me.lblStatus.Text = "*Hit Enter to Edit"
        '
        'btnADelete
        '
        Me.btnADelete.Image = CType(resources.GetObject("btnADelete.Image"), System.Drawing.Image)
        Me.btnADelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnADelete.Location = New System.Drawing.Point(437, 122)
        Me.btnADelete.Name = "btnADelete"
        Me.btnADelete.Size = New System.Drawing.Size(100, 30)
        Me.btnADelete.TabIndex = 27
        Me.btnADelete.Text = "&Delete"
        Me.btnADelete.UseVisualStyleBackColor = True
        Me.btnADelete.Visible = False
        '
        'btnANew
        '
        Me.btnANew.Image = CType(resources.GetObject("btnANew.Image"), System.Drawing.Image)
        Me.btnANew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnANew.Location = New System.Drawing.Point(227, 122)
        Me.btnANew.Name = "btnANew"
        Me.btnANew.Size = New System.Drawing.Size(100, 30)
        Me.btnANew.TabIndex = 25
        Me.btnANew.Text = "New [F3]"
        Me.btnANew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnANew.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(35, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(19, 13)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Id"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnAGrid
        '
        Me.btnAGrid.Image = CType(resources.GetObject("btnAGrid.Image"), System.Drawing.Image)
        Me.btnAGrid.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAGrid.Location = New System.Drawing.Point(122, 122)
        Me.btnAGrid.Name = "btnAGrid"
        Me.btnAGrid.Size = New System.Drawing.Size(100, 30)
        Me.btnAGrid.TabIndex = 24
        Me.btnAGrid.Text = "Grid [F2]"
        Me.btnAGrid.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAGrid.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(35, 42)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabId
        '
        Me.tabId.Controls.Add(Me.txtName)
        Me.tabId.Controls.Add(Me.Label20)
        Me.tabId.Controls.Add(Me.txtIFormat)
        Me.tabId.Controls.Add(Me.Label19)
        Me.tabId.Controls.Add(Me.txtDispOrd_NUM)
        Me.tabId.Controls.Add(Me.Label5)
        Me.tabId.Controls.Add(Me.Label6)
        Me.tabId.Controls.Add(Me.CmbActive)
        Me.tabId.Controls.Add(Me.lblActive)
        Me.tabId.Controls.Add(Me.txtMaxlen_NUM)
        Me.tabId.Controls.Add(Me.gridviewIdProof)
        Me.tabId.Controls.Add(Me.Label7)
        Me.tabId.Controls.Add(Me.Label8)
        Me.tabId.Controls.Add(Me.btnIGrid)
        Me.tabId.Controls.Add(Me.btnINew)
        Me.tabId.Controls.Add(Me.btnIDelete)
        Me.tabId.Controls.Add(Me.btnIExit)
        Me.tabId.Controls.Add(Me.btnISave)
        Me.tabId.Controls.Add(Me.txtProofId)
        Me.tabId.Location = New System.Drawing.Point(4, 22)
        Me.tabId.Name = "tabId"
        Me.tabId.Size = New System.Drawing.Size(575, 469)
        Me.tabId.TabIndex = 2
        Me.tabId.Text = "Id Proof"
        Me.tabId.UseVisualStyleBackColor = True
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(122, 41)
        Me.txtName.MaxLength = 25
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(246, 21)
        Me.txtName.TabIndex = 20
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.BackColor = System.Drawing.Color.Transparent
        Me.Label20.Location = New System.Drawing.Point(35, 102)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(47, 13)
        Me.Label20.TabIndex = 23
        Me.Label20.Text = "Format"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtIFormat
        '
        Me.txtIFormat.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtIFormat.Location = New System.Drawing.Point(122, 99)
        Me.txtIFormat.MaxLength = 20
        Me.txtIFormat.Name = "txtIFormat"
        Me.txtIFormat.Size = New System.Drawing.Size(126, 21)
        Me.txtIFormat.TabIndex = 24
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.Color.Red
        Me.Label19.Location = New System.Drawing.Point(27, 446)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(103, 13)
        Me.Label19.TabIndex = 34
        Me.Label19.Text = "*Hit Enter to Edit"
        '
        'txtDispOrd_NUM
        '
        Me.txtDispOrd_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDispOrd_NUM.Location = New System.Drawing.Point(467, 97)
        Me.txtDispOrd_NUM.MaxLength = 50
        Me.txtDispOrd_NUM.Name = "txtDispOrd_NUM"
        Me.txtDispOrd_NUM.Size = New System.Drawing.Size(80, 21)
        Me.txtDispOrd_NUM.TabIndex = 28
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(396, 100)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 13)
        Me.Label5.TabIndex = 27
        Me.Label5.Text = "DispOrder"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(35, 73)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(45, 13)
        Me.Label6.TabIndex = 21
        Me.Label6.Text = "Length"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CmbActive
        '
        Me.CmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbActive.FormattingEnabled = True
        Me.CmbActive.Location = New System.Drawing.Point(312, 97)
        Me.CmbActive.Name = "CmbActive"
        Me.CmbActive.Size = New System.Drawing.Size(56, 21)
        Me.CmbActive.TabIndex = 26
        '
        'lblActive
        '
        Me.lblActive.AutoSize = True
        Me.lblActive.BackColor = System.Drawing.Color.Transparent
        Me.lblActive.Location = New System.Drawing.Point(254, 102)
        Me.lblActive.Name = "lblActive"
        Me.lblActive.Size = New System.Drawing.Size(42, 13)
        Me.lblActive.TabIndex = 25
        Me.lblActive.Text = "Active"
        Me.lblActive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMaxlen_NUM
        '
        Me.txtMaxlen_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMaxlen_NUM.Location = New System.Drawing.Point(122, 70)
        Me.txtMaxlen_NUM.MaxLength = 2
        Me.txtMaxlen_NUM.Name = "txtMaxlen_NUM"
        Me.txtMaxlen_NUM.Size = New System.Drawing.Size(100, 21)
        Me.txtMaxlen_NUM.TabIndex = 22
        '
        'gridviewIdProof
        '
        Me.gridviewIdProof.AllowUserToAddRows = False
        Me.gridviewIdProof.AllowUserToDeleteRows = False
        Me.gridviewIdProof.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridviewIdProof.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridviewIdProof.Location = New System.Drawing.Point(27, 171)
        Me.gridviewIdProof.MultiSelect = False
        Me.gridviewIdProof.Name = "gridviewIdProof"
        Me.gridviewIdProof.ReadOnly = True
        Me.gridviewIdProof.RowHeadersVisible = False
        Me.gridviewIdProof.RowTemplate.Height = 20
        Me.gridviewIdProof.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridviewIdProof.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridviewIdProof.Size = New System.Drawing.Size(520, 272)
        Me.gridviewIdProof.TabIndex = 34
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Location = New System.Drawing.Point(35, 15)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(19, 13)
        Me.Label7.TabIndex = 17
        Me.Label7.Text = "Id"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(35, 44)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(40, 13)
        Me.Label8.TabIndex = 19
        Me.Label8.Text = "Name"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnIGrid
        '
        Me.btnIGrid.Image = CType(resources.GetObject("btnIGrid.Image"), System.Drawing.Image)
        Me.btnIGrid.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnIGrid.Location = New System.Drawing.Point(132, 135)
        Me.btnIGrid.Name = "btnIGrid"
        Me.btnIGrid.Size = New System.Drawing.Size(100, 30)
        Me.btnIGrid.TabIndex = 30
        Me.btnIGrid.Text = "Grid [F2]"
        Me.btnIGrid.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnIGrid.UseVisualStyleBackColor = True
        '
        'btnINew
        '
        Me.btnINew.Image = CType(resources.GetObject("btnINew.Image"), System.Drawing.Image)
        Me.btnINew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnINew.Location = New System.Drawing.Point(237, 135)
        Me.btnINew.Name = "btnINew"
        Me.btnINew.Size = New System.Drawing.Size(100, 30)
        Me.btnINew.TabIndex = 31
        Me.btnINew.Text = "New [F3]"
        Me.btnINew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnINew.UseVisualStyleBackColor = True
        '
        'btnIDelete
        '
        Me.btnIDelete.Image = CType(resources.GetObject("btnIDelete.Image"), System.Drawing.Image)
        Me.btnIDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnIDelete.Location = New System.Drawing.Point(447, 135)
        Me.btnIDelete.Name = "btnIDelete"
        Me.btnIDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnIDelete.TabIndex = 33
        Me.btnIDelete.Text = "&Delete"
        Me.btnIDelete.UseVisualStyleBackColor = True
        Me.btnIDelete.Visible = False
        '
        'btnIExit
        '
        Me.btnIExit.Image = CType(resources.GetObject("btnIExit.Image"), System.Drawing.Image)
        Me.btnIExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnIExit.Location = New System.Drawing.Point(342, 135)
        Me.btnIExit.Name = "btnIExit"
        Me.btnIExit.Size = New System.Drawing.Size(100, 30)
        Me.btnIExit.TabIndex = 32
        Me.btnIExit.Text = "Exit [F12]"
        Me.btnIExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnIExit.UseVisualStyleBackColor = True
        '
        'btnISave
        '
        Me.btnISave.Image = CType(resources.GetObject("btnISave.Image"), System.Drawing.Image)
        Me.btnISave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnISave.Location = New System.Drawing.Point(27, 135)
        Me.btnISave.Name = "btnISave"
        Me.btnISave.Size = New System.Drawing.Size(100, 30)
        Me.btnISave.TabIndex = 29
        Me.btnISave.Text = "Save [F1]"
        Me.btnISave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnISave.UseVisualStyleBackColor = True
        '
        'txtProofId
        '
        Me.txtProofId.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtProofId.Enabled = False
        Me.txtProofId.Location = New System.Drawing.Point(122, 12)
        Me.txtProofId.MaxLength = 50
        Me.txtProofId.Name = "txtProofId"
        Me.txtProofId.Size = New System.Drawing.Size(100, 21)
        Me.txtProofId.TabIndex = 18
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.GridToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.DeleteToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 114)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'GridToolStripMenuItem
        '
        Me.GridToolStripMenuItem.Name = "GridToolStripMenuItem"
        Me.GridToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.GridToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.GridToolStripMenuItem.Text = "Grid"
        Me.GridToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.DeleteToolStripMenuItem.Text = "Delete"
        Me.DeleteToolStripMenuItem.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.tabMain)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(583, 495)
        Me.Panel1.TabIndex = 2
        '
        'tabCountry
        '
        Me.tabCountry.Controls.Add(Me.Label23)
        Me.tabCountry.Controls.Add(Me.gridviewCountry)
        Me.tabCountry.Controls.Add(Me.btnCoGrid)
        Me.tabCountry.Controls.Add(Me.btnCoNew)
        Me.tabCountry.Controls.Add(Me.btnCoDelete)
        Me.tabCountry.Controls.Add(Me.btnCoExit)
        Me.tabCountry.Controls.Add(Me.btnCoSave)
        Me.tabCountry.Controls.Add(Me.txtCountry_MAN)
        Me.tabCountry.Controls.Add(Me.Label21)
        Me.tabCountry.Controls.Add(Me.Label22)
        Me.tabCountry.Controls.Add(Me.txtCountryId_MAN)
        Me.tabCountry.Location = New System.Drawing.Point(4, 22)
        Me.tabCountry.Name = "tabCountry"
        Me.tabCountry.Size = New System.Drawing.Size(575, 469)
        Me.tabCountry.TabIndex = 4
        Me.tabCountry.Text = "Country"
        Me.tabCountry.UseVisualStyleBackColor = True
        '
        'txtCountry_MAN
        '
        Me.txtCountry_MAN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCountry_MAN.Location = New System.Drawing.Point(134, 46)
        Me.txtCountry_MAN.MaxLength = 50
        Me.txtCountry_MAN.Name = "txtCountry_MAN"
        Me.txtCountry_MAN.Size = New System.Drawing.Size(227, 21)
        Me.txtCountry_MAN.TabIndex = 3
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.BackColor = System.Drawing.Color.Transparent
        Me.Label21.Location = New System.Drawing.Point(35, 49)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(90, 13)
        Me.Label21.TabIndex = 2
        Me.Label21.Text = "Country Name"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.BackColor = System.Drawing.Color.Transparent
        Me.Label22.Location = New System.Drawing.Point(35, 24)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(19, 13)
        Me.Label22.TabIndex = 0
        Me.Label22.Text = "Id"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCountryId_MAN
        '
        Me.txtCountryId_MAN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCountryId_MAN.Enabled = False
        Me.txtCountryId_MAN.Location = New System.Drawing.Point(134, 21)
        Me.txtCountryId_MAN.MaxLength = 50
        Me.txtCountryId_MAN.Name = "txtCountryId_MAN"
        Me.txtCountryId_MAN.Size = New System.Drawing.Size(100, 21)
        Me.txtCountryId_MAN.TabIndex = 1
        '
        'btnCoGrid
        '
        Me.btnCoGrid.Image = CType(resources.GetObject("btnCoGrid.Image"), System.Drawing.Image)
        Me.btnCoGrid.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCoGrid.Location = New System.Drawing.Point(136, 82)
        Me.btnCoGrid.Name = "btnCoGrid"
        Me.btnCoGrid.Size = New System.Drawing.Size(100, 30)
        Me.btnCoGrid.TabIndex = 5
        Me.btnCoGrid.Text = "Grid [F2]"
        Me.btnCoGrid.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCoGrid.UseVisualStyleBackColor = True
        '
        'btnCoNew
        '
        Me.btnCoNew.Image = CType(resources.GetObject("btnCoNew.Image"), System.Drawing.Image)
        Me.btnCoNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCoNew.Location = New System.Drawing.Point(242, 82)
        Me.btnCoNew.Name = "btnCoNew"
        Me.btnCoNew.Size = New System.Drawing.Size(100, 30)
        Me.btnCoNew.TabIndex = 6
        Me.btnCoNew.Text = "New [F3]"
        Me.btnCoNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCoNew.UseVisualStyleBackColor = True
        '
        'btnCoDelete
        '
        Me.btnCoDelete.Image = CType(resources.GetObject("btnCoDelete.Image"), System.Drawing.Image)
        Me.btnCoDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCoDelete.Location = New System.Drawing.Point(452, 82)
        Me.btnCoDelete.Name = "btnCoDelete"
        Me.btnCoDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnCoDelete.TabIndex = 8
        Me.btnCoDelete.Text = "&Delete"
        Me.btnCoDelete.UseVisualStyleBackColor = True
        Me.btnCoDelete.Visible = False
        '
        'btnCoExit
        '
        Me.btnCoExit.Image = CType(resources.GetObject("btnCoExit.Image"), System.Drawing.Image)
        Me.btnCoExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCoExit.Location = New System.Drawing.Point(347, 82)
        Me.btnCoExit.Name = "btnCoExit"
        Me.btnCoExit.Size = New System.Drawing.Size(100, 30)
        Me.btnCoExit.TabIndex = 7
        Me.btnCoExit.Text = "Exit [F12]"
        Me.btnCoExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCoExit.UseVisualStyleBackColor = True
        '
        'btnCoSave
        '
        Me.btnCoSave.Image = CType(resources.GetObject("btnCoSave.Image"), System.Drawing.Image)
        Me.btnCoSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCoSave.Location = New System.Drawing.Point(30, 82)
        Me.btnCoSave.Name = "btnCoSave"
        Me.btnCoSave.Size = New System.Drawing.Size(100, 30)
        Me.btnCoSave.TabIndex = 4
        Me.btnCoSave.Text = "Save [F1]"
        Me.btnCoSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCoSave.UseVisualStyleBackColor = True
        '
        'gridviewCountry
        '
        Me.gridviewCountry.AllowUserToAddRows = False
        Me.gridviewCountry.AllowUserToDeleteRows = False
        Me.gridviewCountry.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridviewCountry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridviewCountry.Location = New System.Drawing.Point(28, 121)
        Me.gridviewCountry.MultiSelect = False
        Me.gridviewCountry.Name = "gridviewCountry"
        Me.gridviewCountry.ReadOnly = True
        Me.gridviewCountry.RowHeadersVisible = False
        Me.gridviewCountry.RowTemplate.Height = 20
        Me.gridviewCountry.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridviewCountry.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridviewCountry.Size = New System.Drawing.Size(528, 321)
        Me.gridviewCountry.TabIndex = 9
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.ForeColor = System.Drawing.Color.Red
        Me.Label23.Location = New System.Drawing.Point(28, 446)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(103, 13)
        Me.Label23.TabIndex = 10
        Me.Label23.Text = "*Hit Enter to Edit"
        '
        'frmAddress
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(583, 495)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmAddress"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Address"
        Me.tabMain.ResumeLayout(False)
        Me.tabState.ResumeLayout(False)
        Me.tabState.PerformLayout()
        CType(Me.gridviewState, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabCity.ResumeLayout(False)
        Me.tabCity.PerformLayout()
        CType(Me.gridviewCity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabArea.ResumeLayout(False)
        Me.tabArea.PerformLayout()
        CType(Me.gridViewArea, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabId.ResumeLayout(False)
        Me.tabId.PerformLayout()
        CType(Me.gridviewIdProof, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.tabCountry.ResumeLayout(False)
        Me.tabCountry.PerformLayout()
        CType(Me.gridviewCountry, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabArea As System.Windows.Forms.TabPage
    Friend WithEvents tabCity As System.Windows.Forms.TabPage
    Friend WithEvents tabId As System.Windows.Forms.TabPage
    Friend WithEvents tabState As System.Windows.Forms.TabPage
    Friend WithEvents gridViewArea As System.Windows.Forms.DataGridView
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtArea__Man As System.Windows.Forms.TextBox
    Friend WithEvents CmbCityName As System.Windows.Forms.ComboBox
    Friend WithEvents txtAreaId_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnASave As System.Windows.Forms.Button
    Friend WithEvents txtPincode__Man As System.Windows.Forms.TextBox
    Friend WithEvents btnAExit As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnADelete As System.Windows.Forms.Button
    Friend WithEvents btnANew As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnAGrid As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtDispOrd_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents CmbActive As System.Windows.Forms.ComboBox
    Friend WithEvents lblActive As System.Windows.Forms.Label
    Friend WithEvents txtMaxlen_NUM As System.Windows.Forms.TextBox
    Friend WithEvents gridviewIdProof As System.Windows.Forms.DataGridView
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnIGrid As System.Windows.Forms.Button
    Friend WithEvents btnINew As System.Windows.Forms.Button
    Friend WithEvents btnIDelete As System.Windows.Forms.Button
    Friend WithEvents btnIExit As System.Windows.Forms.Button
    Friend WithEvents btnISave As System.Windows.Forms.Button
    Friend WithEvents txtProofId As System.Windows.Forms.TextBox
    Friend WithEvents txtCountry__Man As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents gridviewState As System.Windows.Forms.DataGridView
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents btnSGrid As System.Windows.Forms.Button
    Friend WithEvents btnSNew As System.Windows.Forms.Button
    Friend WithEvents btnSDelete As System.Windows.Forms.Button
    Friend WithEvents btnSExit As System.Windows.Forms.Button
    Friend WithEvents btnSSave As System.Windows.Forms.Button
    Friend WithEvents txtStateId_MAN As System.Windows.Forms.TextBox
    Friend WithEvents txtState__Man As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtZonal As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtDistrict As System.Windows.Forms.TextBox
    Friend WithEvents CmbStateName As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents gridviewCity As System.Windows.Forms.DataGridView
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents btnCGrid As System.Windows.Forms.Button
    Friend WithEvents btnCNew As System.Windows.Forms.Button
    Friend WithEvents btnCDelete As System.Windows.Forms.Button
    Friend WithEvents btnCExit As System.Windows.Forms.Button
    Friend WithEvents btnCSave As System.Windows.Forms.Button
    Friend WithEvents txtCityId_MAN As System.Windows.Forms.TextBox
    Friend WithEvents txtCity__Man As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GridToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtIFormat As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents tabCountry As System.Windows.Forms.TabPage
    Friend WithEvents txtCountry_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents txtCountryId_MAN As System.Windows.Forms.TextBox
    Friend WithEvents btnCoGrid As System.Windows.Forms.Button
    Friend WithEvents btnCoNew As System.Windows.Forms.Button
    Friend WithEvents btnCoDelete As System.Windows.Forms.Button
    Friend WithEvents btnCoExit As System.Windows.Forms.Button
    Friend WithEvents btnCoSave As System.Windows.Forms.Button
    Friend WithEvents gridviewCountry As System.Windows.Forms.DataGridView
    Friend WithEvents Label23 As System.Windows.Forms.Label
End Class
