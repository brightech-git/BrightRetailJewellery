Imports System.Data.OleDb
Public Class frmClosingStockReport
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtCostCentre As New DataTable
    Dim dtCompany As New DataTable
    Private Sub AccSalesAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Function LoadCombo(ByVal CmbBox As ComboBox)
        If CType(CmbBox, ComboBox).Name = cmbMetal.Name Then
            cmbMetal.Items.Clear()
            cmbMetal.Items.Add("ALL")
            strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
            objGPack.FillCombo(strSql, cmbMetal, False)
            cmbMetal.Text = "ALL"
        ElseIf CType(CmbBox, ComboBox).Name = cmbCategory.Name Then
            Dim Metalname As String = ""
            If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then
                Metalname = cmbMetal.Text
                Metalname = Replace(Metalname, ",", "','")
            End If
            cmbCategory.Items.Add("ALL")
            strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
            If Metalname <> "" Then
                strSql = strSql + " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN ('" & Metalname & "')) "
            End If
            strSql = strSql + " ORDER BY SHORTNAME"
            objGPack.FillCombo(strSql, cmbCategory)
            cmbCategory.Text = "ALL"
        End If
    End Function
    Private Sub AccSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized

        strSql = " Select 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%' "
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        btnNew_Click(Me, New EventArgs)

    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        tabMain.SelectedTab = TabView
        lblTitle.Text = ""
        gridView.DataSource = Nothing

        Dim StrNew As String
        Dim Startdate As Date
        StrNew = "SELECT STARTDATE FROM " & cnAdminDb & "..DBMASTER WHERE '" & IIf(chkAsOnDate.Checked, dtpFrom.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "' BETWEEN STARTDATE AND ENDDATE "
        Startdate = GetSqlValue(cn, StrNew)
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME")
        Dim chkCompanyId As String = GetQryStringForSp(CmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME")

        Dim metalId As String = "ALL"
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            metalId = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'", , "ALL")
        End If
        Dim selCatCode As String = Nothing
        If cmbCategory.Text = "ALL" Then
            selCatCode = "ALL"
        ElseIf cmbMetal.Text <> "" Then
            Dim sql As String = "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(cmbCategory.Text) & ")"
            Dim dtCat As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtCat)
            If dtCat.Rows.Count > 0 Then
                'selCatCode = "'"
                For i As Integer = 0 To dtCat.Rows.Count - 1
                    selCatCode += dtCat.Rows(i).Item("CATCODE").ToString + ","
                    'selCatCode += dtItem.Rows(i).Item("ITEMID").ToString
                Next
                If selCatCode <> "" Then
                    selCatCode = Mid(selCatCode, 1, selCatCode.Length - 1)
                End If
                'selCatCode += "'"
            End If
        End If
        If chkAsOnDate.Checked Then
            If Not CheckBckDays(userId, Me.Name, GetServerDate()) Then dtpTo.Focus() : Exit Sub
        Else
            If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        End If

        strSql = " EXEC " & cnAdminDb & "..SP_CLOSING_STOCK"
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@ADMINDB = '" & cnAdminDb & "'"
        If chkAsOnDate.Checked Then
            strSql += vbCrLf + " ,@FRMDATE = '" & Startdate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + " ,@FRMDATE = '" & Startdate.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
        strSql += vbCrLf + " ,@CATCODE = '" & selCatCode & "'"
        strSql += vbCrLf + " ,@CATNAME = '" & cmbCategory.Text & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " ,@RPTTYPE = '" & IIf(rbtSummary.Checked, "S", "R") & "'"
        Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        strSql += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        Dim dtGrid As New DataTable
        Dim dCol As New DataColumn("KEYNO")
        dCol.AutoIncrement = True
        dCol.AutoIncrementSeed = 0
        dCol.AutoIncrementStep = 1
        dtGrid.Columns.Add(dCol)
        If rbtDateWise.Checked Then
            strSql = "SELECT * FROM TEMPTABLEDB..TEMPSTOCK" & SYSTEMID & "REPORT "
            If cmbCategory.Text <> "ALL" Then
                strSql = strSql + " WHERE CATNAME IN ('" & Replace(cmbCategory.Text, ",", "','") & "') "
            End If
            strSql = strSql + " ORDER BY  CATNAME ,RESULT,TRANDATE"
        ElseIf rbtSummary.Checked Then
            strSql = "SELECT * FROM TEMPTABLEDB..TEMPCSTKVALUE4" & SYSTEMID
            strSql = strSql + " ORDER BY METALNAME,RESULT"
        End If

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record Not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("KEYNO").SetOrdinal(3)
        gridView.DataSource = dtGrid
        FillGridGroupStyle_KeyNoWise(gridView)
        Dim tit As String = Nothing
        tit = "CLOSING STOCK REPORT from " & dtpFrom.Text & " to " & dtpTo.Text
        lblTitle.Text = tit
        With gridView
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            If .Columns.Contains("CATCODE") Then .Columns("CATCODE").Visible = False
            If .Columns.Contains("CATNAME") Then .Columns("CATNAME").Visible = False
            If .Columns.Contains("REGISTER") Then .Columns("REGISTER").Visible = False
            If .Columns.Contains("REFNO") Then .Columns("REFNO").Visible = False
            If .Columns.Contains("PARTICULARS") Then .Columns("PARTICULARS").Width = 250
            If .Columns.Contains("RPCS") Then .Columns("RPCS").Width = 100
            If .Columns.Contains("RECEIPT_GRSWT") Then .Columns("RECEIPT_GRSWT").Width = 80
            If .Columns.Contains("RECEIPT_NETWT") Then .Columns("RECEIPT_NETWT").Width = 100
            If .Columns.Contains("RECEIPT_AMOUNT") Then .Columns("RECEIPT_AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("RGRSWT") Then .Columns("RGRSWT").Width = 80
            If .Columns.Contains("RNETWT") Then .Columns("RNETWT").Width = 100
            If .Columns.Contains("RAMOUNT") Then .Columns("RAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("IPCS") Then .Columns("IPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("ISSUE_GRSWT") Then .Columns("ISSUE_GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("ISSUE_NETWT") Then .Columns("ISSUE_NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("ISSUE_AMOUNT") Then .Columns("ISSUE_AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("CPCS") Then .Columns("CPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("CLOSING_GRSWT") Then .Columns("CLOSING_GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("CLOSING_NETWT") Then .Columns("CLOSING_NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("CLOSING_AMOUNT") Then .Columns("CLOSING_AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If .Columns.Contains("CLOSING_AVGRATE") Then .Columns("CLOSING_AVGRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            For cnt As Integer = 0 To .ColumnCount - 1
                gridView.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
        FormatGridColumns(gridView, False, False, True, False)
        GridHeaderStyle(gridHeader)
        gridView.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        lblTitle.Text = ""
        cmbCategory.Text = "ALL"
        LoadCombo(cmbMetal)
        DateSelection()
        dtpFrom.Select()
        'tabMain.RectangleToScreen(TabView.DisplayRectangle)
        tabMain.Size = New Size(TabView.Width, TabView.Height)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        BrightPosting.GExport.Post(Me.NAME, cnCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        BrightPosting.GExport.Post(Me.NAME, cnCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
    End Sub

    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = TabGeneral
    End Sub
    Function DateSelection()
        Dim Dtemp As New DataTable
        strSql = "SELECT STARTDATE,ENDDATE FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME = '" & cnStockDb & "' "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(Dtemp)
        If Dtemp.Rows(0).Item("ENDDATE") > Today.ToString("yyyy-MM-dd") Then
            dtpFrom.Value = If(chkAsOnDate.Checked, Today, Dtemp.Rows(0).Item("STARTDATE"))
            dtpTo.Value = IIf(chkAsOnDate.Checked, Dtemp.Rows(0).Item("ENDDATE"), Today)
        Else
            dtpFrom.Value = If(chkAsOnDate.Checked, Dtemp.Rows(0).Item("ENDDATE"), Dtemp.Rows(0).Item("STARTDATE"))
            dtpTo.Value = IIf(chkAsOnDate.Checked, Dtemp.Rows(0).Item("ENDDATE"), Dtemp.Rows(0).Item("ENDDATE"))
        End If
    End Function
    Private Sub GridHeaderStyle(ByVal gridheaderview As DataGridView)
        Dim DtHead As New DataTable
        Dim HeadString As String = ""
        Dim HeadColName As String = ""
        Dim HeadColName1 As String = ""
        gridheaderview.DataSource = Nothing
        Dim STR As String = ""
        Dim Oldstr As String = ""
        With gridView
            For Each Col As DataGridViewColumn In .Columns
                ''''''''''For Getting  Second position (_)
                STR = Col.Name.ToString
                Dim Fristindex As Integer = STR.IndexOf("_", 0) + 2
                Dim Secondindex As Integer = 0
                STR = Mid(STR, Fristindex)
                Secondindex = STR.IndexOf("_", 0) + 2
                STR = Mid(Col.Name.ToString, 1, (Fristindex + Secondindex) - 2)
                If Col.Name.Contains(STR) And STR = Oldstr And Col.Visible Then
                    HeadColName = Col.Name.ToUpper
                    HeadString = HeadString & IIf(HeadString <> "", "~", "") & HeadColName
                ElseIf Col.Visible Then
                    If STR <> Oldstr Then Oldstr = STR
                    If HeadString <> "" Then DtHead.Columns.Add(HeadString, GetType(String)) : HeadString = ""
                    HeadColName = Col.Name.ToUpper
                    HeadString = HeadString & IIf(HeadString <> "", "~", "") & HeadColName
                End If
                gridView.Columns(Col.Name).HeaderText = Replace(Col.Name, STR, "")
            Next
            If HeadString <> "" Then DtHead.Columns.Add(HeadString, GetType(String)) : HeadString = ""
        End With
        DtHead.Columns.Add("SCROLL", GetType(String))
        gridheaderview.DataSource = DtHead
        Dim HealColWidth As Integer = 0 : HeadString = ""
        Dim HeadString1 = "" : Oldstr = "" : STR = ""
        With gridView
            For Each Col As DataGridViewColumn In .Columns
                STR = Col.Name.ToString
                Dim Fristindex As Integer = STR.IndexOf("_", 0) + 2
                Dim Secondindex As Integer = 0
                STR = Mid(STR, Fristindex)
                Secondindex = STR.IndexOf("_", 0) + 2
                STR = Mid(Col.Name.ToString, 1, (Fristindex + Secondindex) - 2)
                If Col.Name.Contains(STR) And STR = Oldstr And Col.Visible Then
                    HeadColName = Col.Name.ToUpper
                    HeadString = HeadString & IIf(HeadString <> "", "~", "") & HeadColName
                    HealColWidth = HealColWidth + Col.Width
                ElseIf Col.Visible Then
                    If HeadString <> "" Then
                        gridheaderview.Columns(HeadString).Width = HealColWidth
                        If STR <> Oldstr And Oldstr <> "" Then
                            HeadString1 = Mid(HeadString, 1, IIf(InStr(HeadString, "_") <> 0, InStr(HeadString, "_") - 1, 0))
                            If HeadString1 Is Nothing Then HeadString1 = ""
                            gridheaderview.Columns(HeadString).HeaderText = HeadString1.ToUpper
                        Else
                            HeadString1 = ""
                            gridheaderview.Columns(HeadString).HeaderText = HeadString1.ToUpper
                        End If
                        HeadString = "" : HeadColName = "" : HealColWidth = 0
                    End If
                    If STR <> Oldstr Then Oldstr = STR
                    HeadColName = Col.Name.ToUpper
                    HeadString = HeadString & IIf(HeadString <> "", "~", "") & HeadColName
                    HealColWidth = HealColWidth + Col.Width
                End If
            Next
            If HeadString <> "" And HealColWidth <> 0 Then
                gridheaderview.Columns(HeadString).Width = HealColWidth
                HeadString1 = Mid(HeadString, 1, IIf(InStr(HeadString, "_") <> 0, InStr(HeadString, "_") - 1, 0))
                If HeadString1 Is Nothing Then HeadString1 = ""
                gridheaderview.Columns(HeadString).HeaderText = HeadString1.ToUpper
            End If
            gridheaderview.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridheaderview.Columns("SCROLL").Visible = False
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            If colWid >= gridView.Width Then
                gridheaderview.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridheaderview.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                gridheaderview.Columns("SCROLL").Visible = False
            End If
            gridheaderview.Columns("SCROLL").HeaderText = ""
        End With
    End Sub
    Private Sub chkAsOnDate_CheckedChanged(sender As Object, e As EventArgs) Handles chkAsOnDate.CheckedChanged
        lblTo.Visible = Not chkAsOnDate.Checked
        dtpTo.Visible = Not chkAsOnDate.Checked
        If chkAsOnDate.Checked Then
            chkAsOnDate.Text = "As On Date"
        Else
            chkAsOnDate.Text = "Date From"
        End If
        DateSelection()
    End Sub

    Private Sub cmbMetal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMetal.SelectedIndexChanged
        LoadCombo(cmbCategory)
    End Sub

    Private Sub gridView_Scroll(sender As Object, e As ScrollEventArgs) Handles gridView.Scroll
        If gridHeader Is Nothing Then Exit Sub
        If Not gridHeader.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridHeader.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridHeader.HorizontalScrollingOffset = e.NewValue
                gridHeader.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridHeader.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
End Class
