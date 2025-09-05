Imports System.Data.OleDb
Imports System.Globalization
Imports System.IO
Public Class LotManualIssue
    Dim ukCultureInfo As New CultureInfo("en-GB")
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagUpdate As Boolean = False
    Dim rateFlag As Boolean = False

    Dim itemDt As New DataTable

    Dim tran As OleDbTransaction = Nothing

    Dim TagNoGen As String = Nothing
    Dim TagNoFrom As String = Nothing
    Dim lastTagNo As String
    Dim startTagNo As String
    Dim endTagNo As String
    Dim currentTagNo As String
    Dim locUpdateRow As Integer = -1

    Dim tagMade As Boolean = False
    Dim cPcs As Integer = -1
    Dim cWeight As Double = -1
    Dim updSno As String = Nothing
    Dim updLotNo As Integer = -1
    Dim recSno As String = ""
    Dim PrintLot As Boolean = IIf(GetAdmindbSoftValue("PRINT_LOT", "N") = "Y", True, False)
    Dim HideAccLink As Boolean = IIf(GetAdmindbSoftValue("HIDE_ACHARI_ACCLINK", "Y") = "Y", True, False)
    Dim _JobNoEnable As Boolean = IIf(GetAdmindbSoftValue("MRMIJOBNO", "N") = "Y", True, False)
    Dim LotPcsChk As Boolean = IIf(GetAdmindbSoftValue("CHK_LOTPCS", "N") = "Y", True, False)
    Dim LinkedSrv As String = Nothing
    Dim LinkedDb As String = Nothing
    Dim STOCKVALIDATION As Boolean = IIf(GetAdmindbSoftValue("MRMISTOCKLOCK", "N") = "Y", True, False)
    Dim _AcnameEnable As Boolean = IIf(GetAdmindbSoftValue("LOTISS_ACNAME_BASED", "N") = "Y", True, False)
    Dim InclCusttype As String = GetAdmindbSoftValue("INCL_CUSTOMER_ISSREC", "N")

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
    End Sub

    Function funcNew() As Integer
        objGPack.TextClear(Me)


        cmbEntryType.Enabled = True
        cmbSubItemName_Man.Text = ""
        cmbSubItemName_Man.Items.Clear()
        cmbSubItemName_Man.Enabled = False

        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbSupplier_Man)
        chkMerge.Checked = False
        cmbOpenDesignerName.Items.Clear()
        cmbOpenDesignerName.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbOpenDesignerName, False)
        cmbOpenDesignerName.Text = "ALL"

        cmbCostCentre_Man.Text = ""
        If cmbCostCentre_Man.Enabled = True Then
            funcCostCentre()
        End If
        cmbItemCounter_MAN.Text = ""
        If cmbItemCounter_MAN.Enabled = True Then
            Dim dt As New DataTable
            dt.Clear()
            strSql = " select ItemCtrName from " & cnAdminDb & "..ItemCounter WHERE ACTIVE = 'Y' order by displayorder,ItemCtrName"
            objGPack.FillCombo(strSql, cmbItemCounter_MAN)
            cmbItemCounter_MAN.Enabled = True
            cmbItemCounter_MAN.Text = ""
        Else
            cmbItemCounter_MAN.Enabled = False
            cmbItemCounter_MAN.Text = ""
        End If

        txtCut.Text = ""
        txtColor.Text = ""
        txtClarity.Text = ""
        txtSetType.Text = ""
        txtSetType.Text = ""
        txtHt.Text = ""
        txtWdh.Text = ""

        CmbHallmark.Items.Clear()
        CmbHallmark.Items.Add("YES")
        CmbHallmark.Items.Add("NO")
        CmbHallmark.Text = "NO"

        funcDefauleValues()
        dtpDate.Value = GetEntryDate(BrighttechPack.GlobalMethods.GetServerDate(cn))
        dtpTranDate.Value = GetEntryDate(BrighttechPack.GlobalMethods.GetServerDate(cn))
        'pnlPurchaseEntry.Enabled = True
        grpPending.Visible = False
        lblPendingPcs.Text = ""
        lblPendingWeight.Text = ""
        btnSave.Enabled = False
        btnAdd.Enabled = False
        If _JobNoEnable Then
            txtJobno.Select()
        ElseIf _AcnameEnable Then
            cmbAcName.Select()
        Else
            dtpTranDate.Select()
        End If

    End Function
    Function funcOpen() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        tabMain.SelectedTab = tabView
        txtOpenLotNo.Clear()
        txtOpenItemName.Clear()
        dtpFrom.Value = BrighttechPack.GlobalMethods.GetServerDate(cn)
        dtpTo.Value = BrighttechPack.GlobalMethods.GetServerDate(cn)
        txtOpenLotNo.Focus()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If Not CheckDate(dtpDate.Value) Then Exit Function
        If CheckEntryDate(dtpDate.Value) Then Exit Function

        If flagUpdate = False Then
            funcAdd()
        Else
            'funcUpdate()
        End If
        Return 0
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcAdd() As Integer
        Dim DesignerId As String = Nothing
        Dim COSTID As String = Nothing
        Dim itemId As Integer = Nothing
        Dim stockType As String = Nothing
        Dim subItemId As Integer = Nothing
        Dim LotNo As Integer
        If Not itemDt.Rows.Count > 0 Then
            MsgBox("No Record Added", MsgBoxStyle.Exclamation)
            Me.SelectNextControl(cmbEntryType, True, True, True, True)
            Exit Function
        End If
        Try

            If itemDt.Columns.Contains("Decpieces") = False Then
                Dim col As DataColumn = New DataColumn("Decpieces", GetType(System.Decimal))
                col.Expression = "Convert([PIECES], 'System.Int32')"
                itemDt.Columns.Add(col)
            End If
            If itemDt.Columns.Contains("Decgwt") = False Then
                Dim col1 As DataColumn = New DataColumn("Decgwt", GetType(System.Decimal))
                col1.Expression = "Convert([GRSWT], 'System.Decimal')"
                itemDt.Columns.Add(col1)
            End If
            If itemDt.Columns.Contains("Decnwt") = False Then
                Dim col1 As DataColumn = New DataColumn("Decnwt", GetType(System.Decimal))
                col1.Expression = "Convert([NETWT], 'System.Decimal')"
                itemDt.Columns.Add(col1)
            End If
            If itemDt.Columns.Contains("Decstnwt") = False Then
                Dim col1 As DataColumn = New DataColumn("Decstnwt", GetType(System.Decimal))
                col1.Expression = "Convert([STNWT], 'System.Decimal')"
                itemDt.Columns.Add(col1)
            End If
            If itemDt.Columns.Contains("Decdiawt") = False Then
                Dim col1 As DataColumn = New DataColumn("Decdiawt", GetType(System.Decimal))
                col1.Expression = "Convert([DIAWT], 'System.Decimal')"
                itemDt.Columns.Add(col1)
            End If

            If itemDt.Columns.Contains("Decstnpcs") = False Then
                Dim col2 As DataColumn = New DataColumn("Decstnpcs", GetType(System.Decimal))
                col2.Expression = "Convert([STNPCS], 'System.Decimal')"
                itemDt.Columns.Add(col2)
            End If
            If itemDt.Columns.Contains("Decdiapcs") = False Then
                Dim col3 As DataColumn = New DataColumn("Decdiapcs", GetType(System.Decimal))
                col3.Expression = "Convert([DIAPCS], 'System.Decimal')"
                itemDt.Columns.Add(col3)
            End If

            tran = Nothing
            tran = cn.BeginTransaction()
            'Getting LotNo
GENLOTNO:
            strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LOTNO'"
            LotNo = Val(objGPack.GetSqlValue(strSql, , , tran))
            strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & LotNo + 1 & "' "
            strSql += " WHERE CTLID = 'LOTNO' AND CTLTEXT = " & LotNo & ""
            cmd = New OleDbCommand(strSql, cn, tran)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GENLOTNO
            End If
            LotNo += 1
            For cnt As Integer = 0 To itemDt.Rows.Count - 1
                Dim lotSno As String = GetNewSno(TranSnoType.ITEMLOTCODE, tran, "GET_ADMINSNO_TRAN") ' GetNewSno(CNSTOCKDB, TranSnoType.ITEMLOTCODE, tran, "GET_SNO_ADMIN") ' GetWSno(TranSnoType.ITEMLOTCODE, tran, CNSTOCKDB)
                With itemDt.Rows(cnt)
                    ''Find ItemId
                    strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEMNAME") & "'"
                    itemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                    strSql = " SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEMNAME") & "'"
                    stockType = objGPack.GetSqlValue(strSql, , , tran)
                    ''Find DesignerId
                    strSql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & .Item("DESIGNERNAME") & "'"
                    DesignerId = objGPack.GetSqlValue(strSql, , , tran)
                    ''Find COSTID
                    strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & .Item("COSTCENTRE") & "'"
                    COSTID = objGPack.GetSqlValue(strSql, , , tran)
                    ''Find SubItem Id
                    strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST "
                    strSql += " WHERE SUBITEMNAME = '" & .Item("SUBITEMNAME") & "'"
                    subItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                    Dim itemCounterId As Integer = Nothing
                    strSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & .Item("ITEMCTRNAME") & "'"
                    itemCounterId = Val(objGPack.GetSqlValue(strSql, , , tran))

                    Dim entryType As String = Nothing
                    Select Case .Item("ENTRYTYPE").ToString.ToUpper
                        Case "REGULAR"
                            entryType = "R"
                        Case "ORDER"
                            entryType = "OR"
                        Case "REPAIR"
                            entryType = "RE"
                        Case Else 'NONTAG TO TAG
                            entryType = "NT"
                    End Select
                    Dim dbtable As String = cnAdminDb & "..ITEMLOT"
                    If LinkedSrv <> "" And LinkedDb <> "" Then dbtable = "[" & LinkedSrv & "]." & LinkedDb & ".DBO.ITEMLOT"
                    If .Item("RECSNO").ToString = .Item("PRECSNO").ToString And .Item("merged") = "Y" Then
                        Dim exprs As String = "PRECSNO = '" & .Item("RECSNO") & "' AND MERGED='Y'"
                        Dim mergepcs As Decimal = Val("" & itemDt.Compute("sum(DECPIECES)", exprs))
                        Dim mergegwt As Decimal = Val("" & itemDt.Compute("sum(DECGWT)", exprs))

                        Dim mergenwt As Decimal = Val("" & itemDt.Compute("sum(DECNWT)", exprs))
                        Dim mergestnpcs As Decimal = Val("" & itemDt.Compute("sum(DECSTNPCS)", exprs))
                        Dim mergestnwt As Decimal = Val("" & itemDt.Compute("sum(DECSTNWT)", exprs))
                        Dim mergediapcs As Decimal = Val("" & itemDt.Compute("sum(DECDIAPCS)", exprs))
                        Dim mergediawt As Decimal = Val("" & itemDt.Compute("sum(DECDIAWT)", exprs))

                        strSql = " INSERT INTO " & dbtable 'cnAdminDb & "..ITEMLOT"
                        strSql += " ("
                        strSql += " SNO,ENTRYTYPE,LOTDATE,LOTNO,DESIGNERID,TRANINVNO,"
                        strSql += " BILLNO,COSTID,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
                        strSql += " ITEMID,SUBITEMID,PCS,GRSWT,STNPCS,STNWT,STNUNIT,"
                        strSql += " DIAPCS,DIAWT,NETWT,NOOFTAG,RATE,ITEMCTRID,WMCTYPE,TABLECODE,"
                        strSql += " BULKLOT,MULTIPLETAGS,NARRATION,FINERATE,TUCH,"
                        strSql += " WASTPER,MCGRM,OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
                        strSql += " COMPANYID,CPCS,CGRSWT,COMPLETED,CANCEL,"
                        strSql += " ACCESSING,USERID,UPDATED,"
                        strSql += " UPTIME,SYSTEMID,APPVER,"
                        strSql += " CUTID,COLORID,CLARITYID,SETTYPEID,SHAPEID,HEIGHT,WIDTH,STKTYPE,HALLMARK"
                        strSql += " )VALUES("
                        strSql += " '" & lotSno & "'" 'SNO
                        strSql += " ,'" & entryType & "'" 'ENTRYTYPE
                        strSql += " ,'" & GetEntryDate(dtpDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'LOTDATE 'DateTime.Parse(.Item("LOTDATE"), ukCultureInfo.DateTimeFormat).ToString("yyyy-MM-dd")
                        strSql += " ," & LotNo & "" 'LOTNO
                        strSql += " ," & DesignerId & "" 'DESIGNERID
                        strSql += " ,'" & .Item("TRANINVNO") & "'" 'TRANINVNO
                        strSql += " ,'" & .Item("BILLNO") & "'" 'BILLNO
                        strSql += " ,'" & COSTID & "'" 'COSTID
                        strSql += " ," & cnt + 1 & "" 'ENTRYORDER
                        strSql += " ,'" & .Item("ORDREPNO") & "'" 'ORDREPNO
                        strSql += " ,'" & .Item("ORDENTRYORDER") & "'" 'ORDENTRYORDER
                        strSql += " ," & Val(itemId) & "" 'ITEMID
                        strSql += " ," & Val(subItemId) & "" 'SUBITEMID
                        strSql += " ," & Val(.Item("PIECES").ToString) & "" 'PCS

                        strSql += " ," & mergegwt & "" 'GRSWT
                        strSql += " ," & mergestnpcs & "" 'STNPCS
                        strSql += " ," & mergestnwt & "" 'STNWT
                        strSql += " ,'" & Mid(.Item("STNUNIT").ToString, 1, 1) & "'" 'STNUNIT
                        strSql += " ," & mergediapcs & "" 'DIAPCS
                        strSql += " ," & mergediawt & "" 'DIAWT
                        strSql += " ," & mergenwt & "" 'NETWT
                        strSql += " ," & Val(.Item("NOOFTAG").ToString) & "" 'NOOFTAG
                        strSql += " ," & Val(.Item("RATE").ToString) & "" 'RATE
                        strSql += " ," & Val(itemCounterId) & "" 'ITEMCTRID
                        strSql += " ,'" & Mid(.Item("VALUEADDEDTYPE").ToString, 1, 1) & "'" 'WMCTYPE
                        strSql += " ,'" & .Item("TABLECODE").ToString & "'" 'TABLECODE
                        strSql += " ,'" & Mid(.Item("BULKLOT").ToString, 1, 1) & "'" 'BULKLOT
                        strSql += " ,'" & Mid(.Item("MULTIPLETAGS").ToString, 1, 1) & "'" 'MULTIPLETAGS
                        strSql += " ,'" & .Item("NARRATION").ToString & "'" 'NARRATION
                        strSql += " ," & Val(.Item("FINERATE").ToString) & "" 'FINERATE
                        strSql += " ," & Val(.Item("TUCH").ToString) & "" 'TUCH
                        strSql += " ," & Val(.Item("WASTPER").ToString) & "" 'WASTPER
                        strSql += " ," & Val(.Item("MCGRM").ToString) & "" 'MCGRM
                        strSql += " ," & Val(.Item("OTHCHARGE").ToString) & "" 'OTHCHARGE
                        If stockType = "T" Then ''STOCKTYPE ==>TAGED ITEM
                            If TagNoGen = "L" Then
                                funcTagNo(itemId, Val(.Item("PIECES").ToString))
                                strSql += " ,'" & startTagNo & "'" 'STARTTAGNO
                                strSql += " ,'" & endTagNo & "'" 'ENDTAGNO
                                strSql += " ,'" & currentTagNo & "'" 'CURTAGNO
                            Else
                                strSql += " ,''" 'STARTTAGNO
                                strSql += " ,''" 'ENDTAGNO
                                strSql += " ,''" 'CURTAGNO
                            End If
                        Else
                            strSql += " ,''" 'STARTTAGNO
                            strSql += " ,''" 'ENDTAGNO
                            strSql += " ,''" 'CURTAGNO
                        End If
                        strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                        strSql += " ,0" 'CPIECE
                        strSql += " ,0" 'CWEIGHT
                        strSql += " ,''" 'COMPLETED
                        strSql += " ,''" 'CANCEL
                        strSql += " ,''" 'ACCESSING
                        strSql += " ," & userId & "" 'USERID
                        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                        strSql += " ,'" & systemId & "'" 'SYSTEMID
                        strSql += " ,'" & VERSION & "'" 'APPVER
                        strSql += " ,'" & Val(.Item("CUTID").ToString) & "'" 'CUTID
                        strSql += " ,'" & Val(.Item("COLORID").ToString) & "'" 'COLORID
                        strSql += " ,'" & Val(.Item("CLARITYID").ToString) & "'" 'CLARITYID
                        strSql += " ,'" & Val(.Item("SETTYPEID").ToString) & "'" 'SETTYPEID
                        strSql += " ,'" & Val(.Item("SHAPEID").ToString) & "'" 'SHAPEID
                        strSql += " ,'" & Val(.Item("HEIGHT").ToString) & "'" 'HEIGHT
                        strSql += " ,'" & Val(.Item("WIDTH").ToString) & "'" 'WIDTH
                        strSql += " ,'" & .Item("STKTYPE").ToString & "'" 'STKTYPE
                        strSql += " ,'" & .Item("HALLMARK").ToString & "'" 'HALLMARK
                        strSql += " )"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                    Else
                        If .Item("MERGED") <> "Y" Then
                            strSql = " INSERT INTO " & dbtable 'cnAdminDb & "..ITEMLOT"
                            strSql += " ("
                            strSql += " SNO,ENTRYTYPE,LOTDATE,LOTNO,DESIGNERID,TRANINVNO,"
                            strSql += " BILLNO,COSTID,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
                            strSql += " ITEMID,SUBITEMID,PCS,GRSWT,STNPCS,STNWT,STNUNIT,"
                            strSql += " DIAPCS,DIAWT,NETWT,NOOFTAG,RATE,ITEMCTRID,WMCTYPE,TABLECODE,"
                            strSql += " BULKLOT,MULTIPLETAGS,NARRATION,FINERATE,TUCH,"
                            strSql += " WASTPER,MCGRM,OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
                            strSql += " COMPANYID,CPCS,CGRSWT,COMPLETED,CANCEL,"
                            strSql += " ACCESSING,USERID,UPDATED,"
                            strSql += " UPTIME,SYSTEMID,APPVER,"
                            strSql += " CUTID,COLORID,CLARITYID,SETTYPEID,SHAPEID,HEIGHT,WIDTH,STKTYPE,HALLMARK"
                            strSql += " )VALUES("
                            strSql += " '" & lotSno & "'" 'SNO
                            strSql += " ,'" & entryType & "'" 'ENTRYTYPE
                            strSql += " ,'" & GetEntryDate(dtpDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'LOTDATE 'DateTime.Parse(.Item("LOTDATE"), ukCultureInfo.DateTimeFormat).ToString("yyyy-MM-dd")
                            strSql += " ," & LotNo & "" 'LOTNO
                            strSql += " ," & DesignerId & "" 'DESIGNERID
                            strSql += " ,'" & .Item("TRANINVNO") & "'" 'TRANINVNO
                            strSql += " ,'" & .Item("BILLNO") & "'" 'BILLNO
                            strSql += " ,'" & COSTID & "'" 'COSTID
                            strSql += " ," & cnt + 1 & "" 'ENTRYORDER
                            strSql += " ,'" & .Item("ORDREPNO") & "'" 'ORDREPNO
                            strSql += " ,'" & .Item("ORDENTRYORDER") & "'" 'ORDENTRYORDER
                            strSql += " ," & Val(itemId) & "" 'ITEMID
                            strSql += " ," & Val(subItemId) & "" 'SUBITEMID
                            strSql += " ," & Val(.Item("PIECES").ToString) & "" 'PCS
                            strSql += " ," & Val(.Item("GRSWT").ToString) & "" 'GRSWT
                            strSql += " ," & Val(.Item("STNPCS").ToString) & "" 'STNPCS
                            strSql += " ," & Val(.Item("STNWT").ToString) & "" 'STNWT
                            'strSql += " ," & mergepcs & "" 'PCS
                            'strSql += " ," & mergegwt & "" 'GRSWT
                            'strSql += " ," & mergestnpcs & "" 'STNPCS
                            'strSql += " ," & mergestnwt & "" 'STNWT
                            strSql += " ,'" & Mid(.Item("STNUNIT").ToString, 1, 1) & "'" 'STNUNIT
                            'strSql += " ," & mergediapcs & "" 'DIAPCS
                            'strSql += " ," & mergediawt & "" 'DIAWT
                            'strSql += " ," & mergenwt & "" 'NETWT
                            strSql += " ," & Val(.Item("DIAPCS").ToString) & "" 'DIAPCS
                            strSql += " ," & Val(.Item("DIAWT").ToString) & "" 'DIAWT
                            strSql += " ," & Val(.Item("NETWT").ToString) & "" 'NETWT
                            strSql += " ," & Val(.Item("NOOFTAG").ToString) & "" 'NOOFTAG
                            strSql += " ," & Val(.Item("RATE").ToString) & "" 'RATE
                            strSql += " ," & Val(itemCounterId) & "" 'ITEMCTRID
                            strSql += " ,'" & Mid(.Item("VALUEADDEDTYPE").ToString, 1, 1) & "'" 'WMCTYPE
                            strSql += " ,'" & .Item("TABLECODE").ToString & "'" 'TABLECODE
                            strSql += " ,'" & Mid(.Item("BULKLOT").ToString, 1, 1) & "'" 'BULKLOT
                            strSql += " ,'" & Mid(.Item("MULTIPLETAGS").ToString, 1, 1) & "'" 'MULTIPLETAGS
                            strSql += " ,'" & .Item("NARRATION").ToString & "'" 'NARRATION
                            strSql += " ," & Val(.Item("FINERATE").ToString) & "" 'FINERATE
                            strSql += " ," & Val(.Item("TUCH").ToString) & "" 'TUCH
                            strSql += " ," & Val(.Item("WASTPER").ToString) & "" 'WASTPER
                            strSql += " ," & Val(.Item("MCGRM").ToString) & "" 'MCGRM
                            strSql += " ," & Val(.Item("OTHCHARGE").ToString) & "" 'OTHCHARGE
                            If stockType = "T" Then ''STOCKTYPE ==>TAGED ITEM
                                If TagNoGen = "L" Then
                                    funcTagNo(itemId, Val(.Item("PIECES").ToString))
                                    strSql += " ,'" & startTagNo & "'" 'STARTTAGNO
                                    strSql += " ,'" & endTagNo & "'" 'ENDTAGNO
                                    strSql += " ,'" & currentTagNo & "'" 'CURTAGNO
                                Else
                                    strSql += " ,''" 'STARTTAGNO
                                    strSql += " ,''" 'ENDTAGNO
                                    strSql += " ,''" 'CURTAGNO
                                End If
                            Else
                                strSql += " ,''" 'STARTTAGNO
                                strSql += " ,''" 'ENDTAGNO
                                strSql += " ,''" 'CURTAGNO
                            End If
                            strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                            strSql += " ,0" 'CPIECE
                            strSql += " ,0" 'CWEIGHT
                            strSql += " ,''" 'COMPLETED
                            strSql += " ,''" 'CANCEL
                            strSql += " ,''" 'ACCESSING
                            strSql += " ," & userId & "" 'USERID
                            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                            strSql += " ,'" & systemId & "'" 'SYSTEMID
                            strSql += " ,'" & VERSION & "'" 'APPVER
                            strSql += " ,'" & Val(.Item("CUTID").ToString) & "'" 'CUTID
                            strSql += " ,'" & Val(.Item("COLORID").ToString) & "'" 'COLORID
                            strSql += " ,'" & Val(.Item("CLARITYID").ToString) & "'" 'CLARITYID
                            strSql += " ,'" & Val(.Item("SETTYPEID").ToString) & "'" 'SETTYPEID
                            strSql += " ,'" & Val(.Item("SHAPEID").ToString) & "'" 'SHAPEID
                            strSql += " ,'" & Val(.Item("HEIGHT").ToString) & "'" 'HEIGHT
                            strSql += " ,'" & Val(.Item("WIDTH").ToString) & "'" 'WIDTH
                            strSql += " ,'" & .Item("STKTYPE").ToString & "'" 'STKTYPE
                            strSql += " ,'" & .Item("HALLMARK").ToString & "'" 'HALLMARK
                            strSql += " )"
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()
                        End If

                    End If
                End With

                strSql = "  INSERT INTO " & cnStockDb & "..LOTISSUE"
                strSql += "  ("
                strSql += "  TRANNO"
                strSql += "  ,TRANDATE"
                strSql += "  ,GRSWT"
                strSql += "  ,NETWT"
                strSql += "  ,CANCEL"
                strSql += "  ,BATCHNO"
                strSql += "  ,USERID"
                strSql += "  ,UPDATED"
                strSql += "  ,APPVER"
                strSql += "  ,COMPANYID"
                strSql += "  ,PCS"
                strSql += "  ,LOTSNO"
                strSql += "  ,ITEMID"
                strSql += "  ,SUBITEMID"
                strSql += "  ,RECSNO"
                strSql += "  ,STKTYPE"
                strSql += "  )"
                strSql += "  SELECT"
                strSql += "  " & itemDt.Rows(cnt).Item("TRANNO") & ""
                strSql += "  ,'" & itemDt.Rows(cnt).Item("TRANDATE") & "'"
                strSql += "  ," & Val(itemDt.Rows(cnt).Item("GRSWT").ToString) & "" 'GRSWT
                strSql += "  ," & Val(itemDt.Rows(cnt).Item("NETWT").ToString) & "" 'GRSWT
                strSql += "  ,''" 'CANCEL
                strSql += "  ,''" 'BATCHNO
                strSql += "  ," & userId & "" 'USERID
                strSql += "  ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += "  ,'" & VERSION & "'" 'APPVER
                strSql += "  ,'" & GetStockCompId() & "'" 'COMPANYID
                strSql += "  ," & Val(itemDt.Rows(cnt).Item("PIECES").ToString) & "" 'PCS
                strSql += "  ,'" & lotSno & "'" 'LOTSNO
                strSql += "  ," & itemId & "" 'ITEMID
                strSql += "  ," & subItemId & "" 'SUBITEMID
                strSql += "  ,'" & itemDt.Rows(cnt).Item("RECSNO").ToString & "'" 'RECSNO
                strSql += " ,'" & itemDt.Rows(cnt).Item("STKTYPE").ToString & "'" 'STKTYPE
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                If TagNoGen = "L" Then ''FROM ITEMMASTER
                    strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO = '" & endTagNo & "' WHERE ITEMID = '" & itemId & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()

                End If
                ' InsertintoNonTag(itemDt.Rows(cnt))
            Next


            tran.Commit()
            tran = Nothing
            Dim prLotNo As Integer = LotNo
            Dim prLotDate As Date = dtpDate.Value.Date
            MsgBox(LotNo.ToString + " Generated...", MsgBoxStyle.Exclamation)
            itemDt.Rows.Clear()
            funcNew()
            chkMerge.Checked = False
            ''Updation Flds
            flagUpdate = False
            tagMade = False
            cPcs = -1
            cWeight = -1
            If PrintLot Then
                Dim objLotPrint As New CLS_LOTPRINT(prLotNo, prLotDate)
                objLotPrint.Print()
            End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Function


    Function funcGetDetails() As Integer

    End Function
    Function funcDefauleValues() As Integer
        cmbEntryType.Text = "REGULAR"
        cmbBulkLot.Text = "NO"
        cmbMultipleTag.Text = "NO"
        cmbStoneUnit.Text = "CARAT"
        txtDiamondUnit.Text = "CARAT"
        txtDiamondUnit.Enabled = False
        txtDiamondUnit.BackColor = txtPiece_Num_Man.BackColor
        cmbValueAdded.Text = "ITEM"
    End Function
    Function funcCostCentre() As Integer
        cmbCostCentre_Man.Items.Clear()
        strSql = " select CostName from " & cnAdminDb & "..CostCentre order by CostName"
        objGPack.FillCombo(strSql, cmbCostCentre_Man, False)
        cmbCostCentre_Man.Text = ""
    End Function
    Function funcLoadGrid() As Integer
        If objGPack.Validator_Check(Me) Then Exit Function
        Dim Ismerge As Boolean = False
        Dim prow As String = ""
        If Val(txtNetWeight_Wet_MAN.Text) > Val(txtGrossWeight_Wet_MAN.Text) Then
            MsgBox("NetWeight Should not Exceed " + txtGrossWeight_Wet_MAN.Text, MsgBoxStyle.Information)
            Exit Function
        End If
        If cmbBulkLot.Text = "YES" Then
            If Not Val(txtItemRate_Amt.Text) > 0 Then
                MsgBox(Me.GetNextControl(txtItemRate_Amt, False).Text + " Should not Empty", MsgBoxStyle.Information)
                txtItemRate_Amt.Focus()
                Exit Function
            End If
        End If
        If recSno = "" Then
            MsgBox("Invalid Receipt Item", MsgBoxStyle.Information)
            Exit Function
        End If
        Dim row As DataRow = Nothing
        If locUpdateRow <> -1 Then ''updat
            row = itemDt.Rows(locUpdateRow)
        Else
            If chkMerge.Checked = True Then
                If itemDt.Rows.Count > 0 Then Ismerge = True : prow = itemDt.Rows(itemDt.Rows.Count - 1).Item("PRECSNO").ToString
            End If
            row = itemDt.NewRow
        End If
        With row
            .Item("ENTRYTYPE") = cmbEntryType.Text
            .Item("LOTDATE") = dtpDate.Text
            .Item("DESIGNERNAME") = cmbSupplier_Man.Text
            .Item("COSTCENTRE") = cmbCostCentre_Man.Text
            .Item("ITEMNAME") = txtItemName.Text
            .Item("SUBITEMNAME") = cmbSubItemName_Man.Text
            .Item("PIECES") = txtPiece_Num_Man.Text
            If txtGrossWeight_Wet_MAN.Enabled = True Then
                .Item("GRSWT") = Val(txtGrossWeight_Wet_MAN.Text)
            Else
                .Item("GRSWT") = ".000"
            End If
            If grpStone.Enabled = True Then
                .Item("STNPCS") = Val("" & txtStonePieces_Num.Text)
                .Item("STNWT") = Val(txtStoneWeight_Wet.Text)
            Else
                .Item("STNPCS") = "0"
                .Item("STNWT") = ".000"
            End If
            .Item("STNUNIT") = cmbStoneUnit.Text
            If grpDiamond.Enabled = True Then
                .Item("DIAPCS") = Val("" & txtDiamondPieces_Num.Text)
                .Item("DIAWT") = Val(txtDidmondWeight_Wet.Text)
            Else
                .Item("DIAPCS") = "0"
                .Item("DIAWT") = ".0000"
            End If
            If txtNetWeight_Wet_MAN.Enabled = True Then
                .Item("NETWT") = Val(txtNetWeight_Wet_MAN.Text)
            Else
                .Item("NETWT") = ".000"
            End If
            If txtNoOfTags_Num.Enabled = True Then
                .Item("NOOFTAG") = txtNoOfTags_Num.Text
            Else
                .Item("NOOFTAG") = ""
            End If
            .Item("RATE") = txtItemRate_Amt.Text
            .Item("ITEMCTRNAME") = cmbItemCounter_MAN.Text
            If cmbValueAdded.Text = "ITEM" Then
                .Item("VALUEADDEDTYPE") = "I"
            ElseIf cmbValueAdded.Text = "TABLE" Then
                .Item("VALUEADDEDTYPE") = "T"
                .Item("TABLECODE") = cmbTableCode.Text
            ElseIf cmbValueAdded.Text = "DESIGNER" Then
                .Item("VALUEADDEDTYPE") = "D"
            ElseIf cmbValueAdded.Text = "TAG" Then
                .Item("VALUEADDEDTYPE") = "P"
            End If

            If cmbBulkLot.Enabled = True Then
                .Item("BULKLOT") = cmbBulkLot.Text
            Else
                .Item("BULKLOT") = ""
            End If
            If cmbMultipleTag.Enabled = True Then
                .Item("MULTIPLETAGS") = cmbMultipleTag.Text
            Else
                .Item("MULTIPLETAGS") = ""
            End If
            .Item("NARRATION") = cmbNarration_OWN.Text
            If pnlPurchaseEntry.Enabled = True Then
                .Item("FINERATE") = txtFineRate_Amt.Text
                .Item("TUCH") = txtTuch_WET.Text
                .Item("WASTPER") = txtWastage_Per.Text
                .Item("MCGRM") = txtMakingPerGrm_Amt.Text
                .Item("OTHCHARGE") = txtOtherCharges_Amt.Text
            End If
            .Item("TRANDATE") = dtpTranDate.Value.ToString("yyyy-MM-dd")
            .Item("TRANNO") = Val(txtTranNo_NUM.Text)
            .Item("RECSNO") = recSno
            If cmbStkType.Text.ToString.Length > 0 Then
                .Item("STKTYPE") = Mid(cmbStkType.Text.ToString, 1, 1)
            End If
            If Ismerge Then .Item("PRECSNO") = prow : .Item("MERGED") = "Y" : itemDt.Rows(itemDt.Rows.Count - 1).Item("MERGED") = "Y" Else .Item("PRECSNO") = recSno : .Item("MERGED") = "N"
            If itemDt.Columns.Contains("CUTID").ToString Then .Item("CUTID") = Val(txtCut.Text)
            If itemDt.Columns.Contains("COLORID").ToString Then .Item("COLORID") = Val(txtColor.Text)
            If itemDt.Columns.Contains("CLARITYID").ToString Then .Item("CLARITYID") = Val(txtClarity.Text)
            If itemDt.Columns.Contains("SHAPEID").ToString Then .Item("SHAPEID") = Val(txtShape.Text)
            If itemDt.Columns.Contains("SETTYPEID").ToString Then .Item("SETTYPEID") = Val(txtSetType.Text)
            If itemDt.Columns.Contains("HEIGHT").ToString Then .Item("HEIGHT") = Val(txtHt.Text)
            If itemDt.Columns.Contains("WIDTH").ToString Then .Item("WIDTH") = Val(txtWdh.Text)
            .Item("HALLMARK") = Mid(CmbHallmark.Text, 1, 1)
        End With
        If locUpdateRow = -1 Then
            itemDt.Rows.Add(row)
        End If
        locUpdateRow = -1
        gridView.DataSource = itemDt
        gridView.AutoResizeColumns()
        gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        funcNew()
        If gridView.RowCount > 0 Then btnSave.Enabled = True
        If flagUpdate Then btnSave_Click(Me, New EventArgs)
    End Function
    Function funcTagNo(ByVal ItemId As Integer, ByVal piece As Integer) As Integer
        Dim Qry As String = Nothing
        Dim currentNo As String
        Dim dt As New DataTable
        dt.Clear()
        Try
            Select Case TagNoGen
                Case "I" ''From Item Master
                    Qry = " SELECT CURRENTTAGNO FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & ItemId & "'"
                Case "U" ''From SoftControl
                    Qry = " SELECT CTLTEXT AS CURRENTTAGNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LASTTAGNO'"
                Case Else
                    Exit Function
            End Select
            currentNo = objGPack.GetSqlValue(Qry, , "0", tran)
            startTagNo = Val(currentNo) + 1
            endTagNo = Val(currentNo) + piece
            currentTagNo = currentNo
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Private Sub frmLotEntry_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If gridView.RowCount > 0 Then
            If MessageBox.Show("You didnt save loaded Items. Do you want save?", "Save Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                btnSave_Click(Me, New EventArgs)
            End If
        End If
    End Sub
    Private Sub frmLotEntry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then 'Esc Key
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
            Else
                btnSave.Focus()
            End If
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            If Not grpPending.Visible Then
                If Not (dtpTranDate.Focused Or txtTranNo_NUM.Focused) Then
                    txtTranNo_NUM.Select()
                    Exit Sub
                End If
            End If
            If txtTranNo_NUM.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmLotEntry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        pnlGrid.BringToFront()
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)

        txtItemName.BackColor = txtItemCode_Num_Man.BackColor

        ''Checking Weather it have PURELOT
        Dim dt As New DataTable
        dt.Clear()
        strSql = " select 1 from " & cnAdminDb & "..SoftControl where ctlId = 'PURLOT' and ctlText = 'Y'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        dt.AcceptChanges()
        If Not dt.Rows.Count > 0 Then
            objGPack.TextClear(pnlPurchaseEntry)
            pnlPurchaseEntry.Enabled = False
        End If

        cmbBulkLot.Items.Add("YES")
        cmbBulkLot.Items.Add("NO")
        cmbMultipleTag.Items.Add("YES")
        cmbMultipleTag.Items.Add("NO")

        cmbStoneUnit.Items.Add("CARAT")
        cmbStoneUnit.Items.Add("GRAM")

        cmbValueAdded.Items.Add("ITEM")
        cmbValueAdded.Items.Add("TABLE")
        cmbValueAdded.Items.Add("DESIGNER")
        cmbValueAdded.Items.Add("TAG")

        cmbStkType.Items.Add("")
        cmbStkType.Items.Add("MANUFACTURE")
        cmbStkType.Items.Add("TRADING")

        Dim dt1 As New DataTable
        dt1.Clear()
        strSql = " select 1 from " & cnAdminDb & "..SoftControl where ctlId = 'ITEMCOUNTER' and ctlText = 'Y'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt1)
        dt1.AcceptChanges()
        If Not dt1.Rows.Count > 0 Then
            cmbItemCounter_MAN.Items.Clear()
            cmbItemCounter_MAN.Enabled = False
        End If
        dt1.Clear()
        strSql = " select 1 from " & cnAdminDb & "..SoftControl where ctlId = 'COSTCENTRE' and ctlText = 'Y'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt1)
        dt1.AcceptChanges()
        If Not dt1.Rows.Count > 0 Then
            cmbCostCentre_Man.Items.Clear()
            cmbCostCentre_Man.Enabled = False
        Else
            cmbCostCentre_Man.Enabled = True
        End If

        If _AcnameEnable Then
            ''ACCNAME
            strSql = " SELECT ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD "
            If InclCusttype = "Y" Then
                strSql += " WHERE ACTYPE IN (SELECT DISTINCT TYPEID FROM " & cnAdminDb & "..ACCTYPE "
                strSql += " WHERE ACTYPE IN ('G','D','I','C'))"
            Else
                strSql += " WHERE ACTYPE IN (SELECT DISTINCT TYPEID FROM " & cnAdminDb & "..ACCTYPE "
                strSql += " WHERE ACTYPE IN ('G','D','I'))"
            End If
            strSql += GetAcNameQryFilteration()
            strSql += " ORDER BY ACNAME"
            Dim dtAcName As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAcName)
            cmbAcName.DataSource = dtAcName
            cmbAcName.ValueMember = "ACCODE"
            cmbAcName.DisplayMember = "ACNAME"
            cmbAcName.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cmbAcName.AutoCompleteSource = AutoCompleteSource.ListItems
        End If

        itemDt.Clear()
        strSql = " SELECT "
        strSql += " '' AS ITEMNAME,'' AS SUBITEMNAME,''AS PIECES,''GRSWT,"
        strSql += " ''STNPCS,''STNWT,''STNUNIT,''DIAPCS,''DIAWT,''NETWT,''NOOFTAG,"
        strSql += " ''RATE,"
        strSql += " ''ENTRYTYPE,''LOTDATE,''DESIGNERNAME,''TRANINVNO,"
        strSql += " ''BILLNO,''COSTCENTRE,''ENTRYORDER,''ORDREPNO,"
        strSql += " ''ORDENTRYORDER,'' AS ITEMCTRNAME,''VALUEADDEDTYPE,''TABLECODE,''BULKLOT,''MULTIPLETAGS,''NARRATION,"
        strSql += " ''FINERATE,''TUCH,''WASTPER,''MCGRM,''OTHCHARGE,''NONTAGITEM,''NONTAGSUBITEM,''NONTAGPCS,''NONTAGGRSWT"
        strSql += " ,CONVERT(SMALLDATETIME,NULL)TRANDATE,CONVERT(INT,NULL)TRANNO,CONVERT(VARCHAR(15),'')RECSNO,CONVERT(VARCHAR(15),'')PRECSNO ,CONVERT(VARCHAR(1),'')MERGED "
        strSql += " ,CONVERT(INT,NULL)CUTID,CONVERT(INT,NULL)COLORID,CONVERT(INT,NULL)CLARITYID,CONVERT(INT,NULL)SETTYPEID,CONVERT(INT,NULL)SHAPEID,CONVERT(NUMERIC(15,2),NULL)HEIGHT,CONVERT(NUMERIC(15,2),NULL)WIDTH,''STKTYPE,'' HALLMARK "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(itemDt)
        itemDt.AcceptChanges()
        itemDt.Clear()

        ''TagNo Gen
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " Select ctlText as TagNoGen from " & cnAdminDb & "..SoftControl where ctlId = 'TagNoGen'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "TagNoGen")
        If ds.Tables("TagNoGen").Rows.Count > 0 Then
            TagNoGen = ds.Tables("TagNoGen").Rows(0).Item("TagNoGen")
        End If

        strSql = " select ctlText as TagNoFrom from " & cnAdminDb & "..SoftControl where ctlId = 'TagNoFrom'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "TagNoFrom")
        If ds.Tables("TagNoFrom").Rows.Count > 0 Then
            TagNoFrom = ds.Tables("TagNoFrom").Rows(0).Item("TagNoFrom")
        End If

        If TagNoGen = "L" Then
            strSql = " Select ctlText as LastTagNo from " & cnAdminDb & "..SoftControl where ctlId = 'LastTagNo'"

            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "LastTagNo")
            If ds.Tables("LastTagNo").Rows.Count > 0 Then
                lastTagNo = ds.Tables("LastTagNo").Rows(0).Item("LastTagNo")
            End If
        End If
        strSql = " SELECT NARRATION FROM " & cnAdminDb & "..NARRATION WHERE MODULEID = 'S'"

        objGPack.FillCombo(strSql, cmbNarration_OWN, , False)
        If _JobNoEnable Then
            lblJobno.Visible = True
            txtJobno.Visible = True
        Else
            lblJobno.Visible = False
            txtJobno.Visible = False
        End If
        If _AcnameEnable Then
            lblAcName.Visible = True
            cmbAcName.Visible = True
        Else
            lblAcName.Visible = False
            cmbAcName.Visible = False
        End If
        Dim xfilepath As String = Application.StartupPath & "\ConinfoY.ini"
        If IO.File.Exists(xfilepath) Then
            Dim xfil As New FileStream(xfilepath, FileMode.Open, FileAccess.Read)
            Dim xfs As New StreamReader(xfil)
            xfs.BaseStream.Seek(0, SeekOrigin.Begin)
            LinkedSrv = Mid(xfs.ReadLine, 1) '1
            LinkedDb = Mid(xfs.ReadLine, 1) & "ADMINDB"
            strSql = " if NOT exists(select 1 from sys.servers where name = '" & LinkedSrv & "')  "
            strSql += vbCrLf & " BEGIN"
            strSql += vbCrLf & " exec sp_addlinkedserver @server=" & LinkedSrv & ", @srvproduct='', @provider='SQLOLEDB', @provstr='Integrated Security=SSPI;'"
            'strSql += vbCrLf & " Exec sp_addlinkedserver '" & LinkedSrv & "'"
            strSql += vbCrLf & "End"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If
        funcNew()
    End Sub

    Private Sub txtItemCode_Num_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemCode_Num_Man.GotFocus
        If tagMade Then
            txtItemCode_Num_Man_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
            SendKeys.Send("{TAB}")
            Exit Sub
        End If

        'Main.ShowHelpText("Press Insert Key to Help")
    End Sub
    Private Sub txtItemCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemCode_Num_Man.KeyDown
        If e.KeyCode = Keys.Insert Then  'Insert Key
            strSql = " SELECT ITEMID,ITEMNAME,"
            strSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' "
            strSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END AS STOCKTYPE,"
            strSql += " CASE WHEN SUBITEM = 'Y' THEN 'YES' ELSE 'NO' END AS SUBITEM, "
            strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
            strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
            strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
            strSql += " WHEN CALTYPE = 'B' THEN 'BOTH'"
            strSql += " WHEN CALTYPE = 'M' THEN 'METAL RATE' END AS CALTYPE"

            strSql += " FROM " & cnAdminDb & "..ITEMMAST"
            strSql += " WHERE ITEMID LIKE '" & txtItemCode_Num_Man.Text & "%'"
            strSql += " AND ACTIVE = 'Y'"
            strSql += " AND STUDDED <> 'S'"
            strSql += GetItemQryFilteration("S")
            strSql += " ORDER BY ITEMNAME"
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            txtItemCode_Num_Man.Text = BrighttechPack.SearchDialog.Show("Search ItemId", strSql, cn, 1)
            'Dim autoResizeCols() As Integer = {1}
            'Dim width As Integer = 640
            'Dim height As Integer = 320
            'Dim xAxis As Integer = Math.Abs(Me.Width - width) / 2
            'Dim yAxis As Integer = Math.Abs(Me.Height - height) / 2
            'txtItemCode_Num_Man.Text = funcFind("Finding ItemName", strSql, New String() {"ItemName"}, "ItemName", "ItemName", width, height, , , , , autoResizeCols, xAxis, yAxis, 23)
        End If
    End Sub
    Private Sub txtNoOfTags_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNoOfTags_Num.GotFocus
        strSql = " select noOfPiece from " & cnAdminDb & "..ItemMast where ItemId = '" & txtItemCode_Num_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dt As New DataTable
        dt.Clear()
        da.Fill(dt)
        dt.AcceptChanges()
        If Not dt.Rows.Count > 0 Then
            Exit Sub
        End If
        Dim piece As Integer
        If Val(dt.Rows(0).Item("noOfPiece")) = 0 Then
            piece = 1
        Else
            piece = Val(dt.Rows(0).Item("noOfPiece"))
        End If
        txtNoOfTags_Num.Text = Val(txtPiece_Num_Man.Text) / piece
    End Sub
    Private Sub txtNoOfTags_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNoOfTags_Num.KeyPress
        If AscW(e.KeyChar) = 13 Then
            If Not Val(txtNoOfTags_Num.Text) <= Val(txtPiece_Num_Man.Text) Then
                MsgBox(Me.GetNextControl(txtNoOfTags_Num, False).Text + " Should not exceed" + " " + txtPiece_Num_Man.Text, MsgBoxStyle.Exclamation)
                txtNoOfTags_Num.Focus()
            End If
        End If
    End Sub
    Private Sub cmbStoneUnit_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStoneUnit.LostFocus
        CalcNetWt()
    End Sub
    Private Sub CalcNetWt()
        Dim wt As Double = Nothing
        If cmbStoneUnit.SelectedIndex = 0 Then 'CARAT
            wt = (Val(txtStoneWeight_Wet.Text) / 5) + (Val(txtDidmondWeight_Wet.Text) / 5)
        Else 'Grm
            wt = Val(txtStoneWeight_Wet.Text) + (Val(txtDidmondWeight_Wet.Text) / 5)
        End If
        wt = Val(txtGrossWeight_Wet_MAN.Text) - wt
        txtNetWeight_Wet_MAN.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub

    Private Sub txtDidmondWeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDidmondWeight_Wet.KeyPress
        CalcNetWt()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub
    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        funcOpen()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub
    Private Sub txtOpenLotNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOpenLotNo.TextChanged
        If txtOpenLotNo.Text <> "" Then
            dtpFrom.Enabled = False
            dtpTo.Enabled = False
        Else
            dtpFrom.Enabled = True
            dtpTo.Enabled = True
        End If
    End Sub
    Private Sub txtOpenItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOpenItemName.KeyDown
        If e.KeyCode = Keys.Insert Then 'Ins Key
            strSql = " SELECT ITEMID,ITEMNAME,"
            strSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' "
            strSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END AS STOCKTYPE,"
            strSql += " CASE WHEN SUBITEM = 'Y' THEN 'YES' ELSE 'NO' END AS SUBITEM FROM " & cnAdminDb & "..ITEMMAST"
            strSql += " WHERE ACTIVE = 'Y'"
            strSql += GetItemQryFilteration("S")
            strSql += " ORDER BY ITEMNAME"
            txtOpenItemName.Text = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, 1)
        End If
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        strSql = " SELECT"
        strSql += " SNO,LOTNO,LOTDATE,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID=LOT.ITEMID)AS ITEM,"
        strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID=LOT.SUBITEMID),'')AS SUBITEM,"
        strSql += " CASE WHEN PCS <> 0 THEN PCS ELSE NULL END PCS,CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END GRSWT,CASE WHEN STNPCS <> 0 THEN STNPCS ELSE NULL END STNPCS,CASE WHEN STNWT <> 0 THEN STNWT ELSE NULL END STNWT,STNUNIT,"
        strSql += " CASE WHEN DIAPCS <> 0 THEN DIAPCS ELSE NULL END DIAPCS,CASE WHEN DIAWT <> 0 THEN DIAWT ELSE NULL END DIAWT,CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END NETWT,NOOFTAG,CASE WHEN RATE <> 0 THEN RATE ELSE NULL END RATE,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = LOT.ITEMCTRID)AS COUNTER,"
        strSql += " ENTRYTYPE,"
        strSql += " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER AS D WHERE D.DESIGNERID = LOT.DESIGNERID)AS DESIGNERNAME,"
        strSql += " TRANINVNO,"
        strSql += " BILLNO,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = LOT.COSTID)AS COSTCENTRE,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
        strSql += " WMCTYPE AS VALUEADDEDTYPE,BULKLOT,MULTIPLETAGS,NARRATION,CASE WHEN FINERATE <> 0 THEN FINERATE ELSE NULL END FINERATE,CASE WHEN TUCH <> 0 THEN TUCH ELSE NULL END TUCH,"
        strSql += " CASE WHEN WASTPER <> 0 THEN WASTPER ELSE NULL END WASTPER,CASE WHEN MCGRM <> 0 THEN MCGRM ELSE NULL END MCGRM,CASE WHEN OTHCHARGE <> 0 THEN OTHCHARGE ELSE NULL END OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
        strSql += " COMPANYID,CASE WHEN CPCS <> 0 THEN CPCS ELSE NULL END CPCS,CASE WHEN CGRSWT <> 0 THEN CGRSWT ELSE NULL END CGRSWT,COMPLETED,CANCEL,"
        strSql += " ACCESSING FROM " & cnAdminDb & "..ITEMLOT AS LOT"
        strSql += " WHERE 1=1"
        If txtOpenLotNo.Text <> "" Then
            strSql += " AND LOTNO = '" & txtOpenLotNo.Text & "'"
        End If
        If cmbOpenDesignerName.Text <> "ALL" Then
            strSql += " AND DESIGNERID = (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbOpenDesignerName.Text & "')"
        End If
        If txtOpenItemName.Text <> "" Then
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOpenItemName.Text & "')"
        End If
        If dtpFrom.Enabled = True And dtpTo.Enabled = True Then
            strSql += " AND LOTDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        Dim DT As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DT)
        DT.AcceptChanges()
        gridViewOpen.DataSource = Nothing
        gridViewOpen.DataSource = DT
        gridViewOpen.Focus()
        gridViewOpen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        With gridViewOpen
            With .Columns("LOTNO")

            End With
            With .Columns("LOTDATE")
                .DefaultCellStyle.Format = "dd/MM/yyyy"
            End With
            With .Columns("ITEM")

            End With
            With .Columns("SUBITEM")

            End With
            With .Columns("PCS")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("GRSWT")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.000"
            End With
            With .Columns("STNPCS")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("STNWT")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.000"
            End With
            With .Columns("STNUNIT")

            End With
            With .Columns("DIAPCS")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("DIAWT")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.000"
            End With
            With .Columns("NETWT")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.000"
            End With
            With .Columns("NOOFTAG")

            End With
            With .Columns("RATE")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
            End With
            With .Columns("COUNTER")

            End With
            With .Columns("ENTRYTYPE")

            End With
            With .Columns("DESIGNERNAME")

            End With
            With .Columns("TRANINVNO")

            End With
            With .Columns("BILLNO")

            End With
            With .Columns("COSTCENTRE")

            End With
            With .Columns("ENTRYORDER")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("ORDREPNO")

            End With
            With .Columns("ORDENTRYORDER")

            End With
            With .Columns("VALUEADDEDTYPE")

            End With
            With .Columns("BULKLOT")

            End With
            With .Columns("MULTIPLETAGS")

            End With
            With .Columns("NARRATION")

            End With
            With .Columns("FINERATE")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
            End With
            With .Columns("TUCH")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
            End With
            With .Columns("WASTPER")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
            End With
            With .Columns("MCGRM")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
            End With
            With .Columns("OTHCHARGE")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
            End With
            With .Columns("STARTTAGNO")

            End With
            With .Columns("ENDTAGNO")

            End With
            With .Columns("CURTAGNO")

            End With
            With .Columns("COMPANYID")

            End With
            With .Columns("CPCS")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("CGRSWT")
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.000"
            End With
            With .Columns("COMPLETED")

            End With
            With .Columns("CANCEL")

            End With
            With .Columns("ACCESSING")

            End With
            '.Columns("LOTNO").Width = 50
            '.Columns("LOTNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            '.Columns("LOTDATE").Width = 80

            '.Columns("ITEM").Width = 200

            '.Columns("SUBITEM").Width = 200

            '.Columns("PCS").Width = 70
            '.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            '.Columns("GRSWT").Width = 70
            '.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("GRSWT").DefaultCellStyle.Format = "0.000"

            '.Columns("STNPCS").Width = 70
            '.Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            '.Columns("STNWT").Width = 70
            '.Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("STNWT").DefaultCellStyle.Format = "0.000"
        End With

    End Sub

    Private Sub cmbSubItemName_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubItemName_Man.GotFocus
        If tagMade Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbSubItemName_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubItemName_Man.LostFocus
        Dim pieceRate As Double = Nothing
        If cmbSubItemName_Man.Text <> "ALL" And cmbSubItemName_Man.Text <> "" Then
            pieceRate = Val(objGPack.GetSqlValue("SELECT PIECERATE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "')"))
        Else
            pieceRate = Val(objGPack.GetSqlValue("SELECT PIECERATE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "'"))
        End If
        txtItemRate_Amt.Text = IIf(pieceRate <> 0, Format(pieceRate, "0.00"), Nothing)
    End Sub

    Private Sub cmbSubItemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubItemName_Man.SelectedIndexChanged
        If cmbSubItemName_Man.Text <> "ALL" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = " & Val(txtItemCode_Num_Man.Text) & ""
            Dim calType As String = objGPack.GetSqlValue(strSql)
            If calType = "R" Then
                txtGrossWeight_Wet_MAN.Enabled = False
                txtNetWeight_Wet_MAN.Enabled = False
                cmbBulkLot.Enabled = False
                cmbMultipleTag.Enabled = False
            ElseIf calType = "M" Then
                txtGrossWeight_Wet_MAN.Enabled = False
                txtNetWeight_Wet_MAN.Enabled = False
                cmbBulkLot.Enabled = False
                cmbMultipleTag.Enabled = False
                txtItemRate_Amt.Enabled = True
            Else
                txtItemRate_Amt.Enabled = True
                txtGrossWeight_Wet_MAN.Enabled = True
                txtNetWeight_Wet_MAN.Enabled = True
                cmbBulkLot.Enabled = True
                cmbMultipleTag.Enabled = True
            End If
        End If

        If cmbSubItemName_Man.Text = "ALL" Then
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "' AND STUDDEDSTONE = 'Y'"
        Else
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND STUDDEDSTONE = 'Y' AND ITEMID = " & Val(txtItemCode_Num_Man.Text) & ""
        End If
        If objGPack.GetSqlValue(strSql).Length > 0 Then
            grpStone.Enabled = True
            grpDiamond.Enabled = True
        Else
            objGPack.TextClear(grpStone)
            objGPack.TextClear(grpDiamond)
            grpStone.Enabled = False
            grpDiamond.Enabled = False
        End If
        If cmbSubItemName_Man.Text = "ALL" Then
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "' AND ISNULL(HALLMARK,'Y') = 'Y'"
        Else
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ISNULL(HALLMARK,'Y') = 'Y' AND ITEMID = " & Val(txtItemCode_Num_Man.Text) & ""
        End If
        If objGPack.GetSqlValue(strSql).Length > 0 Then
            CmbHallmark.Text = "YES"
        Else
            CmbHallmark.Text = "NO"
        End If

    End Sub

    Private Sub gridViewOpen_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridViewOpen.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridViewOpen.RowCount > 0 Then gridViewOpen.CurrentCell = gridViewOpen.CurrentRow.Cells("LOTNO")
        End If
    End Sub

    Private Sub gridViewOpen_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridViewOpen.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not gridViewOpen.RowCount > 0 Then Exit Sub
            With gridViewOpen.Rows(gridViewOpen.CurrentRow.Index)
                Select Case .Cells("ENTRYTYPE").Value.ToString
                    Case "R"
                        cmbEntryType.Text = "REGULAR"
                    Case "OR"
                        cmbEntryType.Text = "ORDER"
                    Case "RE"
                        cmbEntryType.Text = "REPAIR"
                    Case Else 'NT
                        cmbEntryType.Text = "NONTAG TO TAG"
                End Select
                cmbCostCentre_Man.Enabled = False
                dtpDate.Value = .Cells("LOTDATE").Value
                cmbSupplier_Man.Text = .Cells("DESIGNERNAME").Value.ToString
                cmbCostCentre_Man.Text = .Cells("COSTCENTRE").Value.ToString
                txtItemName.Text = .Cells("ITEM").Value.ToString
                txtItemCode_Num_Man.Text = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "'")
                cmbSubItemName_Man.Text = IIf(.Cells("SUBITEM").Value.ToString = "", "ALL", .Cells("SUBITEM").Value.ToString)
                txtPiece_Num_Man.Text = .Cells("PCS").Value.ToString
                txtGrossWeight_Wet_MAN.Text = .Cells("GRSWT").Value.ToString
                txtStonePieces_Num.Text = .Cells("STNPCS").Value.ToString
                txtStoneWeight_Wet.Text = .Cells("STNWT").Value.ToString
                cmbStoneUnit.Text = .Cells("STNUNIT").Value.ToString
                txtDiamondPieces_Num.Text = .Cells("DIAPCS").Value.ToString
                txtDidmondWeight_Wet.Text = .Cells("DIAWT").Value.ToString
                txtNetWeight_Wet_MAN.Text = .Cells("NETWT").Value.ToString
                txtNoOfTags_Num.Text = .Cells("NOOFTAG").Value.ToString
                txtItemRate_Amt.Text = .Cells("RATE").Value.ToString
                cmbItemCounter_MAN.Text = .Cells("COUNTER").Value.ToString
                If .Cells("VALUEADDEDTYPE").Value.ToString = "I" Then
                    cmbValueAdded.Text = "ITEM"
                ElseIf .Cells("VALUEADDEDTYPE").Value.ToString = "T" Then
                    cmbValueAdded.Text = "TABLE"
                ElseIf .Cells("VALUEADDEDTYPE").Value.ToString = "D" Then
                    cmbValueAdded.Text = "DESIGNER"
                ElseIf .Cells("VALUEADDEDTYPE").Value.ToString = "P" Then
                    cmbValueAdded.Text = "TAG"
                End If
                cmbBulkLot.Text = IIf(.Cells("BULKLOT").Value.ToString = "Y", "YES", "NO")
                cmbMultipleTag.Text = IIf(.Cells("MULTIPLETAGS").Value.ToString = "Y", "YES", "NO")
                cmbNarration_OWN.Text = .Cells("NARRATION").Value.ToString
                txtFineRate_Amt.Text = .Cells("FINERATE").Value.ToString
                txtTuch_WET.Text = .Cells("TUCH").Value.ToString
                txtWastage_Per.Text = .Cells("WASTPER").Value.ToString
                txtMakingPerGrm_Amt.Text = .Cells("MCGRM").Value.ToString
                txtOtherCharges_Amt.Text = .Cells("OTHCHARGE").Value.ToString
                flagUpdate = True
                cPcs = Val(.Cells("CPCS").Value.ToString)
                cWeight = Val(.Cells("CGRSWT").Value.ToString)
                updLotNo = Val(.Cells("LOTNO").Value.ToString)
                updSno = .Cells("SNO").Value.ToString
                If objGPack.GetSqlValue("SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO = '" & updSno & "'") <> "" Then
                    tagMade = True
                Else
                    tagMade = False
                End If
            End With
            tabMain.SelectedTab = tabGeneral
            cmbEntryType.Focus()
        End If
    End Sub

    Private Sub txtGrossWeight_Wet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrossWeight_Wet_MAN.LostFocus
        If grpPending.Visible Then
            If Val(txtGrossWeight_Wet_MAN.Text) > Val(lblPendingWeight.Text) Then
                MsgBox("Pending Weight " & lblPendingWeight.Text & " Only", MsgBoxStyle.Information)
                txtGrossWeight_Wet_MAN.Text = lblPendingWeight.Text
                txtGrossWeight_Wet_MAN.Focus()
            End If
        End If
        If flagUpdate And Val(txtGrossWeight_Wet_MAN.Text) < cWeight Then
            MsgBox("Completed Weight : " & cWeight & vbCrLf & "You must enter minimum of completed weight", MsgBoxStyle.Information)
            txtGrossWeight_Wet_MAN.Text = Format(cWeight, "0.000")
            txtGrossWeight_Wet_MAN.Focus()
            txtGrossWeight_Wet_MAN.SelectAll()
            Exit Sub
        End If
        'txtNetWeight_Wet_MAN.Text = IIf(Val(txtGrossWeight_Wet_MAN.Text) <> 0, Format(Val(txtGrossWeight_Wet_MAN.Text), "0.000"), "")
        CalcNetWt()
    End Sub

    Private Sub txtItemCode_Num_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemCode_Num_Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then 'Enter Key
            If txtItemCode_Num_Man.Text = "" Then
                Exit Sub
            End If
            Dim itemName As String = objGPack.GetSqlValue(" select ItemName from " & cnAdminDb & "..ItemMast where ItemId = '" & txtItemCode_Num_Man.Text & "' AND ACTIVE = 'Y'", "itemname")
            txtItemName.Text = itemName
            If txtItemName.Text = "" Then
                MsgBox("Invalid " + Me.GetNextControl(txtItemCode_Num_Man, False).Text, MsgBoxStyle.Information)
                txtItemCode_Num_Man.Select()
                Exit Sub
            End If

            cmbItemCounter_MAN.Text = ""
            strSql = " SELECT SUBITEM,VALUEADDEDTYPE,CALTYPE,STOCKTYPE,NOOFPIECE,"
            strSql += " ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = DEFAULTCOUNTER),'')AS ITEMCTRNAME"
            strSql += " ,STUDDEDSTONE,ISNULL(HALLMARK,'Y')HALLMARK"
            strSql += " FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & txtItemCode_Num_Man.Text & "'"
            strSql += GetItemQryFilteration("S")
            Dim dt As New DataTable
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            dt.AcceptChanges()

            txtGrossWeight_Wet_MAN.Enabled = False
            grpStone.Enabled = False
            grpDiamond.Enabled = False
            txtNetWeight_Wet_MAN.Enabled = False
            txtItemRate_Amt.Enabled = False
            cmbBulkLot.Enabled = False
            cmbMultipleTag.Enabled = False
            cmbSubItemName_Man.Enabled = False
            txtNoOfTags_Num.Enabled = False

            ''SUBITEMSETTING
            If Not STOCKVALIDATION Then
                strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = '" & txtItemCode_Num_Man.Text & "' AND ACTIVE = 'Y' ORDER BY SUBITEMNAME"
                cmbSubItemName_Man.Items.Clear()
                cmbSubItemName_Man.Items.Add("ALL")
                objGPack.FillCombo(strSql, cmbSubItemName_Man, False)
                cmbSubItemName_Man.Text = "ALL"
                If Not cmbSubItemName_Man.Items.Count > 1 Then
                    cmbSubItemName_Man.Enabled = False
                Else
                    cmbSubItemName_Man.Enabled = True
                End If
            ElseIf STOCKVALIDATION And cmbSubItemName_Man.Text = "" Then
                If dt.Rows(0).Item("SUBITEM").ToString = "Y" Then
                    strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = '" & txtItemCode_Num_Man.Text & "' AND ACTIVE = 'Y' ORDER BY SUBITEMNAME"
                    cmbSubItemName_Man.Items.Clear()
                    cmbSubItemName_Man.Items.Add("ALL")
                    objGPack.FillCombo(strSql, cmbSubItemName_Man, False)
                    cmbSubItemName_Man.Text = "ALL"
                    If Not cmbSubItemName_Man.Items.Count > 1 Then
                        cmbSubItemName_Man.Enabled = False
                    Else
                        cmbSubItemName_Man.Enabled = True
                    End If
                End If
            End If
            cmbTableCode.Enabled = False
            With dt.Rows(0)
                Select Case .Item("VALUEADDEDTYPE").ToString

                    Case "I"
                        cmbValueAdded.Text = "ITEM"
                    Case "T"
                        cmbValueAdded.Text = "TABLE"
                        cmbTableCode.Enabled = True
                    Case "D"
                        cmbValueAdded.Text = "DESIGNER"
                    Case "P"
                        cmbValueAdded.Text = "TAG"
                    Case ""
                        cmbValueAdded.Text = ""
                End Select

                If .Item("CALTYPE").ToString = "R" Then ''RATE SETTING
                    txtItemRate_Amt.Enabled = True
                    cmbBulkLot.Enabled = True
                    cmbMultipleTag.Enabled = True
                Else
                    txtGrossWeight_Wet_MAN.Enabled = True
                    txtNetWeight_Wet_MAN.Enabled = True
                End If

                If .Item("CALTYPE").ToString = "M" Then txtItemRate_Amt.Enabled = True
                If .Item("ITEMCTRNAME").ToString <> "" Then cmbItemCounter_MAN.Text = .Item("ITEMCTRNAME").ToString
                If .Item("SUBITEM").ToString = "Y" Then cmbSubItemName_Man.Enabled = True
                If .Item("STOCKTYPE").ToString = "T" Then txtNoOfTags_Num.Enabled = True Else txtNoOfTags_Num.Clear()
                If .Item("STUDDEDSTONE").ToString = "Y" Then
                    grpStone.Enabled = True
                    grpDiamond.Enabled = True
                End If
                If STOCKVALIDATION And txtTranNo_NUM.Text <> "" Then cmbSubItemName_Man.Enabled = False
                If cmbTableCode.Enabled = True Then
                    strSql = " SELECT '' TABLECODE UNION ALL SELECT DISTINCT TABLECODE FROM " & cnAdminDb & "..WMCTABLE WHERE TABLECODE <> '' "
                    strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
                    strSql += " ORDER BY TABLECODE"
                    objGPack.FillCombo(strSql, cmbTableCode)
                    'strSql = " SELECT TABLECODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= '" & txtItemCode_Num_Man.Text & "' AND TABLECODE <> ''"
                    'Dim dtTableCode As New DataTable
                    'dtTableCode.Clear()
                    'da = New OleDbDataAdapter(strSql, cn)
                    'da.Fill(dtTableCode)
                    'If dtTableCode.Rows.Count > 0 Then
                    '    cmbTableCode.Text = dtTableCode.Rows(0).Item("TABLECODE").ToString
                    'End If
                End If
                If .Item("HALLMARK").ToString = "Y" Then
                    CmbHallmark.Text = "YES"
                Else

                    CmbHallmark.Text = "NO"
                End If
            End With
        End If
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.CurrentRow.Cells(0)
            End If
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress

    End Sub

    Private Sub gridView_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridView.UserDeletedRow
        itemDt.AcceptChanges()
        If Not gridView.RowCount > 0 Then btnSave.Enabled = False
    End Sub

    Private Sub txtNetWeight_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNetWeight_Wet_MAN.GotFocus


        CalcNetWt()
    End Sub

    Private Sub txtStoneWeight_Wet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStoneWeight_Wet.LostFocus
        CalcNetWt()
    End Sub

    Private Sub txtDidmondWeight_Wet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDidmondWeight_Wet.LostFocus
        CalcNetWt()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        funcLoadGrid()
    End Sub

    Private Sub txtItemRate_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemRate_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbBulkLot.Text = "YES" Then
                If Not Val(txtItemRate_Amt.Text) > 0 Then
                    MsgBox(Me.GetNextControl(txtItemRate_Amt, False).Text + " Should not empty", MsgBoxStyle.Information)
                    txtItemRate_Amt.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txtNetWeight_Wet_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNetWeight_Wet_MAN.KeyPress
        If txtNetWeight_Wet_MAN.Text = "" Then txtNetWeight_Wet_MAN.Text = "0.000"
    End Sub



    Private Sub txtNetWeight_Wet_MAN_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNetWeight_Wet_MAN.KeyUp
        If Val(txtGrossWeight_Wet_MAN.Text) < Val(txtNetWeight_Wet_MAN.Text) Then
            MsgBox("Net weight should not exeed Gross weight", MsgBoxStyle.Information)
            txtNetWeight_Wet_MAN.Text = Val(txtNetWeight_Wet_MAN.Text) - Val(Chr(e.KeyCode))
            txtNetWeight_Wet_MAN.SelectAll()
        End If
    End Sub


    Private Sub txtItemCode_Num_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemCode_Num_Man.LostFocus
        'Main.HideHelpText()
        Dim pieceRate As Double = Val(objGPack.GetSqlValue("SELECT PIECERATE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "'"))
        txtItemRate_Amt.Text = IIf(pieceRate <> 0, Format(pieceRate, "0.00"), Nothing)
    End Sub


    Private Sub txtWastage_Per_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWastage_Per.GotFocus
        If Val(txtTuch_WET.Text) > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbSupplier_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSupplier_Man.GotFocus
        If tagMade Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub

    Private Sub txtPiece_Num_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPiece_Num_Man.LostFocus
        If grpPending.Visible Then
            If Val(txtPiece_Num_Man.Text) > Val(lblPendingPcs.Text) And LotPcsChk = True Then
                MsgBox("Pending Pcs " & lblPendingPcs.Text & " Only", MsgBoxStyle.Information)
                txtPiece_Num_Man.Text = lblPendingPcs.Text
                txtPiece_Num_Man.Focus()
                Exit Sub
            End If
        End If
        If flagUpdate And Val(txtPiece_Num_Man.Text) < cPcs And LotPcsChk = True Then
            MsgBox("Completed Pcs : " & cPcs & vbCrLf & "You must enter minimum of completed pcs", MsgBoxStyle.Information)
            txtPiece_Num_Man.Text = cPcs.ToString
            txtPiece_Num_Man.Focus()
            txtPiece_Num_Man.SelectAll()
        End If
    End Sub

    Private Sub txtTranNo_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTranNo_NUM.KeyDown
        If e.KeyCode = Keys.Insert Then
            If STOCKVALIDATION Then
                strSql = vbCrLf + "SELECT SNO"
                strSql += vbCrLf + ",(SELECT TOP 1 COSTID FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)COSTID"
                strSql += vbCrLf + ",(SELECT TOP 1 TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TRANNO"
                strSql += vbCrLf + ",(SELECT TOP 1 TRANDATE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TRANDATE"
                strSql += vbCrLf + ",(SELECT TOP 1 COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =  "
                strSql += vbCrLf + "(SELECT TOP 1 COSTID FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))COSTNAME"
                strSql += vbCrLf + ",(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE="
                strSql += vbCrLf + "(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))ACNAME"
                strSql += vbCrLf + ",(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST  WHERE ITEMID ="
                strSql += vbCrLf + "(SELECT TOP 1 ITEMID FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))ITEM"
                strSql += vbCrLf + ",(SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST  WHERE SUBITEMID ="
                strSql += vbCrLf + "(SELECT TOP 1 SUBITEMID FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))SUBITEM"
                strSql += vbCrLf + ",SUM(PCS)PCS,SUM(GRSWT)GRSWT"
                strSql += vbCrLf + ",SUM(NETWT)NETWT,SUM(PUREWT)PUREWT"
                strSql += vbCrLf + ",SUM(WASTAGE)WASTAGE,SUM(MCHARGE)MCHARGE"
                strSql += vbCrLf + ",(SELECT TOP 1 TOUCH FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TOUCH"
                strSql += vbCrLf + ",(SELECT TOP 1 ITEMID FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)ITEMID"
                strSql += vbCrLf + ",(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)ACCODE"
                strSql += vbCrLf + ",(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=X.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S') )STNPCS"
                strSql += vbCrLf + ",(SELECT SUM(CASE WHEN STONEUNIT='C' THEN (STNWT/5) ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=X.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')  )STNWT"
                strSql += vbCrLf + ",(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=X.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') )DIAPCS"
                strSql += vbCrLf + ",(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=X.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') )DIAWT"
                strSql += vbCrLf + ",(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=(SELECT TOP 1 LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO=X.SNO))LSPCS"
                strSql += vbCrLf + ",(SELECT SUM(CASE WHEN STNUNIT='C' THEN (STNWT/5) ELSE STNWT END) FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=(SELECT TOP 1 LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO=X.SNO))LSWT"
                strSql += vbCrLf + ",(SELECT SUM(DIAPCS) FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=(SELECT TOP 1 LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO=X.SNO))LDPCS"
                strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=(SELECT TOP 1 LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO=X.SNO))LDWT"
                strSql += vbCrLf + ",'G' AS STONEUNIT,STKTYPE "
                strSql += vbCrLf + ",(SELECT TOP 1 RATE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)RATE "
                'strSql += vbCrLf + ",(SELECT TOP 1 ISNULL(STONEUNIT,'G') FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=X.SNO)STONEUNIT"
                strSql += vbCrLf + "FROM ("
                strSql += vbCrLf + "SELECT "
                strSql += vbCrLf + "SUM(R.PCS)PCS"
                strSql += vbCrLf + ",SUM(R.GRSWT)GRSWT"
                strSql += vbCrLf + ",SUM(R.NETWT)NETWT"
                strSql += vbCrLf + ",SUM(R.PUREWT)PUREWT"
                strSql += vbCrLf + ",0 WASTAGE"
                strSql += vbCrLf + ",0 MCHARGE"
                strSql += vbCrLf + ",R.SNO,'' STKTYPE "
                strSql += vbCrLf + "FROM " & cnStockDb & "..OPENWEIGHT R WHERE 1=1"
                If cnCostId <> "" Then strSql += vbCrLf + " AND R.COSTID='" & cnCostId & "'"
                strSql += vbCrLf + "AND R.GRSWT <> 0 AND STOCKTYPE='C' "
                strSql += vbCrLf + Replace(GetLoadedSno(), "I.SNO", "R.SNO")
                strSql += vbCrLf + "GROUP BY R.SNO"
                strSql += vbCrLf + "UNION ALL"
                strSql += vbCrLf + "SELECT "
                strSql += vbCrLf + "SUM(R.PCS)PCS"
                strSql += vbCrLf + ",SUM(R.GRSWT)GRSWT"
                strSql += vbCrLf + ",SUM(R.NETWT)NETWT"
                strSql += vbCrLf + ",SUM(R.PUREWT)PUREWT"
                strSql += vbCrLf + ",SUM(R.WASTAGE)WASTAGE"
                strSql += vbCrLf + ",SUM(R.MCHARGE)MCHARGE"
                strSql += vbCrLf + ",R.SNO,ISNULL(R.STKTYPE,'')STKTYPE "
                strSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT R"
                strSql += vbCrLf + "WHERE R.TRANDATE = '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + "AND ISNULL(R.CANCEL,'') = '' "
                strSql += vbCrLf + "AND R.GRSWT >= 0 AND LEN(R.TRANTYPE) > 2 "
                strSql += vbCrLf + "AND ISNULL(R.FLAG,'')<>'X' "
                'strSql += vbCrLf + "AND ISNULL(R.ITEMID,0)<>0 "
                strSql += vbCrLf + Replace(GetLoadedSno(), "I.SNO", "R.SNO")
                If cnCostId <> "" Then strSql += vbCrLf + " AND R.COSTID='" & cnCostId & "'"
                'FOR NMJ APP REC -> LOT ISSUE -> PURCHASE/APP ***
                'strSql += vbCrLf + "AND ISNULL(JOBISNO,'') NOT IN(SELECT RECSNO FROM " & cnStockDb & "..LOTISSUE) "
                'strSql += vbCrLf + "AND ISNULL(SNO,'') NOT IN (SELECT JOBISNO FROM " & cnStockDb & "..RECEIPT WHERE SNO IN (SELECT RECSNO FROM " & cnStockDb & "..LOTISSUE)) "
                '###
                strSql += vbCrLf + "GROUP BY R.SNO,R.STKTYPE"
                strSql += vbCrLf + "UNION ALL"
                strSql += vbCrLf + "SELECT "
                strSql += vbCrLf + "-1*SUM(I.PCS)PCS"
                strSql += vbCrLf + ",-1*SUM(I.GRSWT)GRSWT"
                strSql += vbCrLf + ",-1*SUM(I.NETWT)NETWT"
                strSql += vbCrLf + ",-1*SUM(I.PUREWT)PUREWT"
                strSql += vbCrLf + ",-1*SUM(I.WASTAGE)WASTAGE"
                strSql += vbCrLf + ",-1*SUM(I.MCHARGE)MCHARGE"
                strSql += vbCrLf + ",I.RESNO,ISNULL(I.STKTYPE,'')STKTYPE "
                strSql += vbCrLf + "FROM " & cnStockDb & "..ISSUE I"
                strSql += vbCrLf + "WHERE "
                strSql += vbCrLf + "ISNULL(I.CANCEL,'') = '' "
                If cnCostId <> "" Then strSql += vbCrLf + " AND I.COSTID='" & cnCostId & "'"
                strSql += vbCrLf + "AND I.GRSWT >= 0 AND LEN(I.TRANTYPE) > 2 "
                strSql += vbCrLf + "AND ISNULL(I.FLAG,'')<>'X' "
                'strSql += vbCrLf + "AND ISNULL(I.ITEMID,0)<>0 "
                strSql += vbCrLf + "AND I.RESNO IN"
                strSql += vbCrLf + "(SELECT SNO FROM " & cnStockDb & "..OPENWEIGHT "
                strSql += vbCrLf + "UNION ALL "
                strSql += vbCrLf + "SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE = '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''  AND TRANTYPE<>'PU' )"
                strSql += vbCrLf + "GROUP BY I.RESNO,I.STKTYPE "
                strSql += vbCrLf + "UNION ALL"
                strSql += vbCrLf + "SELECT "
                strSql += vbCrLf + "-1*SUM(I.PCS)PCS"
                strSql += vbCrLf + ",-1*SUM(I.GRSWT)GRSWT"
                strSql += vbCrLf + ",-1*SUM(I.NETWT)NETWT"
                strSql += vbCrLf + ",0 PUREWT"
                strSql += vbCrLf + ",0 WASTAGE"
                strSql += vbCrLf + ",0 MCHARGE"
                strSql += vbCrLf + ",I.RECSNO AS SNO,ISNULL(I.STKTYPE,'')STKTYPE "
                strSql += vbCrLf + "FROM " & cnStockDb & "..LOTISSUE I"
                strSql += vbCrLf + "WHERE "
                strSql += vbCrLf + "ISNULL(I.CANCEL,'') = '' "
                strSql += vbCrLf + "AND ISNULL(I.ITEMID,0)<>0 "
                strSql += vbCrLf + "AND I.RECSNO IN"
                strSql += vbCrLf + "(SELECT SNO FROM " & cnStockDb & "..OPENWEIGHT "
                strSql += vbCrLf + "UNION ALL "
                strSql += vbCrLf + "SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE = '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''  AND TRANTYPE<>'PU' )"
                strSql += vbCrLf + "GROUP BY I.RECSNO,I.STKTYPE "
                strSql += vbCrLf + ")X GROUP BY SNO,STKTYPE HAVING (SUM(PCS)>0 OR SUM(GRSWT)>0)"
                strSql += vbCrLf + "ORDER BY TRANDATE,TRANNO"
            Else
                strSql = vbCrLf + "  SELECT I.SNO,I.COSTID,I.TRANNO,I.TRANDATE"
                strSql += vbCrLf + "  ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID)COSTNAME"
                strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)ACNAME"
                strSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
                strSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)AS SUBITEM"
                strSql += vbCrLf + "  ,(I.PCS - SUM(ISNULL(L.PCS,0)))PCS"
                strSql += vbCrLf + "  ,(I.GRSWT - SUM(ISNULL(L.GRSWT,0)))GRSWT"
                strSql += vbCrLf + "  ,(I.NETWT - SUM(ISNULL(L.NETWT,0)))NETWT"
                strSql += vbCrLf + "  ,I.TOUCH,I.WASTAGE,I.MCHARGE"
                strSql += vbCrLf + "  ,I.PUREWT PUREWT,I.ITEMID,I.ACCODE"
                'strSql += vbCrLf + "  ,NULL STNPCS,NULL STNWT"
                'strSql += vbCrLf + "  ,NULL DIAPCS,NULL DIAWT,NULL STONEUNIT"
                strSql += vbCrLf + " ,(SELECT ISNULL(SUM(ISNULL(STNPCS,0)),0) SPCS FROM " & cnStockDb & "..RECEIPTSTONE  S WHERE"
                strSql += vbCrLf + " I.SNO = S.ISSSNO And STNITEMID In (Select ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNPCS ,"
                strSql += vbCrLf + " (SELECT SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT ELSE 0 END) FROM " & cnStockDb & "..RECEIPTSTONE  S WHERE "
                strSql += vbCrLf + " I.SNO = S.ISSSNO And STNITEMID In (Select ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWT ,"
                strSql += vbCrLf + " (SELECT ISNULL(SUM(ISNULL(S.STNPCS,0)),0) DPCS FROM  " & cnStockDb & "..RECEIPTSTONE S WHERE "
                strSql += vbCrLf + " I.SNO = S.ISSSNO And STNITEMID In (Select ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAPCS ,"
                strSql += vbCrLf + " Convert(NUMERIC(15, 4), (SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT "
                strSql += vbCrLf + " From  " & cnStockDb & "..RECEIPTSTONE S Where I.SNO = S.ISSSNO And STNITEMID In (Select ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT "
                ''strSql += vbCrLf + " ,(SELECT DISTINCT ISNULL(STONEUNIT,'') stoneunit FROM  " & cnStockDb & "..RECEIPTSTONE S WHERE "
                ''strSql += vbCrLf + " I.SNO = S.ISSSNO And STNITEMID In (Select ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D' or DIASTONE = 'S' ))STONEUNIT "
                strSql += vbCrLf + "  ,NULL LSPCS,NULL LSWT,NULL LDPCS,NULL LDWT"
                strSql += vbCrLf + "  ,I.CUTID,I.COLORID,I.CLARITYID,I.SETTYPEID,I.SHAPEID,I.HEIGHT,I.WIDTH,I.STKTYPE,I.RATE"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I"
                strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..LOTISSUE AS L ON L.TRANNO = I.TRANNO AND L.TRANDATE = I.TRANDATE AND L.RECSNO = I.SNO"
                strSql += vbCrLf + "  WHERE i.TRANDATE = '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "' AND (I.GRSWT <> 0 OR I.PCS <> 0)"
                If cnCostId <> "" Then strSql += vbCrLf + " AND I.COSTID='" & cnCostId & "'"
                'strSql += vbCrLf + "  AND TRANTYPE NOT IN('SR','PU') "
                strSql += vbCrLf + "  AND LEN(TRANTYPE)>2"
                strSql += vbCrLf + "  AND ISNULL(JOBISNO,'') NOT IN(SELECT RECSNO FROM " & cnStockDb & "..LOTISSUE)"
                strSql += vbCrLf + "  AND NOT EXISTS(SELECT 1 FROM  " & cnStockDb & "..ISSUE WHERE BATCHNO=I.BATCHNO)"
                strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = '' AND ISNULL(L.CANCEL,'') = ''"
                strSql += vbCrLf + "  AND ISNULL(I.FLAG,'')<>'X' "
                strSql += vbCrLf + GetLoadedSno()
                strSql += vbCrLf + "  GROUP BY I.TRANNO,I.TRANDATE,I.ACCODE,I.ITEMID,I.SUBITEMID,I.TOUCH,I.PUREWT,I.SNO,I.COSTID,I.WASTAGE,I.MCHARGE,I.PCS,I.GRSWT,I.NETWT"
                strSql += vbCrLf + "  ,I.CUTID,I.COLORID,I.CLARITYID,I.SETTYPEID,I.SHAPEID,I.HEIGHT,I.WIDTH,I.STKTYPE,I.RATE "
                strSql += vbCrLf + "  HAVING (I.PCS - SUM(ISNULL(L.PCS,0)) > 0   OR I.GRSWT - SUM(ISNULL(L.GRSWT,0)) > 0)"
            End If
            Dim retRow As DataRow = BrighttechPack.SearchDialog.Show_R("Select Tranno", strSql, cn)
            If retRow Is Nothing Then Exit Sub
            If HideAccLink = False Then
                strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACCODE,'') = '" & retRow.Item("ACCODE").ToString & "'"
                Dim desName As String = objGPack.GetSqlValue(strSql)
                'Manepally
                'If desName = "" Then
                '    MsgBox(retRow.Item("ACNAME").ToString & vbCrLf & "Designer info not found", MsgBoxStyle.Information)
                '    Exit Sub
                'End If
                cmbSupplier_Man.Text = desName
            End If
            LoadDefaultLotValues(retRow)
            'txtItemCode_Num_Man.Enabled = False
            'cmbSubItemName_Man.Enabled = False
            txtPiece_Num_Man.Text = lblPendingPcs.Text
            txtGrossWeight_Wet_MAN.Text = lblPendingWeight.Text
            btnAdd.Enabled = True
            Me.SelectNextControl(txtTranNo_NUM, True, True, True, True)
        End If
    End Sub

    Private Function GetLoadedSno() As String
        Dim retStr As String = ""
        Dim DtDistinct As DataTable
        If itemDt.Columns.Contains("computedpcs") = False Then
            Dim col As DataColumn = New DataColumn("computedpcs", GetType(System.Int32))
            col.Expression = "Convert([PIECES], 'System.Int32')"
            itemDt.Columns.Add(col)
        End If

        If itemDt.Columns.Contains("computedwt") = False Then
            Dim col1 As DataColumn = New DataColumn("computedwt", GetType(System.Decimal))
            col1.Expression = "Convert([GRSWT], 'System.Decimal')"
            itemDt.Columns.Add(col1)
        End If
        If itemDt.Columns.Contains("computedstnpcs") = False Then
            Dim col As DataColumn = New DataColumn("computedstnpcs", GetType(System.Int32))
            col.Expression = "Convert([STNPCS], 'System.Int32')"
            itemDt.Columns.Add(col)
        End If

        If itemDt.Columns.Contains("computedstnwt") = False Then
            Dim col1 As DataColumn = New DataColumn("computedstnwt", GetType(System.Decimal))
            col1.Expression = "Convert([STNWT], 'System.Decimal')"
            itemDt.Columns.Add(col1)
        End If
        If itemDt.Columns.Contains("computeddiapcs") = False Then
            Dim col As DataColumn = New DataColumn("computeddiapcs", GetType(System.Int32))
            col.Expression = "Convert([DIAPCS], 'System.Int32')"
            itemDt.Columns.Add(col)
        End If

        If itemDt.Columns.Contains("computeddiawt") = False Then
            Dim col1 As DataColumn = New DataColumn("computeddiawt", GetType(System.Decimal))
            col1.Expression = "Convert([DIAWT], 'System.Decimal')"
            itemDt.Columns.Add(col1)
        End If

        DtDistinct = itemDt.DefaultView.ToTable(True, "RECSNO")
        For cnt As Integer = 0 To DtDistinct.Rows.Count - 1
            'For cnt As Integer = 0 To itemDt.Rows.Count - 1
            Dim exprs As String = "RECSNO = '" & DtDistinct.Rows(cnt).Item(0).ToString & "'"

            Dim entpcs As Double = Val("" & itemDt.Compute("SUM(computedpcs)", exprs))
            Dim entgwt As Double = Val("" & itemDt.Compute("Sum(computedwt)", exprs))
            Dim balances As String = getbalwtforsno(DtDistinct.Rows(cnt).Item("RECSNO").ToString)
            Dim balarry() As String = Split(balances, ",")
            If Val(balarry(0)) - entpcs <= 0 And Val(balarry(1)) - entgwt <= 0 Then
                retStr += "'" & DtDistinct.Rows(cnt).Item("RECSNO").ToString & "'"
                If cnt <> DtDistinct.Rows.Count - 1 Then
                    retStr += ","
                End If
            End If
        Next
        If retStr <> "" Then Return " AND I.SNO NOT IN (" & retStr & ")"
        Return retStr
    End Function

    Private Function getbalwtforsno(ByVal zSno As String) As String
        Dim Retbalance As String = ""
        strSql = "  SELECT SUM(I.PCS - ISNULL(L.PCS,0))PCS"
        strSql += vbCrLf + "  ,SUM(I.GRSWT - ISNULL(L.GRSWT,0))GRSWT"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..LOTISSUE AS L ON L.TRANNO = I.TRANNO AND L.TRANDATE = I.TRANDATE AND I.SNO = L.RECSNO "
        strSql += vbCrLf + "  WHERE (I.GRSWT <> 0 OR I.PCS <> 0)"
        strSql += vbCrLf + " AND I.TRANTYPE <>'PU'"
        strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = '' AND ISNULL(L.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.SNO ='" & zSno & "'"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtBal As New DataTable
        da.Fill(dtBal)
        If dtBal.Rows.Count > 0 Then Retbalance = dtBal.Rows(0).Item(0).ToString & "," & dtBal.Rows(0).Item(1).ToString
        Return Retbalance
    End Function
    Private Sub txtTranNo_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTranNo_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If STOCKVALIDATION Then
                strSql = vbCrLf + "SELECT SNO"
                strSql += vbCrLf + ",(SELECT COSTID FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)COSTID"
                strSql += vbCrLf + ",(SELECT TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TRANNO"
                strSql += vbCrLf + ",(SELECT TRANDATE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TRANDATE"
                strSql += vbCrLf + ",(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =  "
                strSql += vbCrLf + "(SELECT TOP 1 COSTID FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))COSTNAME"
                strSql += vbCrLf + ",(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE="
                strSql += vbCrLf + "(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))ACNAME"
                strSql += vbCrLf + ",(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST  WHERE ITEMID ="
                strSql += vbCrLf + "(SELECT TOP 1 ITEMID FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))ITEM"
                strSql += vbCrLf + ",(SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST  WHERE SUBITEMID ="
                strSql += vbCrLf + "(SELECT TOP 1 SUBITEMID FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))SUBITEM"
                strSql += vbCrLf + ",SUM(PCS)PCS,SUM(GRSWT)GRSWT"
                strSql += vbCrLf + ",SUM(NETWT)NETWT,SUM(PUREWT)PUREWT"
                strSql += vbCrLf + ",(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=X.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S') )STNPCS"
                strSql += vbCrLf + ",(SELECT SUM(CASE WHEN STONEUNIT='C' THEN (STNWT/5) ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=X.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')  )STNWT"
                strSql += vbCrLf + ",(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=X.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') )DIAPCS"
                strSql += vbCrLf + ",(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=X.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') )DIAWT"
                strSql += vbCrLf + ",(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=(SELECT TOP 1 LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO=X.SNO))LSPCS"
                strSql += vbCrLf + ",(SELECT SUM(CASE WHEN STNUNIT='C' THEN (STNWT/5) ELSE STNWT END) FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=(SELECT TOP 1 LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO=X.SNO))LSWT"
                strSql += vbCrLf + ",(SELECT SUM(DIAPCS) FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=(SELECT TOP 1 LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO=X.SNO))LDPCS"
                strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=(SELECT TOP 1 LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO=X.SNO))LDWT"
                'strSql += vbCrLf + ",(SELECT TOP 1 ISNULL(STONEUNIT,'G') FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=X.SNO)STONEUNIT"
                strSql += vbCrLf + ",'G' AS STONEUNIT"
                strSql += vbCrLf + ",SUM(WASTAGE)WASTAGE,SUM(MCHARGE)MCHARGE"
                strSql += vbCrLf + ",(SELECT TOP 1 TOUCH FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TOUCH"
                strSql += vbCrLf + ",(SELECT TOP 1 ITEMID FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)ITEMID"
                strSql += vbCrLf + ",(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)ACCODE,STKTYPE "
                strSql += vbCrLf + ",(SELECT TOP 1 RATE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)RATE "
                strSql += vbCrLf + "FROM ("
                strSql += vbCrLf + "SELECT "
                strSql += vbCrLf + "SUM(R.PCS)PCS"
                strSql += vbCrLf + ",SUM(R.GRSWT)GRSWT"
                strSql += vbCrLf + ",SUM(R.NETWT)NETWT"
                strSql += vbCrLf + ",SUM(R.PUREWT)PUREWT"
                strSql += vbCrLf + ",0 WASTAGE"
                strSql += vbCrLf + ",0 MCHARGE"
                strSql += vbCrLf + ",R.SNO,'' STKTYPE "
                strSql += vbCrLf + "FROM " & cnStockDb & "..OPENWEIGHT R WHERE 1=1"
                If cnCostId <> "" Then strSql += vbCrLf + " AND R.COSTID='" & cnCostId & "'"
                strSql += vbCrLf + "AND R.GRSWT <> 0 AND STOCKTYPE='C' "
                strSql += vbCrLf + Replace(GetLoadedSno(), "I.SNO", "R.SNO")
                strSql += vbCrLf + "GROUP BY R.SNO"
                strSql += vbCrLf + "UNION ALL"
                strSql += vbCrLf + "SELECT "
                strSql += vbCrLf + "SUM(R.PCS)PCS"
                strSql += vbCrLf + ",SUM(R.GRSWT)GRSWT"
                strSql += vbCrLf + ",SUM(R.NETWT)NETWT"
                strSql += vbCrLf + ",SUM(R.PUREWT)PUREWT"
                strSql += vbCrLf + ",SUM(R.WASTAGE)WASTAGE"
                strSql += vbCrLf + ",SUM(R.MCHARGE)MCHARGE"
                strSql += vbCrLf + ",R.SNO,ISNULL(R.STKTYPE,'') STKTYPE "
                strSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT R"
                strSql += vbCrLf + "WHERE R.TRANDATE = '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + "AND ISNULL(R.CANCEL,'') = '' "
                If cnCostId <> "" Then strSql += vbCrLf + " AND R.COSTID='" & cnCostId & "'"
                strSql += vbCrLf + "AND R.GRSWT <> 0 AND LEN(R.TRANTYPE) > 2 "
                'strSql += vbCrLf + "AND ISNULL(R.ITEMID,0)<>0 "
                strSql += vbCrLf + "  AND ISNULL(R.FLAG,'')<>'X' "
                strSql += vbCrLf + "AND R.TRANNO = " & Val(txtTranNo_NUM.Text) & ""
                strSql += vbCrLf + Replace(GetLoadedSno(), "I.SNO", "R.SNO")
                strSql += vbCrLf + "GROUP BY R.SNO,R.STKTYPE"
                strSql += vbCrLf + "UNION ALL"
                strSql += vbCrLf + "SELECT "
                strSql += vbCrLf + "-1*SUM(I.PCS)PCS"
                strSql += vbCrLf + ",-1*SUM(I.GRSWT)GRSWT"
                strSql += vbCrLf + ",-1*SUM(I.NETWT)NETWT"
                strSql += vbCrLf + ",-1*SUM(I.PUREWT)PUREWT"
                strSql += vbCrLf + ",-1*SUM(I.WASTAGE)WASTAGE"
                strSql += vbCrLf + ",-1*SUM(I.MCHARGE)MCHARGE"
                strSql += vbCrLf + ",I.RESNO,ISNULL(I.STKTYPE,'') STKTYPE "
                strSql += vbCrLf + "FROM " & cnStockDb & "..ISSUE I"
                strSql += vbCrLf + "WHERE "
                strSql += vbCrLf + "ISNULL(I.CANCEL,'') = '' "
                If cnCostId <> "" Then strSql += vbCrLf + " AND I.COSTID='" & cnCostId & "'"
                strSql += vbCrLf + "AND I.GRSWT <> 0 AND LEN(I.TRANTYPE) > 2 "
                strSql += vbCrLf + "  AND ISNULL(I.FLAG,'')<>'X' "
                'strSql += vbCrLf + "AND ISNULL(I.ITEMID,0)<>0 "
                strSql += vbCrLf + "AND I.RESNO IN"
                strSql += vbCrLf + "(SELECT SNO FROM " & cnStockDb & "..OPENWEIGHT "
                strSql += vbCrLf + "UNION ALL "
                strSql += vbCrLf + "SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE = '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''  AND TRANTYPE<>'PU' "
                strSql += vbCrLf + "AND TRANNO = " & Val(txtTranNo_NUM.Text) & ")"
                strSql += vbCrLf + "GROUP BY I.RESNO,I.STKTYPE "
                strSql += vbCrLf + "UNION ALL"
                strSql += vbCrLf + "SELECT "
                strSql += vbCrLf + "-1*SUM(I.PCS)PCS"
                strSql += vbCrLf + ",-1*SUM(I.GRSWT)GRSWT"
                strSql += vbCrLf + ",-1*SUM(I.NETWT)NETWT"
                strSql += vbCrLf + ",0 PUREWT"
                strSql += vbCrLf + ",0 WASTAGE"
                strSql += vbCrLf + ",0 MCHARGE"
                strSql += vbCrLf + ",I.RECSNO AS SNO,ISNULL(I.STKTYPE,'') STKTYPE "
                strSql += vbCrLf + "FROM " & cnStockDb & "..LOTISSUE I"
                strSql += vbCrLf + "WHERE "
                strSql += vbCrLf + " ISNULL(I.CANCEL,'') = '' "
                strSql += vbCrLf + "AND ISNULL(I.ITEMID,0)<>0 "
                strSql += vbCrLf + "AND I.RECSNO IN"
                strSql += vbCrLf + "(SELECT SNO FROM " & cnStockDb & "..OPENWEIGHT "
                strSql += vbCrLf + "UNION ALL "
                strSql += vbCrLf + "SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE = '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''  AND TRANTYPE<>'PU' "
                strSql += vbCrLf + "AND TRANNO = " & Val(txtTranNo_NUM.Text) & ")"
                strSql += vbCrLf + "GROUP BY I.RECSNO,I.STKTYPE "
                strSql += vbCrLf + ")X GROUP BY SNO,STKTYPE HAVING (SUM(PCS)>0 OR SUM(GRSWT)>0)"
            Else
                strSql = vbCrLf + "  SELECT I.COSTID,I.SNO,I.TRANNO,I.TRANDATE"
                strSql += vbCrLf + "  ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID)COSTNAME"
                strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)ACNAME"
                strSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
                strSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)AS SUBITEM"
                strSql += vbCrLf + "  ,(I.PCS - SUM(ISNULL(L.PCS,0))- SUM(ISNULL(R.PCS,0)))PCS"
                strSql += vbCrLf + "  ,(I.GRSWT - SUM(ISNULL(L.GRSWT,0))- SUM(ISNULL(R.GRSWT,0)))GRSWT"
                strSql += vbCrLf + "  ,(I.NETWT - SUM(ISNULL(L.NETWT,0))- SUM(ISNULL(R.NETWT,0)))NETWT"
                strSql += vbCrLf + "  ,I.TOUCH,I.WASTAGE,I.MCHARGE"
                strSql += vbCrLf + "  ,I.PUREWT PUREWT,I.ITEMID,I.ACCODE"
                'strSql += vbCrLf + "  ,NULL STNPCS,NULL STNWT"
                'strSql += vbCrLf + "  ,NULL DIAPCS,NULL DIAWT,NULL STONEUNIT"
                strSql += vbCrLf + " ,(SELECT ISNULL(SUM(ISNULL(STNPCS,0)),0) SPCS FROM " & cnStockDb & "..RECEIPTSTONE  S WHERE"
                strSql += vbCrLf + "I.SNO = S.ISSSNO And STNITEMID In (Select ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNPCS ,"
                strSql += vbCrLf + "(SELECT SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT ELSE 0 END) FROM " & cnStockDb & "..RECEIPTSTONE  S WHERE "
                strSql += vbCrLf + "I.SNO = S.ISSSNO And STNITEMID In (Select ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWT ,"
                strSql += vbCrLf + "(SELECT ISNULL(SUM(ISNULL(S.STNPCS,0)),0) DPCS FROM  " & cnStockDb & "..RECEIPTSTONE S WHERE "
                strSql += vbCrLf + "I.SNO = S.ISSSNO And STNITEMID In (Select ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAPCS ,"
                strSql += vbCrLf + "Convert(NUMERIC(15, 4), (SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT "
                strSql += vbCrLf + "From  " & cnStockDb & "..RECEIPTSTONE S Where I.SNO = S.ISSSNO And STNITEMID In (Select ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT "
                strSql += vbCrLf + ",(SELECT ISNULL(stoneunit,'') stoneunit FROM  " & cnStockDb & "..RECEIPTSTONE S WHERE "
                strSql += vbCrLf + "I.SNO = S.ISSSNO And STNITEMID In (Select ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D' or DIASTONE = 'S' ))STONEUNIT "
                strSql += vbCrLf + "  ,NULL LSPCS,NULL LSWT,NULL LDPCS,NULL LDWT"
                strSql += vbCrLf + "  ,I.CUTID,I.COLORID,I.CLARITYID,I.SETTYPEID,I.SHAPEID,I.HEIGHT,I.WIDTH,I.STKTYPE,I.RATE"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I"
                strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..LOTISSUE AS L ON L.TRANNO = I.TRANNO AND L.TRANDATE = I.TRANDATE  AND L.RECSNO = I.SNO"
                strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..ISSUE AS R ON R.JOBNO=I.JOBNO"
                strSql += vbCrLf + "  WHERE I.TRANDATE = '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "' AND (I.GRSWT <> 0 or I.PCS <> 0)"
                If cnCostId <> "" Then strSql += vbCrLf + " AND I.COSTID='" & cnCostId & "'"
                strSql += vbCrLf + "  AND ISNULL(JOBISNO,'') NOT IN(SELECT RECSNO FROM " & cnStockDb & "..LOTISSUE)"
                strSql += vbCrLf + "  AND NOT EXISTS(SELECT 1 FROM  " & cnStockDb & "..ISSUE WHERE BATCHNO=I.BATCHNO)"
                strSql += vbCrLf + "  AND I.TRANNO = " & Val(txtTranNo_NUM.Text) & ""
                strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = '' AND ISNULL(L.CANCEL,'') = ''"
                strSql += vbCrLf + "  AND ISNULL(I.FLAG,'')<>'X' "
                strSql += vbCrLf + GetLoadedSno()
                strSql += vbCrLf + "  GROUP BY I.COSTID,I.TRANNO,I.TRANDATE,I.ACCODE,I.ITEMID,I.SUBITEMID,I.TOUCH,I.PUREWT,I.SNO,I.WASTAGE,I.MCHARGE,I.PCS,I.GRSWT,I.NETWT   "
                strSql += vbCrLf + "  ,I.CUTID,I.COLORID,I.CLARITYID,I.SETTYPEID,I.SHAPEID,I.HEIGHT,I.WIDTH,I.STKTYPE,I.RATE"
                strSql += vbCrLf + "  HAVING (I.PCS - SUM(ISNULL(L.PCS,0))- SUM(ISNULL(R.PCS,0)) > 0   OR I.GRSWT - SUM(ISNULL(L.GRSWT,0))- SUM(ISNULL(R.GRSWT,0)) > 0)"
            End If
            da = New OleDbDataAdapter(strSql, cn)
            Dim dtBalance As New DataTable
            da.Fill(dtBalance)
            If dtBalance.Rows.Count > 0 Then
                If dtBalance.Rows.Count = 1 Then
                    If HideAccLink = False Then
                        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACCODE,'') = '" & dtBalance.Rows(0).Item("ACCODE").ToString & "'"
                        Dim desName As String = objGPack.GetSqlValue(strSql)
                        'Manepally
                        'If desName = "" Then
                        '    MsgBox(dtBalance.Rows(0).Item("ACNAME").ToString & vbCrLf & "Designer info not found", MsgBoxStyle.Information)
                        '    Exit Sub
                        'End If
                        cmbSupplier_Man.Text = desName
                    End If
                    LoadDefaultLotValues(dtBalance.Rows(0))
                Else ''more than one receipt items
                    Dim retRow As DataRow = BrighttechPack.SearchDialog.Show_R("Search Item", strSql, cn)
                    If retRow Is Nothing Then Exit Sub
                    If HideAccLink = False Then
                        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACCODE,'') = '" & retRow.Item("ACCODE").ToString & "'"
                        Dim desName As String = objGPack.GetSqlValue(strSql)
                        'Manepally
                        'If desName = "" Then
                        '    MsgBox(retRow.Item("ACNAME").ToString & vbCrLf & "Designer info not found", MsgBoxStyle.Information)
                        '    Exit Sub
                        'End If
                        cmbSupplier_Man.Text = desName
                    End If
                    LoadDefaultLotValues(retRow)
                End If
            Else
                grpPending.Visible = False
                lblPendingPcs.Text = ""
                lblPendingWeight.Text = ""
                MsgBox("Record not found", MsgBoxStyle.Information)
                txtTranNo_NUM.Focus()
                Exit Sub
            End If
            If dtBalance.Rows.Count > 0 Then
                'txtItemCode_Num_Man.Enabled = False
                'cmbSubItemName_Man.Enabled = False
                txtPiece_Num_Man.Text = lblPendingPcs.Text
                txtGrossWeight_Wet_MAN.Text = lblPendingWeight.Text
                SendKeys.Send("{tab}")
                btnAdd.Enabled = True
            End If
        End If
    End Sub
    Private Sub LoadDefaultLotValues(ByVal row As DataRow)
        Dim exprs As String = "RECSNO = '" & row.Item("SNO").ToString & "'"
        Dim entpcs As Double = 0
        Dim entgwt As Double = 0
        Dim entstnpcs As Double = 0
        Dim entstnwt As Double = 0
        Dim entdiapcs As Double = 0
        Dim entdiawt As Double = 0
        If itemDt.Rows.Count > 0 Then
            entpcs = Val("" & itemDt.Compute("SUM(computedpcs)", exprs))
            entgwt = Val("" & itemDt.Compute("Sum(computedwt)", exprs))
            entstnpcs = Val("" & itemDt.Compute("SUM(computedstnpcs)", exprs))
            entstnwt = Val("" & itemDt.Compute("Sum(computedstnwt)", exprs))
            entdiapcs = Val("" & itemDt.Compute("SUM(computeddiapcs)", exprs))
            entdiawt = Val("" & itemDt.Compute("Sum(computeddiawt)", exprs))
        End If
        recSno = row.Item("SNO").ToString
        txtTranNo_NUM.Text = row.Item("TRANNO").ToString
        dtpTranDate.Value = row.Item("TRANDATE").ToString
        txtPiece_Num_Man.Text = Val(row.Item("PCS").ToString) - entpcs
        txtGrossWeight_Wet_MAN.Text = Val(row.Item("GRSWT").ToString) - entgwt
        txtNetWeight_Wet_MAN.Text = Val(row.Item("NETWT").ToString) - entgwt
        txtItemCode_Num_Man.Text = row.Item("ITEMID").ToString
        txtItemName.Text = row.Item("ITEM").ToString
        cmbSubItemName_Man.Text = row.Item("SUBITEM").ToString
        cmbCostCentre_Man.Text = row.Item("COSTNAME").ToString
        lblPendingPcs.Text = Val(row.Item("PCS").ToString) - entpcs
        lblPendingWeight.Text = Format(Val(row.Item("GRSWT").ToString) - entgwt, "0.000")
        txtTuch_WET.Text = IIf(Val(row.Item("TOUCH").ToString) <> 0, Format(Val(row.Item("TOUCH").ToString), "0.000"), "")
        txtFineRate_Amt.Text = IIf(Val(row.Item("RATE").ToString) <> 0, Format(Val(row.Item("RATE").ToString), "0.00"), "")
        txtStonePieces_Num.Text = IIf(Val(row.Item("STNPCS").ToString) = 0, 0, Val(row.Item("STNPCS").ToString)) - IIf(Val(row.Item("LSPCS").ToString) = 0, 0, Val(row.Item("LSPCS").ToString)) - entstnpcs
        txtStoneWeight_Wet.Text = IIf(Val(row.Item("STNWT").ToString) = 0, 0, Val(row.Item("STNWT").ToString)) - IIf(Val(row.Item("LSWT").ToString) = 0, 0, Val(row.Item("LSWT").ToString)) - entstnwt
        txtDiamondPieces_Num.Text = IIf(Val(row.Item("DIAPCS").ToString) = 0, 0, Val(row.Item("DIAPCS").ToString)) - IIf(Val(row.Item("LDPCS").ToString) = 0, 0, Val(row.Item("LDPCS").ToString)) - entdiapcs
        txtDidmondWeight_Wet.Text = IIf(Val(row.Item("DIAWT").ToString) = 0, 0, Val(row.Item("DIAWT").ToString)) - IIf(Val(row.Item("LDWT").ToString) = 0, 0, Val(row.Item("LDWT").ToString)) - entdiawt
        If row.Item("STKTYPE").ToString = "M" Then
            cmbStkType.Text = "MANUFACTURE"
        ElseIf row.Item("STKTYPE").ToString = "T" Then
            cmbStkType.Text = "TRADING"
        Else
            cmbStkType.Text = ""
        End If
        If STOCKVALIDATION = False Then
            txtCut.Text = Val(row.Item("CUTID").ToString)
            txtColor.Text = Val(row.Item("COLORID").ToString)
            txtClarity.Text = Val(row.Item("CLARITYID").ToString)
            txtShape.Text = Val(row.Item("SHAPEID").ToString)
            txtSetType.Text = Val(row.Item("SETTYPEID").ToString)
            txtHt.Text = Val(row.Item("HEIGHT").ToString)
            txtWdh.Text = Val(row.Item("WIDTH").ToString)
        End If
        Dim StnUnit As String = "G"
        If Val(row.Item("STNWT").ToString) = 0 Then
            strSql = "SELECT STONEUNIT FROM " & cnStockDb & "..RECEIPT WHERE SNO='" & recSno & "'"
            StnUnit = objGPack.GetSqlValue(strSql, "STONEUNIT", "G")
        End If
        cmbStoneUnit.Text = IIf(StnUnit = "G", "GRAM", "CARAT")
        Dim valu As Decimal = Nothing
        If Val(row.Item("grswt").ToString) > 0 Then valu = IIf(Val(row.Item("WASTAGE").ToString), Val(row.Item("WASTAGE").ToString) / Val(row.Item("GRSWT").ToString), 0) * 100
        txtWastage_Per.Text = IIf(valu <> 0, Format(valu, "0.000"), "")
        If Val(row.Item("grswt").ToString) > 0 Then valu = IIf(Val(row.Item("MCHARGE").ToString), Val(row.Item("MCHARGE").ToString) / Val(row.Item("GRSWT").ToString), 0)
        txtMakingPerGrm_Amt.Text = IIf(valu <> 0, Format(valu, "0.00"), "")
        grpPending.Visible = True
        strSql = " select TableCode from " & cnAdminDb & "..itemMast where ITEMID= '" & txtItemCode_Num_Man.Text & "' and TableCode <> ''"
        Dim dtTableCode As New DataTable
        dtTableCode.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTableCode)
        If Not dtTableCode.Rows.Count > 0 Then
            cmbTableCode.Enabled = False
        End If
        strSql = " SELECT OM.ORTYPE FROM " & cnAdminDb & "..ORIRDETAIL OI"
        strSql += " JOIN " & cnAdminDb & "..ORMAST OM ON SUBSTRING(OM.ORNO,6,20)=SUBSTRING(OI.ORNO,6,20)"
        strSql += " WHERE OI.BATCHNO =(SELECT TOP 1 BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE SNO='" & row.Item("SNO").ToString & "')"
        Dim OrType As String = objGPack.GetSqlValue(strSql, "ORTYPE", "").ToString
        If OrType = "O" Then cmbEntryType.Text = "ORDER"
        If OrType = "R" Then cmbEntryType.Text = "REPAIR"
        If cmbCostCentre_Man.Text <> "" And STOCKVALIDATION Then cmbCostCentre_Man.Enabled = False
        If Val(txtItemCode_Num_Man.Text) > 0 Then
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & txtItemCode_Num_Man.Text & "' AND ISNULL(HALLMARK,'Y') = 'Y'"
            If objGPack.GetSqlValue(strSql).Length > 0 Then
                CmbHallmark.Text = "YES"
            Else
                CmbHallmark.Text = "NO"
            End If
        End If
    End Sub

    Private Sub txtJobno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtJobno.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT SUBSTRING(JOBNO,6,LEN(I.JOBNO)) AS JOBNO"
            strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=I.ACCODE)NAME"
            strSql += vbCrLf + "  ,I.TRANDATE,I.TRANNO FROM " & cnStockDb & "..RECEIPT I  "
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..LOTISSUE AS L ON L.TRANNO = I.TRANNO AND L.TRANDATE = I.TRANDATE  AND L.RECSNO = I.SNO"
            strSql += vbCrLf + "  WHERE I.GRSWT <> 0 AND ISNULL(I.CANCEL,'') = '' AND ISNULL(L.CANCEL,'') = '' AND ISNULL(I.JOBNO,'')<>''"
            strSql += vbCrLf + GetLoadedSno()
            strSql += vbCrLf + "  GROUP BY I.COSTID,I.TRANNO,I.TRANDATE,I.JOBNO,I.PCS,I.GRSWT,I.ACCODE "
            strSql += vbCrLf + "  HAVING (I.PCS - SUM(ISNULL(L.PCS,0)) > 0   OR I.GRSWT - SUM(ISNULL(L.GRSWT,0)) > 0)"
            Dim retRow As DataRow = BrighttechPack.SearchDialog.Show_R("Select Jobno", strSql, cn)
            If retRow Is Nothing Then Exit Sub
            txtJobno.Text = retRow.Item("JOBNO").ToString
            txtTranNo_NUM.Text = retRow.Item("TRANNO").ToString
            dtpTranDate.Value = retRow.Item("TRANDATE")
            txtTranNo_NUM_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
            Label1.Focus()
            'Me.SelectNextControl(txtJobno, True, True, True, True)
        End If
    End Sub

    Private Sub txtJobno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtJobno.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT I.TRANDATE,I.TRANNO FROM " & cnStockDb & "..RECEIPT I  "
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..LOTISSUE AS L ON L.TRANNO = I.TRANNO AND L.TRANDATE = I.TRANDATE  AND L.RECSNO = I.SNO"
            strSql += vbCrLf + "  WHERE SUBSTRING(JOBNO,6,LEN(JOBNO))='" & txtJobno.Text & "' AND (I.GRSWT <> 0 or I.PCS <> 0 )"
            strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = '' AND ISNULL(L.CANCEL,'') = ''"
            strSql += vbCrLf + GetLoadedSno()
            strSql += vbCrLf + "  GROUP BY I.COSTID,I.TRANNO,I.TRANDATE,I.PCS,I.GRSWT "
            strSql += vbCrLf + "  HAVING (I.PCS - SUM(ISNULL(L.PCS,0)) > 0   OR I.GRSWT - SUM(ISNULL(L.GRSWT,0)) > 0)"
            Dim DR As DataRow
            DR = GetSqlRow(strSql, cn)
            If Not DR Is Nothing Then
                txtTranNo_NUM.Text = DR.Item("TRANNO").ToString
                dtpTranDate.Value = DR.Item("TRANDATE")
                txtTranNo_NUM_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
                'Me.SelectNextControl(txtTranNo_NUM, True, True, True, True)
            Else
                MsgBox("No record found", MsgBoxStyle.Information)
                txtJobno.Focus()
            End If
        End If
    End Sub

    Private Sub cmbAcName_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbAcName.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbAcName.Text = "" Or cmbAcName.Text = "All" Then SendKeys.Send("{TAB}") : Exit Sub
            If STOCKVALIDATION Then
                strSql = vbCrLf + "SELECT SNO "
                strSql += vbCrLf + ",(SELECT TOP 1 COSTID FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)COSTID"
                strSql += vbCrLf + ",(SELECT TOP 1 TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TRANNO"
                strSql += vbCrLf + ",(SELECT TOP 1 TRANDATE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TRANDATE"
                strSql += vbCrLf + ",(SELECT TOP 1 COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID =  "
                strSql += vbCrLf + "(SELECT TOP 1 COSTID FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))COSTNAME"
                strSql += vbCrLf + ",(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE="
                strSql += vbCrLf + "(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))ACNAME"
                strSql += vbCrLf + ",(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST  WHERE ITEMID ="
                strSql += vbCrLf + "(SELECT TOP 1 ITEMID FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))ITEM"
                strSql += vbCrLf + ",(SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST  WHERE SUBITEMID ="
                strSql += vbCrLf + "(SELECT TOP 1 SUBITEMID FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO))SUBITEM"
                strSql += vbCrLf + ",SUM(PCS)PCS,SUM(GRSWT)GRSWT"
                strSql += vbCrLf + ",SUM(NETWT)NETWT,SUM(PUREWT)PUREWT"
                strSql += vbCrLf + ",SUM(WASTAGE)WASTAGE,SUM(MCHARGE)MCHARGE"
                strSql += vbCrLf + ",(SELECT TOP 1 TOUCH FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TOUCH"
                strSql += vbCrLf + ",(SELECT TOP 1 ITEMID FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)ITEMID"
                strSql += vbCrLf + ",(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)ACCODE"
                strSql += vbCrLf + ",(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=X.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S') )STNPCS"
                strSql += vbCrLf + ",(SELECT SUM(CASE WHEN STONEUNIT='C' THEN (STNWT/5) ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=X.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')  )STNWT"
                strSql += vbCrLf + ",(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=X.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') )DIAPCS"
                strSql += vbCrLf + ",(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=X.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') )DIAWT"
                strSql += vbCrLf + ",(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=(SELECT TOP 1 LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO=X.SNO))LSPCS"
                strSql += vbCrLf + ",(SELECT SUM(CASE WHEN STNUNIT='C' THEN (STNWT/5) ELSE STNWT END) FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=(SELECT TOP 1 LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO=X.SNO))LSWT"
                strSql += vbCrLf + ",(SELECT SUM(DIAPCS) FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=(SELECT TOP 1 LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO=X.SNO))LDPCS"
                strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=(SELECT TOP 1 LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE RECSNO=X.SNO))LDWT"
                strSql += vbCrLf + ",'G' AS STONEUNIT,STKTYPE "
                strSql += vbCrLf + ",(SELECT TOP 1 RATE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)RATE "
                strSql += vbCrLf + "FROM ("
                strSql += vbCrLf + "SELECT "
                strSql += vbCrLf + "SUM(R.PCS)PCS"
                strSql += vbCrLf + ",SUM(R.GRSWT)GRSWT"
                strSql += vbCrLf + ",SUM(R.NETWT)NETWT"
                strSql += vbCrLf + ",SUM(R.PUREWT)PUREWT"
                strSql += vbCrLf + ",0 WASTAGE"
                strSql += vbCrLf + ",0 MCHARGE"
                strSql += vbCrLf + ",R.SNO,'' STKTYPE "
                strSql += vbCrLf + "FROM " & cnStockDb & "..OPENWEIGHT R WHERE 1=1"
                If cnCostId <> "" Then strSql += vbCrLf + " AND R.COSTID='" & cnCostId & "'"
                strSql += vbCrLf + "AND R.GRSWT <> 0 AND STOCKTYPE='C' "
                strSql += vbCrLf + Replace(GetLoadedSno(), "I.SNO", "R.SNO")
                strSql += vbCrLf + "GROUP BY R.SNO"
                strSql += vbCrLf + "UNION ALL"
                strSql += vbCrLf + "SELECT "
                strSql += vbCrLf + "SUM(R.PCS)PCS"
                strSql += vbCrLf + ",SUM(R.GRSWT)GRSWT"
                strSql += vbCrLf + ",SUM(R.NETWT)NETWT"
                strSql += vbCrLf + ",SUM(R.PUREWT)PUREWT"
                strSql += vbCrLf + ",SUM(R.WASTAGE)WASTAGE"
                strSql += vbCrLf + ",SUM(R.MCHARGE)MCHARGE"
                strSql += vbCrLf + ",R.SNO,ISNULL(R.STKTYPE,'')STKTYPE "
                strSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT R"
                strSql += vbCrLf + "WHERE R.TRANDATE <= '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + "AND ISNULL(R.CANCEL,'') = '' "
                strSql += vbCrLf + "AND R.GRSWT >= 0 AND LEN(R.TRANTYPE) > 2 "
                strSql += vbCrLf + "AND ISNULL(R.FLAG,'')<>'X' "
                strSql += vbCrLf + Replace(GetLoadedSno(), "I.SNO", "R.SNO")
                If cnCostId <> "" Then strSql += vbCrLf + " AND R.COSTID='" & cnCostId & "'"
                strSql += vbCrLf + "GROUP BY R.SNO,R.STKTYPE"
                strSql += vbCrLf + "UNION ALL"
                strSql += vbCrLf + "SELECT "
                strSql += vbCrLf + "-1*SUM(I.PCS)PCS"
                strSql += vbCrLf + ",-1*SUM(I.GRSWT)GRSWT"
                strSql += vbCrLf + ",-1*SUM(I.NETWT)NETWT"
                strSql += vbCrLf + ",-1*SUM(I.PUREWT)PUREWT"
                strSql += vbCrLf + ",-1*SUM(I.WASTAGE)WASTAGE"
                strSql += vbCrLf + ",-1*SUM(I.MCHARGE)MCHARGE"
                strSql += vbCrLf + ",I.RESNO,ISNULL(I.STKTYPE,'')STKTYPE "
                strSql += vbCrLf + "FROM " & cnStockDb & "..ISSUE I"
                strSql += vbCrLf + "WHERE "
                strSql += vbCrLf + "ISNULL(I.CANCEL,'') = '' "
                If cnCostId <> "" Then strSql += vbCrLf + " AND I.COSTID='" & cnCostId & "'"
                strSql += vbCrLf + "AND I.GRSWT >= 0 AND LEN(I.TRANTYPE) > 2 "
                strSql += vbCrLf + "AND ISNULL(I.FLAG,'')<>'X' "
                strSql += vbCrLf + "AND I.RESNO IN"
                strSql += vbCrLf + "(SELECT SNO FROM " & cnStockDb & "..OPENWEIGHT "
                strSql += vbCrLf + "UNION ALL "
                strSql += vbCrLf + "SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE <= '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''  AND TRANTYPE<>'PU' )"
                strSql += vbCrLf + "GROUP BY I.RESNO,I.STKTYPE "
                strSql += vbCrLf + "UNION ALL"
                strSql += vbCrLf + "SELECT "
                strSql += vbCrLf + "-1*SUM(I.PCS)PCS"
                strSql += vbCrLf + ",-1*SUM(I.GRSWT)GRSWT"
                strSql += vbCrLf + ",-1*SUM(I.NETWT)NETWT"
                strSql += vbCrLf + ",0 PUREWT"
                strSql += vbCrLf + ",0 WASTAGE"
                strSql += vbCrLf + ",0 MCHARGE"
                strSql += vbCrLf + ",I.RECSNO AS SNO,ISNULL(I.STKTYPE,'')STKTYPE "
                strSql += vbCrLf + "FROM " & cnStockDb & "..LOTISSUE I"
                strSql += vbCrLf + "WHERE "
                strSql += vbCrLf + "ISNULL(I.CANCEL,'') = '' "
                strSql += vbCrLf + "AND ISNULL(I.ITEMID,0)<>0 "
                strSql += vbCrLf + "AND I.RECSNO IN"
                strSql += vbCrLf + "(SELECT SNO FROM " & cnStockDb & "..OPENWEIGHT "
                strSql += vbCrLf + "UNION ALL "
                strSql += vbCrLf + "SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE <= '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''  AND TRANTYPE<>'PU' )"
                strSql += vbCrLf + "GROUP BY I.RECSNO,I.STKTYPE "
                strSql += vbCrLf + ")X "
                strSql += vbCrLf + " WHERE "
                strSql += vbCrLf + " (SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE="
                strSql += vbCrLf + " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO = X.SNO)) = '" & cmbAcName.Text.ToString & "'"
                strSql += vbCrLf + " GROUP BY SNO,STKTYPE HAVING (SUM(PCS)>0 Or SUM(GRSWT)>0)"
                strSql += vbCrLf + " ORDER BY TRANDATE,TRANNO"
            Else
                strSql = vbCrLf + "  SELECT I.SNO,I.COSTID,I.TRANNO,I.TRANDATE"
                strSql += vbCrLf + "  ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID)COSTNAME"
                strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)ACNAME"
                strSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
                strSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID And SUBITEMID = I.SUBITEMID)AS SUBITEM"
                strSql += vbCrLf + "  ,(I.PCS - SUM(ISNULL(L.PCS,0)))PCS"
                strSql += vbCrLf + "  ,(I.GRSWT - SUM(ISNULL(L.GRSWT,0)))GRSWT"
                strSql += vbCrLf + "  ,(I.NETWT - SUM(ISNULL(L.NETWT,0)))NETWT"
                strSql += vbCrLf + "  ,I.TOUCH,I.WASTAGE,I.MCHARGE"
                strSql += vbCrLf + "  ,I.PUREWT PUREWT,I.ITEMID,I.ACCODE"
                strSql += vbCrLf + " ,(SELECT ISNULL(SUM(ISNULL(STNPCS,0)),0) SPCS FROM " & cnStockDb & "..RECEIPTSTONE  S WHERE"
                strSql += vbCrLf + " I.SNO = S.ISSSNO And STNITEMID In (Select ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNPCS ,"
                strSql += vbCrLf + " (SELECT SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT ELSE 0 END) FROM " & cnStockDb & "..RECEIPTSTONE  S WHERE "
                strSql += vbCrLf + " I.SNO = S.ISSSNO And STNITEMID In (Select ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWT ,"
                strSql += vbCrLf + " (SELECT ISNULL(SUM(ISNULL(S.STNPCS,0)),0) DPCS FROM  " & cnStockDb & "..RECEIPTSTONE S WHERE "
                strSql += vbCrLf + " I.SNO = S.ISSSNO And STNITEMID In (Select ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAPCS ,"
                strSql += vbCrLf + " Convert(NUMERIC(15, 4), (SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT "
                strSql += vbCrLf + " From  " & cnStockDb & "..RECEIPTSTONE S Where I.SNO = S.ISSSNO And STNITEMID In (Select ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT "
                strSql += vbCrLf + "  ,NULL LSPCS,NULL LSWT,NULL LDPCS,NULL LDWT"
                strSql += vbCrLf + "  ,I.CUTID,I.COLORID,I.CLARITYID,I.SETTYPEID,I.SHAPEID,I.HEIGHT,I.WIDTH,I.STKTYPE,I.RATE"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I"
                strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..LOTISSUE AS L ON L.TRANNO = I.TRANNO AND L.TRANDATE = I.TRANDATE AND L.RECSNO = I.SNO"
                strSql += vbCrLf + "  WHERE i.TRANDATE <= '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "' AND (I.GRSWT <> 0 OR I.PCS <> 0)"
                If cnCostId <> "" Then strSql += vbCrLf + " AND I.COSTID='" & cnCostId & "'"
                strSql += vbCrLf + "  AND I.ACCODE='" & cmbAcName.SelectedValue & "'"
                strSql += vbCrLf + "  AND LEN(TRANTYPE)>2"
                strSql += vbCrLf + "  AND ISNULL(JOBISNO,'') NOT IN(SELECT RECSNO FROM " & cnStockDb & "..LOTISSUE)"
                strSql += vbCrLf + "  AND NOT EXISTS(SELECT 1 FROM  " & cnStockDb & "..ISSUE WHERE BATCHNO=I.BATCHNO)"
                strSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = '' AND ISNULL(L.CANCEL,'') = ''"
                strSql += vbCrLf + "  AND ISNULL(I.FLAG,'')<>'X' "
                strSql += vbCrLf + GetLoadedSno()
                strSql += vbCrLf + "  GROUP BY I.TRANNO,I.TRANDATE,I.ACCODE,I.ITEMID,I.SUBITEMID,I.TOUCH,I.PUREWT,I.SNO,I.COSTID,I.WASTAGE,I.MCHARGE,I.PCS,I.GRSWT,I.NETWT"
                strSql += vbCrLf + "  ,I.CUTID,I.COLORID,I.CLARITYID,I.SETTYPEID,I.SHAPEID,I.HEIGHT,I.WIDTH,I.STKTYPE,I.RATE "
                strSql += vbCrLf + "  HAVING (I.PCS - SUM(ISNULL(L.PCS,0)) > 0   OR I.GRSWT - SUM(ISNULL(L.GRSWT,0)) > 0)"
            End If
            Dim retRow As DataRow = BrighttechPack.SearchDialog.Show_R("Select Tranno", strSql, cn)
            If retRow Is Nothing Then Exit Sub
            If HideAccLink = False Then
                strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACCODE,'') = '" & retRow.Item("ACCODE").ToString & "'"
                Dim desName As String = objGPack.GetSqlValue(strSql)
                cmbSupplier_Man.Text = desName
            End If
            LoadDefaultLotValues(retRow)
            txtPiece_Num_Man.Text = lblPendingPcs.Text
            txtGrossWeight_Wet_MAN.Text = lblPendingWeight.Text
            btnAdd.Enabled = True
            Me.SelectNextControl(txtTranNo_NUM, True, True, True, True)
        End If
    End Sub
End Class