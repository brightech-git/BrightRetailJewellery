Imports System.Data.OleDb
Public Class frmStateCategory
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtGrid As New DataTable
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        strSql = " SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST ORDER BY STATENAME"
        objGPack.FillCombo(strSql, cmbState_MAN)
        cmbOpenState.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbOpenState, False, False)
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY ORDER BY CATNAME "
        objGPack.FillCombo(strSql, cmbCategory_MAN)
    End Sub

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        tabMain.SelectedTab = tabGeneral
        cmbState_MAN.Focus()
    End Sub

    Private Sub frmStateCategory_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
                cmbState_MAN.Focus()
            End If
        End If
    End Sub

    Private Sub frmStateCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmStateCategory_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub Save()
        Dim stateId As String = objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & cmbState_MAN.Text & "'")
        Dim catCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "'")
        tran = Nothing
        tran = cn.BeginTransaction
        Try
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..STATECATEGORY "
            strSql += " WHERE STATEID = (SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & cmbState_MAN.Text & "')"
            strSql += " AND CATCODE = (sELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "')"
            If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
                strSql = " UPDATE " & cnAdminDb & "..STATECATEGORY SET"
                strSql += " CATCODE = '" & catCode & "'" 'CATCODE
                strSql += " ,SALESTAX = " & Val(txtSaTax_PER.Text) & ""
                strSql += " ,SSC = " & Val(txtSaSc_PER.Text) & ""
                strSql += " ,SASC = " & Val(txtSaAdlSc_PER.Text) & ""
                strSql += " ,PTAX = " & Val(txtPuTax_PER.Text) & ""
                strSql += " ,PSC = " & Val(txtPuSc_PER.Text) & ""
                strSql += " ,PASC = " & Val(txtPuAdlSc_PER.Text) & ""
                strSql += " WHERE STATEID = '" & stateId & "'"
                ExecQuery(SyncMode.Master, strSql, cn, tran, , stateId)
                tran.Commit()
                btnNew_Click(Me, New EventArgs)
                MsgBox("Updated..")
            Else
                strSql = " INSERT INTO " & cnAdminDb & "..STATECATEGORY"
                strSql += " ("
                strSql += " STATEID,CATCODE,SALESTAX"
                strSql += " ,SSC,SASC,PTAX,PSC,PASC"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ("
                strSql += " '" & stateId & "'" 'STATEID
                strSql += " ,'" & catCode & "'" 'CATCODE
                strSql += " ," & Val(txtSaTax_PER.Text) & "" 'SALESTAX
                strSql += " ," & Val(txtSaSc_PER.Text) & "" 'SSC
                strSql += " ," & Val(txtSaAdlSc_PER.Text) & "" 'SASC
                strSql += " ," & Val(txtPuTax_PER.Text) & "" 'PTAX
                strSql += " ," & Val(txtPuSc_PER.Text) & "" 'PSC
                strSql += " ," & Val(txtPuAdlSc_PER.Text) & "" 'PASC
                strSql += " )"
                ExecQuery(SyncMode.Master, strSql, cn, tran, , stateId)
                tran.Commit()
                btnNew_Click(Me, New EventArgs)
                MsgBox("Inserted..")
            End If
            tran = Nothing
        Catch ex As Exception
            If tran IsNot Nothing Then
                tran.Rollback()
                tran = Nothing
            End If
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then
            Exit Sub
        End If
        Try
            Save()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub CallGrid()
        strSql = " SELECT "
        strSql += " (SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID = S.STATEID)aS STATE"
        strSql += " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)AS CATEGORY"
        strSql += " ,SALESTAX,SSC,SASC,PTAX,PSC,PASC,STATEID,CATCODE"
        strSql += " FROM " & cnAdminDb & "..STATECATEGORY AS S"
        If cmbOpenState.Text <> "ALL" Then
            strSql += " WHERE STATEID = (SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & cmbOpenState.Text & "')"
        End If
        dtGrid = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        gridView.DataSource = dtGrid
        gridView.Columns("STATEID").Visible = False
        gridView.Columns("CATCODE").Visible = False
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        cmbOpenState.Text = "ALL"
        CallGrid()
        tabMain.SelectedTab = tabView
        cmbOpenState.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub cmbCategory_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCategory_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT SALESTAX,SSC,SASC,PTAX,PSC,PASC FROM " & cnAdminDb & "..STATECATEGORY "
            strSql += " WHERE STATEID = (SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & cmbState_MAN.Text & "')"
            strSql += " AND CATCODE = (sELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "')"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            For Each ro As DataRow In dt.Rows
                txtSaTax_PER.Text = IIf(Val(ro!SALESTAX.ToString) <> 0, Format(Val(ro!SALESTAX.ToString), "0.00"), Nothing)
                txtSaSc_PER.Text = IIf(Val(ro!SSC.ToString) <> 0, Format(Val(ro!SSC.ToString), "0.00"), Nothing)
                txtSaAdlSc_PER.Text = IIf(Val(ro!SASC.ToString) <> 0, Format(Val(ro!SASC.ToString), "0.00"), Nothing)
                txtPuTax_PER.Text = IIf(Val(ro!PTAX.ToString) <> 0, Format(Val(ro!PTAX.ToString), "0.00"), Nothing)
                txtPuSc_PER.Text = IIf(Val(ro!PSC.ToString) <> 0, Format(Val(ro!PSC.ToString), "0.00"), Nothing)
                txtPuAdlSc_PER.Text = IIf(Val(ro!PASC.ToString) <> 0, Format(Val(ro!PASC.ToString), "0.00"), Nothing)
            Next
            If Not dt.Rows.Count > 0 Then
                objGPack.TextClear(grpSales)
                objGPack.TextClear(grpPurchase)
            End If
        End If
    End Sub

    Private Sub cmbOpenState_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOpenState.SelectedIndexChanged
        CallGrid()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        If MessageBox.Show("Do you want to Delete the current row?", "Del Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = MessageBoxDefaultButton.Button2 Then
            Exit Sub
        End If
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            strSql = " DELETE FROM " & cnAdminDb & "..STATECATEGORY"
            strSql += " WHERE STATEID = " & gridView.CurrentRow.Cells("STATEID").Value & ""
            strSql += " AND CATCODE = '" & gridView.CurrentRow.Cells("CATCODE").Value.ToString & "'"
            ExecQuery(SyncMode.Master, strSql, cn, tran, , gridView.CurrentRow.Cells("STATEID").Value)
            tran.Commit()
            MsgBox("Deleted Successfully", MsgBoxStyle.Information)
            cmbOpenState_SelectedIndexChanged(Me, New EventArgs)
        Catch ex As Exception
            If tran.Connection IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
End Class

