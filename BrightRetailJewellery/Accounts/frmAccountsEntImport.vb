Imports System.Data.OleDb
Imports System.Math
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json.Schema
Imports System.IO
Imports System.Globalization
Public Class frmAccountsEntImport
    Dim acCode As String = ""
    Private Cmd As OleDbCommand
    Private Da As OleDbDataAdapter
    Private DtTran As New DataTable
    'Private dtGridStone As New DataTable
    Private StrSql As String
    Dim BatchNo As String = Nothing
    Dim ukculinfo As New CultureInfo("en-GB")
    Dim tranno As Integer = 0


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
            .Columns.Add("DocDate", GetType(String))
            .Columns.Add("Debit Accountcode", GetType(String))
            .Columns.Add("Debit AccountName", GetType(String))
            .Columns.Add("Credit Accountcode", GetType(String))
            .Columns.Add("Credit AccountName", GetType(String))
            .Columns.Add("Amount", GetType(String))
            .Columns.Add("Billno", GetType(String))
            .Columns.Add("BillDate", GetType(String))
            .Columns.Add("Cheque No", GetType(String))
            .Columns.Add("Cheque Date", GetType(String))
            .Columns.Add("Error Msg", GetType(String))
            .Columns.Add("Remark1", GetType(String))
            .Columns.Add("Remark2", GetType(String))
            .Columns.Add("Voucher No", GetType(String))
            .Columns.Add("voucher type", GetType(String))
            .Columns.Add("TDS Accountcode", GetType(String))
            .Columns.Add("TDS AccountName", GetType(String))
            .Columns.Add("TDS Percent", GetType(String))
            .Columns.Add("TDS Id", GetType(String))
            .Columns.Add("Actual Value", GetType(String))
        End With
        DgvTran.Columns.Clear()
        DgvTran.DataSource = DtTran
        ClearTran()
        DgvTran.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 9, FontStyle.Bold)
        DgvTran.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
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
        BtnExcelDownload.Enabled = True
        objGPack.TextClear(Me)
        ClearTran()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim billgencatcode As String = ""
        Dim billcontrolid As String = ""
        Try
            DtTran.AcceptChanges()
            Dim dttemp As New DataTable
            dttemp = DgvTran.DataSource
            Dim tempcnt As Integer = Val(dttemp.Select("[Error Msg]='Imported'").Length)
            If Val(tempcnt) > 0 Then
                MsgBox("Records Already Saved", MsgBoxStyle.Information)
                Exit Sub
            End If
            tempcnt = Val(dttemp.Select("[Error Msg]=''").Length)
            If Val(tempcnt) = 0 Then
                MsgBox("No Records to save", MsgBoxStyle.Information)
                Exit Sub
            End If
            For cnt As Integer = 0 To DgvTran.RowCount - 1
                If DgvTran.Rows(cnt).Cells("Error Msg").Value.ToString <> "" Then Continue For
                Save(cnt)
            Next
            Dim msgdesc As String = "Imported "
            MsgBox(msgdesc, MsgBoxStyle.Information)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            DgvTran.DataSource = Nothing
            DgvTran.Refresh()
        Finally
        End Try
    End Sub


    Private Sub BtnExcelDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExcelDownload.Click
        BtnExcelDownload.Enabled = False
        Dim OpenDialog As New OpenFileDialog
        Dim Str As String = ""
        Str = "(*.xlsx) | *.xlsx"
        Str += "|(*.xls) | *.xls"
        'Str = "(*.json) | *.json"
        OpenDialog.Filter = Str
        If OpenDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim path As String = OpenDialog.FileName
            If path <> "" Then
                LoadFromExel(path)
                If DgvTran.Rows.Count > 0 Then
                    DgvTran.DefaultCellStyle.Font = New Font("VERDANA", 8)
                    If DgvTran.Columns.Contains("Remark1") Then DgvTran.Columns("Remark1").Width = 250
                    If DgvTran.Columns.Contains("Remark2") Then DgvTran.Columns("Remark2").Width = 250
                    If DgvTran.Columns.Contains("Error Msg") Then DgvTran.Columns("Error Msg").Width = 250
                End If
                For j As Integer = 0 To DgvTran.Rows.Count - 1
                    If DgvTran.Rows(j).Cells("Error Msg").Value.ToString <> "" Then
                        DgvTran.Rows(j).DefaultCellStyle.BackColor = Color.Red
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub LoadFromExel(ByVal Path As String)
        Try
            DtTran.Rows.Clear()
            DtTran.AcceptChanges()
            Dim MyConnection As System.Data.OleDb.OleDbConnection
            'MyConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.11.0;Data Source='" & Path & "';Extended Properties=""Excel 11.0 Xml;HDR=YES;IMEX=1;""")
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
            If myWorkBook.Worksheets.Count > 0 Then myWorkBook.Close()
            MyConnection.Close()
            myExcel.Quit()

            Dim chkcostcentre As Boolean = False
            StrSql = "SELECT COUNT(*) CNT FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE ISNULL(ACTIVE,'')<>'N'"
            If Val(GetSqlValue(cn, StrSql).ToString) > 0 Then
                chkcostcentre = True
            End If

            If Dt.Rows.Count > 0 Then
                For i As Integer = 0 To Dt.Rows.Count - 1
                    If Dt.Rows(i).Item("DocDate").ToString = "" _
                        And Dt.Rows(i).Item("Debit Accountcode").ToString = "" _
                        And Dt.Rows(i).Item("Credit Accountcode").ToString = "" _
                        And Val(Dt.Rows(i).Item("Amount").ToString) <= 0 Then
                        Exit Sub
                    End If

                    Dim tempmsg As String = ""
                    Dim _TDSAccode As String = ""
                    Dim dtrow As DataRow = Nothing
                    dtrow = DtTran.NewRow
                    If Dt.Rows(i).Item("DocDate").ToString = "" Then
                        tempmsg += "Document date should not be empty" & ","
                    End If
                    Dim docdate As String = Dt.Rows(i).Item("DocDate").ToString
                    Dim tempdate As Date = Nothing
                    Dim dattt As String = ""
                    Dim _Temptrandate As String = ""
                    If Dt.Rows(i).Item("DocDate").ToString <> "" Then
                        tempdate = Date.Parse(docdate, ukculinfo.DateTimeFormat)
                        dattt = tempdate.ToString("yyyy-MM-dd")
                        _Temptrandate = tempdate.ToString("yyyy-MM-dd")
                    Else
                        dattt = ""
                    End If
                    StrSql = " SELECT COUNT(*)CNT FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME='" & cnStockDb.ToString & "' AND "
                    StrSql += " '" & dattt.ToString & "' BETWEEN STARTDATE AND ENDDATE"
                    If objGPack.GetSqlValue(StrSql, "CNT", 0) = 0 Then
                        tempmsg += "Invalid Entry Date" & ","
                    End If
                    dtrow("DocDate") = dattt.ToString
                    If Dt.Rows(i).Item("Debit Accountcode").ToString = "" Then
                        tempmsg += "Debit Accountcode should not be empty" & ","
                    End If
                    StrSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & Dt.Rows(i).Item("Debit Accountcode").ToString & "'"
                    If objGPack.GetSqlValue(StrSql, "CNT", 0) = 0 Then
                        tempmsg += "Debit Accountcode Invalid " & ","
                    ElseIf objGPack.GetSqlValue(StrSql, "CNT", 0) <> 0 And Dt.Rows(i).Item("Debit Accountcode").ToString <> "" Then
                        StrSql = "SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & Dt.Rows(i).Item("Debit Accountcode").ToString & "'"
                        dtrow("Debit AccountName") = IIf(objGPack.GetSqlValue(StrSql, "ACNAME", "").ToString <> "", objGPack.GetSqlValue(StrSql, "ACNAME", "").ToString, "")
                    End If
                    StrSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..TDSCATEGORY WHERE ACCODE='" & Dt.Rows(i).Item("Debit Accountcode").ToString & "'"
                    If objGPack.GetSqlValue(StrSql, "CNT", 0) <> 0 Then
                        _TDSAccode = Dt.Rows(i).Item("Debit Accountcode").ToString
                    End If
                    dtrow("Debit Accountcode") = Dt.Rows(i).Item("Debit Accountcode").ToString
                    If Dt.Rows(i).Item("Credit Accountcode").ToString = "" Then
                        tempmsg += "Credit Accountcode should not be empty" & ","
                    End If
                    StrSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & Dt.Rows(i).Item("Credit Accountcode").ToString & "'"
                    If objGPack.GetSqlValue(StrSql, "CNT", 0) = 0 Then
                        tempmsg += "Credit Accountcode Invalid " & ","
                    ElseIf objGPack.GetSqlValue(StrSql, "CNT", 0) <> 0 And Dt.Rows(i).Item("Credit Accountcode").ToString <> "" Then
                        StrSql = "SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & Dt.Rows(i).Item("Credit Accountcode").ToString & "'"
                        dtrow("Credit AccountName") = IIf(objGPack.GetSqlValue(StrSql, "ACNAME", "").ToString <> "", objGPack.GetSqlValue(StrSql, "ACNAME", "").ToString, "")
                    End If
                    StrSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..TDSCATEGORY WHERE ACCODE='" & Dt.Rows(i).Item("Credit Accountcode").ToString & "'"
                    If objGPack.GetSqlValue(StrSql, "CNT", 0) <> 0 Then
                        _TDSAccode = Dt.Rows(i).Item("Credit Accountcode").ToString
                    End If
                    dtrow("Credit Accountcode") = Dt.Rows(i).Item("Credit Accountcode").ToString
                    If Val(Dt.Rows(i).Item("Amount").ToString) <= 0 Then
                        tempmsg += "Amount should not be EMPTY" & ","
                    End If
                    dtrow("Amount") = Val(Dt.Rows(i).Item("Amount").ToString)
                    dtrow("Billno") = Dt.Rows(i).Item("Billno").ToString
                    docdate = Dt.Rows(i).Item("BillDate").ToString
                    If Dt.Rows(i).Item("BillDate").ToString <> "" Then
                        tempdate = Date.Parse(docdate, ukculinfo.DateTimeFormat)
                        dattt = tempdate.ToString("yyyy-MM-dd")
                    Else
                        dattt = ""
                    End If
                    dtrow("BillDate") = dattt.ToString
                    dtrow("Cheque No") = Dt.Rows(i).Item("Cheque No").ToString
                    docdate = Dt.Rows(i).Item("Cheque Date").ToString
                    If Dt.Rows(i).Item("Cheque Date").ToString <> "" Then
                        tempdate = Date.Parse(docdate, ukculinfo.DateTimeFormat)
                        dattt = tempdate.ToString("yyyy-MM-dd")
                    Else
                        dattt = ""
                    End If
                    dtrow("Cheque Date") = dattt.ToString
                    If Dt.Rows(i).Item("Remark1").ToString.Length > 100 Then
                        dtrow("Remark1") = Dt.Rows(i).Item("Remark1").ToString.Substring(0, 99)
                    Else
                        dtrow("Remark1") = Dt.Rows(i).Item("Remark1").ToString
                    End If
                    If Dt.Rows(i).Item("Remark2").ToString.Length > 100 Then
                        dtrow("Remark2") = Dt.Rows(i).Item("Remark2").ToString.Substring(0, 99)
                    Else
                        dtrow("Remark2") = Dt.Rows(i).Item("Remark2").ToString
                    End If
                    dtrow("Voucher No") = Dt.Rows(i).Item("Voucher No").ToString
                    If Dt.Rows(i).Item("Voucher type").ToString = "" Then
                        tempmsg += "Voucher type should not be empty" & ","
                    End If
                    dtrow("Voucher type") = Dt.Rows(i).Item("Voucher type").ToString
                    Dim paymode As String = ""
                    StrSql = "SELECT paymode FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE PAYMODE LIKE '" & Dt.Rows(i).Item("Voucher type").ToString.ToUpper & "%'"
                    paymode = objGPack.GetSqlValue(StrSql, "PAYMODE", "").ToString.ToUpper
                    If paymode.ToString = "" Then
                        If Dt.Rows(i).Item("Voucher type").ToString.ToUpper.Contains("PAYMENT") Then
                            paymode = "CP"
                        ElseIf Dt.Rows(i).Item("Voucher type").ToString.ToUpper.Contains("RECEIPT") Then
                            paymode = "CR"
                        ElseIf Dt.Rows(i).Item("Voucher type").ToString.ToUpper.Contains("JOURNAL") Then
                            paymode = "JE"
                        ElseIf Dt.Rows(i).Item("Voucher type").ToString.ToUpper.Contains("DEBIT") Then
                            paymode = "DN"
                        ElseIf Dt.Rows(i).Item("Voucher type").ToString.ToUpper.Contains("CREDIT") Then
                            paymode = "CN"
                        End If
                    End If
                    If paymode.ToString = "" Then
                        tempmsg += "Voucher type MisMatch" & ","
                    End If
                    If paymode.ToString <> "" _
                        And _Temptrandate.ToString <> "" _
                        And Dt.Rows(i).Item("Amount").ToString <> "" _
                        And Dt.Rows(i).Item("Debit Accountcode").ToString <> "" _
                        And Dt.Rows(i).Item("Credit Accountcode").ToString <> "" _
                        Then
                        StrSql = " SELECT COUNT(*)CNT FROM " & cnStockDb & "..ACCTRAN WHERE "
                        StrSql += " TRANDATE='" & _Temptrandate.ToString & "' "
                        StrSql += " And ACCODE ='" & Dt.Rows(i).Item("Debit Accountcode").ToString & "' "
                        StrSql += " And CONTRA ='" & Dt.Rows(i).Item("Credit Accountcode").ToString & "' "
                        StrSql += " And PAYMODE='" & paymode.ToString & "' AND AMOUNT='" & Dt.Rows(i).Item("Amount").ToString & "' "
                        If Dt.Rows(i).Item("Cheque No").ToString <> "" Then StrSql += " And CHQCARDNO ='" & Dt.Rows(i).Item("Cheque No").ToString & "'"
                        If Val(objGPack.GetSqlValue(StrSql, "CNT", 0).ToString) > 0 Then
                            tempmsg = "Already Entry Saved ,"
                        End If
                    End If
                    If _TDSAccode.ToString <> "" Then
                        StrSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & _TDSAccode.ToString & "'"
                        If objGPack.GetSqlValue(StrSql, "CNT", 0) = 0 Then
                            tempmsg += "Debit Accountcode Invalid " & ","
                        ElseIf objGPack.GetSqlValue(StrSql, "CNT", 0) <> 0 And _TDSAccode.ToString <> "" Then
                            StrSql = "SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & _TDSAccode.ToString & "'"
                            dtrow("TDS AccountName") = IIf(objGPack.GetSqlValue(StrSql, "ACNAME", "").ToString <> "", objGPack.GetSqlValue(StrSql, "ACNAME", "").ToString, "")
                            StrSql = "SELECT TOP 1 TDSPER FROM " & cnAdminDb & "..TDSCATEGORY WHERE ACCODE='" & _TDSAccode.ToString & "'"
                            dtrow("TDS Percent") = IIf(objGPack.GetSqlValue(StrSql, "TDSPER", "").ToString <> "", objGPack.GetSqlValue(StrSql, "TDSPER", "").ToString, "")
                            StrSql = "SELECT TOP 1 TDSCATID FROM " & cnAdminDb & "..TDSCATEGORY WHERE ACCODE='" & _TDSAccode.ToString & "'"
                            dtrow("TDS Id") = IIf(objGPack.GetSqlValue(StrSql, "TDSCATID", "").ToString <> "", objGPack.GetSqlValue(StrSql, "TDSCATID", "").ToString, "")
                            dtrow("TDS Accountcode") = _TDSAccode.ToString
                            dtrow("Actual Value") = Val(Dt.Rows(i).Item("Actual Value").ToString)
                        End If
                    End If


                    If tempmsg.ToString.Length > 0 Then
                        tempmsg = tempmsg.Substring(0, tempmsg.Length - 1).ToString
                    Else
                        tempmsg = ""
                    End If
                    dtrow("Error Msg") = tempmsg.ToString
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
    Private Sub btntemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btntemplate.Click
        exceltemplate()
    End Sub
    Function exceltemplate() As Integer
        Dim rwStart As Integer = 0
        Dim oXL As Excel.Application
        Dim oWB As Excel.Workbook
        Dim oSheet As Excel.Worksheet
        Dim oRng As Excel.Range

        oXL = CreateObject("Excel.Application")
        oXL.Visible = True
        oWB = oXL.Workbooks.Add
        oSheet = oWB.ActiveSheet

        oSheet.Range("A1").Value = "DocDate"
        oSheet.Range("A1").ColumnWidth = 10
        oSheet.Range("B1").Value = "Debit Accountcode"
        oSheet.Range("B1").ColumnWidth = 15
        oSheet.Range("C1").Value = "Credit Accountcode"
        oSheet.Range("C1").ColumnWidth = 15
        oSheet.Range("D1").Value = "Amount"
        oSheet.Range("D1").ColumnWidth = 15
        oSheet.Range("E1").Value = "Billno"
        oSheet.Range("E1").ColumnWidth = 10
        oSheet.Range("F1").Value = "BillDate"
        oSheet.Range("F1").ColumnWidth = 10
        oSheet.Range("G1").Value = "Cheque No"
        oSheet.Range("G1").ColumnWidth = 20
        oSheet.Range("H1").Value = "Cheque Date"
        oSheet.Range("H1").ColumnWidth = 10
        oSheet.Range("I1").Value = "Remark1"
        oSheet.Range("I1").ColumnWidth = 100
        oSheet.Range("J1").Value = "Remark2"
        oSheet.Range("J1").ColumnWidth = 100
        oSheet.Range("K1").Value = "Voucher No"
        oSheet.Range("K1").ColumnWidth = 10
        oSheet.Range("L1").Value = "voucher type"
        oSheet.Range("L1").ColumnWidth = 15
        oSheet.Range("M1").Value = "Actual Value"
        oSheet.Range("M1").ColumnWidth = 15

        oSheet.Range("A1:B1:C1:D1:E1:F1:G1:H1:I1:J1:K1:L1:M1").Font.Bold = False
        oSheet.Range("A1:B1:C1:D1:E1:F1:G1:H1:I1:J1:K1:L1:M1").Font.Name = "VERDANA"
        oSheet.Range("A1:B1:C1:D1:E1:F1:G1:H1:I1:J1:K1:L1:M1").Font.Size = 8
    End Function

    Private Sub Save(ByVal index As Integer)
        Try
            tran = Nothing
            tran = cn.BeginTransaction()
            With DgvTran.Rows(index)

                Dim ctrlId As String = ""
                Dim accode As String = ""
                Dim Contra As String = ""
                Dim payMode As String = ""
                Dim amt As Double = 0
                Dim _TDSamt As Double = 0
                Dim _TDSPartyAccode As String = ""
                Dim _TDSCatId As String = ""
                Dim _TDSpercent As String = ""
                Dim RefInvno As String = ""
                Dim Refinvdate As String = ""
                Dim _billdate As String = ""
                Dim sChqNo As String = ""
                Dim sChqDate As String = ""
                Dim sChqDetail As String = ""
                Dim remark1 As String = ""
                Dim remark2 As String = ""
                StrSql = "SELECT paymode FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE PAYMODE LIKE '" & .Cells("voucher type").Value.ToString.ToUpper & "%'"
                payMode = objGPack.GetSqlValue(StrSql, "PAYMODE", "", tran).ToString.ToUpper
                If payMode.ToString <> "" Then
                    ctrlId = "GEN-" & payMode
                Else
                    If .Cells("voucher type").Value.ToString.ToUpper.Contains("PAYMENT") Then
                        ctrlId = "GEN-CP"
                        payMode = "CP"
                    ElseIf .Cells("voucher type").Value.ToString.ToUpper.Contains("RECEIPT") Then
                        ctrlId = "GEN-CR"
                        payMode = "CR"
                    ElseIf .Cells("voucher type").Value.ToString.ToUpper.Contains("JOURNAL") Then
                        ctrlId = "GEN-JE"
                        payMode = "JE"
                    ElseIf .Cells("voucher type").Value.ToString.ToUpper.Contains("DEBIT") Then
                        ctrlId = "GEN-DN"
                        payMode = "DN"
                    ElseIf .Cells("voucher type").Value.ToString.ToUpper.Contains("CREDIT") Then
                        ctrlId = "GEN-CN"
                        payMode = "CN"
                    End If
                End If

                _billdate = .Cells("DocDate").Value.ToString
GenBillNo:
                tranno = Val(GetBillControlValue(ctrlId, tran, False))
                StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & tranno + 1 & "'"
                StrSql += vbCrLf + "WHERE CTLID = '" & ctrlId & "' AND COMPANYID = '" & strCompanyId & "'"
                StrSql += vbCrLf + "AND COSTID ='" & cnCostId & "'"
                StrSql += vbCrLf + "AND CONVERT(INT,CTLTEXT) = '" & tranno & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                If Not Cmd.ExecuteNonQuery() > 0 Then
                    If strBCostid <> Nothing Then MsgBox("Tran No. empty. Please check Bill control") : tran.Rollback() : tran.Dispose() : tran = Nothing : Exit Sub
                    GoTo GenBillNo
                End If
                tranno += 1
                BatchNo = GetNewBatchno(cnCostId, GetServerDate(tran), tran)
                If BatchNo = "" Then
                    tran.Rollback()
                    tran = Nothing
                    MsgBox("Batchno is empty", MsgBoxStyle.Information)
                    Exit Sub
                End If

                accode = .Cells("Debit Accountcode").Value.ToString
                Contra = .Cells("Credit Accountcode").Value.ToString
                amt = Val(.Cells("Amount").Value.ToString)
                _TDSamt = Val(.Cells("Actual Value").Value.ToString)
                _TDSCatId = Val(.Cells("TDS Id").Value.ToString)
                _TDSpercent = Val(.Cells("TDS PERCENT").Value.ToString)
                _TDSPartyAccode = IIf(.Cells("TDS Accountcode").Value.ToString <> accode, accode, Contra)
                RefInvno = .Cells("Billno").Value.ToString
                Refinvdate = IIf(.Cells("BillDate").Value.ToString <> "", .Cells("BillDate").Value.ToString, Nothing)
                sChqNo = .Cells("Cheque No").Value.ToString
                sChqDate = IIf(.Cells("Cheque Date").Value.ToString <> "", .Cells("Cheque Date").Value.ToString, Nothing) ''.Cells("Cheque Date").Value.ToString
                sChqDetail = ""
                remark1 = .Cells("Remark1").Value.ToString
                remark2 = .Cells("Remark2").Value.ToString

                InsertIntoAccTran(1, tranno, _billdate, "D" _
                        , accode, amt, 0, 0, 0, payMode, Contra _
                        , IIf(_TDSPartyAccode <> "" And _TDSPartyAccode = accode, _TDSCatId, 0) _
                        , IIf(_TDSPartyAccode <> "" And _TDSPartyAccode = accode, _TDSpercent, 0) _
                        , IIf(_TDSPartyAccode <> "" And _TDSPartyAccode = accode, _TDSamt, 0) _
                        , RefInvno, Refinvdate, sChqNo, sChqDate, , sChqDetail, remark1, remark2, "", , cnCostId)
                InsertIntoAccTran(2, tranno, _billdate, "C" _
                        , Contra, amt, 0, 0, 0, payMode, accode _
                        , IIf(_TDSPartyAccode <> "" And _TDSPartyAccode = Contra, _TDSCatId, 0) _
                        , IIf(_TDSPartyAccode <> "" And _TDSPartyAccode = Contra, _TDSpercent, 0) _
                        , IIf(_TDSPartyAccode <> "" And _TDSPartyAccode = Contra, _TDSamt, 0) _
                        , RefInvno, Refinvdate, sChqNo, sChqDate, , sChqDetail, remark1, remark2, "", , cnCostId)

                If _TDSamt <> 0 Then
                    StrSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
                    StrSql += vbCrLf + "("
                    StrSql += vbCrLf + "SNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
                    StrSql += vbCrLf + ",AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID,COMPANYID"
                    StrSql += vbCrLf + ")"
                    StrSql += vbCrLf + "VALUES("
                    StrSql += vbCrLf + "'" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
                    StrSql += vbCrLf + ",'" & _TDSPartyAccode & "'"
                    StrSql += vbCrLf + ",'" & _TDSPartyAccode & "'"
                    StrSql += vbCrLf + "," & tranno & "" 'TRANNO
                    StrSql += vbCrLf + ",'" & _billdate & "'" 'TRANDATE
                    StrSql += vbCrLf + ",'" & payMode & "'"
                    StrSql += vbCrLf + ",'" & BatchNo & "'" 'BATCHNO
                    StrSql += vbCrLf + ",'" & _TDSCatId & "'" 'TAXID
                    StrSql += vbCrLf + "," & Math.Round(_TDSamt, 2) & "" 'AMOUNT
                    StrSql += vbCrLf + "," & _TDSpercent & "" 'TAXPER
                    StrSql += vbCrLf + "," & amt
                    StrSql += vbCrLf + ",'TD'"
                    StrSql += vbCrLf + ",1" 'TSNO
                    StrSql += vbCrLf + ",'" & cnCostId & "'" 'COSTID
                    StrSql += vbCrLf + ",'" & strCompanyId & "'" 'COMPANYID
                    StrSql += vbCrLf + ")"
                    ExecQuery(SyncMode.Transaction, StrSql, cn, tran, cnCostId)
                End If

            End With
            tran.Commit()
            tran = Nothing
            DgvTran.Rows(index).Cells("voucher No").Value = tranno.ToString
            DgvTran.Rows(index).Cells("Error Msg").Value = "Imported"
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace, MsgBoxStyle.Information)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub

    Private Sub InsertIntoAccTran _
  (ByVal EntryOrder As Integer, ByVal tNo As Integer,
   ByVal billdate As String,
  ByVal tranMode As String,
  ByVal accode As String,
  ByVal amount As Double,
  ByVal pcs As Integer,
  ByVal grsWT As Double,
  ByVal netWT As Double,
  ByVal payMode As String,
  ByVal contra As String,
  ByVal TDSCATID As Integer,
  ByVal TDSPER As Decimal,
  ByVal TDSAMOUNT As Decimal,
  Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing,
  Optional ByVal chqCardNo As String = Nothing,
  Optional ByVal chqDate As String = Nothing,
  Optional ByVal chqCardId As Integer = Nothing,
  Optional ByVal chqCardRef As String = Nothing,
  Optional ByVal Remark1 As String = Nothing,
  Optional ByVal Remark2 As String = Nothing,
  Optional ByVal fLAG As String = Nothing,
  Optional ByVal SAccode As String = "",
  Optional ByVal SCostid As String = ""
  )
        If amount = 0 Then Exit Sub

        StrSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        StrSql += vbCrLf + "("
        StrSql += vbCrLf + "SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,SACCODE"
        StrSql += vbCrLf + ",AMOUNT,PCS,GRSWT,NETWT,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        StrSql += vbCrLf + ",CARDID,CHQCARDREF,CHQDATE,BRSFLAG,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        StrSql += vbCrLf + ",CONTRA,BATCHNO,USERID,UPDATED,UPTIME,SYSTEMID,CASHID,COSTID,SCOSTID"
        StrSql += vbCrLf + ",APPVER,COMPANYID,WT_ENTORDER,TDSCATID,TDSPER,TDSAMOUNT,FLAG"
        StrSql += vbCrLf + ")"
        StrSql += vbCrLf + "VALUES"
        StrSql += vbCrLf + "("
        StrSql += vbCrLf + "'" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        StrSql += vbCrLf + "," & tNo & "" 'TRANNO 
        StrSql += vbCrLf + ",'" & billdate & "'" 'TRANDATE
        StrSql += vbCrLf + ",'" & tranMode & "'" 'TRANMODE
        StrSql += vbCrLf + ",'" & accode & "'" 'ACCODE
        StrSql += vbCrLf + ",'" & SAccode & "'" 'ACCODE
        StrSql += vbCrLf + "," & Math.Abs(amount) & "" 'AMOUNT
        StrSql += vbCrLf + "," & Math.Abs(pcs) & "" 'PCS
        StrSql += vbCrLf + "," & Math.Abs(grsWT) & "" 'GRSWT
        StrSql += vbCrLf + "," & Math.Abs(netWT) & "" 'NETWT
        StrSql += vbCrLf + ",'" & refNo & "'" 'REFNO
        If refDate = Nothing Or refDate = "12:00:00 AM" Then
            StrSql += vbCrLf + ",NULL" 'REFDATE
        Else
            StrSql += vbCrLf + ",'" & refDate & "'" 'REFDATE
        End If
        StrSql += vbCrLf + ",'" & payMode & "'" 'PAYMODE
        StrSql += vbCrLf + ",'" & chqCardNo & "'" 'CHQCARDNO
        StrSql += vbCrLf + "," & chqCardId & "" 'CARDID
        StrSql += vbCrLf + ",'" & chqCardRef & "'" 'CHQCARDREF
        If chqDate = Nothing Then
            StrSql += vbCrLf + ",NULL" 'CHQDATE
        Else
            StrSql += vbCrLf + ",'" & chqDate & "'" 'CHQDATE
        End If
        StrSql += vbCrLf + ",''" 'BRSFLAG
        StrSql += vbCrLf + ",NULL" 'RELIASEDATE
        StrSql += vbCrLf + ",'A'" 'FROMFLAG
        StrSql += vbCrLf + ",'" & Mid(Remark1, 1, 100) & "'" 'REMARK1
        StrSql += vbCrLf + ",'" & Mid(Remark2, 1, 100) & "'" 'REMARK2
        StrSql += vbCrLf + ",'" & contra & "'" 'CONTRA
        StrSql += vbCrLf + ",'" & BatchNo & "'" 'BATCHNO
        StrSql += vbCrLf + ",'" & userId & "'" 'USERID
        StrSql += vbCrLf + ",'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        StrSql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        StrSql += vbCrLf + ",'" & systemId & "'" 'SYSTEMID
        StrSql += vbCrLf + ",''" 'CASHID
        StrSql += vbCrLf + ",'" & cnCostId & "'" 'COSTID
        StrSql += vbCrLf + ",'" & SCostid & "'" 'FLAG
        StrSql += vbCrLf + ",'" & VERSION & "'" 'APPVER
        StrSql += vbCrLf + ",'" & strCompanyId & "'" 'COMPANYID
        StrSql += vbCrLf + "," & EntryOrder & "" 'WT_ENTORDER
        StrSql += vbCrLf + "," & TDSCATID & "" 'TDSCATID
        StrSql += vbCrLf + "," & TDSPER & "" 'TDSPER
        StrSql += vbCrLf + "," & TDSAMOUNT & "" 'TDSAMOUNT
        StrSql += vbCrLf + ",'" & fLAG & "'" 'FLAG
        StrSql += vbCrLf + ")"
        ExecQuery(SyncMode.Transaction, StrSql, cn, tran, cnCostId)
        StrSql = ""
        Cmd = Nothing
    End Sub
End Class
