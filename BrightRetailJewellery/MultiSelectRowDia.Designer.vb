<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MultiSelectRowDia
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
        Me.GrpSelection = New CodeVendor.Controls.Grouper()
        Me.txtSearchItem = New System.Windows.Forms.TextBox()
        Me.lblSearchTagNo = New System.Windows.Forms.Label()
        Me.txtSearchTagNo = New System.Windows.Forms.TextBox()
        Me.chkAppSales = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.Dgv = New System.Windows.Forms.DataGridView()
        Me.lblTotNetwt = New System.Windows.Forms.Label()
        Me.lblTotGrswt = New System.Windows.Forms.Label()
        Me.lblTotPcs = New System.Windows.Forms.Label()
        Me.GrpSelection.SuspendLayout()
        CType(Me.Dgv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GrpSelection
        '
        Me.GrpSelection.BackgroundColor = System.Drawing.Color.Lavender
        Me.GrpSelection.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.GrpSelection.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.GrpSelection.BorderColor = System.Drawing.Color.Empty
        Me.GrpSelection.BorderThickness = 1.0!
        Me.GrpSelection.Controls.Add(Me.lblTotNetwt)
        Me.GrpSelection.Controls.Add(Me.lblTotGrswt)
        Me.GrpSelection.Controls.Add(Me.lblTotPcs)
        Me.GrpSelection.Controls.Add(Me.txtSearchItem)
        Me.GrpSelection.Controls.Add(Me.lblSearchTagNo)
        Me.GrpSelection.Controls.Add(Me.txtSearchTagNo)
        Me.GrpSelection.Controls.Add(Me.chkAppSales)
        Me.GrpSelection.Controls.Add(Me.Label2)
        Me.GrpSelection.Controls.Add(Me.Label1)
        Me.GrpSelection.Controls.Add(Me.chkSelectAll)
        Me.GrpSelection.Controls.Add(Me.btnCancel)
        Me.GrpSelection.Controls.Add(Me.btnOk)
        Me.GrpSelection.Controls.Add(Me.Dgv)
        Me.GrpSelection.CustomGroupBoxColor = System.Drawing.Color.White
        Me.GrpSelection.GroupImage = Nothing
        Me.GrpSelection.GroupTitle = ""
        Me.GrpSelection.Location = New System.Drawing.Point(10, -1)
        Me.GrpSelection.Name = "GrpSelection"
        Me.GrpSelection.Padding = New System.Windows.Forms.Padding(20)
        Me.GrpSelection.PaintGroupBox = False
        Me.GrpSelection.RoundCorners = 10
        Me.GrpSelection.ShadowColor = System.Drawing.Color.DarkGray
        Me.GrpSelection.ShadowControl = False
        Me.GrpSelection.ShadowThickness = 3
        Me.GrpSelection.Size = New System.Drawing.Size(846, 331)
        Me.GrpSelection.TabIndex = 0
        '
        'txtSearchItem
        '
        Me.txtSearchItem.Location = New System.Drawing.Point(648, 308)
        Me.txtSearchItem.Name = "txtSearchItem"
        Me.txtSearchItem.Size = New System.Drawing.Size(60, 21)
        Me.txtSearchItem.TabIndex = 8
        '
        'lblSearchTagNo
        '
        Me.lblSearchTagNo.AutoSize = True
        Me.lblSearchTagNo.Location = New System.Drawing.Point(502, 312)
        Me.lblSearchTagNo.Name = "lblSearchTagNo"
        Me.lblSearchTagNo.Size = New System.Drawing.Size(139, 13)
        Me.lblSearchTagNo.TabIndex = 7
        Me.lblSearchTagNo.Text = "&Search Item id & Tag No"
        '
        'txtSearchTagNo
        '
        Me.txtSearchTagNo.Location = New System.Drawing.Point(714, 308)
        Me.txtSearchTagNo.Name = "txtSearchTagNo"
        Me.txtSearchTagNo.Size = New System.Drawing.Size(109, 21)
        Me.txtSearchTagNo.TabIndex = 6
        '
        'chkAppSales
        '
        Me.chkAppSales.AutoSize = True
        Me.chkAppSales.Location = New System.Drawing.Point(612, 291)
        Me.chkAppSales.Name = "chkAppSales"
        Me.chkAppSales.Size = New System.Drawing.Size(223, 17)
        Me.chkAppSales.TabIndex = 5
        Me.chkAppSales.Text = "Approval Receipt to UnSelect Item"
        Me.chkAppSales.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(223, 277)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(128, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "[Ctrl + F] Search"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(223, 291)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(146, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "[Ctrl + A] Select All"
        '
        'chkSelectAll
        '
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Location = New System.Drawing.Point(226, 308)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(75, 17)
        Me.chkSelectAll.TabIndex = 2
        Me.chkSelectAll.Text = "SelectAll"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(117, 292)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(11, 292)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 1
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'Dgv
        '
        Me.Dgv.AllowUserToAddRows = False
        Me.Dgv.AllowUserToDeleteRows = False
        Me.Dgv.BackgroundColor = System.Drawing.Color.Lavender
        Me.Dgv.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Dgv.Location = New System.Drawing.Point(11, 23)
        Me.Dgv.MultiSelect = False
        Me.Dgv.Name = "Dgv"
        Me.Dgv.Size = New System.Drawing.Size(824, 251)
        Me.Dgv.TabIndex = 0
        '
        'lblTotNetwt
        '
        Me.lblTotNetwt.AutoSize = True
        Me.lblTotNetwt.Location = New System.Drawing.Point(370, 311)
        Me.lblTotNetwt.Name = "lblTotNetwt"
        Me.lblTotNetwt.Size = New System.Drawing.Size(54, 13)
        Me.lblTotNetwt.TabIndex = 11
        Me.lblTotNetwt.Text = "TotNwt :"
        Me.lblTotNetwt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblTotNetwt.Visible = False
        '
        'lblTotGrswt
        '
        Me.lblTotGrswt.AutoSize = True
        Me.lblTotGrswt.Location = New System.Drawing.Point(370, 295)
        Me.lblTotGrswt.Name = "lblTotGrswt"
        Me.lblTotGrswt.Size = New System.Drawing.Size(55, 13)
        Me.lblTotGrswt.TabIndex = 10
        Me.lblTotGrswt.Text = "TotGwt :"
        Me.lblTotGrswt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblTotGrswt.Visible = False
        '
        'lblTotPcs
        '
        Me.lblTotPcs.AutoSize = True
        Me.lblTotPcs.Location = New System.Drawing.Point(370, 279)
        Me.lblTotPcs.Name = "lblTotPcs"
        Me.lblTotPcs.Size = New System.Drawing.Size(52, 13)
        Me.lblTotPcs.TabIndex = 9
        Me.lblTotPcs.Text = "TotPcs :"
        Me.lblTotPcs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblTotPcs.Visible = False
        '
        'MultiSelectRowDia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(866, 340)
        Me.ControlBox = False
        Me.Controls.Add(Me.GrpSelection)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "MultiSelectRowDia"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Selection Form"
        Me.GrpSelection.ResumeLayout(False)
        Me.GrpSelection.PerformLayout()
        CType(Me.Dgv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpSelection As CodeVendor.Controls.Grouper
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents Dgv As System.Windows.Forms.DataGridView
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkAppSales As System.Windows.Forms.CheckBox
    Friend WithEvents lblSearchTagNo As System.Windows.Forms.Label
    Friend WithEvents txtSearchTagNo As System.Windows.Forms.TextBox
    Friend WithEvents txtSearchItem As System.Windows.Forms.TextBox
    Friend WithEvents lblTotNetwt As Label
    Friend WithEvents lblTotGrswt As Label
    Friend WithEvents lblTotPcs As Label
End Class
