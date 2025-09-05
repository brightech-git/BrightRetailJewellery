Imports System.Data.OleDb
Public Class CatalogStoneDia
    Public dtGridStone As New DataTable
    Dim strSql As String

    Public grsWt As Double = Nothing
    Public _DiaPcs As Integer = Nothing
    Public _DiaWt As Double = Nothing
    Public _EditLock As Boolean = False
    Public _DelLock As Boolean = False
    Public _Authorize As Boolean = False
    Dim StudWtDedut As String = GetAdmindbSoftValue("STUDWTDEDUCT", "DSP")

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        Me.BackColor = GlobalVariables.frmBackColor
        grsWt = Nothing
        gridStone.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridStoneTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridStoneTotal.DefaultCellStyle.BackColor = grpStone.BackgroundColor
        gridStoneTotal.DefaultCellStyle.SelectionBackColor = grpStone.BackgroundColor

        ''Stone Group
        cmbStUnit.Items.Add("C")
        cmbStUnit.Items.Add("G")
        cmbStUnit.Text = "C"
        cmbStCalc.Items.Add("W")
        cmbStCalc.Items.Add("P")
        cmbStCalc.Text = "W"

        ''Stone
        With dtGridStone.Columns
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("PCS", GetType(Int32))
            .Add("WEIGHT", GetType(Decimal))
            .Add("SIZE", GetType(String))
            .Add("UNIT", GetType(String))
            .Add("CALC", GetType(String))
        End With
        With gridStone
            .DataSource = dtGridStone
            For i As Integer = 0 To .ColumnCount - 1
                If .Columns(i).ValueType.Name Is GetType(Decimal).Name Then
                    .Columns(i).DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Double).Name Then
                    .Columns(i).DefaultCellStyle.Format = "0.00"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Integer).Name Then
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
                .Columns(i).Resizable = DataGridViewTriState.False
            Next
        End With
        StyleGridStone(gridStone)

        Dim dtGridStoneTotal As New DataTable
        dtGridStoneTotal = dtGridStone.Copy
        dtGridStoneTotal.Rows.Clear()
        dtGridStoneTotal.Rows.Add()
        dtGridStoneTotal.Rows(0).Item("ITEM") = "Total"
        dtGridStoneTotal.AcceptChanges()
        With gridStoneTotal
            .DataSource = dtGridStoneTotal
            For Each col As DataGridViewColumn In gridStone.Columns
                With gridStoneTotal.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
        End With
        CalcStoneWtAmount()
        StyleGridStone(gridStoneTotal)
    End Sub
    Public Sub CalcStoneWtAmount()
        Dim diaCaratWt As Double = 0
        Dim diaGramWt As Double = 0
        Dim diaPcs As Integer = 0
        Dim diaAmt As Double = 0

        Dim preCaratWt As Double = 0
        Dim preGramWt As Double = 0
        Dim prePcs As Integer = 0
        Dim preAmt As Double = 0

        Dim stoCaratWt As Double = 0
        Dim stoGramWt As Double = 0
        Dim stoPcs As Integer = 0
        Dim stoAmt As Double = 0


        _DiaPcs = 0
        _DiaWt = 0

        For cnt As Integer = 0 To gridStone.RowCount - 1
            With gridStone.Rows(cnt)
                strSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'"
                Dim METALID As String = objGPack.GetSqlValue(strSql).ToUpper
                Select Case METALID  '.Cells("METALID").Value.ToString
                    Case "D"
                        diaPcs += Val(.Cells("PCS").Value.ToString)
                        _DiaPcs += Val(.Cells("PCS").Value.ToString)
                    Case "S"
                        stoPcs += Val(.Cells("PCS").Value.ToString)
                    Case "P"
                        prePcs += Val(.Cells("PCS").Value.ToString)
                End Select
                Select Case .Cells("UNIT").Value.ToString
                    Case "G"
                        If METALID = "S" Then
                            stoGramWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf METALID = "P" Then
                            preGramWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf METALID = "D" Then
                            diaGramWt += Val(.Cells("WEIGHT").Value.ToString)
                            _DiaWt += Val(.Cells("WEIGHT").Value.ToString)
                        End If
                    Case "C"
                        If METALID = "S" Then
                            stoCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf METALID = "P" Then
                            preCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf METALID = "D" Then
                            diaCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                            _DiaWt += Val(.Cells("WEIGHT").Value.ToString)
                        End If
                End Select
            End With
        Next

        Dim lessWt As Double = Nothing '(diaCaratWt + stoCaratWt + preCaratWt) / 5 + (diaGramWt + stoGramWt + preGramWt)

        If StudWtDedut.Contains("D") Then
            lessWt += (diaCaratWt / 5) + diaGramWt
        End If
        If StudWtDedut.Contains("S") Then
            lessWt += (stoCaratWt / 5) + stoGramWt
        End If
        If StudWtDedut.Contains("P") Then
            lessWt += (preCaratWt / 5) + preGramWt
        End If
        If StudWtDedut.Contains("N") Then
            lessWt = 0
        End If


        Dim stnAmt As Double = diaAmt + stoAmt + preAmt
        If gridStoneTotal.RowCount > 0 Then
            gridStoneTotal.Rows(0).Cells("WEIGHT").Value = IIf(lessWt <> 0, lessWt, DBNull.Value)
        End If
    End Sub
    Public Sub StyleGridStone(ByVal gridStone As DataGridView)
        With gridStone
            .Columns("ITEM").Width = txtStItem.Width + 1
            .Columns("SUBITEM").Width = txtStSubItem.Width + 1
            .Columns("PCS").Width = txtStPcs_NUM.Width + 1
            .Columns("WEIGHT").Width = txtStWeight.Width + 1
            .Columns("SIZE").Width = txtSize.Width + 1
            .Columns("UNIT").Width = cmbStUnit.Width + 1
            .Columns("CALC").Width = cmbStCalc.Width + 1
        End With
    End Sub

    Private Sub CatalogStoneDia_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            dtGridStone.AcceptChanges()
            txtStItem.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub CatalogStoneDia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridStone.Focused Then Exit Sub
            If txtStItem.Focused Then Exit Sub
            If txtStSubItem.Focused Then Exit Sub
            If cmbStCalc.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtStPcs_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStPcs_NUM.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridStone.RowCount > 0 Then gridStone.Select()
        End If
    End Sub

    Private Sub txtStWeight_WET_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStWeight.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridStone.RowCount > 0 Then gridStone.Select()
        End If
    End Sub

    Private Sub txtStWeight_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStWeight.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            CheckStoneWeight()
        Else
            If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'") = "D" And diaRnd = 4 Then
                WeightValidation(txtStWeight, e, diaRnd)
            Else
                WeightValidation(txtStWeight, e)
            End If
        End If
    End Sub

    Private Sub txtStWeight_WET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStWeight.LostFocus
        cmbStCalc.Text = IIf(Val(txtStWeight.Text) <> 0, "W", "P")
        If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'") = "D" Then
            txtStWeight.Text = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), txtStWeight.Text)
        Else
            txtStWeight.Text = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), "0.000"), txtStWeight.Text)
        End If
    End Sub

    Private Sub txtStItem_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStItem.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtStItem_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStItem.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtStItem_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStItem.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadStoneItemName()
        ElseIf e.KeyCode = Keys.Down Then
            If gridStone.RowCount > 0 Then
                gridStone.Focus()
            End If
        End If
    End Sub

    Private Sub txtStItem_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStItem.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtStItem.Text = "" Then
                LoadStoneItemName()
            ElseIf txtStItem.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'") = False Then
                LoadStoneItemName()
            Else
                LoadStoneitemDetails()
            End If
        End If
    End Sub

    Private Sub txtStSubItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStSubItem.GotFocus
        'If objGPack.DupCheck("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem_MAN.Text & "')") = False Then
        SendKeys.Send("{TAB}")
        'End If
    End Sub
    Private Sub LoadStoneItemName()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') = 'S' AND ACTIVE = 'Y'"
        Dim itemName As String = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, 1, , txtStItem.Text)
        If itemName <> "" Then
            txtStItem.Text = itemName
            LoadStoneitemDetails()
        Else
            txtStItem.Focus()
            txtStItem.SelectAll()
        End If
    End Sub

    Private Sub LoadStoneitemDetails()
        'txtStSubItem.Clear()
        'txtStPcs_NUM.Clear()
        'txtStWeight.Clear()
        'txtStRate_AMT.Clear()
        'txtStAmount_AMT.Clear()
        If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'") = "Y" Then
            Dim DefItem As String = txtStSubItem.Text
            Dim iId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"))
            If Not _SubItemOrderByName Then
                DefItem = objGPack.GetSqlValue("SELECT DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = " & iId & "")
            End If
            strSql = GetSubItemQry(New String() {"SUBITEMID ID", "DISPLAYORDER DISP", "SUBITEMNAME SUBITEM"}, iId)
            txtStSubItem.Text = BrighttechPack.SearchDialog.Show("Search SubItem", strSql, cn, IIf(_SubItemOrderByName, 2, 1), 2, , DefItem, , False, True)
            'Dim qry As String = "SELECT SUBITEMID ID,SUBITEMNAME SUBITEM FROM " & cnAdminDb & "..SUBITEMMAST "
            'qry += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            'qry += " AND ACTIVE = 'Y'"
            'txtStSubItem.Text = BrighttechPack.SearchDialog.Show("Search SubItem", qry, cn, 1, 1, , txtStSubItem.Text, , False, True)
        Else
            txtStSubItem.Clear()
        End If

        If txtStSubItem.Text <> "" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
        Else
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"
        End If
        Dim calType As String = objGPack.GetSqlValue(strSql, , , tran)
        cmbStCalc.Text = IIf(calType = "R", "P", "W")

        If txtStSubItem.Text <> "" Then
            strSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..SUBITEMMAST  WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
        Else
            strSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"
        End If
        cmbStUnit.Text = objGPack.GetSqlValue(strSql, , , tran)
        Me.SelectNextControl(txtStItem, True, True, True, True)
    End Sub


    Private Sub gridStone_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridStone.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridStone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridStone.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not gridStone.RowCount > 0 Then Exit Sub
            If _EditLock = True Then Exit Sub
            gridStone.CurrentCell = gridStone.CurrentRow.Cells("ITEM")
            With gridStone.Rows(gridStone.CurrentRow.Index)
                txtStItem.Text = .Cells("ITEM").FormattedValue
                txtStSubItem.Text = .Cells("SUBITEM").FormattedValue
                txtStPcs_NUM.Text = .Cells("PCS").FormattedValue
                txtStWeight.Text = .Cells("WEIGHT").FormattedValue
                cmbStUnit.Text = .Cells("UNIT").FormattedValue
                cmbStCalc.Text = .Cells("CALC").FormattedValue
                txtSize.Text = .Cells("SIZE").Value.ToString
                txtStRowIndex.Text = gridStone.CurrentRow.Index
                txtStPcs_NUM.Focus()
            End With
        End If
    End Sub

    Private Sub gridStone_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridStone.UserDeletedRow
        dtGridStone.AcceptChanges()
        CalcStoneWtAmount()
        If Not gridStone.RowCount > 0 Then txtStItem.Focus()
    End Sub

    Private Sub CatalogStoneDia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridStone.AllowUserToDeleteRows = Not _DelLock
        gridStone.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridStoneTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridStoneTotal.DefaultCellStyle.BackColor = grpStone.BackgroundColor
        gridStoneTotal.DefaultCellStyle.SelectionBackColor = grpStone.BackgroundColor
        CalcStoneWtAmount()
    End Sub

    Private Function CheckStoneWeight() As Boolean
        Dim stWeight As Double = IIf(cmbStUnit.Text = "C", Val(txtStWeight.Text) / 5, Val(txtStWeight.Text))
        For cnt As Integer = 0 To gridStone.RowCount - 1
            If txtStRowIndex.Text <> "" Then If Val(txtStRowIndex.Text) = cnt Then Continue For
            With gridStone.Rows(cnt)
                If .Cells("UNIT").Value.ToString = "C" Then
                    stWeight += Val(.Cells("WEIGHT").Value.ToString) / 5
                ElseIf .Cells("UNIT").Value.ToString = "G" Then
                    stWeight += Val(.Cells("WEIGHT").Value.ToString)
                End If
            End With
        Next
        If stWeight > grsWt Then
            MsgBox("Stone Weight should not exeed Grsweight", MsgBoxStyle.Information)
            txtStWeight.Focus()
            Return False
        End If
        Return True
    End Function

    Private Sub cmbStCalc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbStCalc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''Validation
            'If GetCalcType() = "R" Then Exit Sub
            'If isTag Then
            '    If Not lckPartlySale Then Exit Sub
            'End If
            ''Validation
            If objGPack.Validator_Check(grpStone) Then Exit Sub
            If objGPack.DupCheck("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "' AND  STUDDED = 'S' AND ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Item", MsgBoxStyle.Information)
                txtStItem.Focus()
                Exit Sub
            End If
            If UCase(objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "' AND STUDDED = 'S' AND ACTIVE = 'Y'")) = "Y" Then
                If txtStSubItem.Text = "" Then
                    MsgBox("SubItem Should Not Empty", MsgBoxStyle.Information)
                    txtStSubItem.Focus()
                    Exit Sub
                End If
                If objGPack.DupCheck("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "') AND SUBITEMNAME = '" & txtStSubItem.Text & "'") = False Then
                    MsgBox("Invalid SubItem", MsgBoxStyle.Information)
                    txtStSubItem.Focus()
                    Exit Sub
                End If
            Else
                txtStSubItem.Clear()
            End If
            If Val(txtStPcs_NUM.Text) = 0 And Val(txtStWeight.Text) = 0 Then
                MsgBox(Me.GetNextControl(txtStPcs_NUM, False).Text + "," + Me.GetNextControl(txtStWeight, False).Text + E0001, MsgBoxStyle.Information)
                txtStItem.Focus()
                Exit Sub
            End If
            Dim cent As Double = Nothing
            If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))) * 100 Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))
            If Not CheckStoneWeight() Then Exit Sub
            If txtStRowIndex.Text = "" Then
                ''Insertion
                Dim ro As DataRow = Nothing
                ro = dtGridStone.NewRow
                ro("ITEM") = txtStItem.Text
                ro("SUBITEM") = txtStSubItem.Text
                ro("PCS") = IIf(Val(txtStPcs_NUM.Text) <> 0, Val(txtStPcs_NUM.Text), DBNull.Value)
                ro("UNIT") = cmbStUnit.Text
                ro("CALC") = cmbStCalc.Text
                ro("WEIGHT") = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), DBNull.Value)
                ro("SIZE") = txtSize.Text
                dtGridStone.Rows.Add(ro)
            Else
                With gridStone.Rows(Val(txtStRowIndex.Text))
                    .Cells("ITEM").Value = txtStItem.Text
                    .Cells("SUBITEM").Value = txtStSubItem.Text
                    .Cells("PCS").Value = IIf(Val(txtStPcs_NUM.Text) <> 0, Val(txtStPcs_NUM.Text), DBNull.Value)
                    .Cells("UNIT").Value = cmbStUnit.Text
                    .Cells("CALC").Value = cmbStCalc.Text
                    .Cells("WEIGHT").Value = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), DBNull.Value)
                    .Cells("SIZE").Value = txtSize.Text
                End With
            End If
            dtGridStone.AcceptChanges()
            gridStone.CurrentCell = gridStone.Rows(gridStone.RowCount - 1).Cells("ITEM")

            CalcStoneWtAmount()
            StyleGridStone(gridStone)
            StyleGridStone(gridStoneTotal)

            ''CLEAR
            'cmbStItem_Man.Text = ""
            'cmbStSubItem_Man.Text = ""
            objGPack.TextClear(grpStone)
            'txtStPcs_NUM.Clear()
            'txtStWeight_WET.Clear()
            'txtStRate_AMT.Clear()
            'txtStAmount_AMT.Clear()
            'txtStMetalCode.Clear()
            txtStItem.Focus()
        End If
    End Sub

    Private Sub txtSize_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSize.GotFocus
        Dim wt As Double = Val(txtStWeight.Text)
        If Val(txtStPcs_NUM.Text) > 0 Then
            wt = wt / Val(txtStPcs_NUM.Text)
        End If
        wt *= 100
        strSql = " SELECT SIZEDESC FROM " & cnAdminDb & "..CENTSIZE AS CZ"
        strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
        strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')),0)"
        strSql += " AND " & wt & " BETWEEN FROMCENT AND TOCENT"
        txtSize.Text = objGPack.GetSqlValue(strSql)
        SendKeys.Send("{TAB}")
    End Sub
End Class