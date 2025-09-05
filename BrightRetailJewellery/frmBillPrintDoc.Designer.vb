<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBillPrintDoc
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBillPrintDoc))
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog
        Me.PrtDoc = New System.Drawing.Printing.PrintDocument
        Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument
        Me.PrintDialog2 = New System.Windows.Forms.PrintDialog
        Me.PrintAdvance = New System.Drawing.Printing.PrintDocument
        Me.PrtDiaAdvance = New System.Windows.Forms.PrintDialog
        Me.PrintOrder = New System.Drawing.Printing.PrintDocument
        Me.PrtDiaOrder = New System.Windows.Forms.PrintDialog
        Me.PrtDiaSmith = New System.Windows.Forms.PrintDialog
        Me.PrtSmith = New System.Drawing.Printing.PrintDocument
        Me.CrystalReportViewer1 = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.PrintDocument2 = New System.Drawing.Printing.PrintDocument
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
        'CrystalReportViewer1
        '
        Me.CrystalReportViewer1.ActiveViewIndex = -1
        Me.CrystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CrystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CrystalReportViewer1.Location = New System.Drawing.Point(0, 0)
        Me.CrystalReportViewer1.Name = "CrystalReportViewer1"
        Me.CrystalReportViewer1.SelectionFormula = ""
        Me.CrystalReportViewer1.Size = New System.Drawing.Size(315, 85)
        Me.CrystalReportViewer1.TabIndex = 0
        Me.CrystalReportViewer1.ViewTimeSelectionFormula = ""
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "NotifyIcon1"
        Me.NotifyIcon1.Visible = True
        '
        'frmBillPrintDoc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(315, 85)
        Me.Controls.Add(Me.CrystalReportViewer1)
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
    Friend WithEvents CrystalReportViewer1 As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents PrintDocument2 As System.Drawing.Printing.PrintDocument
End Class
