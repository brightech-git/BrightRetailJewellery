Imports System.Data.OleDb
Public Class frmOrderMerge
    Dim cmd As OleDbCommand
    Dim objItemDetail As LotMergeItemDet
    Public objSoftKeys As New EstimationSoftKeys
    Dim objTagFiltration As New frmTagFiltration
    Dim dtTagDetail As New DataTable("GridTag")
    Dim dtStoneDetails As New DataTable("GridStone")
    Dim dtGridOrder, dt As New DataTable
    Dim dtGridOutstand As New DataTable
    Dim ObjOrderTagInfo As New TagOrderInfo
    Dim RowTagMargin As DataRow = Nothing
    Public BillDate As Date = GetServerDate(tran)
    Dim _CheckOrderInfo As Boolean = IIf(GetAdmindbSoftValue("CHECKORDERINFO", "Y") = "Y", True, False)
    Dim REFNOPREFIXAUTO As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "REFNOPREFIXAUTO", "")
    Dim objTag As New TagGeneration
    Dim taggrstwt As Decimal = 0
    Dim tagStoneWeight As Decimal = 0
    Dim tagSno As String = Nothing
    Dim tagStoneSno As String = Nothing
    Dim LOTSNO As String
    Dim TagNoGen As String = Nothing
    Dim orderRow As DataRow = Nothing
    Dim TagNoFrom As String = Nothing
    Dim currentTagNo As String
    Dim lastTagNo As String
    Dim startTagNo As String
    Dim objBill As New RetailBill
    Dim objManualBill As frmManualBillNoGen
    Dim endTagNo As String
    Dim OldTagGrswt As Decimal = 0
    Dim OldTagNetwt As Decimal = 0
    Dim OldTagAmount As Double = 0
    Dim OldTagStoneWt As Decimal = 0
    Dim OldTagStoneAmt As Double = 0
    Dim ObjMinValue As New TagMinValues
    Dim SNO, OrderNo As String
    Dim TranNo As Integer
    Dim strSql As String
    Dim tabCheckBy As String = GetAdmindbSoftValue("LOTCHECKBY", "P")
    Dim Lotchkdate As Boolean = IIf(GetAdmindbSoftValue("LOTCHKDATE", "N") = "Y", True, False)
    Dim objdatescr As New frmDateInput
    Dim objAddressDia As New frmAddressDia(True, False)
    Dim orderDel As String

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        With dtGridOrder
            .Columns.Add("ORDERNO", GetType(String))
            .Columns.Add("ITEMID", GetType(Integer))
            .Columns.Add("ITEMNAME", GetType(String))
            .Columns.Add("SUBITEMID", GetType(Integer))
            .Columns.Add("SUBITEMNAME", GetType(String))
            .Columns.Add("TAGNO", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("RATE", GetType(Decimal))
            .Columns.Add("MC", GetType(Decimal))
            .Columns.Add("WAST", GetType(Decimal))
            .Columns.Add("SNO", GetType(String))
            .Columns.Add("REMDATE", GetType(Date))
            .Columns.Add("DUEDATE", GetType(Date))
            .Columns.Add("BATCHNO", GetType(String))
            .Columns.Add("ITEMTYPEID", GetType(Integer))
            .Columns.Add("ORVALUE", GetType(Decimal))
            .Columns.Add("WASTPER", GetType(Decimal))
            .Columns.Add("MCGRM", GetType(Decimal))
            .Columns.Add("DESCRIPT", GetType(String))
            .Columns.Add("EMPID", GetType(Integer))
            .Columns.Add("TAX", GetType(Decimal))
        End With
        dtGridOrder.AcceptChanges()
        gridTAG.DataSource = dtGridOrder
        gridTAG.ColumnHeadersVisible = False
        FormatGridColumns(gridTAG)
        StyleGridTag(gridTAG)

        Dim dtGridtagTotal As New DataTable
        dtGridtagTotal = dtGridOrder.Copy
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
        funcGridOustandingStyle()
        txtOrderNo.Focus()
    End Sub
    Private Sub funcGridOustandingStyle()
        With dtGridOutstand
            .Columns.Add("ORDERNO", GetType(String))
            .Columns.Add("ORDERDATE", GetType(Date))
            .Columns.Add("CUSTOMER", GetType(String))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("AMOUNT", GetType(Decimal))
            .Columns.Add("RATE", GetType(Decimal))
            .Columns.Add("VALUE", GetType(Decimal))
        End With
        dtGridOutstand.AcceptChanges()
        gridOutstanding.DataSource = dtGridOutstand
        gridOutstanding.ColumnHeadersVisible = False
        FormatGridColumns(gridOutstanding)
        GridstyleOutstanding(gridOutstanding)

        Dim dtGridOutTotal As New DataTable
        dtGridOutTotal = dtGridOutstand.Copy
        dtGridOutTotal.Rows.Clear()
        dtGridOutTotal.Rows.Add()
        gridOutTotal.ColumnHeadersVisible = False
        gridOutTotal.DataSource = dtGridOutTotal
        For Each col As DataGridViewColumn In gridOutstanding.Columns
            With gridOutstanding.Columns(col.Name)
                .Visible = col.Visible
                .Width = col.Width
                .DefaultCellStyle = col.DefaultCellStyle
            End With
        Next
        FormatGridColumns(gridOutTotal)
        GridstyleOutstanding(gridOutTotal)
        CalcOutstandingTotal()
    End Sub
    Private Sub GridstyleOutstanding(ByVal grid As DataGridView)
        With grid
            .Columns("ORDERNO").Width = lblOrderno.Width + 1
            .Columns("ORDERDATE").Width = lblOrderDate.Width + 1
            .Columns("CUSTOMER").Width = lblCustomer.Width + 1
            .Columns("GRSWT").Width = lblGrsWt.Width + 1
            .Columns("NETWT").Width = lblNetWt.Width + 1
            .Columns("AMOUNT").Width = lblAmount.Width + 1
            .Columns("RATE").Width = lblRate.Width + 1
            .Columns("VALUE").Width = lblValue.Width + 1
        End With
    End Sub
    Private Sub CalcOutstandingTotal()
        Dim amount As Integer = Nothing
        Dim value As Decimal = Nothing
        Dim grswt As Decimal = Nothing
        Dim netwt As Decimal = Nothing
        For i As Integer = 0 To gridOutstanding.RowCount - 1
            With gridOutstanding.Rows(i)
                amount += Val(.Cells("AMOUNT").Value.ToString)
                value += Val(.Cells("VALUE").Value.ToString)
                grswt += Val(.Cells("GRSWT").Value.ToString)
                netwt += Val(.Cells("NETWT").Value.ToString)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
                .DefaultCellStyle.SelectionBackColor = Color.White
            End With
        Next
        With gridOutTotal.Rows(0)
            .Cells("CUSTOMER").Value = "TOTAL"
            .Cells("GRSWT").Value = IIf(grswt <> 0, grswt, DBNull.Value)
            .Cells("NETWT").Value = IIf(netwt <> 0, netwt, DBNull.Value)
            .Cells("VALUE").Value = IIf(value <> 0, value, DBNull.Value)
            .Cells("AMOUNT").Value = IIf(amount <> 0, amount, DBNull.Value)
            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .DefaultCellStyle.ForeColor = Color.White
            .DefaultCellStyle.SelectionForeColor = Color.Blue
        End With
    End Sub
    Private Sub CalcGridtagTotal()
        Dim tagPcs As Integer = Nothing
        Dim tagGrsWt As Decimal = Nothing
        Dim tagNetWt As Decimal = Nothing
        Dim tagMc As Decimal = Nothing
        Dim tagWast As Decimal = Nothing
        Dim OrValue As Decimal = Nothing
        Dim tax As Decimal = Nothing
        For i As Integer = 0 To gridTAG.RowCount - 1
            With gridTAG.Rows(i)
                tagPcs += Val(.Cells("PCS").Value.ToString)
                tagGrsWt += Val(.Cells("GRSWT").Value.ToString)
                tagNetWt += Val(.Cells("NETWT").Value.ToString)
                tagWast += Val(.Cells("WAST").Value.ToString)
                tagMc += Val(.Cells("MC").Value.ToString)
                OrValue += Val(.Cells("ORVALUE").Value.ToString)
                tax += Val(.Cells("TAX").Value.ToString)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
                .DefaultCellStyle.SelectionBackColor = Color.AliceBlue
            End With
        Next
        grpTagDetail.BackColor = SystemColors.InactiveCaption

        With gridTAGTotal.Rows(0)
            .Cells("ITEMNAME").Value = "TOTAL"
            .Cells("PCS").Value = IIf(tagPcs <> 0, tagPcs, DBNull.Value)
            .Cells("GRSWT").Value = IIf(tagGrsWt <> 0, tagGrsWt, DBNull.Value)
            .Cells("NETWT").Value = IIf(tagNetWt <> 0, tagNetWt, DBNull.Value)
            .Cells("WAST").Value = IIf(tagWast <> 0, tagWast, DBNull.Value)
            .Cells("MC").Value = IIf(tagMc <> 0, tagMc, DBNull.Value)
            .Cells("ORVALUE").Value = IIf(OrValue <> 0, OrValue, DBNull.Value)
            .Cells("TAX").Value = IIf(tax <> 0, tax, DBNull.Value)
            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .DefaultCellStyle.ForeColor = Color.White
            .DefaultCellStyle.SelectionForeColor = Color.Blue
        End With
    End Sub
    Private Sub StyleGridTag(ByVal gridTagView As DataGridView)
        With gridTagView
            .Columns("ORDERNO").Width = txtOrderNo.Width + 1
            .Columns("ITEMID").Width = txtItemId.Width + 1
            .Columns("ITEMNAME").Width = txtItemname.Width + 1
            .Columns("SUBITEMID").Width = txtSubItemId.Width + 1
            .Columns("SUBITEMNAME").Width = txtSubItemName.Width + 1
            .Columns("TAGNO").Width = txtOtagno.Width + 1
            .Columns("PCS").Width = txtPcs.Width + 1
            .Columns("GRSWT").Width = txtTagGrsWt_WET.Width + 1
            .Columns("NETWT").Width = txtOnetwt.Width + 1
            .Columns("MC").Width = txtOMc.Width + 1
            .Columns("WAST").Width = txtOWast.Width + 1
            .Columns("RATE").Width = txtOrate.Width + 1
            For i As Integer = 12 To gridTagView.ColumnCount - 1
                .Columns(i).Visible = False
            Next
        End With
    End Sub
    Private Sub TagMerge_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtOrderNo.Focused Then Exit Sub
            If txtItemId.Focused Then Exit Sub
            If txtOtagno.Focused Then Exit Sub
            If txtTagGrsWt_WET.Focused Then Exit Sub
            If txtPcs.Focused Then Exit Sub
            If txtOrate.Focused Then Exit Sub
            If txtOnetwt.Focused Then Exit Sub
            If txtOMc.Focused Then Exit Sub
            If txtOWast.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub TagMerge_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridTAG.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridOutstanding.Font = New Font("VERDANA", 8, FontStyle.Bold)
        StyleGridTag(gridTAG)
        StyleGridTag(gridTAGTotal)
        GridstyleOutstanding(gridOutstanding)
        GridstyleOutstanding(gridOutTotal)
        txtOrderNo.Focus()
    End Sub

    Private Sub txtOrderNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOrderNo.GotFocus
        Main.ShowHelpText("Press Insert Key for Help and Escape to Save")
    End Sub

    Private Sub txtOrderNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOrderNo.KeyDown
        If e.KeyCode = Keys.Down Then
            gridTAG.Focus()
        ElseIf e.KeyCode = Keys.Escape Then
            objAddressDia.StartPosition = FormStartPosition.CenterScreen
            If objAddressDia.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
            btnSave.Focus()
        ElseIf e.KeyCode = Keys.Insert Then
            strSql = vbCrLf + " IF OBJECT_ID('TEMP" & systemId & "ORMAST','V') IS NOT NULL DROP VIEW TEMP" & systemId & "ORMAST"
            strSql += vbCrLf + " IF OBJECT_ID('TEMP" & systemId & "ORMAST','U') IS NOT NULL DROP TABLE TEMP" & systemId & "ORMAST"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " CREATE VIEW TEMP" & systemId & "ORMAST"
            strSql += vbCrLf + " AS"
            strSql += vbCrLf + "  SELECT O.ORNO"
            strSql += vbCrLf + "  ,O.ITEMID"
            strSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = O.ITEMID)AS ITEM"
            strSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = O.SUBITEMID AND ITEMID = O.ITEMID)AS SUBITEM"
            strSql += vbCrLf + "  ,(SELECT TOP 1 TAGNO FROM " & cnadmindb & "..ORIRDETAIL WHERE ORNO = O.ORNO AND ORSNO = O.SNO AND ISNULL(ORSTATUS,'') = 'R' AND ISNULL(CANCEL,'') = '')AS TAGNO"
            strSql += vbCrLf + "  ,O.PCS,O.GRSWT,O.NETWT,O.RATE,O.ORRATE,O.SNO"
            strSql += vbCrLf + "  ,O.BATCHNO"
            strSql += vbCrLf + "  FROM " & cnadmindb & "..ORMAST O"
            strSql += vbCrLf + " WHERE ISNULL(ODBATCHNO,'') = ''"
            strSql += vbCrLf + " AND ORTYPE = 'O' AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & cnCostId & "'"
            strSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(ORDCANCEL,'') = ''"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " SELECT (SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))AS NAME"
            strSql += vbCrLf + " ,SUBSTRING(ORNO,6,20)ORNO,ITEMID"
            strSql += vbCrLf + " ,ITEM,SUBITEM,TAGNO"
            strSql += vbCrLf + " ,PCS,GRSWT,NETWT,RATE,ORRATE,SNO"
            strSql += vbCrLf + " FROM TEMP" & systemId & "ORMAST AS O"
            orderRow = BrighttechPack.SearchDialog.Show_R("Select OrderItem", strSql, cn, , , , , , False)
            If Not orderRow Is Nothing Then
                txtOrderNo.Text = orderRow.Item("ORNO").ToString
            End If
        End If
    End Sub
    Private Sub txtLotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOrderNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtOrderNo.Text <> String.Empty Then
                For Each ro As DataRow In dtGridOrder.Rows
                    If ro.Item("ORDERNO").ToString = txtOrderNo.Text Then
                        MsgBox("This Order Already Loaded", MsgBoxStyle.Information)
                        txtOrderNo.Clear()
                        Exit Sub
                    End If
                Next
                Dim OrderNo As String = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & txtOrderNo.Text
                Dim dtorder As New DataTable
                strSql = vbCrLf + " SELECT "
                strSql += vbCrLf + " (SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = X.BATCHNO))AS NAME"
                strSql += vbCrLf + " ,SUBSTRING(ORNO,6,30)AS ORDERNO,ITEMID,SUBITEMID "
                strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.ITEMID)AS ITEMNAME"
                strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = X.ITEMID AND SUBITEMID = X.SUBITEMID)AS SUBITEMNAME"
                strSql += vbCrLf + " ,TAGNO,PCS,GRSWT,NETWT,RATE,ORRATE,SNO,MC,WAST,REMDATE,DUEDATE,BATCHNO,ITEMTYPEID,ORVALUE,WASTPER,MCGRM,DESCRIPT,EMPID,TAX FROM "
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " SELECT O.ORNO"
                strSql += vbCrLf + " ,CASE WHEN T.TAGNO <> '' THEN T.ITEMID ELSE O.ITEMID END ITEMID"
                strSql += vbCrLf + " ,CASE WHEN T.TAGNO <> '' THEN T.SUBITEMID ELSE O.SUBITEMID END SUBITEMID"
                strSql += vbCrLf + " ,T.TAGNO"
                strSql += vbCrLf + " ,CASE WHEN T.TAGNO <> '' THEN T.PCS ELSE O.PCS END PCS"
                strSql += vbCrLf + " ,CASE WHEN T.TAGNO <> '' THEN T.GRSWT ELSE O.GRSWT END GRSWT"
                strSql += vbCrLf + " ,CASE WHEN T.TAGNO <> '' THEN T.NETWT ELSE O.NETWT END NETWT"
                strSql += vbCrLf + " ,O.RATE,O.MC,O.WAST"
                strSql += vbCrLf + " ,O.ORRATE,O.SNO,O.BATCHNO,O.REMDATE,O.DUEDATE,O.ITEMTYPEID,O.ORVALUE,O.WASTPER,O.MCGRM,O.DESCRIPT,O.EMPID,O.TAX"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST O "
                strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.ORSNO = O.SNO"
                strSql += vbCrLf + " WHERE ISNULL(ODBATCHNO,'') = ''  AND ISNULL(O.CANCEL,'') = ''"
                strSql += vbCrLf + " AND ORTYPE = 'O'"
                strSql += vbCrLf + " AND ISNULL(O.COSTID,'') = '" & cnCostId & "'"
                strSql += vbCrLf + " AND ISNULL(O.COMPANYID,'') = '" & GetCompanyId(strCompanyId) & "'"
                strSql += vbCrLf + " AND ORNO = '" & OrderNo & "'"
                strSql += vbCrLf + " AND ISNULL(ORDCANCEL,'') = ''"
                strSql += vbCrLf + "  )X"

                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtorder)
                If Not dtorder.Rows.Count > 0 Then
                    MsgBox("OrderNo Not Found", , "BrighttechGold")
                    txtOrderNo.Clear()
                    Exit Sub
                Else
                    LoadLotDetails(dtorder)
                    strSql = "SELECT AC.ACNAME AS CUSTOMER,TRANDATE AS ORDERDATE,SUBSTRING(RUNNO,6,30) AS ORDERNO,GRSWT,NETWT,AMOUNT,RATE,VALUE "
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE =O.ACCODE "
                    strSql += vbCrLf + " WHERE RUNNO ='" & OrderNo & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    dt = New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        LoadOutstandingDetails(dt)
                    End If
                End If
                txtOrderNo.Clear()
            End If
        End If
    End Sub
    Private Sub LoadOutstandingDetails(ByVal dtout As DataTable)
        For K As Integer = 0 To dtout.Rows.Count - 1
            Dim row As DataRow
            row = dtGridOutstand.NewRow
            row("ORDERNO") = dtout.Rows(K).Item("ORDERNO").ToString
            row("ORDERDATE") = dtout.Rows(K).Item("ORDERDATE").ToString
            row("CUSTOMER") = dtout.Rows(K).Item("CUSTOMER").ToString
            row("RATE") = dtout.Rows(K).Item("RATE").ToString
            row("GRSWT") = dtout.Rows(K).Item("GRSWT").ToString
            row("NETWT") = dtout.Rows(K).Item("NETWT").ToString
            row("VALUE") = dtout.Rows(K).Item("VALUE").ToString
            row("AMOUNT") = dtout.Rows(K).Item("AMOUNT").ToString
            dtGridOutstand.Rows.Add(row)
        Next
        gridOutstanding.DataSource = dtGridOutstand
        CalcOutstandingTotal()
        GridstyleOutstanding(gridOutstanding)
    End Sub
    Private Sub LoadLotDetails(ByVal dtl As DataTable)
        For ii As Integer = 0 To dtl.Rows.Count - 1
            Dim row As DataRow
            row = dtGridOrder.NewRow
            row("SNO") = dtl.Rows(ii).Item("SNO").ToString
            row("ITEMNAME") = dtl.Rows(ii).Item("ITEMNAME").ToString
            row("ITEMID") = dtl.Rows(ii).Item("ITEMID").ToString
            row("SUBITEMID") = IIf(dtl.Rows(ii).Item("SUBITEMID").ToString = "", 0, dtl.Rows(ii).Item("SUBITEMID").ToString)
            row("SUBITEMNAME") = dtl.Rows(ii).Item("SUBITEMNAME").ToString
            row("PCS") = dtl.Rows(ii).Item("PCS").ToString
            row("GRSWT") = dtl.Rows(ii).Item("GRSWT").ToString
            row("NETWT") = dtl.Rows(ii).Item("NETWT").ToString
            row("MC") = dtl.Rows(ii).Item("MC").ToString
            row("WAST") = dtl.Rows(ii).Item("WAST").ToString
            row("ORDERNO") = dtl.Rows(ii).Item("ORDERNO").ToString
            row("TAGNO") = dtl.Rows(ii).Item("TAGNO").ToString
            row("RATE") = dtl.Rows(ii).Item("RATE").ToString
            row("REMDATE") = dtl.Rows(ii).Item("REMDATE").ToString
            row("DUEDATE") = dtl.Rows(ii).Item("DUEDATE").ToString
            row("BATCHNO") = dtl.Rows(ii).Item("BATCHNO").ToString
            row("ITEMTYPEID") = IIf(dtl.Rows(ii).Item("ITEMTYPEID").ToString = "", 0, dtl.Rows(ii).Item("ITEMTYPEID").ToString)
            row("ORVALUE") = dtl.Rows(ii).Item("ORVALUE").ToString
            row("WASTPER") = dtl.Rows(ii).Item("WASTPER").ToString
            row("MCGRM") = dtl.Rows(ii).Item("MCGRM").ToString
            row("DESCRIPT") = dtl.Rows(ii).Item("DESCRIPT").ToString
            row("EMPID") = dtl.Rows(ii).Item("EMPID").ToString
            row("TAX") = dtl.Rows(ii).Item("TAX").ToString
            dtGridOrder.Rows.Add(row)
        Next
        gridTAG.DataSource = dtGridOrder
        CalcGridtagTotal()
        StyleGridTag(gridTAG)
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
        objTagFiltration.grpTagFiltration.BackgroundColor = grpTagDetail.BackgroundColor
        If objTagFiltration.ShowDialog() = Windows.Forms.DialogResult.OK Then
            RetStr = " AND GRSWT BETWEEN " & Val(objTagFiltration.txtWeightFrom.Text) & " AND " & Val(objTagFiltration.txtWeightTo.Text) & ""
        End If
        objTagFiltration.txtWeightFrom.Clear()
        objTagFiltration.txtWeightTo.Clear()
        Return RetStr
    End Function
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
        dtGridOrder.Clear()
        dtGridOutstand.Clear()
        CalcOutstandingTotal()
        ClearTagDet()
        ClearItemDetails()
        CalcGridtagTotal()
        txtTAGRowIndex.Text = ""
        txtOrderNo.Focus()
    End Function

    Private Sub ClearTagDet()
        txtOrderNo.Clear()
        txtItemId.Clear()
        txtItemname.Clear()
        txtSubItemId.Clear()
        txtSubItemName.Clear()
        txtOtagno.Clear()
        txtTagGrsWt_WET.Clear()
        txtPcs.Clear()
        txtOnetwt.Clear()
        txtOrate.Clear()
        txtOMc.Clear()
        txtOWast.Clear()
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
            gridTAG.CurrentCell = gridTAG.Rows(gridTAG.CurrentRow.Index).Cells("ORDERNO")
            gridTAG.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.White
            gridTAG.CurrentRow.DefaultCellStyle.SelectionForeColor = Color.Black
            Dim rwIndex As Integer = gridTAG.CurrentRow.Index
            With gridTAG.Rows(rwIndex)
                txtOrderNo.Text = .Cells("ORDERNO").FormattedValue
                txtItemId.Text = .Cells("ITEMID").FormattedValue
                txtItemname.Text = .Cells("ITEMNAME").FormattedValue
                txtSubItemId.Text = .Cells("SUBITEMID").FormattedValue
                txtSubItemName.Text = .Cells("SUBITEMNAME").FormattedValue
                txtOtagno.Text = .Cells("TAGNO").FormattedValue
                txtTagGrsWt_WET.Text = .Cells("GRSWT").FormattedValue
                txtOnetwt.Text = .Cells("NETWT").FormattedValue
                txtOMc.Text = .Cells("MC").FormattedValue
                txtOWast.Text = .Cells("WAST").FormattedValue
                txtPcs.Text = .Cells("PCS").FormattedValue
                txtOrate.Text = .Cells("RATE").FormattedValue
                CalcGridtagTotal()
                txtTAGRowIndex.Text = rwIndex
                txtPcs.Focus()
                tagSno = .Cells("SNO").FormattedValue
            End With
        ElseIf e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not dtGridOrder.Rows.Count > 0 Then
            MsgBox("No Record Added", MsgBoxStyle.Exclamation)
            txtOrderNo.Focus()
            Exit Sub
        End If
Address:
        If objAddressDia.txtAddressName.Text = "" Then
            MsgBox("Party Name Should Not Empty", MsgBoxStyle.Information)
            objAddressDia.StartPosition = FormStartPosition.CenterScreen
            If objAddressDia.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
        End If
        If objAddressDia.txtAddressName.Text = "" Then GoTo Address
        Try
            tran = Nothing
            tran = cn.BeginTransaction
GETORDERNO:
            strSql = " SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'ORDERNO' AND COMPANYID = '" & strCompanyId & "'"
            TranNo = Val(objGPack.GetSqlValue(strSql, , , tran))

            strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TranNo + 1 & "' "
            strSql += " WHERE CTLID = 'ORDERNO' AND COMPANYID = '" & strCompanyId & "'"
            strSql += " AND CONVERT(INT,CTLTEXT) = " & TranNo & ""
            cmd = New OleDbCommand(strSql, cn, tran)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GETORDERNO
            End If
            strSql = " SELECT DISTINCT ORNO FROM " & cnadmindb & "..ORMAST"
            strSql += " WHERE ORNO = '" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "O" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & Val(TranNo + 1).ToString & "'"
            strSql += " AND COMPANYID = '" & strCompanyId & "'"
            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                GoTo GETORDERNO
            End If
            TranNo = TranNo + 1
            OrderNo = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & "O" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & TranNo
            Dim batchno As String = GetNewBatchno(cnCostId, BillDate, tran)
            Dim orSno As String = GetNewSno(TranSnoType.ORMASTCODE, tran, "GET_ADMINSNO_TRAN")

            strSql = " INSERT INTO " & cnadmindb & "..ORMAST" ''ORDER MERGE
            strSql += " ("
            strSql += " SNO,ORNO,ORDATE,REMDATE,DUEDATE,ORTYPE,COMPANYID,ORRATE"
            strSql += " ,ORMODE,ITEMID,SUBITEMID,DESCRIPT,PCS,GRSWT,NETWT"
            strSql += " ,SIZEID,RATE,NATURE,MCGRM,MC,WASTPER,WAST"
            strSql += " ,COMMPER,COMM,OTHERAMT,CANCEL,ORVALUE,COSTID,BATCHNO"
            strSql += " ,CORNO,PROPSMITH,PICTFILE,EMPID"
            strSql += " ,USERID,UPDATED,UPTIME,APPVER"
            strSql += " ,TAX,SC,ADSC,ITEMTYPEID,SIZENO,STYLENO,DISCOUNT"
            strSql += " )"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & orSno & "'" 'SNO
            strSql += " ,'" & OrderNo & "'" 'ORNO
            strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'ORDATE
            strSql += " ,'" & gridTAG.Rows(0).Cells("REMDATE").Value & "'" 'REMDATE
            strSql += " ,'" & gridTAG.Rows(0).Cells("DUEDATE").Value & "'" 'DUEDATE
            strSql += " ,'O'" 'ORTYPE
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " ,'C'"  'ORRATE
            strSql += " ,'C'" 'ORMODE
            strSql += " ," & gridTAG.Rows(0).Cells("ITEMID").Value  'ITEMID
            strSql += " ," & gridTAG.Rows(0).Cells("SUBITEMID").Value 'SUBITEMID
            strSql += " ,'" & gridTAG.Rows(0).Cells("DESCRIPT").Value & "'"  'DESCRIPT
            strSql += " ," & Val(IIf(IsDBNull(gridTAGTotal.Rows(0).Cells("PCS").Value), 0, gridTAGTotal.Rows(0).Cells("PCS").Value)) & "" 'PCS
            strSql += " ," & Val(IIf(IsDBNull(gridTAGTotal.Rows(0).Cells("GRSWT").Value), 0, gridTAGTotal.Rows(0).Cells("GRSWT").Value)) & "" 'GRSWT
            strSql += " ," & Val(IIf(IsDBNull(gridTAGTotal.Rows(0).Cells("NETWT").Value), 0, gridTAGTotal.Rows(0).Cells("NETWT").Value)) & "" 'NETWT
            strSql += " ,0"  'SIZEID
            strSql += " ," & Val(gridTAG.Rows(0).Cells("RATE").Value.ToString) & "" 'RATE
            strSql += " ,''" 'NATURE
            strSql += " ," & Val(gridTAG.Rows(0).Cells("MCGRM").Value) & "" 'MCGRM
            strSql += " ," & Val(IIf(IsDBNull(gridTAGTotal.Rows(0).Cells("MC").Value), 0, gridTAGTotal.Rows(0).Cells("MC").Value)) & "" 'MC
            strSql += " ," & Val(gridTAG.Rows(0).Cells("WASTPER").Value) & ""  'WASTPER
            strSql += " ," & Val(IIf(IsDBNull(gridTAGTotal.Rows(0).Cells("WAST").Value), 0, gridTAGTotal.Rows(0).Cells("WAST").Value)) & "" 'WAST
            strSql += " ,0" 'COMMPER
            strSql += " ,0"  'COMM
            strSql += " ,0" 'OTHERAMT
            strSql += " ,''" 'CANCEL
            strSql += " ," & Val(IIf(IsDBNull(gridTAGTotal.Rows(0).Cells("ORVALUE").Value), 0, gridTAGTotal.Rows(0).Cells("ORVALUE").Value)) & "" 'ORVALUE
            strSql += " ,'" & cnCostId & "'" 'COSTID
            strSql += " ,'" & batchno & "'" 'BATCHNO
            strSql += " ,0" 'CORNO
            strSql += " ,''" 'PROPSMITH
            strSql += " ,''" 'PICTFILE
            strSql += " ," & gridTAG.Rows(0).Cells("EMPID").Value 'eMPID
            strSql += " ," & userId & ""
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,'" & Date.Now.ToLongTimeString & "'"
            strSql += " ,'" & VERSION & "'"
            strSql += " ," & Val(IIf(IsDBNull(gridTAGTotal.Rows(0).Cells("TAX").Value), 0, gridTAGTotal.Rows(0).Cells("TAX").Value)) & "" 'TAX
            strSql += " ,0" 'SC
            strSql += " ,0" 'ADSC
            strSql += " ," & Val(gridTAG.Rows(0).Cells("ITEMTYPEID").Value) & "" 'ITEMTYPEID
            strSql += " ,''" 'SIZENO
            strSql += " ,''" 'STYLENO
            strSql += " ,0"  'DISCOUNT
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            objAddressDia.InsertIntoPersonalInfo(BillDate, cnCostId, batchno, tran)
            Dim OrderNum As String = Nothing
            For cnt As Integer = 0 To dtGridOrder.Rows.Count - 1 ''UPDATE ORMAST
                With dtGridOrder.Rows(cnt)
                    strSql = "UPDATE " & cnadmindb & "..ORMAST SET"
                    strSql += vbCrLf + " ODBATCHNO='" & batchno & "' ,ODSNO='" & batchno & "',REASON='MERGED' "
                    strSql += vbCrLf + " WHERE SNO='" & .Item("SNO").ToString & "' AND ISNULL(ODBATCHNO,'') = '' "
                    strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    'cmd = New OleDbCommand(strSql, cn, tran)
                    'cmd.ExecuteNonQuery()
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    strSql = " INSERT INTO " & cnadmindb & "..ORIRDETAIL" ''INSERT ORIDETAIL STATUS
                    strSql += " ("
                    strSql += " SNO,ORSNO,TRANNO,TRANDATE,DESIGNERID,PCS,GRSWT,NETWT,WASTAGE,MC,TAGNO,ORSTATUS,CANCEL,COSTID,DESCRIPT,ORNO"
                    strSql += " ,BATCHNO,USERID,UPDATED,UPTIME,APPVER,COMPANYID,ENTRYORDER,ORDSTATE_ID,CATCODE"
                    strSql += " )"
                    strSql += " VALUES"
                    strSql += " ("
                    strSql += " '" & GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                    strSql += " ,'" & .Item("SNO").ToString & "'" 'ORSNO
                    strSql += " ," & TranNo & "" 'TRANNO
                    strSql += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'" 'TRANDATE
                    strSql += " ,0" 'DESIGNERID
                    strSql += " ," & Val(.Item("PCS").ToString) & "" 'PCS
                    strSql += " ," & Val(.Item("GRSWT").ToString) & "" 'GRSWT
                    strSql += " ," & Val(.Item("NETWT").ToString) & "" 'NETWT
                    strSql += " ,0" 'WASTAGE
                    strSql += " ,0" 'MC
                    strSql += " ,''" 'TAGNO
                    strSql += " ,'I'"  'ORSTATUS
                    strSql += " ,''" 'CANCEL
                    strSql += " ,'" & cnCostId & "'" 'COSTID
                    strSql += " ,''"  'DESCRIPT
                    strSql += " ,'" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & .Item("ORDERNO").ToString & "'" 'ORNO
                    strSql += " ,'" & batchno & "'" 'BATCHNO
                    strSql += " ," & userId & "" 'USERID
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & GetCompanyId(strCompanyId) & "'" 'COMPANYID
                    strSql += " ," & objGPack.GetSqlValue("SELECT ISNULL(MAX(ENTRYORDER),0)+1 FROM " & cnadmindb & "..ORIRDETAIL", , , tran) & "" 'ENTRYORDER
                    strSql += " ,7" 'TRANSFERED
                    strSql += " ,'" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(.Item("ITEMID").ToString) & "", , , tran) & "'" 'CATCODE
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    OrderNum += "'" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & .Item("ORDERNO").ToString & "',"
                End With
            Next
            OrderNum = Mid(OrderNum, 1, Len(OrderNum) - 1)
            dt = New DataTable
            strSql = "SELECT * FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO IN (" & OrderNum & ")"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dt)
            Dim GrsWt, NetWt, Value, Rate, Purity As Decimal
            Dim Catcode As String = Nothing
            For j As Integer = 0 To dt.Rows.Count - 1
                With dt.Rows(j)
                    GrsWt += .Item("GRSWT").ToString
                    NetWt += .Item("NETWT").ToString
                    Value += .Item("VALUE").ToString
                    Rate = .Item("RATE").ToString
                    Purity = .Item("PURITY").ToString
                    Catcode = .Item("CATCODE").ToString
                    strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"  ''INSERT OUSTANDING FOR OLD ORDER NOS
                    strSql += " ("
                    strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
                    strSql += " ,AMOUNT,GRSWT,NETWT,PUREWT,RECPAY"
                    strSql += " ,REFNO,REFDATE,EMPID"
                    strSql += " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
                    strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
                    strSql += " ,RATE,ADVFIXWTPER,VALUE,CASHID,VATEXM,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,FROMFLAG,FLAG,PAYMODE)"
                    strSql += " VALUES"
                    strSql += " ("
                    strSql += " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
                    strSql += " ," & TranNo & "" 'TRANNO
                    strSql += " ,'" & BillDate & "'" 'TRANDATE
                    strSql += " ,'A'"  'TRANTYPE
                    strSql += " ,'" & .Item("RUNNO").ToString & "'" 'RUNNO
                    strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'AMOUNT
                    strSql += " ," & Val(.Item("GRSWT").ToString) & "" 'GRSWT
                    strSql += " ," & Val(.Item("NETWT").ToString) & "" 'NETWT
                    strSql += " ," & Val(.Item("PUREWT").ToString) & "" 'PUREWT
                    strSql += " ,'P'"  'RECPAY
                    strSql += " ,''"   'REFNO
                    strSql += " ,NULL" 'REFDATE
                    strSql += " ,0" 'EMPID
                    strSql += " ,''" 'TRANSTATUS
                    strSql += " ," & .Item("PURITY").ToString & "" 'PURITY
                    strSql += " ,'" & .Item("CATCODE").ToString & "'" 'CATCODE
                    strSql += " ,'" & batchno & "'" 'BATCHNO
                    strSql += " ," & userId & "" 'USERID

                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,0" 'RATE
                    strSql += " ,0"  'RATE
                    strSql += " ,0" 'VALUE
                    strSql += " ,0"  'CASHID
                    strSql += " ,'" & .Item("VATEXM").ToString & "'" 'VATEXM
                    strSql += " ,''" 'REMARK1
                    strSql += " ,''" 'REMARK1
                    strSql += " ,'" & .Item("ACCODE").ToString & "'" 'ACCODE
                    strSql += " ,0" 'CTRANCODE
                    strSql += " ,NULL" 'DUEDATE
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                    strSql += " ,'" & cnCostId & "'" 'COSTID
                    strSql += " ,'P'" 'FROMFLAG
                    strSql += " ,''" 'FLAG FOR ORDER ADVANCE REPAY
                    strSql += " ,'AA'" 'PAYMODE
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End With
            Next
            strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"          ''INSERT OUSTANDING FOR NEW ORDER NO
            strSql += " ("
            strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
            strSql += " ,AMOUNT,GRSWT,NETWT,PUREWT,RECPAY"
            strSql += " ,REFNO,REFDATE,EMPID"
            strSql += " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
            strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
            strSql += " ,RATE,ADVFIXWTPER,VALUE,CASHID,VATEXM,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,FROMFLAG,FLAG,PAYMODE)"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
            strSql += " ," & TranNo & "" 'TRANNO
            strSql += " ,'" & BillDate & "'" 'TRANDATE
            strSql += " ,'A'"  'TRANTYPE
            strSql += " ,'" & OrderNo & "'" 'RUNNO
            strSql += " ,0" 'AMOUNT
            strSql += " ," & GrsWt & "" 'GRSWT
            strSql += " ," & NetWt & "" 'NETWT
            strSql += " ,0" 'PUREWT
            strSql += " ,'R'"  'RECPAY
            strSql += " ,''"   'REFNO
            strSql += " ,NULL" 'REFDATE
            strSql += " ,0" 'EMPID
            strSql += " ,''" 'TRANSTATUS
            strSql += " ," & Purity & "" 'PURITY
            strSql += " ,'" & Catcode & "'" 'CATCODE
            strSql += " ,'" & batchno & "'" 'BATCHNO
            strSql += " ," & userId & "" 'USERID

            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ,'" & systemId & "'" 'SYSTEMID
            strSql += " ," & Rate & "" 'RATE
            strSql += " ,0"  'RATE
            strSql += " ," & Value & "" 'VALUE
            strSql += " ,0"  'CASHID
            strSql += " ,'Y'"  'VATEXM
            strSql += " ,''" 'REMARK1
            strSql += " ,''" 'REMARK1
            strSql += " ,'" & objAddressDia.txtAddressPartyCode.Text & "'" 'ACCODE
            strSql += " ,0" 'CTRANCODE
            strSql += " ,NULL" 'DUEDATE
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " ,'" & cnCostId & "'" 'COSTID
            strSql += " ,'D'" 'FROMFLAG
            strSql += " ,''" 'FLAG FOR ORDER ADVANCE REPAY
            strSql += " ,'OR'" 'PAYMODE
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            tran.Commit()
            tran = Nothing
            MsgBox(Mid(OrderNo, 6, 30).ToString + " Generated...", MsgBoxStyle.Exclamation)
            fnew()
            dtGridOrder.Rows.Clear()
        Catch ex As Exception
            MsgBox(ex.Message + ex.StackTrace)
            If Not tran Is Nothing Then tran.Rollback()
        End Try
    End Sub

    Private Sub txtOWast_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOWast.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            dtGridOrder.AcceptChanges()
            With dtGridOrder.Rows(Val(txtTAGRowIndex.Text))
                .Item("ORDERNO") = Val(txtOrderNo.Text)
                .Item("ITEMID") = Val(txtItemId.Text)
                .Item("ITEMNAME") = txtItemname.Text
                .Item("SUBITEMID") = IIf(Val(txtSubItemId.Text) > 0, Val(txtSubItemId.Text), DBNull.Value)
                .Item("SUBITEMNAME") = txtSubItemName.Text
                .Item("TAGNO") = txtOtagno.Text
                .Item("PCS") = IIf(Val(txtPcs.Text) > 0, txtPcs.Text, DBNull.Value)
                .Item("GRSWT") = IIf(Val(txtTagGrsWt_WET.Text) > 0, Val(txtTagGrsWt_WET.Text), DBNull.Value)
                .Item("NETWT") = IIf(Val(txtOnetwt.Text) > 0, txtOnetwt.Text, DBNull.Value)
                .Item("MC") = IIf(Val(txtOMc.Text) > 0, txtOMc.Text, DBNull.Value)
                .Item("WAST") = IIf(Val(txtOWast.Text) > 0, txtOWast.Text, DBNull.Value)
                .Item("RATE") = IIf(Val(txtOrate.Text) > 0, txtOrate.Text, DBNull.Value)
            End With
            CalcGridtagTotal()
            ClearTagDet()
            gridTAG.Focus()
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
            txtOnetwt.Focus()
        End If
    End Sub

    Private Sub txtOnetwt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOnetwt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtOrate.Focus()
        End If
    End Sub

    Private Sub gridTAG_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridTAG.UserDeletedRow
        dtGridOrder.AcceptChanges()
        CalcGridtagTotal()
        gridOutstanding_UserDeletedRow(Me, e)
        CalcOutstandingTotal()
        If gridTAG.Rows.Count = 0 Then txtOrderNo.Focus()
    End Sub
    Private Sub gridTAG_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles gridTAG.UserDeletingRow
        OrderDel = gridTAG.CurrentRow.Cells("ORDERNO").Value.ToString
    End Sub

    Private Sub btnNew_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        fnew()
    End Sub

    Private Sub btnSave_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnSave.KeyDown
        If e.KeyCode = Keys.Escape Then
            txtOrderNo.Focus()
        End If
    End Sub

    Private Sub txtOrate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOrate.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtOMc.Focus()
        End If
    End Sub

    Private Sub txtOMc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOMc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtOWast.Focus()
        End If
    End Sub

    Private Sub txtOrderNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOrderNo.LostFocus
        Main.ShowHelpText("")
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub gridOutstanding_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridOutstanding.UserDeletedRow
        For i As Integer = 0 To dtGridOutstand.Rows.Count - 1
            If dtGridOutstand.Rows(i).Item("ORDERNO").ToString = orderDel Then
                dtGridOutstand.Rows(i).Delete()
            End If
        Next
        dtGridOutstand.AcceptChanges()
        gridOutstanding.DataSource = Nothing
        gridOutstanding.DataSource = dtGridOutstand
    End Sub
End Class

