Imports System.Data.OleDb
Public Class frmCategoryUpdate
    Dim strsql As String
    Dim catdt As New DataTable
    Dim da As OleDbDataAdapter
    Private Sub frmCategoryUpdate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        comboload()
        grdload()
    End Sub
    Private Sub comboload()
        cmbcategory.Items.Clear()
        strsql = "SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY"
        objGPack.FillCombo(strsql, cmbcategory)
        cmbcategory.DisplayMember = "CATNAME"
    End Sub
    Private Sub funupdate()
        strsql = " UPDATE " & cnAdminDb & "..CATEGORY "
        strsql += " SET "
        strsql += " ITEMID= (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName.Text & "')"
        strsql += " ,SUBITEMID =(SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSubName.Text & "')"
        ' strsql += " and ITEMID= (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName.Text & "')) "
        strsql += " WHERE CATNAME='" & cmbcategory.Text & "'"
        ExecQuery(SyncMode.Master, strsql, cn)
    End Sub
    Private Sub grdload()
        strsql = "SELECT C.CATNAME,I.ITEMNAME,S.SUBITEMNAME "
        strsql += " FROM  " & cnAdminDb & "..category AS C INNER JOIN  " & cnAdminDb & "..ITEMMAST AS I"
        strsql += " ON C.ITEMID=I.ITEMID LEFT OUTER  JOIN  " & cnAdminDb & "..SUBITEMMAST AS S ON S.SUBITEMID=C.SUBITEMID "
        da = New OleDbDataAdapter(strsql, cn)
        catdt.Clear()
        da.Fill(catdt)
        grdCategoryUpdate.DataSource = Nothing
        grdCategoryUpdate.DataSource = catdt
        grdalign()
    End Sub
    Private Sub frmCategoryUpdate_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf (e.KeyCode = Keys.F12) Then
           btnExit_Click_1(Me, New EventArgs)
        ElseIf (e.KeyCode = Keys.F1) Then
            btnUpdate_Click_1(Me, New EventArgs)
        End If
    End Sub
    Private Sub grdalign()
        With grdCategoryUpdate
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("CATNAME").Width = 250
            .Columns("ITEMNAME").Width = 250
            .Columns("SUBITEMNAME").Width = 240
            .ColumnHeadersDefaultCellStyle.Font = New Font("Verdana", 9, FontStyle.Bold)
        End With
    End Sub
    Private Sub cmbcategory_SelectionChangeCommitted_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbcategory.SelectionChangeCommitted
        strsql = "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        ' strsql += " WHERE CATCODE= (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & cmbcategory.Text & "')"
        objGPack.FillCombo(strsql, cmbItemName)
    End Sub
    Private Sub cmbItemName_SelectionChangeCommitted_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItemName.SelectionChangeCommitted
        strsql = "SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST  "
        '  strsql += " WHERE ITEMID= (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName.Text & "')"
        objGPack.FillCombo(strsql, cmbSubName)
    End Sub
    Private Sub grdCategoryUpdate_KeyDown_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles grdCategoryUpdate.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            Dim i As Integer
            i = grdCategoryUpdate.CurrentRow.Index
            cmbcategory.Text = grdCategoryUpdate.Item("CATNAME", i).Value
            cmbItemName.Text = Convert.ToString(grdCategoryUpdate.Item("ITEMNAME", i).Value)
            cmbSubName.Text = Convert.ToString(grdCategoryUpdate.Item("SUBITEMNAME", i).Value)
            cmbcategory.Focus()
        End If
    End Sub
    Private Sub btnUpdate_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        funupdate()
        grdload()
    End Sub
    Private Sub btnExit_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Hide()
    End Sub
End Class
