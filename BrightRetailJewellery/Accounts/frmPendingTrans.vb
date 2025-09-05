Imports System.Data.OleDb
Public Class frmPendingTrans
    Dim strSql As String
    Dim cmd As OleDbCommand

    Private Sub PartlySales()
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & SYSTEMID & "PENDINGTRANS')>0"
        strSql += " DROP TABLE TEMP" & SYSTEMID & "PENDINGTRANS"
        strSql += " SELECT *,1 RESULT INTO TEMP" & SYSTEMID & "PENDINGTRANS FROM"
        strSql += " ("
        strSql += " SELECT"
        strSql += " CASE WHEN ISNULL(SUBITEMID,0) = 0 THEN (SELECT ITEMNAME FROM " & cnadmindb & "..ITEMMAST WHERE ITEMID = I.ITEMID)"
        strSql += " ELSE (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)END AS [DESCRIPTION]"
        strSql += " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))aS METAL"
        strSql += " ,TRANDATE,TRANNO"
        strSql += " ,TAGNO,PCS,GRSWT,NETWT"
        strSql += " ,TAGPCS,TAGGRSWT,TAGNETWT"
        strSql += " ,TAGPCS - PCS AS BALPCS,TAGGRSWT - GRSWT BALGRSWT,TAGNETWT - NETWT AS BALNETWT"
        strSql += " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += " WHERE TAGNO <> '' AND COMPANYID = 'SSS' AND TRANTYPE = 'SA'"
        strSql += " GROUP BY TRANDATE,TRANNO,PCS,GRSWT,NETWT,TAGPCS"
        strSql += " ,TAGGRSWT,TAGNETWT,ITEMID,SUBITEMID,TAGNO"
        strSql += " )X"
        strSql += " WHERE PCS <> TAGPCS OR GRSWT <> TAGGRSWT OR NETWT <> TAGNETWT"
        strSql += " "
        strSql += " "
        strSql += " IF (SELECT DISTINCT 1 FROM TEMP" & SYSTEMID & "PENDINGTRANS) > 0"
        strSql += " BEGIN"
        strSql += " INSERT INTO TEMP" & SYSTEMID & "PENDINGTRANS(METAL,[DESCRIPTION],RESULT)"
        strSql += " SELECT DISTINCT METAL,METAL,0 FROM TEMP" & SYSTEMID & "PENDINGTRANS"
        strSql += " "
        strSql += " INSERT INTO TEMP" & SYSTEMID & "PENDINGTRANS("
        strSql += " METAL,DESCRIPTION"
        strSql += " ,PCS,GRSWT,NETWT"
        strSql += " ,TAGPCS,TAGGRSWT,TAGNETWT"
        strSql += " ,BALPCS,BALGRSWT,BALNETWT,RESULT)"
        strSql += " SELECT METAL,'SUB TOTAL' DESCRIPTION,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT"
        strSql += " ,SUM(TAGPCS)TAGPCS,SUM(TAGGRSWT)TAGGRSWT,SUM(TAGNETWT)TAGNETWT"
        strSql += " ,SUM(BALPCS)BALPCS,SUM(BALGRSWT)BALGRSWT,SUM(BALNETWT)BALNETWT,2 RESULT"
        strSql += " FROM TEMP" & systemId & "PENDINGTRANS"
        strSql += " GROUP BY METAL"
        strSql += " "
        strSql += " INSERT INTO TEMP" & systemId & "PENDINGTRANS("
        strSql += " METAL,DESCRIPTION"
        strSql += " ,PCS,GRSWT,NETWT"
        strSql += " ,TAGPCS,TAGGRSWT,TAGNETWT"
        strSql += " ,BALPCS,BALGRSWT,BALNETWT,RESULT)"
        strSql += " SELECT 'ZZZZZ'METAL,'GRAND TOTAL' DESCRIPTION,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT"
        strSql += " ,SUM(TAGPCS)TAGPCS,SUM(TAGGRSWT)TAGGRSWT,SUM(TAGNETWT)TAGNETWT"
        strSql += " ,SUM(BALPCS)BALPCS,SUM(BALGRSWT)BALGRSWT,SUM(BALNETWT)BALNETWT,3 RESULT"
        strSql += " FROM TEMP" & systemId & "PENDINGTRANS WHERE RESULT = 2"
        strSql += " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " SELECT *"
        'strSql += " DESCRIPTION,METAL,TRANDATE,TRANNO,TAGNO"
        'strSql += " ,CASE WHEN PCS <> 0 THEN PCS ELSE NULL END PCS "
        'strSql += " ,CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END GRSWT"
        'strSql += " ,CASE WHEN NETWT <> 0 THEN  NETWT ELSE NULL END NETWT"
        'strSql += " ,CASE WHEN TAGPCS <> 0 THEN  TAGPCS ELSE NULL END TAGPCS"
        'strSql += " ,CASE WHEN TAGGRSWT <> 0 THEN  TAGGRSWT ELSE NULL END TAGGRSWT"
        'strSql += " ,CASE WHEN TAGNETWT <> 0 THEN  TAGNETWT ELSE NULL END TAGNETWT"
        'strSql += " ,CASE WHEN BALPCS <> 0 THEN  BALPCS ELSE NULL END BALPCS"
        'strSql += " ,CASE WHEN BALWT <> 0 THEN  BALWT ELSE NULL END BALWT"
        'strSql += " ,RESULT"
        strSql += " FROM TEMP" & systemId & "PENDINGTRANS ORDER BY METAL,RESULT,DESCRIPTION"
        Dim dtGrid As New DataTable
        'Dim dtCol As New DataColumn("CHECK")
        'dtCol.DataType = GetType(Boolean)
        'dtCol.DefaultValue = True
        'dtGrid.Columns.Add(dtCol)
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then Exit Sub

        gridView.DataSource = dtGrid
        With gridView
            'set Visiblity
            .Columns("METAL").Visible = False
            .Columns("RESULT").Visible = False
            'set width
            '.Columns("CHECK").HeaderText = ""
            '.Columns("CHECK").Width = 25
            .Columns("DESCRIPTION").Width = 120
            .Columns("TRANDATE").Width = 80
            .Columns("TRANNO").Width = 60
            .Columns("TAGNO").Width = 70
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("TAGPCS").Width = 60
            .Columns("TAGGRSWT").Width = 80
            .Columns("TAGNETWT").Width = 80
            .Columns("BALPCS").Width = 60
            .Columns("BALGRSWT").Width = 80
            .Columns("BALNETWT").Width = 80
            ''set alignment
            '.Columns("CHECK").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAGPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAGGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAGNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("BALPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("BALGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("BALNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            ''set format
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            For cnt As Integer = 0 To .Columns.Count - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                .Columns(cnt).ReadOnly = True
            Next
            '.Columns("CHECK").ReadOnly = False
        End With
    End Sub

    Private Sub frmPendingTrans_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmPendingTrans_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmPendingTrans_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre.Enabled = True
            strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre, , False)
        Else
            cmbCostCentre.Text = ""
            cmbCostCentre.Enabled = False
        End If
        gridView.RowTemplate.Height = 21
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        tabMain.SelectedTab = tabGeneral
        rbtPartySales.Checked = True
        If cmbCostCentre.Enabled Then cmbCostCentre.Select() Else Me.SelectNextControl(cmbCostCentre, True, True, True, True)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        Me.Refresh()
        If rbtPartySales.Checked Then
            PartlySales()
        ElseIf rbtSalesReturn.Checked Then
            MsgBox("Sales Return")
            Exit Sub
        Else
            MsgBox("Purchase ")
            Exit Sub
        End If
        If gridView.RowCount > 0 Then gridView.Select() Else MsgBox("Record not found", MsgBoxStyle.Information)
        tabMain.SelectedTab = tabView
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        If cmbCostCentre.Enabled Then cmbCostCentre.Select() Else Me.SelectNextControl(cmbCostCentre, True, True, True, True)
    End Sub

    Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
        If gridView.Rows(e.RowIndex).Cells("RESULT").Value.ToString = "0" Then 'HEADING
            gridView.Rows(e.RowIndex).Cells("DESCRIPTION").Style.BackColor = Color.LightBlue
            gridView.Rows(e.RowIndex).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        ElseIf gridView.Rows(e.RowIndex).Cells("RESULT").Value.ToString = "2" Then 'SUBTOTAL
            gridView.Rows(e.RowIndex).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        ElseIf gridView.Rows(e.RowIndex).Cells("RESULT").Value.ToString = "3" Then 'SUBTOTAL
            gridView.Rows(e.RowIndex).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridView.Rows(e.RowIndex).Cells("DESCRIPTION").Style.BackColor = Color.LightGoldenrodYellow
        End If
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridView.RowCount > 0 Then gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(0)
        End If
    End Sub

    Private Sub btnTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click

    End Sub
End Class