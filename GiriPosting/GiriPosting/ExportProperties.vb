Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.OleDb

Public Class ExportProperties
    Dim WithEvents objExport As Export2Excel
    Dim printerNames As System.Drawing.Printing.PrinterSettings.StringCollection = Nothing
    Dim printDoc As New System.Drawing.Printing.PrintDocument
    Dim dtColHead As New DataTable
    Private Dgv As New DataGridView
    Private OptionalDgv As New DataGridView
    Private InkJetFont As New Font("Courier New", 10.0F, FontStyle.Regular)
    Private dsFooter As New DataSet
    Private objMailConfiguration As FRM_MAILCONFIGURATION
    Dim SettingsFileName As String
    Dim frmName As String
    Dim OledbCn As OleDb.OleDbConnection = Nothing
    Dim StrSql As String = ""
    Dim DbName As String = ""
    Dim reportTitle As String
    Dim reportCompany As String
    Dim frmuserid As Integer
    'Dim DateFt As String = "MM/dd/yyyy"
    Dim DateFt As String = "dd/MM/yyyy"
    Dim DateAsStr As Boolean = False
    Dim frmUserName As String = ""

#Region "Constructors"
    Private Sub SaveMySettings(ByVal fName As String, Optional ByVal fuserid As Integer = 999)
        Dim obj As New CLS_EXPORTPROPERTIES
        ''General
        obj.p_chkAllColumnsSelect = chkAllColumnsSelect.Checked
        obj.p_SelectedColumns = GetSelectedColumns()
        obj.p_chkCompany = chkCompany.Checked
        ''InkJet
        obj.p_chkInkCellColor = chkInkCellColor.Checked
        obj.p_chkInkCellBorder = chkInkCellBorder.Checked
        obj.p_chkInkFitToPageWidth = chkInkFitToPageWidth.Checked
        obj.p_chkInkLandscape = chkInkLandscape.Checked
        obj.p_chkInkWithCellFont = chkInkWithCellFont.Checked
        obj.p_txtInkFontDetail = txtInkFontDetail.Text
        obj.p_txtInkSplitBy = txtInkSplitBy.Text
        obj.p_lblCellBorderColor = lblCellBorderColor.BackColor.ToArgb
        obj.p_chkHeaderLeftalign = chkHeaderLeftalign.Checked
        obj.p_chkPrintDateTime = chkPrintDateTime.Checked
        obj.p_chkShrinkContent = chkstrContent.Checked
        obj.p_chkPrintHeadBold = ChkPrintBold.Checked
        obj.p_chkPrintUser = ChkPrintUser.Checked
        ''Pdf Prop
        obj.p_txtPdfMargTop = txtPdfMargTop.Text
        obj.p_txtPdfMargBottom = txtPdfMargBottom.Text
        obj.p_txtPdfMargLeft = txtPdfMargLeft.Text
        obj.p_txtPdfMargRight = txtPdfMargRight.Text
        obj.p_chkPdfCellColor = chkPdfCellColor.Checked
        obj.p_chkPdfHeaderForAllpages = chkPdfHeaderForAllpages.Checked
        obj.p_chkPdfFittoPageWid = chkPdfFittoPageWid.Checked
        obj.p_chkPdfLandscape = chkPdfLandscape.Checked
        obj.p_chkPdfBorder = chkPdfBorder.Checked
        obj.p_txtPdfSavePath = txtPdfSavePath.Text
        ''Dotmatrix Prop
        obj.p_cmbDotPrinterNames = cmbDotPrinterNames.Text
        obj.p_rbtDot80Column = rbtDot80Column.Checked
        obj.p_rbtDot132Column = rbtDot132Column.Checked
        obj.p_rbtDotAuto = rbtDotAuto.Checked
        obj.p_rbtDotMedium = rbtDotMedium.Checked
        obj.p_rbtDotCondensed = rbtDotCondensed.Checked
        obj.p_rbtDotMicro = rbtDotMicro.Checked
        obj.p_txtDotLinesPerPage = txtDotLinesPerPage.Text
        obj.p_txtDotHeaderSeperator = txtDotHeaderSeperator.Text
        obj.p_txtDotMultiCol = txtDotMultiCol.Text
        obj.p_chkDotHeadSep = chkDotHeadSep.Checked
        obj.p_chkDotHeadBold = chkDotHeadBold.Checked
        obj.p_chkDotRowSeperation = chkDotRowSeperation.Checked
        obj.p_chkDotUnderLine = chkDotUnderLine.Checked
        obj.p_chkDotAutoFit = chkDotAutoFit.Checked
        obj.p_chkWithEject = chkWithEject.Checked
        obj.p_ChkDelHead = ChkDelHead.Checked
        ''Execl Prop
        obj.p_chkXLVisibleColOnly = chkXLVisibleColOnly.Checked
        obj.p_chkXLHighlightCellFont = chkXLHighlightCellFont.Checked
        obj.p_chkXLHighlightCellColor = chkXLHighlightCellColor.Checked

        Dim objStreamWriter As New IO.StreamWriter(fName)
        Dim x As New System.Xml.Serialization.XmlSerializer(GetType(CLS_EXPORTPROPERTIES))
        x.Serialize(objStreamWriter, obj)
        objStreamWriter.Close()

        If DbName <> "" Then
            StrSql = " SELECT NAME FROM " & DbName & "..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'APPDATA'"
            If GetSqlValue(OledbCn, StrSql).ToString <> "" Then
                Dim fileStr As New IO.FileStream(fName, IO.FileMode.Open, IO.FileAccess.Read)
                Dim reader As New IO.BinaryReader(fileStr)
                Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                fileStr.Read(result, 0, result.Length)
                fileStr.Close()

                Dim Cmd As OleDb.OleDbCommand

                OledbCn.Open()
                ''Appdata Table Exists"
                StrSql = " DELETE FROM " & DbName & "..APPDATA WHERE FILENAMES = 'SET_" & frmName & ".xml'"
                StrSql += " AND USERID = " & fuserid & ""
                Cmd = New OleDb.OleDbCommand(StrSql, OledbCn)
                Cmd.ExecuteNonQuery()

                frmUserName = ""
                Dim da As New OleDbDataAdapter
                Dim dt As New DataTable
                StrSql = " SELECT USERNAME FROM " & DbName & "..USERMASTER WHERE USERID='" & frmuserid & "'"
                da = New OleDbDataAdapter(StrSql, OledbCn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    frmUserName = dt.Rows(0)("USERNAME").ToString
                End If

                Dim fiINfo As New IO.FileInfo(fName)
                StrSql = " INSERT INTO " & DbName & "..APPDATA"
                StrSql += vbCrLf + " (FILENAMES"
                StrSql += vbCrLf + " ,CONTENT,USERID"
                StrSql += vbCrLf + "  )"
                StrSql += vbCrLf + "  VALUES"
                StrSql += vbCrLf + "  (?,?,?)"
                Cmd = New OleDb.OleDbCommand(StrSql, OledbCn)
                Cmd.Parameters.AddWithValue("@FILENAMES", fiINfo.Name)
                Cmd.Parameters.AddWithValue("@CONTENT", result)
                Cmd.Parameters.AddWithValue("@USERID", fuserid)
                Cmd.ExecuteNonQuery()
                OledbCn.Close()
            End If
        End If
    End Sub

    Private Sub GetMySettings(ByVal fName As String)
        Dim obj As New CLS_EXPORTPROPERTIES
        If IO.File.Exists(fName) Then
            Dim fInfo As New IO.FileInfo(fName)
            Dim objStreamReader As New IO.StreamReader(fName)
            Dim x As New System.Xml.Serialization.XmlSerializer(GetType(CLS_EXPORTPROPERTIES))
            obj = x.Deserialize(objStreamReader)
            objStreamReader.Close()
            ''General
            chkAllColumnsSelect.Checked = obj.p_chkAllColumnsSelect
            For cnt As Integer = 0 To chklst.Items.Count - 1
                If obj.p_SelectedColumns.Contains(dtColHead.Rows(cnt).Item("NAME").ToString) Then
                    chklst.SetItemChecked(cnt, True)
                Else
                    chklst.SetItemChecked(cnt, False)
                End If
            Next
        End If
        ''InkJet Prop
        chkInkCellColor.Checked = obj.p_chkInkCellColor
        chkInkCellBorder.Checked = obj.p_chkInkCellBorder
        chkInkFitToPageWidth.Checked = obj.p_chkInkFitToPageWidth
        chkInkLandscape.Checked = obj.p_chkInkLandscape
        chkInkWithCellFont.Checked = obj.p_chkInkWithCellFont
        txtInkFontDetail.Text = obj.p_txtInkFontDetail
        txtInkSplitBy.Text = obj.p_txtInkSplitBy
        lblCellBorderColor.BackColor = Color.FromArgb(obj.p_lblCellBorderColor)
        chkHeaderLeftalign.Checked = obj.p_chkHeaderLeftalign
        chkPrintDateTime.Checked = obj.p_chkPrintDateTime
        chkstrContent.Checked = obj.p_chkShrinkContent
        ChkPrintBold.Checked = obj.p_chkPrintHeadBold
        ChkPrintUser.Checked = obj.p_chkPrintUser
        ''Pdf Prop
        txtPdfMargTop.Text = obj.p_txtPdfMargTop
        txtPdfMargBottom.Text = obj.p_txtPdfMargBottom
        txtPdfMargLeft.Text = obj.p_txtPdfMargLeft
        txtPdfMargRight.Text = obj.p_txtPdfMargRight
        chkPdfCellColor.Checked = obj.p_chkPdfCellColor
        chkPdfHeaderForAllpages.Checked = obj.p_chkPdfHeaderForAllpages
        chkPdfFittoPageWid.Checked = obj.p_chkPdfFittoPageWid
        chkPdfLandscape.Checked = obj.p_chkPdfLandscape
        chkPdfBorder.Checked = obj.p_chkPdfBorder
        txtPdfSavePath.Text = obj.p_txtPdfSavePath
        ''Dotmatrix Prop
        If cmbDotPrinterNames.Items.Contains(obj.p_cmbDotPrinterNames) = False And obj.p_cmbDotPrinterNames <> "" Then
            cmbDotPrinterNames.Items.Add(obj.p_cmbDotPrinterNames)
        End If
        cmbDotPrinterNames.Text = obj.p_cmbDotPrinterNames
        rbtDot80Column.Checked = obj.p_rbtDot80Column
        rbtDot132Column.Checked = obj.p_rbtDot132Column
        rbtDotAuto.Checked = obj.p_rbtDotAuto
        rbtDotMedium.Checked = obj.p_rbtDotMedium
        rbtDotCondensed.Checked = obj.p_rbtDotCondensed
        rbtDotMicro.Checked = obj.p_rbtDotMicro
        txtDotLinesPerPage.Text = obj.p_txtDotLinesPerPage
        txtDotHeaderSeperator.Text = obj.p_txtDotHeaderSeperator
        txtDotMultiCol.Text = obj.p_txtDotMultiCol
        chkDotHeadSep.Checked = obj.p_chkDotHeadSep
        chkDotHeadBold.Checked = obj.p_chkDotHeadBold
        chkDotRowSeperation.Checked = obj.p_chkDotRowSeperation
        chkDotUnderLine.Checked = obj.p_chkDotUnderLine
        chkDotAutoFit.Checked = obj.p_chkDotAutoFit
        chkCompany.Checked = obj.p_chkCompany
        chkWithEject.Checked = obj.p_chkWithEject
        ChkDelHead.Checked = obj.p_ChkDelHead
        ''Execl Prop
        chkXLVisibleColOnly.Checked = obj.p_chkXLVisibleColOnly
        chkXLHighlightCellFont.Checked = obj.p_chkXLHighlightCellFont
        chkXLHighlightCellColor.Checked = obj.p_chkXLHighlightCellColor
    End Sub

    Private Function GetSqlValue(ByVal Cn As OleDb.OleDbConnection, ByVal Qry As String) As Object
        Dim Obj As Object = Nothing
        Dim Da As OleDb.OleDbDataAdapter
        Dim DtTemp As New DataTable
        Da = New OleDb.OleDbDataAdapter(Qry, Cn)
        Da.Fill(DtTemp)
        If DtTemp.Rows.Count > 0 Then
            Obj = DtTemp.Rows(0).Item(0)
        End If
        Return Obj
    End Function

    Private Sub Initializer(ByVal frmName As String, Optional ByVal fUserid As Integer = 999)
        SettingsFileName = Application.StartupPath & "\AppData\SET_" & frmName & ".xml"
        If IO.File.Exists(Application.StartupPath & "\CONINFO.INI") Then
            Dim fil As New IO.FileStream(Application.StartupPath & "\CONINFO.INI", IO.FileMode.Open, IO.FileAccess.Read)
            Dim fs As New IO.StreamReader(fil, System.Text.Encoding.Default)
            'Dim fs As New IO.StreamReader(fil)
            Dim pCompanyId As String = Nothing
            Dim pServerName As String = Nothing
            Dim pDbPwd As String = Nothing
            Dim pDbLoginType As String = Nothing
            fs.BaseStream.Seek(0, IO.SeekOrigin.Begin)
            pCompanyId = Mid(fs.ReadLine, 21) '1
            fs.ReadLine() '2
            pServerName = Mid(fs.ReadLine, 21) '3
            fs.ReadLine() '4
            pDbPwd = Mid(fs.ReadLine, 21) '5
            pDbLoginType = Mid(fs.ReadLine, 21) '6

            If pDbPwd <> "" Then pDbPwd = Decrypt(pDbPwd)
            'If UCase(pDbLoginType) = "S" Then
            '    OledbCn = New OleDb.OleDbConnection("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=;Data Source=" & pServerName & ";user id=sa;password=" & pDbPwd & ";")
            'ElseIf UCase(pDbLoginType) = "I" Then
            '    OledbCn = New OleDb.OleDbConnection("Provider=SQLOLEDB.1;Persist Security Info=False;user id = sa;pwd=" & pDbPwd & ";Initial Catalog=;Data Source=" & pServerName & "")
            'Else
            '    OledbCn = New OleDb.OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=;Data Source=" & pServerName & "")
            'End If
            If UCase(pDbLoginType) = "W" Then
                OledbCn = New OleDb.OleDbConnection("PROVIDER=SQLOLEDB.1;INTEGRATED SECURITY=SSPI;PERSIST SECURITY INFO=FALSE;INITIAL CATALOG=;DATA SOURCE=" & pServerName & "")
            Else
                OledbCn = New OleDb.OleDbConnection("PROVIDER = SQLOLEDB.1;PERSIST SECURITY INFO=FALSE; INITIAL CATALOG=;DATA SOURCE=" & pServerName & ";USER ID=" & IIf(UCase(pDbLoginType) = "I" Or UCase(pDbLoginType) = "S", "SA", UCase(pDbLoginType)) & ";PASSWORD=" & pDbPwd & ";")
            End If

            StrSql = " SELECT NAME FROM MASTER..SYSDATABASES WHERE NAME = '" & pCompanyId & "ADMINDB'"
            DbName = GetSqlValue(OledbCn, StrSql).ToString
            If DbName <> "" Then
                ''Db Exists
                StrSql = " SELECT NAME FROM " & pCompanyId & "ADMINDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'APPDATA'"
                If GetSqlValue(OledbCn, StrSql).ToString <> "" Then
                    ''Appdata Table Exists"
                    StrSql = " SELECT CONTENT FROM " & DbName & "..APPDATA WHERE FILENAMES = 'SET_" & frmName & ".xml'"
                    StrSql += " AND USERID = " & fUserid & ""
                    Dim filContent As Object = GetSqlValue(OledbCn, StrSql)
                    If filContent IsNot Nothing Then
                        SettingsFileName = IO.Path.GetTempPath & "\SET_" & frmName & ".xml"
                        Dim myByte() As Byte = filContent
                        Dim fileStr As New IO.FileStream(SettingsFileName, IO.FileMode.Create, IO.FileAccess.ReadWrite)
                        fileStr.Write(myByte, 0, myByte.Length)
                        fileStr.Close()
                    End If
                Else
                    DbName = ""
                End If
            End If
        End If

        StrSql = " SELECT ISNULL(CTLTEXT,'')CTLTEXT FROM " & DbName & "..SOFTCONTROL WHERE CTLID='MAILEXPORT_CC_EDIT'"
        If GetSqlValue(OledbCn, StrSql).ToString = "N" And Val(fUserid) <> 999 Then
            txtEmailCc.Enabled = False
        Else
            txtEmailCc.Enabled = True
        End If

        If IO.File.Exists(SettingsFileName) = False Then
            GetMySettings("")
        Else
            GetMySettings(SettingsFileName)
        End If
    End Sub
    Public Function Decrypt(Str_Pwd As String) As String
        Dim vv As String = ""
        Dim x As Integer = 0
        For k As Integer = 0 To Str_Pwd.Length - 1
            Dim vIn As Char = Str_Pwd.Substring(x, 1)
            Dim vOut As String = vIn.ToString(vIn)
            Dim Byte_Data As Byte   ' For Reading Data
            Byte_Data = Asc(vOut)
            Byte_Data = Byte_Data - 23
            Byte_Data = Not Byte_Data
            vv += Chr(Byte_Data)
            x += 1
        Next
        Return vv
    End Function

    Public Sub New(ByVal frmName As String, ByVal CompanyName As String _
    , ByVal ExportTo As List(Of String), ByVal Title As String, ByVal Dgv As DataGridView _
    , Optional ByVal OptionalDgvHeader As DataGridView = Nothing _
    , Optional ByVal dsFooter As DataSet = Nothing, Optional ByVal frmuserid As Integer = 999 _
    , Optional ByVal ToEmailId As String = "", Optional ByVal DateForm As String = "dd/MM/yyyy" _
    , Optional ByVal DateAsStr As Boolean = False)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        frmName = frmName.ToUpper
        Me.frmName = frmName
        Me.frmuserid = frmuserid
        Me.DateFt = DateForm
        Me.DateAsStr = DateAsStr
        ' Add any initialization after the InitializeComponent() call.
        ''InkJet Initialization
        Me.dsFooter = dsFooter
        chkInkFitToPageWidth.Checked = True
        txtInkFontDetail.Text = InkJetFont.ToString
        chkInkWithCellFont.Checked = True
        chkInkWithCellFont_CheckedChanged(Me, New EventArgs)

        ''DotMatrix Initialization
        For Each name As String In System.Drawing.Printing.PrinterSettings.InstalledPrinters
            cmbDotPrinterNames.Items.Add(name)
        Next
        If cmbDotPrinterNames.Items.Count > 0 Then
            cmbDotPrinterNames.Text = printDoc.PrinterSettings.PrinterName
        End If
        Email_DefaultSetting()
        If ToEmailId <> "" Then txtEmailTo.Text = ToEmailId

        ''Main Initialization
        tabMain.TabPages.Remove(tabDotmatrix)
        tabMain.TabPages.Remove(tabInkJet)
        tabMain.TabPages.Remove(tabPdf)
        tabMain.TabPages.Remove(tabExcel)
        tabMain.TabPages.Remove(tabEMail)
        tabGeneral.Controls.Add(pnlDotMatrix)
        tabGeneral.Controls.Add(pnlInkJet)
        tabGeneral.Controls.Add(pnlPdf)
        tabGeneral.Controls.Add(pnlExcel)
        tabGeneral.Controls.Add(pnlEmail)
        pnlDotMatrix.Visible = False
        pnlInkJet.Visible = False
        pnlPdf.Visible = False
        pnlExcel.Visible = False
        pnlEmail.Visible = False
        pnlDotMatrix.Size = New Size(10, 199)
        pnlInkJet.Size = pnlDotMatrix.Size
        pnlPdf.Size = pnlDotMatrix.Size
        pnlExcel.Size = pnlDotMatrix.Size
        pnlEmail.Size = pnlDotMatrix.Size
        pnlDotMatrix.Dock = DockStyle.Bottom
        pnlInkJet.Dock = DockStyle.Bottom
        pnlPdf.Dock = DockStyle.Bottom
        pnlExcel.Dock = DockStyle.Bottom
        pnlEmail.Dock = DockStyle.Bottom
        txtEmailSub.Text = Title.Replace(ChrW(10), "").Replace(Environment.NewLine, "").Replace(vbCrLf, "")

        Me.Dgv = Dgv
        Me.OptionalDgv = OptionalDgvHeader
        'txtTitle.Text = Title
        reportTitle = Title
        reportCompany = CompanyName
        If chkCompany.Checked = True Then txtTitle.Text = CompanyName + vbCrLf Else txtTitle.Text = vbCrLf
        txtTitle.Text += Title

        For Each str As String In ExportTo
            cmbExportTo.Items.Add(str)
        Next
        cmbExportTo.SelectedIndex = 0

        dtColHead.Columns.Add("CAPTION", GetType(String))
        dtColHead.Columns.Add("NAME", GetType(String))
        For cnt As Integer = 0 To Dgv.ColumnCount - 1
            If Not Dgv.Columns(cnt).Visible Then Continue For
            Dim ro As DataRow = Nothing
            ro = dtColHead.NewRow
            ro!NAME = Dgv.Columns(cnt).Name
            ro!CAPTION = Dgv.Columns(cnt).HeaderText
            dtColHead.Rows.Add(ro)
        Next
        dtColHead.AcceptChanges()

        For Each ro As DataRow In dtColHead.Rows
            chklst.Items.Add(ro("CAPTION").ToString, True)
        Next

        Initializer(frmName, frmuserid)
    End Sub
#End Region

    Public Function GetSelectedColumns() As List(Of String)
        Dim lst As New List(Of String)
        For Each item As Object In chklst.CheckedIndices
            lst.Add(dtColHead.Rows(Val(item.ToString)).Item("NAME").ToString)
        Next
        Return lst
    End Function

    Public ReadOnly Property Title() As String
        Get
            Return txtTitle.Text
        End Get
    End Property

    Private Sub ExportProperties_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub ExportProperties_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub ExportProperties_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbExportTo.Select()
    End Sub

    Private Sub cmbExportTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbExportTo.SelectedIndexChanged
        pic.Image = Nothing
        pnlDotMatrix.Visible = False
        pnlInkJet.Visible = False
        pnlPdf.Visible = False
        pnlExcel.Visible = False
        pnlEmail.Visible = False
        Select Case cmbExportTo.Text.ToUpper
            Case "INKJET PRINT"
                pic.Image = My.Resources.Printer_256x256
                pnlInkJet.Visible = True
            Case "DOTMATRIX PRINT"
                pic.Image = My.Resources.dot_matrix_printer
                pnlDotMatrix.Visible = True
            Case "ADOBE PDF"
                pic.Image = My.Resources.pdf_logo
                pnlPdf.Visible = True
            Case "OPEN OFFICE SPREAD SHEET"
                pic.Image = My.Resources.OpenOfficeLogo
                pnlExcel.Visible = True
            Case "MS OFFICE EXCEL"
                pic.Image = My.Resources.microsoft_office_excel_2007_logo
                pnlExcel.Visible = True
            Case "CHART"
                pic.Image = My.Resources.Color_MS_Excel_256
            Case "EMAIL"
                pic.Image = My.Resources.Send_256
                pnlEmail.Visible = True
        End Select
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If chkSaveSettings.Checked Then SaveMySettings(SettingsFileName, frmuserid)
        Select Case cmbExportTo.Text.ToUpper
            Case "INKJET PRINT"
                Me.Hide()
                Export_InkJet()
            Case "DOTMATRIX PRINT"
                Me.Hide()
                Export_Dotmatrix()
            Case "ADOBE PDF"
                Export_Pdf()
                Me.Hide()
            Case "OPEN OFFICE SPREAD SHEET"
                'If IO.File.Exists("C:\Program Files\OpenOffice.org 3\program\scalc.exe") = False Then
                '    If IO.File.Exists("C:\Program Files (x86)\OpenOffice.org 3\program\scalc.exe") = False Then
                '        MsgBox("Please Install OpenOffice SpreadSheet Calculator", MsgBoxStyle.Information)
                '        Exit Sub
                '    End If
                'End If
                Export_Excel(False)
            Case "MS OFFICE EXCEL"
                Dim objKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot
                Dim objSubKey As Microsoft.Win32.RegistryKey = objKey.OpenSubKey("Excel.Application")
                If objSubKey Is Nothing Then
                    MsgBox("Please Install Miscrosoft Office Excel", MsgBoxStyle.Information)
                    Exit Sub
                End If
                Export_Excel(True, DateFt, DateAsStr)
            Case "CHART"
                Dim dtChart As New DataTable
                dtChart = CType(Dgv.DataSource, DataTable).Copy
                For cnt As Integer = 0 To Dgv.ColumnCount - 1
                    dtChart.Columns(Dgv.Columns(cnt).Name).SetOrdinal(cnt)
                Next
                Dim lst As New List(Of String)
                lst = GetSelectedColumns()
ColDel:
                For cnt As Integer = 0 To dtChart.Columns.Count - 1
                    If dtChart.Columns(cnt).ColumnName.ToUpper = "COLHEAD" Then Continue For
                    If lst.Contains(dtChart.Columns(cnt).ColumnName) = False Then
                        dtChart.Columns.Remove(dtChart.Columns(cnt))
                        GoTo ColDel
                    End If
                Next
                dtChart.AcceptChanges()
                Dim frmChar As New GChart.ChartView(dtChart)
                frmChar.Show()
                Hide()
            Case "EMAIL"
                If Email_ValidateId() = False Then
                    txtEmailTo.Focus()
                    Exit Sub
                End If
                Dim thEmail As New Threading.Thread(AddressOf Export_Email)
                thEmail.Start()
        End Select
        Close()
    End Sub

    Private Function Email_ValidateId() As Boolean
        Dim pattern As String = "^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|" & "0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" & "[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$"
        Dim match As System.Text.RegularExpressions.Match = Regex.Match(txtEmailTo.Text.Trim(), pattern, RegexOptions.IgnoreCase)
        If match.Success Then
            Return True
        Else
            MsgBox("Invalid mail id", MsgBoxStyle.Information)
            Return False
        End If
    End Function

    Private Sub Email_DefaultSetting()
        objMailConfiguration = New FRM_MAILCONFIGURATION
        txtEmailFromId.Text = objMailConfiguration.txtFromEmailId.Text
        txtEmailTo.Text = objMailConfiguration.txtToMailIds.Text
        txtEmailBody.Text = objMailConfiguration.txtBody.Text
    End Sub

    Private Sub Export_Email()
        Dim objEMail As New Email
        Dim AttachmentPath As String = Nothing
        If objMailConfiguration.chkEmailAttachment.Checked Then
            Dim Ra As New Random
GetFile:
            AttachmentPath = System.IO.Path.GetTempPath & "Att_" & Ra.Next(189899, 999999).ToString & ".pdf"
            If IO.File.Exists(AttachmentPath) Then
                GoTo GetFile
            End If
            Dim objPdf As New Pdf
            objPdf.Export(Dgv, Title _
            , GetSelectedColumns _
            , OptionalDgv _
            , Title _
            , 0 _
            , 0 _
            , 25 _
            , 25 _
            , AttachmentPath _
            , objMailConfiguration.chkEmailFittoPageWid.Checked _
            , objMailConfiguration.chkEmailCellColor.Checked _
            , objMailConfiguration.chkEMailLandscape.Checked _
            , objMailConfiguration.chkEmailHeaderForAllpages.Checked _
            , objMailConfiguration.chkEmailBorder.Checked _
            , False)
        End If
        objEMail.pFromMailId = objMailConfiguration.txtFromEmailId.Text
        objEMail.pFromMailPwd = objMailConfiguration.txtFromMailPwd.Text
        objEMail.pToMailId = txtEmailTo.Text
        objEMail.pCcMailId = txtEmailCc.Text
        objEMail.pSubject = txtEmailSub.Text.Replace(ChrW(10), "").Replace(Environment.NewLine, "").Replace(vbCrLf, "")
        objEMail.pBody = txtEmailBody.Text
        objEMail.pAttachFilePath = AttachmentPath
        objEMail.pIsBodyHtml = True
        If objEMail.Send(Val(objMailConfiguration.txtPortNo.Text), objMailConfiguration.txtHostName.Text.Trim, IIf(objMailConfiguration.chkEmailSSL.Checked, True, False)) = False Then

        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub chkAllColumnsSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAllColumnsSelect.CheckedChanged
        For cnt As Integer = 0 To chklst.Items.Count - 1
            If chkAllColumnsSelect.Checked = True Then
                chklst.SetItemChecked(cnt, True)
            Else
                chklst.SetItemChecked(cnt, False)
            End If
        Next
    End Sub


#Region "Pdf Property,Members,Events"
#Region "Property"
    Public ReadOnly Property Pdf_WithBorder() As Boolean
        Get
            Return chkPdfBorder.Checked
        End Get
    End Property

    Public ReadOnly Property Pdf_PageLeft() As Single
        Get
            Return Val(txtPdfMargLeft.Text)
        End Get
    End Property

    Public ReadOnly Property Pdf_PageRight() As Single
        Get
            Return Val(txtPdfMargRight.Text)
        End Get
    End Property

    Public ReadOnly Property Pdf_PageBottom() As Single
        Get
            Return Val(txtPdfMargBottom.Text)
        End Get
    End Property

    Public ReadOnly Property Pdf_PageTop() As Single
        Get
            Return Val(txtPdfMargTop.Text)
        End Get
    End Property

    Public ReadOnly Property Pdf_CellColor() As Boolean
        Get
            Return chkPdfCellColor.Checked
        End Get
    End Property

    Public ReadOnly Property Pdf_Landscape() As Boolean
        Get
            Return chkPdfLandscape.Checked
        End Get
    End Property



    Public ReadOnly Property Pdf_SavePath() As String
        Get
            Return txtPdfSavePath.Text
        End Get
    End Property

    Public ReadOnly Property Pdf_FitToPageWidth() As Boolean
        Get
            Return chkPdfFittoPageWid.Checked
        End Get
    End Property

    Public ReadOnly Property Pdf_HeaderForAllPages() As Boolean
        Get
            Return chkPdfHeaderForAllpages.Checked
        End Get
    End Property
#End Region

    Private Sub Export_Pdf()
        Dim objPdf As New Pdf
        objPdf.Export(Dgv, Title _
        , GetSelectedColumns _
        , OptionalDgv _
        , Title _
        , Pdf_PageLeft _
        , Pdf_PageRight _
        , Pdf_PageBottom _
        , Pdf_PageTop _
        , Pdf_SavePath _
        , Pdf_FitToPageWidth _
        , Pdf_CellColor _
        , Pdf_Landscape _
        , Pdf_HeaderForAllPages _
        , Pdf_WithBorder _
        , True)
    End Sub

#Region "Events"
    Private Sub btnPdfBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPdfBrowse.Click
        Dim svfDia As New SaveFileDialog
        svfDia.DefaultExt = ".pdf"
        svfDia.Filter = "Adobe PDF files(*.pdf)|*.pdf"
        If svfDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtPdfSavePath.Text = svfDia.FileName
        End If
    End Sub
#End Region
#End Region

#Region "InkJetPrint Property,Members,Events"
#Region "Property"
    Public ReadOnly Property Ink_NoOfLables() As Integer
        Get
            Return Val(txtInkSplitBy.Text)
        End Get
    End Property
    Public ReadOnly Property Ink_FitToPageWidth() As Boolean
        Get
            Return chkInkFitToPageWidth.Checked
        End Get
    End Property

    Public ReadOnly Property Ink_GetFont() As Font
        Get
            Return New Font("Courier New", 10.0F, FontStyle.Regular)
        End Get
    End Property

    Public ReadOnly Property Ink_GetCellColor() As Boolean
        Get
            Return chkInkCellColor.Checked
        End Get
    End Property

    Public ReadOnly Property Ink_GetCellFont() As Boolean
        Get
            Return chkInkWithCellFont.Checked
        End Get
    End Property

    Public ReadOnly Property Ink_Landscape() As Boolean
        Get
            Return chkInkLandscape.Checked
        End Get
    End Property
    Public ReadOnly Property Ink_ShrinkContent() As Boolean
        Get
            Return chkstrContent.Checked
        End Get
    End Property
    Public ReadOnly Property Ink_PrintHeadBold() As Boolean
        Get
            Return ChkPrintBold.Checked
        End Get
    End Property

    Public ReadOnly Property Ink_PrintUser() As Boolean
        Get
            Return ChkPrintUser.Checked
        End Get
    End Property

#End Region

#Region "Members"
    Public Sub Export_InkJet()
        Dim objInkJet
        'If frmName = "FRMACCOUNTSLEDGER" Then
        '    objInkJet = New PRINT_DGV_MULTICOLUMN_LEDGER
        'Else
        objInkJet = New PRINT_DGV_MULTICOLUMN
        'End If
        objInkJet.Print_DataGridView(Title _
        , Dgv _
        , Ink_FitToPageWidth _
        , GetSelectedColumns _
        , Ink_GetFont _
        , Ink_GetCellColor _
        , Ink_GetCellFont _
        , Ink_Landscape _
        , Val(txtInkSplitBy.Text) _
        , chkInkCellBorder.Checked _
        , lblCellBorderColor.BackColor _
        , chkPrintDateTime.Checked _
        , OptionalDgv, chkHeaderLeftalign.Checked _
        , Ink_ShrinkContent _
        , Ink_PrintHeadBold _
        , IIf(Ink_PrintUser, frmUserName.ToString, ""))
    End Sub
#End Region

#Region "Events"
    Private Sub btnInkFont_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInkFont.Click
        Dim fontDia As New FontDialog
        If fontDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            InkJetFont = fontDia.Font
            txtInkFontDetail.Text = InkJetFont.ToString
        End If
    End Sub

    Private Sub chkInkWithCellFont_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkInkWithCellFont.CheckedChanged
        If chkInkWithCellFont.Checked = True Then
            btnInkFont.Enabled = False
            txtInkFontDetail.Clear()
        Else
            btnInkFont.Enabled = True
            txtInkFontDetail.Text = InkJetFont.ToString
        End If
    End Sub

    Private Sub txtInkFontDetail_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInkFontDetail.GotFocus
        SendKeys.Send("{TAB}")
    End Sub
#End Region
#End Region

#Region "DotMatrix Property,Members,Events"
#Region "Events"
    Private Sub txtDotHeaderSeperator_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDotHeaderSeperator.TextChanged
        If txtDotHeaderSeperator.Text = "" Then chkDotHeadSep.Checked = False
    End Sub
#End Region

#Region "Members"
    Private Sub Export_Dotmatrix()
        Dim modeOfPrint As DotMatrix.PrintMode = DotMatrix.PrintMode.Auto
        If rbtDotAuto.Checked Then
            modeOfPrint = DotMatrix.PrintMode.Auto
        ElseIf rbtDotMedium.Checked Then
            modeOfPrint = DotMatrix.PrintMode.Medium
        ElseIf rbtDotCondensed.Checked Then
            modeOfPrint = DotMatrix.PrintMode.Condensed
        ElseIf rbtDotMicro.Checked Then
            modeOfPrint = DotMatrix.PrintMode.Micro
        ElseIf rbtDotNone.Checked Then
            modeOfPrint = DotMatrix.PrintMode.None
        End If

        Dim objDotMatrix As New DotMatrix
        objDotMatrix.Print_GridView(Title _
        , Dgv _
        , GetSelectedColumns _
        , IIf(rbtDot80Column.Checked, DotMatrix.PrintType.Column_80, IIf(rbtDot132Column.Checked, DotMatrix.PrintType.Column_130, DotMatrix.PrintType.column_40)) _
        , cmbDotPrinterNames.Text _
        , txtDotHeaderSeperator.Text _
        , chkDotHeadSep.Checked _
        , Val(txtDotMultiCol.Text) _
        , chkDotAutoFit.Checked _
        , chkDotUnderLine.Checked _
        , chkDotRowSeperation.Checked _
        , chkDotHeadBold.Checked _
        , modeOfPrint _
        , OptionalDgv _
        , IIf(Val(txtDotLinesPerPage.Text) = 0, 60, Val(txtDotLinesPerPage.Text)) _
        , dsFooter _
        , chkZeroSupress.Checked, chkWithEject.Checked, ChkDelHead.Checked)
    End Sub
#End Region
#End Region

#Region "Excel Members,Events"
    Private Sub Export_Excel(ByVal excel As Boolean, Optional ByVal DateFt As String = "dd/MM/yyyy", Optional ByVal DateAsStr As Boolean = False)
        btnOk.Enabled = False
        objExport = New Export2Excel(Dgv, excel, GetSelectedColumns, OptionalDgv, txtExcelSavePath.Text)
        objExport.HighlightFont = chkXLHighlightCellFont.Checked
        objExport.HighlightColor = chkXLHighlightCellColor.Checked
        objExport.VisibleColumnsOnly = chkXLVisibleColOnly.Checked
        objExport.ExpressWayExcel = ChkExpressExcelMode.Checked
        objExport.QuickExcelSave = ChkQuickExcelSave.Checked
        objExport.Title = txtTitle.Text
        AddHandler objExport._Prg, AddressOf export2XLS_prg
        Me.Refresh()
        If objExport.ExpressWayExcel = True Then
            objExport.ExpressfPost()
        ElseIf objExport.QuickExcelSave = True Then
            objExport.QuickExcelSavePost()
        ElseIf excel Then
            objExport.fPost(DateFt, DateAsStr)
        Else
            objExport.fPost()
        End If
    End Sub
    Private Sub export2XLS_prg(ByVal sender As Object, ByVal e As ProgressEventArgs)
        pBar.Value = e.ProgressValue
    End Sub
#End Region

    Private Sub txtTitle_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTitle.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub chkDotRowSeperation_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDotRowSeperation.CheckedChanged
        If chkDotRowSeperation.Checked Then
            chkDotUnderLine.Checked = False
        End If
    End Sub

    Private Sub chkDotUnderLine_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDotUnderLine.CheckedChanged
        If chkDotUnderLine.Checked Then
            chkDotRowSeperation.Checked = False
        End If
    End Sub

    Private Sub btnEmailConfiguration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmailConfiguration.Click
        If objMailConfiguration.ShowDialog = Windows.Forms.DialogResult.OK Then
            Email_DefaultSetting()
        End If
    End Sub

    Private Sub txtEmail_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEmailTo.KeyPress, txtEmailSub.KeyPress, txtEmailBody.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnEmailConfiguration_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmailConfiguration.GotFocus
        'SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtEmailFromId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEmailFromId.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub lblCellBorderColor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblCellBorderColor.Click
        Dim cd As New ColorDialog
        If cd.ShowDialog = Windows.Forms.DialogResult.OK Then
            lblCellBorderColor.BackColor = cd.Color
        End If
    End Sub

    Private Sub rbtDotMedium_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtDotMedium.CheckedChanged

    End Sub

    Private Sub rbtDotMicro_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtDotMicro.CheckedChanged

    End Sub

    Private Sub chkCompany_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCompany.CheckedChanged
        If chkCompany.Checked = True Then txtTitle.Text = reportCompany & vbCrLf & reportTitle Else txtTitle.Text = vbCrLf & reportTitle
    End Sub

    Private Sub ChkExpressExcelMode_CheckedChanged(sender As Object, e As EventArgs) Handles ChkExpressExcelMode.CheckedChanged
        If ChkExpressExcelMode.Checked = True Then
            PanelExpressExcel.Visible = True
        Else
            PanelExpressExcel.Visible = False
        End If
    End Sub

    Private Sub BtnExcelBrowse_Click(sender As Object, e As EventArgs) Handles BtnExcelBrowse.Click
        Dim ExcelsvfDia As New SaveFileDialog
        ExcelsvfDia.DefaultExt = ".xlsx"
        ExcelsvfDia.Filter = "Microsoft Excel files(*.xlsx)|*.xlsx"
        If ExcelsvfDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtExcelSavePath.Text = ExcelsvfDia.FileName
        End If
    End Sub

    Private Sub ChkSaveExcelMode_CheckedChanged(sender As Object, e As EventArgs) Handles ChkQuickExcelSave.CheckedChanged
        If ChkQuickExcelSave.Checked = True Then
            PanelExpressExcel.Visible = True
        Else
            PanelExpressExcel.Visible = False
        End If
    End Sub
End Class