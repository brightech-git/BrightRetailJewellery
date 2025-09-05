Imports System.Data.OleDb
Imports System.IO
Public Class frmBounzUpload
    Dim strSql As String
    Public objSoftKeys As New SoftKeys
    Dim dtSoftKeys As DataTable = GetAdmindbSoftValueAll()
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim Transact As OleDbTransaction
    Dim BOUNZ_SALES As Boolean = IIf(GetAdmindbSoftValue("BOUNZ_SALES", "N") = "Y", True, False)
    Dim BOUNZTRAN_URL As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZTRAN_URL", "")
    Dim BOUNZ_PUBLICKEY As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZ_PUBLICKEY", "")
    Dim BOUNZ_USERNAME As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZ_USERNAME", "")
    Dim BOUNZ_PASSWORD As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZ_PASSWORD", "")
    Dim BOUNZ_PARTNERID As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZ_PARTNERID", "")
    Dim BOUNZ_STOREID As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZ_STOREID", "")
    Dim BOUNZ_COUNTRYCODE As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "BOUNZ_COUNTRYCODE", "")

    Private Sub frmBounzUpload_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If BOUNZ_SALES = False Then
            MsgBox("SET BOUNZ_SALES = Y", MsgBoxStyle.Information)
            Me.Close()
        End If
        gridView.DataSource = Nothing
        chkSelectAll.Checked = True
        strSql = vbCrLf + " SELECT P.MOBILE, PT.PREVILEGEID,P.PNAME,PT.TRANNO,CONVERT(VARCHAR(12),PT.TRANDATE,103) TRANDATE"
        strSql += vbCrLf + " ,PT.TRANTYPE,PT.BATCHNO FROM " & cnAdminDb & "..PRIVILEGETRAN PT "
        strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ISSUE I ON I.BATCHNO=PT.BATCHNO AND I.TRANDATE=PT.TRANDATE AND I.TRANNO=PT.TRANNO "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=PT.BATCHNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=C.PSNO"
        strSql += vbCrLf + " WHERE I.TRANDATE<= GETDATE() -15 AND I.TRANTYPE='SA'"
        strSql += vbCrLf + " AND ISNULL(PT.REFNO,'')=''"
        strSql += vbCrLf + " AND ISNULL(PT.ENTRYTYPE,'')='B'"
        Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
        Dim dtGrid As New DataTable
        dtCol.DefaultValue = True
        dtGrid.Columns.Add(dtCol)
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.AcceptChanges()
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            gridView.DataSource = Nothing
            chkSelectAll.Checked = False
            Exit Sub
        End If
        gridView.DataSource = dtGrid
        FormatGridColumns(gridView, False, False)
        gridView.Columns("CHECK").ReadOnly = False
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged
        If chkSelectAll.Checked And gridView.Rows.Count > 0 Then
            For i As Integer = 0 To gridView.Rows.Count - 1
                gridView.Rows(i).Cells("CHECK").Value = True
            Next
        ElseIf chkSelectAll.Checked = False And gridView.Rows.Count > 0 Then
            For i As Integer = 0 To gridView.Rows.Count - 1
                gridView.Rows(i).Cells("CHECK").Value = False
            Next
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim dt As New DataTable
        If gridView.DataSource Is Nothing Then
            MsgBox("There is no selected record", MsgBoxStyle.Information)
            Exit Sub
        End If
        dt = CType(gridView.DataSource, DataTable).Copy
        Dim dv As DataView
        dv = dt.DefaultView
        dv.RowFilter = "CHECK = True"
        If Not dv.ToTable.Rows.Count > 0 Then
            MsgBox("There is no selected record", MsgBoxStyle.Information)
            Exit Sub
        End If

        If BOUNZTRAN_URL.ToString = "" Then
            MsgBox("BOUNZ TRANACTION URL Should not be empty")
            Exit Sub
        End If

        If BOUNZ_PUBLICKEY.ToString = "" Then
            MsgBox("BOUNZ PUBLICKEY Should not be empty")
            Exit Sub
        End If
        If BOUNZ_USERNAME.ToString = "" Then
            MsgBox("BOUNZ UserName Should not be empty")
            Exit Sub
        End If
        If BOUNZ_PASSWORD.ToString = "" Then
            MsgBox("BOUNZ Password Should not be empty")
            Exit Sub
        End If
        Try
            If BOUNZ_SALES Then
                tran = Nothing
                tran = cn.BeginTransaction
                For Each ro As DataRow In dv.ToTable.Rows
                    strSql = GenUpdateQry(ro)
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
                tran.Commit()
                tran = Nothing
            End If
            MsgBox("Transaction Updated Successfully", MsgBoxStyle.Information)
            frmBounzUpload_Load(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Function GenUpdateQry(ByVal ro As DataRow) As String
        Dim qry As String = Nothing
        Dim dtItems As New DataTable
        strSql = " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR,CASE WHEN A.TRANTYPE='SA' THEN (CASE WHEN ISNULL(A.COSTID,'')='' THEN A.COMPANYID ELSE A.COSTID END) + '/SA/'  + CONVERT(VARCHAR,TRANNO) WHEN (A.TRANTYPE='SR' OR A.TRANTYPE='OD' OR A.TRANTYPE='RD') "
        strSql += vbCrLf + " THEN  (CASE WHEN ISNULL(A.COSTID,'')='' THEN A.COMPANYID ELSE A.COSTID END) + '/'+ A.TRANTYPE +'/'  + CONVERT(VARCHAR,TRANNO)   ELSE REFNO END) bill_number"
        'strSql += vbCrLf + " ,REPLACE(CONVERT(VARCHAR(12),A.TRANDATE,105),'-','/') bill_date"
        strSql += vbCrLf + " ,REPLACE(CONVERT(VARCHAR(12),A.TRANDATE,111),'/','-') + A.UPTIME bill_date"
        strSql += vbCrLf + " ,'" & BOUNZ_STOREID & "' store_code" 'aeartt
        strSql += vbCrLf + " ,'" & BOUNZ_PARTNERID & "' partner_code" 'hgsf
        strSql += vbCrLf + " ,P.PREVILEGEID loyalty_id"
        strSql += vbCrLf + " ,P.MOBILE mobile_number"
        strSql += vbCrLf + " ,'" & BOUNZ_COUNTRYCODE & "' country_code" '971
        strSql += vbCrLf + " ,ISNULL(AMOUNT,0)+ISNULL(TAX,0)"
        strSql += vbCrLf + " -ISNULL((SELECT AMOUNT FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & ro.Item("BATCHNO").ToString & "' AND ACCODE IN "
        strSql += vbCrLf + " (SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' AND NAME LIKE 'BOUNZ%')),0) total_bill_amount"
        strSql += vbCrLf + " ,ISNULL(DISCOUNT,0)total_discount"
        strSql += vbCrLf + " ,ISNULL(TAX,0)tax_amount"
        strSql += vbCrLf + " ,'' charity_amount" '300
        strSql += vbCrLf + " ,'0'tender_type" '300
        strSql += vbCrLf + " ,CASE WHEN A.TRANTYPE='SA' THEN 'SALES'ELSE CASE WHEN A.TRANTYPE='SR' THEN 'RETURN' ELSE '' END END transaction_type"
        strSql += vbCrLf + " ,CASE WHEN P.GSTNO='' THEN 'B2C' ELSE 'B2B' END customer_type"
        strSql += vbCrLf + " ,'' excise_duty" '12
        strSql += vbCrLf + " ,'ADD' action"
        strSql += vbCrLf + " ,C.CATNAME category_code"
        strSql += vbCrLf + " ,'' tentative_date" '2020-03-10
        strSql += vbCrLf + " ,'AED' currency"
        strSql += vbCrLf + " ,'POS' channel_code"
        strSql += vbCrLf + " ,'100' til_no" '2T
        strSql += vbCrLf + " ,A.TRANNO e_id FROM " & cnStockDb & "..ISSUE A"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS B ON A.ITEMID=B.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE=B.CATCODE"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CUSTOMERINFO CI ON CI.BATCHNO=A.BATCHNO "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=CI.PSNO "
        strSql += vbCrLf + " WHERE A.BATCHNO='" & ro.Item("BATCHNO").ToString & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        Dim reader As OleDbDataReader = cmd.ExecuteReader
        dtItems.Load(reader)
        If dtItems.Rows.Count > 0 And dtItems.Rows(0).Item("transaction_type").ToString <> "" Then
            Dim crd As New CallApi.BounzInv.BOUNZ_TRANACTION
            crd.USERNAME = BOUNZ_USERNAME
            crd.PASSWORD = BOUNZ_PASSWORD
            crd.PublicKey = BOUNZ_PUBLICKEY

            Dim _api As New CallApi.PushData
            Dim cls As New CallApi.BounzInv.Paralist
            cls.transactions = CallApi.B2BInv.ConvertDataTable(Of CallApi.BounzInv.transactions)(dtItems)
            _api.apiurl = BOUNZTRAN_URL
            _api.tpkey = BOUNZ_PUBLICKEY
            _api.username = BOUNZ_USERNAME
            _api.password = BOUNZ_PASSWORD
            Dim res As CallApi.BounzInv.BOUNZRESULT = _api.CallapiBounz(Newtonsoft.Json.JsonConvert.SerializeObject(cls))
            If res.status Then
                qry = "UPDATE " & cnAdminDb & "..PRIVILEGETRAN SET REFNO='" & res.values.batch_id & "'  WHERE BATCHNO='" & ro.Item("BATCHNO").ToString & "'"
            Else
                qry = "UPDATE " & cnAdminDb & "..PRIVILEGETRAN SET REFNO='FAILED' WHERE BATCHNO='" & ro.Item("BATCHNO").ToString & "'"
                MessageBox.Show(res.status_code.ToString + vbCrLf + res.message, "BOUNZ")
            End If
        End If
        Return qry
    End Function
    Private Sub frmBounzUpload_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub


End Class