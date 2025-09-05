Imports System.Data.OleDb
Imports System.IO
'HISTORY
'221 date 22-10-2012 SATHYARAJ
Public Class CatalogView
    Dim Cmd As OleDbCommand
    Dim bmap As New Bitmap(My.Resources.no_photo)
    Dim strSql As String
    Dim dtImagePath As New DataTable
    Dim dtCatalogStone As New DataTable
    Dim dtTemp As New DataTable
    Dim count As Integer = 0
    Dim objGridShower As frmGridDispDia
    Dim dtGridResult As New DataTable
    Dim dgvImageColumn As DataGridViewImageColumn
    Dim dgvTextColumn As DataGridViewTextBoxColumn
    Dim dgvImageCell As DataGridViewImageCell
    Dim dgvTextCell As DataGridViewTextBoxCell
    Dim myImageListSmall As New ImageList
    Dim myImageListLarge As New ImageList
    Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGCATALOGPATH'")

    Private Sub PrintCatalogSno(ByVal Sno As String)
        Dim StyleNo As String
        strSql = vbCrLf + " SELECT STYLENO FROM " & cnAdminDb & "..TAGCATALOG"
        strSql += vbCrLf + " WHERE SNO = '" & Sno & "'"
        Dim DtTemp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtTemp)
        If DtTemp.Rows.Count > 0 Then
            StyleNo = DtTemp.Rows(0).Item("STYLENO").ToString
            PrintCatalog(StyleNo)
        Else
            Exit Sub
        End If
    End Sub
    Private Sub PrintCatalog(ByVal StyleNo As String)
        Dim vPicBox As New PictureBox
        strSql = vbCrLf + " SELECT PCTFILE FROM " & cnAdminDb & "..TAGCATALOG"
        strSql += vbCrLf + " WHERE STYLENO = '" & StyleNo & "'"
        Dim DtTemp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtTemp)
        If DtTemp.Rows.Count > 0 Then
            vPicBox.Size = New Size(650, 500)
            AutoImageSizer(defaultPic & DtTemp.Rows(0).Item("PCTFILE").ToString, vPicBox, PictureBoxSizeMode.CenterImage)
        Else
            Exit Sub
        End If
        strSql = " IF OBJECT_ID('MASTER..TEMP_STYLEINFO') IS NOT NULL DROP TABLE MASTER..TEMP_STYLEINFO"
        strSql += vbCrLf + " SELECT T.STYLENO,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM,IT.NAME AS ITEMTYPE"
        strSql += vbCrLf + " ,T.PCS,T.GRSWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..TAGCATALOGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..TAGCATALOGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
        strSql += vbCrLf + " INTO MASTER..TEMP_STYLEINFO"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..TAGCATALOG T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = T.ITEMID AND SM.SUBITEMID = T.SUBITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTYPE AS IT ON IT.ITEMTYPEID = T.ITEMTYPEID"
        strSql += vbCrLf + " WHERE T.STYLENO = '" & StyleNo & "'"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        strSql = " SELECT 'ITEM' AS DESC1,ITEM DESC2 ,'STYLENO' DESC3,STYLENO DESC4,'PCS' DESC5 ,CONVERT(VARCHAR,PCS) DESC6,'GRSWT' DESC7,CONVERT(VARCHAR,GRSWT) DESC8"
        strSql += " FROM MASTER..TEMP_STYLEINFO"
        strSql += " UNION ALL"
        strSql += " SELECT 'SUBITEM' AS DESC1,SUBITEM DESC2 ,'ITEMTYPE' DESC3,ITEMTYPE DESC4,'DIA PCS' DESC5 ,CONVERT(VARCHAR,DIAPCS) DESC6,'DIA WT' DESC7,CONVERT(VARCHAR,DIAWT) DESC8"
        strSql += " FROM MASTER..TEMP_STYLEINFO"
        Dim vDtCatalogInfo As New BrightPosting.GDatatable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(vDtCatalogInfo)
        vDtCatalogInfo.pRowHeight = 21
        vDtCatalogInfo.pColHeaderVisible = False
        vDtCatalogInfo.pCellBorder = False
        vDtCatalogInfo.pContentFont = New Font("TIMES NEW ROMAN", 8, FontStyle.Bold)
        vDtCatalogInfo.Columns("DESC1").Caption = 60
        vDtCatalogInfo.Columns("DESC2").Caption = 150
        vDtCatalogInfo.Columns("DESC3").Caption = 60
        vDtCatalogInfo.Columns("DESC4").Caption = 150
        vDtCatalogInfo.Columns("DESC5").Caption = 60
        vDtCatalogInfo.Columns("DESC6").Caption = 60
        vDtCatalogInfo.Columns("DESC7").Caption = 60
        vDtCatalogInfo.Columns("DESC8").Caption = 60

        strSql = vbCrLf + " SELECT CASE WHEN ISNULL(SM.SUBITEMNAME,'') = '' THEN  IM.ITEMNAME ELSE SM.SUBITEMNAME END AS [DESCRIPTION],T.STNPCS AS PCS,T.STNWT AS WEIGHT,SIZEDESC AS [SEIVE SIZE]"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..TAGCATALOGSTONE AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.STNITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = T.STNITEMID AND SM.SUBITEMID = T.STNSUBITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CENTSIZE AS CZ ON CZ.ITEMID = T.STNITEMID AND CZ.SUBITEMID = T.STNSUBITEMID"
        strSql += vbCrLf + " AND (T.STNWT/CASE WHEN T.STNPCS <> 0 THEN T.STNPCS ELSE 1 END)*100 BETWEEN CZ.FROMCENT AND CZ.TOCENT"
        strSql += vbCrLf + " WHERE T.TAGSNO = (SELECT SNO FROM " & cnAdminDb & "..TAGCATALOG WHERE STYLENO = '" & StyleNo & "')"
        Dim vDtCatalogStone As New BrightPosting.GDatatable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(vDtCatalogStone)
        vDtCatalogStone.pRowHeight = 21
        vDtCatalogStone.Columns("DESCRIPTION").Caption = 220
        vDtCatalogStone.Columns("PCS").Caption = 60
        vDtCatalogStone.Columns("WEIGHT").Caption = 100
        vDtCatalogStone.Columns("SEIVE SIZE").Caption = 140
        Dim lstSource As New List(Of Object)
        lstSource.Clear()
        lstSource.Add(vPicBox)
        lstSource.Add(vDtCatalogInfo)
        lstSource.Add(vDtCatalogStone)
        Dim obj As New BrightPosting.GListPrinter(lstSource)
        obj.pTitle = "STYLENO : " & StyleNo
        obj.Print()
    End Sub

    Function funcFiltration() As String
        Dim Qry As String = Nothing
        If chkLstItem.CheckedItems.Count > 0 Then
            Qry += " AND C.ITEMID IN ((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & funcGetCheckedVal(chkLstItem) & ")))"
        End If
        If chkLstSubItem.CheckedItems.Count > 0 Then
            Qry += " AND C.SUBITEMID IN ((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & funcGetCheckedVal(chkLstSubItem) & ")))"
        End If
        If Val(txtGrsWtFrom_Wet.Text) > 0 And Val(txtGrsWtTo_Wet.Text) > 0 Then
            Qry += " AND C.GRSWT BETWEEN " & Val(txtGrsWtFrom_Wet.Text) & " AND " & Val(txtGrsWtTo_Wet.Text) & ""
        End If
        If Val(txtNetWtFrom_Wet.Text) > 0 And Val(txtNetWtTo_Wet.Text) > 0 Then
            Qry += " AND C.NETWT BETWEEN " & Val(txtNetWtFrom_Wet.Text) & " AND " & Val(txtNetWtTo_Wet.Text) & ""
        End If
        If chkLstStoneItem.CheckedItems.Count > 0 Then
            Qry += " AND SNO IN (SELECT TAGSNO FROM " & cnAdminDb & "..TAGCATALOGSTONE WHERE STNITEMID IN ((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & funcGetCheckedVal(chkLstStoneItem) & "))))"
        End If
        If chkLstStoneSubItem.CheckedItems.Count > 0 Then
            Qry += " AND SNO IN (SELECT TAGSNO FROM " & cnAdminDb & "..TAGCATALOGSTONE WHERE STNSUBITEMID IN ((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & funcGetCheckedVal(chkLstStoneSubItem) & "))))"
        End If
        If Val(txtStnWtFrom_Wet.Text) > 0 And Val(txtStnWtTo_Wet.Text) > 0 Then
            Qry += " AND SNO IN (SELECT TAGSNO FROM " & cnAdminDb & "..TAGCATALOGSTONE WHERE CONVERT(NUMERIC(15,3),CASE WHEN STONEUNIT = 'C' THEN STNWT /5 ELSE STNWT END) BETWEEN " & txtStnWtFrom_Wet.Text & " AND " & txtStnWtTo_Wet.Text & " )"
        End If
        If Qry <> Nothing Then
            Qry = Qry.Remove(0, 4)
            Qry = " WHERE " & Qry
        End If
        Return Qry
    End Function

    Private Sub SearchItem(ByRef obj As CheckedListBox, ByRef txt As TextBox)
        If txt.Text = "" Then
            obj.SetSelected(0, False)
            Exit Sub
        End If
        For cnt As Integer = 0 To chkLstItem.Items.Count - 1
            If UCase(obj.Items.Item(cnt).ToString).StartsWith(UCase(txt.Text)) Then
                obj.SelectedIndex = cnt
                'obj.SetSelected(cnt, True)
                Exit For
            End If
        Next
    End Sub
    Function funcGetCheckedVal(ByVal obj As CheckedListBox) As String
        Dim str As String = Nothing
        Dim Qry As String = Nothing
        For cnt As Integer = 0 To obj.CheckedItems.Count - 1
            str += "'" + obj.CheckedItems.Item(cnt).ToString + "',"
        Next
        If str = Nothing Then
            str = "''"
        Else
            str = str.Remove(str.Length - 1, 1)
        End If
        'dtTemp.Clear()
        'Qry = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & str & ")"
        'da = New OleDbDataAdapter(Qry, cn)
        'da.Fill(dtTemp)
        'str = Nothing
        'For CNT As Integer = 0 To dtTemp.Rows.Count - 1
        '    str += "'" + dtTemp.Rows(CNT).Item(0).ToString + "',"
        'Next
        'If str = Nothing Then
        '    str = "''"
        'Else
        '    str = str.Remove(str.Length - 1, 1)
        'End If
        Return str
    End Function

    Function funcNew() As Integer
        objGPack.TextClear(Me)
        chkLstSubItem.Items.Clear()
        txtSubItem.Enabled = False
        chkLstSubItem.Enabled = False
        chkLstStoneSubItem.Items.Clear()
        chkLstStoneSubItem.Enabled = False
        txtSubItem.BackColor = System.Drawing.SystemColors.Window
    End Function

    Function funcLoadCheckedList(ByRef obj As CheckedListBox, ByVal Qry As String) As Integer
        obj.Items.Clear()
        Dim dtList As New DataTable
        da = New OleDbDataAdapter(Qry, cn)
        da.Fill(dtList)
        If Not dtList.Rows.Count > 0 Then
            Exit Function
        End If
        For cnt As Integer = 0 To dtList.Rows.Count - 1
            obj.Items.Add(dtList.Rows(cnt).Item(0).ToString)
        Next
    End Function
    Private Sub LoadFilmStrip(ByVal dt As DataTable)
        picImage.BorderStyle = BorderStyle.None
        pnlImage.BackColor = Color.White
        pnlFooter.BackColor = Color.White
        pnlRightPart.BackColor = Color.White
        pnlRightPart.BorderStyle = BorderStyle.None
        tabView.BackColor = Color.White
        Dim thumbWidth As Integer = 100
        Dim thumbHeight As Integer = 100
        ''DgvStone
        dgvStoneDetail.BackgroundColor = Color.White
        ''Adding Dgv
        dgv.Columns.Clear()
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.None
        dgv.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        dgv.BackgroundColor = Color.White
        dgv.Height = thumbHeight
        dgv.ColumnHeadersVisible = False
        dgv.RowHeadersVisible = False
        Dim dgvImageCell As New DataGridViewImageCell
        Dim dgvTextCell As New DataGridViewTextBoxCell
        Dim dgvImageCol As New DataGridViewImageColumn
        Dim dgvTextCol As New DataGridViewTextBoxColumn
        dgvImageCol.Name = "-1"
        dgvImageCol.Width = thumbWidth
        dgvImageCol.Visible = False
        dgv.Columns.Add(dgvImageCol)
        dgv.Rows.Add()
        dgv.Rows(0).Height = thumbHeight
        ''Adding Dgv Footer
        dgvFooter.Columns.Clear()
        dgvFooter.CellBorderStyle = DataGridViewCellBorderStyle.None
        dgvFooter.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        dgvFooter.BackgroundColor = Color.White
        dgvFooter.ColumnHeadersVisible = False
        dgvFooter.RowHeadersVisible = False
        dgvTextCol.Name = "-1"
        dgvTextCol.Width = thumbWidth
        dgvTextCol.Visible = False
        dgvFooter.Columns.Add(dgvTextCol)
        dgvFooter.Rows.Add()
        dgvFooter.Rows.Add()
        dgvFooter.Rows.Add()
        dgvFooter.GridColor = Color.White


        Dim fStream As FileStream = Nothing
        Dim picTemp As New PictureBox
        Dim i As Integer = 1
        Dim refreshVar As Integer = 1

        For Each Row As DataRow In dt.Rows
            dgvImageCol = New DataGridViewImageColumn
            dgvImageCol.Name = i
            dgvImageCol.Width = thumbWidth
            dgv.Columns.Add(dgvImageCol)

            AutoImageSizer(Row!PCTFILE.ToString, picTemp, PictureBoxSizeMode.CenterImage)
            dgvImageCell = dgv.Rows(0).Cells(i)
            dgvImageCell.Value = picTemp.Image
            dgvImageCell.ImageLayout = DataGridViewImageCellLayout.Normal

            dgvTextCol = New DataGridViewTextBoxColumn
            dgvTextCol.Name = i
            dgvTextCol.Width = thumbWidth
            dgvFooter.Columns.Add(dgvTextCol)

            dgvTextCell = dgvFooter.Rows(0).Cells(i)
            dgvTextCell.Tag = Row!PCTFILE.ToString
            dgvTextCell.Value = Row!STYLENO

            dgvTextCell = dgvFooter.Rows(1).Cells(i)
            dgvTextCell.Value = "P-" & Row!PCS.ToString & "/G-" & Row!Grswt.ToString

            dgvTextCell = dgvFooter.Rows(2).Cells(i)
            If Row!DIAPCS.ToString <> "" Or Row!DIAWT.ToString <> "" Then
                dgvTextCell.Value = "D-" & Row!DIAPCS.ToString & "/" & Row!DIAWT.ToString
            Else
                dgvTextCell.Value = DBNull.Value
            End If
            If (i - refreshVar) = 200 Then
                ProgressBarStep(i.ToString + " Tag's Loaded..")
                refreshVar = i
            End If
            i += 1
        Next
        ProgressBarHide()
        tabMain.SelectedTab = tabView
        dgvFooter.CurrentCell = dgvFooter.Rows(0).Cells(1)
        dgvFooter.Focus()
        dgvFooter_SelectionChanged(Me, New EventArgs)
    End Sub
    Private Sub CrystalView()
        btnSearch.Enabled = False
        Try
            ProgressBarShow(ProgressBarStyle.Marquee)
            strSql = vbCrLf + " IF OBJECT_ID('MASTER..VTAGCATALOG','U') IS NOT NULL DROP TABLE MASTER..VTAGCATALOG"
            strSql += vbCrLf + " SELECT C.STYLENO,C.ITEMID,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
            strSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = C.ITEMTYPEID)AS ITEMTYPE"
            strSql += vbCrLf + " ,C.PCS,C.GRSWT,C.SNO,C.PCTFILE,CONVERT(IMAGE,NULL)CATIMAGE"
            strSql += vbCrLf + " INTO MASTER..VTAGCATALOG"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..TAGCATALOG AS C"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = C.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = C.ITEMID AND SM.SUBITEMID = C.SUBITEMID "
            strSql += funcFiltration()
            Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
            strSql = vbCrLf + " IF OBJECT_ID('MASTER..VTAGCATALOGSTONE','U') IS NOT NULL DROP TABLE MASTER..VTAGCATALOGSTONE"
            strSql += vbCrLf + " SELECT IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM,STNPCS AS STNPCS,STNWT"
            strSql += vbCrLf + " ,(SELECT SIZEDESC FROM " & cnadmindb & "..CENTSIZE WHERE ITEMID = T.STNITEMID AND SUBITEMID = T.STNSUBITEMID "
            strSql += vbCrLf + " 		AND (CASE WHEN T.STNPCS > 0 THEN STNWT/STNPCS ELSE STNWT END*100) BETWEEN FROMCENT AND TOCENT)AS SEIVE"
            strSql += vbCrLf + " 		,T.TAGSNO"
            strSql += vbCrLf + " INTO MASTER..VTAGCATALOGSTONE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..TAGCATALOGSTONE AS T"
            strSql += vbCrLf + " INNER JOIN MASTER..VTAGCATALOG AS C ON C.SNO = T.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.STNITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = T.STNITEMID AND SM.SUBITEMID = T.STNSUBITEMID "
            Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            strSql = " SELECT * FROM MASTER..VTAGCATALOG"
            Dim dtUpd As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtUpd)
            If Not dtUpd.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Sub
            End If
            For Each ro As DataRow In dtUpd.Rows
                Dim serverPath As String = Nothing
                Dim fileDestPath As String = defaultPic & ro!PCTFILE.ToString
                If IO.File.Exists(fileDestPath) Then
                    Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, IO.FileAccess.Read)
                    Dim reader As New IO.BinaryReader(fileStr)
                    Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                    fileStr.Read(result, 0, result.Length)
                    fileStr.Close()
                    strSql = " UPDATE MASTER..VTAGCATALOG SET CATIMAGE = ? WHERE SNO = '" & ro!SNO.ToString & "'"
                    Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000
                    Cmd.Parameters.AddWithValue("@image", result)
                    Cmd.ExecuteNonQuery()
                End If
            Next
            ProgressBarStep("Loading Report")
            Dim objReport As New GiritechReport

            Dim objRpt As New RPT_CATALOGVIEW
            Dim objPramValue As New CrystalDecisions.Shared.ParameterValues
            Dim objDiscValue As CrystalDecisions.Shared.ParameterDiscreteValue
            objPramValue = New CrystalDecisions.Shared.ParameterValues
            objDiscValue = New CrystalDecisions.Shared.ParameterDiscreteValue
            objDiscValue.Value = ""
            objPramValue.Add(objDiscValue)
            objRpt.SetParameterValue(0, objPramValue)

            Dim objRptViewer As New frmReportViewer
            objRptViewer.CrystalReportViewer1.ReportSource = objReport.ShowReport(objRpt, cnDataSource)
            objRptViewer.MdiParent = Main
            objRptViewer.Dock = DockStyle.Fill
            objRptViewer.Show()
            objRptViewer.CrystalReportViewer1.Select()
        Catch ex As Exception
            ProgressBarHide()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            ProgressBarHide()
            btnSearch.Enabled = True
        End Try

    End Sub
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If rbtCrystalView.Checked Then
            CrystalView()
            Exit Sub
        End If
        btnSearch.Enabled = False
        Try
            If chkStudColumn.Checked = False Then
                ProgressBarShow()
                dtImagePath.Rows.Clear()
                strSql = " IF (SELECT TOP 1 1 FROM MASTER..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATALOG')>0 DROP TABLE MASTER..TEMP" & systemId & "CATALOG"
                strSql += " DECLARE @DEFPATH VARCHAR(200)"
                strSql += " SELECT @DEFPATH = '" & defaultPic & "'"
                strSql += " SELECT * INTO MASTER..TEMP" & systemId & "CATALOG FROM "
                strSql += " ("
                strSql += vbCrLf + " SELECT"
                strSql += vbCrLf + " C.STYLENO,I.ITEMNAME AS ITEM,S.SUBITEMNAME AS SUBITEM,C.PCS,C.GRSWT,C.NETWT"
                strSql += vbCrLf + " ,(SELECT SUM(ISNULL(STNPCS,0)) FROM " & cnAdminDb & "..TAGCATALOGSTONE WHERE TAGSNO = C.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
                strSql += vbCrLf + " ,(SELECT SUM(ISNULL(STNWT,0)) FROM " & cnAdminDb & "..TAGCATALOGSTONE WHERE TAGSNO = C.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAWT"
                strSql += vbCrLf + " ,T.NAME AS ITEMTYPE,C.SIZEDESC,NARRATION "
                strSql += vbCrLf + " ,@DEFPATH + C.PCTFILE PCTFILE,C.PCTFILE PICNAME,C.SNO,C.ITEMID"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..TAGCATALOG AS C"
                strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..ITEMMAST AS I ON C.ITEMID = I.ITEMID"
                strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.ITEMID = C.ITEMID AND S.SUBITEMID = C.SUBITEMID"
                strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..ITEMTYPE AS T ON T.ITEMTYPEID = C.ITEMTYPEID"
                strSql += funcFiltration()
                strSql += " )X"
                If Val(txtDiaWtFrom_Wet.Text) > 0 And Val(txtDiaWtTo_Wet.Text) > 0 Then
                    strSql += " WHERE DIAWT BETWEEN " & Val(txtDiaWtFrom_Wet.Text) & " AND " & Val(txtDiaWtTo_Wet.Text) & ""
                End If
                Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                strSql = vbCrLf + " SELECT "
                strSql += vbCrLf + " (SELECT CASE WHEN SHORTNAME <> '' THEN SHORTNAME ELSE ITEMNAME END FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID)AS ITEM"
                strSql += vbCrLf + " ,STNPCS PCS,STNWT WEIGHT,TAGSNO"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..TAGCATALOGSTONE AS T"
                strSql += vbCrLf + " WHERE TAGSNO IN (SELECT SNO FROM MASTER..TEMP" & systemId & "CATALOG)"
                dtCatalogStone = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtCatalogStone)
            Else
                strSql = "IF(Select top 1 1 from master..sysobjects where NAME='TEMP" & systemId & "CATALOG')>0"
                strSql += " DROP TABLE MASTER..TEMP" & systemId & "CATALOG"
                strSql += " SELECT "
                strSql += " SNO,C.STYLENO,C.ITEMID ID,ITEMNAME ITEM,SUBITEMNAME SUBITEM,NAME ITEMTYPE,PCS,C.GRSWT"
                strSql += " INTO MASTER..TEMP" & systemId & "CATALOG"
                strSql += " FROM " & cnAdminDb & "..TAGCATALOG AS C"
                strSql += " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID=C.ITEMID"
                strSql += " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.ITEMID=C.ITEMID AND S.SUBITEMID=C.SUBITEMID"
                strSql += " LEFT JOIN " & cnAdminDb & "..ITEMTYPE AS IT ON IT.ITEMTYPEID=C.ITEMTYPEID"
                'If txtItem.Text <> "" Then
                '    strSql += "  WHERE C.ITEMID = '" & txtItem.Text & "'"
                'End If

                'If txtSubItem.Text <> "" Then
                '    strSql += "  AND C.SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtSubItem.Text & "' AND ITEMID = " & Val(txtItem.Text) & "),0)"
                'End If
                strSql += funcFiltration()
                If txtDiaWtFrom_Wet.Text <> "" Or txtDiaWtTo_Wet.Text <> "" Then
                    strSql += " and ((select sum(stnwt) from " & cnAdminDb & "..TAGCATALOGSTONE WHERE TAGSNO = C.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) between '" & Val(txtDiaWtFrom_Wet.Text) & "' and '" & Val(txtDiaWtTo_Wet.Text) & "')"
                End If
                Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                strSql = "IF (SELECT TOP 1 1 FROM MASTER..SYSOBJECTS WHERE NAME='TEMPCATELOGVIEWSTONE')>0"
                strSql += " DROP TABLE MASTER..TEMPCATELOGVIEWSTONE"
                strSql += " Select TAGSNO"
                strSql += " ,I.ITEMNAME ITEM"
                strSql += " ,S.SUBITEMNAME SUBITEM"
                strSql += " ,T.STNPCS PCS"
                strSql += " ,T.STNWT WEIGHT"
                strSql += " ,CONVERT(VARCHAR(15),ISNULL(CASE WHEN T.STONEUNIT='C' THEN (SELECT SIZEDESC FROM " & cnAdminDb & "..CENTSIZE WHERE T.STNWT BETWEEN FROMCENT AND TOCENT ) ELSE NULL END,'')) SEIVE"
                strSql += " INTO MASTER..TEMPCATELOGVIEWSTONE"
                strSql += " FROM " & cnAdminDb & "..TAGCATALOGSTONE AS T"
                strSql += "  INNER JOIN MASTER..TEMP" & systemId & "CATALOG AS TT ON TT.SNO = T.TAGSNO"
                strSql += "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.STNITEMID "
                strSql += " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.ITEMID = T.STNITEMID AND S.SUBITEMID = T.STNSUBITEMID "

                Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                strSql = "Select * from Master..TEMP" & systemId & "CATALOG ORDER BY SNO"
                dtGridResult.Columns.Clear()
                dtGridResult.Rows.Clear()
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGridResult)

                strSql = "select * from master..TEMPCATELOGVIEWSTONE where  TAGSNO in ('637','1400','1814','1967') ORDER BY TAGSNO"
                Dim dtGrid As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGrid)
                count = Val(objGPack.GetSqlValue("SELECT TOP 1 COUNT(*) T FROM MASTER..TEMPCATELOGVIEWSTONE GROUP BY TAGSNO ORDER BY COUNT(*) DESC"))

                For i As Integer = 0 To count - 1
                    'If Not dtGridResult.Columns.Contains("Item" + i.ToString) Then
                    dtGridResult.Columns.Add("ITEM" + i.ToString)
                    dtGridResult.Columns.Add("SUBITEM" + i.ToString)
                    dtGridResult.Columns.Add("PCS" + i.ToString)
                    dtGridResult.Columns.Add("WEIGHT" + i.ToString)
                    dtGridResult.Columns.Add("SEIVE" + i.ToString)
                    'End If
                Next
                Dim k As Integer = 0
                For j As Integer = 0 To dtGridResult.Rows.Count - 1
                    For i As Integer = 0 To count - 1
                        If k < dtGrid.Rows.Count Then
                            If dtGridResult.Rows(j).Item("SNO") = dtGrid.Rows(k).Item("TAGSNO") Then
                                dtGridResult.Rows(j).Item("ITEM" + i.ToString) = dtGrid.Rows(k).Item("ITEM")
                                dtGridResult.Rows(j).Item("SUBITEM" + i.ToString) = dtGrid.Rows(k).Item("SUBITEM")
                                dtGridResult.Rows(j).Item("PCS" + i.ToString) = dtGrid.Rows(k).Item("PCS")
                                dtGridResult.Rows(j).Item("WEIGHT" + i.ToString) = dtGrid.Rows(k).Item("WEIGHT")
                                dtGridResult.Rows(j).Item("SEIVE" + i.ToString) = dtGrid.Rows(k).Item("SEIVE")
                                k = k + 1
                            End If
                        End If
                    Next
                Next


                'count = Val(objGPack.GetSqlValue("SELECT TOP 1 COUNT(*) T FROM MASTER..TEMPCATELOGVIEWSTONE GROUP BY TAGSNO ORDER BY COUNT(*) DESC"))

                'For i As Integer = 1 To count
                '    strSql = "ALTER TABLE MASTER..TEMP" & systemId & "CATALOG ADD ITEM" + i.ToString() + " VARCHAR(50)"
                '    strSql += "   ALTER TABLE MASTER..TEMP" & systemId & "CATALOG ADD SUBITEM" + i.ToString() + " VARCHAR(50)"
                '    strSql += "   ALTER TABLE MASTER..TEMP" & systemId & "CATALOG ADD PCS" + i.ToString() + " int"
                '    strSql += "   ALTER TABLE MASTER..TEMP" & systemId & "CATALOG ADD WEIGHT" + i.ToString() + " Numeric(12,4)"
                '    strSql += "   ALTER TABLE MASTER..TEMP" & systemId & "CATALOG ADD SEIVE" + i.ToString() + " VARCHAR(15)"
                '    Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                '    strSql = "   UPDATE TV SET ITEM" + i.ToString() + "=Y.ITEM ,SUBITEM" + i.ToString() + "=Y.SUBITEM, PCS" + i.ToString() + "=Y.PCS, WEIGHT" + i.ToString() + "=Y.WEIGHT, SEIVE" + i.ToString() + "=Y.SEIVE"
                '    strSql += "   FROM MASTER..TEMP" & systemId & "CATALOG TV ,"
                '    strSql += "   (SELECT SNO"
                '    strSql += "   ,(SELECT ITEM FROM"
                '    strSql += "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,ITEM"
                '    strSql += "   FROM MASTER..TEMPCATELOGVIEWSTONE "
                '    strSql += "   WHERE TAGSNO =T.SNO"
                '    strSql += "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) ITEM"
                '    strSql += "   ,(SELECT SUBITEM FROM"
                '    strSql += "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,SUBITEM"
                '    strSql += "   FROM MASTER..TEMPCATELOGVIEWSTONE "
                '    strSql += "   WHERE TAGSNO =T.SNO"
                '    strSql += "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) SUBITEM"
                '    strSql += "   ,(SELECT PCS FROM"
                '    strSql += "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,PCS"
                '    strSql += "   FROM MASTER..TEMPCATELOGVIEWSTONE "
                '    strSql += "   WHERE TAGSNO =T.SNO"
                '    strSql += "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) PCS"
                '    strSql += "   ,(SELECT WEIGHT FROM"
                '    strSql += "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,WEIGHT"
                '    strSql += "   FROM MASTER..TEMPCATELOGVIEWSTONE "
                '    strSql += "   WHERE TAGSNO =T.SNO"
                '    strSql += "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) WEIGHT"
                '    strSql += "   ,(SELECT SEIVE FROM"
                '    strSql += "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,SEIVE"
                '    strSql += "   FROM MASTER..TEMPCATELOGVIEWSTONE "
                '    strSql += "   WHERE TAGSNO =T.SNO"
                '    strSql += "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) SEIVE"
                '    strSql += "   FROM MASTER..TEMP" & systemId & "CATALOG T)Y"
                '    strSql += "  WHERE TV.SNO =Y.SNO"
                '    Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

                'Next i
            End If
            If chkStudColumn.Checked = False Then
                strSql = " SELECT * FROM MASTER..TEMP" & systemId & "CATALOG "
                strSql += "ORDER BY ITEM,SUBITEM"

                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtImagePath)
                If Not dtImagePath.Rows.Count > 0 Then
                    btnSearch.Enabled = True
                    MsgBox(E0011, MsgBoxStyle.Information)
                    Exit Sub
                End If
                If rbtList.Checked Then
                    tabView.Show()
                    LoadFilmStrip(dtImagePath)
                ElseIf rbtGrid.Checked Then
                    gridDetailView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
                    tabGridView.Show()
                    Dim refreshVar As Integer = 0
                    Dim fileDestPath As String = Nothing
                    Dim picBox As New PictureBox
                    picBox.Size = New Size(100, 100)
                    picBox.SizeMode = PictureBoxSizeMode.CenterImage
                    Dim cel As DataGridViewImageCell
                    For Each ro As DataGridViewRow In gridDetailView.Rows
                        refreshVar += 1
                        cel = New DataGridViewImageCell
                        cel = ro.Cells("TAGIMAGE")
                        If ro.Cells("PCTFILE").Value.ToString = "" Then
                            ro.Cells("TAGIMAGE").Value = My.Resources.EmptyImage
                        Else
                            fileDestPath = ro.Cells("PCTFILE").Value.ToString
                            If IO.File.Exists(fileDestPath) Then
                                AutoImageSizer(fileDestPath, picBox, PictureBoxSizeMode.CenterImage)
                                cel.Value = picBox.Image
                                cel.ImageLayout = DataGridViewImageCellLayout.Normal
                                ro.Height = 100
                                'ro.Cells("TAGIMAGE").Value = picBox.Image
                                'ro.Resizable = DataGridViewTriState.True
                                'ro.Height = 50
                            Else
                                ro.Cells("TAGIMAGE").Value = My.Resources.EmptyImage
                            End If
                        End If
                        If (refreshVar) = 200 Then
                            ProgressBarStep(refreshVar.ToString + " Tag's Loaded..")
                            refreshVar = 1
                        End If
                    Next
                    gridDetailView.AllowUserToOrderColumns = True
                    For Each col As DataGridViewColumn In gridDetailView.Columns
                        col.SortMode = DataGridViewColumnSortMode.NotSortable
                    Next
                    tabMain.SelectedTab = tabGridView
                    gridDetailView.Select()
                End If
            Else
                'Dim dtGrid As New DataTable
                'da = New OleDbDataAdapter(strSql, cn)
                'da.Fill(dtGrid)
                'If Not dtGrid.Rows.Count > 0 Then
                '    MsgBox("Record not found", MsgBoxStyle.Information)
                '    Exit Sub
                'End If
                'Dim dtGridResult As New DataTable
                'dtGridResult = dtGrid.Clone
                'dtGridResult.Columns.Add("SLNO", GetType(String))
                'dtGridResult.Columns("SLNO").SetOrdinal(0)
                'For Each col As DataColumn In dtGridResult.Columns
                '    col.DataType = GetType(String)
                'Next

                'Dim RoDest As DataRow = Nothing
                'Dim RoSource As DataRow = Nothing
                'Dim TagSno As String = Nothing
                'Dim SlNo As Integer = 0
                'For i As Integer = 0 To dtGrid.Rows.Count - 1
                '    RoSource = dtGrid.Rows(i)
                '    If TagSno <> RoSource.Item("SNO").ToString Then
                '        dtGridResult.Rows.Add()
                '        RoDest = dtGridResult.Rows(SlNo)
                '        SlNo += 1
                '        If i <> dtGrid.Rows.Count - 1 Then RoDest.Item("SLNO") = SlNo
                '        TagSno = RoSource.Item("SNO").ToString
                '        For j As Integer = 0 To dtGrid.Columns.Count - 1
                '            RoDest.Item(dtGrid.Columns(j).ColumnName) = RoSource.Item(j)

                '        Next
                '    Else
                '        For j As Integer = 0 To dtGrid.Columns.Count - 1
                '            'If RoSource.Item(j).ToString <> "" Then 
                '            RoDest.Item(dtGrid.Columns(j).ColumnName) = RoDest.Item(dtGrid.Columns(j).ColumnName).ToString + vbCrLf + RoSource.Item(j).ToString
                '        Next
                '    End If
                'Next

                dtGridResult.AcceptChanges()
                '221
                If Not dtGridResult.Rows.Count > 0 Then
                    MsgBox("Records not found.", MsgBoxStyle.Information)
                    Exit Sub
                End If
                '221
                objGridShower = New frmGridDispDia
                objGridShower.Name = Me.Name
                objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
                objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
                objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                objGridShower.Text = "CATELOG VIEW REPORT"
                Dim tit As String = Nothing
                tit += "CATELOG VIEW REPORT"
                If txtItem.Text <> "" Then tit += " OF " & txtItem.Text + "'s "
                ' tit += " AS ON " & dtpAsOnDate.Text
                objGridShower.lblTitle.Text = tit
                'AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ ColumnWidthChanged
                objGridShower.StartPosition = FormStartPosition.CenterScreen

                objGridShower.gridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True
                objGridShower.gridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft

                objGridShower.gridView.DataSource = dtGridResult
                objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
                objGridShower.FormReSize = True
                objGridShower.FormReLocation = True
                objGridShower.pnlFooter.Visible = False
                objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                DataGridView_SummaryFormatting(objGridShower.gridView, count)
                objGridShower.Show()
                objGridShower.FormReSize = True
                objGridShower.gridViewHeader.Visible = True
                ' GridViewHeaderCreator(objGridShower.gridViewHeader, count)
                'Prop_Sets()
            End If
            ProgressBarStep("Loading..")

            'tabGridView.Show()
            'Dim refreshVar As Integer = 0
            'Dim bmp As Bitmap = Nothing
            'For cnt As Integer = 0 To dtImagePath.Rows.Count - 1
            '    dgvImageCell = New DataGridViewImageCell
            '    dgvImageCell.ImageLayout = DataGridViewImageCellLayout.Stretch
            '    If dtImagePath.Rows(cnt).Item("PCTFILE").ToString <> "" Then
            '        Dim serverPath As String = Nothing
            '        Dim fileDestPath As String = dtImagePath.Rows(cnt).Item("PCTFILE").ToString
            '        If IO.File.Exists(fileDestPath) Then
            '            Dim Finfo As IO.FileInfo
            '            Finfo = New IO.FileInfo(fileDestPath)
            '            'Finfo.IsReadOnly = False
            '            If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
            '                dgvImageCell.Value = bmap
            '            Else
            '                Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, FileAccess.Read)
            '                dgvImageCell.Value = Bitmap.FromStream(fileStr)
            '                fileStr.Close()
            '            End If
            '        Else
            '            dgvImageCell.Value = bmap
            '        End If
            '        'dgvImageCell.Value = Bitmap.FromFile(dtImagePath.Rows(colindex).Item("PATH").ToString)
            '    Else
            '        dgvImageCell.Value = bmap
            '    End If
            '    gridDetailView.Rows(cnt).Cells("TAGIMAGE") = dgvImageCell
            '    If (cnt - refreshVar) = 200 Then
            '        ProgressBarStep(cnt.ToString + " Tag's Loaded..")
            '        refreshVar = cnt
            '    End If
            'Next
            'gridDetailView.AllowUserToOrderColumns = True
            'For Each col As DataGridViewColumn In gridDetailView.Columns
            '    col.SortMode = DataGridViewColumnSortMode.NotSortable
            'Next
            'tabMain.SelectedTab = tabGridView
            'gridDetailView.Select()

        Catch ex As Exception
            ProgressBarHide()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            ProgressBarHide()
            btnSearch.Enabled = True
        End Try
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView, ByVal count As Integer)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            '.Columns("SNO").Visible = False
            '.Columns("SLNO").HeaderText = "SNO"
            For i As Integer = 0 To count - 1
                Dim J As Integer = 7 + i * 5
                .Columns(J + 1).HeaderText = "ITEM"
                .Columns(J + 2).HeaderText = "SUBITEM"
                .Columns(J + 3).HeaderText = "PCS"
                .Columns(J + 4).HeaderText = "WEIGHT"
                .Columns(J + 5).HeaderText = "SEIVE"
            Next

            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
    Private Sub frmTagCatalog_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                If pnlFooter.Visible = False Then
                    pnlFooter.Visible = True
                    pnlRightPart.Visible = True
                    AutoImageSizer(picImage.Image, picImage, PictureBoxSizeMode.CenterImage)
                    SetLocation()
                    dgvFooter.Focus()
                    Exit Sub
                End If
            End If
            If tabMain.SelectedTab.Name <> tabGeneral.Name Then
                tabMain.SelectedTab = tabGeneral
                pnlContainer.Focus()
            End If
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            If dgvFooter.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagCatalog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not defaultPic.EndsWith("\") And defaultPic <> "" Then defaultPic += "\"

        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        pnlContainer.Location = New Point((ScreenWid - pnlContainer.Width) / 2, ((ScreenHit - 128) - pnlContainer.Height) / 2)

        If Not defaultPic.StartsWith("\") Then defaultPic += "\"



        With dtImagePath.Columns
            .Add("STYLENO", GetType(String))
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("PCS", GetType(Integer))
            .Add("GRSWT", GetType(Decimal))
            .Add("NETWT", GetType(Decimal))
            .Add("DIAPCS", GetType(Integer))
            .Add("DIAWT", GetType(Decimal))
            .Add("ITEMTYPE", GetType(String))
            .Add("SIZEDESC", GetType(String))
            .Add("NARRATION", GetType(String))
            .Add("PCTFILE", GetType(String))
            .Add("PICNAME", GetType(String))
            .Add("SNO", GetType(String))
            .Add("ITEMID", GetType(Integer))
        End With
        'strSql = " SELECT TAGNO,''ITEMNAME,PCS,GRSWT,NETWT,SALVALUE,DIAPCS "
        'strSql += " ,DIAWT,PICFILENAME DESIGNER,PICFILENAME PATH,PCTFILE PICNAME,"
        'strSql += " ITEMID FROM " & cnAdminDb & "..ITEMTAG AS T"
        'strSql += " WHERE 1 <> 1"
        dtImagePath.Clear()
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtImagePath)
        gridDetailView.DataSource = dtImagePath
        dgvImageColumn = New DataGridViewImageColumn
        dgvImageColumn.Name = "TAGIMAGE"
        dgvImageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch
        'dgvImageColumn.Width = 50
        gridDetailView.RowTemplate.Height = 120
        gridDetailView.Columns.Insert(0, dgvImageColumn)
        With gridDetailView
            .Columns("TAGIMAGE").Width = .RowTemplate.Height
            With .Columns("STYLENO")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("ITEM")
                .Width = 180
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("SUBITEM")
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("PCS")
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("GRSWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.000"
            End With
            With .Columns("NETWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.000"
            End With
            .Columns("ITEMTYPE").Width = 100
            .Columns("SIZEDESC").Width = 100
            With .Columns("DIAPCS")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("DIAWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.000"
            End With
            With .Columns("NARRATION")
                .Width = 150
            End With
            With .Columns("PCTFILE")
                .Width = 135
            End With
            .Columns("SNO").Visible = False
            .Columns("ITEMID").Visible = False
            .Columns("PICNAME").Visible = False
        End With


        ''Load Item Name
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T' "
        strSql += GetItemQryFilteration("S")
        strSql += " ORDER BY ITEMNAME"
        funcLoadCheckedList(chkLstItem, strSql)

        ''Load Stone Item Name
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN ('D','T') "
        strSql += GetItemQryFilteration("S")
        strSql += " ORDER BY ITEMNAME"
        funcLoadCheckedList(chkLstStoneItem, strSql)

        funcNew()
    End Sub

    Private Sub gridDetailView_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridDetailView.CellDoubleClick
        'If gridDetailView.RowCount - 1 Then
        '    Dim objTagViewer As New frmTagImageViewer(gridDetailView.Rows(gridDetailView.CurrentRow.Index).Cells("TAGNO").Value.ToString, gridDetailView.Rows(gridDetailView.CurrentRow.Index).Cells("ITEMID").Value.ToString)
        '    objTagViewer.ShowDialog()
        'End If
    End Sub

    Private Sub gridDetailView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridDetailView.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridDetailView.RowCount > 0 Then
                gridDetailView_CellDoubleClick(Me, New DataGridViewCellEventArgs(gridDetailView.CurrentCell.ColumnIndex, gridDetailView.CurrentCell.RowIndex))
                gridDetailView.CurrentCell = gridDetailView.Rows(gridDetailView.CurrentRow.Index).Cells(0)
                e.Handled = True
            End If
        End If

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub txtItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItem.KeyDown
        If e.KeyCode = Keys.Down Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtSubItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSubItem.KeyDown
        If e.KeyCode = Keys.Down Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtItem_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItem.TextChanged
        SearchItem(chkLstItem, sender)
    End Sub

    Private Sub txtSubItem_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubItem.TextChanged
        SearchItem(chkLstSubItem, sender)
    End Sub

    Private Sub chkLstItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkLstItem.KeyPress
        Select Case e.KeyChar
            Case Chr(Keys.Enter)
                strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN ((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & funcGetCheckedVal(chkLstItem) & ")))"
                If _SubItemOrderByName Then
                    strSql += " ORDER BY SUBITEMNAME"
                Else
                    strSql += " ORDER BY DISPLAYORDER,SUBITEMNAME"
                End If
                funcLoadCheckedList(chkLstSubItem, strSql)
                If chkLstSubItem.Items.Count > 0 Then
                    txtSubItem.Enabled = True
                    chkLstSubItem.Enabled = True
                Else
                    txtSubItem.Enabled = False
                    chkLstSubItem.Enabled = False
                End If
        End Select
    End Sub

    Private Sub chkLstItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstItem.LostFocus
        chkLstItem.SetSelected(0, False)
    End Sub

    Private Sub chkLstSubItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstSubItem.LostFocus
        chkLstSubItem.SetSelected(0, False)
    End Sub

    Private Sub chkLstStoneItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkLstStoneItem.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN ((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & funcGetCheckedVal(chkLstStoneItem) & ")))"
            If _SubItemOrderByName Then
                strSql += " ORDER BY SUBITEMNAME"
            Else
                strSql += " ORDER BY DISPLAYORDER,SUBITEMNAME"
            End If
            funcLoadCheckedList(chkLstStoneSubItem, strSql)
            If chkLstStoneSubItem.Items.Count > 0 Then
                chkLstStoneSubItem.Enabled = True
            Else
                chkLstStoneSubItem.Enabled = False
            End If
        End If
    End Sub

    Private Sub chkLstStoneItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstStoneItem.LostFocus
        chkLstStoneItem.SetSelected(0, False)
    End Sub

    Private Sub chkLstStoneSubItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstStoneSubItem.LostFocus
        chkLstStoneSubItem.SetSelected(0, False)
    End Sub

    Private Sub txtGrsWtTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrsWtTo_Wet.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtGrsWtFrom_Wet.Text) > 0 Then
                If txtGrsWtTo_Wet.Text = "" Then
                    txtGrsWtTo_Wet.Text = txtGrsWtFrom_Wet.Text
                    Exit Sub
                End If
                If Not Val(txtGrsWtFrom_Wet.Text) <= Val(txtGrsWtTo_Wet.Text) Then
                    MsgBox("To value should not Exceed from value", MsgBoxStyle.Information)
                    txtGrsWtTo_Wet.Focus()
                    txtGrsWtTo_Wet.SelectAll()
                End If
            ElseIf Val(txtGrsWtTo_Wet.Text) > 0 Then
                txtGrsWtFrom_Wet.Text = 1
            End If
        End If
    End Sub

    Private Sub txtNetWtTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNetWtTo_Wet.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtNetWtFrom_Wet.Text) > 0 Then
                If txtNetWtTo_Wet.Text = "" Then
                    txtNetWtTo_Wet.Text = txtNetWtFrom_Wet.Text
                    Exit Sub
                End If
                If Not Val(txtNetWtFrom_Wet.Text) <= Val(txtNetWtTo_Wet.Text) Then
                    MsgBox("To value should not Exceed from value", MsgBoxStyle.Information)
                    txtNetWtTo_Wet.Focus()
                    txtNetWtTo_Wet.SelectAll()
                End If
            ElseIf Val(txtNetWtTo_Wet.Text) > 0 Then
                txtNetWtFrom_Wet.Text = 1
            End If
        End If
    End Sub

    Private Sub txtDiaWtTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDiaWtTo_Wet.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtDiaWtFrom_Wet.Text) > 0 Then
                If txtDiaWtTo_Wet.Text = "" Then
                    txtDiaWtTo_Wet.Text = txtDiaWtFrom_Wet.Text
                    Exit Sub
                End If
                If Not Val(txtDiaWtFrom_Wet.Text) <= Val(txtDiaWtTo_Wet.Text) Then
                    MsgBox("To value should not Exceed from value", MsgBoxStyle.Information)
                    txtDiaWtTo_Wet.Focus()
                    txtDiaWtTo_Wet.SelectAll()
                End If
            ElseIf Val(txtDiaWtTo_Wet.Text) > 0 Then
                txtDiaWtFrom_Wet.Text = 1
            End If
        End If

    End Sub

    Private Sub txtStnWtTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStnWtTo_Wet.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtStnWtFrom_Wet.Text) > 0 Then
                If txtStnWtTo_Wet.Text = "" Then
                    txtStnWtTo_Wet.Text = txtStnWtFrom_Wet.Text
                    Exit Sub
                End If
                If Not Val(txtStnWtFrom_Wet.Text) <= Val(txtStnWtTo_Wet.Text) Then
                    MsgBox("To value should not Exceed from value", MsgBoxStyle.Information)
                    txtStnWtTo_Wet.Focus()
                    txtStnWtTo_Wet.SelectAll()
                End If
            ElseIf Val(txtStnWtTo_Wet.Text) > 0 Then
                txtStnWtFrom_Wet.Text = 0.0001
            End If
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub cmbImageViewStyle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridDetailView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Tag Catalog", gridDetailView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridDetailView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Tag Catalog", gridDetailView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub dgvFooter_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvFooter.CellEnter
        With dtImagePath.Rows(e.ColumnIndex - 1)
            lblCatSno.Text = .Item("SNO").ToString
            lblStyleNo.Text = .Item("STYLENO").ToString
            lblItemId.Text = .Item("ITEMID").ToString
            lblItemName.Text = .Item("ITEM").ToString
            lblSubItem.Text = .Item("SUBITEM").ToString
            lblItemType.Text = .Item("ITEMTYPE").ToString
            lblPcsWt.Text = "P-" & .Item("PCS").ToString & "/W-" & .Item("GRSWT").ToString
            lblDiaPcsWt.Text = "P-" & .Item("DIAPCS").ToString & "/W-" & .Item("DIAWT").ToString

            Dim dv As DataView
            dv = dtCatalogStone.DefaultView
            dv.RowFilter = "TAGSNO = '" & .Item("SNO").ToString & "'"
            dgvStoneDetail.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            dgvStoneDetail.DataSource = dv
            If dgvStoneDetail.RowCount > 0 Then
                dgvStoneDetail.Visible = True
                dgvStoneDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                dgvStoneDetail.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dgvStoneDetail.Columns("WEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dgvStoneDetail.Columns("TAGSNO").Visible = False
            Else
                dgvStoneDetail.Visible = False
            End If
        End With
    End Sub

    Private Sub dgvFooter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvFooter.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub dgvFooter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgvFooter.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            dgvFooter.CurrentCell = dgvFooter.Rows(0).Cells(dgvFooter.CurrentCell.ColumnIndex)
            pnlFooter.Visible = False
            pnlRightPart.Visible = False
            AutoImageSizer(dgvFooter.CurrentCell.Tag.ToString, picImage, PictureBoxSizeMode.AutoSize)
            SetLocation()
        ElseIf UCase(e.KeyChar) = "S" Then
            dgvFooter.CurrentCell = dgvFooter.Rows(0).Cells(dgvFooter.CurrentCell.ColumnIndex)
            PrintCatalog(dgvFooter.CurrentCell.Value.ToString)
        End If
    End Sub

    Private Sub dgvFooter_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles dgvFooter.Scroll
        dgv.HorizontalScrollingOffset = e.NewValue
    End Sub

    Private Sub dgvFooter_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvFooter.SelectionChanged
        If Not dgv.Columns.Count > 0 Then Exit Sub
        If Not dgvFooter.Columns.Count > 0 Then Exit Sub
        If Not dgv.Columns.Count = dgvFooter.ColumnCount Then Exit Sub
        If dgvFooter.CurrentCell Is Nothing Then Exit Sub
        dgvFooter.CurrentCell = dgvFooter.Rows(0).Cells(dgvFooter.CurrentCell.ColumnIndex)
        dgv.CurrentCell = dgv.Rows(0).Cells(dgvFooter.CurrentCell.ColumnIndex)
        If dgvFooter.CurrentCell.Tag Is Nothing Then
            picImage.Image = Nothing
            SetLocation()
            Exit Sub
        End If
        AutoImageSizer(dgvFooter.CurrentCell.Tag.ToString, picImage, PictureBoxSizeMode.CenterImage)
    End Sub

    Private Sub dgv_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv.CellClick
        dgvFooter.CurrentCell = dgvFooter.Rows(0).Cells(e.ColumnIndex)
        dgvFooter_SelectionChanged(Me, New EventArgs)
        dgvFooter.Focus()
    End Sub
    Private Sub SetLocation()
        picImage.Invalidate()
        Dim x As Integer = 0
        If pnlRightPart.Visible Then
            x = (pnlImage.Width - pnlRightPart.Width - picImage.Width) / 2
        Else
            x = (pnlImage.Width - picImage.Width) / 2
        End If
        Dim y As Integer = ((pnlImage.Height) - picImage.Height) / 2
        If pnlImage.Width <= picImage.Width Then x = 0
        If pnlImage.Height <= picImage.Height Then y = 0
        picImage.Location = New Point(x, y)
    End Sub

    Private Sub pnlImage_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnlImage.Resize
        SetLocation()
    End Sub

    Private Sub MultiImageViewer_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        SetLocation()
    End Sub

    Private Sub gridDetailView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridDetailView.KeyPress
        If UCase(e.KeyChar) = "E" Then
            If gridDetailView.CurrentRow Is Nothing Then Exit Sub
            Dim objCatalog As New TagCatalog(gridDetailView.Rows(gridDetailView.CurrentRow.Index).Cells("SNO").Value.ToString)
            objCatalog.ShowDialog()
        ElseIf UCase(e.KeyChar) = "S" Then
            PrintCatalogSno(gridDetailView.Rows(gridDetailView.CurrentRow.Index).Cells("SNO").Value.ToString)
        End If
    End Sub

    Private Sub btnAddCart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShopCart.Click
        If cmbOrderName_OWN.Text = "" Then
            MsgBox("Please Enter OrderName", MsgBoxStyle.Information)
            cmbOrderName_OWN.Text = ""
            cmbOrderName_OWN.Select()
            Exit Sub
        End If
        strSql = " INSERT INTO " & cnStockDb & "..STYLECARD"
        strSql += " ("
        strSql += " CARTNAME,STYLENO,USERID,UPDATED,UPTIME,SID,CATSNO"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & cmbOrderName_OWN.Text & "'"
        strSql += " ,'" & lblStyleNo.Text & "'"
        strSql += " ," & userId & ""
        strSql += " ,'" & GetEntryDate(GetServerDate) & "'"
        strSql += " ,'" & GetServerTime() & "'"
        strSql += " ," & Val(objGPack.GetMax("SID", "STYLECARD", cnStockDb)) & ""
        strSql += " ,'" & lblCatSno.Text & "'"
        strSql += " )"
        Cmd = New OleDbCommand(strSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        If cmbOrderName_OWN.Items.Contains(cmbOrderName_OWN.Text) = False Then cmbOrderName_OWN.Items.Add(cmbOrderName_OWN.Text)
        MsgBox("Successfully Added", MsgBoxStyle.Information)
        dgvFooter.Select()
    End Sub
End Class
