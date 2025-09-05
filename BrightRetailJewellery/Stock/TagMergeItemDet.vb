Imports System.Data.OleDb
Public Class TagMergeItemDet
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dtSoftKeyss As DataTable
    Public ItemName As String
    Public SubitemName As String
    Public DesignerName As String
    Public Sub New(ByVal Start As Boolean)
        InitializeComponent()
    End Sub
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y' order by ITEMNAME"
        objGPack.FillCombo(strSql, cmbItem_MAN, , False)
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner_MAN, , False)
    End Sub
    Private Sub TagMergeItemDia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmbItem_MAN.Text = ItemName
        cmbSubItem_Man.Text = SubitemName
        cmbDesigner_MAN.Text = DesignerName
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If cmbItem_MAN.Text <> "" And cmbDesigner_MAN.Text <> "" Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub cmbDesigner_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbDesigner_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            btnOk.Focus()
        End If
    End Sub


    Private Sub cmbItem_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbItem_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbItem_MAN.Text = "" Then Exit Sub
            Dim AA As Integer = Val(cmbItem_MAN.Text)
            If Val(cmbItem_MAN.Text) <> 0 And cmbItem_MAN.Text = AA.ToString() Then
                ''entrered itemid 
                Dim itemName As String = objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & cmbItem_MAN.Text & " AND STOCKTYPE = 'T' AND ACTIVE = 'Y' AND ISNULL(STUDDED,'') <> 'Y'")
                If itemName <> "" Then cmbItem_MAN.Text = itemName
            End If
            cmbSubItem_Man.Items.Clear()
            If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'") = "Y" Then
                strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'")))
                objGPack.FillCombo(strSql, cmbSubItem_Man, False)
                cmbSubItem_Man.Enabled = True
            Else
                cmbSubItem_Man.Text = ""
                cmbSubItem_Man.Enabled = False
            End If
            Me.SelectNextControl(cmbItem_MAN, True, True, True, True)
        End If
    End Sub

    Private Sub cmbSubItem_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubItem_Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            cmbDesigner_MAN.Focus()
        End If
    End Sub
End Class