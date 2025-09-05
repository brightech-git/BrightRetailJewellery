Imports System.Data.OleDb
Public Class frmStoneDia
    Public dtGridStone As New DataTable
    Dim strSql As String
    Public _Accode As String = Nothing
    Public grsWt As Double = Nothing
    Public _DiaPcs As Integer = Nothing
    Public _DiaWt As Double = Nothing
    Public _EditLock As Boolean = False
    Public _DelLock As Boolean = False
    Public _Authorize As Boolean = False
    Public RootItemid As Integer = 0
    Public RootSItemid As Integer = 0
    Dim StudWtDedut As String
    Public PacketNo As String = Nothing
    Public PacketItemId As Integer = Nothing
    Public FromFlag As String = Nothing
    Public SHOWTRANNO As Boolean = False
    Dim objMultiSelect As MultiSelectRowDia = Nothing
    Dim dtGrid As New DataTable
    Dim rateRevcal As String
    Dim _JobNoEnable As Boolean = IIf(GetAdmindbSoftValue("MRMIJOBNO", "N") = "Y", True, False)
    Public _Designerid As Integer = 0
    Dim CENTRATE_DESIGNER As Boolean = IIf(GetAdmindbSoftValue("CENTRATE_DES", "Y") = "Y", True, False)
    Dim customertype As String = Nothing
    Dim _FourCMaintain As Boolean = IIf(GetAdmindbSoftValue("4CMAINTAIN", "Y") = "Y", True, False)
    Dim Cut As String = ""
    Dim Color As String = ""
    Dim Clarity As String = ""
    Dim Shape As String = ""
    Dim SetType As String = ""
    Dim StnWidth As Decimal = 0
    Dim StnHeight As Decimal = 0
    Dim StnGroup As String = ""
    Dim stnGrpId As Integer = 0
    Public ObjDiaDetails As New frmDiamondDetails
    Public Own As Boolean = False
    Public CashPurch As Boolean = False
    Public TagType As String = ""
    Public purItemName As String = ""
    Public purSubitemname As String = ""
    Dim STUDDIA_PCS_MAND_EST_POS As Boolean = IIf(GetAdmindbSoftValue("STUDDIA_PCS_MAND_EST_POS", "Y") = "Y", True, False)
    Dim POS_ENABLE_STNGRP As Boolean = IIf(GetAdmindbSoftValue("POS_ENABLE_STNGRP", "N") = "Y", True, False)
    Dim STUDWT_DECREASE As Boolean = IIf(GetAdmindbSoftValue("STUDWT_DECREASE", "N") = "Y", True, False)
    Dim RoleBasedDisc As String = GetAdmindbSoftValue("ROLEBASED_DISCOUNT", "N")
    Dim MetalBasedStone As Boolean = IIf(GetAdmindbSoftValue("MULTIMETALBASEDSTONE", "N") = "Y", True, False)
    Public _SalesReturn As Boolean = False
    Public _istag As Boolean = False
    Dim EXCESSSTUD As Boolean = IIf(GetAdmindbSoftValue("EXCESSSTUD_EST_POS", "N") = "Y", True, False)
    Dim StnEditName As Boolean = IIf(GetAdmindbSoftValue("STUDDEDSTNNAME_EDIT_EST_POS", "Y") = "Y", True, False)

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

        txtTranno.Visible = SHOWTRANNO

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        Me.BackColor = frmBackColor
        objGPack.TextClear(Me)
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
            .Add("KEYNO", GetType(Integer))
            .Add("TRANTYPE", GetType(String))
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
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
            .Add("CUTID", GetType(Integer))
            .Add("COLORID", GetType(Integer))
            .Add("CLARITYID", GetType(Integer))
            .Add("SHAPEID", GetType(Integer))
            .Add("SETTYPEID", GetType(Integer))
            .Add("HEIGHT", GetType(Integer))
            .Add("WIDTH", GetType(Integer))
            .Add("SGST", GetType(Decimal))
            .Add("CGST", GetType(Decimal))
            .Add("IGST", GetType(Decimal))
            .Add("CESS", GetType(Decimal))
            If MetalBasedStone Then
                .Add("TAGMSNO", GetType(String))
            End If
            .Add("STNGRPID", GetType(Integer))
            .Add("ORGAMOUNT", GetType(Double))
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
        Dim dirlesswt As Double = 0


        _DiaPcs = 0
        _DiaWt = 0

        If gridStone.RowCount = 0 Then
            If gridStoneTotal.RowCount > 0 Then
                gridStoneTotal.Rows(0).Cells("WEIGHT").Value = DBNull.Value
                gridStoneTotal.Rows(0).Cells("AMOUNT").Value = DBNull.Value
            End If
            Exit Sub
        End If
        Dim Isdeductstud As Boolean = False
        For cnt As Integer = 0 To gridStone.RowCount - 1
            With gridStone.Rows(cnt)

                strSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'"
                Dim STONETYPE As String = objGPack.GetSqlValue(strSql).ToUpper

                If StudWtDedut = "I" Then
                    If .Cells("SUBITEM").Value.ToString <> "" Then
                        strSql = " SELECT STUDDEDUCT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Cells("SUBITEM").Value.ToString & "'"
                        strSql += " AND ITEMID IN (SELECT TOP 1 ITEMID  FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "')"
                    Else
                        strSql = " SELECT STUDDEDUCT  FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'"
                    End If
                    Isdeductstud = IIf(objGPack.GetSqlValue(strSql).ToString.ToUpper = "Y", True, False)
                End If

                Select Case STONETYPE  '.Cells("METALID").Value.ToString
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
                        If STONETYPE = "S" Then
                            stoGramWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf STONETYPE = "P" Then
                            preGramWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf STONETYPE = "D" Then
                            diaGramWt += Val(.Cells("WEIGHT").Value.ToString)
                            _DiaWt += Val(.Cells("WEIGHT").Value.ToString)
                        End If
                        If Isdeductstud Then dirlesswt += Val(.Cells("weight").Value.ToString)
                    Case "C"
                        If STONETYPE = "S" Then
                            stoCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf STONETYPE = "P" Then
                            preCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf STONETYPE = "D" Then
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
    End Sub
    Public Sub StyleGridStone(ByVal gridStone As DataGridView)
        With gridStone
            .Columns("KEYNO").Visible = False
            .Columns("TRANTYPE").Visible = False
            .Columns("METALID").Visible = False
            .Columns("ITEM").Width = txtStItem.Width + 1
            .Columns("SUBITEM").Width = txtStSubItem.Width + 1
            .Columns("PCS").Width = txtStPcs_NUM.Width + 1
            .Columns("UNIT").Width = cmbStUnit.Width + 1
            .Columns("CALC").Width = cmbStCalc.Width + 1
            .Columns("WEIGHT").Width = txtStWeight.Width + 1
            .Columns("RATE").Width = txtStRate_AMT.Width + 1
            .Columns("AMOUNT").Width = txtStAmount_AMT.Width + 1
            .Columns("DISCOUNT").Visible = False
            .Columns("TAGSTNPCS").Visible = False
            .Columns("TAGSTNWT").Visible = False
            .Columns("TAGSNO").Visible = False
            .Columns("R_VAT").Visible = False
            .Columns("CUTID").Visible = False
            .Columns("COLORID").Visible = False
            .Columns("CLARITYID").Visible = False
            .Columns("SHAPEID").Visible = False
            .Columns("SETTYPEID").Visible = False
            .Columns("HEIGHT").Visible = False
            .Columns("WIDTH").Visible = False
            If .Columns.Contains("ISSSNO") = True Then .Columns("ISSSNO").Visible = False
            .Columns("SGST").Visible = False
            .Columns("CGST").Visible = False
            .Columns("IGST").Visible = False
            .Columns("STNGRPID").Visible = False
            If MetalBasedStone Then
                .Columns("TAGMSNO").Visible = False
            End If
        End With
    End Sub

    Private Sub frmStoneDia_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            dtGridStone.AcceptChanges()
            txtStItem.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmStoneDia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridStone.Focused Then Exit Sub
            If txtStItem.Focused Then Exit Sub
            If txtStSubItem.Focused Then Exit Sub
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
            If STUDWT_DECREASE = False Then CheckStoneWeight()
            ''Validation
            'If GetCalcType() = "R" Then Exit Sub
            'If isTag Then
            '    If Not lckPartlySale Then Exit Sub
            'End If
            ''Validation
            If objGPack.Validator_Check(grpStone) Then Exit Sub
            If objGPack.DupCheck("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "' AND  STUDDED IN('S','B') AND ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Item", MsgBoxStyle.Information)
                txtStItem.Focus()
                Exit Sub
            End If
            If UCase(objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "' AND STUDDED IN('S','B') AND ACTIVE = 'Y'")) = "Y" Then
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
            If Val(txtStPcs_NUM.Text) = 0 And Val(txtStWeight.Text) = 0 And Val(txtStAmount_AMT.Text) = 0 Then
                MsgBox(Me.GetNextControl(txtStPcs_NUM, False).Text + "," + Me.GetNextControl(txtStWeight, False).Text + E0001, MsgBoxStyle.Information)
                txtStItem.Focus()
                Exit Sub
            End If

            If validStonepcs() = False Then
                txtStPcs_NUM.Focus()
                txtStPcs_NUM.SelectAll()
                Exit Sub
            End If

            If txtStRowIndex.Text = "" And _istag And EXCESSSTUD = False Then
                MsgBox("Not able to add Extra studded Detail", MsgBoxStyle.Information)
                Exit Sub
            End If

            ''NEWLY ADDED [02-AUG-2021] For Ngopaldas
            If RoleBasedDisc.ToString = "Y" And txtStRowIndex.Text <> "" Then
                Dim stTagsno As String = ""
                stTagsno = gridStone.Rows(Val(txtStRowIndex.Text)).Cells("TAGSNO").Value.ToString
                Dim msubitemid As Integer = 0
                Dim mitemid As Integer = 0
                Dim mstnmetal As String = ""
                strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = (SELECT TAGSNO FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE SNO='" & stTagsno & "')"
                mitemid = Val(objGPack.GetSqlValue(strSql).ToString)

                strSql = " select subitemid from " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = '" & mitemid & "'"
                strSql += " AND SUBITEMID =(SELECT SUBITEMID FROM " & cnAdminDb & "..ITEMTAG WHERE "
                strSql += " SNO = (SELECT TAGSNO FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE SNO='" & stTagsno & "'))"
                msubitemid = Val(objGPack.GetSqlValue(strSql).ToString)

                strSql = " SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = (SELECT STNITEMID FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE SNO='" & stTagsno & "')"
                mstnmetal = objGPack.GetSqlValue(strSql).ToString

                strSql = " select count(*) CNT from " & cnAdminDb & "..VACONTROL where VATYPE = 'D' AND Isnull(COSTID,'') ='" & cnCostId & "' "
                strSql += " And itemid = " & mitemid
                strSql += " and subitemid = " & msubitemid
                strSql += " and isnull(designerid,0) =0"
                strSql += " and isnull(ROLEID,0) = (SELECT TOP 1 ISNULL(ROLEID,0) FROM " & cnAdminDb & "..USERROLE WHERE USERID='" & userId & "') "
                Dim chkexist As Integer = Val(GetSqlValue(cn, strSql))
                If chkexist <> 0 And mstnmetal.ToString = "D" Then ''Diamond
                    Dim DrTag As DataRow
                    Dim OrgStnamt As Double
                    If txtStRowIndex.Text <> "" Then
                        strSql = "select stnamt from " & cnAdminDb & "..itemtagstone where SNO = '" & stTagsno.ToString & "'"
                        DrTag = GetSqlRow(strSql, cn)
                        If Not DrTag Is Nothing Then
                            OrgStnamt = Math.Round(Val(DrTag.Item("stnamt").ToString), 2)
                        End If
                    End If

                    strSql = " select DIAAMTPER from " & cnAdminDb & "..VACONTROL where VATYPE = 'D' AND Isnull(COSTID,'') ='" & cnCostId & "' "
                    strSql += " And itemid = " & mitemid
                    strSql += " and subitemid = " & msubitemid
                    strSql += " and isnull(designerid,0) =0"
                    strSql += " and isnull(ROLEID,0) = (SELECT TOP 1 ISNULL(ROLEID,0) FROM " & cnAdminDb & "..USERROLE WHERE USERID='" & userId & "') "
                    Dim drdisc As DataRow = GetSqlRow(strSql, cn)
                    Dim VADiaDiscAmt As Decimal
                    If drdisc Is Nothing Then
                        VADiaDiscAmt = 0
                    Else
                        VADiaDiscAmt = Math.Round(OrgStnamt * (Val(drdisc.Item("DIAAMTPER").ToString) / 100), 2)
                    End If
                    Dim cDiaAmt As Decimal = 0
                    cDiaAmt = Val(txtStAmount_AMT.Text)
                    If cDiaAmt < Val(IIf(VADiaDiscAmt > OrgStnamt, OrgStnamt, OrgStnamt - VADiaDiscAmt)) And Val(VADiaDiscAmt.ToString) <> 0 Then
                        MsgBox("Given Diamond Amount is Less Than Allowed Discount Amount" _
                                & vbCrLf & " Current Discount :" & Val(OrgStnamt - cDiaAmt) _
                                & vbCrLf & " Allowed Discount :" & Val(VADiaDiscAmt) _
                               , MsgBoxStyle.Information, "RoleBased DiaAmt% Discount")
                        txtStAmount_AMT.Text = OrgStnamt
                        txtStAmount_AMT.Focus()
                        Exit Sub
                    End If

                End If
                If chkexist <> 0 And mstnmetal.ToString = "T" Then  ''STONE
                    Dim DrTag As DataRow
                    Dim OrgStnamt As Double
                    If txtStRowIndex.Text <> "" Then
                        strSql = "select stnamt from " & cnAdminDb & "..itemtagstone where SNO = '" & stTagsno.ToString & "'"
                        DrTag = GetSqlRow(strSql, cn)
                        If Not DrTag Is Nothing Then
                            OrgStnamt = Math.Round(Val(DrTag.Item("stnamt").ToString), 2)
                        End If
                    End If

                    strSql = " SELECT STNAMTPER from " & cnAdminDb & "..VACONTROL where VATYPE = 'D' AND Isnull(COSTID,'') ='" & cnCostId & "' "
                    strSql += " And itemid = " & mitemid
                    strSql += " and subitemid = " & msubitemid
                    strSql += " and isnull(designerid,0) =0"
                    strSql += " and isnull(ROLEID,0) = (SELECT TOP 1 ISNULL(ROLEID,0) FROM " & cnAdminDb & "..USERROLE WHERE USERID='" & userId & "') "
                    Dim drdisc As DataRow = GetSqlRow(strSql, cn)
                    Dim VADiaDiscAmt As Decimal
                    If drdisc Is Nothing Then
                        VADiaDiscAmt = 0
                    Else
                        VADiaDiscAmt = Math.Round(OrgStnamt * (Val(drdisc.Item("STNAMTPER").ToString) / 100), 2)
                    End If
                    Dim cDiaAmt As Decimal = 0
                    cDiaAmt = Val(txtStAmount_AMT.Text)
                    If cDiaAmt < Val(IIf(VADiaDiscAmt > OrgStnamt, OrgStnamt, OrgStnamt - VADiaDiscAmt)) And Val(VADiaDiscAmt.ToString) <> 0 Then
                        MsgBox("Given Stone Amount is Less Than Allowed Discount Amount" _
                                    & vbCrLf & " Current Discount :" & Val(OrgStnamt - cDiaAmt) _
                                    & vbCrLf & " Allowed Discount :" & Val(VADiaDiscAmt) _
                                   , MsgBoxStyle.Information, "RoleBased DiaAmt% Discount")
                        txtStAmount_AMT.Text = OrgStnamt
                        txtStAmount_AMT.Focus()
                        Exit Sub
                    End If
                End If
            End If
            ''END NEWLY ADDED [02-AUG-2021] For Ngopaldas


            Dim cent As Double = Nothing
            Dim first As Boolean = True
Second:
            If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))) Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))
            cent *= 100
            strSql = " DECLARE @CENT FLOAT"
            strSql += " SET @CENT = " & cent & ""
            strSql += " SELECT MINRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            strSql += " @CENT BETWEEN FROMCENT AND TOCENT "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')),0)"
            If first = True Then strSql += " AND ISNULL(ACCODE,'') = '" & _Accode & "'" Else strSql += " AND ISNULL(ACCODE,'') = ''"
            If CENTRATE_DESIGNER Then strSql += " AND ISNULL(DESIGNERID,0)=" & _Designerid & ""
            If _FourCMaintain Then

                Dim ColorId As Integer = 0
                Dim CutId As Integer = 0
                Dim ClarityId As Integer = 0
                Dim stnGrpId As Integer = 0
                ColorId = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & ObjDiaDetails.CmbColor.Text & "'", "COLORID", 0)
                CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & ObjDiaDetails.CmbCut.Text & "'", "CUTID", 0)
                ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & ObjDiaDetails.CmbClarity.Text & "'", "CLARITYID", 0)
                stnGrpId = objGPack.GetSqlValue("SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & ObjDiaDetails.cmbStnGrp.Text & "'", "GROUPID", 0)
                strSql += vbCrLf + " AND ISNULL(COLORID,0)=" & ColorId
                strSql += vbCrLf + " AND ISNULL(CUTID,0)=" & CutId
                strSql += vbCrLf + " AND ISNULL(CLARITYID,0)=" & ClarityId
                If POS_ENABLE_STNGRP Then
                    strSql += vbCrLf + " AND ISNULL(STNGRPID,0)=" & stnGrpId
                End If
            End If
            Dim rate As Double = Val(objGPack.GetSqlValue(strSql, , , tran))
            If rate = 0 And first Then first = False : GoTo Second
            If Val(txtStRate_AMT.Text) < rate Then
                'If _Authorize Then
                If Ogstnrateedit = True Or _Authorize Then
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
                ro("ITEM") = txtStItem.Text
                ro("SUBITEM") = txtStSubItem.Text
                ro("PCS") = IIf(Val(txtStPcs_NUM.Text) <> 0, Val(txtStPcs_NUM.Text), DBNull.Value)
                ro("UNIT") = cmbStUnit.Text
                ro("CALC") = cmbStCalc.Text
                ro("WEIGHT") = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), DBNull.Value)
                ro("RATE") = IIf(Val(txtStRate_AMT.Text) <> 0, Format(Val(txtStRate_AMT.Text), "0.00"), DBNull.Value)
                ro("AMOUNT") = IIf(Val(txtStAmount_AMT.Text) <> 0, Format(Val(txtStAmount_AMT.Text), "0.00"), DBNull.Value)

                ro("METALID") = txtStMetalCode.Text
                If _JobNoEnable = True Then
                    ro("ISSSNO") = txtTranno.Text
                    jobno = txtTranno.Text
                End If
                If _FourCMaintain Then
                    ro("CUTID") = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & ObjDiaDetails.CmbCut.Text & "'", "CUTID", , tran))
                    ro("COLORID") = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & ObjDiaDetails.CmbColor.Text & "'", "COLORID", , tran))
                    ro("CLARITYID") = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & ObjDiaDetails.CmbClarity.Text & "'", "CLARITYID", , tran))
                    ro("SHAPEID") = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & ObjDiaDetails.cmbShape.Text & "'", "SHAPEID", , tran))
                    ro("SETTYPEID") = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & ObjDiaDetails.cmbSetType.Text & "'", "SETTYPEID", , tran))
                    ro("HEIGHT") = Val(ObjDiaDetails.txtHeight_WET.Text)
                    ro("WIDTH") = Val(ObjDiaDetails.txtWidth_WET.Text)
                    ro("STNGRPID") = Val(objGPack.GetSqlValue("SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & ObjDiaDetails.cmbStnGrp.Text & "'", "GROUPID", , tran))
                End If
                dtGridStone.Rows.Add(ro)
            Else
                With gridStone.Rows(Val(txtStRowIndex.Text))
                    .Cells("ITEM").Value = txtStItem.Text
                    .Cells("SUBITEM").Value = txtStSubItem.Text
                    .Cells("PCS").Value = IIf(Val(txtStPcs_NUM.Text) <> 0, Val(txtStPcs_NUM.Text), DBNull.Value)
                    .Cells("UNIT").Value = cmbStUnit.Text
                    .Cells("CALC").Value = cmbStCalc.Text
                    .Cells("WEIGHT").Value = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), DBNull.Value)
                    .Cells("RATE").Value = IIf(Val(txtStRate_AMT.Text) <> 0, Format(Val(txtStRate_AMT.Text), "0.00"), DBNull.Value)
                    .Cells("AMOUNT").Value = IIf(Val(txtStAmount_AMT.Text) <> 0, Format(Val(txtStAmount_AMT.Text), "0.00"), DBNull.Value)
                    .Cells("METALID").Value = txtStMetalCode.Text
                    If _JobNoEnable = True Then
                        .Cells("ISSSNO").Value = txtTranno.Text
                        jobno = txtTranno.Text
                    End If
                    If _FourCMaintain Then
                        .Cells("CUTID").Value = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & ObjDiaDetails.CmbCut.Text & "'", "CUTID", , tran))
                        .Cells("COLORID").Value = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & ObjDiaDetails.CmbColor.Text & "'", "COLORID", , tran))
                        .Cells("CLARITYID").Value = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & ObjDiaDetails.CmbClarity.Text & "'", "CLARITYID", , tran))
                        .Cells("SHAPEID").Value = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & ObjDiaDetails.cmbShape.Text & "'", "SHAPEID", , tran))
                        .Cells("SETTYPEID").Value = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & ObjDiaDetails.cmbSetType.Text & "'", "SETTYPEID", , tran))
                        .Cells("HEIGHT").Value = Val(ObjDiaDetails.txtHeight_WET.Text)
                        .Cells("WIDTH").Value = Val(ObjDiaDetails.txtWidth_WET.Text)
                        .Cells("STNGRPID").Value = Val(objGPack.GetSqlValue("SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & ObjDiaDetails.cmbStnGrp.Text & "'", "GROUPID", , tran))
                    End If
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
            txtTranno.Text = jobno
            'txtStPcs_NUM.Clear()
            'txtStWeight_WET.Clear()
            'txtStRate_AMT.Clear()
            'txtStAmount_AMT.Clear()
            'txtStMetalCode.Clear()
            Cut = ""
            Color = ""
            Clarity = ""
            Shape = ""
            SetType = ""
            StnWidth = 0
            StnHeight = 0
            stnGrpId = 0
            txtStItem.Focus()
        End If
    End Sub
    Private Sub txtStPcs_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStPcs_NUM.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridStone.RowCount > 0 Then gridStone.Select()
        End If
    End Sub

    Private Sub txtStPcs_NUM_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStPcs_NUM.Leave
        Dim view4c As String = ""
        Dim maintain4c As Boolean
        strSql = "SELECT ISNULL(MAINTAIN4C,'N')AS MAINTAIN4C ,VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "' AND METALID='D'"
        Dim dr As DataRow
        dr = GetSqlRow(strSql, cn, tran)
        If Not dr Is Nothing Then
            maintain4c = IIf(dr("MAINTAIN4C").ToString = "Y", True, False)
            view4c = dr("VIEW4C").ToString
        End If
        strSql = "SELECT ISNULL(MAINTAIN4C,'N')AS MAINTAIN4C,VIEW4C  FROM " & cnAdminDb & "..SUBITEMMAST "
        strSql += " WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' "
        strSql += " AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtStItem.Text & "')"
        dr = GetSqlRow(strSql, cn, tran)
        If Not dr Is Nothing Then
            maintain4c = IIf(dr("MAINTAIN4C").ToString = "Y", True, False)
            view4c = dr("VIEW4C").ToString
        End If
        If _FourCMaintain And maintain4c Then
            ObjDiaDetails = New frmDiamondDetails
            ObjDiaDetails.cmbStnGrp.Text = StnGroup
            ObjDiaDetails.CmbCut.Text = Cut
            ObjDiaDetails.CmbColor.Text = Color
            ObjDiaDetails.CmbClarity.Text = Clarity
            ObjDiaDetails.cmbShape.Text = Shape
            ObjDiaDetails.cmbSetType.Text = SetType
            ObjDiaDetails.txtWidth_WET.Text = StnWidth
            ObjDiaDetails.txtHeight_WET.Text = StnHeight
            If Not view4c.Contains("CU") Then ObjDiaDetails.CmbCut.Enabled = False
            If Not view4c.Contains("CO") Then ObjDiaDetails.CmbColor.Enabled = False
            If Not view4c.Contains("CL") Then ObjDiaDetails.CmbClarity.Enabled = False
            If Not view4c.Contains("SH") Then ObjDiaDetails.cmbShape.Enabled = False
            If Not view4c.Contains("SE") Then ObjDiaDetails.cmbSetType.Enabled = False
            If Not view4c.Contains("HE") Then ObjDiaDetails.txtHeight_WET.Enabled = False
            If Not view4c.Contains("WI") Then ObjDiaDetails.txtWidth_WET.Enabled = False
            ObjDiaDetails.CmbCut.Focus()
            ObjDiaDetails.Location = New Point(ScreenWid / 2, ((ScreenHit + 128) - grpStone.Height) / 2)
            ObjDiaDetails.ShowDialog()
            StnGroup = ObjDiaDetails.cmbStnGrp.Text
            Cut = ObjDiaDetails.CmbCut.Text
            Color = ObjDiaDetails.CmbColor.Text
            Clarity = ObjDiaDetails.CmbClarity.Text
            Shape = ObjDiaDetails.cmbShape.Text
            SetType = ObjDiaDetails.cmbSetType.Text
            StnWidth = ObjDiaDetails.txtWidth_WET.Text
            StnHeight = ObjDiaDetails.txtHeight_WET.Text
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
            If STUDWT_DECREASE = False Then CheckStoneWeight()
            Dim cent As Double = 0
            Dim first As Boolean = True
Second:
            If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))) Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))
            cent *= 100
            strSql = " DECLARE @CENT FLOAT"
            strSql += " SET @CENT = " & cent & ""
            strSql += " SELECT MINRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            strSql += " @CENT BETWEEN FROMCENT AND TOCENT "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')),0)"
            If first = True Then strSql += " AND ISNULL(ACCODE,'') = '" & _Accode & "'" Else strSql += " AND ISNULL(ACCODE,'') = ''"
            If CENTRATE_DESIGNER Then strSql += " AND ISNULL(DESIGNERID,0)=" & _Designerid & ""
            If _FourCMaintain Then
                Dim ColorId As Integer = 0
                Dim CutId As Integer = 0
                Dim ClarityId As Integer = 0
                Dim StnGrpId As Integer = 0
                ColorId = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & ObjDiaDetails.CmbColor.Text & "'", "COLORID", 0)
                CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & ObjDiaDetails.CmbCut.Text & "'", "CUTID", 0)
                ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & ObjDiaDetails.CmbClarity.Text & "'", "CLARITYID", 0)
                StnGrpId = objGPack.GetSqlValue("SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & ObjDiaDetails.cmbStnGrp.Text & "'", "GROUPID", 0)
                strSql += vbCrLf + " AND ISNULL(COLORID,0)=" & ColorId
                strSql += vbCrLf + " AND ISNULL(CUTID,0)=" & CutId
                strSql += vbCrLf + " AND ISNULL(CLARITYID,0)=" & ClarityId
                If POS_ENABLE_STNGRP Then
                    strSql += vbCrLf + " AND ISNULL(STNGRPID,0)=" & StnGrpId
                End If
            End If
            Dim rate As Double = Val(objGPack.GetSqlValue(strSql, , , tran))
            If rate = 0 And first Then first = False : GoTo Second
            If Val(txtStRate_AMT.Text) < rate Then
                'If _Authorize Then
                If Ogstnrateedit = True Or _Authorize Then
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
            strSql += vbCrLf + " AND STNITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            strSql += vbCrLf + " AND STNSUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'))"
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
        Dim first As Boolean = False
Second:
        If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))) Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))
        cent *= 100
        customertype = "C"
        strSql = " SELECT ACTYPE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE= '" & _Accode & "'"
        If _Accode <> "" Then customertype = objGPack.GetSqlValue(strSql, , "")
        Dim ColorId As Integer = 0
        Dim CutId As Integer = 0
        Dim ClarityId As Integer = 0
        If _FourCMaintain Then
            ColorId = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & ObjDiaDetails.CmbColor.Text & "'", "COLORID", 0)
            CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & ObjDiaDetails.CmbCut.Text & "'", "CUTID", 0)
            ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & ObjDiaDetails.CmbClarity.Text & "'", "CLARITYID", 0)
        End If
        If _IsWholeSaleType And customertype = "C" Then
            strSql = " DECLARE @CENT FLOAT"
            strSql += vbCrLf + "  SET @CENT = " & cent & ""
            strSql += vbCrLf + "  SELECT PURRATE+((SALESPER/100)*PURRATE) AS MAXRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            strSql += vbCrLf + "  @CENT BETWEEN FROMCENT AND TOCENT AND ISNULL(SALESPER,0)<>0"
            strSql += vbCrLf + "  AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            strSql += vbCrLf + "  AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')),0)"
            If first Then strSql += vbCrLf + "  AND ISNULL(ACCODE,'') = '" & _Accode & "'" Else strSql += vbCrLf + "  AND ISNULL(ACCODE,'') = ''"
            If CENTRATE_DESIGNER Then strSql += vbCrLf + "  AND ISNULL(DESIGNERID,0)=" & _Designerid & ""
            strSql += vbCrLf + " AND ISNULL(COLORID,0)=" & ColorId
            strSql += vbCrLf + " AND ISNULL(CUTID,0)=" & CutId
            strSql += vbCrLf + " AND ISNULL(CLARITYID,0)=" & ClarityId
        ElseIf _IsWholeSaleType And customertype = "D" Then
            strSql = " DECLARE @CENT FLOAT"
            strSql += vbCrLf + "  SET @CENT = " & cent & ""
            strSql += vbCrLf + "  SELECT MAXRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            strSql += vbCrLf + "  @CENT BETWEEN FROMCENT AND TOCENT "
            strSql += vbCrLf + "  AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            strSql += vbCrLf + "  AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')),0)"
            'If first Then strsql += VBCRLF + "  AND ISNULL(ACCODE,'') = '" & _Accode & "'" Else strsql += VBCRLF + "  AND ISNULL(ACCODE,'') = ''"
            If _Accode <> "" Then strSql += vbCrLf + "  AND ISNULL(ACCODE,'') = '" & _Accode & "'" Else strSql += vbCrLf + "  AND ISNULL(ACCODE,'') = ''"
            If CENTRATE_DESIGNER Then strSql += vbCrLf + "  AND ISNULL(DESIGNERID,0)=" & _Designerid & ""
            strSql += vbCrLf + " AND ISNULL(COLORID,0)=" & ColorId
            strSql += vbCrLf + " AND ISNULL(CUTID,0)=" & CutId
            strSql += vbCrLf + " AND ISNULL(CLARITYID,0)=" & ClarityId
        Else
            strSql = " DECLARE @CENT FLOAT"
            strSql += vbCrLf + "  SET @CENT = " & cent & ""
            strSql += vbCrLf + "  SELECT MAXRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            strSql += vbCrLf + "  @CENT BETWEEN FROMCENT AND TOCENT "
            strSql += vbCrLf + "  AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            strSql += vbCrLf + "  AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')),0)"
            If first Then strSql += vbCrLf + "  AND ISNULL(ACCODE,'') = '" & _Accode & "'" Else strSql += vbCrLf + "  AND ISNULL(ACCODE,'') = ''"
            If CENTRATE_DESIGNER Then strSql += vbCrLf + "  AND ISNULL(DESIGNERID,0)=" & _Designerid & ""
            strSql += vbCrLf + " AND ISNULL(COLORID,0)=" & ColorId
            strSql += vbCrLf + " AND ISNULL(CUTID,0)=" & CutId
            strSql += vbCrLf + " AND ISNULL(CLARITYID,0)=" & ClarityId
        End If

        Dim rate As Double = Val(objGPack.GetSqlValue(strSql, "MAXRATE", "", tran))
        If rate = 0 And first Then first = False : GoTo Second
        If TagType <> "" Then
            strSql = " SELECT OWNPURDIALESSPER,OWNPURDIALESSAMT,OWNEXDIALESSPER,OWNEXDIALESSAMT"
            strSql += " ,OTHPURDIALESSPER,OTHPURDIALESSAMT,OTHEXDIALESSPER,OTHEXDIALESSAMT"
            strSql += " FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME='" & TagType & "'"
            Dim Dr As DataRow
            Dr = GetSqlRow(strSql, cn, tran)
            If Not Dr Is Nothing Then
                Dim LessPer, LessAmt As Decimal
                If Own Then
                    If CashPurch Then
                        LessPer = Val(Dr("OWNPURDIALESSPER").ToString)
                        LessAmt = Val(Dr("OWNPURDIALESSAMT").ToString)
                    Else
                        LessPer = Val(Dr("OWNEXDIALESSPER").ToString)
                        LessAmt = Val(Dr("OWNEXDIALESSAMT").ToString)
                    End If
                Else
                    If CashPurch Then
                        LessPer = Val(Dr("OTHPURDIALESSPER").ToString)
                        LessAmt = Val(Dr("OTHPURDIALESSAMT").ToString)
                    Else
                        LessPer = Val(Dr("OTHEXDIALESSPER").ToString)
                        LessAmt = Val(Dr("OTHEXDIALESSAMT").ToString)
                    End If
                End If
                If LessPer <> 0 Then
                    rate -= Math.Round((rate * LessPer) / 100, 2)
                ElseIf LessAmt <> 0 Then
                    rate -= LessAmt
                End If
            End If
        End If
        'If rate <> 0 Then
        txtStRate_AMT.Text = IIf(rate <> 0, Format(rate, "0.00"), "")
        'End If
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
        txtStItem.ReadOnly = False
        If txtStItem.Text <> "" And StnEditName = False And txtStRowIndex.Text <> "" And _istag Then txtStItem.ReadOnly = True : SendKeys.Send("{TAB}") : Exit Sub
    End Sub

    Private Sub txtStItem_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStItem.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtStItem_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStItem.KeyDown
        If e.KeyCode = Keys.Insert Then
            txtStItem.ReadOnly = False
            If txtStItem.Text <> "" And StnEditName = False And txtStRowIndex.Text <> "" And _istag Then txtStItem.ReadOnly = True : SendKeys.Send("{TAB}") : Exit Sub
            LoadStoneItemName()
        ElseIf e.KeyCode = Keys.Down Then

            If gridStone.RowCount > 0 Then
                gridStone.Focus()
            End If
        End If
    End Sub

    Private Sub txtStItem_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStItem.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtStItem.ReadOnly = False
            If txtStItem.Text <> "" And StnEditName = False And txtStRowIndex.Text <> "" And _istag Then txtStItem.ReadOnly = True : SendKeys.Send("{TAB}") : Exit Sub
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
                txtStItem.Text = .Cells("ITEM").FormattedValue
                txtStSubItem.Text = .Cells("SUBITEM").FormattedValue
                txtStPcs_NUM.Text = .Cells("PCS").FormattedValue
                txtStWeight.Text = .Cells("WEIGHT").FormattedValue
                cmbStUnit.Text = .Cells("UNIT").FormattedValue
                cmbStCalc.Text = .Cells("CALC").FormattedValue
                txtStRate_AMT.Text = .Cells("RATE").FormattedValue
                txtStAmount_AMT.Text = .Cells("AMOUNT").FormattedValue
                stnGrpId = Val(.Cells("STNGRPID").FormattedValue)
                txtStMetalCode.Text = objGPack.GetSqlValue("SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'")
                StnGroup = objGPack.GetSqlValue("SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = '" & Val(.Cells("STNGRPID").FormattedValue) & "'")
                Cut = objGPack.GetSqlValue("SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = '" & Val(.Cells("CUTID").FormattedValue) & "'")
                Color = objGPack.GetSqlValue("SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID = '" & Val(.Cells("COLORID").FormattedValue) & "'")
                Clarity = objGPack.GetSqlValue("SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID = '" & Val(.Cells("CLARITYID").FormattedValue) & "'")
                Shape = objGPack.GetSqlValue("SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID = '" & Val(.Cells("SHAPEID").FormattedValue) & "'")
                SetType = objGPack.GetSqlValue("SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPEID = '" & Val(.Cells("SETTYPEID").FormattedValue) & "'")
                StnHeight = Val(.Cells("HEIGHT").FormattedValue)
                StnWidth = Val(.Cells("WIDTH").FormattedValue)
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

    Private Sub frmStoneDia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If FromFlag = "A" Then
            ''Accounts Entry
            StudWtDedut = GetAdmindbSoftValue("STUDWTDEDUCT_ACC", "DSP")
        Else
            StudWtDedut = GetAdmindbSoftValue("STUDWTDEDUCT", "DSP")
        End If
        POS_ENABLE_STNGRP = IIf(GetAdmindbSoftValue("POS_ENABLE_STNGRP", "N") = "Y", True, False)
        gridStone.AllowUserToDeleteRows = Not _DelLock
        If STUDWT_DECREASE = False Then
            gridStone.AllowUserToDeleteRows = False
        End If
        gridStone.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridStoneTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridStoneTotal.DefaultCellStyle.BackColor = grpStone.BackgroundColor
        gridStoneTotal.DefaultCellStyle.SelectionBackColor = grpStone.BackgroundColor
        CalcStoneWtAmount()
    End Sub

    Private Function CheckStoneWeight() As Boolean
        Dim stWeight As Double = 0
        stWeight = IIf(cmbStUnit.Text = "C", Val(txtStWeight.Text) / 5, Val(txtStWeight.Text))
        For cnt As Integer = 0 To gridStone.RowCount - 1

            If STUDWT_DECREASE = False Then
                If txtStRowIndex.Text <> "" Then
                    If Val(txtStRowIndex.Text) = cnt Then
                        Dim _stnWt As Double = 0
                        Dim _stnPcs As Double = 0
                        Dim _stnRate As Double = 0
                        Dim _stnAmount As Double = 0

                        _stnPcs = Val(gridStone.Rows(cnt).Cells("PCS").Value.ToString)
                        _stnRate = Val(gridStone.Rows(cnt).Cells("RATE").Value.ToString)
                        _stnAmount = Val(gridStone.Rows(cnt).Cells("AMOUNT").Value.ToString)

                        _stnWt = IIf(gridStone.Rows(cnt).Cells("UNIT").Value.ToString = "C", Val(gridStone.Rows(cnt).Cells("WEIGHT").Value.ToString) / 5, Val(gridStone.Rows(cnt).Cells("WEIGHT").Value.ToString))
                        If _stnWt > stWeight Then
                            MsgBox("Stone Weight less than actual weight.", MsgBoxStyle.Information)
                            txtStWeight.Text = Math.Round(Val(gridStone.Rows(cnt).Cells("WEIGHT").Value.ToString), 4)
                            txtStWeight.Focus()
                        End If
                        If _stnPcs > Val(txtStPcs_NUM.Text) Then
                            MsgBox("Stone Pcs less than actual Pcs.", MsgBoxStyle.Information)
                            txtStPcs_NUM.Text = Math.Round(Val(gridStone.Rows(cnt).Cells("PCS").Value.ToString), 4)
                            txtStPcs_NUM.Focus()
                        End If
                        If _stnRate > Val(txtStRate_AMT.Text) Then
                            MsgBox("Stone Rate less than actual Rate.", MsgBoxStyle.Information)
                            txtStRate_AMT.Text = Math.Round(Val(gridStone.Rows(cnt).Cells("RATE").Value.ToString), 4)
                            txtStRate_AMT.Focus()
                        End If
                        If _stnAmount > Val(txtStAmount_AMT.Text) Then
                            MsgBox("Stone Amount less than actual Amount.", MsgBoxStyle.Information)
                            txtStAmount_AMT.Text = Math.Round(Val(gridStone.Rows(cnt).Cells("AMOUNT").Value.ToString), 4)
                            txtStAmount_AMT.Focus()
                        End If

                    End If
                End If
            End If

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

    Private Sub txtTranno_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTranno.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If txtTranno.Text.Trim() = "" Then Exit Sub
            strSql = vbCrLf + "  SELECT "
            strSql += vbCrLf + "   IM.ITEMNAME ITEM, "
            strSql += vbCrLf + "   SI.SUBITEMNAME SUBITEM,"
            strSql += vbCrLf + "   PCS,"
            strSql += vbCrLf + "   I.STONEUNIT UNIT,"
            strSql += vbCrLf + "   CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC,"
            strSql += vbCrLf + "   PUREWT WEIGHT,"
            strSql += vbCrLf + "   CONVERT(NUMERIC(15,2),NULL) RATE,"
            strSql += vbCrLf + "   CONVERT(NUMERIC(15,2),NULL) AMOUNT,"
            strSql += vbCrLf + "   I.METALID,"
            strSql += vbCrLf + "   I.SNO"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE I INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
            strSql += vbCrLf + "  WHERE I.TRANTYPE ='IIS'  AND ISNULL(STUDDED,'') IN ('S','L','B') AND ISNULL(I.CANCEL,'')='' "
            strSql += vbCrLf + "  and I.SNO NOT IN (SELECT DISTINCT JOBISNO FROM " & cnStockDb & "..RECEIPTSTONE WHERE "
            strSql += vbCrLf + "  ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')='') and isnull(jobisno,'')<>'')"
            strSql += vbCrLf + "  AND I.TRANNO =" & Val(txtTranno.Text)
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
                    txtTranno.Select()
                    Exit Sub
                End If
                strSql = vbCrLf + "  SELECT "
                strSql += vbCrLf + "   IM.ITEMNAME ITEM, "
                strSql += vbCrLf + "   SI.SUBITEMNAME SUBITEM,"
                strSql += vbCrLf + "   PCS,"
                strSql += vbCrLf + "   I.STONEUNIT UNIT,"
                strSql += vbCrLf + "   CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC,"
                strSql += vbCrLf + "   PUREWT WEIGHT,"
                strSql += vbCrLf + "   CONVERT(NUMERIC(15,2),NULL) RATE,"
                strSql += vbCrLf + "   CONVERT(NUMERIC(15,2),NULL) AMOUNT,"
                strSql += vbCrLf + "   I.METALID,"
                strSql += vbCrLf + "   I.SNO"
                strSql += vbCrLf + "  FROM " & prevdb & "..ISSUE I INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
                strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
                strSql += vbCrLf + "  WHERE I.TRANTYPE ='IIS'  AND ISNULL(STUDDED,'') IN ('S','L','B') AND ISNULL(I.CANCEL,'')='' "
                strSql += vbCrLf + "  and I.SNO NOT IN (SELECT DISTINCT JOBISNO FROM " & prevdb & "..RECEIPTSTONE WHERE "
                strSql += vbCrLf + "  ISSSNO IN (SELECT SNO FROM " & prevdb & "..RECEIPT WHERE ISNULL(CANCEL,'')='') and isnull(jobisno,'')<>'')"
                strSql += vbCrLf + "  AND I.TRANNO =" & Val(txtTranno.Text)
                dtGrid = New DataTable
                dtGrid.Columns.Add("CHECK", GetType(Boolean))
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGrid)
                If Not dtGrid.Rows.Count > 0 Then
                    txtTranno.Select()
                    Exit Sub
                End If
            End If
            objMultiSelect = New MultiSelectRowDia(dtGrid, "SNO")
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
                    dtGridStone.Rows.Add(ro)
                Next
                dtGridStone.AcceptChanges()
                gridStone.CurrentCell = gridStone.Rows(gridStone.RowCount - 1).Cells("ITEM")
                CalcStoneWtAmount()
                StyleGridStone(gridStone)
                StyleGridStone(gridStoneTotal)
                objGPack.TextClear(grpStone)
            End If
        End If
    End Sub

    Private Sub txtTranno_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTranno.KeyDown
        If e.KeyCode = Keys.Enter Then e.Handled = True
    End Sub

    Private Function validStonepcs() As Boolean
        If STUDDIA_PCS_MAND_EST_POS = True Then
            If txtStItem.Text.Trim <> "" Then
                strSql = "SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text.Trim & "'"
                If GetSqlValue(cn, strSql).ToString = "D" Then
                    If Val(txtStPcs_NUM.Text) = 0 Then
                        MsgBox("PCS should not empty", MsgBoxStyle.Information)
                        txtStPcs_NUM.Focus()
                        txtStPcs_NUM.SelectAll()
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return True
                End If
            Else
                Return True
            End If
        Else
            Return True
        End If
    End Function

    Private Sub txtStPcs_NUM_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtStPcs_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If STUDWT_DECREASE = False Then CheckStoneWeight()
            If validStonepcs() = False Then
                Exit Sub
            End If
        End If
    End Sub

End Class