Imports System.Data.OleDb

Public Class FRM_RedeemPriviledge
    Dim StrSql As String
    Dim cmd As OleDbCommand = Nothing
    Dim da As OleDbDataAdapter
    Private TRANNO As Integer = 0
    Public mRate As Decimal = 0
    Dim PREVDISC_PER As String = GetAdmindbSoftValue("PREVDISC_PER", "")
    Dim PREV_RECORD As Boolean = IIf(GetAdmindbSoftValue("PREV_RECORD", "N") = "Y", True, False)
    Dim PREV_MIN_POINTS As String = GetAdmindbSoftValue("PREV_MIN_POINTS", "")
    Dim PREV_MIN_POINTS_PER As Decimal = Val(GetAdmindbSoftValue("PREV_MIN_POINTS_PER", "0").ToString)
    Public BillAmt As Decimal
    Dim PrevMinPoints As Integer
    Dim PrevValue As Decimal
    Dim PREMAXPOINT_REDPERDAY As Double = Val(GetAdmindbSoftValue("PREMAXPOINT_REDPERDAY", "0").ToString)
    Dim PREPOINT_VALIDITY As Double = Val(GetAdmindbSoftValue("PREPOINT_VALIDITY", "0").ToString)
    Dim PRETYPE As Boolean = IIf(GetAdmindbSoftValue("PRETYPE", "N") = "Y", True, False)

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        txtPriviledge.Text = ""
        txtPoints_NUM.Text = ""
        txtValue_NUM.Text = ""
        txtPValue_NUM.Text = ""
        txtRPoints_AMT.Text = ""
        txtPAmount_NUM.Text = ""
        lblCustomername.Text = ""
        lblValType.Text = ""
        txtPriviledge.Focus()
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If PrevMinPoints > 0 And Val(txtRPoints_AMT.Text) > PrevMinPoints Then
            txtRPoints_AMT.Focus()
            MsgBox("Redemption points should not exceed Minimum Points...", MsgBoxStyle.Information)
            Exit Sub
        End If
        If PREV_MIN_POINTS_PER > 0 Then
            Dim MinPoints As Decimal = (Val(txtPoints_NUM.Text) * PREV_MIN_POINTS_PER) / 100
            If Val(txtRPoints_AMT.Text) > MinPoints Then
                txtRPoints_AMT.Focus()
                MsgBox("Redemption points should not exceed Minimum Points...", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        If txtPriviledge.Text.Trim <> "" And Val(txtPValue_NUM.Text) > 0 Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            txtPriviledge.Focus()
            Me.Close()
        End If
        If PREVDISC_PER <> "" Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            txtPriviledge.Focus()
            Me.Close()
        End If
    End Sub

    Private Sub FRM_RedeemPriviledge_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.Escape Then
            If Val(txtPValue_NUM.Text) > 0 Then
                btnOk_Click(Me, New EventArgs)
                Exit Sub
            End If
            If txtPriviledge.Text.ToString <> "" Then
                btnOk_Click(Me, New EventArgs)
                Exit Sub
            End If
            txtPriviledge.Text = ""
            txtPoints_NUM.Text = ""
            txtValue_NUM.Text = ""
            txtPValue_NUM.Text = ""
            txtRPoints_AMT.Text = ""
            txtPAmount_NUM.Text = ""
            lblCustomername.Text = ""
            lblValType.Text = ""
            txtPriviledge.Focus()
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub txtReturn_NUM_Enter(ByVal sender As Object, ByVal e As System.EventArgs)
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtPriviledge_NUM_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPriviledge.KeyDown
        If e.KeyCode = Keys.Insert Then
            custname()
        End If
        If e.KeyCode = Keys.Enter Then
            If txtPriviledge.Text.ToString() = "" Then
                Dim STRSQL As String
                STRSQL = " SELECT PREVILEGEID,ACCODE ,ACNAME FROM " & cnAdminDb & "..ACHEAD "
                STRSQL += " where  PREVILEGEID <>'' GROUP BY PREVILEGEID,ACCODE ,ACNAME "
                txtPriviledge.Text = BrighttechPack.SearchDialog.Show("Select CUSTOMER", STRSQL, cn, 2)
            End If
            If Trim(txtPriviledge.Text) <> "" Then
                If PREV_RECORD Then
                    StrSql = "SELECT SUM(CASE WHEN TRANTYPE='R' THEN POINTS ELSE -1*POINTS END) AS POINTS "
                    StrSql += vbCrLf + " ,SUM(CASE WHEN TRANTYPE='R' THEN PVALUE ELSE -1*PVALUE END)AS PVALUE"
                    StrSql += vbCrLf + " FROM  " & cnAdminDb & "..PRIVILEGETRAN  "
                    StrSql += vbCrLf + " WHERE PREVILEGEID='" & Trim(txtPriviledge.Text) & "'"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''  "
                    If PREPOINT_VALIDITY > 0 Then
                        Dim PreDateFrom As DateTime = GetEntryDate(GetServerDate(tran)) '
                        PreDateFrom = PreDateFrom.AddDays(-1 * PREPOINT_VALIDITY)
                        StrSql += vbCrLf + " AND TRANDATE>='" + PreDateFrom.ToString("yyyy-MM-dd") + "'  "
                    End If
                    cmd = New OleDbCommand(StrSql, cn)
                    da = New OleDbDataAdapter(cmd)
                    Dim dt As New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        txtPoints_NUM.Text = Val(dt.Rows(0).Item(0).ToString())
                        txtValue_NUM.Text = Math.Round(Val(dt.Rows(0).Item(1).ToString), 2)
                        lblValType.Text = "AMOUNT"
                        Dim minPvPoints As Integer = Val(GetAdmindbSoftValue("PREVMINPOINTS", "0", tran))
                        If minPvPoints <> 0 And minPvPoints > Val(txtPoints_NUM.Text) Then
                            MsgBox("Accumulated Points are less than Minimum Points", MsgBoxStyle.Information)
                            txtRPoints_AMT.Enabled = False
                            btnOk.Enabled = False
                        End If
                    End If
                Else
                    Dim StartDate As Date
                    StartDate = objGPack.GetSqlValue("SELECT TOP 1 STARTDATE FROM " & cnAdminDb & "..DBMASTER ORDER BY STARTDATE ASC")
                    Dim Fromdate As Date = IIf(GetAdmindbSoftValue("PREVILEDGE_DATE", StartDate) = "", StartDate, GetAdmindbSoftValue("PREVILEDGE_DATE", StartDate))
                    Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))

                    lblCustomername.Text = objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = '" & txtPriviledge.Text.ToString & "'", , )
                    StrSql = " EXEC " & cnStockDb & "..SP_RPT_PREVILEGETRAN"
                    StrSql += vbCrLf + " @SYSTEMID = '" & Sysid & "'"
                    'By Rajkumar for ARRS for taking Last Year Privelige and changes in SP in Particular Db
                    'StrSql += vbCrLf + " ,@FRMDATE = '" & cnTranFromDate.Date.ToString("yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " ,@FRMDATE = '" & Format(Fromdate, "yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " ,@TODATE = '" & cnTranToDate.Date.ToString("yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " ,@PREVILEGEID = '" & txtPriviledge.Text & "'"
                    StrSql += vbCrLf + " ,@METALNAME = 'ALL'"
                    StrSql += vbCrLf + " ,@COSTNAME = 'ALL'"
                    StrSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
                    If GetAdmindbSoftValue("CHITDB", "N").ToUpper = "Y" Then
                        StrSql += vbCrLf + " ,@WITHCHITPOINTS = 'Y'"
                    Else
                        StrSql += vbCrLf + " ,@WITHCHITPOINTS = 'N'"
                    End If
                    If PRETYPE Then
                        StrSql += vbCrLf + " ,@ONLYPREVILEGE = 'N'"
                    End If
                    cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    Dim dt As New DataTable
                    If PREVDISC_PER <> "" And PREVDISC_PER.Contains("/") Then
                        If Val(txtPValue_NUM.Text) = 0 Then
                            Dim NoofSlap As Integer
                            Dim TotalAmt, DiscPer, FrmRange, ToRange As Decimal
                            Dim PREVDISC_PERS() As String
                            Dim PREVDISC_PER_DISC() As String
                            Dim Flag As Boolean = False
                            PREVDISC_PERS = PREVDISC_PER.Split("/")
                            NoofSlap = PREVDISC_PERS.Length
                            StrSql = "SELECT SUM(ISNULL(AMOUNT,0))AMOUNT  "
                            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN  "
                            StrSql += vbCrLf + " WHERE RESULT=1 "
                            cmd = New OleDbCommand(StrSql, cn)
                            da = New OleDbDataAdapter(cmd)
                            da.Fill(dt)
                            If dt.Rows.Count > 0 Then
                                TotalAmt = Val(dt.Rows(0).Item("AMOUNT").ToString())
                            End If
                            TotalAmt += BillAmt
                            If NoofSlap > 0 Then
                                PREVDISC_PER_DISC = PREVDISC_PERS(0).Split("-")
                                If PREVDISC_PER_DISC.Length > 2 Then
                                    FrmRange = Val(PREVDISC_PER_DISC(0).ToString)
                                    ToRange = Val(PREVDISC_PER_DISC(1).ToString)
                                    DiscPer = Val(PREVDISC_PER_DISC(2).ToString)
                                    If FrmRange < TotalAmt And ToRange > TotalAmt Then
                                        txtPValue_NUM.Text = Math.Round((BillAmt * DiscPer) / 100, 2)
                                        txtPAmount_NUM.Text = Math.Round((BillAmt * DiscPer) / 100, 2)
                                        Flag = True
                                    End If
                                End If
                            End If
                            If NoofSlap > 1 And Flag = False Then
                                PREVDISC_PER_DISC = PREVDISC_PERS(1).Split("-")
                                If PREVDISC_PER_DISC.Length > 2 Then
                                    FrmRange = Val(PREVDISC_PER_DISC(0).ToString)
                                    ToRange = Val(PREVDISC_PER_DISC(1).ToString)
                                    DiscPer = Val(PREVDISC_PER_DISC(2).ToString)
                                    If FrmRange < TotalAmt And ToRange > TotalAmt Then
                                        txtPValue_NUM.Text = Math.Round((BillAmt * DiscPer) / 100, 2)
                                        txtPAmount_NUM.Text = Math.Round((BillAmt * DiscPer) / 100, 2)
                                        Flag = True
                                    End If
                                End If
                            End If
                            If Flag Then
                                txtRPoints_AMT.Enabled = False
                                btnOk.Enabled = True
                            End If
                        End If
                    Else
                        StrSql = "SELECT SUM(ISNULL(POINTS,0)) , SUM(ISNULL(BPOINTVALUES,0)),VALUEINWTRAMT  "
                        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN  "
                        StrSql += vbCrLf + " WHERE RESULT=1 AND ISNULL(VALUEINWTRAMT,'')<>'' GROUP BY VALUEINWTRAMT"
                        cmd = New OleDbCommand(StrSql, cn)
                        da = New OleDbDataAdapter(cmd)
                        da.Fill(dt)
                        If dt.Rows.Count > 0 Then
                            txtPoints_NUM.Text = Val(dt.Rows(0).Item(0).ToString())
                            txtValue_NUM.Text = Math.Round(Val(dt.Rows(0).Item(1).ToString()), IIf(dt.Rows(0).Item(2).ToString() = "W", 3, 2))
                            lblValType.Text = IIf(dt.Rows(0).Item(2).ToString() = "W", "WEIGHT", "AMOUNT")
                            'txtRPoints_NUM.Focus()
                            Dim minPvPoints As Integer = Val(GetAdmindbSoftValue("PREVMINPOINTS", "0", tran))
                            If minPvPoints <> 0 And minPvPoints > Val(txtPoints_NUM.Text) Then
                                MsgBox("Accumulated Points are less than Minimum Points", MsgBoxStyle.Information)
                                txtRPoints_AMT.Enabled = False
                                btnOk.Enabled = False
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub custname()
        Dim STRSQL As String
        STRSQL = " SELECT LTRIM(PREVILEGEID)PREVILEGEID,ACCODE ,ACNAME,MOBILE FROM " & cnAdminDb & "..ACHEAD "
        STRSQL += " where  PREVILEGEID <>'' order BY PREVILEGEID,ACCODE,ACNAME,MOBILE"
        Dim priid As String = BrighttechPack.SearchDialog.Show("Search Previledge Customer ", STRSQL, cn, 3)
        If Trim(priid) <> "" Then
            txtPriviledge.Text = priid
        End If
    End Sub
    Private Sub txtRPoints_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRPoints_AMT.TextChanged
        If Val(txtRPoints_AMT.Text.Trim()) > Val(txtPoints_NUM.Text) Then txtRPoints_AMT.Text = txtPoints_NUM.Text
        If Val(PREMAXPOINT_REDPERDAY.ToString) > 0 Then
            StrSql = "SELECT ISNULL(SUM(POINTS),0)POINTS FROM " & cnAdminDb & "..PRIVILEGETRAN WHERE PREVILEGEID='" + txtPriviledge.Text.ToString + "' AND TRANDATE='" + GetEntryDate(GetServerDate(tran)) + "' AND ISNULL(TRANTYPE,'')='I' AND ISNULL(CANCEL,'')=''"
            Dim TodayPoints As Double = Val(objGPack.GetSqlValue(StrSql,, "0").ToString)
            If TodayPoints <> 0 Then
                If TodayPoints >= Val(PREMAXPOINT_REDPERDAY.ToString) Then
                    MsgBox("Maximum points Redeem perday : " + PREMAXPOINT_REDPERDAY.ToString)
                    txtRPoints_AMT.Text = "0.00"
                    'Exit Sub
                ElseIf Val(txtRPoints_AMT.Text.Trim()) + TodayPoints > Val(PREMAXPOINT_REDPERDAY.ToString) Then
                    MsgBox("Maximum points Redeem perday : " + PREMAXPOINT_REDPERDAY.ToString)
                    txtRPoints_AMT.Text = Math.Round(Val(PREMAXPOINT_REDPERDAY.ToString) - TodayPoints, 2)
                    'Exit Sub
                End If
            End If
            If Val(txtRPoints_AMT.Text.Trim()) > Val(PREMAXPOINT_REDPERDAY.ToString) Then
                    MsgBox("Maximum points Redeem perday : " + PREMAXPOINT_REDPERDAY.ToString)
                    txtRPoints_AMT.Text = Val(PREMAXPOINT_REDPERDAY.ToString)
                'Exit Sub
            End If
            End If
            If txtRPoints_AMT.Text.Trim() <> "" Then
            If lblValType.Text = "WEIGHT" And mRate <> 0 Then
                txtPAmount_NUM.Text = Math.Round(Val(txtValue_NUM.Text) * Val(txtRPoints_AMT.Text) / Val(txtPoints_NUM.Text), 3).ToString()
                txtPValue_NUM.Text = Math.Round((Val(txtPAmount_NUM.Text)) * mRate, 2).ToString()
            Else
                txtPAmount_NUM.Text = Math.Round(Val(txtValue_NUM.Text) * Val(txtRPoints_AMT.Text) / Val(txtPoints_NUM.Text), 2).ToString()
                txtPValue_NUM.Text = Math.Round(Val(txtPAmount_NUM.Text), 2).ToString()
            End If
        End If
    End Sub

    Private Sub txtPriviledge_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPriviledge.TextChanged

    End Sub

    Private Sub FRM_RedeemPriviledge_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim prev() As String
        prev = PREV_MIN_POINTS.Split("-")
        If prev.Length = 2 Then
            PrevValue = prev(0)
            PrevMinPoints = (BillAmt / PrevValue) * Val(prev(1).ToString)
        End If
    End Sub
End Class