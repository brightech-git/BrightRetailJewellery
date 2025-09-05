Imports System.Data.OleDb
Public Class frmRequestionSlip
#Region "Variable Declaration"
    Dim strsql As String = String.Empty
    Dim title As String = String.Empty
    Dim dtOptions As New DataTable
    Dim dtitName As New DataTable
    Dim til As String = String.Empty
    Dim dtGridView As New DataTable
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim costId As String = Nothing
    Dim lastData As Object
#End Region
#Region "Common Function"
    Private Sub FuncNew()
        strsql = "   SELECT 'ALL'   ITEMCTRNAME , 'ALL',1 RESULT"
        strsql += "  UNION ALL SELECT ITEMCTRNAME,CONVERT(VARCHAR,ITEMCTRID),2 RESULT FROM"
        strsql += " " & cnAdminDb & "..ITEMCOUNTER ORDER BY RESULT,ITEMCTRNAME "
        dtOptions = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtOptions)
        BrighttechPack.GlobalMethods.FillCombo(cmbCountername, dtOptions, "ITEMCTRNAME", )
        cmbCountername.SelectedText = "ALL"
    End Sub
#End Region
    Private Sub itemName()
        strsql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strsql += " UNION ALL"
        strsql += " SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strsql += " ORDER BY RESULT,ITEMNAME"
        dtitName = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtitName)
        BrighttechPack.GlobalMethods.FillCombo(chkItemName, dtitName, "ITEMNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId Then chkItemName.Enabled = False
    End Sub
    Private Sub frmDesign_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.F3
                btnNew_Click(btnNew, Nothing)
                Exit Select
            Case Keys.F12
                btnExit_Click(btnExit, Nothing)
                Exit Select
            Case Keys.S
                btnSearch_Click(btnSearch, Nothing)
                Exit Select
        End Select
    End Sub
    Private Sub frmDesign_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmDesign_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call itemName()
        Call FuncNew()
        btnNew_Click(Me, Nothing)
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Val(Me.txtFrom_NUM.Text) = 0 Then
            MsgBox("Must be Enter Weight From.", MsgBoxStyle.Information)
            Me.txtFrom_NUM.Focus()
            Exit Sub
        End If
        If Val(Me.txtTo_num.Text) = 0 Then
            MsgBox("Must be Enter Weight To.", MsgBoxStyle.Information)
            Me.txtTo_num.Focus()
            Exit Sub
        End If
        If Val(Me.txtRange_num.Text) = 0 Then
            MsgBox("Must be Enter Weight Range.", MsgBoxStyle.Information)
            Me.txtRange_num.Focus()
            Exit Sub
        End If
        If Val(Me.txtFrom_NUM.Text) = Val(Me.txtTo_num.Text) Then
            MsgBox("From Weight and To Weight are Same.", MsgBoxStyle.Information)
            Me.txtTo_num.Focus()
            Exit Sub
        End If
        If Val(Me.txtFrom_NUM.Text) > Val(Me.txtTo_num.Text) Then
            MsgBox("To weight must greater than From weight.", MsgBoxStyle.Information)
            Me.txtFrom_NUM.Focus()
            Exit Sub
        End If
        Dim r1 As Integer = Val(txtFrom_NUM.Text)
        Dim r2 As Integer = Val(txtTo_num.Text)
        Dim r3 As Integer = Val(txtRange_num.Text)
        strsql = String.Empty
        dGrid.DataSource = Nothing
        Dim ItemName As String = GetQryStringForSp(chkItemName.Text, cnAdminDb & "..ITEMMAST", "ITEMID", "ITEMNAME", False)
        strsql = "  IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "REQUIREMENT') IS NOT NULL DROP TABLE  TEMPTABLEDB..TEMP" & systemId & "REQUIREMENT"
        strsql += "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "REQUIREMENT(PARTICULAR VARCHAR(500),COUNTERNAME VARCHAR(50),"
        strsql += " ITEMNAME VARCHAR(500),DESIGNERCODE VARCHAR(50),SIZE VARCHAR(30),PARIS VARCHAR(30),GRAM VARCHAR(30)"
        For i As Integer = r1 To r2 Step r3
            strsql += ",[" & i & "GMS] DECIMAL(15,3)"
        Next
        strsql += " ,RESULT INT,COLHEAD VARCHAR(1),TOTAL DECIMAL(18,3))"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "REQUIREMENT"
        strsql += " (COUNTERNAME,ITEMNAME,SIZE,GRAM,RESULT,COLHEAD)"

        If cmbCountername.Text <> "ALL" Then
            strsql += " SELECT DISTINCT '" & Me.cmbCountername.Text & "',"
        Else
            strsql += " SELECT DISTINCT IC.ITEMCTRNAME,"
        End If
        strsql += " IM.ITEMNAME,IE.SIZENAME,'" & r1 & " - " & r2 & "',2 RESULT,'I' COLHEAD"
        strsql += " FROM " & cnAdminDb & "..ITEMMAST AS IM "
        strsql += " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER AS IC ON IM.ITEMID = IC.ITEMCTRID "
        strsql += " LEFT JOIN " & cnAdminDb & "..ITEMSIZE AS IE ON IM.ITEMID = IE.ITEMID"
        If Me.chkItemName.Text <> "ALL" And Me.cmbCountername.Text <> "ALL" Then
            strsql += " WHERE IC.ITEMCTRNAME ='" & Me.cmbCountername.Text & "'"
            strsql += " AND IM.ITEMID IN('" & ItemName.Replace(",", "','") & "')"
            strsql += " AND IM.ITEMNAME IS NOT NULL AND IC.ITEMCTRNAME IS NOT NULL"
        ElseIf Me.cmbCountername.Text <> "ALL" Then
            strsql += " WHERE IC.ITEMCTRNAME ='" & Me.cmbCountername.Text & "'"
            strsql += " AND IM.ITEMNAME IS NOT NULL AND IC.ITEMCTRNAME IS NOT NULL"
        ElseIf Me.chkItemName.Text <> "ALL" Then
            strsql += " WHERE IM.ITEMID IN('" & ItemName.Replace(",", "','") & "')"
            strsql += " AND IM.ITEMNAME IS NOT NULL AND IC.ITEMCTRNAME IS NOT NULL"
        Else
            strsql += " WHERE IM.ITEMNAME IS NOT NULL AND IC.ITEMCTRNAME IS NOT NULL"
        End If
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "REQUIREMENT(PARTICULAR,COUNTERNAME,RESULT,COLHEAD)"
        strsql += " SELECT DISTINCT COUNTERNAME,COUNTERNAME,1 RESULT,'G' "
        strsql += "  COLHEAD FROM  TEMPTABLEDB..TEMP" & systemId & "REQUIREMENT"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = " SELECT DISTINCT * FROM TEMPTABLEDB..TEMP" & systemId & "REQUIREMENT ORDER BY COUNTERNAME,RESULT"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        Dim ds As New DataSet
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(ds)
        If ds.Tables(0).Rows.Count > 0 Then
            dGrid.DataSource = ds.Tables(0)
            GridColor()
        End If
    End Sub
    Private Sub GridColor()
        With dGrid
            dGrid.Columns("PARTICULAR").Frozen = True
            dGrid.Columns("ITEMNAME").Frozen = True
            dGrid.Columns("RESULT").Visible = False
            dGrid.Columns("COLHEAD").Visible = False
            dGrid.Columns("PARTICULAR").Width = 130
            dGrid.Columns("COUNTERNAME").Visible = False
            dGrid.Columns("DESIGNERCODE").Width = 100
            dGrid.Columns("ITEMNAME").Width = 150
            dGrid.Columns("SIZE").Width = 50
            dGrid.Columns("PARTICULAR").ReadOnly = True
            dGrid.Columns("GRAM").ReadOnly = True
            For Each dgvRow As DataGridViewRow In dGrid.Rows
                Select Case dgvRow.Cells("COLHEAD").Value.ToString
                    Case "G"
                        dgvRow.DefaultCellStyle.BackColor = Color.Yellow
                        dgvRow.DefaultCellStyle.ForeColor = Color.Black
                        dgvRow.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                        'dgvRow.Frozen = True
                        dgvRow.ReadOnly = True
                End Select
            Next
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
        dGrid.Focus()
        Me.dGrid.Refresh()
    End Sub
    Private Sub dGrid_EditingControlShowing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles dGrid.EditingControlShowing
        For i As Integer = 0 To dGrid.Columns.Count - 1
            AddHandler CType(e.Control, TextBox).KeyPress, AddressOf txtAmt_KeyPress
        Next
    End Sub
    Private Sub TBox_keyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> "."c Then
            e.Handled = True
        End If
        If e.KeyChar = "."c AndAlso TryCast(sender, TextBox).Text.IndexOf("."c) > -1 Then
            e.Handled = True
        End If
    End Sub
    Private Sub txtAmt_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If e.KeyChar = "-" Then
            e.Handled = True
            Exit Sub
        End If
        AmountValidation(sender, e)
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        With Me
            .dGrid.DataSource = Nothing
            .txtFrom_NUM.Text = String.Empty
            .txtRange_num.Text = String.Empty
            .txtTo_num.Text = String.Empty
        End With
        cmbCountername.Focus()
    End Sub
    Private Sub txtTo_num_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTo_num.Leave
        If Val(Me.txtFrom_NUM.Text) > Val(Me.txtTo_num.Text) Then
            MsgBox("To Weight must be greater than From weight." & Environment.NewLine & " for Ex.From Weight : 10 and To Weight : 20", MsgBoxStyle.Information, "Alert")
            Me.txtTo_num.Focus()
            Exit Sub
        End If
    End Sub
End Class