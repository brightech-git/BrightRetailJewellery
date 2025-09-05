Imports System.Data.OleDb
Public Class frmNonTagIssue
    Dim strSql As String
    Dim da As OleDbDataAdapter

    Dim cmd As OleDbCommand

    Dim tran As OleDbTransaction

    Dim dReader As OleDbDataReader
    Dim objStone As frmStoneDia

    Dim noOfPiece As Integer = 1
    Dim pieceRate As Double = 0

    Dim calType As String
    Dim ObjExtraWt As New frmExtaWt

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre, True, True)
        Else
            cmbCostCentre.Enabled = False
        End If
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner)
        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ITEMCOUNTER'")) = "Y" Then
            strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY DISPLAYORDER,ITEMCTRNAME"
            objGPack.FillCombo(strSql, cmbCounter)
            cmbCounter.Enabled = True
        Else
            cmbCounter.Enabled = False
        End If

        strSql = " SELECT NARRATION FROM " & cnAdminDb & "..NARRATION WHERE MODULEID IN ('S')"
        objGPack.FillCombo(strSql, cmbNarration_OWN, , False)
    End Sub

    Function funcNew() As Integer
        objGPack.TextClear(Me)
        objStone = New frmStoneDia
        ObjExtraWt = New frmExtaWt
        dtpTranDate.Value = GetEntryDate(GetServerDate(tran), tran)
        cmbCostCentre.Text = ""
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre, True, True)
        Else
            cmbCostCentre.Enabled = False
        End If
        cmbDesigner.Text = ""
        cmbCounter.Text = ""
        cmbNarration_OWN.Text = ""
        dtpTranDate.Focus()
    End Function

    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If Not CheckDate(dtpTrandate.Value) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If CheckEntryDate(dtpTrandate.Value) Then Exit Function
        If calType <> "R" And (Not Val(txtWeight_Wet.Text) > 0) Then
            MsgBox("Grs Weight Should not Empty", MsgBoxStyle.Information)
            txtWeight_Wet.Focus()
        End If
        If txtPacketNo__Man.Enabled = True Then
            If funcCheckPacketNo() = True Then
                Exit Function
            End If
        End If
        Dim allowzreopcs As String
        strSql = "SELECT isnull(ALLOWZEROPCS,'N') ALLOWZEROPCS  FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtItem__Man.Text & "'"
        allowzreopcs = GetSqlValue(cn, strSql)
        If allowzreopcs = "N" Then
            If Not Val(txtPieces_Num_Man.Text) > 0 Then
                MsgBox(Me.GetNextControl(txtPieces_Num_Man, False).Text + E0001, MsgBoxStyle.Exclamation)
                txtPieces_Num_Man.Focus()
                Exit Function
            End If
        End If
        strSql = " SELECT "
        strSql += " SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1 * PCS END)AS BALPCS"
        strSql += " FROM " & cnAdminDb & "..ITEMNONTAG WHERE ITEMID = "
        strSql += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem__Man.Text & "')"
        strSql += " AND ISNULL(CANCEL,'') = ''"
        If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Dim balPcs As Integer = Val(dt.Rows(0).Item("BalPcs").ToString)
            If Val(txtPieces_Num_Man.Text) > balPcs Then
                MsgBox("Invalid Bal Piece(s)" + vbCrLf + "Balance Receipt Pieces " & balPcs & "", MsgBoxStyle.Exclamation)
                txtPieces_Num_Man.Focus()
                Return 0
            End If
        Else
            MsgBox("Invalid Bal Pieces1", MsgBoxStyle.Exclamation)
            txtPieces_Num_Man.Focus()
            Return 0
        End If
        funcAdd()

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
            tran = cn.BeginTransaction()
            ''Find ItemId
            strSql = " Select ItemId from " & cnAdminDb & "..itemMast where ItemName = '" & txtItem__Man.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            dReader = cmd.ExecuteReader
            If dReader.Read = True Then
                itemId = dReader.Item("ItemId")
            End If
            dReader.Close()
            ''Find SubItemid
            strSql = " Select SubItemId from  " & cnAdminDb & "..SubItemMast where SubItemName = '" & cmbSubItem.Text & "' AND ITEMID = " & itemId & ""
            cmd = New OleDbCommand(strSql, cn, tran)
            dReader = cmd.ExecuteReader
            If dReader.Read = True Then
                subItemId = dReader.Item("SubItemId")
            End If
            dReader.Close()
            ''Find itemCtrId
            strSql = " Select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName = '" & cmbCounter.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            dReader = cmd.ExecuteReader
            If dReader.Read = True Then
                itemCtrId = dReader.Item("ItemCtrId")
            End If
            dReader.Close()
            ''Find COSTID
            strSql = " Select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            dReader = cmd.ExecuteReader
            If dReader.Read = True Then
                COSTID = dReader.Item("CostId")
            End If
            dReader.Close()
            ''Find DesignerId
            strSql = " Select DesignerId from  " & cnAdminDb & "..Designer where DesignerName = '" & cmbDesigner.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            dReader = cmd.ExecuteReader
            If dReader.Read = True Then
                designerId = dReader.Item("DesignerId")
            End If
            dReader.Close()
            ''Find itemType
            strSql = " Select itemTypeId from " & cnAdminDb & "..itemType where Name = '" & cmbItemType.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            dReader = cmd.ExecuteReader
            If dReader.Read = True Then
                itemType = dReader.Item("itemTypeId")
            End If
            dReader.Close()

            Dim tagSno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
            strSql = " Insert into " & cnAdminDb & "..ITEMNONTAG"
            strSql += " ("
            strSql += " SNO,ItemId,SubItemId,Companyid,Recdate,"
            strSql += " Pcs,GrsWt,LessWt,NetWt,"
            strSql += " FinRate,Isstype,RecIss,Posted,"
            strSql += " Packetno,DRefno,ItemCtrId,"
            strSql += " OrdRepNo,ORSNO,Narration,"
            strSql += " Rate,COSTID,"
            strSql += " CtGrm,DesignerId,ItemTypeID,"
            strSql += " Carryflag,Reason,BatchNo,Cancel,"
            strSql += " UserId,Updated,Uptime,SystemId,APPVER,EXTRAWT,TCOSTID)Values("
            strSql += " '" & tagsno & "'" 'SNO
            strSql += " ," & itemId & "" 'ItemId
            strSql += " ," & subItemId & "" 'SubItemId
            strSql += " ,'" & GetStockCompId() & "'" 'Companyid
            strSql += " ,'" & GetEntryDate(dtpTranDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'Recdate
            strSql += " ," & Val(txtPieces_Num_Man.Text) & "" 'Pcs
            strSql += " ," & Val(txtWeight_Wet.Text) & "" 'GrsWt
            strSql += " ," & Val(txtWeight_Wet.Text) - Val(txtNetWeight.Text) & "" 'LessWt
            strSql += " ," & Val(txtNetWeight.Text) & "" 'NetWt
            strSql += " ," & Val(txtMetalRate_Amt.Text) & "" 'FinRate
            strSql += " ,''" 'Isstype
            strSql += " ,'I'" 'RecIss
            strSql += " ,''" 'Posted
            strSql += " ,'" & txtPacketNo__Man.Text & "'" 'Packetno
            strSql += " ,0" 'DRefno
            strSql += " ," & itemCtrId & "" 'ItemCtrId
            strSql += " ,''" 'OrdRepNo
            strSql += " ,''" 'ORSNO
            strSql += " ,'" & cmbNarration_OWN.Text & "'" 'Narration
            strSql += " ," & Val(txtSaleRate_Amt.Text) & "" 'Rate
            strSql += " ,'" & COSTID & "'" 'COSTID
            If cmbStoneUnit.Enabled = True Then 'CtGrm
                strSql += " ,'" & Mid(cmbStoneUnit.Text, 1, 1) & "'"
            Else
                strSql += " ,''"
            End If
            strSql += " ," & designerId & "" 'DesignerId
            strSql += " ," & itemType & "" 'ItemTypeID
            strSql += " ,''" 'Carryflag
            strSql += " ,'0'" 'Reason
            strSql += " ,''" 'BatchNo
            strSql += " ,''" 'Cancel
            strSql += " ," & UserId & "" 'UserId
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
            strSql += " ,'" & systemId & "'" 'Systemid
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " ,'" & Val(ObjExtraWt.txtExtraWt_WET.Text) & "'" 'EXTRAWT
            strSql += " ,'" & COSTID & "'" 'tCOSTID
            strSql += " )"
            ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)

            ''Inserting StoneDetail
            For Each ro As DataRow In objStone.dtGridStone.Rows
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
                strSql += " CARRYFLAG,COSTID,SYSTEMID,APPVER)VALUES("
                strSql += " '" & GetNewSno(TranSnoType.ITEMNONTAGSTONECODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
                strSql += " ,'I'" ' RECISS
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
                strSql += " ,'" & COSTID & "'" 'COSTID
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " )"
                ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TITEMNONTAGSTONE")
            Next
            tran.Commit()
            funcMultyNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcMultyNew() As Integer
        txtLessWt.Clear()
        txtPieces_Num_Man.Clear()
        txtWeight_Wet.Clear()
        txtNetWeight.Clear()
        txtSaleRate_Amt.Clear()
        txtPacketNo__Man.Clear()
        objStone = New frmStoneDia
        ObjExtraWt = New frmExtaWt
        cmbNarration_OWN.Text = ""
        If cmbSubItem.Enabled = True Then
            cmbSubItem.Focus()
        Else
            txtPieces_Num_Man.Focus()
        End If
    End Function
    Function funcFindMetalRate() As Double
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
        strSql += " WHERE"
        strSql += " RDATE = '" & dtpTrandate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += " AND RATEGROUP = (SELECT TOP 1 RATEGROUP FROM " & cnAdminDb & "..RATEMAST AS LR WHERE LR.RDATE = M.RDATE ORDER BY RATEGROUP DESC)"
        strSql += " AND METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY "
        strSql += "     WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem__Man.Text & "')))"
        strSql += " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY "
        strSql += "     WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem__Man.Text & "'))))"
        strSql += " ORDER BY SNO DESC"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Dim rate As Double = Nothing
        If dt.Rows.Count > 0 Then
            rate = Val(dt.Rows(0).Item("Srate").ToString)
        End If
        Return rate
    End Function
    Function funcCheckPacketNo() As Boolean
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMNONTAG WHERE "
        strSql += " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem__Man.Text & "')"
        strSql += " AND PACKETNO = '" & txtPacketNo__Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            MsgBox(E0004 + Me.GetNextControl(txtPacketNo__Man, False).Text, MsgBoxStyle.Exclamation)
            txtPacketNo__Man.Focus()
            Return True
        End If
        Return False
    End Function

    Private Sub frmNonTagIssue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtWeight_Wet.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmNonTagIssue_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ''cmbValidation
        Dim dt As New DataTable
        ''CostCentre Checking..
        'strSql = " select 1 from " & cnAdminDb & "..SoftControl where ctlId = 'COSTCENTRE' and ctlText = 'Y'"
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dt)
        'If Not dt.Rows.Count > 0 Then
        '    cmbCostCentre.Items.Clear()
        '    cmbCostCentre.Enabled = False
        'End If
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre, True, True)
        Else
            cmbCostCentre.Enabled = False
        End If
        strSql = " SELECT NARRATION FROM " & cnAdminDb & "..NARRATION WHERE MODULEID = 'S'"
        objGPack.FillCombo(strSql, cmbNarration_OWN, , False)
        cmbStoneUnit.Items.Add("CARAT")
        cmbStoneUnit.Items.Add("GRAM")
        funcNew()
    End Sub

    Private Sub txtItem__Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItem__Man.GotFocus
        Main.ShowHelpText("Press Insert to Help")
    End Sub
    Private Sub txtItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItem__Man.KeyDown
            If e.KeyCode = Keys.Insert Then
            strSql = " SELECT "
            strSql += " DISTINCT(NT.ITEMID),ITEMNAME,"
            strSql += " CASE WHEN STOCKTYPE = 'N' THEN 'NON TAG' ELSE 'PACKET BASED' END AS STOCKTYPE,"
            strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT' "
            strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
            strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
            strSql += " WHEN CALTYPE = 'B' THEN 'BOTH'"
            strSql += " WHEN CALTYPE = 'M' THEN 'METAL RATE' END AS CALTYPE,"
            strSql += " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = IM.METALID)AS METALNAME"
            strSql += " FROM " & cnAdminDb & "..ITEMNONTAG AS NT"
            strSql += " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON NT.ITEMID = IM.ITEMID"
            strSql += " WHERE STOCKTYPE <> 'T'"
            strSql += " AND ISNULL(CANCEL,'') = '' "
            strSql += " AND IM.ACTIVE = 'Y'"
            If Not cnCentStock Then strSql += GetItemQryFilteration("S", "IM")
            If _HideBackOffice Then strSql += " AND ISNULL(NT.FLAG,'') <> 'B'"
            'If Not cnCentStock Then strSql += " AND IM.COMPANYID = '" & GetStockCompId() & "'"
            strSql += " AND NT.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMNONTAG WHERE ISNULL(CANCEL,'') = '' GROUP BY ITEMID HAVING "
            strSql += " SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1 * PCS END) > 0)"
            strSql += " ORDER BY ITEMNAME"
            txtItem__Man.Text = BrighttechPack.SearchDialog.Show("Finding ItemName", strSql, cn, 1, 1, BrighttechPack.frmSearch.GridStyle.DefaultStyle)
            txtItem__Man.SelectAll()
        End If
    End Sub
    Private Sub txtItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItem__Man.KeyPress
        Dim dt As New DataTable
        dt.Clear()
        If txtItem__Man.Text = "" Then
            Exit Sub
        End If
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT CALTYPE,NOOFPIECE,PIECERATE,METALID,TAGTYPE,STOCKTYPE,CASE STONEUNIT WHEN 'C' THEN 'CARAT' WHEN 'G' THEN 'GRAM' ELSE '' END STONEUNIT,"
            strSql += " ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = DEFAULTCOUNTER),'')AS DEFAULTCOUNTER"
            strSql += " FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem__Man.Text & "' AND ACTIVE = 'Y'"
            strSql += GetItemQryFilteration("S")
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                MsgBox("Invalid Item", MsgBoxStyle.Information)
                txtItem__Man.Focus()
                Exit Sub
            End If
            With dt.Rows(0)
                noOfPiece = Val(.Item("NOOFPIECE").ToString)
                calType = .Item("CALTYPE").ToString

                If calType = "R" Then
                    txtSaleRate_Amt.Enabled = True
                    pieceRate = Val(.Item("PIECERATE").ToString)
                Else
                    txtSaleRate_Amt.Clear()
                    txtSaleRate_Amt.Enabled = False
                End If
                If .Item("STOCKTYPE").ToString = "N" Then
                    txtPacketNo__Man.Clear()
                    txtPacketNo__Man.Enabled = False
                Else
                    txtPacketNo__Man.Enabled = True
                    txtSaleRate_Amt.Enabled = True
                    pieceRate = Val(.Item("PIECERATE").ToString)
                End If
                If cmbCounter.Enabled Then
                    cmbCounter.Text = .Item("DEFAULTCOUNTER").ToString
                Else
                    cmbCounter.Text = ""
                End If

                strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem__Man.Text & "'")))
                'strSql = "SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.ITEMID = "
                'strSql += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem__Man.Text & "')"
                objGPack.FillCombo(strSql, cmbSubItem)
                If cmbSubItem.Items.Count > 0 Then
                    cmbSubItem.Enabled = True
                Else
                    cmbSubItem.Enabled = False
                End If

                If .Item("METALID").ToString = "D" Or .Item("METALID").ToString = "T" Then
                    cmbStoneUnit.Enabled = True
                    cmbStoneUnit.Text = .Item("STONEUNIT").ToString
                Else
                    cmbStoneUnit.Enabled = False
                End If

                If .Item("TAGTYPE").ToString = "Y" Then ''LOAD ITEMTYPE
                    strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE ORDER BY NAME"
                    objGPack.FillCombo(strSql, cmbItemType)
                    cmbItemType.Enabled = True
                Else
                    cmbItemType.Items.Clear()
                    cmbItemType.Enabled = False
                End If
            End With
            Dim rate As Double = funcFindMetalRate()
            txtMetalRate_Amt.Text = IIf(rate <> 0, Format(rate, "0.00"), "")
        End If
    End Sub
    Private Sub txtPieces_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPieces_Num_Man.GotFocus
        If Val(txtPieces_Num_Man.Text) = 0 Then
            If noOfPiece = 0 Then
                noOfPiece = 1
            End If
            txtPieces_Num_Man.Text = noOfPiece
        End If
    End Sub

    Private Sub txtSaleRate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSaleRate_Amt.GotFocus
        txtSaleRate_Amt.Text = IIf(pieceRate <> 0, Format(pieceRate, "0.00"), "")
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
    Private Sub txtPacketNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPacketNo__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            funcCheckPacketNo()
        End If
    End Sub

    Private Sub txtWeight_Wet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtWeight_Wet.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If calType <> "R" And (Not Val(txtWeight_Wet.Text) > 0) Then
                MsgBox("Grs Weight Should not Empty", MsgBoxStyle.Information)
                txtWeight_Wet.Focus()
                Exit Sub
            End If
            If objGPack.GetSqlValue("SELECT EXTRAWT FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & txtItem__Man.Text & "'", "EXTRAWT", "N") = "Y" Then
                ObjExtraWt.txtExtraWt_WET.Focus()
                ObjExtraWt.ShowDialog()
            End If
            ShowStoneDia()
        End If
    End Sub
    Private Sub CalcNetWt()
        Dim netWt As Double = Nothing
        netWt = Val(txtWeight_Wet.Text) - Val(txtLessWt.Text)
        txtNetWeight.Text = IIf(netWt <> 0, Format(netWt, "0.000"), "")
    End Sub

    Private Sub txtWeight_Wet_Man_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWeight_Wet.TextChanged
        CalcNetWt()
    End Sub

    Private Sub txtItem__Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItem__Man.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtNetWeight_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNetWeight.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtNetWeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNetWeight.KeyPress
        If Val(txtWeight_Wet.Text) < Val(txtNetWeight.Text + e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub ShowStoneDia()
        If objStone.Visible Then Exit Sub
        If cmbSubItem.Enabled Then
            strSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem__Man.Text & "')"
        Else
            strSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem__Man.Text & "'"
        End If
        If objGPack.GetSqlValue(strSql).ToUpper <> "Y" Then
            Me.SelectNextControl(txtWeight_Wet, True, True, True, True)
            Exit Sub
        End If
        objStone.grsWt = Val(txtWeight_Wet.Text)
        objStone._EditLock = True
        objStone._DelLock = True
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

    Private Sub txtItem__Man_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtItem__Man.TextChanged

    End Sub
End Class