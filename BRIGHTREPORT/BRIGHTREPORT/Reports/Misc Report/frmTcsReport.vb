Imports System.Data.OleDb
Public Class frmTcsReport
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim objGridShower As frmGridDispDia
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable


    Private Sub frmTcsReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)

        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub frmTcsReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click

        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()

        dtpFrom.Focus()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click


        Dim dtGrid As New DataTable

#Region "commented previous qry"
        ''strSql = " SELECT T.TRANDATE,P.PNAME,TRANNO,AMOUNT,TAXAMOUNT,P.ADDRESS1 AS ADDRESS,P.CITY,P.PINCODE,C.PAN,1 RESULT,TRANNO TTRANNO FROM " & cnStockDb & "..TAXTRAN T"
        ''strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=T.BATCHNO "
        ''strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=C.PSNO"
        ''strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        ''strSql += vbCrLf + " AND T.BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(COMPANYID,'')='" & strCompanyId & "' AND ISNULL(CANCEL,'')='' AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "')"
        ''strSql += vbCrLf + " UNION ALL"
        ''strSql += vbCrLf + " SELECT NULL TRANDATE,NULL PNAME,NULL TRANNO,NULL AMOUNT,NULL TAXAMOUNT,P.ADDRESS2 AS ADDRESS,NULL CITY,NULL PINCODE,NULL PAN,2 RESULT,TRANNO TTRANNO  FROM " & cnStockDb & "..TAXTRAN T"
        ''strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=T.BATCHNO "
        ''strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=C.PSNO"
        ''strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' AND ISNULL(P.ADDRESS2,'')<>'' "
        ''strSql += vbCrLf + " AND T.BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(COMPANYID,'')='" & strCompanyId & "' AND ISNULL(CANCEL,'')='' AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "')"
        ''strSql += vbCrLf + " UNION ALL"
        ''strSql += vbCrLf + " SELECT NULL TRANDATE,NULL PNAME,NULL TRANNO,NULL AMOUNT,NULL TAXAMOUNT,P.AREA  AS ADDRESS,NULL CITY,NULL PINCODE,NULL PAN,3 RESULT,TRANNO TTRANNO  FROM " & cnStockDb & "..TAXTRAN T"
        ''strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=T.BATCHNO "
        ''strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=C.PSNO"
        ''strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' AND ISNULL(P.AREA ,'')<>''"
        ''strSql += vbCrLf + " AND T.BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(COMPANYID,'')='" & strCompanyId & "' AND ISNULL(CANCEL,'')='' AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "')"
        ''strSql += vbCrLf + " UNION ALL"
        ''strSql += vbCrLf + " SELECT NULL TRANDATE,NULL PNAME,NULL TRANNO,NULL AMOUNT,NULL TAXAMOUNT,NULL ADDRESS,NULL CITY,NULL PINCODE,NULL PAN,4 RESULT,TRANNO TTRANNO  FROM " & cnStockDb & "..TAXTRAN T"
        ''strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        ''strSql += vbCrLf + " AND T.BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE ISNULL(COMPANYID,'')='" & strCompanyId & "' AND ISNULL(CANCEL,'')='' AND TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "')"
        ''strSql += vbCrLf + " ORDER BY TTRANNO,RESULT "
#End Region

        strSql = vbCrLf + " SELECT T.TRANDATE,P.PNAME,TRANNO"
        strSql += vbCrLf + " ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ISSUE WHERE TRANDATE=T.TRANDATE AND BATCHNO=T.BATCHNO)AMOUNT"
        strSql += vbCrLf + " ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE=T.TRANDATE AND BATCHNO=T.BATCHNO AND ISNULL(STUDDED,'')<>'Y')TAXAMOUNT"
        strSql += vbCrLf + " ,T.TDSPER TCSPER,T.TDSAMOUNT CALCAMT,T.AMOUNT TCS,P.ADDRESS1 AS ADDRESS,P.CITY,P.PINCODE,C.PAN,1 RESULT,T.TRANDATE TTRANDATE,TRANNO TTRANNO FROM " & cnStockDb & "..ACCTRAN  T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=T.BATCHNO "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=C.PSNO"
        strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND T.ACCODE='TCS'"
        strSql += vbCrLf + " AND ISNULL(T.CANCEL,'')='' "
        strSql += vbCrLf + " AND ISNULL(T.COMPANYID,'')='" & strCompanyId & "'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT DISTINCT NULL TRANDATE,NULL PNAME,NULL TRANNO,NULL AMOUNT,NULL TAXAMOUNT,NULL TCSPER,NULL CALCAMT,NULL TCS"
        strSql += vbCrLf + " ,P.ADDRESS2 AS ADDRESS,NULL CITY,NULL PINCODE,NULL PAN,2 RESULT,T.TRANDATE TTRANDATE,TRANNO TTRANNO  "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=T.BATCHNO "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=C.PSNO"
        strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' AND ISNULL(P.ADDRESS2,'')<>'' "
        strSql += vbCrLf + " AND T.ACCODE='TCS'"
        strSql += vbCrLf + " AND ISNULL(T.CANCEL,'')='' "
        strSql += vbCrLf + " AND ISNULL(T.COMPANYID,'')='" & strCompanyId & "'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT DISTINCT NULL TRANDATE,NULL PNAME,NULL TRANNO,NULL AMOUNT,NULL TAXAMOUNT,NULL TCSPER,NULL CALCAMT,NULL TCS"
        strSql += vbCrLf + " ,P.AREA  AS ADDRESS,NULL CITY,NULL PINCODE,NULL PAN,3 RESULT,T.TRANDATE TTRANDATE,TRANNO TTRANNO  "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=T.BATCHNO "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=C.PSNO"
        strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' AND ISNULL(P.AREA ,'')<>''"
        strSql += vbCrLf + " AND T.ACCODE='TCS'"
        strSql += vbCrLf + " AND ISNULL(T.CANCEL,'')='' "
        strSql += vbCrLf + " AND ISNULL(T.COMPANYID,'')='" & strCompanyId & "'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT DISTINCT NULL TRANDATE,NULL PNAME,NULL TRANNO,NULL AMOUNT,NULL TAXAMOUNT,NULL TCSPER,NULL CALCAMT,NULL TCS"
        strSql += vbCrLf + " ,NULL ADDRESS,NULL CITY,NULL PINCODE,NULL PAN,4 RESULT,T.TRANDATE TTRANDATE,TRANNO TTRANNO "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND T.ACCODE='TCS'"
        strSql += vbCrLf + " AND ISNULL(T.CANCEL,'')='' "
        strSql += vbCrLf + " AND ISNULL(T.COMPANYID,'')='" & strCompanyId & "'"
        strSql += vbCrLf + " ORDER BY TTRANDATE,TTRANNO,RESULT "

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        'dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        'GridViewHeaderCreator(objGridShower.gridViewHeader)
        objGridShower.gridViewHeader.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.RowTemplate.Height = 21
        'AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "TCS DETAIL"
        Dim tit As String = "TCS DETAIL FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        If (chkCmbCompany.Text <> "ALL") And (chkCmbCompany.Text <> "") Then
            tit += vbCrLf & "For " & chkCmbCompany.Text
        End If
        If (chkCmbCostCentre.Text <> "ALL") And (chkCmbCostCentre.Text <> "") Then
            tit += vbCrLf & chkCmbCostCentre.Text
        End If
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        objGridShower.gridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.Show()
        objGridShower.gridViewHeader.Visible = True
        objGridShower.pnlHeader.Size = New Size(objGridShower.pnlHeader.Size.Width, objGridShower.pnlHeader.Size.Height + 10)
        objGridShower.FormReLocation = True
        'FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "PARTICULARS")
        'objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULARS")))
        SetGridHeadColWidth(objGridShower.gridViewHeader)

    End Sub


    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            BrighttechPack.GlobalMethods.FormatGridColumns(dgv)

            .Columns("TRANDATE").Width = 120
            .Columns("TRANDATE").HeaderText = "BILL DATE"
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            .Columns("PNAME").Width = 150
            .Columns("PNAME").HeaderText = "CUSTOMER NAME"
            .Columns("TRANNO").Width = 100
            .Columns("TRANNO").HeaderText = "BILL NO"
            .Columns("AMOUNT").Width = 140
            .Columns("AMOUNT").HeaderText = "BILL VALUE"
            .Columns("TAXAMOUNT").Width = 120
            .Columns("TAXAMOUNT").HeaderText = "TAX VALUE"
            .Columns("TCSPER").Width = 100
            .Columns("TCSPER").HeaderText = "TCS %"
            .Columns("CALCAMT").Width = 120
            .Columns("CALCAMT").HeaderText = "TCS CALC AMT"
            .Columns("TCS").Width = 120
            .Columns("TCS").HeaderText = "TCS VALUE"
            .Columns("ADDRESS").Width = 150
            .Columns("CITY").Width = 150
            .Columns("PINCODE").Width = 100
            .Columns("PAN").Width = 100
            .Columns("PAN").HeaderText = "PAN NO"


            .Columns("TAXAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RESULT").Visible = False
            .Columns("TTRANNO").Visible = False
            .Columns("TTRANDATE").Visible = False
            For CNT As Integer = 10 To .ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
            .Columns("PINCODE").Visible = True
            .Columns("PAN").Visible = True
        End With
    End Sub
End Class
