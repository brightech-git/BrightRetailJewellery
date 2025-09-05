Imports System.Data.OleDb
Public Class frmSchemeOfferRange
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim flagUpdate As Boolean
    Dim strSql As String
    Dim dt As New DataTable
    Dim da1 As OleDbDataAdapter
    Dim CSchemeid As Integer
    Dim CInsfrom As Integer
    Dim CInsto As Integer

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='C' ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbScheme, False, False)

        CmbActive.Items.Add("YES")
        CmbActive.Items.Add("NO")

    End Sub

    Function funcClear()
        cmbScheme.Text = ""
        txtInsFrom.Clear()
        txtInsTo.Clear()
        txtApplicablePer.Clear()
        Return 0
    End Function
    Function funcNew()
        tabMain.SelectedTab = tabGen
        flagUpdate = False
        funcClear()
        cmbScheme.Focus()
        CmbActive.Text = "YES"
        Return 0
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If Not flagUpdate Then
            funcAdd()
        Else
            funcUpdate()
        End If
    End Function
    Function funcAdd()

        Dim schemeid As Integer
        Dim costid As Integer
        If cmbScheme.Text = "" Then
            MsgBox("Scheme name should not empty", MsgBoxStyle.Information)
            cmbScheme.Focus()
            Exit Function
        End If

        Dim dtscheme As New DataTable
        strSql = " SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & cmbScheme.Text & "' AND CARDTYPE='C'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtscheme)
        If dtscheme.Rows.Count > 0 Then
            schemeid = Val(dtscheme.Rows(0).Item("CARDCODE").ToString)
        End If
        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SCHEMEOFFERRANGE"
        strSql += " WHERE SCHEMEID = " & schemeid & ""
        strSql += " AND INSFROM = " & txtInsFrom.Text & ""
        strSql += " AND INSTO = " & txtInsTo.Text & ""
        If objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox("This Group Already Exist")
            Exit Function
        End If

        strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..SCHEMEOFFERRANGE"
        strSql += vbCrLf + " (SCHEMEID,OFFERPER,INSFROM,INSTO,ACTIVE,USERID,UPDATED,UPTIME"
        strSql += vbCrLf + " ) VALUES ("
        strSql += vbCrLf + " " & schemeid & "" 'SCHEMEID
        strSql += vbCrLf + "," & Val(txtApplicablePer.Text) & "" 'APPLICABLE %
        strSql += vbCrLf + "," & Val(txtInsFrom.Text) & "" 'INSFROM
        strSql += vbCrLf + "," & Val(txtInsTo.Text) & "" 'INSTO
        strSql += vbCrLf + ",'" & Mid(CmbActive.Text, 1, 1) & "'" 'ACTIVE
        strSql += vbCrLf + "," & userId & "" 'USERID
        strSql += vbCrLf + ",'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "')" 'UPTIME
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            MsgBox("Saved..", MsgBoxStyle.Information)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
        Return 0
    End Function
    Function funcUpdate()
        Dim schemeid As Integer
        Dim costid As Integer
        Dim dtscheme As New DataTable
        strSql = " SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & cmbScheme.Text & "' AND CARDTYPE='C'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtscheme)
        If dtscheme.Rows.Count > 0 Then
            schemeid = Val(dtscheme.Rows(0).Item("CARDCODE").ToString)
        End If

        strSql = "SELECT 1 FROM " & cnAdminDb & "..SCHEMEOFFERRANGE "
        strSql += " WHERE SCHEMEID = " & schemeid & ""
        strSql += " AND INSFROM = " & txtInsFrom.Text & ""
        strSql += " AND INSTO = " & txtInsTo.Text & ""
        If objGPack.GetSqlValue(strSql, , 0).Length > 0 Then
            strSql = " DELETE " & cnAdminDb & "..SCHEMEOFFERRANGE"
            strSql += " WHERE SCHEMEID = " & schemeid & ""
            strSql += " AND INSFROM = " & txtInsFrom.Text & ""
            strSql += " AND INSTO = " & txtInsTo.Text & ""
            ExecQuery(SyncMode.Master, strSql, cn)
        End If
        strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..SCHEMEOFFERRANGE"
        strSql += vbCrLf + " (SCHEMEID,OFFERPER,INSFROM,INSTO,ACTIVE,USERID,UPDATED,UPTIME"
        strSql += vbCrLf + " ) VALUES ("
        strSql += vbCrLf + " " & CSchemeid & "" 'SCHEMEID
        strSql += vbCrLf + "," & Val(txtApplicablePer.Text) & "" 'APPLICABLE %
        strSql += vbCrLf + "," & CInsfrom & "" 'INSFROM
        strSql += vbCrLf + "," & CInsto & "" 'INSTO
        strSql += vbCrLf + ",'" & Mid(CmbActive.Text, 1, 1) & "'" 'ACTIVE
        strSql += vbCrLf + "," & userId & "" 'USERID
        strSql += vbCrLf + ",'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "')" 'UPTIME
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            MsgBox("Updated..", MsgBoxStyle.Information)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
        Return 0
    End Function

    Private Sub frmSchemePer_Ins_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{tab}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub frmSchemePer_Ins_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        funcNew()
        cmbScheme.Select()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click, btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcView()
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

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If Not gridView.RowCount > 0 Then Exit Sub
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("SCHEME")
            With gridView.CurrentRow
                cmbScheme.Text = .Cells("SCHEME").Value.ToString
                txtApplicablePer.Text = .Cells("OFFERPER").Value.ToString
                txtInsFrom.Text = .Cells("INSFROM").Value.ToString
                txtInsTo.Text = .Cells("INSTO").Value.ToString
                CmbActive.Text = IIf((.Cells("ACTIVE").Value.ToString) = "Y", "YES", "NO")
                CSchemeid = Val(.Cells("SCHEMEID").Value.ToString)
                CInsfrom = Val(.Cells("INSFROM").Value.ToString)
                CInsto = Val(.Cells("INSTO").Value.ToString)
            End With
            tabMain.SelectedTab = tabGen
            cmbScheme.Focus()
            flagUpdate = True
        End If
    End Sub


    Function funcView() As Integer
        Try
            Me.btnOpen.Enabled = False
            'Me.Cursor = Cursors.WaitCursor

            strSql = vbCrLf + " SELECT S.SCHEMEID,C.NAME AS SCHEME"
            strSql += vbCrLf + " ,OFFERPER,INSFROM,INSTO,S.ACTIVE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..SCHEMEOFFERRANGE S"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CREDITCARD C ON C.CARDCODE=S.SCHEMEID "
            strSql += vbCrLf + " ORDER BY SCHEMEID"
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)

            If dt.Rows.Count <= 0 Then
                MsgBox("No Records to View", MsgBoxStyle.Information)
                Exit Function
            End If

            gridView.DataSource = dt
            funcGridStyle()
            If gridView.Rows.Count > 0 Then
                tabMain.SelectedTab = tabView
                gridView.Focus()
            End If
        Catch ex As Exception
            MsgBox("Error : " & ex.Message & " Position : " & MsgBox(ex.StackTrace), MsgBoxStyle.Critical)
        Finally
            Me.btnOpen.Enabled = True
            Me.Cursor = Cursors.Arrow
        End Try
    End Function
    Function funcGridStyle() As Integer
        With gridView
            .RowHeadersVisible = False
            .Columns("SCHEMEID").Visible = False
            .Columns("ACTIVE").Visible = False
            .Columns("INSFROM").HeaderText = "INSTALLMENT FROM"
            .Columns("INSTO").HeaderText = "INSTALLMENT TO"
            .Columns("OFFERPER").HeaderText = "APPLICABLE %"
            .Columns("SCHEME").Width = 200
            .Columns("INSFROM").Width = 160
            .Columns("INSTO").Width = 150
            .Columns("OFFERPER").Width = 130
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
        End With

    End Function
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        cmbScheme.Focus()
    End Sub
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..SCHEMEOFFERRANGE WHERE 1<>1"
        Dim delid As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SCHEMEID").Value.ToString
        Dim delfrom As String = gridView.Rows(gridView.CurrentRow.Index).Cells("INSFROM").Value.ToString
        Dim delto As String = gridView.Rows(gridView.CurrentRow.Index).Cells("INSTO").Value.ToString
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..SCHEMEOFFERRANGE WHERE SCHEMEID = '" & delid & "'AND INSFROM='" & delfrom & "' AND INSTO='" & delto & "' ")
        funcView()
        If gridView.RowCount > 0 Then gridView.Focus() Else btnBack_Click(Me, New EventArgs)

    End Sub
End Class