Imports System.Data.OleDb
Public Class frmCustomerDateWise
#Region "Variable"
    Dim strsql As String
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim cmd As OleDbCommand
#End Region

#Region "Form Events"

    Private Sub frmCustomerReport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmCustomerReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New System.EventArgs)
    End Sub
#End Region

#Region "Button Events"

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If GridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Customer Report", GridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If GridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Customer Report", GridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        GridView.DataSource = Nothing
        txtCustomer.Text = ""
        strsql = "SELECT COLUMN_NAME AS NAME FROM " & cnAdminDb & ".INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'PERSONALINFO' "
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cmbFilter.DataSource = Nothing
            cmbFilter.DataSource = dt
            cmbFilter.DisplayMember = "NAME"
        End If
        cmbFilter.Text = "MOBILE"
    End Sub

    Private Sub AutoSize_Gridview()
        If GridView.RowCount > 0 Then
            If True Then
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                GridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In GridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In GridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        GridView.DataSource = Nothing
        strsql = vbCrLf + " SELECT "
        If chkLostCustomer.Checked = True Then
            strsql += vbCrLf + " DISTINCT "
        End If
        If chkOldCustomer.Checked = True Or chkCustVisit.Checked = True Then strsql += vbCrLf + " COUNT(SNO) [Visit], "
        strsql += vbCrLf + " " & colheadline() & ""
        strsql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS A"
        If chkNewCustomer.Checked = True Or chkLostCustomer.Checked = True Then
            strsql += vbCrLf + " WHERE "
        ElseIf chkOldCustomer.Checked = True Or chkCustVisit.Checked = True Then
            strsql += vbCrLf + " ," & cnAdminDb & "..CUSTOMERINFO AS B WHERE A.SNO = B.PSNO AND "
        Else
            strsql += vbCrLf + " WHERE "
        End If
        strsql += vbCrLf + " A.TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "'"
        If txtCustomer.Text <> "" Then strsql += vbCrLf + " AND A." & cmbFilter.Text & " LIKE '%" & txtCustomer.Text & "%'"
        If chkNewCustomer.Checked = True Then
            strsql += vbCrLf + " AND SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO B WHERE A.SNO = B.PSNO "
            strsql += vbCrLf + " AND BATCHNO IN (SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE "
            strsql += vbCrLf + " TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'')<>'Y')"
            strsql += vbCrLf + " GROUP BY B.PSNO HAVING COUNT(B.PSNO) = 1)"
        End If
        If chkLostCustomer.Checked = True Then
            strsql += vbCrLf + " ORDER BY A.TRANDATE "
        ElseIf chkOldCustomer.Checked = True Or chkCustVisit.Checked = True Then
            strsql += vbCrLf + " GROUP BY " & colheadline() & " "
            strsql += vbCrLf + " HAVING COUNT(SNO) > 1"
        End If
        da = New OleDbDataAdapter(strsql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With GridView
                .DataSource = Nothing
                .DataSource = dt
                FormatGridColumns(GridView, False, False, True, False)
                AutoSize_Gridview()
            End With
        Else
            MsgBox("No Record found", MsgBoxStyle.Information)
            GridView.DataSource = Nothing
            Exit Sub
        End If

    End Sub

    Private Function colheadline() As String
        Dim Qry As String
        If chkCustVisit.Checked = True Then
            Qry = ""
        Else
            Qry = ""
        End If
        Qry += " TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3"
        Qry += " ,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES"
        Qry += " ,MOBILE,EMAIL,FAX,A.PAN,IDTYPE,IDNO,SNO "
        Return Qry
    End Function
#End Region

#Region "Checkbox Events"
    Private Sub chkNewCustomer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNewCustomer.CheckedChanged
        If chkNewCustomer.Checked = True Then
            chkOldCustomer.Checked = False
            chkLostCustomer.Checked = False
            chkCustVisit.Checked = False
        End If
    End Sub

    Private Sub chkOldCustomer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOldCustomer.CheckedChanged
        If chkOldCustomer.Checked = True Then
            chkNewCustomer.Checked = False
            chkLostCustomer.Checked = False
            chkCustVisit.Checked = False
        End If
    End Sub

    Private Sub chkLostCustomer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLostCustomer.CheckedChanged
        If chkLostCustomer.Checked = True Then
            chkNewCustomer.Checked = False
            chkOldCustomer.Checked = False
            chkCustVisit.Checked = False
        End If
    End Sub
    Private Sub chkCustVisit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCustVisit.CheckedChanged
        If chkCustVisit.Checked = True Then
            chkNewCustomer.Checked = False
            chkOldCustomer.Checked = False
            chkLostCustomer.Checked = False
        End If
    End Sub
#End Region

    
End Class