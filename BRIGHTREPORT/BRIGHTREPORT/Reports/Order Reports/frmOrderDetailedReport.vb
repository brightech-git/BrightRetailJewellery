Imports System.Data.OleDb
Public Class frmOrderDetailedReport
    'CALNO 051212 VASANTH  FOR GIRITECH MUMBAI

    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim objGridShower As frmGridDispDia
    Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'OR_PICPATH'")
    Dim dtMetal As New DataTable
    Dim dtCounter As New DataTable()
    Dim dtCostCentre As New DataTable
    Dim dtItem As New DataTable
    Dim dtDesigner As New DataTable
    Dim dtOrderStatus As New DataTable


    Private Sub frmOrderStatusReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmOrderStatusReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate
        dtpTo.MinimumDate = (New DateTimePicker).MinDate
        dtpTo.MaximumDate = (New DateTimePicker).MaxDate

        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.TabGeneral.Left, Me.TabGeneral.Top, Me.TabGeneral.Width, Me.TabGeneral.Height))
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        strSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
        strSql += " ORDER BY RESULT,METALNAME"
        dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")

        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        strSql = " SELECT 'ALL' ORDSTATE_NAME,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT CASE WHEN ORDSTATE_ID = 1 THEN 'PENDING WITH US' ELSE ORDSTATE_NAME END AS ORDSTATE_NAME,2 RESULT FROM " & cnAdminDb & "..ORDERSTATUS"
        strSql += " ORDER BY RESULT,ORDSTATE_NAME"
        chkSpecificFormat.Checked = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Function funcLoadItemName() As Integer
        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
        End If
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
    End Function
    Private Sub chkCmbMetal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbMetal.TextChanged
        funcLoadItemName()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        tabMain.SelectedTab = TabGeneral
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        chkSpecificFormat.Checked = False
        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If chkSpecificFormat.Checked Then
            OrderStatusDet()
        Else
            OrderStatus()
        End If
        Prop_Sets()
    End Sub

    Private Sub OrderStatusDet()
        strSql = "  EXEC " + cnAdminDb + "..RPT_ORDERSTATUS "
        strSql += vbCrLf + "  @FROMDATE='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  ,@TODATE='" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  ,@BASEDON='" + IIf(rbtOrdate.Checked, "O", "D") + "'"
        strSql += vbCrLf + "  ,@STATUS=''"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPTABLEDB..TEMPORIRDET AS T"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(String))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 1
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        'dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "Order status Report from " + dtpFrom.Text + " and " + dtpTo.Text
        Dim tit As String = "Order status Report from " + dtpFrom.Text + " and " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = True
        objGridShower.pnlFooter.BorderStyle = BorderStyle.Fixed3D
        objGridShower.pnlFooter.Size = New Size(objGridShower.pnlFooter.Width, 20)
        AddHandler objGridShower.gridView.KeyPress, AddressOf GridView_KeyPress
        FormatGridColumns(objGridShower.gridView, False, True, , False)
        objGridShower.formuser = userId
        objGridShower.Show()
        objGridShower.gridView.Columns("ORDSTATE_NAME").Visible = False
        objGridShower.gridView.Columns("KEYNO").HeaderText = "SNO"
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)

    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        Dim GstFlag As Boolean = funcGstView(dtpFrom.Value)
        With dgv
            For Each dgvRow As DataGridViewRow In dgv.Rows
                For Each dgvCol As DataGridViewColumn In dgv.Columns
                    If dgvRow.Cells(dgvCol.Name).Value.ToString = "Y" Then
                        If dgvCol.Name.ToString = "CAD" Then
                            dgvRow.Cells(dgvCol.Name).Style.BackColor = Color.Blue
                            dgvRow.Cells(dgvCol.Name).Style.ForeColor = Color.Blue
                        ElseIf dgvCol.Name.ToString = "CASTING" Then
                            dgvRow.Cells(dgvCol.Name).Style.BackColor = Color.Khaki
                            dgvRow.Cells(dgvCol.Name).Style.ForeColor = Color.Khaki
                        ElseIf dgvCol.Name.ToString = "SOCKET" Then
                            dgvRow.Cells(dgvCol.Name).Style.BackColor = Color.RosyBrown
                            dgvRow.Cells(dgvCol.Name).Style.ForeColor = Color.RosyBrown
                        ElseIf dgvCol.Name.ToString = "SETTING" Then
                            dgvRow.Cells(dgvCol.Name).Style.BackColor = Color.Green
                            dgvRow.Cells(dgvCol.Name).Style.ForeColor = Color.Green
                        ElseIf dgvCol.Name.ToString = "FINISHING" Then
                            dgvRow.Cells(dgvCol.Name).Style.BackColor = Color.Yellow
                            dgvRow.Cells(dgvCol.Name).Style.ForeColor = Color.Yellow
                        ElseIf dgvCol.Name.ToString = "POLISHING" Then
                            dgvRow.Cells(dgvCol.Name).Style.BackColor = Color.Orange
                            dgvRow.Cells(dgvCol.Name).Style.ForeColor = Color.Orange
                        ElseIf dgvCol.Name.ToString = "RECEIVED FROM SMITH" Then
                            dgvRow.Cells(dgvCol.Name).Style.BackColor = Color.LightGreen
                            dgvRow.Cells(dgvCol.Name).Style.ForeColor = Color.LightGreen
                        ElseIf dgvCol.Name.ToString = "ISSUE TO SMITH" Then
                            dgvRow.Cells(dgvCol.Name).Style.BackColor = Color.Violet
                            dgvRow.Cells(dgvCol.Name).Style.ForeColor = Color.Violet
                        Else
                            dgvRow.Cells(dgvCol.Name).Style.BackColor = Color.DarkRed
                            dgvRow.Cells(dgvCol.Name).Style.ForeColor = Color.DarkRed
                        End If

                    End If
                Next
            Next
            FormatGridColumns(dgv, False, False, , False)
        End With
    End Sub

    Private Sub OrderStatus()
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim chkCostName As String = ""
        Dim chkMetalName As String = ""
        Dim chkItemName As String = ""
        Dim chkDesigner As String = ""
        Dim chkStatus As String = ""

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPORDETAILEDTITLE')> 0"
        strSql += vbCrLf + "     DROP TABLE TEMPORDETAILEDTITLE"
        strSql += vbCrLf + " SELECT 'ORDER DETAILED VIEW FROM " & dtpFrom.Text & " TO " & dtpTo.Text & "' AS TITLE INTO TEMPORDETAILEDTITLE "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPORDETAILED' AND XTYPE = 'U') > 0 DROP TABLE TEMPORDETAILED"
        strSql += " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPORDETAILED' AND XTYPE = 'V') > 0 DROP VIEW TEMPORDETAILED"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT DISTINCT SUBSTRING(O.ORNO,6,20)ORNO,CONVERT(VARCHAR,O.ORDATE,105) ORDATE"
        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST IM WHERE IM.ITEMID=O.ITEMID) + '-'+ O.DESCRIPT AS DESCRIPT"
        'strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST IM WHERE IM.SUBITEMID=O.SUBITEMID) AS SUBITEM"
        strSql += vbCrLf + " ,O.PCS,O.GRSWT,O.PICTFILE,O.SNO"
        'strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE OS WHERE OS.ORSNO=O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
        'strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE OS WHERE OS.ORSNO=O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
        'strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
        'strSql += vbCrLf + " ,CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        'strSql += vbCrLf + " ,O.SIZENO"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(100),CASE WHEN ISNULL(O.ODBATCHNO,'') <> '' AND ISNULL(ISS.SNO,'')='' "
        'For Giritech Mumbai
        strSql += vbCrLf + " And Isnull((Select top 1 trandate from " & cnStockDb & "..issue where isnull(CANCEL,'')='' and batchno=o.odbatchno),'')<='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " THEN (SELECT TOP 1 ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = 5)"
        strSql += vbCrLf + " WHEN ISNULL(O.ODBATCHNO,'') <> '' AND ISNULL(ISS.SNO,'')<>'' THEN 'APPROVAL ISSUE'"
        strSql += vbCrLf + "   WHEN ISNULL(O.ORDCANCEL,'') <> '' THEN (SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = 6)"
        strSql += vbCrLf + "   ELSE ISNULL((SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = (SELECT TOP 1 ORDSTATE_ID FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ISNULL(CANCEL,'') = '' ORDER BY ENTRYORDER DESC)),'PENDING WITH US') "
        strSql += vbCrLf + "   END) AS STATUS"
        'strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),O.RATE)AS ORRATE"
        
        strSql += vbCrLf + "  ,CONVERT(VARCHAR,P.PNAME + CASE WHEN ISNULL(INITIAL,'') <> '' THEN INITIAL ELSE '' END ) AS PNAME,"
        strSql += vbCrLf + "   CASE WHEN ISNULL(P.PHONERES,'') <> '' THEN CASE WHEN ISNULL(P.MOBILE,'') <> '' THEN P.PHONERES+','+P.MOBILE ELSE P.PHONERES END   ELSE CASE WHEN ISNULL(P.MOBILE,'') <> '' THEN P.MOBILE ELSE '' END END AS DELIVERYNO "
        'strSql += vbCrLf + "  ,CONVERT(VARCHAR(20),CASE WHEN ISNULL(O.ODBATCHNO,'') <> '' THEN (SELECT TOP 1 TRANNO  FROM " & cnStockDb & "..ISSUE WHERE BATCHNO  = O.ODBATCHNO GROUP BY TRANNO) ELSE NULL END) DELIVERYNO"
        'strSql += vbCrLf + "  ,CONVERT(VARCHAR(12),CASE WHEN ISNULL(O.ODBATCHNO,'') <> '' THEN (SELECT TOP 1 TRANDATE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO  = O.ODBATCHNO GROUP BY TRANDATE) ELSE NULL END,103) DELIVERYDATE"
        'strSql += vbCrLf + " ,(SELECT  TOP 1 DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = (SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'I' AND ISNULL(CANCEL,'') = ''))DESIGNER"
        'strSql += vbCrLf + "  ,(SELECT  TOP 1 COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = O.COSTID)AS COSTNAME"
        'strSql += vbCrLf + "  ,(SELECT  TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = O.PROPSMITH)AS SMITHNAME"
        'strSql += vbCrLf + "  ,O.BATCHNO"
        'strSql += vbCrLf + "  ,(SELECT  TOP 1 EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = O.EMPID)AS EMPNAME"

        'strSql += vbCrLf + "  ,CONVERT(vARCHAR(3),NULL)COLHEAD"
        'strSql += vbCrLf + "  ,CONVERT(INT,1)RESULT"
        'strSql += vbCrLf + "  ,CONVERT(INT,NULL)GRESULT,CONVERT(VARCHAR(15),O.SNO)SNO"
        'strSql += vbCrLf + "  ,IDENTITY(INT,1,1)ROWID"
        'strSql += vbCrLf + "  ,(SELECT TOP 1 TRANNO FROM " & cnAdminDb & "..ORIRDETAIL WHERE ISNULL(ORSNO,'') = ISNULL(O.SNO,'')) TRANNO"
        'strSql += vbCrLf + "  ,(SELECT  TOP 1 ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID) COUNTER"
        'strSql += vbCrLf + " ,O.ORNO as ORSNO"
        strSql += vbCrLf + "  INTO TEMPORDETAILED"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ORMAST O"

        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..ITEMTAG AS T ON ISNULL(T.ORSNO,'') = O.SNO AND ISNULL(T.ORDREPNO,'') = O.ORNO"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..ISSUE ISS ON ISS.SNO =O.ODSNO AND ISNULL(ISS.TRANTYPE,'')='AI'"

        strSql += vbCrLf + "LEFT OUTER JOIN " & cnAdminDb & "..customerinfo c ON c.batchno =O.batchno"
        strSql += vbCrLf + "LEFT OUTER JOIN " & cnAdminDb & "..PERSONALINFO p ON p.SNO =C.PSNO"
        strSql += vbCrLf + " WHERE O.ORDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If rbtorder.Checked Then strSql += vbCrLf + " AND  O.ORTYPE='O'"
        If rbtRepair.Checked Then strSql += vbCrLf + " AND  O.ORTYPE='R'"
        If rbtboth.Checked Then strSql += vbCrLf + " AND  O.ORTYPE in ('O','R')"
        strSql += vbCrLf + " AND O.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(O.CANCEL,'') = ''"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + "  AND   O.COSTID IN (SELECT  COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            strSql += vbCrLf + "  AND O.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & ")))"
        End If
        If chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql += vbCrLf + "  AND O.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " ALTER TABLE TEMPORDETAILED ADD TAGIMAGE IMAGE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        Dim dtImage As New DataTable
        strSql = " SELECT SNO,PICTFILE FROM TEMPORDETAILED"
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
                    strSql = " UPDATE TEMPORDETAILED SET TAGIMAGE = ? WHERE SNO = '" & ro!SNO.ToString & "'"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.Parameters.AddWithValue("@image", result)
                    cmd.ExecuteNonQuery()
                End If
            End If
        Next


        Dim objReport As New BrighttechReport
        Dim objRptViewer As New frmReportViewer
        objRptViewer.CrystalReportViewer1.ReportSource = objReport.ShowReport(New rptOrderDetailedReport, cnDataSource)
        objRptViewer.Dock = DockStyle.Fill
        objRptViewer.Show()
        objRptViewer.CrystalReportViewer1.Select()



        '''''   GRID BINDING OLD METHOD
        'Dim dtGrid As New DataTable
        'dtGrid.Columns.Add("KEYNO", GetType(Integer))
        'dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        'dtGrid.Columns("KEYNO").AutoIncrement = True
        'dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtGrid)
        'If Not dtGrid.Rows.Count > 0 Then
        '    MsgBox("Record not found", MsgBoxStyle.Information)
        '    dtpFrom.Select()
        '    Exit Sub
        'End If

        'dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        'objGridShower = New frmGridDispDia
        'objGridShower.Name = Me.Name
        'objGridShower.gridView.RowTemplate.Height = 21
        'objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        'objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        ''objGridShower.Size = New Size(778, 550)
        'objGridShower.Text = "ORDER\REPAIR STATUS REPORT"
        'Dim tit As String = ""
        ''If rbtBoth.Checked Then
        ''    tit += "ORDER & REPAIR STATUS REPORT" + vbCrLf
        ''ElseIf rbtOrder.Checked Then
        ''    tit += "ORDER STATUS REPORT" + vbCrLf
        ''Else
        ''    tit += "REPAIR STATUS REPORT" + vbCrLf
        ''End If

        ''If chkAsOnDate.Checked Then
        ''    tit += " AS ONDATE " & dtpFrom.Text
        ''Else
        'tit += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        ''End If
        'tit += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        'objGridShower.lblTitle.Text = tit
        'objGridShower.StartPosition = FormStartPosition.CenterScreen
        'objGridShower.dsGrid.DataSetName = objGridShower.Name
        'objGridShower.dsGrid.Tables.Add(dtGrid)
        'objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)

        'objGridShower.FormReSize = True
        'objGridShower.FormReLocation = False
        'objGridShower.pnlFooter.Visible = False
        'objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        'Ddv_OrderStatusFormatting(objGridShower.gridView)
        'FormatGridColumns(objGridShower.gridView, False, False, , False)
        'objGridShower.Show()
        'objGridShower.FormReSize = True
        'objGridShower.gridViewHeader.Visible = False
        'objGridShower.pnlFooter.Visible = True
        'objGridShower.lblDeliver.Visible = True
        'objGridShower.lblDelivered.Visible = True
        'objGridShower.lblIss.Visible = True
        'objGridShower.lblIsstoSmith.Visible = True
        'objGridShower.lblRec.Visible = True
        'objGridShower.lblRecSmith.Visible = True
        'objGridShower.lblPending.Visible = True
        'objGridShower.lblPendingWithUs.Visible = True
        'objGridShower.lblCancel.Visible = True
        'objGridShower.lblCancelled.Visible = True
        'objGridShower.lblReady.Visible = True
        'objGridShower.lblReadyforDel.Visible = True
        'objGridShower.lblAppIss.Visible = True
        'objGridShower.lblApproval.Visible = True
        'AddHandler objGridShower.gridView.KeyPress, AddressOf GridView_KeyPress
        'FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        'GridViewFormat()
        'objGridShower.gridView.Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)


    End Sub
    Private Sub Ddv_OrderStatusFormatting(ByVal dgv As DataGridView)
        'With dgv
        '    For cnt As Integer = 22 To dgv.ColumnCount - 1
        '        .Columns(cnt).Visible = False
        '    Next
        '    .Columns("DESIGNER").Width = 100
        '    .Columns("DESIGNER").Visible = True
        '    .Columns("SMITHNAME").Width = 100
        '    '.Columns("SMITHNAME").Visible = IIf(rbtSmith.Checked, False, True)

        '    .Columns("PNAME").Visible = True
        '    .Columns("PNAME").Width = 100
        '    .Columns("PNAME").HeaderText = "PARTY NAME"
        '    .Columns("MCGRM").HeaderText = "MC/Gm"
        '    .Columns("WASTPER").HeaderText = "W%"
        '    .Columns("WAST").HeaderText = "W/Gm"
        '    .Columns("PARTICULAR").Width = 200
        '    .Columns("ORNO").Visible = False
        '    .Columns("ORSNO").Visible = False
        '    .Columns("ORDATE").Width = 80
        '    .Columns("REMDATE").Width = 80
        '    .Columns("DUEDATE").Width = 80
        '    .Columns("STYLENO").Width = 80
        '    .Columns("DESCRIPT").Width = 230
        '    .Columns("PCS").Width = 60
        '    .Columns("GRSWT").Width = 80
        '    .Columns("NETWT").Width = 80
        '    .Columns("MCGRM").Width = 80
        '    .Columns("WASTPER").Width = 80
        '    .Columns("WAST").Width = 80
        '    .Columns("STNPCS").Width = 60
        '    .Columns("STNWT").Width = 80
        '    .Columns("DIAPCS").Width = 60
        '    .Columns("DIAWT").Width = 80
        '    .Columns("SIZENO").Width = 60
        '    .Columns("STATUS").Width = 150
        '    .Columns("PNAME").Width = 200
        '    .Columns("COSTNAME").Width = 150
        '    .Columns("ORRATE").Width = 70
        '    ' .Columns("ADV_WT").Width = 70
        '    '.Columns("ADV_AMT").Width = 70
        '    .Columns("TRANNO").Width = 70
        '    .Columns("DELIVERYNO").Width = 90
        '    .Columns("DELIVERYNO").Visible = True
        '    .Columns("DELIVERYDATE").Width = 120
        '    .Columns("DELIVERYDATE").Visible = True
        '    .Columns("TRANNO").HeaderText = "SMITH" + vbCrLf + "ISS NO"
        '    .Columns("TRANNO").Visible = True
        '    '.Columns("COUNTER").Visible = IIf(rbtCounter.Checked, False, True)
        '    .Columns("COUNTER").Width = 100
        '    .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
        '    .Columns("ORDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        '    .Columns("REMDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        '    .Columns("DUEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"

        '    'If rbtOrder.Checked Then
        '    '    .Columns("ORDATE").HeaderText = "ORDATE"
        '    'ElseIf rbtRepair.Checked Then
        '    '    .Columns("ORDATE").HeaderText = "REPDATE"
        '    'Else
        '    '    .Columns("ORDATE").HeaderText = "OR_REP DATE"
        '    'End If
        '    For cnt As Integer = 0 To dgv.ColumnCount - 1
        '        .Columns(cnt).SortMode = DataGridViewColumnSortMode.Automatic
        '    Next
        '    ' .Columns("ADV_AMT").Visible = True
        '    '.Columns("ADV_WT").Visible = True
        '    '.Columns("ADV_AMT").HeaderText = "ADV AMT"
        '    '.Columns("ADV_WT").HeaderText = "ADV WT"
        'End With
    End Sub
    Function GridViewFormat() As Integer
        'For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
        '    With dgvRow
        '        Select Case .Cells("STATUS").Value.ToString
        '            Case "BOOKED"
        '                .DefaultCellStyle.BackColor = Color.AliceBlue
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '            Case "ISSUE TO SMITH"
        '                .DefaultCellStyle.BackColor = Color.LightPink
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '            Case "RECEIVE FROM SMITH"
        '                .DefaultCellStyle.BackColor = Color.RosyBrown
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '            Case "READY FOR DELIVERY"
        '                .DefaultCellStyle.BackColor = Color.Orange
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '            Case "DELIVERED"
        '                .DefaultCellStyle.BackColor = Color.LightGreen
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '            Case "CANCELLED"
        '                .DefaultCellStyle.BackColor = Color.Red
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '            Case "PENDING WITH US"
        '                .DefaultCellStyle.BackColor = Color.Wheat
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '            Case "APPROVAL ISSUE"
        '                .DefaultCellStyle.BackColor = Color.LightSkyBlue
        '                .DefaultCellStyle.Font = reportTotalStyle.Font
        '        End Select
        '    End With
        'Next
    End Function
    'Private Sub Dgv_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
    '    Dim dgv As DataGridView = CType(sender, DataGridView)
    '    frmOrderRepairStatus.txtOrdRepNo.Text = dgv.Rows(dgv.CurrentRow.Index).Cells("ORNO").Value.ToString
    'End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmOrderDetailedReport_Properties
        SetSettingsObj(obj, Me.Name, GetType(frmOrderDetailedReport_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmOrderDetailedReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmOrderDetailedReport_Properties))
    End Sub

    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'If UCase(e.KeyChar) = "D" Then
        '    If GetAdmindbSoftValue("PRN_ORIR", "N") = "Y" Then
        '        Dim prnmemsuffix As String = ""
        '        Dim pbatchno As String
        '        Dim pbilldate As Date
        '        Dim dgv As DataGridView = CType(sender, DataGridView)
        '        If dgv.CurrentRow Is Nothing Then Exit Sub
        '        If dgv.CurrentRow.Cells("BATCHNO").Value.ToString = "" Then Exit Sub
        '        pbatchno = dgv.CurrentRow.Cells("BATCHNO").Value.ToString
        '        pbilldate = dgv.CurrentRow.Cells("ORDATE").Value.ToString
        '        If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId

        '        If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
        '            Dim write As IO.StreamWriter
        '            Dim memfile As String = "\BillPrint" & prnmemsuffix.Trim & ".mem"
        '            write = IO.File.CreateText(Application.StartupPath & memfile)
        '            write.WriteLine(LSet("TYPE", 15) & ":OIR")
        '            write.WriteLine(LSet("BATCHNO", 15) & ":" & pbatchno)
        '            write.WriteLine(LSet("TRANDATE", 15) & ":" & pbilldate.ToString("yyyy-MM-dd"))
        '            write.WriteLine(LSet("DUPLICATE", 15) & ":N")
        '            write.Flush()
        '            write.Close()
        '            If EXE_WITH_PARAM = False Then
        '                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
        '            Else
        '                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
        '                LSet("TYPE", 15) & ":OIR" & ";" & _
        '                LSet("BATCHNO", 15) & ":" & pbatchno & ";" & _
        '                LSet("TRANDATE", 15) & ":" & pbilldate.ToString("yyyy-MM-dd") & ";" & _
        '                LSet("DUPLICATE", 15) & ":N")
        '            End If
        '        Else
        '            MsgBox("Billprint exe not found", MsgBoxStyle.Information)
        '        End If
        '    End If
        'End If
    End Sub

    Private Sub grpContainer_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpContainer.Enter

    End Sub

    Private Sub TabGeneral_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabGeneral.Click

    End Sub

    Private Sub chkSpecificFormat_CheckedChanged(sender As Object, e As EventArgs) Handles chkSpecificFormat.CheckedChanged
        If chkSpecificFormat.Checked Then
            pnlSpecific.Visible = True
        Else
            pnlSpecific.Visible = False
        End If
    End Sub
End Class

Public Class frmOrderDetailedReport_Properties

    Private rbtEmpName As Boolean = True
    Public Property p_rbtEmpName() As Boolean
        Get
            Return rbtEmpName
        End Get
        Set(ByVal value As Boolean)
            rbtEmpName = value
        End Set
    End Property

    Private rbtCounter As Boolean
    Public Property p_rbtCounter() As Boolean
        Get
            Return rbtCounter
        End Get
        Set(ByVal value As Boolean)
            rbtCounter = value
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
    Private rbtOrder As Boolean = False
    Public Property p_rbtOrder() As Boolean
        Get
            Return rbtOrder
        End Get
        Set(ByVal value As Boolean)
            rbtOrder = value
        End Set
    End Property
    Private rbtRepair As Boolean = False
    Public Property p_rbtRepair() As Boolean
        Get
            Return rbtRepair
        End Get
        Set(ByVal value As Boolean)
            rbtRepair = value
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
    Private rbtOrderDate As Boolean = True
    Public Property p_rbtOrderDate() As Boolean
        Get
            Return rbtOrderDate
        End Get
        Set(ByVal value As Boolean)
            rbtOrderDate = value
        End Set
    End Property
    Private rbtDueDate As Boolean = False
    Public Property p_rbtDueDate() As Boolean
        Get
            Return rbtDueDate
        End Get
        Set(ByVal value As Boolean)
            rbtDueDate = value
        End Set
    End Property
    Private rbtTypeAll As Boolean = True
    Public Property p_rbtTypeAll() As Boolean
        Get
            Return rbtTypeAll
        End Get
        Set(ByVal value As Boolean)
            rbtTypeAll = value
        End Set
    End Property
    Private rbtTypeCompany As Boolean = False
    Public Property p_rbtTypeCompany() As Boolean
        Get
            Return rbtTypeCompany
        End Get
        Set(ByVal value As Boolean)
            rbtTypeCompany = value
        End Set
    End Property
    Private rbtTypeCustomer As Boolean = False
    Public Property p_rbtTypeCustomer() As Boolean
        Get
            Return rbtTypeCustomer
        End Get
        Set(ByVal value As Boolean)
            rbtTypeCustomer = value
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
    Private chkCmbMetal As New List(Of String)
    Public Property p_chkCmbMetal() As List(Of String)
        Get
            Return chkCmbMetal
        End Get
        Set(ByVal value As List(Of String))
            chkCmbMetal = value
        End Set
    End Property
    Private chkCmbItem As New List(Of String)
    Public Property p_chkCmbItem() As List(Of String)
        Get
            Return chkCmbItem
        End Get
        Set(ByVal value As List(Of String))
            chkCmbItem = value
        End Set
    End Property
    Private chkCmbDesigner As New List(Of String)
    Public Property p_chkCmbDesigner() As List(Of String)
        Get
            Return chkCmbDesigner
        End Get
        Set(ByVal value As List(Of String))
            chkCmbDesigner = value
        End Set
    End Property
    Private cmbEmpName As String = "ALL"
    Public Property p_cmbEmpName() As String
        Get
            Return cmbEmpName
        End Get
        Set(ByVal value As String)
            cmbEmpName = value
        End Set
    End Property
    Private cmbOrderBy As String = "ORDER DATE"
    Public Property p_cmbOrderBy() As String
        Get
            Return cmbOrderBy
        End Get
        Set(ByVal value As String)
            cmbOrderBy = value
        End Set
    End Property
    Private txtCustomerName As String = ""
    Public Property p_txtCustomerName() As String
        Get
            Return txtCustomerName
        End Get
        Set(ByVal value As String)
            txtCustomerName = value
        End Set
    End Property
    Private ChkCmbStatus As New List(Of String)
    Public Property p_ChkCmbStatus() As List(Of String)
        Get
            Return ChkCmbStatus
        End Get
        Set(ByVal value As List(Of String))
            ChkCmbStatus = value
        End Set
    End Property

    Private chkGroupByEmpName As Boolean = True
    Public Property p_chkGroupByEmpName() As Boolean
        Get
            Return chkGroupByEmpName
        End Get
        Set(ByVal value As Boolean)
            chkGroupByEmpName = value
        End Set
    End Property
End Class