Imports System.Data.OleDb
Public Class frmPurItemType
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim Pitemtypeid As Integer
    Dim PFromwt As Decimal
    Dim PTowt As Decimal

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        funcGridStyle(gridView)
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        tabGeneral.BackgroundImage = bakImage
        tabView.BackgroundImage = bakImage
        tabGeneral.BackgroundImageLayout = ImageLayout.Stretch
        tabGeneral.BackgroundImageLayout = ImageLayout.Stretch

        pnlGrid.Dock = DockStyle.Fill


        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"
        funcNew()

    End Sub

    Function funcNew() As Integer
        Dim dt As New DataTable
        dt.Clear()
        cmbItemtypeid.Items.Clear()
        cmbItemtypeid.Items.Add("ALL")
        cmbItemtypeid.Text = "ALL"
        strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE ORDER BY DISPALYORDER"
        objGPack.FillCombo(strSql, cmbItemtypeid, False, False)
        objGPack.TextClear(Me)

        tabMain.SelectedTab = tabGeneral
        cmbItemtypeid.Focus()
    End Function
    Function funcSave() As Integer
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcOpen() As Integer
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        tabMain.SelectedTab = tabView
        strSql = " SELECT"
        strSql += " A.ITEMTYPEID,isnull(NAME,'ALL') NAME ,RANGEFROM,RANGETO"
        strSql += " ,DUSTWTPER,DUSTWT,MELTPER,MELTWT,CASHWASTPER,PURCHWASTPER,RATELESSPER"
        strSql += " FROM " & cnAdminDb & "..PURITEMTYPE a left join  " & cnAdminDb & "..ITEMTYPE b on a.ITEMTYPEID = B.ITEMTYPEID "
        funcOpenGrid(strSql, gridView)
        With gridView
            .Columns("ITEMTYPEID").HeaderText = "ITEMTYPEID"
            .Columns("ITEMTYPEID").Width = 0
            .Columns("ITEMTYPEID").Visible = False
            .Columns("NAME").HeaderText = "DESCRIPTION"
            .Columns("NAME").Width = 180
            .Columns("RANGEFROM").Width = 120
            .Columns("RANGEFROM").HeaderText = "RANGE FROM"
            .Columns("RANGETO").Width = 100
            .Columns("RANGETO").HeaderText = "RANGE TO"
            .Columns("DUSTWTPER").HeaderText = "DUST WAST%"
            .Columns("DUSTWTPER").Width = 120
            .Columns("DUSTWT").HeaderText = "DUST WASTAGE"
            .Columns("DUSTWT").Width = 120
            .Columns("MELTPER").HeaderText = "MELTING %"
            .Columns("MELTPER").Width = 120
            .Columns("MELTWT").HeaderText = "MELT WEIGHT"
            .Columns("MELTWT").Width = 120

            .Columns("CASHWASTPER").Width = 120
            .Columns("CASHWASTPER").HeaderText = "CASH WAST%"
            .Columns("PURCHWASTPER").Width = 150
            .Columns("PURCHWASTPER").HeaderText = "EXCHANGE WAST%"
            .Columns("RATELESSPER").Width = 150
            .Columns("RATELESSPER").HeaderText = "RATE LESS %"
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
        End With
        BrighttechPack.FormatGridColumns(gridView, False, True, False)
        'For CNT As Integer = 7 To gridView.ColumnCount - 1
        '    gridView.Columns(CNT).Visible = False
        'Next
        gridView.Select()
    End Function
    Function funcAdd() As Integer
        Dim CatCode As String = Nothing
        Dim PurityId As String = Nothing
        Dim softModule As String = Nothing

        Dim ds As New Data.DataSet
        ds.Clear()

        strSql = " Insert into " & cnAdminDb & "..PURITEMTYPE"
        strSql += " ("
        strSql += " ITEMTYPEID,RANGEFROM,RANGETO,DUSTWTPER,DUSTWT,MELTPER,MELTWT,CASHWASTPER,PURCHWASTPER"
        strSql += " ,CASHWASTPER_OTH,PURCHWASTPER_OTH,ACTIVE,USERID,UPDATED,UPTIME"
        strSql += " )Values ("
        strSql += " " & Val(objGPack.GetSqlValue("Select Itemtypeid from " & cnAdminDb & "..ITEMTYPE where name = '" & cmbItemtypeid.Text & "'")) & "" 'ItemTypeID
        strSql += " ," & Val(txtRangefrom.Text) & "" 'RANGEFROM
        strSql += " ," & Val(txtRangeto.Text) & "" 'RANGETO
        strSql += " ," & Val(txt_DustWastPer.Text) & "" 'DUSTWTPER
        strSql += " ," & Val(txt_DustWeight.Text) & "" 'DUSTWT
        strSql += " ," & Val(txtMelt_Per.Text) & "" 'DUSTWTPER
        strSql += " ," & Val(txtMelt_WET.Text) & "" 'DUSTWT
        strSql += " ," & Val(txtWastPerCash.Text) & "" 'WASTPER
        strSql += " ," & Val(txtWastPerEx.Text) & "" '
        strSql += " ," & Val(txtWastPerCash_oth.Text) & "" 'WASTPER
        strSql += " ," & Val(txtWastPerEx_oth.Text) & "" '
        strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
        strSql += " ," & Val(userId) & "" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += " )"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)

            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate() As Integer
        Dim CatCode As String = Nothing
        Dim PurityId As String = Nothing
        Dim softModule As String = Nothing


        Dim ds As New Data.DataSet
        ds.Clear()

        'strSql += " ITEMTYPEID,RANGEFROM,RANGETO,DUSTWTPER,WASTPER,RATELESSPER"
        'strSql += " ,ACTIVE,USERID,UPDATED,UPTIME"
        strSql = " Update " & cnAdminDb & "..PURITEMTYPE Set"
        strSql += " RANGEFROM=" & Val(txtRangefrom.Text) & ""
        strSql += " ,RANGETO=" & Val(txtRangeto.Text) & ""
        strSql += " ,DUSTWTPER=" & Val(txt_DustWastPer.Text) & ""
        strSql += " ,DUSTWT=" & Val(txt_DustWeight.Text) & ""
        strSql += " ,MELTPER=" & Val(txtMelt_Per.Text) & ""
        strSql += " ,MELTWT=" & Val(txtMelt_WET.Text) & ""
        strSql += " ,CASHWASTPER=" & Val(txtWastPerCash.Text) & ""
        strSql += " ,PURCHWASTPER=" & Val(txtWastPerEx.Text) & ""
        strSql += " ,CASHWASTPER_OTH=" & Val(txtWastPerCash_oth.Text) & ""
        strSql += " ,PURCHWASTPER_OTH=" & Val(txtWastPerEx_oth.Text) & ""
        strSql += " ,ACTIVE='" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " ,UserId=" & userId & ""
        strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        strSql += " Where ItemTypeId = " & Val(objGPack.GetSqlValue("Select Itemtypeid from " & cnAdminDb & "..ITEMTYPE where name = '" & cmbItemtypeid.Text & "'")) & ""
        strSql += " AND RANGEFROM=" & PFromwt & " AND RANGETO=" & PTowt & ""
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
            flagSave = False
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal tempItemTypeId As String, ByVal tempname As String, ByVal Rangefrom As String, ByVal Rangeto As String) As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT"
        strSql += " ITEMTYPEID,RANGEFROM,RANGETO,DUSTWTPER,DUSTWT,MELTPER,MELTWT,CASHWASTPER,PURCHWASTPER,CASHWASTPER_OTH,PURCHWASTPER_OTH"
        strSql += " FROM " & cnAdminDb & "..PURITEMTYPE AS I"
        strSql += " WHERE ITEMTYPEID = '" & tempItemTypeId & "' and RANGEFROM='" & Rangefrom & "' AND RANGETO='" & Rangeto & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            cmbItemtypeid.Text = tempname
            Pitemtypeid = Val(.Item("ItemTypeID").ToString)
            txtRangefrom.Text = .Item("RANGEFROM").ToString
            PFromwt = Val(.Item("RANGEFROM").ToString)
            txtRangeto.Text = .Item("RANGETO").ToString
            PTowt = Val(.Item("RANGETO").ToString)
            txt_DustWastPer.Text = .Item("DUSTWTPER").ToString
            txt_DustWeight.Text = .Item("DUSTWT").ToString
            txtMelt_Per.Text = .Item("MELTPER").ToString
            txtMelt_WET.Text = .Item("MELTWT").ToString
            txtWastPerCash.Text = .Item("CASHWASTPER").ToString
            txtWastPerEx.Text = .Item("PURCHWASTPER").ToString
            txtWastPerCash_oth.Text = .Item("CASHWASTPER_OTH").ToString
            txtWastPerEx_oth.Text = .Item("PURCHWASTPER_OTH").ToString

        End With
        flagSave = True
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function

    Private Sub frmPurItemType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
                cmbItemtypeid.Focus()
            End If
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        funcOpen()
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

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        funcOpen()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString, gridView.Item(1, gridView.CurrentRow.Index).Value.ToString, gridView.Item(2, gridView.CurrentRow.Index).Value.ToString, gridView.Item(3, gridView.CurrentRow.Index).Value.ToString)
                tabMain.SelectedTab = tabGeneral
                cmbItemtypeid.Focus()
            End If
        End If
    End Sub


    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub

 

    Private Sub cmbItemtypeid_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemtypeid.LostFocus
        If flagSave = False Then
            strSql = "SELECT MAX(ISNULL(RANGETO,0)) FROM " & cnAdminDb & "..PURITEMTYPE WHERE ITEMTYPEID='" & Val(cmbItemtypeid.Text) & "' "
            Dim rangefrom As Decimal = 0
            Dim value As String = IIf(IsDBNull(GetSqlValue(cn, strSql)), "", GetSqlValue(cn, strSql))
            rangefrom = Val(value) + 0.1
            txtRangefrom.Text = rangefrom
        End If
        
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If gridView.RowCount > 0 Then
            With gridView
                strSql = "DELETE FROM " & cnAdminDb & "..PURITEMTYPE WHERE ITEMTYPEID=" & .Item("ITEMTYPEID", .CurrentRow.Index).Value.ToString
                strSql += vbCrLf + " AND RANGEFROM ='" & .Item("RANGEFROM", .CurrentRow.Index).Value.ToString & "'"
                strSql += vbCrLf + " AND RANGETO ='" & .Item("RANGETO", .CurrentRow.Index).Value.ToString & "'"
            End With
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            MsgBox("Deleted..", MsgBoxStyle.Information)
            funcOpen()
        End If
    End Sub
End Class