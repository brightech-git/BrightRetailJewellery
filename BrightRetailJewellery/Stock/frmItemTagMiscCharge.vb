Imports System.Data.OleDb
Public Class frmItemTagMiscCharge
    Dim dtTagDet As New DataTable
    Dim strSql As String
    Dim dtMiscDetails As New DataTable("GridMisc")
    Dim updIssSno As String
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Public Sub New(ByVal ISSsno As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        updIssSno = ISSsno
        ''OtherCharges
        With dtMiscDetails.Columns
            .Add("MISC", GetType(String))
            .Add("AMOUNT", GetType(Double))
            .Add("PURAMOUNT", GetType(Double))
            .Add("KEYNO", GetType(Integer))
        End With
        dtMiscDetails.Columns("KEYNO").AutoIncrement = True
        dtMiscDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtMiscDetails.Columns("KEYNO").AutoIncrementSeed = 1
        gridMisc.DataSource = dtMiscDetails
        StyleGridMisc()
        Dim dtMiscFooter As New DataTable
        dtMiscFooter = dtMiscDetails.Copy
        dtMiscFooter.Rows.Clear()
        dtMiscFooter.Rows.Add()
        dtMiscFooter.Rows(0).Item("MISC") = "TOTAL"
        dtMiscDetails.AcceptChanges()
        gridMiscFooter.DataSource = dtMiscFooter
        With gridMiscFooter
            For cnt As Integer = 0 To gridMisc.ColumnCount - 1
                .Columns(cnt).Width = gridMisc.Columns(cnt).Width
                .Columns(cnt).Visible = gridMisc.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = gridMisc.Columns(cnt).DefaultCellStyle
            Next
            .DefaultCellStyle.BackColor = Color.Gainsboro
            .DefaultCellStyle.SelectionBackColor = Color.Gainsboro
        End With
        ''MISCCHARGE
        strSql = " SELECT"
        strSql += " (SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCID = T.MISCID)MISC"
        strSql += " ,AMOUNT"
        strSql += " ,(SELECT PURAMOUNT FROM " & cnAdminDb & "..PURITEMTAGMISCCHAR WHERE MISSNO = T.SNO)PURAMOUNT "
        strSql += " FROM " & cnAdminDb & "..ITEMTAGMISCCHAR AS T"
        strSql += " WHERE TAGSNO = '" & updIssSno & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMiscDetails)
        StyleGridMisc()
        dtMiscDetails.AcceptChanges()
        CalcMiscTotalAmount()
    End Sub

#Region "Miscellaneous Procedures"
    Private Sub CalcMiscTotalAmount()
        Dim miscTot As Double = Nothing
        For cnt As Integer = 0 To gridMisc.Rows.Count - 1
            miscTot += Val(gridMisc.Rows(cnt).Cells("AMOUNT").Value.ToString)
        Next
        gridMiscFooter.Rows(0).Cells("AMOUNT").Value = IIf(miscTot <> 0, Format(miscTot, "0.00"), DBNull.Value)
        txtMiscTotAmt.Text = IIf(miscTot <> 0, Format(miscTot, "0.00"), "")
    End Sub

    Private Sub txtMiscAmount_Amt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMiscAmount_Amt.KeyDown
        If e.KeyCode = Keys.Down Then
            gridMisc.Select()
        End If
    End Sub

    Private Sub txtMiscAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMiscAmount_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMiscMisc.Text = "" Then
                MsgBox(Me.GetNextControl(txtMiscMisc, False).Text + E0001, MsgBoxStyle.Information)
                txtMiscMisc.Select()
                Exit Sub
            End If
            If objGPack.DupCheck("SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & txtMiscMisc.Text & "' AND  ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Misc", MsgBoxStyle.Information)
                txtMiscMisc.Focus()
                Exit Sub
            End If
            If Not Val(txtMiscAmount_Amt.Text) > 0 Then
                MsgBox(Me.GetNextControl(txtMiscAmount_Amt, False).Text + E0001, MsgBoxStyle.Information)
                txtMiscAmount_Amt.Select()
                Exit Sub
            End If
            If txtMiscRowIndex.Text <> "" Then
                With gridMisc.Rows(Val(txtMiscRowIndex.Text))
                    .Cells("MISC").Value = txtMiscMisc.Text
                    .Cells("AMOUNT").Value = IIf(Val(txtMiscAmount_Amt.Text) <> 0, Val(txtMiscAmount_Amt.Text), DBNull.Value)
                    dtMiscDetails.AcceptChanges()
                    GoTo AFTERINSERT
                End With
            End If
            Dim ro As DataRow = Nothing
            ro = dtMiscDetails.NewRow
            ro("MISC") = txtMiscMisc.Text
            ro("AMOUNT") = IIf(Val(txtMiscAmount_Amt.Text) <> 0, Val(txtMiscAmount_Amt.Text), DBNull.Value)
            dtMiscDetails.Rows.Add(ro)
            dtMiscDetails.AcceptChanges()
AFTERINSERT:
            CalcMiscTotalAmount()
            gridMisc.CurrentCell = gridMisc.Rows(gridMisc.RowCount - 1).Cells(0)

            txtMiscMisc.Clear()
            txtMiscAmount_Amt.Clear()
            txtMiscMisc.Select()
            txtMiscRowIndex.Clear()
        End If
    End Sub

    Private Sub gridMisc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridMisc.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridMisc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridMisc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            gridMisc.CurrentCell = gridMisc.Rows(gridMisc.CurrentRow.Index).Cells(0)
            With gridMisc.Rows(gridMisc.CurrentRow.Index)
                txtMiscMisc.Text = .Cells("MISC").FormattedValue
                txtMiscAmount_Amt.Text = .Cells("AMOUNT").FormattedValue
                txtMiscRowIndex.Text = gridMisc.CurrentRow.Index
                txtMiscMisc.Focus()
                txtMiscMisc.SelectAll()
            End With
        End If
    End Sub

    Private Sub gridMisc_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridMisc.UserDeletedRow
        dtMiscDetails.AcceptChanges()
        CalcMiscTotalAmount()
        If Not gridMisc.RowCount > 0 Then
            txtMiscMisc.Select()
        End If
    End Sub
#End Region



    Private Sub frmTagCheck_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub StyleGridMisc()
        With gridMisc
            With .Columns("MISC")
                .HeaderText = "MISCELLANEOUS"
                .Width = 298
            End With
            With .Columns("AMOUNT")
                .HeaderText = "AMOUNT"
                .Width = 99
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("PURAMOUNT")
                .Visible = False
                .Width = 99
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            .Columns("KEYNO").Visible = False
        End With
    End Sub


    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcsave()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcsave()
    End Sub

    Private Sub funcsave()
        Try
            Dim TagNo As String = Nothing
            Dim COSTID As String = Nothing
            Dim ITemid As Integer = Nothing
            Dim dt As New DataTable
            strSql = "  SELECT ITEMID,TAGNO,COSTID"
            strSql += "  FROM " & cnAdminDb & "..ITEMTAG "
            strSql += " WHERE SNO = '" & updIssSno & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then Exit Sub
            ITemid = Val(dt.Rows(0).Item(0).ToString())
            TagNo = dt.Rows(0).Item(1).ToString()
            COSTID = dt.Rows(0).Item(2).ToString()

            tran = Nothing
            tran = cn.BeginTransaction()

            ''DELETING MISCDETAIL
            strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGMISCCHAR"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            ''iNSERTING mISC
            For cnt As Integer = 0 To dtMiscDetails.Rows.Count - 1
                Dim miscSno As String = GetNewSno(TranSnoType.ITEMTAGMISCCHARCODE, tran, "GET_ADMINSNO_TRAN")
                With dtMiscDetails.Rows(cnt)
                    Dim miscId As String = Nothing
                    strSql = " SELECT MISCID FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & .Item("MISC").ToString & "'"
                    miscId = Val(objGPack.GetSqlValue(strSql, "MISCID", , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGMISCCHAR"
                    strSql += " ("
                    strSql += " SNO,ITEMID,TAGNO,MISCID,AMOUNT,"
                    strSql += " TAGSNO,COSTID,SYSTEMID,APPVER,COMPANYID)VALUES("
                    strSql += " '" & miscSno & "'" 'SNO
                    strSql += " ," & ITemid & "" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ," & miscId & "" 'MISCID
                    strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'AMOUNT
                    strSql += " ,'" & updIssSno & "'" 'TAGSNO
                    strSql += " ,'" & COSTID & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & GetStockCompId() & "'" ' COMPANYID
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End With
            Next
            tran.Commit()
            tran = Nothing
            MsgBox(TagNo + E0009, MsgBoxStyle.Exclamation)
            Me.Dispose()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub txtMiscMisc_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMiscMisc.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadMiscName()
        ElseIf e.KeyCode = Keys.Down Then
            If gridMisc.RowCount > 0 Then gridMisc.Focus()
        End If
    End Sub

    Private Sub txtMiscMisc_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMiscMisc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMiscMisc.Text = "" Then Exit Sub
            If objGPack.DupCheck("SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & txtMiscMisc.Text & "' AND  ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Misc", MsgBoxStyle.Information)
                txtMiscMisc.Focus()
            End If
        End If
    End Sub

    Private Sub LoadMiscName()
        strSql = " SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE ACTIVE = 'Y'"
        Dim Name As String = BrighttechPack.SearchDialog.Show("Find MiscName", strSql, cn, , , , txtMiscMisc.Text)
        If Name <> "" Then
            txtMiscMisc.Text = Name
            LoadMiscDetails()
        Else
            txtMiscMisc.Focus()
            txtMiscMisc.SelectAll()
        End If
    End Sub
    Private Sub LoadMiscDetails()
        If txtMiscMisc.Text <> "" Then
            strSql = " SELECT DEFAULTVALUE FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & txtMiscMisc.Text & "'"
            Dim amt As Double = Val(objGPack.GetSqlValue(strSql, "DEFAULTVALUE", , tran))
            txtMiscAmount_Amt.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
            Me.SelectNextControl(txtMiscMisc, True, True, True, True)
        End If
    End Sub

End Class