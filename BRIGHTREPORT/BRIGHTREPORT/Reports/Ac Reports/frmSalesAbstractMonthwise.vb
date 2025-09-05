Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmSalesAbstractMonthwise
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim dtCostCentre, dtmetal As New DataTable
    Dim OLtran As OleDbTransaction
    Dim costids As String = "ALL"
    Dim metalids As String = "ALL"
    Dim cmd As OleDbCommand

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Trim(chkCmbCostCentre.Text.ToString) = "" Then chkCmbCostCentre.Text = "ALL"
        If Trim(chkcmbmetal.Text.ToString) = "" Then chkcmbmetal.Text = "ALL"
        If Trim(chkCmbCostCentre.Text.ToString) = "ALL" Then costids = "ALL" Else costids = GetSelectedCostId(chkCmbCostCentre, True)
        If Trim(chkcmbmetal.Text.ToString) = "ALL" Then metalids = "ALL" Else metalids = GetSelectedMetalid(chkcmbmetal, True)
        Dim strsql As String
        If chkreceipt.Checked = False And chksales.Checked = False Then
            MsgBox("Pls select the transcation type" & vbCrLf & "Either sales or purchase")
            chksales.Focus()
        End If
        'blagr_int.AGRN_SALPURCHASEAMTWTRPT(cn, OLtran)
        strsql = " EXECUTE " & cnAdminDb & "..AGRN_SALPURCHASEAMTWTRPTMONTHWISE"
        strsql += vbCrLf + "@DBNAME ='" & cnstockdb.ToString & "'"
        strsql += vbCrLf + ",@TEMPTABLE='TEMPTABLEDB..TEMPCURSALESPUR'"
        strsql += vbCrLf + ",@FRMDATE ='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
        strsql += vbCrLf + ",@TODATE ='" & dtpTo.Value.ToString("yyyy/MM/dd") & "' "
        strsql += vbCrLf + ",@COSTID1=""" & costids.ToString & """"
        strsql += vbCrLf + ",@METALID=""" & metalids.ToString & """"
        strsql += vbCrLf + ",@METAL='" & IIf(rbtmonthwise.Checked, "Y", "N") & "'"
        strsql += vbCrLf + ",@CATEGORY='" & IIf(rbtcategorywise.Checked, "Y", "N") & "'"
        strsql += vbCrLf + ",@WITHSR='" & IIf(chkwithsr.Checked, "Y", "N") & "'"
        strsql += vbCrLf + ",@WITHTOT='N'"
        strsql += vbCrLf + ",@MONTHWISE='Y'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.CommandTimeout = 1000
        Dim da As New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        Dim dt As New DataTable
        Dim tempdt As New DataTable
        dt = dss.Tables(0)
        tempdt = dt.Copy
        If chksales.Checked = False Then
            For i As Integer = 0 To dt.Columns.Count - 1
                If dt.Columns(i).Caption.Contains("SALES") = True Then tempdt.Columns.Remove(dt.Columns(i).Caption)
            Next
        End If
        If chkreceipt.Checked = False Then
            For i As Integer = 0 To dt.Columns.Count - 1
                If dt.Columns(i).Caption.Contains("PURCHASE") = True Then tempdt.Columns.Remove(dt.Columns(i).Caption)
            Next
        End If

        'If chkwithtot.Checked = True Then
        '    Dim CNT As Integer = GetSqlValue(cn, "SELECT COUNT(DISTINCT MONT) FROM TEMPTABLEDB..TEMPCURSALESPUR")
        '    If CNT <= 1 Then
        '        For i As Integer = 0 To dt.Columns.Count - 1
        '            If dt.Columns(i).Caption.Contains("TOTAL") = True Then tempdt.Columns.Remove(dt.Columns(i).Caption)
        '        Next
        '    End If
        'End If


        dt = New DataTable
        dt = tempdt.Copy()
        If chkPcs.Checked = False Then
            For i As Integer = 0 To dt.Columns.Count - 1
                If dt.Columns(i).Caption.Contains("PCS") = True Then tempdt.Columns.Remove(dt.Columns(i).Caption)
            Next
        End If
        If chkGrsWt.Checked = False Then
            For i As Integer = 0 To dt.Columns.Count - 1
                If dt.Columns(i).Caption.Contains("WEIGHT") = True Then tempdt.Columns.Remove(dt.Columns(i).Caption)
            Next
        End If
        If chkamt.Checked = False Then
            For i As Integer = 0 To dt.Columns.Count - 1
                If dt.Columns(i).Caption.Contains("AMOUNT") = True Then tempdt.Columns.Remove(dt.Columns(i).Caption)
            Next
        End If

        dt = New DataTable
        dt = tempdt.Copy()
        If dt.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dt

        Else
            gridView.DataSource = Nothing
            gridheader1.DataSource = Nothing
            gridheader2.DataSource = Nothing
            MsgBox("No Records found..")
            Exit Sub
        End If
        With gridView
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COSTNAME").Visible = False
            If rbtmonthwise.Checked Then .Columns("METALNAME").Visible = False
            If rbtcategorywise.Checked Then .Columns("catNAME").Visible = False

            For i As Integer = 0 To gridView.Columns.Count - 1
                If .Columns(i).HeaderText.Contains("PCS") Then .Columns(i).Width = 90
                If .Columns(i).HeaderText.Contains("WEIGHT") Then .Columns(i).Width = 120
                If .Columns(i).HeaderText.Contains("PARTICULAR") Then .Columns(i).Width = 250
            Next

        End With
        GridViewHeaderStyle()

    End Sub

    Private Sub GridViewHeaderStyle()
        Dim dtcost As New DataTable("COST")
        Dim dttype As New DataTable("TYPE")
        Dim hro() As String
        Dim typero(IIf((chksales.Checked And chkreceipt.Checked), 1, 0)) As String
        If chksales.Checked Then typero(0) = "SALES"
        If chkreceipt.Checked And chksales.Checked Then
            typero(1) = "PURCHASE"
        ElseIf chkreceipt.Checked And chksales.Checked = False Then
            typero(0) = "PURCHASE"
        End If
        With dtcost
            .Columns.Add("PARTICULAR", GetType(String))
            dttype.Columns.Add("PARTICULAR", GetType(String))
            Dim cdt As New DataTable
            'strSql = " DECLARE @QRY AS VARCHAR(1000)"
            'strSql += "  SET @QRY=''"
            'If chkwithtot.Checked Then strSql += " IF (SELECT COUNT(DISTINCT TRANDATE) FROM TEMPTABLEDB..TEMPCURSALESPUR )>1 BEGIN SELECT @QRY=' SELECT ''TOTAL'' COSTNAME,0 MONT UNION ALL '  END"
            'strSql += " SELECT @QRY=@QRY+' SELECT DISTINCT TRANDATE COSTNAME,MONT FROM TEMPTABLEDB..TEMPCURSALESPUR ORDER BY MONT' EXEC(@QRY)"

            strSql = "SELECT DISTINCT TRANDATE COSTNAME,MONT FROM TEMPTABLEDB..TEMPCURSALESPUR ORDER BY MONT"

            cdt = GetSqlTable(strSql, cn)
            If cdt.Rows.Count > 0 Then
                costids = ""
                For i As Integer = 0 To cdt.Rows.Count - 1
                    costids += cdt.Rows(i).Item("COSTNAME").ToString
                    If i < cdt.Rows.Count - 1 Then costids += ","
                Next
            End If
            If costids <> "ALL" Then
                costids = costids.Replace("'", "")
                hro = costids.Split(",")
            End If
            For i As Integer = 0 To hro.Length - 1
                Dim txt As String = ""
                For k As Integer = 0 To typero.Length - 1
                    Dim type As String = ""
                    For j As Integer = 2 To gridView.Columns.Count - 1
                        If gridView.Columns(j).HeaderText.Contains(hro(i).ToString) Then
                            If gridView.Columns(j).HeaderText.Contains(typero(k).ToString) Then
                                txt += gridView.Columns(j).HeaderText
                                If j <= gridView.Columns.Count Then txt += "~"

                                type += gridView.Columns(j).HeaderText
                                If j <= gridView.Columns.Count Then type += "~"
                            End If
                        End If
                    Next
                    If type <> "" Then txt += "$" : dttype.Columns.Add(type, GetType(String)) : dttype.Columns(type).Caption = typero(k).ToString
                Next
                If txt <> "" Then .Columns.Add(txt, GetType(String)) : .Columns(txt).Caption = hro(i).ToString
            Next
            dtcost.Columns.Add("SCROLL", GetType(String))
            dttype.Columns.Add("SCROLL", GetType(String))
            dtcost.Columns("PARTICULAR").Caption = ""
            dttype.Columns("PARTICULAR").Caption = ""
            dtcost.Columns("SCROLL").Caption = ""
            dttype.Columns("SCROLL").Caption = ""
        End With

        With gridheader1
            .DataSource = Nothing
            gridheader2.DataSource = Nothing
            .DataSource = dtcost
            gridheader2.DataSource = dttype
            .Columns("PARTICULAR").HeaderText = "PARTICULAR"
            gridheader2.Columns("PARTICULAR").HeaderText = ""
            For i As Integer = 0 To hro.Length - 1
                Dim txt As String = ""
                For k As Integer = 0 To typero.Length - 1
                    Dim type As String = ""
                    For j As Integer = 2 To gridView.Columns.Count - 1
                        If gridView.Columns(j).HeaderText.Contains(hro(i).ToString) Then
                            If gridView.Columns(j).HeaderText.Contains(typero(k).ToString) Then
                                txt += gridView.Columns(j).HeaderText
                                If j <= gridView.Columns.Count Then txt += "~"
                                type += gridView.Columns(j).HeaderText
                                If j <= gridView.Columns.Count Then type += "~"
                            End If
                        End If
                    Next
                    If type <> "" Then txt += "$" : gridheader2.Columns(type).HeaderText = typero(k).ToString
                Next
                If txt <> "" Then .Columns(txt).HeaderText = hro(i).ToString
            Next
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridheader2.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridheader2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width
            gridheader2.Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width
            For i As Integer = 0 To hro.Length - 1
                Dim txt As String = ""
                Dim width As Integer = 0
                For k As Integer = 0 To typero.Length - 1
                    Dim type As String = ""
                    Dim typwidth As Integer = 0
                    Dim metalwidth As Integer = 0
                    For j As Integer = 2 To gridView.Columns.Count - 1
                        If gridView.Columns(j).HeaderText.Contains(hro(i).ToString) Then
                            If gridView.Columns(j).HeaderText.Contains(typero(k).ToString) Then
                                'If gridView.Columns(j).HeaderText.Contains(mt.Rows(w).Item("METALNAME").ToString) Then
                                txt += gridView.Columns(j).HeaderText
                                width += gridView.Columns(j).Width
                                If j <= gridView.Columns.Count Then txt += "~"
                                type += gridView.Columns(j).HeaderText
                                typwidth += gridView.Columns(j).Width
                                If j <= gridView.Columns.Count Then type += "~"
                            End If
                        End If
                    Next
                    If type <> "" Then txt += "$" : gridheader2.Columns(type).Width = typwidth
                Next
                If txt <> "" Then .Columns(txt).Width = width
            Next
            gridView.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            gridheader1.Columns("PARTICULAR").Frozen = True
            gridheader2.Columns("PARTICULAR").Frozen = True
            gridView.Columns("PARTICULAR").Frozen = True
            gridView.Columns("PARTICULAR").HeaderText = "PARTICULAR"
            If colWid >= gridView.Width Then
                gridheader1.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridheader1.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                gridheader2.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridheader2.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                gridheader1.Columns("SCROLL").Visible = False
                gridheader2.Columns("SCROLL").Visible = False
            End If
        End With
        For i As Integer = 0 To gridView.Columns.Count - 1
            'If chkgrpcostcentre.Checked = False Then
            '    If i >= 4 And gridView.Columns(i).Name.Contains("TOTAL") = False Then gridView.Columns(i).Visible = False
            'End If
            If gridView.Columns(i).HeaderText.Contains("PCS") Then gridView.Columns(i).HeaderText = "PCS"
            If gridView.Columns(i).HeaderText.Contains("WEIGHT") Then gridView.Columns(i).HeaderText = "GRSWT"
            If gridView.Columns(i).HeaderText.Contains("AMOUNT") Then gridView.Columns(i).HeaderText = "AMOUNT"
            gridView.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
        Next
        For i As Integer = 0 To gridView.Rows.Count - 1

            If gridView.Rows(i).Cells("COLHEAD").Value = "G" Then
                gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightYellow
                gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Black
                gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

            ElseIf gridView.Rows(i).Cells("COLHEAD").Value = "H" Then
                gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridView.Rows(i).Cells("COLHEAD").Value = "S" Then
                gridView.Rows(i).DefaultCellStyle.BackColor = Color.White
                gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Blue
                gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
        gridView.Columns("PARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        'gridView.Columns("PARTICULAR").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Dim dt As DataTable = CType(gridView.DataSource, DataTable)
        Dim dt1 As DataTable = CType(gridheader1.DataSource, DataTable)
        Dim dt2 As DataTable = CType(gridheader2.DataSource, DataTable)
        gridheader1.Columns("SCROLL").Visible = False
        gridheader2.Columns("SCROLL").Visible = False
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub

    Private Sub frmSalesAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT 'ALL'COSTNAME, 'ALL' COSTID,1 RESULT UNION ALL SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnadmindb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")

        strSql = " SELECT 'ALL'METALNAME, 'ALL' METALID,1 RESULT UNION ALL SELECT METALNAME,CONVERT(vARCHAR,METALID),2 RESULT FROM " & cnadmindb & "..METALMAST"
        strSql += " ORDER BY RESULT,METALNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtmetal)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbmetal, dtmetal, "METALNAME", , "ALL")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = System.DateTime.Now
        dtpTo.Value = System.DateTime.Now
        pnlHeading.Visible = False
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True
        chkCmbCostCentre.Text = "ALL"
        chkcmbmetal.Text = "ALL"
        dtpFrom.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If gridView.Rows.Count > 0 Then
            Dim rpttitle As String
            rpttitle = "SALES AND PURCHASE ABSTRACT MONTH WISE"
            If chkwithsr.Checked Then lblTitle.Text = "WITH SALES RETURN " Else lblTitle.Text = ""
            If chkwithtot.Checked Then rpttitle += "-TOTAL "
            If chkGrsWt.Checked Then rpttitle += " WEIGHT"
            If chkGrsWt.Checked And chkamt.Checked Then rpttitle += " , "
            If chkamt.Checked Then rpttitle += " AMOUNT "
            rpttitle += " BETWEEN [" & dtpFrom.Value.ToString("dd/MM/yyyy") & "] AND [" & dtpTo.Value.ToString("dd/MM/yyyy") & "]"
            BrightPosting.GExport.Post(Me.Name, rpttitle, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridheader1, Nothing, Nothing)
        End If

    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then

        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub


    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
       
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridheader1.HorizontalScrollingOffset = e.NewValue
            gridheader2.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then

                gridheader1.HorizontalScrollingOffset = e.NewValue
                gridheader1.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridheader1.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width

                gridheader2.HorizontalScrollingOffset = e.NewValue
                gridheader2.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridheader2.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width

            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub chkcmbmetal_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbmetal.Leave
        'If chkcmbmetal.Text.Contains("ALL") Or chkcmbmetal.Text = "" Then chkcmbmetal.Text = "ALL"
        If chkcmbmetal.CheckedItems.Contains("ALL") Or chkcmbmetal.Text = "" Then
            chkcmbmetal.Text = "ALL"
            chkcmbmetal.SetItemCheckState(0, CheckState.Checked)
            For i As Integer = 1 To chkcmbmetal.Items.Count - 1
                If chkcmbmetal.GetItemCheckState(i) = CheckState.Checked Then
                    chkcmbmetal.SetItemCheckState(i, CheckState.Unchecked)
                End If
            Next
        End If

    End Sub

    Private Sub chkCmbCostCentre_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbCostCentre.Leave
        If chkCmbCostCentre.CheckedItems.Contains("ALL") Or chkCmbCostCentre.Text = "" Then
            chkCmbCostCentre.Text = "ALL"
            chkCmbCostCentre.SetItemCheckState(0, CheckState.Checked)
            For i As Integer = 1 To chkCmbCostCentre.Items.Count - 1
                If chkCmbCostCentre.GetItemCheckState(i) = CheckState.Checked Then
                    chkCmbCostCentre.SetItemCheckState(i, CheckState.Unchecked)
                End If
            Next
        End If
    End Sub
End Class