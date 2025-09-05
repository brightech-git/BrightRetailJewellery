Imports System.Data.OleDb

Public Class W_RunningBalance
    Dim objGridShower As frmGridDispDia
    Dim cmd As New OleDbCommand
    Dim da As New OleDbDataAdapter
    Dim strSql As String = Nothing
    Dim dtDetailView As New DataTable
    Dim dtView As New DataTable
    Dim dtHeader As New DataTable
    Dim tran As OleDbTransaction
    Dim schemaTable As DataTable

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Try
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            MsgBox("ERROR :" + ex.Message + vbCrLf + vbCrLf + "POSITION :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub frmCustomerLedger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbPartyName.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        Try
            Dim dtView As New DataTable
            Me.Refresh()
            strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "RUNBAL')>0 /**/"
            strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "RUNBAL /**/"
            strSql += vbCrLf + " SELECT TEMPTRANDATE,TRANDATE,TRANNO /**/"
            strSql += vbCrLf + " ,CASE WHEN SUM(PUREWT)>0 THEN SUM(PUREWT) ELSE 0 END IWT /**/"
            strSql += vbCrLf + " ,CASE WHEN SUM(PUREWT)<0 THEN ABS(SUM(PUREWT)) ELSE 0 END RWT /**/"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),0) BWT /**/"
            strSql += vbCrLf + " ,CASE WHEN SUM(AMOUNT)>0 THEN SUM(AMOUNT) ELSE 0 END IAMT /**/"
            strSql += vbCrLf + " ,CASE WHEN SUM(AMOUNT)<0 THEN ABS(SUM(AMOUNT)) ELSE 0 END RAMT /**/"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) BAMT /**/"
            strSql += vbCrLf + " ,BATCHNO  /**/"
            strSql += vbCrLf + " INTO TEMP" & systemId & "RUNBAL /**/"
            strSql += vbCrLf + " FROM /**/"
            strSql += vbCrLf + " ( /*X STARTS */ /**/"
            strSql += vbCrLf + " /***OPENINGTRAILBALANCE***/ /**/"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR,'OPENING') TRANDATE, NULL TEMPTRANDATE, 0 TRANNO/**/ "
            strSql += vbCrLf + " ,DEBITWT-CREDITWT PUREWT /**/"
            strSql += vbCrLf + " ,DEBIT-CREDIT AMOUNT /**/"
            strSql += vbCrLf + " ,NULL BATCHNO /**/ "
            strSql += vbCrLf + "  FROM " & cnStockDb & "..OPENTRAILBALANCE /**/"
            strSql += vbCrLf + " WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "') /**/"
            strSql += vbCrLf + " UNION ALL /**/"
            strSql += vbCrLf + " /***OPENWEIGHT***/ /**/"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR,'OPENING')  TRANDATE, NULL TEMPTRANDATE,0 TRANNO /**/ "
            strSql += vbCrLf + " ,CASE WHEN TRANTYPE = 'I' THEN PUREWT ELSE -PUREWT END PUREWT /**/ "
            strSql += vbCrLf + " ,0 AMOUNT  /**/ "
            strSql += vbCrLf + " ,NULL BATCHNO /**/ "
            strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT O /**/"
            strSql += vbCrLf + " WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "') /**/"
            strSql += vbCrLf + " UNION ALL /**/"
            strSql += vbCrLf + " /***OPENING ISSUE***/ /**/"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR,'OPENING') TRANDATE,NULL TEMPTRANDATE,0 TRANNO,PUREWT ,0 AMOUNT,NULL BATCHNO /**/"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I/**/"
            strSql += vbCrLf + " WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "') /**/"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') ='' /**/"
            strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'SE' /**/"
            strSql += vbCrLf + " AND TRANDATE <'" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' /**/"
            If rdbExceptApproval.Checked Then
                strSql += vbCrLf + "  AND TRANTYPE NOT IN ('AI','IAP') "
            ElseIf rdbApproval.Checked Then
                strSql += vbCrLf + "  AND TRANTYPE IN ('AI','IAP') "
            End If
            strSql += vbCrLf + " UNION ALL /**/"
            strSql += vbCrLf + " /***OPENING RECEIPT***/ /**/"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR,'OPENING') TRANDATE,NULL TEMPTRANDATE,0 TRANNO,-1*PUREWT PUREWT ,0 AMOUNT,NULL BATCHNO /**/"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R/**/"
            strSql += vbCrLf + " WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "') /**/"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') ='' /**/"
            strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'SE' /**/"
            strSql += vbCrLf + " AND TRANDATE <'" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' /**/"
            If rdbExceptApproval.Checked Then
                strSql += vbCrLf + "  AND TRANTYPE NOT IN ('AR','RAP') "
            ElseIf rdbApproval.Checked Then
                strSql += vbCrLf + "  AND TRANTYPE IN ('AR','RAP') "
            End If
            ''CORRECTION MADE ON 02-12-09
            strSql += vbCrLf + " UNION ALL /**/"
            strSql += vbCrLf + " /***ACCTRAN***/ /**/"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR,'OPENING') TRANDATE,NULL TEMPTRANDATE,0 TRANNO /**/ "
            strSql += vbCrLf + " ,0 PUREWT /**/ "
            strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -AMOUNT END AMOUNT  /**/ "
            strSql += vbCrLf + " ,NULL BATCHNO /**/ "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A/**/"
            strSql += vbCrLf + " WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "') /**/"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') ='' /**/"
            strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'OP' /**/"
            strSql += vbCrLf + " AND TRANDATE <'" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' /**/"
            ''''''

            strSql += vbCrLf + " UNION ALL /**/"
            strSql += vbCrLf + " /***ISSUE***/ /**/"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR,TRANDATE,103) TRANDATE,TRANDATE TEMPTRANDATE,TRANNO,PUREWT ,0 AMOUNT,BATCHNO /**/"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I/**/"
            strSql += vbCrLf + " WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "') /**/"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') ='' /**/"
            strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'SE' /**/"
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' /**/"
            If rdbExceptApproval.Checked Then
                strSql += vbCrLf + "  AND TRANTYPE NOT IN ('AI','IAP') "
            ElseIf rdbApproval.Checked Then
                strSql += vbCrLf + "  AND TRANTYPE IN ('AI','IAP') "
            End If
            strSql += vbCrLf + " UNION ALL /**/"
            strSql += vbCrLf + " /***RECEIPT***/ /**/"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR,TRANDATE,103) TRANDATE,TRANDATE TEMPTRANDATE,TRANNO,-1*PUREWT PUREWT ,0 AMOUNT,BATCHNO /**/"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R/**/"
            strSql += vbCrLf + " WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "') /**/"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') ='' /**/"
            strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'SE' /**/"
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' /**/"
            If rdbExceptApproval.Checked Then
                strSql += vbCrLf + "  AND TRANTYPE NOT IN ('AR','RAP')"
            ElseIf rdbApproval.Checked Then
                strSql += vbCrLf + "  AND TRANTYPE IN ('AR','RAP')"
            End If
            strSql += vbCrLf + " UNION ALL /**/"
            strSql += vbCrLf + " /***ACCTRAN***/ /**/"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR,TRANDATE,103) TRANDATE,TRANDATE TEMPTRANDATE,TRANNO /**/ "
            strSql += vbCrLf + " ,0 PUREWT /**/ "
            strSql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -AMOUNT END AMOUNT  /**/ "
            strSql += vbCrLf + " ,BATCHNO /**/ "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A/**/"
            strSql += vbCrLf + " WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "') /**/"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') ='' /**/"
            strSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'OP' /**/"
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' /**/"
            strSql += vbCrLf + " )X /**/"
            strSql += vbCrLf + " GROUP BY TEMPTRANDATE,TRANDATE,TRANNO,BATCHNO /**/"
            strSql += vbCrLf + " ORDER BY TEMPTRANDATE,TRANNO,BATCHNO /**/"
            strSql += vbCrLf + " /***RUNNING BALANCE CURSOR***/ /**/"
            strSql += vbCrLf + " DECLARE @IWT NUMERIC(15,3) /**/"
            strSql += vbCrLf + " DECLARE @RWT NUMERIC(15,3) /**/"
            strSql += vbCrLf + " DECLARE @BWT NUMERIC(15,3) /**/"
            strSql += vbCrLf + " DECLARE @IAMT NUMERIC(15,2) /**/"
            strSql += vbCrLf + " DECLARE @RAMT NUMERIC(15,2) /**/"
            strSql += vbCrLf + " DECLARE @BAMT NUMERIC(15,2) /**/"
            strSql += vbCrLf + " SELECT @BWT = 0 /**/"
            strSql += vbCrLf + " SELECT @BAMT = 0 /**/"
            strSql += vbCrLf + "  DECLARE RBCUR CURSOR FOR SELECT IWT,RWT,IAMT,RAMT FROM TEMP" & systemId & "RUNBAL /**/"
            strSql += vbCrLf + " OPEN RBCUR WHILE 1=1 /**/"
            strSql += vbCrLf + " BEGIN /**/"
            strSql += vbCrLf + "  FETCH NEXT FROM RBCUR INTO @IWT,@RWT,@IAMT,@RAMT /**/ "
            strSql += vbCrLf + " IF @@FETCH_STATUS = -1 BREAK /**/"
            strSql += vbCrLf + " 	BEGIN /**/"
            strSql += vbCrLf + " 		SELECT @BWT = @BWT+@IWT-@RWT /**/"
            strSql += vbCrLf + " 		SELECT @BAMT = @BAMT+@IAMT-@RAMT /**/		"
            strSql += vbCrLf + " 		UPDATE TEMP" & systemId & "RUNBAL SET BWT = @BWT,BAMT = @BAMT /**/"
            strSql += vbCrLf + " 		WHERE CURRENT OF RBCUR /**/"
            strSql += vbCrLf + " 	END 	 /**/"
            strSql += vbCrLf + " END /**/"
            strSql += vbCrLf + " CLOSE RBCUR /**/"
            strSql += vbCrLf + " DEALLOCATE RBCUR /**/ "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " UPDATE TEMP" & systemId & "RUNBAL SET IWT = NULL WHERE IWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "RUNBAL SET RWT = NULL WHERE RWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "RUNBAL SET BWT = NULL WHERE BWT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "RUNBAL SET IAMT = NULL WHERE IAMT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "RUNBAL SET RAMT = NULL WHERE RAMT = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "RUNBAL SET BAMT = NULL WHERE BAMT = 0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "  SELECT "
            strSql += vbCrLf + "  TRANDATE,TRANNO,RWT,IWT,BWT"
            strSql += vbCrLf + "  ,RAMT,IAMT,BAMT"
            strSql += vbCrLf + "  ,BATCHNO"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "RUNBAL"
            strSql += vbCrLf + "  ORDER BY TEMPTRANDATE,TRANNO"

            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Sub
            End If
            objGridShower = New frmGridDispDia
            objGridShower.Name = Me.Name
            objGridShower.gridView.RowTemplate.Height = 21
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            objGridShower.Text = "RUNNING BALANCE"
            Dim tit As String = cmbPartyName.Text & " RUNNING BALANCE" + vbCrLf
            tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
            objGridShower.lblTitle.Text = tit
            AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
            objGridShower.StartPosition = FormStartPosition.CenterScreen
            objGridShower.dsGrid.DataSetName = objGridShower.Name
            objGridShower.dsGrid.Tables.Add(dtGrid)
            objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
            objGridShower.FormReSize = False
            objGridShower.FormReLocation = False
            objGridShower.pnlFooter.Visible = False
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            DataGridView_SummaryFormatting(objGridShower.gridView)
            FormatGridColumns(objGridShower.gridView, False, False, , False)

            objGridShower.Show()
            objGridShower.FormReSize = True
            objGridShower.FormReLocation = True
            objGridShower.gridViewHeader.Visible = True
            GridViewHeaderCreator(objGridShower.gridViewHeader)
            FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
            objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns(0)))

            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dtView)
            'If Not dtView.Rows.Count > 0 Then
            '    MsgBox("Records Not Found..", MsgBoxStyle.Information, "Message")
            '    cmbPartyName.Focus()
            '    cmbPartyName.SelectAll()
            'Else
            '    gridView.DataSource = dtView
            '    funcgridcol()
            '    funcGridViewcolWdth()
            '    funcGridHeaderColWdth()
            '    tabMain.SelectedTab = tabView
            '    gridView.Rows(0).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            '    gridView.Rows(0).Cells(0).Style.BackColor = Color.LightBlue

            '    gridView.DefaultCellStyle.SelectionBackColor = gridView.DefaultCellStyle.BackColor
            '    lblTitle.Text = UCase("LEDGER DETAILS WITH RUNNING BALANCE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & " FOR " & cmbPartyName.Text & "")
            '    gridView.Focus()
            'End If
        Catch ex As Exception
            MsgBox("ERROR :" + ex.Message + vbCrLf + vbCrLf + "POSITION :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWid(CType(sender, DataGridView))
    End Sub

    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("TRANDATE~TRANNO", GetType(String))
            .Columns.Add("RWT~IWT~BWT", GetType(String))
            .Columns.Add("RAMT~IAMT~BAMT", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("TRANDATE~TRANNO").Caption = "PARTICULAR"
            .Columns("RWT~IWT~BWT").Caption = "WEIGHT"
            .Columns("RAMT~IAMT~BAMT").Caption = "AMOUNT"
        End With
        gridviewHead.DataSource = dtMergeHeader
        With gridviewHead
            .Columns("TRANDATE~TRANNO").HeaderText = ""
            .Columns("RWT~IWT~BWT").HeaderText = "WEIGHT"
            .Columns("RAMT~IAMT~BAMT").HeaderText = "AMOUNT"
            .Columns("SCROLL").HeaderText = ""
        End With
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWid(gridviewHead)
    End Sub

    Private Sub SetGridHeadColWid(ByVal gridViewHeader As DataGridView)
        Dim f As frmGridDispDia
        f = objGPack.GetParentControl(gridViewHeader)
        If Not f.gridViewHeader.Visible Then Exit Sub
        If f.gridViewHeader Is Nothing Then Exit Sub
        If Not f.gridView.ColumnCount > 0 Then Exit Sub
        If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With f.gridViewHeader
            .Columns("TRANDATE~TRANNO").Width = f.gridView.Columns("TRANDATE").Width + f.gridView.Columns("TRANNO").Width
            .Columns("RWT~IWT~BWT").Width = f.gridView.Columns("IWT").Width + f.gridView.Columns("RWT").Width + f.gridView.Columns("BWT").Width
            .Columns("RAMT~IAMT~BAMT").Width = f.gridView.Columns("IAMT").Width + f.gridView.Columns("RAMT").Width + f.gridView.Columns("BAMT").Width
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To f.gridView.ColumnCount - 1
                If f.gridView.Columns(cnt).Visible Then colWid += f.gridView.Columns(cnt).Width
            Next
            If colWid >= f.gridView.Width Then
                f.gridViewHeader.Columns("SCROLL").Visible = CType(f.gridView.Controls(1), VScrollBar).Visible
                f.gridViewHeader.Columns("SCROLL").Width = CType(f.gridView.Controls(1), VScrollBar).Width
                f.gridViewHeader.Columns("SCROLL").HeaderText = ""
            Else
                f.gridViewHeader.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            With .Columns("TRANDATE")
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TRANNO")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = "TRAN NO"
            End With
            With .Columns("IWT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .HeaderText = "ISSUE"
            End With
            With .Columns("IAMT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .HeaderText = "ISSUE"
            End With
            With .Columns("RWT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .HeaderText = "RECEIPT"
            End With
            With .Columns("RAMT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .HeaderText = "RECEIPT"
            End With
            With .Columns("BWT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .HeaderText = "BALANCE"
            End With
            With .Columns("BAMT")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .HeaderText = "BALANCE"
            End With
            .Columns("BATCHNO").Visible = False
            .Columns("KEYNO").Visible = False
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            strSql = " SELECT DISTINCT ACNAME FROM " & cnAdminDb & "..ACHEAD "
            strSql += vbCrLf + " WHERE ACGRPCODE NOT IN ('3','4')"
            strSql += vbCrLf + " ORDER BY ACNAME"
            objGPack.FillCombo(strSql, cmbPartyName)

            dtpFrom.Value = Today.Date
            dtpTo.Value = Today.Date
            cmbPartyName.Text = ""
            rdbAll.Checked = True
            cmbPartyName.Focus()
            cmbPartyName.Select()
            Me.Refresh()
        Catch ex As Exception
            MsgBox("ERROR :" + ex.Message + vbCrLf + vbCrLf + "POSITION :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub cmbPartyName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPartyName.GotFocus
        'cmbPartyName.Height = 150
    End Sub

    Private Sub cmbPartyName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbPartyName.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbPartyName.Text = "" Then
                MsgBox("Ac Name Should not empty", MsgBoxStyle.Information)
                cmbPartyName.Select()
                Exit Sub
            End If
            If Not cmbPartyName.Items.Contains(cmbPartyName.Text) Then
                MsgBox("Invalid Ac Name", MsgBoxStyle.Information)
                cmbPartyName.Select()
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
        'If e.KeyCode = Keys.F4 Then
        '    'cmbPartyName.Height = 150
        'ElseIf e.KeyCode = Keys.Enter Then
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub cmbPartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPartyName.KeyPress
        e.KeyChar = UCase(e.KeyChar)
        If e.KeyChar = Chr(Keys.Enter) Then
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            'cmbPartyName.Height = 20
        End If
    End Sub

    Private Sub cmbPartyName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPartyName.LostFocus
        'cmbPartyName.Height = 20
    End Sub

    Private Sub W_RunningBalance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grpControls.Location = New Point((ScreenWid - grpControls.Width) / 2, ((ScreenHit - 128) - grpControls.Height) / 2)
    End Sub

End Class