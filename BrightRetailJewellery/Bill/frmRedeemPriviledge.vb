Imports System.Data.OleDb

Public Class frmRedeemPriviledge
    Dim StrSql As String

    Dim cmd As OleDbCommand = Nothing
    Dim da As OleDbDataAdapter
    Private TRANNO As Integer = 0
    Public mRate As Decimal = 0


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        txtPriviledge.Text = ""
        txtPoints_NUM.Text = ""
        txtPValue_NUM.Text = ""
        txtPAmount_NUM.Text = ""
        lblCustomername.Text = ""
        txtPriviledge.Focus()
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If txtPriviledge.Text.Trim <> "" And Val(txtPValue_NUM.Text) > 0 Then
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
            txtPriviledge.Text = ""
            txtPoints_NUM.Text = ""
            txtPValue_NUM.Text = ""
            txtRPoints_AMT.Text = ""
            txtPAmount_NUM.Text = ""
            lblCustomername.Text = ""
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
                txtPriviledge.Text = GiritechPack.SearchDialog.Show("Select CUSTOMER", STRSQL, cn, 2)
            End If
            If Trim(txtPriviledge.Text) <> "" Then
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
                    StrSql += vbCrLf + " ,@WithChitPoints = 'Y'"
                Else
                    StrSql += vbCrLf + " ,@WithChitPoints = 'N'"
                End If
                cmd = New OleDbCommand(StrSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                Dim dt As New DataTable
                StrSql = "SELECT SUM(ISNULL(POINTS,0)) , SUM(ISNULL(BPOINTVALUES,0)),VALUEINWTRAMT  FROM TEMPTABLEDB..TEMP" & Sysid & "PREVILEGETRAN  WHERE RESULT=1 AND ISNULL(VALUEINWTRAMT,'')<>'' GROUP BY VALUEINWTRAMT"
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

    End Sub
    Private Sub custname()
        Dim STRSQL As String
        STRSQL = " SELECT LTRIM(PREVILEGEID)PREVILEGEID,ACCODE ,ACNAME,MOBILE FROM " & cnAdminDb & "..ACHEAD "
        STRSQL += " where  PREVILEGEID <>'' order BY PREVILEGEID,ACCODE,ACNAME,MOBILE"
        Dim priid As String = GiritechPack.SearchDialog.Show("Search Previledge Customer ", STRSQL, cn, 3)
        If Trim(priid) <> "" Then
            txtPriviledge.Text = priid
        End If
    End Sub
    Private Sub txtRPoints_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If Val(txtRPoints_AMT.Text.Trim()) > Val(txtPoints_NUM.Text) Then txtRPoints_AMT.Text = txtPoints_NUM.Text
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
End Class