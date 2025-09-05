Imports System.Data.OleDb
Imports System.Math
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json.Schema
Imports System.IO
Imports System.Globalization
Public Class frmEinvoiceImportExcel
    Dim acCode As String = ""
    Private Cmd As OleDbCommand
    Private Da As OleDbDataAdapter
    Private DtTran As New DataTable
    'Private dtGridStone As New DataTable
    Private StrSql As String
    Dim BatchNo As String = Nothing
    Dim ukculinfo As New CultureInfo("en-GB")


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        Initializer()
    End Sub

    Private Sub Initializer()
        With DtTran
            .Columns.Add("SNO", GetType(String))
            .Columns.Add("ACK NO", GetType(String))
            .Columns.Add("ACK DATE", GetType(String))
            .Columns.Add("DOC NO", GetType(String))
            .Columns.Add("DOC DATE", GetType(DateTime))
            .Columns.Add("DOC TYPE", GetType(String))
            .Columns.Add("INV VALUE", GetType(String))
            .Columns.Add("RECIPIENT GSTIN", GetType(String))
            .Columns.Add("STATUS", GetType(String))
            .Columns.Add("IRN", GetType(String))
            .Columns.Add("SIGNEDQRCODE", GetType(String))
            .Columns.Add("EWAY BILL NO", GetType(String))
            .Columns.Add("BATCHNO", GetType(String))
            .Columns.Add("DUPREC", GetType(Integer))
        End With
        DgvTran.Columns.Clear()
        DgvTran.DataSource = DtTran
        ClearTran()
        DgvTran.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 9, FontStyle.Bold)
        DgvTran.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DgvTran.Columns("BATCHNO").Visible = False
        DgvTran.Columns("DUPREC").Visible = False
    End Sub

    Private Sub ClearTran()
        DtTran.Rows.Clear()
        DtTran.AcceptChanges()
    End Sub

    Private Sub frmMaterialIssRecExcel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.BackColor = SystemColors.InactiveCaption
        objGPack.Validator_Object(Me)
        DgvTran.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        ClearTran()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim billgencatcode As String = ""
        Dim billcontrolid As String = ""
        'If Not CheckRateSave() Then MsgBox("Rate Should Not Empty", MsgBoxStyle.Information) : Exit Sub
        For cnt As Integer = 0 To DgvTran.RowCount - 1
            If DgvTran.Rows(cnt).Cells(4).Value.ToString = "" Then
                MsgBox("Must Enter StyleNo(TagNo) In All Rows")
                Exit Sub
            End If
        Next
        Try
            tran = Nothing
            tran = cn.BeginTransaction()
            For cnt As Integer = 0 To DgvTran.RowCount - 1
                If Val(DgvTran.Rows(cnt).Cells("DUPREC").Value.ToString) = 1 Then Continue For
                If DgvTran.Rows(cnt).Cells("BATCHNO").Value.ToString = "" Then
                    MsgBox("Batchno Empty ", MsgBoxStyle.Information)
                    Exit Sub
                End If
                If Val(DgvTran.Rows(cnt).Cells("DUPREC").Value.ToString) = 2 Then
                    CancelSave(cnt)
                Else
                    Save(cnt)
                End If

            Next
            tran.Commit()
            tran = Nothing
            Dim msgdesc As String = "Imported "
            MsgBox(msgdesc, MsgBoxStyle.Information)

            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            DgvTran.DataSource = Nothing
            DgvTran.Refresh()
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub

    Private Sub Save(ByVal index As Integer)
        With DgvTran.Rows(index)
            StrSql = " INSERT INTO " & cnStockDb & "..EINVTRAN"
            StrSql += " (BATCHNO,IRN,QRDATA,ACKNO,ACKDATE,CANCELDATE)"
            StrSql += " VALUES("
            StrSql += " '" & .Cells("BATCHNO").Value.ToString & "'"
            StrSql += " ,'" & .Cells("IRN").Value.ToString & "'"
            StrSql += " ,'" & .Cells("SIGNEDQRCODE").Value.ToString & "'"
            StrSql += " ,'" & .Cells("ACK NO").Value.ToString & "'"
            StrSql += " ,'" & .Cells("ACK DATE").Value.ToString & "'"
            StrSql += " ,NULL"
            StrSql += " )"
            ExecQuery(SyncMode.Transaction, StrSql, cn, tran)
        End With
    End Sub
    Private Sub CancelSave(ByVal index As Integer)
        With DgvTran.Rows(index)
            StrSql = " UPDATE " & cnStockDb & "..EINVTRAN SET "
            StrSql += " CANCELDATE='" & .Cells("DOC DATE").Value.ToString & "'"
            StrSql += " WHERE IRN='" & .Cells("IRN").Value.ToString & "'"
            ExecQuery(SyncMode.Transaction, StrSql, cn, tran)
        End With
    End Sub

    Private Sub BtnExcelDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExcelDownload.Click
        Dim OpenDialog As New OpenFileDialog
        Dim Str As String = ""
        Str = "(*.xls) | *.xls"
        Str += "|(*.xlsx) | *.xlsx"
        'Str = "(*.json) | *.json"
        OpenDialog.Filter = Str
        If OpenDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim path As String = OpenDialog.FileName
            If path <> "" Then
                LoadFromExel(path)
                'LoadFromJson(path)
                For j As Integer = 0 To DgvTran.Rows.Count - 1
                    If Val(DgvTran.Rows(j).Cells("DUPREC").Value.ToString) = 1 Then
                        DgvTran.Rows(j).DefaultCellStyle.BackColor = Color.Red
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub btnCancelUpload_Click(sender As Object, e As EventArgs) Handles btnCancelUpload.Click
        Dim OpenDialog As New OpenFileDialog
        Dim Str As String
        Str = "(*.xls) | *.xls"
        Str += "|(*.xlsx) | *.xlsx"
        OpenDialog.Filter = Str
        If OpenDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim path As String = OpenDialog.FileName
            If path <> "" Then
                LoadFromExelCancel(path)
            End If
        End If
    End Sub

    Private Sub LoadFromJson(ByVal Path As String)
        'Try
        '    Dim jstr As String = File.ReadAllText(Path)
        '    'DgvTran.Columns("DOC DATE").HeaderText = "DOC DATE"
        '    Dim dtjson As New DataTable
        '    DgvTran.DataSource = Nothing
        '    Dim Jobj = JObject.Parse(jstr)
        '    For Each j As JProperty In Jobj
        '        Dim dtrow As DataRow = Nothing

        '        dtrow = DtTran.NewRow
        '        dtrow(j.Name) = j.Value
        '        DtTran.Rows.Add(dtrow)
        '        DtTran.AcceptChanges()
        '    Next
        'Catch ex As Exception
        '    DtTran.Rows.Clear()
        '    DtTran.AcceptChanges()
        '    MsgBox(ex.Message, MsgBoxStyle.Information)
        'End Try
    End Sub
    Private Sub LoadFromExel(ByVal Path As String)
        Try
            DtTran.Rows.Clear()
            DtTran.AcceptChanges()
            Dim MyConnection As System.Data.OleDb.OleDbConnection
            'MyConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" & Path & "';Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=1;""")
            MyConnection = New OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & Path & "';Extended Properties=Excel 8.0;")
            Dim myExcel As Excel.Application
            Dim myWorkBook As Excel.Workbook
            Dim tworksheet As Excel.Worksheet
            myExcel = CreateObject("Excel.Application")
            myWorkBook = myExcel.Workbooks.Open(Path)
            Dim cmbSheetName As New ComboBox
            cmbSheetName.Items.Clear()
            For Each tworksheet In myWorkBook.Worksheets
                cmbSheetName.Items.Add(tworksheet.Name)
            Next
            If cmbSheetName.Items.Count > 0 Then
                cmbSheetName.Text = cmbSheetName.Items.Item(0)
            End If
            If MyConnection.State = ConnectionState.Closed Then MyConnection.Open()
            StrSql = "SELECT * FROM [" & cmbSheetName.Text & "$]"
            Da = New OleDbDataAdapter(StrSql, MyConnection)
            Dim Dt As New DataTable
            Da.Fill(Dt)
            MyConnection.Close()

            Dim chkcostcentre As Boolean = False
            StrSql = "SELECT COUNT(*) CNT FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE ISNULL(ACTIVE,'')<>'N'"
            If Val(GetSqlValue(cn, StrSql).ToString) > 0 Then
                chkcostcentre = True
            End If

            If Dt.Rows.Count > 0 Then
                For i As Integer = 0 To Dt.Rows.Count - 1
                    Dim dtrow As DataRow = Nothing
                    dtrow = DtTran.NewRow
                    dtrow("SNO") = Val(Dt.Rows(i).Item("Sl# No").ToString)
                    dtrow("ACK NO") = Dt.Rows(i).Item("Ack No").ToString
                    Dim tempackdate As Date = Date.Parse(Dt.Rows(i).Item("Ack Date").ToString, ukculinfo.DateTimeFormat)
                    Dim datttACK As String = tempackdate.ToString("yyyy-MM-dd")
                    dtrow("ACK DATE") = datttACK.ToString
                    ''dtrow("ACK DATE") = Dt.Rows(i).Item("Ack Date").ToString
                    Dim docno() As String = Dt.Rows(i).Item("Doc No").ToString.Split("/")
                    dtrow("DOC NO") = Dt.Rows(i).Item("Doc No").ToString
                    Dim docdate As String = Dt.Rows(i).Item("Doc Date").ToString
                    ''Dim tempdate As Date = docdate
                    Dim tempdate As Date = Date.Parse(docdate, ukculinfo.DateTimeFormat)
                    Dim dattt As String = tempdate.ToString("yyyy-MM-dd")
                    dtrow("DOC DATE") = dattt.ToString
                    dtrow("DOC TYPE") = Dt.Rows(i).Item("Doc Type").ToString
                    dtrow("INV VALUE") = Dt.Rows(i).Item("Inv Value#").ToString
                    dtrow("RECIPIENT GSTIN") = Dt.Rows(i).Item("Recipient GSTIN").ToString
                    dtrow("STATUS") = Dt.Rows(i).Item("Status").ToString
                    dtrow("IRN") = Dt.Rows(i).Item("IRN").ToString
                    dtrow("SIGNEDQRCODE") = Dt.Rows(i).Item("SignedQRCode").ToString
                    dtrow("EWAY BILL NO") = ""
                    StrSql = " SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE "
                    StrSql += vbCrLf + " TRANDATE='" & dattt.ToString & "'"
                    StrSql += vbCrLf + " AND TRANNO='" & docno(2).ToString & "' AND TRANTYPE='" & docno(1).ToString & "'"
                    If chkcostcentre = True And docno(0).ToString <> strCompanyId.ToString Then
                        StrSql += vbCrLf + " AND COSTID='" & docno(0).ToString & "'"
                    End If
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE "
                    StrSql += vbCrLf + " TRANDATE='" & dattt.ToString & "'"
                    StrSql += vbCrLf + " AND TRANNO='" & docno(2).ToString & "' AND TRANTYPE='" & docno(1).ToString & "'"
                    If chkcostcentre = True And docno(0).ToString <> strCompanyId.ToString Then
                        StrSql += vbCrLf + " AND COSTID='" & docno(0).ToString & "'"
                    End If
                    Dim batchno As String = GetSqlValue(cn, StrSql)
                    dtrow("BATCHNO") = batchno.ToString
                    StrSql = " SELECT COUNT(*) CNT FROM " & cnStockDb & "..EINVTRAN WHERE BATCHNO='" & batchno.ToString & "'"
                    dtrow("DUPREC") = IIf(Val(GetSqlValue(cn, StrSql)) = 0, 0, 1)
                    DtTran.Rows.Add(dtrow)
                    DtTran.AcceptChanges()
                Next
            End If
            DgvTran.Columns("DOC DATE").HeaderText = "DOC DATE"
        Catch ex As Exception
            DtTran.Rows.Clear()
            DtTran.AcceptChanges()
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
    Private Sub LoadFromExelCancel(ByVal Path As String)
        Try
            DtTran.Rows.Clear()
            DtTran.AcceptChanges()
            Dim MyConnection As System.Data.OleDb.OleDbConnection
            'MyConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" & Path & "';Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=1;""")
            MyConnection = New OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & Path & "';Extended Properties=Excel 8.0;")
            Dim myExcel As Excel.Application
            Dim myWorkBook As Excel.Workbook
            Dim tworksheet As Excel.Worksheet
            myExcel = CreateObject("Excel.Application")
            myWorkBook = myExcel.Workbooks.Open(Path)
            Dim cmbSheetName As New ComboBox
            cmbSheetName.Items.Clear()
            For Each tworksheet In myWorkBook.Worksheets
                cmbSheetName.Items.Add(tworksheet.Name)
            Next
            If cmbSheetName.Items.Count > 0 Then
                cmbSheetName.Text = cmbSheetName.Items.Item(0)
            End If
            If MyConnection.State = ConnectionState.Closed Then MyConnection.Open()
            StrSql = "SELECT * FROM [" & cmbSheetName.Text & "$]"
            Da = New OleDbDataAdapter(StrSql, MyConnection)
            Dim Dt As New DataTable
            Da.Fill(Dt)
            MyConnection.Close()
            If Dt.Rows.Count > 0 Then
                For i As Integer = 0 To Dt.Rows.Count - 1
                    Dim dtrow As DataRow = Nothing
                    dtrow = DtTran.NewRow
                    dtrow("SNO") = Val(Dt.Rows(i).Item("Sl# No").ToString)
                    dtrow("IRN") = Dt.Rows(i).Item("IRN").ToString
                    Dim Candate() As String = Dt.Rows(i).Item("Cancelled Date").ToString.Split(" ")
                    Dim docdate() As String = Candate(0).ToString.Split("/")
                    dtrow("DOC DATE") = docdate(2).ToString & "-" & docdate(1).ToString & "-" & docdate(0).ToString
                    dtrow("DUPREC") = 2
                    DtTran.Rows.Add(dtrow)
                    DtTran.AcceptChanges()
                Next
            End If
        Catch ex As Exception
            DtTran.Rows.Clear()
            DtTran.AcceptChanges()
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
End Class
