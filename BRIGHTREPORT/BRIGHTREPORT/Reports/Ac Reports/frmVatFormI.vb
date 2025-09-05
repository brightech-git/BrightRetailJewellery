Imports System.Data.OleDb
Public Class frmVatFormI

    'CALNO 051212 VASANTH  FOR GIRITECH MUMBAI

    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim objGridShower As frmGridDispDia
    Dim dtMetal As New DataTable
    Dim dtCounter As New DataTable()
    Dim dtCostCentre As New DataTable
    Dim dtItem As New DataTable
    Dim dtDesigner As New DataTable
    Dim dtOrderStatus As New DataTable


    Private Sub frmOrderStatusReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmOrderStatusReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.TabGeneral.Left, Me.TabGeneral.Top, Me.TabGeneral.Width, Me.TabGeneral.Height))
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)

        btnNew_Click(Me, New EventArgs)
    End Sub


    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        tabMain.SelectedTab = TabGeneral

        Dim dtcompany = New DataTable
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtcompany = GetSqlTable(strSql, cn)
        If dtcompany.Rows.Count > 0 Then
            cmbcompany.DataSource = dtcompany
            cmbcompany.DisplayMember = "COMPANYNAME"
            cmbcompany.ValueMember = "COMPANYID"
            cmbcompany.SelectedIndex = 0
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Report()
    End Sub

    Private Sub Report()
        Dim companylist As String
        Dim mtdate As DateTime
        Dim todate As DateTime

        Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString(), 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        Dim mtemptable As String = "TEMPTABLEDB..TEMP" & SYSTEMID & "VATFORMI"


        companylist = "'" & cmbcompany.SelectedValue.ToString() & "'"
        todate = dtpto.Value
        todate = DateAdd("d", -todate.Day + 1, todate)
        Dim fd As String = Microsoft.VisualBasic.Format(Convert.ToDateTime(todate), "dd/MMM/yyyy")
        Dim td As String = Microsoft.VisualBasic.Format(DateAdd("d", -1, DateAdd("m", 1, fd)), "dd/MMM/yyyy")

        strSql = "EXEC " & cnAdminDb & "..SP_RPT_VATFORMI"
        strSql += vbCrLf + " @TEMPTABLE ='" & mtemptable & "'"
        strSql += vbCrLf + ",@DBNAME= '" & cnStockDb & "'"
        strSql += vbCrLf + ",@FROMDATE='" & fd & "'"
        strSql += vbCrLf + ",@TODATE='" & td & "'"
        strSql += vbCrLf + ",@COMPANYIDS=""" & companylist & """"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM " & mtemptable & "1 ORDER BY TRANTYPE,CATNAME,RESULT"
        Dim dtsales = New DataTable
        dtsales.Columns.Add("KEYNO", GetType(Integer))
        dtsales.Columns("KEYNO").AutoIncrement = True
        dtsales.Columns("KEYNO").AutoIncrementSeed = 0
        dtsales.Columns("KEYNO").AutoIncrementStep = 1
        Dim da As New OleDbDataAdapter(cmd)
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtsales)
        dtsales.Columns("KEYNO").SetOrdinal(dtsales.Columns.Count - 1)

        gridView.DataSource = dtsales
        'If dtsales.Rows.Count > 0 Then
        '    tabMain.SelectedTab = tabView
        '    gridView.Focus()
        'End If
        If Not dtsales.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPVATFORMITITLE')> 0"
        strSql += vbCrLf + "     DROP TABLE TEMPVATFORMITITLE"
        strSql += vbCrLf + "SELECT 'VAT REPORT FOR THE MONTH OF  ( " & Convert.ToDateTime(fd).ToString("dd-MM-yyyy") & " TO " & Convert.ToDateTime(td).ToString("dd-MM-yyyy") & " )' AS FROMTODATE,'" & cmbcompany.Text.ToString() & "' AS TITTLE  INTO TEMPVATFORMITITLE "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim OpenVat As Decimal
        Dim Input As Decimal = Val(dtsales.Compute("SUM(TAX)", "ISNULL(COLHEAD,'')='G' AND TRANTYPE='PURCHASE'").ToString)
        Dim Output As Decimal = Val(dtsales.Compute("SUM(TAX)", "ISNULL(COLHEAD,'')='G' AND TRANTYPE='SALES'").ToString)
        Dim UsPay As Decimal = Val(dtsales.Compute("SUM(ASSESSABLEVALUE)", "ISNULL(COLHEAD,'')='G' AND TRANTYPE='PURCHASE'").ToString)
        If UsPay <> 0 Then UsPay = (UsPay * 1) / 100 : UsPay = CalcRoundoffAmt(UsPay, "H")
        If Input <> 0 Then Input = CalcRoundoffAmt(Input, "H")
        If Output <> 0 Then Output = CalcRoundoffAmt(Output, "H")
        Dim OpenAccode As String = GetAdmindbSoftValue("VATOPNCODE", "", tran).ToString
        Dim MM As Integer = dtpto.Value.Month
        Dim YY As Integer = dtpto.Value.Year
        strSql = "SELECT SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE -1*AMOUNT END)AMOUNT "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(CANCEL,'')=''"
        strSql += vbCrLf + " AND COMPANYID IN(" & companylist & ")"
        If OpenAccode <> "" Then strSql += vbCrLf + " AND ACCODE='" & OpenAccode & "'"
        strSql += vbCrLf + " AND MONTH(TRANDATE)<" & MM
        strSql += vbCrLf + " AND YEAR(TRANDATE)=" & YY
        OpenVat = Val(objGPack.GetSqlValue(strSql, "AMOUNT", , tran))

        Dim objrepName As New CrystalDecisions.Shared.ParameterValues
        Dim objrepName1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepOpenVat As New CrystalDecisions.Shared.ParameterValues
        Dim objrepOpenVat1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepInput As New CrystalDecisions.Shared.ParameterValues
        Dim objrepInput1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepOutput As New CrystalDecisions.Shared.ParameterValues
        Dim objrepOutput1 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepT1 As New CrystalDecisions.Shared.ParameterValues
        Dim objrepT11 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepT2 As New CrystalDecisions.Shared.ParameterValues
        Dim objrepT21 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepT3 As New CrystalDecisions.Shared.ParameterValues
        Dim objrepT31 As New CrystalDecisions.Shared.ParameterDiscreteValue
        Dim objrepUsPay As New CrystalDecisions.Shared.ParameterValues
        Dim objrepUsPay1 As New CrystalDecisions.Shared.ParameterDiscreteValue

        objrepName1.Value = "VAT ABSTRACT FOR " & dtpto.Value.ToString("MMM-yyyy")
        objrepOpenVat1.Value = Format(OpenVat, "0.00")
        objrepInput1.Value = Format(Input, "0.00")
        objrepOutput1.Value = Format(Output, "0.00")
        objrepT11.Value = Format((OpenVat + Input), "0.00")
        objrepT21.Value = Format((Val(objrepOutput1.Value.ToString) + Output), "0.00")
        objrepUsPay1.Value = Format(UsPay, "0.00")
        objrepT31.Value = Format((Val(objrepT21.Value.ToString) + UsPay), "0.00")

        objrepName.Add(objrepName1)
        objrepOpenVat.Add(objrepOpenVat1)
        objrepInput.Add(objrepInput1)
        objrepOutput.Add(objrepOutput1)
        objrepT1.Add(objrepT11)
        objrepT2.Add(objrepT21)
        objrepT3.Add(objrepT31)
        objrepUsPay.Add(objrepUsPay1)

        Dim objrpt As New rptVatFormat
        objrpt.SetParameterValue("rbtName", objrepName1)
        objrpt.SetParameterValue("rptOpenVat", objrepOpenVat1)
        objrpt.SetParameterValue("rptInput", objrepInput1)
        objrpt.SetParameterValue("rptOutput", objrepOutput1)
        objrpt.SetParameterValue("rptT1", objrepT1)
        objrpt.SetParameterValue("rptT2", objrepT2)
        objrpt.SetParameterValue("rptT3", objrepT3)
        objrpt.SetParameterValue("rptUsPay", objrepUsPay1)

        Dim objReport As New BrighttechReport
        Dim objRptViewer As New frmReportViewer
        objRptViewer.CrystalReportViewer1.ReportSource = objReport.ShowReport(objrpt, cnDataSource)
        objRptViewer.Dock = DockStyle.Fill
        objRptViewer.Show()
        objRptViewer.CrystalReportViewer1.Select()

        'funcGridSalesReportstylefunction()

        'strSql = "VAT FORMAT FOR THE DATE"
        'strSql += " FROM ( " + fd + " TO " + td + " )"
        'lbltitle.Text = strSql

    End Sub
    Private Sub gridSalesReport_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
      
    End Sub
    Private Sub GridFormatting(ByVal Dgv As DataGridView)
        FillGridGroupStyle_KeyNoWise(Dgv)
    End Sub
    Private Sub gridsalesreport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnNew.Focus()
        End If
    End Sub

    Public Function funcGridSalesReportstylefunction() As Integer
        With gridView

            If .Columns.Contains("TRANTYPE") Then .Columns("TRANTYPE").Visible = False
            If .Columns.Contains("CATNAME") Then .Columns("CATNAME").Visible = False
            If .Columns.Contains("RESULT") Then .Columns("RESULT").Visible = False
            If .Columns.Contains("COLHEAD") Then .Columns("COLHEAD").Visible = False
            If .Columns.Contains("KEYNO") Then .Columns("KEYNO").Visible = False

            With .Columns("PARTICULAR")
                .Width = 200
                '.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                '.DefaultCellStyle.BackColor = Color.AliceBlue
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ASSESSABLEVALUE")
                '                .HeaderText = "TABLE"
                .Width = 200
                '.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("AMOUNT")
                .Width = 100
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TAX")
                .Width = 100
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

            '.Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Regular)
            gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function
    Function GridViewFormat() As Integer
        'For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
        '    With dgvRow
        '        Select Case .Cells("STATUS").Value.ToString
        '            Case "BOOKED"
        '                .DefaultCellStyle.BackColor = Color.AliceBlue
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '            Case "ISSUE TO SMITH"
        '                .DefaultCellStyle.BackColor = Color.LightPink
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '            Case "RECEIVE FROM SMITH"
        '                .DefaultCellStyle.BackColor = Color.RosyBrown
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '            Case "READY FOR DELIVERY"
        '                .DefaultCellStyle.BackColor = Color.Orange
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '            Case "DELIVERED"
        '                .DefaultCellStyle.BackColor = Color.LightGreen
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '            Case "CANCELLED"
        '                .DefaultCellStyle.BackColor = Color.Red
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '            Case "PENDING WITH US"
        '                .DefaultCellStyle.BackColor = Color.Wheat
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '            Case "APPROVAL ISSUE"
        '                .DefaultCellStyle.BackColor = Color.LightSkyBlue
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '        End Select
        '    End With
        'Next
    End Function


    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'If UCase(e.KeyChar) = "D" Then
        '    If GetAdmindbSoftValue("PRN_ORIR", "N") = "Y" Then
        '        Dim prnmemsuffix As String = ""
        '        Dim pbatchno As String
        '        Dim pbilldate As Date
        '        Dim dgv As DataGridView = CType(sender, DataGridView)
        '        If dgv.CurrentRow Is Nothing Then Exit Sub
        '        If dgv.CurrentRow.Cells("BATCHNO").Value.ToString = "" Then Exit Sub
        '        pbatchno = dgv.CurrentRow.Cells("BATCHNO").Value.ToString
        '        pbilldate = dgv.CurrentRow.Cells("ORDATE").Value.ToString
        '        If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId

        '        If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
        '            Dim write As IO.StreamWriter
        '            Dim memfile As String = "\BillPrint" & prnmemsuffix.Trim & ".mem"
        '            write = IO.File.CreateText(Application.StartupPath & memfile)
        '            write.WriteLine(LSet("TYPE", 15) & ":OIR")
        '            write.WriteLine(LSet("BATCHNO", 15) & ":" & pbatchno)
        '            write.WriteLine(LSet("TRANDATE", 15) & ":" & pbilldate.ToString("yyyy-MM-dd"))
        '            write.WriteLine(LSet("DUPLICATE", 15) & ":N")
        '            write.Flush()
        '            write.Close()
        '            If EXE_WITH_PARAM = False Then
        '                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
        '            Else
        '                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
        '                LSet("TYPE", 15) & ":OIR" & ";" & _
        '                LSet("BATCHNO", 15) & ":" & pbatchno & ";" & _
        '                LSet("TRANDATE", 15) & ":" & pbilldate.ToString("yyyy-MM-dd") & ";" & _
        '                LSet("DUPLICATE", 15) & ":N")
        '            End If
        '        Else
        '            MsgBox("Billprint exe not found", MsgBoxStyle.Information)
        '        End If
        '    End If
        'End If
    End Sub

    Private Sub TabGeneral_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabGeneral.Click

    End Sub

   

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = TabGeneral
        dtpto.Focus()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

    End Sub

    Private Sub cmbcompany_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbcompany.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If

    End Sub

    Private Sub cmbcompany_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbcompany.KeyPress

    End Sub
End Class























