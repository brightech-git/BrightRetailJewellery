<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFinalDiscSal_Dia
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
        Me.grpFinalDisc = New CodeVendor.Controls.Grouper()
        Me.lblAmtWord = New System.Windows.Forms.Label()
        Me.gbadjOption = New System.Windows.Forms.GroupBox()
        Me.chkAdjDia = New System.Windows.Forms.CheckBox()
        Me.chkAdjStn = New System.Windows.Forms.CheckBox()
        Me.chkAdjmc = New System.Windows.Forms.CheckBox()
        Me.chkAdjw = New System.Windows.Forms.CheckBox()
        Me.txtFinalAmount_AMT = New System.Windows.Forms.TextBox()
        Me.lblFinalAmount = New System.Windows.Forms.Label()
        Me.grpFinalDisc.SuspendLayout()
        Me.gbadjOption.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpFinalDisc
        '
        Me.grpFinalDisc.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpFinalDisc.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpFinalDisc.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpFinalDisc.BorderColor = System.Drawing.Color.Transparent
        Me.grpFinalDisc.BorderThickness = 1.0!
        Me.grpFinalDisc.Controls.Add(Me.lblAmtWord)
        Me.grpFinalDisc.Controls.Add(Me.gbadjOption)
        Me.grpFinalDisc.Controls.Add(Me.txtFinalAmount_AMT)
        Me.grpFinalDisc.Controls.Add(Me.lblFinalAmount)
        Me.grpFinalDisc.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpFinalDisc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpFinalDisc.GroupImage = Nothing
        Me.grpFinalDisc.GroupTitle = ""
        Me.grpFinalDisc.Location = New System.Drawing.Point(0, 0)
        Me.grpFinalDisc.Name = "grpFinalDisc"
        Me.grpFinalDisc.Padding = New System.Windows.Forms.Padding(0, 10, 0, 0)
        Me.grpFinalDisc.PaintGroupBox = False
        Me.grpFinalDisc.RoundCorners = 10
        Me.grpFinalDisc.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpFinalDisc.ShadowControl = False
        Me.grpFinalDisc.ShadowThickness = 3
        Me.grpFinalDisc.Size = New System.Drawing.Size(262, 164)
        Me.grpFinalDisc.TabIndex = 0
        '
        'lblAmtWord
        '
        Me.lblAmtWord.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblAmtWord.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmtWord.ForeColor = System.Drawing.Color.Red
        Me.lblAmtWord.Location = New System.Drawing.Point(0, 101)
        Me.lblAmtWord.Name = "lblAmtWord"
        Me.lblAmtWord.Size = New System.Drawing.Size(262, 63)
        Me.lblAmtWord.TabIndex = 6
        '
        'gbadjOption
        '
        Me.gbadjOption.Controls.Add(Me.chkAdjDia)
        Me.gbadjOption.Controls.Add(Me.chkAdjStn)
        Me.gbadjOption.Controls.Add(Me.chkAdjmc)
        Me.gbadjOption.Controls.Add(Me.chkAdjw)
        Me.gbadjOption.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbadjOption.Location = New System.Drawing.Point(0, 73)
        Me.gbadjOption.Name = "gbadjOption"
        Me.gbadjOption.Size = New System.Drawing.Size(262, 28)
        Me.gbadjOption.TabIndex = 5
        Me.gbadjOption.TabStop = False
        Me.gbadjOption.Visible = False
        '
        'chkAdjDia
        '
        Me.chkAdjDia.AutoSize = True
        Me.chkAdjDia.Location = New System.Drawing.Point(128, 9)
        Me.chkAdjDia.Name = "chkAdjDia"
        Me.chkAdjDia.Size = New System.Drawing.Size(60, 17)
        Me.chkAdjDia.TabIndex = 3
        Me.chkAdjDia.Text = "Diam."
        Me.chkAdjDia.UseVisualStyleBackColor = True
        '
        'chkAdjStn
        '
        Me.chkAdjStn.AutoSize = True
        Me.chkAdjStn.Location = New System.Drawing.Point(188, 9)
        Me.chkAdjStn.Name = "chkAdjStn"
        Me.chkAdjStn.Size = New System.Drawing.Size(59, 17)
        Me.chkAdjStn.TabIndex = 2
        Me.chkAdjStn.Text = "Stone"
        Me.chkAdjStn.UseVisualStyleBackColor = True
        '
        'chkAdjmc
        '
        Me.chkAdjmc.AutoSize = True
        Me.chkAdjmc.Location = New System.Drawing.Point(80, 9)
        Me.chkAdjmc.Name = "chkAdjmc"
        Me.chkAdjmc.Size = New System.Drawing.Size(48, 17)
        Me.chkAdjmc.TabIndex = 1
        Me.chkAdjmc.Text = "M.C"
        Me.chkAdjmc.UseVisualStyleBackColor = True
        '
        'chkAdjw
        '
        Me.chkAdjw.AutoSize = True
        Me.chkAdjw.Location = New System.Drawing.Point(6, 9)
        Me.chkAdjw.Name = "chkAdjw"
        Me.chkAdjw.Size = New System.Drawing.Size(74, 17)
        Me.chkAdjw.TabIndex = 0
        Me.chkAdjw.Text = "Wastage"
        Me.chkAdjw.UseVisualStyleBackColor = True
        '
        'txtFinalAmount_AMT
        '
        Me.txtFinalAmount_AMT.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtFinalAmount_AMT.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFinalAmount_AMT.Location = New System.Drawing.Point(0, 42)
        Me.txtFinalAmount_AMT.Name = "txtFinalAmount_AMT"
        Me.txtFinalAmount_AMT.Size = New System.Drawing.Size(262, 31)
        Me.txtFinalAmount_AMT.TabIndex = 4
        '
        'lblFinalAmount
        '
        Me.lblFinalAmount.BackColor = System.Drawing.Color.Transparent
        Me.lblFinalAmount.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblFinalAmount.Font = New System.Drawing.Font("Verdana", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinalAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFinalAmount.Location = New System.Drawing.Point(0, 10)
        Me.lblFinalAmount.Name = "lblFinalAmount"
        Me.lblFinalAmount.Size = New System.Drawing.Size(262, 32)
        Me.lblFinalAmount.TabIndex = 0
        Me.lblFinalAmount.Text = "FINAL AMOUNT"
        Me.lblFinalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmFinalDiscSal_Dia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(262, 164)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpFinalDisc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmFinalDiscSal_Dia"
        Me.Text = "Final Discount"
        Me.grpFinalDisc.ResumeLayout(False)
        Me.grpFinalDisc.PerformLayout()
        Me.gbadjOption.ResumeLayout(False)
        Me.gbadjOption.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpFinalDisc As CodeVendor.Controls.Grouper
    Friend WithEvents txtFinalAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents lblFinalAmount As System.Windows.Forms.Label
    Friend WithEvents gbadjOption As System.Windows.Forms.GroupBox
    Friend WithEvents chkAdjmc As System.Windows.Forms.CheckBox
    Friend WithEvents chkAdjw As System.Windows.Forms.CheckBox
    Friend WithEvents chkAdjStn As System.Windows.Forms.CheckBox
    Friend WithEvents chkAdjDia As System.Windows.Forms.CheckBox
    Friend WithEvents lblAmtWord As Label
End Class
