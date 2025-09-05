Imports System.Data.OleDb
Public Class TagwisePurchaseUpdate
    Dim dtTagDet As New DataTable
    Dim dtTagDetails As DataTable
    Dim strSql As String
    Dim cmd As OleDbCommand

    Dim SNO As String
    Dim ObjPurDetail As New TagPurchaseDetailEntry(True)
    Dim ObjOrderTagInfo As New TagOrderInfo
    Dim objTag As New TagGeneration

    Dim Pieces_Num_Man As String
    Dim CalcMode As String
    Dim GrossWt_Wet As String
    Dim LessWt_Wet As String
    Dim NetWt_Wet As String
    Dim RecieptDate As DateTime
    Dim Rate_Amt As String
    Dim MetalRate_Amt As String
    Dim purRate As String
    Dim PurchaseValue_Amt As String
    Dim Item_MAN As String
    Dim SubItem_Man As String
    Dim updIssSno As String
    Dim TagNo__Man As String
    Dim ItemType_MAN As String
    Dim CostCentre_Man As String
    Dim lotPcs As Integer
    Dim lotGrsWt As Double
    Dim lotNetWt As Double

    Dim OrderRow As DataRow = Nothing

    Dim Designer_MAN As String

    Dim dtStoneDetails As New DataTable("GridStone")
    Dim dtMultiMetalDetails As New DataTable("GridMultiMetal")
    Dim dtMiscDetails As New DataTable("GridMisc")

    Dim flagPurValSet As Boolean = False
    Dim tagEdit As Boolean

    Dim FixedVa As Boolean = False
    Dim calType As String

    Dim _HasPurchase As Boolean = IIf(GetAdmindbSoftValue("PURTAB", "N") = "N", False, True)
    Dim PUR_AUTOCALC As Boolean = IIf(GetAdmindbSoftValue("PUR_AUTOCALC", "N") = "N", False, True)

    Private Property pFixedVa() As Boolean
        Get
            Return FixedVa
        End Get
        Set(ByVal value As Boolean)
            FixedVa = value
        End Set
    End Property

    Private Sub fNew()
        txtTagNo.Clear()
        txtItemId.Clear()
        dtTagDet.Rows.Clear()
        dtTagDet.AcceptChanges()
        txtItemId.Focus()
    End Sub

    Private Sub txtTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTagNo.Text = "" Then
                MsgBox("TagNo should not empty", MsgBoxStyle.Information)
                txtTagNo.Focus()
                Exit Sub
            End If
            Dim itemId As String = ""
            'If txtItemId.Text = "" Then
            '    MsgBox("ItemId should not empty", MsgBoxStyle.Information)
            '    txtItemId.Focus()
            '    Exit Sub
            'End If
            Dim spChar As String = GetAdmindbSoftValue("PRODTAGSEP", "")
            If spChar <> "" Then
                If txtTagNo.Text.Contains(spChar) Then
                    Dim sp() As String = txtTagNo.Text.Split(spChar)
                    itemId = Trim(sp(0))
                    If sp.Length >= 2 Then
                        txtTagNo.Text = Trim(sp(1))
                    End If
                End If
            End If
TagReCheck:
            dtTagDet.Rows.Clear()
            strSql = " SELECT "
            strSql += " SNO,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += " ,TAGNO,ITEMID FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += " WHERE TAGNO = '" & txtTagNo.Text & "'"
            If itemId <> "" Then
                strSql += " AND ITEMID = " & Val(itemId) & ""
            End If
            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTagDet)
            If dtTagDet.Rows.Count > 0 Then
                If dtTagDet.Rows.Count = 1 Then
                    ObjPurDetail = New TagPurchaseDetailEntry
                    GettagDetails(dtTagDet.Rows(0).Item("SNO").ToString)
                    ShowPurDetails()
                    fNew()
                    Exit Sub
                End If
            Else
                MsgBox("Enter Tagno or Itemid invalid")
            End If
        End If
    End Sub
    Function GettagDetails(ByVal ISSsno As String)

        ''MultiMetal
        Dim dtMultiMetalDetails As New DataTable
        With dtMultiMetalDetails.Columns
            .Add("CATEGORY", GetType(String))
            .Add("RATE", GetType(Double))
            .Add("WEIGHT", GetType(Double))
            .Add("WASTAGEPER", GetType(Double))
            .Add("WASTAGE", GetType(Double))
            .Add("MCPERGRM", GetType(Double))
            .Add("MC", GetType(Double))
            .Add("AMOUNT", GetType(Double))
            .Add("PURWASTAGEPER", GetType(Double))
            .Add("PURWASTAGE", GetType(Double))
            .Add("PURMCPERGRM", GetType(Double))
            .Add("PURMC", GetType(Double))
            .Add("PURRATE", GetType(Double))
            .Add("PURAMOUNT", GetType(Double))
            .Add("STNSNO", GetType(String))
            .Add("KEYNO", GetType(Integer))
        End With
        dtMultiMetalDetails.Columns("KEYNO").AutoIncrement = True
        dtMultiMetalDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtMultiMetalDetails.Columns("KEYNO").AutoIncrementSeed = 1


        ''Stone
        Dim dtStoneDetails As New DataTable
        With dtStoneDetails.Columns
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("PCS", GetType(Int32))
            .Add("WEIGHT", GetType(Decimal))
            .Add("UNIT", GetType(String))
            .Add("CALC", GetType(String))
            .Add("RATE", GetType(Decimal))
            .Add("AMOUNT", GetType(Decimal))
            .Add("METALID", GetType(String))
            .Add("PURRATE", GetType(Decimal))
            .Add("PURVALUE", GetType(Decimal))
            .Add("KEYNO", GetType(Integer))
            .Add("STNSNO", GetType(String))
            .Add("USRATE", GetType(Decimal))
            .Add("INDRS", GetType(Decimal))
        End With
        dtStoneDetails.Columns("KEYNO").AutoIncrement = True
        dtStoneDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtStoneDetails.Columns("KEYNO").AutoIncrementSeed = 1

        ''OtherCharges
        Dim dtMiscDetails As New DataTable
        With dtMiscDetails.Columns
            .Add("MISC", GetType(String))
            .Add("AMOUNT", GetType(Double))
            .Add("PURAMOUNT", GetType(Double))
            .Add("KEYNO", GetType(Integer))
        End With
        dtMiscDetails.Columns("KEYNO").AutoIncrement = True
        dtMiscDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtMiscDetails.Columns("KEYNO").AutoIncrementSeed = 1


        tagEdit = True
        updIssSno = ISSsno
        Dim dtTagDetails As New DataTable
        strSql = " SELECT T.LOTSNO,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME"
        strSql += " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)AS ITEMTYPE"
        strSql += " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
        strSql += " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)AS COSTCENTRE"
        strSql += " ,T.RECDATE,T.PURITY"
        strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID),'')AS SUBITEM"
        strSql += " ,ISNULL((SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID),'')AS ITEMTYPE"
        strSql += " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS ITEMCOUNTER"
        strSql += " ,(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)AS ITEMSIZE"
        strSql += " ,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT,T.RATE,T.GRSNET,T.TABLECODE,T.STYLENO"
        strSql += " ,T.MAXWASTPER,T.MAXWAST,T.MAXMCGRM,T.MAXMC,T.MINWASTPER,T.MINWAST,T.MINMCGRM,T.MINMC"
        strSql += " ,T.SALVALUE,T.NARRATION,T.PCTFILE"
        strSql += " ,P.PURLESSWT,P.PURNETWT,P.PURRATE,P.PURGRSNET,P.PURTOUCH,P.PURWASTAGE,P.PURMC,P.PURVALUE"
        strSql += " ,T.BOARDRATE,T.ORSNO,T.RFID,P.PURTAX,P.RECDATE AS PURDATE,T.SALEMODE,T.TOUCH"
        strSql += " ,T.HM_BILLNO,T.HM_CENTER,T.ADD_VA_PER,T.REFVALUE,T.ORDREPNO,T.EXTRAWT,T.USRATE,T.INDRS"
        strSql += "  FROM " & cnAdminDb & "..ITEMTAG AS T "
        strSql += "  LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
        strSql += " WHERE T.SNO = '" & updIssSno & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTagDetails)
        If Not dtTagDetails.Rows.Count > 0 Then Me.Dispose()
        With dtTagDetails.Rows(0)
            TagNo__Man = txtTagNo.Text
            Item_MAN = .Item("ITEMNAME").ToString
            SubItem_Man = .Item("SUBITEM").ToString
            pFixedVa = IIf(objGPack.GetSqlValue("SELECT ISNULL(FIXEDVA,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Item_MAN & "'", , "N") = "N", False, True)


            strSql = " SELECT METALID,STUDDEDSTONE,SIZESTOCK,MULTIMETAL,OTHCHARGE,CALTYPE,NOOFPIECE,PIECERATE,VALUEADDEDTYPE,GROSSNETWTDIFF"
            strSql += " ,STUDDED,STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEMNAME").ToString & "'"
            Dim dtItemDetail As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItemDetail)
            If dtItemDetail.Rows.Count > 0 Then
                With dtItemDetail.Rows(0)
                    If SubItem_Man.ToString <> "" Then
                        pFixedVa = IIf(objGPack.GetSqlValue("SELECT ISNULL(FIXEDVA,'N') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & SubItem_Man & "'", , "N") = "N", False, True)
                    End If
                    If SubItem_Man <> "" Then
                        strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & SubItem_Man & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Item_MAN & "')"
                    Else
                        strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Item_MAN & "'"
                    End If
                    calType = objGPack.GetSqlValue(strSql)

                End With
            End If

            MetalRate_Amt = .Item("BOARDRATE").ToString ' GetMetalRate()
            RecieptDate = .Item("RECDATE")
            Pieces_Num_Man = .Item("PCS").ToString
            GrossWt_Wet = .Item("GRSWT").ToString
            LessWt_Wet = .Item("LESSWT").ToString
            NetWt_Wet = .Item("NETWT").ToString
            Rate_Amt = IIf(Val(.Item("RATE").ToString) <> 0, .Item("RATE").ToString, Nothing)
            CalcMode = IIf(.Item("GRSNET").ToString = "G", "GRS WT", "NET WT")

            ''MULTIMETAL
            strSql = " SELECT"
            strSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = T.CATCODE)AS CATEGORY"
            strSql += " ,GRSWT WEIGHT,WASTPER WASTAGEPER,WAST WASTAGE,MCGRM MCPERGRM,MC MC,AMOUNT,NULL PURRATE"
            strSql += " ,(SELECT PURWASTAGE FROM " & cnAdminDb & "..PURITEMTAGMETAL WHERE METSNO = T.SNO)AS PURWASTAGE"
            strSql += " ,(SELECT PURMC FROM " & cnAdminDb & "..PURITEMTAGMETAL WHERE METSNO = T.SNO)AS PURMC"
            strSql += " ,(SELECT PURVALUE FROM " & cnAdminDb & "..PURITEMTAGMETAL WHERE METSNO = T.SNO)AS PURAMOUNT"

            strSql += " FROM " & cnAdminDb & "..ITEMTAGMETAL AS T"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtMultiMetalDetails)


            ''STONEDETAILS
            strSql = " SELECT "
            strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)AS ITEM"
            strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = STNSUBITEMID)AS SUBITEM"
            strSql += " ,STNPCS PCS,STNWT WEIGHT"
            strSql += " ,STONEUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT"
            strSql += " ,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) AS METALID"
            strSql += " ,NULL PURRATE"
            strSql += " ,NULL PURVALUE,SNO STNSNO,USRATE,INDRS"
            'strSql += " ,PURRATE,PURAMT PURVALUE"
            strSql += " FROM " & cnAdminDb & "..ITEMTAGSTONE S"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtStoneDetails)
            dtStoneDetails.AcceptChanges()

            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                strSql = " SELECT "
                strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)AS ITEM"
                strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = STNSUBITEMID)AS SUBITEM"
                strSql += " ,STNPCS PCS,STNWT WEIGHT"
                strSql += " ,STONEUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT"
                strSql += " ,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) AS METALID"
                strSql += " ,PURRATE,PURAMT PURVALUE,STNSNO"
                strSql += " FROM " & cnAdminDb & "..PURITEMTAGSTONE S"
                strSql += " WHERE TAGSNO = '" & updIssSno & "'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(ObjPurDetail.dtGridStone)
                ObjPurDetail.dtGridStone.AcceptChanges()
            End If

            For Each RoPur As DataRow In ObjPurDetail.dtGridStone.Rows
                If RoPur.Item("STNSNO").ToString = "" Then Continue For
                For Each Ro_S As DataRow In dtStoneDetails.Rows
                    If Ro_S.Item("STNSNO") = RoPur.Item("STNSNO") Then
                        RoPur.Item("KEYNO") = Ro_S.Item("KEYNO")
                        Exit For
                    End If
                Next
            Next

            ''MISCCHARGE
            strSql = " SELECT"
            strSql += " (SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCID = T.MISCID)MISC"
            strSql += " ,AMOUNT"
            strSql += " ,(SELECT PURAMOUNT FROM " & cnAdminDb & "..PURITEMTAGMISCCHAR WHERE MISSNO = T.SNO)PURAMOUNT "
            strSql += " FROM " & cnAdminDb & "..ITEMTAGMISCCHAR AS T"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtMiscDetails)

            dtMiscDetails.AcceptChanges()


            If .Item("PURDATE").ToString <> "" Then
                ObjPurDetail.dtpPurchaseDate.Value = .Item("PURDATE")
            End If
            ObjPurDetail.txtpURFixedValueVa_AMT.Text = IIf(Val(.Item("ADD_VA_PER").ToString) <> 0, Format(Val(.Item("ADD_VA_PER").ToString), "0.00"), "")
            ObjPurDetail.salePcs = Val(Pieces_Num_Man)
            ObjPurDetail.CalcMode = calType
            ObjPurDetail.txtPurGrossWt_Wet.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, .Item("GRSWT").ToString, Nothing)
            ObjPurDetail.txtPurLessWt_Wet.Text = IIf(Val(.Item("PURLESSWT").ToString) <> 0, .Item("PURLESSWT").ToString, Nothing)
            ObjPurDetail.txtPurNetWt_Wet.Text = IIf(Val(.Item("PURNETWT").ToString) <> 0, .Item("PURNETWT").ToString, Nothing)
            purRate = Val(.Item("PURRATE").ToString)
            ObjPurDetail.txtPurRate_Amt.Text = IIf(Val(.Item("PURRATE").ToString) <> 0, .Item("PURRATE").ToString, Nothing)
            'ObjPurDetail.cmbPurCalMode.Text = IIf(.Item("PURGRSNET").ToString = "G", ObjPurDetail.cmbPurCalMode.Text = "GRS WT", ObjPurDetail.cmbPurCalMode.Text = "NET WT")
            If .Item("PURGRSNET").ToString = "G" Then
                ObjPurDetail.cmbPurCalMode.Text = "GRS WT"
            Else
                ObjPurDetail.cmbPurCalMode.Text = "NET WT"
            End If
            ObjPurDetail.txtPurTouch_Amt.Text = IIf(Val(.Item("PURTOUCH").ToString) <> 0, .Item("PURTOUCH").ToString, Nothing)
            ObjPurDetail.txtPurWastage_Wet.Text = IIf(Val(.Item("PURWASTAGE").ToString) <> 0, .Item("PURWASTAGE").ToString, Nothing)
            ObjPurDetail.txtPurMakingChrg_Amt.Text = IIf(Val(.Item("PURMC").ToString) <> 0, .Item("PURMC").ToString, Nothing)
            ObjPurDetail.txtPurTax_AMT.Text = IIf(Val(.Item("PURTAX").ToString) <> 0, .Item("PURTAX").ToString, Nothing)
            ObjPurDetail.txtPurPurchaseVal_Amt.Text = IIf(Val(.Item("PURVALUE").ToString) <> 0, .Item("PURVALUE").ToString, Nothing)
            ObjPurDetail.CalcPurchaseGrossValue()
            ObjPurDetail.CalcPurchaseValue()

            lotPcs = Val(Pieces_Num_Man)
            lotGrsWt = Val(GrossWt_Wet)
            lotNetWt = Val(NetWt_Wet)

        End With

    End Function

    Private Sub ShowPurDetails()
        If _HasPurchase = False And pFixedVa = False And PUR_AUTOCALC = False Then Exit Sub
        Dim RowCheck() As DataRow = Nothing
        With ObjPurDetail
            .CalcMode = calType
            If pFixedVa Then
                .lblFixedValVa.Visible = True
                .txtpURFixedValueVa_AMT.Visible = True
            Else
                .lblFixedValVa.Visible = False
                .txtpURFixedValueVa_AMT.Visible = False
            End If
            .salePcs = Val(Pieces_Num_Man)
            If Not flagPurValSet Then
                If Not tagEdit Then
                    .cmbPurCalMode.Text = CalcMode
                    .txtPurGrossWt_Wet.Text = GrossWt_Wet
                    .txtPurLessWt_Wet.Text = LessWt_Wet
                    .txtPurNetWt_Wet.Text = NetWt_Wet
                    .dtpPurchaseDate.Value = RecieptDate
                    .txtPurMcPerGrm_TextChanged(Me, New EventArgs)
                    If purRate = 0 Then
                        .txtPurRate_Amt.Text = IIf(calType <> "W" And calType <> "B", Rate_Amt, MetalRate_Amt)
                    Else
                        .txtPurRate_Amt.Text = purRate
                    End If
                End If
                SyncStoneMiscToPurStoneMisc()
            End If
            For cnt As Integer = 0 To dtStoneDetails.Rows.Count - 1
                RowCheck = .dtGridStone.Select("KEYNO = " & Val(dtStoneDetails.Rows(cnt).Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    .dtGridStone.ImportRow(dtStoneDetails.Rows(cnt))
                End If
            Next
            .dtGridStone.AcceptChanges()
            For cnt As Integer = 0 To dtStoneDetails.Rows.Count - 1
                RowCheck = .dtGridStone.Select("KEYNO = " & Val(dtStoneDetails.Rows(cnt).Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    .dtGridStone.ImportRow(dtStoneDetails.Rows(cnt))
                End If
            Next
            .dtGridStone.AcceptChanges()

            If Not flagPurValSet Then
                If Not tagEdit Then
                    If Val(.txtPurTaxPer_PER.Text) = 0 And Val(.txtPurTax_AMT.Text) = 0 Then
                        Dim pTax As Decimal = Val(objGPack.GetSqlValue("SELECT PTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (sELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Item_MAN & "')"))
                        .txtPurTaxPer_PER.Text = IIf(pTax <> 0, Format(pTax, "0.00"), "")
                    End If
                End If
            End If
            If Not (_HasPurchase = False And pFixedVa = False) Then
                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    If tagEdit Then
                        UpdateTag()
                        Exit Sub
                    Else
                        funcAdd()
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub SyncStoneMiscToPurStoneMisc()
        Dim RowCheck() As DataRow = Nothing
        ''Set Stone Value
        With ObjPurDetail
            If Not .dtGridStone.Rows.Count > 0 Then
StDel:
                For Each ro As DataRow In .dtGridStone.Rows
                    RowCheck = dtStoneDetails.Select("KEYNO = " & Val(ro.Item("KEYNO").ToString) & "")
                    If RowCheck.Length = 0 Then
                        ro.Delete()
                        .dtGridStone.AcceptChanges()
                        GoTo StDel
                    End If
                Next
                .dtGridStone.AcceptChanges()
                For cnt As Integer = 0 To dtStoneDetails.Rows.Count - 1
                    RowCheck = .dtGridStone.Select("KEYNO = " & Val(dtStoneDetails.Rows(cnt).Item("KEYNO").ToString) & "")
                    If RowCheck.Length = 0 Then
                        .dtGridStone.ImportRow(dtStoneDetails.Rows(cnt))
                    End If
                Next
                .dtGridStone.AcceptChanges()

                Dim DtTemp As New DataTable
                Dim purRate As Decimal = Nothing
                For Each Row As DataRow In .dtGridStone.Rows
                    Dim stItemId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Row.Item("ITEM").ToString & "'"))
                    Dim wt As Decimal = (Val(Row.Item("WEIGHT").ToString) / IIf(Val(Row.Item("PCS").ToString) <> 0, Val(Row.Item("PCS").ToString), 1)) * 100
                    strSql = vbCrLf + " SELECT C.PURRATE FROM " & cnAdminDb & "..CENTRATE AS C"
                    strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = C.ITEMID AND IM.ITEMNAME = '" & Row.Item("ITEM").ToString & "'"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.SUBITEMID = C.SUBITEMID AND SM.ITEMID = C.ITEMID AND SM.SUBITEMNAME = '" & Row.Item("SUBITEM").ToString & "'"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..DESIGNER AS DE ON DE.ACCODE = C.ACCODE AND DE.DESIGNERNAME = '" & Designer_MAN & "'"
                    strSql += vbCrLf + " WHERE " & wt & " BETWEEN FROMCENT AND TOCENT"
                    strSql += vbCrLf + " AND ISNULL(C.SUBITEMID,0) = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & Row.Item("SUBITEM").ToString & "' AND ITEMID = " & stItemId & "),0)"
                    'strSql += vbCrLf + " WHERE ISNULL(CONVERT(NUMERIC(15,4),(" & Val(Row.Item("WEIGHT").ToString) & " /CASE WHEN " & Val(Row.Item("PCS").ToString) & "  = 0 THEN 1 ELSE " & Val(Row.Item("PCS").ToString) & " END)*100),0) BETWEEN FROMCENT AND TOCENT"
                    strSql += vbCrLf + " ORDER BY C.ACCODE desc,C.SUBITEMID desc,C.ITEMID"
                    DtTemp = New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(DtTemp)
                    If DtTemp.Rows.Count > 0 Then
                        Row.Item("PURRATE") = IIf(Val(DtTemp.Rows(0).Item("PURRATE").ToString) <> 0, Val(DtTemp.Rows(0).Item("PURRATE").ToString), DBNull.Value)
                        If Val(Row.Item("PURRATE").ToString) <> 0 Then
                            Row.Item("PURVALUE") = IIf(Row.Item("CALC").ToString = "P", Val(Row.Item("PCS").ToString), Val(Row.Item("WEIGHT").ToString)) * Val(Row.Item("PURRATE").ToString)
                        End If
                    End If
                Next
            End If
            .CalcStoneWtAmount()
MiscDel:
            ''Misc Value
            For Each ro As DataRow In .dtGridMisc.Rows
                RowCheck = dtMiscDetails.Select("KEYNO = " & Val(ro.Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    ro.Delete()
                    .dtGridMisc.AcceptChanges()
                    GoTo MiscDel
                End If
            Next
            .dtGridMisc.AcceptChanges()
            For cnt As Integer = 0 To dtMiscDetails.Rows.Count - 1
                RowCheck = .dtGridMisc.Select("KEYNO = " & Val(dtMiscDetails.Rows(cnt).Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    .dtGridMisc.ImportRow(dtMiscDetails.Rows(cnt))
                End If
            Next
            .dtGridMisc.AcceptChanges()
            .CalcMiscTotalAmount()
MetalDel:
            ''MultiMetal Value
            For Each ro As DataRow In .dtGridMultiMetal.Rows
                RowCheck = dtMultiMetalDetails.Select("KEYNO = " & Val(ro.Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    ro.Delete()
                    .dtGridMultiMetal.AcceptChanges()
                    GoTo MetalDel
                End If
            Next
            .dtGridMultiMetal.AcceptChanges()
            For cnt As Integer = 0 To dtMultiMetalDetails.Rows.Count - 1
                RowCheck = .dtGridMultiMetal.Select("KEYNO = " & Val(dtMultiMetalDetails.Rows(cnt).Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    .dtGridMultiMetal.ImportRow(dtMultiMetalDetails.Rows(cnt))
                End If
            Next
            .dtGridMultiMetal.AcceptChanges()
        End With
    End Sub
    Private Function GetSoftValue(ByVal id As String) As String
        Return UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & id & "'", , "", tran))
    End Function

    Function funcAdd() As Integer
        Dim RowFiter() As DataRow = Nothing
        Dim TagSno As String = Nothing
        Dim TagNo As String = Nothing
        Dim COSTID As String = Nothing
        Dim itemId As Integer = Nothing
        Dim subitemId As Integer = Nothing
        Dim sizeId As Integer = Nothing
        Dim itemCtrId As Integer = Nothing
        Dim designerId As Integer = Nothing
        Dim tagVal As Integer = 0
        'Dim saleMode As String = Nothing
        Dim itemTypeId As Integer = Nothing
        Dim tranInvNo As String = Nothing
        Dim supBillno As String = Nothing

        Dim stlPcs As Integer = 0
        Dim stlWt As Double = 0
        Dim stlType As String = Nothing

        Dim dialPcs As Integer = 0
        Dim dialWt As Double = 0


        Try
            tran = Nothing
            tran = cn.BeginTransaction()
            ''Find TagSno

TagDupGen:
            If GetSoftValue("TAGNOFROM") = "I" Then
                strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
                strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Item_MAN & "') "
                strSql += " AND TAGNO = '" & TagNo__Man & "'"
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    TagNo__Man = objTag.GetTagNo(RecieptDate.ToString("yyyy-MM-dd"), Item_MAN, SNO, tran)
                    GoTo TagDupGen
                End If
            ElseIf GetSoftValue("TAGNOFROM") = "U" Then
                TagNo__Man = objTag.GetTagNo(RecieptDate.ToString("yyyy-MM-dd"), Item_MAN, SNO, tran)
            End If

            TagNo = TagNo__Man

            ''Find COSTID
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME  = '" & CostCentre_Man & "'"
            COSTID = objGPack.GetSqlValue(strSql, "COSTID", , tran)

            ''Find ItemId
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & Item_MAN & "'"
            itemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))

            ''Find DesignerId
            strSql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & Designer_MAN & "'"
            designerId = Val(objGPack.GetSqlValue(strSql, "DESIGNERID", "", tran))

            ''Find itemTypeId
            strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & ItemType_MAN & "'"
            itemTypeId = Val(objGPack.GetSqlValue(strSql, "ITEMTYPEID", , tran))

            ''FIND TRANINVNO AND SUPBILLNO
            strSql = " SELECT TRANINVNO,BILLNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"
            tranInvNo = objGPack.GetSqlValue(strSql, "TRANINVNO", , tran)
            supBillno = objGPack.GetSqlValue(strSql, "BILLNO", , tran)

            Dim purStoneValue As Double
            For Each roStn As DataRow In dtStoneDetails.Rows
                purStoneValue += Val(roStn!PURVALUE.ToString)
            Next

            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
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
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " VALUES"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " '" & TagSno & "'" 'TAGSNO
                strSql += vbCrLf + " ," & itemId & "" 'ITEMID
                strSql += vbCrLf + " ,'" & TagNo & "'" 'TAGNO
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurLessWt_Wet.Text) & "" ' PURLESSWT
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurNetWt_Wet.Text) & "" ' PURNETWT"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurRate_Amt.Text) & "" ' PURRATE"
                strSql += vbCrLf + " ,'" & Mid(ObjPurDetail.cmbPurCalMode.Text, 1, 1) & "'" ' PURGRSNET"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurWastage_Wet.Text) & "" ' PURWASTAGE"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurTouch_Amt.Text) & "" ' PURTOUCH"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurMakingChrg_Amt.Text) & "" ' PURMC"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurPurchaseVal_Amt.Text) & "" ' PURVALUE"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurTax_AMT.Text) & ""
                strSql += vbCrLf + " ,'" & ObjPurDetail.dtpPurchaseDate.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                strSql += vbCrLf + " )"
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TPURITEMTAG", , False)
            End If
            ''Inserting StoneDetail
            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                For Each RoPur As DataRow In ObjPurDetail.dtGridStone.Rows
                    If RoPur.Item("KEYNO").ToString = "" Then Continue For
                    For Each Ro_S As DataRow In dtStoneDetails.Rows
                        If Ro_S.Item("KEYNO") = RoPur.Item("KEYNO") Then
                            RoPur.Item("STNSNO") = Ro_S.Item("STNSNO")
                            Exit For
                        End If
                    Next
                Next

                Dim stnItemId As Integer = 0
                Dim stnSubItemId As Integer = 0
                For Each RoPur As DataRow In ObjPurDetail.dtGridStone.Rows
                    'Dim caType As String = Nothing
                    strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & RoPur.Item("ITEM").ToString & "'"
                    stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                    strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & RoPur.Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                    stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                    strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGSTONE"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " TAGSNO"
                    strSql += vbCrLf + " ,ITEMID"
                    strSql += vbCrLf + " ,TAGNO"
                    strSql += vbCrLf + " ,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNRATE,STNAMT,STONEUNIT,CALCMODE"
                    strSql += vbCrLf + " ,PURRATE"
                    strSql += vbCrLf + " ,PURAMT,COMPANYID,COSTID"
                    strSql += vbCrLf + " ,STNSNO"
                    strSql += vbCrLf + " )"
                    strSql += vbCrLf + " VALUES"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " '" & TagSno & "'" 'TAGSNO
                    strSql += vbCrLf + " ," & itemId & "" 'ITEMID
                    strSql += vbCrLf + " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ," & stnItemId & "" 'STNITEMID
                    strSql += " ," & stnSubItemId & "" 'STNSUBITEMID
                    strSql += " ," & Val(RoPur.Item("PCS").ToString) & "" 'STNPCS
                    strSql += " ," & Val(RoPur.Item("WEIGHT").ToString) & "" 'STNWT
                    strSql += " ," & Val(RoPur.Item("RATE").ToString) & "" 'STNRATE
                    strSql += " ," & Val(RoPur.Item("AMOUNT").ToString) & "" 'STNAMT
                    strSql += " ,'" & RoPur.Item("UNIT").ToString & "'" 'STONEUNIT
                    strSql += " ,'" & RoPur.Item("CALC").ToString & "'" 'CALCMODE
                    strSql += vbCrLf + " ," & Val(RoPur.Item("PURRATE").ToString) & "" 'PURRATE
                    strSql += vbCrLf + " ," & Val(RoPur.Item("PURVALUE").ToString) & "" 'PURAMT
                    strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                    strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                    strSql += vbCrLf + " ,'" & RoPur.Item("STNSNO").ToString & "'"
                    strSql += vbCrLf + " )"
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TPURITEMTAGSTONE", , False)
                Next
            End If
            ''iNSERTING mISC
            For cnt As Integer = 0 To dtMiscDetails.Rows.Count - 1
                Dim miscSno As String = GetNewSno(TranSnoType.ITEMTAGMISCCHARCODE, tran, "GET_ADMINSNO_TRAN")
                If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                    RowFiter = ObjPurDetail.dtGridMisc.Select("KEYNO = " & Val(dtMiscDetails.Rows(cnt).Item("KEYNO").ToString))
                    If RowFiter.Length > 0 Then
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
                        strSql += vbCrLf + " ,'" & TagSno & "'" 'TAGSNO
                        strSql += " ," & itemId & "" 'ITEMID
                        strSql += " ,'" & TagNo & "'" 'TAGNO
                        strSql += " ," & Val(RowFiter(0).Item("PURAMOUNT").ToString) & "" 'PURAMOUNT
                        strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                        strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                        strSql += vbCrLf + " ,'" & miscSno & "'" 'STNSNO
                        strSql += vbCrLf + " )"
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                        'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TPURITEMTAGMISCCHAR", , False)
                    End If
                End If
            Next

            ''INSERTING MULTIMETAL
            For cnt As Integer = 0 To dtMultiMetalDetails.Rows.Count - 1
                Dim metalSno As String = GetNewSno(TranSnoType.ITEMTAGMETALCODE, tran, "GET_ADMINSNO_TRAN")
                Dim multiMetalId As String = Nothing
                Dim multiCatCode As String = Nothing
                If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                    RowFiter = ObjPurDetail.dtGridMultiMetal.Select("KEYNO = " & Val(dtMultiMetalDetails.Rows(cnt).Item("KEYNO").ToString))
                    If RowFiter.Length > 0 Then
                        strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGMETAL"
                        strSql += vbCrLf + " ("
                        strSql += vbCrLf + " METSNO,TAGSNO"
                        strSql += vbCrLf + " ,ITEMID"
                        strSql += vbCrLf + " ,TAGNO,PURRATE,PURWASTAGE,PURMC,PURTOUCH,PURVALUE"
                        strSql += vbCrLf + " ,COMPANYID,COSTID"
                        strSql += vbCrLf + " )"
                        strSql += vbCrLf + " VALUES"
                        strSql += vbCrLf + " ("
                        strSql += vbCrLf + " '" & metalSno & "'" 'MISSNO
                        strSql += vbCrLf + " ,'" & TagSno & "'" 'TAGSNO
                        strSql += " ," & itemId & "" 'ITEMID
                        strSql += " ,'" & TagNo & "'" 'TAGNO
                        strSql += ",0" 'PURRATE
                        strSql += "," & Val(RowFiter(0).Item("PURWASTAGE").ToString) & "" 'PURWASTAGE
                        strSql += "," & Val(RowFiter(0).Item("PURMC").ToString) & "" 'PURMC
                        strSql += ",0" 'PURTOUCH
                        strSql += "," & Val(RowFiter(0).Item("PURAMOUNT").ToString) & "" 'PURVALUE
                        strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                        strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                        strSql += vbCrLf + " )"
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                        'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TPURITEMTAGMISCCHAR", , False)
                    End If
                End If
            Next

            Dim tagPrefix As String = GetSoftValue("TAGPREFIX")
            Dim updTagNo As String = Nothing
            If tagPrefix.Length > 0 Then
                updTagNo = TagNo__Man.Replace(tagPrefix, "")
            Else
                updTagNo = TagNo__Man
            End If

            If GetSoftValue("TAGNOFROM") = "I" Then ''FROM ITEMMAST OR UNIQUE
                If GetSoftValue("TAGNOGEN") = "I" Then ''FROM ITEM
                    strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO = '" & updTagNo & "' WHERE ITEMNAME = '" & Item_MAN & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                    'ExecQuery(strSql, cn, tran, COSTID)
                Else ''FROM LOT
                    strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CURTAGNO = '" & updTagNo & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                    'ExecQuery(strSql, cn, tran, COSTID)
                End If
            ElseIf GetSoftValue("TAGNOFROM") = "U" Then  'UNIQUE
                strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT  = '" & updTagNo & "' WHERE CTLID = 'LASTTAGNO'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                'ExecQuery(strSql, cn, tran, COSTID)
            End If


            'CPIECES AND CWT
            strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CPCS = CPCS + " & Val(Pieces_Num_Man) & ""
            strSql += " ,CGRSWT = CGRSWT + " & Val(GrossWt_Wet) & ""
            strSql += " ,CNETWT = ISNULL(CNETWT,0) + " & Val(NetWt_Wet) & ""
            strSql += " WHERE SNO = '" & SNO & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            'ExecQuery(strSql, cn, tran, COSTID)


            tran.Commit()
            tran = Nothing
            MsgBox(TagNo + E0012, MsgBoxStyle.Exclamation)
            ''Lot Pcs
            Dim flagComplete As Boolean = False

        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Function

    Private Sub TagEditSave()

        Dim RowFiter() As DataRow = Nothing
        Dim COSTID As String = Nothing
        Dim itemId As Integer = Nothing
        Dim subitemId As Integer = Nothing
        Dim sizeId As Integer = Nothing
        Dim itemCtrId As Integer = Nothing
        Dim designerId As Integer = Nothing

        Dim itemTypeId As Integer = Nothing
        Dim tranInvNo As String = Nothing
        Dim supBillno As String = Nothing

        Dim stlPcs As Integer = 0
        Dim stlWt As Double = 0
        Dim stlType As String = Nothing

        Dim dialPcs As Integer = 0
        Dim dialWt As Double = 0

        Try
            tran = Nothing
            tran = cn.BeginTransaction()

            ''Find COSTID
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME  = '" & CostCentre_Man & "'"
            COSTID = objGPack.GetSqlValue(strSql, "COSTID", , tran)

            ''Find ItemId
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & Item_MAN & "'"
            itemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
            ''Find DesignerId
            strSql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & Designer_MAN & "'"
            designerId = Val(objGPack.GetSqlValue(strSql, "DESIGNERID", "", tran))

            ''Find itemTypeId
            strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & ItemType_MAN & "'"
            itemTypeId = Val(objGPack.GetSqlValue(strSql, "ITEMTYPEID", , tran))

            ''FIND TRANINVNO AND SUPBILLNO
            strSql = " SELECT TRANINVNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"
            tranInvNo = objGPack.GetSqlValue(strSql, "TRANINVNO", , tran)
            strSql = " SELECT BILLNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"
            supBillno = objGPack.GetSqlValue(strSql, "BILLNO", , tran)

            Dim purStnValue As Double
            For Each roStn As DataRow In dtStoneDetails.Rows
                purStnValue += Val(roStn!PURVALUE.ToString)
            Next

            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                ''DELETING PURSTONEDETAIL
                strSql = " DELETE FROM " & cnAdminDb & "..PURITEMTAG"
                strSql += " WHERE TAGSNO = '" & updIssSno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If
            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
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
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " VALUES"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " '" & updIssSno & "'" 'TAGSNO
                strSql += vbCrLf + " ," & itemId & "" 'ITEMID
                strSql += vbCrLf + " ,'" & TagNo__Man & "'" 'TAGNO
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurLessWt_Wet.Text) & "" ' PURLESSWT
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurNetWt_Wet.Text) & "" ' PURNETWT"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurRate_Amt.Text) & "" ' PURRATE"
                strSql += vbCrLf + " ,'" & Mid(ObjPurDetail.cmbPurCalMode.Text, 1, 1) & "'" ' PURGRSNET"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurWastage_Wet.Text) & "" ' PURWASTAGE"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurTouch_Amt.Text) & "" ' PURTOUCH"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurMakingChrg_Amt.Text) & "" ' PURMC"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurPurchaseVal_Amt.Text) & "" ' PURVALUE"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurTax_AMT.Text) & ""
                strSql += vbCrLf + " ,'" & ObjPurDetail.dtpPurchaseDate.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                strSql += vbCrLf + " )"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If

            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                ''DELETING PURSTONEDETAIL
                strSql = " DELETE FROM " & cnAdminDb & "..PURITEMTAGSTONE"
                strSql += " WHERE TAGSNO = '" & updIssSno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If
            ''DELETING STONEDETAIL
            strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGSTONE"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            ''Inserting StoneDetail
            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                For Each RoPur As DataRow In ObjPurDetail.dtGridStone.Rows
                    If RoPur.Item("KEYNO").ToString = "" Then Continue For
                    For Each Ro_S As DataRow In dtStoneDetails.Rows
                        If Ro_S.Item("KEYNO") = RoPur.Item("KEYNO") Then
                            RoPur.Item("STNSNO") = Ro_S.Item("STNSNO")
                            Exit For
                        End If
                    Next
                Next
                Dim stnItemId As Integer = 0
                Dim stnSubItemId As Integer = 0
                For Each RoPur As DataRow In ObjPurDetail.dtGridStone.Rows
                    'Dim caType As String = Nothing
                    strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & RoPur.Item("ITEM").ToString & "'"
                    stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                    strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & RoPur.Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                    stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                    strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGSTONE"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " TAGSNO"
                    strSql += vbCrLf + " ,ITEMID"
                    strSql += vbCrLf + " ,TAGNO"
                    strSql += vbCrLf + " ,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNRATE,STNAMT,STONEUNIT,CALCMODE"
                    strSql += vbCrLf + " ,PURRATE"
                    strSql += vbCrLf + " ,PURAMT,COMPANYID,COSTID,STNSNO"
                    strSql += vbCrLf + " )"
                    strSql += vbCrLf + " VALUES"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " '" & updIssSno & "'" 'TAGSNO
                    strSql += vbCrLf + " ," & itemId & "" 'ITEMID
                    strSql += vbCrLf + " ,'" & TagNo__Man & "'" 'TAGNO
                    strSql += " ," & stnItemId & "" 'STNITEMID
                    strSql += " ," & stnSubItemId & "" 'STNSUBITEMID
                    strSql += " ," & Val(RoPur.Item("PCS").ToString) & "" 'STNPCS
                    strSql += " ," & Val(RoPur.Item("WEIGHT").ToString) & "" 'STNWT
                    strSql += " ," & Val(RoPur.Item("RATE").ToString) & "" 'STNRATE
                    strSql += " ," & Val(RoPur.Item("AMOUNT").ToString) & "" 'STNAMT
                    strSql += " ,'" & RoPur.Item("UNIT").ToString & "'" 'STONEUNIT
                    strSql += " ,'" & RoPur.Item("CALC").ToString & "'" 'CALCMODE
                    strSql += vbCrLf + " ," & Val(RoPur.Item("PURRATE").ToString) & "" 'PURRATE
                    strSql += vbCrLf + " ," & Val(RoPur.Item("PURVALUE").ToString) & "" 'PURAMT
                    strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                    strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                    strSql += vbCrLf + " ,'" & RoPur.Item("STNSNO").ToString & "'" 'STNSNO
                    strSql += vbCrLf + " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
            End If
            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                ''DELETING PURMISCDETAIL
                strSql = " DELETE FROM " & cnAdminDb & "..PURITEMTAGMISCCHAR"
                strSql += " WHERE TAGSNO = '" & updIssSno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If
            ''DELETING MISCDETAIL
            strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGMISCCHAR"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            ''iNSERTING mISC
            For cnt As Integer = 0 To dtMiscDetails.Rows.Count - 1
                Dim miscSno As String = GetNewSno(TranSnoType.ITEMTAGMISCCHARCODE, tran, "GET_ADMINSNO_TRAN")
                If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                    RowFiter = ObjPurDetail.dtGridMisc.Select("KEYNO = " & Val(dtMiscDetails.Rows(cnt).Item("KEYNO").ToString))
                    If RowFiter.Length > 0 Then
                        strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGMISCCHAR"
                        strSql += vbCrLf + " ("
                        strSql += vbCrLf + " MISSNO,TAGSNO"
                        strSql += vbCrLf + " ,ITEMID"
                        strSql += vbCrLf + " ,TAGNO"
                        strSql += vbCrLf + " ,PURAMOUNT"
                        strSql += vbCrLf + " ,COMPANYID,COSTID"
                        strSql += vbCrLf + " )"
                        strSql += vbCrLf + " VALUES"
                        strSql += vbCrLf + " ("
                        strSql += vbCrLf + " '" & miscSno & "'" 'MISSNO
                        strSql += vbCrLf + " ,'" & updIssSno & "'" 'TAGSNO
                        strSql += " ," & itemId & "" 'ITEMID
                        strSql += " ,'" & TagNo__Man & "'" 'TAGNO
                        strSql += " ," & Val(RowFiter(0).Item("PURAMOUNT").ToString) & "" 'PURAMOUNT
                        strSql += " ,'" & strCompanyId & "'"
                        strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                        strSql += vbCrLf + " )"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    End If
                End If
            Next

            ''DELETING MULTIMETALDETAIL
            strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGMETAL"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            ''INSERTING MULTIMETAL
            For cnt As Integer = 0 To dtMultiMetalDetails.Rows.Count - 1
                Dim metalSno As String = GetNewSno(TranSnoType.ITEMTAGMETALCODE, tran, "GET_ADMINSNO_TRAN")
                Dim multiMetalId As String = Nothing
                Dim multiCatCode As String = Nothing
                If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                    RowFiter = ObjPurDetail.dtGridMultiMetal.Select("KEYNO = " & Val(dtMultiMetalDetails.Rows(cnt).Item("KEYNO").ToString))
                    If RowFiter.Length > 0 Then
                        strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGMETAL"
                        strSql += vbCrLf + " ("
                        strSql += vbCrLf + " METSNO,TAGSNO"
                        strSql += vbCrLf + " ,ITEMID"
                        strSql += vbCrLf + " ,TAGNO,PURRATE,PURWASTAGE,PURMC,PURTOUCH,PURVALUE"
                        strSql += vbCrLf + " ,COMPANYID,COSTID"
                        strSql += vbCrLf + " )"
                        strSql += vbCrLf + " VALUES"
                        strSql += vbCrLf + " ("
                        strSql += vbCrLf + " '" & metalSno & "'" 'MISSNO
                        strSql += vbCrLf + " ,'" & updIssSno & "'" 'TAGSNO
                        strSql += " ," & itemId & "" 'ITEMID
                        strSql += " ,'" & TagNo__Man & "'" 'TAGNO
                        strSql += ",0" 'PURRATE
                        strSql += "," & Val(RowFiter(0).Item("PURWASTAGE").ToString) & "" 'PURWASTAGE
                        strSql += "," & Val(RowFiter(0).Item("PURMC").ToString) & "" 'PURMC
                        strSql += ",0" 'PURTOUCH
                        strSql += "," & Val(RowFiter(0).Item("PURAMOUNT").ToString) & "" 'PURVALUE
                        strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                        strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                        strSql += vbCrLf + " )"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    End If
                End If
            Next

            'CPIECES AND CWT
            strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CPCS = CPCS + " & (Val(Pieces_Num_Man) - lotPcs) & ""
            strSql += " ,CGRSWT = CGRSWT + " & (Val(GrossWt_Wet) - lotGrsWt) & ""
            strSql += " ,CNETWT = ISNULL(CNETWT,0) + " & (Val(NetWt_Wet) - lotNetWt) & ""
            strSql += " WHERE SNO = '" & SNO & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()


            tran.Commit()
            tran = Nothing

        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try

    End Sub
    Private Sub UpdateTag()
        'If objGPack.Validator_Check(grpSaveControls) Then Exit Sub
        Dim ds As New DataSet
        ds.Clear()
        TagEditSave()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        fNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub


    Private Sub TagwisePurchaseUpdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub TagwisePurchaseUpdate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        fNew()
    End Sub
End Class