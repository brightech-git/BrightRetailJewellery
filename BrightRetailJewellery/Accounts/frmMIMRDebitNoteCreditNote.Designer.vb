<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMIMRDebitNoteCreditNote
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.dtpTrandate = New BrighttechPack.DatePicker(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtInvoiceNo = New System.Windows.Forms.TextBox()
        Me.txtNewRateInclusive_RATE = New System.Windows.Forms.TextBox()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.txtInValue = New System.Windows.Forms.TextBox()
        Me.txtInCGST = New System.Windows.Forms.TextBox()
        Me.txtInTDS = New System.Windows.Forms.TextBox()
        Me.txtInNet = New System.Windows.Forms.TextBox()
        Me.txtCuNet = New System.Windows.Forms.TextBox()
        Me.txtCuTDS = New System.Windows.Forms.TextBox()
        Me.txtCuCGST = New System.Windows.Forms.TextBox()
        Me.txtCuValue = New System.Windows.Forms.TextBox()
        Me.lblCNoteOrDNote = New System.Windows.Forms.Label()
        Me.txtCDNet = New System.Windows.Forms.TextBox()
        Me.txtcdTDS = New System.Windows.Forms.TextBox()
        Me.txtCDCGst = New System.Windows.Forms.TextBox()
        Me.txtCDValue = New System.Windows.Forms.TextBox()
        Me.txtCDSGst = New System.Windows.Forms.TextBox()
        Me.txtCuSGST = New System.Windows.Forms.TextBox()
        Me.txtInSGST = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtCDIGst = New System.Windows.Forms.TextBox()
        Me.txtCuIGST = New System.Windows.Forms.TextBox()
        Me.txtInIGST = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.cmbAcname_OWN = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.lblCGSTPer = New System.Windows.Forms.Label()
        Me.lblSGSTPer = New System.Windows.Forms.Label()
        Me.lblIGSTPer = New System.Windows.Forms.Label()
        Me.lblTDSPer = New System.Windows.Forms.Label()
        Me.lblRateFixedAcName = New System.Windows.Forms.Label()
        Me.lblRateFixedAccode = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'dtpTrandate
        '
        Me.dtpTrandate.Location = New System.Drawing.Point(167, 33)
        Me.dtpTrandate.Mask = "##/##/####"
        Me.dtpTrandate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTrandate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTrandate.Name = "dtpTrandate"
        Me.dtpTrandate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTrandate.Size = New System.Drawing.Size(102, 21)
        Me.dtpTrandate.TabIndex = 2
        Me.dtpTrandate.Text = "06/03/9998"
        Me.dtpTrandate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(37, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(98, 21)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Date"
        '
        'txtInvoiceNo
        '
        Me.txtInvoiceNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtInvoiceNo.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInvoiceNo.Location = New System.Drawing.Point(167, 60)
        Me.txtInvoiceNo.MaxLength = 20
        Me.txtInvoiceNo.Name = "txtInvoiceNo"
        Me.txtInvoiceNo.Size = New System.Drawing.Size(228, 26)
        Me.txtInvoiceNo.TabIndex = 4
        '
        'txtNewRateInclusive_RATE
        '
        Me.txtNewRateInclusive_RATE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNewRateInclusive_RATE.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewRateInclusive_RATE.Location = New System.Drawing.Point(167, 141)
        Me.txtNewRateInclusive_RATE.MaxLength = 20
        Me.txtNewRateInclusive_RATE.Name = "txtNewRateInclusive_RATE"
        Me.txtNewRateInclusive_RATE.Size = New System.Drawing.Size(228, 26)
        Me.txtNewRateInclusive_RATE.TabIndex = 8
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(167, 173)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 9
        Me.btnOk.Text = "OK"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(295, 173)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(37, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 21)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "No."
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(37, 146)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(98, 21)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "RATE"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(37, 251)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(127, 21)
        Me.Label4.TabIndex = 21
        Me.Label4.Text = "INVOICE VALUE"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(37, 278)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(124, 21)
        Me.Label5.TabIndex = 28
        Me.Label5.Text = "CURRENT VALUE"
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(166, 231)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(137, 14)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Value"
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(310, 231)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(74, 14)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "CGST"
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(730, 230)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 14)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "TDS"
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(876, 230)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(131, 14)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "NET"
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(167, 358)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(136, 30)
        Me.btnGenerate.TabIndex = 42
        Me.btnGenerate.Text = "Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'txtInValue
        '
        Me.txtInValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtInValue.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInValue.Location = New System.Drawing.Point(167, 248)
        Me.txtInValue.MaxLength = 20
        Me.txtInValue.Name = "txtInValue"
        Me.txtInValue.ReadOnly = True
        Me.txtInValue.Size = New System.Drawing.Size(137, 26)
        Me.txtInValue.TabIndex = 22
        Me.txtInValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtInCGST
        '
        Me.txtInCGST.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtInCGST.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInCGST.Location = New System.Drawing.Point(307, 248)
        Me.txtInCGST.MaxLength = 20
        Me.txtInCGST.Name = "txtInCGST"
        Me.txtInCGST.ReadOnly = True
        Me.txtInCGST.Size = New System.Drawing.Size(137, 26)
        Me.txtInCGST.TabIndex = 23
        Me.txtInCGST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtInTDS
        '
        Me.txtInTDS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtInTDS.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInTDS.Location = New System.Drawing.Point(727, 248)
        Me.txtInTDS.MaxLength = 20
        Me.txtInTDS.Name = "txtInTDS"
        Me.txtInTDS.ReadOnly = True
        Me.txtInTDS.Size = New System.Drawing.Size(137, 26)
        Me.txtInTDS.TabIndex = 26
        Me.txtInTDS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtInNet
        '
        Me.txtInNet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtInNet.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInNet.Location = New System.Drawing.Point(867, 248)
        Me.txtInNet.MaxLength = 20
        Me.txtInNet.Name = "txtInNet"
        Me.txtInNet.ReadOnly = True
        Me.txtInNet.Size = New System.Drawing.Size(137, 26)
        Me.txtInNet.TabIndex = 27
        Me.txtInNet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCuNet
        '
        Me.txtCuNet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCuNet.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCuNet.Location = New System.Drawing.Point(867, 275)
        Me.txtCuNet.MaxLength = 20
        Me.txtCuNet.Name = "txtCuNet"
        Me.txtCuNet.ReadOnly = True
        Me.txtCuNet.Size = New System.Drawing.Size(137, 26)
        Me.txtCuNet.TabIndex = 34
        Me.txtCuNet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCuTDS
        '
        Me.txtCuTDS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCuTDS.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCuTDS.Location = New System.Drawing.Point(727, 275)
        Me.txtCuTDS.MaxLength = 20
        Me.txtCuTDS.Name = "txtCuTDS"
        Me.txtCuTDS.ReadOnly = True
        Me.txtCuTDS.Size = New System.Drawing.Size(137, 26)
        Me.txtCuTDS.TabIndex = 33
        Me.txtCuTDS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCuCGST
        '
        Me.txtCuCGST.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCuCGST.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCuCGST.Location = New System.Drawing.Point(307, 275)
        Me.txtCuCGST.MaxLength = 20
        Me.txtCuCGST.Name = "txtCuCGST"
        Me.txtCuCGST.ReadOnly = True
        Me.txtCuCGST.Size = New System.Drawing.Size(137, 26)
        Me.txtCuCGST.TabIndex = 30
        Me.txtCuCGST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCuValue
        '
        Me.txtCuValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCuValue.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCuValue.Location = New System.Drawing.Point(167, 275)
        Me.txtCuValue.MaxLength = 20
        Me.txtCuValue.Name = "txtCuValue"
        Me.txtCuValue.ReadOnly = True
        Me.txtCuValue.Size = New System.Drawing.Size(137, 26)
        Me.txtCuValue.TabIndex = 29
        Me.txtCuValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblCNoteOrDNote
        '
        Me.lblCNoteOrDNote.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCNoteOrDNote.Location = New System.Drawing.Point(37, 326)
        Me.lblCNoteOrDNote.Name = "lblCNoteOrDNote"
        Me.lblCNoteOrDNote.Size = New System.Drawing.Size(115, 21)
        Me.lblCNoteOrDNote.TabIndex = 35
        Me.lblCNoteOrDNote.Text = "...."
        '
        'txtCDNet
        '
        Me.txtCDNet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCDNet.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCDNet.Location = New System.Drawing.Point(870, 326)
        Me.txtCDNet.MaxLength = 20
        Me.txtCDNet.Name = "txtCDNet"
        Me.txtCDNet.ReadOnly = True
        Me.txtCDNet.Size = New System.Drawing.Size(137, 26)
        Me.txtCDNet.TabIndex = 41
        Me.txtCDNet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtcdTDS
        '
        Me.txtcdTDS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtcdTDS.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtcdTDS.Location = New System.Drawing.Point(727, 326)
        Me.txtcdTDS.MaxLength = 20
        Me.txtcdTDS.Name = "txtcdTDS"
        Me.txtcdTDS.ReadOnly = True
        Me.txtcdTDS.Size = New System.Drawing.Size(143, 26)
        Me.txtcdTDS.TabIndex = 40
        Me.txtcdTDS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCDCGst
        '
        Me.txtCDCGst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCDCGst.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCDCGst.Location = New System.Drawing.Point(307, 326)
        Me.txtCDCGst.MaxLength = 20
        Me.txtCDCGst.Name = "txtCDCGst"
        Me.txtCDCGst.ReadOnly = True
        Me.txtCDCGst.Size = New System.Drawing.Size(137, 26)
        Me.txtCDCGst.TabIndex = 37
        Me.txtCDCGst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCDValue
        '
        Me.txtCDValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCDValue.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCDValue.Location = New System.Drawing.Point(167, 326)
        Me.txtCDValue.MaxLength = 20
        Me.txtCDValue.Name = "txtCDValue"
        Me.txtCDValue.ReadOnly = True
        Me.txtCDValue.Size = New System.Drawing.Size(137, 26)
        Me.txtCDValue.TabIndex = 36
        Me.txtCDValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCDSGst
        '
        Me.txtCDSGst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCDSGst.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCDSGst.Location = New System.Drawing.Point(447, 326)
        Me.txtCDSGst.MaxLength = 20
        Me.txtCDSGst.Name = "txtCDSGst"
        Me.txtCDSGst.ReadOnly = True
        Me.txtCDSGst.Size = New System.Drawing.Size(137, 26)
        Me.txtCDSGst.TabIndex = 38
        Me.txtCDSGst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCuSGST
        '
        Me.txtCuSGST.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCuSGST.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCuSGST.Location = New System.Drawing.Point(447, 275)
        Me.txtCuSGST.MaxLength = 20
        Me.txtCuSGST.Name = "txtCuSGST"
        Me.txtCuSGST.ReadOnly = True
        Me.txtCuSGST.Size = New System.Drawing.Size(137, 26)
        Me.txtCuSGST.TabIndex = 31
        Me.txtCuSGST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtInSGST
        '
        Me.txtInSGST.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtInSGST.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInSGST.Location = New System.Drawing.Point(447, 248)
        Me.txtInSGST.MaxLength = 20
        Me.txtInSGST.Name = "txtInSGST"
        Me.txtInSGST.ReadOnly = True
        Me.txtInSGST.Size = New System.Drawing.Size(137, 26)
        Me.txtInSGST.TabIndex = 24
        Me.txtInSGST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(453, 231)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(55, 14)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "SGST"
        '
        'txtCDIGst
        '
        Me.txtCDIGst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCDIGst.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCDIGst.Location = New System.Drawing.Point(587, 326)
        Me.txtCDIGst.MaxLength = 20
        Me.txtCDIGst.Name = "txtCDIGst"
        Me.txtCDIGst.ReadOnly = True
        Me.txtCDIGst.Size = New System.Drawing.Size(137, 26)
        Me.txtCDIGst.TabIndex = 39
        Me.txtCDIGst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCuIGST
        '
        Me.txtCuIGST.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCuIGST.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCuIGST.Location = New System.Drawing.Point(587, 275)
        Me.txtCuIGST.MaxLength = 20
        Me.txtCuIGST.Name = "txtCuIGST"
        Me.txtCuIGST.ReadOnly = True
        Me.txtCuIGST.Size = New System.Drawing.Size(137, 26)
        Me.txtCuIGST.TabIndex = 32
        Me.txtCuIGST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtInIGST
        '
        Me.txtInIGST.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtInIGST.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInIGST.Location = New System.Drawing.Point(587, 248)
        Me.txtInIGST.MaxLength = 20
        Me.txtInIGST.Name = "txtInIGST"
        Me.txtInIGST.ReadOnly = True
        Me.txtInIGST.Size = New System.Drawing.Size(137, 26)
        Me.txtInIGST.TabIndex = 25
        Me.txtInIGST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(596, 231)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(48, 14)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "IGST"
        '
        'Label12
        '
        Me.Label12.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label12.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(0, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(1019, 21)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "MIMR CREDIT/DEBIT NOTE"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbAcname_OWN
        '
        Me.cmbAcname_OWN.BackColor = System.Drawing.SystemColors.Window
        Me.cmbAcname_OWN.Enabled = False
        Me.cmbAcname_OWN.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAcname_OWN.FormattingEnabled = True
        Me.cmbAcname_OWN.Location = New System.Drawing.Point(167, 92)
        Me.cmbAcname_OWN.Name = "cmbAcname_OWN"
        Me.cmbAcname_OWN.Size = New System.Drawing.Size(557, 26)
        Me.cmbAcname_OWN.TabIndex = 6
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(37, 92)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(98, 21)
        Me.Label13.TabIndex = 5
        Me.Label13.Text = "AcName"
        '
        'lblCGSTPer
        '
        Me.lblCGSTPer.AutoSize = True
        Me.lblCGSTPer.ForeColor = System.Drawing.Color.Red
        Me.lblCGSTPer.Location = New System.Drawing.Point(408, 231)
        Me.lblCGSTPer.Name = "lblCGSTPer"
        Me.lblCGSTPer.Size = New System.Drawing.Size(19, 13)
        Me.lblCGSTPer.TabIndex = 13
        Me.lblCGSTPer.Text = "..."
        '
        'lblSGSTPer
        '
        Me.lblSGSTPer.AutoSize = True
        Me.lblSGSTPer.ForeColor = System.Drawing.Color.Red
        Me.lblSGSTPer.Location = New System.Drawing.Point(545, 232)
        Me.lblSGSTPer.Name = "lblSGSTPer"
        Me.lblSGSTPer.Size = New System.Drawing.Size(19, 13)
        Me.lblSGSTPer.TabIndex = 15
        Me.lblSGSTPer.Text = "..."
        '
        'lblIGSTPer
        '
        Me.lblIGSTPer.AutoSize = True
        Me.lblIGSTPer.ForeColor = System.Drawing.Color.Red
        Me.lblIGSTPer.Location = New System.Drawing.Point(688, 232)
        Me.lblIGSTPer.Name = "lblIGSTPer"
        Me.lblIGSTPer.Size = New System.Drawing.Size(19, 13)
        Me.lblIGSTPer.TabIndex = 17
        Me.lblIGSTPer.Text = "..."
        '
        'lblTDSPer
        '
        Me.lblTDSPer.AutoSize = True
        Me.lblTDSPer.ForeColor = System.Drawing.Color.Red
        Me.lblTDSPer.Location = New System.Drawing.Point(827, 232)
        Me.lblTDSPer.Name = "lblTDSPer"
        Me.lblTDSPer.Size = New System.Drawing.Size(19, 13)
        Me.lblTDSPer.TabIndex = 19
        Me.lblTDSPer.Text = "..."
        '
        'lblRateFixedAcName
        '
        Me.lblRateFixedAcName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRateFixedAcName.Location = New System.Drawing.Point(307, 302)
        Me.lblRateFixedAcName.Name = "lblRateFixedAcName"
        Me.lblRateFixedAcName.Size = New System.Drawing.Size(697, 21)
        Me.lblRateFixedAcName.TabIndex = 43
        Me.lblRateFixedAcName.Text = "...."
        Me.lblRateFixedAcName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblRateFixedAccode
        '
        Me.lblRateFixedAccode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRateFixedAccode.Location = New System.Drawing.Point(166, 302)
        Me.lblRateFixedAccode.Name = "lblRateFixedAccode"
        Me.lblRateFixedAccode.Size = New System.Drawing.Size(138, 21)
        Me.lblRateFixedAccode.TabIndex = 44
        Me.lblRateFixedAccode.Text = "...."
        Me.lblRateFixedAccode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmMIMRDebitNoteCreditNote
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(1019, 399)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblRateFixedAccode)
        Me.Controls.Add(Me.lblRateFixedAcName)
        Me.Controls.Add(Me.lblTDSPer)
        Me.Controls.Add(Me.lblIGSTPer)
        Me.Controls.Add(Me.lblSGSTPer)
        Me.Controls.Add(Me.lblCGSTPer)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.cmbAcname_OWN)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.txtCDIGst)
        Me.Controls.Add(Me.txtCuIGST)
        Me.Controls.Add(Me.txtInIGST)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtCDSGst)
        Me.Controls.Add(Me.txtCuSGST)
        Me.Controls.Add(Me.txtInSGST)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtCDNet)
        Me.Controls.Add(Me.txtcdTDS)
        Me.Controls.Add(Me.txtCDCGst)
        Me.Controls.Add(Me.txtCDValue)
        Me.Controls.Add(Me.lblCNoteOrDNote)
        Me.Controls.Add(Me.txtCuNet)
        Me.Controls.Add(Me.txtCuTDS)
        Me.Controls.Add(Me.txtCuCGST)
        Me.Controls.Add(Me.txtCuValue)
        Me.Controls.Add(Me.txtInNet)
        Me.Controls.Add(Me.txtInTDS)
        Me.Controls.Add(Me.txtInCGST)
        Me.Controls.Add(Me.txtInValue)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.txtNewRateInclusive_RATE)
        Me.Controls.Add(Me.txtInvoiceNo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtpTrandate)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmMIMRDebitNoteCreditNote"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MIMR Debit Note /Credit Not"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dtpTrandate As BrighttechPack.DatePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents txtInvoiceNo As TextBox
    Friend WithEvents txtNewRateInclusive_RATE As TextBox
    Friend WithEvents btnOk As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents btnGenerate As Button
    Friend WithEvents txtInValue As TextBox
    Friend WithEvents txtInCGST As TextBox
    Friend WithEvents txtInTDS As TextBox
    Friend WithEvents txtInNet As TextBox
    Friend WithEvents txtCuNet As TextBox
    Friend WithEvents txtCuTDS As TextBox
    Friend WithEvents txtCuCGST As TextBox
    Friend WithEvents txtCuValue As TextBox
    Friend WithEvents lblCNoteOrDNote As Label
    Friend WithEvents txtCDNet As TextBox
    Friend WithEvents txtcdTDS As TextBox
    Friend WithEvents txtCDCGst As TextBox
    Friend WithEvents txtCDValue As TextBox
    Friend WithEvents txtCDSGst As TextBox
    Friend WithEvents txtCuSGST As TextBox
    Friend WithEvents txtInSGST As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtCDIGst As TextBox
    Friend WithEvents txtCuIGST As TextBox
    Friend WithEvents txtInIGST As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents cmbAcname_OWN As ComboBox
    Friend WithEvents Label13 As Label
    Friend WithEvents lblCGSTPer As Label
    Friend WithEvents lblSGSTPer As Label
    Friend WithEvents lblIGSTPer As Label
    Friend WithEvents lblTDSPer As Label
    Friend WithEvents lblRateFixedAcName As Label
    Friend WithEvents lblRateFixedAccode As Label
End Class
