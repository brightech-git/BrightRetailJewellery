Imports System.Data.OleDb
Public Class frmStoneDiaAc
    Public Enum Material
        Issue = 0
        Receipt = 1
    End Enum
    Public dtGridStone As New DataTable
    Dim strSql As String
    Dim cmd As New OleDbCommand
    Public oMaterial As Material
    Public _Accode As String = Nothing
    Public grsWt As Double = Nothing
    Public _DiaPcs As Integer = Nothing
    Public _DiaWt As Double = Nothing
    Public _EditLock As Boolean = False
    Public _DelLock As Boolean = False
    Public _Authorize As Boolean = False
    Public IssRecCat As Boolean = False
    Public IssRecStudWtDedut As Boolean = False
    Dim StudWtDedut As String
    Public PacketNo As String = Nothing
    Public PacketItemId As Integer = Nothing
    Public FromFlag As String = Nothing
    Public SHOWTRANNO As Boolean = False
    Dim objMultiSelect As MultiSelectRowDia = Nothing
    Dim dtGrid As New DataTable
    Dim rateRevcal As String
    Dim _JobNoEnable As Boolean = IIf(GetAdmindbSoftValue("MRMIJOBNO", "N") = "Y", True, False)
    Dim StnRowEDit As Boolean = IIf(GetAdmindbSoftValue("ACC_STONEEDIT", "Y") = "Y", True, False)
    Dim STOCKVALIDATION As Boolean = IIf(GetAdmindbSoftValue("MRMISTOCKLOCK", "N") = "Y", True, False)
    Dim ACC_STUDITEM_POPUP As Boolean = IIf(GetAdmindbSoftValue("ACC_STUDITEM_POPUP", "Y") = "Y", True, False)
    Dim _FourCMaintain As Boolean = IIf(GetAdmindbSoftValue("4CMAINTAIN", "Y") = "Y", True, False)
    Dim ORDER_MULTI_MIMR As Boolean = IIf(GetAdmindbSoftValue("ORDER_MULTI_MIMR", "N") = "Y", True, False)
    Dim ObjDiaDetails As New frmDiamondDetails
    Dim DtStnGrp As New DataTable
    Dim MIMR_USER_RESTRICT As String = GetAdmindbSoftValue("MIMR_USER_RESTRICT", "")
    Public MIMRSTONETYPE As String

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        If FromFlag = "A" Then
            ''Accounts Entry
            StudWtDedut = GetAdmindbSoftValue("STUDWTDEDUCT_ACC", "DSP")
        Else
            StudWtDedut = GetAdmindbSoftValue("STUDWTDEDUCT", "DSP")
        End If
        rateRevcal = GetAdmindbSoftValue("STNRATEREVCAL", "N")

        txtTranno_OWN.Visible = SHOWTRANNO

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        Me.BackColor = frmBackColor
        objGPack.TextClear(Me)
        grsWt = Nothing
        gridStone.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridStoneTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridStoneTotal.DefaultCellStyle.BackColor = grpStone.BackgroundColor
        gridStoneTotal.DefaultCellStyle.SelectionBackColor = grpStone.BackgroundColor

        strSql = "SELECT CATNAME,CATCODE FROM " & cnAdminDb & "..CATEGORY "
        strSql += " WHERE METALID in('P','T','D')"
        strSql += " ORDER BY DISPLAYORDER,CATNAME"
        Dim dtTemp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If Not dtTemp.Rows.Count > 0 Then
            cmbIssRecCat.Enabled = False
            cmbIssRecCat.Text = ""
        Else
            cmbIssRecCat.Enabled = True
        End If
        cmbIssRecCat.DataSource = dtTemp
        cmbIssRecCat.ValueMember = "CATCODE"
        cmbIssRecCat.DisplayMember = "CATNAME"

        If ACC_STUDITEM_POPUP Then
            cmbItem.Visible = False
            cmbSubItem.Visible = False
            cmbItem.Enabled = False
            cmbSubItem.Enabled = False
            txtStItem.Enabled = True
            txtStSubItem.Enabled = True
            txtStItem.Visible = True
            txtStSubItem.Visible = True
        Else
            LoadItem(cmbItem)
            LoadSubItem(cmbSubItem)
            cmbItem.Visible = True
            cmbSubItem.Visible = True
            cmbItem.Enabled = True
            cmbSubItem.Enabled = True
            txtStItem.Enabled = False
            txtStSubItem.Enabled = False
            txtStItem.Visible = False
            txtStSubItem.Visible = False
        End If

        ''Transaction Type

        If MIMR_USER_RESTRICT.ToString <> "" Then
            Dim _temptrantypeuser() As String = MIMR_USER_RESTRICT.ToString.Split(",")
            Dim _tempcontains As Boolean = False
            Dim _tempcontainstr As String = ""
            For cnt As Integer = 0 To _temptrantypeuser.Length - 1
                _tempcontainstr = ""
                _tempcontains = False
                If _temptrantypeuser(cnt).ToString.Contains(userId.ToString & ":") Then
                    _tempcontains = True
                    _tempcontainstr = _temptrantypeuser(cnt).ToString
                    Exit For
                End If
            Next
            If _tempcontains = True And _tempcontainstr.ToString <> "" Then
                If _tempcontainstr.ToString.Contains("IIS") Or _tempcontainstr.ToString.Contains("IPU") Or _tempcontainstr.ToString.Contains("IIN") Or _tempcontainstr.ToString.Contains("IOT") _
                   Or _tempcontainstr.ToString.Contains("IPA") Or _tempcontainstr.ToString.Contains("IDN") Then
                    CmbTrantype.Items.Add("ISSUE")
                    CmbTrantype.Items.Add("PURCHASE RETURN")
                ElseIf _tempcontainstr.ToString.Contains("RRE") _
                Or _tempcontainstr.ToString.Contains("RPU") Or _tempcontainstr.ToString.Contains("RIN") Or _tempcontainstr.ToString.Contains("ROT") _
                Or _tempcontainstr.ToString.Contains("RPA") Or _tempcontainstr.ToString.Contains("RDN") Then
                    CmbTrantype.Items.Add("RECEIPT")
                    CmbTrantype.Items.Add("PURCHASE")
                End If
                If _tempcontainstr.ToString.Contains("IAP") Or _tempcontainstr.ToString.Contains("RAP") Then CmbTrantype.Items.Add("APPROVAL")
                CmbTrantype.SelectedIndex = 0
            Else
                GoTo MoveType
            End If
        Else
MoveType:
            If oMaterial = Material.Issue Then
                CmbTrantype.Items.Add("ISSUE")
                CmbTrantype.Items.Add("PURCHASE RETURN")
                CmbTrantype.Items.Add("APPROVAL")
                CmbTrantype.Text = "ISSUE"

            Else
                CmbTrantype.Items.Add("RECEIPT")
                CmbTrantype.Items.Add("PURCHASE")
                CmbTrantype.Items.Add("APPROVAL")
                CmbTrantype.Text = "RECEIPT"
            End If
        End If

        ''Stone Group
        cmbStUnit.Items.Add("C")
        cmbStUnit.Items.Add("G")
        cmbStUnit.Text = "C"
        cmbStCalc.Items.Add("W")
        cmbStCalc.Items.Add("P")
        cmbStCalc.Text = "W"
        'Seive
        LoadSeiveSize(CmbSeive)
        ''Stone Deduct
        CmbStudDeduct.Items.Add("YES")
        CmbStudDeduct.Items.Add("NO")
        CmbStudDeduct.Text = "YES"
        ''Stone
        With dtGridStone.Columns
            .Add("KEYNO", GetType(Integer))
            .Add("TRANTYPE", GetType(String))
            .Add("STNTYPE", GetType(String))
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("OCATCODE", GetType(String))
            .Add("STUDDEDUCT", GetType(String))
            .Add("PCS", GetType(Int32))
            .Add("WEIGHT", GetType(Decimal))
            .Add("UNIT", GetType(String))
            .Add("CALC", GetType(String))
            .Add("RATE", GetType(Double))
            .Add("AMOUNT", GetType(Double))
            .Add("METALID", GetType(String))
            .Add("DISCOUNT", GetType(Double))
            .Add("TAGSTNPCS", GetType(Integer))
            .Add("TAGSTNWT", GetType(Decimal))
            .Add("TAGSNO", GetType(String))
            .Add("R_VAT", GetType(Decimal))
            .Add("ISSSNO", GetType(String))
            .Add("RESNO", GetType(String))
            .Add("SEIVE", GetType(String))
            .Add("CUTID", GetType(String))
            .Add("COLORID", GetType(String))
            .Add("CLARITYID", GetType(String))
            .Add("SETTYPEID", GetType(String))
            .Add("SHAPEID", GetType(String))
            .Add("HEIGHT", GetType(String))
            .Add("WIDTH", GetType(String))
            .Add("SNO", GetType(String))
            .Add("STNGRPID", GetType(String))
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
            .Columns("WEIGHT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
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
        StyleGridStone(gridStone)
        StyleGridStone(gridStoneTotal)
        CalcStoneWtAmount()
    End Sub
    Private Sub LoadItem(ByVal Cmb As ComboBox)
        strSql = " SELECT"
        strSql += " ITEMNAME AS ITEMNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') IN('S','B') AND ACTIVE = 'Y'"
        strSql += GetItemQryFilteration()
        objGPack.FillCombo(strSql, Cmb, True, False)
        Cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        Cmb.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Private Sub LoadSubItem(ByVal Cmb As ComboBox)
        If cmbItem.Text = "" Then Exit Sub
        Dim iId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'"))
        strSql = GetSubItemQry(New String() {"SUBITEMNAME SUBITEM"}, iId)
        objGPack.FillCombo(strSql, Cmb, True, False)
        If Cmb.Items.Count = 0 Then
            Cmb.Enabled = False
            Me.SelectNextControl(cmbItem, True, True, True, True)
        Else
            Cmb.Enabled = True
            Cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            Cmb.AutoCompleteSource = AutoCompleteSource.ListItems
        End If
    End Sub
    Private Sub LoadSeiveSize(ByVal Cmb As ComboBox)
        strSql = vbCrLf + " SELECT DISTINCT SIZEDESC FROM " & cnAdminDb & "..CENTSIZE ORDER BY SIZEDESC"
        objGPack.FillCombo(strSql, Cmb, True, False)
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
        Dim dirlesswt As Double = 0

        _DiaPcs = 0
        _DiaWt = 0
        If FromFlag = "A" Then
            ''Accounts Entry
            StudWtDedut = GetAdmindbSoftValue("STUDWTDEDUCT_ACC", "DSP")
        Else
            StudWtDedut = GetAdmindbSoftValue("STUDWTDEDUCT", "DSP")
        End If
        If gridStone.RowCount = 0 Then
            If gridStoneTotal.RowCount > 0 Then
                gridStoneTotal.Rows(0).Cells("WEIGHT").Value = DBNull.Value
                gridStoneTotal.Rows(0).Cells("AMOUNT").Value = DBNull.Value
            End If
            Exit Sub
        End If
        If IssRecStudWtDedut Then
            For cnt As Integer = 0 To gridStone.RowCount - 1
                With gridStone.Rows(cnt)
                    strSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'"
                    Dim METALID As String = objGPack.GetSqlValue(strSql).ToUpper
                    Select Case METALID
                        Case "D"
                            diaPcs += Val(.Cells("PCS").Value.ToString)
                            diaAmt += Val(.Cells("AMOUNT").Value.ToString)
                            _DiaPcs += Val(.Cells("PCS").Value.ToString)
                        Case "S"
                            stoPcs += Val(.Cells("PCS").Value.ToString)
                            stoAmt += Val(.Cells("AMOUNT").Value.ToString)
                        Case "P"
                            prePcs += Val(.Cells("PCS").Value.ToString)
                            preAmt += Val(.Cells("AMOUNT").Value.ToString)
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
                            ElseIf .Cells("STUDDEDUCT").Value.ToString = "YES" Then
                                dirlesswt += Val(.Cells("WEIGHT").Value.ToString)
                            End If
                        Case "C"
                            If METALID = "S" Then
                                stoCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf METALID = "P" Then
                                preCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf METALID = "D" Then
                                diaCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                                _DiaWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf .Cells("STUDDEDUCT").Value.ToString = "YES" Then
                                dirlesswt += (Val(.Cells("WEIGHT").Value.ToString) / 5)
                            End If
                    End Select
                End With
            Next
            Dim lessWt As Double = Nothing
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
            If dirlesswt <> 0 Then lessWt += dirlesswt
            Dim stnAmt As Double = diaAmt + stoAmt + preAmt
            If gridStoneTotal.RowCount > 0 Then
                gridStoneTotal.Rows(0).Cells("WEIGHT").Value = IIf(lessWt <> 0, lessWt, DBNull.Value)
                gridStoneTotal.Rows(0).Cells("AMOUNT").Value = IIf(stnAmt <> 0, stnAmt, DBNull.Value)
            End If
        Else
            Dim Isdeductstud As Boolean = False
            For cnt As Integer = 0 To gridStone.RowCount - 1
                With gridStone.Rows(cnt)
                    strSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'"
                    Dim METALID As String = objGPack.GetSqlValue(strSql).ToUpper
                    If StudWtDedut = "I" Then
                        If .Cells("SUBITEM").Value.ToString <> "" Then
                            strSql = " SELECT STUDDEDUCT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Cells("SUBITEM").Value.ToString & "'"
                        Else
                            strSql = " SELECT STUDDEDUCT  FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'"
                        End If
                        Isdeductstud = IIf(objGPack.GetSqlValue(strSql).ToString.ToUpper = "Y", True, False)
                    End If

                    Select Case METALID  '.Cells("METALID").Value.ToString
                        Case "D"
                            diaPcs += Val(.Cells("PCS").Value.ToString)
                            diaAmt += Val(.Cells("AMOUNT").Value.ToString)
                            _DiaPcs += Val(.Cells("PCS").Value.ToString)
                        Case "S"
                            stoPcs += Val(.Cells("PCS").Value.ToString)
                            stoAmt += Val(.Cells("AMOUNT").Value.ToString)
                        Case "P"
                            prePcs += Val(.Cells("PCS").Value.ToString)
                            preAmt += Val(.Cells("AMOUNT").Value.ToString)
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
                            If Isdeductstud Then dirlesswt += Val(.Cells("weight").Value.ToString)
                        Case "C"
                            If METALID = "S" Then
                                stoCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf METALID = "P" Then
                                preCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf METALID = "D" Then
                                diaCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                                _DiaWt += Val(.Cells("WEIGHT").Value.ToString)
                            End If
                            If Isdeductstud Then dirlesswt += (Val(.Cells("weight").Value.ToString) / 5)
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
            If dirlesswt <> 0 Then lessWt += dirlesswt

            Dim stnAmt As Double = diaAmt + stoAmt + preAmt
            If gridStoneTotal.RowCount > 0 Then
                gridStoneTotal.Rows(0).Cells("WEIGHT").Value = IIf(lessWt <> 0, lessWt, DBNull.Value)
                gridStoneTotal.Rows(0).Cells("AMOUNT").Value = IIf(stnAmt <> 0, stnAmt, DBNull.Value)
            End If
        End If
    End Sub
    Public Sub StyleGridStone(ByVal gridStone As DataGridView)
        With gridStone
            .Columns("KEYNO").Visible = False
            .Columns("TRANTYPE").Visible = False
            .Columns("METALID").Visible = False
            .Columns("ITEM").Width = txtStItem.Width + 1
            .Columns("STNTYPE").Width = CmbTrantype.Width + 1
            .Columns("SUBITEM").Width = txtStSubItem.Width + 1
            .Columns("OCATCODE").Width = cmbIssRecCat.Width + 1
            .Columns("STUDDEDUCT").Width = CmbStudDeduct.Width + 1
            .Columns("PCS").Width = txtStPcs_NUM.Width + 1
            .Columns("UNIT").Width = cmbStUnit.Width + 1
            .Columns("CALC").Width = cmbStCalc.Width + 1
            .Columns("WEIGHT").Width = txtStWeight.Width + 1
            .Columns("RATE").Width = txtStRate_AMT.Width + 1
            .Columns("AMOUNT").Width = txtStAmount_AMT.Width + 1
            .Columns("SEIVE").Width = CmbSeive.Width + 1
            .Columns("DISCOUNT").Visible = False
            .Columns("TAGSTNPCS").Visible = False
            .Columns("TAGSTNWT").Visible = False
            .Columns("TAGSNO").Visible = False
            .Columns("R_VAT").Visible = False
            .Columns("OCATCODE").Visible = IssRecCat
            .Columns("STUDDEDUCT").Visible = IssRecStudWtDedut
            If .Columns.Contains("ISSSNO") = True Then .Columns("ISSSNO").Visible = False
            If .Columns.Contains("RESNO") = True Then .Columns("RESNO").Visible = False
        End With
    End Sub

    Private Sub frmStoneDiaac_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            dtGridStone.AcceptChanges()
            If ACC_STUDITEM_POPUP Then txtStItem.Select() Else cmbItem.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmStoneDiaac_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridStone.Focused Then Exit Sub
            If txtStItem.Focused Then Exit Sub
            If txtStSubItem.Focused Then Exit Sub
            If cmbItem.Focused Then Exit Sub
            If cmbSubItem.Focused Then Exit Sub
            If txtStRate_AMT.Focused Then Exit Sub
            If txtStAmount_AMT.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtStAmount_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStAmount_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridStone.RowCount > 0 Then gridStone.Select()
        End If
    End Sub

    Private Sub txtStAmount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''Validation
            'If GetCalcType() = "R" Then Exit Sub
            'If isTag Then
            '    If Not lckPartlySale Then Exit Sub
            'End If
            ''Validation
            If objGPack.Validator_Check(grpStone) Then Exit Sub
            If objGPack.DupCheck("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbItem.Text) & "' AND  STUDDED IN('S','B') AND ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Item", MsgBoxStyle.Information)
                If ACC_STUDITEM_POPUP Then txtStItem.Focus() Else cmbItem.Focus()
                Exit Sub
            End If
            If UCase(objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbItem.Text) & "' AND STUDDED IN('S','B') AND ACTIVE = 'Y'")) = "Y" Then
                If IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, cmbSubItem.Text) = "" And IIf(ACC_STUDITEM_POPUP, txtStSubItem.Enabled, cmbSubItem.Enabled) Then
                    MsgBox("SubItem Should Not Empty", MsgBoxStyle.Information)
                    If ACC_STUDITEM_POPUP Then txtStSubItem.Focus() Else cmbSubItem.Focus()
                    Exit Sub
                End If
                If objGPack.DupCheck("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbItem.Text) & "') AND SUBITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, cmbSubItem.Text) & "'") = False And IIf(ACC_STUDITEM_POPUP, txtStSubItem.Enabled, cmbSubItem.Enabled) Then
                    MsgBox("Invalid SubItem", MsgBoxStyle.Information)
                    If ACC_STUDITEM_POPUP Then txtStSubItem.Focus() Else cmbSubItem.Focus()
                    Exit Sub
                End If
            Else
                If ACC_STUDITEM_POPUP Then txtStSubItem.Clear()
            End If
            If Val(txtStPcs_NUM.Text) = 0 And Val(txtStWeight.Text) = 0 And Val(txtStAmount_AMT.Text) = 0 Then
                MsgBox(Me.GetNextControl(txtStPcs_NUM, False).Text + "," + Me.GetNextControl(txtStWeight, False).Text + E0001, MsgBoxStyle.Information)
                If ACC_STUDITEM_POPUP Then txtStItem.Focus() Else cmbItem.Focus()
                Exit Sub
            End If
            Dim cent As Double = Nothing
            If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))) Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))
            cent *= 100
            strSql = " DECLARE @CENT FLOAT"
            strSql += " SET @CENT = " & cent & ""
            strSql += " SELECT PURRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            strSql += " @CENT BETWEEN FROMCENT AND TOCENT "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbItem.Text) & "')"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, cmbSubItem.Text) & "' "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbItem.Text) & "')),0)"
            strSql += " AND ISNULL(ACCODE,'') = '" & _Accode & "'"
            Dim rate As Double = Val(objGPack.GetSqlValue(strSql, , , tran))
            If Val(txtStRate_AMT.Text) < rate Then
                'If _Authorize Then
                If Ogstnrateedit = True Then
                    If MessageBox.Show(Me.GetNextControl(txtStRate_AMT, False).Text + E0020 + rate.ToString + vbCrLf + "Do you wish to Continue?", "Authorize Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                        txtStRate_AMT.Focus()
                        Exit Sub
                    End If
                Else
                    MsgBox(Me.GetNextControl(txtStRate_AMT, False).Text + E0020 + rate.ToString)
                    txtStRate_AMT.Focus()
                    Exit Sub
                End If
            End If
            If Not CheckStoneWeight() Then Exit Sub
            If rateRevcal = "Y" Then
                If Val(txtStRate_AMT.Text) = 0 And Val(txtStAmount_AMT.Text) <> 0 Then
                    If cmbStCalc.Text = "P" Then txtStRate_AMT.Text = Val(txtStAmount_AMT.Text) / Val(txtStPcs_NUM.Text)
                    If cmbStCalc.Text = "W" Then txtStRate_AMT.Text = Val(txtStAmount_AMT.Text) / Val(txtStWeight.Text)
                End If
            End If
            Dim jobno As String
            If txtStRowIndex.Text = "" Then
                ''Insertion
                Dim ro As DataRow = Nothing
                ro = dtGridStone.NewRow
                If ACC_STUDITEM_POPUP Then
                    ro("ITEM") = txtStItem.Text
                    ro("SUBITEM") = txtStSubItem.Text
                Else
                    ro("ITEM") = cmbItem.Text
                    ro("SUBITEM") = cmbSubItem.Text
                End If
                ro("PCS") = IIf(Val(txtStPcs_NUM.Text) <> 0, Val(txtStPcs_NUM.Text), DBNull.Value)
                ro("UNIT") = cmbStUnit.Text
                ro("CALC") = cmbStCalc.Text
                ro("WEIGHT") = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), DBNull.Value)
                ro("RATE") = IIf(Val(txtStRate_AMT.Text) <> 0, Format(Val(txtStRate_AMT.Text), "0.00"), DBNull.Value)
                ro("AMOUNT") = IIf(Val(txtStAmount_AMT.Text) <> 0, Format(Val(txtStAmount_AMT.Text), "0.00"), DBNull.Value)
                ro("METALID") = txtStMetalCode.Text
                ro("SEIVE") = CmbSeive.Text
                ro("OCATCODE") = "" & cmbIssRecCat.SelectedValue.ToString
                ro("STUDDEDUCT") = CmbStudDeduct.Text
                If _JobNoEnable = True Then
                    ro("ISSSNO") = txtTranno_OWN.Text
                    jobno = txtTranno_OWN.Text
                End If
                If Material.Issue = oMaterial Then
                    ro("STNTYPE") = CmbTrantype.Text
                    ''ro("STNTYPE") = ""
                    If STOCKVALIDATION Then
                        ro("RESNO") = txtReSno_OWN.Text
                    End If
                Else
                    ro("STNTYPE") = CmbTrantype.Text
                End If
                'If Material.Issue = oMaterial Then
                '    ro("STNTYPE") = ""
                '    If STOCKVALIDATION Then
                '        ro("RESNO") = txtReSno_OWN.Text
                '    End If
                'Else
                '    ro("STNTYPE") = CmbTrantype.Text
                'End If
                '4C****
                Dim view4c As String = ""
                Dim maintain4c As Boolean
                strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "' AND METALID='D'"
                If objGPack.GetSqlValue(strSql).ToString.ToUpper = "Y" Then maintain4c = True Else maintain4c = False
                If txtStSubItem.Text <> "" Then
                    strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtStItem.Text & "')"
                    If objGPack.GetSqlValue(strSql).ToString.ToUpper = "Y" Then maintain4c = True Else maintain4c = False
                End If
                If _FourCMaintain And maintain4c Then
                    Dim stnGrpId As String = "0"
                    Dim ColorId As String = "0"
                    Dim CutId As String = "0"
                    Dim ClarityId As String = "0"
                    Dim ShapeId As String = "0"
                    Dim SetTypeId As String = "0"
                    Dim StnHeight As String = "0"
                    Dim StnWidth As String = "0"
                    stnGrpId = objGPack.GetSqlValue("SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & ObjDiaDetails.cmbStnGrp.Text & "'", "GROUPID", 0)
                    ColorId = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & ObjDiaDetails.CmbColor.Text & "'", "COLORID", 0)
                    CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & ObjDiaDetails.CmbCut.Text & "'", "CUTID", 0)
                    ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & ObjDiaDetails.CmbClarity.Text & "'", "CLARITYID", 0)
                    ShapeId = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & ObjDiaDetails.cmbShape.Text & "'", "SHAPEID", 0)
                    SetTypeId = objGPack.GetSqlValue("SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & ObjDiaDetails.cmbSetType.Text & "'", "SETTYPEID", 0)
                    StnHeight = ObjDiaDetails.txtWidth_WET.Text.ToString
                    StnWidth = ObjDiaDetails.txtHeight_WET.Text.ToString
                    If ro("STNGRPID") Is DBNull.Value Then ro("STNGRPID") = Convert.ToString(stnGrpId)
                    If ro("CUTID") Is DBNull.Value Then ro("CUTID") = Convert.ToString(CutId)
                    If ro("COLORID") Is DBNull.Value Then ro("COLORID") = Convert.ToString(ColorId)
                    If ro("CLARITYID") Is DBNull.Value Then ro("CLARITYID") = Convert.ToString(ClarityId)
                    If ro("SHAPEID") Is DBNull.Value Then ro("SHAPEID") = Convert.ToString(ShapeId)
                    If ro("SETTYPEID") Is DBNull.Value Then ro("SETTYPEID") = Convert.ToString(SetTypeId)
                    If ro("HEIGHT") Is DBNull.Value Then ro("HEIGHT") = Convert.ToString(StnHeight)
                    If ro("WIDTH") Is DBNull.Value Then ro("WIDTH") = Convert.ToString(StnWidth)
                End If
                '****4C
                dtGridStone.Rows.Add(ro)
            Else
                With gridStone.Rows(Val(txtStRowIndex.Text))
                    If ACC_STUDITEM_POPUP Then
                        .Cells("ITEM").Value = txtStItem.Text
                        .Cells("SUBITEM").Value = txtStSubItem.Text
                    Else
                        .Cells("ITEM").Value = cmbItem.Text
                        .Cells("SUBITEM").Value = cmbSubItem.Text
                    End If
                    .Cells("PCS").Value = IIf(Val(txtStPcs_NUM.Text) <> 0, Val(txtStPcs_NUM.Text), DBNull.Value)
                    .Cells("UNIT").Value = cmbStUnit.Text
                    .Cells("CALC").Value = cmbStCalc.Text
                    .Cells("WEIGHT").Value = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), DBNull.Value)
                    .Cells("RATE").Value = IIf(Val(txtStRate_AMT.Text) <> 0, Format(Val(txtStRate_AMT.Text), "0.00"), DBNull.Value)
                    .Cells("AMOUNT").Value = IIf(Val(txtStAmount_AMT.Text) <> 0, Format(Val(txtStAmount_AMT.Text), "0.00"), DBNull.Value)
                    .Cells("METALID").Value = txtStMetalCode.Text
                    .Cells("SEIVE").Value = CmbSeive.Text
                    .Cells("OCATCODE").Value = "" & cmbIssRecCat.SelectedValue.ToString
                    .Cells("STUDDEDUCT").Value = CmbStudDeduct.Text
                    If _JobNoEnable = True Then
                        .Cells("ISSSNO").Value = txtTranno_OWN.Text
                        jobno = txtTranno_OWN.Text
                    End If
                    If Material.Issue = oMaterial Then
                        '.Cells("STNTYPE").Value = ""
                        .Cells("STNTYPE").Value = CmbTrantype.Text
                        If STOCKVALIDATION Then
                            .Cells("RESNO").Value = txtReSno_OWN.Text
                        End If
                    Else
                        .Cells("STNTYPE").Value = CmbTrantype.Text
                    End If
                    Dim stnGrpId As String = "0"
                    stnGrpId = objGPack.GetSqlValue("SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & ObjDiaDetails.cmbStnGrp.Text & "'", "GROUPID", 0)
                    .Cells("STNGRPID").Value = stnGrpId
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
            txtTranno_OWN.Text = jobno
            'txtStPcs_NUM.Clear()
            'txtStWeight_WET.Clear()
            'txtStRate_AMT.Clear()
            'txtStAmount_AMT.Clear()
            'txtStMetalCode.Clear()
            If ACC_STUDITEM_POPUP Then txtStItem.Focus() Else cmbItem.Focus()
        End If
    End Sub
    Private Sub txtStPcs_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStPcs_NUM.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridStone.RowCount > 0 Then gridStone.Select()
        End If
    End Sub

    Private Sub txtStPcs_NUM_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStPcs_NUM.Leave

        ''''''
        Dim view4c As String = ""
        Dim maintain4c As Boolean
        strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "' AND METALID='D'"
        If objGPack.GetSqlValue(strSql).ToString.ToUpper = "Y" Then maintain4c = True Else maintain4c = False
        If txtStSubItem.Text <> "" Then
            strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtStItem.Text & "')"
            If objGPack.GetSqlValue(strSql).ToString.ToUpper = "Y" Then maintain4c = True Else maintain4c = False
        End If
        If _FourCMaintain And maintain4c Then
            ObjDiaDetails = New frmDiamondDetails
            If txtStItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtStItem.Text & "'", , , )
            If txtStSubItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & txtStSubItem.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtStItem.Text & "')", , , )
            If Not view4c.Contains("CU") Then ObjDiaDetails.CmbCut.Enabled = False
            If Not view4c.Contains("CO") Then ObjDiaDetails.CmbColor.Enabled = False
            If Not view4c.Contains("CL") Then ObjDiaDetails.CmbClarity.Enabled = False
            If Not view4c.Contains("SH") Then ObjDiaDetails.cmbShape.Enabled = False
            If Not view4c.Contains("SE") Then ObjDiaDetails.cmbSetType.Enabled = False
            If Not view4c.Contains("HE") Then ObjDiaDetails.txtHeight_WET.Enabled = False
            If Not view4c.Contains("WI") Then ObjDiaDetails.txtWidth_WET.Enabled = False
            ObjDiaDetails.BackColor = Color.Lavender
            ObjDiaDetails.StartPosition = FormStartPosition.CenterParent
            If gridStone.Rows.Count > 0 Then
                ObjDiaDetails.cmbStnGrp.Text = GetSqlValue(cn, "SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = '" & gridStone.CurrentRow.Cells("STNGRPID").Value.ToString & "'")
                ObjDiaDetails.SetDefaultValues(ObjDiaDetails.cmbStnGrp.Text)
                If ObjDiaDetails.cmbStnGrp.Text = "" Then
                    Dim cut As String = GetSqlValue(cn, "SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = '" & gridStone.CurrentRow.Cells("CUTID").Value.ToString & "'")
                    Dim colorname As String = GetSqlValue(cn, "SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID = '" & gridStone.CurrentRow.Cells("COLORID").Value.ToString & "'")
                    Dim clarity As String = GetSqlValue(cn, "SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID ='" & gridStone.CurrentRow.Cells("CLARITYID").Value.ToString & "'")
                    Dim settype As String = GetSqlValue(cn, "SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPEID ='" & gridStone.CurrentRow.Cells("SETTYPEID").Value.ToString & "'")
                    Dim shape As String = GetSqlValue(cn, "SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID = '" & gridStone.CurrentRow.Cells("SHAPEID").Value.ToString & "'")
                    ObjDiaDetails.CmbCut.Text = cut
                    ObjDiaDetails.CmbColor.Text = colorname
                    ObjDiaDetails.CmbClarity.Text = clarity
                    ObjDiaDetails.cmbShape.Text = shape
                    ObjDiaDetails.cmbSetType.Text = settype
                    ObjDiaDetails.txtWidth_WET.Text = Width
                    ObjDiaDetails.txtHeight_WET.Text = Height
                End If
            End If
        End If
        ObjDiaDetails.cmbStnGrp.Focus()
        ObjDiaDetails.ShowDialog()
        'ElseIf _FourCMaintain And tagEdit And maintain4c Then
        '    ObjDiaDetails = New frmDiamondDetails
        '    ObjDiaDetails.CmbCut.Text = Cut
        '    ObjDiaDetails.CmbColor.Text = Color
        '    ObjDiaDetails.CmbClarity.Text = Clarity
        '    ObjDiaDetails.cmbShape.Text = Shape
        '    ObjDiaDetails.cmbSetType.Text = SetType
        '    ObjDiaDetails.txtWidth_WET.Text = Width
        '    ObjDiaDetails.txtHeight_WET.Text = Height
        '    If cmbSItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbSItem.Text & "'", , , )
        '    If cmbSSubItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSSubItem.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbSItem.Text & "')", , , )
        '    If Not view4c.Contains("CU") Then ObjDiaDetails.CmbCut.Enabled = False
        '    If Not view4c.Contains("CO") Then ObjDiaDetails.CmbColor.Enabled = False
        '    If Not view4c.Contains("CL") Then ObjDiaDetails.CmbClarity.Enabled = False
        '    If Not view4c.Contains("SH") Then ObjDiaDetails.cmbShape.Enabled = False
        '    If Not view4c.Contains("SE") Then ObjDiaDetails.cmbSetType.Enabled = False
        '    If Not view4c.Contains("HE") Then ObjDiaDetails.txtHeight_WET.Enabled = False
        '    If Not view4c.Contains("WI") Then ObjDiaDetails.txtWidth_WET.Enabled = False
        '    ObjDiaDetails.CmbCut.Focus()
        '    ObjDiaDetails.ShowDialog()
        ''''''

        If STOCKVALIDATION = False Then Exit Sub
        If oMaterial = Material.Receipt Then Exit Sub
        If Val(txtEditPcs.Text) <> 0 Then
            If Val(txtStPcs_NUM.Text) > Val(txtEditPcs.Text) Then
                If oMaterial = Material.Issue Then
                    MsgBox("Issue Pcs Should not Exceed the Receipt Pcs", MsgBoxStyle.Information)
                Else
                    MsgBox("Receipt Pcs Should not Exceed the Issue Pcs", MsgBoxStyle.Information)
                End If
                txtStPcs_NUM.Text = txtEditPcs.Text
                txtStPcs_NUM.Select()
            End If
        End If
    End Sub

    Private Sub txtStPcs_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStPcs_NUM.TextChanged
        CalcRate()
        CalcStoneAmount()
    End Sub
    Private Sub txtStRate_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStRate_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridStone.RowCount > 0 Then gridStone.Select()
        End If
    End Sub

    Private Sub txtStRate_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStRate_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim cent As Double = 0
            If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))) Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))
            cent *= 100
            strSql = " DECLARE @CENT FLOAT"
            strSql += " SET @CENT = " & cent & ""
            strSql += " SELECT PURRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            strSql += " @CENT BETWEEN FROMCENT AND TOCENT "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbItem.Text) & "')"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, cmbSubItem.Text) & "' "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbItem.Text) & "')),0)"
            strSql += " AND ISNULL(ACCODE,'') = '" & _Accode & "'"
            Dim rate As Double = Val(objGPack.GetSqlValue(strSql, , , tran))
            If Val(txtStRate_AMT.Text) < rate Then
                'If _Authorize Then
                If Ogstnrateedit = True Then
                    If MessageBox.Show(Me.GetNextControl(txtStRate_AMT, False).Text + E0020 + rate.ToString + vbCrLf + "Do you wish to Continue?", "Authorize Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                        txtStRate_AMT.Focus()
                        Exit Sub
                    Else
                        SendKeys.Send("{TAB}")
                    End If
                Else
                    MsgBox(Me.GetNextControl(txtStRate_AMT, False).Text + E0020 + rate.ToString)
                    txtStRate_AMT.Focus()
                    Exit Sub
                End If
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub txtStRate_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStRate_AMT.TextChanged
        CalcStoneAmount()
    End Sub

    Private Sub txtStWeight_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStWeight.GotFocus
        If Val(txtStWeight.Text) = 0 And PacketNo <> Nothing And PacketItemId <> Nothing Then
            strSql = vbCrLf + " SELECT CONVERT(NUMERIC(15,4)," & Val(txtStPcs_NUM.Text) & " * (STNWT/STNPCS)) FROM " & cnAdminDb & "..ITEMNONTAGSTONE"
            strSql += vbCrLf + " WHERE TAGSNO = (SELECT TOP 1 SNO FROM " & cnAdminDb & "..ITEMNONTAG WHERE ITEMID = " & PacketItemId & " AND PACKETNO = '" & PacketNo & "' AND RECISS = 'R' ORDER BY RECDATE DESC)"
            strSql += vbCrLf + " AND STNITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbItem.Text) & "')"
            strSql += vbCrLf + " AND STNSUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, cmbSubItem.Text) & "' "
            strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbItem.Text) & "'))"
            Dim stWt As Decimal = Val(objGPack.GetSqlValue(strSql))
            txtStWeight.Text = IIf(stWt <> 0, Format(stWt, FormatNumberStyle(DiaRnd)), "")
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
            WeightValidation(txtStWeight, e, DiaRnd)
        End If
    End Sub

    Private Sub txtStWeight_WET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStWeight.LostFocus
        cmbStCalc.Text = IIf(Val(txtStWeight.Text) <> 0, "W", "P")
        txtStWeight.Text = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), txtStWeight.Text)
    End Sub

    Private Sub CalcRate()
        If txtStItem.Text = "" Then Exit Sub
        Dim cent As Double = 0
        If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))) Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))
        cent *= 100
        strSql = " DECLARE @CENT FLOAT"
        strSql += " SET @CENT = " & cent & ""
        strSql += " SELECT PURRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
        strSql += " @CENT BETWEEN FROMCENT AND TOCENT "
        strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbItem.Text) & "')"
        strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, cmbSubItem.Text) & "' "
        strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbItem.Text) & "')),0)"
        strSql += " AND ISNULL(ACCODE,'') = '" & _Accode & "'"
        'If GetAdmindbSoftValue("COSTID") = "Y" Then strSql += " AND ISNULL(COSTID,'') = '" & _Costcode & "'"
        Dim rate As Double = Val(objGPack.GetSqlValue(strSql, "PURRATE", "", tran))
        If rate <> 0 Then
            txtStRate_AMT.Text = IIf(rate <> 0, Format(rate, "0.00"), "")
        End If
        strSql = " DECLARE @CENT FLOAT"
        strSql += " SET @CENT = " & cent & ""
        strSql += " SELECT SIZEDESC FROM " & cnAdminDb & "..CENTSIZE WHERE"
        strSql += " @CENT BETWEEN FROMCENT AND TOCENT "
        strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbItem.Text) & "')"
        strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, cmbSubItem.Text) & "' "
        strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbItem.Text) & "')),0)"
        Dim sizedisc As String = ""
        sizedisc = objGPack.GetSqlValue(strSql, "SIZEDESC", "", tran)
        CmbSeive.Text = sizedisc

    End Sub

    Private Sub txtStWeight_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStWeight.TextChanged
        CalcRate()
        CalcStoneAmount()
    End Sub
    Private Sub CalcStoneAmount()
        Dim amt As Double = Nothing
        If cmbStCalc.Text = "P" Then
            amt = Val(txtStRate_AMT.Text) * Val(txtStPcs_NUM.Text)
        Else
            amt = Val(txtStRate_AMT.Text) * Val(txtStWeight.Text)

        End If
        txtStAmount_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
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
            ElseIf txtStItem.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'" & GetItemQryFilteration()) = False Then
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
        strSql += " WHERE ISNULL(STUDDED,'') IN('S','B') AND ACTIVE = 'Y'"
        strSql += GetItemQryFilteration()
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


        strSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"
        txtStMetalCode.Text = objGPack.GetSqlValue(strSql, "DIASTONE", , tran)
        'If txtStMetalCode.Text = "S" Then cmbStUnit.Text = "G" Else cmbStUnit.Text = "C"
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
                CmbTrantype.Text = .Cells("STNTYPE").FormattedValue
                If ACC_STUDITEM_POPUP Then
                    txtStItem.Text = .Cells("ITEM").FormattedValue
                    txtStSubItem.Text = .Cells("SUBITEM").FormattedValue
                Else
                    cmbItem.Text = .Cells("ITEM").FormattedValue
                    cmbSubItem.Text = .Cells("SUBITEM").FormattedValue
                End If
                txtStPcs_NUM.Text = .Cells("PCS").FormattedValue
                txtStWeight.Text = .Cells("WEIGHT").FormattedValue
                cmbStUnit.Text = .Cells("UNIT").FormattedValue
                cmbStCalc.Text = .Cells("CALC").FormattedValue
                txtStRate_AMT.Text = .Cells("RATE").FormattedValue
                txtStAmount_AMT.Text = .Cells("AMOUNT").FormattedValue
                txtStMetalCode.Text = objGPack.GetSqlValue("SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'")
                CmbSeive.Text = .Cells("SEIVE").FormattedValue
                CmbStudDeduct.Text = .Cells("STUDDEDUCT").FormattedValue
                txtTranno_OWN.Text = .Cells("ISSSNO").FormattedValue
                If .Cells("ISSSNO").FormattedValue <> "" Then
                    txtEditPcs.Text = .Cells("PCS").FormattedValue
                    txtEditWet.Text = .Cells("WEIGHT").FormattedValue
                    txtEditUnit.Text = .Cells("UNIT").FormattedValue
                Else
                    txtEditPcs.Clear()
                    txtEditWet.Clear()
                    txtEditUnit.Clear()
                End If
                If STOCKVALIDATION Then
                    txtReSno_OWN.Text = .Cells("RESNO").FormattedValue
                    txtEditPcs.Text = .Cells("PCS").FormattedValue
                    txtEditWet.Text = .Cells("WEIGHT").FormattedValue
                    txtEditUnit.Text = .Cells("UNIT").FormattedValue
                End If
                txtStRowIndex.Text = gridStone.CurrentRow.Index
                If IssRecStudWtDedut Then CmbStudDeduct.Focus() Else txtStPcs_NUM.Focus()
            End With
        End If
    End Sub

    Private Sub gridStone_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridStone.UserDeletedRow
        dtGridStone.AcceptChanges()
        CalcStoneWtAmount()
        If Not gridStone.RowCount > 0 Then
            If ACC_STUDITEM_POPUP Then txtStItem.Focus() Else cmbItem.Focus()
        End If
    End Sub
    Public Sub frmStoneDiaAc_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If FromFlag = "A" Then
            ''Accounts Entry
            StudWtDedut = GetAdmindbSoftValue("STUDWTDEDUCT_ACC", "DSP")
        Else
            StudWtDedut = GetAdmindbSoftValue("STUDWTDEDUCT", "DSP")
        End If
        If oMaterial = Material.Issue Then  '' change for  vbj
            'CmbTrantype.Enabled = False
            CmbTrantype.Text = ""
        End If
        ''Transaction Type
        CmbTrantype.Items.Clear()
        If MIMR_USER_RESTRICT.ToString <> "" Then
            Dim _temptrantypeuser() As String = MIMR_USER_RESTRICT.ToString.Split(",")
            Dim _tempcontains As Boolean = False
            Dim _tempcontainstr As String = ""
            For cnt As Integer = 0 To _temptrantypeuser.Length - 1
                _tempcontainstr = ""
                _tempcontains = False
                If _temptrantypeuser(cnt).ToString.Contains(userId.ToString & ":") Then
                    _tempcontains = True
                    _tempcontainstr = _temptrantypeuser(cnt).ToString
                    Exit For
                End If
            Next
            If _tempcontains = True And _tempcontainstr.ToString <> "" Then
                If _tempcontainstr.ToString.Contains("IIS") Or _tempcontainstr.ToString.Contains("IPU") Or _tempcontainstr.ToString.Contains("IIN") Or _tempcontainstr.ToString.Contains("IOT") _
                   Or _tempcontainstr.ToString.Contains("IPA") Or _tempcontainstr.ToString.Contains("IDN") Then
                    If oMaterial = Material.Issue Then CmbTrantype.Items.Add("ISSUE")
                    If oMaterial = Material.Issue Then CmbTrantype.Items.Add("PURCHASE RETURN")
                ElseIf _tempcontainstr.ToString.Contains("RRE") _
                Or _tempcontainstr.ToString.Contains("RPU") Or _tempcontainstr.ToString.Contains("RIN") Or _tempcontainstr.ToString.Contains("ROT") _
                Or _tempcontainstr.ToString.Contains("RPA") Or _tempcontainstr.ToString.Contains("RDN") Then
                    If oMaterial = Material.Receipt Then CmbTrantype.Items.Add("RECEIPT")
                    If oMaterial = Material.Receipt Then CmbTrantype.Items.Add("PURCHASE")
                End If
                If _tempcontainstr.ToString.Contains("IAP") Or _tempcontainstr.ToString.Contains("RAP") Then CmbTrantype.Items.Add("APPROVAL")
                CmbTrantype.SelectedIndex = 0
            Else
                GoTo MoveType
            End If
        Else
MoveType:
            If oMaterial = Material.Issue Then
                CmbTrantype.Items.Add("ISSUE")
                CmbTrantype.Items.Add("PURCHASE RETURN")
                CmbTrantype.Items.Add("APPROVAL")
                If MIMRSTONETYPE.ToString = "ISSUE" Or MIMRSTONETYPE.ToString = "PURCHASE RETURN" Then
                    CmbTrantype.Text = MIMRSTONETYPE
                ElseIf MIMRSTONETYPE.ToString = "APPROVAL ISSUE" Then
                    CmbTrantype.Text = "APPROVAL"
                Else
                    CmbTrantype.Text = "ISSUE"
                End If
            Else
                CmbTrantype.Items.Add("RECEIPT")
                CmbTrantype.Items.Add("PURCHASE")
                CmbTrantype.Items.Add("APPROVAL")
                If MIMRSTONETYPE.ToString = "RECEIPT" Or MIMRSTONETYPE.ToString = "PURCHASE" Then
                    CmbTrantype.Text = MIMRSTONETYPE
                ElseIf MIMRSTONETYPE.ToString = "APPROVAL RECEIPT" Then
                    CmbTrantype.Text = "APPROVAL"
                Else
                    CmbTrantype.Text = "RECEIPT"
                End If
            End If
        End If
        gridStone.AllowUserToDeleteRows = Not _DelLock
        gridStone.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridStoneTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridStoneTotal.DefaultCellStyle.BackColor = grpStone.BackgroundColor
        gridStoneTotal.DefaultCellStyle.SelectionBackColor = grpStone.BackgroundColor
        CalcStoneWtAmount()
        StyleGridStone(gridStone)
        StyleGridStone(gridStoneTotal)
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

    Private Sub txtTranno_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTranno_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If txtTranno_OWN.Text.Trim() = "" Then Exit Sub
            'If Not STOCKVALIDATION Then
            If _JobNoEnable = False Then
                strSql = vbCrLf + "  SELECT ITEM,SUBITEM,PCS,UNIT,CALC,WEIGHT,RATE,AMOUNT,METALID,SNO,''STNTYPE FROM("
                strSql += vbCrLf + " SELECT IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM"
                'strSql += vbCrLf + " ,(PCS-ISNULL((SELECT SUM(PCS) FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO ),0)) AS PCS"
                strSql += vbCrLf + " ,PCS-"
                strSql += vbCrLf + " (SELECT SUM(PCS)PCS FROM ("
                strSql += vbCrLf + " SELECT ISNULL(SUM(PCS),0)PCS FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')='' "
                strSql += vbCrLf + " UNION ALL "
                'Manapalley Yokesh Altered
                'strSql += vbCrLf + " SELECT ISNULL(SUM(STNPCS),0)PCS FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')='') )X"
                strSql += vbCrLf + " SELECT ISNULL(SUM(STNPCS),0)PCS FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')='') )X"
                strSql += vbCrLf + " ) AS PCS"
                strSql += vbCrLf + " ,I.STONEUNIT UNIT,"
                strSql += vbCrLf + " CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC"
                'strSql += vbCrLf + " ,(PUREWT-ISNULL((SELECT SUM(PUREWT) FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO ),0)) WEIGHT"
                strSql += vbCrLf + " ,PUREWT-"
                strSql += vbCrLf + " (SELECT SUM(PUREWT)PUREWT FROM ("
                strSql += vbCrLf + " SELECT ISNULL(SUM(PUREWT),0)PUREWT FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')=''"
                strSql += vbCrLf + " UNION ALL "
                'Manepally Yokesh Altered
                'strSql += vbCrLf + " SELECT ISNULL(SUM(STNWT),0)PUREWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')='') )X"
                strSql += vbCrLf + " SELECT ISNULL(SUM(STNWT),0)PUREWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE  ISNULL(CANCEL,'')='') )X"
                strSql += vbCrLf + " ) AS WEIGHT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,"
                strSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL) AMOUNT,I.METALID,I.SNO"
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
                strSql += vbCrLf + " WHERE I.TRANTYPE ='IIS'  AND ISNULL(STUDDED,'') IN ('S','L','B') AND ISNULL(I.CANCEL,'')='' "
                'strSql += vbCrLf + " AND I.SNO NOT IN (SELECT DISTINCT JOBISNO FROM " & cnStockDb & "..RECEIPTSTONE WHERE "
                'strSql += vbCrLf + " ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')='') AND ISNULL(JOBISNO,'')<>'')"
                strSql += vbCrLf + " AND I.TRANNO =" & Val(txtTranno_OWN.Text)
                'strSql += vbCrLf + " AND I.ACCODE='" & _Accode & "'"
                strSql += vbCrLf + " ) X WHERE ISNULL(PCS,0)<>0 OR ISNULL(WEIGHT,0)<>0 "
                dtGrid = New DataTable
                dtGrid.Columns.Add("CHECK", GetType(Boolean))
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGrid)
                If Not dtGrid.Rows.Count > 0 Then
                    strSql = " SELECT STARTDATE-1 FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME = '" & cnStockDb & "'"
                    Dim menddate As Date = (objGPack.GetSqlValue(strSql))
                    strSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE '" & menddate & "' BETWEEN STARTDATE AND ENDDATE"
                    Dim prevdb As String = objGPack.GetSqlValue(strSql)
                    strSql = " SELECT 'EXISTS' FROM MASTER..SYSDATABASES WHERE NAME = '" & prevdb & "'"
                    If objGPack.GetSqlValue(strSql, , "-1") = "-1" Then
                        txtTranno_OWN.Select()
                        Exit Sub
                    End If

                    strSql = vbCrLf + "  SELECT ITEM,SUBITEM,PCS,UNIT,CALC,WEIGHT,RATE,AMOUNT,METALID,SNO,''STNTYPE FROM("
                    strSql += vbCrLf + " SELECT IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM"
                    'strSql += vbCrLf + " ,(PCS-ISNULL((SELECT SUM(PCS) FROM " & prevdb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO ),0)) AS PCS"
                    strSql += vbCrLf + " ,PCS-"
                    strSql += vbCrLf + " (SELECT SUM(PCS)PCS FROM ("
                    strSql += vbCrLf + " SELECT ISNULL(SUM(PCS),0)PCS FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')='' "
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT ISNULL(SUM(STNPCS),0)PCS FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')='') )X"
                    strSql += vbCrLf + " ) AS PCS"
                    strSql += vbCrLf + " ,I.STONEUNIT UNIT,"
                    strSql += vbCrLf + " CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC"
                    'strSql += vbCrLf + " ,(PUREWT-ISNULL((SELECT SUM(PUREWT) FROM " & prevdb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO ),0)) WEIGHT"
                    strSql += vbCrLf + " ,PUREWT-"
                    strSql += vbCrLf + " (SELECT SUM(PUREWT)PUREWT FROM ("
                    strSql += vbCrLf + " SELECT ISNULL(SUM(PUREWT),0)PUREWT FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')=''"
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT ISNULL(SUM(STNWT),0)PUREWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')='') )X"
                    strSql += vbCrLf + " ) AS WEIGHT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,"
                    strSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL) AMOUNT,I.METALID,I.SNO"
                    strSql += vbCrLf + " FROM " & prevdb & "..ISSUE I INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
                    strSql += vbCrLf + " WHERE I.TRANTYPE ='IIS'  AND ISNULL(STUDDED,'') IN ('S','L','B') AND ISNULL(I.CANCEL,'')='' "
                    'strSql += vbCrLf + " AND I.SNO NOT IN (SELECT DISTINCT JOBISNO FROM " & prevdb & "..RECEIPTSTONE WHERE "
                    'strSql += vbCrLf + " ISSSNO IN (SELECT SNO FROM " & prevdb & "..RECEIPT WHERE ISNULL(CANCEL,'')='') AND ISNULL(JOBISNO,'')<>'')"
                    strSql += vbCrLf + " AND I.TRANNO =" & Val(txtTranno_OWN.Text)
                    strSql += vbCrLf + " AND I.ACCODE='" & _Accode & "'"
                    strSql += vbCrLf + " ) X WHERE ISNULL(PCS,0)<>0 OR ISNULL(WEIGHT,0)<>0 "

                    dtGrid = New DataTable
                    dtGrid.Columns.Add("CHECK", GetType(Boolean))
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtGrid)
                    If Not dtGrid.Rows.Count > 0 Then
                        txtTranno_OWN.Select()
                        Exit Sub
                    End If
                End If

                objMultiSelect = New MultiSelectRowDia(dtGrid, "")
                objMultiSelect.chkAppSales.Visible = False
                If objMultiSelect.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Me.Refresh()
                    Dim sno As String = ""
                    For Each RoS As DataRow In objMultiSelect.RowSelected
                        If Val(dtGridStone.Compute("COUNT(ISSSNO)", "ISSSNO ='" & RoS.Item("SNO").ToString & "'")) > 0 Then Continue For
                        Dim ro As DataRow = Nothing
                        ro = dtGridStone.NewRow
                        ro("ITEM") = RoS.Item("ITEM").ToString
                        ro("SUBITEM") = RoS.Item("SUBITEM").ToString
                        ro("PCS") = RoS.Item("PCS").ToString
                        ro("UNIT") = RoS.Item("UNIT").ToString
                        ro("CALC") = RoS.Item("CALC").ToString
                        ro("WEIGHT") = RoS.Item("WEIGHT").ToString
                        ro("RATE") = Val(RoS.Item("RATE").ToString)
                        ro("AMOUNT") = Val(RoS.Item("AMOUNT").ToString)
                        ro("METALID") = RoS.Item("METALID").ToString
                        ro("ISSSNO") = RoS.Item("SNO").ToString
                        ro("STNTYPE") = IIf(RoS.Item("STNTYPE").ToString = "PU", "Purchase", "Receipt")
                        dtGridStone.Rows.Add(ro)
                    Next
                    dtGridStone.AcceptChanges()
                    gridStone.CurrentCell = gridStone.Rows(gridStone.RowCount - 1).Cells("ITEM")
                    CalcStoneWtAmount()
                    StyleGridStone(gridStone)
                    StyleGridStone(gridStoneTotal)
                    objGPack.TextClear(grpStone)
                    If StnRowEDit = False Then _EditLock = True 'gridStone.Enabled = False
                End If
            Else
                loadJobDetails()
            End If
            'End If
        End If
    End Sub

    Private Sub txtTranno_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTranno_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        ElseIf e.KeyCode = Keys.Insert Then
            If _JobNoEnable Then
                strSql = vbCrLf + "  SELECT JOBNO,SUM(RSTNPCS) [REC STNPCS],SUM(RSTNWT) [REC STNWT],SUM(ISTNPCS) [ISS STNPCS],SUM(ISTNWT) [ISS STNWT] "
                strSql += vbCrLf + " FROM ("
                strSql += vbCrLf + " SELECT RS.JOBISNO JOBNO,SUM(ISNULL(STNPCS,0)) RSTNPCS,SUM(ISNULL(STNWT,0)) RSTNWT "
                strSql += vbCrLf + " ,0 ISTNPCS,0 ISTNWT"
                strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE RS "
                strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..RECEIPT R ON R.SNO=RS.ISSSNO AND R.BATCHNO=RS.BATCHNO  "
                strSql += vbCrLf + " WHERE RS.TRANTYPE IN('RRE','RPU') AND ISNULL(RS.JOBISNO,'')<>'' "
                strSql += vbCrLf + " AND ACCODE='" & _Accode & "' AND ISNULL(CANCEL,'')=''"
                strSql += vbCrLf + " GROUP BY RS.JOBISNO"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT ISS.JOBISNO JOBNO,0 ISTNPCS,0 ISTNWT"
                strSql += vbCrLf + " ,SUM(ISNULL(STNPCS,0)) ISTNPCS,SUM(ISNULL(STNWT,0)) ISTNWT "
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE ISS  LEFT JOIN " & cnStockDb & "..ISSUE  I ON I.SNO =ISS.ISSSNO AND I.BATCHNO=ISS.BATCHNO"
                strSql += vbCrLf + " WHERE ISS.TRANTYPE IN('IIS','IPU','IRC') AND ISNULL(ISS.JOBISNO,'')<>'' "
                strSql += vbCrLf + " AND ACCODE='" & _Accode & "' AND ISNULL(CANCEL,'')='' GROUP BY ISS.JOBISNO"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT R.JOBNO JOBNO,SUM(ISNULL(PCS,0)) RSTNPCS,SUM(ISNULL(GRSWT,0)) RSTNWT "
                strSql += vbCrLf + " ,0 ISTNPCS,0 ISTNWT"
                strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R "
                strSql += vbCrLf + " WHERE R.TRANTYPE IN('RRE','RPU') AND ISNULL(R.JOBNO,'')<>'' "
                strSql += vbCrLf + " AND R.METALID IN ('T')"
                strSql += vbCrLf + " AND ACCODE='" & _Accode & "' AND ISNULL(CANCEL,'')=''"
                strSql += vbCrLf + " GROUP BY R.JOBNO"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT I.JOBNO JOBNO,0 ISTNPCS,0 ISTNWT"
                strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) ISTNPCS,SUM(ISNULL(GRSWT,0)) ISTNWT "
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I  "
                strSql += vbCrLf + " WHERE I.TRANTYPE IN('IIS','IPU','IRC') "
                strSql += vbCrLf + " AND I.METALID IN ('T')"
                strSql += vbCrLf + " AND ACCODE='" & _Accode & "' AND ISNULL(CANCEL,'')='' GROUP BY I.JOBNO"
                strSql += vbCrLf + " )X GROUP BY JOBNO HAVING SUM(ISTNWT-RSTNWT) <>0 "
                strSql += vbCrLf + " ORDER BY JOBNO"
                Dim JnoRow As DataRow
                JnoRow = BrighttechPack.SearchDialog.Show_R("Find JobNo", strSql, cn, , 0, , , , , , False)
                If Not JnoRow Is Nothing Then
                    txtTranno_OWN.Text = JnoRow.Item("JOBNO").ToString
                End If
                txtTranno_OWN.SelectAll()
            Else
                Dim sql As String
                strSql = vbCrLf + "  SELECT TRANNO,(SELECT TOP 1 RFID FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO)RFID,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,ITEM,SUBITEM,PCS,UNIT,CALC,WEIGHT,RATE,AMOUNT,METALID,SNO,''STNTYPE "
                strSql += vbCrLf + " "
                strSql += vbCrLf + " FROM("
                strSql += vbCrLf + " SELECT I.TRANNO,I.TRANDATE,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM"
                strSql += vbCrLf + " ,PCS-"
                strSql += vbCrLf + " (SELECT SUM(PCS)PCS FROM ("
                strSql += vbCrLf + " SELECT ISNULL(SUM(PCS),0)PCS FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(ISSNO,'')=I.SNO AND ISNULL(CANCEL,'')='' "
                strSql += vbCrLf + " UNION ALL "
                'strSql += vbCrLf + " SELECT ISNULL(SUM(STNPCS),0)PCS FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')='') )X"
                strSql += vbCrLf + " SELECT ISNULL(SUM(STNPCS),0)PCS FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE  ISNULL(CANCEL,'')='') )X"
                strSql += vbCrLf + " ) AS PCS"
                strSql += vbCrLf + " ,I.STONEUNIT UNIT,"
                strSql += vbCrLf + " CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC"
                strSql += vbCrLf + " ,PUREWT-"
                strSql += vbCrLf + " (SELECT SUM(PUREWT)PUREWT FROM ("
                strSql += vbCrLf + " SELECT ISNULL(SUM(PUREWT),0)PUREWT FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(ISSNO,'')=I.SNO  AND ISNULL(CANCEL,'')=''"
                strSql += vbCrLf + " UNION ALL "
                'strSql += vbCrLf + " SELECT ISNULL(SUM(STNWT),0)PUREWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')='') )X"
                strSql += vbCrLf + " SELECT ISNULL(SUM(STNWT),0)PUREWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE  ISNULL(CANCEL,'')='') )X"
                strSql += vbCrLf + " ) AS WEIGHT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,"
                strSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL) AMOUNT,I.METALID,I.SNO"
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
                strSql += vbCrLf + " WHERE I.TRANTYPE ='IIS'  AND ISNULL(STUDDED,'') IN ('S','L','B') AND ISNULL(I.CANCEL,'')='' "
                If Val(txtTranno_OWN.Text) <> 0 Then strSql += vbCrLf + " AND I.TRANNO =" & Val(txtTranno_OWN.Text)
                strSql += vbCrLf + " AND I.ACCODE='" & _Accode & "'"
                strSql += vbCrLf + " ) X WHERE ISNULL(PCS,0)<>0 OR ISNULL(WEIGHT,0)<>0 "
                sql = " SELECT count(*) FROM " & cnAdminDb & "..DBMASTER "
                Dim prevdb As String = cnStockDb
                Dim cnt As Integer = (objGPack.GetSqlValue(sql))
                For X As Integer = 0 To cnt - 1
                    sql = " SELECT STARTDATE-1 FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME = '" & prevdb & "'"
                    Dim menddate As Date = (objGPack.GetSqlValue(sql))
                    sql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE '" & menddate & "' BETWEEN STARTDATE AND ENDDATE"
                    prevdb = objGPack.GetSqlValue(sql)
                    sql = " SELECT 'EXISTS' FROM MASTER..SYSDATABASES WHERE NAME = '" & prevdb & "'"
                    If objGPack.GetSqlValue(sql, , "-1") <> "-1" Then
                        strSql += vbCrLf + "  UNION ALL"
                        strSql += vbCrLf + "  SELECT TRANNO,(SELECT TOP 1 RFID FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO)RFID,CONVERT(VARCHAR(20),TRANDATE,105)TRANDATE,ITEM,SUBITEM,PCS,UNIT,CALC,WEIGHT,RATE,AMOUNT,METALID,SNO,''STNTYPE "
                        strSql += vbCrLf + " "
                        strSql += vbCrLf + " FROM("
                        strSql += vbCrLf + " SELECT I.TRANNO,I.TRANDATE,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM"
                        strSql += vbCrLf + " ,PCS-"
                        strSql += vbCrLf + " (SELECT SUM(PCS)PCS FROM ("
                        strSql += vbCrLf + " SELECT ISNULL(SUM(PCS),0)PCS FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(ISSNO,'')=I.SNO AND ISNULL(CANCEL,'')='' "
                        strSql += vbCrLf + " UNION ALL "
                        strSql += vbCrLf + " SELECT ISNULL(SUM(STNPCS),0)PCS FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')='') )X"
                        strSql += vbCrLf + " ) AS PCS"
                        strSql += vbCrLf + " ,I.STONEUNIT UNIT,"
                        strSql += vbCrLf + " CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC"
                        strSql += vbCrLf + " ,PUREWT-"
                        strSql += vbCrLf + " (SELECT SUM(PUREWT)PUREWT FROM ("
                        strSql += vbCrLf + " SELECT ISNULL(SUM(PUREWT),0)PUREWT FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(ISSNO,'')=I.SNO  AND ISNULL(CANCEL,'')=''"
                        strSql += vbCrLf + " UNION ALL "
                        strSql += vbCrLf + " SELECT ISNULL(SUM(STNWT),0)PUREWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')='') )X"
                        strSql += vbCrLf + " ) AS WEIGHT"
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,"
                        strSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL) AMOUNT,I.METALID,I.SNO"
                        strSql += vbCrLf + " FROM " & prevdb & "..ISSUE I INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
                        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
                        strSql += vbCrLf + " WHERE I.TRANTYPE ='IIS'  AND ISNULL(STUDDED,'') IN ('S','L','B') AND ISNULL(I.CANCEL,'')='' "
                        If Val(txtTranno_OWN.Text) <> 0 Then strSql += vbCrLf + " AND I.TRANNO =" & Val(txtTranno_OWN.Text)
                        strSql += vbCrLf + " AND I.ACCODE='" & _Accode & "'"
                        strSql += vbCrLf + " ) X WHERE ISNULL(PCS,0)<>0 OR ISNULL(WEIGHT,0)<>0 "
                    End If
                Next
                Dim JnoRow As DataRow
                JnoRow = BrighttechPack.SearchDialog.Show_R("Find TranNo", strSql, cn, , 0, , , , , , False)
                If Not JnoRow Is Nothing Then
                    txtTranno_OWN.Text = JnoRow.Item("TRANNO").ToString
                End If
                txtTranno_OWN.SelectAll()
            End If
        End If
    End Sub




    Private Sub loadJobDetails()
        If txtTranno_OWN.Text <> "" Then
            Dim JObno As String = ""
            JObno = txtTranno_OWN.Text
            Dim mStrSql As String = ""
            If oMaterial = Material.Issue Then
                mStrSql = vbCrLf + " SELECT DESCRIP,ITEM,SUBITEM,JOBNO,SUM(STNPCS)STNPCS,SUM(STNWT) STNWT,UNIT,CALC,RATE,AMOUNT,METALID,0 AS RESULT  FROM"
                mStrSql += vbCrLf + " ("
                mStrSql += vbCrLf + " SELECT 'ISSUE' DESCRIP,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM,JOBISNO JOBNO"
                mStrSql += vbCrLf + " ,SUM(ISNULL(STNPCS,0))*(-1) STNPCS,SUM(ISNULL(ISS.STNWT,0))*(-1) STNWT,ISS.STONEUNIT UNIT,ISS.CALCMODE CALC"
                mStrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,CONVERT(NUMERIC(15,2),NULL) AMOUNT,IM.METALID  FROM " & cnStockDb & "..ISSSTONE ISS  "
                mStrSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSUE I ON I.SNO=ISS.ISSSNO AND I.BATCHNO=ISS.BATCHNO "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = ISS.STNITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = ISS.STNSUBITEMID"
                mStrSql += vbCrLf + " WHERE I.TRANTYPE IN('IIS','IPU','IRC','ISS') AND ACCODE='" & _Accode & "' AND ISS.JOBISNO='" & JObno & "'  AND ISNULL(CANCEL,'')=''"
                mStrSql += vbCrLf + " GROUP BY JOBISNO,IM.ITEMNAME,SI.SUBITEMNAME,ISS.STONEUNIT,CALCMODE,IM.METALID "
                mStrSql += vbCrLf + " UNION ALL"
                mStrSql += vbCrLf + " SELECT 'RECEIPT' DESCRIP,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " ,RS.JOBISNO JOBNO,SUM(ISNULL(STNPCS,0)) STNPCS,SUM(ISNULL(RS.STNWT,0)) STNWT,RS.STONEUNIT UNIT,CALCMODE CALC"
                mStrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,CONVERT(NUMERIC(15,2),NULL) AMOUNT,IM.METALID "
                mStrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE RS "
                mStrSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..RECEIPT R  ON R.SNO=RS.ISSSNO AND R.BATCHNO=RS.BATCHNO  "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = RS.STNITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = RS.STNSUBITEMID"
                mStrSql += vbCrLf + " WHERE R.TRANTYPE IN('RRE','RPU') AND ACCODE='" & _Accode & "' AND RS.JOBISNO='" & JObno & "' AND ISNULL(CANCEL,'')=''"
                mStrSql += vbCrLf + " GROUP BY RS.JOBISNO,IM.ITEMNAME,SI.SUBITEMNAME,RS.STONEUNIT,CALCMODE,IM.METALID"
                mStrSql += vbCrLf + " )X GROUP BY DESCRIP,JOBNO,ITEM,SUBITEM,UNIT,CALC,RATE,AMOUNT,METALID HAVING SUM(STNWT)<>0 "
            ElseIf oMaterial = Material.Receipt Then
                mStrSql = vbCrLf + " SELECT DESCRIP,ITEM,SUBITEM,JOBNO,SUM(STNPCS)STNPCS,SUM(STNWT) STNWT,UNIT,CALC,RATE,AMOUNT,METALID,RESULT  FROM"
                mStrSql += vbCrLf + " ("
                mStrSql += vbCrLf + " SELECT 'ISSUE' DESCRIP,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM,JOBISNO JOBNO"
                mStrSql += vbCrLf + " ,SUM(ISNULL(STNPCS,0)) STNPCS,SUM(ISNULL(ISS.STNWT,0)) STNWT,ISS.STONEUNIT UNIT,ISS.CALCMODE CALC"
                mStrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,CONVERT(NUMERIC(15,2),NULL) AMOUNT,IM.METALID,0 AS RESULT FROM " & cnStockDb & "..ISSSTONE ISS "
                mStrSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSUE I ON I.SNO=ISS.ISSSNO AND I.BATCHNO=ISS.BATCHNO"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = ISS.STNITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = ISS.STNSUBITEMID"
                mStrSql += vbCrLf + " WHERE I.TRANTYPE IN('IIS','IPU','IRC','ISS') AND ACCODE='" & _Accode & "' AND ISS.JOBISNO='" & JObno & "'  AND ISNULL(CANCEL,'')=''"
                mStrSql += vbCrLf + " GROUP BY JOBISNO,IM.ITEMNAME,SI.SUBITEMNAME,ISS.STONEUNIT,CALCMODE,IM.METALID "
                mStrSql += vbCrLf + " UNION ALL"
                mStrSql += vbCrLf + " SELECT 'RECEIPT' DESCRIP,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " ,RS.JOBISNO JOBNO,SUM(ISNULL(STNPCS,0))*(-1) STNPCS,SUM(ISNULL(RS.STNWT,0))*(-1) STNWT,RS.STONEUNIT UNIT,CALCMODE CALC"
                mStrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,CONVERT(NUMERIC(15,2),NULL) AMOUNT,IM.METALID,0 AS RESULT "
                mStrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE RS "
                mStrSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..RECEIPT R  ON R.SNO=RS.ISSSNO AND R.BATCHNO=RS.BATCHNO  "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = RS.STNITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = RS.STNSUBITEMID"
                mStrSql += vbCrLf + " WHERE R.TRANTYPE IN('RRE','RPU') AND ACCODE='" & _Accode & "' AND RS.JOBISNO='" & JObno & "' AND ISNULL(CANCEL,'')=''"
                mStrSql += vbCrLf + " GROUP BY RS.JOBISNO,IM.ITEMNAME,SI.SUBITEMNAME,RS.STONEUNIT,CALCMODE,IM.METALID"

                mStrSql += vbCrLf + " UNION ALL"
                mStrSql += vbCrLf + " SELECT 'RECEIPT' DESCRIP,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM,JOBNO JOBNO"
                mStrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) STNPCS,SUM(ISNULL(I.GRSWT,0)) STNWT,'G' UNIT,'W' CALC"
                mStrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,CONVERT(NUMERIC(15,2),NULL) AMOUNT,IM.METALID,1 AS RESULT "
                mStrSql += vbCrLf + " from " & cnStockDb & "..ISSUE I"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.iTEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                mStrSql += vbCrLf + " WHERE I.TRANTYPE IN('IIS','IPU','IRC','ISS') "
                mStrSql += vbCrLf + " AND ACCODE='" & _Accode & "' AND I.JOBNO='" & JObno & "' "
                mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''   AND I.METALID IN ('T') "
                mStrSql += vbCrLf + " GROUP BY JOBNO,IM.ITEMNAME,SI.SUBITEMNAME,IM.METALID "
                mStrSql += vbCrLf + " UNION ALL"
                mStrSql += vbCrLf + " SELECT 'ISSUE' DESCRIP,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM,JOBNO JOBNO"
                mStrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) STNPCS,SUM(ISNULL(I.GRSWT,0)) STNWT,'G' UNIT,'W' CALC"
                mStrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,CONVERT(NUMERIC(15,2),NULL) AMOUNT,IM.METALID,1 AS RESULT "
                mStrSql += vbCrLf + " from " & cnStockDb & "..RECEIPT I"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.iTEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                mStrSql += vbCrLf + " WHERE I.TRANTYPE IN('RRE','RPU')  AND I.METALID IN ('T') "
                mStrSql += vbCrLf + " AND ACCODE='" & _Accode & "' AND I.JOBNO='" & JObno & "' "
                mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"

                mStrSql += vbCrLf + " GROUP BY JOBNO,IM.ITEMNAME,SI.SUBITEMNAME,IM.METALID "
                mStrSql += vbCrLf + " )X GROUP BY DESCRIP,JOBNO,ITEM,SUBITEM,UNIT,CALC,RATE,AMOUNT,METALID,RESULT HAVING SUM(STNWT)<>0 "
            End If
            Dim dtJob As New DataTable
            cmd = New OleDbCommand(mStrSql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtJob)
showjobs:
            Dim JnoRow As DataRow
            If dtJob.Rows.Count > 0 Then
                JnoRow = BrighttechPack.SearchDialog.Show_R("Find ", mStrSql, cn, , 1, , , , , , False)
            End If
            If Not JnoRow Is Nothing Then
                Dim Assignval As Boolean = True
                If oMaterial = Material.Receipt And JnoRow.Item(0) = "RECEIPT" And JnoRow.Item("RESULT") = "0" Then
                    If MsgBox("Further Receipt add to this Job No", MsgBoxStyle.YesNo) = MsgBoxResult.No Then GoTo showjobs Else Assignval = False
                End If
                If oMaterial = Material.Issue And JnoRow.Item(0) = "ISSUE" And JnoRow.Item("RESULT") = "0" Then
                    If MsgBox("Further Issue add to this Job No", MsgBoxStyle.YesNo) = MsgBoxResult.No Then GoTo showjobs Else Assignval = False
                End If

                Me.Refresh()
                Dim sno As String = ""
                Dim ro As DataRow = Nothing
                ro = dtGridStone.NewRow
                
                ro("ITEM") = JnoRow.Item("ITEM").ToString
                ro("SUBITEM") = JnoRow.Item("SUBITEM").ToString
                ro("STNTYPE") = JnoRow.Item("DESCRIP").ToString
                'If Assignval Then
                '    ro("PCS") = Val(dtJob.Compute("SUM(STNPCS)", Nothing).ToString)
                '    ro("WEIGHT") = Val(dtJob.Compute("SUM(STNWT)", Nothing).ToString)
                'Else
                ro("PCS") = Val(JnoRow.Item("STNPCS").ToString)
                ro("WEIGHT") = Val(JnoRow.Item("STNWT").ToString)
                'End If
                ro("UNIT") = JnoRow.Item("UNIT").ToString
                ro("CALC") = JnoRow.Item("CALC").ToString
                ro("RATE") = Val(JnoRow.Item("RATE").ToString)
                ro("AMOUNT") = Val(JnoRow.Item("AMOUNT").ToString)
                ro("METALID") = JnoRow.Item("METALID").ToString
                ro("ISSSNO") = JObno
                dtGridStone.Rows.Add(ro)

                dtGridStone.AcceptChanges()
                gridStone.CurrentCell = gridStone.Rows(gridStone.RowCount - 1).Cells("ITEM")
                CalcStoneWtAmount()
                StyleGridStone(gridStone)
                StyleGridStone(gridStoneTotal)
                objGPack.TextClear(grpStone)
            Else

            End If
        End If
    End Sub

    Private Sub cmbStUnit_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStUnit.Leave
        If STOCKVALIDATION = False Then Exit Sub
        If oMaterial = Material.Receipt Then Exit Sub
        If Val(txtEditWet.Text) <> 0 Then
            If txtEditUnit.Text = cmbStUnit.Text Then
                If Val(txtStWeight.Text) > Val(txtEditWet.Text) Then
                    If oMaterial = Material.Issue Then
                        MsgBox("Issue Weight Should not Exceed the Receipt Weight", MsgBoxStyle.Information)
                    Else
                        MsgBox("Receipt Weight Should not Exceed the Issue Weight", MsgBoxStyle.Information)
                    End If
                    txtStWeight.Text = txtEditWet.Text
                    txtStWeight.Select()
                End If
            Else
                If txtEditUnit.Text = "G" And cmbStUnit.Text = "C" Then
                    If Val(txtStWeight.Text) > Val(txtEditWet.Text) * 5 Then
                        If oMaterial = Material.Issue Then
                            MsgBox("Issue Weight Should not Exceed the Receipt Weight", MsgBoxStyle.Information)
                        Else
                            MsgBox("Receipt Weight Should not Exceed the Issue Weight", MsgBoxStyle.Information)
                        End If
                        txtStWeight.Text = txtEditWet.Text
                        txtStWeight.Select()
                    End If
                ElseIf txtEditUnit.Text = "C" And cmbStUnit.Text = "G" Then
                    If Val(txtStWeight.Text) > Val(txtEditWet.Text) / 5 Then
                        If oMaterial = Material.Issue Then
                            MsgBox("Issue Weight Should not Exceed the Receipt Weight", MsgBoxStyle.Information)
                        Else
                            MsgBox("Receipt Weight Should not Exceed the Issue Weight", MsgBoxStyle.Information)
                        End If
                        txtStWeight.Text = txtEditWet.Text
                        txtStWeight.Select()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub cmbStUnit_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStUnit.SelectedValueChanged
        CalcStoneAmount()
    End Sub

    Private Sub cmbStCalc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStCalc.SelectedIndexChanged
        CalcStoneAmount()
    End Sub

    Private Sub cmbItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbItem.KeyDown, cmbSubItem.KeyDown
        If e.KeyCode = Keys.Enter Then
            If CType(sender, ComboBox).FindStringExact(CType(sender, ComboBox).SelectedText) <> -1 Then
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub cmbItem_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem.Leave
        If cmbItem.Text <> "" Then LoadSubItem(cmbSubItem)
        Me.SelectNextControl(cmbItem, True, True, True, True)
    End Sub
End Class