Imports System.Data.OleDb
Public Class frmPend
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim CTRITEMTRF As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ISCTRITEMTRF'", , , tran)

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        tabMain.SelectedTab = tabGeneral
        rbtPartySales.Checked = True

        dtpFrom.Select()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        dtpFrom.Focus()
    End Sub

    Private Sub frmPend_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmPend_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub frmPend_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre_MAN.Enabled = True
            strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre_MAN, , False)
        Else
            cmbCostCentre_MAN.Text = ""
            cmbCostCentre_MAN.Enabled = False
        End If
        gridView.RowTemplate.Height = 21
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect

        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRNAME"
        cmbToCounter.Items.Clear()
        cmbToCounter.Items.Add("")
        objGPack.FillCombo(strSql, cmbToCounter, False, False)

        ''Load Metal
        cmbOpenMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbOpenMetal, False)
        cmbOpenMetal.Text = ""


        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        btnSearch.Enabled = False
        Me.Refresh()
        If rbtPurchase.Checked = True Then CTRITEMTRF = "N"
        If rbtPartySales.Checked Then
            strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & SYSTEMID & "PENDTRANS') > 0 DROP TABLE TEMP" & SYSTEMID & "PENDTRANS"
            strSql += " SELECT TRANDATE,SNO,ITEMCTRID,CATCODE,PCS,GRSWT,NETWT,TAGPCS,TAGGRSWT,TAGNETWT,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
            strSql += " INTO TEMP" & SYSTEMID & "PENDTRANS"
            strSql += " FROM " & CNSTOCKDB & "..ISSUE"
            strSql += " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += " AND TRANTYPE = 'SA' AND ISNULL(CANCEL,'') = ''"
            strSql += " AND (TAGPCS <> PCS OR TAGGRSWT <> GRSWT OR TAGNETWT <> NETWT)"
            strSql += " AND (TAGPCS <> 0 OR TAGGRSWT <> 0 OR TAGNETWT <> 0)"
            If cmbOpenMetal.Text <> "ALL" Then strSql += " AND METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"

            strSql += " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
            strSql += " AND COMPANYID = '" & strCompanyId & "'"

            strSql += " "
            strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PENDTRANSSTUD') > 0 DROP TABLE TEMP" & systemId & "PENDTRANSSTUD"
            strSql += " SELECT TRANDATE,ISSSNO SNO,(SELECT ITEMCTRID FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSSNO)ITEMCTRID,CATCODE,TAGSTNPCS-STNPCS STNPCS,TAGSTNWT - STNWT STNWT"
            strSql += " INTO TEMP" & systemId & "PENDTRANSSTUD"
            strSql += " FROM " & cnStockDb & "..ISSSTONE I"
            strSql += " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' AND TRANTYPE = 'SA')"
            strSql += " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')"
            strSql += " AND (TAGSTNPCS <> STNPCS OR TAGSTNWT <> STnWT)"
            strSql += " AND (TAGSTNPCS <> 0 OR TAGSTNWT <> 0)"
            strSql += " AND (TAGSTNPCS - STNPCS <> 0 OR TAGSTNWT - STNWT <> 0)"
            strSql += " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
            strSql += " AND COMPANYID = '" & strCompanyId & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = " "
            strSql += " UPDATE TEMP" & systemId & "PENDTRANS SET DIAPCS = (SELECT SUM(STNPCS) FROM TEMP" & systemId & "PENDTRANSSTUD WHERE SNO = T.SNO)"
            strSql += " ,DIAWT = (SELECT SUM(STNWT) FROM TEMP" & systemId & "PENDTRANSSTUD WHERE SNO = T.SNO)"
            strSql += " FROM TEMP" & systemId & "PENDTRANS AS T"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If CTRITEMTRF = "N" Then
                strSql = " SELECT TRANDATE,METALNAME,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT) AS DIAWT"
                strSql += " FROM"
                strSql += " ("
                strSql += " SELECT TRANDATE,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = T.CATCODE))AS METALNAME"
                strSql += " ,SUM(TAGPCS-PCS)PCS,SUM(TAGGRSWT-GRSWT)GRSWT,SUM(TAGNETWT-NETWT)NETWT,SUM(ISNULL(DIAPCS,0))DIAPCS,SUM(ISNULL(DIAWT,0))DIAWT"
                strSql += " FROM TEMP" & systemId & "PENDTRANS AS T"
                strSql += " GROUP BY TRANDATE,CATCODE"
                strSql += " )X GROUP BY METALNAME,TRANDATE"
                strSql += " ORDER BY TRANDATE,METALNAME"
            Else
                strSql = " SELECT TRANDATE,ITEMCTRNAME,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT) AS DIAWT"
                strSql += " FROM"
                strSql += " ("
                strSql += " SELECT TRANDATE,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS ITEMCTRNAME"
                strSql += " ,SUM(TAGPCS-PCS)PCS,SUM(TAGGRSWT-GRSWT)GRSWT,SUM(TAGNETWT-NETWT)NETWT,SUM(ISNULL(DIAPCS,0))DIAPCS,SUM(ISNULL(DIAWT,0))DIAWT"
                strSql += " FROM TEMP" & systemId & "PENDTRANS AS T"
                strSql += " GROUP BY TRANDATE,ITEMCTRID"
                strSql += " )X GROUP BY ITEMCTRNAME,TRANDATE"
                strSql += " ORDER BY TRANDATE,ITEMCTRNAME"
            End If
        End If
            If rbtSalesReturn.Checked Then
                strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PENDTRANS') > 0 DROP TABLE TEMP" & systemId & "PENDTRANS"
            strSql += " SELECT TRANDATE,SNO,ITEMCTRID,CATCODE,PCS,GRSWT,NETWT,TAGPCS,TAGGRSWT,TAGNETWT,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                strSql += " INTO TEMP" & systemId & "PENDTRANS"
                strSql += " FROM " & cnStockDb & "..RECEIPT"
                strSql += " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                strSql += "     AND TRANTYPE = '" & IIf(rbtSalesReturn.Checked, "SR", "PU") & "'"
                strSql += " AND ISNULL(CANCEL,'') = ''"
                If cmbOpenMetal.Text <> "ALL" Then strSql += " AND METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
                strSql += " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                strSql += " AND COMPANYID = '" & strCompanyId & "'"

                strSql += " "
                strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PENDTRANSSTUD') > 0 DROP TABLE TEMP" & systemId & "PENDTRANSSTUD"
            strSql += " SELECT TRANDATE,ISSSNO SNO,(SELECT ITEMCTRID FROM " & cnStockDb & "..RECEIPT WHERE SNO=R.ISSSNO)ITEMCTRID,CATCODE,TAGSTNPCS-STNPCS STNPCS,TAGSTNWT - STNWT STNWT"
                strSql += " INTO TEMP" & systemId & "PENDTRANSSTUD"
            strSql += " FROM " & cnStockDb & "..RECEIPTSTONE R"
                strSql += " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                strSql += " AND ISSSNO IN (sELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
                strSql += "     AND TRANTYPE = '" & IIf(rbtSalesReturn.Checked, "SR", "PU") & "')"
                strSql += " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')"
                strSql += " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                strSql += " AND COMPANYID = '" & strCompanyId & "'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                strSql = " "
                strSql += " UPDATE TEMP" & systemId & "PENDTRANS SET DIAPCS = (SELECT SUM(STNPCS) FROM TEMP" & systemId & "PENDTRANSSTUD WHERE SNO = T.SNO)"
                strSql += " ,DIAWT = (SELECT SUM(STNWT) FROM TEMP" & systemId & "PENDTRANSSTUD WHERE SNO = T.SNO)"
                strSql += " FROM TEMP" & systemId & "PENDTRANS AS T"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If CTRITEMTRF = "N" Then
                strSql = " SELECT TRANDATE,METALNAME,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT) AS DIAWT"
                strSql += " FROM"
                strSql += " ("
                strSql += " SELECT TRANDATE,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = T.CATCODE))AS METALNAME"
                strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(ISNULL(DIAPCS,0))DIAPCS,SUM(ISNULL(DIAWT,0))DIAWT"
                strSql += " FROM TEMP" & systemId & "PENDTRANS AS T"
                strSql += " GROUP BY TRANDATE,CATCODE"
                strSql += " )X GROUP BY METALNAME,TRANDATE"
                strSql += " ORDER BY TRANDATE,METALNAME"
            Else
                strSql = " SELECT TRANDATE,ITEMCTRNAME,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT) AS DIAWT"
                strSql += " FROM"
                strSql += " ("
                strSql += " SELECT TRANDATE,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS ITEMCTRNAME"
                strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(ISNULL(DIAPCS,0))DIAPCS,SUM(ISNULL(DIAWT,0))DIAWT"
                strSql += " FROM TEMP" & systemId & "PENDTRANS AS T"
                strSql += " GROUP BY TRANDATE,ITEMCTRID"
                strSql += " )X GROUP BY ITEMCTRNAME,TRANDATE"
                strSql += " ORDER BY TRANDATE,ITEMCTRNAME"
            End If
                
            End If
            If rbtPurchase.Checked Then
                strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PENDTRANS') > 0 DROP TABLE TEMP" & systemId & "PENDTRANS"
                strSql += " SELECT TRANDATE,SNO,CATCODE,PCS,GRSWT,NETWT,TAGPCS,TAGGRSWT,TAGNETWT,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                strSql += " INTO TEMP" & systemId & "PENDTRANS"
                strSql += " FROM " & cnStockDb & "..RECEIPT"
                strSql += " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                strSql += "     AND TRANTYPE = '" & IIf(rbtSalesReturn.Checked, "SR", "PU") & "'"
                strSql += " AND ISNULL(CANCEL,'') = ''"
                If cmbOpenMetal.Text <> "ALL" Then strSql += " AND METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
                strSql += " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                strSql += " AND COMPANYID = '" & strCompanyId & "'"

                strSql += " "
                strSql += " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PENDTRANSSTUD') > 0 DROP TABLE TEMP" & systemId & "PENDTRANSSTUD"
                strSql += " SELECT TRANDATE,ISSSNO SNO,CATCODE,STNPCS,STNWT"
                strSql += " INTO TEMP" & systemId & "PENDTRANSSTUD"
                strSql += " FROM " & cnStockDb & "..RECEIPTSTONE"
                strSql += " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                strSql += " AND ISSSNO IN (sELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
                strSql += "     AND TRANTYPE = '" & IIf(rbtSalesReturn.Checked, "SR", "PU") & "')"
                strSql += " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')"
                strSql += " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                strSql += " AND COMPANYID = '" & strCompanyId & "'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                strSql = " "
                strSql += " UPDATE TEMP" & systemId & "PENDTRANS SET DIAPCS = (SELECT SUM(STNPCS) FROM TEMP" & systemId & "PENDTRANSSTUD WHERE SNO = T.SNO)"
                strSql += " ,DIAWT = (SELECT SUM(STNWT) FROM TEMP" & systemId & "PENDTRANSSTUD WHERE SNO = T.SNO)"
                strSql += " FROM TEMP" & systemId & "PENDTRANS AS T"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                'strSql = " SELECT TRANDATE,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = T.CATCODE))AS METALNAME"
                'strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(ISNULL(DIAPCS,0))DIAPCS,SUM(ISNULL(DIAWT,0))DIAWT"
                'strSql += " FROM TEMP" & systemId & "PENDTRANS AS T"
                'strSql += " GROUP BY TRANDATE,CATCODE"
                'strSql += " ORDER BY TRANDATE,METALNAME"
                strSql = " SELECT TRANDATE,METALNAME,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT) AS DIAWT"
                strSql += " FROM"
                strSql += " ("
                strSql += " SELECT TRANDATE,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = T.CATCODE))AS METALNAME"
                strSql += " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(ISNULL(DIAPCS,0))DIAPCS,SUM(ISNULL(DIAWT,0))DIAWT"
                strSql += " FROM TEMP" & systemId & "PENDTRANS AS T"
                strSql += " GROUP BY TRANDATE,CATCODE"
                strSql += " )X GROUP BY METALNAME,TRANDATE"
                strSql += " ORDER BY TRANDATE,METALNAME"
            End If
            Try
                Dim dtGrid As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGrid)
                If Not dtGrid.Rows.Count > 0 Then
                    MsgBox("Record not found", MsgBoxStyle.Information)
                    Exit Sub
                End If
                tabMain.SelectedTab = tabView
                gridView.DataSource = dtGrid
                With gridView
                    .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                .Columns("TRANDATE").Width = 100
                If CTRITEMTRF = "N" Then
                    .Columns("METALNAME").Width = 100
                Else
                    .Columns("ITEMCTRNAME").Width = 100
                End If
                .Columns("PCS").Width = 60
                .Columns("GRSWT").Width = 100
                .Columns("NETWT").Width = 100
                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAPCS").Width = 60
                .Columns("DIAWT").Width = 80
                For cnt As Integer = 0 To .ColumnCount - 1
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
            End With
                btnTransfer.Focus()
            Catch ex As Exception
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Finally
                btnSearch.Enabled = True
            End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        Dim key As String = Nothing
        If rbtPartySales.Checked Then
            key = "PARTLY-"
        ElseIf rbtSalesReturn.Checked Then
            key = "RETURN-"
        Else
            CTRITEMTRF = "N"
            key = "PURCHASE-"
        End If
        Dim dt As New DataTable
        Dim itemId As Integer
        If CTRITEMTRF = "N" Then
            dt = CType(gridView.DataSource, DataTable).DefaultView.ToTable(True, "METALNAME")
        Else
            dt = CType(gridView.DataSource, DataTable).DefaultView.ToTable(True, "ITEMCTRNAME")
        End If

        For Each ro As DataRow In dt.Rows

            Dim metalId As String

            If CTRITEMTRF = "N" Then metalId = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & ro.Item("METALNAME").ToString & "'")
            If CTRITEMTRF = "Y" Then
                Dim itemIds As String = objGPack.GetSqlValue("SELECT POS_ITEMID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & ro.Item("ITEMCTRNAME").ToString & "'")
                Dim Itemidarry() As String = Split(itemIds, ",")
                If rbtSalesReturn.Checked = True Then itemId = Val(Itemidarry(1).ToString())
                If rbtPartySales.Checked = True Then itemId = Val(Itemidarry(0).ToString())
            End If


            If CTRITEMTRF = "Y" Then
                If itemId = 0 Then
                    MsgBox("Please fill valid itemid into ITEMCOUNTER for [" & key + ro.Item("itemCtrname") & "  Control Id")
                    Exit Sub
                End If
            Else
                itemId = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & key + metalId & "'"))
                If itemId = 0 Then
                    MsgBox("Please fill valid itemid into softcontrol for [" & key + metalId & "] Control Id")
                    Exit Sub
                End If
            End If
        Next
        If CTRITEMTRF = "N" Then
            Dim diaPcs As Integer = Val(CType(gridView.DataSource, DataTable).Compute("SUM(DIAPCS)", "DIAPCS IS NOT NULL").ToString)
            Dim diaWt As Double = Val(CType(gridView.DataSource, DataTable).Compute("SUM(DIAWT)", "DIAWT IS NOT NULL").ToString)
            If diaPcs <> 0 Or diaWt <> 0 Then
                Dim metalId As String = "D" '            objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = 'D'")
                itemId = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & key + metalId & "'"))
                If itemId = 0 Then
                    MsgBox("Please fill valid itemid into softcontrol for [" & key + metalId & "] Control Id")
                    Exit Sub
                End If
            End If
        End If


        Dim ItemCtrId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbToCounter.Text & "'"))
        If ItemCtrId = 0 Then
            If MessageBox.Show("To Counter selection is empty. Sure you want post?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If
        End If

        If MessageBox.Show("Do you want to transfer the above items into nontag?", "Transfer Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then Exit Sub
        Try
            Dim costId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")
            tran = Nothing
            tran = cn.BeginTransaction
            For cnt As Integer = 0 To gridView.RowCount - 1
                With gridView.Rows(cnt)
                    If CTRITEMTRF = "N" Then
                        Dim metalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & .Cells("METALNAME").Value.ToString & "'", , , tran)
                        itemId = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & key + metalId & "'", , , tran))

                    Else
                        Dim itemIds As String = objGPack.GetSqlValue("SELECT POS_ITEMID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & .Cells("ITEMCTRNAME").Value.ToString & "'", , , tran)
                        Dim Itemidarry() As String = Split(itemIds, ",")
                        If rbtSalesReturn.Checked = True Then itemId = Val(Itemidarry(1).ToString())
                        If rbtPartySales.Checked = True Then itemId = Val(Itemidarry(0).ToString())
                    End If

                    strSql = " DELETE FROM " & cnAdminDb & "..ITEMNONTAGSTONE"
                    strSql += " where TAGSNO IN ("
                    strSql += " SELECT SNO FROM " & cnAdminDb & "..ITEMNONTAG"
                    strSql += " WHERE RECDATE = '" & CType(.Cells("TRANDATE").Value, Date).ToString("yyyy-MM-dd") & "'"
                    strSql += " AND ITEMID = " & itemId & ""
                    If rbtPartySales.Checked Then
                        strSql += " AND POSTED = 'P'"   ''FROM PENDING TRANSFER
                    ElseIf rbtSalesReturn.Checked Then
                        strSql += " AND POSTED = 'R'"   ''FROM Return Trans
                    Else
                        strSql += " AND POSTED = 'U'"   ''FROM Purchase Trans
                    End If
                    strSql += " AND ISNULL(CANCEL,'') = ''"
                    strSql += " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += " )"
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, costId, , , "ITEMNONTAGSTONE")

                    strSql = " DELETE FROM " & cnAdminDb & "..ITEMNONTAG"
                    strSql += " WHERE RECDATE = '" & CType(.Cells("TRANDATE").Value, Date).ToString("yyyy-MM-dd") & "'"
                    strSql += " AND ITEMID = " & itemId & ""
                    If rbtPartySales.Checked Then
                        strSql += " AND POSTED = 'P'"   ''FROM PENDING TRANSFER
                    ElseIf rbtSalesReturn.Checked Then
                        strSql += " AND POSTED = 'R'"   ''FROM Return Trans
                    Else
                        strSql += " AND POSTED = 'U'"   ''FROM Purchase Trans
                    End If
                    strSql += " AND ISNULL(CANCEL,'') = ''"
                    strSql += " AND COMPANYID = '" & strCompanyId & "'"

                    ExecQuery(SyncMode.Stock, strSql, cn, tran, costId, , , "ITEMNONTAG")

                    Dim tagSno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAG"
                    strSql += " ("
                    strSql += " SNO,ITEMID,SUBITEMID,COMPANYID,RECDATE,"
                    strSql += " PCS,GRSWT,LESSWT,NETWT,"
                    strSql += " FINRATE,ISSTYPE,RECISS,POSTED,"
                    strSql += " LOTNO,PACKETNO,DREFNO,ITEMCTRID,"
                    strSql += " ORDREPNO,ORSNO,NARRATION,PURWASTAGE,"
                    strSql += " PURRATE,PURMC,RATE,COSTID,"
                    strSql += " CTGRM,DESIGNERID,VATEXM,ITEMTYPEID,"
                    strSql += " CARRYFLAG,REASON,BATCHNO,CANCEL,"
                    strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER)VALUES("
                    strSql += " '" & tagSno & "'" 'SNO
                    strSql += " ," & itemId & "" 'ITEMID
                    strSql += " ,0" 'SUBITEMID
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & CType(.Cells("TRANDATE").Value, Date).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                    strSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
                    strSql += " ," & Val(.Cells("GRSWT").Value.ToString) - Val(.Cells("NETWT").Value.ToString) & "" 'LESSWT
                    strSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                    strSql += " ,0" 'FINRATE
                    strSql += " ,''" 'ISSTYPE
                    strSql += " ,'R'" 'RECISS
                    If rbtPartySales.Checked Then
                        strSql += " ,'P'" 'POSTED   ''FROM PENDING TRANSFER
                    ElseIf rbtSalesReturn.Checked Then
                        strSql += " ,'R'" 'POSTED   ''FROM Return Trans
                    Else
                        strSql += " ,'U'" 'POSTED   ''FROM Purchase Trans
                    End If
                    strSql += " ,0" 'LOTNO
                    strSql += " ,''" 'PACKETNO
                    strSql += " ,0" 'DREFNO
                    strSql += " ," & ItemCtrId & "" 'ITEMCTRID
                    strSql += " ,''" 'ORDREPNO
                    strSql += " ,''" 'ORSNO
                    If rbtPartySales.Checked Then
                        strSql += " ,'TRANS FROM PENDING'" 'NARRATION   ''FROM PENDING TRANSFER
                    ElseIf rbtSalesReturn.Checked Then
                        strSql += " ,'TRANS FROM RETURN'" 'NARRATION   ''FROM Return Trans
                    Else
                        strSql += " ,'TRANS FROM PURCHASE'" 'NARRATION   ''FROM Purchase Trans
                    End If
                    strSql += " ,0" 'PURWASTAGE
                    strSql += " ,0" 'PURRATE
                    strSql += " ,0" 'PURMC
                    strSql += " ,0" 'RATE
                    strSql += " ,'" & costId & "'" 'COSTID
                    strSql += " ,''" 'CTGRM
                    strSql += " ,0" 'DESIGNERID
                    strSql += " ,''" 'VATEXM
                    strSql += " ,0" 'ITEMTYPEID
                    strSql += " ,''" 'CARRYFLAG
                    strSql += " ,'0'" 'REASON
                    strSql += " ,''" 'BATCHNO
                    strSql += " ,''" 'CANCEL
                    strSql += " ," & userId & "" 'USERID
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, costId, , , "ITEMNONTAG")

                    If Val(.Cells("DIAPCS").Value.ToString) <> 0 Or Val(.Cells("DIAWT").Value.ToString) <> 0 Then
                        Dim DiaMetalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID = 'D'", , , tran)
                        Dim DiaItemId As Integer = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & key + DiaMetalId & "'", , , tran))
                        ''INSERTING NONTAG STONE
                        strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAGSTONE("
                        strSql += " SNO,RECISS,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                        strSql += " STNSUBITEMID,STNPCS,STNWT,"
                        strSql += " STNRATE,STNAMT,DESCRIP,"
                        strSql += " RECDATE,PURRATE,PURAMT,CALCMODE,"
                        strSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
                        strSql += " VATEXM,CARRYFLAG,COSTID,SYSTEMID,APPVER)VALUES("
                        'strSql += " '" & GetSnoFromSoftControl("NONTAGSTONESNO", cnStockDb, "ITEMNONTAGSTONE", tran) & "'" ''SNO
                        strSql += " '" & GetNewSno(TranSnoType.ITEMNONTAGSTONECODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
                        strSql += " ,'R'" ' RECISS
                        strSql += " ,'" & tagSno & "'" 'TAGSNO
                        strSql += " ,'" & itemId & "'" 'ITEMID
                        strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                        strSql += " ," & DiaItemId & "" 'STNITEMID
                        strSql += " ,0" 'STNSUBITEMID
                        strSql += " ," & Val(.Cells("DIAPCS").Value.ToString) & "" 'STNPCS
                        strSql += " ," & Val(.Cells("DIAWT").Value.ToString) & "" 'STNWT
                        strSql += " ,0" 'STNRATE
                        strSql += " ,0" 'STNAMT
                        strSql += " ,''"
                        strSql += " ,'" & CType(.Cells("TRANDATE").Value, Date).ToString("yyyy-MM-dd") & "'" 'RECDATE
                        strSql += " ,0" 'PURRATE
                        strSql += " ,0" 'PURAMT
                        strSql += " ,'W'" 'CALCMODE
                        strSql += " ,0" 'MINRATE
                        strSql += " ,0" 'SIZECODE
                        strSql += " ,'C'" 'STONEUNIT
                        strSql += " ,NULL" 'ISSDATE
                        strSql += " ,''" 'VATEXM
                        strSql += " ,''" 'CARRYFLAG
                        strSql += " ,'" & costId & "'" 'COSTID
                        strSql += " ,'" & systemId & "'" 'SYSTEMID
                        strSql += " ,'" & VERSION & "'" 'APPVER
                        strSql += " )"
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, costId, , , "ITEMNONTAGSTONE")
                    End If
                End With
            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Transfered successfully..")
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
End Class