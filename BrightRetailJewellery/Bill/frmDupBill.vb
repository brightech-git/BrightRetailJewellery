Imports System.Data.OleDb
Public Class frmDupBill
    Dim strsql As String
    Dim Iscurrdate As Boolean = IIf(GetAdmindbSoftValue("VIEW_CURDATE") = "Y", True, False)
    Dim Authorize As Boolean = False

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim type As String = Nothing
        Dim mbatchno As String
        'CALNO 230113
        Select Case cmbEntryType.Text
            Case "SALES", "MISC ISSUE", "APPROVAL ISSUE", "SALES RETURN", "PURCHASE", "APPROVAL RECEIPT", "ORDER DELIVERY", "REPAIR DELIVERY", "GIFT VOUCHER"
                type = "POS"
            Case "ORDER BOOKING"
                type = "ORD"
            Case "REPAIR BOOKING"
                type = "REP"
            Case Else
                MsgBox("Invalid Entry type", MsgBoxStyle.Information)
                Exit Sub
                Exit Select
        End Select

        Select Case cmbEntryType.Text
            Case "SALES", "MISC ISSUE", "APPROVAL ISSUE"
                strsql = " SELECT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE='" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' AND TRANNO='" & txtBillNo.Text & "'"
            Case "PURCHASE", "SALES RETURN", "APPROVAL RECEIPT"
                strsql = " SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE  TRANDATE='" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' AND  TRANNO='" & txtBillNo.Text & "'"
            Case "PAYMENTS", "RECEIPTS", "GIFT VOUCHER"
                strsql = " SELECT BATCHNO FROM " & cnStockDb & "..ACCTRAN WHERE  TRANDATE='" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' AND TRANNO='" & txtBillNo.Text & "'"
            Case "ORDER BOOKING", "REPAIR BOOKING", "ORDER DELIVERY", "REPAIR DELIVERY"
                strsql = " SELECT BATCHNO FROM " & cnadmindb & "..ORMAST WHERE ORDATE='" & dtpBillDate.Value.ToString("yyyy-MM-dd") & "' AND SUBSTRING(ORNO,6,LEN(ORNO))='" & txtBillNo.Text & "'"
            Case Else
                MsgBox("Invalid Entry type", MsgBoxStyle.Information)
                Exit Sub
                Exit Select
        End Select
        mbatchno = GetSqlValue(cn, strsql)
        If mbatchno Is Nothing Then Exit Sub

        Dim prnmemsuffix As String = ""
        If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
        If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
            Dim write As IO.StreamWriter
            Dim memfile As String = "\BillPrint" & prnmemsuffix.Trim & ".mem"
            write = IO.File.CreateText(Application.StartupPath & memfile)

            write.WriteLine(LSet("TYPE", 15) & ":" & type & "")
            write.WriteLine(LSet("BATCHNO", 15) & ":" & mbatchno)
            write.WriteLine(LSet("TRANDATE", 15) & ":" & dtpBillDate.Value.ToString("yyyy-MM-dd"))
            write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
            write.Flush()
            write.Close()
            If EXE_WITH_PARAM = False Then
                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
            Else
                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                LSet("TYPE", 15) & ":" & type & ";" & _
                LSet("BATCHNO", 15) & ":" & mbatchno & ";" & _
                LSet("TRANDATE", 15) & ":" & dtpBillDate.Value.ToString("yyyy-MM-dd") & ";" & _
                LSet("DUPLICATE", 15) & ":Y")
            End If
        Else
            MsgBox("Billprint exe not found", MsgBoxStyle.Information)
        End If

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmDupBill_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        ElseIf e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

   
    Private Sub frmDupBill_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtpBillDate.Value = GetServerDate()
        Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, "frmBillView", BrighttechPack.Methods.RightMode.Authorize, False)
        If Iscurrdate And Not Authorize Then dtpBillDate.Enabled = False Else dtpBillDate.Enabled = True
    End Sub
End Class