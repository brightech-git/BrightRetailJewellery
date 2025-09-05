Imports System.Data.OleDb
Imports System.Net.Mail
Imports System.IO

Public Class frmGlobalEntryDate
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim objSendMail As New rptSendMail

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
    End Sub

    Private Sub frmGlobalEntryDate_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Main.Global_Date_Rate()
    End Sub

    Private Sub frmGlobalEntryDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim RptMail As Boolean = IIf(GetAdmindbSoftValue("MAILRPT_DAILYTRAN", "N") = "Y", True, False)
        Dim GDate_SingleComp As Boolean = IIf(GetAdmindbSoftValue("GLOBALDATE_SINGLECOMP", "N") = "Y", True, False)
        Dim CHIT_GBDate As String = "N"
        If cnChitCompanyid.ToString <> "" Then
            CHIT_GBDate = objGPack.GetSqlValue(" SELECT CTLTEXT FROM " & cnChitCompanyid & "SAVINGS..SOFTCONTROL  WHERE CTLID='GLOBALDATE'", "CTLTEXT", "N")
        End If
        If GDate_SingleComp Then
            strSql = " SELECT 1 FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='TAB_COMPANY')"
            If Val(objGPack.GetSqlValue(strSql, , "").ToString) > 0 Then
                MessageBox.Show("Global Date Cannot be change. Tab Company found.")
                Exit Sub
            End If
        End If
        strSql = " SELECT TOP 1 SNO FROM " & cnAdminDb & "..TITEMTAG"
        Dim DtTransitCheck As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtTransitCheck)
        If DtTransitCheck.Rows.Count > 0 Then
            If MessageBox.Show("Stock Pending in Transit" + vbCrLf + "Sure you want to change date?", "Date Change Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If
        End If
        strSql = "UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & dtpEntryDate.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " WHERE CTLID = 'GLOBALDATEVAL'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        dtpEntryDate.Focus()
        If CHIT_GBDate = "Y" Then
            strSql = "UPDATE " & cnChitCompanyid & "SAVINGS..SOFTCONTROL SET CTLTEXT = '" & dtpEntryDate.Value.Date.ToString("yyyy-MM-dd") & "',UPDATED='" & GetServerDate() & "'"
            strSql += " WHERE CTLID = 'GLOBALDATEVAL'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        BillNoRegenerator()
        ''LAST DATE RATE UPDATION
        strSql = "  IF NOT (SELECT COUNT(*) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = '" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "')>0"
        strSql += "  BEGIN"
        strSql += "  INSERT INTO " & cnAdminDb & "..RATEMAST(METALID,RDATE,RATEGROUP,PURITY,SRATE,PRATE,USERID,UPDATED,UPTIME)"
        strSql += "  SELECT METALID,'" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "',(SELECT ISNULL(MAX(RATEGROUP),0)+1 FROM " & cnAdminDb & "..RATEMAST) AS RATEGROUP,PURITY,SRATE,PRATE,USERID,UPDATED,UPTIME "
        strSql += "  FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = (SELECT MAX(RDATE) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE <= '" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "' AND  RDATE BETWEEN '" & cnTranFromDate.ToString("yyyy-MM-dd") & "' AND '" & cnTranToDate.ToString("yyyy-MM-dd") & "')"
        strSql += "  AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = (SELECT MAX(RDATE) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE <= '" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "' AND RDATE BETWEEN '" & cnTranFromDate.ToString("yyyy-MM-dd") & "' AND '" & cnTranToDate.ToString("yyyy-MM-dd") & "'))"
        strSql += "  END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If RptMail Then
            'objSendMail.rptDate = dtpEntryDate.Value.AddDays(-1).Date
            'objSendMail.funcReportDailyTran()
        End If
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        dtpEntryDate.Focus()
        Me.Close()
    End Sub

    Private Sub frmGlobalEntryDate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtpEntryDate.Value = GetEntryDate(GetServerDate)
    End Sub


End Class