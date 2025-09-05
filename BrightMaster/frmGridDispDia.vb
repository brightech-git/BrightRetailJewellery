Imports System.Data.OleDb
Imports System.IO
Public Class frmGridDispDia
    Public formuser As Integer
    Public dsGrid As New DataSet
    Dim strSql As String
    Public dtFilteration As New DataTable ''This table has 2 columns. one is filter caption and another one is filter text
    Public FormReLocation As Boolean = True
    Public FormReSize As Boolean = True
    Public WithEvents TxtBox As New TextBox 'this control helps the user to raise events like if user press d or ...
    Public BaseName As String = Nothing
    Public da As New OleDbDataAdapter()
    'Public tagViewDetail As New frmTagViewDetails

    Dim strprint As String
    Dim FileWrite As StreamWriter
    Dim PgNo As Integer
    Dim line As Integer
    Public Sub New(Optional ByVal Detprint As Boolean = False)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        tStripExcel.Image = My.Resources.Excel_icon_22
        tStripPrint.Image = My.Resources.Printer_22
        If Detprint = True Then
            TStripDetPrint.Visible = True
        End If
    End Sub

    Public Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If FormReSize Then
            Dim wid As Double = Nothing
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then
                    wid += gridView.Columns(cnt).Width
                End If
            Next
            wid += 10
            If CType(gridView.Controls(1), VScrollBar).Visible Then
                wid += 20
            End If
            wid += 20
            If wid > ScreenWid Then
                wid = ScreenWid
            End If
            Me.Size = New Size(wid, Me.Height)
        End If
        If FormReLocation Then SetCenterLocation(Me)
    End Sub

    Private Sub gridView_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles gridView.DataError

    End Sub

    Private Sub tStripExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripExcel.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(IIf(BaseName = Nothing, Me.Name, BaseName), strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, IIf(gridViewHeader.Visible, gridViewHeader, Nothing), , formuser)
        End If
    End Sub

    Private Sub tStripPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(IIf(BaseName = Nothing, Me.Name, BaseName), strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, IIf(gridViewHeader.Visible, gridViewHeader, Nothing), , formuser)
        End If
    End Sub

    Private Sub frmGridDispDia_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'SetCenterLocation(Me)
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridViewHeader.BackgroundColor = SystemColors.Control
    End Sub

    Private Sub tStripExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If UCase(e.KeyChar) = "X" Then
            tStripExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            tStripPrint_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                Dim ind As Integer = gridView.FirstDisplayedCell.ColumnIndex
                If gridView.CurrentRow Is Nothing Then Exit Sub
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(gridView.CurrentCell.ColumnIndex)
            End If
        End If
    End Sub
    Public Sub SetFilterStr(ByVal filterColumnName As String, ByVal setValue As Object)
        If Not dtFilteration.Columns.Contains(filterColumnName) Then Exit Sub
        If Not dtFilteration.Rows.Count > 0 Then Exit Sub
        dtFilteration.Rows(0).Item(filterColumnName) = setValue
    End Sub

    Public Function GetFilterStr(ByVal filterColumnName As String) As String
        Dim ftrStr As String = Nothing
        If Not dtFilteration.Columns.Contains(filterColumnName) Then Return ftrStr
        If Not dtFilteration.Rows.Count > 0 Then Return ftrStr
        Return dtFilteration.Rows(0).Item(filterColumnName).ToString
        Return ftrStr
    End Function

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHeader.HorizontalScrollingOffset = e.NewValue
                If gridViewHeader.Columns.Count > 0 Then
                    If gridViewHeader.Columns.Contains("SCROLL") Then
                        gridViewHeader.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                        gridViewHeader.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
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
            SetGridHeadColWidth(gridViewHeader)
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub gridViewHeader_ColumnHeadersHeightChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridViewHeader.ColumnHeadersHeightChanged
        If gridViewHeader.DataSource IsNot Nothing Then
            gridViewHeader.Size = New Size(gridViewHeader.Size.Width, gridViewHeader.ColumnHeadersHeight - 4)
        End If
    End Sub

    'Private Sub TStripDetPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TStripDetPrint.Click
    '    detprint()
    'End Sub
    'Function detprint()
    '    Dim CompanyName, Address1, Address2, Address3, Phone As String
    '    Dim dtprint As New DataTable
    '    Dim dtheader As New DataTable
    '    Dim i As Integer
    '    Dim dt As New DataTable
    '    dtprint.Clear()
    '    dtheader.Clear()

    '    dtprint = gridView.DataSource
    '    'dtheader = gridViewHeader.DataSource

    '    strSql = "SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4 FROM " & cnAdminDb & "..COMPANY"
    '    da = New OleDbDataAdapter(strSql, cn)
    '    da.Fill(dt)

    '    CompanyName = dt.Rows(1).Item("COMPANYNAME").ToString
    '    Address1 = dt.Rows(1).Item("ADDRESS1").ToString
    '    Address2 = dt.Rows(1).Item("ADDRESS2").ToString
    '    Address3 = dt.Rows(1).Item("ADDRESS3").ToString

    '    FileWrite = File.CreateText(Application.StartupPath + "\FilePrint.txt")
    '    PgNo = 0
    '    line = 0
    '    Dim str1 As String = Space(22) : Dim str2 As String = Space(35) : Dim str3 As String = Space(8)
    '    Dim str4 As String = Space(12) : Dim str5 As String = Space(12) : Dim str6 As String = Space(12)
    '    Dim str7 As String = Space(12) : Dim str8 As String = Space(25)
    '    strprint = Chr(27) + "@"
    '    FileWrite.WriteLine(strprint)
    '    line += 1

    '    If dtprint.Rows.Count > 0 Then
    '        printheader(CompanyName, Address1, Address2, Address3)
    '        For i = 0 To dtprint.Rows.Count - 1
    '            str1 = LSet(dtprint.Rows(i).Item(0).ToString, 22)
    '            str2 = LSet(dtprint.Rows(i).Item(2).ToString, 35)
    '            str3 = LSet(dtprint.Rows(i).Item(3).ToString, 8)
    '            If dtprint.Rows(i).Item(5).ToString = "0.00" Then
    '                str4 = Space(12)
    '            Else
    '                str4 = RSet(dtprint.Rows(i).Item(5).ToString, 12)
    '            End If

    '            If dtprint.Rows(i).Item(6).ToString = "0.00" Then
    '                str5 = Space(12)
    '            Else
    '                str5 = RSet(dtprint.Rows(i).Item(6).ToString, 12)
    '            End If
    '            If dtprint.Rows(i).Item(7).ToString = "0.00" Or dtprint.Rows(i).Item(7).ToString = "0" Then
    '                str6 = Space(12)
    '            Else
    '                str6 = RSet(dtprint.Rows(i).Item(7).ToString, 12)
    '            End If

    '            If dtprint.Rows(i).Item(8).ToString = "0.00" Or dtprint.Rows(i).Item(8).ToString = "0" Then
    '                str7 = Space(12)
    '            Else
    '                str7 = RSet(dtprint.Rows(i).Item(8).ToString, 12)
    '            End If
    '            str8 = LSet(dtprint.Rows(i).Item(4).ToString, 12)


    '            If dtprint.Rows(i).Item(2).ToString.Trim = "CLOSING BALANCE" Then
    '                strprint = "------------------------------------------------------------------------------------------------------------"
    '                FileWrite.WriteLine(strprint)
    '                line += 1
    '            End If

    '            strprint = str1 + Space(2) + str3 + Space(5) + str2 + Space(1) + str4 + Space(1) + str5 + Space(1) + str6 + Space(1) + str7
    '            FileWrite.WriteLine(strprint)
    '            line += 1
    '            If dtprint.Rows(i).Item(4).ToString <> "" Then
    '                strprint = Space(37) + str8
    '                FileWrite.WriteLine(strprint)
    '                line += 1
    '            End If

    '            If dtprint.Rows(i).Item(2).ToString.Trim = "CLOSING BALANCE" Then
    '                strprint = "----------------------------------------------------------------------------------------------------------------------------"
    '                FileWrite.WriteLine(strprint)
    '                line += 1
    '            End If
    '            If line >= 61 Then
    '                strprint = "----------------------------------------------------------------------------------------------------------------------------"
    '                FileWrite.WriteLine(strprint)
    '                strprint = Chr(12)
    '                FileWrite.WriteLine(strprint)
    '                strprint = Chr(27) + Chr(18)
    '                FileWrite.WriteLine(strprint)
    '                printheader(CompanyName, Address1, Address2, Address3)
    '            End If
    '        Next
    '    End If
    '    FileWrite.Close()
    '    line += 1
    '    frmPrinterSelect.Show()
    'End Function
    Function printheader(ByVal CompanyName As String, Optional ByVal Address1 As String = Nothing, Optional ByVal Address2 As String = Nothing, Optional ByVal Address3 As String = Nothing)
        PgNo += 1
        line = 0
        Dim str1 As String = Space(22) : Dim str2 As String = Space(35) : Dim str3 As String = Space(8)
        Dim str4 As String = Space(12) : Dim str5 As String = Space(12) : Dim str6 As String = Space(12)
        Dim str7 As String = Space(12) : Dim str8 As String = Space(20)

        strprint = Space((132 - Len(strCompanyName)) / 2) + strCompanyName
        FileWrite.WriteLine(strprint) : line += 1
        strprint = Space((132 - Len(Trim(Address1 & "," & Address2 & "," & Address3))) / 2) + Address1 + Address2 + Address3
        FileWrite.WriteLine(strprint) : line += 1
        strprint = Space((132 - Len(lblTitle.Text)) / 2) + lblTitle.Text
        FileWrite.WriteLine(strprint) : line += 1
        strprint = Space(118) & "Pg #" & PgNo
        FileWrite.WriteLine(strprint)
        line += 1
        str1 = LSet("PAYHEAD", 22)
        str2 = LSet("PARTICULAR", 35)
        str3 = LSet("TRANNO", 8)
        str4 = RSet("DEBIT", 12)
        str5 = RSet("CREDIT", 12)
        str6 = RSet("DEBIT", 12)
        str7 = RSet("CREDIT", 12)
        str8 = LSet("JOURNAL ENTRIES", 20)

        strprint = Space(81) + str8
        FileWrite.WriteLine(strprint)
        line += 1
        strprint = str1 + Space(2) + str3 + Space(5) + str2 + Space(1) + str4 + Space(1) + str5 + Space(1) + str6 + Space(1) + str7
        FileWrite.WriteLine(strprint)
        line += 1
        strprint = "----------------------------------------------------------------------------------------------------------------------------"
        FileWrite.WriteLine(Trim(strprint)) : line += 1

    End Function
    Public Sub AutoResize()
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                ResizeToolStripMenuItem.Checked = False
            Else
                ResizeToolStripMenuItem.Checked = True
            End If
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
End Class