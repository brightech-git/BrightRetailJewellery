Imports System.Data.OleDb
Public Class frmitemBulkTag
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dReader As OleDbDataReader
    Dim tran As OleDbTransaction
    Dim MetalId As String
    Dim strSql As String

    Dim Sno As String = Nothing
    Dim EntryOrder As Integer = 0
    Dim multipleTagFlag As String = Nothing

    Dim pcs As Integer = 0
    Dim narration As String
    Dim StyleNo As String
    Dim WT As Decimal = 0
    Dim pieceRate As Decimal = 0
    Dim purFineRate As Double = 0

    Dim pieceWeight As Decimal = 0
    Dim purPieceweight As Decimal = 0

    Dim purTouch As Double = 0

    Dim CallBarcodeExe As Boolean = IIf(GetAdmindbSoftValue("CALLBARCODEEXE", "N") = "Y", True, False)
    Dim TagNoGen As String = Nothing
    Dim TagNoFrom As String = Nothing
    Dim LastTagNo As String = Nothing
    Dim mItemid As Integer
    Dim objTag As New TagGeneration
    Dim mwccaltype As String
    Dim tabCheckBy As String = GetAdmindbSoftValue("LOTCHECKBY", "P")
    Dim SALEVALUEPLUS As Integer = Val(GetAdmindbSoftValue("SALEVALUEPLUS", "0"))
    Dim SALVALUEROUND As Integer = Val(GetAdmindbSoftValue("STKSALVALUEROUND", "0"))
    Dim PURTABBULKTAG As String = GetAdmindbSoftValue("PURTABBULKTAG", "N")
    Dim ObjMiscDetails As New frmMiscDetails

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim dt As New DataTable
        ''CostCentre Checking..
        strSql = " SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' AND CTLTEXT = 'Y'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Enabled = False
        End If

        If cmbCostCentre.Enabled = True Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre)
        End If

        ''LOADING ITEMTYPE
        strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbItemType)

        ''LOADING COUNTER
        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ITEMCOUNTER'")) = "Y" Then
            strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY DISPLAYORDER,ITEMCTRNAME "
            objGPack.FillCombo(strSql, cmbCounter)
            cmbCounter.Enabled = True
        Else
            cmbCounter.Enabled = False
        End If

        ''LOADING DESIGNER
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner)

        ''TAGNO GEN
        strSql = " SELECT CTLTEXT AS TAGNOGEN FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGNOGEN'"
        TagNoGen = objGPack.GetSqlValue(strSql)

        If PURTABBULKTAG = "N" Then
            txtPurPieRate_NUM.ReadOnly = True
            txtWtPur_WET.ReadOnly = True
        Else
            txtPurPieRate_NUM.ReadOnly = False
            txtWtPur_WET.ReadOnly = False
        End If

        funcNew()
    End Sub
    Function funcExit() As Integer
        Me.Close()
    End Function
    Private Sub cmbTableCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTableCode.GotFocus
        Me.cmbTableCode_SelectedIndexChanged(Me, New System.EventArgs)
    End Sub

    Private Sub cmbTableCode_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTableCode.Leave
        CalcMaxMinValues(Val(txtWt_WET.Text))
    End Sub
    Private Sub cmbTableCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTableCode.SelectedIndexChanged
        CalcMaxMinValues(Val(txtWt_WET.Text))
    End Sub

    Function funcGenPurItemTagStrSql(ByVal TagSno As String, ByVal ItemId As Integer _
                                     , ByVal TagNo As String, ByVal PurRate As Decimal _
                                     , ByVal BulkPcs As Integer, ByVal CostId As String, ByVal _purFineRate As Double) As String
        ''ITEM PUR DETAIL
        strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAG"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " TAGSNO"
        strSql += vbCrLf + " ,ITEMID"
        strSql += vbCrLf + " ,TAGNO"
        strSql += vbCrLf + " ,PURLESSWT"
        strSql += vbCrLf + " ,PURNETWT"
        strSql += vbCrLf + " ,PURRATE"
        strSql += vbCrLf + " ,PURGRSNET"
        strSql += vbCrLf + " ,PURWASTAGE"
        strSql += vbCrLf + " ,PURTOUCH"
        strSql += vbCrLf + " ,PURMC"
        strSql += vbCrLf + " ,PURVALUE"
        strSql += vbCrLf + " ,PURTAX"
        strSql += vbCrLf + " ,RECDATE"
        strSql += vbCrLf + " ,COMPANYID,COSTID"
        strSql += vbCrLf + " ,USERID"
        strSql += vbCrLf + " ,UPDATED"
        strSql += vbCrLf + " ,UPTIME"
        strSql += vbCrLf + " )"
        strSql += vbCrLf + " VALUES"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " '" & TagSno & "'" 'TAGSNO
        strSql += vbCrLf + " ," & ItemId & "" 'ITEMID
        strSql += vbCrLf + " ,'" & TagNo & "'" 'TAGNO
        strSql += vbCrLf + " ,0" ' PURLESSWT
        strSql += vbCrLf + " ," & purPieceweight & "" ' PURNETWT" 'strSql += vbCrLf + " ,0" ' PURNETWT"
        strSql += vbCrLf + " ," & _purFineRate & "" 'PURRATE
        strSql += vbCrLf + " ,'N'" 'PURGRSNET"
        strSql += vbCrLf + " ," & Val(txtPurWastageGram_wet.Text) & "" 'PURWASTAGE"
        'strSql += vbCrLf + " ,0" 'PURTOUCH"
        strSql += vbCrLf + " , '" & Val(txtPurTouch_amt.Text) & "'" 'PURTOUCH
        strSql += vbCrLf + " ," & Val(txtpurMCAmt_AMT.Text) & "" 'PURMC"
        strSql += vbCrLf + " ," & Val(txtPURAmt_AMT.Text) & "" ' PURVALUE"'----BulkPcs * _purFineRate
        strSql += vbCrLf + " ,0" 'PURTAX
        strSql += vbCrLf + " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
        strSql += vbCrLf + " ,'" & GetStockCompId() & "'" 'COMPANYID
        strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, CostId) & "'" 'COSTID
        strSql += vbCrLf + " , '" & userId & "'" 'USERID
        strSql += vbCrLf + " , '" & Format(Now.Date, "yyyy-MM-dd") & "'" 'UPDATED
        strSql += vbCrLf + " , '" & Now.ToLongTimeString & "'" 'UPTIME
        strSql += vbCrLf + " )"
        Return strSql
    End Function
    Function funcGenItemTagStrSql(ByVal TagSno As String, ByVal COSTID As String _
    , ByVal itemId As Integer, ByVal subItemId As Integer, ByVal SizeId As String, ByVal itemCtrId As String _
    , ByVal designerId As String, ByVal tagNo As String, ByVal bulkPcs As Integer _
    , ByVal Pcsweight As Decimal, ByVal tagVAl As Long, ByVal saleMode As String, ByVal itemTypeId As String _
    , ByVal tranInvNo As String, ByVal supBillNo As String, ByVal Rate As Double) As String
        'tagval Integer to long
        Dim pctFile As String = ""
        Dim StyleCode As String = ""
        If subItemId <> 0 Then
            strSql = " SELECT PCTFILE,STYLECODE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & itemId & " AND SUBITEMID = " & subItemId & ""
        Else
            strSql = " SELECT PCTFILE,STYLECODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemId & ""
        End If
        pctFile = objGPack.GetSqlValue(strSql, "PCTFILE", , tran)
        StyleCode = objGPack.GetSqlValue(strSql, "STYLECODE", , tran)
        ''Inserting itemTag
        strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAG"
        strSql += " ("
        strSql += " SNO,RECDATE,COSTID,ITEMID,ORDREPNO,ORSNO,"
        strSql += " ORDSALMANCODE,SUBITEMID,SIZEID,ITEMCTRID,TABLECODE,DESIGNERID,"
        strSql += " TAGNO,PCS,GRSWT,"
        strSql += " LESSWT,NETWT,RATE,FINERATE,MAXWASTPER,MAXMCGRM,"
        strSql += " MAXWAST,MAXMC,MINWASTPER,MINMCGRM,MINWAST,MINMC,"
        strSql += " TAGKEY,TAGVAL,LOTSNO,COMPANYID,SALVALUE,PURITY,NARRATION,DESCRIP,"
        strSql += " REASON,ENTRYMODE,GRSNET,"
        strSql += " ISSDATE,ISSREFNO,ISSPCS,ISSWT,FROMFLAG,TOFLAG,APPROVAL,SALEMODE,"
        'strSql += " PURRATE,PURGRSNET,PURWASTAGE,PURTOUCH,PURMC,PURSTNVALUE,PURVALUE,"
        strSql += " BATCHNO,MARK,"
        strSql += " PCTFILE,OLDTAGNO,ITEMTYPEID,ACTUALRECDATE,WEIGHTUNIT,"
        strSql += " TRANSFERWT,CHKDATE,CHKTRAY,CARRYFLAG,BRANDID,PRNFLAG,"
        strSql += " MCDISCPER,WASTDISCPER,RESDATE,TRANINVNO,SUPBILLNO,WORKDAYS,"
        strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER,TCOSTID,STYLENO,TOUCH)VALUES("
        strSql += " '" & TagSno & "'" 'SNO
        strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
        strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
        strSql += " ," & itemId & "" 'ITEMID
        strSql += " ,''" 'ORDREPNO
        strSql += " ,''" 'ORSNO
        strSql += " ,''" 'ORDSALMANCODE
        strSql += " ," & subItemId & "" 'SUBITEMID
        strSql += " ,'" & SizeId & "'" 'SIZEID
        strSql += " ,'" & itemCtrId & "'" 'ITEMCTRID
        strSql += " ,'" & cmbTableCode.Text & "'"
        strSql += " ,'" & Val(designerId) & "'" 'DESIGNERID
        strSql += " ,'" & tagNo & "'" 'TAGNO
        strSql += " ," & bulkPcs & "" 'PCS
        strSql += " ," & Pcsweight 'GRSWT
        strSql += " ,0" 'LESSWT
        strSql += " ," & Pcsweight 'NETWT
        strSql += " ," & Rate & "" 'RATE'pieceRate
        strSql += " ,0" 'FINERATE

        strSql += " ," & Val(txtMaxwastper_Per.Text) 'MAXWASTPER
        strSql += " ," & Val(txtMaxMcGrm_Amt.Text) 'MAXMCGRM
        strSql += " ," & Val(txtMaxWast_wet.Text) 'Pcsweight * (Val(txtMaxwastper.Text) / 100) 'MAXWAST
        strSql += " ," & Val(txtMaxMc_amt.Text) ' Pcsweight * Val(txtMaxMcGrm.Text) 'MAXMC
        strSql += " ," & Val(txtMinwastper_Per.Text) 'MINWASTPER
        strSql += " ," & Val(txtMinMcGrm_amt.Text) 'MINMCGRM
        strSql += " ," & Val(txtMinWast_wet.Text) 'Pcsweight * (Val(txtMinwastper.Text) / 100) 'MINWAST
        strSql += " ," & Val(txtMinMc_amt.Text) 'Pcsweight * Val(txtMinMcGrm.Text) 'MINMC
        strSql += " ,'" & itemId & "" & tagNo & "'" 'TAGKEY
        strSql += " ," & tagVAl & "" 'TAGVAL
        strSql += " ,'" & Sno & "'" 'LOSNO
        strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
        strSql += " ," & Val(txtSaleValue_AMT.Text) & "" 'SALVALUE
        strSql += " ,0" 'PURITY
        strSql += " ,'" & narration & "'" 'NARRATION
        strSql += " ,''" 'DESCRIP
        strSql += " ,''" 'REASON
        strSql += " ,'A'" 'ENTRYMODE
        strSql += " ,'N'" 'GRSNET
        strSql += " ,null" 'ISSDATE
        strSql += " ,0" 'ISSREFNO
        strSql += " ,0" 'ISSPCS
        strSql += " ,0" 'ISSWT
        strSql += " ,''" 'FROMFLAG
        strSql += " ,''" 'TOFLAG
        strSql += " ,''" 'APPROVAL
        strSql += " ,'" & saleMode & "'" 'SALEMODE
        'strSql += " ," & purFineRate & "" 'PURRATE
        'strSql += " ,''" 'PURGRSNET
        'strSql += " ,0" 'PURWASTAGE
        'strSql += " ," & purTouch & "" 'PURTOUCH
        'strSql += " ,0" 'PURMC
        'strSql += " ,0" 'PURSTNVALUE
        'strSql += " ," & bulkPcs * purFineRate & "" 'PURVALUE
        strSql += " ,''" 'BATCHNO
        strSql += " ,0" 'MARK
        strSql += " ,'" & pctFile & "'" 'PCTFILE
        strSql += " ,''" 'OLDTAGNO
        strSql += " ," & Val(itemTypeId) & "" 'ITEMTYPEID
        strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'ACTUALRECDATE
        strSql += " ,''" 'WEIGHTUNIT
        strSql += " ,0" 'TRANSFERWT
        strSql += " ,''" 'CHKDATE
        strSql += " ,''" 'CHKTRAY
        strSql += " ,''" 'CARRYFLAG
        strSql += " ,''" 'BRANDID
        strSql += " ,''" 'PRNFLAG
        strSql += " ,0" 'MCDISCPER
        strSql += " ,0" 'WASTDISCPER
        strSql += " ,''" 'RESDATE
        strSql += " ,'" & tranInvNo & "'" 'TRANINVNO
        strSql += " ,'" & supBillNo & "'" 'SUPBILLNO
        strSql += " ,''" 'WORKDAYS
        strSql += " ," & userId & "" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'"
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & COSTID & "'" 'TCOSTID
        strSql += " ,'" & IIf(StyleCode = "", StyleNo, StyleCode) & "'" 'STYLECODE
        strSql += " ,'" & Val(txtTouch_amt.Text) & "'" 'TOUCH
        strSql += " )"
        Return strSql
    End Function
    Function funcNext() As Integer


        objTag = New TagGeneration
        If cmbItemType.Items.Count > 0 Then cmbItemType.Enabled = True Else cmbItemType.Enabled = False
        If Not cmbItemSize.Items.Count > 0 Then
            cmbItemSize.Enabled = False
        End If
        lblPBalance.Text = Val(lblPBalance.Text) - pcs
        lblPCompled.Text = Val(lblPCompled.Text) + pcs
        lblBalwt.Text = Val(lblBalwt.Text) - Val(totwt_WET.Text)
        lblComwt.Text = Val(lblComwt.Text) + Val(totwt_WET.Text)

        If Val(lblPBalance.Text) <> 0 Then
            EntryOrder = EntryOrder + 1
            cmbSubItem_MAN.Focus()
        Else
            Sno = Nothing
            objGPack.TextClear(Me)
            EntryOrder = 0
            multipleTagFlag = Nothing
            lblPCompled.Text = ""
            lblPLot.Text = ""
            lblPBalance.Text = ""
            lblLastTagNo.Text = ""
            lblBalwt.Text = ""
            lblComwt.Text = ""
            totwt_WET.Text = ""
            dtpRecieptDate.Value = GetEntryDate(GetServerDate)
            cmbCostCentre.Text = ""
            txtLotNo_Num_Man.Enabled = True
            txtLotNo_Num_Man.Focus()
        End If
        pieceRate = 0
        pcs = 0
    End Function

    Function funcNew() As Integer
        objGPack.TextClear(Me)
        Sno = Nothing
        EntryOrder = 0
        multipleTagFlag = Nothing
        pieceRate = 0
        pcs = 0

        lblPCompled.Text = ""
        lblPLot.Text = ""
        lblPBalance.Text = ""
        lblLastTagNo.Text = ""
        lblBalwt.Text = ""
        dtpRecieptDate.Value = GetEntryDate(GetServerDate)
        cmbCostCentre.Text = ""
        cmbItemType.Text = ""
        cmbCounter.Text = ""
        cmbDesigner.Text = ""
        cmbItemSize.Text = ""
        objTag = New TagGeneration
        ObjMiscDetails = New frmMiscDetails
        If cmbItemType.Items.Count > 0 Then cmbItemType.Enabled = True Else cmbItemType.Enabled = False
        If Not cmbItemSize.Items.Count > 0 Then
            cmbItemSize.Enabled = False
        End If

        cmbCalMode.Items.Clear()
        cmbCalMode.Items.Add("N")
        cmbCalMode.Text = "N"

        lblHint.Text = "*Hint"
        txtLotNo_Num_Man.Enabled = True
        txtLotNo_Num_Man.Focus()

    End Function
    Function funcMiscDetailsAdd(ByVal itemid As Integer, ByVal Tagno As String, ByVal costid As String, ByVal Tagsno As String)
        With ObjMiscDetails
            For cnt As Integer = 0 To .dtMiscDetails.Rows.Count - 1
                Dim miscSno As String = ""
                If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Or objTag._TagNoGen = "W" Then
                    'miscSno = IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & TagNo & cnt
                    miscSno = objTag.BasedOnSNOGenerator("", dtpRecieptDate.Value.Date, "") & cnt
                Else
                    miscSno = GetNewSno(TranSnoType.ITEMTAGMISCCHARCODE, tran, "GET_ADMINSNO_TRAN")
                End If
                With .dtMiscDetails.Rows(cnt)
                    Dim miscId As String = Nothing
                    strSql = " SELECT MISCID FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & .Item("MISC").ToString & "'"
                    miscId = Val(objGPack.GetSqlValue(strSql, "MISCID", , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGMISCCHAR"
                    strSql += " ("
                    strSql += " SNO,ITEMID,TAGNO,MISCID,AMOUNT,"
                    strSql += " TAGSNO,COSTID,SYSTEMID,APPVER,COMPANYID)VALUES("
                    strSql += " '" & miscSno & "'" 'SNO
                    strSql += " ," & itemid & "" 'ITEMID
                    strSql += " ,'" & Tagno & "'" 'TAGNO
                    strSql += " ," & miscId & "" 'MISCID
                    strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'AMOUNT
                    strSql += " ,'" & Tagsno & "'" 'TAGSNO
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, costid) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " )"
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()

                    If PURTABBULKTAG = "N" Then
                        If Val(purFineRate) <> 0 Then
                            strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGMISCCHAR"
                            strSql += vbCrLf + " ("
                            strSql += vbCrLf + " MISSNO,TAGSNO"
                            strSql += vbCrLf + " ,ITEMID"
                            strSql += vbCrLf + " ,TAGNO"
                            strSql += vbCrLf + " ,PURAMOUNT,COMPANYID,COSTID,STNSNO"
                            strSql += vbCrLf + " )"
                            strSql += vbCrLf + " VALUES"
                            strSql += vbCrLf + " ("
                            strSql += vbCrLf + " '" & miscSno & "'" 'MISSNO
                            strSql += vbCrLf + " ,'" & Tagsno & "'" 'TAGSNO
                            strSql += " ," & itemid & "" 'ITEMID
                            strSql += " ,'" & Tagno & "'" 'TAGNO
                            strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'PURAMOUNT
                            strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                            strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, costid) & "'"
                            strSql += vbCrLf + " ,'" & miscSno & "'" 'STNSNO
                            strSql += vbCrLf + " )"
                            cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                        End If
                    End If
                End With
            Next
        End With
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If Not CheckDate(dtpRecieptDate.Value) Then Exit Function
        If CheckEntryDate(dtpRecieptDate.Value) Then Exit Function

        If tabCheckBy = "P" Then
            If Val(lblPBalance.Text) < 0 Then
                MsgBox("Lot Completed")
                txtLotNo_Num_Man.Select()
                Exit Function
            End If
        ElseIf tabCheckBy = "E" Then
            If Val(lblPBalance.Text) < 0 Then
                MsgBox("Lot Completed")
                txtLotNo_Num_Man.Select()
                Exit Function
            End If
        End If

        If Val(txtWt_WET.Text) > 0 Then
            If Val(txtBoardGoldRate.Text) = 0 Then
                MsgBox("Gold Rate Should Not Empty", MsgBoxStyle.Information)
                txtBoardGoldRate.Focus()
                Exit Function
            End If
        End If

        If Val(txtSaleValue_AMT.Text) = 0 Then
            MsgBox("Sale value Should Not Empty", MsgBoxStyle.Information)
            txtSaleValue_AMT.Focus()
            Exit Function
        End If

        pcs = Val(txtNoofPcs.Text)

        'PIECE OR RATEBASE
        pieceRate = Val(txtRateid_NUM.Text) 'salerate
        purFineRate = Val(txtPurPieRate_NUM.Text) 'purchaserate
        'Weight 
        pieceWeight = Val(txtWt_WET.Text) 'saleWeight
        purPieceweight = Val(txtWtPur_WET.Text) 'purchaseweight

        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ITEMCOUNTER'")) = "Y" Then
            If cmbCounter.Text = "" Then
                MsgBox("Counter Name Should Not Empty", MsgBoxStyle.Information)
                cmbCounter.Focus()
                Exit Function
            End If
        End If

        funcAdd()
    End Function
    Function funcFindTagVal(ByVal TagNo As String) As String
        Dim tagVal As String
        ''Find TagVal
        If IsNumeric(TagNo) = True Then
            tagVal = Val(TagNo)
        Else
            Dim index As Integer = 0
            For Each c As Char In TagNo
                If Char.IsLetter(c) = True Then
                    index += 1
                Else
                    Exit For
                End If
            Next
            Dim fNo As String
            Dim sNo As String
            fNo = TagNo.Substring(0, index)
            sNo = TagNo.Substring(index, TagNo.Length - index)
            tagVal = (AscW(fNo) * 1000) + Val(sNo)
        End If
        Return tagVal
    End Function
    Private Function GetTagNo() As String
        Dim tagNo As String = Nothing
        Dim str As String = Nothing
        If GetAdmindbSoftValue("TAGNOFROM", , tran) = "I" Then ''FROM ITEMMAST OR UNIQUE
            If GetAdmindbSoftValue("TAGNOGEN", , tran) = "I" Then ''FROM ITEM
                str = " SELECT CURRENTTAGNO LASTTAGNO FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = "
                str += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
            Else ''FROM LOT
                str = " SELECT CURTAGNO AS LASTTAGNO FROM " & cnAdminDb & "..ITEMLOT WHERE ITEMID = "
                str += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
                str += " AND SNO = '" & Sno & "'"
            End If
        Else 'UNIQUE
            str = " SELECT CTLTEXT AS LASTTAGNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LASTTAGNO'"
        End If
        tagNo = objGPack.GetSqlValue(str, , "1", tran)
        Return GenTagNo(tagNo)
    End Function

    Private Sub UpdateTagNo(ByVal tagNo As String, ByVal COSTID As String)
        Dim tagPrefix As String = GetAdmindbSoftValue("TAGPREFIX", , tran)
        Dim Comp_TagPrefix As String = GetAdmindbSoftValue("TAGPREFIX_" & strCompanyId, , tran)
        If Comp_TagPrefix <> "" Then
            tagPrefix = Comp_TagPrefix
        End If
        Dim updTagNo As String = Nothing
        If tagPrefix.Length > 0 Then
            updTagNo = tagNo.Replace(tagPrefix, "")
        Else
            updTagNo = tagNo
        End If

        If GetAdmindbSoftValue("TAGNOFROM", , tran) = "I" Then ''FROM ITEMMAST OR UNIQUE
            If GetAdmindbSoftValue("TAGNOGEN", , tran) = "I" Then ''FROM ITEM
                strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO = '" & updTagNo & "' WHERE ITEMNAME = '" & txtItemName__Man.Text & "'"
            Else ''FROM LOT
                strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CURTAGNO = '" & updTagNo & "'"
            End If
        Else 'UNIQUE
            strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT  = '" & updTagNo & "' WHERE CTLID = 'LASTTAGNO'"
        End If
        ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)
    End Sub

    Private Function GenTagNo(ByRef TagNo As String) As String
        Dim str As String = Nothing
        If IsNumeric(TagNo) Then
            TagNo = Val(TagNo) + 1
        Else
            Dim fPart As String = Nothing
            Dim sPart As String = Nothing
            For Each c As Char In TagNo
                If IsNumeric(c) Then
                    sPart += c
                Else
                    fPart += c
                End If
            Next
            TagNo = fPart + (Val(sPart) + 1).ToString
        End If
        Dim tagPrefix As String = GetAdmindbSoftValue("TAGPREFIX", , tran)
        Return tagPrefix + TagNo
    End Function

    Private Function GetMetalRate() As Double
        Dim metalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "'")
        Dim rate As Double = Nothing
        strSql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
        strSql += " WHERE RDATE = '" & dtpRecieptDate.Value & "'"
        strSql += " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
        strSql += " AND METALID = '" & metalId & "'"
        strSql += " AND PURITY = '91.60' "
        If RATE_BRANCHWISE Then
            strSql += " AND COSTID='" & cnCostId & "'"
        End If
        strSql += " ORDER BY SNO DESC"
        'rate = Val(objGPack.GetSqlValue(strSql, , , tran))
        rate = Val(objGPack.GetSqlValue(strSql, , , Nothing))
        If IsDate(dtpRecieptDate.Value) Then
            Return rate
        Else
            Return 0
        End If
    End Function



    Function funcAdd() As Integer
        Dim TagSno As String = Nothing
        Dim TagNo As String = Nothing

        Dim COSTID As String = cnCostId
        Dim sizeId As Integer = Nothing
        Dim itemCtrId As Integer = Nothing
        Dim designerId As Integer = Nothing
        Dim itemTypeId As Integer = Nothing
        Dim MetalId As String
        Dim itemId As Integer = Nothing
        Dim subitemId As Integer = Nothing

        'Dim tagVal As Integer = 0
        Dim tagVal As Long = 0
        Dim saleMode As String = Nothing

        Dim tranInvNo As String = Nothing
        Dim supBillno As String = Nothing

        tran = Nothing
        ''Find ItemId
        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtItemName__Man.Text & "'"
        itemId = Val(objGPack.GetSqlValue(strSql, , , tran))

        ''FIND SUBITEMID
        subitemId = Val(objGPack.GetSqlValue(" SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "' AND ITEMID = " & itemId & "", , , tran))
        If subitemId <> 0 Then
            strSql = " SELECT PIECERATE,PIECERATE_PUR,CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = " & subitemId & " AND ITEMID = " & itemId & ""
        Else
            strSql = " SELECT PIECERATE,PIECERATE_PUR,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemId & ""
        End If
        Dim dtGetRate As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGetRate)

        If dtGetRate.Rows.Count = 0 Then
            MsgBox("No Record Found", MsgBoxStyle.Information)
            Exit Function
        End If

        Dim _Rate As Double = 0

        If rbtRateFixed.Checked = True Then
            If pieceRate = 0 Then
                If dtGetRate.Rows.Count > 0 Then
                    pieceRate = Val(dtGetRate.Rows(0).Item("PIECERATE").ToString)
                    purFineRate = Val(dtGetRate.Rows(0).Item("PIECERATE_PUR").ToString)
                End If
            End If
            If purFineRate = 0 Then
                If dtGetRate.Rows.Count > 0 Then
                    purFineRate = Val(dtGetRate.Rows(0).Item("PIECERATE_PUR").ToString)
                End If
            End If

            _Rate = pieceRate
        End If

        If rbtWeightFixed.Checked = True Then
            purFineRate = 0
            If purPieceweight > 0 Then
                purFineRate = GetMetalRate()
            End If
            _Rate = GetMetalRate()
        End If

        If dtGetRate.Rows(0).Item("CALTYPE") <> "W" Then
            If pieceRate = 0 Then
                MsgBox("PieceRate does not assign for selected Item,SubItem", MsgBoxStyle.Information)
                Exit Function
            End If
        End If

        Try
            tran = Nothing
            tran = cn.BeginTransaction()
            ''Find TagSno
            TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") ' GetTagSno()
            If cmbCostCentre.Enabled = True Then
                ''Find COSTID
                strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME  = '" & cmbCostCentre.Text & "'"
                COSTID = objGPack.GetSqlValue(strSql, , , tran)
            End If


            If cmbItemSize.Enabled = True Then
                ''Find ItemSize
                strSql = " SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & cmbItemSize.Text & "'"
                sizeId = Val(objGPack.GetSqlValue(strSql, , , tran))
            End If

            If cmbCounter.Enabled = True Then
                ''Find ItemCounter
                strSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter.Text & "'"
                itemCtrId = Val(objGPack.GetSqlValue(strSql, , , tran))
            End If
            Dim pcswt As Decimal = Val(txtWt_WET.Text)
            ''Find DesignerId
            strSql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner.Text & "'"
            designerId = Val(objGPack.GetSqlValue(strSql, , , tran))
            TagNo = objTag.GetTagNo(GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd"), txtItemName__Man.Text, Sno, tran, itemId, pieceRate)
            If TagNo = Nothing Then MsgBox("CHECK SOFTCONTROL TAGNOGEN") : tran.Rollback() : tran = Nothing : Exit Try
            tagVal = objTag.GetTagVal(TagNo, tran)
            'TagNo = GetTagNo()
            'tagVal = GetTagVal(TagNo)
            ''Find SaleMode
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & itemId & "'"
            saleMode = objGPack.GetSqlValue(strSql, , , tran)

            ''Find itemTypeId
            strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType.Text & "'"
            itemTypeId = Val(objGPack.GetSqlValue(strSql, , , tran))

            ''Find TranInvNo and SupBillNo
            strSql = " SELECT TRANINVNO,BILLNO,SUBITEMID FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & Sno & "'" '' LOTNO ='" & txtLotNo_Num_Man.Text & "' AND ENTRYORDER = '" & EntryOrder & "'"
            tranInvNo = objGPack.GetSqlValue(strSql, "TRANINVNO", , tran)
            supBillno = objGPack.GetSqlValue(strSql, "BILLNO", , tran)


            Dim dtTagPrint As New DataTable
            dtTagPrint.Columns.Add("ITEMID", GetType(Integer))
            dtTagPrint.Columns.Add("TAGNO", GetType(String))

            Dim rowTag As DataRow = Nothing
            Dim bulkPcs As Integer = pcs
            ''Inserting itemTag
            If multipleTagFlag = "N" Then
                rowTag = dtTagPrint.NewRow
                rowTag!ITEMID = itemId
                rowTag!TAGNO = TagNo
                dtTagPrint.Rows.Add(rowTag)
                strSql = funcGenItemTagStrSql(TagSno, COSTID, itemId, subitemId, sizeId, itemCtrId, designerId, TagNo,
                pcs, pcswt, tagVal, saleMode, itemTypeId, tranInvNo, supBillno, _Rate)
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                If purFineRate <> 0 Then
                    strSql = funcGenPurItemTagStrSql(TagSno, itemId, TagNo, purFineRate, pcs, COSTID, purFineRate)
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                End If
                funcMiscDetailsAdd(itemId, TagNo, COSTID, TagSno)
                UpdateTagNo(TagNo, COSTID)

            Else
                For cnt As Integer = 1 To pcs
                    rowTag = dtTagPrint.NewRow
                    rowTag!ITEMID = itemId
                    rowTag!TAGNO = TagNo
                    dtTagPrint.Rows.Add(rowTag)
                    bulkPcs = cnt
                    strSql = funcGenItemTagStrSql(TagSno, COSTID, itemId, subitemId, sizeId, itemCtrId, designerId, TagNo,
                    1, pcswt, tagVal, saleMode, itemTypeId, tranInvNo, supBillno, _Rate)
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)
                    If purFineRate <> 0 Then
                        strSql = funcGenPurItemTagStrSql(TagSno, itemId, TagNo, purFineRate, 1, COSTID, purFineRate)
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                    UpdateTagNo(TagNo, COSTID)
                    If cnt <> pcs Then
                        TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") ' GetTagSno()
                        TagNo = objTag.GetTagNo(GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd"), txtItemName__Man.Text, Sno, tran, itemId, pieceRate)
                        tagVal = objTag.GetTagVal(TagNo, tran)
                        'TagNo = GetTagNo()
                        'tagVal = GetTagVal(TagNo)
                    End If
                    funcMiscDetailsAdd(itemId, TagNo, COSTID, TagSno)
                Next
            End If


            'CPIECES AND CWT
            Dim mcwt As Decimal = pcswt * pcs
            strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CPCS = isnull(CPCS,0) + " & pcs & ",CGRSWT =  isnull(CGRSWT,0) + " & mcwt & ",CNETWT =  isnull(CNETWT,0) + " & mcwt & ""
            strSql += " WHERE SNO = '" & Sno & "'"
            'strSql += " AND ENTRYORDER = '" & EntryOrder & "'"
            ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)

            tran.Commit()
            tran = Nothing
            MsgBox(E0008, MsgBoxStyle.Exclamation)
            ''Last Tag and Wt
            'Dim lotPcs As Integer = lblPLot.Text
            'Dim cPcs As Integer = pcs
            funcNext()
            ''Lot Pcs
            'lblPLot.Text = lotPcs
            'lblPCompled.Text = Val(lblPCompled.Text) + cPcs
            'lblPBalance.Text = IIf(Val(lblPLot.Text) - Val(lblPCompled.Text) <> 0, _
            'Val(lblPLot.Text) - Val(lblPCompled.Text), "")

            If CallBarcodeExe = False Then
                For Each ro As DataRow In dtTagPrint.Rows

                    Dim objBar As New clsBarcodePrint
                    strSql = "SELECT (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)METALID FROM"
                    strSql += vbCrLf + " " & cnAdminDb & "..ITEMTAG AS T WHERE ITEMID = '" & ro!ITEMID & "' AND TAGNO = '" & ro!TAGNO & "'"
                    MetalId = objGPack.GetSqlValue(strSql, "", "")
                    If MetalId = "G" Then
                        objBar.FuncprintBarcode_Single(ro!ITEMID.ToString, ro!TAGNO.ToString)
                    Else
                        If Val(GetAdmindbSoftValue("BARCODE_FORMAT", "1")) <= "1" Then
                            objBar.FuncprintBarcode_Single(ro!ITEMID.ToString, ro!TAGNO.ToString)
                        Else
                            objBar.FuncprintBarcode_Multi(ro!ITEMID.ToString, ro!TAGNO.ToString, Val(GetAdmindbSoftValue("BARCODE_FORMAT", "1")))
                        End If
                    End If
                Next
            Else
                lblLastTagNo.Text = TagNo
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim oldItem As Integer = Nothing
                Dim memfile As String = "\Barcodeprint" & prnmemsuffix.Trim & ".mem"
                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & memfile)
                For Each ro As DataRow In dtTagPrint.Rows
                    If oldItem <> Val(ro!itemid.ToString) Then
                        write.WriteLine(LSet("PROC", 7) & ":" & ro!ITEMID.ToString)
                        oldItem = Val(ro!itemid.ToString)
                    End If
                    write.WriteLine(LSet("TAGNO", 7) & ":" & ro!TAGNO.ToString)
                Next
                write.Flush()
                write.Close()
                If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe")
                Else
                    MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                End If
            End If

        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Function

    Private Function GetTagVal(ByVal tagNo As String) As String
        Dim TagVal As Integer = Nothing
        ''Find TagVal
        If IsNumeric(tagNo) = True Then
            TagVal = Val(tagNo)
        Else
            Dim index As Integer = 0
            For Each c As Char In tagNo
                If Char.IsLetter(c) = True Then
                    index += 1
                Else
                    Exit For
                End If
            Next
            Dim fNo As String
            Dim sNo As String
            fNo = tagNo.Substring(0, index)
            sNo = tagNo.Substring(index, tagNo.Length - index)
            TagVal = (AscW(fNo) * 1000) + Val(sNo)
        End If
        Return TagVal.ToString
    End Function

    Function funcGetTagNo() As String
        ''Find LastTagNo
        If TagNoGen = "I" Then
            strSql = " SELECT CTLTEXT AS TAGNOFROM FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGNOFROM'"
            TagNoFrom = objGPack.GetSqlValue(strSql, , , tran)
            If TagNoFrom = "I" Then
                strSql = " SELECT CURRENTTAGNO FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = "
                strSql += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
                LastTagNo = Val(objGPack.GetSqlValue(strSql, , , tran)).ToString
            Else
                strSql = " Select ctlText as LastTagNo from " & cnAdminDb & "..SoftControl where ctlId = 'LastTagNo'"
                LastTagNo = objGPack.GetSqlValue(strSql, , , tran).ToString
            End If
        Else ''L--Lot Item Entry
            strSql = " SELECT CURTAGNO FROM " & cnAdminDb & "..ITEMLOT WHERE ITEMID = "
            strSql += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
            strSql += " AND LOTNO = '" & txtLotNo_Num_Man.Text & "' AND ENTRYORDER = '" & EntryOrder & "'"
            LastTagNo = objGPack.GetSqlValue(strSql, , , tran)
        End If
        Return funcGenTagNo(LastTagNo)
    End Function
    Function funcGenTagNo(ByVal lstTagNo As String) As String
        'If lstTagNo = "0" Then
        '    Return "0"
        'End If
        If IsNumeric(lstTagNo) = True Then
            lstTagNo = Val(lstTagNo) + 1
        Else
            Dim index As Integer = 0
            For Each c As Char In lstTagNo
                If Char.IsLetter(c) = True Then
                    index += 1
                Else
                    Exit For
                End If
            Next
            Dim fNo As String
            Dim sNo As String
            fNo = lstTagNo.Substring(0, index)
            sNo = lstTagNo.Substring(index, lstTagNo.Length - index)
            sNo = Val(sNo) + 1
            lstTagNo = fNo + sNo
        End If
        Return lstTagNo
    End Function
    Private Sub funcGetLotValues()
        strSql = " SELECT"
        strSql += " IT.LOTNO,IT.LOTDATE,IT.ENTRYORDER,IT.MULTIPLETAGS,IT.RATE,"
        strSql += " IT.PCS,IT.CPCS,(IT.PCS - IT.CPCS)BALPCS,IT.GRSWT,IT.CGRSWT,(IT.GRSWT- IT.CGRSWT)BALWT,"
        strSql += " IT.ITEMID,item.itemname ITEMNAME,ITEM.CALTYPE ,sitem.subitemname  SUBITEMNAME,"
        strSql += "  cost.costname,ctr.itemctrname ,ISNULL(DESIGNERNAME,'') DESIGNERNAME, "
        strSql += " IT.NARRATION,IT.STYLENO,IT.FINERATE,IT.TUCH,IT.ITEMID,IT.SUBITEMID,WMCTYPE,IT.TABLECODE "
        strSql += " FROM " & cnAdminDb & "..ITEMLOT IT"
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST ITEM ON ITEM.ITEMID = IT.ITEMID "
        strSql += " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SITEM ON SITEM.ITEMID = IT.ITEMID AND SITEM.SUBITEMID = IT.SUBITEMID"
        strSql += " LEFT JOIN " & cnAdminDb & "..COSTCENTRE COST ON COST.COSTID = IT.COSTID "
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID = IT.ITEMCTRID"
        strSql += " LEFT JOIN " & cnAdminDb & "..DESIGNER DESG ON DESG.DESIGNERID = IT.DESIGNERID "
        strSql += " WHERE IT.SNO = '" & Sno & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Exit Sub
        End If
        With dt.Rows(0)
            txtLotNo_Num_Man.Text = .Item("LOTNO").ToString
            dtpRecieptDate.Value = GetEntryDate(.Item("LOTDATE"))
            'dtpRecieptDate.Value = .Item("LOTDATE").ToString

            EntryOrder = Val(.Item("ENTRYORDER").ToString)
            multipleTagFlag = .Item("MULTIPLETAGS").ToString

            pieceRate = Val(.Item("RATE").ToString)
            pcs = Val(.Item("PCS").ToString)
            mItemid = Val(.Item("ITEMID").ToString)
            txtItemName__Man.Text = .Item("ITEMNAME").ToString
            If cmbCostCentre.Enabled = True Then
                cmbCostCentre.Text = .Item("COSTNAME").ToString
            End If
            cmbCounter.Text = .Item("ITEMCTRNAME").ToString
            If cmbCounter.Items.Count > 0 Then cmbCounter.Enabled = True Else cmbCounter.Enabled = False
            cmbDesigner.Text = .Item("DESIGNERNAME").ToString
            If cmbDesigner.Items.Count > 0 Then cmbDesigner.Enabled = True Else cmbDesigner.Enabled = False
            cmbItemSize.Items.Clear()
            If txtItemName__Man.Text <> "" Then
                strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE"
                strSql += " WHERE ITEMID =(SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
                strSql += " ORDER BY SIZENAME"
                objGPack.FillCombo(strSql, cmbItemSize)
            End If
            If .Item("Tablecode").ToString <> "" Then cmbTableCode.Text = .Item("Tablecode").ToString
            If cmbItemSize.Items.Count > 0 Then cmbItemSize.Enabled = True Else cmbItemSize.Enabled = False
            narration = .Item("NARRATION").ToString
            StyleNo = .Item("STYLENO").ToString
            purFineRate = Val(.Item("FINERATE").ToString)
            purTouch = Val(.Item("TUCH").ToString)
            txtPurPieRate_NUM.Text = Val(.Item("FINERATE").ToString)
            cmbSubItem_MAN.Items.Clear()
            cmbSubItem_MAN.Text = ""
            cmbSubItem_MAN.Enabled = False
            If .Item("SUBITEMNAME").ToString <> "" Then
                cmbSubItem_MAN.Enabled = True
                cmbSubItem_MAN.Items.Add(.Item("SUBITEMNAME").ToString)
                cmbSubItem_MAN.Text = .Item("SUBITEMNAME").ToString
            Else
                'pieceRate = 0
                'purFineRate = 0
                strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & Val(.Item("ITEMID").ToString) & ""
                objGPack.FillCombo(strSql, cmbSubItem_MAN)
                If cmbSubItem_MAN.Items.Count > 0 Then cmbSubItem_MAN.Enabled = True
            End If
            mwccaltype = .Item("WMCTYPE").ToString
            If .Item("WMCTYPE") = "T" Then
                cmbTableCode.Enabled = True
                strSql = " SELECT DISTINCT TABLECODE FROM " & cnAdminDb & "..WMCTABLE WHERE TABLECODE <> '' "
                strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'),'')"
                strSql += " ORDER BY TABLECODE"
                objGPack.FillCombo(strSql, cmbTableCode)

            Else
                cmbTableCode.Items.Clear()
                cmbTableCode.Enabled = False
            End If
            lblPLot.Text = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, "")
            lblPCompled.Text = IIf(Val(.Item("CPCS").ToString) <> 0, .Item("CPCS").ToString, 0) ' "" ->0
            lblPBalance.Text = IIf(Val(.Item("BALPCS").ToString) <> 0, .Item("BALPCS").ToString, "")
            lblLotwt.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, .Item("GRSWT").ToString, "")
            lblComwt.Text = IIf(Val(.Item("CGRSWT").ToString) <> 0, .Item("CGRSWT").ToString, "")
            lblBalwt.Text = IIf(Val(.Item("BALWT").ToString) <> 0, .Item("BALWT").ToString, "")

            lblRate.Text = pieceRate
            txtRateid_NUM.Text = pieceRate
            txtNoofPcs.Text = IIf(Val(.Item("BALPCS").ToString) <> 0, .Item("BALPCS").ToString, "")
            WT = Val(.Item("BALWT").ToString)
            'If .Item("CALTYPE") <> "R" Then grpWastmc.Visible = True Else grpWastmc.Visible = False
            txtBoardGoldRate.Text = GetMetalRate()
            lblHint.Text = ""
            If .Item("CALTYPE") = "R" Then
                grpPieces.Enabled = True
                grpWastmc.Enabled = False
                lblHint.Text = "..."
                rbtRateFixed.Checked = True
            ElseIf .Item("CALTYPE") = "W" Then
                grpPieces.Enabled = False
                grpWastmc.Enabled = True
                lblHint.Text = "..."
                rbtWeightFixed.Checked = True
            ElseIf .Item("CALTYPE") = "B" Then
                grpPieces.Enabled = True
                grpWastmc.Enabled = True
                lblHint.Text = "..."
            End If
        End With
    End Sub

    Private Sub frmitemBulkTag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtLotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLotNo_Num_Man.KeyDown
        Sno = Nothing
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT SNO"
            strSql += vbCrLf + " ,LOTNO,CONVERT(VARCHAR(12),LOTDATE,103)AS LOTDATE"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID = LOT.ITEMID)AS ITEMNAME"
            strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID = LOT.SUBITEMID),'')AS SUBITEMNAME"
            strSql += vbCrLf + " ,(SELECT (CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END) FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS STOCKTYPE"
            strSql += vbCrLf + " ,PCS,CPCS AS COMPLETEDPCS,(PCS-CPCS)AS BALPCS"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMLOT AS LOT"
            strSql += vbCrLf + " WHERE "
            If tabCheckBy = "P" Then
                strSql += vbCrLf + " LOT.PCS > LOT.CPCS "
            ElseIf tabCheckBy = "E" Then
                strSql += vbCrLf + " ((LOT.GRSWT > LOT.CGRSWT) OR ( LOT.PCS > LOT.CPCS))"
            Else
                strSql += vbCrLf + " ((LOT.GRSWT > LOT.CGRSWT) or (LOT.rate <> 0 and LOT.pcs > LOT.cpcs))"
            End If
            strSql += vbCrLf + " AND ISNULL(COMPLETED,'') <> 'Y'"
            strSql += vbCrLf + " AND BULKLOT = 'Y'"
            strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
            strSql += vbCrLf + " AND (PCS-CPCS) > 0 "
            strSql += vbCrLf + " ORDER BY ITEMNAME "
            Sno = BrighttechPack.SearchDialog.Show("Finding LotNo", strSql, cn, 1)
            funcGetLotValues()
            If Sno <> "" Then
                txtLotNo_Num_Man.Enabled = False
                Me.SelectNextControl(txtLotNo_Num_Man, True, True, True, True)
            End If
        End If
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub
    Private Sub txtLotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtLotNo_Num_Man.KeyPress
        Sno = Nothing
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtLotNo_Num_Man.Text = "" Then Exit Sub
            strSql = " SELECT SNO"
            strSql += " ,LOTNO,CONVERT(VARCHAR(12),LOTDATE,103)AS LOTDATE"
            strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID = LOT.ITEMID)AS ITEMNAME"
            strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID = LOT.SUBITEMID),'')AS SUBITEMNAME"
            strSql += " ,(SELECT (CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END) FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS STOCKTYPE"
            strSql += " ,PCS,CPCS AS COMPLETEDPCS,(PCS-CPCS)AS BALPCS"
            strSql += " FROM " & cnAdminDb & "..ITEMLOT AS LOT"
            strSql += " WHERE "
            strSql += " LOTNO = " & Val(txtLotNo_Num_Man.Text) & ""
            If tabCheckBy = "P" Then
                strSql += " AND LOT.PCS > LOT.CPCS "
            ElseIf tabCheckBy = "E" Then
                strSql += vbCrLf + " AND ((LOT.GRSWT > LOT.CGRSWT) OR ( LOT.PCS > LOT.CPCS))"
            Else
                strSql += vbCrLf + " AND ((LOT.GRSWT > LOT.CGRSWT) or (LOT.rate <> 0 and LOT.pcs > LOT.cpcs))"
            End If
            strSql += " AND ISNULL(COMPLETED,'') <> 'Y'"
            strSql += " AND BULKLOT = 'Y'"
            strSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
            strSql += " ORDER BY ITEMNAME "
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows.Count = 1 Then
                    Sno = dt.Rows(0).Item("SNO").ToString
                    funcGetLotValues()
                Else 'MORE THAN ONE 
                    Sno = BrighttechPack.SearchDialog.Show("Finding LotNo", strSql, cn, 1)
                    funcGetLotValues()
                End If
                If Sno <> "" Then
                    txtLotNo_Num_Man.Enabled = False
                    Me.SelectNextControl(txtLotNo_Num_Man, True, True, True, True)
                Else
                    MsgBox(E0004 + Me.GetNextControl(txtLotNo_Num_Man, False).Text, MsgBoxStyle.Information)
                End If
            Else
                MsgBox(E0004 + Me.GetNextControl(txtLotNo_Num_Man, False).Text, MsgBoxStyle.Information)
            End If
        End If
    End Sub
    Private Sub CalcMaxMinValues(ByVal calcwt As Decimal)
        strSql = Nothing
        Dim itemid As Integer
        Dim tablecode As String
        If cmbTableCode.Text = "" Then
            Dim dritem As DataRow = GetSqlRow("SELECT ITEMID,TABLECODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "'", cn)
            itemid = dritem(0)
            tablecode = dritem(1)
            Dim drlot As DataRow = GetSqlRow(" SELECT WMCTYPE,TABLECODE FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & Sno & "'", cn)
            If drlot(1).ToString <> "" Then tablecode = drlot(1)
            mwccaltype = drlot(0).ToString
            If drlot(0) = "" Then Exit Sub
        Else
            itemid = mItemid
            tablecode = cmbTableCode.Text
        End If
        Select Case mwccaltype
            Case "T"
                strSql = " DECLARE @WT FLOAT"
                strSql += vbCrLf + " SET @WT = " & calcwt & ""
                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE TABLECODE = '" & tablecode & "'"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'),'')"
            Case "I"
                strSql = " DECLARE @WT FLOAT"
                strSql += vbCrLf + " SET @WT = " & calcwt & ""
                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE ITEMID = " & itemid
                strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "' AND ITEMID = " & itemid & "),0)"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'),'')"
            Case "D"
                strSql = " DECLARE @WT FLOAT"
                strSql += vbCrLf + " SET @WT = " & calcwt & ""
                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE ITEMID = " & itemid
                strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "' AND ITEMID = " & itemid & "),0)"
                strSql += vbCrLf + " AND DESIGNERID = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner.Text & "'),0)"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'),'')"
            Case "P"
                strSql = " DECLARE @WT FLOAT"
                strSql += vbCrLf + " SET @WT = " & calcwt & ""
                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE ITEMTYPE = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType.Text & "')"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'),'')"
        End Select
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                Dim wmcWastPer As Double = Val(.Item("MAXWASTPER").ToString)
                Dim wmcWast As Double = Val(.Item("MAXWAST").ToString)
                Dim wmcMcGrm As Double = Val(.Item("MAXMCGRM").ToString)
                Dim wmcMc As Double = Val(.Item("MAXMC").ToString)
                txtMaxwastper_Per.Text = IIf(Val(.Item("MAXWASTPER").ToString) <> 0, Format(Val(.Item("MAXWASTPER").ToString), "0.00"), "")
                txtMaxWast_wet.Text = IIf(wmcWast <> 0, Format(wmcWast, "0.00"), "")
                txtMaxMcGrm_Amt.Text = IIf(Val(.Item("MAXMCGRM").ToString) <> 0, Format(Val(.Item("MAXMCGRM").ToString), "0.00"), "")
                txtMaxMc_amt.Text = IIf(wmcMc <> 0, Format(wmcMc, "0.00"), "")
                txtMinwastper_Per.Text = Val("" & .Item("MINWASTPER").ToString)
                txtMinWast_wet.Text = Val("" & .Item("MINWAST").ToString)
                txtMinMcGrm_amt.Text = Val("" & .Item("MINMC").ToString)
            End With
        End If

    End Sub

    Private Sub txtPcsWt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then totwt_WET.Text = Val(txtNoofPcs.Text) * Val(txtWt_WET.Text) : CalcMaxMinValues(Val(txtWt_WET.Text))
    End Sub

    Private Sub txtPcsWt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWt_WET.TextChanged
        If Val(txtWt_WET.Text) = 0 Then Exit Sub
        If Val(txtWt_WET.Text) <> 0 Then totwt_WET.Text = Val(txtNoofPcs.Text) * Val(txtWt_WET.Text)
        If Val(totwt_WET.Text) > Val(lblBalwt.Text) Then
            If MsgBox("Total Weight Exceed" & vbCrLf & "Continue?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then txtWt_WET.Text = "" : totwt_WET.Text = "" : Exit Sub
        End If
        SaleValue(Val(txtWt_WET.Text), Val(txtMaxWast_wet.Text), Val(txtMaxMc_amt.Text), Val(txtBoardGoldRate.Text), Val(txtTouch_amt.Text))
    End Sub

    Private Sub cmbSubItem_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubItem_MAN.LostFocus
        If cmbSubItem_MAN.Text <> "" Then
            txtWt_WET.Text = objGPack.GetSqlValue("SELECT ISNULL(GRSWT,0) GRSWT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "' AND ITEMID = " & mItemid & "")
            Dim mtablecode As String = objGPack.GetSqlValue("SELECT ISNULL(TABLECODE,'') TABLECODE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "' AND ITEMID = " & mItemid & "")
            If mtablecode <> "" Then cmbTableCode.Text = mtablecode
        End If
    End Sub

    Private Sub txtPcs_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNoofPcs.TextChanged
        If Val(txtNoofPcs.Text) = 0 Then Exit Sub
        If Val(txtNoofPcs.Text) <> 0 Then totwt_WET.Text = Val(txtNoofPcs.Text) * Val(txtWt_WET.Text)
        If Val(totwt_WET.Text) > Val(lblBalwt.Text) Then
            If MsgBox("Total Weight Exceed" & vbCrLf & "Continue?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then txtNoofPcs.Text = "" : totwt_WET.Text = "" : Exit Sub
        End If
    End Sub



    Private Sub txtMaxwastper_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMaxwastper_Per.TextChanged
        If Val(txtRateid_NUM.Text) > 0 Then
            MaxWastagePerSale(1, Val(txtRateid_NUM.Text))
        ElseIf Val(txtWt_WET.Text) > 0 Then
            MaxWastagePerSale(Val(txtWt_WET.Text), Val(txtBoardGoldRate.Text))
        End If
    End Sub

    Private Sub txtMinwastper_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMinwastper_Per.TextChanged
        If Val(txtRateid_NUM.Text) > 0 Then
            MinWastagePersale(1)
        ElseIf Val(txtWt_WET.Text) > 0 Then
            MinWastagePersale(Val(txtWt_WET.Text))
        End If
    End Sub

    Private Sub txtMaxMcGrm_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMaxMcGrm_Amt.TextChanged
        If Val(txtRateid_NUM.Text) > 0 Then
            MaxMcPersales(1, Val(txtRateid_NUM.Text))
        ElseIf Val(txtWt_WET.Text) > 0 Then
            MaxMcPersales(Val(txtWt_WET.Text), Val(txtBoardGoldRate.Text))
        End If
    End Sub

    Private Sub txtMinMcGrm_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMinMcGrm_amt.TextChanged
        If Val(txtRateid_NUM.Text) > 0 Then
            MinMcpersales(Val(txtRateid_NUM.Text))
        ElseIf Val(txtWt_WET.Text) > 0 Then
            MinMcpersales(Val(txtWt_WET.Text))
        End If
    End Sub

    Private Function MaxWastagePerSale(ByVal Value As Double, ByVal boardrate As Double) As Double
        txtMaxWast_wet.Text = Value * (Val(txtMaxwastper_Per.Text) / 100)
        SaleValue(Val(txtWtPur_WET.Text), Val(txtMaxWast_wet.Text), Val(txtMaxMc_amt.Text), boardrate, Val(txtTouch_amt.Text))
    End Function

    Private Function MinWastagePersale(ByVal value As Double) As Double
        txtMinWast_wet.Text = value * (Val(txtMinwastper_Per.Text) / 100)
    End Function

    Private Function MaxMcPersales(ByVal value As Double, ByVal boardrate As Double) As Double
        txtMaxMc_amt.Text = value * Val(txtMaxMcGrm_Amt.Text)
        SaleValue(Val(txtWtPur_WET.Text), Val(txtMaxWast_wet.Text), Val(txtMaxMc_amt.Text), Val(txtBoardGoldRate.Text), Val(txtTouch_amt.Text))
    End Function

    Private Function MinMcpersales(ByVal value As Double) As Double
        txtMinMc_amt.Text = value * Val(txtMinMcGrm_amt.Text)
    End Function

    Private Function SaleValue(ByVal pcsWeight As Double, ByVal wastagegms As Double, ByVal Mcrs As Double, ByVal _rate As Double, ByVal _touch As Double) As Double
        Dim SaleAmt As Double = (pcsWeight + wastagegms) * _rate
        Dim TouchAmt As Double = (pcsWeight * (_touch / 100)) * _rate
        If TouchAmt > 0 Then
            SaleAmt = TouchAmt
        End If
        SaleAmt = SaleAmt + Mcrs + IIf(Val(ObjMiscDetails.txtMiscTotAmt.Text) > 0, Val(ObjMiscDetails.txtMiscTotAmt.Text), 0)
        txtSaleValue_AMT.Text = Format(Math.Round(SaleAmt, SALVALUEROUND), "0.00")
    End Function

    Private Sub txtTouch_amt_TextChanged(sender As Object, e As EventArgs) Handles txtTouch_amt.TextChanged
        If Val(txtRateid_NUM.Text) > 0 Then
        ElseIf Val(txtWt_WET.Text) > 0 Then
            SaleValue(Val(txtWt_WET.Text), Val(txtMaxWast_wet.Text), Val(txtMaxMc_amt.Text), Val(txtBoardGoldRate.Text), Val(txtTouch_amt.Text))
        End If
    End Sub

    Private Sub txtMaxWast_TextChanged(sender As Object, e As EventArgs) Handles txtMaxWast_wet.TextChanged
        If Val(txtMaxwastper_Per.Text) = 0 Then
            If Val(txtRateid_NUM.Text) > 0 Then
            ElseIf Val(txtWt_WET.Text) > 0 Then
                SaleValue(Val(txtWt_WET.Text), Val(txtMaxWast_wet.Text), Val(txtMaxMc_amt.Text), Val(txtBoardGoldRate.Text), Val(txtTouch_amt.Text))
            End If
        End If
    End Sub
    Private Sub txtMaxMc_TextChanged(sender As Object, e As EventArgs) Handles txtMaxMc_amt.TextChanged
        If Val(txtMaxMcGrm_Amt.Text) = 0 Then
            If Val(txtRateid_NUM.Text) > 0 Then
            ElseIf Val(txtWt_WET.Text) > 0 Then
                SaleValue(Val(txtWt_WET.Text), Val(txtMaxWast_wet.Text), Val(txtMaxMc_amt.Text), Val(txtBoardGoldRate.Text), Val(txtTouch_amt.Text))
            End If
        End If
    End Sub

    Private Sub txtMaxWast_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMaxWast_wet.KeyPress
        If Val(txtMaxwastper_Per.Text) > 0 Then
            txtMaxWast_wet.ReadOnly = True
        Else
            txtMaxWast_wet.ReadOnly = False
        End If
    End Sub

    Private Sub txtMinWast_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMinWast_wet.KeyPress
        If Val(txtMinwastper_Per.Text) > 0 Then
            txtMinWast_wet.ReadOnly = True
        Else
            txtMinWast_wet.ReadOnly = False
        End If
    End Sub

    Private Sub txtMaxMc_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMaxMc_amt.KeyPress
        If Val(txtMaxMcGrm_Amt.Text) > 0 Then
            txtMaxMc_amt.ReadOnly = True
        Else
            txtMaxMc_amt.ReadOnly = False
        End If
    End Sub

    Private Sub txtMinMc_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMinMc_amt.KeyPress
        If Val(txtMinMcGrm_amt.Text) > 0 Then
            txtMinMc_amt.ReadOnly = True
        Else
            txtMinMc_amt.ReadOnly = False
        End If
    End Sub

    Private Sub txtPurWastageGram_WET_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPurWastageGram_wet.KeyPress
        If Val(txtPurWasterPer_Per.Text) > 0 Then
            txtPurWastageGram_wet.ReadOnly = True
        Else
            txtPurWastageGram_wet.ReadOnly = False
        End If
    End Sub

    Private Sub txtpurMCAmt_AMT_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtpurMCAmt_AMT.KeyPress
        If Val(txtPurMCGM_AMT.Text) > 0 Then
            txtpurMCAmt_AMT.ReadOnly = True
        Else
            txtpurMCAmt_AMT.ReadOnly = False
        End If
    End Sub


    Private Function PurchaseValue(ByVal pcsWeight As Double, ByVal wastagegms As Double, ByVal Mcrs As Double, ByVal _rate As Double, ByVal _touch As Double) As Double
        Dim PurAmt As Double = (pcsWeight + wastagegms) * _rate
        Dim TouchAmt As Double = (pcsWeight * (_touch / 100)) * _rate
        If TouchAmt > 0 Then
            PurAmt = TouchAmt
        End If
        PurAmt = PurAmt + Mcrs + IIf(Val(ObjMiscDetails.txtMiscTotAmt.Text) > 0, Val(ObjMiscDetails.txtMiscTotAmt.Text), 0)
        txtPURAmt_AMT.Text = Format(Math.Round(PurAmt, SALVALUEROUND), "0.00")
    End Function

    Private Sub txtPurTouch_amt_TextChanged(sender As Object, e As EventArgs) Handles txtPurTouch_amt.TextChanged
        If Val(txtRateid_NUM.Text) > 0 Then
        ElseIf Val(txtWt_WET.Text) > 0 Then
            PurchaseValue(Val(txtWtPur_WET.Text), Val(txtPurWastageGram_wet.Text), Val(txtpurMCAmt_AMT.Text), Val(txtBoardGoldRate.Text), Val(txtPurTouch_amt.Text))
        End If
    End Sub

    Private Sub txtPurWasterPer_AMT_TextChanged(sender As Object, e As EventArgs) Handles txtPurWasterPer_Per.TextChanged
        If Val(txtRateid_NUM.Text) > 0 Then
        ElseIf Val(txtWt_WET.Text) > 0 Then
            MaxWastagePertogramPurchase(Val(txtWtPur_WET.Text), Val(txtBoardGoldRate.Text))
        End If
    End Sub
    Private Sub txtPurWastageGram_WET_TextChanged(sender As Object, e As EventArgs) Handles txtPurWastageGram_wet.TextChanged
        If Val(txtRateid_NUM.Text) > 0 Then
        ElseIf Val(txtWt_WET.Text) > 0 Then
            PurchaseValue(Val(txtWtPur_WET.Text), Val(txtPurWastageGram_wet.Text), Val(txtpurMCAmt_AMT.Text), Val(txtBoardGoldRate.Text), Val(txtPurTouch_amt.Text))
        End If
    End Sub

    Private Sub txtPurMCGM_AMT_TextChanged(sender As Object, e As EventArgs) Handles txtPurMCGM_AMT.TextChanged
        If Val(txtRateid_NUM.Text) > 0 Then
        ElseIf Val(txtWt_WET.Text) > 0 Then
            MaxMcPertorupeesPurchase(Val(txtWtPur_WET.Text))
            PurchaseValue(Val(txtWtPur_WET.Text), Val(txtPurWastageGram_wet.Text), Val(txtpurMCAmt_AMT.Text), Val(txtBoardGoldRate.Text), Val(txtPurTouch_amt.Text))
        End If
    End Sub

    Private Sub txtpurMCAmt_AMT_TextChanged(sender As Object, e As EventArgs) Handles txtpurMCAmt_AMT.TextChanged
        If Val(txtRateid_NUM.Text) > 0 Then
        ElseIf Val(txtWt_WET.Text) > 0 Then
            PurchaseValue(Val(txtWtPur_WET.Text), Val(txtPurWastageGram_wet.Text), Val(txtpurMCAmt_AMT.Text), Val(txtBoardGoldRate.Text), Val(txtPurTouch_amt.Text))
        End If
    End Sub


    Private Function MaxWastagePertogramPurchase(ByVal Value As Double, ByVal boardrate As Double) As Double
        txtPurWastageGram_wet.Text = Value * (Val(txtPurWasterPer_Per.Text) / 100)
        PurchaseValue(Val(txtWtPur_WET.Text), Val(txtPurWastageGram_wet.Text), Val(txtpurMCAmt_AMT.Text), boardrate, Val(txtPurTouch_amt.Text))
    End Function
    Private Function MaxMcPertorupeesPurchase(ByVal value As Double) As Double
        txtpurMCAmt_AMT.Text = value * Val(txtPurMCGM_AMT.Text)
        PurchaseValue(Val(txtWtPur_WET.Text), Val(txtMaxWast_wet.Text), Val(txtpurMCAmt_AMT.Text), Val(txtBoardGoldRate.Text), Val(txtPurTouch_amt.Text))
    End Function

    Private Sub txtWtPur_WET_TextChanged(sender As Object, e As EventArgs) Handles txtWtPur_WET.TextChanged
        PurchaseValue(Val(txtWtPur_WET.Text), Val(txtPurWastageGram_wet.Text), Val(txtpurMCAmt_AMT.Text), Val(txtBoardGoldRate.Text), Val(txtPurTouch_amt.Text))
    End Sub

    Private Sub txtMinwastper_KeyDown(sender As Object, e As KeyEventArgs) Handles txtMinwastper_Per.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtMaxwastper_Per.Text) > 0 Then
                If Val(txtMaxwastper_Per.Text) < Val(txtMinwastper_Per.Text) Then
                    MsgBox("Max wastage should not allowed min", MsgBoxStyle.Information)
                    txtMinwastper_Per.Text = ""
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub txtMinWast_KeyDown(sender As Object, e As KeyEventArgs) Handles txtMinWast_wet.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtMaxWast_wet.Text) > 0 Then
                If Val(txtMaxWast_wet.Text) < Val(txtMinWast_wet.Text) Then
                    MsgBox("Max should not allowed min", MsgBoxStyle.Information)
                    txtMinWast_wet.Text = ""
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub txtMinMcGrm_KeyDown(sender As Object, e As KeyEventArgs) Handles txtMinMcGrm_amt.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtMaxMcGrm_Amt.Text) > 0 Then
                If Val(txtMaxMcGrm_Amt.Text) < Val(txtMinMcGrm_amt.Text) Then
                    MsgBox("Max should not allowed min", MsgBoxStyle.Information)
                    txtMinMcGrm_amt.Text = ""
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub txtMinMc_KeyDown(sender As Object, e As KeyEventArgs) Handles txtMinMc_amt.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtMaxMc_amt.Text) > 0 Then
                If Val(txtMaxMc_amt.Text) < Val(txtMinMc_amt.Text) Then
                    MsgBox("Max should not allowed min", MsgBoxStyle.Information)
                    txtMinMc_amt.Text = ""
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub txtRateid_NUM_TextChanged(sender As Object, e As EventArgs) Handles txtRateid_NUM.TextChanged
        SaleValue(1, Val(txtMaxWast_wet.Text), Val(txtMaxMc_amt.Text), Val(txtRateid_NUM.Text), Val(txtTouch_amt.Text))
    End Sub

    Private Sub txtPurPieRate_NUM_TextChanged(sender As Object, e As EventArgs) Handles txtPurPieRate_NUM.TextChanged
        PurchaseValue(1, Val(txtPurWastageGram_wet.Text), Val(txtpurMCAmt_AMT.Text), Val(txtPurPieRate_NUM.Text), Val(txtPurTouch_amt.Text))
    End Sub
    Private Sub OtherChargesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OtherChargesToolStripMenuItem.Click
        Dim OtherCharges As String = IIf(GetSqlValue(cn, "SELECT OTHCHARGE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(OTHCHARGE ,'N') ='Y'") = "Y", True, False)
        If OtherCharges = True Then
            ObjMiscDetails.ShowDialog()
        End If
    End Sub
    Private Sub txtNoofPcs_Leave(sender As Object, e As EventArgs) Handles txtNoofPcs.Leave
        Dim OtherCharges As String = IIf(GetSqlValue(cn, "SELECT OTHCHARGE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(OTHCHARGE ,'N') ='Y'") = "Y", True, False)
        If OtherCharges = True Then
            ObjMiscDetails.ShowDialog()
            SaleValue(1, Val(txtMaxWast_wet.Text), Val(txtMaxMc_amt.Text), Val(txtRateid_NUM.Text), Val(txtTouch_amt.Text))
            PurchaseValue(1, Val(txtPurWastageGram_wet.Text), Val(txtpurMCAmt_AMT.Text), Val(txtPurPieRate_NUM.Text), Val(txtPurTouch_amt.Text))
        End If
    End Sub


End Class

