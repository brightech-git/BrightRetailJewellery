Imports System.Data.OleDb
Public Class WCustomerOutstanding
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Private Sub WCustomerOutstanding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub WCustomerOutstanding_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpTo.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate
        dtpTo.MaximumDate = (New DateTimePicker).MaxDate
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        lblTitle.Text = "CUSTOMER OUTSTANDING"
        gridView.DataSource = Nothing
        Prop_Gets()
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        gridView.DataSource = Nothing
        lblTitle.Text = "CUSTOMER OUTSTANDING FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        Me.Refresh()
        If rbtDetailed.Checked Then
            StrSql = " IF (SELECT TOP 1 1 FROM MASTER..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "OUTSTDT')>0 DROP TABLE MASTER..TEMP" & systemId & "OUTSTDT"
            StrSql += vbCrLf + " SELECT "
            StrSql += vbCrLf + " H.ACNAME "
            StrSql += vbCrLf + " ,O.TRANDATE,O.TRANNO,SUBSTRING(O.RUNNO,6,20)RUNNO"
            StrSql += vbCrLf + " ,CASE WHEN RECPAY = 'P' THEN O.PUREWT ELSE 0 END AS ISSUE"
            StrSql += vbCrLf + " ,CASE WHEN RECPAY = 'R' THEN O.PUREWT ELSE 0 END AS RECEIPT"
            StrSql += vbCrLf + " ,CASE WHEN RECPAY = 'P' THEN O.AMOUNT ELSE 0 END AS DEBIT"
            StrSql += vbCrLf + " ,CASE WHEN RECPAY = 'R' THEN O.AMOUNT ELSE 0 END AS CREDIT"
            StrSql += vbCrLf + " INTO MASTER..TEMP" & systemId & "OUTSTDT"
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O"
            StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AS H ON H.ACCODE = O.ACCODE"
            StrSql += vbCrLf + " WHERE "
            StrSql += vbCrLf + " O.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND ISNULL(O.ACCODE,'') <> ''"
            StrSql += vbCrLf + " AND ISNULL(O.CANCEL,'') = ''"
            StrSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
            StrSql = " UPDATE TEMP" & systemId & "OUTSTDT"
            StrSql += " SET ISSUE = CASE WHEN ISSUE <> 0 THEN ISSUE ELSE NULL END"
            StrSql += " ,RECEIPT = CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END"
            StrSql += " ,DEBIT = CASE WHEN DEBIT <> 0 THEN DEBIT ELSE NULL END"
            StrSql += " ,CREDIT = CASE WHEN CREDIT <> 0 THEN CREDIT ELSE NULL END"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        Else
            StrSql = " IF (SELECT TOP 1 1 FROM MASTER..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "OUTSTDT')>0 DROP TABLE MASTER..TEMP" & systemId & "OUTSTDT"
            StrSql += " SELECT ACNAME"
            StrSql += " ,SUM(CASE WHEN RECPAY = 'P' THEN PUREWT ELSE -1*PUREWT END) AS WEIGHT"
            StrSql += " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) AS AMOUNT"
            StrSql += vbCrLf + " INTO MASTER..TEMP" & systemId & "OUTSTDT"
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O"
            StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AS H ON H.ACCODE = O.ACCODE"
            StrSql += vbCrLf + " WHERE "
            StrSql += vbCrLf + " O.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND ISNULL(O.ACCODE,'') <> ''"
            StrSql += vbCrLf + " AND ISNULL(O.CANCEL,'') = ''"
            StrSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
            StrSql += " GROUP BY ACNAME"
            StrSql += " HAVING (SUM(CASE WHEN RECPAY = 'P' THEN PUREWT ELSE -1*PUREWT END) <>0  OR"
            StrSql += " SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END) <> 0)"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
            StrSql = " UPDATE TEMP" & systemId & "OUTSTDT"
            StrSql += " SET WEIGHT = CASE WHEN WEIGHT <> 0 THEN WEIGHT ELSE NULL END"
            StrSql += " ,AMOUNT = CASE WHEN AMOUNT <> 0 THEN AMOUNT ELSE NULL END"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        StrSql = " SELECT * FROM TEMP" & systemId & "OUTSTDT ORDER BY ACNAME"
        Dim dtGrid As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpFrom.Focus()
            Exit Sub
        End If
        gridView.DataSource = dtGrid
        If gridView.Columns.Contains("TRANDATE") Then gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView)
        gridView.Focus()
        Prop_Sets()
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New WCustomerOutstanding_Properties
        GetSettingsObj(obj, Me.Name, GetType(WCustomerOutstanding_Properties))
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtSummary.Checked = obj.p_rbtSummary
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New WCustomerOutstanding_Properties
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_rbtSummary = rbtSummary.Checked
        SetSettingsObj(obj, Me.Name, GetType(WCustomerOutstanding_Properties))
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, cnCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, cnCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
End Class
Public Class WCustomerOutstanding_Properties
    Private rbtDetailed As Boolean = True
    Public Property p_rbtDetailed() As Boolean
        Get
            Return rbtDetailed
        End Get
        Set(ByVal value As Boolean)
            rbtDetailed = value
        End Set
    End Property
    Private rbtSummary As Boolean = False
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
End Class