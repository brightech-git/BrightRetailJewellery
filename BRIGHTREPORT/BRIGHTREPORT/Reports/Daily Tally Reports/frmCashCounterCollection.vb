Imports System.Data.OleDb
Public Class frmCashCounterCollection
    Dim strSql As String = ""
    Dim SelectedCompanyId As String = ""
    Dim DT As New DataTable()
    Dim temptable As String
    Dim StrUseridFtr As String
    Dim cmd As New OleDbCommand
    Dim StrFilter As String
    Dim dsReportCol As New DataSet
    Dim RPT_SELFCTR_DABS As Boolean = IIf(GetAdmindbSoftValue("RPT_SELFCTR_DABS", "N") = "Y", True, False)
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub
    Private Sub frmCashCounterCollection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New EventArgs())
        LoadCompany(chkLstCompany)
        If RPT_SELFCTR_DABS = True Then
            pnlUser.Visible = True
            ProcAddUser()
        Else
            pnlUser.Visible = False
        End If
    End Sub
    Private Sub ProcAddUser()
        strSql = "SELECT USERNAME,USERID FROM " & cnAdminDb & "..USERMASTER  ORDER BY USERNAME"
        DT = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            cmbUser.Enabled = True
            cmbUser.Items.Add("ALL")
            For CNT As Integer = 0 To DT.Rows.Count - 1
                cmbUser.Items.Add(DT.Rows(CNT).Item(0).ToString)
                If RPT_SELFCTR_DABS = True And userId <> 999 And userId = DT.Rows(CNT).Item(1).ToString Then
                    cmbUser.Text = DT.Rows(CNT).Item(0).ToString
                    cmbUser.Enabled = False
                End If
            Next
        Else
            cmbUser.Items.Clear()
            cmbUser.Enabled = False
        End If

    End Sub
    Function funcAddCostCentre() As Integer
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = "SELECT DISTINCT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbCostCentre, False, False)
            cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False
        Else
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Enabled = False
        End If
    End Function
    Function funcAddCashCounter() As Integer
        cmbCounterID_OWN.DataSource = Nothing
        'cmbCounterID_OWN.Items.Add("ALL")
        strSql = " SELECT 'ALL',0 RESULT "
        strSql += " UNION ALL "
        strSql += " SELECT CASHNAME,1 RESULT FROM " & cnAdminDb & "..CASHCOUNTER WHERE  1=1  "

        If RPT_SELFCTR_DABS And userId <> 999 Then strSql += " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..USERCASH WHERE USERID='" & userId & "' )"
        strSql += " ORDER BY RESULT"
        'DT = New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(DT)
        objGPack.FillCombo(strSql, cmbCounterID_OWN, True, True)
        'If DT.Rows.Count > 0 Then
        '    cmbCounterID_OWN.DataSource = DT
        '    cmbCounterID_OWN.DisplayMember = "CASHNAME"
        '    cmbCounterID_OWN.ValueMember = "CASHID"
        'Else
        '    cmbCounterID_OWN.Items.Clear()
        '    cmbCounterID_OWN.Enabled = False
        'End If
    End Function
    Private Function GetViewDetails()
        SelectedCompanyId = GetSelectedCompanyId(chkLstCompany, False)
        Dim CASHID As String
        If cmbCounterID_OWN.Text <> "ALL" Then
            CASHID = objGPack.GetSqlValue("SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME='" & cmbCounterID_OWN.Text & "'", , "ALL", )
        Else
            CASHID = "ALL"
        End If
        temptable = "TEMP" & systemId & "CASHCOUNTCOLLECTION"

        strSql = " EXEC " & cnAdminDb & "..SP_CASHCOUNTER_COLLECTION"
        strSql += vbCrLf + "@DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + ",@TEMPTABLE='" & temptable & "'"
        strSql += vbCrLf + " ,@DATE = '" & dtpFrom.Value.ToString("dd-MMM-yyyy") & "'"
        strSql += vbCrLf + " ,@BILLCOUNTID = '" & CASHID & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompanyId & "'"
        strSql += vbCrLf + " ,@COSTID='" & cmbCostCentre.Text & "'"


        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        Dim DtGrid As New DataTable
        DtGrid = dss.Tables(0)
        DtGrid.Columns.Add("KEYNO", GetType(Integer))
        DtGrid.Columns("KEYNO").AutoIncrement = True
        DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        DtGrid.Columns("KEYNO").AutoIncrementStep = 1
        If Not DtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            btnView_Search.Enabled = True
            Exit Function
        End If
        DtGrid.Columns("KEYNO").SetOrdinal(DtGrid.Columns.Count - 1)
        GridView2.DataSource = Nothing
        GridView2.DataSource = DtGrid
        GridView2.Columns("JDEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView2.Columns("JCREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView2.Columns("DEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView2.Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView2.Columns("COLHEAD").Visible = False
        GridView2.Columns("KEYNO").Visible = False
        GridView2.Columns("PAYMODE").Visible = False
        GridView2.Columns("DEBIT").HeaderText = "RECEIPT"
        GridView2.Columns("CREDIT").HeaderText = "PAYMENT"

        FillGridGroupStyle_KeyNoWise(GridView2)
        GridView2.Focus()
    End Function

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub
#Region "Get&Set Properties"
    Private Sub Prop_Gets()
        Dim obj As New frmcashcountercollection_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmcashcountercollection_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmcashcountercollection_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        SetSettingsObj(obj, Me.Name, GetType(frmcashcountercollection_Properties))
    End Sub

    Public Class frmcashcountercollection_Properties

        Private chkCompanySelectAll As Boolean = False
        Public Property p_chkCompanySelectAll() As Boolean
            Get
                Return chkCompanySelectAll
            End Get
            Set(ByVal value As Boolean)
                chkCompanySelectAll = value
            End Set
        End Property
        Private chkLstCompany As New List(Of String)
        Public Property p_chkLstCompany() As List(Of String)
            Get
                Return chkLstCompany
            End Get
            Set(ByVal value As List(Of String))
                chkLstCompany = value
            End Set
        End Property
    End Class
#End Region
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        dtpFrom.Value = GetServerDate()
        funcAddCostCentre()
        funcAddCashCounter()
        GridView2.DataSource = Nothing
        Prop_Gets()
        cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
        btnView_Search.Enabled = True
        chkCompanySelectAll.Focus()
    End Sub
    
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        'GetViewDetails()
        'Exit Sub
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        SelectedCompanyId = GetSelectedCompanyId(chkLstCompany, True)
        StrUseridFtr = ""
        If cmbCounterID_OWN.Text <> "ALL" Then
            Dim Cashid As String = GetSqlValue(cn, "SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "')")
            StrFilter += " AND CASHID IN ('" & Cashid & "')"
        End If
        ''Userid
        If cmbUser.Text <> "ALL" And cmbUser.Text <> "" And cmbUser.Visible = True Then
            Dim USERID As String = GetSqlValue(cn, "SELECT USERID FROM " & cnAdminDb & "..USERMASTER I WHERE USERNAME ='" & cmbUser.Text & "'")
            StrUseridFtr += " AND USERID IN ('" & USERID & "')"
        End If
        'Costid
        If cmbCostCentre.Text <> "ALL" Then
            Dim Costid As String = GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN('" & cmbCostCentre.Text & "')")
            StrUseridFtr += " AND COSTID IN ('" & Costid & "')"
        End If
        Report(False)
        '  btnDotMatrix.Visible = False
        Prop_Sets()

    End Sub

    Private Sub Report(ByVal DotMatrix As Boolean)
        Try
            ProcSASRPU(DotMatrix)
        Catch e As Exception
            MsgBox(e.Message, MsgBoxStyle.Information)
            MsgBox(e.Message + vbCrLf + e.StackTrace)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In GridView2.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("PARTICULARS").Style.BackColor = Color.LightBlue
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "G"
                        .Cells("PAYMENT").Style.BackColor = Color.Red
                        .Cells("RECEIPT").Style.BackColor = Color.Red
                        .Cells("PAYMENT").Style.ForeColor = Color.White
                        .Cells("RECEIPT").Style.ForeColor = Color.White
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "S"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "T1"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "T3"
                        .Cells("PARTICULARS").Style.BackColor = Color.LightGreen
                        .Cells("PARTICULARS").Style.ForeColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
            End With
        Next
    End Function
    Private Sub ProcSASRPU(ByVal DotMatrix As Boolean)
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND "
        strSql += vbCrLf + "  NAME = 'TEMP" & systemId & "COLLECTIONRPT') DROP TABLE  " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = "  CREATE TABLE " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT (CATCODE VARCHAR(20),PARTICULARS VARCHAR(50),RECEIPT NUMERIC(15,2),PAYMENT NUMERIC(15,2),RESULT VARCHAR(2),RESULT1 VARCHAR(3),COLHEAD VARCHAR(3))"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ProcSales()
        ProcRepairDelivery()
        ProcSalesReturn()
        ProcPurchase()
        ProcCreditSales()
        ProcCreditPurchase()
        ProcCreditAdjustment()
        ProcCreditPurchasePayment()
        ProcAdvanceReceived()
        ProcAdvanceAdjustment()
        ProcRepairAdvance()
        ProcRepairAdvanceAdjusted()
        ProcOrderAdvance()
        ProcOrderAdvanceAdjusted()
        ProcChitPayment()
        ProcMiscReceipt()
        ProcMiscPayment()
        ''EMPTY LINE
        strSql = " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT(PARTICULARS) VALUES('')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ProcDiscount()
        ProcHandling()
        ProcroundOff()
        If ChkWithScheme.Checked Then ProcChitCollection()
        ProcWtSubtot()

        strSql = " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT(PARTICULARS) VALUES('')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT(PARTICULARS) VALUES('')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ProcCollection()
        ProcAmtSubtot()
        strSql = "INSERT INTO  " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += vbCrLf + " (PARTICULARS,COLHEAD) "
        strSql += vbCrLf + "SELECT 'DISCOUNT['+CONVERT(VARCHAR,(SUM(DISCOUNT)))+']','T3' FROM " & cnStockDb & "..ISSUE WHERE 1=1"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "')) AND TRANDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        ProcAdvDueSummary()
        strSql = "SELECT * FROM " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        da = New OleDbDataAdapter(strSql, cn)
        Dim tempdt As New DataTable()
        da.Fill(tempdt)
        If tempdt.Rows.Count > 0 Then
            GridView2.DataSource = Nothing
            GridView2.DataSource = tempdt
        Else
            GridView2.DataSource = Nothing
            MessageBox.Show("Data Not Found")
            Exit Sub
        End If

        GridViewFormat()
        GridView2.Columns("COLHEAD").Visible = False
        GridView2.Columns("CATCODE").Visible = False
        GridView2.Columns("RESULT1").Visible = False
        GridView2.Columns("RESULT").Visible = False
        GridView2.Columns("PARTICULARS").Width = 450
        GridView2.Columns("RESULT").Width = 180
        GridView2.Columns("RESULT").Width = 180
        GridView2.Columns("RECEIPT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView2.Columns("PAYMENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView2.Focus()
    End Sub
    Private Sub ProcSales()
        strSql = " IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND "
        strSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSSALES') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ABSSALES"
        strSql += vbCrLf + " SELECT CATCODE, "
        strSql += vbCrLf + " CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "
        strSql += vbCrLf + " PAYMENT, AVERAGE, RATE, COLHEAD, RESULT, RESULT1 INTO " & cnAdminDb & "..TEMP" & systemId & "ABSSALES FROM ("
        strSql += vbCrLf + " SELECT I.CATCODE, "
        strSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
        strSql += vbCrLf + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        strSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        strSql += vbCrLf + " SUM(AMOUNT) "
        strSql += vbCrLf + " AS RECEIPT, "
        strSql += vbCrLf + " 0 AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        strSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        strSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        strSql += vbCrLf + " ELSE "
        strSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        strSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        strSql += vbCrLf + " FROM "
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT PCS"
        strSql += vbCrLf + " ,GRSWT"
        strSql += vbCrLf + " ,NETWT"
        strSql += vbCrLf + " ,AMOUNT"
        strSql += vbCrLf + " -ISNULL("
        strSql += vbCrLf + "(SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO "
        strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')"
        strSql += vbCrLf + " ),0)"
        strSql += vbCrLf + " AS AMOUNT,RATE,CATCODE,METALID"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        strSql += vbCrLf + " WHERE I.TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        strSql += vbCrLf + " I.TRANTYPE IN('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
        If cmbCounterID_OWN.Text <> "ALL" Then strSql += vbCrLf + " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN ('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        strSql += vbCrLf + " )AS I"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        strSql += vbCrLf + " GROUP BY I.CATCODE,C.METALID "
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += vbCrLf + " WHERE ACCODE = C.STAXID) AS CATNAME, "
        strSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
        strSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        strSql += vbCrLf + " SUM(TAX+ISNULL(TT.TAXAMT,0)) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
        strSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        strSql += vbCrLf + " LEFT JOIN (SELECT SUM(TAXAMOUNT) TAXAMT,SNO,BATCHNO from " & cnStockDb & "..TAXTRAN GROUP BY SNO,BATCHNO) TT "
        strSql += vbCrLf + " ON TT.SNO=I.SNO and TT.BATCHNO=I.BATCHNO "
        strSql += vbCrLf + " WHERE I.TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        strSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = ''  "
        If cmbCounterID_OWN.Text <> "ALL" Then strSql += vbCrLf + " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN ('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        strSql += vbCrLf + " GROUP BY I.CATCODE, C.STAXID HAVING SUM(TAX) <> 0"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += vbCrLf + " WHERE ACCODE = C.SSCID) AS CATNAME, "
        strSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
        strSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        strSql += vbCrLf + " SUM(SC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
        strSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        strSql += vbCrLf + " WHERE I.TRANDATE =  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + "  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        strSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = ''  "
        If cmbCounterID_OWN.Text <> "ALL" Then strSql += vbCrLf + " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN ('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        strSql += vbCrLf + " GROUP BY I.CATCODE, C.SSCID HAVING SUM(SC) <> 0"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += vbCrLf + " WHERE ACCODE = C.SASCID) AS CATNAME, "
        strSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
        strSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        strSql += vbCrLf + " SUM(ADSC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
        strSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        strSql += vbCrLf + " WHERE I.TRANDATE =  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + "  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        strSql += vbCrLf + " I.TRANTYPE IN ('SA','OD') AND ISNULL(I.CANCEL,'') = '' "
        If cmbCounterID_OWN.Text <> "ALL" Then strSql += vbCrLf + " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN ('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        strSql += vbCrLf + " GROUP BY I.CATCODE, C.SASCID HAVING SUM(ADSC) <> 0"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT CATCODE,CATNAME, SUM(ISSPCS) AS ISSPCS, "
        strSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
        strSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
        strSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE, COLHEAD FROM ("
        strSql += vbCrLf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..ISSUE "
        strSql += vbCrLf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE,"
        strSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
        strSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATNAME,  "
        strSql += vbCrLf + " SUM(S.STNPCS) ISSPCS,  "
        strSql += vbCrLf + " SUM(S.STNWT) AS ISSGRSWT,  "
        strSql += vbCrLf + " SUM(S.STNWT) AS ISSNETWT,  "
        strSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.STNAMT) AS RECEIPT,  "
        strSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
        strSql += vbCrLf + " 'D' COLHEAD  FROM " & cnStockDb & "..ISSSTONE S "
        strSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..ISSUE I "
        strSql += vbCrLf + " WHERE I.TRANDATE =  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        If cmbCounterID_OWN.Text <> "ALL" Then strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN ('" & cmbCounterID_OWN.Text & "')) "
        strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') IN ('SA','OD') AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & "))"
        strSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
        strSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
        strSql += vbCrLf + " ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        strSql += vbCrLf + " ) X "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        
        
        strSql = "  INSERT INTO  " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT (CATCODE,PARTICULARS,RECEIPT,RESULT,RESULT1,COLHEAD)"
        strSql += vbCrLf + "  SELECT 'Z','SALES',SUM(RECEIPT),'4','2','T' FROM " & cnAdminDb & "..TEMP" & systemId & "ABSSALES"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

    End Sub
    
    Private Sub ProcRepairDelivery()
        strSql = "IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND "
        strSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSREPAIR') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ABSREPAIR"
        strSql += vbCrLf + " SELECT CATCODE, "
        strSql += vbCrLf + " CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "
        strSql += vbCrLf + " PAYMENT, AVERAGE, RATE, COLHEAD, RESULT, RESULT1 INTO " & cnAdminDb & "..TEMP" & systemId & "ABSREPAIR FROM ("
        strSql += vbCrLf + " SELECT I.CATCODE, "
        strSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
        strSql += vbCrLf + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        strSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        strSql += vbCrLf + " SUM(AMOUNT) "
        strSql += vbCrLf + " AS RECEIPT, "
        strSql += vbCrLf + " 0 AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        strSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        strSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        strSql += vbCrLf + " ELSE "
        strSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        strSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        strSql += vbCrLf + " WHERE I.TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + "  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        strSql += vbCrLf + " I.TRANTYPE IN('RD') AND ISNULL(I.CANCEL,'') = '' "
        strSql += vbCrLf + Replace(StrUseridFtr, "USERID", "I.USERID")
        If cmbCounterID_OWN.Text <> "ALL" Then strSql += vbCrLf + " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN ('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + " GROUP BY I.CATCODE"
        strSql += vbCrLf + " ) X "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "ABSREPAIR)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT(CATCODE,PARTICULARS,RECEIPT, PAYMENT,RESULT,RESULT1, COLHEAD) "
        strSql += vbCrLf + " SELECT 'Z' AS CATCODE, 'SALES REPAIR' PARTICULARS, SUM(RECEIPT),SUM(PAYMENT),4 RESULT,2 RESULT1, 'T' COLHEAD "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..TEMP" & systemId & "ABSREPAIR WHERE RESULT = 1 "
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcSalesReturn()
        strSql = "IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSSALESRETURN') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ABSSALESRETURN"
        strSql += vbCrLf + "  SELECT CATCODE, "
        strSql += vbCrLf + "  CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, "
        strSql += vbCrLf + "  RECGRSWT, RECNETWT, RECEIPT, "
        strSql += vbCrLf + "  PAYMENT, AVERAGE, RATE, COLHEAD, RESULT,  RESULT1  INTO " & cnAdminDb & "..TEMP" & systemId & "ABSSALESRETURN FROM ("
        strSql += vbCrLf + "  SELECT R.CATCODE, "
        strSql += vbCrLf + "  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
        strSql += vbCrLf + "  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
        strSql += vbCrLf + "  SUM(PCS) AS RECPCS, SUM(GRSWT) AS RECGRSWT, SUM(NETWT) AS RECNETWT, "
        strSql += vbCrLf + " 0 AS RECEIPT, "
        strSql += vbCrLf + " SUM(AMOUNT) AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        strSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        strSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        strSql += vbCrLf + " ELSE "
        strSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        strSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        strSql += vbCrLf + " FROM "
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT PCS"
        strSql += vbCrLf + " ,GRSWT"
        strSql += vbCrLf + " ,NETWT"
        strSql += vbCrLf + " ,AMOUNT"
        strSql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
        strSql += vbCrLf + " AS AMOUNT,RATE,CATCODE,METALID"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT R"
        strSql += vbCrLf + "  WHERE R.TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND TRANTYPE = 'SR' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
        If cmbCounterID_OWN.Text <> "ALL" Then strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN ('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + " )AS R"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
        strSql += vbCrLf + " GROUP BY R.CATCODE "
        strSql += vbCrLf + "  ) X "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
 
        strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "ABSSALESRETURN)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT(CATCODE,PARTICULARS,RECEIPT, PAYMENT,RESULT,RESULT1, COLHEAD) "
        strSql += vbCrLf + " SELECT 'Z' AS CATCODE, 'SALES RETURN TOT' PARTICULARS, SUM(RECEIPT),SUM(PAYMENT),4 RESULT,2 RESULT1, 'T' COLHEAD "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..TEMP" & systemId & "ABSSALESRETURN WHERE RESULT = 1 "
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

    End Sub

    Private Sub ProcPurchase()
        strSql = " IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPURCHASE') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ABSPURCHASE SELECT CATCODE,  CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS,  RECGRSWT, RECNETWT, RECEIPT,  PAYMENT, AVERAGE, RATE, COLHEAD, RESULT,  RESULT1  INTO " & cnAdminDb & "..TEMP" & systemId & "ABSPURCHASE FROM ( SELECT R.CATCODE,  (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME,  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT,  SUM(PCS) AS RECPCS, SUM(GRSWT) AS RECGRSWT, SUM(NETWT) AS RECNETWT, "
        strSql += vbCrLf + "  0 AS RECEIPT, "
        strSql += vbCrLf + "  SUM(AMOUNT) AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        strSql += vbCrLf + "  CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        strSql += vbCrLf + "  (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        strSql += vbCrLf + "  ELSE "
        strSql += vbCrLf + "  (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        strSql += vbCrLf + "  CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        strSql += vbCrLf + "  FROM "
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "  SELECT PCS,GRSWT,NETWT"
        strSql += vbCrLf + "  ,AMOUNT"
        strSql += vbCrLf + "  -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
        strSql += vbCrLf + "  AS AMOUNT,RATE,CATCODE,METALID FROM " & cnStockDb & "..RECEIPT R WHERE "
        strSql += vbCrLf + "  R.TRANDATE =  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        strSql += vbCrLf + "  TRANTYPE = 'PU' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + Replace(StrUseridFtr, "USERID", "R.USERID")
        If cmbCounterID_OWN.Text <> "ALL" Then strSql += vbCrLf + " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN ('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + "  )AS R"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
        strSql += vbCrLf + "  GROUP BY R.CATCODE,C.METALID  UNION ALL SELECT R.CATCODE, "
        strSql += vbCrLf + "  (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = C.PTAXID)"
        strSql += vbCrLf + "   AS CATNAME,  0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT,  0 AS RECPCS,"
        strSql += vbCrLf + "    0 AS RECGRSWT, 0 AS RECNETWT,  SUM(TAX) AS RECEIPT,0 PAYMENT,"
        strSql += vbCrLf + "     3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') "
        strSql += vbCrLf + "     COLHEAD  FROM " & cnStockDb & "..RECEIPT R JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE "
        strSql += vbCrLf + "    WHERE R.TRANDATE =  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'  AND  R.TRANTYPE = 'PU' "
        strSql += vbCrLf + "   AND ISNULL(R.CANCEL,'') = ''  AND R.COMPANYID IN (" & SelectedCompanyId & ") GROUP BY R.CATCODE,"
        strSql += vbCrLf + "   C.PTAXID  HAVING SUM(TAX) > 0 UNION ALL SELECT CATCODE,CATNAME, SUM(ISSPCS) AS ISSPCS, "
        strSql += vbCrLf + "   SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT, SUM(RECPCS) AS RECPCS, SUM(RECGRSWT)"
        strSql += vbCrLf + "   AS RECGRSWT, SUM(RECNETWT) AS RECNETWT,  SUM(RECEIPT) RECEIPT, SUM(PAYMENT)"
        strSql += vbCrLf + "   PAYMENT,  RESULT, 1 AS RESULT1, AVERAGE, RATE, COLHEAD FROM "
        strSql += vbCrLf + "  ( SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..RECEIPT  WHERE BATCHNO = "
        strSql += vbCrLf + "   S.BATCHNO AND COMPANYID IN (" & SelectedCompanyId & ") AND CATCODE = S.CATCODE) CATCODE, "
        strSql += vbCrLf + "   (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY   WHERE CATCODE = S.CATCODE) CATNAME,"
        strSql += vbCrLf + "     0 AS ISSPCS,  0 AS ISSGRSWT, 0 AS ISSNETWT,   SUM(S.STNPCS) AS RECPCS,"
        strSql += vbCrLf + "      SUM(S.STNWT)  AS RECGRSWT,  SUM(S.STNWT)  AS RECNETWT,   0 AS RECEIPT,"
        strSql += vbCrLf + "       SUM(S.STNAMT) AS PAYMENT,   2 AS RESULT, '' AS AVERAGE, '' AS RATE,  'D' "
        strSql += vbCrLf + "       COLHEAD  FROM " & cnStockDb & "..RECEIPTSTONE S  WHERE S.BATCHNO IN "
        strSql += vbCrLf + "       ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I  WHERE I.TRANDATE "
        strSql += vbCrLf + "  =  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'   AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "       AND TRANTYPE = 'PU' AND COMPANYID IN (" & SelectedCompanyId & ")) AND  "
        strSql += vbCrLf + "     (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
        strSql += vbCrLf + "    AND COMPANYID IN (" & SelectedCompanyId & ") GROUP BY S.CATCODE,S.BATCHNO "
        strSql += vbCrLf + "    ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT ) X "
        
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "ABSPURCHASE)>0 "
        'strSql += vbCrLf + " BEGIN "
        strSql = vbCrLf + " INSERT INTO  " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT(CATCODE,PARTICULARS,RECEIPT,PAYMENT"
        strSql += vbCrLf + " ,RESULT, RESULT1, COLHEAD)  SELECT 'Z' AS CATCODE, "
        strSql += vbCrLf + " 'PURCHASE'PARTICULARS, SUM(RECEIPT), "
        strSql += vbCrLf + " SUM(PAYMENT),4 RESULT, 2 AS RESULT1, 'T' COLHEAD  FROM " & cnAdminDb & "..TEMP" & systemId & "ABSPURCHASE WHERE RESULT = 1   "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcCreditSales()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDSAL') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ABSCREDSAL "
        strSql += vbCrLf + " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        strSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        strSql += vbCrLf + " ISNULL((select top 1 '(JND)' from " & cnAdminDb & "..ITEMDETAIL where batchno=o.batchno),'')+"
        strSql += vbCrLf + " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        strSql += vbCrLf + " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END) AS PAYMENT,"
        strSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += vbCrLf + " INTO " & cnAdminDb & "..TEMP" & systemId & "ABSCREDSAL FROM " & cnAdminDb & "..OUTSTANDING O"
        strSql += vbCrLf + " WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'  "
        strSql += vbCrLf + " AND ISNULL(O.CANCEL,'') = ''  "
        strSql += vbCrLf + " AND O.COMPANYID IN (" & SelectedCompanyId & ") AND O.RECPAY = 'P' AND O.PAYMODE = 'DU'"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        strSql += vbCrLf + " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCREDSAL)>0 "
        'strsql += vbcrlf + " BEGIN "

        strSql = vbCrLf + "INSERT INTO  " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += vbCrLf + " (PARTICULARS, COLHEAD,PAYMENT) "
        strSql += vbCrLf + " SELECT 'CREDIT BILL(DUE)'"
        strSql += vbCrLf + " ,'T',SUM(PAYMENT)-SUM(RECEIPT)"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCREDSAL"

        ' strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcAdvDueSummary()
        strSql = "IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVDUE')"
        strSql += vbCrLf + " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ADVDUE"
        StrSql += vbCrLf + " SELECT 'OPE' SEP"
        StrSql += vbCrLf + " ,CASE WHEN SUBSTRING(RUNNO,6,1) IN ('O','R') THEN 'O' ELSE TRANTYPE END TRANTYPE"
        'StrSql += vbCrLf + " ,CASE WHEN TRANTYPE IN ('D','J') THEN CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END ELSE CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END END AS AMOUNT"
        StrSql += vbCrLf + " ,CASE WHEN TRANTYPE IN ('D','J') THEN CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END ELSE CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END END AS AMOUNT"
        StrSql += " ,BATCHNO"
        strSql += vbCrLf + " INTO " & cnAdminDb & "..TEMP" & systemId & "ADVDUE"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' AND ISNULL(COMPANYID,'') IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        strSql += " AND TRANTYPE <> 'T'"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT 'REC' SEP,CASE WHEN SUBSTRING(RUNNO,6,1) IN ('O','R') THEN 'O' ELSE TRANTYPE END TRANTYPE,CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END AS AMOUNT,BATCHNO"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND RECPAY = 'R' AND ISNULL(CANCEL,'') = '' AND ISNULL(COMPANYID,'') IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        strSql += " AND TRANTYPE <> 'T'"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT 'PAY' SEP,CASE WHEN SUBSTRING(RUNNO,6,1) IN ('O','R') THEN 'O' ELSE TRANTYPE END TRANTYPE,CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END AS AMOUNT,BATCHNO"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND RECPAY = 'P' AND ISNULL(CANCEL,'') = '' AND ISNULL(COMPANYID,'') IN (" & SelectedCompanyId & ")"
        StrSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = " UPDATE " & cnAdminDb & "..TEMP" & systemId & "ADVDUE SET TRANTYPE = 'J'"
        strSql += " FROM " & cnAdminDb & "..TEMP" & systemId & "ADVDUE AS O INNER JOIN " & cnAdminDb & "..ITEMDETAIL I ON I.BATCHNO = O.BATCHNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = "IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVDUESUMMARY')"
        strSql += vbCrLf + " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ADVDUESUMMARY"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CASE TRANTYPE WHEN 'O' THEN 'ORDER ADVANCE' WHEN 'D' THEN 'DUE' WHEN 'T' THEN 'OTHERS' WHEN 'J' THEN 'TOBE' ELSE 'ADVANCE' END AS TRANTYPE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN AMOUNT ELSE 0 END) AS OPENING"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN AMOUNT ELSE 0 END)AS RECEIPT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'PAY' THEN AMOUNT ELSE 0 END)AS PAYMENT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'PAY' THEN -1*AMOUNT ELSE AMOUNT END)AS CLOSING"
        strSql += vbCrLf + " INTO " & cnAdminDb & "..TEMP" & systemId & "ADVDUESUMMARY"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT SEP,TRANTYPE,AMOUNT FROM " & cnAdminDb & "..TEMP" & systemId & "ADVDUE WHERE TRANTYPE NOT IN ('D','J')"
        strSql += vbCrLf + " )X"
        strSql += vbCrLf + " GROUP BY TRANTYPE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CASE TRANTYPE WHEN 'O' THEN 'ORDER ADVANCE' WHEN 'D' THEN 'DUE' WHEN 'T' THEN 'OTHERS' WHEN 'J' THEN 'TOBE' ELSE 'ADVANCE' END AS TRANTYPE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'OPE' THEN AMOUNT ELSE 0 END) AS OPENING"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN AMOUNT ELSE 0 END)AS RECEIPT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'PAY' THEN AMOUNT ELSE 0 END)AS PAYMENT"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN -1*AMOUNT ELSE AMOUNT END)AS CLOSING"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT SEP,TRANTYPE,AMOUNT FROM " & cnAdminDb & "..TEMP" & systemId & "ADVDUE WHERE TRANTYPE IN ('D','J')"
        strSql += vbCrLf + " )X"
        strSql += vbCrLf + " GROUP BY TRANTYPE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcCreditPurchase()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDPUR') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ABSCREDPUR"
        strSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        strSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        strSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        strSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        strSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += " INTO " & cnAdminDb & "..TEMP" & systemId & "ABSCREDPUR FROM " & cnAdminDb & "..OUTSTANDING O"
        strSql += " WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        strSql += "  AND ISNULL(CANCEL,'') = ''  "
        strSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += " AND O.RECPAY = 'R' AND PAYMODE = 'DU'"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        strSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCREDPUR)>0 "
        strSql += " BEGIN "

        strSql += "INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS,COLHEAD,RECEIPT) "
        strSql += " SELECT 'CREDIT PURCHASE(DUE)','T',SUM(RECEIPT)-SUM(PAYMENT)"
        strSql += " FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCREDPUR"

        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcCreditAdjustment()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDADJ') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ABSCREDADJ "
        strSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        strSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO  WHERE BATCHNO = O.BATCHNO))+ "
        strSql += vbCrLf + " ISNULL((select top 1 '(JD)' from " & cnAdminDb & "..ITEMDETAIL where runno=o.runno),'')+"
        strSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        strSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        strSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += " INTO " & cnAdminDb & "..TEMP" & systemId & "ABSCREDADJ FROM " & cnAdminDb & "..OUTSTANDING AS O "
        strSql += " WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        strSql += " AND ISNULL(CANCEL,'') = '' "
        strSql += "  AND O.RECPAY = 'R' AND COMPANYID IN (" & SelectedCompanyId & ") AND PAYMODE = 'DR'"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        strSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCREDADJ)>0 "
        strSql += " BEGIN "

        strSql += "INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS, COLHEAD,RECEIPT) "
        strSql += " SELECT 'CREDIT ADJUSTMENT','T',SUM(RECEIPT)-SUM(PAYMENT)"
        strSql += " FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCREDADJ "

        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcCreditPurchasePayment()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCREDPURPAY') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ABSCREDPURPAY "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO  WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN AMOUNT ELSE 0 END) AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += " INTO " & cnAdminDb & "..TEMP" & systemId & "ABSCREDPURPAY FROM " & cnAdminDb & "..OUTSTANDING AS O "
        strSql += " WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        strSql += "  AND ISNULL(CANCEL,'') = '' "
        StrSql += "  AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += "  AND PAYMODE = 'DP' AND O.RECPAY = 'P'"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCREDPURPAY)>0 "
        StrSql += " BEGIN "

        strSql += "INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS, COLHEAD,PAYMENT) "
        strSql += " SELECT 'PURCHASE/SALESRETURN PAYMENT','T',SUM(PAYMENT)-SUM(RECEIPT)"
        strSql += " FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCREDPURPAY "

        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcAdvanceReceived()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVREC') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ADVREC "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += " INTO " & cnAdminDb & "..TEMP" & systemId & "ADVREC FROM " & cnAdminDb & "..OUTSTANDING AS O "
        strSql += " WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += "  AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE = 'AR' AND O.RECPAY = 'R' "
        StrSql += " AND O.TRANTYPE = 'A'"
        StrSql += " AND NOT (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        'strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "ADVREC)>0 "
        'StrSql += " BEGIN "

        strSql = "INSERT INTO  " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS, COLHEAD,RECEIPT) "
        strSql += " SELECT 'ADVANCE RECEIVED','T',SUM(RECEIPT)-SUM(PAYMENT)"
        strSql += " FROM " & cnAdminDb & "..TEMP" & systemId & "ADVREC "

        'strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcAdvanceAdjustment()
        strSql = "  IF EXISTS(SELECT 1 FROM  " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ADVADJ') DROP TABLE  " & cnAdminDb & "..TEMP" & systemId & "ADVADJ "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END) AS PAYMENT, "
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += " INTO  " & cnAdminDb & "..TEMP" & systemId & "ADVADJ FROM " & cnAdminDb & "..OUTSTANDING O"
        strSql += " WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'  "
        strSql += "  AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND PAYMODE IN ('AA','AP') AND O.RECPAY = 'P' "
        StrSql += " AND O.TRANTYPE = 'A'"
        StrSql += " AND NOT (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        'StrSql += " AND O.RUNNO LIKE 'A%'"
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        'StrSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ADVADJ)>0 "
        'StrSql += " BEGIN "

        strSql = "INSERT INTO  " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS, COLHEAD,PAYMENT) "
        strSql += " SELECT 'ADVANCE ADJUSTED','T',SUM(PAYMENT)-SUM(RECEIPT)"
        strSql += " FROM  " & cnAdminDb & "..TEMP" & systemId & "ADVADJ "

        ' strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcRepairAdvance()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "REPAIRADV') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "REPAIRADV "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE 0 END)  AS RECEIPT, 0 AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += " INTO " & cnAdminDb & "..TEMP" & systemId & "REPAIRADV FROM " & cnAdminDb & "..OUTSTANDING AS O "
        strSql += " WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        strSql += " AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('OR','AR') AND O.RECPAY = 'R' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " AND SUBSTRING(O.RUNNO,6,1) = 'R'"
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        strSql = "INSERT INTO  " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS, COLHEAD,PAYMENT) "
        strSql += " SELECT 'REPAIR ADVANCE','T',SUM(PAYMENT)-SUM(RECEIPT)"
        strSql += " FROM  " & cnAdminDb & "..TEMP" & systemId & "REPAIRADV "

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcRepairAdvanceAdjusted()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "REPAIRADVADJ') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "REPAIRADVADJ "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END)  AS PAYMENT, 0 AS RECEIPT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += " INTO " & cnAdminDb & "..TEMP" & systemId & "REPAIRADVADJ FROM " & cnAdminDb & "..OUTSTANDING AS O "

        strSql += " WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += "  AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('AA','AP') AND O.RECPAY = 'P' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " AND SUBSTRING(O.RUNNO,6,1) = 'R'"
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "REPAIRADVADJ)>0 "
        StrSql += " BEGIN "

        strSql += "INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS, COLHEAD,PAYMENT) "
        strSql += " SELECT 'REPAIR ADVANCE ADJUSTED','T',SUM(PAYMENT)-SUM(RECEIPT)"
        strSql += " FROM " & cnAdminDb & "..TEMP" & systemId & "REPAIRADVADJ "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcCollection()
        strSql = " IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "COLDET')"
        strSql += vbCrLf + " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "COLDET  DECLARE @CASHID VARCHAR(7) SELECT @CASHID = CTLTEXT"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(50),(CASE "
        strSql += vbCrLf + " WHEN PAYMODE='CA' THEN 'CASH' "
        strSql += vbCrLf + " WHEN PAYMODE = 'CC' THEN 'CREDIT CARD' "
        ''strSql += vbCrLf + " WHEN PAYMODE = 'CH' THEN 'CHEQUE' "
        strSql += vbCrLf + " WHEN PAYMODE = 'CH' THEN FLAG "
        strSql += vbCrLf + " WHEN PAYMODE = 'SS' THEN 'SCHEME' "
        strSql += vbCrLf + " WHEN PAYMODE = 'GV' THEN 'GIFT VOUCHER' "
        strSql += vbCrLf + " WHEN PAYMODE = 'ET' THEN 'ETRANSFER' "
        strSql += vbCrLf + " WHEN PAYMODE = 'OT' THEN 'OTHERS' "
        strSql += vbCrLf + " END)) AS CATNAME,"
        strSql += vbCrLf + " (CASE WHEN SUM(AMOUNT)> 0 THEN  SUM(AMOUNT) ELSE 0 END) AS PAYMENT, "
        strSql += vbCrLf + " (CASE WHEN SUM(AMOUNT)< 0 THEN  ABS(SUM(AMOUNT)) ELSE 0 END) AS RECEIPT "
        strSql += vbCrLf + " INTO " & cnAdminDb & "..TEMP" & systemId & "COLDET FROM ("
        strSql += vbCrLf + " SELECT PAYMODE,'' FLAG,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END)) AS AMOUNT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A"
        strSql += vbCrLf + " WHERE TRANDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = '' AND FROMFLAG NOT IN ('','S','O','A')  "
        If cmbCounterID_OWN.Text <> "ALL" Then strSql += vbCrLf + " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN ('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += vbCrLf + " AND PAYMODE IN ('CC','GV')"
        If ChkWithScheme.Checked = False Then strSql += vbCrLf + " AND FROMFLAG <> 'C'"
        strSql += vbCrLf + " GROUP BY TRANMODE,PAYMODE"

        strSql += vbCrLf + " UNION ALL "

        strSql += vbCrLf + " SELECT PAYMODE"
        strSql += vbCrLf + "  ,(CASE WHEN ISNULL((SELECT MODENAME FROM " & cnAdminDb & "..PAYMENTMODE WHERE CONVERT(VARCHAR,MODEID)=A.FLAG),'') <>''  "
        strSql += vbCrLf + "  THEN (SELECT MODENAME FROM " & cnAdminDb & "..PAYMENTMODE WHERE CONVERT(VARCHAR,MODEID)=A.FLAG)"
        strSql += vbCrLf + "  ELSE 'CHEQUE' END) FLAG"
        strSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END)) AS AMOUNT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A"
        strSql += vbCrLf + " WHERE TRANDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = '' AND FROMFLAG NOT IN ('','S','O','A')  "
        If cmbCounterID_OWN.Text <> "ALL" Then strSql += vbCrLf + " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN ('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += vbCrLf + " AND PAYMODE IN ('CH','ET')"
        If ChkWithScheme.Checked = False Then strSql += vbCrLf + " AND FROMFLAG <> 'C'"
        strSql += vbCrLf + " GROUP BY TRANMODE,PAYMODE,FLAG"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT 'SS' PAYMODE,'' FLAG,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END)) AS AMOUNT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A"
        strSql += vbCrLf + " WHERE TRANDATE='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = '' "
        If cmbCounterID_OWN.Text <> "ALL" Then strSql += vbCrLf + " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER  WHERE CASHNAME IN ('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += vbCrLf + " AND PAYMODE IN ('SS','CB','CZ','CG','CD') AND FROMFLAG NOT IN ('','S','O','A') "
        If ChkWithScheme.Checked = False Then strSql += vbCrLf + " AND FROMFLAG <> 'C'"
        strSql += vbCrLf + " GROUP BY TRANMODE"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT 'CA' PAYMODE,'' FLAG,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END)) AS AMOUNT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A"
        strSql += vbCrLf + " WHERE TRANDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + " AND ISNULL(A.CANCEL,'') = '' "
        If cmbCounterID_OWN.Text <> "ALL" Then strSql += vbCrLf + " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN ('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += vbCrLf + " AND PAYMODE IN ('CA') AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE = '1') "
        strSql += vbCrLf + " AND FROMFLAG NOT IN ('','S','O','A') "
        If ChkWithScheme.Checked = False Then strSql += vbCrLf + " AND FROMFLAG <> 'C'"
        strSql += vbCrLf + " GROUP BY TRANMODE"
        strSql += vbCrLf + " )X "
        strSql += vbCrLf + " GROUP BY PAYMODE,FLAG "
        strSql += vbCrLf + " HAVING(SUM(AMOUNT) <> 0)"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "COLDET)>0 "
        strSql += vbCrLf + " BEGIN "
        ''strSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
        'strSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('') "
        strSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT(PARTICULARS,COLHEAD) "
        strSql += vbCrLf + " VALUES('COLLECTION DETAILS','T') "
        strSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += vbCrLf + " (PARTICULARS,RECEIPT,PAYMENT) "
        strSql += vbCrLf + " SELECT '  ' + CATNAME PARTICULARS, "
        strSql += vbCrLf + " CASE WHEN RECEIPT<> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
        strSql += vbCrLf + " CASE WHEN PAYMENT<> 0 THEN PAYMENT  ELSE NULL END PAYMENT "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..TEMP" & systemId & "COLDET ORDER BY CATNAME "
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcOrderAdvance()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ORDERADV') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ORDERADV "
        strSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        strSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        strSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        strSql += " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE 0 END)  AS RECEIPT, 0 AS PAYMENT,"
        strSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += " INTO " & cnAdminDb & "..TEMP" & systemId & "ORDERADV FROM " & cnAdminDb & "..OUTSTANDING AS O "
        strSql += " WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        strSql += "  AND ISNULL(CANCEL,'') = '' "
        strSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += " AND PAYMODE IN ('OR','AR') AND O.RECPAY = 'R' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        strSql += vbCrLf + StrUseridFtr
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += " AND SUBSTRING(O.RUNNO,6,1) <> 'R'"
        strSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        strSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "ORDERADV)>0 "
        strSql += " BEGIN "
        strSql += "INSERT INTO  " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS, COLHEAD,RECEIPT) "
        strSql += " SELECT 'ORDER ADVANCE RECEIVED','T',SUM(RECEIPT)-SUM(PAYMENT)"
        strSql += " FROM  " & cnAdminDb & "..TEMP" & systemId & "ORDERADV "
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcOrderAdvanceAdjusted()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ORDERADVADJ') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ORDERADVADJ "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        StrSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        'StrSql += " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        StrSql += " '(REFNO: '+SUBSTRING(O.RUNNO,6,20) +')' AS CATNAME,  "
        StrSql += " SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END)  AS PAYMENT, 0 AS RECEIPT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += " INTO " & cnAdminDb & "..TEMP" & systemId & "ORDERADVADJ FROM " & cnAdminDb & "..OUTSTANDING AS O "

        strSql += " WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += " AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('AA','AP') AND O.RECPAY = 'P' AND (SUBSTRING(O.RUNNO,6,20) LIKE 'O%' OR SUBSTRING(O.RUNNO,6,20) LIKE 'R%')"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " AND SUBSTRING(O.RUNNO,6,1) <> 'R'"
        StrSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "ORDERADVADJ)>0 "
        StrSql += " BEGIN "
        strSql += "INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS, COLHEAD,PAYMENT) "
        strSql += " SELECT 'ORDER ADVANCE ADJUSTED','T',SUM(PAYMENT)-SUM(RECEIPT)"
        strSql += " FROM " & cnAdminDb & "..TEMP" & systemId & "ORDERADVADJ "

        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcChitPayment()
        strSql = "  IF EXISTS(SELECT 1 FROM  " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITPAYMENT') DROP TABLE  " & cnAdminDb & "..TEMP" & systemId & "CHITPAYMENT "
        StrSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        StrSql += " '(GRP:' + CHQCARDREF + ' REGNO: ' + CHQCARDNO + ')' AS CATNAME,  "
        StrSql += " 0 AS RECEIPT, SUM(CASE WHEN TRANMODE = 'D' THEN O.AMOUNT ELSE -1*AMOUNT END) AS PAYMENT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += " INTO  " & cnAdminDb & "..TEMP" & systemId & "CHITPAYMENT FROM " & cnStockDb & "..ACCTRAN O"
        strSql += " WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += " AND ISNULL(CANCEL,'') = '' "
        StrSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += " AND PAYMODE IN ('HP','HG','HZ','HB','HD')"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " GROUP BY O.BATCHNO,O.CHQCARDREF,O.CHQCARDNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "CHITPAYMENT)>0 "
        StrSql += " BEGIN "
        strSql += "INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS, COLHEAD) "
        StrSql += " SELECT 'SCHEME PAYMENTS ['+CONVERT(VARCHAR,ISNULL(SUM(PAYMENT-RECEIPT),''))+']','T'"
        strSql += " FROM " & cnAdminDb & "..TEMP" & systemId & "CHITPAYMENT"
        strSql += " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += " SELECT '  ' + CATNAME CATNAME, "
        StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += " 1 RESULT1, COLHEAD  "

        strSql += " FROM " & cnAdminDb & "..TEMP" & systemId & "CHITPAYMENT ORDER BY RESULT1,PAYMENT "

        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcWtSubtot()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "WTSTOT') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "WTSTOT "
        strSql += " SELECT * INTO " & cnAdminDb & "..TEMP" & systemId & "WTSTOT  FROM ("
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "ABSSALES WHERE ISNULL(RESULT1,1) NOT IN (0,2) " 'AND ISNULL(COLHEAD,'') <> 'D'
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "ABSREPAIR WHERE ISNULL(RESULT1,1) NOT IN (0,2) AND ISNULL(COLHEAD,'') <> 'D'"
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "ABSSALESRETURN WHERE ISNULL(RESULT1,1) NOT IN (0,2) " 'AND ISNULL(COLHEAD,'') <> 'D'"
        StrSql += " UNION ALL"
        'strSql += " SELECT 0 RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "ABS_ORDAMTWT"
        'StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "ABSPURCHASE WHERE ISNULL(RESULT1,1) NOT IN (0,2)  " 'AND ISNULL(COLHEAD,'') <> 'D'"
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "ADVADJ"
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "ADVREC"
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCREDSAL"
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCREDPUR"
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCREDADJ"
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCREDPURPAY"
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "CHITPAYMENT"
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "MISCRECEIPTS"
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "MISCPAYMENT"
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "ORDERADV"
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "ORDERADVADJ"
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "REPAIRADV"
        StrSql += " UNION ALL"
        strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "REPAIRADVADJ"
        strSql += " UNION ALL"
        If ChkWithScheme.Checked Then
            strSql += " SELECT RECEIPT, PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "CHITCOLLECTION"
            strSql += " UNION ALL"
        End If
        strSql += " SELECT CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT, "
        strSql += " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "DISCOUNT"
        strSql += " UNION ALL"
        strSql += " SELECT CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT, "
        strSql += " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "HANDLING"
        strSql += " UNION ALL"
        strSql += " SELECT CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT, "
        strSql += " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT  FROM " & cnAdminDb & "..TEMP" & systemId & "ROUNDOFF"

        'strSql += " UNION ALL"
        'strSql += " SELECT CASE WHEN AMOUNT > 0 THEN AMOUNT ELSE 0 END RECEIPT"
        'strSql += " ,CASE WHEN AMOUNT < 0 THEN AMOUNT ELSE 0 END PAYMENT FROM " & cnAdminDb & "..TEMPCASHOPEN"

        strSql += " )X "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "WTSTOT)>0 "
        strSql += " BEGIN "
        strSql += "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "WTSTOTAL') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "WTSTOTAL "
        strSql += " SELECT SUM(RECEIPT) AS RECEIPT, SUM(PAYMENT) AS PAYMENT INTO " & cnAdminDb & "..TEMP" & systemId
        strSql += "WTSTOTAL FROM " & cnAdminDb & "..TEMP" & systemId & "WTSTOT  END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        DT = New DataTable
        strSql = " SELECT * FROM " & cnAdminDb & "..TEMP" & systemId & "WTSTOT"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DT)
        If DT.Rows.Count <= 0 Then Exit Sub

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "WTSTOTAL)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS,RECEIPT,PAYMENT,COLHEAD) "
        strSql += " SELECT 'TOTAL', RECEIPT, PAYMENT,'S' FROM " & cnAdminDb & "..TEMP" & systemId & "WTSTOTAL"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "WTSTOTAL)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS,RECEIPT,PAYMENT,COLHEAD) "
        strSql += " SELECT 'GRAND TOTAL', "
        strSql += " ISNULL((CASE WHEN X>0 THEN X END),NULL) AS RECEIPT,"
        strSql += " ISNULL((CASE WHEN X<0 THEN X END),NULL) AS PAYMENT, 'G'"
        strSql += " FROM ("
        strSql += " SELECT RECEIPT-PAYMENT  AS X FROM " & cnAdminDb & "..TEMP" & systemId & "WTSTOTAL) Y"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcMiscReceipt()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "MISCRECEIPTS') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "MISCRECEIPTS "
        strsql += vbcrlf + " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM "
        strSql += cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        strSql += " CASE WHEN ISNULL(O.REMARK1,'') <> '' THEN '('+O.REMARK1+')' ELSE '' END AS CATNAME,"
        'strsql += vbcrlf + " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        strSql += vbCrLf + " SUM(CASE WHEN O.RECPAY = 'R' THEN O.AMOUNT ELSE 0 END) AS RECEIPT, 0 AS PAYMENT, "
        strSql += vbCrLf + " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += vbCrLf + " INTO " & cnAdminDb & "..TEMP" & systemId & "MISCRECEIPTS FROM " & cnAdminDb & "..OUTSTANDING O"
        strSql += vbCrLf + " WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        strSql += vbCrLf + " ISNULL(CANCEL,'') = '' "
        strSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + " AND PAYMODE IN ('MR') AND O.RECPAY = 'R'"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        strSql += vbCrLf + " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        strSql += vbCrLf + " ,O.REMARK1"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = " IF (SELECT TOP 1 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMPCASHOPEN')>0 DROP TABLE " & cnAdminDb & "..TEMPCASHOPEN"
        strSql += vbCrLf + " DECLARE @CASHID VARCHAR(7)"
        strSql += vbCrLf + " SELECT @CASHID = CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CASH'"
        strSql += vbCrLf + " SELECT SUM(ISNULL(AMOUNT,0)) AS AMOUNT"
        strSql += vbCrLf + " INTO " & cnAdminDb & "..TEMPCASHOPEN"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + "     ("
        strSql += vbCrLf + "     SELECT DEBIT-CREDIT AS AMOUNT FROM " & cnStockDb & "..OPENTRAILBALANCE"
        strSql += vbCrLf + "     WHERE ACCODE = @CASHID"
        strSql += vbCrLf + "     AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + "     UNION ALL"
        strSql += vbCrLf + "     SELECT CASE WHEN A.TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT"
        strSql += vbCrLf + "     FROM " & cnStockDb & "..ACCTRAN A"
        strSql += vbCrLf + "     WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "     AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + "     AND ISNULL(A.CANCEL,'') = ''"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += vbCrLf + "     AND PAYMODE IN ('CA')"
        strSql += vbCrLf + "     AND ACCODE = @CASHID"
        strSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        strSql += vbCrLf + "     )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM  " & cnAdminDb & "..TEMPCASHOPEN)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " INSERT INTO  " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += vbCrLf + " (PARTICULARS,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        strSql += vbCrLf + " SELECT 'CASH OPENING..'CATNAME, "
        strSql += vbCrLf + " CASE WHEN AMOUNT  > 0 THEN AMOUNT  ELSE NULL END RECEIPT, "
        strSql += vbCrLf + " CASE WHEN AMOUNT  < 0 THEN AMOUNT  ELSE NULL END PAYMENT, "
        strSql += vbCrLf + " 1 RESULT1,'T' COLHEAD  "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..TEMPCASHOPEN"
        strSql += vbCrLf + " END "
        ' cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '  cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM  " & cnAdminDb & "..TEMP" & systemId & "MISCRECEIPTS)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + "INSERT INTO  " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += vbCrLf + " (PARTICULARS, COLHEAD,RECEIPT) "
        strSql += vbCrLf + " SELECT 'MISC RECEIPTS','T',SUM(RECEIPT)-SUM(PAYMENT)"
        strSql += vbCrLf + " FROM  " & cnAdminDb & "..TEMP" & systemId & "MISCRECEIPTS"
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcMiscPayment()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "MISCPAYMENT') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "MISCPAYMENT "
        strSql += " SELECT (SELECT (PNAME+' '+INITIAL) CATNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (sELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO))+ "
        strSql += " CASE WHEN ISNULL(O.REMARK1,'') <> '' THEN '('+O.REMARK1+')' ELSE '' END AS CATNAME,"
        'StrSql += " '(REFNO: '+O.RUNNO +' - '+CONVERT(VARCHAR,O.TRANDATE,103)+')' AS CATNAME,  "
        strSql += " 0 AS RECEIPT, SUM(CASE WHEN O.RECPAY = 'P' THEN O.AMOUNT ELSE 0 END) AS PAYMENT,"
        strSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += " INTO " & cnAdminDb & "..TEMP" & systemId & "MISCPAYMENT FROM " & cnAdminDb & "..OUTSTANDING O"
        strSql += " WHERE TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        strSql += " ISNULL(CANCEL,'') = '' "
        strSql += " AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += " AND PAYMODE IN ('MP') AND O.RECPAY = 'P'"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " AND O.FROMFLAG NOT IN ('','S','O','A')"
        strSql += " GROUP BY O.BATCHNO,O.RUNNO,O.TRANDATE"
        strSql += " ,O.REMARK1"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "MISCPAYMENT)>0 "
        strSql += " BEGIN "

        strSql += "INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS, COLHEAD,PAYMENT) "
        strSql += " SELECT 'MISC PAYMENTS','T',SUM(PAYMENT)-SUM(RECEIPT)"
        strSql += " FROM " & cnAdminDb & "..TEMP" & systemId & "MISCPAYMENT"

        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcDiscount()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "DISCOUNT') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "DISCOUNT "
        StrSql += " SELECT 'DISCOUNT' CATNAME,  "
        StrSql += " SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END) AS AMT, "
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += " INTO " & cnAdminDb & "..TEMP" & systemId & "DISCOUNT FROM " & cnStockDb & "..ACCTRAN AS A  "
        strSql += " WHERE A.TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        strSql += " ISNULL(A.CANCEL,'') = '' "
        StrSql += " AND PAYMODE = 'DI'   AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END),0) <> 0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "DISCOUNT)>0 "
        StrSql += " BEGIN "
        'StrSql += " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        strSql += " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += " SELECT '  ' + CATNAME CATNAME,  "
        StrSql += " CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT,  "
        StrSql += " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT, "
        StrSql += " 1 RESULT1, COLHEAD  "
        strSql += " FROM " & cnAdminDb & "..TEMP" & systemId & "DISCOUNT ORDER BY CATNAME  "
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcHandling()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND "
        strSql += " NAME = 'TEMP" & systemId & "HANDLING') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "HANDLING "
        StrSql += " SELECT 'HANDLING CHARGES' CATNAME,  "
        StrSql += " SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END) AS AMT, "
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += " INTO " & cnAdminDb & "..TEMP" & systemId & "HANDLING FROM " & cnStockDb & "..ACCTRAN AS A  "
        strSql += " WHERE A.TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        strSql += " ISNULL(A.CANCEL,'') = '' "
        StrSql += " AND PAYMODE = 'HC'   AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END),0) <> 0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "HANDLING)>0 "
        StrSql += " BEGIN "
        'StrSql += " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        strSql += " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += " SELECT '  ' + CATNAME CATNAME,  "
        StrSql += " CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT,  "
        StrSql += " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT, "
        StrSql += " 1 RESULT1, COLHEAD  "
        strSql += " FROM " & cnAdminDb & "..TEMP" & systemId & "HANDLING ORDER BY CATNAME  "
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcroundOff()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ROUNDOFF') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ROUNDOFF "
        StrSql += " SELECT 'ROUNDOFF' CATNAME, "
        StrSql += " SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END) AS AMT,"
        StrSql += " 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += " INTO " & cnAdminDb & "..TEMP" & systemId & "ROUNDOFF FROM " & cnStockDb & "..ACCTRAN AS A "
        strSql += " WHERE A.TRANDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND "
        strSql += " ISNULL(A.CANCEL,'') = '' "
        StrSql += " AND PAYMODE = 'RO'  AND COMPANYID IN (" & SelectedCompanyId & ")"
        strSql += vbCrLf + "  AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN('" & cmbCounterID_OWN.Text & "'))"
        strSql += vbCrLf + StrUseridFtr
        strSql += " AND FROMFLAG NOT IN ('','S','O','A')"
        StrSql += " HAVING ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END),0) <> 0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "ROUNDOFF)>0 "
        StrSql += " BEGIN "
        'StrSql += " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        strSql += " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS,RECEIPT,PAYMENT,RESULT1,COLHEAD) "
        StrSql += " SELECT '  ' + CATNAME CATNAME, "
        StrSql += " CASE WHEN AMT>= 0 THEN AMT  ELSE NULL END RECEIPT, "
        StrSql += " CASE WHEN AMT<= 0 THEN ABS(AMT)  ELSE NULL END PAYMENT, "
        StrSql += " 1 RESULT1, COLHEAD  "
        strSql += " FROM " & cnAdminDb & "..TEMP" & systemId & "ROUNDOFF ORDER BY CATNAME "
        StrSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub ProcChitCollection()
        strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITCOLLECTION') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "CHITCOLLECTION "
        StrSql += VBCRLF + " SELECT CONVERT(NUMERIC(15,2),0)RECEIPT,CONVERT(NUMERIC(15,2),0)PAYMENT"
        strSql += vbCrLf + " INTO " & cnAdminDb & "..TEMP" & systemId & "CHITCOLLECTION"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        ' If hasChit Then
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CHITDBPREFIX'"
        If (Trim(objGPack.GetSqlValue(strSql, , ""))) <> "" Then
            strSql = "  IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CHITCOLLECTION') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "CHITCOLLECTION "
            strSql += vbCrLf + " SELECT  CASE "
            strSql += vbCrLf + " WHEN MODEPAY = 'C' THEN 'SCHEME CASH' "
            strSql += vbCrLf + " WHEN MODEPAY IN('Q','D') THEN 'CHEQUE' "
            strSql += vbCrLf + " WHEN MODEPAY = 'R' THEN 'CREDITCARD'"
            strSql += vbCrLf + " WHEN MODEPAY = 'E' THEN 'ETRANSFER'"
            strSql += vbCrLf + " WHEN MODEPAY = 'O' THEN 'OTHERS'"
            strSql += vbCrLf + " END CATNAME,"
            strSql += vbCrLf + " SUM(AMOUNT) AS RECEIPT, 0 AS PAYMENT "
            strSql += vbCrLf + " INTO " & cnAdminDb & "..TEMP" & systemId & "CHITCOLLECTION FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
            strSql += vbCrLf + " WHERE RDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
            strSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + StrUseridFtr
            strSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
            strSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))AND T.GROUPCODE = SC.GROUPCODE AND T.REGNO = SC.REGNO)"
            strSql += vbCrLf + " GROUP BY MODEPAY"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT CATNAME,SUM(RECEIPT)RECEIPT,SUM(PAYMENT)PAYMENT  FROM"
            strSql += vbCrLf + " (SELECT  'CC COMMISION' CATNAME,"
            strSql += vbCrLf + " (SELECT AMOUNT FROM " & cnChitCompanyid & "SAVINGS..RECPAY WHERE T.ENTREFNO = EntRefNo) AS RECEIPT, 0 AS PAYMENT "
            strSql += vbCrLf + " FROM " & cnChitTrandb & "..SCHEMECOLLECT AS T"
            strSql += vbCrLf + " WHERE RDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
            strSql += vbCrLf + " MODEPAY='R' AND "
            strSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + StrUseridFtr
            strSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMECOLLECT AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
            strSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & ")))AND T.GROUPCODE = SC.GROUPCODE AND T.REGNO = SC.REGNO))X"
            strSql += vbCrLf + " group by X.CATNAME "

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "CHITCOLLECTION)>0 "
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT(PARTICULARS) VALUES('') "
            strSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT(PARTICULARS,COLHEAD) "
            'StrSql += VBCRLF + " VALUES('SCHEME COLLECTION ','T') "
            strSql += vbCrLf + " SELECT 'SCHEME COLLECTION [' +  CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),SUM(AMOUNT))) + ']' AS PARTICULAR,'T' FROM " & cnChitTrandb & "..SCHEMETRAN "
            strSql += vbCrLf + " WHERE RDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
            strSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + StrUseridFtr
            strSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
            strSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"

            strSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT(PARTICULARS) "
            strSql += vbCrLf + " SELECT PARTICULAR FROM "
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT 'NEW : Rs '+ CASE WHEN SUM(AMOUNT) > 0 THEN CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),SUM(AMOUNT))) + ' ('+CONVERT(VARCHAR,COUNT(*)) + ' NOs)' ELSE '' END AS PARTICULAR FROM " & cnChitTrandb & "..SCHEMETRAN "
            strSql += vbCrLf + " WHERE RDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
            strSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + StrUseridFtr
            strSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
            strSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"
            strSql += vbCrLf + " AND INSTALLMENT = 1"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'OTHER : Rs '+ CASE WHEN SUM(AMOUNT) > 0 THEN CONVERT(VARCHAR,ISNULL(CONVERT(NUMERIC(15,2),SUM(AMOUNT)),'')) + ' ('+CONVERT(VARCHAR,ISNULL(COUNT(*),'')) + ' NOs)' ELSE '' END AS PARTICULAR FROM " & cnChitTrandb & "..SCHEMETRAN"
            strSql += vbCrLf + " WHERE RDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND"
            strSql += vbCrLf + " ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + StrUseridFtr
            strSql += vbCrLf + "  AND EXISTS (SELECT 1 FROM " & cnChitTrandb & "..SCHEMETRAN AS SC WHERE EXISTS (SELECT 1 FROM " & cnChitCompanyid & "SAVINGS..SCHEMEMAST WHERE GROUPCODE = SC.GROUPCODE AND REGNO = SC.REGNO AND COMPANYID IN"
            strSql += vbCrLf + "  (SELECT COMPANYID FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE JCOMPID IN (" & SelectedCompanyId & "))) )"
            strSql += vbCrLf + " AND INSTALLMENT <> 1"
            strSql += vbCrLf + " )C"

            strSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
            strSql += vbCrLf + " (PARTICULARS,RECEIPT,PAYMENT) "
            strSql += vbCrLf + " SELECT '  ' + CATNAME CATNAME,  "
            strSql += vbCrLf + " CASE WHEN RECEIPT<> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
            strSql += vbCrLf + " CASE WHEN PAYMENT<> 0 THEN PAYMENT  ELSE NULL END PAYMENT "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..TEMP" & systemId & "CHITCOLLECTION ORDER BY CATNAME "
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            'End If
        End If
    End Sub

    Private Sub ProcAmtSubtot()
        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "COLDET)>0 "
        strSql += " BEGIN "
        strSql += " IF EXISTS(SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSCOLDETTOT') DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "ABSCOLDETTOT "
        strSql += " SELECT   SUM(RECEIPT) AS RECEIPT, SUM(PAYMENT) AS PAYMENT INTO " & cnAdminDb & "..TEMP" & systemId
        strSql += "ABSCOLDETTOT FROM " & cnAdminDb & "..TEMP" & systemId & "COLDET END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        DT = New DataTable
        strSql = " SELECT * FROM " & cnAdminDb & "..TEMP" & systemId & "COLDET"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DT)
        If DT.Rows.Count <= 0 Then Exit Sub

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCOLDETTOT)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS,RECEIPT,PAYMENT,COLHEAD) "
        strSql += " SELECT  'TOTAL', "
        strSql += " CASE WHEN RECEIPT<> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
        strSql += " CASE WHEN PAYMENT<> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        strSql += " 'S' FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCOLDETTOT "
        strSql += " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  IF (SELECT COUNT(*) FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCOLDETTOT)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO " & cnAdminDb & "..TEMP" & systemId & "COLLECTIONRPT"
        strSql += " (PARTICULARS,RECEIPT,PAYMENT,COLHEAD) "
        strSql += " SELECT 'GRAND TOTAL', ISNULL((CASE WHEN X>0 THEN X END),NULL) AS RECEIPT,"
        strSql += " ISNULL((CASE WHEN X<0 THEN X END),NULL) AS PAYMENT, 'G'"
        strSql += " FROM ("
        strSql += " SELECT RECEIPT-PAYMENT  AS X FROM " & cnAdminDb & "..TEMP" & systemId & "ABSCOLDETTOT) Y"
        strSql += " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmCashCounterCollection_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If GridView2.Rows.Count > 0 Then
            Dim title As String
            title = "CASH COLLECTION REPORT"
            title += " For Date : " & dtpFrom.Text & " ."
            If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then title += " Costcentre : " & cmbCostCentre.Text & " ."
            BrightPosting.GExport.Post(Me.Name, strCompanyName, title, GridView2, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
End Class