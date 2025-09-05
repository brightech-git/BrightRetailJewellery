Imports System.IO

Public Class DotMatrix
    'Print Settings for Reports:
    '--------------------------
    '1.Page length setting->chr(27)+chr(67)+chr(72)  -for 72 lines / page                      
    '2.Page Ejection      ->chr(12)
    '3.Scrolling UpDown   ->chr(27)+j+chr(180)  
    '                                 30 - one line 
    '                                180 - one inch
    '4.Bold Heading       ->chr(14)   (applicable for the entire line)
    '                     ->chr(18)+chr(20) (return to original mode in the same line) 
    '5.Double Strike      ->chr(27)+G+ ---- + Chr(27)+H ,
    '                       chr(27)+E+ ---- + Chr(27)+F
    '                                          (for 80 col papers)
    '6.Condensed mode    -> (i) chr(15)            - 154 chars  (132 col.printer)
    '                                             - 136 chars  (80 col.printer) 
    '                      (ii) chr(27)+M         - 110 chars  (132 col.printer)
    '                                              -  95 chars  (80 col.printer)  
    '                     (iii) chr(27)+M+chr(15) - 185 chars  (132 col.printer)                         
    '                                            - 160 chars  (80 col.printer)
    '7.Underline         -> chr(27)+"-1"+-------+chr(27)+"-0"
    Public Enum PrintType
        Column_80 = 0
        Column_130 = 1
        column_40 = 2
    End Enum

    Public Enum PrintMode
        Auto = 0
        Medium = 1
        Condensed = 2
        Micro = 3
        None = 4
    End Enum

    Private pBoldDoubleStart As String = Chr(27) + "E"
    Private pBoldDoubleEnd As String = Chr(27) + "F"
    Private pBoldStart As String = Chr(27) + "G"
    Private pBoldEnd As String = Chr(27) + "H"
    Private pEjectPaper As String = Chr(12)
    Private pCondens As String
    Private pUnderLineOn As String = Chr(27) & "-1"
    Private pUnderLineOff As String = Chr(27) & "-0"
    Private oLenOfString As Integer = 15  ''It May change depends the column maxlength
    Private oSepChar As String = "|"
    Private oHeaderBold As Boolean
    Private oSepForWholeColumn As Boolean
    Private oLinesPerPage As Integer = 60
    Private oWrite As System.IO.StreamWriter
    Private oFilePath As String = Nothing
    Private oReportTitle As String = Nothing
    Private oCharPerLine As Integer = 185
    Private oMaxCharPerLine As Integer = Nothing
    Private oReportHeaderStr As String = Nothing
    Private oReportMergeHeaderStr As String = Nothing
    Private oPrinterType As PrintType
    Private oPageNoCurrent As Integer
    Private oRowsTotal As Integer
    Private WithEvents txtLineCounter As New TextBox
    Private oPrinterName As String
    Private oFileName As String
    Private oMultiCol As Integer
    Private oGridAvailableColumns As New List(Of String)
    Private oModeOfPrint As PrintMode
    Private oUnderLine As Boolean
    Private oRowSeperation As Boolean
    Private oZeroSupress As Boolean
    Private RowPos As Integer = 0
    Private GridRowCount As Integer = 0
    Private oWithEject As Boolean
    Private oRemoveHead As Boolean

    Private Sub PrintLine(ByVal len As Integer, Optional ByVal character As Char = "-")
        Dim line As String = Nothing
        For cnt As Integer = 1 To len
            line += character
        Next
        WriteLine(line)
    End Sub

    Private Sub WriteLine(ByVal str As String)
        '        str = Mid(str, 1, oCharPerLine)
        ''Find Length
        Dim lenStr As String = str
        lenStr = lenStr.Replace(pBoldDoubleStart, "")
        lenStr = lenStr.Replace(pBoldDoubleEnd, "")
        lenStr = lenStr.Replace(pBoldStart, "")
        lenStr = lenStr.Replace(pBoldEnd, "")
        lenStr = lenStr.Replace(pEjectPaper, "")
        lenStr = lenStr.Replace(pUnderLineOn, "")
        lenStr = lenStr.Replace(pUnderLineOff, "")
        If oMaxCharPerLine < lenStr.Length Then oMaxCharPerLine = lenStr.Length
        oWrite.WriteLine(str) '+ " " + txtLineCounter.Text
        txtLineCounter.Text = Val(txtLineCounter.Text) + 1
    End Sub

    Private Sub GridView_Header_ReportMerge()
        If RowPos >= GridRowCount Then Exit Sub
        Dim headStr As String = Nothing
        PrintLine(oReportMergeHeaderStr.Length)
        WriteLine(oReportMergeHeaderStr)
    End Sub

    Private Sub GridView_Header_Report()
        If RowPos >= GridRowCount Then Exit Sub
        If oHeaderBold Then oWrite.Write(pBoldDoubleStart)
        If oReportMergeHeaderStr <> Nothing Then GridView_Header_ReportMerge() Else PrintLine(oReportHeaderStr.Length)
        WriteLine(oReportHeaderStr)
        PrintLine(oReportHeaderStr.Length)
        If oHeaderBold Then oWrite.Write(pBoldDoubleEnd)
    End Sub

    Private Sub GridView_Header_Page()
        If RowPos >= GridRowCount Then Exit Sub
        Dim s As String = Nothing
        oWrite.Write(pBoldStart)
        'oWrite.Write(Chr(18) + Chr(27) + "E" + Chr(14))
        For Each str As String In oReportTitle.Split(Environment.NewLine)
            oWrite.Write(Chr(15) + Chr(14))
            'oWrite.Write(Chr(14))
            s = Trim(str.Replace(ChrW(10), ""))
            oWrite.WriteLine(s)
            txtLineCounter.Text = Val(txtLineCounter.Text) + 1
        Next
        'oWrite.Write(Chr(27) + "F")
        oWrite.Write(pBoldEnd)
        oWrite.Write("####$$$$printmode")
    End Sub

    Private Function GetColumnName(ByVal assName As String) As String
        Dim RetStr As String = Nothing
        If oMultiCol > 1 Then
            RetStr = assName
        Else
            Dim t As String() = assName.Split("~")
            RetStr = t(0)
        End If
        Return RetStr
    End Function

    Private Sub Print_GridViewMergeHeader(ByVal dgvMerge As DataGridView, ByVal dgv As DataGridView)
        Dim dgvHeader As New DataGridView
        Dim oColumnWidth As New List(Of Integer)
        Dim dtMerge As New DataTable
        Dim dt As New DataTable
        dtMerge = CType(dgvMerge.DataSource, DataTable)
        dt = CType(dgv.DataSource, DataTable)

        Dim i As Integer = 0
        oReportMergeHeaderStr = Nothing
        For spt As Integer = 0 To oMultiCol - 1
            For Each dgvMergCol As DataGridViewColumn In dgvMerge.Columns
                Dim leng As Integer = 0
                Dim flag As Boolean = False
                If dgvMergCol.Name.Contains("~") Then
                    i = 0
                    For Each colName As String In dgvMergCol.Name.Split("~")
                        If Not oGridAvailableColumns.Contains(colName) Then Continue For
                        flag = True
                        leng += Math.Ceiling(dgv.Columns(GetColumnName(colName & "~" & spt.ToString)).Width / 8)
                        i += 1
                    Next
                    leng += i - 1
                ElseIf oGridAvailableColumns.Contains(dgvMergCol.Name) Then
                    leng += Math.Ceiling(dgv.Columns(GetColumnName(dgvMergCol.Name & "~" & spt.ToString)).Width / 8)
                    flag = True
                End If
                If flag Then
                    Dim headName As String = Space(Math.Abs((leng - dgvMergCol.HeaderText.Length) / 2)) + dgvMergCol.HeaderText
                    oReportMergeHeaderStr += LSet(headName, IIf(leng > 0, leng, 0)) + oSepChar
                End If
            Next
        Next
    End Sub

    Private Function GetGridColName(ByVal orgName As String) As String
        Dim sp() As String = orgName.Split("~")
        Return sp(0)
    End Function


    Public Sub Dispo()
        oWrite.Flush()
        oWrite.Close()
        pCondens = Chr(27) + "@"
        'pCondens = Chr(27) + Chr(18)
        If oModeOfPrint = PrintMode.Auto Then
            If oMaxCharPerLine > 80 Then
                Select Case oPrinterType
                    Case PrintType.Column_80
                        If oMaxCharPerLine < 95 Then
                            pCondens += Chr(27) + "M"
                        ElseIf oMaxCharPerLine < 136 Then
                            pCondens += Chr(15)
                        Else
                            pCondens += Chr(27) + "M" + Chr(15)
                        End If
                    Case PrintType.Column_130
                        If oMaxCharPerLine < 110 Then
                            pCondens += Chr(27) + "M"
                        ElseIf oMaxCharPerLine < 154 Then
                            pCondens += Chr(15)
                        Else
                            pCondens += Chr(27) + "M" + Chr(15)
                        End If
                End Select
            Else
                GoTo Create_BatFile
            End If
        ElseIf oModeOfPrint = PrintMode.Condensed Then
            pCondens += Chr(15)
        ElseIf oModeOfPrint = PrintMode.Medium Then
            pCondens += Chr(27) + "M"
        ElseIf oModeOfPrint = PrintMode.Micro Then
            pCondens += Chr(27) + "M" + Chr(15)
        End If

        'Dim filestm As FileStream
        'filestm = New FileStream(oFilePath, FileMode.Open, FileAccess.Read, FileShare.None)
        'Dim sReader As StreamReader = New StreamReader(filestm)
        'sReader.BaseStream.Seek(0, SeekOrigin.Begin)
        'Dim firstLine As String = sReader.ReadLine
        'sReader.Close()
        'filestm.Close()

        'filestm = New FileStream(oFilePath, FileMode.Open, FileAccess.Write, FileShare.None)
        'Dim sWriter As StreamWriter = New StreamWriter(filestm)
        'sWriter.BaseStream.Seek(0, SeekOrigin.Begin)
        'sWriter.Write(pCondens & Space(pCondens.Length - 1) & firstLine)
        'sWriter.Flush()
        'sWriter.Close()

        Dim Fs As FileStream = New FileStream(oFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
        Dim sw As New StreamWriter(Fs)
        Dim sr As New StreamReader(Fs)
        Dim str As String
        str = sr.ReadToEnd()
        str = str.Replace("####$$$$printmode", pCondens)
        Fs.Position = 0
        Fs.SetLength(str.Length)
        sw.Write(str)
        sw.Flush()
        sw.Close()
        Fs.Close()

Create_BatFile:
        oWrite = System.IO.File.CreateText("C:\REPORTS\REPORTPRINT.BAT")
        oWrite.WriteLine("TYPE " + oFilePath + " > " + oPrinterName)
        oWrite.Flush()
        oWrite.Close()
        System.Diagnostics.Process.Start("C:\REPORTS\REPORTPRINT.BAT")
    End Sub

    Private Sub txtLineCounter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLineCounter.TextChanged
        If Val(txtLineCounter.Text) = oLinesPerPage Then
            Dim pageNum As String = Nothing
            pageNum = "Page No : " & oPageNoCurrent.ToString ' oPageNoCurrent.ToString + " of " + Math.Ceiling(oRowsTotal / oLinesPerPage).ToString
            pageNum = Space((oCharPerLine - pageNum.Length) / 2) + pageNum
            oWrite.WriteLine(pageNum)
            oPageNoCurrent += 1
            oWrite.Write(pEjectPaper)
            txtLineCounter.Clear()
            GridView_Header_Page()
            If oReportHeaderStr <> Nothing Then GridView_Header_Report()
        End If
    End Sub
    Private Sub FillGrid(ByVal objMulti As DotMatrixPrintMultiCol)
        With objMulti
            .dtGridView.AcceptChanges()
            .gridView.DataSource = .dtGridView
            For Each gridCol As DataGridViewColumn In .gridView.Columns
                Dim sp() As String = gridCol.Name.Split("~")
                Dim colName As String = sp(0)
                If Not .AutoFitColumns Then
                    gridCol.Width = .dgv.Columns(colName).Width
                End If
                gridCol.CellTemplate = .dgv.Columns(colName).CellTemplate
                gridCol.DefaultCellStyle = .dgv.Columns(colName).DefaultCellStyle
                gridCol.HeaderText = .dgv.Columns(colName).HeaderText
            Next
            Dim rwCount As Integer = 0
            Dim spRwIndex As Integer = 0
            Dim lbCount As Integer = 0
            Dim rwIndex As Integer = 0
            For Each dgvRow As DataGridViewRow In .dgv.Rows
                If rwCount = .pageRowCount Then
                    rwCount = 0
                    lbCount += 1
                    If lbCount <= .noOfSplit - 1 Then
                        rwIndex = (spRwIndex * .pageRowCount)
                    Else
                        spRwIndex += 1
                        rwIndex = (spRwIndex * .pageRowCount)
                        lbCount = 0
                    End If
                End If
                If lbCount = 0 Then .dtGridView.Rows.Add()
                For Each dgvCell As DataGridViewCell In dgvRow.Cells
                    If Not ((dgvCell.OwningColumn.Name.ToUpper = "COLHEAD" Or dgvCell.OwningColumn.Name.ToUpper = "PAGEBREAK") And .noOfSplit = 1) Then
                        If Not .AvailableColumns.Contains(dgvCell.OwningColumn.Name) Then Continue For
                    End If
                    With .gridView.Rows(rwIndex).Cells(dgvCell.OwningColumn.Name + "~" + lbCount.ToString)
                        If dgvRow.Cells(dgvCell.OwningColumn.Name).Visible Then
                            objMulti.dgv.CurrentCell = dgvRow.Cells(dgvCell.OwningColumn.Name)
                        End If
                        .Style.BackColor = dgvCell.InheritedStyle.BackColor
                        .Style.Font = dgvCell.InheritedStyle.Font
                    End With
                    .dtGridView.Rows(rwIndex).Item(dgvCell.OwningColumn.Name + "~" + lbCount.ToString) = dgvCell.Value
                Next
                rwIndex += 1
                rwCount += 1
            Next
           
        End With
    End Sub
#Region "GridView Printer"
    Public Sub Print_GridView(ByVal rptTitle As String _
    , ByVal dgv As DataGridView _
    , ByVal GetSelectedColumns As List(Of String) _
    , ByVal PrntType As PrintType _
    , ByVal PrinterName As String _
    , ByVal SepChar As String _
    , ByVal SepForWholeColumn As Boolean _
    , ByVal MultiCol As Integer _
    , ByVal AutFitColumns As Boolean _
    , ByVal UnderLine As Boolean _
    , ByVal RowSeperation As Boolean _
    , ByVal HeaderBold As Boolean _
    , Optional ByVal PntMode As PrintMode = PrintMode.Auto _
    , Optional ByVal dgvGroupHeader As DataGridView = Nothing _
    , Optional ByVal LinesPerPage As Integer = 60 _
    , Optional ByVal dsFooter As DataSet = Nothing _
    , Optional ByVal Zerosupress As Boolean = False _
    , Optional ByVal WithEject As Boolean = False _
    , Optional ByVal RemHead As Boolean = False)

        oUnderLine = UnderLine
        oRowSeperation = RowSeperation
        oModeOfPrint = PntMode
        oLinesPerPage = LinesPerPage
        oReportTitle = rptTitle
        oGridAvailableColumns = GetSelectedColumns
        oPrinterType = PrntType
        oPrinterName = PrinterName
        oSepChar = SepChar
        oSepForWholeColumn = SepForWholeColumn
        oHeaderBold = HeaderBold
        oMultiCol = MultiCol
        oZeroSupress = Zerosupress
        oWithEject = WithEject
        oRemoveHead = RemHead
        If oPrinterType = PrintType.column_40 Then
            oCharPerLine = 40 : oMaxCharPerLine = 40
        ElseIf oPrinterType = PrintType.Column_80 Then
            oCharPerLine = 160
        Else
            oCharPerLine = 185
        End If

        If GridView_Initializer(rptTitle, dgv) Then Exit Sub
        Dim headerCount As Integer = 0
        For Each str As String In oReportTitle.Split(vbCrLf)
            headerCount += 1
        Next
        headerCount += 2  ''for Header Lines
        headerCount += 1  ''GridHeader
        If Not dgvGroupHeader Is Nothing Then headerCount += 1
        Dim tDgv As New DataGridView
        If oMultiCol > 1 Then
            Dim objMulti As New DotMatrixPrintMultiCol(dgv, oMultiCol, oLinesPerPage - headerCount, oGridAvailableColumns, AutFitColumns)
            'FillGrid(objMulti)
            objMulti.ShowDialog()
            objMulti.Hide()
            tDgv = objMulti.gridView
            If tDgv Is Nothing Then Exit Sub
        Else
            tDgv = dgv
        End If
        If Not dgvGroupHeader Is Nothing Then
            oRowsTotal += 3
        Else
            oRowsTotal += 1
        End If
        oRowsTotal += dgv.RowCount - 1
        oPageNoCurrent = 1
        RowPos = 0
        GridRowCount = tDgv.RowCount
        GridView_Header_Page()
        If Not dgvGroupHeader Is Nothing Then
            Print_GridViewMergeHeader(dgvGroupHeader, tDgv)
        End If
        GridPrint_PrintGridView(tDgv)
        ''PrintLine(oMaxCharPerLine)
        If Not dsFooter Is Nothing Then
            Dim Align As StringAlignment
            Dim FooterLen As Integer = Nothing
            Dim Footer As String = Nothing
            For Each dtFooter As DataTable In dsFooter.Tables
                For Each roFooter As DataRow In dtFooter.Rows
                    Align = StringAlignment.Near
                    If dtFooter.Columns.Contains("ALIGN") Then
                        If roFooter.Item("ALIGN").ToString = "R" Then
                            Align = StringAlignment.Far
                        ElseIf roFooter.Item("ALIGN").ToString = "C" Then
                            Align = StringAlignment.Center
                        Else
                            Align = StringAlignment.Near
                        End If
                    End If
                    Footer = roFooter.Item(0).ToString
                    FooterLen = Footer.Length
                    If Align = StringAlignment.Far Then
                        WriteLine(RSet(Footer, oMaxCharPerLine))
                    ElseIf Align = StringAlignment.Center Then
                        If FooterLen < oMaxCharPerLine Then
                            WriteLine(Space((oMaxCharPerLine - FooterLen) / 2) & Footer & Space((oMaxCharPerLine - FooterLen) / 2))
                        Else
                            WriteLine(Footer)
                        End If
                    Else
                        WriteLine(LSet(Footer, oMaxCharPerLine))
                    End If
                Next
            Next
            PrintLine(oMaxCharPerLine)
        End If
        If oWithEject Then oWrite.WriteLine(pEjectPaper)
        Dispo()
    End Sub


    Private Function GridView_Initializer(ByVal rptTitle As String, ByRef dgv As DataGridView) As Boolean
        ''Initialize
        oReportTitle = rptTitle

        'Dim dtColHead As New DataTable
        'dtColHead.Columns.Add("CAPTION", GetType(String))
        'dtColHead.Columns.Add("NAME", GetType(String))
        'With CType(dgv, DataGridView)
        '    For cnt As Integer = 0 To .ColumnCount - 1
        '        If Not .Columns(cnt).Visible Then Continue For
        '        Dim ro As DataRow = Nothing
        '        ro = dtColHead.NewRow
        '        ro!NAME = .Columns(cnt).Name
        '        ro!CAPTION = .Columns(cnt).HeaderText
        '        dtColHead.Rows.Add(ro)
        '    Next
        'End With
        'dtColHead.AcceptChanges()
        'Dim dlg As New DotMatrixPrintOptions(rptTitle, dtColHead)
        'If dlg.ShowDialog <> DialogResult.OK Then Return True
        'oReportTitle = dlg.txtTitle.Text
        'oGridAvailableColumns = dlg.GetSelectedColumns

        'Dim objPrintDia As New PrintDia
        'If objPrintDia.ShowDialog = DialogResult.OK Then
        '    oPrinterType = IIf(objPrintDia.rbt80Column.Checked, GridView_DotMatrix.PrintType.Column_80, GridView_DotMatrix.PrintType.Column_130)
        '    oPrinterName = objPrintDia.cmbPrinterNames.Text
        '    oSepChar = IIf(objPrintDia.txtHeaderSeperator.Text = "", " ", objPrintDia.txtHeaderSeperator.Text)
        '    oSepForWholeColumn = objPrintDia.chkHeadSep.Checked
        '    oMultiCol = IIf(Val(objPrintDia.txtMultiCol.Text) = 0, 1, Val(objPrintDia.txtMultiCol.Text))
        'Else
        '    Return True
        'End If
        'If oPrinterType = PrintType.Column_80 Then
        '    oCharPerLine = 160
        'Else
        '    oCharPerLine = 185
        'End If
        txtLineCounter.Clear()
        If Not Directory.Exists("C:\REPORTS") Then
            Directory.CreateDirectory("C:\Reports")
        End If
        Dim oFileDupStr As Integer = 1
        oFileName = "REPORT.TXT" '"REPORT" + IIf(oFileDupStr = 0, "", "~" + oFileDupStr.ToString) + ".TXT"
        oFilePath = "C:\REPORTS\" + oFileName
        'GenFilePath:
        '        oFilePath = "C:\REPORTS\" + oFileName
        '        If IO.File.Exists(oFilePath) Then
        '            oFileDupStr += 1
        '            oFileName = "REPORT" + IIf(oFileDupStr = 0, "", "~" + oFileDupStr.ToString) + ".TXT"
        '            GoTo GenFilePath
        '        End If
        oWrite = System.IO.File.CreateText(oFilePath)
        oWrite.WriteLine("")
        oWrite.WriteLine("")
        oWrite.Write(Chr(27) + Chr(67) + Chr(72))
    End Function

    Private Sub GridPrint_PrintGridView(ByVal dgv As DataGridView)
        Dim dgvCellTemp As DataGridViewCell = Nothing
        Dim roTemp As DataRow = Nothing
        Dim dtTemp As New DataTable
        Dim dtDgv As New DataTable
        Dim str As String = Nothing
        Dim Title_Total_Row As Boolean = False
        dtDgv = CType(dgv.DataSource, DataTable)
        dtTemp = dtDgv.Clone
        For Each ColTemp As DataColumn In dtTemp.Columns
            ColTemp.DataType = GetType(String)
        Next
        ''Print Column Header
        oReportHeaderStr = Nothing
        For Each dgvCol As DataGridViewColumn In dgv.Columns
            If Not oGridAvailableColumns.Contains(GetGridColName(dgvCol.Name)) Then Continue For
            Dim HeadName As String = dgvCol.HeaderText
            Dim Cel As DataGridViewCell = dgv.Rows(0).Cells(dgvCol.Name)
            Select Case Cel.InheritedStyle.Alignment
                Case DataGridViewContentAlignment.BottomLeft, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.NotSet, DataGridViewContentAlignment.TopLeft

                    oReportHeaderStr += IIf(oPrinterType = PrintType.column_40, LSet(HeadName, Math.Ceiling(dgvCol.Width / 16)) + oSepChar, LSet(HeadName, Math.Ceiling(dgvCol.Width / 8)) + oSepChar)
                    'LSet(HeadName, Math.Ceiling(dgvCol.Width / 8)) + oSepChar
                Case Else
                    oReportHeaderStr += IIf(oPrinterType = PrintType.column_40, RSet(HeadName, Math.Ceiling(dgvCol.Width / 16)) + oSepChar, RSet(HeadName, Math.Ceiling(dgvCol.Width / 8)) + oSepChar)
                    'oReportHeaderStr += RSet(HeadName, Math.Ceiling(dgvCol.Width / 8)) + oSepChar
            End Select
            'oReportHeaderStr += LSet(HeadName, Math.Ceiling(dgvCol.Width / 8)) + oSepChar
        Next
        Dim i As Integer = 0
        GridView_Header_Report()
        'Row by Row Insertion
        If oPrinterType = PrintType.column_40 Then oMaxCharPerLine = 40
        Do While RowPos <= dgv.RowCount - 1
            Dim GridRow As DataGridViewRow = dgv.Rows(RowPos)
            Title_Total_Row = False
            str = Nothing
            i = 0
            dtTemp.Clear()
            For Each Cel As DataGridViewCell In GridRow.Cells
                Dim sp As String() = Cel.Value.ToString.Split(vbCrLf)
                For cnt As Integer = 0 To sp.Length - 1
                    If Not dtTemp.Rows.Count > cnt Then dtTemp.Rows.Add()
                    dtTemp.Rows(cnt).Item(Cel.OwningColumn.Name) = sp(cnt).Replace(ChrW(10), "")
                Next
            Next
            If dgv.Columns.Contains(GetColumnName("COLHEAD~0")) Then

                Select Case GridRow.Cells(GetColumnName("COLHEAD~0")).Value.ToString.ToUpper
                    Case "S", "S1", "S2"
                        Title_Total_Row = True
                        If oPrinterType = PrintType.column_40 Then oMaxCharPerLine = 40
                        PrintLine(oMaxCharPerLine)
                    Case "G"
                        If oPrinterType = PrintType.column_40 Then oMaxCharPerLine = 40
                        PrintLine(oMaxCharPerLine, "=")
                        Title_Total_Row = True
                        oWrite.Write(pBoldDoubleStart)
                End Select
            End If
            If Not Title_Total_Row And oUnderLine Then oWrite.Write(pUnderLineOn)
            Dim prnVal As String = ""
            For Each RoTe As DataRow In dtTemp.Rows
                str = Nothing
                For Each CoTe As DataColumn In dtTemp.Columns
                    dgvCellTemp = GridRow.Cells(CoTe.ColumnName)
                    If Not oGridAvailableColumns.Contains(GetGridColName(dgvCellTemp.OwningColumn.Name)) Then Continue For
                    If dgvCellTemp.InheritedStyle.Font.Bold Then str += pBoldStart
                    Dim wid As Integer = 0
                    wid = Math.Ceiling(dgvCellTemp.OwningColumn.Width / 8)
                    If oPrinterType = PrintType.column_40 Then wid = Math.Ceiling(dgvCellTemp.OwningColumn.Width / 16)
                    If dtDgv.Columns(CoTe.ColumnName).DataType.Name = GetType(Date).Name Then
                        If RoTe.Item(CoTe).ToString <> "" Then
                            prnVal = Format(Convert.ToDateTime(RoTe.Item(CoTe)), "dd/MM/yyyy")
                        End If
                    Else
                        prnVal = RoTe.Item(CoTe).ToString
                        If oRemoveHead Then
                            If dtTemp.Columns.Contains("COLHEAD") And GetGridColName(dgvCellTemp.OwningColumn.Name) = "PARTICULAR" Then
                                If RoTe.Item("COLHEAD").ToString = "T" _
                                Or RoTe.Item("COLHEAD").ToString = "G" _
                                Or RoTe.Item("COLHEAD").ToString = "S" _
                                Then prnVal = Nothing
                            End If
                        End If
                        If oZeroSupress Then
                            If Trim(prnVal) = "0.00" Or Trim(prnVal) = "0" Then prnVal = Space(Len(RoTe.Item(CoTe).ToString))
                        End If
                    End If
                    Select Case dgvCellTemp.InheritedStyle.Alignment
                        Case DataGridViewContentAlignment.BottomLeft, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.NotSet, DataGridViewContentAlignment.TopLeft
                            If oPrinterType = PrintType.column_40 Then prnVal = Trim(prnVal)
                            str += LSet(prnVal, wid) _
                                    + IIf(oSepForWholeColumn, oSepChar, " ")
                        Case Else

                            str += RSet(prnVal, wid) _
                                    + IIf(oSepForWholeColumn, oSepChar, " ")
                    End Select
                    If dgvCellTemp.InheritedStyle.Font.Bold Then str += pBoldEnd
                Next

                WriteLine(str)
            Next
            RowPos += 1
            If Not Title_Total_Row And oUnderLine Then oWrite.Write(pUnderLineOff)

            If Not Title_Total_Row And oRowSeperation Then PrintLine(oMaxCharPerLine)
            If dgv.Columns.Contains(GetColumnName("COLHEAD~0")) Then
                Select Case GridRow.Cells(GetColumnName("COLHEAD~0")).Value.ToString.ToUpper
                    Case "S", "S1", "S2"
                        If oPrinterType = PrintType.column_40 Then oMaxCharPerLine = 40
                        PrintLine(oMaxCharPerLine)
                    Case "G"
                        If oPrinterType = PrintType.column_40 Then oMaxCharPerLine = 40
                        PrintLine(oMaxCharPerLine, "=")
                        oWrite.Write(pBoldDoubleEnd)
                End Select
            End If
        Loop
    End Sub
#End Region

End Class
