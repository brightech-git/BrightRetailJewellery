Imports System.Data.OleDb
Public Class frmRapaport
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim Sno As Integer
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT SLNO,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = CR.ITEMID)AS ITEMNAME,"
        strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.SUBITEMID = CR.SUBITEMID),'')AS SUBITEMNAME,"
        strSql += " ISNULL((SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT AS S WHERE S.CUTID = CR.CUTID),'')AS CUTNAME,"
        strSql += " ISNULL((SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY AS S WHERE S.CLARITYID = CR.CLARITYID),'')AS CLARITYNAME,"
        strSql += " ISNULL((SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE AS SH WHERE SH.SHAPEID = CR.SHAPEID),'')AS SHAPENAME,"
        strSql += " FROMCENT,TOCENT,USRATE,INDRATE FROM " & cnAdminDb & "..RAPAPORT AS CR"
        strSql += " ORDER BY "
        strSql += " ITEMNAME,SUBITEMNAME,FROMCENT,TOCENT"
        funcOpenGrid(strSql, gridView)
        With gridView
            .Columns("SLNO").Visible = False
            .Columns("ITEMNAME").Width = 150
            .Columns("SUBITEMNAME").Width = 150
            .Columns("FROMCENT").HeaderText = "FROMCENT"
            .Columns("FROMCENT").Width = 70
            .Columns("FROMCENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOCENT").HeaderText = "TOCENT"
            .Columns("TOCENT").Width = 70
            .Columns("TOCENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("USRATE").Width = 70
            .Columns("USRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("INDRATE").Width = 70
            .Columns("INDRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("INDRATE").Visible = False
        End With
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        funcCallGrid()
        If gridView.Rows.Count > 0 Then
            btnUpdate.Enabled = True
        Else
            btnUpdate.Enabled = False
        End If
        funcLoadItemName()
        cmbItemName_Man.Select()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If funcValidation() = True Then Exit Function
        If flagSave = False Then
            If funcCheckUnique(txtCentFrom.Text, txtCentTo.Text) = True Then
                MsgBox("Already Exist", MsgBoxStyle.Information)
                cmbItemName_Man.Focus()
                Exit Function
            End If
            funcAdd()
            Exit Function
        Else
            If funcCheckUnique(txtCentFrom.Text, txtCentTo.Text) = True Then
                MsgBox("Already Exist", MsgBoxStyle.Information)
                txtCentFrom.Focus()
                Exit Function
            End If
            funcUpdate()
            Exit Function
        End If
    End Function
    Function funcAdd() As Integer
        Try
            Dim ds As New Data.DataSet
            ds.Clear()
            Dim ItemId As Integer = Nothing
            Dim SubItemId As Integer = Nothing
            Dim CutId As Integer = 0
            Dim ClarityId As Integer = 0
            Dim ShapeId As Integer = 0
            CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & CmbCut.Text & "'", "CUTID", 0)
            ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & CmbClarity.Text & "'", "CLARITYID", 0)
            ShapeId = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & cmbShape.Text & "'", "SHAPEID", 0)
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE"
            strSql += " ITEMNAME = '" & cmbItemName_Man.Text & "'"
            ItemId = Val(objGPack.GetSqlValue(strSql, , , tran))

            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE"
            strSql += " SUBITEMNAME = '" & cmbSubItemName_Man.Text & "'"
            strSql += " AND ITEMID = " & ItemId & ""
            SubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))


            strSql = " INSERT INTO " & cnAdminDb & "..RAPAPORT"
            strSql += " ("
            strSql += " ITEMID,SUBITEMID,FROMCENT,TOCENT,USRATE,INDRATE,UPDATED,UPTIME"
            strSql += " ,CUTID,CLARITYID,SHAPEID"
            strSql += " )VALUES("
            strSql += " " & ItemId & "" 'ItemId
            strSql += " ," & SubItemId & "" 'SubItemId
            strSql += " ," & Val(txtCentFrom.Text) & "" 'FromCent
            strSql += " ," & Val(txtCentTo.Text) & "" 'ToCent
            strSql += " ," & Val(txtUSRate_Amt.Text) & "" 'USrate
            strSql += " ," & Val(txtINDRate_Amt.Text) & "" 'INDRate
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
            strSql += " ," & CutId & ""
            strSql += " ," & ClarityId & ""
            strSql += " ," & ShapeId & ""
            strSql += " )"

            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate() As Integer

        Try
            Dim ItemId As Integer = Nothing
            Dim SubItemId As Integer = Nothing

            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE"
            strSql += " ITEMNAME = '" & cmbItemName_Man.Text & "'"
            ItemId = Val(objGPack.GetSqlValue(strSql))

            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE"
            strSql += " SUBITEMNAME = '" & cmbSubItemName_Man.Text & "'"
            strSql += " AND ITEMID = " & ItemId & ""
            SubItemId = Val(objGPack.GetSqlValue(strSql))

            Dim CutId As Integer = 0
            Dim ClarityId As Integer = 0
            Dim ShapeId As Integer = 0
            CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & CmbCut.Text & "'", "CUTID", 0)
            ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & CmbClarity.Text & "'", "CLARITYID", 0)
            ShapeId = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & cmbShape.Text & "'", "SHAPEID", 0)


            strSql = " UPDATE " & cnAdminDb & "..RAPAPORT SET"
            strSql += " ITEMID=" & ItemId & ""
            strSql += " ,SUBITEMID=" & SubItemId & ""
            strSql += " ,FROMCENT=" & Val(txtCentFrom.Text) & ""
            strSql += " ,TOCENT=" & Val(txtCentTo.Text) & ""
            strSql += " ,USRATE=" & Val(txtUSRate_Amt.Text) & ""
            strSql += " ,INDRATE=" & Val(txtINDRate_Amt.Text) & ""
            strSql += " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,UPTIME='" & Date.Now.ToLongTimeString & "'"
            strSql += " ,CUTID = '" & CutId & "'" 'CUTID
            strSql += " ,CLARITYID = '" & ClarityId & "'" 'CLARITYID
            strSql += " ,SHAPEID = '" & ShapeId & "'" 'SHAPEID
            strSql += " WHERE SLNO = '" & Sno & "'"

            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal temp As Integer) As Integer
        strSql = " SELECT SLNO,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = CR.ITEMID)AS ITEMNAME,"
        strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.SUBITEMID = CR.SUBITEMID),'')AS SUBITEMNAME,"
        strSql += " FROMCENT,TOCENT,USRATE,INDRATE"
        strSql += " ,(SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = CR.CUTID)AS CUTNAME"
        strSql += " ,(SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID = CR.CLARITYID)AS CLARITYNAME"
        strSql += " ,(SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID = CR.SHAPEID)AS SHAPENAME"
        strSql += " FROM " & cnAdminDb & "..RAPAPORT AS CR"
        strSql += " WHERE SLNO = '" & temp & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            cmbItemName_Man.Text = .Item("ITEMNAME").ToString
            cmbSubItemName_Man.Text = .Item("SUBITEMNAME").ToString
            txtCentFrom.Text = .Item("FROMCENT").ToString
            txtCentTo.Text = .Item("TOCENT").ToString
            txtUSRate_Amt.Text = .Item("USRATE").ToString
            txtINDRate_Amt.Text = .Item("INDRATE").ToString
            CmbCut.Text = .Item("CUTNAME").ToString
            CmbClarity.Text = .Item("CLARITYNAME").ToString
            cmbShape.Text = .Item("SHAPENAME").ToString
        End With
        flagSave = True
        Sno = temp  
    End Function
    Function funcLoadItemName() As Integer
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE METALID = 'D' OR METALID = 'T'"
        strSql += " ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, cmbItemName_Man)
    End Function
    Function funcLoadSubItemName() As Integer
        cmbSubItemName_Man.Text = ""
        strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'")))
        'strSql = " select SubItemName from " & cnAdminDb & "..SubitemMast "
        'strSql += " where ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemName_Man.Text & "' and SubItem = 'Y')"
        'strSql += " order by subitemname"
        objGPack.FillCombo(strSql, cmbSubItemName_Man, True)
        If cmbSubItemName_Man.Items.Count > 0 Then cmbSubItemName_Man.Enabled = True Else cmbSubItemName_Man.Enabled = False
    End Function

    Private Sub frmRapaport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            cmbItemName_Man.Focus()
        End If
    End Sub

    Private Sub frmRapaport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbItemName_Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmRapaport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnl4c.Visible = True
        strSql = " SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE ISNULL(ACTIVE,'')='Y' ORDER BY CUTNAME"
        objGPack.FillCombo(strSql, CmbCut)
        strSql = " SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE ISNULL(ACTIVE,'')='Y' ORDER BY CLARITYNAME"
        objGPack.FillCombo(strSql, CmbClarity)
        strSql = " SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE ISNULL(ACTIVE,'')='Y' ORDER BY SHAPENAME"
        objGPack.FillCombo(strSql, cmbShape)
        funcGridStyle(gridView)
        funcNew()
    End Sub
    Function funcValidation() As Boolean
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
        If txtUSRate_Amt.Text = "" And txtINDRate_Amt.Text = "" Then
            MsgBox(E0007, MsgBoxStyle.Information)
            txtUSRate_Amt.Focus()
            Return True
        End If
        Return False
    End Function
    Function funcCheckUnique(ByVal frmCent As String, ByVal toCent As String) As Boolean
        Dim str As String = Nothing
        Dim dt As New DataTable
        dt.Clear()
        str = " DECLARE @FROMCENT AS FLOAT,@TOCENT AS FLOAT"
        str += " SET @FROMCENT = " & Val(frmCent) & ""
        str += " SET @TOCENT = " & Val(toCent) & ""
        str += " SELECT 1 FROM " & cnAdminDb & "..RAPAPORT"
        str += " WHERE"
        str += " ((FROMCENT BETWEEN @FROMCENT AND @TOCENT)OR (TOCENT BETWEEN @FROMCENT AND @TOCENT))"
        str += " AND "
        str += " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')"
        str += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
        str += " AND CUTID = '" & objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & CmbCut.Text & "'") & "'" 'CUTID
        str += " AND CLARITYID = '" & objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & CmbClarity.Text & "'") & "'" 'CLARITYID
        str += " AND SHAPEID = '" & objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & cmbShape.Text & "'") & "'" 'SHAPEID
        If flagSave = True Then
            str += " AND SLNO <> '" & Sno & "'"
        End If
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return True ''Already Exist
        End If
        Return False
    End Function
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
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
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub cmbItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbItemName_Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub cmbItemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName_Man.SelectedIndexChanged
        funcLoadSubItemName()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus

    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            cmbItemName_Man.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value)
                cmbItemName_Man.Focus()
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..RAPAPORT WHERE 1<>1"
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SLNO").Value.ToString
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..RAPAPORT WHERE SLNO = '" & delKey & "'")
        funcCallGrid()
    End Sub

    Private Sub txtCentFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCentFrom.GotFocus
        If flagSave = True Then Exit Sub
        'If Val(txtWtFrom_Wet.Text) <> 0 Then Exit Sub
        strSql = " SELECT MAX(TOCENT) FROM " & cnAdminDb & "..RAPAPORT"
        strSql += vbCrLf + " WHERE "
        strSql += vbCrLf + " ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'),0)"
        strSql += vbCrLf + " AND ISNULL(SUBITEMID,0) = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
        Dim wt As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" Then
            txtCentFrom.Text = Format(wt + 0.0001, FormatNumberStyle(DiaRnd))
        Else
            txtCentFrom.Text = Format(wt + 0.001, "0.000")
        End If
    End Sub

    Private Sub txtCentFrom_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCentFrom.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
        Else
            If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" And DiaRnd = 4 Then
                WeightValidation(txtCentFrom, e, DiaRnd)
            Else
                WeightValidation(txtCentFrom, e)
            End If
        End If
    End Sub

    Private Sub txtCentTo_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCentTo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
        Else
            If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" And DiaRnd = 4 Then
                WeightValidation(txtCentTo, e, DiaRnd)
            Else
                WeightValidation(txtCentTo, e)
            End If
        End If
    End Sub


    Private Sub txtCentFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCentFrom.LostFocus
        If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" Then
            txtCentFrom.Text = IIf(Val(txtCentFrom.Text) <> 0, Format(Val(txtCentFrom.Text), FormatNumberStyle(DiaRnd)), txtCentFrom.Text)
        Else
            txtCentFrom.Text = IIf(Val(txtCentFrom.Text) <> 0, Format(Val(txtCentFrom.Text), "0.000"), txtCentFrom.Text)
        End If
    End Sub

    Private Sub txtCentTo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCentTo.LostFocus
        If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" Then
            txtCentTo.Text = IIf(Val(txtCentTo.Text) <> 0, Format(Val(txtCentTo.Text), FormatNumberStyle(DiaRnd)), txtCentTo.Text)
        Else
            txtCentTo.Text = IIf(Val(txtCentTo.Text) <> 0, Format(Val(txtCentTo.Text), "0.000"), txtCentTo.Text)
        End If
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If gridView.Rows.Count > 0 Then
            For k As Integer = 0 To gridView.Rows.Count - 1
                Dim ItemId As Integer = Nothing
                Dim SubItemId As Integer = Nothing

                strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE"
                strSql += " ITEMNAME = '" & gridView.Rows(k).Cells("ITEMNAME").Value.ToString & "'"
                ItemId = Val(objGPack.GetSqlValue(strSql))

                strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE"
                strSql += " SUBITEMNAME = '" & gridView.Rows(k).Cells("SUBITEMNAME").Value.ToString & "'"
                strSql += " AND ITEMID = " & ItemId & ""
                SubItemId = Val(objGPack.GetSqlValue(strSql))

                Dim CutId As Integer = 0
                Dim ClarityId As Integer = 0
                Dim ShapeId As Integer = 0
                CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & gridView.Rows(k).Cells("CUTNAME").Value.ToString & "'", "CUTID", 0)
                ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & gridView.Rows(k).Cells("CLARITYNAME").Value.ToString & "'", "CLARITYID", 0)
                ShapeId = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & gridView.Rows(k).Cells("SHAPENAME").Value.ToString & "'", "SHAPEID", 0)

                'UPDATE ITEMTAG 
                strSql = " SELECT * FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=" & ItemId & ""
                strSql += " AND SUBITEMID=" & SubItemId & ""
                strSql += " AND CUTID=" & CutId & " AND CLARITYID=" & ClarityId & " AND SHAPEID=" & ShapeId & ""
                Dim dtitemtag As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtitemtag)
                If dtitemtag.Rows.Count > 0 Then
                    For Each ro As DataRow In dtitemtag.Rows
                        Dim cent As Double = 0
                        cent = (Val(ro.Item("GRSWT").ToString) / IIf(Val(ro.Item("PCS").ToString) = 0, 1, Val(ro.Item("PCS").ToString)))
                        cent *= 100
                        If Val(txtCentFrom.Text) < cent And Val(txtCentTo.Text) > cent Then
                            strSql = "UPDATE " & cnAdminDb & "..ITEMTAG SET "
                            strSql += vbCrLf + " USRATE=" & Val(txtUSRate_Amt.Text) & ""
                            strSql += vbCrLf + " WHERE SNO='" & ro.Item("SNO").ToString & "'"
                            cmd = New OleDbCommand(strSql, cn)
                            cmd.ExecuteNonQuery()
                        End If
                    Next
                End If

                'UPDATE ITEMTAGSTONE 
                strSql = " SELECT * FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE STNITEMID=" & ItemId & ""
                strSql += " AND STNSUBITEMID=" & SubItemId & ""
                strSql += " AND CUTID=" & CutId & " AND CLARITYID=" & ClarityId & " AND SHAPEID=" & ShapeId & ""
                Dim dtitemtagstn As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtitemtagstn)
                If dtitemtagstn.Rows.Count > 0 Then
                    For Each ro As DataRow In dtitemtagstn.Rows
                        Dim cent As Double = 0
                        cent = (Val(ro.Item("STNWT").ToString) / IIf(Val(ro.Item("STNPCS").ToString) = 0, 1, Val(ro.Item("STNPCS").ToString)))
                        cent *= 100
                        If Val(txtCentFrom.Text) < cent And Val(txtCentTo.Text) > cent Then
                            strSql = "UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET "
                            strSql += vbCrLf + " USRATE=" & Val(txtUSRate_Amt.Text) & ""
                            strSql += vbCrLf + " WHERE SNO='" & ro.Item("SNO").ToString & "'"
                            cmd = New OleDbCommand(strSql, cn)
                            cmd.ExecuteNonQuery()
                        End If
                    Next
                End If
            Next
            MsgBox("USD updated successfully completed", MsgBoxStyle.Information)
        End If
    End Sub
End Class