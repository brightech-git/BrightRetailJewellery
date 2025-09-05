Imports System.Data.OleDb
Imports System.IO

Public Class frmTagImage
    Dim bmap As New Bitmap(My.Resources.no_photo)
    Dim strSql As String
    Dim dtImagePath As New DataTable
    Dim dtTemp As New DataTable
    Dim dtimagenew As New DataTable

    Dim dgvImageColumn As DataGridViewImageColumn
    Dim dgvTextColumn As DataGridViewTextBoxColumn
    Dim dgvImageCell As DataGridViewImageCell
    Dim dgvTextCell As DataGridViewTextBoxCell
    Dim myImageListSmall As New ImageList
    Dim myImageListLarge As New ImageList
    Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")
    Dim dgvchkbok As DataGridViewCheckBoxColumn
    Dim dgvchkcell As DataGridViewCheckBoxCell
    Dim myImagelargeview As New ImageList
    Public dtimage As DataTable
    Dim cmd As OleDbCommand
    Dim _MCONWTAMT As Boolean = IIf(GetAdmindbSoftValue("MC_ON_WTAMT", "W") = "W", True, False)
    Dim _MCONGRSNET As Boolean = IIf(GetAdmindbSoftValue("MC_ON_GRSNET", "Y") = "Y", True, False)
    Dim _McWithWastage As Boolean = IIf(GetAdmindbSoftValue("MCWITHWASTAGE", "N") = "Y", True, False)
    Dim _FourCMaintain As Boolean = IIf(GetAdmindbSoftValue("4CMAINTAIN", "Y") = "Y", True, False)
    Dim PUR_LANDCOST As Boolean = IIf(GetAdmindbSoftValue("PUR_LANDCOST", "N").ToUpper = "Y", True, False)
    Dim _MCCALCON_ITEM_GRS As Boolean = False
    Dim dt As New DataTable
    Dim notify As NotifyIcon
    Dim ShowPurchaseDetail As Boolean = True
    Dim dtchk As DataTable
    Dim Save As Boolean = False
    Dim Authorize As Boolean = False
    Dim Itemmast_PctPath As Boolean = IIf(GetAdmindbSoftValue("PICPATHFROM", "S") = "I", True, False)

    Function funcGetTagVal() As String
        Dim tag As String = Nothing
        For cnt As Integer = 0 To lstTagNo.Items.Count - 1
            tag += "'" + lstTagNo.Items.Item(cnt).ToString + "',"
        Next
        If tag <> Nothing Then
            tag = tag.Remove(tag.Length - 1, 1)
        Else
            tag = "''"
        End If
        Return tag
    End Function
    Function funcFiltration() As String
        Dim Qry As String = Nothing
        If rbtStockView.Checked Then
            Qry += " AND RECDATE <= '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND ISSDATE IS NULL"
        ElseIf rbtReceiptView.Checked Then
            Qry += " AND RECDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        ElseIf rbtIssueView.Checked Then
            Qry += " AND ISSDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        ElseIf rbtSalesView.Checked Then
            Qry += " AND BATCHNO IN (SELECT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' AND TRANTYPE = 'SA')"
        End If
        If cmbCostCentre.Enabled Then
            If cmbCostCentre.Text <> "" Then
                Qry += " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "')"
            End If
        End If
        If cmbCounter.Text <> "" Then
            Qry += " AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter.Text & "')"
        End If
        If txtTagNo.Text <> "" Then
            Qry += " AND TAGNO IN (" & funcGetTagVal() & ")"
        End If
        If lstTagNo.Items.Count > 0 Then
            Qry += " AND TAGNO IN (" & funcGetTagVal() & ")"
        End If
        If chkLstDesigner.CheckedItems.Count > 0 Then
            Qry += " AND DESIGNERID IN (" & funcGetCheckedDesignerVal(chkLstDesigner) & ")"
        End If
        If chkLstItem.CheckedItems.Count > 0 Then
            Qry += " AND ITEMID IN (" & funcGetCheckedVal(chkLstItem) & ")"
        Else
            If cmbMetal.Text <> "ALL" Then
                Qry += " AND itemid in (select itemid from " & cnAdminDb & "..itemmast where metalid in (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetal.Text & "'))"
            End If
        End If
        If chkLstSubItem.CheckedItems.Count > 0 Then
            Qry += " AND SUBITEMID IN (" & funcGetCheckedsubitemVal(chkLstSubItem) & ")"
        End If
        If Val(txtGrsWtFrom_Wet.Text) > 0 And Val(txtGrsWtTo_Wet.Text) > 0 Then
            Qry += " AND GRSWT BETWEEN " & Val(txtGrsWtFrom_Wet.Text) & " AND " & Val(txtGrsWtTo_Wet.Text) & ""
        End If
        If Val(txtNetWtFrom_Wet.Text) > 0 And Val(txtNetWtTo_Wet.Text) > 0 Then
            Qry += " AND NETWT BETWEEN " & Val(txtNetWtFrom_Wet.Text) & " AND " & Val(txtNetWtTo_Wet.Text) & ""
        End If
        If Val(txtSaleValueFrom_Amt.Text) > 0 And Val(txtSaleValueTo_Amt.Text) > 0 Then
            Qry += " AND SALVALUE BETWEEN " & Val(txtSaleValueFrom_Amt.Text) & " AND " & Val(txtSaleValueTo_Amt.Text) & ""
        End If
        If Val(txtDiaWtFrom_Wet.Text) > 0 And Val(txtDiaWtTo_Wet.Text) > 0 Then
            Qry += " AND DIAWT BETWEEN " & Val(txtDiaWtFrom_Wet.Text) & " AND " & Val(txtDiaWtTo_Wet.Text) & ""
        End If
        If chkLstStoneItem.CheckedItems.Count > 0 Then
            Qry += " AND SNO IN (SELECT TAGSNO FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE STNITEMID IN (" & funcGetCheckedVal(chkLstStoneItem) & "))"
        End If
        If chkLstStoneSubItem.CheckedItems.Count > 0 Then
            Qry += " AND SNO IN (SELECT TAGSNO FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE STNSUBITEMID IN (" & funcGetCheckedVal(chkLstStoneSubItem) & "))"
        End If
        If Val(txtStnWtFrom_Wet.Text) > 0 And Val(txtStnWtTo_Wet.Text) > 0 Then
            Qry += " AND SNO IN (SELECT TAGSNO FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE CONVERT(NUMERIC(15,3),CASE WHEN STONEUNIT = 'C' THEN STNWT /5 ELSE STNWT END) BETWEEN " & txtStnWtFrom_Wet.Text & " AND " & txtStnWtTo_Wet.Text & " )"
        End If
        If Not cnCentStock Then Qry += " AND COMPANYID = '" & GetStockCompId() & "'"
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
    Private Sub SearchsubItem(ByRef obj As CheckedListBox, ByRef txt As TextBox)
        If txt.Text = "" Then
            obj.SetSelected(0, False)
            Exit Sub
        End If
        For cnt As Integer = 0 To chkLstSubItem.Items.Count - 1
            If UCase(obj.Items.Item(cnt).ToString).StartsWith(UCase(txt.Text)) Then
                obj.SelectedIndex = cnt
                'obj.SetSelected(cnt, True)
                Exit For
            End If
        Next
    End Sub
    Private Sub SearchDesigner(ByRef obj As CheckedListBox, ByRef txt As TextBox)
        If txt.Text = "" Then
            obj.SetSelected(0, False)
            Exit Sub
        End If
        For cnt As Integer = 0 To chkLstDesigner.Items.Count - 1
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
        dtTemp.Clear()
        Qry = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & str & ")"
        da = New OleDbDataAdapter(Qry, cn)
        dtTemp = New DataTable
        da.Fill(dtTemp)
        str = Nothing
        For CNT As Integer = 0 To dtTemp.Rows.Count - 1
            str += "'" + dtTemp.Rows(CNT).Item(0).ToString + "',"
        Next
        If str = Nothing Then
            str = "''"
        Else
            str = str.Remove(str.Length - 1, 1)
        End If
        Return str
    End Function
    Function funcGetCheckedDesignerVal(ByVal obj As CheckedListBox) As String
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
        dtTemp.Clear()
        Qry = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & str & ")"
        da = New OleDbDataAdapter(Qry, cn)
        dtTemp = New DataTable
        da.Fill(dtTemp)
        str = Nothing
        For CNT As Integer = 0 To dtTemp.Rows.Count - 1
            str += "'" + dtTemp.Rows(CNT).Item(0).ToString + "',"
        Next
        If str = Nothing Then
            str = "''"
        Else
            str = str.Remove(str.Length - 1, 1)
        End If
        Return str
    End Function
    Function funcGetCheckedsubitemVal(ByVal obj As CheckedListBox) As String
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
        dtTemp.Clear()
        Qry = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & str & ")"
        da = New OleDbDataAdapter(Qry, cn)
        dtTemp = New DataTable
        da.Fill(dtTemp)
        str = Nothing
        For CNT As Integer = 0 To dtTemp.Rows.Count - 1
            str += "'" + dtTemp.Rows(CNT).Item(0).ToString + "',"
        Next
        If str = Nothing Then
            str = "''"
        Else
            str = str.Remove(str.Length - 1, 1)
        End If
        Return str
    End Function

    Function funcNew() As Integer
        objGPack.TextClear(Me)
        rbtStockView.Checked = True
        dtpFrom.Value = GetEntryDate(GetServerDate(tran), tran)
        dtpTo.Value = GetEntryDate(GetServerDate(tran), tran)
        cmbCostCentre.Text = ""
        cmbCounter.Text = ""
        'cmbImageViewStyle.Text = "DETAIL VIEW"
        cmbImageViewStyle.Text = "TILE"
        chkLstSubItem.Items.Clear()
        txtSubItem.Enabled = False
        chkLstSubItem.Enabled = False
        lstTagNo.Items.Clear()
        chkLstStoneSubItem.Items.Clear()
        chkLstStoneSubItem.Enabled = False
        txtSubItem.BackColor = System.Drawing.SystemColors.Window
        cmbMetal.Text = "ALL"
        Chksalvalue.Checked = False
        Chksalvalue.Enabled = False
        ChkPctPath.Checked = False
        ChkPctPath.Enabled = False
        rbtStockView.Select()
        Prop_Gets()
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
    Function funcFilmstripStyle() As Integer
        Dim dtgridview As New DataTable

        Dim imagecellwid As Integer = 100
        Dim imageindex As Integer = 0


        For cnt As Integer = 0 To dtImagePath.Rows.Count - 1
            ''Adding Image Col
            dgvImageColumn = New DataGridViewImageColumn
            dgvImageColumn.Image = bmap
            dgvImageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom
            gridImageView.Columns.Add(dgvImageColumn)
            gridImageView.Columns(cnt).Width = imagecellwid
            gridImageView.Columns(cnt).Resizable = DataGridViewTriState.False

            ''Add TextBox Col
            dgvTextColumn = New DataGridViewTextBoxColumn
            gridTagDetail.Columns.Add(dgvTextColumn)
            gridTagDetail.Columns(cnt).Width = imagecellwid
            gridTagDetail.Columns(cnt).Resizable = DataGridViewTriState.False
        Next
        gridImageView.RowTemplate.Height = imagecellwid
        gridTagDetail.RowTemplate.Height = 15



        gridImageView.Rows.Add()
        gridTagDetail.Rows.Add() 'Row 0 For TagNo
        gridTagDetail.Rows.Add() 'Row 1 For ItemId
        gridTagDetail.Rows.Add() 'Row 2 For GrsWt
        gridTagDetail.Rows.Add() 'Row 3 For NetWt
        For colindex As Integer = 0 To dtImagePath.Rows.Count - 1
            dgvImageCell = New DataGridViewImageCell
            dgvImageCell.ImageLayout = DataGridViewImageCellLayout.Stretch
            If dtImagePath.Rows(colindex).Item("PCTFILE").ToString <> "" Then
                Dim serverPath As String = Nothing
                Dim fileDestPath As String = dtImagePath.Rows(colindex).Item("PCTFILE").ToString
                If IO.File.Exists(fileDestPath) Then
                    Dim Finfo As IO.FileInfo
                    Finfo = New IO.FileInfo(fileDestPath)
                    'Finfo.IsReadOnly = False
                    If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                        dgvImageCell.Value = bmap
                    Else
                        Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, FileAccess.Read)
                        dgvImageCell.Value = Bitmap.FromStream(fileStr)
                        fileStr.Close()
                    End If
                Else
                    dgvImageCell.Value = bmap
                End If
                'dgvImageCell.Value = Bitmap.FromFile(dtImagePath.Rows(colindex).Item("PATH").ToString)
            Else
                dgvImageCell.Value = bmap
            End If
            gridImageView.Rows(0).Cells(colindex) = dgvImageCell

            ''Adding TagNo
            dgvTextCell = New DataGridViewTextBoxCell
            dgvTextCell.Value = dtImagePath.Rows(colindex).Item("TAGNO").ToString
            gridTagDetail.Rows(0).Cells(colindex) = dgvTextCell

            ''Adding ITEMID
            dgvTextCell = New DataGridViewTextBoxCell
            dgvTextCell.Value = dtImagePath.Rows(colindex).Item("ITEMID").ToString
            gridTagDetail.Rows(1).Cells(colindex) = dgvTextCell
            ''Adding GRSWT
            dgvTextCell = New DataGridViewTextBoxCell
            dgvTextCell.Value = dtImagePath.Rows(colindex).Item("GRSWT").ToString
            gridTagDetail.Rows(2).Cells(colindex) = dgvTextCell
            ''Adding NETWT
            dgvTextCell = New DataGridViewTextBoxCell
            dgvTextCell.Value = dtImagePath.Rows(colindex).Item("NETWT").ToString
            gridTagDetail.Rows(3).Cells(colindex) = dgvTextCell
            imageindex += 1
        Next
        gridTagDetail.Rows(1).Visible = False
        gridTagDetail.Rows(2).Visible = False
        gridTagDetail.Rows(3).Visible = False
        If gridTagDetail.Columns.Count > 0 Then
            gridTagDetail.CurrentCell = gridTagDetail.Rows(0).Cells(0)
            gridTagDetail_SelectionChanged(Me, New EventArgs)
        End If
    End Function
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        btnSearch.Enabled = False
        If chkspecific.Checked Then
            specificformat()
            Prop_Sets()
            Exit Sub
        End If
        If chkNewformat.Checked Then
            NewFormat()
            Prop_Sets()
            Exit Sub
        End If
        Try
            btnExcel.Visible = True
            ProgressBarShow()
            dtImagePath.Rows.Clear()
            strSql = " DECLARE @DEFPATH VARCHAR(200)"
            strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
            strSql += vbCrLf + " SELECT TAGNO,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME"
            strSql += vbCrLf + " ,(SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID AND ITEMID = T.ITEMID) SUBITEMNAME "
            strSql += vbCrLf + " ,CASE WHEN PCS<> 0 THEN PCS ELSE NULL END PCS"
            strSql += vbCrLf + " ,CASE WHEN GRSWT <> 0  THEN GRSWT ELSE NULL END GRSWT"
            strSql += vbCrLf + " ,CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END NETWT"
            strSql += vbCrLf + " ,CASE WHEN SALVALUE <> 0 THEN SALVALUE ELSE NULL END SALVALUE"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS  DIAPCS "
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS  DIAWT "
            strSql += vbCrLf + " ,(SELECT TOP 1 DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
            ''strSql += vbCrLf + " ,@DEFPATH + PCTFILE PCTFILE"
            If Itemmast_PctPath Then
                strSql += vbCrLf + " , (CASE WHEN (SELECT ISNULL(ITEMPCTPATH,'') ITEMPCTPATH FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =T.ITEMID) <>'' THEN "
                strSql += vbCrLf + " (SELECT ISNULL(CASE WHEN SUBSTRING(ITEMPCTPATH,LEN(ITEMPCTPATH)-1,1 )='\' OR SUBSTRING(ITEMPCTPATH,LEN(ITEMPCTPATH)-1,1 )='/' "
                strSql += vbCrLf + " THEN ITEMPCTPATH ELSE ITEMPCTPATH+'\' END,'') ITEMPCTPATH FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =T.ITEMID) ELSE @DEFPATH END)"
                strSql += vbCrLf + " + PCTFILE PCTFILE"
            Else
                strSql += vbCrLf + " ,@DEFPATH + PCTFILE PCTFILE"
            End If
            strSql += vbCrLf + " ,PCTFILE PICNAME,ITEMID,0 SELECTED FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE 1 = 1"
            strSql += vbCrLf + funcFiltration()
            ''strSql += " ORDER BY ITEMID "
            If rbtItemidwise.Checked Then
                strSql += vbCrLf + " ORDER BY ITEMID "
            ElseIf rbtDesignerwise.Checked Then
                strSql += vbCrLf + " ORDER BY (SELECT TOP 1 DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID) "
            Else
                strSql += vbCrLf + " ORDER BY ITEMID "
            End If

            ProgressBarStep("Loading..")


            'dtImagePath.Columns.Add("CHK", GetType(Boolean))
            'dtImagePath.Columns.Add("TAGIMAGE", GetType(Image))

            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtImagePath)
            If Not dtImagePath.Rows.Count > 0 Then
                btnSearch.Enabled = True
                MsgBox(E0011, MsgBoxStyle.Information)
                Exit Sub
            End If
            Prop_Sets()
            If rbtList.Checked Then
                tabListview.Show()
                lstView.Items.Clear()
                lstView.SmallImageList = myImageListSmall
                lstView.LargeImageList = myImageListLarge
                Dim lstItem As ListViewItem
                myImageListSmall.Images.Add(bmap)
                myImageListSmall.Images.SetKeyName(0, "IMAGENOTFOUND")

                myImageListLarge.Images.Add(bmap)
                myImageListLarge.Images.SetKeyName(0, "IMAGENOTFOUND")
                Dim imageIndex As Integer = 1
                Dim refreshVar As Integer = 0

                For cnt As Integer = 0 To dtImagePath.Rows.Count - 1
                    lstItem = New ListViewItem(dtImagePath.Rows(cnt).Item("tagno").ToString)
                    lstItem.SubItems.Add(dtImagePath.Rows(cnt).Item("ITEMID").ToString)
                    lstView.Items.Add(lstItem)
                    If dtImagePath.Rows(cnt).Item("PCTFILE").ToString <> "" Then
                        Dim serverPath As String = Nothing
                        Dim fileDestPath As String = dtImagePath.Rows(cnt).Item("PCTFILE").ToString
                        If IO.File.Exists(fileDestPath) Then
                            Dim Finfo As IO.FileInfo
                            Finfo = New IO.FileInfo(fileDestPath)
                            'Finfo.IsReadOnly = False
                            If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                                dgvImageCell.Value = bmap
                            Else
                                Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, FileAccess.Read)
                                myImageListSmall.Images.Add(Bitmap.FromStream(fileStr))
                                myImageListSmall.Images.SetKeyName(imageIndex, dtImagePath.Rows(cnt).Item("PICNAME").ToString)

                                myImageListLarge.Images.Add(Bitmap.FromStream(fileStr))
                                myImageListLarge.Images.SetKeyName(imageIndex, dtImagePath.Rows(cnt).Item("PICNAME").ToString)
                                lstView.Items(cnt).ImageKey = dtImagePath.Rows(cnt).Item("PICNAME").ToString
                                imageIndex += 1
                                fileStr.Close()
                            End If
                        Else
                            lstView.Items(cnt).ImageKey = "IMAGENOTFOUND"
                        End If
                    Else
                        'myImageList.Images.Add(bmap)
                        lstView.Items(cnt).ImageKey = "IMAGENOTFOUND"
                    End If
                    If (cnt - refreshVar) = 200 Then
                        ProgressBarStep(cnt.ToString + " Tag's Loaded..")
                        refreshVar = cnt
                    End If
                Next
                tabMain.SelectedTab = tabView
                tabViewStyle.SelectedTab = tabListview
                Me.tabViewStyle.Region = New Region(New RectangleF(
                Me.tabFilmStrip.Left + 1,
                Me.tabFilmStrip.Top,
                Me.tabFilmStrip.Width,
                Me.tabFilmStrip.Height))
                lstView.Select()
            ElseIf rbtGrid.Checked Then
                tabGridView.Show()
                Dim refreshVar As Integer = 0
                Dim bmp As Bitmap = Nothing
                Dim selected As CheckBox
                For cnt As Integer = 0 To dtImagePath.Rows.Count - 1
                    dgvImageCell = New DataGridViewImageCell
                    dgvchkbok = New DataGridViewCheckBoxColumn
                    dgvImageCell.ImageLayout = DataGridViewImageCellLayout.Stretch
                    If dtImagePath.Rows(cnt).Item("PCTFILE").ToString <> "" Then
                        Dim serverPath As String = Nothing
                        Dim fileDestPath As String = dtImagePath.Rows(cnt).Item("PCTFILE").ToString
                        If IO.File.Exists(fileDestPath) Then
                            Dim Finfo As IO.FileInfo
                            Finfo = New IO.FileInfo(fileDestPath)
                            If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                                dgvImageCell.Value = bmap
                            Else
                                ''Dim newImage As Image
                                'Using ms As New MemoryStream(imageData, 0, imageData.Length)
                                '    ms.Write(imageData, 0, imageData.Length)
                                '    'Set image variable value using memory stream.
                                '    newImage = Image.FromStream(ms, True)
                                'End Using
                                'dgvImageCell.Value = Bitmap.FromStream(fileStr)

                                ''Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, FileAccess.Read)
                                ''dgvImageCell.Value = Image.FromStream(fileStr, True)
                                ''fileStr.Close()
                                Dim _temppctbox As New PictureBox
                                AutoImageSizer(fileDestPath, _temppctbox, PictureBoxSizeMode.StretchImage)
                                dgvImageCell.Value = _temppctbox.Image

                            End If
                        Else
                            dgvImageCell.Value = bmap
                        End If
                        'dgvImageCell.Value = Bitmap.FromFile(dtImagePath.Rows(colindex).Item("PATH").ToString)
                    Else
                        dgvImageCell.Value = bmap
                    End If

                    Dim chk As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn
                    gridDetailView.Rows(cnt).Cells("TAGIMAGE") = dgvImageCell
                    'gridDetailView.Rows(cnt).Cells("CHK") = 
                    If (cnt - refreshVar) = 200 Then
                        ProgressBarStep(cnt.ToString + " Tag's Loaded..")
                        refreshVar = cnt
                    End If
                Next
                gridDetailView.AllowUserToOrderColumns = True

                'For Each col As DataGridViewColumn In gridDetailView.Columns
                '    If col.Name = "CHECK" Then
                '        gridDetailView.Columns("CHECK").ReadOnly = False
                '        gridDetailView.Columns("SELECTED").ReadOnly = False
                '    Else
                '        gridDetailView.Columns("CHECK").ReadOnly = True
                '    End If
                '    col.SortMode = DataGridViewColumnSortMode.NotSortable
                'Next
                'If gridDetailView.Columns("CHECK").ReadOnly = True Then
                '    gridDetailView.Columns("CHECK").ReadOnly = False
                '    gridDetailView.Columns("SELECTED").ReadOnly = False
                'End If

                tabMain.SelectedTab = tabGridView
                gridDetailView.Select()
            End If
        Catch ex As Exception
            ProgressBarHide()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            ProgressBarHide()
            btnSearch.Enabled = True
        End Try
    End Sub


    Private Sub NewFormat()
        Try
            btnExcel.Visible = True
            gridDetailView.DataSource = Nothing
            dtImagePath = New DataTable
            With dtImagePath.Columns
                .Add("ITEMID", GetType(Integer))
                .Add("ITEM", GetType(String))
                .Add("SIZE", GetType(String))
                .Add("SUBITEM", GetType(String))
                .Add("MINWT", GetType(Double))
                .Add("MAXWT", GetType(Double))
                .Add("RECDATE", GetType(DateTime))
                .Add("TAGNO", GetType(String))
                .Add("TAGKEY", GetType(String))
                .Add("PCS", GetType(Integer))
                .Add("GRSWT", GetType(Double))
                .Add("NETWT", GetType(Double))
                .Add("MAXMCGRM", GetType(Double))
                .Add("MAXMC", GetType(Double))
                .Add("PURITY", GetType(Double))
                .Add("SALEVALUE", GetType(Double))
                .Add("BILLVALUE", GetType(Double))
                .Add("PCTFILE", GetType(String))
                .Add("PICNAME", GetType(String))
                .Add("SELECTED", GetType(Integer))
            End With
            'dtImagePath.Clear()
            gridDetailView.DataSource = dtImagePath
            If gridDetailView.Columns.Contains("TAGIMAGE") = False Then
                dgvImageColumn = New DataGridViewImageColumn
                dgvImageColumn.Name = "TAGIMAGE"
                dgvImageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom
                gridDetailView.Columns.Insert(0, dgvImageColumn)
            End If

            'dgvchkbok = New DataGridViewCheckBoxColumn
            'dgvchkbok.Name = "CHECK"
            'gridDetailView.Columns.Insert(0, dgvchkbok)

            With gridDetailView
                .Columns("TAGIMAGE").Width = .RowTemplate.Height
            End With

            ProgressBarShow()
            dtImagePath.Rows.Clear()
            strSql = " DECLARE @DEFPATH VARCHAR(200)"
            strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
            strSql += vbCrLf + " SELECT T.ITEMID,(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + " ,(SELECT TOP 1 SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID = T.SIZEID)SIZE "
            strSql += vbCrLf + " ,(SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID AND ITEMID = T.ITEMID) SUBITEM"
            strSql += vbCrLf + " ,(SELECT TOP 1 FROMWT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS MINWT"
            strSql += vbCrLf + " ,(SELECT TOP 1 TOWT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS MAXWT"
            strSql += vbCrLf + " ,RECDATE,TAGNO,TAGKEY,PCS,GRSWT,NETWT,MAXMCGRM,MAXMC,PURITY,T.SALVALUE SALEVALUE,(SELECT TOP 1 AMOUNT FROM " & cnStockDb & "..ISSUE WHERE BATCHNO=T.BATCHNO AND TAGNO=T.TAGNO)BILLVALUE"
            ''strSql += vbCrLf + " ,@DEFPATH + PCTFILE PCTFILE"
            If Itemmast_PctPath Then
                strSql += vbCrLf + " , (CASE WHEN (SELECT ISNULL(ITEMPCTPATH,'') ITEMPCTPATH FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =T.ITEMID) <>'' THEN "
                strSql += vbCrLf + " (SELECT ISNULL(CASE WHEN SUBSTRING(ITEMPCTPATH,LEN(ITEMPCTPATH)-1,1 )='\' OR SUBSTRING(ITEMPCTPATH,LEN(ITEMPCTPATH)-1,1 )='/' "
                strSql += vbCrLf + " THEN ITEMPCTPATH ELSE ITEMPCTPATH+'\' END,'') ITEMPCTPATH FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =T.ITEMID) ELSE @DEFPATH END)"
                strSql += vbCrLf + " + PCTFILE PCTFILE"
            Else
                strSql += vbCrLf + " ,@DEFPATH + PCTFILE PCTFILE"
            End If
            strSql += vbCrLf + " ,PCTFILE PICNAME,0 SELECTED FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE 1 = 1"
            strSql += funcFiltration()
            ''strSql += vbCrLf + " ORDER BY T.ITEMID "
            If rbtItemidwise.Checked Then
                strSql += " ORDER BY ITEMID "
            ElseIf rbtDesignerwise.Checked Then
                strSql += " ORDER BY (SELECT TOP 1 DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID) "
            Else
                strSql += " ORDER BY ITEMID "
            End If
            ProgressBarStep("Loading..")
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtImagePath)
            If Not dtImagePath.Rows.Count > 0 Then
                btnSearch.Enabled = True
                MsgBox(E0011, MsgBoxStyle.Information)
                Exit Sub
            End If
            Prop_Sets()
            If rbtGrid.Checked Then
                tabGridView.Show()
                Dim refreshVar As Integer = 0
                Dim bmp As Bitmap = Nothing
                Dim selected As CheckBox
                For cnt As Integer = 0 To dtImagePath.Rows.Count - 1
                    dgvImageCell = New DataGridViewImageCell
                    dgvchkbok = New DataGridViewCheckBoxColumn
                    dgvImageCell.ImageLayout = DataGridViewImageCellLayout.Stretch
                    If dtImagePath.Rows(cnt).Item("PCTFILE").ToString <> "" Then
                        Dim serverPath As String = Nothing
                        Dim fileDestPath As String = dtImagePath.Rows(cnt).Item("PCTFILE").ToString
                        If IO.File.Exists(fileDestPath) Then
                            Dim Finfo As IO.FileInfo
                            Finfo = New IO.FileInfo(fileDestPath)
                            If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                                dgvImageCell.Value = bmap
                            Else
                                ''Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, FileAccess.Read)
                                ''Dim newImage As Image
                                ''dgvImageCell.Value = Image.FromStream(fileStr, True)
                                ''fileStr.Close()
                                Dim _temppctbox As New PictureBox
                                AutoImageSizer(fileDestPath, _temppctbox, PictureBoxSizeMode.StretchImage)
                                dgvImageCell.Value = _temppctbox.Image
                            End If
                        Else
                            dgvImageCell.Value = bmap
                        End If
                    Else
                        dgvImageCell.Value = bmap
                    End If

                    Dim chk As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn
                    gridDetailView.Rows(cnt).Cells("TAGIMAGE") = dgvImageCell
                    If (cnt - refreshVar) = 200 Then
                        ProgressBarStep(cnt.ToString + " Tag's Loaded..")
                        refreshVar = cnt
                    End If
                Next

                gridDetailView.Columns("PCTFILE").Visible = False
                gridDetailView.Columns("PICNAME").Visible = False
                gridDetailView.Columns("SELECTED").Visible = False

                gridDetailView.AllowUserToOrderColumns = True
                tabMain.SelectedTab = tabGridView
                gridDetailView.Select()
            End If
        Catch ex As Exception
            ProgressBarHide()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            ProgressBarHide()
            btnSearch.Enabled = True
        End Try
    End Sub


    Private Sub gridTagDetail_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridTagDetail.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridDetailView.RowCount - 1 Then
                gridDetailView.CurrentCell = gridDetailView.CurrentCell
                Dim objTagViewer As New frmTagImageViewer(gridTagDetail.Rows(0).Cells(gridTagDetail.CurrentCell.ColumnIndex).Value.ToString, gridTagDetail.Rows(1).Cells(gridTagDetail.CurrentCell.ColumnIndex).Value.ToString)
                objTagViewer.ShowDialog()
            End If
            e.Handled = True
        End If
    End Sub

    Private Sub gridDetailView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridTagDetail.Scroll
        gridImageView.HorizontalScrollingOffset = e.NewValue
    End Sub


    Private Sub gridImageView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridImageView.Click
        gridTagDetail.CurrentCell = gridTagDetail.Rows(0).Cells(gridImageView.CurrentCell.ColumnIndex)
        gridTagDetail_SelectionChanged(Me, New EventArgs)
    End Sub

    Private Sub gridImageView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridImageView.GotFocus
        gridTagDetail.Focus()
    End Sub

    Private Sub frmTagCatalog_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name <> tabGeneral.Name Then
                tabMain.SelectedTab = tabGeneral
                pnlCatalogType.Focus()
            End If
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagCatalog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        defaultPic = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")
        If Not defaultPic.EndsWith("\") And defaultPic <> "" Then defaultPic += "\"
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate

        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        If Not defaultPic.StartsWith("\") Then defaultPic += "\"

        tabViewStyle.ItemSize = New Size(1, 1)
        Me.tabViewStyle.Region = New Region(New RectangleF(
        Me.tabFilmStrip.Left + 1,
        Me.tabFilmStrip.Top,
        Me.tabFilmStrip.Width,
        Me.tabFilmStrip.Height))

        gridDetailView.RowTemplate.Height = 80
        gridTagDetail.SelectionMode = DataGridViewSelectionMode.CellSelect
        gridTagDetail.RowTemplate.Height = 50
        myImageListSmall.ImageSize = New Size(45, 45)
        myImageListLarge.ImageSize = New Size(80, 80)
        myImagelargeview.ImageSize = New Size(250, 180)
        With dtImagePath.Columns
            .Add("TAGNO", GetType(String))
            .Add("ITEMNAME", GetType(String))
            .Add("SUBITEMNAME", GetType(String))
            .Add("PCS", GetType(Integer))
            .Add("GRSWT", GetType(Double))
            .Add("NETWT", GetType(Double))
            .Add("SALVALUE", GetType(Double))
            .Add("DIAPCS", GetType(Integer))
            .Add("DIAWT", GetType(Double))
            .Add("DESIGNER", GetType(String))
            .Add("PCTFILE", GetType(String))
            .Add("PICNAME", GetType(String))
            .Add("ITEMID", GetType(Integer))
            .Add("SELECTED", GetType(String))
            .Add("ITEMTAGNO", GetType(String))
        End With
        dtImagePath.Clear()
        gridDetailView.DataSource = dtImagePath
        dgvImageColumn = New DataGridViewImageColumn
        dgvImageColumn.Name = "TAGIMAGE"
        dgvImageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom
        gridDetailView.Columns.Insert(0, dgvImageColumn)

        'dgvchkbok = New DataGridViewCheckBoxColumn
        'dgvchkbok.Name = "CHECK"
        'gridDetailView.Columns.Insert(0, dgvchkbok)

        With gridDetailView
            .Columns("TAGIMAGE").Width = .RowTemplate.Height

            'With .Columns("CHECK")
            '    .Width = 40
            '    .HeaderText = ""
            'End With
            With .Columns("ITEMNAME")
                .Width = 180
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("PCS")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("TAGNO")
                .Width = 80
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
            With .Columns("SALVALUE")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
            End With
            With .Columns("DIAPCS")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("DIAWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.000"
            End With
            With .Columns("DESIGNER")
                .Width = 150
            End With
            With .Columns("PCTFILE")
                .Width = 135
            End With
            .Columns("PICNAME").Visible = False
            .Columns("ITEMID").Visible = False
            .Columns("SELECTED").Visible = False
        End With

        ''Load CostCentre
        If funcCheckCostCentreStatusFalse() Then
            cmbCostCentre.Enabled = False
        End If
        If cmbCostCentre.Enabled Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre, False)
        End If

        ''Loat Item Counter
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRNAME"
        objGPack.FillCombo(strSql, cmbCounter, False)

        ''LOAD METAL NAME
        strSql = " SELECT 'ALL' METALNAME,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME,2 RESULT  FROM " & cnAdminDb & "..METALMAST  "
        strSql += " ORDER BY  RESULT "
        objGPack.FillCombo(strSql, cmbMetal, False)

        ''Load Image View Style
        ''cmbImageViewStyle.Items.Add("DETAIL VIEW")
        cmbImageViewStyle.Items.Add("THUMBNAIL")
        cmbImageViewStyle.Items.Add("LIST")
        'cmbImageViewStyle.Items.Add("FILMSTRIP")
        cmbImageViewStyle.Items.Add("SMALL ICONS")
        cmbImageViewStyle.Items.Add("TILE")

        ''Load Designer
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        funcLoadCheckedList(chkLstDesigner, strSql)

        ''Load Item Name
        'strSql = " SELECT ITEMNAME +' (' +CONVERT (VARCHAR,ITEMID)+')' AS ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T' "
        strSql = " SELECT ITEMNAME  FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T' "
        strSql += GetItemQryFilteration("S")
        strSql += " ORDER BY ITEMNAME"
        funcLoadCheckedList(chkLstItem, strSql)

        ''Load Stone Item Name
        'strSql = " SELECT ITEMNAME +' (' +CONVERT (VARCHAR,ITEMID)+')' AS ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN ('D','T') "
        strSql = " SELECT ITEMNAME  FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN ('D','T') "
        strSql += GetItemQryFilteration("S")
        strSql += " ORDER BY ITEMNAME"
        funcLoadCheckedList(chkLstStoneItem, strSql)

        funcNew()
        Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
        If Authorize = False Then
            pnlCatalogType.Enabled = False
            btnSave_OWN.Enabled = False
            chkspecific.Enabled = False
            txtDesigner.Enabled = False
            chkLstDesigner.Enabled = False
        End If
    End Sub

    Private Sub gridDetailView_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridDetailView.CellDoubleClick
        If gridDetailView.RowCount - 1 Then
            Dim objTagViewer As New frmTagImageViewer(gridDetailView.Rows(gridDetailView.CurrentRow.Index).Cells("TAGNO").Value.ToString, gridDetailView.Rows(gridDetailView.CurrentRow.Index).Cells("ITEMID").Value.ToString)
            objTagViewer.ShowDialog()
        End If
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

    Private Sub lstView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstView.DoubleClick
        If lstView.Items.Count > 0 Then
            Dim objTagViewer As New frmTagImageViewer(lstView.SelectedItems(0).SubItems(0).Text, lstView.SelectedItems(0).SubItems(1).Text)
            objTagViewer.ShowDialog()
        End If
    End Sub

    Private Sub lstView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lstView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            lstView_DoubleClick(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub cmbCostCentre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCostCentre.KeyPress
        e.KeyChar = UCase(e.KeyChar)
    End Sub

    Private Sub cmbCounter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCounter.KeyPress
        e.KeyChar = UCase(e.KeyChar)
    End Sub

    Private Sub txtItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItem.KeyDown
        If e.KeyCode = Keys.Down Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtDesigner_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDesigner.KeyDown
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

    Private Sub txtDesigner_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDesigner.TextChanged
        SearchDesigner(chkLstDesigner, sender)
    End Sub

    Private Sub txtSubItem_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubItem.TextChanged
        SearchsubItem(chkLstSubItem, sender)
    End Sub

    Private Sub chkLstItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkLstItem.KeyPress
        Select Case e.KeyChar
            Case Chr(Keys.Enter)
                'strSql = " SELECT SUBITEMNAME +' (' +CONVERT (VARCHAR,SUBITEMID)+')'AS SUBITEMNAME  FROM " & cnAdminDb & "..SUBITEMMAST"
                strSql = " SELECT SUBITEMNAME   FROM " & cnAdminDb & "..SUBITEMMAST"
                If chkLstItem.CheckedItems.Count > 0 Then
                    strSql += " WHERE ITEMID IN (" & funcGetCheckedVal(chkLstItem) & ")"
                End If

                If Not _SubItemOrderByName Then
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
            strSql = " SELECT SUBITEMNAME +' (' +CONVERT (VARCHAR,SUBITEMID)+')'AS SUBITEMNAME  FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN (" & funcGetCheckedVal(chkLstStoneItem) & ")"
            If Not _SubItemOrderByName Then
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

    Private Sub chkLstDesigner_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstDesigner.LostFocus
        chkLstDesigner.SetSelected(0, False)
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

    Private Sub txtSaleValueTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSaleValueTo_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtSaleValueFrom_Amt.Text) > 0 Then
                If txtGrsWtTo_Wet.Text = "" Then
                    txtSaleValueTo_Amt.Text = txtSaleValueFrom_Amt.Text
                    Exit Sub
                End If
                If Not Val(txtSaleValueFrom_Amt.Text) <= Val(txtSaleValueTo_Amt.Text) Then
                    MsgBox("To value should not Exceed from value", MsgBoxStyle.Information)
                    txtSaleValueTo_Amt.Focus()
                    txtSaleValueTo_Amt.SelectAll()
                End If
            ElseIf Val(txtSaleValueTo_Amt.Text) > 0 Then
                txtSaleValueFrom_Amt.Text = 1
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
                txtStnWtFrom_Wet.Text = 1
            End If
        End If
    End Sub

    Private Sub gridTagDetail_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridTagDetail.SelectionChanged
        If tabViewStyle.SelectedTab.Name = tabFilmStrip.Name Then
            lblGrsWt.Text = ""
            lblNetWt.Text = ""
            If gridTagDetail.RowCount > 0 Then
                gridImageView.CurrentCell = gridImageView.Rows(0).Cells(gridTagDetail.CurrentCell.ColumnIndex)
                picTag.SizeMode = PictureBoxSizeMode.CenterImage
                If CType(gridImageView.CurrentCell.Value, Image).Size.Width > picTag.Size.Width Then
                    picTag.SizeMode = PictureBoxSizeMode.Zoom
                End If
                If CType(gridImageView.CurrentCell.Value, Image).Size.Height > picTag.Size.Height Then
                    picTag.SizeMode = PictureBoxSizeMode.Zoom
                End If
                picTag.Image = CType(gridImageView.CurrentCell.Value, Image)
                lblGrsWt.Text = "GRS WEIGHT : " + Format(Val(gridTagDetail.Rows(2).Cells(gridTagDetail.CurrentCell.ColumnIndex).Value.ToString), "0.00")
                lblNetWt.Text = "NET WEIGHT : " + Format(Val(gridTagDetail.Rows(3).Cells(gridTagDetail.CurrentCell.ColumnIndex).Value.ToString), "0.00")
            End If
        End If
    End Sub

    Private Sub txtTagNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTagNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtTagNo.Text <> "" Then
                For cnt As Integer = 0 To lstTagNo.Items.Count - 1
                    If UCase(txtTagNo.Text) = lstTagNo.Items.Item(cnt).ToString Then
                        MsgBox(E0002, MsgBoxStyle.Information)
                        txtTagNo.Focus()
                        txtTagNo.SelectAll()
                        e.Handled = True
                        Exit Sub
                    End If
                Next
                lstTagNo.Items.Add(txtTagNo.Text)
                txtTagNo.Focus()
                Exit Sub
            End If
        ElseIf e.KeyCode = Keys.Down Then
            txtTagNo.Clear()
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub lstTagNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstTagNo.KeyDown
        If e.KeyCode = Keys.Delete Then
            lstTagNo.Items.Remove(lstTagNo.SelectedItem)
        End If
    End Sub

    Private Sub txtTagNo_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo.Leave
        If txtTagNo.Text <> "" Then
            txtTagNo.Focus()
            txtTagNo.Clear()
        End If
    End Sub

    Private Sub lstTagNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstTagNo.LostFocus
        If lstTagNo.Items.Count > 0 Then
            lstTagNo.SetSelected(0, False)
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub cmbImageViewStyle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbImageViewStyle.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbImageViewStyle_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbImageViewStyle.SelectedIndexChanged
        Select Case cmbImageViewStyle.Text
            ''Case "DETAIL VIEW"
            ''    tabViewStyle.SelectedTab = tabFilmStrip
            Case "LIST"
                tabViewStyle.SelectedTab = tabListview
                lstView.View = View.List
            Case "SMALL ICONS"
                tabViewStyle.SelectedTab = tabListview
                lstView.View = View.SmallIcon
            Case "THUMBNAIL"
                tabViewStyle.SelectedTab = tabListview
                lstView.View = View.LargeIcon
            Case "TILE"
                tabViewStyle.SelectedTab = tabListview
                lstView.View = View.Tile
        End Select
        Me.tabViewStyle.Region = New Region(New RectangleF(
        Me.tabFilmStrip.Left + 1,
        Me.tabFilmStrip.Top,
        Me.tabFilmStrip.Width,
        Me.tabFilmStrip.Height))
        cmbImageViewStyle.Select()
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

    Private Sub rbtStockView_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtStockView.CheckedChanged
        dtpTo.Enabled = Not rbtStockView.Checked
    End Sub

    Private Sub cmbMetal_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbMetal.KeyPress
        Select Case e.KeyChar
            Case Chr(Keys.Enter)
                If cmbMetal.Text <> "ALL" Then
                    'strSql = "  SELECT ITEMNAME +' (' +CONVERT (VARCHAR,ITEMID)+')' AS ITEMNAME FROM " & cnAdminDb & " ..ITEMMAST WHERE METALID ="
                    strSql = "  SELECT ITEMNAME  FROM " & cnAdminDb & " ..ITEMMAST WHERE METALID ="
                    strSql += "(SELECT METALID FROM  " & cnAdminDb & "..METALMAST WHERE METALNAME ='" & cmbMetal.Text & "' )"
                    strSql += "AND STOCKTYPE='T'"
                    strSql += GetItemQryFilteration("S")
                    funcLoadCheckedList(chkLstItem, strSql)
                Else
                    'strSql = " SELECT ITEMNAME +' (' +CONVERT (VARCHAR,ITEMID)+')' AS ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T' "
                    strSql = " SELECT ITEMNAME  FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T' "
                    strSql += GetItemQryFilteration("S")
                    strSql += " ORDER BY ITEMNAME"
                    funcLoadCheckedList(chkLstItem, strSql)
                End If
        End Select
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dtSource As New DataTable
        If gridDetailView.DataSource Is Nothing Then
            MsgBox("There is no record", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtSource = CType(gridDetailView.DataSource, DataTable).Copy
        If Not dtSource.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim roSelected() As DataRow = dtSource.Select("SELECTED = 1")
        If Not roSelected.Length > 0 Then
            MsgBox("There is no selected record", MsgBoxStyle.Information)
            If gridDetailView.RowCount > 0 Then gridDetailView.Select()
            Exit Sub
        End If

        Dim filename As String = ""
        Dim defalutDestination As String = Nothing
        Dim storepath As String = Nothing
        Dim Finfo As IO.FileInfo
        Dim open_file As New FolderBrowserDialog

        open_file = New FolderBrowserDialog
        open_file.RootFolder = Environment.SpecialFolder.Desktop
        If open_file.ShowDialog() = Windows.Forms.DialogResult.OK Then
            defalutDestination = open_file.SelectedPath
        End If

        For Each ro As DataRow In roSelected
            Dim sitemid As String = ro!ITEMID.ToString
            Dim stagno As String = ro!TAGNO.ToString
            Dim sfile As String = sitemid & "_" & stagno
            Dim fileDestPath As String = ro!PCTFILE.ToString
            If IO.File.Exists(fileDestPath) Then
                Finfo = New IO.FileInfo(fileDestPath)
                If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                Else
                    Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, FileAccess.Read)
                    dgvImageCell.Value = Bitmap.FromStream(fileStr)
                    storepath = defalutDestination & "\" & sfile & Finfo.Extension
                    If IO.File.Exists(storepath) Then IO.File.Delete(storepath)
                    IO.File.Copy(fileDestPath, storepath)
                    fileStr.Close()

                End If
            End If
        Next
        MsgBox("Image Store Successfully.", MsgBoxStyle.Information)
    End Sub
    Private Sub gridDetailView_CellValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridDetailView.CellValueChanged
        Try
            Dim rwIndex As Integer = gridDetailView.CurrentRow.Index
            With gridDetailView.Rows(rwIndex)
                If CType(.Cells(0).Value, Boolean) Then
                    gridDetailView.Rows(rwIndex).Cells("SELECTED").Value = "1"
                Else
                    gridDetailView.Rows(rwIndex).Cells("SELECTED").Value = "0"
                End If
            End With
        Catch ex As Exception

        End Try
    End Sub
    Function specificformat()
        btnSearch.Enabled = False
        Try
            ProgressBarShow()
            dtImagePath.Rows.Clear()
            lblPctFile.Text = "Default Picture Path - " + defaultPic.ToString
            If ChkPctPath.Checked Then lblPctFile.Visible = True Else lblPctFile.Visible = False
            strSql = " DECLARE @DEFPATH VARCHAR(200)"
            strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
            strSql += vbCrLf + " SELECT TAGNO"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME"
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = T.ITEMID AND SUBITEMID = T.SUBITEMID)AS SUBITEMNAME"
            strSql += vbCrLf + " ,CASE WHEN PCS<> 0 THEN PCS ELSE NULL END PCS"
            strSql += vbCrLf + " ,CASE WHEN GRSWT <> 0  THEN GRSWT ELSE NULL END GRSWT"
            strSql += vbCrLf + " ,CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END NETWT"
            strSql += vbCrLf + " ,CASE WHEN SALVALUE <> 0 THEN SALVALUE ELSE NULL END SALVALUE"
            'If rbtStockView.Checked = True Then
            '    If Chksalvalue.Checked = False Then
            '        strSql += vbCrLf + " ,CASE WHEN SALVALUE <> 0 THEN SALVALUE ELSE NULL END SALVALUE"
            '    Else
            '        strSql += vbCrLf + " ,ROUND((CASE WHEN T.SALEMODE IN ('R','F') THEN T.SALVALUE ELSE((((CASE WHEN GRSNET='G' THEN GRSWT ELSE NETWT END)+"
            '        strSql += vbCrLf + " (CASE WHEN ISNULL(MAXWASTPER,0)> 0 THEN ROUND(((NETWT)*MAXWASTPER/100),3) ELSE "
            '        strSql += vbCrLf + " MAXWAST END)))* "
            '        strSql += vbCrLf + " (CASE WHEN ISNULL((SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M WHERE RDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND "
            '        strSql += vbCrLf + " RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
            '        strSql += vbCrLf + " AND METALID = (SELECT METALID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID))"
            '        strSql += vbCrLf + " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID)) ORDER BY SNO DESC),0)>0	"
            '        strSql += vbCrLf + " THEN "
            '        strSql += vbCrLf + " ISNULL((SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M WHERE RDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND "
            '        strSql += vbCrLf + " RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
            '        strSql += vbCrLf + " AND METALID = (SELECT METALID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID))"
            '        strSql += vbCrLf + " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID)) ORDER BY SNO DESC),0)"
            '        strSql += vbCrLf + " WHEN 	"
            '        strSql += vbCrLf + " ISNULL((SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M WHERE RDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
            '        strSql += vbCrLf + " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
            '        strSql += vbCrLf + " AND METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID))"
            '        strSql += vbCrLf + " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY "
            '        strSql += vbCrLf + " WHERE CATCODE = (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID))) ORDER BY SNO DESC),0)>0"
            '        strSql += vbCrLf + " THEN"
            '        strSql += vbCrLf + " ISNULL((SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M WHERE RDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
            '        strSql += vbCrLf + " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
            '        strSql += vbCrLf + " AND METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID))"
            '        strSql += vbCrLf + " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY "
            '        strSql += vbCrLf + " WHERE CATCODE = (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID))) ORDER BY SNO DESC),0) END) 	"
            '        strSql += vbCrLf + " + MAXMC+(SELECT ISNULL(SUM(STNAMT),0) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGNO =T.TAGNO)) END),0)SALVALUE"
            '    End If
            'Else
            '    strSql += vbCrLf + " ,CASE WHEN SALVALUE <> 0 THEN SALVALUE ELSE NULL END SALVALUE"
            'End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS  DIAPCS "
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS  DIAWT "
            strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
            ''strSql += vbCrLf + " ,@DEFPATH + PCTFILE PCTFILE"
            If Itemmast_PctPath Then
                strSql += vbCrLf + " ,(CASE WHEN (SELECT ISNULL(ITEMPCTPATH,'') ITEMPCTPATH FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =T.ITEMID) <>'' THEN "
                strSql += vbCrLf + " (SELECT ISNULL(CASE WHEN SUBSTRING(ITEMPCTPATH,LEN(ITEMPCTPATH)-1,1 )='\' OR SUBSTRING(ITEMPCTPATH,LEN(ITEMPCTPATH)-1,1 )='/' "
                strSql += vbCrLf + " THEN ITEMPCTPATH ELSE ITEMPCTPATH+'\' END,'') ITEMPCTPATH FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =T.ITEMID) ELSE @DEFPATH END)"
                strSql += vbCrLf + " + PCTFILE PCTFILE"
            Else
                strSql += vbCrLf + " ,@DEFPATH + PCTFILE PCTFILE"
            End If
            strSql += vbCrLf + " ,PCTFILE PICNAME,ITEMID,0 SELECTED "
            strSql += vbCrLf + " ,(CONVERT(VARCHAR(15),ITEMID )+'-'+CONVERT(VARCHAR(15),TAGNO)+'-'+CONVERT(VARCHAR(15),'N:')+'' +CONVERT(VARCHAR(15),NETWT)"
            If rbtStockView.Checked = True Then
                If Chksalvalue.Checked = True Then
                    strSql += vbCrLf + "+'-'+CONVERT(VARCHAR(15),'Salval:')+ ISNULL(CONVERT (varchar(15),"
                    strSql += vbCrLf + " ROUND((CASE WHEN T.SALEMODE IN ('R','F') THEN T.SALVALUE ELSE((((CASE WHEN GRSNET='G' THEN GRSWT ELSE NETWT END)+"
                    strSql += vbCrLf + " (CASE WHEN ISNULL(MAXWASTPER,0)> 0 THEN ROUND(((NETWT)*MAXWASTPER/100),3) ELSE "
                    strSql += vbCrLf + " MAXWAST END)))* "
                    strSql += vbCrLf + " (CASE WHEN ISNULL((SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M WHERE RDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND "
                    strSql += vbCrLf + " RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
                    strSql += vbCrLf + " AND METALID = (SELECT METALID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID))"
                    strSql += vbCrLf + " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID)) ORDER BY SNO DESC),0)>0	"
                    strSql += vbCrLf + " THEN "
                    strSql += vbCrLf + " ISNULL((SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M WHERE RDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND "
                    strSql += vbCrLf + " RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
                    strSql += vbCrLf + " AND METALID = (SELECT METALID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID))"
                    strSql += vbCrLf + " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID)) ORDER BY SNO DESC),0)"
                    strSql += vbCrLf + " WHEN 	"
                    strSql += vbCrLf + " ISNULL((SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M WHERE RDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
                    strSql += vbCrLf + " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
                    strSql += vbCrLf + " AND METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID))"
                    strSql += vbCrLf + " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY "
                    strSql += vbCrLf + " WHERE CATCODE = (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID))) ORDER BY SNO DESC),0)>0"
                    strSql += vbCrLf + " THEN"
                    strSql += vbCrLf + " ISNULL((SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M WHERE RDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
                    strSql += vbCrLf + " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
                    strSql += vbCrLf + " AND METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID))"
                    strSql += vbCrLf + " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY "
                    strSql += vbCrLf + " WHERE CATCODE = (SELECT DISTINCT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID))) ORDER BY SNO DESC),0) END) 	"
                    strSql += vbCrLf + " + MAXMC+(SELECT ISNULL(SUM(STNAMT),0) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGNO =T.TAGNO)) END),0)"
                    strSql += vbCrLf + "),0)"
                End If
            End If
            If ChkPctPath.Checked Then
                strSql += vbCrLf + " + CHAR(13) + PCTFILE"
            End If
            strSql += vbCrLf + " )AS ITEMTAGNO  "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE 1 = 1"
            strSql += funcFiltration()
            ''strSql += vbCrLf + " ORDER BY T.ITEMID "
            If rbtItemidwise.Checked Then
                strSql += " ORDER BY ITEMID "
            ElseIf rbtDesignerwise.Checked Then
                strSql += " ORDER BY (SELECT TOP 1 DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID) "
            Else
                strSql += " ORDER BY ITEMID "
            End If
            ProgressBarStep("Loading..")
            dtchk = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtImagePath)
            da.Fill(dtchk)
            gridtagimageview.DataSource = Nothing
            gridtagimageview.DataSource = dtchk
            If Not dtImagePath.Rows.Count > 0 Then
                btnSearch.Enabled = True
                MsgBox(E0011, MsgBoxStyle.Information)
                Exit Function
            End If
            If rbtList.Checked Then
                tabListview.Show()
                ListView1.Items.Clear()
                ListView1.LargeImageList = myImagelargeview
                Dim lstItem As ListViewItem
                myImagelargeview.Images.Clear()
                myImagelargeview.Images.Add(bmap)
                myImagelargeview.Images.SetKeyName(0, "IMAGENOTFOUND")
                Dim imageIndex As Integer = 1
                Dim refreshVar As Integer = 0
                For cnt As Integer = 0 To dtImagePath.Rows.Count - 1
                    lstItem = New ListViewItem(dtImagePath.Rows(cnt).Item("ITEMTAGNO").ToString)
                    'lstItem = New ListViewItem(dtImagePath.Rows(cnt).Item("tagno").ToString)
                    lstItem.SubItems.Add(dtImagePath.Rows(cnt).Item("ITEMID").ToString)
                    ListView1.Items.Add(lstItem)
                    ListView1.View = View.LargeIcon
                    ListView1.CheckBoxes = True
                    If dtImagePath.Rows(cnt).Item("PCTFILE").ToString <> "" Then
                        Dim serverPath As String = Nothing
                        Dim fileDestPath As String = dtImagePath.Rows(cnt).Item("PCTFILE").ToString
                        If IO.File.Exists(fileDestPath) Then
                            Dim Finfo As IO.FileInfo
                            Finfo = New IO.FileInfo(fileDestPath)
                            'Finfo.IsReadOnly = False
                            If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                                dgvImageCell.Value = bmap
                            Else
                                Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, FileAccess.Read)
                                'myImageListSmall.Images.Add(Bitmap.FromStream(fileStr))
                                'myImageListSmall.Images.SetKeyName(imageIndex, dtImagePath.Rows(cnt).Item("PICNAME").ToString)

                                myImagelargeview.Images.Add(Bitmap.FromStream(fileStr))
                                myImagelargeview.Images.SetKeyName(imageIndex, dtImagePath.Rows(cnt).Item("PICNAME").ToString)
                                ListView1.Items(cnt).ImageKey = dtImagePath.Rows(cnt).Item("PICNAME").ToString
                                imageIndex += 1
                                fileStr.Close()
                            End If
                        Else
                            ListView1.Items(cnt).ImageKey = "IMAGENOTFOUND"
                        End If
                    Else
                        'myImageList.Images.Add(bmap)
                        ListView1.Items(cnt).ImageKey = "IMAGENOTFOUND"
                    End If
                    If (cnt - refreshVar) = 200 Then
                        ProgressBarStep(cnt.ToString + " Tag's Loaded..")
                        refreshVar = cnt
                    End If
                Next
                tabMain.SelectedTab = tabLarge
                Me.tabViewStyle.Region = New Region(New RectangleF(
                Me.tabFilmStrip.Left + 1,
                Me.tabFilmStrip.Top,
                Me.tabFilmStrip.Width,
                Me.tabFilmStrip.Height))
                ListView1.Select()
            End If
        Catch ex As Exception
            ProgressBarHide()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        Finally
            ProgressBarHide()
            btnSearch.Enabled = True
        End Try
    End Function
    Private Sub chkspecific_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkspecific.CheckedChanged
        If chkspecific.Checked Then
            chkNewformat.Checked = False
            rbtGrid.Enabled = False
            rbtList.Checked = True
            If rbtStockView.Checked Then
                If chkspecific.Checked Then
                    Chksalvalue.Enabled = True
                    ChkPctPath.Enabled = True
                Else
                    Chksalvalue.Enabled = False
                    ChkPctPath.Enabled = False
                End If
            Else
                Chksalvalue.Enabled = False
                ChkPctPath.Enabled = False
            End If
        Else
            rbtGrid.Enabled = True
            rbtGrid.Checked = True
            If rbtStockView.Checked Then
                If chkspecific.Checked Then
                    Chksalvalue.Enabled = True
                    ChkPctPath.Enabled = True
                Else
                    Chksalvalue.Enabled = False
                    ChkPctPath.Enabled = False
                End If
            Else
                Chksalvalue.Enabled = False
                ChkPctPath.Enabled = False
            End If
        End If
    End Sub
    Private Sub ListView1_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        If ListView1.Items.Count > 0 Then
            Dim itemid As Integer
            Dim tagno As String
            If ListView1.Items.Count > 0 Then
                Dim s As String = ListView1.SelectedItems(0).SubItems(0).Text
                If s.Contains("-") Then
                    Dim words As String() = s.Split("-")
                    itemid = words(0)
                    tagno = words(1)
                End If
                Dim objTagViewer As New frmTagImageViewer_Format2(tagno, itemid, , , dtchk)
                objTagViewer.Show()
            End If
        End If
    End Sub
    Private Sub ListView1_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ListView1.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ListView1_DoubleClick(Me, New EventArgs)
        End If
    End Sub
    Private Sub SetSelectedRowsStatus(ByVal rows As IEnumerable, ByVal status As String)
        For Each row As DataGridViewRow In rows
            row.Cells("selected").Value = 1
        Next
    End Sub

    Private Sub btncopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncopy.Click

        Dim rwIndex As String
        Dim dtSource As New DataTable
        If gridtagimageview.DataSource Is Nothing Then
            MsgBox("There is no record", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtSource = CType(gridtagimageview.DataSource, DataTable).Copy
        For Each itm As ListViewItem In ListView1.Items
            rwIndex = 0
            If itm.Checked Then
                Dim s As String = itm.Text
                If s.Contains("-") Then
                    Dim words As String() = s.Split("-")
                    'tagno = words(1)
                    rwIndex = words(1)
                End If
                'rwIndex = itm.Text
            End If
            If rwIndex <> 0 Then
                For i As Integer = 0 To dtSource.Rows.Count - 1
                    If dtSource.Rows(i).Item("TAGNO") = rwIndex Then
                        dtSource.Rows(i).Item("SELECTED") = 1
                    End If
                Next

                'For Each dr As DataRow In dtSource.Rows
                '    Select Case CInt(dr.Item("SELECTED"))
                '        Case 0
                '            dr.Item("SELECTED") = "1"
                '    End Select
                'Next

            End If
        Next
        If Not dtSource.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim roSelected() As DataRow = dtSource.Select("SELECTED = 1")
        If Not roSelected.Length > 0 Then
            MsgBox("There is no selected record", MsgBoxStyle.Information)
            If gridtagimageview.RowCount > 0 Then gridtagimageview.Select()
            Exit Sub
        End If

        Dim filename As String = ""
        Dim defalutDestination As String = Nothing
        Dim storepath As String = Nothing
        Dim Finfo As IO.FileInfo
        Dim open_file As New FolderBrowserDialog

        open_file = New FolderBrowserDialog
        open_file.RootFolder = Environment.SpecialFolder.Desktop
        If open_file.ShowDialog() = Windows.Forms.DialogResult.OK Then
            defalutDestination = open_file.SelectedPath
        End If
        If defalutDestination = Nothing Then Exit Sub

        For Each ro As DataRow In roSelected
            Dim sitemid As String = ro!ITEMID.ToString
            Dim stagno As String = ro!TAGNO.ToString
            Dim sfile As String = sitemid & "_" & stagno
            Dim fileDestPath As String = ro!PCTFILE.ToString
            If IO.File.Exists(fileDestPath) Then
                Finfo = New IO.FileInfo(fileDestPath)
                If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                Else
                    Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, FileAccess.Read)
                    'dgvImageCell.Value = Bitmap.FromStream(fileStr)
                    storepath = defalutDestination & "\" & sfile & Finfo.Extension
                    If IO.File.Exists(storepath) Then IO.File.Delete(storepath)
                    IO.File.Copy(fileDestPath, storepath)
                    fileStr.Close()

                End If
            End If
        Next
        MsgBox("Image Store Successfully.", MsgBoxStyle.Information)
    End Sub

    Private Sub Panel4_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel4.Paint

    End Sub

    Private Sub btnSave_OWN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave_OWN.Click
        Save = True
        Prop_Sets()
        Save = False
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmTagImage_properties
        GetSettingsObj(obj, Me.Name, GetType(frmTagImage_properties), IIf(Authorize = False, True, False))
        rbtStockView.Checked = obj.p_rbtStk
        rbtSalesView.Checked = obj.p_rbtSales
        rbtReceiptView.Checked = obj.p_rbtReceipt
        rbtIssueView.Checked = obj.p_rbtIssue

    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmTagImage_properties
        obj.p_rbtStk = rbtStockView.Checked
        obj.p_rbtSales = rbtSalesView.Checked
        obj.p_rbtReceipt = rbtReceiptView.Checked
        obj.p_rbtIssue = rbtIssueView.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmTagImage_properties), Save)
    End Sub



    Private Sub ExportGridToExcel()

        'Save to excel with headers
        Dim ExcelApp As Object, ExcelBook As Object
        Dim ExcelSheet As Object
        Dim i As Integer
        Dim j As Integer

        'create object of excel
        ExcelApp = CreateObject("Excel.Application")
        ExcelBook = ExcelApp.WorkBooks.Add
        ExcelSheet = ExcelBook.WorkSheets(1)

        With ExcelSheet
            For Each column As DataGridViewColumn In gridDetailView.Columns
                If column.Index + 1 <= gridDetailView.Columns.Count - 3 Then
                    .cells(1, column.Index + 1) = column.HeaderText
                End If
            Next
            For i = 1 To Me.gridDetailView.RowCount
                For j = 0 To gridDetailView.Columns.Count - 4
                    If j = 0 Then
                        Dim imagString As String = gridDetailView.Rows(i - 1).Cells("PCTFILE").Value
                        Dim oRange As Excel.Range = CType(ExcelSheet.Cells((i + 1), (j + 1)), Excel.Range)
                        Dim Left As Single = CType(CType(oRange.Left, Double), Single)
                        Dim Top As Single = CType(CType(oRange.Top, Double), Single)
                        Dim ImageSize As Single = 64
                        If File.Exists(imagString) Then
                            ExcelSheet.Shapes.AddPicture(imagString, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize)
                            oRange.RowHeight = (ImageSize + 2)
                        End If
                    Else
                        .cells(i + 1, j + 1) = gridDetailView.Rows(i - 1).Cells(j).Value
                    End If
                Next
                Dim formatRange As Excel.Range
                formatRange = ExcelSheet.Range("a1")
                formatRange.EntireRow.Font.Bold = True
                formatRange = ExcelSheet.Range("A1", "T1")
                formatRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue)
                formatRange.BorderAround(Excel.XlLineStyle.xlContinuous)
                formatRange = ExcelSheet.Range("a1", "T1")
                formatRange.EntireRow.BorderAround()
            Next
        End With

        ExcelApp.Visible = True
        '
        ExcelSheet = Nothing
        ExcelBook = Nothing
        ExcelApp = Nothing
    End Sub



    Private Sub btnExcel_Click_1(sender As Object, e As EventArgs) Handles btnExcel.Click
        ExportGridToExcel()
    End Sub

    Private Sub btnNewformat_CheckedChanged(sender As Object, e As EventArgs) Handles chkNewformat.CheckedChanged
        chkspecific.Checked = False
    End Sub

End Class
Public Class frmTagImage_properties
    Private rbtStk As Boolean = True
    Public Property p_rbtStk() As Boolean
        Get
            Return rbtStk
        End Get
        Set(ByVal value As Boolean)
            rbtStk = value
        End Set
    End Property
    Private rbtSales As Boolean = False
    Public Property p_rbtSales() As Boolean
        Get
            Return rbtSales
        End Get
        Set(ByVal value As Boolean)
            rbtSales = value
        End Set
    End Property
    Private rbtIssue As Boolean = False
    Public Property p_rbtIssue() As Boolean
        Get
            Return rbtIssue
        End Get
        Set(ByVal value As Boolean)
            rbtIssue = value
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
End Class

