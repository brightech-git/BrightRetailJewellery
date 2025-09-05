Imports System.Data.OleDb
Imports System.IO
Public Class TagMerge

    Dim cmd As OleDbCommand
    Public objSoftKeys As New EstimationSoftKeys
    Dim objTagFiltration As New frmTagFiltration
    Dim objItemDetail As New TagMergeItemDet
    Dim dtTagDetail As New DataTable("GridTag")
    Dim dtStoneDetails As New DataTable("GridStone")
    Public BillCostId As String = cnCostId

    Dim dtGridTAG As New DataTable
    Dim dtGridTAGSTONE As New DataTable
    Dim RowTagMargin As DataRow = Nothing

    Public BillDate As Date = GetServerDate(tran)
    Dim objTag As New TagGeneration

    Dim taggrstwt As Decimal = 0
    Dim tagStoneWeight As Decimal = 0

    Dim tagSno As String = Nothing
    Dim tagStoneSno As String = Nothing
    Dim LOTSNO As String

    Dim OldTagGrswt As Decimal = 0
    Dim OldTagNetwt As Decimal = 0
    Dim OldTagAmount As Double = 0

    Dim OldTagStoneWt As Decimal = 0
    Dim OldTagStoneAmt As Double = 0
    Dim ObjMinValue As New TagMinValues

    Dim CallBarcodeExe As Boolean = IIf(GetAdmindbSoftValue("CALLBARCODEEXE", "N") = "Y", True, False)

    Dim strSql As String

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        With dtGridTAG
            .Columns.Add("SNO", GetType(String))
            .Columns.Add("ITEMID", GetType(Integer))
            .Columns.Add("TAGNO", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("OLDGRSWT", GetType(Decimal))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("OLDNETWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("RATE", GetType(Double))
            .Columns.Add("STNWT", GetType(Decimal))
            .Columns.Add("STNAMT", GetType(Double))
            .Columns.Add("DIAWT", GetType(Decimal))
            .Columns.Add("DIAAMT", GetType(Double))
            .Columns.Add("OLDAMOUNT", GetType(Double))
            .Columns.Add("AMOUNT", GetType(Double))
        End With
        dtGridTAG.AcceptChanges()
        gridTAG.DataSource = dtGridTAG
        gridTAG.ColumnHeadersVisible = False
        FormatGridColumns(gridTAG)
        StyleGridTag(gridTAG)

        Dim dtGridtagTotal As New DataTable
        dtGridtagTotal = dtGridTAG.Copy
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


        With dtGridTAGSTONE
            .Columns.Add("SNO", GetType(String))
            .Columns.Add("ITEMID", GetType(Integer))
            .Columns.Add("TAGNO", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("OLDSTNWT", GetType(Decimal))
            .Columns.Add("STNWT", GetType(Decimal))
            .Columns.Add("RATE", GetType(Double))
            .Columns.Add("OLDSTNAMT", GetType(Double))
            .Columns.Add("STNAMT", GetType(Double))
        End With
        dtGridTAGSTONE.AcceptChanges()
        gridTagStone.DataSource = dtGridTAGSTONE
        gridTagStone.ColumnHeadersVisible = False
        FormatGridColumns(gridTagStone)
        StyleGridTagStone(gridTagStone)

        Dim dtGridtagstoneTotal As New DataTable
        dtGridtagstoneTotal = dtGridTAGSTONE.Copy
        dtGridtagstoneTotal.Rows.Clear()
        dtGridtagstoneTotal.Rows.Add()
        gridTagStonetotal.ColumnHeadersVisible = False
        gridTagStonetotal.DataSource = dtGridtagstoneTotal
        For Each col1 As DataGridViewColumn In gridTagStonetotal.Columns
            With gridTagStonetotal.Columns(col1.Name)
                .Visible = col1.Visible
                .Width = col1.Width
                .DefaultCellStyle = col1.DefaultCellStyle
            End With
        Next
        CalcGridtagStoneTotal()
        StyleGridTagStone(gridTagStonetotal)
        txtTagItemId.Focus()
    End Sub
    Private Sub CalcGridtagTotal()

        Dim OldtagAmt As Double = Nothing
        Dim tagAmt As Double = Nothing
        Dim tagPcs As Integer = Nothing
        Dim OldtagGrsWt As Decimal = Nothing
        Dim tagGrsWt As Decimal = Nothing
        Dim OldtagNetWt As Decimal = Nothing
        Dim tagNetWt As Decimal = Nothing
        Dim tagStnWt As Decimal = Nothing
        Dim tagStnAmt As Double = Nothing
        Dim tagDiaWt As Decimal = Nothing
        Dim tagDiaAmt As Double = Nothing

        For i As Integer = 0 To gridTAG.RowCount - 1
            With gridTAG.Rows(i)
                .DefaultCellStyle.BackColor = SystemColors.HighlightText
                tagPcs += Val(.Cells("PCS").Value.ToString)
                OldtagGrsWt += Val(.Cells("OLDGRSWT").Value.ToString)
                tagGrsWt += Val(.Cells("GRSWT").Value.ToString)
                OldtagNetWt += Val(.Cells("OLDNETWT").Value.ToString)
                tagNetWt += Val(.Cells("NETWT").Value.ToString)
                tagStnAmt += Val(.Cells("STNAMT").Value.ToString)
                tagStnWt += Val(.Cells("STNWT").Value.ToString)
                tagDiaAmt += Val(.Cells("DIAAMT").Value.ToString)
                tagDiaWt += Val(.Cells("DIAWT").Value.ToString)
                OldtagAmt += Val(.Cells("OLDAMOUNT").Value.ToString)
                tagAmt += Val(.Cells("AMOUNT").Value.ToString)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
            End With
        Next
        'Dim tranTypes As New List(Of String)
        'For Each ro As DataRow In dtGridTAG.Rows
        '    If ro!ENTFLAG.ToString = "" Then Exit For
        '    'If ro("ENTFLAG").ToString = "" Then Continue For
        '    tranTypes.Add(ro("TRANTYPE").ToString)
        'Next
        '        gridTAGTotal.DefaultCellStyle.BackColor = grpHeader.BackgroundColor
        '       gridTAGTotal.DefaultCellStyle.SelectionBackColor = grpHeader.BackgroundColor


        With gridTAGTotal.Rows(0)
            .Cells("TAGNO").Value = "TOTAL"
            .Cells("PCS").Value = IIf(tagPcs <> 0, tagPcs, DBNull.Value)
            .Cells("OLDGRSWT").Value = IIf(OldtagGrsWt <> 0, OldtagGrsWt, DBNull.Value)
            .Cells("GRSWT").Value = IIf(tagGrsWt <> 0, tagGrsWt, DBNull.Value)
            .Cells("OLDNETWT").Value = IIf(OldtagNetWt <> 0, OldtagNetWt, DBNull.Value)
            .Cells("NETWT").Value = IIf(tagNetWt <> 0, tagNetWt, DBNull.Value)
            .Cells("STNWT").Value = IIf(tagStnWt <> 0, tagStnWt, DBNull.Value)
            .Cells("STNAMT").Value = IIf(tagStnAmt <> 0, tagStnAmt, DBNull.Value)
            .Cells("DIAWT").Value = IIf(tagDiaWt <> 0, tagDiaWt, DBNull.Value)
            .Cells("DIAAMT").Value = IIf(tagDiaAmt <> 0, tagDiaAmt, DBNull.Value)
            .Cells("OLDAMOUNT").Value = IIf(OldtagAmt <> 0, OldtagAmt, DBNull.Value)
            .Cells("AMOUNT").Value = IIf(tagAmt <> 0, tagAmt, DBNull.Value)
            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .DefaultCellStyle.ForeColor = Color.Blue
            .DefaultCellStyle.SelectionForeColor = Color.Blue
        End With
    End Sub
    Private Sub StyleGridTag(ByVal gridTagView As DataGridView)
        With gridTagView
            .Columns("ITEMID").Width = txtTagItemId.Width + 1
            .Columns("TAGNO").Width = txtTagTagNo.Width + 1
            .Columns("PCS").Width = txtTagPcs_NUM.Width + 1
            .Columns("OLDGRSWT").Width = txtTagOldGrsWt_WET.Width + 1
            .Columns("GRSWT").Width = txtTagGrsWt_WET.Width + 1
            .Columns("OLDNETWT").Width = txtTagOldNetWt_WET.Width + 1
            .Columns("NETWT").Width = txtTagNetWt_WET.Width + 1
            .Columns("RATE").Width = txtTagRate_AMT.Width + 1
            .Columns("STNWT").Width = txttAGStoneWt_WET.Width + 1
            .Columns("STNAMT").Width = txttAGStoneAmount_AMT.Width + 1
            .Columns("DIAWT").Width = txttAGDiaWt_WET.Width + 1
            .Columns("DIAAMT").Width = txttAGDiaAmount_AMT.Width + 1
            .Columns("OLDAMOUNT").Width = txtTagOldAmount_AMT.Width + 1
            .Columns("AMOUNT").Width = txtTagAmount_AMT.Width + 1
            For i As Integer = 15 To gridTagView.Columns.Count - 1
                .Columns(i).Visible = False
            Next
            .Columns("SNO").Visible = False
        End With
    End Sub
    Private Sub StyleGridTagStone(ByVal gridTagStoneView As DataGridView)
        With gridTagStoneView
            .Columns("ITEMID").Width = txtStItem.Width + 1
            .Columns("TAGNO").Width = txtStTagno.Width + 1
            .Columns("RATE").Width = txtStRate_Amt.Width + 1
            .Columns("PCS").Width = txtStPcs_NUM.Width + 1
            .Columns("OLDSTNWT").Width = txtStOldWeight_WET.Width + 1
            .Columns("STNWT").Width = txtStWeight_WET.Width + 1
            .Columns("OLDSTNAMT").Width = txtStOldAmount_Amt.Width + 1
            .Columns("STNAMT").Width = txtStAmount_Amt.Width + 1

            For i As Integer = 15 To gridTagStoneView.Columns.Count - 1
                .Columns(i).Visible = False
            Next
            .Columns("SNO").Visible = False
        End With
        gridTagStoneView.Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridTagStoneView.Columns("STNAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridTagStoneView.Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridTagStoneView.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

    End Sub
    Private Sub CalcGridtagStoneTotal()

        Dim tagAmt As Double = Nothing
        Dim tagPcs As Integer = Nothing
        Dim tagStnOldWt As Decimal = Nothing
        Dim tagStnWt As Decimal = Nothing
        Dim tagStnOldAmt As Double = Nothing
        Dim tagStnAmt As Double = Nothing

        For i As Integer = 0 To gridTagStone.RowCount - 1
            With gridTagStone.Rows(i)
                .DefaultCellStyle.BackColor = SystemColors.HighlightText
                tagPcs += Val(.Cells("PCS").Value.ToString)
                tagStnOldAmt += Val(.Cells("OLDSTNAMT").Value.ToString)
                tagStnAmt += Val(.Cells("STNAMT").Value.ToString)
                tagStnOldWt += Val(.Cells("OLDSTNWT").Value.ToString)
                tagStnWt += Val(.Cells("STNWT").Value.ToString)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
            End With
        Next
        'gridTAGTotal.DefaultCellStyle.BackColor = grpHeader.BackgroundColor
        'gridTAGTotal.DefaultCellStyle.SelectionBackColor = grpHeader.BackgroundColor

        With gridTagStonetotal.Rows(0)
            .Cells("TAGNO").Value = "TOTAL"
            .Cells("PCS").Value = IIf(tagPcs <> 0, tagPcs, DBNull.Value)
            .Cells("OLDSTNWT").Value = IIf(tagStnOldWt <> 0, tagStnOldWt, DBNull.Value)
            .Cells("STNWT").Value = IIf(tagStnWt <> 0, tagStnWt, DBNull.Value)
            .Cells("OLDSTNAMT").Value = IIf(tagStnOldAmt <> 0, tagStnOldAmt, DBNull.Value)
            .Cells("STNAMT").Value = IIf(tagStnAmt <> 0, tagStnAmt, DBNull.Value)
            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .DefaultCellStyle.ForeColor = Color.Blue
            .DefaultCellStyle.SelectionForeColor = Color.Blue
        End With
    End Sub
    Private Sub TagMerge_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTagItemId.Focused Then Exit Sub
            If txtTagTagNo.Focused Then Exit Sub
            If txtTagOldGrsWt_WET.Focused Then Exit Sub
            If txtTagGrsWt_WET.Focused Then Exit Sub
            If txtTagOldNetWt_WET.Focused Then Exit Sub
            If txtTagNetWt_WET.Focused Then Exit Sub
            If txtTagRate_AMT.Focused Then Exit Sub
            If txttAGStoneAmount_AMT.Focused Then Exit Sub
            If txttAGStoneWt_WET.Focused Then Exit Sub
            If txttAGDiaAmount_AMT.Focused Then Exit Sub
            If txttAGDiaWt_WET.Focused Then Exit Sub
            If txtTagOldAmount_AMT.Focused Then Exit Sub
            If txtTagAmount_AMT.Focused Then Exit Sub

            If rbtGenerateNewTag.Focused Then Exit Sub
            If rbtInterchange.Focused Then Exit Sub

            If txtStItem.Focused Then Exit Sub
            If txtStTagno.Focused Then Exit Sub
            If txtStPcs_NUM.Focused Then Exit Sub
            If txtStOldWeight_WET.Focused Then Exit Sub
            If txtStWeight_WET.Focused Then Exit Sub
            If txtStRate_Amt.Focused Then Exit Sub
            If txtStOldAmount_Amt.Focused Then Exit Sub
            If txtStAmount_Amt.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub TagMerge_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        gridTAG.Font = New Font("VERDANA", 8, FontStyle.Bold)
        StyleGridTag(gridTAG)
        StyleGridTag(gridTAGTotal)
        txtTagItemId.Focus()
    End Sub


    Private Sub LoadItemName()
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
        Dim itemId As Integer

        itemId = Val(BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, , , txtTagItemId.Text))
        If itemId > 0 Then
            txtTagItemId.Text = itemId
        Else
            txtTagItemId.Focus()
            txtTagItemId.SelectAll()
        End If

    End Sub


    Private Sub txtTagTagNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTagTagNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            If Val(txtTagItemId.Text) = 0 Then Exit Sub
            Dim stockType As String = objGPack.GetSqlValue("SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & txtTagItemId.Text & "'")
            If stockType = "T" Then
                strSql = " SELECT"
                strSql += " TAGNO AS TAGNO,STYLENO,ITEMID AS ITEMID,"
                strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME,"
                strSql += " PCS AS PCS,"
                strSql += " GRSWT AS GRS_WT,NETWT AS NET_WT,RATE AS RATE,"
                strSql += " SALVALUE AS SALVALUE,TAGVAL AS TAGVAL,"
                strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID  = T.SUBITEMID),'')AS SUBITEM,"
                strSql += " CONVERT(VARCHAR,RECDATE,103) AS RECDATE,"
                strSql += " CASE WHEN APPROVAL = 'A' THEN 'APPROVAL' WHEN APPROVAL = 'R' THEN 'RESERVED' ELSE NULL END AS STATUS,"
                strSql += " CASE WHEN APPROVAL = 'A' THEN '" & Color.MistyRose.Name & "' WHEN APPROVAL = 'R' THEN '" & Color.PaleTurquoise.Name & "' ELSE NULL END AS COLOR_HIDE"
                strSql += " FROM"
                strSql += " " & cnAdminDb & "..ITEMTAG AS T"
                strSql += " WHERE T.ITEMID = " & Val(txtTagItemId.Text) & ""
                If cnHOCostId <> BillCostId Then strSql += vbCrLf + " AND COSTID = '" & BillCostId & "'"
                If cnCostId <> "" Then
                    strSql += vbCrLf + " AND T.COSTID = '" & cnCostId & "'"
                End If
                strSql += GridTagNoFiltStr(dtTagDetail)
                'strSql += ShowTagFiltration()
                strSql += " AND ISSDATE IS NULL AND ISNULL(APPROVAL,'')<>'A'"
                strSql += " ORDER BY TAGNO"
                txtTagTagNo.Text = BrighttechPack.SearchDialog.Show("Find TagNo", strSql, cn, , , , , , , , False)
                txtTagTagNo.SelectAll()
            ElseIf stockType = "P" Then
                strSql = " SELECT"
                strSql += " PACKETNO AS PACKETNO,ITEMID,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = NT.ITEMID) AS ITEMNAME"
                strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END)AS PCS"
                strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END)AS GRSWT"
                strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END)AS NETWT"
                strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID  = NT.SUBITEMID),'')AS SUBITEM"
                strSql += " FROM " & cnAdminDb & "..ITEMNONTAG AS NT"
                strSql += " WHERE NT.ITEMID = " & Val(txtTagItemId.Text) & ""
                strSql += vbCrLf + " AND COSTID = '" & BillCostId & "'"
                If cnCostId <> "" Then
                    strSql += vbCrLf + " AND NT.COSTID = '" & cnCostId & "'"
                End If
                strSql += " GROUP BY PACKETNO,ITEMID,SUBITEMID"
                strSql += " HAVING SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) > 0"
                strSql += " AND SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END)> 0"
                strSql += " AND SUM(CASE WHEN RECISS = 'R' THEN NETWT ELSE -1*NETWT END)> 0"
                txtTagTagNo.Text = BrighttechPack.SearchDialog.Show("Find PacketNo", strSql, cn)
                'txtRate_AMT.Text = GetSARate()
                txtTagTagNo.SelectAll()
            End If
        End If
    End Sub

    Private Sub txtTagTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagTagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Len(txtTagTagNo.Text) > 11 Then MsgBox("Given Tag No Length is Exceed") : Exit Sub
            Dim dtTagDet As New DataTable
            strSql = vbCrLf + " SELECT T.SNO AS SNO"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM,T.TAGNO AS TAGNO,T.ITEMID AS ITEMID,PCS,GRSWT"
            strSql += vbCrLf + " ,NETWT,LESSWT,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO "
            strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT "
            strSql += vbCrLf + " ,(SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO "
            strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNAMT"
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO "
            strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT "
            strSql += vbCrLf + " ,(SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO "
            strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAAMT,BOARDRATE AS RATE,SALVALUE AS AMOUNT,LOTSNO "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAGSTONE AS S ON T.SNO=S.TAGSNO"
            strSql += vbCrLf + " WHERE T.TAGNO = '" & txtTagTagNo.Text & "' AND ISNULL(T.ISSDATE,'')='' AND ISNULL(APPROVAL,'')<>'A'"
            If txtTagItemId.Text <> "" Then
                strSql += " AND T.ITEMID = " & Val(txtTagItemId.Text) & ""
            End If
            If cnCostId <> "" Then
                strSql += vbCrLf + " AND T.COSTID = '" & cnCostId & "'"
            End If
            If Not cnCentStock Then strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTagDet)
            If dtTagDet.Rows.Count = 0 Then
                MsgBox("Invalid Itemid of Tagno", MsgBoxStyle.Information)
                txtTagTagNo.Focus()
                Exit Sub
            End If

            Dim rwIndex As Integer = -1
            For Each ro As DataRow In dtGridTAG.Rows
                rwIndex += 1
                If ro("ITEMID").ToString <> "" Then
                    If txtTAGRowIndex.Text <> "" And rwIndex = Val(txtTAGRowIndex.Text) Then
                        Continue For
                    End If
                    If ro("ITEMID").ToString = txtTagItemId.Text _
                    And ro("TAGNO").ToString = txtTagTagNo.Text Then
                        MsgBox(" This Tag Already Loaded in Grid", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                End If
            Next
            dtGridTAG.AcceptChanges()
            Dim tranType As String = Nothing
            Dim index As Integer = Nothing

            If txtTAGRowIndex.Text = "" Then
                Dim ro As DataRow = Nothing
                ro = dtGridTAG.NewRow
                ro("SNO") = IIf(dtTagDet.Rows(0).Item("SNO").ToString <> "", dtTagDet.Rows(0).Item("SNO").ToString, DBNull.Value)
                ro("ITEMID") = IIf(Val(dtTagDet.Rows(0).Item("ITEMID").ToString) > 0, dtTagDet.Rows(0).Item("ITEMID").ToString, DBNull.Value)
                ro("TAGNO") = IIf(dtTagDet.Rows(0).Item("TAGNO").ToString <> "B" And dtTagDet.Rows(0).Item("TAGNO").ToString <> "C", dtTagDet.Rows(0).Item("TAGNO").ToString, DBNull.Value)
                ro("PCS") = IIf(Val(dtTagDet.Rows(0).Item("PCS").ToString) > 0, dtTagDet.Rows(0).Item("PCS").ToString, DBNull.Value)
                ro("GRSWT") = IIf(Val(dtTagDet.Rows(0).Item("GRSWT").ToString) > 0, dtTagDet.Rows(0).Item("GRSWT").ToString, DBNull.Value)
                ro("OLDGRSWT") = IIf(Val(dtTagDet.Rows(0).Item("GRSWT").ToString) > 0, dtTagDet.Rows(0).Item("GRSWT").ToString, 0)
                ro("NETWT") = IIf(Val(dtTagDet.Rows(0).Item("NETWT").ToString) > 0, dtTagDet.Rows(0).Item("NETWT").ToString, DBNull.Value)
                ro("OLDNETWT") = IIf(Val(dtTagDet.Rows(0).Item("NETWT").ToString) > 0, dtTagDet.Rows(0).Item("NETWT").ToString, 0)
                ro("RATE") = IIf(Val(dtTagDet.Rows(0).Item("RATE").ToString) > 0, dtTagDet.Rows(0).Item("RATE").ToString, DBNull.Value)
                ro("STNWT") = IIf(Val(dtTagDet.Rows(0).Item("STNWT").ToString) > 0, dtTagDet.Rows(0).Item("STNWT").ToString, DBNull.Value)
                ro("STNAMT") = IIf(Val(dtTagDet.Rows(0).Item("STNAMT").ToString) > 0, dtTagDet.Rows(0).Item("STNAMT").ToString, DBNull.Value)
                ro("DIAWT") = IIf(Val(dtTagDet.Rows(0).Item("DIAWT").ToString) > 0, dtTagDet.Rows(0).Item("DIAWT").ToString, DBNull.Value)
                ro("DIAAMT") = IIf(Val(dtTagDet.Rows(0).Item("DIAAMT").ToString) > 0, dtTagDet.Rows(0).Item("DIAAMT").ToString, DBNull.Value)
                ro("AMOUNT") = IIf(Val(dtTagDet.Rows(0).Item("AMOUNT").ToString) > 0, dtTagDet.Rows(0).Item("AMOUNT").ToString, DBNull.Value)
                ro("OLDAMOUNT") = IIf(Val(dtTagDet.Rows(0).Item("AMOUNT").ToString) > 0, dtTagDet.Rows(0).Item("AMOUNT").ToString, 0)
                LOTSNO = dtTagDet.Rows(0).Item("LOTSNO").ToString
                dtGridTAG.Rows.Add(ro)
                dtGridTAG.AcceptChanges()
                gridTAG.CurrentCell = gridTAG.Rows(0).Cells("ITEMID")
                'Exit For
            Else
                'update
                With dtGridTAG.Rows(Val(txtTAGRowIndex.Text))
                    .Item("ITEMID") = IIf(dtTagDet.Rows(0).Item("SNO").ToString <> "", dtTagDet.Rows(0).Item("SNO").ToString, DBNull.Value)
                    .Item("ITEMID") = IIf(Val(dtTagDet.Rows(0).Item("ITEMID").ToString) > 0, dtTagDet.Rows(0).Item("ITEMID").ToString, DBNull.Value)
                    .Item("TAGNO") = IIf(dtTagDet.Rows(0).Item("TAGNO").ToString <> "B" And dtTagDet.Rows(0).Item("TAGNO").ToString <> "C", dtTagDet.Rows(0).Item("TAGNO").ToString, DBNull.Value)
                    .Item("PCS") = IIf(Val(dtTagDet.Rows(0).Item("PCS").ToString) > 0, dtTagDet.Rows(0).Item("PCS").ToString, DBNull.Value)
                    .Item("GRSWT") = IIf(Val(dtTagDet.Rows(0).Item("GRSWT").ToString) > 0, dtTagDet.Rows(0).Item("GRSWT").ToString, DBNull.Value)
                    .Item("OLDGRSWT") = IIf(Val(dtTagDet.Rows(0).Item("GRSWT").ToString) > 0, dtTagDet.Rows(0).Item("GRSWT").ToString, 0)
                    .Item("NETWT") = IIf(Val(dtTagDet.Rows(0).Item("NETWT").ToString) > 0, dtTagDet.Rows(0).Item("NETWT").ToString, DBNull.Value)
                    .Item("OLDNETWT") = IIf(Val(dtTagDet.Rows(0).Item("NETWT").ToString) > 0, dtTagDet.Rows(0).Item("NETWT").ToString, 0)
                    .Item("RATE") = IIf(Val(dtTagDet.Rows(0).Item("RATE").ToString) > 0, dtTagDet.Rows(0).Item("RATE").ToString, DBNull.Value)
                    .Item("STNWT") = IIf(Val(dtTagDet.Rows(0).Item("STNWT").ToString) > 0, dtTagDet.Rows(0).Item("STNWT").ToString, DBNull.Value)
                    .Item("STNAMT") = IIf(Val(dtTagDet.Rows(0).Item("STNAMT").ToString) > 0, dtTagDet.Rows(0).Item("STNAMT").ToString, DBNull.Value)
                    .Item("DIAWT") = IIf(Val(dtTagDet.Rows(0).Item("DIAWT").ToString) > 0, dtTagDet.Rows(0).Item("DIAWT").ToString, DBNull.Value)
                    .Item("DIAAMT") = IIf(Val(dtTagDet.Rows(0).Item("DIAAMT").ToString) > 0, dtTagDet.Rows(0).Item("DIAAMT").ToString, DBNull.Value)
                    .Item("AMOUNT") = IIf(Val(dtTagDet.Rows(0).Item("AMOUNT").ToString) > 0, dtTagDet.Rows(0).Item("AMOUNT").ToString, DBNull.Value)
                    .Item("OLDAMOUNT") = IIf(Val(dtTagDet.Rows(0).Item("AMOUNT").ToString) > 0, dtTagDet.Rows(0).Item("AMOUNT").ToString, 0)
                    index = Val(txtTAGRowIndex.Text)
                End With
            End If

            CalcGridtagTotal()
            Dim dtTagStoneDet As New DataTable
            strSql = " SELECT SNO,ITEMID,TAGNO,STNPCS,STNRATE AS RATE,STNWT,STNAMT FROM " & cnAdminDb & "..ITEMTAGSTONE "
            strSql += vbCrLf + " WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..ITEMTAG "
            strSql += vbCrLf + " WHERE TAGNO = '" & txtTagTagNo.Text & "' AND ISNULL(ISSDATE,'')='' AND ISNULL(APPROVAL,'')<>'A'"
            If txtTagItemId.Text <> "" Then
                strSql += " AND ITEMID = " & Val(txtTagItemId.Text) & ""
            End If
            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            strSql += vbCrLf + " )"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTagStoneDet)

            dtGridTAGSTONE.AcceptChanges()
            If dtTagStoneDet.Rows.Count > 0 Then
                TabControlstone.Visible = True
                grpStoneDetails.Visible = True
                For i As Integer = 0 To dtTagStoneDet.Rows.Count - 1
                    If txtStRowIndex.Text = "" Then
                        Dim row As DataRow = Nothing
                        row = dtGridTAGSTONE.NewRow
                        row("SNO") = IIf(dtTagStoneDet.Rows(i).Item("SNO").ToString <> "", dtTagStoneDet.Rows(i).Item("SNO").ToString, DBNull.Value)
                        row("ITEMID") = IIf(Val(dtTagStoneDet.Rows(i).Item("ITEMID").ToString) > 0, dtTagStoneDet.Rows(i).Item("ITEMID").ToString, DBNull.Value)
                        row("TAGNO") = IIf(dtTagStoneDet.Rows(i).Item("TAGNO").ToString <> "B" And dtTagStoneDet.Rows(i).Item("TAGNO").ToString <> "C", dtTagStoneDet.Rows(i).Item("TAGNO").ToString, DBNull.Value)
                        row("PCS") = IIf(Val(dtTagStoneDet.Rows(i).Item("STNPCS").ToString) > 0, dtTagStoneDet.Rows(i).Item("STNPCS").ToString, DBNull.Value)
                        row("STNWT") = IIf(Val(dtTagStoneDet.Rows(i).Item("STNWT").ToString) > 0, dtTagStoneDet.Rows(i).Item("STNWT").ToString, DBNull.Value)
                        row("OLDSTNWT") = IIf(Val(dtTagStoneDet.Rows(0).Item("STNWT").ToString) > 0, dtTagStoneDet.Rows(0).Item("STNWT").ToString, 0)
                        row("RATE") = IIf(Val(dtTagStoneDet.Rows(i).Item("RATE").ToString) > 0, dtTagStoneDet.Rows(i).Item("RATE").ToString, DBNull.Value)
                        row("STNAMT") = IIf(Val(dtTagStoneDet.Rows(i).Item("STNAMT").ToString) > 0, dtTagStoneDet.Rows(i).Item("STNAMT").ToString, DBNull.Value)
                        row("OLDSTNAMT") = IIf(Val(dtTagStoneDet.Rows(i).Item("STNAMT").ToString) > 0, dtTagStoneDet.Rows(i).Item("STNAMT").ToString, 0)
                        dtGridTAGSTONE.Rows.Add(row)
                        dtGridTAGSTONE.AcceptChanges()
                        gridTagStone.CurrentCell = gridTagStone.Rows(i).Cells("ITEMID")
                        'Exit For
                    Else
                        'Dim row As DataRow = Nothing
                        'row = dtGridTAGSTONE.NewRow
                        'row("SNO") = IIf(dtTagStoneDet.Rows(i).Item("SNO").ToString <> "", dtTagStoneDet.Rows(i).Item("SNO").ToString, DBNull.Value)
                        'row("ITEMID") = IIf(Val(dtTagStoneDet.Rows(i).Item("ITEMID").ToString) > 0, dtTagStoneDet.Rows(i).Item("ITEMID").ToString, DBNull.Value)
                        'row("TAGNO") = IIf(dtTagStoneDet.Rows(i).Item("TAGNO").ToString <> "B" And dtTagStoneDet.Rows(i).Item("TAGNO").ToString <> "C", dtTagStoneDet.Rows(i).Item("TAGNO").ToString, DBNull.Value)
                        'row("PCS") = IIf(Val(dtTagStoneDet.Rows(i).Item("STNPCS").ToString) > 0, dtTagStoneDet.Rows(i).Item("STNPCS").ToString, DBNull.Value)
                        'row("STNWT") = IIf(Val(dtTagStoneDet.Rows(i).Item("STNWT").ToString) > 0, dtTagStoneDet.Rows(i).Item("STNWT").ToString, DBNull.Value)
                        'OldTagStoneWt = OldTagStoneWt + IIf(Val(dtTagStoneDet.Rows(0).Item("STNWT").ToString) > 0, dtTagStoneDet.Rows(0).Item("STNWT").ToString, 0)
                        'row("RATE") = IIf(Val(dtTagStoneDet.Rows(i).Item("RATE").ToString) > 0, dtTagStoneDet.Rows(i).Item("RATE").ToString, DBNull.Value)
                        'row("STNAMT") = IIf(Val(dtTagStoneDet.Rows(i).Item("STNAMT").ToString) > 0, dtTagStoneDet.Rows(i).Item("STNAMT").ToString, DBNull.Value)
                        'OldTagStoneAmt = OldTagStoneAmt + IIf(Val(dtTagStoneDet.Rows(i).Item("STNAMT").ToString) > 0, dtTagStoneDet.Rows(i).Item("STNAMT").ToString, 0)
                        'dtGridTAGSTONE.Rows.Add(row)
                        'dtGridTAGSTONE.AcceptChanges()
                        'gridTagStone.CurrentCell = gridTagStone.Rows(i).Cells("ITEMID")
                        'update
                        With dtGridTAG.Rows(Val(txtStRowIndex.Text))
                            .Item("SNO") = IIf(dtTagStoneDet.Rows(i).Item("SNO").ToString <> "", dtTagStoneDet.Rows(i).Item("SNO").ToString, DBNull.Value)
                            .Item("ITEMID") = IIf(Val(dtTagStoneDet.Rows(i).Item("ITEMID").ToString) > 0, dtTagStoneDet.Rows(i).Item("ITEMID").ToString, DBNull.Value)
                            .Item("TAGNO") = IIf(dtTagStoneDet.Rows(i).Item("TAGNO").ToString <> "B" And dtTagStoneDet.Rows(i).Item("TAGNO").ToString <> "C", dtTagStoneDet.Rows(i).Item("TAGNO").ToString, DBNull.Value)
                            .Item("PCS") = IIf(Val(dtTagStoneDet.Rows(i).Item("STNPCS").ToString) > 0, dtTagStoneDet.Rows(i).Item("STNPCS").ToString, DBNull.Value)
                            .Item("STNWT") = IIf(Val(dtTagStoneDet.Rows(i).Item("STNWT").ToString) > 0, dtTagStoneDet.Rows(i).Item("STNWT").ToString, DBNull.Value)
                            .Item("OLDSTNWT") = IIf(Val(dtTagStoneDet.Rows(i).Item("STNWT").ToString) > 0, dtTagStoneDet.Rows(i).Item("STNWT").ToString, 0)
                            .Item("RATE") = IIf(Val(dtTagStoneDet.Rows(i).Item("RATE").ToString) > 0, dtTagStoneDet.Rows(i).Item("RATE").ToString, DBNull.Value)
                            .Item("STNAMT") = IIf(Val(dtTagStoneDet.Rows(i).Item("STNAMT").ToString) > 0, dtTagStoneDet.Rows(i).Item("STNAMT").ToString, DBNull.Value)
                            .Item("OLDSTNAMT") = IIf(Val(dtTagStoneDet.Rows(0I).Item("STNAMT").ToString) > 0, dtTagStoneDet.Rows(i).Item("STNAMT").ToString, 0)
                            index = Val(txtTAGRowIndex.Text)
                        End With
                    End If
                Next
                CalcGridtagStoneTotal()
            End If

            txtTagItemId.Clear()
            txtTagTagNo.Clear()
            txtTagItemId.Focus()
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            rbtGenerateNewTag.Focus()
        End If
    End Sub
    Private Function GridTagNoFiltStr(ByVal dt As DataTable) As String
        Dim retStr As String = Nothing
        Dim itemId As String = Nothing
        Dim tagNo As String = Nothing
        For cnt As Integer = 0 To dt.Rows.Count - 1
            If Val(dt.Rows(cnt).Item("ITEMID").ToString) = 0 Then Continue For
            itemId += Val(dt.Rows(cnt).Item("ITEMID").ToString).ToString + ","
            tagNo += "'" & dt.Rows(cnt).Item("TAGNO").ToString & "'" + ","
        Next
        If itemId <> Nothing Then
            If itemId.EndsWith(",") Then itemId = itemId.Remove(itemId.Length - 1, 1)
            If tagNo.EndsWith(",") Then tagNo = tagNo.Remove(tagNo.Length - 1, 1)
            retStr += " AND (ITEMID NOT IN (" & itemId & ") OR TAGNO NOT IN (" & tagNo & "))"
        End If
        Return retStr
    End Function
    Private Function ShowTagFiltration() As String
        Dim RetStr As String = Nothing
        If objTagFiltration.Visible Then Return RetStr
        objTagFiltration.BackColor = Me.BackColor
        objTagFiltration.StartPosition = FormStartPosition.CenterScreen
        objTagFiltration.MaximizeBox = False
        '        objTagFiltration.grpTagFiltration.BackgroundColor = grpHeader.BackgroundColor
        If objTagFiltration.ShowDialog() = Windows.Forms.DialogResult.OK Then
            RetStr = " AND GRSWT BETWEEN " & Val(objTagFiltration.txtWeightFrom.Text) & " AND " & Val(objTagFiltration.txtWeightTo.Text) & ""
        End If
        objTagFiltration.txtWeightFrom.Clear()
        objTagFiltration.txtWeightTo.Clear()
        Return RetStr
    End Function
    Private Sub ShowToolTip(ByVal text As String, ByVal ctrl As Control, Optional ByVal tipIco As ToolTipIcon = ToolTipIcon.Info)
        lblHelpText.Text = text
    End Sub
    Private Sub HideToolTip(ByVal ctrl As Control)
        lblHelpText.Text = ""
        ToolTip1.Hide(ctrl)
    End Sub


    Private Sub ClearDtGrid(ByVal dt As DataTable)
        dt.Columns("KeyNo").AutoIncrementSeed = 1
        dt.Rows.Clear()
        For i As Integer = 1 To 10
            dt.Rows.Add()
        Next
        dt.AcceptChanges()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        fnew()
    End Sub
    Function fnew()
        dtGridTAG.Clear()
        dtGridTAGSTONE.Clear()

        ClearTagDet()
        ClearTagStoneDet()
        ClearItemDetails()

        CalcGridtagTotal()
        CalcGridtagStoneTotal()
        OldTagGrswt = 0
        OldTagNetwt = 0
        OldTagAmount = 0

        OldTagStoneWt = 0
        OldTagStoneAmt = 0

        rbtGenerateNewTag.Enabled = True
        rbtGenerateNewTag.Checked = True

        txtTAGRowIndex.Text = ""
        txtStRowIndex.Text = ""
        txtTagItemId.Focus()
        'ClearDtGrid(dtTagDetail)
        'ClearDtGrid(dtStoneDetails)
    End Function


    Private Sub ClearTagDet()
        txtTagItemId.Clear()
        txtTagTagNo.Clear()
        txtTagPcs_NUM.Clear()
        txtTagOldGrsWt_WET.Clear()
        txtTagGrsWt_WET.Clear()
        txtTagOldNetWt_WET.Clear()
        txtTagNetWt_WET.Clear()
        txttAGStoneAmount_AMT.Clear()
        txttAGStoneWt_WET.Clear()
        txttAGDiaAmount_AMT.Clear()
        txttAGDiaWt_WET.Clear()
        txtTagRate_AMT.Clear()
        txtTagOldAmount_AMT.Clear()
        txtTagAmount_AMT.Clear()
    End Sub
    Private Sub ClearTagStoneDet()
        txtStItem.Clear()
        txtStTagno.Clear()
        txtStPcs_NUM.Clear()
        txtStOldWeight_WET.Clear()
        txtStWeight_WET.Clear()
        txtStRate_Amt.Clear()
        txtStOldAmount_Amt.Clear()
        txtStAmount_Amt.Clear()
    End Sub
    Private Sub ClearItemDetails()
        dtTagDetail.Clear()
        dtStoneDetails.Clear()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub gridTAG_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridTAG.KeyDown
        If e.KeyCode = Keys.Enter Then
            If rbtInterchange.Checked = False Then
                rbtInterchange.Checked = True
                rbtGenerateNewTag.Checked = False
                rbtGenerateNewTag.Enabled = False
            End If
            e.Handled = True
            gridTAG.CurrentCell = gridTAG.Rows(gridTAG.CurrentRow.Index).Cells("TAGNO")
            gridTAG.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.White
            gridTAG.CurrentRow.DefaultCellStyle.SelectionForeColor = Color.Black
            Dim rwIndex As Integer = gridTAG.CurrentRow.Index
            With gridTAG.Rows(rwIndex)
                txtTagItemId.Text = .Cells("ITEMID").FormattedValue
                txtTagTagNo.Text = .Cells("TAGNO").FormattedValue
                txtTagPcs_NUM.Text = .Cells("PCS").FormattedValue
                txtTagOldGrsWt_WET.Text = .Cells("OLDGRSWT").FormattedValue
                txtTagGrsWt_WET.Text = .Cells("GRSWT").FormattedValue
                txtTagOldNetWt_WET.Text = .Cells("OLDNETWT").FormattedValue
                txtTagNetWt_WET.Text = .Cells("NETWT").FormattedValue
                txtTagRate_AMT.Text = .Cells("RATE").FormattedValue
                txttAGStoneWt_WET.Text = .Cells("STNWT").FormattedValue
                txttAGStoneAmount_AMT.Text = .Cells("STNAMT").FormattedValue
                txttAGDiaWt_WET.Text = .Cells("DIAWT").FormattedValue
                txttAGDiaAmount_AMT.Text = .Cells("DIAAMT").FormattedValue
                txtTagOldAmount_AMT.Text = .Cells("OLDAMOUNT").FormattedValue
                txtTagAmount_AMT.Text = .Cells("AMOUNT").FormattedValue
                taggrstwt = Val(.Cells("GRSWT").FormattedValue.ToString)
                CalcGridtagTotal()
                txtTAGRowIndex.Text = rwIndex
                txtTagGrsWt_WET.Focus()
                tagSno = .Cells("SNO").FormattedValue
            End With
        ElseIf e.KeyCode = Keys.Escape Then
            If gridTagStone.Rows.Count > 0 Then
                gridTagStone.Focus()
            End If
        End If
    End Sub
    Private Sub txtTagGrsWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagGrsWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            CalcSalAmount()
            txtTagNetWt_WET.Focus()
        End If
    End Sub

    Private Sub txtTagNetWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNetWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            CalcSalAmount()
            txtTagAmount_AMT.Focus()
        End If
    End Sub

    Private Sub txtTagAmount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If rbtInterchange.Checked = True Then
                dtGridTAG.AcceptChanges()
                With dtGridTAG.Rows(Val(txtTAGRowIndex.Text))
                    .Item("ITEMID") = Val(txtTagItemId.Text)
                    .Item("TAGNO") = txtTagTagNo.Text
                    .Item("PCS") = Val(txtTagPcs_NUM.Text)
                    .Item("GRSWT") = Val(txtTagGrsWt_WET.Text)
                    .Item("NETWT") = Val(txtTagNetWt_WET.Text)
                    .Item("RATE") = Val(txtTagRate_AMT.Text)
                    .Item("STNAMT") = IIf(Val(txttAGStoneAmount_AMT.Text) > 0, txttAGStoneAmount_AMT.Text, DBNull.Value)
                    .Item("STNWT") = IIf(Val(txttAGStoneWt_WET.Text) > 0, txttAGStoneWt_WET.Text, DBNull.Value)
                    .Item("DIAAMT") = IIf(Val(txttAGDiaAmount_AMT.Text) > 0, txttAGDiaAmount_AMT.Text, DBNull.Value)
                    .Item("DIAWT") = IIf(Val(txttAGDiaWt_WET.Text) > 0, txttAGDiaWt_WET.Text, DBNull.Value)
                    .Item("AMOUNT") = IIf(Val(txtTagAmount_AMT.Text) > 0, txtTagAmount_AMT.Text, DBNull.Value)
                End With
                CalcGridtagTotal()
                ClearTagDet()
                gridTAG.Focus()
            ElseIf rbtGenerateNewTag.Checked = True Then
                If gridTagStone.Rows.Count > 0 Then
                    txtStItem.ReadOnly = False
                    txtStItem.Focus()
                Else
                    btnSave.Focus()
                End If
            End If
        End If
    End Sub
    Private Sub gridTagStone_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridTagStone.KeyDown
        If e.KeyCode = Keys.Enter Then
            If rbtInterchange.Checked = True Then
                e.Handled = True
                gridTagStone.CurrentCell = gridTagStone.Rows(gridTagStone.CurrentRow.Index).Cells("TAGNO")
                gridTagStone.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.White
                gridTagStone.CurrentRow.DefaultCellStyle.SelectionForeColor = Color.Black
                Dim rwIndex As Integer = gridTagStone.CurrentRow.Index
                With gridTagStone.Rows(rwIndex)
                    txtStItem.Text = .Cells("ITEMID").FormattedValue
                    txtStTagno.Text = .Cells("TAGNO").FormattedValue
                    txtStPcs_NUM.Text = .Cells("PCS").FormattedValue
                    txtStRate_Amt.Text = .Cells("RATE").FormattedValue
                    txtStOldWeight_WET.Text = .Cells("OLDSTNWT").FormattedValue
                    txtStWeight_WET.Text = .Cells("STNWT").FormattedValue
                    txtStOldAmount_Amt.Text = .Cells("OLDSTNAMT").FormattedValue
                    txtStAmount_Amt.Text = .Cells("STNAMT").FormattedValue
                    CalcGridtagStoneTotal()
                    txtStRowIndex.Text = rwIndex
                    txtStTagno.Focus()
                    tagStoneSno = .Cells("SNO").FormattedValue
                End With
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        End If
    End Sub
    Private Sub txtStTagno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStTagno.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtStWeight_WET.Focus()
        End If
    End Sub

    Private Sub txtStWeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStWeight_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtStAmount_Amt.Focus()
        End If
    End Sub

    Private Sub txtStAmount_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStAmount_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If rbtInterchange.Checked = True Then
                dtGridTAGSTONE.AcceptChanges()
                With dtGridTAGSTONE.Rows(Val(txtStRowIndex.Text))
                    .Item("ITEMID") = Val(txtStItem.Text)
                    .Item("TAGNO") = txtStTagno.Text
                    .Item("PCS") = Val(txtStPcs_NUM.Text)
                    .Item("OLDSTNWT") = IIf(Val(txtStOldWeight_WET.Text) > 0, txtStOldWeight_WET.Text, DBNull.Value)
                    .Item("STNWT") = IIf(Val(txtStWeight_WET.Text) > 0, txtStWeight_WET.Text, DBNull.Value)
                    .Item("RATE") = Val(txtStRate_Amt.Text)
                    .Item("OLDSTNAMT") = IIf(Val(txtStOldAmount_Amt.Text) > 0, txtStOldAmount_Amt.Text, DBNull.Value)
                    .Item("STNAMT") = IIf(Val(txtStAmount_Amt.Text) > 0, txtStAmount_Amt.Text, DBNull.Value)
                End With
                CalcGridtagStoneTotal()
                ClearTagStoneDet()
                gridTagStone.Focus()
            ElseIf rbtGenerateNewTag.Checked = True Then
                btnSave.Focus()
            End If
        End If

    End Sub
    Function CalcSalAmount()
        If Val(txtTagGrsWt_WET.Text) <> 0 Then
            Dim grsnet As String = Nothing
            Dim MaxWast As Decimal = 0
            Dim MaxMC As Decimal = 0
            Dim dt As New DataTable
            strSql = " SELECT GRSNET,MAXWAST,MAXMC FROM " & cnAdminDb & "..ITEMTAG WHERE SNO='" & tagSno & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                grsnet = dt.Rows(0).Item("GRSNET").ToString
                MaxWast = dt.Rows(0).Item("MAXWAST").ToString
                MaxMC = dt.Rows(0).Item("MAXMC").ToString
            End If

            Dim amt As Double = 0
            Dim wt As Decimal = 0
            If grsnet = "N" Then
                wt = Val(txtTagNetWt_WET.Text)
            Else
                wt = Val(txtTagGrsWt_WET.Text)
            End If

            amt = ((wt + Val(MaxWast)) * Val(txtTagRate_AMT.Text)) _
            + Val(MaxMC) + Val(txttAGDiaAmount_AMT.Text) + Val(txttAGStoneAmount_AMT.Text)
            ' + Val(txtMultiAmt.Text) + Val(txtMiscAmt.Text)
            'amt += IIf(calType = "B", Val(txtRate_Amt.Text), 0)
            'amt += IIf(calType = "F", Val(txtRate_Amt.Text), 0)


            txtTagAmount_AMT.Text = Format(amt, "0.000")
        End If
    End Function
    Function CalcStoneAmount()
        If Val(txtStWeight_WET.Text) <> 0 Then
            Dim amount As Decimal = 0
            Dim salamount As Decimal = 0
            amount = Val(txtStAmount_Amt.Text)
            salamount = (amount / tagStoneWeight) * Val(txtStWeight_WET.Text)
            txtStAmount_Amt.Text = Format(salamount, "0.000")
        End If
    End Function
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If rbtGenerateNewTag.Checked = True Then
            If objItemDetail.cmbItem_MAN.Text <> "" Then
                TagSave()
            Else
                ShowItemDia()
            End If

        ElseIf rbtInterchange.Checked = True Then
            'If OldTagAmount = Val(gridTAGTotal.Rows(0).Cells("AMOUNT").Value.ToString) Then
            If Val(gridTAGTotal.Rows(0).Cells("OLDGRSWT").Value.ToString) = Val(gridTAGTotal.Rows(0).Cells("GRSWT").Value.ToString) Then
                TagInterChange()
            Else
                MsgBox("GRSWT total not tally with OLD GRSWT", MsgBoxStyle.Information)
                gridTAG.Focus()
                Exit Sub
            End If
            'Else
            '    MsgBox("AMOUNT total not tally with OLD AMOUNT", MsgBoxStyle.Information)
            '    gridTAG.Focus()
            '    Exit Sub
            'End If

        End If
    End Sub
    Function TagSave()

        Try

            strSql = vbCrLf + " SELECT * FROM " & cnAdminDb & "..ITEMTAG "
            strSql += vbCrLf + " WHERE TAGNO = '" & gridTAG.Rows(0).Cells("TAGNO").Value.ToString & "' AND SNO='" & gridTAG.Rows(0).Cells("SNO").Value.ToString & "'"
            strSql += vbCrLf + " AND ITEMID = " & Val(gridTAG.Rows(0).Cells("ITEMID").Value.ToString) & ""
            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            Dim dtTag As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTag)

            Dim TagSno As String = Nothing
            Dim COSTID As String = Nothing
            Dim TagNo As String = Nothing
            Dim ITEMID As Integer
            Dim SUBITEMID As Integer
            Dim DesignerId As Integer = Nothing
            Dim ITEMNAME As String
            Dim SUBITEMNAME As String
            Dim PCS As Integer
            Dim GRSWT As Decimal
            Dim NETWT As Decimal
            Dim LESSWT As Decimal
            Dim RATE As Double
            Dim AMOUNT As Double
            Dim TAGVAL As String
            Dim MaxWastage As Decimal = 0
            Dim MAXMcharge As Decimal = 0

            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & objItemDetail.cmbItem_MAN.Text & "'"
            ITEMID = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))

            If objItemDetail.cmbSubItem_Man.Enabled = True Then
                ''Find SubItemId
                strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & objItemDetail.cmbSubItem_Man.Text & "' AND ITEMID = " & ITEMID & ""
                SUBITEMID = Val(objGPack.GetSqlValue(strSql, "SUBITEMID", , tran))
            End If
            strSql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & objItemDetail.cmbDesigner_MAN.Text & "'"
            DesignerId = Val(objGPack.GetSqlValue(strSql, "DESIGNERID", "", tran))

            ITEMNAME = GetSqlValue(cn, "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=" & ITEMID & "")
            PCS = Val(gridTAGTotal.Rows(0).Cells("PCS").Value.ToString)
            GRSWT = Val(gridTAGTotal.Rows(0).Cells("GRSWT").Value.ToString)
            NETWT = Val(gridTAGTotal.Rows(0).Cells("NETWT").Value.ToString)
            RATE = Val(gridTAG.Rows(0).Cells("RATE").Value.ToString)
            AMOUNT = Val(gridTAGTotal.Rows(0).Cells("AMOUNT").Value.ToString)
            LESSWT = GRSWT - NETWT
            TagNo = objTag.GetTagNo(BillDate, "", LOTSNO, )
            If GRSWT <> 0 Then
                MaxWastage = ((Val(dtTag.Rows(0).Item("MAXWAST").ToString)) / (Val(dtTag.Rows(0).Item("GRSWT").ToString))) * GRSWT
                MAXMcharge = ((Val(dtTag.Rows(0).Item("MAXMC").ToString)) / (Val(dtTag.Rows(0).Item("GRSWT").ToString))) * GRSWT
            End If
            COSTID = dtTag.Rows(0).Item("TCOSTID").ToString

            TAGVAL = objTag.GetTagVal(TagNo) '(ITEMID & TagNo).ToString
            Dim sizeId As Integer = 0

            TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_ADMINSNO_TRAN")
            tran = Nothing
            tran = cn.BeginTransaction()
            ''INSERTING ITEMTAG
            strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAG"
            strSql += " ("
            strSql += " SNO,RECDATE,COSTID,ITEMID,ORDREPNO,ORSNO,ORDSALMANCODE,SUBITEMID,SIZEID,ITEMCTRID"
            strSql += " ,TABLECODE,DESIGNERID,TAGNO,PCS,GRSWT,LESSWT,NETWT,RATE,FINERATE,MAXWASTPER"
            strSql += " ,MAXMCGRM,MAXWAST,MAXMC,MINWASTPER,MINMCGRM,MINWAST,MINMC,TAGKEY,TAGVAL"
            strSql += " ,LOTSNO,COMPANYID,SALVALUE,PURITY,NARRATION,DESCRIP,REASON,ENTRYMODE,GRSNET,"
            strSql += " ISSDATE,ISSREFNO,ISSPCS,ISSWT,FROMFLAG,TOFLAG,APPROVAL,SALEMODE,BATCHNO,MARK,"
            strSql += " PCTFILE,OLDTAGNO,ITEMTYPEID,ACTUALRECDATE,WEIGHTUNIT,TRANSFERWT,CHKDATE,CHKTRAY"
            strSql += " ,CARRYFLAG,BRANDID,PRNFLAG,MCDISCPER,WASTDISCPER,RESDATE,TRANINVNO,SUPBILLNO,WORKDAYS,"
            strSql += " USERID,UPDATED,UPTIME,SYSTEMID,STYLENO,APPVER,TRANSFERDATE,BOARDRATE,RFID,TOUCH"
            strSql += " ,HM_BILLNO,HM_CENTER,ADD_VA_PER,REFVALUE,VALUEADDEDTYPE,TCOSTID,EXTRAWT,USRATE,INDRS"
            strSql += " )VALUES("

            strSql += " '" & TagSno & "'" 'SNO
            strSql += " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'RECDATE 
            strSql += " ,'" & cnCostId & "'" 'COSTID
            strSql += " ," & ITEMID & "" 'ITEMID
            strSql += " ,''" 'ORDREPNO
            strSql += " ,''" 'ORsno
            strSql += " ,''" 'ORDSALMANCODE
            strSql += " ," & SUBITEMID & "" 'SUBITEMID
            strSql += " ," & sizeId & "" 'SIZEID
            strSql += " ," & Val(dtTag.Rows(0).Item("ITEMCTRID").ToString) & "" 'ITEMCTRID
            strSql += " ,'" & dtTag.Rows(0).Item("TABLECODE").ToString & "'"
            strSql += " ," & DesignerId & "" 'DESIGNERID
            strSql += " ,'" & TagNo & "'" 'TAGNO
            strSql += " ," & PCS & "" 'PCS
            strSql += " ," & GRSWT & "" 'GRSWT
            strSql += " ," & LESSWT & "" 'LESSWT
            strSql += " ," & NETWT & "" 'NETWT
            strSql += " ," & 0 & "" 'RATE
            strSql += ",0" 'FINERATE
            strSql += " ," & Val(dtTag.Rows(0).Item("MAXWASTPER").ToString) & "" 'MAXWASTPER
            strSql += " ," & Val(dtTag.Rows(0).Item("MAXMCGRM").ToString) & "" 'MAXMCGRM
            strSql += " ," & MaxWastage & "" 'MAXWAST
            strSql += " ," & MAXMcharge & "" 'MAXMC
            strSql += " ," & Val(ObjMinValue.txtMinWastage_Per.Text) & "" 'MINWASTPER
            strSql += " ," & Val(ObjMinValue.txtMinMcPerGram_Amt.Text) & "" 'MINMCGRM
            strSql += " ," & Val(ObjMinValue.txtMinWastage_Wet.Text) & "" 'MINWAST
            strSql += " ," & Val(ObjMinValue.txtMinMkCharge_Amt.Text) & "" 'MINMC
            strSql += " ,'" & ITEMID.ToString & "" & TagNo & "'" 'TAGKEY
            strSql += " ," & TAGVAL & "" 'TAGVAL
            strSql += " ,'" & dtTag.Rows(0).Item("LOTSNO").ToString & "'" 'LOTSNO
            strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
            strSql += " ," & AMOUNT & "" 'SALVALUE
            strSql += " ," & Val(dtTag.Rows(0).Item("PURITY").ToString) & "" 'PURITY
            strSql += " ,'MERGED'" 'NARRATION
            strSql += " ,'" & ITEMNAME & "'"
            strSql += " ,''" 'REASON
            strSql += " ,'" & dtTag.Rows(0).Item("ENTRYMODE").ToString & "'" 'ENTRYMODE
            strSql += " ,'" & dtTag.Rows(0).Item("GRSNET").ToString & "'" 'GRSNET
            strSql += " ,NULL" 'ISSDATE
            strSql += " ,0" 'ISSREFNO
            strSql += " ,0" 'ISSPCS
            strSql += " ,0" 'ISSWT
            strSql += " ,''" 'FROMFLAG
            strSql += " ,''" 'TOFLAG
            strSql += " ,''" 'APPROVAL
            strSql += " ,'" & dtTag.Rows(0).Item("SALEMODE").ToString & "'" 'SALEMODE
            strSql += " ,''" 'BATCHNO
            strSql += " ,0" 'MARK
            strSql += " ,''" ' pctfile
            strSql += " ,''" 'OLDTAGNO
            strSql += " ," & Val(dtTag.Rows(0).Item("ITEMTYPEID").ToString) & "" 'ITEMTYPEID
            strSql += " ,'" & dtTag.Rows(0).Item("ACTUALRECDATE").ToString & "'" 'ACTUALRECDATE
            strSql += " ,''" 'WEIGHTUNIT
            strSql += " ,0" 'TRANSFERWT
            strSql += " ,NULL" 'CHKDATE
            strSql += " ,''" 'CHKTRAY
            strSql += " ,''" 'CARRYFLAG
            strSql += " ,''" 'BRANDID
            strSql += " ,''" 'PRNFLAG
            strSql += " ,0" 'MCDISCPER
            strSql += " ,0" 'WASTDISCPER
            strSql += " ,NULL" 'RESDATE
            strSql += " ,'" & dtTag.Rows(0).Item("TRANINVNO").ToString & "'" 'TRANINVNO
            strSql += " ,'" & dtTag.Rows(0).Item("SUPBILLNO").ToString & "'" 'SUPBILLNO
            strSql += " ,''" 'WORKDAYS
            strSql += " ," & userId & "" 'USERID
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ,'" & systemId & "'" 'SYSTEMID
            strSql += " ,'" & dtTag.Rows(0).Item("STYLENO").ToString & "'" 'STYLENO
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'TRANSFERDATE
            strSql += " ," & RATE & "" 'BOARDRATE
            strSql += " ,''"
            strSql += " ," & Val(dtTag.Rows(0).Item("TOUCH").ToString) & "" 'TOUCH
            strSql += " ,''" 'HM_BILLNO
            strSql += " ,''" 'HM_CENTER
            strSql += " ," & Val(dtTag.Rows(0).Item("ADD_VA_PER").ToString) & "" 'ADD_VA_PER
            strSql += " ," & Val(dtTag.Rows(0).Item("REFVALUE").ToString) & "" 'REFVALUE
            strSql += " ,'" & dtTag.Rows(0).Item("VALUEADDEDTYPE").ToString & "'"
            strSql += " ,'" & IIf(COSTID <> "", COSTID, cnCostId) & "'" 'TCOSTID
            strSql += " ,'" & Val(dtTag.Rows(0).Item("EXTRAWT").ToString) & "'" 'EXTRAWT
            strSql += " ," & 0 & ""
            strSql += " ," & 0 & ""
            strSql += " )"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()

            Dim Oldtagsno As String = ""
            For m As Integer = 0 To gridTAG.RowCount - 1
                If gridTAG.Rows(0).Cells("SNO").Value.ToString.Trim = "" Then Continue For
                Oldtagsno = Oldtagsno + gridTAG.Rows(0).Cells("SNO").Value.ToString() + ","
            Next

            If Oldtagsno <> "" Then Oldtagsno = Mid(Oldtagsno, 1, Len(Oldtagsno) - 1)
            strSql = vbCrLf + " SELECT * FROM " & cnAdminDb & "..PURITEMTAG "
            strSql += vbCrLf + " WHERE TAGSNO IN ('" & Oldtagsno.Replace(",", "','") & "')"
            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            Dim dtpurTag As New DataTable
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtpurTag)
            If dtpurTag.Rows.Count > 0 Then
                Dim PurWastper As Decimal = 0 'dtpurTag.Compute("AVG(PURWASTPER)", "")
                Dim PurWt As Decimal = dtpurTag.Compute("SUM(PURNETWT)", "")
                Dim PurLessWt As Decimal = dtpurTag.Compute("SUM(PURLESSWT)", "")
                Dim PurAmt As Decimal = dtpurTag.Compute("SUM(PURVALUE)", "")
                Dim Purtax As Decimal = dtpurTag.Compute("SUM(PURTAX)", "")
                Dim Purwast As Decimal = dtpurTag.Compute("SUM(PURWASTAGE)", "")
                Dim Purmc As Decimal = dtpurTag.Compute("SUM(PURMC)", "")
                If PurWastper <> 0 Then Purwast = (PurWt * PurWastper) / 100
                strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAG"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " TAGSNO,ITEMID,TAGNO,PURLESSWT,PURNETWT,PURRATE,PURGRSNET,PURWASTAGE"
                strSql += vbCrLf + " ,PURTOUCH,PURMC,PURVALUE,PURTAX,RECDATE,COMPANYID,COSTID"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " VALUES"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " '" & TagSno & "'" 'TAGSNO
                strSql += vbCrLf + " ," & ITEMID & "" 'ITEMID
                strSql += vbCrLf + " ,'" & TagNo & "'" 'TAGNO
                strSql += vbCrLf + " ," & PurLessWt & "" ' PURLESSWT
                strSql += vbCrLf + " ," & PurWt & "" ' PURNETWT"
                strSql += vbCrLf + " ," & Val(dtpurTag.Rows(0).Item("PURRATE").ToString) & "" ' PURRATE"
                strSql += vbCrLf + " ,'" & dtpurTag.Rows(0).Item("PURGRSNET").ToString & "'" ' PURGRSNET"
                strSql += vbCrLf + " ," & Purwast & "" ' PURWASTAGE"
                strSql += vbCrLf + " ," & Val(dtpurTag.Rows(0).Item("PURTOUCH").ToString) & "" ' PURTOUCH"
                strSql += vbCrLf + " ," & Purmc & "" ' PURMC"
                strSql += vbCrLf + " ," & PurAmt & "" ' PURVALUE"
                strSql += vbCrLf + " ," & Purtax & ""
                strSql += vbCrLf + " ,'" & BillDate.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                strSql += vbCrLf + " ,'" & dtpurTag.Rows(0).Item("COSTID").ToString & "'"
                strSql += vbCrLf + " )"
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()

            End If

            If gridTagStone.Rows.Count > 0 Then
                strSql = vbCrLf + " SELECT * FROM " & cnAdminDb & "..ITEMTAGSTONE "
                strSql += vbCrLf + " WHERE TAGNO = '" & gridTagStone.Rows(0).Cells("TAGNO").Value.ToString & "' AND SNO='" & gridTagStone.Rows(0).Cells("SNO").Value.ToString & "'"
                strSql += vbCrLf + " AND ITEMID = " & Val(gridTagStone.Rows(0).Cells("ITEMID").Value.ToString) & ""
                If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
                Dim dtTagStone As New DataTable
                cmd = New OleDbCommand(strSql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtTagStone)

                Dim STNPCS As Integer
                Dim STNWT As Decimal
                Dim STNRATE As Double
                Dim STNAMOUNT As Double

                STNPCS = Val(gridTagStonetotal.Rows(0).Cells("PCS").Value.ToString)
                STNWT = Val(gridTagStonetotal.Rows(0).Cells("STNWT").Value.ToString)
                STNRATE = Val(gridTagStone.Rows(0).Cells("RATE").Value.ToString)
                STNAMOUNT = Val(gridTagStonetotal.Rows(0).Cells("STNAMT").Value.ToString)
                If STNRATE = 0 Then
                    STNRATE = dtTagStone.Compute("MAX(STNRATE)", "")
                End If

                Dim _TagStnPcs As Integer
                Dim _TagStnwt As Decimal
                Dim _TagStnRate As Double
                Dim _TagStnAmt As Double
                Dim Sno As String = ""
                If gridTagStone.Rows.Count > 0 Then
                    For i As Integer = 0 To gridTagStone.Rows.Count - 1
                        Sno += "'" & gridTagStone.Rows(i).Cells("SNO").Value.ToString & "',"
                    Next
                    If Sno <> "" Then
                        Sno = Mid(Sno, 1, Len(Sno) - 1)
                    End If
                    strSql = "SELECT STNITEMID,STNSUBITEMID,SUM(STNPCS)STNPCS"
                    strSql += " ,SUM(STNWT)STNWT,STNRATE,SUM(STNAMT)STNAMT"
                    strSql += " FROM " & cnAdminDb & "..ITEMTAGSTONE"
                    strSql += " WHERE SNO IN(" & Sno & ")"
                    strSql += " GROUP BY STNITEMID,STNSUBITEMID,STNRATE"
                    Dim dt As New DataTable
                    cmd = New OleDbCommand(strSql, cn, tran)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Dim stnSno As String = GetNewSno(TranSnoType.ITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
                            ''Inserting itemTagStone
                            strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGSTONE("
                            strSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                            strSql += " STNSUBITEMID,TAGNO,STNPCS,STNWT,"
                            strSql += " STNRATE,STNAMT,DESCRIP,"
                            strSql += " RECDATE,CALCMODE,"
                            strSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
                            strSql += " OLDTAGNO,CARRYFLAG,COSTID,SYSTEMID,APPVER,"
                            strSql += " USRATE,INDRS"
                            strSql += " )VALUES("
                            strSql += " '" & stnSno & "'" ''SNO
                            strSql += " ,'" & TagSno & "'" 'TAGSNO
                            strSql += " ,'" & ITEMID & "'" 'ITEMID
                            strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                            strSql += " ," & Val(dt.Rows(j).Item("STNITEMID").ToString) & "" 'STNITEMID
                            strSql += " ," & Val(dt.Rows(j).Item("STNSUBITEMID").ToString) & "" 'STNSUBITEMID
                            strSql += " ,'" & TagNo & "'" 'TAGNO
                            strSql += " ," & Val(dt.Rows(j).Item("STNPCS").ToString) & "" 'STNPCS
                            strSql += " ," & Val(dt.Rows(j).Item("STNWT").ToString) & "" 'STNWT
                            strSql += " ," & Val(dt.Rows(j).Item("STNRATE").ToString) & "" 'STNRATE
                            strSql += " ," & Val(dt.Rows(j).Item("STNAMT").ToString) & "" 'STNAMT
                            strSql += " ,' '" 'DESCRIP
                            strSql += " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'RECDATE
                            strSql += " ,'" & dtTagStone.Rows(0).Item("CALCMODE").ToString & "'" 'CALCMODE
                            strSql += " ,0" 'MINRATE
                            strSql += " ,0" 'SIZECODE
                            strSql += " ,'" & dtTagStone.Rows(0).Item("STONEUNIT").ToString & "'" 'STONEUNIT
                            strSql += " ,NULL" 'ISSDATE
                            strSql += " ,''" 'OLDTAGNO
                            strSql += " ,''" 'CARRYFLAG
                            strSql += " ,'" & dtTagStone.Rows(0).Item("COSTID").ToString & "'" 'COSTID
                            strSql += " ,'" & systemId & "'" 'SYSTEMID
                            strSql += " ,'" & VERSION & "'" 'APPVER
                            strSql += " ," & Val(dtTagStone.Rows(0).Item("USRATE").ToString) & "" 'USRATE
                            strSql += " ," & Val(dtTagStone.Rows(0).Item("INDRS").ToString) & "" 'INDRS
                            strSql += " )"
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()

                            strSql = vbCrLf + " SELECT * FROM " & cnAdminDb & "..PURITEMTAGSTONE "
                            strSql += vbCrLf + " WHERE TAGSNO IN('" & Oldtagsno.Replace(",", "','") & "')"
                            strSql += vbCrLf + " AND ITEMID = " & Val(gridTagStone.Rows(0).Cells("ITEMID").Value.ToString) & ""
                            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
                            Dim dtpurTagStone As New DataTable
                            cmd = New OleDbCommand(strSql, cn, tran)
                            da = New OleDbDataAdapter(cmd)
                            da.Fill(dtpurTagStone)
                            If dtpurTagStone.Rows.Count > 0 Then
                                Dim purval As Decimal = dtpurTagStone.Compute("SUM(PURAMT)", "")
                                strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGSTONE"
                                strSql += vbCrLf + " ("
                                strSql += vbCrLf + " TAGSNO,ITEMID,TAGNO,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNRATE,STNAMT"
                                strSql += vbCrLf + " ,STONEUNIT,CALCMODE,PURRATE,PURAMT,COMPANYID,COSTID,STNSNO"
                                strSql += vbCrLf + " )"
                                strSql += vbCrLf + " VALUES"
                                strSql += vbCrLf + " ("
                                strSql += vbCrLf + " '" & TagSno & "'" 'TAGSNO
                                strSql += vbCrLf + " ," & ITEMID & "" 'ITEMID
                                strSql += vbCrLf + " ,'" & TagNo & "'" 'TAGNO
                                strSql += vbCrLf + " ," & Val(dt.Rows(j).Item("STNITEMID").ToString) & "" 'STNITEMID
                                strSql += vbCrLf + " ," & Val(dt.Rows(j).Item("STNSUBITEMID").ToString) & "" 'STNSUBITEMID
                                strSql += vbCrLf + " ," & Val(dt.Rows(j).Item("STNPCS").ToString) & "" 'STNPCS
                                strSql += vbCrLf + " ," & Val(dt.Rows(j).Item("STNWT").ToString) & "" 'STNWT
                                strSql += vbCrLf + " ," & Val(dt.Rows(j).Item("STNRATE").ToString) & "" 'STNRATE
                                strSql += vbCrLf + " ," & Val(dt.Rows(j).Item("STNAMT").ToString) & "" 'STNAMT
                                strSql += vbCrLf + " ,'" & dtTagStone.Rows(0).Item("STONEUNIT").ToString & "'" 'STONEUNIT
                                strSql += vbCrLf + " ,'" & dtTagStone.Rows(0).Item("CALCMODE").ToString & "'" 'CALCMODE
                                strSql += vbCrLf + " ," & Val(dtpurTagStone.Rows(0).Item("PURRATE").ToString) & "" 'PURRATE
                                strSql += vbCrLf + " ," & purval & "" 'PURAMT
                                strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                                strSql += vbCrLf + " ,'" & dtTagStone.Rows(0).Item("COSTID").ToString & "'"
                                strSql += vbCrLf + " ,'" & stnSno & "'"
                                strSql += vbCrLf + " )"
                                cmd = New OleDbCommand(strSql, cn, tran)
                                cmd.ExecuteNonQuery()
                            End If
                        Next
                    End If
                End If
            End If


            For k As Integer = 0 To gridTAG.Rows.Count - 1
                strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET ISSDATE='" & Format(BillDate, "yyyy-MM-dd") & "',TOFLAG='MI'"
                strSql += " WHERE SNO='" & gridTAG.Rows(k).Cells("SNO").Value.ToString & "'"
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            Next
            If gridTagStone.Rows.Count > 0 Then
                For j As Integer = 0 To gridTagStone.Rows.Count - 1
                    strSql = " UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET ISSDATE='" & Format(BillDate, "yyyy-MM-dd") & "'"
                    strSql += " WHERE SNO='" & gridTagStone.Rows(j).Cells("SNO").Value.ToString & "'"
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                Next
            End If
            fnew()
            tran.Commit()
            tran = Nothing
            MsgBox(TagNo + E0012, MsgBoxStyle.Exclamation)
            Dim prnmemsuffix As String = ""
            Dim objBar As New clsBarcodePrint
            Dim MetalId As String = ""
            strSql = " SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & objItemDetail.cmbItem_MAN.Text & "'"
            MetalId = GetSqlValue(cn, strSql).ToString
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            If chkBarcode.Checked = True Then
                If CallBarcodeExe = False Then
                    If MetalId = "G" Then
                        objBar.FuncprintBarcode_Single(ITEMID, TagNo)
                    Else
                        If Val(GetAdmindbSoftValue("BARCODE_FORMAT", "1")) <= "1" Then
                            objBar.FuncprintBarcode_Single(ITEMID, TagNo)
                        Else
                            FuncprintBarcode_Multi(ITEMID, TagNo, Val(GetAdmindbSoftValue("BARCODE_FORMAT", "1")), MetalId)
                        End If
                    End If
                Else
                    If GetAdmindbSoftValue("SING-TRANDB", "N") = "Y" Then
                        ''for kanaga laxmi
                        If objItemDetail.cmbSubItem_Man.Enabled = True Then 'DESCRIP
                            BarcodeDescrip = objItemDetail.cmbSubItem_Man.Text
                        Else
                            BarcodeDescrip = ITEMNAME
                        End If
                        BarcodeTagNo = TagNo
                        BarcodeSno = TagSno
                        FRM_PRINTDIA.ShowDialog()
                    Else
                        ''190609 modified
                        Dim memfile As String = "\Barcodeprint" & prnmemsuffix.Trim & ".mem"
                        Dim write As StreamWriter
                        write = IO.File.CreateText(Application.StartupPath & memfile)
                        write.WriteLine(LSet("PROC", 7) & ":" & ITEMID)
                        write.WriteLine(LSet("TAGNO", 7) & ":" & TagNo)
                        write.Flush()
                        write.Close()

                        If EXE_WITH_PARAM = False Then
                            If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                                System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe")
                            Else
                                MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                            End If
                        Else
                            Dim itDesc As String = LSet("PROC", 7) & ":" & ITEMID
                            Dim tagDesc As String = LSet("TAGNO", 7) & ":" & TagNo
                            If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                                System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe", itDesc & ";" & tagDesc)
                            Else
                                MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try

    End Function

    Function TagInterChange()
        Try
            tran = Nothing
            tran = cn.BeginTransaction()
            If gridTAG.Rows.Count > 0 Then
                For i As Integer = 0 To gridTAG.Rows.Count - 1
                    strSql = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET GRSWT= " & Val(gridTAG.Rows(i).Cells("GRSWT").Value.ToString) & ""
                    strSql += vbCrLf + " ,NETWT=" & Val(gridTAG.Rows(i).Cells("NETWT").Value.ToString) & ",SALVALUE=" & Val(gridTAG.Rows(i).Cells("AMOUNT").Value.ToString) & ""
                    strSql += vbCrLf + " WHERE TAGNO = '" & gridTAG.Rows(i).Cells("TAGNO").Value.ToString & "' AND SNO='" & gridTAG.Rows(i).Cells("SNO").Value.ToString & "'"
                    strSql += vbCrLf + " AND ITEMID = " & Val(gridTAG.Rows(i).Cells("ITEMID").Value.ToString) & ""
                    If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                Next
            End If

            If gridTagStone.Rows.Count > 0 Then
                For i As Integer = 0 To gridTagStone.Rows.Count - 1
                    strSql = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET STNWT= " & Val(gridTagStone.Rows(i).Cells("STNWT").Value.ToString) & ""
                    strSql += vbCrLf + " ,STNAMT=" & Val(gridTagStone.Rows(i).Cells("STNAMT").Value.ToString) & ",TAGNO=" & Val(gridTagStone.Rows(i).Cells("TAGNO").Value.ToString) & ""
                    strSql += vbCrLf + " WHERE TAGNO = '" & gridTagStone.Rows(i).Cells("TAGNO").Value.ToString & "' AND SNO='" & gridTagStone.Rows(i).Cells("SNO").Value.ToString & "'"
                    strSql += vbCrLf + " AND ITEMID = " & Val(gridTagStone.Rows(i).Cells("ITEMID").Value.ToString) & ""
                    If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                Next
            End If

            fnew()
            tran.Commit()
            tran = Nothing
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try

    End Function


    Private Sub grpTagDetail_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles grpTagDetail.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            gridTagStone.Focus()
        End If
    End Sub


    Private Sub rbtInterchange_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rbtInterchange.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            gridTAG.Focus()
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub ShowItemDia()
        If objItemDetail.Visible Then Exit Sub
        objItemDetail.BackColor = Me.BackColor
        objItemDetail.StartPosition = FormStartPosition.CenterScreen
        'objAddressDia.StartPosition = FormStartPosition.Manual
        'objAddressDia.Location = New Point(75, 181)
        objItemDetail.MaximizeBox = False
        If gridTAG.Rows.Count > 0 Then
            Dim dr As DataRow = Nothing
            strSql = " SELECT "
            strSql += "  ISNULL((SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =T.ITEMID),'') ITEMNAME "
            strSql += "  ,ISNULL((SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID =T.ITEMID AND SUBITEMID = T.SUBITEMID),'') SUBITEMNAME "
            strSql += "  ,ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID =T.DESIGNERID),'') DESIGNERNAME "
            strSql += " FROM " & cnAdminDb & "..ITEMTAG AS T WHERE T.ITEMID = '" & gridTAG.Rows(0).Cells("ITEMID").Value.ToString & "' "
            strSql += " AND T.TAGNO = '" & gridTAG.Rows(0).Cells("TAGNO").Value.ToString & "' AND ISSDATE IS NULL"
            dr = GetSqlRow(strSql, cn)
            If Not dr Is Nothing Then
                objItemDetail.ItemName = dr.Item("ITEMNAME").ToString
                objItemDetail.SubitemName = dr.Item("SUBITEMNAME").ToString
                objItemDetail.DesignerName = dr.Item("DESIGNERNAME").ToString
            End If
        End If
        If objItemDetail.ShowDialog() = Windows.Forms.DialogResult.OK Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub rbtGenerateNewTag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles rbtGenerateNewTag.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ShowItemDia()
        End If
    End Sub

    Private Sub txtTagItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTagItemId.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadItemName()
        ElseIf e.KeyCode = Keys.Down Then
            gridTAG.Select()
        End If
    End Sub

    Private Sub txtTagItemId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagItemId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim sp() As String = txtTagItemId.Text.Split(objSoftKeys.PRODTAGSEP)
            Dim SCANSTR As String = txtTagItemId.Text
            If objSoftKeys.PRODTAGSEP <> "" And txtTagItemId.Text <> "" Then
                sp = txtTagItemId.Text.Split(objSoftKeys.PRODTAGSEP)
                txtTagItemId.Text = Trim(sp(0))
                If sp.Length > 1 Then
                    If Len(SCANSTR) > Len(Trim(sp(0)) & objSoftKeys.PRODTAGSEP & Trim(sp(1))) Then SCANSTR = Trim(sp(0)) & objSoftKeys.PRODTAGSEP & Trim(sp(1))
                End If
CheckItem:
                Dim dtItemDet As New DataTable
                If txtTagItemId.Text = "" Then
                    LoadItemName()
                    Exit Sub
                ElseIf txtTagItemId.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtTagItemId.Text) & "'" & GetItemQryFilteration()) = False Then
                    strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = '" & Trim(txtTagItemId.Text) & "'"
                    dtItemDet = New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtItemDet)
                    If dtItemDet.Rows.Count > 0 Then
                        txtTagItemId.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    Else
                        LoadItemName()
                        Exit Sub
                    End If
                End If

            End If
            If sp.Length > 1 And objSoftKeys.PRODTAGSEP <> "" And txtTagItemId.Text <> "" Then
                txtTagTagNo.Text = Trim(sp(1))
            End If
            If txtTagTagNo.Text <> "" And txtTagTagNo.Text <> "B" And txtTagTagNo.Text <> "C" Then
                txtTagTagNo.Focus()
                'txt_TagNo_KeyPress(Me, New KeyPressEventArgs(e.KeyChar))
            End If
            txtTagTagNo.Focus()
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            rbtGenerateNewTag.Focus()
        End If

        'If rbtGenerateNewTag.Checked = True Then
        '    If txtTagItemId.Text <> "" Then
        '        Dim itemname As String
        '        itemname = GetSqlValue(cn, "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=" & Val(txtTagItemId.Text) & "")
        '        txtTagTagNo.Text = objTag.GetTagNo(BillDate, "", LOTSNO, )
        '        txtTagPcs_NUM.Text = gridTAG.Rows(0).Cells("PCS").Value.ToString
        '        txtTagGrsWt_WET.Text = gridTAGTotal.Rows(0).Cells("GRSWT").Value.ToString
        '        txtTagNetWt_WET.Text = gridTAGTotal.Rows(0).Cells("NETWT").Value.ToString
        '        txtTagRate_AMT.Text = gridTAG.Rows(0).Cells("RATE").Value.ToString
        '        txttAGStoneWt_WET.Text = gridTAGTotal.Rows(0).Cells("STNWT").Value.ToString
        '        txttAGStoneAmount_AMT.Text = gridTAGTotal.Rows(0).Cells("STNAMT").Value.ToString
        '        txttAGDiaWt_WET.Text = gridTAGTotal.Rows(0).Cells("DIAWT").Value.ToString
        '        txttAGDiaAmount_AMT.Text = gridTAGTotal.Rows(0).Cells("DIAAMT").Value.ToString
        '        txtTagAmount_AMT.Text = gridTAGTotal.Rows(0).Cells("AMOUNT").Value.ToString
        '        txtTagAmount_AMT.Focus()
        '    Else
        '        txtTagItemId.Focus()
        '    End If
        'End If
    End Sub

    Private Sub txtStItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStItem.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadStoneItemName()
        End If

    End Sub

    Private Sub txtStItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStItem.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If rbtGenerateNewTag.Checked = True Then
                If txtStItem.Text <> "" Then
                    txtStTagno.Text = txtTagTagNo.Text
                    txtStPcs_NUM.Text = gridTagStone.Rows(0).Cells("PCS").Value.ToString
                    txtStRate_Amt.Text = gridTagStone.Rows(0).Cells("RATE").Value.ToString
                    txtStWeight_WET.Text = gridTagStonetotal.Rows(0).Cells("STNWT").Value.ToString
                    txtStAmount_Amt.Text = gridTagStonetotal.Rows(0).Cells("STNAMT").Value.ToString
                    txtStAmount_Amt.Focus()
                Else
                    txtStItem.Focus()
                End If
            End If
        End If
    End Sub
    Private Sub LoadStoneItemName()
        strSql = " SELECT ITEMID AS ITEMID,ITEMNAME AS ITEMNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') = 'S' AND ACTIVE = 'Y'"
        Dim itemName As String = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, 1, , txtStItem.Text)
        If itemName <> "" Then
            txtStItem.Text = itemName
        Else
            txtStItem.Focus()
            txtStItem.SelectAll()
        End If
    End Sub

    Private Sub rbtGenerateNewTag_CheckedChanged(sender As Object, e As EventArgs) Handles rbtGenerateNewTag.CheckedChanged

    End Sub
End Class