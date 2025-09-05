'CALNO 598 -VASANTHAN, CLIENT-Senthil Murugan
Imports System.Data.OleDb
Public Class frmStockReorder
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim flagUpdate As Boolean = False
    Dim Sno As String = Nothing
    Dim designReorder As Boolean = IIf(GetAdmindbSoftValue("DESIGNREORDER", "N") = "N", False, True)
    Dim designReorderFix As Boolean = IIf(GetAdmindbSoftValue("REORDERFIX", "Y") = "Y", True, False)
    Dim REORD_AVG_LOCK As Boolean = IIf(GetAdmindbSoftValue("REORD_AVG_LOCK", "Y") = "Y", True, False)

    Dim blnValid As Boolean = False
    Dim blnIsCheckValid As Boolean = False

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        funcGridStyle(gridView)
        'cmbSubItemSearch_OWN.Items.Add("ALL")
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT SNO,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID=SR.ITEMID)AS ITEMNAME,"
        strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.SUBITEMID=SR.SUBITEMID),'')AS SUBITEMNAME,"
        strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE AS C WHERE C.COSTID = SR.COSTID),'')AS COSTNAME,"
        strSql += " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = SR.ITEMCTRID)AS ITEMCTRNAME,"
        strSql += " FROMWEIGHT,TOWEIGHT,PIECE,WEIGHT "
        strSql += " ,(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = SR.SIZEID)AS SIZENAME"
        If designReorder = True Then
            strSql += " ,(Select DesignerName from " & cnAdminDb & "..DESIGNER Where DesignerId=SR.DesignId) as DESIGNERNAME"
        End If
        strSql += " ,CASE WHEN RANGEMODE='W' THEN 'Weight Range' ELSE 'Rate Range' END RANGEMODE"
        strSql += " ,ISNULL(RANGECAPTION,'') CAPTION"
        strSql += " FROM " & cnAdminDb & "..STKREORDER AS SR WHERE FROMDAY IS NULL"
        funcOpenGrid(strSql, gridView)
        gridView.Columns("SNO").Visible = False
        gridView.Columns("ITEMNAME").Width = 200
        gridView.Columns("SUBITEMNAME").Width = 150
        gridView.Columns("COSTNAME").Width = 150
        gridView.Columns("ITEMCTRNAME").Width = 150
        gridView.Columns("FROMWEIGHT").HeaderText = "FROMWEIGHT"
        gridView.Columns("FROMWEIGHT").Width = 90
        gridView.Columns("FROMWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        gridView.Columns("TOWEIGHT").HeaderText = "TOWEIGHT"
        gridView.Columns("TOWEIGHT").Width = 90
        gridView.Columns("TOWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        If designReorderFix = True Then
            gridView.Columns("PIECE").Visible = False
            gridView.Columns("WEIGHT").Visible = False
        End If
        If designReorder = True Then
            gridView.Columns("DESIGNERNAME").HeaderText = "DESIGNER"
            gridView.Columns("DESIGNERNAME").Width = 150
        End If
        gridView.Columns("PIECE").HeaderText = "PIECE"
        gridView.Columns("PIECE").Width = 70
        gridView.Columns("PIECE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        gridView.Columns("WEIGHT").HeaderText = "WEIGHT"
        gridView.Columns("WEIGHT").Width = 70
        gridView.Columns("WEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("SIZENAME").HeaderText = "SIZE"
        gridView.Columns("SIZENAME").Width = 100
        gridView.Columns("CAPTION").HeaderText = "CAPTION"
        gridView.Columns("CAPTION").Width = 100

    End Function

    Function funcNew() As Integer
        objGPack.TextClear(Me)
        flagSave = False
        flagUpdate = False
        funcLoadMetalName()
        funcLoaditemCounter()
        funcLoadCostCentre()
        If designReorderFix Then
            Me.grpDesignOrder.Visible = True
            cmbDesignerName.Enabled = True
            funcLoadDesignerName(cmbDesignerName)
            minLbl.Visible = False
            lblReOrderPce.Visible = False
            lblWt.Visible = False
            txtPiece.Visible = False
            txtWeight.Visible = False
            txtReOrder.Visible = False
        End If
        If REORD_AVG_LOCK Then
            txtWeight.Enabled = False
        End If

        If designReorder Then
            Me.grpDesignOrder.Visible = True
            minLbl.Visible = True
            lblReOrderPce.Visible = True
            lblWt.Visible = True
            txtPiece.Visible = True
            txtWeight.Visible = True
            txtReOrder.Visible = True
            Me.minLbl.Text = "Max.Piece"
            txtLeadTime.Visible = True
            cmbDesignerName.Enabled = True
            funcLoadDesignerName(cmbDesignerName)
        End If
        funcCallGrid()
        cmbMetal_Man.Focus()
        cmbRangeMode.Text = "W"
    End Function
    Function funcLoadDesignerName(ByVal combo As ComboBox) As Integer
        strSql = " Select DesignerName from " & cnAdminDb & "..Designer order by DesignerName"
        objGPack.FillCombo(strSql, combo, , False)
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If txtWtFrom_Own.Text = "" Then
            MsgBox(Me.GetNextControl(txtWtFrom_Own, False).Text + E0001, MsgBoxStyle.Information)
            txtWtFrom_Own.Focus()
            Exit Function
        End If
        If txtWtTo.Text = "" Then
            MsgBox(Me.GetNextControl(txtWtTo, False).Text + E0001, MsgBoxStyle.Information)
            txtWtTo.Focus()
            Exit Function
        End If
        If Not Val(txtWtFrom_Own.Text) <= Val(txtWtTo.Text) Then
            MsgBox(E0005, MsgBoxStyle.Information)
            txtWtFrom_Own.Focus()
            Exit Function
        End If
        If designReorderFix = False Then
            If Val(txtPiece.Text) = 0 Then
                MsgBox(Me.GetNextControl(txtPiece, False).Text + E0001, MsgBoxStyle.Information)
                txtPiece.Focus()
                Exit Function
            End If
        End If

        'If funcUniqueValidation() = True Then
        '    MsgBox(E0002, MsgBoxStyle.Information)
        '    txtWtFrom.Focus()
        '    Exit Function
        'End If
        If flagSave = False Then
            funcAdd()
        Else
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        Dim ItemId As Integer = Nothing
        Dim ItemCtrId As Integer = Nothing
        Dim DesignId As Integer = Nothing
        Dim SubItemId As Integer = Nothing
        Dim COSTID As String = Nothing
        Dim ds As New Data.DataSet
        ds.Clear()
        ''Find ItemId
        strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItem_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "ItemId")
        If ds.Tables("ItemId").Rows.Count > 0 Then
            ItemId = Val(ds.Tables("Itemid").Rows(0).Item("ItemId").ToString)
        Else
            ItemId = 0
        End If
        ''Find ItemCtrId
        strSql = " Select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName = '" & cmbItemCounter.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "ItemCtrId")
        If ds.Tables("ItemCtrId").Rows.Count > 0 Then
            ItemCtrId = Val(ds.Tables("ItemCtrId").Rows(0).Item("ItemCtrId").ToString)
        Else
            ItemCtrId = 0
        End If
        If designReorderFix = True Then
            strSql = " Select DESIGNERID from " & cnAdminDb & "..DESIGNER where DESIGNERNAME = '" & cmbDesignerName.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "Designid")
            If ds.Tables("Designid").Rows.Count > 0 Then
                DesignId = Val(ds.Tables("Designid").Rows(0).Item("DESIGNERID").ToString)
            Else
                DesignId = 0
            End If
        End If
        If designReorder = True Then
            strSql = " Select DESIGNERID from " & cnAdminDb & "..DESIGNER where DESIGNERNAME = '" & cmbDesignerName.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "Designid")
            If ds.Tables("Designid").Rows.Count > 0 Then
                DesignId = Val(ds.Tables("Designid").Rows(0).Item("DESIGNERID").ToString)
            Else
                DesignId = 0
            End If
            If txtLeadTime.Text <> "" Then txtLeadTime.Text = "0"
        End If

        ''Find SubItemId
        If cmbSubItem_Man.Text <> "ALL" Then
            strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName = '" & cmbSubItem_Man.Text & "' AND ITEMID = " & ItemId & ""
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "SubItemId")
            If ds.Tables("SubItemId").Rows.Count > 0 Then
                SubItemId = Val(ds.Tables("SubItemId").Rows(0).Item("SubItemid").ToString)
            Else
                SubItemId = 0
            End If
        Else
            SubItemId = 0
        End If

        ''Find COSTID
        '598
        If cmbCostCentre_own.Text <> "" Then
            strSql = " Select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre_own.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "CostId")
            If ds.Tables("CostId").Rows.Count > 0 Then
                COSTID = ds.Tables("CostId").Rows(0).Item("CostId")
            Else
                COSTID = ""
            End If
        Else
            COSTID = ""
        End If

        If COSTID = "" Then
            strSql = "  SELECT ISNULL(MAX(CAST(SUBSTRING(SNO,6,20)AS INT)),0)+1  AS SNO FROM " & cnAdminDb & "..STKREORDER "
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            Sno = GetCostId(cnCostId) + GetCompanyId(strCompanyId) + IIf(Val(dt.Rows(0).Item("SNO").ToString) = 0, 1, Val(dt.Rows(0).Item("SNO").ToString)).ToString
        Else
            Sno = GetNewSno(TranSnoType.STKREORDERCODE, tran, "GET_SNO_ADMIN")
        End If


        strSql = " Insert into " & cnAdminDb & "..StkReorder"
        strSql += " ("
        strSql += " SNO,ItemId,SubItemId,ItemCtrId,COSTID,"
        strSql += " fromweight,toweight,"
        strSql += " piece,weight,Userid,"
        If designReorderFix = True Then
            strSql += "DESIGNID,"
        ElseIf designReorder = True Then
            strSql += "MINPIECE,DESIGNID,LEADTIME,"
        End If

        strSql += " Updated,Uptime,sizeid,RangeMode,RangeCaption)Values ("
        strSql += " '" & Sno & "'"
        strSql += " ," & ItemId & "" 'ItemId
        strSql += " ," & SubItemId & "" 'SubItemId
        strSql += " ," & ItemCtrId & "" 'ItemCtrId
        strSql += " ,'" & COSTID & "'" 'COSTID
        strSql += " ," & Val(txtWtFrom_Own.Text) & "" 'fromweight
        strSql += " ," & Val(txtWtTo.Text) & "" 'toweight
        strSql += " ," & Val(txtPiece.Text) & "" 'piece
        strSql += " ," & Val(txtWeight.Text) & "" 'weight
        strSql += " ," & userId & "" 'Userid
        If designReorderFix = True Then
            strSql += "," & DesignId & ""
        ElseIf designReorder = True Then
            strSql += "," & Val("" & txtReOrder.Text) & "," & DesignId
            strSql += "," & Val("" & txtLeadTime.Text)
        End If
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += " ," & Val(objGPack.GetSqlValue("select sizeid from " & cnAdminDb & "..itemsize where sizename = '" & cmbSize.Text & "' and itemid=" & ItemId).ToString) & "" 'sizeid
        strSql += " ,'" & Mid(cmbRangeMode.Text, 1, 1) & "'" 'RangeMode
        strSql += " ,'" & txtStkReordCaption.Text & "'" 'RangeCaption
        strSql += " )"
        Try
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            funcCallGrid()
            txtWtFrom_Own.Clear()
            txtWtTo.Clear()
            txtPiece.Clear()
            txtWeight.Clear()
            txtReOrder.Clear()
            txtWtFrom_Own.Select()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate() As Integer
        Dim ItemId As Integer = Nothing
        Dim ItemCtrId As Integer = Nothing
        Dim Designid As Integer = Nothing
        Dim SubItemId As Integer = Nothing
        Dim COSTID As String = Nothing
        Dim ds As New Data.DataSet
        ds.Clear()
        ''Find ItemId
        strSql = " Select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItem_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "ItemId")
        If ds.Tables("ItemId").Rows.Count > 0 Then
            ItemId = Val(ds.Tables("Itemid").Rows(0).Item("ItemId").ToString)
        Else
            ItemId = 0
        End If

        ''Find ItemCtrId
        strSql = " Select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName = '" & cmbItemCounter.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "ItemCtrId")
        If ds.Tables("ItemCtrId").Rows.Count > 0 Then
            ItemCtrId = Val(ds.Tables("ItemCtrId").Rows(0).Item("ItemCtrId").ToString)
        Else
            ItemCtrId = 0
        End If

        ''Find SubItemId
        '598
        If cmbSubItem_Man.Text <> "ALL" Then
            strSql = " Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName = '" & cmbSubItem_Man.Text & "' AND ITEMID = " & ItemId & ""
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "SubItemId")
            If ds.Tables("SubItemId").Rows.Count > 0 Then
                SubItemId = Val(ds.Tables("SubItemId").Rows(0).Item("SubItemid").ToString)
            Else
                SubItemId = 0
            End If
        Else
            SubItemId = 0
        End If
        If designReorderFix = True Then
            strSql = " Select DESIGNERID from " & cnAdminDb & "..DESIGNER where DESIGNERNAME = '" & cmbDesignerName.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "DESIGNERID")
            If ds.Tables("DESIGNERID").Rows.Count > 0 Then
                Designid = Val(ds.Tables("DESIGNERID").Rows(0).Item("DESIGNERID").ToString)
            Else
                Designid = 0
            End If
        End If
        If designReorder = True Then
            strSql = " Select DESIGNERID from " & cnAdminDb & "..DESIGNER where DESIGNERNAME = '" & cmbDesignerName.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "DESIGNERID")
            If ds.Tables("DESIGNERID").Rows.Count > 0 Then
                Designid = Val(ds.Tables("DESIGNERID").Rows(0).Item("DESIGNERID").ToString)
            Else
                Designid = 0
            End If
        End If

        ''Find COSTID
        If cmbCostCentre_own.Text <> "" Then
            strSql = " Select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre_own.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "CostId")
            If ds.Tables("CostId").Rows.Count > 0 Then
                COSTID = ds.Tables("CostId").Rows(0).Item("CostId")
            Else
                COSTID = ""
            End If
        Else
            COSTID = ""
        End If

        strSql = " Update " & cnAdminDb & "..StkReorder Set"
        strSql += " ItemId= " & ItemId & ""
        strSql += " ,SubItemId= " & SubItemId & ""
        strSql += " ,ItemCtrid= " & ItemCtrId & ""
        strSql += " ,COSTID='" & COSTID & "'"
        strSql += " ,fromweight=" & Val(txtWtFrom_Own.Text) & ""
        strSql += " ,toweight=" & Val(txtWtTo.Text) & ""
        strSql += " ,piece=" & Val(txtPiece.Text) & ""
        strSql += " ,weight=" & Val(txtWeight.Text) & ""
        If designReorderFix = True Then
            strSql += " ,DESIGNID=" & Designid & ""
        ElseIf designReorder = True Then
            strSql += " ,MINPIECE=" & Val(txtReOrder.Text) & ",DESIGNID=" & Designid
            strSql += " ,LEADTIME=" & Val(txtLeadTime.Text)
        End If
        strSql += " ,Userid=" & userId & ""
        strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        strSql += " ,sizeid = " & Val(objGPack.GetSqlValue("select sizeid from " & cnAdminDb & "..itemsize where sizename = '" & cmbSize.Text & "' and itemid =" & ItemId).ToString) & "" 'sizeid
        strSql += ",RangeMode='" & Mid(cmbRangeMode.Text, 1, 1) & "'"
        strSql += ",RangeCaption='" & txtStkReordCaption.Text & "'"
        strSql += " where Sno = '" & Sno & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            flagSave = False
            flagUpdate = False

            funcCallGrid()
            txtWtFrom_Own.Clear()
            txtWtTo.Clear()
            txtPiece.Clear()
            txtWeight.Clear()
            txtReOrder.Clear()
            txtWtFrom_Own.Select()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal temp As String) As String
        strSql = " SELECT"
        strSql += " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST AS M, " & cnAdminDb & "..ITEMMAST IM WHERE M.METALID=IM.METALID AND IM.ITEMID=SR.ITEMID)AS METALNAME,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID=SR.ITEMID)AS ITEMNAME,"
        strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S WHERE S.SUBITEMID=SR.SUBITEMID),'')AS SUBITEMNAME,"
        strSql += " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER AS C WHERE C.ITEMCTRID=SR.ITEMCTRID)AS ITEMCTRNAME,"
        strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE AS C WHERE C.COSTID = SR.COSTID),'')AS COSTNAME,"
        strSql += " ISNULL((SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE AS C WHERE C.SIZEID = SR.SIZEID),'')AS SIZENAME,"
        'IF DESIGNREORDERFIX = TRUE THEN
        '    STRSQL += " ISNULL((SELECT DESIGNERNAME FROM " & CNADMINDB & "..DESIGNER AS DS WHERE DS.DESIGNERID= SR.DESIGNID),'')AS DESIGNERNAME,"
        If designReorder = True Then
            strSql += " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER AS DS WHERE DS.DESIGNERID= SR.DESIGNID),'')AS DESIGNERNAME,"

        End If
        strSql += " FROMWEIGHT,TOWEIGHT,PIECE,WEIGHT"
        'IF DESIGNREORDERFIX = TRUE THEN
        '    STRSQL += ",DESIGNID"
        If designReorder = True Then
            strSql += ",MINPIECE,DESIGNID,LEADTIME"
        End If
        strSql += " ,CASE WHEN RANGEMODE='W' THEN 'Weight Range' ELSE 'Rate Range' END RANGEMODE"
        strSql += " ,ISNULL(RANGECAPTION,'') CAPTION"
        strSql += " FROM " & cnAdminDb & "..STKREORDER AS SR"
        strSql += " WHERE SNO = '" & temp & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        flagSave = True
        flagUpdate = True
        With dt.Rows(0)
            cmbMetal_Man.Text = .Item("MetalName").ToString

            strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
            strSql += " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal_Man.Text & "')"
            strSql += " ORDER BY ITEMNAME"
            objGPack.FillCombo(strSql, cmbItem_Man)

            cmbItem_Man.Text = .Item("ItemName").ToString
            cmbItemCounter.Text = .Item("ItemCtrName").ToString
            cmbSubItem_Man.Text = .Item("SubItemName").ToString
            cmbCostCentre_own.Text = .Item("CostName").ToString
            cmbSize.Text = .Item("SIZENAME").ToString
            txtWtFrom_Own.Text = .Item("fromweight").ToString
            txtWtTo.Text = .Item("toweight").ToString
            txtPiece.Text = .Item("piece").ToString
            txtWeight.Text = .Item("weight").ToString
            'If designReorderFix = True Then
            '    Me.cmbDesignerName.Text = .Item("Designername").ToString
            If designReorder = True Then
                txtReOrder.Text = .Item("Minpiece").ToString
                Me.cmbDesignerName.Text = .Item("Designername").ToString
                txtLeadTime.Text = .Item("LEADTIME").ToString
            End If
            cmbRangeMode.Text = .Item("RangeMode").ToString
            txtStkReordCaption.Text = .Item("CAPTION").ToString.Trim
        End With
        Sno = temp
    End Function
    Function funcLoaditemCounter() As Integer
        strSql = " Select ItemCtrName from " & cnAdminDb & "..Itemcounter Order by ItemCtrName"
        objGPack.FillCombo(strSql, cmbItemCounter)
    End Function
    Function funcLoadMetalName() As Integer
        strSql = " Select Metalname from " & cnAdminDb & "..MetalMast Order by DISPLAYORDER"
        objGPack.FillCombo(strSql, cmbMetal_Man)
    End Function
    Function funcLoadCostCentre() As Integer
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            strSql = " select CostName from " & cnAdminDb & "..CostCentre order by CostName"
            objGPack.FillCombo(strSql, cmbCostCentre_own)
            cmbCostCentre_own.Enabled = True
        Else
            cmbCostCentre_own.Enabled = False
        End If
    End Function
    Function funcUniqueValidation() As Boolean
        strSql = " Declare @wtFrom as float,@wtTo as Float"
        strSql += " Set @wtFrom = " & Val(txtWtFrom_Own.Text) & ""
        strSql += " Set @wtTo = " & Val(txtWtTo.Text) & ""
        strSql += " select 1 from " & cnAdminDb & "..StkReorder"
        strSql += " where "
        strSql += " ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItem_Man.Text & "')"
        strSql += " and SubItemId = isnull((select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_Man.Text & "')),0)"
        If cmbCostCentre_own.Text <> "" Then strSql += " and COSTID = isnull((select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre_own.Text & "'),'')"
        strSql += " and"
        strSql += " ((fromWeight between @wtFrom and @wtTo) OR (ToWeight between @wtFrom and @wtTo))"
        If cmbSize.Text <> "" Then strSql += " and ISNULL(sizeid,0) = ISNULL((select sizeid FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & cmbSize.Text & "' and ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItem_Man.Text & "')),0)"
        If designReorder = True Then
            If cmbDesignerName.Text <> "" Then strSql += " and ISNULL(DESIGNID,0) = ISNULL((select DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesignerName.Text & "'),0)"
        End If

        If blnValid = False Then
            If flagSave = True Then
                strSql += " and sno <> '" & Sno & "'"
            End If
        End If
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return True
            'MsgBox("Already Exist...")
        End If
        Return False
    End Function

    Private Sub frmStockReorder_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
        'If e.KeyChar = Chr(Keys.F9) Then
        '    cmbCostSearch.Focus()
        'End If
    End Sub
    Private Sub frmStockReorder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcNew()
        cmbRangeMode.Text = "W"
        SearchCostCentre(cmbCostSearch_OWN)
        SearchItem(cmbItemSearch_OWN)
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub
    Private Sub NEwToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NEwToolStripMenuItem.Click
        funcNew()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        cmbCostSearch_OWN.Focus()
        'gridView.Focus()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub
    Private Sub cmbMetal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetal_Man.SelectedIndexChanged
        If flagSave = True Then
            Exit Sub
        End If
        strSql = " Select ItemName from " & cnAdminDb & "..ItemMast"
        strSql += " where METALID = (select Metalid from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetal_Man.Text & "')"
        strSql += " Order by ItemName"
        objGPack.FillCombo(strSql, cmbItem_Man)
    End Sub


    Private Sub cmbItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_Man.SelectedIndexChanged
        cmbSubItem_Man.Text = ""
        '598
        'cmbSubItem_Man.Items.Add("ALL")
        strSql = " SELECT 'ALL' SUBITEMNAME,1 RESULT UNION ALL"
        strSql += " SELECT SUBITEMNAME,2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST WHERE "
        strSql += " ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_Man.Text & "' AND SUBITEM = 'Y')"
        strSql += " ORDER BY RESULT,SUBITEMNAME"
        If flagSave = True Then
            objGPack.FillCombo(strSql, cmbSubItem_Man, True, False)
        Else
            objGPack.FillCombo(strSql, cmbSubItem_Man, True)
        End If
        If cmbSubItem_Man.Items.Count > 0 Then cmbSubItem_Man.Enabled = True Else cmbSubItem_Man.Enabled = False

        cmbSize.Text = ""
        strSql = " SELECT SIZESTOCK FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_Man.Text & "'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE "
            strSql += " WHERE ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_Man.Text & "')"
            strSql += " ORDER BY SIZENAME"
            If flagSave = True Then
                objGPack.FillCombo(strSql, cmbSize, True, False)
            Else
                objGPack.FillCombo(strSql, cmbSize, True)
            End If
        End If
        If cmbSize.Items.Count > 0 Then cmbSize.Enabled = True Else cmbSize.Enabled = False

        Dim Sql As String = "Select RangeMode from " & cnAdminDb & "..STKREORDER where ItemId= (Select ItemId from " & cnAdminDb & "..ItemMast Where ItemName='" & cmbItem_Man.Text & "')"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(Sql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0).Item("RangeMode").ToString = "R" Then
                lblWtFrom.Text = "From Rate"
                lblWtTo.Text = "To"
                lblWt.Text = "AvgRt"
                cmbRangeMode.Text = "R"
                If gridView.Rows.Count > 0 Then
                    gridView.Columns(5).HeaderText = "FROMRATE"
                    gridView.Columns(6).HeaderText = "TORATE"
                    gridView.Columns(7).HeaderText = "AVG RATE"
                End If
            ElseIf dt.Rows(0).Item("RangeMode").ToString = "W" Then
                lblWtFrom.Text = "FromWeight"
                lblWtTo.Text = "To"
                lblWt.Text = "AvgWt"
                cmbRangeMode.Text = "W"
                If gridView.Rows.Count > 0 Then
                    gridView.Columns(5).HeaderText = "FROMWEIGHT"
                    gridView.Columns(6).HeaderText = "TOWEIGHT"
                    gridView.Columns(7).HeaderText = "AVG WEIGHT"
                End If
            Else

                Dim sSql As String = "Select CalType from " & cnAdminDb & "..ItemMast where CalType in ('R','F','M','B')"
                sSql += " and ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItem_Man.Text & "')"
                Dim dts As New DataTable
                dts.Clear()
                da = New OleDbDataAdapter(sSql, cn)
                da.Fill(dts)
                If dts.Rows.Count > 0 Then
                    lblWtFrom.Text = "From Rate"
                    lblWtTo.Text = "To"
                    lblWt.Text = "Rate"
                    cmbRangeMode.Text = "R"
                    If gridView.Rows.Count > 0 Then
                        gridView.Columns(5).HeaderText = "FROMRATE"
                        gridView.Columns(6).HeaderText = "TORATE"
                        gridView.Columns(7).HeaderText = "AVG RATE"
                    End If
                End If

                Dim sSql1 As String = "Select CalType from " & cnAdminDb & "..ItemMast where CalType in ('W')"
                sSql1 += " and ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItem_Man.Text & "')"
                Dim dts1 As New DataTable
                dts1.Clear()
                da = New OleDbDataAdapter(sSql1, cn)
                da.Fill(dts1)
                If dts1.Rows.Count > 0 Then
                    lblWtFrom.Text = "FromWeight"
                    lblWtTo.Text = "To"
                    lblWt.Text = "Weight"
                    cmbRangeMode.Text = "W"
                    If gridView.Rows.Count > 0 Then
                        gridView.Columns(5).HeaderText = "FROMWEIGHT"
                        gridView.Columns(6).HeaderText = "TOWEIGHT"
                        '598
                        gridView.Columns(7).HeaderText = "WEIGHT"
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value)
                cmbMetal_Man.Focus()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            cmbMetal_Man.Focus()
        End If
        If e.KeyCode = Keys.Delete Then
            If gridView.Rows.Count > 0 Then
                'Dim result = MessageBox.Show("Do you want to Delete...", "Message", MessageBoxButtons.YesNo)
                'If result = Windows.Forms.DialogResult.Yes Then
                Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..STKREORDER WHERE 1<>1"
                Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString
                DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..STKREORDER WHERE SNO = '" & delKey & "' ")
                'strSql = " Delete from " & cnAdminDb & "..STKREORDER where Sno='" & gridView("SNo", gridView.CurrentRow.Index).Value & "'"
                'cmd = New OleDbCommand(strSql, cn)
                'If Not tran Is Nothing Then cmd.Transaction = tran
                'ExecQuery(SyncMode.Master, strSql, cn, tran)
                funcCallGrid()
                'funcNew()
                Call btnSearch_Click(sender, e)
                'ElseIf result = Windows.Forms.DialogResult.No Then
                '   Exit Sub
                'End If
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..STKREORDER WHERE 1<>1"
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..STKREORDER WHERE SNO = '" & delKey & "' ")
        funcCallGrid()
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub
    Private Sub txtPiece_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPiece.GotFocus
        If funcUniqueValidation() = True Then
            txtWtFrom_Own.Focus()
            MsgBox(E0002, MsgBoxStyle.Information)
            Exit Sub
        End If
        calcPWt()
    End Sub   

    Private Sub txtWtTo_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWtTo.Leave
        calcWt()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim calWt As Boolean = False
        Dim calRate As Boolean = False
        Dim rangeMode As Boolean = False
        strSql = " Select RangeMode from " & cnAdminDb & "..STKREORDER Where ItemId=(Select ItemId from " & cnAdminDb & "..ItemMast Where ItemName='" & cmbItemSearch_OWN.Text & "')"
        Dim dta As New DataTable
        dta.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dta)
        If dta.Rows.Count > 0 Then
            If dta.Rows(0).Item("RangeMode").ToString = "R" Then
                calRate = True
            ElseIf dta.Rows(0).Item("RangeMode").ToString = "W" Then
                calWt = True
            Else
                Dim sSql As String = "Select CalType from " & cnAdminDb & "..ItemMast where CalType in ('R','F','M','B')"
                sSql += " and ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemSearch_OWN.Text & "')"
                Dim dts As New DataTable
                dts.Clear()
                da = New OleDbDataAdapter(sSql, cn)
                da.Fill(dts)
                If dts.Rows.Count > 0 Then
                    rangeMode = True
                End If

                Dim sSql1 As String = "Select CalType from " & cnAdminDb & "..ItemMast where CalType in ('W')"
                sSql1 += " and ItemId = (select ItemId from " & cnAdminDb & "..ItemMast where ItemName = '" & cmbItemSearch_OWN.Text & "')"
                Dim dts1 As New DataTable
                dts1.Clear()
                da = New OleDbDataAdapter(sSql1, cn)
                da.Fill(dts1)
                If dts1.Rows.Count > 0 Then
                    rangeMode = False
                End If
            End If
        End If

        strSql = " SELECT SNO,"
        strSql += " ITEMNAME, "
        strSql += " SUBITEMNAME,"
        strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE AS C WHERE C.COSTID = SR.COSTID),'')AS COSTNAME,"
        strSql += " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = SR.ITEMCTRID)AS ITEMCTRNAME,"
        If calWt = True Then
            strSql += " FROMWEIGHT AS FROMWEIGHT,"
            strSql += " TOWEIGHT as TOWEIGHT,"
            strSql += " PIECE,"
        ElseIf calRate = True Then
            strSql += " FROMWEIGHT AS FROMRATE,"
            strSql += " TOWEIGHT as TORATE,"
            strSql += " PIECE,"
        ElseIf rangeMode = True Then
            'lblWtFrom.Text = "From Rate"
            'lblWtTo.Text = "To"
            'lblWt.Text = "Rate"
            'cmbRangeMode.Text = "R"
            strSql += " FROMWEIGHT AS FROMRATE,"
            strSql += " TOWEIGHT as TORATE,"
            strSql += " PIECE,"
        ElseIf rangeMode = False Then
            strSql += " FROMWEIGHT AS FROMWEIGHT,"
            strSql += " TOWEIGHT as TOWEIGHT,"
            strSql += " PIECE,"
        End If
        If calWt = True Then
            strSql += " WEIGHT as [AVG WEIGHT],"
        ElseIf calRate = True Then
            strSql += " WEIGHT as [AVG RATE],"
        ElseIf rangeMode = True Then
            strSql += " WEIGHT as [AVG RATE],"
        ElseIf rangeMode = False Then
            strSql += " WEIGHT as [AVG WEIGHT],"
        End If
        strSql += " (SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = SR.SIZEID)AS SIZENAME"
        strSql += " FROM " & cnAdminDb & "..STKREORDER AS SR"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST I on I.ITEMID=SR.ITEMID"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST S on S.SUBITEMID=SR.SUBITEMID"
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..COSTCENTRE C on C.COSTID=SR.COSTID"
        If UCase(cmbSubItemSearch_OWN.Text) = "ALL" Then
            If cmbCostSearch_OWN.Text <> "" Or cmbItemSearch_OWN.Text <> "" Or cmbSubItemSearch_OWN.Text <> "" Then
                strSql += " where 1=1 "
                If cmbCostSearch_OWN.Text <> "" And cmbCostSearch_OWN.Text <> "ALL" Then
                    strSql += " and C.CostName='" & cmbCostSearch_OWN.Text & "'"
                End If
                strSql += " and I.ItemName='" & cmbItemSearch_OWN.Text & "' AND FROMDAY IS NULL"
                'strSql += " and S.SubItemName='" & cmbSubItemSearch.Text & "'"
                funcOpenGrid(strSql, gridView)
                With gridView
                    .Columns("SNO").Width = 120
                    .Columns("ITEMNAME").Width = 200
                    .Columns("SUBITEMNAME").Width = 200
                    .Columns("COSTNAME").Width = 120
                    .Columns("ITEMCTRNAME").Width = 150
                End With
                gridView.Select()
                gridView.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
            'ElseIf cmbCostSearch.Text <> "" Or (cmbItemSearch.Text <> "" And cmbSubItemSearch.Text <> "") Then
        Else
            If cmbCostSearch_OWN.Text <> "" Or cmbItemSearch_OWN.Text <> "" Or cmbSubItemSearch_OWN.Text <> "" Then
                strSql += " where 1=1"
                If cmbCostSearch_OWN.Text <> "" And cmbCostSearch_OWN.Text <> "ALL" Then
                    strSql += " and C.CostName='" & cmbCostSearch_OWN.Text & "'"
                End If
                strSql += " and I.ItemName='" & cmbItemSearch_OWN.Text & "'"
                strSql += " and S.SubItemName='" & cmbSubItemSearch_OWN.Text & "' AND FROMDAY IS NULL"
                funcOpenGrid(strSql, gridView)
                With gridView
                    .Columns("SNO").Width = 120
                    .Columns("ITEMNAME").Width = 200
                    .Columns("SUBITEMNAME").Width = 200
                    .Columns("COSTNAME").Width = 120
                    .Columns("ITEMCTRNAME").Width = 150
                End With
                gridView.Select()
                gridView.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
        End If
    End Sub

    Private Function SearchCostCentre(ByVal combo As ComboBox) As Integer
        strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
        objGPack.FillCombo(strSql, combo, , False)
    End Function
    Private Function SearchItem(ByVal combo As ComboBox) As Integer
        cmbSubItemSearch_OWN.Items.Add("ALL")
        strSql = " Select ItemName from " & cnAdminDb & "..ItemMast order by ItemName"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dt As New DataTable()
        da.Fill(dt)
        cmbItemSearch_OWN.DataSource = dt
        cmbItemSearch_OWN.DisplayMember = "ItemName"
        cmbItemSearch_OWN.ValueMember = "ItemName"
        'objGPack.FillCombo(strSql, combo, , False)
        'cmbSubItemSearch_OWN.Items.Add("ALL")
    End Function
    Private Function SearchSubItem(ByVal combo As ComboBox) As Integer
        'cmbSubItemSearch_OWN.Items.Add("ALL")
        cmbSubItemSearch_OWN.Items.Clear()
        strSql = vbCrLf + " SELECT 'ALL' SUBITEMNAME,'ALL'SUBITEMID,1 RESULT,0 DISPLAYORDER"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),2 RESULT,DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST"
        strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemSearch_OWN.Text & "') order by SubItemName"

        'strSql = vbCrLf + " SELECT 'ALL' SUBITEMNAME"
        'strSql += vbCrLf + " UNION ALL"
        'strSql += " Select SubItemName from " & cnAdminDb & "..SubItemMast where ItemId= (Select ItemId from " & cnAdminDb & "..ItemMast where ItemName='" & cmbItemSearch_OWN.Text & "') order by SubItemName"
        objGPack.FillCombo(strSql, combo, , False)
        'cmbSubItemSearch_OWN.Items.Add("ALL")
    End Function

    Private Sub cmbItemSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItemSearch_OWN.SelectedIndexChanged
        SearchSubItem(cmbSubItemSearch_OWN)
        ' cmbSubItemSearch_OWN.Items.Add("ALL")
    End Sub

    Private Sub frmStockReorder_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = (Keys.F9) Then
            cmbCostSearch_OWN.Focus()
            'cmbSubItemSearch_OWN.Items.Add("ALL")
        End If
    End Sub

    Private Sub txtWtFrom_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWtFrom_Own.Leave

    End Sub

    Private Sub txtWtTo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWtTo.TextChanged
        If flagSave = True Then
            Exit Sub
        End If

        '598
        'strSql = vbCrLf + " SELECT ISNULL(MAX(SR.TOWEIGHT),0) + .01 FROM " & cnAdminDb & "..STKREORDER AS SR"
        'strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_Man.Text & "')"
        'strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_Man.Text & "') AND SUBITEMNAME = '" & cmbSubItem_Man.Text & "'),0)"
        'strSql += vbCrLf + " AND SIZEID = ISNULL((SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & cmbSize.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_Man.Text & "')),0)"
        'strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_own.Text & "'),'')"
        'If designReorder = True Then
        '    strSql += vbCrLf + " AND DESIGNID = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesignerName.Text & "'),'')"
        'End If
        'txtWtFrom_Own.Text = objGPack.GetSqlValue(strSql)

    End Sub

    Private Sub txtWeight_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWeight.Leave
        'If designReorder Then
        '    cmbDesignerName.Focus()
        'End If
    End Sub

    Private Sub txtReOrder_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtReOrder.Leave
        'If designReorder Then
        '    btnSave.Focus()
        'End If
        'If designReorderFix Then
        '    btnSave.Focus()
        'End If
    End Sub

    Private Sub cmbRangeMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbRangeMode.SelectedIndexChanged
        If Mid(cmbRangeMode.Text, 1, 1) = "W" Then

            lblWtTo.Text = "To"
            lblWt.Text = "AVGWT"
            Dim wtmode As String = ""
            If cmbMetal_Man.Text = "DIAMOND" Then wtmode = "CENT" Else wtmode = "WEIGHT"
            lblWtFrom.Text = "From " & wtmode
            If gridView.Rows.Count > 0 Then
                gridView.Columns(5).HeaderText = "FROM" & wtmode
                gridView.Columns(6).HeaderText = "TO" & wtmode
                gridView.Columns(7).HeaderText = "AVG " & wtmode
            End If
        End If
        If Mid(cmbRangeMode.Text, 1, 1) = "R" Then
            lblWtFrom.Text = "From Rate"
            lblWtTo.Text = "To"
            lblWt.Text = "AvgRt"
            If gridView.Rows.Count > 0 Then
                gridView.Columns(5).HeaderText = "FROMRATE"
                gridView.Columns(6).HeaderText = "TORATE"
                gridView.Columns(7).HeaderText = "AVG RATE"
            End If
        End If
    End Sub

    Private Sub cmbRangeMode_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbRangeMode.Leave
        strSql = " Select RangeMode from " & cnAdminDb & "..STKREORDER Where ItemID=(Select ItemID from " & cnAdminDb & "..ItemMast where itemName='" & cmbItem_Man.Text & "')"
        Dim dts As New DataTable
        dts.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dts)
        If dts.Rows.Count > 0 Then
            If Mid(cmbRangeMode.Text, 1, 1) <> dts.Rows(0).Item("RangeMode").ToString Then
                If dts.Rows(0).Item("RangeMode").ToString = "R" Then
                    MsgBox("Already in Rate Range")
                    cmbRangeMode.Focus()
                ElseIf dts.Rows(0).Item("RangeMode").ToString = "W" Then
                    MsgBox("Already in Weight Range")
                    cmbRangeMode.Focus()
                End If
                Exit Sub
            End If
        End If
    End Sub

    Private Sub cmbItem_Man_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItem_Man.Leave
        strSql = " Select RangeMode from " & cnAdminDb & "..STKREORDER Where ItemID=(Select ItemID from " & cnAdminDb & "..ItemMast where itemName='" & cmbItem_Man.Text & "')"
        Dim dts As New DataTable
        dts.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dts)
        If dts.Rows.Count > 0 Then
            cmbRangeMode.Text = IIf(dts.Rows(0).Item("RangeMode").ToString = "R", "Rate Range", "Weight Range")
        End If
    End Sub

    Private Sub btnDelete_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnDelete.KeyDown

    End Sub
    Private Function calcWt()
        Dim wt As Double
        If txtWtFrom_Own.Text <> "" Then
            If txtWtTo.Text <> "" Then
                wt = (Convert.ToDouble(txtWtFrom_Own.Text) + Convert.ToDouble(txtWtTo.Text)) / 2
                txtWeight.Text = wt
            End If
        End If
    End Function
    Private Function calcPWt()
        Dim wt As Double
        If txtWtFrom_Own.Text <> "" Then
            If txtWtTo.Text <> "" Then
                wt = (Convert.ToDouble(txtWtFrom_Own.Text) + Convert.ToDouble(txtWtTo.Text)) / 2
                txtWeight.Text = wt
                If txtWeight.Text <> "" And Val(txtPiece.Text) <> 0 Then
                    wt = txtWeight.Text
                    txtWeight.Text = wt * Val(txtPiece.Text)
                End If
            End If
        End If
    End Function
    Private Sub txtPiece_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPiece.TextChanged
        calcPWt()
    End Sub
End Class