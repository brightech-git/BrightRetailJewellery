Imports System.Data.OleDb
Public Class frmNonTagTransfer
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
    End Sub

    Function funcNew() As Integer
        objGPack.TextClear(Me)
        CmbFromItem.Focus()
    End Function

    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If calType <> "R" And (Not Val(txtWeight_Wet.Text) > 0) Then
            MsgBox("Grs Weight Should not Empty", MsgBoxStyle.Information)
            txtWeight_Wet.Focus()
        End If
        Dim allowzeropcs As String
        strSql = "SELECT ISNULL(ALLOWZEROPCS,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & CmbFromItem.Text & "'"
        allowzeropcs = GetSqlValue(cn, strSql)
        If allowzeropcs = "N" Then
            If Not Val(txtPieces_Num_Man.Text) > 0 Then
                MsgBox(Me.GetNextControl(txtPieces_Num_Man, False).Text + E0001, MsgBoxStyle.Exclamation)
                txtPieces_Num_Man.Focus()
                Exit Function
            End If
        End If
        'strSql = " SELECT "
        'strSql += " SUM(CASE WHEN RECISS = 'R' THEN PCS ELSE -1 * PCS END)AS BALPCS"
        'strSql += " FROM " & cnAdminDb & "..ITEMNONTAG WHERE ITEMID = "
        'strSql += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItem__Man.Text & "')"
        'strSql += " AND ISNULL(CANCEL,'') = ''"
        'If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        'Dim dt As New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    Dim balPcs As Integer = Val(dt.Rows(0).Item("BalPcs").ToString)
        '    If Val(txtPieces_Num_Man.Text) > balPcs Then
        '        MsgBox("Invalid Bal Piece(s)" + vbCrLf + "Balance Receipt Pieces " & balPcs & "", MsgBoxStyle.Exclamation)
        '        txtPieces_Num_Man.Focus()
        '        Return 0
        '    End If
        'Else
        '    MsgBox("Invalid Bal Pieces1", MsgBoxStyle.Exclamation)
        '    txtPieces_Num_Man.Focus()
        '    Return 0
        'End If
        funcAdd()

    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcAdd() As Integer
        Dim FromitemId As Integer = 0
        Dim FromsubItemId As Integer = 0
        Dim ToitemId As Integer = 0
        Dim TosubItemId As Integer = 0
        Dim itemCtrId As Integer = 0
        Dim COSTID As String = ""
        Dim designerId As Integer = 0
        Dim itemType As Integer = 0

        Try
            tran = cn.BeginTransaction()
            ''Find From ItemId
            strSql = " Select ItemId from " & cnAdminDb & "..itemMast where ItemName = '" & CmbFromItem.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            dReader = cmd.ExecuteReader
            If dReader.Read = True Then
                FromitemId = dReader.Item("ItemId")
            End If
            dReader.Close()
            ''Find To ItemId
            strSql = " Select ItemId from " & cnAdminDb & "..itemMast where ItemName = '" & cmbToItem.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            dReader = cmd.ExecuteReader
            If dReader.Read = True Then
                ToitemId = dReader.Item("ItemId")
            End If
            dReader.Close()
            ''Find From SubItemid
            strSql = " Select SubItemId from  " & cnAdminDb & "..SubItemMast where SubItemName = '" & CmbFromSubitem.Text & "' AND ITEMID = " & FromitemId & ""
            cmd = New OleDbCommand(strSql, cn, tran)
            dReader = cmd.ExecuteReader
            If dReader.Read = True Then
                FromsubItemId = dReader.Item("SubItemId")
            End If
            dReader.Close()
            ''Find To SubItemid
            strSql = " Select SubItemId from  " & cnAdminDb & "..SubItemMast where SubItemName = '" & cmbToSubItem.Text & "' AND ITEMID = " & ToitemId & ""
            cmd = New OleDbCommand(strSql, cn, tran)
            dReader = cmd.ExecuteReader
            If dReader.Read = True Then
                TosubItemId = dReader.Item("SubItemId")
            End If
            dReader.Close()
            ' ''Find itemCtrId
            'strSql = " Select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName = '" & cmbCounter.Text & "'"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'dReader = cmd.ExecuteReader
            'If dReader.Read = True Then
            '    itemCtrId = dReader.Item("ItemCtrId")
            'End If
            'dReader.Close()
            ' ''Find COSTID
            'strSql = " Select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre.Text & "'"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'dReader = cmd.ExecuteReader
            'If dReader.Read = True Then
            '    COSTID = dReader.Item("CostId")
            'End If
            'dReader.Close()
            ' ''Find DesignerId
            'strSql = " Select DesignerId from  " & cnAdminDb & "..Designer where DesignerName = '" & cmbDesigner.Text & "'"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'dReader = cmd.ExecuteReader
            'If dReader.Read = True Then
            '    designerId = dReader.Item("DesignerId")
            'End If
            'dReader.Close()
            ' ''Find itemType
            'strSql = " Select itemTypeId from " & cnAdminDb & "..itemType where Name = '" & cmbItemType.Text & "'"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'dReader = cmd.ExecuteReader
            'If dReader.Read = True Then
            '    itemType = dReader.Item("itemTypeId")
            'End If
            'dReader.Close()
            If FromitemId = 0 Or ToitemId = 0 Then
                MsgBox("ItemName should not Empty", MsgBoxStyle.Information)
                CmbFromItem.Focus()
                If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
                Exit Function
            End If
            If FromitemId = ToitemId And FromsubItemId = TosubItemId Then
                MsgBox("Select different item or subitem", MsgBoxStyle.Information)
                cmbToItem.Focus()
                If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
                Exit Function
            End If
            Dim tagSno As String
            'INSERT ISSUE RECORD
            tagSno = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
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
            strSql += " UserId,Updated,Uptime,SystemId,APPVER,EXTRAWT)Values("
            strSql += " '" & tagSno & "'" 'SNO
            strSql += " ," & FromitemId & "" 'ItemId
            strSql += " ," & FromsubItemId & "" 'SubItemId
            strSql += " ,'" & GetStockCompId() & "'" 'Companyid
            strSql += " ,'" & GetServerDate(tran) & "'" 'Recdate
            strSql += " ," & Val(txtPieces_Num_Man.Text) & "" 'Pcs
            strSql += " ," & Val(txtWeight_Wet.Text) & "" 'GrsWt
            strSql += " ," & Val(txtWeight_Wet.Text) - Val(txtNetWeight.Text) & "" 'LessWt
            strSql += " ," & Val(txtNetWeight.Text) & "" 'NetWt
            strSql += " ,0" 'FinRate
            strSql += " ,''" 'Isstype
            strSql += " ,'I'" 'RecIss
            strSql += " ,''" 'Posted
            strSql += " ,0"  'Packetno
            strSql += " ,0" 'DRefno
            strSql += " ," & itemCtrId & "" 'ItemCtrId
            strSql += " ,''" 'OrdRepNo
            strSql += " ,''" 'ORSNO
            strSql += " ,''" 'Narration
            strSql += " ,0" 'Rate
            strSql += " ,'" & cnCostId & "'" 'COSTID
            strSql += " ,''"
            strSql += " ," & designerId & "" 'DesignerId
            strSql += " ," & itemType & "" 'ItemTypeID
            strSql += " ,''" 'Carryflag
            strSql += " ,'0'" 'Reason
            strSql += " ,''" 'BatchNo
            strSql += " ,''" 'Cancel
            strSql += " ," & userId & "" 'UserId
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
            strSql += " ,'" & systemId & "'" 'Systemid
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " ,'" & Val(ObjExtraWt.txtExtraWt_WET.Text) & "'" 'APPVER
            strSql += " )"
            ExecQuery(SyncMode.Stock, strSql, cn, tran, cnCostId)

            'INSERT RECEIPT RECORD
            tagSno = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
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
            strSql += " UserId,Updated,Uptime,SystemId,APPVER,EXTRAWT)Values("
            strSql += " '" & tagSno & "'" 'SNO
            strSql += " ," & ToitemId & "" 'ItemId
            strSql += " ," & TosubItemId & "" 'SubItemId
            strSql += " ,'" & GetStockCompId() & "'" 'Companyid
            strSql += " ,'" & GetServerDate(tran) & "'" 'Recdate
            strSql += " ," & Val(txtPieces_Num_Man.Text) & "" 'Pcs
            strSql += " ," & Val(txtWeight_Wet.Text) & "" 'GrsWt
            strSql += " ," & Val(txtWeight_Wet.Text) - Val(txtNetWeight.Text) & "" 'LessWt
            strSql += " ," & Val(txtNetWeight.Text) & "" 'NetWt
            strSql += " ,0" 'FinRate
            strSql += " ,''" 'Isstype
            strSql += " ,'R'" 'RecIss
            strSql += " ,''" 'Posted
            strSql += " ,0"  'Packetno
            strSql += " ,0" 'DRefno
            strSql += " ," & itemCtrId & "" 'ItemCtrId
            strSql += " ,''" 'OrdRepNo
            strSql += " ,''" 'ORSNO
            strSql += " ,''" 'Narration
            strSql += " ,0" 'Rate
            strSql += " ,'" & cnCostId & "'" 'COSTID
            strSql += " ,''"
            strSql += " ," & designerId & "" 'DesignerId
            strSql += " ," & itemType & "" 'ItemTypeID
            strSql += " ,''" 'Carryflag
            strSql += " ,'0'" 'Reason
            strSql += " ,''" 'BatchNo
            strSql += " ,''" 'Cancel
            strSql += " ," & userId & "" 'UserId
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
            strSql += " ,'" & systemId & "'" 'Systemid
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " ,'" & Val(ObjExtraWt.txtExtraWt_WET.Text) & "'" 'APPVER
            strSql += " )"
            ExecQuery(SyncMode.Stock, strSql, cn, tran, cnCostId)


            Dim BATCHNO As String = 0 'GetNewBatchno(cnCostId, GetServerDate(tran), tran)
            Dim catcode As String

            strSql = " SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & ToitemId & "'"
            Dim dtItemDetail As New DataTable
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtItemDetail)
            If dtItemDetail.Rows.Count > 0 Then
                With dtItemDetail.Rows(0)
                    catcode = .Item("CATCODE").ToString
                End With
            End If
            If chkReceiptPost.Checked = True Then
                'INSERT VALUES IN RECEIPT
                strSql = " INSERT INTO " & cnStockDb & "..RECEIPT"
                strSql += " ("
                strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
                strSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
                strSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
                strSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                strSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
                strSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                strSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                strSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                strSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
                strSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                strSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                strSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,SC"
                strSql += " ,DUSTWT,MELTWT,PUREXCH,MAKE,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER"
                strSql += " ,TOUCH,ESTSNO,OTHERAMT"
                strSql += " )"
                strSql += " VALUES("
                strSql += " '" & GetNewSno(TranSnoType.RECEIPTCODE, tran) & "'" ''SNO
                strSql += " ,0" 'TRANNO
                strSql += " ,'" & GetServerDate(tran) & "'" 'TRANDATE
                strSql += " ,'PU'" 'TRANTYPE
                strSql += " ," & Val(txtPieces_Num_Man.Text) & "" 'PCS
                strSql += " ," & Val(txtWeight_Wet.Text) & "" 'GRSWT
                strSql += " ," & Val(txtNetWeight.Text) & "" 'NETWT
                strSql += " ," & Val(txtWeight_Wet.Text) - Val(txtNetWeight.Text) & "" 'LESSWT
                strSql += " ," & 0 & "" 'PUREWT '0
                strSql += " ,''" 'TAGNO
                strSql += " ," & ToitemId & "" 'ITEMID
                strSql += " ," & TosubItemId & "" 'SUBITEMID
                strSql += " ," & 0 & "" 'WASTPER
                strSql += " ," & 0 & "" 'WASTAGE
                strSql += " ,0" 'MCGRM
                strSql += " ,0" 'MCHARGE
                strSql += " ," & 0 & "" 'AMOUNT
                strSql += " ," & 0 & "" 'RATE
                strSql += " ," & 0 & "" 'BOARDRATE
                strSql += " ,''" 'SALEMODE
                strSql += " ,''" 'GRSNET
                strSql += " ,''" 'TRANSTATUS ''
                strSql += " ,''" 'REFNO ''
                strSql += " ,NULL" 'REFDATE NULL
                strSql += " ,'" & cnCostId & "'" 'COSTID 
                strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                strSql += " ,'W'" 'FLAG 
                strSql += " ,0" 'EMPID
                strSql += " ,0" 'TAGGRSWT
                strSql += " ,0" 'TAGNETWT
                strSql += " ,0" 'TAGRATEID
                strSql += " ,0" 'TAGSVALUE
                strSql += " ,''" 'TAGDESIGNER  
                strSql += " ,0" 'ITEMCTRID
                strSql += " ," & 0 & "" 'ITEMTYPEID
                strSql += " ," & 0 & "" 'PURITY
                strSql += " ,''" 'TABLECODE
                strSql += " ,''" 'INCENTIVE
                strSql += " ,''" 'WEIGHTUNIT
                strSql += " ,'" & catcode & "'" 'CATCODE
                strSql += " ,''" 'OCATCODE
                strSql += " ,''" 'ACCODE
                strSql += " ,0" 'ALLOY
                strSql += " ,'" & BATCHNO & "'" 'BATCHNO
                strSql += " ,'NONTAG TRANSFER'" 'REMARK1
                strSql += " ,''" 'REMARK2
                strSql += " ,'" & userId & "'" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ," & 0 & "" 'DISCOUNT
                strSql += " ,''" 'RUNNO
                strSql += " ,''" 'CASHID
                strSql += " ," & 0 & "" 'TAX
                strSql += " ," & 0 & "" 'SC

                strSql += " ," & 0 & "" 'DUSTWT
                strSql += " ," & 0 & "" 'DUSTWT
                strSql += " ,''" 'PUREXCH
                strSql += " ,''" 'MAKE
                strSql += " ," & 0 & "" 'STONEAMT
                strSql += " ," & 0 & "" 'MISCAMT
                strSql += " ,'" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE= '" & catcode & "'", , , tran) & "'" 'METALID
                strSql += " ,''" 'STONEUNIT
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ,0" 'TOUCH
                strSql += " ,''" 'ESTNO'
                strSql += " ," & 0 & "" 'OTHERAMT
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If
            ' ''Inserting StoneDetail
            'For Each ro As DataRow In objStone.dtGridStone.Rows
            '    Dim stnItemId As Integer = 0
            '    Dim stnSubItemId As Integer = 0
            '    'Dim caType As String = Nothing
            '    strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("ITEM").ToString & "'"
            '    stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
            '    strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & ro.Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
            '    stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))


            '    ''Inserting itemTagStone
            '    strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAGSTONE("
            '    strSql += " SNO,RECISS,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
            '    strSql += " STNSUBITEMID,STNPCS,STNWT,"
            '    strSql += " STNRATE,STNAMT,DESCRIP,"
            '    strSql += " RECDATE,PURRATE,PURAMT,CALCMODE,"
            '    strSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
            '    strSql += " VATEXM,CARRYFLAG,COSTID,SYSTEMID,APPVER)VALUES("
            '    strSql += " '" & GetNewSno(TranSnoType.ITEMNONTAGSTONECODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
            '    strSql += " ,'I'" ' RECISS
            '    strSql += " ,'" & tagSno & "'" 'TAGSNO
            '    strSql += " ,'" & itemId & "'" 'ITEMID
            '    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
            '    strSql += " ," & stnItemId & "" 'STNITEMID
            '    strSql += " ," & stnSubItemId & "" 'STNSUBITEMID
            '    strSql += " ," & Val(ro.Item("PCS").ToString) & "" 'STNPCS
            '    strSql += " ," & Val(ro.Item("WEIGHT").ToString) & "" 'STNWT
            '    strSql += " ," & Val(ro.Item("RATE").ToString) & "" 'STNRATE
            '    strSql += " ," & Val(ro.Item("AMOUNT").ToString) & "" 'STNAMT
            '    If stnSubItemId <> 0 Then 'DESCRIP
            '        strSql += " ,'" & ro.Item("SUBITEM").ToString & "'"
            '    Else
            '        strSql += " ,'" & ro.Item("ITEM").ToString & "'"
            '    End If
            '    strSql += " ,'" & GetEntryDate(dtpTranDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
            '    strSql += " ,0" 'PURRATE
            '    strSql += " ,0" 'PURAMT
            '    strSql += " ,'" & ro.Item("CALC").ToString & "'" 'CALCMODE
            '    strSql += " ,0" 'MINRATE
            '    strSql += " ,0" 'SIZECODE
            '    strSql += " ,'" & ro.Item("UNIT").ToString & "'" 'STONEUNIT
            '    strSql += " ,NULL" 'ISSDATE
            '    strSql += " ,''" 'VATEXM
            '    strSql += " ,''" 'CARRYFLAG
            '    strSql += " ,'" & COSTID & "'" 'COSTID
            '    strSql += " ,'" & systemId & "'" 'SYSTEMID
            '    strSql += " ,'" & VERSION & "'" 'APPVER
            '    strSql += " )"
            '    ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TITEMNONTAGSTONE")
            'Next
            tran.Commit()
            funcMultyNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcMultyNew() As Integer
        txtPieces_Num_Man.Clear()
        txtWeight_Wet.Clear()
        txtNetWeight.Clear()
        objStone = New frmStoneDia
        ObjExtraWt = New frmExtaWt
        If cmbToSubItem.Enabled = True Then
            cmbToSubItem.Focus()
        Else
            txtPieces_Num_Man.Focus()
        End If
    End Function
    Private Sub frmNonTagIssue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmNonTagIssue_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dt As New DataTable
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'')='Y' AND ISNULL(STOCKTYPE,'')<>'T'"
        objGPack.FillCombo(strSql, CmbFromItem, , False)
        objGPack.FillCombo(strSql, cmbToItem, , False)
        funcNew()
    End Sub
    Private Sub txtPieces_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPieces_Num_Man.GotFocus
        If Val(txtPieces_Num_Man.Text) = 0 Then
            If noOfPiece = 0 Then
                noOfPiece = 1
            End If
            txtPieces_Num_Man.Text = noOfPiece
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

    Private Sub txtWeight_Wet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtWeight_Wet.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If calType <> "R" And (Not Val(txtWeight_Wet.Text) > 0) Then
                MsgBox("Grs Weight Should not Empty", MsgBoxStyle.Information)
                txtWeight_Wet.Focus()
                Exit Sub
            End If
        End If
    End Sub
    Private Sub CalcNetWt()
        Dim netWt As Double = Nothing
        netWt = Val(txtWeight_Wet.Text)
        txtNetWeight.Text = IIf(netWt <> 0, Format(netWt, "0.000"), "")
    End Sub

    Private Sub txtWeight_Wet_Man_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWeight_Wet.TextChanged
        CalcNetWt()
    End Sub
    Private Sub cmbToItem_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbToItem.Leave
        If cmbToItem.Text <> "" Then
            strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbToItem.Text & "'")))
            objGPack.FillCombo(strSql, cmbToSubItem)
            If cmbToSubItem.Items.Count > 0 Then
                cmbToSubItem.Enabled = True
            Else
                cmbToSubItem.Enabled = False
            End If
        Else
            cmbToSubItem.Enabled = False
        End If
    End Sub

    Private Sub CmbFromItem_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbFromItem.Leave
        If CmbFromItem.Text <> "" Then
            strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & CmbFromItem.Text & "'")))
            objGPack.FillCombo(strSql, CmbFromSubitem)
            If CmbFromSubitem.Items.Count > 0 Then
                CmbFromSubitem.Enabled = True
            Else
                CmbFromSubitem.Enabled = False
            End If
        Else
            CmbFromSubitem.Enabled = False
        End If
    End Sub
End Class