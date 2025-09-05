Imports System.Data.OleDb
Public Class frmLotnoUpdation
#Region "Variable"
    Dim strSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim dt As New DataTable
    Dim cmd As OleDbCommand
#End Region

#Region "Form Event"
    Private Sub frmLotnoUpdation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'If txtLotNo.Focused = True Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmLotnoUpdation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'COMBOBOX DESIGNER NAME
        btnNew_Click(Me, New System.EventArgs)
    End Sub
#End Region

#Region "Button Event"
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        txtLotNo.Text = ""
        CmbDesigner.Items.Clear()
        strSql = "SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER ORDER BY DESIGNERID"
        objGPack.FillCombo(strSql, CmbDesigner, False)
        txtLotNo.Focus()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If txtLotNo.Text <> "" Then
            strSql = "UPDATE " & cnAdminDb & "..ITEMLOT SET "
            strSql += " DESIGNERID = (SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..DESIGNER "
            strSql += " WHERE DESIGNERNAME = '" & CmbDesigner.Text & "') "
            strSql += " WHERE LOTNO = '" & txtLotNo.Text & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            MsgBox("Updated Successfully", MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Else
            MsgBox("Lot No Empty", MsgBoxStyle.Information)
            txtLotNo.Focus()
        End If
    End Sub
#End Region
#Region "TextBox Event "
    Private Sub txtbxLotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLotNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            dt = New DataTable
            strSql = "SELECT (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER AS D WHERE D.DESIGNERID = IT.DESIGNERID ) AS DESIGNERNAME"
            strSql += " FROM " & cnAdminDb & "..ITEMLOT AS IT WHERE LOTNO = '" & txtLotNo.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Dim temp As String = dt.Rows(0).Item("DESIGNERNAME").ToString
                If temp <> "" Then CmbDesigner.Text = temp
            Else
                MsgBox("Invalid LotNo", MsgBoxStyle.Information)
                txtLotNo.Focus()
            End If
        End If
    End Sub
#End Region
End Class