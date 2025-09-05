Imports System.Data.OleDb
Public Class frmOrderReceipt
    Dim strSql As String
    Dim Cmd As OleDbCommand
    Dim dtGridView As New DataTable
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'OR_PICPATH'")

    Function funcNew() As Integer
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        rbtBoth.Checked = True
        txtOrderNo.Clear()
        cmbMetal.Text = "ALL"
        txtSalesMan_NUM.Clear()
        dtGridView.Rows.Clear()
        rbtOrder.Focus()
        gridView.DataSource = Nothing
        gridViewHeader1.DataSource = Nothing
    End Function
    Function funcStyleGridView() As Integer
        With gridView
            With .Columns("ORNO")
                .Width = 80
                '.DefaultCellStyle.BackColor = Color.OldLace
            End With
            With .Columns("ORDATE")
                .Width = 80
                '.DefaultCellStyle.BackColor = Color.OldLace
            End With
            With .Columns("DUEDATE")
                .Width = 80
                '.DefaultCellStyle.BackColor = Color.OldLace
            End With
            If .Columns.Contains("CUSTOMER") Then .Columns("CUSTOMER").Width = 175
            With .Columns("DESCRIPT")
                .Width = 195
                '.DefaultCellStyle.BackColor = Color.OldLace
            End With
            .Columns("STYLENO").Width = 80
            .Columns("PCS").Width = 60
            .Columns("PCS").DefaultCellStyle.BackColor = Color.OldLace
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            With .Columns("GRSWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.OldLace
            End With
            With .Columns("NETWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.OldLace
            End With
            With .Columns("DIAPCS")
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.OldLace
            End With
            With .Columns("DIAWT")
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.BackColor = Color.OldLace
            End With
            If .Columns.Contains("RATE") Then .Columns("RATE").Width = 70
            If .Columns.Contains("RATE") Then .Columns("RATE").DefaultCellStyle.BackColor = Color.OldLace
            If .Columns.Contains("RATE") Then .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            If .Columns.Contains("AMOUNT") Then
                With .Columns("AMOUNT")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.BackColor = Color.OldLace
                End With
            End If

            If .Columns.Contains("SALESMAN") Then
                With .Columns("SALESMAN")
                    .Width = 100
                    '.DefaultCellStyle.BackColor = Color.OldLace
                End With
            End If
            
            If .Columns.Contains("ADVWT") Then
                With .Columns("ADVWT")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.BackColor = Color.GhostWhite
                End With
            End If
            
            If .Columns.Contains("ADVAMT") Then
                With .Columns("ADVAMT")
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.BackColor = Color.GhostWhite
                End With
            End If
            
            If .Columns.Contains("USERNAME") Then
                With .Columns("USERNAME")
                    .HeaderText = "USERNAME"
                    .Width = 120
                End With
            End If
            If .Columns.Contains("COLHEAD") Then .Columns("COLHEAD").Visible = False
            If .Columns.Contains("BATCHNO") Then .Columns("BATCHNO").Visible = False
            If .Columns.Contains("TORDATE") Then .Columns("TORDATE").Visible = False
            If .Columns.Contains("TORDATE") Then .Columns("TORDATE").DefaultCellStyle.Format = "yyyy-MM-dd"
        End With
    End Function

    Private Sub frmOrderReceipt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmOrderReceipt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False, False)

        pnlHeading.Visible = False
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate
        dtpTo.MinimumDate = (New DateTimePicker).MinDate
        dtpTo.MaximumDate = (New DateTimePicker).MaxDate

        

        ''dtGridView
        'strSql = "  SELECT ORNO,''ORDATE,''DUEDATE,''CUSTOMER,DESCRIPT,STYLENO,PCS,GRSWT,NETWT,PCS AS DIAPCS,GRSWT AS DIAWT,RATE,ORVALUE AMOUNT,''SALESMAN,GRSWT ADVWT,ORVALUE ADVAMT,CONVERT(VARCHAR(150),NULL)USERNAME,CONVERT(VARCHAR(3),NULL)COLHEAD,CONVERT(VARCHAR(12),NULL)BATCHNO,CONVERT(SMALLDATETIME,NULL)TORDATE,CONVERT(vARCHAR(50),NULL)COSTNAME FROM " & cnadmindb & "..ORMAST "
        'strSql += " WHERE 1<>1"
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtGridView)
        'gridView.DataSource = dtGridView
        'funcStyleGridView()
        'gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        'gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        'With gridView
        '    gridViewHeader1.Columns("ORD").Width = .Columns("ORNO").Width + .Columns("ORDATE").Width + .Columns("DUEDATE").Width + .Columns("DESCRIPT").Width + .Columns("GRSWT").Width + .Columns("NETWT").Width + .Columns("AMOUNT").Width + .Columns("SALESMAN").Width
        '    gridViewHeader1.Columns("ORD").HeaderText = "ORDER"
        '    gridViewHeader1.Columns("ADVANCE").Width = .Columns("ADVWT").Width + .Columns("ADVAMT").Width
        '    gridViewHeader1.Columns("ADVANCE").HeaderText = "ADVANCE"
        'End With
       

        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        
    End Sub


    
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "X" Then
            btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "D" Then
            If Not gridView.RowCount > 0 Then Exit Sub
            If gridView.CurrentRow Is Nothing Then Exit Sub
            If gridView.CurrentRow.Cells("TORDATE").Value.ToString = "" Then Exit Sub
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim write As IO.StreamWriter
                write = IO.File.CreateText(Application.StartupPath & "\BillPrint.mem")
                write.WriteLine(LSet("TYPE", 15) & ":ORD")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & gridView.CurrentRow.Cells("BATCHNO").Value.ToString)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & gridView.CurrentRow.Cells("TORDATE").FormattedValue.ToString)
                write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                    LSet("TYPE", 15) & ":ORD" & ";" & _
                    LSet("BATCHNO", 15) & ":" & gridView.CurrentRow.Cells("BATCHNO").Value.ToString & ";" & _
                    LSet("TRANDATE", 15) & ":" & gridView.CurrentRow.Cells("TORDATE").FormattedValue.ToString & ";" & _
                    LSet("DUPLICATE", 15) & ":Y")
                End If

            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If
        ElseIf UCase(e.KeyChar) = "O" Then
            Dim TEMPORNO As String
            Dim TEMPORSNO As String
            strSql = "SELECT ORNO FROM " & cnAdminDb & "..ORMAST WHERE BATCHNO='" & gridView.CurrentRow.Cells("BATCHNO").Value.ToString & "' "
            TEMPORNO = GetSqlValue(cn, strSql)
            strSql = " SELECT SNO FROM " & cnAdminDb & "..ORMAST WHERE BATCHNO='" & gridView.CurrentRow.Cells("BATCHNO").Value.ToString & "'  "
            strSql += " AND SNO='" & gridView.CurrentRow.Cells("SNO").Value.ToString & "' "
            TEMPORSNO = GetSqlValue(cn, strSql)
            strSql = " EXEC " & cnAdminDb & "..SP_ORDERDETILS @DBNAME='" & cnStockDb & "',@ORNO='" & TEMPORNO & "',@ORSNO='" & TEMPORSNO & "'"
            Cmd = New OleDbCommand(strSql, cn)
            Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            Dim dtImage As New DataTable
            strSql = " SELECT ORNO,PICTFILE,SNO FROM MASTER..TEMPORDERITEM"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtImage)
            If Not dtImage.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            For Each ro As DataRow In dtImage.Rows
                Dim serverPath As String = Nothing
                Dim fileDestPath As String = defaultPic & "\" & ro!PiCTFILE.ToString
                If IO.File.Exists(fileDestPath) Then
                    Dim Finfo As IO.FileInfo
                    Finfo = New IO.FileInfo(fileDestPath)
                    Dim bmp As New Bitmap(Finfo.FullName)
                    Dim width As Integer = bmp.Width
                    Dim height As Integer = bmp.Height
                    Dim resizeimg As Boolean = False
                    If width > 3000 Then
                        width = 3000
                        resizeimg = True
                    End If
                    If height > 2400 Then
                        height = 2400
                        resizeimg = True
                    End If
                    bmp.Dispose()
                    If resizeimg = True Then
                        Dim fileName = Finfo.FullName
                        Dim CropRect As New Rectangle(0, 0, width, height)
                        Dim OrignalImage = Image.FromFile(fileName)
                        Dim CropImage = New Bitmap(CropRect.Width, CropRect.Height)
                        Using grp = Graphics.FromImage(CropImage)
                            grp.DrawImage(OrignalImage, New Rectangle(0, 0, CropRect.Width, CropRect.Height), CropRect, GraphicsUnit.Pixel)
                            OrignalImage.Dispose()
                            CropImage.Save(fileName)
                        End Using
                    End If

                    Finfo.IsReadOnly = False
                    If IO.Directory.Exists(Finfo.Directory.FullName) Then
                        Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, IO.FileAccess.Read)
                        Dim reader As New IO.BinaryReader(fileStr)
                        Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                        fileStr.Read(result, 0, result.Length)
                        fileStr.Close()
                        strSql = " UPDATE MASTER..TEMPORDERITEM SET ORIMAGE = ? WHERE ORNO = '" & ro!ORNO.ToString & "' and SNO='" & ro!SNO.ToString & "'"
                        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000
                        Cmd.Parameters.AddWithValue("@image", result)
                        Cmd.ExecuteNonQuery()
                    End If
                End If
            Next
            Dim objReport As New BrighttechReport
            Dim objRptViewer As New frmReportViewer
            objRptViewer.CrystalReportViewer1.ReportSource = objReport.ShowReport(New rptorderdetails, cnDataSource)
            objRptViewer.Dock = DockStyle.Fill
            objRptViewer.Show()
            objRptViewer.CrystalReportViewer1.Select()
        End If
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHeader1.HorizontalScrollingOffset = e.NewValue
                If gridViewHeader1.Columns.Count > 0 Then
                    If gridViewHeader1.Columns.Contains("SCROLL") Then
                        gridViewHeader1.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                        gridViewHeader1.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try

        'If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
        '    gridViewHeader1.HorizontalScrollingOffset = e.NewValue
        '    'gridViewHeader1.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
        '    'gridViewHeader1.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        'End If
    End Sub

    Private Function Receipt()
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Function
        strSql = "  IF OBJECT_ID('TEMPTABLEDB..TEMPORRECEIPT')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPORRECEIPT  "
        strSql += vbCrLf + "   SELECT RTRIM(SUBSTRING(ORNO,6,3) + ' ' + SUBSTRING(ORNO,9,LEN(ORNO)))AS ORNO"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR,ORDATE,103)ORDATE,CONVERT(VARCHAR,DUEDATE,103)DUEDATE"
        strSql += vbCrLf + "  ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = OM.BATCHNO))AS CUSTOMER"
        strSql += vbCrLf + "  ,UPPER((SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = OM.ITEMID)) as ITEMNAME, DESCRIPT AS DESCRIPT,OM.REASON ,STYLENO,PCS,GRSWT,NETWT"
        strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
        strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = OM.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
        strSql += vbCrLf + "  ,RATE,ORVALUE AMOUNT " + vbCrLf
        strSql += vbCrLf + "  ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = OM.EMPID)SALESMAN" + vbCrLf
        strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,3),NULL)ADVWT"
        strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),NULL)ADVAMT"
        strSql += vbCrLf + "  ,(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = OM.USERID)AS USERNAME"
        strSql += vbCrLf + "  ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = OM.COSTID)AS COSTNAME"
        strSql += vbCrLf + "  ,CONVERT(INT,1)AS RESULT,CONVERT(VARCHAR,NULL)COLHEAD,OM.BATCHNO,CONVERT(SMALLDATETIME,OM.ORDATE) TORDATE"
        strSql += vbCrLf + "  ,IDENTITY(INT,1,1)ROWID,ODBATCHNO,CONVERT(VARCHAR(15),SNO)SNO"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPORRECEIPT"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ORMAST AS OM" + vbCrLf
        strSql += vbCrLf + "  WHERE ORDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'" + vbCrLf
        If rbtOrder.Checked Then strSql += vbCrLf + "  AND ORTYPE = 'O'" + vbCrLf 'FOR ORDER ONLY
        If rbtRepair.Checked Then strSql += vbCrLf + "  AND ORTYPE = 'R'" + vbCrLf 'FOR REPAIR ONLY
        strSql += vbCrLf + "  AND ISNULL(OM.ORDCANCEL,'') = ''"
        If txtOrderNo.Text <> "" Then
            strSql += vbCrLf + "  AND SUBSTRING(ORNO,6,20) = '" & txtOrderNo.Text & "'"
        End If
        If txtSalesMan_NUM.Text <> "" Then
            strSql += vbCrLf + "  AND EMPID = " & Val(txtSalesMan_NUM.Text) & ""
        End If
        If rbtCustomer.Checked Then
            strSql += vbCrLf + "  AND ORMODE = 'C'"
        End If
        If rbtCompany.Checked Then
            strSql += vbCrLf + "  AND ORMODE = 'O'"
        End If
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            strSql += vbCrLf + "  AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'))"
        End If
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            strSql += vbCrLf + "  AND OM.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += vbCrLf + "  AND OM.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"

        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + "  and OM.COSTID in"
            strSql += vbCrLf + " (select CostId from " & cnAdminDb & "..CostCentre where CostName in (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        strSql += vbCrLf + "  AND ISNULL(OM.CANCEL,'') = ''"
        strSql += vbCrLf + "  ORDER BY om.ORDATE,convert(int,SUBSTRING(ORNO,9,LEN(ORNO)))"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = vbCrLf + " update TEMPTABLEDB..TEMPORRECEIPT SET ADVWT = X.WT,ADVAMT = X.AMT"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPORRECEIPT AS T"
        strSql += vbCrLf + " INNER JOIN "
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT (SELECT MIN(ROWID) FROM TEMPTABLEDB..TEMPORRECEIPT WHERE BATCHNO = OM.BATCHNO)AS  ROWID,OM.BATCHNO,SUM(O.GRSWT)WT, SUM(O.AMOUNT)AMT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O INNER JOIN (SELECT DISTINCT BATCHNO FROM TEMPTABLEDB..TEMPORRECEIPT) AS OM ON OM.BATCHNO = O.BATCHNO"
        strSql += vbCrLf + " GROUP BY OM.BATCHNO"
        strSql += vbCrLf + " )X ON X.BATCHNO = T.BATCHNO AND X.ROWID = T.ROWID"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        'STRSQL += VBCRLF + "  ,(SELECT sum(GRSWT) FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO = OM.BATCHNO)ADVWT" + vbCrLf
        'STRSQL += VBCRLF + "  ,(SELECT sum(AMOUNT) FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO = OM.BATCHNO)ADVAMT" + vbCrLf
        If chkpaymode.Checked Then
            'ADDING PAYMODE 
            strSql = "  IF OBJECT_ID('TEMPTABLEDB..TEMPORRECEIPTPAYMODE')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPORRECEIPTPAYMODE  "
            strSql += vbCrLf + "  SELECT SUM(A.AMOUNT)AMOUNT,"
            strSql += vbCrLf + "  CASE WHEN PAYMODE='CA' THEN 'CASH'"
            strSql += vbCrLf + "  WHEN PAYMODE='CC' THEN 'CREDITCARD' "
            strSql += vbCrLf + "  WHEN PAYMODE='PU' THEN 'OLD' "
            strSql += vbCrLf + "  WHEN PAYMODE='CH' THEN 'CHEQUE' END PAYMODE,A.BATCHNO"
            strSql += vbCrLf + "  ,(SELECT MIN(ROWID) FROM TEMPTABLEDB..TEMPORRECEIPT WHERE BATCHNO = A.BATCHNO)AS  ROWID"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPORRECEIPTPAYMODE "
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN A "
            strSql += vbCrLf + "  WHERE BATCHNO IN (SELECT OM.BATCHNO FROM " & cnAdminDb & "..OUTSTANDING AS O INNER JOIN (SELECT DISTINCT BATCHNO FROM TEMPTABLEDB..TEMPORRECEIPT) AS OM ON OM.BATCHNO = O.BATCHNO)"
            strSql += vbCrLf + "  AND TRANMODE='D'"
            strSql += vbCrLf + "  GROUP BY A.PAYMODE,A.BATCHNO "
            Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            strSql = "  DECLARE @PAYMODE AS VARCHAR(10)"
            strSql += vbCrLf + "  DECLARE @QRY VARCHAR(2000)"
            strSql += vbCrLf + "  SET @PAYMODE=''"
            strSql += vbCrLf + "  SET @QRY=''"
            strSql += vbCrLf + "  DECLARE CUR CURSOR FOR SELECT DISTINCT PAYMODE FROM TEMPTABLEDB..TEMPORRECEIPTPAYMODE where paymode <> ''"
            strSql += vbCrLf + "  OPEN CUR"
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  WHILE 1=1"
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  FETCH NEXT FROM CUR INTO @PAYMODE"
            strSql += vbCrLf + "  IF @@FETCH_STATUS=-1 BREAK"
            strSql += vbCrLf + "  SET @QRY=' ALTER TABLE TEMPTABLEDB..TEMPORRECEIPT ADD ['+ @PAYMODE +'] DECIMAL(15,2) 'PRINT @QRY EXEC(@QRY)"
            strSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET '+ @PAYMODE +'=(SELECT SUM(AMOUNT) FROM TEMPTABLEDB..TEMPORRECEIPTPAYMODE WHERE BATCHNO=TT.BATCHNO AND ROWID=TT.ROWID AND PAYMODE='''+ @PAYMODE +''')'"
            strSql += vbCrLf + "  SELECT @QRY= @QRY+' FROM TEMPTABLEDB..TEMPORRECEIPT TT WHERE BATCHNO=BATCHNO'"
            strSql += vbCrLf + "  PRINT @QRY "
            strSql += vbCrLf + "  EXEC(@QRY)"
            strSql += vbCrLf + "  END"
            strSql += vbCrLf + "  END"
            strSql += vbCrLf + "  CLOSE CUR"
            strSql += vbCrLf + "  DEALLOCATE CUR"
            Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        End If

        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORRECEIPT)>0"
        strSql += vbCrLf + "  BEGIN"
        strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMPORRECEIPT(DESCRIPT,PCS,GRSWT,NETWT,DIAPCS,DIAWT,AMOUNT,ADVWT,ADVAMT,RESULT,COLHEAD)"
        strSql += vbCrLf + "  SELECT 'GRAND TOTAL ('+CAST(COUNT(PCS) AS VARCHAR) +')',SUM(ISNULL(PCS,0))PCS,SUM(ISNULL(GRSWT,0))GRSWT,SUM(ISNULL(NETWT,0))NETWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT ,SUM(ISNULL(AMOUNT,0))AMOUNT"
        strSql += vbCrLf + "  ,SUM(ISNULL(ADVWT,0))AMOUNT,SUM(ISNULL(ADVAMT,0))AMOUNT"
        strSql += vbCrLf + "  ,2 RESULT,'G'COLHEAD"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPORRECEIPT T"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMPORRECEIPT SET ADVWT = CASE WHEN ADVWT = 0 THEN NULL ELSE ADVWT END"
        strSql += vbCrLf + "  ,ADVAMT = CASE WHEN ADVAMT = 0 THEN NULL ELSE ADVAMT END"
        strSql += vbCrLf + "  END"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPTABLEDB..TEMPORRECEIPT ORDER BY RESULT,convert(int,SUBSTRING(ORNO,9,LEN(ORNO)))"
        da = New OleDbDataAdapter(strSql, cn)
        dtGridView = New DataTable
        da.Fill(dtGridView)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            dtpFrom.Focus()
            Exit Function
        End If
        gridView.DataSource = dtGridView
        funcStyleGridView()
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Fillheadergridcolumn()
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView)
        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = reportTotalStyle
        gridView.Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
        gridView.Columns("RESULT").Visible = False
        gridView.Columns("ROWID").Visible = False
        gridView.Columns("ODBATCHNO").Visible = False
        gridView.Columns("SNO").Visible = False
        gridViewHeader1.Visible = True
        gridView.Focus()
    End Function

    Private Function Delivery()
        Dim chkCostName As String = ""
        Dim chkMetalName As String = ""
        Dim chkItemName As String = ""
        Dim chkDesigner As String = ""
        Dim chkStatus As String = ""

        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPORSTATUS' AND XTYPE = 'U') > 0 DROP TABLE TEMPTABLEDB..TEMPORSTATUS"
        strSql += " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPORSTATUS' AND XTYPE = 'V') > 0 DROP VIEW TEMPORSTATUS"
        Cmd = New OleDbCommand(strSql, cn)
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = " SELECT DISTINCT CONVERT(VARCHAR(500),NULL)PARTICULAR,O.STYLENO,SUBSTRING(O.ORNO,6,20)ORNO,ORDATE,REMDATE,O.DUEDATE"
        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST IM WHERE IM.ITEMID=O.ITEMID) + '-'+ O.DESCRIPT AS DESCRIPT,O.REASON"
        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST IM WHERE IM.SUBITEMID=O.SUBITEMID) AS SUBITEM"
        strSql += vbCrLf + " ,I.PCS,I.GRSWT,I.NETWT,I.MCGRM,I.MCHARGE AS MC,I.WASTPER,I.WASTAGE AS WAST"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE OS WHERE OS.ORSNO=O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE OS WHERE OS.ORSNO=O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        strSql += vbCrLf + " ,O.SIZENO"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(100),CASE WHEN ISNULL(O.ODBATCHNO,'') <> '' AND ISNULL(ISS.SNO,'')='' "
        strSql += vbCrLf + " THEN (SELECT TOP 1 ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = 5)"
        strSql += vbCrLf + " WHEN ISNULL(O.ODBATCHNO,'') <> '' AND ISNULL(ISS.SNO,'')<>'' THEN 'APPROVAL ISSUE'"
        strSql += vbCrLf + "   WHEN ISNULL(O.ORDCANCEL,'') <> '' THEN (SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = 6)"
        strSql += vbCrLf + "   ELSE ISNULL((SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = (SELECT TOP 1 ORDSTATE_ID FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ISNULL(CANCEL,'') = '' ORDER BY ENTRYORDER DESC)),'PENDING WITH US') "
        strSql += vbCrLf + "   END) AS STATUS"
        strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),O.RATE)AS ORRATE"
        'If chkAsOnDate.Checked = True Then
        '    strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),(SELECT SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND RUNNO = O.ORNO AND ISNULL(CANCEL,'') = ''))AS ADV_AMT"
        '    strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,3),(SELECT SUM(CASE WHEN RECPAY = 'P' THEN NETWT ELSE -1*NETWT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'AND RUNNO = O.ORNO  AND ISNULL(CANCEL,'') = ''))AS ADV_WT"
        'Else
        strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),(SELECT SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND RUNNO = O.ORNO AND ISNULL(CANCEL,'') = ''))AS ADV_AMT"
        strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,3),(SELECT SUM(CASE WHEN RECPAY = 'P' THEN NETWT ELSE -1*NETWT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND RUNNO = O.ORNO  AND ISNULL(CANCEL,'') = ''))AS ADV_WT"
        'End If
        strSql += vbCrLf + "  ,CONVERT(VARCHAR,P.PNAME + CASE WHEN ISNULL(INITIAL,'') <> '' THEN INITIAL ELSE '' END"
        strSql += vbCrLf + "  + CASE WHEN ISNULL(P.PHONERES,'') <> '' THEN CASE WHEN ISNULL(P.MOBILE,'') <> '' THEN ' ('+P.PHONERES+','+P.MOBILE+')' ELSE ' ('+P.PHONERES+')' END   ELSE CASE WHEN ISNULL(P.MOBILE,'') <> '' THEN ' ('+P.MOBILE+')' ELSE '' END END) AS PNAME "
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(20),CASE WHEN ISNULL(O.ODBATCHNO,'') <> '' THEN (SELECT TOP 1 TRANNO  FROM " & cnStockDb & "..ISSUE WHERE BATCHNO  = O.ODBATCHNO GROUP BY TRANNO) ELSE NULL END) DELIVERYNO"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(12),CASE WHEN ISNULL(O.ODBATCHNO,'') <> '' THEN (SELECT TOP 1 TRANDATE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO  = O.ODBATCHNO GROUP BY TRANDATE) ELSE NULL END,103) DELIVERYDATE"
        strSql += vbCrLf + " ,(SELECT  TOP 1 DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = (SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'I' AND ISNULL(CANCEL,'') = ''))DESIGNER"
        strSql += vbCrLf + "  ,(SELECT  TOP 1 COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = O.COSTID)AS COSTNAME"
        strSql += vbCrLf + "  ,(SELECT  TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = O.PROPSMITH)AS SMITHNAME"
        strSql += vbCrLf + "  ,O.BATCHNO"
        strSql += vbCrLf + "  ,(SELECT  TOP 1 EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = O.EMPID)AS EMPNAME"

        strSql += vbCrLf + "  ,CONVERT(vARCHAR(3),NULL)COLHEAD"
        strSql += vbCrLf + "  ,CONVERT(INT,1)RESULT"
        strSql += vbCrLf + "  ,CONVERT(INT,NULL)GRESULT,CONVERT(VARCHAR(15),O.SNO)SNO"
        strSql += vbCrLf + "  ,IDENTITY(INT,1,1)ROWID"
        strSql += vbCrLf + "  ,(SELECT TOP 1 TRANNO FROM " & cnAdminDb & "..ORIRDETAIL WHERE ISNULL(ORSNO,'') = ISNULL(O.SNO,'')) TRANNO"
        strSql += vbCrLf + "  ,(SELECT  TOP 1 ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID) COUNTER"
        strSql += vbCrLf + " ,O.ORNO as ORSNO"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPORSTATUS"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ORMAST O"
        strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..ISSUE I ON I.BATCHNO = O.ODBATCHNO AND I.SNO=O.ODSNO"
        'If txtCustomerName.Text <> "" Then
        '    strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CUSTOMERINFO CU ON CU.BATCHNO = O.BATCHNO"
        '    strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..PERSONALINFO AS P ON P.SNO = CU.PSNO"
        '    strSql += vbCrLf + " AND P.PNAME LIKE '" & txtCustomerName.Text & "%'"
        'Else
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..CUSTOMERINFO CU ON CU.BATCHNO = O.BATCHNO"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..PERSONALINFO AS P ON P.SNO = CU.PSNO"
        'End If
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..ITEMTAG AS T ON ISNULL(T.ORSNO,'') = O.SNO AND ISNULL(T.ORDREPNO,'') = O.ORNO"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..ISSUE ISS ON ISS.SNO =O.ODSNO AND ISNULL(ISS.TRANTYPE,'')='AI'"
        'strSql += vbCrLf + "  WHERE " & IIf(rbtOrderDate.Checked, "O.ORDATE", "O.DUEDATE") & " "
        'strSql += vbCrLf + "  WHERE O.ORDATE "
        strSql += vbCrLf + "  WHERE I.TRANDATE "
        'If chkAsOnDate.Checked Then
        '    strSql += vbCrLf + "  <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        'Else
        strSql += vbCrLf + "  BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        'End If
        If rbtOrder.Checked Then strSql += vbCrLf + " AND  O.ORTYPE='O'"
        If rbtRepair.Checked Then strSql += vbCrLf + " AND  O.ORTYPE='R'"
        'If rbtbooked.Checked Then strSql += vbCrLf + " AND  O.ORTYPE='B'"

        strSql += vbCrLf + "  AND ISNULL(O.ORDCANCEL,'') = ''"
        strSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(O.CANCEL,'') = ''"
        'If Not rbtTypeAll.Checked Then
        '    If rbtTypeCustomer.Checked Then strSql += vbCrLf + "  AND ORMODE = 'C'" Else strSql += vbCrLf + "  AND ORMODE = 'O'"
        'End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + "  AND   O.COSTID IN (SELECT  COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        If CmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            strSql += vbCrLf + "  AND O.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(cmbMetal.Text) & ")))"
        End If
        'If chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
        '    strSql += vbCrLf + "  AND O.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
        'End If
        'If chkcountercmb.Text <> "ALL" And chkcountercmb.Text <> "" Then
        '    strSql += vbCrLf + "  AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & GetQryString(chkcountercmb.Text) & "))"
        'End If
        'If cmbEmpName.Text <> "ALL" And cmbEmpName.Text <> "" Then strSql += vbCrLf + " AND O.EMPID = (SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME = '" & cmbEmpName.Text & "')"
        If txtSalesMan_NUM.Text <> "" Then
            strSql += vbCrLf + "  AND O.EMPID = " & Val(txtSalesMan_NUM.Text) & ""
        End If
        'If rbtCurrentRate.Checked = True Then
        '    strSql += vbCrLf + " AND O.ORRATE = 'C'"
        'ElseIf rbtDeliveryRate.Checked = True Then
        'strSql += vbCrLf + " AND O.ORRATE = 'D'"
        'End If
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()


        If chkpaymode.Checked Then
            'ADDING PAYMODE 
            strSql = "  IF OBJECT_ID('TEMPTABLEDB..TEMPORRECEIPTPAYMODE')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPORRECEIPTPAYMODE  "
            strSql += vbCrLf + "  SELECT SUM(A.AMOUNT)AMOUNT,"
            strSql += vbCrLf + "  CASE WHEN PAYMODE='CA' THEN 'CASH'"
            strSql += vbCrLf + "  WHEN PAYMODE='CC' THEN 'CREDITCARD' "
            strSql += vbCrLf + "  WHEN PAYMODE='PU' THEN 'OLD' "
            strSql += vbCrLf + "  WHEN PAYMODE='CH' THEN 'CHEQUE' END PAYMODE,A.BATCHNO"
            strSql += vbCrLf + "  ,(SELECT MIN(ROWID) FROM TEMPTABLEDB..TEMPORSTATUS WHERE BATCHNO = A.BATCHNO)AS  ROWID"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPORRECEIPTPAYMODE "
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN A "
            strSql += vbCrLf + "  WHERE BATCHNO IN (SELECT OM.BATCHNO FROM " & cnAdminDb & "..OUTSTANDING AS O "
            strSql += vbCrLf + "  INNER JOIN (SELECT DISTINCT BATCHNO FROM TEMPTABLEDB..TEMPORSTATUS) AS OM ON OM.BATCHNO = O.BATCHNO)"
            strSql += vbCrLf + "  And TRANMODE='D'"
            strSql += vbCrLf + "  GROUP BY A.PAYMODE,A.BATCHNO "
            Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            strSql = "  DECLARE @PAYMODE AS VARCHAR(10)"
            strSql += vbCrLf + "  DECLARE @QRY VARCHAR(2000)"
            strSql += vbCrLf + "  SET @PAYMODE=''"
            strSql += vbCrLf + "  SET @QRY=''"
            strSql += vbCrLf + "  DECLARE CUR CURSOR FOR SELECT DISTINCT PAYMODE FROM TEMPTABLEDB..TEMPORRECEIPTPAYMODE where paymode <> ''"
            strSql += vbCrLf + "  OPEN CUR"
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  WHILE 1=1"
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  FETCH NEXT FROM CUR INTO @PAYMODE"
            strSql += vbCrLf + "  IF @@FETCH_STATUS=-1 BREAK"
            strSql += vbCrLf + "  SET @QRY=' ALTER TABLE TEMPTABLEDB..TEMPORSTATUS ADD ['+ @PAYMODE +'] DECIMAL(15,2) 'PRINT @QRY EXEC(@QRY)"
            strSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET '+ @PAYMODE +'=(SELECT SUM(AMOUNT) FROM TEMPTABLEDB..TEMPORRECEIPTPAYMODE WHERE BATCHNO=TT.BATCHNO AND ROWID=TT.ROWID AND PAYMODE='''+ @PAYMODE +''')'"
            strSql += vbCrLf + "  SELECT @QRY= @QRY+' FROM TEMPTABLEDB..TEMPORSTATUS TT WHERE BATCHNO=BATCHNO'"
            strSql += vbCrLf + "  PRINT @QRY "
            strSql += vbCrLf + "  EXEC(@QRY)"
            strSql += vbCrLf + "  END"
            strSql += vbCrLf + "  END"
            strSql += vbCrLf + "  CLOSE CUR"
            strSql += vbCrLf + "  DEALLOCATE CUR"
            Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        End If


        strSql = " UPDATE TEMPTABLEDB..TEMPORSTATUS SET ADV_WT = NULL,ADV_AMT = NULL"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPORSTATUS AS T"
        strSql += vbCrLf + " WHERE ROWID NOT IN (SELECT MIN(ROWID) FROM TEMPTABLEDB..TEMPORSTATUS GROUP BY ORNO)"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        strSql = " IF (sELECT COUNT(*) FROM TEMPTABLEDB..TEMPORSTATUS)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTATUS"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " DESCRIPT,COLHEAD,RESULT,PCS,GRSWT,NETWT,MCGRM,MC,WASTPER,WAST,STNPCS,STNWT,DIAPCS,DIAWT,ADV_AMT,ADV_WT"
        strSql += vbCrLf + " )"
        strSql += vbCrLf + " SELECT 'GRAND TOTAL ('+CAST(COUNT(PCS) AS VARCHAR) +')','G',2 RESULT,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(MCGRM),SUM(MC),SUM(WASTPER),SUM(WAST),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(ISNULL(ADV_AMT,0)),SUM(ISNULL(ADV_WT,0)) FROM TEMPTABLEDB..TEMPORSTATUS WHERE RESULT = 1"
        strSql += vbCrLf + " END"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPTABLEDB..TEMPORSTATUS ORDER BY RESULT,convert(int,SUBSTRING(ORNO,4,LEN(ORNO)))"
        da = New OleDbDataAdapter(strSql, cn)
        dtGridView = New DataTable
        da.Fill(dtGridView)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            dtpFrom.Focus()
            Exit Function
        End If
        gridView.DataSource = dtGridView
        funcStyleGridView()
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'Fillheadergridcolumn()
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView)
        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = reportTotalStyle
        gridView.Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
        gridView.Columns("PARTICULAR").Visible = False
        gridView.Columns("STYLENO").Visible = False
        gridView.Columns("STATUS").Visible = False
        gridView.Columns("ADV_AMT").Visible = False
        gridView.Columns("ADV_WT").Visible = False

        gridView.Columns("SMITHNAME").Visible = False
        'gridView.Columns("COSTNAME").Visible = False
        gridView.Columns("GRESULT").Visible = False

        gridView.Columns("PNAME").HeaderText = "CUSTOMER"
        gridView.Columns("DESCRIPT").HeaderText = "DESCRIPTION"

        gridView.Columns("RESULT").Visible = False
        gridView.Columns("ROWID").Visible = False
        gridView.Columns("SNO").Visible = False
        If gridView.Columns.Contains("ORRATE") Then gridView.Columns("ORRATE").HeaderText = "RATE"
        If gridView.Columns.Contains("ORSNO") Then gridView.Columns("ORSNO").HeaderText = "SNO"

        If gridView.Columns.Contains("ORDATE") Then gridView.Columns("ORDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        If gridView.Columns.Contains("DUEDATE") Then gridView.Columns("DUEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        If gridView.Columns.Contains("REMDATE") Then gridView.Columns("REMDATE").DefaultCellStyle.Format = "dd/MM/yyyy"

        gridViewHeader1.DataSource = Nothing
        gridViewHeader1.Visible = False
        gridView.Focus()

    End Function

    Private Function Pending()
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Function
        Dim chkCostName As String = ""
        Dim chkMetalName As String = ""
        Dim chkItemName As String = ""
        Dim chkDesigner As String = ""
        Dim chkStatus As String = ""

        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPORSTATUS' AND XTYPE = 'U') > 0 DROP TABLE TEMPTABLEDB..TEMPORSTATUS"
        strSql += " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPORSTATUS' AND XTYPE = 'V') > 0 DROP VIEW TEMPORSTATUS"
        Cmd = New OleDbCommand(strSql, cn)
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = " SELECT DISTINCT CONVERT(VARCHAR(500),NULL)PARTICULAR,O.STYLENO,SUBSTRING(O.ORNO,6,20)ORNO,ORDATE,REMDATE,O.DUEDATE"
        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST IM WHERE IM.ITEMID=O.ITEMID) + '-'+ O.DESCRIPT AS DESCRIPT"
        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST IM WHERE IM.SUBITEMID=O.SUBITEMID) AS SUBITEM"
        strSql += vbCrLf + " ,O.PCS,O.GRSWT,O.NETWT,O.MCGRM,O.MC,O.WASTPER,O.WAST"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE OS WHERE OS.ORSNO=O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE OS WHERE OS.ORSNO=O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        strSql += vbCrLf + " ,O.SIZENO"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(100),CASE WHEN ISNULL(O.ODBATCHNO,'') <> '' AND ISNULL(ISS.SNO,'')='' "
        strSql += vbCrLf + " And Isnull((Select top 1 trandate from " & cnStockDb & "..issue where isnull(CANCEL,'')='' and batchno=o.odbatchno),'')<='" & IIf(chkAsOnDate.Checked = True, dtpFrom.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " THEN (SELECT TOP 1 ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = 5)"
        strSql += vbCrLf + " WHEN ISNULL(O.ODBATCHNO,'') <> '' AND ISNULL(ISS.SNO,'')<>'' THEN 'APPROVAL ISSUE'"
        strSql += vbCrLf + "   WHEN ISNULL(O.ORDCANCEL,'') <> '' THEN (SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = 6)"
        strSql += vbCrLf + "   ELSE ISNULL((SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = (SELECT TOP 1 ORDSTATE_ID FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ISNULL(CANCEL,'') = '' ORDER BY ENTRYORDER DESC)),'PENDING WITH US') "
        strSql += vbCrLf + "   END) AS STATUS,O.REASON"

        strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),O.RATE)AS ORRATE"
        'CALNO 051212
        If chkAsOnDate.Checked = True Then
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),(SELECT SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND RUNNO = O.ORNO AND ISNULL(CANCEL,'') = ''))AS ADV_AMT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,3),(SELECT SUM(CASE WHEN RECPAY = 'P' THEN GRSWT ELSE -1*GRSWT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'AND RUNNO = O.ORNO  AND ISNULL(CANCEL,'') = ''))AS ADV_GRSWT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,3),(SELECT SUM(CASE WHEN RECPAY = 'P' THEN NETWT ELSE -1*NETWT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'AND RUNNO = O.ORNO  AND ISNULL(CANCEL,'') = ''))AS ADV_NETWT"
        Else
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),(SELECT SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1*AMOUNT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND RUNNO = O.ORNO AND ISNULL(CANCEL,'') = ''))AS ADV_AMT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,3),(SELECT SUM(CASE WHEN RECPAY = 'P' THEN GRSWT ELSE -1*GRSWT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND RUNNO = O.ORNO  AND ISNULL(CANCEL,'') = ''))AS ADV_GRSWT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,3),(SELECT SUM(CASE WHEN RECPAY = 'P' THEN NETWT ELSE -1*NETWT END) FROM " & cnAdminDb & "..OUTSTANDING WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND RUNNO = O.ORNO  AND ISNULL(CANCEL,'') = ''))AS ADV_NETWT"
        End If
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(150),P.PNAME + CASE WHEN ISNULL(INITIAL,'') <> '' THEN INITIAL ELSE '' END"
        strSql += vbCrLf + "  + CASE WHEN ISNULL(P.PHONERES,'') <> '' THEN CASE WHEN ISNULL(P.MOBILE,'') <> '' THEN ' ('+P.PHONERES+','+P.MOBILE+')' ELSE ' ('+P.PHONERES+')' END   ELSE CASE WHEN ISNULL(P.MOBILE,'') <> '' THEN ' ('+P.MOBILE+')' ELSE '' END END) AS PNAME "
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(20),CASE WHEN ISNULL(O.ODBATCHNO,'') <> '' THEN (SELECT TOP 1 TRANNO  FROM " & cnStockDb & "..ISSUE WHERE BATCHNO  = O.ODBATCHNO GROUP BY TRANNO) ELSE NULL END) DELIVERYNO"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(12),CASE WHEN ISNULL(O.ODBATCHNO,'') <> '' THEN (SELECT TOP 1 TRANDATE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO  = O.ODBATCHNO GROUP BY TRANDATE) ELSE NULL END,103) DELIVERYDATE"
        strSql += vbCrLf + " ,(SELECT  TOP 1 DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = (SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'I' AND ISNULL(CANCEL,'') = ''))DESIGNER"
        strSql += vbCrLf + "  ,(SELECT  TOP 1 COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = O.COSTID)AS COSTNAME"
        strSql += vbCrLf + "  ,(SELECT  TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = O.PROPSMITH)AS SMITHNAME"
        strSql += vbCrLf + "  ,O.BATCHNO"
        strSql += vbCrLf + "  ,(SELECT  TOP 1 EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = O.EMPID)AS EMPNAME"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(3),NULL)COLHEAD"
        strSql += vbCrLf + "  ,CONVERT(INT,1)RESULT"
        strSql += vbCrLf + "  ,CONVERT(INT,NULL)GRESULT,CONVERT(VARCHAR(15),O.SNO)SNO"
        strSql += vbCrLf + "  ,IDENTITY(INT,1,1)ROWID"
        strSql += vbCrLf + "  ,(SELECT TOP 1 TRANNO FROM " & cnAdminDb & "..ORIRDETAIL WHERE ISNULL(ORSNO,'') = ISNULL(O.SNO,'')) TRANNO"
        strSql += vbCrLf + "  ,(SELECT  TOP 1 ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID) COUNTER"
        strSql += vbCrLf + "  ,O.ORNO as ORSNO"
        'Newly add'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        strSql += vbCrLf + "  ,CMP.COMPANYNAME ,P.MOBILE AS MOBILENO , P.PNAME AS CUST_NAME"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(150), NULL)SUMMARY"
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPORSTATUS"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ORMAST O"
        'If txtCustomerName.Text <> "" Then
        '    strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CUSTOMERINFO CU ON CU.BATCHNO = O.BATCHNO"
        '    strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..PERSONALINFO AS P ON P.SNO = CU.PSNO"
        '    strSql += vbCrLf + " AND P.PNAME LIKE '" & txtCustomerName.Text & "%'"
        'Else
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..CUSTOMERINFO CU ON CU.BATCHNO = O.BATCHNO"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..PERSONALINFO AS P ON P.SNO = CU.PSNO"
        'End If
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..ITEMTAG AS T ON ISNULL(T.ORSNO,'') = O.SNO AND ISNULL(T.ORDREPNO,'') = O.ORNO"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..ISSUE ISS ON ISS.SNO =O.ODSNO AND ISNULL(ISS.TRANTYPE,'')='AI'"
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..COMPANY AS CMP ON CMP.COMPANYID = O.COMPANYID"
        'strSql += vbCrLf + " LEFT OUTER JOIN " & cnadmindb & "..ORIRDETAIL AS I ON ISNULL(I.ORSNO,'') = ISNULL(O.SNO,'')"
        strSql += vbCrLf + "  WHERE O.ORDATE "
        If chkAsOnDate.Checked Then
            strSql += vbCrLf + "  <= '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + "  BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        'strSql += vbCrLf + "  AND ISNULL(O.ORDCANCEL,'') = ''"
        strSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(O.CANCEL,'') = ''"

        If rbtOrder.Checked Then
            strSql += vbCrLf + " AND O.ORTYPE = 'O'"
        ElseIf rbtRepair.Checked Then
            strSql += vbCrLf + " AND O.ORTYPE = 'R'"
        End If
        If Not rbtBoth.Checked Then
            If rbtCustomer.Checked Then strSql += vbCrLf + "  AND ORMODE = 'C'" Else strSql += vbCrLf + "  AND ORMODE = 'O'"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + "  AND   O.COSTID IN (SELECT  COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            strSql += vbCrLf + "  AND O.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(cmbMetal.Text) & ")))"
        End If
        If txtSalesMan_NUM.Text <> "ALL" And txtSalesMan_NUM.Text <> "" Then strSql += vbCrLf + " AND O.EMPID = " & txtSalesMan_NUM.Text.ToString & " "

        'strSql += vbCrLf + "  AND ISNULL(T.ORDREPNO,'') != '' AND ISNULL(T.ORSNO,'') != ''"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        strSql = " DELETE FROM TEMPTABLEDB..TEMPORSTATUS WHERE"
        strSql += vbCrLf + "  STATUS NOT IN ('PENDING WITH US')"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        strSql = " UPDATE TEMPTABLEDB..TEMPORSTATUS SET ADV_GRSWT = NULL,ADV_NETWT = NULL,ADV_AMT = NULL"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPORSTATUS AS T"
        strSql += vbCrLf + " WHERE ROWID NOT IN (SELECT MIN(ROWID) FROM TEMPTABLEDB..TEMPORSTATUS GROUP BY ORNO)"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        'Total
        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPORSTATUS)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPORSTATUS"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " DESCRIPT,COLHEAD,GRESULT,PCS,GRSWT,NETWT,MCGRM,MC,WASTPER,WAST,STNPCS,STNWT,DIAPCS,DIAWT,ADV_AMT,ADV_GRSWT,ADV_NETWT"
        strSql += vbCrLf + " )"
        strSql += vbCrLf + " SELECT 'GRAND TOTAL ('+CAST(COUNT(PCS) AS VARCHAR) +')'"
        strSql += vbCrLf + " ,'G',1 GRESULT,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(MCGRM),SUM(MC),SUM(WASTPER),SUM(WAST),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(ISNULL(ADV_AMT,0)),SUM(ISNULL(ADV_GRSWT,0)),SUM(ISNULL(ADV_NETWT,0)) FROM TEMPTABLEDB..TEMPORSTATUS WHERE RESULT = 1"
        strSql += vbCrLf + " END"
        Cmd = New OleDbCommand(strSql, cn)
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        strSql = " SELECT * FROM TEMPTABLEDB..TEMPORSTATUS"
        strSql += vbCrLf + "  ORDER BY GRESULT "
        strSql += " ,convert(int,SUBSTRING(ORNO,4,LEN(ORNO)))"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpFrom.Select()
            Exit Function
        End If
        gridView.DataSource = dtGrid
        funcStyleGridView()
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'Fillheadergridcolumn()
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView)
        gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = reportTotalStyle
        gridView.Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
        gridView.Columns("PARTICULAR").Visible = False
        gridView.Columns("STYLENO").Visible = False
        gridView.Columns("STATUS").Visible = False
        'gridView.Columns("ADV_AMT").Visible = False
        gridView.Columns("ADV_AMT").Visible = True
        gridView.Columns("ADV_AMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("ADV_AMT").DefaultCellStyle.Format = "0.00"
        gridView.Columns("ADV_AMT").HeaderText = "AMOUNT"
        gridView.Columns("ADV_GRSWT").Visible = False
        gridView.Columns("ADV_NETWT").Visible = False
        gridView.Columns("SMITHNAME").Visible = False
        'gridView.Columns("COSTNAME").Visible = False
        gridView.Columns("GRESULT").Visible = False

        gridView.Columns("MOBILENO").Visible = False
        gridView.Columns("CUST_NAME").Visible = False
        gridView.Columns("COMPANYNAME").Visible = False

        gridView.Columns("DELIVERYNO").Visible = False
        gridView.Columns("DELIVERYDATE").Visible = False
        gridView.Columns("SUMMARY").Visible = False

        gridView.Columns("PNAME").HeaderText = "CUSTOMER"
        gridView.Columns("DESCRIPT").HeaderText = "DESCRIPTION"

        gridView.Columns("RESULT").Visible = False
        gridView.Columns("ROWID").Visible = False
        gridView.Columns("SNO").Visible = False
        gridView.Columns("ORNO").HeaderText = "NO"
        gridView.Columns("ORDATE").HeaderText = "DATE"
        gridView.Columns("ORRATE").HeaderText = "RATE"
        gridView.Columns("ORSNO").HeaderText = "SNO"

        gridViewHeader1.DataSource = Nothing
        gridViewHeader1.Visible = False
        If gridView.Columns.Contains("ORDATE") Then gridView.Columns("ORDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        If gridView.Columns.Contains("DUEDATE") Then gridView.Columns("DUEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        If gridView.Columns.Contains("REMDATE") Then gridView.Columns("REMDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        gridView.Focus()

    End Function

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        dtGridView.Rows.Clear()
        If rbtReceipt.Checked Then
            Receipt()
        ElseIf rbtDelivery.Checked Then
            Delivery()
        ElseIf rbtPending.Checked Then
            Pending()
        Else
            MsgBox("Please select any one Type...")
            Exit Sub
        End If
        Prop_Sets()
    End Sub



    Private Sub Fillheadergridcolumn()
        Dim colname As String = ""
        Dim colwidth As Integer = 0
        Dim dtpay As DataTable
        If chkpaymode.Checked Then
            dtpay = GetSqlTable("SELECT DISTINCT PAYMODE FROM TEMPTABLEDB..TEMPORRECEIPTPAYMODE where paymode <> '' ", cn)
            If dtpay.Rows.Count > 0 Then
                colname += "["
                For i As Integer = 0 To dtpay.Rows.Count - 1
                    colname += dtpay.Rows(i).Item("PAYMODE").ToString
                    If i < dtpay.Rows.Count - 1 Then colname += "~"
                    If i >= dtpay.Rows.Count - 1 Then colname += "]"
                    colwidth += gridView.Columns(dtpay.Rows(i).Item("PAYMODE").ToString).Width
                Next
            Else
                colname = "[NOTHING]"
            End If
        End If

        gridViewHeader1.BackgroundColor = gridView.BackgroundColor
        ''GridViewHeader1
        strSql = " SELECT"
        strSql += "''[ORNO~ORDATE~DUEDATE~CUSTOMER~ITEMNAME~DESCRIPT~STYLENO],''[PCS~GRSWT~NETWT~DIAPCS~DIAWT~RATE~AMOUNT],''[SALESMAN]"
        strSql += ",''[ADVWT~ADVAMT],''[USERNAME~COSTNAME]"
        If chkpaymode.Checked Then strSql += ",''" & colname & ""
        strSql += ",''[SCROLL] WHERE 1<>1"
        'strSql = " SELECT 'ORDER'ORD,'ADVANCE'ADVANCE,'USERNAME'USERNAME WHERE 1<>1"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridViewHeader1.DataSource = dt
        gridViewHeader1.Size = New System.Drawing.Size(1, gridViewHeader1.ColumnHeadersHeight)
        gridViewHeader1.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridViewHeader1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        With gridView
            gridViewHeader1.Columns("ORNO~ORDATE~DUEDATE~CUSTOMER~ITEMNAME~DESCRIPT~STYLENO").Width =
            .Columns("ORNO").Width _
            + .Columns("ORDATE").Width _
            + .Columns("DUEDATE").Width +
            .Columns("CUSTOMER").Width +
            .Columns("DESCRIPT").Width +
            .Columns("STYLENO").Width
            gridViewHeader1.Columns("PCS~GRSWT~NETWT~DIAPCS~DIAWT~RATE~AMOUNT").Width =
            .Columns("PCS").Width +
            .Columns("GRSWT").Width +
            .Columns("NETWT").Width +
            .Columns("DIAPCS").Width +
            .Columns("DIAWT").Width +
            .Columns("RATE").Width +
            .Columns("AMOUNT").Width
            gridViewHeader1.Columns("SALESMAN").Width = .Columns("SALESMAN").Width
            gridViewHeader1.Columns("ORNO~ORDATE~DUEDATE~CUSTOMER~ITEMNAME~DESCRIPT~STYLENO").HeaderText = ""
            gridViewHeader1.Columns("PCS~GRSWT~NETWT~DIAPCS~DIAWT~RATE~AMOUNT").HeaderText = IIf(rbtOrder.Checked, "ORDER", "REPAIR")
            gridViewHeader1.Columns("SALESMAN").HeaderText = ""
            gridViewHeader1.Columns("ADVWT~ADVAMT").Width =
            .Columns("ADVWT").Width _
            + .Columns("ADVAMT").Width
            gridViewHeader1.Columns("USERNAME~COSTNAME").Width =
             .Columns("USERNAME").Width _
            + .Columns("COSTNAME").Width
            If chkpaymode.Checked Then
                colname = colname.Replace("[", "")
                colname = colname.Replace("]", "")
                If colname <> "NOTHING" Then
                    gridViewHeader1.Columns(colname).Width = colwidth
                    gridViewHeader1.Columns(colname).HeaderText = "PAYMODE"
                Else
                    gridViewHeader1.Columns(colname).Width = 0
                    gridViewHeader1.Columns(colname).HeaderText = ""
                    gridViewHeader1.Columns(colname).Visible = False
                End If

            End If
            gridViewHeader1.Columns("ADVWT~ADVAMT").HeaderText = ""
            gridViewHeader1.Columns("USERNAME~COSTNAME").HeaderText = ""
            gridViewHeader1.Columns("SCROLL").HeaderText = ""
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            If colWid >= gridView.Width Then
                gridViewHeader1.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                gridViewHeader1.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                gridViewHeader1.Columns("SCROLL").HeaderText = ""
            Else
                gridViewHeader1.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub


    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        Dim Title As String = Nothing
        If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then Title += cmbMetal.Text
        Title += IIf(rbtOrder.Checked, " ORDER ", "REPAIR")
        If rbtReceipt.Checked Then
            Title += " RECEIPT "
        ElseIf rbtDelivery.Checked Then
            Title += " DELIVERY "
        Else
            Title += " PENDING "
        End If
        If chkAsOnDate.Checked Then
            Title += " AS ON " & dtpFrom.Text
        Else
            Title += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        End If
        Title += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, Title, gridView, BrightPosting.GExport.GExportType.Print, gridViewHeader1)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
        Prop_Gets()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            Dim Title As String = Nothing
            If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then Title += cmbMetal.Text
            Title += IIf(rbtOrder.Checked, " ORDER ", "REPAIR")
            If rbtReceipt.Checked Then
                Title += " RECEIPT "
            ElseIf rbtDelivery.Checked Then
                Title += " DELIVERY "
            Else
                Title += " PENDING "
            End If
            If chkAsOnDate.Checked Then
                Title += " AS ON " & dtpFrom.Text
            Else
                Title += " FROM " & dtpFrom.Text & " TO " & dtpTo.Text
            End If
            Title += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
            BrightPosting.GExport.Post(Me.Name, strCompanyName, Title, gridView, BrightPosting.GExport.GExportType.Export, gridViewHeader1)
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmOrderReceipt_Properties
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        obj.p_txtOrderNo = txtOrderNo.Text
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_txtSalesMan_NUM = txtSalesMan_NUM.Text
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_rbtCustomer = rbtCustomer.Checked
        obj.p_rbtCompany = rbtCompany.Checked

        obj.p_chkAsOnDate = chkAsOnDate.Checked
        obj.p_rbtReceipt = rbtReceipt.Checked
        obj.p_rbtDelivery = rbtDelivery.Checked
        obj.p_rbtPending = rbtPending.Checked

        SetSettingsObj(obj, Me.Name, GetType(frmOrderReceipt_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmOrderReceipt_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmOrderReceipt_Properties))
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        txtOrderNo.Text = obj.p_txtOrderNo
        cmbMetal.Text = obj.p_cmbMetal
        txtSalesMan_NUM.Text = obj.p_txtSalesMan_NUM
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, cnCostName)
        rbtBoth.Checked = obj.p_rbtBoth
        rbtCustomer.Checked = obj.p_rbtCustomer
        rbtCompany.Checked = obj.p_rbtCompany
        chkAsOnDate.Checked = obj.p_chkAsOnDate
        rbtReceipt.Checked = obj.p_rbtReceipt
        rbtDelivery.Checked = obj.p_rbtDelivery
        rbtPending.Checked = obj.p_rbtPending
    End Sub

    Private Sub rbtOrder_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtOrder.CheckedChanged
        If rbtOrder.Checked Then lblOrder.Text = "Order No"
    End Sub

    Private Sub rbtRepair_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtRepair.CheckedChanged
        If rbtRepair.Checked Then lblOrder.Text = "Repair No"
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        If chkAsOnDate.Checked Then
            lblTo.Visible = False
            dtpTo.Visible = False
            chkAsOnDate.Text = "As On"
            rbtPending.Checked = True
        Else
            lblTo.Visible = True
            dtpTo.Visible = True
            chkAsOnDate.Text = "From Date"
        End If
    End Sub


    Private Sub rbtReceipt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtReceipt.CheckedChanged
        If rbtReceipt.Checked Then
            lblTo.Visible = True
            dtpTo.Visible = True
            chkAsOnDate.Text = "From Date"
            chkAsOnDate.Enabled = False
            chkAsOnDate.Checked = False
            chkpaymode.Enabled = True
            chkpaymode.Checked = False
            chkpaymode.Text = "With Paymode"
        End If
    End Sub

    Private Sub rbtDelivery_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtDelivery.CheckedChanged
        If rbtDelivery.Checked Then
            lblTo.Visible = True
            dtpTo.Visible = True
            chkAsOnDate.Text = "From Date"
            chkAsOnDate.Enabled = False
            chkAsOnDate.Checked = False
            chkpaymode.Enabled = True
            chkpaymode.Checked = False
            chkpaymode.Text = "With Receipt Paymode"
        End If
    End Sub

    Private Sub rbtPending_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtPending.CheckedChanged
        If rbtPending.Checked Then
            lblTo.Visible = False
            dtpTo.Visible = False
            chkAsOnDate.Text = "As On"
            chkAsOnDate.Enabled = True
            chkpaymode.Enabled = False
            chkpaymode.Checked = False
        End If
    End Sub
End Class

Public Class frmOrderReceipt_Properties
    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCmbCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
        End Set
    End Property
    Private txtOrderNo As String = ""
    Public Property p_txtOrderNo() As String
        Get
            Return txtOrderNo
        End Get
        Set(ByVal value As String)
            txtOrderNo = value
        End Set
    End Property
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private txtSalesMan_NUM As String = ""
    Public Property p_txtSalesMan_NUM() As String
        Get
            Return txtSalesMan_NUM
        End Get
        Set(ByVal value As String)
            txtSalesMan_NUM = value
        End Set
    End Property
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property
    Private rbtBoth As Boolean = True
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
        End Set
    End Property
    Private rbtCustomer As Boolean = False
    Public Property p_rbtCustomer() As Boolean
        Get
            Return rbtCustomer
        End Get
        Set(ByVal value As Boolean)
            rbtCustomer = value
        End Set
    End Property
    Private rbtCompany As Boolean = False
    Public Property p_rbtCompany() As Boolean
        Get
            Return rbtCompany
        End Get
        Set(ByVal value As Boolean)
            rbtCompany = value
        End Set
    End Property


    Private chkAsOnDate As Boolean = False
    Public Property p_chkAsOnDate() As Boolean
        Get
            Return chkAsOnDate
        End Get
        Set(ByVal value As Boolean)
            chkAsOnDate = value
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

    Private rbtDelivery As Boolean = False
    Public Property p_rbtDelivery() As Boolean
        Get
            Return rbtDelivery
        End Get
        Set(ByVal value As Boolean)
            rbtDelivery = value
        End Set
    End Property

    Private rbtPending As Boolean = False
    Public Property p_rbtPending() As Boolean
        Get
            Return rbtPending
        End Get
        Set(ByVal value As Boolean)
            rbtPending = value
        End Set
    End Property
End Class