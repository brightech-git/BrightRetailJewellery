<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRowDiscSal_Dia
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
        Me.grpFinalDisc = New CodeVendor.Controls.Grouper()
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
        Me.grpFinalDisc.Controls.Add(Me.gbadjOption)
        Me.grpFinalDisc.Controls.Add(Me.txtFinalAmount_AMT)
        Me.grpFinalDisc.Controls.Add(Me.lblFinalAmount)
        Me.grpFinalDisc.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpFinalDisc.GroupImage = Nothing
        Me.grpFinalDisc.GroupTitle = ""
        Me.grpFinalDisc.Location = New System.Drawing.Point(6, -5)
        Me.grpFinalDisc.Name = "grpFinalDisc"
        Me.grpFinalDisc.Padding = New System.Windows.Forms.Padding(20)
        Me.grpFinalDisc.PaintGroupBox = False
        Me.grpFinalDisc.RoundCorners = 10
        Me.grpFinalDisc.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpFinalDisc.ShadowControl = False
        Me.grpFinalDisc.ShadowThickness = 3
        Me.grpFinalDisc.Size = New System.Drawing.Size(241, 102)
        Me.grpFinalDisc.TabIndex = 0
        '
        'gbadjOption
        '
        Me.gbadjOption.Controls.Add(Me.chkAdjDia)
        Me.gbadjOption.Controls.Add(Me.chkAdjStn)
        Me.gbadjOption.Controls.Add(Me.chkAdjmc)
        Me.gbadjOption.Controls.Add(Me.chkAdjw)
        Me.gbadjOption.Location = New System.Drawing.Point(0, 76)
        Me.gbadjOption.Name = "gbadjOption"
        Me.gbadjOption.Size = New System.Drawing.Size(241, 28)
        Me.gbadjOption.TabIndex = 5
        Me.gbadjOption.TabStop = False
        Me.gbadjOption.Visible = False
        '
        'chkAdjDia
        '
        Me.chkAdjDia.AutoSize = True
        Me.chkAdjDia.Location = New System.Drawing.Point(122, 9)
        Me.chkAdjDia.Name = "chkAdjDia"
        Me.chkAdjDia.Size = New System.Drawing.Size(60, 17)
        Me.chkAdjDia.TabIndex = 3
        Me.chkAdjDia.Text = "Diam."
        Me.chkAdjDia.UseVisualStyleBackColor = True
        Me.chkAdjDia.Visible = False
        '
        'chkAdjStn
        '
        Me.chkAdjStn.AutoSize = True
        Me.chkAdjStn.Location = New System.Drawing.Point(182, 9)
        Me.chkAdjStn.Name = "chkAdjStn"
        Me.chkAdjStn.Size = New System.Drawing.Size(59, 17)
        Me.chkAdjStn.TabIndex = 2
        Me.chkAdjStn.Text = "Stone"
        Me.chkAdjStn.UseVisualStyleBackColor = True
        Me.chkAdjStn.Visible = False
        '
        'chkAdjmc
        '
        Me.chkAdjmc.AutoSize = True
        Me.chkAdjmc.Location = New System.Drawing.Point(77, 9)
        Me.chkAdjmc.Name = "chkAdjmc"
        Me.chkAdjmc.Size = New System.Drawing.Size(48, 17)
        Me.chkAdjmc.TabIndex = 1
        Me.chkAdjmc.Text = "M.C"
        Me.chkAdjmc.UseVisualStyleBackColor = True
        Me.chkAdjmc.Visible = False
        '
        'chkAdjw
        '
        Me.chkAdjw.AutoSize = True
        Me.chkAdjw.Location = New System.Drawing.Point(6, 9)
        Me.chkAdjw.Name = "chkAdjw"
        Me.chkAdjw.Size = New System.Drawing.Size(75, 17)
        Me.chkAdjw.TabIndex = 0
        Me.chkAdjw.Text = "Wastage"
        Me.chkAdjw.UseVisualStyleBackColor = True
        Me.chkAdjw.Visible = False
        '
        'txtFinalAmount_AMT
        '
        Me.txtFinalAmount_AMT.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFinalAmount_AMT.Location = New System.Drawing.Point(6, 43)
        Me.txtFinalAmount_AMT.Name = "txtFinalAmount_AMT"
        Me.txtFinalAmount_AMT.Size = New System.Drawing.Size(229, 31)
        Me.txtFinalAmount_AMT.TabIndex = 4
        '
        'lblFinalAmount
        '
        Me.lblFinalAmount.BackColor = System.Drawing.Color.Transparent
        Me.lblFinalAmount.Font = New System.Drawing.Font("Verdana", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinalAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFinalAmount.Location = New System.Drawing.Point(6, 8)
        Me.lblFinalAmount.Name = "lblFinalAmount"
        Me.lblFinalAmount.Size = New System.Drawing.Size(229, 32)
        Me.lblFinalAmount.TabIndex = 0
        Me.lblFinalAmount.Text = "FINAL AMOUNT"
        Me.lblFinalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmRowDiscSal_Dia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(252, 97)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpFinalDisc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmRowDiscSal_Dia"
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
End Class
