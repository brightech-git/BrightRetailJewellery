<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGSTGSTTDS
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
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNetAmount_AMT = New System.Windows.Forms.TextBox()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.txtTdsAmt_AMT = New System.Windows.Forms.TextBox()
        Me.txtTdsPer_PER = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtGstAmount = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmbTCS = New System.Windows.Forms.ComboBox()
        Me.txtTCSPer_PER = New System.Windows.Forms.TextBox()
        Me.txtTCSAmt_AMT = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(22, 53)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(97, 19)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Net Amount"
        '
        'txtNetAmount_AMT
        '
        Me.txtNetAmount_AMT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNetAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNetAmount_AMT.Location = New System.Drawing.Point(129, 51)
        Me.txtNetAmount_AMT.Name = "txtNetAmount_AMT"
        Me.txtNetAmount_AMT.ReadOnly = True
        Me.txtNetAmount_AMT.Size = New System.Drawing.Size(112, 22)
        Me.txtNetAmount_AMT.TabIndex = 3
        '
        'Label54
        '
        Me.Label54.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label54.Location = New System.Drawing.Point(22, 31)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(97, 19)
        Me.Label54.TabIndex = 0
        Me.Label54.Text = "TDS %"
        '
        'Label49
        '
        Me.Label49.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(22, 75)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(97, 19)
        Me.Label49.TabIndex = 4
        Me.Label49.Text = "TDS Amount"
        '
        'txtTdsAmt_AMT
        '
        Me.txtTdsAmt_AMT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTdsAmt_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTdsAmt_AMT.Location = New System.Drawing.Point(129, 73)
        Me.txtTdsAmt_AMT.Name = "txtTdsAmt_AMT"
        Me.txtTdsAmt_AMT.ReadOnly = True
        Me.txtTdsAmt_AMT.Size = New System.Drawing.Size(112, 22)
        Me.txtTdsAmt_AMT.TabIndex = 5
        '
        'txtTdsPer_PER
        '
        Me.txtTdsPer_PER.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTdsPer_PER.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTdsPer_PER.Location = New System.Drawing.Point(129, 29)
        Me.txtTdsPer_PER.Name = "txtTdsPer_PER"
        Me.txtTdsPer_PER.Size = New System.Drawing.Size(112, 22)
        Me.txtTdsPer_PER.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtGstAmount)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.Label49)
        Me.GroupBox2.Controls.Add(Me.txtTdsAmt_AMT)
        Me.GroupBox2.Controls.Add(Me.txtTdsPer_PER)
        Me.GroupBox2.Controls.Add(Me.txtNetAmount_AMT)
        Me.GroupBox2.Controls.Add(Me.Label54)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(252, 127)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "TDS"
        '
        'txtGstAmount
        '
        Me.txtGstAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtGstAmount.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGstAmount.Location = New System.Drawing.Point(129, 95)
        Me.txtGstAmount.Name = "txtGstAmount"
        Me.txtGstAmount.ReadOnly = True
        Me.txtGstAmount.Size = New System.Drawing.Size(112, 22)
        Me.txtGstAmount.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(22, 97)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 19)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Gst Amount"
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(373, 3)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(100, 30)
        Me.btnOk.TabIndex = 0
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(475, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.btnOk)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 127)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(635, 36)
        Me.Panel1.TabIndex = 2
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbTCS)
        Me.GroupBox1.Controls.Add(Me.txtTCSPer_PER)
        Me.GroupBox1.Controls.Add(Me.txtTCSAmt_AMT)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(252, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(383, 127)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "TCS"
        '
        'cmbTCS
        '
        Me.cmbTCS.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTCS.FormattingEnabled = True
        Me.cmbTCS.Location = New System.Drawing.Point(100, 27)
        Me.cmbTCS.Name = "cmbTCS"
        Me.cmbTCS.Size = New System.Drawing.Size(280, 22)
        Me.cmbTCS.TabIndex = 1
        '
        'txtTCSPer_PER
        '
        Me.txtTCSPer_PER.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTCSPer_PER.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTCSPer_PER.Location = New System.Drawing.Point(100, 49)
        Me.txtTCSPer_PER.Name = "txtTCSPer_PER"
        Me.txtTCSPer_PER.Size = New System.Drawing.Size(112, 22)
        Me.txtTCSPer_PER.TabIndex = 3
        '
        'txtTCSAmt_AMT
        '
        Me.txtTCSAmt_AMT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTCSAmt_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTCSAmt_AMT.Location = New System.Drawing.Point(100, 71)
        Me.txtTCSAmt_AMT.Name = "txtTCSAmt_AMT"
        Me.txtTCSAmt_AMT.ReadOnly = True
        Me.txtTCSAmt_AMT.Size = New System.Drawing.Size(112, 22)
        Me.txtTCSAmt_AMT.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(6, 73)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(97, 19)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "TCS Amount"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 51)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(97, 19)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "TCS %"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 19)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "TCS"
        '
        'frmGSTGSTTDS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(635, 163)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmGSTGSTTDS"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TDS"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNetAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents txtTdsAmt_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtTdsPer_PER As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtTCSPer_PER As TextBox
    Friend WithEvents txtTCSAmt_AMT As TextBox
    Friend WithEvents cmbTCS As ComboBox
    Friend WithEvents txtGstAmount As TextBox
    Friend WithEvents Label1 As Label
End Class
