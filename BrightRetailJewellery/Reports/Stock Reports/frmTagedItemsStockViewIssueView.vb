Imports System.Data.OleDb
Public Class frmTagedItemsStockViewIssueView
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim objMoreOption As New frmcheckothermaster
    Dim OTHmaster As Boolean = False
    Dim tempDt As New DataTable("OtherDetails")
    Dim dtStoneDetails As New DataTable("StoneDetails")
    Dim dtGrandTotalDetails As New DataTable("GrandTotal")
    Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")
    Dim headerBgColor As New System.Drawing.Color
    Dim dtCostCentre As New DataTable
    Dim dtCompany As New DataTable
    Dim DISABLE_ECISEDUTY As Boolean = IIf(GetAdmindbSoftValue("DISABLE_EXCISEDUTY", "N") = "Y", True, False)
    Dim dtMetal As New DataTable
    Dim RowFillState As Boolean = False
    Dim RPT_ISSUEVIEW_DESIGN As Boolean = IIf(GetAdmindbSoftValue("RPT_ISSUEVIEW_DESIGN", "N") = "Y", True, False)
    Dim LoosestnwtInGrswt As Boolean = IIf(GetAdmindbSoftValue("RPT_ISSUEVIEW_STNWTINGRSWT", "N") = "Y", True, False)
    Dim Studded As Boolean = False
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

        strSql = " IF (SELECT COUNT(*) FROM MASTER..SYSOBJECTS WHERE NAME = 'TEMPITEMTAGTITLE')> 0"
        strSql += vbCrLf + " DROP TABLE MASTER..TEMPITEMTAGTITLE"
        strSql += vbCrLf + " SELECT 'TAG ISSUE VIEW FROM " & dtpFrom.Text & " TO " & dtpTo.Text & "' AS TITLE INTO MASTER..TEMPITEMTAGTITLE "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " IF (SELECT COUNT(*) FROM MASTER..SYSOBJECTS WHERE NAME = 'TEMPITEMTAGSTOCK')> 0"
        strSql += vbCrLf + " DROP TABLE MASTER..TEMPITEMTAGSTOCK"
        strSql += vbCrLf + " DECLARE @DEFPATH VARCHAR(200)"
        strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
        strSql += vbCrLf + " SELECT SNO"
        strSql += vbCrLf + " ,CASE WHEN SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)"
        strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) END AS PARTICULAR"
        strSql += vbCrLf + " ," & IIf(chkactualdate.Checked, "ACTUALRECDATE", "RECDATE") & " RECDATE,TAGNO,PCS,GRSWT,NETWT,ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)),CASE WHEN PURITY<> 0 THEN PURITY ELSE NULL END)AS PURITY,RATE,MAXWAST,MAXMC,SALVALUE"
        strSql += vbCrLf + " ,(SELECT CASE WHEN ISNULL(SEAL,'') <> '' THEN SEAL ELSE DESIGNERNAME END  FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,@DEFPATH + T.PCTFILE PCTFILE,NARRATION,TAGVAL,STYLENO,CONVERT(VARCHAR,TAGVAL) + TAGKEY AS TAGID"
        strSql += vbCrLf + " ,(SELECT REMARK1 FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO) REMARK"
        strSql += vbCrLf + " INTO MASTER..TEMPITEMTAGSTOCK"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += funcFiltrationString()
        If chkWithCumulative.Checked Then
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT SNO"
            strSql += vbCrLf + " ,CASE WHEN SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)"
            strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) END AS PARTICULAR"
            strSql += vbCrLf + " ," & IIf(chkactualdate.Checked, "ACTUALRECDATE", "RECDATE") & " RECDATE,TAGNO,PCS,GRSWT,NETWT,PURITY,RATE,MAXWAST,MAXMC,SALVALUE"
            strSql += vbCrLf + " ,(SELECT CASE WHEN ISNULL(SEAL,'') <> '' THEN SEAL ELSE DESIGNERNAME END  FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
            strSql += vbCrLf + " ,@DEFPATH + T.PCTFILE PCTFILE,NARRATION,TAGVAL,STYLENO,CONVERT(VARCHAR,TAGVAL) + TAGKEY AS TAGID"
            strSql += vbCrLf + " ,(SELECT REMARK1 FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO) REMARK"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..CITEMTAG AS T"
            strSql += funcFiltrationString(True)
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " ALTER TABLE MASTER..TEMPITEMTAGSTOCK ADD TAGIMAGE IMAGE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " IF (SELECT COUNT(*) FROM MASTER..SYSOBJECTS WHERE NAME = 'TEMPITEMTAGSTONESTOCK')> 0"
        strSql += vbCrLf + " DROP TABLE MASTER..TEMPITEMTAGSTONESTOCK"
        strSql += vbCrLf + " SELECT TAGSNO,"
        strSql += vbCrLf + " CASE WHEN STNSUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.STNSUBITEMID)"
        strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID) END AS PARTICULAR"
        strSql += vbCrLf + " ,STNPCS,STNWT,STNRATE,STNAMT,CASE WHEN CALCMODE = 'W' THEN 'WIGHT' ELSE 'PIECE' END CALCMODE"
        strSql += vbCrLf + " ,CASE WHEN STONEUNIT = 'C' THEN 'CARAT' ELSE 'GRAM' END STONEUNIT,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID)AS STNTYPE"
        strSql += vbCrLf + " ,(SELECT TOP 1 SIZEDESC FROM " & cnAdminDb & "..CENTSIZE WHERE CONVERT(NUMERIC(15,4),(CASE WHEN STNPCS > 0 THEN STNWT/STNPCS ELSE STNWT END)*100) BETWEEN FROMCENT AND TOCENT)SIZEDESC"
        strSql += vbCrLf + " INTO MASTER..TEMPITEMTAGSTONESTOCK"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T"
        strSql += vbCrLf + " WHERE TAGSNO IN (SELECT SNO FROM MASTER..TEMPITEMTAGSTOCK)"
        If chkWithCumulative.Checked Then
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TAGSNO,"
            strSql += vbCrLf + " CASE WHEN STNSUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.STNSUBITEMID)"
            strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID) END AS PARTICULAR"
            strSql += vbCrLf + " ,STNPCS,STNWT,STNRATE,STNAMT,CASE WHEN CALCMODE = 'W' THEN 'WIGHT' ELSE 'PIECE' END CALCMODE"
            strSql += vbCrLf + " ,CASE WHEN STONEUNIT = 'C' THEN 'CARAT' ELSE 'GRAM' END STONEUNIT,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID)AS STNTYPE"
            strSql += vbCrLf + " ,(SELECT TOP 1 SIZEDESC FROM " & cnAdminDb & "..CENTSIZE WHERE CONVERT(NUMERIC(15,4),(CASE WHEN STNPCS > 0 THEN STNWT/STNPCS ELSE STNWT END)*100) BETWEEN FROMCENT AND TOCENT)SIZEDESC"

            strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAGSTONE AS T"
            strSql += vbCrLf + " WHERE TAGSNO IN (SELECT SNO FROM MASTER..TEMPITEMTAGSTOCK)"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim dtImage As New DataTable
        strSql = " SELECT SNO,PCTFILE FROM MASTER..TEMPITEMTAGSTOCK"
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
                    strSql = " UPDATE MASTER..TEMPITEMTAGSTOCK SET TAGIMAGE = ? WHERE SNO = '" & ro!SNO.ToString & "'"
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

    Function funcAddStoneDetails(ByVal Desc As String, ByVal dDetails As String, ByVal sDetails As String, ByVal pDetails As String) As Integer
        Dim ro As DataRow = Nothing
        ro = dtStoneDetails.NewRow
        ro("Description") = Desc
        ro("Diamond") = dDetails
        ro("Stone") = sDetails
        ro("Precious") = pDetails
        dtStoneDetails.Rows.Add(ro)
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
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE = 'T' THEN STNPCS END),0) SPCS,"
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE = 'T' AND STONEUNIT = 'C' THEN STNWT END),0)SCARAT,"
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE = 'T' AND STONEUNIT = 'G' THEN STNWT END),0)SGRAM,"
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE = 'P' THEN STNPCS END),0) PPCS,"
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE = 'P' AND STONEUNIT = 'C' THEN STNWT END),0)PCARAT,"
        strSql += " ISNULL(SUM(CASE WHEN STNTYPE = 'P' AND STONEUNIT = 'G' THEN STNWT END),0)PGRAM"
        strSql += " FROM"
        strSql += " ("
        strSql += " SELECT STNPCS,STONEUNIT,STNWT,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID)AS STNTYPE"
        strSql += " FROM " & cnAdminDb & "..ITEMTAGSTONE S" + vbCrLf
        strSql += " WHERE TAGSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "TAGISSUEVIEW)" + vbCrLf
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

    Function funcAddGrandDetails(ByVal desc As String, ByVal pcs As String, ByVal grsWt As String, ByVal lessWt As String, ByVal netWt As String, ByVal ExtraWt As String, ByVal salValue As String) As Integer
        Dim ro As DataRow = Nothing
        ro = dtGrandTotalDetails.NewRow
        ro("Description") = desc
        ro("Pcs") = pcs
        ro("GrsWt") = grsWt
        ro("LessWt") = lessWt
        ro("NetWt") = netWt
        ro("ExtraWt") = ExtraWt
        ro("SalValue") = salValue
        dtGrandTotalDetails.Rows.Add(ro)
    End Function

    Function funcFillGrandDetails() As Integer
        dtGrandTotalDetails.Clear()
        strSql = " SELECT"
        strSql += " CASE WHEN SUM(PCS) <> 0 THEN SUM(PCS) ELSE NULL END PCS,"
        strSql += " CASE WHEN SUM(GRSWT) <> 0 THEN SUM(GRSWT) ELSE NULL END GRSWT,"
        strSql += " CASE WHEN SUM(LESSWT) <> 0 THEN SUM(LESSWT) ELSE NULL END LESSWT,"
        strSql += " CASE WHEN SUM(NETWT) <> 0 THEN SUM(NETWT) ELSE NULL END NETWT,"
        strSql += " CASE WHEN SUM(EXTRAWT) <> 0 THEN SUM(EXTRAWT) ELSE NULL END EXTRAWT,"
        strSql += " CASE WHEN SUM(SALVALUE) <> 0 THEN SUM(SALVALUE) ELSE NULL END SALVALUE"
        strSql += " FROM " & cnAdminDb & "..ITEMTAG T"
        strSql += funcFiltrationString()
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CASE WHEN SUM(PCS) <> 0 THEN SUM(PCS) ELSE NULL END PCS,"
        strSql += vbCrLf + " CASE WHEN SUM(GRSWT) <> 0 THEN SUM(GRSWT) ELSE NULL END GRSWT,"
        strSql += vbCrLf + " CASE WHEN SUM(LESSWT) <> 0 THEN SUM(LESSWT) ELSE NULL END LESSWT,"
        strSql += vbCrLf + " CASE WHEN SUM(NETWT) <> 0 THEN SUM(NETWT) ELSE NULL END NETWT,"
        strSql += vbCrLf + " CASE WHEN SUM(EXTRAWT) <> 0 THEN SUM(EXTRAWT) ELSE NULL END EXTRAWT,"
        strSql += vbCrLf + " CASE WHEN SUM(SALVALUE) <> 0 THEN SUM(SALVALUE) ELSE NULL END SALVALUE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T"
        strSql += funcFiltrationString()
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
        End With
        With dt.Rows(1)
            funcAddGrandDetails("Transfer Total", .Item("Pcs").ToString, .Item("GrsWt").ToString, .Item("LessWt").ToString, .Item("NetWt").ToString, .Item("ExtraWt").ToString, .Item("SalValue").ToString)
        End With
        With dt.Rows(0)
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
        Dim ExtraWt As Double = 0
        Dim salValue As Double = 0
        For cnt As Integer = 0 To rowIndex
            If gridTotalView.Item("RESULT", cnt).Value.ToString.Trim <> "1" Then Continue For
            pcs += Val(gridTotalView.Item("Pcs", cnt).Value.ToString)
            GrsWt += Val(gridTotalView.Item("GrsWt", cnt).Value.ToString)
            lessWt += Val(gridTotalView.Item("lessWt", cnt).Value.ToString)
            netWt += Val(gridTotalView.Item("netWt", cnt).Value.ToString)
            ExtraWt += Val(gridTotalView.Item("extraWt", cnt).Value.ToString)
            salValue += Val(gridTotalView.Item("salValue", cnt).Value.ToString)
        Next
        If gridGrandTotal.Rows.Count > 1 Then
            gridGrandTotal.Item("Pcs", 1).Value = pcs
            gridGrandTotal.Item("GrsWt", 1).Value = Format(GrsWt, "0.000")
            gridGrandTotal.Item("lessWt", 1).Value = Format(lessWt, "0.000")
            gridGrandTotal.Item("netWt", 1).Value = Format(netWt, "0.000")
            gridGrandTotal.Item("extraWt", 1).Value = Format(ExtraWt, "0.000")
            gridGrandTotal.Item("salValue", 1).Value = Format(salValue, "0.00")
        End If
    End Function

    Function funcFiltrationString(Optional ByVal CTag As Boolean = False) As String
        Dim str As String = Nothing
        str = " Where (IssDate between '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' and '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "')"
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            str += " and t.itemid in (select itemid from " & cnAdminDb & "..itemmast where METALID in (select Metalid from " & cnAdminDb & "..MetalMast where MetalName IN (" & GetQryString(chkCmbMetal.Text) & ")))"
        End If
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            str += " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            'str += " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        'str += " AND COMPANYID = '" & GetStockCompId() & "'"
        If txtItemCode_NUM.Text <> "" Then
            str += " and t.itemId = '" & txtItemCode_NUM.Text & "'"
        End If
        If cmbSubItemGroup.Text <> "ALL" And cmbSubItemGroup.Text <> "" Then
            str += vbCrLf + " and t.subItemId IN (select subItemId from " & cnAdminDb & "..SubItemMast where  "
            str += vbCrLf + " sgroupid in (select sgroupid from " & cnAdminDb & "..SubItemgroup where sgroupname in (" & GetQryString(cmbSubItemGroup.Text, ",") & ")))"
        End If
        If cmbSubItemName.Enabled = True Then
            If cmbSubItemName.Text <> "ALL" And cmbSubItemName.Text <> "" Then
                'str += " and t.subItemId = isnull((select subItemId from " & cnAdminDb & "..SubItemMast where subItemName = '" & cmbSubItemName.Text & "' and itemid = " & Val(txtItemCode_NUM.Text) & "),0)"
                str += " and t.subItemId IN (select subItemId from " & cnAdminDb & "..SubItemMast where subItemName IN (" & GetQryString(cmbSubItemName.Text) & ") and itemid = " & Val(txtItemCode_NUM.Text) & ")"
            End If
        End If
        If txtTagNo.Text <> "" Then
            str += " and t.tagno = '" & txtTagNo.Text & "'"
        End If
        If txtTagAge.Text <> "" Then
            str += " and DATEDIFF(DAY," & IIf(chkactualdate.Checked, "ACTUALRECDATE", "RECDATE") & ",ISSDATE) >= '" & txtTagAge.Text & "'"
        End If
        If chkTrans.Checked = False Then str += " and toflag <> 'TR'"
        If cmbDesigner.Text <> "ALL" And cmbDesigner.Text <> "" Then
            str += " and DesignerId = (Select DesignerId from " & cnAdminDb & "..Designer where DesignerName = '" & cmbDesigner.Text & "')"
        End If
        If cmbCounterName.Text <> "ALL" And cmbCounterName.Text <> "" Then
            'str += " and ItemCtrId = (Select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName = '" & cmbCounterName.Text & "')"
            str += " and ItemCtrId IN (Select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName IN (" & GetQryString(cmbCounterName.Text) & "))"
        End If
        If cmbItemType.Text <> "ALL" And cmbItemType.Text <> "" Then
            str += " and itemTypeId = (select itemTypeId from " & cnAdminDb & "..itemType where Name = '" & cmbItemType.Text & "')"
        End If

        'If cmbCostCenter.Text <> "ALL" And cmbCostCenter.Text <> "" Then
        '    str += " and COSTID = (select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCenter.Text & "')"
        'End If

        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            str += " AND COSTID IN"
            str += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If

        If CmbSize.Text <> "ALL" And CmbSize.Text <> "" Then
            str += " AND SIZEID IN (SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & CmbSize.Text & "'"
            If txtItemCode_NUM.Text <> "" Then str += " AND ITEMID='" & txtItemCode_NUM.Text & "' "
            str += " )"
        End If
        If chkLotIssue.Checked = True Then
            If txtLotNo_NUM.Text <> "" Then
                str += " AND LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO IN (SELECT LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE TRANNO = '" & txtLotNo_NUM.Text.Trim & "'))"
            End If
        Else
            If txtLotNo_NUM.Text <> "" Then
                'str += " AND LOTSNO = '" & txtLotNo_NUM.Text & "'"
                str += " and LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LotNo = '" & txtLotNo_NUM.Text & "')"
            End If
        End If

        If txtFromWt_WET.Text <> "" Or txtToWt_WET.Text <> "" Then
            str += " and (T.GrsWt between '" & Val(txtFromWt_WET.Text) & "' and '" & Val(txtToWt_WET.Text) & "')"
        End If
        If txtFromDiaWt_WET.Text <> "" Or txtToDiaWt_WET.Text <> "" Then
            str += " and ((select sum(stnwt) from " & cnAdminDb & ".." & IIf(CTag, "CITEMTAGSTONE", "ITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) between '" & Val(txtFromDiaWt_WET.Text) & "' and '" & Val(txtToDiaWt_WET.Text) & "')"
        End If
        'If txtFromDiaWt_WET.Text <> "" Or txtToDiaWt_WET.Text <> "" Then
        '    str += " and (DiaWt between '" & Val(txtFromDiaWt_WET.Text) & "' and '" & Val(txtToDiaWt_WET.Text) & "')"
        'End If
        If txtFromRate_WET.Text <> "" Or txtToRate_WET.Text <> "" Then
            str += " and (SalValue between '" & Val(txtFromRate_WET.Text) & "' and '" & Val(txtToRate_WET.Text) & "')"
        End If
        If rbtRegular.Checked = True Then
            str += " and ordRepNo =''"
        End If
        If rbtOrder.Checked = True Then
            str += " and ordRepNo <> ''"
        End If
        If txtSearch_txt.Text <> "" And cmbSearchKey.Text <> "" Then
            str += " AND T." & cmbSearchKey.Text & " LIKE"
            str += " '" & txtSearch_txt.Text & "%'"
        End If
        If OTHmaster = True Then
            If objMoreOption.chkothermisc.Text <> "" Then
                str += vbCrLf + " AND T.SNO IN (SELECT TAGSNO FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE  OTHID IN(SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME IN (" & GetQryString(objMoreOption.chkothermisc.Text) & ")))"
            End If
        End If
        If cmbStockType.Text = "MANUFACTURING" Then
            str += " AND ISNULL(T.STKTYPE,'')='M'"
        ElseIf cmbStockType.Text = "EXEMPTED" Then
            str += " AND ISNULL(T.STKTYPE,'')='E'"
        ElseIf cmbStockType.Text = "TRADING" Then
            str += " AND ISNULL(T.STKTYPE,'T') NOT IN('M','E') "
        End If
        If ChkMultimetal.Checked = True Then
            str += " AND T.SNO IN (SELECT TAGSNO FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE RECDATE between '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' and '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' ) "
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

        Return str
    End Function

    Private Sub frmTagedItemsStockViewIssueView_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPITEMTAGTITLE')> 0"
        strSql += "     DROP TABLE TEMPTABLEDB..TEMPITEMTAGTITLE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPITEMTAGSTOCK')> 0"
        strSql += "     DROP TABLE TEMPTABLEDB..TEMPITEMTAGSTOCK"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPITEMTAGSTONESTOCK')> 0"
        strSql += "    DROP TABLE TEMPTABLEDB..TEMPITEMTAGSTONESTOCK"
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
        If DISABLE_ECISEDUTY Then
            Label24.Enabled = False
            cmbStockType.Enabled = False
        End If
        'Me.WindowState = FormWindowState.Maximized
        If Not defaultPic.EndsWith("\") Then defaultPic += "\"

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
        dtGrandTotalDetails.Columns.Add("NetWt", GetType(String))
        dtGrandTotalDetails.Columns.Add("ExtraWt", GetType(String))
        dtGrandTotalDetails.Columns.Add("SalValue", GetType(String))

        ''Checking CostCentre Status
        strSql = " select 1 from " & cnAdminDb & "..softcontrol where ctlText = 'Y' and ctlId = 'COSTCENTRE'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        'OLD COST CENTRE LOAD
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

        SqlVersion = ""
        strSql = " SELECT	SUBSTRING(CONVERT(VARCHAR,SERVERPROPERTY('PRODUCTVERSION')),1,CHARINDEX('.',CONVERT(VARCHAR,SERVERPROPERTY('PRODUCTVERSION')))-1) AS [VERSION]"
        SqlVersion = GetSqlValue(cn, strSql)

        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If chkExport.Checked = True Then
            Dim obj As New frmTagedItemStockExportM1(Val(txtItemCode_NUM.Text), 0, Format(dtpFrom.Value.Date, "yyyy-MM-dd") _
                                                   , Format(dtpTo.Value.Date, "yyyy-MM-dd"), GetQryString(chkCmbCostCentre.Text, ","), "ISSUE")
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
        Try
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
            OtherView()
            btnView_Search.Enabled = False
            Dim grpCtr As Boolean = False
            For cnt As Integer = 0 To ChkLstGroupBy.CheckedItems.Count - 1
                If ChkLstGroupBy.CheckedItems.Item(cnt).ToString = "COUNTER" Then
                    grpCtr = True
                End If
            Next

            strSql = vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "TAGISSUEVIEW')>0"
            strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "TAGISSUEVIEW"
            strSql += vbCrLf + " DECLARE @DEFPATH VARCHAR(200)"
            strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
            strSql += vbCrLf + " SELECT CONVERT(INT,NULL)SLNO,CONVERT(VARCHAR(15),SNO)SNO,"
            strSql += vbCrLf + " CASE WHEN ISNULL(SM.SUBITEMNAME,'') <> '' THEN "
            strSql += vbCrLf + " SM.SUBITEMNAME "
            strSql += vbCrLf + " ELSE IM.ITEMNAME END AS PARTICULAR,"
            strSql += vbCrLf + " " & IIf(chkactualdate.Checked, "ACTUALRECDATE", "RECDATE") & " RECDATE,ISSDATE,"
            strSql += vbCrLf + " (SELECT TOP 1 TRANNO FROM " & cnStockDb & "..ISSUE WHERE TAGNO = T.TAGNO AND ITEMID = T.ITEMID AND BATCHNO=T.BATCHNO) AS BILLNO"
            'strSql += vbCrLf + " CASE WHEN RECDATE IS NULL THEN '' ELSE CONVERT(VARCHAR(12),RECDATE,103) END AS RECDATE,"
            'strSql += vbCrLf + " CASE WHEN ISSDATE IS NULL THEN '' ELSE CONVERT(VARCHAR(12),ISSDATE,103) END AS ISSDATE,"
            strSql += vbCrLf + " ,TAGNO,T.PCS,T.GRSWT,LESSWT,"
            strSql += vbCrLf + " NETWT,T.EXTRAWT,CASE WHEN TOUCH <> 0 THEN TOUCH ELSE NULL END AS TOUCH,GRSNET,RATE,SALVALUE,"

            strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNPCS),0) SPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNPCS ,"


            strSql += vbCrLf + " (SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT ELSE (CASE WHEN S.STONEUNIT = 'C' THEN CONVERT(NUMERIC(15,3),S.STNWT) END) END),0) STNWT "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.STONEUNIT ='G' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWT ,"
            'STN CARAT
            strSql += vbCrLf + " (SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT ELSE (CASE WHEN S.STONEUNIT = 'C' THEN CONVERT(NUMERIC(15,3),S.STNWT) END) END),0) STNWT "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.STONEUNIT ='C' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWTC ,"

            'stnamt
            strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNAMT),0) SAMT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNAMT ,"

            strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNPCS),0) DPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAPCS ,"

            'strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT "
            'strSql += vbCrLf + " ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            'strSql += vbCrLf + " S.STONEUNIT ='G' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT ,"
            ''DIA CARAT
            'strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT "
            'strSql += vbCrLf + " ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            'strSql += vbCrLf + " S.STONEUNIT ='C' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWTC ,"

            strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 "
            strSql += vbCrLf + " ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT ,"

            'DiaAmt
            strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNAMT),0) DAMT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAAMT ,"

            strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNPCS),0) PPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREPCS ,"

            strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 "
            strSql += vbCrLf + " ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) PWT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))) PREWT ,"

            'PreAmt
            strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNAMT),0) PAMT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREAMT ,"

            strSql += vbCrLf + " CASE WHEN MAXWASTPER <> 0 THEN MAXWASTPER ELSE NULL END AS [WAST%],CASE WHEN MAXWAST <> 0 THEN MAXWAST ELSE NULL END [WAST],"
            strSql += vbCrLf + " CASE WHEN MAXMCGRM <> 0 THEN MAXMCGRM ELSE NULL END AS [MCGRM],CASE WHEN MAXMC <> 0 THEN MAXMC ELSE NULL END [MC],"
            strSql += vbCrLf + " (SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)TAGTYPE,"

            strSql += vbCrLf + " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)COUNTER,"
            If grpCtr = True Then
                strSql += vbCrLf + " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)ITEMCOUNTER,"
            End If

            strSql += vbCrLf + " T.TABLECODE,ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)),CASE WHEN PURITY<> 0 THEN PURITY ELSE NULL END)AS PURITY,NARRATION,BRANDID"
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
            strSql += vbCrLf + " ,SALEMODE,@DEFPATH + T.PCTFILE PCTFILE,TRANINVNO,SUPBILLNO/*,HM_BILLNO HALLMARKNO*/"
            If Val(SqlVersion) > 8 Then
                strSql += vbCrLf + " , (STUFF((SELECT CAST(',' + H.[HM_BILLNO] AS VARCHAR(MAX)) FROM " & cnAdminDb & "..ITEMTAGHALLMARK H WHERE (T.SNO =H.TAGSNO) FOR XML PATH ('')), 1, 1, '')) HALLMARKNO"
            Else
                strSql += vbCrLf + " ,HM_BILLNO HALLMARKNO"
            End If
            strSql += vbCrLf + " ,ENTRYMODE,CASE WHEN T.TOFLAG='SA' THEN 'SALES' WHEN T.TOFLAG='MI' THEN 'MISC ISSUE' WHEN T.TOFLAG='OI' THEN 'OTHER ISSUE' WHEN T.TOFLAG='TR' THEN 'TRANSFER' ELSE TOFLAG END AS SALETYPE,"
            strSql += vbCrLf + " T.USERID,CONVERT(VARCHAR(12),T.UPDATED,103)UPDATED,CONVERT(VARCHAR(10),t.UPTIME,108)UPTIME,T.ITEMID,CONVERT(VARCHAR(1),'')COLHEAD,TAGVAL,1 RESULT"
            strSql += vbCrLf + " ,(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = T.USERID)AS USERNAME,T.STYLENO"
            strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)AS COSTCENTRE"
            strSql += vbCrLf + " ,IM.ITEMNAME aS ITEM"
            strSql += vbCrLf + " ,SM.SUBITEMNAME aS SUBITEM"
            strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
            strSql += vbCrLf + " ,(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)AS ITEMSIZE,T.SIZEID AS ITEMSIZEID"
            strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = (SELECT TOP 1 EMPID FROM " & cnStockDb & "..ISSUE WHERE TAGNO = T.TAGNO AND ITEMID = T.ITEMID AND BATCHNO = T.BATCHNO)) AS SALEMAN"
            strSql += vbCrLf + " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO)) AS CUSTOMER"
            strSql += vbCrLf + " ,(SELECT REMARK1 FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO) REMARK"
            strSql += vbCrLf + " ,(SELECT TOP 1 CAPTION FROM " & cnAdminDb & "..RANGEMAST AS R WHERE R.ITEMID=T.ITEMID AND R.SUBITEMID=T.SUBITEMID "
            strSql += vbCrLf + " AND T.GRSWT BETWEEN R.FROMWEIGHT AND R.TOWEIGHT)RANGE, DATEDIFF(DAY," & IIf(chkactualdate.Checked, "ACTUALRECDATE", "RECDATE") & ",ISSDATE) AS AGE,ORDREPNO  "
            strSql += vbCrLf + " ,(SELECT SGROUPNAME FROM " & cnAdminDb & "..SUBITEMGROUP WHERE SGROUPID=SM.SGROUPID) SUBITEMGROUP"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "TAGISSUEVIEW FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = T.ITEMID AND SM.SUBITEMID = T.SUBITEMID"
            strSql += funcFiltrationString()
            If chkWithCumulative.Checked Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT CONVERT(INT,NULL)SLNO,CONVERT(VARCHAR(15),SNO)SNO,"
                strSql += vbCrLf + " CASE WHEN ISNULL(SM.SUBITEMNAME,'') <> ''  THEN "
                strSql += vbCrLf + " SM.SUBITEMNAME"
                strSql += vbCrLf + " ELSE IM.ITEMNAME END AS PARTICULAR,"

                strSql += vbCrLf + " " & IIf(chkactualdate.Checked, "ACTUALRECDATE", "RECDATE") & " RECDATE,ISSDATE,"
                strSql += vbCrLf + " (SELECT TOP 1 TRANNO FROM " & cnStockDb & "..ISSUE WHERE TAGNO = T.TAGNO AND ITEMID = T.ITEMID AND BATCHNO=T.BATCHNO ) AS BILLNO"
                'strSql += vbCrLf + " CASE WHEN RECDATE IS NULL THEN '' ELSE CONVERT(VARCHAR(12),RECDATE,103) END AS RECDATE,"
                'strSql += vbCrLf + " CASE WHEN ISSDATE IS NULL THEN '' ELSE CONVERT(VARCHAR(12),ISSDATE,103) END AS ISSDATE,"
                strSql += vbCrLf + " ,TAGNO,T.PCS,T.GRSWT,T.LESSWT,"
                strSql += vbCrLf + " NETWT,T.EXTRAWT,CASE WHEN TOUCH <> 0 THEN TOUCH ELSE NULL END AS TOUCH,GRSNET,RATE,SALVALUE,"

                strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNPCS),0) SPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND S.TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNPCS ,"

                strSql += vbCrLf + " (SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT ELSE (CASE WHEN S.STONEUNIT = 'C' THEN CONVERT(NUMERIC(15,3),S.STNWT) END) END),0) STNWT "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE S.STONEUNIT ='G' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND S.TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWT ,"
                'STN CARAT
                strSql += vbCrLf + " (SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT  ELSE (CASE WHEN S.STONEUNIT = 'C' THEN CONVERT(NUMERIC(15,3),S.STNWT) END) END),0) STNWT "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE S.STONEUNIT ='C' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWTC ,"

                'StnAmt
                strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNAMT),0) SAMT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND S.TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNAMT ,"

                strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNPCS),0) DPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND S.TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAPCS ,"


                'strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT "
                'strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAGSTONE S WHERE S.STONEUNIT ='G' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND S.TAGSNO = T.SNO AND S.RECDATE =T.RECDATE AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT ,"
                ''DIA CARAT
                'strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT "
                'strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAGSTONE S WHERE S.STONEUNIT ='C' AND S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWTC ,"

                strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT ,"

                'DiaAmt
                strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNAMT),0) DAMT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND S.TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAAMT ,"

                strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNPCS),0) PPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND S.TAGSNO = T.SNO  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREPCS ,"

                strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) PWT "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND S.TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))) PREWT ,"

                'PreAmt
                strSql += vbCrLf + " (SELECT ISNULL(SUM(S.STNAMT),0) PAMT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND S.TAGSNO = T.SNO  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREAMT ,"

                strSql += vbCrLf + " CASE WHEN MAXWASTPER <> 0 THEN MAXWASTPER ELSE NULL END AS [WAST%],CASE WHEN MAXWAST <> 0 THEN MAXWAST ELSE NULL END [WAST],"
                strSql += vbCrLf + " CASE WHEN MAXMCGRM <> 0 THEN MAXMCGRM ELSE NULL END AS [MCGRM],CASE WHEN MAXMC <> 0 THEN MAXMC ELSE NULL END [MC],"
                strSql += vbCrLf + " (SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)TAGTYPE,"

                strSql += vbCrLf + " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)COUNTER,"
                If grpCtr = True Then
                    strSql += vbCrLf + " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)ITEMCOUNTER,"
                End If
                strSql += vbCrLf + " T.TABLECODE,ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)),CASE WHEN PURITY<> 0 THEN PURITY ELSE NULL END)AS PURITY,NARRATION,BRANDID"
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
                strSql += vbCrLf + " ,SALEMODE,@DEFPATH + T.PCTFILE,TRANINVNO,SUPBILLNO/*,HM_BILLNO HALLMARKNO*/"
                If Val(SqlVersion) > 8 Then
                    strSql += vbCrLf + " , (STUFF((SELECT CAST(',' + H.[HM_BILLNO] AS VARCHAR(MAX)) FROM " & cnAdminDb & "..ITEMTAGHALLMARK H WHERE (T.SNO =H.TAGSNO) FOR XML PATH ('')), 1, 1, '')) HALLMARKNO"
                Else
                    strSql += vbCrLf + " ,HM_BILLNO HALLMARKNO"
                End If
                strSql += vbCrLf + " ,ENTRYMODE,CASE WHEN T.TOFLAG='SA' THEN 'SALES' WHEN T.TOFLAG='MI' THEN 'MISC ISSUE' WHEN T.TOFLAG='OI' THEN 'OTHER ISSUE' WHEN T.TOFLAG='TR' THEN 'TRANSFER' ELSE TOFLAG  END AS SALETYPE,"
                strSql += vbCrLf + " T.USERID,CONVERT(VARCHAR(12),T.UPDATED,103)UPDATED,CONVERT(VARCHAR(10),T.UPTIME,108)UPTIME,T.ITEMID,CONVERT(VARCHAR(1),'')COLHEAD,TAGVAL,1 RESULT"
                strSql += vbCrLf + " ,(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = T.USERID)AS USERNAME,T.STYLENO"
                strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)AS COSTCENTRE"
                strSql += vbCrLf + " ,IM.ITEMNAME aS ITEM"
                strSql += vbCrLf + " ,SM.SUBITEMNAME AS SUBITEM"
                strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
                strSql += vbCrLf + " ,(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)AS ITEMSIZE,T.SIZEID AS ITEMSIZEID"
                strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = (SELECT TOP 1 EMPID FROM " & cnStockDb & "..ISSUE WHERE TAGNO = T.TAGNO AND ITEMID = T.ITEMID AND BATCHNO = T.BATCHNO)) AS SALEMAN"
                strSql += vbCrLf + " ,(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO)) AS CUSTOMER"
                strSql += vbCrLf + " ,(SELECT REMARK1 FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = T.BATCHNO) REMARK"
                strSql += vbCrLf + " ,(SELECT TOP 1 CAPTION FROM " & cnAdminDb & "..RANGEMAST AS R WHERE R.ITEMID=T.ITEMID AND R.SUBITEMID=T.SUBITEMID "
                strSql += vbCrLf + " AND T.GRSWT BETWEEN R.FROMWEIGHT AND R.TOWEIGHT)RANGE , DATEDIFF(DAY," & IIf(chkactualdate.Checked, "ACTUALRECDATE", "RECDATE") & ",ISSDATE) AS AGE,ORDREPNO"
                strSql += vbCrLf + " ,(SELECT SGROUPNAME FROM " & cnAdminDb & "..SUBITEMGROUP WHERE SGROUPID=SM.SGROUPID) SUBITEMGROUP"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG AS T"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = T.ITEMID AND SM.SUBITEMID = T.SUBITEMID"
                strSql += funcFiltrationString(True)
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If LoosestnwtInGrswt = False Then
                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "TAGISSUEVIEW"
                strSql += vbCrLf + " SET PCS = null,GRSWT = null,NETWT = null,DIAPCS = L.PCS"
                strSql += vbCrLf + " ,DIAWT = L.GRSWT"
                strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "TAGISSUEVIEW AS L"
                strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'D')"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

                strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "TAGISSUEVIEW"
                strSql += vbCrLf + " SET PCS = null,GRSWT = null,NETWT = null,STNPCS = L.PCS"
                strSql += vbCrLf + " ,STNWT = L.GRSWT"
                strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "TAGISSUEVIEW AS L"
                strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'T')"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
            End If


            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "TAGISSUEVIEW"
            Dim dtSource As New DataTable
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


            ObjGrouper.pColumns_Sum.Add("PCS")
            ObjGrouper.pColumns_Sum.Add("GRSWT")
            ObjGrouper.pColumns_Sum.Add("LESSWT")
            ObjGrouper.pColumns_Sum.Add("NETWT")
            ObjGrouper.pColumns_Sum.Add("EXTRAWT")
            ObjGrouper.pColumns_Sum.Add("SALVALUE")
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
            ObjGrouper.pColName_Particular = "PARTICULAR"
            ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
            'ObjGrouper.pColumns_Sort = "TAGVAL"
            Dim filter As String = ""
            For i As Integer = 0 To LstOrderby.Items.Count - 1
                If LstOrderby.Items(i).ToString = "TAGNO" Then
                    filter += IIf(LstOrderby.Items(i).ToString = "TAGNO", "TAGVAL", LstOrderby.Items(i).ToString)
                ElseIf LstOrderby.Items(i).ToString = "ITEMNAME" Then
                    filter += IIf(LstOrderby.Items(i).ToString = "ITEMNAME", "PARTICULAR", LstOrderby.Items(i).ToString)
                ElseIf LstOrderby.Items(i).ToString = "SALEVALUE" Then
                    filter += IIf(LstOrderby.Items(i).ToString = "SALEVALUE", "SALVALUE", LstOrderby.Items(i).ToString)
                Else
                    filter += LstOrderby.Items(i).ToString
                End If
                If i < LstOrderby.Items.Count - 1 Then filter += ","
            Next
            If filter <> "" Then
                ObjGrouper.pColumns_Sort = filter
            End If
            ObjGrouper.pIdentityColName = "SLNO"
            ObjGrouper.pColumns_Count.Add("TAGNO")
            RowFillState = True
            ObjGrouper.GroupDgv()
            RowFillState = False
            'If cmbGroup.Text = "COUNTER" Then
            '    strSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "TAGISSUEVIEW) > 0"
            '    strSql += vbCrLf + " BEGIN"
            '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "TAGISSUEVIEW(PARTICULAR,ITEMCTRNAME,RESULT,COLHEAD)"
            '    strSql += vbCrLf + " SELECT DISTINCT ITEMCTRNAME,ITEMCTRNAME,0 RESULT,'T' FROM TEMP" & systemId & "TAGISSUEVIEW WHERE RESULT = 1"
            '    strSql += vbCrLf + " "
            '    strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "TAGISSUEVIEW(PARTICULAR,ITEMCTRNAME"
            '    strSql += vbCrLf + " ,PCS,GRSWT,LESSWT,NETWT,STNPCS,STNWT,DIAPCS,DIAWT,WAST,MC,SALVALUE,RESULT,COLHEAD)"
            '    strSql += vbCrLf + " SELECT DISTINCT ITEMCTRNAME,ITEMCTRNAME,SUM(PCS),SUM(GRSWT),SUM(LESSWT),SUM(NETWT),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(WAST),SUM(MC),SUM(SALVALUE)"
            '    strSql += vbCrLf + " ,2 RESULT,'S' COLHEAD FROM TEMP" & systemId & "TAGISSUEVIEW WHERE RESULT = 1 GROUP BY ITEMCTRNAME"
            '    strSql += vbCrLf + " END"
            '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            '    cmd.ExecuteNonQuery()
            'End If

            'strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "TAGISSUEVIEW)>0"
            'strSql += " BEGIN"
            'strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "TAGISSUEVIEW(PARTICULAR,ITEMCTRNAME"
            'strSql += vbCrLf + " ,PCS,GRSWT,LESSWT,NETWT,STNPCS,STNWT,DIAPCS,DIAWT,WAST,MC,SALVALUE,RESULT,COLHEAD)"
            'strSql += vbCrLf + " SELECT DISTINCT 'GRAND TOTAL','ZZZZ'ITEMCTRNAME,SUM(PCS),SUM(GRSWT),SUM(LESSWT),SUM(NETWT),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(WAST),SUM(MC),SUM(SALVALUE)"
            'strSql += vbCrLf + " ,3 RESULT,'G' COLHEAD FROM TEMP" & systemId & "TAGISSUEVIEW WHERE RESULT = 1 "

            'strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "TAGISSUEVIEW(PARTICULAR,ITEMCTRNAME"
            'strSql += vbCrLf + " ,PCS,RESULT,COLHEAD)"
            'strSql += vbCrLf + " SELECT DISTINCT 'NO OF TAG`S','ZZZZ'ITEMCTRNAME,COUNT(*)"
            'strSql += vbCrLf + " ,4 RESULT,'G' COLHEAD FROM TEMP" & systemId & "TAGISSUEVIEW WHERE RESULT = 1 AND ISNULL(TAGNO,'') <> ''"
            'strSql += " END"
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()


            'strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "TAGISSUEVIEW)>0 "
            'strSql += " BEGIN "
            'strSql += " UPDATE TEMP" & systemId & "TAGISSUEVIEW SET PCS = NULL WHERE PCS = 0 "
            'strSql += " UPDATE TEMP" & systemId & "TAGISSUEVIEW SET GRSWT = NULL WHERE GRSWT = 0 "
            'strSql += " UPDATE TEMP" & systemId & "TAGISSUEVIEW SET LESSWT = NULL WHERE LESSWT = 0 "
            'strSql += " UPDATE TEMP" & systemId & "TAGISSUEVIEW SET NETWT = NULL WHERE NETWT = 0 "
            'strSql += " UPDATE TEMP" & systemId & "TAGISSUEVIEW SET RATE = NULL WHERE RATE = 0 "
            'strSql += " UPDATE TEMP" & systemId & "TAGISSUEVIEW SET SALVALUE = NULL WHERE SALVALUE = 0 "
            'strSql += " UPDATE TEMP" & systemId & "TAGISSUEVIEW SET STNPCS = NULL WHERE STNPCS = 0 "
            'strSql += " UPDATE TEMP" & systemId & "TAGISSUEVIEW SET STNWT = NULL WHERE STNWT = 0 "
            'strSql += " UPDATE TEMP" & systemId & "TAGISSUEVIEW SET DIAPCS = NULL WHERE DIAPCS = 0 "
            'strSql += " UPDATE TEMP" & systemId & "TAGISSUEVIEW SET DIAWT = NULL WHERE DIAWT = 0 "
            'strSql += " UPDATE TEMP" & systemId & "TAGISSUEVIEW SET PURITY = NULL WHERE PURITY = 0 "
            'strSql += " END "
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()

            'strSql = " SELECT * FROM TEMP" & systemId & "TAGISSUEVIEW "
            'If cmbGroup.Text = "NONE" Then
            '    strSql += " order by RESULT,TAGVAL"
            'ElseIf cmbGroup.Text = "COUNTER" Then
            '    strSql += " ORDER BY ITEMCTRNAME,RESULT,TAGVAL"
            'End If

            ''strSql = " SELECT * FROM TEMP" & systemId & "TAGRECEIPTVIEW ORDER BY TAGVAL"
            'da = New OleDbDataAdapter(strSql, cn)
            'dt = New DataTable("ISSUEVIEW")
            'dt.Columns.Add("KEYNO", GetType(Integer))
            'dt.Columns("KEYNO").AutoIncrement = True
            'dt.Columns("KEYNO").AutoIncrementSeed = 0
            'dt.Columns("KEYNO").AutoIncrementStep = 1
            'da.Fill(dt)

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

            ''dt.AcceptChanges()
            'gridTotalView.DataSource = dt
            'tabView.Show()
            'FillGridGroupStyle_KeyNoWise(gridTotalView)
            ''GridViewFormat()
            'If gridTotalView.RowCount > 0 Then
            '    gridTotalView.Rows(gridTotalView.RowCount - 1).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
            'End If
            ''StoneDetails
            funcFillStoneDetails()
            ''GrandTotalDetails
            funcFillGrandDetails()

            With gridTotalView
                For cnt As Integer = 0 To gridTotalView.ColumnCount - 1
                    gridTotalView.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("COLHEAD").Visible = False
                .Columns("ITEMID").Visible = False
                .Columns("TAGVAL").Visible = False
                .Columns("KEYNO").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("ITEM").Visible = False
                .Columns("SUBITEM").Visible = False
                '.Columns("DESIGNER").Visible = False
                If RPT_ISSUEVIEW_DESIGN = True Then
                    .Columns("DESIGNER").Visible = False
                End If
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
                With .Columns("ISSDATE")
                    .HeaderText = "ISSDATE"
                    .DefaultCellStyle.Format = "dd/MM/yyyy"
                    .Width = 80
                End With
                With .Columns("ISSDATE")
                    .HeaderText = "ISSDATE"
                    .Width = 80
                End With
                With .Columns("BILLNO")
                    .HeaderText = "BILL NO"
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("TAGNO")
                    .HeaderText = "TAG NO"
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
                    .Width = 70
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
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.000"
                End With
                With .Columns("EXTRAWT")
                    .HeaderText = "EXTRAWT"
                    .Width = 70
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
                    .HeaderText = "STN_WT (CARAT)"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = "0.000"
                End With
                With .Columns("DIAPCS")
                    .HeaderText = "DIA PCS"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("DIAWT")
                    .HeaderText = "DIA_WT (CARAT)"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                End With
                'With .Columns("DIAWTC")
                '    .HeaderText = "DIA_WT (CARAT)"
                '    .Width = 70
                '    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                '    .DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                'End With
                With .Columns("PREPCS")
                    .HeaderText = "PRE PCS"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("PREWT")
                    .HeaderText = "PRE_WT (CARAT)"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
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
                .Columns("CUSTOMER").Width = 150
                With .Columns("REMARK")
                    .Width = 120
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
            lblReportTitle.Text = "TAGED ITEM STOCK ISSUE "
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
        funcAddNewRow("MakingCharg ", Nothing, "Min MakingCharg ", Nothing, "LotNo", Nothing, "", "")
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
        strSql += " (SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = T.LOTSNO)LOTNO,"
        strSql += " CASE TOFLAG WHEN 'SA' THEN 'SALES'"
        strSql += " WHEN 'AI' THEN 'APPROVAL ISSUE'"
        strSql += " WHEN 'MI' THEN 'MISCELLANEOUS ISSUE' END AS TOFLAG"
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
            funcAddNewRow("MakingCharg ", .Item("MaxMc").ToString, "Min MakingCharg ", .Item("MinMc").ToString, "LotNo", .Item("LotNo").ToString, "Trantype", .Item("toflag").ToString)
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
            'End If
            strSql = " Select itemName,STUDDEDSTONE from " & cnAdminDb & "..itemMast where itemId = '" & txtItemCode_NUM.Text & "'"
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
                strSql += vbCrLf + " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
            End If
            Dim dt As New DataTable
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtItemName.Text = dt.Rows(0).Item("itemName")
                If dt.Rows(0).Item("STUDDEDSTONE").ToString = "Y" Then
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
    Private Sub ChklstboxOrderby_ItemCheck(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles ChklstboxOrderby.ItemCheck
        If ChklstboxOrderby.SelectedIndex < 0 Then Exit Sub
        If ChklstboxOrderby.GetItemChecked(ChklstboxOrderby.SelectedIndex) = False Then
            LstOrderby.Items.Add(ChklstboxOrderby.Items(ChklstboxOrderby.SelectedIndex).ToString())
        Else
            LstOrderby.Items.Remove(ChklstboxOrderby.Items(ChklstboxOrderby.SelectedIndex).ToString())
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
            ''Size
            CmbSize.Items.Clear()
            CmbSize.Items.Add("ALL")
            strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID='" & txtItemCode_NUM.Text & "' ORDER BY SIZENAME"
            objGPack.FillCombo(strSql, CmbSize, False, False)
            ' cmbSize.Text = "ALL"

        Else
            cmbSubItemName.Items.Clear()
            cmbSubItemName.Enabled = False
        End If
    End Sub
    Private Sub cmbSubItemGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSubItemGroup.TextChanged
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
        ElseIf e.KeyChar = Chr(Keys.S) Or AscW(e.KeyChar) = 115 Then
            If gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("SNO").Value.ToString = "" Then Exit Sub
            Dim objStnDetails As New frmTagStoneDetails(gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("SNO").Value.ToString, lblReportTitle.Text)
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            If gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("SNO").Value.ToString = "" Then Exit Sub
            Dim objTagViewer As New frmTagImageViewer(
            gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("TAGNO").Value.ToString,
            gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("ITEMID").Value.ToString)
            objTagViewer.ShowDialog()
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click

        LstOrderby.Items.Clear()
        LstOrderby.Items.Add("TAGNO")

        ChklstboxOrderby.Items.Clear()
        ChklstboxOrderby.Items.Add("TAGNO", True)
        ChklstboxOrderby.Items.Add("ISSDATE", False)
        ChklstboxOrderby.Items.Add("GRSWT", False)
        ChklstboxOrderby.Items.Add("SALEVALUE", False)
        ChklstboxOrderby.Items.Add("BILLNO", False)
        ChklstboxOrderby.Items.Add("TRANINVNO", False)
        ChklstboxOrderby.Items.Add("ITEMNAME", False)
        ChklstboxOrderby.Items.Add("BILLNO", False)
        objGPack.TextClear(Me)
        cmbSubItemName.Items.Clear()
        cmbSubItemName.Enabled = False

        cmbGroup.Items.Clear()
        cmbGroup.Items.Add("NONE")
        cmbGroup.Items.Add("COUNTER")
        'cmbGroup.Text = "NONE"

        ''Designer
        cmbDesigner.Items.Clear()
        cmbDesigner.Items.Add("ALL")
        strSql = " Select DesignerName from " & cnAdminDb & "..Designer order by DesignerName"
        objGPack.FillCombo(strSql, cmbDesigner, False)
        ' cmbDesigner.Text = "ALL"

        ''Counter
        cmbCounterName.Items.Clear()
        cmbCounterName.Items.Add("ALL")
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..itemCounter order by itemCtrName"
        Dim dtCtr As DataTable
        dtCtr = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCtr)
        BrighttechPack.GlobalMethods.FillCombo(cmbCounterName, dtCtr, "ITEMCTRNAME", False)
        'objGPack.FillCombo(strSql, cmbCounterName, False)
        ' cmbCounterName.Text = "ALL"

        ''Size
        CmbSize.Items.Clear()
        CmbSize.Items.Add("ALL")
        strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE ORDER BY SIZENAME"
        objGPack.FillCombo(strSql, CmbSize, False, False)
        ' cmbSize.Text = "ALL"


        ''ItemType
        cmbItemType.Items.Clear()
        cmbItemType.Items.Add("ALL")
        strSql = " Select Name from " & cnAdminDb & "..itemType order by Name"
        objGPack.FillCombo(strSql, cmbItemType, False)
        ' cmbItemType.Text = "ALL"

        ''stockType
        cmbStockType.Items.Clear()
        cmbStockType.Items.Add("ALL")
        cmbStockType.Items.Add("EXEMPTED")
        cmbStockType.Items.Add("MANUFACTURING")
        cmbStockType.Items.Add("TRADING")
        cmbStockType.Text = "ALL"

        ''CostCenter

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
        'rbtAll.Checked = True
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
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
        txtItemName.Text = ""
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridTotalView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblReportTitle.Text, gridTotalView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridTotalView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblReportTitle.Text, gridTotalView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        txtItemCode_NUM.Focus()
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub


    Private Sub Prop_Sets()
        Dim obj As New frmTagedItemsStockViewIssueView_Properties
        obj.p_txtItemCode_NUM = txtItemCode_NUM.Text
        obj.p_txtItemName = txtItemName.Text
        obj.p_cmbSubItemName = cmbSubItemName.Text
        obj.p_txtTagNo = txtTagNo.Text
        obj.p_cmbDesigner = cmbDesigner.Text
        obj.p_cmbCounterName = cmbCounterName.Text
        obj.p_cmbItemType = cmbItemType.Text
        obj.p_cmbCostCenter = cmbCostCenter.Text
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        obj.p_txtLotNo_NUM = txtLotNo_NUM.Text
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
        obj.p_chkWithCumulative = chkWithCumulative.Checked
        obj.p_cmbGroup = cmbGroup.Text
        obj.p_chkExtraWt = chkExtraWt.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmTagedItemsStockViewIssueView_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTagedItemsStockViewIssueView_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTagedItemsStockViewIssueView_Properties))
        SetChecked_CheckedList(chkCmbMetal, obj.p_chkCmbMetal, "ALL")
        txtItemCode_NUM.Text = obj.p_txtItemCode_NUM
        txtItemName.Text = obj.p_txtItemName
        cmbSubItemName.Text = obj.p_cmbSubItemName
        txtTagNo.Text = obj.p_txtTagNo
        cmbDesigner.Text = obj.p_cmbDesigner
        cmbCounterName.Text = obj.p_cmbCounterName
        cmbItemType.Text = obj.p_cmbItemType
        'cmbCostCenter.Text = obj.p_cmbCostCenter
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        txtLotNo_NUM.Text = obj.p_txtLotNo_NUM
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
        chkWithCumulative.Checked = obj.p_chkWithCumulative
        cmbGroup.Text = obj.p_cmbGroup
        chkExtraWt.Checked = obj.p_chkExtraWt
    End Sub

    Private Sub chkLotIssue_CheckedChanged(sender As Object, e As EventArgs) Handles chkLotIssue.CheckedChanged
        If chkLotIssue.Checked = True Then
            chkLotIssue.Text = "LotIssue From"
        Else
            chkLotIssue.Text = "Lot No From"
        End If
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


End Class

Public Class frmTagedItemsStockViewIssueView_Properties

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
    Private cmbDesigner As String = "ALL"
    Public Property p_cmbDesigner() As String
        Get
            Return cmbDesigner
        End Get
        Set(ByVal value As String)
            cmbDesigner = value
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
    Private chkExtraWt As New Boolean
    Public Property p_chkExtraWt() As Boolean
        Get
            Return chkExtraWt
        End Get
        Set(ByVal value As Boolean)
            chkExtraWt = value
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
    Private chkWithCumulative As Boolean = True
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