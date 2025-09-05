Imports System.Data.OleDb
Public Class frmEstSales
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtCounter As New DataTable
    Dim chk As CheckBox

    Private Sub frmEstSales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmEstSales_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Me.WindowState = FormWindowState.Maximized
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        txtItemId_NUM.Clear()
        lblItemName.Text = ""
        gridView.DataSource = Nothing
        'cmbCounter.Items.Clear()
        'cmbCounter.Items.Add("ALL")
        'objGPack.FillCombo("SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y'" _
        ', cmbCounter, False)
        'cmbCounter.Text = "ALL"
        strSql = " SELECT 'ALL' ITEMCTRNAME,'ALL' ITEMCTRID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMCTRNAME,CONVERT(vARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y'"
        strSql += " ORDER BY RESULT,ITEMCTRNAME"
        dtCounter = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCounter)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCounter, dtCounter, "ITEMCTRNAME", , "ALL")

        rbtBoth.Checked = True
        rdbSales.Checked = True
        chkSummary.Checked = False
        dtpFrom.Select()
    End Sub

    Function funcTitle() As Integer
        lblTitle.Text = ""
        SelectAll.Checked = False
        lblTitle.Text += "SALES ESTIMATION " + IIf(chkSummary.Checked, " SUMMARY ", "") + " DETAILS"
        lblTitle.Text += " FROM " + dtpFrom.Value.ToString("dd/MM/yyyy") + " TO " + dtpTo.Value.ToString("dd/MM/yyyy")
        If rbtBilled.Checked Then
            lblTitle.Text += "(ONLY BILLED)"
        ElseIf rbtPending.Checked Then
            lblTitle.Text += "(ONLY PENDING)"
        End If
        lblTitle.Refresh()
    End Function

    Private Sub WithOutSummary()

        strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ESTIMATE')>0"
        strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ESTIMATE "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        Dim dtGridView As New DataTable
        Dim dtCol As DataColumn
        dtCol = New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = False
        dtGridView.Columns.Add(dtCol)

        Me.Refresh()

        If rdbSales.Checked Then
            strSql = vbCrLf + " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ESTIMATESA')>0"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ESTIMATESA "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT "
            If chkEstWiseSummary.Checked Then
                strSql += vbCrLf + " ROW_NUMBER() OVER (ORDER BY CTRGROUP,ITEMCTRNAME,TRANDATE,TRANNO)SERNO,"
            End If
            strSql += vbCrLf + " ' '+IC.ITEMCTRNAME AS PARTICULAR,SNO,ISNULL(IGM.GROUPNAME,'') AS CTRGROUP ,IC.ITEMCTRNAME,EMPID,TRANNO,TRANDATE"
            If chkEstWiseSummary.Checked Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL(SI.SUBITEMNAME,'') <>'' THEN ISNULL(SI.SUBITEMNAME,'') ELSE "
                strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST X WHERE X.ITEMID = I.ITEMID) END AS ITEMNAME"
            Else
                strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST X WHERE X.ITEMID = I.ITEMID) + ' ['+ CONVERT(VARCHAR,I.ITEMID) + ']'+ISNULL(SI.SUBITEMNAME,'') AS ITEMNAME"
            End If

            strSql += vbCrLf + " ,TAGNO,I.PCS,I.GRSWT,I.NETWT"
            strSql += vbCrLf + " ,CASE WHEN WASTAGE <> 0 THEN WASTAGE ELSE NULL END AS WASTAGE"
            If chkEstWiseSummary.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15),CASE WHEN MCHARGE <> 0 THEN MCHARGE ELSE NULL END) AS MCHARGE"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15),CASE WHEN ISNULL(MISCAMT,0) <> 0 THEN MISCAMT ELSE NULL END) AS OTHERAMT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15),CASE WHEN ISNULL(I.GRSWT,0) <> 0 AND ISNULL(I.RATE,0) <> 0 THEN (I.GRSWT*I.RATE) ELSE NULL END) AS METALAMT"
            Else
                strSql += vbCrLf + " ,CASE WHEN MCHARGE <> 0 THEN MCHARGE ELSE NULL END AS MCHARGE"
                strSql += vbCrLf + " ,CASE WHEN ISNULL(MISCAMT,0) <> 0 THEN MISCAMT ELSE NULL END AS OTHERAMT"
            End If
            'DIAMOND
            strSql += vbCrLf + ",(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ESTISSSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'  ) ) DIAPCS "
            If chkEstWiseSummary.Checked Then
                strSql += vbCrLf + ",CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ESTISSSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT "
                strSql += vbCrLf + ",CONVERT(NUMERIC(15),(SELECT SUM(STNAMT) FROM " & cnStockDb & "..ESTISSSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'  ))) DIAAMT "
            Else
                strSql += vbCrLf + ",(SELECT SUM(STNWT) FROM " & cnStockDb & "..ESTISSSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'  )) DIAWT "
                strSql += vbCrLf + ",(SELECT SUM(STNAMT) FROM " & cnStockDb & "..ESTISSSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'  )) DIAAMT "
            End If
            'STONE
            strSql += vbCrLf + ",(SELECT SUM(STNPCS ) FROM " & cnStockDb & "..ESTISSSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'  )) STNPCS "
            If chkEstWiseSummary.Checked Then
                strSql += vbCrLf + ",CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ESTISSSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'  ))) STNWT "
                strSql += vbCrLf + ",CONVERT(NUMERIC(15),(SELECT SUM(STNAMT) FROM " & cnStockDb & "..ESTISSSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'  ))) STNAMT "
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15),RATE)RATE"
            Else
                strSql += vbCrLf + ",(SELECT SUM(STNWT) FROM " & cnStockDb & "..ESTISSSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'  ) ) STNWT "
                strSql += vbCrLf + ",(SELECT SUM(STNAMT) FROM " & cnStockDb & "..ESTISSSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'  ) ) STNAMT "
                strSql += vbCrLf + " ,RATE"
            End If
            strSql += vbCrLf + " ,AMOUNT,TAX "
            If ChkWithPaymode.Checked Then
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)CASH,CONVERT(NUMERIC(15,2),NULL)CRCARD"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)CHEQUE,CONVERT(NUMERIC(15,2),NULL)PURCHASE,CONVERT(NUMERIC(15,2),NULL)SRRETURN"
            End If
            strSql += vbCrLf + " ,TRANDATE TTRANDATE,ESTBATCHNO,COSTID,TRANTYPE,1 RESULT"
            strSql += vbCrLf + " ,(CASE WHEN ISNULL(BATCHNO,'')<>'' THEN 'BILLED' ELSE 'PENDING' END)STATUS "
            strSql += vbCrLf + " ,RIGHT(CONVERT(VARCHAR,I.UPTIME,22),12)UPTIME"
            strSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=I.ITEMTYPEID)ITEMTYPE"
            strSql += vbCrLf + " ,ROW_NUMBER() OVER (PARTITION BY ESTBATCHNO ORDER BY CTRGROUP,ITEMCTRNAME,TRANDATE,TRANNO) UPDSNO "
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ESTIMATESA"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ESTISSUE AS I"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SI ON I.SUBITEMID =SI.SUBITEMID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER AS IC ON I.ITEMCTRID =IC.ITEMCTRID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMGROUPMAST AS IGM ON IC.CTRGROUP = IGM.GROUPID and IGM.GROUPTYPE='C'"
            strSql += Condition()
            strSql += vbCrLf + " ORDER BY CTRGROUP,ITEMCTRNAME,TRANDATE,TRANNO"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            If ChkWithPaymode.Checked Then
                strSql = vbCrLf + " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ESTIMATEACC')>0"
                strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ESTIMATEACC "
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()

                strSql = vbCrLf + " SELECT ESTBATCHNO,SUM(ISNULL(CASH,0))CASH,SUM(ISNULL(CRCARD,0))CRCARD,SUM(ISNULL(CHEQUE,0))CHEQUE,SUM(ISNULL(PURCHASE,0))PURCHASE "
                strSql += vbCrLf + " ,SUM(ISNULL(SRRETURN,0))SRRETURN INTO TEMPTABLEDB..TEMP" & systemId & "ESTIMATEACC "
                strSql += vbCrLf + " FROM ( "
                strSql += vbCrLf + " SELECT A.ESTBATCHNO "
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(ISNULL(AMOUNT,0)) FROM " & cnStockDb & "..ESTACCTRAN WHERE ESTBATCHNO =A.ESTBATCHNO AND PAYMODE='CA'),NULL) CASH "
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(ISNULL(AMOUNT,0)) FROM " & cnStockDb & "..ESTACCTRAN WHERE ESTBATCHNO =A.ESTBATCHNO AND PAYMODE='CC'),NULL) CRCARD "
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(ISNULL(AMOUNT,0)) FROM " & cnStockDb & "..ESTACCTRAN WHERE ESTBATCHNO =A.ESTBATCHNO AND PAYMODE='CH'),NULL) CHEQUE "
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(ISNULL(AMOUNT,0)+ISNULL(TAX,0)) FROM " & cnStockDb & "..ESTRECEIPT WHERE ESTBATCHNO =A.ESTBATCHNO AND TRANTYPE='PU' ),NULL) PURCHASE "
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(ISNULL(AMOUNT,0)+ISNULL(TAX,0)) FROM " & cnStockDb & "..ESTRECEIPT WHERE ESTBATCHNO =A.ESTBATCHNO AND TRANTYPE='SR' ),NULL) SRRETURN "
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ESTIMATESA A "
                strSql += vbCrLf + " GROUP BY A.ESTBATCHNO) X GROUP BY X.ESTBATCHNO"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()

                strSql = vbCrLf + " UPDATE A SET A.CASH=B.CASH,A.CRCARD=B.CRCARD,A.CHEQUE=B.CHEQUE,A.PURCHASE=B.PURCHASE,A.SRRETURN=B.SRRETURN "
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ESTIMATESA A,TEMPTABLEDB..TEMP" & systemId & "ESTIMATEACC B WHERE A.ESTBATCHNO =B.ESTBATCHNO "
                strSql += vbCrLf + " AND A.UPDSNO=1 "
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            End If
            strSql = vbCrLf + " SELECT * INTO TEMPTABLEDB..TEMP" & systemId & "ESTIMATE FROM TEMPTABLEDB..TEMP" & systemId & "ESTIMATESA "

        ElseIf rdbSaReturn.Checked Then
            strSql = vbCrLf + " SELECT ' '+IC.ITEMCTRNAME AS PARTICULAR,SNO,ISNULL(IGM.GROUPNAME,'') AS CTRGROUP ,IC.ITEMCTRNAME,EMPID,TRANNO,TRANDATE"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) + ' ['+ CONVERT(VARCHAR,ITEMID) + ']' AS ITEMNAME"
            strSql += vbCrLf + " ,TAGNO,I.PCS,GRSWT,NETWT"
            strSql += vbCrLf + " ,CASE WHEN WASTAGE <> 0 THEN WASTAGE ELSE NULL END AS WASTAGE"
            strSql += vbCrLf + " ,CASE WHEN MCHARGE <> 0 THEN MCHARGE ELSE NULL END AS MCHARGE"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(MISCAMT,0) <> 0 THEN MISCAMT ELSE NULL END AS OTHERAMT"
            strSql += vbCrLf + ",(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ESTRECEIPTSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'  ) ) DIAPCS "
            strSql += vbCrLf + ",(SELECT SUM(STNWT) FROM " & cnStockDb & "..ESTRECEIPTSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'  )) DIAWT "
            strSql += vbCrLf + ",(SELECT SUM(STNAMT) FROM " & cnStockDb & "..ESTRECEIPTSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'  )) DIAAMT "
            strSql += vbCrLf + ",(SELECT SUM(STNPCS ) FROM " & cnStockDb & "..ESTRECEIPTSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'  ) ) STNPCS "
            strSql += vbCrLf + ",(SELECT SUM(STNWT) FROM " & cnStockDb & "..ESTRECEIPTSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'  ) ) STNWT "
            strSql += vbCrLf + ",(SELECT SUM(STNAMT) FROM " & cnStockDb & "..ESTRECEIPTSTONE WHERE ISSSNO = I.SNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'  ) ) STNAMT "
            strSql += vbCrLf + " ,RATE,AMOUNT,TRANDATE TTRANDATE,ESTBATCHNO,COSTID,TRANTYPE,1 RESULT"
            strSql += vbCrLf + " ,(CASE WHEN ISNULL(BATCHNO,'')<>'' THEN 'BILLED' ELSE 'PENDING' END)STATUS "
            strSql += vbCrLf + " ,RIGHT(CONVERT(VARCHAR,I.UPTIME,22),12)UPTIME"
            strSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=I.ITEMTYPEID)ITEMTYPE"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ESTIMATE"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ESTRECEIPT AS I"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER AS IC ON I.ITEMCTRID =IC.ITEMCTRID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMGROUPMAST AS IGM ON IC.CTRGROUP = IGM.GROUPID and IGM.GROUPTYPE='C'"
            strSql += Condition()
            strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') = 'SR'"
            strSql += vbCrLf + " ORDER BY CTRGROUP,ITEMCTRNAME,TRANDATE,TRANNO"
        ElseIf rdbPurchase.Checked Then
            strSql = vbCrLf + " SELECT SNO,EMPID,TRANNO,TRANDATE"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) + ' ['+ CONVERT(VARCHAR,ITEMID) + ']' AS ITEMNAME"
            strSql += vbCrLf + " ,TAGNO,I.PCS,GRSWT,NETWT"
            strSql += vbCrLf + " ,CASE WHEN WASTAGE <> 0 THEN WASTAGE ELSE NULL END AS WASTAGE"
            strSql += vbCrLf + " ,CASE WHEN MCHARGE <> 0 THEN MCHARGE ELSE NULL END AS MCHARGE"
            strSql += vbCrLf + " ,RATE,AMOUNT,TRANDATE TTRANDATE,ESTBATCHNO,COSTID,TRANTYPE"
            strSql += vbCrLf + " ,(CASE WHEN ISNULL(BATCHNO,'')<>'' THEN 'BILLED' ELSE 'PENDING' END)STATUS "
            strSql += vbCrLf + " ,RIGHT(CONVERT(VARCHAR,I.UPTIME,22),12)UPTIME"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ESTIMATE"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ESTRECEIPT AS I"
            strSql += Condition()
            strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') = 'PU'"
            strSql += vbCrLf + " ORDER BY TRANDATE,TRANNO"
        ElseIf rdbOrder.Checked Then
            strSql = vbCrLf + " SELECT SNO,EMPID,ORNO AS TRANNO,ORDATE AS TRANDATE"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) + ' ['+ CONVERT(VARCHAR,ITEMID) + ']' AS ITEMNAME"
            strSql += vbCrLf + " ,'' TAGNO,I.PCS,GRSWT,NETWT"
            strSql += vbCrLf + " ,CASE WHEN WAST <> 0 THEN WAST ELSE NULL END AS WASTAGE"
            strSql += vbCrLf + " ,CASE WHEN MC <> 0 THEN MC ELSE NULL END AS MCHARGE"
            strSql += vbCrLf + " ,RATE,ORVALUE AS AMOUNT,ORDATE TTRANDATE,BATCHNO AS ESTBATCHNO,COSTID,ORTYPE TRANTYPE"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ESTIMATE"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ESTORMAST AS I"
            strSql += OrderCondition()
            strSql += vbCrLf + " AND ISNULL(ORTYPE,'') = 'O'"
            strSql += vbCrLf + " ORDER BY ORDATE,ORNO"
        End If

        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        If rdbSales.Checked Or rdbSaReturn.Checked Then
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ESTIMATE(PARTICULAR,SNO,CTRGROUP,RESULT,STATUS)"
            strSql += vbCrLf + " SELECT DISTINCT CTRGROUP AS PARTICULAR,''SNO,CTRGROUP,0,'' RESULT FROM TEMPTABLEDB..TEMP" & systemId & "ESTIMATE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            If chkEstWiseSummary.Checked Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ESTIMATE(PARTICULAR,SNO,CTRGROUP,TRANNO,TRANDATE,PCS,GRSWT,NETWT"
                strSql += vbCrLf + " ,WASTAGE,MCHARGE,OTHERAMT,METALAMT,DIAPCS,DIAWT,DIAAMT,STNPCS,STNWT,STNAMT,AMOUNT,RESULT,[STATUS])"
                strSql += vbCrLf + " SELECT 'SUB TOTAL' AS PARTICULAR,'' AS SNO,'' AS CTRGROUP,TRANNO,TRANDATE "
                strSql += vbCrLf + " ,SUM(PCS) AS PCS,SUM(GRSWT) AS GRSWT,SUM(NETWT) AS NETWT,SUM(WASTAGE) AS WASTAGE,SUM(MCHARGE) AS MCHARGE,SUM(OTHERAMT) AS OTHERAMT "
                If chkEstWiseSummary.Checked Then
                    strSql += vbCrLf + " ,SUM(METALAMT) AS METALAMT "
                End If
                strSql += vbCrLf + " ,SUM(DIAPCS) AS DIAPCS,SUM(DIAWT) AS DIAWT,SUM(DIAAMT) AS DIAAMT "
                strSql += vbCrLf + " ,SUM(STNPCS) AS STNPCS,SUM(STNWT) AS STNWT,SUM(STNAMT) AS STNAMT,SUM(AMOUNT) AS AMOUNT,2 RESULT,'' [STATUS] "
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ESTIMATE WHERE RESULT=1 "
                strSql += vbCrLf + " GROUP BY TRANNO,TRANDATE"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            End If

        End If
        If rdbSales.Checked Or rdbSaReturn.Checked Then
            strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "ESTIMATE ORDER BY CTRGROUP,TRANDATE,TRANNO,RESULT "
        Else
            strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "ESTIMATE ORDER BY TRANDATE,TRANNO"
        End If

        da = New OleDbDataAdapter(strSql, cn)
        'Dim dtRow As DataRow = Nothing
        'For Each dtRo As DataRow In dtGridView.Rows
        '    dtRo = dtGridView.NewRow
        '    dtRo.Item("PARTICULAR") = " "
        '    dtRo.Item("SNO") = " "
        '    dtRo.Item("EMPID") = " "
        '    dtRo.Item("TRANNO") = " "
        '    dtRo.Item("ITEMNAME") = " "
        '    dtRo.Item("TAGNO") = " "
        '    dtRo.Item("PCS") = " "
        '    dtRo.Item("GRSWT") = " "
        '    dtRo.Item("NETWT") = " "
        '    dtRo.Item("WASTAGE") = " "
        '    dtRo.Item("MC") = " "
        '    dtRo.Item("RATE") = " "
        '    dtRo.Item("AMOUNT") = " "
        'Next
        'dtGridView.Rows.Add(dtRow)
        'dtGridView.AcceptChanges()

        da.Fill(dtGridView)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            btnView_Search.Enabled = True
            dtpFrom.Focus()
            Exit Sub
        End If
        gridView.DataSource = dtGridView
        If rdbSales.Checked Or rdbSaReturn.Checked Then
            gridView.Columns("CTRGROUP").Visible = False
            gridView.Columns("ITEMCTRNAME").Visible = False
            gridView.Columns("RESULT").Visible = False
        End If
        gridView.Columns("CHECK").ReadOnly = False
        If gridView.Columns.Contains("UPDSNO") Then gridView.Columns("UPDSNO").Visible = False

        StyleGrid()
        btnView_Search.Enabled = True
        funcTitle()
        If gridView.Rows.Count > 0 Then tabMain.SelectedTab = tabView
        If chkSummary.Checked = False And chkEstWiseSummary.Checked Then
            For cnt As Integer = 0 To gridView.Rows.Count - 1
                If Val(gridView.Rows(cnt).Cells("RESULT").Value.ToString) = 2 Then
                    gridView.Rows(cnt).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    gridView.Rows(cnt).DefaultCellStyle.BackColor = Color.Lavender
                End If
            Next
        End If
        gridView.Focus()
    End Sub

    Private Sub WithSummary()


        Dim dtGrid As New DataTable
        Dim dtCol As DataColumn
        dtCol = New DataColumn("CHECK", GetType(Boolean))
        dtCol.DefaultValue = False
        dtGrid.Columns.Add(dtCol)

        Me.Refresh()
        If rdbOrder.Checked = False Then
            strSql = " SELECT EMPID,TRANNO, BILLNO, [STATUS],TRANDATE,PCS,GRSWT,NETWT"
            strSql += " ,CASE WHEN WASTAGE <> 0 THEN WASTAGE ELSE NULL END AS WASTAGE"
            strSql += " ,CASE WHEN MCHARGE <> 0 THEN MCHARGE ELSE NULL END AS MCHARGE,DIAPCS,DIAWT ,DIAAMT,STNPCS ,STNWT ,STNAMT "
            strSql += " ,RATE,AMOUNT,TRANDATE TTRANDATE,ESTBATCHNO,COSTID,TRANTYPE,PARTY AS CUSTOMER"
            strSql += " FROM"
            strSql += " ("
            strSql += " SELECT EMPID,TRANNO"
            strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT"
            strSql += " ,SUM(ISNULL(WASTAGE,0)) AS WASTAGE"
            strSql += " ,SUM(ISNULL(MCHARGE,0)) AS MCHARGE"
            If rdbSales.Checked Then
                strSql += vbCrLf + ",(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ESTISSSTONE WHERE ESTBATCHNO = I.ESTBATCHNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'  ) ) DIAPCS "
                strSql += vbCrLf + ",(SELECT SUM(STNWT) FROM " & cnStockDb & "..ESTISSSTONE WHERE ESTBATCHNO = I.ESTBATCHNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'  )) DIAWT "
                strSql += vbCrLf + ",(SELECT SUM(STNAMT) FROM " & cnStockDb & "..ESTISSSTONE WHERE ESTBATCHNO = I.ESTBATCHNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'  )) DIAAMT "
                strSql += vbCrLf + ",(SELECT SUM(STNPCS ) FROM " & cnStockDb & "..ESTISSSTONE WHERE ESTBATCHNO = I.ESTBATCHNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'  ) ) STNPCS "
                strSql += vbCrLf + ",(SELECT SUM(STNWT) FROM " & cnStockDb & "..ESTISSSTONE WHERE ESTBATCHNO = I.ESTBATCHNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'  ) ) STNWT "
                strSql += vbCrLf + ",(SELECT SUM(STNAMT) FROM " & cnStockDb & "..ESTISSSTONE WHERE ESTBATCHNO = I.ESTBATCHNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'  ) ) STNAMT "
            Else
                strSql += vbCrLf + ",(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ESTRECEIPTSTONE WHERE ESTBATCHNO = I.ESTBATCHNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'  ) ) DIAPCS "
                strSql += vbCrLf + ",(SELECT SUM(STNWT) FROM " & cnStockDb & "..ESTRECEIPTSTONE WHERE ESTBATCHNO = I.ESTBATCHNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'  )) DIAWT "
                strSql += vbCrLf + ",(SELECT SUM(STNAMT) FROM " & cnStockDb & "..ESTRECEIPTSTONE WHERE ESTBATCHNO = I.ESTBATCHNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'  )) DIAAMT "
                strSql += vbCrLf + ",(SELECT SUM(STNPCS ) FROM " & cnStockDb & "..ESTRECEIPTSTONE WHERE ESTBATCHNO = I.ESTBATCHNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'  ) ) STNPCS "
                strSql += vbCrLf + ",(SELECT SUM(STNWT) FROM " & cnStockDb & "..ESTRECEIPTSTONE WHERE ESTBATCHNO = I.ESTBATCHNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'  ) ) STNWT "
                strSql += vbCrLf + ",(SELECT SUM(STNAMT) FROM " & cnStockDb & "..ESTRECEIPTSTONE WHERE ESTBATCHNO = I.ESTBATCHNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'  ) ) STNAMT "
            End If
            strSql += " ,RATE,SUM(AMOUNT)AMOUNT,TRANDATE,ESTBATCHNO,COSTID,TRANTYPE,"
            strSql += " (SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = PSNO) AS PARTY ,"
            strSql += " (SELECT TOP 1 TRANNO FROM " & cnStockDb & "..ISSUE WHERE BATCHNO IN (I.BATCHNO)) AS BILLNO,"
            strSql += " CASE WHEN BATCHNO = '' THEN 'PENDING' ELSE 'BILLED' END AS [STATUS]"
            If rdbSales.Checked Then
                strSql += " FROM " & cnStockDb & "..ESTISSUE AS I"
                strSql += Condition()
            Else
                strSql += " FROM " & cnStockDb & "..ESTRECEIPT AS I"
                strSql += Condition()
                strSql += " AND ISNULL(TRANTYPE,'') = 'SR'"
            End If
            strSql += " GROUP BY TRANNO,RATE,TRANDATE,COSTID,ESTBATCHNO,EMPID,TRANTYPE,PSNO,BATCHNO"
            strSql += " )X"
            strSql += " ORDER BY TRANDATE,TRANNO"
        Else
            strSql = " SELECT EMPID,TRANNO,TRANDATE,PCS,GRSWT,NETWT"
            strSql += " ,CASE WHEN WASTAGE <> 0 THEN WASTAGE ELSE NULL END AS WASTAGE"
            strSql += " ,CASE WHEN MCHARGE <> 0 THEN MCHARGE ELSE NULL END AS MCHARGE"
            strSql += " ,RATE,AMOUNT,TTRANDATE,ESTBATCHNO,COSTID,TRANTYPE"
            strSql += " FROM"
            strSql += " ("
            strSql += " SELECT EMPID,ORNO AS TRANNO,ORDATE AS TRANDATE"
            strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT"
            strSql += " ,SUM(ISNULL(WAST,0)) AS WASTAGE"
            strSql += " ,SUM(ISNULL(MC,0)) AS MCHARGE"
            strSql += " ,RATE,SUM(ORVALUE)AMOUNT,ORDATE AS TTRANDATE,BATCHNO ESTBATCHNO,COSTID,ORTYPE TRANTYPE"
            strSql += " FROM " & cnStockDb & "..ESTORMAST AS I"
            strSql += OrderCondition()
            strSql += " GROUP BY ORNO,RATE,ORDATE,COSTID,BATCHNO,EMPID,ORTYPE"
            strSql += " )X"
            strSql += " ORDER BY TTRANDATE,TRANNO"
        End If
        'Dim dtGridView As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            btnView_Search.Enabled = True
            dtpFrom.Focus()
            Exit Sub
        End If
        gridView.DataSource = dtGrid
        gridView.Columns("CHECK").ReadOnly = False
        StyleGrid()
        btnView_Search.Enabled = True
        funcTitle()
        If gridView.Rows.Count > 0 Then tabMain.SelectedTab = tabView
        gridView.Focus()
    End Sub

    Private Sub StyleGrid()
        For Each dgvCol As DataGridViewColumn In gridView.Columns
            Select Case dgvCol.ValueType.FullName
                Case GetType(Decimal).FullName, GetType(Double).FullName, GetType(Int32).FullName,
                GetType(Int64).FullName, GetType(Integer).FullName, GetType(Int16).FullName
                    dgvCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                Case Else
                    dgvCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End Select
        Next
        With gridView
            .Columns("CHECK").Width = 60
            .Columns("EMPID").Width = 60
            .Columns("TRANNO").Width = 60
            .Columns("TTRANDATE").Visible = False
            .Columns("TTRANDATE").DefaultCellStyle.Format = "yyyy-MM-dd"
            If chkSummary.Checked = False Then
                .Columns("ITEMNAME").Width = 250
                .Columns("TAGNO").Width = 60
            End If
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 100
            .Columns("NETWT").Width = 100
            .Columns("WASTAGE").Width = 80
            .Columns("MCHARGE").Width = 80
            .Columns("MCHARGE").HeaderText = "MC"
            If .Columns.Contains("OTHERAMT") Then .Columns("OTHERAMT").HeaderText = "OTH AMT"
            If .Columns.Contains("METALAMT") Then .Columns("METALAMT").HeaderText = "MET AMT"
            .Columns("AMOUNT").Width = 100
            .Columns("ESTBATCHNO").Visible = False
            .Columns("COSTID").Visible = False
            .Columns("TRANTYPE").Visible = False
            If .Columns.Contains("UPTIME") Then .Columns("UPTIME").HeaderText = "TIME"
        End With
    End Sub

    Public Function GetSelectedCounderid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function

    Private Function Condition() As String
        Dim qry As String = Nothing
        qry += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        If Val(txtItemId_NUM.Text) > 0 Then
            qry += " AND I.ITEMID = " & Val(txtItemId_NUM.Text) & ""
        End If
        'If cmbCounter.Text <> "" And cmbCounter.Items.Contains(cmbCounter.Text) And cmbCounter.Text <> "ALL" Then
        '    qry += " AND ITEMCTRID = ISNULL((SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter.Text & "'),0)"
        'End If
        If chkCmbCounter.Text <> "" And chkCmbCounter.Text <> "ALL" Then
            qry += " AND I.ITEMCTRID IN(" & GetSelectedCounderid(chkCmbCounter, False) & ")"
        End If
        If rbtBilled.Checked Then
            qry += " AND ISNULL(BATCHNO,'') <> ''"
        ElseIf rbtPending.Checked Then
            qry += " AND ISNULL(BATCHNO,'') = ''"
        End If
        If txtEstNo_NUM.Text <> "" Then
            qry += " AND ISNULL(TRANNO,'') = '" & Val(txtEstNo_NUM.Text) & "'"
        End If
        qry += " AND ISNULL(CANCEL,'') <> 'Y'"
        'qry += " AND COMPANYID = '" & strCompanyId & "' "
        Return qry
    End Function

    Private Function OrderCondition() As String
        Dim qry As String = Nothing
        qry += " WHERE ORDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        If Val(txtItemId_NUM.Text) > 0 Then
            qry += " AND ITEMID = " & Val(txtItemId_NUM.Text) & ""
        End If
        If rbtBilled.Checked Then
            qry += " AND ISNULL(ODBATCHNO,'') <> ''"
        ElseIf rbtPending.Checked Then
            qry += " AND ISNULL(ODBATCHNO,'') = ''"
        End If
        If txtEstNo_NUM.Text <> "" Then
            qry += " AND ISNULL(ORNO,'') = '" & Val(txtEstNo_NUM.Text) & "'"
        End If
        qry += " AND ISNULL(CANCEL,'') <> 'Y'"
        Return qry
    End Function

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        btnView_Search.Enabled = False
        gridView.DataSource = Nothing
        Try
            If rdbOrder.Checked Then
                Label11.Text = "[D] -Duplicate"
            Else
                Label11.Text = "[D] -Duplicate     [E] - Edit"
            End If
            If chkSummary.Checked Then
                WithSummary()
            Else
                WithOutSummary()
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            btnView_Search.Enabled = True
        End Try
    End Sub

    Private Sub loadItemHelp()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME,"
        strSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGGED' "
        strSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGGED' ELSE 'POCKET BASED' END AS STOCK_TYPE,"
        strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
        strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
        strSql += " WHEN CALTYPE = 'B' THEN 'BOTH' ELSE 'METAL RATE' END AS CAL_TYPE"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y'"
        Dim row As DataRow = BrighttechPack.SearchDialog.Show_R("Find ItemId", strSql, cn)
        If Not row Is Nothing Then
            txtItemId_NUM.Text = row("ITEMID").ToString
            lblItemName.Text = row("ITEMNAME").ToString
            Me.SelectNextControl(txtItemId_NUM, True, True, True, True)
        End If
    End Sub

    Private Sub txtItemId_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemId_NUM.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemId_NUM.KeyDown
        If e.KeyCode = Keys.Insert Then
            loadItemHelp()
        End If
    End Sub

    Private Sub txtItemId_NUM_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemId_NUM.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtItemId_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemId_NUM.TextChanged
        lblItemName.Text = ""
    End Sub

    Private Sub NEWToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NEWToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub EXITToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EXITToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick
        For i As Integer = 0 To gridView.Rows.Count - 1
            If gridView.Rows(i).Cells("CHECK").Selected = True Then
                If gridView.Rows(i).Cells("CHECK").Value = False Then
                    gridView.Rows(i).Cells("CHECK").Value = True
                Else
                    gridView.Rows(i).Cells("CHECK").Value = False
                End If
            End If
        Next
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = Chr(Keys.P) Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "D" Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
            If gridView.CurrentRow Is Nothing Then Exit Sub
            If gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString = "" Then Exit Sub
            If gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString Is Nothing Then Exit Sub
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim write As IO.StreamWriter
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"
                Dim EstIssno As Integer
                Dim EstPurno As Integer
                If ChkdupPrit_SP.Checked = True Then
                    strSql = " SELECT TOP 1 TRANNO FROM " & cnStockDb & "..ESTISSUE WHERE ESTBATCHNO='" & gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString & "'"
                    strSql += " AND ISNULL(CANCEL,'') = ''"
                    EstIssno = Val(objGPack.GetSqlValue(strSql, , "", ).ToString)
                    strSql = " SELECT TOP 1 TRANNO FROM " & cnStockDb & "..ESTRECEIPT WHERE ESTBATCHNO='" & gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString & "'"
                    strSql += " AND ISNULL(CANCEL,'') = ''"
                    EstPurno = Val(objGPack.GetSqlValue(strSql, , "", ).ToString)
                End If
                write = IO.File.CreateText(Application.StartupPath & memfile)
                If gridView.CurrentRow.Cells("TRANTYPE").Value.ToString = "SA" Then
                    If ChkdupPrit_SP.Checked = False Then EstIssno = Val(gridView.CurrentRow.Cells("TRANNO").Value.ToString)
                    write.WriteLine(LSet("TYPE", 15) & ":EST")
                    write.WriteLine(LSet("ESTNO", 15) & ":S." & EstIssno.ToString & ";P." & EstPurno.ToString)
                ElseIf gridView.CurrentRow.Cells("TRANTYPE").Value.ToString = "O" Then
                    write.WriteLine(LSet("TYPE", 15) & ":ORD")
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString)
                Else
                    If ChkdupPrit_SP.Checked = False Then EstPurno = Val(gridView.CurrentRow.Cells("TRANNO").Value.ToString)
                    write.WriteLine(LSet("TYPE", 15) & ":EST")
                    write.WriteLine(LSet("ESTNO", 15) & ":S." & EstIssno.ToString & ";P." & EstPurno.ToString)
                End If
                write.WriteLine(LSet("TRANDATE", 15) & ":" & gridView.CurrentRow.Cells("TTRANDATE").FormattedValue.ToString)
                write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    If gridView.CurrentRow.Cells("TRANTYPE").Value.ToString = "SA" Then
                        If ChkdupPrit_SP.Checked = False Then EstIssno = Val(gridView.CurrentRow.Cells("TRANNO").Value.ToString)
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                        LSet("TYPE", 15) & ":EST" & ";" &
                        LSet("ESTNO", 15) & ":S." & EstIssno.ToString & ";P." & EstPurno.ToString & "" & ";" &
                        LSet("TRANDATE", 15) & ":" & gridView.CurrentRow.Cells("TTRANDATE").FormattedValue.ToString & ";" &
                        LSet("DUPLICATE", 15) & ":Y")
                    ElseIf gridView.CurrentRow.Cells("TRANTYPE").Value.ToString = "O" Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                        LSet("TYPE", 15) & ":ORD" & ";" &
                        LSet("BATCHNO", 15) & ":" & gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString & ";" &
                        LSet("TRANDATE", 15) & ":" & gridView.CurrentRow.Cells("TTRANDATE").FormattedValue.ToString & ";" &
                        LSet("DUPLICATE", 15) & ":Y")
                    Else
                        If ChkdupPrit_SP.Checked = False Then EstPurno = Val(gridView.CurrentRow.Cells("TRANNO").Value.ToString)
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                                                LSet("TYPE", 15) & ":EST" & ";" &
                                                LSet("ESTNO", 15) & ":S." & EstIssno.ToString & ";P." & EstPurno.ToString & ";" &
                                                LSet("TRANDATE", 15) & ":" & gridView.CurrentRow.Cells("TTRANDATE").FormattedValue.ToString & ";" &
                                                LSet("DUPLICATE", 15) & ":Y")
                    End If
                End If

            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If
        ElseIf UCase(e.KeyChar) = "E" Then
            If chkSummary.Checked = False Then
                If rdbSales.Checked = True Or rdbSaReturn.Checked = True Then
                    If gridView.CurrentRow.Cells("RESULT").Value.ToString = 0 Then Exit Sub
                Else
                    Exit Sub
                End If
            End If

            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
            If gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString = "" Then Exit Sub
            If gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString Is Nothing Then Exit Sub
            Dim objEst As New frmEstimation1
            objEst.Hide()
            objEst.BillDate = GetEntryDate(GetServerDate)
            objEst.lblUserName.Text = cnUserName
            objEst.lblSystemId.Text = systemId
            objEst.lblBillDate.Text = GetEntryDate(GetServerDate).ToString("dd/MM/yyyy")
            objEst.Set916Rate(objEst.BillDate)
            objEst.WindowState = FormWindowState.Minimized
            BrighttechPack.LanguageChange.Set_Language_Form(objEst, LangId)
            objGPack.Validator_Object(objEst)

            objEst.Size = New Size(1032, 745)
            objEst.MaximumSize = New Size(1032, 745)
            objEst.StartPosition = FormStartPosition.Manual
            objEst.Location = New Point((ScreenWid - objEst.Width) / 2, ((ScreenHit - 25) - objEst.Height) / 2)

            objEst.KeyPreview = True
            objEst.MaximizeBox = False
            objEst.StartPosition = FormStartPosition.CenterScreen
            objEst.EditTranNo = gridView.CurrentRow.Cells("TRANNO").Value.ToString
            objEst.EditBatchno = gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString
            objEst.EditTranDate = CType(gridView.CurrentRow.Cells("TTRANDATE").Value.ToString, Date)
            objEst.EditCostId = gridView.CurrentRow.Cells("COSTID").Value.ToString
            objEst.btnNew.Enabled = False
            objEst.Show()
            objEst.WindowState = FormWindowState.Normal
            If Estmationviewinown Then
                'frmEstimation1.BillDate = GetEntryDate(GetServerDate)
                'frmEstimation1.lblUserName.Text = cnUserName
                'frmEstimation1.lblSystemId.Text = systemId
                'frmEstimation1.lblBillDate.Text = GetEntryDate(GetServerDate).ToString("dd/MM/yyyy")
                'frmEstimation1.Set916Rate(objEst.BillDate)
                'frmEstimation1.EditTranNo = gridView.CurrentRow.Cells("TRANNO").Value.ToString
                'frmEstimation1.EditBatchno = gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString
                'frmEstimation1.EditTranDate = CType(gridView.CurrentRow.Cells("TTRANDATE").Value.ToString, Date)
                'frmEstimation1.EditCostId = gridView.CurrentRow.Cells("COSTID").Value.ToString
                'frmEstimation1.btnNew.Enabled = False
                Estmationviewinown = False
                'frmEstimation1.Show()
                Me.Hide()
            End If
        ElseIf UCase(e.KeyChar) = "C" Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
            If gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString = "" Then Exit Sub
            If gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString Is Nothing Then Exit Sub
            If gridView.CurrentRow.DefaultCellStyle.BackColor = Color.Red Then
                MessageBox.Show("Estimation No is already canceled...")
            Else
                Dim objSecret As New frmAdminPassword()
                If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
                If MessageBox.Show("Do you want to cancel", "CANCEL", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                    For i As Integer = 0 To gridView.Rows.Count - 1
                        'If rdbPurchase.Checked = True Then i = 0
                        gridView.CurrentCell = gridView.Rows(i).Cells("CHECK")
                        If CType(gridView.Rows(i).Cells("CHECK").Value, Boolean) = True Then
                            If gridView.Rows(i).Cells("TRANTYPE").Value.ToString = "O" Then
                                strSql = " UPDATE " & cnStockDb & "..ESTORMAST SET CANCEL = 'Y' WHERE ISNULL(BATCHNO,'') = '" & gridView.Rows(i).Cells("ESTBATCHNO").Value.ToString & "' AND ORTYPE='" & gridView.Rows(i).Cells("TRANTYPE").Value.ToString & "'"
                                If chkSummary.Checked = False Then strSql += " AND ISNULL(SNO,'') = '" & gridView.Rows(i).Cells("SNO").Value.ToString & "' AND ORTYPE='" & gridView.Rows(i).Cells("TRANTYPE").Value.ToString & "'"
                            Else
                                strSql = " UPDATE " & cnStockDb & "..ESTISSUE SET CANCEL = 'Y' WHERE ISNULL(ESTBATCHNO,'') = '" & gridView.Rows(i).Cells("ESTBATCHNO").Value.ToString & "' AND TRANTYPE='" & gridView.Rows(i).Cells("TRANTYPE").Value.ToString & "'"
                                If chkSummary.Checked = False Then strSql += " AND ISNULL(SNO,'') = '" & gridView.Rows(i).Cells("SNO").Value.ToString & "' AND TRANTYPE='" & gridView.Rows(i).Cells("TRANTYPE").Value.ToString & "'"
                                strSql += vbCrLf + " UPDATE " & cnStockDb & "..ESTRECEIPT SET CANCEL = 'Y' WHERE ISNULL(ESTBATCHNO,'') = '" & gridView.Rows(i).Cells("ESTBATCHNO").Value.ToString & "'AND TRANTYPE='" & gridView.Rows(i).Cells("TRANTYPE").Value.ToString & "'"
                                If chkSummary.Checked = False Then strSql += " AND ISNULL(SNO,'') = '" & gridView.Rows(i).Cells("SNO").Value.ToString & "' AND TRANTYPE='" & gridView.Rows(i).Cells("TRANTYPE").Value.ToString & "' "
                            End If
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                            If cnCostId <> gridView.Rows(i).Cells("COSTID").Value.ToString Then
                                Exec(strSql.Replace("'", "''"), cn, gridView.Rows(i).Cells("COSTID").Value.ToString, Nothing, tran)
                            End If
                            gridView.Rows(i).DefaultCellStyle.BackColor = Color.Red
                            gridView.Rows(i).DefaultCellStyle.SelectionBackColor = Color.Red
                        End If
                    Next
                    MessageBox.Show("Successfully Canceled...")
                    'strSql = " UPDATE " & cnStockDb & "..ESTISSUE SET CANCEL = 'Y' WHERE ISNULL(ESTBATCHNO,'') = '" & gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString & "'"
                    'If chkSummary.Checked = False Then strSql += " AND ISNULL(SNO,'') = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                    'strSql += vbCrLf + " UPDATE " & cnStockDb & "..ESTRECEIPT SET CANCEL = 'Y' WHERE ISNULL(ESTBATCHNO,'') = '" & gridView.CurrentRow.Cells("ESTBATCHNO").Value.ToString & "'"
                    'If chkSummary.Checked = False Then strSql += " AND ISNULL(SNO,'') = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                    'ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    'If cnCostId <> gridView.CurrentRow.Cells("COSTID").Value.ToString Then
                    '    Exec(strSql.Replace("'", "''"), cn, gridView.CurrentRow.Cells("COSTID").Value.ToString, Nothing, tran)
                    'End If
                End If

            End If

        End If

    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub

    Private Sub gridView_CellBeginEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles gridView.CellBeginEdit

    End Sub

    Private Sub gridView_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gridView.SelectionChanged

        'gridView.CurrentRow.DefaultCellStyle.BackColor = Color.Red

    End Sub

    Private Sub gridView_CellValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellValueChanged

    End Sub

    Private Sub SelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAll.CheckedChanged
        For j As Integer = 0 To Me.gridView.RowCount - 1
            gridView.Rows(j).Cells("CHECK").Value = SelectAll.Checked
        Next
        gridView.Focus()
    End Sub

    Private Sub chkSummary_CheckedChanged(sender As Object, e As EventArgs) Handles chkSummary.CheckedChanged
        chkEstWiseSummary.Enabled = Not chkSummary.Checked
        chkEstWiseSummary.Checked = False
        If Not rdbSales.Checked Then
            ChkWithPaymode.Visible = False
            ChkWithPaymode.Checked = False
        Else
            ChkWithPaymode.Visible = Not chkSummary.Checked
            ChkWithPaymode.Checked = False
        End If

    End Sub

    Private Sub rdbSales_CheckedChanged(sender As Object, e As EventArgs) Handles rdbSales.CheckedChanged
        ChkWithPaymode.Visible = rdbSales.Checked
        ChkWithPaymode.Checked = False
    End Sub
End Class