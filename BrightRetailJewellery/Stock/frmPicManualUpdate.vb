Imports System.Data.OleDb
Imports System.IO
Public Class frmPicManualUpdate
    Dim strSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim defaultDestination As String
    Dim defaultSourcePath As String
    Dim tran As OleDbTransaction = Nothing
    Dim cmd As OleDbCommand = Nothing
    Dim flagDeviceMode As Boolean = False
    Dim iDevice As Integer = 0
    Const CAP As Short = &H400S
    Const CAP_DRIVER_CONNECT As Integer = CAP + 10
    Const CAP_DRIVER_DISCONNECT As Integer = CAP + 11
    Const CAP_EDIT_COPY As Integer = CAP + 30
    Const CAP_SET_PREVIEW As Integer = CAP + 50
    Const CAP_SET_PREVIEWRATE As Integer = CAP + 52
    Const CAP_SET_SCALE As Integer = CAP + 53
    Const WS_CHILD As Integer = &H40000000
    Const WS_VISIBLE As Integer = &H10000000
    Const SWP_NOMOVE As Short = &H2S
    Const SWP_NOSIZE As Short = 1
    Const SWP_NOZORDER As Short = &H4S
    Const HWND_BOTTOM As Short = 1
    Dim hHwnd As Integer  ' Handle value to preview window
    Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
        (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, _
         ByVal lParam As Object) As Integer
    Declare Function SetWindowPos Lib "user32" Alias "SetWindowPos" (ByVal hwnd As Integer, _
        ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, _
        ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer

    Declare Function DestroyWindow Lib "user32" (ByVal hndw As Integer) As Boolean
    Declare Function capCreateCaptureWindowA Lib "avicap32.dll" _
            (ByVal lpszWindowName As String, ByVal dwStyle As Integer, _
            ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, _
            ByVal nHeight As Short, ByVal hWndParent As Integer, _
            ByVal nID As Integer) As Integer

    Dim picPath As String = Nothing
    Dim openDia As New OpenFileDialog
    Dim picExtension As String = Nothing
    Dim destFilename As String = Nothing

    Dim GridRowIndex As Integer = Nothing
    Dim flagOtherMaster As Boolean = False

#Region "Newly add"
    Dim Checkfile1 As String
    Dim fileDesginationPath As String
#End Region

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
        defaultDestination = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
        If Not defaultDestination.EndsWith("\") And defaultDestination <> Nothing Then defaultDestination += "\"
        'With dtGridView
        '    .Columns.Add("SNO", GetType(String))
        '    .Columns.Add("LOTNO", GetType(Integer))
        '    .Columns.Add("PARTICULAR", GetType(String))
        '    .Columns.Add("ITEMID", GetType(Integer))
        '    .Columns.Add("TAGNO", GetType(String))
        '    .Columns.Add("RECDATE", GetType(Date))
        '    .Columns.Add("PCS", GetType(Integer))
        '    .Columns.Add("GRSWT", GetType(Decimal))
        '    .Columns.Add("NETWT", GetType(Decimal))
        '    .Columns.Add("SALVALUE", GetType(Double))
        '    .Columns.Add("PICFILENAME", GetType(String))
        '    .Columns.Add("COUNTER", GetType(String))
        '    .Columns.Add("PCTFILE", GetType(String))
        'End With
        'gridView.DataSource = dtGridView
        'FormatGridColumns(gridView)
        'gridView.ColumnHeadersVisible = True

        AutoImageSizer(My.Resources.no_photo, picImage)
        'picImage.Image = My.Resources.no_photo
        'picImage.SizeMode = PictureBoxSizeMode.StretchImage
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            picImage.Image = Nothing
            btnSearch.Enabled = False
            strSql = " DECLARE @DEFPATH VARCHAR(200)"
            strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultDestination & "'"
            strSql += vbCrLf + " SELECT SNO"
            strSql += vbCrLf + " ,CASE WHEN SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)"
            strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) END AS PARTICULAR"
            strSql += vbCrLf + " ,(SELECT LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = T.LOTSNO)LOTNO"
            strSql += vbCrLf + " ,ITEMID,TAGNO,RECDATE,PCS,GRSWT,NETWT,SALVALUE"
            strSql += vbCrLf + " ,PCTFILE"
            strSql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS COUNTER"
            strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
            strSql += vbCrLf + " ,COSTID,CASE WHEN PCTFILE <> '' THEN @DEFPATH + PCTFILE ELSE '' END AS PICFILENAME FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE ISSDATE IS NULL "
            If dtpTo.Enabled Then
                strSql += vbCrLf + " AND RECDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + " AND RECDATE <= '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            End If
            If chkWithImage.Checked And chkWithOutImage.Checked = False Then
                strSql += vbCrLf + " AND ISNULL(PCTFILE,'') <> ''"
            ElseIf chkWithImage.Checked = False And chkWithOutImage.Checked Then
                strSql += vbCrLf + " AND ISNULL(PCTFILE,'') = ''"
            End If
            If txtItemId.Text <> "" Then strSql += vbCrLf + " AND ITEMID = " & Val(txtItemId.Text) & ""
            If txtTagNo.Text <> "" Then strSql += vbCrLf + " AND TAGNO = '" & txtTagNo.Text & "'"
            If Not cnCentStock Then strSql += vbCrLf + " AND COMPANYID = '" & GetStockCompId() & "'"
            If cmbItemCounter.Text <> "" And cmbItemCounter.Text <> "ALL" Then
                strSql += vbCrLf + " AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbItemCounter.Text & "')"
            End If
            If CmbCostcentre.Text <> "" And CmbCostcentre.Text <> "ALL" Then
                strSql += vbCrLf + " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & CmbCostcentre.Text & "')"
            End If
            gridView.DataSource = Nothing
            gridView.Columns.Clear()
            Me.Refresh()
            Dim dtGridView As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridView)
            If Not dtGridView.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                dtpFrom.Select()
                Exit Sub
            End If
            If chkPhysicalImage.Checked Then
                For Each Ro As DataRow In dtGridView.Rows
                    If Not IO.File.Exists(Ro.Item("PICFILENAME").ToString) Then
                        Ro.Item("PICFILENAME") = ""
                    End If
                Next
                Dim dvTemp As New DataView
                dvTemp = dtGridView.DefaultView
                dvTemp.RowFilter = "PICFILENAME <> ''"
                gridView.DataSource = dvTemp.ToTable
            Else
                gridView.DataSource = dtGridView
            End If
            With gridView
                .Columns("SNO").Visible = False
                .Columns("PARTICULAR").Width = 150
                .Columns("LOTNO").Width = 50
                .Columns("ITEMID").Width = 50
                .Columns("TAGNO").Width = 80
                .Columns("RECDATE").Width = 80
                .Columns("PCS").Width = 40
                .Columns("GRSWT").Width = 80
                .Columns("NETWT").Width = 80
                .Columns("SALVALUE").Width = 100
                .Columns("COUNTER").Width = 120
                .Columns("PCTFILE").Width = 200
                .Columns("PICFILENAME").Visible = False
                .Columns("RECDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                .Columns("COSTID").Visible = False
            End With
            Dim dgvBtnBrowse As New DataGridViewButtonColumn
            dgvBtnBrowse.Name = "btnBrowse"
            dgvBtnBrowse.Width = gridView.RowTemplate.Height
            dgvBtnBrowse.Text = "Browse"
            dgvBtnBrowse.HeaderText = ""
            gridView.Columns.Insert(9, dgvBtnBrowse)
            BrighttechPack.FormatGridColumns(gridView)
            gridView.Select()
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(0).Cells("BTNBROWSE")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            btnSearch.Enabled = True
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        gridView.DataSource = Nothing
        dtpFrom.Value = GetEntryDate(GetServerDate(tran), tran)
        dtpTo.Value = GetEntryDate(GetServerDate(tran), tran)
        picImage.Image = Nothing
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICSOURCEPATH'"
        defaultSourcePath = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
        chkWithImage.Checked = True
        chkWithOutImage.Checked = True
        chkPhysicalImage.Checked = False
        cmbItemCounter.Text = "ALL"
        txtItemId.Clear()
        txtTagNo.Clear()
        chkDate.Focus()
        'dtpFrom.Select()
    End Sub

    Private Sub frmPicManualUpdate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            If btnCapture.Focused = True Then
                btnCapture.Visible = False
                gridView.Focus()
                gridView.CurrentCell = gridView.Rows(GridRowIndex).Cells("BTNBROWSE")
                GridRowIndex = Nothing
            End If
        End If
    End Sub


    Private Sub chkAutosnap()
        If GetSoftValue("AUTOSNAP") = "Y" Then
            flagDeviceMode = True
        End If
        If flagDeviceMode = True Then

            PictureBox1.Visible = False
            picImage.Visible = False
            'Panel2.Location = New Point(192, 3)

            OpenForm()
        Else
            picImage.Visible = True
            PictureBox1.Visible = False
        End If

    End Sub
    Private Sub OpenForm()
        Dim iHeight As Integer = PictureBox1.Height
        Dim iWidth As Integer = PictureBox1.Width
        ' Open Preview window in picturebox .
        ' Create a child window with capCreateCaptureWindowA so you can display it in a picturebox.
       
        hHwnd = capCreateCaptureWindowA(iDevice, WS_VISIBLE Or WS_CHILD, 0, 0, 640, _
            480, PictureBox1.Handle.ToInt32, 0)
        ' Connect to device
        If SendMessage(hHwnd, CAP_DRIVER_CONNECT, iDevice, 0) Then

            ' Set the preview scale
            SendMessage(hHwnd, CAP_SET_SCALE, True, 0)

            ' Set the preview rate in milliseconds
            SendMessage(hHwnd, CAP_SET_PREVIEWRATE, 66, 0)

            ' Start previewing the image from the camera 
            SendMessage(hHwnd, CAP_SET_PREVIEW, True, 0)

            ' Resize window to fit in picturebox 
            SetWindowPos(hHwnd, HWND_BOTTOM, 0, 0, PictureBox1.Width, PictureBox1.Height, _
                                   SWP_NOMOVE Or SWP_NOZORDER)

        Else
            ' Error connecting to device close window 
            DestroyWindow(hHwnd)

        End If
    End Sub

    Private Function GetSoftValue(ByVal id As String) As String
        Return UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & id & "'", , "", tran))
    End Function
    Private Sub piccap()
        Dim data As IDataObject
        Dim bmap As Image
        If File.Exists(Application.StartupPath & "\tst.jpg") Then File.Delete(Application.StartupPath & "\tst.jpg")
        ' Copy image to clipboard 
        SendMessage(hHwnd, CAP_EDIT_COPY, 0, 0)
        ' Get image from clipboard and convert it to a bitmap 
        Data = Clipboard.GetDataObject()
        If Data.GetDataPresent(GetType(System.Drawing.Bitmap)) Then
            bmap = CType(Data.GetData(GetType(System.Drawing.Bitmap)), Image)
            picImage.Image = bmap

            picPath = Application.StartupPath & "\tst.jpg"
            picExtension = "Jpg"
            picImage.Image.Save(picPath, System.Drawing.Imaging.ImageFormat.Jpeg)

        End If

    End Sub
    Private Sub gridView_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellClick
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Not IO.Directory.Exists(defaultDestination) Then
            MsgBox(defaultDestination & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim Syncdb As String = cnAdminDb
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix) <> 0 Then Syncdb = uprefix + usuffix
        End If

        If UCase(gridView.Columns(e.ColumnIndex).Name) = "BTNBROWSE" Then
            Try
                GridRowIndex = gridView.CurrentRow.Index
                If flagDeviceMode = True Then
                    'gridView.Rows(GridRowIndex).DefaultCellStyle.BackColor = Color.LightBlue
                    gridView.Rows(GridRowIndex).DefaultCellStyle = reportHeadStyle1
                    PictureBox1.Visible = True
                    picImage.Visible = False
                    btnCapture.Visible = True
                    btnCapture.Focus()
                Else
                    AutoImageSizer(My.Resources.no_photo, picImage)
                    'picImage.Image = My.Resources.no_photo
                    Dim str As String
                    If IO.File.Exists(defaultSourcePath) Or IO.Directory.Exists(defaultDestination) Then
                        openDia.InitialDirectory = defaultSourcePath
                    End If
                    str = "JPEG(*.jpg)|*.jpg"
                    str += "|Bitmap(*.bmp)|*.bmp"
                    str += "|GIF(*.gif)|*.gif"
                    str += "|All Files(*.*)|*.*"
                    openDia.Filter = str
                    If openDia.ShowDialog = Windows.Forms.DialogResult.OK Then
                        Dim Finfo As IO.FileInfo
                        Finfo = New IO.FileInfo(openDia.FileName)
                        'Finfo.IsReadOnly = False
                        AutoImageSizer(openDia.FileName, picImage)
                        'Dim fileStr As New IO.FileStream(openDia.FileName, IO.FileMode.Open)
                        'picImage.Image = Bitmap.FromStream(fileStr)
                        'fileStr.Close()
                        'picImage.Image = Image.FromFile(openDia.FileName)
                        picExtension = Finfo.Extension
                        picPath = openDia.FileName
                        'If openDia.FilterIndex = 1 Then
                        '    picExtension = "JPG"
                        'ElseIf openDia.FilterIndex = 2 Then
                        '    picExtension = "BMP"
                        'ElseIf openDia.FilterIndex = 2 Then
                        '    picExtension = "GIF"
                        'End If
                        Dim Itemmast_PctPath As Boolean = IIf(GetAdmindbSoftValue("PICPATHFROM", "S") = "I", True, False)
                        If Itemmast_PctPath Then
                            Dim defaultPic As String = ""
                            strSql = "SELECT ISNULL(ITEMPCTPATH,'') ITEMPCTPATH FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & gridView.Rows(e.RowIndex).Cells("ITEMID").Value.ToString & "'"
                            defaultPic = UCase(objGPack.GetSqlValue(strSql, "ITEMPCTPATH", "", tran))
                            If defaultPic.ToString = "" Then
                                strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
                                defaultPic = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
                            End If
                            If Not defaultPic.EndsWith("\") And defaultPic <> "" Then defaultPic += "\"
                            If defaultPic.ToString <> "" Then
                                defaultDestination = defaultPic.ToString
                            End If
                        Else
                            defaultDestination = defaultDestination
                        End If
                        destFilename = defaultDestination + GetStockCompId().ToString + "L" + gridView.Rows(e.RowIndex).Cells("LOTNO").Value.ToString + "I" + gridView.Rows(e.RowIndex).Cells("ITEMID").Value.ToString + "T" + gridView.Rows(e.RowIndex).Cells("TAGNO").Value.ToString + IIf(picExtension.Contains("."), picExtension, "." & picExtension)
                            Dim serverPath As String = Nothing
                            Dim fileDestPath As String = defaultDestination + GetStockCompId().ToString + "L" + gridView.Rows(e.RowIndex).Cells("LOTNO").Value.ToString + "I" + gridView.Rows(e.RowIndex).Cells("ITEMID").Value.ToString + "T" + gridView.Rows(e.RowIndex).Cells("TAGNO").Value.ToString + IIf(picExtension.Contains("."), picExtension, "." & picExtension)
                            'Dim Finfo As FileInfo
                            Finfo = New FileInfo(fileDestPath)
                            If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                                MsgBox(Finfo.Directory.FullName & " Does not exist. Please make a corresponding path", MsgBoxStyle.Information)
                                Exit Sub
                            End If

                            If File.Exists(picPath) = True Then
                                If UCase(picPath) <> fileDestPath.ToUpper Then
                                    If File.Exists(fileDestPath) Then
                                        File.SetAttributes(fileDestPath, FileAttribute.Normal)
                                        File.Delete(fileDestPath)
                                    End If
                                    File.Copy(picPath, fileDestPath)
                                End If
                            End If

                            'destFilename = defaultDestination + "\" + "L" + gridView.Rows(e.RowIndex).Cells("LOTNO").Value.ToString + "I" + gridView.Rows(e.RowIndex).Cells("ITEMID").Value.ToString + "T" + gridView.Rows(e.RowIndex).Cells("TAGNO").Value.ToString + "." + picExtension
                            'If IO.File.Exists(destFilename) Then
                            '    If MessageBox.Show(destFilename + vbCrLf + "Already Exist" + vbCrLf + "Do you want to Replace", "Replace Alrert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                            '        picImage.Image = My.Resources.no_photo
                            '        IO.File.Delete(destFilename)
                            '    Else
                            '        Exit Sub
                            '    End If
                            'End If
                            Try
                                tran = Nothing
                                tran = cn.BeginTransaction
                                strSql = "UPDATE " & cnAdminDb & "..ITEMTAG SET "
                                strSql += " PCTPATH ='" & defaultDestination & "'"
                                strSql += " ,PCTFILE = '" & GetStockCompId().ToString & "L" + gridView.Rows(e.RowIndex).Cells("LOTNO").Value.ToString + "I" + gridView.Rows(e.RowIndex).Cells("ITEMID").Value.ToString + "T" + gridView.Rows(e.RowIndex).Cells("TAGNO").Value.ToString.Replace(":", "") + IIf(picExtension.Contains("."), picExtension, "." & picExtension) & "'"
                                strSql += " WHERE SNO = '" & gridView.Rows(e.RowIndex).Cells("SNO").Value.ToString & "'"
                                ExecQuery(SyncMode.Stock, strSql, cn, tran, gridView.Rows(e.RowIndex).Cells("COSTID").Value.ToString.ToUpper)
                                If IO.File.Exists(fileDestPath) Then
                                    If cnCostId.ToUpper <> gridView.Rows(e.RowIndex).Cells("COSTID").Value.ToString.ToUpper Then
                                        strSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID"
                                        strSql += ",TAGIMAGE,TAGIMAGENAME,IMAGE_CTRLID"
                                        strSql += " )"
                                        strSql += " VALUES"
                                        strSql += " ('" & cnCostId & "','" & gridView.Rows(e.RowIndex).Cells("COSTID").Value.ToString & "',?,?,'PICPATH')"
                                        cmd = New OleDbCommand(strSql, cn, tran)
                                        Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, IO.FileAccess.Read)
                                        Dim reader As New IO.BinaryReader(fileStr)
                                        Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                                        fileStr.Read(result, 0, result.Length)
                                        fileStr.Close()
                                        cmd.Parameters.AddWithValue("@TAGIMAGE", result)
                                        Dim fFInfo As New IO.FileInfo(fileDestPath)
                                        cmd.Parameters.AddWithValue("@TAGIMAGENAME", fFInfo.Name)
                                        cmd.ExecuteNonQuery()
                                    End If
                                End If
                                tran.Commit()
                                tran = Nothing
                            Catch ex As Exception
                                If tran IsNot Nothing Then tran.Rollback()
                                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
                            End Try
                            gridView.Rows(GridRowIndex).Cells("PICFILENAME").Value = destFilename
                            gridView.Rows(GridRowIndex).Cells("PCTFILE").Value = GetStockCompId().ToString & "L" + gridView.Rows(GridRowIndex).Cells("LOTNO").Value.ToString + "I" + gridView.Rows(GridRowIndex).Cells("ITEMID").Value.ToString + "T" + gridView.Rows(GridRowIndex).Cells("TAGNO").Value.ToString + "." + picExtension
                            defaultSourcePath = openDia.FileName
                            gridView.Select()
                        End If
                    End If

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation)
            Finally


            End Try
        End If

    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("BTNBROWSE")
            End If
        ElseIf e.KeyCode = Keys.I Then
            With gridView
                If .CurrentRow Is Nothing Then Exit Sub
                Dim frm As New frmOtherMast(.CurrentRow.Cells("SNO").Value.ToString, .CurrentRow.Cells("TAGNO").Value.ToString, .CurrentRow.Cells("ITEMID").Value.ToString, .CurrentRow.Cells("RECDATE").Value, .CurrentRow.Cells("COSTID").Value.ToString, False)
                frm.ShowDialog()
            End With
        End If
    End Sub

    Private Sub gridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        If gridView.RowCount > 0 Then
            With gridView.Rows(e.RowIndex)
                Dim serverPath As String = Nothing
                Dim fileDestPath As String = .Cells("PICFILENAME").Value.ToString
                If IO.File.Exists(fileDestPath) Then
                    Dim Finfo As FileInfo
                    Finfo = New FileInfo(fileDestPath)
                    'Finfo.IsReadOnly = False
                    If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                        AutoImageSizer(My.Resources.no_photo, picImage)
                        'picImage.Image = My.Resources.no_photo

                        If (flagDeviceMode) Then
                            PictureBox1.Visible = False 
                            picImage.Visible = False
                        Else 'N
                            PictureBox1.Visible = False
                            picImage.Visible = True
                        End If

                        Exit Sub
                    Else
                        AutoImageSizer(fileDestPath, picImage)
                        PictureBox1.Visible = False
                        picImage.Visible = True
                    End If
                Else
                    AutoImageSizer(My.Resources.no_photo, picImage)
                    If (flagDeviceMode) Then
                        PictureBox1.Visible = True
                        picImage.Visible = False
                    Else
                        PictureBox1.Visible = False
                        picImage.Visible = True
                    End If
                End If
                lblHelp.Text = " I- Insert Tag Additional Info"
                If flagOtherMaster Then funcTagOtherDetails(.Cells("SNO").Value.ToString)
            End With
        End If
    End Sub

    Private Sub gridView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.SelectionChanged
        If gridView.RowCount > 0 Then
            If gridView.Columns.Contains("BTNBROWSE") Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("BTNBROWSE")
            End If
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub frmPicManualUpdate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        cmbItemCounter.Items.Clear()
        cmbItemCounter.Items.Add("ALL")
        strSql = "SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRNAME"
        objGPack.FillCombo(strSql, cmbItemCounter, False, False)
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            CmbCostcentre.Items.Clear()
            CmbCostcentre.Items.Add("ALL")
            strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, CmbCostcentre, False, False)
            CmbCostcentre.Enabled = True
        Else
            CmbCostcentre.Enabled = False
        End If
        strSql = "SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTER WHERE ISNULL(ACTIVE,'N')<>'N' "
        If GetSqlValue(cn, strSql) = 0 Then flagOtherMaster = False Else flagOtherMaster = True
        If flagOtherMaster = False Then
            PnlTagInfo.Visible = False
            pnlTag.Dock = DockStyle.Fill
            lblHelp.Visible = False
        Else
            PnlTagInfo.Visible = True
            PnlTagInfo.Dock = DockStyle.Bottom
            pnlTag.Dock = DockStyle.Fill
            lblHelp.Visible = True
        End If
        chkAutosnap()
        chkDate.Checked = True
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "TAG IMAGE DETAIL", gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "TAG IMAGE DETAIL", gridView, BrightPosting.GExport.GExportType.Print)
        End If
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

    Private Sub chkPhysicalImage_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPhysicalImage.CheckedChanged
        If chkPhysicalImage.Checked Then
            chkWithOutImage.Checked = chkPhysicalImage.Checked
            chkWithImage.Checked = chkPhysicalImage.Checked
        End If
        chkWithOutImage.Enabled = Not chkPhysicalImage.Checked
        chkWithImage.Enabled = Not chkPhysicalImage.Checked
    End Sub

    Private Sub frmPicManualUpdate_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Leave
        If flagDeviceMode = True Then
            CloseCam()
        End If
    End Sub
    Private Sub CloseCam()

        ' Disconnect from device
        SendMessage(hHwnd, CAP_DRIVER_DISCONNECT, iDevice, 0)
        ' close window 
        DestroyWindow(hHwnd)

    End Sub

    Private Sub btnCapture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCapture.Click
        If gridView.RowCount > 0 Then
            Dim Syncdb As String = cnAdminDb
            Dim uprefix As String = Mid(cnAdminDb, 1, 3)
            If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
                Dim usuffix As String = "UTILDB"
                If DbChecker(uprefix + usuffix) <> 0 Then Syncdb = uprefix + usuffix
            End If

            piccap()
            PictureBox1.Visible = False
            picImage.Visible = True
            destFilename = defaultDestination + GetStockCompId().ToString + "L" + gridView.Rows(GridRowIndex).Cells("LOTNO").Value.ToString + "I" + gridView.Rows(GridRowIndex).Cells("ITEMID").Value.ToString + "T" + gridView.Rows(GridRowIndex).Cells("TAGNO").Value.ToString + "." + picExtension
            Dim serverPath As String = Nothing
            Dim fileDestPath As String = defaultDestination + GetStockCompId().ToString + "L" + gridView.Rows(GridRowIndex).Cells("LOTNO").Value.ToString + "I" + gridView.Rows(GridRowIndex).Cells("ITEMID").Value.ToString + "T" + gridView.Rows(GridRowIndex).Cells("TAGNO").Value.ToString + "." + picExtension
            Dim Finfo As FileInfo
            Finfo = New FileInfo(fileDestPath)
            If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                MsgBox(Finfo.Directory.FullName & " Does not exist. Please make a corresponding path", MsgBoxStyle.Information)
                Exit Sub
            End If

            If File.Exists(picPath) = True Then
                If UCase(picPath) <> fileDestPath.ToUpper Then
                    If File.Exists(fileDestPath) Then
                        File.Delete(fileDestPath)
                    End If
                    File.Copy(picPath, fileDestPath)
                End If
            End If
            Try
                tran = Nothing
                tran = cn.BeginTransaction
                strSql = "UPDATE " & cnAdminDb & "..ITEMTAG SET "
                strSql += " PCTPATH ='" & defaultDestination & "'"
                strSql += " ,PCTFILE = '" & "L" + gridView.Rows(GridRowIndex).Cells("LOTNO").Value.ToString + "I" + gridView.Rows(GridRowIndex).Cells("ITEMID").Value.ToString + "T" + gridView.Rows(GridRowIndex).Cells("TAGNO").Value.ToString.Replace(":", "") + "." + picExtension & "'"
                strSql += " WHERE SNO = '" & gridView.Rows(GridRowIndex).Cells("SNO").Value.ToString & "'"
                ExecQuery(SyncMode.Stock, strSql, cn, tran, gridView.Rows(GridRowIndex).Cells("COSTID").Value.ToString.ToUpper)
                If IO.File.Exists(fileDestPath) Then
                    If cnCostId.ToUpper <> gridView.Rows(GridRowIndex).Cells("COSTID").Value.ToString.ToUpper Then
                        strSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID"
                        strSql += ",TAGIMAGE,TAGIMAGENAME,IMAGE_CTRLID"
                        strSql += " )"
                        strSql += " VALUES"
                        strSql += " ('" & cnCostId & "','" & gridView.Rows(GridRowIndex).Cells("COSTID").Value.ToString & "',?,?,'PICPATH')"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, IO.FileAccess.Read)
                        Dim reader As New IO.BinaryReader(fileStr)
                        Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                        fileStr.Read(result, 0, result.Length)
                        fileStr.Close()
                        cmd.Parameters.AddWithValue("@TAGIMAGE", result)
                        Dim fFInfo As New IO.FileInfo(fileDestPath)
                        cmd.Parameters.AddWithValue("@TAGIMAGENAME", fFInfo.Name)
                        cmd.ExecuteNonQuery()
                    End If
                End If
                tran.Commit()
                tran = Nothing
            Catch ex As Exception
                If tran IsNot Nothing Then tran.Rollback()
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End Try
            gridView.Rows(GridRowIndex).Cells("PICFILENAME").Value = destFilename
            gridView.Rows(GridRowIndex).Cells("PCTFILE").Value = "L" + gridView.Rows(GridRowIndex).Cells("LOTNO").Value.ToString + "I" + gridView.Rows(GridRowIndex).Cells("ITEMID").Value.ToString + "T" + gridView.Rows(GridRowIndex).Cells("TAGNO").Value.ToString + "." + picExtension
            btnCapture.Visible = False
            gridView.Focus()
            gridView.Rows(GridRowIndex).DefaultCellStyle.BackColor = Color.Empty
            gridView.Rows(GridRowIndex).DefaultCellStyle = Nothing
            'gridView.CurrentCell = gridView.Rows(GridRowIndex).Cells("BTNBROWSE")
            GridRowIndex = Nothing
        End If
    End Sub

    Private Sub btnCapture_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCapture.Leave
        btnCapture.Visible = False
    End Sub

    Private Sub txtItemId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim PRODTAGSEP As String = GetAdmindbSoftValue("PRODTAGSEP", "")
            Dim sp() As String = txtItemId.Text.Split(PRODTAGSEP)
            Dim SCANSTR As String = txtItemId.Text
            If PRODTAGSEP <> "" And txtItemId.Text <> "" Then
                sp = txtItemId.Text.Split(PRODTAGSEP)
                txtItemId.Text = Trim(sp(0))
                If sp.Length > 1 Then
                    If Len(SCANSTR) > Len(Trim(sp(0)) & PRODTAGSEP & Trim(sp(1))) Then SCANSTR = Trim(sp(0)) & PRODTAGSEP & Trim(sp(1))
                End If
            End If
CheckItem:
            Dim dtItemDet As New DataTable
            If txtItemId.Text = "" Then
                'LoadSalesItemName()
                Exit Sub
            ElseIf IsNumeric(SCANSTR) = False And SCANSTR.Contains(PRODTAGSEP) = False Then
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(SCANSTR) & "'"
                dtItemDet = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtItemId.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    GoTo LoadItemInfo
                End If
            ElseIf IsNumeric(SCANSTR) = True And SCANSTR.Contains(PRODTAGSEP) = False And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Val(SCANSTR) & "'" & GetItemQryFilteration()) = True Then
                'LoadSalesItemNameDetail() : Exit Sub
            ElseIf PRODTAGSEP <> "" Then
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(SCANSTR).Replace(PRODTAGSEP, "") & "'"
                dtItemDet = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtItemId.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    GoTo LoadItemInfo
                End If
            ElseIf txtItemId.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemId.Text) & "'" & GetItemQryFilteration()) = False Then
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(txtItemId.Text) & "'"
                dtItemDet = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtItemId.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    GoTo LoadItemInfo
                Else
                    '   LoadSalesItemName()
                    Exit Sub
                End If
            Else
LoadItemInfo:
                'LoadSalesItemNameDetail()
            End If
            If sp.Length > 1 And PRODTAGSEP <> "" And txtItemId.Text <> "" Then
                txtTagNo.Text = Trim(sp(1))
            End If
            If txtTagNo.Text <> "" Then
                'txtTagNo_KeyPress(Me, New KeyPressEventArgs(e.KeyChar))
                btnSearch_Click(sender, e)
            Else
                txtTagNo.Focus()
            End If
        End If
    End Sub

    Private Sub chkDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        'dtpFrom.Enabled = chkDate.Checked
        dtpTo.Enabled = chkDate.Checked
        chkDate.Text = IIf(chkDate.Checked, "Date from", "As on date")
    End Sub
    Private Sub funcTagOtherDetails(ByVal TagSno As String)
        strSql = "SELECT A.SNO,ITEMNAME AS ITEM,A.TAGNO,MISCNAME,OT.NAME,U.USERNAME"
        strSql += " FROM " & cnAdminDb & "..ADDINFOITEMTAG A"
        strSql += " INNER JOIN " & cnAdminDb & "..OTHERMASTER OT ON OT.ID=A.OTHID"
        strSql += " INNER JOIN " & cnAdminDb & "..OTHERMASTERENTRY OM ON OM.MISCID=OT.MISCID"
        strSql += " INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID=A.ITEMID"
        strSql += " LEFT JOIN " & cnAdminDb & "..USERMASTER U ON U.USERID=A.USERID"
        strSql += " WHERE A.TAGSNO='" & TagSno & "'"
        gridView1.DataSource = Nothing
        gridView1.Columns.Clear()
        Me.Refresh()
        Dim dtGridView As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridView)
        If dtGridView.Rows.Count > 0 Then
            With gridView1
                .DataSource = dtGridView
                .ReadOnly = True
                BrighttechPack.FormatGridColumns(gridView)
                .Columns("SNO").Visible = False
                .Columns("ITEM").Width = 200
                .Columns("NAME").Width = 200
                .Columns("TAGNO").Width = 100
                .Columns("USERNAME").Width = 200
            End With
        End If
    End Sub

    Private Sub gridView1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView1.KeyDown
        With gridView1
            If .CurrentRow Is Nothing Then Exit Sub
            If e.KeyCode = Keys.U Then
                Dim frm As New frmOtherMast(.CurrentRow.Cells("SNO").Value.ToString, .CurrentRow.Cells("TAGNO").Value.ToString, .CurrentRow.Cells("MISCNAME").Value.ToString, .CurrentRow.Cells("NAME").Value.ToString, True)
                frm.ShowDialog()
            End If
        End With
    End Sub

    Private Sub gridView1_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView1.RowEnter
        lblHelp.Text = " U- Update Tag Additional Info"
    End Sub

    Private Sub btnBulkImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBulkImage.Click
        Try
            'Userright
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
            If gridView.RowCount > 1 Then
                'Designation Path
                strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
                defaultDestination = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
                If Not defaultDestination.EndsWith("\") And defaultDestination <> Nothing Then defaultDestination += "\"
                'Source Path
                strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICSOURCEPATH'"
                defaultSourcePath = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
                'Checking Designation Path
                If Not IO.Directory.Exists(defaultDestination) Then
                    MsgBox(defaultDestination & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
                    Exit Sub
                End If
                If defaultDestination.Contains(defaultSourcePath) Then
                    MsgBox("Source & Destination Path are Same", MsgBoxStyle.Information)
                    Exit Sub
                End If
                If IO.Directory.Exists(defaultSourcePath) Then
                    FolderBrowserDialog1.SelectedPath = defaultSourcePath
                End If
                Dim Syncdb As String = cnAdminDb
                Dim uprefix As String = Mid(cnAdminDb, 1, 3)
                If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
                    Dim usuffix As String = "UTILDB"
                    If DbChecker(uprefix + usuffix) <> 0 Then Syncdb = uprefix + usuffix
                End If
                If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim str As String = FolderBrowserDialog1.SelectedPath
                    If Not str.EndsWith("\") And str <> Nothing Then str += "\"
                    If UCase(str) = UCase(defaultDestination) Then
                        MsgBox("Source & Destination Path are Same", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    Dim fileff() As String = Directory.GetFiles(FolderBrowserDialog1.SelectedPath)
                    For j As Integer = 0 To gridView.Rows.Count - 1
                        Dim GLot As String = gridView.Rows(j).Cells("LOTNO").Value.ToString()
                        Dim GItem As String = gridView.Rows(j).Cells("ITEMID").Value.ToString()
                        Dim GTag As String = gridView.Rows(j).Cells("TAGNO").Value.ToString()
                        Dim GSno As String = gridView.Rows(j).Cells("SNO").Value.ToString()
                        Dim GCost As String = gridView.Rows(j).Cells("COSTID").Value.ToString()
                        Checkfile1 = GItem + "-" + GTag
                        fileDesginationPath = defaultDestination + GetStockCompId().ToString + "L" + GLot + "I" + GItem + "T" + GTag
                        Dim PCTPATH As String = GetStockCompId().ToString + "L" + GLot + "I" + GItem + "T" + GTag + ".jpg"
                        For u As Integer = 0 To Directory.GetFiles(FolderBrowserDialog1.SelectedPath).Length - 1
                            If fileff(u).Contains(Checkfile1) Then
                                Dim temp As String = Checkfile1
                                picPath = fileff(u)
                                If File.Exists(Checkfile1) = True Then
                                    File.Delete(Checkfile1)
                                End If
                                If File.Exists(fileDesginationPath + ".jpg") = True Then
                                    File.Delete(fileDesginationPath + ".jpg")
                                End If
                                AutoImageSizer(fileDesginationPath + ".jpg", picImage)
                                'File.Copy(picPath, Checkfile1)
                                File.Copy(picPath, fileDesginationPath + ".jpg")
                                BulkImageUpdate(Syncdb, defaultDestination, PCTPATH, fileDesginationPath + ".jpg", GSno, GCost)
                                Main.ShowHelpText("TagImage Update" & j + 1 & "of" & gridView.Rows.Count)
                                Main.Refresh()
                                Exit For
                            End If
                        Next
                    Next
                End If
                Main.HideHelpText()
                btnSearch_Click(Me, New System.EventArgs)
            Else
                MsgBox("No Record Found", MsgBoxStyle.Information)
                Exit Sub
            End If
        Catch ex As Exception
            If ex.Message.Contains("generic error occurred in GDI+") Then
                MsgBox("Check Directory Permission.", MsgBoxStyle.Information)
            Else
                MsgBox(ex.Message + vbCrLf + ex.StackTrace, MsgBoxStyle.Information)
            End If
        End Try
    End Sub
#Region "User Define Function"
    Private Sub BulkImageUpdate(ByVal Syncdb As String, ByVal defaultDestination As String, ByVal PCTFILE As String, ByVal fileDestPath As String, ByVal sno As String, ByVal costid As String)
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            strSql = "UPDATE " & cnAdminDb & "..ITEMTAG SET "
            strSql += " PCTPATH ='" & defaultDestination & "'"
            strSql += " ,PCTFILE = '" & PCTFILE & "'"
            strSql += " WHERE SNO = '" & sno & "'"
            ExecQuery(SyncMode.Stock, strSql, cn, tran, costid)
            If IO.File.Exists(fileDestPath) Then
                If cnCostId.ToUpper <> costid Then
                    strSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID"
                    strSql += ",TAGIMAGE,TAGIMAGENAME,IMAGE_CTRLID"
                    strSql += " )"
                    strSql += " VALUES"
                    strSql += " ('" & cnCostId & "','" & costid & "',?,?,'PICPATH')"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, IO.FileAccess.Read)
                    Dim reader As New IO.BinaryReader(fileStr)
                    Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                    fileStr.Read(result, 0, result.Length)
                    fileStr.Close()
                    cmd.Parameters.AddWithValue("@TAGIMAGE", result)
                    Dim fFInfo As New IO.FileInfo(fileDestPath)
                    cmd.Parameters.AddWithValue("@TAGIMAGENAME", fFInfo.Name)
                    cmd.ExecuteNonQuery()
                End If
            End If
            tran.Commit()
            tran = Nothing
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
#End Region
End Class