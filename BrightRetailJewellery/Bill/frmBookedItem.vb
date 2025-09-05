Imports System.Data.OleDb
Public Class frmBookedItem
    Dim strSql As String
    Public dtBookedAdvanceView As New DataTable
    Public dtReservedItem As New DataTable
    Public REFNOPREFIXAUTO As String = ""
    Dim returnRow As DataRow = Nothing
    Private lock As Boolean
    Dim cmd As OleDbCommand
    Dim WTADV2AMTCURRRATE As Boolean = IIf(GetAdmindbSoftValue("WTADV2AMTCURRRATE", "N") = "Y", True, False)
    Dim WTADV2WTADJ As Boolean = IIf(GetAdmindbSoftValue("WTADV2WTADJ", "N") = "Y", True, False)
    Dim ADV_RATE_METAL As String = GetAdmindbSoftValue("ADV_RATE_METAL", "G")
    Dim POS_BOOK_EMPID As Boolean = IIf(GetAdmindbSoftValue("POS_BOOK_EMPID", "N") = "Y", True, False)
    Dim ADV_RATE_OFFERBASE As Boolean = IIf(GetAdmindbSoftValue("ADV_RATE_OFFERBASE", "N") = "Y", True, False)
    Public Trandate As Date
    Dim GstPer As Decimal
    Dim GstFlag As Boolean
    Dim Round_Gst As String = GetAdmindbSoftValue("ROUNDOFF-GST", "F")

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
            .Add("BALAMOUNT", GetType(Double))
            .Add("GST", GetType(Double))
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
            'WTADV2WTADJ
            .Add("ADVMODE", GetType(String))
        End With
        gridBookedAdvance.DataSource = dtBookedAdvanceView
        With gridBookedAdvance
            .Columns("COMPANYID").Width = 0
            .Columns("RUNNO").Width = txtRunNO.Width + 1
            .Columns("AMOUNT").Width = txtBalance_AMT.Width + 1
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("GST").Width = txtAdvanceGST_AMT.Width + 1
            .Columns("GST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GST").DefaultCellStyle.Format = "0.00"
            .Columns("BALAMOUNT").Width = txtAdvanceAmt_AMT.Width + 1
            .Columns("BALAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("BALAMOUNT").DefaultCellStyle.Format = "0.00"
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
            For cnt As Integer = 9 To gridBookedAdvance.ColumnCount - 1
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
            .Add("EMPID", GetType(Integer))
            .Add("MAXWASTPER", GetType(Double))
            .Add("MAXMCGRM", GetType(Double))
        End With
        gridRes.DataSource = dtReservedItem
        With gridRes
            .Columns("ESTNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ITEMID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Format = "0.000"
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").DefaultCellStyle.Format = "0.00"
            .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
            .Columns("MAXWASTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MAXWASTPER").DefaultCellStyle.Format = "0.00"
            .Columns("MAXMCGRM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MAXMCGRM").DefaultCellStyle.Format = "0.00"
            .Columns("MAXWASTPER").Visible = False
            .Columns("MAXMCGRM").Visible = False
        End With
    End Sub
    Private Function getHelpStr(Optional ByVal ruNo As String = Nothing) As String
        ''strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO') > 0"
        ''strSql += vbCrLf + " DROP VIEW TEMP" & systemId & "_CUSTOMERINFO"

        strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "_CUSTOMERINFO') > 0"
        strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''strSql = " CREATE VIEW TEMP" & systemId & "_CUSTOMERINFO"
        ''strSql += vbCrLf + " AS"
        ''strSql += vbCrLf + " SELECT C.PSNO as SNO,C.BATCHNO,TRANDATE,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX"
        ''strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
        strSql = ""
        strSql += vbCrLf + " SELECT C.PSNO as SNO,C.BATCHNO,TRANDATE,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO FROM " & cnAdminDb & "..PERSONALINFO P INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT COMPANYID,SUBSTRING(RUNNO,6,20)RUNNO "
        strSql += vbCrLf + " ,CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END AS RECEIPT"
        strSql += vbCrLf + " ,CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END AS PAYMENT"
        strSql += vbCrLf + " ,CASE WHEN BALANCE <> 0 THEN BALANCE ELSE NULL END AS BALANCE"
        strSql += vbCrLf + " ,V.TRANDATE,V.PREVILEGEID,V.ACCODE,V.TITLE,V.INITIAL,V.PNAME,V.DOORNO,V.ADDRESS1,V.ADDRESS2,V.ADDRESS3"
        strSql += vbCrLf + " ,V.AREA,V.CITY,V.STATE,V.COUNTRY,V.PINCODE,V.PHONERES,V.MOBILE,V.EMAIL,V.FAX,V.SNO"
        strSql += vbCrLf + " ,V.BATCHNO,RATE,ADVFIXWTPER,ADVMODE,GST"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("

        strSql += vbCrLf + " SELECT COMPANYID,RUNNO"
        'strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END)AS RECEIPT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END)AS PAYMENT"
        'strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END)AS BALANCE"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END)AS RECEIPT"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE 0 END)AS PAYMENT"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT+ISNULL(GSTVAL,0) ELSE -1*(AMOUNT+ISNULL(GSTVAL,0)) END)AS BALANCE"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GSTVAL ELSE -1*GSTVAL END)AS GST"
        strSql += vbCrLf + " ,(SELECT TOP 1 RATE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND ISNULL(CANCEL,'') = '' ORDER BY SNO )AS RATE"
        strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND ISNULL(CANCEL,'') = '' ORDER BY SNO )AS BATCHNO"
        strSql += vbCrLf + " ,(SELECT TOP 1 ADVFIXWTPER FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND ISNULL(CANCEL,'') = '' ORDER BY SNO )AS ADVFIXWTPER"
        strSql += vbCrLf + " ,'A' ADVMODE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
        strSql += vbCrLf + " WHERE TRANTYPE = 'A' AND COMPANYID='" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        If ruNo <> Nothing Then
            If Mid(ruNo, 6, 1) = "A" Then strSql += vbCrLf + " AND  RUNNO= '" & ruNo & "'" Else strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,10) = '" & ruNo & "'"
        End If
        strSql += vbCrLf + " GROUP BY COMPANYID,RUNNO"
        strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) > 0 "

        If WTADV2AMTCURRRATE Then
            strSql += vbCrLf + " union all SELECT COMPANYID,RUNNO"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN RATE*GRSWT ELSE 0 END)AS RECEIPT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN RATE*GRSWT ELSE 0 END)AS PAYMENT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN RATE*GRSWT ELSE -1*RATE*GRSWT END)AS BALANCE"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GSTVAL ELSE -1*GSTVAL END)AS GST"
            strSql += vbCrLf + " ,(SELECT TOP 1 RATE FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND ISNULL(CANCEL,'') = '' ORDER BY SNO )AS RATE"
            strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND ISNULL(CANCEL,'') = '' ORDER BY SNO DESC)AS BATCHNO,O.ADVFIXWTPER,'A' ADVMODE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " WHERE TRANTYPE = 'A'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            If ruNo <> Nothing Then
                If Mid(ruNo, 6, 1) = "A" Then strSql += vbCrLf + " AND  RUNNO= '" & ruNo & "'" Else strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,10) = '" & ruNo & "'"
            End If
            strSql += vbCrLf + " GROUP BY COMPANYID,RUNNO,ADVFIXWTPER"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN RATE*GRSWT ELSE -1*RATE*GRSWT END) > 0 "
        End If

        If WTADV2WTADJ Then
            strSql += vbCrLf + " UNION ALL SELECT COMPANYID,RUNNO"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE 0 END)AS RECEIPT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'P' THEN GRSWT ELSE 0 END)AS PAYMENT"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END)AS BALANCE"
            strSql += vbCrLf + " ,SUM(CASE WHEN RECPAY = 'R' THEN GSTVAL ELSE -1*GSTVAL END)AS GST"
            strSql += vbCrLf + " ,1 AS RATE"
            strSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.RUNNO AND ISNULL(CANCEL,'') = '' ORDER BY SNO DESC)AS BATCHNO,O.ADVFIXWTPER,'W' ADVMODE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " WHERE TRANTYPE = 'A' AND COMPANYID='" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' and AMOUNT =0"
            If ruNo <> Nothing Then
                If Mid(ruNo, 6, 1) = "A" Then strSql += vbCrLf + " AND  RUNNO= '" & ruNo & "'" Else strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,10) = '" & ruNo & "'"
            End If
            strSql += vbCrLf + " GROUP BY COMPANYID,RUNNO,ADVFIXWTPER"
            strSql += vbCrLf + " HAVING SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END) > 0 "
        End If


        strSql += vbCrLf + " )X"
        strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP" & systemId & "_CUSTOMERINFO AS V ON V.BATCHNO = X.BATCHNO"
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
        'value = Val(GetRate(BillDate, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtSAItemId.Text) & "")))
        txtReceived_AMT.Text = Format(Val(returnRow!RECEIPT.ToString), "0.00")
        txtAdjusted_AMT.Text = IIf(Val(returnRow!PAYMENT.ToString) > 0, Format(Val(returnRow!PAYMENT.ToString), "0.00"), Nothing)
        txtBalance_AMT.Text = Format(Val(returnRow!BALANCE.ToString), "0.00")
        If Val(returnRow!GST.ToString) > 0 Then
            GstFlag = True
        End If
        If GST And GstFlag Then
            'Dim GstAmt As Double = (Val(txtBalance_AMT.Text) * GstPer / 100)
            'GstAmt = CalcRoundoffAmt(GstAmt, "H")
            'txtAdvanceGST_AMT.Text = IIf(GstAmt <> 0, Format(GstAmt, "0.00"), Nothing)
            Dim Per As Decimal = 103
            Dim GstAmt As Double
            Dim SGstAmt As Double = (Val(txtBalance_AMT.Text) * (GstPer / 2) / Per)
            Dim CGstAmt As Double = (Val(txtBalance_AMT.Text) * (GstPer / 2) / Per)
            SGstAmt = Math.Round(SGstAmt, 2)
            CGstAmt = Math.Round(CGstAmt, 2)
            SGstAmt = CalcRoundoffAmt(SGstAmt, Round_Gst)
            CGstAmt = CalcRoundoffAmt(CGstAmt, Round_Gst)

            GstAmt = SGstAmt + CGstAmt
            txtAdvanceGST_AMT.Text = IIf(GstAmt <> 0, Format(GstAmt, "0.00"), Nothing)
            Dim TotAmt As Double = Val(txtBalance_AMT.Text)
            TotAmt -= Val(txtAdvanceGST_AMT.Text)
            txtAdvanceAmt_AMT.Text = IIf(TotAmt <> 0, Format(TotAmt, "0.00"), Nothing)
        End If
        txtRateFix.Text = IIf(Val(returnRow!RATE.ToString) <> 0, Format(Val(returnRow!RATE.ToString), "0.00"), Nothing)
        If ADV_RATE_METAL <> "G" Or ADV_RATE_METAL <> "S" Then
            ADV_RATE_METAL = "G"
        End If
        strSql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PURITYID = "
        strSql += " (SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN 91.6 AND 92 "
        If ADV_RATE_METAL <> "N" And Not ADV_RATE_METAL Is Nothing Then strSql += " AND METALID='" & ADV_RATE_METAL & "'"
        strSql += " ORDER BY PURITY DESC)"
        Dim purrate As Double = Val(GetRate(GetEntryDate(GetServerDate), objGPack.GetSqlValue(strSql)))
        If Val(txtRateFix.Text) = 0 Then
            txtRateFix.Text = Format(purrate, "0.00")
        End If
        If ADV_RATE_OFFERBASE Then
            If purrate < Val(txtRateFix.Text) Then
                txtRateFix.Text = Format(purrate, "0.00")
            End If
        End If
        txtName.Text = returnRow!pNAME.ToString
        txtDoorNo.Text = returnRow!DOORNO.ToString
        txtBalance_AMT.Focus()
        txtCompanyid.Text = returnRow!companyid.ToString
        txtAdvMode.Text = returnRow!advmode.ToString
    End Sub

    Private Sub txtRunNO_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRunNO.GotFocus
        If REFNOPREFIXAUTO = "Y" And txtRunNO.Text.Trim = "" Then txtRunNO.Text = "A" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString : SendKeys.Send("{END}")

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
        If txtSalesMan_NUM.Text = "" And POS_BOOK_EMPID = False Then
            LoadSalesMan()
        ElseIf txtSalesMan_NUM.Text <> "" Then
            txtSalesManName.Text = objGPack.GetSqlValue("SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = " & Val(txtSalesMan_NUM.Text) & "")
            If txtSalesManName.Text = Nothing Then
                LoadSalesMan()
            Else
                btnOk.Enabled = False
            End If
        End If
        'btnOk.Enabled = False
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

    Private Sub frmBookedItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If dtBookedAdvanceView.Rows.Count > 0 Then
                If txtSalesMan_NUM.Visible Then
                    txtSalesMan_NUM.Focus()
                Else
                    btnOk.Focus()
                End If
            Else
                btnExit.Focus()
            End If
        End If
    End Sub

    Private Sub frmBookedItem_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridBookedAdvance.ColumnHeadersVisible = False
        gridRes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        If POS_BOOK_EMPID Then
            txtSalesManName.Visible = False
            txtSalesMan_NUM.Visible = False
            Label39.Visible = False
        End If
        GstPer = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE='GT'", , "0").ToString)
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
            If Val(txtBalance_AMT.Text) > Val(returnRow!BALANCE.ToString) Then
                MsgBox("Amount Should not exceed " & Format(Val(returnRow!BALANCE.ToString), "0.00"), MsgBoxStyle.Information)
                txtBalance_AMT.Focus()
                Exit Sub
            End If
            Dim row As DataRow = dtBookedAdvanceView.NewRow
            row("RUNNO") = txtRunNO.Text
            row("AMOUNT") = Val(txtBalance_AMT.Text)
            row("BALAMOUNT") = Val(txtAdvanceAmt_AMT.Text)
            row("GST") = Val(txtAdvanceGST_AMT.Text)
            row("FIXEDRATE") = Val(txtRateFix.Text)
            row("NAME") = txtName.Text
            row("ADDRESS") = txtDoorNo.Text + "," + returnRow!ADDRESS1.ToString

            If Val("" & returnRow!ADVFIXWTPER) > 100 Then
                row("BOOKWT") = Math.Round((Val(txtBalance_AMT.Text) / Val(txtRateFix.Text)) * (Val("" & returnRow!advfixwtper) / 100), 3)
                If WTADV2WTADJ And txtadvmode.text = "W" Then row("BOOKWT") = Math.Round(Val(txtBalance_AMT.Text), 3)
            ElseIf Val("" & returnRow!ADVFIXWTPER) < 100 Then
                Dim madvbalance As Double = Val(txtBalance_AMT.Text) / (Val(returnRow!advfixwtper.ToString) / 100)

                row("BOOKWT") = Math.Round((madvbalance / Val(txtRateFix.Text)), 3)
                If WTADV2WTADJ And txtadvmode.text = "W" Then row("BOOKWT") = Math.Round(Val(txtBalance_AMT.Text), 3)

            Else
                row("BOOKWT") = Math.Round(Val(txtBalance_AMT.Text) / Val(txtRateFix.Text), 3)
                If WTADV2WTADJ And txtadvmode.text = "W" Then row("BOOKWT") = Math.Round(Val(txtBalance_AMT.Text), 3)
            End If
            If WTADV2WTADJ And txtAdvMode.Text = "W" Then row("ADVMODE") = "W"
            row("RECEIPT") = returnRow!RECEIPT
            row("PAYMENT") = returnRow!PAYMENT
            row("TRANDATE") = returnRow!TRANDATE
            row("COMPANYID") = returnRow!COMPANYID
            row!PREVILEGEID = returnRow!PREVILEGEID
            row!ACCODE = returnRow!ACCODE
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

            strSql = "SELECT 1 AS CNT FROM " & cnStockDb & "..ESTISSUE WHERE BATCHNO = '" & returnRow!BATCHNO.ToString & "' "
            If Val(objGPack.GetSqlValue(strSql, "CNT", "")) > 0 Then
                strSql = " SELECT TRANNO ESTNO,'" & returnRow!RUNNO.ToString & "'RUNNO,ITEMID,"
                strSql += vbCrLf + "  (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME,TAGNO,PCS,GRSWT,NETWT," & Val(txtRateFix.Text) & "AS RATE,AMOUNT AS SALVALUE,'" & returnRow!BATCHNO.ToString & "' AS BATCHNO"
                strSql += vbCrLf + "  ,(SELECT EMPID FROM " & cnStockDb & "..BOOKEDITEM_HISTORY WHERE ITEMID=T.ITEMID AND TAGNO=T.TAGNO)EMPID"
                strSql += vbCrLf + "  ,(SELECT TOP 1 MAXWASTPER FROM " & cnStockDb & "..BOOKEDITEM_HISTORY WHERE ITEMID=T.ITEMID AND TAGNO=T.TAGNO)MAXWASTPER"
                strSql += vbCrLf + "  ,(SELECT TOP 1 MAXMCGRM FROM " & cnStockDb & "..BOOKEDITEM_HISTORY WHERE ITEMID=T.ITEMID AND TAGNO=T.TAGNO)MAXMCGRM"
                strSql += vbcrlf + "   FROM " & cnStockDb & "..ESTISSUE AS T"
                strSql += vbcrlf + "  WHERE BATCHNO = '" & returnRow!BATCHNO.ToString & "' "
            Else
                strSql = " SELECT 0 ESTNO,'" & returnRow!RUNNO.ToString & "'RUNNO,ITEMID,"
                strSql += vbCrLf + "  (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME,TAGNO,PCS,GRSWT,NETWT," & Val(txtRateFix.Text) & "AS RATE, SALVALUE,'" & returnRow!BATCHNO.ToString & "' AS BATCHNO"
                strSql += vbCrLf + "  ,(SELECT TOP 1 EMPID FROM " & cnStockDb & "..BOOKEDITEM_HISTORY WHERE ITEMID=T.ITEMID AND TAGNO=T.TAGNO)EMPID"
                strSql += vbCrLf + "  ,(SELECT TOP 1 MAXWASTPER FROM " & cnStockDb & "..BOOKEDITEM_HISTORY WHERE ITEMID=T.ITEMID AND TAGNO=T.TAGNO)MAXWASTPER"
                strSql += vbCrLf + "  ,(SELECT TOP 1 MAXMCGRM FROM " & cnStockDb & "..BOOKEDITEM_HISTORY WHERE ITEMID=T.ITEMID AND TAGNO=T.TAGNO)MAXMCGRM"
                strSql += vbCrLf + "   FROM " & cnAdminDb & "..ITEMTAG AS T"
                strSql += vbCrLf + "  WHERE BATCHNO = '" & returnRow!BATCHNO.ToString & "' AND APPROVAL = 'R'"
            End If
            Dim dt As New DataTable
            dt.Rows.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            For Each ro As DataRow In dt.Rows
                dtReservedItem.ImportRow(ro)
            Next
            dtReservedItem.AcceptChanges()
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

    Private Sub txtSalesMan_NUM_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSalesMan_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSalesMan_NUM.Text = "" Then
                LoadSalesMan()
            ElseIf txtSalesMan_NUM.Text <> "" Then
                txtSalesManName.Text = objGPack.GetSqlValue("SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = " & Val(txtSalesMan_NUM.Text) & "")
                If txtSalesManName.Text = Nothing Then
                    LoadSalesMan()
                Else
                    Me.SelectNextControl(txtSalesMan_NUM, True, True, True, True)
                End If
            End If
        End If
    End Sub

    Private Sub LoadSalesMan()
        strSql = " SELECT EMPID,EMPNAME FROM " & cnAdminDb & "..EMPMASTER "
        strSql += " WHERE ACTIVE = 'Y'"
        Dim row As DataRow = Nothing
        row = BrighttechPack.SearchDialog.Show_R("Select Employee Name", strSql, cn, 1)
        If Not row Is Nothing Then
            txtSalesMan_NUM.Text = row!EMPID.ToString
            txtSalesManName.Text = row!EMPNAME.ToString
            Me.SelectNextControl(txtSalesMan_NUM, True, True, True, True)
        End If
    End Sub

    Private Sub txtSalesManName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSalesManName.GotFocus
        SendKeys.Send("{TAB}")
    End Sub
 
    Private Sub txtBalance_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBalance_AMT.TextChanged
        If GST And GstFlag Then
            'Dim GstAmt As Double = (Val(txtBalance_AMT.Text) * GstPer / 100)
            'GstAmt = CalcRoundoffAmt(GstAmt, "H")
            'txtAdvanceGST_AMT.Text = IIf(GstAmt <> 0, Format(GstAmt, "0.00"), Nothing)
            Dim Per As Decimal = 103

            Dim GstAmt As Double
            Dim SGstAmt As Double = (Val(txtBalance_AMT.Text) * (GstPer / 2) / Per)
            Dim CGstAmt As Double = (Val(txtBalance_AMT.Text) * (GstPer / 2) / Per)
            SGstAmt = Math.Round(SGstAmt, 2)
            CGstAmt = Math.Round(CGstAmt, 2)
            SGstAmt = CalcRoundoffAmt(SGstAmt, Round_Gst)
            CGstAmt = CalcRoundoffAmt(CGstAmt, Round_Gst)

            GstAmt = SGstAmt + CGstAmt
            txtAdvanceGST_AMT.Text = IIf(GstAmt <> 0, Format(GstAmt, "0.00"), Nothing)
            Dim TotAmt As Double = Val(txtBalance_AMT.Text)
            TotAmt -= Val(txtAdvanceGST_AMT.Text)
            txtAdvanceAmt_AMT.Text = IIf(TotAmt <> 0, Format(TotAmt, "0.00"), Nothing)
        End If
    End Sub
End Class