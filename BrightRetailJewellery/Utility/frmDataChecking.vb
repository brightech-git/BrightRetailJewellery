Imports System.Data.OleDb

Public Class frmDataChecking
    Public Enum Type
        Issue = 0
        Receipt = 1
        Collect = 2
    End Enum
    Dim cmd As OleDbCommand
    Dim IssRec As Type
    Dim strSql As String
    Dim ChitDb As String = GetAdmindbSoftValue("CHITDBPREFIX", "")
    Dim FlagDetailView As Boolean = False
    Dim Batchno As String
    Dim CostId As String = ""

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Public Sub New(ByVal IssRec As Type)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.IssRec = IssRec

        ' Add any initialization after the InitializeComponent() call.
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY ORDER BY CATNAME"
        cmbCategory.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbCategory, False, False)
        cmbCategory.Text = "ALL"
        If Me.IssRec = Type.Issue Then
            Me.Text = "Issue Vs Transaction Data Checking"
        ElseIf Me.IssRec = Type.Receipt Then
            Me.Text = "Receipt Vs Transaction Data Checking"
        Else
            Me.Text = "Savings Vs Accounts Data Reconzilation"
            lblCategory.Text = "AcName"
            cmbCategory.Items.Clear()
            cmbCategory.Items.Add("ALL")
            strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='C' ORDER BY NAME"
            objGPack.FillCombo(strSql, cmbCategory, False, False)
            cmbCategory.Text = "ALL"
            btnDetail.Visible = True
            btnUpdate.Visible = True
            CmbCostName.Visible = True
            lblCostcenter.Visible = True
            'btnAdjust.Visible = True
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY COSTNAME "
            CmbCostName.Items.Add("ALL")
            objGPack.FillCombo(strSql, CmbCostName, False, False)
            CmbCostName.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then CmbCostName.Enabled = False
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = dtpFrom.Value
        cmbCategory.Text = "ALL"
        gridView.DataSource = Nothing
        If Me.IssRec = Type.Collect Then
            rbtDiffer.Visible = True
        Else
            rbtDiffer.Visible = False
        End If
        rbtAll.Checked = True
        lblStatus.Text = ""
        lblStatus.Visible = False
        dtpFrom.Focus()
        DataGridView1.Visible = False
    End Sub

    Private Sub frmDataChecking_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmDataChecking_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridView.SelectionMode = DataGridViewSelectionMode.CellSelect
        gridView.MultiSelect = True
        btnNew_Click(Me, New EventArgs)
    End Sub
    Public Sub CheckCollect(ByVal frmDate As String, ByVal toDate As String, ByVal Accode As String, ByVal CostId As String, ByVal companyId As String)
        If ChitDb = "" Then Exit Sub
        ChitDb = Mid(ChitDb, 1, 3) + "SH0708"
        If Accode <> "ALL" Then
            strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & Accode & "'"
            Accode = objGPack.GetSqlValue(strSql, "ACCODE", "").ToString.Trim
        End If
        strSql = " /** COLLECT VS ACCTRAN **/"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "DATACHECK','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DATACHECK"
        strSql += vbCrLf + " SELECT NAME TRANDATE,TRANDATE TRANDATE1,NAME,ACCODE,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),COLLECTION))COLLECTION,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),TRANSACT))TRANSACT,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),COLLECTION))-"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),TRANSACT))TRANDIFF ,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),ACCOUNTS))ACCOUNTS ,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),COLLECTION))-"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),ACCOUNTS))COLLECTDIFF ,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),LEDGER))LEDGER ,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),SCCLOSED))SCCLOSED,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),ACCLOSED))ACCLOSED ,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),CHQRETURN))CHQRETURN ,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),ACCLOSED))+"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),CHQRETURN))TOTAL ,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),SCCLOSED))-"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),ACCLOSED))CLOSEDIFF ,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),LEDGER1))LEDGER1 ,"
        strSql += vbCrLf + " 1 AS RESULT  "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK FROM ("
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " RDATE AS TRANDATE ,AC.ACNAME AS NAME,CC.ACCODE"
        strSql += vbCrLf + " ,SUM(AMOUNT) COLLECTION"
        strSql += vbCrLf + " ,0 TRANSACT,0 ACCOUNTS,0 LEDGER,0 SCCLOSED, 0 ACCLOSED,0 CHQRETURN,0 AS LEDGER1"
        strSql += vbCrLf + " FROM " & ChitDb & "..SCHEMECOLLECT C "
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=C.GROUPCODE AND M.REGNO=C.REGNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.SCHEMECODE =M.SCHEMEID "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE =CC.ACCODE "
        strSql += vbCrLf + " WHERE RDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        'strSql += vbCrLf + " AND RECEIPTNO IS NOT NULL"
        'strSql += vbCrLf + " AND ISNULL(CHEQUERETDATE,'')=''"
        If Accode <> "ALL" Then
            strSql += vbCrLf + " AND CC.ACCODE='" & Accode & "'"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY AC.ACNAME ,CC.ACCODE,RDATE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " RDATE AS TRANDATE ,AC.ACNAME AS NAME,CC.ACCODE"
        strSql += vbCrLf + " ,-1*SUM(ISNULL(TAX,0)) COLLECTION"
        strSql += vbCrLf + " ,0 TRANSACT,0 ACCOUNTS,0 LEDGER,0 SCCLOSED, 0 ACCLOSED,0 CHQRETURN,0 AS LEDGER1"
        strSql += vbCrLf + " FROM " & ChitDb & "..SCHEMETRAN C "
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=C.GROUPCODE AND M.REGNO=C.REGNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.SCHEMECODE =M.SCHEMEID "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE =CC.ACCODE "
        strSql += vbCrLf + " WHERE RDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        If Accode <> "ALL" Then
            strSql += vbCrLf + " AND CC.ACCODE='" & Accode & "'"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY AC.ACNAME ,CC.ACCODE,RDATE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " RDATE AS TRANDATE ,AC.ACNAME AS NAME,CC.ACCODE,0 COLLECTION,SUM(AMOUNT) TRANSACT,0 ACCOUNTS,0 LEDGER,0 SCCLOSED, 0 ACCLOSED,0 CHQRETURN,0 AS LEDGER1"
        strSql += vbCrLf + " FROM " & ChitDb & "..SCHEMETRAN C "
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=C.GROUPCODE AND M.REGNO=C.REGNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.SCHEMECODE =M.SCHEMEID "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE =CC.ACCODE "
        strSql += vbCrLf + " WHERE RDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        If Accode <> "ALL" Then
            strSql += vbCrLf + " AND CC.ACCODE='" & Accode & "'"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY AC.ACNAME ,CC.ACCODE,RDATE"
        strSql += vbCrLf + " UNION ALL"
        If Accode = "ALL" Then
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CHEQUERETDATE AS TRANDATE ,'CHEQUE RETURN' ,'CHEQUE RETURN',SUM(AMOUNT) COLLECTION,0 TRANSACT,0 ACCOUNTS,0 LEDGER,0 SCCLOSED, 0 ACCLOSED,0 CHQRETURN,0 AS LEDGER1"
            strSql += vbCrLf + " FROM " & ChitDb & "..SCHEMECOLLECT C "
            strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=C.GROUPCODE AND M.REGNO=C.REGNO"
            strSql += vbCrLf + " WHERE CHEQUERETDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
            If Accode <> "ALL" Then
                strSql += vbCrLf + " AND M.SCHEMEID=(SELECT TOP 1 SCHEMECODE FROM " & cnAdminDb & "..CREDITCARD WHERE ACCODE='" & Accode & "')"
            End If
            If CostId <> "" Then
                strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
            End If
            strSql += vbCrLf + " GROUP BY CHEQUERETDATE"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CHEQUERETDATE AS TRANDATE ,'CHEQUE RETURN' ,'CHEQUE RETURN',0 COLLECTION,SUM(AMOUNT)+SUM(ISNULL(TAX,0)) TRANSACT,0 ACCOUNTS,0 LEDGER,0 SCCLOSED, 0 ACCLOSED,0 CHQRETURN,0 AS LEDGER1"
            strSql += vbCrLf + " FROM " & ChitDb & "..SCHEMETRAN C "
            strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=C.GROUPCODE AND M.REGNO=C.REGNO"
            strSql += vbCrLf + " WHERE CHEQUERETDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
            If Accode <> "ALL" Then
                strSql += vbCrLf + " AND M.SCHEMEID=(SELECT TOP 1 SCHEMECODE FROM " & cnAdminDb & "..CREDITCARD WHERE ACCODE='" & Accode & "')"
            End If
            If CostId <> "" Then
                strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
            End If
            strSql += vbCrLf + " GROUP BY CHEQUERETDATE"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TRANDATE,AC.ACNAME,AC.ACCODE,0 COLLECTION,0 TRANSACT, SUM(CASE WHEN TRANMODE='C' THEN AMOUNT ELSE -1*AMOUNT END) ACCOUNTS,0 LEDGER,0 SCCLOSED, 0 ACCLOSED,0 CHQRETURN,0 AS LEDGER1"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN SC"
            strSql += vbCrLf + " JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE =SC.ACCODE "
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "' AND TRANNO =9999 AND FROMFLAG ='C'"
            strSql += vbCrLf + " AND ISNULL(REMARK1,'')<>'CHEQUE RETURN'"
            strSql += vbCrLf + " AND AC.ACCODE IN(SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='C')"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
            If Accode <> "ALL" Then
                strSql += vbCrLf + " AND SC.ACCODE='" & Accode & "'"
            End If
            If CostId <> "" Then
                strSql += vbCrLf + " AND SC.COSTID='" & CostId & "'"
            End If
            strSql += vbCrLf + " GROUP BY AC.ACNAME,AC.ACCODE,TRANDATE"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TRANDATE,AC.ACNAME,AC.ACCODE,0 COLLECTION,0 TRANSACT,0 ACCOUNTS,SUM(AMOUNT) LEDGER,0 SCCLOSED, 0 ACCLOSED,0 CHQRETURN,0 AS LEDGER1"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN SC"
            strSql += vbCrLf + " JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE =SC.ACCODE "
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "' AND TRANMODE ='C' "
            strSql += vbCrLf + " AND AC.ACCODE IN(SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='C')"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
            If Accode <> "ALL" Then
                strSql += vbCrLf + " AND SC.ACCODE='" & Accode & "'"
            End If
            If CostId <> "" Then
                strSql += vbCrLf + " AND SC.COSTID='" & CostId & "'"
            End If
            strSql += vbCrLf + " GROUP BY AC.ACNAME,AC.ACCODE,TRANDATE"

            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TRANDATE,'CHEQUE RETURN','CHEQUE RETURN',0 COLLECTION,0 TRANSACT, SUM(AMOUNT) ACCOUNTS,0 LEDGER,0 SCCLOSED, 0 ACCLOSED,0 CHQRETURN,0 AS LEDGER1"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN SC"
            strSql += vbCrLf + " JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE =SC.ACCODE "
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "' AND TRANNO =9999 AND TRANMODE ='C'"
            strSql += vbCrLf + " AND ISNULL(REMARK1,'')='CHEQUE RETURN'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
            If Accode <> "ALL" Then
                strSql += vbCrLf + " AND SC.ACCODE='" & Accode & "'"
            End If
            If CostId <> "" Then
                strSql += vbCrLf + " AND SC.COSTID='" & CostId & "'"
            End If
            strSql += vbCrLf + " GROUP BY TRANDATE"
            strSql += vbCrLf + " UNION ALL"
        End If
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " DOCLOSE AS TRANDATE ,AC.ACNAME ,CC.ACCODE,0 COLLECTION,0 TRANSACT,0 ACCOUNTS,0 LEDGER,SUM(AMOUNT) SCCLOSED,0 ACCLOSED,0 CHQRETURN,0 AS LEDGER1"
        strSql += vbCrLf + " FROM " & ChitDb & "..SCHEMECOLLECT C "
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=C.GROUPCODE AND M.REGNO=C.REGNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.SCHEMECODE =M.SCHEMEID "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE =CC.ACCODE "
        strSql += vbCrLf + " WHERE DOCLOSE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        strSql += vbCrLf + " AND ISNULL(CHEQUERETDATE,'')=''"
        strSql += vbCrLf + " AND RECEIPTNO IS NOT NULL"
        If Accode <> "ALL" Then
            strSql += vbCrLf + " AND CC.ACCODE='" & Accode & "'"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY AC.ACNAME ,CC.ACCODE,DOCLOSE"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " DOCLOSE AS TRANDATE ,AC.ACNAME ,CC.ACCODE,0 COLLECTION,0 TRANSACT,0 ACCOUNTS,0 LEDGER"
        strSql += vbCrLf + " ,-1* SUM(ISNULL(TAX,0)) AS SCCLOSED"
        strSql += vbCrLf + " ,0 ACCLOSED,0 CHQRETURN,0 AS LEDGER1"
        strSql += vbCrLf + " FROM " & ChitDb & "..SCHEMETRAN C "
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=C.GROUPCODE AND M.REGNO=C.REGNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.SCHEMECODE =M.SCHEMEID "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE =CC.ACCODE "
        strSql += vbCrLf + " WHERE DOCLOSE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        strSql += vbCrLf + " AND ISNULL(CHEQUERETDATE,'')=''"
        strSql += vbCrLf + " AND RECEIPTNO IS NOT NULL"
        If Accode <> "ALL" Then
            strSql += vbCrLf + " AND CC.ACCODE='" & Accode & "'"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY AC.ACNAME ,CC.ACCODE,DOCLOSE"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + " SELECT TRANDATE,A.ACNAME,AC.ACCODE,0 COLLECTION,0 TRANSACT,0 ACCOUNTS,0 LEDGER,0 SCCLOSED, SUM(AMOUNT) ACCLOSED,0 CHQRETURN,0 AS LEDGER1"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN SC"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD AC ON AC.ACCODE =SC.ACCODE "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD A ON AC.ACCODE =A.ACCODE "
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=SC.CHQCARDREF  AND CAST(M.REGNO AS VARCHAR)=CAST(SC.CHQCARDNO AS VARCHAR) "
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += vbCrLf + " AND SC.PAYMODE IN('SS','HP','CZ')"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        If Accode <> "ALL" Then
            strSql += vbCrLf + " AND SC.ACCODE='" & Accode & "'"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND SC.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY A.ACNAME,AC.ACCODE,TRANDATE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TRANDATE,AC.ACNAME,AC.ACCODE,0 COLLECTION,0 TRANSACT,0 ACCOUNTS,0 LEDGER,0 SCCLOSED, 0 ACCLOSED,0 CHQRETURN,SUM(AMOUNT)AS LEDGER1"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN SC"
        strSql += vbCrLf + " JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE =SC.ACCODE "
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "' AND TRANMODE ='D' "
        strSql += vbCrLf + " AND AC.ACCODE IN(SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='C')"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        If Accode <> "ALL" Then
            strSql += vbCrLf + " AND SC.ACCODE='" & Accode & "'"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND SC.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY AC.ACNAME,AC.ACCODE,TRANDATE"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + " SELECT TRANDATE,A.ACNAME,AC.ACCODE,0 COLLECTION,0 TRANSACT,0 ACCOUNTS,0 LEDGER,0 SCCLOSED, 0 ACCLOSED ,SUM(AMOUNT) CHQRETURN,0 AS LEDGER1"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN SC"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD AC ON AC.ACCODE =SC.ACCODE "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD A ON AC.ACCODE =A.ACCODE "
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += vbCrLf + " AND SC.PAYMODE IN('CT')"
        strSql += vbCrLf + " AND REMARK1='CHEQUE RETURN'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        If Accode <> "ALL" Then
            strSql += vbCrLf + " AND SC.ACCODE='" & Accode & "'"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND SC.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY A.ACNAME,AC.ACCODE,TRANDATE"
        strSql += vbCrLf + " )X"
        strSql += vbCrLf + " GROUP BY TRANDATE,NAME,ACCODE"
        If rbtDiffer.Checked Then
            strSql += vbCrLf + " HAVING ABS(SUM(CONVERT(NUMERIC(20,2),SCCLOSED))-SUM(CONVERT(NUMERIC(20,2),ACCLOSED))) <> 0 OR "
            strSql += vbCrLf + " ABS(SUM(CONVERT(NUMERIC(20,2),COLLECTION))-SUM(CONVERT(NUMERIC(20,2),ACCOUNTS)))<>0 OR "
            strSql += vbCrLf + " ABS(SUM(CONVERT(NUMERIC(20,2),COLLECTION))-SUM(CONVERT(NUMERIC(20,2),TRANSACT)))<>0 "
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " IF(SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK(TRANDATE,TRANDATE1,RESULT,ACCODE)"
        strSql += vbCrLf + " SELECT DISTINCT CONVERT(VARCHAR(15),TRANDATE1,105),TRANDATE1,0,'' FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK "
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " IF(SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK(TRANDATE,TRANDATE1,ACCODE,COLLECTION,TRANSACT,ACCOUNTS,LEDGER,LEDGER1,TRANDIFF,COLLECTDIFF,SCCLOSED,ACCLOSED,CHQRETURN,TOTAL,CLOSEDIFF,RESULT)"
        strSql += vbCrLf + " SELECT 'TOTAL',TRANDATE1,'',SUM(COLLECTION),SUM(TRANSACT),SUM(ACCOUNTS),SUM(LEDGER),SUM(LEDGER1),SUM(TRANDIFF),SUM(COLLECTDIFF)"
        strSql += vbCrLf + " ,SUM(SCCLOSED),SUM(ACCLOSED),SUM(CHQRETURN),SUM(TOTAL),SUM(CLOSEDIFF),2  "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK GROUP BY TRANDATE1"
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        Dim DaysDiffer As Integer
        DaysDiffer = DateDiff(DateInterval.Day, dtpFrom.Value, dtpTo.Value)
        If DaysDiffer <> 0 Then
            strSql = vbCrLf + " IF(SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK (TRANDATE,TRANDATE1,RESULT)"
            strSql += vbCrLf + " SELECT 'SUMMARY','2059-01-01',0"
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " IF(SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK (TRANDATE,TRANDATE1,NAME,ACCODE,COLLECTION,TRANSACT ,ACCOUNTS ,LEDGER,LEDGER1,TRANDIFF,COLLECTDIFF ,SCCLOSED ,ACCLOSED ,CHQRETURN,CLOSEDIFF,TOTAL,RESULT )"
            strSql += vbCrLf + " SELECT DISTINCT NAME,'2059-01-01',NAME,ACCODE,SUM(COLLECTION),SUM(TRANSACT),SUM(ACCOUNTS),SUM(LEDGER),SUM(LEDGER1),SUM(TRANDIFF),SUM(COLLECTDIFF),SUM(SCCLOSED),SUM(ACCLOSED),SUM(CHQRETURN),SUM(CLOSEDIFF),SUM(TOTAL), 4 FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK WHERE RESULT=1 GROUP BY NAME,ACCODE"
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " IF(SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK (TRANDATE,TRANDATE1,COLLECTION,TRANSACT ,ACCOUNTS ,LEDGER1,LEDGER,TRANDIFF,COLLECTDIFF ,SCCLOSED ,ACCLOSED ,CHQRETURN,CLOSEDIFF ,TOTAL,RESULT )"
            strSql += vbCrLf + " SELECT 'TOTAL','2059-01-01',SUM(COLLECTION),SUM(TRANSACT),SUM(ACCOUNTS),SUM(LEDGER1),SUM(LEDGER),SUM(TRANDIFF),SUM(COLLECTDIFF),SUM(SCCLOSED),SUM(ACCLOSED),SUM(CHQRETURN),SUM(CLOSEDIFF),SUM(TOTAL),6 FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK WHERE RESULT=4 "
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        strSql = "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK SET COLLECTDIFF=NULL WHERE COLLECTDIFF=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK SET TRANDIFF=NULL WHERE TRANDIFF=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK SET CLOSEDIFF=NULL WHERE CLOSEDIFF=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK SET ACCOUNTS=NULL WHERE ACCOUNTS=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK SET LEDGER=NULL WHERE LEDGER=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK SET LEDGER1=NULL WHERE LEDGER1=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK SET COLLECTION=NULL WHERE COLLECTION=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK SET TRANSACT=NULL WHERE TRANSACT=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK SET ACCLOSED=NULL WHERE ACCLOSED=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK SET SCCLOSED=NULL WHERE SCCLOSED=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK SET CHQRETURN=NULL WHERE CHQRETURN=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK SET TOTAL=NULL WHERE TOTAL=0"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub CheckTranDetail(ByVal Trandate As String, ByVal Trandate1 As String, ByVal Accode As String)
        Dim TranDateD As String
        TranDateD = Trandate
        If Trandate = "2059-01-01" Then
            Trandate = Format(dtpFrom.Value, "yyyy-MM-dd")
            Trandate1 = Format(dtpTo.Value, "yyyy-MM-dd")
        End If
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "DATACHECK3','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DATACHECK3"
        strSql += vbCrLf + " SELECT TRANDATE,GROUPCODE,CONVERT(VARCHAR(15),REGNO)REGNO,SUM(COLLECT)COLLECT,SUM(CCANCEL)CCANCEL,SUM(CCHQRETURN)CCHQRETURN"
        strSql += vbCrLf + " ,SUM(TRANSACT)TRANSACT,SUM(TCANCEL)TCANCEL,SUM(TCHQRETURN)TCHQRETURN "
        strSql += vbCrLf + " ,SUM(COLLECT)-SUM(TRANSACT)DIFF,1 AS RESULT"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK3"
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT RDATE TRANDATE,C.GROUPCODE,C.REGNO,SUM(AMOUNT)COLLECT,0 CCANCEL,0 CCHQRETURN,0 TRANSACT,0 TCANCEL,0 TCHQRETURN  FROM " & ChitDb & "..SCHEMECOLLECT C"
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=C.GROUPCODE AND M.REGNO=C.REGNO"
        strSql += vbCrLf + " WHERE RDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        strSql += vbCrLf + " AND ISNULL(CHEQUERETDATE,'')=''"
        strSql += vbCrLf + " AND RECEIPTNO IS NOT NULL"
        If Accode <> "" Then
            strSql += vbCrLf + " AND SCHEMEID IN(SELECT SCHEMECODE FROM " & cnAdminDb & "..CREDITCARD WHERE ACCODE='" & Accode & "')"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY C.GROUPCODE,C.REGNO,RDATE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT RDATE TRANDATE,C.GROUPCODE,C.REGNO,0 COLLECT,SUM(AMOUNT) CCANCEL,0 CCHQRETURN,0 TRANSACT,0 TCANCEL,0 TCHQRETURN  FROM " & ChitDb & "..SCHEMECOLLECT C"
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=C.GROUPCODE AND M.REGNO=C.REGNO"
        strSql += vbCrLf + " WHERE RDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')='Y'"
        strSql += vbCrLf + " AND ISNULL(CHEQUERETDATE,'')=''"
        If Accode <> "" Then
            strSql += vbCrLf + " AND SCHEMEID IN(SELECT SCHEMECODE FROM " & cnAdminDb & "..CREDITCARD WHERE ACCODE='" & Accode & "')"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY C.GROUPCODE,C.REGNO,RDATE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CHEQUERETDATE TRANDATE,C.GROUPCODE,C.REGNO,0 COLLECT,0 CCANCEL,SUM(AMOUNT)CCHQRETURN ,0 TRANSACT,0 TCANCEL,0 TCHQRETURN FROM " & ChitDb & "..SCHEMECOLLECT C"
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=C.GROUPCODE AND M.REGNO=C.REGNO"
        strSql += vbCrLf + " WHERE CHEQUERETDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        If Accode <> "" Then
            strSql += vbCrLf + " AND SCHEMEID IN(SELECT SCHEMECODE FROM " & cnAdminDb & "..CREDITCARD WHERE ACCODE='" & Accode & "')"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY C.GROUPCODE,C.REGNO,CHEQUERETDATE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT RDATE TRANDATE,C.GROUPCODE,C.REGNO,0 COLLECT,0 CCANCEL,0 CCHQRETURN ,SUM(AMOUNT) TRANSACT,0 TCANCEL,0 TCHQRETURN  FROM " & ChitDb & "..SCHEMETRAN C"
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=C.GROUPCODE AND M.REGNO=C.REGNO"
        strSql += vbCrLf + " WHERE RDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        strSql += vbCrLf + " AND ISNULL(CHEQUERETDATE,'')=''"
        If Accode <> "" Then
            strSql += vbCrLf + " AND SCHEMEID IN(SELECT SCHEMECODE FROM " & cnAdminDb & "..CREDITCARD WHERE ACCODE='" & Accode & "')"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY C.GROUPCODE,C.REGNO,RDATE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT RDATE TRANDATE,C.GROUPCODE,C.REGNO,0 COLLECT,0 CCANCEL,0 CCHQRETURN,0 TRANSACT,SUM(AMOUNT) TCANCEL,0 TCHQRETURN FROM " & ChitDb & "..SCHEMETRAN C"
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=C.GROUPCODE AND M.REGNO=C.REGNO"
        strSql += vbCrLf + " WHERE RDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')='Y'"
        strSql += vbCrLf + " AND ISNULL(CHEQUERETDATE,'')=''"
        If Accode <> "" Then
            strSql += vbCrLf + " AND SCHEMEID IN(SELECT SCHEMECODE FROM " & cnAdminDb & "..CREDITCARD WHERE ACCODE='" & Accode & "')"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY C.GROUPCODE,C.REGNO,RDATE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CHEQUERETDATE TRANDATE,C.GROUPCODE,C.REGNO,0 COLLECT,0 CCANCEL,0 CCHQRETURN,0 TRANSACT,0 TCANCEL,SUM(AMOUNT)TCHQRETURN FROM " & ChitDb & "..SCHEMETRAN C"
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=C.GROUPCODE AND M.REGNO=C.REGNO"
        strSql += vbCrLf + " WHERE CHEQUERETDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        If Accode <> "" Then
            strSql += vbCrLf + " AND SCHEMEID IN(SELECT SCHEMECODE FROM " & cnAdminDb & "..CREDITCARD WHERE ACCODE='" & Accode & "')"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY C.GROUPCODE,C.REGNO,CHEQUERETDATE"
        strSql += vbCrLf + " )X GROUP BY GROUPCODE,REGNO,TRANDATE"
        If rbtDiffer.Checked Then
            strSql += vbCrLf + " HAVING SUM(COLLECT)-SUM(TRANSACT)<>0 "
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK3(GROUPCODE,TRANDATE,RESULT)"
        strSql += "SELECT DISTINCT CONVERT(VARCHAR(15),TRANDATE,105),TRANDATE,0 FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK3"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK3(TRANDATE,GROUPCODE,RESULT,DIFF,COLLECT,TRANSACT,CCANCEL,CCHQRETURN,TCANCEL,TCHQRETURN)"
        strSql += vbCrLf + " SELECT TRANDATE,'DIFF',3,SUM(DIFF),SUM(COLLECT),SUM(TRANSACT),SUM(CCANCEL),SUM(CCHQRETURN),SUM(TCANCEL),SUM(TCHQRETURN) FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK3 WHERE RESULT=1 GROUP BY TRANDATE"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If TranDateD = "2059-01-01" Then
            Dim DaysDiffer As Integer
            DaysDiffer = DateDiff(DateInterval.Day, dtpFrom.Value, dtpTo.Value)
            If DaysDiffer <> 0 Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK3(TRANDATE,GROUPCODE,RESULT,DIFF,COLLECT,TRANSACT,CCANCEL,CCHQRETURN,TCANCEL,TCHQRETURN)"
                strSql += vbCrLf + " SELECT '2059-01-01','GRAND DIFF',3,SUM(DIFF),SUM(COLLECT),SUM(TRANSACT),SUM(CCANCEL),SUM(CCHQRETURN),SUM(TCANCEL),SUM(TCHQRETURN) FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK3 WHERE RESULT=1"
                cmd = New OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        End If

        strSql = "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK3 SET COLLECT=NULL WHERE COLLECT=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK3 SET TRANSACT=NULL WHERE TRANSACT=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK3 SET CCANCEL=NULL WHERE CCANCEL=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK3 SET CCHQRETURN=NULL WHERE CCHQRETURN=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK3 SET TCANCEL=NULL WHERE TCANCEL=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK3 SET TCHQRETURN=NULL WHERE TCHQRETURN=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK3 SET DIFF=NULL WHERE DIFF=0"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK3 "
        strSql += vbCrLf + "ORDER BY TRANDATE,RESULT"
        Dim dtGridView As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridView)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Me.Cursor = Cursors.Default
            Exit Sub
        End If
        With gridView
            .DataSource = Nothing
            .DataSource = dtGridView
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            .Columns("COLLECT").DefaultCellStyle.BackColor = Color.Lavender
            .Columns("TRANSACT").DefaultCellStyle.BackColor = Color.Lavender
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("COLLECT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TRANSACT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CCANCEL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TCANCEL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CCHQRETURN").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TCHQRETURN").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RESULT").Visible = False
            .Columns("TRANDATE").Visible = False
            .Columns("COLLECT").HeaderText = "AMOUNT"
            .Columns("TRANSACT").HeaderText = "AMOUNT"
            .Columns("TCHQRETURN").HeaderText = "CHQRETURN"
            .Columns("CCHQRETURN").HeaderText = "CHQRETURN"
            .Columns("TCANCEL").HeaderText = "CANCEL"
            .Columns("CCANCEL").HeaderText = "CANCEL"
            For i As Integer = 0 To .Rows.Count - 1
                With .Rows(i)
                    If .Cells("RESULT").Value = 0 Then
                        .Cells("GROUPCODE").Style.BackColor = Color.LightGreen
                        .Cells("GROUPCODE").Style.ForeColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    ElseIf .Cells("RESULT").Value = 3 Then
                        .DefaultCellStyle.ForeColor = Color.Blue
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If
                End With
            Next
            gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridViewHeader.Columns("PARTICULARS").Width = .Columns("GROUPCODE").Width + .Columns("REGNO").Width
            gridViewHeader.Columns("COLLECTION").Width = .Columns("COLLECT").Width + .Columns("CCANCEL").Width + .Columns("CCHQRETURN").Width
            gridViewHeader.Columns("COLLECTION").HeaderText = "COLLECTION"
            gridViewHeader.Columns("CLOSING").Width = .Columns("TRANSACT").Width + .Columns("TCANCEL").Width + .Columns("TCHQRETURN").Width + .Columns("DIFF").Width
            gridViewHeader.Columns("CLOSING").HeaderText = "TRANSACTION"
            gridViewHeader.Columns("PARTICULARS").HeaderText = ""
        End With
    End Sub
    Private Sub CheckCollectDetail(ByVal Trandate As String, ByVal Trandate1 As String, ByVal Accode As String)
        If Trandate = "2059-01-01" Then
            Trandate = Format(dtpFrom.Value, "yyyy-MM-dd")
            Trandate1 = Format(dtpTo.Value, "yyyy-MM-dd")
        End If
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "DATACHECK1','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DATACHECK1"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(100),MODEPAY)MODEPAY,NAME,CONVERT(NUMERIC(15,2),SUM(COLLECT))COLLECT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(CANCEL))CANCEL,CONVERT(NUMERIC(15,2),SUM(ACCOUNTS))ACCOUNTS"
        strSql += vbCrLf + " ,ABS(CONVERT(NUMERIC(15,2),SUM(COLLECT)-SUM(ACCOUNTS)))DIFF ,1 AS RESULT"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK1 FROM ("
        strSql += vbCrLf + " SELECT CASE"
        strSql += vbCrLf + " WHEN MODEPAY='C' THEN 'CASH'"
        strSql += vbCrLf + " WHEN MODEPAY='R' THEN 'CARD'"
        strSql += vbCrLf + " WHEN MODEPAY='D' THEN 'CHEQUE' END MODEPAY,CC.NAME"
        strSql += vbCrLf + " ,SUM(AMOUNT)COLLECT,0 CANCEL,0 ACCOUNTS "
        strSql += vbCrLf + " FROM " & ChitDb & "..SCHEMECOLLECT C "
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE =C.GROUPCODE AND M.REGNO =C.REGNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.SCHEMECODE =M.SCHEMEID "
        'strSql += vbCrLf + " AND CARDTYPE ='C'"
        strSql += vbCrLf + " AND RDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " WHERE ISNULL(CANCEL,'')=''"
        strSql += vbCrLf + " GROUP BY MODEPAY,CC.NAME,CC.ACCODE "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CASE"
        strSql += vbCrLf + " WHEN MODEPAY='C' THEN 'CASH'"
        strSql += vbCrLf + " WHEN MODEPAY='R' THEN 'CARD'"
        strSql += vbCrLf + " WHEN MODEPAY='D' THEN 'CHEQUE' END MODEPAY,CC.NAME"
        strSql += vbCrLf + " ,SUM(AMOUNT)COLLECT,0 CANCEL,0 ACCOUNTS "
        strSql += vbCrLf + " FROM " & ChitDb & "..SCHEMECOLLECT C "
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE =C.GROUPCODE AND M.REGNO =C.REGNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.SCHEMECODE =M.SCHEMEID "
        strSql += vbCrLf + " AND CHEQUERETDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        strSql += vbCrLf + " WHERE ISNULL(CANCEL,'')=''"
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY MODEPAY,CC.NAME,CC.ACCODE "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CASE"
        strSql += vbCrLf + " WHEN MODEPAY='C' THEN 'CASH'"
        strSql += vbCrLf + " WHEN MODEPAY='R' THEN 'CARD'"
        strSql += vbCrLf + " WHEN MODEPAY='D' THEN 'CHEQUE' END MODEPAY,"
        strSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=ISNULL((SELECT CTLTEXT FROM " & Mid(ChitDb, 1, 3) & "SAVINGS..SOFTCONTROL WHERE CTLID='BANKCHARGE-ID'),'BCCHRG'))NAME "
        strSql += vbCrLf + " ,SUM(R.AMOUNT)COLLECT,0 CANCEL,0 ACCOUNTS "
        strSql += vbCrLf + " FROM " & ChitDb & "..SCHEMECOLLECT C "
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE =C.GROUPCODE AND M.REGNO =C.REGNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.SCHEMECODE =M.SCHEMEID "
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..RECPAY R ON R.ENTREFNO=C.ENTREFNO"
        strSql += vbCrLf + " WHERE TRANDATE='" & Trandate & "'"
        strSql += vbCrLf + " AND ISNULL(R.CANCEL,'')=''"
        strSql += vbCrLf + " AND ISNULL(C.CANCEL,'')=''"
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " AND ISNULL(C.MODEPAY,'')='R'"
        strSql += vbCrLf + " AND ISNULL(R.PAYMODE,'')='CC'"
        strSql += vbCrLf + " GROUP BY MODEPAY"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CASE"
        strSql += vbCrLf + " WHEN MODEPAY='C' THEN 'CASH'"
        strSql += vbCrLf + " WHEN MODEPAY='R' THEN 'CARD'"
        strSql += vbCrLf + " WHEN MODEPAY='D' THEN 'CHEQUE' END MODEPAY,"
        strSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=ISNULL((SELECT CTLTEXT FROM " & Mid(ChitDb, 1, 3) & "SAVINGS..SOFTCONTROL WHERE CTLID='NETCHARGE-ID'),'NETCHRG'))NAME "
        strSql += vbCrLf + " ,SUM(R.AMOUNT)COLLECT,0 CANCEL,0 ACCOUNTS "
        strSql += vbCrLf + " FROM " & ChitDb & "..SCHEMECOLLECT C "
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE =C.GROUPCODE AND M.REGNO =C.REGNO"
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..RECPAY R ON R.ENTREFNO=C.ENTREFNO"
        strSql += vbCrLf + " AND TRANDATE='" & Trandate & "'"
        strSql += vbCrLf + " WHERE ISNULL(R.CANCEL,'')=''"
        strSql += vbCrLf + " AND ISNULL(C.CANCEL,'')=''"
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " AND ISNULL(C.MODEPAY,'')='D'"
        strSql += vbCrLf + " GROUP BY MODEPAY"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CASE"
        strSql += vbCrLf + " WHEN MODEPAY='C' THEN 'CASH'"
        strSql += vbCrLf + " WHEN MODEPAY='R' THEN 'CARD'"
        strSql += vbCrLf + " WHEN MODEPAY='D' THEN 'CHEQUE' END MODEPAY,CC.NAME"
        strSql += vbCrLf + " ,0 COLLECT,SUM(AMOUNT)CANCEL,0 ACCOUNTS "
        strSql += vbCrLf + " FROM " & ChitDb & "..SCHEMECOLLECT C "
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE =C.GROUPCODE AND M.REGNO =C.REGNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.SCHEMECODE =M.SCHEMEID "
        strSql += vbCrLf + " AND CARDTYPE ='C'"
        strSql += vbCrLf + " AND RDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        strSql += vbCrLf + " WHERE ISNULL(CANCEL,'')='Y'"
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY MODEPAY,CC.NAME,CC.ACCODE "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CASE "
        strSql += vbCrLf + " WHEN PAYMODE='CA' THEN 'CASH'"
        strSql += vbCrLf + " WHEN PAYMODE='CC' THEN 'CARD'"
        strSql += vbCrLf + " WHEN PAYMODE='CH' THEN 'CHEQUE' END MODEPAY,CC.NAME"
        strSql += vbCrLf + " ,0 COLLECT,0 CANCEL,SUM(AMOUNT) ACCOUNTS "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN C "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.ACCODE=C.CONTRA  "
        'strSql += vbCrLf + " AND CARDTYPE ='C'"
        strSql += vbCrLf + " AND REMARK1 <>'CHEQUE RETURN'"
        strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        strSql += vbCrLf + " AND TRANNO=9999 AND TRANMODE ='D'"
        strSql += vbCrLf + " WHERE ISNULL(CANCEL,'')=''"
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY PAYMODE,CC.NAME,CC.ACCODE "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CASE "
        strSql += vbCrLf + " WHEN PAYMODE='BC' THEN 'CARD'"
        strSql += vbCrLf + " WHEN PAYMODE='CN' THEN 'CHEQUE' END MODEPAY,CC.ACNAME"
        strSql += vbCrLf + " ,0 COLLECT,0 CANCEL,SUM(AMOUNT) ACCOUNTS "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN C "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD CC ON CC.ACCODE=C.ACCODE  "
        strSql += vbCrLf + " AND REMARK1 <>'CHEQUE RETURN'"
        strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        strSql += vbCrLf + " AND TRANNO=9999 AND TRANMODE ='C'"
        strSql += vbCrLf + " AND PAYMODE IN('BC','CN')"
        strSql += vbCrLf + " WHERE ISNULL(CANCEL,'')=''"
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY PAYMODE,CC.ACNAME,CC.ACCODE "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CASE "
        strSql += vbCrLf + " WHEN PAYMODE='CA' THEN 'CASH'"
        strSql += vbCrLf + " WHEN PAYMODE='CC' THEN 'CARD'"
        strSql += vbCrLf + " WHEN PAYMODE='CH' THEN 'CHEQUE' END MODEPAY,CC.NAME"
        strSql += vbCrLf + " ,0 COLLECT,0 CANCEL,SUM(AMOUNT) ACCOUNTS "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN C "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.ACCODE=C.CONTRA  "
        strSql += vbCrLf + " AND CARDTYPE ='C'"
        strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        strSql += vbCrLf + " AND TRANNO=9999 --AND TRANMODE ='D'"
        strSql += vbCrLf + " AND REMARK1='CHEQUE RETURN'"
        strSql += vbCrLf + " WHERE ISNULL(CANCEL,'')=''"
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY PAYMODE,CC.NAME,CC.ACCODE "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT  "
        strSql += vbCrLf + " 'CARD'"
        strSql += vbCrLf + " MODEPAY,CC.NAME"
        strSql += vbCrLf + " ,0 COLLECT,0 CANCEL,-SUM(AMOUNT) ACCOUNTS "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN C "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.ACCODE=C.CONTRA  "
        strSql += vbCrLf + " AND CARDTYPE ='C'"
        strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        strSql += vbCrLf + " AND TRANNO=9999 AND TRANMODE ='C'"
        strSql += vbCrLf + " WHERE ISNULL(CANCEL,'')=''"
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " AND ISNULL(PAYMODE,'')='BC'"
        strSql += vbCrLf + " GROUP BY PAYMODE,CC.NAME,CC.ACCODE "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT  "
        strSql += vbCrLf + " 'CHEQUE'"
        strSql += vbCrLf + " MODEPAY,CC.NAME"
        strSql += vbCrLf + " ,0 COLLECT,0 CANCEL,-SUM(AMOUNT) ACCOUNTS "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN C "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.ACCODE=C.CONTRA  "
        strSql += vbCrLf + " AND CARDTYPE ='C'"
        strSql += vbCrLf + " AND TRANDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        strSql += vbCrLf + " AND TRANNO=9999 AND TRANMODE ='C'"
        strSql += vbCrLf + " WHERE ISNULL(CANCEL,'')=''"
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " AND ISNULL(PAYMODE,'')='CN'"
        strSql += vbCrLf + " GROUP BY PAYMODE,CC.NAME,CC.ACCODE "
        strSql += vbCrLf + " ) X GROUP BY MODEPAY,NAME"
        If rbtDiffer.Checked Then
            strSql += vbCrLf + " HAVING SUM(COLLECT)-SUM(ACCOUNTS)<>0 "
        End If

        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK1(MODEPAY,NAME,RESULT)"
        strSql += vbCrLf + " SELECT DISTINCT NAME,NAME,0 FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK1"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK1(MODEPAY,NAME,RESULT,DIFF,COLLECT,CANCEL,ACCOUNTS)"
        strSql += vbCrLf + " SELECT 'DIFF','ZZZZZ',3,SUM(DIFF),SUM(COLLECT),SUM(CANCEL),SUM(ACCOUNTS) FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK1 WHERE RESULT=1"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK1 SET COLLECT=NULL WHERE COLLECT=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK1 SET ACCOUNTS=NULL WHERE ACCOUNTS=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK1 SET CANCEL=NULL WHERE CANCEL=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK1 SET DIFF=NULL WHERE DIFF=0"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK1 "
        strSql += vbCrLf + "ORDER BY NAME,RESULT"
        Dim dtGridView As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridView)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Me.Cursor = Cursors.Default
            Exit Sub
        End If
        With gridView
            .DataSource = Nothing
            .DataSource = dtGridView
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("COLLECT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ACCOUNTS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CANCEL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MODEPAY").HeaderText = "PARTICULARS"
            .Columns("COLLECT").HeaderText = "SAVINGS"
            .Columns("RESULT").Visible = False
            .Columns("NAME").Visible = False
            For i As Integer = 0 To gridView.Rows.Count - 1
                With gridView.Rows(i)
                    If .Cells("RESULT").Value = 0 Then
                        .Cells("MODEPAY").Style.BackColor = Color.LightGreen
                        .Cells("MODEPAY").Style.ForeColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    ElseIf .Cells("RESULT").Value = 3 Then
                        .DefaultCellStyle.BackColor = Color.White
                        .DefaultCellStyle.ForeColor = Color.Blue
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If
                End With
            Next
            gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridViewHeader.Columns.Remove("CLOSING")
            gridViewHeader.Columns("PARTICULARS").Width = gridView.Columns("MODEPAY").Width
            gridViewHeader.Columns("COLLECTION").Width = gridView.Columns("COLLECT").Width + gridView.Columns("CANCEL").Width + gridView.Columns("ACCOUNTS").Width + gridView.Columns("DIFF").Width
            gridViewHeader.Columns("COLLECTION").HeaderText = "COLLECTION"
            gridViewHeader.Columns("PARTICULARS").HeaderText = ""
        End With
    End Sub
    Private Sub CheckClosedDetail(ByVal Trandate As String, ByVal Trandate1 As String, ByVal Accode As String)
        Dim TranDateD As String
        TranDateD = Trandate
        If Trandate = "2059-01-01" Then
            Trandate = Format(dtpFrom.Value, "yyyy-MM-dd")
            Trandate1 = Format(dtpTo.Value, "yyyy-MM-dd")
        End If
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "DATACHECK2','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DATACHECK2"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(20),BILLNO)BILLNO,CONVERT(VARCHAR,DOCLOSE,105)DOCLOSE,' ' CLOSEDBY,GROUPCODE,REGNO,CONVERT(NUMERIC(15,2),SUM(SCCLOSED))COLLECT "
        strSql += vbCrLf + " ,TRANNO,TRANDATE BILLDATE,SUM(ACCLOSED)ACCOUNTS,CONVERT(NUMERIC(15,2),SUM(SCCLOSED)-SUM(ACCLOSED))DIFF,1 AS RESULT"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK2 FROM("
        strSql += vbCrLf + " SELECT M.GROUPCODE,M.REGNO,M.BILLNO,DOCLOSE,SUM(AMOUNT)SCCLOSED"
        strSql += vbCrLf + " ,M.BILLNO AS TRANNO,DOCLOSE AS TRANDATE,0 AS ACCLOSED  "
        strSql += vbCrLf + " FROM  " & ChitDb & "..SCHEMECOLLECT C "
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=C.GROUPCODE AND M.REGNO=C.REGNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD CC ON CC.SCHEMECODE =M.SCHEMEID "
        strSql += vbCrLf + " WHERE DOCLOSE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        If Accode <> "" Then
            strSql += vbCrLf + " AND ISNULL(CC.ACCODE,'')='" & Accode & "'"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND C.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        strSql += vbCrLf + " AND ISNULL(CHEQUERETDATE,'')=''"
        strSql += vbCrLf + " AND RECEIPTNO IS NOT NULL"
        strSql += vbCrLf + " GROUP BY M.GROUPCODE,M.REGNO,M.BILLNO,DOCLOSE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CHQCARDREF,CHQCARDNO,TRANNO BILLNO,TRANDATE DOCLOSE,0 SCCLOSED,TRANNO BILLNO,TRANDATE DOCLOSE ,SUM(AMOUNT) AS ACCLOSED  "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN SC"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE =SC.ACCODE "
        strSql += vbCrLf + " INNER JOIN " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M ON M.GROUPCODE=SC.CHQCARDREF  AND CAST(M.REGNO AS VARCHAR)=CAST(SC.CHQCARDNO AS VARCHAR) "
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & Trandate & "' AND '" & Trandate1 & "'"
        strSql += vbCrLf + " AND SC.PAYMODE IN('SS','HP','CZ')"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        If Accode <> "" Then
            strSql += vbCrLf + " AND ISNULL(SC.ACCODE,'')='" & Accode & "'"
        End If
        If CostId <> "" Then
            strSql += vbCrLf + " AND SC.COSTID='" & CostId & "'"
        End If
        strSql += vbCrLf + " GROUP BY CHQCARDREF,CHQCARDNO,TRANNO ,TRANDATE "
        strSql += vbCrLf + " )X "
        strSql += vbCrLf + " GROUP BY GROUPCODE,REGNO,BILLNO,DOCLOSE,TRANNO,TRANDATE"
        strSql += vbCrLf + " ORDER BY TRANDATE ,TRANNO,GROUPCODE , REGNO"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK2(BILLNO,DOCLOSE,BILLDATE,RESULT,CLOSEDBY)"
        strSql += "SELECT DISTINCT DOCLOSE,DOCLOSE,BILLDATE,0,'' FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK2"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK2(BILLNO,DOCLOSE,BILLDATE,COLLECT,ACCOUNTS,DIFF,RESULT,CLOSEDBY)"
        strSql += "SELECT 'DIFF',DOCLOSE,BILLDATE,SUM(COLLECT),SUM(ACCOUNTS),SUM(DIFF),3 ,'' FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK2 WHERE RESULT=1"
        strSql += "GROUP BY DOCLOSE,BILLDATE"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If TranDateD = "2059-01-01" Then
            Dim DaysDiffer As Integer
            DaysDiffer = DateDiff(DateInterval.Day, dtpFrom.Value, dtpTo.Value)
            If DaysDiffer <> 0 Then
                strSql = "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK2(BILLNO,DOCLOSE,BILLDATE,COLLECT,ACCOUNTS,DIFF,RESULT,CLOSEDBY)"
                strSql += "SELECT 'GRAND DIFF','2059-01-01','2059-01-01',SUM(COLLECT),SUM(ACCOUNTS),SUM(DIFF),4 ,'' FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK2 WHERE RESULT=1"
                cmd = New OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        End If

        strSql = "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK2 SET COLLECT=NULL WHERE COLLECT=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK2 SET ACCOUNTS=NULL WHERE ACCOUNTS=0"
        strSql += "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK2 SET DIFF=NULL WHERE DIFF=0"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "UPDATE T SET CLOSEDBY=M.CLOSEDBY FROM " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST M,TEMPTABLEDB..TEMP" & systemId & "DATACHECK2 T WHERE T.GROUPCODE=M.GROUPCODE AND T.REGNO=M.REGNO"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = "SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK2 ORDER BY BILLDATE,RESULT"
        Dim dtGridView As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridView)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Me.Cursor = Cursors.Default
            Exit Sub
        End If
        'Dim objGridShower As frmGridDispDia
        'objGridShower = New frmGridDispDia
        'objGridShower.Name = Me.Name
        'objGridShower.gridView.RowTemplate.Height = 21
        'objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        'objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'objGridShower.Size = New Size(778, 550)
        'objGridShower.Text = "SAVINGS VS ACCOUNTS CLOSED DETAIL"
        'objGridShower.lblTitle.Text = "SAVINGS VS ACCOUNTS CLOSED DETAIL"
        'objGridShower.StartPosition = FormStartPosition.CenterScreen
        'objGridShower.dsGrid.DataSetName = objGridShower.Name
        'objGridShower.dsGrid.Tables.Add(dtGridView)
        'objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        'objGridShower.FormReLocation = False
        'objGridShower.pnlFooter.Visible = True
        'objGridShower.pnlFooter.BorderStyle = BorderStyle.Fixed3D
        'objGridShower.pnlFooter.Size = New Size(objGridShower.pnlFooter.Width, 20)
        'objGridShower.WindowState = FormWindowState.Maximized
        'objGridShower.Show()
        'With objGridShower.gridView
        '    .DataSource = Nothing
        '    .DataSource = dtGridView
        '    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        '    .Columns("COLLECT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        '    .Columns("ACCOUNTS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        '    .Columns("DIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        '    .Columns("REGNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        '    .Columns("BILLNO").HeaderText = "PARTICULARS"
        '    .Columns("COLLECT").HeaderText = "SAVINGS"
        '    .Columns("TRANNO").Visible = False
        '    .Columns("BILLDATE").Visible = False
        '    .Columns("DOCLOSE").Visible = False
        '    .Columns("RESULT").Visible = False
        '    For i As Integer = 0 To objGridShower.gridView.Rows.Count - 1
        '        With objGridShower.gridView.Rows(i)
        '            If .Cells("RESULT").Value = 0 Then
        '                .Cells("BILLNO").Style.BackColor = Color.LightGreen
        '                .Cells("BILLNO").Style.ForeColor = Color.Red
        '                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '            ElseIf .Cells("RESULT").Value = 3 Or .Cells("RESULT").Value = 4 Then
        '                .DefaultCellStyle.BackColor = Color.White
        '                .DefaultCellStyle.ForeColor = Color.Blue
        '                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '            End If
        '        End With
        '    Next
        'End With

        With gridView
            .DataSource = Nothing
            .DataSource = dtGridView
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("COLLECT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ACCOUNTS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("REGNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("BILLNO").HeaderText = "PARTICULARS"
            .Columns("COLLECT").HeaderText = "SAVINGS"
            .Columns("TRANNO").Visible = False
            .Columns("BILLDATE").Visible = False
            .Columns("DOCLOSE").Visible = False
            .Columns("RESULT").Visible = False
            For i As Integer = 0 To gridView.Rows.Count - 1
                With gridView.Rows(i)
                    If .Cells("RESULT").Value = 0 Then
                        .Cells("BILLNO").Style.BackColor = Color.LightGreen
                        .Cells("BILLNO").Style.ForeColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    ElseIf .Cells("RESULT").Value = 3 Or .Cells("RESULT").Value = 4 Then
                        .DefaultCellStyle.BackColor = Color.White
                        .DefaultCellStyle.ForeColor = Color.Blue
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If
                End With
            Next
        End With
        gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridViewHeader.Columns.Remove("CLOSING")
        gridViewHeader.Columns("PARTICULARS").Width = gridView.Columns("GROUPCODE").Width + gridView.Columns("REGNO").Width + gridView.Columns("BILLNO").Width + +gridView.Columns("CLOSEDBY").Width
        gridViewHeader.Columns("COLLECTION").Width = gridView.Columns("COLLECT").Width + gridView.Columns("ACCOUNTS").Width + gridView.Columns("DIFF").Width
        gridViewHeader.Columns("COLLECTION").HeaderText = "SAVINGS VS ACCOUNTS "
        gridViewHeader.Columns("PARTICULARS").HeaderText = "CLOSED"
    End Sub


    Public Sub CheckReceipt(ByVal frmDate As String, ByVal toDate As String, ByVal catCode As String, ByVal companyId As String)
        strSql = " /** RECEIPT VS ACCTRAN **/"
        strSql += " /**TAKING SA RECORD ONLY**/ /**/"
        strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP_SRPU')>0 /**/"
        strSql += " DROP TABLE TEMP_SRPU /**/"
        strSql += " SELECT 'IS'SEP,TRANNO,TRANDATE,PCS,GRSWT,NETWT,AMOUNT,TAX,CATCODE,BATCHNO,0 ITEMID,TRANTYPE PAYMODE /**/"
        strSql += " INTO TEMP_SRPU FROM " & cnstockdb & "..RECEIPT /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " AND TRANTYPE IN ('SR','PU') /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        strSql += " "
        strSql += " INSERT INTO TEMP_SRPU /**/"
        strSql += " SELECT 'AS'SEP,TRANNO,TRANDATE,PCS,GRSWT,NETWT,AMOUNT,0 TAX /**/"
        strSql += " ,(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PURCHASEID = T.ACCODE)AS CATCODE,T.BATCHNO,0 ITEMID,'PU'PAYMODE /**/"
        strSql += " FROM " & cnstockdb & "..ACCTRAN T /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM TEMP_SRPU WHERE PAYMODE = 'PU') /**/"
        strSql += " AND ACCODE IN (SELECT PURCHASEID FROM " & CNADMINDB & "..CATEGORY WHERE CATCODE IN (SELECT CATCODE FROM TEMP_SRPU WHERE PAYMODE = 'PU')) /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " "
        strSql += " UNION ALL /**/"
        strSql += " SELECT 'AS'SEP,TRANNO,TRANDATE,PCS,GRSWT,NETWT,AMOUNT,0 TAX /**/"
        strSql += " ,(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE SRETURNID = T.ACCODE)AS CATCODE,T.BATCHNO,0 ITEMID,'SR' PAYMODE /**/"
        strSql += " FROM " & cnstockdb & "..ACCTRAN T /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM TEMP_SRPU WHERE PAYMODE = 'SR') /**/"
        strSql += " AND ACCODE IN (SELECT SRETURNID FROM " & CNADMINDB & "..CATEGORY WHERE CATCODE IN (SELECT CATCODE FROM TEMP_SRPU WHERE PAYMODE = 'SR')) /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " UNION ALL /**/"
        strSql += " SELECT 'AS'SEP,TRANNO,TRANDATE,PCS,GRSWT,NETWT,0 AMOUNT,AMOUNT TAX /**/"
        strSql += " ,(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PTAXID = T.ACCODE)AS CATCODE,BATCHNO,0 ITEMID,'PU' PAYMODE /**/"
        strSql += " FROM " & cnstockdb & "..ACCTRAN T /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM TEMP_SRPU) /**/"
        strSql += " AND ACCODE IN (SELECT PTAXID FROM " & CNADMINDB & "..CATEGORY WHERE CATCODE IN (SELECT CATCODE FROM TEMP_SRPU)) /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " UNION ALL /**/"
        strSql += " SELECT 'AS'SEP,TRANNO,TRANDATE,PCS,GRSWT,NETWT,0 AMOUNT,AMOUNT TAX /**/"
        strSql += " ,(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE STAXID = T.ACCODE)AS CATCODE,BATCHNO,0 ITEMID,'SR'PAYMODE /**/"
        strSql += " FROM " & cnstockdb & "..ACCTRAN T /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM TEMP_SRPU) /**/"
        strSql += " AND ACCODE IN (SELECT STAXID FROM " & CNADMINDB & "..CATEGORY WHERE CATCODE IN (SELECT CATCODE FROM TEMP_SRPU)) /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP_RECVSTRAN')>0"
        strSql += " DROP TABLE TEMP_RECVSTRAN"
        strSql += " SELECT * INTO TEMP_RECVSTRAN FROM"
        strSql += " ("
        strSql += " SELECT  /**/"
        strSql += " TRANNO,TRANDATE /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'IS' THEN PCS  ELSE 0 END)AS IPCS /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'IS' THEN GRSWT ELSE 0 END)AS IGRSWT /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'IS' THEN NETWT ELSE 0 END)AS INETWT /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'IS' THEN AMOUNT ELSE 0 END)AS IAMOUNT /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'IS' THEN TAX ELSE 0 END)AS ITAX /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'AS' THEN PCS  ELSE 0 END)AS TPCS /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'AS' THEN GRSWT ELSE 0 END)AS TGRSWT /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'AS' THEN NETWT ELSE 0 END)AS TNETWT /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'AS' THEN AMOUNT ELSE 0 END)AS TAMOUNT /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'AS' THEN TAX ELSE 0 END)AS TTAX /**/"
        strSql += " ,CATCODE /**/"
        strSql += " ,BATCHNO"
        strSql += " ,ISNULL((SELECT CATNAME FROM " & CNADMINDB & "..CATEGORY WHERE CATCODE = T.CATCODE),T.CATCODE)AS CATNAME,PAYMODE /**/"
        strSql += " FROM TEMP_SRPU T /**/"
        strSql += " GROUP BY TRANNO,TRANDATE,BATCHNO,CATCODE,PAYMODE /**/"
        strSql += " )X"
        strSql += " ORDER BY TRANDATE,BATCHNO,TRANNO /**/"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Public Sub CheckIssue(ByVal frmDate As String, ByVal toDate As String, ByVal catCode As String, ByVal companyId As String)
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP_SA_BATCH')>0 /**SA ONLY**/ /**/ "
        strSql += " DROP TABLE TEMP_SA_BATCH /**/"
        strSql += " SELECT DISTINCT CATCODE,ITEMID,BATCHNO INTO TEMP_SA_BATCH FROM " & cnstockdb & "..ISSUE AS I /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND TRANTYPE = 'SA' AND NOT EXISTS (SELECT 1 FROM " & cnstockdb & "..ISSUE WHERE TRANTYPE <> 'SA' AND BATCHNO = I.BATCHNO) /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " "
        strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP_OD_BATCH')>0 /**OD ONLY**/ /**/"
        strSql += " DROP TABLE TEMP_OD_BATCH /**/"
        strSql += " SELECT DISTINCT CATCODE,ITEMID,BATCHNO INTO TEMP_OD_BATCH FROM " & cnstockdb & "..ISSUE AS I /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND TRANTYPE IN ('OD','RD') AND NOT EXISTS (SELECT 1 FROM " & cnstockdb & "..ISSUE WHERE TRANTYPE NOT IN ('OD','RD') AND BATCHNO = I.BATCHNO) /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " "
        strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP_SAOD_BATCH')>0 /**SA WITH OD**/ /**/"
        strSql += " DROP TABLE TEMP_SAOD_BATCH /**/"
        strSql += " SELECT DISTINCT CATCODE,ITEMID,BATCHNO  /**/"
        strSql += " INTO TEMP_SAOD_BATCH  /**/"
        strSql += " FROM " & cnstockdb & "..ISSUE AS I /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND TRANTYPE = 'SA' AND EXISTS (SELECT 1 FROM " & cnstockdb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE IN ('OD','RD')) /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " "
        strSql += " /**TAKING SA RECORD ONLY**/ /**/"
        strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP_SA')>0 /**/"
        strSql += " DROP TABLE TEMP_SA /**/"
        strSql += " SELECT 'IS'SEP,TRANNO,TRANDATE,PCS,GRSWT,NETWT,AMOUNT,TAX,CATCODE,BATCHNO,0 ITEMID /**/"
        strSql += " INTO TEMP_SA FROM " & cnstockdb & "..ISSUE /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM TEMP_SA_BATCH) /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " INSERT INTO TEMP_SA /**/"
        strSql += " SELECT 'AS'SEP,TRANNO,TRANDATE,PCS,GRSWT,NETWT,AMOUNT,0 TAX /**/"
        strSql += " ,(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE SALESID = T.ACCODE)AS CATCODE,T.BATCHNO,0 ITEMID /**/"
        strSql += " FROM " & cnStockDb & "..ACCTRAN T /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM TEMP_SA_BATCH) /**/"
        strSql += " AND ACCODE IN (SELECT SALESID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE IN (SELECT CATCODE FROM TEMP_SA)) /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " UNION ALL /**/"
        strSql += " SELECT 'AS'SEP,TRANNO,TRANDATE,PCS,GRSWT,NETWT,0 AMOUNT,AMOUNT TAX /**/"
        strSql += " ,(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE STAXID = T.ACCODE)AS CATCODE,BATCHNO,0 ITEMID /**/"
        strSql += " FROM " & cnstockdb & "..ACCTRAN T /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM TEMP_SA_BATCH) /**/"
        strSql += " AND ACCODE IN (SELECT STAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE IN (SELECT CATCODE FROM TEMP_SA)) /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " /**TAKING OD RECORD ONLY**/ /**/"
        strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP_OD')>0 /**/"
        strSql += " DROP TABLE TEMP_OD /**/"
        strSql += " SELECT 'IS'SEP,TRANNO,TRANDATE,PCS,GRSWT,NETWT,AMOUNT,TAX,(SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)CATCODE,BATCHNO,ITEMID /**/"
        strSql += " INTO TEMP_OD FROM " & cnStockDb & "..ISSUE AS I /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM TEMP_OD_BATCH) /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " INSERT INTO TEMP_OD /**/"
        strSql += " SELECT 'AS'SEP,TRANNO,TRANDATE,PCS,GRSWT,NETWT,AMOUNT,0 TAX /**/"
        strSql += " ,(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE SALESID = T.ACCODE)AS CATCODE,T.BATCHNO,0 ITEMID /**/"
        strSql += " FROM " & cnstockdb & "..ACCTRAN T /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM TEMP_OD_BATCH) /**/"
        strSql += " AND ACCODE IN (SELECT SALESID FROM " & CNADMINDB & "..CATEGORY WHERE CATCODE IN (SELECT CATCODE FROM TEMP_OD)) /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " UNION ALL /**/"
        strSql += " SELECT 'AS'SEP,TRANNO,TRANDATE,PCS,GRSWT,NETWT,0 AMOUNT,AMOUNT TAX /**/"
        strSql += " ,(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE STAXID = T.ACCODE)AS CATCODE,BATCHNO,0 ITEMID /**/"
        strSql += " FROM " & cnstockdb & "..ACCTRAN T /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM TEMP_OD_BATCH) /**/"
        strSql += " AND ACCODE IN (SELECT STAXID FROM " & CNADMINDB & "..CATEGORY WHERE CATCODE IN (SELECT CATCODE FROM TEMP_OD)) /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " "
        strSql += " /**TAKING SAOD RECORD ONLY**/ /**/"
        strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP_SAOD')>0 /**/"
        strSql += " DROP TABLE TEMP_SAOD /**/"
        strSql += " SELECT 'IS'SEP,TRANNO,TRANDATE,PCS,GRSWT,NETWT,AMOUNT,TAX,CATCODE,BATCHNO,0 ITEMID /**/"
        strSql += " INTO TEMP_SAOD FROM " & cnstockdb & "..ISSUE /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM TEMP_SAOD_BATCH) /**/"
        strSql += " AND TRANTYPE = 'SA' /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " UNION ALL"
        strSql += " SELECT 'IS'SEP,TRANNO,TRANDATE,0 PCS,0 GRSWT,0 NETWT,AMOUNT,0 TAX,'MISC'CATCODE,BATCHNO,0 ITEMID /**/"
        strSql += " FROM " & cnstockdb & "..ISSUE /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM TEMP_SAOD_BATCH) /**/"
        strSql += " AND TRANTYPE = 'OD' AND AMOUNT <> 0 /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " INSERT INTO TEMP_SAOD /**/"
        strSql += " SELECT 'AS'SEP,TRANNO,TRANDATE,PCS,GRSWT,NETWT,AMOUNT,0 TAX /**/"
        strSql += " ,ISNULL((SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE SALESID = T.ACCODE),T.ACCODE)AS CATCODE,T.BATCHNO,0 ITEMID /**/"
        strSql += " FROM " & cnStockDb & "..ACCTRAN T /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM TEMP_SAOD_BATCH) /**/"
        strSql += " AND PAYMODE IN ('SA') /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " UNION ALL /**/"
        strSql += " SELECT 'AS'SEP,TRANNO,TRANDATE,PCS,GRSWT,NETWT,0 AMOUNT,AMOUNT TAX /**/"
        strSql += " ,(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE STAXID = T.ACCODE)AS CATCODE,BATCHNO,0 ITEMID /**/"
        strSql += " FROM " & cnStockDb & "..ACCTRAN T /**/"
        strSql += " WHERE TRANDATE BETWEEN '" & frmDate & "' AND '" & toDate & "'"
        strSql += " AND BATCHNO IN (SELECT BATCHNO FROM TEMP_SAOD_BATCH) /**/"
        strSql += " AND PAYMODE IN ('SV') /**/"
        strSql += " AND ISNULL(CANCEL,'') = '' /**/"
        If catCode <> "" Then strSql += " AND CATCODE = '" & catCode & "'"
        If companyId <> "" Then strSql += " AND ISNULL(COMPANYID,'') = '" & companyId & "'"
        strSql += " "
        strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP_ISSVSTRAN')>0"
        strSql += " DROP TABLE TEMP_ISSVSTRAN"
        strSql += " SELECT * INTO TEMP_ISSVSTRAN FROM /**/"
        strSql += " ( /**Y STARTS**/ /**/"
        strSql += " SELECT  /**/"
        strSql += " TRANNO,TRANDATE /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'IS' THEN PCS  ELSE 0 END)AS IPCS /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'IS' THEN GRSWT ELSE 0 END)AS IGRSWT /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'IS' THEN NETWT ELSE 0 END)AS INETWT /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'IS' THEN AMOUNT ELSE 0 END)AS IAMOUNT /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'IS' THEN TAX ELSE 0 END)AS ITAX /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'AS' THEN PCS  ELSE 0 END)AS TPCS /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'AS' THEN GRSWT ELSE 0 END)AS TGRSWT /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'AS' THEN NETWT ELSE 0 END)AS TNETWT /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'AS' THEN AMOUNT ELSE 0 END)AS TAMOUNT /**/"
        strSql += " ,SUM(CASE WHEN SEP = 'AS' THEN TAX ELSE 0 END)AS TTAX /**/"
        strSql += " ,CATCODE /**/"
        strSql += " ,BATCHNO /**/"
        strSql += " ,ISNULL((SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE),X.CATCODE)AS CATNAME,'SA' PAYMODE"
        strSql += " FROM /**/"
        strSql += " 	( /**/"
        strSql += " 	SELECT * FROM TEMP_SA /**/"
        strSql += " 	UNION ALL /**/"
        strSql += " 	SELECT * FROM TEMP_OD /**/"
        strSql += " 	UNION ALL /**/"
        strSql += " 	SELECT * FROM TEMP_SAOD /**/"
        strSql += " 	)X /**/"
        strSql += " 	GROUP BY TRANNO,TRANDATE,BATCHNO,CATCODE /**/"
        strSql += " )Y /**/"
        strSql += " ORDER BY TRANDATE,BATCHNO,TRANNO /**/"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub StyleGridView()
        With gridView
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("CATCODE").Visible = False
            .Columns("BATCHNO").Visible = False
            .Columns("CATNAME").Visible = False
            .Columns("IPCS").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("IGRSWT").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("INETWT").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("IAMOUNT").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("ITAX").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("TPCS").DefaultCellStyle.BackColor = Color.Honeydew
            .Columns("TGRSWT").DefaultCellStyle.BackColor = Color.Honeydew
            .Columns("TNETWT").DefaultCellStyle.BackColor = Color.Honeydew
            .Columns("TAMOUNT").DefaultCellStyle.BackColor = Color.Honeydew
            .Columns("TTAX").DefaultCellStyle.BackColor = Color.Honeydew

            .Columns("IPCS").HeaderText = "PCS"
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("INETWT").HeaderText = "NETWT"
            .Columns("IAMOUNT").HeaderText = "AMOUNT"
            .Columns("ITAX").HeaderText = "TAX"

            .Columns("TPCS").HeaderText = "PCS"
            .Columns("TGRSWT").HeaderText = "GRSWT"
            .Columns("TNETWT").HeaderText = "NETWT"
            .Columns("TAMOUNT").HeaderText = "AMOUNT"
            .Columns("TTAX").HeaderText = "TAX"
            For i As Integer = 0 To .ColumnCount - 1
                If .Columns(i).ValueType.Name = GetType(String).Name Or .Columns(i).ValueType.Name = GetType(Date).Name Then
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                Else
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
            Next
        End With
    End Sub

    Private Sub StyleGridViewHeader()
        gridViewHeader.Enabled = False
        Dim dtGridViewHeader As New DataTable
        dtGridViewHeader.Columns.Add("TRANNO")
        dtGridViewHeader.Columns.Add("TRANDATE")

        dtGridViewHeader.Columns.Add("ISSUE")
        dtGridViewHeader.Columns.Add("TRANSACTION")
        gridViewHeader.DataSource = dtGridViewHeader
        With gridViewHeader
            .Columns("TRANNO").HeaderText = ""
            .Columns("TRANDATE").HeaderText = ""
            .Columns("TRANNO").Width = gridView.Columns("TRANNO").Width
            .Columns("TRANDATE").Width = gridView.Columns("TRANDATE").Width
            .Columns("ISSUE").Width = gridView.Columns("IPCS").Width + _
                                        gridView.Columns("IGRSWT").Width + _
                                        gridView.Columns("INETWT").Width + _
                                        gridView.Columns("IAMOUNT").Width + _
                                        gridView.Columns("ITAX").Width
            .Columns("TRANSACTION").Width = gridView.Columns("TPCS").Width + _
                                        gridView.Columns("TGRSWT").Width + _
                                        gridView.Columns("TNETWT").Width + _
                                        gridView.Columns("TAMOUNT").Width + _
                                        gridView.Columns("TTAX").Width
        End With
        If IssRec = Type.Receipt Then gridViewHeader.Columns("ISSUE").HeaderText = "RECEIPT"
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        Try
            gridView.DataSource = Nothing
            gridView.Refresh()
            gridViewHeader.DataSource = Nothing
            gridViewHeader.Refresh()
            CostId = ""
            Me.Cursor = Cursors.WaitCursor
            If IssRec = Type.Issue Then
                CheckIssue(dtpFrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), _
                objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory.Text & "'").ToString _
                , strCompanyId)
                strSql = " SELECT TRANNO,TRANDATE,IPCS,IGRSWT,INETWT,IAMOUNT,ITAX"
                strSql += " ,TPCS,TGRSWT,TNETWT,TAMOUNT,TTAX,CATNAME,BATCHNO,CATCODE"
                strSql += "  FROM TEMP_ISSVSTRAN"
            ElseIf IssRec = Type.Receipt Then
                CheckReceipt(dtpFrom.Value.Date.ToString("yyyy-MM-dd"), dtpTo.Value.Date.ToString("yyyy-MM-dd"), _
                objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory.Text & "'").ToString _
                , strCompanyId)
                strSql = " SELECT TRANNO,TRANDATE,IPCS,IGRSWT,INETWT,IAMOUNT,ITAX"
                strSql += " ,TPCS,TGRSWT,TNETWT,TAMOUNT,TTAX,CATNAME,BATCHNO,CATCODE"
                strSql += "  FROM TEMP_RECVSTRAN"
            ElseIf IssRec = Type.Collect Then
                If CmbCostName.Text <> "ALL" And CmbCostName.Text <> "" Then
                    strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & CmbCostName.Text & "'"
                    CostId = objGPack.GetSqlValue(strSql, "COSTID", "").ToString.Trim
                End If
                CheckCollect(dtpFrom.Value.Date.ToString("yyyy-MM-dd"), _
                            dtpTo.Value.Date.ToString("yyyy-MM-dd"), cmbCategory.Text, CostId, strCompanyId)
                strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK "
                strSql += " ORDER BY TRANDATE1,RESULT,ABS(COLLECTDIFF),NAME"
                FlagDetailView = False
                btnUpdate.Enabled = False
            End If
            Dim dtGridView As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            dtGridView.Columns.Add("KEYNO", GetType(Integer))
            dtGridView.Columns("KEYNO").AutoIncrement = True
            dtGridView.Columns("KEYNO").AutoIncrementSeed = 0
            dtGridView.Columns("KEYNO").AutoIncrementStep = 1
            da.Fill(dtGridView)
            If Not dtGridView.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Me.Cursor = Cursors.Default
                Exit Sub
            End If
            gridView.DataSource = dtGridView
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            If IssRec = Type.Collect Then
                With gridView
                    .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    .Columns("TOTAL").DefaultCellStyle.BackColor = Color.Lavender
                    .Columns("SCCLOSED").DefaultCellStyle.BackColor = Color.Lavender
                    .Columns("COLLECTION").DefaultCellStyle.BackColor = Color.LavenderBlush
                    .Columns("ACCOUNTS").DefaultCellStyle.BackColor = Color.LavenderBlush
                    .Columns("LEDGER").DefaultCellStyle.BackColor = Color.LavenderBlush
                    .Columns("LEDGER1").DefaultCellStyle.BackColor = Color.LavenderBlush
                    .Columns("COLLECTION").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("TRANSACT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("ACCOUNTS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("LEDGER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("LEDGER1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("SCCLOSED").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("ACCLOSED").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("CHQRETURN").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("TOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("CLOSEDIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("COLLECTDIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("TRANDIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("TRANDATE").HeaderText = "PARTICULARS"
                    .Columns("CLOSEDIFF").HeaderText = "DIFF"
                    .Columns("COLLECTDIFF").HeaderText = "DIFF"
                    .Columns("TRANDIFF").HeaderText = "DIFF"
                    .Columns("COLLECTION").HeaderText = "COLLECT"
                    .Columns("TRANSACT").HeaderText = "TRAN"
                    .Columns("SCCLOSED").HeaderText = "SAVINGS"
                    .Columns("ACCLOSED").HeaderText = "ACCOUNTS"
                    .Columns("LEDGER1").HeaderText = "LEDGER"
                    .Columns("CHQRETURN").HeaderText = "CHQRET"
                    .Columns("ACCODE").Visible = False
                    .Columns("NAME").Visible = False
                    .Columns("TRANDATE1").Visible = False
                    .Columns("RESULT").Visible = False
                    .Columns("KEYNO").Visible = False
                    .Columns("TRANDATE").Frozen = True
                    Dim dv As New DataView
                    Dim dt As New DataTable
                    dv = CType(gridView.DataSource, DataTable).Copy.DefaultView
                    dv.RowFilter = "RESULT IN(0,2,6)"
                    dt = dv.ToTable
                    Dim SNO As Integer
                    For i As Integer = 0 To dt.Rows.Count - 1
                        SNO = Val(dt.Rows(i).Item("KEYNO").ToString)
                        With gridView.Rows(SNO)
                            If .Cells("RESULT").Value = 0 Then
                                .Cells("TRANDATE").Style.BackColor = Color.LightGreen
                                .Cells("TRANDATE").Style.ForeColor = Color.Red
                                '.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                            ElseIf .Cells("RESULT").Value = 2 Or .Cells("RESULT").Value = 6 Then
                                .Cells("TRANDATE").Style.BackColor = Color.White
                                .DefaultCellStyle.ForeColor = Color.Blue
                                '.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                            End If
                        End With
                    Next
                    For i As Integer = 0 To .Columns.Count - 1
                        .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
                        .Columns(i).ReadOnly = True
                    Next
                    gridViewHeader.Enabled = False
                    Dim dtGridViewHeader As New DataTable
                    dtGridViewHeader.Columns.Add("PARTICULARS")
                    dtGridViewHeader.Columns.Add("COLLECTION")
                    dtGridViewHeader.Columns.Add("CLOSING")
                    dtGridViewHeader.Columns.Add("SCROLL")
                    With gridViewHeader
                        .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .DataSource = dtGridViewHeader
                        .Columns("PARTICULARS").Width = gridView.Columns("TRANDATE").Width
                        .Columns("COLLECTION").Width = gridView.Columns("ACCOUNTS").Width + gridView.Columns("COLLECTION").Width + gridView.Columns("TRANSACT").Width + gridView.Columns("COLLECTDIFF").Width + gridView.Columns("TRANDIFF").Width + gridView.Columns("LEDGER").Width
                        .Columns("CLOSING").Width = gridView.Columns("SCCLOSED").Width + gridView.Columns("ACCLOSED").Width + gridView.Columns("CHQRETURN").Width + gridView.Columns("TOTAL").Width + gridView.Columns("CLOSEDIFF").Width + gridView.Columns("LEDGER1").Width
                        .Columns("PARTICULARS").HeaderText = Replace(CmbCostName.Text, " ", "")
                        .Columns("COLLECTION").HeaderText = "COLLECTION"
                        .Columns("CLOSING").HeaderText = "CLOSED"
                        .Columns("SCROLL").HeaderText = ""
                        .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                        .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                        .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
                    End With
                End With
                DataGridView1.DataSource = gridView.DataSource
            Else
                StyleGridView()
                StyleGridViewHeader()
            End If
            gridView.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            If FlagDetailView Then
                FlagDetailView = False
                btnUpdate.Enabled = False
                gridView.DataSource = Nothing
                gridView.Refresh()
                gridView.DataSource = DataGridView1.DataSource
                With gridView
                    .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    .Columns("TOTAL").DefaultCellStyle.BackColor = Color.Lavender
                    .Columns("SCCLOSED").DefaultCellStyle.BackColor = Color.Lavender
                    .Columns("COLLECTION").DefaultCellStyle.BackColor = Color.LavenderBlush
                    .Columns("ACCOUNTS").DefaultCellStyle.BackColor = Color.LavenderBlush
                    .Columns("LEDGER").DefaultCellStyle.BackColor = Color.LavenderBlush
                    .Columns("LEDGER1").DefaultCellStyle.BackColor = Color.LavenderBlush
                    .Columns("COLLECTION").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("TRANSACT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("ACCOUNTS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("LEDGER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("SCCLOSED").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("ACCLOSED").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("CLOSEDIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("COLLECTDIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("TRANDIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("CHQRETURN").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("TOTAL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("TRANDATE").HeaderText = "PARTICULARS"
                    .Columns("CLOSEDIFF").HeaderText = "DIFF"
                    .Columns("COLLECTDIFF").HeaderText = "DIFF"
                    .Columns("TRANDIFF").HeaderText = "DIFF"
                    .Columns("COLLECTION").HeaderText = "COLLECT"
                    .Columns("TRANSACT").HeaderText = "TRAN"
                    .Columns("SCCLOSED").HeaderText = "SAVINGS"
                    .Columns("ACCLOSED").HeaderText = "ACCOUNTS"
                    .Columns("LEDGER1").HeaderText = "LEDGER"
                    .Columns("CHQRETURN").HeaderText = "CHQRET"
                    .Columns("ACCODE").Visible = False
                    .Columns("NAME").Visible = False
                    .Columns("TRANDATE1").Visible = False
                    .Columns("RESULT").Visible = False
                    .Columns("KEYNO").Visible = False
                    .Columns("TRANDATE").Frozen = True
                    'Dim dv As New DataView
                    'Dim dt As New DataTable
                    'dv = CType(gridView.DataSource, DataTable).Copy.DefaultView
                    'dv.RowFilter = "RESULT IN(0,2,6)"
                    'dt = dv.ToTable
                    'Dim SNO As Integer
                    'For i As Integer = 0 To dt.Rows.Count - 1
                    '    SNO = Val(dt.Rows(i).Item("KEYNO").ToString)
                    '    With gridView.Rows(SNO)
                    '        If .Cells("RESULT").Value = 0 Then
                    '            .Cells("TRANDATE").Style.BackColor = Color.LightGreen
                    '            .Cells("TRANDATE").Style.ForeColor = Color.Red
                    '            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    '        ElseIf .Cells("RESULT").Value = 2 Or .Cells("RESULT").Value = 6 Then
                    '            .Cells("TRANDATE").Style.BackColor = Color.White
                    '            .DefaultCellStyle.ForeColor = Color.Blue
                    '            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    '        End If
                    '    End With
                    'Next
                    For i As Integer = 0 To .Columns.Count - 1
                        .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
                        .Columns(i).ReadOnly = True
                    Next
                    gridViewHeader.Enabled = False
                    Dim dtGridViewHeader As New DataTable
                    dtGridViewHeader.Columns.Add("PARTICULARS")
                    dtGridViewHeader.Columns.Add("COLLECTION")
                    dtGridViewHeader.Columns.Add("CLOSING")
                    dtGridViewHeader.Columns.Add("SCROLL")
                    With gridViewHeader
                        .DataSource = dtGridViewHeader
                        .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .Columns("PARTICULARS").Width = gridView.Columns("TRANDATE").Width
                        .Columns("COLLECTION").Width = gridView.Columns("ACCOUNTS").Width + gridView.Columns("COLLECTION").Width + gridView.Columns("TRANSACT").Width + gridView.Columns("COLLECTDIFF").Width + gridView.Columns("TRANDIFF").Width + gridView.Columns("LEDGER").Width
                        .Columns("CLOSING").Width = gridView.Columns("SCCLOSED").Width + gridView.Columns("ACCLOSED").Width + gridView.Columns("CHQRETURN").Width + gridView.Columns("TOTAL").Width + gridView.Columns("CLOSEDIFF").Width + gridView.Columns("LEDGER1").Width
                        .Columns("PARTICULARS").HeaderText = ""
                        .Columns("COLLECTION").HeaderText = "COLLECTION"
                        .Columns("CLOSING").HeaderText = "CLOSED"
                        .Columns("SCROLL").HeaderText = ""
                        .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                        .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                        .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
                    End With
                End With
            End If
        End If
    End Sub
    Private Sub DetailView(ByVal TranDate As String)
        strSql = " /** COLLECT VS ACCTRAN **/"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "DATACHECK','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DATACHECK"
        strSql += vbCrLf + " SELECT COSTID,ACNAME TRANDATE,"
        strSql += vbCrLf + " TRANDATE TRANDATE1,ACNAME,ACCODE,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),COLLECTION))COLLECTION,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),ACCOUNTS))ACCOUNTS ,"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),COLLECTION))-"
        strSql += vbCrLf + " SUM(CONVERT(NUMERIC(20,2),ACCOUNTS))DIFF,2 AS RESULT"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK FROM("
        strSql += vbCrLf + " SELECT COSTID,RDATE AS TRANDATE ,AC.ACNAME ,AC.ACCODE,SUM(AMOUNT) COLLECTION,0 ACCOUNTS"
        strSql += vbCrLf + " FROM " & ChitDb & "..SCHEMECOLLECT SC"
        strSql += vbCrLf + " JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE =SC.ACCODE "
        strSql += vbCrLf + " WHERE RDATE = '" & TranDate & "' "
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        strSql += vbCrLf + " AND ISNULL(CHEQUERETDATE,'')=''"
        strSql += vbCrLf + " GROUP BY AC.ACNAME ,AC.ACCODE,RDATE,COSTID"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COSTID,TRANDATE,AC.ACNAME,AC.ACCODE,0 COLLECTION, SUM(AMOUNT) ACCOUNTS"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN SC"
        strSql += vbCrLf + " JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE =SC.ACCODE "
        strSql += vbCrLf + " WHERE TRANDATE = '" & TranDate & "' "
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        strSql += vbCrLf + " AND TRANNO =9999 AND TRANMODE ='D' AND FROMFLAG ='C'"
        strSql += vbCrLf + " GROUP BY AC.ACNAME,AC.ACCODE,TRANDATE ,COSTID )X"
        strSql += vbCrLf + " GROUP BY ACNAME,ACCODE,TRANDATE,COSTID   ORDER BY ACNAME "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK(COSTID,TRANDATE,TRANDATE1,RESULT,ACCODE)"
        strSql += vbCrLf + " SELECT DISTINCT CC.COSTID,COSTNAME ,TRANDATE1,0,'' FROM "
        strSql += vbCrLf + " TEMPTABLEDB..TEMP" & systemId & "DATACHECK T1"
        strSql += vbCrLf + " JOIN " & cnAdminDb & "..COSTCENTRE CC ON CC.COSTID=T1.COSTID "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        'strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK(COSTID,TRANDATE,TRANDATE1,RESULT,ACCODE)"
        'strSql += vbCrLf + " SELECT DISTINCT COSTID,CONVERT(VARCHAR(15),TRANDATE1,105),TRANDATE1,1,'' FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK "
        'cmd = New OleDbCommand(strSql, cn)
        'cmd.ExecuteNonQuery()
        strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DATACHECK(TRANDATE,TRANDATE1,ACCODE,COLLECTION,ACCOUNTS,DIFF,RESULT,COSTID)"
        strSql += vbCrLf + " SELECT 'TOTAL',TRANDATE1,'',SUM(COLLECTION),SUM(ACCOUNTS),SUM(DIFF),3,COSTID  "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK GROUP BY TRANDATE1,COSTID"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = "UPDATE TEMPTABLEDB..TEMP" & systemId & "DATACHECK SET DIFF=NULL WHERE DIFF=0"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "DATACHECK ORDER BY TRANDATE1,COSTID,RESULT"
        Dim dtGridView As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridView)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Me.Cursor = Cursors.Default
            Exit Sub
        End If
        With gridView
            .DataSource = Nothing
            .DataSource = dtGridView
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            .Columns("COLLECTION").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ACCOUNTS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TRANDATE").HeaderText = "PARTICULARS"
            .Columns("ACCODE").Visible = False
            .Columns("ACNAME").Visible = False
            .Columns("COSTID").Visible = False
            .Columns("TRANDATE1").Visible = False
            .Columns("RESULT").Visible = False
            For i As Integer = 0 To gridView.Rows.Count - 1
                With gridView.Rows(i)
                    If .Cells("RESULT").Value = 0 Then
                        .DefaultCellStyle.BackColor = Color.LightGreen
                        .DefaultCellStyle.ForeColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    ElseIf .Cells("RESULT").Value = 3 Then
                        .DefaultCellStyle.BackColor = Color.White
                        .DefaultCellStyle.ForeColor = Color.Blue
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If
                End With
            Next
            gridViewHeader.Columns("PARTICULARS").Width = gridView.Columns("TRANDATE").Width + gridView.Columns("ACCOUNTS").Width + gridView.Columns("COLLECTION").Width + gridView.Columns("DIFF").Width
        End With
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub

    Private Sub gridView_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles gridView.MouseClick
        With gridView
            If .Columns(.CurrentCell.ColumnIndex).Name = "COLLECTDIFF" Or .Columns(.CurrentCell.ColumnIndex).Name = "TRANDIFF" Or .Columns(.CurrentCell.ColumnIndex).Name = "CLOSEDIFF" Then
                btnDetail.Enabled = True
                If .Columns(.CurrentCell.ColumnIndex).Name = "COLLECTDIFF" Then
                    btnAdjust.Enabled = True
                Else
                    btnAdjust.Enabled = False
                End If
            Else
                btnDetail.Enabled = False
            End If
        End With
    End Sub

    Private Sub gridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        If Not gridView.RowCount > 0 Then Exit Sub
        If Not Me.IssRec = Type.Collect Then
            lblStatus.Text = gridView.Rows(e.RowIndex).Cells("CATNAME").Value.ToString + "  [BATCHNO:" + gridView.Rows(e.RowIndex).Cells("BATCHNO").Value.ToString + "]"
        Else
            If gridView.Columns.Contains("ACCODE") Then
                lblStatus.Text = gridView.Rows(e.RowIndex).Cells("NAME").Value.ToString + "  [ACCODE:" + gridView.Rows(e.RowIndex).Cells("ACCODE").Value.ToString + "]"
            Else
                lblStatus.Text = ""
            End If
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDetail.Click
        If Not gridView.RowCount > 0 Then Exit Sub
        If Me.IssRec = Type.Collect Then
            If FlagDetailView = False Then
                With gridView
                    If .CurrentRow.Cells("RESULT").Value <> 0 Then
                        'If IIf(IsDBNull(.CurrentRow.Cells("COLLECTDIFF").Value), 0, .CurrentRow.Cells("COLLECTDIFF").Value) = 0 And IIf(IsDBNull(.CurrentRow.Cells("CLOSEDIFF").Value), 0, .CurrentRow.Cells("CLOSEDIFF").Value) = 0 Then Exit Sub
                        If .Columns(.CurrentCell.ColumnIndex).Name = "COLLECTDIFF" Then
                            CheckCollectDetail(Format(.CurrentRow.Cells("TRANDATE1").Value, "yyyy-MM-dd"), Format(.CurrentRow.Cells("TRANDATE1").Value, "yyyy-MM-dd"), .CurrentRow.Cells("ACCODE").Value.ToString)
                        ElseIf .Columns(.CurrentCell.ColumnIndex).Name = "TRANDIFF" Then
                            CheckTranDetail(Format(.CurrentRow.Cells("TRANDATE1").Value, "yyyy-MM-dd"), Format(.CurrentRow.Cells("TRANDATE1").Value, "yyyy-MM-dd"), .CurrentRow.Cells("ACCODE").Value.ToString)
                        ElseIf .Columns(.CurrentCell.ColumnIndex).Name = "CLOSEDIFF" Then
                            CheckClosedDetail(Format(.CurrentRow.Cells("TRANDATE1").Value, "yyyy-MM-dd"), Format(.CurrentRow.Cells("TRANDATE1").Value, "yyyy-MM-dd"), .CurrentRow.Cells("ACCODE").Value.ToString)
                            btnUpdate.Enabled = True
                        End If
                        FlagDetailView = True
                        gridView.Focus()
                    End If
                End With
            End If
        End If
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If MessageBox.Show("Sure want to Update.?", "Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Cancel Then Exit Sub
        If FlagDetailView Then
            Dim dv As New DataView
            Dim dt, dtAcc As New DataTable
            Dim SAmt, AAmt As Double
            Dim Grp, Regno, CCostId As String
            dv = CType(gridView.DataSource, DataTable).Copy.DefaultView
            dv.RowFilter = "RESULT =1 AND DIFF<>0"
            dt = dv.ToTable
            Try
                For Cnt As Integer = 0 To dt.Rows.Count - 1
                    tran = cn.BeginTransaction
                    SAmt = Val(dt.Rows(Cnt).Item("COLLECT").ToString)
                    AAmt = Val(dt.Rows(Cnt).Item("ACCOUNTS").ToString)
                    Grp = dt.Rows(Cnt).Item("GROUPCODE").ToString
                    Regno = dt.Rows(Cnt).Item("REGNO").ToString
                    strSql = "SELECT COSTID,TRANNO,TRANDATE,BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE CHQCARDREF='" & Grp & "' AND CHQCARDNO='" & Regno & "' AND ISNULL(CANCEL,'')='' ORDER BY TRANDATE"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    da = New OleDbDataAdapter(cmd)
                    dtAcc = New DataTable
                    da.Fill(dtAcc)
                    If dtAcc.Rows.Count > 0 Then
                        If AAmt > SAmt And SAmt = 0 Then 'DOCLOSE UPDATE IN SCHEMEMAST
                            strSql = "UPDATE " & Mid(ChitDb, 1, 3) & "SAVINGS..SCHEMEMAST SET "
                            strSql += " DOCLOSE='" & dtAcc.Rows(0).Item("TRANDATE") & "'"
                            strSql += ",BILLNO=" & dtAcc.Rows(0).Item("TRANNO")
                            strSql += ",CLOSECOSTID='" & dtAcc.Rows(0).Item("COSTID") & "'"
                            strSql += ",CLOSETYPE='A'"
                            strSql += " WHERE GROUPCODE='" & Grp & "' AND REGNO='" & Regno & "' AND DOCLOSE IS NULL"
                            ExecQuery(SyncMode.Master, strSql, cn, tran)
                        ElseIf AAmt <> 0 And SAmt <> 0 And SAmt > AAmt Then 'UPDATE PRINCIPAL AMOUNT IN ACCTRAN WITH BONUS
                            Dim Diff As Double
                            Dim SNO As String
                            Diff = SAmt - AAmt
                            strSql = "SELECT SNO,COSTID FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & dtAcc.Rows(0).Item("BATCHNO") & "' AND AMOUNT=" & AAmt & "AND PAYMODE='SS'"
                            CCostId = objGPack.GetSqlValue(strSql, "COSTID", "", tran)
                            SNO = objGPack.GetSqlValue(strSql, "SNO", "", tran)
                            strSql = "UPDATE " & cnStockDb & "..ACCTRAN SET "
                            strSql += " AMOUNT=" & SAmt
                            strSql += " WHERE BATCHNO='" & dtAcc.Rows(0).Item("BATCHNO") & "'"
                            strSql += " AND PAYMODE='SS'"
                            strSql += " AND AMOUNT=" & AAmt
                            strSql += " AND SNO='" & SNO & "'"
                            strSql += " AND CHQCARDREF='" & Grp & "' AND CHQCARDNO='" & Regno & "' AND ISNULL(CANCEL,'')=''"
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, CCostId)

                            strSql = "SELECT SNO FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & dtAcc.Rows(0).Item("BATCHNO") & "' AND PAYMODE='CB'"
                            strSql += " AND CHQCARDREF='" & Grp & "' AND CHQCARDNO='" & Regno & "' AND ISNULL(CANCEL,'')=''"
                            SNO = objGPack.GetSqlValue(strSql, "SNO", "", tran)
                            If SNO <> "" Then
                                strSql = "UPDATE " & cnStockDb & "..ACCTRAN SET "
                                strSql += " AMOUNT=AMOUNT-" & Diff
                                strSql += " WHERE BATCHNO='" & dtAcc.Rows(0).Item("BATCHNO") & "'"
                                strSql += " AND PAYMODE='CB'"
                                strSql += " AND SNO='" & SNO & "'"
                                strSql += " AND ISNULL(CANCEL,'')=''"
                                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CCostId)
                            Else
                                strSql = "SELECT * FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & dtAcc.Rows(0).Item("BATCHNO") & "' AND PAYMODE='SS'"
                                Dim dtAccTran As New DataTable
                                cmd = New OleDbCommand(strSql, cn, tran)
                                da = New OleDbDataAdapter(cmd)
                                da.Fill(dtAccTran)
                                If dtAccTran.Rows.Count > 0 Then
                                    With dtAccTran
                                        strSql = "SELECT DEDUCTAC FROM " & cnAdminDb & "..CREDITCARD WHERE ACCODE='" & .Rows(0).Item("ACCODE") & "'"
                                        Dim DeductAc As String = objGPack.GetSqlValue(strSql, "DEDUCTAC", "", tran)
                                        InsertIntoAccTran(.Rows(0).Item("TRANNO"), .Rows(0).Item("TRANDATE"), "C", DeductAc, Diff, "CD", .Rows(0).Item("CONTRA"), "P", .Rows(0).Item("COSTID"), .Rows(0).Item("BATCHNO"), CCostId, , , Regno, , , Grp, , , , , , .Rows(0).Item("CASHID"))
                                    End With
                                End If
                            End If
                            strSql = "SELECT SUM(CASE WHEN TRANMODE ='D' THEN AMOUNT ELSE -1*AMOUNT END )AMOUNT "
                            strSql += " FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & dtAcc.Rows(0).Item("BATCHNO") & "'"
                            If Val(objGPack.GetSqlValue(strSql, "AMOUNT", "0", tran)) = 0 Then
                                tran.Commit()
                                tran = Nothing
                            Else
                                tran.Rollback()
                                tran = Nothing
                            End If
                        ElseIf AAmt <> 0 And SAmt <> 0 And SAmt < AAmt Then 'UPDATE PRINCIPAL AMOUNT IN ACCTRAN WITH BONUS
                            Dim Diff As Double
                            Dim SNO As String
                            Diff = AAmt - SAmt
                            strSql = "SELECT SNO,COSTID FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & dtAcc.Rows(0).Item("BATCHNO") & "' AND AMOUNT=" & AAmt & "AND PAYMODE='SS'"
                            CCostId = objGPack.GetSqlValue(strSql, "COSTID", "", tran)
                            SNO = objGPack.GetSqlValue(strSql, "SNO", "", tran)
                            strSql = "UPDATE " & cnStockDb & "..ACCTRAN SET "
                            strSql += " AMOUNT=" & SAmt
                            strSql += " WHERE BATCHNO='" & dtAcc.Rows(0).Item("BATCHNO") & "'"
                            strSql += " AND PAYMODE='SS'"
                            strSql += " AND AMOUNT=" & AAmt
                            strSql += " AND SNO='" & SNO & "'"
                            strSql += " AND CHQCARDREF='" & Grp & "' AND CHQCARDNO='" & Regno & "' AND ISNULL(CANCEL,'')=''"
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, CCostId)

                            strSql = "SELECT SNO FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & dtAcc.Rows(0).Item("BATCHNO") & "' AND PAYMODE='CB'"
                            strSql += " AND CHQCARDREF='" & Grp & "' AND CHQCARDNO='" & Regno & "' AND ISNULL(CANCEL,'')=''"
                            SNO = objGPack.GetSqlValue(strSql, "SNO", "", tran)
                            If SNO <> "" Then
                                strSql = "UPDATE " & cnStockDb & "..ACCTRAN SET "
                                strSql += " AMOUNT=AMOUNT+" & Diff
                                strSql += " WHERE BATCHNO='" & dtAcc.Rows(0).Item("BATCHNO") & "'"
                                strSql += " AND PAYMODE='CB'"
                                strSql += " AND SNO='" & SNO & "'"
                                strSql += " AND ISNULL(CANCEL,'')=''"
                                ExecQuery(SyncMode.Transaction, strSql, cn, tran, CCostId)
                            Else
                                strSql = "SELECT * FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & dtAcc.Rows(0).Item("BATCHNO") & "' AND PAYMODE='SS'"
                                Dim dtAccTran As New DataTable
                                cmd = New OleDbCommand(strSql, cn, tran)
                                da = New OleDbDataAdapter(cmd)
                                da.Fill(dtAccTran)
                                If dtAccTran.Rows.Count > 0 Then
                                    With dtAccTran
                                        strSql = "SELECT BONUSAC FROM " & cnAdminDb & "..CREDITCARD WHERE ACCODE='" & .Rows(0).Item("ACCODE") & "'"
                                        Dim BonusAc As String = objGPack.GetSqlValue(strSql, "BONUSAC", "", tran)
                                        InsertIntoAccTran(.Rows(0).Item("TRANNO"), .Rows(0).Item("TRANDATE"), "D", BonusAc, Diff, "CB", .Rows(0).Item("CONTRA"), "P", .Rows(0).Item("COSTID"), .Rows(0).Item("BATCHNO"), CCostId, , , Regno, , , Grp, , , , , , .Rows(0).Item("CASHID"))
                                    End With
                                End If
                            End If
                            strSql = "SELECT SUM(CASE WHEN TRANMODE ='D' THEN AMOUNT ELSE -1*AMOUNT END )AMOUNT "
                            strSql += " FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & dtAcc.Rows(0).Item("BATCHNO") & "'"
                            If Val(objGPack.GetSqlValue(strSql, "AMOUNT", "0", tran)) = 0 Then
                                tran.Commit()
                                tran = Nothing
                            Else
                                tran.Rollback()
                                tran = Nothing
                            End If
                        End If
                    End If
                    If Not tran Is Nothing Then
                        tran.Commit()
                        tran = Nothing
                    End If
                Next
                MsgBox("Updated.")
            Catch ex As Exception
                If Not tran Is Nothing Then
                    tran.Rollback()
                    tran = Nothing
                End If
                MsgBox(ex.Message + ex.StackTrace)
            End Try
        End If
    End Sub

    Private Sub btnAdjust_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdjust.Click
        Try
            If MessageBox.Show("Sure want to Pass Adjustmnet Entry in Accounts.?", "Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Cancel Then Exit Sub
            Me.Cursor = Cursors.WaitCursor
            Dim dv As New DataView
            Dim dt, dtAcc As New DataTable
            dv = CType(gridView.DataSource, DataTable).Copy.DefaultView
            dv.RowFilter = "RESULT =1 AND COLLECTDIFF<>0 AND TRANDATE<>'2059-01-01' "
            dt = dv.ToTable
            For Cnt As Integer = 0 To dt.Rows.Count - 1
                Dim Amt1, Amt2 As Decimal
                Dim Acc1, Acc2 As String
                Dim Trandate As Date = CType(dt.Rows(Cnt).Item("TRANDATE1").ToString, Date)
                'Dim Trandate1 As Date = CType(dt.Rows(Cnt + 1).Item("TRANDATE1").ToString, Date)
                'If Trandate <> Trandate1 Then
                'Continue For
                'End If
                Amt1 = Val(dt.Rows(Cnt).Item("COLLECTDIFF").ToString)
                'Amt2 = Val(dt.Rows(Cnt + 1).Item("COLLECTDIFF").ToString)
                Acc1 = dt.Rows(Cnt).Item("ACCODE").ToString
                'Acc2 = dt.Rows(Cnt + 1).Item("ACCODE").ToString
                'If Math.Abs(Amt1) = Math.Abs(Amt2) And Acc1 <> "CHEQUE RETURN" And Acc2 <> "CHEQUE RETURN" Then
                If Acc1 <> "CHEQUE RETURN" Then
                    tran = cn.BeginTransaction
                    Batchno = GetNewBatchno(cnCostId, CType(dt.Rows(Cnt).Item("TRANDATE1").ToString, Date), tran)
                    If Amt1 > 0 Then
                        InsertIntoAccTran(9999, dt.Rows(Cnt).Item("TRANDATE1").ToString, "C", Acc1, Math.Abs(Amt1), "CT", "CASH", "C", cnCostId, Batchno, cnCostId, , , , , , , "ADJUSTMENT")
                        InsertIntoAccTran(9999, dt.Rows(Cnt).Item("TRANDATE1").ToString, "D", "CASH", Math.Abs(Amt1), "CT", Acc1, "C", cnCostId, Batchno, cnCostId, , , , , , , "ADJUSTMENT")
                    Else
                        InsertIntoAccTran(9999, dt.Rows(Cnt).Item("TRANDATE1").ToString, "D", Acc1, Math.Abs(Amt1), "CT", "CASH", "C", cnCostId, Batchno, cnCostId, , , , , , , "ADJUSTMENT")
                        InsertIntoAccTran(9999, dt.Rows(Cnt).Item("TRANDATE1").ToString, "C", "CASH", Math.Abs(Amt1), "CT", Acc1, "C", cnCostId, Batchno, cnCostId, , , , , , "ADJUSTMENT")
                    End If
                    strSql = "SELECT SUM(CASE WHEN TRANMODE ='D' THEN AMOUNT ELSE -1*AMOUNT END )AMOUNT "
                    strSql += " FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO='" & Batchno & "'"
                    If Val(objGPack.GetSqlValue(strSql, "AMOUNT", "0", tran)) = 0 Then
                        tran.Commit()
                        tran = Nothing
                    Else
                        tran.Rollback()
                        tran = Nothing
                    End If
                End If
            Next
            MsgBox("Completed.")
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            If Not tran Is Nothing Then
                tran.Rollback()
                tran = Nothing
            End If
            MsgBox(ex.Message + ex.StackTrace)
        End Try
    End Sub
    Private Sub InsertIntoAccTran _
  (ByVal tNo As Integer, _
  ByVal tranDate As String, _
  ByVal tranMode As String, _
  ByVal accode As String, _
  ByVal amount As Double, _
  ByVal payMode As String, _
  ByVal contra As String, _
  ByVal FromFlag As String, _
  ByVal CostId As String, _
  ByVal Batchno As String, _
  ByVal CCostId As String, _
  Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing, _
  Optional ByVal chqCardNo As String = Nothing, _
  Optional ByVal chqDate As String = Nothing, _
  Optional ByVal chqCardId As Integer = Nothing, _
  Optional ByVal chqCardRef As String = Nothing, _
  Optional ByVal Remark1 As String = Nothing, _
  Optional ByVal Remark2 As String = Nothing, _
  Optional ByVal fLAG As String = Nothing, _
  Optional ByVal SAccode As String = "", _
  Optional ByVal SCostid As String = "", _
  Optional ByVal CashId As String = "" _
  )
        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,SACCODE"
        strSql += " ,AMOUNT,REFNO,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,BRSFLAG,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED,UPTIME,SYSTEMID,CASHID,COSTID,SCOSTID"
        strSql += " ,APPVER,COMPANYID,FLAG"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & tranDate & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ,'" & SAccode & "'" 'ACCODE
        strSql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strSql += " ,'" & refNo & "'" 'REFNO
        strSql += " ,'" & payMode & "'" 'PAYMODE
        strSql += " ,'" & chqCardNo & "'" 'CHQCARDNO
        strSql += " ," & chqCardId & "" 'CARDID
        strSql += " ,'" & chqCardRef & "'" 'CHQCARDREF
        strSql += " ,''" 'BRSFLAG
        strSql += " ,NULL" 'RELIASEDATE
        strSql += " ,'" & FromFlag & "'" 'FROMFLAG
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK2
        strSql += " ,'" & contra & "'" 'CONTRA
        strSql += " ,'" & Batchno & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'" & Cashid & "'" 'CASHID
        strSql += " ,'" & CostId & "'" 'COSTID
        strSql += " ,'" & SCostid & "'" 'FLAG
        strSql += " ,'ACCTALLY'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & fLAG & "'" 'FLAG
        strSql += " )"
        'ExecQuery(SyncMode.Transaction, strSql, cn, tran, CCostId)
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        strSql = ""
        cmd = Nothing
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If gridViewHeader Is Nothing Then Exit Sub
        If Not gridViewHeader.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHeader.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHeader.HorizontalScrollingOffset = e.NewValue
                gridViewHeader.Columns("SCROLL").Visible = CType(gridViewHeader.Controls(0), HScrollBar).Visible
                gridViewHeader.Columns("SCROLL").Width = CType(gridViewHeader.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "SAVINGS VS ACCOUNTS DATA RECONZILATION DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "SAVINGS VS ACCOUNTS DATA RECONZILATION DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
End Class