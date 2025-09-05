Imports System.Data.OleDb
Public Class OrderEditView
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim StrSql As String
    Dim CostId As String
    Dim ORADDITIONALDETAIL As Boolean = IIf(GetAdmindbSoftValue("ORADDITIONALDETAIL", "N") = "Y", True, False)

    Private Sub OrderEditView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub OrderEditView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ''LOAD COSTCENTRE
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            cmbCostCentre.Enabled = True
            StrSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(StrSql, cmbCostCentre)
            cmbCostCentre.Text = cnCostName
        Else
            cmbCostCentre.Enabled = False
        End If
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        gridView.DataSource = Nothing
        txtOrderNo.Clear()
        txtOrderNo.Select()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        CostId = cnCostId
        If cmbCostCentre.Text <> "" And cmbCostCentre.Text <> "ALL" Then
            CostId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'")
        End If
        StrSql = vbCrLf + " SELECT O.STYLENO,SUBSTRING(O.ORNO,6,20)ORNO,O.ORDATE,IM.ITEMNAME,O.DESCRIPT,IT.TAGNO,O.PCS,O.GRSWT,O.NETWT"
        StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnadmindb & "..ORSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
        StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnadmindb & "..ORSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
        StrSql += vbCrLf + " ,O.WAST,O.WASTPER,O.MC,O.MCGRM,O.ORVALUE,CO.COSTNAME,O.SNO,O.COSTID,O.BATCHNO,O.ORDCANCEL,O.ODBATCHNO"
        StrSql += vbCrLf + " ,CASE WHEN ISNULL(O.ODBATCHNO,'') <> '' THEN 'DELIVERED'"
        StrSql += vbCrLf + "   WHEN ISNULL(O.ORDCANCEL,'') <> '' THEN 'CANCELLED'"
        StrSql += vbCrLf + "   ELSE ISNULL((SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = (SELECT TOP 1 ORDSTATE_ID FROM " & cnadmindb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ISNULL(CANCEL,'') = '' ORDER BY ENTRYORDER DESC)),'PENDING WITH US') "
        StrSql += vbCrLf + "   END AS STATUS,O.ORNO AS AORNO,O.ORTYPE,O.REASON,O.MC "
        StrSql += vbCrLf + " FROM " & cnadmindb & "..ORMAST AS O"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = O.ITEMID"
        StrSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..COSTCENTRE AS CO ON CO.COSTID = O.COSTID"
        StrSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..ITEMTAG AS IT ON IT.ORSNO = O.SNO"
        StrSql += vbCrLf + " WHERE ISNULL(ODBATCHNO,'') = '' AND ISNULL(ODSNO,'') = ''"
        StrSql += vbCrLf + " AND ORNO = '" & GetCostId(CostId) & GetCompanyId(strCompanyId) & txtOrderNo.Text & "'"
        StrSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
        StrSql += vbCrLf + " ORDER BY O.ORDATE"
        Dim dtGrid As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        gridView.DataSource = dtGrid
        For Each DgvRow As DataGridViewRow In gridView.Rows
            If DgvRow.Cells("ORDCANCEL").Value.ToString = "Y" Then
                DgvRow.DefaultCellStyle.BackColor = Color.Red
            End If
        Next
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView)
        gridView.Columns("ORDCANCEL").HeaderText = "CANCEL"
        gridView.Columns("SNO").Visible = False
        gridView.Columns("ORDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        gridView.Columns("COSTID").Visible = False
        gridView.Columns("COSTNAME").Visible = IIf(GetAdmindbSoftValue("COSTID", "") = "", False, True)
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        gridView.Columns("DESCRIPT").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        gridView.Columns("DESCRIPT").Width = 200
        gridView.Columns("BATCHNO").Visible = False
        gridView.Columns("ITEMNAME").Visible = False
        gridView.Columns("ORDCANCEL").Visible = False
        gridView.Columns("ODBATCHNO").Visible = False
        gridView.Columns("AORNO").Visible = False
        gridView.Select()
    End Sub

    Private Sub EstimateView(ByVal OrNo As String, ByVal CostId As String)
        StrSql = vbCrLf + " IF OBJECT_ID('TEMP" & SYSTEMID & "ORSTATUS','U') IS NOT NULL DROP TABLE TEMP" & SYSTEMID & "ORSTATUS"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " DECLARE @ORNO VARCHAR(15)"
        StrSql += vbCrLf + " DECLARE @COSTID VARCHAR(2)"
        StrSql += vbCrLf + " SET @ORNO = '" & OrNo & "'"
        StrSql += vbCrLf + " SET @COSTID = '" & CostId & "'"
        StrSql += vbCrLf + " SELECT DISTINCT IM.ITEMNAME AS ITEM,TG.TAGNO,"
        StrSql += vbCrLf + " CASE WHEN ISNULL(TG.PCS,0) <> 0 THEN TG.PCS ELSE OM.PCS END AS PCS,"
        StrSql += vbCrLf + " CASE WHEN ISNULL(TG.GRSWT,0) <> 0 THEN TG.GRSWT ELSE OM.GRSWT END AS GRSWT,"
        StrSql += vbCrLf + " CASE WHEN ISNULL(TG.NETWT,0) <> 0 THEN TG.NETWT ELSE OM.NETWT END AS NETWT,"
        StrSql += vbCrLf + " CASE WHEN ISNULL(ISS.NETWT,0) <> 0 THEN ISS.NETWT WHEN ISNULL(TG.NETWT,0) <> 0 THEN TG.NETWT ELSE OM.NETWT END AS WEIGHT,"
        StrSql += vbCrLf + " ISNULL((SELECT SALESTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS TAX,"
        StrSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL)ORVALUE,CONVERT(NUMERIC(15,2),NULL)TAGVALUE,CONVERT(NUMERIC(15,2),NULL)SALVALUE"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)VALUE,EMP.EMPNAME,"
        StrSql += vbCrLf + " OM.SNO"
        StrSql += vbCrLf + " INTO TEMP" & systemId & "ORSTATUS"
        StrSql += vbCrLf + " FROM "
        StrSql += vbCrLf + " " & cnadmindb & "..ORMAST AS OM"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = OM.ITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.ORSNO = OM.SNO"
        StrSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSUE AS ISS ON ISS.SNO = OM.ODSNO AND ISS.BATCHNO = OM.ODBATCHNO"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER AS EMP ON EMP.EMPID = OM.EMPID"
        StrSql += vbCrLf + " WHERE OM.ORNO = @ORNO AND OM.COSTID = @COSTID"
        StrSql += vbCrLf + " AND ISNULL(OM.ORDCANCEL,'') = '' AND ISNULL(OM.CANCEL,'') = '' AND ISNULL(ODBATCHNO,'') = ''"
        StrSql += vbCrLf + " "
        StrSql += vbCrLf + " UPDATE TEMP" & systemId & "ORSTATUS SET TAGVALUE = TDET.TAGVALUE"
        StrSql += vbCrLf + "   FROM TEMP" & systemId & "ORSTATUS AS T,"
        StrSql += vbCrLf + "   ("
        StrSql += vbCrLf + "   	SELECT"
        StrSql += vbCrLf + "   	   CASE WHEN CALTYPE = 'R' THEN PCS * RATE"
        StrSql += vbCrLf + "   	   ELSE ((WEIGHT + MAXWAST) * METALRATE)+STNAMT+MISCAMT+MAXMC END TAGVALUE"
        StrSql += vbCrLf + "   	   ,ORSNO	"
        StrSql += vbCrLf + "   	FROM"
        StrSql += vbCrLf + "   	("
        StrSql += vbCrLf + "   		SELECT *,"
        StrSql += vbCrLf + "   		(SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
        StrSql += vbCrLf + "   		    WHERE RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST)"
        StrSql += vbCrLf + "   		    AND METALID = Y.METALID"
        StrSql += vbCrLf + "   		    AND PURITY = Y.PURITY"
        StrSql += vbCrLf + "   		    ORDER BY SNO DESC"
        StrSql += vbCrLf + "   		)AS METALRATE"
        StrSql += vbCrLf + "   		FROM"
        StrSql += vbCrLf + "   		("
        StrSql += vbCrLf + "   			SELECT ITEMID,SUBITEMID,ITEMTYPEID,PCS,WEIGHT,MAXWAST,MAXMC,STNAMT,MISCAMT,CASE WHEN TPURITY > 0 THEN TPURITY ELSE PURITY END AS PURITY,METALID,CALTYPE,RATE,ORSNO"
        StrSql += vbCrLf + "   			FROM"
        StrSql += vbCrLf + "   			("
        StrSql += vbCrLf + "   			SELECT ITEMID,SUBITEMID,ITEMTYPEID,PCS,CASE WHEN GRSNET = 'G' THEN GRSWT ELSE NETWT END AS WEIGHT,MAXWAST,MAXMC"
        StrSql += vbCrLf + "   			,ISNULL((SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO),0)AS STNAMT"
        StrSql += vbCrLf + "   			,ISNULL((SELECT SUM(ISNULL(AMOUNT,0)) FROM " & cnAdminDb & "..ITEMTAGMISCCHAR WHERE TAGSNO = T.SNO),0)AS MISCAMT"
        StrSql += vbCrLf + "   			,ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID AND RATEGET = 'Y' AND SOFTMODULE = 'S')),0)AS TPURITY"
        StrSql += vbCrLf + "   			,(SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)))AS PURITY"
        StrSql += vbCrLf + "   			,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID))AS METALID"
        StrSql += vbCrLf + "   			,CASE WHEN SUBITEMID = 0 THEN (SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) ELSE"
        StrSql += vbCrLf + "   			(SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID) END AS CALTYPE"
        StrSql += vbCrLf + "   		        ,RATE,ORSNO"
        StrSql += vbCrLf + "   			FROM " & cnAdminDb & "..ITEMTAG AS T"
        StrSql += vbCrLf + "   	 		WHERE ORSNO IN (SELECT SNO FROM TEMP" & systemId & "ORSTATUS)"
        StrSql += vbCrLf + "   			)X"
        StrSql += vbCrLf + "   		)Y"
        StrSql += vbCrLf + "   	)Z"
        StrSql += vbCrLf + "   )AS TDET"
        StrSql += vbCrLf + "   WHERE T.SNO = TDET.ORSNO"
        StrSql += vbCrLf + " "
        StrSql += vbCrLf + " UPDATE TEMP" & systemId & "ORSTATUS SET SALVALUE = (SELECT AMOUNT FROM " & cnStockDb & "..ISSUE WHERE SNO = (SELECT ODSNO FROM " & cnadmindb & "..ORMAST WHERE SNO = T.SNO) AND ISNULL(SNO,'') <> '')"
        StrSql += vbCrLf + "  ,ORVALUE = (SELECT ORVALUE FROM " & cnadmindb & "..ORMAST WHERE SNO = T.SNO)"
        StrSql += vbCrLf + "  FROM TEMP" & systemId & "ORSTATUS AS T"
        StrSql += vbCrLf + " "
        StrSql += vbCrLf + " UPDATE TEMP" & systemId & "ORSTATUS SET VALUE = CASE WHEN ISNULL(SALVALUE,0) <> 0 THEN SALVALUE WHEN ISNULL(TAGVALUE,0) <> 0 THEN TAGVALUE ELSE ORVALUE END"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " SELECT ITEM,TAGNO,PCS,GRSWT,NETWT,WEIGHT,CONVERT(NUMERIC(15,2),VALUE)VALUE,EMPNAME,1 RESULT FROM TEMP" & SYSTEMID & "ORSTATUS"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT 'TAX'ITEM,NULL TAGNO,NULL PCS,NULL GRSWT,NULL NETWT,NULL WEIGHT,CONVERT(NUMERIC(15,2),SUM(VALUE * (TAX/100))) AS VALUE,NULL EMPNAME,2 RESULT FROM TEMP" & systemId & "ORSTATUS"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT 'TOTAL'ITEM,NULL TAGNO,NULL PCS,NULL GRSWT,NULL NETWT,NULL WEIGHT,CONVERT(NUMERIC(15,2),SUM(VALUE + VALUE * (TAX/100))) AS VALUE,NULL EMPNAME,3 RESULT FROM TEMP" & systemId & "ORSTATUS"
        StrSql += vbCrLf + " ORDER BY RESULT,ITEM"

        Dim objGridShower As New frmGridDispDia
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(objGridShower.dsGrid, "SUMMARY")
        If Not objGridShower.dsGrid.Tables("SUMMARY").Rows.Count > 0 Then
            MsgBox("There is no record", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim NetTotal As Decimal = Nothing
        Dim AdvAmtTotal As Decimal = Nothing

        Dim balAmt As Double = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = '" & OrNo & "' AND COSTID = '" & CostId & "'"))
        'Val(.Cells("RECEIPTAMT").Value.ToString) - Val(.Cells("PAYMENTAMT").Value.ToString)
        Dim balWt As Double = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = '" & OrNo & "' AND COSTID = '" & CostId & "'"))
        'Val(.Cells("RECEIPTWT").Value.ToString) - Val(.Cells("PAYMENTWT").Value.ToString)
        Dim discAmt As Decimal = Val(objGPack.GetSqlValue("SELECT TOP 1 DISCOUNT FROM " & cnadmindb & "..ORMAST WHERE ORNO = '" & OrNo & "' AND COSTID = '" & CostId & "'"))
        'Val(lblDiscount.Text)
        Dim balance As Double = Val(objGridShower.dsGrid.Tables("SUMMARY").Rows(objGridShower.dsGrid.Tables("SUMMARY").Rows.Count - 1).Item("VALUE").ToString)
        NetTotal = balance
        Dim ro As DataRow = Nothing
        ro = objGridShower.dsGrid.Tables("SUMMARY").NewRow
        ro("ITEM") = "ADV-AMOUNT"
        ro("VALUE") = Format(balAmt, "0.00")
        ro("RESULT") = "3"
        objGridShower.dsGrid.Tables("SUMMARY").Rows.Add(ro)
        AdvAmtTotal += balAmt
        balance -= balAmt
        If discAmt <> 0 Then
            ro = objGridShower.dsGrid.Tables("SUMMARY").NewRow
            ro("ITEM") = "DISCOUNT"
            ro("VALUE") = Format(discAmt, "0.00")
            ro("RESULT") = "3"
            objGridShower.dsGrid.Tables("SUMMARY").Rows.Add(ro)
            balance -= discAmt
        End If
        If balWt <> 0 Then
            Dim rate As Double = GetRate_Purity(GetEntryDate(GetServerDate), "03")
            ro = objGridShower.dsGrid.Tables("SUMMARY").NewRow
            ro("ITEM") = "ADV-Weight(" & Format(balWt, "0.000") & " @" & Format(rate, "0.00") & ")"
            ro("VALUE") = Format(balWt * rate, "0.00")
            ro("RESULT") = "3"
            objGridShower.dsGrid.Tables("SUMMARY").Rows.Add(ro)
            AdvAmtTotal += (balWt * rate)
            balance -= (balWt * rate)
        End If
        ro = objGridShower.dsGrid.Tables("SUMMARY").NewRow
        ro("ITEM") = "BALANCE"
        ro("VALUE") = Format(balance, "0.00")
        ro("RESULT") = "3"
        objGridShower.dsGrid.Tables("SUMMARY").Rows.Add(ro)


        Dim Title As String = "ORDER/REPAIR ESTIMATE VIEW"
        Title += vbCrLf + "GIVEN ADVANCE : " & (Format((AdvAmtTotal / NetTotal) * 100, "0.00")).ToString & "%"
        objGridShower.dsGrid.Tables("SUMMARY").AcceptChanges()
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables("SUMMARY")
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(652, 400)
        objGridShower.Text = "ORDER/REPAIR ESTIMATE VIEW"
        objGridShower.lblTitle.Text = Title '"ORDER/REPAIR ESTIMATE VIEW"
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.FormReSize = False
        objGridShower.formuser = userId
        objGridShower.Show()
        With objGridShower.gridView
            .Columns("ITEM").Width = 200
            .Columns("TAGNO").Width = 80
            .Columns("PCS").Width = 70
            .Columns("GRSWT").Width = 100
            .Columns("NETWT").Width = 100
            .Columns("WEIGHT").Width = 80
            .Columns("VALUE").Width = 100
            .Columns("RESULT").Visible = False
            objGridShower.FormReSize = True
            .Columns("EMPNAME").Width = 100
            FormatGridColumns(objGridShower.gridView, False)
            .Columns("VALUE").DefaultCellStyle.Format = "0.00"
        End With
        For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
            If dgvRow.Cells("RESULT").Value.ToString = "2" Then
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf dgvRow.Cells("RESULT").Value.ToString = "3" Then
                dgvRow.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
                dgvRow.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("ITEM")))
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        ElseIf e.KeyCode = Keys.E Then
            If Not gridView.RowCount > 0 Then Exit Sub
            If gridView.CurrentRow Is Nothing Then Exit Sub
            EstimateView(gridView.CurrentRow.Cells("AORNO").Value.ToString, gridView.CurrentRow.Cells("COSTID").Value.ToString)
        ElseIf e.KeyCode = Keys.C Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
            If Not gridView.RowCount > 0 Then Exit Sub
            If gridView.CurrentRow Is Nothing Then Exit Sub
            Dim RowIndex As Integer = gridView.CurrentRow.Index
            Dim DtOrMast As New DataTable
            StrSql = " SELECT * FROM " & cnadmindb & "..ORMAST"
            StrSql += " WHERE SNO = '" & gridView.Rows(RowIndex).Cells("SNO").Value.ToString & "'"
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(DtOrMast)
            If Not DtOrMast.Rows.Count > 0 Then
                MsgBox("Order info not found", MsgBoxStyle.Information)
                gridView.Select()
                Exit Sub
            End If
            If gridView.Rows(RowIndex).Cells("ORDCANCEL").Value.ToString = "Y" Then
                MsgBox("Already Cancelled", MsgBoxStyle.Information)
                Exit Sub
            End If
            'StrSql = "SELECT ORSNO FROM " & cnAdminDb & "..ITEMTAG WHERE ORSNO = '" & gridView.Rows(RowIndex).Cells("SNO").Value.ToString & "'"
            'If objGPack.GetSqlValue(StrSql, , "-1") <> "-1" Then
            '    MsgBox("Tag was generated against this order entry." + vbCrLf + "First you should transfer that Order Tag to Regular tag", MsgBoxStyle.Information)
            '    Exit Sub
            'End If
            If gridView.Rows(RowIndex).Cells("TAGNO").Value.ToString <> "" Then
                MsgBox("Tag was generated against this order entry." + vbCrLf + "First you should transfer that Order Tag to Regular tag", MsgBoxStyle.Information)
                Exit Sub
            End If
            If MessageBox.Show("Do you want to cancel", "CANCEL", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                Dim objSecret As New frmAdminPassword()
                If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
                Dim objRemark As New frmBillRemark
                objRemark.Text = "Cancel Order"
                If objRemark.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
                Dim DtDelOrMast As New DataTable
                DtDelOrMast = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "DEL_ORMAST", cn)
                DtDelOrMast.Rows.Add()
                For Each Col As DataColumn In DtDelOrMast.Columns
                    DtDelOrMast.Rows(0).Item(Col.ColumnName) = DtOrMast.Rows(0).Item(Col.ColumnName)
                Next
                Dim Reason As String = objRemark.cmbRemark1_OWN.Text
                If Reason <> "" And objRemark.cmbRemark2_OWN.Text <> "" Then
                    Reason += "~" + objRemark.cmbRemark2_OWN.Text
                Else
                    Reason += objRemark.cmbRemark2_OWN.Text
                End If
                DtDelOrMast.Rows(0).Item("REASON") = Reason
                DtDelOrMast.Rows(0).Item("USERID") = userId
                DtDelOrMast.Rows(0).Item("UPDATED") = GetEntryDate(GetServerDate)
                DtDelOrMast.Rows(0).Item("UPTIME") = Date.Now.ToLongTimeString
                Try
                    tran = Nothing
                    tran = cn.BeginTransaction
                    InsertData(SyncMode.Transaction, DtDelOrMast, cn, tran, gridView.Rows(RowIndex).Cells("COSTID").Value.ToString)
                    StrSql = " UPDATE " & cnAdminDb & "..ORMAST SET ORDCANCEL = 'Y' WHERE SNO = '" & gridView.Rows(RowIndex).Cells("SNO").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, gridView.Rows(RowIndex).Cells("COSTID").Value.ToString)
                    tran.Commit()
                    tran = Nothing
                    MsgBox("Successfully Cancelled", MsgBoxStyle.Information)
                    gridView.Rows.RemoveAt(RowIndex)
                    gridView.Refresh()
                    gridView.Select()
                Catch ex As Exception
                    If Not tran Is Nothing Then tran.Rollback()
                    MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                    gridView.Select()
                End Try
            End If
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        e.Handled = True
        If UCase(e.KeyChar) = "N" Then
            If Not gridView.RowCount > 0 Then
                Exit Sub
            End If
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(gridView.FirstDisplayedCell.ColumnIndex)
            If gridView.CurrentRow Is Nothing Then Exit Sub

            Dim objOrderRep As New frmOrder
            objOrderRep.BillDate = gridView.CurrentRow.Cells("ORDATE").Value
            objOrderRep.BillCostId = gridView.CurrentRow.Cells("COSTID").Value.ToString
            objOrderRep.lblUserName.Text = cnUserName
            objOrderRep.lblSystemId.Text = systemId
            objOrderRep.lblCashCounter.Text = ""
            objOrderRep.Set916Rate(objOrderRep.BillDate)
            If Val(objOrderRep.lblGoldRate.Text) = 0 Then
                MsgBox("Rate not updated in Rate Master", MsgBoxStyle.Information)
                Exit Sub
            End If
            BrighttechPack.LanguageChange.Set_Language_Form(objOrderRep, LangId)
            objGPack.Validator_Object(objOrderRep)
            objOrderRep.MdiParent = Main
            objOrderRep.KeyPreview = True
            objOrderRep.MaximizeBox = False
            objOrderRep.FormBorderStyle = Windows.Forms.FormBorderStyle.None
            objOrderRep.WindowState = FormWindowState.Maximized
            objOrderRep.Dock = DockStyle.Fill
            objOrderRep.OrderNo = gridView.CurrentRow.Cells("AORNO").Value.ToString
            objOrderRep.Batchno = gridView.CurrentRow.Cells("BATCHNO").Value.ToString
            objOrderRep.lblBillDate.Text = gridView.CurrentRow.Cells("ORDATE").FormattedValue.ToString
            objOrderRep.OrdType = frmOrder.OrderType.OrderEdit
            objOrderRep.Show()
        ElseIf UCase(e.KeyChar) = "R" Then
            If Not gridView.RowCount > 0 Then
                Exit Sub
            End If
            If gridView.CurrentRow.Cells("SNO").Value.ToString = "" Then Exit Sub
            If gridView.CurrentRow.Cells("TAGNO").Value.ToString = "" Then MsgBox("Tag Not Found .", MsgBoxStyle.Information) : Exit Sub
            If gridView.CurrentRow.Cells("ODBATCHNO").Value.ToString <> "" Then MsgBox("Delivered .", MsgBoxStyle.Information) : Exit Sub
            Try
                tran = Nothing
                tran = cn.BeginTransaction
                StrSql = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET ORDREPNO = '',ORSNO = '' WHERE ORSNO = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, gridView.CurrentRow.Cells("COSTID").Value.ToString)
                StrSql = vbCrLf + " UPDATE " & cnAdminDb & "..ORIRDETAIL SET ORDSTATE_ID = 54 WHERE ORSNO = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                ExecQuery(SyncMode.Transaction, StrSql, cn, tran, gridView.CurrentRow.Cells("COSTID").Value.ToString)
                tran.Commit()
                tran = Nothing
                MsgBox("ReOrder Booked." & vbCrLf & "Order Tag Transfer to Regular Tag", MsgBoxStyle.Information)
                gridView.Refresh()
                gridView.Select()
                btnSearch_Click(Me, New EventArgs)
            Catch ex As Exception
                If Not tran Is Nothing Then tran.Rollback()
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                gridView.Select()
            End Try
        ElseIf UCase(e.KeyChar) = "U" Then
            If Not gridView.RowCount > 0 Then
                Exit Sub
            End If
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
            If gridView.CurrentRow.Cells("ORDCANCEL").Value.ToString = "Y" Then
                MsgBox("Cancelled Item Cannot Edit", MsgBoxStyle.Information)
                Exit Sub
            End If
            If gridView.CurrentRow.Cells("ODBATCHNO").Value.ToString <> "" Then
                MsgBox("Delivered Item Cannot edit", MsgBoxStyle.Information)
                Exit Sub
            End If
            If gridView.CurrentRow.Cells("SNO").Value.ToString = "" Then Exit Sub
            Dim obj As New OrderStatusUpdate
            obj.OrdSno = gridView.CurrentRow.Cells("SNO").Value.ToString
            obj.OrdRepNo = gridView.CurrentRow.Cells("AORNO").Value.ToString
            If obj.ShowDialog() = Windows.Forms.DialogResult.OK Then
                btnSearch_Click(Me, New EventArgs)
            End If
            Exit Sub
        ElseIf UCase(e.KeyChar) = "D" Then
            If Not gridView.RowCount > 0 Then
                Exit Sub
            End If
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(gridView.FirstDisplayedCell.ColumnIndex)
            If gridView.CurrentRow Is Nothing Then Exit Sub


            If (gridView.CurrentRow.Cells("ORTYPE").Value.ToString() = "R") Then
                Dim objRepair As New frmRepair()
                If (objRepair Is Nothing) Then Exit Sub
                objRepair.BillDate = gridView.CurrentRow.Cells("ORDATE").Value
                objRepair.BillCostId = gridView.CurrentRow.Cells("COSTID").Value.ToString
                objRepair.lblUserName.Text = cnUserName
                objRepair.lblSystemId.Text = systemId
                objRepair.lblCashCounter.Text = ""
                objRepair.Set916Rate(objRepair.BillDate)
                If Val(objRepair.lblGoldRate.Text) = 0 Then
                    MsgBox("Rate not updated in Rate Master", MsgBoxStyle.Information)
                    Exit Sub
                End If
                BrighttechPack.LanguageChange.Set_Language_Form(objRepair, LangId)
                objGPack.Validator_Object(objRepair)
                With objRepair
                    objRepair.MdiParent = Main
                    objRepair.KeyPreview = True
                    objRepair.MaximizeBox = False
                    objRepair.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                    objRepair.WindowState = FormWindowState.Maximized
                    objRepair.Dock = DockStyle.Fill
                    objRepair.RepairNo = gridView.CurrentRow.Cells("AORNO").Value.ToString
                    objRepair.Batchno = gridView.CurrentRow.Cells("BATCHNO").Value.ToString
                    objRepair.lblBillDate.Text = gridView.CurrentRow.Cells("ORDATE").FormattedValue.ToString
                    objRepair.RepType = frmRepair.RepairType.RepairUpdate
                    objRepair.RepairUpdSno = gridView.CurrentRow.Cells("SNO").Value.ToString
                    StrSql = vbCrLf + " SELECT IM.ITEMID,IM.ITEMNAME,SM.SUBITEMNAME"
                    StrSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = O.ITEMTYPEID)AS ITEMTYPE"
                    StrSql += vbCrLf + " ,(SELECT SUM(AMOUNT) FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.ORNO AND ISNULL(CANCEL,'')='')AS AMOUNT"
                    StrSql += vbCrLf + " ,O.SIZENO SIZEDESC"
                    StrSql += vbCrLf + " ,O.PCS,O.GRSWT,O.NETWT,O.RATE,O.WASTPER,O.WAST,O.MC,O.MCGRM,O.COMMPER,O.COMM,O.DESCRIPT,O.STYLENO,O.COMMPER,O.COMM,O.ORVALUE,O.TAX,O.PICTFILE,O.REASON"
                    StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST O "
                    StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = O.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = O.ITEMID AND SM.SUBITEMID = O.SUBITEMID"
                    StrSql += vbCrLf + " WHERE O.SNO = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                    Dim dt As New DataTable
                    Da = New OleDbDataAdapter(StrSql, cn)
                    Da.Fill(dt)
                    If Not dt.Rows.Count > 0 Then
                        MsgBox("Record not found", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    objRepair.Show()
                    Dim Row As DataRow = dt.Rows(0)
                    .txtODescription.Text = IIf(Row.Item("SUBITEMNAME").ToString <> "", Row.Item("SUBITEMNAME").ToString, Row.Item("ITEMNAME").ToString)
                    .txtOItemName.Text = Row.Item("ITEMNAME").ToString
                    .txtOPcs_NUM.Text = Row.Item("PCS").ToString
                    .txtOGrsWt_WET.Text = Row.Item("GRSWT").ToString
                    .txtONetWt_WET.Text = Row.Item("NETWT").ToString
                    .txtOMc_AMT.Text = Row.Item("MC").ToString
                    .txtOMcPerGrm_AMT.Text = Row.Item("MCGRM").ToString
                    .txtOAmount_AMT.Text = Row.Item("AMOUNT").ToString
                    .txtONatureOfRepair.Text = Row.Item("REASON").ToString
                    .txtAdjAdvance_AMT.Text = Row.Item("AMOUNT").ToString
                    .subItemName = Row.Item("SUBITEMNAME").ToString
                    Dim ItemId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .txtOItemName.Text & "'"))
                    Dim SubItemId As Integer = Nothing
                    If .subItemName <> Nothing Then
                        SubItemId = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .subItemName & "' AND ITEMID = " & ItemId & ""))
                        StrSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .subItemName & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .txtOItemName.Text & "')"
                    Else
                        StrSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .txtOItemName.Text & "'"
                    End If
                    If UCase(objGPack.GetSqlValue(StrSql)) = "Y" Then
                        .isStone = True
                    Else
                        .isStone = False
                    End If
                    If .isStone Then
                        StrSql = vbCrLf + " SELECT IM.ITEMNAME ITEM,SM.SUBITEMNAME SUBITEM,S.STNPCS PCS,S.STNWT WEIGHT,S.STNUNIT UNIT,S.CALCMODE CALC,S.STNRATE RATE"
                        StrSql += vbCrLf + " ,S.STNAMT AMOUNT,IM.DIASTONE METALID"
                        StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORSTONE AS S"
                        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
                        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = S.STNITEMID AND SM.SUBITEMID = S.STNSUBITEMID"
                        StrSql += vbCrLf + " WHERE S.ORSNO = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                        Dim dtSt As New DataTable
                        Da = New OleDbDataAdapter(StrSql, cn)
                        Da.Fill(dtSt)
                        Dim RoStDia As DataRow = Nothing
                        For Each RoSt As DataRow In dtSt.Rows
                            RoStDia = .objStone.dtGridStone.NewRow
                            For Each col As DataColumn In dtSt.Columns
                                RoStDia.Item(col.ColumnName) = RoSt.Item(col)
                            Next
                            .objStone.dtGridStone.Rows.Add(RoStDia)
                        Next
                        .objStone.CalcStoneWtAmount()
                        Dim stnWt As Double = Val(.objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
                        Dim stnAmt As Double = Val(.objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
                        .txtStAmount_AMT.Text = IIf(stnAmt <> 0, Format(stnAmt, "0.00"), Nothing)
                    End If

                    If ORADDITIONALDETAIL Then
                        Dim dCol As New DataColumn("KEYNO")
                        Dim dtad As New DataTable
                        dCol.AutoIncrement = True
                        dCol.AutoIncrementSeed = 0
                        dCol.AutoIncrementStep = 1
                        dtad.Columns.Add(dCol)
                        StrSql = "SELECT (SELECT TYPENAME FROM " & cnAdminDb & "..ORADMAST WHERE TYPEID = T.TYPEID  ) TYPENAME,VALUENAME  FROM " & cnAdminDb & "..ORADTRAN T WHERE ORSNO = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                        Da = New OleDbDataAdapter(StrSql, cn)
                        Da.Fill(dtad)
                        If dtad.Rows.Count > 0 Then
                            .objAddtionalDetails.dtOrAdditionalDetails = dtad.Copy
                        End If
                    End If

                    Dim PicPath As String = GetAdmindbSoftValue("PICPATH")
                    If Not PicPath.EndsWith("\") Then PicPath += "\"
                    If IO.File.Exists(PicPath & Row.Item("PICTFILE").ToString) Then
                        .picPath = PicPath & Row.Item("PICTFILE").ToString
                        AutoImageSizer(.picPath, .picImage, PictureBoxSizeMode.StretchImage)
                        .chkOImage.Checked = True
                        .grpImage.Visible = True
                        .UpdateFileImagePath = .picPath
                    End If
                End With
            Else
                Dim objOrderRep As New frmOrder
                If (objOrderRep Is Nothing) Then Exit Sub
                objOrderRep.BillDate = gridView.CurrentRow.Cells("ORDATE").Value
                objOrderRep.BillCostId = gridView.CurrentRow.Cells("COSTID").Value.ToString
                objOrderRep.lblUserName.Text = cnUserName
                objOrderRep.lblSystemId.Text = systemId
                objOrderRep.lblCashCounter.Text = ""
                objOrderRep.Set916Rate(objOrderRep.BillDate)
                If Val(objOrderRep.lblGoldRate.Text) = 0 Then
                    MsgBox("Rate not updated in Rate Master", MsgBoxStyle.Information)
                    Exit Sub
                End If
                BrighttechPack.LanguageChange.Set_Language_Form(objOrderRep, LangId)
                objGPack.Validator_Object(objOrderRep)

                With objOrderRep
                    objOrderRep.MdiParent = Main
                    objOrderRep.KeyPreview = True
                    objOrderRep.MaximizeBox = False
                    objOrderRep.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                    objOrderRep.WindowState = FormWindowState.Maximized
                    objOrderRep.Dock = DockStyle.Fill
                    objOrderRep.OrderNo = gridView.CurrentRow.Cells("AORNO").Value.ToString
                    objOrderRep.Batchno = gridView.CurrentRow.Cells("BATCHNO").Value.ToString
                    objOrderRep.lblBillDate.Text = gridView.CurrentRow.Cells("ORDATE").FormattedValue.ToString
                    objOrderRep.OrdType = frmOrder.OrderType.OrderUpdate
                    objOrderRep.OrderUpdSno = gridView.CurrentRow.Cells("SNO").Value.ToString
                    StrSql = vbCrLf + " SELECT IM.ITEMID,IM.ITEMNAME,SM.SUBITEMNAME"
                    StrSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = O.ITEMTYPEID)AS ITEMTYPE"
                    StrSql += vbCrLf + " ,(SELECT SUM(AMOUNT) FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.ORNO AND ISNULL(CANCEL,'')='')AS AMOUNT"
                    StrSql += vbCrLf + " ,O.SIZENO SIZEDESC"
                    StrSql += vbCrLf + " ,O.PCS,O.GRSWT,O.NETWT,O.RATE,O.WASTPER,O.WAST,O.MC,O.MCGRM,O.COMMPER,O.COMM,O.DESCRIPT,O.STYLENO,O.COMMPER,O.COMM,O.ORVALUE,O.TAX,O.PICTFILE"
                    StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST O "
                    StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = O.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = O.ITEMID AND SM.SUBITEMID = O.SUBITEMID"
                    StrSql += vbCrLf + " WHERE O.SNO = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                    Dim dt As New DataTable
                    Da = New OleDbDataAdapter(StrSql, cn)
                    Da.Fill(dt)
                    If Not dt.Rows.Count > 0 Then
                        MsgBox("Record not found", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    objOrderRep.Show()
                    Dim Row As DataRow = dt.Rows(0)
                    .txtOItemId.Text = Row.Item("ITEMID").ToString
                    .txtOParticular.Text = IIf(Row.Item("STYLENO").ToString <> "", Row.Item("STYLENO").ToString, Row.Item("ITEMNAME").ToString)
                    .txtOItem.Text = Row.Item("ITEMNAME").ToString
                    .txtOStyleNo.Text = Row.Item("STYLENO").ToString
                    .subItemName = Row.Item("SUBITEMNAME").ToString
                    .itemTypeName = Row.Item("ITEMTYPE").ToString
                    .txtOSize.Text = Row.Item("SIZEDESC").ToString
                    .txtOPcs_NUM.Text = Row.Item("PCS").ToString
                    .txtOGrsWt_WET.Text = Row.Item("GRSWT").ToString
                    .txtONetWt_WET.Text = Row.Item("NETWT").ToString
                    .txtORate_AMT.Text = Row.Item("RATE").ToString
                    .txtAdjAdvance_AMT.Text = Row.Item("AMOUNT").ToString
                    Dim ItemId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .txtOItem.Text & "'"))
                    Dim SubItemId As Integer = Nothing
                    If .subItemName <> Nothing Then
                        SubItemId = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .subItemName & "' AND ITEMID = " & ItemId & ""))
                        StrSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .subItemName & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .txtOItem.Text & "')"
                    Else
                        StrSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .txtOItem.Text & "'"
                    End If
                    If UCase(objGPack.GetSqlValue(StrSql)) = "Y" Then
                        .isStone = True
                    Else
                        .isStone = False
                    End If
                    If .isStone Then
                        StrSql = vbCrLf + " SELECT IM.ITEMNAME ITEM,SM.SUBITEMNAME SUBITEM,S.STNPCS PCS,S.STNWT WEIGHT,S.STNUNIT UNIT,S.CALCMODE CALC,S.STNRATE RATE"
                        StrSql += vbCrLf + " ,S.STNAMT AMOUNT,IM.DIASTONE METALID"
                        StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORSTONE AS S"
                        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
                        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = S.STNITEMID AND SM.SUBITEMID = S.STNSUBITEMID"
                        StrSql += vbCrLf + " WHERE S.ORSNO = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                        Dim dtSt As New DataTable
                        Da = New OleDbDataAdapter(StrSql, cn)
                        Da.Fill(dtSt)
                        Dim RoStDia As DataRow = Nothing
                        For Each RoSt As DataRow In dtSt.Rows
                            RoStDia = .objStone.dtGridStone.NewRow
                            For Each col As DataColumn In dtSt.Columns
                                RoStDia.Item(col.ColumnName) = RoSt.Item(col)
                            Next
                            .objStone.dtGridStone.Rows.Add(RoStDia)
                        Next
                        .objStone.CalcStoneWtAmount()
                        Dim stnWt As Double = Val(.objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
                        Dim stnAmt As Double = Val(.objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
                        .txtOOtherAmt_AMT.Text = IIf(stnAmt <> 0, Format(stnAmt, "0.00"), Nothing)
                    End If
                    If ORADDITIONALDETAIL Then
                        Dim dCol As New DataColumn("KEYNO")
                        Dim dtad As New DataTable
                        dCol.AutoIncrement = True
                        dCol.AutoIncrementSeed = 0
                        dCol.AutoIncrementStep = 1
                        dtad.Columns.Add(dCol)
                        StrSql = "SELECT (SELECT TYPENAME FROM " & cnAdminDb & "..ORADMAST WHERE TYPEID = T.TYPEID  ) TYPENAME,VALUENAME  FROM " & cnAdminDb & "..ORADTRAN T WHERE ORSNO = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                        Da = New OleDbDataAdapter(StrSql, cn)
                        Da.Fill(dtad)
                        If dtad.Rows.Count > 0 Then
                            .objAddtionalDetails.dtOrAdditionalDetails = dtad.Copy
                        End If
                    End If
                    .txtOWastagePer_Per.Clear()
                    .txtOMcPerGrm_AMT.Clear()
                    .txtOCommGrm_AMT.Clear()
                    .txtOWastage_WET.Text = IIf(Val(Row.Item("WAST").ToString) <> 0, Format(Val(Row.Item("WAST").ToString), "0.000"), Nothing)
                    .txtOMc_AMT.Text = IIf(Val(Row.Item("MC").ToString) <> 0, Format(Val(Row.Item("MC").ToString), "0.00"), Nothing)
                    .txtOCommision_AMT.Text = IIf(Val(Row.Item("COMM").ToString) <> 0, Format(Val(Row.Item("COMM").ToString), "0.00"), Nothing)
                    .txtOWastagePer_Per.Text = IIf(Val(Row.Item("WASTPER").ToString) <> 0, Format(Val(Row.Item("WASTPER").ToString), "0.000"), Nothing)
                    .txtOMcPerGrm_AMT.Text = IIf(Val(Row.Item("MCGRM").ToString) <> 0, Format(Val(Row.Item("MCGRM").ToString), "0.00"), Nothing)
                    .txtOCommGrm_AMT.Text = IIf(Val(Row.Item("COMMPER").ToString) <> 0, Format(Val(Row.Item("COMMPER").ToString), "0.00"), Nothing)
                    .txtODescription.Text = Row.Item("DESCRIPT").ToString
                    .txtOGrossAmount_AMT.Text = IIf(Val(Row.Item("ORVALUE").ToString) <> 0, Format(Val(Row.Item("ORVALUE").ToString), "0.00"), Nothing)
                    .txtOVat_AMT.Text = IIf(Val(Row.Item("TAX").ToString) <> 0, Format(Val(Row.Item("TAX").ToString), "0.00"), Nothing)
                    If .txtOStyleNo.Text = "" Then
                        Dim PicPath As String = GetAdmindbSoftValue("PICPATH")
                        If Not PicPath.EndsWith("\") Then PicPath += "\"
                        If IO.File.Exists(PicPath & Row.Item("PICTFILE").ToString) Then
                            .picPath = PicPath & Row.Item("PICTFILE").ToString
                            AutoImageSizer(.picPath, .picImage, PictureBoxSizeMode.StretchImage)
                            .chkOImage.Checked = True
                            .grpImage.Visible = True
                            .UpdateFileImagePath = .picPath
                        End If
                    Else
                        Dim defPic As String = GetAdmindbSoftValue("TAGCATALOGPATH")
                        If Not defPic.EndsWith("\") And defPic <> "" Then defPic += "\"
                        .picPath = defPic & Row.Item("PICTFILE").ToString
                        .UpdateFileImagePath = .picPath
                        AutoImageSizer(.picPath, .picImage, PictureBoxSizeMode.CenterImage)
                        .chkOImage.Checked = True
                        .grpImage.Visible = True
                        .btnBrowse.Visible = False
                    End If
                End With
            End If
        End If
        btnSearch_Click(Me, New EventArgs)
        'Dim defPic As String = GetAdmindbSoftValue("TAGCATALOGPATH")
        'If Not defPic.EndsWith("\") And defPic <> "" Then defPic += "\"
        'picPath = defPic & Row.Item("PCTFILE").ToString
        'AutoImageSizer(picPath, picImage, PictureBoxSizeMode.CenterImage)
        'chkOImage.Checked = True
        'grpDescription.Visible = True
        'grpImage.Visible = True
        'btnBrowse.Visible = False
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click, Label1.Click

    End Sub

    Private Sub txtOrderNo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOrderNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            CostId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'")
            StrSql = vbCrLf + "  SELECT"
            StrSql += vbCrLf + "  	 DISTINCT SUBSTRING(ORNO,6,20)ORNO,O.COMPANYID COMPANYID_HIDE,O.COSTID COSTID_HIDE"
            StrSql += vbCrLf + "  	,PNAME"
            StrSql += vbCrLf + "  	,CASE WHEN ISNULL(DOORNO,'') = '' THEN ADDRESS1 ELSE ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') END ADDRESS1"
            StrSql += vbCrLf + "  	,ADDRESS2,MOBILE"
            StrSql += vbCrLf + "  	,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = O.COSTID) COSTCENTRE"
            StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ORMAST AS O LEFT OUTER JOIN " & cnAdminDb & "..CUSTOMERINFO AS C"
            StrSql += vbCrLf + "  ON O.BATCHNO = C.BATCHNO"
            StrSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..PERSONALINFO AS P"
            StrSql += vbCrLf + "  ON C.PSNO = P.SNO WHERE ISNULL(O.CANCEL,'') != 'Y'"
            StrSql += vbCrLf + "  AND ISNULL(O.COSTID,'') = '" & CostId & "'"
            StrSql += vbCrLf + "  AND ISNULL(O.ODBATCHNO,'')=''"
            Dim dr As DataRow
            dr = Nothing
            dr = BrighttechPack.SearchDialog.Show_R("Find Order No", StrSql, cn, , , , , , , , False)
            If dr IsNot Nothing Then
                txtOrderNo.Text = dr.Item("ORNO").ToString
            End If
            txtOrderNo.SelectAll()
        End If
    End Sub
End Class