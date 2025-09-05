Imports System.Data.OleDb
Public Class frmCustomerDashboardDetail

#Region " Variable"
    Dim strsql As String = ""
    Dim da As New OleDbDataAdapter
    Dim dt As New DataTable
    Dim cmd As OleDbCommand
    Dim dtDetail As New DataTable
    Dim dtInvoice As New DataTable
    Dim dtCustomerFeed As New DataTable
    Dim dtScheme As New DataTable
    Dim dtOutstanding As New DataTable
#End Region

#Region " Form Load"
    Private Sub frmCustomerDashboardDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub frmCustomerDashboardDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{tab}")
        End If
    End Sub
#End Region

#Region " Lable Link "
    Private Sub LinkLPrint1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLPrint1.LinkClicked
        If grid1_Own.Rows.Count > 0 Then
            If grid1_Own.Rows.Count > 0 Then
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblInvoiceDetail.Text, grid1_Own, BrightPosting.GExport.GExportType.Print)
            End If
        End If
    End Sub

    Private Sub LinkLPrint2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLPrint2.LinkClicked
        If grid2_Own.Rows.Count > 0 Then
            If grid2_Own.Rows.Count > 0 Then
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblSchemeDetail.Text, grid2_Own, BrightPosting.GExport.GExportType.Print)
            End If
        End If
    End Sub

    Private Sub LinkLPrint3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLPrint3.LinkClicked
        If grid3_Own.Rows.Count > 0 Then
            If grid3_Own.Rows.Count > 0 Then
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblCustomerFeedback.Text, grid3_Own, BrightPosting.GExport.GExportType.Print)
            End If
        End If
    End Sub

    Private Sub LinkLPrint4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLPrint4.LinkClicked
        If grid4_Own.Rows.Count > 0 Then
            If grid4_Own.Rows.Count > 0 Then
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblOutstanding.Text, grid4_Own, BrightPosting.GExport.GExportType.Print)
            End If
        End If
    End Sub
#End Region

#Region " User Defined Function"
    Private Sub funGridClear()
        lblCustName.Text = "CUSTOMER NAME"
        gridView_Own.DataSource = Nothing
        grid1_Own.DataSource = Nothing
        gridT1_Own.DataSource = Nothing
        grid2_Own.DataSource = Nothing
        gridT2_Own.DataSource = Nothing
        grid3_Own.DataSource = Nothing
        gridT3_Own.DataSource = Nothing
        grid4_Own.DataSource = Nothing
        gridT4_OWN.DataSource = Nothing
        dtDetail = New DataTable
        dtInvoice = New DataTable
        dtCustomerFeed = New DataTable
        dtScheme = New DataTable
        dtOutstanding = New DataTable
    End Sub
    Function GetTable() As DataTable
        Dim table As New DataTable
        ' Create 3 typed columns in the DataTable.
        table.Columns.Add("PARTICULAR", GetType(String))
        table.Columns.Add("DETAIL", GetType(String))
        Return table
    End Function



#End Region

#Region "Button Events"
    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Try
            btnGo.Enabled = False
            Me.Cursor = Cursors.WaitCursor
            funGridClear()
            If txtCusMobileNo.Text.Trim <> "" Then
                Dim ds As New DataSet
                strsql = " EXEC " & cnAdminDb & "..[WEB_CUST_MOBILEWISE] @MOBILENO = '" & txtCusMobileNo.Text.Trim & "' "
                cmd = New OleDbCommand(strsql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(ds)
                dt = New DataTable
                dt = ds.Tables(0)
                If dt.Rows.Count > 0 Then

                    Dim dv As New DataView(dt)
                    Dim dtHead As New DataTable
                    dv.RowFilter = "PAYTYPE = 'SALES'"
                    dtHead = dv.ToTable

                    Dim dtSale As New DataTable
                    dtSale = dtHead.Copy

                    dv = New DataView(dt)
                    Dim dtScheme As New DataTable
                    dv.RowFilter = "TYPE = 'CHIT'"
                    dtScheme = dv.ToTable


                    dv = New DataView(dt)
                    Dim dtCustomerFeedback As New DataTable
                    dv.RowFilter = "PAYTYPE = ''"
                    dtCustomerFeedback = dv.ToTable

                    dv = New DataView(dt)
                    Dim dtOutstanding As New DataTable
                    dv.RowFilter = "PAYTYPE = 'OUTSTANDING'"
                    dtOutstanding = dv.ToTable


                    If dtHead.Rows.Count > 0 Then
                        lblCustName.Text = dtHead.Rows(0).Item("PNAME").ToString.Trim
                    Else
                        lblCustName.Text = "NO NAME"
                    End If
                    Dim dr As DataRow = Nothing
                    If dtHead.Rows.Count > 0 Then
                        dtDetail = GetTable()
                        dr = dtDetail.NewRow
                        dr!PARTICULAR = "Mobile"
                        dr!DETAIL = dtHead.Rows(0).Item("MOBILE").ToString
                        dtDetail.Rows.Add(dr)

                        Dim dtTNI As New DataTable
                        dtTNI = dtHead.DefaultView.ToTable(True, "TRANNO")
                        dr = dtDetail.NewRow
                        dr!PARTICULAR = "Total No. Invoices"
                        dr!DETAIL = dtTNI.Rows.Count
                        dtDetail.Rows.Add(dr)

                        dr = dtDetail.NewRow
                        dr!PARTICULAR = "Total No. Live Schemes"
                        dr!DETAIL = "0"
                        dtDetail.Rows.Add(dr)

                        dr = dtDetail.NewRow
                        dr!PARTICULAR = "Date Of First Entry"
                        dr!DETAIL = Format(dtHead.Compute("MIN(TDATE)", ""), "dd-MM-yyyy")
                        dtDetail.Rows.Add(dr)

                        dr = dtDetail.NewRow
                        dr!PARTICULAR = "Last visit"
                        dr!DETAIL = Format(dtHead.Compute("MAX(TDATE)", ""), "dd-MM-yyyy")
                        dtDetail.Rows.Add(dr)

                        dtTNI = New DataTable
                        dtTNI = dtHead.DefaultView.ToTable(True, "TRANDATE")
                        dr = dtDetail.NewRow
                        dr!PARTICULAR = "Total No. Of Visits"
                        dr!DETAIL = dtTNI.Rows.Count
                        dtDetail.Rows.Add(dr)

                        strsql = " SELECT DATEDIFF(D,'" & Format(dtHead.Compute("MAX(TDATE)", ""), "yyyy-MM-dd") & "',GETDATE()) DATES"
                        Dim dd As Integer = Val(GetSqlValue(cn, strsql).ToString)
                        dr = dtDetail.NewRow
                        dr!PARTICULAR = "Last Visited Before [in Days]"
                        dr!DETAIL = dd

                        dtDetail.Rows.Add(dr)
                        With gridView_Own
                            .DataSource = Nothing
                            .DataSource = dtDetail
                            .Columns("PARTICULAR").HeaderText = ""
                            .Columns("PARTICULAR").DefaultCellStyle.WrapMode = DataGridViewTriState.True
                            .Columns("PARTICULAR").Width = 180
                            .Columns("DETAIL").HeaderText = ""
                            .Columns("DETAIL").DefaultCellStyle.WrapMode = DataGridViewTriState.True
                            .Columns("DETAIL").Width = 150
                            For i As Integer = 0 To .Rows.Count - 1
                                .Rows(i).Height = 75
                                .Rows(i).Cells(0).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                .Rows(i).Cells(0).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                                .Rows(i).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                .Rows(i).Cells(1).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                            Next
                        End With

                        With grid1_Own
                            .DataSource = Nothing
                            .DataSource = dtSale
                            FormatGridColumns(grid1_Own, False, True, True, True)
                            For c As Integer = 0 To .Columns.Count - 1
                                .Columns(c).Visible = False
                            Next
                            .Columns("TRANDATE").Visible = True
                            .Columns("TRANDATE").HeaderText = "DATE"
                            .Columns("TRANDATE").Width = lblIDate.Width

                            .Columns("TRANNO").Visible = True
                            .Columns("TRANNO").HeaderText = "TRANNO"
                            .Columns("TRANNO").Width = lblITraNo.Width

                            .Columns("GRSWT").Visible = True
                            .Columns("GRSWT").HeaderText = "GRSWT"
                            .Columns("GRSWT").Width = lblIGrswt.Width

                            .Columns("NETWT").Visible = True
                            .Columns("NETWT").HeaderText = "NETWT"
                            .Columns("NETWT").Width = lblINetwt.Width

                            .Columns("AMOUNT").Visible = True
                            .Columns("AMOUNT").HeaderText = "AMOUNT"
                            .Columns("AMOUNT").Width = lblIAmount.Width

                        End With

                        With grid2_Own
                            .DataSource = Nothing
                            .DataSource = dtScheme
                            FormatGridColumns(grid2_Own, False, True, True, True)
                            For c As Integer = 0 To .Columns.Count - 1
                                .Columns(c).Visible = False
                            Next
                            .Columns("GROUPCODE").Visible = True
                            .Columns("GROUPCODE").HeaderText = "GROUPCODE"
                            .Columns("GROUPCODE").Width = lblSGrpcode.Width

                            .Columns("REGNO").Visible = True
                            .Columns("REGNO").HeaderText = "REGNO"
                            .Columns("REGNO").Width = lblSRegno.Width

                            .Columns("AMOUNT").Visible = True
                            .Columns("AMOUNT").HeaderText = "AMOUNT"
                            .Columns("AMOUNT").Width = lblSAmount.Width

                            .Columns("TRANDATE").Visible = True
                            .Columns("TRANDATE").HeaderText = "JOINDATE"
                            .Columns("TRANDATE").Width = lblSJoinDate.Width

                            .Columns("DOCLOSE").Visible = True
                            .Columns("DOCLOSE").HeaderText = "CLOSED"
                            .Columns("DOCLOSE").Width = lblSDoclose.Width

                        End With
                        With grid3_Own
                            .DataSource = Nothing
                            .DataSource = dtCustomerFeed
                            FormatGridColumns(grid3_Own, False, True, True, True)
                            For c As Integer = 0 To .Columns.Count - 1
                                .Columns(c).Visible = False
                            Next
                        End With

                        With grid4_Own
                            .DataSource = Nothing
                            .DataSource = dtOutstanding
                            FormatGridColumns(grid4_Own, False, True, True, True)
                            For c As Integer = 0 To .Columns.Count - 1
                                .Columns(c).Visible = False
                            Next
                        End With

                    End If
                Else
                    MsgBox("No Record found", MsgBoxStyle.Information)
                    txtCusMobileNo.Focus()
                    txtCusMobileNo.SelectAll()
                End If
            Else
                MsgBox("Enter the mobileNo.", MsgBoxStyle.Information)
                txtCusMobileNo.Focus()
                txtCusMobileNo.SelectAll()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        Finally
            txtCusMobileNo.Focus()
            txtCusMobileNo.SelectAll()
            btnGo.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub txtCusMobileNo_GotFocus(sender As Object, e As EventArgs) Handles txtCusMobileNo.GotFocus
        txtCusMobileNo.BackColor = Color.LightPink
    End Sub

    Private Sub txtCusMobileNo_LostFocus(sender As Object, e As EventArgs) Handles txtCusMobileNo.LostFocus
        txtCusMobileNo.BackColor = Color.White
    End Sub
#End Region
End Class