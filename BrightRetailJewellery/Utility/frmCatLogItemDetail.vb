Imports System.Data.OleDb
Imports System.IO
Imports System.Net
Public Class frmCatLogItemDetail
#Region "VARIABLE"
    Dim strsql As String
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim cmd As OleDbCommand
    Dim dtMetal As New DataTable
    Dim metalName As String
    Dim openDia As New OpenFileDialog
    Dim defaultSourcePath As String
    Dim picExtension As String = Nothing
#End Region
#Region "FORM LOAD"
    Private Sub frmCatLogItemDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '' LOAD METAL
        strsql = vbCrLf + " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT METALNAME,CONVERT(VARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        strsql += vbCrLf + " ORDER BY RESULT,METALNAME"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtMetal)
        cmbMetal.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(cmbMetal, dtMetal, "METALNAME", , "ALL")

        strsql = vbCrLf + " SELECT 'ALL' DESIGNERNAME,1 RESULT"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT DESIGNERNAME,2 RESULT "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..DESIGNER"
        strsql += vbCrLf + " WHERE ISNULL(ACTIVE,'Y')<>'N'"
        strsql += vbCrLf + " ORDER BY RESULT,DESIGNERNAME"
        objGPack.FillCombo(strsql, CmbDesigner)

        strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICSOURCEPATH'"
        defaultSourcePath = UCase(objGPack.GetSqlValue(strsql, "CTLTEXT", , Nothing))

        If connectionString() = True Then
        Else
            MsgBox("Invalid Format", MsgBoxStyle.Information)
            Exit Sub
        End If


        btnNew_Click(Me, New System.EventArgs)
    End Sub
    Private Sub frmCatLogItemDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
#End Region
#Region "BUTTON EVENTS"
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub FtpUploadFile(ByVal filetoupload As String, ByVal ftpuri As String, ByVal ftpusername As String, ByVal ftppassword As String)
        ' Create a web request that will be used to talk with the server and set the request method to upload a file by ftp.
        Dim ftpRequest As FtpWebRequest = CType(WebRequest.Create(ftpuri), FtpWebRequest)
        Try
            ftpRequest.EnableSsl = False
            ftpRequest.KeepAlive = True
            ' Confirm the Network credentials based on the user name and password passed in.
            ftpRequest.Credentials = New NetworkCredential(ftpusername, ftppassword)
            ftpRequest.UseBinary = True
            ftpRequest.UsePassive = True
            'ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory
            'Using response = DirectCast(ftpRequest.GetResponse(), FtpWebResponse)
            '    response.Write(response.StatusCode)
            'End Using
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile
            ' Read into a Byte array the contents of the file to be uploaded 
            Dim bytes() As Byte = System.IO.File.ReadAllBytes(filetoupload)
            ' Transfer the byte array contents into the request stream, write and then close when done.
            ftpRequest.ContentLength = bytes.Length
            Using UploadStream As Stream = ftpRequest.GetRequestStream()
                UploadStream.Write(bytes, 0, bytes.Length)
                UploadStream.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Exit Sub
        End Try
        'MessageBox.Show("Process Complete")
    End Sub

    Private Sub btnTransfer_Click(sender As Object, e As EventArgs) Handles btnTransfer.Click
        If MsgBox("Do You Want Transfer", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then
            If gridView_OWN.Rows.Count > 0 Then
                Try
                    'FtpUploadFile("e:\file.txt", "ftp://ftp.site4now.net/SSB/test/Download/file.txt", "akshayagold-001", "giritech999") 'file.txt
                    btnTransfer.Enabled = False
                    LocalToInternet()
                Catch ex As Exception
                    MessageBox.Show(ex.ToString)
                    Exit Sub
                Finally
                    btnTransfer.Enabled = True
                End Try
            End If
        End If
    End Sub


    Dim split(10) As String
    Dim Webcn As OleDbConnection
    Dim ftpUrl As String
    Dim ftpUrlUserName As String
    Dim ftpUrlPassword As String

    Public Function connectionString() As Boolean
        Dim filePath As String = Application.StartupPath + "\CatInfo.ini"
        If IO.File.Exists(filePath) Then
            Try
                Dim fil As New FileStream(filePath, FileMode.Open, FileAccess.Read)
                Dim fs As New StreamReader(fil, System.Text.Encoding.Default)
                fs.BaseStream.Seek(0, SeekOrigin.Begin)
                Dim a() As String = Nothing
                a = fs.ReadLine().Split(":")
                split(0) = a(1) 'DATABASE

                a = fs.ReadLine().Split(":")
                split(1) = a(1) 'WebServername 'E.g sql5004.site4now.net 

                a = fs.ReadLine().Split(":")
                split(2) = a(1) 'username' E.g sa

                a = fs.ReadLine().Split(":")
                split(3) = a(1) 'Password 
                If split(3) <> "" Then
                    split(3) = BrighttechPack.Decrypt(split(3)) 'Password
                Else
                    split(3) = ""
                End If

                a = fs.ReadLine().Split(":")
                split(4) = a(1)
                ftpUrl = "ftp://" & split(4) ' FTP Url

                If Not ftpUrl.EndsWith("/") Then
                    ftpUrl = "ftp://" & split(4) & "/"
                End If

                a = fs.ReadLine().Split(":")
                split(5) = a(1)
                ftpUrlUserName = split(5) ' FTP UserName

                a = fs.ReadLine().Split(":")
                split(6) = a(1)
                ftpUrlPassword = split(6) ' FTP Password

            Catch ex As Exception
                Return False
            End Try
        Else
            MsgBox("CatInfo not Available")
            Return False
        End If
        Dim ErrorCnt As Integer
        Try
            Webcn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & split(0) & ";Data Source={0};User Id=" & split(2) & ";password=" & split(3) & ";", split(1)))
            Webcn.Open()
        Catch ex As Exception
            ErrorCnt += 1
            If ErrorCnt < 10 Then
                Threading.Thread.Sleep(2000)
            End If
            MsgBox("Connection Problem online" + vbCrLf + ex.Message)
            Return False
        End Try
        Return True
    End Function
    Private Sub LocalToInternet()
        Dim dtCatlog As New DataTable
        dtCatlog = gridView_OWN.DataSource

        Dim dtDistinctItemId As New DataTable
        Dim dtDistinctItemidSubItemId As New DataTable
        Dim dtTemp As New DataTable
        dtDistinctItemId = dtCatlog.DefaultView.ToTable(True, "ITEMID")
        dtDistinctItemidSubItemId = dtCatlog.DefaultView.ToTable(True, "ITEMID", "SUBITEMID")

        'transfer itemmast to online

        If rbtAll.Checked = True Or rbtMaster.Checked = True Then
            If dtDistinctItemId.Rows.Count > 0 Then
                For t As Integer = 0 To dtDistinctItemId.Rows.Count - 1
                    strsql = "SELECT * FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & dtDistinctItemId.Rows(t).Item("ITEMID").ToString & "'"
                    dtTemp = New DataTable
                    da = New OleDbDataAdapter(strsql, cn)
                    da.Fill(dtTemp)
                    If dtTemp.Rows.Count > 0 Then
                        dtTemp.TableName = "ITEMMAST"
                        strsql = " DELETE " & split(0) & "..ITEMMAST WHERE ITEMID = '" & dtDistinctItemId.Rows(t).Item("ITEMID").ToString & "' "
                        cmd = New OleDbCommand(strsql, Webcn)
                        cmd.ExecuteNonQuery()
                        For Each ro As DataRow In dtTemp.Rows
                            Dim Qry As String
                            Qry = InsertQry(ro, split(0))
                            cmd = New OleDbCommand(Qry, Webcn)
                            cmd.ExecuteNonQuery()
                        Next
                    End If
                Next
            End If
        End If

        'transfer subitemmast to online
        If rbtMaster.Checked = True Or rbtAll.Checked = True Then
            If dtDistinctItemidSubItemId.Rows.Count > 0 Then
                For t As Integer = 0 To dtDistinctItemidSubItemId.Rows.Count - 1
                    strsql = " SELECT * FROM " & cnAdminDb & "..SUBITEMMAST "
                    strsql += " WHERE ITEMID = '" & dtDistinctItemidSubItemId.Rows(t).Item("ITEMID").ToString & "' AND SUBITEMID = '" & dtDistinctItemidSubItemId.Rows(t).Item("SUBITEMID").ToString & "' "
                    dtTemp = New DataTable
                    da = New OleDbDataAdapter(strsql, cn)
                    da.Fill(dtTemp)
                    If dtTemp.Rows.Count > 0 Then
                        dtTemp.TableName = "SUBITEMMAST"
                        strsql = " DELETE " & split(0) & "..SUBITEMMAST WHERE ITEMID = '" & dtDistinctItemidSubItemId.Rows(t).Item("ITEMID").ToString & "' AND SUBITEMID = '" & dtDistinctItemidSubItemId.Rows(t).Item("SUBITEMID").ToString & "'"
                        cmd = New OleDbCommand(strsql, Webcn)
                        cmd.ExecuteNonQuery()
                        For Each ro As DataRow In dtTemp.Rows
                            Dim Qry As String
                            Qry = InsertQry(ro, split(0))
                            cmd = New OleDbCommand(Qry, Webcn)
                            cmd.ExecuteNonQuery()
                        Next
                    End If
                Next
            End If
        End If

        ' transfer itemdetail to online
        If rbtAll.Checked = True Or rbtItemDetail.Checked = True Then
            With dtCatlog
                If .Rows.Count > 0 Then
                    For i As Integer = 0 To .Rows.Count - 1
                        If .Rows(i).Item("ROWID").ToString = "" Then Continue For
                        If .Rows(i).Item("CHECK").ToString = "False" Then Continue For
                        If .Rows(i).Item("ROWID").ToString <> "" Then

                            Dim BoardRate As Double = 0
                            Dim ItemRate As Double = 0

                            Dim metalid As String = ""
                            Dim drMetalRate As DataRow = Nothing
                            strsql = "SELECT METALID FROM " & cnAdminDb & "..ITEMMAST  WHERE ITEMID = " & .Rows(i).Item("ITEMID").ToString & ""
                            metalid = GetSqlValue(cn, strsql).ToString


                            strsql = vbCrLf + " SELECT TOP 1 MAX(RATEGROUP)RATEGROUP"
                            strsql += vbCrLf + " ,SRATE,RDATE FROM " & cnAdminDb & "..RATEMAST WHERE METALID = '" & metalid & "' AND PURITY = '91.60' "
                            strsql += vbCrLf + " GROUP BY SRATE,RDATE "
                            strsql += vbCrLf + " ORDER BY RDATE DESC"
                            drMetalRate = GetSqlRow(strsql, cn)
                            If Not drMetalRate Is Nothing Then
                                BoardRate = Val(drMetalRate.Item("SRATE").ToString)
                                ItemRate = Val(drMetalRate.Item("SRATE").ToString)
                            Else
                                MsgBox("Rate should not update in master")
                                Exit Sub
                            End If

                            Dim grsnet As String
                            Dim grswt As Decimal
                            Dim netwt As Decimal
                            Dim lesswt As Decimal
                            Dim wastper As Decimal
                            Dim wast As Decimal
                            Dim mcgrm As Decimal
                            Dim mcharge As Decimal
                            Dim baseRate As Decimal
                            Dim Amount As Decimal
                            Dim taxper As Decimal
                            Dim tax As Decimal
                            Dim imagePath() As String = .Rows(i).Item("IMAGEPATH").ToString().Split(",")
                            Dim itemName As String = .Rows(i).Item("ITEMNAME").ToString()
                            Dim subitemName As String = .Rows(i).Item("SUBITEMNAME").ToString()
                            Dim imageurl As String = .Rows(i).Item("IMAGENAME").ToString()

                            strsql = "select S_IGSTTAX from " & cnAdminDb & "..CATEGORY where CATCODE in (select CATCODE from " & cnAdminDb & "..ITEMMAST where ITEMID = " & .Rows(i).Item("ITEMID").ToString & ") "
                            taxper = GetSqlValue(cn, strsql)


                            grsnet = .Rows(i).Item("GRSNET").ToString
                            grswt = IIf(Val(.Rows(i).Item("GRSWT").ToString()) = 0, 0, .Rows(i).Item("GRSWT").ToString)
                            netwt = IIf(Val(.Rows(i).Item("NETWT").ToString()) = 0, 0, .Rows(i).Item("NETWT").ToString)
                            lesswt = IIf(Val(.Rows(i).Item("LESSWT").ToString()) = 0, 0, .Rows(i).Item("LESSWT").ToString)
                            wastper = Val(.Rows(i).Item("WAST%").ToString)
                            wast = Val(.Rows(i).Item("WAST").ToString)
                            mcgrm = Val(.Rows(i).Item("MCGRM").ToString)
                            mcharge = Val(.Rows(i).Item("MC").ToString())
                            If grsnet = "N" Then
                                grswt = netwt
                            End If

                            baseRate = ((grswt + wast) * (BoardRate))

                            Amount = baseRate + mcharge
                            Amount = Math.Round(Amount, 2)

                            tax = (Amount * taxper) / 100
                            tax = Math.Round(tax, 2)

                            strsql = vbCrLf + " SELECT '' SKU "
                            strsql += vbCrLf + " , '" & .Rows(i).Item("TAGNO").ToString & "' TAGNO "
                            strsql += vbCrLf + " , '" & .Rows(i).Item("PCS").ToString & "' PCS "
                            strsql += vbCrLf + " , '" & grswt & "' GRSWT "
                            strsql += vbCrLf + " , '" & netwt & "' NETWT "
                            strsql += vbCrLf + " , '" & lesswt & "' LESSWT "
                            strsql += vbCrLf + " , '" & .Rows(i).Item("ITEMID").ToString() & "' ITEMID "
                            strsql += vbCrLf + " , '" & wastper & "' WASTPER "
                            strsql += vbCrLf + " , '" & wast & "' WASTAGE "
                            strsql += vbCrLf + " , '" & Format(baseRate, "0.00") & "' BASERATE "
                            strsql += vbCrLf + " , '" & mcgrm & "' MCGRM "
                            strsql += vbCrLf + " , '" & mcharge & "' MCHARGE "
                            strsql += vbCrLf + " , '" & Format(Amount, "0.00") & "' AMOUNT "
                            strsql += vbCrLf + " , '" & BoardRate & "' RATE "
                            strsql += vbCrLf + " , '" & BoardRate & "' BOARDRATE "
                            strsql += vbCrLf + " , '" & grsnet & "' GRSNET "
                            strsql += vbCrLf + " , '' COSTID " ''" & .Rows(i).Item("COSTID").ToString & "'
                            strsql += vbCrLf + " , '' FLAG "
                            strsql += vbCrLf + " , 0 TAGRATEID "
                            strsql += vbCrLf + " , 0 TAGSVALUE "
                            strsql += vbCrLf + " , '" & .Rows(i).Item("PURITY").ToString & "' PURITY "
                            strsql += vbCrLf + " , '' VATEXM "
                            strsql += vbCrLf + " , '" & metalid & "'  METALID "
                            strsql += vbCrLf + " , '" & Format(tax, "0.00") & "' TAX "
                            strsql += vbCrLf + " , 0 STNAMT "
                            strsql += vbCrLf + " , '" & itemName & "' AS [ITEMNAME]"
                            strsql += vbCrLf + " , '" & subitemName & "' AS [SUBITEMNAME]"
                            strsql += vbCrLf + " , '" & imageurl & "' AS [IMAGEURL]"
                            Dim dtTransfer As New DataTable
                            da = New OleDbDataAdapter(strsql, cn)
                            da.Fill(dtTransfer)
                            If dtTransfer.Rows.Count > 0 Then
                                dtTransfer.TableName = "ITEMDETAIL"
                                For Each ro As DataRow In dtTransfer.Rows

                                    strsql = " DELETE " & split(0) & "..ITEMDETAIL WHERE ITEMID = '" & .Rows(i).Item("ITEMID").ToString & "' "
                                    strsql += vbCrLf + " AND TAGNO = '" & .Rows(i).Item("TAGNO").ToString & "' "
                                    cmd = New OleDbCommand(strsql, Webcn)
                                    cmd.ExecuteNonQuery()

                                    Dim Qry As String
                                    Qry = InsertQry(ro, split(0))
                                    cmd = New OleDbCommand(Qry, Webcn)
                                    cmd.ExecuteNonQuery()

                                    If imagePath.Length > 0 Then
                                        For s As Integer = 0 To imagePath.Length - 1
                                            Dim extenstion As String = System.IO.Path.GetFileName(imagePath(s).ToString)
                                            If extenstion = "" Then Continue For
                                            FtpUploadFile(imagePath(s).ToString, ftpUrl & extenstion, ftpUrlUserName, ftpUrlPassword)
                                        Next
                                    End If

                                Next
                            End If
                        End If
                    Next
                End If
            End With
        End If

        MsgBox("Completed")
        btnNew_Click(Me, New System.EventArgs)
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        strsql = vbCrLf + " SELECT NAME FROM " & cnAdminDb & "..SYSCOLUMNS WHERE ID IN (SELECT ID FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME='ITEMTAG')"
        strsql += vbCrLf + " ORDER BY NAME"
        objGPack.FillCombo(strsql, cmbSearchKey, True, False)
        cmbSearchKey.Text = ""
        gridView_OWN.DataSource = Nothing
        gridView_OWN.Rows.Clear()
        gridView_OWN.Columns.Clear()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "Catlog Transfer", gridView_OWN, BrightPosting.GExport.GExportType.Export)
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "Catlog Transfer", gridView_OWN, BrightPosting.GExport.GExportType.Print)
    End Sub

    Private Function filterBy() As String
        Dim Qry As String = ""
        If chkRecDate.Checked = True Then Qry += vbCrLf + " AND I.RECDATE = '" & Format(dtpDate.Value.Date, "yyyy-MM-dd") & "' "
        If txtItemId.Text.Trim <> "" Then Qry += vbCrLf + " AND I.ITEMID IN ('" & txtItemId.Text.Trim & "') "
        If txtTagNo.Text.Trim <> "" Then Qry += vbCrLf + " AND I.TAGNO IN ('" & txtTagNo.Text.Trim & "') "
        If CmbDesigner.Text <> "ALL" And CmbDesigner.Text <> "" Then Qry += vbCrLf + " AND I.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & CmbDesigner.Text & "') "
        If CmbStockType.Text <> "ALL" And CmbStockType.Text <> "" Then Qry += vbCrLf + " AND ISNULL(I.STKTYPE,'') = '" & Mid(CmbStockType.Text, 1, 1) & "' "
        If txtSearch.Text <> "" Then Qry += vbCrLf + " AND I." & cmbSearchKey.Text & " = '" & txtSearch.Text.Trim & "' " 'ISNULL
        If metalName.Trim <> "" Then Qry += vbCrLf + " AND  I.ITEMID IN (select ITEMID from " & cnAdminDb & "..ITEMMAST where METALID in (select METALID from " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & metalName & ") )) "
        Return Qry
    End Function

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try

            btnSearch.Enabled = False
            gridView_OWN.DataSource = Nothing

            metalName = ""
            If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then metalName = GetQryString(cmbMetal.Text) '.Replace("'", "")
            'ROW_NUMBER() OVER(ORDER BY ITEMNAME,SUBITEMNAME) ROWID
            strsql = " SELECT ROW_NUMBER() OVER(ORDER BY ITEMNAME, SUBITEMNAME) ROWID,* FROM ("
            strsql += vbCrLf + " SELECT "
            strsql += vbCrLf + " CONVERT(VARCHAR(15),RECDATE,103) RECDATE "
            strsql += vbCrLf + " ,ITEMID"
            strsql += vbCrLf + " ,SUBITEMID"
            strsql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) ITEMNAME"
            strsql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID AND ITEMID =I.ITEMID) SUBITEMNAME"
            strsql += vbCrLf + " ,TAGNO"
            strsql += vbCrLf + " ,PCS,GRSWT,NETWT,LESSWT"
            strsql += vbCrLf + " ,MAXWASTPER [WAST%],MAXMCGRM [WAST]"
            strsql += vbCrLf + " ,MAXWAST AS [MCGRM],MAXMC AS [MC]"
            strsql += vbCrLf + " ,COSTID,PURITY,GRSNET "
            strsql += vbCrLf + " ,CONVERT(VARCHAR(8000),'') IMAGEPATH "
            strsql += vbCrLf + " ,CONVERT(VARCHAR(8000),'') IMAGENAME "
            strsql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS I WHERE ISSDATE IS NULL"
            strsql += vbCrLf + " " & filterBy() & " "
            strsql += vbCrLf + " )X "
            strsql += vbCrLf + " ORDER BY ITEMNAME,SUBITEMNAME"
            Dim dtGridview As New DataTable
            Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
            dtCol.DefaultValue = chkSelect.Checked 'True
            dtGridview.Columns.Add(dtCol)
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dtGridview)
            If dtGridview.Rows.Count = 0 Then
                MsgBox("No Record Found", MsgBoxStyle.Information)
                btnSearch.Enabled = True
                dtpDate.Focus()
                Exit Sub
            End If
            With gridView_OWN
                .DataSource = Nothing
                .Rows.Clear()
                .Columns.Clear()
                .DataSource = dtGridview
                With .Columns("ROWID")
                    .ReadOnly = True
                End With
                With .Columns("RECDATE")
                    .ReadOnly = True
                End With
                With .Columns("ITEMID")
                    .ReadOnly = True
                    .Visible = False
                End With
                With .Columns("SUBITEMID")
                    .ReadOnly = True
                    .Visible = False
                End With
                With .Columns("ITEMNAME")
                    .ReadOnly = True
                End With
                With .Columns("SUBITEMNAME")
                    .ReadOnly = True
                End With
                With .Columns("TAGNO")
                    .ReadOnly = True
                End With
                With .Columns("PCS")
                    .ReadOnly = True
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("GRSWT")
                    .ReadOnly = True
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("NETWT")
                    .ReadOnly = True
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("LESSWT")
                    .ReadOnly = True
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("WAST%")
                    .ReadOnly = True
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("WAST")
                    .ReadOnly = True
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("MCGRM")
                    .ReadOnly = True
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("MC")
                    .ReadOnly = True
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("PURITY")
                    .ReadOnly = True
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("COSTID")
                    .ReadOnly = True
                End With
                With .Columns("PURITY")
                    .ReadOnly = True
                End With
                With .Columns("GRSNET")
                    .ReadOnly = True
                End With
                With .Columns("IMAGEPATH")
                    .ReadOnly = True
                    .Visible = False
                End With
                With .Columns("IMAGENAME")
                    .ReadOnly = True
                End With
                Dim dgvBtnBrowse As New DataGridViewButtonColumn
                dgvBtnBrowse.Name = "btnBrowse"
                dgvBtnBrowse.Width = gridView_OWN.RowTemplate.Height
                dgvBtnBrowse.Text = "Browse"
                dgvBtnBrowse.HeaderText = ""
                gridView_OWN.Columns.Insert(19, dgvBtnBrowse)

                'Dim dgvCheckBox As New DataGridViewCheckBoxColumn
                'dgvCheckBox.Name = "chkChecked"
                'dgvCheckBox.Width = gridView_OWN.RowTemplate.Height
                'dgvCheckBox.HeaderText = ""
                'gridView_OWN.Columns.Insert(6, dgvCheckBox)

                AutoSize()

            End With
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        Finally
            btnSearch.Enabled = True
        End Try
    End Sub

    Private Sub txtItemId_KeyDown(sender As Object, e As KeyEventArgs) Handles txtItemId.KeyDown
        If e.KeyCode = Keys.Insert Then
            strsql = " SELECT ITEMID,ITEMNAME,"
            strsql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' "
            strsql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END AS STOCKTYPE,"
            strsql += " CASE WHEN SUBITEM = 'Y' THEN 'YES' ELSE 'NO' END AS SUBITEM, "
            strsql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
            strsql += " WHEN CALTYPE = 'R' THEN 'RATE'"
            strsql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
            strsql += " WHEN CALTYPE = 'B' THEN 'BOTH'"
            strsql += " WHEN CALTYPE = 'M' THEN 'METAL RATE' END AS CALTYPE"
            strsql += " FROM " & cnAdminDb & "..ITEMMAST"
            'strsql += " WHERE ITEMID LIKE '" & txtItemCode_Num_Man.Text & "%'"
            strsql += " WHERE ACTIVE = 'Y'"
            strsql += " AND STUDDED <> 'S'"
            strsql += GetItemQryFilteration("S")
            strsql += " ORDER BY ITEMNAME"
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            txtItemId.Text = BrighttechPack.SearchDialog.Show("Search ItemId", strsql, cn, 2)
        End If
    End Sub

    Private Sub AutoSize()
        gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        gridView_OWN.Invalidate()
        For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
            dgvCol.Width = dgvCol.Width
        Next
        gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView_OWN.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView_OWN.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub gridView_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridView_OWN.CellClick
        If gridView_OWN.Rows.Count > 0 Then
            With gridView_OWN
                If UCase(gridView_OWN.Columns(e.ColumnIndex).Name) = "BTNBROWSE" Then
                    If IO.File.Exists(defaultSourcePath) Then 'Or IO.Directory.Exists(defaultDestination)
                        openDia.InitialDirectory = defaultSourcePath
                    End If
                    Dim str As String
                    str = "JPEG(*.jpg)|*.jpg"
                    Str += "|Bitmap(*.bmp)|*.bmp"
                    Str += "|GIF(*.gif)|*.gif"
                    Str += "|All Files(*.*)|*.*"
                    openDia.Filter = str
                    openDia.Multiselect = True

                    Dim getPICPATH As String
                    Dim getPICPATHNAME As String
                    Dim PICPATH1 As String()

                    If openDia.ShowDialog = Windows.Forms.DialogResult.OK Then
                        Dim Finfo As IO.FileInfo
                        Finfo = New IO.FileInfo(openDia.FileName)
                        picExtension = Finfo.Extension
                        getPICPATH = "" 'PICPATH = openDia.FileName
                        getPICPATHNAME = ""
                        PICPATH1 = openDia.FileNames
                        If gridView_OWN.Rows(e.RowIndex).Cells("IMAGEPATH").Value.ToString <> "" Then
                            getPICPATH = gridView_OWN.Rows(e.RowIndex).Cells("IMAGEPATH").Value
                            getPICPATHNAME = gridView_OWN.Rows(e.RowIndex).Cells("IMAGENAME").Value
                            If PICPATH1.Length > 0 Then
                                For i As Integer = 0 To PICPATH1.Length - 1
                                    getPICPATH += "," & PICPATH1(i).ToString
                                    getPICPATHNAME += "," & System.IO.Path.GetFileName(PICPATH1(i).ToString)
                                Next
                            End If
                        Else
                            If PICPATH1.Length > 0 Then
                                For i As Integer = 0 To PICPATH1.Length - 1
                                    getPICPATH += "," & PICPATH1(i).ToString
                                    getPICPATHNAME += "," & System.IO.Path.GetFileName(PICPATH1(i).ToString)
                                Next
                            End If
                        End If
                        gridView_OWN.Rows(e.RowIndex).Cells("IMAGEPATH").Value = getPICPATH.Trim(",")
                        gridView_OWN.Rows(e.RowIndex).Cells("IMAGENAME").Value = getPICPATHNAME.Trim(",")
                        AutoSize()
                    End If
                ElseIf UCase(gridView_OWN.Columns(e.ColumnIndex).Name) = "CHECK" Then
                    If gridView_OWN.Rows(e.RowIndex).Cells("CHECK").Value.ToString = "True" Then
                        gridView_OWN.Rows(e.RowIndex).Cells("TAGNO").Style.BackColor = Color.Red
                    Else
                        gridView_OWN.Rows(e.RowIndex).Cells("TAGNO").Style.BackColor = Color.Lavender
                    End If
                End If
            End With
        End If
    End Sub

    Private Sub chkSelect_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelect.CheckedChanged
        If gridView_OWN.Rows.Count > 0 Then
            For M As Integer = 0 To gridView_OWN.Rows.Count - 1
                gridView_OWN.Rows(M).Cells("CHECK").Value = chkSelect.Checked
            Next
        End If
    End Sub
#End Region

#Region "USER DEFINE FUNCTION"
    Private Sub CheckingTransfer()
        'Dim wrUpload As FtpWebRequest = DirectCast(WebRequest.Create("ftp://ftp.site4now.net/file.txt"), FtpWebRequest)
        'wrUpload.Credentials = New NetworkCredential("akshayagold-001", "giritech999")
        'wrUpload.Method = WebRequestMethods.Ftp.UploadFile
        'Dim btfile() As Byte = File.ReadAllBytes("e:\file.txt")
        'Dim strFile As Stream = wrUpload.GetRequestStream()
        'strFile.Write(btfile, 0, btfile.Length)
        'strFile.Close()
        'strFile.Dispose()
        'Dim SourceFile As String = "D:\text.txt"
        'Dim address As String = "http://test.saravanastoreslegend.com/uploads/L15926I3T14A95214.jpg"
        'Dim _userid As String = "akshayagold-001"
        'Dim _password As String = "giritech999"
        'My.Computer.Network.UploadFile(SourceFile, (address), _userid, _password, True, 500, FileIO.UICancelOption.DoNothing)
        'My.Computer.Network.UploadFile(SourceFile, (address), _userid, _password)
        'Using client As New WebClient

        '    client.Credentials = New System.Net.NetworkCredential(_userid, _password)
        '    client.BaseAddress = address
        '    client.UploadFile(WebRequestMethods.File.UploadFile, SourceFile)
        'End Using
        'FtpUploadFile("e:\file.txt", "ftp://www.test.saravanastoreslegend.com/Uploads/", "akshayagold-001", "giritech999") 'file.txt
    End Sub
#End Region
End Class