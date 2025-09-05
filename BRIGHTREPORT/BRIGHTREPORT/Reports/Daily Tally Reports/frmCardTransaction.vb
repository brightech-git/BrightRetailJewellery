Imports System.Data.OleDb
Public Class frmCardTransaction
    Dim strsql As String = ""
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dtc As New DataTable
    Dim dscard As New DataSet
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", "", gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        'If Not txtcommission.Text <> "" Then
        '    MsgBox("Commission Should Not Empty ", MsgBoxStyle.Information)
        '    txtcommission.Focus()
        'ElseIf Not txtservicetax.Text <> "" Then
        '    MsgBox("ServiceTax Should Not Empty ", MsgBoxStyle.Information)
        '    txtservicetax.Focus()
        'Else
        '    Report()
        '    gridView.Focus()
        'End If
        Report()
        gridView.Focus()
    End Sub
    Private Sub Report()
        strsql = " EXEC " & cnAdminDb & "..RPT_CARDTRAN"
        strsql += vbCrLf + "@DBNAME='" & cnStockDb & "'"
        strsql += vbCrLf + ",@FROMDATE ='" & Format(dtpFrom.Value, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + ",@TODATE ='" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
        strsql += vbCrLf + ",@COMMISSION='" & Val(txtcommission.Text.ToString) & "'"
        strsql += vbCrLf + ",@SERVICETAX ='" & Val(txtservicetax.Text.ToString) & "'"
        strsql += vbCrLf + ",@CARDNAME='" & GetQryString(chkCmbBankName.Text).Replace("'", "") & "'"
        Dim tes As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        dscard = New DataSet
        da.Fill(dscard)
        tes = dscard.Tables(0)
        If tes.Rows.Count > 1 Then
            With gridView
                .DataSource = tes
                For i As Integer = 0 To gridView.Rows.Count - 1
                    Select Case gridView.Rows(i).Cells("TRANDATE").Value
                        Case "TOTAL"
                            gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightYellow
                            gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Black
                            gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End Select
                Next

                With .Columns("TRANDATE")
                    .HeaderText = "DATE"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("PNAME")
                    .HeaderText = "NAME"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("MOBILE")
                    .HeaderText = "MOBILE"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("PAYMODE")
                    .HeaderText = "PAYMODE"
                    .Width = 80
                    .Visible = False
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("CARDNAME")
                    .HeaderText = "DETAILS"
                    .Width = 200
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("TRANNO")
                    .HeaderText = "BILLNO"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("AMOUNT")
                    .HeaderText = "AMOUNT"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("COMMISSION")
                    .HeaderText = "COMMISSION %"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("SERVICETAX")
                    .HeaderText = "SERVICETAX %"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("NETAMOUNT")
                    .HeaderText = "NETAMOUNT"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("result")
                    .HeaderText = "result"
                    .Width = 120
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Visible = False
                End With
               
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .Columns("AMOUNT").DefaultCellStyle.Format = "###0.00"
                .Columns("NETAMOUNT").DefaultCellStyle.Format = "###0.00"
                .Columns("SERVICETAX").DefaultCellStyle.Format = "###0.00"
                .Columns("COMMISSION").DefaultCellStyle.Format = "###0.00"
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End With
        Else
            MsgBox("Record Not Found", MsgBoxStyle.Information)
        End If
    End Sub
    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        chkCmbBankName.Text = ""
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        dtpFrom.Focus()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbBankName, dtc, "NAME", , "ALL")
        gridView.DataSource = Nothing
    End Sub
    Private Sub frmCardTransaction_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExport_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub
  
    Private Sub frmCardTransaction_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strsql = "SELECT 'ALL' NAME" & vbCrLf
        strsql += "UNION ALL" & vbCrLf
        strsql += "SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='R' "
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtc)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbBankName, dtc, "NAME", , "ALL")

    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", "", gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
   
    Private Sub ExitToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub
End Class