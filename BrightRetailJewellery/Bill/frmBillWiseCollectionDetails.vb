Imports System.Data.OleDb
Public Class frmBillWiseCollectionDetails
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim dtGridView As New DataTable
    Dim dtOtherDetails As New DataTable
    'CALID-605:CURRECTION-ADD OPTION TO TALLY THE BILL DIFFERENCE AMOUNT: ALTER BY SATHYA
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'BATCHNO,TRANNO,TRANDATE,PARTICULAR,DEBIT,CREDIT,DIFF,RESULT
        With dtGridView
            .Columns.Add("BATCHNO", GetType(String))
            .Columns.Add("TRANNO", GetType(Integer))
            .Columns.Add("TRANDATE", GetType(Date))
            .Columns.Add("ACCODE", GetType(String))
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("DEBIT", GetType(Double))
            .Columns.Add("CREDIT", GetType(Double))
            .Columns.Add("DIFF", GetType(Double))
            .Columns.Add("RESULT", GetType(Integer))
        End With
        gridView.DataSource = dtGridView
        FormatGridColumns(gridView)
        With gridView
            .Columns("BATCHNO").Width = 110
            .Columns("TRANNO").Width = 60
            .Columns("TRANDATE").Width = 80
            .Columns("ACCODE").Width = 70
            .Columns("PARTICULAR").Width = 350
            .Columns("DEBIT").Width = 120
            .Columns("CREDIT").Width = 120
            .Columns("DIFF").Width = 80
            .Columns("RESULT").Visible = False
        End With
    End Sub
    Function funcFiltration() As String
        Dim qry As String
        qry = " WHERE TRANDATE BETWEEN ' " & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        If txtBatchNo.Text <> "" Then qry += " AND BATCHNO = '" & txtBatchNo.Text & "'"
        If txtTranNo.Text <> "" Then
            qry += " AND EXISTS (SELECT 1 FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND TRANNO = " & Val(txtTranNo.Text) & ")"
            'qry += " AND TRANNO = '" & txtTranNo.Text & "'"
        End If
        If txtSystemId.Text <> "" Then
            qry += " AND SYSTEMID = '" & txtSystemId.Text & "'"
        End If
        qry += " AND ISNULL(CANCEL,'') = ''"
        qry += " AND T.COMPANYID = '" & strCompanyId & "'"
        Return qry
    End Function
    Function funcSetGridViewStyle() As Integer
        With gridView
            With .Columns("BrefNo")
                .HeaderText = "BATCH NO"
                .Width = 90
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TranNo")
                .HeaderText = "TRAN NO"
                .Width = 60
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TranDate")
                .HeaderText = "TRAN DATE"
                .Width = 80
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ACCODE")
                .HeaderText = "ACCODE"
                .Width = 80
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("AcName")
                .HeaderText = "AC NAME"
                .Width = 300
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("Debit")
                .HeaderText = "DEBIT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 80
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("Credit")
                .HeaderText = "CREDIT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 80
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("Diff")
                .HeaderText = "DIFF"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 80
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("Result")
                .Visible = False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

        End With
        For rwIndex As Integer = 0 To gridView.RowCount - 1
            With gridView.Rows(rwIndex)
                If Val(.Cells("DEBIT").Value) > 0 Then
                    .Cells("DEBIT").Style.BackColor = Color.Lavender
                End If
                If Val(.Cells("CREDIT").Value) > 0 Then
                    .Cells("CREDIT").Style.BackColor = Color.LavenderBlush
                End If
                If Val(.Cells("DIFF").Value) <> 0 Then
                    .Cells("DIFF").Style.BackColor = Color.LightGreen
                End If
            End With
        Next
    End Function

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim selCost As String = Nothing
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        pnlHeading.Visible = False
        dtGridView.Rows.Clear()
        dtGridView.AcceptChanges()

        If chkCmbCostCentre.Text = "ALL" Then
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += " ORDER BY COSTNAME"
            Dim dt As New DataTable()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    selCost += "'"
                    selCost += dt.Rows(i).Item("COSTID").ToString
                    selCost += "'"
                    selCost += ","
                Next
                If selCost <> "" Then
                    selCost = Mid(selCost, 1, selCost.Length - 1)
                End If
            End If
        ElseIf chkCmbCostCentre.Text <> "" Then
            Dim sql As String = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & ")"
            Dim dtCost As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtCost)
            If dtCost.Rows.Count > 0 Then
                For j As Integer = 0 To dtCost.Rows.Count - 1
                    selCost += "'"
                    selCost += dtCost.Rows(j).Item("COSTID").ToString + ""
                    selCost += "'"
                    selCost += ","
                Next
                If selCost <> "" Then
                    selCost = Mid(selCost, 1, selCost.Length - 1)
                End If
            End If
        End If

        strSql = " SELECT BATCHNO,BATCHNO AS BATCHNO1"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 THEN SNO ELSE NULL END AS SNO"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 THEN TRANNO ELSE NULL END AS TRANNO"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 THEN TRANDATE ELSE NULL END AS TRANDATE"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 THEN ACCODE ELSE NULL END AS ACCODE"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 THEN ACNAME ELSE NULL END AS PARTICULAR"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 AND DEBIT > 0 THEN DEBIT ELSE NULL END AS DEBIT"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 AND CREDIT > 0 THEN CREDIT ELSE NULL END AS CREDIT"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 THEN CONTRA ELSE NULL END AS CONTRA"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 THEN TRANTYPE ELSE NULL END AS TRANTYPE"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 THEN CHQNO ELSE NULL END AS CHQNO"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 THEN BANK ELSE NULL END AS BANK"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 AND REFDATE IS NOT NULL  THEN "
        strSql += vbCrLf + " CONVERT(VARCHAR(15),REFNO,105)+ ' / ' + CONVERT(VARCHAR(15),REFDATE,105)  "
        strSql += vbCrLf + " WHEN RESULT=1 AND REFDATE IS NULL THEN REFNO ELSE NULL END AS REFNO"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 THEN CHQDATE ELSE NULL END  AS CHQDATE"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 THEN REMARK1 ELSE NULL END AS REMARK1"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 THEN REMARK2 ELSE NULL END AS REMARK2"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 THEN COSTNAME ELSE NULL END AS COSTNAME"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 1 THEN COMPANY ELSE NULL END AS COMPANY"
        strSql += vbCrLf + " ,CASE WHEN RESULT = 2 AND ISNULL(DIFF,0) <> 0 THEN DIFF ELSE NULL END AS DIFF"
        strSql += vbCrLf + " ,RESULT"
        strSql += vbCrLf + " ,FLAG FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT SNO,REFNO,REFDATE,"
        strSql += vbCrLf + " BATCHNO,TRANNO,TRANDATE,T.ACCODE,ISNULL(AC.ACNAME,'**NH**') ACNAME "
        strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END AS DEBIT"
        strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END AS CREDIT"
        strSql += vbCrLf + " ,ISNULL(CC.ACNAME,'**NH**')  AS CONTRA"
        strSql += vbCrLf + " ,T.PAYMODE AS TRANTYPE, T.CHQCARDNO AS CHQNO,T.CHQCARDREF AS BANK,T.CHQDATE,T.REMARK1,T.REMARK2"
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=T.COSTID) AS COSTNAME"
        strSql += vbCrLf + " ,(SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID=T.COMPANYID) AS COMPANY"
        strSql += vbCrLf + " ,0 AS DIFF"
        strSql += vbCrLf + " ,1 RESULT"
        strSql += vbCrLf + " ,T.FLAG FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE = T.ACCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD CC ON CC.ACCODE = T.CONTRA"
        strSql += funcFiltration()
        If chkCmbCostCentre.Text.ToUpper <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " AND T.COSTID IN (" & selCost & ")"
        If chkDiffVoucherOnly.Checked = True Then
            strSql += vbCrLf + " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN  V"
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD VAC ON VAC.ACCODE = V.ACCODE"
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD VCC ON VCC.ACCODE = V.CONTRA"
            Dim functionfilter As String = Replace(funcFiltration(), "T.COMPANYID", "V.COMPANYID")
            strSql += functionfilter & " GROUP BY BATCHNO HAVING SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE AMOUNT*-1 END) <> 0)"
        End If
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT '' SNO,''REFNO,''REFDATE,"
        strSql += vbCrLf + " BATCHNO,''TRANNO,''TRANDATE,''ACCODE,''ACNAME"
        strSql += vbCrLf + " ,0 AS DEBIT"
        strSql += vbCrLf + " ,0 AS CREDIT"
        strSql += vbCrLf + ",'' CONTRA,'' TRANTYPE,''CHQNO,''BANK,NULL CHQDATE,''REMARK1,''REMARK2,''COSTID,''COMPANID"
        strSql += vbCrLf + " ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE AMOUNT*-1 END)AS DIFF"
        strSql += vbCrLf + " ,2 RESULT"
        strSql += vbCrLf + " ,'' FLAG FROM " & cnStockDb & "..ACCTRAN T"
        strSql += funcFiltration()
        If chkCmbCostCentre.Text.ToUpper <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " AND T.COSTID IN (" & selCost & ")"
        strSql += vbCrLf + " GROUP BY BATCHNO"
        If chkDiffVoucherOnly.Checked = True Then
            strSql += vbCrLf + " HAVING SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE AMOUNT*-1 END) <> 0"
        End If
        strSql += vbCrLf + " )AS B"
        strSql += vbCrLf + " ORDER BY BATCHNO1,RESULT,TRANNO,FLAG"
        dtGridView.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridView)
        dtGridView.AcceptChanges()
        'gridView.DataSource = dtGridView
        'funcSetGridViewStyle()
        For Each dgvRow As DataGridViewRow In gridView.Rows
            If Val(dgvRow.Cells("DEBIT").Value.ToString) <> 0 Then dgvRow.Cells("DEBIT").Style.BackColor = Color.Lavender
            If Val(dgvRow.Cells("CREDIT").Value.ToString) <> 0 Then dgvRow.Cells("CREDIT").Style.BackColor = Color.LavenderBlush
            If Val(dgvRow.Cells("DIFF").Value.ToString) <> 0 Then dgvRow.Cells("DIFF").Style.BackColor = Color.LightGreen
            If Val(dgvRow.Cells("RESULT").Value) = 2 Then dgvRow.Cells("BATCHNO").Value = ""
        Next
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("There is no Record", MsgBoxStyle.Information)
            dtpFrom.Focus()
        Else
            pnlHeading.Visible = True
            Dim title As String = Nothing
            title = " BILL REFNO WISE COLLECTION DETAILS FROM " + dtpFrom.Text + " TO " + dtpTo.Text + " For " + strCompanyName
            If chkCmbCostCentre.Text <> "ALL" Then
                title += " at " + chkCmbCostCentre.Text
            End If
            lblTitle.Text = title
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            With gridView
                .Columns("CONTRA").Visible = False
                .Columns("SNO").Visible = False
                .Columns("BATCHNO1").Visible = False
                .Columns("TRANTYPE").Visible = False
                .Columns("CHQNO").Visible = False
                .Columns("BANK").Visible = False
                .Columns("CHQDATE").Visible = False
                .Columns("REMARK1").Visible = False
                .Columns("REMARK2").Visible = False
            End With
            gridView.Focus()
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        dtGridView.Clear()
        pnlHeading.Visible = False
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        funcClearOtherDetails()
        dtpFrom.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblHelp.Visible = True
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.P) Or UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.X) Or UCase(e.KeyChar) = "X" Then
            btnExcel_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmBillWiseCollectionDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmBillWiseCollectionDetails_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gridView.ColumnHeadersVisible = True
        gridView.RowTemplate.Height = 21
        'gridView.Font = New Font("VERDANA", 9)
        btnNew_Click(Me, New EventArgs)
        strSql = vbCrLf + " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += vbCrLf + " WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%' ORDER BY RESULT,COSTNAME"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtCost As New DataTable()
        da.Fill(dtCost)
        chkCmbCostCentre.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCost, "COSTNAME", , "ALL")
    End Sub
    Function funcClearOtherDetails() As Integer
        strSql = " Select 'TRANTYPE'Col1,''Col2,'CONTRA'Col3,''COL4,''Col5,''Col6"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  Select 'CHEQUE NO'Col1,''Col2,'BANK'Col3,''Col4,''Col5,''Col6"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  Select 'CHEQUE DATE'Col1,''Col2,'NARRATION 1'Col3,''Col4,''Col5,''Col6"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  Select 'REFNO'Col1,''Col2,'NARRATION 2'Col3,''Col4,''Col5,''Col6"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  Select 'COSTCENTRE'Col1,''Col2,'COMPANY'Col3,''Col4,''Col5,''Col6"
        dtOtherDetails.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtOtherDetails)
        gridMiscDetails.DataSource = dtOtherDetails
        funcSetOtherDetailsStyle()
    End Function
    Function funcSetOtherDetailsStyle() As Integer
        With gridMiscDetails
            With .Columns("Col1")
                .Width = 100
                .DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("Col2")
                .Width = 175
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("Col3")
                .Width = 100
                .Resizable = DataGridViewTriState.False
                .DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control
            End With
            With .Columns("Col4")
                .Width = 400
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("Col5")
                .Width = 110
                .Resizable = DataGridViewTriState.False
                '.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control
                .Resizable = DataGridViewTriState.False
            End With
            With .Columns("Col6")
                .Width = 115
                .Resizable = DataGridViewTriState.False
            End With
            .ClearSelection()
            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Function
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Function FlagTran(ByVal Tran As String, ByVal Flag As String) As String
        If Tran.Trim <> "CH" Then
            Return Tran
        End If
        ''If Flag = "C" Then
        ''    Return "CH [CHEQUE]"
        ''ElseIf Flag = "N" Then
        ''    Return "CH [NEFT]"
        ''ElseIf Flag = "I" Then
        ''    Return "CH [IMPS]"
        ''ElseIf Flag = "R" Then
        ''    Return "CH [RTGS]"
        ''ElseIf Flag = "F" Then
        ''    Return "CH [FUND TRANSFER]"
        ''End If

        If Flag <> "" Then
            strSql = " SELECT CASE WHEN ISNULL(X.MODENAME,'')<>'' THEN X.MODENAME ELSE 'CHEQUE' END MODENAME  FROM ( "
            strSql += " SELECT ISNULL((SELECT  ISNULL(MODENAME,'')MODENAME FROM " & cnAdminDb & "..PAYMENTMODE WHERE CONVERT(VARCHAR,MODEID)='" & Flag & "' GROUP BY MODENAME),'')MODENAME "
            strSql += " )X "
            Return GetSqlValue(cn, strSql)
        End If

        Return ""
    End Function
    Private Sub gridView_RowEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        Dim rw As Integer = e.RowIndex
        If gridView.RowCount > 0 Then
            With gridView.Rows(rw)
                strSql = " Select 'TRANTYPE'Col1,'" & FlagTran(IIf(IsDBNull(.Cells("TRANTYPE").Value), "", .Cells("TRANTYPE").Value), IIf(IsDBNull(.Cells("FLAG").Value), "", .Cells("FLAG").Value)) & "'Col2,"
                strSql += vbCrLf + "  'CONTRA'Col3,'" & IIf(IsDBNull(.Cells("CONTRA").Value), "", .Cells("CONTRA").Value) & "'COL4,''Col5,''Col6"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  Select 'CHEQUE NO'Col1,'" & .Cells("CHQNO").Value & "'Col2,'BANK'Col3,'" & .Cells("BANK").Value & "'Col4,''Col5,''Col6"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  Select 'CHEQUE DATE'Col1,'" & .Cells("CHQDATE").Value & "'Col2,'NARRATION 1'Col3,'" & .Cells("REMARK1").Value & "'Col4,''Col5,''Col6"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  Select 'REFNO'Col1,'" & .Cells("REFNO").Value & "'Col2,'NARRATION 2'Col3,'" & .Cells("REMARK2").Value & "'Col4,''Col5,''Col6"
                strSql += vbCrLf + "  UNION ALL"
                strSql += vbCrLf + "  Select 'COSTCENTRE'Col1,'" & .Cells("COSTNAME").Value & "'Col2,'COMPANY'Col3,'" & .Cells("COMPANY").Value & "'Col4,''Col5,''Col6"
            End With
            dtOtherDetails.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtOtherDetails)
            gridMiscDetails.DataSource = dtOtherDetails
            funcSetOtherDetailsStyle()

        End If
    End Sub
    '605
    Private Sub gridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.I Then
            If gridView.RowCount < 0 Then Exit Sub
            With gridView.CurrentRow
                ''Checking Companyid
                If gridView.CurrentRow.Cells("batchno1").Value.ToString.Trim = "" Or gridView.CurrentRow.Cells("TRANDATE").Value.ToString.Trim = "" Then MsgBox("Select batchno and Trandate filled row.", MsgBoxStyle.Information) : Exit Sub

                strSql = " SELECT COUNT(*) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & Format(gridView.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd") & "' AND BATCHNO = '" & gridView.CurrentRow.Cells("batchno1").Value.ToString & "' AND COMPANYID='" & strCompanyId & "'"
                If Val(objGPack.GetSqlValue(strSql, , , )) = 0 Then
                    MsgBox("Company mismatch for this Bill", MsgBoxStyle.Information)
                    Exit Sub
                End If
                Dim diffamt, debit, credit As Decimal
                Dim accode As String = ""
                Dim trandate As String = ""
                Dim tranno As String = ""
                Dim RO As DataRow()
                RO = dtGridView.Select("batchno1='" & gridView.CurrentRow.Cells("batchno1").Value.ToString & "' ")
                For i As Integer = 0 To RO.Length - 1
                    If RO(i).Item("DIFF").ToString <> "" Then
                        diffamt = Val(RO(i).Item("DIFF").ToString)
                    Else
                        debit = Val(RO(i).Item("DEBIT").ToString)
                        credit = Val(RO(i).Item("CREDIT").ToString)
                        accode = RO(i).Item("ACCODE").ToString
                        tranno = RO(i).Item("TRANNO").ToString
                        trandate = RO(i).Item("TRANDATE").ToString
                    End If
                Next
                If diffamt = 0 Then Exit Sub
                Dim tallyobj As New FRM_TALLYBILLDIFFERENCE("I", tranno, credit, debit, accode, .Cells("BATCHNO").Value.ToString,
                trandate, diffamt)
                tallyobj.ShowDialog()
            End With
        ElseIf e.KeyCode = Keys.U Then
            With gridView.CurrentRow
                If gridView.CurrentRow.Cells("batchno1").Value.ToString.Trim = "" Or gridView.CurrentRow.Cells("TRANDATE").Value.ToString.Trim = "" Then MsgBox("Select batchno and Trandate filled row.", MsgBoxStyle.Information) : Exit Sub
                strSql = " SELECT COUNT(*) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & Format(gridView.CurrentRow.Cells("TRANDATE").Value, "yyyy-MM-dd") & "' AND BATCHNO = '" & gridView.CurrentRow.Cells("batchno1").Value.ToString & "' AND COMPANYID='" & strCompanyId & "'"
                If Val(objGPack.GetSqlValue(strSql, , , )) = 0 Then
                    MsgBox("Company mismatch for this Bill", MsgBoxStyle.Information)
                    Exit Sub
                End If
                If Val(.Cells("DIFF").Value.ToString) <> 0 Then Exit Sub
                If .Cells("BATCHNO").Value.ToString = "" Then Exit Sub
                Dim tallyobj As New FRM_TALLYBILLDIFFERENCE("U", .Cells("TRANNO").Value.ToString, Val(.Cells("CREDIT").Value.ToString), Val(.Cells("DEBIT").Value.ToString) _
                , .Cells("ACCODE").Value.ToString, .Cells("BATCHNO").Value.ToString, .Cells("TRANDATE").Value.ToString _
                , Val(.Cells("DIFF").Value.ToString), .Cells("TRANTYPE").Value.ToString, .Cells("SNO").Value.ToString)
                tallyobj.ShowDialog()
                btnView_Click(Me, New EventArgs)
            End With
        End If
    End Sub
    '605

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblHelp.Visible = False
    End Sub
End Class