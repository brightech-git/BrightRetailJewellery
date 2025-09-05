Imports System.Data.OleDb
Public Class frmCertificationCharges
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT SLNO,M.METALNAME,FROMWT,TOWT,FROMCENT,TOCENT,PERCARATAMT,FLATAMT "
        strSql += " FROM " & cnAdminDb & "..CERCHARGES AS CR"
        strSql += " LEFT JOIN " & cnAdminDb & "..METALMAST M ON CR.METALID=M.METALID"
        strSql += " ORDER BY FROMCENT,TOCENT"
        funcOpenGrid(strSql, gridView)
        With gridView
            .Columns("SLNO").Visible = False
            .Columns("FROMWT").HeaderText = "FROMWT"
            .Columns("FROMWT").Width = 80
            .Columns("FROMWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOWT").HeaderText = "TOWT"
            .Columns("TOWT").Width = 80
            .Columns("TOWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("FROMCENT").HeaderText = "FROMCENT"
            .Columns("FROMCENT").Width = 80
            .Columns("FROMCENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOCENT").HeaderText = "TOCENT"
            .Columns("TOCENT").Width = 80
            .Columns("TOCENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PERCARATAMT").Width = 100
            .Columns("PERCARATAMT").HeaderText = "PerCarat/PerGm"
            .Columns("PERCARATAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("FLATAMT").Width = 100
            .Columns("FLATAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST "
        strSql += " WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DISPLAYORDER,METALNAME"
        objGPack.FillCombo(strSql, cmbMetalName_Man, True, False)
        funcCallGrid()
        txtMiscId.Text = objGPack.GetMax("SLNO", "CERCHARGES", cnAdminDb)
        cmbMetalName_Man.Focus()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        Dim Metalid As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetalName_Man.Text & "'", "", )
        If funcValidation(Metalid) = True Then Exit Function
        If flagSave = False Then
            If funcCheckUnique(Metalid, txtCentFrom.Text, txtCentTo.Text, txtFromWt_WET.Text, txtToWt_WET.Text) = True Then
                MsgBox("Already Exist", MsgBoxStyle.Information)
                txtCentFrom.Focus()
                Exit Function
            End If
            funcAdd(Metalid)
            Exit Function
        Else
            If funcCheckUnique(Metalid, txtCentFrom.Text, txtCentTo.Text, txtFromWt_WET.Text, txtToWt_WET.Text) = True Then
                MsgBox("Already Exist", MsgBoxStyle.Information)
                txtCentFrom.Focus()
                Exit Function
            End If
            funcUpdate(Metalid)
            Exit Function
        End If
    End Function
    Function funcAdd(ByVal Metalid As String) As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " INSERT INTO " & cnAdminDb & "..CERCHARGES"
        strSql += " ("
        strSql += " SLNO,METALID,FROMWT,TOWT,FROMCENT,TOCENT,PERCARATAMT,FLATAMT,USERID,UPDATED,UPTIME"
        strSql += " )VALUES("
        strSql += " '" & txtMiscId.Text & "'" 'SLNO
        strSql += " ,'" & Metalid & "'" 'SLNO
        strSql += " ," & Val(txtFromWt_WET.Text) & "" 'FROMWT
        strSql += " ," & Val(txtToWt_WET.Text) & "" 'TOWT
        strSql += " ," & Val(txtCentFrom.Text) & "" 'FROMCENT
        strSql += " ," & Val(txtCentTo.Text) & "" 'TOCENT
        strSql += " ," & Val(txtPercarat_Amt.Text) & "" 'PERCARATAMT
        strSql += " ," & Val(txtflatCharge_AMT.Text) & "" 'FLATAMT
        strSql += " ,'" & userId & "'" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " )"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate(ByVal Metalid As String) As Integer
        strSql = " UPDATE " & cnAdminDb & "..CERCHARGES SET"
        strSql += " METALID='" & Metalid & "'"
        strSql += " ,FROMWT=" & Val(txtFromWt_WET.Text) & ""
        strSql += " ,TOWT=" & Val(txtToWt_WET.Text) & ""
        strSql += " ,FROMCENT=" & Val(txtCentFrom.Text) & ""
        strSql += " ,TOCENT=" & Val(txtCentTo.Text) & ""
        strSql += " ,PERCARATAMT=" & Val(txtPercarat_Amt.Text) & ""
        strSql += " ,FLATAMT=" & Val(txtflatCharge_AMT.Text) & ""
        strSql += " ,USERID='" & userId & "'"
        strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        strSql += " WHERE SLNO = '" & txtMiscId.Text & "'"
        Try
            ExecQuery(SyncMode.Transaction, strSql, cn)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal temp As Integer) As Integer
        strSql = " SELECT SLNO,M.METALNAME,FROMWT,TOWT,FROMCENT,TOCENT,PERCARATAMT,FLATAMT"
        strSql += " FROM " & cnAdminDb & "..CERCHARGES AS CR"
        strSql += " LEFT JOIN " & cnAdminDb & "..METALMAST M ON CR.METALID=M.METALID"
        strSql += " WHERE SLNO = '" & temp & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            cmbMetalName_Man.Text = Val(.Item("METALNAME").ToString)
            txtFromWt_WET.Text = Val(.Item("FROMWT").ToString)
            txtToWt_WET.Text = Val(.Item("TOWT").ToString)
            txtCentFrom.Text = .Item("FROMCENT").ToString
            txtCentTo.Text = .Item("TOCENT").ToString
            txtPercarat_Amt.Text = .Item("PERCARATAMT").ToString
            txtflatCharge_AMT.Text = .Item("FLATAMT").ToString
        End With
        flagSave = True
        txtMiscId.Text = temp
    End Function
    Function funcValidation(ByVal Metalid As String) As Boolean
        If Metalid = "D" Then
            If txtCentFrom.Text = "" Then
                MsgBox(E0005, MsgBoxStyle.Information)
                txtCentFrom.Focus()
                Return True
            End If
            If txtCentTo.Text = "" Then
                MsgBox(E0005, MsgBoxStyle.Information)
                txtCentTo.Focus()
                Return True
            End If
            If Not Val(txtCentFrom.Text) <= Val(txtCentTo.Text) Then
                MsgBox(E0005 + vbCrLf + E0006 + txtCentTo.Text, MsgBoxStyle.Information)
                txtCentFrom.Focus()
                Return True
            End If
        Else
            If txtFromWt_WET.Text = "" Then
                MsgBox(E0005, MsgBoxStyle.Information)
                txtFromWt_WET.Focus()
                Return True
            End If
            If txtToWt_WET.Text = "" Then
                MsgBox(E0005, MsgBoxStyle.Information)
                txtToWt_WET.Focus()
                Return True
            End If
            If Not Val(txtFromWt_WET.Text) <= Val(txtToWt_WET.Text) Then
                MsgBox(E0005 + vbCrLf + E0006 + txtToWt_WET.Text, MsgBoxStyle.Information)
                txtFromWt_WET.Focus()
                Return True
            End If
        End If

        If txtPercarat_Amt.Text.Trim = "" And txtflatCharge_AMT.Text.Trim = "" Then
            MsgBox("Enter any one value.", MsgBoxStyle.Information)
            txtPercarat_Amt.Focus()
            Return True
        End If
        Return False
    End Function
    Function funcCheckUnique(ByVal Metalid As String, ByVal frmCent As String, ByVal toCent As String, ByVal frmWt As String, ByVal toWt As String) As Boolean
        Dim str As String = Nothing
        Dim dt As New DataTable
        dt.Clear()
        str = " DECLARE @FROMCENT AS FLOAT,@TOCENT AS FLOAT"
        If Metalid = "D" Then
            str += " SET @FROMCENT = " & Val(frmCent) & ""
            str += " SET @TOCENT = " & Val(toCent) & ""
            str += " SELECT 1 FROM " & cnAdminDb & "..CERCHARGES"
            str += " WHERE"
            str += " METALID='" & Metalid & "' AND ((FROMCENT BETWEEN @FROMCENT AND @TOCENT)OR (TOCENT BETWEEN @FROMCENT AND @TOCENT))"
        Else
            str += " SET @FROMCENT = " & Val(frmWt) & ""
            str += " SET @TOCENT = " & Val(toWt) & ""
            str += " SELECT 1 FROM " & cnAdminDb & "..CERCHARGES"
            str += " WHERE"
            str += " METALID='" & Metalid & "' AND ((FROMWT BETWEEN @FROMCENT AND @TOCENT)OR (TOWT BETWEEN @FROMCENT AND @TOCENT))"
        End If
        If flagSave = True Then
            str += " AND SLNO <> '" & txtMiscId.Text & "'"
        End If
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return True ''Already Exist
        End If
        Return False
    End Function

    Private Sub frmCertificationCharges_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            txtCentFrom.Focus()
        End If
    End Sub

    Private Sub frmCertificationCharges_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmCertificationCharges_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        funcGridStyle(gridView)
        funcNew()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        gridView.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        gridView.Focus()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            txtCentFrom.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value)
                txtCentTo.Select()
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..CERCHARGES WHERE 1<>1"
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SLNO").Value.ToString
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..CERCHARGES WHERE SLNO = '" & delKey & "'")
        funcCallGrid()
        funcNew()
    End Sub

    Private Sub txtCentFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCentFrom.GotFocus
        If flagSave = True Then Exit Sub
        Dim Metalid As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetalName_Man.Text & "'", "", )
        strSql = " SELECT MAX(TOCENT) FROM " & cnAdminDb & "..CERCHARGES WHERE METALID='" & Metalid & "'"
        Dim wt As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        txtCentFrom.Text = Format(wt + 0.0001, FormatNumberStyle(DiaRnd))
    End Sub

    Private Sub txtCentFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCentFrom.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
        Else
            WeightValidation(txtCentFrom, e, DiaRnd)
        End If
    End Sub

    Private Sub txtCentTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCentTo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
        Else
            WeightValidation(txtCentTo, e, DiaRnd)
        End If
    End Sub

    Private Sub txtCentFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCentFrom.LostFocus
        txtCentFrom.Text = IIf(Val(txtCentFrom.Text) <> 0, Format(Val(txtCentFrom.Text), FormatNumberStyle(DiaRnd)), txtCentFrom.Text)
    End Sub

    Private Sub txtCentTo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCentTo.LostFocus
        txtCentTo.Text = IIf(Val(txtCentTo.Text) <> 0, Format(Val(txtCentTo.Text), FormatNumberStyle(DiaRnd)), txtCentTo.Text)
    End Sub

    Private Sub cmbMetalName_Man_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetalName_Man.SelectedIndexChanged
        If cmbMetalName_Man.Text <> "" Then
            If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetalName_Man.Text & "'", "", ) = "D" Then
                pnlDia.Visible = True
                pnlGeneral.Visible = False
                txtFromWt_WET.Text = ""
                txtToWt_WET.Text = ""
                lblPercarat.Text = "Per Carat"
            Else
                pnlDia.Visible = False
                pnlGeneral.Visible = True
                txtCentFrom.Text = ""
                txtCentTo.Text = ""
                lblPercarat.Text = "Per Gram"
            End If
        End If
    End Sub

    Private Sub txtFromWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromWt_WET.GotFocus
        Dim Metalid As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetalName_Man.Text & "'", "", )
        strSql = " SELECT MAX(TOWT) FROM " & cnAdminDb & "..CERCHARGES WHERE METALID='" & Metalid & "'"
        Dim wt As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        txtFromWt_WET.Text = Format(wt + 0.001, FormatNumberStyle(3))
    End Sub

    Private Sub txtFromWt_WET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromWt_WET.LostFocus
        txtFromWt_WET.Text = Format(Val(txtFromWt_WET.Text), FormatNumberStyle(3))
    End Sub

    Private Sub txtToWt_WET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtToWt_WET.LostFocus
        txtToWt_WET.Text = Format(Val(txtToWt_WET.Text), FormatNumberStyle(3))
    End Sub
End Class