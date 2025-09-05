Imports System.Data.OleDb
Public Class frmNonTagReceipt
    'CALNO 240113 VASANTHAN ,CLIENT - KFJ
    Dim strSql As String

    Dim da As OleDbDataAdapter
    Dim tran As OleDbTransaction
    Dim cmd As OleDbCommand
    Dim dReader As OleDbDataReader
    Dim SNO As String = Nothing
    Dim objStone As frmStoneDia

    Dim focusPiece As Boolean = True
    Dim pieceRate As Double = 0
    Dim calType As String
    Dim packetNoDupFlag As Boolean = False
    Public UpdateFlag As Boolean
    Dim ISSsno1 As String
    Dim pices As Integer
    Dim Weight As Decimal
    Dim NetWeight As Decimal
    Dim ObjExtraWt As New frmExtaWt
    Dim mzitemid As Integer
    Dim mzdesignid, mzsubid, mztypeid As Integer
    Dim LotCheckBy As String = GetAdmindbSoftValue("LOTCHECKBY", "P")
    Private LOCKWOPCRATE As Boolean = IIf(GetAdmindbSoftValue("LOCKWOPCRATE", "N") = "Y", True, False)
    Dim NONTAGPRINT As Boolean = IIf(GetAdmindbSoftValue("NONTAGPRINT", "N") = "Y", True, False)
    Dim LOTPCSWT As Boolean = IIf(GetAdmindbSoftValue("LOTPCSWT", "Y") = "Y", True, False)
    Dim NonTagSno As String = ""
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "")


    Private Sub Initializer()
        Dim dt As New DataTable
        ''CostCentre Checking..
        strSql = " SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' AND CTLTEXT = 'Y'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            cmbCostCentre_MAN.Text = ""
            cmbCostCentre_MAN.Items.Clear()
            cmbCostCentre_MAN.Enabled = False
        End If
        cmbStoneUnit.Items.Add("CARAT")
        cmbStoneUnit.Items.Add("GRAM")
        strSql = " SELECT NARRATION FROM " & cnAdminDb & "..NARRATION WHERE MODULEID = 'S'"
        objGPack.FillCombo(strSql, cmbNarration_OWN, , False)

        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ITEMCOUNTER'"
        If objGPack.GetSqlValue(strSql, , "N") = "N" Then cmbCounter_MAN.Enabled = False
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "N" Then cmbCostCentre_MAN.Enabled = False

        ' Add any initialization after the InitializeComponent() call.
        If cmbCostCentre_MAN.Enabled Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre_MAN)
        End If
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner_MAN)
        If cmbCounter_MAN.Enabled Then
            strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY DISPLAYORDER,ITEMCTRNAME"
            objGPack.FillCombo(strSql, cmbCounter_MAN)
        End If

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Initializer()
    End Sub
    Public Sub New(ByVal ISSsno As String)
        ' This call is required by the Windows Form Designer.
        Me.BackColor = frmBackColor
        Me.BackgroundImage = bakImage
        Me.BackgroundImageLayout = ImageLayout.Stretch
        Me.StartPosition = FormStartPosition.CenterScreen
        InitializeComponent()
        objGPack.Validator_Object(Me)
        Initializer()
        funcNew()
        btnNew.Enabled = False
        txtPacketNo__Man.Enabled = False
        'txtLotNo_Num_Man.Enabled = False
        dtpTranDate.Enabled = False
        'txtItem_Man.Enabled = False
        'txtMetalRate_Amt.Enabled = False
        ISSsno1 = ISSsno

        strSql = " SELECT SNO,RECDATE,PCS,GRSWT,LESSWT,NETWT,FINRATE,ISSTYPE,RECISS,LOTNO,PACKETNO,DREFNO,ORDREPNO,NARRATION,"
        strSql += " PURWASTAGE,PURRATE,PURMC,RATE,CTGRM,MC,WASTPER,WASTAGE,MCPERGRM,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) AS ITEMNAME,"
        strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID) AS SUBITEM,"
        strSql += " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = I.ITEMCTRID) AS ITEMCTRNAME,"
        strSql += " (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID) AS COSTNAME,"
        strSql += " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = I.DESIGNERID) AS DESIGNER,"
        strSql += " ISNULL((SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = I.ITEMTYPEID),'') AS TYPENAME"
        strSql += " ,LOTSNO,I.EXTRAWT"
        strSql += " FROM " & cnAdminDb & "..ITEMNONTAG I "
        strSql += " WHERE RECISS = 'R' AND SNO = '" & ISSsno & "' "
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                UpdateFlag = True
                SNO = .Item("LOTSNO").ToString
                txtItem_Man.Text = .Item("ITEMNAME").ToString
                AssignValues()
                txtLotNo_Num_Man.Text = .Item("LOTNO").ToString
                dtpTranDate.Value = .Item("RECDATE")
                txtPieces_Num.Text = .Item("PCS").ToString
                pices = Val(.Item("PCS").ToString)
                txtWeight_Wet.Text = Val(.Item("GRSWT").ToString)
                ObjExtraWt.txtExtraWt_WET.Text = Val(.Item("EXTRAWT").ToString)
                Weight = Val(.Item("GRSWT").ToString)
                NetWeight = Val(.Item("NETWT").ToString)
                txtLessWt.Text = .Item("LESSWT").ToString
                txtNetWeight_Wet.Text = .Item("NETWT").ToString
                txtMcPerGrm_AMT.Text = .Item("MCPERGRM").ToString
                txtMc_AMT.Text = .Item("MC").ToString
                txtSaleRate_Amt.Text = .Item("Rate").ToString
                txtWastagePer_PER.Text = .Item("WASTPER").ToString
                txtWastage_WET.Text = .Item("WASTAGE").ToString
                txtMetalRate_Amt.Text = .Item("FINRATE").ToString
                txtPacketNo__Man.Text = .Item("PACKETNO").ToString
                txtPurWastage_Wet.Text = .Item("PURWASTAGE").ToString
                txtPurRate_Amt.Text = .Item("PURRATE").ToString
                txtPurMkCharge_Amt.Text = .Item("PURMC").ToString
                cmbSubItem_Man.Text = .Item("SUBITEM").ToString
                cmbNarration_OWN.Text = .Item("NARRATION").ToString
                cmbItemType_MAN.Text = .Item("TYPENAME").ToString
                cmbCounter_MAN.Text = .Item("ITEMCTRNAME").ToString
                cmbCostCentre_MAN.Text = .Item("COSTNAME").ToString
                cmbDesigner_MAN.Text = .Item("DESIGNER").ToString
            End With
        End If
        If calType = "R" Or calType = "M" Then
            txtSaleRate_Amt.Enabled = True
            'pieceRate = Val(.Item("PieceRate").ToString)
        Else
            Select Case objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "'")
                Case "D", "T"
                    txtSaleRate_Amt.Enabled = True
                Case Else
                    txtSaleRate_Amt.Clear()
                    txtSaleRate_Amt.Enabled = False
            End Select
        End If
        strSql = " SELECT 0 KEYNO,NULL TRANTYPE"
        strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)AS ITEM"
        strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = STNSUBITEMID)AS SUBITEM"
        strSql += " ,STNPCS PCS,STNWT WEIGHT,STONEUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT"
        strSql += " ,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERe ITEMID = STNITEMID)AS METALID"
        strSql += " ,STNPCS TAGSTNPCS,STNWT TAGSTNWT,TAGSNO"
        strSql += " FROM " & cnAdminDb & "..ITEMNONTAGSTONE"
        strSql += " WHERE TAGSNO = '" & ISSsno & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(objStone.dtGridStone)
        objStone._EditLock = False
        objStone._DelLock = False
        objStone.CalcStoneWtAmount()


        'If calType = "R" Or calType = "M" Then
        '    txtSaleRate_Amt.Enabled = True
        'Else
        '    txtSaleRate_Amt.Clear()
        '    txtSaleRate_Amt.Enabled = False
        'End If

        lblPCompled.Text = Val(lblPCompled.Text) - Val(txtPieces_Num.Text)
        lblPBalance.Text = Val(lblPLot.Text) - Val(lblPCompled.Text)

        lblWCompleted.Text = Val(lblWCompleted.Text) - Val(txtWeight_Wet.Text)
        lblWBalance.Text = Format(Val(lblWLot.Text) - Val(lblWCompleted.Text), "0.000")

    End Sub
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        SNO = Nothing
        txtLotNo_Num_Man.Enabled = True
        grpPurchase.Visible = True
        objStone = New frmStoneDia
        ObjExtraWt = New frmExtaWt

        pieceRate = 0
        calType = ""
        packetNoDupFlag = False
        lblPLot.Text = ""
        lblPCompled.Text = ""
        lblPBalance.Text = ""

        lblWLot.Text = ""
        lblWCompleted.Text = ""
        lblWBalance.Text = ""
        dtpTranDate.Value = GetEntryDate(GetServerDate)
        txtLotNo_Num_Man.Focus()

    End Function
    Function funcMultyNew() As Integer
        txtPieces_Num.Clear()
        txtWeight_Wet.Clear()
        txtLessWt.Clear()
        txtNetWeight_Wet.Clear()
        txtSaleRate_Amt.Clear()
        txtPacketNo__Man.Clear()
        cmbNarration_OWN.Text = ""

        txtPurWastage_Wet.Clear()
        txtPurMkCharge_Amt.Clear()
        txtPurRate_Amt.Clear()
        If cmbSubItem_Man.Enabled = True Then
            cmbSubItem_Man.Focus()
        Else
            txtPieces_Num.Focus()
        End If
        objStone = New frmStoneDia
        ObjExtraWt = New frmExtaWt
    End Function

    Function funcSave() As Integer

        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If Not UpdateFlag Then
            If Not CheckDate(dtpTranDate.Value) Then Exit Function
            If CheckEntryDate(dtpTranDate.Value) Then Exit Function
        End If
        Dim allowzreopcs As String
        strSql = "SELECT isnull(ALLOWZEROPCS,'') ALLOWZEROPCS FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtItem_Man.Text & "'"
        allowzreopcs = GetSqlValue(cn, strSql).ToString
        If allowzreopcs = "N" Then
            If Not Val(txtPieces_Num.Text) > 0 Then
                If focusPiece Then
                    MsgBox(Me.GetNextControl(txtPieces_Num, False).Text + E0001, MsgBoxStyle.Exclamation)
                    txtPieces_Num.Select()
                    Exit Function
                End If
            End If
        End If
        If Not Val(txtWeight_Wet.Text) > 0 Then
            If calType = "W" Then
                MsgBox(Me.GetNextControl(txtWeight_Wet, False).Text + E0001, MsgBoxStyle.Information)
                txtWeight_Wet.Select()
                Exit Function
            End If
        End If
        'If Val(txtSaleRate_Amt.Text) = 0 And objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "' AND METALID IN ('D','T')", , "-1") <> "-1" Then
        '    MsgBox("Sale Rate should not empty", MsgBoxStyle.Information)
        '    txtSaleRate_Amt.Select()
        '    Exit Function
        'End If
        If Val(txtSaleRate_Amt.Text) > 0 And Val(txtPurRate_Amt.Text) > 0 And Val(txtSaleRate_Amt.Text) < Val(txtPurRate_Amt.Text) Then
            MsgBox("Sale rate should not below with purchase rate", MsgBoxStyle.Information)
            txtSaleRate_Amt.Select()
            Exit Function
        End If
        Dim dt As New DataTable
        ''CPcs Validation
        If SNO <> Nothing Then
            'strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CLOSETIME=GETDATE(),CPCS = CPCS - " & Val(pices) & " + " & Val(txtPieces_Num.Text) & ""
            'strSql += " ,CGRSWT = CGRSWT - " & Val(Weight) & " + " & Val(txtWeight_Wet.Text) & ""
            'strSql += " ,CGRSWT = CNETWT - " & Val(NetWeight) & " + " & Val(txtNetWeight_Wet.Text) & ""
            If LotCheckBy = "P" Then
                strSql = " SELECT (PCS-CPCS+" & pices & ")BALPCS FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"
                lblPBalance.Text = Val(objGPack.GetSqlValue(strSql, "BALPCS"))
                If allowzreopcs = "N" Then
                    If Not Val(lblPBalance.Text) > 0 Then
                        MsgBox(Me.GetNextControl(lblPLot, False).Text +
                    Me.GetNextControl(Me.GetNextControl(lblPLot, False), False).Text +
                    E0018, MsgBoxStyle.Exclamation)
                        txtLotNo_Num_Man.Focus()
                        Exit Function
                    End If
                End If
            ElseIf LotCheckBy = "W" Then
                strSql = " SELECT (GRSWT-CGRSWT+" & Weight & ")BALWEIGHT FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"
                lblWBalance.Text = Val(objGPack.GetSqlValue(strSql, "BALWEIGHT"))
                If Not Val(lblWBalance.Text) > 0 Then
                    MsgBox(Me.GetNextControl(lblPLot, False).Text +
                    Me.GetNextControl(Me.GetNextControl(lblPLot, False), False).Text +
                    E0018, MsgBoxStyle.Exclamation)
                    txtLotNo_Num_Man.Focus()
                    Exit Function
                End If
            ElseIf LotCheckBy = "E" Then
                strSql = " SELECT (GRSWT-CGRSWT+" & Weight & ")BALWEIGHT FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"
                lblWBalance.Text = Val(objGPack.GetSqlValue(strSql, "BALWEIGHT"))
                strSql = " SELECT (PCS-CPCS+" & pices & ")BALPCS FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"
                lblPBalance.Text = Val(objGPack.GetSqlValue(strSql, "BALPCS"))
                If Not Val(lblPBalance.Text) > 0 And Not Val(lblWBalance.Text) > 0 Then
                    MsgBox(Me.GetNextControl(lblPLot, False).Text +
                    Me.GetNextControl(Me.GetNextControl(lblPLot, False), False).Text +
                    E0018, MsgBoxStyle.Exclamation)
                    txtLotNo_Num_Man.Focus()
                    Exit Function
                End If
            End If
            ''Receipt Date Checking
            strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "' AND LOTDATE <= '" & GetEntryDate(dtpTranDate.Value, tran).ToString("yyyy-MM-dd") & "'"
            If objGPack.DupCheck(strSql) = False Then
                MsgBox(E0004 + Me.GetNextControl(dtpTranDate, False).Text, MsgBoxStyle.Information)
                dtpTranDate.Focus()
                Exit Function
            End If

        End If

        If UpdateFlag = False Then
            If txtPacketNo__Man.Enabled = True Then
                If txtPacketNo__Man.Text <> "" Then
                    Dim Msgfirst As Boolean = True
nextnos:
                    strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMNONTAG WHERE PACKETNO = '" & txtPacketNo__Man.Text & "'"
                    If objGPack.DupCheck(strSql) Then
                        packetNoDupFlag = True
                        If Msgfirst Then
                            If MessageBox.Show(Me.GetNextControl(txtPacketNo__Man, False).Text + E0002 + E0019, E0002, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Cancel Then
                                packetNoDupFlag = False
                                Msgfirst = False
                                txtPacketNo__Man.Text = Val(txtPacketNo__Man.Text) + 1
                                GoTo nextnos
                            End If
                        Else
                            txtPacketNo__Man.Text = Val(txtPacketNo__Man.Text) + 1
                            packetNoDupFlag = False
                            GoTo nextnos
                        End If
                    End If
                End If
            End If
        End If

        ''Gross Wt Validation
        If Not Val(txtWeight_Wet.Text) >= Val(txtNetWeight_Wet.Text) Then
            MsgBox(Me.GetNextControl(txtNetWeight_Wet, False).Text + E0006 + txtWeight_Wet.Text, MsgBoxStyle.Exclamation)
            txtNetWeight_Wet.Focus()
            Exit Function
        End If
        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "'"
        Dim MitemId As Integer = Val(objGPack.GetSqlValue(strSql, , , tran))
        Dim mpktno As Integer = Val(txtPacketNo__Man.Text)
        funcAdd()
        MsgBox("Successfully Saved.." & IIf(mpktno <> 0, " Packetno-" & mpktno, ""))
        If (mpktno <> 0 And GetAdmindbSoftValue("PKTNOTAGPRINT", "N") = "Y") Then
            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Dim oldItem As Integer = Nothing
            Dim memfile As String = "\Barcodeprint" & prnmemsuffix.Trim & ".mem"
            Dim write As IO.StreamWriter
            write = IO.File.CreateText(Application.StartupPath & memfile)
            write.WriteLine(LSet("PROC", 7) & ":" & MitemId)
            write.WriteLine(LSet("PKTNO", 7) & ":" & mpktno.ToString)
            write.Flush()
            write.Close()
            If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe")
            Else
                MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
            End If
        ElseIf NONTAGPRINT Then
            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Dim oldItem As Integer = Nothing
            Dim memfile As String = "\Barcodeprint" & prnmemsuffix.Trim & ".mem"
            Dim write As IO.StreamWriter
            write = IO.File.CreateText(Application.StartupPath & memfile)
            write.WriteLine(LSet("PROC", 7) & ":" & MitemId)
            write.WriteLine(LSet("SNO", 7) & ":" & NonTagSno)
            write.Flush()
            write.Close()
            If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe")
            Else
                MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
            End If
        End If
        NonTagSno = ""
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcAdd() As Integer
        Dim itemId As Integer = 0
        Dim subItemId As Integer = 0
        Dim itemCtrId As Integer = 0
        Dim COSTID As String = ""
        Dim designerId As Integer = 0
        Dim itemType As Integer = 0
        Try


            ''Find ItemId
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "'"
            itemId = Val(objGPack.GetSqlValue(strSql, , , tran))
            ''Find SubItemid
            strSql = " SELECT SUBITEMID FROM  " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' and itemid = " & itemId & ""
            subItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
            ''Find itemCtrId
            strSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_MAN.Text & "'"
            itemCtrId = Val(objGPack.GetSqlValue(strSql, , , tran))
            ''Find COSTID
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'"
            COSTID = objGPack.GetSqlValue(strSql, , , tran)
            ''Find DesignerId
            strSql = " SELECT DESIGNERID FROM  " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'"
            designerId = Val(objGPack.GetSqlValue(strSql, , , tran))
            ''Find itemType
            strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'"
            itemType = Val(objGPack.GetSqlValue(strSql, , , tran))

            Dim OLDSNO As String = ""
            strSql = "SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE NEWSNO='" & SNO & "' "
            OLDSNO = objGPack.GetSqlValue(strSql, , , tran)
            'If flagSave = 1 Then

            If UpdateFlag = False Then
                tran = cn.BeginTransaction()
                Dim tagSno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
                NonTagSno = tagSno

                strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAG"
                strSql += " ("
                strSql += " SNO,ITEMID,SUBITEMID,COMPANYID,RECDATE,"
                strSql += " PCS,GRSWT,LESSWT,NETWT,"
                strSql += " FINRATE,ISSTYPE,RECISS,POSTED,"
                strSql += " LOTNO,PACKETNO,DREFNO,ITEMCTRID,"
                strSql += " ORDREPNO,ORSNO,NARRATION,PURWASTAGE,"
                strSql += " PURRATE,PURMC,RATE,COSTID,TCOSTID,"
                strSql += " CTGRM,DESIGNERID,ITEMTYPEID,"
                strSql += " CARRYFLAG,REASON,BATCHNO,CANCEL,"
                strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER"
                strSql += " ,WASTPER,WASTAGE,MCPERGRM,MC,lotsno,EXTRAWT,STYLENO"
                strSql += " )VALUES("
                strSql += " '" & tagSno & "'" 'SNO
                strSql += " ," & itemId & "" 'ITEMID
                strSql += " ," & subItemId & "" 'SUBITEMID
                strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                strSql += " ,'" & GetEntryDate(dtpTranDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                strSql += " ," & Val(txtPieces_Num.Text) & "" 'PCS
                strSql += " ," & Val(txtWeight_Wet.Text) & "" 'GRSWT
                strSql += " ," & Val(txtWeight_Wet.Text) - Val(txtNetWeight_Wet.Text) & "" 'LESSWT
                strSql += " ," & Val(txtNetWeight_Wet.Text) & "" 'NETWT
                strSql += " ," & Val(txtMetalRate_Amt.Text) & "" 'FINRATE
                strSql += " ,''" 'ISSTYPE
                strSql += " ,'R'" 'RECISS
                strSql += " ,''" 'POSTED
                strSql += " ," & Val(txtLotNo_Num_Man.Text) & "" 'LOTNO
                strSql += " ,'" & txtPacketNo__Man.Text & "'" 'PACKETNO
                strSql += " ,0" 'DREFNO
                strSql += " ," & itemCtrId & "" 'ITEMCTRID
                strSql += " ,''" 'ORDREPNO
                strSql += " ,''" 'ORSNO
                strSql += " ,'" & cmbNarration_OWN.Text & "'" 'NARRATION
                strSql += " ," & Val(txtPurWastage_Wet.Text) & "" 'PURWASTAGE
                strSql += " ," & Val(txtPurRate_Amt.Text) & "" 'PURRATE
                strSql += " ," & Val(txtPurMkCharge_Amt.Text) & "" 'PURMC
                strSql += " ," & Val(txtSaleRate_Amt.Text) & "" 'RATE
                'commented removed by magesh current costid store in cost id col ' 19-nov

                strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                'strSql += " ,'" & COSTID & "'" 'COSTID
                strSql += " ,'" & COSTID & "'" 'TCOSTID
                If cmbStoneUnit.Enabled = True Then 'CTGRM
                    strSql += " ,'" & Mid(cmbStoneUnit.Text, 1, 1) & "'"
                Else
                    strSql += " ,''"
                End If
                strSql += " ," & designerId & "" 'DESIGNERID
                strSql += " ," & itemType & "" 'ITEMTYPEID
                strSql += " ,''" 'CARRYFLAG
                strSql += " ,'0'" 'REASON
                strSql += " ,''" 'BATCHNO
                strSql += " ,''" 'CANCEL
                strSql += " ," & userId & "" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ," & Val(txtWastagePer_PER.Text) & "" 'WASTAGEPER
                strSql += " ," & Val(txtWastage_WET.Text) & "" 'WASTAGE
                strSql += " ," & Val(txtMcPerGrm_AMT.Text) & "" 'MCGRM
                strSql += " ," & Val(txtMc_AMT.Text) & "" 'MC
                strSql += " ,'" & IIf(OLDSNO <> "", OLDSNO, SNO) & "'"
                strSql += " ,'" & Val(ObjExtraWt.txtExtraWt_WET.Text) & "'" 'EXTRA WEIGHT
                strSql += " ,'" & txtStyleno.Text & "'" 'STYLENO
                strSql += " )"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()

                ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TITEMNONTAG", , False)

                ''Inserting StoneDetail
                For Each ro As DataRow In objStone.dtGridStone.Rows
                    Dim stnItemId As Integer = 0
                    Dim stnSubItemId As Integer = 0
                    'Dim caType As String = Nothing
                    strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("ITEM").ToString & "'"
                    stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                    strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & ro.Item("SUBITEM").ToString & "' and itemid = " & stnItemId & ""
                    stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                    ''Inserting itemTagStone
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAGSTONE("
                    strSql += " SNO,RECISS,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                    strSql += " STNSUBITEMID,STNPCS,STNWT,"
                    strSql += " STNRATE,STNAMT,DESCRIP,"
                    strSql += " RECDATE,PURRATE,PURAMT,CALCMODE,"
                    strSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
                    strSql += " CARRYFLAG,COSTID,SYSTEMID,APPVER)VALUES("
                    'strSql += " '" & GetSnoFromSoftControl("NONTAGSTONESNO", cnStockDb, "ITEMNONTAGSTONE", tran) & "'" ''SNO
                    strSql += " '" & GetNewSno(TranSnoType.ITEMNONTAGSTONECODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
                    strSql += " ,'R'" ' RECISS
                    strSql += " ,'" & tagSno & "'" 'TAGSNO
                    strSql += " ,'" & itemId & "'" 'ITEMID
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
                    strSql += " ,'" & GetEntryDate(dtpTranDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,0" 'PURRATE
                    strSql += " ,0" 'PURAMT
                    strSql += " ,'" & ro.Item("CALC").ToString & "'" 'CALCMODE
                    strSql += " ,0" 'MINRATE
                    strSql += " ,0" 'SIZECODE
                    strSql += " ,'" & ro.Item("UNIT").ToString & "'" 'STONEUNIT
                    strSql += " ,NULL" 'ISSDATE

                    strSql += " ,''" 'CARRYFLAG
                    'commented removed by magesh current costid store in cost id col ' 19-nov
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    'strSql += " ,'" & COSTID & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TITEMNONTAGSTONE", , False)

                Next
                ''Updating
                'CPieces and CWt
                strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CLOSETIME=GETDATE(),CPCS = CPCS + " & Val(txtPieces_Num.Text) & ""
                strSql += " ,CGRSWT = ISNULL(CGRSWT,0) + " & Val(txtWeight_Wet.Text) & ""
                strSql += " ,CNETWT = ISNULL(CNETWT,0) + " & Val(txtNetWeight_Wet.Text) & ""
                strSql += " WHERE SNO = '" & SNO & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                If packetNoDupFlag = False Then
                    Dim billcontrolid As String = "PACKETNO"
                    strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
                    strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                    If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
                        strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET "
                        strSql += " CTLTEXT = " & Val(txtPacketNo__Man.Text) & ""
                        strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                    Else
                        strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET "
                        strSql += " CURRENTTAGNO = '" & txtPacketNo__Man.Text & "'"
                        strSql += " WHERE ITEMNAME = '" & txtItem_Man.Text & "'"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                    End If
                End If
               

                tran.Commit()

                ''Lot Pcs
                Dim comP As Integer = Val(lblPCompled.Text) + Val(txtPieces_Num.Text)
                Dim comW As Double = Val(lblWCompleted.Text) + Val(txtWeight_Wet.Text)
                lblPCompled.Text = IIf(comP <> 0, comP, "")
                lblWCompleted.Text = IIf(comW <> 0, Format(comW, "0.000"), "")

                ''Lot Wt
                Dim balW As Double = Val(lblWLot.Text) - Val(lblWCompleted.Text)
                Dim balP As Integer = Val(lblPLot.Text) - Val(lblPCompled.Text)
                lblPBalance.Text = IIf(balP <> 0, balP, "")
                lblWBalance.Text = IIf(balW <> 0, Format(balW, "0.000"), "")
                If LotCheckBy = "P" Then
                    If Val(lblPBalance.Text) = 0 Then
                        funcNew()
                    Else
                        funcMultyNew()
                    End If
                Else
                    If Val(lblWBalance.Text) = 0 Then
                        funcNew()
                    Else
                        funcMultyNew()
                    End If
                End If

            End If

            If UpdateFlag = True Then
                If SNO <> Nothing Then
                    If Val(txtPieces_Num.Text) > Val(lblPBalance.Text) Then
                        MsgBox("Pieces Exceed", MsgBoxStyle.Information)
                        txtPieces_Num.Focus()
                        Exit Function
                    End If
                    If calType = "W" Then
                        If Val(txtWeight_Wet.Text) > Val(lblWBalance.Text) Then
                            MsgBox("Weight Exceed", MsgBoxStyle.Information)
                            txtWeight_Wet.Focus()
                            Exit Function
                        End If
                    End If
                End If
                'Dim mmcostid As String = objGPack.GetSqlValue("select costid from " & cnAdminDb & "..costcentre where costname = '" & cmbCostCentre_MAN.Text & "'")
                strSql = " UPDATE " & cnAdminDb & "..ITEMNONTAG SET COSTID = '" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "',ITEMCTRID = '" & itemCtrId & "',   "
                strSql += " TCOSTID = '" & COSTID & "',ITEMTYPEID = '" & itemType & "', DESIGNERID = '" & designerId & "',SUBITEMID = '" & subItemId & "', "
                strSql += " PCS = " & Val(txtPieces_Num.Text) & ", GRSWT = " & Val(txtWeight_Wet.Text) & " , LESSWT = " & Val(txtWeight_Wet.Text) - Val(txtNetWeight_Wet.Text) & ", "
                strSql += " NETWT = " & Val(txtNetWeight_Wet.Text) & ",FINRATE = " & Val(txtMetalRate_Amt.Text) & ", "
                strSql += " RATE = " & Val(txtSaleRate_Amt.Text) & ", NARRATION = '" & cmbNarration_OWN.Text & "', "
                strSql += " WASTPER = " & Val(txtWastagePer_PER.Text) & ",WASTAGE = " & Val(txtWastage_WET.Text) & ", MCPERGRM = " & Val(txtMcPerGrm_AMT.Text) & ", MC = " & Val(txtMc_AMT.Text) & " "
                strSql += " ,PURWASTAGE = " & Val(txtPurWastage_Wet.Text) & ""
                strSql += " ,PURMC = " & Val(txtPurMkCharge_Amt.Text) & ""
                strSql += " ,PURRATE = " & Val(txtPurRate_Amt.Text) & ",EXTRAWT = '" & Val(ObjExtraWt.txtExtraWt_WET.Text) & "'"
                strSql += " WHERE SNO = '" & ISSsno1 & "' "
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)

                strSql = " DELETE FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = '" & ISSsno1 & "'"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)

                ''Inserting StoneDetail
                For Each ro As DataRow In objStone.dtGridStone.Rows
                    Dim stnItemId As Integer = 0
                    Dim stnSubItemId As Integer = 0
                    'Dim caType As String = Nothing
                    strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("ITEM").ToString & "'"
                    stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                    strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & ro.Item("SUBITEM").ToString & "' and itemid = " & stnItemId & ""
                    stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                    ''Inserting itemTagStone
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAGSTONE("
                    strSql += " SNO,RECISS,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                    strSql += " STNSUBITEMID,STNPCS,STNWT,"
                    strSql += " STNRATE,STNAMT,DESCRIP,"
                    strSql += " RECDATE,PURRATE,PURAMT,CALCMODE,"
                    strSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
                    strSql += " CARRYFLAG,COSTID,SYSTEMID,APPVER)VALUES("
                    'strSql += " '" & GetSnoFromSoftControl("NONTAGSTONESNO", cnStockDb, "ITEMNONTAGSTONE", tran) & "'" ''SNO
                    strSql += " '" & GetNewSno(TranSnoType.ITEMNONTAGSTONECODE, tran, "GET_SNO_ADMIN") & "'" ''SNO
                    strSql += " ,'R'" ' RECISS
                    strSql += " ,'" & ISSsno1 & "'" 'TAGSNO
                    strSql += " ,'" & itemId & "'" 'ITEMID
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
                    strSql += " ,'" & GetEntryDate(dtpTranDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,0" 'PURRATE
                    strSql += " ,0" 'PURAMT
                    strSql += " ,'" & ro.Item("CALC").ToString & "'" 'CALCMODE
                    strSql += " ,0" 'MINRATE
                    strSql += " ,0" 'SIZECODE
                    strSql += " ,'" & ro.Item("UNIT").ToString & "'" 'STONEUNIT
                    strSql += " ,NULL" 'ISSDATE
                    strSql += " ,''" 'CARRYFLAG
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ' cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TITEMNONTAGSTONE", , False)
                Next

                strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CLOSETIME=GETDATE(),CPCS = CPCS - " & Val(pices) & " + " & Val(txtPieces_Num.Text) & ""
                strSql += " ,CGRSWT = CGRSWT - " & Val(Weight) & " + " & Val(txtWeight_Wet.Text) & ""
                strSql += " ,CNETWT = CNETWT - " & Val(NetWeight) & " + " & Val(txtNetWeight_Wet.Text) & ""
                strSql += " WHERE SNO = '" & SNO & "'"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)

                MsgBox("Transaction Updated...!", MsgBoxStyle.Information)
                funcNew()
                Me.Close()
                Exit Function
            End If

        Catch ex As Exception
            tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcFindMetalRate() As Double
        Dim dt As New DataTable
        dt.Clear()

        strSql = " SELECT SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
        strSql += " WHERE"
        strSql += " METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY "
        strSql += "     WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "')))"
        strSql += " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY "
        strSql += "     WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "'))))"
        strSql += " AND RDATE = (SELECT TOP 1 RDATE FROM " & cnAdminDb & "..RATEMAST ORDER BY SNO DESC)"
        strSql += " AND RATEGROUP = (SELECT TOP 1 RATEGROUP FROM " & cnAdminDb & "..RATEMAST AS LR WHERE LR.RDATE = M.RDATE ORDER BY RATEGROUP DESC)"
        strSql += " ORDER BY SNO DESC"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Dim rate As Double = Nothing
        If dt.Rows.Count > 0 Then
            rate = Val(dt.Rows(0).Item("Srate").ToString)
        End If
        Return rate
    End Function
    Function funcGenPacketNo(ByVal packetNo As String) As String
        If IsNumeric(packetNo) = True Then
            packetNo = Val(packetNo) + 1
        Else
            Dim index As Integer = 0
            For Each c As Char In packetNo
                If Char.IsLetter(c) = True Then
                    index += 1
                Else
                    Exit For
                End If
            Next
            Dim fNo As String
            Dim sNo As String
            fNo = packetNo.Substring(0, index)
            sNo = packetNo.Substring(index, packetNo.Length - index)
            sNo = Val(sNo) + 1
            packetNo = fNo + sNo
        End If
        Return packetNo
    End Function
    Function funcFindPacketNo() As String
        Dim packetNo As String = Nothing
        Dim billcontrolid As String = "PACKETNO"
        strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
        strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
        If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
            packetNo = GetBillNoValue(billcontrolid, tran)
        Else
            Dim dt As New DataTable
            dt.Clear()
            strSql = " SELECT CURRENTTAGNO FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                Return ""
            End If
            packetNo = dt.Rows(0).Item("CurrentTagNo").ToString
            If packetNo = "" Then
                packetNo = 0
            End If
        End If
        Return funcGenPacketNo(packetNo)
    End Function



    Private Sub frmNonTagReceipt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtLotNo_Num_Man.Focused Then Exit Sub
            'If txtWeight_Wet.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmNonTag_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If UpdateFlag Then Exit Sub
        If LOCKWOPCRATE Then
            Grp_Wtdet.Enabled = False
            txtPurMkCharge_Amt.Enabled = False
            txtPurWastage_Wet.Enabled = False

            lblMargin.Visible = True
            txt_Margin.Visible = True
        End If
        funcNew()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub AssignLotValues()
        strSql = " SELECT T.LOTNO,L.SNO,T.RECDATE LOTDATE"
        strSql += vbCrLf + " ,IM.ITEMNAME"
        strSql += vbCrLf + " ,IC.ITEMCTRNAME DefaultCounter"
        strSql += vbCrLf + " ,IM.STOCKTYPE,IM.FOCUSPIECE,IM.PIECERATE,IM.CALTYPE,DE.DESIGNERNAME DESIGNER,CO.COSTNAME"
        strSql += vbCrLf + " ,L.ENTRYTYPE,IM.STONEUNIT,IM.METALID,IM.TAGTYPE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS T "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..ITEMCOUNTER AS IC ON IC.ITEMCTRID = T.ITEMCTRID"
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..DESIGNER AS DE ON DE.DESIGNERID = T.DESIGNERID"
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..COSTCENTRE AS CO ON CO.COSTID = T.COSTID"
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..ITEMLOT AS L ON L.SNO = T.LOTSNO"
        strSql += vbCrLf + " WHERE T.SNO = '" & ISSsno1 & "'"
        Dim dtTemp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If Not dtTemp.Rows.Count > 0 Then
            MsgBox("Edit info not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        With dtTemp.Rows(0)
            SNO = Nothing 'RESET LOTSNO
            'If calType = "R" Or calType = "M" Then
            '    txtSaleRate_Amt.Enabled = True
            '    pieceRate = Val(.Item("PieceRate").ToString)
            'Else
            '    txtSaleRate_Amt.Clear()
            '    txtSaleRate_Amt.Enabled = False
            'End If
            Dim Assorted As Boolean
            If objGPack.GetSqlValue("SELECT ISNULL(ASSORTED,'') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ItemName").ToString & "'") = "Y" Then
                Assorted = True
            End If
            If .Item("StockType").ToString = "N" Or Assorted Then
                txtPacketNo__Man.Clear()
                txtPacketNo__Man.Enabled = False
                txtWastage_WET.Clear()
                txtWastagePer_PER.Clear()
                txtMc_AMT.Clear()
                txtMcPerGrm_AMT.Clear()
                txtWastage_WET.Enabled = False
                txtWastagePer_PER.Enabled = False
                txtMc_AMT.Enabled = False
                txtMcPerGrm_AMT.Enabled = False
            Else
                If UpdateFlag = False Then
                    txtPacketNo__Man.Enabled = True
                Else
                    txtPacketNo__Man.Enabled = False
                End If
                txtWastage_WET.Enabled = True
                txtWastagePer_PER.Enabled = True
                txtMc_AMT.Enabled = True
                txtMcPerGrm_AMT.Enabled = True
                'txtSaleRate_Amt.Enabled = True
                pieceRate = Val(.Item("PieceRate").ToString)
                txtSaleRate_Amt.Text = pieceRate
            End If
            txtLotNo_Num_Man.Text = .Item("LotNo").ToString
            txtItem_Man.Text = .Item("ItemName").ToString

            dtpTranDate.Value = GetEntryDate(.Item("LotDate"))
            cmbDesigner_MAN.Text = .Item("DESIGNER").ToString
            cmbCounter_MAN.Text = .Item("DefaultCounter").ToString
            cmbCostCentre_MAN.Text = .Item("COSTNAME").ToString
            If calType = "R" Or calType = "M" Then
                txtSaleRate_Amt.Enabled = True
                pieceRate = Val(.Item("PieceRate").ToString)
            Else
                Select Case objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "'")
                    Case "D", "T"
                        txtSaleRate_Amt.Enabled = True
                    Case Else
                        txtSaleRate_Amt.Clear()
                        txtSaleRate_Amt.Enabled = False
                End Select
            End If

            strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "'")))
            'strSql = "SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.ITEMID = "
            'strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "')"
            objGPack.FillCombo(strSql, cmbSubItem_Man)
            If cmbSubItem_Man.Items.Count > 0 Then
                cmbSubItem_Man.Enabled = True
            Else
                cmbSubItem_Man.Enabled = False
            End If
            If .Item("FOCUSPIECE").ToString = "Y" Then
                focusPiece = True
            Else
                focusPiece = False
            End If
            calType = .Item("calType").ToString
            If .Item("METALID") = "D" Or .Item("METALID") = "T" Then
                cmbStoneUnit.Enabled = True
                cmbStoneUnit.Text = .Item("StoneUnit").ToString
            Else
                cmbStoneUnit.Enabled = False
            End If
            If .Item("TagType").ToString = "Y" Then ''Load itemType
                strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE ORDER BY NAME"
                objGPack.FillCombo(strSql, cmbItemType_MAN)
                cmbItemType_MAN.Enabled = True
            Else
                cmbItemType_MAN.Items.Clear()
                cmbItemType_MAN.Enabled = False
            End If

            ' ''Lot Summary

            'lblPLot.Text = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, "")
            'lblPCompled.Text = IIf(Val(.Item("CPCS").ToString) <> 0, Val(.Item("CPCS").ToString), "")
            'lblPBalance.Text = IIf(Val(.Item("BALPCS").ToString) <> 0, Val(.Item("BALPCS").ToString), "")

            'lblWLot.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, Format(Val(.Item("GRSWT").ToString), "0.000"), "")
            'lblWCompleted.Text = IIf(Val(.Item("CGRSWT").ToString) <> 0, Format(Val(.Item("CGRSWT").ToString), "0.000"), "")
            'lblWBalance.Text = IIf(Val(.Item("BALGRSWT").ToString) <> 0, Format(Val(.Item("BALGRSWT").ToString), "0.000"), "")

            If cmbStoneUnit.Enabled = False Then
                grpPurchase.Visible = True
            Else
                grpPurchase.Visible = False
            End If
            Dim rate As Double = funcFindMetalRate()
            txtMetalRate_Amt.Text = IIf(rate <> 0, Format(rate, "0.00"), "")

        End With
    End Sub

    Private Sub AssignValues()
        strSql = " SELECT "
        strSql += vbCrLf + " LOTNO,ENTRYORDER,LOTDATE"
        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID = LOT.ITEMID)AS ITEMNAME"
        strSql += vbCrLf + " ,ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = LOT.ITEMCTRID),'') AS DEFAULTCOUNTER"
        strSql += vbCrLf + " ,(SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS STOCKTYPE"
        strSql += vbCrLf + " ,(SELECT FOCUSPIECE FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS FOCUSPIECE"
        strSql += vbCrLf + " ,(SELECT PIECERATE FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS PIECERATE"
        strSql += vbCrLf + " ,(SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS CALTYPE"
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = LOT.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,(sELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = LOT.COSTID)AS COSTNAME"
        strSql += vbCrLf + " ,CASE WHEN ENTRYTYPE = 'R' THEN 'REGULAR'"
        strSql += vbCrLf + " WHEN ENTRYTYPE = 'OR' THEN 'ORDER'"
        strSql += vbCrLf + " WHEN ENTRYTYPE = 'RE' THEN 'REPAIR'"
        strSql += vbCrLf + " WHEN ENTRYTYPE = 'SR' THEN 'SALES RETURN'"
        strSql += vbCrLf + " WHEN ENTRYTYPE = 'NT' THEN 'NONTAG TO TAG' ELSE 'LOT' END AS ENTRYTYPE"
        strSql += vbCrLf + " ,PCS,GRSWT,CPCS,CGRSWT,(PCS-CPCS)AS BALPCS,(GRSWT-CGRSWT)AS BALGRSWT"
        strSql += vbCrLf + " ,(SELECT (CASE WHEN STONEUNIT = 'C' THEN 'CARAT' ELSE 'GRAM' END) FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS STONEUNIT"
        strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS METALID"
        strSql += vbCrLf + " ,(SELECT TAGTYPE FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS TAGTYPE,FINERATE PURRATE,WASTPER PURWASTPER,MCGRM PURMCGRM"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMLOT AS LOT"
        strSql += vbCrLf + " WHERE SNO = '" & SNO & "'"
        If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + " ORDER BY ITEMNAME"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            AssignLotValues()
        End If
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                calType = .Item("calType").ToString
                Dim Assorted As Boolean
                If objGPack.GetSqlValue("SELECT ISNULL(ASSORTED,'') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ItemName").ToString & "'") = "Y" Then
                    Assorted = True
                End If
                If .Item("StockType").ToString = "N" Or Assorted Then
                    txtPacketNo__Man.Clear()
                    txtPacketNo__Man.Enabled = False
                    txtWastage_WET.Clear()
                    txtWastagePer_PER.Clear()
                    txtMc_AMT.Clear()
                    txtMcPerGrm_AMT.Clear()
                    txtWastage_WET.Enabled = False
                    txtWastagePer_PER.Enabled = False
                    txtMc_AMT.Enabled = False
                    txtMcPerGrm_AMT.Enabled = False
                Else
                    If UpdateFlag = False Then
                        txtPacketNo__Man.Enabled = True
                    Else
                        txtPacketNo__Man.Enabled = False
                    End If
                    txtWastage_WET.Enabled = True
                    txtWastagePer_PER.Enabled = True
                    txtMc_AMT.Enabled = True
                    txtMcPerGrm_AMT.Enabled = True
                    'txtSaleRate_Amt.Enabled = True
                    pieceRate = Val(.Item("PieceRate").ToString)
                End If
                txtLotNo_Num_Man.Text = .Item("LotNo").ToString
                txtItem_Man.Text = .Item("ItemName").ToString
                strSql = "SELECT ASSORTED FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtItem_Man.Text & "'"
                If objGPack.GetSqlValue(strSql, "ASSORTED", "Y").ToString = "Y" Then
                    lblItemChange.Visible = True
                End If
                If calType = "R" Or calType = "M" Then
                    txtSaleRate_Amt.Enabled = True
                    pieceRate = Val(.Item("PieceRate").ToString)
                Else
                    Select Case objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "'")
                        Case "D", "T"
                            txtSaleRate_Amt.Enabled = True
                            pieceRate = Val(.Item("PieceRate").ToString)
                        Case Else
                            txtSaleRate_Amt.Clear()
                            txtSaleRate_Amt.Enabled = False
                    End Select
                End If
                dtpTranDate.Value = GetEntryDate(.Item("LotDate"))
                cmbDesigner_MAN.Text = .Item("DESIGNER").ToString
                cmbCounter_MAN.Text = .Item("DefaultCounter").ToString
                cmbCostCentre_MAN.Text = .Item("COSTNAME").ToString

                Dim iId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"))
                mzitemid = iId
                strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, iId)
                'strSql = "SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.ITEMID = "
                'strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "')"
                objGPack.FillCombo(strSql, cmbSubItem_Man)
                If cmbSubItem_Man.Items.Count > 0 Then
                    cmbSubItem_Man.Enabled = True
                Else
                    cmbSubItem_Man.Enabled = False
                End If
                If .Item("FOCUSPIECE").ToString = "Y" Then
                    focusPiece = True
                Else
                    focusPiece = False
                End If
                If .Item("METALID") = "D" Or .Item("METALID") = "T" Then
                    cmbStoneUnit.Enabled = True
                    cmbStoneUnit.Text = .Item("StoneUnit").ToString
                Else
                    cmbStoneUnit.Enabled = False
                End If
                If .Item("TagType").ToString = "Y" Then ''Load itemType
                    strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE ORDER BY NAME"
                    objGPack.FillCombo(strSql, cmbItemType_MAN)
                    cmbItemType_MAN.Enabled = True
                Else
                    cmbItemType_MAN.Items.Clear()
                    cmbItemType_MAN.Enabled = False
                End If

                ''Lot Summary
                lblPLot.Text = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, "")
                lblPCompled.Text = IIf(Val(.Item("CPCS").ToString) <> 0, Val(.Item("CPCS").ToString), "")
                lblPBalance.Text = IIf(Val(.Item("BALPCS").ToString) <> 0, Val(.Item("BALPCS").ToString), "")

                lblWLot.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, Format(Val(.Item("GRSWT").ToString), "0.000"), "")
                lblWCompleted.Text = IIf(Val(.Item("CGRSWT").ToString) <> 0, Format(Val(.Item("CGRSWT").ToString), "0.000"), "")
                lblWBalance.Text = IIf(Val(.Item("BALGRSWT").ToString) <> 0, Format(Val(.Item("BALGRSWT").ToString), "0.000"), "")
                txtPurRate_Amt.Text = IIf(Val(.Item("PURRATE").ToString) <> 0, Format(Val(.Item("PURRATE").ToString), "0.00"), "")
                txtPurWastage_Wet.Text = IIf(Val(.Item("PURWASTPER").ToString) <> 0, Format(Val(.Item("PURWASTPER").ToString), "0.000"), "")
                txtPurMkCharge_Amt.Text = IIf(Val(.Item("PURMCGRM").ToString) <> 0, Format(Val(.Item("PURMCGRM").ToString), "0.00"), "")
            End With
            
            If cmbStoneUnit.Enabled = False Then
                grpPurchase.Visible = True
            Else
                grpPurchase.Visible = False
            End If
            Dim rate As Double = funcFindMetalRate()
            txtMetalRate_Amt.Text = IIf(rate <> 0, Format(rate, "0.00"), "")
        End If
    End Sub
    Private Sub txtLotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLotNo_Num_Man.KeyDown
        SNO = Nothing
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT SNO,"
            strSql += " LOTNO"
            strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID = LOT.ITEMID)AS ITEM"
            strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID = LOT.SUBITEMID),'')AS SUBITEM"
            strSql += " ,(SELECT (CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END) FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS STOCKTYPE"
            strSql += " ,PCS,GRSWT,CPCS AS ""COMPLETED PCS"",CGRSWT AS ""COMPLETED GRSWT"""
            strSql += " ,(PCS-CPCS)AS BALPCS,(GRSWT-CGRSWT)AS BALGRSWT"
            strSql += " FROM " & cnAdminDb & "..ITEMLOT AS LOT"
            strSql += " WHERE 1=1 "
            If LotCheckBy = "P" Then
                strSql += " AND PCS > CPCS "
            ElseIf LotCheckBy = "E" Then
                strSql += vbCrLf + " AND ((GRSWT > CGRSWT) OR ( PCS > CPCS))"
            Else
                strSql += " AND GRSWT > CGRSWT"
            End If
            strSql += " AND ISNULL(COMPLETED,'') <> 'Y'"
            strSql += " AND ISNULL(CANCEL,'') <> 'Y'"
            strSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE (STOCKTYPE <> 'T' OR ASSORTED='Y'))"
            If txtLotNo_Num_Man.Text <> "" Then
                strSql += " AND LOTNO = '" & txtLotNo_Num_Man.Text & "'" 'LIKE
            End If
            If SPECIFICFORMAT.ToString = "1" Then
                strSql += " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COMPANYID LIKE '%" & strCompanyId & "%')"
                strSql += " AND LOT.COMPANYID='" & strCompanyId & "'"
            End If
            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            strSql += " ORDER BY LOTNO "
            SNO = BrighttechPack.SearchDialog.Show("Finding LotNo", strSql, cn, 1)
            AssignValues()
            If SNO.Length > 0 Then
                txtLotNo_Num_Man.Enabled = False
                Me.SelectNextControl(txtLotNo_Num_Man, True, True, True, True)
            End If
        End If
    End Sub
    Private Sub txtLotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtLotNo_Num_Man.KeyPress
        If txtLotNo_Num_Man.Text = "" Then
            Exit Sub
        End If
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT SNO,"
            strSql += " LOTNO"
            strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID = LOT.ITEMID)AS ITEM"
            strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID = LOT.SUBITEMID),'')AS SUBITEM"
            strSql += " ,(SELECT (CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END) FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS STOCKTYPE"
            strSql += " ,PCS,GRSWT,CPCS AS ""COMPLETED PCS"",CGRSWT AS ""COMPLETED GRSWT"""
            strSql += " ,(PCS-CPCS)AS BALPCS,(GRSWT-CGRSWT)AS BALGRSWT"
            strSql += " FROM " & cnAdminDb & "..ITEMLOT AS LOT"
            strSql += " WHERE 1=1 "
            If LotCheckBy = "P" Then
                strSql += " AND PCS > CPCS "
            ElseIf LotCheckBy = "E" Then
                strSql += vbCrLf + " AND ((GRSWT > CGRSWT) OR ( PCS > CPCS))"
            Else
                strSql += " AND GRSWT > CGRSWT"
            End If
            strSql += " AND ISNULL(COMPLETED,'') <> 'Y'"
            strSql += " AND ISNULL(LOT.CANCEL,'') <> 'Y'"
            strSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE (STOCKTYPE <> 'T' OR ASSORTED='Y'))"
            If SPECIFICFORMAT.ToString = "1" Then
                strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COMPANYID LIKE '%" & strCompanyId & "%')"
                strSql += vbCrLf + " AND LOT.COMPANYID='" & strCompanyId & "'"
            End If
            If txtLotNo_Num_Man.Text <> "" Then
                strSql += " AND LOTNO LIKE '" & txtLotNo_Num_Man.Text & "'"
            End If
            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            strSql += " ORDER BY LOTNO "
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows.Count = 1 Then
                    SNO = dt.Rows(0).Item("SNO").ToString
                    AssignValues()
                Else 'MORE THAN ONE 
                    SNO = BrighttechPack.SearchDialog.Show("Finding LotNo", strSql, cn, 1)
                    AssignValues()
                End If
                If SNO.Length > 0 Then
                    txtLotNo_Num_Man.Enabled = False
                    Me.SelectNextControl(txtLotNo_Num_Man, True, True, True, True)
                Else
                    MsgBox(E0004 + Me.GetNextControl(txtLotNo_Num_Man, False).Text, MsgBoxStyle.Information)
                End If
            Else
                MsgBox(E0004 + Me.GetNextControl(txtLotNo_Num_Man, False).Text, MsgBoxStyle.Information)
            End If

            strSql = "SELECT B.ITEMID,B.ALLOWZEROPCS FROM " & cnAdminDb & "..ITEMLOT A," & cnAdminDb & "..ITEMMAST B  WHERE LOTNO='" & txtLotNo_Num_Man.Text & "' AND A.ITEMID =B.ITEMID"
            Dim dta As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dta)
            If dta.Rows.Count > 0 Then
                If dta.Rows.Item(0).Item("ALLOWZEROPCS").ToString = "Y" Then
                    txtPieces_Num.Text = "0"
                Else
                    txtPieces_Num.Text = ""
                End If
            End If
        End If
    End Sub

    Private Sub txtPieces_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPieces_Num.GotFocus
        If LOTPCSWT = False Then Exit Sub
        If Val(txtPieces_Num.Text) = 0 Then
            txtPieces_Num.Text = lblPBalance.Text
        End If
        strSql = "SELECT FOCUSPIECE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtItem_Man.Text & "'"
        Dim dta As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dta)
        If dta.Rows.Count > 0 Then
            If dta.Rows.Item(0).Item("FOCUSPIECE").ToString = "N" Then
                txtWeight_Wet.Focus()
            End If
        End If
    End Sub

    Private Sub txtSaleRate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSaleRate_Amt.GotFocus
        If UpdateFlag = False And Val(txtSaleRate_Amt.Text) = 0 Then
            txtSaleRate_Amt.Text = IIf(pieceRate <> 0, Format(pieceRate, "0.00"), "")
        End If
    End Sub

    Private Sub txtNetWeight_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNetWeight_Wet.GotFocus
        If UpdateFlag = False Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtNetWeight_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNetWeight_Wet.LostFocus
        If Val(txtNetWeight_Wet.Text) > Val(txtWeight_Wet.Text) Then
            MsgBox("Net Weight Should not Exceed Gross Weight", MsgBoxStyle.Exclamation)
            txtNetWeight_Wet.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub txtPacketNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPacketNo__Man.GotFocus
        txtPacketNo__Man.Text = funcFindPacketNo()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub
    Private Sub SaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub
    Private Sub VeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VeToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub txtWeight_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWeight_Wet.GotFocus
        If LOTPCSWT = False Then Exit Sub
        GETIDS()
        If Val(txtWeight_Wet.Text) = 0 Then
            If cmbSubItem_Man.Text = "" Then
                strSql = " SELECT GRSWT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "'"
            Else
                strSql = " SELECT GRSWT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "')"
            End If
            Dim defWt As Double = Val(txtPieces_Num.Text) * Val(objGPack.GetSqlValue(strSql))
            txtWeight_Wet.Text = IIf(defWt > 0, Format(defWt, "0.000"), "")
        End If

        If Val(txtWeight_Wet.Text) = 0 Then
            txtWeight_Wet.Text = lblWBalance.Text
        End If
    End Sub

    Private Sub txtWeight_Wet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtWeight_Wet.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not Val(txtWeight_Wet.Text) > 0 Then
                If calType = "W" Then
                    MsgBox(Me.GetNextControl(txtWeight_Wet, False).Text + E0001, MsgBoxStyle.Information)
                    txtWeight_Wet.Select()
                    Exit Sub
                End If
            End If
            If objGPack.GetSqlValue("SELECT EXTRAWT FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & txtItem_Man.Text & "'", "EXTRAWT", "N") = "Y" Then
                ObjExtraWt.txtExtraWt_WET.Focus()
                ObjExtraWt.ShowDialog()
            End If
            ShowStoneDia()
        End If
    End Sub

    Private Sub CalcNetWt()
        Dim netWT As Double = Nothing
        netWT = Val(txtWeight_Wet.Text) - Val(txtLessWt.Text)
        txtNetWeight_Wet.Text = IIf(netWT <> 0, Format(netWT, "0.000"), "")
        Call Getmcwastagetable(mzdesignid, mzitemid, mzsubid, netWT, mztypeid)
    End Sub

    Private Sub txtWeight_Wet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWeight_Wet.TextChanged
        CalcNetWt()
    End Sub

    Private Sub txtPieces_Num_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPieces_Num.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
           

            strSql = "SELECT isnull(ALLOWZEROPCS,'N') ALLOWZEROPCS FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtItem_Man.Text & "'"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows.Item(0).Item("ALLOWZEROPCS").ToString = "Y" Then
                    'CALNO 240113
                    'txtPieces_Num.Text = "0"
                    SendKeys.Send("{TAB}")
                    'txtWastage_WET.Focus()
                Else
                    If Not Val(txtPieces_Num.Text) > 0 Then
                        If focusPiece Then
                            MsgBox(Me.GetNextControl(txtPieces_Num, False).Text + E0001, MsgBoxStyle.Exclamation)
                            txtPieces_Num.Select()
                        End If
                    End If
                    If txtPieces_Num.Text = "0" Or txtPieces_Num.Text = "" Then
                        txtPieces_Num.Focus()
                    Else
                        e.Handled = False
                        'SendKeys.Send("{TAB}")
                        '   txtWastage_WET.Focus()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub ShowStoneDia()
        If objStone.Visible Then Exit Sub
        If cmbSubItem_Man.Enabled Then
            strSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' and itemid = (select itemid from " & cnAdminDb & "..itemmast where itemname = '" & txtItem_Man.Text & "')"
        Else
            strSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "'"
        End If
        If objGPack.GetSqlValue(strSql).ToUpper <> "Y" Then
            Me.SelectNextControl(txtWeight_Wet, True, True, True, True)
            Exit Sub
        End If
        objStone.grsWt = Val(txtWeight_Wet.Text)
        objStone._EditLock = False
        objStone._DelLock = False
        objStone.BackColor = Me.BackColor
        objStone.StartPosition = FormStartPosition.CenterScreen
        objStone.MaximizeBox = False
        objStone.grpStone.BackgroundColor = Me.BackColor
        objStone.StyleGridStone(objStone.gridStone)
        objStone.txtStItem.Select()
        objStone.ShowDialog()
        Dim stnWt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
        Dim stnAmt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtLessWt.Text = IIf(stnWt <> 0, Format(stnWt, "0.000"), Nothing)
        Me.SelectNextControl(txtWeight_Wet, True, True, True, True)
    End Sub

    Private Sub txtLessWt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLessWt.TextChanged
        CalcNetWt()
    End Sub

    Private Sub txtMcPerGrm_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMcPerGrm_AMT.GotFocus
        If Val(txtMc_AMT.Text) > 0 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtMc_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMc_AMT.GotFocus
        If Val(txtMcPerGrm_AMT.Text) > 0 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtWastage_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWastage_WET.GotFocus
        If Val(txtWastagePer_PER.Text) > 0 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtWastagePer_PER_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWastagePer_PER.GotFocus
        If UpdateFlag = False Then
            If Val(txtWastage_WET.Text) > 0 Then SendKeys.Send("{TAB}")
        End If
    End Sub





    Private Sub UpdateGotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtLotNo_Num_Man.GotFocus _
    , dtpTranDate.GotFocus _
    , txtItem_Man.GotFocus _
    , txtMetalRate_Amt.GotFocus

        If UpdateFlag Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtSaleRate_Amt_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSaleRate_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'If Val(txtSaleRate_Amt.Text) = 0 And objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "' AND METALID IN ('D','T')", , "-1") <> "-1" Then
            '    MsgBox("Sale Rate should not empty", MsgBoxStyle.Information)
            '    txtSaleRate_Amt.Select()
            '    Exit Sub
            'ElseIf Val(txtSaleRate_Amt.Text) = 0 Then
            '    If MessageBox.Show("Sale Rate should not empty", "Sale Rate Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            '        txtSaleRate_Amt.Select()
            '        Exit Sub
            '    End If
            'End If
            If Val(txtSaleRate_Amt.Text) > 0 And Val(txtPurRate_Amt.Text) > 0 And Val(txtSaleRate_Amt.Text) < Val(txtPurRate_Amt.Text) Then
                MsgBox("Sale rate should not below with purchase rate", MsgBoxStyle.Information)
                txtSaleRate_Amt.Select()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtItem_Man_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItem_Man.KeyDown
        If e.KeyCode = Keys.F5 Then
            strSql = "SELECT ASSORTED FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtItem_Man.Text & "'"
            If objGPack.GetSqlValue(strSql, "ASSORTED", "Y").ToString = "Y" Then
                strSql = " SELECT"
                strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME"
                strSql += " FROM " & cnAdminDb & "..ITEMMAST"
                strSql += " WHERE ISNULL(STOCKTYPE,'') = 'N' "
                strSql += GetItemQryFilteration()
                txtItem_Man.Text = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, 1, , )
            End If
        End If
    End Sub

    Private Sub txtItem_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItem_Man.KeyPress
        e.Handled = True
    End Sub

    Private Sub txtItem_Man_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItem_Man.TextChanged
        'If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "' AND METALID IN ('D','T')", , "-1") = "-1" Then
        '    txtSaleRate_Amt.Clear()
        '    txtSaleRate_Amt.Enabled = False
        'Else
        '    txtSaleRate_Amt.Enabled = True
        'End If
    End Sub



    Private Sub cmbSubItem_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubItem_Man.KeyPress

    End Sub

    Public Function Getmcwastagetable(ByVal designer As Integer, ByVal items As Integer, ByVal subitems As Integer, ByVal wtt As Decimal, Optional ByVal itemtypes As Integer = 0)
        Dim strsql As String
        Dim ii As Integer = 0
nextcond:
        If ii = 4 Then Exit Function
        If ii = 1 Then designer = 0
        If ii = 2 Then designer = 0 : subitems = 0
        If ii = 3 Then designer = 0 : subitems = 0 : items = 0
        strsql = " SELECT  MAXWASTPER, MAXMCGRM, MAXWAST, MAXMC, MINWASTPER, MINMCGRM, MINWAST, MINMC, TOUCH, MAXWASTPER_PUR, MAXMCGRM_PUR, MAXWAST_PUR, MAXMC_PUR, MINWASTPER_PUR,MINMCGRM_PUR, MINWAST_PUR, MINMC_PUR, TOUCH_PUR FROM " & cnAdminDb & "..WMCTABLE WHERE "
        strsql += "ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        strsql += " AND ITEMID = " & items & " AND SUBITEMID = " & subitems & " AND ITEMTYPE =" & itemtypes & " AND DESIGNERID = " & designer
        strsql += " AND fromweight <= " & wtt & " and toweight >= " & wtt
        Dim drow As DataRow = GetSqlRow(strsql, cn)
        If drow Is Nothing Then
            ii = ii + 1
            GoTo nextcond
        Else
            txtWastagePer_PER.Text = drow.Item("maxwastper").ToString : txtMcPerGrm_AMT.Text = drow.Item("maxmcgrm").ToString
            txtWastage_WET.Text = drow.Item("maxwast").ToString : txtMc_AMT.Text = drow.Item("maxmc").ToString
        End If

    End Function
    Function GETIDS()
        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem_Man.Text & "'"
        mzitemid = Val(objGPack.GetSqlValue(strSql, , , tran))

        strSql = " SELECT SUBITEMID FROM  " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' and itemid = " & mzitemid & ""
        mzsubid = Val(objGPack.GetSqlValue(strSql, , , tran))
        strSql = " SELECT DESIGNERID FROM  " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'"
        mzdesignid = Val(objGPack.GetSqlValue(strSql, , , tran))
        strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'"
        mztypeid = Val(objGPack.GetSqlValue(strSql, , , tran))
    End Function

    Private Sub txtPieces_Num_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPieces_Num.TextChanged

    End Sub

    Private Sub txt_Margin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Margin.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If LOCKWOPCRATE Then
                If Val(txtPurRate_Amt.Text) > 0 And Val(txt_Margin.Text) > 0 Then
                    Dim selrate As Decimal
                    selrate = Val(txtPurRate_Amt.Text) + ((Val(txtPurRate_Amt.Text) / 100) * Val(txt_Margin.Text))
                    txtSaleRate_Amt.Text = IIf(selrate <> 0, Format(selrate, "0.00"), Nothing)
                End If
            End If
        End If
    End Sub
End Class