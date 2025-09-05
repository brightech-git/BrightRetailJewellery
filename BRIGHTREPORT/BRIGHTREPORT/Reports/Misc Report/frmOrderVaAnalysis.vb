Imports System.Data.OleDb
Public Class frmOrderVaAnalysis
    Dim objGridShower As frmGridDispDia
    Dim dtSalesPerson As New DataTable
    Dim dsSalesPerson As New DataSet
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtCostCentre As New DataTable

    Private Sub frmWastagewiseSales_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY DISPLAYORDER,METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False, False)
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        funcNew()
        dtpFrom.Select()
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Try
            Dim _metalId As String = ""
            If cmbMetal.Text = "ALL" And cmbMetal.Text = "" Then
                _metalId = cmbMetal.Text.ToString.Substring(0, 1)
            End If

            strSql = " EXEC " & cnAdminDb & "..RPT_ORDERSMITH"
            strSql += vbCrLf + " @FROMDATE ='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "',"
            strSql += vbCrLf + " @TODATE ='" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "',"
            strSql += vbCrLf + " @DBNAME='" & cnStockDb & "',"
            strSql += vbCrLf + " @COMPANYID='" & cnCompanyId & "',"
            strSql += vbCrLf + " @COSTID='',"
            strSql += vbCrLf + " @METALID='" & _metalId & "',"
            strSql += vbCrLf + " @ORNO=''"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            dtSalesPerson = New DataTable
            dtSalesPerson.Columns.Add("KEYNO", GetType(Integer))
            dtSalesPerson.Columns("KEYNO").AutoIncrement = True
            dtSalesPerson.Columns("KEYNO").AutoIncrementSeed = 0
            dtSalesPerson.Columns("KEYNO").AutoIncrementStep = 1
            strSql = " SELECT * FROM TEMPTABLEDB..TEMPPAYABLERPT ORDER BY RESULT,ORDATE,ORNO "
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSalesPerson)
            If dtSalesPerson.Rows.Count < 1 Then
                btnView_Search.Enabled = True
                MsgBox("Records not found..", MsgBoxStyle.Information, "Message")
                Exit Sub
            End If
            dtSalesPerson.Columns("KEYNO").SetOrdinal(dtSalesPerson.Columns.Count - 1)
            gridView.DataSource = dtSalesPerson
            For Each gv As DataGridViewRow In gridView.Rows
                With gv
                    Select Case .Cells("RESULT").Value.ToString
                        Case "2"
                            .DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                            .DefaultCellStyle.ForeColor = Color.DarkGreen
                            .DefaultCellStyle.Font = New Font("VERDHANA", 8, FontStyle.Bold)
                    End Select
                End With
            Next
            With gridView
                .Columns("ORNO").HeaderText = "NO"
                .Columns("ORDATE").HeaderText = "DATE"
                .Columns("OPCS").HeaderText = "PCS"
                .Columns("OGRSWT").HeaderText = "GRSWT"
                .Columns("OSTNWT").HeaderText = "STONE"
                .Columns("ONETWT").HeaderText = "NETWT"
                .Columns("RPCS").HeaderText = "PCS"
                .Columns("RGRSWT").HeaderText = "GRSWT"
                .Columns("RNETWT").HeaderText = "NET"
                .Columns("RWASTAGE").HeaderText = "WASTAGE"
                .Columns("RPUREWT").HeaderText = "PUREWT"
                .Columns("RMCGRM").HeaderText = "MC/GRM"
                .Columns("RMCHARGE").HeaderText = "MC"
                .Columns("RTOUCH").HeaderText = "TOUCH"
                .Columns("WASTVAL").HeaderText = "WAST COST"
                .Columns("PAYABLE").HeaderText = "PAYABLE"

                .Columns("ORNO").DefaultCellStyle.BackColor = Color.MistyRose
                .Columns("ORDATE").DefaultCellStyle.BackColor = Color.MistyRose
                .Columns("OPCS").DefaultCellStyle.BackColor = Color.MistyRose
                .Columns("OGRSWT").DefaultCellStyle.BackColor = Color.MistyRose
                .Columns("OSTNWT").DefaultCellStyle.BackColor = Color.MistyRose
                .Columns("ONETWT").DefaultCellStyle.BackColor = Color.MistyRose
            End With
            gridView.Columns("RESULT").Visible = False
            gridView.Columns("KEYNO").Visible = False
            Dim strTitle As String = Nothing
            strTitle = "ORDER VA ANALYSIS FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            strTitle += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
            lblTitle.Text = strTitle
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            gridView.Focus()
        Catch ex As Exception
            btnView_Search.Enabled = True
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try

        btnView_Search.Enabled = True
        'End If
    End Sub


    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Function funcGridHeaderNew() As Integer
        Try
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                .Columns.Add("SALESPERSON", GetType(String))
                .Columns.Add("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENAMOUNT", GetType(String))
                .Columns.Add("SALEPCS~SALEWEIGHT~SALESTNWT~SALEAMOUNT", GetType(String))
                .Columns.Add("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNAMOUNT", GetType(String))
                .Columns.Add("PCS~WEIGHT~STNWT~AMOUNT", GetType(String))
                .Columns.Add("SCROLL", GetType(String))
                .Columns("SALESPERSON").Caption = ""
                .Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENAMOUNT").Caption = "OPENING"
                .Columns("SALEPCS~SALEWEIGHT~SALESTNWT~SALEAMOUNT").Caption = "SALES"
                .Columns("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNAMOUNT").Caption = "RETURN"
                .Columns("PCS~WEIGHT~STNWT~AMOUNT").Caption = "DIFFERENCE"
                .Columns("Scroll").Caption = ""

            End With

            ''Dim dtHeader As New DataTable
            ''gridViewHead.DataSource = Nothing
            ''If chkCounterWise.Checked = True Then
            ''    strSql = "select ''SALESPERSON,''SALES,''RETURN1,''DIFFERENCE1,''SCROLL WHERE 1<>1"
            ''Else
            ''    strSql = "select ''SALESPERSON,''SALES,''RETURN1,''DIFFERENCE1,''SCROLL WHERE 1<>1"
            ''End If
            ''da = New OleDbDataAdapter(strSql, cn)
            ''da.Fill(dtHeader)
            ''gridViewHead.DataSource = dtHeader
            gridViewHead.DataSource = dtMergeHeader
            'funcGridHeaderStyle()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Private Sub frmWastagewiseSales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If AscW(e.KeyChar) = 13 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

   
    Private Sub gridSalesPersonPerform_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        
    End Sub

    Private Sub gridSalesPersonPerform_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.btnNew.Focus()
        End If
    End Sub

    Function funcGridHeaderStyle() As Integer
        With gridViewHead
           
            .Columns("SCROLL").HeaderText = ""
            With .Columns("SALESPERSON")
                'If chkCounterWise.Checked = True Then
                '    .Visible = False
                'Else
                .Visible = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("SALESPERSON").Width
                .HeaderText = " "
                'End If
            End With

            With .Columns("OPENPCS~OPENWEIGHT~OPENSTNWT~OPENAMOUNT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("OPENPCS").Width + gridView.Columns("OPENWEIGHT").Width + gridView.Columns("OPENSTNWT").Width + gridView.Columns("OPENAMOUNT").Width
                .HeaderText = "OPENING"
            End With
            With .Columns("SALEPCS~SALEWEIGHT~SALESTNWT~SALEAMOUNT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("SALEPCS").Width + gridView.Columns("SALEWEIGHT").Width + gridView.Columns("SALESTNWT").Width + gridView.Columns("SALEAMOUNT").Width
                .HeaderText = "SALES"
            End With
            With .Columns("RETURNPCS~RETURNWEIGHT~RETURNSTNWT~RETURNAMOUNT")
                .HeaderText = "RETURN"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("RETURNPCS").Width + gridView.Columns("RETURNWEIGHT").Width + gridView.Columns("RETURNSTNWT").Width + gridView.Columns("RETURNAMOUNT").Width
            End With
            With .Columns("PCS~WEIGHT~STNWT~AMOUNT")
                .HeaderText = "DIFFERENCE"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("PCS").Width + gridView.Columns("WEIGHT").Width + gridView.Columns("STNWT").Width + gridView.Columns("AMOUNT").Width
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Function

    Function funcNew() As Integer
        ' cmbMetal.Text = "ALL"
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridViewHead.DataSource = Nothing
        gridView.DataSource = Nothing
        lblTitle.Text = "TITLE"
        dtpFrom.Select()
    End Function

    Private Sub gridSalesPersonPerform_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub grpControls_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpControls.Enter

    End Sub
End Class
