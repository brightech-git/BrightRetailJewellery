Imports System.Data.OleDb
Public Class frmCardCollectionsReport
    Dim objGridShower As frmGridDispDia
    Dim dtCardCollection As New DataTable
    Dim StrItemFilter As String = Nothing
    Dim StrHeader As String = Nothing
    Dim strChit As String = Nothing
    Dim strcredit As String = Nothing
    Dim strcheque As String = Nothing
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dt As New DataTable
    Dim CCENTRE As String = Nothing
    Dim NOID As String = Nothing
    Dim SelectedCompany As String

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus

        dtpFrom.Value = GetServerDate(tran)
        dtpTo.Value = GetServerDate(tran)
        chkCheque.Checked = True
        chkChitCard.Checked = True
        chkCreditCard.Checked = True
    End Sub

    Private Sub frmCardCollectionsReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        LoadCompany(chkLstCompany)
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        btnNew_Click(Me, New EventArgs)
        dtpFrom.Select()
    End Sub

    Public Function funcAddCostName() As Integer
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' "
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = "SELECT DISTINCT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                chkLstCostCentre.Enabled = True
                chkLstCostCentre.Items.Clear()
                chkLstCostCentre.Items.Add("ALL", IIf(cnDefaultCostId, True, False))
                'For cnt As Integer = 0 To dt.Rows.Count - 1
                '    chkLstCostCentre.Items.Add(dt.Rows(cnt).Item(0).ToString)
                'Next
                For i As Integer = 0 To dt.Rows.Count - 1
                    If cnCostName = dt.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                        chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, True)
                    Else
                        chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, False)
                    End If
                Next
                If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
            End If
        Else
            chkLstCostCentre.Items.Clear()
            chkLstCostCentre.Enabled = False
        End If
    End Function

    Private Sub funcAddNodeId()
        strSql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN  WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        strSql += " UNION"
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ISSUE  WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        strSql += " UNION"
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..RECEIPT  WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            chkLstNodeId.Items.Clear()
            chkLstNodeId.Items.Add("ALL", True)
            For CNT As Integer = 0 To dt.Rows.Count - 1
                chkLstNodeId.Items.Add(dt.Rows(CNT).Item(0).ToString)
            Next
        Else
            chkLstNodeId.Items.Clear()
            chkLstNodeId.Enabled = False
        End If
    End Sub

    Function funcCostName() As String
        ''COSTCENTRE
        CCENTRE = ""
        If chkLstCostCentre.Enabled Then
            If chkLstCostCentre.CheckedItems.Count > 0 And chkLstCostCentre.GetItemChecked(0) <> True Then
                For CNT As Integer = 0 To chkLstCostCentre.CheckedItems.Count - 1
                    CCENTRE += chkLstCostCentre.CheckedItems.Item(CNT).ToString
                    If Not (CNT = chkLstCostCentre.CheckedItems.Count - 1) Then CCENTRE += ","
                Next
            End If
        End If
        Return CCENTRE
    End Function

    Function funcSystemId() As String
        ''NODE ID
        NOID = ""
        If chkLstNodeId.CheckedItems.Count > 0 Then
            If chkLstNodeId.GetItemChecked(0) <> True Then
                For CNT As Integer = 0 To chkLstNodeId.CheckedItems.Count - 1
                    NOID += chkLstNodeId.CheckedItems.Item(CNT).ToString
                    If Not (CNT = chkLstNodeId.CheckedItems.Count - 1) Then NOID += ","
                Next
            End If
        End If
        Return NOID
    End Function

    Private Sub frmCardCollectionsReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) And tabMain.SelectedTab.Name = tabView.Name Then
            btnExcel_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.P) And tabMain.SelectedTab.Name = tabView.Name Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub 'Try
        If chkLstCostCentre.Enabled = True Then
            If Not chkLstCostCentre.CheckedItems.Count > 0 Then
                chkLstCostCentre.SetItemChecked(0, True)
            End If
        End If
        If Not chkLstNodeId.CheckedItems.Count > 0 Then
            If chkLstNodeId.Items.Count = 0 Then
                funcAddNodeId()
            End If
            If chkLstCostCentre.CheckedItems.Count > 0 Then
                chkLstNodeId.SetItemChecked(0, True)
            End If
        End If
        SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
        Dim CostFtr As String = funcCostName()
        Dim NodeFtr As String = funcSystemId()
        Dim Type As String = ""
        If chkChitCard.Checked Then Type += "SS,CG,CB,CZ,CD,HP,HR,"
        If chkCreditCard.Checked Then Type += "CC,"
        If chkCheque.Checked Then Type += "CH,"
        If Type <> "" Then Type = Mid(Type, 1, Type.Length - 1)
        strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPCARDCOLLECT','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPCARDCOLLECT"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = " EXEC " & cnStockDb & "..SP_RPT_CARDCOLLECTION"
        strSql += vbCrLf + " @FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TYPE = '" & Type & "'"
        strSql += vbCrLf + " ,@NODEID = '" & NodeFtr & "'"
        strSql += vbCrLf + " ,@SYSTEMID = '" & systemId & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & CostFtr & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompany & "'"
        strSql += vbCrLf + " ,@WITHCHIT ='" & IIf(chkWithChit.Checked, "Y", "N") & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "CARDCOLLECTION  ORDER BY PAYMODE,CARDNAME,RESULT,PARTICULAR,TRANDATE,TRANNO"
        strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "CARDCOLLECTION "
        If rbtTranno.Checked Then
            strSql += vbCrLf + " ORDER BY PAYMODE,CARDNAME,RESULT,TRANDATE,TRANNO,PARTICULAR"
        Else
            strSql += vbCrLf + " ORDER BY PAYMODE,CARDNAME,RESULT,PARTICULAR,TRANDATE,TRANNO"
        End If

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "CARD COLLECTION REPORT"
        Dim tit As String = "CARD COLLECTION REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        Dim Cname As String = ""
        If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                If chkLstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chkLstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        End If
        objGridShower.lblTitle.Text = tit + Cname
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        objGridShower.Show()
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        Prop_Sets()
        '    funcFilter()
        '    dtCardCollection.Clear()
        '    lblTitle.Text = "TITLE"
        '    Me.Refresh()
        '    'Final Query
        '    Dim Chit As String = Nothing
        '    If chkChitCard.Checked = True Then
        '        Chit = "Y"
        '    Else
        '        Chit = "N"
        '    End If
        '    Dim Credit As String = Nothing
        '    If chkCreditCard.Checked = True Then
        '        Credit = "Y"
        '    Else
        '        Credit = "N"
        '    End If
        '    Dim Cheque As String = Nothing
        '    If chkCheque.Checked = True Then
        '        Cheque = "Y"
        '    Else
        '        Cheque = "N"
        '    End If
        '    ''cmd.CommandType = CommandType.StoredProcedure
        '    strSql = "EXECUTE " & cnStockDb & "..SP_RPT_CARDCOLLECTION"
        '    strsql += vbcrlf + " @DATEFROM ='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
        '    strsql += vbcrlf + " ,@DATETO ='" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        '    strsql += vbcrlf + " ,@NodeId ='" & Replace(NOID, "'", "''") & "'"
        '    strsql += vbcrlf + " ,@CostCentre ='" & Replace(CCENTRE, "'", "''") & "'"
        '    strsql += vbcrlf + " ,@CHIT='" & Chit & "'"
        '    strsql += vbcrlf + " ,@CREDIT='" & Credit & "'"
        '    strsql += vbcrlf + " ,@CHEQUE='" & Cheque & "'"
        '    strsql += vbcrlf + " ,@SystemId ='" & systemId & "'"
        '    strsql += vbcrlf + " ,@CNCOMPANYID ='" & strCompanyId & "'"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.ExecuteNonQuery()

        '    strSql = "select * from TEMP" & systemId & "TOTALCARD ORDER BY RESULT,DUMMYDATE,DUMMYNO"
        '    dtCardCollection = New DataTable
        '    da = New OleDbDataAdapter(strSql, cn)
        '    da.Fill(dtCardCollection)
        '    If dtCardCollection.Rows.Count < 1 Then
        '        funcNew()
        '        MessageBox.Show("Records not found..", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '        btnView_Search.Focus()
        '        btnView_Search.Enabled = True
        '        Exit Sub
        '    End If

        '    strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "CARDCOL')"
        '    strsql += vbcrlf + " DROP TABLE TEMP" & systemId & "CARDCOL"
        '    strsql += vbcrlf + " CREATE TABLE TEMP" & systemId & "CARDCOL("
        '    strsql += vbcrlf + " BATCHNO VARCHAR(20),"
        '    strsql += vbcrlf + " TRANNO VARCHAR(30),"
        '    strsql += vbcrlf + " TRANDATE VARCHAR(12),"
        '    strsql += vbcrlf + " CHQCARDREF VARCHAR(50),"
        '    strsql += vbcrlf + " AMOUNT NUMERIC(15,2),"
        '    strsql += vbcrlf + " SYSTEMID VARCHAR(3),"
        '    strsql += vbcrlf + " CHQCARDNO VARCHAR(20),"
        '    strsql += vbcrlf + " RESULT INT,"
        '    strsql += vbcrlf + " DUMMYDATE SMALLDATETIME,"
        '    strsql += vbcrlf + " DUMMYNO INT,"
        '    strsql += vbcrlf + " COLHEAD VARCHAR(1),"
        '    strsql += vbcrlf + " SNO INT IDENTITY)"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.ExecuteNonQuery()

        '    strSql = "ALTER TABLE TEMP" & systemId & "TOTALCARD ADD COLHEAD VARCHAR(1)"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.ExecuteNonQuery()

        '    strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "TOTALCARD)>0"
        '    strsql += vbcrlf + " BEGIN "
        '    strsql += vbcrlf + " UPDATE TEMP" & systemId & "TOTALCARD SET COLHEAD = 'T' WHERE RESULT IN (1,3,5)"
        '    strsql += vbcrlf + " END "
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.ExecuteNonQuery()

        '    strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "TOTALCARD)>0"
        '    strsql += vbcrlf + " BEGIN "
        '    strsql += vbcrlf + " INSERT INTO TEMP" & systemId & "CARDCOL(BATCHNO, TRANNO, TRANDATE, CHQCARDREF,"
        '    strsql += vbcrlf + " AMOUNT, SYSTEMID, CHQCARDNO, RESULT, DUMMYDATE, DUMMYNO, COLHEAD)"
        '    strsql += vbcrlf + " SELECT BATCHNO, TRANNO, TRANDATE, CHQCARDREF,"
        '    strsql += vbcrlf + " CONVERT(FLOAT,AMOUNT)AMOUNT, SYSTEMID, CHQCARDNO, CONVERT(INT,RESULT)RESULT, DUMMYDATE, DUMMYNO, COLHEAD"
        '    strsql += vbcrlf + " FROM TEMP" & systemId & "TOTALCARD ORDER BY RESULT,DUMMYDATE,DUMMYNO"
        '    strsql += vbcrlf + " END "
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.ExecuteNonQuery()

        '    strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "CARDCOL)>0"
        '    strsql += vbcrlf + " BEGIN "
        '    strsql += vbcrlf + " UPDATE TEMP" & systemId & "CARDCOL SET AMOUNT = NULL WHERE AMOUNT = 0"
        '    strsql += vbcrlf + " END "
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.ExecuteNonQuery()

        '    strSql = "select * from TEMP" & systemId & "CARDCOL ORDER BY SNO"
        '    dtCardCollection = New DataTable
        '    da = New OleDbDataAdapter(strSql, cn)
        '    da.Fill(dtCardCollection)

        '    'Add Title of the Report into Label lblTitle
        '    Dim strTitle As String = Nothing
        '    strTitle = " CARDCOLLECTIONS REPORT"
        '    strTitle += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        '    If Strings.Right(strTitle, 3) = "AND" Then
        '        strTitle = Strings.Left(strTitle, strTitle.Length - 3)
        '    End If
        '    strTitle += "("
        '    If chkChitCard.Checked = True Then
        '        strTitle += "CHITCARDWISE AND "
        '    End If
        '    If chkCreditCard.Checked = True Then
        '        strTitle += "CREDITCARDWISE AND "
        '    End If
        '    If chkCheque.Checked = True Then
        '        strTitle += "CHEQUEWISE"
        '    End If
        '    If Strings.Right(strTitle, 4) = "AND " Then
        '        strTitle = Strings.Left(strTitle, strTitle.Length - 4)
        '    End If
        '    strTitle += ")"
        '    lblTitle.Text = strTitle
        '    btnView_Search.Enabled = True
        '    gridView.DataSource = dtCardCollection
        '    tabView.Show()
        '    GridViewFormat()
        '    gridView.Columns("COLHEAD").Visible = False
        '    gridView.Columns("SNO").Visible = False
        '    'funcCellColor()
        '    If dtCardCollection.Rows.Count > 0 Then Me.tabMain.SelectedTab = tabView
        '    gridView.Focus()
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        '    MsgBox(ex.StackTrace)
        'Finally
        '    Me.Cursor = Cursors.Arrow
        '    btnView_Search.Enabled = True
        'End Try
    End Sub


    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            FormatGridColumns(dgv, False, False, , False)
            .Columns("PARTICULAR").Width = 250
            .Columns("TRANDATE").Width = 80
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("TRANNO").Width = 60
            .Columns("AMOUNT").Width = 100
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("CHQCARDNO").Width = 150
            .Columns("SYSTEMID").Width = 70
            For cnt As Integer = 6 To dgv.Columns.Count - 1
                .Columns(cnt).Visible = False
            Next
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Prop_Gets()
        funcNew()
        funcAddCostName()
        'funcAddNodeId()
        dtpFrom.Select()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Function funcNew() As Integer
        Try
            dtCardCollection.Clear()
            lblTitle.Text = "TITLE"
            strSql = "select ''BATCHNO,''TRANNO,''TRANDATE,''CHQCARDREF,' 'AMOUNT,'' SYSTEMID,''CHQCARDNO,1 RESULT,''DUMMYDATE,''DUMMYNO where 1<>1"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCardCollection)
            gridView.DataSource = dtCardCollection
            funcGridStyle()
            lblTitle.Height = gridView.ColumnHeadersHeight
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Function funcGridStyle() As Integer
        With gridView
            .Columns("RESULT").Visible = False
            .Columns("BATCHNO").Visible = False
            .Columns("DUMMYDATE").Visible = False
            .Columns("DUMMYNO").Visible = False
            With .Columns("TRANNO")
                .Width = 100
                .HeaderText = "BILLNO"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TRANDATE")
                .Width = 100
                .HeaderText = "BILLDATE"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CHQCARDREF")
                .Width = 150
                .HeaderText = "DESCRIPTION"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("AMOUNT")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SYSTEMID")
                .Width = 100
                .HeaderText = "NODEID"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CHQCARDNO")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function

    Function funcCellColor() As Integer
        For rowCount As Integer = 0 To dtCardCollection.Rows.Count - 1
            If gridView.Rows(rowCount).Cells(1).Value = "CHITCARD" Or gridView.Rows(rowCount).Cells(1).Value = "CREDIT" Or gridView.Rows(rowCount).Cells(1).Value = "CHEQUE" Then
                gridView.Rows(rowCount).Cells(1).Style.BackColor = Color.LightBlue
            End If
        Next
    End Function

    Function funcHeaderCall() As String
        'Query for add the Header to tables
        Dim strHeaderCall As String = Nothing
        strHeaderCall += " select BATCHNO,CONVERT(VARCHAR,TranNo)TRANNO,CONVERT(VARCHAR,TranDate,103)TRANDATE"
        strHeaderCall += ",CHQCARDREF,CONVERT(VARCHAR,Amount)AMOUNT,SYSTEMID"
        strHeaderCall += ",CHQCARDNO,' ' RESULT"
        Return strHeaderCall
    End Function

    Function funcExecute() As Integer
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "T"
    ''                .Rows(e.RowIndex).Cells("TRANNO").Style.BackColor = reportHeadStyle.BackColor
    ''                .Rows(e.RowIndex).Cells("TRANNO").Style.Font = reportHeadStyle.Font
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("TRANNO").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("TRANNO").Style.Font = reportHeadStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnNew.Focus()
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmCardCollectionsReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmCardCollectionsReport_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, "ALL")
        SetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId, "ALL")
        chkChitCard.Checked = obj.p_chkChitCard
        chkCreditCard.Checked = obj.p_chkCreditCard
        chkCheque.Checked = obj.p_chkCheque
        chkWithChit.Checked = obj.p_chkWithChit
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmCardCollectionsReport_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        GetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId)
        obj.p_chkChitCard = chkChitCard.Checked
        obj.p_chkCreditCard = chkCreditCard.Checked
        obj.p_chkCheque = chkCheque.Checked
        obj.p_chkWithChit = chkWithChit.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmCardCollectionsReport_Properties))
    End Sub

    Private Sub dtpTo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpTo.LostFocus
        funcAddNodeId()
    End Sub

    Private Sub chkLstNodeId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstNodeId.GotFocus
        If chkLstNodeId.Items.Count = 0 Then
            funcAddNodeId()
        End If
    End Sub
End Class


Public Class frmCardCollectionsReport_Properties
    Private chkWithChit As Boolean = False
    Public Property p_chkWithChit() As Boolean
        Get
            Return chkWithChit
        End Get
        Set(ByVal value As Boolean)
            chkWithChit = value
        End Set
    End Property
    Private chkCompanySelectAll As Boolean = False
    Public Property p_chkCompanySelectAll() As Boolean
        Get
            Return chkCompanySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCompanySelectAll = value
        End Set
    End Property
    Private chkLstCompany As New List(Of String)
    Public Property p_chkLstCompany() As List(Of String)
        Get
            Return chkLstCompany
        End Get
        Set(ByVal value As List(Of String))
            chkLstCompany = value
        End Set
    End Property
    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
    Private chkLstNodeId As New List(Of String)
    Public Property p_chkLstNodeId() As List(Of String)
        Get
            Return chkLstNodeId
        End Get
        Set(ByVal value As List(Of String))
            chkLstNodeId = value
        End Set
    End Property
    Private chkChitCard As Boolean = True
    Public Property p_chkChitCard() As Boolean
        Get
            Return chkChitCard
        End Get
        Set(ByVal value As Boolean)
            chkChitCard = value
        End Set
    End Property
    Private chkCreditCard As Boolean = True
    Public Property p_chkCreditCard() As Boolean
        Get
            Return chkCreditCard
        End Get
        Set(ByVal value As Boolean)
            chkCreditCard = value
        End Set
    End Property

    Private chkCheque As Boolean = True
    Public Property p_chkCheque() As Boolean
        Get
            Return chkCheque
        End Get
        Set(ByVal value As Boolean)
            chkCheque = value
        End Set
    End Property
End Class