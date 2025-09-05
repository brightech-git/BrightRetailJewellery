Imports System.Data.OleDb
Public Class frmSchOffer
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim objEST As New frmEstimation1
    Dim chitMainDb As String = Nothing
    Dim chitTrandb As String = Nothing
    Public dtChitAdj As New DataTable
    Public dtBookedSchView As New DataTable
    Public dtReservedItem As New DataTable
    Public dtchitadjdet As New DataTable
    Dim chitEdit As Boolean = False
    Dim autoPost As Boolean = True
    Public ChitPaymode As String = "SS"
    Public ChitMetalid As String = "G"
    Public BillDate As Date
    Public Tranflag As String = ""
    Public Bookrate As Decimal = 0
    Public Schtype As String = "R"
    Dim dtsoftkeyss As New DataTable
    Dim recdamount As Decimal
    Public IsOrAdv As Boolean = False
    Public IsOrRate As String = ""
    Dim mAdvSchAdjAmt As Decimal = 0
    Dim DaysinYear As Decimal = GetAdmindbSoftValue("DAYSINYEAR", 365)
    Public dtGridSASR As DataTable
    Dim CloseType As String = "W"
    Dim _ChitTotWt As Double = 0
    Dim mPartweightadj As Boolean = False
    Dim chitClose_mdateDays As String = GetAdmindbSoftValue("CHIT_CLOSE_MDATE_PREDAYS", "0")

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

        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITEDIT", "N").ToUpper = "Y" Then
            chitEdit = True
        End If

        With dtChitAdj.Columns
            .Add("CARDTYPE", GetType(String))
            .Add("SLIPNO", GetType(Integer))
            .Add("GRPCODE", GetType(String))
            .Add("REGNO", GetType(Integer))
            .Add("AMOUNT", GetType(Double))
            .Add("RATE", GetType(Double))
            .Add("WEIGHT", GetType(Decimal))
            .Add("NOINS", GetType(Integer))
            .Add("OFFPERCENT", GetType(Decimal))
            .Add("OFFWEIGHT", GetType(Decimal))
            .Add("OFFAMOUNT", GetType(Double))
            .Add("BONUSAMOUNT", GetType(Double))
            .Add("FLATAMOUNT", GetType(Double))
            .Add("ACTUALAMOUNT", GetType(Double))
            .Add("VAPER", GetType(Double))
            .Add("DEDAMOUNT", GetType(Double))
            .Add("PRIZEAMOUNT", GetType(Double))
            .Add("GIFTAMOUNT", GetType(Double))
            .Add("ADVAMOUNT", GetType(Double))
            .Add("RATEDIFFER", GetType(String))
            .Add("BONUSTYPE", GetType(String))
            .Add("VBC", GetType(String))
            .Add("BALWEIGHT", GetType(Double))
            .Add("ADJWEIGHT", GetType(Double))
            .Add("ISPARTWT", GetType(String))
            .Add("ISCLOSE", GetType(String))
        End With
        gridCHITCard.DataSource = dtChitAdj

        txtCHITCardRate_AMT.Text = Bookrate
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
            '.Columns("WEIGHT").Visible = False
        End With
        FormatGridColumns(gridCHITCardTotal)
        StyleGridCheque(gridCHITCardTotal)


        With dtBookedSchView.Columns
            .Add("REGNO", GetType(String))
            .Add("GRSWT", GetType(Decimal))
            .Add("VAPER", GetType(Double))
            .Add("WASTAGE", GetType(Decimal))
            .Add("RATE", GetType(Double))
            .Add("GROSSAMT", GetType(Double))
            .Add("VATAMT", GetType(Double))
            .Add("NETAMT", GetType(Double))
            .Add("AMOUNT", GetType(Double))
            .Add("WASTPER", GetType(Double))
            .Add("MCGRM", GetType(Double))
            .Add("MC", GetType(Double))
            .Add("BONUS", GetType(Double))
        End With
        dgAdjdetails.DataSource = dtBookedSchView
        FormatGridColumns(dgAdjdetails)
        StyleGridadj()
        With dtchitadjdet.Columns
            .Add("ADJWT", GetType(Decimal))
            .Add("ADJAMOUNT", GetType(Double))
            .Add("ADJVAT", GetType(Double))
            .Add("ADJTOTAL", GetType(Double))
            .Add("ADJFINALAMT", GetType(Double))
        End With
        _ChitTotWt = 0
        mPartweightadj = False
    End Sub

    Private Sub StyleGridCheque(ByVal grid As DataGridView)
        gridCHITCardTotal.DefaultCellStyle.SelectionBackColor = grpCHIT.BackgroundColor
        With grid
            .Columns("CARDTYPE").Visible = False
            .Columns("SLIPNO").Width = txtCHITSlipNo.Width + 1
            .Columns("GRPCODE").Width = txtCHITCardGrpCode.Width + 1
            .Columns("REGNO").Width = txtCHITCardRegNo_NUM.Width + 1
            .Columns("AMOUNT").Width = txtCHITCardAmount_AMT.Width + 1
            .Columns("RATE").Width = txtCHITCardRate_AMT.Width + 1
            .Columns("WEIGHT").Width = txtCHITCardWT_WET.Width + 1
            .Columns("NOINS").Width = txtInsPaid.Width + 1
            .Columns("OFFPERCENT").Width = txtOffPer.Width + 1
            For cnt As Integer = 12 To grid.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            '  .Columns("CARDTYPE").Visible = False
        End With
    End Sub
    Private Function GETFLATVALUE(ByVal Cardid As Integer) As String
        Dim Totbonusamt As Decimal = 0
        Dim Vaslab As Decimal = 0
        Dim flatper As Decimal = 0
        Dim tempvaper As Decimal = 0
        Dim tempflatper As Decimal = 0
        For Each sasrRow As DataRow In dtReservedItem.Rows
            With sasrRow
                If !ENTFLAG.ToString = "" Then Exit For
                If !TRANTYPE.ToString = "SR" Or !TRANTYPE.ToString = "AR" Then Continue For
                Dim dtschrow As DataRow
                dtschrow = Getschdetail(Cardid, Val(!ITEMID.ToString), Val(!SUBITEMID.ToString))
                If dtschrow Is Nothing Then dtschrow = Getschdetail(Cardid, Val(!ITEMID.ToString), 0)
                If Not dtschrow Is Nothing Then
                    tempflatper = dtschrow.Item("FLATPER") : tempvaper = dtschrow.Item("va_slab")
                End If
                If flatper <> 0 Then
                    If flatper <> tempflatper Then MsgBox("Items have different Discount %", MsgBoxStyle.Information) : Return Nothing
                Else
                    flatper = tempflatper
                End If
                If Schtype = "V" Then
                    If Vaslab <> 0 Then
                        If Vaslab <> tempvaper Then MsgBox("Items have different VA slab", MsgBoxStyle.Information) : Return Nothing
                    Else
                        Vaslab = tempvaper
                    End If
                End If
                'Totbonusamt += !grossamt * (FLATPER / 100)

            End With
        Next
        Totbonusamt = (flatper / 100)
        Dim retstr As String = Totbonusamt.ToString & "," & Vaslab.ToString
        Return retstr

    End Function

    Private Function Getschdetail(ByVal cardid As Integer, ByVal ITEMID As Integer, ByVal SUBITEMID As Integer) As DataRow
        Dim dtsch As DataRow
        'Dim cardids As Integer = objGPack.GetSqlValue("select cardcode from " & cnAdminDb & "..creditcard where schemecode = " & cardid)
        Dim sqlqry As String = "select va_slab,flatper,vbc from " & cnAdminDb & "..schemeoffermast where schemeid= " & cardid & " and Itemid = " & ITEMID & " and subitemid = " & SUBITEMID
        dtsch = GetSqlRow(sqlqry, cn)
        If Not dtsch Is Nothing Then Return dtsch
    End Function

    Private Sub StyleGridadj()

        With dgAdjdetails
            .DefaultCellStyle.SelectionBackColor = grpCHIT.BackgroundColor
            .Columns("REGNO").Width = lblParticular.Width + 1
            .Columns("GRSWT").Width = lblGrswt.Width + 1
            .Columns("VAPER").Width = lblVa.Width + 1
            .Columns("WASTAGE").Width = lblWastage.Width + 1
            .Columns("AMOUNT").Width = lblGrsAmt.Width + 1
            .Columns("RATE").Width = lblRate.Width + 1
            .Columns("VATAMT").Width = lblVat.Width + 1
            .Columns("NETAMT").Width = lblNetAmt.Width + 1
            .Columns("AMOUNT").Visible = False
            For cnt As Integer = 9 To .ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            '  .Columns("CARDTYPE").Visible = False

        End With
    End Sub


    Private Sub frmChidAdj_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Call btnOk_Click(sender, e)


    End Sub

    Private Sub frmChidAdj_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

        If e.KeyChar = Chr(Keys.Enter) Then
            If Adjgrouper.Visible = True Then dgAdjdetails.Focus() : Exit Sub
            If txtCHITCardWT_WET.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmChidAdj_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITDB", "N").ToUpper <> "Y" Then
            MsgBox("SCHEME Transaction provision not enabled in this pack", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            Me.Close()
            Exit Sub
        End If
        chitMainDb = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITDBPREFIX", "") + "SAVINGS"
        chitTrandb = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITDBPREFIX", "") + "SH0708"
        If Not objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & chitMainDb & "'").Length > 0 Then
            MsgBox("SCHEME main database not found", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            Me.Close()
            Exit Sub
        End If
        If Not objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & chitTrandb & "'").Length > 0 Then
            MsgBox("SCHEME transaction database not found", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            Me.Close()
            Exit Sub
        End If
        objGPack.TextClear(Me)
        lblAddress.Text = ""
        _ChitTotWt = 0
        mPartweightadj = False
        'lblWeightSchemeDetail.Text = ""
        CalcTotal()
        If dtOffdet.Columns.Contains("Offgrm") = True Then dtOffdet.Columns("offgrm").Width = Label10.Width
        If dtOffdet.Columns.Contains("Offkey") = True Then dtOffdet.Columns("Offkey").Visible = False

        'dtOffdet.Rows(0).DefaultCellStyle.Padding.
    End Sub



    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        'If Not dtBookedAdvanceView.Rows.Count > 0 Then
        ' btnExit.Focus()
        ' End If
        dtBookedSchView.Clear()
        CalcTotal()
        'For Each ro As DataRow In dtChitAdj.Rows
        '    Dim row As DataRow = dtBookedSchView.NewRow
        '    row("REGNO") = "SCH:" & ro!GRPCODE & "/" & ro!REGNO
        '    row("GROSSAMT") = Val(ro!AMOUNT)
        '    row("RATE") = Bookrate
        '    row("GRSWT") = Val(ro!AMOUNT) / Bookrate

        '    dtBookedSchView.Rows.Add(row)
        '    dtBookedSchView.AcceptChanges()
        'Next
        Me.DialogResult = Windows.Forms.DialogResult.OK
        btnOk.Enabled = False
        Me.Close()
    End Sub
    Private Function CalcReceiptAmount(ByVal flagActualReceipt As Boolean) As Double
        Dim ReceivedAmount As Double = Nothing
        If flagActualReceipt Then
            strSql = " SELECT  ISNULL(SUM(CASE WHEN SYN = 'RECEIPT' THEN AMOUNT ELSE -1*AMOUNT END),0) AS AMOUNT,"
            strSql += " SCHEMENAME,'RECEIPT' AS SYN "
            strSql += " FROM "
            strSql += " ("
            strSql += " SELECT SUM(AMOUNT)AS AMOUNT,S.SCHEMENAME,'RECEIPT'AS SYN "
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
            strSql = " SELECT SUM(AMOUNT) AS AMOUNT FROM " & chitTrandb & "..SCHEMETRAN WHERE "
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

    Private Function CalcPrizeAmount_Oldadj(ByVal groupcode As String, ByVal regno As String) As Double
        strSql = "  SELECT ISNULL(DRAWSCHEME,'')DRAWSCHEME FROM " & chitMainDb & "..SCHEME "
        strSql += "  WHERE SCHEMEID IN "
        strSql += "     ( "
        strSql += "     SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST "
        strSql += "     WHERE GROUPCODE = '" & groupcode & "' "
        strSql += "     AND REGNO = " & Val(regno) & ""
        strSql += "     )"
        Dim drawScheme As String = objGPack.GetSqlValue(strSql)
        strSql = " SELECT "
        strSql += " (SELECT PRIZENAME FROM " & chitMainDb & "..PRIZEMAST WHERE PRIZEID = P.PRIZEID)"
        strSql += "  AS PRIZENAME,"
        strSql += " (SELECT PRIZEVALUE FROM " & chitMainDb & "..PRIZEMAST WHERE PRIZEID = P.PRIZEID)"
        If UCase(drawScheme) = "Y" Then
            strSql += " - ISNULL((SELECT ISNULL(SUM(AMOUNT),0) AS AMOUNT"
            strSql += " FROM " & chitTrandb & "..SCHEMETRAN WHERE "
            strSql += " GROUPCODE  = '" & groupcode & "' "
            strSql += " AND REGNO = " & Val(regno) & ""
            strSql += " AND ISNULL(CANCEL,'') = '' "
            strSql += " AND ISNULL(CHEQUERETDATE,'')=''),0)"
        End If
        strSql += " AS PRIZEVALUE,"
        strSql += " CONVERT(VARCHAR(12),PRIZEDATE,103) AS PRIZEDATE FROM " & chitMainDb & "..CUSTOMERPRIZE P"
        strSql += " WHERE GROUPCODE  = '" & groupcode & "' AND REGNO = " & Val(regno) & ""
        Return Val(objGPack.GetSqlValue(strSql))
    End Function
    Private Function CalcBonus_Oldadj(ByVal groupcode As String, ByVal regno As String) As Double
        Dim bonus As Double = Nothing
        strSql = " SELECT TOP 1 I.BONUS"
        strSql += " FROM " & chitMainDb & "..INSAMOUNT I "
        strSql += " INNER JOIN " & chitMainDb & "..SCHEMEMAST M "
        strSql += "   ON M.SCHEMEID = I.SCHEMEID AND I.GROUPCODE = '" & groupcode & "'"
        strSql += "   AND I.AMOUNT = (SELECT TOP 1 AMOUNT FROM " & chitTrandb & "..SCHEMECOLLECT WHERE GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & ")"
        bonus = Val(objGPack.GetSqlValue(strSql))

        strSql = " SELECT ISNULL(ANNUALINTCAL,'') AS ANNUALINTCAL FROM " & chitMainDb & "..SCHEME "
        strSql += " WHERE SCHEMEID IN "
        strSql += "       (SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST "
        strSql += "        WHERE GROUPCODE = '" & groupcode & "' "
        strSql += "        AND REGNO = " & Val(regno) & ")"
        If objGPack.GetSqlValue(strSql).ToUpper = "Y" Then
            strSql = " SELECT ISNULL(INTEREST,0) AS INTEREST FROM " & chitMainDb & "..SCHEME "
            strSql += " WHERE SCHEMEID IN "
            strSql += "       (SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST "
            strSql += "        WHERE GROUPCODE = '" & groupcode & "' "
            strSql += "        AND REGNO = " & Val(regno) & ")"
            Dim interest As Double = Val(objGPack.GetSqlValue(strSql))

            Dim NetDays As Integer = 0
            Dim MBon As Double = 0, TotBonus As Double = 0
            strSql = " SELECT RDATE,AMOUNT FROM " & chitTrandb & "..SCHEMETRAN "
            strSql += " WHERE GROUPCODE  = '" & groupcode & "' "
            strSql += " AND REGNO = " & Val(regno) & " "
            strSql += " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
            strSql += " ORDER BY INSTALLMENT"
            Dim dtAnnInt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAnnInt)
            For Each ro As DataRow In dtAnnInt.Rows
                If ro.Item("RDATE") <= Today.Date Then
                    NetDays = Val(ro.Item("AMOUNT").ToString) *
                    DateDiff(DateInterval.Day, ro.Item("RDATE"), Today.Date)
                    MBon = NetDays * Val(interest / 36500)
                    If MBon > 0 Then TotBonus = TotBonus + (NetDays * Val(interest / 36500))
                End If
            Next
            bonus = Math.Round(TotBonus, 3)
        End If
        Return bonus
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

    Private Function CalcBonus() As Double
        Dim bonus As Double = Nothing
        strSql = " SELECT TOP 1 I.BONUS"
        strSql += " FROM " & chitMainDb & "..INSAMOUNT I "
        strSql += " INNER JOIN " & chitMainDb & "..SCHEMEMAST M "
        strSql += "   ON M.SCHEMEID = I.SCHEMEID AND I.GROUPCODE = '" & txtCHITCardGrpCode.Text & "'"
        strSql += "   AND I.AMOUNT = (SELECT TOP 1 AMOUNT FROM " & chitTrandb & "..SCHEMECOLLECT WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ")"
        bonus = Val(objGPack.GetSqlValue(strSql))

        strSql = " SELECT ISNULL(ANNUALINTCAL,'') AS ANNUALINTCAL FROM " & chitMainDb & "..SCHEME "
        strSql += " WHERE SCHEMEID IN "
        strSql += "       (SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST "
        strSql += "        WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
        strSql += "        AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ")"
        If objGPack.GetSqlValue(strSql).ToUpper = "Y" Then
            strSql = " SELECT ISNULL(INTEREST,0) AS INTEREST FROM " & chitMainDb & "..SCHEME "
            strSql += " WHERE SCHEMEID IN "
            strSql += "       (SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST "
            strSql += "        WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
            strSql += "        AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ")"
            Dim interest As Double = Val(objGPack.GetSqlValue(strSql))

            Dim NetDays As Integer = 0
            Dim MBon As Double = 0, TotBonus As Double = 0
            strSql = " SELECT RDATE,AMOUNT FROM " & chitTrandb & "..SCHEMETRAN "
            strSql += " WHERE GROUPCODE  = '" & txtCHITCardGrpCode.Text & "' "
            strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " "
            strSql += " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
            strSql += " ORDER BY INSTALLMENT"
            Dim dtAnnInt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAnnInt)
            For Each ro As DataRow In dtAnnInt.Rows
                If ro.Item("RDATE") <= Today.Date Then
                    NetDays = Val(ro.Item("AMOUNT").ToString) *
                    DateDiff(DateInterval.Day, ro.Item("RDATE"), Today.Date)
                    MBon = NetDays * Val(interest / 36500)
                    If MBon > 0 Then TotBonus = TotBonus + (NetDays * Val(interest / 36500))
                End If
            Next
            bonus = Math.Round(TotBonus, 3)
        End If
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
        strSql += "   ON M.SCHEMEID = I.SCHEMEID AND I.GROUPCODE = '" & txtCHITCardGrpCode.Text & "'"
        Return Val(objGPack.GetSqlValue(strSql))
    End Function

    Private Sub CalcTotal()
        Dim totAmt As Double = Nothing
        Dim totweight As Decimal = Nothing
        Dim totoffweight As Decimal = Nothing
        Dim totoffAmt As Double = Nothing
        For Each ro As DataRow In dtChitAdj.Rows
            totAmt += Val(ro!AMOUNT.ToString)
            totweight += Val(ro!WEIGHT.ToString)
            totoffweight += Val(ro!OFFWEIGHT.ToString)
            totoffAmt += Val(ro!OFFAMOUNT.ToString)
        Next
        gridCHITCardTotal.Rows(0).Cells("AMOUNT").Value = IIf(totAmt <> 0, Format(totAmt, "0.00"), DBNull.Value)
        gridCHITCardTotal.Rows(0).Cells("WEIGHT").Value = IIf(totweight <> 0, Format(totweight, "0.000"), DBNull.Value)
        gridCHITCardTotal.Rows(0).Cells("OFFWEIGHT").Value = IIf(totoffweight <> 0, Format(totoffweight, "0.000"), DBNull.Value)
        gridCHITCardTotal.Rows(0).Cells("OFFAMOUNT").Value = IIf(totoffAmt <> 0, Format(totoffAmt, "0.00"), DBNull.Value)
    End Sub

    Private Function CheckGroupCode() As Boolean
        If txtCHITCardGrpCode.Text.Trim = "" Then txtCHITCardGrpCode.Focus() : Return True : Exit Function
        If Not objGPack.GetSqlValue("SELECT GROUPCODE FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "'").Length > 0 Then
            MsgBox("Invalid GroupCode", MsgBoxStyle.Information)
            txtCHITCardGrpCode.Focus()
            Return True
        End If
    End Function

    Private Function Checkslipno() As Boolean
        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITCLOSESLIP", "N") = "Y" Then
            strSql = " SELECT * FROM " & chitMainDb & "..ESTIMATECLOSE CL "
            strSql += " WHERE ISNULL(CANCEL,'') <> 'Y'"
            strSql += " AND SLIPNO= " & Val(txtCHITSlipNo.Text) & ""
            Dim dchitclrow As DataRow
            dchitclrow = GetSqlRow(strSql, cn)
            If Not dchitclrow Is Nothing Then
                txtCHITCardGrpCode.Text = dchitclrow.Item("GROUPCODE").ToString
                txtCHITCardRegNo_NUM.Text = dchitclrow.Item("REGNO").ToString
                Return False
            Else
                MsgBox("Given Slip No. Not Valid " & vbCrLf & "Please Check ", MsgBoxStyle.Information)
                txtCHITSlipNo.Focus()
                Return True
            End If
        End If
    End Function



    Private Function CheckRegNo() As Boolean
        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITCLOSESLIP", "N") = "Y" Then
            strSql = " SELECT * FROM " & chitMainDb & "..ESTIMATECLOSE CL "
            strSql += " WHERE ISNULL(CANCEL,'') <> 'Y'"
            strSql += " AND GROUPCODE = '" & txtCHITCardGrpCode.Text & "'"
            strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
            strSql += " AND EXISTS (SELECT 1 FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = CL.GROUPCODE AND REGNO = CL.REGNO )"
            Dim dchitclrow As DataRow
            dchitclrow = GetSqlRow(strSql, cn)

            If dchitclrow Is Nothing Then
                MsgBox("Slip Not Generate" & vbCrLf & "Member dosn't Close", MsgBoxStyle.Information)
                txtCHITCardRegNo_NUM.Focus()
                Return True
            Else
                'txtCHITCardRate_AMT.Text = Val("" & dchitclrow.Item("GIFTVALUE").ToString)
            End If
        End If
        strSql = " SELECT GROUPCODE FROM " & chitTrandb & "..SCHEMECOLLECT C"
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

            'strSql = " SELECT SCHEMECODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & cmbCHITtCardType_MAN.Text & "'"
            'txtCHITCardSchemeId.Text = objGPack.GetSqlValue(strSql)
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

    Private Sub SetAddress(Optional ByVal pserid As String = Nothing)
        strSql = " SELECT PNAME,SNAME,DOORNO,ADDRESS1,ADDRESS2,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE"
        strSql += " ,EMAIL,FAX FROM " & chitMainDb & "..PERSONALINFO "
        strSql += " WHERE PERSONALID = '" & pserid & "'"
        '(SELECT SNO FROM " & chitMainDb & "..SCHEMEMAST"
        '        strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = '" & Val(txtCHITCardRegNo_NUM.Text) & "'"
        'strSql += " )"
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
            'If .Item("DOORNO").ToString <> "" Then str += "," & vbCrLf & .Item("DOORNO").ToString
            'If .Item("ADDRESS1").ToString <> "" Then str += "," + vbCrLf & .Item("ADDRESS1").ToString
            'If .Item("ADDRESS2").ToString <> "" Then str += "," + vbCrLf & .Item("ADDRESS2").ToString
            'If .Item("AREA").ToString <> "" Then str += "," + vbCrLf & .Item("AREA").ToString
            'If .Item("CITY").ToString + .Item("PINCODE").ToString <> "" Then str += "," & vbCrLf & .Item("CITY").ToString + IIf(.Item("PINCODE").ToString <> "", "-", "") + .Item("PINCODE").ToString
            'If .Item("PHONERES").ToString <> "" Then str += "," & vbCrLf & "ph:" & .Item("PHONERES").ToString
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
            str += "," + vbCrLf & .Item("EMAIL").ToString
            str += "," + vbCrLf & .Item("FAX").ToString
        End With
        lblAddress.Text = str
    End Sub

    'Private Function Getschdetail(ByVal ITEMID As Integer, ByVal SUBITEMID As Integer, Optional ByVal Noins As Integer = Nothing) As DataRow
    '    Dim dtsch As DataRow
    '    With dtChitAdj
    '        For ii As Integer = 0 To .Rows.Count - 1
    '            Dim cardid As Integer = objGPack.GetSqlValue("select cardcode from " & cnAdminDb & "..creditcard where schemecode = '" & Val(.Rows(ii).Item("Cardtype").ToString) & "'")
    '            Dim sqlqry As String = "select " & cardid & " as cardid,va_slab,wast_slab,mc_slab,flatper,vbc,vatper from " & cnAdminDb & "..schemeoffermast where isnull(active,'Y') <> 'N' and  COSTID = '" & cnCostId & "' AND schemeid= " & cardid & " and Itemid = " & ITEMID & " and subitemid = " & SUBITEMID
    '            If Noins <> Nothing Then sqlqry += " and " & Noins & " between insfrom and insto"
    '            dtsch = GetSqlRow(sqlqry, cn)
    '            If Not dtsch Is Nothing Then Return dtsch
    '        Next
    '    End With
    'End Function

    Private Function computemorelessavg(ByVal fixva As Integer) As String
        Dim returnstr As String
        Dim moreschgrwt As Decimal = 0
        Dim moreschvawt As Decimal = 0
        Dim moreschavgper As Decimal = 0
        Dim lessschgrwt As Decimal = 0
        Dim lessschvawt As Decimal = 0
        Dim lessschavgper As Decimal = 0
        For Each sasrRow As DataRow In dtGridSASR.Rows
            With sasrRow
                If !ENTFLAG.ToString = "" Then Exit For
                If !TRANTYPE.ToString = "SR" Or !TRANTYPE.ToString = "AR" Then Continue For
                Dim vavalue As Decimal = (Val(!wastage.ToString) * !rate) '+ Val(!mc.ToString)
                Dim vawt As Decimal = Math.Round(vavalue / !rate, 3)
                Dim mwt As Decimal = IIf(!grsnet = "NET WT", !netwt, !grswt)
                Dim wastper As Decimal = Val(!wastageper.ToString)
                If wastper = 0 Then wastper = Math.Round(((vawt / mwt) * 100), 2)
                If wastper >= fixva Then moreschgrwt += mwt : moreschvawt += mwt + vawt Else lessschgrwt += mwt : lessschvawt += mwt + vawt
            End With
        Next
        If moreschgrwt > 0 Then moreschavgper = Math.Round(((moreschvawt / moreschgrwt) - 1) * 100, 2)
        If lessschgrwt > 0 Then lessschavgper = Math.Round(((lessschvawt / lessschgrwt) - 1) * 100, 2)
        returnstr = moreschavgper.ToString & "," & lessschavgper.ToString
        Return returnstr
    End Function

    Public Function chitBonus() As Double
        '        Dim AVGVASCHEME As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "AVGVASCHEME", "N") = "Y", True, False)
        '        Dim AVGRATESCHEME As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "AVGRATESCHEME", "N") = "Y", True, False)
        '        Dim slab As String = "Y"
        '        Dim schoffdt As New DataTable
        '        Dim TotAvgrate As Decimal = Val(GetRate_Purity(GetEntryDate(GetServerDate), objGPack.GetSqlValue("SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN 91.6 AND 92 AND METALID = '" & ChitMetalid & "' ORDER BY PURITY DESC")).ToString)
        '        TotAvgrate = Val("" & dtGridSASR.Compute("Avg(RATE)", Nothing))
        '        Dim avgrate As Decimal = TotAvgrate 'dtChitAdj.Compute("AVG(rate)", Nothing)
        '        Dim mschamt As Decimal = Val(gridCHITCardTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        '        Dim mschoffamt As Decimal = Val(gridCHITCardTotal.Rows(0).Cells("OFFAMOUNT").Value.ToString)
        '        Dim mschbonamt As Decimal = Val(dtChitAdj.Compute("sum(BONUSAMOUNT)", Nothing))
        '        Dim mschwt As Double = Math.Round(Val(gridCHITCardTotal.Rows(0).Cells("WEIGHT").Value.ToString), 4)
        '        Dim mschoffwt As Double = Math.Round(Val(gridCHITCardTotal.Rows(0).Cells("OFFWEIGHT").Value.ToString), 4)
        '        If mschoffwt = 0 Then
        '            mschoffwt = Math.Round(mschoffamt / avgrate, 3)
        '        End If
        '        If mschwt = 0 Then mschwt = Math.Round(mschoffamt / Val(TotAvgrate), 4)
        '        Dim mschActualamt As Decimal = Val(dtChitAdj.Compute("sum(ACTUALAMOUNT)", Nothing))
        '        Dim Balschamt As Decimal = mschoffamt 'mschamt
        '        Dim Balschamt_Vat As Decimal = mschoffamt 'mschamt
        '        Dim Balschwt As Decimal = mschoffwt 'mschwt
        '        If mschoffwt = 0 And mschoffamt = 0 Then Exit Function

        '        Dim TotGrwt As Decimal = 0
        '        Dim TotNetwt As Decimal = 0
        '        Dim TotWast As Decimal = 0
        '        Dim TotGrsamt As Decimal = 0
        '        Dim TotVat As Decimal = 0
        '        Dim TotNetamt As Decimal = 0
        '        Dim Totstnamt As Decimal = 0
        '        Dim TotMiscamt As Decimal = 0
        '        Dim Totmc As Decimal = 0

        '        Dim safilter As String = Nothing
        '        TotGrsamt = Val("" & dtGridSASR.Compute("sum(GROSSAMT)", safilter))
        '        TotGrwt = Val("" & dtGridSASR.Compute("sum(Netwt)", safilter))
        '        TotVat = Val("" & dtGridSASR.Compute("sum(Vat)", safilter))
        '        TotNetamt = Val("" & dtGridSASR.Compute("sum(AMOUNT)", safilter))
        '        TotWast = Val("" & dtGridSASR.Compute("sum(wastage)", safilter))
        '        Totmc = Val("" & dtGridSASR.Compute("sum(mc)", safilter))
        '        Totstnamt = Val("" & dtGridSASR.Compute("sum(stoneamt)", safilter))
        '        TotMiscamt = Val("" & dtGridSASR.Compute("sum(MiscAmt)", safilter))
        '        Dim vatper As Decimal = 0
        '        If TotGrsamt <> 0 And TotVat <> 0 Then vatper = (TotVat / TotGrsamt) * 100
        '        Dim CCloseType_Both As Boolean = False
        '        strSql = " SELECT ISNULL(CLOSETYPE,'')CLOSETYPE FROM " & cnChitCompanyid & "SAVINGS..SCHEME WHERE SCHEMEID IN ("
        '        strSql += vbCrLf + " SELECT SCHEMEID FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE='" & dtChitAdj.Rows(0)("GRPCODE").ToString & "' AND REGNO='" & dtChitAdj.Rows(0)("REGNO").ToString & "')"
        '        If objGPack.GetSqlValue(strSql, "CLOSETYPE", "S") = "B" Then
        '            CCloseType_Both = True
        '        End If
        '        If mschbonamt <> 0 And CCloseType_Both = False Then mschoffwt = 0 : mschoffamt = 0 : GoTo flatdisc
        '        Dim Balva As Decimal
        '        Dim findAvgva As Decimal = 0
        '        Dim mmaxwastper As Decimal = 0
        '        Dim DiscountVaper As Decimal = 0
        '        Dim Totdiscva As Decimal = 0

        '        Dim sqlqry As String = "select 1 from " & cnAdminDb & "..schemeoffermast "
        '        If Val(objGPack.GetSqlValue(sqlqry, , "0")) = 0 Then slab = "N"
        '        Dim mschslabva As Decimal = Val(dtChitAdj.Rows(0).Item("VAPER").ToString)
        '        Dim avgvastring As String = computemorelessavg(mschslabva)
        '        Dim avgvaarr() As String = Split(avgvastring, ",")
        '        Dim moreavgvaper As Decimal = Val("" & avgvaarr(0).ToString)
        '        Dim lessavgvaper As Decimal = Val("" & avgvaarr(1).ToString)
        '        If moreavgvaper = 0 Then moreavgvaper = lessavgvaper
        '        Dim VASLAB As Decimal = 0
        '        Dim VBC As String = "N"
        '        Dim FLATPER As Decimal = 0
        '        Dim Totdiscvat As Decimal = 0
        '        Dim excessva As Decimal = 0
        '        Dim TotVa As Decimal = 0
        '        Dim flatdisamt As Decimal
        '        Dim mexcessvat As Decimal = 0
        '        Dim TotExcessva As Decimal = 0

        '        'Dim offtabrowx As DataRow = dtOffTable.NewRow
        '        'offtabrowx("Item") = "Item /Subitem "
        '        'offtabrowx("SLAB") = "Slab%(VA)"
        '        'offtabrowx("Offgrm") = "Offer Wt"
        '        'offtabrowx("Offkey") = 0
        '        'offtabrowx("Offwast") = "Offer Wast"
        '        'offtabrowx("Offrate") = "@ Rate"
        '        'offtabrowx("Offmc") = "Offer Mc"
        '        'offtabrowx("Offvat") = "Offer Vat"
        '        'dtOffTable.Rows.Add(offtabrowx)

        '        schoffdt = dtBookedSchView.Clone
        '        schoffdt.Columns.Add("KEYNO", GetType(Integer))
        '        Dim disc_allowed_amt As Decimal = 0
        '        Dim _Discountva As Decimal = 0
        '        For Each sasrRow As DataRow In dtGridSASR.Select("ENTFLAG <>''", "WASTAGEPER")
        '            With sasrRow
        '                Dim Discountva As Decimal = 0
        '                Dim discountvat As Decimal = 0
        '                If !ENTFLAG.ToString = "" Then Exit For
        '                If !TRANTYPE.ToString = "SR" Or !TRANTYPE.ToString = "AR" Then Continue For
        '                Dim mxitemid As Integer = Val(!ITEMID.ToString)
        '                Dim mxsubitemid As Integer
        '                Dim dtschrow As DataRow
        '                If slab = "Y" Then
        '                    mxsubitemid = Val(!SUBITEMID.ToString)
        '                    dtschrow = Getschdetail(Val(!ITEMID.ToString), Val(!SUBITEMID.ToString))

        '                    If dtschrow Is Nothing Then
        '                        dtschrow = Getschdetail(Val(!ITEMID.ToString), 0)
        '                        mxsubitemid = 0
        '                    End If
        '                    If Not dtschrow Is Nothing Then
        '                        VASLAB = dtschrow.Item("VA_SLAB") : VBC = dtschrow.Item("VBC") : FLATPER = dtschrow.Item("FLATPER")
        '                    End If
        '                End If
        '                Dim disc_allow_wt As Decimal
        '                Dim disc_allow_amt As Decimal
        '                Dim disc_allow_amt_Vat As Decimal

        '                Dim mwt As Decimal = IIf(!grsnet = "NET WT", !netwt, !grswt)
        '                flatdisamt = 0
        '                If FLATPER > 0 Then GoTo FLATDISCALL
        '                If mwt < Balschwt Then disc_allow_wt = mwt : Balschwt -= mwt Else disc_allow_wt = Balschwt : Balschwt = 0
        '                Dim vavalue As Decimal = (Val(!wastage.ToString) * !rate) ' + Val(!mc.ToString)
        '                Dim vawt As Decimal = Math.Round(vavalue / !rate, 3)
        '                TotVa += vawt
        '                Dim Grsamt As Decimal = !GROSSAMT

        '                Dim wastper As Decimal = Val(!wastageper.ToString)
        '                If wastper = 0 Then wastper = Math.Round(((vawt / mwt) * 100), 2)
        '                If disc_allow_wt > 0 Then
        '                    If slab = "N" Then
        '                        Discountva = vawt : DiscountVaper = wastper
        '                    Else
        '                        If wastper >= VASLAB Then
        '                            Discountva = disc_allow_wt * (VASLAB / 100) : DiscountVaper = VASLAB
        '                            excessva = disc_allow_wt * ((moreavgvaper - VASLAB) / 100)
        '                        Else
        '                            Dim avgvaper As Decimal = lessavgvaper 'IIf(wastper > moreavgvaper, wastper - moreavgvaper, wastper)
        '                            Discountva = (disc_allow_wt * (avgvaper / 100)) : DiscountVaper = avgvaper
        '                        End If
        '                    End If
        '                End If
        'nnnext:
        '                _Discountva += Discountva
        '            End With
        '        Next
        'flatdisc:
        'FLATDISCALL:
        '        _Discountva = Math.Round(Val(_Discountva) * Val(TotAvgrate), 2)
        '        Return _Discountva
    End Function


    Private Sub GetChitDetails()
        Dim flagActualReceipt As Boolean = False
        Dim flagMonthBonus As Boolean = False
        Dim Ratediffbonus As Boolean = True
        Dim MONTHONMONTHBONUS As Boolean = False
        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "ACTUALRECEIPT", "N") = "Y" Then flagActualReceipt = True
        strSql = " SELECT CTLTEXT FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID = 'MONTHBONUS'"
        Dim monthbonus As String = objGPack.GetSqlValue(strSql).ToUpper
        If monthbonus = "Y" Then flagMonthBonus = True
        If monthbonus = "M" Then MONTHONMONTHBONUS = True
        strSql = " SELECT ctltext FROM " & chitMainDb & "..SOFTCONTROL WHERE CTLID= 'RATEDIFFBONUS'"
        Ratediffbonus = IIf(objGPack.GetSqlValue(strSql).ToString = "N", False, True)

        Dim schvatasdiscount() As String = GetAdmindbSoftValuefromDt(dtsoftkeyss, "ESTSCHVAT_NOOFFER", "N,''").ToString.Split(",")
        Dim SCHEMESPLITITEM As Boolean = IIf(schvatasdiscount(0) = "N", False, True)
        Dim ReceivedAmount As Double = Nothing
        Dim Gift As Double = Nothing
        Dim PrizeAmount As Double = Nothing
        Dim Bonus As Double = Nothing
        Dim Bonusamt As Double = Nothing
        Dim Interest As Double = Nothing
        Dim Deduction As Double = Nothing
        Dim RecPayAmt As Decimal = Nothing
        Dim RecPayWt As Decimal = Nothing
        Dim RateDiffAmt As Decimal = Nothing

        strSql = "SELECT SUM(CASE WHEN TRANTYPE = 'R' THEN AMOUNT ELSE -1*AMOUNT END) FROM " & chitMainDb & "..RECPAY "
        strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
        strSql += " AND ISNULL(FLAG,'') <> 'CO'"
        RecPayAmt = Val(objGPack.GetSqlValue(strSql))
        'lblInterestPer.Text = IIf(RecPayAmt <> 0, "RecPay : " & Format(RecPayAmt, "0.00"), "")

        Dim mSchrow As DataRow
        mSchrow = GetSqlRow("SELECT SCHEMEID,SNO FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & "", cn)
        Dim Mschid As Integer = Val(mSchrow(0).ToString)

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

        ''Otp Prompt when last paid date less then CHIT_CLOSEMAXDAYS
        Dim ChitcloseMaxDays As Integer = 0
        Dim ChitcloseMax As String = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHIT_CLOSEMAXDAYS", 0)
        'If ChitcloseMax.Contains(",") Then
        '    Dim ChitClose() As String = ChitcloseMax.Split(",")
        '    ChitcloseMaxDays = Val(ChitClose(0).ToString)
        '    If Mschid <> Val(ChitClose(1).ToString) Then
        '        ChitcloseMaxDays = 0
        '    End If
        'Else
        '    ChitcloseMaxDays = Val(ChitcloseMax)
        'End If
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
                If MsgBox("Chit Card is locked" & vbCrLf & vbCrLf & " Have an OTP to Proceed ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
                If Not usrpwdok("CHITCARDLOCK", True) Then Exit Sub
            End If
        End If

        ''TOTAL AMOUNT RECEIVED
        Dim psno As String = mSchrow(1).ToString
        strSql = " SELECT ISNULL(VARIABLEINS,''), ISNULL(SCHEMETYPE,''),ISNULL(purity,0),ISNULL(WEIGHTLEDGER,''),ISNULL(INSTALMENT,0),ISNULL(METALTYPE,'G') AS METALID,ISNULL(RATEDIFFER,'N') AS RATEDIFFER,CLOSETYPE FROM " & chitMainDb & "..SCHEME WHERE SCHEMEID = " & Mschid
        Dim mSchdetrow As DataRow
        mSchdetrow = GetSqlRow(strSql, cn)
        Dim schemeType As String = mSchdetrow(1).ToString
        Dim purity As Decimal = Val(mSchdetrow(2).ToString)
        Dim weightLedger As String = mSchdetrow(3).ToString
        Dim Instalment As Integer = Val(mSchdetrow(4).ToString)
        Dim Isvariableins As Boolean = IIf(mSchdetrow(0).ToString = "Y", True, False)
        Dim Mssmetalid As String = mSchdetrow(5).ToString
        Dim MultiCloseType As Boolean = IIf(mSchdetrow(7).ToString = "B", True, False)
        txtRateDiff.Text = mSchdetrow(6).ToString
        Dim InsMonth As Integer = 0
        If Mssmetalid <> ChitMetalid And weightLedger = "Y" Then
            'If MsgBox("Accumulated metal has been differ adjustment metal" & vbCrLf & " Is consider rate for Scheme metal? " & vbCrLf & "Yes ->" & Mssmetalid & " No ->" & ChitMetalid, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then ChitMetalid = Mssmetalid
            MsgBox("Scheme Card metal and  Sales metal having difference" & vbCrLf & " Adjustment consider the Scheme metal Rate & It must be Offer flat discount Only")
            ChitMetalid = Mssmetalid
        End If
        strSql = " SELECT COUNT(*) FROM (SELECT CONVERT(VARCHAR(2),MONTH(RDATE))+CONVERT(VARCHAR(5),YEAR(RDATE)) NOMONTH  FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO =  " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL GROUP BY CONVERT(VARCHAR(2),MONTH(RDATE))+CONVERT(VARCHAR(5),YEAR(RDATE))) XX"
        InsMonth = Val(objGPack.GetSqlValue(strSql))

        If MultiCloseType Then
            If MsgBox("Do you want Adjust chit as Amount based?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                CloseType = "A"
            End If
        End If



        Dim wt As Double = 0
        recdamount = 0
        Dim rate As Double = 0
        Dim MONTHASINSNO As String = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITMONTHASINSNO", "N")
        Dim MCHITWCTYPE As String = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITWCTYPE", "W")
        strSql = " SELECT SUM(WEIGHT) AS WEIGHT,SUM(AMOUNT) RECDAMT FROM " & chitTrandb & "..SCHEMETRAN WHERE "
        strSql += " GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
        Dim drac As DataRow = GetSqlRow(strSql, cn, tran)
        wt = Val(drac.Item(0).ToString)
        strSql = " SELECT SUM(AMOUNT) RECDAMT FROM " & chitTrandb & "..SCHEMECOLLECT WHERE "
        strSql += " GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
        Dim drCollect As DataRow = GetSqlRow(strSql, cn, tran)
        recdamount = Val(drCollect.Item(0).ToString)
        If MONTHASINSNO = "Y" Then Instalment = InsMonth
        Dim ratemode As String = ""
        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITAVGRATE", "N") = "Y" Or txtRateDiff.Text = "R" Then
            strSql = " SELECT SUM(WEIGHT) TOTWT,SUM(AMOUNT) TOTAMT  FROM " & chitTrandb & "..SCHEMETRAN WHERE "
            strSql += " GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
            Dim dri As DataRow = GetSqlRow(strSql, cn, tran)
            rate = Math.Round(Val(dri.Item(1).ToString) / Val(dri.Item(0).ToString), 2)
            ratemode = "(AVG.)"
        Else
            If GetAdmindbSoftValuefromDt(dtsoftkeyss, "METALRATE", "R") = "R" Then
                rate = Val(GetRate_Purity(GetEntryDate(GetServerDate), objGPack.GetSqlValue("SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN 91.6 AND 92 AND METALID = '" & ChitMetalid & "' ORDER BY PURITY DESC")).ToString)
            Else
                strSql = " SELECT TOP 1 RATE FROM " & chitMainDb & "..RATEMAST WHERE RATEDATE <= '" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "' and METALID = '" & ChitMetalid & "'"
                If purity = 91.6 Then
                    strSql += " AND ISNULL(PURITY,91.6) BETWEEN 91 AND 92"
                Else
                    strSql += " AND PURITY=" & Val(purity)
                End If
                strSql += "  ORDER BY RATEID DESC"
                rate = Val(objGPack.GetSqlValue(strSql))
            End If
        End If
        If IsOrRate = "C" And GetAdmindbSoftValuefromDt(dtsoftkeyss, "ORDRATESCHEME", "N") = "Y" Then
            rate = Bookrate
        End If
        wt = Math.Round(wt, 3)
        strSql = " SELECT isnull(PARTWEIGHTADJ,'N') PARTWEIGHTADJ  FROM " & chitMainDb & "..SCHEME WHERE SCHEMEID = " & Mschid
        mPartweightadj = IIf(objGPack.GetSqlValue(strSql).ToString = "Y", True, False)
        Dim _adjwt As Double = 0
        If mPartweightadj = True And weightLedger = "Y" Then
            strSql = " SELECT SUM(WEIGHT) AS WEIGHT FROM " & chitTrandb & "..SCHEMEADJTRAN WHERE "
            strSql += " GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' "
            _adjwt = Val("" & objGPack.GetSqlValue(strSql).ToString)
        End If

        If mPartweightadj And Val(wt) > 0 Then
            If _adjwt > 0 Then
                wt = wt - _adjwt
                wt = Math.Round(wt, 3)
            End If
            _ChitTotWt = wt
        End If

        Dim mratediffbonus As Double = 0
        Dim mratediffded As Double = 0
        If Not SCHEMESPLITITEM And MCHITWCTYPE <> "W" Then MCHITWCTYPE = "O"

        If Schtype <> "AR" And MCHITWCTYPE <> "A" Then
            If txtRateDiff.Text = "R" Then
                ReceivedAmount = recdamount
            Else
                ReceivedAmount = Math.Round(wt * rate, 2)
            End If
            If wt <> 0 And recdamount > ReceivedAmount Then
                mratediffded = Math.Round(recdamount - ReceivedAmount, 0)
            End If
            If CloseType = "A" Then
                ReceivedAmount = recdamount
            End If
        Else
            mratediffbonus = Math.Round(wt * rate - recdamount, 2)
        End If

        If ReceivedAmount <> 0 Then RateDiffAmt = ReceivedAmount - recdamount
        If Schtype = "V" Then RateDiffAmt = 0
        If RateDiffAmt >= 0 And ReceivedAmount <> 0 Then
            If txtRateDiff.Text = "R" Then
                mratediffbonus = RateDiffAmt
            End If
        Else
            If txtRateDiff.Text = "R" Then
                mratediffded = Math.Abs(RateDiffAmt)
            End If
        End If
        If ReceivedAmount = 0 Then ReceivedAmount = recdamount
        If ReceivedAmount - Math.Floor(ReceivedAmount) > 0.5 Then
            ReceivedAmount = Math.Ceiling(ReceivedAmount)
        Else
            ReceivedAmount = Math.Floor(ReceivedAmount)
        End If

        ' ReceivedAmount += RecPayAmt
        If RecPayAmt < 0 Then Deduction += Math.Abs(RecPayAmt)
        ''PrizeAmount
        PrizeAmount = CalcPrizeAmount()

        ''Bonus
        If schemeType = "W" Then
            strSql = " SELECT TOP 1 CASE WHEN ISNULL(INSWEIGHT,0)=0 THEN WEIGHT ELSE INSWEIGHT END WEIGHT FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " ORDER BY RDATE"
        Else
            strSql = " SELECT TOP 1 CASE WHEN ISNULL(INSAMOUNT,0)=0 THEN AMOUNT ELSE INSAMOUNT END AMOUNT FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " ORDER BY RDATE"
        End If
        Dim insAmt As Double = Val(objGPack.GetSqlValue(strSql))
        Dim noOfIns As Integer
        If Not Isvariableins Then
            strSql = " SELECT TOP 1 NOOFINS FROM " & chitMainDb & "..INSAMOUNT "
            'strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
            strSql += " WHERE SCHEMEID = " & Mschid
            strSql += " AND AMOUNT = " & insAmt & ""
            noOfIns = Val(objGPack.GetSqlValue(strSql))
        Else
            noOfIns = Instalment
        End If
        If MONTHONMONTHBONUS Then
            Bonusamt = CalcBonusSlab()
        Else
            If flagMonthBonus = True Then
                Bonusamt = CalcMonthBonus()
            Else
                If ReceivedAmount / insAmt <= noOfIns Then
                    Bonusamt = CalcBonus()
                End If
            End If
        End If

        strSql = " SELECT TOP 1 GIFTVALUE FROM " & chitMainDb & "..INSAMOUNT "
        strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
        strSql += " AND AMOUNT = " & insAmt & ""
        Dim Giftpayvalue As Decimal = Val(objGPack.GetSqlValue(strSql).ToString)
        strSql = " SELECT Giftminins FROM " & chitMainDb & "..SCHEME WHERE SCHEMEID = " & Mschid
        Dim Giftrefundins As Decimal = Val(objGPack.GetSqlValue(strSql).ToString)
        If Instalment < Giftrefundins Then Deduction += Giftpayvalue

        strSql = " SELECT GROUPCODE FROM " & chitMainDb & "..GIFTTRAN"
        strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
        strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
        strSql += " AND ISNULL(CANCEL,'') <> 'Y'"
        If objGPack.GetSqlValue(strSql).Length > 0 Then
            If ReceivedAmount / insAmt <> noOfIns And Giftrefundins = 0 Then
                Deduction += CalcGift()
            Else
                Gift = 0
            End If
        Else
            If ReceivedAmount / insAmt <> noOfIns Then
                Gift = 0
            Else
                Gift = CalcGift()
            End If
        End If


        ''Interest
        Interest = CalcInterest()
        'If Interest <> 0 Then lblInterestPer.Text += " Interest : " & Interest & " %"
        Bonusamt += mratediffbonus
        Dim ClosedBy As String = ""
        strSql = " SELECT BONUS,PRIZEVALUE,GIFTVALUE,DEDUCTGIFTVALUE+DEDUCTION DEDUCTION,CLOSEDBY "
        strSql += " FROM " & chitMainDb & "..ESTIMATECLOSE "
        strSql += " WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' "
        strSql += " AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
        strSql += " AND ISNULL(CANCEL,'')<>'Y' "
        strSql += " AND SLIPDATE='" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "'"
        strSql += " AND SLIPNO = " & Val(txtCHITSlipNo.Text) & ""
        Dim DTEST As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DTEST)
        If DTEST.Rows.Count > 0 Then
            Bonusamt = Val(DTEST.Rows(0).Item("BONUS"))
            PrizeAmount = Val(DTEST.Rows(0).Item("PRIZEVALUE"))
            Gift = Val(DTEST.Rows(0).Item("GIFTVALUE"))
            ClosedBy = DTEST.Rows(0).Item("CLOSEDBY").ToString
            Deduction = Val(DTEST.Rows(0).Item("DEDUCTION")) - mratediffded
        End If

        'ReceivedAmount -= mratediffbonus

        Dim othadddedamt As Decimal = PrizeAmount + Gift - Deduction
        'Deduction = 0
        ReceivedAmount += othadddedamt
        txtCHITCardAmount_AMT.Text = IIf(ReceivedAmount <> 0, Format(ReceivedAmount, "0.00"), Nothing)
        txtDed.Text = Deduction
        txtPrize.Text = PrizeAmount
        txtGift.Text = Gift
        If wt = 0 Then wt = Math.Round((ReceivedAmount / rate), 3) Else wt += Math.Round(othadddedamt / rate, 3)
        wt += RecPayWt
        If ClosedBy = "W" Then wt = Val(drac.Item(0).ToString)
        'FOR JMT AKSHAYA SCHEME
        If MultiCloseType And CloseType = "A" Then
            wt = 0
            wt = Math.Round(ReceivedAmount / rate, 3)
            If Bonusamt = 0 Then Bonusamt = CalcBonus()
            'ReceivedAmount += mratediffded
            ReceivedAmount += Bonusamt
            txtCHITCardAmount_AMT.Text = Format(ReceivedAmount, "#0.00")
            Bonusamt = 0
            txtCHITCardRate_AMT.ReadOnly = True : txtCHITCardAmount_AMT.ReadOnly = True : Label4.Focus()
        End If
        If wt <> 0 Then txtCHITCardWT_WET.Text = wt
        txtBon_AMT.Text = Bonusamt
        txtBonustype.Text = IIf(MONTHONMONTHBONUS, "M", "")
        If rate <> 0 Then txtCHITCardRate_AMT.Text = rate
        txtInsPaid.Text = Instalment
        SetAddress(psno)
        If wt <> 0 Then txtCHITCardRate_AMT.ReadOnly = True : txtCHITCardAmount_AMT.ReadOnly = True : Label4.Focus() 'txtCHITCardWT_WET.Focus()
        If mPartweightadj = True And wt > 0 Then
            txtCHITCardWT_WET.ReadOnly = False
            txtCHITCardWT_WET.Focus()
        Else
            txtCHITCardWT_WET.ReadOnly = True
        End If
    End Sub

    Private Function CalcBonusSlab() As Double
        Dim bonus As Double = Nothing
        Dim interest As Double
        Dim Intinsno As Double
        Dim STARTDATE As Date = Nothing
        Dim ENDDATE As Date = Nothing
        Dim mSchid As Integer = objGPack.GetSqlValue("SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text))
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

    Public Function GetSchemeDetails_OldadvAdj(ByVal groupcode As String, ByVal regno As String, Optional ByVal madvschadj As Decimal = 0)
        Dim flagActualReceipt As Boolean = False
        Dim flagMonthBonus As Boolean = False
        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "ACTUALRECEIPT", "N") = "Y" Then flagActualReceipt = True
        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "MONTHBONUS", "N") = "Y" Then flagMonthBonus = True
        chitMainDb = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITDBPREFIX", "") + "SAVINGS"
        chitTrandb = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITDBPREFIX", "") + "SH0708"

        Dim ReceivedAmount As Double = Nothing
        Dim Gift As Double = Nothing
        Dim PrizeAmount As Double = Nothing
        Dim Bonus As Double = Nothing
        Dim Bonusamt As Double = Nothing
        Dim Interest As Double = Nothing
        Dim Deduction As Double = Nothing
        Dim RecPayAmt As Decimal = Nothing
        Dim RecPayWt As Decimal = Nothing
        strSql = "SELECT SUM(CASE WHEN TRANTYPE = 'R' THEN AMOUNT ELSE -1*AMOUNT END) FROM " & chitMainDb & "..RECPAY "
        strSql += " WHERE GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & ""
        strSql += " AND ISNULL(FLAG,'') <> 'CO'"
        RecPayAmt = Val(objGPack.GetSqlValue(strSql))




        ''TOTAL AMOUNT RECEIVED
        Dim mSchrow As DataRow
        mSchrow = GetSqlRow("SELECT SCHEMEID,SNO FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & "", cn)
        Dim Mschid As Integer = Val(mSchrow(0).ToString)
        Dim psno As String = mSchrow(1).ToString

        strSql = " SELECT ISNULL(VARIABLEINS,''), ISNULL(SCHEMETYPE,''),ISNULL(purity,0),ISNULL(WEIGHTLEDGER,''),ISNULL(INSTALMENT,0) FROM " & chitMainDb & "..SCHEME WHERE SCHEMEID = " & Mschid
        Dim mSchdetrow As DataRow
        mSchdetrow = GetSqlRow(strSql, cn)
        Dim schemeType As String = mSchdetrow(1).ToString
        Dim purity As Decimal = Val(mSchdetrow(2).ToString)
        Dim weightLedger As String = mSchdetrow(3).ToString
        Dim Instalment As Integer = Val(mSchdetrow(4).ToString)
        Dim Isvariableins As Boolean = IIf(mSchdetrow(0).ToString = "Y", True, False)
        Dim InsMonth As Integer = 0
        strSql = " SELECT COUNT(*) FROM (SELECT MONTH(RDATE) NOMONTH FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE = '" & groupcode & "' AND REGNO =  " & Val(regno) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL GROUP BY MONTH(RDATE)) XX"
        InsMonth = Val(objGPack.GetSqlValue(strSql))

        Dim wt As Double = 0
        recdamount = 0
        Dim rate As Double = 0
        Dim MONTHASINSNO As String = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITMONTHASINSNO", "N")
        Dim MCHITWCTYPE As String = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITWCTYPE", "W")
        strSql = " SELECT SUM(WEIGHT) AS WEIGHT,SUM(AMOUNT) RECDAMT FROM " & chitTrandb & "..SCHEMETRAN WHERE "
        strSql += " GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
        Dim drac As DataRow = GetSqlRow(strSql, cn, tran)
        wt = Val(drac.Item(0).ToString) : recdamount = Val(drac.Item(1).ToString)
        If MONTHASINSNO = "Y" Then Instalment = InsMonth
        Dim ratemode As String = ""
        rate = Bookrate
        If rate = 0 Then
            If GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITAVGRATE", "N") = "Y" Then
                strSql = " SELECT SUM(WEIGHT) TOTWT,SUM(AMOUNT) TOTAMT  FROM " & chitTrandb & "..SCHEMETRAN WHERE "
                strSql += " GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
                Dim dri As DataRow = GetSqlRow(strSql, cn, tran)
                rate = Val(dri.Item(1).ToString) / Val(dri.Item(0).ToString)
                ratemode = "(AVG.)"
            Else
                If GetAdmindbSoftValuefromDt(dtsoftkeyss, "METALRATE", "R") = "R" Then
                    rate = Val(GetRate_Purity(GetEntryDate(GetServerDate), objGPack.GetSqlValue("SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN 91.6 AND 92 AND METALID = 'G' ORDER BY PURITY DESC")).ToString)
                Else
                    strSql = " SELECT TOP 1 RATE FROM " & chitMainDb & "..RATEMAST WHERE RATEDATE <= '" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "' "
                    If purity = 91.6 Then
                        strSql += " AND ISNULL(PURITY,91.6) BETWEEN 91 AND 92"
                    Else
                        strSql += " AND PURITY=" & Val(purity)
                    End If
                    strSql += "  ORDER BY RATEID DESC"
                    rate = Val(objGPack.GetSqlValue(strSql))
                End If
            End If
        End If
        wt = Math.Round(madvschadj / rate, 3)
        'If Schtype <> "AR" Then ReceivedAmount = Math.Round(wt * rate, 2)
        If ReceivedAmount = 0 Then ReceivedAmount = recdamount
        If ReceivedAmount - Math.Floor(ReceivedAmount) > 0.5 Then
            ReceivedAmount = Math.Ceiling(ReceivedAmount)
        Else
            ReceivedAmount = Math.Floor(ReceivedAmount)
        End If


        Dim txtCHITCardAmount As Decimal = IIf(madvschadj <> 0, Format(madvschadj, "0.00"), Nothing)
        Dim txtCHITCardWT As Decimal

        If wt = 0 Then wt = Math.Round((madvschadj / rate), 3)
        wt += RecPayWt

        Dim txtInsPaid As Decimal = Instalment

        SetAddress(psno)

        strSql = " SELECT COUNT(*) FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & " AND ISNULL(CANCEL,'') <> 'Y' AND ISNULL(CHEQUERETDATE,'') = ''"
        Dim NOINS As Integer = Val(objGPack.GetSqlValue(strSql))
        Dim cardid As Integer = Val(objGPack.GetSqlValue("select cardcode from " & cnAdminDb & "..creditcard where schemecode = " & Mschid))
        If cardid = 0 Then MsgBox("Scheme Card not configured") : Exit Function
        Dim Offerper As Decimal = Val(objGPack.GetSqlValue("SELECT OFFERPER FROM " & cnAdminDb & "..SCHEMEOFFERRANGE WHERE SCHEMEID = " & cardid & " AND " & NOINS & " BETWEEN INSFROM AND INSTO  AND ISNULL(ACTIVE,'') <> 'N'").ToString)
        Dim Offervbc As String = objGPack.GetSqlValue("SELECT OFFERVBC FROM " & cnAdminDb & "..SCHEMEOFFERRANGE WHERE SCHEMEID = " & cardid & " AND " & NOINS & " BETWEEN INSFROM AND INSTO  AND ISNULL(ACTIVE,'') <> 'N'").ToString



        Dim ro As DataRow = dtChitAdj.NewRow
        ro!CARDTYPE = Mschid
        ro!slipno = 0 'Val(txtCHITSlipNo.Text)
        ro!GRPCODE = groupcode
        ro!REGNO = IIf(Val(regno) <> 0, regno, DBNull.Value)
        ro!AMOUNT = madvschadj
        ro!RATE = IIf(rate <> 0, rate, DBNull.Value)
        ro!WEIGHT = IIf(wt <> 0, wt, DBNull.Value)
        ro!NOINS = NOINS
        ro!OFFPERCENT = Format(Offerper, "000.00")
        ro!OFFWEIGHT = IIf(Val(wt) <> 0, Val(wt) * (Offerper / 100), DBNull.Value)
        ro!OFFAMOUNT = IIf(Val(txtCHITCardAmount) <> 0, Val(txtCHITCardAmount) * (Offerper / 100), DBNull.Value)
        Dim bonva() As String = Split(GETFLATVALUE(cardid), ",")
        ro!BONUSAMOUNT = 0
        ro!VAPER = Val("" & bonva(1))
        ro!ACTUALAMOUNT = madvschadj
        ro!DEDAMOUNT = 0
        ro!PRIZEAMOUNT = 0
        ro!GIFTAMOUNT = 0
        ro!VBC = Offervbc
        ro!ADVAMOUNT = madvschadj
        dtChitAdj.Rows.Add(ro)
        dtChitAdj.AcceptChanges()
        recdamount = 0
        mAdvSchAdjAmt = 0
        CalcTotal()
    End Function


    Public Function GetSchemeDetails_Oldadj(ByVal groupcode As String, ByVal regno As String, Optional ByVal madvschadj As Decimal = 0)
        Dim flagActualReceipt As Boolean = False
        Dim flagMonthBonus As Boolean = False
        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "ACTUALRECEIPT", "N") = "Y" Then flagActualReceipt = True
        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "MONTHBONUS", "N") = "Y" Then flagMonthBonus = True
        chitMainDb = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITDBPREFIX", "") + "SAVINGS"
        chitTrandb = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITDBPREFIX", "") + "SH0708"

        Dim ReceivedAmount As Double = Nothing
        Dim Gift As Double = Nothing
        Dim PrizeAmount As Double = Nothing
        Dim Bonus As Double = Nothing
        Dim Bonusamt As Double = Nothing
        Dim Interest As Double = Nothing
        Dim Deduction As Double = Nothing
        Dim RecPayAmt As Decimal = Nothing
        Dim RecPayWt As Decimal = Nothing
        strSql = "SELECT SUM(CASE WHEN TRANTYPE = 'R' THEN AMOUNT ELSE -1*AMOUNT END) FROM " & chitMainDb & "..RECPAY "
        strSql += " WHERE GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & ""
        strSql += " AND ISNULL(FLAG,'') <> 'CO'"
        RecPayAmt = Val(objGPack.GetSqlValue(strSql))


        strSql = "SELECT SUM(CASE WHEN TRANTYPE = 'R' THEN WEIGHT ELSE -1*WEIGHT END) FROM " & chitMainDb & "..RECPAY "
        strSql += " WHERE GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & ""
        strSql += " AND ISNULL(FLAG,'') <> 'CO'"
        'RecPaywt = Val(objGPack.GetSqlValue(strSql))



        ''TOTAL AMOUNT RECEIVED
        Dim mSchrow As DataRow
        mSchrow = GetSqlRow("SELECT SCHEMEID,SNO FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & "", cn)
        Dim Mschid As Integer = Val(mSchrow(0).ToString)
        Dim psno As String = mSchrow(1).ToString

        strSql = " SELECT ISNULL(VARIABLEINS,''), ISNULL(SCHEMETYPE,''),ISNULL(purity,0),ISNULL(WEIGHTLEDGER,''),ISNULL(INSTALMENT,0) FROM " & chitMainDb & "..SCHEME WHERE SCHEMEID = " & Mschid
        Dim mSchdetrow As DataRow
        mSchdetrow = GetSqlRow(strSql, cn)
        Dim schemeType As String = mSchdetrow(1).ToString
        Dim purity As Decimal = Val(mSchdetrow(2).ToString)
        Dim weightLedger As String = mSchdetrow(3).ToString
        Dim Instalment As Integer = Val(mSchdetrow(4).ToString)
        Dim Isvariableins As Boolean = IIf(mSchdetrow(0).ToString = "Y", True, False)
        Dim InsMonth As Integer = 0
        strSql = " SELECT COUNT(*) FROM (SELECT MONTH(RDATE) NOMONTH FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE = '" & groupcode & "' AND REGNO =  " & Val(regno) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL GROUP BY MONTH(RDATE)) XX"
        InsMonth = Val(objGPack.GetSqlValue(strSql))

        Dim wt As Double = 0
        recdamount = 0
        Dim rate As Double = 0
        Dim MONTHASINSNO As String = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITMONTHASINSNO", "N")
        Dim MCHITWCTYPE As String = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITWCTYPE", "W")
        strSql = " SELECT SUM(WEIGHT) AS WEIGHT,SUM(AMOUNT) RECDAMT FROM " & chitTrandb & "..SCHEMETRAN WHERE "
        strSql += " GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
        Dim drac As DataRow = GetSqlRow(strSql, cn, tran)
        wt = Val(drac.Item(0).ToString) : recdamount = Val(drac.Item(1).ToString)
        If MONTHASINSNO = "Y" Then Instalment = InsMonth
        Dim ratemode As String = ""
        If GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITAVGRATE", "N") = "Y" Then
            strSql = " SELECT SUM(WEIGHT) TOTWT,SUM(AMOUNT) TOTAMT  FROM " & chitTrandb & "..SCHEMETRAN WHERE "
            strSql += " GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL"
            Dim dri As DataRow = GetSqlRow(strSql, cn, tran)
            rate = Val(dri.Item(1).ToString) / Val(dri.Item(0).ToString)
            ratemode = "(AVG.)"
        Else
            If GetAdmindbSoftValuefromDt(dtsoftkeyss, "METALRATE", "R") = "R" Then
                rate = Val(GetRate_Purity(GetEntryDate(GetServerDate), objGPack.GetSqlValue("SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN 91.6 AND 92 AND METALID = 'G' ORDER BY PURITY DESC")).ToString)
            Else
                strSql = " SELECT TOP 1 RATE FROM " & chitMainDb & "..RATEMAST WHERE RATEDATE <= '" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "' "
                If purity = 91.6 Then
                    strSql += " AND ISNULL(PURITY,91.6) BETWEEN 91 AND 92"
                Else
                    strSql += " AND PURITY=" & Val(purity)
                End If
                strSql += "  ORDER BY RATEID DESC"
                rate = Val(objGPack.GetSqlValue(strSql))
            End If
        End If
        wt = Math.Round(wt, 3)
        ReceivedAmount = madvschadj
        'If ReceivedAmount = 0 Then ReceivedAmount = recdamount
        'If ReceivedAmount - Math.Floor(ReceivedAmount) > 0.5 Then
        '    ReceivedAmount = Math.Ceiling(ReceivedAmount)
        'Else
        '    ReceivedAmount = Math.Floor(ReceivedAmount)
        'End If

        '' ReceivedAmount += RecPayAmt

        'If RecPayAmt < 0 Then Deduction += Math.Abs(RecPayAmt)
        ' ''PrizeAmount
        'PrizeAmount = CalcPrizeAmount_Oldadj(groupcode, regno)

        ' ''Bonus
        'strSql = " SELECT TOP 1 AMOUNT FROM " & chitTrandb & "..SCHEMECOLLECT WHERE GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & ""
        'Dim insAmt As Double = Val(objGPack.GetSqlValue(strSql))
        'Dim noOfIns As Integer
        'If Not Isvariableins Then
        '    strSql = " SELECT TOP 1 NOOFINS FROM " & chitMainDb & "..INSAMOUNT "
        '    strSql += " WHERE GROUPCODE = '" & groupcode & "' "
        '    strSql += " AND AMOUNT = " & insAmt & ""
        '    noOfIns = Val(objGPack.GetSqlValue(strSql))
        'Else
        '    noOfIns = Instalment
        'End If

        'If flagMonthBonus = True Then
        '    strSql = " SELECT ISNULL(SUM(ISNULL(BONUSAMOUNT,0)),0)"
        '    strSql += " FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno)
        '    strSql += " AND ISNULL(CANCEL,'')='' AND ISNULL(CHEQUERETDATE,'')=''"
        '    Bonusamt = Val(objGPack.GetSqlValue(strSql))
        'Else
        '    If ReceivedAmount / insAmt <= noOfIns Then
        '        Bonusamt = CalcBonus_Oldadj(groupcode, regno)
        '    End If
        'End If

        'strSql = " SELECT GROUPCODE FROM " & chitMainDb & "..GIFTTRAN"
        'strSql += " WHERE GROUPCODE = '" & groupcode & "' "
        'strSql += " AND REGNO = " & Val(regno) & ""
        'strSql += " AND ISNULL(CANCEL,'') <> 'Y'"
        'If objGPack.GetSqlValue(strSql).Length > 0 Then
        '    If ReceivedAmount / insAmt <> noOfIns Then

        '        strSql = " SELECT TOP 1 I.GIFTVALUE"
        '        strSql += " FROM " & chitMainDb & "..INSAMOUNT I "
        '        strSql += " INNER JOIN " & chitMainDb & "..SCHEMEMAST M "
        '        strSql += "   ON M.SCHEMEID = I.SCHEMEID AND I.GROUPCODE = '" & groupcode & "'"
        '        strSql += "   AND I.AMOUNT = (SELECT TOP 1 AMOUNT FROM " & chitTrandb & "..SCHEMECOLLECT WHERE GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & ")"
        '        Deduction += Val(objGPack.GetSqlValue(strSql))
        '    Else
        '        Gift = 0
        '    End If
        'Else
        '    If ReceivedAmount / insAmt <> noOfIns Then
        '        Gift = 0
        '    Else
        '        strSql = " SELECT TOP 1 I.GIFTVALUE"
        '        strSql += " FROM " & chitMainDb & "..INSAMOUNT I "
        '        strSql += " INNER JOIN " & chitMainDb & "..SCHEMEMAST M "
        '        strSql += "   ON M.SCHEMEID = I.SCHEMEID AND I.GROUPCODE = '" & groupcode & "'"
        '        strSql += "   AND I.AMOUNT = (SELECT TOP 1 AMOUNT FROM " & chitTrandb & "..SCHEMECOLLECT WHERE GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & ")"
        '        Gift = Val(objGPack.GetSqlValue(strSql))
        '    End If
        'End If


        ' ''Interest
        'strSql = " SELECT TOP 1 I.INTPER"
        'strSql += " FROM " & chitMainDb & "..INSAMOUNT I "
        'strSql += " INNER JOIN " & chitMainDb & "..SCHEMEMAST M "
        'strSql += "   ON M.SCHEMEID = I.SCHEMEID AND I.GROUPCODE = '" & groupcode & "'"
        'Interest = Val(objGPack.GetSqlValue(strSql).ToString)


        ''If Interest <> 0 Then lblInterestPer.Text += " Interest : " & Interest & " %"

        'strSql = " SELECT BONUS,PRIZEVALUE,GIFTVALUE,DEDUCTGIFTVALUE+DEDUCTION DEDUCTION "
        'strSql += " FROM " & chitMainDb & "..ESTIMATECLOSE "
        'strSql += " WHERE GROUPCODE = '" & groupcode & "' "
        'strSql += " AND REGNO = " & Val(regno) & ""
        'strSql += " AND ISNULL(CANCEL,'')<>'Y' "
        'strSql += " AND SLIPDATE='" & GetEntryDate(GetServerDate).ToString("yyyy-MM-dd") & "'"
        'Dim DTEST As New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(DTEST)
        'If DTEST.Rows.Count > 0 Then
        '    Bonusamt = Val(DTEST.Rows(0).Item("BONUS"))
        '    PrizeAmount = Val(DTEST.Rows(0).Item("PRIZEVALUE"))
        '    Gift = Val(DTEST.Rows(0).Item("GIFTVALUE"))
        '    Deduction = Val(DTEST.Rows(0).Item("DEDUCTION"))
        'End If

        Dim txtCHITCardAmount As Decimal = IIf(ReceivedAmount <> 0, Format(ReceivedAmount, "0.00"), Nothing)
        Dim txtCHITCardWT As Decimal
        ReceivedAmount -= Deduction
        ReceivedAmount += PrizeAmount + Gift
        If wt = 0 Then wt = Math.Round((ReceivedAmount / rate), 3)
        wt += RecPayWt


        Dim txtBon As Decimal = Bonusamt


        Dim txtInsPaid As Decimal = Instalment

        SetAddress(psno)
        'If wt <> 0 Then txtCHITCardRate.ReadOnly = True : txtCHITCardAmount_AMT.ReadOnly = True : Label4.Focus() 'txtCHITCardWT_WET.Focus()

        strSql = " SELECT COUNT(*) FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE = '" & groupcode & "' AND REGNO = " & Val(regno) & " AND ISNULL(CANCEL,'') <> 'Y' AND ISNULL(CHEQUERETDATE,'') = ''"
        Dim NOINS As Integer = Val(objGPack.GetSqlValue(strSql))
        Dim cardid As Integer = Val(objGPack.GetSqlValue("select cardcode from " & cnAdminDb & "..creditcard where schemecode = " & Mschid))
        If cardid = 0 Then MsgBox("Scheme Card not configured") : Exit Function
        Dim Offerper As Decimal = Val(objGPack.GetSqlValue("SELECT OFFERPER FROM " & cnAdminDb & "..SCHEMEOFFERRANGE WHERE SCHEMEID = " & cardid & " AND " & NOINS & " BETWEEN INSFROM AND INSTO  AND ISNULL(ACTIVE,'') <> 'N'").ToString)


        Dim ro As DataRow = dtChitAdj.NewRow
        ro!CARDTYPE = Mschid
        ro!slipno = 0 'Val(txtCHITSlipNo.Text)
        ro!GRPCODE = groupcode
        ro!REGNO = IIf(Val(regno) <> 0, regno, DBNull.Value)
        ro!AMOUNT = IIf(Val(txtCHITCardAmount) <> 0, txtCHITCardAmount, DBNull.Value)
        ro!RATE = IIf(rate <> 0, rate, DBNull.Value)
        ro!WEIGHT = IIf(wt <> 0, wt, DBNull.Value)
        ro!NOINS = NOINS
        ro!OFFPERCENT = Format(Offerper, "000.00")
        ro!OFFWEIGHT = IIf(Val(wt) <> 0, Val(wt) * (Offerper / 100), DBNull.Value)
        ro!OFFAMOUNT = IIf(Val(txtCHITCardAmount) <> 0, Val(txtCHITCardAmount) * (Offerper / 100), DBNull.Value)
        Dim bonva() As String = Split(GETFLATVALUE(cardid), ",")
        ro!BONUSAMOUNT = Val(txtCHITCardAmount) * Val("" & bonva(0))
        ro!VAPER = Val("" & bonva(1))
        ro!ACTUALAMOUNT = ReceivedAmount 'recdamount
        ro!DEDAMOUNT = Deduction
        ro!PRIZEAMOUNT = PrizeAmount
        ro!GIFTAMOUNT = Gift
        ro!ADVAMOUNT = madvschadj
        dtChitAdj.Rows.Add(ro)
        dtChitAdj.AcceptChanges()
        recdamount = 0
        mAdvSchAdjAmt = 0
        CalcTotal()
    End Function

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
            If IsOrAdv Then
                strSql = " SELECT AMOUNT FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANTYPE = 'A' AND RECPAY = 'R' AND ISNULL(CANCEL,'') <> 'Y' "
                strSql += " AND BATCHNO IN (SELECT TOP 1 BATCHNO "
                strSql += " FROM " & cnStockDb & "..ACCTRAN WHERE CHQCARDREF = '" & txtCHITCardGrpCode.Text & "' "
                strSql += " AND  CHQCARDNO = " & Val(txtCHITCardRegNo_NUM.Text) & ""
                strSql += " AND PAYMODE = 'SS' AND ISNULL(CANCEL,'') <> 'Y')"
                Dim dtAdvCheck As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtAdvCheck)
                If dtAdvCheck.Rows.Count > 0 Then mAdvSchAdjAmt = Val(dtAdvCheck.Rows(0).Item(0).ToString)
                MsgBox("Chit card adjusted in Order Booking as Advance !!!")
            Else

                With dtCloseCheck.Rows(0)
                    MsgBox("Already Closed !!!" + vbCrLf _
                    + "Close Date : " + .Item("DOCLOSE").ToString + vbCrLf _
                    + "Close Type : " + .Item("CLOSETYPE").ToString + vbCrLf _
                    + "Bill No    : " + .Item("BILLNO").ToString + vbCrLf _
                    + "Bill Date  : " + .Item("BILLDATE").ToString)
                    Return True
                End With
            End If
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
        If ChkLockDays And Val(txtCHITSlipNo.Text) = 0 Then
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
                strSql = "SELECT 'CHECK' FROM " & cnAdminDb & "..SCHEMENOOFFER WHERE SCHEMEID = '" & ccode & "' AND ITEMID IN (" & itemids & ")  AND ISNULL(ACTIVE,'')='Y' "
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


    Private Sub txtChitCardGrpCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCHITCardGrpCode.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If CheckGroupCode() Then Exit Sub
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            Me.Close()
        End If
    End Sub

    Private Sub txtChitCardRegNo_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCHITCardRegNo_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            mPartweightadj = False
            _ChitTotWt = 0
            If CheckRegNo() Then Exit Sub
            If CheckClose(True) Then Exit Sub
            ''If CheckNoOffer() = True Then MsgBox("No Offer Item available in sales", MsgBoxStyle.Information) : Exit Sub

            Dim Optionid As Integer = 0
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
                            Exit Sub
                        End If
                    End If
                    If Not usrpwdok("CHITMDATE_PRECLOSE", True) Then txtCHITCardRegNo_NUM.Focus() : Exit Sub
                    SendKeys.Send("{TAB}")
                End If
            End If

                If autoPost = True Then GetChitDetails()
                If mPartweightadj = True And Val(txtCHITCardWT_WET.Text) > 0 Then
                    txtCHITCardWT_WET.ReadOnly = False
                Else
                    txtCHITCardWT_WET.ReadOnly = True
                End If
            End If
    End Sub
#Region "User Defined Events"
    Private Sub chitDetail_Gotfocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
        txtCHITCardAmount_AMT.GotFocus, txtCHITCardRate_AMT.GotFocus, txtCHITCardWT_WET.GotFocus
        If Not autoPost Then Exit Sub
        'If Not (CType(sender, TextBox).Name <> txtCHITCardAmount_AMT.Name And chitEdit) Then
        ' SendKeys.Send("{TAB}")
        ' End If
    End Sub
    'Private Sub chitDetail_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _
    '    txtCHITCardAmount_AMT.TextChanged, txtCHITCardRate_AMT.TextChanged, txtCHITCardWT_WET.TextChanged
    '    Dim tot As Double = Nothing
    '    tot = Val(txtCHITCardAmount_AMT.Text) + Val(txtCHITCardGift_AMT.Text) - Val(txtCHITCardDeduction_AMT.Text) _
    '    + Val(txtCHITCardPrize_AMT.Text) + Val(txtCHITCardBonus_AMT.Text)
    '    txtCHITCardTotal_AMT.Text = IIf(tot <> 0, Format(tot, "0.00"), Nothing)
    'End Sub
    Private Sub chitDetail_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles _
        txtCHITCardGrpCode.KeyDown, txtCHITCardRegNo_NUM.KeyDown,
        txtCHITCardAmount_AMT.KeyDown, txtCHITCardRate_AMT.KeyDown, txtCHITCardWT_WET.KeyDown
        If e.KeyCode = Keys.Down And gridCHITCard.RowCount > 0 Then
            gridCHITCard.Focus()
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
#End Region

    Private Sub txtChitCardRegNo_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCHITCardRegNo_NUM.TextChanged
        txtCHITCardAmount_AMT.Clear()
        txtCHITCardWT_WET.Clear()
        txtCHITCardRate_AMT.Clear()
        '        lblInterestPer.Text = ""
    End Sub

    Private Sub txtChitCardGrpCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCHITCardGrpCode.TextChanged
        txtCHITCardAmount_AMT.Clear()
        txtCHITCardWT_WET.Clear()
        'txtCHITCardBonus_AMT.Clear()
        'txtCHITCardDeduction_AMT.Clear()
        txtCHITCardRate_AMT.Clear()
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

                Dim drow As Integer = gridCHITCard.CurrentRow.Index
                txtCHITCardGrpCode.Text = .Cells("GRPCODE").Value.ToString
                txtCHITCardRegNo_NUM.Text = .Cells("REGNO").Value.ToString
                txtCHITCardAmount_AMT.Text = .Cells("AMOUNT").Value.ToString
                txtCHITCardRate_AMT.Text = .Cells("RATE").Value.ToString
                txtCHITCardWT_WET.Text = .Cells("WEIGHT").Value.ToString
                '      txtCHITCardBonus_AMT.Text = .Cells("BONUS").Value.ToString
                '     txtCHITCardDeduction_AMT.Text = .Cells("DEDUCTION").Value.ToString
                txtCHITCardRowIndex.Text = gridCHITCard.CurrentRow.Index
                '    cmbCHITtCardType_MAN.Focus()
                txtCHITCardGrpCode.Focus()
                dtChitAdj.Rows(drow).Delete()
                dtChitAdj.AcceptChanges()
            End With
        End If
    End Sub

    Private Sub gridChitCard_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridCHITCard.UserDeletedRow
        dtChitAdj.AcceptChanges()
        CalcTotal()
        If Not gridCHITCard.RowCount > 0 Then txtCHITCardGrpCode.Focus() ' cmbCHITtCardType_MAN.Focus()
    End Sub

    Public Function InsertChitCardDetail(
    ByVal batchno As String, ByVal tNo As Integer, ByVal billdate As Date, ByVal billCashCounterId As String,
    ByVal billCostId As String, ByVal VatExm As String, ByVal tran As OleDbTransaction, ByVal frmFlag As String) As Boolean
        For Each ro As DataRow In dtChitAdj.Rows
            Dim aPost As Boolean = True
            If objGPack.GetSqlValue("SELECT ISNULL(AUTOPOST,'') FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) = "N" Then aPost = False Else aPost = True
            If aPost Then
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
            End If
            Dim accode As String = Nothing
            accode = objGPack.GetSqlValue(" SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)
            InsertIntoAccTran(frmFlag, tran, tNo, billdate, "D", accode,
            Val(ro!AMOUNT.ToString), 0, 0, 0, ChitPaymode, "" _
            , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, , , billCashCounterId, billCostId, batchno, VatExm)
            ''GIFT ACC
            accode = objGPack.GetSqlValue(" SELECT GIFTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)
            InsertIntoAccTran(frmFlag, tran, tNo, billdate, "D", accode,
            Val(ro!GIFT.ToString), 0, 0, 0, IIf(ChitPaymode = "SS", "CG", "HG"), "" _
            , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, , , billCashCounterId, billCostId, batchno, VatExm)
            ''PRIZE ACC
            accode = objGPack.GetSqlValue(" SELECT PRIZEAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)
            InsertIntoAccTran(frmFlag, tran, tNo, billdate, "D", accode,
            Val(ro!PRIZE.ToString), 0, 0, 0, IIf(ChitPaymode = "SS", "CZ", "HZ"), "" _
            , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, , , billCashCounterId, billCostId, batchno, VatExm)
            ''BONUS ACC
            accode = objGPack.GetSqlValue(" SELECT BONUSAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)
            InsertIntoAccTran(frmFlag, tran, tNo, billdate, "D", accode,
            Val(ro!BONUS.ToString), 0, 0, 0, IIf(ChitPaymode = "SS", "CB", "HB"), "" _
            , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, , , billCashCounterId, billCostId, batchno, VatExm)
            ''DEDUCT AC
            accode = objGPack.GetSqlValue(" SELECT DEDUCTAC FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)
            InsertIntoAccTran(frmFlag, tran, tNo, billdate, "C", accode,
            Val(ro!DEDUCTION.ToString), 0, 0, 0, IIf(ChitPaymode = "SS", "CD", "HD"), "" _
            , , , ro!REGNO.ToString, , , ro!GRPCODE.ToString, , , billCashCounterId, billCostId, batchno, VatExm)

            If aPost Then
                strSql = " UPDATE " & chitMainDb & "..SCHEMEMAST SET"
                strSql += " DOCLOSE = '" & billdate.ToString("yyyy-MM-dd") & "'"
                strSql += " ,CLOSETYPE= 'B'"
                strSql += " ,BILLNO= " & tNo & ""
                strSql += " ,BILLDATE= '" & billdate.ToString("yyyy-MM-dd") & "'"
                strSql += " ,BONUS= " & Val(ro!BONUS.ToString) & ""
                strSql += " ,INTEREST= 0"
                strSql += " ,PRIZEVALUE= " & Val(ro!PRIZE.ToString) & ""
                strSql += " ,GIFTVALUE= " & Val(ro!GIFT.ToString) & ""
                strSql += " ,DEDUCTGIFTVALUE= 0"
                strSql += " ,DEDUCTION= " & Val(ro!DEDUCTION.ToString) & ""
                strSql += " ,CLOSEDBY= 'M'"
                strSql += " ,CLOSIUSER= 0"
                strSql += " ,CLOSEDATE= '" & billdate.ToString("yyyy-MM-dd") & "'"
                strSql += " WHERE GROUPCODE = '" & ro!GRPCODE.ToString & "' "
                strSql += " AND REGNO = " & Val(ro!REGNO.ToString) & ""
                ' ExecQuery(SyncMode.Transaction, strSql, cn, tran, billCostId)
                ExecQuery(SyncMode.Master, strSql, cn, tran, billCostId)
            End If
        Next
    End Function

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
   Optional ByVal VATEXM As String = Nothing
   )
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
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        strSql = ""
        cmd = Nothing
    End Sub

    Private Sub cmbChittCardType_MAN_Leave(ByVal sender As Object, ByVal e As System.EventArgs)

        'If objGPack.GetSqlValue("SELECT ISNULL(AUTOPOST,'') FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & cmbCHITtCardType_MAN.Text & "'") = "N" Then autoPost = False Else autoPost = True
        txtCHITCardAmount_AMT.Clear()
        txtCHITCardWT_WET.Clear()
        txtCHITCardRate_AMT.Clear()
        '    lblInterestPer.Text = ""
        'If cmbCHITtCardType_MAN.Text <> "" And cmbCHITtCardType_MAN.Items.Contains(cmbCHITtCardType_MAN.Text) Then
        '    strSql = " SELECT SCHEMECODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & cmbCHITtCardType_MAN.Text & "'"
        '    txtCHITCardSchemeId.Text = objGPack.GetSqlValue(strSql)
        'End If
    End Sub

    Private Sub txtChitCardSchemeId_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        'If txtCHITCardSchemeId.Text <> "" Then
        '    strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE SCHEMECODE = '" & txtCHITCardSchemeId.Text & "'"
        '    cmbCHITtCardType_MAN.Text = objGPack.GetSqlValue(strSql)
        '    SendKeys.Send("{TAB}")
        'End If

    End Sub



    Private Sub txtCHITCardWT_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCHITCardWT_WET.KeyPress

        If e.KeyChar = Chr(Keys.Enter) Then
            ''Validation
            If objGPack.Validator_Check(Me) Then Exit Sub
            If CheckGroupCode() Then Exit Sub
            If CheckRegNo() Then Exit Sub
            If CheckClose() Then Exit Sub
            ''If CheckNoOffer() = True Then MsgBox("No Offer Item available in sales", MsgBoxStyle.Information) : Exit Sub
            If Val(txtCHITCardAmount_AMT.Text) = 0 Then
                Exit Sub
            End If
            Dim mSchid As Integer
            Dim mSchName As String
            Dim mschrow As DataRow
            mschrow = GetSqlRow("SELECT SCHEMEID FROM " & chitMainDb & "..SCHEMEMAST WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & "", cn)
            mSchid = Val(mschrow(0).ToString)



            Dim MONTHASINSNO As String = GetAdmindbSoftValuefromDt(dtsoftkeyss, "CHITMONTHASINSNO", "N")
            Dim NOINS As Integer
            If MONTHASINSNO = "Y" Then

                strSql = " SELECT COUNT(*) FROM (SELECT CONVERT(VARCHAR(2),MONTH(RDATE))+CONVERT(VARCHAR(5),YEAR(RDATE)) NOMONTH  FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO =  " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') = '' AND CHEQUERETDATE IS NULL GROUP BY CONVERT(VARCHAR(2),MONTH(RDATE))+CONVERT(VARCHAR(5),YEAR(RDATE))) XX"
                NOINS = Val(objGPack.GetSqlValue(strSql))
            Else
                strSql = " SELECT COUNT(*) FROM " & chitTrandb & "..SCHEMETRAN WHERE GROUPCODE = '" & txtCHITCardGrpCode.Text & "' AND REGNO = " & Val(txtCHITCardRegNo_NUM.Text) & " AND ISNULL(CANCEL,'') <> 'Y' AND ISNULL(CHEQUERETDATE,'') = ''"
                NOINS = Val(objGPack.GetSqlValue(strSql))
                strSql = " SELECT ISNULL(TOTALINS,0) NOOFINS FROM " & chitTrandb & "..SCHEMETRAN "
                strSql += " WHERE GROUPCODE='" & txtCHITCardGrpCode.Text & "' AND REGNO='" & Val(txtCHITCardRegNo_NUM.Text) & "' "
                strSql += " AND ISNULL(CANCEL,'')='' AND CHEQUERETDATE IS NULL AND ISNULL(TOTALINS,0)<>0"
                Dim TotInst As Integer = 0
                TotInst = Val(objGPack.GetSqlValue(strSql))
                If TotInst > 0 Then TotInst -= 1
                NOINS += TotInst
            End If
            Dim cardid As Integer = Val(objGPack.GetSqlValue("select cardcode from " & cnAdminDb & "..creditcard where schemecode = '" & mSchid & "'"))
            If cardid = 0 Then MsgBox("Scheme Card not configured") : Exit Sub
            Dim Offerper As Decimal = Val(objGPack.GetSqlValue("SELECT OFFERPER FROM " & cnAdminDb & "..SCHEMEOFFERRANGE WHERE SCHEMEID = " & cardid & " AND " & NOINS & " BETWEEN INSFROM AND INSTO  AND ISNULL(ACTIVE,'') <> 'N'").ToString)



            ''Insertion

            'If Val(txtCHITCardRowIndex.Text) = 0 Then
            Dim ro As DataRow = dtChitAdj.NewRow
            ro!CARDTYPE = mSchid
            ro!slipno = Val(txtCHITSlipNo.Text)
            ro!GRPCODE = txtCHITCardGrpCode.Text
            ro!REGNO = IIf(Val(txtCHITCardRegNo_NUM.Text) <> 0, txtCHITCardRegNo_NUM.Text, DBNull.Value)
            ro!AMOUNT = IIf(Val(txtCHITCardAmount_AMT.Text) <> 0, txtCHITCardAmount_AMT.Text, DBNull.Value)
            ro!RATE = IIf(Val(txtCHITCardRate_AMT.Text) <> 0, txtCHITCardRate_AMT.Text, DBNull.Value)
            ro!WEIGHT = IIf(Val(txtCHITCardWT_WET.Text) <> 0, txtCHITCardWT_WET.Text, DBNull.Value)
            ro!NOINS = NOINS
            ro!RATEDIFFER = txtRateDiff.Text.Trim
            ro!BONUSTYPE = txtBonustype.Text.Trim
            ro!OFFPERCENT = Format(Offerper, "000.00")
            ro!OFFWEIGHT = IIf(Val(txtCHITCardWT_WET.Text) <> 0, Val(txtCHITCardWT_WET.Text) * (Offerper / 100), DBNull.Value)
            ro!OFFAMOUNT = IIf(Val(txtCHITCardAmount_AMT.Text) <> 0, Val(txtCHITCardAmount_AMT.Text) * (Offerper / 100), DBNull.Value)
            Dim bonva() As String = Split(GETFLATVALUE(cardid), ",")
            ro!FLATAMOUNT = Math.Round(Val(txtCHITCardAmount_AMT.Text) * Val("" & bonva(0)), 2) '+ Val(txtBon_AMT.Text)
            'ro!FLATAMOUNT = Val(txtBon_AMT.Text)
            ro!BONUSAMOUNT = Val(Val(txtCHITCardAmount_AMT.Text) * Val("" & bonva(0))) + Val(txtBon_AMT.Text)
            ro!VAPER = Val("" & bonva(1))

            ro!ACTUALAMOUNT = recdamount
            ro!DEDAMOUNT = Val(txtDed.Text)
            ro!PRIZEAMOUNT = txtPrize.Text
            ro!GIFTAMOUNT = txtGift.Text
            ro!ADVAMOUNT = mAdvSchAdjAmt
            If mPartweightadj = True Then
                ro!BALWEIGHT = IIf(Val(_ChitTotWt) <> 0, Math.Round(_ChitTotWt, 3), DBNull.Value)
                ro!ADJWEIGHT = IIf(Val(txtCHITCardWT_WET.Text) <> 0, txtCHITCardWT_WET.Text, DBNull.Value)
                ro!ISPARTWT = "Y"
                ro!ISCLOSE = IIf(Val(txtCHITCardWT_WET.Text) = Val(_ChitTotWt), "Y", "N")
            Else
                ro!BALWEIGHT = IIf(Val(txtCHITCardWT_WET.Text) <> 0, txtCHITCardWT_WET.Text, DBNull.Value)
                ro!ADJWEIGHT = IIf(Val(txtCHITCardWT_WET.Text) <> 0, txtCHITCardWT_WET.Text, DBNull.Value)
                ro!ISPARTWT = "N"
                ro!ISCLOSE = "Y"
            End If

            dtChitAdj.Rows.Add(ro)
            dtChitAdj.AcceptChanges()

            'If CloseType = "A" Then
            '    ReceivedAmount += chitBonus()
            'End If

            ''CLEAR
            CalcTotal()
            objGPack.TextClear(Me)
            recdamount = 0
            mAdvSchAdjAmt = 0
            _ChitTotWt = 0
            mPartweightadj = False
            'lblWeightSchemeDetail.Text = ""
            'lblWeightSchemeDetail.Visible = False
            txtCHITCardGrpCode.Focus()
        Else
            If txtCHITCardWT_WET.ReadOnly = False And mPartweightadj = True Then
                Exit Sub
            End If
            e.Handled = True
        End If
    End Sub

    Private Sub txtCHITCardWT_WET_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCHITCardWT_WET.TextChanged
        If mPartweightadj And Val(txtCHITCardWT_WET.Text) > 0 And Val(txtCHITCardRate_AMT.Text) > 0 Then
            txtCHITCardAmount_AMT.Text = Math.Round(txtCHITCardWT_WET.Text * txtCHITCardRate_AMT.Text, 2)
            recdamount = txtCHITCardAmount_AMT.Text
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        dtBookedSchView.Clear()
        'CalcTotal()
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txtCHITSlipNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCHITSlipNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Checkslipno() Then Exit Sub
            If txtCHITCardRegNo_NUM.Text <> "" Then Call txtChitCardRegNo_NUM_KeyPress(sender, e)
        End If
    End Sub

    Private Sub grpCHIT_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpCHIT.Load

    End Sub



    Private Sub Adjgrouper_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Adjgrouper.Load

    End Sub

    Private Sub Label9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label9.Click

    End Sub

End Class