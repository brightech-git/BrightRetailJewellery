Imports System.Data.OleDb
Public Class frmPartlySalesReport
    'calno 181212 VASANTHAN, CLIENT -PRINCE
    'Local Variables 
    Dim dtsales As New DataTable
    Dim dtname As New DataTable
    Dim dt As New DataTable
    Dim da As OleDbDataAdapter
    Dim CCENTRE As String = Nothing
    Dim NOID As String = Nothing
    Dim StrCashId As String = Nothing
    Dim strSql As String = Nothing
    Dim cmd As OleDbCommand
    Dim SelectedCompany As String
    Dim dtGrid As New DataTable()
    Dim RPT_PARTLYWT As String = GetAdmindbSoftValue("RPT_PARTLY_WTDIFF", "0")

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        grpControls.Location = New Point((ScreenWid - grpControls.Width) / 2, ((ScreenHit - 128) - grpControls.Height) / 2)
        LoadCompany(chkLstCompany)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Try
            'Me.Cursor = Cursors.WaitCursor
            Dim Rempty As Boolean = False
            If chkLstCostCentre.Enabled = True Then
                If Not chkLstCostCentre.CheckedItems.Count > 0 Then
                    chkLstCostCentre.SetItemChecked(0, True)
                End If
            End If
            If Not chkLstNodeId.CheckedItems.Count > 0 Then
                If chkLstNodeId.Items.Count = 0 Then
                    funcAddNodeId()
                End If
                chkLstNodeId.SetItemChecked(0, True)
            End If
            If Not chkLstCashCounter.CheckedItems.Count > 0 Then
                If chkLstCashCounter.Items.Count = 0 Then
                    funcAddCashCounter()
                End If
                chkLstCashCounter.SetItemChecked(0, True)
            End If
            SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
            btnView_Search.Enabled = False
            dtsales.Clear()
            funcFilter()
            lblTitle.Text = "TITLE"
            gridView.DataSource = Nothing
            dtGrid = New DataTable
            Me.Refresh()
            ''cmd.CommandType = CommandType.StoredProcedure
            strSql = "EXEC " & cnStockDb & "..SP_RPT_PARTLYSALES"
            strSql += vbcrlf + " @DATEFROM ='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbcrlf + ",@DATETO= '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbcrlf + ",@METALNAME='" & Replace(cmbMetalName.Text, "'", "''''") & "'"
            strSql += vbcrlf + ",@ITEMNAME='" & Replace(cmbItemName.Text, "'", "''''") & "'"
            strSql += vbcrlf + ",@COSTNAME='" & Replace(CCENTRE, "'", "''") & "'"
            strSql += vbcrlf + ",@NODEID='" & Replace(NOID, "'", "''") & "'"
            strSql += vbcrlf + ",@SystemId='" & systemId & "'"
            strSql += vbcrlf + ",@cnAdminDB='" & cnAdminDb & "'"
            strSql += vbcrlf + ",@cnStockDB='" & cnStockDb & "'"
            strSql += vbCrLf + ",@CompanyId='" & SelectedCompany & "'"
            strSql += vbCrLf + ",@CASHID='" & Replace(StrCashId, "'", "''") & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT "
            Dim mtrantype As String = "'SA'"
            If chkOnlyApproval.Checked Then mtrantype = mtrantype & ",'AI'"
            If chkMiscOnly.Checked Then mtrantype = mtrantype & ",'MI'"
            strSql += vbcrlf + " WHERE TRANTYPE IN (" & mtrantype & ")"
            If ChkStnSep.Checked Then strSql += vbcrlf + " AND RESULT NOT IN(2)"
            If Val(RPT_PARTLYWT) > 0 Then strSql += vbcrlf + "  AND GRSWT > " & Val(RPT_PARTLYWT) & ""
            strSql += vbcrlf + " ORDER BY TRANNO,RESULT,BATCHNO"
            dtsales = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtsales)

            If dtsales.Rows.Count < 1 Then
                MessageBox.Show("Records not found..", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Rempty = True
                strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT (TRANNO,ITEMNAME,RESULT) VALUES(0,'NO TRANSACTION AVAILABLE',1) "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT "
                GoTo FINALVIEW
                btnView_Search.Focus()
                btnView_Search.Enabled = True
                btnNew_Click(Me, New EventArgs)
                Exit Sub
            End If

            strSql = " IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "PARTSALE')"
            strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "PARTSALE"
            strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "PARTSALE("
            strSql += vbcrlf + " TRANDATE DATETIME,"
            strSql += vbcrlf + " TRANTYPE VARCHAR(15),"
            strSql += vbcrlf + " ISSSNO VARCHAR(15),"
            strSql += vbcrlf + " TAGSNO VARCHAR(15),"
            strSql += vbcrlf + " TRANNO int,"
            strSql += vbcrlf + " ITEMID INT,"
            strSql += vbcrlf + " ITEMNAME VARCHAR(50),"
            strSql += vbcrlf + " TAGNO VARCHAR(15),"
            strSql += vbcrlf + " ACTUALPCS INT,"
            strSql += vbcrlf + " ACTUALGRSWT NUMERIC(15,3),"
            strSql += vbcrlf + " ACTUALNETWT NUMERIC(15,3),"
            strSql += vbcrlf + " SALESPCS INT,"
            strSql += vbcrlf + " SALESGRSWT NUMERIC(15,3),"
            strSql += vbcrlf + " SALESNETWT NUMERIC(15,3),"
            strSql += vbcrlf + " PCS INT,"
            strSql += vbcrlf + " GRSWT NUMERIC(15,3),"
            strSql += vbcrlf + " NETWT NUMERIC(15,3),"
            strSql += vbcrlf + " STNWT NUMERIC(15,3),"
            strSql += vbcrlf + " DESIGNER VARCHAR(50),"
            strSql += vbcrlf + " EMPNAME VARCHAR(40),"
            strSql += vbcrlf + " TABLECODE VARCHAR(40),"
            strSql += vbcrlf + " BATCHNO VARCHAR(15),"
            strSql += vbcrlf + " RESULT INT,"
            strSql += vbcrlf + " COLHEAD VARCHAR(1),"
            strSql += vbcrlf + " SNO INT IDENTITY)"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT ADD COLHEAD VARCHAR(1) "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If ChkStnSep.Checked = False Then
                strSql = " if (select count(*) from TEMPTABLEDB..temp" & systemId & "partlyreport)>0 "
                strSql += vbcrlf + " begin "
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT(TRANTYPE,ISSSNO,ITEMNAME,ITEMID,ACTUALPCS,ACTUALGRSWT, ACTUALNETWT,"
                strSql += vbcrlf + " SALESPCS, SALESGRSWT, SALESNETWT, PCS, GRSWT, NETWT,COLHEAD,RESULT)"
                strSql += vbcrlf + " SELECT TRANTYPE, CONVERT(VARCHAR,'') ISSSNO,ltrim(rtrim(ITEMNAME)), 999998, SUM(ACTUALPCS) ACTUALPCS, SUM(ACTUALGRSWT) ACTUALGRSWT, "
                strSql += vbcrlf + " SUM(ACTUALNETWT) ACTUALNETWT, SUM(SALESPCS) SALESPCS, "
                strSql += vbcrlf + " SUM(SALESGRSWT) SALESGRSWT, SUM(SALESNETWT) SALESNETWT, SUM(PCS) PCS,"
                strSql += vbcrlf + " SUM(GRSWT) GRSWT, SUM(NETWT) NETWT,'G',3"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT WHERE RESULT = 2"
                strSql += vbcrlf + " AND TRANTYPE IN (" & mtrantype & ")"
                If Val(RPT_PARTLYWT) > 0 Then strSql += vbcrlf + "  AND GRSWT > " & Val(RPT_PARTLYWT) & ""
                strSql += vbcrlf + " GROUP BY TRANTYPE,ITEMNAME "
                strSql += vbcrlf + " END "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            strSql = " if (select count(*) from TEMPTABLEDB..temp" & systemId & "partlyreport)>0 "
            strSql += vbcrlf + " begin "
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT(TRANTYPE,ISSSNO,ITEMNAME,ITEMID,ACTUALPCS,ACTUALGRSWT, ACTUALNETWT,"
            strSql += vbcrlf + " SALESPCS, SALESGRSWT, SALESNETWT, PCS, GRSWT, NETWT,STNWT,COLHEAD,RESULT)"
            strSql += vbcrlf + " SELECT TRANTYPE, CONVERT(VARCHAR,'') ISSSNO,'TOTAL', 999997, SUM(ACTUALPCS) ACTUALPCS, SUM(ACTUALGRSWT) ACTUALGRSWT, "
            strSql += vbcrlf + " SUM(ACTUALNETWT) ACTUALNETWT, SUM(SALESPCS) SALESPCS, "
            strSql += vbcrlf + " SUM(SALESGRSWT) SALESGRSWT, SUM(SALESNETWT) SALESNETWT, SUM(PCS) PCS,"
            strSql += vbcrlf + " SUM(GRSWT) GRSWT, SUM(NETWT) NETWT,SUM(STNWT) STNWT,'G',3"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT WHERE RESULT = 1"
            strSql += vbcrlf + " AND TRANTYPE IN (" & mtrantype & ")"
            If Val(RPT_PARTLYWT) > 0 Then strSql += vbcrlf + "  AND GRSWT > " & Val(RPT_PARTLYWT) & ""
            strSql += vbcrlf + " GROUP BY TRANTYPE"
            strSql += vbcrlf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            'calno 181212 
            strSql = " if (select count(*) from TEMPTABLEDB..temp" & systemId & "partlyreport)>0 "
            strSql += vbcrlf + " begin "
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT(TRANTYPE,ISSSNO,ITEMNAME,ITEMID,ACTUALPCS,ACTUALGRSWT, ACTUALNETWT,"
            strSql += vbcrlf + " SALESPCS, SALESGRSWT, SALESNETWT, PCS, GRSWT, NETWT,STNWT,COLHEAD,RESULT)"
            strSql += vbcrlf + " SELECT ''TRANTYPE, CONVERT(VARCHAR,'') ISSSNO,'GRAND TOTAL', 999999, SUM(ACTUALPCS) ACTUALPCS, SUM(ACTUALGRSWT) ACTUALGRSWT, "
            strSql += vbcrlf + " SUM(ACTUALNETWT) ACTUALNETWT, SUM(SALESPCS) SALESPCS, "
            strSql += vbcrlf + " SUM(SALESGRSWT) SALESGRSWT, SUM(SALESNETWT) SALESNETWT, SUM(PCS) PCS,"
            strSql += vbcrlf + " SUM(GRSWT) GRSWT, SUM(NETWT) NETWT,SUM(STNWT) STNWT,'G',4"
            strSql += vbcrlf + " FROM TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT WHERE RESULT = 3"
            strSql += vbcrlf + " AND TRANTYPE IN (" & mtrantype & ")"
            If Val(RPT_PARTLYWT) > 0 Then strSql += vbcrlf + "  AND GRSWT > " & Val(RPT_PARTLYWT) & ""
            strSql += vbcrlf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = " IF (SELECT COUNT(*) from TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT)>0 "
            strSql += vbcrlf + " begin "
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "PARTSALE(TRANDATE,TRANTYPE,ISSSNO, TRANNO,ITEMID,ITEMNAME,"
            strSql += vbcrlf + " TAGNO,ACTUALPCS,ACTUALGRSWT,ACTUALNETWT,SALESPCS,SALESGRSWT,SALESNETWT,PCS,"
            strSql += vbcrlf + " GRSWT,NETWT,STNWT,DESIGNER,EMPNAME,TABLECODE,BATCHNO,RESULT,COLHEAD)"
            strSql += vbcrlf + " SELECT TRANDATE,CASE WHEN TRANTYPE = 'SA' THEN 'SALES' WHEN TRANTYPE = 'MI' THEN 'MISC.ISSUE' WHEN TRANTYPE ='AI' THEN 'APPROVAL' ELSE 'OTHER' END TRANTYPE,ISSSNO,Convert(varchar(50), TRANNO)TRANNO,ITEMID,ITEMNAME,TAGNO,ACTUALPCS,ACTUALGRSWT,ACTUALNETWT,"
            strSql += vbcrlf + " SALESPCS,SALESGRSWT,SALESNETWT,PCS,GRSWT,NETWT,STNWT,DESIGNER,EMPNAME,TABLECODE,BATCHNO,RESULT,COLHEAD"
            strSql += vbCrLf + " from TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT "
            strSql += vbcrlf + " WHERE TRANTYPE IN (" & mtrantype & ")"
            If ChkStnSep.Checked Then strSql += vbcrlf + " AND RESULT NOT IN(2)"
            If Val(RPT_PARTLYWT) > 0 Then strSql += vbcrlf + "  AND GRSWT > " & Val(RPT_PARTLYWT) & ""
            strSql += vbcrlf + " ORDER BY ITEMID,TAGNO,RESULT"
            strSql += vbcrlf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT TRANDATE,TRANTYPE,ISSSNO,CONVERT(VARCHAR(50), TRANNO)TRANNO,ITEMID,ITEMNAME,"
            strSql += vbcrlf + " TAGNO,ACTUALPCS,ACTUALGRSWT,ACTUALNETWT,SALESPCS,SALESGRSWT,SALESNETWT,PCS,"
            strSql += vbCrLf + " GRSWT,NETWT,STNWT,DESIGNER,EMPNAME,TABLECODE,BATCHNO,RESULT,COLHEAD,TAGSNO,SNO from TEMPTABLEDB..TEMP" & systemId & "PARTSALE "
            If (chkOnlyApproval.Checked And chkMiscOnly.Checked) Then
                strSql += vbcrlf + " Where TRANTYPE='SALES'"
            ElseIf chkOnlyApproval.Checked Then
                strSql += vbcrlf + " Where TRANTYPE<>'APPROVAL'"
            ElseIf chkMiscOnly.Checked Then
                strSql += vbcrlf + " Where TRANTYPE<>'MISC.ISSUE'"
            End If
            strSql += vbcrlf + " ORDER BY COLHEAD,TRANNO,RESULT,TRANDATE,ISSSNO "
            dtGrid = New DataTable
            Dim dViewRow As DataRow = Nothing
            dViewRow = dtGrid.NewRow()
            dtGrid.Rows.Add(dViewRow)
FINALVIEW:
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            gridView.DataSource = dtGrid
            If dtGrid.Rows.Count > 0 Then
                If Not Rempty Then gridView.Rows(0).Cells("TRANNO").Value = "SALES"
                If gridView.Rows(0).Cells("TRANNO").Value.ToString = "SALES" Then gridView.Rows(0).DefaultCellStyle.ForeColor = Color.RoyalBlue : gridView.Rows(0).DefaultCellStyle.Font = lblTitle.Font
            End If
            CType(gridView.DataSource, DataTable).Rows.Add()

            'If chkMiscOnly.Checked Then
            '    If gridView.Rows(gridView.RowCount).Cells("TRANTYPE").Value = "MISCELLANEOUS" Then
            '        gridView.Rows(gridView.RowCount - 1).Cells("TRANNO").Value = "MISCELLANEOUS"
            '    End If
            'End If

            If chkMiscOnly.Checked = True Then
                strSql = " select TRANDATE,TRANTYPE,ISSSNO,Convert(varchar(50), TRANNO)TRANNO,ITEMID,ITEMNAME,"
                strSql += vbcrlf + " TAGNO,ACTUALPCS,ACTUALGRSWT,ACTUALNETWT,SALESPCS,SALESGRSWT,SALESNETWT,PCS,"
                strSql += vbCrLf + " GRSWT,NETWT,STNWT,DESIGNER,EMPNAME,TABLECODE,BATCHNO,RESULT,COLHEAD,TAGNO AS TAGSNO,0 AS SNO from TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT Where TranType='MI' order by ItemId,TagNo,Result,BatchNo"
                Dim dtMis As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                Dim dViewMI As DataRow = Nothing
                dViewMI = dtMis.NewRow()
                dtMis.Rows.Add(dViewMI)
                da.Fill(dtMis)
                If dtMis.Rows.Count > 1 Then
                    dtMis.Rows(0).Item("TRANNO") = "MISCELLANEOUS"
                    dtGrid.Merge(dtMis)
                    gridView.DataSource = dtGrid
                End If
                'CType(gridView.DataSource, DataTable).Rows.Add()
            End If
            If chkOnlyApproval.Checked = True Then
                strSql = " select TRANDATE,TRANTYPE,ISSSNO,Convert(varchar(50), TRANNO)TRANNO,ITEMID,ITEMNAME,"
                strSql += vbcrlf + " TAGNO,ACTUALPCS,ACTUALGRSWT,ACTUALNETWT,SALESPCS,SALESGRSWT,SALESNETWT,PCS,"
                strSql += vbCrLf + " GRSWT,NETWT,STNWT,DESIGNER,EMPNAME,TABLECODE,BATCHNO,RESULT,COLHEAD,TAGNO AS TAGSNO,0 AS SNO from TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT Where TranType='AI' order by ItemId,TagNo,Result,BatchNo"
                Dim dtApp As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                Dim dViewApp As DataRow = Nothing
                dViewApp = dtApp.NewRow()
                dtApp.Rows.Add(dViewApp)
                da.Fill(dtApp)
                If dtApp.Rows.Count > 1 Then
                    dtApp.Rows(0).Item("TRANNO") = "APPROVAL ONLY"
                    dtGrid.Merge(dtApp)
                    gridView.DataSource = dtGrid
                End If
            End If

            'calno 181212 
            If chkMiscOnly.Checked = True And chkOnlyApproval.Checked = True Then
                strSql = " select TRANDATE,TRANTYPE,ISSSNO,Convert(varchar(50), TRANNO)TRANNO,ITEMID,ITEMNAME,"
                strSql += vbcrlf + " TAGNO,ACTUALPCS,ACTUALGRSWT,ACTUALNETWT,SALESPCS,SALESGRSWT,SALESNETWT,PCS,"
                strSql += vbCrLf + " GRSWT,NETWT,STNWT,DESIGNER,EMPNAME,TABLECODE,BATCHNO,RESULT,COLHEAD,TAGNO AS TAGSNO,0 AS SNO from TEMPTABLEDB..TEMP" & systemId & "PARTLYREPORT Where ITEMNAME='GRAND TOTAL' AND RESULT=3 "
                Dim dtGt As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                Dim dViewGT As DataRow = Nothing
                dViewGT = dtGt.NewRow()
                dtGt.Rows.Add(dViewGT)
                da.Fill(dtGt)
                dtGrid.Merge(dtGt)
            End If
            gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            For i As Integer = 0 To gridView.Rows.Count - 1
                If gridView("TRANNO", i).Value.ToString <> "" Then
                    If gridView.Rows(i).Cells("TRANNO").Value.ToString = "SALES" Then gridView.Rows(i).DefaultCellStyle.ForeColor = Color.RoyalBlue : gridView.Rows(i).DefaultCellStyle.Font = lblTitle.Font
                    If gridView.Rows(i).Cells("TRANNO").Value.ToString = "APPROVAL ONLY" Then gridView.Rows(i).DefaultCellStyle.ForeColor = Color.RoyalBlue : gridView.Rows(i).DefaultCellStyle.Font = lblTitle.Font
                    If gridView.Rows(i).Cells("TRANNO").Value.ToString = "MISCELLANEOUS" Then gridView.Rows(i).DefaultCellStyle.ForeColor = Color.RoyalBlue : gridView.Rows(i).DefaultCellStyle.Font = lblTitle.Font

                End If
            Next

            funcGridSalesReportstylefunction()

            Dim strTitle As String = Nothing
            strTitle = " PARTLY SALES REPORT"
            strTitle += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
            If Strings.Right(strTitle, 3) = "AND" Then
                strTitle = Strings.Left(strTitle, strTitle.Length - 3)
            End If
            Dim Cname As String = ""
            If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
                For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                    If chkLstCostCentre.GetItemChecked(CNT) = True Then
                        Cname += "" & chkLstCostCentre.Items(CNT) + ","
                    End If
                Next
                If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
            End If

            lblTitle.Text = strTitle + Cname
            gridView.Focus()
            gridView.DataSource = dtGrid
            tabView.Show()
            If Not Rempty Then
                GridViewFormat()
                gridView.Columns("SNO").Visible = False
                btnView_Search.Enabled = True
            End If
            funcHeaderNew()
            With gridViewHead
                .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
                .Height = gridView.ColumnHeadersHeight
                Dim colWid As Integer = 0
                For cnt As Integer = 0 To gridView.ColumnCount - 1
                    If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
                Next
                If Not Rempty Then
                    If colWid >= gridView.Width Then
                        .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                        .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                    Else
                        .Columns("SCROLL").Visible = False
                    End If
                End If
            End With
            If dtsales.Rows.Count > 0 Or Rempty Then
                tabMain.SelectedTab = tabView
                gridView.Focus()
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            Me.Cursor = Cursors.Arrow
            btnView_Search.Enabled = True
        End Try
        Prop_Sets()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            lblTitle.Text = "TITLE"
            dtsales.Rows.Clear()
            'strSql = "select ' ' ITEMNAME,CONVERT(INT,NULL)TRANNO,"
            'strSql += vbcrlf + " i.tagno 'TAGNO',"
            'strSql += vbcrlf + " CONVERT(INT,0) ACTUALPCS,CONVERT(NUMERIC(15,3),0) ACTUALGRSWT,CONVERT(NUMERIC(15,3),0) ACTUALNETWT,"
            'strSql += vbcrlf + " i.pcs 'SALESPCS',i.grswt  'SALESGRSWT',i.netwt 'SALESNETWT',"
            'strSql += vbcrlf + " CONVERT(INT,0) PCS,CONVERT(NUMERIC(15,3),0) GRSWT,CONVERT(NUMERIC(15,3),0) NETWT,CONVERT(VARCHAR(40),NULL)EMPNAME,"
            'strSql += vbcrlf + " CONVERT(INT,0) ISSSNO,CONVERT(VARCHAR(20),' ') TAGSNO,CONVERT(INT,0) ITEMID,"
            'strSql += vbcrlf + " CONVERT(VARCHAR(20),' ') BATCHNO,CONVERT(INT,'') RESULT, CONVERT(VARCHAR(1),'')COLHEAD"
            'strSql += vbcrlf + " FROM " & cnStockDb & "..ISSUE AS I WHERE 1<>1 "
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dtsales)
            'Dim i As Integer
            'i = dtsales.Rows.Count
            'gridView.DataSource = dtsales
            'If i > 0 Then
            '    gridView.Rows.Clear()
            'End If
            dtpFrom.Value = GetServerDate()
            dtpTo.Value = GetServerDate()
            dtpFrom.Select()
            lblTitle.Height = gridView.ColumnHeadersHeight
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        LoadCostName(chkLstCostCentre)

        'funcAddNodeId()
        funcAddMetalName()
        funcAddCashCounter()
        funcAddItemName()
        'funcGridSalesReportstylefunction()

        If cmbItemName.Items.Count > 0 Then cmbItemName.SelectedIndex = 0
        If cmbMetalName.Items.Count > 0 Then cmbMetalName.SelectedIndex = 0
        btnNew.Enabled = True
        Prop_Gets()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub frmpartlysalesreport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExcel_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    ''Private Sub gridSalesReport_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "G"
    ''                .Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow

                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select

            End With
        Next
    End Function
    Private Sub gridsalesreport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnNew.Focus()
        End If
    End Sub

    Public Function funcHeaderNew() As Integer
        Dim dtheader As New DataTable
        dtheader.Clear()
        Try
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader

                .Columns.Add("TRANDATE", GetType(String))
                .Columns.Add("TRANNO", GetType(String))
                .Columns.Add("ITEMNAME", GetType(String))
                .Columns.Add("TAGNO", GetType(String))
                .Columns.Add("ACTUALPCS~ACTUALGRSWT~ACTUALNETWT", GetType(String))
                .Columns.Add("SALESPCS~SALESGRSWT~SALESNETWT", GetType(String))
                .Columns.Add("PCS~GRSWT~NETWT~STNWT", GetType(String))
                .Columns.Add("DESIGNER", GetType(String))
                .Columns.Add("EMPNAME", GetType(String))
                .Columns.Add("TABLECODE", GetType(String))
                .Columns.Add("SCROLL", GetType(String))
                .Columns("TRANDATE").Caption = ""
                .Columns("ITEMNAME").Caption = ""
                .Columns("TRANNO").Caption = ""
                .Columns("TAGNO").Caption = ""
                .Columns("ACTUALPCS~ACTUALGRSWT~ACTUALNETWT").Caption = "ACTUAL"
                .Columns("SALESPCS~SALESGRSWT~SALESNETWT").Caption = "SALES"
                .Columns("PCS~GRSWT~NETWT~STNWT").Caption = "DIFFERENCE"
                .Columns("DESIGNER").Caption = ""
                .Columns("EMPNAME").Caption = ""
                .Columns("TABLECODE").Caption = ""
                .Columns("Scroll").Caption = ""
            End With

            'strSql = "select ''ITEMNAME,''TAGNO,'ACTUAL'ACTUAL,'SALES'SALES,'DIFFERENCE'DIFFERENCE,''SCROLL where 1<>1"
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dtheader)
            'gridViewHead.DataSource = dtheader
            gridViewHead.DataSource = dtMergeHeader

            funcGridHeaderStyle()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Public Function funcAddMetalName() As Integer
        strSql = "select DISTINCT metalname from  " & cnAdminDb & "..metalmast order by metalname"
        cmbMetalName.Items.Clear()
        cmbMetalName.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbMetalName, False, False)
        cmbMetalName.Text = "ALL"
    End Function



    Private Sub funcAddNodeId()
        chkLstNodeId.Items.Clear()
        chkLstNodeId.Items.Add("ALL", True)

        strSql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        'strSql += vbcrlf + " UNION"
        'strSql += vbcrlf + " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN  "
        'strSql += vbcrlf + " UNION"
        'strSql += vbcrlf + " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..RECEIPT "
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then

            For CNT As Integer = 0 To dt.Rows.Count - 1
                chkLstNodeId.Items.Add(dt.Rows(CNT).Item(0).ToString)
            Next
        End If
    End Sub

    Private Sub funcAddCashCounter()
        chkLstCashCounter.Items.Clear()
        chkLstCashCounter.Items.Add("ALL", True)

        strSql = "SELECT DISTINCT CASHNAME FROM  " & cnAdminDb & "..CASHCOUNTER "
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then

            For CNT As Integer = 0 To dt.Rows.Count - 1
                chkLstCashCounter.Items.Add(dt.Rows(CNT).Item(0).ToString)
            Next
        End If
    End Sub

    Public Function funcAddItemName() As Integer
        strSql = "SELECT DISTINCT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE ISNULL(STOCKTYPE,'')<>'P' ORDER BY ITEMNAME"
        cmbItemName.Items.Clear()
        cmbItemName.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbItemName, False, False)
        cmbItemName.Text = "ALL"
    End Function

    Public Function funcGridSalesReportstylefunction() As Integer
        With gridView
            .Columns("TRANTYPE").Visible = False
            If .Columns.Contains("ISSSNO") Then .Columns("ISSSNO").Visible = False
            If .Columns.Contains("TAGSNO") Then .Columns("TAGSNO").Visible = False
            If .Columns.Contains("ITEMID") Then .Columns("ITEMID").Visible = False
            If .Columns.Contains("BATCHNO") Then .Columns("BATCHNO").Visible = False
            If .Columns.Contains("RESULT") Then .Columns("RESULT").Visible = False
            If .Columns.Contains("COLHEAD") Then .Columns("COLHEAD").Visible = False
            With .Columns("ITEMNAME")
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TRANNO")
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TAGNO")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ACTUALPCS")
                .Width = 50
                .HeaderText = "PCS"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.AliceBlue
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ACTUALGRSWT")
                .Width = 70
                .HeaderText = "GRSWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.AliceBlue
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ACTUALNETWT")
                .Width = 70
                .HeaderText = "NETWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.AliceBlue
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SALESPCS")
                .Width = 50
                .HeaderText = "PCS"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SALESGRSWT")
                .Width = 70
                .HeaderText = "GRSWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SALESNETWT")
                .Width = 70
                .HeaderText = "NETWT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PCS")
                .Width = 50
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.AliceBlue
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("GRSWT")
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.AliceBlue
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("NETWT")
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.AliceBlue
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("STNWT")
                If ChkStnSep.Checked = False Then .Visible = False
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.AliceBlue
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("DESIGNER")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.AliceBlue
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("EMPNAME")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.AliceBlue
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TABLECODE")
                .HeaderText = "TABLE"
                .Width = 50
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.AliceBlue
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Regular)
            gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function

    Public Function funcGridHeaderStyle() As Integer
        With gridViewHead
            With .Columns("ITEMNAME")
                .Width = gridView.Columns("ITEMNAME").Width
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = ""
            End With
            With .Columns("TRANNO")
                .Width = gridView.Columns("TRANNO").Width
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = ""
            End With
            With .Columns("TAGNO")
                .Width = gridView.Columns("TAGNO").Width
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = ""
            End With
            With .Columns("ACTUALPCS~ACTUALGRSWT~ACTUALNETWT")
                .Width = gridView.Columns("ACTUALPCS").Width + gridView.Columns("ACTUALGRSWT").Width + gridView.Columns("ACTUALNETWT").Width
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = "ACTUAL"
            End With
            With .Columns("SALESPCS~SALESGRSWT~SALESNETWT")
                .Width = gridView.Columns("SALESPCS").Width + gridView.Columns("SALESGRSWT").Width + gridView.Columns("SALESNETWT").Width
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = "SALES"
            End With
            With .Columns("PCS~GRSWT~NETWT~STNWT")
                .Width = gridView.Columns("PCS").Width + gridView.Columns("GRSWT").Width + gridView.Columns("NETWT").Width + IIf(gridView.Columns("STNWT").Visible, gridView.Columns("STNWT").Width, 0)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = "DIFFERENCE"
            End With
            With .Columns("DESIGNER")
                .Width = gridView.Columns("DESIGNER").Width
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = ""
            End With
            With .Columns("EMPNAME")
                .Width = gridView.Columns("EMPNAME").Width
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = ""
            End With
            With .Columns("TABLECODE")
                .Width = gridView.Columns("TABLECODE").Width
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = ""
            End With
            With .Columns("SCROLL")
                .Width = 20
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = ""
            End With
            gridViewHead.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            gridViewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function

    Private Sub gridSalesReport_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        With gridViewHead
            If .ColumnCount > 0 Then
                .Columns("TRANDATE").Width = gridView.Columns("TRANDATE").Width
                .Columns("ITEMNAME").Width = gridView.Columns("ITEMNAME").Width
                .Columns("TAGNO").Width = gridView.Columns("TAGNO").Width
                .Columns("ACTUALPCS~ACTUALGRSWT~ACTUALNETWT").Width = gridView.Columns("ACTUALPCS").Width + gridView.Columns("ACTUALGRSWT").Width + gridView.Columns("ACTUALNETWT").Width
                .Columns("SALESPCS~SALESGRSWT~SALESNETWT").Width = gridView.Columns("SALESPCS").Width + gridView.Columns("SALESGRSWT").Width + gridView.Columns("SALESNETWT").Width
                .Columns("PCS~GRSWT~NETWT~STNWT").Width = gridView.Columns("PCS").Width + gridView.Columns("GRSWT").Width + gridView.Columns("NETWT").Width + gridView.Columns("STNWT").Width
                .Columns("DESIGNER").Width = gridView.Columns("DESIGNER").Width
                .Columns("EMPNAME").Width = gridView.Columns("EMPNAME").Width
                .Columns("TABLECODE").Width = gridView.Columns("TABLECODE").Width
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        End With
    End Sub

    Private Sub gridSalesReport_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    Function funcFilter() As Integer
        ''COSTCENTRE
        CCENTRE = ""
        If chkLstCostCentre.Enabled Then
            If chkLstCostCentre.CheckedItems.Count > 0 And chkLstCostCentre.GetItemChecked(0) <> True Then
                For CNT As Integer = 0 To chkLstCostCentre.CheckedItems.Count - 1
                    CCENTRE += "'" + chkLstCostCentre.CheckedItems.Item(CNT).ToString + "'"
                    If Not (CNT = chkLstCostCentre.CheckedItems.Count - 1) Then CCENTRE += ","
                Next
            End If
        End If

        ''NODE ID
        NOID = ""
        If chkLstNodeId.CheckedItems.Count > 0 And chkLstNodeId.GetItemChecked(0) <> True Then
            For CNT As Integer = 0 To chkLstNodeId.CheckedItems.Count - 1
                NOID += "'" + chkLstNodeId.CheckedItems.Item(CNT).ToString + "'"
                If Not (CNT = chkLstNodeId.CheckedItems.Count - 1) Then NOID += ","
            Next
        End If
        ''CASHCOUNTER
        StrCashId = ""
        If chkLstCashCounter.Enabled Then
            If chkLstCashCounter.CheckedItems.Count > 0 And chkLstCashCounter.GetItemChecked(0) <> True Then
                For CNT As Integer = 0 To chkLstCashCounter.CheckedItems.Count - 1
                    StrCashId += "'" + chkLstCashCounter.CheckedItems.Item(CNT).ToString + "'"
                    If Not (CNT = chkLstCashCounter.CheckedItems.Count - 1) Then StrCashId += ","
                Next
            End If
        End If
    End Function

    Function QUERY_FOR_WITHOUT_STORED_PROCEDURE() As Integer
        'Main Table Query
        'strsql = "if (select 1 from sysobjects where name = 'TEMP" & SystemId & "PARTLYREPORT') > 0"
        'strsql = " drop table TEMP" & SystemId & "PARTLYREPORT"
        'strSql += vbcrlf + " select I.SNo as ISSSNo,IT.SNo as TagSNo,I.ItemId,"
        'strSql += vbcrlf + "(select itemname from " & cnAdminDb & "..itemmast as im where i.itemid=im.itemid) as ITEMNAME,"
        'strSql += vbcrlf + "i.tagno as TAGNO,"
        'strSql += vbcrlf + "it.pcs as ACTUALPCS,it.grswt as ACTUALGRSWT,it.netwt as ACTUALNETWT,"
        'strSql += vbcrlf + "i.pcs AS SALESPCS,i.grswt as SALESGRSWT,i.netwt as SALESNETWT,"
        'strSql += vbcrlf + "(it.pcs-i.pcs) as PCS,(it.grswt-i.grswt) as GRSWT,(it.netwt-i.netwt) as NETWT,"
        'strSql += vbcrlf + "I.BatchNo as BATCHNO,1 RESULT"
        'strSql += vbcrlf + " into TEMP" & SystemId & "PARTLYREPORT"
        'strSql += vbcrlf + " from " & cnAdminDb & "..ITEMTAG as it," & cnStockDb & "..issue as i"
        'strSql += vbcrlf + " where i.itemid = it.itemid and i.tagno=it.tagno and i.tagno<>''"
        'strSql += vbcrlf + " and i.trandate between '" & dtpDateFrom.Value.Date.ToString("yyyy-MM-dd") & "' and '" & dtpDateTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        'strSql += vbcrlf + " and ((it.grswt-i.grswt)<>0 or (it.netwt-i.netwt)<>0 or (it.grswt-i.grswt)<>0)"
        'If txtNodeId.Text <> "ALL" And txtNodeId.Text <> "" Then
        '    strSql += vbcrlf + " and i.systemid='" & Replace(txtNodeId.Text, "'", "''") & "'"
        'End If
        'If cmbCostName.Text <> "ALL" And cmbCostName.Text <> "" Then
        '    strSql += vbcrlf + " and i.costid=(select costid from " & cnAdminDb & "..costcentre where costname='" & Replace(cmbCostName.Text, "'", "''") & "')"
        'End If
        'If cmbMetalName.Text <> "ALL" And cmbMetalName.Text <> "" Then
        '    strSql += vbcrlf + " and i.metalid=(select metalid from " & cnAdminDb & "..metalmast where metalname='" & Replace(cmbMetalName.Text, "'", "''") & "')"
        'End If
        'If cmbItemName.Text <> "ALL" And cmbItemName.Text <> "" Then
        '    strSql += vbcrlf + " and i.itemid =(select itemid from " & cnAdminDb & "..itemmast where itemname='" & Replace(cmbItemName.Text, "'", "''") & "')"
        'End If
        'strsql = "SELECT * from TEMPTABLEDB..TEMP" & SystemId & "PARTLYREPORT"
        'dtsales.Clear()
        'da = New OleDbDataAdapter(strsql, cn)
        'da.Fill(dtsales)
        ''Stone Table Query
        ''Item Tagstone Query
        'strSql += vbcrlf + " if (select 1 from sysobjects  where name ='TEMP" & SystemId & "TAGSTONE') > 0 "
        'strSql += vbcrlf + " drop table TEMP" & SystemId & "TAGSTONE"
        'strSql += vbcrlf + " select T.ItemId as ITEMID,T.TagNo as TAGNO,TS.TagSNo,TS.StnItemId,"
        'strSql += vbcrlf + "case (select Diastone from " & cnAdminDb & "..ITEMMAST where ItemId = TS.StnItemId)"
        'strSql += vbcrlf + " when 'P' then '     PRECIOUS' when 'D' then '     DIOMAND' else '     STONE' END as ITEMNAME,"
        'strSql += vbcrlf + "TS.StnPCS as ACTUALSTNPCS,TS.StnWt as ACTUALGRSWT,TS.StnWt as ACTUALNTWT,T.BatchNo as BATCHNO"
        'strSql += vbcrlf + " into TEMP" & SystemId & "TAGSTONE from TEMPTABLEDB..TEMP" & SystemId & "PARTLYREPORT as T," & cnAdminDb & "..ITEMTAGSTONE as TS"
        'strSql += vbcrlf + " where T.TAGSNo =TS.TagSNo"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '
        'cmd.ExecuteNonQuery()
        '
        ''Issue Stone Query
        'strSql += vbcrlf + " if (select 1 from sysobjects where name ='TEMP" & SystemId & "ISSSTONE') > 0 "
        'strSql += vbcrlf + " drop table TEMP" & SystemId & "ISSSTONE"
        'strSql += vbcrlf + " select I.ItemId,I.TagNo,S.IssSNo,S.StnItemId,"
        'strSql += vbcrlf + "case (select Diastone from " & cnAdminDb & "..ITEMMAST where ItemId = S.StnItemId)"
        'strSql += vbcrlf + " when 'P' then '     PRECIOUS' when 'D' then '     DIOMAND' else '     STONE' end as ITEMNAME,"
        'strSql += vbcrlf + "S.StnPCS as SALESSTNPCS,S.StnWt as SALESGRSWT,S.StnWt as SALESNTWT,I.BatchNo as BATCHNO"
        'strSql += vbcrlf + " into TEMP" & SystemId & "ISSSTONE"
        'strSql += vbcrlf + " from TEMPTABLEDB..TEMP" & SystemId & "PARTLYREPORT as I," & cnStockDb & "..ISSSTONE as S"
        'strSql += vbcrlf + " where I.ISSSNo =S.ISSSNo"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '
        'cmd.ExecuteNonQuery()
        '
        ''Inner Join Query
        'strSql += vbcrlf + " if(select count(*) from TEMPTABLEDB..TEMP" & SystemId & "PARTLYREPORT)>0"
        'strSql += vbcrlf + " BEGIN"
        'strSql += vbcrlf + " insert into TEMP" & SystemId & "PARTLYREPORT"
        'strSql += vbcrlf + "  select "
        'strSql += vbcrlf + " ' ' AS ISSSNO ,' ' AS ITSNO,TS.ItemId AS ITEMID,T.ItemName AS ITEMNAME, TS.TagNo AS TAGNO,"
        'strSql += vbcrlf + "SUM(ACTUALSTNPCS) AS ACTUALPCS,SUM(ACTUALGRSWT) ACTUALGRSWT,SUM(ACTUALNTWT) ACTUALNETWT,"
        'strSql += vbcrlf + "SUM(SALESSTNPCS) SALESPCS,SUM(SALESGRSWT) SALESGRSWT,SUM(SALESNTWT) SALESNETWT,"
        'strSql += vbcrlf + "(SUM(ACTUALSTNPCS)-SUM(SALESSTNPCS)) AS PCS,"
        'strSql += vbcrlf + "(SUM(ACTUALGRSWT)-SUM(SALESGRSWT)) AS GRSWT,"
        'strSql += vbcrlf + "(SUM(ACTUALNTWT)-SUM(SALESNTWT)) AS NETWT,"
        'strSql += vbcrlf + "T.BATCHNO,2 RESULT"
        'strSql += vbcrlf + " from TEMPTABLEDB..TEMP" & SystemId & "TAGSTONE as T inner join TEMP" & SystemId & "ISSSTONE as TS"
        'strSql += vbcrlf + " on T.ItemId=TS.ItemId and T.TagNo=TS.TagNo"
        'strSql += vbcrlf + " group by T.ItemName,TS.ItemId,TS.TagNo,T.BatchNo"
        'strSql += vbcrlf + " END"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '
        'cmd.ExecuteNonQuery()
    End Function

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmPartlySalesReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmPartlySalesReport_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        cmbItemName.Text = obj.p_cmbItemName
        cmbMetalName.Text = obj.p_cmbMetalName
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        SetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId, "ALL")
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmPartlySalesReport_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        obj.p_cmbItemName = cmbItemName.Text
        obj.p_cmbMetalName = cmbMetalName.Text
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        GetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId)
        SetSettingsObj(obj, Me.Name, GetType(frmPartlySalesReport_Properties))
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

Public Class frmPartlySalesReport_Properties
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
    Private cmbItemName As String = "ALL"
    Public Property p_cmbItemName() As String
        Get
            Return cmbItemName
        End Get
        Set(ByVal value As String)
            cmbItemName = value
        End Set
    End Property
    Private cmbMetalName As String = "ALL"
    Public Property p_cmbMetalName() As String
        Get
            Return cmbMetalName
        End Get
        Set(ByVal value As String)
            cmbMetalName = value
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
End Class
