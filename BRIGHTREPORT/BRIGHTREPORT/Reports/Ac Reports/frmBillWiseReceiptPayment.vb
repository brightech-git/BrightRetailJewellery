Imports System.Data.OleDb
Public Class frmBillWiseReceiptPayment
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dt As DataTable
    Dim da As OleDbDataAdapter
    Dim StrFilter As String = Nothing
    Dim dtCostCentre As New DataTable
    Dim dtCashCounter As New DataTable
    Dim dsReport As New DataSet
    Dim TITLE As String
    Dim OnlineAccode As String = GetAdmindbSoftValue("RPT_BILLWISERECPAY_ONLACCODES", "",, True)
    Function funcGridStyle() As Integer
        With gridView
            With .Columns("TRANNO")
                .HeaderText = "BILLNO"
                .Width = 70
                .Resizable = DataGridViewTriState.False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TRANTYPE")
                .HeaderText = "TYPE"
                .Width = 50
                .Resizable = DataGridViewTriState.False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RCASH")
                .HeaderText = "CASH"
                .Width = 170
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RCHEQUE")
                .HeaderText = "CHEQUE"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RCREDITCARD")
                .HeaderText = "CREDIT CARD"
                .Width = 90
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            If OnlineAccode <> "" And chkdatewise.Checked = False Then
                With .Columns("RONLINE")
                    .HeaderText = "ONLINE"
                    .Width = 90
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Resizable = DataGridViewTriState.False
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            With .Columns("RSAVINGS")
                .HeaderText = "SAVINGS"
                .Width = 90
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RGIFTVOUCHER")
                .HeaderText = "GIFT VOUCHER"
                .Width = 90
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

            With .Columns("ICASH")
                .HeaderText = "CASH"
                .Width = 90
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ICHEQUE")
                .HeaderText = "CHEQUE"
                .Width = 90
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ICREDITCARD")
                .HeaderText = "CREDIT CARD"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            If OnlineAccode <> "" And chkdatewise.Checked = False Then
                With .Columns("IONLINE")
                    .HeaderText = "ONLINE"
                    .Width = 90
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Resizable = DataGridViewTriState.False
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
            With .Columns("ISAVINGS")
                .HeaderText = "SAVINGS"
                .Visible = False
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("IGIFTVOUCHER")
                .HeaderText = "GIFT VOUCHER"
                .Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TOTAL")
                .HeaderText = "REC-PAY"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Function
    Function funcFiltration() As String
        Dim tempchkitem As String = Nothing
        Dim tmpcnt As Integer = 0
        StrFilter = ""
        tempchkitem = ""
        ''COSTCENTRE
        If chkLstCostCentre.Enabled = True Then
            If chkLstCostCentre.Items.Count > 1 And chkLstCostCentre.CheckedItems.Count > 0 And chkLstCostCentre.GetItemChecked(0) <> True Then
                For CNT As Integer = 1 To chkLstCostCentre.Items.Count - 1
                    If chkLstCostCentre.GetItemChecked(CNT) = True Then
                        tempchkitem += " '" + dtCostCentre.Rows(CNT - 1).Item(0) + "'"
                        tmpcnt += 1
                        If tmpcnt < chkLstCostCentre.CheckedItems.Count Then tempchkitem += ","
                    End If
                Next
            Else
                tempchkitem = ""
            End If

            If tempchkitem <> "" Then
                StrFilter = " AND T.COSTID IN (" & tempchkitem & ")"
            Else
                StrFilter = ""
            End If
        End If

        ''CASH COUNTER
        tempchkitem = ""
        tmpcnt = 0
        If chkLstCashCounter.Items.Count > 0 And chkLstCashCounter.CheckedItems.Count > 0 And chkLstCashCounter.GetItemChecked(0) <> True Then
            For CNT As Integer = 1 To chkLstCashCounter.Items.Count - 1
                If chkLstCashCounter.GetItemChecked(CNT) = True Then
                    tempchkitem += " '" & dtCashCounter.Rows(CNT - 1).Item(0) + "'"
                    tmpcnt += 1
                    If tmpcnt < chkLstCashCounter.CheckedItems.Count Then tempchkitem += ","
                End If
            Next
        Else
            tempchkitem = ""
        End If
        If tempchkitem <> "" Then StrFilter += " AND CASHID IN (" & tempchkitem & ") "



        ''NODE ID
        tempchkitem = ""
        tmpcnt = 0
        If chkLstNodeId.Items.Count > 0 And chkLstNodeId.GetItemChecked(0) <> True Then
            For CNT As Integer = 1 To chkLstNodeId.Items.Count - 1
                If chkLstNodeId.GetItemChecked(CNT) = True Then
                    tempchkitem = tempchkitem & " '" & chkLstNodeId.Items.Item(CNT) + "'"
                    tmpcnt += 1
                    If tmpcnt < chkLstNodeId.CheckedItems.Count Then tempchkitem += ","
                End If
            Next
        Else
            tempchkitem = ""
        End If

        If tempchkitem <> "" Then
            StrFilter += " AND SYSTEMID IN (" & tempchkitem & ")"
        End If
        Return StrFilter
    End Function
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        pnlHeading.Visible = False
        gridView.DataSource = Nothing
        dtpDate.Value = GetServerDate()
        btnView_Search.Enabled = True
        Prop_Gets()
        dtpDate.Select()
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        funcFiltration()
        Prop_Sets()
        Dim tmpcnt As Integer
        Dim COSTID, COUNTID, NODEID, COSTNAME As String
        COSTID = "ALL" : COUNTID = "ALL" : NODEID = "ALL" : COSTNAME = "ALL"

        If (chkLstCostCentre.CheckedItems.Count = 0 And chkLstCostCentre.Enabled = True) Then chkLstCostCentre.SetItemChecked(0, True)
        If chkLstCashCounter.CheckedItems.Count = 0 Then chkLstCashCounter.SetItemChecked(0, True)
        If chkLstNodeId.CheckedItems.Count = 0 Then chkLstNodeId.SetItemChecked(0, True)
        btnView_Search.Enabled = False
        gridView.DataSource = Nothing
        Try
            If Not chkdatewise.Checked Then
                GeneralFormat()
            Else
                If chkLstCostCentre.Items.Count > 0 Then
                    tmpcnt = 0
                    If chkLstCostCentre.Items.Count > 1 And chkLstCostCentre.CheckedItems.Count > 0 And chkLstCostCentre.GetItemChecked(0) <> True Then
                        COSTID = ""
                        COSTNAME = ""
                        For CNT As Integer = 1 To chkLstCostCentre.Items.Count - 1
                            If chkLstCostCentre.GetItemChecked(CNT) = True Then
                                COSTID += "" & dtCostCentre.Rows(CNT - 1).Item("COSTID") & ""
                                COSTNAME += "" & dtCostCentre.Rows(CNT - 1).Item("COSTNAME") & ""
                                tmpcnt += 1
                                If tmpcnt < chkLstCostCentre.CheckedItems.Count Then COSTID += "," : COSTNAME += ","
                            End If
                        Next
                    End If
                End If
                tmpcnt = 0
                If chkLstCashCounter.Items.Count > 0 And chkLstCashCounter.CheckedItems.Count > 0 And chkLstCashCounter.GetItemChecked(0) <> True Then
                    COUNTID = ""
                    For CNT As Integer = 1 To chkLstCashCounter.Items.Count - 1
                        If chkLstCashCounter.GetItemChecked(CNT) = True Then
                            COUNTID += "" & dtCashCounter.Rows(CNT - 1).Item(0) + ""
                            tmpcnt += 1
                            If tmpcnt < chkLstCashCounter.CheckedItems.Count Then COUNTID += ","
                        End If
                    Next
                End If
                tmpcnt = 0
                If chkLstNodeId.Items.Count > 0 And chkLstNodeId.GetItemChecked(0) <> True Then
                    NODEID = ""
                    For CNT As Integer = 1 To chkLstNodeId.Items.Count - 1
                        If chkLstNodeId.GetItemChecked(CNT) = True Then
                            NODEID += "" & chkLstNodeId.Items.Item(CNT) + ""
                            tmpcnt += 1
                            If tmpcnt < chkLstNodeId.CheckedItems.Count Then NODEID += ","
                        End If
                    Next
                End If
                If COSTID = "" Then COSTID = "ALL"
                strSql = "EXEC " & cnAdminDb & "..SP_RPT_BILLWISERECEIPTANDPAYMENT "
                strSql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
                strSql += vbCrLf + " ,@FROMDATE='" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,@TODATE='" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,@COSTID=""" & COSTID & """"
                strSql += vbCrLf + " ,@CASHID=""" & COUNTID & """"
                strSql += vbCrLf + " ,@NODEID=""" & NODEID & """"
                Dim ds As New DataSet
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(ds, "DATEWISE")
                dt = New DataTable()
                If ds.Tables.Count > 0 Then
                    dt = ds.Tables("DATEWISE")
                Else
                    btnView_Search.Enabled = True
                    MsgBox("Record Not Found", MsgBoxStyle.Information)
                    dtpDate.Focus()
                    Exit Sub
                End If
                tabView.Show()
                gridView.DataSource = dt
                With gridView
                    .Columns("PARTICULAR").Width = 250
                    .Columns("ACDETAILS").Width = 150
                    .Columns("RECEIPT").Width = 150
                    .Columns("PAYMENT").Width = 150

                    .Columns("ACDETAILS").HeaderText = ""

                    .Columns("PARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Columns("ACDETAILS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("RECEIPT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("PAYMENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    pnlHeading.Visible = True
                End With
                Dim dthead As New DataTable
                dthead.Columns.Add("Billwise", Type.GetType("System.String"))
                gridHead1.DataSource = Nothing
                gridHead1.DataSource = dthead
                Dim width As Integer = 0
                For i As Integer = 0 To gridView.Columns.Count - 2
                    width += gridView.Columns(i).Width
                Next
                gridHead1.Columns("Billwise").Width = width
                gridHead1.Columns("Billwise").HeaderText = ""
                GridViewFormat()
                TITLE = ""
                TITLE = "BILLWISE TRANSACTION REPORT ON " & dtpDate.Text
                Dim Cname As String = ""
                If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
                    For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                        If chkLstCostCentre.GetItemChecked(CNT) = True Then
                            Cname += "" & chkLstCostCentre.Items(CNT) + ","
                        End If
                    Next
                    If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
                End If
                TITLE = TITLE + IIf(Cname <> "", " FOR COSTCENTRE" & Cname, Cname)
                lblTitle.Text = TITLE
                lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
                gridView.Columns("COLHEAD").Visible = False
                btnView_Search.Enabled = True
                If gridView.Rows.Count > 0 Then tabMain.SelectedTab = tabView
                gridView.Focus()
            End If
        Catch ex As Exception
            MsgBox(Err.Description, MsgBoxStyle.Information)
        Finally
            Me.Cursor = Cursors.Arrow

        End Try
    End Sub

    Private Sub GeneralFormat()
        ''GRID
        strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYGRID') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYGRID "
        strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYGRID("
        strSql += vbCrLf + " TRANNO INT,"
        strSql += vbCrLf + " TRANDATE SMALLDATETIME,"
        strSql += vbCrLf + " TRANTYPE VARCHAR(3),"
        strSql += vbCrLf + " RCASH VARCHAR(20),"
        strSql += vbCrLf + " RCHEQUE VARCHAR(20),"
        strSql += vbCrLf + " RCREDITCARD VARCHAR(20),"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " RONLINE VARCHAR(20),"
        End If
        strSql += vbCrLf + " RSAVINGS VARCHAR(20),"
        strSql += vbCrLf + " RGIFTVOUCHER VARCHAR(20),"
        strSql += vbCrLf + " RCREDIT VARCHAR(20),"
        strSql += vbCrLf + " RADVANCEADJ VARCHAR(20),"
        strSql += vbCrLf + " ICASH VARCHAR(20),"
        strSql += vbCrLf + " ICHEQUE VARCHAR(20),"
        strSql += vbCrLf + " ICREDITCARD VARCHAR(20),"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " IONLINE VARCHAR(20),"
        End If
        strSql += vbCrLf + " ISAVINGS VARCHAR(20),"
        strSql += vbCrLf + " IGIFTVOUCHER VARCHAR(20),"
        strSql += vbCrLf + " ICREDIT VARCHAR(20),"
        strSql += vbCrLf + " IADVANCEADJ VARCHAR(20),"
        strSql += vbCrLf + " TOTAL VARCHAR(20),"
        strSql += vbCrLf + " COLHEAD VARCHAR(1),"
        strSql += vbCrLf + " SNO INT IDENTITY)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''PRINTING    
        strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYPRINT') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYPRINT "
        strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYPRINT("
        strSql += vbCrLf + " TRANNO INT,"
        strSql += vbCrLf + " TRANDATE SMALLDATETIME,"
        strSql += vbCrLf + " TRANTYPE VARCHAR(3),"
        strSql += vbCrLf + " RCASH NUMERIC(15,2),"
        strSql += vbCrLf + " RCHEQUE NUMERIC(15,2),"
        strSql += vbCrLf + " RCREDITCARD NUMERIC(15,2),"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " RONLINE VARCHAR(20),"
        End If
        strSql += vbCrLf + " RSAVINGS NUMERIC(15,2),"
        strSql += vbCrLf + " RGIFTVOUCHER NUMERIC(15,2),"
        strSql += vbCrLf + " RCREDIT VARCHAR(20),"
        strSql += vbCrLf + " RADVANCEADJ VARCHAR(20),"
        strSql += vbCrLf + " ICASH NUMERIC(15,2),"
        strSql += vbCrLf + " ICHEQUE NUMERIC(15,2),"
        strSql += vbCrLf + " ICREDITCARD NUMERIC(15,2),"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " IONLINE VARCHAR(20),"
        End If
        strSql += vbCrLf + " ISAVINGS NUMERIC(15,2),"
        strSql += vbCrLf + " IGIFTVOUCHER NUMERIC(15,2),"
        strSql += vbCrLf + " ICREDIT NUMERIC(15,2),"
        strSql += vbCrLf + " IADVANCEADJ NUMERIC(15,2),"
        strSql += vbCrLf + " TOTAL NUMERIC(15,2),"
        strSql += vbCrLf + " RESULT INT,"
        strSql += vbCrLf + " COLHEAD VARCHAR(1),"
        strSql += vbCrLf + " SNO INT IDENTITY)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "RECPAYSUMMPRINT') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "RECPAYSUMMPRINT "
        strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "RECPAYSUMMPRINT("
        strSql += vbCrLf + " TRANNO INT,"
        strSql += vbCrLf + " TRANDATE SMALLDATETIME,"
        strSql += vbCrLf + " TRANTYPE VARCHAR(3),"
        strSql += vbCrLf + " HEAD VARCHAR(30),"
        strSql += vbCrLf + " AMOUNT NUMERIC(15,2),"
        strSql += vbCrLf + " COLHEAD VARCHAR(1),"
        strSql += vbCrLf + " SNO INT IDENTITY)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "RECPAY1')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "RECPAY1"
        strSql += vbCrLf + "  SELECT TRANNO,"
        strSql += vbCrLf + "  TRANDATE,"
        strSql += vbCrLf + "  CASE WHEN PAYMODE IN ('CA') THEN 'CASH' "
        strSql += vbCrLf + "  WHEN PAYMODE IN ('CH') THEN 'CHEQUE'"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + "  WHEN PAYMODE IN ('CC') and T.ACCODE NOT IN ('" & OnlineAccode.Replace(",", "','") & "') THEN 'CREDITCARD'"
            strSql += vbCrLf + "  WHEN PAYMODE IN ('CC') and T.ACCODE IN ('" & OnlineAccode.Replace(",", "','") & "') THEN 'ONLINE'"
        Else
            strSql += vbCrLf + "  WHEN PAYMODE IN ('CC') THEN 'CREDITCARD'"
        End If

        strSql += vbCrLf + "  WHEN PAYMODE IN ('SS','CG','CB','CZ','CD') THEN 'SAVINGS'"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('AA') THEN 'ADVADJUSTMENT'"
        strSql += vbCrLf + "  WHEN PAYMODE IN('DU','DP') THEN 'CREDIT'"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('GV') THEN 'GIFTVOUCHER' ELSE'' END PAYMODE"
        strSql += vbCrLf + "  ,(CASE WHEN PAYMODE IN ('SS','CG','CB','CZ','CD') THEN "
        strSql += vbCrLf + "  (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO=T.BATCHNO AND PAYMODE='SS')) ELSE A.ACNAME END) ACNAME"
        strSql += vbCrLf + "  ,ISNULL((SELECT TOP 1 TRANTYPE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = T.BATCHNO AND COMPANYID = '" & strCompanyId & "'),'') AS ITRANTYPE"
        strSql += vbCrLf + "  ,ISNULL((SELECT TOP 1 TRANTYPE FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = T.BATCHNO AND COMPANYID = '" & strCompanyId & "'),'') AS RTRANTYPE"
        strSql += vbCrLf + "  ,AMOUNT "
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='D' AND PAYMODE = 'CA' THEN AMOUNT ELSE 0 END) AS RCASH"
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='D' AND PAYMODE = 'CH' THEN AMOUNT ELSE 0 END) AS RCHEQUE"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='D' AND PAYMODE = 'CC' and T.ACCODE NOT IN ('" & OnlineAccode.Replace(",", "','") & "') THEN AMOUNT ELSE 0 END) AS RCREDITCARD"
            strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='D' AND PAYMODE = 'CC' and T.ACCODE IN ('" & OnlineAccode.Replace(",", "','") & "') THEN AMOUNT ELSE 0 END) AS RONLINE"
        Else
            strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='D' AND PAYMODE = 'CC' THEN AMOUNT ELSE 0 END) AS RCREDITCARD"
        End If
        strSql += vbCrLf + "  ,SUM(CASE WHEN PAYMODE IN ('SS','CG','CB','CZ','CD') THEN CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END ELSE 0 END) AS RSAVINGS"
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='D' AND PAYMODE = 'AA' THEN AMOUNT ELSE 0 END) AS RADVANCEADJ"
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='D' AND PAYMODE IN('DU','DP') THEN AMOUNT ELSE 0 END) AS RCREDIT"
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='D' AND PAYMODE = 'GV' THEN AMOUNT ELSE 0 END) AS RGIFTVOUCHER"
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='C' AND PAYMODE = 'CA' THEN AMOUNT ELSE 0 END) AS ICASH"
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='C' AND PAYMODE = 'CH' THEN AMOUNT ELSE 0 END) AS ICHEQUE"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='C' AND PAYMODE = 'CC' AND T.ACCODE NOT IN ('" & OnlineAccode.Replace(",", "','") & "') THEN AMOUNT ELSE 0 END) AS ICREDITCARD"
            strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='C' AND PAYMODE = 'CC' AND T.ACCODE IN ('" & OnlineAccode.Replace(",", "','") & "') THEN AMOUNT ELSE 0 END) AS IONLINE"
        Else
            strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='C' AND PAYMODE = 'CC' THEN AMOUNT ELSE 0 END) AS ICREDITCARD"
        End If

        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='C' AND PAYMODE = 'SS' THEN AMOUNT ELSE 0 END) AS ISAVINGS"
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='C' AND PAYMODE = 'AA' THEN AMOUNT ELSE 0 END) AS IADVANCEADJ"
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='C' AND PAYMODE IN('DU','DP') THEN AMOUNT ELSE 0 END) AS ICREDIT"
        strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='C' AND PAYMODE = 'GV' THEN AMOUNT ELSE 0 END) AS IGIFTVOUCHER"
        strSql += vbCrLf + "  ,UPDATED,UPTIME,COUNT(PAYMODE)NOOFTRAN"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & "RECPAY1"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ACHEAD AS A ON T.ACCODE=A.ACCODE"
        strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND PAYMODE IN ('CA','CH','CC','SS','GV','CG','CB','CZ','CD','AA','DU','DP')"
        strSql += vbCrLf + " AND T.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(T.CANCEL,'') = ''"
        strSql += StrFilter
        strSql += vbCrLf + "  GROUP BY TRANNO,TRANDATE,PAYMODE,AMOUNT,BATCHNO,UPDATED,UPTIME,A.ACCODE,A.ACNAME"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " ,T.ACCODE"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "RECPAY') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "RECPAY "
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " TRANNO,TRANDATE"
        strSql += vbCrLf + " ,CASE WHEN ITRANTYPE <> '' THEN ITRANTYPE ELSE RTRANTYPE END AS TRANTYPE"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " ,SUM(RCASH) RCASH, SUM(RCHEQUE) RCHEQUE, SUM(RCREDITCARD) RCREDITCARD, SUM(RONLINE) RONLINE, SUM(RSAVINGS) RSAVINGS, SUM(RGIFTVOUCHER) RGIFTVOUCHER"
        Else
            strSql += vbCrLf + " ,SUM(RCASH) RCASH, SUM(RCHEQUE) RCHEQUE, SUM(RCREDITCARD) RCREDITCARD, SUM(RSAVINGS) RSAVINGS, SUM(RGIFTVOUCHER) RGIFTVOUCHER"
        End If
        strSql += vbCrLf + " ,SUM(RCREDIT) RCREDIT, SUM(RADVANCEADJ) RADVANCEADJ"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " ,SUM(ICASH) ICASH, SUM(ICHEQUE) ICHEQUE, SUM(ICREDITCARD) ICREDITCARD, SUM(IONLINE) IONLINE, SUM(ISAVINGS) ISAVINGS, SUM(IGIFTVOUCHER) IGIFTVOUCHER"
        Else
            strSql += vbCrLf + " ,SUM(ICASH) ICASH, SUM(ICHEQUE) ICHEQUE, SUM(ICREDITCARD) ICREDITCARD, SUM(ISAVINGS) ISAVINGS, SUM(IGIFTVOUCHER) IGIFTVOUCHER"
        End If
        strSql += vbCrLf + " ,SUM(ICREDIT) ICREDIT, SUM(IADVANCEADJ) IADVANCEADJ"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " ,SUM(RCASH+RCHEQUE+RCREDITCARD+RONLINE+RSAVINGS+RGIFTVOUCHER+RCREDIT+RADVANCEADJ)-SUM(ICASH+ICHEQUE+ICREDITCARD+IONLINE+ISAVINGS+IGIFTVOUCHER+ICREDIT+IADVANCEADJ)AS TOTAL"
        Else
            strSql += vbCrLf + " ,SUM(RCASH+RCHEQUE+RCREDITCARD+RSAVINGS+RGIFTVOUCHER+RCREDIT+RADVANCEADJ)-SUM(ICASH+ICHEQUE+ICREDITCARD+ISAVINGS+IGIFTVOUCHER+ICREDIT+IADVANCEADJ)AS TOTAL"
        End If
        strSql += vbCrLf + " ,UPDATED,UPTIME,'1'RESULT, 'C' COLHEAD"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "RECPAY"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY1"
        strSql += vbCrLf + " )X"
        strSql += vbCrLf + " GROUP BY TRANNO,TRANDATE,ITRANTYPE,RTRANTYPE,UPDATED,UPTIME"
        strSql += vbCrLf + " ORDER BY UPDATED,UPTIME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'SUBTOTAL
        strSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "RECPAY(TRANTYPE,RCASH,RCHEQUE,"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " RCREDITCARD,RONLINE, RSAVINGS, RGIFTVOUCHER,RCREDIT,RADVANCEADJ, ICASH,ICHEQUE, ICREDITCARD,IONLINE,"
        Else
            strSql += vbCrLf + " RCREDITCARD, RSAVINGS, RGIFTVOUCHER,RCREDIT,RADVANCEADJ, ICASH,ICHEQUE, ICREDITCARD,"
        End If
        strSql += vbCrLf + " ISAVINGS, IGIFTVOUCHER,ICREDIT,IADVANCEADJ, TOTAL, RESULT,COLHEAD) "
        strSql += vbCrLf + " SELECT ' ', "
        strSql += vbCrLf + " SUM(RCASH) RCASH, SUM(RCHEQUE) RCHEQUE,"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " SUM(RCREDITCARD) RCREDITCARD,SUM(RONLINE) RONLINE, SUM(RSAVINGS) RSAVINGS,"
        Else
            strSql += vbCrLf + " SUM(RCREDITCARD) RCREDITCARD, SUM(RSAVINGS) RSAVINGS,"
        End If
        strSql += vbCrLf + " SUM(RGIFTVOUCHER) RGIFTVOUCHER,"
        strSql += vbCrLf + " SUM(RCREDIT) RCREDIT,SUM(RADVANCEADJ) RADVANCEADJ,"
        strSql += vbCrLf + " SUM(ICASH) ICASH,"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " SUM(ICHEQUE) ICHEQUE, SUM(ICREDITCARD) ICREDITCARD, SUM(IONLINE) IONLINE,"
        Else
            strSql += vbCrLf + " SUM(ICHEQUE) ICHEQUE, SUM(ICREDITCARD) ICREDITCARD,"
        End If
        strSql += vbCrLf + " SUM(ISAVINGS) ISAVINGS, SUM(IGIFTVOUCHER) IGIFTVOUCHER,"
        strSql += vbCrLf + " SUM(ICREDIT) ICREDIT,SUM(IADVANCEADJ) IADVANCEADJ,"
        strSql += vbCrLf + " SUM(TOTAL) TOTAL, 2 RESULT,'S' COLHEAD"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY "
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY)>0 " + vbCrLf
        strSql += vbCrLf + " BEGIN " + vbCrLf
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYPRINT(TRANNO,TRANDATE,TRANTYPE,RCASH,RCHEQUE," + vbCrLf
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " RCREDITCARD, RONLINE, RSAVINGS, RGIFTVOUCHER,RCREDIT,RADVANCEADJ, ICASH,ICHEQUE, ICREDITCARD, IONLINE," + vbCrLf
        Else
            strSql += vbCrLf + " RCREDITCARD, RSAVINGS, RGIFTVOUCHER,RCREDIT,RADVANCEADJ, ICASH,ICHEQUE, ICREDITCARD," + vbCrLf
        End If

        strSql += vbCrLf + " ISAVINGS, IGIFTVOUCHER,ICREDIT,IADVANCEADJ, TOTAL, COLHEAD) " + vbCrLf
        strSql += vbCrLf + " SELECT TRANNO, TRANDATE,TRANTYPE, " + vbCrLf
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " RCASH, RCHEQUE, RCREDITCARD, RONLINE, RSAVINGS, RGIFTVOUCHER,RCREDIT,RADVANCEADJ," + vbCrLf
            strSql += vbCrLf + " ICASH, ICHEQUE, ICREDITCARD, IONLINE, ISAVINGS, IGIFTVOUCHER,ICREDIT,IADVANCEADJ," + vbCrLf
        Else
            strSql += vbCrLf + " RCASH, RCHEQUE, RCREDITCARD, RSAVINGS, RGIFTVOUCHER,RCREDIT,RADVANCEADJ," + vbCrLf
            strSql += vbCrLf + " ICASH, ICHEQUE, ICREDITCARD, ISAVINGS, IGIFTVOUCHER,ICREDIT,IADVANCEADJ," + vbCrLf
        End If
        strSql += vbCrLf + " TOTAL, COLHEAD" + vbCrLf
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY " + vbCrLf
        strSql += vbCrLf + " ORDER BY RESULT,TRANTYPE,TRANNO" + vbCrLf
        strSql += vbCrLf + " END " + vbCrLf
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If OnlineAccode <> "" Then
            strSql = " SELECT TRANNO,TRANDATE, TRANTYPE, RCASH, RCHEQUE, RCREDITCARD, RONLINE, RSAVINGS, RGIFTVOUCHER,RCREDIT,RADVANCEADJ,"
            strSql += vbCrLf + " ICASH, ICHEQUE, ICREDITCARD, IONLINE, ISAVINGS, IGIFTVOUCHER,ICREDIT,IADVANCEADJ, TOTAL, COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYPRINT  ORDER BY SNO"
        Else
            strSql = " SELECT TRANNO,TRANDATE, TRANTYPE, RCASH, RCHEQUE, RCREDITCARD, RSAVINGS, RGIFTVOUCHER,RCREDIT,RADVANCEADJ,"
            strSql += vbCrLf + " ICASH, ICHEQUE, ICREDITCARD, ISAVINGS, IGIFTVOUCHER,ICREDIT,IADVANCEADJ, TOTAL, COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYPRINT  ORDER BY SNO"
        End If
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        dt.AcceptChanges()
        If dt.Rows.Count > 0 Then
            dsReport.Tables.Add(dt)
        Else
            btnView_Search.Enabled = True
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            dtpDate.Focus()
            Exit Sub
        End If

        strSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "RECPAYSUMM')>0"
        strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "RECPAYSUMM"
        strSql += vbCrLf + " SELECT HEAD,AMOUNT,COLHEAD INTO TEMPTABLEDB..TEMP" & systemId & "RECPAYSUMM FROM ("
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " SELECT 'RECEIPT' HEAD, (RCASH+RCHEQUE+RCREDITCARD+RONLINE+RSAVINGS+RGIFTVOUCHER+RCREDIT+RADVANCEADJ) AMOUNT, "
        Else
            strSql += vbCrLf + " SELECT 'RECEIPT' HEAD, (RCASH+RCHEQUE+RCREDITCARD+RSAVINGS+RGIFTVOUCHER+RCREDIT+RADVANCEADJ) AMOUNT, "
        End If
        strSql += vbCrLf + " ' ' COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY WHERE COLHEAD = 'S'"
        strSql += vbCrLf + " UNION ALL"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " SELECT 'PAYMENT' HEAD, (ICASH+ICHEQUE+ICREDITCARD+IONLINE+ISAVINGS+IGIFTVOUCHER+ICREDIT+IADVANCEADJ), ' ' "
        Else
            strSql += vbCrLf + " SELECT 'PAYMENT' HEAD, (ICASH+ICHEQUE+ICREDITCARD+ISAVINGS+IGIFTVOUCHER+ICREDIT+IADVANCEADJ), ' ' "
        End If
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY WHERE COLHEAD = 'S'"
        strSql += vbCrLf + " UNION ALL"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " SELECT 'SUB TOTAL' HEAD, (RCASH+RCHEQUE+RCREDITCARD+RONLINE+RSAVINGS+RGIFTVOUCHER+RCREDIT+RADVANCEADJ)-"
            strSql += vbCrLf + " (ICASH+ICHEQUE+ICREDITCARD+IONLINE+ISAVINGS+IGIFTVOUCHER+ICREDIT+IADVANCEADJ), 'S' "
        Else
            strSql += vbCrLf + " SELECT 'SUB TOTAL' HEAD, (RCASH+RCHEQUE+RCREDITCARD+RSAVINGS+RGIFTVOUCHER+RCREDIT+RADVANCEADJ)-"
            strSql += vbCrLf + " (ICASH+ICHEQUE+ICREDITCARD+ISAVINGS+IGIFTVOUCHER+ICREDIT+IADVANCEADJ), 'S' "
        End If
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY WHERE COLHEAD = 'S'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT NULL,NULL,NULL "
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT 'CASH' HEAD, (RCASH-ICASH), ' '"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY WHERE COLHEAD = 'S'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'CHEQUE' , (RCHEQUE-ICHEQUE), ' '"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY WHERE COLHEAD = 'S'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'CREDITCARD' HEAD, (RCREDITCARD-ICREDITCARD), ' '"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY WHERE COLHEAD = 'S'"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'ONLINE' HEAD, (RONLINE-IONLINE), ' '"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY WHERE COLHEAD = 'S'"
        End If
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'SAVINGS' HEAD, (RSAVINGS-ISAVINGS), ' '"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY WHERE COLHEAD = 'S'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'GIFTVOUCHER' HEAD, (RGIFTVOUCHER-IGIFTVOUCHER), ' '"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY WHERE COLHEAD = 'S'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'CREDIT' HEAD, (RCREDIT-ICREDIT), ' '"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY WHERE COLHEAD = 'S'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'ADVANCE_ADJUSTED' HEAD, (RADVANCEADJ-IADVANCEADJ), ' '"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY WHERE COLHEAD = 'S'"
        strSql += vbCrLf + " UNION ALL"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " SELECT 'SUB TOTAL' HEAD, (RCASH+RCHEQUE+RCREDITCARD+RONLINE+RSAVINGS+RGIFTVOUCHER+RCREDIT+RADVANCEADJ)-"
            strSql += vbCrLf + " (ICASH+ICHEQUE+ICREDITCARD+IONLINE+ISAVINGS+IGIFTVOUCHER+ICREDIT+IADVANCEADJ), 'S' "
        Else
            strSql += vbCrLf + " SELECT 'SUB TOTAL' HEAD, (RCASH+RCHEQUE+RCREDITCARD+RSAVINGS+RGIFTVOUCHER+RCREDIT+RADVANCEADJ)-"
            strSql += vbCrLf + " (ICASH+ICHEQUE+ICREDITCARD+ISAVINGS+IGIFTVOUCHER+ICREDIT+IADVANCEADJ), 'S' "
        End If
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY WHERE COLHEAD = 'S' ) X"
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "RECPAYSUMM)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "RECPAYSUMMPRINT(TRANNO,TRANTYPE,HEAD,AMOUNT,COLHEAD)"
        strSql += vbCrLf + " SELECT NULL, ' ', "
        strSql += vbCrLf + " HEAD, "
        strSql += vbCrLf + " (CASE WHEN AMOUNT=0 THEN NULL ELSE AMOUNT END) AMOUNT, COLHEAD"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAYSUMM"
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT TRANNO, TRANTYPE, HEAD, AMOUNT, COLHEAD"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAYSUMMPRINT ORDER BY SNO"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        dt.AcceptChanges()
        If dt.Rows.Count > 0 Then dsReport.Tables.Add(dt)

        'GRID
        strSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYGRID(TRANNO,TRANDATE,TRANTYPE,"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " RCASH, RCHEQUE, RCREDITCARD, RONLINE, RSAVINGS, RGIFTVOUCHER,RCREDIT,RADVANCEADJ, "
            strSql += vbCrLf + " ICASH, ICHEQUE, ICREDITCARD, IONLINE, ISAVINGS, IGIFTVOUCHER,ICREDIT,IADVANCEADJ, "
        Else
            strSql += vbCrLf + " RCASH, RCHEQUE, RCREDITCARD, RSAVINGS, RGIFTVOUCHER,RCREDIT,RADVANCEADJ, "
            strSql += vbCrLf + " ICASH, ICHEQUE, ICREDITCARD, ISAVINGS, IGIFTVOUCHER,ICREDIT,IADVANCEADJ, "
        End If
        strSql += vbCrLf + " TOTAL, COLHEAD) "
        strSql += vbCrLf + " SELECT TRANNO,TRANDATE, TRANTYPE,"
        strSql += vbCrLf + " CASE WHEN RCASH=0 THEN NULL ELSE CONVERT(VARCHAR(20),RCASH) END RCASH, "
        strSql += vbCrLf + " CASE WHEN RCHEQUE=0 THEN NULL ELSE CONVERT(VARCHAR(20),RCHEQUE) END RCHEQUE, "
        strSql += vbCrLf + " CASE WHEN RCREDITCARD=0 THEN NULL ELSE CONVERT(VARCHAR(20),RCREDITCARD) END RCREDITCARD,"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " CASE WHEN RONLINE=0 THEN NULL ELSE CONVERT(VARCHAR(20),RONLINE) END RONLINE,"
        End If
        strSql += vbCrLf + " CASE WHEN RSAVINGS=0 THEN NULL ELSE CONVERT(VARCHAR(20),RSAVINGS) END RSAVINGS, "
        strSql += vbCrLf + " CASE WHEN RGIFTVOUCHER=0 THEN NULL ELSE CONVERT(VARCHAR(20),RGIFTVOUCHER) END RGIFTVOUCHER,"

        strSql += vbCrLf + " CASE WHEN RCREDIT=0 THEN NULL ELSE CONVERT(VARCHAR(20),RCREDIT) END RCREDIT,"
        strSql += vbCrLf + " CASE WHEN RADVANCEADJ=0 THEN NULL ELSE CONVERT(VARCHAR(20),RADVANCEADJ) END RADVANCEADJ,"

        strSql += vbCrLf + " CASE WHEN ICASH=0 THEN NULL ELSE CONVERT(VARCHAR(20),ICASH) END ICASH, "
        strSql += vbCrLf + " CASE WHEN ICHEQUE=0 THEN NULL ELSE CONVERT(VARCHAR(20),ICHEQUE) END ICHEQUE, "
        strSql += vbCrLf + " CASE WHEN ICREDITCARD=0 THEN NULL ELSE CONVERT(VARCHAR(20),ICREDITCARD) END ICREDITCARD,"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + " CASE WHEN IONLINE=0 THEN NULL ELSE CONVERT(VARCHAR(20),IONLINE) END IONLINE,"
        End If
        strSql += vbCrLf + " CASE WHEN ISAVINGS=0 THEN NULL ELSE CONVERT(VARCHAR(20),ISAVINGS) END ISAVINGS, "
        strSql += vbCrLf + " CASE WHEN IGIFTVOUCHER=0 THEN NULL ELSE CONVERT(VARCHAR(20),IGIFTVOUCHER) END IGIFTVOUCHER,"

        strSql += vbCrLf + " CASE WHEN ICREDIT=0 THEN NULL ELSE CONVERT(VARCHAR(20),ICREDIT) END ICREDIT,"
        strSql += vbCrLf + " CASE WHEN IADVANCEADJ=0 THEN NULL ELSE CONVERT(VARCHAR(20),IADVANCEADJ) END IADVANCEADJ,"

        strSql += vbCrLf + " CASE WHEN TOTAL=0 THEN NULL ELSE CONVERT(VARCHAR(20),TOTAL) END TOTAL, "
        strSql += vbCrLf + " COLHEAD"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY ORDER BY RESULT, TRANDATE,TRANNO, TRANTYPE"
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "RECPAYSUMMPRINT)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYGRID(TRANNO)VALUES (NULL) "
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYGRID(TRANNO,TRANDATE,TRANTYPE,"
        strSql += vbCrLf + " RCASH, RCHEQUE, COLHEAD) "
        strSql += vbCrLf + " SELECT TRANNO,TRANDATE, TRANTYPE, HEAD,"
        strSql += vbCrLf + " CONVERT(VARCHAR(20),AMOUNT)AMOUNT, COLHEAD"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAYSUMMPRINT ORDER BY SNO"
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'ALTER   
        strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "RECPAY2')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "RECPAY2"
        strSql += vbCrLf + "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY1)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + "  SELECT PARTICULAR"
        strSql += vbCrLf + "  ,CASE WHEN NOOFTRAN=0 THEN '' ELSE CONVERT(VARCHAR(20),NOOFTRAN)END NOOFTRAN"
        strSql += vbCrLf + "  ,CASE WHEN RAMOUNT=0 THEN '' ELSE CONVERT(VARCHAR(20),RAMOUNT)END RECEIPT"
        strSql += vbCrLf + "  ,CASE WHEN IAMOUNT=0 THEN '' ELSE CONVERT(VARCHAR(20),IAMOUNT)END PAYMENT"
        strSql += vbCrLf + "  ,CASE WHEN COLHEAD<>'S' THEN '' ELSE CONVERT(VARCHAR(20),(RAMOUNT-IAMOUNT))END AMOUNT"
        strSql += vbCrLf + "  ,COLHEAD,RESULT "
        strSql += vbCrLf + "  ,IDENTITY(INT,1,1)KEYNO"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & "RECPAY2"
        strSql += vbCrLf + "  FROM("
        strSql += vbCrLf + "  SELECT DISTINCT PAYMODE PARTICULAR,PAYMODE,0 NOOFTRAN,0.00 RAMOUNT,0.00 IAMOUNT,'T1'COLHEAD,3 RESULT FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY1 GROUP BY PAYMODE,ACNAME"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT ACNAME,PAYMODE,CONVERT(VARCHAR(50),SUM(NOOFTRAN))NOOFTRAN"
        strSql += vbCrLf + "  ,SUM(CASE WHEN PAYMODE IN ('CASH') THEN RCASH"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('CHEQUE') THEN RCHEQUE"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('CREDITCARD') THEN RCREDITCARD"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + "  WHEN PAYMODE IN ('ONLINE') THEN RONLINE"
        End If
        strSql += vbCrLf + "  WHEN PAYMODE IN ('ADVADJUSTMENT') THEN RADVANCEADJ"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('CREDIT') THEN RCREDIT"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('SAVINGS') THEN RSAVINGS"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('GIFTVOUCHER') THEN RGIFTVOUCHER ELSE 0 END) RAMOUNT"
        strSql += vbCrLf + "  ,SUM(CASE WHEN PAYMODE IN ('CASH') THEN ICASH"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('CHEQUE') THEN ICHEQUE"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('CREDITCARD') THEN ICREDITCARD"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + "  WHEN PAYMODE IN ('ONLINE') THEN IONLINE"
        End If
        strSql += vbCrLf + "  WHEN PAYMODE IN ('ADVADJUSTMENT') THEN IADVANCEADJ"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('CREDIT') THEN ICREDIT"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('SAVINGS') THEN ISAVINGS"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('GIFTVOUCHER') THEN IGIFTVOUCHER ELSE 0 END) IAMOUNT"
        strSql += vbCrLf + "  ,'D' COLHEAD,4 RESULT FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY1 GROUP BY ACNAME,PAYMODE"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT 'SUBTOTAL' ACNAME,PAYMODE,0 NOOFTRAN"
        strSql += vbCrLf + "  ,SUM(CASE WHEN PAYMODE IN ('CASH') THEN RCASH"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('CHEQUE') THEN RCHEQUE"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('CREDITCARD') THEN RCREDITCARD"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + "  WHEN PAYMODE IN ('ONLINE') THEN RONLINE"
        End If
        strSql += vbCrLf + "  WHEN PAYMODE IN ('ADVADJUSTMENT') THEN RADVANCEADJ"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('CREDIT') THEN RCREDIT"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('SAVINGS') THEN RSAVINGS"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('GIFTVOUCHER') THEN RGIFTVOUCHER ELSE 0 END) RAMOUNT"
        strSql += vbCrLf + "  ,SUM(CASE WHEN PAYMODE IN ('CASH') THEN ICASH"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('CHEQUE') THEN ICHEQUE"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('CREDITCARD') THEN ICREDITCARD"
        If OnlineAccode <> "" Then
            strSql += vbCrLf + "  WHEN PAYMODE IN ('ONLINE') THEN IONLINE"
        End If
        strSql += vbCrLf + "  WHEN PAYMODE IN ('ADVADJUSTMENT') THEN IADVANCEADJ"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('CREDIT') THEN ICREDIT"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('SAVINGS') THEN ISAVINGS"
        strSql += vbCrLf + "  WHEN PAYMODE IN ('GIFTVOUCHER') THEN IGIFTVOUCHER ELSE 0 END) IAMOUNT"
        strSql += vbCrLf + "  ,'S' COLHEAD,5 RESULT FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY1 GROUP BY PAYMODE)X"
        strSql += vbCrLf + "  ORDER BY PAYMODE,RESULT"
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYGRID ALTER COLUMN RCASH VARCHAR(100)"
        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYGRID ALTER COLUMN RCHEQUE VARCHAR(50)"
        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYGRID ALTER COLUMN COLHEAD VARCHAR(2)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY2)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYGRID(TRANNO)VALUES (NULL) "
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYGRID(TRANNO,TRANTYPE,RCASH, RCHEQUE,RCREDITCARD,RSAVINGS,RGIFTVOUCHER, COLHEAD) "
        strSql += vbCrLf + " SELECT '','','MODE','NO.OF TRAN','RECEIPT','PAYMENT','AMOUNT','T2'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT '','',PARTICULAR,CONVERT(VARCHAR(3),NOOFTRAN),CONVERT(VARCHAR(100),RECEIPT),CONVERT(VARCHAR(100),PAYMENT),CONVERT(VARCHAR(100),AMOUNT),COLHEAD"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "RECPAY2 "
        strSql += vbCrLf + " End"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        If OnlineAccode <> "" Then
            strSql = "SELECT TRANNO,TRANDATE,TRANTYPE,RCASH,RCHEQUE,RCREDITCARD,RONLINE,RSAVINGS,RGIFTVOUCHER,RCREDIT,RADVANCEADJ,"
            strSql += vbCrLf + " ICASH, ICHEQUE, ICREDITCARD, IONLINE, ISAVINGS, IGIFTVOUCHER,ICREDIT,IADVANCEADJ, TOTAL, COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYGRID ORDER BY SNO"
        Else
            strSql = "SELECT TRANNO,TRANDATE,TRANTYPE,RCASH,RCHEQUE,RCREDITCARD,RSAVINGS,RGIFTVOUCHER,RCREDIT,RADVANCEADJ,"
            strSql += vbCrLf + " ICASH, ICHEQUE, ICREDITCARD, ISAVINGS, IGIFTVOUCHER,ICREDIT,IADVANCEADJ, TOTAL, COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "BILLRECPAYGRID ORDER BY SNO"
        End If
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count < 1 Then
            btnView_Search.Enabled = True
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            dtpDate.Focus()
            Exit Sub
        End If
        tabView.Show()
        gridView.DataSource = dt
        gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        GridViewFormat()
        lblTitle.Text = "BILLWISE TRANSACTION REPORT DATE FROM " & dtpDate.Text & " TO " & dtpToDate.Text

        TITLE = ""
        TITLE = "BILLWISE TRANSACTION REPORT DATE FROM " & dtpDate.Text & " TO " & dtpToDate.Text
        Dim Cname As String = ""
        If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                If chkLstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chkLstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        End If
        TITLE = TITLE + IIf(Cname <> "", " FOR COSTCENTRE" & Cname, Cname)
        lblTitle.Text = TITLE

        lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        gridView.Columns("COLHEAD").Visible = False
        funcGridStyle()
        funcGridHead()
        btnView_Search.Enabled = True
        If gridView.Rows.Count > 0 Then tabMain.SelectedTab = tabView
        gridView.Focus()
    End Sub
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            Select Case dgvRow.Cells("COLHEAD").Value.ToString
                Case "S"
                    dgvRow.DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                    dgvRow.DefaultCellStyle.Font = reportSubTotalStyle.Font
                Case "T"
                    dgvRow.Cells("PARTICULAR").Style.BackColor = Color.LightGreen
                    dgvRow.Cells("PARTICULAR").Style.ForeColor = Color.Red
                    dgvRow.DefaultCellStyle.Font = reportHeadStyle.Font
                Case "T1"
                    dgvRow.DefaultCellStyle.BackColor = reportHeadStyle.BackColor
                    dgvRow.DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                    dgvRow.DefaultCellStyle.Font = reportHeadStyle.Font
                Case "T2"
                    dgvRow.DefaultCellStyle.BackColor = reportHeadStyle1.BackColor
                    dgvRow.DefaultCellStyle.ForeColor = reportHeadStyle1.ForeColor
                    dgvRow.DefaultCellStyle.Font = reportHeadStyle1.Font
                Case "C"
                    dgvRow.Cells("RCASH").Style.BackColor = Color.LightBlue
                    dgvRow.Cells("RCHEQUE").Style.BackColor = Color.LightBlue
                    dgvRow.Cells("RCREDITCARD").Style.BackColor = Color.LightBlue
                    If OnlineAccode <> "" Then
                        dgvRow.Cells("RONLINE").Style.BackColor = Color.LightBlue
                    End If
                    dgvRow.Cells("RSAVINGS").Style.BackColor = Color.LightBlue
                    dgvRow.Cells("RGIFTVOUCHER").Style.BackColor = Color.LightBlue
                    dgvRow.Cells("RCREDIT").Style.BackColor = Color.LightBlue
                    dgvRow.Cells("RADVANCEADJ").Style.BackColor = Color.LightBlue
                    dgvRow.Cells("ICASH").Style.BackColor = Color.LightYellow
                    dgvRow.Cells("ICHEQUE").Style.BackColor = Color.LightYellow
                    dgvRow.Cells("ICREDITCARD").Style.BackColor = Color.LightYellow
                    If OnlineAccode <> "" Then
                        dgvRow.Cells("IONLINE").Style.BackColor = Color.LightYellow
                    End If
                    dgvRow.Cells("ISAVINGS").Style.BackColor = Color.LightYellow
                    dgvRow.Cells("IGIFTVOUCHER").Style.BackColor = Color.LightYellow
                    dgvRow.Cells("ICREDIT").Style.BackColor = Color.LightYellow
                    dgvRow.Cells("IADVANCEADJ").Style.BackColor = Color.LightYellow
            End Select
        Next
    End Function
    Function funcGridHead() As Integer
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("TRANNO~TRANDATE~TRANTYPE", GetType(String))
            If OnlineAccode <> "" Then
                .Columns.Add("RCASH~RCHEQUE~RCREDITCARD~RONLINE~RSAVINGS~RGIFTVOUCHER~RCREDIT~RADVANCEADJ", GetType(String))
                .Columns.Add("ICASH~ICHEQUE~ICREDITCARD~IONLINE~ISAVINGS~IGIFTVOUCHER~ICREDIT~IADVANCEADJ", GetType(String))
            Else
                .Columns.Add("RCASH~RCHEQUE~RCREDITCARD~RSAVINGS~RGIFTVOUCHER~RCREDIT~RADVANCEADJ", GetType(String))
                .Columns.Add("ICASH~ICHEQUE~ICREDITCARD~ISAVINGS~IGIFTVOUCHER~ICREDIT~IADVANCEADJ", GetType(String))
            End If
            .Columns.Add("TOTAL", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("TRANNO~TRANDATE~TRANTYPE").Caption = "BILLNO"
            If OnlineAccode <> "" Then
                .Columns("RCASH~RCHEQUE~RCREDITCARD~RONLINE~RSAVINGS~RGIFTVOUCHER~RCREDIT~RADVANCEADJ").Caption = "RECEIPTS"
                .Columns("ICASH~ICHEQUE~ICREDITCARD~IONLINE~ISAVINGS~IGIFTVOUCHER~ICREDIT~IADVANCEADJ").Caption = "PAYMENTS"
            Else
                .Columns("RCASH~RCHEQUE~RCREDITCARD~RSAVINGS~RGIFTVOUCHER~RCREDIT~RADVANCEADJ").Caption = "RECEIPTS"
                .Columns("ICASH~ICHEQUE~ICREDITCARD~ISAVINGS~IGIFTVOUCHER~ICREDIT~IADVANCEADJ").Caption = "PAYMENTS"
            End If
            .Columns("TOTAL").Caption = "REC-PAY"
            .Columns("SCROLL").Caption = ""
        End With
        gridHead1.DataSource = dtMergeHeader

        'strSql = "SELECT 'RECEIPTS' RECEIPTS, 'PAYMENTS' PAYMENTS"
        'dt = New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dt)
        'gridHead1.DataSource = dt
        With gridView
            gridHead1.Columns("TRANNO~TRANDATE~TRANTYPE").Width = .Columns("TRANNO").Width + .Columns("TRANTYPE").Width + .Columns("TRANDATE").Width
            If OnlineAccode <> "" Then
                gridHead1.Columns("RCASH~RCHEQUE~RCREDITCARD~RONLINE~RSAVINGS~RGIFTVOUCHER~RCREDIT~RADVANCEADJ").Width = .Columns("RCASH").Width + .Columns("RCHEQUE").Width _
                    + .Columns("RCREDITCARD").Width + .Columns("RSAVINGS").Width + .Columns("RGIFTVOUCHER").Width + .Columns("RCREDIT").Width + .Columns("RADVANCEADJ").Width _
                    + .Columns("RONLINE").Width
                gridHead1.Columns("ICASH~ICHEQUE~ICREDITCARD~IONLINE~ISAVINGS~IGIFTVOUCHER~ICREDIT~IADVANCEADJ").Width = .Columns("ICASH").Width + .Columns("ICHEQUE").Width _
                    + .Columns("ICREDITCARD").Width + .Columns("ISAVINGS").Width + .Columns("IGIFTVOUCHER").Width + .Columns("ICREDIT").Width + .Columns("IADVANCEADJ").Width _
                    + .Columns("IONLINE").Width
            Else
                gridHead1.Columns("RCASH~RCHEQUE~RCREDITCARD~RSAVINGS~RGIFTVOUCHER~RCREDIT~RADVANCEADJ").Width = .Columns("RCASH").Width + .Columns("RCHEQUE").Width + .Columns("RCREDITCARD").Width + .Columns("RSAVINGS").Width + .Columns("RGIFTVOUCHER").Width + .Columns("RCREDIT").Width + .Columns("RADVANCEADJ").Width
                gridHead1.Columns("ICASH~ICHEQUE~ICREDITCARD~ISAVINGS~IGIFTVOUCHER~ICREDIT~IADVANCEADJ").Width = .Columns("ICASH").Width + .Columns("ICHEQUE").Width + .Columns("ICREDITCARD").Width + .Columns("ISAVINGS").Width + .Columns("IGIFTVOUCHER").Width + .Columns("ICREDIT").Width + .Columns("IADVANCEADJ").Width
            End If

            gridHead1.Columns("TOTAL").Width = .Columns("TOTAL").Width
            If OnlineAccode <> "" Then
                gridHead1.Columns("ICASH~ICHEQUE~ICREDITCARD~IONLINE~ISAVINGS~IGIFTVOUCHER~ICREDIT~IADVANCEADJ").HeaderText = "PAYMENTS"
                gridHead1.Columns("RCASH~RCHEQUE~RCREDITCARD~RONLINE~RSAVINGS~RGIFTVOUCHER~RCREDIT~RADVANCEADJ").HeaderText = "RECEIPTS"
            Else
                gridHead1.Columns("ICASH~ICHEQUE~ICREDITCARD~ISAVINGS~IGIFTVOUCHER~ICREDIT~IADVANCEADJ").HeaderText = "PAYMENTS"
                gridHead1.Columns("RCASH~RCHEQUE~RCREDITCARD~RSAVINGS~RGIFTVOUCHER~RCREDIT~RADVANCEADJ").HeaderText = "RECEIPTS"
            End If
            gridHead1.Columns("TRANNO~TRANDATE~TRANTYPE").HeaderText = ""
            gridHead1.Columns("TOTAL").HeaderText = ""
            gridHead1.ColumnHeadersDefaultCellStyle = .ColumnHeadersDefaultCellStyle
            pnlHeading.Visible = True
        End With
        Dim colWid As Integer = 0
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
        Next
        With gridHead1
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With
    End Function

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            strSql = " SELECT"
            strSql += " ''TRANNO,''TRANTYPE"
            If OnlineAccode <> "" Then
                strSql += " ,'RECEIPTS'RCASH,''RCHEQUE,''RCREDITCARD,''RONLINE,''RSAVINGS,''RGIFTVOUCHER,''RCREDIT,''RADVANCEADJ"
                strSql += " ,'PAYMENTS'ICASH,''ICHEQUE,''ICREDITCARD,''IONLINE,''ISAVINGS,''IGIFTVOUCHER,''ICREDIT,''IADVANCEADJ"
            Else
                strSql += " ,'RECEIPTS'RCASH,''RCHEQUE,''RCREDITCARD,''RSAVINGS,''RGIFTVOUCHER,''RCREDIT,''RADVANCEADJ"
                strSql += " ,'PAYMENTS'ICASH,''ICHEQUE,''ICREDITCARD,''ISAVINGS,''IGIFTVOUCHER,''ICREDIT,''IADVANCEADJ"
            End If
            strSql += " ,''TOTAL"
            Dim dtHead As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtHead)
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub frmBillWiseReceiptPayment_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmBillWiseReceiptPayment_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ProcAddCostCentre()
        ProcAddCashCounter()
        ProcAddNodeid()

        btnNew_Click(Me, New EventArgs)
        'Me.WindowState = FormWindowState.Maximized
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        dtpDate.Focus()
    End Sub

    Private Sub ProcAddCostCentre()
        strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        Dim strCC As Char = UCase(objGPack.GetSqlValue(strSql, , "N"))
        If strCC = "Y" Then
            strSql = "SELECT COSTID,COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTID "

            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            If dtCostCentre.Rows.Count > 0 Then
                chkLstCostCentre.Items.Add("ALL", IIf(cnDefaultCostId, True, False))
                For CNT As Integer = 0 To dtCostCentre.Rows.Count - 1
                    If cnCostName = dtCostCentre.Rows(CNT).Item(1).ToString And cnDefaultCostId = False Then
                        chkLstCostCentre.Items.Add(dtCostCentre.Rows(CNT).Item(1).ToString, True)
                    Else
                        chkLstCostCentre.Items.Add(dtCostCentre.Rows(CNT).Item(1).ToString, False)
                    End If
                Next
                If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
            Else
                chkLstCostCentre.Items.Clear()
                chkLstCostCentre.Enabled = False
            End If
        Else
            chkLstCostCentre.Items.Clear()
            chkLstCostCentre.Enabled = False
        End If
    End Sub

    Private Sub ProcAddCashCounter()
        strSql = "SELECT CASHID,CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER ORDER BY CASHID "
        dtCashCounter = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCashCounter)
        If dtCashCounter.Rows.Count > 0 Then
            chkLstCashCounter.Items.Add("ALL", True)
            For CNT As Integer = 0 To dtCashCounter.Rows.Count - 1
                chkLstCashCounter.Items.Add(dtCashCounter.Rows(CNT).Item(1).ToString)
            Next
        Else
            chkLstCashCounter.Items.Clear()
            chkLstCashCounter.Enabled = False
        End If
    End Sub

    Private Sub ProcAddNodeId()
        strSql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN WHERE COMPANYID = '" & strCompanyId & "'"
        strSql += " UNION"
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ISSUE WHERE COMPANYID = '" & strCompanyId & "'"
        strSql += " UNION"
        strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..RECEIPT WHERE COMPANYID = '" & strCompanyId & "'"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            chkLstNodeId.Items.Add("ALL", True)
            For CNT As Integer = 0 To dt.Rows.Count - 1
                chkLstNodeId.Items.Add(dt.Rows(CNT).Item(0).ToString)
            Next
        Else
            chkLstNodeId.Items.Clear()
            chkLstNodeId.Enabled = False
        End If
    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    Select Case gridView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''        Case "S"
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''        Case "C"
    ''            gridView.Rows(e.RowIndex).Cells("RCASH").Style.BackColor = Color.LightBlue
    ''            gridView.Rows(e.RowIndex).Cells("RCHEQUE").Style.BackColor = Color.LightBlue
    ''            gridView.Rows(e.RowIndex).Cells("RCREDITCARD").Style.BackColor = Color.LightBlue
    ''            gridView.Rows(e.RowIndex).Cells("RSAVINGS").Style.BackColor = Color.LightBlue
    ''            gridView.Rows(e.RowIndex).Cells("RGIFTVOUCHER").Style.BackColor = Color.LightBlue
    ''            gridView.Rows(e.RowIndex).Cells("ICASH").Style.BackColor = Color.LightYellow
    ''            gridView.Rows(e.RowIndex).Cells("ICHEQUE").Style.BackColor = Color.LightYellow
    ''            gridView.Rows(e.RowIndex).Cells("ICREDITCARD").Style.BackColor = Color.LightYellow
    ''            gridView.Rows(e.RowIndex).Cells("ISAVINGS").Style.BackColor = Color.LightYellow
    ''            gridView.Rows(e.RowIndex).Cells("IGIFTVOUCHER").Style.BackColor = Color.LightYellow
    ''    End Select

    ''End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridHead1)
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.gridView.BorderStyle = BorderStyle.None
        Me.gridHead1.BorderStyle = BorderStyle.None
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpDate.Focus()
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmBillWiseReceiptPayment_Properties
        GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        GetChecked_CheckedList(chkLstCashCounter, obj.p_chkLstCashCounter)
        GetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId)
        SetSettingsObj(obj, Me.Name, GetType(frmBillWiseReceiptPayment_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmBillWiseReceiptPayment_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmBillWiseReceiptPayment_Properties))
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        SetChecked_CheckedList(chkLstCashCounter, obj.p_chkLstCashCounter, "ALL")
        SetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId, "ALL")
    End Sub

    Private Sub gridView_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If chkdatewise.Checked Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridHead1.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridHead1.HorizontalScrollingOffset = e.NewValue
                gridHead1.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridHead1.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                gridHead1.Columns("SCROLL").Visible = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

End Class

Public Class frmBillWiseReceiptPayment_Properties
    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
    Private chkLstCashCounter As New List(Of String)
    Public Property p_chkLstCashCounter() As List(Of String)
        Get
            Return chkLstCashCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstCashCounter = value
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
