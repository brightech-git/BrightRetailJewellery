Imports System.Data.OleDb
Public Class frmPutaxpost
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim CTRITEMTRF As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ISCTRITEMTRF'", , , tran)

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        tabMain.SelectedTab = tabGeneral

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

        
        ''Load Metal
        cmbOpenMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbOpenMetal, False)
        cmbOpenMetal.Text = ""


        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Val(txtVatper.Text) = 0 Then MsgBox("Vat percent is zero") : txtVatper.Focus() : Exit Sub
        gridView.DataSource = Nothing
        btnSearch.Enabled = False
        Me.Refresh()
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PENDTRANS') > 0 DROP TABLE TEMP" & systemId & "PENDTRANS"
        strSql += " SELECT TRANDATE,CATCODE,PCS,GRSWT,NETWT,AMOUNT,CASE WHEN ISNULL(TAX,0) = 0 THEN AMOUNT*" & (Val(txtVatper.Text) / 100) & " ELSE TAX END TAX "
        strSql += " INTO TEMP" & systemId & "PENDTRANS"
        strSql += " FROM " & cnStockDb & "..RECEIPT"
        strSql += " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += "     AND TRANTYPE = 'PU'"
        strSql += " AND ISNULL(CANCEL,'') = ''"
        If cmbOpenMetal.Text <> "ALL" Then strSql += " AND METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
        strSql += " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        strSql += " AND COMPANYID = '" & strCompanyId & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT TRANDATE,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(ISNULL(AMOUNT,0))AMOUNT,SUM(ISNULL(TAX,0))TAX"
        strSql += " FROM TEMP" & systemId & "PENDTRANS AS T"
        strSql += " GROUP BY TRANDATE"
        strSql += " ORDER BY TRANDATE"

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
                .Columns("PCS").Width = 60
                .Columns("GRSWT").Width = 100
                .Columns("NETWT").Width = 100
                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").Width = 80
                .Columns("TAX").Width = 60
                .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("TAX").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("TAX").DefaultCellStyle.Format = "0.00"
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

        key = "PURCHASE-"

        Dim dt As New DataTable
        Dim metalid As String
        If cmbOpenMetal.Text <> "ALL" Then metalid = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'") Else metalid = ""
        '        dt = CType(gridView.DataSource, DataTable).DefaultView.ToTable(True, "METALNAME")

        Dim mCostid As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")

       

        If MessageBox.Show("Do you want to Post tax into Account?", "Posting Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then Exit Sub
        Try
            Dim drcode As String = "PTAXURD"
            Dim crcode As String = "VATOUT"
            Dim Drname As String = objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & drcode & "'")
            Dim Crname As String = objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & crcode & "'")
            Dim costId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")
            tran = Nothing
            tran = cn.BeginTransaction
            strSql = " DELETE FROM " & cnStockDb & "..ACCTRAN"
            strSql += " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += " AND PAYMODE ='PV' AND TRANNO = 88888 AND FROMFLAG= 'U' AND TRANSFERED='P'"
            If metalid <> "" Then strSql += " AND REFNO = '" & metalid & "'"
            strSql += " AND COSTID = '" & mCostid & "' AND COMPANYID = '" & strCompanyId & "'"
            ExecQuery(SyncMode.Stock, strSql, cn, tran, cnCostId)
            For cnt As Integer = 0 To gridView.RowCount - 1
                With gridView.Rows(cnt)
                    If Val(.Cells("TAX").Value.ToString) <> 0 Then
                        Dim BatchNo As String = GetNewBatchno(cnCostId, CDate(.Cells("TRANDATE").Value.ToString), tran)
                        InsertIntoAccTran(88888, "D", drcode, Val(.Cells("TAX").Value.ToString), Val(.Cells("PCS").Value.ToString), Val(.Cells("GRSWT").Value.ToString), Val(.Cells("NETWT").Value.ToString), "PV", crcode, .Cells("TRANDATE").Value.ToString, "Purch.Tax Posting", metalid, BatchNo, mCostid)
                        InsertIntoAccTran(88888, "C", crcode, Val(.Cells("TAX").Value.ToString), Val(.Cells("PCS").Value.ToString), Val(.Cells("GRSWT").Value.ToString), Val(.Cells("NETWT").Value.ToString), "PV", drcode, .Cells("TRANDATE").Value.ToString, "Purch.Tax Posting", metalid, BatchNo, mCostid)
                    End If
                End With
            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Posting successfully.." & vbCrLf & "Check Ledger's of " & vbCrLf & Crname & " vs " & Drname)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub InsertIntoAccTran _
        (ByVal tNo As Integer, _
        ByVal tranMode As String, _
        ByVal accode As String, _
        ByVal amount As Double, _
        ByVal pcs As Integer, _
        ByVal grsWT As Double, _
        ByVal netWT As Double, _
        ByVal payMode As String, _
        ByVal contra As String, _
        Optional ByVal BillDate As String = Nothing, _
        Optional ByVal Remark1 As String = Nothing, _
        Optional ByVal Metalid As String = Nothing, _
    Optional ByVal BATCHNO As String = Nothing, _
Optional ByVal BillCostId As String = Nothing _
        )
        If amount = 0 Then Exit Sub

        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE"
        strSql += " ,AMOUNT,BALANCE,PCS,GRSWT,NETWT"
        strSql += " ,REFNO,REFDATE,PAYMODE"
        strSql += " ,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,TRANSFERED,APPVER,COMPANYID"
        strSql += " "
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & BillDate & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strSql += " ,0" 'AMOUNT
        strSql += " ," & Math.Abs(pcs) & "" 'PCS
        strSql += " ," & Math.Abs(grsWT) & "" 'GRSWT
        strSql += " ," & Math.Abs(netWT) & "" 'NETWT
        strSql += " ,'" & Metalid & "'" 'REFNO
        strSql += " ,NULL" 'REFDATE

        strSql += " ,'" & payMode & "'" 'PAYMODE
        strSql += " ,'U'" 'FROMFLAG
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,''" 'REMARK2
        strSql += " ,'" & contra & "'" 'CONTRA
        strSql += " ,'" & BATCHNO & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,''" 'CASHID
        strSql += " ,'" & BillCostId & "'" 'COSTID
        strSql += " ,'P'" 'TRANSFERED
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        strSql = ""
        cmd = Nothing

    End Sub


End Class