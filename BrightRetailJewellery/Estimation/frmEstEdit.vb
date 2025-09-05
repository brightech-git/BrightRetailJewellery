Imports System.Data.OleDb
Public Class frmEstEdit
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
                If eststated = "E" Then GlobalShowEstMargin()
                DialogResult = Windows.Forms.DialogResult.OK
                EscapePress = False
                Me.Close()
            End If
        End If
    End Sub

    Private Sub GlobalShowEstMargin()
reget:
        'If ESTMARGINMULTI = "N" Then Exit Sub
        objEstMargin.BackColor = Me.BackColor
        objEstMargin.StartPosition = FormStartPosition.CenterScreen
        objEstMargin.MaximizeBox = False
        'objEstMargin.grpEstMargin.BackgroundColor = grpHeader.BackgroundColor
        strSql = " SELECT DISTINCT MARGINNAME"
        strSql += " FROM " & cnAdminDb & "..VAMARGIN "
        strSql += " WHERE ISNULL(DISPLAY,'') = 'Y'"
        strSql += " and (isnull(COSTID,'') = '' OR COSTID = '" & BillCostId & "')"
        If Rmarginid <> "" Then strSql += " AND MARGINID IN (" & Rmarginid & ")"
        objGPack.FillCombo(strSql, objEstMargin.cmbVaMargin_OWN)
        'objEstMargin.cmbVaMargin_OWN.Text = txtDetMargin.Text
        If objEstMargin.cmbVaMargin_OWN.Text = "" Then Exit Sub
        If objEstMargin.ShowDialog() = Windows.Forms.DialogResult.OK Then
            strSql = " SELECT * FROM " & cnAdminDb & "..VAMARGIN "
            strSql += " where MARGINNAME = '" & objEstMargin.cmbVaMargin_OWN.Text & "'"
            Dim dtEstMarg As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtEstMarg)
            If dtEstMarg.Rows.Count > 0 Then
                'GlbEstmargin = objEstMargin.cmbVaMargin_OWN.Text
                RowEstMargin = dtEstMarg.Rows(0)
            Else
                MsgBox("VA Margin Is Invalid")
                GoTo reget
            End If
        Else
            GoTo reget
        End If
    End Sub

    Private Sub txtEstimate_Num_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEstimate_Num.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtEstimate_Num.Text <> "" Then
                Dim strsql As String
                EstBatchno = ""
                strsql = "SELECT top 1 ESTBATCHNO FROM " & cnStockDb & "..ESTISSUE AS E"
                strsql += " WHERE TRANNO = " & Val(txtEstimate_Num.Text) & ""
                strsql += vbCrLf + " AND TRANTYPE = 'SA' "
                If objSoftKey.DUPLESTASALE.ToString = "N" Then
                    strsql += vbCrLf + " AND ISNULL(BATCHNO,'') = ''"
                End If
                If objSoftKey.HasEstPost Then strsql += " AND TRANDATE = '" & BillDate.Date & "'"
                Dim dtEstbatch As New DataTable
                da = New OleDbDataAdapter(strsql, cn)
                da.Fill(dtEstbatch)
                If dtEstbatch.Rows.Count > 0 Then EstBatchno = dtEstbatch.Rows(0).Item("ESTBATCHNO") : estSaNo = Val(txtEstimate_Num.Text)
                strsql = "SELECT top 1 ESTBATCHNO FROM " & cnStockDb & "..ESTRECEIPT AS E"
                strsql += " WHERE TRANNO = " & Val(txtEstimate_Num.Text) & ""
                strsql += " AND TRANTYPE = 'PU' "
                If objSoftKey.DUPLESTASALE.ToString = "N" Then
                    strsql += vbCrLf + " And ISNULL(BATCHNO,'') = ''"
                End If
                If EstBatchno <> "" Then strsql += " AND ESTBATCHNO='" & EstBatchno & "'"
                If objSoftKey.HasEstPost Then strsql += " AND TRANDATE = '" & BillDate.Date & "'"
                Dim dtPEstbatch As New DataTable
                da = New OleDbDataAdapter(strsql, cn)
                da.Fill(dtPEstbatch)
                If dtPEstbatch.Rows.Count > 0 Then EstBatchno = dtPEstbatch.Rows(0).Item("ESTBATCHNO") : estPuNo = Val(txtEstimate_Num.Text)
                strsql = "SELECT top 1 BATCHNO FROM " & cnStockDb & "..ESTOUTSTANDING AS E"
                strsql += " WHERE TRANNO = " & Val(txtEstimate_Num.Text) & ""
                strsql += " AND TRANTYPE = 'A' AND ISNULL(SETTLEBATCHNO,'') = ''"
                If objSoftKey.HasEstPost Then strsql += " AND TRANDATE = '" & BillDate.Date & "'"
                Dim dtGEstbatch As New DataTable
                da = New OleDbDataAdapter(strsql, cn)
                da.Fill(dtGEstbatch)
                If dtGEstbatch.Rows.Count > 0 Then EstBatchno = dtGEstbatch.Rows(0).Item("BATCHNO") : estGenNo = Val(txtEstimate_Num.Text)
                If EstBatchno <> "" Then Exit Sub
                MsgBox("Estimate No. Not found", MsgBoxStyle.Critical)
                txtEstimate_Num.Text = ""
                txtEstimate_Num.Focus()
                Exit Sub
            End If
        End If
    End Sub


    Private Sub optDupl_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optDupl.CheckedChanged
        If optDupl.Checked = True Then eststated = "D"
    End Sub



    Private Sub optRevise_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optRevise.CheckedChanged
        If optRevise.Checked = True Then eststated = "E"
    End Sub





End Class