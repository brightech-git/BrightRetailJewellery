Imports System.Data.OleDb
Public Class frmSchemeNoOfferMast
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim flagUpdateId As String = Nothing

    Private Sub frmSchemeNoOfferMast_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub LoadGrid()
        strSql = " SELECT "
        strSql += " (SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='C' AND  CARDCODE = S.SCHEMEID) SCHEMENAME "
        strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.ITEMID) ITEMNAME "
        strSql += " ,CASE WHEN ACTIVE='Y' THEN 'YES' ELSE 'NO' END ACTIVE,SNO "
        strSql += " FROM " & cnAdminDb & "..SCHEMENOOFFER S ORDER BY SNO"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtGrid As New DataTable
        da.Fill(dtGrid)
        gridView.DataSource = dtGrid
        gridView.Columns("SNO").Visible = False
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If cmbScheme.Text = "" Or cmbScheme.Text = "ALL" Then MsgBox("Scheme Name should Not Be Empty", MsgBoxStyle.Information) : cmbScheme.Focus() : Exit Sub
        If cmbItemName_Man.Text = "" Or cmbItemName_Man.Text = "ALL" Then MsgBox("Item Name should Not Be Empty", MsgBoxStyle.Information) : cmbItemName_Man.Focus() : Exit Sub
        Dim schemeid As String = ""
        schemeid = Val(GetSqlValue(cn, "SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='C' AND NAME = '" & cmbScheme.Text & "'").ToString)
        Dim itemid As String = ""
        itemid = Val(GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'").ToString)
        If flagUpdateId <> Nothing Then
            strSql = "SELECT 'CHECK' FROM " & cnAdminDb & "..SCHEMENOOFFER WHERE SCHEMEID = '" & schemeid & "' AND ITEMID = '" & itemid & "' AND SNO <>'" & flagUpdateId.ToString & "'"
        Else
            strSql = "SELECT 'CHECK' FROM " & cnAdminDb & "..SCHEMENOOFFER WHERE SCHEMEID = '" & schemeid & "' AND ITEMID = '" & itemid & "'"
        End If
        If objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox("Already exist", MsgBoxStyle.Information)
            cmbScheme.Focus()
            Exit Sub
        End If
        If flagUpdateId <> Nothing Then 'UPDATE
            strSql = "UPDATE " & cnAdminDb & "..SCHEMENOOFFER"
            strSql += " SET SCHEMEID = '" & schemeid & "'"
            strSql += " , ITEMID = '" & itemid & "'"
            strSql += " , ACTIVE='" & Mid(CmbActive.Text, 1, 1) & "'"
            strSql += " , USERID = '" & userId & "'"
            strSql += " , UPDATED = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " , UPTIME = '" & Date.Now.ToLongTimeString & "'"
            strSql += " WHERE SNO = '" & flagUpdateId & "'"
            ExecQuery(SyncMode.Master, strSql, cn)
            MsgBox("Successfully Updated")
            btnNew_Click(Me, New EventArgs)
            Exit Sub
        End If

        strSql = " INSERT INTO " & cnAdminDb & "..SCHEMENOOFFER"
        strSql += " (SCHEMEID,ITEMID,ACTIVE,USERID,UPDATED,UPTIME)VALUES"
        strSql += " ('" & schemeid & "','" & itemid & "','" & Mid(CmbActive.Text, 1, 1) & "','" & userId & "'"
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "'"
        strSql += " )"
        ExecQuery(SyncMode.Master, strSql, cn)
        MsgBox("Successfully Inserted")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        cmbScheme.Text = ""
        cmbItemName_Man.Text = ""
        CmbActive.Text = "YES"
        flagUpdateId = Nothing
        LoadGrid()
        cmbScheme.Focus()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmSchemeNoOfferMast_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='C' ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbScheme, False, False)

        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE  ACTIVE = 'Y' ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, cmbItemName_Man, True, False)

        CmbActive.Items.Add("YES")
        CmbActive.Items.Add("NO")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then e.Handled = True
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(0)
                cmbScheme.Text = gridView.CurrentRow.Cells("SCHEMENAME").Value.ToString
                cmbItemName_Man.Text = gridView.CurrentRow.Cells("ITEMNAME").Value.ToString
                CmbActive.Text = gridView.CurrentRow.Cells("ACTIVE").Value.ToString
                flagUpdateId = gridView.CurrentRow.Cells("SNO").Value.ToString
                cmbScheme.Focus()
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class