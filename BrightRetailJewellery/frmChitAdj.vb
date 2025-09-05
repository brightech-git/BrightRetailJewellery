Imports System.Data.OleDb
Public Class frmChitAdj
    Dim strSql As String
    Dim cmd As OleDbCommand

    Dim chitMainDb As String = Nothing
    Dim chitTrandb As String = Nothing
    Public dtChitAdj As New DataTable
    Public dtReservedItem As New DataTable
    Dim chitEdit As Boolean = False
    Dim autoPost As Boolean = True
    Public ChitPaymode As String = "SS"
    Public BillDate As Date
    Public Tranflag As String = ""
    Dim dtsoftkeyss As New DataTable
    Dim mPartweightadj As Boolean = False
    Dim mWeight As Decimal = 0
    Dim mBalweight As Decimal = 0
    Dim mAdjweight As Decimal = 0
    Dim mSettrate As Decimal = 0
    Dim mBonweight As Decimal = 0
    Dim Isclosed As String = "Y"
    Dim DaysinYear As Decimal = GetAdmindbSoftValue("DAYSINYEAR", 365)
    Dim PARTPREWTBONUS As Boolean = IIf(GetAdmindbSoftValue("PART_PREADICAL_BONUS", "N") = "Y", True, False)
    Dim IS_USERLEVELPWD As Boolean = IIf(GetAdmindbSoftValue("USERLEVELPWD", "N") = "Y", True, False)
    Dim INTER_TRF_CHITCLOSE As String = GetAdmindbSoftValue("INTER_TRF_CHITCLOSE", "N")
    Public InsBonus As Boolean = True
    Dim AuthPwdPass As Boolean = False
    Dim GstRecCode As String = GetAdmindbSoftValue("GSTACCODE", "")
    Dim GstRecAcc() As String
    Dim SCode As String
    Dim CCode As String
    Dim ICode As String
    Dim ScGstRecCode As String = GetAdmindbSoftValue("GSTACCODE_CHIT", "")
    Dim ScGstRecAcc() As String
    Dim SchemeSCode As String
    Dim SchemeCCode As String
    Dim SchemeICode As String
    Dim GstCalc As String = "I"
    Dim CLIENTID_AS_SLIPNO As Boolean = IIf(GetAdmindbSoftValue("CLIENTID_AS_SLIPNO", "N") = "Y", True, False)
    Dim SPECIFICBONUS As String = GetAdmindbSoftValue("SPECIFICBONUS", "")
    Dim Partlybonusschid As String = GetAdmindbSoftValue("PARTLYBONUSSCHID", "")
    Dim chitClose_mdateDays As String = GetAdmindbSoftValue("CHIT_CLOSE_MDATE_PREDAYS", "0")
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "0")
    Dim _chitNoOffLock As Boolean = False

    Public Sub New(ByVal starting As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Dim CTLIDS As String = "" 'Where ctlid like 'CHIT%' or CTLID IN('OTHERSMODE','ACTUALRECEIPT','MONTHBONUS','METALRATE')"
        dtsoftkeyss = GetAdmindbSoftValueAll(CTLIDS)

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        gridCHITCard.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridCHITCardTotal.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        ' Add any initialization after the InitializeComponent() call.
        ''CHEQUE
        strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE = 'C' ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbCHITtCardType_MAN)

        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITEDIT", "N").ToUpper = "Y" Then
            chitEdit = True
        End If

        With dtChitAdj.Columns
            .Add("CARDTYPE", GetType(String))
            .Add("SLIPNO", GetType(String))
            .Add("GRPCODE", GetType(String))
            .Add("REGNO", GetType(Integer))
            .Add("AMOUNT", GetType(Double))
            .Add("GST", GetType(Double))
            .Add("GIFT", GetType(Double))
            .Add("PRIZE", GetType(Double))
            .Add("BONUS", GetType(Double))
            .Add("DEDUCTION", GetType(Double))
            .Add("TOTAL", GetType(Double))
            .Add("WEIGHT", GetType(Double))
            .Add("BONUSWT", GetType(Double))
            .Add("BALWEIGHT", GetType(Double))
            .Add("ADJWEIGHT", GetType(Double))
            .Add("ISPARTWT", GetType(String))
            .Add("ISCLOSE", GetType(String))
            .Add("NAME", GetType(String))
            .Add("MOBILENO", GetType(String))
        End With
        gridCHITCard.DataSource = dtChitAdj
        FormatGridColumns(gridCHITCard)
        StyleGridCheque(gridCHITCard)
        Dim dtGridChitTotal As New DataTable
        dtGridChitTotal = dtChitAdj.Copy
        gridCHITCardTotal.DataSource = dtGridChitTotal
        dtGridChitTotal.Rows.Clear()
        dtGridChitTotal.Rows.Add()
        dtGridChitTotal.Rows(0).Item("CARDTYPE") = "Total"
        With gridCHITCardTotal
            .DataSource = dtGridChitTotal
            For Each col As DataGridViewColumn In gridCHITCard.Columns
                With gridCHITCardTotal.Columns(col.Name)
                    .Visible = col.Visible
                    .Width = col.Width
                    .DefaultCellStyle = col.DefaultCellStyle
                End With
            Next
            .Columns("WEIGHT").Visible = False
            .Columns("BONUSWT").Visible = False
            .Columns("BALWEIGHT").Visible = False
            .Columns("ADJWEIGHT").Visible = False
            .Columns("ISPARTWT").Visible = False
            .Columns("ISCLOSE").Visible = False
            .Columns("NAME").Visible = False
            .Columns("MOBILENO").Visible = False
        End With
        FormatGridColumns(gridCHITCardTotal)
        StyleGridCheque(gridCHITCardTotal)
    End Sub

    Private Sub StyleGridCheque(ByVal grid As DataGridView)
        gridCHITCardTotal.DefaultCellStyle.SelectionBackColor = grpCHIT.BackgroundColor
        With grid
            .Columns("CARDTYPE").Width = 0
            .Columns("SLIPNO").Width = txtChitslipNo.Width + 1

            .Columns("GRPCODE").Width = txtCHITCardGrpCode.Width + 1
            .Columns("REGNO").Width = txtCHITCardRegNo_NUM.Width + 1
            .Columns("AMOUNT").Width = txtCHITCardAmount_AMT.Width + 1
            .Columns("GST").Width = txtCHITGST_AMT.Width + 1
            .Columns("GIFT").Width = txtCHITCardGift_AMT.Width + 1
            .Columns("PRIZE").Width = txtCHITCardPrize_AMT.Width + 1
            .Columns("BONUS").Width = txtCHITCardBonus_AMT.Width + 1
            .Columns("DEDUCTION").Width = txtCHITCardDeduction_AMT.Width + 1
            .Columns("TOTAL").Width = txtCHITCardTotal_AMT.Width
            For cnt As Integer = 11 To grid.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            .Columns("CARDTYPE").Visible = False
        End With
    End Sub

    Private Sub frmChidAdj_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            CalcTotal()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmChidAdj_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtCHITCardTotal_AMT.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmChidAdj_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If chitdbchk() = False Then Me.Close() : Exit Sub
        objGPack.TextClear(Me)
        If CLIENTID_AS_SLIPNO Then
            lblSlipNo.Text = "C.ID"
        Else
            lblSlipNo.Text = "Slip"
        End If
        lblCardLost.Text = ""
        lblPDCCheque.Text = ""
        lblRemarks.Text = ""
        lblAddress.Text = ""
        lblWeightSchemeDetail.Text = ""
        lblSetwt.Visible = False : txtSettleWt.Visible = False
        If Not chitEdit Then
            txtCHITCardGift_AMT.ReadOnly = True
            txtCHITCardPrize_AMT.ReadOnly = True
            txtCHITCardBonus_AMT.ReadOnly = True
            txtCHITCardDeduction_AMT.ReadOnly = True
        End If
        txtCHITCardAmount_AMT.ReadOnly = True
        CalcTotal()
        If GstRecCode.Contains(",") Then
            GstRecAcc = GstRecCode.Split(",")
            If GstRecAcc.Length <> 3 Then
                MsgBox("GST Advance Receive Account not set Properly.", MsgBoxStyle.Information)
            Else
                SCode = GstRecAcc(0).ToString
                CCode = GstRecAcc(1).ToString
                ICode = GstRecAcc(2).ToString
            End If
        End If
        If ScGstRecCode.Contains(",") Then
            ScGstRecAcc = ScGstRecCode.Split(",")
            If ScGstRecAcc.Length <> 3 Then
                MsgBox("GST Chit Adjustments Account not set Properly.", MsgBoxStyle.Information)
            Else
                SchemeSCode = ScGstRecAcc(0).ToString
                SchemeCCode = ScGstRecAcc(1).ToString
                SchemeICode = ScGstRecAcc(2).ToString
            End If
        End If
        If SPECIFICBONUS <> "" Then
            lblBonusType.Visible = True
            cmbBonusType.Visible = True
            Dim STRSPECIFICBONUS() As String = Nothing
            STRSPECIFICBONUS = SPECIFICBONUS.Split(",")

            If STRSPECIFICBONUS.Length > 0 Then
                Dim dttemp As New DataTable
                With dttemp
                    .Columns.Add("BONUSTYPE", GetType(String))
                    .Columns.Add("BONUSPER", GetType(Double))
                End With
                For cnt As Integer = 0 To STRSPECIFICBONUS.Length - 1
                    Dim strspesplit() As String = Nothing
                    strspesplit = STRSPECIFICBONUS(cnt).Split(":")
                    If strspesplit.Length = 2 Then
                        With dttemp
                            Dim ro As DataRow = dttemp.NewRow
                            ro!BONUSTYPE = strspesplit(0).ToString
                            ro!BONUSPER = strspesplit(1).ToString
                            dttemp.Rows.Add(ro)
                            dttemp.AcceptChanges()
                        End With
                    End If
                Next
                cmbBonusType.DataSource = Nothing
                If dttemp.Rows.Count > 0 Then
                    cmbBonusType.DataSource = dttemp
                    cmbBonusType.ValueMember = "BONUSPER"
                    cmbBonusType.DisplayMember = "BONUSTYPE"
                Else
                    lblBonusType.Visible = False
                    cmbBonusType.Visible = False
                End If
            Else
                lblBonusType.Visible = False
                cmbBonusType.Visible = False
            End If
        Else
            lblBonusType.Visible = False
            cmbBonusType.Visible = False
        End If
    End Sub
    Public Function chitdbchk() As Boolean
        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITDB", "N").ToUpper <> "Y" Then
            MsgBox("SCHEME Transaction provision not enabled in this pack", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            Return False
            Exit Function
        End If
        chitMainDb = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITDBPREFIX", "") + "SAVINGS"
        chitTrandb = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITDBPREFIX", "") + "SH0708"
        If Not objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & chitMainDb & "'").Length > 0 Then
            MsgBox("SCHEME main database not found", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            Return False
            Exit Function
        End If
        If Not objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & chitTrandb & "'").Length > 0 Then
            MsgBox("SCHEME transaction database not found", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            Return False
            Exit Function
        End If
        Return True
    End Function
    Private Sub txtChitCardTotal_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCHITCardTotal_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''Validation
            If objGPack.Validator_Check(Me) Then Exit Sub
            If CheckGroupCode() Then Exit Sub
            If CheckRegNo() Then Exit Sub
            If CheckClose() Then Exit Sub
            If CheckAuth() Then Exit Sub
            If CheckNoOffer() = True And _chitNoOffLock = False Then
                If MsgBox("No Offer Item available in sales" & vbCrLf & vbCrLf & " Do You Wish to Continue without bonus ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    _chitNoOffLock = True
                Else
                    txtCHITCardBonus_AMT.Text = ""
                    Exit Sub
                End If
            End If
            If SPECIFICFORMAT = 1 And _chitNoOffLock = True Then
                txtCHITCardBonus_AMT.Text = ""
            End If
            If Val(txtCHITCardTotal_AMT.Text) = 0 Then
                Exit Sub
            End If
            AuthPwdPass = False
            _chitNoOffLock = False
            ''Insertion
            If txtCHITCardRowIndex.Text = "" Then
                Dim ro As DataRow = dtChitAdj.NewRow
                ro!CARDTYPE = cmbCHITtCardType_MAN.Text
                ro!SLIPNO = txtChitslipNo.Text
                ro!GRPCODE = txtCHITCardGrpCode.Text
                ro!REGNO = IIf(Val(txtCHITCardRegNo_NUM.Text) <> 0, txtCHITCardRegNo_NUM.Text, DBNull.Value)
                ro!AMOUNT = IIf(Val(txtCHITCardAmount_AMT.Text) <> 0, txtCHITCardAmount_AMT.Text, DBNull.Value)
                ro!GST = IIf(Val(txtCHITGST_AMT.Text) <> 0, txtCHITGST_AMT.Text, DBNull.Value)
                ro!GIFT = IIf(Val(txtCHITCardGift_AMT.Text) <> 0, txtCHITCardGift_AMT.Text, DBNull.Value)
                ro!PRIZE = IIf(Val(txtCHITCardPrize_AMT.Text) <> 0, txtCHITCardPrize_AMT.Text, DBNull.Value)
                ro!BONUS = IIf(Val(txtCHITCardBonus_AMT.Text) <> 0, txtCHITCardBonus_AMT.Text, DBNull.Value)
                ro!DEDUCTION = IIf(Val(txtCHITCardDeduction_AMT.Text) <> 0, txtCHITCardDeduction_AMT.Text, DBNull.Value)
                ro!TOTAL = IIf(Val(txtCHITCardTotal_AMT.Text) <> 0, txtCHITCardTotal_AMT.Text, DBNull.Value)
                ro!WEIGHT = Math.Round(mWeight, 3)
                ro!BONUSWT = Math.Round(mBonweight, 3)
                ro!BALWEIGHT = Math.Round(mBalweight, 3)
                ro!ADJWEIGHT = Math.Round(mAdjweight, 3)
                ro!isclose = Isclosed
                ro!ispartwt = IIf(mPartweightadj, "Y", "N")
                ro!NAME = lblPname.Text
                ro!MOBILENO = lblMobileNo.Text
                dtChitAdj.Rows.Add(ro)
                dtChitAdj.AcceptChanges()
            Else
                ''UPDATE
                With gridCHITCard.Rows(Val(txtCHITCardRowIndex.Text))
                    .Cells("CARDTYPE").Value = cmbCHITtCardType_MAN.Text
                    .Cells("GRPCODE").Value = txtCHITCardGrpCode.Text
                    .Cells("REGNO").Value = IIf(Val(txtCHITCardRegNo_NUM.Text) <> 0, txtCHITCardRegNo_NUM.Text, DBNull.Value)
                    .Cells("AMOUNT").Value = IIf(Val(txtCHITCardAmount_AMT.Text) <> 0, txtCHITCardAmount_AMT.Text, DBNull.Value)
                    .Cells("GST").Value = IIf(Val(txtCHITGST_AMT.Text) <> 0, txtCHITGST_AMT.Text, DBNull.Value)
                    .Cells("GIFT").Value = IIf(Val(txtCHITCardGift_AMT.Text) <> 0, txtCHITCardGift_AMT.Text, DBNull.Value)
                    .Cells("PRIZE").Value = IIf(Val(txtCHITCardPrize_AMT.Text) <> 0, txtCHITCardPrize_AMT.Text, DBNull.Value)
                    .Cells("BONUS").Value = IIf(Val(txtCHITCardBonus_AMT.Text) <> 0, txtCHITCardBonus_AMT.Text, DBNull.Value)
                    .Cells("DEDUCTION").Value = IIf(Val(txtCHITCardDeduction_AMT.Text) <> 0, txtCHITCardDeduction_AMT.Text, DBNull.Value)
                    .Cells("TOTAL").Value = IIf(Val(txtCHITCardTotal_AMT.Text) <> 0, txtCHITCardTotal_AMT.Text, DBNull.Value)
                    .Cells("WEIGHT").Value = Math.Round(mWeight, 3)
                    .Cells("BONUSWT").Value = Math.Round(mBonweight, 3)
                    .Cells("BALWEIGHT").Value = Math.Round(mBalweight, 3)
                    .Cells("ADJWEIGHT").Value = Math.Round(mAdjweight, 3)
                    .Cells("ISCLOSE").Value = Isclosed
                    .Cells("ISPARTWT").Value = IIf(mPartweightadj, "Y", "N")
                    .Cells("NAME").Value = lblPname.Text
                    .Cells("MOBILENO").Value = lblMobileNo.Text
                End With
            End If
            ''CLEAR
            CalcTotal()
            objGPack.TextClear(Me)
            lblWeightSchemeDetail.Text = ""
            lblBonusDeduction.Text = ""
            'lblWeightSchemeDetail.Visible = False
            cmbCHITtCardType_MAN.Focus()
        Else
            e.Handled = True
        End If
    End Sub

    Private Function CalcReceiptAmount(ByVal flagActualReceipt As Boolean) As Double
        Dim ReceivedAmount As Double = Nothing
        If flagActualReceipt Then
            strSql = " SELECT  ISNULL(SUM(CASE WHEN SYN = 'RECEIPT' THEN AMOUNT ELSE -1*AMOUNT END),0) AS AMOUNT,"
            strSql += " SCHEMENAME,'RECEIPT' AS SYN "
            strSql += " FROM "
            strSql += " ("
            strSql += " SELECT SUM(AMOUNT)AS AMOUNT"
            strSql += " ,S.SCHEMENAME,'RECEIPT'AS SYN "
            strSql += " FROM  " & chitTrandb & "..SCHEMETRAN AS T "
            strSql += " INNER JOIN " & chitMainDb & "..SCHEMEMAST AS M "
            strSql += " ON T.REGNO = M.REGNO AND T.GROUPCODE = M.GROUPCODE "
            strSql += " INNER JOIN " & chitMainDb & "..SCHEME AS S "
            strSql += " ON S.SCHEMEID = M.SCHEMEID "
            strSql += " WHERE T.GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND T.REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & "  AND ISNULL(T.CANCEL,'') = '' AND T.CHEQUERETDATE IS NULL GROUP BY S.SCHEMENAME "
            strSql += " UNION ALL"
            strSql += " SELECT SUM(CASHINCENTIVE+RDEOINCENTIVE) AS AMOUNT,SCHEMENAME,SYN "
            strSql += " FROM "
            strSql += " 	( "
            strSql += " 	SELECT S.SCHEMENAME AS SCHEMENAME"
            strSql += " 	,ISNULL(SUM(CASE WHEN IMODE = 'C' THEN IAMOUNT ELSE 0 END),0)AS CASHINCENTIVE"
            strSql += " 	,ISNULL(SUM(CASE WHEN IMODE = 'D' OR IMODE = 'E' OR IMODE = 'R' OR IMODE = 'O' THEN IAMOUNT ELSE 0 END),0)AS RDEOINCENTIVE"
            strSql += " 	,'INCENTIVE' AS SYN  "
            strSql += " 	FROM " & chitMainDb & "..SCHEMEINCENTIVE AS I "
            strSql += " 	INNER JOIN " & chitMainDb & "..SCHEMEMAST AS M "
            strSql += " 	  ON I.REGNO = M.REGNO AND I.GROUPCODE = M.GROUPCODE  "
            strSql += " 	INNER JOIN " & chitMainDb & "..SCHEME AS S "
            strSql += " 	  ON S.SCHEMEID = M.SCHEMEID 	"
            strSql += " 	WHERE I.GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND I.REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " GROUP BY S.SCHEMENAME"
            strSql += " 	)AS Y GROUP BY Y.SCHEMENAME,Y.SYN"
            strSql += " ) Z GROUP BY Z.SCHEMENAME"
        Else
            strSql = " SELECT SUM(AMOUNT) AS AMOUNT "
            strSql += " FROM " & chitTrandb & "..SCHEMETRAN WHERE "
            strSql += " GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
        End If
        ReceivedAmount = Val(objGPack.GetSqlValue(strSql))
        strSql = "SELECT CTLTEXT FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID = 'OTHERSMODE'"
        If objGPack.GetSqlValue(strSql).ToUpper = "Y" Then
            'If GetAdmindbSoftValuefromDt(dtsoftkeyss, "OTHERSMODE", "N").ToUpper <> "Y" Then
            strSql = " SELECT SUM(AMOUNT) AS OTHERAMT FROM " & chitTrandb & "..SCHEMECOLLECT "
            strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
            strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND MODEPAY = 'O'"
            strSql += " AND ISNULL(CANCEL,'') = ''"
            ReceivedAmount -= Val(objGPack.GetSqlValue(strSql))
        End If
        Return ReceivedAmount
    End Function
    Private Function CalcPrizeAmountNew() As Double
        strSql = "  SELECT ISNULL(DRAWSCHEME,'')DRAWSCHEME FROM " & chitMainDb & "..SCHEME "
        strSql += "  WHERE SCHEMEID IN "
        strSql += "     ( "
        strSql += "     SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST "
        strSql += "     WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
        strSql += "     AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
        strSql += "     )"
        Dim drawScheme As String = objGPack.GetSqlValue(strSql)
        If UCase(drawScheme) = "Y" Then
            strSql = " SELECT SUM(PRIZEVALUE)PRIZEVALUE FROM ("
            strSql += " SELECT "
            strSql += " (SELECT PRIZEVALUE FROM " & chitMainDb & "..PRIZEMAST WHERE PRIZEID = P.PRIZEID)AS PRIZEVALUE"
            strSql += " FROM " & chitMainDb & "..CUSTOMERPRIZE P"
            strSql += " WHERE GROUPCODE  = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
            strSql += " )X"
            Return Val(objGPack.GetSqlValue(strSql))
        End If
    End Function
    Private Function CalcPrizeAmount() As Double
        strSql = "  SELECT ISNULL(DRAWSCHEME,'')DRAWSCHEME FROM " & chitMainDb & "..SCHEME "
        strSql += "  WHERE SCHEMEID IN "
        strSql += "     ( "
        strSql += "     SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST "
        strSql += "     WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
        strSql += "     AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
        strSql += "     )"
        Dim drawScheme As String = objGPack.GetSqlValue(strSql)
        strSql = " SELECT "
        strSql += " (SELECT PRIZENAME FROM " & chitMainDb & "..PRIZEMAST WHERE PRIZEID = P.PRIZEID)"
        strSql += "  AS PRIZENAME,"
        strSql += " (SELECT PRIZEVALUE FROM " & chitMainDb & "..PRIZEMAST WHERE PRIZEID = P.PRIZEID)"
        If UCase(drawScheme) = "Y" Then
            strSql += " - ISNULL((SELECT ISNULL(SUM(AMOUNT),0) AS AMOUNT"
            strSql += " FROM " & chitTrandb & "..SCHEMETRAN WHERE "
            strSql += " GROUPCODE  = '" & txtCHITCardGrpCode.Text & "' "
            strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
            strSql += " AND ISNULL(CANCEL,'') = '' "
            strSql += " AND ISNULL(CHEQUERETDATE,'')=''),0)"
        End If
        strSql += " AS PRIZEVALUE,"
        strSql += " CONVERT(VARCHAR(12),PRIZEDATE,103) AS PRIZEDATE FROM " & chitMainDb & "..CUSTOMERPRIZE P"
        strSql += " WHERE GROUPCODE  = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
        Return Val(objGPack.GetSqlValue(strSql))
    End Function

    Private Function CalcBonus(Optional ByVal bonustype As String = "A") As Double
        Dim bonus As Double = Nothing
        Dim mSchid As Integer = objGPack.GetSqlValue("SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text))

        strSql = " SELECT TOP 1 I.BONUS"
        strSql += " FROM " & chitMainDb & "..INSAMOUNT I "
        strSql += " INNER JOIN " & chitMainDb & "..SCHEMEMAST M "
        strSql += "   ON M.SCHEMEID = I.SCHEMEID AND I.GROUPCODE = '" & txtCHITCardGrpCode.Text & "'"
        strSql += "   AND I.AMOUNT = (SELECT TOP 1 AMOUNT+ISNULL(TAX,0) FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ")"
        bonus = Val(objGPack.GetSqlValue(strSql))

        Dim Isbonuswt As Boolean = IIf(bonustype = "W", True, False)
        '
        strSql = " SELECT ISNULL(ANNUALINTCAL,'') AS ANNUALINTCAL FROM " & chitMainDb & "..SCHEME "
        strSql += " WHERE SCHEMEID =" & mSchid
        If objGPack.GetSqlValue(strSql).ToUpper = "Y" Then
            strSql = " SELECT ISNULL(INTEREST,0) AS INTEREST FROM " & chitMainDb & "..SCHEME "
            strSql += " WHERE SCHEMEID IN "
            strSql += "       (SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST "
            strSql += "        WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
            strSql += "        AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ")"
            Dim interest As Double = Val(objGPack.GetSqlValue(strSql))

            strSql = " SELECT ISNULL(INTINSNO,0) AS INTINSNO FROM " & chitMainDb & "..SCHEME "
            strSql += " WHERE SCHEMEID IN "
            strSql += "       (SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST "
            strSql += "        WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
            strSql += "        AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ")"
            Dim Intinsno As Double = Val(objGPack.GetSqlValue(strSql))

            Dim BONUSSTARTDATE As Date = Nothing
            Dim PENDINGWT As Decimal = 0
            Dim PARTSETTLED As Boolean = False
            If Isbonuswt Then
                strSql = " SELECT TOP 1 TRANDATE,TOTALWEIGHT-WEIGHT AS BALWEIGHT FROM " & chitTrandb & "..SCHEMEADJTRAN "
                strSql += " WHERE GROUPCODE  = '" & txtCHITCardGrpCode.Text & "' "
                strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " "
                strSql += " AND ISNULL(CANCEL,'') = '' "
                strSql += " ORDER BY TRANDATE DESC"

                Dim sadjrow As DataRow = GetSqlRow(strSql, cn)
                If Not sadjrow Is Nothing Then
                    BONUSSTARTDATE = sadjrow.Item(0)
                    PENDINGWT = Val(sadjrow.Item(1).ToString)
                    PARTSETTLED = True
                End If
            End If
            'strSql = " SELECT count(*) as maxins FROM " & chitTrandb & "..SCHEMETRAN "
            'strSql += " WHERE GROUPCODE  = '" & txtCHITCardGrpCode.Text & "' "
            'strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " "
            'strSql += " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
            'If PARTSETTLED Then strSql += " AND RDATE >= '" & BONUSSTARTDATE & "'"
            'Dim Paidinsno As Double = Val(objGPack.GetSqlValue(strSql))
            'If Paidinsno < Intinsno Then Return 0

            Dim NetDays As Integer = 0
            Dim MBon As Double = 0, TotBonus As Double = 0
            strSql = " SELECT RDATE,AMOUNT ,WEIGHT FROM " & chitTrandb & "..SCHEMETRAN "
            strSql += " WHERE GROUPCODE  = '" & txtCHITCardGrpCode.Text & "' "
            strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " "
            strSql += " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
            If PARTSETTLED Then strSql += " AND RDATE >= '" & BONUSSTARTDATE & "'"
            strSql += " ORDER BY INSTALLMENT"
            Dim dtAnnInt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAnnInt)


            Dim mDaysinYear As Decimal = DaysinYear * 100
            If PARTSETTLED Then
                If PARTPREWTBONUS = False Then
                    NetDays = PENDINGWT * DateDiff(DateInterval.Day, BONUSSTARTDATE, Today.Date)
                    TotBonus = NetDays * Val(interest / mDaysinYear)
                Else
                    strSql = " SELECT RDATE,AMOUNT ,WEIGHT FROM " & chitTrandb & "..SCHEMETRAN "
                    strSql += " WHERE GROUPCODE  = '" & txtCHITCardGrpCode.Text & "' "
                    strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " "
                    strSql += " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
                    If PARTSETTLED Then strSql += " AND RDATE <= '" & BONUSSTARTDATE & "'"
                    strSql += " ORDER BY INSTALLMENT"
                    Dim dtAnnIntpart As New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtAnnIntpart)

                    For Each ro As DataRow In dtAnnIntpart.Rows
                        Dim mintdate As DateTime = DateAdd(DateInterval.Month, (-1) * Intinsno, BONUSSTARTDATE)
                        If ro.Item("RDATE") <= mintdate Then

                        Else
                            Dim startdate As DateTime = DateAdd(DateInterval.Month, -1 * Intinsno, Today.Date)
                            If ro.Item("RDATE") <= startdate Then
                                If Isbonuswt Then
                                    NetDays = Val(ro.Item("WEIGHT").ToString) *
                                    DateDiff(DateInterval.Day, ro.Item("RDATE"), Today.Date)
                                    MBon = NetDays * Val(interest / mDaysinYear)
                                    If MBon > 0 Then TotBonus = TotBonus + (NetDays * Val(interest / mDaysinYear))
                                Else
                                    NetDays = Val(ro.Item("AMOUNT").ToString) *
                                                        DateDiff(DateInterval.Day, ro.Item("RDATE"), Today.Date)
                                    MBon = NetDays * Val(interest / mDaysinYear)
                                    If MBon > 0 Then TotBonus = TotBonus + (NetDays * Val(interest / mDaysinYear))
                                End If
                            End If
                        End If
                    Next

                End If

            End If
            If bonustype = "F" Then
                Dim Totrecdamt As Double = Val(dtAnnInt.Compute("sum(AMOUNT)", Nothing).ToString)
                TotBonus = Totrecdamt * (interest / 100)
                Return TotBonus
            End If
            For Each ro As DataRow In dtAnnInt.Rows
                Dim mintdate As DateTime = DateAdd(DateInterval.Month, -1 * Intinsno, Today.Date)
                If ro.Item("RDATE") <= mintdate Then
                    If Isbonuswt Then
                        NetDays = Val(ro.Item("WEIGHT").ToString) *
                        DateDiff(DateInterval.Day, ro.Item("RDATE"), Today.Date)
                        MBon = NetDays * Val(interest / mDaysinYear)
                        If MBon > 0 Then TotBonus = TotBonus + (NetDays * Val(interest / mDaysinYear))
                    Else
                        NetDays = Val(ro.Item("AMOUNT").ToString) *
                                            DateDiff(DateInterval.Day, ro.Item("RDATE"), Today.Date)
                        MBon = NetDays * Val(interest / mDaysinYear)
                        If MBon > 0 Then TotBonus = TotBonus + (NetDays * Val(interest / mDaysinYear))
                    End If

                End If
            Next
            bonus = TotBonus
        End If
        Return bonus
    End Function

    Private Function CalcGst() As Double
        Dim Gst As Double = Nothing
        strSql = " SELECT ISNULL(SUM(ISNULL(TAX,0)),0)"
        strSql += " FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text)
        strSql += " AND ISNULL(CANCEL,'')='' AND ISNULL(CHEQUERETDATE,'')=''"
        Gst = Val(objGPack.GetSqlValue(strSql))
        Return Gst
    End Function
    Private Function CalcBonusSlab() As Double
        Dim bonus As Double = Nothing
        Dim interest As Double
        Dim Intinsno As Double
        Dim STARTDATE As Date = Nothing
        Dim ENDDATE As Date = Nothing
        Dim mSchid As Integer = objGPack.GetSqlValue("SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text))
        'strSql = " SELECT TOP 1 I.BONUS"
        'strSql += " FROM " & chitMainDb & "..INSAMOUNT I "
        'strSql += " INNER JOIN " & chitMainDb & "..SCHEMEMAST M "
        'strSql += "   ON M.SCHEMEID = I.SCHEMEID AND I.GROUPCODE = '" & txtCHITCardGrpCode.Text & "'"
        'strSql += "   AND I.AMOUNT = (SELECT TOP 1 AMOUNT FROM " & chitTrandb & "..SCHEMECOLLECT WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ")"
        'bonus = Val(objGPack.GetSqlValue(strSql))
        ''
        strSql = " SELECT ISNULL(ANNUALINTCAL,'') AS ANNUALINTCAL,ISNULL(INTEREST,0) AS INTEREST,ISNULL(INTINSNO,0) AS INTINSNO FROM " & chitMainDb & "..SCHEME "
        strSql += " WHERE SCHEMEID =" & mSchid
        Dim drsch As DataRow
        drsch = GetSqlRow(strSql, cn)
        If drsch Is Nothing Then Return Nothing
        If drsch.Item(0).ToString = "Y" Then
            interest = Val(drsch.Item(1).ToString)
            Intinsno = Val(drsch.Item(1).ToString)
        End If
        If interest = 0 Then Return 0
        strSql = " SELECT TOP 1 RDATE FROM " & chitTrandb & "..SCHEMETRAN "
        strSql += " WHERE GROUPCODE  = '" & txtCHITCardGrpCode.Text & "' "
        strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " "
        strSql += " AND ISNULL(CANCEL,'') = '' "
        strSql += " ORDER BY RDATE "
        Dim sadjrow As DataRow = GetSqlRow(strSql, cn)
        If Not sadjrow Is Nothing Then STARTDATE = sadjrow.Item(0)
        strSql = " SELECT TOP 1 RDATE FROM " & chitTrandb & "..SCHEMETRAN "
        strSql += " WHERE GROUPCODE  = '" & txtCHITCardGrpCode.Text & "' "
        strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " "
        strSql += " AND ISNULL(CANCEL,'') = '' "
        strSql += " ORDER BY RDATE DESC"
        Dim sadjrowx As DataRow = GetSqlRow(strSql, cn)
        If Not sadjrowx Is Nothing Then ENDDATE = sadjrowx.Item(0)


        strSql = " SELECT RDATE,AMOUNT ,WEIGHT FROM " & chitTrandb & "..SCHEMETRAN "
        strSql += " WHERE GROUPCODE  = '" & txtCHITCardGrpCode.Text & "' "
        strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " "
        strSql += " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
        'If PARTSETTLED Then strSql += " AND RDATE >= '" & BONUSSTARTDATE & "'"
        strSql += " ORDER BY INSTALLMENT"
        Dim dtAnnInt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAnnInt)
        Dim Rrdate As DateTime = STARTDATE
        Dim Prdate As DateTime
        Dim PrAmt As Decimal
        bonus = 0
        Dim Incmonth As Integer = DateDiff(DateInterval.Month, STARTDATE, ENDDATE)
        For ii As Integer = 0 To Incmonth
            Prdate = Rrdate
            Rrdate = Rrdate.AddMonths(1)
            If Today.Date < Rrdate Then Rrdate = Today.Date
            PrAmt = dtAnnInt.Compute("sum(AMOUNT)", "RDATE <='" & Prdate & "'")
            'strSql = "select datediff(dd,dateadd(dd, 1-day('" & Rrdate & "'),'" & Rrdate & "'), dateadd(m,1,dateadd(dd, 1-day('" & Rrdate & "'),'" & Rrdate & "')))"
            Dim mintday As Integer
            mintday = DateDiff(DateInterval.Day, Prdate, Rrdate) 'objGPack.GetSqlValue(strSql)
            bonus += Math.Round((mintday / DaysinYear) * (PrAmt * (interest / 100)), 2)

        Next
        Dim Excdays As Integer = DateDiff(DateInterval.Day, Rrdate, Today.Date)
        bonus += (Excdays / DaysinYear) * (PrAmt * (interest / 100))
        Return bonus
    End Function




    Private Function CalcMonthBonus() As Double
        Dim bonus As Double = Nothing
        strSql = " SELECT ISNULL(SUM(ISNULL(BONUSAMOUNT,0)),0)"
        strSql += " FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text)
        strSql += " AND ISNULL(CANCEL,'')='' AND ISNULL(CHEQUERETDATE,'')=''"
        bonus = Val(objGPack.GetSqlValue(strSql))
        Return bonus
    End Function

    Private Function CalcInterest() As Double
        strSql = " SELECT TOP 1 I.INTPER"
        strSql += " FROM " & chitMainDb & "..INSAMOUNT I "
        strSql += " INNER JOIN " & chitMainDb & "..SCHEMEMAST M "
        strSql += " ON M.SCHEMEID = I.SCHEMEID AND I.GROUPCODE = '" & txtCHITCardGrpCode.Text & "'"
        Return Val(objGPack.GetSqlValue(strSql))
    End Function

    Public Sub CalcTotal()
        Dim totAmt As Double = Nothing
        For Each ro As DataRow In dtChitAdj.Rows
            totAmt += Val(ro!TOTAL.ToString)
        Next
        gridCHITCardTotal.Rows(0).Cells("TOTAL").Value = IIf(totAmt <> 0, Format(totAmt, "0.00"), DBNull.Value)
    End Sub

    Private Function CheckGroupCode() As Boolean
        If Not objGPack.GetSqlValue("SELECT GROUPCODE FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "'").Length > 0 Then
            MsgBox("Invalid GroupCode", MsgBoxStyle.Information)
            txtCHITCardGrpCode.Focus()
            Return True
        End If
    End Function

    Private Function CheckRegNo() As Boolean
        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITCLOSESLIP", "N") = "Y" Then
            strSql = " SELECT * FROM " & chitMainDb & "..ESTIMATECLOSE CL "
            strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "'"
            strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
            strSql += " AND ISNULL(CANCEL,'') <> 'Y'"
            strSql += " AND EXISTS (SELECT 1 FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = CL.GROUPCODE AND REGNO = CL.REGNO )"
            Dim dchitclrow As DataRow
            dchitclrow = GetSqlRow(strSql, cn)

            If dchitclrow Is Nothing Then
                MsgBox("Slip Not Generate" & vbCrLf & "Member dosn't Close", MsgBoxStyle.Information)
                txtCHITCardRegNo_NUM.Focus()
                Return True
            Else
                txtCHITCardBonus_AMT.Text = Format(Val("" & dchitclrow.Item("Bonus").ToString) + Val("" & dchitclrow.Item("INTEREST").ToString), "0.00")
                txtCHITCardGift_AMT.Text = Val("" & dchitclrow.Item("GIFTVALUE").ToString)
            End If
        End If


        ''Otp Prompt when last paid date less then CHIT_CLOSEMAXDAYS

        Dim mSchrow As DataRow
        mSchrow = GetSqlRow("SELECT SCHEMEID,SNO FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & "", cn)
        If mSchrow Is Nothing Then
            MsgBox("Invalid RegNo", MsgBoxStyle.Information)
            Return True
        End If
        Dim Mschid As Integer = Val(mSchrow(0).ToString)

        ''Otp Prompt when Duplicate Card Issued
        strSql = "SELECT OPTIONID FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME ='CHITDUPCARD' AND ACTIVE = 'Y'"
        Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(strSql, , , tran))
        If Optionid <> 0 Then
            strSql = "SELECT 1 FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND DUPCARDISSDATE IS NOT NULL"
            If Val("" & objGPack.GetSqlValue(strSql, , 0, tran)) = 1 Then
                If MsgBox("Duplicate Card issued." & vbCrLf & vbCrLf & " Have an OTP to Proceed ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return True
                If Not usrpwdok("CHITDUPCARD", True) Then Return True
                SendKeys.Send("{TAB}")
            End If
        End If





        Dim ChitcloseMaxDays As Integer = 0
        Dim ChitcloseMax As String = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHIT_CLOSEMAXDAYS", 0)
        If ChitcloseMax.Contains(",") Then
            Dim ChitClose() As String = ChitcloseMax.Split(",")
            For i As Integer = 0 To ChitClose.Length - 1 Step 2
                If Mschid <> Val(ChitClose(i + 1).ToString) Then
                    ChitcloseMaxDays = 0
                Else
                    ChitcloseMaxDays = Val(ChitClose(i).ToString) : Exit For
                End If
            Next
        Else
            ChitcloseMaxDays = Val(ChitcloseMax)
        End If
        If ChitcloseMaxDays <> 0 Then
            strSql = "SELECT TOP 1 RDATE FROM " & chitTrandb & "..SCHEMECOLLECT WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
            strSql += " ORDER BY RDATE DESC"
            Dim lchqdate As Date = Convert.ToDateTime(objGPack.GetSqlValue(strSql, , Nothing, ))
            If ChitcloseMaxDays > DateDiff(DateInterval.Day, lchqdate, BillDate) Then
                If MsgBox("Chit Card is locked" & vbCrLf & vbCrLf & " Have an OTP to Proceed ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return True
                If Not usrpwdok("CHITCARDLOCK", True) Then Return True
            End If
        End If


        strSql = " SELECT GROUPCODE FROM " & chitTrandb & "..SCHEMETRAN C"
        strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "'"
        strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
        strSql += " AND EXISTS (SELECT 1 FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = C.GROUPCODE AND REGNO = C.REGNO )"
        'AND CONVERT(VARCHAR,SCHEMEID) = (SELECT SCHEMECODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & cmbChittCardType_MAN.Text & "')
        If Not objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox("Invalid RegNo", MsgBoxStyle.Information)
            txtCHITCardRegNo_NUM.Focus()
            Return True
        Else
            strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE SCHEMECODE = "
            strSql += " (SELECT CONVERT(VARCHAR,SCHEMEID) FROM " & chitMainDb & "..SCHEMEMAST "
            strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "'"
            strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ")"
            cmbCHITtCardType_MAN.Text = objGPack.GetSqlValue(strSql).ToString()
            If objGPack.GetSqlValue("SELECT ISNULL(AUTOPOST,'') FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & cmbCHITtCardType_MAN.Text & "'") = "N" Then autoPost = False Else autoPost = True
            strSql = " SELECT SCHEMECODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & cmbCHITtCardType_MAN.Text & "'"
            txtCHITCardSchemeId.Text = objGPack.GetSqlValue(strSql)
        End If
    End Function

    Private Function CalcDeduction(ByVal GiftDeductIns As Integer) As Double
        Dim CSchemeid As Integer
        strSql = "SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE='" & txtCHITCardGrpCode.Text & "' AND REGNO='" & txtCHITCardRegNo_NUM.Text & "'"
        CSchemeid = Val(objGPack.GetSqlValue(strSql).ToString)
        strSql = " SELECT ISNULL(INCENTIVE,'') INCENTIVE FROM " & chitMainDb & "..SCHEME WHERE SCHEMEID = " & CSchemeid
        If objGPack.GetSqlValue(strSql).ToString = "Y" And GiftDeductIns > 0 Then
            strSql = "SELECT TOP 1 RESULT FROM("
            strSql += vbCrLf + " SELECT 'Y' RESULT FROM " & chitMainDb & "..SCHEMEINCENTIVE WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text.Trim & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text.Trim) & " AND ISNULL(CANCEL,'')<>'Y'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'Y' RESULT FROM " & chitMainDb & "..GIFTTRAN WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text.Trim & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text.Trim) & " AND ISNULL(CANCEL,'')<>'Y'"
            strSql += vbCrLf + " )X"
            If objGPack.GetSqlValue(strSql).ToString = "Y" Then
                Dim SchAmt As Double
                Dim SchType As String
                strSql = "SELECT SCHEMETYPE FROM " & chitMainDb & "..SCHEME WHERE SCHEMEID=" & CSchemeid
                SchType = objGPack.GetSqlValue(strSql, , "A").ToString
                If SchType = "A" Then
                    strSql = "SELECT  CASE WHEN ISNULL(INSAMOUNT,0)<>0 THEN INSAMOUNT ELSE AMOUNT END AMOUNT FROM " & chitTrandb & "..SCHEMETRAN "
                    strSql += " WHERE GROUPCODE='" & txtCHITCardGrpCode.Text.Trim & "' AND REGNO=" & Val(txtCHITCardRegNo_NUM.Text.Trim)
                    strSql += " AND ISNULL(CANCEL,'')=''"
                    strSql += " AND CHEQUERETDATE IS NULL ORDER BY RDATE"
                    SchAmt = objGPack.GetSqlValue(strSql, "AMOUNT")
                Else
                    strSql = "SELECT  CASE WHEN ISNULL(INSWEIGHT,0)<>0 THEN INSWEIGHT ELSE WEIGHT END WEIGHT FROM " & chitTrandb & "..SCHEMETRAN "
                    strSql += " WHERE GROUPCODE='" & txtCHITCardGrpCode.Text.Trim & "' AND REGNO=" & Val(txtCHITCardRegNo_NUM.Text.Trim)
                    strSql += " AND ISNULL(CANCEL,'')=''"
                    strSql += " AND CHEQUERETDATE IS NULL ORDER BY RDATE"
                    SchAmt = objGPack.GetSqlValue(strSql, "WEIGHT")
                End If

                strSql = " SELECT JOINDATE FROM " & chitMainDb & "..SCHEMEMAST"
                strSql += " WHERE GROUPCODE='" & txtCHITCardGrpCode.Text.Trim & "' AND REGNO=" & Val(txtCHITCardRegNo_NUM.Text.Trim)
                Dim Joindate As Date = objGPack.GetSqlValue(strSql, "JOINDATE")

                strSql = "SELECT * FROM " & chitMainDb & "..GIFTRANGE "
                strSql += "  WHERE SCHEMEID=" & CSchemeid & " AND " & SchAmt & " "
                strSql += "  BETWEEN FROMAMT AND TOAMT "
                strSql += "  AND ISNULL(ACTIVE,'Y')<>'N'"
                strSql += "  AND ISNULL(GIFTDEDUCT,0)>0"
                strSql += "  AND '" & Joindate & "' BETWEEN ISNULL(DATEFROM,'" & Joindate & "') AND ISNULL(DATETO,'" & Joindate & "')"
                Dim dtGiftRange As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGiftRange)
                If dtGiftRange.Rows.Count > 0 Then
                    Return Val(dtGiftRange.Rows(0).Item("GIFTDEDUCT").ToString)
                End If
            End If
        End If
    End Function

    Private Function CalcGift() As Double
        strSql = " SELECT TOP 1 I.GIFTVALUE"
        strSql += " FROM " & chitMainDb & "..INSAMOUNT I "
        strSql += " INNER JOIN " & chitMainDb & "..SCHEMEMAST M "
        strSql += "   ON M.SCHEMEID = I.SCHEMEID AND I.GROUPCODE = '" & txtCHITCardGrpCode.Text & "'"
        strSql += "   AND I.AMOUNT = (SELECT TOP 1 AMOUNT FROM " & chitTrandb & "..SCHEMECOLLECT WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ")"
        Return Val(objGPack.GetSqlValue(strSql))
    End Function

    Private Sub SetAddress()
        strSql = " SELECT PNAME,SNAME,DOORNO,ADDRESS1,ADDRESS2,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE"
        strSql += " ,ISNULL(CASE WHEN IDPROOF='PANCARD' THEN IDPROOFNO ELSE '' END,'') PAN"
        strSql += " FROM " & chitMainDb & "..PERSONALINFO "
        strSql += " WHERE PERSONALID = (SELECT SNO FROM " & chitMainDb & "..SCHEMEMAST"
        strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = '" & Val(txtCHITCardRegNo_NUM.Text) & "'"
        strSql += " )"
        'strSql = " SELECT PNAME,SNAME,DOORNO,ADDRESS1,ADDRESS2,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES"
        'strSql += " FROM " & chitMainDb & "..SCHEMEMAST "
        'strSql += " WHERE GROUPCODE = '" & txtChitCardGrpCode.Text & "' AND REGNO = '" & Val(txtChitCardRegNo_NUM.Text) & "'"
        lblAddress.Text = ""
        Dim dtAddress As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAddress)
        If Not dtAddress.Rows.Count > 0 Then Exit Sub
        Dim str As String = Nothing
        With dtAddress.Rows(0)
            'str += .Item("PNAME").ToString
            'If str <> Nothing Then str += "," + vbCrLf
            'str += .Item("SNAME").ToString
            'If str <> Nothing Then str += "," + vbCrLf
            'str += .Item("ADDRESS1").ToString
            'If str <> Nothing Then str += ","
            'str += .Item("ADDRESS2").ToString
            'If str <> Nothing Then str += "," + vbCrLf
            'str += .Item("AREA").ToString
            'If str <> Nothing Then str += ","
            'str += .Item("CITY").ToString + IIf(.Item("PINCODE").ToString <> "", "-", "") + .Item("PINCODE").ToString
            'If str <> Nothing Then str += "," + vbCrLf
            'str += .Item("PHONERES").ToString
            str += .Item("PNAME").ToString
            str += "," + vbCrLf & .Item("DOORNO").ToString
            str += "," + vbCrLf & .Item("ADDRESS1").ToString
            str += "," + vbCrLf & .Item("ADDRESS2").ToString
            str += "," + vbCrLf & .Item("AREA").ToString
            str += "," + vbCrLf & .Item("CITY").ToString
            str += "," + vbCrLf & .Item("STATE").ToString
            str += "," + vbCrLf & .Item("COUNTRY").ToString
            str += "," + vbCrLf & .Item("PINCODE").ToString
            str += "," + vbCrLf & .Item("PHONERES").ToString
            str += "," + vbCrLf & .Item("MOBILE").ToString
            If .Item("PAN").ToString <> "" Then str += "," + vbCrLf & .Item("PAN").ToString
            lblPname.Text = .Item("PNAME").ToString
            lblMobileNo.Text = .Item("MOBILE").ToString
        End With
        lblAddress.Text = str
    End Sub

    Private Function ChitIntegValidate() As Boolean
        If GstCalc = "I" Or GstCalc = "E" Then
            strSql = " SELECT SUM(AMOUNT)+ISNULL(SUM(TAX),0) AS AMOUNT "
        Else
            strSql = " SELECT SUM(AMOUNT)+ (CASE WHEN ISNULL(SUM(TAX),0) <>0 THEN ISNULL(SUM(TAX),0) ELSE 0 END) AS AMOUNT "
        End If
        strSql += " FROM " & chitTrandb & "..SCHEMETRAN WHERE "
        strSql += " GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
        Dim chtamt, chcamt As Double
        chtamt = Val(objGPack.GetSqlValue(strSql).ToString)
        strSql = " SELECT SUM(AMOUNT) AS AMOUNT FROM " & chitTrandb & "..SCHEMECOLLECT WHERE "
        strSql += " GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL AND ISNULL(RECEIPTNO,0)<>0 "
        chcamt = Val(objGPack.GetSqlValue(strSql).ToString)
        If chtamt <> chcamt Then Return False
        Return True
    End Function

    Private Sub GetChitDetails()
        Dim flagActualReceipt As Boolean = False
        Dim flagAddPrize As Boolean = False
        Dim flagRecPay As Boolean = False
        If Not ChitIntegValidate() Then
            If MsgBox("Chit Card Amount mismatched" & vbCrLf & vbCrLf & "Please check the member ledger" & vbCrLf & vbCrLf & "Can You Continue?..", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub Else GoTo NEXT0
        End If
NEXT0:
        Dim minchqdays As Integer = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHIT_CHQCLEARDAYS", 0)
        If minchqdays <> 0 Then
            strSql = "SELECT TOP 1 RDATE FROM " & chitTrandb & "..SCHEMECOLLECT WHERE MODEPAY = 'D' "
            strSql += " AND GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
            strSql += " ORDER BY RDATE DESC"
            Dim chkdate As Date = Nothing
            Dim lchqdate As Date = Convert.ToDateTime(objGPack.GetSqlValue(strSql, , Nothing, ))
            If DateDiff(DateInterval.Day, lchqdate, BillDate) <= minchqdays And lchqdate <> chkdate Then
                If MsgBox("Cheque Clearing days not completed" & vbCrLf & vbCrLf & "* * * Confirm to Cheque Clearance * * *" & vbCrLf & vbCrLf & "  Can you Proceed Anyway?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub Else GoTo NEXT1
            End If
        End If
        strSql = "SELECT ISNULL(AMOUNT,0)+ISNULL(TAX,0) FROM " & chitTrandb & "..SCHEMETRAN WHERE 1=1 "
        strSql += " AND GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' "
        Dim mInsAmount As Double = Val(objGPack.GetSqlValue(strSql).ToString)

        Dim mSchid As Integer
        strSql = "SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST "
        strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text)
        mSchid = objGPack.GetSqlValue(strSql)
        strSql = " SELECT CTLTEXT FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID = 'RANGE_BONUS_" & mSchid.ToString & "_" & mInsAmount.ToString & "'"
        Dim RangeBonus_AR() As String = objGPack.GetSqlValue(strSql).ToString.Split(":")
        ''Otp Prompt when Duplicate Card Issued
        strSql = "SELECT OPTIONID FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME ='CHITDUPCARD' AND ACTIVE = 'Y'"
        Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(strSql, , , tran))
        If Optionid <> 0 Then
            strSql = "SELECT 1 FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND DUPCARDISSDATE IS NOT NULL"
            If Val("" & objGPack.GetSqlValue(strSql, , 0, tran)) = 1 Then
                If MsgBox("Duplicate Card issued." & vbCrLf & vbCrLf & " Have an OTP to Proceed ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
                If Not usrpwdok("CHITDUPCARD", True) Then Exit Sub
                SendKeys.Send("{TAB}")
            End If
        End If
        lblCardLost.Text = ""
        lblRemarks.Text = ""
        lblPDCCheque.Text = ""
        strSql = "SELECT CARDLOST,CONVERT(VARCHAR(12),CARDLDATE,103)CARDLDATE,REMARK FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
        Dim drCl As DataRow = GetSqlRow(strSql, cn)
        If Not drCl Is Nothing Then
            lblRemarks.Text = drCl.Item("REMARK").ToString
            If drCl.Item("CARDLOST").ToString = "Y" Then
                lblCardLost.Text = "Card Lost on " & drCl.Item("CARDLDATE").ToString
                Timer1.Start()
            Else
                Timer1.Stop()
            End If
        End If
        strSql = "SELECT OPTIONID FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME ='CHITCARDLOST' AND ACTIVE = 'Y'"
        Dim _Optionid As Integer = Val("" & objGPack.GetSqlValue(strSql, , , Nothing))
        If _Optionid <> 0 Then
            strSql = "SELECT 1 FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CARDLOST,'') = 'Y'"
            If Val("" & objGPack.GetSqlValue(strSql, , 0, Nothing)) = 1 Then
                If MsgBox("Card Lost issued." & vbCrLf & vbCrLf & " Have an OTP to Proceed ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
                If Not usrpwdok("CHITCARDLOST", True) Then Exit Sub
                SendKeys.Send("{TAB}")
            End If
        End If
        strSql = "SELECT PDCRECEIPTNO FROM " & chitMainDb & "..PDCHEAD WHERE PDCRECEIPTNO IN (SELECT PDCRECEIPTNO"
        strSql += vbCrLf + " FROM " & chitMainDb & "..PDCTRAN WHERE RECEIPTNO IS NULL AND GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = '" & txtCHITCardRegNo_NUM.Text & "') "
        da = New OleDbDataAdapter(strSql, cn)
        Dim dt As New DataTable
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            lblPDCCheque.Text = "PDC Cheques Available."
            Timer1.Start()
        End If
        ''Otp Prompt when last paid date exceed CHIT_CLOSEVALIDDAYS
        Dim mchitclosedays As Integer = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHIT_CLOSEVALIDDAYS", 0)
        If mchitclosedays <> 0 Then
            strSql = "SELECT TOP 1 RDATE FROM " & chitTrandb & "..SCHEMECOLLECT WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
            strSql += " ORDER BY RDATE DESC"
            Dim chkdate As Date = Nothing
            Dim lchqdate As Date = Convert.ToDateTime(objGPack.GetSqlValue(strSql, , Nothing, ))
            If DateDiff(DateInterval.Day, lchqdate, BillDate) > mchitclosedays And lchqdate <> chkdate Then
                If MsgBox("Chit Card is locked" & vbCrLf & vbCrLf & " Have an OTP to Proceed ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
                If Not usrpwdok("CHITCARDLOCK", True) Then Exit Sub
                'AddExemptionrow(7, "OTP", "Expiry chit closing allowed")
            End If
        End If

        ''Otp Prompt when last paid date less then CHIT_CLOSEMAXDAYS
        Dim ChitcloseMaxDays As Integer = 0
        Dim ChitcloseMax As String = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHIT_CLOSEMAXDAYS", 0)
        If ChitcloseMax.Contains(",") Then
            Dim ChitClose() As String = ChitcloseMax.Split(",")
            ChitcloseMaxDays = Val(ChitClose(0).ToString)
            If mSchid <> Val(ChitClose(1).ToString) Then
                ChitcloseMaxDays = 0
            End If
        Else
            ChitcloseMaxDays = Val(ChitcloseMax)
        End If
        If ChitcloseMaxDays <> 0 Then
            strSql = "SELECT TOP 1 RDATE FROM " & chitTrandb & "..SCHEMECOLLECT WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
            strSql += " ORDER BY RDATE DESC"
            Dim lchqdate As Date = Convert.ToDateTime(objGPack.GetSqlValue(strSql, , Nothing, ))
            If ChitcloseMaxDays > DateDiff(DateInterval.Day, lchqdate, BillDate) Then
                If MsgBox("Chit Card is locked" & vbCrLf & vbCrLf & " Have an OTP to Proceed ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
                If Not usrpwdok("CHITCARDLOCK", True) Then Exit Sub
            End If
        End If

        ''Send Otp to Customer
        strSql = "SELECT OPTIONID FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME ='CHITCLOSE_USEROTP' AND ACTIVE = 'Y'"
        Optionid = Val("" & objGPack.GetSqlValue(strSql, , , tran))
        strSql = "SELECT PWDTYPE FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME ='CHITCLOSE_USEROTP' AND ACTIVE = 'Y'"
        Dim PwdType As String = objGPack.GetSqlValue(strSql, , , tran)
        If Optionid <> 0 Then
            SetAddress()
            If MsgBox("Do you want to Generate OTP ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub

            Dim MSG As String = objGPack.GetSqlValue("SELECT ISNULL(TEMPLATE_DESC,'') AS TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='CHITCLOSE_USEROTP' AND ISNULL(ACTIVE,'Y')<>'N'", "TEMPLATE_DESC", "").ToString
            If MSG = "" Then
                MSG = "<OTP> is the one time password (OTP) for your Savings scheme Redemption "
                MSG += "for " & txtCHITCardGrpCode.Text.ToString & "-" & txtCHITCardRegNo_NUM.Text.ToString
                MSG += " PLS DO NOT SHARE WITH ANYONE."
                MSG += " If you have not come for redemption please contact our customer service Executive immediately"
            End If
            If MSG <> "" Then
                MSG = MSG.Replace("<GROUPCODE>", txtCHITCardGrpCode.Text.ToString)
                MSG = MSG.Replace("<REGNO>", txtCHITCardRegNo_NUM.Text.ToString)
            End If
            If lblMobileNo.Text = "" Or lblMobileNo.Text.Length < 10 Or lblMobileNo.Text.Length > 10 Then
                MsgBox("Invalid Customer Mobile No...", MsgBoxStyle.Information)
                Exit Sub
            End If
            ''funcGenPwd(Optionid, lblMobileNo.Text, MSG, True)
            Dim pwdid As Integer = GetuserPwd(Optionid, cnCostId, userId)
            If pwdid <> 0 Or PwdType = "S" Then
                Dim objUpwd As New frmUserPassword(pwdid, Optionid, False, lblMobileNo.Text, Msg)
                If objUpwd.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub
                txtCHITCardTotal_AMT.Focus()
            Else
                MsgBox("OTP Not Generated...", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If


NEXT1:
        strSql = " SELECT CTLTEXT FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID = 'ACTUALRECEIPT'"
        If objGPack.GetSqlValue(strSql).ToUpper = "Y" Then flagActualReceipt = True
        strSql = " SELECT CTLTEXT FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID = 'INCLUDEPRIZEAMT'"
        If objGPack.GetSqlValue(strSql).ToUpper = "Y" Then flagAddPrize = True
        strSql = " SELECT CTLTEXT FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID = 'INCLUDERECPAY'"
        If objGPack.GetSqlValue(strSql).ToUpper = "Y" Then flagRecPay = True
        strSql = " SELECT CTLTEXT FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID = 'GSTCALC'"
        GstCalc = objGPack.GetSqlValue(strSql).ToUpper

        Dim flagMonthBonus As Boolean = False
        Dim MONTHONMONTHBONUS As Boolean = False
        Dim flagInterest As Boolean = False
        Dim Isflatbonus As Boolean = False
        strSql = " SELECT CTLTEXT FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID = 'MONTHBONUS'"
        Dim monthbonus As String = objGPack.GetSqlValue(strSql).ToUpper
        If monthbonus = "Y" Then flagMonthBonus = True
        If monthbonus = "M" Then MONTHONMONTHBONUS = True
        If monthbonus = "I" Then flagInterest = True
        If monthbonus = "F" Then Isflatbonus = True
        'If GetAdmindbSoftValuefromDt(dtsoftkeyss, "MONTHBONUS", "N").ToUpper = "Y" Then
        Dim ReceivedAmount As Double = Nothing
        Dim Gift As Double = Nothing
        Dim PrizeAmount As Double = Nothing
        Dim Bonus As Double = Nothing
        Dim GstAmt As Double = Nothing
        Dim Interest As Double = Nothing
        Dim Deduction As Double = Nothing
        Dim RecPayAmt As Decimal = Nothing
        Dim Bonuswt As Decimal = Nothing
        Dim ReceivedAmountinwt As Decimal
        Dim wt As Double
        Dim adjwt As Double
        lblInterestPer.Text = ""
        strSql = "SELECT SUM(CASE WHEN TRANTYPE = 'R' THEN AMOUNT ELSE -1*AMOUNT END) FROM " & chitMainDb & "..RECPAY "
        strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
        strSql += " AND ISNULL(FLAG,'') <> 'CO'"
        RecPayAmt = Val(objGPack.GetSqlValue(strSql))
        lblInterestPer.Text = IIf(RecPayAmt <> 0, "RecPay : " & Format(RecPayAmt, "0.00"), "")


        strSql = " SELECT CTLTEXT FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID = 'VBCFORSCHEMEIDS'"
        Dim Vbc_sch As Boolean = False
        Dim mSchids As String = ""
        mSchids = objGPack.GetSqlValue(strSql)
        If mSchids <> "" Then
            Dim mSchemeId() As String = mSchids.Split(",")
            For j As Integer = 0 To mSchemeId.Length - 1
                If Val(mSchemeId(j).ToString) = mSchid Then
                    Vbc_sch = True
                    Exit For
                End If
            Next
        End If

        ''TOTAL AMOUNT RECEIVED
        strSql = " SELECT ISNULL(SCHEMETYPE,'') schemetype,ISNULL(purity,0) PURITY,ISNULL(WEIGHTLEDGER,'') WEIGHTLEDGER,isnull(PARTWEIGHTADJ,'N') PARTWEIGHTADJ,ISNULL(BONUSWEIGHT,'N') AS BONUSWEIGHT,instalment  FROM " & chitMainDb & "..SCHEME WHERE SCHEMEID = " & mSchid
        Dim schrow As DataRow
        schrow = GetSqlRow(strSql, cn)
        Dim SchemeType As String
        Dim Purity As Decimal = 0
        Dim WeightLedger As String
        Dim Bonustype As String
        Dim MaxIns As Integer
        If Not schrow Is Nothing Then
            SchemeType = schrow.Item(0).ToString
            Purity = Val(schrow.Item(1).ToString)
            WeightLedger = schrow.Item(2).ToString
            mPartweightadj = IIf(schrow.Item(3).ToString = "Y", True, False)
            Bonustype = IIf(schrow.Item(4) = "Y", "W", "A")
            MaxIns = Val(schrow.Item(5).ToString)
        End If
        Dim rate As Double = 0
        Dim ratemode As String = ""
        Dim MCHITWCTYPE As String = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITWCTYPE", "A")
        Dim NOWASTMC_ENABLE As Boolean = IIf(GetAdmindbSoftValuefromDt(dtsoftkeyss, "NOWASTMC_ENABLE", "N") = "N", False, True)
        Dim Bonusbywtc As Decimal = 0
        strSql = " SELECT SUM(WEIGHT) AS WEIGHT FROM " & chitTrandb & "..SCHEMETRAN WHERE "
        strSql += " GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
        wt = Val(objGPack.GetSqlValue(strSql).ToString)

        strSql = " SELECT CTLTEXT FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID = 'METALRATE'"
        If objGPack.GetSqlValue(strSql, , "R").ToString = "R" Then
            rate = Val(GetRate_Purity(GetEntryDate(GetServerDate), objGPack.GetSqlValue("SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN 91.6 AND 92 AND METALID = 'G' ORDER BY PURITY DESC")).ToString)
        Else
            strSql = " SELECT TOP 1 RATE FROM " & chitMainDb & "..RATEMAST WHERE RATEDATE <= '" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "' "
            If Purity = 91.6 Then
                strSql += " AND ISNULL(PURITY,91.6) BETWEEN 91 AND 92"
            Else
                strSql += " AND PURITY=" & Val(Purity)
            End If
            strSql += "  ORDER BY RATEID DESC"
            rate = Val(objGPack.GetSqlValue(strSql).ToString)
        End If

        If (SchemeType = "W" Or WeightLedger = "Y") And MCHITWCTYPE = "W" Then
            strSql = " SELECT SUM(AMOUNT) AS AMOUNT FROM " & chitTrandb & "..SCHEMETRAN WHERE "
            strSql += " GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
            ReceivedAmount = Val(objGPack.GetSqlValue(strSql).ToString)

            strSql = " SELECT SUM(WEIGHT) AS WEIGHT FROM " & chitTrandb & "..SCHEMEADJTRAN WHERE "
            strSql += " GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' "
            adjwt = Val("" & objGPack.GetSqlValue(strSql).ToString)

            If GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITAVGRATE", "N") = "Y" Then
                strSql = " SELECT SUM(WEIGHT) TOTWT,SUM(AMOUNT) TOTAMT  FROM " & chitTrandb & "..SCHEMETRAN WHERE "
                strSql += " GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
                Dim dri As DataRow = GetSqlRow(strSql, cn, tran)
                rate = Val(dri.Item(1).ToString) / Val(dri.Item(0).ToString)
                ratemode = "(AVG.)"
            End If


            If mPartweightadj Then
                lblSetwt.Visible = True : txtSettleWt.Visible = True
            End If
            ReceivedAmountinwt = Math.Round((wt - adjwt) * rate, 2)

            If ReceivedAmount - Math.Floor(ReceivedAmount) > 0.5 Then
                ReceivedAmount = Math.Ceiling(ReceivedAmount)
            Else
                ReceivedAmount = Math.Floor(ReceivedAmount)
            End If
            mSettrate = rate
            If ReceivedAmountinwt > 0 And mPartweightadj = False Then
                If ReceivedAmountinwt > ReceivedAmount Then Bonusbywtc = ReceivedAmountinwt - ReceivedAmount Else Deduction = ReceivedAmount - ReceivedAmountinwt
            End If
        ElseIf SchemeType = "W" And MCHITWCTYPE = "A" And MaxIns = 1 Then ''FOR KFJ MOMS GOLD 19/08/2015
            lblRate.Text = "@Rate : " & Format(rate, "0.00")
            Dim wtdtls As String = "Accu. Wt.: " & Format(wt, "0.000")
            lblWeightSchemeDetail.Text = wtdtls
            '            lblBonusDeduction.Text = "Bonus-Deduction : "
            ReceivedAmountinwt = Math.Round((wt - adjwt) * rate, 2)
            ReceivedAmount = ReceivedAmountinwt
            If ReceivedAmount - Math.Floor(ReceivedAmount) > 0.5 Then
                ReceivedAmount = Math.Ceiling(ReceivedAmount)
            Else
                ReceivedAmount = Math.Floor(ReceivedAmount)
            End If
        Else
            ReceivedAmountinwt = Math.Round((wt - adjwt) * rate, 2)
            ReceivedAmount = CalcReceiptAmount(flagActualReceipt)
        End If
        GstAmt = CalcGst()
        ''RecPay Amount
        If flagRecPay Then ''Mumbai
            If RecPayAmt < 0 Then
                Deduction += Math.Abs(RecPayAmt)
            Else
                Bonus += RecPayAmt
            End If
        Else
            If RecPayAmt < 0 Then Deduction += Math.Abs(RecPayAmt)
        End If
        ''PrizeAmount
        If flagAddPrize Then ''Mumbai
            PrizeAmount = CalcPrizeAmountNew()
        Else
            PrizeAmount = CalcPrizeAmount()
        End If

        ''Bonus
        'strSql = " SELECT instalment FROM " & chitMainDb & "..SCHEME WHERE SCHEMEID = " & mSchid

        strSql = " SELECT TOP 1 AMOUNT FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
        Dim insAmt As Double = Val(objGPack.GetSqlValue(strSql))
        strSql = " SELECT count(*) FROM " & chitTrandb & "..SCHEMETRAN WHERE ISNULL(CANCEL,'') <> 'Y' AND GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
        Dim Totins As Double = Val(objGPack.GetSqlValue(strSql))
        strSql = " SELECT TOP 1 NOOFINS FROM " & chitMainDb & "..INSAMOUNT "
        strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
        strSql += " AND AMOUNT = " & insAmt & ""
        Dim noOfIns As Integer = Val(objGPack.GetSqlValue(strSql))
        'strSql = " SELECT ISNULL(BONUSWEIGHT,'N') AS BONUSWEIGHT FROM " & chitMainDb & "..SCHEME WHERE SCHEMEID =" & mSchid
        If flagInterest Then
            If Bonustype = "W" Then
                Bonusbywtc = 0
                Bonuswt = Math.Round(CalcBonus(Bonustype), 3)
            Else
                strSql = " SELECT ISNULL(ANNUALINTCAL,'') AS ANNUALINTCAL FROM " & chitMainDb & "..SCHEME "
                strSql += " WHERE SCHEMEID =" & mSchid
                If objGPack.GetSqlValue(strSql).ToUpper = "Y" Then
                    Bonus = CalcBonus("A")
                Else
                    Bonus = CalcMonthBonus()
                End If
            End If
        Else
            Dim Currentvalue4bonus As Boolean
            strSql = " SELECT ISNULL(BONUSWEIGHT,'N') AS CURRENTRATE,ISNULL(ANNUALINTCAL,'') AS ANNUALINTCAL,ISNULL(INTEREST,0) FROM " & chitMainDb & "..SCHEME "
            strSql += " WHERE SCHEMEID =" & mSchid
            Dim drInt As DataRow
            drInt = GetSqlRow(strSql, cn)
            If Not drInt Is Nothing Then
                Currentvalue4bonus = IIf(drInt.Item(0).ToString = "C", True, False)
                If Currentvalue4bonus Then
                    Dim Interestper As Double = Val(drInt.Item(2).ToString)
                    If drInt.Item(1).ToString = "Y" Then Bonus = ReceivedAmountinwt * (Interestper / 100) : GoTo bonusover
                End If
            End If

            If MONTHONMONTHBONUS Then
                Bonus = CalcBonusSlab()
            Else
                If Bonustype = "W" Then
                    Bonusbywtc = 0
                    Bonuswt = Math.Round(CalcBonus(Bonustype), 3)
                Else
                    If flagMonthBonus = True Then
                        Bonus = CalcMonthBonus()
                    Else
                        If GstCalc = "E" Then
                            If ((ReceivedAmount) / insAmt = noOfIns) Then
                                If Isflatbonus Then Bonus = CalcBonus("F") Else Bonus = CalcBonus("A")
                            End If
                        Else
                            If ((ReceivedAmount + GstAmt) / insAmt = noOfIns) Then
                                If Isflatbonus Then Bonus = CalcBonus("F") Else Bonus = CalcBonus("A")
                            End If
                        End If
                    End If
                End If
            End If


        End If
bonusover:
        strSql = " SELECT TOP 1 GIFTVALUE FROM " & chitMainDb & "..INSAMOUNT "
        strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
        strSql += " AND AMOUNT = " & insAmt & ""
        Dim Giftpayvalue As Decimal = Val(objGPack.GetSqlValue(strSql).ToString)
        strSql = " SELECT Giftminins FROM " & chitMainDb & "..SCHEME WHERE SCHEMEID = " & mSchid
        Dim Giftrefundins As Decimal = Val(objGPack.GetSqlValue(strSql).ToString)
        If Totins < Giftrefundins Then Deduction += Giftpayvalue

        strSql = " SELECT GROUPCODE FROM " & chitMainDb & "..GIFTTRAN"
        strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
        strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
        strSql += " AND ISNULL(CANCEL,'') <> 'Y'"
        If objGPack.GetSqlValue(strSql).Length > 0 Then
            If (ReceivedAmount + GstAmt) / insAmt <> noOfIns And Giftrefundins = 0 Then Deduction += CalcGift() Else Gift = 0
        Else
            If (ReceivedAmount + GstAmt) / insAmt <> noOfIns Then Gift = 0 Else Gift = CalcGift()
        End If

        If Totins <> noOfIns Then Deduction += CalcDeduction(Giftrefundins)
        ''GST

        ''Interest  
        Interest = CalcInterest()
        If Interest <> 0 Then lblInterestPer.Text += " Interest : " & Interest & " %"

        'If Not MONTHONMONTHBONUS Then Bonus = Bonus + Bonusbywtc
        Bonus = Bonus + Bonusbywtc
        strSql = " SELECT ISNULL(BONUS,0)+ISNULL(INTEREST,0) BONUS,PRIZEVALUE,GIFTVALUE,DEDUCTGIFTVALUE+DEDUCTION DEDUCTION "
        strSql += " FROM " & chitMainDb & "..ESTIMATECLOSE "
        strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
        strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
        strSql += " AND ISNULL(CANCEL,'')<>'Y' "
        strSql += " AND SLIPDATE='" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "'"
        Dim DTEST As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DTEST)
        If DTEST.Rows.Count > 0 Then
            Bonus = Val(DTEST.Rows(0).Item("BONUS"))
            PrizeAmount = Val(DTEST.Rows(0).Item("PRIZEVALUE"))
            Gift = Val(DTEST.Rows(0).Item("GIFTVALUE"))
            Deduction = Val(DTEST.Rows(0).Item("DEDUCTION"))
        End If
        If (SchemeType = "W" Or WeightLedger = "Y") And MCHITWCTYPE = "W" Then
            lblRate.Text = "@Rate : " & ratemode & Format(rate, "0.00")
            Dim wtdetails As String = "Accu. Wt.: " & Format(wt, "0.000") '+ "+" + Format(adjwt, "0.000")
            If Bonuswt <> 0 Then wtdetails += " Bonus Wt.: " & Format(Bonuswt, "0.000")
            If mPartweightadj Then
                wtdetails += vbCrLf & " Bal.Wt.: " & Format(wt + Bonuswt - adjwt, "0.000")
                lblWeightSchemeDetail.Text = wtdetails
                'lblBonusDeduction.Text = "Bonus-Deduction : "
                txtSettleWt.Text = Format(wt + Bonuswt - adjwt, "0.000")
                txtCHITCardAmount_AMT.Text = Format(Val(txtSettleWt.Text) * rate, "#0.00")
            Else
                lblWeightSchemeDetail.Text = wtdetails
                'lblBonusDeduction.Text = "Bonus-Deduction : "
            End If
            If mPartweightadj Then wt = wt + Bonuswt Else Bonus += Bonuswt * rate
        End If
        If Val(txtSettleWt.Text) = 0 Then
            txtCHITCardAmount_AMT.Text = IIf(ReceivedAmount <> 0, Format(ReceivedAmount, "0.00"), Nothing)
        End If

        If InsBonus = False Then Bonus = 0
        If NOWASTMC_ENABLE Then PrizeAmount = Bonus : Bonus = 0
        txtCHITCardGift_AMT.Text = IIf(Gift <> 0, Format(Gift, "0.00"), Nothing)
        txtCHITCardPrize_AMT.Text = IIf(PrizeAmount <> 0, Format(PrizeAmount, "0.00"), Nothing)
        txtCHITCardBonus_AMT.Text = IIf(Bonus <> 0, Format(Bonus, "0.00"), Nothing)
        For Each strRange As String In RangeBonus_AR
            Dim RBonus_Ar() As String = strRange.ToString.Split("-")
            If RBonus_Ar.Length = 3 Then
                If Totins >= Val(RBonus_Ar(0).ToString()) And Totins <= Val(RBonus_Ar(1).ToString()) Then
                    txtCHITCardBonus_AMT.Text = Val(RBonus_Ar(2).ToString())
                End If
            End If
        Next

        If Not cmbBonusType.DataSource Is Nothing And Val(txtCHITCardBonus_AMT.Text) > 0 Then
            txtCHITCardBonus_AMT.Text = Val(txtCHITCardBonus_AMT.Text) * (Val(cmbBonusType.SelectedValue) / 100)
        End If



        txtCHITGST_AMT.Text = IIf(GstAmt <> 0, Format(GstAmt, "0.00"), Nothing)
        txtCHITCardDeduction_AMT.Text = IIf(Deduction <> 0, Format(Deduction, "0.00"), Nothing)
        If Vbc_sch Then
            txtCHITCardPrize_AMT.Text = Val(txtCHITCardPrize_AMT.Text) + Val(txtCHITCardTotal_AMT.Text) * 1 / 100
        End If
        mBonweight = Bonuswt
        mWeight = wt
        mBalweight = wt - adjwt
        SetAddress()
    End Sub

    Private Function CheckClose(Optional ByVal ChkLockDays As Boolean = False) As Boolean
        strSql = " SELECT CONVERT(VARCHAR(12),DOCLOSE,103) AS DOCLOSE,"
        strSql += " CASE WHEN ISNULL(CLOSETYPE,'') = 'B' THEN 'Bill'"
        strSql += "      WHEN ISNULL(CLOSETYPE,'') = 'C' THEN 'Cash' END AS CLOSETYPE"
        strSql += " ,BILLNO,CONVERT(VARCHAR(12),BILLDATE,103) AS BILLDATE "
        strSql += " FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE  = '" & txtCHITCardGrpCode.Text & "'  "
        strSql += " AND  REGNO  = " & Val(txtCHITCardRegNo_NUM.Text) & ""
        strSql += " AND DOCLOSE IS NOT NULL AND DOCLOSE <> '1900-01-01 00:00:00'"
        Dim dtCloseCheck As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCloseCheck)
        If dtCloseCheck.Rows.Count > 0 Then
            With dtCloseCheck.Rows(0)
                MsgBox("Already Closed !!!" + vbCrLf _
                + "Close Date : " + .Item("DOCLOSE").ToString + vbCrLf _
                + "Close Type : " + .Item("CLOSETYPE").ToString + vbCrLf _
                + "Bill No    : " + .Item("BILLNO").ToString + vbCrLf _
                + "Bill Date  : " + .Item("BILLDATE").ToString)
                Return True
            End With
        End If
        Dim i As Integer = 0
        For Each ro As DataRow In dtChitAdj.Rows
            If ro!GRPCODE.ToString = txtCHITCardGrpCode.Text And ro!REGNO.ToString = txtCHITCardRegNo_NUM.Text Then
                If txtCHITCardRowIndex.Text <> "" And Val(txtCHITCardRowIndex.Text) = i Then Continue For
                MsgBox("Already Adjusted this membership detail", MsgBoxStyle.Information)
                Return True
            End If
            i += 1
        Next
        If ChkLockDays And Val(txtChitslipNo.Text) = 0 Then
            strSql = "SELECT CTLTEXT FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID='LOCKDAYS'"
            Dim ChitLockDays As Integer
            ChitLockDays = objGPack.GetSqlValue(strSql, "CTLTEXT", 0)
            If ChitLockDays > 0 Then
                strSql = "SELECT MAX(RDATE)RDATE"
                strSql += " FROM " & Mid(chitMainDb, 1, 3) & "SH0708..SCHEMETRAN WHERE GROUPCODE  = '" & txtCHITCardGrpCode.Text & "'  "
                strSql += " AND  REGNO  = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'')='' AND ISNULL(CHEQUERETDATE,'')=''"
                Dim LsDate As Date = CType(objGPack.GetSqlValue(strSql, "RDATE"), Date)
                Dim LsDays As Integer = DateDiff(DateInterval.Day, LsDate, CType(GetServerDate(), Date))
                If LsDays >= ChitLockDays Then
                    MsgBox("Card Locked,Please Generate Slip", MsgBoxStyle.Information)
                    Return True
                End If
            End If
        End If
    End Function



    Private Sub txtChitCardGrpCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCHITCardGrpCode.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If CheckGroupCode() Then Exit Sub
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            Me.Close()
        End If
    End Sub
    Private Function CheckAuth() As Boolean
        If AuthPwdPass = True Then Return False
        strSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..BILLAUTHORIZE"
        If Val(objGPack.GetSqlValue(strSql, "CNT", 0)) > 0 Then
            strSql = "SELECT  DATEDIFF(DD,MAX(RDATE),'" & BillDate & "')AS DAYSDIFF  "
            strSql += " FROM " & Mid(chitMainDb, 1, 3) & "SH0708..SCHEMETRAN WHERE GROUPCODE='" & txtCHITCardGrpCode.Text & "' AND REGNO =" & Val(txtCHITCardRegNo_NUM.Text)
            Dim DaysDiff As Integer = Val(objGPack.GetSqlValue(strSql, "DAYSDIFF", 0))
            strSql = "SELECT (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=A.USERID)USERNAME,* "
            strSql += " FROM " & cnAdminDb & "..BILLAUTHORIZE A WHERE " & DaysDiff & " BETWEEN FROMDAYS AND TODAYS"
            Dim dr As DataRow = GetSqlRow(strSql, cn)
            If Not dr Is Nothing Then
                strSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..PWDAUTHORIZE "
                strSql += " WHERE GROUPCODE='" & txtCHITCardGrpCode.Text & "' AND REGNO =" & Val(txtCHITCardRegNo_NUM.Text)
                If Val(objGPack.GetSqlValue(strSql, "CNT", 0).ToString) = 0 Then
                    Dim PwdId As Integer = 0
                    strSql = "SELECT ISNULL(MAX(PWDID),0) FROM " & cnAdminDb & "..PWDAUTHORIZE"
                    PwdId = Val(GetSqlValue(cn, strSql)) + 1
                    strSql = "INSERT INTO " & cnAdminDb & "..PWDAUTHORIZE"
                    strSql += " (PWDID,GROUPCODE,REGNO,CRUSERID,AUTHORIZE,PWDTYPE,DAYS,UPDATED,PWDDATE)"
                    strSql += " VALUES"
                    strSql += " ("
                    strSql += " " & PwdId
                    strSql += " ,'" & txtCHITCardGrpCode.Text & "'"
                    strSql += " ," & Val(txtCHITCardRegNo_NUM.Text)
                    strSql += " ," & userId
                    strSql += " ,'N'"
                    strSql += " ,'S'"
                    strSql += " ," & DaysDiff
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
                    strSql += " ,'" & GetServerDate() & "'" 'Updated
                    strSql += " )"
                    cmd = New OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                    MsgBox("Authorization Required from " & dr("USERNAME").ToString, MsgBoxStyle.Information)
                    Return True
                End If
                strSql = "SELECT ISNULL(AUTHORIZE,'N')AUTHORIZE FROM " & cnAdminDb & "..PWDAUTHORIZE "
                strSql += " WHERE GROUPCODE='" & txtCHITCardGrpCode.Text & "' AND REGNO =" & Val(txtCHITCardRegNo_NUM.Text)
                If objGPack.GetSqlValue(strSql, "AUTHORIZE", "N") = "Y" Then
                    AuthPwdPass = True
                    Return False
                Else
                    MsgBox("Authorization Required from " & dr("USERNAME").ToString, MsgBoxStyle.Information)
                    Return True
                End If
            Else
                AuthPwdPass = True
                Return False
            End If
        Else
            AuthPwdPass = True
            Return False
        End If
    End Function

    Private Function CheckNoOffer() As Boolean
        Dim itemids As String = ""
        If dtReservedItem.Rows.Count > 0 Then
            itemids = ""
            For cntt As Integer = 0 To dtReservedItem.Rows.Count - 1
                If dtReservedItem.Rows(cntt).Item("TRANTYPE").ToString <> "SA" Then Continue For
                If dtReservedItem.Rows(cntt).Item("ENTFLAG").ToString = "" Then Exit For
                itemids += dtReservedItem.Rows(cntt).Item("ITEMID").ToString & ","
            Next
            If itemids <> "" Then
                Dim mSchrow As DataRow
                mSchrow = GetSqlRow("SELECT SCHEMEID,SNO FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & "", cn)
                Dim Mschid As Integer = Val(mSchrow(0).ToString)
                itemids = itemids.Substring(0, itemids.Length - 1)
                Dim ccode As String = ""
                ccode = Val(GetSqlValue(cn, "SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='C' AND SCHEMECODE = '" & Mschid & "'").ToString)
                strSql = "SELECT 'CHECK' FROM " & cnAdminDb & "..SCHEMENOOFFER WHERE SCHEMEID = '" & ccode & "' AND ITEMID IN (" & itemids & ") AND ISNULL(ACTIVE,'')='Y' "
                If objGPack.GetSqlValue(strSql).Length > 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End If
    End Function
    Private Sub txtChitCardRegNo_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCHITCardRegNo_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If CheckRegNo() Then Exit Sub
            If CheckClose(True) Then Exit Sub
            If CheckAuth() Then Exit Sub
            If CheckNoOffer() = True Then
                If MsgBox("No Offer Item available in sales" & vbCrLf & vbCrLf & " Do You Wish to Continue without bonus ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    _chitNoOffLock = True
                Else
                    txtCHITCardBonus_AMT.Text = ""
                    Exit Sub
                End If
            End If

            ''Otp Prompt when Duplicate Card Issued
            Dim Optionid As Integer = 0
            strSql = "SELECT OPTIONID FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME ='CHITPRECLOSE' AND ACTIVE = 'Y'"
            Optionid = Val("" & objGPack.GetSqlValue(strSql, , , tran))
            If Optionid <> 0 Then
                strSql = vbCrLf + " SELECT (CASE WHEN MAX(S.Instalment)>MAX(ST.INSTALLMENT) THEN 'P' ELSE 'N' END) SCTYPE"
                strSql += vbCrLf + " FROM " & chitMainDb & "..SCHEMEMAST AS A"
                strSql += vbCrLf + " INNER JOIN " & chitMainDb & "..SCHEME AS S ON S.SCHEMEID=A.SCHEMEID"
                strSql += vbCrLf + " INNER JOIN " & chitTrandb & "..SCHEMETRAN AS ST ON ST.GROUPCODE=A.GROUPCODE AND ST.REGNO=A.REGNO AND ISNULL(CANCEL,'')=''"
                strSql += vbCrLf + " WHERE A.GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND A.REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " "
                If objGPack.GetSqlValue(strSql, , "N", tran).ToString = "P" Then
                    If MsgBox("CHIT PRECLOSE." & vbCrLf & vbCrLf & " Have an OTP to Proceed ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then txtCHITCardRegNo_NUM.Focus() : Exit Sub
                    If Not usrpwdok("CHITPRECLOSE", True) Then txtCHITCardRegNo_NUM.Focus() : Exit Sub
                    SendKeys.Send("{TAB}")
                End If
            End If

            strSql = "SELECT OPTIONID FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME ='CHITMDATE_PRECLOSE' AND ACTIVE = 'Y'"
            Optionid = Val("" & objGPack.GetSqlValue(strSql, , , tran))
            If Optionid <> 0 Then
                Dim tempmaturedate As Date = funcMatureDate(txtCHITCardGrpCode.Text, Val(txtCHITCardRegNo_NUM.Text))
                Dim maturedate As Date
                If Val(chitClose_mdateDays.ToString) <> 0 Then
                    maturedate = tempmaturedate.AddDays(Val(chitClose_mdateDays.ToString) * -1)
                Else
                    maturedate = tempmaturedate
                End If
                If BillDate < maturedate Then
                    strSql = "SELECT PWDTYPE FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME ='CHITMDATE_PRECLOSE' AND ACTIVE = 'Y'"
                    Dim PwdType As String = objGPack.GetSqlValue(strSql, , , tran)
                    If PwdType <> "S" Then
                        If Val(GetuserPwd(Optionid, cnCostId, userId).ToString) = 0 Then
                            MsgBox("Otp Not Generated for Maturity Date :" & maturedate.ToString("dd-MM-yyyy") & ".", MsgBoxStyle.Information)
                            txtCHITCardBonus_AMT.Text = ""
                            Exit Sub
                        End If
                    End If
                    If Not usrpwdok("CHITMDATE_PRECLOSE", True) Then txtCHITCardRegNo_NUM.Focus() : txtCHITCardBonus_AMT.Text = "" : Exit Sub
                    SendKeys.Send("{TAB}")
                End If
            End If

            If autoPost = True Then GetChitDetails()
            If mPartweightadj Then txtSettleWt.Focus() : txtSettleWt.SelectAll() Else SendKeys.Send("{TAB}")
            strSql = " SELECT COUNT(*) FROM " & chitMainDb & "..CUSTOMERPRIZE P"
            strSql += " WHERE GROUPCODE  = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
            If Val(objGPack.GetSqlValue(strSql)) > 0 Then
                txtCHITCardPrize_AMT.ReadOnly = False
            End If
            If SPECIFICFORMAT = 1 And _chitNoOffLock = True Then
                txtCHITCardBonus_AMT.Text = 0
            End If
        End If
    End Sub
#Region "User Defined Events"
    Private Sub chitDetail_Gotfocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
        txtCHITCardGift_AMT.GotFocus, txtCHITCardPrize_AMT.GotFocus _
        , txtCHITCardBonus_AMT.GotFocus, txtCHITCardDeduction_AMT.GotFocus _
        , txtCHITCardAmount_AMT.GotFocus
        If Not autoPost Then Exit Sub

        If Not (CType(sender, TextBox).Name <> txtCHITCardAmount_AMT.Name) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub chitDetail_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _
        txtCHITCardAmount_AMT.TextChanged, txtCHITCardGift_AMT.TextChanged, txtCHITCardPrize_AMT.TextChanged _
        , txtCHITCardBonus_AMT.TextChanged, txtCHITCardDeduction_AMT.TextChanged, txtCHITGST_AMT.TextChanged
        Dim tot As Double = Nothing
        tot = Val(txtCHITCardAmount_AMT.Text) + Val(txtCHITCardGift_AMT.Text) - Val(txtCHITCardDeduction_AMT.Text) _
        + Val(txtCHITCardPrize_AMT.Text) + Val(txtCHITCardBonus_AMT.Text) + Val(txtCHITGST_AMT.Text)
        txtCHITCardTotal_AMT.Text = IIf(tot <> 0, Format(tot, "0.00"), Nothing)
    End Sub
    Private Sub chitDetail_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles _
        txtCHITCardGrpCode.KeyDown, txtCHITCardRegNo_NUM.KeyDown,
        txtCHITCardAmount_AMT.KeyDown, txtCHITCardGift_AMT.KeyDown, txtCHITCardPrize_AMT.KeyDown _
        , txtCHITCardBonus_AMT.KeyDown, txtCHITCardDeduction_AMT.KeyDown
        If e.KeyCode = Keys.Down And gridCHITCard.RowCount > 0 Then
            gridCHITCard.Focus()
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
#End Region

    Private Sub txtChitCardRegNo_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCHITCardRegNo_NUM.TextChanged
        txtCHITCardAmount_AMT.Clear()
        txtCHITCardPrize_AMT.Clear()
        txtCHITCardBonus_AMT.Clear()
        txtCHITCardDeduction_AMT.Clear()
        txtCHITCardGift_AMT.Clear()
        lblInterestPer.Text = ""
    End Sub

    Private Sub txtChitCardGrpCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCHITCardGrpCode.TextChanged
        txtCHITCardAmount_AMT.Clear()
        txtCHITCardPrize_AMT.Clear()
        txtCHITCardBonus_AMT.Clear()
        txtCHITCardDeduction_AMT.Clear()
        txtCHITCardGift_AMT.Clear()
    End Sub

    Private Sub gridChitCard_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridCHITCard.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridChitCard_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridCHITCard.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridCHITCard.RowCount > 0 Then gridCHITCard.CurrentCell = gridCHITCard.Rows(gridCHITCard.CurrentRow.Index).Cells("grpcode")
            With gridCHITCard.CurrentRow
                cmbCHITtCardType_MAN.Text = .Cells("CARDTYPE").Value.ToString
                txtCHITCardGrpCode.Text = .Cells("GRPCODE").Value.ToString
                txtCHITCardRegNo_NUM.Text = .Cells("REGNO").Value.ToString
                txtCHITCardAmount_AMT.Text = .Cells("AMOUNT").Value.ToString
                txtCHITCardGift_AMT.Text = .Cells("GIFT").Value.ToString
                txtCHITCardPrize_AMT.Text = .Cells("PRIZE").Value.ToString
                txtCHITCardBonus_AMT.Text = .Cells("BONUS").Value.ToString
                txtCHITCardDeduction_AMT.Text = .Cells("DEDUCTION").Value.ToString
                lblPname.Text = .Cells("NAME").Value.ToString
                lblMobileNo.Text = .Cells("MOBILENO").Value.ToString
                mWeight = Val(.Cells("WEIGHT").Value.ToString)
                mBonweight = Val(.Cells("BONUSWT").Value.ToString)
                txtCHITCardRowIndex.Text = gridCHITCard.CurrentRow.Index
                lblBonusDeduction.Text = "Bonus-Deduction : " + Val(Math.Round(Val(txtCHITCardBonus_AMT.Text.ToString) - Val(txtCHITCardDeduction_AMT.Text.ToString), 2)).ToString + " [" + .Cells("GRPCODE").Value.ToString + "-" + .Cells("REGNO").Value.ToString + "]"
                cmbCHITtCardType_MAN.Focus()
            End With
        End If
    End Sub

    Private Sub gridChitCard_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridCHITCard.UserDeletedRow
        dtChitAdj.AcceptChanges()
        CalcTotal()
        If Not gridCHITCard.RowCount > 0 Then cmbCHITtCardType_MAN.Focus()
    End Sub

    Public Function InsertChitCardDetail(
    ByVal batchno As String, ByVal tNo As Integer, ByVal billdate As Date, ByVal billCashCounterId As String,
    ByVal billCostId As String, ByVal VatExm As String, ByVal tran As OleDbTransaction, ByVal frmFlag As String _
    , ByVal InterStateBill As Boolean _
    , Optional ByVal remarks As String = Nothing _
    , Optional ByVal Dttemptb As String = Nothing, Optional ByVal EmpId As Integer = 0) As Boolean
        Dim mtranmode As String
        If chitMainDb = "" Or chitMainDb = Nothing Then
            chitMainDb = GetAdmindbSoftValue("CHITDBPREFIX", "", tran) + "SAVINGS"
            chitTrandb = GetAdmindbSoftValue("CHITDBPREFIX", "", tran) + "SH0708"
        End If
        For Each ro As DataRow In dtChitAdj.Rows
            Dim aPost As Boolean = True
            If objGPack.GetSqlValue("SELECT ISNULL(AUTOPOST,'') FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) = "N" Then aPost = False Else aPost = True
            'If aPost Then
            strSql = " SELECT CONVERT(VARCHAR(12),DOCLOSE,103) AS DOCLOSE,"
            strSql += " CASE WHEN ISNULL(CLOSETYPE,'') = 'B' THEN 'Bill'"
            strSql += "      WHEN ISNULL(CLOSETYPE,'') = 'C' THEN 'Cash' END AS CLOSETYPE"
            strSql += " ,BILLNO,CONVERT(VARCHAR(12),BILLDATE,103) AS BILLDATE "
            strSql += " FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE  = '" & ro!GRPCODE.ToString & "'  "
            strSql += " AND  REGNO  = " & Val(ro!REGNO.ToString) & ""
            strSql += " AND DOCLOSE IS NOT NULL AND DOCLOSE <> '1900-01-01 00:00:00'"
            If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
                tran.Rollback()
                tran = Nothing
                MsgBox("SCHEME!!!" + vbCrLf + "Group Code : " & ro!grpcode.ToString & " RegNo : " & ro!regno.ToString & " Already Closed..")
                Return True
            End If
            'strSql = " SELECT CTLTEXT FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID='GRSNETWTCALC'"
            'Dim GrsNetCalc As String = objGPack.GetSqlValue(strSql, , , tran).ToString
            'Dim WeightScheme As String
            'If GrsNetCalc = "Y" Then
            '    strSql = " SELECT ISNULL(BONUSWEIGHT,'') AS BONUSWEIGHT"
            '    strSql += " FROM " & chitMainDb & "..SCHEME WHERE SCHEMEID=(SELECT TOP 1 SCHEMEID"
            '    strSql += " FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE  = '" & ro!GRPCODE.ToString & "'"
            '    strSql += " AND REGNO  = " & Val(ro!REGNO.ToString) & ")"
            '    WeightScheme = objGPack.GetSqlValue(strSql, , , tran).ToString
            '    If WeightScheme = "Y" Then
            '        strSql = " SELECT CTLTEXT FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID='CATCODEGOLD'"
            '        Dim Catcode As String = objGPack.GetSqlValue(strSql, , , tran).ToString
            '        strSql = " SELECT ISNULL(SUM(GRSWT),0)GRSWT FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE  = '" & ro!GRPCODE.ToString & "'"
            '        strSql += " AND REGNO  = " & Val(ro!REGNO.ToString) & " AND ISNULL(CANCEL,'')='' AND ISNULL(CHEQUERETDATE,'')=''"
            '        Dim OGrswt As Decimal = Val(objGPack.GetSqlValue(strSql, , , tran).ToString)
            '        strSql = " SELECT ISNULL(SUM(WEIGHT),0)WEIGHT FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE  = '" & ro!GRPCODE.ToString & "'"
            '        strSql += " AND REGNO  = " & Val(ro!REGNO.ToString) & " AND ISNULL(CANCEL,'')='' AND ISNULL(CHEQUERETDATE,'')=''"
            '        Dim ONetwt As Decimal = Val(objGPack.GetSqlValue(strSql, , , tran).ToString)
            '        InsertIntoIssueReceipt("IIS", tNo, Catcode, 0, OGrswt, ONetwt, 0, 0, 0, 0, 0, 0, 0, 0, batchno, , , , , , , billCashCounterId, billCostId)
            '    End If
            'End If

            Dim accode As String = Nothing
            accode = objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)
            InsertIntoAccTran(frmFlag, tran, tNo, billdate, "D", accode,
            Val(ro!AMOUNT.ToString), 0, 0, 0, ChitPaymode, "" _
            , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, remarks, , billCashCounterId, billCostId, batchno, VatExm, Dttemptb)
            ''GIFT ACC
            accode = objGPack.GetSqlValue(" SELECT GIFTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)
            InsertIntoAccTran(frmFlag, tran, tNo, billdate, "D", accode,
            Val(ro!GIFT.ToString), 0, 0, 0, IIf(ChitPaymode = "SS", "CG", "HG"), "" _
            , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, remarks, , billCashCounterId, billCostId, batchno, VatExm, Dttemptb)
            ''PRIZE ACC
            accode = objGPack.GetSqlValue(" SELECT PRIZEAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)
            InsertIntoAccTran(frmFlag, tran, tNo, billdate, "D", accode,
            Val(ro!PRIZE.ToString), 0, 0, 0, IIf(ChitPaymode = "SS", "CZ", "HZ"), "" _
            , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, remarks, , billCashCounterId, billCostId, batchno, VatExm, Dttemptb)
            ''BONUS ACC
            If Val(ro!BONUS.ToString) > 0 Then mtranmode = "D" Else mtranmode = "C"
            accode = objGPack.GetSqlValue(" SELECT BONUSAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)
            InsertIntoAccTran(frmFlag, tran, tNo, billdate, mtranmode, accode,
            Val(ro!BONUS.ToString), 0, 0, 0, IIf(ChitPaymode = "SS", "CB", "HB"), "" _
            , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, remarks, , billCashCounterId, billCostId, batchno, VatExm, Dttemptb)
            ''DEDUCT AC
            accode = objGPack.GetSqlValue(" SELECT DEDUCTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)
            InsertIntoAccTran(frmFlag, tran, tNo, billdate, "C", accode,
            Val(ro!DEDUCTION.ToString), 0, 0, 0, IIf(ChitPaymode = "SS", "CD", "HD"), "" _
            , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, remarks, , billCashCounterId, billCostId, batchno, VatExm, Dttemptb)
            If ro!ispartwt.ToString = "Y" Then
                InsertintoSCHEMEADJTRAN(billdate, tNo, billCostId, ro!GRPCODE.ToString, ro!REGNO, Val(ro!AMOUNT.ToString), Val(ro!ADJWEIGHT.ToString), Val(ro!BONUSWT.ToString), Val(ro!BALWEIGHT.ToString), batchno, tran)
            End If
            If Val(ro!GST.ToString) > 0 Then
                If InterStateBill Then
                    InsertIntoAccTran(frmFlag, tran, tNo, billdate, "D", SchemeICode,
                                    Val(ro!GST.ToString), 0, 0, 0, "SV", "" _
                                    , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, remarks, , billCashCounterId, billCostId, batchno, VatExm, Dttemptb)
                Else
                    InsertIntoAccTran(frmFlag, tran, tNo, billdate, "D", SchemeSCode,
                                (Val(ro!GST.ToString) / 2), 0, 0, 0, "SV", "" _
                                , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, remarks, , billCashCounterId, billCostId, batchno, VatExm, Dttemptb)
                    InsertIntoAccTran(frmFlag, tran, tNo, billdate, "D", SchemeCCode,
                                (Val(ro!GST.ToString) / 2), 0, 0, 0, "SV", "" _
                                , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, remarks, , billCashCounterId, billCostId, batchno, VatExm, Dttemptb)
                End If
            End If

            If INTER_TRF_CHITCLOSE = "Y" Then
                strSql = " SELECT COSTID FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE  = '" & ro!GRPCODE.ToString & "'  "
                strSql += " AND  REGNO  = " & Val(ro!REGNO.ToString) & ""
                Dim ORGCOSTID As String = objGPack.GetSqlValue(strSql, , , tran).ToString
                If billCostId <> ORGCOSTID Then
                    Dim mremarks As String = "Internal Transfer entry generation"
                    Dim inttrfcode As String = objGPack.GetSqlValue(" SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ORGCOSTID & "'", , , tran)
                    accode = objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)
                    InsertIntoAccTran(frmFlag, tran, tNo, billdate, "C", accode,
                    Val(ro!AMOUNT.ToString), 0, 0, 0, "TI", inttrfcode _
                    , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, mremarks, , billCashCounterId, billCostId, batchno, VatExm, Dttemptb)

                    InsertIntoAccTran(frmFlag, tran, tNo, billdate, "D", inttrfcode,
                    Val(ro!AMOUNT.ToString), 0, 0, 0, "TI", accode _
                    , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, mremarks, , billCashCounterId, billCostId, batchno, VatExm, Dttemptb)

                    Dim inttrfcontracode As String = objGPack.GetSqlValue(" SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & billCostId & "'", , , tran)
                    SyncInsertIntoAccTran(frmFlag, tran, tNo, billdate, "D", accode,
                                        Val(ro!AMOUNT.ToString), 0, 0, 0, "TR", inttrfcontracode _
                                        , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, mremarks, , billCashCounterId, ORGCOSTID, batchno, VatExm)

                    SyncInsertIntoAccTran(frmFlag, tran, tNo, billdate, "C", inttrfcontracode,
                    Val(ro!AMOUNT.ToString), 0, 0, 0, "TR", accode _
                    , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, mremarks, , billCashCounterId, ORGCOSTID, batchno, VatExm)

                End If
            End If

            If ro!IsClose.ToString <> "N" Then
                ' If aPost Then
                strSql = " UPDATE " & chitMainDb & "..SCHEMEMAST SET"
                strSql += " DOCLOSE = '" & billdate.ToString("yyyy-MM-dd") & "'"
                strSql += " ,CLOSECOSTID= '" & billCostId & "'"
                strSql += " ,CLOSETYPE= 'B'"
                strSql += " ,BILLNO= " & tNo & ""
                strSql += " ,BILLDATE= '" & billdate.ToString("yyyy-MM-dd") & "'"
                strSql += " ,BONUS= " & Val(ro!BONUS.ToString) & ""
                strSql += " ,INTEREST= 0"
                strSql += " ,PRIZEVALUE= " & Val(ro!PRIZE.ToString) & ""
                strSql += " ,GIFTVALUE= " & Val(ro!GIFT.ToString) & ""
                strSql += " ,DEDUCTGIFTVALUE= 0"
                strSql += " ,DEDUCTION= " & Val(ro!DEDUCTION.ToString) & ""
                strSql += " ,CLOSEDBY= 'A'"
                strSql += " ,CLOSIUSER= " & userId
                strSql += " ,CLOSEEMPID= " & EmpId
                strSql += " ,CLOSEDATE= '" & billdate.ToString("yyyy-MM-dd") & "'"
                strSql += " WHERE GROUPCODE = '" & ro!GRPCODE.ToString & "' "
                strSql += " AND REGNO = " & Val(ro!REGNO.ToString) & ""
                ' ExecQuery(SyncMode.Transaction, strSql, cn, tran, billCostId)
                If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID = 'SYNC-SAVINGSDB'", "CTLTEXT", "N", tran) = "Y" Then
                    ExecQuery(SyncMode.Master, strSql, cn, tran, billCostId, , , , , , , , , chitMainDb)
                Else
                    ExecQuery(SyncMode.Master, strSql, cn, tran, billCostId, , , , , , , , , , chitMainDb)
                End If
            End If
            'End If
        Next
    End Function

    Private Sub InsertintoSCHEMEADJTRAN(ByVal TranDate As Date, ByVal TRANNO As Integer,
    ByVal billCostId As String, ByVal GROUPCODE As String, ByVal RegNo As Integer, ByVal AMOUNT As Double,
    ByVal Weight As Double, ByVal TotbonWt As Double, ByVal TotbalWt As Double, ByVal BATCHNO As String, ByVal tran As OleDbTransaction)
        If Weight = 0 And AMOUNT = 0 Then Exit Sub
        Dim _adjsno As String = GetNewSno_Chit(TranSnoType.SCHEMEADJTRANCODE, tran, chitTrandb)
        strSql = " INSERT INTO " & chitTrandb & "..SCHEMEADJTRAN"
        strSql += " (SNO,TRANDATE,TRANNO,COSTID,GROUPCODE,REGNO,AMOUNT,WEIGHT,TOTALWEIGHT,BONUSWEIGHT,BATCHNO) VALUES "
        strSql += " ('" & _adjsno & "','" & TranDate.ToString("yyyy-MM-dd") & "'," & TRANNO & ",'" & billCostId & "','" & GROUPCODE & "'," & RegNo & "," & AMOUNT & "," & Weight & "," & TotbalWt & "," & TotbonWt & ",'" & BATCHNO & "') "
        ExecQuery(SyncMode.Master, strSql, cn, tran, billCostId)
        strSql = ""
        cmd = Nothing
    End Sub


    Private Sub InsertIntoAccTran _
   (ByVal frmFlag As String, ByVal tran As OleDbTransaction, ByVal tNo As Integer,
   ByVal billDate As Date,
   ByVal tranMode As String,
   ByVal accode As String,
   ByVal amount As Double,
   ByVal pcs As Integer,
   ByVal grsWT As Double,
   ByVal netWT As Double,
   ByVal payMode As String,
   ByVal contra As String,
   Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing,
   Optional ByVal chqCardNo As String = Nothing,
   Optional ByVal chqDate As String = Nothing,
   Optional ByVal chqCardId As Integer = Nothing,
   Optional ByVal chqCardRef As String = Nothing,
   Optional ByVal Remark1 As String = Nothing,
   Optional ByVal Remark2 As String = Nothing,
   Optional ByVal BillCashCounterId As String = Nothing,
   Optional ByVal BillCostId As String = Nothing,
   Optional ByVal Batchno As String = Nothing,
   Optional ByVal VATEXM As String = Nothing,
   Optional ByVal Dttemptb As String = Nothing
   )
        If amount = 0 Then Exit Sub
        If Dttemptb = Nothing Then
            strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        Else
            strSql = " INSERT INTO " & cnStockDb & ".." & Dttemptb & ""
        End If
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
        strSql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
        strSql += " ,TRANFLAG)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & billDate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strSql += " ," & Math.Abs(pcs) & "" 'PCS
        strSql += " ," & Math.Abs(grsWT) & "" 'GRSWT
        strSql += " ," & Math.Abs(netWT) & "" 'NETWT
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate = Nothing Then
            strSql += " ,NULL" 'REFDATE
        Else
            strSql += " ,'" & refDate & "'" 'REFDATE
        End If
        strSql += " ,'" & payMode & "'" 'PAYMODE
        strSql += " ,'" & chqCardNo & "'" 'CHQCARDNO
        strSql += " ," & chqCardId & "" 'CARDID
        strSql += " ,'" & chqCardRef & "'" 'CHQCARDREF
        If chqDate = Nothing Then
            strSql += " ,NULL" 'CHQDATE
        Else
            strSql += " ,'" & chqDate & "'" 'CHQDATE
        End If
        strSql += " ,''" 'BRSFLAG
        strSql += " ,NULL" 'RELIASEDATE
        strSql += " ,'" & frmFlag & "'" 'FROMFLAG
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK2
        strSql += " ,'" & contra & "'" 'CONTRA
        strSql += " ,'" & Batchno & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'" & BillCashCounterId & "'" 'CASHID
        strSql += " ,'" & BillCostId & "'" 'COSTID
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & Tranflag & "'" 'TRANFLAG
        strSql += " )"
        If Dttemptb = Nothing Then
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        Else
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
        End If
        strSql = ""
        cmd = Nothing
    End Sub
    Public Sub InsertIntoIssueReceipt(
         ByVal tranType As String, ByVal tNo As Integer, ByVal CATCODE As String, ByVal pcs As Integer, ByVal grsWt As Double, ByVal netWT As Double _
        , ByVal wast As Double _
        , ByVal mc As Double, ByVal rate As Double, ByVal amount As Double _
        , ByVal vat As Double, ByVal stnAmt As Double, ByVal miscAmt As Double, ByVal empId As Integer _
        , Optional ByVal Batchno As String = Nothing _
        , Optional ByVal tranNo As Integer = Nothing _
        , Optional ByVal Findisc As Double = Nothing, Optional ByVal ACCODE As Double = Nothing _
        , Optional ByVal Itemid As Integer = 0, Optional ByVal SubItemid As Integer = 0 _
        , Optional ByVal ItemCtrid As Integer = 0 _
        , Optional ByVal BillCashCounterId As String = Nothing _
        , Optional ByVal BillCostId As String = Nothing)
        Dim issSno As String
        issSno = GetNewSno(TranSnoType.ISSUECODE, tran)
        strSql = " INSERT INTO " & cnStockDb & "..ISSUE"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
        strSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
        strSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
        strSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
        strSql += " ,RATE,SALEMODE,GRSNET"
        strSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
        strSql += " ,COMPANYID,FLAG,EMPID,TAGPCS,TAGGRSWT"
        strSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
        strSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
        strSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE"
        strSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
        strSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT"
        strSql += " ,RUNNO,CASHID,TAX,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER"
        strSql += " )"
        strSql += " VALUES("
        strSql += " '" & issSno & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO
        strSql += " ,'" & BillDate & "'" 'TRANDATE
        strSql += " ,'" & tranType & "'" 'TRANTYPE
        strSql += " ," & pcs & "" 'PCS
        strSql += " ," & grsWt & "" 'GRSWT
        strSql += " ," & netWT & "" 'NETWT
        strSql += " ,0" 'LESSWT
        strSql += " ,0" 'PUREWT '0
        strSql += " ,''" 'TAGNO
        strSql += " ," & Itemid & "" 'ITEMID
        strSql += " ," & SubItemid & "" 'SUBITEMID
        strSql += " ,0" 'WASTPER
        strSql += " ," & wast & "" 'WASTAGE
        strSql += " ,0" 'MCGRM
        strSql += " ," & mc & "" 'MCHARGE
        strSql += " ," & amount & "" 'AMOUNT
        strSql += " ," & rate & "" 'RATE
        strSql += " ,''" 'SALEMODE
        strSql += " ,'N'" 'GRSNET
        strSql += " ,''" 'TRANSTATUS ''
        strSql += " ,''" 'REFNO ''
        strSql += " ,NULL" 'REFDATE NULL
        strSql += " ,'" & BillCostId & "'" 'COSTID 
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,''" 'FLAG 
        strSql += " ," & empId & "" 'EMPID
        strSql += " ,0" 'TAGPCS
        strSql += " ,0" 'TAGGRSWT
        strSql += " ,0" 'TAGNETWT
        strSql += " ,0" 'TAGRATEID
        strSql += " ,0" 'TAGSVALUE
        strSql += " ,'0'" 'TAGDESIGNER
        strSql += " ," & ItemCtrid & "" 'ITEMCTRID
        strSql += " ,0" 'ITEMTYPEID
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & CATCODE & "')", , , tran)) & "" 'PURITY
        strSql += " ,''" 'TABLECODE
        strSql += " ,''" 'INCENTIVE
        strSql += " ,''" 'WEIGHTUNIT
        strSql += " ,'" & CATCODE & "'" 'CATCODE
        strSql += " ,'" & ACCODE & "'" 'ACCODE
        strSql += " ,0" 'ALLOY
        strSql += " ,'" & Batchno & "'" 'BATCHNO
        strSql += " ,''" 'REMARK1
        strSql += " ,''" 'REMARK2
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,0" 'DISCOUNT
        strSql += " ,''" 'RUNNO
        strSql += " ,'" & BillCashCounterId & "'" 'CASHID
        strSql += " ," & vat & "" 'TAX
        strSql += " ," & stnAmt & "" 'STONEAMT
        strSql += " ," & miscAmt & "" 'MISCAMT
        strSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & CATCODE & "'", , , tran) & "'" 'METALID
        strSql += " ,''" 'STONEUNIT
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
    End Sub

    Private Sub SyncInsertIntoAccTran _
       (ByVal frmFlag As String, ByVal tran As OleDbTransaction, ByVal tNo As Integer,
       ByVal billDate As Date,
       ByVal tranMode As String,
       ByVal accode As String,
       ByVal amount As Double,
       ByVal pcs As Integer,
       ByVal grsWT As Double,
       ByVal netWT As Double,
       ByVal payMode As String,
       ByVal contra As String,
       Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing,
       Optional ByVal chqCardNo As String = Nothing,
       Optional ByVal chqDate As String = Nothing,
       Optional ByVal chqCardId As Integer = Nothing,
       Optional ByVal chqCardRef As String = Nothing,
       Optional ByVal Remark1 As String = Nothing,
       Optional ByVal Remark2 As String = Nothing,
       Optional ByVal BillCashCounterId As String = Nothing,
       Optional ByVal BillCostId As String = Nothing,
       Optional ByVal Batchno As String = Nothing,
       Optional ByVal VATEXM As String = Nothing)
        If amount = 0 Then Exit Sub

        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
        strSql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
        strSql += " ,TRANFLAG)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & billDate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strSql += " ," & Math.Abs(pcs) & "" 'PCS
        strSql += " ," & Math.Abs(grsWT) & "" 'GRSWT
        strSql += " ," & Math.Abs(netWT) & "" 'NETWT
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate = Nothing Then
            strSql += " ,NULL" 'REFDATE
        Else
            strSql += " ,'" & refDate & "'" 'REFDATE
        End If
        strSql += " ,'" & payMode & "'" 'PAYMODE
        strSql += " ,'" & chqCardNo & "'" 'CHQCARDNO
        strSql += " ," & chqCardId & "" 'CARDID
        strSql += " ,'" & chqCardRef & "'" 'CHQCARDREF
        If chqDate = Nothing Then
            strSql += " ,NULL" 'CHQDATE
        Else
            strSql += " ,'" & chqDate & "'" 'CHQDATE
        End If
        strSql += " ,''" 'BRSFLAG
        strSql += " ,NULL" 'RELIASEDATE
        strSql += " ,'" & frmFlag & "'" 'FROMFLAG
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK2
        strSql += " ,'" & contra & "'" 'CONTRA
        strSql += " ,'" & Batchno & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'" & BillCashCounterId & "'" 'CASHID
        strSql += " ,'" & BillCostId & "'" 'COSTID
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & Tranflag & "'" 'TRANFLAG
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId, , , , False)
        strSql = ""
        cmd = Nothing
    End Sub




    Private Sub cmbChittCardType_MAN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCHITtCardType_MAN.Leave

        If objGPack.GetSqlValue("SELECT ISNULL(AUTOPOST,'') FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & cmbCHITtCardType_MAN.Text & "'") = "N" Then autoPost = False Else autoPost = True
        txtCHITCardAmount_AMT.Clear()
        txtCHITCardPrize_AMT.Clear()
        txtCHITCardBonus_AMT.Clear()
        txtCHITCardDeduction_AMT.Clear()
        txtCHITCardGift_AMT.Clear()
        lblInterestPer.Text = ""
        If cmbCHITtCardType_MAN.Text <> "" And cmbCHITtCardType_MAN.Items.Contains(cmbCHITtCardType_MAN.Text) Then
            strSql = " SELECT SCHEMECODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & cmbCHITtCardType_MAN.Text & "'"
            txtCHITCardSchemeId.Text = objGPack.GetSqlValue(strSql)
        End If
    End Sub

    Private Sub txtChitCardSchemeId_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCHITCardSchemeId.LostFocus
        If txtCHITCardSchemeId.Text <> "" Then
            strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE SCHEMECODE = '" & txtCHITCardSchemeId.Text & "'"
            cmbCHITtCardType_MAN.Text = objGPack.GetSqlValue(strSql)
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtSettleWt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSettleWt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtSettleWt.Text) <> 0 And Val(txtSettleWt.Text) <= Math.Round(mBalweight, 3) Then
                mAdjweight = Val(txtSettleWt.Text)
                Isclosed = "N"
                If Math.Round(mBalweight, 3) = mAdjweight Then Isclosed = IIf(MsgBox("Close Chit", MsgBoxStyle.YesNo) = MsgBoxResult.No, "N", "Y")
                txtCHITCardAmount_AMT.Text = Format(mAdjweight * mSettrate, "#0.00")
                'txtCHITCardAmount_AMT.Focus()
                Dim dtPartlybons As New DataTable
                If Partlybonusschid.ToString <> "" And Val(txtCHITCardBonus_AMT.Text) <> 0 Then
                    strSql = " SELECT * FROM " & cnChitCompanyid & "SAVINGS..SCHEME WHERE SCHEMEID IN (" & Partlybonusschid.ToString & ") "
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtPartlybons)
                    For i As Integer = 0 To dtPartlybons.Rows.Count - 1
                        If dtPartlybons.Rows(i).Item("SCHEMEID").ToString = txtCHITCardSchemeId.Text Then
                            txtCHITCardBonus_AMT.Text = ""
                            Exit For
                        End If
                    Next
                End If
                Me.SelectNextControl(txtCHITCardAmount_AMT, True, True, True, True)
                txtCHITCardTotal_AMT.Focus()
                Exit Sub
            End If
            txtCHITCardTotal_AMT.Focus()
        End If
    End Sub

    Private Sub txtChitslipNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtChitslipNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If CLIENTID_AS_SLIPNO Then
                If CheckClientId() Then Exit Sub
                If txtCHITCardRegNo_NUM.Text <> "" Then Call txtChitCardRegNo_NUM_KeyPress(sender, e)
            Else
                If Checkslipno() Then Exit Sub
                If txtCHITCardRegNo_NUM.Text <> "" Then Call txtChitCardRegNo_NUM_KeyPress(sender, e)
            End If
            If mPartweightadj Then lblSetwt.Focus() : Exit Sub
        End If
    End Sub

    Private Function CheckClientId() As Boolean
        strSql = " SELECT * FROM " & chitMainDb & "..SCHEMEMAST CL "
        strSql += " WHERE VAID = '" & txtChitslipNo.Text & "'"
        Dim dchitclrow As DataRow
        dchitclrow = GetSqlRow(strSql, cn)
        If Not dchitclrow Is Nothing Then
            txtCHITCardGrpCode.Text = dchitclrow.Item("GROUPCODE").ToString
            txtCHITCardRegNo_NUM.Text = dchitclrow.Item("REGNO").ToString
            Return False
        Else
            MsgBox("Given Client ID is not valid " & vbCrLf & "Please Check ", MsgBoxStyle.Information)
            txtChitslipNo.Focus()
            Return True
        End If
    End Function

    Private Function Checkslipno() As Boolean
        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITCLOSESLIP", "N") = "Y" Then
            strSql = " SELECT * FROM " & chitMainDb & "..ESTIMATECLOSE CL "
            strSql += " WHERE ISNULL(CANCEL,'') <> 'Y'"
            strSql += " AND SLIPNO= " & Val(txtChitslipNo.Text) & ""
            Dim dchitclrow As DataRow
            dchitclrow = GetSqlRow(strSql, cn)
            If Not dchitclrow Is Nothing Then
                txtCHITCardGrpCode.Text = dchitclrow.Item("GROUPCODE").ToString
                txtCHITCardRegNo_NUM.Text = dchitclrow.Item("REGNO").ToString
                Return False
            Else
                MsgBox("Given Slip No. Not Valid " & vbCrLf & "Please Check ", MsgBoxStyle.Information)
                txtChitslipNo.Focus()
                Return True
            End If
        End If
    End Function

    Private Sub txtCHITGST_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCHITGST_AMT.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If lblCardLost.Visible Then
            lblCardLost.Visible = False
        Else
            lblCardLost.Visible = True
        End If
        If lblPDCCheque.Visible Then
            lblPDCCheque.Visible = False
        Else
            lblPDCCheque.Visible = True
        End If
    End Sub

    Private Sub cmbBonusType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBonusType.SelectedIndexChanged
        If txtCHITCardGrpCode.Text <> "" And txtCHITCardRegNo_NUM.Text <> "" Then
            txtChitCardRegNo_NUM_KeyPress(sender, New KeyPressEventArgs(Chr(Keys.Enter)))
        End If
    End Sub
End Class