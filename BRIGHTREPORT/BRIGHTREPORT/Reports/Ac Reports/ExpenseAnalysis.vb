Imports System.Data.OleDb
Public Class ExpenseAnalysis
    Dim objGridShower As frmGridDispDia
    Dim StrSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim SelectedCompany As String
    Dim MultiCostcentre As Boolean = IIf(GetAdmindbSoftValue("CCWISE_FINRPT", "N", ) = "Y", True, False)

    Private Sub TagWiseProfitLoss_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub TagWiseProfitLoss_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadCompany(chkLstCompany)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            StrSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(StrSql, chkLstCostCentre)
            Dim dt As New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dt)
            For i As Integer = 0 To dt.Rows.Count - 1
                If cnCostName = dt.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, True)
                Else
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, False)
                End If
            Next
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If


        chklstgroup.Enabled = True
        chklstgroup.Items.Clear()
        StrSql = " SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE GRPTYPE = 'E' ORDER BY ACGRPNAME"
        FillCheckedListBox(StrSql, chklstgroup)


        grpContrainer.Location = New Point((ScreenWid - grpContrainer.Width) / 2, ((ScreenHit - 128) - grpContrainer.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate

        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Prop_Gets()
        dtpFrom.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre, False)
        Dim chkgroupName As String = GetChecked_CheckedList(chklstgroup, False)
        If MultiCostcentre = False Then
            StrSql = vbCrLf + " EXEC " & cnStockDb & "..SP_ExpenseAnalysis "
            StrSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " ,@COMPANYID = '" & IIf(SelectedCompany = "", "All", SelectedCompany) & "'"
            StrSql += vbCrLf + " ,@COSTNAME = '" & IIf(chkCostName = "", "ALL", chkCostName) & "'"
            StrSql += vbCrLf + " ,@GROUPNAME = '" & IIf(chkgroupName = "", "All", chkgroupName) & "'"
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(String))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(StrSql, cn)
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
            objGridShower.Text = "EXPENSE ANALYSIS"
            Dim tit As String = "EXPENSE ANALYSIS FROM " + dtpFrom.Text + " TO " + dtpTo.Text
            If chkCostName <> "" Then tit += vbNewLine + " COST CENTRE :" & Replace(chkCostName, "'", "")

            objGridShower.lblTitle.Text = tit
            objGridShower.StartPosition = FormStartPosition.CenterScreen
            objGridShower.dsGrid.DataSetName = objGridShower.Name
            objGridShower.dsGrid.Tables.Add(dtGrid)
            Dim Dggp As New BrighttechPack.DataGridViewGrouper(objGridShower.gridView, dtGrid)
            Dggp.pColName_Particular = "ACCODE"

            For cnt As Integer = 2 To dtGrid.Columns.Count - 1
                Dggp.pColumns_Sum.Add(dtGrid.Columns(cnt).ToString)
            Next
            Dggp.GroupDgv()
            'objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
            objGridShower.FormReSize = True
            objGridShower.FormReLocation = False
            objGridShower.pnlFooter.Visible = False
            objGridShower.gridViewHeader.Visible = True
            GridViewHeaderCreator(objGridShower.gridView, objGridShower.gridViewHeader)
            DataGridView_SummaryFormatting(objGridShower.gridView)
            objGridShower.formuser = userId
            objGridShower.Show()
            objGridShower.gridViewHeader.Visible = True
            AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
            gridView_ColumnWidthChanged(objGridShower.gridView, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("ACCODE")))
            objGridShower.gridView.Columns("KEYNO").Visible = False
            For cnt As Integer = 0 To objGridShower.gridView.Columns.Count - 1
                objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).DefaultCellStyle.BackColor = Color.LightBlue
                objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).DefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            Next
        Else
            Dim chkCostids As String = GetSelectedCostId(chkLstCostCentre, True)
            Dim chkgroupID As String = GetSelectedAcgrpId(chklstgroup, True)

            StrSql = " EXECUTE " & cnAdminDb & "..AGRN_SP_EXPENSEANALYSIS"
            StrSql += vbCrLf + "@COMPANYID = '" & IIf(SelectedCompany = "", "ALL", SelectedCompany) & "'"
            StrSql += vbCrLf + ",@FROMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + ",@TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + ",@COSTIDS  =""" & IIf(chkCostids = "", "ALL", chkCostids) & """"
            StrSql += vbCrLf + ",@ACGRP  =""" & IIf(chkgroupID = "", "ALL", chkgroupID) & """"
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(String))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dtGrid)
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            If Not dtGrid.Rows.Count > 1 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Sub
            End If

            objGridShower = New frmGridDispDia
            objGridShower.Name = Me.Name
            objGridShower.gridView.RowTemplate.Height = 21
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            objGridShower.Text = "EXPENSE ANALYSIS"
            Dim tit As String = "EXPENSE ANALYSIS FROM " + dtpFrom.Text + " TO " + dtpTo.Text
            If chkCostName <> "" Then tit += vbNewLine + " COST CENTRE :" & Replace(chkCostName, "'", "")

            objGridShower.lblTitle.Text = tit
            objGridShower.StartPosition = FormStartPosition.CenterScreen
            objGridShower.dsGrid.DataSetName = objGridShower.Name
            objGridShower.dsGrid.Tables.Add(dtGrid)
            Dim Dggp As New BrighttechPack.DataGridViewGrouper(objGridShower.gridView, dtGrid)
            Dggp.pColName_Particular = "ACCODE"

            'For cnt As Integer = 2 To dtGrid.Columns.Count - 1
            '    Dggp.pColumns_Sum.Add(dtGrid.Columns(cnt).ToString)
            'Next
            Dggp.GroupDgv()
            'objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
            objGridShower.FormReSize = True
            objGridShower.FormReLocation = False
            objGridShower.pnlFooter.Visible = False
            objGridShower.gridViewHeader.Visible = True
            'GridViewHeaderCreator(objGridShower.gridView, objGridShower.gridViewHeader)

            GridViewHeaderStyle(objGridShower.gridView, objGridShower.gridViewHeader, chkCostids)
            'DataGridView_SummaryFormatting(objGridShower.gridView)
            objGridShower.formuser = userId
            objGridShower.Show()
            objGridShower.gridViewHeader.Visible = True
            AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
            gridView_ColumnWidthChanged(objGridShower.gridView, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("ACCODE")))
            objGridShower.gridView.Columns("KEYNO").Visible = False
            objGridShower.gridView.Columns("ACNAME").Visible = False
            objGridShower.gridView.Columns("ACCODE").Visible = False
            objGridShower.gridView.Columns("RESULT").Visible = False
            For cnt As Integer = 0 To objGridShower.gridView.Columns.Count - 1
                objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).DefaultCellStyle.BackColor = Color.LightBlue
                objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).DefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            Next
        End If
        Prop_Sets()
    End Sub
    Private Sub GridViewHeaderStyle(ByVal gridView As DataGridView, ByVal gridheader3 As DataGridView, ByVal Costids As String)
        Dim dtcost As New DataTable("COST")
        Dim dttype As New DataTable("TYPE")
        Dim hro() As String
        With dtcost
            .Columns.Add("PARTICULAR", GetType(String))
            'dttype.Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OPEANING", GetType(String))
            'dttype.Columns.Add("OPEANING", GetType(String))
            Dim cdt As New DataTable
            StrSql = ""
            'If chkwithtotal.Checked Then

            'End If
            StrSql = "SELECT 'Total' COSTID,3 RESULT UNION ALL"
            StrSql += " SELECT 'AVG' COSTID,2 RESULT UNION ALL"
            StrSql += " SELECT DISTINCT COSTID,1 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            If costids <> "ALL" Then
                StrSql += " WHERE COSTID IN(" & costids & ")"
            End If
            StrSql += " ORDER BY RESULT,COSTID"
            cdt = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(cdt)
            'cdt = getData(StrSql)
            If cdt.Rows.Count > 0 Then
                costids = ""
                For i As Integer = 0 To cdt.Rows.Count - 1
                    costids += cdt.Rows(i).Item("COSTID").ToString
                    If i < cdt.Rows.Count - 1 Then costids += ","
                Next
            End If
            'End If
            If costids <> "ALL" Then
                costids = costids.Replace("'", "")
                hro = costids.Split(",")
            End If

            StrSql = "SELECT DISTINCT MNAME FROM TEMPTABLEDB..TMPRES1"
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dttype)
            'dttype = getData(StrSql)
            For i As Integer = 0 To hro.Length - 1
                Dim txt As String = ""
                For k As Integer = 0 To dttype.Rows.Count - 1
                    Dim type As String = ""
                    For j As Integer = 2 To gridView.Columns.Count - 1
                        If gridView.Columns(j).HeaderText.Contains(hro(i).ToString & "-" & dttype.Rows(k).Item(0).ToString & "-Value") Then
                            txt += gridView.Columns(j).HeaderText
                        End If
                        If gridView.Columns(j).HeaderText.Contains(hro(i).ToString & "-" & dttype.Rows(k).Item(0).ToString & "-%") Then
                            txt += gridView.Columns(j).HeaderText
                            If j <= gridView.Columns.Count Then txt += "~"
                        End If
                        If gridView.Columns(j).HeaderText.Contains(hro(i).ToString & "-Value") Then
                            txt += gridView.Columns(j).HeaderText
                        End If
                        If gridView.Columns(j).HeaderText.Contains(hro(i).ToString & "-%") Then
                            txt += gridView.Columns(j).HeaderText
                            If j <= gridView.Columns.Count Then txt += "~"
                        End If
                    Next
                Next

                If txt <> "" Then .Columns.Add(txt, GetType(String)) : .Columns(txt).Caption = IIf(hro(i).ToString = "Total", "Total", IIf(hro(i).ToString = "AVG", "AVG", objGPack.GetSqlValue("SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID IN('" & hro(i).ToString & "')").ToString))
            Next
            dtcost.Columns.Add("SCROLL", GetType(String))
            dttype.Columns.Add("SCROLL", GetType(String))
            dtcost.Columns("PARTICULAR").Caption = ""
            dtcost.Columns("OPEANING").Caption = ""
        End With

        With gridheader3
            gridheader3.DataSource = Nothing
            .DataSource = dtcost
            .Columns("PARTICULAR").HeaderText = "PARTICULAR"
            .Columns("OPEANING").HeaderText = "OPEANING"
            For i As Integer = 0 To hro.Length - 1
                Dim txt As String = ""
                Dim typwidth As Integer = 0
                For k As Integer = 0 To dttype.Rows.Count - 1
                    Dim type As String = ""
                    For j As Integer = 2 To gridView.Columns.Count - 1
                        If gridView.Columns(j).HeaderText.Contains(hro(i).ToString & "-" & dttype.Rows(k).Item(0).ToString & "-Value") Then
                            txt += gridView.Columns(j).HeaderText
                            If gridView.Columns(j).Visible = True Then typwidth += gridView.Columns(j).Width
                            'If j <= gridView.Columns.Count Then txt += "~"
                        End If
                        If gridView.Columns(j).HeaderText.Contains(hro(i).ToString & "-" & dttype.Rows(k).Item(0).ToString & "-%") Then
                            txt += gridView.Columns(j).HeaderText
                            If gridView.Columns(j).Visible = True Then typwidth += gridView.Columns(j).Width
                            If j <= gridView.Columns.Count Then txt += "~"
                        End If
                        If gridView.Columns(j).HeaderText.Contains(hro(i).ToString & "-Value") Then
                            txt += gridView.Columns(j).HeaderText
                            If gridView.Columns(j).Visible = True Then typwidth += gridView.Columns(j).Width
                            'If j <= gridView.Columns.Count Then txt += "~"
                        End If
                        If gridView.Columns(j).HeaderText.Contains(hro(i).ToString & "-%") Then
                            txt += gridView.Columns(j).HeaderText
                            If gridView.Columns(j).Visible = True Then typwidth += gridView.Columns(j).Width
                            If j <= gridView.Columns.Count Then txt += "~"
                        End If
                    Next
                Next
                If txt <> "" Then .Columns(txt).HeaderText = IIf(hro(i).ToString = "Total", "Total", IIf(hro(i).ToString = "AVG", "AVG", objGPack.GetSqlValue("SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID IN('" & hro(i).ToString & "')").ToString)) : .Columns(txt).Width = typwidth
            Next
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width
            gridheader3.Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width
            .Columns("OPEANING").Width = gridView.Columns("OPEANING").Width
            gridheader3.Columns("OPEANING").Width = gridView.Columns("OPEANING").Width
            gridView.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            gridheader3.Columns("PARTICULAR").Frozen = True
            gridView.Columns("PARTICULAR").Frozen = True
            gridView.Columns("PARTICULAR").HeaderText = "PARTICULAR"
            gridheader3.Columns("OPEANING").Frozen = True
            gridView.Columns("OPEANING").Frozen = True
            gridView.Columns("OPEANING").HeaderText = "OPEANING"
            If colWid >= gridView.Width Then
                gridheader3.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridheader3.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                gridheader3.Columns("SCROLL").Visible = False
            End If
        End With

        For i As Integer = 0 To gridView.Columns.Count - 1
            With dttype.Rows(0)
                If gridView.Columns(i).HeaderText.Contains(.Item(0).ToString & "-Value") Then gridView.Columns(i).HeaderText = .Item(0).ToString & "-VALUE"
                If gridView.Columns(i).HeaderText.Contains(.Item(0).ToString & "-%") Then gridView.Columns(i).HeaderText = .Item(0).ToString & "-%"
                gridView.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End With

        Next
        For i As Integer = 0 To gridView.Rows.Count - 1
            If gridView.Rows(i).Cells("COLHEAD").Value = "G" Then
                gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightYellow
                gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Black
                gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
        gridView.Columns("PARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

    End Sub


    Public Function GetSelectedAcgrpId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'", , "", )
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            If WithQuotes Then retStr += "'"
            retStr += "" & strCompanyId & ""
            If WithQuotes Then retStr += "'"
        End If
        Return retStr
    End Function

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWidth(CType(sender, DataGridView))
    End Sub

    Private Function GetNextColName(ByVal ColName As String) As String
        Dim Sp() As String = ColName.Split("-")
        Return Sp(0)
    End Function

    Private Sub GridViewHeaderCreator(ByVal Dgv As DataGridView, ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        Dim ColName As String = ""
        For cnt As Integer = 0 To Dgv.Columns.Count - 1
            If cnt <> Dgv.Columns.Count - 1 Then
                If Not Dgv.Columns(cnt + 1).Name.Contains(GetNextColName(Dgv.Columns(cnt).Name)) Then
                    ColName = IIf(ColName <> "", ColName + "~", ColName) + Dgv.Columns(cnt).Name
                    dtHead.Columns.Add(ColName, GetType(String))
                    dtHead.Columns(ColName).Caption = GetNextColName(ColName)
                    ColName = ""
                Else
                    ColName += Dgv.Columns(cnt).Name
                End If
            End If
        Next
        dtHead.Columns.Add("SCROLL", GetType(String))
        gridviewHead.DataSource = dtHead
        For Each DtCol As DataColumn In dtHead.Columns
            gridviewHead.Columns(DtCol.ColumnName).HeaderText = DtCol.Caption
        Next
        gridviewHead.Visible = True
        SetGridHeadColWidth(Dgv)
    End Sub


    Private Function GetDgvHeaderText(ByVal Dgv As DataGridView, ByVal ColName As String) As String
        Dim Sp() As String = ColName.Split("-")
        Dim HText As String = ColName
        If Sp.Length > 1 Then
            HText = Sp(1)
        End If
        Return UCase(HText)
    End Function

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For cnt As Integer = 0 To dgv.ColumnCount - 1
                dgv.Columns(cnt).HeaderText = GetDgvHeaderText(dgv, dgv.Columns(cnt).Name)
            Next
        End With
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New ExpenseAnalysis_Properties
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        GetChecked_CheckedList(chklstgroup, obj.p_chklstgroup)
        SetSettingsObj(obj, Me.Name, GetType(ExpenseAnalysis_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New ExpenseAnalysis_Properties
        GetSettingsObj(obj, Me.Name, GetType(ExpenseAnalysis_Properties))
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        SetChecked_CheckedList(chklstgroup, obj.p_chklstgroup, Nothing)
    End Sub

End Class

Public Class ExpenseAnalysis_Properties
    Private chkLstCompany As New List(Of String)
    Public Property p_chkLstCompany() As List(Of String)
        Get
            Return chkLstCompany
        End Get
        Set(ByVal value As List(Of String))
            chkLstCompany = value
        End Set
    End Property
    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
    Private chklstgroup As New List(Of String)
    Public Property p_chklstgroup() As List(Of String)
        Get
            Return chklstgroup
        End Get
        Set(ByVal value As List(Of String))
            chklstgroup = value
        End Set
    End Property
End Class