Imports System.Data.OleDb
Public Class frmGiftVoucherWt2WtAdj
    Dim strSql As String
    Public dtBookedAdvanceView As New DataTable
    Public dtReservedItem As New DataTable
    Public dtGridAdvance As New DataTable
    Public REFNOPREFIXAUTO As String = ""
    Dim returnRow As DataRow = Nothing
    Private lock As Boolean
    Dim cmd As OleDbCommand
    Dim WTADV2AMTCURRRATE As Boolean = IIf(GetAdmindbSoftValue("WTADV2AMTCURRRATE", "N") = "Y", True, False)
    Dim ADV_RATE_METAL As String = GetAdmindbSoftValue("ADV_RATE_METAL", "G")
    Public Trandate As Date

    Public Sub New(ByVal start As Boolean)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
    End Sub
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        With dtBookedAdvanceView.Columns
            .Add("RUNNO", GetType(String))
            .Add("RECEIPT", GetType(Double))
            .Add("PAYMENT", GetType(Double))
            .Add("AMOUNT", GetType(Double))
            .Add("FIXEDRATE", GetType(Double))
            .Add("NAME", GetType(String))
            .Add("ADDRESS", GetType(String))
            .Add("BOOKWT", GetType(Double))
            .Add("ADJWT", GetType(Double))
            .Add("ACCODE", GetType(String))
            .Add("TRANDATE", GetType(Date))
            .Add("TITLE", GetType(String))
            .Add("INITIAL", GetType(String))
            .Add("DOORNO", GetType(String))
            .Add("ADDRESS1", GetType(String))
            .Add("ADDRESS2", GetType(String))
            .Add("ADDRESS3", GetType(String))
            .Add("AREA", GetType(String))
            .Add("CITY", GetType(String))
            .Add("STATE", GetType(String))
            .Add("COUNTRY", GetType(String))
            .Add("PINCODE", GetType(String))
            .Add("FAX", GetType(String))
            .Add("EMAIL", GetType(String))
            .Add("PHONERES", GetType(String))
            .Add("MOBILE", GetType(String))
            .Add("PREVILEGEID", GetType(String))
            .Add("SNO", GetType(String))
            .Add("COMPANYID", GetType(String))
            .Add("ADVFIXWTPER", GetType(Double))
            .Add("ADJTYPE", GetType(String))
            .Add("ADVMODE", GetType(String))
        End With
        gridBookedAdvance.DataSource = dtBookedAdvanceView
        With gridBookedAdvance
            .Columns("COMPANYID").Width = 0
            .Columns("RUNNO").Width = txtRunNO.Width + 1
            .Columns("AMOUNT").Width = txtBalance_AMT.Width + 1
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("RECEIPT").Width = txtReceived_AMT.Width + 1
            .Columns("RECEIPT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RECEIPT").DefaultCellStyle.Format = "0.00"
            .Columns("PAYMENT").Width = txtAdjusted_AMT.Width + 1
            .Columns("PAYMENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PAYMENT").DefaultCellStyle.Format = "0.00"
            .Columns("FIXEDRATE").Width = txtRateFix.Width + 1
            .Columns("FIXEDRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("FIXEDRATE").DefaultCellStyle.Format = "0.00"
            .Columns("NAME").Width = txtName.Width + 1
            .Columns("ADDRESS").Width = txtDoorNo.Width + 1
            For cnt As Integer = 7 To gridBookedAdvance.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
        End With
        With dtReservedItem.Columns
            .Add("ESTNO", GetType(String))
            .Add("RUNNO", GetType(String))
            .Add("ITEMID", GetType(Integer))
            .Add("ITEMNAME", GetType(String))
            .Add("PCS", GetType(Integer))
            .Add("TAGNO", GetType(String))
            .Add("GRSWT", GetType(Double))
            .Add("NETWT", GetType(Double))
            .Add("RATE", GetType(Double))
            .Add("SALVALUE", GetType(Double))
            .Add("BATCHNO", GetType(String))
        End With
        With dtGridAdvance
            .Columns.Add("COSTID", GetType(String))
            .Columns.Add("RUNNO", GetType(String))
            .Columns.Add("DATE", GetType(Date))
            .Columns.Add("RECEIVEDAMT", GetType(Double))
            .Columns.Add("ADJUSTEDAMT", GetType(Double))
            .Columns.Add("AMOUNT", GetType(Double))
            .Columns.Add("PNAME", GetType(String))
            .Columns.Add("ADDRESS1", GetType(String))
            .Columns.Add("ADDRESS2", GetType(String))
            .Columns.Add("ADDRESS3", GetType(String))
            .Columns.Add("REFNO", GetType(String))
            .Columns.Add("ACCODE", GetType(String))
            .Columns.Add("ISEDIT", GetType(Char))
            .Columns.Add("COMPANYID", GetType(String))
            .Columns.Add("ADVFIXWTPER", GetType(Double))
            .Columns.Add("REMARK", GetType(String))
            .Columns.Add("ORGRUNNO", GetType(String))
            .Columns.Add("ORGBATCHNO", GetType(String))
            .Columns.Add("ADJTYPE", GetType(String))
        End With        
    End Sub
    Private Function getHelpStr(Optional ByVal ruNo As String = Nothing) As String
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO') > 0"
        strSql += vbCrLf + " DROP VIEW TEMP" & systemId & "_CUSTOMERINFO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " CREATE VIEW TEMP" & systemId & "_CUSTOMERINFO"
        strSql += vbCrLf + " AS"
        strSql += vbCrLf + " SELECT C.PSNO as SNO,C.BATCHNO,TRANDATE,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT COMPANYID,RUNNO "
        strSql += vbCrLf + " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END AS RECEIPT"
        strSql += vbCrLf + " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END AS PAYMENT"
        strSql += vbCrLf + " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS BALANCE"
        strSql += vbCrLf + " ,V.TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
        strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO"
        strSql += vbCrLf + " ,V.BATCHNO,RATE,ADVFIXWTPER,ADVMODE"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT COMPANYID,RUNNO"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE 0 END)AS RECEIPT"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN GRSWT ELSE 0 END)AS PAYMENT"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END)AS BALANCE"
        strSql += vbCrLf + " ,1 AS RATE"
        strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND ISNULL(CANCEL,'') = '' ORDER BY SNO DESC)AS BATCHNO,O.ADVFIXWTPER,'W' ADVMODE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
        strSql += vbCrLf + " WHERE TRANTYPE = 'GV' AND COMPANYID='" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' and AMOUNT =0"
        If ruNo <> Nothing Then
            strSql += vbCrLf + " AND  RUNNO= '" & ruNo & "'"
        End If
        strSql += vbCrLf + " GROUP BY COMPANYID,RUNNO,ADVFIXWTPER"
        strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END) > 0 "
        strSql += vbCrLf + " )X"
        strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
        strSql += vbCrLf + " WHERE RATE > 0 OR V.BATCHNO IN (SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..ITEMTAG WHERE BATCHNO = X.BATCHNO AND APPROVAL = 'R')"
        Return strSql
    End Function
    Private Function CheckRunno() As Boolean
        For Each ro As DataRow In dtBookedAdvanceView.Rows
            If ro!RUNNO.ToString = txtRunNO.Text Then
                Return True
            End If
        Next
    End Function
    Private Sub assignValues()
        If returnRow Is Nothing Then
            objGPack.TextClear(Me)
            txtRunNO.Select()
            Exit Sub
        End If
        txtRunNO.Text = returnRow!RUNNO.ToString
        If CheckRunno() Then
            MsgBox("Already Loaded", MsgBoxStyle.Information)
            objGPack.TextClear(Me)
            txtRunNO.Select()
            Exit Sub
        End If
        txtReceived_AMT.Text = Format(Val(returnRow!RECEIPT.ToString), "0.000")
        txtAdjusted_AMT.Text = IIf(Val(returnRow!PAYMENT.ToString) > 0, Format(Val(returnRow!PAYMENT.ToString), "0.000"), Nothing)
        txtBalance_AMT.Text = Format(Val(returnRow!BALANCE.ToString), "0.00")
        txtRateFix.Text = IIf(Val(returnRow!RATE.ToString) <> 0, Format(Val(returnRow!RATE.ToString), "0.000"), Nothing)
        If Val(txtRateFix.Text) = 0 Then
            strSql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PURITYID = "
            strSql += " (SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN 91.6 AND 92 "
            If ADV_RATE_METAL <> "N" And Not ADV_RATE_METAL Is Nothing Then strSql += " AND METALID='" & ADV_RATE_METAL & "'"
            strSql += " ORDER BY PURITY DESC)"
            Dim purrate As Double = Val(GetRate(Trandate, objGPack.GetSqlValue(strSql)))
            txtRateFix.Text = Format(purrate, "0.000")
        End If

        strSql = "SELECT TRANTYPE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO='" & txtRunNO.Text.ToString & "'"

        If objGPack.GetSqlValue(strSql).ToString = "GV" Then
            strSql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PURITYID = "
            If txtRunNO.Text.ToString.Contains("GG") Then
                strSql += " (SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY ='91.6'"
                strSql += " AND METALID='G'"
            Else
                strSql += " (SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN 91.6 AND 92 "
                strSql += " AND METALID='S'"
            End If
            strSql += " ORDER BY PURITY DESC)"
            Dim purrate As Double = Val(GetRate(Trandate, objGPack.GetSqlValue(strSql)))
            txtRateFix.Text = Format(purrate, "0.000")
            txtBalance_AMT.Text = Format(Val(returnRow!BALANCE.ToString) * Val(txtRateFix.Text.ToString), "0.00")
        End If

        txtName.Text = returnRow!pNAME.ToString
        txtDoorNo.Text = returnRow!DOORNO.ToString
        txtBalance_AMT.Focus()
        txtCompanyid.Text = returnRow!companyid.ToString
        txtAdvMode.Text = returnRow!advmode.ToString
    End Sub

    Private Sub txtRunNO_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRunNO.GotFocus
        If REFNOPREFIXAUTO = "Y" And txtRunNO.Text.Trim = "" Then txtRunNO.Text = "GV" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString : SendKeys.Send("{END}")
    End Sub
    Private Sub txtRunNO_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRunNO.KeyDown
        If lock Then Exit Sub
        If e.KeyCode = Keys.Insert Then
            returnRow = BrighttechPack.SearchDialog.Show_R("Search RunNo", getHelpStr, cn)
            assignValues()
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        txtRunNO.Focus()
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Not dtBookedAdvanceView.Rows.Count > 0 Then
            btnExit.Focus()
        End If
        lock = True
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub txtRunNO_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRunNO.KeyPress
        If lock Then Exit Sub
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtRunNO.Text = "" Then
                MsgBox("RunNo Should Not Empty", MsgBoxStyle.Information)
                txtRunNO.Select()
            Else
                Dim dt As New DataTable
                da = New OleDbDataAdapter(getHelpStr(txtRunNO.Text), cn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    returnRow = dt.Rows(0)
                    assignValues()
                Else
                    MsgBox(E0004 + Me.GetNextControl(txtRunNO, False).Text, MsgBoxStyle.Information)
                    txtRunNO.Select()
                End If
            End If
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub frmGiftVoucherWt2WtAdj_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If dtBookedAdvanceView.Rows.Count > 0 Then
                btnOk.Focus()
            Else
                btnExit.Focus()
            End If
        End If
    End Sub

    Private Sub frmGiftVoucherWt2WtAdj_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridBookedAdvance.ColumnHeadersVisible = False
    End Sub

    Private Sub gtFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
    txtName.GotFocus, txtRateFix.GotFocus, txtDoorNo.GotFocus, txtAdjusted_AMT.GotFocus, txtReceived_AMT.GotFocus
        txtBalance_AMT.Focus()
    End Sub

    Private Sub gridBookedAdvance_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridBookedAdvance.UserDeletedRow
        dtBookedAdvanceView.AcceptChanges()
    End Sub

    Private Sub gridBookedAdvance_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles gridBookedAdvance.UserDeletingRow
        For Each ro As DataRow In dtReservedItem.Rows
            If ro!RUNNO.ToString = e.Row.Cells("RUNNO").Value.ToString Then
                ro.Delete()
            End If
        Next
        dtReservedItem.AcceptChanges()
    End Sub

    Private Sub txtBalance_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBalance_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If returnRow Is Nothing Then
                objGPack.TextClear(Me)
                txtRunNO.Select()
                Exit Sub
            End If
            txtRunNO.Text = returnRow!RUNNO.ToString
            If CheckRunno() Then
                MsgBox("Already Loaded", MsgBoxStyle.Information)
                objGPack.TextClear(Me)
                txtRunNO.Select()
                Exit Sub
            End If
            Dim row As DataRow = dtBookedAdvanceView.NewRow
            row("RUNNO") = txtRunNO.Text
            row("AMOUNT") = Val(txtBalance_AMT.Text)
            row("FIXEDRATE") = Val(txtRateFix.Text)
            row("NAME") = txtName.Text
            row("ADDRESS") = txtDoorNo.Text + "," + returnRow!ADDRESS1.ToString

            If Val("" & returnRow!ADVFIXWTPER) > 100 Then
                row("BOOKWT") = Math.Round((Val(txtBalance_AMT.Text) / Val(txtRateFix.Text)) * (Val("" & returnRow!advfixwtper) / 100), 3)
                If txtAdvMode.Text = "W" Then row("BOOKWT") = Math.Round(Val(txtBalance_AMT.Text), 3)
            ElseIf Val("" & returnRow!ADVFIXWTPER) < 100 Then
                Dim madvbalance As Double = Val(txtBalance_AMT.Text) / (Val(returnRow!advfixwtper.ToString) / 100)

                row("BOOKWT") = Math.Round((madvbalance / Val(txtRateFix.Text)), 3)
                If txtAdvMode.Text = "W" Then row("BOOKWT") = Math.Round(Val(txtReceived_AMT.Text), 3)

            Else
                row("BOOKWT") = Math.Round(Val(txtBalance_AMT.Text) / Val(txtRateFix.Text), 3)
                If txtAdvMode.Text = "W" Then row("BOOKWT") = Math.Round(Val(txtReceived_AMT.Text), 3)
            End If
            If txtAdvMode.Text = "W" Then row("ADVMODE") = "W"
            row("RECEIPT") = returnRow!RECEIPT
            row("PAYMENT") = returnRow!PAYMENT
            row("TRANDATE") = returnRow!TRANDATE
            row("COMPANYID") = returnRow!COMPANYID
            row!PREVILEGEID = returnRow!PREVILEGEID


            strSql = "SELECT TRANTYPE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO='" & txtRunNO.Text.ToString & "'"
            If objGPack.GetSqlValue(strSql).ToString = "GV" Then
                strSql = "SELECT ACCODE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO='" & txtRunNO.Text.ToString & "'"
                row!ACCODE = objGPack.GetSqlValue(strSql).ToString
            Else
                row!ACCODE = returnRow!ACCODE
            End If

            row!TITLE = returnRow!TITLE
            row!INITIAL = returnRow!INITIAL
            row!NAME = returnRow!pNAME
            row!DOORNO = returnRow!DOORNO
            row!ADDRESS1 = returnRow!ADDRESS1
            row!ADDRESS2 = returnRow!ADDRESS2
            row!ADDRESS3 = returnRow!ADDRESS3
            row!AREA = returnRow!AREA
            row!CITY = returnRow!CITY
            row!STATE = returnRow!STATE
            row!COUNTRY = returnRow!COUNTRY
            row!PINCODE = returnRow!PINCODE
            row!FAX = returnRow!FAX
            row!EMAIL = returnRow!EMAIL
            row!PHONERES = returnRow!PHONERES
            row!MOBILE = returnRow!MOBILE
            row!SNO = returnRow!SNO
            row!advfixwtper = returnRow!advfixwtper
            dtBookedAdvanceView.Rows.Add(row)
            dtBookedAdvanceView.AcceptChanges()
            objGPack.TextClear(Me)
            txtRunNO.Select()
        End If
    End Sub

    Public Sub GetVariableAdv(ByVal OrdNo As String, ByVal AdvAmt As Double, ByVal AdvRate As Double)
        Dim row As DataRow = dtBookedAdvanceView.NewRow
        row("RUNNO") = OrdNo.ToString
        row("AMOUNT") = AdvAmt
        row("FIXEDRATE") = AdvRate
        row("BOOKWT") = Math.Round(AdvAmt / AdvRate, 3)
        row("ADVMODE") = "W"
        dtBookedAdvanceView.Rows.Add(row)
        dtBookedAdvanceView.AcceptChanges()
    End Sub
End Class