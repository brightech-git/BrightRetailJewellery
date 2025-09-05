Imports System.Data.OleDb
Public Class frmMIMRUpdate

#Region " Variable"
    Dim strsql As String = ""

#End Region

#Region " User Defined Function"
    Private Sub funNew()
        dtpFrom.Value = GetServerDate()
        txtInvoiceNo.Text = ""
    End Sub
#End Region

#Region "Constructor"
    Public Sub New(ByVal InvoiceDate As Date, ByVal InvoiceNo As String)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        funNew()
        dtpFrom.Value = InvoiceDate
        txtInvoiceNo.Text = InvoiceNo
    End Sub
#End Region

#Region " Form Load"
    Private Sub frmMIMRUpdate_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub frmMIMRUpdate_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{tab}")
        End If
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txtInvoiceNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtInvoiceNo.KeyPress
        If textInvoiceCharacterNotallowed(sender, e) = True Then
            MsgBox("Not Allowed Special Character", MsgBoxStyle.Information)
            e.Handled = True
            Exit Sub
        End If
    End Sub
#End Region
End Class