<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RfIdReader
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RfIdReader))
        Me.TagReader = New AxKITagReader.AxTagReader
        CType(Me.TagReader, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TagReader
        '
        Me.TagReader.Enabled = True
        Me.TagReader.Location = New System.Drawing.Point(130, 117)
        Me.TagReader.Name = "TagReader"
        Me.TagReader.OcxState = CType(resources.GetObject("TagReader.OcxState"), System.Windows.Forms.AxHost.State)
        Me.TagReader.Size = New System.Drawing.Size(33, 33)
        Me.TagReader.TabIndex = 65
        '
        'RfIdReader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me.TagReader)
        Me.Name = "RfIdReader"
        Me.Text = "RfIdReader"
        CType(Me.TagReader, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TagReader As AxKITagReader.AxTagReader
End Class
