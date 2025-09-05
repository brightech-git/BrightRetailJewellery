Imports System.Data.OleDb
Public Class frmCentRate
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim Sno As Integer
    Dim strpath As String
    'Dim HideAccLink As Boolean = IIf(GetAdmindbSoftValue("HIDE_ACHARI_ACCLINK", "Y") = "Y", True, False)
    Dim _FourCMaintain As Boolean = IIf(GetAdmindbSoftValue("4CMAINTAIN", "Y") = "Y", True, False)
    Dim CENTRATE_DESIGN As Boolean = IIf(GetAdmindbSoftValue("CENTRATE_DES", "Y") = "Y", True, False)
    Dim W_Sale As Boolean = IIf(GetAdmindbSoftValue("ISWHOLESALE", "N") = "Y", True, False)
    Dim selectedcostid As String
    Dim ORDER_MULTI_MIMR As Boolean = IIf(GetAdmindbSoftValue("ORDER_MULTI_MIMR", "N") = "Y", True, False)
    Dim POS_ENABLE_STNGRP As Boolean = IIf(GetAdmindbSoftValue("POS_ENABLE_STNGRP", "N") = "Y", True, False)
    Dim CENTRATE_EXCELUPLOAD As Boolean = IIf(GetAdmindbSoftValue("CENTRATE_EXCELUPLOAD", "N") = "Y", True, False)
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT SLNO,"
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            strSql += " (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = CR.COSTID)AS COSTNAME,"
        End If
        If _IsWholeSaleType Then
            strSql += " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = CR.ACCODE)aS ACNAME,"
        End If
        If CENTRATE_DESIGN Then
            strSql += " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = CR.DESIGNERID)AS DESIGNER,"
        End If
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = CR.ITEMID)AS ITEMNAME,"
        strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.SUBITEMID = CR.SUBITEMID),'')AS SUBITEMNAME,"
        If _FourCMaintain Then
            strSql += " ISNULL((SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR AS S WHERE S.COLORID = CR.COLORID),'')AS COLORNAME,"
            strSql += " ISNULL((SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT AS S WHERE S.CUTID = CR.CUTID),'')AS CUTNAME,"
            strSql += " ISNULL((SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY AS S WHERE S.CLARITYID = CR.CLARITYID),'')AS CLARITYNAME,"
            strSql += " ISNULL((SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE AS SH WHERE SH.SHAPEID = CR.SHAPEID),'')AS SHAPENAME,"
            strSql += " ISNULL((SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP AS SH WHERE SH.GROUPID = CR.STNGRPID),'')AS GROUPNAME,"
        End If
        strSql += " FROMCENT,TOCENT,"
        strSql += " MAXRATE,MINRATE,PURRATE,SALESPER"
        strSql += " FROM " & cnAdminDb & "..CENTRATE AS CR"
        strSql += " ORDER BY "
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then strSql += " COSTNAME,"
        If _IsWholeSaleType Then strSql += " ACNAME,"
        strSql += " ITEMNAME,SUBITEMNAME,FROMCENT,TOCENT"
        funcOpenGrid(strSql, gridView)
        With gridView
            .Columns("SLNO").Visible = False
            If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then .Columns("COSTNAME").Width = 150
            If _IsWholeSaleType Then .Columns("ACNAME").Width = 200
            If CENTRATE_DESIGN Then .Columns("DESIGNER").Width = 100
            .Columns("ITEMNAME").Width = 150
            .Columns("SUBITEMNAME").Width = 150
            .Columns("FROMCENT").HeaderText = "FROMCENT"
            .Columns("FROMCENT").Width = 70
            .Columns("FROMCENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TOCENT").HeaderText = "TOCENT"
            .Columns("TOCENT").Width = 70
            .Columns("TOCENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MAXRATE").Width = 70
            .Columns("MAXRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MINRATE").Width = 70
            .Columns("MINRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PURRATE").Width = 70
            .Columns("PURRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALESPER").Width = 70
            .Columns("SALESPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        pnlControls.Enabled = True
        BtnExport.Enabled = True
        BtnUpdate.Enabled = False
        flagSave = False
        funcCallGrid()
        funcLoadItemName()
        funcLoadStnGroup()
        If _IsWholeSaleType Then
            cmbParty.Select()
        ElseIf CENTRATE_DESIGN Then
            cmbDesigner_MAN.Select()
        Else
            cmbItemName_Man.Select()
        End If
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If funcValidation() = True Then Exit Function
        If flagSave = False Then
            '            if chkLstCostcentre.CheckedItems.
            If chkLstCostcentre.CheckedItems.Count > 0 Then
                For cnt As Integer = 0 To chkLstCostcentre.CheckedItems.Count - 1
                    Dim costid As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkLstCostcentre.CheckedItems.Item(cnt).ToString & "'")
                    If funcCheckUnique(txtCentFrom.Text, txtCentTo.Text, costid) = True Then
                        MsgBox("Already Exist", MsgBoxStyle.Information)
                        cmbItemName_Man.Focus()
                        Exit Function
                    Else
                        funcAdd(costid)
                    End If
                Next
            Else
                If GetAdmindbSoftValue("COSTCENTRE", "N") <> "Y" Then funcAdd() Else MsgBox("Atleast One costid must be select") : Exit Function
            End If
            funcNew()
            Exit Function
        Else
            'If funcCheckUnique(txtCentFrom.Text, txtCentTo.Text) = True Then
            '    MsgBox("Already Exist", MsgBoxStyle.Information)
            '    txtCentFrom.Focus()
            '    Exit Function
            'End If
            funcUpdate(selectedcostid)
            Exit Function
        End If
    End Function
    Function funcAdd(Optional ByVal costid As String = Nothing) As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        Dim ItemId As Integer = Nothing
        Dim SubItemId As Integer = Nothing
        Dim ColorId As Integer = 0
        Dim CutId As Integer = 0
        Dim ClarityId As Integer = 0
        Dim ShapeId As Integer = 0
        Dim StnGrpId As Integer = 0
        If _FourCMaintain Then
            ColorId = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & CmbColor.Text & "'", "COLORID", 0)
            CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & CmbCut.Text & "'", "CUTID", 0)
            ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & CmbClarity.Text & "'", "CLARITYID", 0)
            ShapeId = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & cmbShape.Text & "'", "SHAPEID", 0)
            StnGrpId = objGPack.GetSqlValue("SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & cmbStnGroup_MAN.Text & "'", "GROUPID", 0)
        End If
        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE"
        strSql += " ITEMNAME = '" & cmbItemName_Man.Text & "'"
        ItemId = Val(objGPack.GetSqlValue(strSql, , , tran))

        strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE"
        strSql += " SUBITEMNAME = '" & cmbSubItemName_Man.Text & "'"
        strSql += " AND ITEMID = " & ItemId & ""
        SubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
        strSql = " INSERT INTO " & cnAdminDb & "..CENTRATE"
        strSql += " (COSTID,ITEMID,SUBITEMID,FROMCENT,TOCENT,"
        strSql += " MAXRATE,MINRATE,PURRATE,"
        strSql += " USERID,UPDATED,UPTIME,ACCODE,DESIGNERID"
        If _FourCMaintain Then
            strSql += " ,COLORID,CUTID,CLARITYID,SHAPEID,STNGRPID"
        End If
        strSql += " ,SALESPER"
        strSql += " )VALUES("
        strSql += " '" & costid & "'" 'costid
        strSql += " ," & ItemId & "" 'ItemId
        strSql += " ," & SubItemId & "" 'SubItemId
        strSql += " ," & Val(txtCentFrom.Text) & "" 'FromCent
        strSql += " ," & Val(txtCentTo.Text) & "" 'ToCent
        strSql += " ," & Val(txtMaxRate_Amt.Text) & "" 'Maxrate
        strSql += " ," & Val(txtMinRate_Amt.Text) & "" 'MinRate
        strSql += " ," & Val(txtPurchaseRate_Amt.Text) & "" 'PurRate
        strSql += " ,'" & userId & "'" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += " ,'" & IIf(_IsWholeSaleType, objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbParty.Text & "'"), "") & "'" 'ACCODE
        strSql += " ,'" & IIf(CENTRATE_DESIGN, objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'"), "") & "'" 'DESIGNERID
        If _FourCMaintain Then
            strSql += " ," & ColorId & ""
            strSql += " ," & CutId & ""
            strSql += " ," & ClarityId & ""
            strSql += " ," & ShapeId & ""
            strSql += " ," & StnGrpId & ""
        End If
        strSql += " ," & Val(txtsaleper_PER.Text) & "" 'SALESPER
        strSql += " )"
        Try
            ExecQuery(SyncMode.Transaction, strSql, cn, , costid)
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate(Optional ByVal costid As String = Nothing) As Integer
        Dim ItemId As Integer = Nothing
        Dim SubItemId As Integer = Nothing

        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE"

        strSql += " ITEMNAME = '" & cmbItemName_Man.Text & "'"
        ItemId = Val(objGPack.GetSqlValue(strSql))

        strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE"
        strSql += " SUBITEMNAME = '" & cmbSubItemName_Man.Text & "'"
        strSql += " AND ITEMID = " & ItemId & ""
        SubItemId = Val(objGPack.GetSqlValue(strSql))
        strSql = " UPDATE " & cnAdminDb & "..CENTRATE SET"
        'STRSQL += " COSTID='" & COSTID & "'"
        strSql += " ITEMID=" & ItemId & ""
        strSql += " ,SUBITEMID=" & SubItemId & ""
        strSql += " ,FROMCENT=" & Val(txtCentFrom.Text) & ""
        strSql += " ,TOCENT=" & Val(txtCentTo.Text) & ""
        strSql += " ,MAXRATE=" & Val(txtMaxRate_Amt.Text) & ""
        strSql += " ,MINRATE=" & Val(txtMinRate_Amt.Text) & ""
        strSql += " ,PURRATE=" & Val(txtPurchaseRate_Amt.Text) & ""
        strSql += " ,USERID='" & userId & "'"
        strSql += " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,UPTIME='" & Date.Now.ToLongTimeString & "'"
        If _IsWholeSaleType Then
            strSql += " ,ACCODE = '" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbParty.Text & "'") & "'" 'ACCODE
        End If
        If CENTRATE_DESIGN Then
            strSql += " ,DESIGNERID='" & objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'") & "'" 'DESIGNERID
        End If
        If _FourCMaintain Then
            strSql += " ,COLORID = '" & objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & CmbColor.Text & "'") & "'" 'COLORID
            strSql += " ,CUTID = '" & objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & CmbCut.Text & "'") & "'" 'CUTID
            strSql += " ,CLARITYID = '" & objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & CmbClarity.Text & "'") & "'" 'CLARITYID
            strSql += " ,SHAPEID = '" & objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & cmbShape.Text & "'") & "'" 'SHAPEID
        End If
        strSql += " ,SALESPER=" & Val(txtsaleper_PER.Text) & ""
        strSql += " WHERE SLNO = '" & Sno & "'"
        Try
            ExecQuery(SyncMode.Transaction, strSql, cn, , costid)

            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal temp As Integer) As Integer
        strSql = " Select Slno,COSTID,"
        strSql += " (select costname from " & cnAdminDb & "..costcentre as C where c.costid= CR.costid)as CostName,"
        strSql += " (select ItemName from " & cnAdminDb & "..ItemMast as I where I.ItemId = CR.ItemId)as ItemName,"
        strSql += " isnull((select SubItemName from " & cnAdminDb & "..SubItemMast as S where S.SubItemId = CR.SubItemId),'')as SubItemName,"
        strSql += " FromCent,ToCent,"
        strSql += " Maxrate,MinRate,PurRate,SALESPER"
        If _IsWholeSaleType Then
            strSql += " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = CR.ACCODE)AS ACNAME"
        End If
        If CENTRATE_DESIGN Then
            strSql += " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = CR.DESIGNERID)AS DESIGNER"
        End If
        If _FourCMaintain Then
            strSql += " ,(SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID = CR.COLORID)AS COLORNAME"
            strSql += " ,(SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = CR.CUTID)AS CUTNAME"
            strSql += " ,(SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID = CR.CLARITYID)AS CLARITYNAME"
            strSql += " ,(SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID = CR.SHAPEID)AS SHAPENAME"
            strSql += " ,(SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = CR.STNGRPID)AS GROUPNAME"
        End If
        strSql += " from " & cnAdminDb & "..CentRate as CR"
        strSql += " Where Slno = '" & temp & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            selectedcostid = .Item("costID").ToString
            cmbItemName_Man.Text = .Item("ItemName").ToString
            cmbSubItemName_Man.Text = .Item("SubItemName").ToString
            txtCentFrom.Text = .Item("FromCent").ToString
            txtCentTo.Text = .Item("ToCent").ToString
            txtMaxRate_Amt.Text = .Item("Maxrate").ToString
            txtMinRate_Amt.Text = .Item("MinRate").ToString
            txtPurchaseRate_Amt.Text = .Item("PurRate").ToString
            txtsaleper_PER.Text = .Item("SALESPER").ToString
            If _IsWholeSaleType Then cmbParty.Text = .Item("ACNAME").ToString
            If CENTRATE_DESIGN Then cmbDesigner_MAN.Text = .Item("DESIGNER").ToString
            If _FourCMaintain Then
                CmbColor.Text = .Item("COLORNAME").ToString
                CmbCut.Text = .Item("CUTNAME").ToString
                CmbClarity.Text = .Item("CLARITYNAME").ToString
                cmbShape.Text = .Item("SHAPENAME").ToString
                cmbStnGroup_MAN.Text = .Item("GROUPNAME").ToString
            End If
            For ij As Integer = 0 To chkLstCostcentre.Items.Count - 1
                If chkLstCostcentre.Items(ij).ToString = .Item("costname").ToString Then chkLstCostcentre.SetItemChecked(ij, True) Else chkLstCostcentre.SetItemChecked(ij, False)
            Next
            chkLstCostcentre.Enabled = False
        End With

        flagSave = True
        Sno = temp
        If Not _IsWholeSaleType Then pnlControls.Enabled = False
    End Function
    Function funcLoadStnGroup() As Integer
        strSql = " SELECT DISTINCT GROUPNAME,DISPLAYORDER FROM " & cnAdminDb & "..STONEGROUP "
        strSql += " WHERE ISNULL(ACTIVE,'')<> 'N'"
        strSql += " ORDER BY DISPLAYORDER,GROUPNAME"
        objGPack.FillCombo(strSql, cmbStnGroup_MAN)
        If cmbStnGroup_MAN.Items.Count < 1 Then cmbStnGroup_MAN.Enabled = False
    End Function
    Function funcLoadItemName() As Integer
        strSql = " select ItemName from " & cnAdminDb & "..ItemMast"
        strSql += " Where METALID = 'D' or METALID = 'T'"
        strSql += " order by itemname"
        objGPack.FillCombo(strSql, cmbItemName_Man)
    End Function
    Function funcLoadSubItemName() As Integer
        cmbSubItemName_Man.Text = ""
        strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'")))
        'strSql = " select SubItemName from " & cnAdminDb & "..SubitemMast "
        'strSql += " where ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemName_Man.Text & "' and SubItem = 'Y')"
        'strSql += " order by subitemname"  
        objGPack.FillCombo(strSql, cmbSubItemName_Man, True)
        If cmbSubItemName_Man.Items.Count > 0 Then cmbSubItemName_Man.Enabled = True Else cmbSubItemName_Man.Enabled = False
    End Function

    Private Sub frmCentRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            cmbItemName_Man.Focus()
        End If
    End Sub

    Private Sub frmCentRate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbItemName_Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmCentRate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        _IsWholeSaleType = W_Sale
        If _IsWholeSaleType Then
            lblParty.Visible = True
            cmbParty.Visible = True
            strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN ('C','G','D') ORDER BY ACNAME"
            objGPack.FillCombo(strSql, cmbParty)
            cmbParty.Enabled = True
        Else
            lblParty.Visible = False
            cmbParty.Visible = False
            cmbParty.Enabled = False
        End If
        If CENTRATE_DESIGN Then
            lbldesigner.Visible = True
            cmbDesigner_MAN.Visible = True
            strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY DESIGNERNAME"
            objGPack.FillCombo(strSql, cmbDesigner_MAN)
            cmbDesigner_MAN.Enabled = True
        Else
            lbldesigner.Visible = False
            cmbDesigner_MAN.Visible = False
            cmbDesigner_MAN.Enabled = False
        End If
        If CENTRATE_EXCELUPLOAD Then
            BtnUpdate.Visible = True
            BtnTemplate.Visible = True
            BtnImport.Visible = True
        Else
            BtnUpdate.Visible = False
            BtnTemplate.Visible = False
            BtnImport.Visible = False
        End If
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            chkLstCostcentre.Items.Clear()
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            For Each ro As DataRow In dt.Rows
                chkLstCostcentre.Items.Add(ro.Item(0))
            Next
        Else
            chkLstCostcentre.Enabled = False
        End If

        If CENTRATE_DESIGN And _IsWholeSaleType = False Then
            lbldesigner.Location = New Point(18, 33)
            cmbDesigner_MAN.Location = New Point(118, 30)
        End If
        If _FourCMaintain Then
            pnl4c.Visible = True
            strSql = " SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE ISNULL(ACTIVE,'')='Y' ORDER BY COLORNAME"
            objGPack.FillCombo(strSql, CmbColor)
            strSql = " SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE ISNULL(ACTIVE,'')='Y' ORDER BY CUTNAME"
            objGPack.FillCombo(strSql, CmbCut)
            strSql = " SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE ISNULL(ACTIVE,'')='Y' ORDER BY CLARITYNAME"
            objGPack.FillCombo(strSql, CmbClarity)
            strSql = " SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE ISNULL(ACTIVE,'')='Y' ORDER BY SHAPENAME"
            objGPack.FillCombo(strSql, cmbShape)
        End If
        funcGridStyle(gridView)
        funcNew()
    End Sub
    Function funcValidation() As Boolean
        If txtCentFrom.Text = "" Then
            MsgBox(E0005, MsgBoxStyle.Information)
            txtCentFrom.Focus()
            Return True
        End If
        If txtCentTo.Text = "" Then
            MsgBox(E0005, MsgBoxStyle.Information)
            txtCentTo.Focus()
            Return True
        End If
        If Not Val(txtCentFrom.Text) <= Val(txtCentTo.Text) Then
            MsgBox(E0005 + vbCrLf + E0006 + txtCentTo.Text, MsgBoxStyle.Information)
            txtCentFrom.Focus()
            Return True
        End If
        If txtMaxRate_Amt.Text = "" And txtMinRate_Amt.Text = "" And txtPurchaseRate_Amt.Text = "" Then
            MsgBox(E0007, MsgBoxStyle.Information)
            txtMaxRate_Amt.Focus()
            Return True
        End If
        Return False
    End Function
    Function funcCheckUnique(ByVal frmCent As String, ByVal toCent As String, Optional ByVal mCostid As String = Nothing) As Boolean
        Dim str As String = Nothing
        Dim dt As New DataTable
        dt.Clear()
        str = " Declare @fromCent as float,@toCent as float"
        str += " Set @fromCent = " & Val(frmCent) & ""
        str += " Set @toCent = " & Val(toCent) & ""
        str += " select 1 from " & cnAdminDb & "..CentRate"
        str += " where"
        str += " ((FromCent between @fromCent and @toCent)or (ToCent between @fromCent and @toCent))"
        str += " and Itemid = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemName_Man.Text & "')"
        str += " and SubItemid = isnull((select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
        If _FourCMaintain Then
            str += " and COLORID = '" & objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & CmbColor.Text & "'") & "'" 'COLORID
            str += " and CUTID = '" & objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & CmbCut.Text & "'") & "'" 'CUTID
            str += " and CLARITYID = '" & objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & CmbClarity.Text & "'") & "'" 'CLARITYID
            str += " and SHAPEID = '" & objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & cmbShape.Text & "'") & "'" 'SHAPEID
        End If
        If _IsWholeSaleType Then
            str += "AND ISNULL(ACCODE,'') = ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbParty.Text & "'),'')"
        End If
        If CENTRATE_DESIGN Then
            str += "AND ISNULL(DESIGNERID,0) = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'),0)"
        End If
        If mCostid <> Nothing Then
            str += "AND ISNULL(COSTID,'') = '" & mCostid & "'"
        End If
        If flagSave = True Then
            str += " and slno <> '" & Sno & "'"
        End If
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return True ''Already Exist
        End If
        Return False
    End Function
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        gridView.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub cmbItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbItemName_Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub cmbItemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName_Man.SelectedIndexChanged
        funcLoadSubItemName()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus

    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            cmbItemName_Man.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value)
                If _FourCMaintain Then
                    lblCut.Focus()
                Else
                    txtCentFrom.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..CENTRATE WHERE 1<>1"

        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SLNO").Value.ToString
        Dim chkcostid As String = ""
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            If gridView.Rows(gridView.CurrentRow.Index).Cells("COSTNAME").Value.ToString <> "" Then chkcostid = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & gridView.Rows(gridView.CurrentRow.Index).Cells("COSTNAME").Value.ToString & "'")
        End If
        If chkcostid = "" Then chkcostid = cnCostId
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..CENTRATE WHERE ISNULL(COSTID,'') ='" & chkcostid & "' AND SLNO = '" & delKey & "'")
        funcCallGrid()
    End Sub

    Private Sub txtCentFrom_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCentFrom.GotFocus
        If flagSave = True Then Exit Sub
        'If Val(txtWtFrom_Wet.Text) <> 0 Then Exit Sub
        strSql = " SELECT MAX(TOCENT) FROM " & cnAdminDb & "..CENTRATE"
        strSql += vbCrLf + " WHERE "
        strSql += vbCrLf + " ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'),0)"
        strSql += vbCrLf + " AND ISNULL(SUBITEMID,0) = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "')),0)"
        If _IsWholeSaleType Then
            strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbParty.Text & "'),'')"
        End If
        If CENTRATE_DESIGN Then
            strSql += vbCrLf + " AND ISNULL(DESIGNERID,0) = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'),0)"
        End If
        If POS_ENABLE_STNGRP Then
            strSql += vbCrLf + " AND ISNULL(STNGRPID,0) = ISNULL((SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & cmbStnGroup_MAN.Text & "'),0)"
        End If
        Dim wt As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" Then
            txtCentFrom.Text = Format(wt + 0.0001, FormatNumberStyle(DiaRnd))
        Else
            txtCentFrom.Text = Format(wt + 0.001, "0.000")
        End If
    End Sub

    Private Sub txtCentFrom_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCentFrom.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then

        Else
            If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" And DiaRnd = 4 Then
                WeightValidation(txtCentFrom, e, DiaRnd)
            Else
                WeightValidation(txtCentFrom, e)
            End If
        End If
    End Sub

    Private Sub txtCentTo_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCentTo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
        Else
            If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" And DiaRnd = 4 Then
                WeightValidation(txtCentTo, e, DiaRnd)
            Else
                WeightValidation(txtCentTo, e)
            End If
        End If
    End Sub


    Private Sub txtCentFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCentFrom.LostFocus
        If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" Then
            txtCentFrom.Text = IIf(Val(txtCentFrom.Text) <> 0, Format(Val(txtCentFrom.Text), FormatNumberStyle(DiaRnd)), txtCentFrom.Text)
        Else
            txtCentFrom.Text = IIf(Val(txtCentFrom.Text) <> 0, Format(Val(txtCentFrom.Text), "0.000"), txtCentFrom.Text)
        End If
    End Sub

    Private Sub txtCentTo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCentTo.LostFocus
        If objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName_Man.Text & "'") = "D" Then
            txtCentTo.Text = IIf(Val(txtCentTo.Text) <> 0, Format(Val(txtCentTo.Text), FormatNumberStyle(DiaRnd)), txtCentTo.Text)
        Else
            txtCentTo.Text = IIf(Val(txtCentTo.Text) <> 0, Format(Val(txtCentTo.Text), "0.000"), txtCentTo.Text)
        End If
    End Sub
    Function StnGrpSelection_Changed()
        Dim dtStnGrp As New DataTable
        strSql = vbCrLf + "SELECT "
        strSql += vbCrLf + "(SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = S.CUTID) CUTNAME "
        strSql += vbCrLf + ",(SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR  WHERE COLORID = S.COLORID) COLORNAME "
        strSql += vbCrLf + ",(SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY  WHERE CLARITYID = S.CLARITYID) CLARITYNAME"
        strSql += vbCrLf + ",(SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE  WHERE SHAPEID = S.SHAPEID) SHAPENAME"
        strSql += vbCrLf + ",* from " & cnAdminDb & "..STONEGROUP S  WHERE GROUPNAME = '" & cmbStnGroup_MAN.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        dtStnGrp = New DataTable
        da.Fill(dtStnGrp)
        If dtStnGrp.Rows.Count > 0 Then
            If dtStnGrp.Rows(0).Item("CUTNAME").ToString <> "" Then
                CmbCut.Items.Clear() : CmbCut.Items.Add(dtStnGrp.Rows(0).Item("CUTNAME").ToString) : CmbCut.Text = dtStnGrp.Rows(0).Item("CUTNAME").ToString
                CmbCut.Enabled = False
            End If
            If dtStnGrp.Rows(0).Item("CLARITYNAME").ToString <> "" Then
                CmbClarity.Items.Clear() : CmbClarity.Items.Add(dtStnGrp.Rows(0).Item("CLARITYNAME").ToString) : CmbClarity.Text = dtStnGrp.Rows(0).Item("CLARITYNAME").ToString
                CmbClarity.Enabled = False
            End If
            If dtStnGrp.Rows(0).Item("COLORNAME").ToString <> "" Then
                CmbColor.Items.Clear() : CmbColor.Items.Add(dtStnGrp.Rows(0).Item("COLORNAME").ToString) : CmbColor.Text = dtStnGrp.Rows(0).Item("COLORNAME").ToString
                CmbColor.Enabled = False
            End If
            If dtStnGrp.Rows(0).Item("SHAPENAME").ToString <> "" Then
                cmbShape.Items.Clear() : cmbShape.Items.Add(dtStnGrp.Rows(0).Item("SHAPENAME").ToString) : cmbShape.Text = dtStnGrp.Rows(0).Item("SHAPENAME").ToString
                cmbShape.Enabled = False
            End If
        Else
            CmbCut.Enabled = True
            CmbColor.Enabled = True
            cmbShape.Enabled = True
            CmbClarity.Enabled = True
        End If
    End Function
    Private Sub cmbStnGroup_MAN_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStnGroup_MAN.SelectedIndexChanged
        StnGrpSelection_Changed()
    End Sub

    Private Sub cmbStnGroup_MAN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbStnGroup_MAN.KeyPress
        StnGrpSelection_Changed()
    End Sub

    Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles BtnImport.Click
        Dim OpenDialog As New OpenFileDialog

        Dim Str As String
        Str = "(*.xls) | *.xls"
        Str += "|(*.xlsx) | *.xlsx"
        OpenDialog.Filter = Str
        If OpenDialog.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            strpath = OpenDialog.FileName
            If strpath = "" Then
            Else
                loadexcel()
            End If
        End If
    End Sub

    Private Sub loadexcel()
        Dim Dt As New DataTable
        Dim MyConnection As System.Data.OleDb.OleDbConnection
        ''MyConnection = New OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & strpath & "';Extended Properties=Excel 8.0;")
        MyConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" & strpath & "';Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=1;""")
        strSql = "SELECT * FROM [SHEET1$] "
        da = New OleDbDataAdapter(strSql, MyConnection)
        Dt = New DataTable
        da.Fill(Dt)

        Dim dttabl As New DataTable
        dttabl = Dt.Clone
        Dim filterstr As String = ""
        filterstr = "ITEMNAME IS NOT NULL AND FROMCENT IS NOT NULL AND TOCENT IS NOT NULL"
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then filterstr += " AND COSTNAME IS NOT NULL"
        If _IsWholeSaleType Then filterstr += " AND ACNAME IS NOT NULL"
        If CENTRATE_DESIGN Then filterstr += " AND DESIGNER IS NOT NULL"

        For Each dr1 As DataRow In Dt.Select(filterstr.ToString)
            dttabl.ImportRow(dr1)
        Next
        gridView.DataSource = Nothing
        gridView.DataSource = dttabl
        If gridView.Rows.Count > 0 Then BtnUpdate.Enabled = True
        MyConnection.Close()
    End Sub

    Private Sub BtnTemplate_Click(sender As Object, e As EventArgs) Handles BtnTemplate.Click
        exceltemplate()
        Exit Sub
    End Sub

    Function exceltemplate() As Integer
        Dim rwStart As Integer = 0
        Dim oXL As Excel.Application
        Dim oWB As Excel.Workbook
        Dim oSheet As Excel.Worksheet
        Dim oRng As Excel.Range

        oXL = CreateObject("Excel.Application")
        oXL.Visible = True
        oWB = oXL.Workbooks.Add
        oSheet = oWB.ActiveSheet

        oSheet.Range("A1").Value = "ITEMNAME"
        oSheet.Range("A1").ColumnWidth = 14
        oSheet.Range("B1").Value = "SUBITEMNAME"
        oSheet.Range("B1").ColumnWidth = 14
        oSheet.Range("C1").Value = "FROMCENT"
        oSheet.Range("C1").ColumnWidth = 10
        oSheet.Range("D1").Value = "TOCENT"
        oSheet.Range("D1").ColumnWidth = 10
        oSheet.Range("E1").Value = "MAXRATE"
        oSheet.Range("E1").ColumnWidth = 10
        oSheet.Range("F1").Value = "MINRATE"
        oSheet.Range("F1").ColumnWidth = 10
        oSheet.Range("G1").Value = "PURRATE"
        oSheet.Range("G1").ColumnWidth = 10
        oSheet.Range("H1").Value = "SALESPER"
        oSheet.Range("H1").ColumnWidth = 10
        oSheet.Range("I1").Value = "COSTNAME"
        oSheet.Range("I1").ColumnWidth = 14
        oSheet.Range("J1").Value = "ACNAME"
        oSheet.Range("J1").ColumnWidth = 14
        oSheet.Range("K1").Value = "DESIGNER"
        oSheet.Range("K1").ColumnWidth = 14
        If _FourCMaintain Then
            oSheet.Range("L1").Value = "COLORNAME"
            oSheet.Range("L1").ColumnWidth = 12
            oSheet.Range("M1").Value = "CUTNAME"
            oSheet.Range("M1").ColumnWidth = 12
            oSheet.Range("N1").Value = "CLARITYNAME"
            oSheet.Range("N1").ColumnWidth = 12
            oSheet.Range("O1").Value = "SHAPENAME"
            oSheet.Range("O1").ColumnWidth = 12
        End If
        If _FourCMaintain Then
            oSheet.Range("A1:B1:C1:D1:E1:F1:G1:H1:I1:J1:K1:L1:M1:N1:O1").Font.Bold = True
        Else
            oSheet.Range("A1:B1:C1:D1:E1:F1:G1:H1:I1:J1:K1").Font.Bold = True
        End If

    End Function

    Private Sub BtnUpdate_Click(sender As Object, e As EventArgs) Handles BtnUpdate.Click
        BtnUpdate.Enabled = False
        For cnt As Integer = 0 To gridView.Rows.Count - 1
            Dim CId As String = ""
            CId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & gridView.Item("costname", cnt).Value.ToString & "'", , , tran)

            strSql = " DECLARE @WTFROM FLOAT,@WTTO FLOAT"
            strSql += " SET @WTFROM = " & Val(gridView.Item("FROMCENT", cnt).Value.ToString) & ""
            strSql += " SET @WTTO = " & Val(gridView.Item("TOCENT", cnt).Value.ToString) & ""
            strSql += " SELECT SLNO FROM " & cnAdminDb & "..CENTRATE WHERE "
            strSql += " ((@WTFROM BETWEEN FROMCENT AND TOCENT)"
            strSql += " OR"
            strSql += " (@WTTO BETWEEN FROMCENT AND TOCENT))"
            strSql += " AND ITEMID = ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & gridView.Item("ITEMNAME", cnt).Value.ToString & "'),0)"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & gridView.Item("SUBITEMNAME", cnt).Value.ToString & "'"
            strSql += " And ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & gridView.Item("ITEMNAME", cnt).Value.ToString & "')),0)"
            If _IsWholeSaleType Then strSql += " AND ISNULL(ACCODE,'') = ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView.Item("ACNAME", cnt).Value.ToString & "'),'')"
            If CENTRATE_DESIGN Then strSql += " AND ISNULL(DESIGNERID,'') = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & gridView.Item("DESIGNER", cnt).Value.ToString & "'),'')"
            If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" And CId <> "" Then strSql += " AND COSTID IN ('" & CId & "')"
            If _FourCMaintain Then
                strSql += " and COLORID = '" & objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & gridView.Item("COLORNAME", cnt).Value.ToString & "'") & "'" 'COLORID
                strSql += " and CUTID = '" & objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & gridView.Item("CUTNAME", cnt).Value.ToString & "'") & "'" 'CUTID
                strSql += " and CLARITYID = '" & objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & gridView.Item("CLARITYNAME", cnt).Value.ToString & "'") & "'" 'CLARITYID
                strSql += " and SHAPEID = '" & objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & gridView.Item("SHAPENAME", cnt).Value.ToString & "'") & "'" 'SHAPEID
            End If
            If Val(objGPack.GetSqlValue(strSql, "SLNO", "").ToString) > 0 Then
                UpdateExcelCentrate(CId, Val(objGPack.GetSqlValue(strSql, "SLNO", "").ToString), cnt)
                Continue For
            End If
            InsertExcelCentRate(CId, cnt)
        Next
        gridView.DataSource = Nothing
        BtnUpdate.Enabled = False
        MsgBox("Inserted Successfully")
        funcNew()
    End Sub

    Private Sub UpdateExcelCentrate(ByVal cId As String, ByVal vId As Integer, ByVal cnt As Integer)
        Dim ItemId As Integer = Nothing
        Dim SubItemId As Integer = Nothing

        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE"

        strSql += " ITEMNAME = '" & gridView.Item("ITEMNAME", cnt).Value.ToString & "'"
        ItemId = Val(objGPack.GetSqlValue(strSql))

        strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE"
        strSql += " SUBITEMNAME = '" & gridView.Item("SUBITEMNAME", cnt).Value.ToString & "'"
        strSql += " AND ITEMID = " & ItemId & ""
        SubItemId = Val(objGPack.GetSqlValue(strSql))
        strSql = " UPDATE " & cnAdminDb & "..CENTRATE SET"
        strSql += " ITEMID=" & ItemId & ""
        strSql += " ,SUBITEMID=" & SubItemId & ""
        strSql += " ,FROMCENT=" & Val(gridView.Item("FROMCENT", cnt).Value.ToString) & ""
        strSql += " ,TOCENT=" & Val(gridView.Item("TOCENT", cnt).Value.ToString) & ""
        strSql += " ,MAXRATE=" & Val(gridView.Item("MAXRATE", cnt).Value.ToString) & ""
        strSql += " ,MINRATE=" & Val(gridView.Item("MINRATE", cnt).Value.ToString) & ""
        strSql += " ,PURRATE=" & Val(gridView.Item("PURRATE", cnt).Value.ToString) & ""
        strSql += " ,USERID='" & userId & "'"
        strSql += " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,UPTIME='" & Date.Now.ToLongTimeString & "'"
        If _IsWholeSaleType Then
            strSql += " ,ACCODE = '" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView.Item("ACNAME", cnt).Value.ToString & "'") & "'" 'ACCODE
        End If
        If CENTRATE_DESIGN Then
            strSql += " ,DESIGNERID='" & objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & gridView.Item("DESIGNER", cnt).Value.ToString & "'") & "'" 'DESIGNERID
        End If
        If _FourCMaintain Then
            strSql += " ,COLORID = '" & objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & gridView.Item("COLORNAME", cnt).Value.ToString & "'") & "'" 'COLORID
            strSql += " ,CUTID = '" & objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & gridView.Item("CUTNAME", cnt).Value.ToString & "'") & "'" 'CUTID
            strSql += " ,CLARITYID = '" & objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & gridView.Item("CLARITYNAME", cnt).Value.ToString & "'") & "'" 'CLARITYID
            strSql += " ,SHAPEID = '" & objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & gridView.Item("SHAPENAME", cnt).Value.ToString & "'") & "'" 'SHAPEID
        End If
        strSql += " ,SALESPER=" & Val(gridView.Item("SALESPER", cnt).Value.ToString) & ""
        strSql += " WHERE SLNO = '" & vId & "'"
        Try
            ExecQuery(SyncMode.Transaction, strSql, cn, , cId)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub InsertExcelCentRate(ByVal cId As String, ByVal cnt As Integer)
        Dim ItemId As Integer = Nothing
        Dim SubItemId As Integer = Nothing
        Dim ColorId As Integer = 0
        Dim CutId As Integer = 0
        Dim ClarityId As Integer = 0
        Dim ShapeId As Integer = 0
        Dim StnGrpId As Integer = 0
        If _FourCMaintain Then
            ColorId = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & gridView.Item("COLORNAME", cnt).Value.ToString & "'", "COLORID", 0)
            CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & gridView.Item("CUTNAME", cnt).Value.ToString & "'", "CUTID", 0)
            ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & gridView.Item("CLARITYNAME", cnt).Value.ToString & "'", "CLARITYID", 0)
            ShapeId = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & gridView.Item("SHAPENAME", cnt).Value.ToString & "'", "SHAPEID", 0)
            ''StnGrpId = objGPack.GetSqlValue("SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & gridView.Item("STNGROUPNAME", cnt).Value.ToString & "'", "GROUPID", 0)
        End If
        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE"
        strSql += " ITEMNAME = '" & gridView.Item("ITEMNAME", cnt).Value.ToString & "'"
        ItemId = Val(objGPack.GetSqlValue(strSql, , , tran))

        strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE"
        strSql += " SUBITEMNAME = '" & gridView.Item("SUBITEMNAME", cnt).Value.ToString & "'"
        strSql += " AND ITEMID = " & ItemId & ""
        SubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
        strSql = " INSERT INTO " & cnAdminDb & "..CENTRATE"
        strSql += " (COSTID,ITEMID,SUBITEMID,FROMCENT,TOCENT,"
        strSql += " MAXRATE,MINRATE,PURRATE,"
        strSql += " USERID,UPDATED,UPTIME,ACCODE,DESIGNERID"
        If _FourCMaintain Then
            strSql += " ,COLORID,CUTID,CLARITYID,SHAPEID,STNGRPID"
        End If
        strSql += " ,SALESPER"
        strSql += " )VALUES("
        strSql += " '" & cId & "'" 'costid
        strSql += " ," & ItemId & "" 'ItemId
        strSql += " ," & SubItemId & "" 'SubItemId
        strSql += " ," & Val(gridView.Item("FROMCENT", cnt).Value.ToString) & "" 'FromCent
        strSql += " ," & Val(gridView.Item("TOCENT", cnt).Value.ToString) & "" 'ToCent
        strSql += " ," & Val(gridView.Item("MAXRATE", cnt).Value.ToString) & "" 'Maxrate
        strSql += " ," & Val(gridView.Item("MINRATE", cnt).Value.ToString) & "" 'MinRate
        strSql += " ," & Val(gridView.Item("PURRATE", cnt).Value.ToString) & "" 'PurRate
        strSql += " ,'" & userId & "'" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += " ,'" & IIf(_IsWholeSaleType, objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & gridView.Item("ACNAME", cnt).Value.ToString & "'"), "") & "'" 'ACCODE
        strSql += " ,'" & IIf(CENTRATE_DESIGN, objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & gridView.Item("DESIGNER", cnt).Value.ToString & "'"), "") & "'" 'DESIGNERID
        If _FourCMaintain Then
            strSql += " ," & ColorId & ""
            strSql += " ," & CutId & ""
            strSql += " ," & ClarityId & ""
            strSql += " ," & ShapeId & ""
            strSql += " ," & StnGrpId & ""
        End If
        strSql += " ," & Val(gridView.Item("SALESPER", cnt).Value.ToString) & "" 'SALESPER
        strSql += " )"
        Try
            ExecQuery(SyncMode.Transaction, strSql, cn, , cId)
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Dim lbltitle1 As String = "Cent Rate Details "
        Dim formattedDate As String = DateTime.Now.ToString("dd/MM/yyyy  hh:mm:ss tt")
        Dim lbltitle = lbltitle1 + formattedDate
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lbltitle, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
        btnExport.Enabled = False
    End Sub
End Class