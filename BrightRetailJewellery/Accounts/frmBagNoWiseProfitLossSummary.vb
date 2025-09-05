Imports System.Data.OleDb
Public Class frmBagNoWiseProfitLossSummary
    Dim strSql As String
    Dim cmd As New OleDbCommand
    Private Sub frmBagNoWiseProfitLossSummary_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmBagNoWiseProfitLossSummary_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y'"
        strSql += " ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False, False)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        gridViewHeader.DataSource = Nothing
        gridView.DataSource = Nothing
        lblTitle.Visible = False
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        cmbMetal.Text = "ALL"
        dtpFrom.Select()
        ChkNetwt.Checked = False
        ChkPurewt.Checked = False
        Prop_Gets()
        CkGrswt.Checked = True
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHeader)
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHeader)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            CkGrswt.Checked = True
            btnSearch.Enabled = False
            gridView.DataSource = Nothing
            gridViewHeader.DataSource = Nothing
            strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMPBAGNOWISE')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPBAGNOWISE "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = " SELECT BAGNO,SUM(MIGRSWT)MIGRSWT ,SUM(MINETWT)MINETWT,SUM(MIPUREWT)MIPUREWT,"
            strSql += " SUM(MIAMOUNT)MIAMOUNT,SUM(MRGRSWT)MRGRSWT ,SUM(MRNETWT)MRNETWT,SUM(MRPUREWT)MRPUREWT,"
            strSql += " SUM(MRAMOUNT)MRAMOUNT,SUM(PIGRSWT)PIGRSWT ,SUM(PINETWT)PINETWT,SUM(PIPUREWT)PIPUREWT,"
            strSql += " SUM(PIAMOUNT)PIAMOUNT,SUM(PRGRSWT)PRGRSWT ,SUM(PRNETWT)PRNETWT,SUM(PRPUREWT)PRPUREWT,"
            strSql += " SUM(PRAMOUNT)PRAMOUNT,SUM(BGRSWT)BGRSWT ,SUM(BNETWT)BNETWT,SUM(MIPUREWT-MRPUREWT)BPUREWT,SUM(BAMOUNT)BAMOUNT  "
            strSql += "  INTO TEMPTABLEDB..TEMPBAGNOWISE  FROM("
            strSql += " SELECT BAGNO,SUM(MIGRSWT)MIGRSWT ,SUM(MINETWT)MINETWT,"
            strSql += "(SELECT SUM(PUREWT) FROM " & cnStockDb & " ..ISSUE WHERE BAGNO =X.BAGNO  AND TRANTYPE='IIS')AS MIPUREWT,"
            strSql += "SUM(MIAMOUNT)MIAMOUNT,"
            strSql += "SUM(MRGRSWT)MRGRSWT ,SUM(MRNETWT)MRNETWT,"
            strSql += "(SELECT SUM(PUREWT) FROM " & cnStockDb & " ..RECEIPT WHERE BAGNO=X.BAGNO AND TRANTYPE='RRE')AS MRPUREWT,"
            strSql += "SUM(MRAMOUNT)MRAMOUNT,"
            strSql += "SUM(PIGRSWT)PIGRSWT ,SUM(PINETWT)PINETWT,"
            strSql += "(SELECT SUM(PUREWT) FROM " & cnStockDb & " ..ISSUE WHERE BAGNO =X.BAGNO AND TRANTYPE ='IMP')AS PIPUREWT,"
            strSql += "SUM(PIAMOUNT)PIAMOUNT,"
            strSql += "SUM(PRGRSWT)PRGRSWT ,SUM(PRNETWT)PRNETWT,"
            strSql += "(SELECT SUM(PUREWT) FROM " & cnStockDb & " ..RECEIPT WHERE BAGNO =X.BAGNO AND TRANTYPE ='RMP')AS PRPUREWT,"
            strSql += "SUM(PRAMOUNT)PRAMOUNT,"
            strSql += "SUM(BGRSWT)BGRSWT ,SUM(BNETWT)BNETWT,0.00 AS BPUREWT,SUM(BAMOUNT)BAMOUNT"
            strSql += " FROM  ("
            strSql += " SELECT BAGNO"
            strSql += " ,SUM(CASE WHEN TRANTYPE = 'MI' THEN GRSWT ELSE 0 END) AS MIGRSWT"
            strSql += " ,SUM(CASE WHEN TRANTYPE = 'MI' THEN NETWT ELSE 0 END) AS MINETWT"
            strSql += " ,SUM(CASE WHEN TRANTYPE = 'MI' THEN AMOUNT ELSE 0 END) AS MIAMOUNT"
            strSql += " ,SUM(CASE WHEN TRANTYPE= 'MR' THEN GRSWT ELSE 0 END) AS MRGRSWT"
            strSql += " ,SUM(CASE WHEN TRANTYPE= 'MR' THEN NETWT ELSE 0 END) AS MRNETWT"
            strSql += " ,SUM(CASE WHEN TRANTYPE= 'MR' THEN AMOUNT ELSE 0 END) AS MRAMOUNT"
            strSql += " ,SUM(CASE WHEN TRANTYPE = 'PI' THEN GRSWT ELSE 0 END) AS PIGRSWT"
            strSql += " ,SUM(CASE WHEN TRANTYPE = 'PI' THEN NETWT ELSE 0 END) AS PINETWT"
            strSql += " ,SUM(CASE WHEN TRANTYPE = 'PI' THEN AMOUNT ELSE 0 END) AS PIAMOUNT"
            strSql += " ,SUM(CASE WHEN TRANTYPE= 'PR' THEN GRSWT ELSE 0 END) AS PRGRSWT"
            strSql += " ,SUM(CASE WHEN TRANTYPE= 'PR' THEN NETWT ELSE 0 END) AS PRNETWT"
            strSql += " ,SUM(CASE WHEN TRANTYPE= 'PR' THEN AMOUNT ELSE 0 END) AS PRAMOUNT"
            strSql += " ,SUM(CASE WHEN RECISS = 'I' THEN GRSWT ELSE -1*GRSWT END) AS BGRSWT"
            strSql += " ,SUM(CASE WHEN RECISS = 'I' THEN NETWT ELSE -1*NETWT END) AS BNETWT"
            strSql += " ,SUM(CASE WHEN RECISS = 'I' THEN AMOUNT ELSE -1*AMOUNT END) AS BAMOUNT"
            strSql += " FROM " & cnStockDb & "..MELTINGDETAIL"
            strSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
            If cmbMetal.Text <> "ALL" Then
                strSql += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
            End If
            strSql += " GROUP BY BAGNO"
            strSql += " )X"
            strSql += " WHERE MIGRSWT <> 0   GROUP BY BAGNO "
            strSql += " )Y GROUP BY BAGNO"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = "UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET MIGRSWT ='0.00' WHERE MIGRSWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET MINETWT ='0.00' WHERE MINETWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET MIPUREWT ='0.00' WHERE MIPUREWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET MIAMOUNT ='0.00' WHERE MIAMOUNT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET MRGRSWT ='0.00' WHERE MRGRSWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET MRNETWT ='0.00' WHERE MRNETWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET MRPUREWT ='0.00' WHERE MRPUREWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET MRAMOUNT ='0.00' WHERE MRAMOUNT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET PIGRSWT ='0.00' WHERE PIGRSWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET PINETWT ='0.00' WHERE PINETWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET PIPUREWT ='0.00' WHERE PIPUREWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET PIAMOUNT ='0.00' WHERE PIAMOUNT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET PRGRSWT ='0.00' WHERE PRGRSWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET PRNETWT ='0.00' WHERE PRNETWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET PRPUREWT ='0.00' WHERE PRPUREWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET PRAMOUNT ='0.00' WHERE PRAMOUNT  IS NULL"
            strSql += "  UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET BGRSWT ='0.00' WHERE BGRSWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET BNETWT ='0.00' WHERE BNETWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET BPUREWT ='0.00' WHERE BPUREWT  IS NULL"
            strSql += " UPDATE TEMPTABLEDB..TEMPBAGNOWISE SET BAMOUNT ='0.00' WHERE BAMOUNT  IS NULL"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = "SELECT * FROM  TEMPTABLEDB..TEMPBAGNOWISE "
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Sub
            End If

            gridView.DataSource = dt
            With gridView
                .Columns("BAGNO").Width = 80
                If CkGrswt.Checked = True Then
                    .Columns("MIGRSWT").Width = 120
                    .Columns("MRGRSWT").Width = 120
                    .Columns("PIGRSWT").Width = 120
                    .Columns("PRGRSWT").Width = 120
                    .Columns("BGRSWT").Width = 120
                    .Columns("MIGRSWT").HeaderText = "GRSWT"
                    .Columns("MRGRSWT").HeaderText = "GRSWT"
                    .Columns("PIGRSWT").HeaderText = "GRSWT"
                    .Columns("PRGRSWT").HeaderText = "GRSWT"
                    .Columns("BGRSWT").HeaderText = "GRSWT"
                    .Columns("MIGRSWT").Visible = True
                    .Columns("MRGRSWT").Visible = True
                    .Columns("PIGRSWT").Visible = True
                    .Columns("PRGRSWT").Visible = True
                    .Columns("BGRSWT").Visible = True
                Else
                    .Columns("MIGRSWT").Visible = False
                    .Columns("MRGRSWT").Visible = False
                    .Columns("PIGRSWT").Visible = False
                    .Columns("PRGRSWT").Visible = False
                    .Columns("BGRSWT").Visible = False
                End If
                If ChkNetwt.Checked Then
                    .Columns("MINETWT").Width = 100
                    .Columns("MRNETWT").Width = 100
                    .Columns("PINETWT").Width = 100
                    .Columns("PRNETWT").Width = 100
                    .Columns("BNETWT").Width = 100
                    .Columns("MINETWT").HeaderText = "NETWT"
                    .Columns("MRNETWT").HeaderText = "NETWT"
                    .Columns("PINETWT").HeaderText = "NETWT"
                    .Columns("PRNETWT").HeaderText = "NETWT"
                    .Columns("BNETWT").HeaderText = "NETWT"
                    .Columns("MINETWT").Visible = True
                    .Columns("MRNETWT").Visible = True
                    .Columns("PINETWT").Visible = True
                    .Columns("PRNETWT").Visible = True
                    .Columns("BNETWT").Visible = True
                Else
                    .Columns("MINETWT").Visible = False
                    .Columns("MRNETWT").Visible = False
                    .Columns("PINETWT").Visible = False
                    .Columns("PRNETWT").Visible = False
                    .Columns("BNETWT").Visible = False
                End If
                If ChkPurewt.Checked Then
                    .Columns("MIPUREWT").Width = 100
                    .Columns("MRPUREWT").Width = 100
                    .Columns("PIPUREWT").Width = 100
                    .Columns("PRPUREWT").Width = 100
                    .Columns("BPUREWT").Width = 100
                    .Columns("MIPUREWT").HeaderText = "PUREWT"
                    .Columns("MRPUREWT").HeaderText = "PUREWT"
                    .Columns("PIPUREWT").HeaderText = "PUREWT"
                    .Columns("PRPUREWT").HeaderText = "PUREWT"
                    .Columns("BPUREWT").HeaderText = "PUREWT"
                    .Columns("MIPUREWT").Visible = True
                    .Columns("MRPUREWT").Visible = True
                    .Columns("PIPUREWT").Visible = True
                    .Columns("PRPUREWT").Visible = True
                    .Columns("BPUREWT").Visible = True
                Else
                    .Columns("MIPUREWT").Visible = False
                    .Columns("MRPUREWT").Visible = False
                    .Columns("PIPUREWT").Visible = False
                    .Columns("PRPUREWT").Visible = False
                    .Columns("BPUREWT").Visible = False
                End If
                If ChkAmount.Checked Then
                    .Columns("MIAMOUNT").Width = 100
                    .Columns("MRAMOUNT").Width = 100
                    .Columns("PIAMOUNT").Width = 100
                    .Columns("PRAMOUNT").Width = 100
                    .Columns("BAMOUNT").Width = 100
                    .Columns("MIAMOUNT").HeaderText = "AMOUNT"
                    .Columns("MRAMOUNT").HeaderText = "AMOUNT"
                    .Columns("PIAMOUNT").HeaderText = "AMOUNT"
                    .Columns("PRAMOUNT").HeaderText = "AMOUNT"
                    .Columns("BAMOUNT").HeaderText = "AMOUNT"
                    .Columns("MIAMOUNT").Visible = True
                    .Columns("MRAMOUNT").Visible = True
                    .Columns("PIAMOUNT").Visible = True
                    .Columns("PRAMOUNT").Visible = True
                    .Columns("BAMOUNT").Visible = True
                Else
                    .Columns("MIAMOUNT").Visible = False
                    .Columns("MRAMOUNT").Visible = False
                    .Columns("PIAMOUNT").Visible = False
                    .Columns("PRAMOUNT").Visible = False
                    .Columns("BAMOUNT").Visible = False
                End If
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                '.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            End With
            FormatGridColumns(gridView, False)

            Dim dtHead As New DataTable
            Dim strminetwt As String = ""
            Dim strmrnetwt As String = ""
            Dim strpinetwt As String = ""
            Dim strprnetwt As String = ""
            Dim strbnetwt As String = ""

            Dim strmipurewt As String = ""
            Dim strmrpurewt As String = ""
            Dim strpipurewt As String = ""
            Dim strprpurewt As String = ""
            Dim strbpurewt As String = ""

            Dim strmiamount As String = ""
            Dim strmramount As String = ""
            Dim strpiamount As String = ""
            Dim strpramount As String = ""
            Dim strbamount As String = ""

            If ChkNetwt.Checked Then strminetwt = "~MINETWT"
            If ChkNetwt.Checked Then strmrnetwt = "~MRNETWT"
            If ChkNetwt.Checked Then strpinetwt = "~PINETWT"
            If ChkNetwt.Checked Then strprnetwt = "~PRNETWT"
            If ChkNetwt.Checked Then strbnetwt = "~BNETWT"


            If ChkPurewt.Checked Then strmipurewt = "~MIPUREWT"
            If ChkPurewt.Checked Then strmrpurewt = "~MRPUREWT"
            If ChkPurewt.Checked Then strpipurewt = "~PIPUREWT"
            If ChkPurewt.Checked Then strprpurewt = "~PRPUREWT"
            If ChkPurewt.Checked Then strbpurewt = "~BPUREWT"

            If ChkAmount.Checked Then strmiamount = "~MIAMOUNT"
            If ChkAmount.Checked Then strmramount = "~MRAMOUNT"
            If ChkAmount.Checked Then strpiamount = "~PIAMOUNT"
            If ChkAmount.Checked Then strpramount = "~PRAMOUNT"
            If ChkAmount.Checked Then strbamount = "~BAMOUNT"

            strSql = "SELECT ''[BAGNO],''[MIGRSWT" & strminetwt & "" & strmipurewt & "" & strmiamount & "],''[MRGRSWT" & strmrnetwt & "" & strmrpurewt & "" & strmramount & "],''[PIGRSWT" & strpinetwt & "" & strpipurewt & "" & strpiamount & "],''[PRGRSWT" & strprnetwt & "" & strprpurewt & "" & strpramount & "],''[BGRSWT" & strbnetwt & "" & strbpurewt & "" & strbamount & "],''SCROLL"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtHead)
            gridViewHeader.DataSource = dtHead
            gridViewHeader.Columns("BAGNO").HeaderText = ""
            gridViewHeader.Columns("MIGRSWT" & strminetwt & "" & strmipurewt & "" & strmiamount & "").HeaderText = "MELT ISSUE"
            gridViewHeader.Columns("MRGRSWT" & strmrnetwt & "" & strmrpurewt & "" & strmramount & "").HeaderText = "MELT RECEIPT"
            gridViewHeader.Columns("PIGRSWT" & strpinetwt & "" & strpipurewt & "" & strpiamount & "").HeaderText = "PURIFY ISSUE"
            gridViewHeader.Columns("PRGRSWT" & strprnetwt & "" & strprpurewt & "" & strpramount & "").HeaderText = "PURIFY RECEIPT"
            gridViewHeader.Columns("BGRSWT" & strbnetwt & "" & strbpurewt & "" & strbamount & "").HeaderText = "DIFFERENCE"

            'gridViewHeader.Columns("BAMOUNT").HeaderText = ""
            gridViewHeader.Columns("SCROLL").HeaderText = ""
            gridViewHeader.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            'gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            SetGridHeadColWid()
            gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            Dim str As String = Nothing
            str = "Bagno wise Profit && Loss report from " & dtpFrom.Text & " To " & dtpTo.Text & ""
            If cmbMetal.Text <> "ALL" Then
                str += " for [" & cmbMetal.Text & "]"
            End If
            lblTitle.Text = str
            lblTitle.Visible = True
            gridView.Select()
            Prop_Sets()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            btnSearch.Enabled = True
        End Try
    End Sub
    Private Sub SetGridHeadColWid()
        If gridViewHeader Is Nothing Then Exit Sub
        If Not gridView.ColumnCount > 0 Then Exit Sub
        If Not gridViewHeader.ColumnCount > 0 Then Exit Sub
        Dim dtHead As New DataTable
        Dim strminetwt As String = ""
        Dim strmrnetwt As String = ""
        Dim strpinetwt As String = ""
        Dim strprnetwt As String = ""
        Dim strbnetwt As String = ""
        Dim strmiamount As String = ""
        Dim strmramount As String = ""
        Dim strpiamount As String = ""
        Dim strpramount As String = ""
        Dim strbamount As String = ""
        If ChkNetwt.Checked Then strminetwt = "~MINETWT"
        If ChkNetwt.Checked Then strmrnetwt = "~MRNETWT"
        If ChkNetwt.Checked Then strpinetwt = "~PINETWT"
        If ChkNetwt.Checked Then strprnetwt = "~PRNETWT"
        If ChkNetwt.Checked Then strbnetwt = "~BNETWT"

        If ChkAmount.Checked Then strmiamount = "~MIAMOUNT"
        If ChkAmount.Checked Then strmramount = "~MRAMOUNT"
        If ChkAmount.Checked Then strpiamount = "~PIAMOUNT"
        If ChkAmount.Checked Then strpramount = "~PRAMOUNT"
        If ChkAmount.Checked Then strbamount = "~BAMOUNT"

        Dim strmipurewt As String = ""
        Dim strmrpurewt As String = ""
        Dim strpipurewt As String = ""
        Dim strprpurewt As String = ""
        Dim strbpurewt As String = ""
        If ChkPurewt.Checked Then strmipurewt = "~MIPUREWT"
        If ChkPurewt.Checked Then strmrpurewt = "~MRPUREWT"
        If ChkPurewt.Checked Then strpipurewt = "~PIPUREWT"
        If ChkPurewt.Checked Then strprpurewt = "~PRPUREWT"
        If ChkPurewt.Checked Then strbpurewt = "~BPUREWT"

        With gridViewHeader
            .Columns("BAGNO").Width = gridView.Columns("BAGNO").Width
            .Columns("MIGRSWT" & strminetwt & "" & strmipurewt & "" & strmiamount & "").Width = IIf(gridView.Columns("MIGRSWT").Visible, gridView.Columns("MIGRSWT").Width, 0) + IIf(gridView.Columns("MINETWT").Visible, gridView.Columns("MINETWT").Width, 0) + IIf(gridView.Columns("MIPUREWT").Visible, gridView.Columns("MIPUREWT").Width, 0) + IIf(gridView.Columns("MIAMOUNT").Visible, gridView.Columns("MIAMOUNT").Width, 0)
            .Columns("MRGRSWT" & strmrnetwt & "" & strmrpurewt & "" & strmramount & "").Width = IIf(gridView.Columns("MRGRSWT").Visible, gridView.Columns("MRGRSWT").Width, 0) + IIf(gridView.Columns("MRNETWT").Visible, gridView.Columns("MRNETWT").Width, 0) + IIf(gridView.Columns("MRPUREWT").Visible, gridView.Columns("MRPUREWT").Width, 0) + IIf(gridView.Columns("MRAMOUNT").Visible, gridView.Columns("MRAMOUNT").Width, 0)
            .Columns("PIGRSWT" & strpinetwt & "" & strpipurewt & "" & strpiamount & "").Width = IIf(gridView.Columns("PIGRSWT").Visible, gridView.Columns("PIGRSWT").Width, 0) + IIf(gridView.Columns("PINETWT").Visible, gridView.Columns("PINETWT").Width, 0) + IIf(gridView.Columns("PIPUREWT").Visible, gridView.Columns("PIPUREWT").Width, 0) + IIf(gridView.Columns("PIAMOUNT").Visible, gridView.Columns("PIAMOUNT").Width, 0)
            .Columns("PRGRSWT" & strprnetwt & "" & strprpurewt & "" & strpramount & "").Width = IIf(gridView.Columns("PRGRSWT").Visible, gridView.Columns("PRGRSWT").Width, 0) + IIf(gridView.Columns("PRNETWT").Visible, gridView.Columns("PRNETWT").Width, 0) + IIf(gridView.Columns("PRPUREWT").Visible, gridView.Columns("PRPUREWT").Width, 0) + IIf(gridView.Columns("PRAMOUNT").Visible, gridView.Columns("PRAMOUNT").Width, 0)
            .Columns("BGRSWT" & strbnetwt & "" & strbpurewt & "" & strbamount & "").Width = IIf(gridView.Columns("BGRSWT").Visible, gridView.Columns("BGRSWT").Width, 0) + IIf(gridView.Columns("BNETWT").Visible, gridView.Columns("BNETWT").Width, 0) + IIf(gridView.Columns("BPUREWT").Visible, gridView.Columns("BPUREWT").Width, 0) + IIf(gridView.Columns("BAMOUNT").Visible, gridView.Columns("BAMOUNT").Width, 0)

            '.Columns("BAMOUNT").Width = gridView.Columns("BAMOUNT").Width
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            If colWid >= gridView.Width Then
                gridViewHeader.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                gridViewHeader.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                gridViewHeader.Columns("SCROLL").HeaderText = ""
            Else
                gridViewHeader.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        SetGridHeadColWid()
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHeader.HorizontalScrollingOffset = e.NewValue
                gridViewHeader.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                gridViewHeader.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmBagNoWiseProfitLossSummary_properties
        obj.p_ChkAmount = ChkAmount.Checked
        obj.p_ChkNetwt = ChkNetwt.Checked
        obj.p_ChkPurewt = ChkPurewt.Checked
        obj.p_CkGrswt = CkGrswt.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmBagNoWiseProfitLossSummary_properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmBagNoWiseProfitLossSummary_properties
        GetSettingsObj(obj, Me.Name, GetType(frmBagNoWiseProfitLossSummary_properties))
        ChkAmount.Checked = obj.p_ChkAmount
        ChkNetwt.Checked = obj.p_ChkNetwt
        ChkPurewt.Checked = obj.p_ChkPurewt
        CkGrswt.Checked = obj.p_CkGrswt
    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub
End Class
Public Class frmBagNoWiseProfitLossSummary_properties
    Private CkGrswt As Boolean = True
    Public Property p_CkGrswt() As Boolean
        Get
            Return CkGrswt
        End Get
        Set(ByVal value As Boolean)
            CkGrswt = value
        End Set
    End Property
    Private ChkNetwt As Boolean = True
    Public Property p_ChkNetwt() As Boolean
        Get
            Return ChkNetwt
        End Get
        Set(ByVal value As Boolean)
            ChkNetwt = value
        End Set
    End Property
    Private ChkPurewt As Boolean = True
    Public Property p_ChkPurewt() As Boolean
        Get
            Return ChkPurewt
        End Get
        Set(ByVal value As Boolean)
            ChkPurewt = value
        End Set
    End Property
    Private ChkAmount As Boolean = True
    Public Property p_ChkAmount() As Boolean
        Get
            Return ChkAmount
        End Get
        Set(ByVal value As Boolean)
            ChkAmount = value
        End Set
    End Property

End Class