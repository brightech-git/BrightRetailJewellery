Imports System.Data.OleDb
Imports System.IO
Public Class frmSetItemRpt
    'LAST MODIFIED ON 20/08/2015-Removed Grouper Total and add it in Query Itself coz slow in for Loop Done by-Jegan
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim objMoreOption As New frmcheckothermaster
    Dim strSql As String
    Dim tempDt As New DataTable("OtherDetails")
    Dim dtStoneDetails As New DataTable("StoneDetails")
    Dim dtGrandTotalDetails As New DataTable("GrandTotal")
    Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")
    Dim headerBgColor As New System.Drawing.Color
    Dim DiaRnd As Integer = Val(GetAdmindbSoftValue("ROUNDOFF-DIA", 4))
    Dim dtCompany As New DataTable
    Dim dtMetal As New DataTable
    Dim RowFillState As Boolean = False
    Dim objSearch As Object = Nothing
    Dim Studded As Boolean = False
    Dim OTHmaster As Boolean = False
    Dim _TagDupPrint As Boolean = IIf(GetAdmindbSoftValue("TAGCHKDUPPRINT", "N") = "Y", True, False)

    Private Sub DetailedReport()
        btnView_Search.Enabled = False
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPITEMTAGTITLE')> 0"
        strSql += vbCrLf + "     DROP TABLE TEMPITEMTAGTITLE"
        strSql += vbCrLf + " SELECT 'TAG STOCK VIEW AS ON " & dtpAsOnDate.Text & "' AS TITLE INTO TEMPITEMTAGTITLE "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPITEMTAGSTOCK')> 0"
        strSql += vbCrLf + " DROP TABLE TEMPITEMTAGSTOCK"
        strSql += vbCrLf + " DECLARE @DEFPATH VARCHAR(200)"
        strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
        strSql += vbCrLf + " SELECT SNO"
        strSql += vbCrLf + " ,CASE WHEN SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)"
        strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) END AS PARTICULAR"
        strSql += vbCrLf + " ,RECDATE,TAGNO,PCS"
        strSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID ) AS PURITY"
        strSql += vbCrLf + " ,GRSWT,NETWT,RATE,MAXWAST,MAXMC,SALVALUE"
        strSql += vbCrLf + " ,(SELECT CASE WHEN ISNULL(SEAL,'') <> '' THEN SEAL ELSE DESIGNERNAME END FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,PCTFILE AS PCTFILE,NARRATION,TAGVAL,STYLENO"
        strSql += vbCrLf + " INTO TEMPITEMTAGSTOCK"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += funcFiltrationString()
        'Order By Detailed Report
        Dim filter As String = " ORDER BY "
        For i As Integer = 0 To LstOrderby.Items.Count - 1
            filter += IIf(LstOrderby.Items(i).ToString = "TAGNO", "TAGVAL", LstOrderby.Items(i).ToString)
            If i < LstOrderby.Items.Count - 1 Then filter += ","
        Next
        'If cmbOrderBy.Text = "WEIGHT" Then
        '    strSql += vbCrLf + " ORDER BY GRSWT,RECDATE,PARTICULAR"
        'ElseIf cmbOrderBy.Text = "RECDATE" Then
        '    strSql += vbCrLf + " ORDER BY RECDATE,GRSWT,TAGVAL,PARTICULAR"
        'Else
        '    strSql += vbCrLf + " ORDER BY TAGVAL,RECDATE,PARTICULAR,GRSWT"
        'End If
        strSql += filter
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If chkWithCumulative.Checked = True Then
            strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPITEMTAGSTOCK1')> 0"
            strSql += vbCrLf + " DROP TABLE TEMPITEMTAGSTOCK1"
            strSql += vbCrLf + " DECLARE @DEFPATH VARCHAR(200)"
            strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
            strSql += vbCrLf + " SELECT SNO"
            strSql += vbCrLf + " ,CASE WHEN SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)"
            strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) END AS PARTICULAR"
            strSql += vbCrLf + " ,RECDATE,TAGNO,PCS"
            strSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID ) AS PURITY"
            strSql += vbCrLf + " ,GRSWT,NETWT,RATE,MAXWAST,MAXMC,SALVALUE"
            strSql += vbCrLf + " ,(SELECT CASE WHEN ISNULL(SEAL,'') <> '' THEN SEAL ELSE DESIGNERNAME END FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
            strSql += vbCrLf + " ,PCTFILE AS PCTFILE,NARRATION,TAGVAL,STYLENO"
            strSql += vbCrLf + " INTO TEMPITEMTAGSTOCK1"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..CITEMTAG AS T"
            strSql += funcFiltrationString()
            'Order By Detailed Report
            filter = " ORDER BY "
            For i As Integer = 0 To LstOrderby.Items.Count - 1
                filter += IIf(LstOrderby.Items(i).ToString = "TAGNO", "TAGVAL", LstOrderby.Items(i).ToString)
                If i < LstOrderby.Items.Count - 1 Then filter += ","
            Next
            'If cmbOrderBy.Text = "WEIGHT" Then
            '    strSql += vbCrLf + " ORDER BY GRSWT,RECDATE,PARTICULAR"
            'ElseIf cmbOrderBy.Text = "RECDATE" Then
            '    strSql += vbCrLf + " ORDER BY RECDATE,GRSWT,TAGVAL,PARTICULAR"
            'Else
            '    strSql += vbCrLf + " ORDER BY TAGVAL,RECDATE,PARTICULAR,GRSWT"
            'End If
            strSql += filter
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = " INSERT INTO TEMPITEMTAGSTOCK"
            strSql += vbCrLf + " SELECT * FROM TEMPITEMTAGSTOCK1"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        strSql = " ALTER TABLE TEMPITEMTAGSTOCK ADD TAGIMAGE IMAGE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPITEMTAGSTONESTOCK')> 0"
        strSql += vbCrLf + "    DROP TABLE TEMPITEMTAGSTONESTOCK"
        strSql += vbCrLf + " SELECT TAGSNO,"
        strSql += vbCrLf + " CASE WHEN STNSUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.STNSUBITEMID)"
        strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID) END AS PARTICULAR"
        strSql += vbCrLf + " ,STNPCS,STNWT,STNRATE,STNAMT,CASE WHEN CALCMODE = 'W' THEN 'WIGHT' ELSE 'PIECE' END CALCMODE"
        strSql += vbCrLf + " ,CASE WHEN STONEUNIT = 'C' THEN 'CARAT' ELSE 'GRAM' END STONEUNIT,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID)AS STNTYPE"
        strSql += vbCrLf + " ,(SELECT TOP 1 SIZEDESC FROM " & cnAdminDb & "..CENTSIZE WHERE CONVERT(NUMERIC(15,4),(CASE WHEN STNPCS > 0 THEN STNWT/STNPCS ELSE STNWT END)*100) BETWEEN FROMCENT AND TOCENT)SIZEDESC"
        strSql += vbCrLf + " INTO TEMPITEMTAGSTONESTOCK"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T"
        strSql += vbCrLf + " WHERE TAGSNO IN (SELECT SNO FROM TEMPITEMTAGSTOCK)"
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
            Dim fileDestPath As String = defaultPic & ro!PCTFILE.ToString
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
        strSql = " SELECT " + vbCrLf
        strSql += vbCrLf + " CASE WHEN DPCS <> 0 THEN DPCS ELSE NULL END AS DPCS" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN DCARAT <> 0 THEN DCARAT ELSE NULL END AS DCARAT" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN DGRAM <> 0 THEN DGRAM ELSE NULL END AS DGRAM" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN SPCS <> 0 THEN SPCS ELSE NULL END AS SPCS" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN SCARAT <> 0 THEN SCARAT ELSE NULL END AS SCARAT" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN SGRAM <> 0 THEN SGRAM ELSE NULL END AS SGRAM" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN PPCS <> 0 THEN PPCS ELSE NULL END AS PPCS" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN PCARAT <> 0 THEN PCARAT ELSE NULL END AS PCARAT" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN PGRAM <> 0 THEN PGRAM ELSE NULL END AS PGRAM" + vbCrLf
        strSql += vbCrLf + " FROM " + vbCrLf
        strSql += vbCrLf + " (" + vbCrLf
        strSql += vbCrLf + " SELECT" + vbCrLf
        strSql += vbCrLf + " ISNULL(SUM(CASE WHEN STNTYPE = 'D' THEN STNPCS END),0) DPCS," + vbCrLf
        strSql += vbCrLf + " ISNULL(SUM(CASE WHEN stntype = 'D' AND STONEUNIT = 'C' THEN STNWT END),0)DCARAT," + vbCrLf
        strSql += vbCrLf + " ISNULL(SUM(CASE WHEN stntype = 'D' AND STONEUNIT = 'G' THEN STNWT END),0)DGRAM," + vbCrLf
        strSql += vbCrLf + " ISNULL(SUM(CASE WHEN stntype = 'S' THEN STNPCS END),0) SPCS," + vbCrLf
        strSql += vbCrLf + " ISNULL(SUM(CASE WHEN stntype = 'S' AND STONEUNIT = 'C' THEN STNWT END),0)SCARAT," + vbCrLf
        strSql += vbCrLf + " ISNULL(SUM(CASE WHEN stntype = 'S' AND STONEUNIT = 'G' THEN STNWT END),0)SGRAM," + vbCrLf
        strSql += vbCrLf + " ISNULL(SUM(CASE WHEN stntype = 'P' THEN STNPCS END),0) PPCS," + vbCrLf
        strSql += vbCrLf + " ISNULL(SUM(CASE WHEN stntype = 'P' AND STONEUNIT = 'C' THEN STNWT END),0)PCARAT," + vbCrLf
        strSql += vbCrLf + " ISNULL(SUM(CASE WHEN stntype = 'P' AND STONEUNIT = 'G' THEN STNWT END),0)PGRAM" + vbCrLf
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT STNPCS,STONEUNIT,STNWT,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID)AS STNTYPE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE S" + vbCrLf
        strSql += vbCrLf + " WHERE TAGSNO IN (SELECT SNO FROM TEMP" & systemId & "TAGSTOCKVIEW)" + vbCrLf
        strSql += vbCrLf + " )Y"
        strSql += vbCrLf + " )X" + vbCrLf
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
        ro("ExtraWt") = ExtraWt
        ro("LessWt") = lessWt
        ro("NetWt") = netWt
        ro("SalValue") = salValue
        dtGrandTotalDetails.Rows.Add(ro)
    End Function

    Function funcFillGrandDetails() As Integer
        dtGrandTotalDetails.Clear()
        strSql = " IF OBJECT_ID('TEMP" & systemId & "GRANDTOTTAGSTOCKVIEW','U') IS NOT NULL DROP TABLE TEMP" & systemId & "GRANDTOTTAGSTOCKVIEW"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " SELECT"
        strSql += vbCrLf + " CASE WHEN SUM(PCS) <> 0 THEN SUM(PCS) ELSE NULL END PCS,"
        strSql += vbCrLf + " CASE WHEN SUM(GRSWT) <> 0 THEN SUM(GRSWT) ELSE NULL END GRSWT,"
        strSql += vbCrLf + " CASE WHEN SUM(LESSWT) <> 0 THEN SUM(LESSWT) ELSE NULL END LESSWT,"
        strSql += vbCrLf + " CASE WHEN SUM(NETWT) <> 0 THEN SUM(NETWT) ELSE NULL END NETWT,"
        strSql += vbCrLf + " CASE WHEN SUM(EXTRAWT) <> 0 THEN SUM(EXTRAWT) ELSE NULL END EXTRAWT,"
        strSql += vbCrLf + " CASE WHEN SUM(SALVALUE) <> 0 THEN SUM(SALVALUE) ELSE NULL END SALVALUE"
        strSql += vbCrLf + " INTO TEMP" & systemId & "GRANDTOTTAGSTOCKVIEW FROM " & cnAdminDb & "..ITEMTAG T"
        strSql += funcFiltrationString()
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If chkWithCumulative.Checked = True Then
            strSql = " IF OBJECT_ID('TEMP" & systemId & "GRANDTOTTAGSTOCKVIEW1','U') IS NOT NULL DROP TABLE TEMP" & systemId & "GRANDTOTTAGSTOCKVIEW1"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = " SELECT"
            strSql += vbCrLf + " CASE WHEN SUM(PCS) <> 0 THEN SUM(PCS) ELSE NULL END PCS,"
            strSql += vbCrLf + " CASE WHEN SUM(GRSWT) <> 0 THEN SUM(GRSWT) ELSE NULL END GRSWT,"
            strSql += vbCrLf + " CASE WHEN SUM(LESSWT) <> 0 THEN SUM(LESSWT) ELSE NULL END LESSWT,"
            strSql += vbCrLf + " CASE WHEN SUM(NETWT) <> 0 THEN SUM(NETWT) ELSE NULL END NETWT,"
            strSql += vbCrLf + " CASE WHEN SUM(EXTRAWT) <> 0 THEN SUM(EXTRAWT) ELSE NULL END EXTRAWT,"
            strSql += vbCrLf + " CASE WHEN SUM(SALVALUE) <> 0 THEN SUM(SALVALUE) ELSE NULL END SALVALUE"
            strSql += vbCrLf + " INTO TEMP" & systemId & "GRANDTOTTAGSTOCKVIEW1 FROM " & cnAdminDb & "..CITEMTAG T"
            strSql += funcFiltrationString()
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = " INSERT INTO  TEMP" & systemId & "GRANDTOTTAGSTOCKVIEW"
            strSql += vbCrLf + " SELECT * FROM TEMP" & systemId & "GRANDTOTTAGSTOCKVIEW1"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        strSql = " SELECT SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(LESSWT)LESSWT,SUM(NETWT)NETWT,SUM(EXTRAWT)EXTRAWT,SUM(SALVALUE)SALVALUE  FROM TEMP" & systemId & "GRANDTOTTAGSTOCKVIEW"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            gridGrandTotal.DataSource = dtGrandTotalDetails
            tabView.Show()
            'FillGridGroupStyle(gridGrandTotal)
            Exit Function
        End If
        With dt.Rows(0)
            funcAddGrandDetails("", "Piece(s)", "Grs_Wt", "Less_Wt", "Net_Wt", "Extra_Wt", "Sal_Val")
            funcAddGrandDetails("Cursor Total", "", "", "", "", "", "")
            funcAddGrandDetails("", "", "", "", "", "", "")
            funcAddGrandDetails("Grand Total", .Item("Pcs").ToString, .Item("GrsWt").ToString, .Item("LessWt").ToString, .Item("NetWt").ToString, .Item("ExtraWt").ToString, .Item("SalValue").ToString)
        End With
        gridGrandTotal.DataSource = dtGrandTotalDetails
        tabView.Show()
        'FillGridGroupStyle(gridGrandTotal)
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
        If rowIndex >= 0 Then
            Dim dtCur As New DataTable
            dtCur = CType(gridTotalView.DataSource, DataTable)
            Dim KeyNo As Integer = gridTotalView.Item("KEYNO", rowIndex).Value
            pcs = Val(dtCur.Compute("SUM(PCS)", "RESULT=1 AND KEYNO <=" & KeyNo).ToString)
            GrsWt = Val(dtCur.Compute("SUM(GRSWT)", "RESULT=1 AND KEYNO <=" & KeyNo).ToString)
            lessWt = Val(dtCur.Compute("SUM(LESSWT)", "RESULT=1 AND KEYNO <=" & KeyNo).ToString)
            netWt = Val(dtCur.Compute("SUM(NETWT)", "RESULT=1 AND KEYNO <=" & KeyNo).ToString)
            extraWt = Val(dtCur.Compute("SUM(EXTRAWT)", "RESULT=1 AND KEYNO <=" & KeyNo).ToString)
            salValue = Val(dtCur.Compute("SUM(SALVALUE)", "RESULT=1 AND KEYNO <=" & KeyNo).ToString)
        End If
        'For cnt As Integer = 0 To rowIndex
        '    If gridTotalView.Item("RESULT", cnt).Value.ToString.Trim <> "1" Then Continue For
        '    pcs += Val(gridTotalView.Item("Pcs", cnt).Value.ToString)
        '    GrsWt += Val(gridTotalView.Item("GrsWt", cnt).Value.ToString)
        '    lessWt += Val(gridTotalView.Item("lessWt", cnt).Value.ToString)
        '    netWt += Val(gridTotalView.Item("netWt", cnt).Value.ToString)
        '    extraWt += Val(gridTotalView.Item("extraWt", cnt).Value.ToString)
        '    salValue += Val(gridTotalView.Item("salValue", cnt).Value.ToString)
        'Next
        If gridGrandTotal.Rows.Count > 1 Then
            gridGrandTotal.Item("Pcs", 1).Value = pcs
            gridGrandTotal.Item("GrsWt", 1).Value = Format(GrsWt, "0.000")
            gridGrandTotal.Item("lessWt", 1).Value = Format(lessWt, "0.000")
            gridGrandTotal.Item("netWt", 1).Value = Format(netWt, "0.000")
            gridGrandTotal.Item("extraWt", 1).Value = Format(extraWt, "0.000")
            gridGrandTotal.Item("salValue", 1).Value = Format(salValue, "0.00")
        End If
    End Function

    Function funcStoneFiltrationString(Optional ByVal cTag As Boolean = False) As String
        Dim str As String = Nothing
        Dim agetodate As Integer
        agetodate = Val(txtageto.Text.ToString) * -1
        If chkactualdate.Checked Then
            If chkage.Checked Then
                str = " WHERE DATEDIFF(DD,ISNULL(ACTUALRECDATE,RECDATE),'" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "') BETWEEN " & Val(txtagefrm.Text.ToString) & " AND " & Val(txtageto.Text.ToString) & ""
            Else
                str = " where ACTUALRECDATE <= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' "
            End If
        Else
            If chkage.Checked Then
                str = " WHERE DATEDIFF(DD,RECDATE,'" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "') BETWEEN " & Val(txtagefrm.Text.ToString) & " AND " & Val(txtageto.Text.ToString) & ""
            Else
                str = " where RecDate <= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' "
            End If
        End If
        '(" & GetQryString(chkmiscname.Text) & "))


        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            str += " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else

            'str += " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        'strSql += " and companyid = '" & GetStockCompId() & "'"
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            str += " and t.itemid in (select itemid from " & cnAdminDb & "..itemmast where METALID in (select Metalid from " & cnAdminDb & "..MetalMast where MetalName IN (" & GetQryString(chkCmbMetal.Text) & ")))"
        End If
        If txtItemCode_NUM.Text <> "" Then
            str += " and t.itemId = '" & txtItemCode_NUM.Text & "'"
        End If
        If cmbSubItemName.Enabled = True Then
            If cmbSubItemName.Text <> "ALL" And cmbSubItemName.Text <> "" Then
                str += " and t.subItemId IN (select subItemId from " & cnAdminDb & "..SubItemMast where subItemName IN (" & GetQryString(cmbSubItemName.Text) & ") and itemid = " & Val(txtItemCode_NUM.Text) & ")"
            End If
        End If
        If txtTagNo.Text <> "" Then
            str += " and t.tagno = '" & txtTagNo.Text & "'"
        End If
        'If chkage.Checked Then
        '    str += " and (issdate is null or issDate > DATEADD(DAY," & agetodate & ",'" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "'))"
        'Else
        str += " and (issdate is null or issDate > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
        'End If

        If cmbDesigner.Text <> "ALL" Then
            str += " and DesignerId = (Select DesignerId from " & cnAdminDb & "..Designer where DesignerName = '" & cmbDesigner.Text & "')"
        End If
        If cmbCounterName.Text <> "ALL" And cmbCounterName.Text <> "" Then
            str += " and ItemCtrId IN (Select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName IN (" & GetQryString(cmbCounterName.Text) & "))"
        End If
        If cmbItemType.Text <> "ALL" Then
            str += " and itemTypeId = (select itemTypeId from " & cnAdminDb & "..itemType where Name = '" & cmbItemType.Text & "')"
        End If
        If cmbCostCenter.Text <> "ALL" And cmbCostCenter.Text <> "" Then
            str += " and COSTID = (select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCenter.Text & "')"
        End If

        If txtLotNo_NUM.Text <> "" Then
            'str += " AND LOTSNO = '" & txtLotNo_NUM.Text & "'"
            str += " and LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = '" & txtLotNo_NUM.Text & "')"
        End If
        If txtFromWt_WET.Text <> "" Or txtToWt_WET.Text <> "" Then
            str += " and (T.GrsWt between '" & Val(txtFromWt_WET.Text) & "' and '" & Val(txtToWt_WET.Text) & "')"
        End If
        If txtFromDiaWt_WET.Text <> "" Or txtToDiaWt_WET.Text <> "" Then
            str += " and ((select sum(stnwt) from " & cnAdminDb & ".." & IIf(cTag, "CITEMTAGSTONE", "ITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) between '" & Val(txtFromDiaWt_WET.Text) & "' and '" & Val(txtToDiaWt_WET.Text) & "')"
        End If
        If txtFromRate_WET.Text <> "" Or txtToRate_WET.Text <> "" Then
            str += " and (SalValue between '" & Val(txtFromRate_WET.Text) & "' and '" & Val(txtToRate_WET.Text) & "')"
        End If
        If rbtRegular.Checked = True Then
            str += " and ordRepNo =''"
        End If
        If rbtOrder.Checked = True Then
            str += " and ordRepNo <> ''"
        End If
        If cmbSize.Text <> "ALL" Then
            str += " and SIZEID IN (select SIZEID from " & cnAdminDb & "..ITEMSIZE where SIZEName = '" & cmbSize.Text & "')"
        End If
        'If Not chkApproval.Checked Then
        '    str += " AND T.TAGNO IN (SELECT TAGNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE < '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' AND TRANTYPE = 'AI' AND (REFDATE IS NULL OR REFDATE>='" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "') "
        '    str += " OR T.TAGNO IN (SELECT TAGNO FROM " & cnStockDb & "..APPISSUE WHERE TRANDATE < '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' AND TRANTYPE = 'AI'  AND (REFDATE IS NULL OR REFDATE>='" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "'))) "
        'End If

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

        If txtSearch_txt.Text <> "" And cmbSearchKey.Text <> "" Then
            str += " AND T." & cmbSearchKey.Text & " LIKE"
            str += " '" & txtSearch_txt.Text & "%'"
        End If

        If txtPcs_NUM.Text <> "" Then
            str += " AND T.PCS ="
            str += " '" & txtPcs_NUM.Text & "'"
        End If

        If OTHmaster = True Then
            If objMoreOption.chkothermisc.Text <> "" Then
                str += vbCrLf + " AND T.SNO IN (SELECT TAGSNO FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE  OTHID IN(SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME IN (" & GetQryString(objMoreOption.chkothermisc.Text) & ")))"
            End If
        End If

        Return str
    End Function
    Function funcFiltrationString(Optional ByVal cTag As Boolean = False) As String
        Dim str As String = Nothing
        Dim agetodate As Integer
        agetodate = Val(txtageto.Text.ToString) * -1
        If chkactualdate.Checked Then
            If chkage.Checked Then
                str = " WHERE DATEDIFF(DD,ISNULL(ACTUALRECDATE,RECDATE),'" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "') BETWEEN " & Val(txtagefrm.Text.ToString) & " AND " & Val(txtageto.Text.ToString) & ""
            Else
                str = " WHERE ACTUALRECDATE <= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' "
            End If
        Else
            If chkage.Checked Then
                str = " WHERE DATEDIFF(DD,RECDATE,'" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "') BETWEEN " & Val(txtagefrm.Text.ToString) & " AND " & Val(txtageto.Text.ToString) & ""
            Else
                str = " where RecDate <= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' "
            End If
        End If
        '(" & GetQryString(chkmiscname.Text) & "))


        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            str += " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            'str += " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        'strSql += " and companyid = '" & GetStockCompId() & "'"
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            str += " and t.itemid in (select itemid from " & cnAdminDb & "..itemmast where METALID in (select Metalid from " & cnAdminDb & "..MetalMast where MetalName IN (" & GetQryString(chkCmbMetal.Text) & ")))"
        End If

        If txtItemCode_NUM.Text <> "" Then
            str += " and t.itemId = '" & txtItemCode_NUM.Text & "'"
        End If
        If cmbSubItemName.Enabled = True Then
            If cmbSubItemName.Text <> "ALL" And cmbSubItemName.Text <> "" Then
                str += " and t.subItemId IN (select subItemId from " & cnAdminDb & "..SubItemMast where subItemName IN (" & GetQryString(cmbSubItemName.Text) & ") and itemid = " & Val(txtItemCode_NUM.Text) & ")"
            End If
        End If
        If txtTagNo.Text <> "" Then
            str += " and t.tagno = '" & txtTagNo.Text & "'"
        End If
        'If chkage.Checked Then
        '    str += " and (issdate is null or issDate > DATEADD(DAY," & agetodate & ",'" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "'))"
        'Else
        str += " and (issdate is null or issDate > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
        'End If

        If cmbDesigner.Text <> "ALL" Then
            str += " and DesignerId = (Select DesignerId from " & cnAdminDb & "..Designer where DesignerName = '" & cmbDesigner.Text & "')"
        End If
        If cmbCounterName.Text <> "ALL" And cmbCounterName.Text <> "" Then
            str += " and ItemCtrId IN (Select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName IN (" & GetQryString(cmbCounterName.Text) & "))"
        End If
        If cmbItemType.Text <> "ALL" Then
            str += " and itemTypeId = (select itemTypeId from " & cnAdminDb & "..itemType where Name = '" & cmbItemType.Text & "')"
        End If
        If cmbCostCenter.Text <> "ALL" And cmbCostCenter.Text <> "" Then
            str += " and COSTID = (select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCenter.Text & "')"
        End If

        If txtLotNo_NUM.Text <> "" Then
            'str += " AND LOTSNO = '" & txtLotNo_NUM.Text & "'"
            str += " and LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = '" & txtLotNo_NUM.Text & "')"
        End If
        If txtFromWt_WET.Text <> "" Or txtToWt_WET.Text <> "" Then
            str += " and (T.GrsWt between '" & Val(txtFromWt_WET.Text) & "' and '" & Val(txtToWt_WET.Text) & "')"
        End If
        If txtFromDiaWt_WET.Text <> "" Or txtToDiaWt_WET.Text <> "" Then
            str += " and ((select sum(stnwt) from " & cnAdminDb & ".." & IIf(cTag, "CITEMTAGSTONE", "ITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) between '" & Val(txtFromDiaWt_WET.Text) & "' and '" & Val(txtToDiaWt_WET.Text) & "')"
        End If
        If txtFromRate_WET.Text <> "" Or txtToRate_WET.Text <> "" Then
            str += " and (SalValue between '" & Val(txtFromRate_WET.Text) & "' and '" & Val(txtToRate_WET.Text) & "')"
        End If
        If rbtRegular.Checked = True Then
            str += " and ordRepNo =''"
        End If
        If rbtOrder.Checked = True Then
            str += " and ordRepNo <> ''"
        End If
        If cmbSize.Text <> "ALL" Then
            str += " and SIZEID IN (select SIZEID from " & cnAdminDb & "..ITEMSIZE where SIZEName = '" & cmbSize.Text & "')"
        End If
        If Not chkApproval.Checked Then
            str += " AND ISNULL(APPROVAL,'') = ''"
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

        If txtSearch_txt.Text <> "" And cmbSearchKey.Text <> "" Then
            str += " AND T." & cmbSearchKey.Text & " LIKE"
            str += " '" & txtSearch_txt.Text & "%'"
        End If

        If txtPcs_NUM.Text <> "" Then
            str += " AND T.PCS ="
            str += " '" & txtPcs_NUM.Text & "'"
        End If

        If OTHmaster = True Then
            If objMoreOption.chkothermisc.Text <> "" Then
                str += vbCrLf + " AND T.SNO IN (SELECT TAGSNO FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE  OTHID IN(SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME IN (" & GetQryString(objMoreOption.chkothermisc.Text) & ")))"
            End If
        End If
        str += " AND (ISNULL(T.SETTAGNO,'')<>'') /*OR T.TAGNO IN (SELECT DISTINCT SETTAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE ISNULL(SETTAGNO,'')<>''))*/"
        Return str
    End Function

    Private Sub frmTagedItemsStockView_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPITEMTAGTITLE')> 0"
        strSql += vbCrLf + "     DROP TABLE TEMPITEMTAGTITLE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPITEMTAGSTOCK')> 0"
        strSql += vbCrLf + "     DROP TABLE TEMPITEMTAGSTOCK"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPITEMTAGSTONESTOCK')> 0"
        strSql += vbCrLf + "    DROP TABLE TEMPITEMTAGSTONESTOCK"
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
        If Not defaultPic.EndsWith("\") Then defaultPic += "\"

        'Me.WindowState = FormWindowState.Maximized
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
        If dt.Rows.Count > 0 Then
            cmbCostCenter.Enabled = True
        Else
            cmbCostCenter.Enabled = False
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
        chkage.Checked = False
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
        If chkDetailed.Checked Then
            DetailedReport()
            Exit Sub
        End If
        Dim dt As New DataTable("StockView")
        dt.Clear()
        btnView_Search.Enabled = False
        Try
            If chkage.Checked Then
                If Val(Trim(txtagefrm.Text)) > Val(Trim(txtageto.Text)) Then
                    MsgBox("Pls Give Valid AgeDate.", MsgBoxStyle.OkOnly, "Brighttech Information")
                    Exit Sub
                End If
            End If
            tabView.Show()
            OTHmaster = False
            Dim dtOth As New DataTable
            'If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE ISNULL(ACTIVE,'')<>'N'", , "", )) > 0 Then
            '    strSql = "SELECT * FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE ISNULL(ACTIVE,'Y')<>'N'"
            '    da = New OleDbDataAdapter(strSql, cn)
            '    da.Fill(dtOth)
            '    If objMoreOption.Visible Then Exit Sub
            '    OTHmaster = True
            '    objMoreOption.BackColor = Me.BackColor
            '    objMoreOption.StartPosition = FormStartPosition.CenterScreen
            '    objMoreOption.MaximizeBox = False
            '    objMoreOption.ShowDialog()
            '    btnView_Search.Focus()
            'End If

            strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "TAGSTOCKVIEW')>0"
            strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "TAGSTOCKVIEW"
            strSql += vbCrLf + " DECLARE @DEFPATH VARCHAR(200)"
            strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
            strSql += vbCrLf + " SELECT CONVERT(INT,NULL)SLNO,CONVERT(VARCHAR(15),SNO)SNO,"
            strSql += vbCrLf + " CASE WHEN T.SUBITEMID > 0  THEN "
            strSql += vbCrLf + " (SELECT  top 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID AND ITEMID = T.ITEMID)"
            strSql += vbCrLf + " ELSE (SELECT  top 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) END AS PARTICULAR,"
            strSql += vbCrLf + " " & IIf(chkactualdate.Checked, "ACTUALRECDATE", "RECDATE") & " RECDATE,"
            strSql += vbCrLf + " SETTAGNO,TAGNO,CASE WHEN PCS <> 0 THEN PCS ELSE NULL END PCS,CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END GRSWT,CASE WHEN LESSWT <> 0 THEN LESSWT ELSE NULL END LESSWT,"
            strSql += vbCrLf + " CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END NETWT,"
            strSql += vbCrLf + " CASE WHEN EXTRAWT <> 0 THEN EXTRAWT ELSE NULL END EXTRAWT,"
            strSql += vbCrLf + " CASE WHEN TOUCH <> 0 THEN TOUCH ELSE NULL END TOUCH,GRSNET,CASE WHEN RATE <> 0 THEN RATE ELSE NULL END RATE"
            strSql += vbCrLf + " ,CASE WHEN SALVALUE <> 0 THEN SALVALUE ELSE NULL END SALVALUE"
            strSql += vbCrLf + " ,(SELECT  top 1 PURVALUE FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGSNO = T.SNO)AS PURVALUE,"
            strSql += vbCrLf + " (SELECT ISNULL(SUM(ISNULL(STNPCS,0)),0) SPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNPCS ,"
            'Stone
            strSql += vbCrLf + " (SELECT SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT ELSE 0 END) FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWTG ,"
            strSql += vbCrLf + " (SELECT SUM(CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT ELSE 0 END) FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWTC ,"
            strSql += vbCrLf + " (SELECT SUM(S.STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNAMT ,"
            'Diamond
            strSql += vbCrLf + " (SELECT ISNULL(SUM(ISNULL(S.STNPCS,0)),0) DPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAPCS ,"

            strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT "

            strSql += vbCrLf + " ,(SELECT ISNULL(SUM(STNAMT),0) DAMT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE  S.ITEMID = T.ITEMID "
            strSql += vbCrLf + " AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAAMT ,"
            'Presious
            strSql += vbCrLf + " (SELECT ISNULL(SUM(ISNULL(STNPCS,0)),0) PPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREPCS ,"

            strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) PWT "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))) PREWT,"

            strSql += vbCrLf + " (SELECT SUM(S.STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
            strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREAMT ,"


            strSql += " CASE WHEN MAXWASTPER <> 0 THEN MAXWASTPER ELSE NULL END AS [WAST%],CASE WHEN MAXWAST <> 0 THEN MAXWAST ELSE NULL END [WAST],"
            strSql += " CASE WHEN MAXMCGRM <> 0 THEN MAXMCGRM ELSE NULL END AS [MCGRM],CASE WHEN MAXMC <> 0 THEN MAXMC ELSE NULL END [MC],"

            strSql += vbCrLf + " (SELECT top 1 NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)TAGTYPE,"
            strSql += vbCrLf + " (SELECT  top 1 ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)COUNTER,"
            strSql += vbCrLf + " TABLECODE,CASE WHEN PURITY <> 0 THEN PURITY ELSE NULL END PURITY,"
            strSql += vbCrLf + " RTRIM(LTRIM(NARRATION))NARRATION,BRANDID,"
            If dtOth.Rows.Count > 0 Then
                For O As Integer = 0 To dtOth.Rows.Count - 1
                    strSql += vbCrLf + " (SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID=" & Val(dtOth.Rows(O).Item("MISCID").ToString) & ""
                    strSql += vbCrLf + " AND ID=(SELECT TOP 1 OTHID FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE TAGSNO=T.SNO)) AS """ & dtOth.Rows(O).Item("MISCNAME") & ""","
                Next
            End If
            strSql += vbCrLf + " SALEMODE,@DEFPATH + PCTFILE AS PCTFILE,TRANINVNO,SUPBILLNO,"
            strSql += vbCrLf + " (SELECT  top 1 DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
            strSql += vbCrLf + " ,ENTRYMODE,ITEMID,CONVERT(VARCHAR(1),'')COLHEAD,TAGVAL"
            strSql += vbCrLf + " ,(SELECT  top 1 SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)AS SIZENAME"
            strSql += vbCrLf + " ,1 RESULT,T.STYLENO,CONVERT(VARCHAR(3),ISNULL(T.APPROVAL,''))APPROVAL"
            strSql += vbCrLf + " ,(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + " ,(SELECT  TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = T.ITEMID AND SUBITEMID = T.SUBITEMID)AS SUBITEM"
            strSql += vbCrLf + " ,(SELECT TOP 1 CAPTION FROM " & cnAdminDb & "..RANGEMAST AS R WHERE R.ITEMID=T.ITEMID AND R.SUBITEMID=T.SUBITEMID "
            strSql += vbCrLf + " AND T.GRSWT BETWEEN R.FROMWEIGHT AND R.TOWEIGHT)RANGE"
            strSql += vbCrLf + " INTO TEMP" & systemId & "TAGSTOCKVIEW "
            strSql += " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += funcFiltrationString()
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If chkWithCumulative.Checked = True Then
                strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "TAGSTOCKVIEW1')>0"
                strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "TAGSTOCKVIEW1"
                strSql += vbCrLf + " DECLARE @DEFPATH VARCHAR(200)"
                strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
                strSql += vbCrLf + " SELECT CONVERT(INT,NULL)SLNO,CONVERT(VARCHAR(15),SNO)SNO,"
                strSql += vbCrLf + " CASE WHEN T.SUBITEMID > 0  THEN "
                strSql += vbCrLf + " (SELECT  top 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID AND ITEMID = T.ITEMID)"
                strSql += vbCrLf + " ELSE (SELECT  top 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) END AS PARTICULAR,"
                strSql += vbCrLf + " " & IIf(chkactualdate.Checked, "ACTUALRECDATE", "RECDATE") & " RECDATE,"
                strSql += vbCrLf + " SETTAGNO,TAGNO,CASE WHEN PCS <> 0 THEN PCS ELSE NULL END PCS,CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END GRSWT,CASE WHEN LESSWT <> 0 THEN LESSWT ELSE NULL END LESSWT,"
                strSql += vbCrLf + " CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END NETWT,"
                strSql += vbCrLf + " CASE WHEN EXTRAWT <> 0 THEN EXTRAWT ELSE NULL END EXTRAWT,"
                strSql += vbCrLf + " CASE WHEN TOUCH <> 0 THEN TOUCH ELSE NULL END TOUCH,GRSNET,CASE WHEN RATE <> 0 THEN RATE ELSE NULL END RATE"
                strSql += vbCrLf + " ,CASE WHEN SALVALUE <> 0 THEN SALVALUE ELSE NULL END SALVALUE"
                strSql += vbCrLf + " ,(SELECT  top 1 PURVALUE FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGSNO = T.SNO)AS PURVALUE,"
                strSql += vbCrLf + " (SELECT ISNULL(SUM(ISNULL(STNPCS,0)),0) SPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNPCS ,"
                'Stone
                strSql += vbCrLf + " (SELECT SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT ELSE 0 END) FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWTG ,"
                strSql += vbCrLf + " (SELECT SUM(CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT ELSE 0 END) FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWTC ,"
                strSql += vbCrLf + " (SELECT SUM(S.STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNAMT ,"
                'Diamond
                strSql += vbCrLf + " (SELECT ISNULL(SUM(ISNULL(S.STNPCS,0)),0) DPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAPCS ,"

                strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) DWT "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) DIAWT "

                strSql += vbCrLf + " ,(SELECT ISNULL(SUM(STNAMT),0) DAMT FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE  S.ITEMID = T.ITEMID "
                strSql += vbCrLf + " AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAAMT ,"
                'Presious
                strSql += vbCrLf + " (SELECT ISNULL(SUM(ISNULL(STNPCS,0)),0) PPCS FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREPCS ,"

                strSql += vbCrLf + " CONVERT(NUMERIC(15," & DiaRnd & "),(SELECT ISNULL(SUM(CASE WHEN S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE (CASE WHEN S.STONEUNIT = 'C' THEN S.STNWT END) END),0) PWT "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))) PREWT,"

                strSql += vbCrLf + " (SELECT SUM(S.STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE "
                strSql += vbCrLf + " S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREAMT ,"


                strSql += " CASE WHEN MAXWASTPER <> 0 THEN MAXWASTPER ELSE NULL END AS [WAST%],CASE WHEN MAXWAST <> 0 THEN MAXWAST ELSE NULL END [WAST],"
                strSql += " CASE WHEN MAXMCGRM <> 0 THEN MAXMCGRM ELSE NULL END AS [MCGRM],CASE WHEN MAXMC <> 0 THEN MAXMC ELSE NULL END [MC],"

                strSql += vbCrLf + " (SELECT top 1 NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)TAGTYPE,"
                strSql += vbCrLf + " (SELECT  top 1 ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)COUNTER,"
                strSql += vbCrLf + " TABLECODE,CASE WHEN PURITY <> 0 THEN PURITY ELSE NULL END PURITY,"
                strSql += vbCrLf + " RTRIM(LTRIM(NARRATION))NARRATION,BRANDID,"
                If dtOth.Rows.Count > 0 Then
                    For O As Integer = 0 To dtOth.Rows.Count - 1
                        strSql += vbCrLf + " (SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID=" & Val(dtOth.Rows(O).Item("MISCID").ToString) & ""
                        strSql += vbCrLf + " AND ID=(SELECT TOP 1 OTHID FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE TAGSNO=T.SNO)) AS """ & dtOth.Rows(O).Item("MISCNAME") & ""","
                    Next
                End If
                strSql += vbCrLf + " SALEMODE,@DEFPATH + PCTFILE AS PCTFILE,TRANINVNO,SUPBILLNO,"
                strSql += vbCrLf + " (SELECT  top 1 DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
                strSql += vbCrLf + " ,ENTRYMODE,ITEMID,CONVERT(VARCHAR(1),'')COLHEAD,TAGVAL"
                strSql += vbCrLf + " ,(SELECT  top 1 SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)AS SIZENAME"
                strSql += vbCrLf + " ,1 RESULT,T.STYLENO,CONVERT(VARCHAR(3),ISNULL(T.APPROVAL,''))APPROVAL"
                strSql += vbCrLf + " ,(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
                strSql += vbCrLf + " ,(SELECT  TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = T.ITEMID AND SUBITEMID = T.SUBITEMID)AS SUBITEM"
                strSql += vbCrLf + " ,(SELECT TOP 1 CAPTION FROM " & cnAdminDb & "..RANGEMAST AS R WHERE R.ITEMID=T.ITEMID AND R.SUBITEMID=T.SUBITEMID "
                strSql += vbCrLf + " AND T.GRSWT BETWEEN R.FROMWEIGHT AND R.TOWEIGHT)RANGE"
                strSql += vbCrLf + " INTO TEMP" & systemId & "TAGSTOCKVIEW1 "
                strSql += " FROM " & cnAdminDb & "..CITEMTAG AS T"
                strSql += funcFiltrationString()
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                strSql = " INSERT INTO TEMP" & systemId & "TAGSTOCKVIEW"
                strSql += vbCrLf + "SELECT * FROM TEMP" & systemId & "TAGSTOCKVIEW1"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            gridTotalView.DataSource = Nothing
            Dim dtSource As New DataTable
            Dim filter As String = ""
            For i As Integer = 0 To LstOrderby.Items.Count - 1
                filter += "SETTAGNO," + IIf(LstOrderby.Items(i).ToString = "TAGNO", "TAGVAL", LstOrderby.Items(i).ToString)
                If i < LstOrderby.Items.Count - 1 Then filter += ","
            Next
            If filter <> "" Then
                filter = Replace(filter, "TRANINVOICENO", "TRANINVNO")
            End If
            If CmbGroupBy.Text = "" Then
                strSql = " IF OBJECT_ID('TEMP" & systemId & "TAGSTOCKVIEWFINAL','U') IS NOT NULL DROP TABLE TEMP" & systemId & "TAGSTOCKVIEWFINAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                'Row_number()
                strSql = " SELECT  ROW_NUMBER() OVER(ORDER BY RESULT"
                If filter <> "" Then
                    strSql += "," & filter
                End If
                strSql += ")KEYNO"
                strSql += ", * INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL "
                strSql += "FROM TEMP" & systemId & "TAGSTOCKVIEW ORDER BY RESULT "
                If filter <> "" Then
                    strSql += "," & filter
                End If
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,PCS, GRSWT, NETWT, SALVALUE, "
                strSql += "DIAPCS, DIAWT,PREPCS,PREWT,WAST,MC,"
                strSql += "RESULT, COLHEAD)"
                strSql += "SELECT 'GRAND TOTAL',SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(SALVALUE),"
                strSql += "SUM(DIAPCS), SUM(DIAWT), SUM(PREPCS), SUM(PREWT), SUM(WAST), SUM(MC),"
                strSql += "'999', 'G' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL "
                strSql += "WHERE RESULT = '1'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,TAGNO, RESULT,COLHEAD)"
                strSql += "SELECT 'TAG NO', COUNT(TAGNO), '9999','G'"
                strSql += "FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                strSql = "SELECT * FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL ORDER BY RESULT, KEYNO"

            ElseIf CmbGroupBy.Text = "DESIGNER" Then
                strSql = "UPDATE TEMP" & systemId & "TAGSTOCKVIEW SET DESIGNER='NO DESIGNER' WHERE DESIGNER IS NULL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " IF OBJECT_ID('TEMP" & systemId & "TAGSTOCKVIEWFINAL','U') IS NOT NULL DROP TABLE TEMP" & systemId & "TAGSTOCKVIEWFINAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " SELECT  ROW_NUMBER() OVER(ORDER BY  DESIGNER, RESULT"
                If filter <> "" Then
                    strSql += "," & filter
                End If
                strSql += ")KEYNO"
                strSql += ", * INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL "
                strSql += "FROM TEMP" & systemId & "TAGSTOCKVIEW"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR, DESIGNER, RESULT, COLHEAD) "
                strSql += "SELECT DISTINCT DESIGNER, DESIGNER, '0', 'T' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,PCS, GRSWT, NETWT, SALVALUE,"
                strSql += "DIAPCS, DIAWT,PREPCS,PREWT,WAST,MC,"
                strSql += "DESIGNER,"
                strSql += "RESULT, COLHEAD)"
                strSql += "SELECT 'SUB TOTAL', SUM(PCS), SUM(GRSWT), SUM(NETWT), SUM(SALVALUE), "
                strSql += "SUM(DIAPCS), SUM(DIAWT), SUM(PREPCS), SUM(PREWT), SUM(WAST), SUM(MC),"
                strSql += "DESIGNER,"
                strSql += "'2', 'S' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL  WHERE RESULT = '1'"
                strSql += "GROUP BY DESIGNER"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,PCS, GRSWT, NETWT, SALVALUE, "
                strSql += "DIAPCS, DIAWT,PREPCS,PREWT,WAST,MC,"
                strSql += "SUBITEM,DESIGNER,"
                strSql += "RESULT, COLHEAD)"
                strSql += "SELECT 'GRAND TOTAL',SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(SALVALUE),"
                strSql += "SUM(DIAPCS), SUM(DIAWT), SUM(PREPCS), SUM(PREWT), SUM(WAST), SUM(MC),"
                strSql += "'ZZZZ','ZZ',"
                strSql += "'999', 'G' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL "
                strSql += "WHERE RESULT = '2'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,TAGNO, SUBITEM, RESULT,COLHEAD,DESIGNER)"
                strSql += "SELECT 'TAG NO', COUNT(TAGNO), 'ZZZZ','9999','G','ZZ'"
                strSql += "FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "SELECT * FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL ORDER BY DESIGNER, RESULT"
                If filter <> "" Then
                    strSql += "," & filter
                End If

            ElseIf CmbGroupBy.Text = "COUNTER" Then
                strSql = "UPDATE TEMP" & systemId & "TAGSTOCKVIEW SET COUNTER='NO COUNTER' WHERE COUNTER IS NULL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " IF OBJECT_ID('TEMP" & systemId & "TAGSTOCKVIEWFINAL','U') IS NOT NULL DROP TABLE TEMP" & systemId & "TAGSTOCKVIEWFINAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " SELECT  ROW_NUMBER() OVER(ORDER BY COUNTER,RESULT"
                If filter <> "" Then
                    strSql += "," & filter
                End If
                strSql += ")KEYNO"
                strSql += ", * INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL "
                strSql += "FROM TEMP" & systemId & "TAGSTOCKVIEW "
                'strSql += " ORDER BY COUNTER,ITEM,RESULT "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,COUNTER,RESULT, COLHEAD)"
                strSql += "SELECT DISTINCT COUNTER, COUNTER,'0', 'T' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,PCS, GRSWT, NETWT, SALVALUE,"
                strSql += "DIAPCS, DIAWT,PREPCS,PREWT,WAST,MC,"
                strSql += "COUNTER, RESULT, COLHEAD)"
                strSql += "SELECT 'SUB TOTAL', SUM(PCS), SUM(GRSWT), SUM(NETWT), SUM(SALVALUE), "
                strSql += "SUM(DIAPCS),SUM(DIAWT),SUM(PREPCS),SUM(PREWT),SUM(WAST),SUM(MC),"
                strSql += "COUNTER, '2', 'S' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL  WHERE RESULT = '1'"
                strSql += "GROUP BY COUNTER"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()


                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,PCS,GRSWT,NETWT,SALVALUE,"
                strSql += "DIAPCS,DIAWT,PREPCS,PREWT,WAST,MC,"
                strSql += "COUNTER,"
                strSql += "RESULT, COLHEAD)"
                strSql += "SELECT 'GRAND TOTAL', SUM(PCS), SUM(GRSWT), SUM(NETWT), SUM(SALVALUE),"
                strSql += "SUM(DIAPCS), SUM(DIAWT), SUM(PREPCS), SUM(PREWT), SUM(WAST), SUM(MC),"
                strSql += "'ZZZZ','999', 'G'"
                strSql += "FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL WHERE RESULT = '2'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,TAGNO,RESULT,COLHEAD,COUNTER)"
                strSql += "SELECT 'TAG NO', COUNT(TAGNO), '9999', 'G','ZZZZ' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "SELECT * FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL ORDER BY COUNTER,RESULT"
                If filter <> "" Then
                    strSql += "," & filter
                End If

            ElseIf CmbGroupBy.Text = "ITEM" Then
                strSql = " IF OBJECT_ID('TEMP" & systemId & "TAGSTOCKVIEWFINAL','U') IS NOT NULL DROP TABLE TEMP" & systemId & "TAGSTOCKVIEWFINAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " SELECT  ROW_NUMBER() OVER(ORDER BY ITEM,RESULT"
                If filter <> "" Then
                    strSql += "," & filter
                End If
                strSql += ")KEYNO"
                strSql += ", * INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL "
                strSql += "FROM TEMP" & systemId & "TAGSTOCKVIEW "
                'strSql += "ORDER BY RESULT "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR, ITEM,RESULT, COLHEAD)"
                strSql += "SELECT DISTINCT ITEM,ITEM,'0','T' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,PCS, GRSWT, NETWT, SALVALUE,"
                strSql += "DIAPCS, DIAWT,PREPCS,PREWT,WAST,MC,"
                strSql += "ITEM, RESULT, COLHEAD)"
                strSql += "SELECT 'SUB TOTAL', SUM(PCS), SUM(GRSWT), SUM(NETWT), SUM(SALVALUE), "
                strSql += "SUM(DIAPCS), SUM(DIAWT), SUM(PREPCS), SUM(PREWT), SUM(WAST), SUM(MC),"
                strSql += "ITEM, '2', 'S' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL "
                strSql += "GROUP BY ITEM"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,PCS, GRSWT, NETWT, SALVALUE,"
                strSql += "DIAPCS, DIAWT,PREPCS,PREWT,WAST,MC,"
                strSql += "ITEM, RESULT, COLHEAD)"
                strSql += "SELECT 'GRAND TOTAL', SUM(PCS), SUM(GRSWT), SUM(NETWT), SUM(SALVALUE), "
                strSql += "SUM(DIAPCS), SUM(DIAWT), SUM(PREPCS), SUM(PREWT), SUM(WAST), SUM(MC),"
                strSql += "'ZZZZ', '2', 'G' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL "
                strSql += "WHERE RESULT = '2'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,TAGNO,RESULT,COLHEAD,ITEM)"
                strSql += "SELECT 'TAG NO', COUNT(TAGNO), '9999', 'G','ZZZZ' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "SELECT * FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL ORDER BY ITEM,RESULT, KEYNO"
                If filter <> "" Then
                    strSql += "," & filter
                End If

            ElseIf CmbGroupBy.Text = "SUBITEM" Then

                strSql = "UPDATE TEMP" & systemId & "TAGSTOCKVIEW SET SUBITEM = 'NO SUBITEM' WHERE SUBITEM IS NULL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " IF OBJECT_ID('TEMP" & systemId & "TAGSTOCKVIEWFINAL','U') IS NOT NULL DROP TABLE TEMP" & systemId & "TAGSTOCKVIEWFINAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " SELECT  ROW_NUMBER() OVER(ORDER BY SUBITEM, RESULT"
                If filter <> "" Then
                    strSql += "," & filter
                End If
                strSql += ")KEYNO"
                strSql += ", * INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL "
                strSql += "FROM TEMP" & systemId & "TAGSTOCKVIEW ORDER BY SUBITEM, RESULT "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR, SUBITEM, RESULT, COLHEAD)"
                strSql += "SELECT DISTINCT SUBITEM, SUBITEM,'0','T' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,PCS, GRSWT, NETWT, SALVALUE, "
                strSql += "DIAPCS, DIAWT,PREPCS,PREWT,WAST,MC,"
                strSql += "SUBITEM, RESULT, COLHEAD)"
                strSql += "SELECT 'SUB TOTAL', SUM(PCS), SUM(GRSWT), SUM(NETWT), SUM(SALVALUE), "
                strSql += "SUM(DIAPCS), SUM(DIAWT), SUM(PREPCS), SUM(PREWT), SUM(WAST), SUM(MC),"
                strSql += "SUBITEM, '2', 'S' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL WHERE RESULT = '1'"
                strSql += "GROUP BY SUBITEM"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()


                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,PCS, GRSWT, NETWT, SALVALUE, "
                strSql += "DIAPCS, DIAWT,PREPCS,PREWT,WAST,MC,"
                strSql += "SUBITEM, RESULT, COLHEAD)"
                strSql += "SELECT 'GRAND TOTAL', SUM(PCS), SUM(GRSWT), SUM(NETWT), SUM(SALVALUE), "
                strSql += "SUM(DIAPCS), SUM(DIAWT), SUM(PREPCS), SUM(PREWT), SUM(WAST), SUM(MC),"
                strSql += "'ZZZZ', '2', 'G' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL "
                strSql += "WHERE RESULT = '2'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "INSERT INTO TEMP" & systemId & "TAGSTOCKVIEWFINAL(PARTICULAR,TAGNO,RESULT,COLHEAD,SUBITEM)"
                strSql += "SELECT 'TAG NO', COUNT(TAGNO), '9999', 'G','ZZZZ' FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = " SELECT * FROM TEMP" & systemId & "TAGSTOCKVIEWFINAL ORDER BY SUBITEM, RESULT "
                If filter <> "" Then
                    strSql += "," & filter
                End If
            End If
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSource)
            gridTotalView.DataSource = dtSource


            'COMMENT ON 18/08/2015
            'Dim ObjGrouper As New BrighttechPack.DataGridViewGrouper(gridTotalView, dtSource)

            'For cnt As Integer = 0 To ChkLstGroupBy.CheckedItems.Count - 1
            '    ObjGrouper.pColumns_Group.Add(ChkLstGroupBy.CheckedItems.Item(cnt).ToString)
            'Next


            'COMMENT ON 18/08/2015
            'For cnt As Integer = 0 To chkCmbGroupBy.CheckedItems.Count - 1
            '    ObjGrouper.pColumns_Group.Add(chkCmbGroupBy.CheckedItems.Item(cnt).ToString)
            'Next

            'If gridTotalView.Columns.Contains("PURVALUE") Then gridTotalView.Columns("PURVALUE").Visible = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
            'BrighttechPack.GlobalMethods.FormatGridColumns(gridTotalView, True)

            ''COMMENT ON 18/08/2015
            'ObjGrouper.pColumns_Sum.Add("PCS")
            'ObjGrouper.pColumns_Sum.Add("GRSWT")
            'ObjGrouper.pColumns_Sum.Add("LESSWT")
            'ObjGrouper.pColumns_Sum.Add("NETWT")
            'ObjGrouper.pColumns_Sum.Add("EXTRAWT")
            'ObjGrouper.pColumns_Sum.Add("SALVALUE")
            'ObjGrouper.pColumns_Sum.Add("PURVALUE")
            'ObjGrouper.pColumns_Sum.Add("STNPCS")
            'ObjGrouper.pColumns_Sum.Add("STNWTG")
            'ObjGrouper.pColumns_Sum.Add("STNWTC")

            'ObjGrouper.pColumns_Sum.Add("STNAMT")

            'ObjGrouper.pColumns_Sum.Add("DIAPCS")
            'ObjGrouper.pColumns_Sum.Add("DIAWT")

            'ObjGrouper.pColumns_Sum.Add("DIAAMT")
            'ObjGrouper.pColumns_Sum.Add("PREPCS")
            'ObjGrouper.pColumns_Sum.Add("PREWT")
            'ObjGrouper.pColumns_Sum.Add("PREAMT")


            'ObjGrouper.pColumns_Sum.Add("WAST")
            'ObjGrouper.pColumns_Sum.Add("MC")
            'ObjGrouper.pColName_Particular = "PARTICULAR"
            'ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"


            'COMMENT ON 18/08/2015
            'Dim filter As String = ""
            'For i As Integer = 0 To LstOrderby.Items.Count - 1
            '    filter += IIf(LstOrderby.Items(i).ToString = "TAGNO", "TAGVAL", LstOrderby.Items(i).ToString)
            '    If i < LstOrderby.Items.Count - 1 Then filter += ","
            'Next

            'COMMENT ON 18/08/2015
            'If filter <> "" Then
            '    ObjGrouper.pColumns_Sort = filter
            'End If

            'ObjGrouper.pColumns_Sort = "TAGVAL"
            ''---OLD SARAVANAN
            'If cmbOrderBy.Text = "WEIGHT" Then
            '    ObjGrouper.pColumns_Sort = "GRSWT,RECDATE,PARTICULAR"
            'ElseIf cmbOrderBy.Text = "RECDATE" Then
            '    ObjGrouper.pColumns_Sort = "RECDATE,GRSWT,TAGVAL,PARTICULAR"
            'ElseIf cmbOrderBy.Text = "TAGNO" Then
            '    ObjGrouper.pColumns_Sort = "TAGVAL,RECDATE,PARTICULAR,GRSWT"
            'End If

            'COMMENT ON 18/08/2015
            'ObjGrouper.pIdentityColName = "SLNO"
            'ObjGrouper.pColumns_Count.Add("TAGNO")

            RowFillState = True
            ''COMMENT ON 18/08/2015
            'ObjGrouper.GroupDgv() 'Grouping Problem 
            RowFillState = False
            funcFillStoneDetails()
            ''GrandTotalDetails
            funcFillGrandDetails()



            With gridTotalView
                For Each gvE As DataGridViewRow In gridTotalView.Rows
                    With gvE
                        Select Case .Cells("SETTAGNO").Value.ToString
                            Case ""
                                .DefaultCellStyle.BackColor = Color.LightGreen
                                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                            Case .Cells("TAGNO").Value.ToString
                                .DefaultCellStyle.BackColor = Color.LightGreen
                                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        End Select
                    End With
                Next
                gridTotalView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridTotalView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridTotalView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridTotalView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None

                For cnt As Integer = 0 To gridTotalView.ColumnCount - 1
                    gridTotalView.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("COLHEAD").Visible = False
                .Columns("ITEMID").Visible = False
                .Columns("TAGVAL").Visible = False
                .Columns("RESULT").Visible = False
                '.Columns("KEYNO").Visible = False
                .Columns("ITEM").Visible = False
                .Columns("SUBITEM").Visible = False
                With .Columns("KEYNO")
                    .HeaderText = "SNO"
                    .Width = 50
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("SLNO")
                    .HeaderText = "SNO"
                    .Width = 50
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Visible = False
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
                With .Columns("TAGNO")
                    .HeaderText = "TAG NO"
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("SETTAGNO")
                    .HeaderText = "SET TAG NO"
                    .Width = 120
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
                    .Visible = chkExtraWt.Checked
                    .Width = 100
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
                    .Visible = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
                End With
                With .Columns("STNPCS")
                    .HeaderText = "STONE PCS"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("STNWTG")
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
                With .Columns("STNAMT")
                    .HeaderText = "STONE AMT"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
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
                With .Columns("DIAAMT")
                    .HeaderText = "DIA AMT"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
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
                With .Columns("PREAMT")
                    .HeaderText = "PRE AMT"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
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
                With .Columns("SIZENAME")
                    .HeaderText = "SIZE"
                    .Width = 70
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
                If CmbGroupBy.Text = "ITEM" Then
                    If .Columns.Contains("ITEM") Then .Columns("ITEM").Visible = False
                ElseIf CmbGroupBy.Text = "SUBITEM" Then
                    If .Columns.Contains("SUBITEM") Then .Columns("SUBITEM").Visible = False
                ElseIf CmbGroupBy.Text = "DESIGNER" Then
                    If .Columns.Contains("DESIGNER") Then .Columns("DESIGNER").Visible = False
                ElseIf CmbGroupBy.Text = "COUNTER" Then
                    If .Columns.Contains("COUNTER") Then .Columns("COUNTER").Visible = False
                End If
            End With
            If ChkWithStnDiaAmt.Checked Then
                gridTotalView.Columns("STNAMT").Visible = True
                gridTotalView.Columns("DIAAMT").Visible = True
                gridTotalView.Columns("PREAMT").Visible = True
            Else
                gridTotalView.Columns("STNAMT").Visible = False
                gridTotalView.Columns("DIAAMT").Visible = False
                gridTotalView.Columns("PREAMT").Visible = False
            End If

            tabMain.SelectedTab = tabView
            Dim title As String = Nothing

            title = "INDIVIDUAL TAGED ITEM STOCK REPORT"
            If txtItemName.Text <> "" Then title += " Of " & txtItemName.Text
            title += " AS ON " + dtpAsOnDate.Value.ToString("dd-MM-yyyy")
            lblReportTitle.Text = title & IIf(cmbCostCenter.Text <> "" And cmbCostCenter.Text <> "ALL", " :" & cmbCostCenter.Text, "")
            pnlTotalGridView.BringToFront()
            gridTotalView.Focus()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            btnView_Search.Enabled = True
        End Try
    End Sub

    Private Sub gridTotalView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridTotalView.CellFormatting
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
    Private Sub gridTotalView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridTotalView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridTotalView.RowCount > 0 Then gridTotalView.CurrentCell = gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("PARTICULAR") 'gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells(0)
        End If
    End Sub

    Private Sub gridTotalView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridTotalView.RowEnter
        If RowFillState = True Then Exit Sub
        funcAddNewRow("Wastage %", Nothing, "Min Wastage %", Nothing, "Weight Unit", Nothing, "CostCentre", Nothing)
        funcAddNewRow("Wastage ", Nothing, "Min Wastage ", Nothing, "Fine Rate", Nothing, "Size Name", Nothing)
        funcAddNewRow("Mc/Grm ", Nothing, "Min Mc/Grm ", Nothing, "OrdRepNo", Nothing, "Designer", Nothing)
        funcAddNewRow("MakingCharg ", Nothing, "Min MakingCharg ", Nothing, "LotNo", Nothing, "", "")
        If gridTotalView.Rows(e.RowIndex).Cells("SNO").Value.ToString = "" Then Exit Sub
        funcGetCursorTot(e.RowIndex)
        strSql = " Select" + vbCrLf
        strSql += vbCrLf + " CASE WHEN MAXWASTPER <> 0 THEN MAXWASTPER ELSE NULL END MaxWastPer" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN MAXWAST <> 0 THEN MAXWAST ELSE NULL END MaxWast" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN MAXMCGRM <> 0 THEN MAXMCGRM ELSE NULL END MaxMcGrm" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN MAXMC <> 0 THEN MAXMC ELSE NULL END AS MAXMC" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN MINWASTPER <> 0 THEN MINWASTPER ELSE NULL END MINWastPer" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN MINWAST <> 0 THEN MINWAST ELSE NULL END MINWast" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN MINMCGRM <> 0 THEN MINMCGRM ELSE NULL END MINMcGrm" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN MINMC <> 0 THEN MINMC ELSE NULL END AS MINMC" + vbCrLf
        strSql += vbCrLf + " ,WEIGHTUNIT,CASE WHEN FINERATE <> 0 THEN FINERATE ELSE NULL END FINERATE,ORDREPNO," + vbCrLf
        strSql += vbCrLf + " (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)COSTCENTRE," + vbCrLf
        strSql += vbCrLf + " (SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)SIZENAME," + vbCrLf
        strSql += vbCrLf + " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)DESIGNERNAME," + vbCrLf
        strSql += vbCrLf + " (SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = T.LOTSNO)AS LOTNO" + vbCrLf
        strSql += vbCrLf + "  ,RTRIM(LTRIM(NARRATION))NARRATION " + vbCrLf
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T" + vbCrLf
        strSql += vbCrLf + " WHERE RECDATE = '" & CType(gridTotalView.Rows(e.RowIndex).Cells("RECDATE").Value, Date).ToString("yyyy-MM-dd") & "' " + vbCrLf
        strSql += vbCrLf + " AND  SNO = '" & gridTotalView.Item("SNO", e.RowIndex).Value & "'" + vbCrLf
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
            funcAddNewRow("Wastage %", .Item("MaxWastPer").ToString, _
            "Min Wastage %", .Item("MinWastPer").ToString, _
            "Weight Unit", .Item("WeightUnit").ToString, _
            "CostCentre", .Item("CostCentre").ToString)
            funcAddNewRow("Wastage ", .Item("MaxWast").ToString, "Min Wastage ", .Item("MinWast").ToString, "Fine Rate", .Item("FineRate").ToString, "Size Name", .Item("SizeName").ToString)
            funcAddNewRow("Mc/Grm ", .Item("MaxMcGrm").ToString, "Min Mc/Grm ", .Item("MinMcGrm").ToString, "OrdRepNo", .Item("OrdRepNo").ToString, "Designer", .Item("DesignerName").ToString)
            funcAddNewRow("MakingCharg ", .Item("MaxMc").ToString, "Min MakingCharg ", .Item("MinMc").ToString, "LotNo", .Item("LotNo").ToString, "Narration", .Item("Narration").ToString)
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
            'Dim Finfo As IO.FileInfo
            'Finfo = New IO.FileInfo(fileDestPath)
            'Finfo.IsReadOnly = False
            'If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
            '    picItem.Image = My.Resources.no_photo
            'Else
            '    Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, IO.FileAccess.Read)
            '    picItem.Image = Image.FromStream(fileStr)
            '    fileStr.Close()
            'End If
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
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMMAST AS T WHERE ISNULL(SETITEM,'')='Y' "
            If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
                strSql += vbCrLf + " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
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

    Private Sub txtItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemName.TextChanged
        If txtItemName.Text <> "" Then
            Dim dtSItem As DataTable
            cmbSubItemName.Items.Clear()
            cmbSubItemName.Items.Add("ALL")
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SETITEM,'')='Y' AND ITEMID = '" & txtItemCode_NUM.Text & "' order by SubItemName"
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

            cmbSize.Items.Clear()
            cmbSize.Items.Add("ALL")
            strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID = " & Val(txtItemCode_NUM.Text) & " ORDER BY SIZENAME"
            objGPack.FillCombo(strSql, cmbSize, False)
            cmbSize.Text = "ALL"
            If cmbSize.Items.Count > 0 Then
                cmbSize.Enabled = True
            Else
                cmbSize.Enabled = False
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
            Dim qry1 As String = Nothing
            qry = " WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG AS T "
            qry += funcStoneFiltrationString()
            qry += " )"
            If chkApproval.Checked Then
                qry1 = " WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG AS T "
                qry1 += funcStoneFiltrationString()
                qry1 += " AND T.TAGNO IN (SELECT TAGNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE < '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' AND TRANTYPE = 'AI' AND (REFDATE IS NULL OR REFDATE>='" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "') "
                qry1 += " OR T.TAGNO IN (SELECT TAGNO FROM " & cnStockDb & "..APPISSUE WHERE TRANDATE < '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' AND TRANTYPE = 'AI'  AND (REFDATE IS NULL OR REFDATE>='" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "'))) )"
                qry1 += " )"
            End If
            Dim objStudDetails As New frmTagStuddedStoneDetails(gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("SNO").Value.ToString, qry, lblReportTitle.Text, qry1)
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
            Dim objTagViewer As New frmTagImageViewer( _
            gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("TAGNO").Value.ToString, _
            gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("ITEMID").Value.ToString)
            objTagViewer.ShowDialog()
        ElseIf e.KeyChar = Chr(Keys.R) And _TagDupPrint Then
            Dim oldItem As Integer = Nothing
            Dim paramStr As String = ""
            Dim prnmemsuffix As String = ""
            Dim tagno As String = gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("TAGNO").Value.ToString
            Dim itemid As String = gridTotalView.Rows(gridTotalView.CurrentRow.Index).Cells("ITEMID").Value.ToString
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Dim write As StreamWriter
            Dim memfile As String = "\Barcodeprint" & prnmemsuffix & ".mem"
            write = IO.File.CreateText(Application.StartupPath & memfile)
            If oldItem <> Val(itemid) Then
                write.WriteLine(LSet("PROC", 7) & ":" & itemid)
                paramStr += LSet("PROC", 7) & ":" & itemid & ";"
                oldItem = Val(itemid)
            End If
            write.WriteLine(LSet("TAGNO", 7) & ":" & tagno)
            paramStr += LSet("TAGNO", 7) & ":" & tagno & ";"
            If paramStr.EndsWith(";") Then
                paramStr = Mid(paramStr, 1, paramStr.Length - 1)
            End If
            write.Flush()
            write.Close()
            If EXE_WITH_PARAM = False Then
                If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe")
                Else
                    MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                End If
            Else
                If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe", paramStr)
                Else
                    MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                End If
            End If
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        cmbSubItemName.Items.Clear()
        cmbSubItemName.Enabled = False
        ' NEWLY ADD ON 18/08/2015
        chkCmbGroupBy.Items.Clear()
        CmbGroupBy.Items.Clear()

        chkCmbGroupBy.Items.Add("DESIGNER")
        chkCmbGroupBy.Items.Add("COUNTER")
        chkCmbGroupBy.Items.Add("ITEM")
        chkCmbGroupBy.Items.Add("SUBITEM")

        CmbGroupBy.Items.Add("DESIGNER")
        CmbGroupBy.Items.Add("COUNTER")
        CmbGroupBy.Items.Add("ITEM")
        CmbGroupBy.Items.Add("SUBITEM")

        LstOrderby.Items.Clear()
        LstOrderby.Items.Add("TAGNO")

        'ChklstboxOrderby.Items.Clear()
        'ChklstboxOrderby.Items.Add("TAGNO", True)
        'ChklstboxOrderby.Items.Add("TAGNO", True)
        'ChklstboxOrderby.Items.Add("RECDATE", False)
        'ChklstboxOrderby.Items.Add("GRSWT", False)
        'ChklstboxOrderby.Items.Add("PARTICULAR", False)
        'ChklstboxOrderby.Items.Add("NETWT", False)
        'ChklstboxOrderby.Items.Add("LESSWT", False)
        'ChklstboxOrderby.Items.Add("TOUCH", False)
        'ChklstboxOrderby.Items.Add("RATE", False)
        'ChklstboxOrderby.Items.Add("SALVALUE", False)
        'ChklstboxOrderby.Items.Add("PURVALUE", False)
        'ChklstboxOrderby.Items.Add("COUNTER", False)
        'ChklstboxOrderby.Items.Add("PURITY", False)
        'ChklstboxOrderby.Items.Add("NARRATION", False)
        'ChklstboxOrderby.Items.Add("BRANDID", False)
        'ChklstboxOrderby.Items.Add("SALEMODE", False)
        'ChklstboxOrderby.Items.Add("TRANINVOICENO", False)
        'ChklstboxOrderby.Items.Add("SUPBILLNO", False)
        'ChklstboxOrderby.Items.Add("DESIGNER", False)
        'ChklstboxOrderby.Items.Add("SIZENAME", False)


        rbtAll.Checked = True
        txtItemCode_NUM.Select()
        cmbGroup.Items.Clear()
        cmbGroup.Items.Add("NONE")
        cmbGroup.Items.Add("ITEM")
        cmbGroup.Items.Add("COUNTER")
        cmbGroup.Items.Add("DESIGNER")
        cmbGroup.Text = "NONE"
        ''Size
        cmbSize.Items.Clear()
        cmbSize.Items.Add("ALL")
        cmbSize.Text = "ALL"
        cmbSize.Enabled = False

        ''Designer
        cmbDesigner.Items.Clear()
        cmbDesigner.Items.Add("ALL")
        strSql = " Select DesignerName from " & cnAdminDb & "..Designer order by DesignerName"
        objGPack.FillCombo(strSql, cmbDesigner, False)
        cmbDesigner.Text = "ALL"

        strSql = vbCrLf + " SELECT NAME FROM " & cnAdminDb & "..SYSCOLUMNS WHERE ID IN (SELECT ID FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME='ITEMTAG')"
        strSql += vbCrLf + " ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbSearchKey, True, False)
        cmbSearchKey.Text = ""
        txtSearch_txt.Text = ""
        txtPcs_NUM.Text = ""

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
        cmbCounterName.Text = "ALL"

        ''ItemType
        cmbItemType.Items.Clear()
        cmbItemType.Items.Add("ALL")
        strSql = " Select Name from " & cnAdminDb & "..itemType order by Name"
        objGPack.FillCombo(strSql, cmbItemType, False)
        cmbItemType.Text = "ALL"

        ''CostCenter
        cmbCostCenter.Items.Clear()
        If cmbCostCenter.Enabled = True Then
            cmbCostCenter.Items.Add("ALL")
            strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
            objGPack.FillCombo(strSql, cmbCostCenter, False)
            cmbCostCenter.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCenter.Enabled = False
        End If
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        dtpAsOnDate.Value = GetEntryDate(GetServerDate)
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
        Studded = False
        chkCmbStoneName.Enabled = Studded
        chkCmbStSubItemName.Enabled = Studded
        chkCmbMetal.Select()
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

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub chkCmbStoneName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbStoneName.GotFocus
        chkCmbStoneName.Enabled = Studded
    End Sub
    Private Sub chkCmbStoneName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbStoneName.LostFocus
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

    Private Sub chkCmbStSubItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbStSubItemName.GotFocus
        chkCmbStSubItemName.Enabled = Studded
    End Sub

    Private Sub ChklstboxOrderby_ItemCheck(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs)
        'If ChklstboxOrderby.SelectedIndex < 0 Then Exit Sub
        'If ChklstboxOrderby.GetItemChecked(ChklstboxOrderby.SelectedIndex) = False Then
        '    LstOrderby.Items.Add(ChklstboxOrderby.Items(ChklstboxOrderby.SelectedIndex).ToString())
        'Else
        '    LstOrderby.Items.Remove(ChklstboxOrderby.Items(ChklstboxOrderby.SelectedIndex).ToString())
        'End If
    End Sub

    Private Sub txtItemCode_NUM_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtItemCode_NUM.TextChanged

    End Sub
End Class