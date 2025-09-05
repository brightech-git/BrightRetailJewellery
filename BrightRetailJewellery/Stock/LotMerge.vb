Imports System.Data.OleDb
Public Class LotMerge
    Dim _AccAudit As Boolean = IIf(GetAdmindbSoftValue("ACC_AUDIT", "N") = "Y", True, False)
    Dim cmd As OleDbCommand
    Dim objItemDetail As LotMergeItemDet
    Public objSoftKeys As New EstimationSoftKeys
    Dim objTagFiltration As New frmTagFiltration
    Dim dtTagDetail As New DataTable("GridTag")
    Dim dtStoneDetails As New DataTable("GridStone")
    Public BillCostId As String = cnCostId
    Dim dtGridLot, dt As New DataTable
    Dim ObjOrderTagInfo As New TagOrderInfo

    Dim RowTagMargin As DataRow = Nothing
    Public BillDate As Date = GetServerDate(tran)
    Dim _CheckOrderInfo As Boolean = IIf(GetAdmindbSoftValue("CHECKORDERINFO", "Y") = "Y", True, False)
    Dim objTag As New TagGeneration

    Dim taggrstwt As Decimal = 0
    Dim tagStoneWeight As Decimal = 0

    Dim tagSno As String = Nothing
    Dim tagStoneSno As String = Nothing
    Dim LOTSNO As String
    Dim TagNoGen As String = Nothing
    Dim TagNoFrom As String = Nothing
    Dim currentTagNo As String
    Dim lastTagNo As String
    Dim startTagNo As String
    Dim endTagNo As String
    Dim OldTagGrswt As Decimal = 0
    Dim OldTagNetwt As Decimal = 0
    Dim OldTagAmount As Double = 0

    Dim OldTagStoneWt As Decimal = 0
    Dim OldTagStoneAmt As Double = 0
    Dim ObjMinValue As New TagMinValues
    Dim SNO As String
    Dim strSql As String
    Dim tabCheckBy As String = GetAdmindbSoftValue("LOTCHECKBY", "P")
    Dim Lotchkdate As Boolean = IIf(GetAdmindbSoftValue("LOTCHKDATE", "N") = "Y", True, False)
    Dim objdatescr As New frmDateInput

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        With dtGridLot
            .Columns.Add("LOTNO", GetType(Integer))
            .Columns.Add("ITEMID", GetType(Integer))
            .Columns.Add("ITEMNAME", GetType(String))
            .Columns.Add("SUBITEMID", GetType(Integer))
            .Columns.Add("SUBITEMNAME", GetType(String))
            .Columns.Add("DESIGNER", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("STNPCS", GetType(Integer))
            .Columns.Add("STNWT", GetType(Decimal))
            .Columns.Add("DIAPCS", GetType(Integer))
            .Columns.Add("DIAWT", GetType(Decimal))
            .Columns.Add("SNO", GetType(String))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("ORGPCS", GetType(Integer))
            .Columns.Add("ORGGRSWT", GetType(Decimal))
            .Columns.Add("ORGNETWT", GetType(Decimal))
            .Columns.Add("ORGSTNPCS", GetType(Integer))
            .Columns.Add("ORGSTNWT", GetType(Decimal))
            .Columns.Add("TYPE", GetType(String))
            .Columns.Add("ITEMCTRID", GetType(Integer))
        End With
        dtGridLot.AcceptChanges()
        gridTAG.DataSource = dtGridLot
        gridTAG.ColumnHeadersVisible = False
        FormatGridColumns(gridTAG)
        StyleGridTag(gridTAG)

        Dim dtGridtagTotal As New DataTable
        dtGridtagTotal = dtGridLot.Copy
        dtGridtagTotal.Rows.Clear()
        dtGridtagTotal.Rows.Add()
        gridTAGTotal.ColumnHeadersVisible = False
        gridTAGTotal.DataSource = dtGridtagTotal
        For Each col As DataGridViewColumn In gridTAG.Columns
            With gridTAGTotal.Columns(col.Name)
                .Visible = col.Visible
                .Width = col.Width
                .DefaultCellStyle = col.DefaultCellStyle
            End With
        Next
        CalcGridtagTotal()
        StyleGridTag(gridTAGTotal)
        txtLotNo.Focus()
    End Sub
    Private Sub CalcGridtagTotal()

        Dim tagPcs As Integer = Nothing
        Dim tagGrsWt As Decimal = Nothing
        Dim tagNetWt As Decimal = Nothing
        Dim tagStnWt As Decimal = Nothing
        Dim tagStnPcs As Integer = Nothing
        Dim tagDiaWt As Decimal = Nothing
        Dim tagDiaPcs As Integer = Nothing

        For i As Integer = 0 To gridTAG.RowCount - 1
            With gridTAG.Rows(i)
                .DefaultCellStyle.BackColor = SystemColors.HighlightText
                tagPcs += Val(.Cells("PCS").Value.ToString)
                tagGrsWt += Val(.Cells("GRSWT").Value.ToString)
                tagStnPcs += Val(.Cells("STNPCS").Value.ToString)
                tagStnWt += Val(.Cells("STNWT").Value.ToString)
                tagDiaPcs += Val(.Cells("DIAPCS").Value.ToString)
                tagDiaWt += Val(.Cells("DIAWT").Value.ToString)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
            End With
        Next
        grpTagDetail.BackColor = SystemColors.InactiveCaption
        With gridTAGTotal.Rows(0)
            .Cells("ITEMNAME").Value = "TOTAL"
            .Cells("PCS").Value = IIf(tagPcs <> 0, tagPcs, DBNull.Value)
            .Cells("GRSWT").Value = IIf(tagGrsWt <> 0, tagGrsWt, DBNull.Value)
            .Cells("STNPCS").Value = IIf(tagStnPcs <> 0, tagStnPcs, DBNull.Value)
            .Cells("STNWT").Value = IIf(tagStnWt <> 0, tagStnWt, DBNull.Value)
            .Cells("DIAPCS").Value = IIf(tagDiaPcs <> 0, tagDiaPcs, DBNull.Value)
            .Cells("DIAWT").Value = IIf(tagDiaWt <> 0, tagDiaWt, DBNull.Value)
            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .DefaultCellStyle.ForeColor = Color.White
            .DefaultCellStyle.SelectionForeColor = Color.Blue
        End With
    End Sub
    Private Sub StyleGridTag(ByVal gridTagView As DataGridView)
        With gridTagView
            .Columns("LOTNO").Width = txtLotNo.Width + 1
            .Columns("ITEMID").Width = txtItemId.Width + 1
            .Columns("ITEMNAME").Width = txtItemname.Width + 1
            .Columns("SUBITEMID").Width = txtSubItemId.Width + 1
            .Columns("SUBITEMNAME").Width = txtSubItemName.Width + 1
            .Columns("DESIGNER").Width = txtDesigner.Width + 1
            .Columns("PCS").Width = txtPcs.Width + 1
            .Columns("GRSWT").Width = txtTagGrsWt_WET.Width + 1
            .Columns("STNPCS").Width = txtStonePcs.Width + 1
            .Columns("STNWT").Width = txtStoneWT.Width + 1
            .Columns("DIAPCS").Width = txtDiaPcs.Width + 1
            .Columns("DIAWT").Width = txtDiaWT.Width + 1
            For i As Integer = 12 To gridTagView.Columns.Count - 1
                .Columns(i).Visible = False
            Next
        End With
    End Sub
    Private Sub TagMerge_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtLotNo.Focused Then Exit Sub
            If txtItemId.Focused Then Exit Sub
            If txtDesigner.Focused Then Exit Sub
            If txtTagGrsWt_WET.Focused Then Exit Sub
            If txtPcs.Focused Then Exit Sub
            If txtStoneWT.Focused Then Exit Sub
            If txtStonePcs.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub TagMerge_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridTAG.Font = New Font("VERDANA", 8, FontStyle.Bold)
        StyleGridTag(gridTAG)
        StyleGridTag(gridTAGTotal)
        txtLotNo.Focus()
    End Sub

    Private Sub txtLotNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLotNo.GotFocus
        lblHelp1.Text = "Press Down Key to Select"
        lblHelp2.Text = "Press Escape Key to Save"
    End Sub

    Private Sub gridTAG_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridTAG.GotFocus
        lblHelp1.Text = "Press Enter Key to Edit"
        lblHelp2.Text = "Press Del Key to Delete"
        gridTAG.Rows(0).DefaultCellStyle.BackColor = Color.Wheat
    End Sub
    Private Sub gridTAG_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridTAG.LostFocus
        lblHelp1.Text = ""
        lblHelp2.Text = ""
    End Sub
    Private Sub txtLotNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLotNo.LostFocus
        lblHelp1.Text = ""
        lblHelp2.Text = ""
    End Sub
    Private Sub txtLotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLotNo.KeyDown
        Dim mLotchk As String = ""
        If e.KeyCode = Keys.Insert Then
            If Lotchkdate = True Then
                objdatescr.Label1.Text = "Lot Receipt Date"
                objdatescr.ShowDialog()
                mLotchk = " AND LOTDATE ='" & objdatescr.dtpBillDatef.Value.ToString("yyyy-MM-dd") & "' "
            End If
            strSql = " SELECT ITEMID,SUBITEMID,"
            strSql += vbCrLf + " LOTNO,LOTDATE"
            strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = LOT.DESIGNERID)AS DESIGNER"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID = LOT.ITEMID)AS ITEM"
            strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID = LOT.SUBITEMID),'')AS SUBITEM"
            strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = LOT.COSTID)AS COSTCENTRE"
            strSql += vbCrLf + " ,PCS,GRSWT,CPCS AS CPCS,CGRSWT AS CGRSWT,(PCS-CPCS)AS BALPCS,(GRSWT-CGRSWT)AS BALGRSWT"
            strSql += vbCrLf + " ,(SELECT (CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END) FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS STOCKTYPE"
            strSql += vbCrLf + " ,CASE ENTRYTYPE WHEN 'R' THEN 'REGULAR' WHEN 'OR' THEN 'ORDER' WHEN 'RE' THEN 'ORDER' ELSE ENTRYTYPE END AS LOTTYPE"
            strSql += vbCrLf + " ,SNO"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMLOT AS LOT"
            strSql += vbCrLf + " WHERE "

            If tabCheckBy = "P" Then
                strSql += vbCrLf + " (PCS > CPCS) "
            Else
                strSql += vbCrLf + " ((GRSWT > CGRSWT) OR (RATE <> 0 AND PCS > CPCS))"
            End If
            If mLotchk <> "" Then strSql += vbCrLf + mLotchk
            strSql += vbCrLf + " AND ISNULL(COMPLETED,'') <> 'Y'"
            strSql += vbCrLf + " AND (SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID) = 'T'"
            If txtItemId.Text <> "" Then
                strSql += vbCrLf + " AND LOTNO LIKE '" & txtItemId.Text & "%'"
            End If
            strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
            strSql += vbCrLf + " AND ISNULL(BULKLOT,'') <> 'Y'"
            strSql += vbCrLf + " ORDER BY LOTNO "
            SNO = BrighttechPack.SearchDialog.Show("Searching LotNo", strSql, cn, 0, 16)
            strSql = " SELECT LOTNO FROM " & cnAdminDb & "..ITEMLOT"
            strSql += vbCrLf + " WHERE SNO = '" & SNO & "'"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtLotNo.Text = dt.Rows(0).Item(0)
                strSql = " SELECT 'T' AS TYPE,SNO,LOTNO,ITEMID,SUBITEMID"
                strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = LOT.DESIGNERID)AS DESIGNER"
                strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID = LOT.ITEMID)AS ITEMNAME"
                strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID = LOT.SUBITEMID),'')AS SUBITEMNAME"
                strSql += " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = LOT.COSTID)AS COSTCENTRE"
                strSql += " ,PCS,GRSWT,CPCS AS CPCS,CGRSWT AS CGRSWT,(PCS-CPCS) AS BALPCS,(GRSWT-CGRSWT)AS BALGRSWT,STNPCS,STNWT,DIAPCS,DIAWT"
                strSql += " ,(SELECT (CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END) FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS STOCKTYPE"
                strSql += " ,CASE ENTRYTYPE WHEN 'R' THEN 'REGULAR' WHEN 'OR' THEN 'ORDER' WHEN 'RE' THEN 'ORDER' ELSE ENTRYTYPE END AS LOTTYPE"
                strSql += " FROM " & cnAdminDb & "..ITEMLOT AS LOT"
                'strSql += " WHERE PCS > CPCS "
                strSql += " WHERE  "
                If tabCheckBy = "P" Then
                    strSql += vbCrLf + " (PCS > CPCS) "
                Else
                    strSql += vbCrLf + " ((GRSWT > CGRSWT) OR (RATE <> 0 AND PCS > CPCS))"
                End If
                strSql += " AND ISNULL(COMPLETED,'') <> 'Y'"
                strSql += " AND LOTNO = '" & txtLotNo.Text & "'"
                strSql += " ORDER BY LOTNO "
                Dim dtlot As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtlot)
                If Not dtlot.Rows.Count > 0 Then
                    LoadLotDetails(dtlot)
                Else
                    Exit Sub
                End If
            End If
        ElseIf e.KeyCode = Keys.Down Then
            If gridTAG.Rows.Count = 0 Then txtLotNo.Focus() : Exit Sub
            gridTAG.Focus()
        ElseIf e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub txtLotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtLotNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtLotNo.Text <> String.Empty Then
                For Each ro As DataRow In dtGridLot.Rows
                    If ro.Item("LOTNO").ToString = txtLotNo.Text Then
                        MsgBox("This Lot Already Loaded", MsgBoxStyle.Information)
                        txtLotNo.Clear()
                        Exit Sub
                    End If
                Next
                Dim dtlot As New DataTable
                strSql = " SELECT 'T' AS TYPE,SNO,LOTNO,ITEMID,SUBITEMID"
                strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = LOT.DESIGNERID)AS DESIGNER"
                strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID = LOT.ITEMID)AS ITEMNAME"
                strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID = LOT.SUBITEMID),'')AS SUBITEMNAME"
                strSql += " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = LOT.COSTID)AS COSTCENTRE"
                strSql += " ,ITEMCTRID,PCS,GRSWT,NETWT,CPCS AS CPCS,CGRSWT AS CGRSWT,(PCS-CPCS) AS BALPCS,(GRSWT-CGRSWT)AS BALGRSWT,(ISNULL(NETWT,0)-ISNULL(CNETWT,0))AS BALNETWT,STNPCS,STNWT,DIAPCS,DIAWT"
                strSql += " ,(SELECT (CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END) FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS STOCKTYPE"
                strSql += vbCrLf + " ,CASE ENTRYTYPE WHEN 'R' THEN 'REGULAR' WHEN 'OR' THEN 'ORDER' WHEN 'RE' THEN 'ORDER' ELSE ENTRYTYPE END AS LOTTYPE"
                strSql += " FROM " & cnAdminDb & "..ITEMLOT AS LOT"
                strSql += " WHERE ISNULL(COMPLETED,'') <> 'Y'"
                If tabCheckBy = "P" Then
                    strSql += vbCrLf + " AND (PCS > CPCS) "
                Else
                    strSql += vbCrLf + "  AND ((GRSWT > CGRSWT) OR (RATE <> 0 AND PCS > CPCS))"
                End If
                strSql += " AND LOTNO = '" & txtLotNo.Text & "'"
                strSql += " ORDER BY LOTNO "
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtlot)
                If Not dtlot.Rows.Count > 0 Then
                    MsgBox("Record Not Found", , "Brighttech Gold")
                    txtLotNo.Clear()
                    Exit Sub
                Else
                    LoadLotDetails(dtlot)
                End If
                txtLotNo.Clear()
            Else
                txtItemId.Focus()
            End If
        End If
    End Sub
    Private Sub LoadLotDetails(ByVal dtl As DataTable)
        For ii As Integer = 0 To dtl.Rows.Count - 1
            Dim drl As DataRow
            drl = dtGridLot.NewRow
            drl("SNO") = dtl.Rows(ii).Item("SNO").ToString
            drl("ITEMNAME") = dtl.Rows(ii).Item("ITEMNAME").ToString
            drl("ITEMID") = dtl.Rows(ii).Item("ITEMID").ToString
            drl("SUBITEMID") = dtl.Rows(ii).Item("SUBITEMID").ToString
            drl("SUBITEMNAME") = dtl.Rows(ii).Item("SUBITEMNAME").ToString
            drl("PCS") = dtl.Rows(ii).Item("BALPCS").ToString
            drl("GRSWT") = dtl.Rows(ii).Item("BALGRSWT").ToString
            drl("NETWT") = dtl.Rows(ii).Item("BALNETWT").ToString
            drl("STNPCS") = dtl.Rows(ii).Item("STNPCS").ToString
            drl("STNWT") = dtl.Rows(ii).Item("STNWT").ToString
            drl("DIAPCS") = dtl.Rows(ii).Item("DIAPCS").ToString
            drl("DIAWT") = dtl.Rows(ii).Item("DIAWT").ToString
            drl("ORGPCS") = dtl.Rows(ii).Item("BALPCS").ToString
            drl("ORGGRSWT") = dtl.Rows(ii).Item("BALGRSWT").ToString
            drl("LOTNO") = dtl.Rows(ii).Item("LOTNO").ToString
            drl("DESIGNER") = dtl.Rows(ii).Item("DESIGNER").ToString
            drl("TYPE") = dtl.Rows(ii).Item("TYPE").ToString
            drl("ITEMCTRID") = dtl.Rows(ii).Item("ITEMCTRID").ToString
            dtGridLot.Rows.Add(drl)
        Next
        If dtGridLot.Rows.Count > 0 Then
            gridTAG.DataSource = dtGridLot
            CalcGridtagTotal()
            gridTAG.Rows(0).DefaultCellStyle.SelectionBackColor = Color.Wheat
        End If
    End Sub
    Private Sub ClearDtGrid(ByVal dt As DataTable)
        dt.Columns("KeyNo").AutoIncrementSeed = 1
        dt.Rows.Clear()
        For i As Integer = 1 To 10
            dt.Rows.Add()
        Next
        dt.AcceptChanges()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        fnew()
    End Sub
    Function fnew()
        dtGridLot.Clear()
        ClearTagDet()
        ClearItemDetails()
        CalcGridtagTotal()
        txtTAGRowIndex.Text = ""
        txtLotNo.Focus()
    End Function


    Private Sub ClearTagDet()
        txtLotNo.Clear()
        txtItemId.Clear()
        txtItemname.Clear()
        txtSubItemId.Clear()
        txtSubItemName.Clear()
        txtDesigner.Clear()
        txtTagGrsWt_WET.Clear()
        txtPcs.Clear()
        txtStonePcs.Clear()
        txtStoneWT.Clear()
        txtDiaPcs.Clear()
        txtDiaWT.Clear()
    End Sub
    Private Sub ClearItemDetails()
        dtTagDetail.Clear()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub
    Private Sub gridTAG_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridTAG.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridTAG.CurrentCell = gridTAG.Rows(gridTAG.CurrentRow.Index).Cells("LOTNO")
            gridTAG.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.White
            gridTAG.CurrentRow.DefaultCellStyle.SelectionForeColor = Color.Black
            Dim rwIndex As Integer = gridTAG.CurrentRow.Index
            With gridTAG.Rows(rwIndex)
                txtLotNo.Text = .Cells("LOTNO").FormattedValue
                txtItemId.Text = .Cells("ITEMID").FormattedValue
                txtItemname.Text = .Cells("ITEMNAME").FormattedValue
                txtSubItemId.Text = .Cells("SUBITEMID").FormattedValue
                txtSubItemName.Text = .Cells("SUBITEMNAME").FormattedValue
                txtDesigner.Text = .Cells("DESIGNER").FormattedValue
                txtTagGrsWt_WET.Text = .Cells("GRSWT").FormattedValue
                txtPcs.Text = .Cells("PCS").FormattedValue
                txtStoneWT.Text = .Cells("STNWT").FormattedValue
                txtStonePcs.Text = .Cells("STNPCS").FormattedValue
                txtDiaPcs.Text = .Cells("DIAPCS").FormattedValue
                txtDiaWT.Text = .Cells("DIAWT").FormattedValue
                taggrstwt = Val(.Cells("GRSWT").FormattedValue.ToString)
                CalcGridtagTotal()
                txtTAGRowIndex.Text = rwIndex
                txtPcs.Focus()
                tagSno = .Cells("SNO").FormattedValue
            End With
        ElseIf e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        ElseIf e.KeyCode = Keys.Up Then
            If gridTAG.CurrentRow.Index = 0 Then txtLotNo.Focus()
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not dtGridLot.Rows.Count > 0 Then
            MsgBox("No Record Added", MsgBoxStyle.Exclamation)
            Me.SelectNextControl(txtLotNo, True, True, True, True)
            Exit Sub
        End If
        Try
            objItemDetail = New LotMergeItemDet(gridTAGTotal.Rows(0).Cells("PCS").Value.ToString, _
            gridTAGTotal.Rows(0).Cells("GRSWT").Value.ToString, _
            gridTAGTotal.Rows(0).Cells("STNPCS").Value.ToString, gridTAGTotal.Rows(0).Cells("STNWT").Value.ToString, _
            gridTAGTotal.Rows(0).Cells("DIAPCS").Value.ToString, gridTAGTotal.Rows(0).Cells("DIAWT").Value.ToString, _
            gridTAG.Rows(0).Cells("ITEMNAME").Value.ToString, _
            gridTAG.Rows(0).Cells("SUBITEMNAME").Value.ToString, gridTAG.Rows(0).Cells("DESIGNER").Value.ToString, _
            gridTAG.Rows(0).Cells("ITEMCTRID").Value.ToString)
            If objItemDetail.Visible Then Exit Sub
            objItemDetail.BackColor = Me.BackColor
            objItemDetail.StartPosition = FormStartPosition.CenterScreen
            objItemDetail.MaximizeBox = False
            If Not objItemDetail.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If
            tran = Nothing
            tran = cn.BeginTransaction()
            'Getting LotNo
            Dim Catsno As Integer
            Dim LOTNO As Integer
            Dim Catcode As New List(Of String)
            Dim dtCatcode As New DataTable
            With dtCatcode
                .Columns.Add("SNO", GetType(Integer))
                .Columns.Add("BATCHNO", GetType(String))
                .Columns.Add("CATCODE", GetType(String))
                .Columns.Add("TYPE", GetType(String))
                .Columns.Add("OMATERIALTYPE", GetType(String))
                .Columns.Add("ITEMID", GetType(Integer))
                .Columns.Add("PCS", GetType(Decimal))
                .Columns.Add("GRSWT", GetType(Decimal))
            End With

            'Call Save_MRMI_ISSREC(BatchNo, Newcatcode, "O", "R", newitemid, Val(.Item("PCS").ToString), Val(.Item("GRSWT").ToString))

            Dim newitemid, newsubitemid, newDesignerId As Integer

GENLOTNO:
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & objItemDetail.cmbItem_MAN.Text & "'"
            newitemid = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))

            If objItemDetail.cmbSubItem_Man.Enabled = True Then
                ''Find SubItemId
                strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & objItemDetail.cmbSubItem_Man.Text & "' AND ITEMID = " & newitemid & ""
                newsubitemid = Val(objGPack.GetSqlValue(strSql, "SUBITEMID", , tran))
            Else
                newsubitemid = Val(objItemDetail.cmbSubItem_Man.Text)
            End If
            strSql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & objItemDetail.cmbDesigner_MAN.Text & "'"
            newDesignerId = Val(objGPack.GetSqlValue(strSql, "DESIGNERID", "", tran))

            strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LOTNO'"
            LOTNO = Val(objGPack.GetSqlValue(strSql, , , tran))
            strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & LOTNO + 1 & "' "
            strSql += vbcrlf + " WHERE CTLID = 'LOTNO' AND CTLTEXT = " & LOTNO & ""
            cmd = New OleDbCommand(strSql, cn, tran)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GENLOTNO
            End If
            LOTNO += 1
            strSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & objItemDetail.CmbItemCounter.Text & "'"
            Dim itemCounterId As Integer = Val(objGPack.GetSqlValue(strSql, , , tran))
            strSql = " SELECT STOCKTYPE,DEFAULTCOUNTER,NOOFPIECE,VALUEADDEDTYPE,CATCODE  FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMid = " & newitemid
            Dim dritem As DataRow
            dritem = GetSqlRow(strSql, cn, tran)
            Dim stockType As String = dritem(0).ToString
            Dim ITEMCTRID As Integer = Val(dritem(1).ToString)
            Dim noOfTag As Integer = Val(dritem(2).ToString)
            Dim VALUEADDTYPE As String = dritem(3).ToString
            Dim Newcatcode As String = dritem(4).ToString
            Dim oldcatcode As String = ""
            Dim entryType As String = "R"
            Dim Sno As String = ""
            Dim Costid As String = cnCostId
            Dim BatchNo As String = GetNewBatchno(Costid, BillDate, tran)

            strSql = " INSERT INTO " & cnAdminDb & "..ITEMLOT"
            strSql += vbcrlf + " ("
            strSql += vbcrlf + " SNO,ENTRYTYPE,LOTDATE,LOTNO,DESIGNERID,TRANINVNO,"
            strSql += vbcrlf + " BILLNO,COSTID,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
            strSql += vbCrLf + " ITEMID,SUBITEMID,PCS,GRSWT,STNPCS,STNWT,STNUNIT,"
            strSql += vbcrlf + " DIAPCS,DIAWT,NETWT,NOOFTAG,RATE,ITEMCTRID,WMCTYPE,"
            strSql += vbcrlf + " BULKLOT,MULTIPLETAGS,NARRATION,FINERATE,TUCH,"
            strSql += vbcrlf + " WASTPER,MCGRM,OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
            strSql += vbcrlf + " COMPANYID,CPCS,CGRSWT,COMPLETED,CANCEL,VATEXM,"
            strSql += vbcrlf + " ACCESSING,USERID,UPDATED,"
            strSql += vbcrlf + " UPTIME,SYSTEMID,APPVER,TABLECODE)VALUES("
            SNO = GetNewSno(TranSnoType.ITEMLOTCODE, tran, "GET_ADMINSNO_TRAN")
            strSql += vbcrlf + " '" & Sno & "'" 'SNO
            strSql += vbcrlf + " ,'R'" 'ENTRYTYPE
            strSql += vbcrlf + " ,'" & GetEntryDate(GetServerDate(tran), tran).ToString("yyyy-MM-dd") & "'" 'LOTDATE 'DateTime.Parse(.Item("LOTDATE"), ukCultureInfo.DateTimeFormat).ToString("yyyy-MM-dd")
            strSql += vbcrlf + " ," & LOTNO & "" 'LOTNO
            strSql += vbcrlf + " ," & newDesignerId & "" 'DESIGNERID
            strSql += vbcrlf + " ,''" ' & .Item("TRANINVNO") & "'" 'TRANINVNO
            strSql += vbcrlf + " ,''" ' & .Item("BILLNO") & "'" 'BILLNO
            strSql += vbcrlf + " ,'" & Costid & "'" 'COSTID
            strSql += vbcrlf + " ,1"
            strSql += vbcrlf + " ,''" ' & .Item("ORDREPNO") & "'" 'ORDREPNO
            strSql += vbcrlf + " ,''" ' & .Item("ORDENTRYORDER") & "'" 'ORDENTRYORDER
            strSql += vbcrlf + " ," & Val(newitemid) & "" 'ITEMID
            strSql += vbcrlf + " ," & Val(newsubitemid) & "" 'SUBITEMID
            strSql += vbcrlf + " ," & Val(objItemDetail.txtPcs.Text) & "" 'PCS
            strSql += vbCrLf + " ," & Val(objItemDetail.txtTotWeight.Text) & "" 'GRSWT
            strSql += vbcrlf + " ," & Val(objItemDetail.txtStnPcs.Text) & "" 'STNPCS
            strSql += vbcrlf + " ," & Val(objItemDetail.txtStnWt.Text) & "" 'STNWT
            strSql += vbcrlf + " ,''" 'STNUNIT
            strSql += vbCrLf + " , '" & Val(objItemDetail.txtDiaPcs.Text.ToString) & "'" 'DIAPCS
            strSql += vbCrLf + " , '" & Val(objItemDetail.txtDiaWt.Text.ToString) & "'" 'DIAWT
            strSql += vbcrlf + " ," & Val(objItemDetail.txtNetWt.Text) & "" 'NETWT
            strSql += vbcrlf + " ," & noOfTag & "" 'NOOFTAG
            strSql += vbcrlf + " ,0" ' & Val(.Item("RATE").ToString) & "" 'RATE
            strSql += vbcrlf + " ," & Val(itemCounterId) & "" 'ITEMCTRID
            strSql += vbcrlf + " ,'" & VALUEADDTYPE & "'" 'WMCTYPE
            strSql += vbcrlf + " ,''"
            strSql += vbcrlf + " ,''"  'MULTIPLETAGS
            strSql += vbcrlf + " ,'LOT MERGE'"
            strSql += vbcrlf + " ,0" ' & Val(.Item("FINERATE").ToString) & "" 'FINERATE
            strSql += vbcrlf + " ,0" ' & Val(.Item("TUCH").ToString) & "" 'TUCH
            strSql += vbcrlf + " ,0" ' & Val(.Item("WASTPER").ToString) & "" 'WASTPER
            strSql += vbcrlf + " ,0" ' & Val(.Item("MCGRM").ToString) & "" 'MCGRM
            strSql += vbcrlf + " ,0" '& Val(.Item("OTHCHARGE").ToString) & "" 'OTHCHARGE
            If stockType = "T" Then ''STOCKTYPE ==>TAGED ITEM
                If TagNoGen = "L" Then
                    funcTagNo(newitemid, Val(objItemDetail.txtPcs.Text))
                    strSql += vbcrlf + " ,'" & startTagNo & "'" 'STARTTAGNO
                    strSql += vbcrlf + " ,'" & endTagNo & "'" 'ENDTAGNO
                    strSql += vbcrlf + " ,'" & currentTagNo & "'" 'CURTAGNO
                Else
                    strSql += vbcrlf + " ,''" 'STARTTAGNO
                    strSql += vbcrlf + " ,''" 'ENDTAGNO
                    strSql += vbcrlf + " ,''" 'CURTAGNO
                End If
            Else
                strSql += vbcrlf + " ,''" 'STARTTAGNO
                strSql += vbcrlf + " ,''" 'ENDTAGNO
                strSql += vbcrlf + " ,''" 'CURTAGNO
            End If
            strSql += vbcrlf + " ,'" & GetStockCompId() & "'" 'COMPANYID
            strSql += vbcrlf + " ,0" 'CPIECE
            strSql += vbcrlf + " ,0" 'CWEIGHT
            strSql += vbcrlf + " ,''" 'COMPLETED
            strSql += vbcrlf + " ,''" 'CANCEL
            strSql += vbcrlf + " ,''" 'VATEXM
            strSql += vbcrlf + " ,''" 'ACCESSING
            strSql += vbcrlf + " ," & userId & "" 'USERID
            strSql += vbcrlf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += vbcrlf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += vbcrlf + " ,'" & systemId & "'" 'SYSTEMID
            strSql += vbcrlf + " ,'" & VERSION & "'" 'APPVER
            strSql += vbcrlf + " ,''" '& .Item("TABLECODE").ToString & "'" 'TABLECODE
            strSql += vbcrlf + " )"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            For cnt As Integer = 0 To dtGridLot.Rows.Count - 1
                With dtGridLot.Rows(cnt)
                    If .Item("TYPE").ToString = "N" Then
                        Dim tagSno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
                        strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAG"
                        strSql += vbcrlf + " ("
                        strSql += vbcrlf + " SNO,ITEMID,SUBITEMID,COMPANYID,RECDATE,"
                        strSql += vbcrlf + " PCS,GRSWT,LESSWT,NETWT,"
                        strSql += vbcrlf + " FINRATE,ISSTYPE,RECISS,POSTED,"
                        strSql += vbcrlf + " PACKETNO,DREFNO,ITEMCTRID,"
                        strSql += vbcrlf + " ORDREPNO,ORSNO,NARRATION,"
                        strSql += vbcrlf + " RATE,COSTID,"
                        strSql += vbcrlf + " CTGRM,DESIGNERID,VATEXM,ITEMTYPEID,"
                        strSql += vbcrlf + " CARRYFLAG,REASON,BATCHNO,CANCEL,"
                        strSql += vbcrlf + " USERID,UPDATED,UPTIME,SYSTEMID,APPVER,EXTRAWT)VALUES("
                        strSql += vbcrlf + " '" & tagSno & "'" 'SNO
                        strSql += vbcrlf + " ," & Val(.Item("ITEMID").ToString) & "" 'ItemId
                        strSql += vbcrlf + " ,0" 'SubItemId
                        strSql += vbcrlf + " ,'" & GetStockCompId() & "'" 'Companyid
                        strSql += vbcrlf + " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'Recdate
                        strSql += vbcrlf + " ," & Val(.Item("PCS").ToString) & "" 'Pcs
                        strSql += vbcrlf + " ," & Val(.Item("GRSWT").ToString) & "" 'GrsWt
                        strSql += vbcrlf + " ,0"  'LessWt
                        strSql += vbcrlf + " ,0" 'NetWt
                        strSql += vbcrlf + " ,0"  'FinRate
                        strSql += vbcrlf + " ,''" 'Isstype
                        strSql += vbcrlf + " ,'I'" 'RecIss
                        strSql += vbcrlf + " ,''" 'Posted
                        strSql += vbcrlf + " ,'0'"  'Packetno
                        strSql += vbcrlf + " ,0" 'DRefno
                        strSql += vbcrlf + " ," & ITEMCTRID & "" 'ItemCtrId
                        strSql += vbcrlf + " ,''" 'OrdRepNo
                        strSql += vbcrlf + " ,''" 'ORSNO
                        strSql += vbcrlf + " ,''" 'Narration
                        strSql += vbcrlf + " ,0"  'Rate
                        strSql += vbcrlf + " ,'" & Costid & "'" 'COSTID
                        strSql += vbcrlf + " ,''"
                        strSql += vbcrlf + " ,0"   'DesignerId
                        strSql += vbcrlf + " ,''" 'VATEXM
                        strSql += vbcrlf + " ,0" 'ItemTypeID
                        strSql += vbcrlf + " ,''" 'Carryflag
                        strSql += vbcrlf + " ,'0'" 'Reason
                        strSql += vbcrlf + " ,''" 'BatchNo
                        strSql += vbcrlf + " ,''" 'Cancel
                        strSql += vbcrlf + " ," & userId & "" 'UserId
                        strSql += vbcrlf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
                        strSql += vbcrlf + " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
                        strSql += vbcrlf + " ,'" & systemId & "'" 'Systemid
                        strSql += vbcrlf + " ,'" & VERSION & "'" 'APPVER
                        strSql += vbcrlf + " ,0" 'EXTRAWEIGHT
                        strSql += vbcrlf + " )"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()

                        Dim olditemid As Integer = Val(.Item("ITEMID").ToString)
                        strSql = " SELECT CATCODE  FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMid = " & olditemid
                        Dim dritemn As DataRow
                        dritemn = GetSqlRow(strSql, cn, tran)

                        If Catcode.Contains(dritemn.Item(0).ToString) = False Then
                            Catcode.Add(dritemn.Item(0).ToString)
                        End If
                        Dim ro As DataRow
                        ro = dtCatcode.NewRow
                        ro("SNO") = Catsno + 1
                        ro("BATCHNO") = BatchNo
                        ro("CATCODE") = dritemn.Item(0).ToString
                        ro("TYPE") = "O"
                        ro("OMATERIALTYPE") = "I"
                        ro("ITEMID") = olditemid
                        ro("PCS") = Val(.Item("PCS").ToString)
                        ro("GRSWT") = Val(.Item("GRSWT").ToString)
                        dtCatcode.Rows.Add(ro)
                        Catsno = Catsno + 1
                        'Call Save_MRMI_ISSREC(BatchNo, dritemn.Item(0).ToString, "O", "I", olditemid, Val(.Item("PCS").ToString), Val(.Item("GRSWT").ToString))
                    Else
                        strSql = "UPDATE " & cnAdminDb & "..ITEMLOT SET "
                        strSql += vbcrlf + " CPCS = CPCS+" & Val(.Item("PCS").ToString) & ""
                        strSql += vbCrLf + " ,CGRSWT=ISNULL(CGRSWT,0)+" & Val(.Item("GRSWT").ToString) & " "
                        strSql += vbCrLf + " ,CNETWT=ISNULL(CNETWT,0)+" & Val(.Item("NETWT").ToString) & " "
                        strSql += vbCrLf + " ,NEWSNO='" & Sno & "' "
                        strSql += vbCrLf + " WHERE SNO='" & .Item("SNO").ToString & "'"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                    End If
                End With
            Next

            If dtCatcode.Rows.Count > 0 Then
                For i As Integer = 0 To Catcode.Count - 1
                    Dim pcs As Decimal = dtCatcode.Compute("SUM(PCS)", "CATCODE='" & Catcode.Item(i).ToString & "'")
                    Dim GRSWT As Decimal = dtCatcode.Compute("SUM(GRSWT)", "CATCODE='" & Catcode.Item(i).ToString & "'")
                    Dim dtrow() As DataRow = dtCatcode.Select("CATCODE='" & Catcode.Item(i).ToString & "'", "")
                    Call Save_MRMI_ISSREC(dtrow(0).Item("BATCHNO").ToString, dtrow(0).Item("CATCODE").ToString, dtrow(0).Item("TYPE").ToString, dtrow(0).Item("OMATERIALTYPE").ToString, Val(dtrow(0).Item("ITEMID").ToString), pcs, GRSWT)
                Next
                Call Save_MRMI_ISSREC(BatchNo, Newcatcode, "O", "R", newitemid, Val(objItemDetail.txtPcs.Text), Val(objItemDetail.txtTotWeight.Text))
            End If

            If TagNoGen = "L" Then ''FROM ITEMMASTER
                strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO = '" & endTagNo & "' WHERE ITEMID = '" & newitemid & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If
            tran.Commit()
            tran = Nothing
            Dim prLotNo As Integer = LOTNO
            Dim prLotDate As Date = BillDate
            MsgBox(LOTNO.ToString + " Generated...", MsgBoxStyle.Exclamation)
            fnew()
            dtGridLot.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message + ex.StackTrace)
            If Not tran Is Nothing Then tran.Rollback()
        End Try
    End Sub

    Function funcTagNo(ByVal ItemId As Integer, ByVal piece As Integer) As Integer
        Dim Qry As String = Nothing
        Dim currentNo As String
        Dim dt As New DataTable
        dt.Clear()
        Try
            Select Case TagNoGen
                Case "I" ''From Item Master
                    Qry = " SELECT CURRENTTAGNO FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & ItemId & "'"
                Case "U" ''From SoftControl
                    Qry = " SELECT CTLTEXT AS CURRENTTAGNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LASTTAGNO'"
                Case Else
                    Exit Function
            End Select
            currentNo = objGPack.GetSqlValue(Qry, , "0", tran)
            startTagNo = Val(currentNo) + 1
            endTagNo = Val(currentNo) + piece
            currentTagNo = currentNo
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Private Sub txtStoneWT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStoneWT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtDiaPcs.Focus()
        End If
    End Sub

    Private Sub btnExit_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub txtPcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPcs.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtTagGrsWt_WET.Focus()
        End If
    End Sub

    Private Sub txtTagGrsWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagGrsWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtStonePcs.Focus()
        End If
    End Sub

    Private Sub txtStonePcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStonePcs.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtStoneWT.Focus()
        End If
    End Sub

    Private Sub gridTAG_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridTAG.UserDeletedRow
        dtGridLot.AcceptChanges()
        CalcGridtagTotal()
        If gridTAG.Rows.Count = 0 Then txtLotNo.Focus()
    End Sub

    Private Sub btnNew_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        fnew()
    End Sub

    Private Sub txtItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemId.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadSalesItemName()
        ElseIf e.KeyCode = Keys.Down Then
            gridTAG.Focus()
        ElseIf e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub txtSubItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSubItemId.KeyDown
        If e.KeyCode = Keys.Insert Then

        End If
    End Sub

    Private Sub LoadSalesItemName()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME,"
        strSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGGED' "
        strSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGGED' ELSE 'POCKET BASED' END AS STOCK_TYPE,"
        strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
        strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
        strSql += " WHEN CALTYPE = 'B' THEN 'BOTH' ELSE 'METAL RATE' END AS CAL_TYPE"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y'"
        strSql += GetItemQryFilteration()
        Dim itemId As Integer = Val(BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, , , txtItemId.Text))
        Dim itemname As String
        itemname = objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemId)
        If itemId > 0 Then
            txtItemId.Text = itemId
            txtItemname.Text = itemname
            LoadSubItemDetails()
        Else
            txtItemId.Focus()
            txtItemId.SelectAll()
        End If
    End Sub

    Private Sub LoadSubItemDetails()
        Dim itemid As Integer
        Dim subitemname As String
        Dim subitemid As Integer
        Dim iId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemname.Text & "'" & GetItemQryFilteration()))
        Itemid = iId
        If UCase(objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemname.Text & "'" & GetItemQryFilteration())) = "Y" Then
            txtSubItemId.Enabled = True
            txtSubItemName.Enabled = True
            Dim DefItem As String = subitemname
            If Not _SubItemOrderByName Then
                DefItem = objGPack.GetSqlValue("SELECT DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & subitemname & "' AND ITEMID = " & iId & "")
            End If
            strSql = GetSubItemQry(New String() {"SUBITEMID ID", "DISPLAYORDER DISP", "SUBITEMNAME SUBITEM"}, iId)
            subitemname = BrighttechPack.SearchDialog.Show("Search SubItem", strSql, cn, IIf(_SubItemOrderByName, 2, 1), 2, , DefItem, , False, True)
            subitemid = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & subitemname & "' and Itemid = " & itemid))
        Else
            txtSubItemId.Enabled = False
            txtSubItemName.Enabled = False
        End If
        If subitemid > 0 Then
            txtSubItemId.Text = subitemid
            txtSubItemName.Text = subitemname
        End If
        Me.SelectNextControl(txtSubItemName, True, True, True, True)
    End Sub

    Private Sub LoadDesignerName()
        strSql = " SELECT"
        strSql += " DESIGNERNAME,DESIGNERID "
        strSql += " FROM " & cnAdminDb & "..DESIGNER "
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " ORDER BY DESIGNERNAME"
        Dim Designer As String = BrighttechPack.SearchDialog.Show("Find Designer", strSql, cn, 1, , , txtDesigner.Text)
        If Designer <> "" Then
            txtDesigner.Text = Designer
        Else
            txtDesigner.Focus()
            txtDesigner.SelectAll()
        End If
        Me.SelectNextControl(txtDesigner, True, True, True, True)
    End Sub
    Private Sub txtDesigner_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDesigner.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadDesignerName()
        End If
    End Sub
    Private Sub Save_MRMI_ISSREC(ByVal batchno As String, ByVal catcode As String, ByVal type As String, ByVal OMaterialType As String, ByVal Itemid As Integer, ByVal Pcs As Decimal, ByVal Grswt As Decimal)
        Dim OrdStateId As Integer = 0
        Dim Tax As Decimal = 0 '
        Dim Tds As Decimal = 0 '
        Dim strsql As String
        Dim issSno As String = Nothing

        If OMaterialType = "I" Then
            issSno = GetNewSno(IIf(_AccAudit, TranSnoType.TISSUECODE, TranSnoType.ISSUECODE), tran)
        Else
            issSno = GetNewSno(IIf(_AccAudit, TranSnoType.TRECEIPTCODE, TranSnoType.RECEIPTCODE), tran)
        End If

        Dim wast As Decimal = Nothing
        Dim wastPer As Decimal = Nothing
        Dim alloy As Decimal = Nothing
        Dim itemTypeId As Integer = 0
        Dim tranno As Integer
        Dim subItemid As Integer = 0
        Dim billcontrolid As String
        If OMaterialType = "I" Then
            billcontrolid = "GEN-SM-REC"
            If _AccAudit Then billcontrolid = billcontrolid + "-APP"
            tranno = GetBillNoValue(billcontrolid, tran)
            strsql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TISSUE", "ISSUE")
        Else
            billcontrolid = "GEN-SM-REC"
            If _AccAudit Then billcontrolid = billcontrolid + "-APP"
            tranno = GetBillNoValue(billcontrolid, tran)
            strsql = " INSERT INTO " & cnStockDb & ".." & IIf(_AccAudit, "TRECEIPT", "RECEIPT")
        End If
        strsql += " ("
        strsql += "  SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
        strsql += " ,GRSWT,NETWT,LESSWT,PUREWT"
        strsql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
        strsql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
        strsql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
        strsql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
        strsql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
        strsql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
        strsql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
        strsql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
        strsql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
        strsql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,VATEXM,TAX,TDS"
        strsql += " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,TOUCH,ORDSTATE_ID"
        strsql += " )"
        strsql += " VALUES("
        strsql += " '" & issSno & "'" ''SNO
        strsql += " ," & tranno & "" 'TRANNO
        strsql += " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strsql += " ,'" & OMaterialType & "'"
        strsql += " ," & Pcs & "" 'PCS
        strsql += " ," & Grswt
        strsql += " ," & Grswt
        strsql += " ,0"
        strsql += " ,0"
        strsql += " ,''" 'TAGNO
        strsql += " ," & Itemid & "" 'ITEMID
        strsql += " ," & subItemid & "" 'SUBITEMID
        strsql += " ," & wastPer & "" 'WASTPER
        strsql += " ," & wast & "" 'WASTAGE
        strsql += " ,0"
        strsql += " ,0"
        strsql += " ,0"
        strsql += " ,0"
        strsql += " ,0"
        strsql += " ,''"
        strsql += " ,''"
        strsql += " ,''"
        strsql += " ,''"
        strsql += " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'REFDATE NULL
        strsql += " ,'" & BillCostId & "'" 'COSTID 
        strsql += " ,'" & strCompanyId & "'" 'COMPANYID
        strsql += " ,'" & type & "'" 'FLAG
        strsql += " ,0" 'EMPID
        strsql += " ,0" 'TAGGRSWT
        strsql += " ,0" 'TAGNETWT
        strsql += " ,0" 'TAGRATEID
        strsql += " ,0" 'TAGSVALUE
        strsql += " ,''" 'TAGDESIGNER  
        strsql += " ,0" 'ITEMCTRID
        strsql += " ," & itemTypeId & "" 'ITEMTYPEID
        strsql += " ,0"  'PURITY
        strsql += " ,''" 'TABLECODE
        strsql += " ,''" 'INCENTIVE
        strsql += " ,''" 'WEIGHTUNIT
        strsql += " ,'" & catcode & "'" 'CATCODE
        strsql += " ,'" & catcode & "'" 'OCATCODE
        strsql += " ,''"  'ACCODE
        strsql += " ," & alloy & "" 'ALLOY
        strsql += " ,'" & batchno & "'" 'BATCHNO
        strsql += " ,'STOCK CONVERSION'" 'REMARK1
        strsql += " ,''"
        strsql += " ,'" & userId & "'" 'USERID
        strsql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strsql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strsql += " ,'" & systemId & "'" 'SYSTEMID
        strsql += " ,0" 'DISCOUNT
        strsql += " ,''" 'RUNNO
        strsql += " ,''" 'CASHID
        strsql += " ,''" 'VATEXM
        strsql += " ," & Tax & "" 'TAX
        strsql += " ," & Tds & "" 'TDS
        strsql += " ,0" 'STNAMT
        strsql += " ,0" 'MISCAMT
        strsql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & Itemid, , , tran) & "'" 'METALID
        strsql += " ,''"
        strsql += " ,'" & VERSION & "'" 'APPVER
        strsql += " ,NULL"
        strsql += " ," & OrdStateId & "" 'ORDSTATE_ID
        strsql += " )"
        cmd = New OleDbCommand(strsql, cn, tran)
        cmd.ExecuteNonQuery()

        Dim Dpcs, Spcs, Dwt, Swt As Decimal
        ''Stone
        Dpcs = 0 : Spcs = 0 : Dwt = 0 : Swt = 0
        'For Each stRow As DataRow In CType(.Cells("METISSREC").Value, MaterialIssRec).objStone.dtGridStone.Rows
        'If Lotautopost = True Then
        '    StrSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & stRow.Item(2).ToString & "'"
        '    Dim METALID As String = objGPack.GetSqlValue(StrSql, , , tran).ToUpper
        '    Select Case METALID
        '        Case "D"
        '            Dpcs += Val(.Cells("PCS").Value.ToString)
        '            Dwt += .Cells("WEIGHT").Value.ToString
        '        Case "S"
        '            Spcs += Val(.Cells("PCS").Value.ToString)
        '            If .Cells("UNIT").Value = "G" Then Swt += Val(.Cells("WEIGHT").Value) Else Swt += Val(.Cells("WEIGHT").Value) / 5
        '        Case "P"
        '            Spcs += Val(.Cells("PCS").Value.ToString)
        '            If .Cells("UNIT").Value = "G" Then Swt += Val(.Cells("WEIGHT").Value) Else Swt += Val(.Cells("WEIGHT").Value) / 5
        '    End Select
        'End If

        'InsertStoneDetails(issSno, TranNo, stRow)
        'Next


    End Sub

    Private Sub txtItemId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim dt As New DataTable
            If Not txtItemId.Text = String.Empty Then
                strSql = "SELECT STOCKTYPE,ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & Val(txtItemId.Text)
                Dim dritem As DataRow
                dritem = GetSqlRow(strSql, cn)
                If dritem Is Nothing Then MsgBox("Invalid Item", MsgBoxStyle.Critical) : Exit Sub
                If dritem.Item(0).ToString <> "T" Then
                    strSql = vbCrLf + " SELECT 'N' AS TYPE,NULL AS SNO,0 AS LOTNO,ITEMID,ITEM AS ITEMNAME,0 AS SUBITEMID,NULL AS SUBITEMNAME,NULL AS DESIGNER,SUM(ISNULL(PCS,0))BALPCS,SUM(ISNULL(GRSWT,0))BALGRSWT"
                    strSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))NETWT,SUM(ISNULL(STNPCS,0))STNPCS,SUM(ISNULL(STNWT,0))STNWT,0 ITEMCTRID  FROM("
                    strSql += vbCrLf + " SELECT ITEMID,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)AS ITEM,"
                    strSql += vbCrLf + " SUBITEMID,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST  WHERE SUBITEMID =T.SUBITEMID)AS SUBITEM,"
                    strSql += vbCrLf + " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER  WHERE DESIGNERID =T.DESIGNERID)AS DESIGNER,"
                    strSql += vbCrLf + " SUM(CASE WHEN RECISS='R' THEN PCS ELSE (-1*PCS) END)PCS,"
                    strSql += vbCrLf + " SUM(CASE WHEN RECISS='R' THEN GRSWT ELSE (-1*GRSWT) END)GRSWT,"
                    strSql += vbCrLf + " SUM(CASE WHEN RECISS='R' THEN NETWT ELSE (-1*NETWT) END)NETWT,"
                    strSql += vbCrLf + " (SELECT SUM(STNPCS)FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE ITEMID=T.ITEMID AND ITEMID IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='T') )AS STNPCS,"
                    strSql += vbCrLf + " (SELECT SUM(STNWT)FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE ITEMID=T.ITEMID AND ITEMID IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='T') )AS STNWT"
                    strSql += vbCrLf + " (SELECT SUM(STNPCS)FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE ITEMID=T.ITEMID AND ITEMID IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D') )AS DIAPCS,"
                    strSql += vbCrLf + " (SELECT SUM(STNWT)FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE ITEMID=T.ITEMID AND ITEMID IN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D') )AS DIAWT"
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T WHERE T.ITEMID =" & Val(txtItemId.Text) & " AND COSTID='" & cnCostId & "'"
                    strSql += vbCrLf + " GROUP BY ITEMID,SUBITEMID,DESIGNERID "
                    strSql += vbCrLf + " ) X  "
                    strSql += vbCrLf + " GROUP BY ITEMID,ITEM"
                    strSql += vbCrLf + " HAVING(SUM(ISNULL(PCS, 0)) > 0 And SUM(ISNULL(GRSWT, 0)) > 0)"
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dt)
                    If Not dt.Rows.Count > 0 Then
                        MsgBox("Record Not Found", , "Brighttech Gold")
                        txtItemId.Clear()
                        Exit Sub
                    End If
                    LoadLotDetails(dt)
                Else
                    txtItemname.Text = dritem.Item(1).ToString
                    txtSubItemId.Focus()
                End If
            Else
                txtItemId.Focus()
            End If
        End If
    End Sub

    Private Sub btnSave_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnSave.KeyDown
        If e.KeyCode = Keys.Escape Then
            txtLotNo.Focus()
        End If
    End Sub

    Private Sub txtItemId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtItemId.TextChanged

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub


    Private Sub txtDiaWT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDiaWT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTAGRowIndex.Text <> "" Then
                dtGridLot.AcceptChanges()
                With dtGridLot.Rows(Val(txtTAGRowIndex.Text))
                    .Item("LOTNO") = Val(txtLotNo.Text)
                    .Item("ITEMID") = Val(txtItemId.Text)
                    .Item("ITEMNAME") = txtItemname.Text
                    .Item("SUBITEMID") = IIf(Val(txtSubItemId.Text) > 0, Val(txtSubItemId.Text), DBNull.Value)
                    .Item("SUBITEMNAME") = txtSubItemName.Text
                    .Item("DESIGNER") = txtDesigner.Text
                    .Item("PCS") = IIf(Val(txtPcs.Text) > 0, txtPcs.Text, DBNull.Value)
                    .Item("GRSWT") = IIf(Val(txtTagGrsWt_WET.Text) > 0, Val(txtTagGrsWt_WET.Text), DBNull.Value)
                    .Item("STNPCS") = IIf(Val(txtStonePcs.Text) > 0, txtStonePcs.Text, DBNull.Value)
                    .Item("STNWT") = IIf(Val(txtStoneWT.Text) > 0, txtStoneWT.Text, DBNull.Value)
                    .Item("DIAPCS") = IIf(Val(txtDiaPcs.Text) > 0, txtDiaPcs.Text, DBNull.Value)
                    .Item("DIAWT") = IIf(Val(txtDiaWT.Text) > 0, txtDiaWT.Text, DBNull.Value)
                End With
            Else
                Dim ro As DataRow = Nothing
                ro = dtGridLot.NewRow
                ro("LOTNO") = Val(txtLotNo.Text)
                ro("ITEMID") = Val(txtItemId.Text)
                ro("ITEMNAME") = txtItemname.Text
                ro("SUBITEMID") = IIf(Val(txtSubItemId.Text) > 0, Val(txtSubItemId.Text), DBNull.Value)
                ro("SUBITEMNAME") = txtSubItemName.Text
                ro("DESIGNER") = txtDesigner.Text
                ro("PCS") = IIf(Val(txtPcs.Text) > 0, txtPcs.Text, DBNull.Value)
                ro("GRSWT") = IIf(Val(txtTagGrsWt_WET.Text) > 0, Val(txtTagGrsWt_WET.Text), DBNull.Value)
                ro("STNPCS") = IIf(Val(txtStonePcs.Text) > 0, txtStonePcs.Text, DBNull.Value)
                ro("STNWT") = IIf(Val(txtStoneWT.Text) > 0, txtStoneWT.Text, DBNull.Value)
                ro("DIAPCS") = IIf(Val(txtDiaPcs.Text) > 0, txtDiaPcs.Text, DBNull.Value)
                ro("DIAWT") = IIf(Val(txtDiaWT.Text) > 0, txtDiaWT.Text, DBNull.Value)
                dtGridLot.Rows.Add(ro)
                dtGridLot.AcceptChanges()
                gridTAG.CurrentCell = gridTAG.Rows(gridTAG.RowCount - 1).Cells(1)
            End If
            CalcGridtagTotal()
            ClearTagDet()
            If txtTAGRowIndex.Text <> "" Then
                gridTAG.Focus()
            Else
                txtLotNo.Focus()
            End If
        End If
    End Sub

    Private Sub txtDiaPcs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDiaPcs.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'txtDiaWT.Select()
        End If
    End Sub
End Class

