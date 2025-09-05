Imports System.Data.OleDb
Imports System.IO
Public Class TagCatalog
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim Tran As OleDbTransaction = Nothing
    Dim DefalutDestination As String = Nothing
    Dim PicExtension As String = Nothing
    Dim PicPath As String = Nothing

    Dim DtCataLog As New DataTable

    Dim objStone As CatalogStoneDia
    Dim UpdateFlag As Boolean
    Dim UPdStyleNo As String
    Dim UpdSno As String

    Dim TAGCATALOG_DESINER As Boolean = IIf(GetSoftValue("TAGCATALOG_DESINER") = "Y", True, False)

    '#    For Image Capture          #
    Dim oldpath, oldpath1, newpath, newpath1 As String
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

    '#    For Image Capture          #

    Private Sub Initializer()
        ' Add any initialization after the InitializeComponent() call.
        AddHandler cmbItem_MAN.GotFocus, AddressOf Combo_Gotfocus
        AddHandler cmbItem_MAN.LocationChanged, AddressOf Combo_LostFocus
        AddHandler cmbItem_MAN.TextChanged, AddressOf Combo_TextChange
        AddHandler cmbSubitem_MAN.GotFocus, AddressOf Combo_Gotfocus
        AddHandler cmbSubitem_MAN.LostFocus, AddressOf Combo_LostFocus
        AddHandler cmbSubitem_MAN.TextChanged, AddressOf Combo_TextChange
        AddHandler cmbItemType_MAN.GotFocus, AddressOf Combo_Gotfocus
        AddHandler cmbItemType_MAN.LostFocus, AddressOf Combo_LostFocus
        AddHandler cmbItemType_MAN.TextChanged, AddressOf Combo_TextChange
        AddHandler cmbSize_OWN.GotFocus, AddressOf Combo_Gotfocus
        AddHandler cmbSize_OWN.LostFocus, AddressOf Combo_LostFocus
        AddHandler cmbSize_OWN.TextChanged, AddressOf Combo_TextChange

        pnlSearch.Location = New Point(702, 8)
        pnlSearch.Size = New Size(209, 327)
        Me.Controls.Add(pnlSearch)
        pnlSearch.BringToFront()

        With DtCataLog
            .Columns.Add("ITEM", GetType(String))
            .Columns.Add("SUBITEM", GetType(String))
            .Columns.Add("STYLENO", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Integer))
            .Columns.Add("LESSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("ITEMTYPE", GetType(String))
            .Columns.Add("SIZE", GetType(String))
            .Columns.Add("CALCMODE", GetType(String))
            .Columns.Add("NARRATION", GetType(String))
            .Columns.Add("PICFILENAME", GetType(String))
        End With
        gridView.DataSource = DtCataLog
        gridView.Columns("ITEMTYPE").Visible = False
        gridView.Columns("SIZE").Visible = False
        gridView.Columns("LESSWT").Visible = False
        gridView.Columns("NARRATION").Visible = False
        gridView.Columns("CALCMODE").Visible = False

        cmbGrsNet.Items.Clear()
        cmbGrsNet.Items.Add("GROSS")
        cmbGrsNet.Items.Add("NET")
        StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGCATALOGPATH'"
        DefalutDestination = UCase(objGPack.GetSqlValue(StrSql, "CTLTEXT", , Tran))
        If Not DefalutDestination.EndsWith("\") And DefalutDestination <> "" Then DefalutDestination += "\"
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Initializer()
    End Sub

    Public Sub New(ByVal updSno As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Initializer()
        btnNew_Click(Me, New EventArgs)
        Dim dtTemp As New DataTable
        StrSql = " DECLARE @PATH AS VARCHAR(100)"
        StrSql += " SELECT @PATH = '" & DefalutDestination & "'"
        StrSql += " SELECT"
        StrSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)aS ITEM"
        StrSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID AND ITEMID = T.ITEMID)AS SUBITEM"
        StrSql += " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)aS ITEMTYPE"
        StrSql += " ,SIZEDESC"
        StrSql += " ,STYLENO,PCS,GRSWT,LESSWT,NETWT"
        StrSql += " ,CASE WHEN GRSNET = 'G' THEN 'GROSS' ELSE 'NET' END AS GRSNET,NARRATION"
        StrSql += " ,PCTFILE"
        StrSql += " ,@PATH + PCTFILE  AS PICPATH"
        StrSql += " FROM " & cnAdminDb & "..TAGCATALOG AS T"
        StrSql += " WHERE SNO = '" & updSno & "'"
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtTemp)
        If Not dtTemp.Rows.Count > 0 Then
            Me.Close()
            Exit Sub
        End If
        With dtTemp.Rows(0)
            cmbItem_MAN.Text = .Item("ITEM").ToString
            cmbSubitem_MAN.Text = .Item("SUBITEM").ToString
            cmbItemType_MAN.Text = .Item("ITEMTYPE").ToString
            cmbSize_OWN.Text = .Item("SIZEDESC").ToString
            txtStyleNo_NUM.Text = .Item("STYLENO").ToString
            UPdStyleNo = .Item("STYLENO").ToString
            txtPcs_NUM_MAN.Text = .Item("PCS").ToString
            txtGrsWt_WET_MAN.Text = .Item("gRSWT").ToString
            txtLessWt_WET.Text = .Item("LESSWT").ToString
            txtNetWt_WET.Text = .Item("NETWT").ToString
            cmbGrsNet.Text = .Item("GRSNET").ToString
            txtNarration.Text = .Item("NARRATION").ToString
            Dim Finfo As FileInfo
            Finfo = New FileInfo(.Item("PICPATH").ToString)
            PicPath = .Item("PICPATH").ToString
            PicExtension = Finfo.Extension
            AutoImageSizer(.Item("PICPATH").ToString, picTagImage, PictureBoxSizeMode.CenterImage)
            Finfo = Nothing

            StrSql = vbCrLf + " SELECT "
            StrSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID)aS ITEM"
            StrSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.STNSUBITEMID AND ITEMID = T.STNITEMID)AS SUBITEM"
            StrSql += vbCrLf + " ,STNPCS AS PCS,STNWT AS WEIGHT"
            StrSql += vbCrLf + " ,(SELECT SIZEDESC FROM " & cnAdminDb & "..CENTSIZE WHERE ITEMID = T.STNITEMID AND SUBITEMID = T.STNSUBITEMID "
            StrSql += vbCrLf + "    AND (CASE WHEN STNPCS > 0 THEN STNWT/STNPCS ELSE STNWT END) BETWEEN FROMCENT AND TOCENT"
            StrSql += vbCrLf + " )AS [SIZE]"
            StrSql += vbCrLf + " ,STONEUNIT UNIT,CALCMODE CALC"
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..TAGCATALOGSTONE T"
            StrSql += " WHERE TAGSNO = '" & updSno & "'"
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(objStone.dtGridStone)
            objStone.dtGridStone.AcceptChanges()
            objStone.CalcStoneWtAmount()
        End With
        UpdateFlag = True
        Me.UpdSno = updSno
        btnNew.Enabled = False
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        StrSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        StrSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y' AND STOCKTYPE = 'T' "
        StrSql += GetItemQryFilteration("S")
        StrSql += " ORDER BY ITEMNAME"
        objGPack.FillCombo(StrSql, cmbItem_MAN, , False)
        StrSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE ORDER BY NAME"
        objGPack.FillCombo(StrSql, cmbItemType_MAN, , False)
        cmbGrsNet.Text = "GROSS"
        objStone = New CatalogStoneDia
        PicPath = Nothing
        PicExtension = Nothing
        'picTagImage.Image = My.Resources.no_photo
        AutoImageSizer(My.Resources.no_photo, picTagImage, PictureBoxSizeMode.StretchImage)
        txtDesiner.Focus()
        If Not TAGCATALOG_DESINER Then
            pnlDesiner.Visible = False
            pnlMain.Location = New Size(10, 8)
            cmbItem_MAN.Focus()
        End If
        If flagDeviceMode = True Then picCapture.Visible = True
    End Sub

    Private Sub TagCatalog_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtGrsWt_WET_MAN.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub TagCatalog_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Leave
        Call CloseCam()
    End Sub

    Private Sub TagCatalog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        If UpdateFlag = False Then btnNew_Click(Me, New EventArgs)
        chkAutosnap()
        txtDesiner.Focus()
        If Not TAGCATALOG_DESINER Then
            pnlDesiner.Visible = False
            pnlMain.Location = New Size(10, 8)
            cmbItem_MAN.Focus()
        End If
    End Sub
    Private Function GetSoftValue(ByVal id As String) As String
        Return UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & id & "'", , "", tran))
    End Function
    Private Sub chkAutosnap()
        If GetSoftValue("AUTOSNAP") = "Y" Then
            flagDeviceMode = True
            btnBrowse.Text = "&Capture"
        Else
            btnBrowse.Text = "&Browse"
            flagDeviceMode = False
        End If

        If flagDeviceMode = True Then

            picCapture.Visible = True
            PictureBox1.Visible = True
            'Panel2.Location = New Point(192, 3)
            OpenForm()
        Else
            picCapture.Visible = False
            PictureBox1.Visible = False
        End If
    End Sub

    Private Sub piccap()
        Dim data As IDataObject
        Dim bmap As Image

        ' Copy image to clipboard 
        SendMessage(hHwnd, CAP_EDIT_COPY, 0, 0)
        ' Get image from clipboard and convert it to a bitmap 
        Data = Clipboard.GetDataObject()
        If Data.GetDataPresent(GetType(System.Drawing.Bitmap)) Then
            bmap = CType(Data.GetData(GetType(System.Drawing.Bitmap)), Image)
            picTagImage.Image = bmap

            picPath = "c:\tst.jpg"
            picExtension = "Jpg"
            picTagImage.Image.Save("c:\tst.jpg", System.Drawing.Imaging.ImageFormat.Jpeg)
            picCapture.Visible = False
        End If

    End Sub
    Private Sub OpenForm()
        Dim iHeight As Integer = picCapture.Height
        Dim iWidth As Integer = picCapture.Width
        ' Open Preview window in picturebox .
        ' Create a child window with capCreateCaptureWindowA so you can display it in a picturebox.

        hHwnd = capCreateCaptureWindowA(iDevice, WS_VISIBLE Or WS_CHILD, 0, 0, 640, _
            480, picCapture.Handle.ToInt32, 0)
        ' Connect to device
        If SendMessage(hHwnd, CAP_DRIVER_CONNECT, iDevice, 0) Then

            ' Set the preview scale
            SendMessage(hHwnd, CAP_SET_SCALE, True, 0)

            ' Set the preview rate in milliseconds
            SendMessage(hHwnd, CAP_SET_PREVIEWRATE, 66, 0)

            ' Start previewing the image from the camera 
            SendMessage(hHwnd, CAP_SET_PREVIEW, True, 0)

            ' Resize window to fit in picturebox 
            SetWindowPos(hHwnd, HWND_BOTTOM, 0, 0, picCapture.Width, picCapture.Height, _
                                   SWP_NOMOVE Or SWP_NOZORDER)

        Else
            ' Error connecting to device close window 
            DestroyWindow(hHwnd)

        End If
    End Sub


    Private Sub CloseCam()
        If flagDeviceMode = True Then
            ' Disconnect from device
            SendMessage(hHwnd, CAP_DRIVER_DISCONNECT, iDevice, 0)
            ' close window 
            DestroyWindow(hHwnd)
        End If
    End Sub

   
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        If Not IO.Directory.Exists(DefalutDestination) Then
            MsgBox(DefalutDestination & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
            Exit Sub
        End If
        If flagDeviceMode = True Then
            piccap()
            Exit Sub
        End If
        Try
            Dim openDia As New OpenFileDialog
            Dim str As String
            str = "JPEG(*.jpg)|*.jpg"
            str += "|Bitmap(*.bmp)|*.bmp"
            str += "|GIF(*.gif)|*.gif"
            str += "|All Files(*.*)|*.*"
            openDia.Filter = str
            If openDia.ShowDialog = Windows.Forms.DialogResult.OK Then
                AutoImageSizer(openDia.FileName, picTagImage)
                'picModel.Visible = True
                Dim Finfo As FileInfo
                Finfo = New FileInfo(openDia.FileName)
                'Finfo.IsReadOnly = False
                'Dim fStream As New FileStream(openDia.FileName, FileMode.Open)
                'picTagImage.Image = Image.FromStream(fStream)
                'fStream.Close()
                PicPath = openDia.FileName
                PicExtension = Finfo.Extension
                'If openDia.FilterIndex = 1 Then
                '    PicExtension = "JPG"
                'ElseIf openDia.FilterIndex = 2 Then
                '    PicExtension = "BMP"
                'ElseIf openDia.FilterIndex = 2 Then
                '    PicExtension = "GIF"
                'End If
                Me.SelectNextControl(btnBrowse, True, True, True, True)
            End If
        Catch ex As Exception
            MsgBox(E0016, MsgBoxStyle.Exclamation)
        End Try
    End Sub
    Private Sub Combo_Gotfocus(ByVal sender As Object, ByVal e As EventArgs)
        ListSearch(CType(sender, ComboBox))
    End Sub
    Private Sub Combo_LostFocus(ByVal sender As Object, ByVal e As EventArgs)
        HideSearch()
    End Sub
    Private Sub Combo_TextChange(ByVal sender As Object, ByVal e As EventArgs)
        cmbSearch_OWN.Text = CType(sender, ComboBox).Text
    End Sub
    Private Sub ListSearch(ByVal cmb As ComboBox)
        If Not cmb.Items.Count > 0 Then Exit Sub
        cmbSearch_OWN.Items.Clear()
        For Each obj As Object In cmb.Items
            cmbSearch_OWN.Items.Add(obj)
        Next
        pnlSearch.Visible = True
    End Sub

    Private Sub HideSearch()
        pnlSearch.Visible = False
        cmbSearch_OWN.Items.Clear()
    End Sub

    Private Sub cmbSize_OWN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSize_OWN.GotFocus
        cmbSize_OWN.BackColor = focusColor
    End Sub

    Private Sub cmbSize_OWN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSize_OWN.LostFocus
        cmbSize_OWN.BackColor = lostFocusColor
    End Sub

    Private Sub funcAdd()
        Dim dt As New DataTable
        dt = BrighttechPack.GlobalMethods.GetTableStructure(cnAdminDb, "TAGCATALOG", cn)
        Dim Syncdb As String = cnAdminDb
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", Tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix) <> 0 Then Syncdb = uprefix + usuffix
        End If

        Dim ro As DataRow = Nothing
        Dim Sno As String = ""
        Dim ItemId As Integer = Nothing
        Dim SubItemId As Integer = Nothing
        Dim StnItemId As Integer = Nothing
        Dim StnSubItemId As Integer = Nothing
        Dim StSno As String = ""
        Dim tSno As Integer = 0
        Dim TNo As Integer = Nothing
        Try
            Tran = cn.BeginTransaction
GETNTAGSNO:
            StrSql = " SELECT CTLTEXT AS SNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGCATALOGSNO'"
            tSno = Val(objGPack.GetSqlValue(StrSql, , , Tran))
            ''UPDATING 
            ''CATSNO INTO SOFTCONTROL
            StrSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & tSno + 1 & "' "
            StrSql += " WHERE CTLID = 'TAGCATALOGSNO' AND CONVERT(INT,CTLTEXT) = " & tSno & ""
            Cmd = New OleDbCommand(StrSql, cn, Tran)
            If Cmd.ExecuteNonQuery() = 0 Then
                GoTo GETNTAGSNO
            End If
            StrSql = " SELECT SNO FROM " & cnAdminDb & "..TAGCATALOG WHERE SNO = '" & cnCostId & (tSno + 1).ToString & "'"
            If objGPack.GetSqlValue(StrSql, , "-1", Tran) <> "-1" Then
                GoTo GETNTAGSNO
            End If
            Sno = cnCostId & (tSno + 1).ToString
            ItemId = Val(BrighttechPack.GlobalMethods.GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'", , , Tran))
            SubItemId = Val(BrighttechPack.GlobalMethods.GetSqlValue(cn, "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubitem_MAN.Text & "' AND ITEMID = " & ItemId & "", , , Tran))

            'txtStyleNo_NUM.Text = GetStyleNo(ItemId, SubItemId, GetEntryDate(GetServerDate(Tran), Tran), True)
GenCatNo:
            TNo = Val(Mid(txtStyleNo_NUM.Text, 5, 12))
            StrSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENT_STYLENO = '" & TNo & "' "
            StrSql += " WHERE ITEMID = " & ItemId & ""
            Cmd = New OleDbCommand(StrSql, cn, Tran)
            Cmd.ExecuteNonQuery()
            StrSql = " SELECT STYLENO FROM " & cnAdminDb & "..TAGCATALOG WHERE STYLENO = '" & txtStyleNo_NUM.Text & "'"
            If objGPack.GetSqlValue(StrSql, , "-1", Tran) <> "-1" Then
                txtStyleNo_NUM.Text = GetStyleNo(ItemId, SubItemId, GetEntryDate(GetServerDate(Tran), Tran), True)
                GoTo GenCatNo
            End If

            ro = dt.NewRow
            ro!SNO = Sno
            ro!STYLENO = txtStyleNo_NUM.Text
            ro!ITEMID = ItemId
            ro!SUBITEMID = SubItemId
            ro!PCS = Val(txtPcs_NUM_MAN.Text)
            ro!GRSWT = Val(txtGrsWt_WET_MAN.Text)
            ro!NETWT = Val(txtNetWt_WET.Text)
            ro!ITEMTYPEID = Val(BrighttechPack.GlobalMethods.GetSqlValue(cn, "SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'", , , Tran))
            ro!PCTFILE = txtStyleNo_NUM.Text & "." & PicExtension
            ro!SIZEDESC = cmbSize_OWN.Text
            ro!NARRATION = txtNarration.Text
            ro!DESIGNER = txtDesiner.Text
            ro!USERID = userId
            ro!UPDATED = GetEntryDate(GetServerDate(Tran), Tran)
            ro!UPTIME = GetServerTime(Tran)
            ro!LESSWT = Val(txtLessWt_WET.Text)
            ro!GRSNET = Mid(cmbGrsNet.Text, 1, 1)
            dt.Rows.Add(ro)
            If File.Exists(PicPath) = True Then
                Dim serverPath As String = Nothing
                Dim fileDestPath As String = (DefalutDestination & txtStyleNo_NUM.Text & "." & PicExtension)
                Dim Finfo As FileInfo
                Finfo = New FileInfo(fileDestPath)
                If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                    MsgBox(Finfo.Directory.FullName & " Does not exist. Please make a corresponding path", MsgBoxStyle.Information)
                    Tran.Rollback()
                    Exit Sub
                End If
                If UCase(PicPath) <> fileDestPath.ToUpper Then
                    Dim cFile As New FileInfo(PicPath)
                    cFile.CopyTo(fileDestPath, True)
                End If
            End If
            If InsertData(SyncMode.Master, dt, cn, Tran) = False Then Exit Sub
            If IO.File.Exists(DefalutDestination & txtStyleNo_NUM.Text & "." & PicExtension) Then
                StrSql = "SELECT COSTID,STATEID FROM " & cnAdminDb & "..SYNCCOSTCENTRE where COSTID <> '" & cnCostId & "'"
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                If Not Tran Is Nothing Then Cmd.Transaction = Tran
                Dim dtCostId As New DataTable
                Da = New OleDbDataAdapter(Cmd)
                Da.Fill(dtCostId)
                For Each row As DataRow In dtCostId.Rows
                    StrSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID"
                    StrSql += ",TAGIMAGE,TAGIMAGENAME,IMAGE_CTRLID"
                    StrSql += " )"
                    StrSql += " VALUES"
                    StrSql += " ('" & cnCostId & "','" & row.Item("COSTID").ToString & "',?,?,'TAGCATALOGPATH')"
                    Cmd = New OleDbCommand(StrSql, cn, Tran)
                    Dim fileStr As New IO.FileStream(DefalutDestination & txtStyleNo_NUM.Text & "." & PicExtension, IO.FileMode.Open, IO.FileAccess.Read)
                    Dim reader As New IO.BinaryReader(fileStr)
                    Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                    fileStr.Read(result, 0, result.Length)
                    fileStr.Close()
                    Cmd.Parameters.AddWithValue("@TAGIMAGE", result)
                    Dim fInfo As New IO.FileInfo(DefalutDestination & txtStyleNo_NUM.Text & "." & PicExtension)
                    Cmd.Parameters.AddWithValue("@TAGIMAGENAME", fInfo.Name)
                    Cmd.ExecuteNonQuery()
                Next
            End If
            dt = New DataTable
            dt = BrighttechPack.GlobalMethods.GetTableStructure(cnAdminDb, "TAGCATALOGSTONE", cn, Tran)
            For Each dgvRow As DataGridViewRow In objStone.gridStone.Rows
                ro = dt.NewRow
GETCatStoneSno:
                StrSql = " SELECT CTLTEXT AS SNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGCATALOGSTONESNO'"
                tSno = Val(objGPack.GetSqlValue(StrSql, , , Tran))
                ''UPDATING 
                ''CATSNO INTO SOFTCONTROL
                StrSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & tSno + 1 & "' "
                StrSql += " WHERE CTLID = 'TAGCATALOGSTONESNO' AND CONVERT(INT,CTLTEXT) = " & tSno & ""
                Cmd = New OleDbCommand(StrSql, cn, Tran)
                If Cmd.ExecuteNonQuery() = 0 Then
                    GoTo GETCatStoneSno
                End If
                StrSql = " SELECT SNO FROM " & cnAdminDb & "..TAGCATALOGSTONE WHERE SNO = '" & cnCostId & (tSno + 1).ToString & "'"
                If objGPack.GetSqlValue(StrSql, , "-1", Tran) <> "-1" Then
                    GoTo GETCatStoneSno
                End If
                StSno = cnCostId & (tSno + 1).ToString
                StnItemId = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & dgvRow.Cells("ITEM").Value.ToString & "'", , , Tran))
                StnSubItemId = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & dgvRow.Cells("SUBITEM").Value.ToString & "' AND ITEMID = " & StnItemId & "", , , Tran))
                ro!SNO = StSno
                ro!TAGSNO = Sno
                ro!ITEMID = ItemId
                ro!STYLENO = txtStyleNo_NUM.Text
                ro!STNITEMID = StnItemId
                ro!STNSUBITEMID = StnSubItemId
                ro!STNPCS = Val(dgvRow.Cells("PCS").Value.ToString)
                ro!STNWT = Val(dgvRow.Cells("WEIGHT").Value.ToString)
                ro!CALCMODE = dgvRow.Cells("CALC").Value.ToString
                ro!STONEUNIT = dgvRow.Cells("UNIT").Value.ToString
                dt.Rows.Add(ro)
            Next
            If InsertData(SyncMode.Master, dt, cn, Tran) = False Then Exit Sub
            Tran.Commit()
            Tran = Nothing
            Dim roCat As DataRow = DtCataLog.NewRow
            roCat.Item("ITEM") = cmbItem_MAN.Text
            roCat.Item("SUBITEM") = cmbSubitem_MAN.Text
            roCat.Item("ITEMTYPE") = cmbItemType_MAN.Text
            roCat.Item("SIZE") = cmbSize_OWN.Text
            roCat.Item("STYLENO") = txtStyleNo_NUM.Text
            roCat.Item("PCS") = Val(txtPcs_NUM_MAN.Text)
            roCat.Item("GRSWT") = Val(txtGrsWt_WET_MAN.Text)
            roCat.Item("LESSWT") = Val(txtLessWt_WET.Text)
            roCat.Item("NETWT") = Val(txtNetWt_WET.Text)
            roCat.Item("CALCMODE") = cmbGrsNet.Text
            roCat.Item("NARRATION") = txtNarration.Text
            roCat.Item("PICFILENAME") = DefalutDestination & txtStyleNo_NUM.Text & "." & PicExtension
            DtCataLog.Rows.Add(roCat)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not Tran Is Nothing Then Tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub funcUpdate()
        Try
            Dim ItemId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"))
            Dim subItemId As Integer = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubitem_MAN.Text & "' AND ITEMID = " & ItemId & ""))
            Dim itemTypeId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'"))
            Dim TNo As Integer = Nothing
            Tran = Nothing
            Tran = cn.BeginTransaction
GenCatNo:
            If UPdStyleNo <> txtStyleNo_NUM.Text Then
                TNo = Val(Mid(txtStyleNo_NUM.Text, 5, 12))
                StrSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENT_STYLENO = '" & TNo & "' "
                StrSql += " WHERE ITEMID = " & ItemId & ""
                Cmd = New OleDbCommand(StrSql, cn, Tran)
                Cmd.ExecuteNonQuery()
                StrSql = " SELECT STYLENO FROM " & cnAdminDb & "..TAGCATALOG WHERE STYLENO = '" & txtStyleNo_NUM.Text & "'"
                If objGPack.GetSqlValue(StrSql, , "-1", Tran) <> "-1" Then
                    txtStyleNo_NUM.Text = GetStyleNo(ItemId, subItemId, GetEntryDate(GetServerDate(Tran), Tran), True)
                    GoTo GenCatNo
                End If
            End If

            StrSql = " UPDATE " & cnAdminDb & "..TAGCATALOG SET"
            StrSql += " STYLENO = '" & txtStyleNo_NUM.Text & "'"
            StrSql += " ,ITEMID = " & ItemId & ""
            StrSql += " ,SUBITEMID = " & subItemId & ""
            StrSql += " ,PCS = " & Val(txtPcs_NUM_MAN.Text) & ""
            StrSql += " ,GRSWT = " & Val(txtGrsWt_WET_MAN.Text) & ""
            StrSql += " ,LESSWT = " & Val(txtLessWt_WET.Text) & ""
            StrSql += " ,NETWT = " & Val(txtNetWt_WET.Text) & ""
            StrSql += " ,GRSNET = '" & Mid(cmbGrsNet.Text, 1, 1) & "'"
            StrSql += " ,ITEMTYPEID = " & itemTypeId & ""
            StrSql += " ,PCTFILE = '" & txtStyleNo_NUM.Text & "." & PicExtension & "'"
            StrSql += " ,NARRATION = '" & txtNarration.Text & "'"
            StrSql += " WHERE SNO = '" & UpdSno & "'"
            If File.Exists(PicPath) = True Then
                Dim serverPath As String = Nothing
                Dim fileDestPath As String = (DefalutDestination & txtStyleNo_NUM.Text & "." & PicExtension)
                Dim Finfo As FileInfo
                Finfo = New FileInfo(fileDestPath)
                If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                    MsgBox(Finfo.Directory.FullName & " Does not exist. Please make a corresponding path", MsgBoxStyle.Information)
                    Tran.Rollback()
                    Exit Sub
                End If
                If PicPath.ToUpper <> fileDestPath.ToUpper Then
                    Dim cFile As New FileInfo(PicPath)
                    cFile.CopyTo(fileDestPath, True)
                End If
            End If
            ExecQuery(SyncMode.Master, StrSql, cn, Tran, , , DefalutDestination & txtStyleNo_NUM.Text & "." & PicExtension)
            StrSql = " DELETE FROM " & cnAdminDb & "..TAGCATALOGSTONE WHERE TAGSNO = '" & UpdSno & "'"
            ExecQuery(SyncMode.Master, StrSql, cn, Tran)

            Dim dt As DataTable
            Dim ro As DataRow = Nothing
            Dim StSno As String = ""
            Dim tSno As Integer = 0
            Dim StnItemId As Integer = Nothing
            Dim StnSubItemId As Integer = Nothing
            dt = New DataTable
            dt = BrighttechPack.GlobalMethods.GetTableStructure(cnAdminDb, "TAGCATALOGSTONE", cn, Tran)
            For Each dgvRow As DataGridViewRow In objStone.gridStone.Rows
                ro = dt.NewRow
GETCatStoneSno:
                StrSql = " SELECT CTLTEXT AS SNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGCATALOGSTONESNO'"
                tSno = Val(objGPack.GetSqlValue(StrSql, , , Tran))
                ''UPDATING 
                ''CATSNO INTO SOFTCONTROL
                StrSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & tSno + 1 & "' "
                StrSql += " WHERE CTLID = 'TAGCATALOGSTONESNO' AND CONVERT(INT,CTLTEXT) = " & tSno & ""
                Cmd = New OleDbCommand(StrSql, cn, Tran)
                If Cmd.ExecuteNonQuery() = 0 Then
                    GoTo GETCatStoneSno
                End If
                StrSql = " SELECT SNO FROM " & cnAdminDb & "..TAGCATALOGSTONE WHERE SNO = '" & cnCostId & (tSno + 1).ToString & "'"
                If objGPack.GetSqlValue(StrSql, , "-1", Tran) <> "-1" Then
                    GoTo GETCatStoneSno
                End If
                StSno = cnCostId & (tSno + 1).ToString
                StnItemId = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & dgvRow.Cells("ITEM").Value.ToString & "'", , , Tran))
                StnSubItemId = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & dgvRow.Cells("SUBITEM").Value.ToString & "' AND ITEMID = " & StnItemId & "", , , Tran))
                ro!SNO = StSno
                ro!TAGSNO = UpdSno
                ro!ITEMID = ItemId
                ro!STYLENO = txtStyleNo_NUM.Text
                ro!STNITEMID = StnItemId
                ro!STNSUBITEMID = StnSubItemId
                ro!STNPCS = Val(dgvRow.Cells("PCS").Value.ToString)
                ro!STNWT = Val(dgvRow.Cells("WEIGHT").Value.ToString)
                ro!CALCMODE = dgvRow.Cells("CALC").Value.ToString
                ro!STONEUNIT = dgvRow.Cells("UNIT").Value.ToString
                dt.Rows.Add(ro)
            Next
            If InsertData(SyncMode.Master, dt, cn, Tran) = False Then Exit Sub

            Tran.Commit()
            Tran = Nothing
            MsgBox("Update Successfully", MsgBoxStyle.Information)
            Me.Close()
        Catch ex As Exception
            If Not Tran Is Nothing Then Tran.Rollback()
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If objGPack.Validator_Check(Me) Then Exit Sub
        If Val(txtNetWt_WET.Text) > Val(txtGrsWt_WET_MAN.Text) Then
            MsgBox("Net weight should not exceed grossweight", MsgBoxStyle.Information)
            Exit Sub
        End If
        If Not IO.File.Exists(PicPath) Then
            MsgBox("Invalid FilePath", MsgBoxStyle.Information)
            btnBrowse.Focus()
            Exit Sub
        End If
        If UpdateFlag Then
            funcUpdate()
        Else
            funcAdd()
        End If
    End Sub

    Private Function GetStyleNo(ByVal ItemId As Integer, ByVal SubItemId As Integer, ByVal todayDate As Date, ByVal Update As Boolean) As String
        Dim CatNo As Integer = Nothing
        Dim CatalogNo As String = ""
        Dim itemStyleCode As String = objGPack.GetSqlValue("SELECT STYLECODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & ItemId & "", , , Tran)
        Dim subItemStyleCode As String = objGPack.GetSqlValue("SELECT STYLECODE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = " & SubItemId & " AND ITEMID = " & ItemId & "", , , Tran)
        'Dim TCatalogNo As String = _
        'FormatStringCustom(itemStyleCode.ToString, "0", 2) _
        '& FormatStringCustom(subItemStyleCode.ToString, "0", 2) _
        '& FormatStringCustom(todayDate.Month.ToString, "0", 2) _
        '& Mid(todayDate.Year.ToString, 3, 2)
        Dim TCatalogNo As String = _
        FormatStringCustom(itemStyleCode.ToString, "0", 2) _
        & FormatStringCustom(subItemStyleCode.ToString, "0", 2)
        Dim tno As Integer = 0
GETCATNO:
        tno = Val(objGPack.GetSqlValue("SELECT ISNULL(CONVERT(INT,CURRENT_STYLENO),0) FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & ItemId & "", , , Tran))
        CatalogNo = TCatalogNo & FormatStringCustom((tno + 1).ToString, "0", 4)
        Return CatalogNo
    End Function

    Private Sub txtStyleNo_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStyleNo_NUM.GotFocus
        If UPdStyleNo <> "" And UPdStyleNo = txtStyleNo_NUM.Text Then Exit Sub
        Dim ItemId As Integer
        Dim SubItemId As Integer
        ItemId = Val(BrighttechPack.GlobalMethods.GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'", , , Tran))
        SubItemId = Val(BrighttechPack.GlobalMethods.GetSqlValue(cn, "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubitem_MAN.Text & "'", , , Tran))
        txtStyleNo_NUM.Text = GetStyleNo(ItemId, SubItemId, GetEntryDate(GetServerDate), False)
    End Sub

    Private Sub txtGrsWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrsWt_WET_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If BrighttechPack.GlobalMethods.GetSqlValue(cn, "SELECT STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'") = "Y" Then
                objStone.grsWt = Val(txtGrsWt_WET_MAN.Text)
                objStone.StartPosition = FormStartPosition.CenterScreen
                objStone.ShowDialog()
                Dim stnWt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
                txtLessWt_WET.Text = IIf(stnWt <> 0, Format(stnWt, "0.000"), Nothing)
            End If
            Me.SelectNextControl(txtGrsWt_WET_MAN, True, True, True, True)
        End If
    End Sub

    Private Sub txtGrsWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrsWt_WET_MAN.TextChanged
        Dim netWt As Double = Val(txtGrsWt_WET_MAN.Text) - Val(txtLessWt_WET.Text)
        txtNetWt_WET.Text = IIf(netWt <> 0, Format(netWt, "0.000"), "")
    End Sub

    Private Sub txtLessWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLessWt_WET.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtLessWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLessWt_WET.TextChanged
        Dim netWt As Double = Val(txtGrsWt_WET_MAN.Text) - Val(txtLessWt_WET.Text)
        txtNetWt_WET.Text = IIf(netWt <> 0, Format(netWt, "0.000"), "")
    End Sub

    Private Sub txtNetWt_WET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNetWt_WET.LostFocus
        If Val(txtNetWt_WET.Text) > Val(txtGrsWt_WET_MAN.Text) Then
            MsgBox("Net weight should not exceed grossweight", MsgBoxStyle.Information)
            Exit Sub
        End If
    End Sub

    Private Sub cmbItem_MAN_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_MAN.SelectedValueChanged
        cmbSubitem_MAN.Text = ""
        'StrSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS S," & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = S.ITEMID AND I.ITEMNAME = '" & cmbItem_MAN.Text & "' ORDER BY SUBITEMNAME"
        StrSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'")))
        objGPack.FillCombo(StrSql, cmbSubitem_MAN, True, False)
        If cmbSubitem_MAN.Items.Count > 0 Then
            cmbSubitem_MAN.Enabled = True
        Else
            cmbSubitem_MAN.Enabled = False
        End If
        cmbSize_OWN.Text = ""
        StrSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE AS S," & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = S.ITEMID AND I.ITEMNAME = '" & cmbItem_MAN.Text & "'"
        objGPack.FillCombo(StrSql, cmbSize_OWN, , False)
        If cmbSize_OWN.Items.Count > 0 Then
            cmbSize_OWN.Enabled = True
        Else
            cmbSize_OWN.Enabled = False
        End If
        cmbItemType_MAN.Text = ""
        StrSql = " SELECT TAGTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
        If objGPack.GetSqlValue(StrSql, , "N") = "N" Then
            cmbItemType_MAN.Enabled = False
        Else
            cmbItemType_MAN.Enabled = True
        End If
    End Sub
End Class