Imports System.Data.OleDb
Public Class frmLotEntry
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
    Dim nonTagItemCtrId As Integer
    Dim locUpdateRow As Integer = -1

    Dim tagMade As Boolean = False
    Dim cPcs As Integer = -1
    Dim cWeight As Double = -1
    Dim updSno As String = Nothing
    Dim updLotNo As Integer = -1
    Dim tabCheckBy As String = GetAdmindbSoftValue("LOTCHECKBY", "P")

    Dim PrintLot As Boolean = IIf(GetAdmindbSoftValue("PRINT_LOT", "N") = "Y", True, False)

    Dim Tag_Tolerance As Decimal = Val(GetAdmindbSoftValue("TAGTOLERANCE", "0"))
    Dim WTITEMBULKTAG As Boolean = IIf(GetAdmindbSoftValue("WTITEMBULKTAG", "N") = "Y", True, False)
    Dim STKAFINDATE As Boolean = IIf(GetAdmindbSoftValue("STKAFINDATE", "N") = "Y", True, False)
    Dim CHKCOST_ORD As Boolean = IIf(GetAdmindbSoftValue("ORD_CHK_COST", "N") = "Y", True, False)
    Dim ORD_REP_COUNTERIDS As String = GetAdmindbSoftValue("ORD_REP_COUNTERID", "")
    Dim ORDERCOUNTERID As Integer = 0
    Dim REPAIRCOUNTERID As Integer = 0
    Dim LOTOPTIONRESTRICT As String = GetAdmindbSoftValue("LOT_OPT_RESTRICT", "")
    Dim AZP As Boolean
    Dim dtGridViewTotal As New DataTable
    Dim OrdRepairNo As String
    Dim OrdCostId As String
    Dim OrdSNO As String
    Dim OrdCompanyId As String
    Dim MoveOrder As Boolean = False
    Dim objStone As frmStoneDia
    Dim dtgridstone As New DataTable
    Dim RangeInLot As Boolean = IIf(GetAdmindbSoftValue("RANGEINLOT", "N") = "Y", True, False)
    Dim TAG_LOT_ALLOW_COSTID As String = GetAdmindbSoftValue("TAG_ALLOW_COSTID", "")
    Dim LOT_LESS_OPTION As Boolean = IIf(GetAdmindbSoftValue("LOT_LESS_OPTION", "N") = "Y", True, False)
    Dim LOT_STKTYPE_AUTO As Boolean = IIf(GetAdmindbSoftValue("LOT_STKTYPE_AUTO", "Y") = "Y", True, False)
    Dim TAGWISE_DISCOUNT As Boolean = IIf(GetAdmindbSoftValue("TAGWISE_DISCOUNT", "N") = "Y", True, False)
    Dim LOT_TRANSFER_COSTID As String = GetAdmindbSoftValue("LOT_TRANSFER_COSTID", "")
    Dim LOT_STKTYPE_DEFAULT As String = GetAdmindbSoftValue("LOT_STKTYPE_DEFAULT", "")
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "")
    Dim TAGEDITCOSTCENTRE As Boolean = IIf(GetAdmindbSoftValue("TAGEDITCOSTCENTRE", "N") = "Y", True, False)

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
    End Sub
    Function funcNew_Less() As Integer
        txtPiece_Num_Man.Clear()
        txtNoOfTags_Num.Clear()
        txtGrossWeight_Wet.Clear()
        txtStonePieces_Num.Clear()
        txtStoneWeight_Wet.Clear()
        txtDiamondPieces_Num.Clear()
        txtDidmondWeight_Wet.Clear()
        txtNetWeight_Wet.Clear()
        txtPiece_Num_Man.Focus()
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        If strBCostid <> Nothing Then
            strSql = " SELECT MAIN FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE ISNULL(ACTIVE,'') <> 'N' AND COSTID = '" & cnCostId & "'"
            If GetAdmindbSoftValue("BRANCHTAG", "N") = "N" And objGPack.GetSqlValue(strSql, , "N") = "N" Then
                MsgBox("Taged entry cannot allow at location", MsgBoxStyle.Information)
                'txtLotNo_Num_Man.Enabled = False
                If Me.Focused = True Then Me.Close()
                Return 0
            End If
        End If
        lblNTOrder.Visible = False
        txtOrderRepairNo.Enabled = True
        If ORD_REP_COUNTERIDS <> "" Then
            Dim ORD_REP_COUNTERIDS_ARR() As String = ORD_REP_COUNTERIDS.Split(",")
            ORDERCOUNTERID = Val(ORD_REP_COUNTERIDS_ARR(0).ToString)
            If ORD_REP_COUNTERIDS_ARR.Length > 0 Then REPAIRCOUNTERID = Val(ORD_REP_COUNTERIDS_ARR(1).ToString)
        End If
        objStone = New frmStoneDia

        cmbEntryType.Enabled = True
        pnlNonTag.Visible = False
        txtNonTagItem.Clear()
        txtNonTagSubItem.Clear()
        txtNonTagPcs.Clear()
        txtNonTagGrsWt.Clear()

        cmbSubItemName_Man.Text = ""
        cmbSubItemName_Man.Items.Clear()
        cmbSubItemName_Man.Enabled = False

        cmbItemRange_OWN.Items.Clear()

        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbSupplier_Man_OWN)

        cmbOpenDesignerName.Items.Clear()
        cmbOpenDesignerName.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbOpenDesignerName, False)
        cmbOpenDesignerName.Text = "ALL"

        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY COSTNAME"
        CmbOpenCostcentre.Items.Clear()
        CmbOpenCostcentre.Items.Add("ALL")
        objGPack.FillCombo(strSql, CmbOpenCostcentre, False)
        CmbOpenCostcentre.Text = "ALL"

        cmbCostCentre_Man.Text = ""
        OrdRepairNo = ""
        OrdCostId = ""
        OrdCompanyId = ""
        If cmbCostCentre_Man.Enabled = True Then
            funcCostCentre()
        End If
        cmbItemCounter_MAN.Text = ""
        If cmbItemCounter_MAN.Enabled = True Then
            Dim dt As New DataTable
            dt.Clear()
            cmbItemCounter_MAN.Items.Clear()
            cmbItemCounter_MAN.Items.Add(" ")
            strSql = " select ItemCtrName from " & cnAdminDb & "..ItemCounter WHERE ACTIVE = 'Y' order by displayorder,ItemCtrName"
            objGPack.FillCombo(strSql, cmbItemCounter_MAN, False)
            cmbItemCounter_MAN.Enabled = True
            cmbItemCounter_MAN.Text = ""
        Else
            cmbItemCounter_MAN.Enabled = False
            cmbItemCounter_MAN.Text = ""
        End If

        CmbHallmark.Items.Clear()
        CmbHallmark.Items.Add("YES")
        CmbHallmark.Items.Add("NO")
        CmbHallmark.Text = "NO"

        funcDefauleValues()
        If STKAFINDATE = False Then
            dtpDate.Value = GetEntryDate(GetServerDate)
        Else
            dtpDate.MinimumDate = (New DateTimePicker).MinDate
        End If

        'pnlPurchaseEntry.Enabled = True
        btnSave.Enabled = False
        cmbEntryType.Focus()
    End Function
    Function funcOpen() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        tabMain.SelectedTab = tabView
        txtOpenLotNo.Clear()
        txtOpenItemName.Clear()
        dtpFrom.Value = GetServerDate(tran)
        dtpTo.Value = GetServerDate(tran)
        gridViewOpen.DataSource = Nothing
        txtOpenLotNo.Focus()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If STKAFINDATE = False Then
            If Not CheckDate(dtpDate.Value) Then Exit Function
            If CheckEntryDate(dtpDate.Value) Then Exit Function
        End If
        If flagUpdate = False Then
            funcAdd()
        Else
            funcUpdate()
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
                    Dim mFromItemid As Integer
                    Dim entryType As String = Nothing
                    Select Case .Item("ENTRYTYPE").ToString.ToUpper
                        Case "REGULAR"
                            entryType = "R"
                        Case "ORDER"
                            entryType = "OR"
                        Case "REPAIR"
                            entryType = "RE"
                        Case "WORK ORDER"
                            entryType = "WO"
                        Case "ALTERATION"
                            entryType = "AL"
                        Case "PARTLY SALE"
                            entryType = "PS"
                        Case "OLD"
                            entryType = "OO"
                        Case "SALES RETURN"
                            entryType = "SR"
                        Case Else 'NONTAG TO TAG
                            entryType = "NT"
                            mFromitemid = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & itemDt.Rows(cnt).Item("NONTAGITEM").ToString & "'", , , tran)) & "" 'ItemId
                    End Select
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMLOT"
                    strSql += " ("
                    strSql += " SNO,ENTRYTYPE,LOTDATE,LOTNO,DESIGNERID,TRANINVNO,"
                    strSql += " BILLNO,COSTID,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
                    strSql += " ITEMID,SUBITEMID,PCS,GRSWT,STNPCS,STNWT,STNUNIT,"
                    strSql += " DIAPCS,DIAWT,NETWT,NOOFTAG,RATE,ITEMCTRID,WMCTYPE,"
                    strSql += " BULKLOT,MULTIPLETAGS,NARRATION,STYLENO,FINERATE,SALEPER,TUCH,"
                    strSql += " WASTPER,MCGRM,OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
                    strSql += " COMPANYID,CPCS,CGRSWT,COMPLETED,CANCEL,VATEXM,"
                    strSql += " ACCESSING,USERID,UPDATED,"
                    strSql += " UPTIME,SYSTEMID,APPVER,TABLECODE,FROMITEMID,OPENTIME,RANGESNO,STKTYPE,WASTDISCPER,HALLMARK)VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ITEMLOTCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                    strSql += " ,'" & IIf(.Item("TOORDER").ToString = "Y", "OR", entryType) & "'" 'ENTRYTYPE
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
                    strSql += " ,'" & Mid(.Item("STNUNIT").ToString, 1, 1) & "'" 'STNUNIT
                    strSql += " ," & Val(.Item("DIAPCS").ToString) & "" 'DIAPCS
                    strSql += " ," & Val(.Item("DIAWT").ToString) & "" 'DIAWT
                    strSql += " ," & Val(.Item("NETWT").ToString) & "" 'NETWT
                    strSql += " ," & Val(.Item("NOOFTAG").ToString) & "" 'NOOFTAG
                    strSql += " ," & Val(.Item("RATE").ToString) & "" 'RATE
                    strSql += " ," & Val(itemCounterId) & "" 'ITEMCTRID
                    strSql += " ,'" & Mid(.Item("VALUEADDEDTYPE").ToString, 1, 1) & "'" 'WMCTYPE
                    strSql += " ,'" & Mid(.Item("BULKLOT").ToString, 1, 1) & "'" 'BULKLOT
                    strSql += " ,'" & Mid(.Item("MULTIPLETAGS").ToString, 1, 1) & "'" 'MULTIPLETAGS
                    strSql += " ,'" & .Item("NARRATION").ToString & "'" 'NARRATION
                    strSql += " ,'" & .Item("STYLENO").ToString & "'" 'STYLENO
                    strSql += " ," & Val(.Item("FINERATE").ToString) & "" 'FINERATE
                    strSql += " ," & Val(.Item("SALEPER").ToString) & "" 'FINERATE
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
                    strSql += " ,''" 'VATEXM
                    strSql += " ,''" 'ACCESSING
                    strSql += " ," & userId & "" 'USERID
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & .Item("TABLECODE").ToString & "'" 'TABLECODE
                    strSql += " ," & mFromItemid 'TABLECODE
                    strSql += " ,GETDATE()"
                    strSql += " ,'" & .Item("RANGESNO").ToString & "'" 'RANGESNO
                    strSql += " ,'" & .Item("STKTYPE").ToString & "'" 'STKTYPE
                    strSql += " ," & Val(.Item("DISCWASTPER").ToString) & "" 'WASTDISCPER
                    strSql += " ,'" & .Item("HALLMARK").ToString & "'" 'HALLMARK
                    strSql += " )"
                End With

                ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)
                If TagNoGen = "L" Then ''FROM ITEMMASTER
                    strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO = '" & endTagNo & "' WHERE ITEMID = '" & itemId & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                    'ElseIf TagNoGen = "L" Then ''FROM SOFTCONTROL
                    '    strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & endTagNo & "' WHERE CTLID = 'LASTTAGNO'"
                    '    cmd = New OleDbCommand(strSql, cn, tran)
                    '    cmd.ExecuteNonQuery()
                End If
                InsertintoNonTag(itemDt.Rows(cnt))
            Next
            tran.Commit()
            tran = Nothing
            Dim prLotNo As Integer = LotNo
            Dim prLotDate As Date = dtpDate.Value.Date
            MsgBox(LotNo.ToString + " Generated...", MsgBoxStyle.Exclamation)
            itemDt.Rows.Clear()
            dtgridstone.Rows.Clear()
            funcNew()
            ''Updation Flds
            flagUpdate = False
            tagMade = False
            cPcs = -1
            cWeight = -1
            If PrintLot Then
                Dim objLotPrint As New CLS_LOTPRINT(prLotNo, prLotDate)
                ' objLotPrint.Print()

                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim write As IO.StreamWriter
                    Dim memfile As String = "\BillPrint" & ".mem"
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    'write = IO.File.CreateText(Application.StartupPath & "\BillPrint.mem")
                    write.WriteLine(LSet("TYPE", 15) & ":" & "LOT" & "")
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & prLotNo)
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & prLotDate.ToString("yyyy-MM-dd"))
                    write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                        'LSet("TYPE", 15) & ":" & Type & ";" &
                        'LSet("BATCHNO", 15) & ":" & .Cells("BATCHNO").Value.ToString & ";" &
                        'LSet("TRANDATE", 15) & ":" & dtpBillDate.Value.ToString("yyyy-MM-dd") & ";" &
                        'LSet("DUPLICATE", 15) & ":Y")
                    End If
                Else
                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If



            End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Function

    Private Sub InsertintoNonTag(ByVal upRow As DataRow)
        If upRow!NONTAGITEM.ToString = "" Then Exit Sub
        Dim nontagsno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
        Dim costid As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & upRow!COSTCENTRE.ToString & "'", , , tran)
        strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAG"
        strSql += " ("
        strSql += " SNO,ITEMID,SUBITEMID,COMPANYID,RECDATE"
        strSql += " ,PCS,GRSWT,LESSWT,NETWT"
        strSql += " ,FINRATE,ISSTYPE,RECISS,POSTED"
        strSql += " ,LOTNO,PACKETNO,DREFNO,ITEMCTRID"
        strSql += " ,ORDREPNO,ORSNO,NARRATION,PURWASTAGE"
        strSql += " ,PURRATE,PURMC,RATE,COSTID"
        strSql += " ,CTGRM,DESIGNERID,VATEXM,ITEMTYPEID"
        strSql += " ,CARRYFLAG,REASON,BATCHNO,CANCEL"
        strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID,APPVER"
        strSql += " )"
        strSql += " Values"
        strSql += " ("
        strSql += " '" & nontagsno & "'" 'SNO
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & upRow!NONTAGITEM.ToString & "'", , , tran)) & "" 'ItemId
        strSql += " ," & Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & upRow!NONTAGSUBITEM.ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & upRow!NONTAGITEM.ToString & "')", , , tran)) & "" 'SUBITEMId
        strSql += " ,'" & GetStockCompId() & "'" 'CompanyId
        strSql += " ,'" & GetEntryDate(dtpDate.Value, tran).ToString("yyyy-MM-dd") & "'" ''Recdate DateTime.Parse(upRow!LOTDATE, ukCultureInfo.DateTimeFormat).ToString("yyyy-MM-dd")
        strSql += " ," & Val(upRow!NONTAGPCS.ToString) & "" 'Pcs
        strSql += " ," & Val(upRow!NONTAGGRSWT.ToString) & "" 'GrsWt
        strSql += " ,0" 'LessWt
        strSql += " ," & Val(upRow!NONTAGGRSWT.ToString) & "" 'NetWt
        strSql += " ,0" 'FineRate
        strSql += " ,'NT'" 'Isstype
        strSql += " ,'I'" 'RecIss
        strSql += " ,''" 'Posted
        strSql += " ,''" 'Lotno
        strSql += " ,''" 'Packetno
        strSql += " ,0" 'DRefno
        strSql += " ," & Val(upRow.Item("NONTAGITEMCTRID").ToString) & "" 'ItemCtrId
        strSql += " ,''" 'OrdRepNo
        strSql += " ,''" 'OrSNO
        strSql += " ,''" 'Narration
        strSql += " ,0" 'PurWastage
        strSql += " ,0" 'PurRate
        strSql += " ,0" 'PurMC
        strSql += " ," & Val(upRow!RATE.ToString) & "" 'Rate
        strSql += " ,'" & costid & "'" 'COSTID
        strSql += " ,''" 'CtGrm
        strSql += " ,0" 'DesignerId
        strSql += " ,'Y'" 'VATEXM
        strSql += " ,0" ' & Val(.Item("ItemTypeId", cnt).Value.ToString) & "" 'ItemTypeId
        strSql += " ,''" 'Carryflag
        strSql += " ,0" 'Reason
        strSql += " ,''" 'BatchNo
        strSql += " ,''" 'Cancel
        strSql += " ,'" & userId & "'" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += " ,'" & systemId & "'" 'SystemId
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " )"
        ExecQuery(SyncMode.Stock, strSql, cn, tran, costid)

        ''Inserting StoneDetail
        For Each ro As DataRow In dtgridstone.Rows
            If Val(upRow.Item("KEYNO").ToString) <> Val(ro.Item("KEYNO").ToString) Then Continue For
            Dim stnItemId As Integer = 0
            Dim stnSubItemId As Integer = 0
            'Dim caType As String = Nothing
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("ITEM").ToString & "'"
            stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & ro.Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
            stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))


            ''Inserting itemTagStone
            strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAGSTONE("
            strSql += " SNO,RECISS,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
            strSql += " STNSUBITEMID,STNPCS,STNWT,"
            strSql += " STNRATE,STNAMT,DESCRIP,"
            strSql += " RECDATE,PURRATE,PURAMT,CALCMODE,"
            strSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
            strSql += " VATEXM,CARRYFLAG,COSTID,SYSTEMID,APPVER)VALUES("
            strSql += " '" & GetNewSno(TranSnoType.ITEMNONTAGSTONECODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
            strSql += " ,'I'" ' RECISS
            strSql += " ,'" & nontagsno & "'" 'TAGSNO
            strSql += " ,'" & Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & upRow!NONTAGITEM.ToString & "'", , , tran)) & "'" 'ITEMID
            strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
            strSql += " ," & stnItemId & "" 'STNITEMID
            strSql += " ," & stnSubItemId & "" 'STNSUBITEMID
            strSql += " ," & Val(ro.Item("PCS").ToString) & "" 'STNPCS
            strSql += " ," & Val(ro.Item("WEIGHT").ToString) & "" 'STNWT
            strSql += " ," & Val(ro.Item("RATE").ToString) & "" 'STNRATE
            strSql += " ," & Val(ro.Item("AMOUNT").ToString) & "" 'STNAMT
            If stnSubItemId <> 0 Then 'DESCRIP
                strSql += " ,'" & ro.Item("SUBITEM").ToString & "'"
            Else
                strSql += " ,'" & ro.Item("ITEM").ToString & "'"
            End If
            strSql += " ,'" & GetEntryDate(dtpDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
            strSql += " ,0" 'PURRATE
            strSql += " ,0" 'PURAMT
            strSql += " ,'" & ro.Item("CALC").ToString & "'" 'CALCMODE
            strSql += " ,0" 'MINRATE
            strSql += " ,0" 'SIZECODE
            strSql += " ,'" & ro.Item("UNIT").ToString & "'" 'STONEUNIT
            strSql += " ,NULL" 'ISSDATE
            strSql += " ,''" 'VATEXM
            strSql += " ,''" 'CARRYFLAG
            strSql += " ,'" & costid & "'" 'COSTID
            strSql += " ,'" & systemId & "'" 'SYSTEMID
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " )"
            ExecQuery(SyncMode.Stock, strSql, cn, tran, costid, , , "TITEMNONTAGSTONE")
        Next
    End Sub
    Private Sub ShowStoneDia()
        If objStone.Visible Then Exit Sub
        objStone.grsWt = Val(txtGrossWeight_Wet.Text)
        objStone._EditLock = True
        objStone._DelLock = True
        objStone.BackColor = Me.BackColor
        objStone.StartPosition = FormStartPosition.CenterScreen
        objStone.MaximizeBox = False
        objStone.grpStone.BackgroundColor = Me.BackColor
        objStone.StyleGridStone(objStone.gridStone)
        'objStone.txtStItem.Select()
        objStone.txtStItem.Focus()
        objStone.ShowDialog()
        Dim stnWt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
        Dim stnAmt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtStoneWeight_Wet.Text = IIf(stnWt <> 0, Format(stnWt, "0.000"), Nothing)
        'txtStonePieces_Num.Text = IIf(stnWt <> 0, Format(stnWt, "0.000"), Nothing)
        Me.SelectNextControl(txtGrossWeight_Wet, True, True, True, True)
    End Sub
    Private Sub SETCOUNTERNAME(ByVal defcounterid As Integer)
        If defcounterid <> 0 Then
            strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = " & defcounterid & ""
            cmbItemCounter_MAN.Text = objGPack.GetSqlValue(strSql, , , tran).ToString
        End If
    End Sub



    Function funcUpdate() As Integer
        Dim DesignerId As String = Nothing
        Dim COSTID As String = Nothing
        Dim itemId As Integer = Nothing
        Dim stockType As String = Nothing
        Dim subItemId As Integer = Nothing
        If itemDt.Rows.Count = 0 Then Exit Function
        With itemDt.Rows(0)
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
                Case "WORK ORDER"
                    entryType = "WO"
                Case "ALTERATION"
                    entryType = "AL"
                Case "PARTLY SALE"
                    entryType = "PS"
                Case "OLD"
                    entryType = "OO"
                Case "SALES RETURN"
                    entryType = "SR"
                Case Else 'NONTAG TO TAG
                    entryType = "NT"
            End Select
            strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET "
            strSql += " ENTRYTYPE = '" & entryType & "'" 'ENTRYTYPE
            strSql += " ,LOTDATE = '" & .Item("LOTDATE") & "'" 'LOTDATE
            strSql += " ,DESIGNERID = " & DesignerId & "" 'DESIGNERID
            strSql += " ,TRANINVNO = '" & .Item("TRANINVNO") & "'" 'TRANINVNO
            strSql += " ,BILLNO = '" & .Item("BILLNO") & "'" 'BILLNO
            strSql += " ,COSTID = '" & COSTID & "'" 'COSTID
            strSql += " ,ORDREPNO = '" & .Item("ORDREPNO") & "'" 'ORDREPNO
            strSql += " ,ORDENTRYORDER = '" & .Item("ORDENTRYORDER") & "'" 'ORDENTRYORDER
            strSql += " ,ITEMID = " & Val(itemId) & "" 'ITEMID
            strSql += " ,SUBITEMID = " & Val(subItemId) & "" 'SUBITEMID
            strSql += " ,PCS = " & Val(.Item("PIECES")) & "" 'PCS
            strSql += " ,GRSWT = " & Val(.Item("GRSWT")) & "" 'GRSWT
            strSql += " ,STNPCS = " & Val(.Item("STNPCS")) & "" 'STNPCS
            strSql += " ,STNWT = " & Val(.Item("STNWT")) & "" 'STNWT
            strSql += " ,STNUNIT = '" & Mid(.Item("STNUNIT").ToString, 1, 1) & "'" 'STNUNIT
            strSql += " ,DIAPCS = " & Val(.Item("DIAPCS").ToString) & "" 'DIAPCS
            strSql += " ,DIAWT = " & Val(.Item("DIAWT").ToString) & "" 'DIAWT
            strSql += " ,NETWT = " & Val(.Item("NETWT").ToString) & "" 'NETWT
            strSql += " ,NOOFTAG = " & Val(.Item("NOOFTAG").ToString) & "" 'NOOFTAG
            strSql += " ,RATE = " & Val(.Item("RATE").ToString) & "" 'RATE
            strSql += " ,ITEMCTRID = " & Val(itemCounterId) & "" 'ITEMCTRID
            strSql += " ,WMCTYPE = '" & Mid(.Item("VALUEADDEDTYPE"), 1, 1) & "'" 'WMCTYPE
            strSql += " ,BULKLOT = '" & Mid(.Item("BULKLOT"), 1, 1) & "'" 'BULKLOT
            strSql += " ,MULTIPLETAGS = '" & Mid(.Item("MULTIPLETAGS").ToString, 1, 1) & "'" 'MULTIPLETAGS
            strSql += " ,NARRATION = '" & .Item("NARRATION").ToString & "'" 'NARRATION
            strSql += " ,STYLENO = '" & .Item("STYLENO").ToString & "'" 'STYLENO
            strSql += " ,FINERATE = " & Val(.Item("FINERATE").ToString) & "" 'FINERATE
            strSql += " ,TUCH = " & Val(.Item("TUCH").ToString) & "" 'TUCH
            strSql += " ,WASTPER = " & Val(.Item("WASTPER").ToString) & "" 'WASTPER
            strSql += " ,MCGRM = " & Val(.Item("MCGRM").ToString) & "" 'MCGRM
            strSql += " ,OTHCHARGE = " & Val(.Item("OTHCHARGE").ToString) & "" 'OTHCHARGE
            strSql += " ,COMPANYID = '" & GetStockCompId() & "'" 'COMPANYID
            strSql += " ,SYSTEMID = '" & systemId & "'" 'SYSTEMID
            strSql += " ,APPVER = '" & VERSION & "'" 'APPVER
            strSql += " ,TABLECODE = '" & .Item("TABLECODE").ToString & "'" 'TABLECODE
            strSql += " ,RANGESNO = '" & .Item("RANGESNO").ToString & "'" 'RANGESNO
            strSql += " ,STKTYPE = '" & .Item("STKTYPE").ToString & "'" 'STKTYPE
            strSql += " ,SALEPER = '" & Val(.Item("SALEPER").ToString) & "'" 'SALEPER
            strSql += " ,WASTDISCPER = " & Val(.Item("DISCWASTPER").ToString) & "" 'WASTDISCPER
            strSql += " ,HALLMARK = '" & .Item("HALLMARK").ToString & "'" 'HALLMARK
            strSql += " WHERE SNO = '" & updSno & "'"
        End With
        ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)
        'VIGNESH FOR PARAG JEWELLERY *******
        strSql = " SELECT 1 FROM  " & cnStockDb & "..LOTISSUE "
        strSql += " WHERE LOTSNO = '" & updSno & "'"
        If Val(objGPack.GetSqlValue(strSql, , "0", tran)) Then
            With itemDt.Rows(0)
                strSql = " UPDATE " & cnStockDb & "..LOTISSUE SET "
                strSql += " PCS = " & Val(.Item("PIECES")) & "" 'PCS
                strSql += " ,GRSWT = " & Val(.Item("GRSWT")) & "" 'GRSWT
                strSql += " ,NETWT = " & Val(.Item("NETWT").ToString) & "" 'NETWT
                strSql += " WHERE LOTSNO = '" & updSno & "'"
                ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)
            End With
        End If
        '*******
        ''Updation Flds
        flagUpdate = False
        tagMade = False
        cPcs = -1
        cWeight = -1
        itemDt.Rows.Clear()
        MsgBox("LotNo " & updLotNo & " Updated..")
    End Function
    Function funcGetDetails() As Integer

    End Function
    Function funcDefauleValues() As Integer
        CmbStockType.Text = ""
        If LOT_STKTYPE_AUTO Then CmbStockType.Text = "MANUFACTURING"
        If LOT_STKTYPE_DEFAULT <> "" Then
            If LOT_STKTYPE_DEFAULT = "M" Then
                CmbStockType.Text = "MANUFACTURING"
            ElseIf LOT_STKTYPE_DEFAULT = "T" Then
                CmbStockType.Text = "TRADING"
            ElseIf LOT_STKTYPE_DEFAULT = "E" Then
                CmbStockType.Text = "EXEMPTED"
            End If
        End If
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
        strSql = " select CostName from " & cnAdminDb & "..CostCentre WHERE 1=1 "
        If TAG_LOT_ALLOW_COSTID <> "" Then
            strSql += " AND costid in('" & Replace(TAG_LOT_ALLOW_COSTID, ",", "','") & "')"
        End If
        If SPECIFICFORMAT.ToString = "1" Then
            strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COMPANYID LIKE '%" & strCompanyId & "%')"
        End If
        strSql += " order by CostName"
        objGPack.FillCombo(strSql, cmbCostCentre_Man, False)
        cmbCostCentre_Man.Text = ""
    End Function
    Function funcLoadGrid() As Integer
        If objGPack.Validator_Check(Me) Then Exit Function
        If cmbSubItemName_Man.Text <> "" And cmbSubItemName_Man.Text <> "ALL" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = " & Val(txtItemCode_Num_Man.Text) & ""
        Else
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemCode_Num_Man.Text) & ""
        End If
        Dim calcType As String = objGPack.GetSqlValue(strSql)
        ''Weight Rate Validation
        Select Case calcType.ToUpper
            Case "W", "B", "M"
                If Val(txtGrossWeight_Wet.Text) = 0 Then
                    MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                    txtGrossWeight_Wet.Focus()
                    Exit Function
                End If
        End Select
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbSupplier_Man_OWN.Text & "'"
        If objGPack.GetSqlValue(strSql) = "" Then
            MsgBox("Invalid Supplier", MsgBoxStyle.Information)
            cmbSupplier_Man_OWN.Select()
            Exit Function
        End If
        If Val(txtGrossWeight_Wet.Text) <> 0 And Val(txtNetWeight_Wet.Text) > Val(txtGrossWeight_Wet.Text) Then
            MsgBox("NetWeight Should not Exceed " + txtGrossWeight_Wet.Text, MsgBoxStyle.Information)
            Exit Function
        End If
        If cmbBulkLot.Text = "YES" And calcType.ToUpper = "R" Then
            If Not Val(txtItemRate_Amt.Text) > 0 Then
                MsgBox(Me.GetNextControl(txtItemRate_Amt, False).Text + E0001, MsgBoxStyle.Information)
                txtItemRate_Amt.Focus()
                Exit Function
            End If
        End If
        Dim row As DataRow = Nothing
        If locUpdateRow <> -1 Then ''updat
            row = itemDt.Rows(locUpdateRow)
        Else
            row = itemDt.NewRow
        End If
        With row
            .Item("STKTYPE") = Mid(CmbStockType.Text, 1, 1)
            .Item("ENTRYTYPE") = cmbEntryType.Text
            .Item("LOTDATE") = dtpDate.Value
            .Item("DESIGNERNAME") = cmbSupplier_Man_OWN.Text
            .Item("TRANINVNO") = txtTransferInvNo.Text
            .Item("BILLNO") = txtBillNo.Text
            .Item("COSTCENTRE") = cmbCostCentre_Man.Text
            '.ITEM("ENTRYORDER") = 1
            .Item("ORDREPNO") = OrdRepairNo ' txtOrderRepairNo.Text
            '.ITEM("ORDENTRYORDER") = 1
            .Item("ITEMNAME") = txtItemName.Text
            .Item("SUBITEMNAME") = cmbSubItemName_Man.Text
            .Item("PIECES") = txtPiece_Num_Man.Text
            .Item("TABLECODE") = IIf(CmbTableCode.Enabled, CmbTableCode.Text, "")
            If txtGrossWeight_Wet.Enabled = True Then
                .Item("GRSWT") = txtGrossWeight_Wet.Text
            Else
                .Item("GRSWT") = ".000"
            End If
            If grpStone.Enabled = True Then
                .Item("STNPCS") = txtStonePieces_Num.Text
                .Item("STNWT") = txtStoneWeight_Wet.Text
                .Item("STNUNIT") = cmbStoneUnit.Text
            Else
                .Item("STNPCS") = ""
                .Item("STNWT") = ".000"
                .Item("STNUNIT") = ""
            End If
            If grpDiamond.Enabled = True Then
                .Item("DIAPCS") = txtDiamondPieces_Num.Text
                .Item("DIAWT") = txtDidmondWeight_Wet.Text
            Else
                .Item("DIAPCS") = ""
                .Item("DIAWT") = ".0000"
            End If
            If txtNetWeight_Wet.Enabled = True Then
                .Item("NETWT") = txtNetWeight_Wet.Text
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
            .Item("STYLENO") = txtStyleNo.Text
            .Item("DISCWASTPER") = txtDWastage_Per.Text
            If pnlPurchaseEntry.Enabled = True Then
                .Item("FINERATE") = txtFineRate_Amt.Text
                .Item("SALEPER") = txtSaleRate_Per.Text
                .Item("TUCH") = txtTuch_Amt.Text
                .Item("WASTPER") = txtWastage_Per.Text
                .Item("MCGRM") = txtMakingPerGrm_Amt.Text
                .Item("OTHCHARGE") = txtOtherCharges_Amt.Text
            End If
            If cmbEntryType.Text = "NONTAG TO TAG" Then
                .Item("NONTAGITEM") = txtNonTagItem.Text
                .Item("NONTAGSUBITEM") = txtNonTagSubItem.Text
                .Item("NONTAGPCS") = Val(txtPiece_Num_Man.Text) ' txtNonTagPcs.Text
                .Item("NONTAGGRSWT") = Val(txtGrossWeight_Wet.Text) ' txtNonTagGrsWt.Text
                .Item("NONTAGITEMCTRID") = nonTagItemCtrId
                .Item("TOORDER") = IIf(MoveOrder = True, "Y", "")
                MoveOrder = False : Me.txtOrderRepairNo.Enabled = False
            End If
            If cmbItemRange_OWN.Visible And cmbItemRange_OWN.Text <> "" Then
                If cmbSubItemName_Man.Text <> "" And cmbSubItemName_Man.Text <> "ALL" Then
                    strSql = " SELECT TOP 1 SNO FROM " & cnAdminDb & "..RANGEMAST WHERE CAPTION = '" & cmbItemRange_OWN.Text.ToString & "'"
                    strSql += " AND ITEMID = " & Val(txtItemCode_Num_Man.Text) & ""
                    strSql += " AND SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = " & Val(txtItemCode_Num_Man.Text) & ")"
                    .Item("RANGESNO") = objGPack.GetSqlValue(strSql, , "")
                Else
                    strSql = " SELECT TOP 1 SNO FROM " & cnAdminDb & "..RANGEMAST WHERE CAPTION = '" & cmbItemRange_OWN.Text.ToString & "'"
                    strSql += " AND ITEMID = " & Val(txtItemCode_Num_Man.Text) & ""
                    .Item("RANGESNO") = objGPack.GetSqlValue(strSql, , "")
                End If
            End If
            .Item("HALLMARK") = Mid(CmbHallmark.Text, 1, 1)
        End With
        If locUpdateRow = -1 Then
            itemDt.Rows.Add(row)
        End If


        If objStone.dtGridStone.Rows.Count > 0 Then
            For i As Integer = 0 To objStone.dtGridStone.Rows.Count - 1
                objStone.dtGridStone.Rows(i).Item("KEYNO") = itemDt.Rows(itemDt.Rows.Count - 1).Item("KEYNO").ToString
            Next
            dtgridstone.Merge(objStone.dtGridStone)
        End If

        'Dim TotPices As Integer = val(itemDt.Compute("SUM(PIECES)", ""))


        locUpdateRow = -1
        gridView.DataSource = itemDt
        gridView.AutoResizeColumns()
        gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Dim des As String = cmbSupplier_Man_OWN.Text
        Dim cos As String = cmbCostCentre_Man.Text
        If LOT_LESS_OPTION Then
            funcNew_Less()
        Else
            funcNew()
        End If
        cmbSupplier_Man_OWN.Text = des
        cmbCostCentre_Man.Text = cos
        If gridView.RowCount > 0 Then btnSave.Enabled = True
        If flagUpdate Then btnSave_Click(Me, New EventArgs)
    End Function
    Private Sub CalcGridViewTotal()
        ' If Not gridViewTotal.RowCount > 0 Then Exit Sub
        Dim pcs As Integer = Nothing
        Dim grsWt As Double = Nothing
        Dim netWt As Double = Nothing
        Dim tuch As Double = Nothing
        Dim amount As Double = Nothing
        Dim mCharge As Double = Nothing
        Dim nooftag As Double = Nothing

        Dim stnPcs As Integer = Nothing
        Dim diaPcs As Integer = Nothing
        Dim stnWt As Double = Nothing
        Dim diaWt As Double = Nothing

        For cnt As Integer = 0 To gridView.RowCount - 1
            With gridView.Rows(cnt)
                pcs += Val(.Cells("PIECES").Value.ToString)
                grsWt += Val(.Cells("GRSWT").Value.ToString)
                netWt += Val(.Cells("NETWT").Value.ToString)
                tuch += Val(.Cells("TUCH").Value.ToString)
                mCharge += Val(.Cells("MCGRM").Value.ToString)
                nooftag += Val(.Cells("NOOFTAG").Value.ToString)
                'amount += Val(.Cells("AMOUNT").Value.ToString)
                stnPcs += Val(.Cells("STNPCS").Value.ToString)
                stnWt += Val(.Cells("STNWT").Value.ToString)
                diaPcs += Val(.Cells("DIAPCS").Value.ToString)
                diaWt += Val(.Cells("DIAWT").Value.ToString)
            End With
        Next
        If gridViewTotal.Rows.Count > 0 Then
            With gridViewTotal.Rows(0)
                .Cells("PIECES").Value = IIf(pcs <> 0, pcs, DBNull.Value)
                .Cells("GRSWT").Value = IIf(Val(grsWt) <> 0, Format(Val(grsWt), "0.000"), DBNull.Value)
                .Cells("NETWT").Value = IIf(Val(netWt) <> 0, Format(Val(netWt), "0.000"), DBNull.Value)
                .Cells("TUCH").Value = IIf(tuch <> 0, tuch, DBNull.Value)
                .Cells("MCGRM").Value = IIf(mCharge <> 0, mCharge, DBNull.Value)
                .Cells("NOOFTAG").Value = IIf(nooftag <> 0, nooftag, DBNull.Value)

                .Cells("STNPCS").Value = IIf(stnPcs <> 0, stnPcs, DBNull.Value)
                .Cells("STNWT").Value = IIf(Val(stnWt) <> 0, Format(Val(stnWt), "0.000"), DBNull.Value)
                .Cells("DIAPCS").Value = IIf(diaPcs <> 0, diaPcs, DBNull.Value)
                .Cells("DIAWT").Value = IIf(Val(diaWt) <> 0, Format(Val(diaWt), "0.000"), DBNull.Value)

                '.Cells("AMOUNT").Value = IIf(amount <> 0, amount, DBNull.Value)
            End With
        End If
        ' ro("GRSWT") = IIf(Val(txtgrswt.Text) <> 0, Format(Val(txtgrswt.Text), "0.000"), "")
    End Sub
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

    Private Sub frmLotEntry_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F10 Then
            If cmbEntryType.Text = "NONTAG TO TAG" Then
                If MsgBox("Item transfer for Order(s)", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    Me.MoveOrder = True
                    Me.txtOrderRepairNo.Enabled = True
                End If
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
            If cmbEntryType.Focused Then Exit Sub
            If cmbSupplier_Man_OWN.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmLotEntry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlGrid.BringToFront()
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
        End If
        itemDt.Clear()
        strSql = " SELECT "
        strSql += " '' AS ITEMNAME,'' AS SUBITEMNAME,0 AS PIECES,''GRSWT,"
        strSql += " ''STNPCS,''STNWT,''STNUNIT,''DIAPCS,''DIAWT,''NETWT,''NOOFTAG,"
        strSql += " ''RATE,"
        strSql += " ''ENTRYTYPE,CONVERT(SMALLDATETIME,NULL)LOTDATE,''DESIGNERNAME,''TRANINVNO,"
        strSql += " ''BILLNO,''COSTCENTRE,''ENTRYORDER,''ORDREPNO,"
        strSql += " ''ORDENTRYORDER,'' AS ITEMCTRNAME,''VALUEADDEDTYPE,''BULKLOT,''MULTIPLETAGS,''NARRATION,''STYLENO,"
        strSql += " ''FINERATE,''SALEPER,''TUCH,''WASTPER,''MCGRM,''OTHCHARGE,''NONTAGITEM,''NONTAGSUBITEM,''NONTAGPCS,''NONTAGGRSWT,''NONTAGITEMCTRID,''TABLECODE,''TOORDER,''RANGESNO"
        strSql += " ,''STKTYPE,''DISCWASTPER,'' HALLMARK"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(itemDt)
        itemDt.Columns.Add("KEYNO", GetType(Integer))
        itemDt.Columns("KEYNO").AutoIncrement = True
        itemDt.Columns("KEYNO").AutoIncrementStep = 1
        itemDt.Columns("KEYNO").AutoIncrementSeed = 1
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
        '
        'If funcNew() = 0 Then Exit Sub
        funcNew()
        Dim ffont As New Font("VERDANA", 8, FontStyle.Bold)
        For cnt As Integer = 0 To gridViewTotal.RowCount - 1
            gridViewTotal.Rows(cnt).Height = 24
        Next
        gridViewTotal.Font = ffont
        With gridViewTotal
            .DefaultCellStyle.BackColor = SystemColors.Control
            .DefaultCellStyle.ForeColor = Color.Black
            .DefaultCellStyle.SelectionBackColor = SystemColors.Control
            .DefaultCellStyle.SelectionForeColor = Color.Black
        End With
        lblRange.Visible = RangeInLot
        cmbItemRange_OWN.Visible = RangeInLot
        dtGridViewTotal = itemDt.Copy
        dtGridViewTotal.Rows.Clear()
        dtGridViewTotal.Rows.Add()
        gridViewTotal.DataSource = dtGridViewTotal

        With gridViewTotal
            .Enabled = False
            .ColumnHeadersVisible = False
        End With
        gridViewTotal.Visible = False
        If LOTOPTIONRESTRICT <> "" Then
            If LOTOPTIONRESTRICT.Contains("REG") = False Then cmbEntryType.Items.Remove("REGULAR")
            If LOTOPTIONRESTRICT.Contains("REP") = False Then cmbEntryType.Items.Remove("REPAIR")
            If LOTOPTIONRESTRICT.Contains("ORD") = False Then cmbEntryType.Items.Remove("ORDER")
            If LOTOPTIONRESTRICT.Contains("NTT") = False Then cmbEntryType.Items.Remove("NONTAG TO TAG")
        End If
        If LOT_LESS_OPTION Then
            pnlBulk.Visible = False
            pnlPurchaseEntry.Visible = False
            CmbTableCode.Enabled = False
        End If
        If TAGWISE_DISCOUNT Then
            txtDWastage_Per.Visible = True
            Label43.Visible = True
        End If
        If LOT_TRANSFER_COSTID <> "" Then
            CmbCostCentre.Visible = True
            lblCostCentre.Visible = True
            btnTransfer.Visible = True
            CmbCostCentre.Items.Clear()
            strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & LOT_TRANSFER_COSTID & "'"
            CmbCostCentre.Items.Add(objGPack.GetSqlValue(strSql, "COSTNAME", ""))
            CmbCostCentre.SelectedIndex = 0
        End If
    End Sub
    Private Sub GridFormat(ByVal dgv As DataGridView)
        With dgv
            For cnt As Integer = 0 To dgv.ColumnCount - 1
                Select Case .Columns(cnt).ValueType.Name
                    Case GetType(Integer).Name
                        .Columns(cnt).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    Case GetType(Decimal).Name
                        .Columns(cnt).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Columns(cnt).DefaultCellStyle.Format = "0.000"
                    Case GetType(Double).Name
                        .Columns(cnt).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Columns(cnt).DefaultCellStyle.Format = "0.00"
                End Select
                .Columns(cnt).Resizable = DataGridViewTriState.False
            Next
        End With
    End Sub

    Private Sub txtItemCode_Num_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemCode_Num_Man.GotFocus

        If tagMade Then
            txtItemCode_Num_Man_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub
    Function GetNontagdetails() As Boolean
        strSql = " SELECT "
        strSql += " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)aS COUNTER"
        strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
        strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT "
        strSql += " FROM " & cnAdminDb & "..ITEMNONTAG T"
        strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtNonTagItem.Text & "')"
        strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtNonTagSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtNonTagItem.Text & "')),0)"
        If cmbCostCentre_Man.Text <> "" Then
            strSql += " AND ISNULL(COSTID,'')  = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & cmbCostCentre_Man.Text & "'),0)"
        End If
        If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        If _HideBackOffice Then strSql += " AND ISNULL(T.FLAG,'') <> 'B'"
        strSql += " AND ISNULL(CANCEL,'') = ''"
        strSql += " GROUP BY ITEMCTRID"
        strSql += " HAVING (SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) > 0 OR SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) > 0)"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If nonTagItemCtrId <= 0 And AZP And dt.Rows.Count = 0 Then
            MsgBox("SET DEFAULT ITEM COUNTER FIRST", MsgBoxStyle.Information)
            txtNonTagItem.Clear()
            txtNonTagSubItem.Clear()
            txtNonTagPcs.Clear()
            txtNonTagGrsWt.Clear()
            nonTagItemCtrId = Nothing
            cmbEntryType.Focus()
            Return False
            Exit Function
        End If
        If Not dt.Rows.Count > 0 And Not AZP Then
            MsgBox("There is no Receipt Entry Made to this Item", MsgBoxStyle.Information)
            txtNonTagItem.Clear()
            txtNonTagSubItem.Clear()
            txtNonTagPcs.Clear()
            txtNonTagGrsWt.Clear()
            nonTagItemCtrId = Nothing
            Me.cmbCostCentre_Man.Focus()
            Return False
            Exit Function
        End If
        Dim pcs As Integer = Nothing
        Dim grsWt As Double = Nothing
        If Not (AZP And dt.Rows.Count = 0) Then
            If dt.Rows.Count > 1 Then
                Dim retRow As DataRow = Nothing
                retRow = BrighttechPack.SearchDialog.Show_R("Select Counter Detail", strSql, cn, , , , , , , True)
                If retRow Is Nothing Then
                    Return False
                    Exit Function
                Else
                    pcs = Val(retRow.Item("PCS").ToString)
                    grsWt = Val(retRow.Item("GRSWT").ToString)
                    nonTagItemCtrId = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & retRow.Item("COUNTER").ToString & "'"))
                End If
            Else
                nonTagItemCtrId = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & dt.Rows(0).Item("COUNTER").ToString & "'"))
                pcs = Val(dt.Rows(0).Item("PCS").ToString)
                grsWt = Val(dt.Rows(0).Item("GRSWT").ToString)
            End If
            For Each ro As DataRow In itemDt.Rows
                If ro!NONTAGITEM.ToString = txtNonTagItem.Text And ro!NONTAGSUBITEM.ToString = txtNonTagSubItem.Text Then
                    pcs -= Val(ro!NONTAGPCS.ToString)
                    grsWt -= Val(ro!NONTAGGRSWT.ToString)
                End If
            Next
            If pcs <= 0 And grsWt <= 0 Then
                MsgBox("There is no Receipt Entry Made to this Item", MsgBoxStyle.Information)
                txtNonTagItem.Clear()
                txtNonTagSubItem.Clear()
                txtNonTagPcs.Clear()
                txtNonTagGrsWt.Clear()
                nonTagItemCtrId = Nothing
                cmbEntryType.Focus()
                Return False
                Exit Function
            End If
        End If
        txtNonTagPcs.Text = IIf(pcs <> 0, pcs, Nothing)
        txtNonTagGrsWt.Text = IIf(grsWt <> 0, Format(grsWt, "0.000"), Nothing)
        cmbEntryType.Enabled = False
        pnlNonTag.Visible = True
        Return True
    End Function
    Private Sub txtItemCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemCode_Num_Man.KeyDown
        If e.KeyCode = Keys.Insert Then  'Insert Key
            strSql = " SELECT ITEMID,ITEMNAME,"
            If LOT_LESS_OPTION Then
                strSql += " SHORTNAME, "
            End If
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
            txtItemCode_Num_Man.Text = BrighttechPack.SearchDialog.Show("Search ItemId", strSql, cn, IIf(LOT_LESS_OPTION, 2, 1))
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
                MsgBox(Me.GetNextControl(txtNoOfTags_Num, False).Text + E0010 + " " + txtPiece_Num_Man.Text, MsgBoxStyle.Exclamation)
                txtNoOfTags_Num.Focus()
            End If

            'For Range Selection
            cmbItemRange_OWN.Items.Clear()
            strSql = " SELECT DISTINCT CAPTION"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..RANGEMAST AS SR"
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID=SR.ITEMID"
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST S ON S.SUBITEMID=SR.SUBITEMID"
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..COSTCENTRE C ON C.COSTID=SR.COSTID"
            If UCase(cmbSubItemName_Man.Text) = "ALL" Then
                If cmbCostCentre_Man.Text <> "" Or txtItemName.Text <> "" Or cmbSubItemName_Man.Text <> "" Then
                    strSql += vbCrLf + "  WHERE 1=1 "
                    If cmbCostCentre_Man.Text <> "" Then strSql += vbCrLf + "  AND C.COSTNAME='" & cmbCostCentre_Man.Text & "'"
                    strSql += vbCrLf + "  AND I.ITEMNAME='" & txtItemName.Text & "'"
                    objGPack.FillCombo(strSql, cmbItemRange_OWN, False)
                End If
            Else
                If cmbCostCentre_Man.Text <> "" Or txtItemName.Text <> "" Or cmbSubItemName_Man.Text <> "" Then
                    strSql += vbCrLf + "  WHERE 1=1 "
                    If cmbCostCentre_Man.Text <> "" Then strSql += vbCrLf + " AND C.COSTNAME='" & cmbCostCentre_Man.Text & "'"
                    strSql += vbCrLf + "  AND I.ITEMNAME='" & txtItemName.Text & "'"
                    strSql += vbCrLf + "  AND S.SUBITEMNAME='" & cmbSubItemName_Man.Text & "'"
                    objGPack.FillCombo(strSql, cmbItemRange_OWN, False)
                End If
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
        wt = Val(txtGrossWeight_Wet.Text) - wt
        txtNetWeight_Wet.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub

    Private Sub txtDidmondWeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDidmondWeight_Wet.KeyPress
        CalcNetWt()
    End Sub

    Private Sub cmbEntryType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbEntryType.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtNonTagItem.Clear()
            txtNonTagSubItem.Clear()
            txtNonTagPcs.Clear()
            txtNonTagGrsWt.Clear()
            pnlNonTag.Visible = False
            If cmbEntryType.Text = "NONTAG TO TAG" Then
                lblNTOrder.Visible = True
                strSql = " SELECT"
                strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME,"
                strSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGGED' "
                strSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGGED' ELSE 'POCKET BASED' END AS STOCK_TYPE,"
                strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
                strSql += " WHEN CALTYPE = 'W' THEN 'WEIGHT'"
                strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
                strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
                strSql += " WHEN CALTYPE = 'B' THEN 'BOTH' ELSE 'METAL RATE' END AS CAL_TYPE"
                strSql += " FROM " & cnAdminDb & "..ITEMMAST"
                strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y'"
                strSql += " AND STOCKTYPE = 'N'"

                txtNonTagItem.Text = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, 1, , txtNonTagItem.Text, , , True)
                Dim iId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtNonTagItem.Text & "'"))
                AZP = IIf(objGPack.GetSqlValue("SELECT ISNULL(ALLOWZEROPCS,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtNonTagItem.Text & "'").ToUpper() = "Y", True, False)
                Dim DefItem As String = txtNonTagSubItem.Text
                If Not _SubItemOrderByName Then
                    DefItem = objGPack.GetSqlValue("SELECT DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtNonTagSubItem.Text & "' AND ITEMID = " & iId & "")
                End If
                strSql = GetSubItemQry(New String() {"SUBITEMID ID", "DISPLAYORDER DISP", "SUBITEMNAME SUBITEM"}, iId)
                txtNonTagSubItem.Text = BrighttechPack.SearchDialog.Show("Search SubItem", strSql, cn, IIf(_SubItemOrderByName, 2, 1), 2, , DefItem, , False, True)
                'txtNonTagSubItem.Text = BrighttechPack.SearchDialog.Show("Find SubItem", "SELECT SUBITEMID ID,SUBITEMNAME SUBITEM FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtNonTagItem.Text & "'")) & " AND ACTIVE = 'Y'", cn, 1, 1, , txtNonTagSubItem.Text, , False, True)
                If txtNonTagSubItem.Text.Trim() <> "" And AZP Then
                    nonTagItemCtrId = Val(objGPack.GetSqlValue("SELECT DEFAULTCOUNTER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtNonTagSubItem.Text & "'"))
                End If
                If nonTagItemCtrId = 0 And AZP Then
                    nonTagItemCtrId = Val(objGPack.GetSqlValue("SELECT DEFAULTCOUNTER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtNonTagItem.Text & "'"))
                End If

                'strSql = " SELECT "
                'strSql += " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)aS COUNTER"
                'strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1*PCS END) AS PCS"
                'strSql += " ,SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT "
                'strSql += " FROM " & cnAdminDb & "..ITEMNONTAG T"
                'strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtNonTagItem.Text & "')"
                'strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtNonTagSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtNonTagItem.Text & "')),0)"
                ''strSql += " AND ISNULL(COSTID,'') = '" & GetCostId() & "'" 'ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
                'If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
                'If _HideBackOffice Then strSql += " AND ISNULL(T.FLAG,'') <> 'B'"
                'strSql += " AND ISNULL(CANCEL,'') = ''"
                'strSql += " GROUP BY ITEMCTRID"
                'strSql += " HAVING SUM(CASE WHEN RECISS = 'R' THEN GRSWT ELSE -1*GRSWT END) > 0"
                'Dim dt As New DataTable
                'da = New OleDbDataAdapter(strSql, cn)
                'da.Fill(dt)
                'If nonTagItemCtrId <= 0 And AZP And dt.Rows.Count = 0 Then
                '    MsgBox("SET DEFAULT ITEM COUNTER FIRST", MsgBoxStyle.Information)
                '    txtNonTagItem.Clear()
                '    txtNonTagSubItem.Clear()
                '    txtNonTagPcs.Clear()
                '    txtNonTagGrsWt.Clear()
                '    nonTagItemCtrId = Nothing
                '    cmbEntryType.Focus()
                '    Exit Sub
                'End If
                'If Not dt.Rows.Count > 0 And Not AZP Then
                '    MsgBox("There is no Receipt Entry Made to this Item", MsgBoxStyle.Information)
                '    txtNonTagItem.Clear()
                '    txtNonTagSubItem.Clear()
                '    txtNonTagPcs.Clear()
                '    txtNonTagGrsWt.Clear()
                '    nonTagItemCtrId = Nothing
                '    cmbEntryType.Focus()
                '    Exit Sub
                'End If
                'Dim pcs As Integer = Nothing
                'Dim grsWt As Double = Nothing
                'If Not (AZP And dt.Rows.Count = 0) Then
                '    If dt.Rows.Count > 1 Then
                '        Dim retRow As DataRow = Nothing
                '        retRow = BrighttechPack.SearchDialog.Show_R("Select Counter Detail", strSql, cn, , , , , , , True)
                '        If retRow Is Nothing Then
                '            Exit Sub
                '        Else
                '            pcs = Val(retRow.Item("PCS").ToString)
                '            grsWt = Val(retRow.Item("GRSWT").ToString)
                '            nonTagItemCtrId = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & retRow.Item("COUNTER").ToString & "'"))
                '        End If
                '    Else
                '        nonTagItemCtrId = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & dt.Rows(0).Item("COUNTER").ToString & "'"))
                '        pcs = Val(dt.Rows(0).Item("PCS").ToString)
                '        grsWt = Val(dt.Rows(0).Item("GRSWT").ToString)
                '    End If
                '    For Each ro As DataRow In itemDt.Rows
                '        If ro!NONTAGITEM.ToString = txtNonTagItem.Text And ro!NONTAGSUBITEM.ToString = txtNonTagSubItem.Text Then
                '            pcs -= Val(ro!NONTAGPCS.ToString)
                '            grsWt -= Val(ro!NONTAGGRSWT.ToString)
                '        End If
                '    Next
                '    If pcs <= 0 And grsWt <= 0 Then
                '        MsgBox("There is no Receipt Entry Made to this Item", MsgBoxStyle.Information)
                '        txtNonTagItem.Clear()
                '        txtNonTagSubItem.Clear()
                '        txtNonTagPcs.Clear()
                '        txtNonTagGrsWt.Clear()
                '        nonTagItemCtrId = Nothing
                '        cmbEntryType.Focus()
                '        Exit Sub
                '    End If
                'End If
                'txtNonTagPcs.Text = IIf(pcs <> 0, pcs, Nothing)
                'txtNonTagGrsWt.Text = IIf(grsWt <> 0, Format(grsWt, "0.000"), Nothing)
                'cmbEntryType.Enabled = False
                'pnlNonTag.Visible = True
            End If

            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbEntryType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbEntryType.SelectedIndexChanged
        ''Order/Repair only
        txtOrderRepairNo.Text = ""
        If cmbEntryType.SelectedIndex = 1 Or cmbEntryType.SelectedIndex = 2 Then
            txtOrderRepairNo.Enabled = True
            If cmbEntryType.Text = "ORDER" Then SETCOUNTERNAME(ORDERCOUNTERID) Else SETCOUNTERNAME(REPAIRCOUNTERID)
        Else
            txtOrderRepairNo.Enabled = False
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
        gridViewTotal.Visible = False
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
        strSql += vbCrLf + " SNO,LOTNO,LOTDATE,"
        strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID=LOT.ITEMID)AS ITEM,"
        strSql += vbCrLf + " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID=LOT.SUBITEMID),'')AS SUBITEM,"
        strSql += vbCrLf + " CASE WHEN PCS <> 0 THEN PCS ELSE NULL END PCS,CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END GRSWT,CASE WHEN STNPCS <> 0 THEN STNPCS ELSE NULL END STNPCS,CASE WHEN STNWT <> 0 THEN STNWT ELSE NULL END STNWT,STNUNIT,"
        strSql += vbCrLf + " CASE WHEN DIAPCS <> 0 THEN DIAPCS ELSE NULL END DIAPCS,CASE WHEN DIAWT <> 0 THEN DIAWT ELSE NULL END DIAWT,CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END NETWT,NOOFTAG,CASE WHEN RATE <> 0 THEN RATE ELSE NULL END RATE,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = LOT.ITEMCTRID)AS COUNTER,"
        strSql += vbCrLf + " ENTRYTYPE,"
        strSql += vbCrLf + " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER AS D WHERE D.DESIGNERID = LOT.DESIGNERID)AS DESIGNERNAME,"
        strSql += vbCrLf + " TRANINVNO,"
        strSql += vbCrLf + " BILLNO,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = LOT.COSTID)AS COSTCENTRE,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
        strSql += vbCrLf + " WMCTYPE AS VALUEADDEDTYPE,BULKLOT,MULTIPLETAGS,NARRATION,CASE WHEN FINERATE <> 0 THEN FINERATE ELSE NULL END FINERATE,CASE WHEN TUCH <> 0 THEN TUCH ELSE NULL END TUCH,"
        strSql += vbCrLf + " CASE WHEN WASTPER <> 0 THEN WASTPER ELSE NULL END WASTPER,CASE WHEN MCGRM <> 0 THEN MCGRM ELSE NULL END MCGRM,CASE WHEN OTHCHARGE <> 0 THEN OTHCHARGE ELSE NULL END OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
        strSql += vbCrLf + " COMPANYID,CASE WHEN CPCS <> 0 THEN CPCS ELSE NULL END CPCS,CASE WHEN CGRSWT <> 0 THEN CGRSWT ELSE NULL END CGRSWT,COMPLETED,CANCEL,VATEXM,"
        strSql += vbCrLf + " ACCESSING,TABLECODE,SALEPER,ISNULL(STYLENO,'')STYLENO "
        strSql += vbCrLf + " ,CASE WHEN ISNULL(STKTYPE,'T')='M' THEN 'MANUFACTURING' WHEN ISNULL(STKTYPE,'T')='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE,ISNULL(HALLMARK,'Y')HALLMARK"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMLOT AS LOT"
        strSql += vbCrLf + " WHERE 1=1"
        If txtOpenLotNo.Text <> "" Then
            strSql += vbCrLf + " AND LOTNO = '" & txtOpenLotNo.Text & "'"
        End If
        If cmbOpenDesignerName.Text <> "ALL" Then
            strSql += vbCrLf + " AND DESIGNERID = (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbOpenDesignerName.Text & "')"
        End If
        If CmbOpenCostcentre.Text <> "ALL" And CmbOpenCostcentre.Text <> "" Then
            strSql += vbCrLf + " AND LOT.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & CmbOpenCostcentre.Text & "')"
        End If
        If txtOpenItemName.Text <> "" Then
            strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOpenItemName.Text & "')"
        End If
        If dtpFrom.Enabled = True And dtpTo.Enabled = True Then
            strSql += vbCrLf + " AND LOTDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        End If
        If CmbCostCentre.Text <> "ALL" And CmbCostCentre.Text <> "" Then
            strSql += vbCrLf + " AND LOT.COSTID NOT IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & CmbCostCentre.Text & "')"
        End If
        If Not cnCentStock Then strSql += vbCrLf + " AND COMPANYID = '" & GetStockCompId() & "'"
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
            With .Columns("VATEXM")

            End With
            With .Columns("ACCESSING")

            End With
            With .Columns("TABLECODE")

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
                txtGrossWeight_Wet.Enabled = False
                txtNetWeight_Wet.Enabled = False
                cmbBulkLot.Enabled = True
                cmbMultipleTag.Enabled = True
            ElseIf calType = "M" Then
                txtGrossWeight_Wet.Enabled = True
                txtNetWeight_Wet.Enabled = True
                cmbBulkLot.Enabled = WTITEMBULKTAG : cmbMultipleTag.Enabled = WTITEMBULKTAG

                txtItemRate_Amt.Enabled = True
            Else
                txtItemRate_Amt.Enabled = True
                txtGrossWeight_Wet.Enabled = True
                txtNetWeight_Wet.Enabled = True
                cmbBulkLot.Enabled = WTITEMBULKTAG : cmbMultipleTag.Enabled = WTITEMBULKTAG
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
        ElseIf e.KeyCode = Keys.P Then
            If PrintLot Then
                Dim objLotPrint As New CLS_LOTPRINT(Val(gridViewOpen.CurrentRow.Cells("LOTNO").Value.ToString), CType(gridViewOpen.CurrentRow.Cells("LOTDATE").Value, Date))
                'objLotPrint.Print()


                ' Dim objLotPrint As New CLS_LOTPRINT(prLotNo, prLotDate)
                ' objLotPrint.Print()

                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim write As IO.StreamWriter
                    Dim memfile As String = "\BillPrint" & ".mem"
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    'write = IO.File.CreateText(Application.StartupPath & "\BillPrint.mem")
                    write.WriteLine(LSet("TYPE", 15) & ":" & "LOT" & "")
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & Val(gridViewOpen.CurrentRow.Cells("LOTNO").Value.ToString))
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & CType(gridViewOpen.CurrentRow.Cells("LOTDATE").Value, Date).ToString("yyyy-MM-dd"))
                    write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                        'LSet("TYPE", 15) & ":" & Type & ";" &
                        'LSet("BATCHNO", 15) & ":" & .Cells("BATCHNO").Value.ToString & ";" &
                        'LSet("TRANDATE", 15) & ":" & dtpBillDate.Value.ToString("yyyy-MM-dd") & ";" &
                        'LSet("DUPLICATE", 15) & ":Y")
                    End If
                Else
                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If





            End If
        End If
    End Sub

    Private Sub gridViewOpen_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridViewOpen.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not gridViewOpen.RowCount > 0 Then Exit Sub
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
            'strSql = " SELECT"
            'strSql += " LOTNO,CONVERT(VARCHAR(10),LOTDATE,103)AS LOTDATE,"
            'strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID=LOT.ITEMID)AS ITEM,"
            'strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID=LOT.SUBITEMID),'')AS SUBITEM,"
            'strSql += " CASE WHEN PCS <> 0 THEN PCS ELSE NULL END PCS,CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END GRSWT,CASE WHEN STNPCS <> 0 THEN STNPCS ELSE NULL END STNPCS,CASE WHEN STNWT <> 0 THEN STNWT ELSE NULL END STNWT,STNUNIT,"
            'strSql += " CASE WHEN DIAPCS <> 0 THEN DIAPCS ELSE NULL END DIAPCS,CASE WHEN DIAWT <> 0 THEN DIAWT ELSE NULL END DIAWT,CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END NETWT,NOOFTAG,CASE WHEN RATE <> 0 THEN RATE ELSE NULL END RATE,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = LOT.ITEMCTRID)AS COUNTER,"
            'strSql += " ENTRYTYPE,"
            'strSql += " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER AS D WHERE D.DESIGNERID = LOT.DESIGNERID)AS DESIGNERNAME,"
            'strSql += " TRANINVNO,"
            'strSql += " BILLNO,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = LOT.COSTID)AS COSTCENTRE,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
            'strSql += " WMCTYPE AS VALUEADDEDTYPE,BULKLOT,MULTIPLETAGS,NARRATION,CASE WHEN FINERATE <> 0 THEN FINERATE ELSE NULL END FINERATE,CASE WHEN TUCH <> 0 THEN TUCH ELSE NULL END TUCH,"
            'strSql += " CASE WHEN WASTPER <> 0 THEN WASTPER ELSE NULL END WASTPER,CASE WHEN MCGRM <> 0 THEN MCGRM ELSE NULL END MCGRM,CASE WHEN OTHCHARGE <> 0 THEN OTHCHARGE ELSE NULL END OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
            'strSql += " COMPANYID,CASE WHEN CPCS <> 0 THEN CPCS ELSE NULL END CPCS,CASE WHEN CGRSWT <> 0 THEN CGRSWT ELSE NULL END CGRSWT,COMPLETED,CANCEL,VATEXM,"
            'strSql += " ACCESSING FROM " & cnAdminDb & "..ITEMLOT AS LOT"
            With gridViewOpen.Rows(gridViewOpen.CurrentRow.Index)
                Select Case .Cells("ENTRYTYPE").Value.ToString
                    Case "R"
                        cmbEntryType.Text = "REGULAR"
                    Case "OR"
                        cmbEntryType.Text = "ORDER"
                    Case "RE"
                        cmbEntryType.Text = "REPAIR"
                    Case "WO"
                        cmbEntryType.Text = "WORK ORDER"
                    Case "AL"
                        cmbEntryType.Text = "ALTERATION"
                    Case "OO"
                        cmbEntryType.Text = "OLD"
                    Case "SR"
                        cmbEntryType.Text = "SALES RETURN"
                    Case "PS"
                        cmbEntryType.Text = "PARTLY SALE"
                    Case Else 'NT
                        cmbEntryType.Text = "NONTAG TO TAG"
                End Select
                CmbStockType.Text = .Cells("STKTYPE").Value.ToString
                dtpDate.Value = .Cells("LOTDATE").Value
                cmbSupplier_Man_OWN.Text = .Cells("DESIGNERNAME").Value.ToString
                txtTransferInvNo.Text = .Cells("TRANINVNO").Value.ToString
                txtBillNo.Text = .Cells("BILLNO").Value.ToString
                cmbCostCentre_Man.Text = .Cells("COSTCENTRE").Value.ToString
                OrdRepairNo = .Cells("ORDREPNO").Value.ToString
                If OrdRepairNo.Length > 2 Then OrdCostId = OrdRepairNo.ToString.Substring(0, 2)
                If OrdRepairNo.Length > 5 Then OrdCompanyId = OrdRepairNo.ToString.Substring(2, 3)
                If OrdRepairNo.Length > 6 Then
                    txtOrderRepairNo.Text = OrdRepairNo.ToString.Substring(5)
                Else
                    txtOrderRepairNo.Text = .Cells("ORDREPNO").Value.ToString
                End If
                strSql = " SELECT 1 COL1 FROM " & cnadmindb & "..ORIRDETAIL WHERE ISNULL(ORSNO,'') IN"
                strSql += vbCrLf + " (SELECT ORSNO FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO = '" & .Cells("SNO").Value.ToString & "')"
                If Not BrighttechPack.GetSqlValue(cn, strSql, "COL1", "") = "" Then
                    txtOrderRepairNo.Enabled = False
                Else
                    txtOrderRepairNo.Enabled = True
                End If

                txtItemName.Text = .Cells("ITEM").Value.ToString
                txtItemCode_Num_Man.Text = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "'")
                cmbSubItemName_Man.Text = IIf(.Cells("SUBITEM").Value.ToString = "", "ALL", .Cells("SUBITEM").Value.ToString)
                txtPiece_Num_Man.Text = .Cells("PCS").Value.ToString
                txtGrossWeight_Wet.Text = .Cells("GRSWT").Value.ToString
                txtStonePieces_Num.Text = .Cells("STNPCS").Value.ToString
                txtStoneWeight_Wet.Text = .Cells("STNWT").Value.ToString
                cmbStoneUnit.Text = .Cells("STNUNIT").Value.ToString
                txtDiamondPieces_Num.Text = .Cells("DIAPCS").Value.ToString
                txtDidmondWeight_Wet.Text = .Cells("DIAWT").Value.ToString
                txtNetWeight_Wet.Text = .Cells("NETWT").Value.ToString
                txtNoOfTags_Num.Text = .Cells("NOOFTAG").Value.ToString
                txtItemRate_Amt.Text = .Cells("RATE").Value.ToString
                cmbItemCounter_MAN.Text = .Cells("COUNTER").Value.ToString
                txtSaleRate_Per.Text = .Cells("SALEPER").Value.ToString
                txtStyleNo.Text = .Cells("STYLENO").Value.ToString

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
                txtTuch_Amt.Text = .Cells("TUCH").Value.ToString
                txtWastage_Per.Text = .Cells("WASTPER").Value.ToString
                txtMakingPerGrm_Amt.Text = .Cells("MCGRM").Value.ToString
                txtOtherCharges_Amt.Text = .Cells("OTHCHARGE").Value.ToString
                CmbTableCode.Text = .Cells("TABLECODE").Value.ToString
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

                If cmbSubItemName_Man.Text = "ALL" Then
                    strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "' AND ISNULL(HALLMARK,'Y') = 'Y'"
                Else
                    strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ISNULL(HALLMARK,'Y') = 'Y' AND ITEMID = " & Val(txtItemCode_Num_Man.Text) & ""
                End If
                If objGPack.GetSqlValue(strSql).Length > 0 Then
                    If .Cells("HALLMARK").Value.ToString = "Y" Then
                        CmbHallmark.Text = "YES"
                    Else
                        CmbHallmark.Text = "NO"
                    End If
                Else
                    CmbHallmark.Text = "NO"
                End If

            End With
            tabMain.SelectedTab = tabGeneral
            cmbEntryType.Focus()
        End If
    End Sub

    Private Sub txtGrossWeight_Wet_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrossWeight_Wet.GotFocus
        If Val(txtGrossWeight_Wet.Text) = 0 Then
            If cmbSubItemName_Man.Text = "" Or cmbSubItemName_Man.Text = "ALL" Then
                strSql = " SELECT GRSWT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "'"
            Else
                strSql = " SELECT GRSWT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = " & Val(txtItemCode_Num_Man.Text) & ""
            End If
            Dim defWt As Double = Val(txtPiece_Num_Man.Text) * Val(objGPack.GetSqlValue(strSql))
            txtGrossWeight_Wet.Text = IIf(defWt > 0, Format(defWt, "0.000"), "")
        End If
    End Sub

    Private Sub txtGrossWeight_Wet_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrossWeight_Wet.KeyPress
        If cmbEntryType.Text = "NONTAG TO TAG" Then
            If tabCheckBy = "W" Then
                If Val(txtGrossWeight_Wet.Text + e.KeyChar) > Val(txtNonTagGrsWt.Text) Then
                    e.Handled = True
                    Exit Sub
                End If
            End If
        End If
        If e.KeyChar = Chr(Keys.Enter) And Val(txtGrossWeight_Wet.Text) = 0 Then
            If cmbSubItemName_Man.Text <> "" And cmbSubItemName_Man.Text <> "ALL" Then
                strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = " & Val(txtItemCode_Num_Man.Text) & ""
            Else
                strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemCode_Num_Man.Text) & ""
            End If
            Dim calcType As String = objGPack.GetSqlValue(strSql)
            ''Weight Rate Validation
            Select Case calcType.ToUpper
                Case "W"
                    If Val(txtGrossWeight_Wet.Text) = 0 Then
                        MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                        txtGrossWeight_Wet.Focus()
                        Exit Sub
                    End If
                Case "B"
                    If Val(txtGrossWeight_Wet.Text) = 0 Then
                        MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                        txtGrossWeight_Wet.Focus()
                        Exit Sub
                    End If
                Case "M"
                    If Val(txtGrossWeight_Wet.Text) = 0 Then
                        MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                        txtGrossWeight_Wet.Focus()
                        Exit Sub
                    End If
            End Select
            Exit Sub
        End If
    End Sub

    Private Sub txtGrossWeight_Wet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrossWeight_Wet.LostFocus
        If tabCheckBy = "W" And flagUpdate And Val(txtGrossWeight_Wet.Text) + Val(Tag_Tolerance) < cWeight Then
            MsgBox("Completed Weight : " & cWeight & vbCrLf & "You must enter minimum of completed weight", MsgBoxStyle.Information)
            txtGrossWeight_Wet.Text = Format(cWeight, "0.000")
            txtGrossWeight_Wet.Focus()
            txtGrossWeight_Wet.SelectAll()
            Exit Sub
        End If
        txtNetWeight_Wet.Text = IIf(Val(txtGrossWeight_Wet.Text) <> 0, Format(Val(txtGrossWeight_Wet.Text), "0.000"), "")

        If flagUpdate = False Then
            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(ITEMID,'') = '" + txtItemCode_Num_Man.Text + "' AND ISNULL(SUBITEMNAME,'') = '" + cmbSubItemName_Man.Text + "'"
            Dim SubItemId As String = BrighttechPack.GetSqlValue(cn, strSql, "SUBITEMID", "")
            strSql = vbCrLf + " SELECT TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR FROM " & cnAdminDb & "..WMCTABLE"
            strSql += vbCrLf + " WHERE ISNULL(ITEMID,'') = '" + txtItemCode_Num_Man.Text + "' AND ISNULL(SUBITEMID,'') = '" + SubItemId + "'"
            strSql += vbCrLf + " AND " + IIf(txtGrossWeight_Wet.Text = "", "0", txtGrossWeight_Wet.Text) + " BETWEEN FROMWEIGHT AND TOWEIGHT"
            strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = '' AND ISNULL(ITEMTYPE,0) = 0 "
            strSql += vbCrLf + " AND ISNULL(DESIGNERID,0) = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbSupplier_Man_OWN.Text & "'),0)"
            strSql += vbCrLf + " AND DESIGNERID <> 0"
            strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"

            'strSql = " SELECT ACCODE FROM " & cnAdmindb & "..DESIGNER WHERE ISNULL(DESIGNERNAME,'') = '" + cmbSupplier_Man_OWN.Text + "'"
            'Dim Accode As String = BrighttechPack.GetSqlValue(cn, strSql, "ACCODE", "")
            'strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(ITEMID,'') = '" + txtItemCode_Num_Man.Text + "' AND ISNULL(SUBITEMNAME,'') = '" + cmbSubItemName_Man.Text + "'"
            'Dim SubItemId As String = BrighttechPack.GetSqlValue(cn, strSql, "SUBITEMID", "")

            'strSql = vbCrLf + " SELECT TOUCH_PUR,MAXWAST_PUR,MAXMCGRM_PUR FROM " & cnAdminDb & "..WMCTABLE"
            'strSql += vbCrLf + " WHERE ISNULL(ACCODE,'') = '" + Accode + "' AND ISNULL(ITEMID,'') = '" + txtItemCode_Num_Man.Text + "' AND ISNULL(SUBITEMID,'') = '" + SubItemId + "'"
            'strSql += vbCrLf + " AND " + IIf(txtGrossWeight_Wet_MAN.Text = "", "0", txtGrossWeight_Wet_MAN.Text) + " BETWEEN FROMWEIGHT AND TOWEIGHT"
            'strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = '' AND ISNULL(ITEMTYPE,0) = 0 "
            'strSql += vbCrLf + " AND ISNULL(DESIGNERID,0) = 0 AND ISNULL(ACCODE,'') != ''"
            'strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
            Dim dtWMC As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtWMC)
            If dtWMC.Rows.Count > 0 Then
                txtTuch_Amt.Text = dtWMC.Rows(0).Item("TOUCH_PUR").ToString
                txtWastage_Per.Text = dtWMC.Rows(0).Item("MAXWASTPER_PUR").ToString
                txtMakingPerGrm_Amt.Text = dtWMC.Rows(0).Item("MAXMCGRM_PUR").ToString
            Else
                txtTuch_Amt.Text = ""
                txtWastage_Per.Text = ""
                txtMakingPerGrm_Amt.Text = ""
            End If
        End If
        If Val(txtGrossWeight_Wet.Text) > 0 Then
            Dim nontagstud As String = ""
            Dim tagstud As String = ""
            strSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtNonTagItem.Text & "'"
            nontagstud = objGPack.GetSqlValue(strSql, "", "", )
            strSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & Val(txtItemCode_Num_Man.Text) & "'"
            tagstud = objGPack.GetSqlValue(strSql, "", "", )
            If tagstud = "Y" And nontagstud = "Y" Then
                ShowStoneDia()
            End If
        End If
    End Sub

    Private Sub txtItemCode_Num_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemCode_Num_Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then 'Enter Key
            If txtItemCode_Num_Man.Text = "" Then
                Exit Sub
            End If
            Dim itemName As String = objGPack.GetSqlValue(" select ItemName from " & cnAdminDb & "..ItemMast where ItemId = '" & txtItemCode_Num_Man.Text & "' AND ACTIVE = 'Y' AND STUDDED <> 'S'" & GetItemQryFilteration("S"), "itemname")
            txtItemName.Text = itemName
            If txtItemName.Text = "" Then
                MsgBox(E0004 + Me.GetNextControl(txtItemCode_Num_Man, False).Text, MsgBoxStyle.Information)
                txtItemCode_Num_Man.Select()
                Exit Sub
            End If


            cmbItemCounter_MAN.Text = ""
            strSql = " SELECT SUBITEM,VALUEADDEDTYPE,CALTYPE,STOCKTYPE,NOOFPIECE,"
            strSql += " ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = DEFAULTCOUNTER),'')AS ITEMCTRNAME"
            strSql += " ,STUDDEDSTONE,ISNULL(HALLMARK,'Y')HALLMARK"
            strSql += " FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & txtItemCode_Num_Man.Text & "'"
            Dim dt As New DataTable
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            dt.AcceptChanges()

            txtGrossWeight_Wet.Enabled = False
            grpStone.Enabled = False
            grpDiamond.Enabled = False
            txtNetWeight_Wet.Enabled = False
            txtItemRate_Amt.Enabled = False
            cmbBulkLot.Enabled = False
            cmbMultipleTag.Enabled = False
            cmbSubItemName_Man.Enabled = False
            txtNoOfTags_Num.Enabled = False

            ''SUBITEMSETTING
            strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(txtItemCode_Num_Man.Text))
            'strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = '" & txtItemCode_Num_Man.Text & "' AND ACTIVE = 'Y' ORDER BY SUBITEMNAME"
            cmbSubItemName_Man.Items.Clear()
            cmbSubItemName_Man.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbSubItemName_Man, False)
            cmbSubItemName_Man.Text = "ALL"
            If Not cmbSubItemName_Man.Items.Count > 1 Then
                cmbSubItemName_Man.Enabled = False
            Else
                cmbSubItemName_Man.Enabled = True
            End If

            With dt.Rows(0)
                Select Case .Item("VALUEADDEDTYPE").ToString
                    Case "I"
                        cmbValueAdded.Text = "ITEM"
                    Case "T"
                        cmbValueAdded.Text = "TABLE"
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
                    txtGrossWeight_Wet.Enabled = True
                    txtNetWeight_Wet.Enabled = True
                    If WTITEMBULKTAG Then cmbBulkLot.Enabled = True : cmbMultipleTag.Enabled = True
                End If
                If .Item("CALTYPE").ToString = "M" Then txtItemRate_Amt.Enabled = True
                If .Item("ITEMCTRNAME").ToString <> "" Then cmbItemCounter_MAN.Text = .Item("ITEMCTRNAME").ToString
                If cmbEntryType.Text = "ORDER" Then SETCOUNTERNAME(ORDERCOUNTERID)
                If cmbEntryType.Text = "REPAIR" Then SETCOUNTERNAME(REPAIRCOUNTERID)
                If .Item("SUBITEM").ToString = "Y" Then cmbSubItemName_Man.Enabled = True
                If .Item("STOCKTYPE").ToString = "T" Then txtNoOfTags_Num.Enabled = True Else txtNoOfTags_Num.Clear()
                If .Item("STUDDEDSTONE").ToString = "Y" Then
                    grpStone.Enabled = True
                    grpDiamond.Enabled = True
                End If
                If .Item("HALLMARK").ToString = "Y" Then
                    CmbHallmark.Text = "YES"
                Else

                    CmbHallmark.Text = "NO"
                End If
            End With

            strSql = "SELECT ALLOWZEROPCS FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & txtItemCode_Num_Man.Text & "'"
            Dim dta As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dta)
            If dta.Rows.Count > 0 Then
                If dta.Rows.Item(0).Item("ALLOWZEROPCS").ToString = "Y" Then
                    txtPiece_Num_Man.Text = "0"
                    'SendKeys.Send("{TAB}")
                    'txtPiece_Num_Man.Focus()
                Else
                    'txtPiece_Num_Man.Focus()
                    'If e.KeyChar = Chr(Keys.Enter) Then
                    '    MsgBox("Piece(s) should not Empty", MsgBoxStyle.Information)
                    'End If
                End If
            End If
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
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.CurrentRow Is Nothing Then Exit Sub
            With itemDt.Rows(gridView.CurrentRow.Index)
                cmbEntryType.Text = .Item("ENTRYTYPE").ToString
                dtpDate.Value = .Item("LOTDATE").ToString
                cmbSupplier_Man_OWN.Text = .Item("DESIGNERNAME").ToString
                txtTransferInvNo.Text = .Item("TRANINVNO").ToString
                txtBillNo.Text = .Item("BILLNO").ToString
                cmbCostCentre_Man.Text = .Item("COSTCENTRE").ToString
                OrdRepairNo = .Item("ORDREPNO").ToString
                If OrdRepairNo.Length > 2 Then OrdCostId = OrdRepairNo.ToString.Substring(0, 2)
                If OrdRepairNo.Length > 5 Then OrdCompanyId = OrdRepairNo.ToString.Substring(2, 3)
                If OrdRepairNo.Length > 6 Then
                    txtOrderRepairNo.Text = OrdRepairNo.ToString.Substring(5)
                Else
                    txtOrderRepairNo.Text = .Item("ORDREPNO").ToString
                End If
                txtItemName.Text = .Item("ITEMNAME").ToString
                txtItemCode_Num_Man.Text = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "'")
                cmbSubItemName_Man.Text = .Item("SUBITEMNAME").ToString
                txtPiece_Num_Man.Text = .Item("PIECES").ToString
                txtGrossWeight_Wet.Text = .Item("GRSWT").ToString
                txtStonePieces_Num.Text = .Item("STNPCS").ToString
                txtStoneWeight_Wet.Text = .Item("STNWT").ToString
                cmbStoneUnit.Text = .Item("STNUNIT").ToString
                txtDiamondPieces_Num.Text = .Item("DIAPCS").ToString
                txtDidmondWeight_Wet.Text = .Item("DIAWT").ToString
                txtNetWeight_Wet.Text = .Item("NETWT").ToString
                txtNoOfTags_Num.Text = .Item("NOOFTAG").ToString
                txtItemRate_Amt.Text = .Item("RATE").ToString
                cmbItemCounter_MAN.Text = .Item("ITEMCTRNAME").ToString
                If .Item("VALUEADDEDTYPE").ToString = "I" Then
                    cmbValueAdded.Text = "ITEM"
                ElseIf .Item("VALUEADDEDTYPE").ToString = "T" Then
                    cmbValueAdded.Text = "TABLE"
                ElseIf .Item("VALUEADDEDTYPE").ToString = "D" Then
                    cmbValueAdded.Text = "DESIGNER"
                ElseIf .Item("VALUEADDEDTYPE").ToString = "P" Then
                    cmbValueAdded.Text = "TAG"
                End If
                cmbBulkLot.Text = .Item("BULKLOT").ToString
                cmbMultipleTag.Text = .Item("MULTIPLETAGS").ToString
                cmbNarration_OWN.Text = .Item("NARRATION").ToString
                txtFineRate_Amt.Text = .Item("FINERATE").ToString
                txtTuch_Amt.Text = .Item("TUCH").ToString
                txtWastage_Per.Text = .Item("WASTPER").ToString
                txtMakingPerGrm_Amt.Text = .Item("MCGRM").ToString
                txtOtherCharges_Amt.Text = .Item("OTHCHARGE").ToString
                txtNonTagItem.Text = .Item("NONTAGITEM").ToString
                txtNonTagSubItem.Text = .Item("NONTAGSUBITEM").ToString
                txtNonTagPcs.Text = .Item("NONTAGPCS").ToString
                txtNonTagGrsWt.Text = .Item("NONTAGGRSWT").ToString
                If .Item("RANGESNO").ToString <> "" And cmbItemRange_OWN.Visible Then
                    cmbItemRange_OWN.Text = objGPack.GetSqlValue("SELECT TOP 1 CAPTION FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("RANGESNO").ToString & "'")
                End If
                If .Item("HALLMARK").ToString = "Y" Then
                    CmbHallmark.Text = "YES"
                Else
                    CmbHallmark.Text = "NO"
                End If
                locUpdateRow = gridView.CurrentRow.Index
                cmbEntryType.Focus()
            End With
        End If
    End Sub

    Private Sub gridView_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridView.UserDeletedRow
        itemDt.AcceptChanges()
        If Not gridView.RowCount > 0 Then btnSave.Enabled = False
    End Sub

    Private Sub txtNetWeight_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNetWeight_Wet.GotFocus
        If Val(txtGrossWeight_Wet.Text) = 0 Then
            SendKeys.Send("{TAB}")
        End If
        CalcNetWt()
    End Sub

    Private Sub txtStoneWeight_Wet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStoneWeight_Wet.LostFocus
        CalcNetWt()
    End Sub

    Private Sub txtDidmondWeight_Wet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDidmondWeight_Wet.LostFocus
        CalcNetWt()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If CmbStockType.Text = "" Then MsgBox("StockType Should not Empty.", MsgBoxStyle.Information) : CmbStockType.Focus() : Exit Sub
        If (cmbEntryType.Text = "ORDER" Or cmbEntryType.Text = "REPAIR") And txtOrderRepairNo.Text <> "" Then
            strSql = " SELECT 1 CHKCOL FROM " & cnadmindb & "..ORMAST AS I"
            strSql += " WHERE NOT EXISTS (SELECT 1 FROM " & cnadmindb & "..ORIRDETAIL WHERE ORSTATUS = 'R' AND ORSNO = I.SNO  AND ISNULL(CANCEL,'') = '' AND ORDSTATE_ID = 4)"
            strSql += " AND I.COMPANYID = '" & strCompanyId & "' AND ISNULL(ORNO,'') = '" & OrdRepairNo & "'"
            Dim dtOrdChk As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtOrdChk)
            If Not dtOrdChk.Rows.Count > 0 Then
                MessageBox.Show("Invalid " & cmbEntryType.Text & " No")
                Return
            End If
        End If
        If GetAdmindbSoftValue("COSTCENTRE", "N").ToString = "Y" Then
            If cmbCostCentre_Man.Text = "" Then
                MsgBox("Costcentre Should not be empty", MsgBoxStyle.Information)
                cmbCostCentre_Man.Text = cnCostName
                Return
            End If
        End If
        funcLoadGrid()

        If gridViewTotal.Visible = False Then
            gridViewTotal.Visible = True
        End If

        If gridViewTotal.Rows.Count > 0 Then
            With gridViewTotal
                .Enabled = False
                .ColumnHeadersVisible = False
                If gridView.Columns.Count > 0 Then
                    For cnt As Integer = 0 To gridViewTotal.ColumnCount - 1
                        .Columns(cnt).Width = gridView.Columns(cnt).Width
                    Next
                    .Rows(0).Cells("ITEMNAME").Value = "TOTAL =>"
                End If
            End With
        End If
    End Sub

    Private Sub txtItemRate_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemRate_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbBulkLot.Text = "YES" Then
                If Not Val(txtItemRate_Amt.Text) > 0 Then
                    MsgBox(Me.GetNextControl(txtItemRate_Amt, False).Text + E0001, MsgBoxStyle.Information)
                    txtItemRate_Amt.Focus()
                End If
            End If
        End If
    End Sub

    'Private Sub txtNetWeight_Wet_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNetWeight_Wet_MAN.KeyPress
    '    If Val(txtGrossWeight_Wet_MAN.Text) < Val(txtNetWeight_Wet_MAN.Text + e.KeyChar) Then
    '        'e.Handled = True
    '        MsgBox("Net weight should not exeed Gross weight", MsgBoxStyle.Information)
    '    End If
    'End Sub

    Private Sub txtNetWeight_Wet_MAN_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNetWeight_Wet.KeyUp
        If Val(txtGrossWeight_Wet.Text) < Val(txtNetWeight_Wet.Text) Then
            MsgBox("Net weight should not exeed Gross weight", MsgBoxStyle.Information)
            txtNetWeight_Wet.Text = Val(txtNetWeight_Wet.Text) - Val(Chr(e.KeyCode))
            txtNetWeight_Wet.SelectAll()
        End If
    End Sub


    Private Sub txtItemCode_Num_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemCode_Num_Man.LostFocus
        Main.HideHelpText()
        Dim pieceRate As Double = Val(objGPack.GetSqlValue("SELECT PIECERATE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName.Text & "'"))
        txtItemRate_Amt.Text = IIf(pieceRate <> 0, Format(pieceRate, "0.00"), Nothing)
        If txtOrderRepairNo.Text <> "" And Val(txtItemCode_Num_Man.Text) <> 0 Then
            If cmbEntryType.Text = "REPAIR" Then
                ''strSql = "SELECT TOP 1 SUM(GRSWT) FROM " & cnAdminDb & "..ORIRDETAIL WHERE SUBSTRING(ORNO,6,12) = '" & txtOrderRepairNo.Text & "' AND ORSTATUS ='R' GROUP BY SNO ORDER BY SNO"
                If OrdSNO <> "" Then
                    strSql = "SELECT TOP 1 SUM(GRSWT) FROM " & cnAdminDb & "..ORIRDETAIL WHERE SUBSTRING(ORNO,6,12) = '" & txtOrderRepairNo.Text & "' AND ORSNO='" & OrdSNO & "' AND ORSTATUS ='R' GROUP BY SNO ORDER BY SNO"
                Else
                    strSql = "SELECT TOP 1 SUM(GRSWT) FROM " & cnAdminDb & "..ORIRDETAIL WHERE SUBSTRING(ORNO,6,12) = '" & txtOrderRepairNo.Text & "' AND ORSTATUS ='R' GROUP BY SNO ORDER BY SNO"
                End If
            Else
                ''strSql = "SELECT SUM(GRSWT) FROM " & cnAdminDb & "..ORMAST WHERE SUBSTRING(ORNO,6,12) = '" & txtOrderRepairNo.Text & "' AND ITEMID = " & txtItemCode_Num_Man.Text
                If OrdSNO <> "" Then
                    strSql = "SELECT SUM(GRSWT) FROM " & cnAdminDb & "..ORMAST WHERE SUBSTRING(ORNO,6,12) = '" & txtOrderRepairNo.Text & "' AND SNO='" & OrdSNO & "' AND ITEMID = " & txtItemCode_Num_Man.Text
                Else
                    strSql = "SELECT SUM(GRSWT) FROM " & cnAdminDb & "..ORMAST WHERE SUBSTRING(ORNO,6,12) = '" & txtOrderRepairNo.Text & "' AND ITEMID = " & txtItemCode_Num_Man.Text
                End If
            End If
            txtGrossWeight_Wet.Text = Format(Val(objGPack.GetSqlValue(strSql).ToString), "0.00")
            If Val(txtGrossWeight_Wet.Text) = 0 And cmbEntryType.Text = "REPAIR" Then
                ''strSql = "SELECT SUM(GRSWT) FROM " & cnAdminDb & "..ORMAST WHERE SUBSTRING(ORNO,6,12) = '" & txtOrderRepairNo.Text & "' AND ITEMID = " & txtItemCode_Num_Man.Text
                If OrdSNO <> "" Then
                    strSql = "SELECT SUM(GRSWT) FROM " & cnAdminDb & "..ORMAST WHERE SUBSTRING(ORNO,6,12) = '" & txtOrderRepairNo.Text & "' AND SNO='" & OrdSNO & "' AND ITEMID = " & txtItemCode_Num_Man.Text
                Else
                    strSql = "SELECT SUM(GRSWT) FROM " & cnAdminDb & "..ORMAST WHERE SUBSTRING(ORNO,6,12) = '" & txtOrderRepairNo.Text & "' AND ITEMID = " & txtItemCode_Num_Man.Text
                End If
                txtGrossWeight_Wet.Text = Format(Val(objGPack.GetSqlValue(strSql).ToString), "0.00")
            End If
        End If
    End Sub

    Private Sub txtPiece_Num_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPiece_Num_Man.KeyPress
        'If cmbEntryType.Text = "NONTAG TO TAG" Then
        '    If tabCheckBy = "P" Then
        '        If Val(txtPiece_Num_Man.Text + e.KeyChar) > Val(txtNonTagPcs.Text) Then
        '            e.Handled = True
        '        End If
        '    End If
        'End If
    End Sub

    Private Sub txtWastage_Per_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWastage_Per.GotFocus
        If Val(txtTuch_Amt.Text) > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbSupplier_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSupplier_Man_OWN.GotFocus
        cmbSupplier_Man_OWN.BackColor = GlobalVariables.focusColor
        If tagMade Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
    End Sub

    Private Sub txtPiece_Num_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPiece_Num_Man.LostFocus
        If flagUpdate And Val(txtPiece_Num_Man.Text) + Val(Tag_Tolerance) < cPcs Then
            MsgBox("Completed Pcs : " & cPcs & vbCrLf & "You must enter minimum of completed pcs", MsgBoxStyle.Information)
            txtPiece_Num_Man.Text = cPcs.ToString
            txtPiece_Num_Man.Focus()
            txtPiece_Num_Man.SelectAll()
        End If
    End Sub

    Private Sub cmbCostCentre_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre_Man.GotFocus
        If cmbCostCentre_Man.Text = "" And cmbCostCentre_Man.Items.Count > 0 Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID"
            strSql += " = (SELECT TOP 1 COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COMPID = '" & strCompanyId & "')"
            If SPECIFICFORMAT.ToString = "1" Then
                strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COMPANYID LIKE '%" & strCompanyId & "%')"
                strSql += " order by CostName"
                cmbCostCentre_Man.Text = objGPack.GetSqlValue(strSql)
                cmbCostCentre_Man.Text = cnCostName
                If TAGEDITCOSTCENTRE = False Then
                    cmbCostCentre_Man.Enabled = False
                Else
                    cmbCostCentre_Man.Enabled = True
                    cmbCostCentre_Man.SelectAll()
                End If
                cmbCostCentre_Man.SelectAll()
            Else
                strSql += " order by CostName"
                cmbCostCentre_Man.Text = objGPack.GetSqlValue(strSql)
                cmbCostCentre_Man.Text = cnCostName
                If TAGEDITCOSTCENTRE = False Then
                    cmbCostCentre_Man.Enabled = False
                Else
                    cmbCostCentre_Man.Enabled = True
                    cmbCostCentre_Man.SelectAll()
                End If
                cmbCostCentre_Man.SelectAll()
            End If
        End If
    End Sub

    Private Sub cmbValueAdded_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbValueAdded.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbValueAdded.Text = "TABLE" Then
                CmbTableCode.Enabled = True
                strSql = " SELECT DISTINCT TABLECODE FROM " & cnAdminDb & "..WMCTABLE WHERE TABLECODE <> '' "
                strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
                strSql += " ORDER BY TABLECODE"
                objGPack.FillCombo(strSql, CmbTableCode)
            Else    
                CmbTableCode.Items.Clear()
                CmbTableCode.Enabled = False
            End If
        End If
    End Sub


    Private Sub gridView_RowsAdded(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles gridView.RowsAdded
        CalcGridViewTotal()
    End Sub

    Private Sub cmbSupplier_Man_OWN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbSupplier_Man_OWN.KeyDown
        If e.KeyCode = Keys.Insert Then  'Insert Key
            strSql = " SELECT DESIGNERID,DESIGNERNAME,SEAL"
            strSql += " FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'') <> 'N'"
            cmbSupplier_Man_OWN.Text = BrighttechPack.SearchDialog.Show("Search DesignerName", strSql, cn, 2, 1)
            cmbSupplier_Man_OWN.SelectAll()
        End If
    End Sub

    Private Sub cmbSupplier_Man_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSupplier_Man_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbSupplier_Man_OWN.Items.Contains(cmbSupplier_Man_OWN.Text) = False Then
                Dim desName As String = ""
                strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'') <> 'N' AND SEAL = '" & cmbSupplier_Man_OWN.Text & "' AND ISNULL(SEAL,'') <> ''"
                desName = objGPack.GetSqlValue(strSql)
                If desName = "" Then
                    strSql = " SELECT SEAL,DESIGNERID,DESIGNERNAME"
                    strSql += " FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'') <> 'N'"
                    cmbSupplier_Man_OWN.Text = BrighttechPack.SearchDialog.Show("Search DesignerName", strSql, cn, 0, 2)
                    cmbSupplier_Man_OWN.SelectAll()
                Else
                    cmbSupplier_Man_OWN.Text = desName  
                    SendKeys.Send("{TAB}")
                End If
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

   
    Private Sub cmbSupplier_Man_OWN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSupplier_Man_OWN.LostFocus
        cmbSupplier_Man_OWN.BackColor = GlobalVariables.lostFocusColor
    End Sub

    Private Sub txtOrderRepairNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOrderRepairNo.KeyDown
            If e.KeyCode = Keys.Insert Then
            If cmbEntryType.Text.ToString().ToUpper = "ORDER" Or cmbEntryType.Text.ToString().ToUpper = "REPAIR" Or MoveOrder = True Then
                Dim mcostid As String = Nothing
                If cmbCostCentre_Man.Enabled = True Then
                    mcostid = objGPack.GetSqlValue("select COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME ='" & cmbCostCentre_Man.Text & "'").ToString()
                End If
                'strSql = " SELECT DISTINCT SUBSTRING(ORNO,6,20) ORNO,ORDATE TRANDATE,SUBSTRING(ORNO,1,2) COSTID ,SUBSTRING(ORNO,3,3) COMPANYID FROM " & cnadmindb & "..ORIRDETAIL AS I"
                'strSql += " WHERE NOT EXISTS (SELECT 1 FROM " & cnadmindb & "..ORIRDETAIL WHERE ORSTATUS = 'R' AND ORSNO = I.ORSNO  AND ISNULL(CANCEL,'') = '' AND ORDSTATE_ID = 4)"
                strSql = " SELECT DISTINCT SUBSTRING(ORNO,6,20) ORNO,ORDATE TRANDATE,SUBSTRING(ORNO,1,2) COSTID ,SUBSTRING(ORNO,3,3) COMPANYID "
                strSql += ",P.PNAME AS CUSTOMER"
                strSql += ",IT.ITEMNAME AS ITEMNAME,I.GRSWT,I.SNO"
                strSql += " FROM " & cnAdminDb & "..ORMAST AS I"
                strSql += " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO AS C ON C.BATCHNO = I.BATCHNO"
                strSql += " LEFT JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO =C.PSNO"
                strSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID =I.ITEMID"
                strSql += " WHERE NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSTATUS = 'R' AND ORSNO = I.SNO  AND ISNULL(CANCEL,'') = '' AND ORDSTATE_ID = 4)"
                strSql += " AND I.COMPANYID = '" & strCompanyId & "' AND ISNULL(ODBATCHNO,'') = ''  "
                strSql += " AND ISNULL(I.CANCEL,'') = ''"
                strSql += " AND ISNULL(I.ORDCANCEL,'') = '' "
                If cmbCostCentre_Man.Enabled = True And CHKCOST_ORD = True Then strSql += " AND ISNULL(I.COSTID,'')= '" & mcostid & "'"
                Dim dr_Ord As DataRow
                If cmbEntryType.Text.ToString().ToUpper = "ORDER" Or MoveOrder Then
                    strSql += " AND SUBSTRING(ORNO,6,1) = 'O'"
                    dr_Ord = BrighttechPack.SearchDialog.Show_R("Select Order No", strSql, cn, , , , , , False)
                Else
                    strSql += " AND SUBSTRING(ORNO,6,1) = 'R'"
                    dr_Ord = BrighttechPack.SearchDialog.Show_R("Select Repair No", strSql, cn, , , , , , False)
                End If
                If dr_Ord IsNot Nothing Then
                    txtOrderRepairNo.Text = dr_Ord.Item("ORNO").ToString
                    OrdCostId = dr_Ord.Item("COSTID").ToString
                    OrdCompanyId = dr_Ord.Item("COMPANYID").ToString
                    strSql = " SELECT ITEMID,STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & dr_Ord.Item("itemname") & "'"
                    Dim dtitemr As DataRow
                    dtitemr = GetSqlRow(strSql, cn)
                    If Not dtitemr Is Nothing Then txtItemCode_Num_Man.Text = dtitemr.Item(0).ToString
                    txtItemName.Text = dr_Ord.Item("Itemname").ToString
                End If
                Me.SelectNextControl(txtOrderRepairNo, True, True, True, True)
            End If
        End If
    End Sub

    Private Sub txtOrderRepairNo_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOrderRepairNo.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If txtOrderRepairNo.Text <> "" Then
                Dim dr_Ord As DataRow
                If cmbEntryType.Text.ToString().ToUpper = "ORDER" Or cmbEntryType.Text.ToString().ToUpper = "REPAIR" Or MoveOrder = True Then
                    Dim mcostid As String = Nothing
                    If cmbCostCentre_Man.Enabled = True Then
                        mcostid = objGPack.GetSqlValue("select COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME ='" & cmbCostCentre_Man.Text & "'").ToString()
                    End If
                    strSql = " SELECT DISTINCT SUBSTRING(ORNO,6,20) ORNO,ORDATE TRANDATE,SUBSTRING(ORNO,1,2) COSTID ,SUBSTRING(ORNO,3,3) COMPANYID "
                    strSql += ",P.PNAME AS CUSTOMER"
                    strSql += ",IT.ITEMNAME AS ITEMNAME,I.GRSWT,I.SNO"
                    strSql += " FROM " & cnAdminDb & "..ORMAST AS I"
                    strSql += " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO AS C ON C.BATCHNO = I.BATCHNO"
                    strSql += " LEFT JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO =C.PSNO"
                    strSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID =I.ITEMID"
                    strSql += " WHERE NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSTATUS = 'R' AND ORSNO = I.SNO  AND ISNULL(CANCEL,'') = '' AND ORDSTATE_ID = 4)"
                    strSql += " AND I.COMPANYID = '" & strCompanyId & "' AND ISNULL(ODBATCHNO,'') = ''  "
                    strSql += " AND ISNULL(I.CANCEL,'') = ''"
                    strSql += " AND ISNULL(I.ORDCANCEL,'') = '' "
                    If cmbCostCentre_Man.Enabled = True And CHKCOST_ORD = True Then strSql += " AND ISNULL(I.COSTID,'')= '" & mcostid & "'"
                    Dim dt_Ord As New DataTable
                    If cmbEntryType.Text.ToString().ToUpper = "ORDER" Or MoveOrder Then
                        strSql += " AND SUBSTRING(ORNO,6,1) = 'O'"
                    Else
                        strSql += " AND SUBSTRING(ORNO,6,1) = 'R'"
                    End If
                    strSql += " AND SUBSTRING(ORNO,6,10) = '" & txtOrderRepairNo.Text & "'"
                    dr_Ord = GetSqlRow(strSql, cn)
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dt_Ord)
                    Dim mitemname As String
                    If dt_Ord.Rows.Count > 1 Then
                        dr_Ord = BrighttechPack.SearchDialog.Show_R("Select Item ", strSql, cn, , , , , , False)
                        If dr_Ord Is Nothing Then Exit Sub
                        txtOrderRepairNo.Text = dr_Ord.Item("ORNO").ToString
                        OrdCostId = dr_Ord.Item("COSTID").ToString
                        OrdCompanyId = dr_Ord.Item("COMPANYID").ToString
                        mitemname = dr_Ord.Item("Itemname")
                        OrdSNO = dr_Ord.Item("SNO")
                    ElseIf dt_Ord.Rows.Count > 0 Then
                        mitemname = dt_Ord.Rows(0).Item("itemname")
                        OrdCostId = dt_Ord.Rows(0).Item("COSTID").ToString
                        OrdCompanyId = dt_Ord.Rows(0).Item("COMPANYID").ToString
                        OrdSNO = dr_Ord.Item("SNO")
                    Else
                        MsgBox("Invalid " & cmbEntryType.Text & " No. Please check")
                        txtOrderRepairNo.Text = ""

                        Exit Sub

                    End If
                    strSql = " SELECT ITEMID,STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & mitemname & "'"
                    Dim dtitemr As DataRow
                    dtitemr = GetSqlRow(strSql, cn)
                    If Not dtitemr Is Nothing Then txtItemCode_Num_Man.Text = dtitemr.Item(0).ToString
                    txtItemName.Text = mitemname
                    Me.SelectNextControl(txtOrderRepairNo, True, True, True, True)
                End If
            End If
        End If
    End Sub

    Private Sub txtOrderRepairNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOrderRepairNo.LostFocus
        If txtOrderRepairNo.Text = "" Then
            OrdRepairNo = ""
        Else
            If OrdCostId = "" Then OrdCostId = cnCostId
            If OrdCompanyId = "" Then OrdCompanyId = strCompanyId
            OrdRepairNo = GetCostId(OrdCostId) + OrdCompanyId + txtOrderRepairNo.Text
        End If
    End Sub

    Private Sub txtPiece_Num_Man_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPiece_Num_Man.Enter
        'strSql = "SELECT ALLOWZEROPCS FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & txtItemCode_Num_Man.Text & "'"
        'Dim dt As New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    If dt.Rows.Item(0).Item("ALLOWZEROPCS").ToString = "Y" Then
        '        txtPiece_Num_Man.Text = "0"
        '        SendKeys.Send("{TAB}")
        '    Else

        '    End If
        'End If
    End Sub

    Private Sub txtPiece_Num_Man_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPiece_Num_Man.Leave
        If txtPiece_Num_Man.Text <> "" Then
            If Val(txtPiece_Num_Man.Text) > 0 Then
                ' txtGrossWeight_Wet.Focus()
                If txtNoOfTags_Num.Enabled Then txtNoOfTags_Num.Focus() Else txtGrossWeight_Wet.Focus() : Exit Sub
                ' txtNoOfTags_Num.Focus()
            Else
                strSql = "SELECT ALLOWZEROPCS FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & txtItemCode_Num_Man.Text & "'"
                Dim dt As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If dt.Rows.Item(0).Item("ALLOWZEROPCS").ToString = "Y" Then
                        txtPiece_Num_Man.Text = "0"
                        'txtGrossWeight_Wet.Focus()
                        If txtNoOfTags_Num.Enabled Then txtNoOfTags_Num.Focus() Else txtGrossWeight_Wet.Focus()
                        'txtNoOfTags_Num.Focus()
                    Else
                        If txtPiece_Num_Man.Text = "0" Or txtPiece_Num_Man.Text = "" Then
                            txtPiece_Num_Man.Focus()
                            'If e.KeyChar = Chr(Keys.Enter) Then
                            MsgBox("Piece(s) should not Empty")
                        Else
                            txtGrossWeight_Wet.Focus()
                            If txtNoOfTags_Num.Enabled Then txtNoOfTags_Num.Focus() Else txtGrossWeight_Wet.Focus()
                            'End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub txtItemCode_Num_Man_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtItemCode_Num_Man.TextChanged

    End Sub

    Private Sub cmbCostCentre_Man_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbCostCentre_Man.KeyDown
        If e.KeyCode = Keys.Escape Then cmbEntryType.Text = "" : cmbEntryType.Focus() : Exit Sub
        If e.KeyCode = Keys.Enter Then
            If cmbEntryType.Text = "NONTAG TO TAG" Then
                If cmbCostCentre_Man.Text <> "" Then
                    Dim values As Boolean = GetNontagdetails()
                    If values = False Then cmbCostCentre_Man.Text = "" : cmbCostCentre_Man.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub cmbCostCentre_Man_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre_Man.Leave
        'If cmbEntryType.Text = "NONTAG TO TAG" Then
        '    If cmbCostCentre_Man.Text <> "" Then
        '        Dim values As Boolean = GetNontagdetails()
        '        If values = False Then cmbCostCentre_Man.Text = "" : cmbCostCentre_Man.Focus()
        '    End If
        'End If
    End Sub

    Private Sub txtBillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBillNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) And cmbCostCentre_Man.Enabled = False Then
            If cmbEntryType.Text = "NONTAG TO TAG" Then
                Dim values As Boolean = GetNontagdetails()
                If values = False Then cmbEntryType.Focus()
            End If
        End If
    End Sub

    Private Sub txtOrderRepairNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOrderRepairNo.TextChanged

    End Sub

    Private Sub CmbTableCode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbTableCode.SelectedIndexChanged

    End Sub

    Private Sub txtSaleRate_Per_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSaleRate_Per.TextChanged
        txtItemRate_Amt.Text = Val(txtFineRate_Amt.Text) + (Val(txtFineRate_Amt.Text) * (Val(txtSaleRate_Per.Text) / 100))
    End Sub

    Private Sub txtItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemName.KeyPress

    End Sub

    Private Sub cmbItemRange_OWN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemRange_OWN.Leave
        If cmbItemRange_OWN.Items.Count > 0 And cmbItemRange_OWN.Text = "" Then MsgBox("Range should not Empty") : cmbItemRange_OWN.Select() : Exit Sub
        If cmbItemRange_OWN.Text <> "" Then
            If cmbItemRange_OWN.Items.Contains(cmbItemRange_OWN.Text) = False Then
                MsgBox("Invalid Range...", MsgBoxStyle.Information)
                cmbItemRange_OWN.Select()
            End If
        End If
    End Sub

    Private Sub CmbStockType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbStockType.KeyDown
        If e.KeyCode = Keys.Enter Then
            If CmbStockType.Text = "" Then MsgBox("StockType Should not Empty.", MsgBoxStyle.Information) : CmbStockType.Focus()
        End If
    End Sub

    Private Sub btnTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        If CmbCostCentre.Text = CmbOpenCostcentre.Text Then
            MsgBox("Not able to Transfer to same Location.", MsgBoxStyle.Information)
            Exit Sub
        End If
        If MessageBox.Show("Sure, Do you want to transfer the Lot?", "Transfer Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        Try
            tran = cn.BeginTransaction
            For I As Integer = 0 To gridViewOpen.RowCount - 1
                With gridViewOpen.Rows(I)
                    strSql = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMLOT SET COSTID = '" & LOT_TRANSFER_COSTID & "' WHERE SNO='" & .Cells("SNO").Value.ToString & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()

                    strSql = " IF OBJECT_ID('" & cnStockDb & "..INS_ITEMLOT', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_ITEMLOT"
                    strSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_ITEMLOT FROM " & cnAdminDb & "..ITEMLOT WHERE SNO='" & .Cells("SNO").Value.ToString & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()

                    Dim mtempqrytb As String = "TEMPQRYTB"
                    strSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLENEW "
                    strSql += vbCrLf + " @DBNAME = '" & cnAdminDb & "',@TABLENAME = 'INS_ITEMLOT',@MASK_TABLENAME = 'ITEMLOT',@TEMPTABLE='" & mtempqrytb & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()

                    strSql = " INSERT INTO " & Mid(cnAdminDb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)    "
                    strSql += vbCrLf + " VALUES('" & cnCostId & "','" & LOT_TRANSFER_COSTID & "','DELETE FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=''" & .Cells("SNO").Value.ToString & "'' ','N') "
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()

                    strSql = " INSERT INTO " & Mid(cnAdminDb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
                    strSql += vbCrLf + " SELECT '" & cnCostId & "','" & LOT_TRANSFER_COSTID & "', SQLTEXT,'N' AS STATUS,'' UPDFILE FROM " & cnStockDb & ".." & mtempqrytb
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()

                End With
            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Tranfered Sucessfully.", MsgBoxStyle.Information)
            gridViewOpen.DataSource = Nothing
            gridViewOpen.Refresh()
            txtOpenLotNo.Focus()
        Catch ex As Exception
            If Not tran Is Nothing Then
                tran.Rollback()
                tran = Nothing
                MsgBox(ex.ToString + vbCrLf + ex.StackTrace, MsgBoxStyle.Information)
            End If
        End Try
    End Sub
End Class