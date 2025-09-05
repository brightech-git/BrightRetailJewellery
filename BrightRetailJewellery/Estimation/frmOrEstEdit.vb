Imports System.Data.OleDb
Public Class frmOrEstEdit
    Dim objEstMargin As New EstMargin
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Public EscapePress As Boolean = False
    Public BillDate As Date
    Public Billcostid As String
    Public EstBatchno As String
    Public objSoftKey As New SoftKeys
    Public estSaNo As Integer
    Public estPuNo As Integer
    Public estGenNo As Integer
    Public eststated As String
    Public Rmarginid As String
    Public RowEstMargin As DataRow = Nothing
    Public ESTMARGINMULTI As String = Nothing



    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub

    Private Sub frmEstEdit_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            DialogResult = Windows.Forms.DialogResult.Cancel
            EscapePress = True
            Me.Close()
        End If
    End Sub

    Private Sub frmestedit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            Call txtEstimate_Num_KeyPress(sender, e)
            If txtEstimate_Num.Text <> "" Then
                '    If eststated = "E" Then GlobalShowEstMargin()
                DialogResult = Windows.Forms.DialogResult.OK
                EscapePress = False
                Me.Close()
            End If
        End If
    End Sub

  

    Private Sub txtEstimate_Num_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEstimate_Num.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtEstimate_Num.Text <> "" Then
                Dim strsql As String
                EstBatchno = ""
                strsql = "SELECT top 1 BATCHNO FROM " & cnStockDb & "..ESTORMAST AS E"
                strsql += " WHERE ORNO = " & Val(txtEstimate_Num.Text) & ""
                strsql += "  AND ISNULL(ODBATCHNO,'') = ''"
                If objSoftKey.HasEstPost Then strsql += " AND ORDATE = '" & BillDate.Date & "'"
                Dim dtEstbatch As New DataTable
                da = New OleDbDataAdapter(strsql, cn)
                da.Fill(dtEstbatch)
                If dtEstbatch.Rows.Count > 0 Then EstBatchno = dtEstbatch.Rows(0).Item("BATCHNO") : estSaNo = Val(txtEstimate_Num.Text)
                If EstBatchno <> "" Then Exit Sub
                MsgBox("Estimate No. Not found", MsgBoxStyle.Critical)
                txtEstimate_Num.Text = ""
                txtEstimate_Num.Focus()
                Exit Sub
            End If
        End If
    End Sub


    'Private Sub optDupl_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If optDupl.Checked = True Then eststated = "D"
    'End Sub



    'Private Sub optRevise_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If optRevise.Checked = True Then eststated = "E"
    'End Sub





End Class