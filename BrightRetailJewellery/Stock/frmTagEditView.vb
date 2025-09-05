Imports System.Data.OleDb
Public Class frmTagEditView
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim dtGridView As New DataTable

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
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        btnSearch.Enabled = False
        Try
            dtGridView.Rows.Clear()
            Dim condition As String = Nothing
            strSql = " SELECT SNO,RECDATE,TAGNO"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)AS SUBITEM"
            strSql += vbCrLf + " ,CASE WHEN PCS <> 0 THEN PCS ELSE NULL END PCS"
            strSql += vbCrLf + " ,CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END GRSWT"
            strSql += vbCrLf + " ,CASE WHEN LESSWT <> 0 THEN LESSWT ELSE NULL END LESSWT"
            strSql += vbCrLf + " ,CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END NETWT"
            strSql += vbCrLf + " ,CASE WHEN SALVALUE <> 0 THEN SALVALUE ELSE NULL END SALVALUE"
            strSql += vbCrLf + " ,(SELECT PURVALUE FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGSNO = I.SNO)AS  PURVALUE,COSTID"
            strSql += vbCrLf + " ,I.TAGVAL"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS I WHERE ISSDATE IS NULL "
            If COSTCENTRE_SINGLE = False Then
                strSql += " AND ISNULL(COSTID,'') = '" & cnCostId & "'"
            End If
            If cmbCostCentre_MAN.Text <> "" Then
                strSql += " AND ISNULL(COSTID,'') in ( select costid from " & cnAdminDb & "..costcentre where costname = '" & cmbCostCentre_MAN.Text & "')"
            End If
            If chkDate.Checked Then
                'condition += " WHERE ISSDATE IS NULL"
                condition += " AND RECDATE = '" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & "'"
            End If
            If txtLotNo_NUM.Text <> "" Then
                'If condition = Nothing Then condition += " WHERE " Else condition += " AND "
                condition += " AND LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE ISNULL(LOTNO,'') = '" & txtLotNo_NUM.Text & "')"
            End If
            If Val(txtItemId_NUM.Text) > 0 Then
                'If condition = Nothing Then condition += " WHERE " Else condition += " AND "
                condition += " AND ITEMID = " & Val(txtItemId_NUM.Text) & ""
            End If
            If txtTagNo.Text <> "" Then
                'If condition = Nothing Then condition += " WHERE " Else condition += " AND "
                condition += " AND TAGNO = '" & txtTagNo.Text & "'"
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
            gridView.Select()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            btnSearch.Enabled = True
        End Try

    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        dtpDate.Value = GetEntryDate(GetServerDate(tran), tran)
        dtGridView.Rows.Clear()
        chkDate.Checked = True
        chkDate.Focus()
    End Sub

    Private Sub chkDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        dtpDate.Enabled = chkDate.Checked
    End Sub

    Private Sub frmTagEditView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagEditView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '        lblDelete.Visible = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete, False)
        GridStyle()
        dtpDate.MinimumDate = (New DateTimePicker).MinDate
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

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("ITEM")
            End If
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGEDITB4DATE'", , "N")) = "N" Then
                If Format(gridView.Rows(gridView.CurrentRow.Index).Cells("RECDATE").Value, "yyyy-MM-dd") <> GetServerDate(tran) Then
                    MsgBox("Cannot Edit this Tag", MsgBoxStyle.Information)
                    gridView.Select()
                    Exit Sub
                End If
            End If
            Dim IS_USERLEVELPWD As Boolean = IIf(GetAdmindbSoftValue("USERLEVELPWD", "N") = "Y", True, False)
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
            If Not usrpwdok("TAGEDIT", IS_USERLEVELPWD) Then
                MsgBox("OTP Required...", MsgBoxStyle.Information)
                Exit Sub
            End If
            If UCase(objGPack.GetSqlValue("SELECT ISNULL(APPROVAL,'') FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString & "'", , "")) = "A" Then
                MsgBox("Cannot Edit Approval issue Tag", MsgBoxStyle.Information)
                gridView.Select()
                Exit Sub
            End If

            'Dim obj As New frmItemTagEdit(Val(gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString))
            Try
                Dim obj As New frmItemTag(gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString)
                'obj.BackColor = frmBackColor
                'obj.BackgroundImage = bakImage
                'obj.BackgroundImageLayout = ImageLayout.Stretch
                'obj.StartPosition = FormStartPosition.CenterScreen
                'BrighttechPack.LanguageChange.Set_Language_Form(obj, LangId)
                'objGPack.Validator_Object(obj)
                'obj.MdiParent = Main
                obj.Show()
                'ElseIf e.KeyChar = "M" Or e.KeyChar = "m" Then
                '    If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
                '    Dim obj As New frmItemTagMiscCharge(gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString)
                '    obj.ShowDialog()
            Catch ex As Exception

            End Try

        End If
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

    
    Private Sub txtItemId_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemId_NUM.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT ITEMID,ITEMNAME,STOCKTYPE,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE 1=1"
            strSql += GetItemQryFilteration("S")
            Dim ro As DataRow = Nothing
            ro = BrighttechPack.SearchDialog.Show_R("Search ItemId", strSql, cn)
            If Not ro Is Nothing Then
                txtItemId_NUM.Text = Val(ro!ITEMID)
                txtItemName.Text = ro!ITEMNAME.ToString
            End If
            txtItemId_NUM.SelectAll()
        End If
    End Sub

    Private Sub txtItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemName.GotFocus
        SendKeys.Send("{TAB}")
    End Sub
End Class