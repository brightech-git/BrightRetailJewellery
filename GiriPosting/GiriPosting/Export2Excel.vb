Imports System.Windows.Forms
Imports SA.TblProc
'Imports Microsoft.Office.Interop

Public Class Export2Excel
    Public Delegate Sub ProgressHandler(ByVal sender As Object, ByVal e As ProgressEventArgs)
    Public Event _Prg As ProgressHandler

    Private _Dgv As New DataGridView
    Private _DgvOptionalHeader As New DataGridView
    Private _VisibleColumnsOnly As Boolean
    Private _HighlightColor As Boolean
    Private _HighlightFont As Boolean
    Private _Excel As Boolean
    Private _ExpressWayExcel As Boolean
    Private _QuickExcelSave As Boolean
    Private _Title As String
    Private _SelectedColumns As List(Of String)
    Private _ColCount As Integer = 0
    Private _SavePath As String

    Public Sub New(ByVal dgv As DataGridView, ByVal isExcel As Boolean, ByVal selectedColumns As List(Of String), Optional ByVal DgvOptionalHeader As DataGridView = Nothing, Optional ByVal Savepath As String = Nothing)
        _Dgv = dgv
        _Excel = isExcel
        _DgvOptionalHeader = DgvOptionalHeader
        _SelectedColumns = selectedColumns
        _SavePath = Savepath
    End Sub
    Public Property Title() As String
        Get
            Return _Title
        End Get
        Set(ByVal value As String)
            _Title = value
        End Set
    End Property

    Public Property VisibleColumnsOnly() As Boolean
        Get
            Return _VisibleColumnsOnly
        End Get
        Set(ByVal value As Boolean)
            _VisibleColumnsOnly = value
        End Set
    End Property

    Public Property HighlightColor() As Boolean
        Get
            Return _HighlightColor
        End Get
        Set(ByVal value As Boolean)
            _HighlightColor = value
        End Set
    End Property

    Public Property ExpressWayExcel() As Boolean
        Get
            Return _ExpressWayExcel
        End Get
        Set(ByVal value As Boolean)
            _ExpressWayExcel = value
        End Set
    End Property
    Public Property QuickExcelSave() As Boolean
        Get
            Return _QuickExcelSave
        End Get
        Set(ByVal value As Boolean)
            _QuickExcelSave = value
        End Set
    End Property


    Public Property HighlightFont() As Boolean
        Get
            Return _HighlightFont
        End Get
        Set(ByVal value As Boolean)
            _HighlightFont = value
        End Set
    End Property
    Private Sub fTitle(ByVal _Sheet As TableSheet, ByRef stRow As Integer)
        Dim sp() As String = Title.Split(Chr(Keys.Enter))
        For Each s As String In sp
            Dim _Range As TableRange = _Sheet.Range(stRow, 1, stRow, _ColCount)
            With _Range
                s = s.Replace(Chr(Keys.Enter), "")
                If s <> "" Then
                    If Asc(Mid(s, 1, 1)) = 10 Then
                        s = s.Replace(Chr(Asc(Mid(s, 1, 1))), "")
                    End If
                End If
                .Merge()
                .HAlign = HAlign.Left
                .VAlign = VAlign.Center
                .Value = s.Trim
                .FontSize = 9
                .FontBold = True
                .WrapText = False
                .BackgroundColorIndex = 50
                .FontColorIndex = 2
            End With
            stRow += 1
        Next
    End Sub
    Private Sub fHeader(ByVal _Sheet As TableSheet, ByRef stRow As Integer)
        ''Inserting Column Heading
        Dim _Col As Integer = 1
        For Each _DgvCol As DataGridViewColumn In _Dgv.Columns
            If _VisibleColumnsOnly And Not _DgvCol.Visible Then Continue For
            If Not _SelectedColumns.Contains(_DgvCol.Name) Then Continue For
            _Sheet.Cell(stRow, _Col).Value = _DgvCol.HeaderText
            _Sheet.Cell(stRow, _Col).FontBold = True
            _Sheet.Cell(stRow, _Col).FontSize = 8
            _Sheet.Cell(stRow, _Col).BackgroundColorIndex = 35
            _Col += 1
        Next
        stRow += 1
    End Sub
    Private Sub fOptionalHeader(ByVal _Sheet As TableSheet, ByRef stRow As Integer)
        ''Inserting Optional Grid Column Heading
        If _DgvOptionalHeader Is Nothing Then Exit Sub
        If _DgvOptionalHeader.Visible = False Then Exit Sub
        If Not _DgvOptionalHeader.Columns.Count > 0 Then Exit Sub
        Dim _Col As Integer = 1
        For Each _DgvCol As DataGridViewColumn In _DgvOptionalHeader.Columns
            Dim sp() As String = _DgvCol.Name.Split("~")
            Dim stCol As Integer = _Col
            For Each s As String In sp
                If Not _Dgv.Columns.Contains(s) Then Continue For
                If Not _SelectedColumns.Contains(s) Then Continue For
                If _VisibleColumnsOnly And Not _Dgv.Columns(s).Visible Then Continue For
                _Col += 1
            Next
            If _Col > stCol Then
                _Sheet.Range(stRow, stCol, stRow, _Col - 1).Merge()
                _Sheet.Range(stRow, stCol, stRow, _Col - 1).HAlign = HAlign.Center
            End If
        Next
        _Col = 1
        For Each _DgvCol As DataGridViewColumn In _Dgv.Columns
            If _VisibleColumnsOnly And Not _DgvCol.Visible Then Continue For
            If Not _SelectedColumns.Contains(_DgvCol.Name) Then Continue For
            _Sheet.Cell(stRow, _Col).Value = _DgvOptionalHeader.Columns(fGetOptMapColName(_DgvCol.Name)).HeaderText
            _Sheet.Cell(stRow, _Col).FontSize = 8
            _Sheet.Cell(stRow, _Col).FontBold = True
            _Sheet.Cell(stRow, _Col).BackgroundColorIndex = 35 '43
            _Col += 1
        Next
        stRow += 1
    End Sub
    Private Function fGetOptMapColName(ByVal colName As String) As String
        Dim retColName As String = colName
        For Each _DgvCol As DataGridViewColumn In _DgvOptionalHeader.Columns
            If UCase(_DgvCol.Name).Contains(colName.ToUpper) Then
                retColName = _DgvCol.Name
                Exit For
            End If
        Next
        Return retColName
    End Function

    Private Function fGetFirstVisibleColumnIndex() As Integer
        For cnt As Integer = 0 To _Dgv.ColumnCount - 1
            If _Dgv.Columns(cnt).Visible Then Return cnt
        Next
    End Function
    Private Function CheckDate(ByVal chkDate As String) As Boolean
        Try
            If (chkDate.Contains("/")) Then
                Dim dt As DateTime = DateTime.Parse(chkDate)
                Return True
            End If
        Catch
            Return False
        End Try
    End Function


    Public Sub ExpressfPost()
        Dim dt As DataTable
        Dim dt1 As DataTable
        Dim _ColCnt As Integer = 0
        Dim _DgvColCnt As Integer

        dt = _Dgv.DataSource
        dt1 = dt.Copy

        For _DgvColCnt = 0 To _Dgv.Columns.Count - 1
            If Not _SelectedColumns.Contains(_Dgv.Columns(_DgvColCnt).Name.ToString()) Then
                If Not dt.Columns.Contains(_Dgv.Columns(_DgvColCnt).Name.ToString()) Then _ColCnt += 1 : Continue For
                Try
                    dt1.Columns.Remove(_Dgv.Columns(_DgvColCnt).Name.ToString())
                Catch ex As Exception
                End Try
            End If
            _ColCnt += 1
        Next



        Try
            Using wb As New ClosedXML.Excel.XLWorkbook
                wb.Worksheets.Add(dt1, "Sheet1$")
                wb.SaveAs(_SavePath)
                wb.Dispose()
            End Using
            MsgBox("Exported..")
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try

    End Sub


    Public Sub QuickExcelSavePost()
        Try
            Dim dt As DataTable
            Dim dt1 As DataTable
            Dim _ColCnt As Integer = 0
            Dim _DgvColCnt As Integer

            dt = _Dgv.DataSource
            dt1 = dt.Copy

            For _DgvColCnt = 0 To _Dgv.Columns.Count - 1
                If Not _SelectedColumns.Contains(_Dgv.Columns(_DgvColCnt).Name.ToString()) Then
                    If Not dt.Columns.Contains(_Dgv.Columns(_DgvColCnt).Name.ToString()) Then _ColCnt += 1 : Continue For
                    Try
                        dt1.Columns.Remove(_Dgv.Columns(_DgvColCnt).Name.ToString())
                    Catch ex As Exception
                    End Try
                End If
                _ColCnt += 1
            Next

            Dim _dvgview As New DataGridView
            _dvgview.DataSource = Nothing
            _dvgview.DataSource = dt1

            Dim workbook As Spire.Xls.Workbook = New Spire.Xls.Workbook()
            Dim sheet As Spire.Xls.Worksheet = workbook.Worksheets(0)
            sheet.InsertDataTable(CType(_dvgview.DataSource, DataTable), True, 1, 1, -1, -1)

            Dim result As String = _SavePath
            workbook.SaveToFile(result)

            Dim xlApp As Spire.Xls.Workbook = New Spire.Xls.Workbook()
            xlApp.LoadFromFile(_SavePath.ToString)
Nextt:
            For i As Integer = 0 To xlApp.Worksheets.Count - 1
                If i = 0 Then Continue For
                xlApp.Worksheets.Remove(i)
                GoTo Nextt
            Next
            xlApp.Save()
            xlApp.Dispose()

            MsgBox("Exported..")
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub


    Public Sub fPost(Optional ByVal DateFrm As String = "dd/MM/yyyy", Optional ByVal DateAsStr As Boolean = False)
        Dim obj As Object = ""
        Try
            Dim _Position As Integer = 0
            Dim _Sheet As TableSheet = Nothing
            If _Excel Then
                _Sheet = TableSheet.CreateAvailable("Sheet1")
            Else
                '_Sheet = OO.OOSheet.CreateOOSheet("Sheet1")
                _Sheet = SA.TblProc.OO.OOSheet.CreateOOSheet("Sheet1")
            End If
            Dim _Row As Integer = 1
            Dim _Col As Integer = 1
            For Each _DgvCol As DataGridViewColumn In _Dgv.Columns
                If _VisibleColumnsOnly And Not _DgvCol.Visible Then Continue For
                'If Not _SelectedColumns.Contains(_DgvCol.Name) Then Continue For
                _ColCount += 1
            Next

            ''Insert Title
            fTitle(_Sheet, _Row)
            ''Insert Optional Header
            fOptionalHeader(_Sheet, _Row)
            ''Insert Header
            fHeader(_Sheet, _Row)

            Dim _RptStRow As Integer = _Row

            ''Inserting Row by Row
            _Col = 1
            _Dgv.Select()
            Dim _DgvRow As DataGridViewRow = Nothing
            Dim _PuncInd As Integer = Nothing
            Dim _NumberFormat As String = ""
            For cnt As Integer = 0 To _Dgv.RowCount - 1
                _DgvRow = _Dgv.Rows(cnt)
                For Each _DgvCell As DataGridViewCell In _DgvRow.Cells
                    If VisibleColumnsOnly And Not _DgvCell.OwningColumn.Visible Then Continue For
                    If Not _SelectedColumns.Contains(_DgvCell.OwningColumn.Name) Then Continue For
                    _PuncInd = -1
                    'obj = _DgvCell.Value
                    ''_Sheet.Cell(_Row, _Col).Value = obj.ToString.Trim  ' _DgvCell.Value
                    '_Sheet.Cell(_Row, _Col).Value = _DgvCell.Value
                    obj = _DgvCell.Value.ToString

                    If CheckDate(_DgvCell.Value.ToString()) And DateAsStr = False Then
                        Dim type As Object
                        type = _DgvCell.ValueType.Name

                        If _DgvCell.ValueType.Name = GetType(DateTime).Name Then
                            Dim result As Date
                            Dim dt As DateTime = DateTime.Parse(_DgvCell.Value)
                            result = DateTime.Parse(dt)
                            obj = Convert.ToDateTime(result)
                        Else
                            Dim result As DateTime
                            Dim value As DateTime = DateTime.Parse(_DgvCell.Value)
                            result = DateTime.Parse(value).ToString("MM/dd/yyyy")
                            Dim dayString As String = result.Day.ToString()
                            Dim monthString As String = result.Month.ToString()
                            Dim yearString As String = result.Year.ToString()
                            obj = dayString + "/" + monthString + "/" + yearString
                        End If

                    End If

                    If _DgvCell.Value.GetType = GetType(Bitmap) Then

                        _Sheet.Cell(_Row, _Col).Value = obj


                        'Dim Excel As Object = CreateObject(“Excel.Application”)
                        'Dim imagString As String = "bitmap1.bmp"
                        'Dim oRange As Excel.Range = CType(_Sheet.Cells(_Row, _Col), Excel.Range)
                        'Dim Left As Single = CType(CType(oRange.Left, Double), Single)
                        'Dim Top As Single = CType(CType(oRange.Top, Double), Single)
                        'Dim ImageSize As Single = 32
                        'Dim ms As Microsoft.Office.Core.MsoTriState
                        'xlWorkSheet.Shapes.AddPicture(imagString, ms.msoFalse, ms.msoCTrue, Left, Top, ImageSize, ImageSize)
                    End If


                    _Sheet.Cell(_Row, _Col).Value = obj

                    If IsNumeric(obj) Then
                        _PuncInd = obj.ToString.IndexOf(".")
                        If _PuncInd > -1 Then
                            _NumberFormat = "0."
                            For Each fr As String In Mid(obj, _PuncInd + 2, obj.ToString.Length)
                                _NumberFormat += "0"
                            Next
                            If _Excel Then _Sheet.Cell(_Row, _Col).NumberFormat = _NumberFormat
                        End If
                    End If

                    If IsDate(obj) And DateAsStr = False Then
                        _PuncInd = obj.ToString.IndexOf("/")
                        If _PuncInd > -1 Then
                            _NumberFormat = "dd/MM/yyyy"
                            If _Excel Then _Sheet.Cell(_Row, _Col).NumberFormat = _NumberFormat
                            'If _Excel And DateAsStr Then _Sheet.Cell(_Row, _Col).NumberFormat = "@"
                        End If
                    End If

                    If _HighlightFont Then
                        If (_DgvCell.Style.Font IsNot Nothing Or _
                            _DgvCell.OwningColumn.DefaultCellStyle.Font IsNot Nothing Or _
                            _DgvCell.OwningRow.DefaultCellStyle.Font IsNot Nothing) Then
                            fSetCellFont(_Sheet.Cell(_Row, _Col), _DgvCell)
                        End If
                    End If
                    If _HighlightColor Then
                        If (_DgvCell.Style.BackColor <> Nothing Or _
                            _DgvCell.OwningColumn.DefaultCellStyle.BackColor <> Nothing Or _
                            _DgvCell.OwningRow.DefaultCellStyle.BackColor <> Nothing) Then
                            fSetCellColor(_Sheet.Cell(_Row, _Col), _DgvCell)
                        End If
                    End If
                    _Col += 1
                Next
                _Position = (100 / _Dgv.RowCount) * (_Row - _RptStRow)
                _Row += 1
                _Col = 1
                Dim pe As New ProgressEventArgs(_Position)
                OnProgressChange(pe)
            Next
            _Col = 1
            For Each _DgvCol As DataGridViewColumn In _Dgv.Columns
                If _VisibleColumnsOnly And Not _DgvCol.Visible Then Continue For
                If Not _SelectedColumns.Contains(_DgvCol.Name) Then Continue For
                If _DgvCol.ValueType Is Nothing Then
                    _Col += 1
                    Continue For
                End If

                If _DgvCol.ValueType.Name = GetType(Decimal).Name Or _DgvCol.ValueType.Name = GetType(Double).Name Then
                    Dim ret As String = CType(_Dgv.DataSource, DataTable).Compute("MAX([" & _DgvCol.Name & "])", "[" & _DgvCol.Name & "] IS NOT NULL ").ToString
                    Dim formt As String = "0."
                    If Mid(ret, InStr(ret, ".") + 1, ret.Length).Length > 2 Then
                        For Each c As Char In Mid(ret, InStr(ret, ".") + 1, ret.Length)
                            formt += "0"
                        Next
                        _Sheet.Range(_RptStRow, _Col, _Row, _Col).NumberFormat = formt
                    End If
                    _Sheet.Range(_RptStRow, _Col, _Row, _Col).HAlign = HAlign.Right
                ElseIf _DgvCol.ValueType.Name = GetType(DateTime).Name Then
                    If _Excel Then _Sheet.Range(_RptStRow, _Col, _Row, _Col).NumberFormat = DateFrm
                ElseIf _DgvCol.ValueType.Name = GetType(String).Name Then
                    _Sheet.Range(_RptStRow, _Col, _Row, _Col).HAlign = HAlign.Left
                End If
                Select Case _DgvCol.DefaultCellStyle.Alignment
                    Case DataGridViewContentAlignment.BottomRight, DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.TopRight
                        _Sheet.Range(_RptStRow, _Col, _Row, _Col).HAlign = HAlign.Right
                End Select
                _Col += 1
            Next
            _Sheet.Range(_RptStRow, 1, _Row, _ColCount).FontName = "VERDANA"
            _Sheet.Range(_RptStRow, 1, _Row, _ColCount).FontSize = "8"
            _Sheet.AutoFitColumns()
            _Sheet.Visible = True
        Catch ex As Exception
            MsgBox(obj.ToString + vbCrLf + ex.Message + vbCrLf + ex.StackTrace)
            fKillExcel()
        End Try
    End Sub
    Public Sub fPost_old(Optional ByVal DateFrm As String = "dd/MM/yyyy")
        Dim obj As Object = ""
        Try
            Dim _Position As Integer = 0
            Dim _Sheet As TableSheet = Nothing
            If _Excel Then
                _Sheet = TableSheet.CreateAvailable("Sheet1")
            Else
                '_Sheet = OO.OOSheet.CreateOOSheet("Sheet1")
                _Sheet = SA.TblProc.OO.OOSheet.CreateOOSheet("Sheet1")
            End If
            Dim _Row As Integer = 1
            Dim _Col As Integer = 1
            For Each _DgvCol As DataGridViewColumn In _Dgv.Columns
                If _VisibleColumnsOnly And Not _DgvCol.Visible Then Continue For
                'If Not _SelectedColumns.Contains(_DgvCol.Name) Then Continue For
                _ColCount += 1
            Next

            ''Insert Title
            fTitle(_Sheet, _Row)
            ''Insert Optional Header
            fOptionalHeader(_Sheet, _Row)
            ''Insert Header
            fHeader(_Sheet, _Row)

            Dim _RptStRow As Integer = _Row

            ''Inserting Row by Row
            _Col = 1
            _Dgv.Select()
            Dim _DgvRow As DataGridViewRow = Nothing
            Dim _PuncInd As Integer = Nothing
            Dim _NumberFormat As String = ""
            For cnt As Integer = 0 To _Dgv.RowCount - 1
                _DgvRow = _Dgv.Rows(cnt)
                For Each _DgvCell As DataGridViewCell In _DgvRow.Cells
                    If VisibleColumnsOnly And Not _DgvCell.OwningColumn.Visible Then Continue For
                    If Not _SelectedColumns.Contains(_DgvCell.OwningColumn.Name) Then Continue For
                    _PuncInd = -1
                    obj = _DgvCell.Value
                    '_Sheet.Cell(_Row, _Col).Value = obj.ToString.Trim  ' _DgvCell.Value
                    _Sheet.Cell(_Row, _Col).Value = _DgvCell.Value
                    If IsNumeric(obj) Then
                        _PuncInd = obj.ToString.IndexOf(".")
                        If _PuncInd > -1 Then
                            _NumberFormat = "0."
                            For Each fr As String In Mid(obj, _PuncInd + 2, obj.ToString.Length)
                                _NumberFormat += "0"
                            Next
                            If _Excel Then _Sheet.Cell(_Row, _Col).NumberFormat = _NumberFormat
                        End If
                    End If
                    If _HighlightFont Then
                        If (_DgvCell.Style.Font IsNot Nothing Or _
                            _DgvCell.OwningColumn.DefaultCellStyle.Font IsNot Nothing Or _
                            _DgvCell.OwningRow.DefaultCellStyle.Font IsNot Nothing) Then
                            fSetCellFont(_Sheet.Cell(_Row, _Col), _DgvCell)
                        End If
                    End If
                    If _HighlightColor Then
                        If (_DgvCell.Style.BackColor <> Nothing Or _
                            _DgvCell.OwningColumn.DefaultCellStyle.BackColor <> Nothing Or _
                            _DgvCell.OwningRow.DefaultCellStyle.BackColor <> Nothing) Then
                            fSetCellColor(_Sheet.Cell(_Row, _Col), _DgvCell)
                        End If
                    End If
                    _Col += 1
                Next
                _Position = (100 / _Dgv.RowCount) * (_Row - _RptStRow)
                _Row += 1
                _Col = 1
                Dim pe As New ProgressEventArgs(_Position)
                OnProgressChange(pe)
            Next
            _Col = 1
            For Each _DgvCol As DataGridViewColumn In _Dgv.Columns
                If _VisibleColumnsOnly And Not _DgvCol.Visible Then Continue For
                If Not _SelectedColumns.Contains(_DgvCol.Name) Then Continue For
                If _DgvCol.ValueType Is Nothing Then
                    _Col += 1
                    Continue For
                End If
                If _DgvCol.ValueType.Name = GetType(Decimal).Name Or _DgvCol.ValueType.Name = GetType(Double).Name Then
                    Dim ret As String = CType(_Dgv.DataSource, DataTable).Compute("MAX([" & _DgvCol.Name & "])", "[" & _DgvCol.Name & "] IS NOT NULL ").ToString
                    Dim formt As String = "0."
                    If Mid(ret, InStr(ret, ".") + 1, ret.Length).Length > 2 Then
                        For Each c As Char In Mid(ret, InStr(ret, ".") + 1, ret.Length)
                            formt += "0"
                        Next
                        _Sheet.Range(_RptStRow, _Col, _Row, _Col).NumberFormat = formt
                    End If
                    _Sheet.Range(_RptStRow, _Col, _Row, _Col).HAlign = HAlign.Right
                ElseIf _DgvCol.ValueType.Name = GetType(DateTime).Name Then
                    If _Excel Then _Sheet.Range(_RptStRow, _Col, _Row, _Col).NumberFormat = DateFrm
                End If
                Select Case _DgvCol.DefaultCellStyle.Alignment
                    Case DataGridViewContentAlignment.BottomRight, DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.TopRight
                        _Sheet.Range(_RptStRow, _Col, _Row, _Col).HAlign = HAlign.Right
                End Select
                _Col += 1
            Next
            _Sheet.Range(_RptStRow, 1, _Row, _ColCount).FontName = "VERDANA"
            _Sheet.Range(_RptStRow, 1, _Row, _ColCount).FontSize = "8"
            _Sheet.AutoFitColumns()
            _Sheet.Visible = True
        Catch ex As Exception
            MsgBox(obj.ToString + vbCrLf + ex.Message + vbCrLf + ex.StackTrace)
            fKillExcel()
        End Try
    End Sub
    Private Sub fKillExcel()
        Try
            Dim ps() As Process = Process.GetProcesses
            For Each p As Process In ps
                If p.ProcessName.ToUpper = "EXCEL" Then p.Kill()
                If p.ProcessName.ToUpper = "sCalc" Then p.Kill()
            Next
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub fSetCellFont(ByVal _Range As TableRange, ByVal _DgvCell As DataGridViewCell)
        Dim fnt As Font = Nothing
        If _DgvCell.Style.Font IsNot Nothing Then
            fnt = _DgvCell.Style.Font
        ElseIf _DgvCell.OwningColumn.DefaultCellStyle.Font IsNot Nothing Then
            fnt = _DgvCell.OwningColumn.DefaultCellStyle.Font
        ElseIf _DgvCell.OwningRow.DefaultCellStyle.Font IsNot Nothing Then
            fnt = _DgvCell.OwningRow.DefaultCellStyle.Font
        End If
        If fnt IsNot Nothing Then
            _Range.FontName = fnt.Name
            _Range.FontSize = fnt.Size
            _Range.FontBold = fnt.Bold
        End If
    End Sub
    Private Sub fSetCellColor(ByVal _Range As TableRange, ByVal _DgvCell As DataGridViewCell)
        Dim clr As Color = Nothing
        If _DgvCell.Style.BackColor <> Nothing Then
            clr = _DgvCell.Style.BackColor
        ElseIf _DgvCell.OwningColumn.DefaultCellStyle.BackColor <> Nothing Then
            clr = _DgvCell.OwningColumn.DefaultCellStyle.BackColor
        ElseIf _DgvCell.OwningRow.DefaultCellStyle.BackColor <> Nothing Then
            clr = _DgvCell.OwningRow.DefaultCellStyle.BackColor
        End If
        If clr <> Nothing Then
            _Range.BackgroundColorIndex = 36
        End If
    End Sub
    Public Overridable Sub OnProgressChange(ByVal e As ProgressEventArgs)
        RaiseEvent _Prg(Me, e)
    End Sub

End Class
Public Class ProgressEventArgs
    Inherits EventArgs
    Private _PrgValue As Integer = 0
    Public Sub New(ByVal _PrgValue As Integer)
        Me._PrgValue = _PrgValue
    End Sub
    Public ReadOnly Property ProgressValue() As Integer
        Get
            Return _PrgValue
        End Get
    End Property
End Class
