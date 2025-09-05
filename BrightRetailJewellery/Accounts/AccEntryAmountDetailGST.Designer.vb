<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AccEntryAmountDetailGST
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
        Me.grpWeightDetail = New CodeVendor.Controls.Grouper()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbWCategory_MAN = New System.Windows.Forms.ComboBox()
        Me.PnlIg = New System.Windows.Forms.Panel()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtIgstPer_PER = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtIgst_AMT = New System.Windows.Forms.TextBox()
        Me.PnlSg = New System.Windows.Forms.Panel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtSgstPer_PER = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtCgstPer_PER = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtSgst_AMT = New System.Windows.Forms.TextBox()
        Me.txtCgst_AMT = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.lblVatAmt = New System.Windows.Forms.Label()
        Me.txtVat_AMT = New System.Windows.Forms.TextBox()
        Me.lblVatPer = New System.Windows.Forms.Label()
        Me.txtVat_per = New System.Windows.Forms.TextBox()
        Me.dtpRefDate = New BrighttechPack.DatePicker(Me.components)
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtRefno = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtWamt_AMT = New System.Windows.Forms.TextBox()
        Me.cmbWType = New System.Windows.Forms.ComboBox()
        Me.btnWCancel = New System.Windows.Forms.Button()
        Me.btnWOk = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.grpWeightDetail.SuspendLayout()
        Me.PnlIg.SuspendLayout()
        Me.PnlSg.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpWeightDetail
        '
        Me.grpWeightDetail.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpWeightDetail.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpWeightDetail.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpWeightDetail.BorderColor = System.Drawing.Color.Transparent
        Me.grpWeightDetail.BorderThickness = 1.0!
        Me.grpWeightDetail.Controls.Add(Me.Label1)
        Me.grpWeightDetail.Controls.Add(Me.cmbWCategory_MAN)
        Me.grpWeightDetail.Controls.Add(Me.PnlIg)
        Me.grpWeightDetail.Controls.Add(Me.PnlSg)
        Me.grpWeightDetail.Controls.Add(Me.lblVatAmt)
        Me.grpWeightDetail.Controls.Add(Me.txtVat_AMT)
        Me.grpWeightDetail.Controls.Add(Me.lblVatPer)
        Me.grpWeightDetail.Controls.Add(Me.txtVat_per)
        Me.grpWeightDetail.Controls.Add(Me.dtpRefDate)
        Me.grpWeightDetail.Controls.Add(Me.Label10)
        Me.grpWeightDetail.Controls.Add(Me.Label9)
        Me.grpWeightDetail.Controls.Add(Me.txtRefno)
        Me.grpWeightDetail.Controls.Add(Me.Label8)
        Me.grpWeightDetail.Controls.Add(Me.txtWamt_AMT)
        Me.grpWeightDetail.Controls.Add(Me.cmbWType)
        Me.grpWeightDetail.Controls.Add(Me.btnWCancel)
        Me.grpWeightDetail.Controls.Add(Me.btnWOk)
        Me.grpWeightDetail.Controls.Add(Me.Label7)
        Me.grpWeightDetail.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpWeightDetail.GroupImage = Nothing
        Me.grpWeightDetail.GroupTitle = ""
        Me.grpWeightDetail.Location = New System.Drawing.Point(4, -5)
        Me.grpWeightDetail.Name = "grpWeightDetail"
        Me.grpWeightDetail.Padding = New System.Windows.Forms.Padding(20)
        Me.grpWeightDetail.PaintGroupBox = False
        Me.grpWeightDetail.RoundCorners = 10
        Me.grpWeightDetail.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpWeightDetail.ShadowControl = False
        Me.grpWeightDetail.ShadowThickness = 3
        Me.grpWeightDetail.Size = New System.Drawing.Size(438, 281)
        Me.grpWeightDetail.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 54)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Category"
        '
        'cmbWCategory_MAN
        '
        Me.cmbWCategory_MAN.FormattingEnabled = True
        Me.cmbWCategory_MAN.Location = New System.Drawing.Point(104, 46)
        Me.cmbWCategory_MAN.Name = "cmbWCategory_MAN"
        Me.cmbWCategory_MAN.Size = New System.Drawing.Size(318, 21)
        Me.cmbWCategory_MAN.TabIndex = 3
        '
        'PnlIg
        '
        Me.PnlIg.Controls.Add(Me.Label17)
        Me.PnlIg.Controls.Add(Me.txtIgstPer_PER)
        Me.PnlIg.Controls.Add(Me.Label18)
        Me.PnlIg.Controls.Add(Me.txtIgst_AMT)
        Me.PnlIg.Location = New System.Drawing.Point(3, 152)
        Me.PnlIg.Name = "PnlIg"
        Me.PnlIg.Size = New System.Drawing.Size(422, 25)
        Me.PnlIg.TabIndex = 7
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(213, 6)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(97, 13)
        Me.Label17.TabIndex = 2
        Me.Label17.Text = "InterState GST "
        '
        'txtIgstPer_PER
        '
        Me.txtIgstPer_PER.Location = New System.Drawing.Point(101, 2)
        Me.txtIgstPer_PER.Name = "txtIgstPer_PER"
        Me.txtIgstPer_PER.Size = New System.Drawing.Size(109, 21)
        Me.txtIgstPer_PER.TabIndex = 1
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(5, 6)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(81, 13)
        Me.Label18.TabIndex = 0
        Me.Label18.Text = "InterState %"
        '
        'txtIgst_AMT
        '
        Me.txtIgst_AMT.Location = New System.Drawing.Point(310, 2)
        Me.txtIgst_AMT.Name = "txtIgst_AMT"
        Me.txtIgst_AMT.Size = New System.Drawing.Size(109, 21)
        Me.txtIgst_AMT.TabIndex = 3
        '
        'PnlSg
        '
        Me.PnlSg.Controls.Add(Me.Label16)
        Me.PnlSg.Controls.Add(Me.txtSgstPer_PER)
        Me.PnlSg.Controls.Add(Me.Label11)
        Me.PnlSg.Controls.Add(Me.txtCgstPer_PER)
        Me.PnlSg.Controls.Add(Me.Label12)
        Me.PnlSg.Controls.Add(Me.txtSgst_AMT)
        Me.PnlSg.Controls.Add(Me.txtCgst_AMT)
        Me.PnlSg.Controls.Add(Me.Label15)
        Me.PnlSg.Location = New System.Drawing.Point(3, 97)
        Me.PnlSg.Name = "PnlSg"
        Me.PnlSg.Size = New System.Drawing.Size(422, 52)
        Me.PnlSg.TabIndex = 6
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(213, 33)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(81, 13)
        Me.Label16.TabIndex = 6
        Me.Label16.Text = "Central GST "
        '
        'txtSgstPer_PER
        '
        Me.txtSgstPer_PER.Location = New System.Drawing.Point(101, 3)
        Me.txtSgstPer_PER.Name = "txtSgstPer_PER"
        Me.txtSgstPer_PER.Size = New System.Drawing.Size(109, 21)
        Me.txtSgstPer_PER.TabIndex = 1
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(5, 7)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(81, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "State GST %"
        '
        'txtCgstPer_PER
        '
        Me.txtCgstPer_PER.Location = New System.Drawing.Point(101, 29)
        Me.txtCgstPer_PER.Name = "txtCgstPer_PER"
        Me.txtCgstPer_PER.Size = New System.Drawing.Size(109, 21)
        Me.txtCgstPer_PER.TabIndex = 5
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(5, 33)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(93, 13)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "Central GST %"
        '
        'txtSgst_AMT
        '
        Me.txtSgst_AMT.Location = New System.Drawing.Point(310, 3)
        Me.txtSgst_AMT.Name = "txtSgst_AMT"
        Me.txtSgst_AMT.Size = New System.Drawing.Size(109, 21)
        Me.txtSgst_AMT.TabIndex = 3
        '
        'txtCgst_AMT
        '
        Me.txtCgst_AMT.Location = New System.Drawing.Point(310, 29)
        Me.txtCgst_AMT.Name = "txtCgst_AMT"
        Me.txtCgst_AMT.Size = New System.Drawing.Size(109, 21)
        Me.txtCgst_AMT.TabIndex = 7
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(213, 7)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(69, 13)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "State GST "
        '
        'lblVatAmt
        '
        Me.lblVatAmt.AutoSize = True
        Me.lblVatAmt.Location = New System.Drawing.Point(213, 213)
        Me.lblVatAmt.Name = "lblVatAmt"
        Me.lblVatAmt.Size = New System.Drawing.Size(79, 13)
        Me.lblVatAmt.TabIndex = 10
        Me.lblVatAmt.Text = "GST Amount"
        Me.lblVatAmt.Visible = False
        '
        'txtVat_AMT
        '
        Me.txtVat_AMT.Location = New System.Drawing.Point(313, 208)
        Me.txtVat_AMT.Name = "txtVat_AMT"
        Me.txtVat_AMT.ReadOnly = True
        Me.txtVat_AMT.Size = New System.Drawing.Size(109, 21)
        Me.txtVat_AMT.TabIndex = 11
        Me.txtVat_AMT.Visible = False
        '
        'lblVatPer
        '
        Me.lblVatPer.AutoSize = True
        Me.lblVatPer.Location = New System.Drawing.Point(8, 212)
        Me.lblVatPer.Name = "lblVatPer"
        Me.lblVatPer.Size = New System.Drawing.Size(47, 13)
        Me.lblVatPer.TabIndex = 8
        Me.lblVatPer.Text = "GST %"
        Me.lblVatPer.Visible = False
        '
        'txtVat_per
        '
        Me.txtVat_per.Location = New System.Drawing.Point(104, 208)
        Me.txtVat_per.Name = "txtVat_per"
        Me.txtVat_per.ReadOnly = True
        Me.txtVat_per.Size = New System.Drawing.Size(109, 21)
        Me.txtVat_per.TabIndex = 9
        Me.txtVat_per.Visible = False
        '
        'dtpRefDate
        '
        Me.dtpRefDate.Location = New System.Drawing.Point(313, 182)
        Me.dtpRefDate.Mask = "##/##/####"
        Me.dtpRefDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpRefDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpRefDate.Name = "dtpRefDate"
        Me.dtpRefDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpRefDate.Size = New System.Drawing.Size(109, 21)
        Me.dtpRefDate.TabIndex = 15
        Me.dtpRefDate.Text = "06/03/9998"
        Me.dtpRefDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(213, 185)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(47, 13)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "Ref Dt."
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(8, 185)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(45, 13)
        Me.Label9.TabIndex = 12
        Me.Label9.Text = "Ref No"
        '
        'txtRefno
        '
        Me.txtRefno.Location = New System.Drawing.Point(104, 182)
        Me.txtRefno.Name = "txtRefno"
        Me.txtRefno.Size = New System.Drawing.Size(109, 21)
        Me.txtRefno.TabIndex = 13
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(8, 77)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(51, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Amount"
        '
        'txtWamt_AMT
        '
        Me.txtWamt_AMT.Location = New System.Drawing.Point(104, 72)
        Me.txtWamt_AMT.Name = "txtWamt_AMT"
        Me.txtWamt_AMT.Size = New System.Drawing.Size(109, 21)
        Me.txtWamt_AMT.TabIndex = 5
        '
        'cmbWType
        '
        Me.cmbWType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbWType.FormattingEnabled = True
        Me.cmbWType.Location = New System.Drawing.Point(104, 20)
        Me.cmbWType.Name = "cmbWType"
        Me.cmbWType.Size = New System.Drawing.Size(191, 21)
        Me.cmbWType.TabIndex = 1
        '
        'btnWCancel
        '
        Me.btnWCancel.Location = New System.Drawing.Point(258, 236)
        Me.btnWCancel.Name = "btnWCancel"
        Me.btnWCancel.Size = New System.Drawing.Size(83, 30)
        Me.btnWCancel.TabIndex = 17
        Me.btnWCancel.Text = "&Cancel"
        Me.btnWCancel.UseVisualStyleBackColor = True
        '
        'btnWOk
        '
        Me.btnWOk.Location = New System.Drawing.Point(173, 236)
        Me.btnWOk.Name = "btnWOk"
        Me.btnWOk.Size = New System.Drawing.Size(83, 30)
        Me.btnWOk.TabIndex = 16
        Me.btnWOk.Text = "Ok"
        Me.btnWOk.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 28)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(34, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Type"
        '
        'AccEntryAmountDetailGST
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(448, 273)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpWeightDetail)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "AccEntryAmountDetailGST"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Weight Detail"
        Me.grpWeightDetail.ResumeLayout(False)
        Me.grpWeightDetail.PerformLayout()
        Me.PnlIg.ResumeLayout(False)
        Me.PnlIg.PerformLayout()
        Me.PnlSg.ResumeLayout(False)
        Me.PnlSg.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpWeightDetail As CodeVendor.Controls.Grouper
    Friend WithEvents btnWCancel As System.Windows.Forms.Button
    Friend WithEvents btnWOk As System.Windows.Forms.Button
    Friend WithEvents cmbWType As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtWamt_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtRefno As System.Windows.Forms.TextBox
    Friend WithEvents dtpRefDate As BrighttechPack.DatePicker
    Friend WithEvents lblVatAmt As System.Windows.Forms.Label
    Friend WithEvents txtVat_AMT As System.Windows.Forms.TextBox
    Friend WithEvents lblVatPer As System.Windows.Forms.Label
    Friend WithEvents txtVat_per As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtCgstPer_PER As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtSgstPer_PER As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtCgst_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtSgst_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txtIgst_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtIgstPer_PER As System.Windows.Forms.TextBox
    Friend WithEvents PnlIg As System.Windows.Forms.Panel
    Friend WithEvents PnlSg As System.Windows.Forms.Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbWCategory_MAN As ComboBox
End Class
