Imports System.Data.OleDb
Public Class DenominationRpt
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Private Sub DenominationRpt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub DenominationRpt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpDate.Value = GetServerDate()
        'rbtBoth.Checked = True
        Prop_Gets()
        dtpDate.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'strSql = vbCrLf + " IF OBJECT_ID('MASTER..DENOMRPT') IS NOT NULL DROP TABLE MASTER..DENOMRPT"
        'strSql += vbCrLf + " DECLARE @TRANDATE SMALLDATETIME"
        'strSql += vbCrLf + " SET @TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + " SELECT CONVERT(VARCHAR(100),M.DEN_VALUE)DEN_VALUE"
        'strSql += vbCrLf + " ," & IIf(rbtPayment.Checked, "-1*", "") & "SUM(T.DEN_QTY)DEN_QTY"
        'strSql += vbCrLf + " ," & IIf(rbtPayment.Checked, "-1*", "") & "SUM(T.DEN_AMOUNT)DEN_AMOUNT"
        'strSql += vbCrLf + " ,M.DEN_ORDER,CONVERT(VARCHAR(3),NULL)COLHEAD"
        'strSql += vbCrLf + " INTO MASTER..DENOMRPT"
        'strSql += vbCrLf + " FROM " & cnAdminDb & "..DENOMMAST AS M"
        'strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..DENOMTRAN AS T ON M.DEN_ID = T.DEN_ID"
        'strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = @TRANDATE AND BATCHNO = T.BATCHNO AND COMPANYID = '" & strCompanyId & "' AND ISNULL(CANCEL,'') = '')"
        'If rbtReceipt.Checked Then strSql += vbCrLf + " AND T.DEN_QTY > 0"
        'If rbtPayment.Checked Then strSql += vbCrLf + " AND T.DEN_QTY < 0"
        'strSql += vbCrLf + " GROUP BY M.DEN_VALUE,M.DEN_ORDER"
        'strSql += vbCrLf + " "
        'strSql += vbCrLf + " IF (SELECT COUNT(*) FROM MASTER..DENOMRPT)>0"
        'strSql += vbCrLf + " BEGIN"
        'strSql += vbCrLf + " INSERT INTO MASTER..DENOMRPT(DEN_VALUE,DEN_QTY,DEN_AMOUNT,DEN_ORDER,COLHEAD)"
        'strSql += vbCrLf + " SELECT 'GRAND TOTAL',SUM(DEN_QTY),SUM(DEN_AMOUNT),MAX(DEN_ORDER)+1,'G' FROM MASTER..DENOMRPT"
        'strSql += vbCrLf + " END"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'strSql = vbCrLf + " SELECT * FROM MASTER..DENOMRPT ORDER BY DEN_ORDER"

        strSql = vbCrLf + " IF OBJECT_ID('MASTER..TEMP_DENCASH') IS NOT NULL DROP TABLE MASTER..TEMP_DENCASH"
        strSql += vbCrLf + " SELECT DISTINCT D.DEN_ID,D.DEN_VALUE,D.DEN_ORDER"
        If chkGroupbyCashCounter.Checked Then
            strSql += vbCrLf + " ,C.CASHID,C.CASHNAME"
        Else
            strSql += vbCrLf + " ,CONVERT(VARCHAR(10),NULL)CASHID,CONVERT(VARCHAR(100),NULL)CASHNAME"
        End If
        strSql += vbCrLf + " INTO MASTER..TEMP_DENCASH"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..DENOMMAST AS D," & cnAdminDb & "..CASHCOUNTER AS C"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " IF OBJECT_ID('MASTER..DENOMRPT') IS NOT NULL DROP TABLE MASTER..DENOMRPT"
        strSql += vbCrLf + " DECLARE @TRANDATE SMALLDATETIME"
        strSql += vbCrLf + " SET @TRANDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(100),M.DEN_VALUE)DEN_VALUE"
        strSql += vbCrLf + " ,SUM(T.DEN_QTY)DEN_QTY"
        strSql += vbCrLf + " ,SUM(T.DEN_AMOUNT)DEN_AMOUNT"
        strSql += vbCrLf + " ,M.DEN_ORDER,CONVERT(VARCHAR(3),NULL)COLHEAD"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(30),M.CASHNAME)AS CASHNAME"
        strSql += vbCrLf + " ,CONVERT(INT,1)RESULT"
        strSql += vbCrLf + " INTO MASTER..DENOMRPT"
        strSql += vbCrLf + " FROM MASTER..TEMP_DENCASH AS M"
        strSql += vbCrLf + " LEFT JOIN "
        strSql += vbCrLf + " 	("
        strSql += vbCrLf + " 	SELECT DISTINCT " & IIf(chkGroupbyCashCounter.Checked, "ACC.CASHID", "CONVERT(VARCHAR(10),NULL)CASHID") & ",T.BATCHNO,T.TRANDATE,T.DEN_ID,T.DEN_QTY,T.DEN_AMOUNT FROM " & cnStockDb & "..DENOMTRAN AS T"
        strSql += vbCrLf + " 		INNER JOIN " & cnStockDb & "..ACCTRAN AS ACC ON "
        strSql += vbCrLf + " 	ACC.TRANDATE = @TRANDATE"
        strSql += vbCrLf + " 	AND ACC.BATCHNO = T.BATCHNO "
        strSql += vbCrLf + " 	AND T.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE = @TRANDATE AND CTRANCODE IN "
        strSql += vbCrLf + " 	(SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID LIKE 'BANKPAYPROID')) "
        strSql += vbCrLf + " 	AND ACC.COMPANYID = '" & strCompanyId & "' "
        If txtSystemId.Text <> "" Then strSql += vbCrLf + "    AND ACC.SYSTEMID = '" & txtSystemId.Text & "'"
        strSql += vbCrLf + " 	AND ISNULL(ACC.CANCEL,'') = ''"
        strSql += vbCrLf + " 	) AS T  ON M.DEN_ID = T.DEN_ID " & IIf(chkGroupbyCashCounter.Checked, " AND M.CASHID = T.CASHID", "") & ""
        If rbtReceipt.Checked Then strSql += vbCrLf + " AND T.DEN_QTY > 0"
        If rbtPayment.Checked Then strSql += vbCrLf + " AND T.DEN_QTY < 0"
        strSql += vbCrLf + " GROUP BY M.DEN_VALUE,M.DEN_ORDER,M.CASHNAME"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " "
        strSql += vbCrLf + " IF (SELECT COUNT(*) FROM MASTER..DENOMRPT)>0"
        strSql += vbCrLf + " BEGIN"
        If chkGroupbyCashCounter.Checked Then
            strSql += vbCrLf + " INSERT INTO MASTER..DENOMRPT(CASHNAME,DEN_VALUE,COLHEAD,RESULT)"
            strSql += vbCrLf + " SELECT CASHNAME,CASHNAME,'T',0 FROM MASTER..DENOMRPT GROUP BY CASHNAME"
            strSql += vbCrLf + " "
            strSql += vbCrLf + " INSERT INTO MASTER..DENOMRPT(CASHNAME,DEN_VALUE,DEN_QTY,DEN_AMOUNT,DEN_ORDER,COLHEAD,RESULT)"
            strSql += vbCrLf + " SELECT CASHNAME,CASHNAME + '->TOT',SUM(DEN_QTY),SUM(DEN_AMOUNT),MAX(DEN_ORDER)+1,'S',2 FROM MASTER..DENOMRPT GROUP BY CASHNAME"
            strSql += vbCrLf + " "
        End If
        strSql += vbCrLf + " INSERT INTO MASTER..DENOMRPT(CASHNAME,DEN_VALUE,DEN_QTY,DEN_AMOUNT,DEN_ORDER,COLHEAD,RESULT)"
        strSql += vbCrLf + " SELECT 'ZZZZZZZZ','GRAND TOTAL',SUM(DEN_QTY),SUM(DEN_AMOUNT),MAX(DEN_ORDER)+1,'G',3 FROM MASTER..DENOMRPT"
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " SELECT * FROM MASTER..DENOMRPT ORDER BY CASHNAME,RESULT,DEN_ORDER"

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
        objGridShower.Text = "DENOMINATION REPORT"
        Dim tit As String = ""
        If rbtReceipt.Checked Then tit += "RECEIPT "
        If rbtPayment.Checked Then tit += "PAYMENT "
        tit += "DENOMINATION REPORT" + vbCrLf
        tit += " ON " & dtpDate.Text & ""
        objGridShower.lblTitle.Text = tit
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
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "DEN_VALUE")
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("DEN_VALUE")))
        Prop_Sets()
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = False
            Next
            .Columns("DEN_VALUE").Visible = True
            .Columns("DEN_QTY").Visible = True
            .Columns("DEN_AMOUNT").Visible = True
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New DenominationRpt_Properties
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_rbtReceipt = rbtReceipt.Checked
        obj.p_rbtPayment = rbtPayment.Checked
        obj.p_chkGroupbyCashCounter = chkGroupbyCashCounter.Checked
        obj.p_txtSystemId = txtSystemId.Text
        SetSettingsObj(obj, Me.Name, GetType(DenominationRpt_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New DenominationRpt_Properties
        GetSettingsObj(obj, Me.Name, GetType(DenominationRpt_Properties))
        rbtBoth.Checked = obj.p_rbtBoth
        rbtReceipt.Checked = obj.p_rbtReceipt
        rbtPayment.Checked = obj.p_rbtPayment
        chkGroupbyCashCounter.Checked = obj.p_chkGroupbyCashCounter
        txtSystemId.Text = obj.p_txtSystemId
    End Sub
End Class


Public Class DenominationRpt_Properties
    Private rbtBoth As Boolean = True
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
        End Set
    End Property
    Private rbtReceipt As Boolean = False
    Public Property p_rbtReceipt() As Boolean
        Get
            Return rbtReceipt
        End Get
        Set(ByVal value As Boolean)
            rbtReceipt = value
        End Set
    End Property
    Private rbtPayment As Boolean = False
    Public Property p_rbtPayment() As Boolean
        Get
            Return rbtPayment
        End Get
        Set(ByVal value As Boolean)
            rbtPayment = value
        End Set
    End Property
    Private chkGroupbyCashCounter As Boolean = False
    Public Property p_chkGroupbyCashCounter() As Boolean
        Get
            Return chkGroupbyCashCounter
        End Get
        Set(ByVal value As Boolean)
            chkGroupbyCashCounter = value
        End Set
    End Property
    Private txtSystemId As String = ""
    Public Property p_txtSystemId() As String
        Get
            Return txtSystemId
        End Get
        Set(ByVal value As String)
            txtSystemId = value
        End Set
    End Property
End Class