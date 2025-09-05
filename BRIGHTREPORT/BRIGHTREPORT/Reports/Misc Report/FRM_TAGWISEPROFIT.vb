Imports System.Data.OleDb
Public Class FRM_TAGWISEPROFIT
    Dim objGridShower As frmGridDispDia
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim dtMetal As New DataTable
    Dim dtItem As New DataTable

    Private Sub FRM_TAGWISEPROFIT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub FRM_TAGWISEPROFIT_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Function funcLoadCategory() As Integer
        cmbCategory.Items.Clear()
        cmbCategory.Items.Add("ALL")
        StrSql = " select CatName from " & cnAdminDb & "..Category "
        If chkCmbMetal.Text <> "ALL" Then
            StrSql += " where metalid = (select metalid from " & cnAdminDb & "..metalmast where metalname = '" & chkCmbMetal.Text & "')"
        End If
        StrSql += "  order by CatName"
        objGPack.FillCombo(StrSql, cmbCategory, False)
        cmbCategory.Text = "ALL"
    End Function
    Function funcLoaddesigner() As Integer
        cmbDesigner.Items.Clear()
        cmbDesigner.Items.Add("ALL")
        StrSql = " SELECT DESIGNERNAME from " & cnAdminDb & "..DESIGNER "
        StrSql += "  ORDER BY DESIGNERNAME"
        objGPack.FillCombo(StrSql, cmbDesigner, False)
        cmbDesigner.Text = "ALL"
    End Function
    Function funcLoadItemName() As Integer
        StrSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        StrSql += " WHERE ACTIVE = 'Y'"
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            StrSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
        End If
        StrSql += " ORDER BY RESULT,ITEMNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
    End Function

    Private Function GetSelectedCatCode(ByVal chkLst As ComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Text <> "ALL" Then
            If WithQuotes Then retStr += "'"
            retStr = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME= '" & chkLst.Text.ToString & "'")
            If WithQuotes Then retStr += "'"
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim desigr As String = Nothing
        If rdbValAdded.Checked Then
            StrSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_TAGWISEPROFIT_VALADDED"
            StrSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " ,@TRANNO = " & Val(txtTranNo_NUM.Text) & ""
            StrSql += vbCrLf + " ,@PURRATE='" & IIf(ChkPurratebase.Checked = True, "Y", "N") & "'"
            StrSql += vbCrLf + " ,@METALID = '" & GetSelectedMetalid(chkCmbMetal, False) & "'"
            StrSql += vbCrLf + " ,@CATCODE ='" & GetSelectedCatCode(cmbCategory, False) & "'"
            StrSql += vbCrLf + " ,@ITEMIDS = '" & GetSelecteditemid(chkCmbItem, False) & "'"
            If cmbDesigner.Text = "ALL" Then
                StrSql += vbCrLf + " ,@DESIGNERIDS = ''"
            Else
                desigr = GetSqlValue(cn, "SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME ='" & cmbDesigner.Text & "'")
                StrSql += vbCrLf + " ,@DESIGNERIDS ='" & desigr & "' "
            End If
        Else
            StrSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_TAGWISEPROFIT"
            StrSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " ,@PURRATE='" & IIf(ChkPurratebase.Checked = True, "Y", "N") & "'"
            StrSql += vbCrLf + " ,@METALID = '" & GetSelectedMetalid(chkCmbMetal, False) & "'"
            StrSql += vbCrLf + " ,@CATCODE ='" & GetSelectedCatCode(cmbCategory, False) & "'"
            StrSql += vbCrLf + " ,@ITEMIDS = '" & GetSelecteditemid(chkCmbItem, False) & "'"
            If cmbDesigner.Text = "ALL" Then
                StrSql += vbCrLf + " ,@DESIGNERIDS = ''"
            Else
                desigr = GetSqlValue(cn, "SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME ='" & cmbDesigner.Text & "'")
                StrSql += vbCrLf + " ,@DESIGNERIDS ='" & desigr & "' "
            End If
        End If
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(StrSql, cn)
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "TAG WISE PROFIT"
        Dim tit As String = "TAG WISE PROFIT ANALYSIS" + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.Show()
        Dim ObjGrouper As New BrighttechPack.DataGridViewGrouper(objGridShower.gridView, dtGrid)
        If rbtTouch.Checked Then
            ObjGrouper.pColumns_Group.Add("TOUCH")
        Else
            ObjGrouper.pColumns_Group.Add("TRANTYPE")
            ObjGrouper.pColumns_Group.Add("DESIGNER")
        End If

        ObjGrouper.pColumns_Sum.Add("PCS")
        ObjGrouper.pColumns_Sum.Add("GRSWT")
        ObjGrouper.pColumns_Sum.Add("NETWT")
        ObjGrouper.pColumns_Sum.Add("DIAPCS")
        ObjGrouper.pColumns_Sum.Add("DIAWT")
        ObjGrouper.pColumns_Sum.Add("DIAAMT")
        ObjGrouper.pColumns_Sum.Add("WASTAGE")
        If rdbValAdded.Checked = False Then ObjGrouper.pColumns_Sum.Add("PURNETWT")

        ObjGrouper.pColumns_Sum.Add("MCHARGE")
        ObjGrouper.pColumns_Sum.Add("AMOUNT")
        ObjGrouper.pColumns_Sum.Add("PURVALUE_CALC")
        ObjGrouper.pColumns_Sum.Add("PURVALUE")
        ObjGrouper.pColumns_Sum.Add("DIFF_STKRATE")
        ObjGrouper.pColumns_Sum.Add("DIFF_ISSRATE")
        ObjGrouper.pColumns_Sum.Add("WASTAGEPROFIT")
        If rdbValAdded.Checked = False Then ObjGrouper.pColumns_Sum.Add("GRSNETWT")
        ObjGrouper.pColumns_Sum.Add("PURWASTAGE")
        ObjGrouper.pColName_Particular = "ITEMNAME"
        ObjGrouper.pColName_ReplaceWithParticular = "ITEMNAME"
        If rdbValAdded.Checked Then
            ObjGrouper.pColumns_Sort = "TRANDATE,TRANNO,ITEMNAME"
        End If

        'ObjGrouper.pColumns_Sum_FilterString = "STONE <> '1'"
        ObjGrouper.GroupDgv()
        If objGridShower.gridView.RowCount > 0 Then
            objGridShower.gridView.Rows.RemoveAt(objGridShower.gridView.Rows.Count - 1)
        End If
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("BATCHNO").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("DESIGNER").Visible = False
            .Columns("PURVALUE").Visible = False
            .Columns("TRANTYPE").Visible = False
            .Columns("SEP").Visible = False
            .Columns("DIFF_STKRATE").Visible = False
            .Columns("PURVALUE").HeaderText = "PURVALUE @STKRATE"
            .Columns("PURVALUE_CALC").HeaderText = "PURVALUE @ISSRATE"

            .Columns("DIFF_STKRATE").HeaderText = "PROFIT @STKRATE"
            .Columns("DIFF_ISSRATE").HeaderText = "PROFIT @ISSRATE"
            If .Columns.Contains("WASTAGEPROFIT") Then .Columns("WASTAGEPROFIT").HeaderText = "PROFIT @WASTAGE"
            If .Columns.Contains("WASTAGEPROFITPER") Then .Columns("WASTAGEPROFITPER").HeaderText = "PROFIT @WASTPER"
            If .Columns.Contains("WASTAGE") Then .Columns("WASTAGE").HeaderText = "SALWASTAGE"
            If rdbValAdded.Checked Then
                .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            End If
            '.Columns("BILLTYPE").HeaderText = "TYPE"
            '.Columns("BILLTYPE").Width = 40
            '.Columns("TRANNO").Width = 60
            '.Columns("TRANDATE").Width = 80
            '.Columns("TAGNO").Width = 70
            '.Columns("ITEMNAME").Width = 150
            '.Columns("PCS").Width = 60
            '.Columns("GRSWT").Width = 80
            '.Columns("NETWT").Width = 80
            '.Columns("GRSNET").Width = 40
            '.Columns("RATE").Width = 100
            '.Columns("TOUCH").Width = 60
            '.Columns("TAGVALUE").Width = 100
            '.Columns("AMOUNT").Width = 100
            '.Columns("DIFF").Width = 100
            '.Columns("DIFFPER").Width = 70
            '.Columns("DAYS").Width = 60
            '.Columns("SALESPERSON").Width = 120
            '.Columns("KEYNO").Visible = False
            '.Columns("COLHEAD").Visible = False
            '.Columns("RESULT").Visible = False
            'FormatGridColumns(dgv, False, False, , False)
            '.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        End With
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()

        StrSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
        StrSql += " ORDER BY RESULT,METALNAME"
        dtMetal = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")
        funcLoadCategory()
        funcLoaddesigner()
        funcLoadItemName()
        rdbValAdded.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub rdbValAdded_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbValAdded.CheckedChanged
        txtTranNo_NUM.Enabled = rdbValAdded.Checked
    End Sub

    Private Sub chkCmbMetal_TextChanged(sender As Object, e As EventArgs) Handles chkCmbMetal.TextChanged
        funcLoadItemName()
    End Sub

    Private Sub chkCmbMetal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkCmbMetal.SelectedIndexChanged

    End Sub
End Class