Imports System.Data.OleDb
Public Class frmCustomerBills
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dsGridView As New DataSet
    Dim ftrStr As String
    Dim dtCostCentre As New DataTable
    Dim dtCostCentre1 As New DataTable
    Dim Specificformat As Boolean = False
    Dim SumDet As String
    Dim CostId As String = Nothing
    Dim dtt As New DataTable
    
    Private Sub frmTotalCustomer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Load PERSONALINFO's Column Names on Search Key on ComboBox

            Dim datatable As New DataTable
            Dim SqlAd As New OleDbDataAdapter
            Dim i As Integer
            SqlAd = New OleDbDataAdapter("SELECT NAME FROM " & cnAdminDb & "..SYSCOLUMNS WHERE ID=(SELECT ID FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE ='U' AND NAME='PERSONALINFO')", cn)
            SqlAd.Fill(datatable)
            For i = 0 To datatable.Rows.Count - 1
                cmbSearchKey.Items.Add(datatable.Rows(i).Item(0).ToString)
            Next

            'Load CostCenter Name on ComboBox
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += " ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(cmbCostCenter, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))

            'Load Company Name on ComboBox
            strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT COMPANYNAME,CONVERT(vARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY"
            strSql += " ORDER BY RESULT,COMPANYNAME"
            dtCostCentre1 = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre1)
            BrighttechPack.GlobalMethods.FillCombo(Chkcombocompany, dtCostCentre1, "COMPANYNAME", , "ALL")

            If strUserCentrailsed <> "Y" And cnDefaultCostId Then cmbCostCenter.Enabled = False
            btnNew_Click(Me, New EventArgs)
            dtpFrom.Focus()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            'If Dgv.DataSource IsNot Nothing Then CType(Dgv.DataSource, DataTable).Rows.Clear()
            If cmbSearchKey.Text = "" Then MsgBox("Search Key Should not Empty", MsgBoxStyle.Information) : Exit Sub
            Dgv.DataSource = Nothing
            Me.Refresh()
            If rbtnSummary.Checked Then
                SumDet = "S"
            ElseIf RbtnMonth.Checked Then
                SumDet = "M"
            Else
                SumDet = "B"
            End If
            


            Dim chkCostId As String = GetQryStringForSp(cmbCostCenter.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", False)
            Dim chkCOMPId As String = GetQryStringForSp(Chkcombocompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", False)
            strSql = "EXEC " & cnAdminDb & "..SP_RPT_NOOFBILLS"
            strSql += vbCrLf + "  @DBNAME='" & cnStockDb & "'"
            strSql += vbCrLf + "  ,@FROMDATE='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
            strSql += vbCrLf + "  ,@TODATE='" & dtpTo.Value.ToString("yyyy/MM/dd") & "'"
            strSql += vbCrLf + "  ,@COSTID='" & chkCostId & "'"
            If SumDet <> "B" Then
                strSql += vbCrLf + "  ,@COMPID='" & chkCOMPId & "'"
            End If
            If SumDet = "M" Then
                strSql += vbCrLf + "  ,@SEARCHKEY ='ACCODE'"
            Else
                strSql += vbCrLf + "  ,@SEARCHKEY ='" & cmbSearchKey.Text & "'"
            End If
            strSql += vbCrLf + "  ,@SEARCHTEXT ='" & txtSearch_txt.Text & "'"
            strSql += vbCrLf + "  ,@SUMDET  ='" & SumDet & "'"
            strSql += vbCrLf + "  ,@SYSTEMID ='" & systemId & "'"
            'If Me.chkSubTotal.Checked = True Then
            '    strSql += vbCrLf + "  ,@SUBTOTAL ='Y'"
            'Else
            '    strSql += vbCrLf + "  ,@SUBTOTAL ='N'"
            'End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If RbtnSummary.Checked Then
                strSql = "SELECT PARTICULAR,NOOFBILLS,AMOUNT,COLHEAD,RESULT FROM TEMPTABLEDB..TEMP" & systemId & "TOTALCUSTOMER  ORDER BY " & cmbSearchKey.Text & ",RESULT"
            ElseIf RbtnMonth.Checked Then
                'strSql = "SELECT PARTICULAR,NOOFBILLS,AMOUNT,COLHEAD,RESULT FROM TEMPTABLEDB..TEMP" & systemId & "TOTALCUSTOMER  ORDER BY MONTH,RESULT"
                strSql = "SELECT * FROM " & cnAdminDb & "..TEMPTEST11_ACCODE ORDER BY RESULT"
            Else
                strSql = "SELECT PARTICULAR,BILLDATE,NAME,AMOUNT,ADDRESS1,ADDRESS2,ADDRESS3,CITY,STATE,COUNTRY,PINCODE,COLHEAD,RESULT FROM TEMPTABLEDB..TEMP" & systemId & "TOTALCUSTOMER  ORDER BY  " & cmbSearchKey.Text & ", RESULT"
            End If
            Dim dtGrid As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da.Fill(dtGrid)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            'dsGridView = New DataSet
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dsGridView)
            If dtGrid.Rows.Count > 0 Then
                Dgv.DataSource = dtGrid
                Dgv.Columns("RESULT").Visible = False
                Dgv.Columns("COLHEAD").Visible = False
                Dgv.Columns("KEYNO").Visible = False
                FillGridGroupStyle_KeyNoWise(Dgv)
                GridViewFormat()
                funcGridViewStyle()
            Else
                Dgv.DataSource = Nothing
                Dgv.Refresh()
                MsgBox("No records found.")
                Exit Sub
                btnNew_Click(btnNew, Nothing)
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
  Function funcGridViewStyle() As Integer
        With Dgv
            'For cnt As Integer = 0 To Dgv.ColumnCount - 1
            '    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            'Next
            'With .Columns("COSTNAME")
            '    .Width = 80
            '    If RbtnSummary.Checked Or RbtnBillno.Checked = True Then
            '        .Visible = False
            '    Else
            '        .Visible = True
            '    End If
            'End With
            'With .Columns("NAME")
            '    .Width = 80
            '    .SortMode = DataGridViewColumnSortMode.NotSortable
            'End With
            If .Columns.Contains("MONTH") Then
                With .Columns("MONTH")
                    .Width = 80
                    If RbtnMonth.Checked = True Then
                        .Visible = False
                    End If
                End With
            End If
            For I As Integer = 0 To Dgv.Rows.Count - 1
                If .Rows(I).Cells("COLHEAD").Value.ToString = "ZZ" Then
                    .Rows(I).DefaultCellStyle = reportTotalStyle
                End If
            Next
            If .Columns.Contains("BILLDATE") Then .Columns("BILLDATE").Visible = False

            If .Columns.Contains("ACNAME") Then .Columns("ACNAME").Width = 200
            If .Columns.Contains("AREA") Then .Columns("AREA").Width = 150
            If .Columns.Contains("BILLNO") Then
                With .Columns("BILLNO")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    '.SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If .Columns.Contains("NOOFBILLS") Then
                With .Columns("NOOFBILLS")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    '.SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            If .Columns.Contains("AMOUNT") Then
                With .Columns("AMOUNT")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    '.SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
        End With

        Dgv.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Function
    Function GridViewFormat() As Integer
        For Each dgvView As DataGridViewRow In Dgv.Rows
            With dgvView

                Select Case .Cells("COLHEAD").Value.ToString
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                    Case "T"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                    Case "G"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                End Select
            End With
        Next
    End Function
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        cmbSearchKey.Text = ""
        txtSearch_txt.Text = String.Empty
        Dgv.DataSource = Nothing
        Dgv.Refresh()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If Dgv.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "NUMBER OF BILLS", Dgv, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub frmTotalCustomer_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If Dgv.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "NUMBER OF BILLS", Dgv, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub frmCustomerBills_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.F3
                btnNew_Click(btnNew, Nothing)
                Exit Select
            Case Keys.F12
                btnExit_Click(btnExit, Nothing)
                Exit Select
        End Select
    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        If Dgv.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
                Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                Dgv.Invalidate()
                For Each dgvCol As DataGridViewColumn In Dgv.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In Dgv.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub SearchToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchToolStripMenuItem.Click
        btnSearch_Click(btnSearch, Nothing)
    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
        btnPrint_Click(btnPrint, Nothing)
    End Sub

    Private Sub ExportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToolStripMenuItem.Click
        btnExport_Click(btnExport, Nothing)
    End Sub

    Private Sub cmbSearchKey_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSearchKey.SelectedIndexChanged
        Me.txtSearch_txt.Text = String.Empty
    End Sub

    Private Sub RbtnMonth_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RbtnMonth.CheckedChanged
        'If RbtnMonth.Checked = True Then
        '    cmbSearchKey.Text = "ACCODE"
        'End If
    End Sub

    Private Sub frmCustomerBills_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LocationChanged

    End Sub

    Private Sub txtSearch_txt_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch_txt.KeyDown
        If e.KeyCode = Keys.Insert Then
            If cmbSearchKey.Text <> "" Then

                strSql = " SELECT ACCODE,ACNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,PINCODE,PHONENO,MOBILE"
                strSql += "  FROM " & cnAdminDb & "..ACHEAD AS H WHERE  ACTYPE IN('C') "
                strSql += GetAcNameQryFilteration()
                strSql += " ORDER BY ACNAME"
                Dim pCode As String = BrighttechPack.SearchDialog.Show("Search Customer Account Code", strSql, cn, 1, , , , , , )
                If pCode <> Nothing Then
                    txtSearch_txt.Clear()
                    txtSearch_txt.Text = pCode
                End If

            Else
                MsgBox("Choose Anyone Field...", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
    End Sub
End Class
