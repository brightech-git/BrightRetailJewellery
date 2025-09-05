Imports System.Data.OleDb
Public Class frmDiaStyle
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Function funcNew() As Integer
        objGPack.TextClear(Me)
        funcCallGrid()
        Dim dt As New DataTable
        strSql = " SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE ISNULL(ACTIVE,'')='Y' ORDER BY COLORNAME"
        objGPack.FillCombo(strSql, CmbColor, True)
        strSql = " SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE ISNULL(ACTIVE,'')='Y' ORDER BY CUTNAME"
        objGPack.FillCombo(strSql, CmbCut, True)
        strSql = " SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE ISNULL(ACTIVE,'')='Y' ORDER BY CLARITYNAME"
        objGPack.FillCombo(strSql, CmbClarity, True)
        strSql = " SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE ISNULL(ACTIVE,'')='Y' ORDER BY SHAPENAME"
        objGPack.FillCombo(strSql, cmbShape, True)
        CmbCut.Focus()
    End Function
    Function funcCallGrid() As Integer
        strSql = " SELECT ISNULL(C.CUTNAME,'') AS CUTNAME,ISNULL(CO.COLORNAME,'') AS COLORNAME"
        strSql += " ,ISNULL(CL.CLARITYNAME,'') AS CLARITYNAME,ISNULL(SH.SHAPENAME,'') AS SHAPENAME,UNIQUEID AS UNIQUENAME,ISNULL(UNIQUECODE,'')UNIQUECODE,D.CUTID,D.COLORID,D.CLARITYID,D.SHAPEID FROM " & cnAdminDb & "..DIASTYLE D "
        strSql += " LEFT JOIN " & cnAdminDb & "..STNCUT C ON  D.CUTID=C.CUTID"
        strSql += " LEFT JOIN " & cnAdminDb & "..STNCOLOR CO ON  D.COLORID=CO.COLORID"
        strSql += " LEFT JOIN " & cnAdminDb & "..STNCLARITY CL ON  D.CLARITYID=CL.CLARITYID"
        strSql += " LEFT JOIN " & cnAdminDb & "..STNSHAPE SH ON  D.SHAPEID=SH.SHAPEID"
        strSql += " ORDER BY UNIQUEID"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        gridView.Columns("CUTID").Visible = False
        gridView.Columns("COLORID").Visible = False
        gridView.Columns("CLARITYID").Visible = False
        gridView.Columns("SHAPEID").Visible = False
        gridView.Columns("UNIQUENAME").MinimumWidth = 80
        gridView.Columns("UNIQUECODE").MinimumWidth = 100

    End Function

    Private Sub frmDiaStyle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmDiaStyle_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        funcNew()
    End Sub


    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        Try
            If txtuniqueId.Text.Trim = "" Then
                MsgBox("Unique name cannot empty", MsgBoxStyle.Information)
                txtuniqueId.Focus()
                Exit Sub
            End If
            'If objGPack.DupChecker(txtuniqueId, " SELECT 1 FROM " & cnAdminDb & "..DIASTYLE WHERE UNIQUEID='" & txtuniqueId.Text.Trim & "'") Then
            '    Exit Sub
            'End If

            Dim ColorId As Integer = 0
            Dim CutId As Integer = 0
            Dim ClarityId As Integer = 0
            Dim ShapeId As Integer = 0
            ColorId = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & CmbColor.Text & "'", "COLORID", 0)
            CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & CmbCut.Text & "'", "CUTID", 0)
            ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & CmbClarity.Text & "'", "CLARITYID", 0)
            ShapeId = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & cmbShape.Text & "'", "SHAPEID", 0)
            Dim QRY As String = ""
            QRY = " SELECT 1 FROM " & cnAdminDb & "..DIASTYLE "
            QRY += " WHERE CUTID=" & CutId & " AND COLORID=" & ColorId & " AND CLARITYID=" & ClarityId & " AND SHAPEID=" & ShapeId & ""
            'QRY += " AND UNIQUEID='" & txtuniqueId.Text.Trim & "'"
            If objGPack.DupChecker(txtuniqueId, QRY) Then
                Exit Sub
            End If

            strSql = " INSERT INTO " & cnAdminDb & "..DIASTYLE (CUTID,COLORID,CLARITYID,SHAPEID,UNIQUEID,UNIQUECODE) "
            strSql += " VALUES(" & CutId & "," & ColorId & "," & ClarityId & "," & ShapeId & ",'" & txtuniqueId.Text & "','" & txtUniquecode.Text & "')"
            ExecQuery(SyncMode.Master, strSql, cn)

            funcCallGrid()
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim uniqueid As String = gridView.Rows(gridView.CurrentRow.Index).Cells("UNIQUENAME").Value.ToString
        Dim cutid As String = gridView.Rows(gridView.CurrentRow.Index).Cells("CUTID").Value.ToString
        Dim colorid As String = gridView.Rows(gridView.CurrentRow.Index).Cells("COLORID").Value.ToString
        Dim clarityid As String = gridView.Rows(gridView.CurrentRow.Index).Cells("CLARITYID").Value.ToString
        Dim shapeid As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SHAPEID").Value.ToString
        'Dim chkQry As String = " SELECT TOP 1 UNIQUEID FROM " & cnAdminDb & "..DIASTYLE WHERE UNIQUEID= '" & uniqueid & "'"
        strSql = "DELETE FROM " & cnAdminDb & "..DIASTYLE WHERE UNIQUEID= '" & uniqueid & "' AND ISNULL(COLORID,0)='" & colorid & "' AND ISNULL(CLARITYID,0)='" & clarityid & "' AND ISNULL(CUTID,0) ='" & cutid & "' AND ISNULL(SHAPEID,0)='" & shapeid & "'"
        'DeleteItem(SyncMode.Master, chkQry, strSql)
        tran = Nothing
        tran = cn.BeginTransaction
        ExecQuery(SyncMode.Master, strSql, cn, tran, , , , , True)
        tran.Commit()
        tran = Nothing
        MsgBox("Successfully Deleted..")
        funcCallGrid()
    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub
End Class