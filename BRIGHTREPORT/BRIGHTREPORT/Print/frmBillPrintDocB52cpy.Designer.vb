<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBillPrintDocB52cpy
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
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog()
        Me.PrtDoc = New System.Drawing.Printing.PrintDocument()
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
        Me.PrintDialog2 = New System.Windows.Forms.PrintDialog()
        Me.PrintAdvance = New System.Drawing.Printing.PrintDocument()
        Me.PrtDiaAdvance = New System.Windows.Forms.PrintDialog()
        Me.PrintOrder = New System.Drawing.Printing.PrintDocument()
        Me.PrtDiaOrder = New System.Windows.Forms.PrintDialog()
        Me.PrtDiaSmith = New System.Windows.Forms.PrintDialog()
        Me.PrtSmith = New System.Drawing.Printing.PrintDocument()
        Me.PrtDisSmith2 = New System.Windows.Forms.PrintDialog()
        Me.PrtSmith2 = New System.Drawing.Printing.PrintDocument()
        Me.PrtSmithA4First = New System.Drawing.Printing.PrintDocument()
        Me.PrtDiaA4First = New System.Windows.Forms.PrintDialog()
        Me.PrtSmithA4Second = New System.Drawing.Printing.PrintDocument()
        Me.PrtDiaA4Second = New System.Windows.Forms.PrintDialog()
        Me.PrtSmithA4Third = New System.Drawing.Printing.PrintDocument()
        Me.PrtDiaA4Third = New System.Windows.Forms.PrintDialog()
        Me.PrtSmithA4Fourth = New System.Drawing.Printing.PrintDocument()
        Me.PrtDiaA4Fourth = New System.Windows.Forms.PrintDialog()
        Me.SuspendLayout()
        '
        'PrintDialog1
        '
        Me.PrintDialog1.Document = Me.PrtDoc
        Me.PrintDialog1.UseEXDialog = True
        '
        'PrtDoc
        '
        '
        'PrintDocument1
        '
        '
        'PrintDialog2
        '
        Me.PrintDialog2.Document = Me.PrintDocument1
        Me.PrintDialog2.UseEXDialog = True
        '
        'PrintAdvance
        '
        '
        'PrtDiaAdvance
        '
        Me.PrtDiaAdvance.Document = Me.PrintAdvance
        Me.PrtDiaAdvance.UseEXDialog = True
        '
        'PrintOrder
        '
        '
        'PrtDiaOrder
        '
        Me.PrtDiaOrder.Document = Me.PrintOrder
        Me.PrtDiaOrder.UseEXDialog = True
        '
        'PrtDiaSmith
        '
        Me.PrtDiaSmith.Document = Me.PrtSmith
        Me.PrtDiaSmith.UseEXDialog = True
        '
        'PrtSmith
        '
        '
        'PrtDisSmith2
        '
        Me.PrtDisSmith2.Document = Me.PrtSmith2
        Me.PrtDisSmith2.UseEXDialog = True
        '
        'PrtSmith2
        '
        '
        'PrtSmithA4First
        '
        '
        'PrtDiaA4First
        '
        Me.PrtDiaA4First.Document = Me.PrtSmithA4First
        Me.PrtDiaA4First.UseEXDialog = True
        '
        'PrtSmithA4Second
        '
        '
        'PrtDiaA4Second
        '
        Me.PrtDiaA4Second.Document = Me.PrtSmithA4Second
        Me.PrtDiaA4Second.UseEXDialog = True
        '
        'PrtSmithA4Third
        '
        '
        'PrtDiaA4Third
        '
        Me.PrtDiaA4Third.Document = Me.PrtSmithA4Third
        Me.PrtDiaA4Third.UseEXDialog = True
        '
        'PrtSmithA4Fourth
        '
        '
        'PrtDiaA4Fourth
        '
        Me.PrtDiaA4Fourth.Document = Me.PrtSmithA4Fourth
        Me.PrtDiaA4Fourth.UseEXDialog = True
        '
        'frmBillPrintDoc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(120, 0)
        Me.Name = "frmBillPrintDoc"
        Me.Text = "frmBillPrintDoc"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents PrtDoc As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDocument1 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrintDialog2 As System.Windows.Forms.PrintDialog
    Friend WithEvents PrintAdvance As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrtDiaAdvance As System.Windows.Forms.PrintDialog
    Friend WithEvents PrintOrder As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrtDiaOrder As System.Windows.Forms.PrintDialog
    Friend WithEvents PrtDiaSmith As System.Windows.Forms.PrintDialog
    Friend WithEvents PrtSmith As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrtDisSmith2 As System.Windows.Forms.PrintDialog
    Friend WithEvents PrtSmith2 As System.Drawing.Printing.PrintDocument
    Friend WithEvents PrtSmithA4First As Printing.PrintDocument
    Friend WithEvents PrtDiaA4First As PrintDialog
    Friend WithEvents PrtSmithA4Second As Printing.PrintDocument
    Friend WithEvents PrtDiaA4Second As PrintDialog
    Friend WithEvents PrtSmithA4Third As Printing.PrintDocument
    Friend WithEvents PrtDiaA4Third As PrintDialog
    Friend WithEvents PrtSmithA4Fourth As Printing.PrintDocument
    Friend WithEvents PrtDiaA4Fourth As PrintDialog
End Class
