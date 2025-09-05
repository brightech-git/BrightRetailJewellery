Public Class Wishes
    Dim StrSql As String
    Dim BirthDayTemplate As String
    Dim AnniversaryTemplate As String

    Private Sub Wishes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        chkAll.Checked = False

        dtpFrom_OWN.Value = Now.Date
        dtpTo_OWN.Value = Now.Date

        cmbtype.Items.Clear()
        cmbtype.Items.Add("ALL")
        cmbtype.Items.Add("BIRTHDAY")
        cmbtype.Items.Add("ANNIVERSARY")
        cmbtype.SelectedIndex = 0

        gridView.DataSource = Nothing
        gridView.Columns.Clear()
    End Sub

    Private Sub Wishes_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Close()
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Wishes_Load(Me, New EventArgs)
    End Sub

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        gridView.DataSource = Nothing
        gridView.Columns.Clear()

        Dim ChitDB As Boolean = False
        Dim strChitDB As String = ""

        If chkSavings.Checked Then
            StrSql = "SELECT CTLTEXT FROM " + cnAdminDb + "..SOFTCONTROL WHERE CTLID LIKE 'CHITDB'"
            ChitDB = GetSqlValue(cn, StrSql).ToString.ToUpper = "Y"
        End If

        If ChitDB Then
            StrSql = "SELECT CTLTEXT FROM " + cnAdminDb + "..SOFTCONTROL WHERE CTLID LIKE 'CHITDBPREFIX'"
            strChitDB = GetSqlValue(cn, StrSql).ToString.ToUpper + "SAVINGS"
            ChitDB = strChitDB <> ""
        End If

        StrSql = "SELECT TEMPLATE_DESC FROM " + cnAdminDb + "..SMSTEMPLATE WHERE ACTIVE = 'Y' AND TEMPLATE_NAME = 'SMS_MSG_BIRTHDAY_WISHES'"
        BirthDayTemplate = GetSqlValue(cn, StrSql)

        StrSql = "SELECT TEMPLATE_DESC FROM " + cnAdminDb + "..SMSTEMPLATE WHERE ACTIVE = 'Y' AND TEMPLATE_NAME = 'SMS_MSG_ANNIV_WISHES'"
        AnniversaryTemplate = GetSqlValue(cn, StrSql)

        If cmbtype.Text = "BIRTHDAY" Then
            If BirthDayTemplate = "" Then
                MsgBox("Check SMS Template " & vbCrLf & "SMS_MSG_BIRTHDAY_WISHES ", MsgBoxStyle.Information)
                Exit Sub
            End If
        ElseIf cmbtype.Text = "ANNIVERSARY" Then
            If AnniversaryTemplate = "" Then
                MsgBox("Check SMS Template " & vbCrLf & "SMS_MSG_ANNIV_WISHES", MsgBoxStyle.Information)
                Exit Sub
            End If
        Else
            If AnniversaryTemplate = "" Or BirthDayTemplate = "" Then
                MsgBox("Check SMS Template " & vbCrLf & "SMS_MSG_BIRTHDAY_WISHES " & vbCrLf & "SMS_MSG_ANNIV_WISHES", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If

        StrSql = ""
        StrSql += vbCrLf + " DECLARE @FROMDATE VARCHAR(12) = '" + dtpFrom_OWN.Value.ToString("yyyy-MM-dd") + "'"
        StrSql += vbCrLf + " DECLARE @TODATE VARCHAR(12) = '" + dtpTo_OWN.Value.ToString("yyyy-MM-dd") + "'"
        StrSql += vbCrLf
        If cmbtype.Text = "ALL" Or cmbtype.Text = "BIRTHDAY" Then
            StrSql += vbCrLf + " SELECT 'BIRTHDAY'TYPE,PNAME NAME,MOBILE,CONVERT(VARCHAR,DOB,103)DATE,CONVERT(VARCHAR,YEAR(GETDATE())-YEAR(DOB))[NO'S],'' WISHES"
            StrSql += vbCrLf + " FROM " + cnAdminDb + "..PERSONALINFO"
            StrSql += vbCrLf + " WHERE MONTH(DOB) BETWEEN MONTH(@FROMDATE) AND MONTH(@TODATE)"
            StrSql += vbCrLf + " AND DAY(DOB) BETWEEN DAY(@FROMDATE) AND DAY(@TODATE) AND ISNULL(DOB,'') <> ''"
            StrSql += vbCrLf + " UNION"
            StrSql += vbCrLf + " SELECT 'BIRTHDAY'TYPE,ACNAME,MOBILE,CONVERT(VARCHAR,DOBIRTH,103)DATE,CONVERT(VARCHAR,YEAR(GETDATE())-YEAR(DOBIRTH))[NO'S],''"
            StrSql += vbCrLf + " FROM " + cnAdminDb + "..ACHEAD"
            StrSql += vbCrLf + " WHERE ACTYPE  = 'C'"
            StrSql += vbCrLf + " AND MONTH(DOBIRTH) BETWEEN MONTH(@FROMDATE) AND MONTH(@TODATE)"
            StrSql += vbCrLf + " AND DAY(DOBIRTH) BETWEEN DAY(@FROMDATE) AND DAY(@TODATE) AND ISNULL(DOBIRTH,'') <> ''"
            If ChitDB Then
                StrSql += vbCrLf + " UNION"
                StrSql += vbCrLf + " SELECT 'BIRTHDAY'TYPE,PNAME NAME,MOBILE,CONVERT(VARCHAR,DOB,103)DATE,CONVERT(VARCHAR,YEAR(GETDATE())-YEAR(DOB))[NO'S],'' WISHES"
                StrSql += vbCrLf + " FROM " + strChitDB + "..PERSONALINFO WHERE ISNULL(DOB,'') <> ''"
                StrSql += vbCrLf + " AND MONTH(DOB) BETWEEN MONTH(@FROMDATE) AND MONTH(@TODATE)"
                StrSql += vbCrLf + " AND DAY(DOB) BETWEEN DAY(@FROMDATE) AND DAY(@TODATE)"
            End If
        End If
        If cmbtype.Text = "ALL" Then
            StrSql += vbCrLf + " UNION"
        End If
        If cmbtype.Text = "ALL" Or cmbtype.Text = "ANNIVERSARY" Then
            StrSql += vbCrLf + " SELECT 'ANNIVERSARY'TYPE,PNAME NAME,MOBILE,CONVERT(VARCHAR,ANNIVERSARY,103)DATE,CONVERT(VARCHAR,YEAR(GETDATE())-YEAR(ANNIVERSARY))[NO'S],'' WISHES"
            StrSql += vbCrLf + " FROM " + cnAdminDb + "..PERSONALINFO"
            StrSql += vbCrLf + " WHERE MONTH(ANNIVERSARY) BETWEEN MONTH(@FROMDATE) AND MONTH(@TODATE) "
            StrSql += vbCrLf + " AND DAY(ANNIVERSARY) BETWEEN DAY(@FROMDATE) AND DAY(@TODATE) AND ISNULL(ANNIVERSARY,'') <> ''"
            StrSql += vbCrLf + " UNION"
            StrSql += vbCrLf + " SELECT 'ANNIVERSARY'TYPE,ACNAME,MOBILE,CONVERT(VARCHAR,ANNIVERSARY,103)DATE,CONVERT(VARCHAR,YEAR(GETDATE())-YEAR(ANNIVERSARY))[NO'S],''"
            StrSql += vbCrLf + " FROM " + cnAdminDb + "..ACHEAD"
            StrSql += vbCrLf + " WHERE ACTYPE  = 'C'"
            StrSql += vbCrLf + " AND MONTH(ANNIVERSARY) BETWEEN MONTH(@FROMDATE) AND MONTH(@TODATE)"
            StrSql += vbCrLf + " AND DAY(ANNIVERSARY) BETWEEN DAY(@FROMDATE) AND DAY(@TODATE) AND ISNULL(ANNIVERSARY,'') <> ''"
            If ChitDB Then
                StrSql += vbCrLf + " UNION"
                StrSql += vbCrLf + " SELECT 'ANNIVERSARY'TYPE,PNAME NAME,MOBILE,CONVERT(VARCHAR,ANNIVERSARY_DATE,103)DATE,CONVERT(VARCHAR,YEAR(GETDATE())-YEAR(ANNIVERSARY_DATE))[NO'S],'' WISHES"
                StrSql += vbCrLf + " FROM " + strChitDB + "..PERSONALINFO WHERE ISNULL(ANNIVERSARY_DATE,'') <> ''"
                StrSql += vbCrLf + " AND MONTH(ANNIVERSARY_DATE) BETWEEN MONTH(@FROMDATE) AND MONTH(@TODATE)"
                StrSql += vbCrLf + " AND DAY(ANNIVERSARY_DATE) BETWEEN DAY(@FROMDATE) AND DAY(@TODATE) "
            End If
        End If
        StrSql += vbCrLf + " ORDER BY DATE,NAME"

        Dim Dt As DataTable = New DataTable()
        Dim Da As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(StrSql, cn)
        Dt.Columns.Add("SNO")
        Dt.Columns("SNO").AutoIncrement = True
        Dt.Columns("SNO").AutoIncrementSeed = 1
        Dt.Columns("SNO").AutoIncrementStep = 1
        Da.Fill(Dt)
        If Not Dt.Rows.Count > 0 Then
            MsgBox("No Record Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        For Each row As DataRow In Dt.Rows
            Dim No As Integer = Convert.ToInt16(row("NO'S").ToString().Substring(row("NO'S").ToString().Length - 1, 1))
            If No = 1 Then
                row("NO'S") = row("NO'S").ToString() + " st "
            ElseIf No = 2 Then
                row("NO'S") = row("NO'S").ToString() + " nd "
            ElseIf No = 3 Then
                row("NO'S") = row("NO'S").ToString() + " rd "
            Else
                row("NO'S") = row("NO'S").ToString() + " th "
            End If
            row("NO'S") = row("NO'S").ToString() + row("TYPE").ToString()

            If row("TYPE") = "BIRTHDAY" Then
                If BirthDayTemplate <> "" Then
                    row("WISHES") = BirthDayTemplate
                Else
                    row("WISHES") = "BIRTH DAY WISHES BY " & cnCompanyName
                End If
            Else
                If AnniversaryTemplate <> "" Then
                    row("WISHES") = AnniversaryTemplate
                Else
                    row("WISHES") = "ANNIVERSARY WISHES BY " & cnCompanyName
                End If
            End If
            For Each col As DataColumn In Dt.Columns
                If row("TYPE") = "BIRTHDAY" Then
                    row("WISHES") = row("WISHES").Replace("<" + col.ColumnName.ToString + ">", row(col.ColumnName.ToString))
                Else
                    row("WISHES") = row("WISHES").Replace("<" + col.ColumnName.ToString + ">", row(col.ColumnName.ToString))
                End If
            Next
        Next
        gridView.DataSource = Dt
        Dim ChkBox As New DataGridViewCheckBoxColumn
        ChkBox.FlatStyle = FlatStyle.Standard
        gridView.Columns.Insert(1, ChkBox)
        gridView.Columns(1).Name = "Chk"
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        For Each col As DataGridViewColumn In gridView.Columns
            col.ReadOnly = True
            col.Width = col.Width
        Next
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        For Each row As DataGridViewRow In gridView.Rows
            row.Cells(1).Value = chkAll.Checked
        Next
        gridView.Columns(1).ReadOnly = chkAll.Checked
    End Sub

    Private Sub chkAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkAll.CheckedChanged
        If Not gridView.Rows.Count > 0 Then
            Exit Sub
        End If
        For Each row As DataGridViewRow In gridView.Rows
            row.Cells(1).Value = chkAll.Checked
        Next
        gridView.Columns(1).ReadOnly = chkAll.Checked
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        StrSql = "SELECT COUNT(*)CNT FROM SYSDATABASES WHERE NAME = 'AKSHAYASMSDB'"
        If Not Convert.ToInt16(GetSqlValue(cn, StrSql)) > 0 Then
            MsgBox("AKSHAYASMSDB Not Found", MsgBoxStyle.Critical)
            Exit Sub
        End If

        StrSql = "SELECT COUNT(*)CNT FROM AKSHAYASMSDB..SYSOBJECTS WHERE NAME = 'SMSDATA'"
        If Not Convert.ToInt16(GetSqlValue(cn, StrSql)) > 0 Then
            MsgBox("AKSHAYASMSDB Not Found", MsgBoxStyle.Critical)
            Exit Sub
        End If

        StrSql = ""
        For Each row As DataGridViewRow In gridView.Rows
            If row.Cells("MOBILE").Value.ToString().Length < 10 Then
                Continue For
            End If
            If Not row.Cells("CHK").Value Then
                Continue For
            End If
            StrSql += vbCrLf
            StrSql += vbCrLf + " INSERT INTO AKSHAYASMSDB..SMSDATA"
            StrSql += vbCrLf + "            ([MOBILENO]"
            StrSql += vbCrLf + "            ,[MESSAGES]"
            StrSql += vbCrLf + "            ,[STATUS])"
            StrSql += vbCrLf + "    VALUES"
            StrSql += vbCrLf + "            ('" + row.Cells("MOBILE").Value.ToString().Replace("'", "''") + "'"
            StrSql += vbCrLf + "            ,'" + row.Cells("WISHES").Value.ToString().Replace("'", "''") + "'"
            StrSql += vbCrLf + "            ,'N')"
            StrSql += vbCrLf
        Next
        If StrSql.Trim <> "" Then
            ExecQuery(Nothing, StrSql, cn)
        End If
        MsgBox("Success", MsgBoxStyle.Information)
        Wishes_Load(Me, New EventArgs)
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Dim title As String
        title = "SMS LIST "
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, title, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
End Class