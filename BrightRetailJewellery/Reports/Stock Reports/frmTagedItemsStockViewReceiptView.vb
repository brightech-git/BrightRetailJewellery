Imports System.Data.OleDb
Imports System.IO
Public Class frmTagedItemsStockViewReceiptView
    '02 SHERIFF
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim objMoreOption As New frmcheckothermaster
    Dim tempDt As New DataTable("OtherDetails")
    Dim dtStoneDetails As New DataTable("StoneDetails")
    Dim dtGrandTotalDetails As New DataTable("GrandTotal")
    Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")
    Dim headerBgColor As New System.Drawing.Color
    Dim dtCompany As New DataTable
    Dim dtMetal As New DataTable
    Dim dtDesigner As New DataTable
    Dim RowFillState As Boolean = False
    Dim OTHmaster As Boolean = False
    Dim dtSource As New DataTable
    Dim Studded As Boolean = False
    Dim cmbDesName As String = Nothing
    Dim dtCostCentre As New DataTable
    Dim RPT_RECEIPTVIEW_DESIGN As Boolean = IIf(GetAdmindbSoftValue("RPT_RECEIPTVIEW_DESIGN", "N") = "Y", True, False)
    Dim LoosestnwtInGrswt As Boolean = IIf(GetAdmindbSoftValue("RPT_RECEIPTVIEW_STNWTINGRSWT", "N") = "Y", True, False)
    Dim SqlVersion As String = ""
    Dim dtOtherMaster As New DataTable
    Private Sub OtherView()
        strSql = " SELECT MISCNAME NAME,MISCID ID,1 RESULT FROM " & cnAdminDb & "..OTHERMASTERENTRY"
        strSql += " ORDER BY MISCNAME"
        dtOtherMaster = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtOtherMaster)
    End Sub
    Private Sub DetailedReport()
        btnView_Search.Enabled = False
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPITEMTAGTITLE')> 0"
        strSql += "     DROP TABLE TEMPITEMTAGTITLE"
        strSql += " SELECT 'TAG RECEIPT VIEW FROM " & dtpFrom.Text & " TO " & dtpTo.Text & "' AS TITLE INTO TEMPITEMTAGTITLE "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPITEMTAGSTOCK')> 0"
        strSql += "     DROP TABLE TEMPITEMTAGSTOCK"
        strSql += " DECLARE @DEFPATH VARCHAR(200)"
        strSql += " SELECT @DEFPATH = '" & defaultPic & "'"
        strSql += " SELECT SNO"
        strSql += " ,CASE WHEN SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)"
        strSql += " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) END AS PARTICULAR"
        strSql += " ," & IIf(chkActualDate.Checked, "ACTUALRECDATE RECDATE", "RECDATE") & ",TAGNO,PCS,GRSWT,NETWT,ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)),CASE WHEN PURITY<> 0 THEN PURITY ELSE NULL END)AS PURITY,RATE,MAXWAST,MAXMC,SALVALUE"
        strSql += " ,(SELECT CASE WHEN ISNULL(SEAL,'') <> '' THEN SEAL ELSE DESIGNERNAME END  FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
        strSql += " ,@DEFPATH + T.PCTFILE AS PCTFILE,NARRATION,TAGVAL,STYLENO,CONVERT(VARCHAR,TAGVAL) + TAGKEY AS TAGID"
        strSql += " INTO TEMPITEMTAGSTOCK"
        strSql += "  FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += funcFiltrationString()
        If chkWithCumulative.Checked Then
            strSql += " UNION ALL"
            strSql += " SELECT SNO"
            strSql += " ,CASE WHEN T.SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)"
            strSql += " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) END AS PARTICULAR"
            strSql += " ," & IIf(chkActualDate.Checked, "ACTUALRECDATE RECDATE", "RECDATE") & ",TAGNO,T.PCS,T.GRSWT,T.NETWT,ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)),CASE WHEN PURITY<> 0 THEN PURITY ELSE NULL END)AS PURITY,RATE,MAXWAST,MAXMC,SALVALUE"
            strSql += " ,(SELECT CASE WHEN ISNULL(SEAL,'') <> '' THEN SEAL ELSE DESIGNERNAME END  FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
            strSql += " ,@DEFPATH + T.PCTFILE AS PCTFILE,NARRATION,TAGVAL,T.STYLENO,CONVERT(VARCHAR,TAGVAL) + TAGKEY AS TAGID"
            strSql += "  FROM " & cnAdminDb & "..CITEMTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = T.ITEMID AND SM.SUBITEMID = T.SUBITEMID"
            strSql += funcFiltrationString(True)

            strSql += vbCrLf + " AND T.TAGNO NOT IN  (SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG I WHERE   I.RECDATE Between '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' and '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'  AND I.TAGNO = T.TAGNO AND I.itemid = T.ITEMID And I.COMPANYID = T.COMPANYID And I.COSTID = T.COSTID)"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " ALTER TABLE TEMPITEMTAGSTOCK ADD TAGIMAGE IMAGE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPITEMTAGSTONESTOCK')> 0"
        strSql += "    DROP TABLE TEMPITEMTAGSTONESTOCK"
        strSql += " SELECT TAGSNO,"
        strSql += " CASE WHEN STNSUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.STNSUBITEMID)"
        strSql += " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID) END AS PARTICULAR"
        strSql += " ,STNPCS,STNWT,STNRATE,STNAMT,CASE WHEN CALCMODE = 'W' THEN 'WIGHT' ELSE 'PIECE' END CALCMODE"
        strSql += " ,CASE WHEN STONEUNIT = 'C' THEN 'CARAT' ELSE 'GRAM' END STONEUNIT,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID)AS STNTYPE"
        strSql += vbCrLf + " ,(SELECT TOP 1 SIZEDESC FROM " & cnAdminDb & "..CENTSIZE WHERE CONVERT(NUMERIC(15,4),(CASE WHEN STNPCS > 0 THEN STNWT/STNPCS ELSE STNWT END)*100) BETWEEN FROMCENT AND TOCENT)SIZEDESC"
        strSql += " INTO TEMPITEMTAGSTONESTOCK"
        strSql += " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T"
        strSql += " WHERE TAGSNO IN (SELECT SNO FROM TEMPITEMTAGSTOCK)"
        If chkWithCumulative.Checked Then
            strSql += " UNION ALL"
            strSql += " SELECT TAGSNO,"
            strSql += " CASE WHEN STNSUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.STNSUBITEMID)"
            strSql += " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID) END AS PARTICULAR"
            strSql += " ,STNPCS,STNWT,STNRATE,STNAMT,CASE WHEN CALCMODE = 'W' THEN 'WIGHT' ELSE 'PIECE' END CALCMODE"
            strSql += " ,CASE WHEN STONEUNIT = 'C' THEN 'CARAT' ELSE 'GRAM' END STONEUNIT,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID)AS STNTYPE"
            strSql += vbCrLf + " ,(SELECT TOP 1 SIZEDESC FROM " & cnAdminDb & "..CENTSIZE WHERE CONVERT(NUMERIC(15,4),(CASE WHEN STNPCS > 0 THEN STNWT/STNPCS ELSE STNWT END)*100) BETWEEN FROMCENT AND TOCENT)SIZEDESC"
            strSql += " FROM " & cnAdminDb & "..CITEMTAGSTONE AS T"
            strSql += " WHERE TAGSNO IN (SELECT SNO FROM TEMPITEMTAGSTOCK)"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim dtImage As New DataTable
        strSql = " SELECT SNO,PCTFILE FROM TEMPITEMTAGSTOCK"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtImage)
        If Not dtImage.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            btnView_Search.Enabled = True
            Exit Sub
        End If
        For Each ro As DataRow In dtImage.Rows
            Dim serverPath As String = Nothing
            Dim fileDestPath As String = ro!PCTFILE.ToString
            If IO.File.Exists(fileDestPath) Then
                Dim Finfo As IO.FileInfo
                Finfo = New IO.FileInfo(fileDestPath)
                'Finfo.IsReadOnly = False
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
                resizeimg = False
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
                If IO.Directory.Exists(Finfo.Directory.FullName) Then
                    Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, IO.FileAccess.Read)
                    Dim reader As New IO.BinaryReader(fileStr)
                    Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                    fileStr.Read(result, 0, result.Length)
                    fileStr.Close()
                    strSql = " UPDATE TEMPITEMTAGSTOCK SET TAGIMAGE = ? WHERE SNO = '" & ro!SNO.ToString & "'"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.Parameters.AddWithValue("@image", result)
                    cmd.ExecuteNonQuery()
                End If
            End If
        Next

        ProgressBarShow(ProgressBarStyle.Marquee)
        ProgressBarStep("Loading Report")
        Dim objReport As New GiritechReport
        Dim objRptViewer As New frmReportViewer
        objRptViewer.CrystalReportViewer1.ReportSource = objReport.ShowReport(New rptTagStockIssueRec, cnDataSource)
        objRptViewer.MdiParent = Main
        objRptViewer.Dock = DockStyle.Fill
        objRptViewer.Show()
        objRptViewer.CrystalReportViewer1.Select()
        ProgressBarHide()
        btnView_Search.Enabled = True
    End Sub

    Function funcExit() As Integer
        Me.Close()
    End Function



    Function funcFillStoneDetails() As Integer
        dtStoneDetails.Clear()
        strSql = " SELECT "
        strSql += " CASE WHEN DPCS <> 0 THEN DPCS ELSE NULL END AS DPCS"
        strSql += " ,CASE WHEN DCARAT <> 0 THEN DCARAT ELSE NULL END AS DCARAT"
        strSql += " ,CASE WHEN DGRAM <> 0 THEN DGRAM ELSE NULL END AS DGRAM"
        strSql += " ,CASE WHEN SPCS <> 0 THEN SPCS ELSE NULL END AS SPCS"
        strSql += " ,CASE WHEN SCARAT <> 0 THEN SCARAT ELSE NULL END AS SCARAT"
        strSql += " ,CASE WHEN SGRAM <> 0 THEN SGRAM ELSE NULL END AS SGRAM"
        strSql += " ,CASE WHEN PPCS <> 0 THEN PPCS ELSE NULL END AS PPCS"
        strSql += " ,CASE WHEN PCARAT <> 0 THEN PCARAT ELSE NULL END AS PCARAT"
        strSql += " ,CASE WHEN PGRAM <> 0 THEN PGRAM ELSE NULL END AS PGRAM"
        strSql += " FROM "
        strSql += " ("
        strSql += " SELECT"
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE = 'D' THEN STNPCS END),0) DPCS,"
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE = 'D' AND STONEUNIT = 'C' THEN STNWT END),0)DCARAT,"
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE = 'D' AND STONEUNIT = 'G' THEN STNWT END),0)DGRAM,"
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE IN ('T','S') THEN STNPCS END),0) SPCS,"
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE IN ('T','S') AND STONEUNIT = 'C' THEN STNWT END),0)SCARAT,"
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE IN ('T','S') AND STONEUNIT = 'G' THEN STNWT END),0)SGRAM,"
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE = 'P' THEN STNPCS END),0) PPCS,"
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE = 'P' AND STONEUNIT = 'C' THEN STNWT END),0)PCARAT,"
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE = 'P' AND STONEUNIT = 'G' THEN STNWT END),0)PGRAM"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT STNPCS,STONEUNIT,STNWT,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID)AS STNTYPE"
        strSql += " FROM " & cnAdminDb & "..ITEMTAGSTONE S" + vbCrLf
        strSql += " WHERE TAGSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW)" + vbCrLf
        strSql += " )Y"
        strSql += " )X"
        Dim dtStone As New DataTable("StoneDetails")
        dtStone.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtStone)
        If Not dtStone.Rows.Count > 0 Then
            gridFullStoneDetails.DataSource = dtStoneDetails
            Exit Function
        End If
        With dtStone.Rows(0)
            funcAddStoneDetails("", "Diamond", "Stone", "Precious")
            funcAddStoneDetails("Piece(s)", .Item("DPcs").ToString, .Item("SPcs").ToString, .Item("PPcs").ToString)
            funcAddStoneDetails("Carat(s)", .Item("DCarat").ToString, .Item("SCarat").ToString, .Item("PCarat").ToString)
            funcAddStoneDetails("Gram(s)", .Item("DGram").ToString, .Item("SGram").ToString, .Item("PGram").ToString)
        End With
        gridFullStoneDetails.DataSource = dtStoneDetails
        With gridFullStoneDetails.Columns("Description")
            .Width = 70
        End With
        With gridFullStoneDetails.Columns("Diamond")
            .Width = 70
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
        With gridFullStoneDetails.Columns("Stone")
            .Width = 70
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
        With gridFullStoneDetails.Columns("Precious")
            .Width = 70
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
    End Function

    Function funcAddNewRow(ByVal maxDesc As String, ByVal maxVal As String, ByVal minDesc As String, ByVal minVal As String, ByVal otherDesc1 As String, ByVal otherVal1 As String, ByVal otherDesc2 As String, ByVal otherVal2 As String) As Integer
        Dim ro As DataRow = Nothing
        ro = tempDt.NewRow
        ro("MaxDescription") = maxDesc
        ro("MaxValues") = maxVal
        ro("MinDescription") = minDesc
        ro("MinValues") = minVal
        ro("OtherDesc1") = otherDesc1
        ro("OtherVal1") = otherVal1
        ro("OtherDesc2") = otherDesc2
        ro("OtherVal2") = otherVal2
        tempDt.Rows.Add(ro)
    End Function

    Function funcAddGrandDetails(ByVal desc As String, ByVal pcs As String, ByVal grsWt As String, ByVal lessWt As String, ByVal netWt As String, ByVal extraWt As String, ByVal salValue As String) As Integer
        Dim ro As DataRow = Nothing
        ro = dtGrandTotalDetails.NewRow
        ro("Description") = desc
        ro("Pcs") = pcs
        ro("GrsWt") = grsWt
        ro("LessWt") = lessWt
        ro("NetWt") = netWt
        ro("ExtraWt") = extraWt
        ro("SalValue") = salValue
        dtGrandTotalDetails.Rows.Add(ro)
    End Function

    Function funcFillGrandDetails() As Integer
        dtGrandTotalDetails.Clear()
        Dim Gtotpcs As Long = Val(dtSource.Compute("sum(Pcs)", Nothing).ToString)
        Dim Gtotgrwt As Decimal = Val(dtSource.Compute("sum(Grswt)", Nothing).ToString)
        Dim Gtotlesswt As Decimal = Val(dtSource.Compute("sum(Lesswt)", Nothing).ToString)
        Dim Gtotnetwt As Decimal = Val(dtSource.Compute("sum(Netwt)", Nothing).ToString)
        Dim GtotExtrawt As Decimal = Val(dtSource.Compute("sum(Extrawt)", Nothing).ToString)
        Dim GtotSalvalue As Decimal = Val(dtSource.Compute("sum(SALVALUE)", Nothing).ToString)
        strSql = " SELECT " & Gtotpcs & " AS PCS," & Gtotgrwt & " AS GRSWT," & Gtotlesswt & " AS LESSWT," & Gtotnetwt & " AS NETWT," & GtotExtrawt & " AS EXTRAWT, " & GtotSalvalue & " AS  SALVALUE"

        'strSql += " CASE WHEN SUM(NETWT) <> 0 THEN SUM(NETWT) ELSE NULL END NETWT,"
        'strSql += " CASE WHEN SUM(EXTRAWT) <> 0 THEN SUM(EXTRAWT) ELSE NULL END EXTRAWT,"
        'strSql += " CASE WHEN SUM(SALVALUE) <> 0 THEN SUM(SALVALUE) ELSE NULL END SALVALUE"
        'strSql += " FROM " & cnAdminDb & "..ITEMTAG T"
        'strSql += funcFiltrationString()
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            gridGrandTotal.DataSource = dtGrandTotalDetails
            Exit Function
        End If
        With dt.Rows(0)
            funcAddGrandDetails("", "Piece(s)", "Grs_Wt", "Less_Wt", "Net_Wt", "Extra_Wt", "Sal_Val")
            funcAddGrandDetails("Cursor Total", "", "", "", "", "", "")
            funcAddGrandDetails("", "", "", "", "", "", "")
            funcAddGrandDetails("Grand Total", .Item("Pcs").ToString, .Item("GrsWt").ToString, .Item("LessWt").ToString, .Item("NetWt").ToString, .Item("ExtraWt").ToString, .Item("SalValue").ToString)
        End With
        gridGrandTotal.DataSource = dtGrandTotalDetails
        With gridGrandTotal
            With .Columns("Description")
                .Width = 90
            End With
            With .Columns("Pcs")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("GrsWt")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("LessWt")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("NetWt")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("ExtraWt")
                .Width = 80
                .Visible = False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("SalValue")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End With
    End Function

    Function funcGetCursorTot(ByVal rowIndex As Integer) As Integer
        Dim pcs As Integer = 0
        Dim GrsWt As Double = 0
        Dim lessWt As Double = 0
        Dim netWt As Double = 0
        Dim extraWt As Double = 0
        Dim salValue As Double = 0
        For cnt As Integer = 0 To rowIndex
            If gridTotalView.Item("RESULT", cnt).Value.ToString.Trim <> "1" Then Continue For
            pcs += Val(gridTotalView.Item("Pcs", cnt).Value.ToString)
            GrsWt += Val(gridTotalView.Item("GrsWt", cnt).Value.ToString)
            lessWt += Val(gridTotalView.Item("lessWt", cnt).Value.ToString)
            netWt += Val(gridTotalView.Item("netWt", cnt).Value.ToString)
            extraWt += Val(gridTotalView.Item("extraWt", cnt).Value.ToString)
            salValue += Val(gridTotalView.Item("salValue", cnt).Value.ToString)
        Next
        If gridGrandTotal.Rows.Count > 1 Then
            gridGrandTotal.Item("Pcs", 1).Value = pcs
            gridGrandTotal.Item("GrsWt", 1).Value = Format(GrsWt, "0.000")
            gridGrandTotal.Item("lessWt", 1).Value = Format(lessWt, "0.000")
            gridGrandTotal.Item("netWt", 1).Value = Format(netWt, "0.000")
            gridGrandTotal.Item("extraWt", 1).Value = Format(extraWt, "0.000")
            gridGrandTotal.Item("salValue", 1).Value = Format(salValue, "0.00")
        End If
    End Function

    Function funcFiltrationString(Optional ByVal CTag As Boolean = False) As String
        Dim str As String = Nothing
        str = " WHERE " & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & " Between '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' and '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' "
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            str += vbCrLf + " and t.itemid in (select itemid from " & cnAdminDb & "..itemmast where METALID in (select Metalid from " & cnAdminDb & "..MetalMast where MetalName IN (" & GetQryString(chkCmbMetal.Text) & ")))"
        End If
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            str += vbCrLf + " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            'str += vbCrLf +  " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        'str += vbCrLf +  " AND COMPANYID = '" & GetStockCompId() & "'"
        If txtItemCode_NUM.Text <> "" Then
            str += vbCrLf + " and t.itemId = '" & txtItemCode_NUM.Text & "'"
        End If
        If cmbSubItemGroup.Text <> "ALL" And cmbSubItemGroup.Text <> "" Then
            str += vbCrLf + " and t.subItemId IN (select subItemId from " & cnAdminDb & "..SubItemMast where  "
            str += vbCrLf + " sgroupid in (select sgroupid from " & cnAdminDb & "..SubItemgroup where sgroupname in (" & GetQryString(cmbSubItemGroup.Text, ",") & ")))"
        End If
        If cmbSubItemName.Enabled = True Then
            If cmbSubItemName.Text <> "ALL" And cmbSubItemName.Text <> "" Then
                'str += vbCrLf +  " and t.subItemId = isnull((select subItemId from " & cnAdminDb & "..SubItemMast where subItemName = '" & cmbSubItemName.Text & "' and itemid = " & Val(txtItemCode_NUM.Text) & "),0)"
                str += vbCrLf + " and t.subItemId IN (select subItemId from " & cnAdminDb & "..SubItemMast where subItemName IN (" & GetQryString(cmbSubItemName.Text) & ") and itemid = " & Val(txtItemCode_NUM.Text) & ")"
            End If
        End If
        'If txtTagNo.Text <> "" Then
        '    str += vbCrLf + " and t.tagno = '" & txtTagNo.Text & "'"
        'End If
        If txtTagNo.Text <> "" And txtTagNo_To.Text = "" Then
            str += vbCrLf + " and t.tagno = '" & txtTagNo.Text & "'"
        End If
        If txtTagNo.Text <> "" And txtTagNo_To.Text <> "" Then
            str += vbCrLf + " and t.tagno BETWEEN '" & txtTagNo.Text & "' AND '" & txtTagNo_To.Text & "'"
        End If
        'If chkCmbDesigner.Text <> "ALL" And chkCmbDesigner.Text <> "" Then
        '    str += vbCrLf +  " and DesignerId IN (Select DesignerId from " & cnAdminDb & "..Designer where DesignerName IN (" & GetQryString(chkCmbDesigner.Text) & "))"
        'End If
        If cmbCounterName.Text <> "ALL" And cmbCounterName.Text <> "" Then
            'str += vbCrLf +  " and ItemCtrId = (Select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName = '" & cmbCounterName.Text & "')"
            str += vbCrLf + " and ItemCtrId IN (Select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName IN (" & GetQryString(cmbCounterName.Text) & "))"
        End If
        If cmbItemType.Text <> "ALL" And cmbItemType.Text <> "" Then
            str += vbCrLf + " and itemTypeId = (select itemTypeId from " & cnAdminDb & "..itemType where Name = '" & cmbItemType.Text & "')"
        End If

        'If cmbCostCenter.Text <> "ALL" And cmbCostCenter.Text <> "" Then
        '    str += vbCrLf +  " and " & IIf(ChkOrgCostCentre.Checked = True, "TCOSTID", "COSTID") & " = (select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCenter.Text & "')"
        'End If

        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            str += vbCrLf + " AND COSTID IN"
            str += vbCrLf + "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If

        If chkLotIssue.Checked = True Then
            If txtLotNoFrom_NUM.Text <> "" And txtLotNoTo_NUM.Text = "" Then
                str += " AND LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO IN (SELECT LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE TRANNO = '" & txtLotNoFrom_NUM.Text.Trim & "'))"
            ElseIf txtLotNoFrom_NUM.Text <> "" And txtLotNoTo_NUM.Text <> "" Then
                str += " AND LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO IN (SELECT LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE TRANNO BETWEEN '" & txtLotNoFrom_NUM.Text.Trim & "' AND '" & txtLotNoTo_NUM.Text.Trim & "'))"
            End If
        Else
            If txtLotNoFrom_NUM.Text <> "" And txtLotNoTo_NUM.Text = "" Then
                'str += vbCrLf +  " AND LOTSNO = '" & txtLotNo_NUM.Text & "'"
                ''str += vbCrLf + " and LOTSNO IN (SELECT CASE WHEN ISNULL((SELECT TOP 1 SNO FROM " & cnAdminDb & "..ITEMLOT WHERE NEWSNO =V.SNO),'') = '' THEN SNO ELSE (SELECT TOP 1 SNO FROM " & cnAdminDb & "..ITEMLOT WHERE NEWSNO =V.SNO) END FROM " & cnAdminDb & "..ITEMLOT AS V WHERE LOTNO = '" & txtLotNoFrom_NUM.Text & "')"
                str += vbCrLf + " and LOTSNO IN (SELECT SNO  FROM " & cnAdminDb & "..ITEMLOT AS V WHERE LOTNO = '" & txtLotNoFrom_NUM.Text & "')"
            ElseIf txtLotNoFrom_NUM.Text <> "" And txtLotNoTo_NUM.Text <> "" Then
                ''str += vbCrLf + " and LOTSNO IN (SELECT CASE WHEN ISNULL((SELECT TOP 1 SNO FROM " & cnAdminDb & "..ITEMLOT WHERE NEWSNO =V.SNO),'') = '' THEN SNO ELSE (SELECT TOP 1 SNO FROM " & cnAdminDb & "..ITEMLOT WHERE NEWSNO =V.SNO) END FROM " & cnAdminDb & "..ITEMLOT AS V WHERE LOTNO BETWEEN '" & txtLotNoFrom_NUM.Text & "' AND '" & txtLotNoTo_NUM.Text & "')"
                str += vbCrLf + " and LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT AS V WHERE LOTNO BETWEEN '" & txtLotNoFrom_NUM.Text & "' AND '" & txtLotNoTo_NUM.Text & "')"
            End If
        End If

        If Studded = True Then
            If chkCmbStoneName.Items.Count > 0 Or chkCmbStSubItemName.Items.Count > 0 Then
                'If chkCmbStoneName.GetItemChecked(0) = False Then
                If (chkCmbStoneName.Text <> "ALL" And chkCmbStoneName.Text <> "") Or (chkCmbStSubItemName.Text <> "ALL" And chkCmbStSubItemName.Text <> "") Then
                    str += " AND ISNULL(SNO,'') IN (SELECT TAGSNO FROM " & cnAdminDb & "..ITEMTAGSTONE "
                    If chkCmbStoneName.Items.Count > 0 Then
                        If chkCmbStoneName.GetItemChecked(0) = False Then
                            str += " WHERE STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') IN (" & GetQryString(chkCmbStoneName.Text, ",") & "))"
                        End If
                    End If
                    If chkCmbStSubItemName.Items.Count > 0 Then
                        If chkCmbStSubItemName.GetItemChecked(0) = False Then
                            If chkCmbStoneName.Items.Count > 0 Then
                                If chkCmbStoneName.GetItemChecked(0) = False Then
                                    str += " AND "
                                Else
                                    str += " WHERE "
                                End If
                            End If
                            str += " STNSUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') IN (" & GetQryString(chkCmbStSubItemName.Text, ",") & "))"
                        End If
                    End If
                    str += ")"
                    'End If
                End If
            End If
        End If

        If txtFromWt_WET.Text <> "" Or txtToWt_WET.Text <> "" Then
            str += vbCrLf + " and (T.GrsWt between '" & Val(txtFromWt_WET.Text) & "' and '" & Val(txtToWt_WET.Text) & "')"
        End If
        If txtFromDiaWt_WET.Text <> "" Or txtToDiaWt_WET.Text <> "" Then
            str += vbCrLf + " and ((select sum(stnwt) from " & cnAdminDb & ".." & IIf(CTag, "CITEMTAGSTONE", "ITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) between '" & Val(txtFromDiaWt_WET.Text) & "' and '" & Val(txtToDiaWt_WET.Text) & "')"
        End If
        If txtFromRate_WET.Text <> "" Or txtToRate_WET.Text <> "" Then
            str += vbCrLf + " and (SalValue between '" & Val(txtFromRate_WET.Text) & "' and '" & Val(txtToRate_WET.Text) & "')"
        End If
        If rbtRegular.Checked = True Then
            str += vbCrLf + " and ordRepNo =''"
        End If
        If rbtOrder.Checked = True Then
            str += vbCrLf + " and ordRepNo <> ''"
        End If
        If cmbSearchKey.Text <> "" And txtSearch_txt.Text <> "" Then
            str += vbCrLf + " AND T." & cmbSearchKey.Text & " LIKE"
            str += vbCrLf + " '" & txtSearch_txt.Text & "%'"
        End If
        If OTHmaster = True Then
            If objMoreOption.chkothermisc.Text <> "" Then
                str += vbCrLf + " AND T.SNO IN (SELECT TAGSNO FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE  OTHID IN(SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME IN (" & GetQryString(objMoreOption.chkothermisc.Text) & ")))"
            End If
        End If
        If ChkMultimetal.Checked = True Then
            str += vbCrLf + " AND T.SNO IN (SELECT TAGSNO FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE RECDATE between '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' and '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' ) "
        End If
        ''Get Designerid
        If (chkCmbDesigner.Text = "ALL" And chkMultiSelect.Checked) Or (CmbDesigner.Text = "ALL" And chkMultiSelect.Checked = False) Then
            strSql = " SELECT DESIGNERNAME  FROM " & cnAdminDb & "..DESIGNER"
            strSql += " ORDER BY DESIGNERNAME"
            Dim dt As New DataTable()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    cmbDesName += "'"
                    cmbDesName += dt.Rows(i).Item("DESIGNERNAME").ToString
                    cmbDesName += "'"
                    cmbDesName += ","
                Next
                If cmbDesName <> "" Then
                    cmbDesName = Mid(cmbDesName, 1, cmbDesName.Length - 1)
                End If
            End If
        ElseIf chkCmbDesigner.Text <> "" And chkMultiSelect.Checked Then
            Dim sql As String = "SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & GetQryString(chkCmbDesigner.Text) & ")"
            Dim dtDesigner As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtDesigner)
            If dtDesigner.Rows.Count > 0 Then
                For i As Integer = 0 To dtDesigner.Rows.Count - 1
                    cmbDesName += "'"
                    cmbDesName += dtDesigner.Rows(i).Item("DESIGNERNAME").ToString
                    cmbDesName += "'"
                    cmbDesName += ","
                Next
                If cmbDesName <> "" Then
                    cmbDesName = Mid(cmbDesName, 1, cmbDesName.Length - 1)
                End If
                'selItemId += "'"
            End If
        ElseIf CmbDesigner.Text <> "" And chkMultiSelect.Checked = False Then
            cmbDesName = "'" & CmbDesigner.Text & "'"
        End If
        If cmbDesName.ToString <> "" Then
            str += vbCrLf + " AND DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & cmbDesName & "))"
        End If

        If cmbLotEntryType.Text <> "" And cmbLotEntryType.Text <> "ALL" Then
            str += vbCrLf + " AND ISNULL(CASE WHEN ISNULL(T.ENTRYTYPE,'')='' THEN 'R' ELSE T.ENTRYTYPE  END,'') ='" & getLotentrytype(cmbLotEntryType.Text.ToString) & "'"
        End If

        If chkCmbUserName.Text <> "ALL" And chkCmbUserName.Text <> "" Then
            str += vbCrLf + " AND T.USERID IN (SELECT USERID FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME IN (" & GetQryString(chkCmbUserName.Text) & "))"
        End If

        If chkMRDetails.Checked Then
            Dim trandbName As String = ""
            strSql = "SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE '" & dtpMRDate.Value.ToString("yyyy-MM-dd") & "' BETWEEN STARTDATE AND ENDDATE"
            trandbName = GetSqlValue(cn, strSql)
            If trandbName = "" Then trandbName = cnStockDb
            str += vbCrLf + " AND LOTSNO IN ("
            str += vbCrLf + " SELECT DISTINCT L.LOTSNO FROM " & cnStockDb & "..RECEIPT R INNER JOIN " & trandbName & "..LOTISSUE L ON  R.SNO = L.RECSNO "
            str += vbCrLf + "  WHERE R.TRANDATE = '" & dtpMRDate.Value.ToString("yyyy-MM-dd") & "' AND R.TRANNO = '" & txtMRTranNO.Text & "' AND ISNULL(R.CANCEL,'') = '')"
        End If
        Return str
    End Function

    Private Function getLotentrytype(ByVal _lotentryType As String) As String
        Dim entryType As String = Nothing
        Select Case _lotentryType.ToString.ToUpper
            Case "REGULAR"
                entryType = "R"
            Case "ORDER"
                entryType = "OR"
            Case "REPAIR"
                entryType = "RE"
            Case "WORK ORDER"
                entryType = "WO"
            Case "ALTERATION"
                entryType = "AL"
            Case "PARTLY SALE"
                entryType = "PS"
            Case "OLD"
                entryType = "OO"
            Case "SALES RETURN"
                entryType = "SR"
            Case "NONTAG TO TAG"
                entryType = "NT"
            Case Else
                entryType = "R"
        End Select
        Return entryType
    End Function

    Private Sub frmTagedItemsStockViewReceiptView_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPITEMTAGTITLE')> 0"
        strSql += "     DROP TABLE TEMPITEMTAGTITLE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPITEMTAGSTOCK')> 0"
        strSql += "     DROP TABLE TEMPITEMTAGSTOCK"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPITEMTAGSTONESTOCK')> 0"
        strSql += "    DROP TABLE TEMPITEMTAGSTONESTOCK"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub frmTagedItemsStockView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagedItemsStockView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        If Not defaultPic.EndsWith("\") Then defaultPic += "\"
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate

        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        grpFiltration.Location = New Point((ScreenWid - grpFiltration.Width) / 2, ((ScreenHit - 128) - grpFiltration.Height) / 2)

        pnlTotalGridView.Dock = DockStyle.Fill

        headerBgColor = System.Drawing.SystemColors.ControlLight

        tempDt.Columns.Add("MaxDescription", GetType(String))
        tempDt.Columns.Add("MaxValues", GetType(String))
        tempDt.Columns.Add("MinDescription", GetType(String))
        tempDt.Columns.Add("MinValues", GetType(String))
        tempDt.Columns.Add("OtherDesc1", GetType(String))
        tempDt.Columns.Add("OtherVal1", GetType(String))
        tempDt.Columns.Add("OtherDesc2", GetType(String))
        tempDt.Columns.Add("OtherVal2", GetType(String))

        dtStoneDetails.Columns.Add("Description", GetType(String))
        dtStoneDetails.Columns.Add("Diamond", GetType(String))
        dtStoneDetails.Columns.Add("Stone", GetType(String))
        dtStoneDetails.Columns.Add("Precious", GetType(String))

        dtGrandTotalDetails.Columns.Add("Description", GetType(String))
        dtGrandTotalDetails.Columns.Add("Pcs", GetType(String))
        dtGrandTotalDetails.Columns.Add("GrsWt", GetType(String))
        dtGrandTotalDetails.Columns.Add("LessWt", GetType(String))
        dtGrandTotalDetails.Columns.Add("ExtraWt", GetType(String))
        dtGrandTotalDetails.Columns.Add("NetWt", GetType(String))
        dtGrandTotalDetails.Columns.Add("SalValue", GetType(String))

        ''Checking CostCentre Status
        strSql = " select 1 from " & cnAdminDb & "..softcontrol where ctlText = 'Y' and ctlId = 'COSTCENTRE'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    cmbCostCenter.Enabled = True
        'Else
        '    cmbCostCenter.Enabled = False
        'End If
        If dt.Rows.Count > 0 Then
            chkCmbCostCentre.Enabled = True
        Else
            chkCmbCostCentre.Enabled = False
        End If

        strSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
        strSql += " ORDER BY RESULT,METALNAME"
        dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)

        'Load Designer
        strSql = " SELECT 'ALL' DESIGNERNAME,'ALL' DESIGNERID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT DESIGNERNAME,CONVERT(VARCHAR,DESIGNERID),2 RESULT FROM " & cnAdminDb & "..DESIGNER"
        strSql += " ORDER BY RESULT,DESIGNERNAME"
        dtDesigner = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDesigner)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbDesigner, dtDesigner, "DESIGNERNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(CmbDesigner, dtDesigner, "DESIGNERNAME", )

        ''UserName 
        Dim dtUser As New DataTable
        strSql = " SELECT 'ALL' USERNAME,'0' USERID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT USERNAME,USERID,2 RESULT FROM " & cnAdminDb & "..USERMASTER "
        strSql += " ORDER BY RESULT,USERNAME"
        dtUser = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtUser)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbUserName, dtUser, "USERNAME", , "ALL")

        SqlVersion = ""
        strSql = " SELECT	SUBSTRING(CONVERT(VARCHAR,SERVERPROPERTY('PRODUCTVERSION')),1,CHARINDEX('.',CONVERT(VARCHAR,SERVERPROPERTY('PRODUCTVERSION')))-1) AS [VERSION]"
        SqlVersion = GetSqlValue(cn, strSql)


        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If chkMRDetails.Checked And dtpMRDate.Value > dtpFrom.Value Then MsgBox("Meterial Receipt Date Should Be Below Recdate") : Exit Sub
        If chkExport.Checked = True Then
            Dim obj As New frmTagedItemStockExportM1(Val(txtItemCode_NUM.Text), 0, Format(dtpFrom.Value.Date, "yyyy-MM-dd") _
                                                     , Format(dtpTo.Value.Date, "yyyy-MM-dd"), GetQryString(chkCmbCostCentre.Text, ","), "RECEIPT")
            obj.ShowDialog()
            Exit Sub
        End If
        If chkDetailed.Checked Then
            DetailedReport()
            Exit Sub
        End If
        Dim dt As New DataTable("StockView")
        dt.Clear()
        tabView.Show()
        OtherView()
        Try
            btnView_Search.Enabled = False
            OTHmaster = False
            If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE ISNULL(ACTIVE,'')<>'N'", , "", )) > 0 Then
                If objMoreOption.Visible Then Exit Sub
                OTHmaster = True
                objMoreOption.BackColor = Me.BackColor
                objMoreOption.StartPosition = FormStartPosition.CenterScreen
                objMoreOption.MaximizeBox = False
                objMoreOption.ShowDialog()
                btnView_Search.Focus()
            End If
            strSql = vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "TAGRECEIPTVIEW')>0"
            strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW"
            strSql += vbCrLf + " DECLARE @DEFPATH VARCHAR(200)"
            strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
            strSql += vbCrLf + " SELECT CONVERT(INT,NULL)SLNO,CONVERT(VARCHAR(15),SNO)SNO,"
            If chkitemname.Checked = True Then
                strSql += vbCrLf + " CASE WHEN ISNULL(SM.SUBITEMNAME,'')<>''  THEN "
                strSql += vbCrLf + " SM.SUBITEMNAME"
                strSql += vbCrLf + " ELSE IM.ITEMNAME END AS PARTICULAR,IM.ITEMNAME AS ITEMNAME ,"
            Else
                strSql += vbCrLf + " CASE WHEN ISNULL(SM.SUBITEMNAME,'')<>''  THEN "
                strSql += vbCrLf + " SM.SUBITEMNAME"
                strSql += vbCrLf + " ELSE IM.ITEMNAME END AS PARTICULAR,"
            End If
            strSql += vbCrLf + " " & IIf(chkActualDate.Checked, "ACTUALRECDATE RECDATE1", "RECDATE RECDATE1") & ","
            strSql += vbCrLf + " CASE WHEN " & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & " IS NULL THEN '' ELSE " & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & " END AS RECDATE,ACTUALRECDATE,"
            strSql += vbCrLf + " (SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=(SELECT CASE WHEN ISNULL(NEWSNO,'')<>'' THEN NEWSNO ELSE SNO END FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=T.LOTSNO))LOTNO,"
            strSql += vbCrLf + " TAGNO,CASE WHEN T.PCS <> 0 THEN T.PCS ELSE NULL END PCS,CASE WHEN T.GRSWT <> 0 THEN T.GRSWT ELSE NULL END GRSWT,CASE WHEN LESSWT <> 0 THEN LESSWT ELSE NULL END LESSWT,"

            strSql += vbCrLf + " CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END NETWT "
            ''strSql += vbCrLf + " ,CASE WHEN T.EXTRAWT <> 0 THEN T.EXTRAWT ELSE NULL END EXTRAWT "
            strSql += vbCrLf + " ,CASE WHEN T.EXTRAWT <> 0 THEN T.EXTRAWT WHEN (SELECT SUM(GRSWT)  FROM " & cnAdminDb & ".. ORMAST   WHERE SNO = T.ORSNO  AND ORNO = T. ORDREPNO ) <> 0 AND ISNULL(T.GRSWT,0)<>0 THEN ISNULL(T.GRSWT,0) - (SELECT GRSWT FROM " & cnAdminDb & ".. ORMAST   WHERE SNO = T.ORSNO  AND ORNO = T. ORDREPNO  )  ELSE NULL END EXTRAWT"
            strSql += vbCrLf + " ,CASE WHEN T.TOUCH <> 0 THEN T.TOUCH ELSE NULL END AS TOUCH,GRSNET"
            strSql += vbCrLf + " ,CASE WHEN RATE <> 0 THEN RATE ELSE NULL END RATE,CASE WHEN SALVALUE <> 0 THEN SALVALUE ELSE NULL END SALVALUE,"

            strSql += vbCrLf + " ISNULL((SELECT ISNULL(SUM(S.PURVALUE),0) PURVALUE FROM " & cnAdminDb & "..PURITEMTAG S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND S.TAGSNO = T.SNO),0) PURVALUE ,"


            strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNPCS),0) SPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNPCS ,"

            strSql += vbCrLf + " (SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT ELSE (CASE WHEN S.STONEUNIT = 'C' THEN CONVERT(NUMERIC(15,3),S.STNWT) END) END),0) STNWT "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.STONEUNIT ='G' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWT ,"
            'STN CARAT
            strSql += vbCrLf + " (SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT ELSE (CASE WHEN S.STONEUNIT = 'C' THEN CONVERT(NUMERIC(15,3),S.STNWT) END) END),0) STNWT "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.STONEUNIT ='C' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWTC ,"

            'stnamt
            strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNAMT),0) SAMT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNAMT ,"

            strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNPCS),0) DPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAPCS ,"

            'strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT "
            'strSql += vbCrLf + " ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            'strSql += vbCrLf + " S.STONEUNIT ='G' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT ,"

            'DIA CARAT

            'strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT "
            'strSql += vbCrLf + " ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            'strSql += vbCrLf + " S.STONEUNIT ='C' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT ,"

            strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 "
            strSql += vbCrLf + " ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT ,"

            'DiaAmt
            strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNAMT),0) DAMT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAAMT ,"

            strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNPCS),0) PPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREPCS ,"

            strSql += vbCrLf + " (SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) PREWT  FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREWT ,"

            'PreAmt
            strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNAMT),0) PAMT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREAMT ,"

            strSql += vbCrLf + " CASE WHEN MAXWASTPER <> 0 THEN MAXWASTPER ELSE NULL END AS [WAST%],CASE WHEN MAXWAST <> 0 THEN MAXWAST ELSE NULL END [WAST],"
            strSql += vbCrLf + " CASE WHEN MAXMCGRM <> 0 THEN MAXMCGRM ELSE NULL END AS [MCGRM],CASE WHEN MAXMC <> 0 THEN MAXMC ELSE NULL END [MC],"
            strSql += vbCrLf + " (SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)TAGTYPE,"
            strSql += vbCrLf + " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)COUNTER,"
            strSql += vbCrLf + " T.TABLECODE,ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)),CASE WHEN PURITY<> 0 THEN PURITY ELSE NULL END)AS PURITY,RTRIM(LTRIM(NARRATION))NARRATION,BRANDID,"
            strSql += vbCrLf + " SALEMODE,@DEFPATH + T.PCTFILE AS PCTFILE,TRANINVNO,SUPBILLNO/*,HM_BILLNO HALLMARKNO*/"
            If Val(SqlVersion) > 8 Then
                strSql += vbCrLf + " , (STUFF((SELECT CAST(',' + H.[HM_BILLNO] AS VARCHAR(MAX)) FROM " & cnAdminDb & "..ITEMTAGHALLMARK H WHERE (T.SNO =H.TAGSNO) FOR XML PATH ('')), 1, 1, '')) HALLMARKNO"
            Else
                strSql += vbCrLf + " ,HM_BILLNO HALLMARKNO"
            End If
            strSql += vbCrLf + " ,ENTRYMODE,T.ITEMID,CONVERT(VARCHAR(1),'')COLHEAD,TAGVAL,1 RESULT"
            strSql += vbCrLf + " ,(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = T.USERID)AS USERNAME,T.STYLENO"
            strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T." & IIf(ChkOrgCostCentre.Checked = True, "TCOSTID", "COSTID") & ")AS COSTCENTRE"

            strSql += vbCrLf + " ,IM.ITEMNAME aS ITEM"
            strSql += vbCrLf + " ,SM.SUBITEMNAME aS SUBITEM"
            strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
            strSql += vbCrLf + " ,(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)AS ITEMSIZE,T.SIZEID AS ITEMSIZEID"
            strSql += vbCrLf + " ,CASE  "
            strSql += vbCrLf + " WHEN ISNULL(ISSDATE,'') = '' AND (TOFLAG = 'AI' OR ISNULL(T.APPROVAL,'')='A') THEN 'APPROVAL ISSUE'"
            strSql += vbCrLf + " WHEN ISNULL(ISSDATE,'') <> '' AND TOFLAG = 'TR' THEN 'TRANSFERED'"
            strSql += vbCrLf + " WHEN ISNULL(ISSDATE,'') <> '' AND TOFLAG = 'MI' THEN 'MISC ISSUE'"
            strSql += vbCrLf + " WHEN ISNULL(ISSDATE,'') <> '' AND (TOFLAG = 'AI' OR ISNULL(T.APPROVAL,'')='A') THEN 'APPROVAL ISSUE'"
            strSql += vbCrLf + " WHEN ISNULL(ISSDATE,'') <> '' AND TOFLAG = 'SA' THEN 'SALES' WHEN ISSDATE IS NULL AND TOFLAG = '' THEN 'LIVE'  END STATUS"
            strSql += vbCrLf + " ,ISSDATE,CASE WHEN ISSWT > 0 THEN ISSWT ELSE NULL END ISSUE_WEIGHT,CONVERT(VARCHAR(3),ISNULL(T.APPROVAL,''))APPROVAL,ORDREPNO"
            If OTHmaster = True Then
                If dtOtherMaster.Rows.Count > 0 Then
                    For i As Integer = 0 To dtOtherMaster.Rows.Count - 1
                        strSql += vbCrLf + ", (SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE ID IN "
                        strSql += vbCrLf + " (SELECT TOP 1 OTHID FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE TAGSNO=T.SNO "
                        strSql += vbCrLf + " AND OTHID IN (SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID IN ( "
                        strSql += vbCrLf + " (SELECT TOP 1 MISCID FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID='" & dtOtherMaster.Rows(i).Item("ID").ToString & "'))))) "
                        strSql += vbCrLf + " AS [" & dtOtherMaster.Rows(i).Item("NAME").ToString & "]"
                    Next
                End If
            End If
            strSql += vbCrLf + " ,(SELECT SGROUPNAME FROM " & cnAdminDb & "..SUBITEMGROUP WHERE SGROUPID=SM.SGROUPID) SUBITEMGROUP"
            strSql += vbCrLf + " ,CASE WHEN T.ENTRYTYPE = 'R' THEN 'REGULAR' WHEN T.ENTRYTYPE = 'OR' THEN 'ORDER' WHEN T.ENTRYTYPE = 'RE' THEN 'REPAIR' WHEN T.ENTRYTYPE = 'SR' THEN 'SALES RETURN' "
            strSql += vbCrLf + " WHEN T.ENTRYTYPE = 'OO' THEN 'OLD' WHEN T.ENTRYTYPE = 'PS' THEN 'PARTLY SALE' WHEN T.ENTRYTYPE = 'AL' THEN 'ALTERATION' WHEN T.ENTRYTYPE = 'WO' THEN 'WORK ORDER' "
            strSql += vbCrLf + " WHEN T.ENTRYTYPE = 'NT' THEN 'NONTAG TO TAG' ELSE 'LOT' END AS LOT_ENTRYTYPE "
            'strSql += vbCrLf + " ,(SELECT LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=T.LOTSNO)LOTNO"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = T.ITEMID AND SM.SUBITEMID = T.SUBITEMID"
            strSql += funcFiltrationString()
            If chkWithCumulative.Checked Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT CONVERT(INT,NULL)SLNO,CONVERT(VARCHAR(15),SNO)SNO,"
                If chkitemname.Checked = True Then
                    strSql += vbCrLf + " CASE WHEN ISNULL(SM.SUBITEMNAME,'')<>''  THEN "
                    strSql += vbCrLf + " SM.SUBITEMNAME"
                    strSql += vbCrLf + " ELSE IM.ITEMNAME END AS PARTICULAR,IM.ITEMNAME AS ITEMNAME ,"
                Else
                    strSql += vbCrLf + " CASE WHEN ISNULL(SM.SUBITEMNAME,'')<>''  THEN "
                    strSql += vbCrLf + " SM.SUBITEMNAME"
                    strSql += vbCrLf + " ELSE IM.ITEMNAME END AS PARTICULAR,"
                End If
                strSql += vbCrLf + " " & IIf(chkActualDate.Checked, "ACTUALRECDATE RECDATE1", "RECDATE RECDATE1") & ","
                strSql += vbCrLf + " CASE WHEN " & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & " IS NULL THEN '' ELSE " & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & " END AS RECDATE,ACTUALRECDATE,"
                strSql += vbCrLf + " (SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=(SELECT CASE WHEN ISNULL(NEWSNO,'')<>'' THEN NEWSNO ELSE SNO END FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=T.LOTSNO))LOTNO,"
                strSql += vbCrLf + " TAGNO,CASE WHEN T.PCS <> 0 THEN T.PCS ELSE NULL END PCS,CASE WHEN T.GRSWT <> 0 THEN T.GRSWT ELSE NULL END GRSWT,CASE WHEN LESSWT <> 0 THEN LESSWT ELSE NULL END LESSWT,"
                strSql += vbCrLf + " CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END NETWT"
                ''strSql += vbCrLf + " ,CASE WHEN T.EXTRAWT <> 0 THEN T.EXTRAWT ELSE NULL END EXTRAWT "
                strSql += vbCrLf + " ,CASE WHEN T.EXTRAWT <> 0 THEN T.EXTRAWT WHEN (SELECT SUM(GRSWT)  FROM " & cnAdminDb & ".. ORMAST   WHERE SNO = T.ORSNO  AND ORNO = T. ORDREPNO ) <> 0 AND ISNULL(T.GRSWT,0)<>0 THEN ISNULL(T.GRSWT,0) - (SELECT GRSWT FROM " & cnAdminDb & ".. ORMAST   WHERE SNO = T.ORSNO  AND ORNO = T. ORDREPNO  )  ELSE NULL END EXTRAWT"
                strSql += vbCrLf + " ,CASE WHEN T.TOUCH <> 0 THEN T.TOUCH ELSE NULL END AS TOUCH,GRSNET,CASE WHEN RATE <> 0 THEN RATE ELSE NULL END RATE,CASE WHEN SALVALUE <> 0 THEN SALVALUE ELSE NULL END SALVALUE,"

                strSql += vbCrLf + " ISNULL((SELECT ISNULL(SUM(S.PURVALUE),0) PURVALUE FROM " & cnAdminDb & "..PURITEMTAG S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID And S.TAGNO = T.TAGNO And S.TAGSNO = T.SNO),0) PURVALUE ,"

                strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNPCS),0) SPCS FROM " & cnAdminDb & "..CITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.COSTID = T.COSTID And S.ITEMID = T.ITEMID And S.TAGNO = T.TAGNO And STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNPCS ,"

                strSql += vbCrLf + " (SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT  ELSE (CASE WHEN S.STONEUNIT = 'C' THEN CONVERT(NUMERIC(15,3),S.STNWT) END) END),0) STNWT "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.COSTID = T.COSTID AND S.STONEUNIT ='G' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWT ,"
                'STN CARAT
                strSql += vbCrLf + " (SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT  ELSE (CASE WHEN S.STONEUNIT = 'C' THEN CONVERT(NUMERIC(15,3),S.STNWT) END) END),0) STNWT "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAGSTONE S WHERE S.STONEUNIT ='C' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWTC ,"

                'stnamt
                strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNAMT),0) SAMT FROM " & cnAdminDb & "..CITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNAMT ,"

                strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNPCS),0) DPCS FROM " & cnAdminDb & "..CITEMTAGSTONE S WHERE "
                strSql += vbCrLf + "  S.RECDATE = T." & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & " "
                strSql += vbCrLf + " AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAPCS ,"

                'strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT "
                'strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAGSTONE S WHERE S.STONEUNIT ='G' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT ,"
                'DIA CARAT
                'strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT "
                'strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAGSTONE S WHERE S.STONEUNIT ='C' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT ,"

                strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAGSTONE S WHERE S.RECDATE = T." & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & " AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT ,"

                'DiaAmt
                strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNAMT),0) DAMT FROM " & cnAdminDb & "..CITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAAMT ,"

                strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNPCS),0) PPCS FROM " & cnAdminDb & "..CITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREPCS ,"

                strSql += vbCrLf + " (SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) PREWT "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAGSTONE S WHERE S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREWT ,"

                'PreAmt
                strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNAMT),0) PAMT FROM " & cnAdminDb & "..CITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREAMT ,"


                strSql += vbCrLf + " CASE WHEN MAXWASTPER <> 0 THEN MAXWASTPER ELSE NULL END AS [WAST%],CASE WHEN MAXWAST <> 0 THEN MAXWAST ELSE NULL END [WAST],"
                strSql += vbCrLf + " CASE WHEN MAXMCGRM <> 0 THEN MAXMCGRM ELSE NULL END AS [MCGRM],CASE WHEN MAXMC <> 0 THEN MAXMC ELSE NULL END [MC],"
                strSql += vbCrLf + " (SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)ITEMTYPE,"
                strSql += vbCrLf + " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)ITEMCTRNAME,"
                strSql += vbCrLf + " T.TABLECODE,ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)),CASE WHEN PURITY<> 0 THEN PURITY ELSE NULL END)AS PURITY,RTRIM(LTRIM(NARRATION))NARRATION,BRANDID,"
                strSql += vbCrLf + " SALEMODE,@DEFPATH + T.PCTFILE AS PCTFILE,TRANINVNO,SUPBILLNO/*,HM_BILLNO HALLMARKNO*/"
                If Val(SqlVersion) > 8 Then
                    strSql += vbCrLf + " , (STUFF((SELECT CAST(',' + H.[HM_BILLNO] AS VARCHAR(MAX)) FROM " & cnAdminDb & "..ITEMTAGHALLMARK H WHERE (T.SNO =H.TAGSNO) FOR XML PATH ('')), 1, 1, '')) HALLMARKNO"
                Else
                    strSql += vbCrLf + " ,HM_BILLNO HALLMARKNO"
                End If
                strSql += vbCrLf + " ,ENTRYMODE,T.ITEMID,CONVERT(VARCHAR(1),'')COLHEAD,TAGVAL,1 RESULT"
                strSql += vbCrLf + " ,(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = T.USERID)AS USERNAME,T.STYLENO"
                strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T." & IIf(ChkOrgCostCentre.Checked = True, "TCOSTID", "COSTID") & ")AS COSTCENTRE"
                strSql += vbCrLf + " ,IM.ITEMNAME aS ITEM"
                strSql += vbCrLf + " ,SM.SUBITEMNAME aS SUBITEM"
                strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
                strSql += vbCrLf + " ,(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)AS ITEMSIZE,T.SIZEID AS ITEMSIZEID"
                strSql += vbCrLf + " ,CASE  "
                strSql += vbCrLf + " WHEN ISNULL(ISSDATE,'') = '' AND (TOFLAG = 'AI' OR ISNULL(T.APPROVAL,'')='A') THEN 'APPROVAL ISSUE'"
                strSql += vbCrLf + " WHEN ISNULL(ISSDATE,'') <> '' AND TOFLAG = 'TR' THEN 'TRANSFERED'"
                strSql += vbCrLf + " WHEN ISNULL(ISSDATE,'') <> '' AND TOFLAG = 'MI' THEN 'MISC ISSUE'"
                strSql += vbCrLf + " WHEN ISNULL(ISSDATE,'') <> '' AND (TOFLAG = 'AI' OR ISNULL(T.APPROVAL,'')='A') THEN 'APPROVAL ISSUE'"
                strSql += vbCrLf + " WHEN ISNULL(ISSDATE,'') <> '' AND TOFLAG = 'SA' THEN 'SALES' WHEN ISSDATE IS NULL AND TOFLAG = '' THEN 'LIVE' END STATUS"
                strSql += vbCrLf + " ,ISSDATE,CASE WHEN ISSWT > 0 THEN ISSWT ELSE NULL END ISSUE_WEIGHT,CONVERT(VARCHAR(3),ISNULL(T.APPROVAL,''))APPROVAL,ORDREPNO"
                If OTHmaster = True Then
                    If dtOtherMaster.Rows.Count > 0 Then
                        For i As Integer = 0 To dtOtherMaster.Rows.Count - 1
                            strSql += vbCrLf + ", (SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE ID IN "
                            strSql += vbCrLf + " (SELECT TOP 1 OTHID FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE TAGSNO=T.SNO "
                            strSql += vbCrLf + " AND OTHID IN (SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID IN ( "
                            strSql += vbCrLf + " (SELECT TOP 1 MISCID FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID='" & dtOtherMaster.Rows(i).Item("ID").ToString & "'))))) "
                            strSql += vbCrLf + " AS [" & dtOtherMaster.Rows(i).Item("NAME").ToString & "]"
                        Next
                    End If
                End If
                strSql += vbCrLf + " ,(SELECT SGROUPNAME FROM " & cnAdminDb & "..SUBITEMGROUP WHERE SGROUPID=SM.SGROUPID) SUBITEMGROUP"
                strSql += vbCrLf + " ,CASE WHEN T.ENTRYTYPE = 'R' THEN 'REGULAR' WHEN T.ENTRYTYPE = 'OR' THEN 'ORDER' WHEN T.ENTRYTYPE = 'RE' THEN 'REPAIR' WHEN T.ENTRYTYPE = 'SR' THEN 'SALES RETURN' "
                strSql += vbCrLf + " WHEN T.ENTRYTYPE = 'OO' THEN 'OLD' WHEN T.ENTRYTYPE = 'PS' THEN 'PARTLY SALE' WHEN T.ENTRYTYPE = 'AL' THEN 'ALTERATION' WHEN T.ENTRYTYPE = 'WO' THEN 'WORK ORDER' "
                strSql += vbCrLf + " WHEN T.ENTRYTYPE = 'NT' THEN 'NONTAG TO TAG' ELSE 'LOT' END AS LOT_ENTRYTYPE "
                'strSql += vbCrLf + " ,(SELECT LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=T.LOTSNO)LOTNO"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG AS T"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = T.ITEMID And SM.SUBITEMID = T.SUBITEMID"
                strSql += vbCrLf + funcFiltrationString(True)

                'strSql += vbCrLf + " And TAGNO Not IN  (SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG i WHERE   i.RECDATE Between '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' and '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'  AND I.TAGNO = T.TAGNO AND I.itemid = T.ITEMID And I.COMPANYID = T.COMPANYID And I.COSTID = T.COSTID)"
                strSql += vbCrLf + " AND TAGNO NOT IN  (SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG i WHERE   i.ACTUALRECDATE Between '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' and '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'  AND I.TAGNO = T.TAGNO AND I.itemid = T.ITEMID And I.COMPANYID = T.COMPANYID And I.COSTID = T.COSTID)"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 600
            cmd.ExecuteNonQuery()
            If LoosestnwtInGrswt = False Then
                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW"
                strSql += vbCrLf + " SET PCS = null,GRSWT = null,NETWT = null,DIAPCS = L.PCS"
                strSql += vbCrLf + " ,DIAWT = L.GRSWT"
                strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW AS L"
                strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'D')"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW"
                strSql += vbCrLf + " SET PCS = null,GRSWT = null,NETWT = null,STNPCS = L.PCS"
                strSql += vbCrLf + " ,STNWT = L.GRSWT"
                strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW AS L"
                strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'T')"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
            End If

            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW"
            dtSource = New DataTable
            dtSource.Columns.Add("KEYNO", GetType(Integer))
            dtSource.Columns("KEYNO").AutoIncrement = True
            dtSource.Columns("KEYNO").AutoIncrementSeed = 0
            dtSource.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSource)
            Dim ObjGrouper As New BrighttechPack.DataGridViewGrouper(gridTotalView, dtSource)
            For cnt As Integer = 0 To ChkLstGroupBy.CheckedItems.Count - 1
                ObjGrouper.pColumns_Group.Add(ChkLstGroupBy.CheckedItems.Item(cnt).ToString)
            Next
            Dim salemodeval As String = ""
            salemodeval = objGPack.GetSqlValue("SELECT CALTYPE FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemCode_NUM.Text) & "", , "")


            ObjGrouper.pColumns_Sum.Add("PCS")
            ObjGrouper.pColumns_Sum.Add("GRSWT")
            ObjGrouper.pColumns_Sum.Add("LESSWT")
            ObjGrouper.pColumns_Sum.Add("NETWT")
            ObjGrouper.pColumns_Sum.Add("EXTRAWT")
            ObjGrouper.pColumns_Sum.Add("SALVALUE")
            ObjGrouper.pColumns_Sum.Add("PURVALUE")
            ObjGrouper.pColumns_Sum.Add("STNPCS")
            ObjGrouper.pColumns_Sum.Add("STNWT")
            ObjGrouper.pColumns_Sum.Add("STNWTC")
            ObjGrouper.pColumns_Sum.Add("STNAMT")
            ObjGrouper.pColumns_Sum.Add("DIAPCS")
            ObjGrouper.pColumns_Sum.Add("DIAWT")
            'ObjGrouper.pColumns_Sum.Add("DIAWTC")
            ObjGrouper.pColumns_Sum.Add("DIAAMT")
            ObjGrouper.pColumns_Sum.Add("PREPCS")
            ObjGrouper.pColumns_Sum.Add("PREWT")
            ObjGrouper.pColumns_Sum.Add("PREAMT")
            ObjGrouper.pColumns_Sum.Add("WAST")
            ObjGrouper.pColumns_Sum.Add("MC")

            If salemodeval = "R" Then ObjGrouper.pColumns_Sum.Add("RATE")
            ObjGrouper.pColName_Particular = "PARTICULAR"
            ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
            '            ObjGrouper.pColumns_Sort = "TAGVAL"
            ObjGrouper.pIdentityColName = "SLNO"
            ObjGrouper.pColumns_Count.Add("TAGNO")

            Dim filter As String = ""
            'For i As Integer = 0 To LstOrderby.Items.Count - 1
            '    filter += IIf(LstOrderby.Items(i).ToString = "TAGNO", "TAGVAL", LstOrderby.Items(i).ToString)
            '    If i < LstOrderby.Items.Count - 1 Then filter += ","
            'Next

            For i As Integer = 0 To LstOrderby.Items.Count - 1
                If LstOrderby.Items(i).ToString = "TAGNO" Then
                    filter += IIf(LstOrderby.Items(i).ToString = "TAGNO", "TAGVAL", LstOrderby.Items(i).ToString)
                ElseIf LstOrderby.Items(i).ToString = "ITEMNAME" Then
                    filter += IIf(LstOrderby.Items(i).ToString = "ITEMNAME", "PARTICULAR", LstOrderby.Items(i).ToString)
                ElseIf LstOrderby.Items(i).ToString = "SALEVALUE" Then
                    filter += IIf(LstOrderby.Items(i).ToString = "SALEVALUE", "SALVALUE", LstOrderby.Items(i).ToString)
                ElseIf LstOrderby.Items(i).ToString = "RECDATE" Then
                    filter += IIf(LstOrderby.Items(i).ToString = "RECDATE", "RECDATE1", LstOrderby.Items(i).ToString)
                Else
                    filter += LstOrderby.Items(i).ToString
                End If
                If i < LstOrderby.Items.Count - 1 Then filter += ","
            Next

            If filter <> "" Then
                ObjGrouper.pColumns_Sort = filter
            End If

            RowFillState = True
            ObjGrouper.GroupDgv()
            RowFillState = False
            'If cmbGroup.Text = "COUNTER" Then
            '    strSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW) > 0"
            '    strSql += vbCrLf + " BEGIN"
            '    strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW(PARTICULAR,ITEMCTRNAME,RESULT,COLHEAD)"
            '    strSql += vbCrLf + " SELECT DISTINCT ITEMCTRNAME,ITEMCTRNAME,0 RESULT,'T' FROM TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW WHERE RESULT = 1"
            '    strSql += vbCrLf + " "
            '    strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW(PARTICULAR,ITEMCTRNAME"
            '    strSql += vbCrLf + " ,PCS,GRSWT,LESSWT,NETWT,STNPCS,STNWT,DIAPCS,DIAWT,[WAST][MC],,SALVALUE,RESULT,COLHEAD)"
            '    strSql += vbCrLf + " SELECT DISTINCT ITEMCTRNAME,ITEMCTRNAME,SUM(PCS),SUM(GRSWT),SUM(LESSWT),SUM(NETWT),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM([WAST]),SUM([MC]),SUM(SALVALUE)"
            '    strSql += vbCrLf + " ,2 RESULT,'S' COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW WHERE RESULT = 1 GROUP BY ITEMCTRNAME"
            '    strSql += vbCrLf + " END"
            '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            '    cmd.ExecuteNonQuery()
            'End If

            'strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW)>0 "
            'strSql += " BEGIN "
            'strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW(PARTICULAR,ITEMCTRNAME"
            'strSql += vbCrLf + " ,PCS,GRSWT,LESSWT,NETWT,STNPCS,STNWT,DIAPCS,DIAWT,[WAST],MC,SALVALUE,RESULT,COLHEAD)"
            'strSql += vbCrLf + " SELECT DISTINCT 'GRAND TOTAL','ZZZZ'ITEMCTRNAME,SUM(PCS),SUM(GRSWT),SUM(LESSWT),SUM(NETWT),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM([WAST]),SUM(MC),SUM(SALVALUE)"
            'strSql += vbCrLf + " ,3 RESULT,'G' COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW WHERE RESULT = 1 "

            'strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW(PARTICULAR,ITEMCTRNAME"
            'strSql += vbCrLf + " ,PCS,RESULT,COLHEAD)"
            'strSql += vbCrLf + " SELECT DISTINCT 'NO OF TAG`S','ZZZZ'ITEMCTRNAME,COUNT(*)"
            'strSql += vbCrLf + " ,4 RESULT,'G' COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW WHERE RESULT = 1 AND ISNULL(TAGNO,'') <> ''"
            'strSql += " END "
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()


            'strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW)>0 "
            'strSql += " BEGIN "
            'strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW SET STNPCS = NULL WHERE STNPCS = 0 "
            'strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW SET STNWT = NULL WHERE STNWT = 0 "
            'strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW SET DIAPCS = NULL WHERE DIAPCS = 0 "
            'strSql += " UPDATE TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW SET DIAWT = NULL WHERE DIAWT = 0 "
            'strSql += " END "
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()


            'strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW "
            'If cmbGroup.Text = "NONE" Then
            '    strSql += " order by RESULT,TAGVAL"
            'ElseIf cmbGroup.Text = "COUNTER" Then
            '    strSql += " ORDER BY ITEMCTRNAME,RESULT,TAGVAL"
            'End If

            ''strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "TAGRECEIPTVIEW ORDER BY TAGVAL"
            'da = New OleDbDataAdapter(strSql, cn)
            'dt = New DataTable("RECEIPTVIEW")
            'dt.Columns.Add("KEYNO", GetType(Integer))
            'dt.Columns("KEYNO").AutoIncrement = True
            'dt.Columns("KEYNO").AutoIncrementSeed = 0
            'dt.Columns("KEYNO").AutoIncrementStep = 1
            'da.Fill(dt)
            'If Not dt.Rows.Count > 0 Then
            '    MsgBox("There is No Result", MsgBoxStyle.Exclamation)
            '    btnNew_Click(Me, New EventArgs)
            '    Exit Sub
            'End If
            ''Dim ro As DataRow = dt.NewRow
            ''ro!PARTICULAR = "GRAND TOTAL"
            ''ro!PCS = dt.Compute("SUM(PCS)", Nothing)
            ''ro!GRSWT = dt.Compute("SUM(GRSWT)", Nothing)
            ''ro!LESSWT = dt.Compute("SUM(LESSWT)", Nothing)
            ''ro!NETWT = dt.Compute("SUM(NETWT)", Nothing)
            ''ro!SALVALUE = dt.Compute("SUM(SALVALUE)", Nothing)
            ''ro!STNPCS = dt.Compute("SUM(STNPCS)", Nothing)
            ''ro!STNWT = dt.Compute("SUM(STNWT)", Nothing)
            ''ro!DIAPCS = dt.Compute("SUM(DIAPCS)", Nothing)
            ''ro!DIAWT = dt.Compute("SUM(DIAWT)", Nothing)
            ''ro!COLHEAD = "G"
            ''dt.Rows.Add(ro)

            ''ro = dt.NewRow
            ''ro!PARTICULAR = "NO OF TAG'S"
            ''ro!PCS = dt.Compute("COUNT(PCS)", "TAGNO <> ''")
            ''ro!COLHEAD = "G"
            ''dt.Rows.Add(ro)

            'dt.AcceptChanges()
            'gridTotalView.DataSource = dt
            'tabView.Show()
            'FillGridGroupStyle_KeyNoWise(gridTotalView)
            ''GridViewFormat()
            'gridTotalView.Rows(gridTotalView.RowCount - 1).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
            ''StoneDetails
            funcFillStoneDetails()
            ''GrandTotalDetails
            funcFillGrandDetails()

            With gridTotalView
                For CNT As Integer = 0 To .Rows.Count - 1
                    If .Rows(CNT).Cells("STATUS").Value.ToString = "LIVE" Then
                        .Rows(CNT).Cells("STATUS").Style.ForeColor = Color.Blue
                        .Rows(CNT).Cells("STATUS").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Else
                        .Rows(CNT).Cells("STATUS").Style.ForeColor = Color.Red
                        .Rows(CNT).Cells("STATUS").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If

                Next
                For cnt As Integer = 0 To gridTotalView.ColumnCount - 1
                    gridTotalView.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("COLHEAD").Visible = False
                .Columns("ITEMID").Visible = False
                .Columns("TAGVAL").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("KEYNO").Visible = False
                .Columns("ITEM").Visible = False
                '.Columns("LOTNO").Visible = False
                .Columns("SUBITEM").Visible = False
                '02
                '.Columns("DESIGNER").Visible = True
                If RPT_RECEIPTVIEW_DESIGN = True Then
                    .Columns("DESIGNER").Visible = False
                End If
                .Columns("RECDATE1").Visible = False
                If .Columns.Contains("APPROVAL") Then .Columns("APPROVAL").Visible = False
                '02
                With .Columns("SLNO")
                    .HeaderText = "SNO"
                    .Width = 50
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("SNO")
                    .HeaderText = "SNO"
                    .Width = 70
                    .Visible = False
                End With
                With .Columns("PARTICULAR")
                    .HeaderText = "PARTICULAR"
                    .Width = 150
                    .Visible = True
                End With
                With .Columns("RECDATE")
                    .HeaderText = "RECDATE"
                    .DefaultCellStyle.Format = "dd/MM/yyyy"
                    .Width = 80
                End With
                With .Columns("ACTUALRECDATE")
                    .HeaderText = "ACTUAL RECDATE"
                    .DefaultCellStyle.Format = "dd/MM/yyyy"
                    .Width = 80
                End With
                With .Columns("TAGNO")
                    .HeaderText = "TAG NO"
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("LOTNO")
                    .HeaderText = "LOT NO"
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("PCS")
                    .HeaderText = "PIECES"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("GRSWT")
                    .HeaderText = "GRSWT"
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.000"
                End With
                With .Columns("LESSWT")
                    .HeaderText = "LESSWT"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.000"
                End With
                With .Columns("NETWT")
                    .HeaderText = "NETWT"
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.000"
                End With
                With .Columns("EXTRAWT")
                    .HeaderText = "EXTRAWT"
                    .Width = 100
                    .Visible = chkExtraWt.Checked
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.000"
                End With
                With .Columns("TOUCH")
                    .HeaderText = "TOUCH"
                    .Width = 60
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.00"
                End With
                With .Columns("GRSNET")
                    .HeaderText = "GRS NET"
                    .Width = 70
                End With
                With .Columns("RATE")
                    .HeaderText = "RATE"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.00"
                End With
                With .Columns("WAST%")
                    .HeaderText = "WAST%"
                    .Width = 55
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.00"
                End With
                With .Columns("WAST")
                    .HeaderText = "WAST"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.000"
                End With
                With .Columns("MCGRM")
                    .HeaderText = "MCGRM"
                    .Width = 60
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.00"
                End With
                With .Columns("MC")
                    .HeaderText = "MC"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.00"
                End With

                With .Columns("SALVALUE")
                    .HeaderText = "SALE VALUE"
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.00"
                End With

                With .Columns("PURVALUE")
                    .HeaderText = "PUR VALUE"
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.00"
                End With

                With .Columns("STNPCS")
                    .HeaderText = "STONE PCS"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("STNWT")
                    .HeaderText = "STN_WT (GRM)"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.000"
                End With
                With .Columns("STNWTC")
                    .HeaderText = "STN_WT (CRT)"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.000"
                End With
                With .Columns("STNAMT")
                    .HeaderText = "STN AMT"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.00"
                End With
                With .Columns("DIAPCS")
                    .HeaderText = "DIA PCS"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                'With .Columns("DIAWTC")
                '    .HeaderText = "DIA_WT (CARAT)"
                '    .Width = 70
                '    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                '    .DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                'End With
                With .Columns("DIAWT")
                    .HeaderText = "DIA_WT (CARAT)"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                End With
                With .Columns("DIAAMT")
                    .HeaderText = "DIA AMT"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.00"
                End With
                With .Columns("PREPCS")
                    .HeaderText = "PRE PCS"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("PREWT")
                    .HeaderText = "PRE_WT "
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                End With
                With .Columns("PREAMT")
                    .HeaderText = "PRE AMT"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.00"
                End With
                With .Columns("TAGTYPE")
                    .HeaderText = "TAG TYPE"
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("COUNTER")
                    .HeaderText = "COUNTER"
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("TABLECODE")
                    .HeaderText = "TABLE CODE"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("PURITY")
                    .HeaderText = "PURITY"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.00"
                End With
                With .Columns("NARRATION")
                    .HeaderText = "NARRATION"
                    .Width = 100
                    '.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("BRANDID")
                    .HeaderText = "BRAND ID"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("SALEMODE")
                    .HeaderText = "SALEMODE"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("PCTFILE")
                    .HeaderText = "FILENAME"
                    .Width = 0
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Visible = False
                End With
                With .Columns("TRANINVNO")
                    .HeaderText = "TRAN INVNO"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("SUPBILLNO")
                    .HeaderText = "SUP BILLNO"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("ENTRYMODE")
                    .HeaderText = "ENTMODE"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                With .Columns("USERNAME")
                    .Width = 120
                End With
                With .Columns("ISSUE_WEIGHT")
                    .HeaderText = "ISS WEIGHT"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.000"
                End With
                With .Columns("ISSDATE")
                    .HeaderText = "ISSDATE"
                    .DefaultCellStyle.Format = "dd/MM/yyyy"
                    .Width = 80
                End With
                If .Columns.Contains("ORDREPNO") Then
                    With .Columns("ORDREPNO")
                        .HeaderText = "ORDREPNO"
                        .Width = 150
                        .Visible = True
                        .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    End With
                End If
            End With
            tabMain.SelectedTab = tabView
            lblReportTitle.Text = "TAGED ITEM STOCK RECEIPT "
            If txtItemName.Text <> "" Then lblReportTitle.Text += " Of " & txtItemName.Text
            lblReportTitle.Text += " FROM " + dtpFrom.Value.ToString("dd-MM-yyyy") + " TO " + dtpTo.Value.ToString("dd-MM-yyyy")
            lblReportTitle.Text += IIf(cmbCostCenter.Text <> "" And cmbCostCenter.Text <> "ALL", " :" & cmbCostCenter.Text, "")
            lblReportTitle.Text += vbCrLf + IIf(cmbCounterName.Text <> "" And cmbCounterName.Text <> "ALL", " Counter : " & cmbCounterName.Text, "")
            pnlTotalGridView.BringToFront()
            gridTotalView.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            btnView_Search.Enabled = True
        End Try
        Prop_Sets()
    End Sub


    ''Private Sub gridTotalView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridTotalView.CellFormatting
    ''    With gridTotalView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "G"
    ''                .Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridTotalView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub gridTotalView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridTotalView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridTotalView.RowCount > 0 Then gridTotalView.CurrentCell = gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells(gridTotalView.FirstDisplayedCell.ColumnIndex)
        End If
    End Sub

    Private Sub gridTotalView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridTotalView.RowEnter
        If RowFillState Then Exit Sub
        funcAddNewRow("Wastage %", Nothing, "Min Wastage %", Nothing, "Weight Unit", Nothing, "CostCentre", Nothing)
        funcAddNewRow("Wastage ", Nothing, "Min Wastage ", Nothing, "Fine Rate", Nothing, "Size Name", Nothing)
        funcAddNewRow("Mc/Grm ", Nothing, "Min Mc/Grm ", Nothing, "OrdRepNo", Nothing, "Designer", Nothing)
        funcAddNewRow("MakingCharg ", Nothing, "Min MakingCharg ", Nothing, "LotNo", Nothing, "New LotNo", Nothing)
        If gridTotalView.Rows(e.RowIndex).Cells("SNO").Value.ToString = "" Then Exit Sub
        funcGetCursorTot(e.RowIndex)
        strSql = " Select"
        strSql += " CASE WHEN MAXWASTPER <> 0 THEN MAXWASTPER ELSE NULL END MaxWastPer"
        strSql += " ,CASE WHEN MAXWAST <> 0 THEN MAXWAST ELSE NULL END MaxWast"
        strSql += " ,CASE WHEN MAXMCGRM <> 0 THEN MAXMCGRM ELSE NULL END MaxMcGrm"
        strSql += " ,CASE WHEN MAXMC <> 0 THEN MAXMC ELSE NULL END AS MAXMC"
        strSql += " ,CASE WHEN MINWASTPER <> 0 THEN MINWASTPER ELSE NULL END MINWastPer"
        strSql += " ,CASE WHEN MINWAST <> 0 THEN MINWAST ELSE NULL END MINWast"
        strSql += " ,CASE WHEN MINMCGRM <> 0 THEN MINMCGRM ELSE NULL END MINMcGrm"
        strSql += " ,CASE WHEN MINMC <> 0 THEN MINMC ELSE NULL END AS MINMC"
        strSql += " ,WEIGHTUNIT,CASE WHEN FINERATE <> 0 THEN FINERATE ELSE NULL END FINERATE,ORDREPNO,"
        strSql += " '" & gridTotalView.Item("COSTCENTRE", e.RowIndex).Value & "' COSTCENTRE,"
        strSql += " (SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)SIZENAME,"
        strSql += " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)DESIGNERNAME,"
        strSql += " (SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = T.LOTSNO)AS LOTNO"
        strSql += " , (SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO IN (SELECT NEWSNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO= T.LOTSNO))AS NEWLOTNO "
        strSql += " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += " WHERE SNO = '" & gridTotalView.Item("SNO", e.RowIndex).Value & "'"
        Dim dt As New DataTable("OtherDetails")
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        tempDt.Clear()
        If Not dt.Rows.Count > 0 Then
            gridFullDetails.DataSource = tempDt
            Exit Sub
        End If
        With dt.Rows(0)
            funcAddNewRow("Wastage %", .Item("MaxWastPer").ToString,
            "Min Wastage %", .Item("MinWastPer").ToString,
            "Weight Unit", .Item("WeightUnit").ToString,
            "CostCentre", .Item("CostCentre").ToString)
            funcAddNewRow("Wastage ", .Item("MaxWast").ToString, "Min Wastage ", .Item("MinWast").ToString, "Fine Rate", .Item("FineRate").ToString, "Size Name", .Item("SizeName").ToString)
            funcAddNewRow("Mc/Grm ", .Item("MaxMcGrm").ToString, "Min Mc/Grm ", .Item("MinMcGrm").ToString, "OrdRepNo", .Item("OrdRepNo").ToString, "Designer", .Item("DesignerName").ToString)
            funcAddNewRow("MakingCharg ", .Item("MaxMc").ToString, "Min MakingCharg ", .Item("MinMc").ToString, "LotNo", .Item("LotNo").ToString, "New LotNo", .Item("NewLotNo").ToString)
        End With
        gridFullDetails.DataSource = tempDt
        With gridFullDetails.Columns("MaxDescription")
            .Width = 90
        End With
        With gridFullDetails.Columns("MaxValues")
            .Width = 80
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
        With gridFullDetails.Columns("MinDescription")
            .Width = 80
            .Visible = False
        End With
        With gridFullDetails.Columns("MinValues")
            .Width = 80
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Visible = False
        End With
        With gridFullDetails.Columns("OtherDesc1")
            .Width = 80
        End With
        With gridFullDetails.Columns("OtherVal1")
            .Width = 80
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
        With gridFullDetails.Columns("OtherDesc2")
            .Width = 80
        End With
        With gridFullDetails.Columns("OtherVal2")
            .Width = 380
        End With
        ''PIC
        Dim serverPath As String = Nothing
        Dim fileDestPath As String = gridTotalView.Rows(e.RowIndex).Cells("PCTFILE").Value.ToString
        If IO.File.Exists(fileDestPath) Then
            AutoImageSizer(fileDestPath, picItem)
        Else
            picItem.Image = My.Resources.no_photo
        End If
    End Sub

    Private Sub gridFullDetails_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridFullDetails.CellFormatting
        If e.ColumnIndex = 0 Or e.ColumnIndex = 2 Or e.ColumnIndex = 4 Or e.ColumnIndex = 6 Then
            e.CellStyle.BackColor = headerBgColor
            e.CellStyle.SelectionBackColor = headerBgColor
        End If
        If e.ColumnIndex = 1 Or e.ColumnIndex = 3 Or e.ColumnIndex = 5 Or e.ColumnIndex = 7 Then
            e.CellStyle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        End If
    End Sub

    Private Sub gridGrandTotal_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridGrandTotal.CellFormatting
        If e.ColumnIndex = 0 Or e.RowIndex = 0 Then
            e.CellStyle.BackColor = headerBgColor
            e.CellStyle.SelectionBackColor = headerBgColor
        End If
        If e.RowIndex <> 0 And e.ColumnIndex <> 0 Then
            e.CellStyle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        End If
    End Sub

    Private Sub gridStoneDetails_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridFullStoneDetails.CellFormatting
        If e.ColumnIndex = 0 Or e.RowIndex = 0 Then
            e.CellStyle.BackColor = headerBgColor
            e.CellStyle.SelectionBackColor = headerBgColor
        End If
        If e.RowIndex <> 0 And e.ColumnIndex <> 0 Then
            e.CellStyle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        End If
    End Sub

    Private Sub txtItemCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemCode_NUM.KeyDown
        Dim itemId As String
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT DISTINCT"
            strSql += vbCrLf + " ITEMID, "
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMMAST AS T"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
                strSql += vbCrLf + " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
            End If
            itemId = BrighttechPack.SearchDialog.Show("Find ItemId", strSql, cn)
            strSql = " Select itemName from " & cnAdminDb & "..itemMast where itemId = '" & itemId & "'"
            Dim dt As New DataTable
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtItemCode_NUM.Text = itemId
                txtItemName.Text = dt.Rows(0).Item("itemName")
            End If
        End If
    End Sub

    Private Sub txtItemCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemCode_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'strSql = " Select itemName from " & cnAdminDb & "..itemMast where itemId = '" & txtItemCode_NUM.Text & "'"
            'If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            '    strSql += vbCrLf + " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
            'End If
            'Dim dt As New DataTable
            'dt.Clear()
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dt)
            'If dt.Rows.Count > 0 Then
            '    txtItemName.Text = dt.Rows(0).Item("itemName")
            'Else
            '    txtItemName.Clear()
            '    '    MsgBox("Invalid ItemId", MsgBoxStyle.Exclamation)
            '    '    txtItemCode.Focus()
            'End If


            strSql = " SELECT ITEMNAME,STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & txtItemCode_NUM.Text & "'"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
                strSql += vbCrLf + " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
            End If
            Dim DT As New DataTable
            DT.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(DT)
            If DT.Rows.Count > 0 Then
                txtItemName.Text = DT.Rows(0).Item("ITEMNAME")
                If DT.Rows(0).Item("STUDDEDSTONE").ToString = "Y" Then
                    Studded = True
                Else
                    Studded = False
                End If
            Else
                txtItemName.Clear()
            End If

            chkCmbStoneName.Enabled = Studded
            chkCmbStSubItemName.Enabled = Studded
            chkCmbStoneName.Text = IIf(Studded = True, chkCmbStoneName.Text, "")
            chkCmbStSubItemName.Text = IIf(Studded = True, chkCmbStSubItemName.Text, "")
        End If
    End Sub
    Private Sub cmbSubItemGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSubItemGroup.SelectedIndexChanged

    End Sub
    Private Sub cmbSubItemGroup_TextChanged(sender As Object, e As EventArgs) Handles cmbSubItemGroup.TextChanged
        If cmbSubItemGroup.Text <> "ALL" And cmbSubItemGroup.Text <> "" Then
            cmbSubItemName.Items.Clear()
            cmbSubItemName.Items.Add("ALL")
            strSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast where itemId = '" & txtItemCode_NUM.Text & "'"
            If cmbSubItemGroup.Text <> "ALL" And cmbSubItemGroup.Text <> "" Then
                strSql += " and sgroupid in (select sgroupid from " & cnAdminDb & "..SubItemgroup where sgroupname in (" & GetQryString(cmbSubItemGroup.Text, ",") & "))"
            End If
            Dim dtSItem As DataTable
            dtSItem = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSItem)
            BrighttechPack.GlobalMethods.FillCombo(cmbSubItemName, dtSItem, "SUBITEMNAME", False)
            'objGPack.FillCombo(strSql, cmbSubItemName, False)
            cmbSubItemName.Text = "ALL"
            If cmbSubItemName.Items.Count > 0 Then
                cmbSubItemName.Enabled = True
            Else
                cmbSubItemName.Enabled = False
            End If
        End If
    End Sub
    Private Sub txtItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemName.TextChanged
        If txtItemName.Text <> "" Then
            cmbSubItemName.Items.Clear()
            cmbSubItemName.Items.Add("ALL")
            strSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast where itemId = '" & txtItemCode_NUM.Text & "'"
            Dim dtSItem As DataTable
            dtSItem = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSItem)
            BrighttechPack.GlobalMethods.FillCombo(cmbSubItemName, dtSItem, "SUBITEMNAME", False)
            'objGPack.FillCombo(strSql, cmbSubItemName, False)
            cmbSubItemName.Text = "ALL"
            If cmbSubItemName.Items.Count > 0 Then
                cmbSubItemName.Enabled = True
            Else
                cmbSubItemName.Enabled = False
            End If
        Else
            cmbSubItemName.Items.Clear()
            cmbSubItemName.Enabled = False
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub gridTotalView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridTotalView.KeyPress
        If e.KeyChar = Chr(Keys.D) Or AscW(e.KeyChar) = 100 Then
            If gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("SNO").Value.ToString = "" Then Exit Sub
            Dim qry As String = Nothing
            qry = " WHERE TAGNO IN (SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG AS T "
            qry += funcFiltrationString()
            qry += " )"
            Dim objStudDetails As New frmTagStuddedStoneDetails(gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("SNO").Value.ToString, qry, lblReportTitle.Text)
            objStudDetails.ShowDialog()
        ElseIf e.KeyChar = Chr(Keys.X) Or AscW(e.KeyChar) = 120 Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.P) Or UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.F) Or UCase(e.KeyChar) = "F" Then
            funcSummaryPrint()
        ElseIf e.KeyChar = Chr(Keys.S) Or AscW(e.KeyChar) = 115 Then
            If gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("SNO").Value.ToString = "" Then Exit Sub
            Dim objStnDetails As New frmTagStoneDetails(gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("SNO").Value.ToString, lblReportTitle.Text)
        ElseIf e.KeyChar = Chr(Keys.M) Or AscW(e.KeyChar) = 109 Then
            Dim qry As String = Nothing
            qry = " WHERE TAGNO IN (SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG AS T "
            qry += funcFiltrationString()
            qry += " )"
            Dim objStudDetails As New frmTagStuddedStoneDetails(gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("SNO").Value.ToString, qry, lblReportTitle.Text, "", "M")
            objStudDetails.ShowDialog()
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            If gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("SNO").Value.ToString = "" Then Exit Sub
            Dim objTagViewer As New frmTagImageViewer(
            gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("TAGNO").Value.ToString,
            gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("ITEMID").Value.ToString)
            objTagViewer.ShowDialog()
        End If
    End Sub
    Function funcSummaryPrint()
        Dim dtview As New DataTable
        Dim dv As New DataView
        Dim rofilter As String = ""
        Dim grs As Double = 0
        Dim net As Double = 0
        Dim salvalue As Double = 0
        Dim pcs As Integer = 0
        Dim prepcs As Integer = 0
        Dim prewt As Double = 0
        Dim diapcs As Integer = 0
        Dim diawt As Double = 0
        Dim stnpcs As Integer = 0
        Dim stnwt As Double = 0
        Dim totpcs As Integer = 0
        Dim totwt As Double = 0
        Dim totrate As Double = 0
        Dim TagNos As String
        Dim Itemids As String
        Dim LotNos As String
        Dim Itemid As Integer
        If ChkLstGroupBy.CheckedItems.Contains("DESIGNER") Then rofilter = "DESIGNER='" & gridTotalView.CurrentRow.Cells("DESIGNER").Value.ToString & "'"
        If (ChkLstGroupBy.CheckedItems.Contains("DESIGNER") And ChkLstGroupBy.CheckedItems.Contains("ITEM")) Then rofilter += " AND "
        If (ChkLstGroupBy.CheckedItems.Contains("ITEM") Or ChkLstGroupBy.CheckedItems.Contains("SUBITEM")) Then rofilter += " ITEM='" & gridTotalView.CurrentRow.Cells("ITEM").Value.ToString & "'"
        If (ChkLstGroupBy.CheckedItems.Contains("DESIGNER") And ChkLstGroupBy.CheckedItems.Contains("COUNTER") Or ChkLstGroupBy.CheckedItems.Contains("ITEM") And ChkLstGroupBy.CheckedItems.Contains("COUNTER")) Then rofilter += " AND "
        If ChkLstGroupBy.CheckedItems.Contains("COUNTER") Then rofilter += "COUNTER='" & gridTotalView.CurrentRow.Cells("COUNTER").Value.ToString & "'"
        dv = dtSource.DefaultView
        dv.RowFilter = (rofilter)
        dtview = dv.ToTable

        If dtview.Rows.Count <= 0 Then
            MsgBox("Please Select Item")
            Exit Function
        End If

        If IO.File.Exists(Application.StartupPath & "\SUMMARYPRINT.TXT") Then
            IO.File.Delete(Application.StartupPath & "\SUMMARYPRINT.TXT")
        End If
        Dim Loosestone As Boolean = False
        For i As Integer = 0 To dtview.Rows.Count - 1
            grs = grs + Val(dtview.Rows(i).Item("GRSWT").ToString)
            net = net + Val(dtview.Rows(i).Item("NETWT").ToString)
            salvalue = salvalue + Val(dtview.Rows(i).Item("SALVALUE").ToString)
            pcs = pcs + Val(dtview.Rows(i).Item("PCS").ToString)
            diapcs = diapcs + Val(dtview.Rows(i).Item("DIAPCS").ToString)
            diawt = diawt + Val(dtview.Rows(i).Item("DIAWT").ToString)
            prepcs = prepcs + Val(dtview.Rows(i).Item("PREPCS").ToString)
            prewt = prewt + Val(dtview.Rows(i).Item("PREWT").ToString)
            stnpcs = stnpcs + Val(dtview.Rows(i).Item("STNPCS").ToString)
            stnwt = stnwt + Val(dtview.Rows(i).Item("STNWT").ToString)
            TagNos += "'" & dtview.Rows(i).Item("TAGNO").ToString & "',"
            Itemid = Val(dtview.Rows(i).Item("ITEMID").ToString)
        Next
        Dim dtLotNo As DataTable = dtview.DefaultView.ToTable(True, "LOTNO")
        For j As Integer = 0 To dtLotNo.Rows.Count - 1
            LotNos += dtLotNo.Rows(j).Item("LOTNO").ToString & ","
        Next
        If LotNos <> "" Then LotNos = Mid(LotNos, 1, Len(LotNos) - 1)
        strSql = "SELECT STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=" & Itemid
        If objGPack.GetSqlValue(strSql, "STUDDED", "") = "L" Then
            Loosestone = True
        End If
        Dim write As StreamWriter
        Dim dttemp As New DataTable
        Dim dt As New DataTable

        If TagNos <> "" Then TagNos = Mid(TagNos, 1, Len(TagNos) - 1)

        write = IO.File.CreateText(Application.StartupPath & "\SUMMARYPRINT.TXT")
        write.WriteLine("--------------------------------------")
        write.WriteLine("PLUS VIEW SUMMARY")
        write.WriteLine("--------------------------------------")
        write.WriteLine(("DATE :").PadRight(7, "") & Space(2) & dtpFrom.Value.ToString("dd-MM-yyyy").PadRight(10, "") & Space(1) & ":" & dtpTo.Value.ToString("dd-MM-yyyy").PadRight(10, ""))
        write.WriteLine(("LOTNO :").PadRight(7, "") & Space(2) & LotNos)
        write.WriteLine(("PRODUCT:").PadRight(7, "") & Space(2) & dtview.Rows(0).Item("ITEM").ToString.PadRight(7, ""))
        write.WriteLine(("DESIGNER:").PadRight(7, "") & Space(2) & dtview.Rows(0).Item("DESIGNER").ToString.PadRight(7, ""))
        write.WriteLine(("PIECES :").PadRight(7, "") & Space(2) & pcs.ToString.PadRight(7, ""))
        write.WriteLine(("GROSSWT:").PadRight(7, "") & Space(2) & grs.ToString.PadRight(7, ""))
        write.WriteLine(("NETWT :").PadRight(7, "") & Space(2) & net.ToString.PadRight(7, ""))
        If prepcs Then write.WriteLine(("PRE.PCS :").PadRight(7, "") & Space(2) & prepcs.ToString.PadRight(7, ""))
        If prewt Then write.WriteLine(("PRE.WT :").PadRight(7, "") & Space(2) & prewt.ToString.PadRight(7, ""))
        If diapcs Then write.WriteLine("DIA.PCS :" & Space(2) & diapcs.ToString.PadRight(7, ""))
        If diawt Then write.WriteLine("DIA.WT :" & Space(2) & diawt.ToString.PadRight(7, ""))
        If stnpcs Then write.WriteLine("STN.PCS :" & Space(2) & stnpcs.ToString.PadRight(7, ""))
        If stnwt Then write.WriteLine("STN.WT :" & Space(2) & stnwt.ToString.PadRight(7, ""))
        write.WriteLine("SAL.VALUE :" & Space(2) & salvalue.ToString.PadRight(15, ""))
        write.WriteLine()
        If MessageBox.Show("Do You Want Studded Detail.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            'strSql = " SELECT (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID)STNTYPE"
            'strSql += vbCrLf + " ,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT,SUM(STNRATE)STNRATE"
            'strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T"
            'strSql += vbCrLf + " WHERE TAGNO IN (SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG AS T  "
            'strSql += vbCrLf + " WHERE ACTUALRECDATE Between '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' and '" & dtpTo.Value.ToString("yyyy/MM/dd") & "'  "
            'strSql += vbCrLf + " AND T.ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & gridTotalView.CurrentRow.Cells("ITEM").Value.ToString & "')"
            'strSql += vbCrLf + " AND T.COMPANYID IN('" & cnCompanyId & "'))"
            'strSql += vbCrLf + " GROUP BY STNRATE,T.STNITEMID"

            If Loosestone Then
                strSql = "   SELECT CASE WHEN ISNULL(SS.SUBITEMNAME,'')='' THEN I.ITEMNAME ELSE SS.SUBITEMNAME END AS ITEM"
                strSql += vbCrLf + "  ,'P' STNTYPE"
                strSql += vbCrLf + "  ,SUM(PCS)STNPCS,"
                strSql += vbCrLf + "   CONVERT(NUMERIC(15,3),SUM(T.GRSWT))STNWT,"
                strSql += vbCrLf + "   CONVERT(NUMERIC(15,0),SUM(SALVALUE))STNAMT,"
                strSql += vbCrLf + "   CONVERT(NUMERIC(15,0),T.RATE)AS STNRATE"
                strSql += vbCrLf + "   FROM " & cnAdminDb & "..ITEMTAG AS T "
                strSql += vbCrLf + "   INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.ITEMID"
                strSql += vbCrLf + "   LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SS ON SS.ITEMID = T.ITEMID AND SS.SUBITEMID=T.SUBITEMID"
                strSql += vbCrLf + "   WHERE 1=1"
                If TagNos <> "" Then strSql += vbCrLf + "   AND TAGNO IN (" & TagNos & ")"
                If Itemids <> "" Then strSql += vbCrLf + "   AND ITEMID IN (" & Itemids & ")"
                strSql += vbCrLf + "   GROUP BY T.RATE,T.ITEMID,SS.SUBITEMNAME ,I.ITEMNAME"
            Else
                strSql = "   SELECT CASE WHEN ISNULL(SS.SUBITEMNAME,'')='' THEN I.ITEMNAME ELSE SS.SUBITEMNAME END AS ITEM"
                strSql += vbCrLf + "  ,I.DIASTONE STNTYPE"
                strSql += vbCrLf + "  ,SUM(STNPCS)STNPCS,"
                strSql += vbCrLf + "   CONVERT(NUMERIC(15,3),SUM(CASE WHEN T.STONEUNIT = 'G' THEN T.STNWT*5 WHEN T.STONEUNIT = 'C' AND I.DIASTONE='S' THEN T.STNWT/5 ELSE (CASE WHEN T.STONEUNIT = 'C' THEN T.STNWT END) END))STNWT,"
                strSql += vbCrLf + "   CONVERT(NUMERIC(15,0),SUM(STNAMT))STNAMT,"
                strSql += vbCrLf + "   CONVERT(NUMERIC(15,0),T.STNRATE)AS STNRATE"
                strSql += vbCrLf + "   FROM " & cnAdminDb & "..ITEMTAGSTONE AS T "
                strSql += vbCrLf + "   INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.STNITEMID"
                strSql += vbCrLf + "   LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SS ON SS.ITEMID = T.STNITEMID AND SS.SUBITEMID=T.STNSUBITEMID"
                strSql += vbCrLf + "   WHERE TAGNO IN"
                strSql += vbCrLf + "   (SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG AS T"
                strSql += vbCrLf + "   INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + "   LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = T.ITEMID AND SM.SUBITEMID = T.SUBITEMID "
                strSql += vbCrLf + "   WHERE ACTUALRECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND '" & dtpTo.Value.ToString("yyyy/MM/dd") & "'  "
                strSql += vbCrLf + "   AND T.ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & gridTotalView.CurrentRow.Cells("ITEM").Value.ToString & "')"
                strSql += vbCrLf + "   )"
                If TagNos <> "" Then strSql += vbCrLf + "   AND TAGNO IN (" & TagNos & ")"
                If Itemids <> "" Then strSql += vbCrLf + "   AND ITEMID IN (" & Itemids & ")"
                strSql += vbCrLf + "   GROUP BY I.DIASTONE,T.STNRATE,T.STNITEMID,SS.SUBITEMNAME ,I.ITEMNAME"
            End If
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                write.WriteLine("--------------------------------------")
                write.WriteLine("STUDDED DETAILS")
                write.WriteLine("--------------------------------------")
                write.WriteLine("Type" & Space(5) & "Pcs" & Space(5) & "Wt" & Space(4) & "PerCt" & Space(4) & "Amt")
                write.WriteLine("--------------------------------------")
                Dim ro() As DataRow
                ro = dt.Select("STNTYPE='S'")
                If ro.Length > 0 Then
                    totpcs = 0
                    totwt = 0
                    totrate = 0
                    For j As Integer = 0 To ro.Length - 1
                        write.WriteLine("S." & Mid(ro(j).Item("ITEM").ToString, 1, 4).PadLeft(4, "") & Space(2) & ro(j).Item("STNPCS").ToString.PadLeft(2, "") & Space(2) & ro(j).Item("STNWT").ToString.PadLeft(7, " ") & Space(2) & ro(j).Item("STNRATE").ToString.PadLeft(6, " ") & Space(2) & ro(j).Item("STNAMT").ToString.PadLeft(7, " "))
                        totpcs = totpcs + Val(ro(j).Item("STNPCS").ToString)
                        totwt = totwt + Val(ro(j).Item("STNWT").ToString)
                        totrate = totrate + Val(ro(j).Item("STNAMT").ToString)
                    Next
                    write.WriteLine("--------------------------------------")
                    write.WriteLine("TOT :" & totpcs.ToString.PadLeft(4, " ") & Space(6) & totwt.ToString.PadLeft(7, " ") & Space(4) & totrate.ToString.PadLeft(7, " "))
                    'write.WriteLine("TOT :" & totpcs.ToString.PadLeft(4, " ") & Space(6) & totwt.ToString.PadLeft(7, " "))
                    write.WriteLine("--------------------------------------")

                End If
                Dim ro1() As DataRow
                ro1 = dt.Select("STNTYPE='P'")
                If ro1.Length > 0 Then
                    totpcs = 0
                    totwt = 0
                    totrate = 0
                    For j As Integer = 0 To ro1.Length - 1
                        write.WriteLine("P." & Mid(ro1(j).Item("ITEM").ToString, 1, 4).PadLeft(4, "") & Space(2) & ro1(j).Item("STNPCS").ToString.PadLeft(2, "") & Space(2) & ro1(j).Item("STNWT").ToString.PadLeft(7, " ") & Space(2) & ro1(j).Item("STNRATE").ToString.PadLeft(6, " ") & Space(2) & ro1(j).Item("STNAMT").ToString.PadLeft(7, " "))
                        totpcs = totpcs + Val(ro1(j).Item("STNPCS").ToString)
                        totwt = totwt + Val(ro1(j).Item("STNWT").ToString)
                        totrate = totrate + Val(ro1(j).Item("STNAMT").ToString)
                    Next
                    write.WriteLine("--------------------------------------")
                    write.WriteLine("TOT :" & totpcs.ToString.PadLeft(4, " ") & Space(6) & totwt.ToString.PadLeft(7, " ") & Space(4) & totrate.ToString.PadLeft(7, " "))
                    'write.WriteLine("TOT :" & totpcs.ToString.PadLeft(4, " ") & Space(6) & totwt.ToString.PadLeft(7, " "))
                    write.WriteLine("--------------------------------------")

                End If
                Dim ro2() As DataRow
                ro2 = dt.Select("STNTYPE='D'")
                If ro2.Length > 0 Then
                    totpcs = 0
                    totwt = 0
                    totrate = 0
                    For j As Integer = 0 To ro2.Length - 1
                        write.WriteLine("D." & Mid(ro2(j).Item("ITEM").ToString, 1, 4).PadLeft(4, "") & Space(2) & ro2(j).Item("STNPCS").ToString.PadLeft(2, "") & Space(2) & ro2(j).Item("STNWT").ToString.PadLeft(7, " ") & Space(2) & ro2(j).Item("STNRATE").ToString.PadLeft(6, " ") & Space(2) & ro2(j).Item("STNAMT").ToString.PadLeft(7, " "))
                        totpcs = totpcs + Val(ro2(j).Item("STNPCS").ToString)
                        totwt = totwt + Val(ro2(j).Item("STNWT").ToString)
                        totrate = totrate + Val(ro2(j).Item("STNAMT").ToString)
                    Next
                    write.WriteLine("--------------------------------------")
                    write.WriteLine("TOT :" & totpcs.ToString & Space(6) & totwt.ToString.PadLeft(7, " ").PadLeft(4, " ") & Space(4) & totrate.ToString.PadLeft(7, " "))
                    'write.WriteLine("TOT :" & totpcs.ToString & Space(6) & totwt.ToString.PadLeft(7, " ").PadLeft(4, " "))
                    write.WriteLine("--------------------------------------")
                End If

            End If
        End If

        write.Close()
        If IO.File.Exists(Application.StartupPath & "\SUMMARYPRINT.BAT") Then
            IO.File.Delete(Application.StartupPath & "\SUMMARYPRINT.BAT")
        End If
        Dim writebat As StreamWriter

        Dim PrnName As String = ""
        Dim CondId As String = ""
        Try
            CondId = "'AGOLDREPORT40COLUMN" & Environ("NODE-ID").ToString & "'"
        Catch ex As Exception
            MsgBox("Set Node-Id", MsgBoxStyle.Information)
            Exit Function
        End Try
        writebat = IO.File.CreateText(Application.StartupPath & "\SUMMARYPRINT.BAT")
        strSql = "SELECT * FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID=" & CondId & ""
        dt = New DataTable
        dt = GetSqlTable(strSql, cn)
        If dt.Rows.Count <> 0 Then
            PrnName = dt.Rows(0).Item("CTLTEXT").ToString
        Else
            PrnName = "PRN"
        End If
        writebat.WriteLine("TYPE " & Application.StartupPath & "\SUMMARYPRINT.TXT>" & PrnName)
        writebat.Flush()
        writebat.Close()
        Shell(Application.StartupPath & "\SUMMARYPRINT.BAT")
    End Function

    Function funcAddStoneDetails(ByVal Desc As String, ByVal dDetails As String, ByVal sDetails As String, ByVal pDetails As String) As Integer
        Dim ro As DataRow = Nothing
        ro = dtStoneDetails.NewRow
        ro("Description") = Desc
        ro("Diamond") = dDetails
        ro("Stone") = sDetails
        ro("Precious") = pDetails
        dtStoneDetails.Rows.Add(ro)
    End Function

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        cmbSubItemName.Items.Clear()
        cmbSubItemName.Enabled = False
        cmbGroup.Items.Clear()
        cmbGroup.Items.Add("NONE")
        cmbGroup.Items.Add("COUNTER")
        cmbGroup.Text = "NONE"

        LstOrderby.Items.Clear()
        LstOrderby.Items.Add("TAGNO")

        ChklstboxOrderby.Items.Clear()
        ChklstboxOrderby.Items.Add("TAGNO", True)
        ChklstboxOrderby.Items.Add("RECDATE", False)
        ChklstboxOrderby.Items.Add("GRSWT", False)
        ChklstboxOrderby.Items.Add("SALEVALUE", False)
        ChklstboxOrderby.Items.Add("WAST", False)
        ChklstboxOrderby.Items.Add("TRANINVNO", False)
        ChklstboxOrderby.Items.Add("ITEMNAME", False)

        ''Counter
        cmbCounterName.Items.Clear()
        cmbCounterName.Items.Add("ALL")
        strSql = " Select ItemCtrName from " & cnAdminDb & "..itemCounter order by itemCtrName"
        Dim dtCtr As DataTable
        dtCtr = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCtr)
        BrighttechPack.GlobalMethods.FillCombo(cmbCounterName, dtCtr, "ITEMCTRNAME", False)
        'objGPack.FillCombo(strSql, cmbCounterName, False)
        cmbCounterName.Text = "ALL"

        ''ItemType
        cmbItemType.Items.Clear()
        cmbItemType.Items.Add("ALL")
        strSql = " Select Name from " & cnAdminDb & "..itemType order by Name"
        objGPack.FillCombo(strSql, cmbItemType, False)
        ' cmbItemType.Text = "ALL"

        ''CostCenter
        'cmbCostCenter.Items.Clear()
        'If cmbCostCenter.Enabled = True Then
        '    cmbCostCenter.Items.Add("ALL")
        '    strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
        '    objGPack.FillCombo(strSql, cmbCostCenter, False)
        '    cmbCostCenter.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
        '    If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCenter.Enabled = False
        'End If

        If chkCmbCostCentre.Enabled = True Then
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        End If


        strSql = vbCrLf + " SELECT NAME FROM " & cnAdminDb & "..SYSCOLUMNS WHERE ID IN (SELECT ID FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME='ITEMTAG')"
        strSql += vbCrLf + " ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbSearchKey, True, False)
        cmbSearchKey.Text = ""
        txtSearch_txt.Text = ""
        cmbLotEntryType.Text = "ALL"
        chkMRDetails.Checked = False
        grpMR_Receipt.Enabled = False
        ' rbtAll.Checked = True
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        dtpMRDate.Value = GetServerDate()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")
        strSql = vbCrLf + " SELECT DISTINCT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') IN ('T','D')"
        strSql += vbCrLf + " ORDER BY ITEMNAME"
        Dim dtItem As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        If dtItem.Rows.Count > 0 Then
            chkCmbStoneName.Items.Add("ALL", True)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbStoneName, dtItem, "ITEMNAME", False, "ALL")
        End If
        cmbSubItemGroup.Items.Clear()
        strSql = vbCrLf + " SELECT DISTINCT SGROUPNAME FROM " & cnAdminDb & "..SUBITEMGROUP WHERE ACTIVE='Y'"
        strSql += vbCrLf + " ORDER BY SGROUPNAME"
        Dim dtsubitemgrp As DataTable
        dtsubitemgrp = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtsubitemgrp)
        If dtsubitemgrp.Rows.Count > 0 Then
            cmbSubItemGroup.Items.Add("ALL", True)
            BrighttechPack.GlobalMethods.FillCombo(cmbSubItemGroup, dtsubitemgrp, "SGROUPNAME", False, "ALL")
        End If
        cmbSubItemGroup.Text = "ALL"
        Studded = False
        chkCmbStoneName.Enabled = Studded
        chkCmbStSubItemName.Enabled = Studded
        chkCmbMetal.Select()
        Prop_Gets()
    End Sub
    Private Sub ChklstboxOrderby_ItemCheck(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles ChklstboxOrderby.ItemCheck
        If ChklstboxOrderby.SelectedIndex < 0 Then Exit Sub
        If ChklstboxOrderby.GetItemChecked(ChklstboxOrderby.SelectedIndex) = False Then
            LstOrderby.Items.Add(ChklstboxOrderby.Items(ChklstboxOrderby.SelectedIndex).ToString())
        Else
            LstOrderby.Items.Remove(ChklstboxOrderby.Items(ChklstboxOrderby.SelectedIndex).ToString())
        End If
    End Sub
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        txtItemCode_NUM.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridTotalView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblReportTitle.Text, gridTotalView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridTotalView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblReportTitle.Text, gridTotalView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmTagedItemsStockViewReceiptView_Properties
        obj.p_txtItemCode_NUM = txtItemCode_NUM.Text
        obj.p_txtItemName = txtItemName.Text
        obj.p_cmbSubItemName = cmbSubItemName.Text
        obj.p_txtTagNo = txtTagNo.Text
        GetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDesigner)
        obj.p_cmbCounterName = cmbCounterName.Text
        obj.p_cmbItemType = cmbItemType.Text
        'obj.p_cmbCostCenter = cmbCostCenter.Text
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        obj.p_txtLotNo_NUM = txtLotNoFrom_NUM.Text
        obj.p_txtFromWt_WET = txtFromWt_WET.Text
        obj.p_txtToWt_WET = txtToWt_WET.Text
        obj.p_txtFromDiaWt_WET = txtFromDiaWt_WET.Text
        obj.p_txtToDiaWt_WET = txtToDiaWt_WET.Text
        obj.p_txtFromRate_WET = txtFromRate_WET.Text
        obj.p_txtToRate_WET = txtToRate_WET.Text
        GetChecked_CheckedList(ChkLstGroupBy, obj.p_ChkLstGroupBy)
        obj.p_rbtAll = rbtAll.Checked
        obj.p_rbtRegular = rbtRegular.Checked
        obj.p_rbtOrder = rbtOrder.Checked
        obj.p_chkDetailed = chkDetailed.Checked
        obj.p_chkActualDate = chkActualDate.Checked
        obj.p_chkWithCumulative = chkWithCumulative.Checked
        obj.p_cmbGroup = cmbGroup.Text
        obj.p_chkExtraWt = chkExtraWt.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmTagedItemsStockViewReceiptView_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTagedItemsStockViewReceiptView_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTagedItemsStockViewReceiptView_Properties))
        SetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal, "ALL")
        txtItemCode_NUM.Text = obj.p_txtItemCode_NUM
        txtItemName.Text = obj.p_txtItemName
        cmbSubItemName.Text = obj.p_cmbSubItemName
        txtTagNo.Text = obj.p_txtTagNo
        SetChecked_CheckedList(chkCmbDesigner, obj.p_chkCmbDesigner, "ALL")
        cmbCounterName.Text = obj.p_cmbCounterName
        cmbItemType.Text = obj.p_cmbItemType
        'cmbCostCenter.Text = obj.p_cmbCostCenter
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        txtLotNoFrom_NUM.Text = obj.p_txtLotNo_NUM
        txtFromWt_WET.Text = obj.p_txtFromWt_WET
        txtToWt_WET.Text = obj.p_txtToWt_WET
        txtFromDiaWt_WET.Text = obj.p_txtFromDiaWt_WET
        txtToDiaWt_WET.Text = obj.p_txtToDiaWt_WET
        txtFromRate_WET.Text = obj.p_txtFromRate_WET
        txtToRate_WET.Text = obj.p_txtToRate_WET
        SetChecked_CheckedList(ChkLstGroupBy, obj.p_ChkLstGroupBy, Nothing)
        rbtAll.Checked = obj.p_rbtAll
        rbtRegular.Checked = obj.p_rbtRegular
        rbtOrder.Checked = obj.p_rbtOrder
        chkDetailed.Checked = obj.p_chkDetailed
        chkActualDate.Checked = obj.p_chkActualDate
        chkWithCumulative.Checked = obj.p_chkWithCumulative
        cmbGroup.Text = obj.p_cmbGroup
        chkExtraWt.Checked = obj.p_chkExtraWt
    End Sub
    Private Sub chkMultiSelect_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMultiSelect.CheckStateChanged
        If chkMultiSelect.Checked Then
            chkCmbDesigner.Visible = True
            CmbDesigner.Visible = False
        Else
            chkCmbDesigner.Visible = False
            CmbDesigner.Visible = True
        End If
    End Sub


    Private Sub CmbDesigner_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbDesigner.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub ChkOrgCostCentre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkOrgCostCentre.CheckedChanged

    End Sub

    Private Sub chkTagNoMulti_CheckedChanged(sender As Object, e As EventArgs) Handles chkTagNoMulti.CheckedChanged
        txtTagNo_To.Visible = chkTagNoMulti.Checked
    End Sub

    Private Sub chkLotIssue_CheckedChanged(sender As Object, e As EventArgs) Handles chkLotIssue.CheckedChanged
        If chkLotIssue.Checked = True Then
            chkLotIssue.Text = "LotIssue From"
        Else
            chkLotIssue.Text = "Lot No From"
        End If
    End Sub

    Private Sub chkMRDetails_CheckedChanged(sender As Object, e As EventArgs) Handles chkMRDetails.CheckedChanged
        grpMR_Receipt.Enabled = chkMRDetails.Checked
        If chkMRDetails.Checked Then dtpMRDate.Value = dtpFrom.Value
    End Sub

    Private Sub dtpFrom_Leave(sender As Object, e As EventArgs) Handles dtpFrom.Leave
        If grpMR_Receipt.Enabled Then
            dtpMRDate.Value = dtpFrom.Value
        End If
    End Sub

    Private Sub dtpMRDate_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label23_Click(sender As Object, e As EventArgs) Handles Label23.Click

    End Sub

    Private Sub chkCmbStoneName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkCmbStoneName.SelectedIndexChanged

    End Sub

    Private Sub chkMultiSelect_CheckedChanged(sender As Object, e As EventArgs) Handles chkMultiSelect.CheckedChanged

    End Sub

    Private Sub chkCmbStoneName_GotFocus(sender As Object, e As EventArgs) Handles chkCmbStoneName.GotFocus
        chkCmbStoneName.Enabled = Studded
    End Sub

    Private Sub chkCmbStoneName_LostFocus(sender As Object, e As EventArgs) Handles chkCmbStoneName.LostFocus
        chkCmbStoneName.Enabled = Studded
        chkCmbStSubItemName.Text = ""
        If Studded = True Then
            chkCmbStSubItemName.Items.Clear()
            strSql = vbCrLf + " SELECT DISTINCT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(ITEMID,'') IN"
            strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(METALID,'') IN ('T','D')"
            If chkCmbStoneName.Items.Count > 0 Then
                If chkCmbStoneName.GetItemChecked(0) = False Then
                    strSql += " AND ISNULL(ITEMNAME,'') IN (" & GetQryString(chkCmbStoneName.Text, ",") & ")"
                End If
            End If
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " ORDER BY SUBITEMNAME"
            Dim dtSubItem As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSubItem)
            If dtSubItem.Rows.Count > 0 Then
                chkCmbStSubItemName.Items.Add("ALL", True)
                BrighttechPack.GlobalMethods.FillCombo(chkCmbStSubItemName, dtSubItem, "SUBITEMNAME", False)
            End If
        End If
    End Sub

    Private Sub chkCmbStSubItemName_GotFocus(sender As Object, e As EventArgs) Handles chkCmbStSubItemName.GotFocus
        chkCmbStSubItemName.Enabled = Studded
    End Sub

    Private Sub gridTotalView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles gridTotalView.CellFormatting
        If gridTotalView.Rows(e.RowIndex).Cells("APPROVAL").Value.ToString = "A" Then
            gridTotalView.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Orchid
        ElseIf gridTotalView.Rows(e.RowIndex).Cells("APPROVAL").Value.ToString = "R" Then
            gridTotalView.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Orchid
        ElseIf gridTotalView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString = "S" Then
            gridTotalView.Rows(e.RowIndex).DefaultCellStyle = reportSubTotalStyle
        ElseIf gridTotalView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString = "T" Then
            gridTotalView.Rows(e.RowIndex).Cells("PARTICULAR").Style = reportHeadStyle
        ElseIf gridTotalView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString = "G" Then
            gridTotalView.Rows(e.RowIndex).DefaultCellStyle = reportTotalStyle
        End If
    End Sub

    'Private Sub dtpMRDate_TextChanged(sender As Object, e As EventArgs) Handles dtpMRDate.TextChanged
    '    If chkMRDetails.Checked Then
    '        If dtpMRDate.Value > dtpFrom.Value Then MsgBox("Meterial Receipt Date Should Be Below Recdate")
    '    End If
    'End Sub
End Class

Public Class frmTagedItemsStockViewReceiptView_Properties

    Private chkExtraWt As Boolean
    Public Property p_chkExtraWt() As Boolean
        Get
            Return chkExtraWt
        End Get
        Set(ByVal value As Boolean)
            chkExtraWt = value
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
    Private txtItemCode_NUM As String = ""
    Public Property p_txtItemCode_NUM() As String
        Get
            Return txtItemCode_NUM
        End Get
        Set(ByVal value As String)
            txtItemCode_NUM = value
        End Set
    End Property
    Private txtItemName As String = ""
    Public Property p_txtItemName() As String
        Get
            Return txtItemName
        End Get
        Set(ByVal value As String)
            txtItemName = value
        End Set
    End Property
    Private cmbSubItemName As String = ""
    Public Property p_cmbSubItemName() As String
        Get
            Return cmbSubItemName
        End Get
        Set(ByVal value As String)
            cmbSubItemName = value
        End Set
    End Property
    Private txtTagNo As String = ""
    Public Property p_txtTagNo() As String
        Get
            Return txtTagNo
        End Get
        Set(ByVal value As String)
            txtTagNo = value
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

    Private cmbCounterName As String = "ALL"
    Public Property p_cmbCounterName() As String
        Get
            Return cmbCounterName
        End Get
        Set(ByVal value As String)
            cmbCounterName = value
        End Set
    End Property
    Private cmbItemType As String = "ALL"
    Public Property p_cmbItemType() As String
        Get
            Return cmbItemType
        End Get
        Set(ByVal value As String)
            cmbItemType = value
        End Set
    End Property
    Private cmbCostCenter As String = "ALL"
    Public Property p_cmbCostCenter() As String
        Get
            Return cmbCostCenter
        End Get
        Set(ByVal value As String)
            cmbCostCenter = value
        End Set
    End Property
    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCmbCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
        End Set
    End Property
    Private txtLotNo_NUM As String = ""
    Public Property p_txtLotNo_NUM() As String
        Get
            Return txtLotNo_NUM
        End Get
        Set(ByVal value As String)
            txtLotNo_NUM = value
        End Set
    End Property
    Private txtFromWt_WET As String = ""
    Public Property p_txtFromWt_WET() As String
        Get
            Return txtFromWt_WET
        End Get
        Set(ByVal value As String)
            txtFromWt_WET = value
        End Set
    End Property
    Private txtToWt_WET As String = ""
    Public Property p_txtToWt_WET() As String
        Get
            Return txtToWt_WET
        End Get
        Set(ByVal value As String)
            txtToWt_WET = value
        End Set
    End Property
    Private txtFromDiaWt_WET As String = ""
    Public Property p_txtFromDiaWt_WET() As String
        Get
            Return txtFromDiaWt_WET
        End Get
        Set(ByVal value As String)
            txtFromDiaWt_WET = value
        End Set
    End Property
    Private txtToDiaWt_WET As String = ""
    Public Property p_txtToDiaWt_WET() As String
        Get
            Return txtToDiaWt_WET
        End Get
        Set(ByVal value As String)
            txtToDiaWt_WET = value
        End Set
    End Property
    Private txtFromRate_WET As String = ""
    Public Property p_txtFromRate_WET() As String
        Get
            Return txtFromRate_WET
        End Get
        Set(ByVal value As String)
            txtFromRate_WET = value
        End Set
    End Property
    Private txtToRate_WET As String = ""
    Public Property p_txtToRate_WET() As String
        Get
            Return txtToRate_WET
        End Get
        Set(ByVal value As String)
            txtToRate_WET = value
        End Set
    End Property
    Private ChkLstGroupBy As New List(Of String)
    Public Property p_ChkLstGroupBy() As List(Of String)
        Get
            Return ChkLstGroupBy
        End Get
        Set(ByVal value As List(Of String))
            ChkLstGroupBy = value
        End Set
    End Property
    Private rbtAll As Boolean = True
    Public Property p_rbtAll() As Boolean
        Get
            Return rbtAll
        End Get
        Set(ByVal value As Boolean)
            rbtAll = value
        End Set
    End Property
    Private rbtRegular As Boolean = False
    Public Property p_rbtRegular() As Boolean
        Get
            Return rbtRegular
        End Get
        Set(ByVal value As Boolean)
            rbtRegular = value
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
    Private chkDetailed As Boolean = False
    Public Property p_chkDetailed() As Boolean
        Get
            Return chkDetailed
        End Get
        Set(ByVal value As Boolean)
            chkDetailed = value
        End Set
    End Property
    Private chkActualDate As Boolean = False
    Public Property p_chkActualDate() As Boolean
        Get
            Return chkActualDate
        End Get
        Set(ByVal value As Boolean)
            chkActualDate = value
        End Set
    End Property
    Private chkWithCumulative As Boolean = False
    Public Property p_chkWithCumulative() As Boolean
        Get
            Return chkWithCumulative
        End Get
        Set(ByVal value As Boolean)
            chkWithCumulative = value
        End Set
    End Property
    Private cmbGroup As String = ""
    Public Property p_cmbGroup() As String
        Get
            Return cmbGroup
        End Get
        Set(ByVal value As String)
            cmbGroup = value
        End Set
    End Property
End Class
