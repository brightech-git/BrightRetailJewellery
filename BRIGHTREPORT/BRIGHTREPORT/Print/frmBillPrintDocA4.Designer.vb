<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBillPrintDocA4
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
        Me.PrtDoc = New System.Drawing.Printing.PrintDocument()
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.PrtSEst = New System.Drawing.Printing.PrintDocument()
        Me.PrintDialog2 = New System.Windows.Forms.PrintDialog()
        Me.printAdvance = New System.Drawing.Printing.PrintDocument()
        Me.PrtDiaAdvance = New System.Windows.Forms.PrintDialog()
        Me.SuspendLayout()
        '
        'PrtDoc
        '
        '
        'PrintDialog1
        '
        Me.PrintDialog1.Document = Me.PrtDoc
        Me.PrintDialog1.UseEXDialog = True
        '
        'PrtSEst
        '
        '
        'PrintDialog2
        '
        Me.PrintDialog2.Document = Me.PrtSEst
        Me.PrintDialog2.UseEXDialog = True
        '
        'printAdvance
        '
        '
        'PrtDiaAdvance
        '
        Me.PrtDiaAdvance.Document = Me.printAdvance
        Me.PrtDiaAdvance.UseEXDialog = True
        '
        'frmBillPrintDocA4
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(120, 0)
        Me.Name = "frmBillPrintDocA4"
        Me.Text = "frmBillPrintDocA4"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PrtDoc As Printing.PrintDocument
    Friend WithEvents PrintDialog1 As PrintDialog
    Friend WithEvents PrtSEst As Printing.PrintDocument
    Friend WithEvents PrintDialog2 As PrintDialog
    Friend WithEvents printAdvance As Printing.PrintDocument
    Friend WithEvents PrtDiaAdvance As PrintDialog
End Class
