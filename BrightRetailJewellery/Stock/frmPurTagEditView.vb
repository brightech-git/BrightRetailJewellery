Imports System.Data.OleDb
Public Class frmPurTagEditView
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim dtGridView As New DataTable
    Dim cmd As New OleDbCommand
    Dim tran As OleDbTransaction

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Dim dt As New DataTable
        strSql = " SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' AND CTLTEXT = 'Y'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            cmbCostCentre_MAN.Text = ""
            cmbCostCentre_MAN.Items.Clear()
            cmbCostCentre_MAN.Enabled = False
        End If
        If cmbCostCentre_MAN.Enabled Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre_MAN)
        End If
        ' Add any initialization after the InitializeComponent() call.
        With dtGridView
            .Columns.Add("SNO", GetType(String))
            .Columns.Add("RECDATE", GetType(Date))
            .Columns.Add("TAGNO", GetType(String))
            .Columns.Add("ITEM", GetType(String))
            .Columns.Add("SUBITEM", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("LESSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("SALVALUE", GetType(Double))
            .Columns.Add("PURVALUE", GetType(Double))
            .Columns.Add("TAGVAL", GetType(String))
            '.Columns.Add("COUNTER", GetType(String))
            '.Columns.Add("DESIGNER", GetType(String))
            '.Columns.Add("SIZE", GetType(String))
        End With

        gridView.DataSource = dtGridView
        FormatGridColumns(gridView)
        gridView.ColumnHeadersVisible = True
    End Sub

    Private Sub GridStyle()
        With gridView
            .Columns("SNO").Visible = False
            .Columns("RECDATE").Width = 80
            .Columns("TAGNO").Width = 100
            .Columns("ITEM").Width = 200
            .Columns("SUBITEM").Width = 150
            .Columns("PCS").Width = 50
            .Columns("GRSWT").Width = 80
            .Columns("LESSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("SALVALUE").Width = 100
            .Columns("PURVALUE").Width = 100
            .Columns("TAGVAL").Visible = False
            '.Columns("COUNTER").Width = 100
            '.Columns("DESIGNER").Width = 100
            '.Columns("SIZE").Width = 100
        End With
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        btnSearch.Enabled = False
        Try
            dtGridView.Rows.Clear()
            Dim _ItemId As Double = 0
            Dim _SubItemId As Double = 0
            Dim _DesignerId As Double = 0
            'cmbDesigner
            If cmbItemName.Text <> "" Or cmbItemName.Text <> "ALL" Then
                strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName.Text.ToString & "'"
                _ItemId = Val(objGPack.GetSqlValue(strSql, , ""))
            End If
            If _ItemId <> 0 And cmbSubItemName.Text <> "" Or cmbSubItemName.Text <> "ALL" Then
                strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID ='" & _ItemId.ToString & "' AND SUBITEMNAME='" & cmbSubItemName.Text.ToString & "'"
                _SubItemId = Val(objGPack.GetSqlValue(strSql, , ""))
            End If
            If cmbDesigner.Text <> "" Or cmbDesigner.Text <> "ALL" Then
                strSql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & cmbDesigner.Text.ToString & "'"
                _DesignerId = Val(objGPack.GetSqlValue(strSql, , ""))
            End If
            Dim condition As String = Nothing
            strSql = " SELECT SNO,I.RECDATE,I.TAGNO"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)AS SUBITEM"
            strSql += vbCrLf + " ,CASE WHEN PCS <> 0 THEN PCS ELSE NULL END PCS"
            strSql += vbCrLf + " ,CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END GRSWT"
            strSql += vbCrLf + " ,CASE WHEN LESSWT <> 0 THEN LESSWT ELSE NULL END LESSWT"
            strSql += vbCrLf + " ,CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END NETWT"
            strSql += vbCrLf + " ,CASE WHEN SALVALUE <> 0 THEN SALVALUE ELSE NULL END SALVALUE"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),PURWASTAGE) PURWASTAGE,CONVERT(NUMERIC(15,2),PURMC) PURMC,CONVERT(NUMERIC(15,2),PURTOUCH) PURTOUCH"
            'If txtPurWast_NUM.Text <> "" Then
            '    strSql += vbCrLf + " ,'" & txtPurWast_NUM.Text.ToString & "' PURWASTAGE"
            'Else
            '    strSql += vbCrLf + " ,PURWASTAGE"
            'End If
            'If txtPurMc_NUM.Text <> "" Then
            '    strSql += vbCrLf + " ,'" & txtPurMc_NUM.Text.ToString & "' PURMC"
            'Else
            '    strSql += vbCrLf + " ,PURMC"
            'End If
            'If txtPurTouch.Text <> "" Then
            '    strSql += vbCrLf + " ,'" & txtPurTouch.Text.ToString & "' PURTOUCH"
            'Else
            '    strSql += vbCrLf + " ,PURTOUCH"
            'End If
            strSql += vbCrLf + " ,PURVALUE,I.COSTID"
            strSql += vbCrLf + " ,I.TAGVAL"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS I "
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON I.SNO=P.TAGSNO AND I.TAGNO=P.TAGNO "
            strSql += vbCrLf + " WHERE ISSDATE IS NULL "
            If COSTCENTRE_SINGLE = False Then
                strSql += " AND ISNULL(I.COSTID,'') = '" & cnCostId & "'"
            End If
            If cmbCostCentre_MAN.Text <> "" Then
                strSql += " AND ISNULL(I.COSTID,'') IN ( SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
            End If
            If chkDate.Checked Then
                condition += " AND I.RECDATE BETWEEN '" & dtpFromDate.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpToDate.Value.Date.ToString("yyyy-MM-dd") & "'"
            End If
            If txtLotNo_NUM.Text <> "" Then
                condition += " AND LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTSNO = '" & txtLotNo_NUM.Text & "')"
            End If
            If _ItemId > 0 Then
                condition += " AND I.ITEMID = " & _ItemId & ""
            End If
            If _SubItemId > 0 Then
                condition += " AND SUBITEMID = " & _SubItemId & ""
            End If
            If _DesignerId > 0 Then
                condition += " AND DESIGNERID = " & _DesignerId & ""
            End If
            If txtTagNo.Text <> "" Then
                condition += " AND I.TAGNO = '" & txtTagNo.Text & "'"
            End If
            'If condition = Nothing Then condition += " WHERE COMPANYID = '" & GetStockCompId() & "'" Else 
            If Not cnCentStock Then condition += " AND COMPANYID = '" & GetStockCompId() & "'"
            strSql += condition
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridView)
            dtGridView.AcceptChanges()
            If Not dtGridView.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            If gridView.Rows.Count > 0 Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgv As DataGridViewColumn In gridView.Columns
                    dgv.Width = dgv.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
                gridView.Columns("PURWASTAGE").HeaderText = "WASTAGE"
                gridView.Columns("PURMC").HeaderText = "MC"
                gridView.Columns("PURTOUCH").HeaderText = "TOUCH"
                gridView.Columns("PURWASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns("PURMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns("PURTOUCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If

            gridView.Select()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            btnSearch.Enabled = True
        End Try

    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ''Load ItemName
        cmbItemName.Items.Clear()
        cmbItemName.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, cmbItemName, False)
        cmbItemName.Text = "ALL"
        ''Load Designer
        cmbDesigner.Items.Clear()
        cmbDesigner.Items.Add("ALL")
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner, False)
        cmbDesigner.Text = "ALL"

        objGPack.TextClear(Me)
        dtpFromDate.Value = GetEntryDate(GetServerDate(tran), tran)
        dtGridView.Rows.Clear()
        chkDate.Checked = True
        chkDate.Focus()
    End Sub

    Private Sub chkDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        dtpFromDate.Enabled = chkDate.Checked
        dtpToDate.Enabled = chkDate.Checked
    End Sub

    Private Sub frmTagEditView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagEditView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GridStyle()
        dtpFromDate.MinimumDate = (New DateTimePicker).MinDate
        dtpToDate.MinimumDate = (New DateTimePicker).MinDate
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub txtLotNo_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLotNo_NUM.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT "
            strSql += " LOTNO"
            strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID = LOT.ITEMID)AS ITEM"
            strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID = LOT.SUBITEMID),'')AS SUBITEM"
            strSql += " ,PCS,GRSWT,CPCS AS CPCS,CGRSWT AS CGRSWT,(PCS-CPCS)AS BALPCS,(GRSWT-CGRSWT)AS BALGRSWT"
            strSql += " ,(SELECT (CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END) FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS STOCKTYPE"
            strSql += " FROM " & cnAdminDb & "..ITEMLOT AS LOT"
            strSql += " WHERE PCS > CPCS AND ISNULL(COMPLETED,'') <> 'Y'"
            strSql += " AND (SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID) = 'T'"
            strSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
            If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            strSql += " ORDER BY LOTNO "
            txtLotNo_NUM.Text = BrighttechPack.SearchDialog.Show("Search LotNo", strSql, cn)
            txtLotNo_NUM.SelectAll()
        End If
    End Sub

    Private Sub cmbItemName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItemName.Leave
        If cmbItemName.Text <> "" And cmbItemName.Text <> "ALL" Then
            cmbSubItemName.Items.Clear()
            cmbSubItemName.Items.Add("ALL")
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST"
            strSql += vbCrLf + " WHERE ITEMID IN (SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItemName.Text.ToString & "') "
            strSql += vbCrLf + " ORDER BY SUBITEMNAME"
            objGPack.FillCombo(strSql, cmbSubItemName, False)
            cmbSubItemName.Text = "ALL"
        Else
            cmbSubItemName.Items.Clear()
            cmbSubItemName.Items.Add("ALL")
        End If
        
    End Sub

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        If Not MsgBox("Do you want to update?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then Exit Sub
        Try
            Dim CostId As String = ""
            tran = Nothing
            tran = cn.BeginTransaction
            For Each ro As DataGridViewRow In gridView.Rows
                With ro
                    CostId = .Cells("COSTID").Value.ToString
                    strSql = " SELECT 1 FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGSNO='" & .Cells("SNO").Value.ToString & "'"
                    If Val(objGPack.GetSqlValue(strSql, , "", tran)) > 0 Then
                        strSql = "UPDATE " & cnAdminDb & "..PURITEMTAG SET TAGNO='" & .Cells("TAGNO").Value.ToString & "' "
                        If txtPurWast_NUM.Text <> "" Then strSql += vbCrLf + " ,PURWASTAGE='" & Val(txtPurWast_NUM.Text.ToString) & "'"
                        If txtPurTouch.Text <> "" Then strSql += vbCrLf + " ,PURTOUCH='" & Val(txtPurTouch.Text.ToString) & "'"
                        If txtPurMc_NUM.Text <> "" Then strSql += vbCrLf + " ,PURMC='" & Val(txtPurMc_NUM.Text.ToString) & "' "
                        strSql += vbCrLf + " WHERE TAGSNO='" & .Cells("SNO").Value.ToString & "'"
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, CostId)
                    Else
                        ''ITEM PUR DETAIL
                        strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAG"
                        strSql += vbCrLf + " ("
                        strSql += vbCrLf + " TAGSNO"
                        strSql += vbCrLf + " ,TAGNO"
                        strSql += vbCrLf + " ,PURLESSWT"
                        strSql += vbCrLf + " ,PURNETWT"
                        If txtPurWast_NUM.Text <> "" Then strSql += vbCrLf + " ,PURWASTAGE"
                        If txtPurTouch.Text <> "" Then strSql += vbCrLf + " ,PURTOUCH"
                        If txtPurMc_NUM.Text <> "" Then strSql += vbCrLf + " ,PURMC"
                        strSql += vbCrLf + " ,RECDATE"
                        strSql += vbCrLf + " ,COMPANYID,COSTID"
                        strSql += vbCrLf + " )"
                        strSql += vbCrLf + " VALUES"
                        strSql += vbCrLf + " ("
                        strSql += vbCrLf + " '" & .Cells("SNO").Value.ToString & "'" 'TAGSNO
                        strSql += vbCrLf + " ,'" & .Cells("TAGNO").Value.ToString & "'" 'TAGNO
                        strSql += vbCrLf + " ," & Val(.Cells("LESSWT").Value.ToString) & "" ' PURLESSWT
                        strSql += vbCrLf + " ," & Val(.Cells("NETWT").Value.ToString) & "" ' PURNETWT"                        
                        If txtPurWast_NUM.Text <> "" Then strSql += vbCrLf + " ," & Val(txtPurWast_NUM.Text.ToString) & "" ' PURWASTAGE"
                        If txtPurTouch.Text <> "" Then strSql += vbCrLf + " ," & Val(txtPurTouch.Text.ToString) & "" ' PURTOUCH"
                        If txtPurMc_NUM.Text <> "" Then strSql += vbCrLf + " ," & Val(txtPurMc_NUM.Text.ToString) & "" ' PURMC"
                        strSql += vbCrLf + " ,'" & Convert.ToDateTime(.Cells("RECDATE").Value).ToString("yyyy-MM-dd") & "'"
                        strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                        strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, cnCostId) & "'"
                        strSql += vbCrLf + " )"
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, CostId)
                    End If
                End With
            Next
            tran.Commit()
            tran = Nothing
            btnSearch_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
End Class