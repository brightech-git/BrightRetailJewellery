Imports System.Data.OleDb
Public Class FRM_ORDERADVANCE
    Dim objGridShower As frmGridDispDia
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim StrSql As String
    Dim DA As OleDbDataAdapter
    Dim CMD As OleDbCommand
    Dim dtSoftKeys As DataTable = GetAdmindbSoftValueAll()
    Dim READYITEMBOOKING As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "READYITEMBOOKING", "N") = "Y", True, False)

    Private Sub FRM_ORDERADVANCE_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub FRM_ORDERADVANCE_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        StrSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        StrSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        DA = New OleDbDataAdapter(StrSql, cn)
        DA.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)

        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        StrSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(StrSql, cmbMetal, False, False)

        StrSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        StrSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        DA = New OleDbDataAdapter(StrSql, cn)
        DA.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        btnNew_Click(Me, New EventArgs)
        If READYITEMBOOKING Then rbtbooked.Visible = True Else rbtbooked.Visible = False
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim dtGrid As New DataTable
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        StrSql = " IF OBJECT_ID('TEMPDB..#OR_ADVANCE') IS NOT NULL DROP TABLE #OR_ADVANCE"
        CMD = New OleDbCommand(StrSql, cn) : CMD.CommandTimeout = 1000 : CMD.ExecuteNonQuery()
        StrSql = vbCrLf + "  SELECT DISTINCT RTRIM(SUBSTRING(ORNO,6,3) + ' ' + SUBSTRING(ORNO,9,LEN(ORNO)))AS ORNO"
        StrSql += vbCrLf + "  ,CONVERT(VARCHAR,ORDATE,103)ORDATE,CONVERT(VARCHAR,O.DUEDATE,103)DUEDATE"
        StrSql += vbCrLf + "  ,PE.ACCODE AS PARTYCODE,PE.PNAME AS CUSTOMER"
        StrSql += vbCrLf + "  ,PE.MOBILE AS MOBILE"
        StrSql += vbCrLf + "  ,(SELECT SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE 0 END) FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.ORNO AND ISNULL(CANCEL,'') = '' AND COSTID = O.COSTID)AS [ADVWT REC]"
        StrSql += vbCrLf + "  ,(SELECT SUM(CASE WHEN RECPAY = 'P' THEN GRSWT ELSE 0 END) FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.ORNO AND ISNULL(CANCEL,'') = '' AND COSTID = O.COSTID)AS [ADVWT PAY]"
        StrSql += vbCrLf + "  ,(SELECT SUM(CASE WHEN RECPAY = 'R' THEN GRSWT ELSE -1*GRSWT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.ORNO AND ISNULL(CANCEL,'')='' AND COSTID = O.COSTID)AS [ADVWT BAL]"
        StrSql += vbCrLf + "  ,(SELECT SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END) FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.ORNO AND ISNULL(CANCEL,'') = '' AND COSTID = O.COSTID)AS [ADVAMT REC]"
        StrSql += vbCrLf + "  ,(SELECT SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END) FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.ORNO AND ISNULL(CANCEL,'') = '' AND COSTID = O.COSTID)AS [ADVAMT PAY]"
        StrSql += vbCrLf + "  ,(SELECT SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE RUNNO = O.ORNO AND ISNULL(CANCEL,'') = '' AND COSTID = O.COSTID)AS [ADVAMT BAL]"
        StrSql += vbCrLf + "  ,CONVERT(INT,1)RESULT"
        StrSql += vbCrLf + "  ,CONVERT(vARCHAR(3),NULL)COLHEAD"
        StrSql += vbCrLf + "  INTO #OR_ADVANCE"
        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ORMAST AS O"
        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CUSTOMERINFO AS CU ON CU.BATCHNO = O.BATCHNO"
        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..PERSONALINFO AS PE ON PE.SNO = CU.PSNO"
        If chkAsOnDate.Checked Then
            StrSql += vbCrLf + "  WHERE O.ORDATE <= '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
        Else
            StrSql += vbCrLf + "  WHERE O.ORDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND  '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        End If
        If READYITEMBOOKING Then
            If rbtorder.Checked Then StrSql += vbCrLf + " AND  O.ORTYPE='O'"
            If rbtRepair.Checked Then StrSql += vbCrLf + " AND  O.ORTYPE='R'"
            If rbtbooked.Checked Then StrSql += vbCrLf + " AND  O.ORTYPE='B'"
            If rbtboth.Checked Then StrSql += vbCrLf + " AND  O.ORTYPE in ('O','R','B')"
        Else
            If rbtorder.Checked Then StrSql += vbCrLf + " AND  O.ORTYPE='O'"
            If rbtRepair.Checked Then StrSql += vbCrLf + " AND  O.ORTYPE='R'"
            If rbtbooked.Checked Then StrSql += vbCrLf + " AND  O.ORTYPE='B'"
            If rbtboth.Checked Then StrSql += vbCrLf + " AND  O.ORTYPE in ('O','R','B')"
        End If
        'StrSql += vbCrLf + "  AND O.ORTYPE = 'O'" + vbCrLf 'FOR ORDER ONLY

        StrSql += vbCrLf + "  AND ISNULL(O.ORDCANCEL,'') = ''"
        If txtOrderNo.Text <> "" Then
            StrSql += vbCrLf + "  AND SUBSTRING(O.ORNO,6,20) = '" & txtOrderNo.Text & "'"
        End If
        If txtSalesMan_NUM.Text <> "" Then
            StrSql += vbCrLf + "  AND O.EMPID = " & Val(txtSalesMan_NUM.Text) & ""
        End If
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            StrSql += vbCrLf + "  AND O.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
        End If
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            StrSql += vbCrLf + "  AND O.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            StrSql += vbCrLf + "  AND O.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            StrSql += vbCrLf + "  and O.COSTID in"
            StrSql += vbCrLf + " (select CostId from " & cnAdminDb & "..CostCentre where CostName in (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        StrSql += vbCrLf + "  AND ISNULL(O.CANCEL,'') = ''"
        CMD = New OleDbCommand(StrSql, cn) : CMD.CommandTimeout = 1000 : CMD.ExecuteNonQuery()

        If (chkPendingOnly.Checked) Then
            StrSql = " IF (SELECT COUNT(*) FROM #OR_ADVANCE)>0"
            StrSql += " BEGIN"
            StrSql += " INSERT INTO #OR_ADVANCE(CUSTOMER,RESULT,COLHEAD"
            StrSql += " ,[ADVWT REC],[ADVWT PAY],[ADVWT BAL]"
            StrSql += " ,[ADVAMT REC],[ADVAMT PAY],[ADVAMT BAL]"
            StrSql += " )"
            StrSql += " SELECT 'GRAND TOTAL',3,'G'"
            StrSql += " ,SUM([ADVWT REC]),SUM([ADVWT PAY]),SUM([ADVWT BAL])"
            StrSql += " ,SUM([ADVAMT REC]),SUM([ADVAMT PAY]),SUM([ADVAMT BAL])"
            StrSql += " FROM #OR_aDVANCE WHERE ISNULL([ADVAMT BAL],0) <> 0"
            StrSql += " END"
            CMD = New OleDbCommand(StrSql, cn) : CMD.CommandTimeout = 1000 : CMD.ExecuteNonQuery()

            StrSql = " UPDATE #OR_ADVANCE SET "
            StrSql += vbCrLf + " [ADVWT REC] = CASE WHEN ISNULL([ADVWT REC],0) <> 0 THEN [ADVWT REC] ELSE NULL END"
            StrSql += vbCrLf + " ,[ADVWT PAY] = CASE WHEN ISNULL([ADVWT PAY],0) <> 0 THEN [ADVWT PAY] ELSE NULL END"
            StrSql += vbCrLf + " ,[ADVWT BAL] = CASE WHEN ISNULL([ADVWT BAL],0) <> 0 THEN [ADVWT BAL] ELSE NULL END"
            StrSql += vbCrLf + " ,[ADVAMT REC] = CASE WHEN ISNULL([ADVAMT REC],0) <> 0 THEN [ADVAMT REC] ELSE NULL END"
            StrSql += vbCrLf + " ,[ADVAMT PAY] = CASE WHEN ISNULL([ADVAMT PAY],0) <> 0 THEN [ADVAMT PAY] ELSE NULL END"
            StrSql += vbCrLf + " ,[ADVAMT BAL] = CASE WHEN ISNULL([ADVAMT BAL],0) <> 0 THEN [ADVAMT BAL] ELSE NULL END"
            CMD = New OleDbCommand(StrSql, cn) : CMD.CommandTimeout = 1000 : CMD.ExecuteNonQuery()

            StrSql = "  SELECT * FROM #OR_ADVANCE WHERE NOT (ISNULL([ADVAMT BAL],0) = 0 )"
            StrSql += vbCrLf + "  ORDER BY RESULT,ORDATE,ORNO"

            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Else
            StrSql = " IF (SELECT COUNT(*) FROM #OR_ADVANCE)>0"
            StrSql += " BEGIN"
            StrSql += " INSERT INTO #OR_ADVANCE(CUSTOMER,RESULT,COLHEAD"
            StrSql += " ,[ADVWT REC],[ADVWT PAY],[ADVWT BAL]"
            StrSql += " ,[ADVAMT REC],[ADVAMT PAY],[ADVAMT BAL]"
            StrSql += " )"
            StrSql += " SELECT 'GRAND TOTAL',3,'G'"
            StrSql += " ,SUM([ADVWT REC]),SUM([ADVWT PAY]),SUM([ADVWT BAL])"
            StrSql += " ,SUM([ADVAMT REC]),SUM([ADVAMT PAY]),SUM([ADVAMT BAL])"
            StrSql += " FROM #OR_aDVANCE"
            StrSql += " END"

            CMD = New OleDbCommand(StrSql, cn) : CMD.CommandTimeout = 1000 : CMD.ExecuteNonQuery()
            StrSql = " UPDATE #OR_ADVANCE SET "
            StrSql += vbCrLf + " [ADVWT REC] = CASE WHEN ISNULL([ADVWT REC],0) <> 0 THEN [ADVWT REC] ELSE NULL END"
            StrSql += vbCrLf + " ,[ADVWT PAY] = CASE WHEN ISNULL([ADVWT PAY],0) <> 0 THEN [ADVWT PAY] ELSE NULL END"
            StrSql += vbCrLf + " ,[ADVWT BAL] = CASE WHEN ISNULL([ADVWT BAL],0) <> 0 THEN [ADVWT BAL] ELSE NULL END"
            StrSql += vbCrLf + " ,[ADVAMT REC] = CASE WHEN ISNULL([ADVAMT REC],0) <> 0 THEN [ADVAMT REC] ELSE NULL END"
            StrSql += vbCrLf + " ,[ADVAMT PAY] = CASE WHEN ISNULL([ADVAMT PAY],0) <> 0 THEN [ADVAMT PAY] ELSE NULL END"
            StrSql += vbCrLf + " ,[ADVAMT BAL] = CASE WHEN ISNULL([ADVAMT BAL],0) <> 0 THEN [ADVAMT BAL] ELSE NULL END"
            CMD = New OleDbCommand(StrSql, cn) : CMD.CommandTimeout = 1000 : CMD.ExecuteNonQuery()

            StrSql = " SELECT * FROM #OR_ADVANCE"
            StrSql += " WHERE "
            StrSql += " NOT (ISNULL([ADVWT REC],0) = 0 AND ISNULL([ADVWT PAY],0) = 0 AND ISNULL([ADVAMT REC],0) = 0 AND ISNULL([ADVAMT PAY],0) = 0) "
            StrSql += vbCrLf + "  ORDER BY RESULT,ORDATE,ORNO"

            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        End If

        DA = New OleDbDataAdapter(StrSql, cn)
        DA.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        Prop_Sets()
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Dim tit As String = ""
        If READYITEMBOOKING Then
            objGridShower.Text = "ORDER/REPAIR ADVANCE"
            tit = "ORDER/REPAIR ADVANCE REPORT" + vbCrLf
        Else
            objGridShower.Text = "ORDER/REPAIR/BOOKED ADVANCE"
            tit = "ORDER/REPAIR/BOOKED ADVANCE REPORT" + vbCrLf
        End If
        If chkAsOnDate.Checked Then
            tit += " AS ON " & dtpFrom.Text & ""
        Else
            tit += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        End If
        tit += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        'DataGridView_SummaryFormatting(objGridShower.gridView)

        With objGridShower.gridView
            .Columns("KEYNO").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
        End With
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.Show()

        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        'objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
    End Sub
   
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        txtOrderNo.Clear()
        cmbMetal.Text = "ALL"
        txtSalesMan_NUM.Clear()
        Prop_Gets()
        chkAsOnDate.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, e)
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New FRM_ORDERADVANCE_Properties
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        obj.p_txtOrderNo = txtOrderNo.Text
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_txtSalesMan_NUM = txtSalesMan_NUM.Text
        obj.p_chkAsOnDate = chkAsOnDate.Checked
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        SetSettingsObj(obj, Me.Name, GetType(FRM_ORDERADVANCE_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New FRM_ORDERADVANCE_Properties
        GetSettingsObj(obj, Me.Name, GetType(FRM_ORDERADVANCE_Properties))
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        txtOrderNo.Text = obj.p_txtOrderNo
        cmbMetal.Text = obj.p_cmbMetal
        txtSalesMan_NUM.Text = obj.p_txtSalesMan_NUM
        chkAsOnDate.Checked = obj.p_chkAsOnDate
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, cnCostName)
    End Sub

    Private Sub chkPendingOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPendingOnly.CheckedChanged

    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        If chkAsOnDate.Checked Then
            Label2.Visible = False
            chkAsOnDate.Text = "As OnDate"
            dtpTo.Visible = False
        Else
            chkAsOnDate.Text = "Date From"
            Label2.Visible = True
            dtpTo.Visible = True
        End If
    End Sub
End Class
Public Class FRM_ORDERADVANCE_Properties
    Private chkAsOnDate As Boolean = True
    Public Property p_chkAsOnDate() As Boolean
        Get
            Return chkAsOnDate
        End Get
        Set(ByVal value As Boolean)
            chkAsOnDate = value
        End Set
        'New list(of string) {"a", "b", "c", "d"}
    End Property
    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCmbCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
        End Set
    End Property
    Private txtOrderNo As String = ""
    Public Property p_txtOrderNo() As String
        Get
            Return txtOrderNo
        End Get
        Set(ByVal value As String)
            txtOrderNo = value
        End Set
    End Property
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private txtSalesMan_NUM As String = ""
    Public Property p_txtSalesMan_NUM() As String
        Get
            Return txtSalesMan_NUM
        End Get
        Set(ByVal value As String)
            txtSalesMan_NUM = value
        End Set
    End Property
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property


End Class